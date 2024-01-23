using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace wpfTreeView
{
    /// <summary>
    /// Convert a full path to a specific image type of a drive, folder or file
    /// </summary>
   
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Get full path
            var path = (string)value;

            //Check if the path is null, if so the ignore
            if (path == null)
                return null;

            //Get the name of the file/folder
            var name = MainWindow.GetFileFolderName(path);

            //By default we presume a file
            var image = "Images/file.png";

            //If the name is blank, we presume it is a drive
            if (string.IsNullOrEmpty(name))
            {
                image = "Images/drive.png";
            }
            else if (new System.IO.FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                image = "Images/folder_closed.png";

            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
