//-----------------------------------------------------------------------
// <copyright file="Visual64TassClassifier.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Visual64TassEditor.Parser;
using Visual64TassEditor.Tagger;

namespace Visual64TassEditor
{
    /// <summary>
    /// Classifier that classifies all text as an instance of the OrinaryClassifierType
    /// </summary>
    class Visual64TassClassifier : ITagger<ClassificationTag>
    {
        private ITextBuffer buffer;
        private ITagAggregator<Visual64TassTokenTag> tagAggregator;
        private IClassificationTypeRegistryService typeService;

        internal Visual64TassClassifier(ITextBuffer buffer, 
                                ITagAggregator<Visual64TassTokenTag> tagAggregator, 
                               IClassificationTypeRegistryService typeService)
        {
            this.buffer = buffer;
            this.tagAggregator = tagAggregator;
            this.typeService = typeService;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (var tagSpan in this.tagAggregator.GetTags(spans))
            {
                var tagSpans = tagSpan.Span.GetSpans(spans[0].Snapshot);
                var classificationType = GetClassificationTagForToken(tagSpan.Tag);

                if (classificationType != null)
                {
                    yield return new TagSpan<ClassificationTag>(tagSpans[0], new ClassificationTag(classificationType));
                }
            }
        }

        private IClassificationType GetClassificationTagForToken(Visual64TassTokenTag tag)
        {
            switch (tag.Token.Type)
            {
                case _64TassTokenType._6502Instruction:
                    return this.typeService.GetClassificationType("Visual64Tass6502Instruction");
                case _64TassTokenType._6502IllegalInstruction:
                    return this.typeService.GetClassificationType("Visual64TassIllegal6502Instruction");
                case _64TassTokenType.Label:
                    return this.typeService.GetClassificationType("Visual64TassLabel");
                case _64TassTokenType.BinaryNumber:
                    return this.typeService.GetClassificationType("Visual64TassBinaryNumber");
                case _64TassTokenType.DecimalNumber:
                    return this.typeService.GetClassificationType("Visual64TassDecimalNumber");
                case _64TassTokenType.HexadecimalNumber:
                    return this.typeService.GetClassificationType("Visual64TassHexadecimalNumber");
                case _64TassTokenType.Star:
                case _64TassTokenType.Equals:
                    return this.typeService.GetClassificationType(PredefinedClassificationTypeNames.Operator);
                case _64TassTokenType.Comment:
                    return this.typeService.GetClassificationType("Visual64TassComment");
                case _64TassTokenType.CompilerDirective:
                    return this.typeService.GetClassificationType("Visual64TassCompilerDirective");
                case _64TassTokenType.StringLiteral:
                    return this.typeService.GetClassificationType("Visual64TassStringLiteral");
                default:
                    return null;
            }
        }
    }
}

