﻿#pragma checksum "C:\Users\mannu\source\Lambda\Lambda\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "FCCD964F39521399DA04D707E8841185B38334AB9F204C05AB09C43F0408F28B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lambda
{
    partial class MainWindow : 
        global::Microsoft.UI.Xaml.Window, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 1.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainWindow.xaml line 14
                {
                    this.AppTitleBar = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 3: // MainWindow.xaml line 25
                {
                    this.NavView = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NavigationView>(target);
                    ((global::Microsoft.UI.Xaml.Controls.NavigationView)this.NavView).SelectionChanged += this.NavigationView_SelectionChanged;
                    ((global::Microsoft.UI.Xaml.Controls.NavigationView)this.NavView).ItemInvoked += this.NavView_ItemInvoked;
                    ((global::Microsoft.UI.Xaml.Controls.NavigationView)this.NavView).Loaded += this.NavView_Loaded;
                    ((global::Microsoft.UI.Xaml.Controls.NavigationView)this.NavView).BackRequested += this.NavView_BackRequested;
                }
                break;
            case 4: // MainWindow.xaml line 49
                {
                    this.HomePage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NavigationViewItem>(target);
                }
                break;
            case 5: // MainWindow.xaml line 55
                {
                    this.AdvancedScanningPage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NavigationViewItem>(target);
                }
                break;
            case 6: // MainWindow.xaml line 61
                {
                    this.ScanHistoryPage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NavigationViewItem>(target);
                }
                break;
            case 7: // MainWindow.xaml line 67
                {
                    this.ResultsPage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NavigationViewItem>(target);
                }
                break;
            case 8: // MainWindow.xaml line 75
                {
                    this.AccountPage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NavigationViewItem>(target);
                }
                break;
            case 9: // MainWindow.xaml line 81
                {
                    this.HelpPage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NavigationViewItem>(target);
                }
                break;
            case 10: // MainWindow.xaml line 87
                {
                    this.SettingsPage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.NavigationViewItem>(target);
                }
                break;
            case 11: // MainWindow.xaml line 95
                {
                    this.ContentFrame = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Frame>(target);
                }
                break;
            case 12: // MainWindow.xaml line 19
                {
                    this.AppTitleTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 1.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

