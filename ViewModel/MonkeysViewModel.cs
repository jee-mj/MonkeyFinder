using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
	MonkeyService _monkeyService;
	public MonkeysViewModel(MonkeyService monkeyService)
	{
		Title = "Monkey Finder";
		_monkeyService = monkeyService;
		//GetMonkeysCommand = new Command(async () => await GetMonkeysAsync());
		//GetMonkeysCommand. // Toolkit handles this below under [RelayCommand]
	}
    [RelayCommand]
    async Task GetMonkeysAsync()
	{
		if(IsBusy)
			return;

		try
		{
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
		[RelayCommand]
		async Task GoToDetailsAsync(Monkey monkey)
		{
			if (monkey is null)
				return;

            // await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?id={monkey.Name}");
            await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true, new Dictionary<string, object>
                {
                    {"Monkey", monkey}
                });
        }
    }
    public ObservableCollection<Monkey> Monkeys { get; } = new();

}
