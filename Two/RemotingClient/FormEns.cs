using System;
using System.Data;
using System.Windows.Forms;

namespace RemotingClient
{
    public partial class FormEns : Form
    {
        private static readonly string[] categories = { "A", "B", "C" };
        private static readonly string[] grades = { "PROFESSEUR", "MCA", "MCB", "MAA", "MAB" };
        private int currentCode = -1;


        public FormEns()
        {
            InitializeComponent();
            RemoteConnection.Connect();
        }

        private void FormEns_Load(object sender, EventArgs e)
        {
            comboBoxCat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCat.Items.AddRange(categories);

            comboBoxGrade.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxGrade.Items.AddRange(grades);

            textBoxCode.MaxLength = 5;
            textBoxDes.MaxLength = 30;
            textBoxVol.MaxLength = 2;
            textBoxNom.MaxLength = 30;
            textBoxPrenom.MaxLength = 20;
            textBoxTel.MaxLength = 20;
            textBoxEmail.MaxLength = 20;
            textBoxEx.MaxLength = 5;

            textBoxCount.ReadOnly = true;
        }

        // ---------- Helpers ----------
        private bool TryGetCode(out int code)
        {
            if (!int.TryParse(textBoxCode.Text.Trim(), out code) || code <= 0)
            {
                MessageBox.Show("Code invalide : entrez un nombre entier positif.");
                textBoxCode.Focus();
                return false;
            }
            return true;
        }

        private bool TryGetCourseFields(out int code, out int volume)
        {
            volume = 0;
            if (!TryGetCode(out code)) return false;

            if (string.IsNullOrWhiteSpace(textBoxDes.Text) ||
                string.IsNullOrWhiteSpace(textBoxVol.Text) ||
                comboBoxCat.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez remplir la designation, le volume et la categorie.");
                return false;
            }

            if (!int.TryParse(textBoxVol.Text.Trim(), out volume) || volume <= 0)
            {
                MessageBox.Show("Volume invalide : entrez un nombre entier entre 1 et 99.");
                textBoxVol.Focus();
                return false;
            }
            return true;
        }

        private bool TryGetTeacherFields(out int experience)
        {
            experience = 0;

            if (string.IsNullOrWhiteSpace(textBoxNom.Text) ||
                string.IsNullOrWhiteSpace(textBoxPrenom.Text) ||
                string.IsNullOrWhiteSpace(textBoxEmail.Text) ||
                string.IsNullOrWhiteSpace(textBoxTel.Text) ||
                comboBoxGrade.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez remplir tous les champs de l'enseignant (y compris le grade).");
                return false;
            }

            if (!int.TryParse(textBoxEx.Text.Trim(), out experience) || experience < 0)
            {
                MessageBox.Show("Experience invalide : entrez un nombre entier.");
                textBoxEx.Focus();
                return false;
            }
            return true;
        }


        private void RefreshTeachers()
        {
            if (currentCode < 0) { dgv.DataSource = null; return; }
            dgv.DataSource = RemoteConnection.operation.GetTeachersByCourse(currentCode);
        }

        private void ClearCourseFields()
        {
            textBoxCode.Clear();
            textBoxDes.Clear();
            textBoxVol.Clear();
            comboBoxCat.SelectedIndex = -1;
            textBoxCode.Focus();
        }

        private void ClearTeacherFields()
        {
            textBoxNom.Clear();
            textBoxPrenom.Clear();
            textBoxEmail.Clear();
            textBoxTel.Clear();
            textBoxEx.Clear();
            comboBoxGrade.SelectedIndex = -1;
            textBoxNom.Focus();
        }

         // ---------- Buttons ----------

        private void Add_Click(object sender, EventArgs e)
        {
            if (!TryGetCourseFields(out int code, out int volume)) return;

            try
            {
                int x = RemoteConnection.operation.AddCourse(
                    code,
                    textBoxDes.Text.Trim(),
                    volume,
                    comboBoxCat.SelectedItem.ToString());

                if (x == 1)
                {
                    currentCode = code;
                    MessageBox.Show("Cours ajouté avec succès. Vous pouvez ajouter ses enseignants.");
                    RefreshTeachers();
                    textBoxNom.Focus();
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du cours");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddEns_Click(object sender, EventArgs e)
        {
            if (currentCode < 0)
            {
                MessageBox.Show("Ajoutez d'abord le cours (bouton \"Ajouter cours\").");
                return;
            }
            if (!TryGetTeacherFields(out int experience)) return;
            try
            {
                int matricule = RemoteConnection.operation.AddTeacher(
                    currentCode,
                    textBoxNom.Text.Trim(),
                    textBoxPrenom.Text.Trim(),
                    textBoxTel.Text.Trim(),
                    textBoxEmail.Text.Trim(),
                    experience,
                    comboBoxGrade.SelectedItem.ToString());

                MessageBox.Show("Enseignant ajouté avec succès (matricule " + matricule + ").");
                RefreshTeachers();
                ClearTeacherFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void NewCourse_Click(object sender, EventArgs e)
        {
            currentCode = -1;
            ClearCourseFields();
            ClearTeacherFields();
            textBoxCount.Clear();
            dgv.DataSource = null;
            textBoxCode.Focus();
        }


        private void NewEns_Click(object sender, EventArgs e)
        {
            ClearTeacherFields();
        }


        private void Show_Click(object sender, EventArgs e)
        {

        }

        private void Count_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int code)) return;
            try
            {
                textBoxCount.Text = RemoteConnection.operation.CountTeacher(code).ToString();
                dgv.DataSource = RemoteConnection.operation.GetTeachersByCourse(code);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {

        }

        private void Return_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
