using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TEKsystem_Test_COSTS.CostManagementBackend;
using TEKsystem_Test_DATABASES.Customers;
using TEKsystem_Test_MODELS.Model;
using ThreadPilot_DataModels;


namespace WebApplication_TEKsystem_Test_B.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsuranceController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FeatureToggles _featureToggles;

        public InsuranceController(IHttpClientFactory httpClientFactory, IOptions<FeatureToggles> featureToggles)
        {
            _httpClientFactory = httpClientFactory;
            _featureToggles = featureToggles.Value;
        }

        [HttpGet("{personalNumber}")]
        public async Task<IActionResult> GetInsurances(string personalNumber)
        {

            if (_featureToggles.EnableFeaturePersonsLookup)
            {
                // Kör ny funktionalitet
                var person = FakeCustomerDatabase.GetByPersonalNumber(personalNumber);

                if (person == null)
                    return NotFound("Person not found.");

                var response = new InsuranceResponse
                {
                    PersonalNumber = personalNumber,
                    Insurances = new List<InsuranceItem>(),
                    TotalMonthlyCost = 0
                };

                foreach (var insurance in person.Insurances)
                {
                    var item = new InsuranceItem
                    {
                        Type = insurance,
                        MonthlyCost = CostManagement.GetMonthlyCost(insurance)
                    };

                    if (insurance == "Car")
                    {
                        var client = _httpClientFactory.CreateClient();
                        var vehicleApiUrl = $"http://localhost:7077/api/Vehicle/{person.VehicleRegistrationNumber}";

                        try
                        {
                            var vehicle = await client.GetFromJsonAsync<Vehicle>(vehicleApiUrl);
                            item.Vehicle = vehicle;
                        }
                        catch
                        {
                            item.Vehicle = null;
                        }
                    }

                    response.Insurances.Add(item);
                    response.TotalMonthlyCost += item.MonthlyCost;
                }

                return Ok(response);
            }
            else
            {
                // Kör gammal funktionalitet
                return StatusCode(501, "Den här funktionen är inte tillgänglig ännu.");
            }
        }
    }
}
