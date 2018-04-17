using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfMusicalSwingPlayer
{
    public class SwingEvent
    {
        public float Roll { get; set; }
        public float Pitch { get; set; }
        public float Heading { get; set; }
        public float Gx { get; set; }
        public float Gy { get; set; }
        public float Gz { get; set; }
        public float PrevGy { get; set; }
        public float PrevGz { get; set; }
    }
}



