using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace wpfTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region On Loaded
        /// <summary>
        /// When the application first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 
            //Get every logical drive on the machine
            foreach (var drive in Directory.GetLogicalDrives())
            {
                //Create a new item for each drive
                var item = new TreeViewItem()
                {
                    //Set the header
                    Header = drive,
                    //Set the full path
                    Tag = drive
                };

                //Add a dummy Item
                item.Items.Add(null);

                //Listen out for item being expanded
                item.Expanded += Folder_Expanded;

                //Add it to the treeview
                FolderView.Items.Add(item);
            }
        }

        #endregion

        #region Folder Expanded   
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Initial Checks
            var item = (TreeViewItem)sender;

            //If the item only contains dummy data
            if (item.Items.Count != 1 || item.Items[0] != null)
            {
                return;
            }

            //Clear Dummy data
            item.Items.Clear();

            //Get full path
            var fullPath = (string)item.Tag;
            #endregion
            #region Get Folders
            //Create a blank list 
            var directories = new List<string>();

            //Try and get directories from the folder, ignoring any issues doing so.
            try
            {
                var dirs = Directory.GetDirectories(fullPath);
                if (dirs.Length > 0)
                {
                    directories.AddRange(dirs);
                }
            }
            catch { }

            //Foreach directory...
            directories.ForEach(directorypath =>
            {
                //Create Directory Item
                var subItem = new TreeViewItem()
                {
                    //Set the header as folder name
                    Header = GetFileFolderName(directorypath),
                    //Set the tag as full path
                    Tag = directorypath
                };

                //Add a dummy Item
                subItem.Items.Add(null);

                //Listen out for item being expanded
                subItem.Expanded += Folder_Expanded;

                //Add it to the Parent
                item.Items.Add(subItem);
            });
            #endregion
            #region Get Files
            //Create a blank list for files
            var files = new List<string>();

            //Try and get files from the folder, ignoring any issues doing so.
            try
            {
                var fs = Directory.GetFiles(fullPath);
                if (fs.Length > 0)
                {
                    files.AddRange(fs);
                }
            }
            catch { }

            //Foreach file...
            files.ForEach(filepath =>
            {
                //Create file Item
                var subItem = new TreeViewItem()
                {
                    //Set the header as file name
                    Header = GetFileFolderName(filepath),
                    //Set the tag as full path
                    Tag = filepath
                };

                //Add it to the Parent
                item.Items.Add(subItem);
            });
            #endregion
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Find the file or folder name from full path
        /// </summary>
        /// <param name="path"> full path</param>
        /// <returns></returns>
        public static string GetFileFolderName(string path)
        {
            //If we have no path return empty
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            //Make all slashes back slashes
            var normalizepath = path.Replace('/', '\\');

            //Find the last backslash
            var lastindex = normalizepath.LastIndexOf('\\');

            //If we don't the backslash return the whole path
            if (lastindex <= 0)
                return path;

            //Return the name after the last backslash
            return path.Substring(lastindex + 1);
        }
        #endregion
    }
}
