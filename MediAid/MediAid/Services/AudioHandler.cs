using System;
using System.Collections.Generic;
using System.Text;

namespace MediAid.Services
{
    public abstract class AudioHandler
    {
        public bool IsRecording { get; set; }
        string recordingPath;

        public virtual void Init(string recordingPath)
        {
            this.recordingPath = recordingPath;
        }


        public abstract bool StartRecording(string fileName);
        public abstract bool ResumeRecording();
        public abstract bool PauseRecording();
        public abstract bool StopRecording();
        public abstract bool PlayRecording(string fileName);

        public string RecordingPath {

            get
            {
                return recordingPath;
            }

            set
            {
                recordingPath = value;
            }
        }

    }

}
