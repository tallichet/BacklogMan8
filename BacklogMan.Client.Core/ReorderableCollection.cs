using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BacklogMan.Client.Core
{
    public class ReorderableCollection<T> : ObservableCollection<T>  
    {
        public ReorderableCollection() : base()
        {
            base.CollectionChanged += ReorderableCollection_CollectionChanged;
        }

        T lastCollectionChangedObject = default(T);

        void ReorderableCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                lastCollectionChangedObject = (T)e.OldItems[0];
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add &&
                     e.NewItems != null && 
                     e.NewItems[0].Equals(lastCollectionChangedObject))
            {
                if (ManualReordered != null)
                {
                    ManualReordered(this, e);
                }
            }
        }

        public event EventHandler<System.Collections.Specialized.NotifyCollectionChangedEventArgs> ManualReordered;

    }
}
