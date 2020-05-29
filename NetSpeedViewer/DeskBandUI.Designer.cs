using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace NetSpeedViewer
{
    partial class DeskBandUI
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private TableLayoutPanel tableLayoutPanel2;
        private Label uploadLabel;
        private Label downloadLabel;
        private Label uploadValLabel;
        private Label downloadValLabel;
        

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }



        #region Component Designer generated code

        private void InitializeComponent(string t = "test1")
        {
            tableLayoutPanel2 = new TableLayoutPanel();
            uploadLabel = new Label();
            downloadLabel = new Label();
            uploadValLabel = new Label();
            downloadValLabel = new Label();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            //tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel2.Controls.Add(uploadLabel, 0, 0);
            tableLayoutPanel2.Controls.Add(downloadLabel, 0, 1);
            tableLayoutPanel2.Controls.Add(uploadValLabel, 1, 0);
            tableLayoutPanel2.Controls.Add(downloadValLabel, 1, 1);
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.TabIndex = 0;
            // 
            // uploadLabel
            // 
            uploadLabel.AutoSize = true;
            uploadLabel.ForeColor = SystemColors.ControlLightLight;
            uploadLabel.Name = "uploadLabel";
            uploadLabel.TabIndex = 0;
            uploadLabel.Text = "U:";
            uploadLabel.BackColor = Color.Black;

            // 
            // downloadLabel
            // 
            downloadLabel.AutoSize = true;
            downloadLabel.ForeColor = SystemColors.ControlLightLight;
            downloadLabel.Name = "downloadLabel";
            downloadLabel.TabIndex = 1;
            downloadLabel.Text = "D:";
            downloadLabel.BackColor = Color.Black;
            // 
            // uploadValLabel
            // 
            uploadValLabel.AutoSize = false;
            uploadValLabel.Dock = DockStyle.Right;
            uploadValLabel.Size = new Size(140, 25);
            uploadValLabel.ForeColor = SystemColors.ControlLightLight;
            uploadValLabel.Name = "uploadValLabel";
            uploadValLabel.TabIndex = 2;
            uploadValLabel.Text = $"--- {formatText}/s";
            uploadValLabel.TextAlign = ContentAlignment.MiddleRight;
            uploadLabel.BackColor = Color.Black;
            // 
            // downloadValLabel
            // 
            downloadValLabel.AutoSize = false;
            downloadValLabel.Dock = DockStyle.Right;
            downloadValLabel.ForeColor = SystemColors.ControlLightLight;
            downloadValLabel.Name = "downloadValLabel";
            downloadValLabel.TabIndex = 3;
            downloadValLabel.Text = $"--- {formatText}/s";
            downloadValLabel.TextAlign = ContentAlignment.MiddleRight;
            downloadValLabel.BackColor = Color.Black;
            // 
            // HelloWorldBar
            // 
            Controls.Add(tableLayoutPanel2);
            Size = new Size(180, 50);
            Name = "NetSpeedViewerBar";
            tableLayoutPanel2.BackColor = Color.Black;
            BackColor = Color.Transparent;

            AddContextMenuAndItems();


            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);

        }


        #endregion
    }
}
