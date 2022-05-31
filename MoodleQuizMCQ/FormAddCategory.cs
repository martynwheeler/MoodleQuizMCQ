using System;
using System.Windows.Forms;

namespace MoodleQuizMCQ
{
    public partial class FormAddCategory : Form
    {
        public string NewCategory { get; set; } = string.Empty;

        public FormAddCategory()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            NewCategory = "";
            this.Close();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (txtCategory.Text != "")
            {
                NewCategory = txtCategory.Text;
                this.Close();
            }
        }
    }
}
