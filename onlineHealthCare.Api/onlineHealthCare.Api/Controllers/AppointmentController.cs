using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using onlineHealthCare.Application.Dtos;
using onlineHealthCare.Application.Repositories;
using onlineHealthCare.Domain.Models;
using System.Security.Claims;
using static onlineHealthCare.Application.Dtos.PatientAppointmentDto;

namespace onlineHealthCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : Controller
    {
        private IAppoinmentService appoinmentService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AppointmentController> _logger;
        public AppointmentController(IAppoinmentService appoinmentService, ILogger<AppointmentController> logger,UserManager<ApplicationUser> userManager)
        {

            this.appoinmentService = appoinmentService;
            _logger = logger;
            this.userManager = userManager;
        }

        //[HttpPost("CreateAppointmentforDoctors")]
        //public async Task<IActionResult> CreateAppointment([FromBody]AppointmentDto appointmentDTO)
        //{
        //    try
        //    {
        //        _logger.LogInformation("Received request: {@AppointmentDTO}", appointmentDTO);
        //        if (ModelState.IsValid)
        //        {
        //            // Map DTO to required parameters
        //            var patientId = appointmentDTO.PatientId;
        //            var doctorId = appointmentDTO.DoctorId;
        //            var appointmentDate = appointmentDTO.AppointmentDateTime;
        //            var startTime = appointmentDTO.TimeStart;
        //            var endTime = appointmentDTO.TimeEnd; // Example: 30-min duration
        //            var description = appointmentDTO.Notes;
        //            // Call AddAsync with the correct parameters
        //            await appoinmentService.AddAsync(patientId, appointmentDate, startTime,endTime, doctorId, description);
        //            return Ok("Appointment created successfully.");
        //        }
        //        return BadRequest(ModelState);
        //        _logger.LogError("Model state is invalid: {@ModelState}", ModelState);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"An error occurred while creating the appointment: {ex.Message}");
        //    }
        //}

        [HttpPost("CreateAppointmentforDoctors")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentDto appointmentDTO)
        {
            _logger.LogInformation("Received request: {@AppointmentDTO}", appointmentDTO);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Model state is invalid: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                // Map DTO to parameters
                var patientId = appointmentDTO.PatientId;
                var doctorId = appointmentDTO.DoctorId;
                var appointmentDate = appointmentDTO.AppointmentDateTime;
                var startTime = appointmentDTO.TimeStart;
                var endTime = appointmentDTO.TimeEnd; // Default to 30 minutes if not provided
                var description = appointmentDTO.Notes;

                // Call AddAsync with the mapped parameters
                await appoinmentService.AddAsync(description, appointmentDate, startTime, endTime, doctorId, patientId);

                return Ok("Appointment created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the appointment.");
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("BookDoctor")]
        public async Task<IActionResult> BookDoctor(PatientAppointmentDto model)
        {
            try
            {

            if (!ModelState.IsValid)
            {
               return BadRequest();
            }
            var isAvailable =await appoinmentService.CheckAvailability(model.DoctorId, model.Date, model.TimeStart);
            if (!isAvailable)
            {
                return BadRequest("Doctor not available");
            }
            model.TimeEnd=model.TimeStart.AddMinutes(30);
                var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

                await appoinmentService.AddAsync(model.Info,
                    model.Date,
                    model.TimeStart,
                    model.TimeEnd,
                    model.DoctorId,
                    currentUser.ToString());
                
            return Ok(model.Id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
                   




            


                   


                


                

            



                   

