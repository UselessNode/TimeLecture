using System.Collections.ObjectModel;
namespace TimeLecture;
public partial class EventsPage : ContentPage
{
	public EventsPage()
	{
        InitializeComponent();
        BindingContext = this;
        Events = new ObservableCollection<AcademEvent> { };
        collectionView.ItemsSource = Events;
    }

    private ObservableCollection<AcademEvent> _events;
    public ObservableCollection<AcademEvent> Events
    {
        get { return _events; }
        set
        {
            _events = value;
            OnPropertyChanged(nameof(Events));
        }
    }

    private void AddButtonNew_Clicked(object sender, EventArgs e)
    {
        Events.Add(new AcademEvent { Name = "Тест", Place = "Тест", Date = new DateTime().AddDays(2), Description = "Тест!" });
    }
}