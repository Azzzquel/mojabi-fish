namespace GAMEU1_TAP4B
{
    partial class registro
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
            datagridPerfiles = new DataGridView();
            txtnombre = new TextBox();
            btneliminar = new Button();
            btnguardarcambios = new Button();
            txtbuscarperfil = new TextBox();
            btnbuscar = new Button();
            ((System.ComponentModel.ISupportInitialize)datagridPerfiles).BeginInit();
            SuspendLayout();
            // 
            // datagridPerfiles
            // 
            datagridPerfiles.AllowUserToAddRows = false;
            datagridPerfiles.AllowUserToDeleteRows = false;
            datagridPerfiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            datagridPerfiles.Location = new Point(284, 212);
            datagridPerfiles.Name = "datagridPerfiles";
            datagridPerfiles.ReadOnly = true;
            datagridPerfiles.RowHeadersWidth = 51;
            datagridPerfiles.Size = new Size(398, 232);
            datagridPerfiles.TabIndex = 0;
            datagridPerfiles.CellClick += datagridPerfiles_CellClick;
            // 
            // txtnombre
            // 
            txtnombre.Location = new Point(542, 167);
            txtnombre.Name = "txtnombre";
            txtnombre.Size = new Size(175, 27);
            txtnombre.TabIndex = 1;
            // 
            // btneliminar
            // 
            btneliminar.BackColor = Color.Transparent;
            btneliminar.FlatAppearance.BorderSize = 0;
            btneliminar.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btneliminar.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btneliminar.FlatStyle = FlatStyle.Flat;
            btneliminar.Location = new Point(343, 469);
            btneliminar.Margin = new Padding(3, 4, 3, 4);
            btneliminar.Name = "btneliminar";
            btneliminar.Size = new Size(217, 58);
            btneliminar.TabIndex = 4;
            btneliminar.UseVisualStyleBackColor = false;
            btneliminar.Click += btneliminar_Click;
            // 
            // btnguardarcambios
            // 
            btnguardarcambios.BackColor = Color.Transparent;
            btnguardarcambios.FlatAppearance.BorderSize = 0;
            btnguardarcambios.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnguardarcambios.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnguardarcambios.FlatStyle = FlatStyle.Flat;
            btnguardarcambios.Location = new Point(31, 469);
            btnguardarcambios.Margin = new Padding(3, 4, 3, 4);
            btnguardarcambios.Name = "btnguardarcambios";
            btnguardarcambios.Size = new Size(277, 58);
            btnguardarcambios.TabIndex = 5;
            btnguardarcambios.UseVisualStyleBackColor = false;
            btnguardarcambios.Click += btnguardarcambios_Click;
            // 
            // txtbuscarperfil
            // 
            txtbuscarperfil.Location = new Point(811, 485);
            txtbuscarperfil.Name = "txtbuscarperfil";
            txtbuscarperfil.Size = new Size(125, 27);
            txtbuscarperfil.TabIndex = 6;
            // 
            // btnbuscar
            // 
            btnbuscar.BackColor = Color.Transparent;
            btnbuscar.FlatAppearance.BorderSize = 0;
            btnbuscar.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnbuscar.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnbuscar.FlatStyle = FlatStyle.Flat;
            btnbuscar.Location = new Point(588, 469);
            btnbuscar.Margin = new Padding(3, 4, 3, 4);
            btnbuscar.Name = "btnbuscar";
            btnbuscar.Size = new Size(217, 58);
            btnbuscar.TabIndex = 7;
            btnbuscar.UseVisualStyleBackColor = false;
            btnbuscar.Click += btnbuscar_Click;
            // 
            // registro
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.perfiles;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(970, 551);
            Controls.Add(btnbuscar);
            Controls.Add(txtbuscarperfil);
            Controls.Add(btnguardarcambios);
            Controls.Add(btneliminar);
            Controls.Add(txtnombre);
            Controls.Add(datagridPerfiles);
            DoubleBuffered = true;
            Name = "registro";
            Text = "registro";
            ((System.ComponentModel.ISupportInitialize)datagridPerfiles).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView datagridPerfiles;
        private TextBox txtnombre;
        private Button btnGuarCambios;
        private Button btneliminar;
        private Button btnguardarcambios;
        private TextBox txtbuscarperfil;
        private Button btnbuscar;
    }
}