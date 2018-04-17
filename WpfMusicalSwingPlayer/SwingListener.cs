using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using Midi;
using Newtonsoft.Json;
using WpfMusicalSwingPlayer.Annotations;

namespace WpfMusicalSwingPlayer
{
    public class SwingListener:INotifyPropertyChanged
    {
        private readonly string _comm;
        private readonly OutputDevice _device;
        private readonly Channel _channel;
        private readonly Instrument _instrument;
        private SerialPort _port;
        //private StreamWriter _sampleFile;
        private float _pitchValue;
        public ObservableCollection<int> IntRanges = new ObservableCollection<int>(new List<int>(){10,15,25});

        private float _gy;
        private float _gz;
        private float _prevGy;
        private float _prevGz;
        public Dictionary<Pitch, MappingRange> Ranges = new Dictionary<Pitch, MappingRange>();
        private float _lowest;
        private float _highest;
        private int _velocity=80;

        public SwingListener(string comm,OutputDevice device,Channel channel,Instrument instrument)
        {
            _comm = comm;
            _device = device;
            _channel = channel;
            _instrument = instrument;
            InitComm();
            InitDevice();
        //    _sampleFile = new StreamWriter("samples.txt");
            Ranges.Add(Pitch.C4, new MappingRange() { Low = -100, High = 5 });
            Ranges.Add(Pitch.D4, new MappingRange() { Low = 5, High = 10 });
            Ranges.Add(Pitch.E4, new MappingRange() { Low = 10, High = 20 });
            Ranges.Add(Pitch.F4, new MappingRange() { Low = 30, High = 40 });

        }

        public string Channel
        {
            get { return _channel.ToString(); }
        }

        public string Instrument
        {
            get { return _instrument.ToString(); }
        }

        public int First
        {
            get { return (int)IntRanges[0]; }
            set
            {
                IntRanges[0] = value;
                OnPropertyChanged("First");
            }
        }


        public int Second
        {
            get { return (int)IntRanges[1]; }
            set
            {
                IntRanges[1] = value;
                OnPropertyChanged("Second");
            }
        }


        public int Third
        {
            get { return (int)IntRanges[2]; }
            set
            {
                IntRanges[2] = value;
                OnPropertyChanged("Third");
            }
        }

        public string Port
        {
            get { return _comm; }
        }

        public float PitchValue
        {
            get { return _pitchValue; }
        }

        public float Gy
        {
            get { return _gy; }
        }

        public float Gz
        {
            get { return _gz; }
        }

        public float PrevGy
        {
            get { return _prevGy; }
        }

        public float PrevGz
        {
            get { return _prevGz; }
        }
        private void InitDevice()
        {
            _device.SendProgramChange(_channel,_instrument);
            _device.SendPitchBend(_channel, 8195);
        }

        private void InitComm()
        {
            _port = new SerialPort(_comm, 115200);
            _port.Parity = Parity.None;
            _port.DataBits = 8;
            _port.StopBits = StopBits.One;
            _port.Handshake = Handshake.None;
            _port.RtsEnable = true;
            _port.DtrEnable = true;
            _port.DataReceived += PortOnDataReceived;
            _port.Open();
        }

        private void PortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            try
            {
                var @event = GetEvent(sender);
                var note = PlayNote(@event);
                AccountValues(@event, note);
            }
            catch (Exception e)
            {
                
            }
        }

        private void AccountValues(SwingEvent @event, Pitch note)
        {
            _pitchValue = @event.Pitch;
            OnPropertyChanged("PitchValue");
            _gy = @event.Gy;
            OnPropertyChanged("Gy");
            _gz = @event.Gz;
            OnPropertyChanged("Gz");
            _prevGy = @event.PrevGy;
            OnPropertyChanged("PrevGy");
            _prevGz = @event.PrevGz;
            OnPropertyChanged("PrevGz");
            Trace.WriteLine(note.ToString());
            if (@event.Pitch > Highest)
            {
                Highest = @event.Pitch;
            }
            if (Math.Abs(@event.Pitch) < Lowest)
            {
                Lowest = Math.Abs(@event.Pitch);
            }
        }

        private Pitch PlayNote(SwingEvent @event)
        {
            var noteMapper = new NoteMappnig(@event, IntRanges.ToList());
            var note = noteMapper.GetNote();
            _device.SendNoteOn(_channel, note, _velocity);
            return note;
        }

        private static SwingEvent GetEvent(object sender)
        {
            SerialPort port = (SerialPort) sender;
            string line = port.ReadLine();
            Trace.WriteLine(line);
            var @event = JsonConvert.DeserializeObject<SwingEvent>(line);
            return @event;
        }


        public float Lowest
        {
            get { return _lowest; }
            set
            {
                _lowest = value;
                OnPropertyChanged();
            }
        }

        public float Highest
        {
            get { return _highest; }
            set
            {
                _highest = value;
                OnPropertyChanged();
            }
        }

        public int Velocity
        {
            get { return _velocity; }
            set
            {
                _velocity=value; 
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}