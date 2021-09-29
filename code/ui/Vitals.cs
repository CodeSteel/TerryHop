using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class Vitals : Panel
{
	public Label VelocityLabel;
	public Label TimerLabel;

	public Vitals()
	{
		// timer
		var timerLabelBackground = Add.Panel( "timerLabelBackground" );
		var timerBackground = timerLabelBackground.Add.Panel( "timerBackground" );
		TimerLabel = timerBackground.Add.Label( "0.0", "timer" );
		timerLabelBackground.Add.Label( "Time", "timerLabel" );

		// velocity
		var velocityLabelBackground = Add.Panel( "velocityLabelBackground" );
		var velocityBackground = velocityLabelBackground.Add.Panel( "velocityBackground" );
		VelocityLabel = velocityBackground.Add.Label( "0", "velocity" );
		velocityLabelBackground.Add.Label( "Velocity", "velocityLabel" );

		Add.Label( "Press R to Restart", "restartLabel" );
	}

	public override void Tick()
	{
		var player = Local.Pawn as ParkourPlayer;
		if ( player == null ) return;

		// Move speed
		Vector3 velo = player.Controller.Velocity;
		int speed = velo.Length.CeilToInt();
		
		VelocityLabel.Text = $"{speed}";

		// Timer
		//string time = player.timer.Elapsed.TotalSeconds.ToString( "F2" );
		string time = string.Format( "{0:00}:{1:00}.{2:00}", player.timer.Elapsed.Minutes, player.timer.Elapsed.Seconds, player.timer.Elapsed.Milliseconds );
		TimerLabel.Text = $"{time}";
	}
}
