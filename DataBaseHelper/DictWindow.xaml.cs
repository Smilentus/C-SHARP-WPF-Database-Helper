using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataBaseHelper
{
    /// <summary>
    /// Логика взаимодействия для DictWindow.xaml
    /// </summary>
    public partial class DictWindow : Window
    {
        public int currentOpenedDict = 0;
        bool autoSave = false;

        public DictWindow()
        {
            InitializeComponent();

            foreach (var dict in GlobalDicts.getDictsNames())
                dictsListBox.Items.Add(dict);
        }

        private void dictsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentOpenedDict = dictsListBox.SelectedIndex;
            MainTextBox.IsEnabled = true;
            NameText.Content = GlobalDicts.dicts[currentOpenedDict].dictName;
            MainTextBox.Text = GlobalDicts.dicts[currentOpenedDict].getDictDataStringify();
        }

        private void SaveNewDataToDicts()
        {
            GlobalDicts.dicts[currentOpenedDict].dictData = MainTextBox.Text.Split(';');
            GlobalDicts.SaveDictionaries();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveNewDataToDicts();
        }

        private void autosave_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            autoSave = (bool)autosave_checkbox.IsChecked;
        }

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (autoSave)
            {
                SaveNewDataToDicts();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }
    }
}
