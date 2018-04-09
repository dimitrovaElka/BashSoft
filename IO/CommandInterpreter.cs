namespace BashSoft
{
    using BashSoft.Exceptions;
    using BashSoft.IO.Commands;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    public class CommandInterpreter
    {
        private Tester judge;
        private StudentRepository repository;
        private IOManager inputOutputManager;

        public CommandInterpreter(Tester judge, StudentRepository repository, IOManager inputOutputManager)
        {
            this.judge = judge;
            this.repository = repository;
            this.inputOutputManager = inputOutputManager;
        }
        public void InterpretCommand(string input)
        {
            string[] data = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string commandName = data[0].ToLower();
            try
            {
                Command command = this.ParseCommand(input, data, commandName);
                command.Execute();
            }
            catch (Exception ex)
            {
                OutputWriter.DisplayException(ex.Message);
            }
            //catch (ArgumentOutOfRangeException aoore)
            //{
            //    OutputWriter.DisplayException(aoore.Message);
            //}
            //catch (ArgumentException ae)
            //{
            //    OutputWriter.DisplayException(ae.Message);
            //}
            //catch (Exception e)
            //{
            //    OutputWriter.DisplayException(e.Message);
            //}

        }

        private Command ParseCommand(string input, string[] data, string command)
        {
            switch (command)
            {
                case "open":
                    return new OpenFileCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                    //TryOpenFile(input, data);
                    //open – opens a file
                    //break;
                case "mkdir":
                    return new MakeDirectoryCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                    // TryCreateDirectory(input, data);
                    // mkdir directoryName – create a directory in the current directory
                    // break;
                case "ls":
                    return new TraverseFoldersCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                    //TryTraverseFolders(input, data);
                    //ls (depth) – traverse the current directory to the given depth
                    //break;
                case "cmp":
                    return new CompareFilesCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                    //TryCompareFiles(input, data);
                    //cmp absolutePath1 absolutePath2 – comparing two files by given two absolute paths 
                    //break;
                case "cdrel":
                    return new ChangeRelativePathCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                //TryChangePathRelatively(input, data);
                //break;
                //changeDirRel relativePath – change the current directory by a relative path
                case "cdabs":
                    return new ChangeAbsolutePathCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                    //TryChangePathAbsolute(input, data);
                    //changeDirAbs absolutePath – change the current directory by an absolute path
                    //break;
                case "readdb":
                    return new ReadDatabaseCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                    //TryReadDatabaseFromFile(input, data);
                    //readDb dataBaseFileName – read students database by a given name of the database file which is placed in the current folder
                    //break;
                case "dropdb":
                    return new DropDatabaseCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                    //TryDropDb(input, data);
                    //break;
                case "show":
                    return new ShowCourseCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                    // TryShowWantedData(input, data);
                    // show courseName (username) – user name may be omitted
                    // break;
                case "help":
                    //help – get help
                    return new GetHelpCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                    //TryGetHelp();
                    //break;
                case "filter":
                    //filter courseName poor/average/excellent take 2/10/42/all – filter students from а given course by a given filter option and add quantity for the number of students to take or all if you want to take all the students matching the current filter option
                    return new PrintFilteredStudentsCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                    //TryFilterAndTake(input, data);
                    //break;
                case "order":
                    //order courseName ascending/descending take 3/26/52/all – order student from a given course by ascending or descending order and then taking some quantity of the filter or all that match it
                    return new PrintOrderedStudentsCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                    //TryOrderAndTake(input, data);
                    //break;
                //case "decorder":
                //    break;
                //case "download":
                //    //download (path of file) – download a file
                //    break;
                //case "downloadasynch":
                //    //downloadAsynch: (path of file) – download file asynchronously
                //    break;
                default:
                    throw new InvalidCommandException(input);
                   // DisplayInvalidCommandMessage(input);
                   // break;
            }
        }

        //private void TryDropDb(string input, string[] data)
        //{
        //    if (data.Length != 1)
        //    {
        //        this.DisplayInvalidCommandMessage(input);
        //        return;
        //    }

        //    this.repository.UnloadData();
        //    OutputWriter.WriteMessageOnNewLine("Database dropped!");
        //}

        //private void TryOrderAndTake(string input, string[] data)
        //{
        //    if (data.Length == 5)
        //    {
        //        string courseName = data[1];
        //        string comparison = data[2].ToLower();
        //        string takeCommand = data[3].ToLower();
        //        string takeQuantity = data[4].ToLower();

        //        TryParseParametersForOrderAndTake(takeCommand, takeQuantity, courseName, comparison);
        //    }
        //    else
        //    {
        //        DisplayInvalidCommandMessage(input);
        //    }
        //}

       

        //private void TryFilterAndTake(string input, string[] data)
        //{
        //    if (data.Length == 5)
        //    {
        //        string courseName = data[1];
        //        string filter = data[2].ToLower();
        //        string takeCommand = data[3].ToLower();
        //        string takeQuantity = data[4].ToLower();
        //        TryParseParametersForFilterAndTake(takeCommand, takeQuantity, courseName, filter);
        //    }
        //    else
        //    {
        //        DisplayInvalidCommandMessage(input);
        //    }
        //}

        //private void TryParseParametersForFilterAndTake(string takeCommand, string takeQuantity, string courseName, string filter)
        //{
        //    if (takeCommand == "take")
        //    {
        //        if (takeQuantity == "all")
        //        {
        //            this.repository.FilterAndTake(courseName, filter);
        //        }
        //        else
        //        {
        //            int studentsToTake;
        //            bool hasParsed = int.TryParse(takeQuantity, out studentsToTake);
        //            if (hasParsed)
        //            {
        //                this.repository.FilterAndTake(courseName, filter, studentsToTake);
        //            }
        //            else
        //            {
        //                OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);
        //    }
        //}

        //private void TryShowWantedData(string input, string[] data)
        //{
        //    if (data.Length == 2)
        //    {
        //        string courseName = data[1];
        //        this.repository.GetAllStudentsFromCourse(courseName);
        //    }
        //    else if (data.Length == 3)
        //    {
        //        string courseName = data[1];
        //        string userName = data[2];
        //        this.repository.GetStudentScoresFromCourse(courseName, userName);
        //    }
        //    else
        //    {
        //        DisplayInvalidCommandMessage(input);
        //    }
        //}

        //private void TryGetHelp()
        //{
        //    OutputWriter.WriteMessageOnNewLine($"{new string('_', 100)}");
        //    OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "make directory - mkdir: path "));
        //    OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "traverse directory - ls: depth "));
        //    OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "comparing files - cmp: path1 path2"));
        //    OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "change directory - changeDirREl:relative path"));
        //    OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "change directory - changeDir:absolute path"));
        //    OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "read students data base - readDb: path"));
        //    OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "filter {courseName} excelent/average/poor  take 2/5/all students - filterExcelent (the output is written on the console)"));
        //    OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "order increasing students - order {courseName} ascending/descending take 20/10/all (the output is written on the console)"));
        //    OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "download file - download: path of file (saved in current directory)"));
        //    OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "download file asinchronously - downloadAsynch: path of file (save in the current directory)"));
        //    OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "get help – help"));
        //    OutputWriter.WriteMessageOnNewLine($"{new string('_', 100)}");
        //    OutputWriter.WriteEmptyLine();
        //}

        //private void TryReadDatabaseFromFile(string input, string[] data)
        //{
        //    if (!CheckForParams(input, data, 2))
        //    {
        //        return;
        //    }
        //    string fileName = data[1];
        //    this.repository.LoadData(fileName);
        //}

        //private void TryChangePathAbsolute(string input, string[] data)
        //{
        //    if (!CheckForParams(input, data, 2))
        //    {
        //        return;
        //    }
        //    string absolutePath = data[1];

        //    this.inputOutputManager.ChangeCurrentDirectoryAbsolute(absolutePath);
        //}

        //private void TryChangePathRelatively(string input, string[] data)
        //{
        //    if (!CheckForParams(input, data, 2))
        //    {
        //        return;
        //    }
        //    string relPath = data[1];
        //    this.inputOutputManager.ChangeCurrentDirectoryRelative(relPath);
        //}

        //private void TryCompareFiles(string input, string[] data)
        //{
        //    if (!CheckForParams(input, data, 3))
        //    {
        //        return;
        //    }
        //    string firstPath = data[1];
        //    string secondPath = data[2];

        //    this.judge.CompareContent(firstPath, secondPath);

        //}

        //private void TryTraverseFolders(string input, string[] data)
        //{
        //    if (data.Length == 1)
        //    {
        //        this.inputOutputManager.TraverseDirectory(0);
        //    }
        //    else if (data.Length == 2)
        //    {
        //        int depth;
        //        bool hasParsed = int.TryParse(data[1], out depth);
        //        if (hasParsed)
        //        {
        //            this.inputOutputManager.TraverseDirectory(depth);
        //        }
        //        else
        //        {
        //            OutputWriter.DisplayException(ExceptionMessages.UnableToParseNumber);
        //        }
        //    }
        //    else
        //    {
        //        DisplayInvalidCommandMessage(input);
        //    }
        //}

        //private void TryCreateDirectory(string input, string[] data)
        //{
        //    if (!CheckForParams(input, data, 2))
        //    {
        //        return;
        //    }
        //    string folderName = data[1];
        //    this.inputOutputManager.CreateDirectoryInCurrentFolder(folderName);
        //}

        //private void TryOpenFile(string input, string[] data)
        //{
        //    if (!CheckForParams(input, data, 2))
        //    {
        //        return;
        //    }
        //    string fileName = data[1];
        //    Process.Start(SessionData.currentPath + "\\" + fileName);
        //}

        //private bool CheckForParams(string input, string[] data, int countParams)
        //{
        //    if (data.Length != countParams)
        //    {
        //        DisplayInvalidCommandMessage(input);
        //        return false;
        //    }
        //    return true;
        //}

        //private void DisplayInvalidCommandMessage(string input)
        //{
        //    string message = String.Format(ExceptionMessages.InvalidCommand, input);
        //    OutputWriter.DisplayException(message);
        //}
    }
}
