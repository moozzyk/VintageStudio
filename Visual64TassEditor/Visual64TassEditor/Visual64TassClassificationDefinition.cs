//-----------------------------------------------------------------------
// <copyright file="Visual64TassClassificationDefinition.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Visual64TassEditor
{
    internal static class Visual64TassClassificationDefinition
    {
        /// <summary>
        /// Defines the "Visual64TassHexadecimalNumber" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Visual64TassHexadecimalNumber")]
        internal static ClassificationTypeDefinition Visual64TassHexadecimalNumber = null;

        /// <summary>
        /// Defines the "Visual64TassDecimalNumber" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Visual64TassDecimalNumber")]
        internal static ClassificationTypeDefinition Visual64TassDecimalNumber = null;

        /// <summary>
        /// Defines the "Visual64TassBinaryNumber" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Visual64TassBinaryNumber")]
        internal static ClassificationTypeDefinition Visual64TassBinaryNumber = null;

        /// <summary>
        /// Defines the "Visual64TassComment" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Visual64TassComment")]
        internal static ClassificationTypeDefinition Visual64TassComment = null;

        /// <summary>
        /// Defines the "Visual64Tass6502Instruction" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Visual64Tass6502Instruction")]
        internal static ClassificationTypeDefinition Visual64Tass6502Instruction = null;

        /// <summary>
        /// Defines the "Visual64TassIllegal6502Instruction" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Visual64TassIllegal6502Instruction")]
        internal static ClassificationTypeDefinition Visual64TassIllegal6502Instruction = null;

        /// <summary>
        /// Defines the "Visual64StringLiteral" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Visual64TassStringLiteral")]
        internal static Visual64TassStringLiteral Visual64TassStringLiteral = null; 

        /// <summary>
        /// Defines the "Visual64TassCompilerDirective" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Visual64TassCompilerDirective")]
        internal static Visual64TassCompilerDirective Visual64TassCompilerDirective = null;

        /// <summary>
        /// Defines the "Visual64TassLabel" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Visual64TassLabel")]
        internal static Visual64TassLabel Visual64TassLabel = null;    
    }
}