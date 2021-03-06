namespace SmallScript.DesktopUI.Details.Theming
{
	internal sealed class DarkGreenTheme : ThemeBase
	{
		private const string Background  = "#2E2C2F";
		private const string Controls    = "#475B63";
		private const string ControlsMid = "#729B79";
		private const string Border      = "#BACDB0";
		private const string Foreground  = "#F3E8EE";

		public static DarkGreenTheme Instance { get; } = new DarkGreenTheme();
		
		private DarkGreenTheme() : base(Background, Controls, ControlsMid, Foreground, Border)
		{
		}
	}
}