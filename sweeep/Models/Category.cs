using System.ComponentModel;

namespace sweeep;

public class Category : INotifyPropertyChanged
{
    public Category(string name)
    {
        _name = name;
    }

    private string _name { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}