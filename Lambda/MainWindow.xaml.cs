using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using Windows.Storage.Pickers;
using static System.Net.WebRequestMethods;


namespace Lambda {

    public sealed partial class MainWindow : Window {
        private static int _clicks = 0;
        public int _success_response_status_code = 200;

        public MainWindow () {
            this.InitializeComponent ();
        }

        private void AdvButton_Click (object sender, RoutedEventArgs e) {
            advblock.Text = "Sample Sent. Awaiting response...";
            AdvancedButton.Visibility = Visibility.Collapsed;
            _clicks++;
            if (_clicks == 1) {
                advblock.Text = "Error Receiving Response";
                advprogressbar.Visibility = Visibility.Visible;
            }
            AdvancedButton = (Button) sender;

        }

        private void ContentFrame_NavigationFailed (object sender, NavigationFailedEventArgs e) {
            throw new Exception ("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void NavView_ItemInvoked (NavigationView sender, NavigationViewItemInvokedEventArgs e) {

        }

        private void NavView_Loaded (object sender, RoutedEventArgs e) {

        }

        private async void PickAFileButton_Click (object sender, RoutedEventArgs e) {
            // Clear previous returned file name, if it exists, between iterations of this scenario
            PickAFileOutputTextBlock.Text = "";

            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);


            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize (openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add ("*");

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            if (file != null) {
                PickAFileOutputTextBlock.Text = "Selected File : " + file.Name;
            } else {
                PickAFileOutputTextBlock.Text = "Operation cancelled";
            }

        }
    }
}

