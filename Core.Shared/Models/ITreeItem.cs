using System.Collections.Generic;

namespace Core.Models
{
    /// <summary>
    /// Интерфейс древовидной сущности
    /// </summary>
    public interface ITreeItem
    {
        /// <summary>
        /// Родитель
        /// </summary>
        ITreeItem Parent { get; }

        /// <summary>
        /// Дети
        /// </summary>
        IReadOnlyList<ITreeItem> Child { get; }

        /// <summary>
        /// Добавить вложенный элемент
        /// </summary>
        /// <param name="item">Новый вложенный элемент</param>
        void Add(ITreeItem item);
    }

    public interface ITreeItem<T> : ITreeItem
        where T : ITreeItem
    {
        /// <summary>
        /// Родитель
        /// </summary>
        new T Parent { get; }
        /// <summary>
        /// Дети
        /// </summary>
        new IReadOnlyList<T> Child { get; }

        /// <summary>
        /// Добавить вложенный элемент
        /// </summary>
        /// <param name="item">Новый вложенный элемент</param>
        void Add(T item);
    }
}
