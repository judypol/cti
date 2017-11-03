using System;
using System.Collections;
using System.Windows.Forms;

namespace PLAgentBar
{
	internal class ListViewColumnSorter_Agent_Status : IComparer
	{
		private int ColumnToSort;

		private int ColumnToSort2;

		private SortOrder OrderOfSort;

		private SortOrder OrderOfSort2;

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

		public ListViewColumnSorter_Agent_Status()
		{
			this.ColumnToSort = 0;
			this.ColumnToSort2 = 1;
			this.OrderOfSort = SortOrder.Descending;
			this.OrderOfSort2 = SortOrder.Ascending;
			this.ObjectCompare = new CaseInsensitiveComparer();
		}

		public int Compare(object x, object y)
		{
			ListViewItem listviewX = (ListViewItem)x;
			ListViewItem listviewY = (ListViewItem)y;
			if (listviewX.SubItems.Count <= this.ColumnToSort || listviewY.SubItems.Count <= this.ColumnToSort)
			{
				this.ColumnToSort = 0;
			}
			if (listviewX.SubItems.Count <= this.ColumnToSort2 || listviewY.SubItems.Count <= this.ColumnToSort2)
			{
				this.ColumnToSort2 = 1;
			}
			int compareResult = this.ObjectCompare.Compare(AgentBar.str2int(listviewX.SubItems[this.ColumnToSort].Text), AgentBar.str2int(listviewY.SubItems[this.ColumnToSort].Text));
			int result;
			if (compareResult == 0)
			{
				compareResult = this.ObjectCompare.Compare(listviewX.SubItems[this.ColumnToSort2].Text, listviewY.SubItems[this.ColumnToSort2].Text);
				if (this.OrderOfSort2 == SortOrder.Ascending)
				{
					result = compareResult;
				}
				else if (this.OrderOfSort2 == SortOrder.Descending)
				{
					result = -compareResult;
				}
				else
				{
					result = compareResult;
				}
			}
			else if (this.OrderOfSort == SortOrder.Ascending)
			{
				result = compareResult;
			}
			else if (this.OrderOfSort == SortOrder.Descending)
			{
				result = -compareResult;
			}
			else
			{
				result = compareResult;
			}
			return result;
		}
	}
}
