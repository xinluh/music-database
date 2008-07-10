using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace CustomForm
{
	/// <summary>
	/// Summary description for AutoCompleteDictionary.
	/// </summary>
	[Serializable]
	public class AutoCompleteEntryCollection : CollectionBase
	{
		public class AutoCompleteEntryCollectionEditor : CollectionEditor
		{
			public AutoCompleteEntryCollectionEditor(Type type) : base(type)
			{
			}

			protected override bool CanSelectMultipleInstances()
			{
				return false;
			}

			protected override Type CreateCollectionItemType()
			{
				return typeof(AutoCompleteEntry);
			}
		}

		public IAutoCompleteEntry this[int index]
		{
			get
			{
				return this.InnerList[index] as AutoCompleteEntry;
			}
		}

		public void Add(IAutoCompleteEntry entry)
		{
			this.InnerList.Add(entry);
		}

		public void AddRange(ICollection col)
		{
			this.InnerList.AddRange(col);
		}

        public void AddRange(IEnumerable<IList> items, int MatchMember, int DisplayMember, int ValueMember)
        {
            ArrayList new_items = new ArrayList();
            foreach (IList l in items)
            {
                new_items.Add(new AutoCompleteDataEntry(l, ValueMember, DisplayMember, MatchMember));
            }
            this.InnerList.AddRange(new_items);
        }

		public void Add(AutoCompleteEntry entry)
		{
			this.InnerList.Add(entry);
		}

		public object[] ToObjectArray()
		{
			return this.InnerList.ToArray();
		}
		
	}
}
