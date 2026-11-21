namespace TP1
{
    partial class Calculatrice
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
            this.Resultat = new System.Windows.Forms.TextBox();
            this.Btn0 = new System.Windows.Forms.Button();
            this.BtnPlus = new System.Windows.Forms.Button();
            this.BtnVirgule = new System.Windows.Forms.Button();
            this.BtnEgal = new System.Windows.Forms.Button();
            this.BtnMoins = new System.Windows.Forms.Button();
            this.Btn3 = new System.Windows.Forms.Button();
            this.Btn2 = new System.Windows.Forms.Button();
            this.Btn1 = new System.Windows.Forms.Button();
            this.BtnInverse = new System.Windows.Forms.Button();
            this.BtnMul = new System.Windows.Forms.Button();
            this.Btn6 = new System.Windows.Forms.Button();
            this.Btn5 = new System.Windows.Forms.Button();
            this.Btn4 = new System.Windows.Forms.Button();
            this.Btn7 = new System.Windows.Forms.Button();
            this.Btn8 = new System.Windows.Forms.Button();
            this.Btn9 = new System.Windows.Forms.Button();
            this.BtnDiv = new System.Windows.Forms.Button();
            this.BtnPourcent = new System.Windows.Forms.Button();
            this.BtnRetour = new System.Windows.Forms.Button();
            this.BtnCE = new System.Windows.Forms.Button();
            this.BtnC = new System.Windows.Forms.Button();
            this.BtnPlusMoins = new System.Windows.Forms.Button();
            this.BtnRacine = new System.Windows.Forms.Button();
            this.BtnMC = new System.Windows.Forms.Button();
            this.BtnMR = new System.Windows.Forms.Button();
            this.BtnMS = new System.Windows.Forms.Button();
            this.BtnMPlus = new System.Windows.Forms.Button();
            this.BtnMMoins = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Resultat
            // 
            this.Resultat.Location = new System.Drawing.Point(112, 113);
            this.Resultat.Multiline = true;
            this.Resultat.Name = "Resultat";
            this.Resultat.Size = new System.Drawing.Size(279, 44);
            this.Resultat.TabIndex = 0;
            this.Resultat.TextChanged += new System.EventHandler(this.Resultat_TextChanged);
            // 
            // Btn0
            // 
            this.Btn0.Location = new System.Drawing.Point(112, 443);
            this.Btn0.Name = "Btn0";
            this.Btn0.Size = new System.Drawing.Size(108, 50);
            this.Btn0.TabIndex = 1;
            this.Btn0.Text = "0";
            this.Btn0.UseVisualStyleBackColor = true;
            this.Btn0.Click += new System.EventHandler(this.Btn0_Click);
            // 
            // BtnPlus
            // 
            this.BtnPlus.Location = new System.Drawing.Point(283, 443);
            this.BtnPlus.Name = "BtnPlus";
            this.BtnPlus.Size = new System.Drawing.Size(51, 50);
            this.BtnPlus.TabIndex = 2;
            this.BtnPlus.Text = "+";
            this.BtnPlus.UseVisualStyleBackColor = true;
            this.BtnPlus.Click += new System.EventHandler(this.BtnPlus_Click);
            // 
            // BtnVirgule
            // 
            this.BtnVirgule.Location = new System.Drawing.Point(226, 443);
            this.BtnVirgule.Name = "BtnVirgule";
            this.BtnVirgule.Size = new System.Drawing.Size(51, 50);
            this.BtnVirgule.TabIndex = 3;
            this.BtnVirgule.Text = ".";
            this.BtnVirgule.UseVisualStyleBackColor = true;
            this.BtnVirgule.Click += new System.EventHandler(this.BtnVirgule_Click);
            // 
            // BtnEgal
            // 
            this.BtnEgal.Location = new System.Drawing.Point(340, 387);
            this.BtnEgal.Name = "BtnEgal";
            this.BtnEgal.Size = new System.Drawing.Size(51, 106);
            this.BtnEgal.TabIndex = 4;
            this.BtnEgal.Text = "=";
            this.BtnEgal.UseVisualStyleBackColor = true;
            this.BtnEgal.Click += new System.EventHandler(this.BtnEgal_Click);
            // 
            // BtnMoins
            // 
            this.BtnMoins.Location = new System.Drawing.Point(283, 387);
            this.BtnMoins.Name = "BtnMoins";
            this.BtnMoins.Size = new System.Drawing.Size(51, 50);
            this.BtnMoins.TabIndex = 5;
            this.BtnMoins.Text = "-";
            this.BtnMoins.UseVisualStyleBackColor = true;
            this.BtnMoins.Click += new System.EventHandler(this.BtnMoins_Click);
            // 
            // Btn3
            // 
            this.Btn3.Location = new System.Drawing.Point(226, 387);
            this.Btn3.Name = "Btn3";
            this.Btn3.Size = new System.Drawing.Size(51, 50);
            this.Btn3.TabIndex = 6;
            this.Btn3.Text = "3";
            this.Btn3.UseVisualStyleBackColor = true;
            this.Btn3.Click += new System.EventHandler(this.Btn3_Click);
            // 
            // Btn2
            // 
            this.Btn2.Location = new System.Drawing.Point(169, 387);
            this.Btn2.Name = "Btn2";
            this.Btn2.Size = new System.Drawing.Size(51, 50);
            this.Btn2.TabIndex = 7;
            this.Btn2.Text = "2";
            this.Btn2.UseVisualStyleBackColor = true;
            this.Btn2.Click += new System.EventHandler(this.Btn2_Click);
            // 
            // Btn1
            // 
            this.Btn1.Location = new System.Drawing.Point(112, 387);
            this.Btn1.Name = "Btn1";
            this.Btn1.Size = new System.Drawing.Size(51, 50);
            this.Btn1.TabIndex = 8;
            this.Btn1.Text = "1";
            this.Btn1.UseVisualStyleBackColor = true;
            this.Btn1.Click += new System.EventHandler(this.Btn1_Click);
            // 
            // BtnInverse
            // 
            this.BtnInverse.Location = new System.Drawing.Point(340, 331);
            this.BtnInverse.Name = "BtnInverse";
            this.BtnInverse.Size = new System.Drawing.Size(51, 50);
            this.BtnInverse.TabIndex = 9;
            this.BtnInverse.Text = "1/x";
            this.BtnInverse.UseVisualStyleBackColor = true;
            this.BtnInverse.Click += new System.EventHandler(this.BtnInverse_Click);
            // 
            // BtnMul
            // 
            this.BtnMul.Location = new System.Drawing.Point(283, 331);
            this.BtnMul.Name = "BtnMul";
            this.BtnMul.Size = new System.Drawing.Size(51, 50);
            this.BtnMul.TabIndex = 10;
            this.BtnMul.Text = "*";
            this.BtnMul.UseVisualStyleBackColor = true;
            this.BtnMul.Click += new System.EventHandler(this.BtnMul_Click);
            // 
            // Btn6
            // 
            this.Btn6.Location = new System.Drawing.Point(226, 331);
            this.Btn6.Name = "Btn6";
            this.Btn6.Size = new System.Drawing.Size(51, 50);
            this.Btn6.TabIndex = 11;
            this.Btn6.Text = "6";
            this.Btn6.UseVisualStyleBackColor = true;
            this.Btn6.Click += new System.EventHandler(this.Btn6_Click);
            // 
            // Btn5
            // 
            this.Btn5.Location = new System.Drawing.Point(169, 331);
            this.Btn5.Name = "Btn5";
            this.Btn5.Size = new System.Drawing.Size(51, 50);
            this.Btn5.TabIndex = 12;
            this.Btn5.Text = "5";
            this.Btn5.UseVisualStyleBackColor = true;
            this.Btn5.Click += new System.EventHandler(this.Btn5_Click);
            // 
            // Btn4
            // 
            this.Btn4.Location = new System.Drawing.Point(112, 331);
            this.Btn4.Name = "Btn4";
            this.Btn4.Size = new System.Drawing.Size(51, 50);
            this.Btn4.TabIndex = 13;
            this.Btn4.Text = "4";
            this.Btn4.UseVisualStyleBackColor = true;
            this.Btn4.Click += new System.EventHandler(this.Btn4_Click);
            // 
            // Btn7
            // 
            this.Btn7.Location = new System.Drawing.Point(112, 275);
            this.Btn7.Name = "Btn7";
            this.Btn7.Size = new System.Drawing.Size(51, 50);
            this.Btn7.TabIndex = 18;
            this.Btn7.Text = "7";
            this.Btn7.UseVisualStyleBackColor = true;
            this.Btn7.Click += new System.EventHandler(this.Btn7_Click);
            // 
            // Btn8
            // 
            this.Btn8.Location = new System.Drawing.Point(169, 275);
            this.Btn8.Name = "Btn8";
            this.Btn8.Size = new System.Drawing.Size(51, 50);
            this.Btn8.TabIndex = 17;
            this.Btn8.Text = "8";
            this.Btn8.UseVisualStyleBackColor = true;
            this.Btn8.Click += new System.EventHandler(this.Btn8_Click);
            // 
            // Btn9
            // 
            this.Btn9.Location = new System.Drawing.Point(226, 275);
            this.Btn9.Name = "Btn9";
            this.Btn9.Size = new System.Drawing.Size(51, 50);
            this.Btn9.TabIndex = 16;
            this.Btn9.Text = "9";
            this.Btn9.UseVisualStyleBackColor = true;
            this.Btn9.Click += new System.EventHandler(this.Btn9_Click);
            // 
            // BtnDiv
            // 
            this.BtnDiv.Location = new System.Drawing.Point(283, 275);
            this.BtnDiv.Name = "BtnDiv";
            this.BtnDiv.Size = new System.Drawing.Size(51, 50);
            this.BtnDiv.TabIndex = 15;
            this.BtnDiv.Text = "/";
            this.BtnDiv.UseVisualStyleBackColor = true;
            this.BtnDiv.Click += new System.EventHandler(this.BtnDiv_Click);
            // 
            // BtnPourcent
            // 
            this.BtnPourcent.Location = new System.Drawing.Point(340, 275);
            this.BtnPourcent.Name = "BtnPourcent";
            this.BtnPourcent.Size = new System.Drawing.Size(51, 50);
            this.BtnPourcent.TabIndex = 14;
            this.BtnPourcent.Text = "%";
            this.BtnPourcent.UseVisualStyleBackColor = true;
            this.BtnPourcent.Click += new System.EventHandler(this.BtnPourcent_Click);
            // 
            // BtnRetour
            // 
            this.BtnRetour.Location = new System.Drawing.Point(112, 219);
            this.BtnRetour.Name = "BtnRetour";
            this.BtnRetour.Size = new System.Drawing.Size(51, 50);
            this.BtnRetour.TabIndex = 23;
            this.BtnRetour.Text = "<--";
            this.BtnRetour.UseVisualStyleBackColor = true;
            this.BtnRetour.Click += new System.EventHandler(this.BtnRetour_Click);
            // 
            // BtnCE
            // 
            this.BtnCE.Location = new System.Drawing.Point(169, 219);
            this.BtnCE.Name = "BtnCE";
            this.BtnCE.Size = new System.Drawing.Size(51, 50);
            this.BtnCE.TabIndex = 22;
            this.BtnCE.Text = "CE";
            this.BtnCE.UseVisualStyleBackColor = true;
            this.BtnCE.Click += new System.EventHandler(this.BtnCE_Click);
            // 
            // BtnC
            // 
            this.BtnC.Location = new System.Drawing.Point(226, 219);
            this.BtnC.Name = "BtnC";
            this.BtnC.Size = new System.Drawing.Size(51, 50);
            this.BtnC.TabIndex = 21;
            this.BtnC.Text = "C";
            this.BtnC.UseVisualStyleBackColor = true;
            this.BtnC.Click += new System.EventHandler(this.BtnC_Click);
            // 
            // BtnPlusMoins
            // 
            this.BtnPlusMoins.Location = new System.Drawing.Point(283, 219);
            this.BtnPlusMoins.Name = "BtnPlusMoins";
            this.BtnPlusMoins.Size = new System.Drawing.Size(51, 50);
            this.BtnPlusMoins.TabIndex = 20;
            this.BtnPlusMoins.Text = "+-";
            this.BtnPlusMoins.UseVisualStyleBackColor = true;
            this.BtnPlusMoins.Click += new System.EventHandler(this.BtnPlusMoins_Click);
            // 
            // BtnRacine
            // 
            this.BtnRacine.Location = new System.Drawing.Point(340, 219);
            this.BtnRacine.Name = "BtnRacine";
            this.BtnRacine.Size = new System.Drawing.Size(51, 50);
            this.BtnRacine.TabIndex = 19;
            this.BtnRacine.Text = "√";
            this.BtnRacine.UseVisualStyleBackColor = true;
            this.BtnRacine.Click += new System.EventHandler(this.BtnRacine_Click);
            // 
            // BtnMC
            // 
            this.BtnMC.Location = new System.Drawing.Point(112, 163);
            this.BtnMC.Name = "BtnMC";
            this.BtnMC.Size = new System.Drawing.Size(51, 50);
            this.BtnMC.TabIndex = 28;
            this.BtnMC.Text = "MC";
            this.BtnMC.UseVisualStyleBackColor = true;
            this.BtnMC.Click += new System.EventHandler(this.BtnMC_Click);
            // 
            // BtnMR
            // 
            this.BtnMR.Location = new System.Drawing.Point(169, 163);
            this.BtnMR.Name = "BtnMR";
            this.BtnMR.Size = new System.Drawing.Size(51, 50);
            this.BtnMR.TabIndex = 27;
            this.BtnMR.Text = "MR";
            this.BtnMR.UseVisualStyleBackColor = true;
            this.BtnMR.Click += new System.EventHandler(this.BtnMR_Click);
            // 
            // BtnMS
            // 
            this.BtnMS.Location = new System.Drawing.Point(226, 163);
            this.BtnMS.Name = "BtnMS";
            this.BtnMS.Size = new System.Drawing.Size(51, 50);
            this.BtnMS.TabIndex = 26;
            this.BtnMS.Text = "MS";
            this.BtnMS.UseVisualStyleBackColor = true;
            this.BtnMS.Click += new System.EventHandler(this.BtnMS_Click);
            // 
            // BtnMPlus
            // 
            this.BtnMPlus.Location = new System.Drawing.Point(283, 163);
            this.BtnMPlus.Name = "BtnMPlus";
            this.BtnMPlus.Size = new System.Drawing.Size(51, 50);
            this.BtnMPlus.TabIndex = 25;
            this.BtnMPlus.Text = "M+";
            this.BtnMPlus.UseVisualStyleBackColor = true;
            this.BtnMPlus.Click += new System.EventHandler(this.BtnMPlus_Click);
            // 
            // BtnMMoins
            // 
            this.BtnMMoins.Location = new System.Drawing.Point(340, 163);
            this.BtnMMoins.Name = "BtnMMoins";
            this.BtnMMoins.Size = new System.Drawing.Size(51, 50);
            this.BtnMMoins.TabIndex = 24;
            this.BtnMMoins.Text = "M-";
            this.BtnMMoins.UseVisualStyleBackColor = true;
            this.BtnMMoins.Click += new System.EventHandler(this.BtnMMoins_Click);
            // 
            // Calculatrice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 829);
            this.Controls.Add(this.BtnMC);
            this.Controls.Add(this.BtnMR);
            this.Controls.Add(this.BtnMS);
            this.Controls.Add(this.BtnMPlus);
            this.Controls.Add(this.BtnMMoins);
            this.Controls.Add(this.BtnRetour);
            this.Controls.Add(this.BtnCE);
            this.Controls.Add(this.BtnC);
            this.Controls.Add(this.BtnPlusMoins);
            this.Controls.Add(this.BtnRacine);
            this.Controls.Add(this.Btn7);
            this.Controls.Add(this.Btn8);
            this.Controls.Add(this.Btn9);
            this.Controls.Add(this.BtnDiv);
            this.Controls.Add(this.BtnPourcent);
            this.Controls.Add(this.Btn4);
            this.Controls.Add(this.Btn5);
            this.Controls.Add(this.Btn6);
            this.Controls.Add(this.BtnMul);
            this.Controls.Add(this.BtnInverse);
            this.Controls.Add(this.Btn1);
            this.Controls.Add(this.Btn2);
            this.Controls.Add(this.Btn3);
            this.Controls.Add(this.BtnMoins);
            this.Controls.Add(this.BtnEgal);
            this.Controls.Add(this.BtnVirgule);
            this.Controls.Add(this.BtnPlus);
            this.Controls.Add(this.Btn0);
            this.Controls.Add(this.Resultat);
            this.Name = "Calculatrice";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Resultat;
        private System.Windows.Forms.Button Btn0;
        private System.Windows.Forms.Button BtnPlus;
        private System.Windows.Forms.Button BtnVirgule;
        private System.Windows.Forms.Button BtnEgal;
        private System.Windows.Forms.Button BtnMoins;
        private System.Windows.Forms.Button Btn3;
        private System.Windows.Forms.Button Btn2;
        private System.Windows.Forms.Button Btn1;
        private System.Windows.Forms.Button BtnInverse;
        private System.Windows.Forms.Button BtnMul;
        private System.Windows.Forms.Button Btn6;
        private System.Windows.Forms.Button Btn5;
        private System.Windows.Forms.Button Btn4;
        private System.Windows.Forms.Button Btn7;
        private System.Windows.Forms.Button Btn8;
        private System.Windows.Forms.Button Btn9;
        private System.Windows.Forms.Button BtnDiv;
        private System.Windows.Forms.Button BtnPourcent;
        private System.Windows.Forms.Button BtnRetour;
        private System.Windows.Forms.Button BtnCE;
        private System.Windows.Forms.Button BtnC;
        private System.Windows.Forms.Button BtnPlusMoins;
        private System.Windows.Forms.Button BtnRacine;
        private System.Windows.Forms.Button BtnMC;
        private System.Windows.Forms.Button BtnMR;
        private System.Windows.Forms.Button BtnMS;
        private System.Windows.Forms.Button BtnMPlus;
        private System.Windows.Forms.Button BtnMMoins;
    }
}

