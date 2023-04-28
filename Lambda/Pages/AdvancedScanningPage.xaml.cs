using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using Windows.Storage.Pickers;
using Program; // local namespace for APIHandler.cs
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
            dynamic response = await __handler.VTAPI_Upload_File(__handler.Request);

            if (response.IsSuccessful) {
               
                advprogressbar.Visibility = Visibility.Visible;
                ShowCustomDialog ("Request sent successfully", "The request was sent successfully, and the server responded with a status code of " + response.StatusCode);
            } else {
                
                advprogressbar.Visibility = Visibility.Visible;
                advprogressbar.ShowError = true;
                ShowCustomDialog ("Request sent successfully", "The request was sent successfully, and the server responded with a status code of " + response.StatusCode);
            }

            AdvancedButton = (AppBarButton) sender;
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

        private void ShowCustomDialog (string title, string message) {
            CustomDialogTitle.Text = title;
            CustomDialogMessage.Text = message;
            CustomDialogOverlay.Visibility = Visibility.Visible;
        }

        private void CustomDialogCloseButton_Click (object sender, RoutedEventArgs e) {
            CustomDialogOverlay.Visibility = Visibility.Collapsed;
        }
    }
}
