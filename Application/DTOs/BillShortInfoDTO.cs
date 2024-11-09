namespace Application.DTOs;

public class BillShortInfoDTO
{
    public Guid Id { get; set; }
    public string OwnerName { get; set; }
    public decimal Amount { get; set; }
}