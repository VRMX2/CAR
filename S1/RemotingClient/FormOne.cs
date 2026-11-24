using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemotingClient
{
    public partial class FormOne : Form
    {
        private static readonly string[] niveau = {"Debutant","Intermediaire","Avance"};
        public FormOne()
        {
            InitializeComponent();
            RemoteConnection.Connect();
        }

        private void FormOne_Load(object sender, EventArgs e)
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

        private static bool TryDate(string text, out DateTime d)
        {
            return DateTime.TryParseExact(text.Trim(), "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
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

        private bool TryGetFormationFields(out int duree,out int prix,out DateTime date)
        {
            duree = 0 ;
            prix= 0;
            date= default(DateTime);
            if(string.IsNullOrWhiteSpace(textBoxInt.Text) ||
               string.IsNullOrWhiteSpace(textBoxDuree.Text)||
               string.IsNullOrWhiteSpace(textBoxPrix.Text)||
               comboBoxNiv.SelectedIndex<0||
               string.IsNullOrWhiteSpace(textBoxDate.Text)){
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
        private void ClearStagiaire()
        {
            textBoxId.Clear();
            textBoxNom.Clear();
            textBoxPrenom.Clear();
            textBoxEmail.Clear();
            textBoxTelephone.Clear();
            textBoxVille.Clear();
            textBoxDateInscription.Clear();
            textBoxMontantPaye.Clear();
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


        // ---------- Buttons ----------


        private void Add_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int code)) return;
            if (!TryGetFormationFields(out int duree,out int prix,out DateTime date)) return;
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
            if (!TryGetFormationFields(out int duree,out int prix,out DateTime date)) return;
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
            try{
                DataTable dt = RemoteConnection.operation.SearchFormation(code);
                if(dt.Rows.Count>0){
                    dgv.DataSource = dt;
                    textBoxCode.Text = dt.Rows[0]["code"].ToString();
                    textBoxInt.Text = dt.Rows[0]["intitule"].ToString();
                    textBoxDuree.Text = dt.Rows[0]["duree"].ToString();
                    textBoxPrix.Text = dt.Rows[0]["prix"].ToString();
                    comboBoxNiv.SelectedItem = dt.Rows[0]["niveau"].ToString();
                    textBoxDate.Text = dt.Rows[0]["date_debut"].ToString();
                }
                else{
                    MessageBox.Show("Formation introuvable");
                    dgv.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddStagiare_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int code)) return;

            if (string.IsNullOrWhiteSpace(textBoxId.Text) ||
                string.IsNullOrWhiteSpace(textBoxNom.Text) ||
                string.IsNullOrWhiteSpace(textBoxPrenom.Text))
            { MessageBox.Show("Identifiant, nom et prenom sont obligatoires."); return; }
            if (textBoxId.Text.Trim().Length > 5)
            { MessageBox.Show("L'identifiant ne doit pas depasser 5 caracteres."); return; }
            if (!TryDate(textBoxDateInscription.Text, out DateTime dateInsc))
            { MessageBox.Show("Date d'inscription invalide : format JJ/MM/AAAA."); return; }
            if (!int.TryParse(textBoxMontantPaye.Text.Trim(), out int montant) || montant < 0)
            { MessageBox.Show("Montant invalide."); return; }

            try
            {
                int x = RemoteConnection.operation.AddStagiaire(
                    code,
                    textBoxId.Text.Trim(),
                    textBoxNom.Text.Trim(),
                    textBoxPrenom.Text.Trim(),
                    textBoxEmail.Text.Trim(),
                    textBoxTelephone.Text.Trim(),
                    textBoxVille.Text.Trim(),
                    dateInsc,
                    montant);

                if (x == 1)
                {
                    MessageBox.Show("Stagiaire inscrit avec succès");
                    dgv.DataSource = RemoteConnection.operation.GetStagiaires(code);
                    ClearStagiaire();
                }
                else MessageBox.Show("Ce stagiaire est déjà inscrit à cette formation.");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void NewFormation_Click(object sender, EventArgs e)
        {
            ClearFields();
            ClearStagiaire();
            textBoxCompter.Clear();
            textBoxMontant.Clear();
            RefreshGrid();
        }

        private void NewSatgiare_Click(object sender, EventArgs e)
        {
            ClearStagiaire();
            textBoxId.Focus();

        }

        private void Retour_Click(object sender, EventArgs e)
        {
            ClearStagiaire();
            textBoxCompter.Clear();
            textBoxMontant.Clear();
            RefreshGrid();
        }

        private void Compter_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int code)) return;
            try
            {
                textBoxCompter.Text = RemoteConnection.operation.CountStagiaires(code).ToString();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void Montant_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int code)) return;
            try
            {
                textBoxMontant.Text = RemoteConnection.operation.SumMontant(code).ToString();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void textBoxId_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxNom_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPrenom_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxTelephone_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxVille_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxDateInscription_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxMontantPaye_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
