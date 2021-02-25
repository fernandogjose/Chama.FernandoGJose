using MediatR;

namespace Chama.FernandoGJose.Domain.Course.Events
{
    public class CourseSignUpProcessedWithErrorEvent : INotification
    {
        public CourseSignUpProcessedWithErrorEvent(string sagaId, string courseId, string email, string name, string errorMessage)
        {
            SagaId = sagaId;
            CourseId = courseId;
            ErrorMessage = errorMessage;
            Student = new StudentEvent(email, name);
        }

        public string SagaId { get; set; }

        public string CourseId { get; }

        public string ErrorMessage { get; set; }

        public StudentEvent Student { get; }

        public class StudentEvent
        {
            public StudentEvent(string email, string name)
            {
                Email = email;
                Name = name;
            }

            public string Email { get; }

            public string Name { get; }
        }
    }
}
