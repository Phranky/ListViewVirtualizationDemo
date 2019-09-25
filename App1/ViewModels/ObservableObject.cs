using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ItemsRepeaterDemo.ViewModels
{
    public abstract class ObservableObject : INotifyPropertyChanging, INotifyPropertyChanged
    {
        protected ObservableObject()
        {
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanging([CallerMemberName] string propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void RaisePropertyChanged<T>(T oldValue, T newValue, bool broadcast = false, [CallerMemberName] string propertyName = null)
        {
            RaisePropertyChanged(propertyName);

            if (broadcast)
            {
                Broadcast(oldValue, newValue, propertyName);
            }
        }

        protected bool Set<T>(ref T field, T value, bool broadcast = false, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            RaisePropertyChanging(propertyName);

            var oldValue = field;
            var newValue = field = value;

            RaisePropertyChanged(oldValue, newValue, broadcast, propertyName);

            return true;
        }

        protected virtual void Broadcast<T>(T oldValue, T newValue, string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
