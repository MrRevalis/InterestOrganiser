using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InterestOrganiser.Custom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingScreen : Frame
    {

        public static readonly BindableProperty ShouldWorkProperty =
            BindableProperty.Create(
                nameof(ShouldWork),
                typeof(bool),
                typeof(LoadingScreen),
                defaultValue: default(bool),
                propertyChanged: ShouldWorkPropertyChanged
                );

        private static void ShouldWorkPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (LoadingScreen)bindable;
            control.frame.IsVisible = (bool)newValue;
            control.indicator.IsRunning = (bool)newValue;
        }

        public bool ShouldWork
        {
            get
            {
                return (bool)base.GetValue(ShouldWorkProperty);
            }
            set
            {
                base.SetValue(ShouldWorkProperty, value);
            }
        }

        public LoadingScreen()
        {
            InitializeComponent();
        }
    }
}