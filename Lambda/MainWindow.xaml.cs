using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;

namespace Lambda {

    public sealed partial class MainWindow : Window {
        private static int _clicks = 0;
        public static bool CameFromToggle = false;
        public static bool CameFromGridChange = false;

        public MainWindow () {
            this.InitializeComponent ();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar (AppTitleBar);
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
                PickAFileOutputTextBlock.Text = "Operation cancelled";
            }
        }

        private void NavView_ItemInvoked (NavigationView sender, NavigationViewItemInvokedEventArgs args) {

        }

        private void NavigationView_SelectionChanged (NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
                var SelectedItem = (NavigationViewItem) args.SelectedItem;
                string SelectedItemTag = (string) SelectedItem.Tag;
                switch (SelectedItemTag) {
                    case "Advanced Scanning":
                        ContentFrame.Navigate (typeof (AdvancedScanningPage));
                        break;
                    case "Scan History":
                        ContentFrame.Navigate (typeof (ScanHistoryPage));
                        break;
                    case "Results":
                        ContentFrame.Navigate (typeof (ResultsPage));
                        break;
                    case "Settings":
                        ContentFrame.Navigate (typeof (SettingsPage));
                        break;
                    case "About":
                        ContentFrame.Navigate (typeof (AboutPage));
                        break;
                    case "Account":
                        ContentFrame.Navigate (typeof (AccountPage));
                        break;
                }
        }
    }
}

