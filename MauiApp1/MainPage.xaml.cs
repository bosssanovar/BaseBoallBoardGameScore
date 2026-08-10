namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public void MainPageBase()
        {
            InitializeComponent();
        }

        override protected void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = this;
        }
    }
}
