namespace TimeLecture
{
    public class Lesson
    {
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int EndHour { get; set; }
        public int EndMinute { get; set; }

        public TimeSpan TimeSpanStart { get; set; }
        public TimeSpan TimeSpanEnd { get; set; }

        public Lesson(int startHour, int startMinute, int endHour, int endMinute)
        {
            this.StartHour = startHour;
            this.StartMinute = startMinute;
            this.EndHour = endHour;
            this.EndMinute = endMinute;

            TimeSpanStart = new TimeSpan(startHour, startMinute,0);
            TimeSpanEnd = new TimeSpan(endHour, endMinute,0);
        }
        public Lesson(TimeSpan startTimeSpan, TimeSpan endTimeSpan)
        {
            TimeSpanStart = startTimeSpan;
            TimeSpanEnd = endTimeSpan;
        }
    }
}