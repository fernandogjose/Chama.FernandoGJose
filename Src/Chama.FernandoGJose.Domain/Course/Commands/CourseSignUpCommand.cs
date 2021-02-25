using Chama.FernandoGJose.Domain.Share.Commands;
using MediatR;
using System;

namespace Chama.FernandoGJose.Domain.Course.Commands
{
    public class CourseSignUpCommand : RequestCommand, IRequest<ResponseCommand>
    {
        public CourseSignUpCommand(string sagaId, string courseId, string email, string name, DateTime dateOfBirth)
        {
            SagaId = sagaId;
            CourseId = courseId;
            Student = new StudentCommand(email, name, dateOfBirth);
        }

        public string SagaId { get; set; }

        public string CourseId { get; }

        public StudentCommand Student { get; }

        public class StudentCommand
        {
            public StudentCommand(string email, string name, DateTime dateOfBirth)
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
