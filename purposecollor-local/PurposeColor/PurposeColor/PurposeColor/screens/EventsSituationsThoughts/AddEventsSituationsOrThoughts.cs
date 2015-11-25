﻿using Contacts.Plugin;
using Contacts.Plugin.Abstractions;
using Cross;
using CustomControls;
using Media.Plugin;
using PurposeColor.CustomControls;
using PurposeColor.Model;
using PurposeColor.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using System.Linq;
using Geolocator.Plugin;
using PurposeColor.interfaces;

namespace PurposeColor.screens
{
	public class AddEventsSituationsOrThoughts : ContentPage, System.IDisposable
	{
		#region MEMBERS

		CustomLayout masterLayout;
		StackLayout TopTitleBar;
		PurposeColorBlueSubTitleBar subTitleBar;
		CustomEditor textInput;
		Entry titleText;
		StackLayout textInputContainer;
		StackLayout audioInputStack;
		Image cameraInput;
		Image audioInput;
		StackLayout cameraInputStack;
		Image galleryInput;
		StackLayout galleryInputStack;
		Image locationInput;
		StackLayout locationInputStack;
		Image contactInput;
		StackLayout contactInputStack;
		StackLayout iconContainer;
		StackLayout textinputAndIconsHolder;
		TapGestureRecognizer CameraTapRecognizer;
		string pageTitle;
		bool isAudioRecording = false;
		PurposeColor.interfaces.IAudioRecorder audioRecorder;
		string folder;
		string path;
		List<CustomListViewItem> contacts;

		#endregion

