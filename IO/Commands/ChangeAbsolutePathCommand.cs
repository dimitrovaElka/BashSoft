using BashSoft.Attributes;
using BashSoft.Contracts;
using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.IO.Commands
{
    [Alias("cdabs")]
    public class ChangeAbsolutePathCommand : Command
    {
        [Inject]
        private IDirectoryManager inputOutputManager;

        public ChangeAbsolutePathCommand(string input, string[] data) 
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 2)
            {
                throw new InvalidCommandException(String.Format(ExceptionMessages.InvalidCommand, this.Input));
            }
            string absolutePath = this.Data[1];

            this.inputOutputManager.ChangeCurrentDirectoryAbsolute(absolutePath);
        }
    }
}
