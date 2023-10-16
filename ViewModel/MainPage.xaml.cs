using Microsoft.Maui.Platform;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using Plugin.LocalNotification.EventArgs;
using System.Security.Principal;

namespace TimeLecture
{
    public partial class MainPage : ContentPage
    {
        TimeSpan timeSpan;

        IDispatcherTimer timer;
        List<Lesson> lessons;
        NotificationRequest notification;
        Lesson currientLesson;
        private readonly INotificationService _notificationService;
        string titleText = "Осталось до начала пары";
        bool isBreak;

        public MainPage()
        {
            InitializeComponent();

            // Создаём уведомление
            notification = new NotificationRequest()
            {
                NotificationId = 1,
                Title = titleText,
                Description = $"Осталось {timeSpan.Hours}:{timeSpan.Minutes}:{timeSpan.Seconds}",
                CategoryType = NotificationCategoryType.Progress,
                Silent = true,
                //Time = DateTime.Now + timeSpan
                Android = new AndroidOptions()
                {
                    IsProgressBarIndeterminate = isBreak,
                    ProgressBarMax = 90 * 60,
                    ProgressBarProgress = (int)(timeSpan.TotalSeconds)
                }
            };

            lessons = new List<Lesson>()
            {
                new Lesson(08, 00, 09, 30),
                new Lesson(09, 50, 11, 20),
                new Lesson(11, 40, 13, 10),
                new Lesson(13, 20, 14, 50),
                new Lesson(15, 10, 16, 40),
                new Lesson(16, 50, 18, 20),
                new Lesson(18, 30, 20, 00)
            };

            timer = Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Start();

            timer.Tick += Timer_Tick;
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            currientLesson = GetLastLesson(DateTime.Now, lessons, out isBreak);
            var time = DateTime.Now.TimeOfDay;
            titleText = $"До начала 1-й пары";
            if (currientLesson is null)
            {
                timeSpan = (lessons[0].TimeSpanStart - time).Duration();
            }
            else
            {
                if (currientLesson == lessons.Last() && isBreak)
                {
                    // Время до начала следующей пары следующего дня
                    DateTime tomorrow = DateTime.Today.AddDays(1);
                    timeSpan = lessons[0].TimeSpanStart + (tomorrow - DateTime.Now).Duration();
                }
                else
                {
                    if (isBreak)
                    {
                        var nextLesson = lessons[lessons.IndexOf(currientLesson) + 1];
                        timeSpan = (nextLesson.TimeSpanStart - DateTime.Now.TimeOfDay).Duration();
                        titleText = $"До начала следующей пары";
                    }
                    else
                    {
                        timeSpan = (currientLesson.TimeSpanEnd - DateTime.Now.TimeOfDay).Duration();
                        titleText = $"До конца пары";
                    }
                }
            }

            // Устанавливаем текст
            TimerLabel.BindingContext = timeSpan;
            TitleLabel.Text = titleText;

            // Отображаем уведомление
            notification.Description = $"Осталось {timeSpan.Hours}:{timeSpan.Minutes}:{timeSpan.Seconds}";
            notification.Android.ProgressBarProgress = (int)timeSpan.TotalSeconds;
            LocalNotificationCenter.Current.Show(notification);

        }

        public Lesson GetLastLesson(DateTime dateTime, List<Lesson> lessons, out bool isBreak)
        {
            isBreak = true;
            var time = dateTime.TimeOfDay;
            int lessonStarted = 0;
            int lessonFinished = 0;
            int total = lessons.Count;
            for (int i = 0; i < total; i++)
            {
                if (dateTime.TimeOfDay >= lessons[i].TimeSpanStart)
                    lessonStarted = i + 1;
                if (time >= lessons[i].TimeSpanEnd)
                    lessonFinished = i + 1;
            }
            // Занятия ещё не начались
            if (lessonStarted == 0 && lessonFinished == 0)
                return null;

            // Последнее занятие закончилось
            if(lessonStarted == total && lessonFinished == total)
                return lessons[total - 1];

            // Сейчас перемена
            if (lessonStarted == lessonFinished)
                return lessons[lessonStarted - 1];

            isBreak = false;
            // Сейчас идёт занятие
            return lessons[lessonStarted - 1];
        }
    }
}