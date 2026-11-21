using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace RemotingClient
{
    public partial class FormClientPro : Form
    {
        private static readonly string[] categories =
            { "Electronique", "Informatique", "Electromenager", "Vestimentaire", "Livre", "Accessoire" };

        // Reference of the product added in this session (-1 = none yet)
        private int currentRef = -1;

        public FormClientPro()
        {
            InitializeComponent();
            RemoteConnection.Connect();
        }

        private void FormClientPro_Load(object sender, EventArgs e)
        {
            comboBoxCat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCat.Items.AddRange(categories);

            // Limits from the PDF's CREATE TABLE script
            textBoxRef.MaxLength = 5;
            textBoxDes.MaxLength = 30;
            textBoxPrix.MaxLength = 5;
            textBoxStock.MaxLength = 5;
            textBoxId.MaxLength = 5;
            textBoxNom.MaxLength = 20;
            textBoxPrenom.MaxLength = 20;
            textBoxAdr.MaxLength = 30;
            textBoxEmail.MaxLength = 30;
            textBoxQte.MaxLength = 5;

            textBoxMon.ReadOnly = true;
        }

        // ---------- Helpers ----------

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

        // Adresse and email are optional, the rest is required
        private bool TryGetClientFields(out DateTime date, out int quantite)
        {
            date = DateTime.Today;
            quantite = 0;

            if (string.IsNullOrWhiteSpace(textBoxId.Text) ||
                string.IsNullOrWhiteSpace(textBoxNom.Text) ||
                string.IsNullOrWhiteSpace(textBoxPrenom.Text) ||
                string.IsNullOrWhiteSpace(textBoxDate.Text) ||
                string.IsNullOrWhiteSpace(textBoxQte.Text))
            {
                MessageBox.Show("Veuillez remplir l'identifiant, le nom, le prenom, la date et la quantite.");
                return false;
            }

            if (!DateTime.TryParseExact(textBoxDate.Text.Trim(), "d/M/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                MessageBox.Show("Date invalide : utilisez le format jj/mm/aaaa (ex : 15/2/2018).");
                textBoxDate.Focus();
                return false;
            }

            if (!int.TryParse(textBoxQte.Text.Trim(), out quantite) || quantite <= 0)
            {
                MessageBox.Show("Quantite invalide : entrez un nombre entier positif.");
                textBoxQte.Focus();
                return false;
            }
            return true;
        }

        private void ShowClients(int reference)
        {
            dgv.DataSource = RemoteConnection.operation.GetClientsByProduct(reference);
            if (dgv.Columns["DATE_COMMANDE"] != null)
                dgv.Columns["DATE_COMMANDE"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void RefreshClients()
        {
            if (currentRef < 0) { dgv.DataSource = null; return; }
            ShowClients(currentRef);
        }

        private void ClearProductFields()
        {
            textBoxRef.Clear();
            textBoxDes.Clear();
            textBoxPrix.Clear();
            textBoxStock.Clear();
            comboBoxCat.SelectedIndex = -1;
            textBoxRef.Focus();
        }

        private void ClearClientFields()
        {
            textBoxId.Clear();
            textBoxNom.Clear();
            textBoxPrenom.Clear();
            textBoxAdr.Clear();
            textBoxEmail.Clear();
            textBoxDate.Clear();
            textBoxQte.Clear();
            textBoxId.Focus();
        }

        // ---------- Buttons ----------

        private void AddProduct_Click(object sender, EventArgs e)
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
                    currentRef = reference;
                    MessageBox.Show("Produit ajouté avec succès. Vous pouvez ajouter ses clients.");
                    RefreshClients();
                    textBoxId.Focus();
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du produit");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddClient_Click(object sender, EventArgs e)
        {
            if (currentRef < 0)
            {
                MessageBox.Show("Ajoutez d'abord le produit (bouton \"Ajouter produit\").");
                return;
            }
            if (!TryGetClientFields(out DateTime date, out int quantite)) return;

            try
            {
                int x = RemoteConnection.operation.AddClientOrder(
                    currentRef,
                    textBoxId.Text.Trim(),
                    textBoxNom.Text.Trim(),
                    textBoxPrenom.Text.Trim(),
                    textBoxAdr.Text.Trim(),
                    textBoxEmail.Text.Trim(),
                    date,
                    quantite);

                if (x == 1)
                {
                    MessageBox.Show("Client ajouté et commande enregistrée.");
                    RefreshClients();
                    ClearClientFields();
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du client");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NewClient_Click(object sender, EventArgs e)
        {
            ClearClientFields();
        }

        private void NewProduct_Click(object sender, EventArgs e)
        {
            currentRef = -1;
            ClearProductFields();
            ClearClientFields();
            textBoxMon.Clear();
            dgv.DataSource = null;
            textBoxRef.Focus();
        }

        // "Montant" button: total amount of the orders of the product in Reference
        private void button5_Click(object sender, EventArgs e)
        {
            if (!TryGetReference(out int reference)) return;

            try
            {
                decimal montant = RemoteConnection.operation.GetAmount(reference);
                textBoxMon.Text = montant.ToString("N0") + " DA";
                ShowClients(reference);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Return_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}