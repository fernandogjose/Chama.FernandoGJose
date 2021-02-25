using Chama.FernandoGJose.Domain.Share.Commands;
using MediatR;
using System;

namespace Chama.FernandoGJose.Domain.Course.Commands
{
    public class CourseCreateCommand : RequestCommand, IRequest<ResponseCommand>
    {
        public string Id { get; }

        public string Name { get; }

        public int CapacityOfStudents { get; }

        public CourseCreateCommand(string name, int capacityOfStudents)
        {
            Id = Guid.NewGuid().ToString();
            CapacityOfStudents = capacityOfStudents;
            Name = name;
        }
    }
}
