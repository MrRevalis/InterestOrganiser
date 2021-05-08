using InterestOrganiser.Views;
using Xamarin.Forms;

namespace InterestOrganiser
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("registration", typeof(RegistrationPage));
            Routing.RegisterRoute("detail", typeof(DetailPage));
            Routing.RegisterRoute("detailbook", typeof(BookDetailPage));
            Routing.RegisterRoute("detailgame", typeof(GameDetailPage));
            Routing.RegisterRoute("search", typeof(SearchPage));
            Routing.RegisterRoute("video", typeof(PlayerPage));

        }

    }
}
