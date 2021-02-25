using Chama.FernandoGJose.Domain.Course.Interfaces.SqlServerRepositories;
using Chama.FernandoGJose.Domain.Course.Queries;
using Chama.FernandoGJose.Domain.Share.Commands;
using Chama.FernandoGJose.Domain.Share.Interfaces.Redis;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Domain.Course.Commands
{
    public class CourseSignUpCommandHandler : IRequestHandler<CourseSignUpCommand, ResponseCommand>
    {
        private readonly ICourseSignUpRepository _courseSignUpRepository;
        private readonly IRepositoryRedis _repositoryRedis;
        private readonly string _courseSignUpReportKey;

        public CourseSignUpCommandHandler(ICourseSignUpRepository courseSignUpRepository, IRepositoryRedis repositoryRedis)
        {
            _courseSignUpRepository = courseSignUpRepository;
            _repositoryRedis = repositoryRedis;
            _courseSignUpReportKey = "CourseSignUpReport";
        }

        public async Task<ResponseCommand> Handle(CourseSignUpCommand request, CancellationToken cancellationToken)
        {
            // Create
            await _courseSignUpRepository.CreateAsync(request).ConfigureAwait(false);

            // Update Cache (I thinked to do that by event but the time is short)
            await UpdateReportCacheAsync(request).ConfigureAwait(false);

            // Response
            return new ResponseCommand(true, request);
        }

        private async Task UpdateReportCacheAsync(CourseSignUpCommand request)
        {
            // Get by Redis cache
            var responseString = _repositoryRedis.GetValueFromKey(_courseSignUpReportKey);
            var coursesSignUpReportResponseQuery = JsonConvert.DeserializeObject<List<CourseSignUpReportResponseQuery>>(responseString);

            // Get current course
            var courseSignUpReportResponseQuery = coursesSignUpReportResponseQuery?.FirstOrDefault(x => x.CourseId == request.CourseId);

            // Get current age
            var currentAge = CalculateAgeByDateOfBirth(request.Student.DateOfBirth);

            // if exist update report in cache
            if (courseSignUpReportResponseQuery != null)
            {
                if (currentAge > courseSignUpReportResponseQuery.AgeMax) courseSignUpReportResponseQuery.AgeMax = currentAge;
                if (currentAge < courseSignUpReportResponseQuery.AgeMin) courseSignUpReportResponseQuery.AgeMin = currentAge;
                courseSignUpReportResponseQuery.AgeAverage = (currentAge + courseSignUpReportResponseQuery.AgeAverage) / 2;
                _repositoryRedis.SetValueFromKey(_courseSignUpReportKey, JsonConvert.SerializeObject(coursesSignUpReportResponseQuery));
                return;
            }

            // if not exist get in db
            var coursesSignUpGetReportByCourseIdResponseQuery = await _courseSignUpRepository.GetReportByCourseIdAsync(request.CourseId).ConfigureAwait(true);
            if (coursesSignUpGetReportByCourseIdResponseQuery?.ToList().Count > 0)
            {
                (coursesSignUpReportResponseQuery ??= new List<CourseSignUpReportResponseQuery>(0)).Add(new CourseSignUpReportResponseQuery
                {
                    CourseId = request.CourseId,
                    AgeMin = currentAge,
                    AgeMax = currentAge,
                    AgeAverage = currentAge
                });

                // Update or add in redis cache
                _repositoryRedis.SetValueFromKey(_courseSignUpReportKey, JsonConvert.SerializeObject(coursesSignUpReportResponseQuery));
            }
        }

        // TODO: Refactor, change this method for helpers ou somthing like that
        private int CalculateAgeByDateOfBirth(DateTime dateOfBirth)
        {
            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - dateOfBirth.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (dateOfBirth.Date > today.AddYears(-age)) age--;

            return age;
        }
    }
}
