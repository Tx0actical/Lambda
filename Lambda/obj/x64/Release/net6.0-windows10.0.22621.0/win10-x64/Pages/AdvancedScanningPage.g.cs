﻿#pragma checksum "C:\Users\mannu\source\Lambda\Lambda\Pages\AdvancedScanningPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "BCF2364741B7761B14687F7D78BB5D52E97E96E562BDF3E0BC4BDE280DA13549"
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
    partial class AdvancedScanningPage : 
        global::Microsoft.UI.Xaml.Controls.Page, 
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
            case 2: // Pages\AdvancedScanningPage.xaml line 40
                {
                    this.CustomDialogOverlay = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 3: // Pages\AdvancedScanningPage.xaml line 43
                {
                    this.CustomDialogTitle = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 4: // Pages\AdvancedScanningPage.xaml line 44
                {
                    this.CustomDialogMessage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 5: // Pages\AdvancedScanningPage.xaml line 45
                {
                    this.CustomDialogCloseButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.CustomDialogCloseButton).Click += this.CustomDialogCloseButton_Click;
                }
                break;
            case 6: // Pages\AdvancedScanningPage.xaml line 12
                {
                    this.AdvScanStackPanel = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.StackPanel>(target);
                }
                break;
            case 7: // Pages\AdvancedScanningPage.xaml line 15
                {
                    this.PickAFileButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.PickAFileButton).Click += this.PickObjectButton_Click;
                }
                break;
            case 8: // Pages\AdvancedScanningPage.xaml line 16
                {
                    this.PickAFileOutputTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 9: // Pages\AdvancedScanningPage.xaml line 28
                {
                    this.IdResponseTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 10: // Pages\AdvancedScanningPage.xaml line 30
                {
                    this.TypeResponseTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 11: // Pages\AdvancedScanningPage.xaml line 32
                {
                    this.OutputTextBox = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                }
                break;
            case 12: // Pages\AdvancedScanningPage.xaml line 34
                {
                    this.StatusDialog = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ContentDialog>(target);
                }
                break;
            case 13: // Pages\AdvancedScanningPage.xaml line 35
                {
                    this.advprogressbar = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ProgressBar>(target);
                }
                break;
            case 14: // Pages\AdvancedScanningPage.xaml line 23
                {
                    this.AdvancedButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.AdvancedButton).Click += this.AdvButton_Click;
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
