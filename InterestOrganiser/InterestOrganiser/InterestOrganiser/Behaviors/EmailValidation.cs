using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace InterestOrganiser.Behaviors
{
    public class EmailValidation : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            string email = e.NewTextValue;
            Regex emailPattern = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            if (emailPattern.IsMatch(email)){
                entry.TextColor = Color.White;
            }
            else
            {
                entry.TextColor = Color.FromHex("cc0000");
            }
        }
    }
}
