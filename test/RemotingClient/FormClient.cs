using System;
using System.Linq;
using System.Windows.Forms;

namespace RemotingClient
{
    public partial class FormClient : Form
    {
        private static readonly string[] grade = { "Doctorant", "Professeur" };
        private static readonly string[] revue = { "Internationale", "Nationale" };

        public FormClient()
        {
            InitializeComponent();
            MessageBox.Show(
        Application.ExecutablePath + "\n\n" +
        typeof(RemotingInterfaces.IRemoteOperation).Assembly.Location + "\n\n" +
        string.Join(", ", typeof(RemotingInterfaces.IRemoteOperation)
            .GetMethods().Select(m => m.Name)));

            RemoteConnection.Connect();
        }

        private void FormClient_Load(object sender, EventArgs e)
        {
            comboBoxGrade.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxGrade.Items.Clear();
            comboBoxGrade.Items.AddRange(grade);
            comboBoxGrade.SelectedIndex = -1;

            comboBoxRevue.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxRevue.Items.Clear();
            comboBoxRevue.Items.AddRange(revue);
            comboBoxRevue.SelectedIndex = -1;

            RefreshChercheurs();
        }

        private void RefreshChercheurs()
        {
            try
            {
                dgv.DataSource = RemoteConnection.operation.ShowChercheur();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de charger les donnees :\n" + ex.Message);
            }
        }

        private void RefreshArticles(int idcher)
        {
            try
            {
                dgv.DataSource = RemoteConnection.operation.ShowArticle(idcher);
                decimal total = RemoteConnection.operation.Calculer(idcher);
                textBoxMontantGlobale.Text = total.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible de charger les donnees :\n" + ex.Message);
            }
        }

        private bool TryGetCode(out int code)
        {
            if (!int.TryParse(textBoxIdCher.Text.Trim(), out code))
            {
                MessageBox.Show("IdCher invalide : entrez un nombre entier.");
                textBoxIdCher.Focus();
                return false;
            }
            return true;
        }

        private bool TryGetChercheurFields()
        {
            if (string.IsNullOrWhiteSpace(textBoxNomCher.Text) ||
                string.IsNullOrWhiteSpace(textBoxPrenomCher.Text) ||
                comboBoxGrade.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez remplir le nom, le prenom et le grade.");
                return false;
            }
            return true;
        }

        private bool TryGetArticleFields(out int idart, out DateTime datepub, out int frais)
        {
            idart = 0; frais = 0; datepub = DateTime.MinValue;

            if (!int.TryParse(textBoxIdaArt.Text.Trim(), out idart))
            {
                MessageBox.Show("IdArt invalide : entrez un nombre entier.");
                textBoxIdaArt.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxTitreArt.Text))
            {
                MessageBox.Show("Veuillez saisir le titre de l'article.");
                textBoxTitreArt.Focus();
                return false;
            }
            if (!DateTime.TryParse(textBoxDatePub.Text.Trim(), out datepub))
            {
                MessageBox.Show("Date invalide (exemple : 25/11/2026).");
                textBoxDatePub.Focus();
                return false;
            }
            if (!int.TryParse(textBoxFrais.Text.Trim(), out frais) || frais < 0)
            {
                MessageBox.Show("Frais invalide : entrez un nombre entier positif.");
                textBoxFrais.Focus();
                return false;
            }
            if (comboBoxRevue.SelectedIndex < 0)
            {
                MessageBox.Show("Choisissez la revue.");
                return false;
            }
            return true;
        }

        private void ClearChercheurFields()
        {
            textBoxIdCher.Clear();
            textBoxNomCher.Clear();
            textBoxPrenomCher.Clear();
            comboBoxGrade.SelectedIndex = -1;
            textBoxIdCher.Focus();
        }

        private void ClearArticleFields()
        {
            textBoxIdaArt.Clear();
            textBoxTitreArt.Clear();
            textBoxDatePub.Clear();
            textBoxFrais.Clear();
            comboBoxRevue.SelectedIndex = -1;
        }

        private void AddCher_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int idcher)) return;
            if (!TryGetChercheurFields()) return;
            try
            {
                int x = RemoteConnection.operation.AddChercheur(
                    idcher,
                    textBoxNomCher.Text.Trim(),
                    textBoxPrenomCher.Text.Trim(),
                    comboBoxGrade.SelectedItem.ToString());

                if (x == 1)
                {
                    MessageBox.Show("Chercheur ajouté avec succès");
                    RefreshChercheurs();
                    ClearChercheurFields();
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

        private void AddArt_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int idcher)) return;
            if (!TryGetArticleFields(out int idart, out DateTime datepub, out int frais)) return;
            try
            {
                int x = RemoteConnection.operation.AddArticle(
                    idart,
                    textBoxTitreArt.Text.Trim(),
                    datepub,
                    frais,
                    comboBoxRevue.SelectedItem.ToString(),
                    idcher);

                if (x == 1)
                {
                    MessageBox.Show("Article ajouté avec succès");
                    ClearArticleFields();
                    RefreshArticles(idcher);
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

        private void MontantGlobale_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int idcher)) return;
            RefreshArticles(idcher);
        }

        private void Show_Click(object sender, EventArgs e)
        {
            RefreshChercheurs();
        }

        private void textBoxMontantGlobale_TextChanged(object sender, EventArgs e)
        {
        }
    }
}