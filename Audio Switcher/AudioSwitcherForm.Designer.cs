namespace Audio_Switcher {
	partial class frmAudioSwitcher {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.lstBoxOutputDevices = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// lstBoxOutputDevices
			// 
			this.lstBoxOutputDevices.FormattingEnabled = true;
			this.lstBoxOutputDevices.Location = new System.Drawing.Point(12, 12);
			this.lstBoxOutputDevices.Name = "lstBoxOutputDevices";
			this.lstBoxOutputDevices.Size = new System.Drawing.Size(260, 108);
			this.lstBoxOutputDevices.TabIndex = 0;
			this.lstBoxOutputDevices.SelectedIndexChanged += new System.EventHandler(this.lstBoxOutputDevices_SelectedIndexChanged);
			// 
			// frmAudioSwitcher
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 133);
			this.Controls.Add(this.lstBoxOutputDevices);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAudioSwitcher";
			this.Text = "Audio Switcher";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lstBoxOutputDevices;
	}
}

