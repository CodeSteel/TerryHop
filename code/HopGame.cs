/////////////////////////////////
/// Terry Hop - made by Steel
/// Github: https://github.com/CodeSteel/TerryHop
/// Discord: Steel#1100
/////////////////////////////////

using Sandbox;
using System;
using System.Linq;

public class HopGame : Game
{
	public HopGame()
	{
		if ( IsServer )
		{
			// Create the HUD
			_ = new HopHud();
		}
	}

	public override void ClientJoined( Client cl )
	{
		base.ClientJoined( cl );

		var player = new ParkourPlayer();
		cl.Pawn = player;

		player.Respawn();
	}

	/// <summary>
	/// Restricts noclip
	/// </summary>
	/// <param name="player"></param>
	public override void DoPlayerNoclip( Client player )
	{
		// only me baby :) (and jett because why not)
		if ( player.SteamId == 76561198828038868 )
			base.DoPlayerNoclip( player );

		if ( player.SteamId == 76561198086102176 )
			base.DoPlayerNoclip( player );
	}

	/* Disabled due to not being used anymore by ParkourPlayer
	/// <summary>
	/// Respawns the player
	/// </summary>
	/// <param name="player"></param>
	public static void Reset(ParkourPlayer player)
	{
		if ( player == null ) return;

		player.Respawn();
	}
	*/

	/// <summary>
	/// Called from the player when finished the map
	/// </summary>
	/// <param name="player"></param>
	[ServerCmd("finish_map")]
	public static void FinishMap(string time )
	{
		var caller = ConsoleSystem.Caller;
		if ( caller == null ) return;

		// loop through all players
		foreach ( var ply in Client.All)
		{
			if ( ply == null ) continue;

			// send notification
			var pawn = ply.Pawn as ParkourPlayer;
			pawn.FinishMapNotify( caller.Name, time );
		}
	}
}
