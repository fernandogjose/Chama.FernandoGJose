using System.Collections.Generic;

namespace Chama.FernandoGJose.Domain.Course.Queries
{
    public class CourseSignUpGetByCourseIdResponseQuery
    {
        public string CourseId { get; }

        public int CapacityOfStudents { get; }

        public List<string> Students { get; set; }

        public CourseSignUpGetByCourseIdResponseQuery(string courseId, int capacityOfStudents, List<string> students)
        {
            CourseId = courseId;
            CapacityOfStudents = capacityOfStudents;
            Students = students;
        }
    }
}
