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
using System.Windows.Navigation;
//using System.Windows.Shapes;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.IO;

namespace Rename_Utility
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

        public void browseFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPEG Images|*.jpeg|JPG Images|*.jpg|PNG Images|*.png";
            openFileDialog.Multiselect = true;
            openFileDialog.ShowDialog();

            String files = (openFileDialog.FileNames.Length).ToString();
            String[] fileNames = openFileDialog.FileNames;
            checkForDuplicateFiles(fileNames);
            lblFiles.Content = files;            
        }

        public void checkForDuplicateFiles(String[] fileNames)
        {
            Dictionary<String,String> fileDictionary = new Dictionary<String, String>();
            String key;
            new Task(delegate {

                for (int i = 0; i < fileNames.Length; i++)
                {
                    key = BitConverter.ToString(MD5.Create().ComputeHash(File.Create(fileNames[i]))).Replace("-", "");
                    if (!fileDictionary.ContainsKey(key))
                        fileDictionary.Add(key, fileNames[i]);
                }

                lblDuplicateFiles.Dispatcher.Invoke( delegate {
                    lblDuplicateFiles.Content = (fileNames.Length - fileDictionary.Count);
                } );
                //lblDuplicateFiles.Content = (fileNames.Length - fileDictionary.Count);
                saveFiles(fileDictionary);
            }).Start();
            
            
        }

        public void saveFiles(Dictionary<String,String> fileDictionary)
        {
            
            String path = @"D:\Wallpaper Collection";
            Directory.CreateDirectory(path);
            int index = 0;
            foreach (var file in fileDictionary)
            {                
                
                FileInfo fileInfo = new FileInfo(file.Value);
                fileInfo.CopyTo(path+@"\w"+index+Path.GetExtension(file.Value),true);                
            }
        }
    }
}
