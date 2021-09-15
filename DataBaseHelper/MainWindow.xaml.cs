using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace DataBaseHelper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {    
        public Random rnd = new Random();
        public List<Block> blocks;
        public List<BlockType> listOfTypes;

        public int IDCounter = 0;
        public bool isDictUpdating = false;

        public string savePath = Environment.CurrentDirectory + @"\LastQuery.txt"; 

        Regex InputNumbersRegex = new Regex(@"^\d+$");

        public MainWindow()
        {
            InitializeComponent();
            InitBlocks();

            GlobalDicts.LoadDictionaries();
        }

        private void InitBlocks()
        {
            blocks = new List<Block>();
            listOfTypes = new List<BlockType>();

            listOfTypes.Add(new BlockType(0, BlockType._typeLabel.LocalAutoIncrement, "Локальный автоинкремент"));
            listOfTypes.Add(new BlockType(1, BlockType._typeLabel.GlobalAutoIncrement, "Глобальный автоинкремент"));
            listOfTypes.Add(new BlockType(2, BlockType._typeLabel.RndString, "Случайная строка размером"));
            listOfTypes.Add(new BlockType(3, BlockType._typeLabel.RndNumber, "Случайное число в интервале"));
            listOfTypes.Add(new BlockType(4, BlockType._typeLabel.Zero, "Обычный нулик (0)"));
            listOfTypes.Add(new BlockType(5, BlockType._typeLabel.Empty, "Пустая строка"));
            listOfTypes.Add(new BlockType(6, BlockType._typeLabel.RndTime, "Время в интервале"));
            listOfTypes.Add(new BlockType(7, BlockType._typeLabel.RndDate, "Дата в интервале"));
            listOfTypes.Add(new BlockType(8, BlockType._typeLabel.RndDateTime, "Дата и время в интервале"));
            listOfTypes.Add(new BlockType(9, BlockType._typeLabel.CustomDict, "Данные из словаря"));
            listOfTypes.Add(new BlockType(10, BlockType._typeLabel.CustomString, "Кастомная строка"));
            //listOfTypes.Add(new BlockType(11, BlockType._typeLabel.DuplicateParam, "Дубликат параметра"));
            //listOfTypes.Add(new BlockType(12, BlockType._typeLabel.LinkedParam, "Связанный параметр"));
            //listOfTypes.Add(new BlockType(, BlockType._typeLabel, ""));
        }

        private void AddBlock()
        {
            int amount = 0;

            try
            {
                amount = int.Parse(InputAmountTextBox.Text.Replace(" ", ""));
            } 
            catch(Exception)
            {
                MessageBox.Show("Ты зачем это делаешь? Не делай так больше НИКОГДА.");
                InputAmountTextBox.Text = "";
                return;
            }

            if (amount <= 0)
            {
                MessageBox.Show("Ты зачем программу ломаешь? Я тебе какое плохое зло сделал?");
                InputAmountTextBox.Text = "";
                return;
            }

            for (int i = 0; i < amount; i++)
            {
                blocks.Add(new Block(IDCounter));
                CreateBlock(IDCounter);
                IDCounter++;
            }

            CalcStackHeight();
            
            InputAmountTextBox.Text = "";
        }

        private void CalcStackHeight()
        {
            MainStack.Height = blocks.Count * 50;
        }

        private void ReDrawBlocks()
        {
            for (int i = 0; i < MainStack.Children.Count; i++)
            {
                Canvas cns = (Canvas)MainStack.Children[i];
                blocks[i].ID = i;
                
                for (int j = 0; j < cns.Children.Count; j++)
                {
                    if (cns.Children[j] is Label)
                    {
                        Label newBlock = (Label)cns.Children[j];
                        if (newBlock.Name.Contains("NUM"))
                        {
                            newBlock.Content = "" + i;
                            newBlock.Name = "NUM" + i;
                            cns.Children[j] = newBlock;
                        }
                    }
                    if (cns.Children[j] is Button)
                    {
                        Button newBlock = (Button)cns.Children[j];
                        if (newBlock.Name.Contains("BTN"))
                        {
                            newBlock.Name = "BTN" + i;
                            cns.Children[j] = newBlock;
                        }
                    }
                    if (cns.Children[j] is ComboBox)
                    {
                        ComboBox cb = (ComboBox)cns.Children[j];
                        if (cb.Name.Contains("CB"))
                        {
                            cb.Name = "CB" + i;
                            cns.Children[j] = cb;
                        }
                    }
                }
            }
            IDCounter = MainStack.Children.Count;
        }

        private void CreateBlock(int value)
        {
            Canvas block = new Canvas();
            block.Style = Resources["MainCanvas"] as Style;
            TextBox tbStr = new TextBox();
            tbStr.Style = Resources["InputString"] as Style;
            tbStr.Name = "STR" + value;
            TextBox tb1 = new TextBox();
            tb1.Style = Resources["InputMinInterval"] as Style;
            tb1.Name = "MIN" + value;
            tb1.TextChanged += TextBox_TextChanged;
            TextBox tb2 = new TextBox();
            tb2.Style = Resources["InputMaxInterval"] as Style;
            tb2.Name = "MAX" + value;
            tb2.TextChanged += TextBox_TextChanged;
            Label lbl1 = new Label();
            lbl1.Style = Resources["Label1"] as Style;
            Label lbl2 = new Label();
            lbl2.Style = Resources["Label2"] as Style;
            Label numLbl = new Label();
            numLbl.Style = Resources["NumLbl"] as Style;
            numLbl.Content = "" + value;
            numLbl.Name = "NUM" + value;
            Button btn = new Button();
            btn.Style = Resources["RemoveButton"] as Style;
            btn.Name = "BTN" + value;
            btn.Click += Button_Click;
            ComboBox cb = new ComboBox();
            cb.Style = Resources["ComboBox"] as Style;
            cb.SelectedIndex = 0;
            cb.Name = "CB" + value;
            cb.SelectionChanged += ComboBox_SelectionChanged;
            for (int i = 0; i < listOfTypes.Count; i++)
                cb.Items.Add(listOfTypes[i].typeName.ToString());
            ComboBox cbDict = new ComboBox();
            cbDict.Style = Resources["ComboBoxDict"] as Style;
            cbDict.SelectedIndex = 0;
            cbDict.Name = "DICT" + value;
            DateTimePicker dtPicker1 = new DateTimePicker();
            dtPicker1.Style = Resources["DateTimePickerMin"] as Style;
            dtPicker1.Name = "DTPMIN";
            DateTimePicker dtPicker2 = new DateTimePicker();
            dtPicker2.Style = Resources["DateTimePickerMax"] as Style;
            dtPicker2.Name = "DTPMAX";
            TimePicker tPicker1 = new TimePicker();
            tPicker1.Style = Resources["TimePickerMin"] as Style;
            tPicker1.Name = "TPMIN";
            TimePicker tPicker2 = new TimePicker();
            tPicker2.Style = Resources["TimePickerMax"] as Style;
            tPicker2.Name = "TPMAX";
            DatePicker dPicker1 = new DatePicker();
            dPicker1.Style = Resources["DatePickerMin"] as Style;
            dPicker1.Name = "DPMIN";
            DatePicker dPicker2 = new DatePicker();
            dPicker2.Style = Resources["DatePickerMax"] as Style;
            dPicker2.Name = "DPMAX";

            block.Children.Add(numLbl); // Всегда отображать
            block.Children.Add(cb);     // Комбобокс выбора типа
            block.Children.Add(btn);    // Кнопка удалить
            block.Children.Add(lbl1);   // Текст "от"
            block.Children.Add(lbl2);   // Текст "до"
            block.Children.Add(tb1);    // Инпут МИН
            block.Children.Add(tb2);    // Инпут МАКС
            block.Children.Add(tbStr);  // Инпут стринг
            block.Children.Add(dtPicker1); // Мин для даты и времени
            block.Children.Add(dtPicker2); // Макс для даты и времени
            block.Children.Add(tPicker1);  // Мин для времени
            block.Children.Add(tPicker2);  // Макс для времени
            block.Children.Add(dPicker1);  // Мин для даты
            block.Children.Add(dPicker2);  // Макс для даты
            block.Children.Add(cbDict); // Комбобокс на словари

            ChangeInputState(block, 0, value);
            MainStack.Children.Add(block);
        }

        private void ChangeInputState(Canvas cns, int selIndex, int index)
        {
            BlockType._typeLabel type = listOfTypes[selIndex].typeLabel;
            blocks[index].Type = listOfTypes[selIndex];

            for (int i = 3; i < cns.Children.Count; i++)
            {
                cns.Children[i].Visibility = Visibility.Hidden;
            }

            switch (type)
            {
                case BlockType._typeLabel.Zero:
                case BlockType._typeLabel.Empty:
                case BlockType._typeLabel.LocalAutoIncrement:
                case BlockType._typeLabel.GlobalAutoIncrement:
                    break;
                case BlockType._typeLabel.LinkedParam:
                case BlockType._typeLabel.CustomString:
                case BlockType._typeLabel.DuplicateParam:
                    cns.Children[7].Visibility = Visibility.Visible;
                    break;
                case BlockType._typeLabel.RndNumber:
                case BlockType._typeLabel.RndString:
                    for (int i = 3; i < 7; i++)
                        cns.Children[i].Visibility = Visibility.Visible;
                    break;
                case BlockType._typeLabel.RndDateTime:
                    cns.Children[3].Visibility = Visibility.Visible;
                    cns.Children[4].Visibility = Visibility.Visible;
                    cns.Children[8].Visibility = Visibility.Visible;
                    cns.Children[9].Visibility = Visibility.Visible;
                    break;
                case BlockType._typeLabel.RndTime:
                    cns.Children[3].Visibility = Visibility.Visible;
                    cns.Children[4].Visibility = Visibility.Visible;
                    cns.Children[10].Visibility = Visibility.Visible;
                    cns.Children[11].Visibility = Visibility.Visible;
                    break;
                case BlockType._typeLabel.RndDate:
                    cns.Children[3].Visibility = Visibility.Visible;
                    cns.Children[4].Visibility = Visibility.Visible;
                    cns.Children[12].Visibility = Visibility.Visible;
                    cns.Children[13].Visibility = Visibility.Visible;
                    break;
                case BlockType._typeLabel.CustomDict:
                    ComboBox customDict = (ComboBox)cns.Children[cns.Children.Count - 1];
                    isDictUpdating = true;
                    customDict.Items.Clear();
                    foreach (var dict in GlobalDicts.getDictsNames())
                        customDict.Items.Add(dict);
                    isDictUpdating = false;
                    cns.Children[cns.Children.Count - 1].Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void RemoveBlock(int value)
        {
            int index = value;

            for (int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i].ID == value)
                { 
                    index = i;
                    break;
                }
            }
            
            blocks.RemoveAt(index);
            MainStack.Children.RemoveAt(index);
            ReDrawBlocks();
            CalcStackHeight();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddBlock();
        }

        /// <summary>
        /// Получить итоговый код
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(InputLinesAmount.Text) || !InputNumbersRegex.IsMatch(InputLinesAmount.Text))
            {
                MessageBox.Show("Ты пытаешься сломать это? Не получится! :Р");
                return;
            }

            GetData();
            GetCode();
        }

        private void GetData()
        {
            // Для каждого блока берём каждую часть внутри
            for (int i = 0; i < MainStack.Children.Count; i++)
            {
                Canvas cns = (Canvas)MainStack.Children[i];

                for (int j = 0; j < cns.Children.Count; j++)
                {
                    var sender = cns.Children[j];

                    if (sender is ComboBox)
                    {
                        ComboBox cb = (ComboBox)sender;
                        if (cb.SelectedIndex != -1)
                        {
                            if (cb.Name.Contains("CB"))
                                blocks[i].Type.typeID = cb.SelectedIndex;
                            if (cb.Name.Contains("DICT"))
                                blocks[i].Value = cb.SelectedIndex.ToString();
                        }
                    }

                    if (sender is TextBox)
                    {
                        TextBox tb = (TextBox)sender;
                        if (!string.IsNullOrEmpty(tb.Text))
                        {
                            if (tb.Name.Contains("MIN"))
                                blocks[i].MinInterval = (tb.Text);
                            else if (tb.Name.Contains("MAX"))
                                blocks[i].MaxInterval = (tb.Text);
                            else if (tb.Name.Contains("STR"))
                                blocks[i].Value = (tb.Text);
                        }
                    }

                    if (sender is DateTimePicker)
                    {
                        DateTimePicker dtp = (DateTimePicker)sender;
                        if (!string.IsNullOrEmpty(dtp.Text))
                        {
                            if (dtp.Name.Contains("DTPMIN"))
                                blocks[i].MinInterval = DateTime.Parse(dtp.Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            if (dtp.Name.Contains("DTPMAX"))
                                blocks[i].MaxInterval = DateTime.Parse(dtp.Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    if (sender is DatePicker)
                    {
                        DatePicker dtp = (DatePicker)sender;
                        if (!string.IsNullOrEmpty(dtp.Text))
                        {
                            if (dtp.Name.Contains("DPMIN"))
                                blocks[i].MinInterval = DateTime.Parse(dtp.SelectedDate.ToString()).ToString("yyyy-MM-dd");
                            if (dtp.Name.Contains("DPMAX"))
                                blocks[i].MaxInterval = DateTime.Parse(dtp.SelectedDate.ToString()).ToString("yyyy-MM-dd");
                        }
                    }
                    if (sender is TimePicker)
                    {
                        TimePicker dtp = (TimePicker)sender;
                        if (!string.IsNullOrEmpty(dtp.Text))
                        {
                            if (dtp.Name.Contains("TPMIN"))
                                blocks[i].MinInterval = DateTime.Parse(dtp.Value.ToString()).ToString("HH:mm:ss");
                            if (dtp.Name.Contains("TPMAX"))
                                blocks[i].MaxInterval = DateTime.Parse(dtp.Value.ToString()).ToString("HH:mm:ss");
                        }
                    }
                }
            }
        }

        private void GetCode()
        {
            int amount = int.Parse(InputLinesAmount.Text);
            string result = "";
            int GlobalAutoIncrement = 0;
            int SelfAutoIncrement = 0;

            for (int i = 0; i < amount; i ++)
            {
                result += "(";
                for (int j = 0; j < blocks.Count; j++)
                {
                    switch (blocks[j].Type.typeLabel)
                    {
                        case BlockType._typeLabel.Zero:
                            result += "0";
                            break;
                        case BlockType._typeLabel.Empty:
                            result += "\"\"";
                            break;
                        case BlockType._typeLabel.GlobalAutoIncrement:
                            result += GlobalAutoIncrement.ToString() + "";
                            break;
                        case BlockType._typeLabel.LocalAutoIncrement:
                            result += SelfAutoIncrement + "";
                            SelfAutoIncrement++;
                            break;
                        case BlockType._typeLabel.RndNumber:
                            result += rnd.Next(int.Parse(blocks[j].MinInterval), int.Parse(blocks[j].MaxInterval));
                            break;
                        case BlockType._typeLabel.RndString:
                            result += "\"" + GetRndString(int.Parse(blocks[j].MinInterval), int.Parse(blocks[j].MaxInterval)) + "\"";
                            break;
                        case BlockType._typeLabel.RndTime:
                            result += "\"" + GetRndTime(blocks[j].MinInterval, blocks[j].MaxInterval) + "\"";
                            break;
                        case BlockType._typeLabel.RndDate:
                            result += "\"" + GetRndDate(blocks[j].MinInterval, blocks[j].MaxInterval) + "\"";
                            break;
                        case BlockType._typeLabel.RndDateTime:
                            result += "\"" + GetRndDateTime(blocks[j].MinInterval, blocks[j].MaxInterval) + "\"";
                            break;                        
                        case BlockType._typeLabel.CustomDict:
                            result += "\"" + GlobalDicts.getRandomLineFromDict(int.Parse(blocks[j].Value)) + "\"";
                            break;
                        case BlockType._typeLabel.CustomString:
                            result += "\"" + blocks[j].Value + "\"";
                            break;
                        case BlockType._typeLabel.DuplicateParam:
                            int val = int.Parse(blocks[j].Value);
                            if (val < blocks.Count && val >= 0)
                                result += "\"" + blocks[val].Value + "\"";
                            else
                                result += "\"\"";
                            break;
                    }

                    result += ", ";
                }
                GlobalAutoIncrement++;
                result = result.Substring(0, result.Length - 2) + "),\n";
            }

            result = result.Substring(0, result.Length - 2) + ";\n";
            InputLinesAmount.Text = "";

            File.WriteAllText(savePath, result, Encoding.UTF8);
            Process.Start(savePath);
        }

        private string GetRndString(int min, int max)
        {
            string str = "";
            string alph = "abcdefghijklmnopqrstvwxyzABCDEFGHIJKLMNOPQRSTVWXYZ";

            if (max == 0)
                max = 8;

            for (int i = 0; i < rnd.Next(min, max); i++)
            {
                str += alph[rnd.Next(0, alph.Length)];    
            }

            return str;
        }

        private string GetRndDate(string min, string max)
        {
            TimeSpan timeSpan = DateTime.Parse(max) - DateTime.Parse(min);
            TimeSpan newTimeSpan = new TimeSpan(0, rnd.Next(0, (int)timeSpan.TotalMinutes), 0);
            DateTime newDate = DateTime.Parse(min) + newTimeSpan;
            return newDate.ToString("yyyy-MM-dd");
        }

        private string GetRndTime(string min, string max)
        {
            TimeSpan timeSpan = DateTime.Parse(max) - DateTime.Parse(min);
            TimeSpan newTimeSpan = new TimeSpan(0, 0, rnd.Next(0, (int)timeSpan.TotalSeconds));
            DateTime newDate = DateTime.Parse(min) + newTimeSpan;
            return newDate.ToString("HH:mm:ss");
        }
        
        private string GetRndDateTime(string min, string max)
        {
            TimeSpan timeSpan = DateTime.Parse(max) - DateTime.Parse(min);
            TimeSpan newTimeSpan = new TimeSpan(0, 0, rnd.Next(0, (int)timeSpan.TotalSeconds));
            DateTime newDate = DateTime.Parse(min) + newTimeSpan;
            return newDate.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int index = int.Parse(btn.Name.Replace("BTN", ""));
            RemoveBlock(index);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isDictUpdating)
            {
                ComboBox cb = (ComboBox)sender;
                int index = int.Parse(cb.Name.Replace("CB", ""));

                // Какая-то слишком умная незадокументированная проверка
                if (MainStack.Children.Count > index)
                    ChangeInputState((Canvas)MainStack.Children[index], cb.SelectedIndex, index);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (!InputNumbersRegex.IsMatch(tb.Text))
                tb.Text = "";
        }

        private void ShowDictsBtn_Click(object sender, RoutedEventArgs e)
        {
            DictWindow window = new DictWindow();
            window.ShowDialog();
        }
    }
}
