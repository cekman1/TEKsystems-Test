namespace TEKsystem_Test_MODELS.Model
{
    public class InsuranceResponse
    {
        public string? PersonalNumber { get; set; }
        public List<InsuranceItem> Insurances { get; set; }
        public decimal? TotalMonthlyCost { get; set; }

        public InsuranceResponse()
        {
            Insurances = new List<InsuranceItem>();
        }
    }

    public class InsuranceItem
    {
        public string? Type { get; set; }
        public decimal? MonthlyCost { get; set; }
        public Vehicle? Vehicle { get; set; }
    }

}
