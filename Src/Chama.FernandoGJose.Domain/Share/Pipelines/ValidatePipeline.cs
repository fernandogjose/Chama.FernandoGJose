using Chama.FernandoGJose.Domain.Share.Commands;
using Chama.FernandoGJose.Domain.Share.Interfaces.Validations;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Domain.Share.Pipelines
{
    public class ValidatePipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse : ResponseCommand
    {
        private readonly IEnumerable<IValidation> _validations;

        public ValidatePipeline(IEnumerable<IValidation> validations)
        {
            _validations = validations;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is RequestCommand requestCommand)
            {
                var validation = _validations.FirstOrDefault(x => x.Command == request.GetType().Name);
                validation?.ValidateAsync(requestCommand);
            }

            return await next().ConfigureAwait(true);
        }
    }
}