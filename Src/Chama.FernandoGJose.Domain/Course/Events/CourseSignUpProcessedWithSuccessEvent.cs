using MediatR;

namespace Chama.FernandoGJose.Domain.Course.Events
{
    public class CourseSignUpProcessedWithSuccessEvent : INotification
    {
        public CourseSignUpProcessedWithSuccessEvent(string sagaId, string courseId, string email, string name)
        {
            SagaId = sagaId;
            CourseId = courseId;
            Student = new StudentEvent(email, name);
        }

        public string SagaId { get; set; }

        public string CourseId { get; }

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
