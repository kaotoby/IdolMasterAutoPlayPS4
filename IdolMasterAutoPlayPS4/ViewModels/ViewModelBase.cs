using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IdolMasterAutoPlayPS4.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property) {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public void SetValue<T>(ref T obj, T vlaue, [CallerMemberName] string name = null) {
            obj = vlaue;
            OnPropertyChanged(name);
        }
    }
}
