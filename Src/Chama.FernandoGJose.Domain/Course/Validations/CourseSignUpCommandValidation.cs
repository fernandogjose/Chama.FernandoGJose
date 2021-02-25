using Chama.FernandoGJose.Domain.Course.Commands;
using Chama.FernandoGJose.Domain.Course.Interfaces.SqlServerRepositories;
using Chama.FernandoGJose.Domain.Share.Commands;
using Chama.FernandoGJose.Domain.Share.Interfaces.Validations;
using Chama.FernandoGJose.Util.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Domain.Course.Validations
{
    public class CourseSignUpCommandValidation : IValidation
    {
        private readonly ICourseSignUpRepository _courseSignUpRepository;

        public CourseSignUpCommandValidation(ICourseSignUpRepository courseSignUpRepository)
        {
            _courseSignUpRepository = courseSignUpRepository;
        }

        public string Command => "CourseSignUpCommand";

        public async Task ValidateAsync<T>(T requestCommand) where T : RequestCommand
        {
            CommandObjectValidate(requestCommand);
            RequiredFieldValidate(requestCommand as CourseSignUpCommand);
            await CapacityOfStudentsByCourseValidateAsync(requestCommand as CourseSignUpCommand).ConfigureAwait(true);
        }

        private void CommandObjectValidate<T>(T requestCommand) where T : RequestCommand
        {
            if (!(requestCommand is CourseSignUpCommand))
            {
                throw new CommandObjectException(Command);
            }
        }

        private void RequiredFieldValidate(CourseSignUpCommand requestCommand)
        {
            var errors = new List<string>(0);

            if (string.IsNullOrEmpty(requestCommand.CourseId))
            {
                errors.Add("CourseId is required");
            }

            if (requestCommand.Student == null)
            {
                errors.Add("Student is required");
            }

            if (string.IsNullOrEmpty(requestCommand.Student.Email))
            {
                errors.Add("E-mail is required");
            }

            // TODO: Make one e-mail validator here

            if (string.IsNullOrEmpty(requestCommand.Student.Name))
            {
                errors.Add("Name is required");
            }

            if (requestCommand.Student.DateOfBirth == DateTime.MinValue)
            {
                errors.Add("DateOfBirth is required");
            }

            if (errors.Count > 0)
            {
                throw new CommandParameterException(JsonConvert.SerializeObject(errors));
            }
        }

        private async Task CapacityOfStudentsByCourseValidateAsync(CourseSignUpCommand requestCommand)
        {
            var courseSignUpGetByCourseIdResponseQuery = await _courseSignUpRepository.GetByCourseIdAsync(requestCommand.CourseId).ConfigureAwait(true);
            if (courseSignUpGetByCourseIdResponseQuery.Students.Any(s => s == requestCommand.Student.Email))
            {
                throw new CapacityOfStudentsByCourseException($"Student {requestCommand.Student.Email} already registered by course {requestCommand.CourseId}");
            }

            if (courseSignUpGetByCourseIdResponseQuery.Students.Count >= courseSignUpGetByCourseIdResponseQuery.CapacityOfStudents)
            {
                throw new CapacityOfStudentsByCourseException($"Course capacity has run out. CourseId: {requestCommand.CourseId}");
            }
        }
    }
}
