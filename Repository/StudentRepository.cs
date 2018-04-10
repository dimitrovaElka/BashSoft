
namespace BashSoft
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Text.RegularExpressions;
    using BashSoft.Models;
    using System.Linq;
    using BashSoft.Exceptions;
    using BashSoft.Contracts;
    using BashSoft.DataStuctures;

    public class StudentRepository : IDatabase
    {
        private bool isDataInitialzed = false;
        private Dictionary<string, ICourse> courses;
        private Dictionary<string, IStudent> students;
        private IDataFilter filter;
        private IDataSorter sorter;

        public StudentRepository(IDataSorter sorter, IDataFilter filter)
        {
            this.filter = filter;
            this.sorter = sorter;
            // this.studentByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
            
        }
        public void LoadData(string fileName)
        {
            if (this.isDataInitialzed)
            {
                OutputWriter.DisplayException(ExceptionMessages.DataAlreadyInitialisedException);
                return;
            }
           
            OutputWriter.WriteMessageOnNewLine("Reading data...");
            this.students = new Dictionary<string, IStudent>();
            this.courses = new Dictionary<string, ICourse>();
            ReadData(fileName);
        }

        public void UnloadData()
        {
            if (!this.isDataInitialzed)
            {
               throw new ArgumentException(ExceptionMessages.DataNotInitializedExceptionMessage);
            }
            this.students = null;
            this.courses = null;
            this.isDataInitialzed = false;
        }
        private void ReadData(string fileName)
        {
            string path = SessionData.currentPath + "\\" + fileName;
            if (!File.Exists(path))
            {
                throw new InvalidPathException();
            }
            // Course name – starts with a capital letter and may contain only small and capital English letters, plus ‘+’ or hashtag ‘#’. 
            // Course instance – should be in the format ‘Feb_2015’, e.g.containing the first three letters of the month, starting with a capital letter, followed by an underscore and the year. The year should be between 2014 and the current year.
            // regex: ([A-Z][a-zA-Z#+]*_[A-Z][a-z]{2}_\d{4})\s+

            // Username – starts with a capital letter and should be followed by at most 3 small letters after that. Then it should have 2 digits, followed by an underscore, followed by two to four digits.
            // ([A-Z][a-z]{0,3}\d{2}_\d{2,4})

            // string pattern = @"([A-Z][a-zA-Z#+]*_[A-Z][a-z]{2}_\d{4})\s+([A-Z][a-z]{0,3}\d{2}_\d{2,4})\s+(\d+)";
            string pattern = @"([A-Z][a-zA-Z#+]*_[A-Z][a-z]{2}_\d{4})\s+([A-Za-z]+\d{2}_\d{2,4})\s([\s0-9]+)";
            Regex rgx = new Regex(pattern);
            string[] allInputLines = File.ReadAllLines(path);

            for (int line = 0; line < allInputLines.Length; line++)
            {
                if (!string.IsNullOrEmpty(allInputLines[line]) && rgx.IsMatch(allInputLines[line]))
                {
                    Match currentMatch = rgx.Match(allInputLines[line]);
                    string courseName = currentMatch.Groups[1].Value;
                    string username = currentMatch.Groups[2].Value;
                    string scoresStr = currentMatch.Groups[3].Value;
                    try
                    {
                        int[] scores = scoresStr.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse).ToArray();
                        if (scores.Any(x => x > 100 || x < 0))
                        {
                            OutputWriter.DisplayException(ExceptionMessages.InvalidScore);
                            continue;
                        }
                        if (scores.Length > Course.NumberOfTaskOnExam)
                        {
                            OutputWriter.DisplayException(ExceptionMessages.InvalidNumberOfScores);
                            continue;
                        }
                        if (!this.students.ContainsKey(username))
                        {
                            this.students.Add(username, new Student(username));
                        }
                        if (!this.courses.ContainsKey(courseName))
                        {
                            this.courses.Add(courseName, new Course(courseName));
                        }
                        ICourse course = this.courses[courseName];
                        IStudent student = this.students[username];

                        student.EnrollInCourse(course);
                        student.SetMarksInCourse(courseName, scores);

                        course.EnrollStudent(student);
                    }
                    catch (FormatException fex)
                    {
                        OutputWriter.DisplayException(fex.Message + $"at line : {line}");
                    }
                }
            }
            isDataInitialzed = true;
            OutputWriter.WriteMessageOnNewLine("Data read!");
        }

        private bool IsQueryForCoursePossible(string courseName)
        {
            if (isDataInitialzed)
            {
                if (this.courses.ContainsKey(courseName))
                {
                    return true;
                }
                else
                {
                    throw new ArgumentException(ExceptionMessages.InexistingCourseInDataBase);
                }
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.DataNotInitializedExceptionMessage);
            }
        }

        private bool IsQueryForStudentPossible(string courseName, string studentUserName)
        {
            if (this.IsQueryForCoursePossible(courseName) && this.courses[courseName].StudentsByName.ContainsKey(studentUserName))
            {
                return true;
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.InexistingStudentInDataBase);
            }
        }

        public void GetStudentMarkInCourse(string courseName, string userName)
        {
            if (IsQueryForStudentPossible(courseName, userName))
            {
                OutputWriter.PtintStudent(new KeyValuePair<string, double>(userName, this.courses[courseName].StudentsByName[userName].MarksByCourseName[courseName]));
            }
        }

        public void GetStudentsByCourse(string courseName)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine($"{courseName}:");
                foreach (var studentMarksEntry in this.courses[courseName].StudentsByName)
                {
                    this.GetStudentMarkInCourse(courseName, studentMarksEntry.Key);
                }
            }
        }

        public void FilterAndTake(string courseName, string givenFilter, int? studentsToTake = null)
        {
            if (this.IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.courses[courseName].StudentsByName.Count;
                }
                Dictionary<string, double> marks = this.courses[courseName].StudentsByName
                   .ToDictionary(x => x.Key, x => x.Value.MarksByCourseName[courseName]);
                this.filter.FilterAndTake(marks, givenFilter, studentsToTake.Value);
            }
        }

        public void OrderAndTake(string courseName, string comparison, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.courses[courseName].StudentsByName.Count;
                }
                Dictionary<string, double> marks = this.courses[courseName].StudentsByName
                    .ToDictionary(x => x.Key, x => x.Value.MarksByCourseName[courseName]);
                this.sorter.OrderAndTake(marks, comparison, studentsToTake.Value);
            }
        }

        public ISimpleOrderedBag<ICourse> GetAllCoursesSorted(IComparer<ICourse> cmp)
        {
            SimpleSortedList<ICourse> sortedCourses = new SimpleSortedList<ICourse>(cmp);
            sortedCourses.AddAll(this.courses.Values);

            return sortedCourses;
        }

        public ISimpleOrderedBag<IStudent> GetAllStudentsSorted(IComparer<IStudent> cmp)
        {
            SimpleSortedList<IStudent> sortedStudents = new SimpleSortedList<IStudent>(cmp);
            sortedStudents.AddAll(this.students.Values);

            return sortedStudents;
        }
    }
}
