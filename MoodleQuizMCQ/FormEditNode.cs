namespace MoodleQuizMCQ
{
    public partial class FormEditNode : Form
    {
        public string EditCategory { get; set; } = string.Empty;

        public FormEditNode()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (txtCategory.Text != "")
            {
                EditCategory = txtCategory.Text;
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void FormEditNode_Load(object sender, EventArgs e)
        {
            txtCategory.Text = EditCategory;
        }
    }
}
