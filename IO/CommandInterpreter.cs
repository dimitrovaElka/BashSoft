namespace BashSoft
{
    using BashSoft.Contracts;
    using BashSoft.Exceptions;
    using BashSoft.IO.Commands;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    public class CommandInterpreter : IInterpreter
    {
        private IContentComparer judge;
        private IDatabase repository;
        private IDirectoryManager inputOutputManager;

        public CommandInterpreter(IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
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
                IExecutable command = this.ParseCommand(input, data, commandName);
                command.Execute();
            }
            catch (Exception ex)
            {
                OutputWriter.DisplayException(ex.Message);
            }

        }

        private IExecutable ParseCommand(string input, string[] data, string command)
        {
            switch (command)
            {
                case "open":
                    return new OpenFileCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                //open – opens a file
                case "mkdir":
                    return new MakeDirectoryCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                // mkdir directoryName – create a directory in the current directory
                case "ls":
                    return new TraverseFoldersCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                //ls (depth) – traverse the current directory to the given depth
                case "cmp":
                    return new CompareFilesCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                //cmp absolutePath1 absolutePath2 – comparing two files by given two absolute paths 
                case "cdrel":
                    return new ChangeRelativePathCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                //break;
                case "cdabs":
                    return new ChangeAbsolutePathCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                //changeDirAbs absolutePath – change the current directory by an absolute path
                case "readdb":
                    return new ReadDatabaseCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                //readDb dataBaseFileName – read students database by a given name of the database file which is placed in the current folder
                case "dropdb":
                    return new DropDatabaseCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                //break;
                case "show":
                    return new ShowCourseCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                // show courseName (username) – user name may be omitted
                case "help":
                    //help – get help
                    return new GetHelpCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                case "filter":
                    //filter courseName poor/average/excellent take 2/10/42/all – filter students from а given course by a given filter option and add quantity for the number of students to take or all if you want to take all the students matching the current filter option
                    return new PrintFilteredStudentsCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                case "order":
                    //order courseName ascending/descending take 3/26/52/all – order student from a given course by ascending or descending order and then taking some quantity of the filter or all that match it
                    return new PrintOrderedStudentsCommand(input, data, this.judge, this.repository, this.inputOutputManager);
                case "display":
                    // two parameters in its input data - entity to display (students / courses) and the order in which 
                    // to display the data (ascending/descending).
                    return new DisplayCommand(input, data, this.judge, this.repository, this.inputOutputManager);
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
            }
        }
    }
}
