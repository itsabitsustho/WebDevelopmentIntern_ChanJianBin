 // Scene setup
        const scene = new THREE.Scene();
        const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 5000);
        const renderer = new THREE.WebGLRenderer({ antialias: true });
        renderer.setSize(window.innerWidth, window.innerHeight);
        renderer.shadowMap.enabled = true;
        renderer.shadowMap.type = THREE.PCFSoftShadowMap;
        renderer.setClearColor(0x000000);
        document.getElementById('container').appendChild(renderer.domElement);

        // Add stars
        const starGeometry = new THREE.BufferGeometry();
        const starVertices = [];
        for (let i = 0; i < 8000; i++) {
            const x = (Math.random() - 0.5) * 2000;
            const y = (Math.random() - 0.5) * 2000;
            const z = (Math.random() - 0.5) * 2000;
            starVertices.push(x, y, z);
        }
        starGeometry.setAttribute('position', new THREE.Float32BufferAttribute(starVertices, 3));
        const starMaterial = new THREE.PointsMaterial({
            color: 0xFFFFFF,
            size: 2.0,
            transparent: true,
            opacity: 0.8
        });
        const stars = new THREE.Points(starGeometry, starMaterial);
        scene.add(stars);

        // Lighting setup
        const ambientLight = new THREE.AmbientLight(0xffffff, 0.6);
        scene.add(ambientLight);

        const sunLight = new THREE.PointLight(0xfff5cc, 3.5, 2000);
        sunLight.position.set(0, 0, 0);
        sunLight.castShadow = true;
        scene.add(sunLight);

        // Camera controls
        const controls = {
            mouseX: 0,
            mouseY: 0,
            mouseXOnMouseDown: 0,
            mouseYOnMouseDown: 0,
            targetRotationX: 0,
            targetRotationY: 0,
            targetRotationOnMouseDownX: 0,
            targetRotationOnMouseDownY: 0,
            isMouseDown: false,
            isDragging: false,
            dragStartTime: 0
        };

        // Set initial camera position
        camera.position.set(0, 30, 150);
        camera.lookAt(0, 0, 0);

        // Planet data with better scaling
        const planetData = {
            sun: {
                name: "Sun",
                radius: 12,
                distance: 0,
                color: 0xFFD700,
                rotationSpeed: 0.002,
                orbitSpeed: 0,
                info: {
                    description: "The Sun is the star at the center of our Solar System. It's a nearly perfect sphere of hot plasma, with internal convective motion that generates a magnetic field.",
                    facts: {
                        "Type": "G-type main-sequence star",
                        "Mass": "1.989 × 10³⁰ kg",
                        "Temperature": "5,778 K (surface)",
                        "Age": "4.6 billion years",
                        "Composition": "73% Hydrogen, 25% Helium",
                        "Distance from Earth": "149.6 million km"
                    }
                }
            },
            mercury: {
                name: "Mercury",
                radius: 2,
                distance: 20,
                color: 0x8C7853,
                rotationSpeed: 0.001,
                orbitSpeed: 0.04,
                info: {
                    description: "Mercury is the smallest planet in our Solar System and the closest to the Sun. It has extreme temperature variations and no atmosphere.",
                    facts: {
                        "Type": "Terrestrial planet",
                        "Mass": "3.301 × 10²³ kg",
                        "Diameter": "4,879 km",
                        "Day length": "59 Earth days",
                        "Year length": "88 Earth days",
                        "Distance from Sun": "57.9 million km"
                    }
                }
            },
            venus: {
                name: "Venus",
                radius: 3,
                distance: 30,
                color: 0xFFA500,
                rotationSpeed: -0.0005,
                orbitSpeed: 0.025,
                info: {
                    description: "Venus is the second planet from the Sun and the hottest in our Solar System due to its thick toxic atmosphere that traps heat.",
                    facts: {
                        "Type": "Terrestrial planet",
                        "Mass": "4.867 × 10²⁴ kg",
                        "Diameter": "12,104 km",
                        "Day length": "243 Earth days",
                        "Year length": "225 Earth days",
                        "Surface temperature": "464°C"
                    }
                }
            },
            earth: {
                name: "Earth",
                radius: 3.2,
                distance: 40,
                color: 0x6B93D6,
                rotationSpeed: 0.01,
                orbitSpeed: 0.02,
                info: {
                    description: "Earth is the third planet from the Sun and the only known planet to harbor life. It has a protective atmosphere and liquid water on its surface.",
                    facts: {
                        "Type": "Terrestrial planet",
                        "Mass": "5.972 × 10²⁴ kg",
                        "Diameter": "12,756 km",
                        "Day length": "24 hours",
                        "Year length": "365.25 days",
                        "Atmosphere": "78% Nitrogen, 21% Oxygen"
                    }
                }
            },
            mars: {
                name: "Mars",
                radius: 2.8,
                distance: 55,
                color: 0xCD5C5C,
                rotationSpeed: 0.008,
                orbitSpeed: 0.015,
                info: {
                    description: "Mars is the fourth planet from the Sun, known as the 'Red Planet' due to iron oxide on its surface. It has the largest volcano in the Solar System.",
                    facts: {
                        "Type": "Terrestrial planet",
                        "Mass": "6.417 × 10²³ kg",
                        "Diameter": "6,792 km",
                        "Day length": "24.6 hours",
                        "Year length": "687 Earth days",
                        "Largest mountain": "Olympus Mons (21 km high)"
                    }
                }
            },
            jupiter: {
                name: "Jupiter",
                radius: 9,
                distance: 80,
                color: 0xD2691E,
                rotationSpeed: 0.02,
                orbitSpeed: 0.008,
                info: {
                    description: "Jupiter is the largest planet in our Solar System, a gas giant with a Great Red Spot storm and over 80 moons.",
                    facts: {
                        "Type": "Gas giant",
                        "Mass": "1.898 × 10²⁷ kg",
                        "Diameter": "142,984 km",
                        "Day length": "9.9 hours",
                        "Year length": "12 Earth years",
                        "Number of moons": "95+"
                    }
                }
            },
            saturn: {
                name: "Saturn",
                radius: 8,
                distance: 110,
                color: 0xFAD5A5,
                rotationSpeed: 0.018,
                orbitSpeed: 0.006,
                info: {
                    description: "Saturn is the sixth planet from the Sun, famous for its prominent ring system made of ice and rock particles.",
                    facts: {
                        "Type": "Gas giant",
                        "Mass": "5.683 × 10²⁶ kg",
                        "Diameter": "120,536 km",
                        "Day length": "10.7 hours",
                        "Year length": "29 Earth years",
                        "Ring span": "282,000 km"
                    }
                }
            },
            uranus: {
                name: "Uranus",
                radius: 6,
                distance: 140,
                color: 0x4FD0E7,
                rotationSpeed: 0.012,
                orbitSpeed: 0.004,
                info: {
                    description: "Uranus is an ice giant that rotates on its side, likely due to a collision with another celestial body. It has faint rings and 27 known moons.",
                    facts: {
                        "Type": "Ice giant",
                        "Mass": "8.681 × 10²⁵ kg",
                        "Diameter": "51,118 km",
                        "Day length": "17.2 hours",
                        "Year length": "84 Earth years",
                        "Axial tilt": "98 degrees"
                    }
                }
            },
            neptune: {
                name: "Neptune",
                radius: 5.8,
                distance: 170,
                color: 0x4169E1,
                rotationSpeed: 0.015,
                orbitSpeed: 0.003,
                info: {
                    description: "Neptune is the farthest planet from the Sun, an ice giant with the strongest winds in the Solar System reaching speeds of 2,100 km/h.",
                    facts: {
                        "Type": "Ice giant",
                        "Mass": "1.024 × 10²⁶ kg",
                        "Diameter": "49,528 km",
                        "Day length": "16.1 hours",
                        "Year length": "165 Earth years",
                        "Wind speeds": "Up to 2,100 km/h"
                    }
                }
            }
        };

        // Create planets and orbits
        const planets = {};
        const orbits = {};
        const solarSystem = new THREE.Group();
        scene.add(solarSystem);

        Object.keys(planetData).forEach(key => {
            const data = planetData[key];

            // Create planet
            const geometry = new THREE.SphereGeometry(data.radius, 32, 32);
            let material;

            if (key === 'sun') {
                material = new THREE.MeshBasicMaterial({
                    color: data.color,
                    emissive: data.color,
                    emissiveIntensity: 0.5
                });
            } else {
                material = new THREE.MeshStandardMaterial({
                    color: data.color,
                    emissive: data.color,
                    emissiveIntensity: 0.15,
                    metalness: 0.2,
                    roughness: 0.7
                });
            }

            const planet = new THREE.Mesh(geometry, material);

            if (key !== 'sun') {
                planet.castShadow = true;
                planet.receiveShadow = true;
            }

            planet.position.x = data.distance;
            planet.userData = { name: key, ...data };
            planets[key] = planet;
            solarSystem.add(planet);

            // Create planet label
            const label = document.createElement('div');
            label.className = 'planet-label';
            label.textContent = data.name;
            label.style.position = 'absolute';
            label.style.whiteSpace = 'nowrap';
            document.body.appendChild(label);
            planet.userData.labelElement = label;


            // Create orbit visualization
            if (key !== 'sun') {
                const orbitPoints = [];
                for (let i = 0; i <= 64; i++) {
                    const angle = (i / 64) * Math.PI * 2;
                    orbitPoints.push(new THREE.Vector3(
                        Math.cos(angle) * data.distance,
                        0,
                        Math.sin(angle) * data.distance
                    ));
                }

                const orbitGeometry = new THREE.BufferGeometry().setFromPoints(orbitPoints);
                const orbitMaterial = new THREE.LineBasicMaterial({
                    color: 0x4a9eff,
                    linewidth: 2,
                    transparent: true,
                    opacity: 0.4
                });
                const orbit = new THREE.Line(orbitGeometry, orbitMaterial);
                orbits[key] = orbit;
                solarSystem.add(orbit);
            }

            // Add Saturn's rings
            if (key === 'saturn') {
                const ringGeometry = new THREE.RingGeometry(data.radius + 1, data.radius + 4, 32);
                const ringMaterial = new THREE.MeshLambertMaterial({
                    color: 0xC2B280,
                    side: THREE.DoubleSide,
                    transparent: true,
                    opacity: 0.8
                });
                const rings = new THREE.Mesh(ringGeometry, ringMaterial);
                rings.rotation.x = -Math.PI / 2;
                planet.add(rings);
            }
        });

        // Mouse event handlers
        function onMouseDown(event) {
            event.preventDefault();
            controls.isMouseDown = true;
            controls.isDragging = false;
            controls.dragStartTime = Date.now();

            controls.mouseXOnMouseDown = event.clientX - window.innerWidth / 2;
            controls.mouseYOnMouseDown = event.clientY - window.innerHeight / 2;

            controls.targetRotationOnMouseDownX = controls.targetRotationX;
            controls.targetRotationOnMouseDownY = controls.targetRotationY;

            document.addEventListener('mousemove', onMouseMove);
            document.addEventListener('mouseup', onMouseUp);
        }

        function onMouseMove(event) {
            if (!controls.isMouseDown) return;

            const deltaTime = Date.now() - controls.dragStartTime;
            const deltaDistance = Math.abs(event.clientX - (controls.mouseXOnMouseDown + window.innerWidth / 2)) +
                Math.abs(event.clientY - (controls.mouseYOnMouseDown + window.innerHeight / 2));

            if (deltaTime > 100 || deltaDistance > 5) {
                controls.isDragging = true;
            }

            controls.mouseX = event.clientX - window.innerWidth / 2;
            controls.mouseY = event.clientY - window.innerHeight / 2;

            controls.targetRotationY = controls.targetRotationOnMouseDownY + (controls.mouseX - controls.mouseXOnMouseDown) * 0.005;
            controls.targetRotationX = controls.targetRotationOnMouseDownX + (controls.mouseY - controls.mouseYOnMouseDown) * 0.005;
        }

        function onMouseUp(event) {
            controls.isMouseDown = false;

            // Only trigger click if not dragging
            if (!controls.isDragging) {
                handleClick(event);
            }

            controls.isDragging = false;

            document.removeEventListener('mousemove', onMouseMove);
            document.removeEventListener('mouseup', onMouseUp);
        }

        function handleClick(event) {
            const mouse = new THREE.Vector2();
            mouse.x = (event.clientX / window.innerWidth) * 2 - 1;
            mouse.y = -(event.clientY / window.innerHeight) * 2 + 1;

            const raycaster = new THREE.Raycaster();
            raycaster.setFromCamera(mouse, camera);

            const planetMeshes = Object.values(planets);
            const intersects = raycaster.intersectObjects(planetMeshes);

            if (intersects.length > 0) {
                const planet = intersects[0].object;
                showPlanetInfo(planet.userData);
            }
        }

        // Zoom control
        function onWheel(event) {
            const zoomSpeed = 5;
            camera.position.z += event.deltaY * zoomSpeed * 0.01;
            camera.position.z = Math.max(20, Math.min(400, camera.position.z));
        }

        // Add event listeners
        renderer.domElement.addEventListener('mousedown', onMouseDown);
        renderer.domElement.addEventListener('wheel', onWheel);

        // Modal functionality
        const modal = document.getElementById('planetModal');
        const closeBtn = document.getElementsByClassName('close')[0];

        closeBtn.onclick = function () {
            modal.style.display = 'none';
        }

        window.onclick = function (event) {
            if (event.target === modal) {
                modal.style.display = 'none';
            }
        }

        function showPlanetInfo(planetInfo) {
            document.getElementById('modalTitle').textContent = planetInfo.name;

            const facts = planetInfo.info.facts;
            let factsHTML = `<p>${planetInfo.info.description}</p><div class="fact-grid">`;

            Object.keys(facts).forEach(key => {
                factsHTML += `
                    <div class="fact-item">
                        <div class="fact-label">${key}</div>
                        <div>${facts[key]}</div>
                    </div>
                `;
            });

            factsHTML += '</div>';
            document.getElementById('modalBody').innerHTML = factsHTML;
            modal.style.display = 'block';
        }

        // Animation loop
        function animate() {
            requestAnimationFrame(animate);

            // Update camera rotation based on mouse input
            const rotationSpeed = 0.1;
            solarSystem.rotation.y += (controls.targetRotationY - solarSystem.rotation.y) * rotationSpeed;
            solarSystem.rotation.x += (controls.targetRotationX - solarSystem.rotation.x) * rotationSpeed;

            // Animate planets
            Object.keys(planets).forEach(key => {
                const planet = planets[key];
                const data = planetData[key];

                // Rotate planet on its axis
                planet.rotation.y += data.rotationSpeed;

                // Orbit around sun
                if (key !== 'sun') {
                    const time = Date.now() * 0.001;
                    planet.position.x = Math.cos(time * data.orbitSpeed) * data.distance;
                    planet.position.z = Math.sin(time * data.orbitSpeed) * data.distance;
                }
            });

            // Update planet label positions
            Object.values(planets).forEach(planet => {
                if (!planet.userData.labelElement) return;

                const pos = new THREE.Vector3();
                pos.setFromMatrixPosition(planet.matrixWorld);
                pos.project(camera);

                const x = (pos.x * 0.5 + 0.5) * window.innerWidth;
                const y = (-pos.y * 0.5 + 0.5) * window.innerHeight + 15;

                if (pos.z > 1 || pos.z < -1) {
                    planet.userData.labelElement.style.display = 'none';
                } else {
                    planet.userData.labelElement.style.display = 'block';
                    planet.userData.labelElement.style.transform =
                        `translate(-50%, -50%) translate(${x}px, ${y}px)`;
                }
            });

            renderer.render(scene, camera);
        }

        // Handle window resize
        window.addEventListener('resize', () => {
            camera.aspect = window.innerWidth / window.innerHeight;
            camera.updateProjectionMatrix();
            renderer.setSize(window.innerWidth, window.innerHeight);

            Object.values(planets).forEach(planet => {
                if (planet.userData.labelElement) {
                    planet.userData.labelElement.style.display = 'none';
                }
            });
        });

        // Initialize
        document.getElementById('loading').style.display = 'none';
        animate();