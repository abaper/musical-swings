using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfMusicalSwingPlayer
{
    public class SwingViewItem
    {
        public int Angle { get; set; }
    }

    /// <summary>
    /// Interaction logic for Swings.xaml
    /// </summary>
    public partial class Swings : Window
    {
        private SwingDispatch _swingDispatch;
        private ArduinoConnector _arduinoConnector;

        public Swings()
        {
            InitializeComponent();
            SwingList.ItemsSource = new List<SwingViewItem>()
            {
                new SwingViewItem()
                {
                    Angle = 10
                },
                new SwingViewItem(){Angle = 15},
                new SwingViewItem(){Angle = 10},
                new SwingViewItem(){Angle = 30},
                new SwingViewItem(){Angle = 10},
                new SwingViewItem(){Angle = 50}
            };
            var allComs = SerialPort.GetPortNames();
            foreach (var allCom in allComs)
            {
                ComPorts.Items.Add(allCom);
            }
        }

        private void Connect_OnClick(object sender, RoutedEventArgs e)
        {
            if(ComPorts.SelectedValue==null)
                throw new InvalidOperationException("Please select port");
            _swingDispatch = new SwingDispatch(new []{0,1,2,3,4,5},new NoteMapper(0,new PlayingDevice()));
            _arduinoConnector = new ArduinoConnector(ComPorts.SelectedValue.ToString(),_swingDispatch);
        }
    }
}
