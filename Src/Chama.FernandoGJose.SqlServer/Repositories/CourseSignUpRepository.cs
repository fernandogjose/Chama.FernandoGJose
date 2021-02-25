using Chama.FernandoGJose.Domain.Course.Commands;
using Chama.FernandoGJose.Domain.Course.Interfaces.SqlServerRepositories;
using Chama.FernandoGJose.Domain.Course.Queries;
using Chama.FernandoGJose.Domain.Share.Interfaces.SqlServerRepositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.SqlServer.Repositories
{
    public class CourseSignUpRepository : BaseSqlServerRepository, ICourseSignUpRepository
    {
        public CourseSignUpRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task CreateAsync(CourseSignUpCommand request)
        {
            const string sql = "" +
                            " INSERT INTO " +
                            " CourseSignUp (CourseId, Email) " +
                            "       VALUES (@CourseId, @Email) ";

            await _unitOfWork.Connection.ExecuteAsync(sql, new { request.CourseId, request.Student.Email }, _unitOfWork?.Transaction).ConfigureAwait(false);
        }

        public async Task<CourseSignUpGetByCourseIdResponseQuery> GetByCourseIdAsync(string courseId)
        {
            const string sql = "" +
                            " SELECT Course.Id as CourseId" +
                            "       ,Course.CapacityOfStudents " +
                            "       ,CourseSignUp.Email" +
                            " FROM CourseSignUp" +
                            " INNER JOIN Course ON CourseSignUp.CourseId = Course.Id" +
                            " WHERE Course.Id = @courseId";

            // I used DataReader to get more performance
            var dataReader = await _unitOfWork.Connection.ExecuteReaderAsync(sql, new { courseId }, _unitOfWork?.Transaction).ConfigureAwait(true);

            // Make object
            var capacityOfStudents = 0;
            var students = new List<string>(0);
            while (dataReader.Read())
            {
                capacityOfStudents = Convert.ToInt32(dataReader["CapacityOfStudents"].ToString());
                students.Add(dataReader["Email"].ToString());
            }

            // Return
            return new CourseSignUpGetByCourseIdResponseQuery(courseId, capacityOfStudents, students);
        }

        public async Task<IEnumerable<CourseSignUpGetReportByCourseIdResponseQuery>> GetReportByCourseIdAsync(string courseId)
        {
            const string sql = "" +
                            " SELECT Course.Name as CourseName " +
                            "       ,Student.DateOfBirth as StudentDateOfBirth" +
                            " FROM CourseSignUp" +
                            " INNER JOIN Course ON CourseSignUp.CourseId = Course.Id" +
                            " INNER JOIN Student ON CourseSignUp.StudentId = Student.Id" +
                            " WHERE Course.Id = @courseId" +
                            " ORDER BY Student.DateOfBirth";

            // I used DataReader to get more performance
            return await _unitOfWork.Connection.QueryAsync<CourseSignUpGetReportByCourseIdResponseQuery>(sql, new { courseId }, _unitOfWork?.Transaction).ConfigureAwait(true);
        }
    }
}
