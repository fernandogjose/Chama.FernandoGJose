using Chama.FernandoGJose.Domain.Share.Interfaces.MessageBus;
using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Domain.Course.Events
{
    public class CourseSignUpProcessedWithSuccessEventHandler : INotificationHandler<CourseSignUpProcessedWithSuccessEvent>
    {
        private readonly IServiceBus _serviceBus;

        public CourseSignUpProcessedWithSuccessEventHandler(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public Task Handle(CourseSignUpProcessedWithSuccessEvent notification, CancellationToken cancellationToken)
        {
            _serviceBus.PublishAsync("CourseSignUpProcessedWithSuccess", JsonConvert.SerializeObject(notification));
            return Task.CompletedTask;
        }
    }
}
