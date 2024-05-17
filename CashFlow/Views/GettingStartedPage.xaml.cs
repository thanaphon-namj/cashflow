namespace CashFlow.Views;

public partial class GettingStartedPage : ContentPage
{
    public GettingStartedPage()
    {
        InitializeComponent();
    }

    void SkipClicked(System.Object sender, System.EventArgs e)
    {
        Preferences.Default.Set("isSetup", true);
        App.Current.MainPage = new AppShell();
    }

    void GetStartClicked(System.Object sender, System.EventArgs e)
    {
        Preferences.Default.Set("isSetup", true);
        App.Current.MainPage = new Views.SetupPage();
    }
}
