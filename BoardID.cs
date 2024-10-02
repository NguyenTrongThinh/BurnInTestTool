

public class BoardID
{
    private string? ipAddress;
    private string? macAddress;

    public string? IpAddress { get => ipAddress; set => ipAddress = value; }
    public string? MacAddress { get => macAddress; set => macAddress = value; }

    public override bool Equals(object? obj)
    {
        return obj is BoardID iD &&
               ipAddress == iD.ipAddress &&
               macAddress == iD.macAddress &&
               IpAddress == iD.IpAddress &&
               MacAddress == iD.MacAddress;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ipAddress, macAddress, IpAddress, MacAddress);
    }
}
