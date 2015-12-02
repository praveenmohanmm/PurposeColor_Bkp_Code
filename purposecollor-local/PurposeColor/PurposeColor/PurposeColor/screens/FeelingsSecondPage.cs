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
using PurposeColor.interfaces;
using PurposeColor.Service;
using System.Collections.ObjectModel;
using PurposeColor.Model;

namespace PurposeColor
{

    public class FeelingsSecondPage : ContentPage, IDisposable
    {
        CustomPicker ePicker;
        CustomLayout masterLayout;
        IDeviceSpec deviceSpec;
        PurposeColor.interfaces.CustomImageButton actionPickerButton;
        PurposeColor.interfaces.CustomImageButton goalsAndDreamsPickerButton;
        public static ObservableCollection<PreviewItem> actionPreviewListSource = new ObservableCollection<PreviewItem>();

        public FeelingsSecondPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
            deviceSpec = DependencyService.Get<IDeviceSpec>();
            this.Appearing += FeelingsSecondPage_Appearing;
            actionPreviewListSource = new ObservableCollection<PreviewItem>();


            PurposeColorTitleBar mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", false);
            mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;

            PurposeColorSubTitleBar subTitleBar = new PurposeColorSubTitleBar(Constants.SUB_TITLE_BG_COLOR, "Emotional Awareness");
            subTitleBar.BackButtonTapRecognizer.Tapped += OnBackButtonTapRecognizerTapped;
            subTitleBar.NextButtonTapRecognizer.Tapped += NextButtonTapRecognizer_Tapped;


            Label firstLine = new Label();
            firstLine.Text = "Does being";
            firstLine.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            firstLine.TextColor = Color.FromRgb(40, 47, 50);
            firstLine.FontSize = Device.OnPlatform(20, 22, 30);
            firstLine.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;
            firstLine.HorizontalOptions = LayoutOptions.Center;
            firstLine.WidthRequest = deviceSpec.ScreenWidth;
            firstLine.XAlign = TextAlignment.Center;


            Label secondLine = new Label();
            secondLine.Text = App.SelectedEmotion;
            secondLine.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            secondLine.TextColor = Color.FromRgb(40, 47, 50);
            secondLine.FontSize = Device.OnPlatform(20, 22, 30);
            secondLine.HeightRequest = deviceSpec.ScreenHeight * 15 / 100;
            secondLine.HorizontalOptions = LayoutOptions.Center;
            secondLine.WidthRequest = deviceSpec.ScreenWidth;
            secondLine.XAlign = TextAlignment.Center;



            Label thirdLine = new Label();
            thirdLine.Text = "support your goals and dreams?";
            thirdLine.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            thirdLine.TextColor = Color.FromRgb(40, 47, 50);
            thirdLine.FontSize = Device.OnPlatform(20, 22, 30);
            thirdLine.HeightRequest = deviceSpec.ScreenHeight * 10 / 100;
            thirdLine.HorizontalOptions = LayoutOptions.Center;
            thirdLine.WidthRequest = deviceSpec.ScreenWidth;
            thirdLine.XAlign = TextAlignment.Center;


            goalsAndDreamsPickerButton = new PurposeColor.interfaces.CustomImageButton();
            goalsAndDreamsPickerButton.ImageName = "select_box_whitebg.png";
            goalsAndDreamsPickerButton.Text = "Goals & Dreams";
            goalsAndDreamsPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            goalsAndDreamsPickerButton.TextOrientation = interfaces.TextOrientation.Left;
            goalsAndDreamsPickerButton.FontSize = 18;
            goalsAndDreamsPickerButton.TextColor = Color.Gray;
            goalsAndDreamsPickerButton.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            goalsAndDreamsPickerButton.HeightRequest = deviceSpec.ScreenHeight * 8 / 100;
            goalsAndDreamsPickerButton.Clicked += OnGoalsPickerButtonClicked;


            actionPickerButton = new CustomImageButton();
            actionPickerButton.IsVisible = false;
            actionPickerButton.BackgroundColor = Color.FromRgb(30, 126, 210);
            actionPickerButton.Text = "Add Supporting Action";
            actionPickerButton.TextColor = Color.White;
            actionPickerButton.FontSize = 18;
            actionPickerButton.TextOrientation = TextOrientation.Middle;
            actionPickerButton.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            // actionPickerButton.TextOrientation = interfaces.TextOrientation.Left;
            actionPickerButton.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            actionPickerButton.HeightRequest = deviceSpec.ScreenHeight * 8 / 100;
            actionPickerButton.Clicked += OnActionPickerButtonClicked;


            CustomSlider slider = new CustomSlider
            {
                Minimum = -2,
                Maximum = 2,
                WidthRequest = deviceSpec.ScreenWidth * 90 / 100
            };
            slider.StopGesture = GetstopGetsture;


            Image sliderDivider1 = new Image();
            sliderDivider1.Source = "drag_sepeate.png";


            Image sliderDivider2 = new Image();
            sliderDivider2.Source = "drag_sepeate.png";


