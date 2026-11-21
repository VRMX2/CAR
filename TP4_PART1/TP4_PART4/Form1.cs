using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TP4_PART4
{
    public partial class Form1 : Form
    {
        private const string mydb =
            "Server=localhost;Port=3306;Database=db_tp4;Uid=root;Pwd=lahcengrissi123;";

        private MySqlConnection con;

        // ---------- Palette ----------
        private static readonly Color Primary = Color.FromArgb(79, 70, 229);
        private static readonly Color PrimaryDk = Color.FromArgb(67, 56, 202);
        private static readonly Color Danger = Color.FromArgb(225, 29, 72);
        private static readonly Color DangerDk = Color.FromArgb(190, 18, 60);
        private static readonly Color Gray = Color.FromArgb(100, 116, 139);
        private static readonly Color GrayDk = Color.FromArgb(71, 85, 105);
        private static readonly Color Dark = Color.FromArgb(15, 23, 42);
        private static readonly Color Back = Color.FromArgb(243, 244, 250);
        private static readonly Color RowAlt = Color.FromArgb(248, 250, 252);
        private static readonly Color Border = Color.FromArgb(226, 232, 240);

        // ---------- Controls (built in code) ----------
        private TextBox txtMatricule, txtSearch;
        private Button btnSupprimer, btnAfficher, btnFermer;
        private DataGridView grid;
        private Label lblHint, lblStatus;
        private Panel header;
        private string countText = "";

        // Placeholder text for TextBox
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        // Panel without flicker
        private class BufferedPanel : Panel
        {
            public BufferedPanel() { DoubleBuffered = true; ResizeRedraw = true; }
        }

        public Form1()
        {
            InitializeComponent();

            // Remove the controls drawn in the Designer: the UI is rebuilt in code
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
        private static void MakeRounded(Control c, int radius)
        {
            Action apply = () =>
            {
                if (c.Width <= 0 || c.Height <= 0) return;
                using (GraphicsPath p = new GraphicsPath())
                {
                    int d = radius * 2;
                    Rectangle r = new Rectangle(0, 0, c.Width, c.Height);
                    p.AddArc(r.X, r.Y, d, d, 180, 90);
                    p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                    p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                    p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                    p.CloseFigure();
                    c.Region = new Region(p);
                }
            };
            apply();
            c.SizeChanged += (s, e) => apply();
        }

        private Button MakeButton(string text, Color back, Color hover, Size size, Point loc)
        {
            Button b = new Button
            {
                Text = text,
                Size = size,
                Location = loc,
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = hover;
            b.FlatAppearance.MouseDownBackColor = Dark;
            MakeRounded(b, 10);
            return b;
        }

        private Panel MakeCard(Point loc, Size size, AnchorStyles anchor)
        {
            Panel card = new BufferedPanel
            {
                BackColor = Color.White,
                Location = loc,
                Size = size,
                Anchor = anchor
            };
            card.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Border))
                    e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
            };
            return card;
        }

        private void BuildUI()
        {
            Text = "Suppression d'un étudiant";
            BackColor = Back;
            Font = new Font("Segoe UI", 10F);
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(920, 660);
            MinimumSize = new Size(760, 540);

            // ---------- Header with gradient ----------
            header = new BufferedPanel { Dock = DockStyle.Top, Height = 88 };
            header.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                if (header.Width <= 0 || header.Height <= 0) return;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                using (LinearGradientBrush br = new LinearGradientBrush(
                    header.ClientRectangle, Color.FromArgb(15, 23, 42),
                    Color.FromArgb(67, 56, 202), LinearGradientMode.Horizontal))
                    g.FillRectangle(br, header.ClientRectangle);

                using (Font f1 = new Font("Segoe UI Semibold", 18F))
                    g.DrawString("Gestion des étudiants", f1, Brushes.White, 28, 14);
                using (Font f2 = new Font("Segoe UI", 10F))
                using (SolidBrush b2 = new SolidBrush(Color.FromArgb(199, 210, 254)))
                    g.DrawString("ADO.NET  •  Suppression d'un enregistrement", f2, b2, 30, 53);

                // Count "pill" on the right
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
                    }
                }
            };
            Controls.Add(header);

            // ---------- Delete card ----------
            Panel cardDel = MakeCard(new Point(24, 112),
                new Size(ClientSize.Width - 48, 120),
                AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            Controls.Add(cardDel);

            cardDel.Controls.Add(new Panel
            {
                BackColor = Danger,
                Location = new Point(0, 0),
                Size = new Size(5, cardDel.Height),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
            });

            cardDel.Controls.Add(new Label
            {
                Text = "Supprimer un étudiant",
                Font = new Font("Segoe UI Semibold", 12.5F),
                ForeColor = Dark,
                AutoSize = true,
                Location = new Point(24, 14)
            });
            cardDel.Controls.Add(new Label
            {
                Text = "Donner le matricule de l'étudiant à supprimer, ou cliquez sur une ligne du tableau.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Gray,
                AutoSize = true,
                Location = new Point(25, 42)
            });

            txtMatricule = new TextBox
            {
                Font = new Font("Segoe UI", 12F),
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(24, 72),
                Width = 240
            };
            txtMatricule.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    Supprimer_Click(s, EventArgs.Empty);
                }
            };
            cardDel.Controls.Add(txtMatricule);

            btnSupprimer = MakeButton("🗑  Supprimer", Danger, DangerDk,
                                      new Size(160, 38), new Point(280, 70));
            btnSupprimer.Click += Supprimer_Click;
            cardDel.Controls.Add(btnSupprimer);

            // ---------- List card ----------
            Panel cardList = MakeCard(new Point(24, 252),
                new Size(ClientSize.Width - 48, ClientSize.Height - 252 - 24),
                AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            Controls.Add(cardList);

            cardList.Controls.Add(new Label
            {
                Text = "Table Etudiant",
                Font = new Font("Segoe UI Semibold", 12.5F),
                ForeColor = Dark,
                AutoSize = true,
                Location = new Point(24, 20)
            });

            btnAfficher = MakeButton("⟳  Afficher", Primary, PrimaryDk,
                                     new Size(140, 38), new Point(cardList.Width - 24 - 140, 12));
            btnAfficher.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAfficher.Click += Afficher_Click;
            cardList.Controls.Add(btnAfficher);

            txtSearch = new TextBox
            {
                Font = new Font("Segoe UI", 11F),
                BorderStyle = BorderStyle.FixedSingle,
                Width = 230,
                Location = new Point(cardList.Width - 24 - 140 - 12 - 230, 17),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            txtSearch.TextChanged += (s, e) => ApplyFilter();
            cardList.Controls.Add(txtSearch);

            cardList.Controls.Add(new Panel
            {
                BackColor = Border,
                Location = new Point(24, 62),
                Size = new Size(cardList.Width - 48, 1),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            });

            // Hint before the first click
            lblHint = new Label
            {
                Text = "Cliquez sur « Afficher » pour charger la table Etudiant.",
                ForeColor = Gray,
                Font = new Font("Segoe UI", 11F),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(24, 76),
                Size = new Size(cardList.Width - 48, cardList.Height - 76 - 70),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            cardList.Controls.Add(lblHint);

            // Grid
            grid = new DataGridView
            {
                Location = new Point(24, 76),
                Size = new Size(cardList.Width - 48, cardList.Height - 76 - 70),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Visible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                GridColor = Border,
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
            grid.RowTemplate.Height = 38;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Dark;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Dark;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 0, 0, 0);
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Dark;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 228, 230);
            grid.DefaultCellStyle.SelectionForeColor = Dark;
            grid.DefaultCellStyle.Padding = new Padding(12, 0, 0, 0);
            grid.AlternatingRowsDefaultCellStyle.BackColor = RowAlt;
            grid.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    txtMatricule.Text = grid.Rows[e.RowIndex].Cells[0].Value.ToString();
            };
            cardList.Controls.Add(grid);

            // Status bar + Close
            lblStatus = new Label
            {
                Text = "Prêt.",
                ForeColor = Gray,
                Font = new Font("Segoe UI", 9.5F),
                AutoSize = true,
                Location = new Point(24, cardList.Height - 46),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            cardList.Controls.Add(lblStatus);

            btnFermer = MakeButton("Fermer", Gray, GrayDk,
                                   new Size(130, 38),
                                   new Point(cardList.Width - 24 - 130, cardList.Height - 54));
            btnFermer.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnFermer.Click += Fermer_Click;
            cardList.Controls.Add(btnFermer);
        }

        private void SetStatus(string text, Color color)
        {
            lblStatus.Text = text;
            lblStatus.ForeColor = color;
        }

        // =====================================================
        //                       LOGIC
        // =====================================================

        // Form1_Load : open the connection when the form loads
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SendMessage(txtMatricule.Handle, EM_SETCUEBANNER, 1, "Matricule…");
            SendMessage(txtSearch.Handle, EM_SETCUEBANNER, 1, "🔍  Rechercher…");

            txtMatricule.Clear();
            txtMatricule.Focus();
            try
            {
                EnsureConnection();
                SetStatus("Connecté à la base de données.", Color.FromArgb(22, 163, 74));
            }
            catch (MySqlException ex)
            {
                SetStatus("Connexion impossible.", Danger);
                MessageBox.Show("Erreur de connexion : " + ex.Message, "Erreur",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureConnection()
        {
            if (con == null) con = new MySqlConnection(mydb);
            if (con.State != ConnectionState.Open) con.Open();
        }

        // Afficher_Click : show the table in the DataGridView
        private void Afficher_Click(object sender, EventArgs e)
        {
            LoadTable();
            txtMatricule.Focus();
        }

        private void LoadTable()
        {
            try
            {
                EnsureConnection();

                string cmdQuery = "SELECT * FROM Etudiant ORDER BY matricule";
                MySqlDataAdapter myAdapter = new MySqlDataAdapter(cmdQuery, con);

                DataSet ds = new DataSet();
                myAdapter.Fill(ds, "TableMySql");

                grid.DataSource = ds.Tables["TableMySql"].DefaultView;
                foreach (DataGridViewColumn col in grid.Columns)
                    col.HeaderText = char.ToUpper(col.HeaderText[0]) + col.HeaderText.Substring(1);
                if (grid.Columns.Count > 0) grid.Columns[0].FillWeight = 40;

                lblHint.Visible = false;
                grid.Visible = true;

                countText = ds.Tables["TableMySql"].Rows.Count + " étudiant(s)";
                header.Invalidate();

                ApplyFilter();
            }
            catch (MySqlException ex)
            {
                SetStatus("Erreur lors du chargement.", Danger);
                MessageBox.Show("Erreur : " + ex.Message, "Erreur de base de données",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Live search (matricule, nom, prenom)
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

            string t = txtSearch.Text.Trim();
            if (t.Length == 0)
            {
                dv.RowFilter = "";
                SetStatus(dv.Count + " étudiant(s) affiché(s).", Gray);
                return;
            }

            DataColumnCollection cols = dv.Table.Columns;
            string like = EscapeLike(t);
            dv.RowFilter = string.Format(
                "Convert([{0}], 'System.String') LIKE '%{3}%' OR [{1}] LIKE '%{3}%' OR [{2}] LIKE '%{3}%'",
                cols[0].ColumnName, cols[1].ColumnName, cols[2].ColumnName, like);
            SetStatus(dv.Count + " résultat(s) pour « " + t + " ».", Gray);
        }

        // Supprimer_Click : delete a record
        private void Supprimer_Click(object sender, EventArgs e)
        {
            int matricule;
            if (!int.TryParse(txtMatricule.Text.Trim(), out matricule))
            {
                MessageBox.Show("Veuillez entrer un matricule valide (nombre).", "Matricule invalide",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatricule.Focus();
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Voulez-vous vraiment supprimer l'étudiant n° " + matricule + " ?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                EnsureConnection();

                string SQL = "DELETE FROM Etudiant WHERE matricule = @matricule";
                MySqlCommand cmd = new MySqlCommand(SQL, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@matricule", matricule);

                int x = cmd.ExecuteNonQuery();

                if (x == 1)
                {
                    SetStatus("Étudiant n° " + matricule + " supprimé.", Color.FromArgb(22, 163, 74));
                    MessageBox.Show("Enregistrement supprimé avec succès... ", "Succès",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (grid.Visible) LoadTable();
                }
                else
                {
                    SetStatus("Matricule " + matricule + " introuvable.", Danger);
                    MessageBox.Show("Enregistrement introuvable...", "Information",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (MySqlException ex)
            {
                SetStatus("Erreur lors de la suppression.", Danger);
                MessageBox.Show("Erreur : " + ex.Message, "Erreur de base de données",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtMatricule.Clear();
            txtMatricule.Focus();
        }

        // Fermer_Click : close the connection and the window
        private void Fermer_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (con != null && con.State == ConnectionState.Open) con.Close();
            base.OnFormClosed(e);
        }

        // =====================================================
        // Empty stubs referenced by the Designer file. KEEP THEM:
        // deleting one causes a build error.
        // =====================================================
        private void Btn_Afficher_Click(object sender, EventArgs e) { }
        private void Btn_Supprimer_Click(object sender, EventArgs e) { }
        private void Text_box_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}