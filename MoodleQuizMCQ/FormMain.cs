using CsvHelper;
using MoodleQuizMCQ.Properties;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using WK.Libraries.SharpClipboardNS;

namespace MoodleQuizMCQ
{
    public partial class FormMain : Form
    {
        private readonly DataTable Datatable = new();
        private bool unsavedQuestions = false;
        private string currentFileName;
        private FormWindowState savedWindowState;

        public FormMain()
        {
            InitializeComponent();
        }

        // Form
        private void Main_Load(object sender, EventArgs e)
        {
            //Build datatable to hold questions - may wish to load this from a file?
            Datatable.Columns.Add("Question Topic");
            Datatable.Columns.Add("Question Category");
            Datatable.Columns.Add("Question Name");
            Datatable.Columns.Add("Question Image");
            Datatable.Columns["Question Image"]!.DataType = System.Type.GetType("System.Byte[]");
            Datatable.Columns.Add("Question Answer");
            
            //Attach to datagrid
            dataGridViewQuestions.DataSource = Datatable;
            dataGridViewQuestions.Columns["Question Image"].Visible = false;

            // Adds our form to the chain of clipboard viewers.
            Clipboard.Clear();
            sharpClipboard1.MonitorClipboard = true;

            //Load settings
            if (Settings.Default.ExamPaperName != null)
            {
                txtExamPaperName.Text = Settings.Default.ExamPaperName;
            }
            if (Settings.Default.selectedTab != null)
            {
                tabControlSubject.SelectedTab = tabControlSubject.TabPages[Settings.Default.selectedTab];
            }
            if (Settings.Default.LastFileLocation != null)
            {
                saveFileDialog1.InitialDirectory = Settings.Default.LastFileLocation;
                openFileDialog1.InitialDirectory = Settings.Default.LastFileLocation;
            }
            if (Settings.Default.Maximised)
            {
                Location = Settings.Default.Location;
                WindowState = FormWindowState.Maximized;
                Size = Settings.Default.Size;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                Location = Settings.Default.Location;
                Size = Settings.Default.Size;
            }

            comboBoxAnswers.Text = "Select";
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            // Load treeview from xml file
            try
            {
                treeViewGCSE.BeginUpdate();
                LoadTreeView(treeViewGCSE, Application.StartupPath + "\\GCSECategories.xml");
                treeViewGCSE.Sort();
                treeViewGCSE.ExpandAll();
                treeViewGCSE.EndUpdate();

                treeViewALevel.BeginUpdate();
                LoadTreeView(treeViewALevel, Application.StartupPath + "\\ALevelCategories.xml");
                treeViewALevel.Sort();
                treeViewALevel.ExpandAll();
                treeViewALevel.EndUpdate();
            }
            catch
            {
                MessageBox.Show("Fatal Error, missing xml files.  The application will now terminate.", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            //enable and disable menu items
            toolsToolStripMenuItem.Enabled = true;
            questionsToolStripMenuItem.Enabled = false;
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = DialogResult.Yes;
            if (unsavedQuestions)
            {
                const string message = "You have unsaved questions, are you sure you want to quit?";
                const string caption = "Form Closing";
                dialogResult = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
            }

            // If the no button was pressed ...
            if (dialogResult == DialogResult.No)
            {
                // cancel the closure of the form.
                e.Cancel = true;
            }
            else
            {
                //Save current settings
                if (WindowState == FormWindowState.Maximized)
                {
                    Settings.Default.Location = RestoreBounds.Location;
                    Settings.Default.Size = RestoreBounds.Size;
                    Settings.Default.Maximised = true;
                }
                else if (WindowState == FormWindowState.Normal)
                {
                    Settings.Default.Location = Location;
                    Settings.Default.Size = Size;
                    Settings.Default.Maximised = false;
                }
                else
                {
                    Settings.Default.Location = RestoreBounds.Location;
                    Settings.Default.Size = RestoreBounds.Size;
                    Settings.Default.Maximised = false;
                }

                Settings.Default.ExamPaperName = txtExamPaperName.Text;
                Settings.Default.selectedTab = tabControlSubject.SelectedTab.Name;
                Settings.Default.Save();
            }
        }

        // Buttons
        private void BtnSaveImg_Click(object sender, EventArgs e)
        {
            SaveImageToQuestion();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            this.numCounter.Value = 1;
            Clipboard.Clear();
        }

        private void BtnDiscard_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            savedWindowState = WindowState;
            Hide();
            notifyIcon1.Visible = true;
        }

        // control changes
        private void TxtFileName_TextChanged(object sender, EventArgs e)
        {
            UpdateFileName();
        }

        private void NumCounter_ValueChanged(object sender, EventArgs e)
        {
            UpdateFileName();
        }

        private void DataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var rowHeaderText = (e.RowIndex + 1).ToString();
            var dgv = sender as DataGridView;
            using SolidBrush brush = new(dgv!.RowHeadersDefaultCellStyle.ForeColor);
            var textFormat = new StringFormat()
            {
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Far
            };

            var bounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dgv.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowHeaderText, this.Font, brush, bounds, textFormat);
        }

