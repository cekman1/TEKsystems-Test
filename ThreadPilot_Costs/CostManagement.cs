using System.Collections.Generic;

namespace TEKsystem_Test_COSTS.CostManagementBackend
{
    public static class CostManagement
    {
        private static readonly Dictionary<string, decimal> InsuranceCosts = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Pet", 10 },
            { "Health", 20 },
            { "Car", 30 }
        };

        public static decimal GetMonthlyCost(string insuranceType)
        {
            if (insuranceType == null) return 0;

            return InsuranceCosts.TryGetValue(insuranceType, out var cost) ? cost : 0;
        }
    }
}