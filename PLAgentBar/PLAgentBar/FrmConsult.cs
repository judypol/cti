using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class FrmConsult : Form
	{
		private IContainer components = null;

		private Button btnTransfer;

		private ListView lvwAgent;

		public TextBox txtAgentID;

		private ImageList imgLstMonitor;

		private Button btnCancel;

		public FrmConsult()
		{
			this.InitializeComponent();
		}

		public void OnMonitorEvent(string EventType, string agentID, int retCode, string strReason, List<string[]> agentData)
		{
			if (0 != retCode)
			{
				string text = EventType.ToLower();
				if (text != null)
				{
					if (!(text == "agentlist"))
					{
						if (!(text == "queuelist"))
						{
						}
					}
					else
					{
						this.UpdateAgentMonitor(agentData);
					}
				}
			}
		}

		private void FrmConsult_Load(object sender, EventArgs e)
		{
			this.initLvwAgent();
		}

		private void initLvwAgent()
		{
			this.lvwAgent.Columns.Add("坐席号", 80, HorizontalAlignment.Left);
			this.lvwAgent.Columns.Add("坐席姓名", 80, HorizontalAlignment.Left);
			this.lvwAgent.Columns.Add("状态", 80, HorizontalAlignment.Left);
		}

		private int FindAgentID(string strAgentID, ListView lvw)
		{
			ListViewItem foundItem = lvw.FindItemWithText(strAgentID);
			int result;
			if (foundItem != null)
			{
				result = foundItem.Index;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		private void UpdateAgentMonitor(List<string[]> newMonitorData)
		{
			this.lvwAgent.BeginUpdate();
			for (int i = 0; i < newMonitorData.Count; i++)
			{
				int foundItemIndex = this.FindAgentID(newMonitorData[i].ElementAt(0), this.lvwAgent);
				if (foundItemIndex == -1)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Text = newMonitorData[i].ElementAt(0).ToString();
					lvi.SubItems.Add(newMonitorData[i].ElementAt(1).ToString());
					lvi.SubItems.Add(newMonitorData[i].ElementAt(2).ToString());
					string text = newMonitorData[i].ElementAt(2).ToLower();
					if (text != null)
					{
						if (!(text == "talk"))
						{
							if (!(text == "idle"))
							{
								if (!(text == "hold"))
								{
									if (!(text == "ring"))
									{
										if (text == "offline")
										{
											lvi.ImageKey = "offline";
										}
									}
									else
									{
										lvi.ImageKey = "ring";
									}
								}
								else
								{
									lvi.ImageKey = "hold";
								}
							}
							else
							{
								lvi.ImageKey = "idle";
							}
						}
						else
						{
							lvi.ImageKey = "talk";
						}
					}
					this.lvwAgent.Items.Add(lvi);
				}
				else
				{
					for (int j = 0; j < 2; j++)
					{
						if (this.lvwAgent.Items[foundItemIndex].SubItems[j + 1].Text != newMonitorData[i].ElementAt(j + 1).ToString())
						{
							this.lvwAgent.Items[foundItemIndex].SubItems[j + 1].Text = newMonitorData[i].ElementAt(j + 1).ToString();
							if (j == 1)
							{
								string text = newMonitorData[i].ElementAt(2).ToLower();
								if (text != null)
								{
									if (!(text == "talk"))
									{
										if (!(text == "idle"))
										{
											if (!(text == "hold"))
											{
												if (!(text == "ring"))
												{
													if (text == "offline")
													{
														this.lvwAgent.Items[foundItemIndex].ImageKey = "offline";
													}
												}
												else
												{
													this.lvwAgent.Items[foundItemIndex].ImageKey = "ring";
												}
											}
											else
											{
												this.lvwAgent.Items[foundItemIndex].ImageKey = "hold";
											}
										}
										else
										{
											this.lvwAgent.Items[foundItemIndex].ImageKey = "idle";
										}
									}
									else
									{
										this.lvwAgent.Items[foundItemIndex].ImageKey = "talk";
									}
								}
							}
						}
					}
				}
			}
			this.lvwAgent.EndUpdate();
		}

		private void lvwAgent_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lvwAgent.SelectedItems.Count > 0)
			{
				this.txtAgentID.Text = this.lvwAgent.SelectedItems[0].Text;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmConsult));
			this.txtAgentID = new TextBox();
			this.btnTransfer = new Button();
			this.lvwAgent = new ListView();
			this.imgLstMonitor = new ImageList(this.components);
			this.btnCancel = new Button();
			base.SuspendLayout();
			this.txtAgentID.Location = new Point(14, 19);
			this.txtAgentID.Name = "txtAgentID";
			this.txtAgentID.Size = new Size(202, 21);
			this.txtAgentID.TabIndex = 5;
			this.btnTransfer.DialogResult = DialogResult.OK;
			this.btnTransfer.Location = new Point(229, 18);
			this.btnTransfer.Name = "btnTransfer";
			this.btnTransfer.Size = new Size(76, 24);
			this.btnTransfer.TabIndex = 4;
			this.btnTransfer.Text = "确定";
			this.btnTransfer.UseVisualStyleBackColor = true;
			this.lvwAgent.FullRowSelect = true;
			this.lvwAgent.GridLines = true;
			this.lvwAgent.LargeImageList = this.imgLstMonitor;
			this.lvwAgent.Location = new Point(14, 53);
			this.lvwAgent.Name = "lvwAgent";
			this.lvwAgent.Size = new Size(291, 162);
			this.lvwAgent.SmallImageList = this.imgLstMonitor;
			this.lvwAgent.TabIndex = 3;
			this.lvwAgent.UseCompatibleStateImageBehavior = false;
			this.lvwAgent.View = View.Details;
			this.lvwAgent.SelectedIndexChanged += new EventHandler(this.lvwAgent_SelectedIndexChanged);
			this.imgLstMonitor.ImageStream = (ImageListStreamer)resources.GetObject("imgLstMonitor.ImageStream");
			this.imgLstMonitor.TransparentColor = Color.Transparent;
			this.imgLstMonitor.Images.SetKeyName(0, "hold");
			this.imgLstMonitor.Images.SetKeyName(1, "talk");
			this.imgLstMonitor.Images.SetKeyName(2, "ring");
			this.imgLstMonitor.Images.SetKeyName(3, "offline");
			this.btnCancel.DialogResult = DialogResult.Cancel;
			this.btnCancel.Location = new Point(400, 103);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(75, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
			base.AcceptButton = this.btnTransfer;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new Size(315, 226);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.txtAgentID);
			base.Controls.Add(this.btnTransfer);
			base.Controls.Add(this.lvwAgent);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.Name = "FrmConsult";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "咨询";
			base.Load += new EventHandler(this.FrmConsult_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
