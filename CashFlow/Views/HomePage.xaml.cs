using System.Collections.ObjectModel;

namespace CashFlow.Views;

public partial class HomePage : ContentPage
{
    public ObservableCollection<Models.Item> Items { get; set; }

    public HomePage()
    {
        InitializeComponent();
        Items = new ObservableCollection<Models.Item>();
        BindingContext = this;

        string name = Preferences.Default.Get("Name", "Guest");
        nameText.Text = $"✌️Hey  {name}!";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Items.Clear();
        LoadData();
    }

    public async void LoadData()
    {
        decimal balance = 0;
        decimal income = 0;
        decimal spend = 0;

        var items = await App.Database.GetItemsAsync();
        foreach (var item in items)
        {
            Items.Add(item);

            if (item.type == "Income")
            {
                income += item.amount;
            }

            if (item.type == "Spend")
            {
                spend += item.amount;
            }
        }

        balance = income - spend;

        balanceText.Text = balance.ToString("C");
        incomeText.Text = income.ToString("C");
        spendText.Text = spend.ToString("C");
    }

    async void Add(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new Views.DetailPage(new Models.Item()));
    }

    async void SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        var collectionView = (CollectionView)sender;
        var selected = collectionView.SelectedItem as Models.Item;
        await Navigation.PushAsync(new Views.DetailPage(selected));
    }
}
