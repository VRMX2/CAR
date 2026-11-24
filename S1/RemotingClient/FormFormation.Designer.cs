namespace RemotingClient
{
    partial class FormFormation
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
            this.comboBoxNiv = new System.Windows.Forms.ComboBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBoxDate = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBoxPrix = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBoxInt = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBoxCode = new System.Windows.Forms.TextBox();
            this.textBoxDuree = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Search = new System.Windows.Forms.Button();
            this.Show = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.Button();
            this.Update = new System.Windows.Forms.Button();
            this.Add = new System.Windows.Forms.Button();
            this.dgv = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxNiv
            // 
            this.comboBoxNiv.FormattingEnabled = true;
            this.comboBoxNiv.Location = new System.Drawing.Point(766, 84);
            this.comboBoxNiv.Name = "comboBoxNiv";
            this.comboBoxNiv.Size = new System.Drawing.Size(121, 28);
            this.comboBoxNiv.TabIndex = 45;
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(623, 84);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(100, 26);
            this.textBox11.TabIndex = 44;
            this.textBox11.Text = "Niveau";
            // 
            // textBoxDate
            // 
            this.textBoxDate.Location = new System.Drawing.Point(757, 25);
            this.textBoxDate.Name = "textBoxDate";
            this.textBoxDate.Size = new System.Drawing.Size(100, 26);
            this.textBoxDate.TabIndex = 43;
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(623, 26);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(100, 26);
            this.textBox9.TabIndex = 42;
            this.textBox9.Text = "date debut";
            // 
            // textBoxPrix
            // 
            this.textBoxPrix.Location = new System.Drawing.Point(448, 84);
            this.textBoxPrix.Name = "textBoxPrix";
            this.textBoxPrix.Size = new System.Drawing.Size(100, 26);
            this.textBoxPrix.TabIndex = 41;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(315, 84);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(100, 26);
            this.textBox7.TabIndex = 40;
            this.textBox7.Text = "prix";
            // 
            // textBoxInt
            // 
            this.textBoxInt.Location = new System.Drawing.Point(448, 26);
            this.textBoxInt.Name = "textBoxInt";
            this.textBoxInt.Size = new System.Drawing.Size(100, 26);
            this.textBoxInt.TabIndex = 39;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(315, 26);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 26);
            this.textBox5.TabIndex = 38;
            this.textBox5.Text = "intitule";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(6, 84);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 26);
            this.textBox4.TabIndex = 37;
            this.textBox4.Text = "duree";
            // 
            // textBoxCode
            // 
            this.textBoxCode.Location = new System.Drawing.Point(135, 26);
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(100, 26);
            this.textBoxCode.TabIndex = 35;
            // 
            // textBoxDuree
            // 
            this.textBoxDuree.Location = new System.Drawing.Point(145, 84);
            this.textBoxDuree.Name = "textBoxDuree";
            this.textBoxDuree.Size = new System.Drawing.Size(100, 26);
            this.textBoxDuree.TabIndex = 36;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 26);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 26);
            this.textBox1.TabIndex = 34;
            this.textBox1.Text = "code";
            // 
            // Search
            // 
            this.Search.Location = new System.Drawing.Point(916, 544);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(129, 52);
            this.Search.TabIndex = 50;
            this.Search.Text = "search";
            this.Search.UseVisualStyleBackColor = true;
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // Show
            // 
            this.Show.Location = new System.Drawing.Point(916, 469);
            this.Show.Name = "Show";
            this.Show.Size = new System.Drawing.Size(129, 52);
            this.Show.TabIndex = 49;
            this.Show.Text = "Show";
            this.Show.UseVisualStyleBackColor = true;
            this.Show.Click += new System.EventHandler(this.Show_Click);
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(916, 381);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(129, 52);
            this.Delete.TabIndex = 48;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Update
            // 
            this.Update.Location = new System.Drawing.Point(916, 292);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(129, 52);
            this.Update.TabIndex = 47;
            this.Update.Text = "Update";
            this.Update.UseVisualStyleBackColor = true;
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(916, 203);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(129, 52);
            this.Add.TabIndex = 46;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(12, 149);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersWidth = 62;
            this.dgv.RowTemplate.Height = 28;
            this.dgv.Size = new System.Drawing.Size(898, 447);
            this.dgv.TabIndex = 51;
            // 
            // FormFormation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 694);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.Search);
            this.Controls.Add(this.Show);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.Update);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.comboBoxNiv);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.textBoxDate);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBoxPrix);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBoxInt);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBoxCode);
            this.Controls.Add(this.textBoxDuree);
            this.Controls.Add(this.textBox1);
            this.Name = "FormFormation";
            this.Text = "FormFormation";
            this.Load += new System.EventHandler(this.FormFormation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxNiv;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.TextBox textBoxDate;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBoxPrix;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBoxInt;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.TextBox textBoxDuree;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Search;
        private System.Windows.Forms.Button Show;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.Button Update;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.DataGridView dgv;
    }
}