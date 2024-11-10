using Microsoft.AspNetCore.Mvc;
using onlineHealthCare.Application.Dtos;
using onlineHealthCare.Application.Repositories;

namespace onlineHealthCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        public IPatientService patientService { get; set; }
        public ILoginService _loginService { get; set; }
        public AuthenticationController(IPatientService _patientService
            , ILoginService loginService)
        {
            patientService = _patientService;
            _loginService = loginService;
        }
        [HttpPost("Registration")]
        public async Task<IActionResult> Registeration([FromBody] RegisterModel Data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await patientService.Register(Data);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           var result=await _loginService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
