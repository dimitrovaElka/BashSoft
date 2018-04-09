using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class CourseNotFoundException : Exception
    {
        public const string NotEnrolled = "Student must be enrolled in a course before you set his mark.";
        public CourseNotFoundException() : base(NotEnrolled)
        {
        }
        public CourseNotFoundException(string message) : base(message)
        {
        }
    }
}
