// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

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
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Lambda
{
    public sealed partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            this.InitializeComponent();
            StartAnimation();
        }

        private void StartAnimation () {
            // Create a Storyboard to hold the animations
            var storyboard = new Storyboard();

            // Create a DoubleAnimation for the opacity
            var opacityAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(3)),
                EnableDependentAnimation = true,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            // Add the animation to the Storyboard
            storyboard.Children.Add (opacityAnimation);

            // Set the target of the animation to the LogoImage element
            Storyboard.SetTarget (opacityAnimation, LogoImage);

            // Set the target property of the animation to the Opacity property
            Storyboard.SetTargetProperty (opacityAnimation, "Opacity");

            // Start the animation
            storyboard.Begin ();
        }
    }
}
