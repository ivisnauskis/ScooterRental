namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        public string Name { get; }
        public void StartRent(string id)
        {
            throw new System.NotImplementedException();
        }

        public decimal EndRent(string id)
        {
            throw new System.NotImplementedException();
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            throw new System.NotImplementedException();
        }
    }
}