using System.ComponentModel;

namespace Core.Interfaces
{
    public interface IViewModel<T>
            where T : class, INotifyPropertyChanged
    {
        T ViewModel { get; set; }
    }
}
