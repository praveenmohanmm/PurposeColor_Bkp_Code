﻿using Cross;
using CustomControls;
using PurposeColor.CustomControls;
using PurposeColor.interfaces;
using PurposeColor.Model;
using PurposeColor.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace PurposeColor.screens
{
	public class GemsMainPage : ContentPage, IDisposable
	{
		CustomLayout masterLayout;
		IProgressBar progressBar;
		int listViewVislbleIndex;
		GemsPageTitleBar mainTitleBar;
		ScrollView masterScroll;
		StackLayout masterStack;
		List<EventWithImage> eventsWithImage;
		List<ActionWithImage> actionsWithImage;

		StackLayout emotionsButtion = null;
		StackLayout goalsButton = null;
		Label goalsAndDreamsLabel = null;
		Label emotionLabel = null;

		TapGestureRecognizer emotionListingBtnTapgesture = null;
		TapGestureRecognizer goalsListingBtnTapgesture = null;

		bool isEmotionsListing = false;
		string localFilePath = string.Empty;
		StackLayout listViewContainer;
		bool isLoading = false;
		string previousTitle = string.Empty;

		bool reachedFront = true;
		bool displayedLastGem = false;
		int lastGemIndexOnDisplay = 0;
		int firstGemIndexOnDisplay = 0;

		public GemsMainPage()
		{

			NavigationPage.SetHasNavigationBar(this, false);
			masterLayout = new CustomLayout();
			masterLayout.BackgroundColor = Color.FromRgb(244, 244, 244);
			progressBar = DependencyService.Get<IProgressBar>();

			this.Appearing += OnAppearing;
			this.Disappearing += GemsMainPage_Disappearing;
			mainTitleBar = new GemsPageTitleBar(Color.FromRgb(8, 135, 224), "My Supporting Emotions", Color.White, "", false);

			masterScroll = new ScrollView();
			masterScroll.WidthRequest = App.screenWidth;
			masterScroll.HeightRequest = App.screenHeight * 85 / 100;
			masterScroll.BackgroundColor = Color.White;
			masterScroll.Scrolled += OnScroll;

			masterStack = new StackLayout();
			masterStack.Orientation = StackOrientation.Vertical;
			masterStack.BackgroundColor = Color.White; //Color.Transparent;

			emotionLabel = new Label {
				Text = "EMOTIONS  ", 
				FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
				FontSize = Device.OnPlatform (14, 18, 14),
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.Center,
				TextColor = Color.White,
				WidthRequest = App.screenWidth * .5,
				XAlign = TextAlignment.End
			};

			emotionsButtion = new StackLayout{
				Children = {
					emotionLabel
				},
				BackgroundColor = Color.FromRgb(8, 159, 245),//Constants.BLUE_BG_COLOR,
				Orientation = StackOrientation.Horizontal,
				WidthRequest = App.screenWidth * .5
			};

			emotionListingBtnTapgesture = new TapGestureRecognizer ();
			emotionListingBtnTapgesture.Tapped += ShowEmotionsTapGesture_Tapped;
			emotionsButtion.GestureRecognizers.Add (emotionListingBtnTapgesture);

			goalsAndDreamsLabel = new Label {
				Text = "  GOALS & DREAMS", 
				FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
				FontSize = Device.OnPlatform (14, 18, 14),
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center,
				TextColor = Color.Gray
			};

			goalsButton = new StackLayout{
				Children = {
					goalsAndDreamsLabel
				},
				BackgroundColor = Constants.INPUT_GRAY_LINE_COLOR, // Color.FromRgb(232, 234, 230),//Constants.LIST_BG_COLOR, //Constants.STACK_BG_COLOR_GRAY,
				Orientation = StackOrientation.Horizontal,
				WidthRequest = App.screenWidth * .5
			};
			goalsListingBtnTapgesture = new TapGestureRecognizer ();
			goalsListingBtnTapgesture.Tapped += GoalsListingBtnTapgesture_Tapped;
			goalsButton.GestureRecognizers.Add (goalsListingBtnTapgesture);

			masterLayout.AddChildToLayout(mainTitleBar, 0, 0);
			masterLayout.AddChildToLayout(new StackLayout{BackgroundColor = Color.Aqua, HeightRequest = App.screenHeight * .08, Orientation = StackOrientation.Horizontal,Spacing = 0, Children = {emotionsButtion, goalsButton}}, 0,10);
		}

		void GemsMainPage_Disappearing (object sender, EventArgs e)
		{
			Dispose ();
		}

		async void GoalsListingBtnTapgesture_Tapped (object sender, EventArgs e)
		{
			try {
				if (!isEmotionsListing) {
					return;
				}
				progressBar.ShowProgressbar ("Loading..");

				if (actionsWithImage == null) 
				{
					actionsWithImage = await ServiceHelper.GetAllActionsWithImage ();

					if (actionsWithImage == null) 
					{
						var success = await DisplayAlert (Constants.ALERT_TITLE, "Error in fetching GEMS", Constants.ALERT_OK, Constants.ALERT_RETRY);
						if (!success) {
							//OnAppearing (sender, EventArgs.Empty);
							GoalsListingBtnTapgesture_Tapped(goalsButton, null);
							return;
						}
						else 
						{
							if (Device.OS != TargetPlatform.WinPhone)
							{
								actionsWithImage = await App.Settings.GetAllActionWithImage();
							}
						}
					}
					else
					{
						if (Device.OS != TargetPlatform.WinPhone)
						{
							App.Settings.SaveActionsWithImage(actionsWithImage);
						}
					}
				}

				// do list the actions.....//
				bool isSuccess = await AddActionsToView(0, true);
				if (isSuccess) 
				{
					isEmotionsListing = false;
					masterScroll.ScrollToAsync(0,0, true);
					#region button color
					// hide emotions & show goals , change selection buttons color
					Color goalBtnClr = goalsButton.BackgroundColor;
					goalsButton.BackgroundColor = emotionsButtion.BackgroundColor;
					emotionsButtion.BackgroundColor = goalBtnClr;
					Color golsTxtClr = goalsAndDreamsLabel.TextColor;
					goalsAndDreamsLabel.TextColor = emotionLabel.TextColor;
					emotionLabel.TextColor = golsTxtClr;
					#endregion
				}

			} catch (Exception ex) {
				var test = ex.Message;
			}
			progressBar.HideProgressbar ();
		}

		async void ShowEmotionsTapGesture_Tapped (object sender, EventArgs e)
		{
			try {
				if (isEmotionsListing) {
					return;
				}
				// display the emotions list and change color of Goals selection uttion
				progressBar.ShowProgressbar ("loading..");

				bool isSuccess = await AddEventsToView (0);
				if (isSuccess) 
				{
					masterScroll.ScrollToAsync(0,0, true);
					isEmotionsListing = true;
					#region MyRegionButton color
					Color eBtnClr = emotionsButtion.BackgroundColor;
					emotionsButtion.BackgroundColor = goalsButton.BackgroundColor;
					goalsButton.BackgroundColor = eBtnClr;
					Color ETxtClr = emotionLabel.TextColor;
					emotionLabel.TextColor = goalsAndDreamsLabel.TextColor;
					goalsAndDreamsLabel.TextColor = ETxtClr;

					#endregion
				}
				progressBar.HideProgressbar();
			} catch (Exception ex) {

			}
		}

		void OnBackButtonTapRecognizerTapped(object sender, EventArgs e)
		{
			try {
				App.Navigator.PopAsync();
			} catch (Exception ex) {

			}
		}

		#region OnAppearing

		async void OnAppearing (object sender, EventArgs e)
		{
			IProgressBar progress = DependencyService.Get<IProgressBar> ();

			try {
				progress.ShowProgressbar ("Loading gems..");
				try {
					if (eventsWithImage == null) 
					{
						eventsWithImage = await ServiceHelper.GetAllEventsWithImage ();

						if (eventsWithImage != null) {
							App.Settings.SaveEventsWithImage(eventsWithImage);
						}
						else
						{
							var success = await DisplayAlert (Constants.ALERT_TITLE, "Error in fetching gems", Constants.ALERT_OK, Constants.ALERT_RETRY);
							if (!success) {
								OnAppearing (sender, EventArgs.Empty);
								return;
							}
							else 
							{
								eventsWithImage = await App.Settings.GetAllEventWithImage(); // get from local db.
								//progress.HideProgressbar ();
							}
						}
					}
				}
				catch (Exception ex) 
				{
					var test = ex.Message;
				}

				listViewContainer = new StackLayout
				{
					Orientation = StackOrientation.Vertical,
					Padding = new Thickness(0, 0, 0, 5),
					//BackgroundColor = Color.White, //
					WidthRequest = App.screenWidth,
					Spacing = 0 // App.screenHeight * .02
				};
				masterStack.Children.Add (listViewContainer);

				StackLayout empty = new StackLayout ();
				empty.HeightRequest = Device.OnPlatform (30, 30, 50);
				empty.WidthRequest = App.screenWidth;
				empty.BackgroundColor = Color.Transparent;
				masterStack.Children.Add (empty);

				//masterLayout.AddChildToLayout(masterStack,0, 18);

				masterScroll.Content = masterStack;

				masterLayout.AddChildToLayout(masterScroll,0, 18);

				Content = masterLayout;

				// call - ShowEmotionsTapGesture_Tapped //
				ShowEmotionsTapGesture_Tapped(emotionsButtion, null);

				progress.HideProgressbar ();

			} catch (Exception ex) {
				var test = ex.Message;
			}
			progress.HideProgressbar ();

		}

		#endregion

		async Task<bool> AddEventsToView(int index, bool showNextGems = true)
		{
			try {
				isLoading = true;
				int listCapacity = 10;
				int max;

				if (showNextGems)
				{
					max = (index + listCapacity) <= eventsWithImage.Count ? (index + listCapacity) : eventsWithImage.Count;
					if (index >= max || eventsWithImage == null || eventsWithImage.Count == 0)
					{
						displayedLastGem = true;
						isLoading = false;
						return false;
					}
				}
				else
				{
					// show previous gems.
					max = index;
					index = (index - listCapacity) > 0 ? (index - listCapacity): 0;
				}

				if (max >= eventsWithImage.Count) {
					displayedLastGem = true;
				}else {
					displayedLastGem = false;
				}

				if (index == 0) {
					reachedFront = true;
				}
				else {
					reachedFront = false;
				}

				firstGemIndexOnDisplay = index;
				lastGemIndexOnDisplay = max;

				IDownload downloader = DependencyService.Get<IDownload> ();
				List<string> filesTodownliad = new List<string> ();
				for (int i = index; i < max; i++) {//eventsWithImage.Count - 1
					filesTodownliad.Add (Constants.SERVICE_BASE_URL + eventsWithImage [i].event_media);
				}

				bool doneDownloading = await downloader.DownloadFiles (filesTodownliad);

				downloader = null;
				filesTodownliad.Clear();
				filesTodownliad = null;

				try {
					int cou = listViewContainer.Children.Count; 
					if (cou > 0) {
						listViewContainer.Children.Clear ();
					}
				} catch (Exception ex) {

				}

				for (int i = index; i < max; i++) {
					try {
						await AddToScrollView (new CustomGemItemModel {
							Description = eventsWithImage [i].event_details,
							Source = App.DownloadsPath + Path.GetFileName (eventsWithImage [i].event_media),
							ID = eventsWithImage [i].event_id.ToString(),
							GroupTitle = eventsWithImage [i].emotion_title,
							CellIndex = i
						});
					} catch (Exception ex) {
						var test = ex.Message;
					}
				}


				//await masterScroll.ScrollToAsync( 0, 10, false );
				return true;

			} catch (Exception ex) {
				var test = ex.Message;
			}

			return false;
		}

		async Task<bool> AddActionsToView(int index, bool showNextGems = true)
		{
			int listCapacity = 10;
			int max  = 0;

			try 
			{
				isLoading = true;

				if (showNextGems) 
				{
					//display next gems
					max = (index + listCapacity) <= actionsWithImage.Count ? (index + listCapacity) : actionsWithImage.Count;
					if (index >= max || actionsWithImage == null || actionsWithImage.Count == 0)
					{
						displayedLastGem = true;
						isLoading = false;
						return false;
					}
				}
				else
				{
					// show previous gems.
					max = index;
					index = (index - listCapacity) > 0 ? (index - listCapacity): 0;
				}

				if (max >= actionsWithImage.Count) {
					displayedLastGem = true;
				}else {
					displayedLastGem = false;
				}

				if (index == 0) {
					reachedFront = true;
				}
				else {
					reachedFront = false;
				}

				firstGemIndexOnDisplay = index;
				lastGemIndexOnDisplay = max;

				IDownload downloader = DependencyService.Get<IDownload> ();
				List<string> filesTodownliad = new List<string> ();
				for (int i = index; i < max; i++)
				{
					filesTodownliad.Add (Constants.SERVICE_BASE_URL + actionsWithImage[i].action_media);
				}

				bool doneDownloading = await downloader.DownloadFiles (filesTodownliad);

				downloader = null;
				filesTodownliad.Clear();
				filesTodownliad = null;

				try {
					int cou = listViewContainer.Children.Count; 
					if (cou > 0) {
						listViewContainer.Children.Clear ();
					}
				} catch (Exception ex) {

				}

				for (int i = index; i < max; i++) {
					try {
						await AddToScrollView (new CustomGemItemModel {
							Description = actionsWithImage [i].action_details,
							Source = App.DownloadsPath + Path.GetFileName (actionsWithImage [i].action_media),
							ID = actionsWithImage [i].goalaction_id.ToString(),
							GroupTitle = actionsWithImage [i].goal_title,
							CellIndex = i
						});
					} catch (Exception ex) {
						var test = ex.Message;
					}
				}


				//await masterScroll.ScrollToAsync( 0, 0, false );

				return true;
			} catch (Exception ex) {
				var test = ex.Message;
			}

			return false;
		}

		async Task<bool> AddToScrollView(CustomGemItemModel gemModel)
		{
			try {
				CustomLayout scrollLayout = null;
				Image image = null;
				StackLayout cellSpacing = null;
				StackLayout bgStack = null;
				Label detailsLabel = null;
				Label groupTitleLabel = null;

				scrollLayout = new CustomLayout ();
				scrollLayout.BackgroundColor = Color.FromRgb(219,221,221);
				scrollLayout.WidthRequest = App.screenWidth;
				scrollLayout.Padding = new Thickness(0,0,0,App.screenHeight * .04);
				detailsLabel = new Label ();
				detailsLabel.Text = gemModel.Description;
				detailsLabel.TextColor = Color.White; // Color.FromRgb(8, 135, 224);//Color.FromRgb(8, 159, 245);
				detailsLabel.FontFamily = Constants.HELVERTICA_NEUE_LT_STD;
				detailsLabel.FontSize = 18;
				detailsLabel.WidthRequest = App.screenWidth - (App.screenWidth *.01);
				detailsLabel.HorizontalOptions = LayoutOptions.Center;
				detailsLabel.YAlign = TextAlignment.Center;
				detailsLabel.XAlign = TextAlignment.Center;

				bgStack = new StackLayout ();
				bgStack.WidthRequest = App.screenWidth;
				bgStack.HeightRequest = 85;
				bgStack.BackgroundColor = Color.Black;//Color.FromRgb(220, 220, 220);
				bgStack.Opacity = .2;

				if(!string.IsNullOrEmpty(detailsLabel.Text))
				{
					int textleng = detailsLabel.Text.Length;
				}

				image = new Image {
					Aspect = Aspect.Fill,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					WidthRequest = App.screenWidth,
					HeightRequest = (App.screenHeight * .35),
					Rotation= 0,
					Source = gemModel.Source
				};

				Image nextBtn = new Image
				{
					Source = "nxtArrow_gem.png",
					WidthRequest = App.screenWidth * .1,
					HeightRequest = App.screenWidth * .1

				};

				if (previousTitle == null || !string.IsNullOrEmpty(gemModel.GroupTitle) && gemModel.GroupTitle != previousTitle)
				{
					groupTitleLabel = new Label
					{
						FontFamily = Constants.HELVERTICA_NEUE_LT_STD,
						FontSize = 19,
						YAlign = TextAlignment.Start,
						VerticalOptions = LayoutOptions.Center,
						TextColor = Color.White,
						Text = "  " + gemModel.GroupTitle.Trim()
					};
					StackLayout titleHolder = new StackLayout{
						Children={groupTitleLabel},
						BackgroundColor =  Color.FromRgb(111, 199, 251),
						Padding = 0,
						Orientation = StackOrientation.Horizontal,
						WidthRequest= App.screenWidth
					};
					scrollLayout.AddChildToLayout (titleHolder, 0, 0);
					scrollLayout.AddChildToLayout (image, 0, 4);
					scrollLayout.AddChildToLayout (bgStack, 0, 16);//16 - appear @ center of img. 26 - text appear at bottom corner of img.
					scrollLayout.AddChildToLayout (detailsLabel, 1, 16);
					scrollLayout.AddChildToLayout (nextBtn, 50, 23);

				}else
				{
					scrollLayout.AddChildToLayout (image, 0, 0);
					scrollLayout.AddChildToLayout (bgStack, 0, 12);// 12 - aliended center to img. // 22 - align to bottom of img.
					scrollLayout.AddChildToLayout (detailsLabel, 1, 12);
					scrollLayout.AddChildToLayout (nextBtn, 50, 19);
				}

				if (!string.IsNullOrEmpty(gemModel.GroupTitle)) {
					previousTitle = gemModel.GroupTitle.Trim();
				}

				listViewContainer.Children.Add(scrollLayout);

				gemModel = null; // to clear mem.

				return true;
			} catch (Exception ex) {
				var test = ex.Message;
			}

			return false;
		}

		async void OnScroll(object sender, ScrolledEventArgs e)
		{
			if(  masterScroll.ScrollY > ( masterStack.Height - Device.OnPlatform( 512, 550, 0 ) )  )
			{
				masterScroll.Scrolled -= OnScroll;
				if (!displayedLastGem) {
					progressBar.ShowProgressbar ("loading..");
					await LoadMoreGemsClicked ();
					progressBar.HideProgressbar ();
				} else {
					progressBar.ShowToast ("Reached end of the list..");
				}

				await Task.Delay (TimeSpan.FromSeconds (3));
				masterScroll.Scrolled += OnScroll;
			}
			else if( masterScroll.ScrollY < Device.OnPlatform( -50, 5, 0 )  )
			{
				masterScroll.Scrolled -= OnScroll;
				if (!reachedFront) {
					progressBar.ShowProgressbar ("loading..");
					await LoadPreviousGems ();
				} else {
					progressBar.ShowToast ("Reached starting of the list..");
				}

				await Task.Delay (TimeSpan.FromSeconds (3));
				progressBar.HideProgressbar ();
				masterScroll.Scrolled += OnScroll;

			}
		}

		async Task<bool>LoadMoreGemsClicked ()
		{
			try 
			{
				if (isEmotionsListing) { // the gems displayed will be Events for each emotion.
					bool isSuccess = await AddEventsToView (lastGemIndexOnDisplay);
				}
				else
				{
					bool isSuccess = await AddActionsToView(lastGemIndexOnDisplay, true);
				}
				await masterScroll.ScrollToAsync( 0, 0, false );
			}
			catch (Exception ex)
			{
				DisplayAlert ( Constants.ALERT_TITLE, "Low memory error.", Constants.ALERT_OK );
				return false;
			}
			return true;
		}

		async Task<bool> LoadPreviousGems()
		{
			try {
				if (isEmotionsListing) { // the gems displayed will be Events for each emotion.
					bool isSuccess = await AddEventsToView (firstGemIndexOnDisplay - 1, false); // false = load previous gems
				}
				else {
					bool isSuccess = await AddActionsToView(firstGemIndexOnDisplay - 1, false); // false = load previous gems
				}
				await masterScroll.ScrollToAsync( 0, 0, false );
			}
			catch (Exception ex) {
				return false;
			}
			return true;
		}

		public void Dispose()
		{
			masterLayout = null;
			progressBar = null;
			mainTitleBar = null;
			masterScroll = null;
			masterStack = null;
			eventsWithImage = null;
			actionsWithImage = null;
			emotionsButtion = null;
			goalsButton = null;
			goalsAndDreamsLabel = null;
			emotionLabel = null;
			emotionListingBtnTapgesture = null;
			goalsListingBtnTapgesture = null;
			listViewContainer = null;
			bool isLoading = false;

			//GC.SuppressFinalize(this);
		}

		public void ScrollVisibleItems( int visbleIndex )
		{
		}

		//		~GemsMainPage()
		//		{
		//			Dispose ();
		//		}
		//
		//		protected virtual void Dispose(bool isDisposing)
		//		{
		//			if (isDisposing) {
		//
		//			}
		//
		//		}
		//
	}

	public class CustomGemItemModel
	{
		string description;

		public string Description
		{
			get { return description; }
			set
			{
				string trimmedText = string.Empty;
				if (value.Length > 70)
				{
					trimmedText = value.Substring(0, 70); // should adjst the height of bg img as well.
					trimmedText += "...";
				}
				else
				{
					trimmedText = value;
				}

				description = trimmedText;
			}
		}

		public string Source { get; set; }
		public string ID { get; set; }
		public string GroupTitle{ get; set;}
		public int CellIndex {
			get;
			set;
		}

		public CustomGemItemModel()
		{
		}
		public void Dispose()
		{
			GC.Collect();
		}
	}
}
