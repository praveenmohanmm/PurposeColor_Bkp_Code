
using Cross;
using CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PurposeColor;
using PurposeColor.CustomControls;
using PurposeColor.Model;
using PurposeColor.Service;
using PurposeColor.interfaces;

namespace PurposeColor.screens
{

    public class MenuItems
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public MenuItems()
        {
        }
    }


    public class CustomMenuItemCell : ViewCell
    {
        public CustomMenuItemCell()
        {

            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.MENU_BG_COLOR;
            //IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            double screenWidth = App.screenWidth;
            double screenHeight = App.screenHeight;
            Label name = new Label();
            name.SetBinding(Label.TextProperty, "Name");
            name.TextColor = Constants.MAIN_MENU_TEXT_COLOR;//Color.Black;
            name.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
            name.FontSize = Device.OnPlatform(12, 17, 18);

            StackLayout divider = new StackLayout();
            divider.WidthRequest = screenWidth;
            divider.HeightRequest = .75;
            divider.BackgroundColor = Color.FromRgb(255, 255, 255);

            Image sideImage = new Image();
            sideImage.WidthRequest = 25;
            sideImage.HeightRequest = 25;
            sideImage.SetBinding(Image.SourceProperty, "ImageName");
            sideImage.Aspect = Aspect.Fill;

            masterLayout.WidthRequest = screenWidth;
            masterLayout.HeightRequest = screenHeight * Device.OnPlatform(30, 50, 10) / 100;

            masterLayout.AddChildToLayout(sideImage, (float)5, (float)Device.OnPlatform(5, 0, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
			masterLayout.AddChildToLayout(name, (float)Device.OnPlatform( 15, 15 , 15 ), (float)Device.OnPlatform(5, 0, 50), (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
           // masterLayout.AddChildToLayout(divider, (float)1, (float)20, (int)masterLayout.WidthRequest, (int)masterLayout.HeightRequest);
            this.View = masterLayout;
        }

    }

    public class MenuPage : ContentPage
    {
        ListView listView;
		PurposeColorTitleBar mainTitleBar = null;
        public MenuPage()
        {
            this.BackgroundColor = Color.White;
            NavigationPage.SetHasNavigationBar(this, false);
            CustomLayout masterLayout = new CustomLayout();
            masterLayout.BackgroundColor = Constants.MENU_BG_COLOR;
            //IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
            double screenWidth = App.screenWidth;
            double screenHeight = App.screenHeight;
            //PurposeColorTitleBar titleBar = new PurposeColorTitleBar(Color.FromRgb(8, 137, 216), "Purpose Color", Color.Black, "back");

			mainTitleBar = new PurposeColorTitleBar(Color.FromRgb(8, 135, 224), "Purpose Color", Color.Black, "back", true);
            List<MenuItems> menuItems = new List<MenuItems>();
			User user = null;
            try
            {
                PurposeColor.Database.ApplicationSettings AppSettings = App.Settings;
                PurposeColor.Model.GlobalSettings globalSettings = AppSettings.GetAppGlobalSettings();
				user = App.Settings.GetUser();
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }

            // add these items only if globalSettings.IsLoggedIn //

			if (user != null ) {

				if (App.burgerMenuItems == null) {
					App.burgerMenuItems = new System.Collections.ObjectModel.ObservableCollection<MenuItems> ();
				}

				App.burgerMenuItems.Clear ();

				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.EMOTIONAL_AWARENESS,
					ImageName = Device.OnPlatform ("emotional_awrness_menu_icon.png", "emotional_awrness_menu_icon.png", "//Assets//emotional_awrness_menu_icon.png")
				});
				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.GEM,
					ImageName = Device.OnPlatform ("gem_menu_icon.png", "gem_menu_icon.png", "//Assets//gem_menu_icon.png")
				});
				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.GOALS_AND_DREAMS,
					ImageName = Device.OnPlatform ("goals_drms_menu_icon.png", "goals_drms_menu_icon.png", "//Assets//goals_drms_menu_icon.png")
				});
				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.EMOTIONAL_INTELLIGENCE,
					ImageName = Device.OnPlatform ("emotion_intellegene_menu_icon.png", "emotion_intellegene_menu_icon.png", "//Assets//emotion_intellegene_menu_icon.png")
				});
				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.COMMUNITY_GEMS,
					ImageName = Device.OnPlatform ("comunity_menu_icon.png", "comunity_menu_icon.png", "//Assets//comunity_menu_icon.png")
				});
				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.APPLICATION_SETTTINGS,
					ImageName = Device.OnPlatform ("setings_menu_icon.png", "setings_menu_icon.png", "//Assets//setings_menu_icon.png")
				});
				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.SIGN_OUT_TEXT,
					ImageName = Device.OnPlatform ("logout_icon.png", "logout_icon.png", "//Assets//logout_icon.png")
				});

				// add these items only if globalSettings.IsLoggedIn //
			} 
			else 
			{
				if (App.burgerMenuItems == null) {
					App.burgerMenuItems = new System.Collections.ObjectModel.ObservableCollection<MenuItems> ();
				}

				App.burgerMenuItems.Add (new MenuItems {
					Name = Constants.SIGN_OUT_IN,
					ImageName = Device.OnPlatform ("logout_icon.png", "logout_icon.png", "//Assets//logout_icon.png")
				});
			}
			
            listView = new ListView();
			listView.ItemsSource = App.burgerMenuItems;
            listView.ItemTemplate = new DataTemplate(typeof(CustomMenuItemCell));
            listView.SeparatorVisibility = SeparatorVisibility.None;
            listView.ItemSelected += OnListViewItemSelected;
            listView.BackgroundColor = Constants.MENU_BG_COLOR;
            listView.RowHeight =(int) screenHeight * 10 / 100;
			listView.HeightRequest = App.screenHeight * .8;

            Icon = Device.OnPlatform("bottom_menu_icon.png", "bottom_menu_icon.png", "//Assets//bottom_menu_icon.png");
            Title = "Menu";

            masterLayout.BackgroundColor = Constants.MENU_BG_COLOR;


			masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
           // masterLayout.AddChildToLayout(titleBar, 0, 0);
			Button btn  = new Button{Text = "Close", TextColor = Color.Red, BackgroundColor = Color.Yellow};
			//masterLayout.AddChildToLayout(btn, 0, 1);//(float)(App.screenWidth * .30), (float)(App.screenHeight * .5));
			masterLayout.AddChildToLayout(listView, 0, 15);
			mainTitleBar.imageAreaTapGestureRecognizer.Tapped += imageAreaTapGestureRecognizer_Tapped;

           // this.TranslationY = screenHeight * 10 / 100;
            Content = masterLayout;
        }

		void imageAreaTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
		{
			App.masterPage.IsPresented = !App.masterPage.IsPresented;
		}

        public void Dispose()
        {
            listView.ItemSelected -= OnListViewItemSelected;
            listView = null;
            GC.Collect();
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {

                if (listView.SelectedItem == null)
                    return;

                MenuItems selItem = e.SelectedItem as MenuItems;

                if ("Emotional Awareness" == selItem.Name)
                {
                    App.masterPage.IsPresented = false;
                    App.masterPage.Detail = new NavigationPage(new FeelingNowPage());
                }
                else if ("Goal Enabling Materials" == selItem.Name)
                {
                    App.masterPage.IsPresented = false;
                    App.masterPage.Detail = new NavigationPage(new GemsMainPage());
                }
                else if ("Goals & Dreams" == selItem.Name)
                {
                    App.masterPage.IsPresented = false;
					App.masterPage.Detail = new NavigationPage(new GoalsPage());
                }
				else if (Constants.EMOTIONAL_INTELLIGENCE == selItem.Name)
                {
                    App.masterPage.IsPresented = false;
					App.masterPage.Detail = new NavigationPage(new PieGraphPage());
                }
                else if ("Community GEMs" == selItem.Name)
                {
					App.masterPage.IsPresented = false;
					DetailsPageModel model = new DetailsPageModel();
					App.masterPage.Detail = new NavigationPage(new CommunityGems( model ));
                }
                else if (Constants.APPLICATION_SETTTINGS == selItem.Name)
                {
                    App.masterPage.IsPresented = false;
                    App.masterPage.Detail = new NavigationPage(new ApplicationSettingsPage());
				}
				else if(Constants.SIGN_OUT_TEXT == selItem.Name)
				{

					try
					{
						

						#region SAVING SIGN OUT SETTINGS

						PurposeColor.Model.User user = null;
						user = App.Settings.GetUser();
						App.Settings.DeleteAllUsers(); // the same user may log in again.
						App.Settings.DeleteAllCompletedGoals();
						App.Settings.DeleteAllEmotions();
						App.Settings.DeleteAllEvents();
						App.Settings.DeleteAllGemsActions();
						App.Settings.DeleteAllGemsEvents();
						App.Settings.DeleteAllPendingGoals();
						App.Settings.DeleteAllActionWithImage();
						App.Settings.DeleteAllEventWithImage();
						//App.Settings.DeleteCommunityGems(); // as its common for all users.

						PurposeColor.Model.GlobalSettings globalSettings = App.Settings.GetAppGlobalSettings();
						if(globalSettings != null)
						{
							globalSettings.ShowRegistrationScreen = false;
							globalSettings.IsLoggedIn = false;
							globalSettings.IsFirstLogin = false;
							await App.Settings.SaveAppGlobalSettings(globalSettings);
						}

						if (user != null)
						{
							string statusCode = await PurposeColor.Service.ServiceHelper.LogOut(user.UserId.ToString());
							if (statusCode != "200")
							{
								await DisplayAlert(Constants.ALERT_TITLE, "Network error, please try again later.", Constants.ALERT_OK);
							}
						}

						App.Current.Properties["IsLoggedIn"] = false;
						#endregion
					}
					catch (Exception)
					{
						DisplayAlert(Constants.ALERT_TITLE, "Network error, Could not process the request.", Constants.ALERT_OK);
					}
					App.IsLoggedIn = false;
					App.masterPage.IsPresented = false;
					App.masterPage.Detail = new NavigationPage(new LogInPage());

					App.burgerMenuItems.Clear();
					App.burgerMenuItems.Add (new MenuItems {
						Name = Constants.SIGN_OUT_IN,
						ImageName = Device.OnPlatform ("logout_icon.png", "logout_icon.png", "//Assets//logout_icon.png")
					});

				} //SIGN_OUT
				else if(Constants.SIGN_OUT_IN == selItem.Name)
				{
					App.masterPage.IsPresented = false;
					App.masterPage.Detail = new NavigationPage(new LogInPage());
				}

                listView.SelectedItem = null; // reset the list selection, other wise the same menu cannot be selected again consecutively.

            }
            catch (Exception)
            {
            }
        }
    }
}

