using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class GamePile
{
    private List<Card> cardpile;



    public GamePile() {
        cardpile = new List<Card>();
    }


    public bool PilePlay(PlayerHand hand, Card card) {
        hand.SetLastCard(card);
        if (ValidCard(card)) {
            PutCard(hand, card);
            hand.CheckTake();
            return true;
        }
        TakePile(hand);
        return false;
    }

    public bool PilePlay(CardTrio trio, Card card) {
        trio.GetParentHand().SetLastCard(card);
        if (trio.GetShown()) {
            if (ValidCard(card)) {
                PutCard(trio, card);
                return true;
            }
            TakePile(trio.GetParentHand());
            return false;
        }
        else {
            bool IsValidBefore = true;
            if (!ValidCard(card)) { IsValidBefore = false; }
            PutCard(trio, card);
            if (!IsValidBefore) TakePile(trio.GetParentHand());
            return IsValidBefore;
        }
    }

    public bool ValidCard(Card card) {
        return ValidateCardFull(card.GetNumber());
    }

    public bool ValidCard(int numofCard) {
        return ValidateCardFull(numofCard);
    }

    public bool ValidCard(string card) {
        return ValidCard(new Card(card));
    }

    public int GetLastCard() {
        if(IsEmpty()) return 0;
        return cardpile[cardpile.Count - 1].GetNumber();
    }

    public bool ValidateCardFull(int card) {
        if (cardpile.Count == 0) return true;
        int last = cardpile[cardpile.Count - 1].GetNumber();

        for (int i = cardpile.Count - 1; i >= 0; i--)
            if (cardpile[i].GetNumber() != 3) {
                last = cardpile[i].GetNumber();
                break;
            }



        switch (card) {
        case 2:
        case 3:
        case 10:
        case 15:
            return true;

        default:
            if (last == 7) return card <= last;
            return card >= last;
        }
    }



    private void PutCard(PlayerHand playerhand, Card card) {
        Console.WriteLine("put card");
        cardpile.Add(card);
        switch (card.GetNumber()) {
        case 8:
            Shithead.SkipTurn();
            break;
        case 9:
            Shithead.ChangeDirection();
            break;
        case 10:
            Burn();
            break;
        case 15:
            Console.WriteLine("reached joker");
            cardpile.RemoveAt(cardpile.Count - 1);
            Shithead.Joker();
            break;
        }
    
        playerhand.RemoveCard(card);

        CheckIfBurn();

    }

    private void PutCard(CardTrio playerTrio, Card card) {

        cardpile.Add(card);
        switch (card.GetNumber()) {
        case 8:
            Shithead.SkipTurn();
            break;
        case 9:
            Shithead.ChangeDirection();
            break;
        case 10:
            Burn();
            break;
        case 15:
            Console.WriteLine("reached joker");
            cardpile.RemoveAt(cardpile.Count - 1);
            Shithead.Joker();
            break;
        }

        playerTrio.RemoveCard(card);

        CheckIfBurn();

    }

    private void Burn() {
        Shithead.BurnTurn();
        cardpile.Clear();
    }

    private void CheckIfBurn() {
        if (cardpile.Count < 4) return;

        bool QuadCard = true;
        int permNum = cardpile[cardpile.Count - 1].GetNumber();

        for (int i = cardpile.Count - 2; i >= cardpile.Count - 4; i--) {
            if (cardpile[i].GetNumber() != permNum) QuadCard = false;
        }
        if (QuadCard) Burn();
    }



    public void TakePile(PlayerHand UnfortunateGuy) {
        foreach (Card card in cardpile) {
            UnfortunateGuy.AddCard(card);
        }
        cardpile.Clear();
    }


    public bool IsEmpty() {
        return cardpile.Count == 0;
    }


    public override string ToString() {
        string retval = "";
        foreach (Card card in cardpile) {
            retval += card.ToString() + ", ";
        }
        return retval;
    }

    public void ViewPile() {
        Console.Write("Game Pile: ");
        foreach (Card card in cardpile) {
            Console.Write(card.ToString() + ", ");
        }
        Console.WriteLine();
    }
}