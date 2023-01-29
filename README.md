# FilterableObservableCollection
Implements an observable collection with a filter predicate, where
changes in the filter, added or removed items are observable.
Meant to be used as a collection in WPF or Maui UI lists with search or filter capabilities.
Items in the collection are stored as they are, ObservableCollection.Items is
maintained with items matching the filter.

Example

    public class MyViewModel : BaseViewModel
    {
        private string _searchString;
        public string SearchString
        {
            get => _searchString;
            set
            {
                SetProperty(ref _searchString, value);
                ItemVMs.Filter = (item) => item.StartsWith(_searchString);
            }
        }

        public FilterableObservableCollection<string> MyStrings { get; }

        public MyViewModel()
        {
            _searchString = "";
            ItemVMs = new FilterableObservableCollection<ItemViewModel>();
            ItemVMs.Add("One");
            ItemVMs.Add("Two");
            ItemVMs.Add("Three");
            ItemVMs.Add("Four");
            ItemVMs.Add("Five");
        }
    }

To see it working, wire up SearchString to an Entry, MyStrings to a CollectionView.