﻿using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using System;
using System.Threading;
using System.Threading.Tasks;
using PurposeColor.screens;
using PurposeColor.Service;
using System.Diagnostics;
using PurposeColor.interfaces;

namespace PurposeColor
{

    public class FeelingNowPage : ContentPage, IDisposable
    {
        CustomSlider slider;
        CustomPicker ePicker;
        CustomLayout masterLayout;
        IDeviceSpec deviceSpec;
        PurposeColor.interfaces.CustomImageButton emotionalPickerButton;
        PurposeColor.interfaces.CustomImageButton eventPickerButton;
        CustomListViewItem selectedEmotionItem;
        CustomListViewItem selectedEventItem;
        Label about;
        public static int sliderValue;
        public FeelingNowPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb( 244, 244, 244 );
            deviceSpec = DependencyService.Get<IDeviceSpec>();

            PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;
            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Emotional Awareness");
            subTitleBar.NextButtonTapRecognizer.Tapped += OnNextButtonTapRecognizerTapped;
            subTitleBar.BackButtonTapRecognizer.Tapped += OnBackButtonTapRecognizerTapped;


            slider = new CustomSlider
            {
                Minimum = -2,
                Maximum = 2,
                WidthRequest = deviceSpec.ScreenWidth * 90 / 100
            };
            slider.StopGesture = GetstopGetsture;

            //slider.ValueChanged += slider_ValueChanged;


            Label howYouAreFeeling = new Label();
            howYouAreFeeling.Text = Constants.HOW_YOU_ARE_FEELING;
            howYouAreFeeling.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            howYouAreFeeling.TextColor = Color.FromRgb(40, 47, 50);
            howYouAreFeeling.FontSize = Device.OnPlatform( 20, 22, 30 );
            howYouAreFeeling.WidthRequest = deviceSpec.ScreenWidth * 70 / 100;
            howYouAreFeeling.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;


            Label howYouAreFeeling2 = new Label();
            howYouAreFeeling2.Text = "feeling now ?";
            howYouAreFeeling2.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            howYouAreFeeling2.TextColor = Color.FromRgb( 40, 47, 50 );
            howYouAreFeeling2.FontSize = Device.OnPlatform( 20, 22, 30 );
            howYouAreFeeling2.WidthRequest = deviceSpec.ScreenWidth * 70 / 100;
            howYouAreFeeling2.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;




            emotionalPickerButton = new PurposeColor.interfaces.CustomImageButton();
            emotionalPickerButton.ImageName = "select_box_whitebg.png";
            emotionalPickerButton.Text = "Select Emotion";
            emotionalPickerButton.FontSize = 18;
            emotionalPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            emotionalPickerButton.TextOrientation = interfaces.TextOrientation.Left;
            emotionalPickerButton.TextColor = Color.Gray;
            emotionalPickerButton.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            emotionalPickerButton.HeightRequest = deviceSpec.ScreenHeight * 8 / 100;
            emotionalPickerButton.Clicked += OnEmotionalPickerButtonClicked;

            eventPickerButton = new PurposeColor.interfaces.CustomImageButton();
            eventPickerButton.IsVisible = false;
            eventPickerButton.ImageName = "select_box_whitebg.png";
            eventPickerButton.Text = "Events, Situation & Thoughts";
            eventPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            eventPickerButton.TextOrientation = interfaces.TextOrientation.Left;
            eventPickerButton.FontSize = 18;
            eventPickerButton.TextColor = Color.Gray;
            eventPickerButton.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            eventPickerButton.HeightRequest = deviceSpec.ScreenHeight * 8 / 100;
            eventPickerButton.Clicked += OnEventPickerButtonClicked;



            about = new Label();
            about.IsVisible = false;
            about.Text = "About";
            about.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            about.TextColor = Color.Gray;
            about.FontSize = Device.OnPlatform(20, 16, 30);
            about.WidthRequest = deviceSpec.ScreenWidth * 50 / 100;
            about.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;
            about.HorizontalOptions = LayoutOptions.Center;

            Image sliderDivider1 = new Image();
            sliderDivider1.Source = "drag_sepeate.png";
            //bgImage.Source = Device.OnPlatform("top_bg.png", "light_blue_bg.png", "//Assets//light_blue_bg.png");


            Image sliderDivider2 = new Image();
            sliderDivider2.Source = "drag_sepeate.png";


