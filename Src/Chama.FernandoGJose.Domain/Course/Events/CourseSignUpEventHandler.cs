using Chama.FernandoGJose.Domain.Share.Interfaces.MessageBus;
using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Domain.Course.Events
{
    public class CourseSignUpEventHandler : INotificationHandler<CourseSignUpEvent>
    {
        private readonly IServiceBus _serviceBus;

        public CourseSignUpEventHandler(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public Task Handle(CourseSignUpEvent notification, CancellationToken cancellationToken)
        {
            _serviceBus.PublishAsync("CourseSignUpCreated", JsonConvert.SerializeObject(notification));
            return Task.CompletedTask;
        }
    }
}
