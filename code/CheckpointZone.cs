using Sandbox;

/// <summary>
/// If a player touches this brush, it will teleport the player to the TargetSpawn entity.
/// </summary>
[Library("thop_checkpoint_zone", Description = "Player's fail here.")]
[Hammer.Solid]
[Hammer.AutoApplyMaterial( "materials/tools/toolstrigger.vmat" )]
[Hammer.EntityTool("Checkpoint Zone", "THOP", "Defines the area for which players will fail.")]
public partial class CheckpointZone : BaseTrigger
{
	[Property(Title = "Checkpoint Spawn")]
	public string TargetSpawn { get; set; }

	[Property( Title = "Reset Velocity" )]
	public bool ResetVelocity { get; set; }

	public override void StartTouch( Entity other )
	{
		base.EndTouch( other );

		ParkourPlayer player = other as ParkourPlayer;
		if ( player == null ) return;

		// reset the velocity
		if ( ResetVelocity )
			player.Controller.Velocity = Vector3.Zero;

		// CheckpintStart is the entry checkpoint for any THOP map
		if (TargetSpawn == "CheckpointStart" )
		{
			player.Respawn();
			return;
		}

		// normal checkpoint? Just set the position.
		player.Position = Entity.FindByName(TargetSpawn).Position;
	}
}
