namespace MonkeyFinder.ViewModel;
// All View Models inherit from the BaseViewModel
// see: https://blog.ericmuchenah.com/mvc-vs-mvvm
public partial class BaseViewModel : ObservableObject
{
    public BaseViewModel()
    {
    }
    [ObservableProperty]
    // [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;
    [ObservableProperty]
    string title;
    public bool IsNotBusy => !isBusy;
    
}

#region Using Inheritance
//public class BaseViewModel : INotifyPropertyChanged
//{
//    bool isBusy;
//    string title;
//
//    public bool IsBusy
//    {
//        get => isBusy;
//        set
//        {
//            if (isBusy == value)
//                return;
//
//            isBusy = value;
//            OnPropertyChanged();
//            OnPropertyChanged(nameof(IsNotBusy));
//        }
//    }
//    public string Title
//    {
//        get => title;
//        set
//        {
//            if (title == value)
//                return;
//            title = value;
//            OnPropertyChanged();
//        }
//    }
//    public bool IsNotBusy => !IsBusy;
//
//    public event PropertyChangedEventHandler PropertyChanged;
//
//    public void OnPropertyChanged([CallerMemberName] string name = null)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
//    }
//}
#endregion

#region Using INotifyPropertyChanged 
//[INotifyPropertyChanged]
//public partial class BaseViewModel
//{
//    bool isBusy;
//    string title;
//    public BaseViewModel()
//    {
//        SetProperty(ref isBusy, true);
//    }
//}
#endregion