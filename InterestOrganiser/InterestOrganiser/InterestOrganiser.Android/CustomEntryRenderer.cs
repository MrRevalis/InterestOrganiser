using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using InterestOrganiser.Custom;
using InterestOrganiser.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace InterestOrganiser.Droid
{
    public class CustomEntryRenderer : EntryRenderer
    {
        CustomEntry customEntry;

        public CustomEntryRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            customEntry = this.Element as CustomEntry;

            FormsEditText editText = this.Control;

            if (!String.IsNullOrEmpty(customEntry.Image))
            {
                switch (customEntry.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(customEntry.Image), null, null, null); break;
                    case ImageAlignment.Right:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(customEntry.Image), null); break;
                }
            }
            editText.CompoundDrawablePadding = 25;

            GradientDrawable gradientDrawable = new GradientDrawable();
            //Zaokraglenie
            gradientDrawable.SetCornerRadius(Context.ToPixels(customEntry.CornerRadius));
            //Obramowka
            gradientDrawable.SetStroke((int)Context.ToPixels(customEntry.BorderThickness), customEntry.BorderColor.ToAndroid());
            //Tlo elementu
            gradientDrawable.SetColor(customEntry.BackgroundColor.ToAndroid());

            Control.SetBackground(gradientDrawable);

            int paddingLeft = (int)Context.ToPixels(customEntry.Padding.Left);
            int paddingTop = (int)Context.ToPixels(customEntry.Padding.Top);
            int paddingRight = (int)Context.ToPixels(customEntry.Padding.Right);
            int paddingBottom = (int)Context.ToPixels(customEntry.Padding.Bottom);

            Control.SetPadding(paddingLeft, paddingTop, paddingRight, paddingBottom);


            IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));

            IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");
            JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.cursor);

        }

        private BitmapDrawable GetDrawable(string image)
        {
            int resourceID = Resources.GetIdentifier(image, "drawable", this.Context.PackageName);
            Drawable drawable = ContextCompat.GetDrawable(this.Context, resourceID);
            Bitmap bitmap = (drawable as BitmapDrawable).Bitmap;

            return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, customEntry.ImageWidth, customEntry.ImageHeight, true));
        }
    }
}