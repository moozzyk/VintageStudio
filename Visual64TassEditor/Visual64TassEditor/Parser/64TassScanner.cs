//-----------------------------------------------------------------------
// <copyright file="64TassScanner.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Visual64TassEditor.Parser
{
    internal class _64TassScanner
    {
        private string input;
        private int currentPos;

        private bool IsEOF
        {
            get { return this.currentPos >= this.input.Length; }
        }

        private char CurrentChar
        {
            get
            {
                Contract.Requires(!this.IsEOF);

                return this.input[this.currentPos];
            }
        }

        private string ReadString()
        {
            Contract.Requires(!IsEOF);

            int startPos = this.currentPos;

            SkipToken();

            return this.input.Substring(startPos, this.currentPos - startPos);
        }

        private string ReadStringLiteral()
        {
            Contract.Requires(this.CurrentChar == '\'' || this.CurrentChar == '\"');

            char delimiter = this.CurrentChar;
            int startPos = this.currentPos;

            while (NextCharacter())
            {
                if (CurrentChar == delimiter)
                {
                    NextCharacter();
                    break;
                }
            }

            return this.input.Substring(startPos, this.currentPos - startPos);
        }

        private string ReadNonDecimalNumber()
        {
            Contract.Requires(!IsEOF);
            Contract.Requires(CurrentChar == '%' || CurrentChar == '$');

            int startPos = this.currentPos;
            SkipCharacter();

            if (!this.IsEOF)
            {
                SkipToken();
            }

            return this.input.Substring(startPos, this.currentPos - startPos);
        }

        private string ReadToEnd()
        {
            string s = this.input.Substring(this.currentPos);
            this.currentPos = this.input.Length; // set EOF

            return s;
        }

        private bool NextCharacter()
        {
            Contract.Requires(!IsEOF);
            ++this.currentPos;
            return !IsEOF;
        }

        private char SkipCharacter()
        {
            char c = CurrentChar;
            NextCharacter();
            return c;
        }

        public bool SkipWhitespace()
        {
            while (!this.IsEOF && char.IsWhiteSpace(this.CurrentChar))
            {
                this.NextCharacter();
            }

            return !this.IsEOF;
        }

        private void SkipToken()
        {
            while (NextCharacter() && !IsSeparator(CurrentChar))
                ;
        }

        private static bool IsSeparator(char c)
        {
            return char.IsWhiteSpace(c) || c == ';' || c == '$' || c == '%' || c == '#' ||
                   c == '=' || c == '<' || c == '>' || c == '+' || c == '-' || c == '*' ||
                   c == '/' || c == ',' || c == '(' || c == ')' || c == '\'' || c == '\"';
        }

        private bool IsASCIIChar(char c)
        {
            Contract.Requires(!IsEOF);

            return (c | 0x20) >= 'a' && (c | 0x20) <= 'z';
        }

        public _64TassScanner(string input)
        {
            this.input = input;
            this.currentPos = 0;
        }

        public string GetNextLexeme(out int starPosition)
        {
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
            Contract.Assert(!this.IsEOF);

            starPosition = this.currentPos;

            if (CurrentChar == ';')
            {
                return ReadToEnd();
            }
            else if (CurrentChar == '\'' || CurrentChar == '\"')
            {
                return ReadStringLiteral();
            }
            else if (CurrentChar == '%' || CurrentChar == '$')
            {
                return ReadNonDecimalNumber();
            }
            else if (IsSeparator(CurrentChar))
            {
                Contract.Assert(!char.IsWhiteSpace(CurrentChar));

                return SkipCharacter().ToString();
            }
            else
            {
                return ReadString();
            }
        }
    }
}
