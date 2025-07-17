using System.Collections.Generic;
using System.Linq;
using TEKsystem_Test_MODELS.Model;

namespace TEKsystem_Test_DATABASES.Vehicles
{
    /// <summary>
    /// Mockad fordonsdatabas med testdata.
    /// </summary>
    public static class FakeVehicleDatabase
    {
        // Fordon uppslagsbara på registreringsnummer
        private static readonly Dictionary<string, Vehicle> _vehicles = new(StringComparer.OrdinalIgnoreCase)
        {
            ["ABC123"] = new Vehicle { RegistrationNumber = "ABC123", Make = "Volvo", Model = "XC60", Year = 2020 },
            ["XYZ789"] = new Vehicle { RegistrationNumber = "XYZ789", Make = "Toyota", Model = "Corolla", Year = 2018 }
        };

        /// <summary>
        /// Hämtar ett fordon baserat på registreringsnummer.
        /// </summary>
        /// <param name="registrationNumber">Fordonets registreringsnummer</param>
        /// <returns>Matchande <see cref="Vehicle"/> eller null</returns>
        public static Vehicle? GetByRegistrationNumber(string registrationNumber)
        {
            if (string.IsNullOrWhiteSpace(registrationNumber))
                return null;

            return _vehicles.TryGetValue(registrationNumber, out var vehicle) ? vehicle : null;
        }

        /// <summary>
        /// Returnerar alla fordon.
        /// </summary>
        public static IEnumerable<Vehicle> GetAll() => _vehicles.Values;
    }
}
