using DaLang.Lims.Client.Handler.Controls;

namespace DaLang.Lims.Client.Handler.Camera
{
    partial class FrmCamera
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            cb_device = new Sunny.UI.UIComboBox();
            panel_shot = new TransparentPanel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(0, 95);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(643, 444);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(cb_device);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 35);
            panel1.Name = "panel1";
            panel1.Size = new Size(643, 60);
            panel1.TabIndex = 1;
            // 
            // cb_device
            // 
            cb_device.DataSource = null;
            cb_device.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            cb_device.FillColor = Color.White;
            cb_device.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            cb_device.ItemHoverColor = Color.FromArgb(155, 200, 255);
            cb_device.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            cb_device.Location = new Point(4, 8);
            cb_device.Margin = new Padding(4, 5, 4, 5);
            cb_device.MinimumSize = new Size(63, 0);
            cb_device.Name = "cb_device";
            cb_device.Padding = new Padding(0, 0, 30, 2);
            cb_device.Size = new Size(225, 44);
            cb_device.SymbolSize = 24;
            cb_device.TabIndex = 1;
            cb_device.Text = "uiComboBox1";
            cb_device.TextAlignment = ContentAlignment.MiddleLeft;
            cb_device.Watermark = "";
            // 
            // panel_shot
            // 
            panel_shot.BackColor = Color.Transparent;
            panel_shot.Dock = DockStyle.Fill;
            panel_shot.ForeColor = Color.Transparent;
            panel_shot.Location = new Point(0, 95);
            panel_shot.Name = "panel_shot";
            panel_shot.Size = new Size(643, 444);
            panel_shot.TabIndex = 2;
            // 
            // FrmCamera
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(643, 539);
            Controls.Add(panel_shot);
            Controls.Add(pictureBox1);
            Controls.Add(panel1);
            Name = "FrmCamera";
            Text = "FrmCamera";
            ZoomScaleRect = new Rectangle(22, 22, 643, 539);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Panel panel1;
        private TransparentPanel panel_shot;
        private Sunny.UI.UIComboBox cb_device;
    }
}