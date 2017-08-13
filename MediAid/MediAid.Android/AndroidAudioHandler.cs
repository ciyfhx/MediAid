using Android.Media;
using MediAid.Services.Android;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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
            player = new MediaPlayer();
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

            //TESTING
            player.SetDataSource(Path.Combine(RecordingPath, fileName));
            player.Prepare();
            player.Start();

            return IsRecorderNull();
        }

        private bool IsRecorderNull()
        {
            return (recorder == null);
        }
    }
}
