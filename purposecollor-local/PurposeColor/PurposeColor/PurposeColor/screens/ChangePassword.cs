﻿using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.screens
{
    public class ChangePassword : ContentPage
    {

        public ChangePassword(User userInfo)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            PurposeColorBlueSubTitleBar subTitleBar = null;
            subTitleBar = new PurposeColorBlueSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Change Password", true, false);

            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();

            PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;

            CustomEntry oldPaswordEntry = new CustomEntry
            {
                Placeholder = "Old Password"
            };

            CustomEntry paswordEntry = new CustomEntry
            {
                Placeholder = "Password"
            };

            CustomEntry confirmPaswordEntry = new CustomEntry
            {
                Placeholder = "Confirm Password"
            };


            Button submitButton = new Button
            {
                Text = "Submit",
                TextColor = Color.White,
                BorderColor = Constants.BLUE_BG_COLOR,
                BorderWidth = 2,
                BackgroundColor = Constants.BLUE_BG_COLOR
            };

            oldPaswordEntry.WidthRequest = deviceSpec.ScreenWidth * 80 / 100;
            paswordEntry.WidthRequest = deviceSpec.ScreenWidth * 80 / 100;
            confirmPaswordEntry.WidthRequest = deviceSpec.ScreenWidth * 80 / 100;
            submitButton.WidthRequest = deviceSpec.ScreenWidth * 40 / 100;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, 10);
            masterLayout.AddChildToLayout(oldPaswordEntry, 10, 25);
            masterLayout.AddChildToLayout(paswordEntry, 10, 33);
            masterLayout.AddChildToLayout(confirmPaswordEntry, 10, 41);
            masterLayout.AddChildToLayout(submitButton, 30, 50);

            submitButton.Clicked += OnSubmitButtonClicked;

            subTitleBar.NextButtonTapRecognizer.Tapped += (s, e) =>
            {
                OnSubmitButtonClicked(submitButton, null);
            };

            Content = masterLayout;
        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        void OnSubmitButtonClicked(object sender, EventArgs e)
        {

        }
    }
}