            Image sliderDivider3 = new Image();
            sliderDivider3.Source = "drag_sepeate.png";

            Image sliderBG = new Image();
            sliderBG.Source = "drag_bg.png";

            this.Appearing += FeelingNowPage_Appearing;

            masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
            masterLayout.AddChildToLayout(subTitleBar, 0, Device.OnPlatform(9, 10, 9));
            masterLayout.AddChildToLayout(firstLine, 0, 20);
            masterLayout.AddChildToLayout(secondLine, 0, 25);
            masterLayout.AddChildToLayout(thirdLine, 0, 30);
            //masterLayout.AddChildToLayout(sliderBG, 7, 45);
            masterLayout.AddChildToLayout(slider, 5, 34);
            /*  masterLayout.AddChildToLayout(sliderDivider1, 30, 45.5f);
              masterLayout.AddChildToLayout(sliderDivider2, 50, 45.5f);
              masterLayout.AddChildToLayout(sliderDivider3, 70, 45.5f);*/
            masterLayout.AddChildToLayout(goalsAndDreamsPickerButton, 5, 50);
            masterLayout.AddChildToLayout(actionPickerButton, 5, 65);


            StackLayout listContainer = new StackLayout();
            listContainer.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            listContainer.WidthRequest = deviceSpec.ScreenWidth * 90 / 100;
            listContainer.HeightRequest = deviceSpec.ScreenHeight * 20 / 100;
            listContainer.ClassId = "preview";

            ListView actionPreviewListView = new ListView();
            actionPreviewListView.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
            actionPreviewListView.ItemTemplate = new DataTemplate(typeof(ActionPreviewCellItem));
            actionPreviewListView.SeparatorVisibility = SeparatorVisibility.None;
            actionPreviewListView.Opacity = 1;
            actionPreviewListView.ItemsSource = actionPreviewListSource;
            listContainer.Children.Add(actionPreviewListView);
            masterLayout.AddChildToLayout(listContainer, 5, 73);

            Content = masterLayout;

        }

        void NextButtonTapRecognizer_Tapped(object sender, System.EventArgs e)
        {
            ILocalNotification notfiy = DependencyService.Get<ILocalNotification>();
            notfiy.ShowNotification("Purpose Color", "Emotional awareness created");
        }


        
        public async void GetstopGetsture(bool pressed)
        {
            var goalsList = await ServiceHelper.GetAllGoals(2); // user id 2 for testing only // test

            if( goalsList != null )
            {
                App.goalsListSource = null;
                App.goalsListSource = goalsList;
            }

            OnGoalsPickerButtonClicked(goalsAndDreamsPickerButton, EventArgs.Empty);
        }

        void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        void FeelingsSecondPage_Appearing(object sender, System.EventArgs e)
        {
            base.OnAppearing();
           // this.Animate("", (s) => Layout(new Rectangle(((1 - s) * Width), Y, Width, Height)), 0, 600, Easing.SpringIn, null, null);
          //  this.Animate("", (s) => Layout(new Rectangle(X, (s - 1) * Height, Width, Height)), 0, 600, Easing.SpringIn, null, null); //slide down

           // this.Animate("", (s) => Layout(new Rectangle(X, (1 - s) * Height, Width, Height)), 0, 600, Easing.SpringIn, null, null); // slide up
        }

