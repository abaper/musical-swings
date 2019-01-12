using System;
using System.Collections.Generic;
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
        }
    }
}
