using Sandbox;
using Sandbox.UI;
using System.Diagnostics;

public partial class ParkourPlayer : Player
{
	private ICamera MainCamera;
	[Net, Predicted] public Vector3 checkpointPosition { get; set; }

	public Clothing.Container Clothing = new();
	public Stopwatch timer = new();

	// saving data such as best time on map
	public PlayerData playerData;

	public ParkourPlayer()
	{
		playerData = PlayerData.Load();
	}

	public ParkourPlayer( Client cl ) : this()
	{
		// Load clothing from client data
		Clothing.LoadFromClient( cl );
	}

	public override void Respawn()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		Controller = new HopWalkController();

		Animator = new StandardPlayerAnimator();
		Camera = new FirstPersonCamera();
		MainCamera = Camera;
		Clothing.DressEntity( this );

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		ResetTimer();

		base.Respawn();
	}

	// overriding to disable player2player collisions
	public override void CreateHull()
	{
		SetupPhysicsFromAABB( PhysicsMotionType.Keyframed, new Vector3( -16, -16, 0 ), new Vector3( 16, 16, 72 ) );
		SetInteractsAs( CollisionLayer.Player );
		MoveType = MoveType.MOVETYPE_WALK;
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( IsServer )
		{
			// set camera view
			if ( Input.Pressed( InputButton.View ) )
			{
				if ( MainCamera is not FirstPersonCamera )
				{
					MainCamera = new FirstPersonCamera();
				} else
				{
					MainCamera = new HopThirdPersonCamera();
				}
				Camera = MainCamera;
			}

			// reset time/pos
			if ( Input.Pressed( InputButton.Reload ) )
			{
				// ParkourGame.Reset( this ); // replaced this with Respawn()
				Respawn();
				RestartSound();
			}
		}
	}

	/// <summary>
	/// Resets the stopwatch timer
	/// </summary>
	[ClientRpc]
	public void ResetTimer()
	{
		timer.Stop();
		timer.Reset();
	}

	/// <summary>
	/// Starts the stopwatch timer
	/// </summary>
	[ClientRpc]
	public void StartTimer()
	{
		timer.Start();
	}

	/// <summary>
	/// Plays the restart sound
	/// </summary>
	[ClientRpc]
	public void RestartSound()
	{
		PlaySound( "reset" );
	}

	/// <summary>
	/// Ran when the client finishes the map.
	/// Plays some effects, saves the time (if it is the best), and notifies other users your time.
	/// </summary>
	[ClientRpc]
	public void FinishMap()
	{
		if ( !timer.IsRunning ) return;

		timer.Stop();

		// particle effect
		var finishParticle = Particles.Create( "particles/physgun_freeze.vpcf" );
		finishParticle.SetPosition( 0, this.Position );
		finishParticle.Destroy( false );

		// sound effect
		PlaySound( "finish" );


		// tells the server this player has finished the map
		// the server will then loop through all players and call FinishMapNotify to add a chat entry.

		string time = string.Format( "{0:00}:{1:00}.{2:00}", timer.Elapsed.Minutes, timer.Elapsed.Seconds, timer.Elapsed.Milliseconds );
		//ChatBox.AddChatEntry( "THOP", "You have completed the map with a time of " + time );
		//ParkourGame.FinishMap( GetClientOwner(), time );
		ConsoleSystem.Run( "finish_map", time );

		double mapTime;
		if (playerData.GetBestTime(Global.MapName, out mapTime))
		{
			Log.Info( "MapTime:" + mapTime + "\nOurTime: " + timer.Elapsed.TotalMilliseconds );
			// new best time
			if (mapTime > timer.Elapsed.TotalMilliseconds )
			{
				SaveBestTime();
			}
		} else
		{
			SaveBestTime();
		}
	}

	/// <summary>
	/// Uses PlayerData to save the time.
	/// </summary>
	private void SaveBestTime()
	{
		double mapTime;
		playerData.GetBestTime( Global.MapName, out mapTime );

		mapTime /= 1000;

		if (mapTime != 0f)
			ChatBox.AddChatEntry( "THOP", "You have beaten your previous best time of " + mapTime.ToString("N3") + " seconds!" );

		playerData.SaveBestTime( Global.MapName, timer.Elapsed.TotalMilliseconds );
	}

	/// <summary>
	/// This is used by the server to send a notification to all connected clients on map finish.
	/// </summary>
	/// <param name="name"></param>
	/// <param name="time"></param>
	[ClientRpc]
	public void FinishMapNotify(string name, string time)
	{
		ChatBox.AddChatEntry( "THOP", name + " has completed the map with a time of " + time );
	}

	// Disable take damage.
	public override void TakeDamage( DamageInfo info )
	{
		return;
	}
}
