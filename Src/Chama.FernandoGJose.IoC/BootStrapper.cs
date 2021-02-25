using Chama.FernandoGJose.Application.Course.AppServices;
using Chama.FernandoGJose.Application.Course.Interfaces;
using Chama.FernandoGJose.Domain.Course.Commands;
using Chama.FernandoGJose.Domain.Course.Interfaces.SqlServerRepositories;
using Chama.FernandoGJose.Domain.Course.Validations;
using Chama.FernandoGJose.Domain.Share.Interfaces.Emails;
using Chama.FernandoGJose.Domain.Share.Interfaces.MessageBus;
using Chama.FernandoGJose.Domain.Share.Interfaces.Redis;
using Chama.FernandoGJose.Domain.Share.Interfaces.SqlServerRepositories;
using Chama.FernandoGJose.Domain.Share.Interfaces.Validations;
using Chama.FernandoGJose.Domain.Share.Pipelines;
using Chama.FernandoGJose.Email;
using Chama.FernandoGJose.MessageBus.Services;
using Chama.FernandoGJose.Redis;
using Chama.FernandoGJose.SqlServer.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Chama.FernandoGJose.IoC
{
    public static class BootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Message Bus
            services.AddTransient<IServiceBus, ServiceBus>();

            // Sql Server Repository
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDbConnection>(_ => new SqlConnection(Environment.GetEnvironmentVariable("CHAMA-FERNANDOGJOSE-SQL-CONNECTION")));
            services.AddTransient<ICourseSignUpRepository, CourseSignUpRepository>();

            // Redis
            services.AddSingleton<IRepositoryRedis, RepositoryRedis>();

            // Emails
            services.AddTransient<ISendEmail, CourseSignUpProcessedWithSuccessEmail>();
            services.AddTransient<ISendEmail, CourseSignUpProcessedWithErrorEmail>();

            // Command
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatePipeline<,>));
            services.AddMediatR(
                typeof(CourseCreateCommand).GetTypeInfo().Assembly,
                typeof(CourseSignUpCommand).GetTypeInfo().Assembly
            );

            // Validations
            services.AddTransient<IValidation, CourseSignUpCommandValidation>();

            // Application
            services.AddTransient<ICourseAppService, CourseAppService>();

        }
    }
}