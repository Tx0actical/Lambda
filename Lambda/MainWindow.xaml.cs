using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static System.Net.WebRequestMethods;


namespace Lambda {
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainWindow : Window {
        private static int _clicks = 0;
        public bool _response = false;

        public MainWindow () {
            this.InitializeComponent ();
        }

        private void AdvButton_Click (object sender, RoutedEventArgs e) {
            advblock.Text = "Sample Sent. Awaiting response...";
            AdvancedButton.Visibility = Visibility.Collapsed;
            _clicks++;
            if (_clicks == 1) {
                advblock.Text = "Error Receiveing Response";
                advprogressbar.Visibility = Visibility.Visible;
            }
        }

        private void ContentFrame_NavigationFailed (object sender, NavigationFailedEventArgs e) {
            throw new Exception ("Failed to load Page " + e.SourcePageType.FullName);
        }
        
        private void NavView_ItemInvoked (NavigationView sender, NavigationViewItemInvokedEventArgs e) {
            
        }

        private void NavView_Loaded (object sender, RoutedEventArgs e) {

        }
    }
}
