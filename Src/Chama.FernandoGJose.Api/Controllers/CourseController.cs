using Chama.FernandoGJose.Application.Course.Dtos;
using Chama.FernandoGJose.Application.Course.Interfaces;
using Chama.FernandoGJose.Application.Share.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Chama.FernandoGJose.Api.Controllers
{
    [ApiController, Route("api/course")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseAppService _courseAppService;

        public CourseController(ICourseAppService courseAppService)
        {
            _courseAppService = courseAppService;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CourseCreateDto request)
        {
            ResponseDto response = await _courseAppService.CreateAsync(request).ConfigureAwait(true);
            return Ok(response);
        }

        [HttpPost("sign-up")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SignUp([FromBody] CourseSignUpDto request)
        {
            ResponseDto response = await _courseAppService.SignUpPublishEventAsync(request).ConfigureAwait(true);
            return Ok(response);
        }

        [HttpGet("sign-up/report")]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseDto), (int)HttpStatusCode.InternalServerError)]
        public IActionResult SignUpReport()
        {
            ResponseDto response = _courseAppService.SignUpReport();
            return Ok(response);
        }
    }
}