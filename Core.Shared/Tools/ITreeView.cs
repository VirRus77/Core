using System.Collections.Generic;
using System.Linq;

namespace Core.Tools
{
    /// <summary>
    /// Интерфейс для поля выделения
    /// </summary>
    public interface ITreeItemSelected
    {
        /// <summary>
        /// Вылелен ли элемент
        /// </summary>
        bool Selected { get; set; }
    }

    /// <summary>
    /// Интерфейс для поля раскрытия
    /// </summary>
    public interface ITreeItemExpanded
    {
        /// <summary>
        /// Раскрыт ли элемент
        /// </summary>
        bool Expanded { get; set; }
    }

    /// <summary>
    /// Интерфейс для древовидных представлений
    /// </summary>
    public interface ITreeItem
    {
        /// <summary>
        /// Список дочерних элементов
        /// </summary>
        IList<ITreeItem> Children { get; }
    }

    /// <summary>
    /// Интерфейс древовидного элемента со значением
    /// </summary>
    public interface ITreeItemValue
    {
        /// <summary>
        /// Хранимое значение
        /// </summary>
        object Value { get; }
    }

    /// <summary>
    /// Интерфейс древовидного элемента со значением типа <see cref="T"/>
    /// </summary>
    public interface ITreeItemValue<T> : ITreeItemValue
    {
        /// <summary>
        /// Хранимое значение
        /// </summary>
        new T Value { get; }
    }

    /// <summary>
    /// Интерфейс для древовидных представлений со значением
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITreeItem<T> : ITreeItem, ITreeItemValue<T>
    {
        /// <summary>
        /// Список дочерних элементов
        /// </summary>
        new IList<ITreeItem<T>> Children { get; }
    }

    /// <summary>
    /// Класс реализации древовидных представлений со значением
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeItem<T> : ITreeItem<T>
    {
        public TreeItem(T value)
        {
            Value = value;
            Children = new List<ITreeItem<T>>();
        }
        public T Value { get; }
        public IList<ITreeItem<T>> Children { get; }

        IList<ITreeItem> ITreeItem.Children
        {
            get { return Children.OfType<ITreeItem>().ToList(); }
        }
        object ITreeItemValue.Value => Value;
    }

    /// <summary>
    /// Класс реализации древовидных представлений со значением
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeItemExpanded<T> : TreeItem<T>, ITreeItemExpanded
    {
        public TreeItemExpanded(T value, bool expanded = false)
        : base(value)
        {
            Expanded = expanded;
        }

        public bool Expanded { get; set; }
    }
}
