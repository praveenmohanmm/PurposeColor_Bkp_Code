﻿using CustomControls;
using ImageCircle.Forms.Plugin.Abstractions;
using PurposeColor.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

namespace PurposeColor.CustomControls
{
    public class PurposeColorBlueSubTitleBar : ContentView, IDisposable
    {
        CustomLayout masterLayout;
        public TapGestureRecognizer BackButtonTapRecognizer;
        public TapGestureRecognizer NextButtonTapRecognizer;
        public CustomImageButton NextButton;
        Label title;
        double screenHeight;
        double screenWidth;

        public PurposeColorBlueSubTitleBar(Color backGroundColor, string titleValue, bool nextButtonVisible = true, bool backButtonVisible = true)
        {
            screenHeight = App.screenHeight;
            screenWidth = App.screenWidth;
            int titlebarHeight = (int)screenHeight * 7 / 100;
            int titlebarWidth = (int)screenWidth;
            this.BackgroundColor = backGroundColor;

            masterLayout = new CustomLayout();
            masterLayout.HeightRequest = titlebarHeight;
            masterLayout.WidthRequest = titlebarWidth;
            masterLayout.BackgroundColor = backGroundColor;

            Image bgImage = new Image();
            bgImage.Source = Device.OnPlatform("top_bg.png", "light_blue_bg.png", "//Assets//light_blue_bg.png");
            //  bgImage.WidthRequest = screenWidth;
            // bgImage.HeightRequest = titlebarHeight;
            bgImage.Aspect = Aspect.Fill;

            Image backArrow = new Image();
            backArrow.Source = Device.OnPlatform("arrow_blue.png", "arrow_blue.png", "//Assets//arrow_blue.png");
            // backArrow.HeightRequest = screenHeightv * 4 / 100;
            // backArrow.WidthRequest = screenWidth * 5 / 100;
            BackButtonTapRecognizer = new TapGestureRecognizer();
            backArrow.GestureRecognizers.Add(BackButtonTapRecognizer);

            

            title = new Label();
            title.Text = titleValue;
            title.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            title.FontSize = Device.OnPlatform(17, 20, 22);
            title.TextColor = Color.FromRgb( 30, 127, 210 );

            Image imgDivider = new Image();
            imgDivider.Source = Device.OnPlatform("icn_seperate.png", "icn_seperate.png", "//Assets//top_seperate.png");
            if (Device.OS != TargetPlatform.iOS)
            {
                masterLayout.AddChildToLayout(imgDivider, 83, Device.OnPlatform(26, 26, 22), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            }
            

            Image nextImage = new Image();
            nextImage.Source = Device.OnPlatform("tick_blue.png", "tick_blue.png", "//Assets//tick_blue.png");
            NextButtonTapRecognizer = new TapGestureRecognizer();
            nextImage.GestureRecognizers.Add(NextButtonTapRecognizer);

            if (Device.OS == TargetPlatform.WinPhone)
            {
                backArrow.HeightRequest = screenHeight * 4 / 100;
                backArrow.WidthRequest = screenWidth * 8 / 100;

                nextImage.HeightRequest = screenHeight * 6 / 100;
                nextImage.WidthRequest = screenWidth * 10 / 100;

                imgDivider.HeightRequest = screenHeight * 4 / 100;
            }
         

            //masterLayout.AddChildToLayout(title, 20, 18, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(title, Device.OnPlatform(20, 20, 28), Device.OnPlatform(18, 18, 32), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            if (nextButtonVisible)
            {
                StackLayout touchArea = new StackLayout();
                touchArea.WidthRequest = 100;
                touchArea.HeightRequest = screenHeight * 8 / 100;
                touchArea.BackgroundColor = Color.Transparent;
                touchArea.GestureRecognizers.Add(NextButtonTapRecognizer);
                masterLayout.AddChildToLayout(nextImage, Device.OnPlatform(88, 89, 85), Device.OnPlatform(30, 10, 8), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
                masterLayout.AddChildToLayout(touchArea, Device.OnPlatform(86, 80, 75), Device.OnPlatform(10, 2, 8), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            }

            if (backButtonVisible)
            {
                masterLayout.AddChildToLayout(backArrow, 5, Device.OnPlatform(25, 25, 20), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            }

            Content = masterLayout;

        }

        public void Dispose()
        {
            masterLayout = null;
            BackButtonTapRecognizer = null;
            NextButton = null;
            title = null;
            GC.Collect();
        }

    }
}
