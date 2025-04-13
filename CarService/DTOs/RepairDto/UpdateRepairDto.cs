using System.Text.Json.Serialization;
using CarService.Enums;

namespace CarService.DTOs.RepairDto;

public class UpdateRepairDto
{
    public Guid RepairId { get; set; }
    public string Description { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]

    public RepairStatusEnum Status { get; set; }
}