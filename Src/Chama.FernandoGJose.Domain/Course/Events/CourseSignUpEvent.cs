using MediatR;
using System;

namespace Chama.FernandoGJose.Domain.Course.Events
{
    public class CourseSignUpEvent : INotification
    {
        public CourseSignUpEvent(string courseId, string email, string name, DateTime dateOfBirth)
        {
            SagaId = Guid.NewGuid().ToString();
            CourseId = courseId;
            Student = new StudentEvent(email, name, dateOfBirth);
        }

        public string SagaId { get; set; }

        public string CourseId { get; }

        public StudentEvent Student { get; }

        public class StudentEvent
        {
            public StudentEvent(string email, string name, DateTime dateOfBirth)
            {
                Email = email;
                Name = name;
                DateOfBirth = dateOfBirth;
            }

            public string Email { get; }

            public string Name { get; }

            public DateTime DateOfBirth { get; }
        }
    }
}
