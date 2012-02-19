//-----------------------------------------------------------------------
// <copyright file="Visual64TassClassificationFormat.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Visual64TassEditor
{
    #region Format definition

    /// <summary>
    /// Defines an editor format for the Visual64TassBinaryNumber classification type.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Visual64TassBinaryNumber")]
    [Name("Visual64TassBinaryNumber")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Visual64TassBinaryNumberFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Visual64TassBinaryNumber" classification type
        /// </summary>
        public Visual64TassBinaryNumberFormat()
        {
            // this will show up in VS editor settings
            this.DisplayName = "Visual 64Tass Binary Number"; 
            this.ForegroundColor = Colors.SaddleBrown;
        }
    }

    /// <summary>
    /// Defines an editor format for the Visual64TassDecimalNumber classification type.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Visual64TassDecimalNumber")]
    [Name("Visual64TassDecimalNumber")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Visual64TassDecimalNumberFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Visual64TassDecimalNumber" classification type
        /// </summary>
        public Visual64TassDecimalNumberFormat()
        {
            // this will show up in VS editor settings
            this.DisplayName = "Visual 64Tass Decimal Number";
            this.ForegroundColor = Colors.DarkOrange;
        }
    }

    /// <summary>
    /// Defines an editor format for the Visual64TassHexadecimalNumber classification type.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Visual64TassHexadecimalNumber")]
    [Name("Visual64TassHexadecimalNumber")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Visual64TassHexadecimalNumberFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Visual64TassHexadecimalNumber" classification type
        /// </summary>
        public Visual64TassHexadecimalNumberFormat()
        {
            // this will show up in VS editor settings
            this.DisplayName = "Visual 64Tass Hexadecimal Number";
            this.ForegroundColor = Colors.Firebrick;
        }
    }

    /// <summary>
    /// Defines an editor format for the Visual64TassComment classification type.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Visual64TassComment")]
    [Name("Visual64TassComment")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Visual64TassComment : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Visual64TassComment" classification type
        /// </summary>
        public Visual64TassComment()
        {
            // this will show up in VS editor settings
            this.DisplayName = "Visual 64Tass Comment";
            this.ForegroundColor = Colors.DimGray;
        }
    }

    /// <summary>
    /// Defines an editor format for the Visual64Tass6502Instruction classification type.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Visual64Tass6502Instruction")]
    [Name("Visual64Tass6502Instruction")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Visual64Tass6502Instruction : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Visual64Tass6502Instruction" classification type
        /// </summary>
        public Visual64Tass6502Instruction()
        {
            // this will show up in VS editor settings
            this.DisplayName = "Visual 64Tass 6502 Instruction";
            this.ForegroundColor = Colors.Blue;
        }
    }

    /// <summary>
    /// Defines an editor format for the Visual64TassIllegal6502Instruction classification type.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Visual64TassIllegal6502Instruction")]
    [Name("Visual64TassIllegal6502Instruction")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Visual64TassIllegal6502Instruction : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Visual64TassIllegal6502Instruction" classification type
        /// </summary>
        public Visual64TassIllegal6502Instruction()
        {
            // this will show up in VS editor settings
            this.DisplayName = "Visual 64Tass Illegal 6502 Instruction";
            this.ForegroundColor = Colors.RosyBrown;
        }
    }

    /// <summary>
    /// Defines an editor format for the Visual64TassStringLiteral classification type.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Visual64TassStringLiteral")]
    [Name("Visual64TassStringLiteral")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Visual64TassStringLiteral : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Visual64TassStringLiteral" classification type
        /// </summary>
        public Visual64TassStringLiteral()
        {
            // this will show up in VS editor settings
            this.DisplayName = "Visual 64Tass String Literal";
            this.ForegroundColor = Colors.Red;
        }
    }

    /// <summary>
    /// Defines an editor format for the Visual64TassCompilerDirective classification type.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Visual64TassCompilerDirective")]
    [Name("Visual64TassCompilerDirective")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Visual64TassCompilerDirective : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Visual64CompilerDirective" classification type
        /// </summary>
        public Visual64TassCompilerDirective()
        {
            // this will show up in VS editor settings
            this.DisplayName = "Visual 64Tass Compiler Directive";
            this.ForegroundColor = Colors.DarkCyan;
        }
    }

    /// <summary>
    /// Defines an editor format for the Visual64TassLabel classification type.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Visual64TassLabel")]
    [Name("Visual64TassLabel")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Visual64TassLabel : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "Visual64TassLabel" classification type
        /// </summary>
        public Visual64TassLabel()
        {
            // this will show up in VS editor settings
            this.DisplayName = "Visual 64Tass Label";
            this.ForegroundColor = Colors.Purple;
        }
    }


    #endregion //Format definition
}
