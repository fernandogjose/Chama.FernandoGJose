using Chama.FernandoGJose.Application.Course.Dtos;
using Chama.FernandoGJose.Application.Course.Interfaces;
using Chama.FernandoGJose.IoC;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Chama.FernandoGJose.Worker.CourseSignUpProcessedWithSuccess
{
    public static class CourseSignUpProcessedWithSuccessFunction
    {
        private static ICourseAppService _courseAppService;

        [FunctionName("CourseSignUpProcessedWithSuccessFunction")]
        public static void Run([ServiceBusTrigger("CourseSignUpProcessedWithSuccess", Connection = "MyConnectionString")]string myQueueItem, ILogger log)
        {
            // TODO: Make circuitBreaker

            try
            {
                // Register services
                RegisterServices();

                // DeserializeObject
                var courseSignUpProcessedWithSuccessDto = JsonConvert.DeserializeObject<CourseSignUpProcessedWithSuccessDto>(myQueueItem);

                // Process
                _courseAppService.SignUpProcessedWithSuccess(courseSignUpProcessedWithSuccessDto);
            }
            catch (Exception exception)
            {
                // TODO: Log Error

                // TODO: Create a retry policy with POLLY

                // TODO: Add another exceptions and improve get exceptions
            }
        }

        private static void RegisterServices()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            BootStrapper.RegisterServices(serviceCollection);

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            _courseAppService = serviceProvider.GetService<ICourseAppService>();
        }
    }
}
