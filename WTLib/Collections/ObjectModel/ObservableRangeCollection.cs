namespace WTLib.Collections.ObjectModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    public class ObservableRangeCollection<T> : ObservableCollection<T>
    {
        public ObservableRangeCollection() : base() { }

        public ObservableRangeCollection(IEnumerable<T> collection) : base(collection) { }

        public ObservableRangeCollection<T> AddRange(IEnumerable<T> collection)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                    Items.Add(item);
                this.NotifyCollectionChanged();
            }
            return this;
        }

        public ObservableRangeCollection<T> RemoveRange(IEnumerable<T> collection)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                    Items.Remove(item);
                this.NotifyCollectionChanged();
            }
            return this;
        }

        public ObservableRangeCollection<T> ReplaceRange(IEnumerable<T> collection)
        {
            Items.Clear();
            if (collection != null)
            {
                foreach (var item in collection)
                    Items.Add(item);
            }
            this.NotifyCollectionChanged();
            return this;
        }

        public void NotifyCollectionChanged()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
