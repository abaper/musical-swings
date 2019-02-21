using System.Diagnostics;
using System.IO.Ports;
using System.Text;

namespace WpfMusicalSwingPlayer
{
    public class ArduinoConnector
    {
        
        private SerialPort _port;
        private SwingDispatch _dispatch;

        public ArduinoConnector(string commPort, SwingDispatch dispatch)
        {
            _dispatch = dispatch;
            _port = new SerialPort(commPort, 9600)
            {
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                RtsEnable = true,
                DtrEnable = true
            };
            _port.DataReceived += PortOnDataReceived;
           _port.Open();
        }

        private void PortOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = sender as SerialPort;
            string line = "";
            byte[] buff = new byte[200];
            
            //if (port != null)

            line = port.ReadLine();
            _dispatch.AddPositions(line);
            //var text = Encoding.ASCII.GetString(buff);
            Trace.TraceInformation($"data received {line}");

        }
    }
}