//-----------------------------------------------------------------------
// <copyright file="Visual64TassClassifierProvider.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using Visual64TassEditor.Tagger;

namespace Visual64TassEditor
{
    #region Provider definition

    [Export(typeof(ITaggerProvider))]
    [ContentType("Visual64Tass")]
    [TagType(typeof(ClassificationTag))]
    internal class Visual64TassClassifierProvider : ITaggerProvider
    {
        [Export]
        [Name("Visual64Tass")]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition Visual64TassContentType = null;

        [Export]
        [FileExtension(".asm")]
        [ContentType("Visual64Tass")]
        internal static FileExtensionToContentTypeDefinition Visual64TassFileType = null;

        [Import]
        internal IClassificationTypeRegistryService ClassificationTypeRegistry = null;

        [Import]
        internal IBufferTagAggregatorFactoryService aggregatorFactory = null;


        public ITagger<T> CreateTagger<T>(ITextBuffer buffer)
            where T : ITag
        {
            ITagAggregator<Visual64TassTokenTag> tagAggregator =
                aggregatorFactory.CreateTagAggregator<Visual64TassTokenTag>(buffer);

            return (ITagger<T>)new Visual64TassClassifier(buffer, tagAggregator, ClassificationTypeRegistry);
        }
    }
    #endregion //provider def
}