            Image sliderDivider3 = new Image();
            sliderDivider3.Source = "drag_sepeate.png";

            Image sliderBG = new Image();
            sliderBG.Source = "drag_bg.png";

            this.Appearing += OnFeelingNowPageAppearing;


            sliderValue = slider.CurrentValue;
            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 10));
            masterLayout.AddChildToLayout(howYouAreFeeling, 15, 22);
            masterLayout.AddChildToLayout(howYouAreFeeling2, 28, 27);
            masterLayout.AddChildToLayout(sliderBG, 7, 40);
            masterLayout.AddChildToLayout(slider, 5, 34);

            masterLayout.AddChildToLayout(sliderDivider1, 30, 40.5f);
            masterLayout.AddChildToLayout(sliderDivider2, 50, 40.5f);
            masterLayout.AddChildToLayout(sliderDivider3, 70, 40.5f);

            masterLayout.AddChildToLayout(emotionalPickerButton, 5, 50);
            masterLayout.AddChildToLayout(about, 5, 65);
            masterLayout.AddChildToLayout(eventPickerButton, 5, 70);

            Content = masterLayout;

        }

        public async void GetstopGetsture( bool pressed )
        {
           /* if (slider.Value != 0)
            {
                View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                if (pickView != null)
                    return;

                CustomPicker ePicker = new CustomPicker(masterLayout, App.GetEmotionsList(), 65, "Select Emotions", true, false);
                ePicker.WidthRequest = deviceSpec.ScreenWidth;
                ePicker.HeightRequest = deviceSpec.ScreenHeight;
                ePicker.ClassId = "ePicker";
                ePicker.listView.ItemSelected += OnEmotionalPickerItemSelected;
                masterLayout.AddChildToLayout(ePicker, 0, 0);
            }*/

            sliderValue = slider.CurrentValue;
            if( slider.CurrentValue == 0 )
            {
                IProgressBar progress = DependencyService.Get<IProgressBar>();
                progress.ShowToast( "slider is in neutral" );
            }
            else
            {
                OnEmotionalPickerButtonClicked(emotionalPickerButton, EventArgs.Empty);
            }


        }

        void slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if( slider.Value != 0 )
            {
                View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
                if (pickView != null)
                    return;
  
                CustomPicker ePicker = new CustomPicker(masterLayout, App.GetEmotionsList(), 65, "Select Emotions", true, false);
                ePicker.WidthRequest = deviceSpec.ScreenWidth;
                ePicker.HeightRequest = deviceSpec.ScreenHeight;
                ePicker.ClassId = "ePicker";
                ePicker.listView.ItemSelected += OnEmotionalPickerItemSelected;
                masterLayout.AddChildToLayout(ePicker, 0, 0);
            }

        }

        protected override bool OnBackButtonPressed()
        {
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            if( pickView != null )
            {
                masterLayout.Children.Remove(pickView);
                pickView = null;
                return true;
            }

            return base.OnBackButtonPressed();
        }

        void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        async void OnNextButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {
            if(  emotionalPickerButton.Text == "Select Emotion")
            {
                DisplayAlert("Purpose Color", "Emotion not selected.", "Ok");
            }
            else if (eventPickerButton.Text == "Events, Situation & Thoughts")
            {
                DisplayAlert("Purpose Color", "Event not selected.", "Ok");
            }
            else if( slider.Value == 0 )
            {
                DisplayAlert("Purpose Color", "Feelings slider is in Neutral", "Ok");
            }
            else
            {
                IProgressBar progress = DependencyService.Get<IProgressBar>();
                progress.ShowProgressbar("Saving Details..");
                await ServiceHelper.SaveEmotionAndEvent(selectedEmotionItem.EmotionID, selectedEventItem.EventID, "2");
                progress.HideProgressbar();
                Navigation.PushAsync(new FeelingsSecondPage());
            }
            
        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        void backButton_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync( new GraphPage() );
        }

        void OnEmotionalPickerButtonClicked(object sender, System.EventArgs e)
        {
            if( App.emotionsListSource == null || App.emotionsListSource.Count <= 0 )
            {
                IProgressBar progress = DependencyService.Get<IProgressBar>();
                progress.ShowToast("emotions empty");
                return;
            }
            List<CustomListViewItem> pickerSource = App.emotionsListSource.Where(toAdd => toAdd.SliderValue == slider.CurrentValue).ToList();
            CustomPicker ePicker = new CustomPicker(masterLayout, pickerSource, 65, Constants.SELECT_EMOTIONS, true, true);
            ePicker.WidthRequest = deviceSpec.ScreenWidth;
            ePicker.HeightRequest = deviceSpec.ScreenHeight;
            ePicker.ClassId = "ePicker";
            ePicker.FeelingsPage = this;
            ePicker.listView.ItemSelected += OnEmotionalPickerItemSelected;
            masterLayout.AddChildToLayout(ePicker, 0, 0);

            //double yPos = 60 * deviceSpec.ScreenHeight / 100;
           // ePicker.TranslateTo(0, -yPos, 250, Easing.BounceIn);
           // ePicker.FadeTo(1, 750, Easing.Linear); 
        }

        void OnEventPickerButtonClicked(object sender, System.EventArgs e)
        {

            CustomPicker ePicker = new CustomPicker(masterLayout,App.GetEventsList(), 50, Constants.ADD_EVENTS, true, true);
            ePicker.WidthRequest = deviceSpec.ScreenWidth;
            ePicker.HeightRequest = deviceSpec.ScreenHeight;
            ePicker.ClassId = "ePicker";
            ePicker.listView.ItemSelected += OnEventPickerItemSelected;
            masterLayout.AddChildToLayout(ePicker, 0, 0);
            //double yPos = 60 * deviceSpec.ScreenHeight / 100;
            //ePicker.TranslateTo(0, yPos, 250, Easing.BounceIn);
           // ePicker.FadeTo(1, 750, Easing.Linear); 
        }

        void OnEmotionalPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
           CustomListViewItem item = e.SelectedItem as CustomListViewItem;
           emotionalPickerButton.Text = item.Name;
           selectedEmotionItem = item;
           emotionalPickerButton.TextColor = Color.Black;
           App.SelectedEmotion = item.Name;
           View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
           masterLayout.Children.Remove( pickView );
           pickView = null;
           eventPickerButton.IsVisible = true;
           about.IsVisible = true;


           OnEventPickerButtonClicked(eventPickerButton, EventArgs.Empty);
     
        }

        void OnEventPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CustomListViewItem item = e.SelectedItem as CustomListViewItem;
            eventPickerButton.Text = item.Name;
            eventPickerButton.TextColor = Color.Black;
            selectedEventItem = item;
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            masterLayout.Children.Remove(pickView);
            pickView = null;
        }

        void emotionPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        async  void OnFeelingNowPageAppearing(object sender, System.EventArgs e)
        {
            IProgressBar progressBar = DependencyService.Get<IProgressBar>();
            if (App.emotionsListSource == null || App.emotionsListSource.Count < 1)
            {
                progressBar.ShowProgressbar("Loading emotions...");
                var downloadEmotionStatus = await DownloadAllEmotions();
                if( !downloadEmotionStatus )
                {
                    DisplayAlert("Purpose Color", "Netwrok error occured.", "Ok");
                    progressBar.HideProgressbar();
                    return;
                }
                App.Settings.SaveEmotions(App.emotionsListSource);
                progressBar.HideProgressbar();

                // for testing
                var testEmotions = App.Settings.GetAllEmotions();
            }

            if (App.eventsListSource == null || App.eventsListSource.Count < 1)
            {
                await DownloadAllEvents();
                App.Settings.SaveEvents(App.eventsListSource);

                // for testing
                var testEvents = App.Settings.GetAllEvents();
            }
        }

        public async Task<bool> DownloadAllEmotions()
        {
            var emotionsReult = await ServiceHelper.GetAllEmotions(2);
            if( emotionsReult != null )
            {
                App.emotionsListSource = null;
                App.emotionsListSource = emotionsReult;
                return true;
            }
            return false;

        }

        private async Task<bool> DownloadAllEvents()
        {
            try
            {
                var eventList = await ServiceHelper.GetAllEvents();
                if (eventList != null)
                {
                    App.eventsListSource = null;
                    App.eventsListSource = new List<CustomListViewItem>();
                    foreach (var item in eventList)
                    {
                        App.eventsListSource.Add(item);
                    }
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            

            return true;
        }

        public void Dispose()
        {
            slider = null;
            ePicker = null;
            masterLayout = null;
            deviceSpec = null;
            emotionalPickerButton = null;
            eventPickerButton = null;
        }
    }
}
