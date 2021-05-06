using InterestOrganiser.Services;
using Xamarin.Forms;

namespace InterestOrganiser
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<IMovieDB, MovieDB>();
            DependencyService.Register<IBookApi, BookApi>();
            DependencyService.Register<IGameApi, GameApi>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
