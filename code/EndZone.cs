using Sandbox;

[Library( "thop_end_zone", Description = "Timer stops here." )]
[Hammer.Solid]
[Hammer.AutoApplyMaterial( "materials/tools/toolstrigger.vmat" )]
[Hammer.EntityTool( "End Zone", "THOP", "Defines the area for which the players complete the map." )]
public partial class EndZone : BaseTrigger
{
	// draw endzone box overlay
	[Event.Tick]
	private void DrawBoxOverlay()
	{
		DebugOverlay.Box( this, new Color( .2f, 1f, .2f ) );
	}

	public override void StartTouch( Entity other )
	{
		base.EndTouch( other );

		ParkourPlayer player = other as ParkourPlayer;
		if ( player == null ) return;

		player.FinishMap();
	}
}
