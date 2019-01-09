using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Services
{

    public interface IAudioService
    {
        void PlayAudioFile(string fileName);
    }
}
