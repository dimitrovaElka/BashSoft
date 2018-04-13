namespace BashSoft
{
    using BashSoft.Attributes;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;
    using BashSoft.IO.Commands;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
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
            object[] parametersForConstruction = new object[]
            {
                input, data
            };

            Type typeOfCommand = Assembly.GetExecutingAssembly()
                .GetTypes()
                .First(type => type.GetCustomAttributes(typeof(AliasAttribute))
                .Where(atr => atr.Equals(command))
                .ToArray().Length > 0);

            Type typeOfInterpreter = typeof(CommandInterpreter);

            Command exe = (Command)Activator.CreateInstance(typeOfCommand, parametersForConstruction);

            FieldInfo[] fieldsOfCommand = typeOfCommand
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo[] fieldsOfInterpreter = typeOfInterpreter
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var fieldOfCommand in fieldsOfCommand)
            {
                Attribute atrAttribute = fieldOfCommand.GetCustomAttribute(typeof(InjectAttribute));
                if (atrAttribute != null)
                {
                    if (fieldsOfInterpreter.Any(x => x.FieldType == fieldOfCommand.FieldType))
                    {
                        fieldOfCommand.SetValue(exe,
                            fieldsOfInterpreter.First(x => x.FieldType == fieldOfCommand.FieldType)
                            .GetValue(this));
                    }
                }
            }

            return exe;
            //switch (command)
            //{
            //    case "open":
            //        return new OpenFileCommand(input, data);
            //    //open – opens a file
            //    case "mkdir":
            //        return new MakeDirectoryCommand(input, data);
            //    // mkdir directoryName – create a directory in the current directory
            //    case "ls":
            //        return new TraverseFoldersCommand(input, data);
            //    //ls (depth) – traverse the current directory to the given depth
            //    case "cmp":
            //        return new CompareFilesCommand(input, data);
            //    //cmp absolutePath1 absolutePath2 – comparing two files by given two absolute paths 
            //    case "cdrel":
            //        return new ChangeRelativePathCommand(input, data);
            //    //break;
            //    case "cdabs":
            //        return new ChangeAbsolutePathCommand(input, data);
            //    //changeDirAbs absolutePath – change the current directory by an absolute path
            //    case "readdb":
            //        return new ReadDatabaseCommand(input, data);
            //    //readDb dataBaseFileName – read students database by a given name of the database file which is placed in the current folder
            //    case "dropdb":
            //        return new DropDatabaseCommand(input, data);
            //    //break;
            //    case "show":
            //        return new ShowCourseCommand(input, data);
            //    // show courseName (username) – user name may be omitted
            //    case "help":
            //        //help – get help
            //        return new GetHelpCommand(input, data);
            //    case "filter":
            //        //filter courseName poor/average/excellent take 2/10/42/all – filter students from а given course by a given filter option and add quantity for the number of students to take or all if you want to take all the students matching the current filter option
            //        return new PrintFilteredStudentsCommand(input, data);
            //    case "order":
            //        //order courseName ascending/descending take 3/26/52/all – order student from a given course by ascending or descending order and then taking some quantity of the filter or all that match it
            //        return new PrintOrderedStudentsCommand(input, data);
            //    case "display":
            //        // two parameters in its input data - entity to display (students / courses) and the order in which 
            //        // to display the data (ascending/descending).
            //        return new DisplayCommand(input, data);
            //    //case "decorder":
            //    //    break;
            //    //case "download":
            //    //    //download (path of file) – download a file
            //    //    break;
            //    //case "downloadasynch":
            //    //    //downloadAsynch: (path of file) – download file asynchronously
            //    //    break;
            //    default:
            //        throw new InvalidCommandException(input);
            //        // DisplayInvalidCommandMessage(input);
            //}
        }
    }
}
