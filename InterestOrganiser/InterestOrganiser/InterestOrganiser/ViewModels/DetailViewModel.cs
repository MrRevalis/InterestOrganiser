using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace InterestOrganiser.ViewModels
{
    [QueryProperty("ItemName","name")]
    public class DetailViewModel : BaseViewModel
    {
        private string itemName;
        public string ItemName
        {
            get => itemName;
            set => SetProperty(ref itemName, value);
        }

        public DetailViewModel()
        {

        }
    }
}
