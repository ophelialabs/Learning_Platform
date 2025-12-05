using Microsoft.AspNetCore.Mvc;
using LP_app.Dtos;
using Microsoft.Extensions.Logging;

namespace LP_app.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class QuantumNetworksController : ControllerBase
{
    private readonly ILogger<QuantumNetworksController> _logger;

    public QuantumNetworksController(ILogger<QuantumNetworksController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get all quantum network nodes
    /// </summary>
    /// <param name="pageNumber">Page number (default 1)</param>
    /// <param name="pageSize">Page size (default 20, max 100)</param>
    /// <returns>List of quantum nodes</returns>
    [HttpGet("nodes")]
    public ActionResult<List<QuantumNodeReadDto>> GetAllNodes(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 20;

            _logger.LogInformation("Fetching quantum network nodes. Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

            // Sample data - replace with database query
            var nodes = new List<QuantumNodeReadDto>
            {
                new QuantumNodeReadDto
                {
                    Id = 1,
                    Name = "Chattanooga QNet",
                    Latitude = 35.0456,
                    Longitude = -85.3097,
                    NodeType = "QUANTUM_HUB",
                    Status = "active",
                    Capabilities = new CapabilitiesDto
                    {
                        QuantumKeyDistribution = true,
                        EntanglementDistribution = true,
                        QuantumMemory = true,
                        QuantumTeleportation = false
                    },
                    CreatedAt = DateTime.UtcNow.AddMonths(-6),
                    UpdatedAt = DateTime.UtcNow
                },
                new QuantumNodeReadDto
                {
                    Id = 2,
                    Name = "Oak Ridge",
                    Latitude = 35.9606,
                    Longitude = -83.9207,
                    NodeType = "QUANTUM_NODE",
                    Status = "active",
                    Capabilities = new CapabilitiesDto
                    {
                        QuantumKeyDistribution = true,
                        EntanglementDistribution = true,
                        QuantumMemory = false,
                        QuantumTeleportation = true
                    },
                    CreatedAt = DateTime.UtcNow.AddMonths(-5),
                    UpdatedAt = DateTime.UtcNow
                },
                new QuantumNodeReadDto
                {
                    Id = 3,
                    Name = "Atlanta",
                    Latitude = 33.7490,
                    Longitude = -84.3880,
                    NodeType = "QUANTUM_NODE",
                    Status = "active",
                    Capabilities = new CapabilitiesDto
                    {
                        QuantumKeyDistribution = true,
                        EntanglementDistribution = false,
                        QuantumMemory = true,
                        QuantumTeleportation = false
                    },
                    CreatedAt = DateTime.UtcNow.AddMonths(-4),
                    UpdatedAt = DateTime.UtcNow
                }
            };

            return Ok(nodes);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching quantum network nodes: {Message}", ex.Message);
            return BadRequest(new { error = "Failed to fetch nodes", details = ex.Message });
        }
    }

    /// <summary>
    /// Get quantum node by ID
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <returns>Quantum node details</returns>
    [HttpGet("nodes/{id}")]
    public ActionResult<QuantumNodeReadDto> GetNodeById([FromRoute] int id)
    {
        try
        {
            _logger.LogInformation("Fetching quantum node with ID: {NodeId}", id);

            if (id <= 0)
                return BadRequest(new { error = "Invalid node ID" });

            // Sample data
            var node = new QuantumNodeReadDto
            {
                Id = id,
                Name = $"Quantum Node {id}",
                Latitude = 35.0456 + (id * 0.5),
                Longitude = -85.3097 + (id * 0.5),
                NodeType = "QUANTUM_NODE",
                Status = "active",
                Capabilities = new CapabilitiesDto
                {
                    QuantumKeyDistribution = true,
                    EntanglementDistribution = true,
                    QuantumMemory = true,
                    QuantumTeleportation = false
                },
                CreatedAt = DateTime.UtcNow.AddMonths(-3),
                UpdatedAt = DateTime.UtcNow
            };

            return Ok(node);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching quantum node {NodeId}: {Message}", id, ex.Message);
            return BadRequest(new { error = "Failed to fetch node", details = ex.Message });
        }
    }

    /// <summary>
    /// Create a new quantum network node
    /// </summary>
    /// <param name="createDto">Node creation details</param>
    /// <returns>Created node with ID</returns>
    [HttpPost("nodes")]
    public ActionResult<QuantumNodeReadDto> CreateNode([FromBody] QuantumNodeCreateDto createDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(createDto.Name))
                return BadRequest(new { error = "Node name is required" });

            _logger.LogInformation("Creating quantum node: {NodeName}", createDto.Name);

            var newNode = new QuantumNodeReadDto
            {
                Id = new Random().Next(1000, 9999),
                Name = createDto.Name,
                Latitude = createDto.Latitude,
                Longitude = createDto.Longitude,
                NodeType = createDto.NodeType,
                Status = "active",
                Capabilities = createDto.Capabilities ?? new CapabilitiesDto(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return CreatedAtAction(nameof(GetNodeById), new { id = newNode.Id }, newNode);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating quantum node: {Message}", ex.Message);
            return BadRequest(new { error = "Failed to create node", details = ex.Message });
        }
    }

    /// <summary>
    /// Update quantum node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="updateDto">Update details</param>
    /// <returns>Updated node</returns>
    [HttpPut("nodes/{id}")]
    public ActionResult<QuantumNodeReadDto> UpdateNode(
        [FromRoute] int id,
        [FromBody] QuantumNodeUpdateDto updateDto)
    {
        try
        {
            if (id <= 0)
                return BadRequest(new { error = "Invalid node ID" });

            _logger.LogInformation("Updating quantum node {NodeId}", id);

            // Sample data - replace with actual update logic
            var updatedNode = new QuantumNodeReadDto
            {
                Id = id,
                Name = updateDto.Name ?? "Quantum Node",
                Latitude = 35.0456,
                Longitude = -85.3097,
                NodeType = "QUANTUM_NODE",
                Status = updateDto.Status ?? "active",
                Capabilities = updateDto.Capabilities ?? new CapabilitiesDto(),
                CreatedAt = DateTime.UtcNow.AddMonths(-3),
                UpdatedAt = DateTime.UtcNow
            };

            return Ok(updatedNode);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating quantum node {NodeId}: {Message}", id, ex.Message);
            return BadRequest(new { error = "Failed to update node", details = ex.Message });
        }
    }

    /// <summary>
    /// Delete quantum node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <returns>No content</returns>
    [HttpDelete("nodes/{id}")]
    public ActionResult DeleteNode([FromRoute] int id)
    {
        try
        {
            if (id <= 0)
                return BadRequest(new { error = "Invalid node ID" });

            _logger.LogInformation("Deleting quantum node {NodeId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting quantum node {NodeId}: {Message}", id, ex.Message);
            return BadRequest(new { error = "Failed to delete node", details = ex.Message });
        }
    }

    /// <summary>
    /// Get all quantum network links
    /// </summary>
    /// <returns>List of quantum links</returns>
    [HttpGet("links")]
    public ActionResult<List<QuantumLinkReadDto>> GetAllLinks()
    {
        try
        {
            _logger.LogInformation("Fetching all quantum network links");

            var links = new List<QuantumLinkReadDto>
            {
                new QuantumLinkReadDto
                {
                    Id = 1,
                    SourceNodeId = 1,
                    TargetNodeId = 2,
                    LinkType = "fiber",
                    Status = "active",
                    Bandwidth = 100,
                    Fidelity = 0.98,
                    RangeKm = 100,
                    EstablishedDate = DateTime.UtcNow.AddMonths(-3),
                    UpdatedAt = DateTime.UtcNow
                },
                new QuantumLinkReadDto
                {
                    Id = 2,
                    SourceNodeId = 1,
                    TargetNodeId = 3,
                    LinkType = "fiber",
                    Status = "active",
                    Bandwidth = 100,
                    Fidelity = 0.96,
                    RangeKm = 200,
                    EstablishedDate = DateTime.UtcNow.AddMonths(-2),
                    UpdatedAt = DateTime.UtcNow
                },
                new QuantumLinkReadDto
                {
                    Id = 3,
                    SourceNodeId = 2,
                    TargetNodeId = 3,
                    LinkType = "satellite",
                    Status = "inactive",
                    Bandwidth = 10,
                    Fidelity = 0.85,
                    RangeKm = 500,
                    EstablishedDate = DateTime.UtcNow.AddMonths(-1),
                    UpdatedAt = DateTime.UtcNow
                }
            };

            return Ok(links);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching quantum network links: {Message}", ex.Message);
            return BadRequest(new { error = "Failed to fetch links", details = ex.Message });
        }
    }

    /// <summary>
    /// Get quantum network state
    /// </summary>
    /// <returns>Current network state metrics</returns>
    [HttpGet("state")]
    public ActionResult<QuantumNetworkStateDto> GetNetworkState()
    {
        try
        {
            _logger.LogInformation("Fetching quantum network state");

            var state = new QuantumNetworkStateDto
            {
                NodeStatus = "active",
                EntanglementRate = "1000 pairs/second",
                Fidelity = 0.98,
                QkdRate = "100 kbps",
                ActiveNodes = 3,
                ActiveLinks = 2,
                LastUpdated = DateTime.UtcNow
            };

            return Ok(state);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching network state: {Message}", ex.Message);
            return BadRequest(new { error = "Failed to fetch network state", details = ex.Message });
        }
    }

    /// <summary>
    /// Get network topology (nodes and links)
    /// </summary>
    /// <returns>Complete network topology</returns>
    [HttpGet("topology")]
    public ActionResult<NetworkTopologyDto> GetNetworkTopology()
    {
        try
        {
            _logger.LogInformation("Fetching quantum network topology");

            // Get nodes
            var nodesResult = GetAllNodes();
            var nodes = (nodesResult.Result as OkObjectResult)?.Value as List<QuantumNodeReadDto> ?? new();

            // Get links
            var linksResult = GetAllLinks();
            var links = (linksResult.Result as OkObjectResult)?.Value as List<QuantumLinkReadDto> ?? new();

            // Get state
            var stateResult = GetNetworkState();
            var state = (stateResult.Result as OkObjectResult)?.Value as QuantumNetworkStateDto;

            var topology = new NetworkTopologyDto
            {
                Nodes = nodes,
                Links = links,
                NetworkState = state,
                GeneratedAt = DateTime.UtcNow
            };

            return Ok(topology);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching network topology: {Message}", ex.Message);
            return BadRequest(new { error = "Failed to fetch topology", details = ex.Message });
        }
    }

    /// <summary>
    /// Create entanglement session
    /// </summary>
    /// <param name="createDto">Session creation details</param>
    /// <returns>Created session</returns>
    [HttpPost("entanglement-sessions")]
    public ActionResult<EntanglementSessionReadDto> CreateEntanglementSession(
        [FromBody] EntanglementSessionCreateDto createDto)
    {
        try
        {
            _logger.LogInformation("Creating entanglement session from node {SourceId} to {TargetId}",
                createDto.SourceNodeId, createDto.TargetNodeId);

            var session = new EntanglementSessionReadDto
            {
                Id = new Random().Next(1000, 9999),
                SourceNodeId = createDto.SourceNodeId,
                TargetNodeId = createDto.TargetNodeId,
                NumberOfQubitPairs = createDto.NumberOfQubitPairs,
                Fidelity = 0.98,
                Status = "active",
                StartTime = DateTime.UtcNow,
                DurationMinutes = 0
            };

            return CreatedAtAction(nameof(CreateEntanglementSession), session);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating entanglement session: {Message}", ex.Message);
            return BadRequest(new { error = "Failed to create session", details = ex.Message });
        }
    }

    /// <summary>
    /// Create QKD session
    /// </summary>
    /// <param name="createDto">QKD session creation details</param>
    /// <returns>Created QKD session</returns>
    [HttpPost("qkd-sessions")]
    public ActionResult<QkdSessionReadDto> CreateQkdSession(
        [FromBody] QkdSessionCreateDto createDto)
    {
        try
        {
            _logger.LogInformation("Creating QKD session from node {SourceId} to {TargetId} using {Protocol}",
                createDto.SourceNodeId, createDto.TargetNodeId, createDto.Protocol);

            var session = new QkdSessionReadDto
            {
                Id = new Random().Next(1000, 9999),
                SourceNodeId = createDto.SourceNodeId,
                TargetNodeId = createDto.TargetNodeId,
                KeyBitsDistributed = 256,
                Protocol = createDto.Protocol,
                Status = "active",
                StartTime = DateTime.UtcNow,
                SuccessRate = 0.99
            };

            return CreatedAtAction(nameof(CreateQkdSession), session);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating QKD session: {Message}", ex.Message);
            return BadRequest(new { error = "Failed to create QKD session", details = ex.Message });
        }
    }

    /// <summary>
    /// Get network metrics
    /// </summary>
    /// <returns>Network performance metrics</returns>
    [HttpGet("metrics")]
    public ActionResult<QuantumNetworkMetricsDto> GetNetworkMetrics()
    {
        try
        {
            _logger.LogInformation("Fetching quantum network metrics");

            var metrics = new QuantumNetworkMetricsDto
            {
                AverageLatency = 2.5,
                AverageFidelity = 0.97,
                TotalBandwidth = 210,
                TotalQubitsInMemory = 1024,
                SuccessRate = 0.985,
                MeasuredAt = DateTime.UtcNow
            };

            return Ok(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error fetching network metrics: {Message}", ex.Message);
            return BadRequest(new { error = "Failed to fetch metrics", details = ex.Message });
        }
    }
}
