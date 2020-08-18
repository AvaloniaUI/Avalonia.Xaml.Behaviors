using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BehaviorsTestApplication.ViewModels.Core
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
#pragma warning disable CS8618
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS8618

        public void Notify([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Update<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (!Equals(field, value))
            {
                field = value;
                Notify(propertyName);
                return true;
            }
            return false;
        }
    }
}
