using Sandbox;

/// <summary>
/// Use this to define the spawn point for the player when failing a checkpoint.
/// Set this name to Checkpoint# then use the Checkpoint Zone to point to this entity.
/// </summary>
[Library("thop_checkpoint_spawn", Description = "Player's spawn here when failing at next checkpoint")]
[Hammer.EntityTool("Checkpoint Spawn", "THOP", "Defines the area for which the players (re)spawn in after failing checkpoint.")]
[Hammer.Solid]
public partial class CheckpointSpawn : Entity
{
}
