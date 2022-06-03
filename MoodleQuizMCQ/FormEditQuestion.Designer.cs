namespace MoodleQuizMCQ
{
    partial class FormEditQuestion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditQuestion));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.textBoxImageBase64 = new System.Windows.Forms.TextBox();
            this.buttonPaste = new System.Windows.Forms.Button();
            this.buttonShowImageString = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.textBoxQuestionName = new System.Windows.Forms.TextBox();
            this.buttonBack = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonUpdateQuestion = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxAnswer = new System.Windows.Forms.ComboBox();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.comboBoxTopic = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonDeleteQuestion = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(749, 467);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 27);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(657, 467);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 27);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxImage.Location = new System.Drawing.Point(174, 12);
            this.pictureBoxImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(662, 312);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImage.TabIndex = 6;
            this.pictureBoxImage.TabStop = false;
            // 
            // textBoxImageBase64
            // 
            this.textBoxImageBase64.Location = new System.Drawing.Point(169, 575);
            this.textBoxImageBase64.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxImageBase64.Multiline = true;
            this.textBoxImageBase64.Name = "textBoxImageBase64";
            this.textBoxImageBase64.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxImageBase64.Size = new System.Drawing.Size(662, 56);
            this.textBoxImageBase64.TabIndex = 7;
            this.textBoxImageBase64.TextChanged += new System.EventHandler(this.TextBoxtextBoxImageBase64_TextChanged);
            // 
            // buttonPaste
            // 
            this.buttonPaste.Location = new System.Drawing.Point(10, 298);
            this.buttonPaste.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonPaste.Name = "buttonPaste";
            this.buttonPaste.Size = new System.Drawing.Size(156, 27);
            this.buttonPaste.TabIndex = 10;
            this.buttonPaste.Text = "Paste New Image";
            this.buttonPaste.UseVisualStyleBackColor = true;
            this.buttonPaste.Click += new System.EventHandler(this.ButtonPaste_Click);
            // 
            // buttonShowImageString
            // 
            this.buttonShowImageString.Location = new System.Drawing.Point(4, 605);
            this.buttonShowImageString.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonShowImageString.Name = "buttonShowImageString";
            this.buttonShowImageString.Size = new System.Drawing.Size(156, 27);
            this.buttonShowImageString.TabIndex = 11;
            this.buttonShowImageString.Text = "Encode to base64 string";
            this.buttonShowImageString.UseVisualStyleBackColor = true;
            this.buttonShowImageString.Click += new System.EventHandler(this.ButtonShowImageString_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(267, 467);
            this.buttonNext.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(88, 27);
            this.buttonNext.TabIndex = 12;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.ButtonNext_Click);
            // 
            // textBoxQuestionName
            // 
            this.textBoxQuestionName.Location = new System.Drawing.Point(174, 402);
            this.textBoxQuestionName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxQuestionName.Name = "textBoxQuestionName";
            this.textBoxQuestionName.Size = new System.Drawing.Size(445, 23);
            this.textBoxQuestionName.TabIndex = 15;
            this.textBoxQuestionName.TextChanged += new System.EventHandler(this.TextBoxQuestionData_TextChanged);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(174, 467);
            this.buttonBack.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(88, 27);
            this.buttonBack.TabIndex = 17;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.ButtonBack_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 373);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 15);
            this.label2.TabIndex = 18;
            this.label2.Text = "Question Category";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 339);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 15);
            this.label3.TabIndex = 19;
            this.label3.Text = "Question Topic";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(74, 406);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 15);
            this.label4.TabIndex = 20;
            this.label4.Text = "Question Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(65, 440);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 15);
            this.label5.TabIndex = 21;
            this.label5.Text = "Question Answer";
            // 
            // buttonUpdateQuestion
            // 
            this.buttonUpdateQuestion.Location = new System.Drawing.Point(393, 467);
            this.buttonUpdateQuestion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonUpdateQuestion.Name = "buttonUpdateQuestion";
            this.buttonUpdateQuestion.Size = new System.Drawing.Size(111, 27);
            this.buttonUpdateQuestion.TabIndex = 22;
            this.buttonUpdateQuestion.Text = "Update Question";
            this.buttonUpdateQuestion.UseVisualStyleBackColor = true;
            this.buttonUpdateQuestion.Click += new System.EventHandler(this.ButtonUpdateQuestion_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonDeleteQuestion);
            this.panel1.Controls.Add(this.comboBoxAnswer);
            this.panel1.Controls.Add(this.comboBoxCategory);
            this.panel1.Controls.Add(this.comboBoxTopic);
            this.panel1.Controls.Add(this.buttonUpdateQuestion);
            this.panel1.Controls.Add(this.buttonBack);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.buttonNext);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.buttonPaste);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.textBoxQuestionName);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.pictureBoxImage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(853, 510);
            this.panel1.TabIndex = 23;
            // 
            // comboBoxAnswer
            // 
            this.comboBoxAnswer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnswer.FormattingEnabled = true;
            this.comboBoxAnswer.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D"});
            this.comboBoxAnswer.Location = new System.Drawing.Point(174, 435);
            this.comboBoxAnswer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBoxAnswer.Name = "comboBoxAnswer";
            this.comboBoxAnswer.Size = new System.Drawing.Size(445, 23);
            this.comboBoxAnswer.TabIndex = 25;
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Location = new System.Drawing.Point(174, 368);
            this.comboBoxCategory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(445, 23);
            this.comboBoxCategory.TabIndex = 24;
            // 
            // comboBoxTopic
            // 
            this.comboBoxTopic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTopic.FormattingEnabled = true;
            this.comboBoxTopic.Location = new System.Drawing.Point(174, 335);
            this.comboBoxTopic.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBoxTopic.Name = "comboBoxTopic";
            this.comboBoxTopic.Size = new System.Drawing.Size(445, 23);
            this.comboBoxTopic.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(72, 12);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "Question Image";
            // 
            // buttonDeleteQuestion
            // 
            this.buttonDeleteQuestion.Location = new System.Drawing.Point(508, 467);
            this.buttonDeleteQuestion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonDeleteQuestion.Name = "buttonDeleteQuestion";
            this.buttonDeleteQuestion.Size = new System.Drawing.Size(111, 27);
            this.buttonDeleteQuestion.TabIndex = 26;
            this.buttonDeleteQuestion.Text = "Delete Question";
            this.buttonDeleteQuestion.UseVisualStyleBackColor = true;
            this.buttonDeleteQuestion.Click += new System.EventHandler(this.ButtonDeleteQuestion_Click);
            // 
            // FormEditQuestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 522);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonShowImageString);
            this.Controls.Add(this.textBoxImageBase64);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FormEditQuestion";
            this.ShowIcon = false;
            this.Text = "Question Image Viewer";
            this.Load += new System.EventHandler(this.FormEditQuestion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.TextBox textBoxImageBase64;
        private System.Windows.Forms.Button buttonPaste;
        private System.Windows.Forms.Button buttonShowImageString;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.TextBox textBoxQuestionName;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonUpdateQuestion;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxTopic;
        private System.Windows.Forms.ComboBox comboBoxCategory;
        private System.Windows.Forms.ComboBox comboBoxAnswer;
        private Button buttonDeleteQuestion;
    }
}