// Quantum Globe Visualization using Three.js
let scene, camera, renderer, globe, links = [];
let animationFrameId = null;

// Wait for Three.js to be loaded
async function waitForThreeJs() {
    return new Promise((resolve) => {
        if (typeof THREE !== 'undefined') {
            resolve();
            return;
        }
        const checkInterval = setInterval(() => {
            if (typeof THREE !== 'undefined') {
                clearInterval(checkInterval);
                resolve();
            }
        }, 100);
        // Timeout after 5 seconds
        setTimeout(() => {
            clearInterval(checkInterval);
            resolve();
        }, 5000);
    });
}

async function initializeQuantumGlobe(quantumLinks) {
    // Wait for Three.js to load
    await waitForThreeJs();

    // Scene setup
    const canvas = document.getElementById('globe-canvas');
    if (!canvas) {
        console.error('globe-canvas element not found');
        return;
    }

    // Clear any existing content
    canvas.innerHTML = '';

    // Check if Three.js is loaded
    if (typeof THREE === 'undefined') {
        console.error('Three.js not loaded');
        return;
    }

    try {
        scene = new THREE.Scene();
        camera = new THREE.PerspectiveCamera(75, canvas.clientWidth / canvas.clientHeight, 0.1, 1000);
        renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
        renderer.setSize(canvas.clientWidth, canvas.clientHeight);
        renderer.setClearColor(0x1a1a2e, 1);
        canvas.appendChild(renderer.domElement);

        camera.position.z = 2.5;

        // Lighting
        const ambientLight = new THREE.AmbientLight(0xffffff, 0.6);
        scene.add(ambientLight);

        const pointLight = new THREE.PointLight(0x667eea, 1);
        pointLight.position.set(5, 3, 5);
        scene.add(pointLight);

        // Create Earth globe
        createGlobe();

        // Create quantum network links
        if (quantumLinks && quantumLinks.length > 0) {
            createQuantumLinks(quantumLinks);
        }

        // Animation loop
        function animate() {
            animationFrameId = requestAnimationFrame(animate);
            
            // Rotate globe
            if (globe) {
                globe.rotation.y += 0.0005;
            }

            // Animate links
            links.forEach((link, index) => {
                if (link.line && link.line.material.uniforms) {
                    link.line.material.uniforms.time.value += 0.01;
                }
            });

            renderer.render(scene, camera);
        }

        animate();

        // Handle window resize
        window.addEventListener('resize', onWindowResize);

        console.log('Quantum globe initialized successfully');
    } catch (error) {
        console.error('Error initializing quantum globe:', error);
    }
}

function createGlobe() {
    // Create sphere geometry
    const geometry = new THREE.SphereGeometry(1, 64, 64);

    // Create canvas texture for Earth
    const canvas = document.createElement('canvas');
    canvas.width = 2048;
    canvas.height = 1024;
    const ctx = canvas.getContext('2d');

    // Draw Earth texture
    ctx.fillStyle = '#1a3a52';
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    // Draw continents (simplified)
    ctx.fillStyle = '#2d5016';
    ctx.beginPath();
    ctx.arc(300, 250, 150, 0, Math.PI * 2);
    ctx.fill();
    ctx.beginPath();
    ctx.arc(800, 300, 100, 0, Math.PI * 2);
    ctx.fill();

    // Add glow effect points for quantum nodes
    ctx.fillStyle = 'rgba(102, 126, 234, 0.8)';
    // Chattanooga
    ctx.beginPath();
    ctx.arc(450, 380, 8, 0, Math.PI * 2);
    ctx.fill();
    // Oak Ridge
    ctx.beginPath();
    ctx.arc(480, 350, 6, 0, Math.PI * 2);
    ctx.fill();
    // Atlanta
    ctx.beginPath();
    ctx.arc(460, 400, 6, 0, Math.PI * 2);
    ctx.fill();

    const texture = new THREE.CanvasTexture(canvas);
    const material = new THREE.MeshPhongMaterial({
        map: texture,
        emissive: 0x1a3a52,
        emissiveIntensity: 0.3
    });

    globe = new THREE.Mesh(geometry, material);
    scene.add(globe);

    // Add atmosphere glow
    const atmosphereGeometry = new THREE.SphereGeometry(1.05, 64, 64);
    const atmosphereMaterial = new THREE.MeshPhongMaterial({
        color: 0x667eea,
        emissive: 0x667eea,
        emissiveIntensity: 0.1,
        transparent: true,
        opacity: 0.2,
        side: THREE.BackSide
    });
    const atmosphere = new THREE.Mesh(atmosphereGeometry, atmosphereMaterial);
    scene.add(atmosphere);
}