		public AddEventsSituationsOrThoughts(string title)
		{
			NavigationPage.SetHasNavigationBar(this, false);
			masterLayout = new CustomLayout();
			audioRecorder = DependencyService.Get<PurposeColor.interfaces.IAudioRecorder>();
			IDeviceSpec deviceSpec = DependencyService.Get<IDeviceSpec>();
			masterLayout.BackgroundColor = Constants.PAGE_BG_COLOR_LIGHT_GRAY;
			pageTitle = title;
			int devWidth = (int)deviceSpec.ScreenWidth;

			#region TITLE BARS
			TopTitleBar = new StackLayout
			{
				BackgroundColor = Constants.BLUE_BG_COLOR,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.StartAndExpand,
				Padding = 0,
				Spacing = 0,
				Children = { new BoxView { WidthRequest = deviceSpec.ScreenWidth } }
			};
			masterLayout.AddChildToLayout(TopTitleBar, 0, 0);

			string trimmedPageTitle = string.Empty;
			if (title.Length > 20)
			{
				trimmedPageTitle = title.Substring(0, 20);
				trimmedPageTitle += "...";
			}
			else
			{
				trimmedPageTitle = pageTitle;
			}

			subTitleBar = new PurposeColorBlueSubTitleBar(Constants.SUB_TITLE_BG_COLOR, trimmedPageTitle, true, true);
			masterLayout.AddChildToLayout(subTitleBar, 0, 1);
			subTitleBar.BackButtonTapRecognizer.Tapped += OnBackButtonTapRecognizerTapped;
			subTitleBar.NextButtonTapRecognizer.Tapped += NextButtonTapRecognizer_Tapped;
			#endregion

			titleText = new Entry
			{
				VerticalOptions = LayoutOptions.StartAndExpand,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				BackgroundColor = Color.White,
				Placeholder = "Title",
				TextColor = Color.FromHex("#424646"),
				WidthRequest = (int)(devWidth * .92) // 92% of screen
					//  FontSize = Device.OnPlatform( 20, 22, 30 ),
					//  FontFamily = Constants.HELVERTICA_NEUE_LT_STD
			};

			masterLayout.AddChildToLayout(titleText, 3, 11);

			#region TEXT INPUT CONTROL

			textInput = new CustomEditor
			{
				VerticalOptions = LayoutOptions.StartAndExpand,
				HorizontalOptions = LayoutOptions.StartAndExpand,
				HeightRequest = 150,
				Placeholder = pageTitle
			};

			//string input = pageTitle;
			//if (input == Constants.ADD_ACTIONS)
			//{
			//    textInput.Text = "Add supporting actions";
			//}
			//else if (input == Constants.ADD_EVENTS)
			//{
			//    textInput.Text = "Add Events";
			//}
			//else if (input == Constants.ADD_GOALS)
			//{
			//    textInput.Text = "Add Goals";
			//}

			textInputContainer = new StackLayout
			{
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				Padding = 1,
				Spacing = 0,
				Children = { textInput }
			};

			//int devWidth = (int)deviceSpec.ScreenWidth;
			int textInputWidth = (int)(devWidth * .92); // 92% of screen
			textInput.WidthRequest = textInputWidth;

			#endregion

			#region ICONS

			audioInput = new Image()
			{
				Source = Device.OnPlatform("ic_music.png", "ic_music.png", "//Assets//ic_music.png"),
				Aspect = Aspect.AspectFit
			};
			audioInputStack = new StackLayout
			{
				Padding = 10,
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				HorizontalOptions = LayoutOptions.Center,
				Spacing = 0,
				Children = { audioInput, new Label { Text = "Audio", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
			};

			cameraInput = new Image()
			{
				Source = Device.OnPlatform("icn_camera.png", "icn_camera.png", "//Assets//icn_camera.png"),
				Aspect = Aspect.AspectFit
			};
			cameraInputStack = new StackLayout
			{
				Padding = 10,
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				HorizontalOptions = LayoutOptions.Center,
				Spacing = 0,
				Children = { cameraInput, new Label { Text = "Camera", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
			};

			#region CAMERA TAP RECOGNIZER
			CameraTapRecognizer = new TapGestureRecognizer();
			cameraInputStack.GestureRecognizers.Add(CameraTapRecognizer);
			CameraTapRecognizer.Tapped += async (s, e) =>
			{
				try
				{
					if (Media.Plugin.CrossMedia.Current.IsCameraAvailable)
					{

						string fileName = string.Format("Image{0}.png", System.DateTime.Now.ToString("yyyyMMddHHmmss"));

						var file = await Media.Plugin.CrossMedia.Current.TakePhotoAsync(new Media.Plugin.Abstractions.StoreCameraMediaOptions
							{

								Directory = "Purposecolor",
								Name = fileName
							});

						//if (file == null)
						//{
						//    DisplayAlert("Alert", "Image could not be saved, please try again later", "ok");
						//}
					}
				}
				catch (System.Exception ex)
				{
					DisplayAlert("Camera", ex.Message + " Please try again later", "ok");
				}
			};

			#endregion

			#region AUDIO TAP RECOGNIZER

			TapGestureRecognizer audioTapGestureRecognizer = new TapGestureRecognizer();

			audioTapGestureRecognizer.Tapped += (s, e) =>
			{
				try
				{
					if (!isAudioRecording)
					{
						isAudioRecording = true;
						if (!audioRecorder.RecordAudio())
						{
							DisplayAlert("Audio recording", "Audio cannot be recorded, please try again later.", "ok");
						}
						else
						{
							DisplayAlert("Audio recording", "Audio recording started, Tap the audio icon again to end.", "ok");
						}
					}
					else
					{
						isAudioRecording = false;
						audioRecorder.StopRecording();
						DisplayAlert("Audio recording", "Audio saved to gallery.", "ok");
					}
				}
				catch (System.Exception)
				{
					DisplayAlert("Audio recording", "Audio cannot be recorded, please try again later.", "ok");
				}
			};
			audioInputStack.GestureRecognizers.Add(audioTapGestureRecognizer);

			#endregion

			galleryInput = new Image()
			{
				Source = Device.OnPlatform("icn_gallery.png", "icn_gallery.png", "//Assets//icn_gallery.png"),
				Aspect = Aspect.AspectFit
			};
			galleryInputStack = new StackLayout
			{
				Padding = 10,
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				HorizontalOptions = LayoutOptions.Center,
				Spacing = 0,
				Children = { galleryInput, new Label { Text = "Gallery", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
			};

			#region GALLERY  TAP RECOGNIZER

			TapGestureRecognizer galleryInputStackTapRecognizer = new TapGestureRecognizer();
			galleryInputStack.GestureRecognizers.Add(galleryInputStackTapRecognizer);
			galleryInputStackTapRecognizer.Tapped += async (s, e) =>
			{
				if (!CrossMedia.Current.IsPickPhotoSupported)
				{
					DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
					return;
				}

				var file = await CrossMedia.Current.PickPhotoAsync();

				MemoryStream ms = new MemoryStream();
				file.GetStream().CopyTo(ms);
				string convertedSrting = Convert.ToBase64String(ms.ToArray());



				MediaPost mediaWeb = new MediaPost();
				mediaWeb.event_details = textInput.Text;
				mediaWeb.event_title = titleText.Text;
				mediaWeb.user_id = 2;
				mediaWeb.event_image = convertedSrting;


				var test = await ServiceHelper.PostMedia(mediaWeb);

			};

			#endregion

			locationInput = new Image()
			{
				Source = Device.OnPlatform("icn_location.png", "icn_location.png", "//Assets//icn_location.png"),
				Aspect = Aspect.AspectFit
			};
			locationInputStack = new StackLayout
			{
				Padding = new Thickness(Device.OnPlatform(10,10,14),10,Device.OnPlatform(10,10,14),10),
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				HorizontalOptions = LayoutOptions.Center,
				Spacing = 0,
				Children = { locationInput, new Label { Text = "Location", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
			};

			#region LOCATION TAP RECOGNIZER

			TapGestureRecognizer locationInputTapRecognizer = new TapGestureRecognizer();
			locationInputStack.GestureRecognizers.Add(locationInputTapRecognizer);
			locationInputTapRecognizer.Tapped += LocationInputTapRecognizer_Tapped;

			#endregion

			contactInput = new Image()
			{
				Source = Device.OnPlatform("icn_contact.png", "icn_contact.png", "//Assets//icn_contact.png"),
				Aspect = Aspect.AspectFit
			};

			contactInputStack = new StackLayout
			{
				Padding = 10,
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				Spacing = 0,
				HorizontalOptions = LayoutOptions.Center,
				Children = { contactInput, new Label { Text = "Contact", TextColor = Constants.TEXT_COLOR_GRAY, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) } }
			};

			#region CONTACTS TAP RECOGNIZER

			TapGestureRecognizer contactsInputTapRecognizer = new TapGestureRecognizer();
			contactInputStack.GestureRecognizers.Add(contactsInputTapRecognizer);
			contactsInputTapRecognizer.Tapped += async (s, e) =>
			{
				try
				{
					if (await CrossContacts.Current.RequestPermission())
					{
						try 
						{	        
							CrossContacts.Current.PreferContactAggregation = false;

							if (CrossContacts.Current.Contacts == null)
							{
								return;
							}

							List<Contact> contactSource = new List<Contact>();

							contactSource = CrossContacts.Current.Contacts.Where( name => name.DisplayName != null ).ToList();
							contacts = new List<CustomListViewItem>();
							foreach (var item in contactSource)
							{
								
								try {
									if( item != null && item.DisplayName != null)
										contacts.Add(new CustomListViewItem { Name = item.DisplayName});
								} catch (Exception ex) {
									
								}
								
							}

							contacts = contacts.OrderBy(c => c.Name).ToList();

							System.Collections.Generic.List<CustomListViewItem> pickerSource = contacts;
							CustomPicker ePicker = new CustomPicker(masterLayout, pickerSource, 65, "Select Contact", true, false);
							ePicker.WidthRequest = deviceSpec.ScreenWidth;
							ePicker.HeightRequest = deviceSpec.ScreenHeight;
							ePicker.ClassId = "ePicker";
							ePicker.listView.ItemSelected += OnContactsPickerItemSelected;
							masterLayout.AddChildToLayout(ePicker, 0, 0);
						}
						catch (Exception ex)
						{
							DisplayAlert( "",ex.Message, "ok" );
						}
					}
					else
					{
						DisplayAlert("contacts access permission ", "Please add permission to access contacts", "ok");
					}
				}
				catch (Exception ex)
				{
					DisplayAlert("contactsInputTapRecognizer: ", ex.Message,"ok");
				}
			};

			#endregion

			#endregion

			#region CONTAINERS

			iconContainer = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Constants.STACK_BG_COLOR_GRAY,
				HorizontalOptions = LayoutOptions.Center,
				WidthRequest = (int)(devWidth * .92) + 2, /// 2 pxl padding added to text input.
				Spacing = Device.OnPlatform((deviceSpec.ScreenWidth * 4.5 / 100),(deviceSpec.ScreenWidth * 4.5 / 100),(deviceSpec.ScreenWidth * 3.8 / 100)),
				Padding = 0,
				Children = { galleryInputStack, cameraInputStack, audioInputStack, locationInputStack, contactInputStack }
			};

			textinputAndIconsHolder = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Center,
				Spacing = 0,
				Padding = 0,
				Children = { textInputContainer, iconContainer }
			};

			int iconY = (int)textInput.Y + (int)textInput.Height + 5;
			masterLayout.AddChildToLayout(textinputAndIconsHolder, 3, 21);

			#endregion

			Content = masterLayout;
		}

		async void LocationInputTapRecognizer_Tapped (object sender, EventArgs e)
		{
			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;
			IProgressBar progress = DependencyService.Get<IProgressBar> ();

			if (locator.IsGeolocationEnabled) 
			{
				DisplayAlert ("Purpose Color", "Please turn ON location services", "Ok");
				return;
			}

		
			progress.ShowProgressbar ( "Getting Location.." );


			var position = await locator.GetPositionAsync (timeoutMilliseconds: 10000);
			App.Lattitude = position.Latitude;
			App.Longitude = position.Longitude;

			ILocation loc = DependencyService.Get<ILocation> ();
			var address = await loc.GetLocation ( position.Latitude, position.Longitude );
			textInput.Text = "@ " + address;
			progress.HideProgressbar ();
		}

		private void OnContactsPickerItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var obj = e.SelectedItem as CustomListViewItem;
			string name = (e.SelectedItem as CustomListViewItem).Name;
			if (!string.IsNullOrEmpty(name))
			{
				int nIndex = 0;
				string preText = " with ";
				if (textInput.Text != null)
				{
					nIndex = textInput.Text.IndexOf(name);
					preText = textInput.Text.IndexOf("with") <= 0 ? " with " : ", ";
				}

				if (nIndex <= 0)
				{
					textInput.Text = textInput.Text + preText + name;
				}

			}

			View pickView = masterLayout.Children.FirstOrDefault(pick => pick.ClassId == "ePicker");
			masterLayout.Children.Remove(pickView);
			pickView = null;
		}

		void NextButtonTapRecognizer_Tapped(object sender, System.EventArgs e)
		{

			if (string.IsNullOrWhiteSpace(textInput.Text)|| string.IsNullOrWhiteSpace(titleText.Text))
			{
				DisplayAlert(pageTitle, "value cannot be empty", "ok");
			}
			else
			{
				string input = pageTitle;
				CustomListViewItem item = new CustomListViewItem { Name = textInput.Text };
				if (input == Constants.ADD_ACTIONS)
				{
					App.actionsListSource.Add(item);
				}
				else if (input == Constants.ADD_EVENTS)
				{
					UserEvent newEvent = new UserEvent();
					newEvent.EventName = textInput.Text;

					// save event to local db and send to API.
					App.eventsListSource.Add(item);
				}
				else if (input == Constants.ADD_GOALS)
				{
					App.goalsListSource.Add(item);
				}

				Navigation.PopAsync();
			}
		}

		void OnBackButtonTapRecognizerTapped(object sender, System.EventArgs e)
		{
			Navigation.PopAsync();
		}

		public void Dispose()
		{
			subTitleBar.BackButtonTapRecognizer.Tapped -= OnBackButtonTapRecognizerTapped;
			subTitleBar.NextButtonTapRecognizer.Tapped -= NextButtonTapRecognizer_Tapped;
			this.masterLayout = null;
			this.TopTitleBar = null;
			this.subTitleBar = null;
			this.textInput = null;
			this.textInputContainer = null;
			this.audioInputStack = null;
			this.cameraInput = null;
			this.audioInput = null;
			this.cameraInputStack = null;
			this.galleryInput = null;
			this.galleryInputStack = null;
			this.locationInput = null;
			this.locationInputStack = null;
			this.contactInput = null;
			this.contactInputStack = null;
			this.iconContainer = null;
			this.textinputAndIconsHolder = null;
			this.audioRecorder = null;
			this.titleText = null;
			this.contacts = null;
		}
	}
}
