import * as THREE from 'https://threejs.org/build/three.module.js';
import { GLTFLoader } from 'https://threejs.org/examples/js/loaders/GLTFLoader.js';

const scene = new THREE.Scene();
const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
const renderer = new THREE.WebGLRenderer();
renderer.setSize(window.innerWidth, window.innerHeight);
document.body.appendChild(renderer.domElement);

const loader = new GLTFLoader();

// Postavite putanju do vaše CAD datoteke (u GLTF formatu)
const cadFilePath = 'putanja/do/vashe/cad.gltf';

loader.load(cadFilePath, (gltf) => {
    scene.add(gltf.scene);
}, undefined, (error) => {
    console.error('Error loading CAD model', error);
});

camera.position.z = 5;

const animate = function () {
    requestAnimationFrame(animate);
    renderer.render(scene, camera);
};

animate();
