using BashSoft.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BashSoft.Models
{
    public class Student
    {
        private string userName;
        private Dictionary<string, Course> enrolledCourses;
        private Dictionary<string, double> marksByCourseName;

        public string UserName
        {
            get
            {
                return this.userName;
            }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(this.userName), ExceptionMessages.NullOrEmptyValue);
                }
                this.userName = value;
            }
        }

        public IReadOnlyDictionary<string, Course> EnrolledCourses
        {
            get { return enrolledCourses; }
        }
        public IReadOnlyDictionary<string, double> MarksByCourseName
        {
            get { return marksByCourseName; }
        }
        public Student(string userName)
        {
            this.UserName = userName;
            this.enrolledCourses = new Dictionary<string, Course>();
            this.marksByCourseName = new Dictionary<string, double>();
        }

        public void EnrollInCourse(Course course)
        {
            if (this.enrolledCourses.ContainsKey(course.Name))
            {
                throw new DuplicateEntryInStructureException(this.UserName, course.Name);
                //OutputWriter.DisplayException(string.Format(ExceptionMessages.StudentAlreadyEnrolledInGivenCourse, 
                //    this.userName, course.Name));
                //return;
            }
            this.enrolledCourses.Add(course.Name, course);
        }

        public void SetMarksInCourse(string courseName, params int[] scores)
        {
            if (!this.enrolledCourses.ContainsKey(courseName))
            {
                throw new CourseNotFoundException();
                //OutputWriter.DisplayException(ExceptionMessages.NotEnrolledInCourse);
                //return;
            }

            if (scores.Length > Course.NumberOfTaskOnExam)
            {
                throw new InvalidNumberOfScoresException();
                //OutputWriter.DisplayException(ExceptionMessages.InvalidNumberOfScores);
                //return;
            }
            this.marksByCourseName.Add(courseName, CalculateMark(scores));
        }

        private double CalculateMark(int[] scores)
        {
            double percentageOfSolveExam = scores.Sum() / (double)(Course.NumberOfTaskOnExam * Course.MaxScoreOnExamTask);
            double mark = percentageOfSolveExam * 4 + 2;

            return mark;
        }
    }
}