        private void DataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var firstRowIndex = dataGridViewQuestions.FirstDisplayedCell.RowIndex;
            var lastRowIndex = firstRowIndex + dataGridViewQuestions.RowCount;


            Graphics Graphics = dataGridViewQuestions.CreateGraphics();
            int measureFirst = (int)(Graphics.MeasureString(firstRowIndex.ToString(), dataGridViewQuestions.Font).Width);
            int measureLast = (int)(Graphics.MeasureString(lastRowIndex.ToString(), dataGridViewQuestions.Font).Width);

            int rowHeaderWitdth = Math.Max(measureFirst, measureLast);
            dataGridViewQuestions.RowHeadersWidth = rowHeaderWitdth + 15;
        }

        // Toolstrip items
        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = savedWindowState;
        }

        private void ToolStripMenuClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ToolStripMenuRestore_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = savedWindowState;
        }

        // Menu Items
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFileName == null)
            {
                saveFileDialog1.Title = "Save questions to file";
                saveFileDialog1.CheckPathExists = true;
                saveFileDialog1.DefaultExt = "xml";
                saveFileDialog1.Filter = "xml files (*.xml)|*.xml";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    currentFileName = saveFileDialog1.FileName;
                    if (SaveQuestionsToXML(currentFileName))
                    {
                        MessageBox.Show("You have successfully saved the questions to file.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                if (SaveQuestionsToXML(currentFileName))
                {
                    MessageBox.Show("You have successfully saved the questions to file.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save questions to file";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "xml";
            saveFileDialog1.Filter = "xml files (*.xml)|*.xml";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentFileName = saveFileDialog1.FileName;
                if (SaveQuestionsToXML(currentFileName))
                {
                    MessageBox.Show("You have successfully saved the questions to file.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void OpenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //should check if existing data and save
            DialogResult dialogResult = DialogResult.No;
            if (unsavedQuestions)
            {
                const string message = "You have unsaved questions, cancel?";
                const string caption = "Warning";
                dialogResult = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
            }

            // If the no button was pressed ...
            if (dialogResult == DialogResult.No)
            {
                Datatable.Clear();
                unsavedQuestions = false;
                openFileDialog1.Title = "Open existing questions file";
                openFileDialog1.FileName = "";
                openFileDialog1.CheckPathExists = true;
                openFileDialog1.DefaultExt = "xml";
                openFileDialog1.Filter = "xml files (*.xml)|*.xml";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    currentFileName = openFileDialog1.FileName;
                    LoadQuestionsFromXML(currentFileName);
                }
            }
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //should check if existing data and save
            DialogResult dialogResult = DialogResult.Yes;
            if (unsavedQuestions)
            {
                const string message = "You have unsaved questions, are you sure you want to reset?";
                const string caption = "Warning";
                dialogResult = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
            }
            // If the no button was pressed ...
            if (dialogResult == DialogResult.Yes)
            {
                Datatable.Clear();
                unsavedQuestions = false;
                numCounter.Value = 1;
                Clipboard.Clear();
                pictureBox1.Image = null;
                currentFileName = null;
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AddcatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using FormAddCategory addCatForm = new();
            addCatForm.ShowDialog();
            if (addCatForm.NewCategory != "")
            {
                if (this.tabControlSubject.SelectedTab == this.tabControlSubject.TabPages["tabPageGCSE"])
                {
                    try
                    {
                        treeViewGCSE.BeginUpdate();
                        treeViewGCSE.Nodes.Add(addCatForm.NewCategory);
                        treeViewGCSE.Sort();
                        treeViewGCSE.EndUpdate();
                        SaveTreeView(this.treeViewGCSE, Application.StartupPath + "\\GCSECategories.xml");
                    }
                    catch
                    {
                        MessageBox.Show("Error", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        treeViewALevel.BeginUpdate();
                        treeViewALevel.Nodes.Add(addCatForm.NewCategory);
                        treeViewALevel.Sort();
                        treeViewALevel.EndUpdate();
                        SaveTreeView(this.treeViewALevel, Application.StartupPath + "\\ALevelCategories.xml");
                    }
                    catch
                    {
                        MessageBox.Show("Error", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void AddsubcatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> nodeList = new()
            {
                "Please select a category..."
            };
            if (this.tabControlSubject.SelectedTab == this.tabControlSubject.TabPages["tabPageGCSE"])
            {
                foreach (TreeNode tn in treeViewGCSE.Nodes)
                {
                    nodeList.Add(tn.Text);
                }
            }
            else
            {
                foreach (TreeNode tn in treeViewALevel.Nodes)
                {
                    nodeList.Add(tn.Text);
                }
            }

            using FormAddSubCategory addSubCatForm = new(nodeList);
            addSubCatForm.ShowDialog();
            if (addSubCatForm.NewSubCategory != "")
            {
                if (this.tabControlSubject.SelectedTab == this.tabControlSubject.TabPages["tabPageGCSE"])
                {
                    try
                    {
                        TreeNode childNode = new(addSubCatForm.NewSubCategory);
                        treeViewGCSE.BeginUpdate();
                        treeViewGCSE.SelectedNode = GetNodeByText(treeViewGCSE.Nodes, addSubCatForm.Category);
                        treeViewGCSE.SelectedNode.Nodes.Add(childNode);
                        treeViewGCSE.Sort();
                        treeViewGCSE.EndUpdate();
                        SaveTreeView(this.treeViewGCSE, Application.StartupPath + "\\GCSECategories.xml");
                    }
                    catch
                    {
                        MessageBox.Show("Error", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        TreeNode childNode = new(addSubCatForm.NewSubCategory);
                        treeViewALevel.BeginUpdate();
                        treeViewALevel.SelectedNode = GetNodeByText(treeViewALevel.Nodes, addSubCatForm.Category);
                        treeViewALevel.SelectedNode.Nodes.Add(childNode);
                        treeViewALevel.Sort();
                        treeViewALevel.EndUpdate();
                        SaveTreeView(this.treeViewALevel, Application.StartupPath + "\\ALevelCategories.xml");
                    }
                    catch
                    {
                        MessageBox.Show("Error", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeViewGCSE.SelectedNode != null || treeViewALevel.SelectedNode != null)
            {
                DialogResult dl = MessageBox.Show("Are you sure you wish to delete the selected item?", FormMain.ActiveForm.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dl == DialogResult.Yes)
                {
                    if (this.tabControlSubject.SelectedTab == this.tabControlSubject.TabPages["tabPageGCSE"])
                    {
                        try
                        {
                            treeViewGCSE.BeginUpdate();
                            treeViewGCSE.Nodes.Remove(treeViewGCSE.SelectedNode);
                            treeViewGCSE.Sort();
                            treeViewGCSE.EndUpdate();
                            SaveTreeView(this.treeViewGCSE, Application.StartupPath + "\\GCSECategories.xml");
                        }
                        catch
                        {
                            MessageBox.Show("Error", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        try
                        {
                            treeViewALevel.BeginUpdate();
                            treeViewALevel.Nodes.Remove(treeViewALevel.SelectedNode);
                            treeViewALevel.Sort();
                            treeViewALevel.EndUpdate();
                            SaveTreeView(this.treeViewALevel, Application.StartupPath + "\\ALevelCategories.xml");
                        }
                        catch
                        {
                            MessageBox.Show("Error", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No item selected", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SaveTreeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tabControlSubject.SelectedTab == this.tabControlSubject.TabPages["tabPageGCSE"])
            {
                try
                {
                    SaveTreeView(this.treeViewGCSE, Application.StartupPath + "\\GCSECategories.xml");
                }
                catch
                {
                    MessageBox.Show("Error", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    SaveTreeView(this.treeViewALevel, Application.StartupPath + "\\ALevelCategories.xml");
                }
                catch
                {
                    MessageBox.Show("Error", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using FormAbout aboutForm = new();
            aboutForm.ShowDialog();
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data!.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy; //not null
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data!.GetData(DataFormats.FileDrop); //not null
            //Only allow one file at a time to be opened
            if (files.Length == 1)
            {
                if (Path.GetExtension(files[0]).ToLower() == ".xml")
                {
                    //should check if existing data and save
                    DialogResult dialogResult = DialogResult.No;
                    if (unsavedQuestions)
                    {
                        const string message = "You have unsaved questions, cancel?";
                        const string caption = "Warning";
                        dialogResult = MessageBox.Show(message, caption,
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);
                    }

                    // If the no button was pressed ...
                    if (dialogResult == DialogResult.No)
                    {
                        Datatable.Clear();
                        unsavedQuestions = false;
                        currentFileName = files[0];
                        //LoadQuestionsFromCSV();
                        LoadQuestionsFromXML(currentFileName);
                    }
                }
                else
                {
                    MessageBox.Show("File " + files[0] + " is not an xml file, please try another file.", ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewQuestions.SelectedRows)
            {
                if (row.Index < dataGridViewQuestions.RowCount - 1)
                {
                    dataGridViewQuestions.Rows.RemoveAt(row.Index);
                    unsavedQuestions = true;
                }
            }

            /*
            //Save updated questions to file
            if (unsavedQuestions && currentFileName != null)
            {
                const string message = "Save the changes to file?";
                const string caption = "Information";
                if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SaveQuestionsToCSV();
                }
            }
            */
        }

        private void ViewSelectedQuestionImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewSelectedQuestionImage();
        }

        private void TabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedIndex == 0)
            {
                toolsToolStripMenuItem.Enabled = true;
                questionsToolStripMenuItem.Enabled = false;
            }
            else
            {
                toolsToolStripMenuItem.Enabled = false;
                questionsToolStripMenuItem.Enabled = true;
            }
        }

        private void DataGridViewQuestions_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ViewSelectedQuestionImage();
        }

        private void SharpClipboard1_ClipboardChanged(object sender, WK.Libraries.SharpClipboardNS.SharpClipboard.ClipboardChangedEventArgs e)
        {
            // Is the content copied of image type?
            if (e.ContentType == SharpClipboard.ContentTypes.Image)
            {
                // Get the cut/copied image.
                pictureBox1.Image = sharpClipboard1.ClipboardImage;
                Show();
                btnSaveImg.Focus();
                TopMost = true;
                Focus();
                BringToFront();
                TopMost = false;
                tabControlMain.SelectTab(0);
            }
        }

        // Functions
        private void UpdateFileName()
        {
            string fileNumber;
            if (this.numCounter.Value < 10)
            {
                fileNumber = "0" + this.numCounter.Value.ToString();
            }
            else
            {
                fileNumber = this.numCounter.Value.ToString();
            }
            txtQuestionName.Text = txtExamPaperName.Text + " q" + fileNumber;
        }

        private void ViewSelectedQuestionImage()
        {
            if (dataGridViewQuestions.SelectedRows.Count == 1)
            {
                int selectedRowIndex = dataGridViewQuestions.SelectedRows[0].Index;

                if (selectedRowIndex < dataGridViewQuestions.RowCount - 1)
                {
                    //Datarow is the selected item
                    DataRow dataRow = ((DataRowView)dataGridViewQuestions.SelectedRows[0].DataBoundItem).Row;
//                    DataRow dataRow = Datatable.Rows[selectedRowIndex];

                    //stop monitoring clipboard
                    sharpClipboard1.MonitorClipboard = false;

                    //create instance of form
                    using (FormEditQuestion formEditQuestion = new())
                    {
                        //pass the selected row from datatable to the form
                        formEditQuestion.QuestionRow = dataRow;

                        //create a datatable of the current treeview
                        formEditQuestion.QuestionSubject = TreeViewtoSubject(treeViewALevel);

                        //show the form
                        if (formEditQuestion.ShowDialog(this) == DialogResult.OK)
                        {
                            if (formEditQuestion.QuestionDataChanged)
                            {
                                unsavedQuestions = true;
                                if (currentFileName != null)
                                {
                                    const string message = "Save the changes to file?";
                                    const string caption = "Information";
                                    if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        SaveQuestionsToXML(currentFileName);
                                    }
                                }
                                Clipboard.Clear();
                            }
                        }
                    }

                    //start monitoring clipboard
                    sharpClipboard1.MonitorClipboard = true;

                }
            }
            else
            {
                MessageBox.Show("Please select a single question row..", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private TreeNode GetNodeByText(TreeNodeCollection nodes, string searchtext)
        {
            TreeNode n_found_node;
            bool b_node_found = false;

            foreach (TreeNode node in nodes)
            {

                if (node.Text == searchtext)
                {
                    b_node_found = true;
                    n_found_node = node;

                    return n_found_node;
                }

                if (!b_node_found)
                {
                    n_found_node = GetNodeByText(node.Nodes, searchtext);

                    if (n_found_node != null)
                    {
                        return n_found_node;
                    }
                }
            }
            return null;
        }

        private static void LoadTreeView(TreeView treeView, string filename)
        {
            // Create an instance of the XmlSerializer class;
            // specify the type of object to be deserialized.
            XmlSerializer serializer = new(typeof(Subject));

            // Declare an object variable of the type to be deserialized.
            Subject subject;

            /* Use the Deserialize method to restore the object's state with
            data from the XML document. */
            using (Stream reader = new FileStream(filename, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                subject = (Subject)serializer.Deserialize(reader);
            }

            // disabling re-drawing of treeview till all nodes are added
            treeView.BeginUpdate();
            foreach (SubjectTopic topic in subject!.Items) //not null
            {
                TreeNode parentNode = new(topic.name);
                treeView.Nodes.Add(parentNode);
                foreach (SubjectTopicCategory category in topic.Category)
                {
                    parentNode.Nodes.Add(category.name);
                }

            }
            treeView.EndUpdate();
        }

        private static void SaveTreeView(TreeView treeView, string filename)
        {
            //Object representation of xml
            Subject subject = TreeViewtoSubject(treeView);

            // Create an instance of the XmlSerializer class;
            // specify the type of object to serialize.
            XmlSerializer serializer = new(typeof(Subject));

            //Set xml settings
            XmlWriterSettings settings = new()
            {
                Indent = true,
            };

            // Create the XmlWriter object and write some content.
            using XmlWriter streamWriter = XmlWriter.Create(filename, settings);
            serializer.Serialize(streamWriter, subject);
        }

        private static Subject TreeViewtoSubject(TreeView treeView)
        {
            Subject subject = new();

            List<SubjectTopic> topics = new();
            foreach (TreeNode parent in treeView.Nodes)
            {
                List<SubjectTopicCategory> categories = new();
                foreach (TreeNode child in parent.Nodes)
                {
                    SubjectTopicCategory category = new()
                    {
                        name = child.Text
                    };
                    categories.Add(category);
                }
                SubjectTopic topic = new()
                {
                    name = parent.Text,
                    Category = categories
                };
                topics.Add(topic);
            }

            subject.Items = topics;

            return subject;
        }

        private void SaveImageToQuestion()
        {
            if (pictureBox1.Image != null)
            {
                TreeNode tn;
                if (tabControlSubject.SelectedTab == tabControlSubject.TabPages["tabPageGCSE"])
                {
                    tn = treeViewGCSE.SelectedNode;
                }
                else
                {
                    tn = treeViewALevel.SelectedNode;
                }
                // check node is selected
                if (tn != null && tn.Parent != null)
                {
                    if (comboBoxAnswers.Text != "Select")
                    {
                        DataRow myDataRow = Datatable.NewRow();
                        myDataRow["Question Topic"] = tn.Parent.Text;
                        myDataRow["Question Category"] = tn.Text;
                        myDataRow["Question Name"] = txtQuestionName.Text;
                        myDataRow["Question Image"] = ClassUtils.ImageToByte(pictureBox1.Image);
                        myDataRow["Question Answer"] = comboBoxAnswers.Text;
                        Datatable.Rows.Add(myDataRow);

                        numCounter.Value++;
                        savedWindowState = WindowState;
                        Hide();
                        notifyIcon1.Visible = true;
                        comboBoxAnswers.Text = "Select";
                        if (currentFileName != null)
                        {
                            SaveQuestionsToXML(currentFileName);
                        }
                        else
                        {
                            unsavedQuestions = true;                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select an answer", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a topic category", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an image", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadQuestionsFromXML(string filename)
        {
            try
            {
                // Create an instance of the XmlSerializer class;
                // specify the type of object to be deserialized.
                XmlSerializer serializer = new(typeof(quiz));

                // Declare an object variable of the type to be deserialized.
                quiz quizdata;

                /* Use the Deserialize method to restore the object's state with
                data from the XML document. */
                using (Stream reader = new FileStream(filename, FileMode.Open))
                {
                    // Call the Deserialize method to restore the object's state.
                    quizdata = (quiz)serializer.Deserialize(reader);
                }

                //now step through data and display in datatable
                string questionTopic = "";
                string questionCategory = "";

                foreach (quizQuestion question in quizdata!.Items) //not null
                {
                    switch (question.type)
                    {
                        case "category":
                            //get the first element
                            string questionTopicCategory = question.category.First().text;
                            //remove standard location
                            questionTopicCategory = questionTopicCategory.Replace("$course$/", "");
                            questionTopicCategory = questionTopicCategory.Replace("top/", "");
                            //split string into Topic and Category
                            string[] questionTopicCategoryList = questionTopicCategory.Split('/');

                            switch (questionTopicCategoryList.Length)
                            {
                                case 1: //only contains topic
                                    questionTopic = questionTopicCategoryList[0];
                                    break;
                                case 2: //contains both topic and category
                                    questionTopic = questionTopicCategoryList[0];
                                    questionCategory = questionTopicCategoryList[1];
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case "multichoice":
                            //Create a new datarow
                            DataRow myDataRow = this.Datatable.NewRow();
                            myDataRow["Question Topic"] = questionTopic;
                            myDataRow["Question Category"] = questionCategory;
                            myDataRow["Question Name"] = question.name.First().text;
                            myDataRow["Question Image"] = Convert.FromBase64String(
                                Regex.Match(question.questiontext.First().text, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value.Replace("data:image/png;base64,", "")
                                );
                            foreach (quizQuestionAnswer answer in question.answer)
                            {
                                if (answer.fraction.Equals("100"))
                                {
                                    myDataRow["Question Answer"] = answer.text;
                                }
                            }
                            this.Datatable.Rows.Add(myDataRow);
                            break;

                        default:
                            break;
                    }
                }
                MessageBox.Show("You have successfully loaded the questions from file.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                unsavedQuestions = false;
                tabControlMain.SelectTab(1);
                dataGridViewQuestions.Rows[0].Selected = true;
            }
            catch (IOException)
            {
                MessageBox.Show("File " + currentFileName + " is open in another process, please close the file and try again", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                MessageBox.Show("File " + currentFileName + " is not valid, please check the file and try again", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool SaveQuestionsToXML(string filename)
        {
            try
            {
                //Object representation of xml
                quiz quiz = new();

                string[] answers = { "A", "B", "C", "D" };

                List<quizQuestion> questions = new();
                foreach (DataRow row in this.Datatable.Rows)
                {
                    quizQuestion question;

                    //create a new question with a category type
                    question = new()
                    {
                        type = "category",
                        category = new() { new quizQuestionCategory() { text = "$course$/top/" + (string)row["Question Topic"] + "/" + (string)row["Question Category"] } },
                        info = new() { new quizQuestionInfo() { format = "moodle_auto_format", text = "" } },
                        idnumber = "",
                    };
                    questions.Add(question);

                    //create a new question with a multichoice type
                    question = new()
                    {
                        type = "multichoice",
                        name = new() { new quizQuestionName() { text = (string)row["Question Name"] } },
                        questiontext = new()
                        {
                            new quizQuestionQuestiontext()
                            {
                                format = "html",
                                text = "<p><img src=\"data:image/png;base64," +
                                    Convert.ToBase64String((byte[])row["Question Image"]) +
                                    "\" alt=\"" +
                                    (string)row["Question Name"] +
                                    "\" class=\"img-responsive\" /></p>",
                            }
                        },
                        generalfeedback = new() { new quizQuestionGeneralfeedback() { text = "" } },
                        defaultgrade = "1.0000000",
                        penalty = ".0000000",
                        hidden = "0",
                        idnumber = "",
                        single = "true",
                        shuffleanswers = "false",
                        answernumbering = "none",
                        showstandardinstruction = "1",
                        correctfeedback = new() { new quizQuestionCorrectfeedback() { format = "html", text = "" } },
                        partiallycorrectfeedback = new() { new quizQuestionPartiallycorrectfeedback() { format = "html", text = "" } },
                        incorrectfeedback = new() { new quizQuestionIncorrectfeedback() { format = "html", text = "" } },
                    };

                    //Add answers
                    List<quizQuestionAnswer> questionAnswers = new();
                    foreach (string answer in answers)
                    {
                        quizQuestionAnswer questionAnswer = new()
                        {
                            format = "html",
                            text = answer,
                            feedback = new() { new quizQuestionAnswerFeedback() { format = "html", text = "" } },
                        };
                        if (answer == (string)row["Question Answer"]) //correct answer found
                        {
                            questionAnswer.fraction = "100";
                        }
                        else
                        {
                            questionAnswer.fraction = "0";
                        }
                        questionAnswers.Add(questionAnswer);
                    }
                    question.answer = questionAnswers;
                    questions.Add(question);
                }

                //Add all questions to quiz
                quiz.Items = questions;

                // Create an instance of the XmlSerializer class;
                // specify the type of object to serialize.
                XmlSerializer serializer = new(typeof(quiz));

                //Set xml settings
                XmlWriterSettings settings = new()
                {
                    Indent = true,
                    IndentChars = ("\t"),
                    OmitXmlDeclaration = false,
                    Encoding = Encoding.UTF8,
                };

                // Create the XmlWriter object and write some content.
                using (XmlWriter streamWriter = XmlWriter.Create(filename, settings))
                {
                    serializer.Serialize(streamWriter, quiz);
                    streamWriter.Flush();
                }

                //Hack to add comments
                XmlDocument doc = new();
                doc.Load(filename);
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodes = root!.SelectNodes("question");
                int counter = 0;
                foreach (XmlNode node in nodes!)
                {
                    XmlComment newComment;                    
                    if (node.Attributes!["type"]!.Value.Equals("category"))
                        newComment = doc.CreateComment("question: 0");
                    else
                        newComment = doc.CreateComment("question: " + ++counter);
                    //Add the new node to the document.
                    doc.DocumentElement!.InsertBefore(newComment, node);
                }
                doc.Save(filename);

                // all done return
                unsavedQuestions = false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void LoadQuestionsFromCSV()
        {
            try
            {
                using (var reader = new StreamReader(currentFileName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var record = csv.GetRecord<ClassMCQRecord>();
                        DataRow myDataRow = Datatable.NewRow();
                        myDataRow["Question Topic"] = record.Topic;
                        myDataRow["Question Category"] = record.Category;
                        myDataRow["Question Name"] = record.Name;
                        myDataRow["Question Image"] = Convert.FromBase64String(record.Image);
                        myDataRow["Question Answer"] = record.Answer;
                        Datatable.Rows.Add(myDataRow);
                    }
                }
                MessageBox.Show("You have successfully loaded the questions from file.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                unsavedQuestions = false;
                tabControlMain.SelectTab(1);
                dataGridViewQuestions.Rows[0].Selected = true;
            }
            catch (CsvHelper.MissingFieldException)
            {
                MessageBox.Show("File " + currentFileName + " is not valid, please check the file and try again", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("File " + currentFileName + " is open in another process, please close the file and try again", FormMain.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool SaveQuestionsToCSV()
        {
            try
            {
                using (var writer = File.CreateText(currentFileName))
                {
                    using CsvWriter csv = new(writer, CultureInfo.InvariantCulture);
                    csv.WriteHeader<ClassMCQRecord>();
                    csv.NextRecord();
                    // Write row values
                    foreach (DataRow row in Datatable.Rows)
                    {
                        var newRecord = new ClassMCQRecord
                        {
                            Topic = (string)row["Question Topic"],
                            Category = (string)row["Question Category"],
                            Name = (string)row["Question Name"],
                            Image = Convert.ToBase64String((byte[])row["Question Image"]),
                            Answer = (string)row["Question Answer"]
                        };
                        csv.WriteRecord(newRecord);
                        csv.NextRecord();
                    }
                }
                unsavedQuestions = false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        //Method to export to Moodle XML format
        private void ExportMoodleXMLformat()
        {
            string xmlFileName = Path.GetFileNameWithoutExtension(currentFileName) + ".xml";

            saveFileDialog1.Title = "Export questions to Moodle XML";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "xml";
            saveFileDialog1.Filter = "xml files (*.xml)|*.xml";
            saveFileDialog1.FileName = xmlFileName;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Set xml settings
                XmlWriterSettings settings = new()
                {
                    Indent = true,
                    IndentChars = ("    "),
                    CloseOutput = true,
                    OmitXmlDeclaration = false
                };
                try
                {
                    using (XmlWriter writer = XmlWriter.Create((saveFileDialog1.FileName), settings))
                    {
                        //Initialize file
                        writer.WriteStartDocument();
                        writer.WriteStartElement("quiz");

                        string[] answers = { "A", "B", "C", "D" };
                        int questionNumber = 0;

                        //Multiple choice questions
                        foreach (DataRow row in Datatable.Rows)
                        {
                            //increment question number
                            questionNumber++;

                            //Write category of question
                            writer.WriteComment(" question: 0 ");
                            writer.WriteStartElement("question");
                            writer.WriteAttributeString("type", "category");
                            writer.WriteStartElement("category");
                            writer.WriteStartElement("text");
                            writer.WriteString("$course$/" + (string)row["Question Topic"] + "/" + (string)row["Question Category"]);
                            writer.WriteFullEndElement();
                            writer.WriteFullEndElement();
                            writer.WriteStartElement("info");
                            writer.WriteAttributeString("format", "moodle_auto_format");
                            writer.WriteStartElement("text");
                            writer.WriteFullEndElement();
                            writer.WriteFullEndElement();
                            writer.WriteStartElement("idnumber");
                            writer.WriteFullEndElement();
                            writer.WriteFullEndElement();

                            //Write Comment
                            writer.WriteComment(" question: " + questionNumber + " ");

                            //Write Question type
                            writer.WriteStartElement("question");
                            writer.WriteAttributeString("type", "multichoice");

                            //Write Question name
                            writer.WriteStartElement("name");
                            writer.WriteStartElement("text");
                            writer.WriteString((string)row["Question Name"]);
                            writer.WriteFullEndElement();
                            writer.WriteFullEndElement();

                            //Write Questiontext
                            writer.WriteStartElement("questiontext");
                            writer.WriteAttributeString("format", "html");
                            writer.WriteStartElement("text");
                            writer.WriteCData("<p><img src=\"data:image/png;base64," + Convert.ToBase64String((byte[])row["Question Image"]) + "\" alt=\"" + (string)row["Question Name"] + "\" class=\"img-responsive\" /></p>");
                            writer.WriteFullEndElement();
                            writer.WriteFullEndElement();

                            //Write GeneralFeedback
                            writer.WriteStartElement("generalfeedback");
                            writer.WriteAttributeString("format", "html");
                            writer.WriteStartElement("text");
                            writer.WriteFullEndElement();
                            writer.WriteFullEndElement();

                            //Write Properties
                            writer.WriteStartElement("defaultgrade");
                            writer.WriteString("1.0000000");
                            writer.WriteFullEndElement();
                            writer.WriteStartElement("penalty");
                            writer.WriteString(".0000000");
                            writer.WriteFullEndElement();
                            writer.WriteStartElement("hidden");
                            writer.WriteString("0");
                            writer.WriteFullEndElement();
                            writer.WriteStartElement("idnumber");
                            writer.WriteFullEndElement();
                            writer.WriteStartElement("single");
                            writer.WriteString("true");
                            writer.WriteFullEndElement();
                            writer.WriteStartElement("shuffleanswers");
                            writer.WriteString("false");
                            writer.WriteFullEndElement();
                            writer.WriteStartElement("answernumbering");
                            writer.WriteString("none");
                            writer.WriteFullEndElement();
                            writer.WriteStartElement("showstandardinstruction");
                            writer.WriteString("1");
                            writer.WriteFullEndElement();
                            writer.WriteStartElement("correctfeedback");
                            writer.WriteAttributeString("format", "html");
                            writer.WriteStartElement("text");
                            writer.WriteString("");
                            writer.WriteFullEndElement();
                            writer.WriteFullEndElement();
                            writer.WriteStartElement("partiallycorrectfeedback");
                            writer.WriteAttributeString("format", "html");
                            writer.WriteStartElement("text");
                            writer.WriteString("");
                            writer.WriteFullEndElement();
                            writer.WriteFullEndElement();
                            writer.WriteStartElement("incorrectfeedback");
                            writer.WriteAttributeString("format", "html");
                            writer.WriteStartElement("text");
                            writer.WriteString("");
                            writer.WriteFullEndElement();
                            writer.WriteFullEndElement();

                            //Write Answer
                            for (int j = 0; j < answers.Length; j++)
                            {
                                if (answers[j] == (string)row["Question Answer"]) //correct answer found
                                {
                                    writer.WriteStartElement("answer");
                                    writer.WriteAttributeString("fraction", "100");
                                    writer.WriteAttributeString("format", "html");
                                    writer.WriteStartElement("text");
                                    writer.WriteString(answers[j]);
                                    writer.WriteFullEndElement();
                                    writer.WriteStartElement("feedback");
                                    writer.WriteAttributeString("format", "html");
                                    writer.WriteStartElement("text");
                                    writer.WriteString("Correct");
                                    writer.WriteFullEndElement();
                                    writer.WriteFullEndElement();
                                    writer.WriteFullEndElement();
                                }
                                else
                                {
                                    writer.WriteStartElement("answer");
                                    writer.WriteAttributeString("fraction", "0");
                                    writer.WriteAttributeString("format", "html");
                                    writer.WriteStartElement("text");
                                    writer.WriteString(answers[j]);
                                    writer.WriteFullEndElement();
                                    writer.WriteStartElement("feedback");
                                    writer.WriteAttributeString("format", "html");
                                    writer.WriteStartElement("text");
                                    writer.WriteString("Sorry! Try again.");
                                    writer.WriteFullEndElement();
                                    writer.WriteFullEndElement();
                                    writer.WriteFullEndElement();
                                }
                            }

                            //End question
                            writer.WriteFullEndElement();
                        }

                        //End XML file
                        writer.WriteFullEndElement();
                        writer.WriteEndDocument();
                        writer.Flush();
                    }
                    MessageBox.Show("You have successfully exported the questions to Moodle XML.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    unsavedQuestions = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
