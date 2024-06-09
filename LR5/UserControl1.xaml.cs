using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LR5
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private List<Horse> horses;
        private List<Ellipse> ellipses;
        private Thread updateThread;
        private bool isRunning;
        private Random random = new Random();

        public UserControl1()
        {
            InitializeComponent();
            random = new Random();

            horses = new List<Horse>
        {
            new Horse { X = 0, Y = 0, Speed = random.Next(1, 6), Color = Brushes.Red, Radius = 50, Angle = 0, },
            new Horse { X = 0, Y = 0, Speed = random.Next(1, 6), Color = Brushes.Blue, Radius = 100, Angle = 0, },
            new Horse {X = 0, Y = 0, Speed = random.Next(1, 6), Color = Brushes.AliceBlue, Radius = 150, Angle = 0}
        };

            ellipses = new List<Ellipse>();

            foreach (var horse in horses)
            {
                Ellipse ellipse = new Ellipse
                {
                    Width = 20,
                    Height = 20,
                    Fill = horse.Color
                };

                ellipse.DataContext = horse;
                ellipse.SetBinding(Canvas.TopProperty, new Binding("Y"));
                ellipse.SetBinding(Canvas.LeftProperty, new Binding("X"));
                ellipse.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
                ellipse.MouseRightButtonDown += Ellipse_MouseRightButtonDown;
                raceTrack.Children.Add(ellipse);
                ellipses.Add(ellipse);
            }
        }


        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            Horse horse = ellipse.DataContext as Horse;
            MessageBox.Show($"Скорость: {horse.Speed}", "Скорость лошади", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Ellipse_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            Horse horse = ellipse.DataContext as Horse;
            MessageBox.Show($"Координаты:\nX:{horse.X}\nY:{horse.Y}", "Координаты лошади", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateLoop()
        {
            while (isRunning)
            {
                double centerX = raceTrack.ActualWidth / 2;
                double centerY = raceTrack.ActualHeight / 2;

                foreach (var horse in horses)
                {
                    horse.Angle += horse.Speed;

                    double angleInRadians = horse.Angle * Math.PI / 180;

                    horse.X = centerX + horse.Radius * Math.Cos(angleInRadians);
                    horse.Y = centerY + horse.Radius * Math.Sin(angleInRadians);
                }

                Dispatcher.Invoke(() =>
                {
                    foreach (var ellipse in ellipses)
                    {
                        EllipseUpdate(ellipse);
                    }
                });

                Thread.Sleep(50);
            }
        }

        private void EllipseUpdate(Ellipse ellipse)
        {
            Horse horse = (Horse)ellipse.DataContext;
            Canvas.SetLeft(ellipse, horse.X);
            Canvas.SetTop(ellipse, horse.Y);
        }

        public void StartRace()
        {
            if (!isRunning)
            {
                isRunning = true;
                updateThread = new Thread(new ThreadStart(UpdateLoop));
                updateThread.Start();
            }
        }

        public void PauseRace()
        {
            isRunning = false;
        }

        public void ResetRace()
        {
            updateThread.Abort();
            foreach (var horse in horses)
            {
                horse.X = 0;
                horse.Y = 0;
                horse.Angle = 0;
                horse.Speed = random.Next(1, 6);
            }
            foreach (var ellipse in ellipses)
            {
                Canvas.SetLeft(ellipse, 0);
                Canvas.SetTop(ellipse, 0);
            }
        }
    }


    public class Horse : INotifyPropertyChanged
    {
        private double x;
        private double y;
        private double speed;
        private double radius;
        private double angle;
        private Brush color;

        public double Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                OnPropertyChanged("Radius");
            }
        }

        public double Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                OnPropertyChanged("Angle");
            }
        }

        public double X
        {
            get { return x; }
            set
            {
                x = value;
                OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                OnPropertyChanged("Y");
            }
        }

        public double Speed
        {
            get { return speed; }
            set
            {
                speed = value;
                OnPropertyChanged("Speed");
            }
        }

        public Brush Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
