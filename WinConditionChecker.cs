using System;
using System.Linq;

namespace KosinkaCore
{
	static class WinConditionChecker
	{
		private static bool[] acesStates = new bool[4];
		public static event EventHandler WinEventHandler;
		static void StateChanged(object sender, EventArgs e)
		{
			Stack stack = (Stack)sender;
			acesStates[stack.x - 3] = stack.Last().Value == 12;
			if (acesStates[0] && acesStates[1] && acesStates[2] && acesStates[3])
			{
				Win();
			}
		}
		public static void Win()
		{
			EventHandler handler = WinEventHandler;
			handler?.Invoke(new object(), EventArgs.Empty);
		}
		public static void Register(Stack stack)
		{
			stack.StateChangedEventHandler += StateChanged;
		}
	}
}
