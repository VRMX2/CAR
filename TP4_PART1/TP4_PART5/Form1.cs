using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace TP4_PART5
{

    internal static class UI
    {
        public static readonly Color Primary = Color.FromArgb(79, 70, 229);
        public static readonly Color PrimaryDk = Color.FromArgb(67, 56, 202);
        public static readonly Color Success = Color.FromArgb(22, 163, 74);
        public static readonly Color SuccessDk = Color.FromArgb(21, 128, 61);
        public static readonly Color Warning = Color.FromArgb(217, 119, 6);
        public static readonly Color WarningDk = Color.FromArgb(180, 83, 9);
        public static readonly Color Danger = Color.FromArgb(225, 29, 72);
        public static readonly Color DangerDk = Color.FromArgb(190, 18, 60);
        public static readonly Color Gray = Color.FromArgb(100, 116, 139);
        public static readonly Color GrayDk = Color.FromArgb(71, 85, 105);
        public static readonly Color Dark = Color.FromArgb(15, 23, 42);
        public static readonly Color Back = Color.FromArgb(243, 244, 250);
        public static readonly Color RowAlt = Color.FromArgb(248, 250, 252);
        public static readonly Color Border = Color.FromArgb(226, 232, 240);
        public static readonly Color BorderDk = Color.FromArgb(203, 213, 225);

        public static GraphicsPath Round(Rectangle r, int radius)
        {
            GraphicsPath p = new GraphicsPath();
            int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            if (d <= 0) { p.AddRectangle(r); return p; }
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }

    // White rounded card with a soft shadow and an optional colored top line
    internal class Card : Panel
    {
        public Color Accent = Color.Empty;

        public Card()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = UI.Back;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Width < 30 || Height < 30) return;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath sp = UI.Round(new Rectangle(1, 3, Width - 3, Height - 5), 14))
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(16, 15, 23, 42)))
                g.FillPath(sb, sp);

            using (GraphicsPath p = UI.Round(new Rectangle(0, 0, Width - 2, Height - 4), 14))
            {
                g.FillPath(Brushes.White, p);
                if (Accent != Color.Empty)
                {
                    g.SetClip(p);
                    using (SolidBrush ab = new SolidBrush(Accent))
                        g.FillRectangle(ab, 0, 0, Width, 4);
                    g.ResetClip();
                }
                using (Pen pen = new Pen(UI.Border))
                    g.DrawPath(pen, p);
            }
        }
    }

    // Rounded button with hover / pressed states
    internal class RoundButton : Button
    {
        public Color Fill = UI.Primary;
        public Color Hover = UI.PrimaryDk;
        public Color Canvas = Color.White;
        public bool Ghost;
        private bool hot, down;

        public RoundButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI Semibold", 10F);
        }

        protected override void OnMouseEnter(EventArgs e) { hot = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hot = false; down = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { down = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { down = false; Invalidate(); base.OnMouseUp(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Canvas);

            Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);
            Color fill, text;
            if (Ghost)
            {
                fill = hot ? Color.FromArgb(238, 242, 255) : Color.White;
                text = hot ? UI.Primary : UI.GrayDk;
            }
            else
            {
                fill = down ? UI.Dark : (hot ? Hover : Fill);
                text = Color.White;
            }

            using (GraphicsPath p = UI.Round(r, 10))
            {
                using (SolidBrush b = new SolidBrush(fill)) g.FillPath(b, p);
                if (Ghost)
                    using (Pen pen = new Pen(hot ? UI.Primary : UI.BorderDk, 1.5f)) g.DrawPath(pen, p);
            }

            TextRenderer.DrawText(g, Text, Font, r, text,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
        }
    }

    // Text box inside a rounded border that turns indigo on focus
    internal class RoundedField : Panel
    {
        public readonly TextBox Input;
        private bool focused;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        public RoundedField(Font font, string placeholder)
        {
            DoubleBuffered = true;
            Height = 42;
            BackColor = Color.White;

            Input = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = font,
                BackColor = Color.White,
                ForeColor = UI.Dark
            };
            Controls.Add(Input);

            Input.GotFocus += (s, e) => { focused = true; Invalidate(); };
            Input.LostFocus += (s, e) => { focused = false; Invalidate(); };
            Input.HandleCreated += (s, e) => SendMessage(Input.Handle, 0x1501, 1, placeholder);
            Click += (s, e) => Input.Focus();
            LayoutInput();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutInput();
        }

        private void LayoutInput()
        {
            if (Input == null) return;
            Input.Width = Math.Max(10, Width - 28);
            Input.Location = new Point(14, Math.Max(0, (Height - Input.Height) / 2));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath p = UI.Round(new Rectangle(1, 1, Width - 3, Height - 3), 10))
            using (Pen pen = new Pen(focused ? UI.Primary : UI.BorderDk, focused ? 2f : 1.5f))
                g.DrawPath(pen, p);
        }
    }

    // =========================================================
    //                         FORM
    // =========================================================
    public partial class Form1 : Form
    {
        // Oracle (serveur de l'enseignant).
        // Easy Connect : host:port/service_name  -> remplacer XE par le vrai nom de service si besoin
        // NB : si vous utilisez votre propre base locale : "Data Source=localhost:1521/XEPDB1"
        private const string mydb =
     "User Id=oracle;Password=manager;Data Source=10.219.110.125:1521/XEPDB1;Connection Timeout=10;";
        private OracleConnection con;
        private bool connected = false;

        // ---------- Controls (built in code) ----------
        private ComboBox cmbMatricule;
        private RoundedField fNom, fPrenom, fFiltre;
        private RoundButton btnRechercher, btnAjouter, btnModifier, btnSupprimer, btnVider,
                            btnAfficher, btnExport, btnFermer;
        private DataGridView grid;
        private Label lblHint, lblStatus;
        private Panel header;
        private string countText = "";

        // Toast notification
        private Panel toast, toastStrip;
        private Label toastLabel;
        private Timer toastTimer;

        private ToolTip tip = new ToolTip();

        // NOTE: the ORA-01882 fix (ORA_SDTZ) is now in Program.Main(), which runs before
        // anything else. No static constructor is needed here any more.

        public Form1()
        {
            InitializeComponent();

            // Remove anything drawn in the Designer: the UI is rebuilt in code
            while (Controls.Count > 0)
            {
                Control c = Controls[0];
                Controls.Remove(c);
                c.Dispose();
            }

            DoubleBuffered = true;
            BuildUI();
        }

        // =====================================================
        //                      DESIGN
        // =====================================================
        private Label L(string text, float size, bool bold, Color color, Point loc)
        {
            return new Label
            {
                Text = text,
                Font = new Font(bold ? "Segoe UI Semibold" : "Segoe UI", size),
                ForeColor = color,
                BackColor = Color.White,
                AutoSize = true,
                Location = loc
            };
        }

        private RoundButton B(string text, Color fill, Color hover, Rectangle bounds, string tooltip, bool ghost = false)
        {
            RoundButton b = new RoundButton
            {
                Text = text,
                Fill = fill,
                Hover = hover,
                Ghost = ghost,
                Bounds = bounds
            };
            tip.SetToolTip(b, tooltip);
            return b;
        }

        private void BuildUI()
        {
            Text = "Gestion des étudiants";
            BackColor = UI.Back;
            Font = new Font("Segoe UI", 10F);
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1000, 780);
            MinimumSize = new Size(920, 700);
            KeyPreview = true;

            // ---------- Header (gradient) ----------
            header = new Panel { Dock = DockStyle.Top, Height = 92 };
            typeof(Panel).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                         .SetValue(header, true, null);
            header.Paint += Header_Paint;
            header.Resize += (s, e) => header.Invalidate();
            Controls.Add(header);

            // ---------- Card 1 : manage a student ----------
            Card cardEdit = new Card
            {
                Location = new Point(24, 114),
                Size = new Size(ClientSize.Width - 48, 244),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Accent = UI.Primary
            };
            Controls.Add(cardEdit);

            cardEdit.Controls.Add(L("Gérer un étudiant", 13F, true, UI.Dark, new Point(24, 20)));
            cardEdit.Controls.Add(L("Choisissez ou saisissez un matricule, puis utilisez les boutons ci-dessous.",
                                    9.5F, false, UI.Gray, new Point(25, 50)));

            cardEdit.Controls.Add(L("MATRICULE", 8.5F, true, UI.Gray, new Point(24, 88)));
            cardEdit.Controls.Add(L("NOM", 8.5F, true, UI.Gray, new Point(370, 88)));
            cardEdit.Controls.Add(L("PRÉNOM", 8.5F, true, UI.Gray, new Point(640, 88)));

            // Editable combo: pick an existing matricule or type a new one
            cmbMatricule = new ComboBox
            {
                Font = new Font("Segoe UI", 12F),
                DropDownStyle = ComboBoxStyle.DropDown,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                Location = new Point(24, 115),
                Width = 170
            };
            cmbMatricule.DropDown += (s, e) => LoadCombo();
            cardEdit.Controls.Add(cmbMatricule);

            btnRechercher = B("🔍  Rechercher", UI.Primary, UI.PrimaryDk,
                              new Rectangle(206, 110, 144, 40), "Chercher le matricule choisi");
            btnRechercher.Click += Rechercher_Click;
            cardEdit.Controls.Add(btnRechercher);

            fNom = new RoundedField(new Font("Segoe UI", 12F), "Nom…")
            { Location = new Point(370, 109), Width = 252 };
            cardEdit.Controls.Add(fNom);

            fPrenom = new RoundedField(new Font("Segoe UI", 12F), "Prénom…")
            { Location = new Point(640, 109), Width = 252 };
            cardEdit.Controls.Add(fPrenom);

            cardEdit.Controls.Add(new Panel
            {
                BackColor = UI.Border,
                Location = new Point(24, 170),
                Size = new Size(cardEdit.Width - 52, 1),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            });

            btnAjouter = B("➕  Ajouter", UI.Success, UI.SuccessDk,
                           new Rectangle(24, 186, 150, 42), "Ajouter un nouvel étudiant");
            btnAjouter.Click += Ajouter_Click;
            cardEdit.Controls.Add(btnAjouter);

            btnModifier = B("✏  Modifier", UI.Warning, UI.WarningDk,
                            new Rectangle(188, 186, 150, 42), "Modifier le nom et le prénom");
            btnModifier.Click += Modifier_Click;
            cardEdit.Controls.Add(btnModifier);

            btnSupprimer = B("🗑  Supprimer", UI.Danger, UI.DangerDk,
                             new Rectangle(352, 186, 150, 42), "Supprimer l'étudiant");
            btnSupprimer.Click += Supprimer_Click;
            cardEdit.Controls.Add(btnSupprimer);

            btnVider = B("✖  Vider", UI.Gray, UI.GrayDk,
                         new Rectangle(516, 186, 130, 42), "Vider les champs (Ctrl+N)", true);
            btnVider.Click += (s, e) => { ClearFields(); ShowToast("Champs vidés.", UI.Gray); };
            cardEdit.Controls.Add(btnVider);

            // ---------- Card 2 : table ----------
            Card cardList = new Card
            {
                Location = new Point(24, 378),
                Size = new Size(ClientSize.Width - 48, ClientSize.Height - 378 - 24),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Accent = UI.Success
            };
            Controls.Add(cardList);
            int W = cardList.Width, H = cardList.Height;

            cardList.Controls.Add(L("Table Etudiant", 13F, true, UI.Dark, new Point(24, 24)));

            btnAfficher = B("⟳  Afficher", UI.Primary, UI.PrimaryDk,
                            new Rectangle(W - 24 - 140, 16, 140, 42), "Charger la table (F5)");
            btnAfficher.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAfficher.Click += Afficher_Click;
            cardList.Controls.Add(btnAfficher);

            btnExport = B("⬇  Exporter CSV", UI.Gray, UI.GrayDk,
                          new Rectangle(W - 24 - 140 - 12 - 160, 16, 160, 42),
                          "Exporter le tableau affiché en CSV (Ctrl+E)", true);
            btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExport.Click += Export_Click;
            cardList.Controls.Add(btnExport);

            fFiltre = new RoundedField(new Font("Segoe UI", 11F), "🔍  Filtrer…")
            {
                Location = new Point(W - 24 - 140 - 12 - 160 - 12 - 230, 16),
                Width = 230,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            fFiltre.Input.TextChanged += (s, e) => ApplyFilter();
            cardList.Controls.Add(fFiltre);

            cardList.Controls.Add(new Panel
            {
                BackColor = UI.Border,
                Location = new Point(24, 70),
                Size = new Size(W - 52, 1),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            });

            lblHint = new Label
            {
                Text = "Cliquez sur « Afficher » pour charger la table Etudiant.",
                ForeColor = UI.Gray,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 11F),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(24, 84),
                Size = new Size(W - 52, H - 84 - 84),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            cardList.Controls.Add(lblHint);

            grid = new DataGridView
            {
                Location = new Point(24, 84),
                Size = new Size(W - 52, H - 84 - 84),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Visible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                GridColor = UI.Border,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeight = 42,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            };
            typeof(DataGridView).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                                .SetValue(grid, true, null);
            grid.RowTemplate.Height = 38;
            grid.ColumnHeadersDefaultCellStyle.BackColor = UI.Dark;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = UI.Dark;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 0, 0, 0);
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = UI.Dark;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 231, 255);
            grid.DefaultCellStyle.SelectionForeColor = UI.Dark;
            grid.DefaultCellStyle.Padding = new Padding(12, 0, 0, 0);
            grid.AlternatingRowsDefaultCellStyle.BackColor = UI.RowAlt;
            grid.CellClick += Grid_CellClick;
            cardList.Controls.Add(grid);

            lblStatus = new Label
            {
                Text = "Prêt.",
                ForeColor = UI.Gray,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                AutoSize = true,
                Location = new Point(24, H - 58),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            cardList.Controls.Add(lblStatus);

            btnFermer = B("Fermer", UI.Gray, UI.GrayDk,
                          new Rectangle(W - 24 - 130, H - 66, 130, 40), "Fermer l'application");
            btnFermer.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnFermer.Click += Fermer_Click;
            cardList.Controls.Add(btnFermer);

            BuildToast();
        }

        private void Header_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (header.Width <= 0 || header.Height <= 0) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            using (LinearGradientBrush br = new LinearGradientBrush(
                header.ClientRectangle, Color.FromArgb(15, 23, 42),
                Color.FromArgb(67, 56, 202), LinearGradientMode.Horizontal))
                g.FillRectangle(br, header.ClientRectangle);

            using (Font f1 = new Font("Segoe UI Semibold", 19F))
                g.DrawString("Gestion des étudiants", f1, Brushes.White, 28, 14);
            using (Font f2 = new Font("Segoe UI", 10F))
            using (SolidBrush b2 = new SolidBrush(Color.FromArgb(199, 210, 254)))
                g.DrawString("ADO.NET + Oracle  •  Ajouter, rechercher, modifier, supprimer et exporter", f2, b2, 30, 56);

            float rightEdge = header.Width - 28;

            if (countText.Length > 0)
            {
                using (Font f3 = new Font("Segoe UI Semibold", 10F))
                {
                    SizeF sz = g.MeasureString(countText, f3);
                    float w = sz.Width + 28, h = 34;
                    float x = header.Width - w - 28, y = (header.Height - h) / 2;
                    using (GraphicsPath p = new GraphicsPath())
                    {
                        p.AddArc(x, y, h, h, 90, 180);
                        p.AddArc(x + w - h, y, h, h, 270, 180);
                        p.CloseFigure();
                        using (SolidBrush pb = new SolidBrush(Color.FromArgb(50, 255, 255, 255)))
                            g.FillPath(pb, p);
                    }
                    g.DrawString(countText, f3, Brushes.White, x + 14, y + (h - sz.Height) / 2);
                    rightEdge = x - 22;
                }
            }

            string conn = connected ? "Connecté" : "Hors ligne";
            using (Font f4 = new Font("Segoe UI", 9.5F))
            using (SolidBrush dot = new SolidBrush(connected ? Color.FromArgb(74, 222, 128) : Color.FromArgb(251, 113, 133)))
            {
                SizeF cs = g.MeasureString(conn, f4);
                float tx = rightEdge - cs.Width;
                g.FillEllipse(dot, tx - 16, header.Height / 2f - 5, 10, 10);
                g.DrawString(conn, f4, Brushes.White, tx, (header.Height - cs.Height) / 2);
            }
        }

        // ---------- Toast ----------
        private void BuildToast()
        {
            toast = new Panel
            {
                Size = new Size(380, 54),
                BackColor = UI.Dark,
                Visible = false,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            toast.Location = new Point(ClientSize.Width - toast.Width - 32, ClientSize.Height - toast.Height - 32);

            toastLabel = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 10, 0),
                AutoEllipsis = true
            };
            toastStrip = new Panel { Dock = DockStyle.Left, Width = 6 };
            toast.Controls.Add(toastLabel);   // Fill first
            toast.Controls.Add(toastStrip);   // then Left

            using (GraphicsPath p = UI.Round(new Rectangle(0, 0, toast.Width, toast.Height), 12))
                toast.Region = new Region(p);

            Controls.Add(toast);

            toastTimer = new Timer { Interval = 3200 };
            toastTimer.Tick += (s, e) => { toastTimer.Stop(); toast.Visible = false; };
        }

        private void ShowToast(string message, Color color)
        {
            toastLabel.Text = message;
            toastStrip.BackColor = color;
            toast.Visible = true;
            toast.BringToFront();
            toastTimer.Stop();
            toastTimer.Start();
        }

        private void SetStatus(string text, Color color)
        {
            lblStatus.Text = text;
            lblStatus.ForeColor = color;
        }

        private void ShowGrid(DataTable dt, string count)
        {
            grid.DataSource = dt.DefaultView;

            // Oracle returns column names in UPPERCASE (MATRICULE, NOM, PRENOM) -> "Matricule", "Nom", "Prenom"
            foreach (DataGridViewColumn col in grid.Columns)
            {
                string h = col.HeaderText;
                col.HeaderText = char.ToUpper(h[0]) + h.Substring(1).ToLower();
            }
            if (grid.Columns.Count > 0) grid.Columns[0].FillWeight = 40;

            lblHint.Visible = false;
            grid.Visible = true;

            countText = count;
            header.Invalidate();
            ApplyFilter();
        }

        // =====================================================
        //                       LOGIC
        // =====================================================

        // Form1_Load : ouvre la connexion au chargement
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                EnsureConnection();
                connected = true;
                header.Invalidate();
                LoadCombo();
                SetStatus("Connecté à la base de données Oracle.", UI.Success);
            }
            catch (OracleException ex)
            {
                connected = false;
                header.Invalidate();
                SetStatus("Connexion impossible.", UI.Danger);
                MessageBox.Show("Erreur de connexion : " + ex.Message, "Erreur",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureConnection()
        {
            if (con == null) con = new OracleConnection(mydb);
            if (con.State != ConnectionState.Open) con.Open();
        }

        // comboBox1_Click : remplit la liste des matricules
        private bool loadingCombo = false;
        private void LoadCombo()
        {
            if (loadingCombo) return;
            loadingCombo = true;
            try
            {
                EnsureConnection();

                string previous = cmbMatricule.Text;

                OracleDataAdapter da = new OracleDataAdapter(
                    "SELECT matricule FROM Etudiant ORDER BY matricule", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbMatricule.DataSource = dt;
                cmbMatricule.DisplayMember = dt.Columns[0].ColumnName;

                int idx = cmbMatricule.FindStringExact(previous);
                cmbMatricule.SelectedIndex = idx;               // -1 = nothing selected
                if (idx < 0) cmbMatricule.Text = previous;      // keep a typed (new) matricule
            }
            catch (OracleException ex)
            {
                SetStatus("Erreur lors du chargement des matricules.", UI.Danger);
                MessageBox.Show("Erreur : " + ex.Message, "Erreur de base de données",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loadingCombo = false;
            }
        }

        // Afficher_Click : affiche toute la table
        private void Afficher_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void LoadTable()
        {
            try
            {
                EnsureConnection();

                OracleDataAdapter da = new OracleDataAdapter(
                    "SELECT * FROM Etudiant ORDER BY matricule", con);
                DataTable dt = new DataTable("Etudiant");
                da.Fill(dt);

                connected = true;
                ShowGrid(dt, dt.Rows.Count + " étudiant(s)");
            }
            catch (OracleException ex)
            {
                connected = false;
                header.Invalidate();
                SetStatus("Erreur lors du chargement.", UI.Danger);
                MessageBox.Show("Erreur : " + ex.Message, "Erreur de base de données",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- Live filter (matricule, nom, prenom) ----------
        private static string EscapeLike(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in s)
            {
                switch (ch)
                {
                    case '\'': sb.Append("''"); break;
                    case '[': sb.Append("[[]"); break;
                    case ']': sb.Append("[]]"); break;
                    case '*': sb.Append("[*]"); break;
                    case '%': sb.Append("[%]"); break;
                    default: sb.Append(ch); break;
                }
            }
            return sb.ToString();
        }

        private void ApplyFilter()
        {
            DataView dv = grid.DataSource as DataView;
            if (dv == null) return;

            string t = fFiltre.Input.Text.Trim();
            if (t.Length == 0)
            {
                dv.RowFilter = "";
                SetStatus(dv.Count + " étudiant(s) affiché(s).", UI.Gray);
                return;
            }

            DataColumnCollection cols = dv.Table.Columns;
            string like = EscapeLike(t);
            dv.RowFilter = string.Format(
                "Convert([{0}], 'System.String') LIKE '%{3}%' OR [{1}] LIKE '%{3}%' OR [{2}] LIKE '%{3}%'",
                cols[0].ColumnName, cols[1].ColumnName, cols[2].ColumnName, like);
            SetStatus(dv.Count + " résultat(s) pour « " + t + " ».", UI.Gray);
        }

        // ---------- Validation helpers ----------
        private bool CheckMatricule(out int matricule)
        {
            if (int.TryParse(cmbMatricule.Text.Trim(), out matricule)) return true;
            MessageBox.Show("Veuillez choisir ou saisir un matricule valide (nombre).", "Matricule invalide",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cmbMatricule.Focus();
            return false;
        }

        private bool CheckNames()
        {
            if (fNom.Input.Text.Trim().Length > 0 && fPrenom.Input.Text.Trim().Length > 0) return true;
            MessageBox.Show("Le nom et le prénom ne peuvent pas être vides.", "Champs manquants",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            (fNom.Input.Text.Trim().Length == 0 ? fNom.Input : fPrenom.Input).Focus();
            return false;
        }

        private void ClearFields()
        {
            cmbMatricule.SelectedIndex = -1;
            cmbMatricule.Text = "";
            fNom.Input.Clear();
            fPrenom.Input.Clear();
            cmbMatricule.Focus();
        }

        // Rechercher_Click : recherche un enregistrement
        private void Rechercher_Click(object sender, EventArgs e)
        {
            int matricule;
            if (!CheckMatricule(out matricule)) return;

            try
            {
                EnsureConnection();

                OracleDataAdapter da = new OracleDataAdapter(
                    "SELECT matricule, nom, prenom FROM Etudiant WHERE matricule = :matricule", con);
                da.SelectCommand.BindByName = true;
                da.SelectCommand.Parameters.Add("matricule", OracleDbType.Int32).Value = matricule;

                DataTable dt = new DataTable("Etudiant");
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    fNom.Input.Clear();
                    fPrenom.Input.Clear();
                    SetStatus("Matricule " + matricule + " introuvable.", UI.Danger);
                    ShowToast("Matricule " + matricule + " introuvable.", UI.Danger);
                    return;
                }

                fNom.Input.Text = Convert.ToString(dt.Rows[0][1]);
                fPrenom.Input.Text = Convert.ToString(dt.Rows[0][2]);

                fFiltre.Input.Text = "";
                ShowGrid(dt, "1 résultat");
                SetStatus("Étudiant n° " + matricule + " trouvé.", UI.Success);
                ShowToast("Étudiant n° " + matricule + " trouvé.", UI.Success);
                fNom.Input.Focus();
                fNom.Input.SelectAll();
            }
            catch (OracleException ex)
            {
                SetStatus("Erreur lors de la recherche.", UI.Danger);
                MessageBox.Show("Erreur : " + ex.Message, "Erreur de base de données",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Ajouter_Click : ajoute un nouvel étudiant
        private void Ajouter_Click(object sender, EventArgs e)
        {
            int matricule;
            if (!CheckMatricule(out matricule)) return;
            if (!CheckNames()) return;

            try
            {
                EnsureConnection();

                string SQL = "INSERT INTO Etudiant (matricule, nom, prenom) VALUES (:matricule, :nom, :prenom)";
                OracleCommand cmd = new OracleCommand(SQL, con);
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.Parameters.Add("matricule", OracleDbType.Int32).Value = matricule;
                cmd.Parameters.Add("nom", OracleDbType.Varchar2).Value = fNom.Input.Text.Trim();
                cmd.Parameters.Add("prenom", OracleDbType.Varchar2).Value = fPrenom.Input.Text.Trim();

                if (cmd.ExecuteNonQuery() == 1)   // auto-commit (pas de transaction ouverte)
                {
                    ShowToast("Étudiant n° " + matricule + " ajouté avec succès.", UI.Success);
                    LoadCombo();
                    LoadTable();
                    SetStatus("Étudiant n° " + matricule + " ajouté.", UI.Success);
                }
            }
            catch (OracleException ex) when (ex.Number == 1)   // ORA-00001 : unique constraint violated
            {
                SetStatus("Le matricule " + matricule + " existe déjà.", UI.Danger);
                MessageBox.Show("Ce matricule existe déjà. Choisissez un autre numéro.", "Doublon",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMatricule.Focus();
            }
            catch (OracleException ex)
            {
                SetStatus("Erreur lors de l'ajout.", UI.Danger);
                MessageBox.Show("Erreur : " + ex.Message, "Erreur de base de données",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Modifier_Click : modifie un enregistrement
        private void Modifier_Click(object sender, EventArgs e)
        {
            int matricule;
            if (!CheckMatricule(out matricule)) return;
            if (!CheckNames()) return;

            try
            {
                EnsureConnection();

                string SQL = "UPDATE Etudiant SET nom = :nom, prenom = :prenom WHERE matricule = :matricule";
                OracleCommand cmd = new OracleCommand(SQL, con);
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.Parameters.Add("nom", OracleDbType.Varchar2).Value = fNom.Input.Text.Trim();
                cmd.Parameters.Add("prenom", OracleDbType.Varchar2).Value = fPrenom.Input.Text.Trim();
                cmd.Parameters.Add("matricule", OracleDbType.Int32).Value = matricule;

                // Oracle counts matched rows (even if values are identical), unlike MySQL
                if (cmd.ExecuteNonQuery() == 1)
                {
                    LoadTable();
                    SetStatus("Étudiant n° " + matricule + " modifié.", UI.Success);
                    ShowToast("Étudiant n° " + matricule + " modifié avec succès.", UI.Success);
                }
                else
                {
                    SetStatus("Matricule " + matricule + " introuvable.", UI.Warning);
                    ShowToast("Matricule " + matricule + " introuvable.", UI.Warning);
                }
            }
            catch (OracleException ex)
            {
                SetStatus("Erreur lors de la modification.", UI.Danger);
                MessageBox.Show("Erreur : " + ex.Message, "Erreur de base de données",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Supprimer_Click : supprime un enregistrement
        private void Supprimer_Click(object sender, EventArgs e)
        {
            int matricule;
            if (!CheckMatricule(out matricule)) return;

            DialogResult confirm = MessageBox.Show(
                "Voulez-vous vraiment supprimer l'étudiant n° " + matricule + " ?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                EnsureConnection();

                string SQL = "DELETE FROM Etudiant WHERE matricule = :matricule";
                OracleCommand cmd = new OracleCommand(SQL, con);
                cmd.CommandType = CommandType.Text;
                cmd.BindByName = true;
                cmd.Parameters.Add("matricule", OracleDbType.Int32).Value = matricule;

                if (cmd.ExecuteNonQuery() == 1)
                {
                    ClearFields();
                    LoadCombo();
                    LoadTable();
                    SetStatus("Étudiant n° " + matricule + " supprimé.", UI.Success);
                    ShowToast("Étudiant n° " + matricule + " supprimé.", UI.Danger);
                }
                else
                {
                    SetStatus("Matricule " + matricule + " introuvable.", UI.Danger);
                    ShowToast("Matricule " + matricule + " introuvable.", UI.Danger);
                }
            }
            catch (OracleException ex)
            {
                SetStatus("Erreur lors de la suppression.", UI.Danger);
                MessageBox.Show("Erreur : " + ex.Message, "Erreur de base de données",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- Export CSV ----------
        private static string Csv(string s)
        {
            if (s.IndexOfAny(new[] { ';', '"', '\n', '\r' }) >= 0)
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }

        private void Export_Click(object sender, EventArgs e)
        {
            DataView dv = grid.DataSource as DataView;
            if (dv == null || dv.Count == 0)
            {
                MessageBox.Show("Il n'y a rien à exporter. Cliquez d'abord sur « Afficher ».",
                                "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Exporter la table Etudiant";
                sfd.Filter = "Fichier CSV (*.csv)|*.csv";
                sfd.FileName = "etudiants.csv";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    StringBuilder sb = new StringBuilder();
                    DataColumnCollection cols = dv.Table.Columns;

                    for (int i = 0; i < cols.Count; i++)
                        sb.Append(i > 0 ? ";" : "").Append(Csv(cols[i].ColumnName));
                    sb.AppendLine();

                    foreach (DataRowView rv in dv)
                    {
                        for (int i = 0; i < cols.Count; i++)
                            sb.Append(i > 0 ? ";" : "").Append(Csv(Convert.ToString(rv[i])));
                        sb.AppendLine();
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));
                    SetStatus("Exporté : " + sfd.FileName, UI.Success);
                    ShowToast(dv.Count + " ligne(s) exportée(s) en CSV.", UI.Success);
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Impossible d'écrire le fichier : " + ex.Message, "Erreur",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show("Accès refusé : " + ex.Message, "Erreur",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Clicking a row fills the combo and the text boxes
        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = grid.Rows[e.RowIndex];

            string mat = Convert.ToString(row.Cells[0].Value);
            int idx = cmbMatricule.FindStringExact(mat);
            if (idx >= 0) cmbMatricule.SelectedIndex = idx;
            else cmbMatricule.Text = mat;

            fNom.Input.Text = Convert.ToString(row.Cells[1].Value);
            fPrenom.Input.Text = Convert.ToString(row.Cells[2].Value);
        }

        // Keyboard shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F5:
                    Afficher_Click(this, EventArgs.Empty); return true;
                case Keys.Control | Keys.N:
                    ClearFields(); return true;
                case Keys.Control | Keys.E:
                    Export_Click(this, EventArgs.Empty); return true;
                case Keys.Control | Keys.F:
                    fFiltre.Input.Focus(); return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Fermer_Click : ferme la connexion et la fenêtre
        private void Fermer_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();
            base.OnFormClosed(e);
        }
    }
}