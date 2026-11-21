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
    public partial class FormCourse : Form
    {
        const readonly string categorie = {"A","B"',"C" };
        public FormCourse()
        {
            InitializeComponent();
            RemoteConnection.Connect();
        }

        private void FormCourse_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        // ---------- Helpers ----------
        private void RefreshGrid()
        {
            try
            {
                dgv.DataSource = RemoteConnection.operation.ShowCourse();
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
                MessageBox.Show("Code invalide : entrez un nombre entier.");
                textBoxCode.Focus();
                return false;
            }
            return true;
        }


        private bool NamesAreFilled()
        {
            if (string.IsNullOrWhiteSpace(textBoxDes.Text) ||
                string.IsNullOrWhiteSpace(textBoxVol.Text) ||
                string.IsNullOrWhiteSpace(comboBoxCat.Text)
                )
            {
                MessageBox.Show("Veuillez remplir le designation et le volume et categorie.");
                return false;
            }
            return true;
        }

        private void ClearFields()
        {
            textBoxCode.Clear();
            textBoxDes.Clear();
            textBoxVol.Clear();
            comboBoxCat.Focus();
        }


        // ---------- Buttons ----------
        private void Add_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int code)) return;
            if (!NamesAreFilled()) return;
            try
            {
                int x = RemoteConnection.operation.AddCourse(
                    code, textBoxDes.Text.Trim(), textBoxVol.Text.Trim(),comboBoxCat.Text.Trim());

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

        }

        private void Delete_Click(object sender, EventArgs e)
        {

        }

        private void Show_Click(object sender, EventArgs e)
        {

        }

        private void ShowCourse_Click(object sender, EventArgs e)
        {

        }

    }
}
