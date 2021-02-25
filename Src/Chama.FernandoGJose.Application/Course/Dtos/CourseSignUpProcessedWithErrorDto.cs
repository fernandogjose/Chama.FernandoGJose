namespace Chama.FernandoGJose.Application.Course.Dtos
{
    public class CourseSignUpProcessedWithErrorDto
    {
        public string SagaId { get; set; }

        public string CourseId { get; set; }

        public string ErrorMessage { get; set; }

        public StudentEvent Student { get; set; }

        public class StudentEvent
        {
            public string Email { get; set; }

            public string Name { get; set; }
        }
    }
}