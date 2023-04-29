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
using System.Net.Http;
using System.Runtime.InteropServices;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Hosting;
using WinRT.Interop;



namespace Lambda
{
    /// <summary>
    /// Page displaying the advanced scanning options.
    /// </summary>

    public sealed partial class AdvancedScanningPage : Page
    {
        public static bool CameFromToggle = false;
        public static bool CameFromGridChange = false;
        public string selectedFilePath;

        public AdvancedScanningPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private async void AdvButton_Click (object sender, RoutedEventArgs e) {
            AdvancedButton.Visibility = Visibility.Collapsed;
            string apiKey = Environment.GetEnvironmentVariable("LAMBDA_ACCOUNT_API_KEY");
            HttpClient httpClient = new HttpClient();
            APIOperationsHandler apiHandler = new APIOperationsHandler(httpClient, apiKey);
            advprogressbar.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty (selectedFilePath)) {
                HttpResponseMessage response = await apiHandler.UploadFileAsync(selectedFilePath);

                if (response.IsSuccessStatusCode) {
                    advprogressbar.Visibility = Visibility.Visible;
                    ShowCustomDialog (AdvancedButton, "Request sent successfully", "The request was sent successfully, and the server responded with a status code of " + response.StatusCode);
                } else {
                    advprogressbar.Visibility = Visibility.Visible;
                    advprogressbar.ShowError = true;
                    ShowCustomDialog (AdvancedButton, "Request failed", "The request failed, and the server responded with a status code of " + response.StatusCode);
                }
            } else {
                ShowCustomDialog (AdvancedButton, "No file selected", "Please select a file to upload before clicking the button.");
            }

            AdvancedButton = (Button) sender;
        }


        private async void PickObjectButton_Click (object sender, RoutedEventArgs e) {
            var openPicker = new FileOpenPicker()
    {
                ViewMode = PickerViewMode.Thumbnail,
                FileTypeFilter = { "*" },
            };

            // Get the Window
            Window window = Window.Current;
            IntPtr hWnd = WindowNative.GetWindowHandle(window);

            // Initialize Window
            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            WinRT.Interop.InitializeWithWindow.Initialize (openPicker, hWnd);

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) {
                PickAFileOutputTextBlock.Text = "Selected File : " + file.Name;
                selectedFilePath = file.Path; // Store the selected file path to use in the AdvButton_Click method
            } else {
                PickAFileOutputTextBlock.Text = "Operation Cancelled";
            }
        }

        private async void ShowCustomDialog (FrameworkElement element, string title, string message) {
            // Create a new ContentDialog instance
            ContentDialog customDialog = new ContentDialog {
                XamlRoot = element.XamlRoot,
                Title = title,
                Content = message,
                CloseButtonText = "Cancel"
                
                
            };

            // Handle the PrimaryButtonClick event
            customDialog.PrimaryButtonClick += (sender, args) =>
            {
                // Perform actions when the PrimaryButton is clicked
            };

            // Handle the SecondaryButtonClick event
            customDialog.SecondaryButtonClick += (sender, args) =>
            {
                // Perform actions when the SecondaryButton is clicked
            };

            // Show the ContentDialog
            ContentDialogResult result = await customDialog.ShowAsync();

            // Handle the result of the ContentDialog
            switch (result) {
                case ContentDialogResult.Primary:
                    // Perform actions for PrimaryButton
                    break;
                case ContentDialogResult.Secondary:
                    // Perform actions for SecondaryButton
                    break;
                case ContentDialogResult.None:
                    // Perform actions for CloseButton or when the user cancels the dialog
                    break;
            }
        }



        private void CustomDialogCloseButton_Click(object sender, RoutedEventArgs e)
        {
            CustomDialogOverlay.Visibility = Visibility.Collapsed;
        }
    }
}
