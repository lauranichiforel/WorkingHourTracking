using SQLitePCL;

namespace HourTracking
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Batteries.Init();
            MainPage = new NavigationPage(new ZileListPage());
        }
    }
}
