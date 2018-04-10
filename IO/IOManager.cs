namespace BashSoft
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using BashSoft.Exceptions;
    using BashSoft.Contracts;

    public class IOManager : IDirectoryManager
    {
        public void TraverseDirectory(int depth)
        {
            OutputWriter.WriteEmptyLine();
            int initialIdentation = SessionData.currentPath.Split('\\').Length;
            Queue<string> subFolders = new Queue<string>();
            subFolders.Enqueue(SessionData.currentPath);
            while (subFolders.Count != 0)
            {
                var currentFolder = subFolders.Dequeue();
                int identation = currentFolder.Split('\\').Length - initialIdentation;
                if (depth - identation < 0)
                {
                    break;
                }
                OutputWriter.WriteMessageOnNewLine(string.Format($"{new string('-', identation)}{currentFolder}"));
                try
                {
                    foreach (var file in Directory.GetFiles(currentFolder))
                    {
                        int indexOfLastSlash = file.LastIndexOf("\\");
                        string fileName = file.Substring(indexOfLastSlash);
                        OutputWriter.WriteMessageOnNewLine(string.Format($"{new string('-', indexOfLastSlash)}{fileName}"));
                    }
                    foreach (string directoryPath in Directory.GetDirectories(currentFolder))
                    {
                        subFolders.Enqueue(directoryPath);
                    }

                }
                catch (UnauthorizedAccessException)
                {
                    OutputWriter.DisplayException(ExceptionMessages.UnauthorizedAccessExceptionMessage);
                }
 
            }
        }

        public void CreateDirectoryInCurrentFolder(string name)
        {
            // string path = GetCurrentDirectoryPath() + "\\" + name;
            string path = SessionData.currentPath + "\\" + name;
            // string path = Directory.GetCurrentDirectory() + "\\" + name;
            try
            {
                Directory.CreateDirectory(path);

            }
            catch (ArgumentException)
            {
                throw new InvalidFileNameException();
                    //ArgumentException(ExceptionMessages.ForbiddenSymbolsContainedInName);
            }
        }

        private string GetCurrentDirectoryPath()
        {
            string path = Directory.GetCurrentDirectory();
            return path;
        }

        public void ChangeCurrentDirectoryRelative(string relativePath)
        {
            if (relativePath == "..")
            {
                try
                {
                    string currentPath = SessionData.currentPath;
                    int indexOfLastSlash = currentPath.LastIndexOf("\\");
                    string newPath = currentPath.Substring(0, indexOfLastSlash);
                    SessionData.currentPath = newPath;
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new InvalidPathException();
                }
            }
            else
            {
                string currentPath = SessionData.currentPath;
                currentPath += "\\" + relativePath;
                ChangeCurrentDirectoryAbsolute(currentPath);
            }
        }

        public void ChangeCurrentDirectoryAbsolute(string absolutePath)
        {
            if (!Directory.Exists(absolutePath))
            {
                throw new InvalidPathException();

            }
            SessionData.currentPath = absolutePath;
        }
    }
}
