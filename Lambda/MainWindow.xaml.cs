using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.ObjectModel;
using Windows.Storage.Pickers;
using WinRT;
using System.Runtime.InteropServices; // For DllImport
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using Windows.UI.Core;
using Windows.System;

namespace Lambda {

    class WindowsSystemDispatcherQueueHelper {
        [StructLayout (LayoutKind.Sequential)]
        struct DispatcherQueueOptions {
            internal int dwSize;
            internal int threadType;
            internal int apartmentType;
        }

        [DllImport ("CoreMessaging.dll")]
        private static extern int CreateDispatcherQueueController ([In] DispatcherQueueOptions options, [In, Out, MarshalAs (UnmanagedType.IUnknown)] ref object dispatcherQueueController);

        object m_dispatcherQueueController = null;
        public void EnsureWindowsSystemDispatcherQueueController () {
            if (Windows.System.DispatcherQueue.GetForCurrentThread () != null) {
                // one already exists, so we'll just use it.
                return;
            }

            if (m_dispatcherQueueController == null) {
                DispatcherQueueOptions options;
                options.dwSize = Marshal.SizeOf (typeof (DispatcherQueueOptions));
                options.threadType = 2;    // DQTYPE_THREAD_CURRENT
                options.apartmentType = 2; // DQTAT_COM_STA

                CreateDispatcherQueueController (options, ref m_dispatcherQueueController);
            }
        }
    }

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

        // Functions for Controls

        private void MainWindow_Activated (object sender, Microsoft.UI.Xaml.WindowActivatedEventArgs args) {
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

        // Navigation Helpers

        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)> {
            // Reference: https://learn.microsoft.com/en-us/windows/apps/design/controls/navigationview
            ("Tag_HomePage",                typeof(HomePage)),
            ("Tag_FileScanningPage",        typeof(AdvancedScanningPage)),
            ("Tag_ScanHistoryPage",         typeof(ScanHistoryPage)),
            ("Tag_ResultsPage",             typeof(ResultsPage)),
            ("Tag_AccountInformationPage",  typeof(AccountPage)),
            ("Tag_HelpPage",                typeof(HelpPage)),
            ("Tag_SettingsPage",            typeof(SettingsPage)),
        };

        private void NavView_Navigate (string navItemTag, NavigationTransitionInfo transitionInfo) {
            // Reference: https://learn.microsoft.com/en-us/windows/apps/design/controls/navigationview
            Type _page = null;
            if (navItemTag == "Tag_SettingsPage") {
                _page = typeof (SettingsPage);
            }
            else {
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                _page = item.Page;
            }
            // get page type
            var preNavPageType = ContentFrame.CurrentSourcePageType;

            // only navigate if selected page isn't currently loaded
            if (!(_page is null) && !Type.Equals(preNavPageType, _page)) {
                ContentFrame.Navigate (_page, null, new DrillInNavigationTransitionInfo ());
            }
        }

        private bool TryGoBack () {
            // Reference: https://learn.microsoft.com/en-us/windows/apps/design/controls/navigationview
            if (!ContentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed.
            if (NavView.IsPaneOpen &&
                (NavView.DisplayMode == NavigationViewDisplayMode.Compact ||
                 NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack ();
            return true;
        }

        private void CoreDispatcher_AcceleratorKeyActivated (CoreDispatcher sender, AcceleratorKeyEventArgs e) {
            // Reference: https://learn.microsoft.com/en-us/windows/apps/design/controls/navigationview
            // When Alt+Left are pressed navigate back
            if (e.EventType == CoreAcceleratorKeyEventType.SystemKeyDown
                && e.VirtualKey == VirtualKey.Left
                && e.KeyStatus.IsMenuKeyDown == true
                && !e.Handled) {
                e.Handled = TryGoBack ();
            }
        }

        private void CoreWindow_PointerPressed (CoreWindow sender, PointerEventArgs e) {
            // Handle mouse back button.
            if (e.CurrentPoint.Properties.IsXButton1Pressed) {
                e.Handled = TryGoBack ();
            }
        }

        private void System_BackRequested (object sender, BackRequestedEventArgs e) {
            if (!e.Handled) {
                e.Handled = TryGoBack ();
            }
        }

        public void SetCurrentNavigationViewItem (NavigationViewItem item) {
            if (item == null) {
                return;
            }

            if (item.Tag == null) {
                return;
            }

            ContentFrame.Navigate (
            Type.GetType (item.Tag.ToString ()),
            item.Content);
            NavView.Header = item.Content;
            NavView.SelectedItem = item;
        }

        private void NavView_Loaded (object sender, RoutedEventArgs e) {
            // Reference: https://learn.microsoft.com/en-us/windows/apps/design/controls/navigationview
            
        }

        private void ContentFrame_Navigated (object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e) {
            throw new NotImplementedException ();
        }

        private void NavView_ItemInvoked (NavigationView sender, NavigationViewItemInvokedEventArgs args) {

        }

        private void NavigationView_SelectionChanged (NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
            var SelectedItem = (NavigationViewItem) args.SelectedItem;
            string SelectedItemTag = (string) SelectedItem.Tag;
            switch (SelectedItemTag) {
                case "Home":
                    ContentFrame.Navigate (typeof (HomePage),               null, new DrillInNavigationTransitionInfo ());
                    break;
                case "Advanced Scanning":
                    ContentFrame.Navigate (typeof (AdvancedScanningPage),   null, new DrillInNavigationTransitionInfo ());
                    break;
                case "Scan History":
                    ContentFrame.Navigate (typeof (ScanHistoryPage),        null, new DrillInNavigationTransitionInfo ());
                    break;
                case "Results":
                    ContentFrame.Navigate (typeof (ResultsPage),            null, new DrillInNavigationTransitionInfo ());
                    break;
                case "Account":
                    ContentFrame.Navigate (typeof (AccountPage),            null, new DrillInNavigationTransitionInfo ());
                    break;
                case "Help":
                    ContentFrame.Navigate (typeof (HelpPage),               null, new DrillInNavigationTransitionInfo ());
                    break;
                case "Settings":
                    ContentFrame.Navigate (typeof (SettingsPage),           null, new DrillInNavigationTransitionInfo ());
                    break;

            }
        }

        private async void Error_Opened (ContentDialog sender, ContentDialogOpenedEventArgs args) {
            ContentDialogResult result = await ErrorContentDialog.ShowAsync();
            if (result == ContentDialogResult.Primary) { /* code to retry request */ } else { /* user pressed cancel, ESC, or back arrow */}
        }

        public NavigationViewPaneDisplayMode ChoosePanePosition (bool toggleOn) {
            if (toggleOn) {
                return Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Left;
            } else {
                return Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Top;
            }
        }

        private void NavView_BackRequested (NavigationView sender, NavigationViewBackRequestedEventArgs args) {
            TryGoBack ();
        }
    }
}

