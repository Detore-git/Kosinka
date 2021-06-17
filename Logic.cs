namespace KosinkaCore
{
	static class Logic
	{
		//places top card on bottom card if logic is true for given stack type of bottom card stack
		public static bool logic(Card top, Card bottom, StackType type)
		{
			switch (type)
			{
				case StackType.ACES_STACK:
					return (bottom is null && top.Value == 0) || (top.Value > bottom.Value && top.Suit == bottom.Suit);
				case StackType.NORMAL:
					return (bottom is null && top.Value == 12) || (bottom.Value - top.Value == 1 && top.Color != bottom.Color);
				default:
					return false;
			}
		}
	}
}
