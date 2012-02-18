//-----------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Visual64TassEditor.Parser
{
    internal class Tokenizer
    {
        private Dictionary<char, _64TassTokenType> tokenTypePerChar =
            new Dictionary<char, _64TassTokenType>()
            {
                { '#', _64TassTokenType.Hash },
                { '=', _64TassTokenType.Equals }, 
                { '<', _64TassTokenType.LessThan },
                { '>', _64TassTokenType.GreaterThan }, 
                { '+', _64TassTokenType.Plus }, 
                { '-', _64TassTokenType.Minus }, 
                { '*', _64TassTokenType.Star }, 
                { '/', _64TassTokenType.Slash }, 
                { ',', _64TassTokenType.Coma }, 
                { '(', _64TassTokenType.LeftParen }, 
                { ')', _64TassTokenType.RightParen },
            };

        private static string[] _6502Instructions = { 
            "LDA", "STA", "LDX", "STX", "LDY", "STY", "BNE", "BPL", "BMI", "BEQ",
            "BCC", "BCS", "BVC", "BVS", "AND", "ORA", "EOR", "DEC", "DEX", "DEY",
            "INC", "INX", "INY", "CMP", "CPX", "CPY", "JMP", "JSR", "RTS", "CLC", 
            "ADC", "SBC", "SEC", "TAX", "TXA", "TAY", "TYA", "NOP", "BIT", "ASL", 
            "LSR", "ROL", "ROR", "PHA", "PLA", "CLD", "CLI", "CLV", "SED", "SEI", 
            "PHP", "PLP", "TSX", "TXS", "RTI", "BRK" };

        private static string[] Illegal6502Instructions = {
            "ANC", "ANE", "XAA", "ARR", "ASR", "ALR", "DCP", "DCM", "ISB", "INS", 
            "ISC", "JAM", "LAE", "LAS", "LDS", "LAX", "LXA", "LAX", "RLA", "RRA", 
            "SAX", "SBX", "AXS", "SHA", "AHX", "SHS", "TAS", "SHX", "SHY", "SLO", 
            "SRE" };

        private static string[] CompilerDirectives = {
            ".byte", ".text", ".char", ".shift", ".null", ".rta", ".word", ".int", ".long", 
            ".offs", ".macro", ".endm", ".for", ".next", ".if", ".ifne", ".ifeq", ".ifpl", 
            ".ifmi", ".else", ".elsif", ".fi", ".endif", ".rept", ".include", ".binary", 
            ".comment", ".endc", ".page", ".endp", ".logical", ".here", ".as", ".al", ".xs", 
            ".xl", ".error", ".warn", ".cerror", ".cwarn", ".proc", ".pend", ".databank", 
            ".dpage", ".fill", ".align", ".enc", ".cpu", ".global", ".assert", ".check" };

        private enum State { BeforeInstruction, _6502InstructionArgument, Index, AfterInstruction };
        private _64TassTokenType intructionTokenType = _64TassTokenType.DontKnow;

        private State currentState;

        public IEnumerable<_64TassToken> Tokenize(string inputLine)
        {
            currentState = State.BeforeInstruction;
            intructionTokenType = _64TassTokenType.DontKnow;

            var scanner = new _64TassScanner(inputLine);

            while (scanner.SkipWhitespace())
            {
                yield return GetNextToken(scanner);
            }
        }
        
        private _64TassToken GetNextToken(_64TassScanner scanner)
        {
            Contract.Requires(scanner != null);
            Contract.Ensures(Contract.Result<_64TassToken>() != null);

            int tokenStartPosition;
            var lexem = scanner.GetNextLexeme(out tokenStartPosition);

            return new _64TassToken(
                this.GetTokenTypeForLexem(lexem),
                tokenStartPosition,
                lexem);
        }

        private _64TassTokenType GetTokenTypeForLexem(string lexem)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(lexem));

            if (lexem[0] == ';')
            {
                return _64TassTokenType.Comment;
            }
            else if (lexem[0] == '\"' || lexem[0] == '\'')
            {
                return _64TassTokenType.StringLiteral;
            }
            else if (lexem[0] == '$')
            {
                return IsValidHexNumber(lexem) ? 
                    _64TassTokenType.HexadecimalNumber : 
                    _64TassTokenType.DontKnow;
            }
            else if (lexem[0] == '%')
            {
                return IsValidBinNumber(lexem) ? 
                    _64TassTokenType.BinaryNumber :
                    _64TassTokenType.DontKnow;
            }
            else if (char.IsDigit(lexem[0]))
            {
                return IsValidDecNumber(lexem) ? 
                        _64TassTokenType.DecimalNumber :
                        _64TassTokenType.DontKnow;
            }
            else if (lexem[0] == '.' && CompilerDirectives.Any(d => d == lexem))
            {
                return _64TassTokenType.CompilerDirective;
            }
            else
            {
                if (lexem.Length == 1)
                {
                    _64TassTokenType tokenType;
                    if (tokenTypePerChar.TryGetValue(lexem[0], out tokenType))
                    {
                        if (currentState == State._6502InstructionArgument && tokenType == _64TassTokenType.Coma)
                        {
                            currentState = State.Index;
                        }

                        return tokenType;
                    }
                    else if(currentState == State.Index && ((lexem[0] & 0xdf) == 'X' || (lexem[0] & 0xdf) == 'Y'))
                    {
                        Contract.Requires(this.intructionTokenType == _64TassTokenType._6502Instruction ||
                            this.intructionTokenType == _64TassTokenType._6502IllegalInstruction);

                        return this.intructionTokenType;
                    }
                }
                else if (lexem.Length == 3 && currentState == State.BeforeInstruction)
                {
                    if (_6502Instructions.Any(i => lexem.ToUpper() == i))
                    {
                        currentState = State._6502InstructionArgument;

                        return intructionTokenType = _64TassTokenType._6502Instruction;
                    }
                    else if (Illegal6502Instructions.Any(i => lexem.ToUpper() == i))
                    {
                        currentState = State._6502InstructionArgument;

                        return intructionTokenType = _64TassTokenType._6502IllegalInstruction;
                    }
                }

                return _64TassTokenType.Label;
            }
        }

        private static bool IsValidHexNumber(string number)
        {
            Contract.Requires(number[0] == '$');

            return IsValidNumber(number, c => (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'), true);
        }

        private static bool IsValidDecNumber(string number)
        {
            Contract.Requires(char.IsDigit(number[0]));

            return IsValidNumber(number, c => c >= '0' && c <= '9', false);
        }

        private static bool IsValidBinNumber(string number)
        {
            Contract.Requires(number[0] == '%');

            return IsValidNumber(number, c => c == '0' || c == '1', true);
        }

        private static bool IsValidNumber(string number, Func<char, bool> isValidDigit, bool skipFirstCharacter)
        {
            return !number.Skip(skipFirstCharacter ? 1 : 0).Any(c => !isValidDigit(c));
        }
    }
}
