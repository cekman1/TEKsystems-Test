namespace TEKsystem_Test_MODELS.Model
{
    public class Person
    {
        public string? PersonalNumber { get; set; }
        public List<string> Insurances { get; set; }
        public string? VehicleRegistrationNumber { get; set; }

        public Person()
        {
            Insurances = new List<string>();
        }
    }
}
