using System;
using System.Linq;
using System.Text;

namespace KosinkaCore
{
	class Program
	{
		static void Main()
		{
			int ConsoleWindowWidth = GetPreferredWindowWidth(107);
			int ConsoleWindowHeight = GetPreferredWindowHeight(63);
			Console.ForegroundColor = ConsoleColor.White;
			Console.SetWindowSize(ConsoleWindowWidth, ConsoleWindowHeight);
			Console.OutputEncoding = Encoding.Unicode;
			Console.CursorVisible = false;


			int cursorX = 0;
			int cursorY = 0;
			int cardStackCount = 0;
			bool SelectMode = true;
			bool won = false;

			Stack[][] field = new Stack[8][];
			InitField();


			Stack stackToTake = null;
			Stack Deck = field[0][0];
			Stack NearDeck = field[1][0];
			Stack BufferStack = field[7][0];

			Stack SelectedStack = null;
			bool hasCards = false;
			int CardCount = 0;
			var type = StackType.NOTHING;

			ConsoleKeyInfo key = new ConsoleKeyInfo();
			WinConditionChecker.WinEventHandler += OnWin;

			CardFactory cardFactory = new CardFactory();
			ResetField();


			while (key.Key != ConsoleKey.Q && !won)
			{

				SelectedStack = field[cursorX][cursorY];
				hasCards = SelectedStack.HasCards;
				CardCount = SelectedStack.Count;
				type = SelectedStack.stackType;

				ScreenUpdate();
				switch (key.Key)
				{
					case ConsoleKey.LeftArrow:
						MoveLeft();
						break;
					case ConsoleKey.RightArrow:
						MoveRight();
						break;
					case ConsoleKey.UpArrow:
						MoveUp();
						break;
					case ConsoleKey.DownArrow:
						MoveDown();
						break;
					case ConsoleKey.Enter:
						if (SelectMode)
						{
							if (hasCards)
							{
								if (type == StackType.DECK)
								{
									SelectedStack.PlaceTo(NearDeck);
								}
								else
								{
									MoveSelectionToBufferAndSwitchMode(cardStackCount);
								}
							}
							else if (type == StackType.DECK)
							{
								while (NearDeck.HasCards)
								{
									NearDeck.PlaceTo(Deck);
								}
							}
						}
						else if (type != StackType.DECK)
						{
							ManageSelected();
						}

						break;
					case ConsoleKey.R:
						ResetField();
						break;
				}

			}

			void ScreenUpdate()
			{
				if (hasCards)
				{
					SelectedStack.SetSelected(cardStackCount + 1);
				}
				else
				{
					SelectedStack.SetSelected(1);
				}

				field.Draw();
				SelectedStack.SetSelected(0);

				key = Console.ReadKey(true);
			}
			void MoveLeft()
			{
				cursorX--;
				if (cursorX < 0)
				{
					cursorX = 6;
				}

				if (cursorX == 2 && cursorY == 0)
				{
					cursorX = 1;
				}

				cardStackCount = 0;
			}
			void MoveRight()
			{
				cursorX++;
				if (cursorX > 6)
				{
					cursorX = 0;
				}

				if (cursorX == 2 && cursorY == 0)
				{
					cursorX = 3;
				}

				cardStackCount = 0;
			}
			void MoveUp()
			{
				// if we have previous card in the stack opened, extend selection to it
				if (cursorY > 0 &&
					cardStackCount != CardCount - 1 &&
					SelectedStack.Count > 1 && hasCards &&
					SelectedStack[CardCount - cardStackCount - 2]
						.isShown == true)
				{
					cardStackCount++;
				}
				// else determine new cursor's position
				else
				{
					cardStackCount = 0;
					if (cursorX != 2)
					{
						if (cursorY == 1)
						{
							cursorY--;
						}
						else
						{
							cursorY++;
						}
					}
				}
			}
			void MoveDown()
			{
				//if we are not at the bottom of stack, move down the stack
				if (cardStackCount > 0)
				{
					cardStackCount--;
				}
				// else determine new cursor's position
				else
				{
					cardStackCount = 0;
					if (cursorX != 2)
					{
						if (cursorY == 1)
						{
							cursorY--;
						}
						else
						{
							cursorY++;
						}
					}
				}
			}
			void MoveSelectionToBufferAndSwitchMode(int count)
			{
				SelectMode = false;
				SelectedStack.Copy(BufferStack, count + 1);
				stackToTake = SelectedStack;
				stackToTake.SetQueried(count + 1);
				cardStackCount = 0;
			}
			void MoveFromOneToOtherWithBuffer()
			{
				var LocalCount = BufferStack.Count;
				for (int i = 0; i < LocalCount; i++)
				{
					Card card = BufferStack[0];
					stackToTake.Remove(card);
					SelectedStack.Add(card);
					BufferStack.Remove(card);
				}
			}
			void ManageSelected()
			{
				SelectMode = true;
				if (Logic.logic(BufferStack[0], hasCards ? SelectedStack.Last() : null, type))
				{
					MoveFromOneToOtherWithBuffer();
				}

				stackToTake.SetQueried(0);
				stackToTake = null;
				BufferStack.Clear();
			}
			void InitField()
			{
				for (int x = 0; x < 8; x++)
				{
					field[x] = new Stack[2];
					for (int y = 0; y < 2; y++)
					{
						field[x][y] = new Stack(x, y);
					}
				}
			}
			void ResetField()
			{
				for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 2; j++)
					{
						field[i][j].Clear();
					}
				}

				cardFactory.GenerateNewCards();
				cardFactory.ShuffleCards();
				cardFactory.PlaceCardsTo(Deck);
				Deck.PlaceCards(field);
			}
			int GetPreferredWindowWidth(int preference) => Console.LargestWindowWidth > preference ? preference : Console.LargestWindowWidth;
			int GetPreferredWindowHeight(int preference) => Console.LargestWindowHeight > preference ? preference : Console.LargestWindowHeight;
			void OnWin(object sender, EventArgs e)
			{
				Console.Clear();
				Console.SetWindowSize(GetPreferredWindowWidth(82), GetPreferredWindowHeight(29));
				Console.WriteLine(Strings.Win);
				won = true;
				Console.ReadLine();
			}
		}
	}
}
