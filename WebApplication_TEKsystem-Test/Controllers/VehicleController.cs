using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TEKsystem_Test_DATABASES.Vehicles;
using ThreadPilot_DataModels;

namespace WebApplication_TEKsystem_Test.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly FeatureToggles _featureToggles;

        public VehicleController(IOptions<FeatureToggles> featureToggles)
        {
            _featureToggles = featureToggles.Value;
        }

        [HttpGet("{registrationNumber}")]
        public IActionResult GetVehicleByRegistration(string registrationNumber)
        {
            if (_featureToggles.EnableFeatureVehiclesLookup)
            {
                // Kör ny funktionalitet
                var vehicle = FakeVehicleDatabase.GetByRegistrationNumber(registrationNumber);

                if (vehicle == null)
                    return NotFound("Vehicle not found.");

                return Ok(vehicle);
            }
            else
            {
                // Kör gammal funktionalitet
                return StatusCode(501, "Den här funktionen är inte tillgänglig ännu.");
            }
        }
    }

}
