using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MoodleQuizMCQ
{
    public partial class FormEditQuestion : Form
    {
        public bool QuestionDataChanged { get; set; } = false;

        public DataRow QuestionRow { get; set; }

        public Subject QuestionSubject { get; set; }

        private bool hasChanged;

        public FormEditQuestion()
        {
            InitializeComponent();
        }

        private void FormEditQuestion_Load(object sender, EventArgs e)
        {
            //load items into topic combobox
            foreach (SubjectTopic topic in QuestionSubject.Items)
            {
                comboBoxTopic.Items.Add(topic.name);
            }

            //load the data into the form
            UpdateFormData();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            //Cancel
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            //Return to the form and trigger save
            if (hasChanged)
            {
                if (MessageBox.Show("Question data changed, update?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    UpdateQuestion();
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                pictureBoxImage.Image = Clipboard.GetImage();
                QuestionDataChanged = true;
                hasChanged = true;
            }
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            if (hasChanged)
            {
                if (MessageBox.Show("Question data changed, update?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    UpdateQuestion();
                }
            }
            
            DataGridView dgv = (DataGridView)this.Owner.Controls["tabControlMain"].Controls["tabPageQuestionData"].Controls["dataGridViewQuestions"];
            if (dgv.SelectedRows.Count == 1)
            {
                int newIndex = dgv.SelectedRows[0].Index + 1;
                if (newIndex < dgv.Rows.Count - 1)
                {
                    dgv.ClearSelection();
                    dgv.Rows[newIndex].Selected = true;
                    QuestionRow = ((DataRowView)dgv.SelectedRows[0].DataBoundItem).Row;
//                    QuestionRow = ((FormMain)this.Owner).Datatable.Rows[newIndex];
                    UpdateFormData();
                }
            }
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            if (hasChanged)
            {
                if (MessageBox.Show("Question data changed, update?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    UpdateQuestion();
                }
            }

            DataGridView dgv = (DataGridView)this.Owner.Controls["tabControlMain"].Controls["tabPageQuestionData"].Controls["dataGridViewQuestions"];
            if (dgv.SelectedRows.Count == 1)
            {
                int newIndex = dgv.SelectedRows[0].Index - 1;
                if (newIndex >= 0)
                {
                    dgv.ClearSelection();
                    dgv.Rows[newIndex].Selected = true;
                    QuestionRow = ((DataRowView)dgv.SelectedRows[0].DataBoundItem).Row;
//                    QuestionRow = ((FormMain)this.Owner).Datatable.Rows[newIndex];
                    UpdateFormData();
                }
            }
        }

        private void ButtonShowImageString_Click(object sender, EventArgs e)
        {
            //Stop checking for changes
            textBoxImageBase64.TextChanged -= TextBoxtextBoxImageBase64_TextChanged;
            //Convert image to string
            textBoxImageBase64.Text = Convert.ToBase64String(ClassUtils.ImageToByte(pictureBoxImage.Image));
            //Start checking for changes
            textBoxImageBase64.TextChanged += TextBoxtextBoxImageBase64_TextChanged;
        }

        private void TextBoxtextBoxImageBase64_TextChanged(object sender, EventArgs e)
        {
            if (ClassUtils.IsBase64String(textBoxImageBase64.Text) && textBoxImageBase64.Text.Length > 0)
            {
                pictureBoxImage.Image = ClassUtils.ByteToImage(Convert.FromBase64String(textBoxImageBase64.Text));
                QuestionDataChanged = true;
                hasChanged = true;
            }
        }

        private void ButtonUpdateQuestion_Click(object sender, EventArgs e)
        {
            UpdateQuestion();
        }

        private void UpdateQuestion()
        {
            //update datarow
            QuestionRow["Question Topic"] = comboBoxTopic.Text;
            QuestionRow["Question Category"] = comboBoxCategory.Text;
            QuestionRow["Question Name"] = textBoxQuestionName.Text;
            QuestionRow["Question Image"] = ClassUtils.ImageToByte(pictureBoxImage.Image);
            QuestionRow["Question Answer"] = comboBoxAnswer.Text;
            hasChanged = false;
/*
            DataGridView dgv = (DataGridView)this.Owner.Controls["tabControlMain"].Controls["tabPageQuestionData"].Controls["dataGridViewQuestions"];
            if (dgv.SelectedRows.Count == 1)
            {
                int newIndex = dgv.SelectedRows[0].Index + 1;
                if (newIndex < dgv.Rows.Count - 1)
                {
                    dgv.ClearSelection();
                    dgv.Rows[newIndex].Selected = true;
                    QuestionRow = ((DataRowView)dgv.SelectedRows[0].DataBoundItem).Row;
                    //                    QuestionRow = ((FormMain)this.Owner).Datatable.Rows[newIndex];
                    UpdateFormData();
                }
            }
*/
        }

        private void UpdateFormData()
        {
            //Unhook event handlers
            comboBoxTopic.SelectedValueChanged -= ComboBoxTopic_SelectedValueChanged;
            comboBoxCategory.SelectedValueChanged -= ComboBox_SelectedValueChanged;
            comboBoxAnswer.SelectedValueChanged -= ComboBox_SelectedValueChanged;
            textBoxQuestionName.TextChanged -= TextBoxQuestionData_TextChanged;

            //Set Form controls
            comboBoxTopic.Text = QuestionRow["Question Topic"].ToString();
            UpdateCategory();
            textBoxQuestionName.Text = QuestionRow["Question Name"].ToString();
            pictureBoxImage.Image = ClassUtils.ByteToImage((byte[])QuestionRow["Question Image"]);
            comboBoxAnswer.Text = QuestionRow["Question Answer"].ToString();

            //Rehook event handlers
            comboBoxTopic.SelectedValueChanged += ComboBoxTopic_SelectedValueChanged;
            comboBoxCategory.SelectedValueChanged += ComboBox_SelectedValueChanged;
            comboBoxAnswer.SelectedValueChanged += ComboBox_SelectedValueChanged;
            textBoxQuestionName.TextChanged += TextBoxQuestionData_TextChanged;

            hasChanged = false;
        }

        private void TextBoxQuestionData_TextChanged(object sender, EventArgs e)
        {
            QuestionDataChanged = true;
            hasChanged = true;
        }

        private void UpdateCategory()
        {
            comboBoxCategory.Items.Clear();

            SubjectTopic selectedTopic = QuestionSubject.Items.First(item => item.name == comboBoxTopic.Text);
            foreach (SubjectTopicCategory category in selectedTopic.Category)
            {
                comboBoxCategory.Items.Add(category.name);
            }

            if (comboBoxCategory.Items.Contains(QuestionRow["Question Category"].ToString()))
            {
                comboBoxCategory.Text = QuestionRow["Question Category"].ToString();
            }
            else
            {
                comboBoxCategory.Text = selectedTopic.Category[0].name;
            }
        }

        private void ComboBoxTopic_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateCategory();
            QuestionDataChanged = true;
            hasChanged = true;
        }

        private void ComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            QuestionDataChanged = true;
            hasChanged = true;
        }
    }
}