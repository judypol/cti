using System;
using System.Collections;
using System.Windows.Forms;

namespace PLAgentBar
{
	internal class ListViewColumnSorter_AgentState : IComparer
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

		public ListViewColumnSorter_AgentState()
		{
			this.ColumnToSort = 0;
			this.OrderOfSort = SortOrder.Ascending;
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
			int compareResult = this.ObjectCompare.Compare(listviewX.SubItems[this.ColumnToSort].Text, listviewY.SubItems[this.ColumnToSort].Text);
			int result;
			if (compareResult == 0)
			{
				int compareResult2 = this.ObjectCompare.Compare(Convert.ToUInt64(listviewX.SubItems[3].Tag), Convert.ToUInt64(listviewY.SubItems[3].Tag));
				if (this.OrderOfSort == SortOrder.Ascending)
				{
					result = compareResult2;
				}
				else if (this.OrderOfSort == SortOrder.Descending)
				{
					result = -compareResult2;
				}
				else
				{
					result = compareResult2;
				}
			}
			else if (listviewX.SubItems[this.ColumnToSort].Text == "通话")
			{
				result = -1;
			}
			else if (listviewY.SubItems[this.ColumnToSort].Text == "通话")
			{
				result = 1;
			}
			else if (listviewX.SubItems[this.ColumnToSort].Text == "后处理")
			{
				if (listviewY.SubItems[this.ColumnToSort].Text == "通话")
				{
					result = 1;
				}
				else
				{
					result = -1;
				}
			}
			else if (listviewY.SubItems[this.ColumnToSort].Text == "后处理")
			{
				if (listviewX.SubItems[this.ColumnToSort].Text == "通话")
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
			}
			else if (listviewX.SubItems[this.ColumnToSort].Text == "离线" && listviewY.SubItems[this.ColumnToSort].Text != "离线")
			{
				result = 1;
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
