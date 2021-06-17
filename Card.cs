using System;

namespace KosinkaCore
{
	class Card
	{
		readonly int ID;
		public int Suit => ID % 4;
		public int Color => ID % 2; //1 - black, 0 - red
		public int Value => ID / 4;
		public bool isShown;
		readonly string[] CardStrings = new string[12];

		public Card(int n, bool shown) : this(n)
		{
			isShown = shown;
		}
		public Card(int n)
		{
			ID = n;
			isShown = true;
			Array.Copy(Strings.EmptyCard, CardStrings, Strings.EmptyCard.Length);
			// setting up drawn texture (rewritng CardStrings)
			{
				string typeString;
				char suitString = Strings.Suits[Suit];
				typeString = Value == 9 ? "10" : Strings.CardValues[Value].ToString();
				PlaceNumber(typeString);

				ChangeColor();
				switch (Value)
				{
					case 0:
						PlaceOne(suitString, 6);
						break;
					case 1:
						PlaceOne(suitString, 4);
						PlaceOne(suitString, 8);
						break;
					case 2:
						PlaceOne(suitString, 4);
						PlaceOne(suitString, 6);
						PlaceOne(suitString, 8);
						break;
					case 3:
						PlaceTwo(suitString, 4);
						PlaceTwo(suitString, 8);

						break;
					case 4:
						PlaceTwo(suitString, 4);
						PlaceTwo(suitString, 8);
						PlaceOne(suitString, 6);

						break;
					case 5:
						PlaceTwo(suitString, 4);
						PlaceTwo(suitString, 6);
						PlaceTwo(suitString, 8);


						break;
					case 6:
						PlaceTwo(suitString, 4);
						PlaceTwo(suitString, 6);
						PlaceTwo(suitString, 8);
						PlaceOne(suitString, 5);
						break;
					case 7:
						PlaceTwo(suitString, 4);
						PlaceTwo(suitString, 6);
						PlaceTwo(suitString, 8);
						PlaceOne(suitString, 5);
						PlaceOne(suitString, 7);

						break;
					case 8:

						PlaceOne(suitString, 6);
						PlaceTwo(suitString, 4);
						PlaceTwo(suitString, 5);
						PlaceTwo(suitString, 7);
						PlaceTwo(suitString, 8);

						break;
					case 9:
						PlaceOne(suitString, 5);
						PlaceOne(suitString, 8);
						PlaceTwo(suitString, 4);
						PlaceTwo(suitString, 6);
						PlaceTwo(suitString, 7);
						PlaceTwo(suitString, 9);
						break;
					case 10:
						PlaceJ(suitString);
						break;
					case 11:
						PlaceL(suitString);
						break;
					case 12:
						PlaceK(suitString);
						break;
				} // swith(Value)
				ChangeColor();
			}
		}

		// Display Card's fields and properties
		public override string ToString()
		{
			return "\nSYSTEM CARD INFO\nID: "
				+ ID.ToString()
				+ "\nSuit: "
				+ Suit.ToString()
				+ "\nColor: "
				+ Color.ToString()
				+ "\nValue: "
				+ Value.ToString();
		}
		public string[] GetCardStrings()
		{
			if (isShown)
			{
				return CardStrings;
			}

			return Strings.FlippedCard;
		}
		void PlaceNumber(string type)
		{
			if (type == "10")
			{
				CardStrings[2] = "|  " + type + "       |";
				CardStrings[10] = "|        " + type + " |";
			}
			else
			{
				CardStrings[2] = "|  " + type + "        |";
				CardStrings[10] = "|        " + type + "  |";
			}
		}
		void PlaceOne(char suit, int row)
		{
			CardStrings[row] = "|     " + suit + "     |";
		}
		void PlaceTwo(char suit, int row)
		{
			CardStrings[row] = "|  " + suit + "     " + suit + "  |";
		}
		void PlaceK(char suit)
		{
			CardStrings[6] = "|" + suit + "Король.tmp|";
		}
		void PlaceL(char suit)
		{
			CardStrings[6] = "| " + suit + "Дама.tmp |";
		}
		void PlaceJ(char suit)
		{
			CardStrings[6] = "|" + suit + "Валет.tmp |";
		}
		void ChangeColor()
		{
			if (Color == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
			}
			if (Console.ForegroundColor == ConsoleColor.Red)
			{
				Console.ForegroundColor = ConsoleColor.White;
			}
		}
	}
}
