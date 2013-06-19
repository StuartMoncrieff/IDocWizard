﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Gui;
using MyLoadTest.Configuration;
using MyLoadTest.SapIDocGenerator.UI.Addin.Pads;

namespace MyLoadTest.SapIDocGenerator.UI.Addin.Commands
{
    /// <summary>
    ///     Represents the command that is executed automatically when IDE starts.
    /// </summary>
    public sealed class StartupCommand : AbstractMenuCommand
    {
        #region Constants and Fields

        private static readonly string SettingNamePrefix = typeof(StartupCommand).Assembly.GetName().Name + ".";

        #endregion

        #region Public Methods

        /// <summary>
        ///     Invokes the command.
        /// </summary>
        public override void Run()
        {
            InitializeSettingManager();

            WorkbenchSingleton.WorkbenchCreated += this.WorkbenchSingleton_WorkbenchCreated;
        }

        #endregion

        #region Private Methods

        private static void InitializeSettingManager()
        {
            SettingManager.Instance.SetAccessors(GetSetting, SetSetting);
        }

        private static string GetActualSettingName(string name)
        {
            var result = SettingNamePrefix + name;
            return result;
        }

        private static void SetSetting(string name, string value)
        {
            PropertyService.Set(GetActualSettingName(name), value);
        }

        private static string GetSetting(string name, string defaultValue)
        {
            var result = PropertyService.Get(GetActualSettingName(name), defaultValue);
            return result;
        }

        private void WorkbenchSingleton_WorkbenchCreated(object sender, EventArgs e)
        {
            WorkbenchSingleton.Workbench.MainWindow.IsVisibleChanged += this.MainWindow_IsVisibleChanged;
            WorkbenchSingleton.Workbench.MainWindow.Activated += this.MainWindow_Activated;
        }

        private void MainWindow_Activated(object sender, EventArgs eventArgs)
        {
            WorkbenchSingleton.Workbench.MainWindow.Activated -= this.MainWindow_Activated;

            WizardPad.ShowInWorkbench();
        }

        private void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!WorkbenchSingleton.Workbench.MainWindow.IsVisible)
            {
                return;
            }

            WorkbenchSingleton.Workbench.MainWindow.IsVisibleChanged -= this.MainWindow_IsVisibleChanged;

            WizardPad.ShowInWorkbench();
        }

        #endregion
    }
}