        void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            App.masterPage.IsPresented = !App.masterPage.IsPresented;
        }

        void backButton_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new GraphPage());
        }


        void OnActionPickerButtonClicked(object sender, System.EventArgs e)
        {

            App.actionsListSource = new List<CustomListViewItem>();
            App.actionsListSource.Add(new CustomListViewItem { EmotionID = "22", EventID = "12", Name = "Go to gym", SliderValue = 2 });
            App.actionsListSource.Add(new CustomListViewItem { EmotionID = "22", EventID = "12", Name = "Make reservations", SliderValue = 2 });
            App.actionsListSource.Add(new CustomListViewItem { EmotionID = "22", EventID = "12", Name = "book flight", SliderValue = 2 });
            App.actionsListSource.Add(new CustomListViewItem { EmotionID = "22", EventID = "12", Name = "Acquire Money", SliderValue = 2 });
           

            CustomPicker ePicker = new CustomPicker(masterLayout, App.actionsListSource, 35, Constants.ADD_ACTIONS, true, true);
            ePicker.WidthRequest = deviceSpec.ScreenWidth;
            ePicker.HeightRequest = deviceSpec.ScreenHeight;
            ePicker.ClassId = "ePicker";
            ePicker.listView.ItemSelected += OnActionPickerItemSelected;
            masterLayout.AddChildToLayout(ePicker, 0, 0);

            //double yPos = 60 * deviceSpec.ScreenHeight / 100;
            // ePicker.TranslateTo(0, -yPos, 250, Easing.BounceIn);
            // ePicker.FadeTo(1, 750, Easing.Linear); 
        }

        void OnGoalsPickerButtonClicked(object sender, System.EventArgs e)
        {
            //if (App.goalsListSource == null || App.goalsListSource.Count <= 0)
            //{
            //    IProgressBar progress = DependencyService.Get<IProgressBar>();
            //    progress.ShowToast("Goals empty");
            //    return;
            //}

            App.goalsListSource = new List<CustomListViewItem>();
            App.goalsListSource.Add(new CustomListViewItem { EmotionID = "22", EventID = "12", Name = "Loose weight", SliderValue = 2 });
            App.goalsListSource.Add(new CustomListViewItem { EmotionID = "22", EventID = "12", Name = "Make a trip", SliderValue = 2 });
            App.goalsListSource.Add(new CustomListViewItem { EmotionID = "22", EventID = "12", Name = "Learn Yoga", SliderValue = 2 });
            App.goalsListSource.Add(new CustomListViewItem { EmotionID = "22", EventID = "12", Name = "Make certification", SliderValue = 2 });

            CustomPicker ePicker = new CustomPicker(masterLayout, App.GetGoalsList(), 35, Constants.ADD_GOALS, true, true);
            ePicker.WidthRequest = deviceSpec.ScreenWidth;
            ePicker.HeightRequest = deviceSpec.ScreenHeight;
            ePicker.ClassId = "ePicker";
            ePicker.listView.ItemSelected += OnGoalsPickerItemSelected;
            masterLayout.AddChildToLayout(ePicker, 0, 0);
            //double yPos = 60 * deviceSpec.ScreenHeight / 100;
            //ePicker.TranslateTo(0, yPos, 250, Easing.BounceIn);
            // ePicker.FadeTo(1, 750, Easing.Linear); 
        }



        protected override bool OnBackButtonPressed()
        {
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            if (pickView != null)
            {
                masterLayout.Children.Remove(pickView);
                pickView = null;
                return true;
            }

            return base.OnBackButtonPressed();
        }

        void OnActionPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CustomListViewItem item = e.SelectedItem as CustomListViewItem;
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            masterLayout.Children.Remove(pickView);
            pickView = null;
            actionPreviewListSource.Add(new PreviewItem { Name = item.Name, Image = null });
        }

        void OnGoalsPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CustomListViewItem item = e.SelectedItem as CustomListViewItem;
            goalsAndDreamsPickerButton.Text = item.Name;
            goalsAndDreamsPickerButton.TextColor = Color.Black;
            View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
            masterLayout.Children.Remove(pickView);
            pickView = null;
            actionPickerButton.IsVisible = true;

            //OnActionPickerButtonClicked( actionPickerButton, EventArgs.Empty );
        }

        void emotionPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        async void FeelingNowPage_Appearing(object sender, System.EventArgs e)
        {
            /* int val = 2;
             for( int index = 0; index < 200; index++ )

             {
                 await Task.Delay(2);

                 if( slider.Value > 90 )
                 {
                     val = -2;
                 }
                 slider.Value += val;
             }*/
        }

        public void Dispose()
        {
            ePicker = null;
            masterLayout = null;
            deviceSpec = null;
            actionPickerButton = null;
            goalsAndDreamsPickerButton = null;
        }
    }




    public class ActionPreviewCellItem : ViewCell
    {
        public ActionPreviewCellItem()
        {
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.MENU_BG_COLOR;
            IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            Label name = new Label();
            name.SetBinding(Label.TextProperty, "Name");
            name.TextColor = Color.Gray;
            name.FontSize = Device.OnPlatform(12, 15, 18);
            name.WidthRequest = deviceSpec.ScreenWidth * 50 / 100;

            StackLayout divider = new StackLayout();
            divider.WidthRequest = deviceSpec.ScreenWidth;
            divider.HeightRequest = .75;
            divider.BackgroundColor = Color.FromRgb(255, 255, 255);


            CustomImageButton deleteButton = new CustomImageButton();
            deleteButton.ImageName = "delete_button.png";
            deleteButton.WidthRequest = 20;
            deleteButton.HeightRequest = 20;
            deleteButton.SetBinding(CustomImageButton.ClassIdProperty, "Name");

            deleteButton.Clicked += (sender, e) =>
            {
                CustomImageButton button = sender as CustomImageButton;
                PreviewItem itemToDel = FeelingsSecondPage.actionPreviewListSource.FirstOrDefault(item => item.Name == button.ClassId);
                if (itemToDel != null)
                {
                    FeelingsSecondPage.actionPreviewListSource.Remove(itemToDel);
                }

            };

            masterLayout.WidthRequest = deviceSpec.ScreenWidth;
            masterLayout.HeightRequest = deviceSpec.ScreenHeight * Device.OnPlatform(30, 50, 10) / 100;


            masterLayout.AddChildToLayout(name, (float)Device.OnPlatform(5, 5, 5), (float)Device.OnPlatform(5, 5, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            masterLayout.AddChildToLayout(deleteButton, (float)80, (float)Device.OnPlatform(5, 3.5, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            // masterLayout.AddChildToLayout(divider, (float)1, (float)20, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            this.View = masterLayout;

        }



    }
}
