using Domain.Enums;

namespace Domain.ValueObjects
{
    public record Device(string DeviceIdentifier, DeviceType DeviceType);
}
