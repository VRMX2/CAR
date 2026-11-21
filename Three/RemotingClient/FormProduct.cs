using System;
using System.Data;
using System.Windows.Forms;

namespace RemotingClient
{
    public partial class FormProduct : Form
    {
        private static readonly string[] categories =
            { "Electronique", "Informatique", "Electromenager", "Vestimentaire", "Livre", "Accessoire" };

        public FormProduct()
        {
            InitializeComponent();
            RemoteConnection.Connect();
        }

        private void FormProduct_Load(object sender, EventArgs e)
        {
            comboBoxCat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCat.Items.Clear();
            comboBoxCat.Items.AddRange(categories);
            comboBoxCat.SelectedIndex = -1;

            // Limits from the PDF's CREATE TABLE script
            textBoxRef.MaxLength = 5;
            textBoxDes.MaxLength = 30;
            textBoxPrix.MaxLength = 5;
            textBoxStock.MaxLength = 5;

            RefreshGrid();
        }

        // ---------- Helpers ----------

        private void RefreshGrid()
        {
            try
            {
                dgv.DataSource = RemoteConnection.operation.ShowProduct();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de charger les donnees :\n" + ex.Message);
            }
        }

        private bool TryGetReference(out int reference)
        {
            if (!int.TryParse(textBoxRef.Text.Trim(), out reference) || reference <= 0)
            {
                MessageBox.Show("Reference invalide : entrez un nombre entier positif.");
                textBoxRef.Focus();
                return false;
            }
            return true;
        }

        private bool TryGetProductFields(out int prix, out int qte)
        {
            prix = 0;
            qte = 0;

            if (string.IsNullOrWhiteSpace(textBoxDes.Text) ||
                string.IsNullOrWhiteSpace(textBoxPrix.Text) ||
                string.IsNullOrWhiteSpace(textBoxStock.Text) ||
                comboBoxCat.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez remplir la designation, le prix, le stock et la categorie.");
                return false;
            }

            if (!int.TryParse(textBoxPrix.Text.Trim(), out prix) || prix <= 0)
            {
                MessageBox.Show("Prix invalide : entrez un nombre entier positif.");
                textBoxPrix.Focus();
                return false;
            }

            if (!int.TryParse(textBoxStock.Text.Trim(), out qte) || qte < 0)
            {
                MessageBox.Show("Stock invalide : entrez un nombre entier (0 ou plus).");
                textBoxStock.Focus();
                return false;
            }
            return true;
        }

        private void ClearFields()
        {
            textBoxRef.Clear();
            textBoxDes.Clear();
            textBoxPrix.Clear();
            textBoxStock.Clear();
            comboBoxCat.SelectedIndex = -1;
            textBoxRef.Focus();
        }

        // ---------- Buttons ----------

        private void Add_Click(object sender, EventArgs e)
        {
            if (!TryGetReference(out int reference)) return;
            if (!TryGetProductFields(out int prix, out int qte)) return;

            try
            {
                int x = RemoteConnection.operation.AddProduct(
                    reference,
                    textBoxDes.Text.Trim(),
                    prix,
                    qte,
                    comboBoxCat.SelectedItem.ToString());

                if (x == 1)
                {
                    MessageBox.Show("Produit ajouté avec succès");
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
            if (!TryGetReference(out int reference)) return;
            if (!TryGetProductFields(out int prix, out int qte)) return;

            try
            {
                int x = RemoteConnection.operation.UpdateProduct(
                    reference,
                    textBoxDes.Text.Trim(),
                    prix,
                    qte,
                    comboBoxCat.SelectedItem.ToString());

                if (x == 1)
                {
                    MessageBox.Show("Produit modifié avec succès");
                    RefreshGrid();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Reference introuvable : aucune modification.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (!TryGetReference(out int reference)) return;

            DialogResult confirm = MessageBox.Show(
                "Supprimer le produit " + reference + " ?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                int x = RemoteConnection.operation.DeleteProduct(reference);

                if (x == 1)
                {
                    MessageBox.Show("Produit supprimé avec succès");
                    RefreshGrid();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Reference introuvable : aucune suppression.");
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
            if (!TryGetReference(out int reference)) return;

            try
            {
                DataTable dt = RemoteConnection.operation.SearchProduct(reference);

                if (dt.Rows.Count > 0)
                {
                    dgv.DataSource = dt;
                    textBoxDes.Text = dt.Rows[0]["designation"].ToString();
                    textBoxPrix.Text = dt.Rows[0]["prix"].ToString();
                    textBoxStock.Text = dt.Rows[0]["quantite_stock"].ToString();
                    comboBoxCat.SelectedItem = dt.Rows[0]["categorie"].ToString();
                }
                else
                {
                    MessageBox.Show("Produit introuvable");
                    dgv.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Opens the "Fiche d'ajout d'un produit" form (add a button for it)
        private void Fiche_Click(object sender, EventArgs e)
        {
            using (FormClientPro f = new FormClientPro())
            {
                f.ShowDialog(this);
            }
            RefreshGrid();
        }
    }
}