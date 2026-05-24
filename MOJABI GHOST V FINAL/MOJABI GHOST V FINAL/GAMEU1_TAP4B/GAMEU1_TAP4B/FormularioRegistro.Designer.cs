namespace GAMEU1_TAP4B
{
    partial class FormularioRegistro
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
            datagridperfiles = new DataGridView();
            txtnombre = new TextBox();
            btnGuardarCambios = new Button();
            btneliminar = new Button();
            ((System.ComponentModel.ISupportInitialize)datagridperfiles).BeginInit();
            SuspendLayout();
            // 
            // datagridperfiles
            // 
            //datagridperfiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //datagridperfiles.Location = new Point(234, 181);
            //datagridperfiles.Name = "datagridperfiles";
            //datagridperfiles.RowHeadersWidth = 51;
            //datagridperfiles.Size = new Size(529, 261);
            //datagridperfiles.TabIndex = 0;
            //datagridperfiles.CellClick += dataGridView1_CellClick;
            // 
            // txtnombre
            // 
            txtnombre.Location = new Point(49, 43);
            txtnombre.Name = "txtnombre";
            txtnombre.Size = new Size(125, 27);
            txtnombre.TabIndex = 1;
            // 
            // btnGuardarCambios
            // 
            btnGuardarCambios.Location = new Point(436, 41);
            btnGuardarCambios.Name = "btnGuardarCambios";
            btnGuardarCambios.Size = new Size(289, 29);
            btnGuardarCambios.TabIndex = 2;
            btnGuardarCambios.Text = "guardarcambios";
            btnGuardarCambios.UseVisualStyleBackColor = true;
            btnGuardarCambios.Click += btnGuardarCambios_Click;
            // 
            // btneliminar
            // 
            btneliminar.Location = new Point(837, 308);
            btneliminar.Name = "btneliminar";
            btneliminar.Size = new Size(94, 29);
            btneliminar.TabIndex = 3;
            btneliminar.Text = "Eliminar";
            btneliminar.UseVisualStyleBackColor = true;
            // 
            // FormularioRegistro
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1061, 561);
            Controls.Add(btneliminar);
            Controls.Add(btnGuardarCambios);
            Controls.Add(txtnombre);
            Controls.Add(datagridperfiles);
            Name = "FormularioRegistro";
            Text = "FormularioRegistro";
            ((System.ComponentModel.ISupportInitialize)datagridperfiles).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView datagridperfiles;
        private TextBox txtnombre;
        private Button btnGuardarCambios;
        private Button btneliminar;
    }
}