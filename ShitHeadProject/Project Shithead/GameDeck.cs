using System;
using System.Collections.Generic;

public class GameDeck
{
    private static Random rnd = new Random();

    public static string[] FullDeck = new string[] {
        "2C", "3C", "4C", "5C", "6C", "7C", "8C", "9C", "10C", "11C", "12C", "13C", "14C",
        "2D", "3D", "4D", "5D", "6D", "7D", "8D", "9D", "10D", "11D", "12D", "13D", "14D",
        "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "10H", "11H", "12H", "13H", "14H",
        "2S", "3S", "4S", "5S", "6S", "7S", "8S", "9S", "10S", "11S", "12S", "13S", "14S",
        "15R", "15B"
    };

    private List<Card> deck = new List<Card>();

    public GameDeck() {
        foreach (string cardStr in FullDeck) {
            deck.Add(new Card(cardStr));
        }
        Shuffle();
    }


    public int DeckCount() {
        return deck.Count;
    }
    public void DrawFromDeck(PlayerHand playerHand) {
        if (deck.Count == 0) return;
        playerHand.AddCard(deck[deck.Count - 1]);
        deck.RemoveAt(deck.Count - 1);
        playerHand.SortHand();
    }

    //should be only at start
    public Card StartDrawCard() {
        Card ret = deck[deck.Count - 1];
        deck.RemoveAt(deck.Count - 1);
        return ret;
    }
    public void Shuffle() {
        int len = deck.Count;
        List<Card> tempDeck = new List<Card>(new Card[len]);
        Card card;
        int index;

        for (int i = 0; i < len; i++) {
            index = rnd.Next(0, len);
            card = deck[index];
            if (!HasCard(tempDeck, card))
                tempDeck[i] = card;
            else i--;
        }

        this.deck = tempDeck;
    }

    public static bool HasCard(List<Card> tempDeck, Card card) {
        foreach (Card c in tempDeck) {
            if (c != null && c.ToString().Equals(card.ToString()))
                return true;
        }

        return false;
    }

    public void PrintDeck() {
        int up = 0;
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < deck.Count / 4; j++)
                Console.Write($"{deck[j + up]}, ");
            Console.WriteLine();
            up += deck.Count / 4;
        }
        for (int i = (deck.Count / 4) * 4; i < deck.Count; i++) {
            Console.Write($"{deck[i]}, ");
        }
    }

    public bool IsEmpty() {
        return this.deck.Count == 0;
    }
    public bool HasCard(Card card) {
        foreach (Card c in deck) {
            if (card.Equals(c)) return true;
        }
        return false;
    }

    public override string ToString() {
        string Retval = "";
        foreach (Card c in deck) {
            Retval += c.ToString() + " ";
        }
        return Retval;
    }
}
