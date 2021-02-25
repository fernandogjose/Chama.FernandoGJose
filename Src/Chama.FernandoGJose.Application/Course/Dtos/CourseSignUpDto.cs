using System;

namespace Chama.FernandoGJose.Application.Course.Dtos
{
    public class CourseSignUpDto
    {
        public string SagaId { get; set; }

        public string CourseId { get; set; }

        public StudentDto Student { get; set; }

        public class StudentDto
        {
            public string Email { get; set; }

            public string Name { get; set; }
            
            public DateTime DateOfBirth { get; set; }
        }
    }
}
