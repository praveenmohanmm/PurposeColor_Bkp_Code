﻿using System;
using Xamarin.Forms;
using CustomControls;
using PurposeColor.interfaces;

namespace PurposeColor
{
	public class AudioRecorderPage : ContentPage
	{
		CustomLayout masterLayout;
		IAudioRecorder audioRecorder;
		public AudioRecorderPage ()
		{
			NavigationPage.SetHasNavigationBar(this, false);

			audioRecorder = DependencyService.Get<IAudioRecorder>();
			masterLayout = new CustomLayout();
			masterLayout.BackgroundColor = Color.Gray;

            PurposeColor.CustomControls.PurposeColorTitleBar titleBar = new PurposeColor.CustomControls.PurposeColorTitleBar(Color.FromRgb(8, 137, 216), "Purpose Color", Color.Black, "back");
			masterLayout.AddChildToLayout(titleBar, 0, 0);

			Button recordBtn = new Button 
			{
				Text = "Record"
			};
			recordBtn.Clicked += RecordBtn_Clicked;
			masterLayout.AddChildToLayout(recordBtn, 0, 40);

			Button stopRecordBtn = new Button 
			{
				Text = "Stop Recording"
			};
			stopRecordBtn.Clicked += StopRecordBtn_Clicked;
			masterLayout.AddChildToLayout(stopRecordBtn, 0, 50);


			Button PlaybackBtn = new Button 
			{
				Text = "Playback"
			};
			PlaybackBtn.Clicked +=	PlaybackBtn_Clicked;
			masterLayout.AddChildToLayout(PlaybackBtn, 0, 60);
            masterLayout.AddChildToLayout(titleBar, 0, 0);


			Content = masterLayout;
		}

		void StopRecordBtn_Clicked (object sender, EventArgs e)
		{
			audioRecorder.StopRecording ();
		}

		void RecordBtn_Clicked (object sender, EventArgs e)
		{
            if (audioRecorder == null)
            {
                audioRecorder = DependencyService.Get<IAudioRecorder>();
            }
			bool isAudioRecording = audioRecorder.RecordAudio ();
			if (isAudioRecording == false) {
				DisplayAlert ("Audio recording", "Sorry the audio recording service is not available now, please try again later", "OK");
			}
			else
			{
				DisplayAlert ("Audio recording", "Audio recording stared", "OK");
			}
		}

		void backButton_Clicked(object sender, System.EventArgs e)
		{
			//Navigation.PushAsync( new GraphPage() );
		}
		void PlaybackBtn_Clicked(object sender, System.EventArgs e)
		{
			audioRecorder.PlayAudio ();
		}
	}
}
