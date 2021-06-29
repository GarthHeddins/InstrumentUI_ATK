using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InstrumentUI_ATK.DataAccess.Model;

namespace InstrumentUI_ATK.Common
{
    public class ScheduleQueue : IEnumerable
    {
        private readonly List<ScheduleQueueItem> _list = new List<ScheduleQueueItem>();
        private int _lastId = 1;

        public event EventHandler<ScheduleQueueEventArgs> ItemEnqueued;
        public event EventHandler<ScheduleQueueEventArgs> ItemDequeued;


        /// <summary>
        /// Event raised when a ScheduleQueueItems is added to the ScheduleQueue
        /// </summary>
        protected virtual void OnItemEnqueued(ScheduleQueueItem item)
        {
            var args = new ScheduleQueueEventArgs {Item = item};
            if (ItemEnqueued != null) ItemEnqueued(this, args);
        }


        /// <summary>
        /// Event raised when a ScheduleQueueItems is removed from the ScheduleQueue
        /// </summary>
        protected virtual void OnItemDequeued(ScheduleQueueItem item)
        {
            var args = new ScheduleQueueEventArgs { Item = item };
            if (ItemDequeued != null) ItemDequeued(this, args);
        }


        /// <summary>
        /// Number of ScheduleQueueItems in the ScheduleQueue
        /// </summary>
        public int Count
        {
            get { return _list.Count; }
        }


        /// <summary>
        /// Removes all ScheduleQueueItems from the ScheduleQueue
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }


        /// <summary>
        /// Add a ScheduleQueueItem to the end of the ScheduleQueue
        /// </summary>
        /// <param name="item">The ScheduleQueueItem to add to the ScheduleQueue</param>
        public virtual void Enqueue(ScheduleQueueItem item)
        {
            item.Id = _lastId++;
            _list.Insert(_list.Count, item);
            OnItemEnqueued(item);
        }


        /// <summary>
        /// Removes and returns the ScheduleQueueItem at the beginning of the ScheduleQueue
        /// </summary>
        /// <returns>The ScheduleQueueItem at the beginning of the ScheduleQueue</returns>
        public ScheduleQueueItem Dequeue()
        {
            if (_list.Count == 0)
                return null;

            var item = _list[0];
            _list.Remove(item);
            OnItemDequeued(item);
            return item;
        }


        /// <summary>
        /// Removes the ScheduleQueueItem with the specified Sequence from the ScheduleQueue
        /// </summary>
        /// <param name="sequence">Sequence</param>
        public bool RemoveBySequence(int sequence)
        {
            var item = GetItemBySequence(sequence);
            
            if (item == null)
                return false;

            _list.Remove(item);
            return true;
        }


        /// <summary>
        /// Gets the ScheduleQueueItem with the specified Sequence from the ScheduleQueue
        /// </summary>
        /// <param name="sequence">Sequence</param>
        /// <returns>ScheduleQueueItem containing the Item</returns>
        public ScheduleQueueItem GetItemBySequence(int sequence)
        {
            return _list.FirstOrDefault(qi => qi.Id == sequence);
        }


        /// <summary>
        /// Returns an enumerator that iterates through the ScheduleQueue
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }


    /// <summary>
    /// Event Arguments sent to ScheduleQueue event handlers
    /// </summary>
    public class ScheduleQueueEventArgs : EventArgs
    {
        public ScheduleQueueItem Item { get; set; }
    }
}
