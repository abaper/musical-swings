using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Midi;
using Newtonsoft.Json;

namespace WpfMusicalSwingPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<SwingListener> SwingListeners = new ObservableCollection<SwingListener>();
        public IList<SwingConfiguration> SwingConfigurations = new List<SwingConfiguration>()
        {
            new SwingConfiguration() { SwingId = "Swing-1", CommPort="COMM1" },
            new SwingConfiguration() { SwingId = "Swing-2", CommPort="COMM1"},
            new SwingConfiguration() { SwingId = "Swing-3", },
            new SwingConfiguration() { SwingId = "Swing-4" },
            new SwingConfiguration() { SwingId = "Swing-5" },
            new SwingConfiguration() { SwingId = "Swing-6" }
        };

        private SerialPort _port;
        private OutputDevice _outputDevice;
        private SwingListener _swingListener;

        public MainWindow()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
            for (int i = 0; i < OutputDevice.InstalledDevices.Count; ++i)
            {
               
                _outputDevice = OutputDevice.InstalledDevices[i];
            }
            _outputDevice.Open();

            var allComs = SerialPort.GetPortNames();
            foreach (var allCom in allComs)
            {
                ComPorts.Items.Add(allCom);
            }
            Swings.ItemsSource = SwingListeners;

            var instruments = Enum.GetValues(typeof(Instrument)).Cast<Instrument>().Select(c=>c.ToString());
            foreach (var instrument1 in instruments)
            {
                Instruments.Items.Add(instrument1);
            }

            var channels = Enum.GetValues(typeof(Instrument)).Cast<Channel>().Select(c => c.ToString());
            foreach (var channel in channels)
            {
                Channels.Items.Add(channel);
            }


        }

        
        
        private void B_OnClick(object sender, RoutedEventArgs e)
        {
            var inst = (Instrument) Enum.Parse(typeof(Instrument), Instruments.SelectedItem.ToString());
            var channel = (Channel)Enum.Parse(typeof(Channel), Channels.SelectedItem.ToString());
            SwingListeners.Add(new SwingListener(ComPorts.SelectedItem.ToString(), _outputDevice, channel, inst));
        }

        
    }
}
