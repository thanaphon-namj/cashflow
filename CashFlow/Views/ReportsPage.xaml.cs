using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CashFlow.Views;

public partial class ReportsPage : ContentPage
{
    public string State { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ToDate { get; set; }

    public ObservableCollection<Models.Item> Transactions { get; set; }
    public ObservableCollection<Models.Item> Items { get; set; }

    public ReportsPage()
    {
        InitializeComponent();
        State = "Month";
        StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        ToDate = StartDate.AddMonths(1);
        dateText.Text = StartDate.ToString("MMMM - yyyy");
        Transactions = new ObservableCollection<Models.Item>();
        Items = new ObservableCollection<Models.Item>();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Transactions.Clear();
        Items.Clear();
        LoadData();
    }

    public async void LoadData()
    {
        decimal balance = 0;
        decimal income = 0;
        decimal spend = 0;

        var result = await App.Database.GetItemsAsync(StartDate, ToDate);
        foreach (var item in result)
        {
            Transactions.Add(item);

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

        if (result.Count > 0)
        {
            var items = result.Where(i => i.type == "Spend").MaxBy(i => i.amount);
            if (items is not null)
            {
                Items.Add(items);
            }
        }

        balanceText.Text = balance.ToString("C");
        incomeText.Text = income.ToString("C");
        spendText.Text = spend.ToString("C");
    }

    void DayClicked(System.Object sender, System.EventArgs e)
    {
        State = "Day";
        StartDate = DateTime.Today;
        ToDate = StartDate.AddDays(1);
        dateText.Text = StartDate.ToString("dd MMMM yyyy");
        dayButton.BorderWidth = 1;
        monthButton.BorderWidth = 0;
        yearButton.BorderWidth = 0;

        Transactions.Clear();
        Items.Clear();
        LoadData();
    }

    void MonthClicked(System.Object sender, System.EventArgs e)
    {
        State = "Month";
        StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        ToDate = StartDate.AddMonths(1);
        dateText.Text = StartDate.ToString("MMMM - yyyy");
        dayButton.BorderWidth = 0;
        monthButton.BorderWidth = 1;
        yearButton.BorderWidth = 0;

        Transactions.Clear();
        Items.Clear();
        LoadData();
    }

    void YearClicked(System.Object sender, System.EventArgs e)
    {
        State = "Year";
        StartDate = new DateTime(DateTime.Today.Year, 1, 1);
        ToDate = StartDate.AddYears(1);
        dateText.Text = StartDate.ToString("yyyy");
        dayButton.BorderWidth = 0;
        monthButton.BorderWidth = 0;
        yearButton.BorderWidth = 1;

        Transactions.Clear();
        Items.Clear();
        LoadData();
    }

    void PrevClicked(System.Object sender, System.EventArgs e)
    {
        switch (State)
        {
            case "Day":
                StartDate = StartDate.AddDays(-1);
                ToDate = ToDate.AddDays(-1);
                dateText.Text = StartDate.ToString("dd MMMM yyyy");
                break;
            case "Month":
                StartDate = StartDate.AddMonths(-1);
                ToDate = ToDate.AddMonths(-1);
                dateText.Text = StartDate.ToString("MMMM - yyyy");
                break;
            case "Year":
                StartDate = StartDate.AddYears(-1);
                ToDate = ToDate.AddYears(-1);
                dateText.Text = StartDate.ToString("yyyy");
                break;

        }

        Transactions.Clear();
        Items.Clear();
        LoadData();
    }

    void NextClicked(System.Object sender, System.EventArgs e)
    {
        switch (State)
        {
            case "Day":
                StartDate = StartDate.AddDays(1);
                ToDate = ToDate.AddDays(1);
                dateText.Text = StartDate.ToString("dd MMMM yyyy");
                break;
            case "Month":
                StartDate = StartDate.AddMonths(1);
                ToDate = ToDate.AddMonths(1);
                dateText.Text = StartDate.ToString("MMMM - yyyy");
                break;
            case "Year":
                StartDate = StartDate.AddYears(1);
                ToDate = ToDate.AddYears(1);
                dateText.Text = StartDate.ToString("yyyy");
                break;

        }

        Transactions.Clear();
        Items.Clear();
        LoadData();
    }
}
