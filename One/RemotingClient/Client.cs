using System;
using System.Data;
using System.Windows.Forms;

namespace RemotingClient
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
            RemoteConnection.Connect();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        // ---------- Helpers ----------

        private void RefreshGrid()
        {
            try
            {
                dgv.DataSource = RemoteConnection.operation.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de charger les donnees :\n" + ex.Message);
            }
        }

        private bool TryGetMatricule(out int matricule)
        {
            if (!int.TryParse(textBoxMatricule.Text.Trim(), out matricule))
            {
                MessageBox.Show("Matricule invalide : entrez un nombre entier.");
                textBoxMatricule.Focus();
                return false;
            }
            return true;
        }

        private bool NamesAreFilled()
        {
            if (string.IsNullOrWhiteSpace(textBoxNom.Text) ||
                string.IsNullOrWhiteSpace(textBoxPrenom.Text)||)
            {
                MessageBox.Show("Veuillez remplir le nom et le prenom.");
                return false;
            }
            return true;
        }

        private void ClearFields()
        {
            textBoxMatricule.Clear();
            textBoxNom.Clear();
            textBoxPrenom.Clear();
            textBoxMatricule.Focus();
        }

        // ---------- Buttons ----------

        private void Add_Click(object sender, EventArgs e)
        {
            if (!TryGetMatricule(out int matricule)) return;
            if (!NamesAreFilled()) return;

            try
            {
                int x = RemoteConnection.operation.Add(
                    matricule, textBoxNom.Text.Trim(), textBoxPrenom.Text.Trim());

                if (x == 1)
                {
                    MessageBox.Show("Etudiant ajouté avec succès");
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
            if (!TryGetMatricule(out int matricule)) return;
            if (!NamesAreFilled()) return;

            try
            {
                int x = RemoteConnection.operation.Update(
                    matricule, textBoxNom.Text.Trim(), textBoxPrenom.Text.Trim());

                if (x == 1)
                {
                    MessageBox.Show("Etudiant modifié avec succès");
                    RefreshGrid();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Matricule introuvable : aucune modification.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (!TryGetMatricule(out int matricule)) return;

            DialogResult confirm = MessageBox.Show(
                "Supprimer l'etudiant " + matricule + " ?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                int x = RemoteConnection.operation.Delete(matricule);

                if (x == 1)
                {
                    MessageBox.Show("Etudiant supprimé avec succès");
                    RefreshGrid();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Matricule introuvable : aucune suppression.");
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

        private void ShowStudent_Click(object sender, EventArgs e)
        {
            if (!TryGetMatricule(out int matricule)) return;

            try
            {
                DataTable dt = RemoteConnection.operation.ShowStudent(matricule);

                if (dt.Rows.Count > 0)
                {
                    dgv.DataSource = dt;
                    textBoxNom.Text = dt.Rows[0]["nom"].ToString();
                    textBoxPrenom.Text = dt.Rows[0]["prenom"].ToString();   // was "nom"
                }
                else
                {
                    MessageBox.Show("Etudiant introuvable");
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