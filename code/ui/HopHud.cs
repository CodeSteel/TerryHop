using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class HopHud : HudEntity<RootPanel>
{
	public HopHud()
	{
		if ( !IsClient ) return;

		RootPanel.StyleSheet.Load( "/ui/Vitals.scss" );

		RootPanel.AddChild<NameTags>();
		RootPanel.AddChild<CrosshairCanvas>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<VoiceList>();
		RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();

		RootPanel.AddChild<Vitals>();
	}
}
