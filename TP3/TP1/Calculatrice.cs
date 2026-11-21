using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP1
{
    public partial class Calculatrice : Form
    {
        // ------------------------------------------------------------------
        //  Constantes
        // ------------------------------------------------------------------
        // Culture française : la virgule est le séparateur décimal (bouton ",")
        private static readonly CultureInfo Fr = CreerCulture();
        private const int MaxChiffres = 16;

        private static CultureInfo CreerCulture()
        {
            CultureInfo c = new CultureInfo("fr-FR");
            c.NumberFormat.NegativeSign = "-";          // signe moins ASCII (certaines cultures ICU utilisent "−")
            c.NumberFormat.NumberDecimalSeparator = ",";
            return c;
        }

        // ------------------------------------------------------------------
        //  .NET Remoting
        // ------------------------------------------------------------------
        // Pour 2 machines : remplacer localhost par l'IP (wifi) de la machine du serveur
        private const string UrlServeur = "tcp://localhost:1069/RemoteOperation";
        private RemotingInterfaces.IRemoteOperation remoteOperation;

        // ------------------------------------------------------------------
        //  État de la calculatrice
        // ------------------------------------------------------------------
        private string saisie = "0";          // nombre affiché sur la 2ème ligne
        private double operande = 0;          // premier opérande (avant l'opérateur)
        private string operateur = "";        // "+", "-", "×", "÷" ou "" si aucun
        private string termeUnaire = "";      // ex. "√(9)" affiché sur la 1ère ligne
        private string historiqueEgal = "";   // ex. "5 + 3 =" affiché après un "="

        // true  => le prochain chiffre tapé REMPLACE l'affichage
        //          (après un opérateur, un "=", un résultat, MR, ...)
        private bool nouvelleSaisie = true;

        // true  => l'affichage contient un opérande valide même si aucun chiffre
        //          n'a été tapé (après √, 1/x, %, ±, MR ...).
        //          Permet de distinguer "5 + ×" (on change juste l'opérateur)
        //          de "5 + √(9) ×" (on doit calculer 5 + 3 avant).
        private bool operandePret = false;

        // Mémoire (MC, MR, MS, M+, M-)
        private double memoire = 0;
        private bool memoireActive = false;
        private string titreOrigine;

        // ==================================================================
        //  Constructeur
        // ==================================================================
        public Calculatrice()
        {
            InitializeComponent();
            titreOrigine = string.IsNullOrEmpty(this.Text) ? "Calculatrice" : this.Text;
            AppliquerDesign();
            ConnecterServeur();
            Afficher();
        }

        // ==================================================================
        //  Connexion à l'objet distant (.NET Remoting)
        // ==================================================================
        private void ConnecterServeur()
        {
            try
            {
                TcpChannel channel = new TcpChannel();
                ChannelServices.RegisterChannel(channel, false);
                remoteOperation = (RemotingInterfaces.IRemoteOperation)Activator.GetObject(
                    typeof(RemotingInterfaces.IRemoteOperation), UrlServeur);
            }
            catch (Exception)
            {
                remoteOperation = null;
            }
        }

        // ==================================================================
        //  DESIGN : thème sombre moderne, appliqué par le code
        //  (les boutons sont retrouvés par leur Name : Btn0, BtnPlus, ...)
        // ==================================================================
        #region Design

        private static readonly Color CouleurFenetre = Color.FromArgb(32, 32, 32);
        private static readonly Color CouleurAccent = Color.FromArgb(76, 194, 255);

        private class StyleBouton
        {
            public Color Fond, Survol, Appui, Texte;
            public Font Police;

            public StyleBouton(Color fond, Color survol, Color appui, Color texte, Font police)
            {
                Fond = fond; Survol = survol; Appui = appui; Texte = texte; Police = police;
            }
        }

        private readonly HashSet<Button> boutonsSurvoles = new HashSet<Button>();
        private readonly HashSet<Button> boutonsEnfonces = new HashSet<Button>();

        private float echelle = 1f;                 // facteur d'échelle DPI (1 = 96 dpi)
        private int marge, espace, largeurBtn, hauteurBtn, rayon, topGrille;

        private int Px(int valeur) { return (int)Math.Round(valeur * echelle); }

        private void AppliquerDesign()
        {
            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.None;
            using (Graphics g = this.CreateGraphics()) echelle = g.DpiX / 96f;

            marge = Px(14); espace = Px(6);
            largeurBtn = Px(76); hauteurBtn = Px(56);
            rayon = Px(8);

            // ---- Fenêtre ------------------------------------------------
            this.BackColor = CouleurFenetre;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;

            // ---- Zone d'affichage multi-ligne ---------------------------
            Resultat.Parent = this;
            Resultat.Dock = DockStyle.None;
            Resultat.Multiline = true;
            Resultat.ReadOnly = true;
            Resultat.WordWrap = true;
            Resultat.ScrollBars = ScrollBars.None;
            Resultat.TextAlign = HorizontalAlignment.Right;
            Resultat.BorderStyle = BorderStyle.None;
            Resultat.BackColor = CouleurFenetre;
            Resultat.ForeColor = Color.White;
            Resultat.Font = new Font("Segoe UI Semibold", 22f);
            Resultat.Cursor = Cursors.Arrow;
            Resultat.TabStop = false;

            int largeurGrille = 5 * largeurBtn + 4 * espace;
            int hauteurAffichage = 2 * Resultat.Font.Height + Px(10);
            Resultat.SetBounds(marge, marge, largeurGrille, hauteurAffichage);
            topGrille = marge + hauteurAffichage + Px(10);

            // ---- Styles des boutons -------------------------------------
            Font fChiffre = new Font("Segoe UI Semibold", 15f);
            Font fFonction = new Font("Segoe UI", 12f);
            Font fOperateur = new Font("Segoe UI Semibold", 17f);
            Font fMemoire = new Font("Segoe UI Semibold", 10f);

            StyleBouton chiffre = new StyleBouton(Color.FromArgb(59, 59, 59), Color.FromArgb(72, 72, 72), Color.FromArgb(50, 50, 50), Color.White, fChiffre);
            StyleBouton fonction = new StyleBouton(Color.FromArgb(50, 50, 50), Color.FromArgb(62, 62, 62), Color.FromArgb(42, 42, 42), Color.White, fFonction);
            StyleBouton effacer = new StyleBouton(Color.FromArgb(50, 50, 50), Color.FromArgb(62, 62, 62), Color.FromArgb(42, 42, 42), Color.FromArgb(255, 120, 120), fFonction);
            StyleBouton operateurStyle = new StyleBouton(Color.FromArgb(50, 50, 50), Color.FromArgb(62, 62, 62), Color.FromArgb(42, 42, 42), CouleurAccent, fOperateur);
            StyleBouton egal = new StyleBouton(CouleurAccent, Color.FromArgb(103, 205, 255), Color.FromArgb(64, 170, 225), Color.Black, fOperateur);
            StyleBouton memoireStyle = new StyleBouton(CouleurFenetre, Color.FromArgb(45, 45, 45), Color.FromArgb(38, 38, 38), Color.FromArgb(165, 165, 165), fMemoire);

            // ---- Grille (même disposition que la calculatrice du TP) ----
            //            nom           lig col nbL nbC style           texte
            Placer("BtnMC", 0, 0, 1, 1, memoireStyle, "MC");
            Placer("BtnMR", 0, 1, 1, 1, memoireStyle, "MR");
            Placer("BtnMS", 0, 2, 1, 1, memoireStyle, "MS");
            Placer("BtnMPlus", 0, 3, 1, 1, memoireStyle, "M+");
            Placer("BtnMMoins", 0, 4, 1, 1, memoireStyle, "M-");

            Placer("BtnRetour", 1, 0, 1, 1, fonction, "←");
            Placer("BtnCE", 1, 1, 1, 1, fonction, "CE");
            Placer("BtnC", 1, 2, 1, 1, effacer, "C");
            Placer("BtnPlusMoins", 1, 3, 1, 1, fonction, "±");
            Placer("BtnRacine", 1, 4, 1, 1, fonction, "√");

            Placer("Btn7", 2, 0, 1, 1, chiffre, null);
            Placer("Btn8", 2, 1, 1, 1, chiffre, null);
            Placer("Btn9", 2, 2, 1, 1, chiffre, null);
            Placer("BtnDiv", 2, 3, 1, 1, operateurStyle, "÷");
            Placer("BtnPourcent", 2, 4, 1, 1, fonction, "%");

            Placer("Btn4", 3, 0, 1, 1, chiffre, null);
            Placer("Btn5", 3, 1, 1, 1, chiffre, null);
            Placer("Btn6", 3, 2, 1, 1, chiffre, null);
            Placer("BtnMul", 3, 3, 1, 1, operateurStyle, "×");
            Placer("BtnInverse", 3, 4, 1, 1, fonction, "1/x");

            Placer("Btn1", 4, 0, 1, 1, chiffre, null);
            Placer("Btn2", 4, 1, 1, 1, chiffre, null);
            Placer("Btn3", 4, 2, 1, 1, chiffre, null);
            Placer("BtnMoins", 4, 3, 1, 1, operateurStyle, "−");
            Placer("BtnEgal", 4, 4, 2, 1, egal, "=");   // occupe 2 lignes

            Placer("Btn0", 5, 0, 1, 2, chiffre, null);  // occupe 2 colonnes
            Placer("BtnVirgule", 5, 2, 1, 1, chiffre, ",");
            Placer("BtnPlus", 5, 3, 1, 1, operateurStyle, "+");

            this.ClientSize = new Size(2 * marge + largeurGrille,
                                       topGrille + 6 * hauteurBtn + 5 * espace + marge);
            this.ResumeLayout(true);
        }

        /// <summary>Retrouve un bouton par son nom, le positionne dans la grille et lui applique son style.</summary>
        private void Placer(string nom, int ligne, int colonne, int nbLignes, int nbColonnes, StyleBouton style, string texte)
        {
            Control[] trouves = this.Controls.Find(nom, true);
            if (trouves.Length == 0 || !(trouves[0] is Button)) return;
            Button b = (Button)trouves[0];

            b.Parent = this;                        // au cas où le bouton serait dans un Panel
            b.BringToFront();
            b.Dock = DockStyle.None;
            b.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            b.AutoSize = false;
            b.SetBounds(marge + colonne * (largeurBtn + espace),
                        topGrille + ligne * (hauteurBtn + espace),
                        nbColonnes * largeurBtn + (nbColonnes - 1) * espace,
                        nbLignes * hauteurBtn + (nbLignes - 1) * espace);

            if (texte != null) b.Text = texte;
            b.Font = style.Police;
            b.Tag = style;
            b.TabStop = false;
            b.Cursor = Cursors.Hand;

            // Bouton "plat" : le dessin réel est fait dans DessinerBouton (coins arrondis lissés)
            b.UseVisualStyleBackColor = false;
            b.BackColor = CouleurFenetre;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = CouleurFenetre;
            b.FlatAppearance.MouseDownBackColor = CouleurFenetre;

            b.Paint += DessinerBouton;
            b.MouseEnter += (s, ev) => { boutonsSurvoles.Add(b); b.Invalidate(); };
            b.MouseLeave += (s, ev) => { boutonsSurvoles.Remove(b); boutonsEnfonces.Remove(b); b.Invalidate(); };
            b.MouseDown += (s, ev) => { boutonsEnfonces.Add(b); b.Invalidate(); };
            b.MouseUp += (s, ev) => { boutonsEnfonces.Remove(b); b.Invalidate(); };
        }

        /// <summary>Dessine un bouton aux coins arrondis, avec effet de survol et de clic.</summary>
        private void DessinerBouton(object sender, PaintEventArgs e)
        {
            Button b = (Button)sender;
            StyleBouton st = (StyleBouton)b.Tag;
            Color fond = boutonsEnfonces.Contains(b) ? st.Appui
                       : boutonsSurvoles.Contains(b) ? st.Survol
                       : st.Fond;

            e.Graphics.Clear(CouleurFenetre);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle zone = new Rectangle(0, 0, b.Width - 1, b.Height - 1);
            using (GraphicsPath chemin = RectArrondi(zone, rayon))
            using (SolidBrush pinceau = new SolidBrush(fond))
            {
                e.Graphics.FillPath(pinceau, chemin);
            }

            TextRenderer.DrawText(e.Graphics, b.Text, b.Font, b.ClientRectangle, st.Texte,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding);
        }

        private static GraphicsPath RectArrondi(Rectangle r, int rad)
        {
            int d = rad * 2;
            GraphicsPath p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        // Barre de titre sombre (Windows 10 / 11) pour s'accorder avec le thème
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribut, ref int valeur, int taille);

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            try
            {
                int sombre = 1;
                if (DwmSetWindowAttribute(this.Handle, 20, ref sombre, sizeof(int)) != 0)
                    DwmSetWindowAttribute(this.Handle, 19, ref sombre, sizeof(int));
            }
            catch (Exception)
            {
                // Système plus ancien : on garde la barre de titre classique
            }
        }

        #endregion

        // ==================================================================
        //  Boutons chiffres et virgule  (réalisés par le CLIENT)
        // ==================================================================
        private void Btn0_Click(object sender, EventArgs e) { AjouterChiffre("0"); }
        private void Btn1_Click(object sender, EventArgs e) { AjouterChiffre("1"); }
        private void Btn2_Click(object sender, EventArgs e) { AjouterChiffre("2"); }
        private void Btn3_Click(object sender, EventArgs e) { AjouterChiffre("3"); }
        private void Btn4_Click(object sender, EventArgs e) { AjouterChiffre("4"); }
        private void Btn5_Click(object sender, EventArgs e) { AjouterChiffre("5"); }
        private void Btn6_Click(object sender, EventArgs e) { AjouterChiffre("6"); }
        private void Btn7_Click(object sender, EventArgs e) { AjouterChiffre("7"); }
        private void Btn8_Click(object sender, EventArgs e) { AjouterChiffre("8"); }
        private void Btn9_Click(object sender, EventArgs e) { AjouterChiffre("9"); }

        private void BtnVirgule_Click(object sender, EventArgs e)
        {
            if (nouvelleSaisie)
            {
                DebutNouvelleSaisie();
                saisie = "0,";
            }
            else if (!saisie.Contains(",") && NombreDeChiffres() < MaxChiffres)
            {
                saisie += ",";
            }
            Afficher();
        }

        // ==================================================================
        //  Opérations binaires : + - × ÷ =   (calcul réalisé par le SERVEUR)
        // ==================================================================
        private void BtnPlus_Click(object sender, EventArgs e) { ChoisirOperateur("+"); }
        private void BtnMoins_Click(object sender, EventArgs e) { ChoisirOperateur("-"); }
        private void BtnMul_Click(object sender, EventArgs e) { ChoisirOperateur("×"); }
        private void BtnDiv_Click(object sender, EventArgs e) { ChoisirOperateur("÷"); }

        private void BtnEgal_Click(object sender, EventArgs e)
        {
            if (operateur == "")
            {
                // Aucune opération en attente : on affiche simplement "valeur ="
                historiqueEgal = (termeUnaire != "" ? termeUnaire : Formater(Valeur())) + " =";
                saisie = Formater(Valeur());
                termeUnaire = "";
                nouvelleSaisie = true;
                operandePret = false;
                Afficher();
                return;
            }

            double b = Valeur();
            string membreDroit = termeUnaire != "" ? termeUnaire : Formater(b);
            string texte = Formater(operande) + " " + operateur + " " + membreDroit + " =";

            double resultat;
            if (!Calculer(operande, operateur, b, out resultat))
                return;                                   // erreur déjà signalée

            saisie = Formater(resultat);
            historiqueEgal = texte;
            operateur = "";
            termeUnaire = "";
            nouvelleSaisie = true;
            operandePret = false;
            Afficher();
        }

        // ==================================================================
        //  Opérations unaires
        //  ±  : réalisé par le CLIENT
        //  √ 1/x % : réalisés par le SERVEUR (méthodes distantes)
        // ==================================================================
        private void BtnPlusMoins_Click(object sender, EventArgs e)
        {
            if (nouvelleSaisie)
            {
                // L'affichage contient un résultat / un opérande : on l'oppose
                double x = Valeur();
                if (x == 0) return;
                AppliquerUnaire("negate(" + ArgumentCourant() + ")", -x);
            }
            else
            {
                // Nombre en cours de saisie : on change simplement son signe
                if (saisie == "0") return;
                saisie = saisie.StartsWith("-") ? saisie.Substring(1) : "-" + saisie;
                Afficher();
            }
        }

        private void BtnRacine_Click(object sender, EventArgs e)
        {
            double x = Valeur();
            if (x < 0)
            {
                Erreur("Impossible de calculer la racine d'un nombre négatif !");
                return;
            }
            double r;
            if (!Distant(() => remoteOperation.Racine(x), out r)) return;
            AppliquerUnaire("√(" + ArgumentCourant() + ")", r);
        }

        private void BtnInverse_Click(object sender, EventArgs e)
        {
            double x = Valeur();
            if (x == 0)
            {
                Erreur("Impossible de fractionner sur zéro !");
                return;
            }
            double r;
            if (!Distant(() => remoteOperation.Fraction(x), out r)) return;
            AppliquerUnaire("1/(" + ArgumentCourant() + ")", r);
        }

        private void BtnPourcent_Click(object sender, EventArgs e)
        {
            // Comme la calculatrice Windows :  a + b %  =>  a + (a × b / 100)
            // Sans opération en attente, le résultat est 0.
            double x = Valeur();
            double r = 0;
            if (operateur != "")
            {
                double p;
                if (!Distant(() => remoteOperation.Pourcentage(x), out p)) return;
                r = operande * p;                  // p = x / 100 calculé par le serveur
            }
            AppliquerUnaire(Formater(r), r);
        }

        // ==================================================================
        //  Effacement : ← CE C   (réalisés par le CLIENT)
        // ==================================================================
        private void BtnRetour_Click(object sender, EventArgs e)
        {
            // On ne peut pas effacer un résultat, seulement un nombre en cours de saisie
            if (nouvelleSaisie) return;

            saisie = (saisie.Length > 1) ? saisie.Substring(0, saisie.Length - 1) : "0";
            if (saisie == "-" || saisie == "-0") saisie = "0";
            Afficher();
        }

        private void BtnCE_Click(object sender, EventArgs e)
        {
            // CE : efface seulement le nombre affiché (l'opération en cours est conservée)
            if (operateur == "")
            {
                ToutEffacer();
                return;
            }
            saisie = "0";
            termeUnaire = "";
            historiqueEgal = "";
            nouvelleSaisie = true;
            operandePret = false;
            Afficher();
        }

        private void BtnC_Click(object sender, EventArgs e)
        {
            ToutEffacer();
        }

        // ==================================================================
        //  Mémoire : MC MR MS M+ M-   (réalisés par le CLIENT)
        // ==================================================================
        private void BtnMC_Click(object sender, EventArgs e)
        {
            memoire = 0;
            memoireActive = false;
            Afficher();
        }

        private void BtnMR_Click(object sender, EventArgs e)
        {
            saisie = Formater(memoire);
            termeUnaire = "";
            historiqueEgal = "";
            nouvelleSaisie = true;
            operandePret = true;
            Afficher();
        }

        private void BtnMS_Click(object sender, EventArgs e)
        {
            memoire = Valeur();
            memoireActive = true;
            FinSaisieMemoire();
        }

        private void BtnMPlus_Click(object sender, EventArgs e)
        {
            memoire += Valeur();
            memoireActive = true;
            FinSaisieMemoire();
        }

        private void BtnMMoins_Click(object sender, EventArgs e)
        {
            memoire -= Valeur();
            memoireActive = true;
            FinSaisieMemoire();
        }

        // ==================================================================
        //  Zone de texte multi-ligne
        // ==================================================================
        private void Resultat_TextChanged(object sender, EventArgs e)
        {
            // Garde toujours visible la dernière ligne (le nombre courant)
            Resultat.SelectionStart = Resultat.TextLength;
            Resultat.ScrollToCaret();
        }

        // ##################################################################
        //  MÉTHODES UTILITAIRES
        // ##################################################################

        /// <summary>Met à jour la TextBox : ligne 1 = historique, ligne 2 = nombre courant.</summary>
        private void Afficher()
        {
            Resultat.Text = Ligne1() + Environment.NewLine + saisie;
            // Indicateur "M" (mémoire non vide) dans la barre de titre
            this.Text = memoireActive ? titreOrigine + "  [M]" : titreOrigine;
        }

        private string Ligne1()
        {
            if (historiqueEgal != "") return historiqueEgal;
            if (operateur == "") return termeUnaire;

            string s = Formater(operande) + " " + operateur;
            return (termeUnaire != "") ? s + " " + termeUnaire : s;
        }

        /// <summary>Convertit le texte affiché en nombre (jamais d'exception).</summary>
        private double Valeur()
        {
            double v;
            string t = saisie.TrimEnd(',');
            return double.TryParse(t, NumberStyles.Float, Fr, out v) ? v : 0;
        }

        /// <summary>Convertit un nombre en texte (15 chiffres significatifs, évite 0,1+0,2 = 0,30000000000000004).</summary>
        private static string Formater(double v)
        {
            if (v == 0) return "0";                 // évite "-0"
            return v.ToString("G15", Fr);
        }

        private int NombreDeChiffres()
        {
            return saisie.Count(char.IsDigit);
        }

        private string ArgumentCourant()
        {
            return (termeUnaire != "") ? termeUnaire : Formater(Valeur());
        }

        /// <summary>Prépare la saisie d'un nouveau nombre (remplace l'affichage courant).</summary>
        private void DebutNouvelleSaisie()
        {
            nouvelleSaisie = false;
            operandePret = false;
            termeUnaire = "";
            historiqueEgal = "";
        }

        private void AjouterChiffre(string chiffre)
        {
            if (nouvelleSaisie)
            {
                DebutNouvelleSaisie();
                saisie = chiffre;
            }
            else if (saisie == "0")
            {
                saisie = chiffre;                   // évite les zéros inutiles : "007"
            }
            else if (NombreDeChiffres() < MaxChiffres)
            {
                saisie += chiffre;
            }
            Afficher();
        }

        private void ChoisirOperateur(string op)
        {
            if (operateur != "" && (!nouvelleSaisie || operandePret))
            {
                // Opérations en chaîne : 5 + 3 ×  => on calcule d'abord 5 + 3 (sur le serveur)
                double r;
                if (!Calculer(operande, operateur, Valeur(), out r))
                    return;
                operande = r;
            }
            else if (operateur == "")
            {
                operande = Valeur();                // premier opérande (ou résultat précédent)
            }
            // sinon : "5 + ×" => seul l'opérateur change, l'opérande reste 5

            operateur = op;
            saisie = Formater(operande);
            termeUnaire = "";
            historiqueEgal = "";
            nouvelleSaisie = true;
            operandePret = false;
            Afficher();
        }

        /// <summary>Calcule a op b en appelant la méthode DISTANTE du serveur.</summary>
        private bool Calculer(double a, string op, double b, out double resultat)
        {
            resultat = 0;
            switch (op)
            {
                case "+": return Distant(() => remoteOperation.Addition(a, b), out resultat);
                case "-": return Distant(() => remoteOperation.Soustraction(a, b), out resultat);
                case "×": return Distant(() => remoteOperation.Multiplication(a, b), out resultat);
                case "÷":
                    if (b == 0)
                    {
                        Erreur("Division impossible !");
                        return false;
                    }
                    return Distant(() => remoteOperation.Division(a, b), out resultat);
            }
            return false;
        }

        /// <summary>Exécute un appel distant en gérant les erreurs réseau.</summary>
        private bool Distant(Func<double> appel, out double resultat)
        {
            resultat = 0;
            if (remoteOperation == null)
            {
                Erreur("Pas de connexion au serveur.");
                return false;
            }
            try
            {
                resultat = appel();
            }
            catch (Exception ex)
            {
                Erreur("Impossible de joindre le serveur (" + UrlServeur + ").\nVérifiez qu'il est lancé.\n\n" + ex.Message);
                return false;
            }
            if (double.IsInfinity(resultat) || double.IsNaN(resultat))
            {
                Erreur("Dépassement de capacité !");
                return false;
            }
            return true;
        }

        /// <summary>Applique une opération unaire directement sur le nombre affiché.</summary>
        private void AppliquerUnaire(string nouveauTerme, double resultat)
        {
            if (double.IsInfinity(resultat) || double.IsNaN(resultat))
            {
                Erreur("Dépassement de capacité !");
                return;
            }
            termeUnaire = nouveauTerme;
            historiqueEgal = "";
            saisie = Formater(resultat);
            nouvelleSaisie = true;
            operandePret = true;
            Afficher();
        }

        /// <summary>Après MS / M+ / M- : un nombre tapé devient un opérande prêt, et le prochain chiffre recommence une saisie.</summary>
        private void FinSaisieMemoire()
        {
            if (!nouvelleSaisie)
            {
                nouvelleSaisie = true;
                operandePret = true;
            }
            Afficher();
        }

        private void ToutEffacer()
        {
            saisie = "0";
            operande = 0;
            operateur = "";
            termeUnaire = "";
            historiqueEgal = "";
            nouvelleSaisie = true;
            operandePret = false;
            Afficher();
        }

        private void Erreur(string message)
        {
            MessageBox.Show(message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ToutEffacer();
        }
    }
}