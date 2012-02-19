//-----------------------------------------------------------------------
// <copyright file="64TassToken.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System.Diagnostics.Contracts;

namespace Visual64TassEditor.Parser
{
    internal class _64TassToken
    {
        private readonly _64TassTokenType tokenType;
        private readonly string tokenValue;
        private readonly int tokenStartPosition;

        public _64TassToken(_64TassTokenType tokenType, int tokenStartPosition, string tokenValue)
        {
            Contract.Requires(tokenStartPosition >= 0);

            this.tokenType = tokenType;
            this.tokenValue = tokenValue;
            this.tokenStartPosition = tokenStartPosition;
        }

        public _64TassTokenType Type 
        { 
            get { return this.tokenType; } 
        }

        public int StartPosition
        {
            get { return this.tokenStartPosition; }
        }

        public string Value
        {
            get { return this.tokenValue; }
        }
    }
}
