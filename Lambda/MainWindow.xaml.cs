using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using Windows.Storage.Pickers;
using System.Runtime.InteropServices; // For DllImport
using System.Collections.Generic;
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
            } else {
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                _page = item.Page;
            }
            // get page type
            var preNavPageType = ContentFrame.CurrentSourcePageType;

            // only navigate if selected page isn't currently loaded
            if (!(_page is null) && !Type.Equals (preNavPageType, _page)) {
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
            if (item != null) {
                string tag = item.Tag.ToString();
                var pageItem = _pages.FirstOrDefault(p => p.Tag == tag);

                if (pageItem.Page == null) {
                    System.Diagnostics.Debug.WriteLine ($"No NavigationViewItem found for {tag}.");
                } else {
                    // Check if the page is already loaded
                    if (ContentFrame.Content?.GetType () != pageItem.Page) {
                        // Navigate to the requested page
                        ContentFrame.Navigate (pageItem.Page, item.Content);
                        NavView.Header = item.Content;
                    }
                }
            }
        }

        public List<NavigationViewItem> GetNavigationViewItems () {
            var result = new List<NavigationViewItem>();
            var items = NavView.MenuItems.OfType<NavigationViewItem>().ToList();
            items.AddRange (NavView.FooterMenuItems.OfType<NavigationViewItem>());
            result.AddRange (items);

            foreach (NavigationViewItem mainItem in items) {
                result.AddRange (mainItem.MenuItems.OfType<NavigationViewItem>());
            }

            return result;
        }

        public List<NavigationViewItem> GetNavigationViewItems (Type type) {
            return GetNavigationViewItems ().Where (i => i.Tag.ToString () == type.FullName).ToList ();
        }

        public List<NavigationViewItem> GetNavigationViewItems (Type type, string title) {
            return GetNavigationViewItems (type).Where (ni => ni.Content.ToString () == title).ToList ();
        }

        public NavigationViewItem GetCurrentNavigationViewItem () {
            return NavView.SelectedItem as NavigationViewItem;
        }

        public interface INavigation {
            NavigationViewItem GetCurrentNavigationViewItem ();

            List<NavigationViewItem> GetNavigationViewItems ();
            List<NavigationViewItem> GetNavigationViewItems (Type type);
            List<NavigationViewItem> GetNavigationViewItems (Type type, string title);

            void SetCurrentNavigationViewItem (NavigationViewItem item);
        }

        private void NavView_Loaded (object sender, RoutedEventArgs e) {
            // Reference: https://learn.microsoft.com/en-us/windows/apps/design/controls/navigationview
            //if (ContentFrame.Content == null) {
            //    ContentFrame.Navigate (typeof (AdvancedScanningPage), null, new DrillInNavigationTransitionInfo ());
            //}

            if (ContentFrame.Content == null) {
                ContentFrame.Navigate (typeof (HomePage), null, new DrillInNavigationTransitionInfo ());
            }

            var items = GetNavigationViewItems(typeof(HomePage));
            if (items.Any ()) // Check if the collection has any elements
            {
                SetCurrentNavigationViewItem (items.First ());
            } else {
                System.Diagnostics.Debug.WriteLine ("No NavigationViewItem found for HomePage.");
            }
        }

        private void ContentFrame_Navigated (object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e) {
            throw new NotImplementedException ();
        }

        private void NavView_ItemInvoked (NavigationView sender, NavigationViewItemInvokedEventArgs args) {

            if (args.IsSettingsInvoked) {
                // Navigate to the settings page
                ContentFrame.Navigate (typeof (SettingsPage));
                NavView.Header = "Settings";
            } else {
                // Get the invoked menu item
                NavigationViewItem menuItem = args.InvokedItemContainer as NavigationViewItem;

                if (menuItem != null) {
                    // Get the page type from the Tag property
                    string pageTypeName = menuItem.Tag.ToString();
                    Type pageType = FindTypeByName(pageTypeName);

                    if (pageType == null) {
                        System.Diagnostics.Debug.WriteLine ($"Could not find type for: {pageTypeName}");
                    } else {
                        // Check if the page is already loaded
                        if (ContentFrame.Content?.GetType () != pageType) {
                            // Navigate to the requested page
                            ContentFrame.Navigate (pageType);
                            NavView.Header = menuItem.Content;
                        }
                    }
                }
            }
        }

        // For debugging

        private Type FindTypeByName (string typeName) {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies ()) {
                var type = assembly.GetType(typeName);
                if (type != null) {
                    return type;
                }
            }

            return null;
        }

        // END: For Debugging
        private void NavigationView_SelectionChanged (NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
            SetCurrentNavigationViewItem (args.SelectedItemContainer as NavigationViewItem);
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

