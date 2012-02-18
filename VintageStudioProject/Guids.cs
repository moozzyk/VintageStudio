//-----------------------------------------------------------------------
// <copyright file="Guids.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace VintageStudio.Project
{
    static class GuidList
    {
        public const string guidVintageStudioProjectPkgString      = "7d63f1c7-9b0e-4cbe-9fc4-efb5fdd1c946";
        public const string guidVintageStudioProjectFactoryString  = "d5b39620-a3e7-4339-b85f-2092a9f893fc";
        public const string guidFolderGuid                         = "e59854e0-ca91-4601-b6b9-ae04454162ce";

        public static readonly Guid guidVintageStudioProjectFactory = new Guid(guidVintageStudioProjectFactoryString);
    };
}