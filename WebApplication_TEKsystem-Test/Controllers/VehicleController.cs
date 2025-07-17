using Microsoft.AspNetCore.Mvc;
using TEKsystem_Test_DATABASES.Vehicles;

namespace WebApplication_TEKsystem_Test.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        [HttpGet("{registrationNumber}")]
        public IActionResult GetVehicleByRegistration(string registrationNumber)
        {

            var vehicle = FakeVehicleDatabase.GetByRegistrationNumber(registrationNumber);

            if (vehicle == null)
                return NotFound("Vehicle not found.");

            return Ok(vehicle);
        }
    }

}
