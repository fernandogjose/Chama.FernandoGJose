using Bogus;
using Chama.FernandoGJose.Domain.Course.Commands;
using Chama.FernandoGJose.Domain.Course.Interfaces.SqlServerRepositories;
using Chama.FernandoGJose.Domain.Course.Queries;
using Chama.FernandoGJose.Domain.Share.Commands;
using Chama.FernandoGJose.Domain.Share.Interfaces.Redis;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Chama.FernandoGJose.UnitTest._3_Domain.Commands
{
    public class CourseSignUpCommandTest
    {
        private readonly Faker _faker;
        private readonly CourseSignUpCommandHandler _courseSignUpCommandHandler;
        private readonly Mock<ICourseSignUpRepository> _courseSignUpRepositoryMock;
        private readonly Mock<IRepositoryRedis> _repositoryRedisMock;

        public CourseSignUpCommandTest()
        {
            // Faker
            _faker = new Faker();

            // Mock
            _courseSignUpRepositoryMock = new Mock<ICourseSignUpRepository>();
            _repositoryRedisMock = new Mock<IRepositoryRedis>();

            // CommandHandler
            _courseSignUpCommandHandler = new CourseSignUpCommandHandler(_courseSignUpRepositoryMock.Object, _repositoryRedisMock.Object);
        }

        [Fact]
        public async Task Should_Create_CourseSignUp_With_Success()
        {
            // Config
            var sagaId = Guid.NewGuid().ToString();
            var courseId = Guid.NewGuid().ToString();
            var email = _faker.Person.Email;
            var name = _faker.Person.FirstName;
            var dateOfBirth = _faker.Person.DateOfBirth;
            CourseSignUpCommand courseSignUpCommand = new CourseSignUpCommand(sagaId, courseId, email, name, dateOfBirth);
            _courseSignUpRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<CourseSignUpCommand>()));
            _repositoryRedisMock.Setup(r => r.GetValueFromKey(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(new List<CourseSignUpReportResponseQuery>
            {
                new CourseSignUpReportResponseQuery
                {
                    CourseId = courseId,
                    AgeAverage = 20,
                    AgeMax = 30,
                    AgeMin = 10,
                }
            }));

            // Process
            ResponseCommand response = await _courseSignUpCommandHandler.Handle(courseSignUpCommand, CancellationToken.None).ConfigureAwait(true);

            // Assert
            Assert.True(response.Success);
            Assert.True(response.Object != null);
            Assert.True(response.Object is CourseSignUpCommand);
            if (response.Object is CourseSignUpCommand responseObject)
            {
                Assert.True(responseObject.SagaId == sagaId);
                Assert.True(responseObject.CourseId == courseId);
                Assert.True(responseObject.Student.Email == email);
                Assert.True(responseObject.Student.Name == name);
                Assert.True(responseObject.Student.DateOfBirth == dateOfBirth);
            }
        }

        // TODO: Add another tests
    }
}
