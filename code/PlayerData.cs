using System;
using System.Collections;
using System.Collections.Generic;
using Sandbox;

/// <summary>
/// This is currently only used for Saving the player's best time on any given map but will be used for more in the future.
/// </summary>
public class PlayerData
{
	public Dictionary<string, double> BestTimes = new Dictionary<string, double>();

	public void SaveBestTime(string mapName, double time)
	{
		if (BestTimes.ContainsKey(mapName))
			BestTimes.Remove( mapName );

		BestTimes.Add( mapName, time );

		Save( this );
	}

	public bool GetBestTime(string mapName, out double time)
	{
		if ( BestTimes.ContainsKey( mapName ) )
		{
			time = BestTimes[mapName];
			return true;
		}

		time = 0;
		return false;
	}

	public static void Save(PlayerData data)
	{
		try
		{
			FileSystem.Data.WriteJson( "player_data.txt", data );
		} catch (Exception e)
		{
			Log.Error( "Saving PlayerData Exception: " + e );	
		}
	}

	public static PlayerData Load()
	{
		try
		{
			if ( FileSystem.Data.FileExists( "player_data.txt" ) )
				return FileSystem.Data.ReadJson<PlayerData>( "player_data.txt" );
			else
				return new PlayerData();
		} catch (Exception e)
		{
			Log.Error( "Loading PlayerData Exception: " + e);
			return new PlayerData();
		}
	}
}
