using System.Collections.Generic;

namespace Core.Models
{
    public class TreeItemContainer<T> : ITreeItem<TreeItemContainer<T>>
    {
        protected readonly List<TreeItemContainer<T>> _child;

        public TreeItemContainer(T value)
        {
            Value = value;
            _child = new List<TreeItemContainer<T>>();
        }

        /// <summary>
        /// Значение
        /// </summary>
        public T Value { get; }

        public TreeItemContainer<T> Parent { get; protected set; }

        public IReadOnlyList<TreeItemContainer<T>> Child => _child;

        public virtual void Add(TreeItemContainer<T> item)
        {
            item.Parent = this;
            _child.Add(item);
        }

        #region

        ITreeItem ITreeItem.Parent
        {
            get => Parent;
        }

        IReadOnlyList<ITreeItem> ITreeItem.Child => _child;

        void ITreeItem.Add(ITreeItem item) => Add((TreeItemContainer<T>)item);

        #endregion
    }
}