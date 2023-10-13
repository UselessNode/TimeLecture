using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLecture
{
    public class AcademEvent : BindableObject
    {
        //public string Name { get; set; }
        //public string Place { get; set; }
        //public DateTime Date { get; set; }
        //public string Description { get; set; }


        public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(AcademEvent));
        public static readonly BindableProperty PlaceProperty = BindableProperty.Create(nameof(Place), typeof(string), typeof(AcademEvent));
        public static readonly BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(AcademEvent));
        public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(AcademEvent));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public string Place
        {
            get { return (string)GetValue(PlaceProperty); }
            set { SetValue(PlaceProperty, value); }
        }

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

    }
}
