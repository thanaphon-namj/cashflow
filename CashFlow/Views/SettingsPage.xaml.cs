namespace CashFlow.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        string name = Preferences.Default.Get("Name", "Guest");
        nameText.Text = name;
    }

    async void ClearTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        bool answer = await DisplayAlert("Confirm Clear Data", $"You want to clear all data?", "Yes", "No");

        if (answer == true)
        {
            await App.Database.DeleteItemsAsync();
        }
    }
}
