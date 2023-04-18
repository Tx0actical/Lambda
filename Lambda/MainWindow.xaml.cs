using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.ObjectModel;
using Windows.Storage.Pickers;

namespace Lambda {

    public sealed partial class MainWindow : Window {
        private static int SuccessStatusCode = 200;
        public static bool CameFromToggle = false;
        public static bool CameFromGridChange = false;
        
        public XamlRoot XamlRoot { get; private set; }

        public MainWindow () {
            this.InitializeComponent ();
            ExtendsContentIntoTitleBar = true;
            Activated += MainWindow_Activated;
            SetTitleBar (AppTitleBar);
        }

        private void MainWindow_Activated (object sender, WindowActivatedEventArgs args) {
            if (args.WindowActivationState == WindowActivationState.Deactivated) {
                AppTitleTextBlock.Foreground =
                    (SolidColorBrush) App.Current.Resources["WindowCaptionForegroundDisabled"];
            } else {
                AppTitleTextBlock.Foreground =
                    (SolidColorBrush) App.Current.Resources["WindowCaptionForeground"];
            }
        }

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
                    ContentFrame.Navigate (typeof (AdvancedScanningPage),   null, new DrillInNavigationTransitionInfo ());
                    break;
                case "Scan History":
                    ContentFrame.Navigate (typeof (ScanHistoryPage),        null, new DrillInNavigationTransitionInfo ());
                    break;
                case "Results":
                    ContentFrame.Navigate (typeof (ResultsPage),            null, new DrillInNavigationTransitionInfo ());
                    break;
                case "Settings":
                    ContentFrame.Navigate (typeof (SettingsPage),           null, new DrillInNavigationTransitionInfo ());
                    break;
                case "About":
                    ContentFrame.Navigate (typeof (AboutPage),              null, new DrillInNavigationTransitionInfo ());
                    break;
                case "Account":
                    ContentFrame.Navigate (typeof (AccountPage),            null, new DrillInNavigationTransitionInfo ());
                    break;
            }
        }

        private async void Error_Opened (ContentDialog sender, ContentDialogOpenedEventArgs args) {
            ContentDialogResult result = await ErrorContentDialog.ShowAsync();
            if (result == ContentDialogResult.Primary) { /* code to retry request */ }
            else { /* user pressed cancel, ESC, or back arrow */}
        }

        public NavigationViewPaneDisplayMode ChoosePanePosition (bool toggleOn) {
            if (toggleOn) {
                return Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Left;
            } else {
                return Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Top;
            }
        }

        private void NavView_BackRequested (NavigationView sender, NavigationViewBackRequestedEventArgs args) {

        }
    }
}

