// Google Maps 3D Globe Implementation
let map;
let markers = [];
let infoWindows = [];
let rotationInterval = null;
let currentRotation = 0;

async function initializeGoogleGlobe(apiKey, nodes) {
    try {
        // Wait for Google Maps to load
        await waitForGoogleMaps();

        const mapContainer = document.getElementById('map');
        if (!mapContainer) {
            console.error('Map container not found');
            return;
        }

        // Initialize map with 3D capabilities
        const mapOptions = {
            zoom: 4,
            center: { lat: 37.5, lng: -95 }, // Center of USA
            mapTypeControl: true,
            mapTypeId: 'roadmap',
            styles: getDarkMapStyles(),
            gestureHandling: 'cooperative',
            fullscreenControl: true,
            streetViewControl: false,
        };

        map = new google.maps.Map(mapContainer, mapOptions);

        // Add markers for each quantum node
        addNodeMarkers(nodes);

        console.log('Google Maps 3D globe initialized successfully');
    } catch (error) {
        console.error('Error initializing globe:', error);
    }
}

function waitForGoogleMaps() {
    return new Promise((resolve, reject) => {
        if (typeof google !== 'undefined' && typeof google.maps !== 'undefined') {
            resolve();
            return;
        }

        let attempts = 0;
        const checkInterval = setInterval(() => {
            attempts++;
            if (typeof google !== 'undefined' && typeof google.maps !== 'undefined') {
                clearInterval(checkInterval);
                resolve();
            }
            if (attempts > 50) {
                clearInterval(checkInterval);
                reject(new Error('Google Maps failed to load'));
            }
        }, 100);
    });
}

function addNodeMarkers(nodes) {
    // Clear existing markers
    markers.forEach(marker => marker.setMap(null));
    markers = [];
    infoWindows.forEach(iw => iw.close());
    infoWindows = [];

    nodes.forEach((node, index) => {
        const position = { lat: node.latitude, lng: node.longitude };

        // Determine marker color based on status
        const markerColor = node.status.includes('Active') ? '#4CAF50' : '#FFC107';

        // Create custom marker
        const marker = new google.maps.Marker({
            position: position,
            map: map,
            title: node.name,
            label: {
                text: (index + 1).toString(),
                color: 'white',
                fontSize: '14px',
                fontWeight: 'bold'
            },
            icon: {
                path: google.maps.SymbolPath.CIRCLE,
                scale: 12,
                fillColor: markerColor,
                fillOpacity: 0.8,
                strokeColor: '#ffffff',
                strokeWeight: 2
            }
        });

        // Create info window
        const infoWindow = new google.maps.InfoWindow({
            content: `
                <div class="node-info-window" style="padding: 10px; font-family: Arial, sans-serif;">
                    <h4 style="margin: 0 0 8px 0; color: #333;">${node.name}</h4>
                    <p style="margin: 4px 0; font-size: 12px;">
                        <strong>Status:</strong> ${node.status}
                    </p>
                    <p style="margin: 4px 0; font-size: 12px;">
                        <strong>Capacity:</strong> ${node.capacity} Gbps
                    </p>
                    <p style="margin: 4px 0; font-size: 11px; color: #666;">
                        Lat: ${node.latitude.toFixed(4)}, Lng: ${node.longitude.toFixed(4)}
                    </p>
                </div>
            `
        });

        marker.addListener('click', () => {
            // Close all other info windows
            infoWindows.forEach(iw => iw.close());
            infoWindow.open(map, marker);
            // Center map on marker
            map.setCenter(position);
            map.setZoom(6);
        });

        markers.push(marker);
        infoWindows.push(infoWindow);
    });

    // Add connecting lines between nodes
    addConnectionLines(nodes);
}

function addConnectionLines(nodes) {
    const connections = [
        { from: 0, to: 1 }, // Chattanooga to Oak Ridge
        { from: 0, to: 2 }, // Chattanooga to Atlanta
        { from: 2, to: 3 }, // Atlanta to Washington
        { from: 3, to: 4 }, // Washington to Boston
        { from: 1, to: 2 }  // Oak Ridge to Atlanta
    ];

    connections.forEach(conn => {
        if (nodes[conn.from] && nodes[conn.to]) {
            const path = [
                { lat: nodes[conn.from].latitude, lng: nodes[conn.from].longitude },
                { lat: nodes[conn.to].latitude, lng: nodes[conn.to].longitude }
            ];

            new google.maps.Polyline({
                path: path,
                geodesic: true,
                strokeColor: '#667eea',
                strokeOpacity: 0.6,
                strokeWeight: 2,
                map: map,
                icons: [
                    {
                        icon: {
                            path: 'M 0,-2 0,2',
                            strokeColor: '#667eea',
                            strokeWeight: 2
                        },
                        offset: '0',
                        repeat: '20px'
                    }
                ]
            });
        }
    });
}

