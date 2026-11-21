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
        private static readonly string[] categories = {"A","B","C"};
        public FormCourse()
        {
            InitializeComponent();
            RemoteConnection.Connect();
        }

        private void FormCourse_Load(object sender, EventArgs e)
        {
            comboBoxCat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCat.Items.Clear();
            comboBoxCat.Items.AddRange(categories);
            comboBoxCat.SelectedIndex = -1;

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


        private bool TryGetCourseFields(out int volume)
        {
            volume = 0 ;
            if(string.IsNullOrWhiteSpace(textBoxDes.Text) ||
               string.IsNullOrWhiteSpace(textBoxVol.Text)||
               comboBoxCat.SelectedIndex<0){
                MessageBox.Show("Veuillez remplir la designation , le volume et la categorie");
            }
            if (!int.TryParse(textBoxVol.Text.Trim(), out volume) || volume <= 0)
            {
                MessageBox.Show("Volume invalide : entrez un nombre entier positif.");
                textBoxVol.Focus();
                return false;
            }
            return true;
        }



        private void ClearFields()
        {
            textBoxCode.Clear();
            textBoxDes.Clear();
            textBoxVol.Clear();
            comboBoxCat.SelectedIndex = -1;
            textBoxCode.Focus();
        }


        // ---------- Buttons ----------
        private void Add_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int code)) return;
            if (!TryGetCourseFields(out int volume)) return;
            try
            {
                int x = RemoteConnection.operation.AddCourse(
                    code,
                    textBoxDes.Text.Trim(),
                    volume,
                    comboBoxCat.SelectedItem.ToString());

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
            if (!TryGetCode(out int code)) return;
            if (!TryGetCourseFields(out int volume)) return;
            try
            {
                int x = RemoteConnection.operation.UpdateCourse(
                    code,
                    textBoxDes.Text.Trim(),
                    volume,
                    comboBoxCat.SelectedItem.ToString());

                if (x == 1)
                {
                    MessageBox.Show("Etudiant modifié avec succès");
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
                "Supprimer le cours " + code + " ?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                int x = RemoteConnection.operation.DeleteCourse(code);

                if (x == 1)
                {
                    MessageBox.Show("Cours supprimé avec succès");
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

        private void ShowCourse_Click(object sender, EventArgs e)
        {
            if (!TryGetCode(out int code)) return;
            try{
                DataTable dt = RemoteConnection.operation.SearchCourse(code);
                if(dt.Rows.Count>0){
                    dgv.DataSource = dt;
                    textBoxDes.Text = dt.Rows[0]["designation"].ToString();
                    textBoxVol.Text = dt.Rows[0]["volume"].ToString();
                    comboBoxCat.SelectedItem = dt.Rows[0]["categorie"].ToString();
                }
                else{
                    MessageBox.Show("Cours introuvable");
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
