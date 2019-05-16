using Ionic.Zip;
using System;
using System.IO;
using System.Linq;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace RarExtract
{
    class Program
    {
        public static int counter = 0;
        //public static int deletedFiles = 0;

        //public static List<string> FilesToDelete;
        static void Main(string[] args)
        {
            //FilesToDelete = new List<string>();
            Console.ForegroundColor = ConsoleColor.White;

            var sourceFolder = "";
            Console.WriteLine("Enter the path to source folder: ");
            sourceFolder = Console.ReadLine();
            

            string[] AllFiles = GetFilesFromSourceFolder(sourceFolder);

            foreach (var file in AllFiles)
            {
                if (file.Contains(".rar"))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Unrar(file);
                    //FilesToDelete.Add(file);
                }
                else if (file.Contains(".zip"))
                {
                    Unzip(file);
                    //FilesToDelete.Add(file);
                }
            }

            Console.WriteLine("{0} files has been extracted", counter);
            Console.WriteLine();
            //Console.WriteLine("Do you wish to delete the zip and rar files? Y/N");
            //string answer = Console.ReadLine();
            //if (answer.ToLower() == "y")
            //{
                //DeleteFiles();
                //Console.WriteLine("{0} files has been deleted!", FilesToDelete);
            //}

            Console.WriteLine("Click enter to exit the program");


            Console.ReadLine();
        }
        private static string[] GetFilesFromSourceFolder(string sourceFolder)
        {
            string[] allfiles = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories);
            return allfiles;
        }
        private static void Unzip(string archiveFile)
        {

            ZipFile zipFile = new ZipFile(archiveFile);
            string directory = GetFolder(archiveFile);
            try
            {
                Console.WriteLine("Extracting file {0}", archiveFile);
                zipFile.ExtractAll(directory);
                counter++;
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }

        }

        private static void Unrar(string archiveFile)
        {
            string directory = GetFolder(archiveFile);
            try
            {
                var archive = SharpCompress.Archives.Rar.RarArchive.Open(archiveFile);
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {

                    entry.WriteToDirectory(directory, new ExtractionOptions() { });
                    counter++;
                    Console.WriteLine("Extracting file {0}", archiveFile);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }

        private static void ErrorMessage(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static string GetFolder(string archiveFile)
        {
            int index = archiveFile.LastIndexOf("\\");
            string directory = archiveFile.Substring(0, index);
            return directory;
        }
        //private static void DeleteFiles()
        //{
        //    foreach (var file in FilesToDelete)
        //    {
        //        File.Delete(file);
        //        deletedFiles++;
        //    }
        //}

    }
}
