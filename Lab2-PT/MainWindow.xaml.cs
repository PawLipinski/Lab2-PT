using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;

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

            this.LoadTree("C:\\Users\\pawel.lipinski\\Documents\\TestFolder");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var path = dialog.SelectedPath;
                    filepathBox.Text = path;
                }

                this.Button_Click_1(sender, e);
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
            SelectedItem.IsSelected = true;
        }

        private void AddFile(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveFile(object sender, RoutedEventArgs e)
        {
            MyTreeViewItem SelectedItem = treeView.SelectedItem as MyTreeViewItem;

            string pathToFile = SelectedItem.LinkPath;

            this.treeView.Items.Remove(SelectedItem);
            this.treeView.Items.Refresh();

            File.Delete(pathToFile);
        }
    }
}
