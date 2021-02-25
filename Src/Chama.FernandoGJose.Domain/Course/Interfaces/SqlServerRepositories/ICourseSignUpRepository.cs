using Chama.FernandoGJose.Domain.Course.Commands;
using Chama.FernandoGJose.Domain.Course.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Domain.Course.Interfaces.SqlServerRepositories
{
    public interface ICourseSignUpRepository
    {
        Task CreateAsync(CourseSignUpCommand request);

        Task<CourseSignUpGetByCourseIdResponseQuery> GetByCourseIdAsync(string courseId);

        Task<IEnumerable<CourseSignUpGetReportByCourseIdResponseQuery>> GetReportByCourseIdAsync(string courseId);
    }
}
