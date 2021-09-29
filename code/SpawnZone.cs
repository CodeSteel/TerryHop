using Sandbox;

[Library("thop_spawn_zone", Description = "Player's spawn here, timer check.")]
[Hammer.Solid]
[Hammer.EntityTool("Spawn Zone", "THOP", "Defines the area for which the players (re)spawn in.")]
public partial class SpawnZone : BaseTrigger
{
	[Property( Title = "Velocity Limit" )]
	public int VeloLimit { get; set; } = 1;

	[Event.Tick]
	private void DrawBoxOverlay()
	{
		DebugOverlay.Box( this, new Color( 1, .2f, .2f ) );
	}

	public override void EndTouch( Entity other )
	{
		base.EndTouch( other );
			
		ParkourPlayer player = other as ParkourPlayer;
		if ( player == null ) return;

		player.StartTimer();
		player.PlaySound( "leavespawn" );

		var controller = player.Controller as HopWalkController;
		controller.IsInSpawn = false;
	}

	public override void StartTouch( Entity other )
	{
		base.EndTouch( other );

		ParkourPlayer player = other as ParkourPlayer;
		if ( player == null ) return;

		player.ResetTimer();

		var controller = player.Controller as HopWalkController;
		controller.IsInSpawn = true;
	}
}
