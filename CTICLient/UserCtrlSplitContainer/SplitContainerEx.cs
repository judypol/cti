using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace UserCtrlSplitContainer
{
	[ToolboxBitmap(typeof(SplitContainer))]
	public class SplitContainerEx : SplitContainer
	{
		private enum MouseState
		{
			Normal,
			Hover
		}

		public enum SplitterPanelEnum
		{
			Panel1,
			Panel2
		}

		private SplitContainerEx.SplitterPanelEnum mCollpasePanel = SplitContainerEx.SplitterPanelEnum.Panel2;

		private bool mCollpased = false;

		private Rectangle mRect = default(Rectangle);

		private SplitContainerEx.MouseState mMouseState = SplitContainerEx.MouseState.Normal;

		private bool mIsSplitterFixed = true;

		private int _HeightOrWidth;

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new int SplitterWidth
		{
			get
			{
				return base.SplitterWidth;
			}
			set
			{
				base.SplitterWidth = 9;
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new int Panel1MinSize
		{
			get
			{
				return base.Panel1MinSize;
			}
			set
			{
				base.Panel1MinSize = value;
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new int Panel2MinSize
		{
			get
			{
				return base.Panel2MinSize;
			}
			set
			{
				base.Panel2MinSize = value;
			}
		}

		[DefaultValue(SplitContainerEx.SplitterPanelEnum.Panel1)]
		public SplitContainerEx.SplitterPanelEnum CollpasePanel
		{
			get
			{
				return this.mCollpasePanel;
			}
			set
			{
				if (value != this.mCollpasePanel)
				{
					this.mCollpasePanel = value;
					base.Invalidate(this.ControlRect);
				}
			}
		}

		public bool IsCollpased
		{
			get
			{
				return this.mCollpased;
			}
		}

		private Rectangle ControlRect
		{
			get
			{
				if (base.Orientation == Orientation.Horizontal)
				{
					this.mRect.X = ((base.Width <= 80) ? 0 : (base.Width / 2 - 40));
					this.mRect.Y = base.SplitterDistance;
					this.mRect.Width = 80;
					this.mRect.Height = 9;
				}
				else
				{
					this.mRect.X = base.SplitterDistance;
					this.mRect.Y = ((base.Height <= 80) ? 0 : (base.Height / 2 - 40));
					this.mRect.Width = 9;
					this.mRect.Height = 80;
				}
				return this.mRect;
			}
		}

		public new bool IsSplitterFixed
		{
			get
			{
				return base.IsSplitterFixed;
			}
			set
			{
				base.IsSplitterFixed = value;
				if (value && !this.mIsSplitterFixed)
				{
					this.mIsSplitterFixed = true;
				}
			}
		}

		public SplitContainerEx()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.SplitterWidth = 9;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.Panel1.Focus();
			base.OnMouseUp(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			this.Cursor = Cursors.Default;
			this.mMouseState = SplitContainerEx.MouseState.Normal;
			base.Invalidate(this.ControlRect);
			base.OnMouseLeave(e);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (this.ControlRect.Contains(e.Location))
			{
				this.CollpaseOrExpand();
			}
			base.OnMouseClick(e);
		}

		public void CollpaseOrExpand()
		{
			if (this.mCollpased)
			{
				this.mCollpased = false;
				base.SplitterDistance = this._HeightOrWidth;
			}
			else
			{
				this.mCollpased = true;
				this._HeightOrWidth = base.SplitterDistance;
				if (this.CollpasePanel == SplitContainerEx.SplitterPanelEnum.Panel2)
				{
					base.SplitterDistance = 0;
				}
				else if (base.Orientation == Orientation.Horizontal)
				{
					base.SplitterDistance = base.Height - 9;
				}
				else
				{
					base.SplitterDistance = base.Width - 9;
				}
			}
			base.Invalidate(this.ControlRect);
		}

		private Bitmap CreateControlImage(bool collapse, Color color)
		{
			Bitmap bmp = new Bitmap(80, 9);
			for (int x = 5; x <= 30; x += 5)
			{
				for (int y = 1; y <= 7; y += 3)
				{
					bmp.SetPixel(x, y, color);
				}
			}
			for (int x = 50; x <= 75; x += 5)
			{
				for (int y = 1; y <= 7; y += 3)
				{
					bmp.SetPixel(x, y, color);
				}
			}
			if (collapse)
			{
				int i = 0;
				for (int y = 1; y <= 7; y++)
				{
					for (int x = 35 + i; x <= 45 - i; x++)
					{
						bmp.SetPixel(x, y, color);
					}
					i++;
				}
			}
			else
			{
				int i = 0;
				for (int y = 7; y >= 1; y--)
				{
					for (int x = 35 + i; x <= 45 - i; x++)
					{
						bmp.SetPixel(x, y, color);
					}
					i++;
				}
			}
			return bmp;
		}
	}
}
