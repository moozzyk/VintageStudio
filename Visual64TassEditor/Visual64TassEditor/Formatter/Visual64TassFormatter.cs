//-----------------------------------------------------------------------
// <copyright file="Visual64TassClassificationFormat.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
//-----------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Visual64TassEditor.Parser;

namespace Visual64TassEditor.Formatter
{
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("Visual64Tass")]
    [TextViewRole("Document")]
    class Visual64TassTextViewCreationListener : IVsTextViewCreationListener
    {
        [Import]
        internal IVsEditorAdaptersFactoryService EditorAdaptersFactoryService { get; set; }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            Contract.Requires(textViewAdapter != null);
            Contract.Ensures(Contract.Result<Visual64TassCommandTarget>() != null);

            var wpfTextView = EditorAdaptersFactoryService.GetWpfTextView(textViewAdapter);

            Debug.Assert(wpfTextView != null, "Unable to get IWpfTextView from text view adapter");

            new Visual64TassCommandTarget(wpfTextView, textViewAdapter);
        }
    }

    class Visual64TassCommandTarget : IOleCommandTarget
    {
        const int CodeStartPosition = 24;
        const int CodeCommentStartPosition = CodeStartPosition; // comment placed over a line with code
        const int AfterCodeCommentStartPosition = 44;

        private IWpfTextView wpfTextView;
        private IOleCommandTarget nextCommandTarget;

        public Visual64TassCommandTarget(IWpfTextView wpfTextView, IVsTextView textViewAdapter)
        {
            Contract.Requires(wpfTextView != null);
            Contract.Requires(textViewAdapter != null);

            this.wpfTextView = wpfTextView;
            textViewAdapter.AddCommandFilter(this, out this.nextCommandTarget);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (pguidCmdGroup == typeof(VSConstants.VSStd2KCmdID).GUID)
            {
                VSConstants.VSStd2KCmdID command = (VSConstants.VSStd2KCmdID)nCmdID;
                if (command == VSConstants.VSStd2KCmdID.RETURN)
                {
                    if (this.nextCommandTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut) == VSConstants.S_OK)
                    {
                        FormatLine();
                        SetCaret();
                        return VSConstants.S_OK;
                    }
                }
                else if (command == VSConstants.VSStd2KCmdID.COMMENT_BLOCK)
                {
                    CommentSelection();
                    return VSConstants.S_OK;
                }
                else if (command == VSConstants.VSStd2KCmdID.UNCOMMENT_BLOCK)
                {
                    UncommentSelection();
                    return VSConstants.S_OK;
                }
                else if (command == VSConstants.VSStd2KCmdID.FORMATDOCUMENT)
                {
                    FormatCode();
                    return VSConstants.S_OK;
                }
            }
            return this.nextCommandTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (pguidCmdGroup == VSConstants.VSStd2K && cCmds > 0)
            {
                // enable commenting and formatting options in the menu
                if ((uint)prgCmds[0].cmdID == (uint)VSConstants.VSStd2KCmdID.FORMATDOCUMENT ||
                    (uint)prgCmds[0].cmdID == (uint)VSConstants.VSStd2KCmdID.COMMENT_BLOCK ||
                    (uint)prgCmds[0].cmdID == (uint)VSConstants.VSStd2KCmdID.UNCOMMENT_BLOCK)
                {
                    prgCmds[0].cmdf = (int)Microsoft.VisualStudio.OLE.Interop.Constants.MSOCMDF_ENABLED |
                                      (int)Microsoft.VisualStudio.OLE.Interop.Constants.MSOCMDF_SUPPORTED;

                    return VSConstants.S_OK;
                }
            }

            return this.nextCommandTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        private void RunSelectionAction(Func<ITextEdit, ITextSnapshotLine, bool> func)
        {
            Contract.Requires(func != null);

            ITextSnapshot snapshot = this.wpfTextView.TextSnapshot;

            if (snapshot != snapshot.TextBuffer.CurrentSnapshot)
            {
                return;
            }

            int selectionStartLine = snapshot.GetLineNumberFromPosition(this.wpfTextView.Caret.Position.BufferPosition);
            int selectionEndLine = selectionStartLine;

            if (!this.wpfTextView.Selection.IsEmpty)
            {
                selectionStartLine = this.wpfTextView.Selection.Start.Position.GetContainingLine().LineNumber;
                selectionEndLine = this.wpfTextView.Selection.End.Position.GetContainingLine().LineNumber;

                var selectionSpanText = this.wpfTextView.Selection.StreamSelectionSpan.GetText();
                if (selectionSpanText[selectionSpanText.Length - 1] == '\n')
                {
                    selectionEndLine--;
                }
            }

            if (!this.wpfTextView.Selection.IsEmpty)
            {
                selectionStartLine = this.wpfTextView.Selection.Start.Position.GetContainingLine().LineNumber;
                selectionEndLine = this.wpfTextView.Selection.End.Position.GetContainingLine().LineNumber;

                var selectionSpanText = this.wpfTextView.Selection.StreamSelectionSpan.GetText();
                if (selectionSpanText[selectionSpanText.Length - 1] == '\n')
                {
                    selectionEndLine--;
                }
            }

            using (ITextEdit edit = this.wpfTextView.TextBuffer.CreateEdit())
            {
                for (int currentLine = selectionStartLine; currentLine <= selectionEndLine; currentLine++)
                {
                    ITextSnapshotLine line = snapshot.GetLineFromLineNumber(currentLine);
                    if (!func(edit, line))
                    {
                        return;
                    }
                }

                edit.Apply();
            }            
        }

        #region Comment/Uncomment

        private void CommentSelection()
        {
            RunSelectionAction((edit, line) =>
                {
                    if (line.GetText().Any(c => !char.IsWhiteSpace(c)))
                    {
                        if (!edit.Insert(line.Start.Position, ";"))
                        {
                            return false;
                        }
                    }

                    return true;
                });
        }

        private void UncommentSelection()
        {
            RunSelectionAction((edit, line) =>
            {
                string currentLine = line.GetText();

                // try removing leading ';' for non-empty lines only
                if (currentLine.Length > 0)
                {
                    int pos = 0;
                    while (char.IsWhiteSpace(currentLine[pos]))
                        pos++;

                    if (currentLine[pos] == ';')
                    {
                        if (!edit.Delete(line.Start.Position + pos, 1))
                        {
                            return false;
                        }
                    }
                }

                return true;
            });
        }

        #endregion

        #region code formatting

        private void FormatCode()
        {
            ITextSnapshot snapshot = this.wpfTextView.TextSnapshot;

            if (snapshot != snapshot.TextBuffer.CurrentSnapshot)
            {
                return;
            }

            using (ITextEdit edit = this.wpfTextView.TextBuffer.CreateEdit())
            {
                var tokenizer = new Tokenizer();

                foreach (var line in edit.Snapshot.Lines)
                {
                    if (!edit.Replace(new Span(line.Start, line.End - line.Start), FormatLine(tokenizer, line.GetText())))
                    {
                        return;
                    }
                }

                edit.Apply();
            }
        }

        private void FormatLine()
        {
            ITextSnapshot snapshot = this.wpfTextView.TextSnapshot;

            if (snapshot != snapshot.TextBuffer.CurrentSnapshot)
            {
                return;
            }

            using (ITextEdit edit = this.wpfTextView.TextBuffer.CreateEdit())
            {

                var lineNum = snapshot.GetLineNumberFromPosition(this.wpfTextView.Caret.Position.BufferPosition);

                if (lineNum > 0)
                {
                    var line = snapshot.GetLineFromLineNumber(lineNum - 1);

                    if (!edit.Replace(new Span(line.Start, line.End - line.Start), FormatLine(new Tokenizer(), line.GetText())))
                    {
                        return;
                    }

                    edit.Apply();
                }
            }           
        }

        private void SetCaret()
        {
            var line = this.wpfTextView.TextSnapshot.GetLineFromPosition(this.wpfTextView.Caret.Position.BufferPosition.Position);
            var point = new VirtualSnapshotPoint(line, CodeStartPosition);
            this.wpfTextView.Caret.MoveTo(point);
        }

        private enum State { BeforeInstruction, BeforeInstructionArgument, InstructionArgument };

        // I wish it was cleaner...
        private static string FormatLine(Tokenizer tokenizer, string rawLine)
        {
            Contract.Requires(tokenizer != null);
            Contract.Requires(rawLine != null);
            Contract.Ensures(Contract.Result<string>() != null);

            var state = State.BeforeInstruction;
            var formattedLine = new StringBuilder();

            foreach (var token in tokenizer.Tokenize(rawLine))
            {
                switch (token.Type)
                {
                    case _64TassTokenType._6502Instruction:
                    case _64TassTokenType._6502IllegalInstruction:
                        if (state == State.BeforeInstruction)
                        {
                            PadToColumn(formattedLine, CodeStartPosition).Append(token.Value).Append(' ');
                            state = State.BeforeInstructionArgument;
                        }
                        else
                        {
                            formattedLine.Append(token.Value).Append(' ');
                        }
                        break;
                    case _64TassTokenType.Star:
                        if (state == State.BeforeInstructionArgument)
                        {
                            formattedLine.Append(token.Value);
                            state = State.InstructionArgument;
                        }
                        else if (state == State.InstructionArgument)
                        {
                            goto case _64TassTokenType.Equals;
                        }
                        else
                        {
                            PadToColumn(formattedLine, CodeStartPosition).Append(token.Value);
                            // change state for comment alignment
                            state = State.InstructionArgument;
                        }
                        break;
                    case _64TassTokenType.CompilerDirective:
                        PadToColumn(formattedLine, CodeStartPosition).Append(token.Value).Append(' ');
                        break;
                    case _64TassTokenType.Minus:
                    case _64TassTokenType.Plus:
                        Contract.Assert(token.Value == "+" || token.Value == "-");
                        if (state == State.BeforeInstructionArgument)
                        {
                            formattedLine.Append(token.Value);
                            state = State.InstructionArgument;
                        }                       
                        else if (state == State.InstructionArgument)
                        {
                            if (formattedLine[formattedLine.Length - 1] == '-')
                            {
                                formattedLine.Append(token.Value);
                            }
                            else
                            {
                                goto case _64TassTokenType.Equals;
                            }                            
                        }
                        else
                        {
                            formattedLine.Append(token.Value).Append(' ');
                        }
                        break;
                    case _64TassTokenType.Equals:
                        formattedLine.Append(' ').Append(token.Value).Append(' ');
                        break;
                    case _64TassTokenType.Comment:
                        if (state == State.BeforeInstructionArgument || state == State.InstructionArgument)
                        {
                            PadToColumn(formattedLine, AfterCodeCommentStartPosition).Append(token.Value).Append(' ');
                        }
                        else
                        {
                            // if the line starts with a comment leave it as it is
                            if (!(formattedLine.Length == 0 && rawLine.IndexOf(';') == 0))
                            {
                                PadToColumn(formattedLine, CodeCommentStartPosition);
                            }
                            formattedLine.Append(token.Value).Append(' ');
                        }
                        break;

                    case _64TassTokenType.BinaryNumber:
                        formattedLine.Append('%')
                            .Append('0', ((((token.Value.Length - 2) >> 3) + 1) << 3) - token.Value.Length + 1)
                            .Append(token.Value.Substring(1));
                        state = State.InstructionArgument;
                        break;
                    case _64TassTokenType.HexadecimalNumber:
                        if ((token.Value.Length & 0x01) == 0)
                        {
                            formattedLine.Append("$0").Append(token.Value.Substring(1));
                        }
                        else
                        {
                            formattedLine.Append(token.Value);
                        }
                        state = State.InstructionArgument;
                        break;
                    case _64TassTokenType.Label:
                        formattedLine.Append(token.Value);
                        if (state == State.BeforeInstructionArgument)
                        {
                            state = State.InstructionArgument;
                        }
                        break;
                    case _64TassTokenType.Hash:
                    case _64TassTokenType.StringLiteral:
                    case _64TassTokenType.DecimalNumber:
                    case _64TassTokenType.Coma:
                    case _64TassTokenType.LeftParen:
                    case _64TassTokenType.RightParen:
                    case _64TassTokenType.GreaterThan:
                    case _64TassTokenType.LessThan:
                    case _64TassTokenType.DontKnow:
                        formattedLine.Append(token.Value);
                        if (state == State.BeforeInstructionArgument)
                        {
                            state = State.InstructionArgument;
                        }
                        break;
                }
            }

            return formattedLine.ToString();
        }

        private static StringBuilder PadToColumn(StringBuilder line, int column)
        {
            Contract.Requires(line != null);
            Contract.Requires(column >= 0);
            Contract.Ensures(Contract.Result<StringBuilder>() != null);

            return line.Append(' ', Math.Max(column - line.Length, 1));
        }

        #endregion
    }
}