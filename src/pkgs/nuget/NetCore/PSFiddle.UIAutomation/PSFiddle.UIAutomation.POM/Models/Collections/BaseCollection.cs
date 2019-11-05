using PSFiddle.UIAutomation.POM.Models.EventArguments;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PSFiddle.UIAutomation.POM.Models.Collections
{
    public class BaseCollection<T> : BaseCollection<T, CollectionItemAddingEventArg<T>, CollectionItemAddedEventArg<T>>
    {
        public BaseCollection(POMParsingContext context) : base(context)
        {
        }
    }
    public class TreeNodes<T1, T2> where T1: TreeNodes<T1, T2>
                                   where T2: BaseCollectionOfTreeNodes<T1, T2>
    {

        protected T2 children;

        public TreeNodes() 
        {
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public T2 Children() => children;

        /// <summary>
        /// Childrens the deep using bredth traversal
        /// </summary>
        /// <returns></returns>
        public List<T1> ChildrenDeep(Predicate<T1> predicate = null, Predicate<T1> continueWithTree = null)
        {
            predicate = predicate ?? ((b) => true);
            continueWithTree = continueWithTree ?? ((b)=>true);

            var items = new List<T1>();
            foreach (var item in Children())
            {
                if (predicate(item))
                    items.Add(item);
            }

            if (!continueWithTree((T1)this))
                return items;

            foreach (var item in Children())
            {
                items.AddRange(item.ChildrenDeep(predicate, continueWithTree));
            }
            return items;
        }
        /// <summary>
        /// Childrens the and self deep.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public List<T1> ChildrenAndSelfDeep(Predicate<T1> predicate = null, Predicate<T1> continueWithTree = null)
        {
            predicate = predicate ?? ((b) => true);
            continueWithTree = continueWithTree ?? ((b) => true);

            var items = new List<T1>();

            if (predicate((T1)this))
                items.Add((T1)this);

            if (!continueWithTree((T1)this))
                return items;

            foreach (var item in Children())
            {
                items.AddRange(item.ChildrenAndSelfDeep(predicate, continueWithTree));
            }
            return items;
        }
    }
    public class BaseCollectionOfTreeNodes<T1, T2> : BaseCollection<T1> where T1: TreeNodes<T1, T2>
                                                                        where T2: BaseCollectionOfTreeNodes<T1, T2>
    {
        public BaseCollectionOfTreeNodes(POMParsingContext context) : base(context)
        {
        }

        
    }

    public class BaseCollection<T1,T2,T3> : ICollection<T1>, IEnumerable<T1>, IList<T1> 
                                                where T2: CollectionItemAddingEventArg<T1>, new()
                                                where T3 : CollectionItemAddedEventArg<T1>, new()
    {
        public event EventHandler<T2> ItemAddingEvent;
        public event EventHandler<T3> ItemAddedEvent;
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        protected POMParsingContext Context { get; private set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        protected ObservableCollection<T1> Items { get; } = new ObservableCollection<T1>();

        public int Count => Items.Count;

        public bool IsReadOnly => false;

        public T1 this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private object _lock = new object();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T1> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        public void Add(T1 item)
        {
            lock (_lock)
            {
                Items.Add(item);
            }
            
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                Items.Clear();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if <paramref name="item">item</paramref> is found in the <see cref="System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        public bool Contains(T1 item)
        {
            lock (_lock)
            {
                return Items.Contains(item);
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T1[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if <paramref name="item">item</paramref> was successfully removed from the <see cref="System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if <paramref name="item">item</paramref> is not found in the original <see cref="System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        public bool Remove(T1 item)
        {
            lock (_lock)
            {
                return Items.Remove(item);
            }
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        /// <returns>
        /// The index of <paramref name="item">item</paramref> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(T1 item)
        {
            lock (_lock)
            {
                return Items.IndexOf(item);
            }
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        public void Insert(int index, T1 item)
        {
            lock (_lock)
            {
                Items.Insert(index, item);
            }
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            lock (_lock)
            {
                Items.RemoveAt(index);
            }
        }

        public BaseCollection(POMParsingContext context)
        {
            this.Context = context;

            Items.CollectionChanged += CollectionChangedEvent;
        }
        protected T2 PreAdding(T1 item)
        {
            T2 arg = new T2();
            arg.Item = item;
            ItemAddingEvent?.Invoke(this, arg);
            return arg;
        }
        private void CollectionChangedEvent(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                List<T1> removeThem = new List<T1>();
                foreach (T1 item in e.NewItems)
                {
                    var arg = PreAdding(item);
                    if (arg.Deny)
                        removeThem.Add(item);
                    else
                    {
                        var addedArg = new T3();
                        addedArg.Item = item;
                        ItemAddedEvent?.Invoke(this, addedArg);
                    }
                           
                }
                foreach (T1 item in removeThem)
                {
                    this.Items.Remove(item);
                }
            }
        }




    }
}
