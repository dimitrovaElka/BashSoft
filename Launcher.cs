
using System;

namespace BashSoft
{
    public class Launcher
    {
        //private InputReader reader;

        //public Launcher(InputReader reader)
        //{
        //    this.reader = reader;
        //}
        public static void Main()
        {
            // IOManager.TraverseDirectory(@"D:\razni");
            //StudentsRepository.InitializeData();
            //StudentsRepository.GetAllStudentsFromCourse("Unity");
            //Tester.CompareContent(@"C:\BashSoft-FirstWeek\03. CSharp-Advanced-Files-And-Directories-Lab\test2.txt", @"C:\BashSoft-FirstWeek\03. CSharp-Advanced-Files-And-Directories-Lab\test3.txt");
            // IOManager.CreateDirectoryInCurrentFolder("pesho");
            //IOManager.ChangeCurrentDirectoryAbsolute(@"C:\Windows");
            //IOManager.TraverseDirectory(20);
            // IOManager.CreateDirectoryInCurrentFolder("*2");
            //IOManager.ChangeCurrentDirectoryRelative("..");
            //IOManager.ChangeCurrentDirectoryRelative("..");
            //IOManager.ChangeCurrentDirectoryRelative("..");
            //IOManager.ChangeCurrentDirectoryRelative("..");
            //IOManager.ChangeCurrentDirectoryRelative("..");
            //IOManager.ChangeCurrentDirectoryRelative("..");
            //IOManager.ChangeCurrentDirectoryRelative("..");

            Tester tester = new Tester();
            IOManager ioManager = new IOManager();
            StudentRepository repo = new StudentRepository(new RepositorySorter(), new RepositoryFilter());

            CommandInterpreter currentInterpreter = new CommandInterpreter(tester, repo, ioManager);
            InputReader reader = new InputReader(currentInterpreter);
            reader.StartReadingCommands();
        }
    }
}
