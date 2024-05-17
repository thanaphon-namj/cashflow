using static System.Net.Mime.MediaTypeNames;
using static Java.Util.Jar.Attributes;

namespace CashFlow.Views;

public partial class SetupPage : ContentPage
{
    public string Name { get; set; }
    public decimal Balance { get; set; }

    public SetupPage()
    {
        InitializeComponent();
    }

    void SkipClicked(System.Object sender, System.EventArgs e)
    {
        App.Current.MainPage = new AppShell();
    }

    async void NextClicked(System.Object sender, System.EventArgs e)
    {
        var name = nameEntry.Text;
        var balance = balanceEntry.Text;

        if (!string.IsNullOrWhiteSpace(name))
        {
            Preferences.Default.Set("Name", name);
        }

        if (!string.IsNullOrWhiteSpace(balance))
        {
            await App.Database.SaveItemAsync(new Models.Item()
            {
                type = "Income",
                category = "money",
                title = "Initial Current Balance",
                amount = decimal.Parse(balance),
                date = DateTime.Today,
                note = "Initial current balance from system"
            });
        }

        App.Current.MainPage = new AppShell();
    }
}
