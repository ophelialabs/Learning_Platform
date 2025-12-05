namespace LP_app.Dtos;

/// <summary>
/// DTO for quantum network node information
/// </summary>
public class QuantumNodeReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string NodeType { get; set; } = "";
    public string Status { get; set; } = "active";
    public CapabilitiesDto? Capabilities { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class QuantumNodeCreateDto
{
    public string Name { get; set; } = "";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string NodeType { get; set; } = "";
    public CapabilitiesDto? Capabilities { get; set; }
}

public class QuantumNodeUpdateDto
{
    public string? Name { get; set; }
    public string? Status { get; set; }
    public CapabilitiesDto? Capabilities { get; set; }
}

/// <summary>
/// DTO for quantum node capabilities
/// </summary>
public class CapabilitiesDto
{
    public bool QuantumKeyDistribution { get; set; }
    public bool EntanglementDistribution { get; set; }
    public bool QuantumMemory { get; set; }
    public bool QuantumTeleportation { get; set; }
}

/// <summary>
/// DTO for quantum network links/connections
/// </summary>
public class QuantumLinkReadDto
{
    public int Id { get; set; }
    public int SourceNodeId { get; set; }
    public int TargetNodeId { get; set; }
    public string LinkType { get; set; } = "";
    public string Status { get; set; } = "active";
    public double Bandwidth { get; set; }
    public double Fidelity { get; set; }
    public int RangeKm { get; set; }
    public DateTime EstablishedDate { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class QuantumLinkCreateDto
{
    public int SourceNodeId { get; set; }
    public int TargetNodeId { get; set; }
    public string LinkType { get; set; } = "";
    public double Bandwidth { get; set; }
    public int RangeKm { get; set; }
}

public class QuantumLinkUpdateDto
{
    public string? Status { get; set; }
    public double? Fidelity { get; set; }
    public double? Bandwidth { get; set; }
}

/// <summary>
/// DTO for real-time quantum network state
/// </summary>
public class QuantumNetworkStateDto
{
    public string NodeStatus { get; set; } = "active";
    public string EntanglementRate { get; set; } = "";
    public double Fidelity { get; set; }
    public string QkdRate { get; set; } = "";
    public int ActiveNodes { get; set; }
    public int ActiveLinks { get; set; }
    public DateTime LastUpdated { get; set; }
}

/// <summary>
/// DTO for quantum network metrics
/// </summary>
public class QuantumNetworkMetricsDto
{
    public double AverageLatency { get; set; }
    public double AverageFidelity { get; set; }
    public double TotalBandwidth { get; set; }
    public int TotalQubitsInMemory { get; set; }
    public double SuccessRate { get; set; }
    public DateTime MeasuredAt { get; set; }
}

/// <summary>
/// DTO for quantum entanglement session
/// </summary>
public class EntanglementSessionReadDto
{
    public int Id { get; set; }
    public int SourceNodeId { get; set; }
    public int TargetNodeId { get; set; }
    public int NumberOfQubitPairs { get; set; }
    public double Fidelity { get; set; }
    public string Status { get; set; } = "active";
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public double DurationMinutes { get; set; }
}

public class EntanglementSessionCreateDto
{
    public int SourceNodeId { get; set; }
    public int TargetNodeId { get; set; }
    public int NumberOfQubitPairs { get; set; }
}

/// <summary>
/// DTO for quantum key distribution session
/// </summary>
public class QkdSessionReadDto
{
    public int Id { get; set; }
    public int SourceNodeId { get; set; }
    public int TargetNodeId { get; set; }
    public int KeyBitsDistributed { get; set; }
    public string Protocol { get; set; } = "BB84";
    public string Status { get; set; } = "active";
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public double SuccessRate { get; set; }
}

public class QkdSessionCreateDto
{
    public int SourceNodeId { get; set; }
    public int TargetNodeId { get; set; }
    public string Protocol { get; set; } = "BB84";
}

/// <summary>
/// DTO for network topology response
/// </summary>
public class NetworkTopologyDto
{
    public List<QuantumNodeReadDto> Nodes { get; set; } = new();
    public List<QuantumLinkReadDto> Links { get; set; } = new();
    public QuantumNetworkStateDto? NetworkState { get; set; }
    public DateTime GeneratedAt { get; set; }
}

/// <summary>
/// DTO for error/alert in quantum network
/// </summary>
public class QuantumNetworkAlertDto
{
    public int Id { get; set; }
    public string Severity { get; set; } = "low"; // low, medium, high, critical
    public string Message { get; set; } = "";
    public string AffectedComponent { get; set; } = "";
    public DateTime OccurredAt { get; set; }
    public bool Resolved { get; set; }
    public DateTime? ResolvedAt { get; set; }
}
