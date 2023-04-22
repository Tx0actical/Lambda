using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using Windows.Storage.Pickers;

namespace Lambda
{
    /// <summary>
    /// Page displaying the advanced scanning options.
    /// </summary>
    public sealed partial class AdvancedScanningPage : Page
    {
        public AdvancedScanningPage() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
        }

        private static int SuccessStatusCode = 200;
        public static bool CameFromToggle = false;
        public static bool CameFromGridChange = false;

        private void AdvButton_Click (object sender, RoutedEventArgs e) {

            ContentDialog RequestSuccessDialog = new ContentDialog ();
            ContentDialog RequestErrorDialog = new ContentDialog ();


            // TODO: need to bind this action with sending request
            // TODO: if request is success, display content dialoue showing the process.

            advblock.Text = "Sample Sent. Awaiting response...";
            AdvancedButton.Visibility = Visibility.Collapsed;
            ContentDialog dialog = new ContentDialog ();

            // TODO: else display error dialogue
            if (SuccessStatusCode != 200) {
                /* call error dialogue code */
                advblock.Text = "Error Receiving Response";
                advprogressbar.Visibility = Visibility.Visible;
            }
            AdvancedButton = (Button) sender;
        }

        private async void PickAFileButton_Click (object sender, RoutedEventArgs e) {

            PickAFileOutputTextBlock.Text = "";
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);

            WinRT.Interop.InitializeWithWindow.Initialize (openPicker, hWnd);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add ("*");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) {
                PickAFileOutputTextBlock.Text = "Selected File : " + file.Name;
            } else {
                PickAFileOutputTextBlock.Text = "Operation Cancelled";
            }
        }

        private async void Error_Opened (ContentDialog sender, ContentDialogOpenedEventArgs args) {
            ContentDialogResult result = await ErrorContentDialog.ShowAsync();
            if (result == ContentDialogResult.Primary) { /* code to retry request */ } else { /* user pressed cancel, ESC, or back arrow */}
        }
    }
}
