using Chama.FernandoGJose.Domain.Share.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Domain.Course.Commands
{
    public class CourseCreateCommandHandler : IRequestHandler<CourseCreateCommand, ResponseCommand>
    {
        public Task<ResponseCommand> Handle(CourseCreateCommand request, CancellationToken cancellationToken)
        {
            // TODO: Implementation

            // Response
            return Task.FromResult(new ResponseCommand(true, request));
        }
    }
}
