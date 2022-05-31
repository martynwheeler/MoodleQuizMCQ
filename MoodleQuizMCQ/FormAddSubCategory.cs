using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MoodleQuizMCQ
{
    public partial class FormAddSubCategory : Form
    {
        public string Category { get; set; } = string.Empty;
        public string NewSubCategory { get; set; } = string.Empty;

        public FormAddSubCategory(List<string> nodeList)
        {
            InitializeComponent();
            this.comboBoxCategories.DataSource = nodeList;

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            NewSubCategory = "";
            this.Close();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxCategories.Text) || comboBoxCategories.Text == "Please select a category...")
            {
                MessageBox.Show("No category is selected", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (txtCategory.Text != "")
                {
                    Category = comboBoxCategories.Text;
                    NewSubCategory = txtCategory.Text;
                    this.Close();
                }
            }
        }
    }
}
