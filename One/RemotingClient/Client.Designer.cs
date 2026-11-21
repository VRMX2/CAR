namespace RemotingClient
{
    partial class Client
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
            this.Add = new System.Windows.Forms.Button();
            this.Update = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.Button();
            this.Show = new System.Windows.Forms.Button();
            this.ShowStudent = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxMatricule = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBoxPrenom = new System.Windows.Forms.TextBox();
            this.textBoxNom = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(710, 228);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(129, 39);
            this.Add.TabIndex = 0;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Update
            // 
            this.Update.Location = new System.Drawing.Point(710, 310);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(129, 39);
            this.Update.TabIndex = 1;
            this.Update.Text = "Update";
            this.Update.UseVisualStyleBackColor = true;
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(710, 382);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(129, 39);
            this.Delete.TabIndex = 2;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Show
            // 
            this.Show.Location = new System.Drawing.Point(710, 454);
            this.Show.Name = "Show";
            this.Show.Size = new System.Drawing.Size(129, 39);
            this.Show.TabIndex = 3;
            this.Show.Text = "Show";
            this.Show.UseVisualStyleBackColor = true;
            this.Show.Click += new System.EventHandler(this.Show_Click);
            // 
            // ShowStudent
            // 
            this.ShowStudent.Location = new System.Drawing.Point(710, 531);
            this.ShowStudent.Name = "ShowStudent";
            this.ShowStudent.Size = new System.Drawing.Size(129, 39);
            this.ShowStudent.TabIndex = 4;
            this.ShowStudent.Text = "Show Student";
            this.ShowStudent.UseVisualStyleBackColor = true;
            this.ShowStudent.Click += new System.EventHandler(this.ShowStudent_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(51, 49);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 26);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "matricule";
            // 
            // textBoxMatricule
            // 
            this.textBoxMatricule.Location = new System.Drawing.Point(193, 49);
            this.textBoxMatricule.Name = "textBoxMatricule";
            this.textBoxMatricule.Size = new System.Drawing.Size(100, 26);
            this.textBoxMatricule.TabIndex = 6;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(51, 110);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 26);
            this.textBox3.TabIndex = 7;
            this.textBox3.Text = "nom";
            // 
            // textBoxPrenom
            // 
            this.textBoxPrenom.Location = new System.Drawing.Point(193, 179);
            this.textBoxPrenom.Name = "textBoxPrenom";
            this.textBoxPrenom.Size = new System.Drawing.Size(100, 26);
            this.textBoxPrenom.TabIndex = 8;
            // 
            // textBoxNom
            // 
            this.textBoxNom.Location = new System.Drawing.Point(193, 119);
            this.textBoxNom.Name = "textBoxNom";
            this.textBoxNom.Size = new System.Drawing.Size(100, 26);
            this.textBoxNom.TabIndex = 8;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(51, 170);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 26);
            this.textBox6.TabIndex = 9;
            this.textBox6.Text = "prenom";
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(51, 228);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersWidth = 62;
            this.dgv.RowTemplate.Height = 28;
            this.dgv.Size = new System.Drawing.Size(653, 342);
            this.dgv.TabIndex = 10;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 685);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBoxNom);
            this.Controls.Add(this.textBoxPrenom);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBoxMatricule);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ShowStudent);
            this.Controls.Add(this.Show);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.Update);
            this.Controls.Add(this.Add);
            this.Name = "Client";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Client_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button Update;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.Button Show;
        private System.Windows.Forms.Button ShowStudent;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBoxMatricule;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBoxPrenom;
        private System.Windows.Forms.TextBox textBoxNom;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.DataGridView dgv;
    }
}

