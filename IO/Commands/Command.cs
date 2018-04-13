using BashSoft.Contracts;
using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.IO.Commands
{
    public abstract class Command : IExecutable
    {
        private string input;
        private string[] data;

        protected Command(string input, string[] data)
        {
            this.Input = input;
            this.Data = data;
        }

        //public Command(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
        //{
        //    this.Input = input;
        //    this.Data = data;
        //    this.judge = judge;
        //    this.repository = repository;
        //    this.inputOutputManager = inputOutputManager;
        //}

        public string Input
        {
            get { return this.input; }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidStringException();
                }
                this.input = value;
            }
        }
        public string[] Data
        {
            get { return this.data; }
            private set
            {
                if (value == null || value.Length == 0)
                {
                    throw new NullReferenceException();
                }
                this.data = value;

            }
        }

        //private IContentComparer judge;
        //private IDatabase repository;
        //private IDirectoryManager inputOutputManager;

        //protected IContentComparer Judge
        //{
        //    get { return this.judge; }
        //}

        //protected IDatabase Repository
        //{
        //    get { return this.repository; }
        //}

        //protected IDirectoryManager InputOutputManager
        //{
        //    get { return this.inputOutputManager; }
        //}

        public abstract void Execute();
    }
}
