using Android.Media;
using MediAid.Services.Android;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidAudioHandler))]
namespace MediAid.Services.Android
{
    public class AndroidAudioHandler : AudioHandler
    {
        MediaRecorder recorder;
        MediaPlayer player;

        string fileName;


        public override void Init(string recordingPath)  
        {
            base.Init(recordingPath);
            recorder = new MediaRecorder();
            IsRecording = false;
        }

        public override bool StartRecording(string fileName)
        {
            this.fileName = fileName;

            recorder.SetAudioSource(AudioSource.Mic);
            //Save recording in .3gpp file
            recorder.SetOutputFormat(OutputFormat.ThreeGpp);
            recorder.SetAudioEncoder(AudioEncoder.AmrNb);
            recorder.SetOutputFile(Path.Combine(RecordingPath, fileName));
            recorder.Prepare();
            recorder.Start();
            IsRecording = true;


            Debug.WriteLine($"Recording File: {fileName}");
            Debug.WriteLine(Path.Combine(RecordingPath, fileName));

            return IsRecorderNull();
        }

        public override bool PauseRecording()
        {
            recorder?.Pause();
            IsRecording = false;
            return IsRecorderNull();
        }

        public override bool ResumeRecording()
        {
            recorder?.Resume();
            IsRecording = true;
            return IsRecorderNull();
        }

        public override bool StopRecording()
        {
            recorder?.Stop();
            IsRecording = false;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return IsRecorderNull();
        }

        public override bool PlayRecording(string fileName)
        {
            if (player != null) return false;
            Debug.WriteLine($"TEST Recording File: {fileName}");
            Debug.WriteLine(Path.Combine(RecordingPath, fileName));

            ListFile(RecordingPath);
            
           player = new MediaPlayer();
           player.SetDataSource(Path.Combine(RecordingPath, fileName));
           player.Prepare();
           player.Start();

           player = null;
            


            return true;
        }

        /// <summary>
        /// Remove the file of the recording
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>If successful</returns>
        public override bool RemoveRecording(string fileName)
        {
            ListFile(RecordingPath);
            //Delete the file
            Debug.WriteLine(Path.Combine(RecordingPath, fileName));
            File.Delete(Path.Combine(RecordingPath, fileName));

            ListFile(RecordingPath);

            return true;

        }


        [Obsolete("Testing Purposes")]
        private void ListFile(string path)
        {
            var files = Directory.GetFiles(path);
            
            foreach(var file in files)
            {
                Debug.WriteLine($"File: {file}");
            }



        }


        private bool IsRecorderNull()
        {
            return (recorder == null);
        }
    }
}
