namespace GAMEU1_TAP4B
{
    partial class MenuGameOver
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
            btnMenu = new Button();
            btsnSalir = new Button();
            SuspendLayout();
            // 
            // btnMenu
            // 
            btnMenu.BackColor = Color.Transparent;
            btnMenu.FlatAppearance.BorderSize = 0;
            btnMenu.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnMenu.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnMenu.FlatStyle = FlatStyle.Flat;
            btnMenu.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnMenu.Location = new Point(343, 454);
            btnMenu.Margin = new Padding(3, 4, 3, 4);
            btnMenu.Name = "btnMenu";
            btnMenu.Size = new Size(246, 81);
            btnMenu.TabIndex = 4;
            btnMenu.UseVisualStyleBackColor = false;
            btnMenu.Click += button1_Click;
            // 
            // btsnSalir
            // 
            btsnSalir.BackColor = Color.Transparent;
            btsnSalir.FlatAppearance.BorderSize = 0;
            btsnSalir.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btsnSalir.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btsnSalir.FlatStyle = FlatStyle.Flat;
            btsnSalir.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btsnSalir.Location = new Point(660, 454);
            btsnSalir.Margin = new Padding(3, 4, 3, 4);
            btsnSalir.Name = "btsnSalir";
            btsnSalir.Size = new Size(251, 76);
            btsnSalir.TabIndex = 5;
            btsnSalir.UseVisualStyleBackColor = false;
            btsnSalir.Click += button2_Click;
            // 
            // MenuGameOver
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources._23;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1262, 673);
            Controls.Add(btsnSalir);
            Controls.Add(btnMenu);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "MenuGameOver";
            Text = "MenuGameOver";
            Load += MenuGameOver_Load;
            ResumeLayout(false);
        }

        #endregion
        private Button btnMenu;
        private Button btsnSalir;
    }
}