using Chama.FernandoGJose.Domain.Share.Interfaces.MessageBus;
using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Domain.Course.Events
{
    public class CourseSignUpProcessedWithErrorEventHandler : INotificationHandler<CourseSignUpProcessedWithErrorEvent>
    {
        private readonly IServiceBus _serviceBus;

        public CourseSignUpProcessedWithErrorEventHandler(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public Task Handle(CourseSignUpProcessedWithErrorEvent notification, CancellationToken cancellationToken)
        {
            _serviceBus.PublishAsync("CourseSignUpProcessedWithError", JsonConvert.SerializeObject(notification));
            return Task.CompletedTask;
        }
    }
}
