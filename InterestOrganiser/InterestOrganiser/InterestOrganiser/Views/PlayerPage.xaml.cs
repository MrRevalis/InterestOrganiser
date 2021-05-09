using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InterestOrganiser.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerPage : ContentPage
    {
        public PlayerPage()
        {
            InitializeComponent();

            DeviceDisplay.MainDisplayInfoChanged += OrientationChanged;

        }

        ~PlayerPage()
        {
            DeviceDisplay.MainDisplayInfoChanged -= OrientationChanged;
        }

        private void OrientationChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            DisplayOrientation orientation = e.DisplayInfo.Orientation;
            switch (orientation)
            {
                case DisplayOrientation.Portrait: videoPlayer.Aspect = Aspect.AspectFit; break;
                case DisplayOrientation.Landscape: videoPlayer.Aspect = Aspect.AspectFill; break;
            }
        }
    }
}