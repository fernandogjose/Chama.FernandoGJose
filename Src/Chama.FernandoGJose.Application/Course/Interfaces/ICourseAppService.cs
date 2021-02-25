using Chama.FernandoGJose.Application.Course.Dtos;
using Chama.FernandoGJose.Application.Share.Dtos;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Application.Course.Interfaces
{
    public interface ICourseAppService
    {
        Task<ResponseDto> SignUpPublishEventAsync(CourseSignUpDto request);

        Task<ResponseDto> SignUpProcessOrderAsync(CourseSignUpDto request);

        Task<ResponseDto> CreateAsync(CourseCreateDto request);

        Task SignUpProcessedWithSuccess(CourseSignUpProcessedWithSuccessDto request);

        Task SignUpProcessedWithError(CourseSignUpProcessedWithErrorDto request);

        ResponseDto SignUpReport();
    }
}
