//-----------------------------------------------------------------------
// <copyright file="VintageStudioProjectNode.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Project;
using VsCommands2K = Microsoft.VisualStudio.VSConstants.VSStd2KCmdID;

namespace VintageStudio.Project
{
    public class VintageStudioProjectNode : ProjectNode
    {
        private static readonly ImageList imageList;
        private readonly int imageIdx;

        private VintageStudioProjectPackage package;

        static VintageStudioProjectNode()
        {
            imageList = Utilities.GetImageList(typeof(VintageStudioProjectNode).Assembly.GetManifestResourceStream("VintageStudio.Project.Resources.65XXProgram.bmp"));
        }

        public VintageStudioProjectNode(VintageStudioProjectPackage package)
        {
            this.package = package;
            this.CanProjectDeleteItems = true;

            this.imageIdx = this.ImageHandler.ImageList.Images.Count;

            foreach (Image img in imageList.Images)
            {
                this.ImageHandler.AddImage(img);
            }
        }

        public override Guid ProjectGuid
        {
            get { return GuidList.guidVintageStudioProjectFactory; }
        }

        public override int ImageIndex
        {
            get { return this.imageIdx; }
        }

        public override string ProjectType
        {
            get { return this.GetType().Name; }
        }

        public override void AddFileFromTemplate(string source, string target)
        {
            this.FileTemplateProcessor.UntokenFile(source, target);
            this.FileTemplateProcessor.Reset();
        }

        protected override ReferenceContainerNode CreateReferenceContainerNode()
        {
            // Don't show "References" node. Note that returning null here causes 
            // NullReference exception in ProjectConfig.RefreshReferences() method. 
            // Since there is no way to override this behavior the bug had to be fixed
            // in the MPFProj sources.
            return null;
        }

        protected override int QueryStatusOnNode(Guid cmdGroup, uint cmd, IntPtr pCmdText, ref QueryStatusResult result)
        {
            // disable/hide "Add Reference..." menu option from context menu
            if ((VsCommands2K)cmd == VsCommands2K.ADDREFERENCE)
            {
                result |= QueryStatusResult.NOTSUPPORTED | QueryStatusResult.INVISIBLE;
                return VSConstants.S_OK;
            }

            return base.QueryStatusOnNode(cmdGroup, cmd, pCmdText, ref result);
        }

        protected override QueryStatusResult QueryStatusCommandFromOleCommandTarget(Guid guidCmdGroup, uint cmd, out bool handled)
        {
            // disable/hide "Add Reference..." menu option from context menu
            if (guidCmdGroup == VsMenus.guidStandardCommandSet2K && (VsCommands2K)cmd == VsCommands2K.ADDREFERENCE)
            {
                handled = false;
                return QueryStatusResult.NOTSUPPORTED | QueryStatusResult.INVISIBLE;
            }

            return base.QueryStatusCommandFromOleCommandTarget(guidCmdGroup, cmd, out handled);
        }


        /// <summary>
        /// Overriding to provide project general property page
        /// </summary>
        /// <returns>Page GUID</returns>
        protected override Guid[] GetConfigurationIndependentPropertyPages()
        {
            Guid[] result = new Guid[3];
            result[0] = typeof(VintageStudioProjectSettingsPage).GUID;
            result[1] = typeof(CompilerSettingsPage).GUID;
            result[2] = typeof(EmulatorSettingsPage).GUID;

            return result;
        }

        protected override ConfigProvider CreateConfigProvider()
        {
            return new VintageStudioProjectConfigProvider(this);
        }

        #region Hack to make the thing build correctly the first time
        /*
         * Without this (or a similar) hack the instruction in ProjectNode.cs on line 3066: 
         *    BuildSubmission submission = BuildManager.DefaultBuildManager.PendBuildRequest(requestData);
         * throws an exception saying: "The operation cannot be completed because BeginBuild has not yet been called."
         * The root cause of this issue is that "BeginBuild should have been called by the solution build manager 
         * before ever invoking your project system". Apparently for some reason it is not happening. 
         * The hack here is to call Clean MSBuild target the very first time the build is being Prepared target which 
         * in turn will call BeginBuild.
         * 
         * More details about this bug here: 
         * http://mpfproj10.codeplex.com/discussions/85680
         */

        private bool alreadyCalled = false;

        public override void PrepareBuild(string config, bool cleanBuild)
        {
            if (!alreadyCalled)
            {
                // for some reason config may not be populated when creating new project. 
                // This line resets config and enforce settings to be populated.
                // TODO: Investigate why this does not happen on the dev box and fix correctly
                this.ProjectMgr.GetProjectProperty(Setting.OutputFileName.ToString(), true);

                InvokeMsBuild(MsBuildTarget.Clean);
                alreadyCalled = true;
            }

            base.PrepareBuild(config, cleanBuild);
        }

        #endregion
    }

    public class VintageStudioProjectConfigProvider : ConfigProvider
    {
        public VintageStudioProjectConfigProvider(ProjectNode manager) :
            base(manager)
        { }

        protected override ProjectConfig CreateProjectConfiguration(string configName)
        {
            return new VintageStudioProjectConfiguration(ProjectMgr, configName);
        }
    }

    public class VintageStudioProjectConfiguration : ProjectConfig
    {
        public VintageStudioProjectConfiguration(ProjectNode project, string configuration) :
            base(project, configuration)
        {  }

        public override int DebugLaunch(uint grfLaunch)
        {
            string emulator = this.ProjectMgr.GetProjectProperty(Setting.EmulatorPath.ToString(), resetCache: true);
            string args;

            if ((grfLaunch & 0x04) != 0)
            {
                args = GetConfigurationProperty(Setting.EmulatorRunArguments.ToString(), resetCache: true);
            }
            else
            {
                args = GetConfigurationProperty(Setting.EmulatorDebugArguments.ToString(), resetCache: true);
            }

            Process.Start(emulator, args);
            return VSConstants.S_OK;
        }

        public override int QueryDebugLaunch(uint flags, out int fCanLaunch)
        {
            fCanLaunch = 1;

            return VSConstants.S_OK;
        }
    }
}
