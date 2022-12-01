using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
	MonkeyService _monkeyService;
    IConnectivity _connectivity;
    IGeolocation _geolocation;
    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Monkey Finder";
        _monkeyService = monkeyService;
        _connectivity = connectivity;
        _geolocation = geolocation;
        //GetMonkeysCommand = new Command(async () => await GetMonkeysAsync());
        //GetMonkeysCommand. // Toolkit handles this below under [RelayCommand]
    }
    [RelayCommand]
    async Task GetClosestMonkey()
    {
        if(IsBusy || Monkeys.Count == 0)
            return;

        try
        {
            //Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            // Instead of having Geolocation API of .NET MAUI check for permissions
            // May want to do it using Permissions class.
            var location = await _geolocation.GetLastKnownLocationAsync();
            if (location is null)
            {
                location = await _geolocation.GetLocationAsync(
                    new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30),
                    });
            }

            var first = Monkeys.OrderBy(m => location.CalculateDistance( new Location(m.Latitude, m.Longitude), DistanceUnits.Kilometers)).FirstOrDefault();

            await Shell.Current.DisplayAlert("", first.Name + " " + first.Location, "OK");
        }
        catch (Exception ex)
        {

            Debug.WriteLine($"Unable to query location: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
    [RelayCommand]
    async Task GoToDetailsAsync(Monkey monkey)
    {
        if (monkey is null)
            return;

        await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true, new Dictionary<string, object>
            {
                {"Monkey", monkey}
            });
    }
    [RelayCommand]
    async Task GetMonkeysAsync()
	{
		if(IsBusy)
			return;

		try
		{
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Not Connected", $"Check your internet and try again!", "OK");
                return;
            }
			IsBusy = true;
			var monkeys = await _monkeyService.GetMonkeys();
			if (Monkeys.Count != 0)
				Monkeys.Clear();

			foreach (var monkey in monkeys)
				Monkeys.Add(monkey);
			// Note that the foreach loop above will raise an event every time a monkey is added to the ObservableCollection<Monkey>
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
			//await App.Current
			await Shell.Current.DisplayAlert("Error!", $"Unable to get monkeys: {ex.Message}", "OK");
		}
		finally
		{
            IsBusy = false;
        }

    }
    [RelayCommand]
    void DeleteMonkeys()
    {
        if (IsBusy)
            return;

		try
        {
            IsBusy = true;
			Monkeys.Clear();
		}
		catch (Exception ex)
		{
            Debug.WriteLine(ex);
            //await App.Current
            Shell.Current.DisplayAlert("Error!", $"Unable to delete monkeys: {ex.Message}", "OK");
        }
		finally
        {
            IsBusy = false;
        }
    }
    public ObservableCollection<Monkey> Monkeys { get; } = new();
}
