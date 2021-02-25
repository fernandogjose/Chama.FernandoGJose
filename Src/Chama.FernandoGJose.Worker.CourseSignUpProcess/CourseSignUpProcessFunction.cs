using Chama.FernandoGJose.Application.Course.Dtos;
using Chama.FernandoGJose.Application.Course.Interfaces;
using Chama.FernandoGJose.IoC;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Chama.FernandoGJose.Worker.CourseSignUpProcess
{
    public static class CourseSignUpProcessFunction
    {
        private static ICourseAppService _courseAppService;

        [FunctionName("CourseSignUpProcessFunction")]
        public static void Run([ServiceBusTrigger("CourseSignUpCreated", Connection = "MyConnectionString")] string myQueueItem, ILogger log)
        {
            // TODO: Make circuitBreaker

            try
            {
                // Register services
                RegisterServices();

                // DeserializeObject
                var courseSignUpDto = JsonConvert.DeserializeObject<CourseSignUpDto>(myQueueItem);

                // Process
                _courseAppService.SignUpProcessOrderAsync(courseSignUpDto);
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
