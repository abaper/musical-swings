using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Midi;

namespace MusicalSwingPlayer
{
    public partial class MainForm : Form
    {
        private OutputDevice _outputDevice = null;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadDeviceList();
            PlayNotes();
        }

        private void PlayNotes()
        {
            
        }

        private void LoadDeviceList()
        {
            for (int i = 0; i < OutputDevice.InstalledDevices.Count; ++i)
            {
                midiDeviceList.Items.Add(string.Format("   {0}: {1}", i, OutputDevice.InstalledDevices[i].Name));
                _outputDevice = OutputDevice.InstalledDevices[i];
            }
            _outputDevice.Open();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _outputDevice.Close();
        }

        private void playSounds_Click(object sender, EventArgs e)
        {
            _outputDevice.SendProgramChange(Channel.Channel2, Instrument.Agogo);
            _outputDevice.SendProgramChange(Channel.Channel1, Instrument.AcousticGrandPiano);
            _outputDevice.SendControlChange(Channel.Channel1, Midi.Control.Expression, 100);
            _outputDevice.SendPitchBend(Channel.Channel1, 8195);
            _outputDevice.SendPitchBend(Channel.Channel2, 8195);
            // Play C, E, G in half second intervals.
            _outputDevice.SendNoteOn(Channel.Channel1, Pitch.C4, 90);
            //Thread.Sleep(500);
            _outputDevice.SendNoteOn(Channel.Channel1, Pitch.E4, 80);
           // Thread.Sleep(500);
            _outputDevice.SendNoteOn(Channel.Channel1, Pitch.G4, 80);
            Thread.Sleep(500);
             _outputDevice.SendNoteOn(Channel.Channel2, Pitch.C4, 90);
        }
    }
}
