/*
Copyright (c) 2023, Balint Nagy
All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace YA
{
    /// <summary>
    /// Implements an observable collection with a filter predicate, where
    /// changes in the filter, added or removed items are observable.
    /// Meant to be used as a collection in WPF or Maui UI lists with search or filter capabilities.
    /// Items in the collection are stored as they are, ObservableCollection.Items is
    /// maintained with items matching the filter.
    /// </summary>
    public class FilterableObservableCollection<TItem> : ObservableCollection<TItem>
    {
        /// <summary>
        /// Stores all items in the collection regardless of them being hidden or not by the filter.
        /// </summary>
        public List<TItem> AllItems { get; private set; }

        private Func<TItem, bool> _filter;

        /// <summary>
        /// Function delegate to implement item filtering.
        /// When the filter is changed and that causes a chang in the set of visible items, fires the OnCollectionChanged event.
        /// </summary>
        public Func<TItem, bool> Filter
        {
            get { return _filter; }
            set
            {
                Func<TItem, bool> newFilter = value;
                var itemsToRemove = Items.Where(item => !newFilter(item)).ToList();
                var itemsToAdd = AllItems.Where(item => newFilter(item) && !Items.Contains(item)).ToList();

                foreach (TItem item in itemsToRemove)
                    Items.Remove(item);

                foreach (TItem item in itemsToAdd)
                    Items.Add(item);

                _filter = value;
                if (itemsToRemove.Any() || itemsToAdd.Any())
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// Default ctor.
        /// Crates an empty collection with a non-hiding filter.
        /// </summary>
        public FilterableObservableCollection()
            : base()
        {
            AllItems = new List<TItem>();
            _filter = (item) => true;
        }

        /// <summary>
        /// Ctor.
        /// Creates a collection from an initial set of items and a filter specified by the caller.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="filter"></param>
        public FilterableObservableCollection(IEnumerable<TItem> items, Func<TItem, bool> filter)
            : base()
        {
            AllItems = items.ToList();
            _filter = filter;
        }

        public new void Clear()
        {
            base.Clear();
            AllItems.Clear();
        }

        public new void ClearItems()
        {
            base.ClearItems();
            AllItems.Clear();
        }

        public new void Add(TItem item)
        {
            AllItems.Add(item);
            if (_filter(item))
                base.Add(item);
        }

        public void AddRange(IEnumerable<TItem> newItems)
        {
            foreach (TItem newItem in newItems)
                Add(newItem);
        }

        public new void Remove(TItem item)
        {
            AllItems.Remove(item);
            base.Remove(item);
        }

        public new int IndexOf(TItem item) => throw new NotImplementedException();
        public new void Insert(int index, TItem item) => throw new NotImplementedException();
        public new void InsertItem(int index, TItem item) => throw new NotImplementedException();
        public new void Move(int oldIndex, int newIndex) => throw new NotImplementedException();
        public new void MoveItem(int oldIndex, int newIndex) => throw new NotImplementedException();
        public new void RemoveAt(int index) => throw new NotImplementedException();
        public new void RemoveItem(int index) => throw new NotImplementedException();
        public new void SetItem(int index, TItem item) => throw new NotImplementedException();
    }
}