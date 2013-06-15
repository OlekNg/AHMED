namespace SimpleVisualizer
{
    partial class VisualizerForm
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
            this.uxImage = new System.Windows.Forms.PictureBox();
            this.uxRefreshButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.uxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // uxImage
            // 
            this.uxImage.Location = new System.Drawing.Point(12, 41);
            this.uxImage.Name = "uxImage";
            this.uxImage.Size = new System.Drawing.Size(608, 296);
            this.uxImage.TabIndex = 0;
            this.uxImage.TabStop = false;
            // 
            // uxRefreshButton
            // 
            this.uxRefreshButton.Location = new System.Drawing.Point(12, 12);
            this.uxRefreshButton.Name = "uxRefreshButton";
            this.uxRefreshButton.Size = new System.Drawing.Size(75, 23);
            this.uxRefreshButton.TabIndex = 1;
            this.uxRefreshButton.Text = "Refresh";
            this.uxRefreshButton.UseVisualStyleBackColor = true;
            this.uxRefreshButton.Click += new System.EventHandler(this.uxRefreshButton_Click);
            // 
            // VisualizerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 482);
            this.Controls.Add(this.uxRefreshButton);
            this.Controls.Add(this.uxImage);
            this.Name = "VisualizerForm";
            this.Text = "AHMED Visualizer";
            ((System.ComponentModel.ISupportInitialize)(this.uxImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox uxImage;
        private System.Windows.Forms.Button uxRefreshButton;
    }
}