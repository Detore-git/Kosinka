using System;
using System.Collections.Generic;

namespace KosinkaCore
{
	class CardFactory
	{
		private List<Card> Cards;
		public CardFactory()
		{
			Cards = new List<Card>();
		}
		void getNewCards()
		{
			for (int i = 0; i < 52; i++)
			{
				Cards.Add(new Card(i, false));
			}
		}
		public void PlaceCardsTo(Stack stack)
		{
			Card[] cards = new Card[Cards.Count];
			Cards.CopyTo(cards);
			stack.Clear();
			stack.AddRange(cards);
		}
		public void GenerateNewCards()
		{
			Cards.Clear();
			getNewCards();
		}
		public void ShuffleCards()
		{
			var rand = new Random();

			for (int i = 0; i < Cards.Count; i++)
			{
				int k = rand.Next(Cards.Count);
				Card temp = Cards[k];
				Cards[k] = Cards[i];
				Cards[i] = temp;
			}
		}
	}
}
