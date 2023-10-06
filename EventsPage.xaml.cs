using System.Collections.ObjectModel;

namespace TimeLecture;

public partial class EventsPage : ContentPage
{

	public EventsPage()
	{
        //InitializeComponent();

        StackLayout stackLayout = new StackLayout();

        foreach (var academEvent in _events)
        {
            Label nameLabel = new Label();
            nameLabel.Text = academEvent.Name;

            Label placeLabel = new Label();
            placeLabel.Text = academEvent.Place;

            Label dateLabel = new Label();
            dateLabel.Text = academEvent.Date.ToString("dd.MM.yyyy");

            Label descriptionLabel = new Label();
            descriptionLabel.Text = academEvent.Description;

            stackLayout.Children.Add(nameLabel);
            stackLayout.Children.Add(placeLabel);
            stackLayout.Children.Add(dateLabel);
            stackLayout.Children.Add(descriptionLabel);
        }

        Content = stackLayout;

    }

    private List<AcademEvent> _events = new List<AcademEvent>()
    {
        new AcademEvent
        {
            Name = "Мероприятие 1",
            Place = "Место 1",
            Date = new DateTime(2023, 10, 3),
            Description = "Описание 1"
        },
        new AcademEvent
        {
            Name = "Мероприятие 2",
            Place = "Место 2",
            Date = new DateTime(2023, 10, 4),
            Description = "Описание 2"
        }
    };
}