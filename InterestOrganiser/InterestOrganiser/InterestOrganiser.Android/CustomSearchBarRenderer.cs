using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using InterestOrganiser.Custom;
using InterestOrganiser.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
namespace InterestOrganiser.Droid
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        public CustomSearchBarRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;


            var searchIconId = Control.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
            if (searchIconId > 0)
            {
                var searchPlateIcon = Control.FindViewById(searchIconId);
                (searchPlateIcon as ImageView).SetColorFilter(Android.Graphics.Color.White, PorterDuff.Mode.SrcIn);
            }

            int searchCloseIconId = Control.Resources.GetIdentifier("android:id/search_close_btn", null, null);
            if (searchCloseIconId > 0)
            {
                var closeIcon = Control.FindViewById(searchCloseIconId);
                (closeIcon as ImageView).SetColorFilter(Android.Graphics.Color.White, PorterDuff.Mode.SrcIn);
            }

            LinearLayout linearLayout = Control.GetChildAt(0) as LinearLayout;
            linearLayout = linearLayout.GetChildAt(2) as LinearLayout;
            linearLayout = linearLayout.GetChildAt(1) as LinearLayout;
            if (linearLayout != null)
            {
                linearLayout.Background.ClearColorFilter();
                linearLayout.Background.SetColorFilter(Android.Graphics.Color.White, PorterDuff.Mode.SrcIn);
            }
        }
    }
}