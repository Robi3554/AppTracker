using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace AppTracker
{
    public class AppUsage : INotifyPropertyChanged
    {
        private string name;
        private BitmapSource icon;
        private TimeSpan usageTime;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public BitmapSource Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        public TimeSpan UsageTime
        {
            get { return usageTime; }
            set
            {
                usageTime = value;
                OnPropertyChanged(nameof(UsageTime));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
