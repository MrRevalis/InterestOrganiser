using InterestOrganiser.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InterestOrganiser.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        //public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        public IFirebase FirebaseAuth => DependencyService.Get<IFirebase>();
        public IDBFirebase FirebaseDB => DependencyService.Get<IDBFirebase>();
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Connectivity
        private bool isNotConnected;
        public bool IsNotConnected
        {
            get => isNotConnected;
            set => SetProperty(ref isNotConnected, value);
        }

        public BaseViewModel()
        {
            Connectivity.ConnectivityChanged += ConnectionChanged;
            IsNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
        }

        ~BaseViewModel()
        {
            Connectivity.ConnectivityChanged -= ConnectionChanged;
        }

        private void ConnectionChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsNotConnected = e.NetworkAccess != NetworkAccess.Internet;
        }

        #endregion
    }
}
