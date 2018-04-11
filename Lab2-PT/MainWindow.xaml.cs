using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;

namespace Lab2_PT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentPath;

        public MainWindow()
        {
            InitializeComponent();

            this.LoadTree("C: \\Users\\Paweł_2\\Pulpit\\Testowy");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var path = dialog.SelectedPath;
                    filepathBox.Text = path;
                    this.LoadTree(path);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.LoadTree(this.filepathBox.Text);
        }

        private void LoadTree(string pathToFile)
        {
            this.currentPath = pathToFile;
            try
            {
                this.treeView.Items.Clear();

                List<MyTreeViewItem> results = FileRetriever.RetrieveFiles(pathToFile);

                foreach (var item in results)
                {
                    this.treeView.Items.Add(item);
                }
            }

            catch (Exception)
            {
                var result = System.Windows.MessageBox.Show("No such directory!", "Error", MessageBoxButton.OK);

                this.filepathBox.Text = "";
            }
        }

        private void SolutionTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem SelectedItem = treeView.SelectedItem as TreeViewItem;
            switch (SelectedItem.Tag.ToString())
            {
                case "File":
                    treeView.ContextMenu = treeView.Resources["FileContext"] as System.Windows.Controls.ContextMenu;
                    break;
                case "Directory":
                    treeView.ContextMenu = treeView.Resources["FolderContext"] as System.Windows.Controls.ContextMenu;
                    break;
            }
        }

        private void AddFile(object sender, RoutedEventArgs e)
        {
            MyTreeViewItem SelectedItem = treeView.SelectedItem as MyTreeViewItem;
            string prefix = SelectedItem.LinkPath;

            this.AddGrid.Visibility = Visibility.Visible;
        }

        private void CreateFile(object sender, RoutedEventArgs e)
        {
            if (nameInput.Text == string.Empty)
            {
                System.Windows.MessageBox.Show("No file name given!", "Error", MessageBoxButton.OK);
            }
            else
            {
                string prefix = (this.treeView.SelectedItem as MyTreeViewItem).LinkPath + "\\";
                MyTreeViewItem toAdd = new MyTreeViewItem { Header = nameInput.Text, Tag = "File", LinkPath = prefix + nameInput.Text };
                (this.treeView.SelectedItem as MyTreeViewItem).Items.Add(toAdd);
                File.Create(toAdd.LinkPath);
            }
            this.nameInput.Text = string.Empty;
            this.AddGrid.Visibility = Visibility.Collapsed;
        }

        private void RemoveFile(object sender, RoutedEventArgs e)
        {
            MyTreeViewItem SelectedItem = treeView.SelectedItem as MyTreeViewItem;

            string pathToFile = SelectedItem.LinkPath;

            this.treeView.Items.Remove(SelectedItem);

            File.Delete(pathToFile);
            this.treeView.Items.Refresh();
        }
    }
}
