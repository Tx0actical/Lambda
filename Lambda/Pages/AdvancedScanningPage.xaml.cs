using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using Windows.Storage.Pickers;
using Program; // local namespace for APIHandler.cs
using System.Net.Http;
using WinRT.Interop;
using System.Text.Json;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Documents;

namespace Lambda {
    /// <summary>
    /// Page displaying the advanced scanning options.
    /// </summary>

    public sealed partial class AdvancedScanningPage : Page
    {
        
        public string __selectedFilePath;
        private readonly DispatcherQueue __dispatcherQueue = DispatcherQueue.GetForCurrentThread();


        public AdvancedScanningPage ()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private async void AdvButton_Click (object sender, RoutedEventArgs e) {

            AdvancedButton.Visibility = Visibility.Collapsed;
            string APIKey = Environment.GetEnvironmentVariable("LAMBDA_ACCOUNT_API_KEY");
            HttpClient httpClient = new HttpClient();
            APIOperationsHandler APIHandler = new APIOperationsHandler(httpClient, APIKey);
            advprogressbar.Visibility = Visibility.Visible;

            if (!string.IsNullOrEmpty (__selectedFilePath)) {
                var APIResponse = await APIHandler.UploadFileAsync(__selectedFilePath);

                if (APIResponse != null && APIResponse.Data != null) {
                    var scanResult = await APIHandler.GetScanResultsAsync(APIResponse.Data.Id);

                    if (scanResult != null && scanResult.Data != null) {

                        var completedScanResults = await APIHandler.WaitForScanCompletionAsync(scanResult.Data.Id);

                        // Update UI elements on the UI thread
                        __dispatcherQueue.TryEnqueue (() => {

                            IdResponseTextBlock.Text = scanResult.Data.Id;
                            TypeResponseTextBlock.Text = scanResult.Data.Type;
                            string output = $"Scan results:\n{JsonSerializer.Serialize (scanResult.Data, new JsonSerializerOptions { WriteIndented = true })}";
                            OutputTextBox.Text = output;
                            OutputTextBox.Visibility = Visibility.Visible;

                        });

                    }
                }

                System.Diagnostics.Debug.WriteLine ("IsSuccessStatusCode: " + APIResponse.IsSuccessStatusCode);

                if (APIResponse != null && APIResponse.Data != null) {

                    IdResponseTextBlock.Text = "API response ID : " + APIResponse.Data.Id;
                    TypeResponseTextBlock.Text = "API response type : " + APIResponse.Data.Type;
                    
                } else {
                    System.Diagnostics.Debug.WriteLine ("APIResponse or APIResponse.Data is null");
                }

                if (APIResponse.IsSuccessStatusCode) {
                    advprogressbar.Visibility = Visibility.Collapsed;
                    // Update UI elements on the UI thread
                    
                    __dispatcherQueue.TryEnqueue (() => {

                        advprogressbar.Visibility = Visibility.Collapsed;
                        ShowCustomDialog (AdvancedButton, "Request sent successfully", $"The request was sent successfully, and the API responded with a status code: {APIResponse.StatusCode}");

                    });
                } else {
                    // Update UI elements on the UI thread
                    __dispatcherQueue.TryEnqueue (() => {
                        advprogressbar.Visibility = Visibility.Collapsed;
                        advprogressbar.IsIndeterminate = true;
                        ShowCustomDialog (AdvancedButton, "Request failed", $"The request failed, and the API responded with a status code: {APIResponse.StatusCode}");
                    });
                }
            } else {
                ShowCustomDialog (AdvancedButton, "No file selected", "Please select a file to upload before clicking the button.");
            }
            AdvancedButton = (Button) sender;
        }

        public IntPtr GetWindowHandle (Window window) {

            return WindowNative.GetWindowHandle (window);

        }

        private async void PickObjectButton_Click (object sender, RoutedEventArgs e) {

            PickAFileOutputTextBlock.Text = "";
            var openPicker = new FileOpenPicker() {
                ViewMode = PickerViewMode.Thumbnail,
                FileTypeFilter = { "*" },
            };
            var currentWindow = ((App)Application.Current).Window;
            Window window = Window.Current;
            IntPtr hWnd = GetWindowHandle(currentWindow);

            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            WinRT.Interop.InitializeWithWindow.Initialize (openPicker, hWnd);

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) {
                PickAFileOutputTextBlock.Text = "Selected File : " + file.Name;
                __selectedFilePath = file.Path; // Store the selected file path to use in the AdvButton_Click method
            } else {
                PickAFileOutputTextBlock.Text = "Operation Cancelled";
            }
        }

        private async void ShowCustomDialog (FrameworkElement element, string title, string message) {
            
            ContentDialog customDialog = new ContentDialog {
                XamlRoot = this.XamlRoot,
                Title = title,
                Content = message,
                CloseButtonText = "Ok"
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
