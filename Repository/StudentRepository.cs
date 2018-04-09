
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

    public class StudentRepository
    {
        private bool isDataInitialzed = false;
        //  private Dictionary<string, Dictionary<string, List<int>>> studentByCourse;
        private Dictionary<string, Course> courses;
        private Dictionary<string, Student> students;
        private RepositoryFilter filter;
        private RepositorySorter sorter;

        public StudentRepository(RepositorySorter sorter, RepositoryFilter filter)
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
            this.students = new Dictionary<string, Student>();
            this.courses = new Dictionary<string, Course>();
            ReadData(fileName);
        }

        public void UnloadData()
        {
            if (!this.isDataInitialzed)
            {
               // OutputWriter.DisplayException(ExceptionMessages.DataNotInitializedExceptionMessage);
               throw new ArgumentException(ExceptionMessages.DataNotInitializedExceptionMessage);
            }
            //  this.studentByCourse = new Dictionary<string, Dictionary<string, List<int>>>();
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
                //throw new FileNotFoundException(ExceptionMessages.InvalidPath);
                //OutputWriter.DisplayException(ExceptionMessages.InvalidPath);
                //return;
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
                    //string[] tokens = allInputLines[line].Split(' ');
                    // string courseName = tokens[0];
                    //string student = tokens[1];
                    //int mark = int.Parse(tokens[2]);
                    string courseName = currentMatch.Groups[1].Value;
                    string username = currentMatch.Groups[2].Value;
                    //int studentScoreOnTask;
                    //bool hasParsedScore = int.TryParse(currentMatch.Groups[3].Value, out studentScoreOnTask);
                    //if (hasParsedScore && studentScoreOnTask >= 0 && studentScoreOnTask <= 100)
                    //{
                    //    if (!studentByCourse.ContainsKey(courseName))
                    //    {
                    //        studentByCourse.Add(courseName, new Dictionary<string, List<int>>());
                    //    }
                    //    if (!studentByCourse[courseName].ContainsKey(username))
                    //    {
                    //        studentByCourse[courseName].Add(username, new List<int>());
                    //    }
                    //    studentByCourse[courseName][username].Add(studentScoreOnTask);
                    //}
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
                        Course course = this.courses[courseName];
                        Student student = this.students[username];

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
                 //   OutputWriter.DisplayException(ExceptionMessages.InexistingCourseInDataBase);
                }
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.DataNotInitializedExceptionMessage);
                // OutputWriter.DisplayException(ExceptionMessages.DataNotInitializedExceptionMessage);
            }
          //  return false;
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
               // OutputWriter.DisplayException(ExceptionMessages.InexistingStudentInDataBase);
            }
           // return false;
        }

        public void GetStudentScoresFromCourse(string courseName, string userName)
        {
            if (IsQueryForStudentPossible(courseName, userName))
            {
                OutputWriter.PtintStudent(new KeyValuePair<string, double>(userName, this.courses[courseName].StudentsByName[userName].MarksByCourseName[courseName]));
            }
        }

        public void GetAllStudentsFromCourse(string courseName)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine($"{courseName}:");
                foreach (var studentMarksEntry in this.courses[courseName].StudentsByName)
                {
                    //  OutputWriter.PtintStudent(studentMarksEntry);
                    this.GetStudentScoresFromCourse(courseName, studentMarksEntry.Key);
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
    }
}
