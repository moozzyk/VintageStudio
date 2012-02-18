//-----------------------------------------------------------------------
// <copyright file="VintageStudioProjectFactory.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace VintageStudio.Project
{
    [Guid(GuidList.guidVintageStudioProjectFactoryString)]
    class VintageStudioProjectFactory : ProjectFactory
    {
        private VintageStudioProjectPackage package;

        public VintageStudioProjectFactory(VintageStudioProjectPackage package) 
            : base(package)
        {
            this.package = package;
        }

        protected override ProjectNode CreateProject()
        {
            var project = new VintageStudioProjectNode(this.package);
            project.SetSite((IOleServiceProvider)((IServiceProvider)this.package).GetService(typeof(IOleServiceProvider)));
            return project;            
        }
    }
}
