﻿#pragma checksum "C:\Users\mannu\source\Lambda\Lambda\Pages\AdvancedScanningPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "56FFE6D07A1E94D924E9130B432D455D3A32D3CEC4761F0B94528E8C1CE477BA"
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
            case 2: // Pages\AdvancedScanningPage.xaml line 23
                {
                    this.CustomDialogOverlay = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Grid>(target);
                }
                break;
            case 3: // Pages\AdvancedScanningPage.xaml line 26
                {
                    this.CustomDialogTitle = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 4: // Pages\AdvancedScanningPage.xaml line 27
                {
                    this.CustomDialogMessage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 5: // Pages\AdvancedScanningPage.xaml line 28
                {
                    this.CustomDialogCloseButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.CustomDialogCloseButton).Click += this.CustomDialogCloseButton_Click;
                }
                break;
            case 6: // Pages\AdvancedScanningPage.xaml line 13
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
            case 9: // Pages\AdvancedScanningPage.xaml line 17
                {
                    this.AdvancedButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.AdvancedButton).Click += this.AdvButton_Click;
                }
                break;
            case 10: // Pages\AdvancedScanningPage.xaml line 18
                {
                    this.StatusDialog = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ContentDialog>(target);
                }
                break;
            case 11: // Pages\AdvancedScanningPage.xaml line 19
                {
                    this.advprogressbar = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ProgressBar>(target);
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

