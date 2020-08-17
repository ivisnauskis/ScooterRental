namespace ScooterRental
{
    public interface IRentalCompany
    {
        string Name { get; }
        void StartRent(string id);
        decimal EndRent(string id);
        decimal CalculateIncome(int? year, bool includeNotCompletedRentals);
    }
}