

using Android.Media;
using System;
using System.IO;
[assembly: Xamarin.Forms.Dependency(typeof(PurposeColor.Droid.Dependency.AudioRecorder))]
namespace PurposeColor.Droid.Dependency
{
    public class AudioRecorder : PurposeColor.interfaces.IAudioRecorder
    {
        Android.Media.MediaRecorder _recorder;
        Android.Media.MediaPlayer _player;
        string fileName;
        string path;
        string folder;
            
        string directoryname;

        public string AudioPath
        {
            get
            {
                return path;
            }
        }

        public AudioRecorder()
        {
            try
            {
                folder = Android.OS.Environment.ExternalStorageDirectory.Path;
                directoryname = Path.Combine(folder, "Purposecolor/Audio");
            }
            catch (Exception ex)
            {
                String test = ex.ToString();
            }
            
        }

        public bool RecordAudio()
        {
            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(directoryname))
            {
                return false;
            }

            //--test
            
            // get all under root:
                /*
                var directories = Directory.EnumerateDirectories("./");
                foreach (var directory in directories)
                {
                    Console.WriteLine(directory);
                }
             */

            // read text file:
                /*
                var text = File.ReadAllText("TestData/ReadMe.txt");
                Console.WriteLine(text);
                */

            // xml stream reader:
               /*
                using (TextReader reader = new StreamReader("./TestData/test.xml"))
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(MyObject));
                    var xml = (MyObject)serializer.Deserialize(reader);
                }
                */

            try
            {
                Directory.CreateDirectory(directoryname);
                fileName = string.Format("Audio{0}.aac", DateTime.Now.ToString("yyyyMMddHHmmss"));
                path = Path.Combine(directoryname, fileName);

                _recorder = new MediaRecorder();
                _recorder.Reset();
				_recorder.SetAudioSource(AudioSource.Mic);
				_recorder.SetOutputFormat(OutputFormat.Default);
				_recorder.SetAudioEncoder(AudioEncoder.Aac);
                _recorder.SetOutputFile(path);
                _recorder.Prepare();
                _recorder.Start();
          
                return true;
            }
            catch (System.Exception ex)
            {
                _recorder = null;
                fileName = string.Empty;
                var test = ex.Message;
                return false;
            }
        }

        public MemoryStream StopRecording()
        {
            try
            {
                _recorder.Stop();
                _recorder.Reset();
                _recorder.Release();
                _recorder = null;

              //  _reader.Close();

                MemoryStream memStream = new MemoryStream();
                using (FileStream fs = File.OpenRead(path))
                {
                    fs.CopyTo(memStream);
                }

                return memStream;
            }
            catch (Exception ex)
            {
                var test = ex.ToString();
                return null;

            }
        }

        public void PlayAudio()
        {
            try
            {
                if (String.IsNullOrEmpty(path))
                {
                    return;
                }

                if (_player == null)
                {
                    _player = new MediaPlayer();
                }

                _player.Reset();
                _player.SetDataSource(path);
                _player.Prepare();
                _player.Start();
            }
            catch (Exception ex)
            {
                _player.Stop();
                _player.Reset();
                _player.Release();
                _player = null;
                String test = ex.ToString();
            }
        }

    }
}