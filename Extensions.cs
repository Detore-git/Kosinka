using System;

namespace KosinkaCore
{
	static class Extensions
	{

		public static void WriteAt(int startX, int startY, string str)
		{
			Console.SetCursorPosition(startX, startY);
			Console.Write(str);
		}
		public static void PlaceCards(this Stack Deck, Stack[][] field)
		{
			int k = 7;
			for (int i = 0; i < 7; i++)
			{
				for (int j = 0; j < k - i; j++)
				{
					Deck.PlaceTo(field[i][1]);
				}
			}
		}
		public static void Draw(this Stack[][] field)
		{
			Console.Clear();
			for (int y = 0; y < 2; y++)
			{
				for (int x = 0; x < 8; x++)
				{
					field[x][y].DrawStack();
				}
			}
		}
	}
}
