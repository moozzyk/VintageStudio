//-----------------------------------------------------------------------
// <copyright file="64TassTokenType.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

namespace Visual64TassEditor.Parser
{
    internal enum _64TassTokenType
    {
        _6502Instruction,
        _6502IllegalInstruction,
        BinaryNumber, 
        DecimalNumber,
        HexadecimalNumber,
        StringLiteral,
        Comment,
        CompilerDirective,
        Label,
        Hash,
        Plus,
        Minus,
        Star,
        Slash,
        Equals,
        GreaterThan,
        LessThan,
        LeftParen, 
        RightParen,
        Coma,
        DontKnow
    }
}
