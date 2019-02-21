using System;
using Midi;

namespace WpfMusicalSwingPlayer
{
    public interface IPlayingDevice
    {
        void PlayNote(Pitch note);
    }

    public class PlayingDevice : IPlayingDevice,IDisposable
    {
        private OutputDevice _outputDevice;
        public PlayingDevice()
        {
            if (OutputDevice.InstalledDevices.Count == 0)
            {
                throw new InvalidOperationException("No MIDI devices installed.");
            }

            for (int i = 0; i < OutputDevice.InstalledDevices.Count; ++i)
            {
                _outputDevice = OutputDevice.InstalledDevices[i];
            }
            _outputDevice.Open();
        }
        public void PlayNote(Pitch note)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            _outputDevice.Close();
        }
    }
}