function createQuantumLinks(quantumLinks) {
    quantumLinks.forEach((link) => {
        // Convert lat/lng to 3D coordinates
        const sourcePos = latLngToVector3(link.source.lat, link.source.lng);
        const targetPos = latLngToVector3(link.target.lat, link.target.lng);

        // Create line with flowing animation
        const points = [sourcePos, targetPos];
        const lineGeometry = new THREE.BufferGeometry().setFromPoints(points);

        // Custom shader material for flowing quantum effect
        const lineMaterial = new THREE.ShaderMaterial({
            uniforms: {
                time: { value: 0 },
                color: { value: new THREE.Color(0x667eea) }
            },
            vertexShader: `
                void main() {
                    gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
                }
            `,
            fragmentShader: `
                uniform float time;
                uniform vec3 color;
                void main() {
                    gl_FragColor = vec4(color, 0.8);
                }
            `,
            transparent: true,
            linewidth: 3
        });

        const line = new THREE.Line(lineGeometry, lineMaterial);
        line.material.uniforms.time = { value: 0 };
        scene.add(line);

        // Create node markers
        createNodeMarker(sourcePos, link.source.name, 0.08, true);
        createNodeMarker(targetPos, link.target.name, 0.06, false);

        links.push({ line, source: sourcePos, target: targetPos });
    });
}

function createNodeMarker(position, name, size, isHub) {
    const geometry = new THREE.SphereGeometry(size, 32, 32);
    const material = new THREE.MeshPhongMaterial({
        color: isHub ? 0x667eea : 0x764ba2,
        emissive: isHub ? 0x667eea : 0x764ba2,
        emissiveIntensity: 0.8,
        wireframe: false
    });
    const marker = new THREE.Mesh(geometry, material);
    marker.position.copy(position);
    scene.add(marker);

    // Add glow
    const glowGeometry = new THREE.SphereGeometry(size * 2, 32, 32);
    const glowMaterial = new THREE.MeshPhongMaterial({
        color: isHub ? 0x667eea : 0x764ba2,
        emissive: isHub ? 0x667eea : 0x764ba2,
        emissiveIntensity: 0.3,
        transparent: true,
        opacity: 0.3,
        side: THREE.BackSide
    });
    const glow = new THREE.Mesh(glowGeometry, glowMaterial);
    glow.position.copy(position);
    scene.add(glow);
}

function latLngToVector3(lat, lng) {
    const phi = (90 - lat) * Math.PI / 180;
    const theta = (lng + 180) * Math.PI / 180;

    const x = Math.sin(phi) * Math.cos(theta);
    const y = Math.cos(phi);
    const z = Math.sin(phi) * Math.sin(theta);

    return new THREE.Vector3(x, y, z);
}

function onWindowResize() {
    const canvas = document.getElementById('globe-canvas');
    if (!canvas || !camera || !renderer) return;

    const width = canvas.clientWidth;
    const height = canvas.clientHeight;
    camera.aspect = width / height;
    camera.updateProjectionMatrix();
    renderer.setSize(width, height);
}

function disposeQuantumGlobe() {
    if (animationFrameId) {
        cancelAnimationFrame(animationFrameId);
    }
    if (renderer) {
        renderer.dispose();
    }
    links = [];
    scene = null;
    camera = null;
    renderer = null;
    globe = null;
}

// Export for use in Blazor
window.initializeQuantumGlobe = initializeQuantumGlobe;
window.disposeQuantumGlobe = disposeQuantumGlobe;
