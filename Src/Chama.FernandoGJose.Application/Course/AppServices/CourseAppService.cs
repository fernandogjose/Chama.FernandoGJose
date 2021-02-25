using Chama.FernandoGJose.Application.Course.Dtos;
using Chama.FernandoGJose.Application.Course.Interfaces;
using Chama.FernandoGJose.Application.Share.Dtos;
using Chama.FernandoGJose.Domain.Course.Commands;
using Chama.FernandoGJose.Domain.Course.Events;
using Chama.FernandoGJose.Domain.Course.Queries;
using Chama.FernandoGJose.Domain.Share.Commands;
using Chama.FernandoGJose.Domain.Share.Interfaces.Emails;
using Chama.FernandoGJose.Domain.Share.Interfaces.Redis;
using Chama.FernandoGJose.Domain.Share.Interfaces.SqlServerRepositories;
using Chama.FernandoGJose.Util.Exceptions;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Application.Course.AppServices
{
    public class CourseAppService : ICourseAppService
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnumerable<ISendEmail> _sendEmails;
        private readonly IRepositoryRedis _repositoryRedis;

        public CourseAppService(IMediator mediator, IUnitOfWork unitOfWork, IEnumerable<ISendEmail> sendEmails, IRepositoryRedis repositoryRedis)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _sendEmails = sendEmails;
            _repositoryRedis = repositoryRedis;
        }

        public async Task<ResponseDto> CreateAsync(CourseCreateDto request)
        {
            using (_unitOfWork)
            {
                // Init transaction
                _unitOfWork.BeginTransaction();

                // Do something

                // Add course
                CourseCreateCommand courseCreateCommand = new CourseCreateCommand(request.Name, request.Capacity);
                ResponseCommand courseCreateCommandResponse = await _mediator.Send(courseCreateCommand, CancellationToken.None).ConfigureAwait(true);

                // Commit
                _unitOfWork.CommitTransaction();
                return new ResponseDto { Success = true, Object = courseCreateCommandResponse.Object };
            }
        }

        public async Task<ResponseDto> SignUpPublishEventAsync(CourseSignUpDto request)
        {
            // Create Event and publish message to process by worker
            var courseSignUpEvent = new CourseSignUpEvent(request.CourseId, request.Student.Email, request.Student.Name, request.Student.DateOfBirth);
            await _mediator.Publish(courseSignUpEvent).ConfigureAwait(true);

            // Return
            return new ResponseDto { Success = true, Object = courseSignUpEvent };
        }

        public async Task<ResponseDto> SignUpProcessOrderAsync(CourseSignUpDto request)
        {
            var response = new ResponseDto();

            try
            {
                using (_unitOfWork)
                {
                    // Init transaction
                    _unitOfWork.BeginTransaction();

                    // Create and process command
                    var courseSignUpCommand = new CourseSignUpCommand(request.SagaId, request.CourseId, request.Student.Email, request.Student.Name, request.Student.DateOfBirth);
                    var responseCommand = await _mediator.Send(courseSignUpCommand, CancellationToken.None).ConfigureAwait(true);

                    // Publish event to notify student
                    var courseSignUpProcessedWithSuccessEvent = new CourseSignUpProcessedWithSuccessEvent(request.SagaId, request.CourseId, request.Student.Email, request.Student.Name);
                    await _mediator.Publish(courseSignUpProcessedWithSuccessEvent).ConfigureAwait(true);

                    // Commit and Return
                    _unitOfWork.CommitTransaction();
                    response.Success = true;
                    response.Object = responseCommand.Object;
                }
            }
            catch (CommandObjectException commandObjectException)
            {
                // Publish event to notify student
                var courseSignUpProcessedWithErrorEvent = new CourseSignUpProcessedWithErrorEvent(request.SagaId, request.CourseId, request.Student.Email, request.Student.Name, commandObjectException.Message);
                await _mediator.Publish(courseSignUpProcessedWithErrorEvent).ConfigureAwait(true);

                // TODO: Log Error

                // Return
                response.Success = false;
                response.Object = commandObjectException.Message;
            }
            catch (CapacityOfStudentsByCourseException capacityOfStudentsByCourseException)
            {
                // Publish event to notify student
                var courseSignUpProcessedWithErrorEvent = new CourseSignUpProcessedWithErrorEvent(request.SagaId, request.CourseId, request.Student.Email, request.Student.Name, capacityOfStudentsByCourseException.Message);
                await _mediator.Publish(courseSignUpProcessedWithErrorEvent).ConfigureAwait(true);

                // TODO: Log Error

                // Return
                response.Success = false;
                response.Object = capacityOfStudentsByCourseException.Message;
            }
            catch (CommandParameterException commandParameterException)
            {
                // Publish event to notify student
                var courseSignUpProcessedWithErrorEvent = new CourseSignUpProcessedWithErrorEvent(request.SagaId, request.CourseId, request.Student.Email, request.Student.Name, commandParameterException.Message);
                await _mediator.Publish(courseSignUpProcessedWithErrorEvent).ConfigureAwait(true);

                // TODO: Log Error

                // Return
                response.Success = false;
                response.Object = commandParameterException.Message;
            }
            catch (Exception exception)
            {
                // TODO: Log Error

                // TODO: Create a retry policy with POLLY

                response.Success = false;
                response.Object = exception.Message;
            }

            return response;
        }

        public async Task SignUpProcessedWithSuccess(CourseSignUpProcessedWithSuccessDto request)
        {
            // Get implementation to e-mail
            var sendEmail = _sendEmails.FirstOrDefault(se => se.TypeEmail == "CourseSignUpProcessedWithSuccess");
            if (sendEmail == null) return;

            // Create parameters
            var parameters = new Dictionary<string, string>(0)
            {
                { "CourseId", request.CourseId }
            };

            // Send e-mail to notify student
            await sendEmail.Send("CourseSignUpProcessedWithSuccess", request.Student.Email, request.Student.Name, parameters).ConfigureAwait(false);
        }

        public async Task SignUpProcessedWithError(CourseSignUpProcessedWithErrorDto request)
        {
            // Get implementation to e-mail
            var sendEmail = _sendEmails.FirstOrDefault(se => se.TypeEmail == "CourseSignUpProcessedWithError");
            if (sendEmail == null) return;

            // Create parameters
            var parameters = new Dictionary<string, string>(0)
            {
                { "CourseId", request.CourseId },
                { "ErrorMessage", request.ErrorMessage }
            };

            // Send e-mail to notify student
            await sendEmail.Send("CourseSignUpProcessedWithError", request.Student.Email, request.Student.Name, parameters).ConfigureAwait(false);
        }

        public ResponseDto SignUpReport()
        {
            // Get in Redis cache
            var responseString = _repositoryRedis.GetValueFromKey("CourseSignUpReport");
            var coursesSignUpReportResponseQuery = JsonConvert.DeserializeObject<List<CourseSignUpReportResponseQuery>>(responseString);
            return new ResponseDto
            {
                Success = true,
                Object = coursesSignUpReportResponseQuery
            };
        }
    }
}
