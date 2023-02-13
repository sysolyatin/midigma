namespace MidiToGrandMA
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.midiChangeStateBtn = new System.Windows.Forms.Button();
            this.midiDevicesComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gmaIpTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gmaConnectBtn = new System.Windows.Forms.Button();
            this.gmaPasswordTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gmaLoginTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.midiUpdateBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.selectHardkeyBtn = new System.Windows.Forms.Button();
            this.pageNumberLabel = new System.Windows.Forms.Label();
            this.pageNumberTextBox = new System.Windows.Forms.TextBox();
            this.executorNumberTextBox = new System.Windows.Forms.TextBox();
            this.cmdContentComboBox = new System.Windows.Forms.ComboBox();
            this.midicmdSaveBtn = new System.Windows.Forms.Button();
            this.midiLearnBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdContentTextBox = new System.Windows.Forms.TextBox();
            this.cmdContentLabel = new System.Windows.Forms.Label();
            this.cmdTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.commandsDataGridView = new System.Windows.Forms.DataGridView();
            this.MidiId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Executor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveProjectBtn = new System.Windows.Forms.Button();
            this.openProjectBtn = new System.Windows.Forms.Button();
            this.aboutBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.commandsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // midiChangeStateBtn
            // 
            this.midiChangeStateBtn.BackColor = System.Drawing.Color.Black;
            this.midiChangeStateBtn.FlatAppearance.BorderColor = System.Drawing.Color.Goldenrod;
            this.midiChangeStateBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(155)))), ((int)(((byte)(25)))));
            this.midiChangeStateBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(155)))), ((int)(((byte)(25)))));
            this.midiChangeStateBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.midiChangeStateBtn.Location = new System.Drawing.Point(9, 75);
            this.midiChangeStateBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.midiChangeStateBtn.Name = "midiChangeStateBtn";
            this.midiChangeStateBtn.Size = new System.Drawing.Size(267, 35);
            this.midiChangeStateBtn.TabIndex = 0;
            this.midiChangeStateBtn.Text = "Подключиться";
            this.midiChangeStateBtn.UseVisualStyleBackColor = false;
            this.midiChangeStateBtn.Click += new System.EventHandler(this.midiChangeStateBtn_Click);
            // 
            // midiDevicesComboBox
            // 
            this.midiDevicesComboBox.BackColor = System.Drawing.Color.Black;
            this.midiDevicesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.midiDevicesComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.midiDevicesComboBox.ForeColor = System.Drawing.Color.White;
            this.midiDevicesComboBox.FormattingEnabled = true;
            this.midiDevicesComboBox.Location = new System.Drawing.Point(9, 34);
            this.midiDevicesComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.midiDevicesComboBox.Name = "midiDevicesComboBox";
            this.midiDevicesComboBox.Size = new System.Drawing.Size(265, 28);
            this.midiDevicesComboBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "IP адрес";
            // 
            // gmaIpTextBox
            // 
            this.gmaIpTextBox.BackColor = System.Drawing.Color.Black;
            this.gmaIpTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gmaIpTextBox.ForeColor = System.Drawing.Color.White;
            this.gmaIpTextBox.Location = new System.Drawing.Point(9, 57);
            this.gmaIpTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gmaIpTextBox.Name = "gmaIpTextBox";
            this.gmaIpTextBox.Size = new System.Drawing.Size(266, 26);
            this.gmaIpTextBox.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gmaConnectBtn);
            this.groupBox1.Controls.Add(this.gmaPasswordTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.gmaLoginTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.gmaIpTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.ForeColor = System.Drawing.Color.Goldenrod;
            this.groupBox1.Location = new System.Drawing.Point(18, 163);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(290, 276);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Подключение к grandMA 2";
            // 
            // gmaConnectBtn
            // 
            this.gmaConnectBtn.BackColor = System.Drawing.Color.Black;
            this.gmaConnectBtn.FlatAppearance.BorderColor = System.Drawing.Color.Goldenrod;
            this.gmaConnectBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(155)))), ((int)(((byte)(25)))));
            this.gmaConnectBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(155)))), ((int)(((byte)(25)))));
            this.gmaConnectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gmaConnectBtn.ForeColor = System.Drawing.Color.White;
            this.gmaConnectBtn.Location = new System.Drawing.Point(9, 222);
            this.gmaConnectBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gmaConnectBtn.Name = "gmaConnectBtn";
            this.gmaConnectBtn.Size = new System.Drawing.Size(267, 35);
            this.gmaConnectBtn.TabIndex = 6;
            this.gmaConnectBtn.Text = "Подключиться";
            this.gmaConnectBtn.UseVisualStyleBackColor = false;
            this.gmaConnectBtn.Click += new System.EventHandler(this.gmaConnectBtn_Click);
            // 
            // gmaPasswordTextBox
            // 
            this.gmaPasswordTextBox.BackColor = System.Drawing.Color.Black;
            this.gmaPasswordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gmaPasswordTextBox.ForeColor = System.Drawing.Color.White;
            this.gmaPasswordTextBox.Location = new System.Drawing.Point(9, 177);
            this.gmaPasswordTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gmaPasswordTextBox.Name = "gmaPasswordTextBox";
            this.gmaPasswordTextBox.Size = new System.Drawing.Size(266, 26);
            this.gmaPasswordTextBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(9, 152);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Пароль";
            // 
            // gmaLoginTextBox
            // 
            this.gmaLoginTextBox.BackColor = System.Drawing.Color.Black;
            this.gmaLoginTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gmaLoginTextBox.ForeColor = System.Drawing.Color.White;
            this.gmaLoginTextBox.Location = new System.Drawing.Point(9, 117);
            this.gmaLoginTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gmaLoginTextBox.Name = "gmaLoginTextBox";
            this.gmaLoginTextBox.Size = new System.Drawing.Size(266, 26);
            this.gmaLoginTextBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(9, 92);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Логин";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.midiUpdateBtn);
            this.groupBox2.Controls.Add(this.midiDevicesComboBox);
            this.groupBox2.Controls.Add(this.midiChangeStateBtn);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(18, 18);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(290, 125);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Подключение к MIDI устройству";
            // 
            // midiUpdateBtn
            // 
            this.midiUpdateBtn.FlatAppearance.BorderColor = System.Drawing.Color.Tomato;
            this.midiUpdateBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.midiUpdateBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.midiUpdateBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.midiUpdateBtn.Location = new System.Drawing.Point(9, 29);
            this.midiUpdateBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.midiUpdateBtn.Name = "midiUpdateBtn";
            this.midiUpdateBtn.Size = new System.Drawing.Size(268, 80);
            this.midiUpdateBtn.TabIndex = 2;
            this.midiUpdateBtn.Text = "Обновить список MIDI устройств";
            this.midiUpdateBtn.UseVisualStyleBackColor = true;
            this.midiUpdateBtn.Click += new System.EventHandler(this.midiUpdateBtn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.selectHardkeyBtn);
            this.groupBox3.Controls.Add(this.pageNumberLabel);
            this.groupBox3.Controls.Add(this.pageNumberTextBox);
            this.groupBox3.Controls.Add(this.executorNumberTextBox);
            this.groupBox3.Controls.Add(this.cmdContentComboBox);
            this.groupBox3.Controls.Add(this.midicmdSaveBtn);
            this.groupBox3.Controls.Add(this.midiLearnBtn);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.cmdContentTextBox);
            this.groupBox3.Controls.Add(this.cmdContentLabel);
            this.groupBox3.Controls.Add(this.cmdTypeComboBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.ForeColor = System.Drawing.Color.Goldenrod;
            this.groupBox3.Location = new System.Drawing.Point(18, 449);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(290, 277);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Новая команда";
            // 
            // selectHardkeyBtn
            // 
            this.selectHardkeyBtn.BackColor = System.Drawing.Color.Black;
            this.selectHardkeyBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.selectHardkeyBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.selectHardkeyBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.selectHardkeyBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectHardkeyBtn.ForeColor = System.Drawing.Color.White;
            this.selectHardkeyBtn.Location = new System.Drawing.Point(9, 117);
            this.selectHardkeyBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.selectHardkeyBtn.Name = "selectHardkeyBtn";
            this.selectHardkeyBtn.Size = new System.Drawing.Size(267, 35);
            this.selectHardkeyBtn.TabIndex = 14;
            this.selectHardkeyBtn.Text = "Выбрать Hardkey";
            this.selectHardkeyBtn.UseVisualStyleBackColor = false;
            this.selectHardkeyBtn.Click += new System.EventHandler(this.selectHardkeyBtn_Click);
            // 
            // pageNumberLabel
            // 
            this.pageNumberLabel.AutoSize = true;
            this.pageNumberLabel.ForeColor = System.Drawing.Color.White;
            this.pageNumberLabel.Location = new System.Drawing.Point(167, 92);
            this.pageNumberLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pageNumberLabel.Name = "pageNumberLabel";
            this.pageNumberLabel.Size = new System.Drawing.Size(83, 20);
            this.pageNumberLabel.TabIndex = 13;
            this.pageNumberLabel.Text = "Страница";
            // 
            // pageNumberTextBox
            // 
            this.pageNumberTextBox.BackColor = System.Drawing.Color.Black;
            this.pageNumberTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pageNumberTextBox.ForeColor = System.Drawing.Color.White;
            this.pageNumberTextBox.Location = new System.Drawing.Point(171, 118);
            this.pageNumberTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pageNumberTextBox.Name = "pageNumberTextBox";
            this.pageNumberTextBox.Size = new System.Drawing.Size(103, 26);
            this.pageNumberTextBox.TabIndex = 12;
            // 
            // executorNumberTextBox
            // 
            this.executorNumberTextBox.BackColor = System.Drawing.Color.Black;
            this.executorNumberTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.executorNumberTextBox.ForeColor = System.Drawing.Color.White;
            this.executorNumberTextBox.Location = new System.Drawing.Point(8, 117);
            this.executorNumberTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.executorNumberTextBox.Name = "executorNumberTextBox";
            this.executorNumberTextBox.Size = new System.Drawing.Size(142, 26);
            this.executorNumberTextBox.TabIndex = 9;
            // 
            // cmdContentComboBox
            // 
            this.cmdContentComboBox.BackColor = System.Drawing.Color.Black;
            this.cmdContentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmdContentComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdContentComboBox.ForeColor = System.Drawing.Color.White;
            this.cmdContentComboBox.FormattingEnabled = true;
            this.cmdContentComboBox.Location = new System.Drawing.Point(10, 117);
            this.cmdContentComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmdContentComboBox.Name = "cmdContentComboBox";
            this.cmdContentComboBox.Size = new System.Drawing.Size(265, 28);
            this.cmdContentComboBox.TabIndex = 11;
            this.cmdContentComboBox.Visible = false;
            // 
            // midicmdSaveBtn
            // 
            this.midicmdSaveBtn.BackColor = System.Drawing.Color.Black;
            this.midicmdSaveBtn.FlatAppearance.BorderColor = System.Drawing.Color.Goldenrod;
            this.midicmdSaveBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(155)))), ((int)(((byte)(25)))));
            this.midicmdSaveBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(155)))), ((int)(((byte)(25)))));
            this.midicmdSaveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.midicmdSaveBtn.ForeColor = System.Drawing.Color.White;
            this.midicmdSaveBtn.Location = new System.Drawing.Point(10, 223);
            this.midicmdSaveBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.midicmdSaveBtn.Name = "midicmdSaveBtn";
            this.midicmdSaveBtn.Size = new System.Drawing.Size(266, 35);
            this.midicmdSaveBtn.TabIndex = 10;
            this.midicmdSaveBtn.Text = "Добавить";
            this.midicmdSaveBtn.UseVisualStyleBackColor = false;
            this.midicmdSaveBtn.Click += new System.EventHandler(this.midicmdSaveBtn_Click);
            // 
            // midiLearnBtn
            // 
            this.midiLearnBtn.BackColor = System.Drawing.Color.Black;
            this.midiLearnBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.midiLearnBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.midiLearnBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.midiLearnBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.midiLearnBtn.ForeColor = System.Drawing.Color.White;
            this.midiLearnBtn.Location = new System.Drawing.Point(9, 177);
            this.midiLearnBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.midiLearnBtn.Name = "midiLearnBtn";
            this.midiLearnBtn.Size = new System.Drawing.Size(267, 35);
            this.midiLearnBtn.TabIndex = 9;
            this.midiLearnBtn.Text = "Обучить";
            this.midiLearnBtn.UseVisualStyleBackColor = false;
            this.midiLearnBtn.Click += new System.EventHandler(this.midiLearnBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(4, 154);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "MIDI команда";
            // 
            // cmdContentTextBox
            // 
            this.cmdContentTextBox.BackColor = System.Drawing.Color.Black;
            this.cmdContentTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cmdContentTextBox.ForeColor = System.Drawing.Color.White;
            this.cmdContentTextBox.Location = new System.Drawing.Point(9, 117);
            this.cmdContentTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmdContentTextBox.Name = "cmdContentTextBox";
            this.cmdContentTextBox.Size = new System.Drawing.Size(266, 26);
            this.cmdContentTextBox.TabIndex = 4;
            // 
            // cmdContentLabel
            // 
            this.cmdContentLabel.AutoSize = true;
            this.cmdContentLabel.ForeColor = System.Drawing.Color.White;
            this.cmdContentLabel.Location = new System.Drawing.Point(3, 92);
            this.cmdContentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cmdContentLabel.Name = "cmdContentLabel";
            this.cmdContentLabel.Size = new System.Drawing.Size(72, 20);
            this.cmdContentLabel.TabIndex = 3;
            this.cmdContentLabel.Text = "Executor";
            // 
            // cmdTypeComboBox
            // 
            this.cmdTypeComboBox.BackColor = System.Drawing.Color.Black;
            this.cmdTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmdTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdTypeComboBox.ForeColor = System.Drawing.Color.White;
            this.cmdTypeComboBox.FormattingEnabled = true;
            this.cmdTypeComboBox.Items.AddRange(new object[] {
            "ExecutorFader",
            "ExecutorButton1",
            "ExecutorButton2",
            "ExecutorButton3",
            "Encoder",
            "Hardkey",
            "Command",
            "SelectPresetType"});
            this.cmdTypeComboBox.Location = new System.Drawing.Point(9, 55);
            this.cmdTypeComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmdTypeComboBox.Name = "cmdTypeComboBox";
            this.cmdTypeComboBox.Size = new System.Drawing.Size(265, 28);
            this.cmdTypeComboBox.TabIndex = 2;
            this.cmdTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.cmdTypeComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Тип команды";
            // 
            // commandsDataGridView
            // 
            this.commandsDataGridView.AllowDrop = true;
            this.commandsDataGridView.AllowUserToAddRows = false;
            this.commandsDataGridView.AllowUserToResizeRows = false;
            this.commandsDataGridView.BackgroundColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Goldenrod;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.commandsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.commandsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.commandsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MidiId,
            this.Type,
            this.Executor,
            this.Value});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.commandsDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.commandsDataGridView.EnableHeadersVisualStyles = false;
            this.commandsDataGridView.GridColor = System.Drawing.Color.Goldenrod;
            this.commandsDataGridView.Location = new System.Drawing.Point(342, 18);
            this.commandsDataGridView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.commandsDataGridView.MultiSelect = false;
            this.commandsDataGridView.Name = "commandsDataGridView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Goldenrod;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.commandsDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.commandsDataGridView.RowHeadersVisible = false;
            this.commandsDataGridView.RowHeadersWidth = 62;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.DarkBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.commandsDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.commandsDataGridView.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.Black;
            this.commandsDataGridView.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.commandsDataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Blue;
            this.commandsDataGridView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.commandsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.commandsDataGridView.Size = new System.Drawing.Size(777, 708);
            this.commandsDataGridView.TabIndex = 9;
            this.commandsDataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.commandsDataGridView_RowsRemoved);
            this.commandsDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.commandsDataGridView_MouseClick);
            // 
            // MidiId
            // 
            this.MidiId.HeaderText = "MIDI Команда";
            this.MidiId.MinimumWidth = 8;
            this.MidiId.Name = "MidiId";
            this.MidiId.ReadOnly = true;
            this.MidiId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MidiId.Width = 150;
            // 
            // Type
            // 
            this.Type.HeaderText = "Тип команды";
            this.Type.MinimumWidth = 8;
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Type.Width = 150;
            // 
            // Executor
            // 
            this.Executor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Executor.HeaderText = "Executor";
            this.Executor.MinimumWidth = 8;
            this.Executor.Name = "Executor";
            this.Executor.ReadOnly = true;
            this.Executor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Value.HeaderText = "Данные";
            this.Value.MinimumWidth = 8;
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // saveProjectBtn
            // 
            this.saveProjectBtn.BackColor = System.Drawing.Color.Black;
            this.saveProjectBtn.FlatAppearance.BorderColor = System.Drawing.Color.Goldenrod;
            this.saveProjectBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(155)))), ((int)(((byte)(25)))));
            this.saveProjectBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(155)))), ((int)(((byte)(25)))));
            this.saveProjectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveProjectBtn.Location = new System.Drawing.Point(873, 749);
            this.saveProjectBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.saveProjectBtn.Name = "saveProjectBtn";
            this.saveProjectBtn.Size = new System.Drawing.Size(246, 35);
            this.saveProjectBtn.TabIndex = 11;
            this.saveProjectBtn.Text = "Сохранить проект";
            this.saveProjectBtn.UseVisualStyleBackColor = false;
            this.saveProjectBtn.Click += new System.EventHandler(this.saveProjectBtn_Click);
            // 
            // openProjectBtn
            // 
            this.openProjectBtn.BackColor = System.Drawing.Color.Black;
            this.openProjectBtn.FlatAppearance.BorderColor = System.Drawing.Color.Goldenrod;
            this.openProjectBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(155)))), ((int)(((byte)(25)))));
            this.openProjectBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(155)))), ((int)(((byte)(25)))));
            this.openProjectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openProjectBtn.Location = new System.Drawing.Point(655, 749);
            this.openProjectBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.openProjectBtn.Name = "openProjectBtn";
            this.openProjectBtn.Size = new System.Drawing.Size(210, 35);
            this.openProjectBtn.TabIndex = 12;
            this.openProjectBtn.Text = "Открыть проект";
            this.openProjectBtn.UseVisualStyleBackColor = false;
            this.openProjectBtn.Click += new System.EventHandler(this.openProjectBtn_Click);
            // 
            // aboutBtn
            // 
            this.aboutBtn.BackColor = System.Drawing.Color.Black;
            this.aboutBtn.FlatAppearance.BorderColor = System.Drawing.Color.Goldenrod;
            this.aboutBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(155)))), ((int)(((byte)(25)))));
            this.aboutBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(155)))), ((int)(((byte)(25)))));
            this.aboutBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutBtn.Location = new System.Drawing.Point(18, 749);
            this.aboutBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.aboutBtn.Name = "aboutBtn";
            this.aboutBtn.Size = new System.Drawing.Size(130, 35);
            this.aboutBtn.TabIndex = 13;
            this.aboutBtn.Text = "О программе";
            this.aboutBtn.UseVisualStyleBackColor = false;
            this.aboutBtn.Click += new System.EventHandler(this.aboutBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(1128, 791);
            this.Controls.Add(this.aboutBtn);
            this.Controls.Add(this.openProjectBtn);
            this.Controls.Add(this.saveProjectBtn);
            this.Controls.Add(this.commandsDataGridView);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1150, 847);
            this.MinimumSize = new System.Drawing.Size(1150, 847);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.commandsDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button midiChangeStateBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox gmaIpTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button gmaConnectBtn;
        private System.Windows.Forms.TextBox gmaPasswordTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox gmaLoginTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button midicmdSaveBtn;
        private System.Windows.Forms.Button midiLearnBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox cmdContentTextBox;
        private System.Windows.Forms.Label cmdContentLabel;
        private System.Windows.Forms.ComboBox cmdTypeComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView commandsDataGridView;
        private System.Windows.Forms.Button saveProjectBtn;
        private System.Windows.Forms.Button openProjectBtn;
        private System.Windows.Forms.Button aboutBtn;
        private System.Windows.Forms.ComboBox midiDevicesComboBox;
        private System.Windows.Forms.Button midiUpdateBtn;
        private System.Windows.Forms.ComboBox cmdContentComboBox;
        private System.Windows.Forms.Label pageNumberLabel;
        private System.Windows.Forms.TextBox pageNumberTextBox;
        private System.Windows.Forms.TextBox executorNumberTextBox;
        private System.Windows.Forms.Button selectHardkeyBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MidiId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Executor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
    }
}

