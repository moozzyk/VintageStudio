//-----------------------------------------------------------------------
// <copyright file="VintageStudioPropertyPages.cs" author="Pawel Kadluczka">
//
// Copyright (C) 2011 by Pawel Kadluczka
//
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.IO;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Project;

namespace VintageStudio.Project
{
    internal enum Setting
    {
        OutputFileName,
        CompilerPath,
        CompileCommand,
        CustomErrorRegularExpression,
        EmulatorPath,
        EmulatorRunArguments,
        EmulatorDebugArguments
    }

    public class VintageStudioProjectSettingsPage : SettingsPage
    {
        private string outputFileName;

        public VintageStudioProjectSettingsPage()
        {
            this.Name = "General";
        }

        protected override void BindProperties()
        {
            this.outputFileName = this.ProjectMgr.GetProjectProperty(Setting.OutputFileName.ToString(), true);
        }

        protected override int ApplyChanges()
        {
            if (string.IsNullOrEmpty(this.OutputFileName))
            {
                throw new ArgumentException("The name of the output file cannot be empty");
            }
 
            this.ProjectMgr.SetProjectProperty(Setting.OutputFileName.ToString(), this.OutputFileName);
            this.IsDirty = false;

            return VSConstants.S_OK;
        }

        [DisplayName("Output File Name")]
        public string OutputFileName
        {
            get
            {
                return this.outputFileName;
            }
            set
            {
                this.outputFileName = value;
                this.IsDirty = true;
            }
        }
    }

    public class CompilerSettingsPage : SettingsPage
    {
        private string compilerPath;
        private string compileCommand;
        private string customErrorRegularExpression;

        public CompilerSettingsPage()
        {
            this.Name = "Compiler";
        }

        protected override void BindProperties()
        {
            this.compilerPath = this.ProjectMgr.GetProjectProperty(Setting.CompilerPath.ToString(), false);
            this.compileCommand = this.ProjectMgr.GetProjectProperty(Setting.CompileCommand.ToString(), false);
            this.customErrorRegularExpression = this.ProjectMgr.GetProjectProperty(Setting.CustomErrorRegularExpression.ToString(), false);
        }

        protected override int ApplyChanges()
        {
            if (string.IsNullOrEmpty(this.compilerPath))
            {
                throw new ArgumentException("Path to the compiler cannot be empty.");
            }

            this.ProjectMgr.SetProjectProperty(Setting.CompilerPath.ToString(), this.compilerPath);
            this.ProjectMgr.SetProjectProperty(Setting.CompileCommand.ToString(), this.compileCommand);
            this.ProjectMgr.SetProjectProperty(Setting.CustomErrorRegularExpression.ToString(), this.customErrorRegularExpression);

            this.IsDirty = false;
            return VSConstants.S_OK;
        }

        [DisplayName("Compiler Path")]
        [Description("Path to the compiler to compile your program.")]
        public string CompilerPath
        {
            get
            {
                return this.compilerPath;
            }
            set
            {
                this.compilerPath = value;
                this.IsDirty = true;
            }
        }

        [DisplayName("Compile Command")]
        [Description("Command used to compile your program (e.g. $(CompilerPath) -o \"$(CompilerOutput)\" @(Compile)).")]
        public string CompileCommand
        {
            get
            {
                return this.compileCommand;
            }
            set
            {
                this.compileCommand = value;
                this.IsDirty = true;
            }
        }

        [DisplayName("Custom Error Regular Expression")]
        [Description(@"Regular expression used to capture errors returned by compiler (e.g. (?<File>^.+\..+):(?<Line>\d+): (?<Message>.+)).")]
        public string CustomErrorRegularExpression
        {
            get
            {
                return this.customErrorRegularExpression;
            }
            set
            {
                this.customErrorRegularExpression = value;
                this.IsDirty = true;
            }
        }
    }

    public class EmulatorSettingsPage : SettingsPage
    {        
        private string emulatorPath;
        private string emulatorRunArguments;
        private string emulatorDebugArguments;

        public EmulatorSettingsPage()
        {
            this.Name = "Emulator";
        }

        protected override void BindProperties()
        {
            this.emulatorPath = this.ProjectMgr.GetProjectProperty(Setting.EmulatorPath.ToString(), false);
            this.emulatorRunArguments = this.ProjectMgr.GetProjectProperty(Setting.EmulatorRunArguments.ToString(), false);
            this.emulatorDebugArguments = this.ProjectMgr.GetProjectProperty(Setting.EmulatorDebugArguments.ToString(), false);
        }

        protected override int ApplyChanges()
        {
            if (string.IsNullOrEmpty(this.EmulatorPath))
            {
                throw new ArgumentException("The path to the emulator cannot be empty.");
            }

            this.ProjectMgr.SetProjectProperty(Setting.EmulatorPath.ToString(), this.EmulatorPath);
            this.ProjectMgr.SetProjectProperty(Setting.EmulatorRunArguments.ToString(), this.emulatorRunArguments);
            this.ProjectMgr.SetProjectProperty(Setting.EmulatorDebugArguments.ToString(), this.emulatorDebugArguments);
            this.IsDirty = false;
            return VSConstants.S_OK;
        }

        [DisplayName("Emulator Path")]
        [Description("Path to the emulator your program will start with.")]
        public string EmulatorPath
        {
            get
            {
                return this.emulatorPath;
            }
            set
            {
                this.emulatorPath = value;
                this.IsDirty = true;
            }
        }

        [DisplayName("Emulator Run Arguments")]
        [Description(@"Arguments used to start the emulator in the non-debug mode. Hint: You can use variables here - e.g. $(MSBuildProjectDirectory)\$(OutputPath)$(OutputFileName) is the path to your compiled program.")]
        public string EmulatorRunArguments
        {
            get
            {
                return this.emulatorRunArguments;
            }
            set
            {
                this.emulatorRunArguments = value;
                this.IsDirty = true;
            }
        }

        [DisplayName("Emulator Debug Arguments")]
        [Description(@"Arguments used to start the emulator in the debug mode.\nHint: You can use variables here - e.g. $(MSBuildProjectDirectory)\$(OutputPath)$(OutputFileName) is the path to your compiled program.")]
        public string EmulatorDebugArguments 
        {
            get
            {
                return this.emulatorDebugArguments;
            }
            set
            {
                this.emulatorDebugArguments = value;
                this.IsDirty = true;
            }
        }
    }

    internal class ResourceProvider
    {
        public string GetSettingName(Setting setting)
        {
            return null;
        }

        public string GetSettingDescrption(Setting setting)
        {
            return null;
        }
    }
}
