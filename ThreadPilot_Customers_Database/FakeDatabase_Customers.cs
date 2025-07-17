using System.Collections.Generic;
using System.Linq;
using TEKsystem_Test_MODELS.Model;

namespace TEKsystem_Test_DATABASES.Customers
{
    // Mockad kunddatabas
    public static class FakeCustomerDatabase
    {
        private static readonly List<Person> _persons = new()
        {
            new Person
            {
                PersonalNumber = "19900101-1234",
                Insurances = new List<string> { "Health", "Car" },
                VehicleRegistrationNumber = "ABC123"
            },
            new Person
            {
                PersonalNumber = "19851212-5678",
                Insurances = new List<string> { "Pet" }
            }
        };

        /// <summary>
        /// Hämtar en person baserat på personnummer.
        /// </summary>
        /// <param name="personalNumber">Personnummer (format: ÅÅÅÅMMDD-XXXX)</param>
        /// <returns>En matchande Person eller null</returns>
        public static Person? GetByPersonalNumber(string personalNumber)
        {
            return _persons.FirstOrDefault(p =>
                string.Equals(p.PersonalNumber, personalNumber, System.StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Hämtar alla personer i databasen.
        /// </summary>
        public static IEnumerable<Person> GetAll() => _persons;
    }
}