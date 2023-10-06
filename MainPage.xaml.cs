using System;
using System.Diagnostics;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace TimeLecture
{
    public partial class MainPage : ContentPage
    {
        private EventsPage eventsPage;
        int count = 0;
        IDispatcherTimer timer;
        private int _pairNumber;
        List<Pair> pairs;

        public MainPage()
        {
            eventsPage = new EventsPage();
            InitializeComponent();

            pairs = new List<Pair>()
            {
                new Pair(8, 0, 9, 30),
                new Pair(9, 50, 11, 20),
                new Pair(11, 40, 13, 10),
                new Pair(13, 20, 14, 50),
                new Pair(15, 10, 16, 40),
                new Pair(16, 50, 18, 20),
                new Pair(18, 30, 20, 0)
            };

            timer = Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();

            timer.Tick += (s, e) =>
            {
                // Получаем текущее время
                TimeSpan timeSpan;

                // Получаем текущую пару
                Pair currentPair = GetCurrentPair(DateTime.Now, pairs);
                string titleText;
                // Если сейчас пара
                if (currentPair != null)
                {
                    // Время до конца текущей пары
                    var pairEndTime = GetTimeToEndOfCurrentPair(DateTime.Now);
                    timeSpan = pairEndTime - DateTime.Now.TimeOfDay;
                    titleText = $"Осталось до конца пары";
                }
                // Если сейчас перемена
                else
                {
                    // Время до начала следующей пары
                    var pairStartTime = GetTimeToStartOfNextPair(DateTime.Now);
                    timeSpan = pairStartTime - DateTime.Now.TimeOfDay;
                    titleText = $"До начала следующей пары";
                }

                // Устанавливаем текст
                TimerLabel.Text = $"{timeSpan.Hours} час {timeSpan.Minutes} минут {timeSpan.Seconds} секунд";
                TitleLabel.Text = titleText;
            };
        }

        async void OpenEventPage() => await Navigation.PushModalAsync(eventsPage);

        private void EventPageButton_Clicked(object sender, EventArgs e) => OpenEventPage();

        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e) => OpenEventPage();

        public TimeSpan GetTimeToEndOfCurrentPair(DateTime now)
        {
            // Получаем текущую пару
            Pair currentPair = GetCurrentPair(now, pairs);

            // Получаем текущее время
            DateTime currentTime = now;

            // Получаем время окончания текущей пары
            DateTime endTime = currentTime.AddHours(currentPair.endHour).AddMinutes(currentPair.endMinute);

            // Возвращаем разницу между текущим временем и временем окончания текущей пары
            return endTime - currentTime;
        }

        public TimeSpan GetTimeToStartOfNextPair(DateTime now)
        {
            // Получаем текущую пару
            Pair currentPair = GetCurrentPair(now, pairs);

            // Получаем следующую пару
            Pair nextPair = GetNextPair(currentPair);

            // Получаем текущее время
            DateTime currentTime = now;

            // Получаем время начала следующей пары
            DateTime startTime = currentTime.AddHours(nextPair.startHour).AddMinutes(nextPair.startMinute);

            // Возвращаем разницу между текущим временем и временем начала следующей пары
            return startTime - currentTime;
        }

        public Pair GetCurrentPair(DateTime now, List<Pair> pairs)
        {
            // Получаем текущее время
            DateTime currentTime = now;

            foreach (Pair pair in pairs)
            {
                DateTime pairStartTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, pair.startHour, pair.startMinute, 0);
                DateTime pairEndTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, pair.endHour, pair.endMinute, 0);

                if (currentTime >= pairStartTime && currentTime <= pairEndTime)
                {
                    return pair;
                }
            }

            // Если пара не найдена, возвращаем null
            return null;
        }

        public Pair GetNextPair(Pair currentPair)
        {
            // Индекс текущей пары
            int index = pairs.IndexOf(currentPair);

            // Если пара не последняя
            if (index < pairs.Count - 1)
            {
                // Возвращаем следующую пару
                return pairs[index + 1];
            }

            // Если пара последняя, возвращаем null
            return null;
        }

        public  bool IsPair(DateTime time, List<Pair> pairs)
        {
            // Получаем текущий час и минуту
            int hour = time.Hour;
            int minute = time.Minute;

            // Перебираем пары
            foreach (Pair pair in pairs)
            {
                // Если текущее время находится в пределах пары, возвращаем "Пара"
                if (hour >= pair.startHour && hour < pair.endHour && 
                    minute >= pair.startMinute && minute < pair.endMinute)
                {
                    return true; // Пара
                }
            }
            return false; // Перемена
        }
    }

    public class Pair
    {
        public int startHour { get; set; }
        public int startMinute { get; set; }
        public int endHour { get; set; }
        public int endMinute { get; set; }

        public Pair(int startHour, int startMinute, int endHour, int endMinute)
        {
            this.startHour = startHour;
            this.startMinute = startMinute;
            this.endHour = endHour;
            this.endMinute = endMinute;
        }
    }
}