namespace WTLib.Utils
{
    using WTLib.Mvvm;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Data;

    public abstract class CollectionViewModelBase<T> : ObservableObject
        where T : class
    {
        protected CollectionViewModelBase()
        {
            this.Reset();
        }

        protected ObservableCollection<T> InnerItems { get; private set; }

        protected void Reset(ObservableCollection<T> items = null)
        {
            this.InnerItems = items == null ? new ObservableCollection<T>() : items;
            Items = CollectionViewSource.GetDefaultView(InnerItems);
            var live = this.Items as ListCollectionView;
            live.IsLiveSorting = true;
            live.IsLiveFiltering = true;
        }

        public ICollectionView Items { get; private set; }
    }
}
