//using System;
//using System.Collections;
//using System.Collections.Generic;

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public class ControlCollection : IEnumerable<BaseControl>
//    {
//        List<BaseControl> collection;
//        BaseControl owner;


//        public ControlCollection(BaseControl owner)
//        {
//            collection = new List<BaseControl>(4);
//            this.owner = owner;
//        }

//        public BaseControl this[int index]
//        {
//            get { return collection[index]; }
//            set { collection[index] = value; }
//        }

//        public int Count
//        {
//            get { return collection.Count; }
//        }

//        public BaseControl Owner
//        {
//            get { return owner; }
//            set { owner = value; }
//        }

//        /// <summary>
//        /// Adds a new control to the collection. It will thrown an ArgumentException if the control is 
//        /// already there or if it there is a control with the same ID.
//        /// </summary>
//        /// <param name="ctl">The control to add.</param>
//        public void Add(BaseControl ctl)
//        {
//            if (collection.Contains(ctl))
//                throw new ArgumentException("The collection already contains control: " + ctl.ID);
//            else if (IndexOf(ctl.ID) != -1)
//                throw new ArgumentException("The collection already contains a control by the same ID: " + ctl.ID);

//            collection.Add(ctl);
//        }


//        /// <summary>
//        /// Returns the index of the first level child control (not recursive) whose ID is 
//        /// the one specified.
//        /// </summary>
//        /// <param name="id">The ID of the control to find.</param>
//        /// <returns>The 0 based index if found, -1 if not.</returns>
//        public int IndexOf(string id)
//        {
//            for (int i = 0; i < collection.Count; i++)
//            {
//                BaseControl ctl = this[i];
//                if (ctl.ID == id)
//                    return i;
//            }

//            return -1;
//        }

//        /// <summary>
//        /// Returns the index of the first level child control (not recursive) passed
//        /// as argument.
//        /// </summary>
//        /// <param name="ctl">The control to find.</param>
//        /// <returns>The 0 based index if found, -1 if not.</returns>
//        public int IndexOf(BaseControl ctl)
//        {
//            return collection.IndexOf(ctl);
//        }

//        /// <summary>
//        /// Checks whether the control is contained in the collection.
//        /// </summary>
//        /// <param name="ctl">The control to check.</param>
//        /// <returns>True if it is in the collection, false otherwise.</returns>
//        public bool Contains(BaseControl ctl)
//        {
//            return collection.Contains(ctl);
//        }


//        /// <summary>
//        /// Removes the control from the collection.
//        /// </summary>       
//        /// <param name="ctl">The control to remove.</param>
//        /// <returns>True if it was removed, false otherwise.</returns>
//        public bool Remove(BaseControl ctl)
//        {
//            return collection.Remove(ctl);
//        }

//        /// <summary>
//        /// Sort the control collection from the foremost to the ones in the
//        /// background.
//        /// </summary>
//        public void Sort()
//        {
//            collection.Sort(DepthSort);
//        }

//        int DepthSort(BaseControl ctl1, BaseControl ctl2)
//        {
//            return -(ctl1.Depth.CompareTo(ctl2.Depth));
//        }

//        #region IEnumerable<BaseControl> Members

//        public IEnumerator<BaseControl> GetEnumerator()
//        {
//            return collection.GetEnumerator();
//        }

//        #endregion

//        #region IEnumerable Members

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }

//        #endregion
//    }
//}