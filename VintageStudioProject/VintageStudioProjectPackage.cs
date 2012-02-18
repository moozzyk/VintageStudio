//-----------------------------------------------------------------------
// <copyright file="VintageStudioProjectPackage.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell;

namespace VintageStudio.Project
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [ProvideProjectFactory(
        typeof(VintageStudioProjectFactory),
        "Vintage Studio",
        "Vintage Studio Project Files (*.VintageProj);*.VintageProj",
        "VintageProj", "VintageProj",
        @"Templates\Projects",
        LanguageVsTemplate = "Vintage Studio")]
    [Guid(GuidList.guidVintageStudioProjectPkgString)]
    [ProvideObject(typeof(VintageStudioProjectSettingsPage))]
    [ProvideObject(typeof(CompilerSettingsPage))]
    [ProvideObject(typeof(EmulatorSettingsPage))]
    public class VintageStudioProjectPackage : ProjectPackage
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public VintageStudioProjectPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // We don't have references in this project. Remove reference update listener since it expects
            // references and throw if they don't find ones. Need to dispose to unhook the listener.
            for (int listenerIdx = this.SolutionListeners.Count; listenerIdx-- > 0; )
            {
                if (this.SolutionListeners[listenerIdx] is SolutionListenerForProjectReferenceUpdate)
                {
                    var listener = this.SolutionListeners[listenerIdx];
                    this.SolutionListeners.RemoveAt(listenerIdx);
                    listener.Dispose();
                }
            }

            this.RegisterProjectFactory(new VintageStudioProjectFactory(this));
        }

        #endregion

        public override string ProductUserContext 
        { 
            get { throw new NotImplementedException();  } 
        }

    }
}
