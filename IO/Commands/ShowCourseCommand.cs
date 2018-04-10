﻿using BashSoft.Contracts;
using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.IO.Commands
{
    public class ShowCourseCommand : Command
    {
        public ShowCourseCommand(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager) 
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                string courseName = this.Data[1];
                this.Repository.GetStudentsByCourse(courseName);
            }
            else if (this.Data.Length == 3)
            {
                string courseName = this.Data[1];
                string userName = this.Data[2];
                this.Repository.GetStudentMarkInCourse(courseName, userName);
            }
            else
            {
                throw new InvalidCommandException(String.Format(ExceptionMessages.InvalidCommand, this.Input));
            }
        }
    }
}
