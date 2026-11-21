namespace TP4_PART2
{
    partial class Form1
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
            this.Btn_Afficher = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Text_box = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Btn_Afficher
            // 
            this.Btn_Afficher.Location = new System.Drawing.Point(67, 32);
            this.Btn_Afficher.Name = "Btn_Afficher";
            this.Btn_Afficher.Size = new System.Drawing.Size(95, 42);
            this.Btn_Afficher.TabIndex = 0;
            this.Btn_Afficher.Text = "button1";
            this.Btn_Afficher.UseVisualStyleBackColor = true;
            this.Btn_Afficher.Click += new System.EventHandler(this.Btn_Afficher_Click_1);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(67, 190);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(656, 330);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick_1);
            // 
            // Text_box
            // 
            this.Text_box.Location = new System.Drawing.Point(67, 92);
            this.Text_box.Multiline = true;
            this.Text_box.Name = "Text_box";
            this.Text_box.Size = new System.Drawing.Size(450, 56);
            this.Text_box.TabIndex = 2;
            this.Text_box.TextChanged += new System.EventHandler(this.Text_box_TextChanged_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 632);
            this.Controls.Add(this.Text_box);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.Btn_Afficher);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btn_Afficher;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox Text_box;
    }
}

