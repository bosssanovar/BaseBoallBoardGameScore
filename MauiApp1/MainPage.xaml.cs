namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ViewModel();
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            double scale = Math.Min(width / BoardContainer.WidthRequest, height / BoardContainer.HeightRequest);
            BoardContainer.Scale = scale;
        }
    }
}
