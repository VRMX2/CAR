using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TP4_PART2
{
    public partial class Form1 : Form
    {
        private const string mydb =
            "Server=localhost;Port=3306;Database=db_tp4;Uid=root;Pwd=lahcengrissi123;";

        // Palette
        private static readonly Color Primary = Color.FromArgb(37, 99, 235);
        private static readonly Color PrimaryDk = Color.FromArgb(29, 78, 216);
        private static readonly Color Dark = Color.FromArgb(15, 23, 42);
        private static readonly Color Muted = Color.FromArgb(100, 116, 139);
        private static readonly Color Back = Color.FromArgb(241, 245, 249);
        private static readonly Color RowAlt = Color.FromArgb(248, 250, 252);
        private static readonly Color Border = Color.FromArgb(226, 232, 240);

        private Label lblCount;
        private Label lblHint;

        public Form1()
        {
            InitializeComponent();
            ApplyTheme();

            // Wire the button in code so it always works,
            // whichever handler the Designer points to.
            Btn_Afficher.Click -= Btn_Afficher_Click;
            Btn_Afficher.Click -= Btn_Afficher_Click_1;
            Btn_Afficher.Click += Btn_Afficher_Click;
        }

        // ---------- DESIGN ----------
        private void ApplyTheme()
        {
            // Form
            Text = "Gestion des étudiants";
            BackColor = Back;
            Font = new Font("Segoe UI", 10F);
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(820, 540);
            MinimumSize = new Size(640, 420);

            // Remove any leftover design-time label
            foreach (Control c in Controls.Cast<Control>().Where(c => c is Label).ToList())
                Controls.Remove(c);

            // ----- Header bar -----
            Panel header = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Dark };

            Label title = new Label
            {
                Text = "Gestion des étudiants",
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 16F),
                Dock = DockStyle.Left,
                Width = 420,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(24, 0, 0, 0)
            };
            lblCount = new Label
            {
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 10F),
                Dock = DockStyle.Right,
                Width = 220,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 24, 0)
            };
            header.Controls.Add(lblCount);
            header.Controls.Add(title);
            Controls.Add(header);

            // ----- White card -----
            Panel card = new Panel
            {
                BackColor = Color.White,
                Location = new Point(24, 94),
                Size = new Size(ClientSize.Width - 48, ClientSize.Height - 94 - 24),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            card.Paint += (s, e) =>
            {
                using (Pen p = new Pen(Border))
                    e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
            };
            Controls.Add(card);

            // ----- Section title (the old textbox, now a clean heading) -----
            Text_box.Multiline = false;
            Text_box.ReadOnly = true;
            Text_box.TabStop = false;
            Text_box.BorderStyle = BorderStyle.None;
            Text_box.BackColor = Color.White;
            Text_box.ForeColor = Dark;
            Text_box.Font = new Font("Segoe UI Semibold", 13F);
            Text_box.Cursor = Cursors.Default;
            Text_box.Text = "Liste des étudiants";
            Text_box.Width = 320;
            Text_box.Location = new Point(20, 24);
            Text_box.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            card.Controls.Add(Text_box);

            // ----- Button (top right of the card) -----
            Btn_Afficher.FlatStyle = FlatStyle.Flat;
            Btn_Afficher.FlatAppearance.BorderSize = 0;
            Btn_Afficher.FlatAppearance.MouseOverBackColor = PrimaryDk;
            Btn_Afficher.FlatAppearance.MouseDownBackColor = Dark;
            Btn_Afficher.BackColor = Primary;
            Btn_Afficher.ForeColor = Color.White;
            Btn_Afficher.Font = new Font("Segoe UI Semibold", 10F);
            Btn_Afficher.Cursor = Cursors.Hand;
            Btn_Afficher.Text = "Afficher";
            Btn_Afficher.Size = new Size(130, 38);
            Btn_Afficher.Location = new Point(card.Width - Btn_Afficher.Width - 20, 18);
            Btn_Afficher.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            card.Controls.Add(Btn_Afficher);

            // ----- Separator line -----
            Panel line = new Panel
            {
                BackColor = Border,
                Location = new Point(20, 70),
                Size = new Size(card.Width - 40, 1),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            card.Controls.Add(line);

            // ----- Hint shown before the first click -----
            lblHint = new Label
            {
                Text = "Cliquez sur « Afficher » pour charger la table Etudiant.",
                ForeColor = Muted,
                Font = new Font("Segoe UI", 11F),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 84),
                Size = new Size(card.Width - 40, card.Height - 84 - 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            card.Controls.Add(lblHint);

            // ----- Grid (hidden until the first click) -----
            DataGridView g = dataGridView1;
            g.Location = new Point(20, 84);
            g.Size = new Size(card.Width - 40, card.Height - 84 - 20);
            g.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            g.Visible = false;
            card.Controls.Add(g);

            g.BackgroundColor = Color.White;
            g.BorderStyle = BorderStyle.None;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.GridColor = Border;
            g.EnableHeadersVisualStyles = false;
            g.RowHeadersVisible = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.AllowUserToResizeRows = false;
            g.AllowUserToResizeColumns = false;
            g.ReadOnly = true;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.RowTemplate.Height = 36;
            g.ColumnHeadersHeight = 40;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            g.ColumnHeadersDefaultCellStyle.BackColor = Dark;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Dark;
            g.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            g.DefaultCellStyle.BackColor = Color.White;
            g.DefaultCellStyle.ForeColor = Dark;
            g.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            g.DefaultCellStyle.SelectionForeColor = Dark;
            g.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            g.AlternatingRowsDefaultCellStyle.BackColor = RowAlt;
        }

        // ---------- LOGIC ----------
        private void LoadData()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(mydb))
                {
                    con.Open();

                    string cmdQuery = "SELECT * FROM Etudiant ORDER BY matricule";
                    MySqlDataAdapter myAdapter = new MySqlDataAdapter(cmdQuery, con);

                    DataSet ds = new DataSet();
                    myAdapter.Fill(ds, "TableMySql");

                    dataGridView1.DataSource = ds.Tables["TableMySql"].DefaultView;

                    // Nicer column titles
                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                        col.HeaderText = char.ToUpper(col.HeaderText[0]) + col.HeaderText.Substring(1);

                    lblHint.Visible = false;
                    dataGridView1.Visible = true;
                    lblCount.Text = ds.Tables["TableMySql"].Rows.Count + " étudiant(s)";
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur de base de données",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // The real click handler (wired in the constructor)
        private void Btn_Afficher_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        // ---------- Empty stubs referenced by the Designer (keep them) ----------
        private void Btn_Afficher_Click_1(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e) { }
        private void Text_box_TextChanged(object sender, EventArgs e) { }
        private void Text_box_TextChanged_1(object sender, EventArgs e) { }
    }
}