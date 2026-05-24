namespace GAMEU1_TAP4B
{
    partial class FormRankeds
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
            datagridrankeds = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)datagridrankeds).BeginInit();
            SuspendLayout();
            // 
            // datagridrankeds
            // 
            datagridrankeds.AllowUserToAddRows = false;
            datagridrankeds.AllowUserToDeleteRows = false;
            datagridrankeds.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            datagridrankeds.Location = new Point(188, 148);
            datagridrankeds.Name = "datagridrankeds";
            datagridrankeds.ReadOnly = true;
            datagridrankeds.RowHeadersWidth = 51;
            datagridrankeds.Size = new Size(433, 135);
            datagridrankeds.TabIndex = 0;
            // 
            // FormRankeds
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.rankeds;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(795, 474);
            Controls.Add(datagridrankeds);
            DoubleBuffered = true;
            Name = "FormRankeds";
            Text = "FormRankeds";
            ((System.ComponentModel.ISupportInitialize)datagridrankeds).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView datagridrankeds;
    }
}