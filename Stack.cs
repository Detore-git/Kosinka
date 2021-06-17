using System;
using System.Collections.Generic;
using System.Linq;
using static KosinkaCore.Extensions;

namespace KosinkaCore
{
	class Stack : List<Card>
	{
		public bool HasCards => Count > 0;
		public int x;
		public int y;
		public bool isSelected;
		public StackType stackType;
		private List<string> StackOutputBuffer = new List<string>();
		public int selectedCount = 0;
		int queriedToTake = 0;
		bool isQueried = false;
		public event EventHandler StateChangedEventHandler;

		public Stack(int x, int y)
		{
			this.x = x;
			this.y = y;
			stackType = (x, y) switch
			{
				var (a, b) when a == 0 && b == 0 => StackType.DECK,
				var (a, b) when a == 1 && b == 0 => StackType.NEAR_DECK,
				var (a, b) when a > 2 && a < 7 && b == 0 => StackType.ACES_STACK,
				var (a, b) when (b > 0 && a < 7) || (b == 0 && a == 7) => StackType.NORMAL,
				(_, _) => StackType.NOTHING
			};
			if (stackType == StackType.ACES_STACK)
			{
				WinConditionChecker.Register(this);
			}
		}
		public void SetSelected(int count)
		{
			selectedCount = count;
			isSelected = count != 0;
		}
		public void SetQueried(int count)
		{
			queriedToTake = count;
			isQueried = count > 0;
		}
		public new void Add(Card card)
		{
			base.Add(card);
			if (stackType == StackType.ACES_STACK && card.Value == 12)
			{
				StateChanged();
			}
		}
		public new void Remove(Card card)
		{
			base.Remove(card);
			if (stackType == StackType.ACES_STACK && card.Value == 12)
			{
				StateChanged();
			}
		}
		public void PlaceTo(Stack target)
		{
			Card temp = this.Last();
			Remove(temp);
			target.Add(temp);
		}
		public void Copy(Stack target, int range)
		{
			target.AddRange(ToArray()[^range..^0]);
		}
		protected virtual void StateChanged()
		{
			EventHandler handler = StateChangedEventHandler;
			handler?.Invoke(this, EventArgs.Empty);
		}

		//updates drawing buffer
		public void UpdateStringBuffer()
		{
			if (stackType == StackType.NOTHING)
			{
				return;
			}

			StackOutputBuffer.Clear();
			if (HasCards)
			{
				this.Last().isShown = true;

				switch (stackType)
				{
					case StackType.ACES_STACK:
						StackOutputBuffer.AddRange(this.Last().GetCardStrings());
						break;
					case StackType.DECK:
						StackOutputBuffer.AddRange(Strings.FlippedCard);
						break;
					case StackType.NEAR_DECK:
						StackOutputBuffer.AddRange(this.Last().GetCardStrings());
						break;
					//case StackType.BUFFER:
					//	break;
					default:
						for (int x = 0; x < Count; x++)
						{
							string[] cardStringsProcessed = this[x].GetCardStrings();
							for (int row = 0; row < Strings.CARD_HEIGHT; row++)
							{
								int nextIndex = row + x * 2;
								if (nextIndex > StackOutputBuffer.Count - 1)
								{
									StackOutputBuffer.Add(cardStringsProcessed[row]);
								}
								else
								{
									StackOutputBuffer[nextIndex] = cardStringsProcessed[row];
								}
							}
						}
						break;
				}
			}
			else
			{
				if (stackType == StackType.DECK)
				{
					StackOutputBuffer.AddRange(Strings.EmptyDeck);
				}
				else
				{
					StackOutputBuffer.AddRange(Strings.EmptyCard);
				}
			}
		}
		public void DrawStack()
		{
			if (stackType == StackType.NOTHING)
			{
				return;
			}

			UpdateStringBuffer();
			int drawnX = x * Strings.CARD_WIDTH;
			drawnX += x > 0 ? 1 : 0;
			for (int i = 0; i < StackOutputBuffer.Count; i++)
			{
				if (isSelected && i >= StackOutputBuffer.Count - Strings.CARD_HEIGHT - 2 * (selectedCount - 1))
				{
					Console.BackgroundColor = ConsoleColor.Gray;
				}

				if (isQueried && i >= StackOutputBuffer.Count - Strings.CARD_HEIGHT - 2 * (queriedToTake - 1))
				{
					Console.BackgroundColor = ConsoleColor.Red;
				}

				int drawnY = y * Strings.CARD_WIDTH + i;
				WriteAt(drawnX, drawnY, StackOutputBuffer[i]);
				Console.BackgroundColor = ConsoleColor.Black;
			}

		}
	}


}
