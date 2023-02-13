using MidiToGrandMA.DomainModel;
using NAudio.Midi;
using System;
using System.Net;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using MidiToGrandMA.gma;

namespace MidiToGrandMA
{
    public partial class MainForm : Form
    {
        private bool _learnMidi;
        private MidiIn _midi;
        private MA2Connector _gma;
        private ProjectFile _projectFile;
        private string _mainTitle;
        private Stack<Command> _deletedCommands;
        private List<MidiEncoderOldValue> _encoderOldValues;
        private string _projectFilePath;
        private string _appDataPath;
        private MA2Hardkeys _hardKeyForNewCmd;

        public MainForm()
        {
            // AppData folder
            _appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "midigma");
            if (!Directory.Exists(_appDataPath))
            {
                Directory.CreateDirectory(_appDataPath);
            }
            _projectFile = new ProjectFile();
            try
            {
                var lastProjectFilePath = File.ReadAllText(Path.Combine(_appDataPath, "last.cfg"));
                _projectFile = JsonConvert.DeserializeObject<ProjectFile>(File.ReadAllText(lastProjectFilePath));
                _projectFilePath = lastProjectFilePath;
            }
            catch
            {
                _projectFile.SetForEmpty();
                _projectFilePath = string.Empty;
            }
            _mainTitle = "MIDI to grandMA2";
            
            _deletedCommands = new Stack<Command>();
            _encoderOldValues = new List<MidiEncoderOldValue>();
            
            InitializeComponent();

