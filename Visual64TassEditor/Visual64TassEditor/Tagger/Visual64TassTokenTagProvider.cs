//-----------------------------------------------------------------------
// <copyright file="Visual64TassTokenTagProvider.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Visual64TassEditor.Tagger
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("Visual64Tass")]
    [TagType(typeof(Visual64TassTokenTag))]
    internal class Visual64TassTokenTagProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) 
            where T : ITag
        {
            Contract.Requires(buffer != null);

            return (ITagger<T>)new Visual64TassTokenTagger( /*buffer*/);
        }
    }
}