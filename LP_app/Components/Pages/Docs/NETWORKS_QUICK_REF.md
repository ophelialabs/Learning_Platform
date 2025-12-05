# Quick Reference - Networks Page & Quantum API

## 🌐 Networks Page
**Route:** `/Networks`
**File:** `Components/Pages/Research/Universities/Networks.razor`
**Views:** 3 (Globe, Topology, Details)

### Key Components:
- 3D rotating Earth (Three.js)
- 3 quantum network nodes (Chattanooga, Oak Ridge, Atlanta)
- 3 quantum links (2 fiber, 1 satellite)
- Real-time network metrics display
- View toggle buttons

---

## 🔌 Quantum Networks API
**Base URL:** `/api/quantum-networks`

### Quick Endpoints:
- `GET /nodes` - List all nodes (paginated)
- `GET /nodes/{id}` - Get specific node
- `POST /nodes` - Create node
- `GET /links` - List all links
- `GET /state` - Real-time network state
- `GET /topology` - Complete network topology
- `GET /metrics` - Network performance metrics
- `POST /entanglement-sessions` - Start entanglement
- `POST /qkd-sessions` - Start QKD session

### Example Request:
```bash
curl https://localhost:7296/api/quantum-networks/nodes
curl https://localhost:7296/api/quantum-networks/topology
```

---

## 📊 DTOs Available
- `QuantumNodeReadDto` / `QuantumNodeCreateDto` / `QuantumNodeUpdateDto`
- `QuantumLinkReadDto` / `QuantumLinkCreateDto` / `QuantumLinkUpdateDto`
- `QuantumNetworkStateDto` - {NodeStatus, EntanglementRate, Fidelity, QkdRate}
- `QuantumNetworkMetricsDto` - {AverageLatency, AverageFidelity, Bandwidth, SuccessRate}
- `EntanglementSessionReadDto` / `EntanglementSessionCreateDto`
- `QkdSessionReadDto` / `QkdSessionCreateDto`
- `NetworkTopologyDto` - Nodes + Links + State combined

---

## 🎨 Styling
- File: `/wwwroot/css/networks.css`
- Theme: Dark with quantum blue (#667eea)
- Effects: Glow, gradient, animations
- Responsive: Mobile-friendly at 768px breakpoint

---

## 🚀 3D Globe
- File: `/wwwroot/js/quantum-globe.js`
- Library: Three.js (CDN)
- Features: Rotating Earth, node markers, connection lines, glow effects
- Integration: Blazor JS interop

---

## 📝 Files Summary

| File | Lines | Purpose |
|------|-------|---------|
| Networks.razor | 480 | UI component with 3 views |
| QuantumNetworksController.cs | 450+ | REST API (12 endpoints) |
| QuantumNetworkDto.cs | 170 | 15 DTO classes |
| networks.css | 500+ | Dark theme styling |
| quantum-globe.js | 220 | 3D Earth visualization |

**Total:** ~2,000 lines of code

---

## ✅ Status
- **Build:** 0 Errors ✓
- **API:** Ready to test via Swagger ✓
- **UI:** Ready to display at /Networks ✓
- **Database:** Using sample data (ready for EF integration) ✓

---

## 🔗 Integration Ready
- Use HttpClient to call API from components
- All endpoints return JSON
- Sample data includes 3 nodes, 3 links, real-time metrics
- Ready for database integration with Entity Framework
