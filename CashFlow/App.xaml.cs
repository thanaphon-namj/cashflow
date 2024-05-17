namespace CashFlow;

public partial class App : Application
{
    public static Data.ItemDatabase Database;

    public App()
    {
        InitializeComponent();
        Database = new Data.ItemDatabase();

        bool isSetup = Preferences.Default.Get("isSetup", false);
        if (isSetup)
        {
            MainPage = new AppShell();
        }
        else
        {
            MainPage = new Views.GettingStartedPage();
        }
    }
}

