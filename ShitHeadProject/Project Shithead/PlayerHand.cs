using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

public class PlayerHand
{
    private Random random;
    private const int NUMBER_OF_CARDS_AT_START = 6;
    private const int NUMBER_OF_CARDS_TO_COMPLETE = 3;

    private List<Card> hand = new List<Card>();

    private GameDeck gamedeck;

    private GamePile gamePile;

    private CardTrio shownTrio;
    private CardTrio hiddenTrio;

    /// <summary>
    /// Do this at a beninging of a game
    /// </summary>
    /// <param name="gamedeck"></param>
    public PlayerHand(GameDeck gamedeck, GamePile gamePile) {
        this.gamedeck = gamedeck;
        for (int i = 0; i < NUMBER_OF_CARDS_AT_START; i++)
            gamedeck.DrawFromDeck(this);
        this.gamePile = gamePile;
        this.hiddenTrio = new CardTrio(this, gamedeck, gamePile, false);
        this.shownTrio = new CardTrio(this, gamedeck,gamePile,true);
    }
    public PlayerHand(GameDeck gamedeck, GamePile gamePile,bool IsTesting) {
        this.gamedeck = gamedeck;
        for (int i = 0; i < NUMBER_OF_CARDS_AT_START; i++)
            gamedeck.DrawFromDeck(this);
        this.gamePile = gamePile;
        this.hiddenTrio = new CardTrio(this, gamedeck, gamePile, false,true);
        this.shownTrio = new CardTrio(this, gamedeck, gamePile, true,true);
    }

    public void AddCard(Card card) {
        hand.Add(card);
        SortHand();
    }

    public void SortHand() {
        Card temp;
        for (int i = hand.Count - 1; i >= 0; i--) {
            for (int j = hand.Count - 1; j >= 0; j--) {
                if (hand[i].GetNumber() > hand[j].GetNumber()) {
                    temp = hand[i];
                    hand[i] = hand[j];
                    hand[j] = temp;
                }
            }
        }

    }

    public List<Card> GetHand() { return hand; }

    public void DrawCard() {
        gamedeck.DrawFromDeck(this);
    }

    public bool IsEmpty() {
        return hand.Count == 0;
    }

    public bool Won() { return IsEmpty() && shownTrio.IsEmpty() && hiddenTrio.IsEmpty(); }

    public void RemoveCard(Card card) {
        hand.Remove(card);
    }

    

    public void ShuffleHand() {
        Random rnd = new Random();
        int len = hand.Count;
        List<Card> tempDeck = new List<Card>(new Card[len]);
        Card card;
        int index;

        for (int i = 0; i < len; i++) {
            index = rnd.Next(0, len);
            card = hand[index];
            if (!GameDeck.HasCard(tempDeck, card))
                tempDeck[i] = card;
            else i--;
        }

        this.hand = tempDeck;
    }



    #region Play
    public bool Play(Card card) {
        if(IsEmpty()) {
            if (!shownTrio.IsEmpty()) return shownTrio.Play(card);
            if (!hiddenTrio.IsEmpty()) return hiddenTrio.Play();
        }
        foreach (Card c in this.hand) {
            if (c.Equals(card)) {
                gamePile.PilePlay(this, c);
                return true;
            }
        }
        if (Won()) Shithead.BroWon();
        return false;
    }

    public bool Play(string card) {
       
        card = card.ToUpper();
        int TempNum;
        if (int.TryParse(card, out TempNum) && gamePile.ValidCard(TempNum)) return Play(TempNum);

        if (!Card.IsValidCard(card)) return false;
        if (IsEmpty()) {
            if (!shownTrio.IsEmpty()) return shownTrio.Play(new Card(card));
            if (!hiddenTrio.IsEmpty()) return hiddenTrio.Play();
        }

        foreach (Card c in this.hand) {
            if (c.Equals(card)) {
                return gamePile.PilePlay(this, c);
            }
        }
        if (Won()) Shithead.BroWon();
        return false;
    }

    public bool Play(int numberToPlay) {

        if(numberToPlay < 0 && numberToPlay > 14) return false;

        if (!gamePile.ValidCard(numberToPlay)) { TakeAll(); return false; }

        bool retval = false;
        List<Card> tempcards = new List<Card>();
        foreach (Card c in this.hand) {
            if (numberToPlay == c.GetNumber()) {
                retval = true;
                tempcards.Add(c);
            }
        }
        foreach (Card c in tempcards) {
            gamePile.PilePlay(this, c);
        }
        if (Won()) Shithead.BroWon();
        if (!retval) TakeAll();
        return retval;
    }

    public string GetStrongestCard() {
        return this.hand[hand.Count - 1].ToString();
    }



    public string GetRandomtCard() {
        return this.hand[random.Next(0,CardCount())].ToString();
    }


    public int CardCount() {
        return this.hand.Count;
    }

    #endregion

    public bool HasCard(string card) {
        foreach(Card c in this.hand)
            if(card.Equals(card)) return true;
        return false;
    }

    public bool HasCard(Card card) {
        foreach (Card c in this.hand)
            if (card.Equals(card)) return true;
        return false;
    }

    public int CardPlace(Card card) {
        for(int i = 0; i < this.hand.Count; i++)
            if(this.hand[i].Equals(card)) return i;
        return -1;
    }

    public bool HasCard(Card card, bool RemoveCard) {
        if(!RemoveCard) return HasCard(card);
        int place = CardPlace(card);
        if (place != -1) {this.hand.RemoveAt(place); return true; }
        return false;
    }

    public override string ToString() {
        string retVal = "";
        foreach (Card card in hand) {
            retVal += card.ToString() + ", ";
        }
        return retVal;
    }



    //empty - 2
    //on shown trio - 1
    //full - 0
    //won - 3
    public int GetState() {
        if (!IsEmpty()) return 0;
        if(!shownTrio.IsEmpty()) return 1;
        if(!hiddenTrio.IsEmpty()) return 2;
        return 3;
    }

    public void Show() {
        switch (GetState()) {
        case 0:
            Console.WriteLine("full hand,");
            ViewHand();
            return;
        case 1:
            shownTrio.View();
            return;
        case 2:
            hiddenTrio.View();
            return;
        }
        Console.WriteLine("It seems I have made a mistake");
    }

    public void ViewHand() {
        Console.Write("player hand: ");
        foreach (Card card in hand) {
            Console.Write(card.ToString() + ", ");
        }
        Console.WriteLine();
    }


    public void CheckTake() {
        while (hand.Count < NUMBER_OF_CARDS_TO_COMPLETE)
            DrawCard();
    }

    public void TakeAll() {
        gamePile.TakePile(this);
    }
}