            this.Text = string.IsNullOrEmpty(_projectFilePath)
                ? _mainTitle
                : $"{_mainTitle} ({Path.GetFileName(_projectFilePath)})";
            updateMidiDevices();
            this.ActiveControl = label1; // Снять фокус с текстбокса
            cmdTypeComboBox.SelectedIndex = 0;
            _learnMidi = false;
            var midiDevices = getMidiDevices();
            try
            {
                if ((midiDevices.Count - 1) >= _projectFile.MidiDeviceId && midiDevices[_projectFile.MidiDeviceId] == _projectFile.MidiDeviceName)
                {
                    midiDevicesComboBox.SelectedIndex = _projectFile.MidiDeviceId;
                    _midi = new MidiIn(_projectFile.MidiDeviceId);
                    _midi.MessageReceived += midiIn_MessageReceived;
                    _midi.Start();
                    midiChangeStateBtn.Text = "Отключиться";
                    midiDevicesComboBox.Enabled = false;
                }
            }
            catch
            {
                MessageBox.Show("Выбранное MIDI устройство занято другим приложением", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            gmaIpTextBox.Text = _projectFile.GmaIPAddress;
            gmaLoginTextBox.Text = _projectFile.GmaLogin;
            gmaPasswordTextBox.Text = _projectFile.GmaPassword;
            if (!string.IsNullOrEmpty(gmaIpTextBox.Text)) connectToGma();
            updateGridViewFromCommands();
        }

        // Обработка midi сообщения
        private void midiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            var response = new MidiResponse(e.MidiEvent.ToString());
            if (_learnMidi)
            {
                _learnMidi = false;
                midiLearnBtn.Invoke(new Action(() => midiLearnBtn.Text = response.Id));
            }
            var cmdList = _projectFile.Commands.Where(c => c.MidiId == response.Id).ToList();
            try
            {
                foreach (var cmd in cmdList)
                {
                    if (_gma == null) return;
                    switch (cmd.CmdType)
                    {
                        case CmdTypes.ExecutorFader:
                            var faderValue = response.ResponseType == MidiResponseTypes.ControlChange ? response.PercentControllerValue : 100;
                            var valueForMA = (float)faderValue / 100;
                            if (cmd.PageNumber == null) _gma.Playbacks.SetFaderLevel(cmd.Executor.Value, valueForMA);
                            if (cmd.PageNumber != null) _gma.Playbacks.SetFaderLevel(cmd.Executor.Value, valueForMA, cmd.PageNumber.Value);
                            break;
                        case CmdTypes.ExecutorButton1:
                            if (response.ResponseType == MidiResponseTypes.NoteOn || response.ControllerValue == 127)
                            {
                                if (cmd.PageNumber == null) _gma.Playbacks.SetButtonState(cmd.Executor.Value, 1, true);
                                if (cmd.PageNumber != null) _gma.Playbacks.SetButtonState(cmd.Executor.Value, 1, true, cmd.PageNumber.Value);
                                break;
                            }
                            if (cmd.PageNumber == null) _gma.Playbacks.SetButtonState(cmd.Executor.Value, 1, false);
                            if (cmd.PageNumber != null) _gma.Playbacks.SetButtonState(cmd.Executor.Value, 1, false, cmd.PageNumber.Value);
                            break;
                        case CmdTypes.ExecutorButton2:
                            if (response.ResponseType == MidiResponseTypes.NoteOn || response.ControllerValue == 127)
                            {
                                if (cmd.PageNumber == null) _gma.Playbacks.SetButtonState(cmd.Executor.Value, 2, true);
                                if (cmd.PageNumber != null) _gma.Playbacks.SetButtonState(cmd.Executor.Value, 2, true, cmd.PageNumber.Value);
                                break;
                            }
                            if (cmd.PageNumber == null) _gma.Playbacks.SetButtonState(cmd.Executor.Value, 2, false);
                            if (cmd.PageNumber != null) _gma.Playbacks.SetButtonState(cmd.Executor.Value, 2, false, cmd.PageNumber.Value);
                            break;
                        case CmdTypes.ExecutorButton3:
                            if (response.ResponseType == MidiResponseTypes.NoteOn || response.ControllerValue == 127)
                            {
                                if (cmd.PageNumber == null) _gma.Playbacks.SetButtonState(cmd.Executor.Value, 3, true);
                                if (cmd.PageNumber != null) _gma.Playbacks.SetButtonState(cmd.Executor.Value, 3, true, cmd.PageNumber.Value);
                                break;
                            }
                            if (cmd.PageNumber == null) _gma.Playbacks.SetButtonState(cmd.Executor.Value, 3, false);
                            if (cmd.PageNumber != null) _gma.Playbacks.SetButtonState(cmd.Executor.Value, 3, false, cmd.PageNumber.Value);
                            break;
                        case CmdTypes.Encoder:
                            var encoderOldValue = _encoderOldValues.SingleOrDefault(v => v.MidiId == response.Id);
                            if (encoderOldValue == null)
                            {
                                _encoderOldValues.Add(new MidiEncoderOldValue(response.Id, response.ControllerValue));
                                break;
                            }
                            var encoderDirection = encoderOldValue.GetEncoderDirection(response.ControllerValue);
                            var encoderDirectionValue = string.Empty;
                            if (!int.TryParse(cmd.Content, out var encoderNumber)) return;
                            switch (encoderDirection)
                            {
                                case EncoderValue.Back:
                                    _gma.ScrollEncoder(encoderNumber, -1);
                                    break;
                                case EncoderValue.Forward:
                                    _gma.ScrollEncoder(encoderNumber, 1);
                                    break;
                                default:
                                    return;
                            }
                            break;
                        case CmdTypes.Command:
                            if (response.ResponseType == MidiResponseTypes.NoteOn || response.ControllerValue == 127)
                            {
                                if (string.IsNullOrEmpty(cmd.Content)) return;
                                _gma.Execute(cmd.Content);
                            }
                            break;
                        case CmdTypes.Hardkey:
                            if (cmd.Hardkey == null) return;
                            if (response.ResponseType == MidiResponseTypes.NoteOn || response.ControllerValue == 127)
                            {
                                _gma.SetHardkey(cmd.Hardkey.Value, true);
                                break;
                            }
                            _gma.SetHardkey(cmd.Hardkey.Value, false);
                            break;
                        case CmdTypes.SelectPresetType:
                            if (response.ResponseType == MidiResponseTypes.NoteOn || response.ControllerValue == 127)
                            {
                                _gma.Execute($"{cmd.Content}");
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Потерялось подключение к grandMA.\nПроверьте его настрйоки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _gma = null;
            }
            
            var rowsForGreen = GetSelectedCommandIndexFromCommandList(cmdList);
            setGreenColorToRows(rowsForGreen);
        }

        private List<string> getMidiDevices()
        {
            var result = new List<string>();
            for (var device = 0; device < MidiIn.NumberOfDevices; device++)
            {
                result.Add(MidiIn.DeviceInfo(device).ProductName);
            }
            return result;
        }

        private void midiChangeStateBtn_Click(object sender, EventArgs e)
        {
            if (midiChangeStateBtn.Text == "Подключиться")
            {
                try
                {
                    _midi = new MidiIn(midiDevicesComboBox.SelectedIndex);
                    _midi.MessageReceived += midiIn_MessageReceived;
                    _midi.Start();
                    midiChangeStateBtn.Text = "Отключить";
                    midiDevicesComboBox.Enabled = false;
                    _projectFile.MidiDeviceId = midiDevicesComboBox.SelectedIndex;
                    _projectFile.MidiDeviceName = midiDevicesComboBox.Items[midiDevicesComboBox.SelectedIndex].ToString();
                    return;
                }
                catch
                {
                    MessageBox.Show("Выбранное MIDI устройство занято другим приложением.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    updateMidiDevices();
                    return;
                }
            }
            try
            {
                _midi.Stop();
                _midi.Dispose();
                midiChangeStateBtn.Text = "Подключиться";
                midiDevicesComboBox.Enabled = true;
                updateMidiDevices();
            }
            catch
            {
                _midi.Dispose();
                _midi = null;
                midiChangeStateBtn.Text = "Подключиться";
                midiDevicesComboBox.Enabled = true;
                updateMidiDevices();
            }
        }

        private void midicmdSaveBtn_Click(object sender, EventArgs e)
        {
            if (midiLearnBtn.Text == "Обучить" || midiLearnBtn.Text == "Жду MIDI сигнал...")
            {
                MessageBox.Show("Для добавления команды необходимо обучить MIDI", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var cmd = new Command();
            int? execNumber = null;
            if (!string.IsNullOrEmpty(executorNumberTextBox.Text))
            {
                if (!int.TryParse(executorNumberTextBox.Text, out var execNumberParam))
                {
                    MessageBox.Show("Неверный номер экзекьютора", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                execNumber = execNumberParam;
            }
            
            int? pageNumber = null;
            if (!string.IsNullOrEmpty(pageNumberTextBox.Text))
            {
                if (!int.TryParse(pageNumberTextBox.Text, out var pageNumberParam))
                {
                    MessageBox.Show("Неверный номер страницы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                pageNumber = pageNumberParam;
            }
            switch (cmdTypeComboBox.SelectedIndex)
            {
                // ExecutorFader
                case 0:
                    if (execNumber == null)
                    {
                        MessageBox.Show("Неверный номер экзекьютора", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    cmd = new Command(CmdTypes.ExecutorFader, midiLearnBtn.Text, execNumber, pageNumber, null, "");
                    break;
                // ExecutorButton1
                case 1:
                    if (execNumber == null)
                    {
                        MessageBox.Show("Неверный номер страницы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    cmd = new Command(CmdTypes.ExecutorButton1, midiLearnBtn.Text, execNumber, pageNumber, null, "");
                    break;
                // ExecutorButton2
                case 2:
                    if (execNumber == null)
                    {
                        MessageBox.Show("Неверный номер экзекьютора", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    cmd = new Command(CmdTypes.ExecutorButton2, midiLearnBtn.Text, execNumber, pageNumber, null, "");
                    break;
                // ExecutorButton3
                case 3:
                    if (execNumber == null)
                    {
                        MessageBox.Show("Неверный номер экзекьютора", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    cmd = new Command(CmdTypes.ExecutorButton3, midiLearnBtn.Text, execNumber, pageNumber, null, "");
                    break;
                // Encoder
                case 4:
                    var encoderNuber = cmdContentComboBox.SelectedIndex + 1;
                    cmd = new Command(CmdTypes.Encoder, midiLearnBtn.Text, null, null, null, encoderNuber.ToString());
                    break;
                // Hardkey
                case 5:
                    if (selectHardkeyBtn.Text == "Выбрать Hardkey")
                    {
                        MessageBox.Show("Неверный выбор hardkey", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    cmd = new Command(CmdTypes.Hardkey, midiLearnBtn.Text, null, null, _hardKeyForNewCmd, selectHardkeyBtn.Text);
                    selectHardkeyBtn.Text = "Выбрать Hardkey";
                    break;
                // Command
                case 6:
                    if (string.IsNullOrEmpty(cmdContentTextBox.Text))
                    {
                        MessageBox.Show($"Поле \"{cmdContentLabel.Text}\" не может быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    cmd = new Command(CmdTypes.Command, midiLearnBtn.Text, null, null, null, cmdContentTextBox.Text);
                    break;
                // SelectPresetType
                case 7:
                    var presetPageNuber = cmdContentComboBox.SelectedIndex + 1;
                    cmd = new Command(CmdTypes.Command, midiLearnBtn.Text, null, null, null, $"PresetType {presetPageNuber}");
                    break;
                default:
                    break;
            }
            _projectFile.Commands.Add(cmd);
            midiLearnBtn.Text = "Обучить";
            updateGridViewFromCommands();
        }

        private void commandsDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            _deletedCommands.Push(_projectFile.Commands[e.RowIndex]);
            _projectFile.Commands.RemoveAt(e.RowIndex);
        }

        private void updateGridViewFromCommands()
        {
            var tempCommands = _projectFile.Commands.ToList();
            var tempDeletedCommand = new Stack<Command>(new Stack<Command>(_deletedCommands));
            commandsDataGridView.Rows.Clear();
            commandsDataGridView.Refresh();
            _projectFile.Commands = tempCommands;
            _deletedCommands = tempDeletedCommand;

            foreach (var cmd in _projectFile.Commands)
            {
                var executorFullNumber = string.Empty;
                if (cmd.IsExecutorControl())
                {
                    if (cmd.Executor == null) continue;
                    executorFullNumber = cmd.PageNumber == null
                        ? $"{cmd.Executor.Value}"
                        : $"{cmd.PageNumber.Value}.{cmd.Executor.Value}";
                }
                commandsDataGridView.Rows.Add(cmd.MidiId, cmd.CmdType, executorFullNumber, cmd.Content);
            }

        }

        private void midiLearnBtn_Click(object sender, EventArgs e)
        {
            if (_midi == null)
            {
                MessageBox.Show("Необходимо подключиться к MIDI-контроллеру", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            _learnMidi = true;
            midiLearnBtn.Text = "Ожидание...";
        }

        private void gmaConnectBtn_Click(object sender, EventArgs e)
        {
            _projectFile.GmaIPAddress = gmaIpTextBox.Text;
            _projectFile.GmaLogin = gmaLoginTextBox.Text;
            _projectFile.GmaPassword = gmaPasswordTextBox.Text;
            connectToGma();
        }

        private void connectToGma()
        {
            try
            {
                var ip = IPAddress.Parse(_projectFile.GmaIPAddress);
                _gma = new MA2Connector(ip.ToString(), _projectFile.GmaLogin, _projectFile.GmaPassword);
                _gma.OnConnectionLost += OnConnectionLost;
                gmaConnectBtn.Text = "Переподключиться";
            }
            catch
            {
                MessageBox.Show("Потеряно подключение к grandMA.\nПроверьте настроойки подключения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void OnConnectionLost(MA2Connector o, EventArgs e)
        {
            connectToGma();
        }

        private void aboutBtn_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void openProjectBtn_Click(object sender, EventArgs e)
        {
            openProject();
        }

        private void saveProjectBtn_Click(object sender, EventArgs e)
        {
            saveProject();
        }

        private void saveProject()
        {
            try
            {
                var dialog = new SaveFileDialog();
                dialog.Title = "Сохранить файл проекта";
                dialog.Filter = "Файл проекта MIDI to grandMA|*.midigma3";
                dialog.InitialDirectory = string.IsNullOrEmpty(_projectFilePath) ? @"C:\" : _projectFilePath;
                dialog.FileName = Path.GetFileName(_projectFilePath);
                if (dialog.ShowDialog() != DialogResult.OK) return;
                _projectFilePath = dialog.FileName;
                var jsonString = JsonConvert.SerializeObject(_projectFile);
                File.WriteAllText(dialog.FileName, jsonString);
                this.Text = $"{_mainTitle} ({Path.GetFileName(dialog.FileName)})";
                SaveLastProjectFilePath();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранение.\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void openProject()
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Открыть файл проекта";
            dialog.Filter = "Файл проекта MIDI to grandMA|*.midigma3";
            dialog.InitialDirectory = string.IsNullOrEmpty(_projectFilePath) ? @"C:\" : _projectFilePath;
            if (dialog.ShowDialog() != DialogResult.OK) return;
            _projectFilePath = dialog.FileName;
            try
            {
                _projectFile = JsonConvert.DeserializeObject<ProjectFile>(File.ReadAllText(_projectFilePath));
                this.Text = $"{_mainTitle} ({Path.GetFileName(dialog.FileName)})";
                updateGridViewFromCommands();
                SaveLastProjectFilePath();
            }
            catch
            {
                MessageBox.Show($"Неправильный файл проекта", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveLastProjectFilePath()
        {
            if (string.IsNullOrEmpty(_projectFilePath)) return;
            File.WriteAllText(Path.Combine(_appDataPath, "last.cfg"), _projectFilePath);
        }

        private void updateMidiDevices()
        {
            var midiDevices = getMidiDevices();
            if (midiDevices.Count == 0)
            {
                midiUpdateBtn.Enabled = true;
                midiUpdateBtn.Visible = true;
                MessageBox.Show("MIDI устройство не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            midiDevicesComboBox.Items.Clear();
            midiDevices.ForEach(d => midiDevicesComboBox.Items.Add(d));
            midiDevicesComboBox.SelectedIndex = 0;
            midiUpdateBtn.Enabled = false;
            midiUpdateBtn.Visible = false;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Отмена последнего действия
            if (e.KeyCode == Keys.Z && e.Modifiers == Keys.Control)
            {
                if (_deletedCommands.Count > 0)
                {
                    _projectFile.Commands.Add(_deletedCommands.Pop());
                    updateGridViewFromCommands();
                }
            }

            // Сохранение проекта
            if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                saveProject();
            }

            // Открытие проекта
            if (e.KeyCode == Keys.O && e.Modifiers == Keys.Control)
            {
                openProject();
            }

            // Окно о программе
            if (e.KeyCode == Keys.P && e.Modifiers == Keys.Control)
            {
                var aboutForm = new AboutForm();
                aboutForm.ShowDialog();
            }

        }

        private void setGreenColorToRows(List<int> rowIndexes)
        {
            commandsDataGridView.ClearSelection();
            for (int i = 0; i <= commandsDataGridView.Rows.Count - 1; i++)
            {
                var color = rowIndexes.Contains(i) ? Color.Green : Color.Black;
                commandsDataGridView.Rows[i].DefaultCellStyle.BackColor = color;
            }
        }

        private List<int> GetSelectedCommandIndexFromCommandList(List<Command> selectedCommands)
        {
            var index = 0;
            var result = new List<int>();
            foreach (var cmd in _projectFile.Commands)
            {
                if (selectedCommands.Contains(cmd)) result.Add(index);
                index++;
            }
            return result;
        }

        private void midiUpdateBtn_Click(object sender, EventArgs e)
        {
            updateMidiDevices();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Закрыть приложение?\nВсе несохраненные данные будут потеряны.", "Закрыть приложение?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) e.Cancel = true;
        }


        private void cmdTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmdTypeComboBox.SelectedIndex)
            {
                // ExecutorFader
                case 0:
                    cmdContentLabel.Text = "Executor Number";
                    cmdContentComboBox.Visible = false;
                    executorNumberTextBox.Visible = true;
                    pageNumberTextBox.Visible = true;
                    pageNumberLabel.Visible = true;
                    cmdContentTextBox.Visible = false;
                    selectHardkeyBtn.Visible = false;
                    break;
                // ExecutorButton1
                case 1:
                    cmdContentLabel.Text = "Executor Number";
                    cmdContentComboBox.Visible = false;
                    executorNumberTextBox.Visible = true;
                    pageNumberTextBox.Visible = true;
                    pageNumberLabel.Visible = true;
                    cmdContentTextBox.Visible = false;
                    selectHardkeyBtn.Visible = false;
                    break;
                // ExecutorButton2
                case 2:
                    cmdContentLabel.Text = "Executor Number";
                    cmdContentComboBox.Visible = false;
                    executorNumberTextBox.Visible = true;
                    pageNumberTextBox.Visible = true;
                    pageNumberLabel.Visible = true;
                    cmdContentTextBox.Visible = false;
                    selectHardkeyBtn.Visible = false;
                    break;
                // ExecutorButton3
                case 3:
                    cmdContentLabel.Text = "Executor Number";
                    cmdContentComboBox.Visible = false;
                    executorNumberTextBox.Visible = true;
                    pageNumberTextBox.Visible = true;
                    pageNumberLabel.Visible = true;
                    cmdContentTextBox.Visible = false;
                    selectHardkeyBtn.Visible = false;
                    break;
                // Encoder
                case 4:
                    cmdContentLabel.Text = "Encoder";
                    cmdContentComboBox.Visible = true;
                    executorNumberTextBox.Visible = false;
                    pageNumberTextBox.Visible = false;
                    pageNumberLabel.Visible = false;
                    cmdContentTextBox.Visible = false;
                    selectHardkeyBtn.Visible = false;
                    cmdContentComboBox.Items.Clear();
                    cmdContentComboBox.Items.Add("Attribute Wheel 1");
                    cmdContentComboBox.Items.Add("Attribute Wheel 2");
                    cmdContentComboBox.Items.Add("Attribute Wheel 3");
                    cmdContentComboBox.Items.Add("Attribute Wheel 4");
                    cmdContentComboBox.Items.Add("Attribute Wheel 5");
                    cmdContentComboBox.SelectedIndex = 0;
                    break;
                // Hardkey
                case 5:
                    cmdContentLabel.Text = "Hardkey";
                    cmdContentComboBox.Visible = false;
                    executorNumberTextBox.Visible = false;
                    pageNumberTextBox.Visible = false;
                    pageNumberLabel.Visible = false;
                    cmdContentTextBox.Visible = true;
                    cmdContentTextBox.Enabled = false;
                    selectHardkeyBtn.Visible = true;
                    break;
                // Command
                case 6:
                    cmdContentLabel.Text = "Command";
                    cmdContentComboBox.Visible = false;
                    executorNumberTextBox.Visible = false;
                    pageNumberTextBox.Visible = false;
                    pageNumberLabel.Visible = false;
                    cmdContentTextBox.Visible = true;
                    selectHardkeyBtn.Visible = false;
                    cmdContentTextBox.Enabled = true;
                    break;
                // SelectPresetType
                case 7:
                    cmdContentLabel.Text = "Select Preset Type";
                    cmdContentComboBox.Visible = true;
                    executorNumberTextBox.Visible = false;
                    pageNumberTextBox.Visible = false;
                    pageNumberLabel.Visible = false;
                    cmdContentTextBox.Visible = false;
                    selectHardkeyBtn.Visible = false;
                    cmdContentComboBox.Items.Clear();
                    cmdContentComboBox.Items.Add("1 Dimmer");
                    cmdContentComboBox.Items.Add("2 Position");
                    cmdContentComboBox.Items.Add("3 Gobo");
                    cmdContentComboBox.Items.Add("4 Color");
                    cmdContentComboBox.Items.Add("5 Beam");
                    cmdContentComboBox.Items.Add("6 Focus");
                    cmdContentComboBox.Items.Add("7 Control");
                    cmdContentComboBox.Items.Add("8 Shapers");
                    cmdContentComboBox.Items.Add("9 Video");
                    cmdContentComboBox.SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        private void commandsDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var currentMouseOverRow = commandsDataGridView.HitTest(e.X, e.Y).RowIndex;
                if (currentMouseOverRow < 0) return;
                commandsDataGridView.Rows[currentMouseOverRow].Selected = true;

                var menu = new ContextMenu();
                menu.MenuItems.Add(new MenuItem("Удалить", DeleteCommandHandler));
                
                if (_deletedCommands.Count > 0) menu.MenuItems.Add(new MenuItem("Отменить удаление (ctrl + z)", UndoRemoveCommandHandler));

                menu.Show(commandsDataGridView, new Point(e.X, e.Y));

            }
        }

        private void UndoRemoveCommandHandler(object sender, EventArgs e)
        {
            if (_deletedCommands.Count > 0)
            {
                _projectFile.Commands.Add(_deletedCommands.Pop());
                updateGridViewFromCommands();
            }
        }

        private void DeleteCommandHandler(object sender, EventArgs e)
        {
            var rowIndex = commandsDataGridView.SelectedRows[0].Index;
            commandsDataGridView.Rows.Remove(commandsDataGridView.Rows[rowIndex]);
        }

        private void selectHardkeyBtn_Click(object sender, EventArgs e)
        {
            var selectHardKeyForm = new SelectHardKeyForm();
            selectHardKeyForm.ShowDialog();
            _hardKeyForNewCmd = selectHardKeyForm.SelectedHardkey;
            selectHardkeyBtn.Text = _hardKeyForNewCmd.ToString();

        }
    }
}