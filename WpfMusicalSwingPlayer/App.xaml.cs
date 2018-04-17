using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfMusicalSwingPlayer
{
    public class SwingConfiguration
    {
        public string SwingId { get; set; }
        public string CommPort { get; set; }
    }
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
    }
}
