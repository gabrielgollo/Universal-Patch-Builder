using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Windows;

class FileInfo
{
    public string filePath;
    public string MD5;

    public FileInfo(string _filePath, string _MD5)
    {
        filePath = _filePath;
        MD5 = _MD5;
    }
}

namespace UniversalPatch_Builder
{
    class PatcherGenerator
    {
        public List<string> ReadAllFiles(string folderPath)
        {
            List<string> fileList = new List<string>();
            try
            {
                foreach (string subFolderPath in Directory.GetDirectories(folderPath))
                {
                    List<string> subFiles = ReadAllFiles(subFolderPath);
                    foreach(string subFile in subFiles)
                    {
                        fileList.Add(subFile);
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }


            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                fileList.Add(filePath);
            }

            return fileList;
        }

        public List<FileInfo> ProcessFiles(List<string> filePathList)
        {
            List<FileInfo> filesInfos = new List<FileInfo>();
            foreach (string filePath in filePathList)
            {
                string MD5 = CalculateMD5(filePath.Replace("\r", "").Replace("\n", ""));
                FileInfo fileInfo = new FileInfo(filePath, MD5);

                filesInfos.Add(fileInfo);
            }

            return filesInfos;
        }

        private string CalculateMD5(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }
    }
}
