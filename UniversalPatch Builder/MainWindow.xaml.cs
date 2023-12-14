using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UniversalPatch_Builder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PatcherGenerator patcher;
        public MainWindow()
        {
            InitializeComponent();
            patcher = new PatcherGenerator();
        }

        private void SelecionarPasta_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {

                DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    System.Windows.MessageBox.Show(dialog.SelectedPath);
                    caminhoDaPastaTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void ListFiles(object sender, RoutedEventArgs e)
        {
            try
            {
                string folderPath = caminhoDaPastaTextBox.Text;

                if (folderPath.Length > 10)
                {

                    filesListBox.Items.Clear(); 
                    List<string> files = patcher.ReadAllFiles(folderPath);
                    // string fileStr = string.Join(Environment.NewLine, files);

                    foreach(string file in files) {
                        filesListBox.Items.Add(file);
                    }

                }
                else
                {
                    throw new Exception("Selecione um caminho antes");
                }
            } catch (Exception error)
            {
                System.Windows.MessageBox.Show("Error: " + error.Message);
            }

        }

        private void CalcularMD5_Click(object sender, RoutedEventArgs e)
        {
            try {
                // Obtemos os caminhos dos arquivos da TextBox
                List<string> filePathList = new List<string>();

                foreach (object fileItem in filesListBox.Items)
                {
                    filePathList.Add(fileItem.ToString());
                }

                
                // Processamos os arquivos e obtemos informações
                List <FileInfo> filesInfos = patcher.ProcessFiles(filePathList);

                // Exibimos os resultados na nova TextBox
                md5ResultsTextBox.Text = "Resultados do MD5:\n";

                foreach (FileInfo fileInfo in filesInfos)
                {
                    md5ResultsTextBox.Text += $"{fileInfo.filePath}: {fileInfo.MD5}\n";
                }
            } catch(Exception error)
            {
                System.Windows.MessageBox.Show("An error has occurred!" + error.Message);
            }
            
        }
    }
}
