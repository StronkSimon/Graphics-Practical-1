# Graphics-Practical-1

### Team members:  
- Simon de Jong 8562024  
- Yoeri Hoebe 4769341  

#### Formalities:  
[X] This readme.txt  
[X] Cleaned (no obj/bin folders)  

#### Minimum requirements implemented:  
[X] Camera: position and orientation controls, field of view in degrees  
[X] Primitives: plane, sphere  
[X] Lights: at least 2 point lights, additive contribution, shadows without "acne"  
[X] Diffuse shading: (N.L), distance attenuation  
[X] Phong shading: (R.V) or (N.H), exponent  
[X] Diffuse color texture: only required on the plane primitive, image or procedural, (u,v) texture coordinates  
[X] Mirror reflection: recursive  
[X] Debug visualization: sphere primitives, rays (primary, shadow, reflected, refracted)  

#### Bonus features implemented:  
[ ] Triangle primitives: single triangles or meshes  
[ ] Interpolated normals: only required on triangle primitives, 3 different vertex normals must be specified  
[ ] Spot lights: smooth falloff optional  
[ ] Glossy reflections: not only of light sources but of other objects  
[ ] Anti-aliasing  
[ ] Parallelized: using parallel-for, async tasks, threads, or [fill in other method]  
[ ] Textures: on all implemented primitives  
[ ] Bump or normal mapping: on all implemented primitives  
[ ] Environment mapping: sphere or cube map, without intersecting actual sphere/cube/triangle primitives  
[ ] Refraction: also requires a reflected ray at every refractive surface, recursive  
[ ] Area lights: soft shadows  
[ ] Acceleration structure: bounding box or hierarchy, scene with 5000+ primitives  
Note: [provide one measurement of speed/time with and without the acceleration structure]  
[ ] GPU implementation: using a fragment shader, CUDA, OptiX, RTX, DXR, or [fill in other method]  

##### Notes:  
RecursionCap can be modified in the Raytracer class 

Controls: W for forward, S for backward, A for left, D for right, U for decrease FOV and I for Increase FOV

Debug: can be turned on and off by uncommenting at the end of the render function.
