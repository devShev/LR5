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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            horseRacingControl.StartRace();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            horseRacingControl.PauseRace();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            horseRacingControl.ResetRace();
        }
    }
}
