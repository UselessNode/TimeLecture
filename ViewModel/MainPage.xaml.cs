namespace TimeLecture
{
    public partial class MainPage : ContentPage
    {
        TimeSpan timeSpan;

        IDispatcherTimer timer;
        List<Lesson> lessons;

        public MainPage()
        {
            InitializeComponent();

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

            timer.Tick += (s, e) =>
            {
                // Получаем количество пар, которые уже были начаты
                //int amount = GetLessonAt(DateTime.Now, lessons);
                bool isBreak;
                var lesson = GetLastLesson(DateTime.Now, lessons, out isBreak);

                var time = DateTime.Now.TimeOfDay;
                string titleText = "Осталось до начала пары (0)";

                titleText = $"До начала 1-й пары";
                if(lesson is null)
                {
                    timeSpan = (lessons[0].TimeSpanStart - time).Duration();
                }
                else
                {
                    if( lesson == lessons.Last() && isBreak)
                    {
                        // Время до начала следующей пары следующего дня
                        DateTime tomorrow = DateTime.Today.AddDays(1);
                        timeSpan = lessons[0].TimeSpanStart + (tomorrow - DateTime.Now).Duration();
                    }
                    else
                    {
                        if (isBreak)
                        {
                            var nextLesson = lessons[lessons.IndexOf(lesson) + 1];
                            timeSpan = (nextLesson.TimeSpanStart - DateTime.Now.TimeOfDay).Duration();
                            titleText = $"До начала следующей пары";
                        }
                        else
                        {
                            timeSpan = (lesson.TimeSpanEnd - DateTime.Now.TimeOfDay).Duration();
                            titleText = $"До конца пары";
                        }
                    }
                }

                //if (time < lessons[0].TimeSpanStart)
                //{
                //    // 1) Сейчас перемена. Перемена до начала всех занятий
                //    timeSpan = (lessons[0].TimeSpanStart - DateTime.Now.TimeOfDay).Duration();
                //    titleText = $"До начала 1-й пары (1)";
                //}
                //else
                //{
                //    if (DateTime.Now.TimeOfDay > lessons[index].TimeSpanEnd) // Сейчас > Конец данного занятия
                //    {
                //        if (index + 1 >= lessons.Count())
                //        {
                //            // 2) Сейчас перемена. Между парами
                //            var pairEndTime = lessons[index].TimeSpanEnd;
                //            timeSpan = (pairEndTime - DateTime.Now.TimeOfDay).Duration();
                //            titleText = $"До начала пары (2)";
                //        }
                //        else
                //        {
                //            // 3) Сейчас перемена. Перемена после всех занятий
                //            DateTime tomorrow = DateTime.Today.AddDays(1);
                //            timeSpan = lessons[0].TimeSpanStart + (tomorrow - DateTime.Now).Duration();
                //            titleText = $"До начала 1-й пары (3)";
                //        }

                //    }
                //    else
                //    {
                //        // 4) Сейчас занятие.
                //        timeSpan = (lessons[index + 1].TimeSpanStart - DateTime.Now.TimeOfDay).Duration();
                //        titleText = $"До конца пары (4)";
                //    }
                //}


                //if (index >= 0)
                //{
                //    if (DateTime.Now.TimeOfDay <= lessons[index].TimeSpanEnd) // 
                //    {
                //        var pairEndTime = lessons[index].TimeSpanEnd;
                //        timeSpan = (pairEndTime - DateTime.Now.TimeOfDay).Duration();
                //    }
                //    else
                //    {

                //        timeSpan = (lessons[index + 1].TimeSpanStart - DateTime.Now.TimeOfDay).Duration();
                //        titleText = $"До начала следующей пары";
                //    }
                //}
                //else
                //{
                //    titleText = $"До начала первой пары";
                //    if (DateTime.Now.TimeOfDay < lessons[0].TimeSpanStart)
                //    {
                //        timeSpan = (lessons[0].TimeSpanStart - DateTime.Now.TimeOfDay).Duration();
                //        titleText = $"До начала 1-й пары";
                //    }
                //    else
                //    {
                //        // Время до начала следующей пары следующего дня
                //        DateTime tomorrow = DateTime.Today.AddDays(1);
                //        timeSpan = lessons[0].TimeSpanStart + (tomorrow - DateTime.Now).Duration();
                //    }
                //}

                // Устанавливаем текст
                TimerLabel.BindingContext = timeSpan;
                TitleLabel.Text = titleText;

            };
        } 


        public Lesson GetLastLesson(DateTime dateTime, List<Lesson> lessons, out bool isBreak)
        {
            isBreak = true;
            var time = dateTime.TimeOfDay;
            int lessonStarted = lessons.Count;
            int lessonFinished = lessons.Count;
            int total = lessons.Count;
            for (int i = 0; i < total; i++)
            {
                if (dateTime.TimeOfDay <= lessons[i].TimeSpanStart)
                    lessonStarted = i + 1;
                if (time <= lessons[i].TimeSpanEnd)
                    lessonFinished = i + 1;
            }
            // Занятия ещё не начались
            if (lessonStarted == 0 && lessonFinished == 0)
                return null;

            // Последнее занятие закончилось
            if(lessonStarted == total && lessonFinished == total)
                return lessons[total - 1];

            // Сейчас перемена
            if (lessonStarted != lessonFinished)
                return lessons[lessonStarted];

            isBreak = false;
            // Сейчас идёт занятие
            return lessons[lessonStarted];

            //var timeSpanEndFirst = lessons[0].TimeSpanEnd;
            //var timeSpanEndLast = lessons[lessons.Count - 1].TimeSpanEnd;

            //if(currentTime > timeSpanEndFirst || currentTime < timeSpanEndLast)
            //    for (int i = 0; i < lessons.Count - 1; i++)
            //    {
            //        if (currentTime >= lessons[i].TimeSpanStart &&
            //            (i == lessons.Count - 1 && currentTime < lessons[i + 1].TimeSpanStart))
            //        {
            //            return i;
            //        }
            //    }
            //return -1;
        }
    }
}