function toggleGlobeRotation(enable) {
    if (enable && !rotationInterval) {
        rotationInterval = setInterval(() => {
            currentRotation += 0.5;
            if (currentRotation >= 360) currentRotation = 0;

            const center = map.getCenter();
            const newLng = (center.lng() + 0.5) % 360;
            map.setCenter(new google.maps.LatLng(center.lat(), newLng));
        }, 50);
    } else if (!enable && rotationInterval) {
        clearInterval(rotationInterval);
        rotationInterval = null;
    }
}

function resetGlobeView() {
    if (rotationInterval) {
        clearInterval(rotationInterval);
        rotationInterval = null;
    }
    
    map.setZoom(4);
    map.setCenter({ lat: 37.5, lng: -95 });
    
    // Close all info windows
    infoWindows.forEach(iw => iw.close());
}

function toggleLabels(show) {
    markers.forEach(marker => {
        if (show) {
            marker.setLabel(marker.getTitle());
        } else {
            marker.setLabel(null);
        }
    });
}

function focusNode(latitude, longitude, name) {
    const position = new google.maps.LatLng(latitude, longitude);
    map.setCenter(position);
    map.setZoom(7);

    // Find and open corresponding info window
    const markerIndex = markers.findIndex(m => 
        m.getPosition().lat() === latitude && 
        m.getPosition().lng() === longitude
    );

    if (markerIndex !== -1) {
        infoWindows.forEach((iw, idx) => {
            if (idx !== markerIndex) iw.close();
        });
        infoWindows[markerIndex].open(map, markers[markerIndex]);
    }
}

function getDarkMapStyles() {
    return [
        {
            elementType: 'geometry',
            stylers: [{ color: '#242f3e' }]
        },
        {
            elementType: 'labels.text.stroke',
            stylers: [{ color: '#242f3e' }]
        },
        {
            elementType: 'labels.text.fill',
            stylers: [{ color: '#746855' }]
        },
        {
            featureType: 'administrative.locality',
            elementType: 'labels.text.fill',
            stylers: [{ color: '#d59563' }]
        },
        {
            featureType: 'poi',
            elementType: 'labels.text.fill',
            stylers: [{ color: '#d59563' }]
        },
        {
            featureType: 'poi.park',
            elementType: 'geometry',
            stylers: [{ color: '#263c3f' }]
        },
        {
            featureType: 'poi.park',
            elementType: 'labels.text.fill',
            stylers: [{ color: '#6b9080' }]
        },
        {
            featureType: 'road',
            elementType: 'geometry',
            stylers: [{ color: '#38414e' }]
        },
        {
            featureType: 'road',
            elementType: 'geometry.stroke',
            stylers: [{ color: '#212a37' }]
        },
        {
            featureType: 'road',
            elementType: 'labels.text.fill',
            stylers: [{ color: '#9ca5b3' }]
        },
        {
            featureType: 'road.highway',
            elementType: 'geometry',
            stylers: [{ color: '#746855' }]
        },
        {
            featureType: 'road.highway',
            elementType: 'geometry.stroke',
            stylers: [{ color: '#1f2835' }]
        },
        {
            featureType: 'road.highway',
            elementType: 'labels.text.fill',
            stylers: [{ color: '#f3d19c' }]
        },
        {
            featureType: 'transit',
            elementType: 'geometry',
            stylers: [{ color: '#2f3948' }]
        },
        {
            featureType: 'transit.station',
            elementType: 'labels.text.fill',
            stylers: [{ color: '#d59563' }]
        },
        {
            featureType: 'water',
            elementType: 'geometry',
            stylers: [{ color: '#17263c' }]
        },
        {
            featureType: 'water',
            elementType: 'labels.text.fill',
            stylers: [{ color: '#515c6d' }]
        },
        {
            featureType: 'water',
            elementType: 'labels.text.stroke',
            stylers: [{ color: '#17263c' }]
        }
    ];
}

// Export functions to global scope
window.initializeGoogleGlobe = initializeGoogleGlobe;
window.toggleGlobeRotation = toggleGlobeRotation;
window.resetGlobeView = resetGlobeView;
window.toggleLabels = toggleLabels;
window.focusNode = focusNode;
