using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfPomodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private TimeSpan time;
        private string currentStatus;
        private string[] statuses = { "Работа:", "Отдых:" };

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            currentStatus = statuses[0];
        }

        /// <summary>
        /// Запускает таймер с указанной продолжительностью работы.
        /// </summary>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtMinutes.Text, out int minutes) && int.TryParse(txtRestMinutes.Text, out int restMinutes))
            {
                time = TimeSpan.FromMinutes(minutes);
                timer.Start();
            }
            else
            {
                MessageBox.Show("Введите корректное количество минут работы и отдыха.");
            }
        }

        /// <summary>
        /// Останавливает таймер.
        /// </summary>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        /// <summary>
        /// Сбрасывает таймер и очищает дисплей.
        /// </summary>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            time = TimeSpan.Zero;
            txtTime.Text = "00:00";
            currentStatus = statuses[0];
            txtRestMinutes.Text = "10";
            txtMinutes.Text = "25";
        }

        /// <summary>
        /// Обрабатывает событие тика таймера, обновляет время и переключается между периодами работы и отдыха.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (time > TimeSpan.Zero)
            {
                time = time.Add(TimeSpan.FromSeconds(-1));
                txtTime.Text = $"{currentStatus} {time.ToString(@"mm\:ss")}";
            }
            else
            {
                FlashBackground();
                timer.Stop();
                currentStatus = currentStatus == statuses[1] ? statuses[0] : statuses[1];
                
                if (int.TryParse(txtRestMinutes.Text, out int restMinutes) && int.TryParse(txtMinutes.Text, out int workMinutes))
                {
                    time = currentStatus == statuses[1] ? TimeSpan.FromMinutes(restMinutes) : TimeSpan.FromMinutes(workMinutes);
                    timer.Start();
                }
                else
                {
                    MessageBox.Show("Введите корректное количество минут работы и отдыха.");
                }
            }
        }

        /// <summary>
        /// Мигает фоновым цветом окна
        /// </summary>
        private async void FlashBackground()
        {
            var originalBrush = this.Background;
            var blackBrush = new SolidColorBrush(Colors.Black);
            var transparentBrush = new SolidColorBrush(Colors.Transparent);

            for (int i = 0; i < 8; i++)
            {
                this.Background = blackBrush;
                await Task.Delay(100);
                this.Background = originalBrush;
                await Task.Delay(100);
            }

            this.Background = originalBrush;
        }
    }
}