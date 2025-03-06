namespace CarService.Model;

public class CarCustomer
{
    public Guid CarId { get; set; }
    public Car Car { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
}