//-----------------------------------------------------------------------
// <copyright file="Visual64TassTokenTagger.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Visual64TassEditor.Parser;

namespace Visual64TassEditor.Tagger
{
    internal class Visual64TassTokenTagger : ITagger<Visual64TassTokenTag>
    {
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        public IEnumerable<ITagSpan<Visual64TassTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            var tokenizer = new Tokenizer();

            foreach (SnapshotSpan currentSpan in spans)
            {
                ITextSnapshotLine containingLine = currentSpan.Start.GetContainingLine();
                int lineStartPos = containingLine.Start.Position;

                foreach (var token in tokenizer.Tokenize(containingLine.GetText()))
                {
                    var tokenSpan = new SnapshotSpan(currentSpan.Snapshot, new Span(lineStartPos + token.StartPosition, token.Value.Length));
                    if (tokenSpan.IntersectsWith(currentSpan))
                    {
                        yield return new TagSpan<Visual64TassTokenTag>(tokenSpan, new Visual64TassTokenTag(token));
                    }
                }
            }
        }
    }
}