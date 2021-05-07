using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InterestOrganiser.Custom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingBar : ContentView
    {
        public Image Star1;
        public Image Star2;
        public Image Star3;
        public Image Star4;
        public Image Star5;
        public RatingBar()
        {
            InitializeComponent();

            Star1 = new Image();
            Star2 = new Image();
            Star3 = new Image();
            Star4 = new Image();
            Star5 = new Image();

            ratingBar.Children.Add(Star1);
            ratingBar.Children.Add(Star2);
            ratingBar.Children.Add(Star3);
            ratingBar.Children.Add(Star4);
            ratingBar.Children.Add(Star5);
        }

        public static readonly BindableProperty ImageHeightProperty =
            BindableProperty.Create(
                propertyName: "ImageHeight",
                returnType: typeof(double),
                declaringType: typeof(RatingBar),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: ImageHeightPropertyChanged
                );

        public double ImageHeight
        {
            get { return (double)base.GetValue(ImageHeightProperty); }
            set { base.SetValue(ImageHeightProperty, value); }
        }

        private static void ImageHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as RatingBar;
            if(control != null)
            {
                control.Star1.HeightRequest = (double)newValue;
                control.Star2.HeightRequest = (double)newValue;
                control.Star3.HeightRequest = (double)newValue;
                control.Star4.HeightRequest = (double)newValue;
                control.Star5.HeightRequest = (double)newValue;
            }
        }

        public static readonly BindableProperty ImageWidthProperty =
            BindableProperty.Create(
                propertyName: "ImageWidth",
                returnType: typeof(double),
                declaringType: typeof(RatingBar),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: ImageWidthPropertyChanged
                );

        public double ImageWidth
        {
            get { return (double)base.GetValue(ImageWidthProperty); }
            set { base.SetValue(ImageWidthProperty, value); }
        }

        private static void ImageWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as RatingBar;
            if (control != null)
            {
                control.Star1.WidthRequest = (double)newValue;
                control.Star2.WidthRequest = (double)newValue;
                control.Star3.WidthRequest = (double)newValue;
                control.Star4.WidthRequest = (double)newValue;
                control.Star5.WidthRequest = (double)newValue;
            }
        }

        public static readonly BindableProperty HorizontalOptionsProperty =
            BindableProperty.Create(
                propertyName: "HorizontalOptions",
                returnType: typeof(LayoutOptions),
                declaringType: typeof(RatingBar),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: HorizontalOptionsPropertyChanged
                );

        public LayoutOptions HorizontalOptions
        {
            get { return (LayoutOptions)base.GetValue(HorizontalOptionsProperty); }
            set { base.SetValue(HorizontalOptionsProperty, value); }
        }

        private static void HorizontalOptionsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as RatingBar;
            if (control != null)
            {
                control.ratingBar.HorizontalOptions = (LayoutOptions)newValue;
            }
        }

        public static readonly BindableProperty VerticalOptionsProperty =
            BindableProperty.Create(
                propertyName: "VerticalOptions",
                returnType: typeof(LayoutOptions),
                declaringType: typeof(RatingBar),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: VerticalOptionsPropertyChanged
                );

        public LayoutOptions VerticalOptions
        {
            get { return (LayoutOptions)base.GetValue(VerticalOptionsProperty); }
            set { base.SetValue(VerticalOptionsProperty, value); }
        }

        private static void VerticalOptionsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as RatingBar;
            if (control != null)
            {
                control.ratingBar.VerticalOptions = (LayoutOptions)newValue;
            }
        }

        public static readonly BindableProperty StarsAmountProperty =
            BindableProperty.Create(
                propertyName: "StarsAmount",
                returnType: typeof(double),
                declaringType: typeof(RatingBar),
                defaultValue: default(double),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: StarsAmountPropertyChanged
                );

        public double StarsAmount
        {
            get { return (double)base.GetValue(StarsAmountProperty); }
            set { base.SetValue(StarsAmountProperty, value); }
        }

        private static void StarsAmountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as RatingBar;
            control.StarsAmount = (double)newValue;
            if (control != null)
            {
                FillStars((double)newValue, control);
            }
        }

        private static void FillStars(double newValue, RatingBar control)
        {
            var value = newValue > 5 ? newValue / 2 : newValue;
            value = Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2;

            switch (value)
            {
                case 0:
                    control.Star1.Source = "emptyStar.png";
                    control.Star2.Source = "emptyStar.png";
                    control.Star3.Source = "emptyStar.png";
                    control.Star4.Source = "emptyStar.png";
                    control.Star5.Source = "emptyStar.png";
                    break;
                case 0.5:
                    control.Star1.Source = "halfStar.png";
                    control.Star2.Source = "emptyStar.png";
                    control.Star3.Source = "emptyStar.png";
                    control.Star4.Source = "emptyStar.png";
                    control.Star5.Source = "emptyStar.png";
                    break;
                case 1:
                    control.Star1.Source = "fullStar.png";
                    control.Star2.Source = "emptyStar.png";
                    control.Star3.Source = "emptyStar.png";
                    control.Star4.Source = "emptyStar.png";
                    control.Star5.Source = "emptyStar.png";
                    break;
                case 1.5:
                    control.Star1.Source = "fullStar.png";
                    control.Star2.Source = "halfStar.png";
                    control.Star3.Source = "emptyStar.png";
                    control.Star4.Source = "emptyStar.png";
                    control.Star5.Source = "emptyStar.png";
                    break;
                case 2:
                    control.Star1.Source = "fullStar.png";
                    control.Star2.Source = "fullStar.png";
                    control.Star3.Source = "emptyStar.png";
                    control.Star4.Source = "emptyStar.png";
                    control.Star5.Source = "emptyStar.png";
                    break;
                case 2.5:
                    control.Star1.Source = "fullStar.png";
                    control.Star2.Source = "fullStar.png";
                    control.Star3.Source = "halfStar.png";
                    control.Star4.Source = "emptyStar.png";
                    control.Star5.Source = "emptyStar.png";
                    break;
                case 3:
                    control.Star1.Source = "fullStar.png";
                    control.Star2.Source = "fullStar.png";
                    control.Star3.Source = "fullStar.png";
                    control.Star4.Source = "emptyStar.png";
                    control.Star5.Source = "emptyStar.png";
                    break;
                case 3.5:
                    control.Star1.Source = "fullStar.png";
                    control.Star2.Source = "fullStar.png";
                    control.Star3.Source = "fullStar.png";
                    control.Star4.Source = "halfStar.png";
                    control.Star5.Source = "emptyStar.png";
                    break;
                case 4:
                    control.Star1.Source = "fullStar.png";
                    control.Star2.Source = "fullStar.png";
                    control.Star3.Source = "fullStar.png";
                    control.Star4.Source = "fullStar.png";
                    control.Star5.Source = "emptyStar.png";
                    break;
                case 4.5:
                    control.Star1.Source = "fullStar.png";
                    control.Star2.Source = "fullStar.png";
                    control.Star3.Source = "fullStar.png";
                    control.Star4.Source = "fullStar.png";
                    control.Star5.Source = "halfStar.png";
                    break;
                case 5:
                    control.Star1.Source = "fullStar.png";
                    control.Star2.Source = "fullStar.png";
                    control.Star3.Source = "fullStar.png";
                    control.Star4.Source = "fullStar.png";
                    control.Star5.Source = "fullStar.png";
                    break;
            }
        }
    }
}