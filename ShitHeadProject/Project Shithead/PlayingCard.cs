using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

public class Card
{
    #region else
    private static readonly char[] Shapes = { 'C', 'D', 'H', 'S' };
    #endregion

    #region Str
    private string CardStr;
    #endregion

    #region Construct
    public Card() {
        this.CardStr = "0C";
    }
    public Card(string CardStr) {
        if (CardStr.Length == 2) {
            if ((!(CardStr[0] >= '2' && CardStr[0] <= '9')) || !IsValidShape(CardStr[CardStr.Length - 1])) { 
                this.CardStr = "0C";
                return;
            }
            this.CardStr = CardStr;
        }
        else if (CardStr.Length == 3) {
            if (!(CardStr[0] == '1') || !(CardStr[1] >= '0' && CardStr[1] <= '4') ||
                !IsValidShape(CardStr[CardStr.Length - 1])) {
                this.CardStr = "0C";
                return;
            }
            this.CardStr = CardStr;
        }
        else this.CardStr = "0C";
    }
    #endregion

    #region Get

    
    public int GetNumber() {
        string Parse = "";

        if (CardStr.Length == 2)
            Parse += CardStr[0];
        else { Parse += CardStr[0]; Parse += CardStr[1]; }

        return int.Parse(Parse);
    }

    public char GetShape() {
        return CardStr[CardStr.Length-1];
    }

    #endregion

    #region ShitHeadOnly
    public bool ValidCardToPut(Card putonthis) {
        return putonthis.GetNumber() >= GetNumber();
    }
    #endregion

    #region Override

    public override string ToString() {
        return CardStr;
    }

    public bool Equals(Card other) {
        return this.CardStr.Equals(other.CardStr);
    }

    public bool Equals(string other) {
        return this.CardStr.Equals(other);
    }
    #endregion

    #region Private mathods
    private bool IsValidShape(char shape) {
        return shape == 'C' || shape == 'H' || shape == 'D' || shape == 'S';
    }
    #endregion

    #region static
    public static bool IsValidCard(string cardTry) {
        return new Card(cardTry).ToString().Equals(cardTry);
    }
    #endregion
}
