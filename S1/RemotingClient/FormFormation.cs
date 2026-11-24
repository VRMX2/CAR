using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemotingClient
{
    public partial class FormFormation : Form
    {
        private static readonly string[] niveau = { "Debutant", "Intermediaire", "Avance" };

        public FormFormation()
        {
            InitializeComponent();
            RemoteConnection.Connect();
        }
        private void FormFormation_Load(object sender, EventArgs e)
        {
            comboBoxNiv.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxNiv.Items.Clear();
            comboBoxNiv.Items.AddRange(niveau);
            comboBoxNiv.SelectedIndex = -1;
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            try
            {
                dgv.DataSource = RemoteConnection.operation.ShowFormation();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de charger les donnees :\n" + ex.Message);
            }
        }

        private bool TryGetCode(out int code)
        {
            if (!int.TryParse(textBoxCode.Text.Trim(), out code))
            {
                MessageBox.Show("code invalide : entrez un nombre entier.");
                textBoxCode.Focus();
                return false;
            }
            return true;
        }

        private bool TryGetFormationFields(out int duree, out int prix, out DateTime date)
        {
            duree = 0;
            prix = 0;
            date = default(DateTime);
            if (string.IsNullOrWhiteSpace(textBoxInt.Text) ||
               string.IsNullOrWhiteSpace(textBoxDuree.Text) ||
               string.IsNullOrWhiteSpace(textBoxPrix.Text) ||
               comboBoxNiv.SelectedIndex < 0 ||
               string.IsNullOrWhiteSpace(textBoxDate.Text))
            {
                MessageBox.Show("Veuillez remplir intitule , la duree, prix, langue et la date");
            }
            if (!int.TryParse(textBoxDuree.Text.Trim(), out duree) || duree <= 0)
            {
                MessageBox.Show("duree invalide : entrez un nombre entier positif.");
                textBoxDuree.Focus();
                return false;
            }
            if (!int.TryParse(textBoxPrix.Text.Trim(), out prix) || prix <= 0)
            {
                MessageBox.Show("Prix invalide : entrez un nombre entier positif.");
                textBoxPrix.Focus();
                return false;
            }
            if (!DateTime.TryParse(textBoxDate.Text.Trim(), out date))
            {
                MessageBox.Show("date invalide : entrez la date.");
                textBoxDate.Focus();
                return false;
            }
            return true;
        }
        private void ClearFields()
        {
            textBoxCode.Clear();
            textBoxInt.Clear();
            textBoxDuree.Clear();
            textBoxPrix.Clear();
            comboBoxNiv.SelectedIndex = -1;
            textBoxDate.Clear();
            textBoxCode.Focus();
        }


        private void Add_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int code)) return;
            if (!TryGetFormationFields(out int duree, out int prix, out DateTime date)) return;
            try
            {
                int x = RemoteConnection.operation.AddFormation(
                    code,
                    textBoxInt.Text.Trim(),
                    duree,
                    prix,
                    comboBoxNiv.SelectedItem.ToString(),
                    date
                    );

                if (x == 1)
                {
                    MessageBox.Show("Formation ajouté avec succès");
                    RefreshGrid();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int code)) return;
            if (!TryGetFormationFields(out int duree, out int prix, out DateTime date)) return;
            try
            {
                int x = RemoteConnection.operation.UpdateFormation(
                    code,
                    textBoxInt.Text.Trim(),
                    duree,
                    prix,
                    comboBoxNiv.SelectedItem.ToString(),
                    date
                    );

                if (x == 1)
                {
                    MessageBox.Show("Formation modifié avec succès");
                    RefreshGrid();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Code introuvable : aucune modification.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int code)) return;
            DialogResult confirm = MessageBox.Show(
                "Supprimer Formation" + code + " ?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                int x = RemoteConnection.operation.DeleteFormation(code);

                if (x == 1)
                {
                    MessageBox.Show("Formation supprimé avec succès");
                    RefreshGrid();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Code introuvable : aucune suppression.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Show_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void Search_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int code)) return;
            try
            {
                DataTable dt = RemoteConnection.operation.SearchFormation(code);
                if (dt.Rows.Count > 0)
                {
                    dgv.DataSource = dt;
                    textBoxCode.Text = dt.Rows[0]["code"].ToString();
                    textBoxInt.Text = dt.Rows[0]["intitule"].ToString();
                    textBoxDuree.Text = dt.Rows[0]["duree"].ToString();
                    textBoxPrix.Text = dt.Rows[0]["prix"].ToString();
                    comboBoxNiv.SelectedItem = dt.Rows[0]["niveau"].ToString();
                    textBoxDate.Text = dt.Rows[0]["date_debut"].ToString();
                }
                else
                {
                    MessageBox.Show("Formation introuvable");
                    dgv.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
