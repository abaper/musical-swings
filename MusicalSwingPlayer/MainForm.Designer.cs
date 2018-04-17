namespace MusicalSwingPlayer
{
    partial class MainForm
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
            this.midiDeviceList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.playSounds = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // midiDeviceList
            // 
            this.midiDeviceList.FormattingEnabled = true;
            this.midiDeviceList.Location = new System.Drawing.Point(12, 26);
            this.midiDeviceList.Name = "midiDeviceList";
            this.midiDeviceList.Size = new System.Drawing.Size(383, 95);
            this.midiDeviceList.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Device List";
            // 
            // playSounds
            // 
            this.playSounds.Location = new System.Drawing.Point(16, 153);
            this.playSounds.Name = "playSounds";
            this.playSounds.Size = new System.Drawing.Size(75, 23);
            this.playSounds.TabIndex = 2;
            this.playSounds.Text = "PlaySounds";
            this.playSounds.UseVisualStyleBackColor = true;
            this.playSounds.Click += new System.EventHandler(this.playSounds_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.playSounds);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.midiDeviceList);
            this.Name = "MainForm";
            this.Text = "Musical Swing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox midiDeviceList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button playSounds;
    }
}

