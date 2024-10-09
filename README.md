# Mission Demolition Enhancements

## Enhancement: Particle Effects for Impact and Destruction

### Description:
This enhancement adds particle effects that trigger when a projectile impacts a castle or any destructible object. The particles simulate dust and debris, making the destruction sequences more immersive and visually appealing.

### How It Works:
- When the projectile collides with a destructible object (e.g., castle blocks), a particle system is instantiated at the point of impact.
- The particle system emits a burst of dust and debris to simulate the impact and destruction of the castle.

### How to Use:
1. The `Projectile` script is responsible for detecting collisions. When a collision is detected, the script instantiates a particle effect at the collision point.
2. You can adjust the particle settings (size, speed, color) in the `ImpactParticle` prefab located in the **Assets** folder.

### Files Modified:
- `Projectile.cs`: Modified to trigger the particle effect upon collision.
- `ImpactParticle.prefab`: A new prefab that defines the particle system for impact.

### Future Enhancements:
- Add different particle effects for different materials (e.g., stone, wood).
- Implement sound effects to further enhance the destruction experience.
