using Microsoft.AspNetCore.Mvc;
using TEKsystem_Test_DATABASES.Customers;
using TEKsystem_Test_MODELS.Model;
using TEKsystem_Test_COSTS.CostManagementBackend;


namespace WebApplication_TEKsystem_Test_B.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsuranceController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public InsuranceController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{personalNumber}")]
        public async Task<IActionResult> GetInsurances(string personalNumber)
        {
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
                    var vehicleApiUrl = $"https://localhost:7077/api/Vehicle/{person.VehicleRegistrationNumber}";

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

        //private decimal GetMonthlyCost(string insuranceType) => insuranceType switch
        //{
        //    "Pet" => 10,
        //    "Health" => 20,
        //    "Car" => 30,
        //    _ => 0
        //};
    }
}
