namespace RemotingClient
{
    partial class FormCourse
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
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBoxDes = new System.Windows.Forms.TextBox();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxVol = new System.Windows.Forms.TextBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.Add = new System.Windows.Forms.Button();
            this.Update = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.Button();
            this.Show = new System.Windows.Forms.Button();
            this.ShowCourse = new System.Windows.Forms.Button();
            this.comboBoxCat = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(360, 132);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 26);
            this.textBox6.TabIndex = 11;
            this.textBox6.Text = "Categorie";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(360, 50);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 26);
            this.textBox5.TabIndex = 10;
            this.textBox5.Text = "Volume";
            // 
            // textBoxDes
            // 
            this.textBoxDes.Location = new System.Drawing.Point(175, 132);
            this.textBoxDes.Name = "textBoxDes";
            this.textBoxDes.Size = new System.Drawing.Size(100, 26);
            this.textBoxDes.TabIndex = 9;
            // 
            // textBoxCode
            // 
            this.textBoxCode.Location = new System.Drawing.Point(175, 50);
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(100, 26);
            this.textBoxCode.TabIndex = 8;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(17, 132);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 26);
            this.textBox2.TabIndex = 7;
            this.textBox2.Text = "Designation";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(17, 50);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 26);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "Code";
            // 
            // textBoxVol
            // 
            this.textBoxVol.Location = new System.Drawing.Point(530, 50);
            this.textBoxVol.Name = "textBoxVol";
            this.textBoxVol.Size = new System.Drawing.Size(100, 26);
            this.textBoxVol.TabIndex = 12;
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(17, 205);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersWidth = 62;
            this.dgv.RowTemplate.Height = 28;
            this.dgv.Size = new System.Drawing.Size(800, 519);
            this.dgv.TabIndex = 14;
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(861, 218);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(133, 46);
            this.Add.TabIndex = 15;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // Update
            // 
            this.Update.Location = new System.Drawing.Point(861, 318);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(133, 46);
            this.Update.TabIndex = 16;
            this.Update.Text = "Update";
            this.Update.UseVisualStyleBackColor = true;
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(861, 411);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(133, 46);
            this.Delete.TabIndex = 17;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Show
            // 
            this.Show.Location = new System.Drawing.Point(861, 497);
            this.Show.Name = "Show";
            this.Show.Size = new System.Drawing.Size(133, 46);
            this.Show.TabIndex = 18;
            this.Show.Text = "Show";
            this.Show.UseVisualStyleBackColor = true;
            this.Show.Click += new System.EventHandler(this.Show_Click);
            // 
            // ShowCourse
            // 
            this.ShowCourse.Location = new System.Drawing.Point(861, 575);
            this.ShowCourse.Name = "ShowCourse";
            this.ShowCourse.Size = new System.Drawing.Size(133, 46);
            this.ShowCourse.TabIndex = 19;
            this.ShowCourse.Text = "Show Course";
            this.ShowCourse.UseVisualStyleBackColor = true;
            this.ShowCourse.Click += new System.EventHandler(this.ShowCourse_Click);
            // 
            // comboBoxCat
            // 
            this.comboBoxCat.FormattingEnabled = true;
            this.comboBoxCat.Location = new System.Drawing.Point(530, 130);
            this.comboBoxCat.Name = "comboBoxCat";
            this.comboBoxCat.Size = new System.Drawing.Size(121, 28);
            this.comboBoxCat.TabIndex = 20;
            // 
            // FormCourse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 777);
            this.Controls.Add(this.comboBoxCat);
            this.Controls.Add(this.ShowCourse);
            this.Controls.Add(this.Show);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.Update);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.textBoxVol);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBoxDes);
            this.Controls.Add(this.textBoxCode);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "FormCourse";
            this.Text = "FormCourse";
            this.Load += new System.EventHandler(this.FormCourse_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBoxDes;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBoxVol;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button Update;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.Button Show;
        private System.Windows.Forms.Button ShowCourse;
        private System.Windows.Forms.ComboBox comboBoxCat;
    }
}