using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public static class Shithead
{
    //max 6 will be fun

    private static int Index;

    private static int indexToAdd = 1;

    private static int SetIndexTo = 0;

    private static int numOfPlayers = 4;

    private static List<PlayerHand> players;

    private static GameDeck gameDeck;

    private static GamePile gamePile;

    private static bool TurnSkipped = false;

    private static bool TurnSwiched = false;

    public static bool SomeoneWon = false;
    public static void StartGame(bool Input) {
        if (Input) ActualGame(int.Parse(Console.ReadLine()));
        ActualGame(4);
    }
    public static void StartGame(int NumOfPlayers) {
        ActualGame(NumOfPlayers);
    }

    public static void StartGame() {
        ActualGame(4);
    }

    /*
    private static void ActualGame(int NumOfPlayers) {
        numOfPlayers = NumOfPlayers;
        if (numOfPlayers < 2 || numOfPlayers > 6) return;
        gameDeck = new GameDeck();
        gamePile = new GamePile();
        players = new List<PlayerHand>();
        for (int i = 1; i <= numOfPlayers; i++)
            players.Add(new PlayerHand(gameDeck, gamePile));
        while (true) {
            gamePile.ViewPile();
            for (Index = SetIndexTo; CheckDirection(); Index += indexToAdd) {
                Console.WriteLine($"the current index is {Index}, the IndextoAdd is {indexToAdd}" +
                    $" \n and the setindex is {SetIndexTo}");
                PlayerHand temp = players[Index];
                Console.Write("player {0}: ", Index + 1);
                temp.ViewHand();
                Console.WriteLine("enter the Card you want to");
                string card = Console.ReadLine();
                int tempCount = temp.CardCount();
                bool InvalidCard = !gamePile.ValidCard(card);

                if (!temp.Play(card)) {

                    if (InvalidCard) {
                        Console.WriteLine("card weaker");
                        temp.TakeAll();
                        Console.WriteLine($"new hand: {temp}");
                        continue;
                    }
                    Console.WriteLine("invalid card");
                    Index -= indexToAdd;

                }
                if(SomeoneWon) break;

            }
            if(SomeoneWon) break;

        }

        Console.WriteLine("player {0} Won! ggs",Index + 1);
    }

    */


    //private static void ActualGame(int NumOfPlayers,bool Testing) {
    //    numOfPlayers = NumOfPlayers;
    //    if (numOfPlayers < 2 || numOfPlayers > 6) return;
    //    gameDeck = new GameDeck();
    //    gamePile = new GamePile();
    //    players = new List<PlayerHand>();
    //    for (int i = 1; i <= numOfPlayers; i++)
    //        players.Add(new PlayerHand(gameDeck, gamePile,true));
    //    while (true) {
    //        gamePile.ViewPile();
    //        for (Index = SetIndexTo; CheckDirection(); Index += indexToAdd) {
    //            Console.WriteLine($"the current index is {Index}, the IndextoAdd is {indexToAdd}" +
    //                $" \n and the setindex is {SetIndexTo}");
    //            PlayerHand temp = players[Index];
    //            Console.Write("player {0}: ", Index + 1);
    //            temp.ViewHand();
    //            Console.WriteLine("enter the Card you want to");
    //            string card = Console.ReadLine();
    //            int tempCount = temp.CardCount();
    //            bool InvalidCard = !gamePile.ValidCard(card);

    //            if (!temp.Play(card)) {

    //                if (InvalidCard) {
    //                    Console.WriteLine("card weaker");
    //                    temp.TakeAll();
    //                    Console.WriteLine($"new hand: {temp}");
    //                    continue;
    //                }
    //                Console.WriteLine("invalid card");
    //                Index -= indexToAdd;

    //            }
    //            if (SomeoneWon) break;

    //        }
    //        if (SomeoneWon) break;

    //    }

    //    Console.WriteLine("player {0} Won! ggs", Index + 1);
    //}

    #region TryForLogic

    #region try
    private const int DEFAULT_NUM_OF_PLAYERS = 4;
    private static int AllCounter = 0;
    private static int tempCount = 0;
    private static bool TurnForward = true;
    private static bool Won = false;
    private static bool Burn = false;
    private static bool Skip = false;
    private static bool TurnReveresedThisTurn = false;
    #endregion

    private static void ActualGame(int NumOfPlayers) {
        const int NUM_OF_TURNS_UNTIL_PILE_VIEW = 4;

        numOfPlayers = NumOfPlayers;
        if (numOfPlayers < 2 || numOfPlayers > 6) return;
        gameDeck = new GameDeck();
        gamePile = new GamePile();
        players = new List<PlayerHand>();
        for (int i = 1; i <= numOfPlayers; i++)
            players.Add(new PlayerHand(gameDeck, gamePile, true));
        Index = 0;

        while (true) {
            
            if (AllCounter % NUM_OF_TURNS_UNTIL_PILE_VIEW == 0) gamePile.ViewPile();
            tempCount = Math.Abs(AllCounter % NumOfPlayers);
            PlayerHand temp = players[tempCount];
            Console.Write("player {0}: ", tempCount + 1);
            temp.ViewHand();
            Console.WriteLine("enter the Card you want to");
            string card = Console.ReadLine();
            bool InvalidCard = !gamePile.ValidCard(card);

            if (!temp.Play(card)) {

                if (InvalidCard) {
                    Console.WriteLine("card weaker");
                    temp.TakeAll();
                    Console.WriteLine($"new hand: {temp}");
                }
            }
            if (Won) break;
            UpdateCounter();
            ZeroOut();
        }
    }

    public static void StartTestGame(int NumOfPLayers) {
        TestGameUntilTrio(numOfPlayers);
    }
    public static void StartTestGame() {
        TestGameUntilTrio(DEFAULT_NUM_OF_PLAYERS);
    }

    private static void TestGameUntilTrio(int NumOfPlayers) {
        const int NUM_OF_TURNS_UNTIL_PILE_VIEW = 4;

        numOfPlayers = NumOfPlayers;
        if (numOfPlayers < 2 || numOfPlayers > 6) return;
        gameDeck = new GameDeck();
        gamePile = new GamePile();
        players = new List<PlayerHand>();
        for (int i = 1; i <= numOfPlayers; i++)
            players.Add(new PlayerHand(gameDeck, gamePile, true));
        Index = 0;

        while (players[tempCount].GetState() == 0) {
            if (AllCounter % NUM_OF_TURNS_UNTIL_PILE_VIEW == 0) gamePile.ViewPile();
            tempCount = Math.Abs(AllCounter % NumOfPlayers);
            PlayerHand temp = players[tempCount];
            Console.Write("player {0}: ", tempCount + 1);
            temp.Show();
            Console.WriteLine("currenmt all counter: {0} and current tempcount: {1}", AllCounter, tempCount);
            string card = temp.GetBestCard();
            bool InvalidCard = !gamePile.ValidCard(card);
            Console.WriteLine(card);
            if (!temp.Play(card)) {

                if (InvalidCard) {
                    Console.WriteLine("card weaker");
                    temp.TakeAll();
                    Console.WriteLine($"new hand: {temp}");
                }
                else { Console.WriteLine("invalid card please try again"); continue; }

            }
            Console.WriteLine(Won);
            if (Won) break;
            UpdateCounter();
            ZeroOut();
        }

        Console.WriteLine("reached trio");

        while (true) {

            if (AllCounter % NUM_OF_TURNS_UNTIL_PILE_VIEW == 0) gamePile.ViewPile();
            tempCount = Math.Abs(AllCounter % NumOfPlayers);
            PlayerHand temp = players[tempCount];
            Console.Write("player {0}: ", tempCount + 1);
            temp.Show();
            Console.WriteLine("currenmt all counter: {0} and current tempcount: {1}",AllCounter,tempCount);
            string card = Console.ReadLine();
            bool InvalidCard = !gamePile.ValidCard(card);

            if (!temp.Play(card)) {

                if (InvalidCard) {
                    Console.WriteLine("card weaker");
                    temp.TakeAll();
                    Console.WriteLine($"new hand: {temp}");
                }
                else {Console.WriteLine("invalid card please try again"); continue; }
            }
            if (Won) break;
            UpdateCounter();
            ZeroOut();
        }
    }



    private static void ZeroOut() {
        Burn = false;
        Skip = false;
        TurnReveresedThisTurn = false;
    }

    private static void UpdateCounter() {
        if (Burn) return;
        if (TurnForward) {
            AllCounter++;
            if (Skip) AllCounter++;
        }
        else { AllCounter--; if (Skip) AllCounter--; }
    }

    public static void BroWon() {
        Won = true;
    }
    public static void ChangeDirection() {
        if (TurnReveresedThisTurn) return;
        TurnReveresedThisTurn = true;
        TurnForward = !TurnForward;
    }
    public static void SkipTurn() {

        Skip = true;

    }
    public static void BurnTurn() {
        Burn = true;
    }

    #endregion
    /*
     * private static bool CheckDirection() {
        TurnSkipped = false;
        if (indexToAdd == 1) return Index < players.Count;
        return Index >= 0;
    }

    public static void BroWon() {
        SomeoneWon = true;
    }
    
    public static void ChangeDirection() {
        indexToAdd = -indexToAdd;
        SetIndexTo = players.Count- 1 - SetIndexTo;
    }

    public static void SkipTurn() {
        if(TurnSkipped) return;
        TurnSkipped = true;
        Index += indexToAdd;
        if (Index == 0 || Index == players.Count - 1)
            SetIndexTo += indexToAdd;
    }

    public static void BurnTurn() {
        Index -= indexToAdd;
    }*/

}