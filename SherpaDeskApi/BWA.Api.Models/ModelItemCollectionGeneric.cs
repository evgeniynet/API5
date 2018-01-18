// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;


namespace BWA.Api.Models
{
   /// <summary>
   /// Summary description for ModelItemCollectionGeneric
   /// </summary>
    [DataContract(Name = "ModelItemCollection")]
    public class ModelItemCollectionGeneric<T> : IList<T> where T : ModelItemBaseInterface
    {
        protected List<T> InnerList;
        protected DataTable ItemsTable;
        protected List<object> DeletedRowPrimaryValues;
        protected int PrimaryColumnIndex;

        internal ModelItemCollectionGeneric(DataTable items)
        {
            ItemsTable = items;
            PrimaryColumnIndex = 0;
            DeletedRowPrimaryValues = new List<object>();
            InnerList = new System.Collections.Generic.List<T>(this);
        }

        public T NewItem()
        {
            return (T)System.Activator.CreateInstance(typeof(T), ItemsTable.NewRow());
        }

        private T NewIem(DataRow row)
        {
            return (T)System.Activator.CreateInstance(typeof(T), row);
        }

        private void AddRowToDeleted(DataRow row)
        {
            if (!row.IsNull(PrimaryColumnIndex)) DeletedRowPrimaryValues.Add(row[PrimaryColumnIndex]);
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            return ItemsTable.Rows.IndexOf(item.Row);
        }

        public void Insert(int index, T item)
        {
            ItemsTable.Rows.InsertAt(item.Row, index);
        }

        public void RemoveAt(int index)
        {
            AddRowToDeleted(ItemsTable.Rows[index]);
            ItemsTable.Rows.RemoveAt(index);
        }

        [DataMember]
        public T this[int index]
        {
            get
            {
                return NewIem(ItemsTable.DefaultView[index].Row);
            }
            set
            {
                AddRowToDeleted(ItemsTable.Rows[index]);
                ItemsTable.Rows.RemoveAt(index);
                ItemsTable.Rows.InsertAt(value.Row, index);
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            ItemsTable.Rows.Add(item.Row);
        }

        public void Clear()
        {
            foreach (DataRow _row in ItemsTable.Rows) AddRowToDeleted(_row);
            ItemsTable.Rows.Clear();
        }

        public bool Contains(T item)
        {
            return ItemsTable.Rows.Contains(item.Id);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < array.Length; i++) array[i] = this[i];
        }

        public bool IsReadOnly
        {
            get { return ItemsTable.Rows.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            AddRowToDeleted(item.Row);
            ItemsTable.Rows.Remove(item.Row);
            return true;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            foreach (DataRow _row in ItemsTable.Rows) yield return NewIem(_row);
        }

        #endregion

        #region ICollection<T> Members


        public int Count
        {
            get { return ItemsTable.DefaultView.Count; }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public List<T> List
        {
            get {return InnerList;}
        }

        public IEnumerable<T> IEnumerable
        {
            get { return InnerList; }
        }
    }
}
