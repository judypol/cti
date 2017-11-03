using System;
using System.Collections;
using System.Windows.Forms;

namespace PLAgentBar
{
	internal class ListViewColumnTimeSorter : IComparer
	{
		private int ColumnToSort;

		private SortOrder OrderOfSort;

		private CaseInsensitiveComparer ObjectCompare;

		public int SortColumn
		{
			get
			{
				return this.ColumnToSort;
			}
			set
			{
				this.ColumnToSort = value;
			}
		}

		public SortOrder Order
		{
			get
			{
				return this.OrderOfSort;
			}
			set
			{
				this.OrderOfSort = value;
			}
		}

		public ListViewColumnTimeSorter()
		{
			this.ColumnToSort = 3;
			this.OrderOfSort = SortOrder.Ascending;
			this.ObjectCompare = new CaseInsensitiveComparer();
		}

		public int Compare(object x, object y)
		{
			ListViewItem listviewX = (ListViewItem)x;
			ListViewItem listviewY = (ListViewItem)y;
			if (listviewX.SubItems.Count <= this.ColumnToSort || listviewY.SubItems.Count <= this.ColumnToSort)
			{
				this.ColumnToSort = 3;
			}
			int compareResult = this.ObjectCompare.Compare(Convert.ToUInt64(listviewX.SubItems[this.ColumnToSort].Tag), Convert.ToUInt64(listviewY.SubItems[this.ColumnToSort].Tag));
			int result;
			if (this.OrderOfSort == SortOrder.Ascending)
			{
				result = compareResult;
			}
			else if (this.OrderOfSort == SortOrder.Descending)
			{
				result = -compareResult;
			}
			else
			{
				result = 0;
			}
			return result;
		}
	}
}
