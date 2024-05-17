using Microsoft.Maui.ApplicationModel;

namespace CashFlow.Views;

public partial class DetailPage : ContentPage
{
    private static List<Models.Category> IncomeCategories = new List<Models.Category>
    {
        new Models.Category() { name = "Money", value = "money", type = "Income" },
        new Models.Category() { name = "Salary", value = "salary", type = "Income" },
        new Models.Category() { name = "Other", value = "other", type = "Income" },
    };

    private static List<Models.Category> SpendCategories = new List<Models.Category>
    {
        new Models.Category(){ name = "Bill", value = "bill", type = "Spend" },
        new Models.Category(){ name = "Food", value = "food", type = "Spend" },
        new Models.Category(){ name = "Rent", value = "rent", type = "Spend" },
        new Models.Category(){ name = "Shopping", value = "shopping", type = "Spend" },
        new Models.Category(){ name = "Travel", value = "travel", type = "Spend" },
        new Models.Category() { name = "Other", value = "other", type = "Spend" },
    };

    private Models.Item Item;

    public DetailPage(Models.Item vm)
    {
        InitializeComponent();
        Item = vm;
        BindingContext = Item;

        datePicker.MaximumDate = DateTime.Now;
        datePicker.Date = DateTime.Now;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Item.ID == 0)
        {
            Item.type = "Income";
            saveButton.IsVisible = true;
            stackButton.IsVisible = false;
        }

        if (Item.type == "Income")
        {
            incomeButton.BackgroundColor = (Color)App.Current.Resources["IncomeColor"];
            incomeButton.TextColor = Colors.White;
            spendButton.BackgroundColor = Colors.White;
            spendButton.TextColor = (Color)App.Current.Resources["SpendColor"];
            spendButton.BorderWidth = 1;
            categoryPicker.ItemsSource = IncomeCategories;
            categoryPicker.SelectedIndex = IncomeCategories.FindIndex(i => i.value == Item.category);
        }

        if (Item.type == "Spend")
        {
            incomeButton.BackgroundColor = Colors.White;
            incomeButton.TextColor = (Color)App.Current.Resources["IncomeColor"];
            incomeButton.BorderWidth = 1;
            spendButton.BackgroundColor = (Color)App.Current.Resources["SpendColor"];
            spendButton.TextColor = Colors.White;
            categoryPicker.ItemsSource = SpendCategories;
            categoryPicker.SelectedIndex = SpendCategories.FindIndex(i => i.value == Item.category);
        }

        if (Item.image != null)
        {
            var localFilePath = Item.image;
            imagePreview.Source = ImageSource.FromFile(localFilePath);
            imagePreview.IsVisible = true;
            galleryButton.IsVisible = false;
            cameraButton.IsVisible = false;
            removeButton.IsVisible = true;
        }
    }

    void IncomeClicked(System.Object sender, System.EventArgs e)
    {
        Item.type = "Income";
        incomeButton.BackgroundColor = (Color)App.Current.Resources["IncomeColor"];
        incomeButton.TextColor = Colors.White;
        spendButton.BackgroundColor = Colors.White;
        spendButton.TextColor = (Color)App.Current.Resources["SpendColor"];
        spendButton.BorderWidth = 1;
        categoryPicker.ItemsSource = IncomeCategories;
    }

    void SpendClicked(System.Object sender, System.EventArgs e)
    {
        Item.type = "Spend";
        incomeButton.BackgroundColor = Colors.White;
        incomeButton.TextColor = (Color)App.Current.Resources["IncomeColor"];
        incomeButton.BorderWidth = 1;
        spendButton.BackgroundColor = (Color)App.Current.Resources["SpendColor"];
        spendButton.TextColor = Colors.White;
        categoryPicker.ItemsSource = SpendCategories;
    }

    void CategoryPickerChanged(System.Object sender, System.EventArgs e)
    {
        var picker = (Picker)sender;
        var selected = picker.SelectedItem as Models.Category;
        Item.category = selected.value;
    }

    async void Save(System.Object sender, System.EventArgs e)
    {
        if (Item.type == null)
        {
            await DisplayAlert("Type Required", "Please select type", "OK");
            return;
        }

        if (Item.category == null)
        {
            await DisplayAlert("Category Required", "Please select category", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(Item.title))
        {
            await DisplayAlert("Title Required", "Please enter tittle", "OK");
            return;
        }

        if (Item.amount == 0)
        {
            await DisplayAlert("Amount Required", "Please enter amount", "OK");
            return;
        }

        await App.Database.SaveItemAsync(Item);
        await Navigation.PopAsync();
    }

    async void Delete(System.Object sender, System.EventArgs e)
    {
        bool answer = await DisplayAlert("Confirm Delete", $"You want to delete {Item.title}?", "Yes", "No");

        if (answer == true)
        {
            await App.Database.DeleteItemAsync(Item);
            await Navigation.PopAsync();
        }
    }

    async void BackClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PopAsync();
    }

    async void GalleryClicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.Default.PickPhotoAsync();
            string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

            using Stream sourceStream = await photo.OpenReadAsync();
            using FileStream localFileStream = File.OpenWrite(localFilePath);

            await sourceStream.CopyToAsync(localFileStream);

            if (localFilePath != "")
            {
                Item.image = localFilePath;
                imagePreview.Source = ImageSource.FromFile(localFilePath);
                imagePreview.IsVisible = true;
                galleryButton.IsVisible = false;
                cameraButton.IsVisible = false;
                removeButton.IsVisible = true;
            }
        }
        catch (Exception)
        {
        }
    }

    async void CameraClicked(System.Object sender, System.EventArgs e)
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);

                if (localFilePath != "")
                {
                    Item.image = localFilePath;
                    imagePreview.Source = ImageSource.FromFile(localFilePath);
                    imagePreview.IsVisible = true;
                    galleryButton.IsVisible = false;
                    cameraButton.IsVisible = false;
                    removeButton.IsVisible = true;
                }
            }
        }
    }

    void RemoveClicked(System.Object sender, System.EventArgs e)
    {
        Item.image = null;
        imagePreview.IsVisible = false;
        galleryButton.IsVisible = true;
        cameraButton.IsVisible = true;
        removeButton.IsVisible = false;
    }
}
