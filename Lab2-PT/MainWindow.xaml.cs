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

namespace Lab2_PT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                this.treeView.Items.Clear();

                List<MyTreeViewItem> results = FileRetriever.RetrieveFiles(filepathBox.Text);

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
    }
}
