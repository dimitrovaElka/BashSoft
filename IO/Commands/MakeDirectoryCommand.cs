﻿using BashSoft.Contracts;
using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.IO.Commands
{
    public class MakeDirectoryCommand : Command
    {
        public MakeDirectoryCommand(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager) 
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 2)
            {
                throw new InvalidCommandException(String.Format(ExceptionMessages.InvalidCommand, this.Input));
            }
            string folderName = this.Data[1];
            this.InputOutputManager.CreateDirectoryInCurrentFolder(folderName);
        }
    }
}
