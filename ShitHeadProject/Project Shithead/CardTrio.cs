using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CardTrio
{
    private const int NumOfCard = 3;

    private PlayerHand parentHand;

    private List<Card> cards;

    private bool shown;

    private GameDeck gameDeck;
    private GamePile gamePile;

    public CardTrio(PlayerHand parentHand, GameDeck gameDeck, GamePile gamePile, bool shown) {
        this.parentHand = parentHand;
        this.gameDeck = gameDeck;
        this.gamePile = gamePile;
        this.shown = shown;
        this.cards = new List<Card>();
        if (!shown)
            for (int i = 0; i < NumOfCard; i++)
                cards.Add(gameDeck.StartDrawCard());
        else {
            int count = 0;
            while (count < NumOfCard) {
                parentHand.ViewHand();
                Console.WriteLine("enter the card number {0} you want to put in your deck", count + 1);
                Card tempCard = new Card(Console.ReadLine());
                if (parentHand.HasCard(tempCard, true)) {
                    cards.Add(tempCard);
                    Console.WriteLine("new player hand = " + parentHand);
                    count++;
                }
                else Console.WriteLine("invalid notation. only number of card and shape (in char)");
            }
        }
    }
    public CardTrio(PlayerHand parentHand, GameDeck gameDeck, GamePile gamePile, bool shown, bool IsTesting) {
        this.parentHand = parentHand;
        this.gameDeck = gameDeck;
        this.gamePile = gamePile;
        this.shown = shown;
        this.cards = new List<Card>();
        if(!shown)
        for (int i = 0; i < NumOfCard; i++)
            cards.Add(gameDeck.StartDrawCard());
        else
            for(int i = 0;i < NumOfCard; i++) {
                cards.Add(parentHand.GetHand()[parentHand.CardCount()-1]);
                parentHand.GetHand().RemoveAt(parentHand.CardCount()-1);
            }
    }


    public bool AddCard(Card card) {
        if (!shown) return false;
        cards.Add(card); return true;
    }

    public bool Play(Card card) {
        if (!shown) return Play();
        foreach (Card c in cards)
            if (c.Equals(card)) {
                return gamePile.PilePlay(this, c);
            }
        if (parentHand.Won()) Shithead.BroWon();
        return false;
    }

    public bool Play(int card) {
        if (!shown) return Play();
        foreach (Card c in cards)
            if (c.GetNumber().Equals(card)) {
                return gamePile.PilePlay(this, c);
            }
        if (parentHand.Won()) Shithead.BroWon();
        return false;
    }


    public bool Play() {
        if (shown) return false;
        if (parentHand.Won()) Shithead.BroWon();

        Console.WriteLine("Blind card= " + cards[cards.Count-1]);

        return gamePile.PilePlay(this, cards[cards.Count - 1]);
    }

    public void RemoveCard(Card card) {
        cards.Remove(card);
    }

    public void View() {
        if (shown) {
            Console.WriteLine("state: shown");
            Console.Write("TrioCards: ");
            foreach (Card c in cards) Console.Write(c + ",");
            return;
        }
        Console.WriteLine("state: hidden");
    }

    public bool IsEmpty() { return cards.Count == 0; }

    public bool GetShown() { return shown; }
    public List<Card> GetCards() { return cards; }

    public PlayerHand GetParentHand() { return parentHand; }
}
