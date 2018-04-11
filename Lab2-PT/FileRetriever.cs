using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Lab2_PT
{
    class FileRetriever
    {
        private static List<MyTreeViewItem> items;

        public static List<MyTreeViewItem> RetrieveFiles(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            FileSystemInfo[] internals;

            items = new List<MyTreeViewItem>();

            if (di.Exists)
            {
                internals = di.GetFileSystemInfos();

                foreach (var item in internals)
                {
                    items.Add(RecursiveSubdirectoriesPrinter(item));
                }
            }
            else
            {
                throw new Exception();
            }


            return items;
        }

        private static MyTreeViewItem RecursiveSubdirectoriesPrinter(FileSystemInfo analysed, MyTreeViewItem parent = null)
        {

            if ((analysed.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                MyTreeViewItem toReturn = new MyTreeViewItem();
                if (((DirectoryInfo)analysed).GetFileSystemInfos().Any())
                {
                    foreach (var item in ((DirectoryInfo)analysed).GetFileSystemInfos())
                    {
                        toReturn.Items.Add(RecursiveSubdirectoriesPrinter(item));
                    }
                }
                toReturn.Header = analysed.Name;
                toReturn.LinkPath = analysed.FullName;
                toReturn.Tag = "Directory";

                return toReturn;
            }

            else
            {
                return new MyTreeViewItem() { Header = analysed.Name, LinkPath = analysed.FullName, Tag = "File"};
            }
        }
    }
}
