using System.Net.Http.Json;

// DTOs matching API responses
public class Vehicle
{
    public required string RegistrationNumber { get; set; }
    public required string Make { get; set; }
    public required string Model { get; set; }
    public int Year { get; set; }
}

public class InsuranceItem
{
    public string Type { get; set; }
    public decimal MonthlyCost { get; set; }
    public Vehicle Vehicle { get; set; }
}

public class InsuranceResponse
{
    public string PersonalNumber { get; set; }
    public List<InsuranceItem> Insurances { get; set; }
    public decimal TotalMonthlyCost { get; set; }
}

class Program
{
    static async Task Main(string[] args)
    {
        var testPersonalNumbers = new List<string>
        {
            // Kända i databasen
            "19900101-1234", // Har Car + Health (borde visa bilinfo)
            "19851212-5678", // Har Pet

            // Okända för test av NotFound
            "19700101-0000",
            "19991212-9999",
            "20010101-1111",
            "19880707-2222",
            "19950505-3333",
            "19770505-4444",
            "19660606-5555",
            "20021212-6666"
        };

        var client = new HttpClient();
        string baseUrl = "http://localhost:7240/api/Insurance";

        foreach (var personalNumber in testPersonalNumbers)
        {
            var url = $"{baseUrl}/{personalNumber}";
            Console.WriteLine($"🔍 Requesting insurance info for: {personalNumber}");

            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var insuranceResponse = await response.Content.ReadFromJsonAsync<InsuranceResponse>();

                    if (insuranceResponse != null)
                    {
                        Console.WriteLine($"✅ PersonalNumber: {insuranceResponse.PersonalNumber}");
                        Console.WriteLine($"   Total Monthly Cost: {insuranceResponse.TotalMonthlyCost} SEK");

                        if (insuranceResponse.Insurances != null)
                        {
                            foreach (var item in insuranceResponse.Insurances)
                            {
                                Console.WriteLine($"   - Type: {item.Type}, Cost: {item.MonthlyCost} SEK");

                                if (item.Vehicle != null)
                                {
                                    Console.WriteLine($"     🚗 Vehicle: {item.Vehicle.Make} {item.Vehicle.Model} ({item.Vehicle.RegistrationNumber}, {item.Vehicle.Year})");
                                }
                                else
                                {
                                    Console.WriteLine("     ⚠️ No vehicle information.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("⚠️ No insurance items returned.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("⚠️ InsuranceResponse deserialization returned null.");
                    }
                }
                else
                {
                    Console.WriteLine($"❌ Failed with status code: {response.StatusCode}");
                    var errorText = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"   Error message: {errorText}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 Exception: {ex.Message}");
            }

            Console.WriteLine(new string('-', 60));
        }

        // test Vehicles

        var testRegistrations = new List<string>
        {
            // ✅ Finns i databasen
            "ABC123",
            "xyz789",

            // ❌ Finns ej – testar 404
            "ZZZ000",
            "NOP321",
            "BAD456",
            "ABC999",
            "123XYZ",
            "HELLO1",
            "TEST01",
            "CARCAR"
        };

        client = new HttpClient();
        baseUrl = "http://localhost:7077/api/vehicle";

        foreach (var reg in testRegistrations)
        {
            var url = $"{baseUrl}/{reg}";
            Console.WriteLine($"🚗 Requesting vehicle info for: {reg}");

            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var vehicle = await response.Content.ReadFromJsonAsync<Vehicle>();
                    if (vehicle != null)
                    {
                        Console.WriteLine($"✅ Found Vehicle:");
                        Console.WriteLine($"   Reg: {vehicle.RegistrationNumber}");
                        Console.WriteLine($"   Make/Model: {vehicle.Make} {vehicle.Model}");
                        Console.WriteLine($"   Year: {vehicle.Year}");
                    }
                    else
                    {
                        Console.WriteLine("⚠️ Vehicle deserialization returned null.");
                    }
                }
                else
                {
                    Console.WriteLine($"❌ Not found. Status code: {response.StatusCode}");
                    var errorText = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"   Message: {errorText}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 Exception: {ex.Message}");
            }

            Console.WriteLine(new string('-', 60));
        }
    }
}
