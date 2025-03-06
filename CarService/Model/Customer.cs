namespace CarService.Model;

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public string PhoneNumber { get; set; }
    
    public ICollection<CarCustomer> CarCustomers { get; set; }
}