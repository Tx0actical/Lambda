using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using Windows.Storage.Pickers;
using Program; // local namespace for APIHandler.cs
using RestSharp;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Lambda {
    /// <summary>
    /// Page displaying the advanced scanning options.
    /// </summary>
    public sealed partial class AdvancedScanningPage : Page {

        
        public static bool CameFromToggle = false;
        public static bool CameFromGridChange = false;
        public dynamic fileContent;

        public AdvancedScanningPage () {
            this.InitializeComponent ();
        }

        protected override void OnNavigatedTo (NavigationEventArgs e) {
            base.OnNavigatedTo (e);
        }

        private async void AdvButton_Click (object sender, RoutedEventArgs e) {

            
            AdvancedButton.Visibility = Visibility.Collapsed;

            APIOperationsHandler __handler = new();
            advprogressbar.Visibility = Visibility.Visible;
            dynamic response = await __handler.VTAPI_Upload_File (__handler.Request);

            if (response.IsSuccessful) {
                await DisplayStatusDialog ("Request sent successfully", "The request was sent successfully, and the server responded with a status code of " + response.StatusCode);
                advprogressbar.IsIndeterminate = false;
                advblock.Text = response.ToString ();
                advblock.Visibility = Visibility.Visible;
            } else {
                await DisplayStatusDialog ("Request failed", "The request failed with a status code of " + response.StatusCode + ". Please try again.");
                advprogressbar.ShowError = true;
                advblock.Text = "Failed";
                advblock.Visibility = Visibility.Visible;
            }
            
            AdvancedButton = (Button) sender;
        }

        private async void PickObjectButton_Click (object sender, RoutedEventArgs e) {

            var openPicker = new FileOpenPicker() { 
                ViewMode = PickerViewMode.Thumbnail,
                FileTypeFilter = { "*" },
            };

            // Initialize Window
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            WinRT.Interop.InitializeWithWindow.Initialize (openPicker, hWnd);

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) {
                PickAFileOutputTextBlock.Text = "Selected File : " + file.Name;
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                byte[] fileContent = buffer.ToArray();  // Pass the fileContent to the SendObjectButton_Click method or another method to handle the file content
            } else {
                PickAFileOutputTextBlock.Text = "Operation Cancelled";
            }
        }

        private async Task DisplayStatusDialog (string title, string message) {
            ContentDialog statusDialog = new ContentDialog {
                Title = title,
                Content = message,
                CloseButtonText = "Ok"
            };
            await statusDialog.ShowAsync ();
        }

    }
}
