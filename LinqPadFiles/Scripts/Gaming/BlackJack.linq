<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
</Query>


public static class Program
{
	static void Main(params string[]? args)
	{
		var run = true;
		while (run)
		{
			Console.Clear();
			var game = new BlackjackGame();
			game.Play();

			run = Continue();
		}
	}

	static bool Continue()
	{
		while (true)
		{
			Console.WriteLine("Game over! Hit ENTER to play again or ESCAPE to quit...\n");
			var key = Console.ReadKey();

			if (key.Key == ConsoleKey.Escape) return false;
			if (key.Key == ConsoleKey.Enter) return true;
		}
	}
}


public class BlackjackGame
{
	private Stack<Card> Deck = new();
	private List<Card> playerCards;
	private List<Card> dealerCards;

	public BlackjackGame()
	{
		CreateDeck();
		playerCards = new List<Card>();
		dealerCards = new List<Card>();
	}

	public void CreateDeck()
	{
		List<Card> deck = new List<Card>();

		// Loop through each value and suit, creating a new card for each combination
		// and adding it to the deck
		foreach (var suit in System.Enum.GetValues<Suit>())
		{
			foreach (var rank in Enum.GetValues<Rank>())
			{
				deck.Add(new Card(suit, rank));
			}
		}

		// Return the completed deck
		var shuffledDeck = deck;

		for (int i = 0; i < 100; i++)
		{
			ShuffleDeck(shuffledDeck);
		}

		shuffledDeck.ForEach(x => Deck.Push(x));
	}

	public void ShuffleDeck(List<Card> deck)
	{
		// Use the Fisher-Yates shuffle algorithm to shuffle the deck
		Random rng = new Random();
		int n = deck.Count;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			Card card = deck[k];
			deck[k] = deck[n];
			deck[n] = card;
		}
	}

	public void Deal()
	{
		dealerCards.Add(DrawCard());
		playerCards.Add(DrawCard());
		dealerCards.Add(DrawCard());
		playerCards.Add(DrawCard());
	}

	public Card DrawCard()
	{
		if (Deck.TryPop(out var result))
		{
			return result;
		}

		CreateDeck();

		return DrawCard();
	}

	public int GetCardValue(Card card)
	{
		// Check if the card is an Ace and return 11 if it is
		if (card.Rank == Rank.Ace)
		{
			return 11;
		}

		// Check if the card is a face card (Jack, Queen, or King) and return 10 if it is
		else if (card.Rank == Rank.Jack || card.Rank == Rank.Queen || card.Rank == Rank.King)
		{
			return 10;
		}

		// Otherwise, return the numeric value of the card
		else
		{
			return (int)card.Rank;
		}
	}

	public int GetTotalValue(List<Card> cards)
	{
		// Initialize the total value to 0
		int totalValue = 0;

		// Loop through each card and add its value to the total
		foreach (Card card in cards)
		{
			totalValue += GetCardValue(card);
		}

		// Check for Aces and adjust the total value if necessary
		foreach (Card card in cards)
		{
			if (card.Rank == Rank.Ace && totalValue > 21)
			{
				totalValue -= 10;
			}
		}

		// Return the total value
		return totalValue;
	}


	public bool IsBlackjack(List<Card> cards)
	{
		// Check if the player has two cards and if their total value is 21
		if (cards.Count == 2 && GetTotalValue(cards) == 21)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void Play()
	{
		// Deal the initial cards
		Deal();

		// Get initial totals
		int playerTotal = GetTotalValue(playerCards);
		int dealerTotal = GetTotalValue(dealerCards);

		// Check for Blackjack
		if (IsBlackjack(playerCards))
		{
			Console.WriteLine("Player Blackjack! The game is over.");
			return;
		}

		// Check for Blackjack
		if (IsBlackjack(dealerCards))
		{
			Console.WriteLine("Dealer Blackjack! The game is over.");
			return;
		}

		// Allow the player to hit or stand
		bool playerTurn = true;
		while (playerTurn)
		{
			Console.WriteLine("Your cards are:");
			foreach (Card card in playerCards)
			{
				Console.WriteLine($"{card.Rank} of {card.Suit}");
			}
			Console.WriteLine("Your total is: {0}", GetTotalValue(playerCards));
			Console.WriteLine("Do you want to (H)it or (S)tand?");
			ConsoleKey key = Console.ReadKey().Key;
			Console.WriteLine();

			if (key == ConsoleKey.S)
			{
				playerTurn = false;
			}
			else if (key == ConsoleKey.H)
			{
				Card newCard = DrawCard();
				playerCards.Add(newCard);
				playerTotal = GetTotalValue(playerCards);
				if (playerTotal > 21)
				{
					Console.WriteLine("You bust!");
					return;
				}
			}
		}

		// Dealer's turn
		Console.WriteLine("Dealer's cards are:");
		foreach (Card card in dealerCards)
		{
			Console.WriteLine($"{card.Rank} of {card.Suit}");
		}
		while (true)
		{
			dealerTotal = GetTotalValue(dealerCards);
			if (dealerTotal >= 17)
			{
				break;
			}
			Card newCard = DrawCard();
			dealerCards.Add(newCard);
			if (GetTotalValue(dealerCards) > 21)
			{
				Console.WriteLine("Dealer busts, you win!");
				return;
			}
		}

		// Determine the winner
		playerTotal = GetTotalValue(playerCards);
		dealerTotal = GetTotalValue(dealerCards);
		if (playerTotal > dealerTotal || dealerTotal > 21)
		{
			Console.WriteLine("You win!");
		}
		else
		{
			Console.WriteLine("Dealer wins.");
		}
	}
}

public enum Suit
{
	Clubs,
	Diamonds,
	Hearts,
	Spades
}

public enum Rank
{
	Ace = 1,
	Two = 2,
	Three = 3,
	Four = 4,
	Five = 5,
	Six = 6,
	Seven = 7,
	Eight = 8,
	Nine = 9,
	Ten = 10,
	Jack = 11,
	Queen = 12,
	King = 13
}

public record struct Card(Suit Suit, Rank Rank);
