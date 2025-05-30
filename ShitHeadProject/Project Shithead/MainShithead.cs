using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    //made my magnizz
    private static int FindStarterPlayer() {
        const int numOfCardsAtStart = 3;
        int first = 0;
        bool SameCards = false;
        int[,] count = new int[numOfPlayers, numOfCardsAtStart];

        for (int i = 0; i < numOfPlayers; i++)
            for (int j = 0; j < numOfCardsAtStart; j++)
                count[i, j] += players[i].GetLeastValuableCardByNumber(j + 1);

        for (int i = 0; i < numOfCardsAtStart; i++) {
            for (int j = 1; j < numOfPlayers; j++) {
                int check = count[j, i];
                int check2 = count[first, i];
                if (count[j, i] < count[first, i]) {
                    SameCards = false;
                    first = j;
                }
                else if (count[j, i] == count[first, i])
                    SameCards = true;
            }
            if (!SameCards)
                return first;
        }


        return first;
    }

    #region more static attreibutes
    private const int DEFAULT_NUM_OF_PLAYERS = 4;
    private static int AllCounter = 0;
    private static int tempCount = 0;
    private static bool TurnForward = true;
    private static bool Won = false;
    private static bool Burn = false;
    private static bool Skip = false;
    private static bool TurnReveresedThisTurn = false;
    private static bool SinglePlayer = false;
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

    public static void StartSingleplayer(int playerNum) {
        StartGameOnePlayer(playerNum);
    }
    public static void StartSingleplayer() {
        StartGameOnePlayer(DEFAULT_NUM_OF_PLAYERS);
    }


    public static void StartGameOnePlayer(int NumOfPlayers) {
        SinglePlayer = true;
        string card;
        numOfPlayers = NumOfPlayers;
        if (numOfPlayers < 2 || numOfPlayers > 6) return;
        gameDeck = new GameDeck();
        gamePile = new GamePile();
        players = new List<PlayerHand>();
        players.Add(new PlayerHand(gameDeck, gamePile));
        for (int i = 2; i <= numOfPlayers; i++)
            players.Add(new PlayerHand(gameDeck, gamePile,true));
        Index = 0;

        while (true) {
            PlayerHand TempPlayerHand = players[tempCount];

            if (tempCount == 0) {
                gamePile.ViewPile();
                TempPlayerHand.Show();
                int state = TempPlayerHand.GetState();
                if (state == 3) { card = "you won bro"; break; }
                else if (state == 2) {
                    if (!TempPlayerHand.Play()) {
                        Console.WriteLine("card {0} weaker",TempPlayerHand.GetLastCard());
                        TempPlayerHand.TakeAll();
                        Console.WriteLine($"new hand: {TempPlayerHand}");
                    }
                    if (Won) break;
                    UpdateCounter();
                    ZeroOut();
                    continue;
                }
                else {
                    Console.WriteLine("last card in the pile: " + gamePile.GetLastCard());
                    Console.WriteLine("enter the card you want to play");
                    card = Console.ReadLine();
                }
            }
            else { card = TempPlayerHand.GetBestCard(); Console.WriteLine("ai number {0} state: {1}", tempCount, TempPlayerHand.GetState()); }
            bool InvalidCard = !gamePile.ValidCard(card);
            Console.WriteLine(card);
            if (!TempPlayerHand.Play(card)) {

                if (InvalidCard) {
                    Console.WriteLine("card weaker");
                    TempPlayerHand.TakeAll();
                    Console.WriteLine($"new hand: {TempPlayerHand}");
                }
                else { Console.WriteLine("invalid card please try again"); continue; }

            }
            Won = TempPlayerHand.Won();
            if (Won) break;
            UpdateCounter();
            ZeroOut();
        }

        Console.WriteLine("player {0} wins!",tempCount);

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
        tempCount = Math.Abs(AllCounter % numOfPlayers);

    }

    #region Joker
    public static void Joker() {
        if (SinglePlayer) JokerSinglePlayer();
        else JokerMultyPlayer();
    }
    private static void JokerMultyPlayer() {
        int PlayerToGiveJoker;
        Console.WriteLine("enter the player you want to give joker to");
        PlayerToGiveJoker = int.Parse(Console.ReadLine()) - 1;
        while (PlayerToGiveJoker == tempCount) {
            Console.WriteLine("invalid player, please try again");
            PlayerToGiveJoker = int.Parse(Console.ReadLine()) - 1;
        }
        GiveJoker(PlayerToGiveJoker);
    }
    private static void JokerSinglePlayer() {
        int PlayerToGiveJoker;
        if (tempCount == 0) {
            Console.WriteLine("enter the player you want");
            PlayerToGiveJoker = int.Parse(Console.ReadLine()) - 1;
            while (PlayerToGiveJoker == 0) {
                Console.WriteLine("invalid player, please try again");
                PlayerToGiveJoker = int.Parse(Console.ReadLine()) - 1;
            }
            GiveJoker(PlayerToGiveJoker);
        }
        else {
            System.Random rnd = new System.Random();
            PlayerToGiveJoker = rnd.Next(0, numOfPlayers);
            while (PlayerToGiveJoker == tempCount) {
                PlayerToGiveJoker = rnd.Next(0, numOfPlayers);
            }
            Console.WriteLine($"gave joker to player {PlayerToGiveJoker + 1}");
            GiveJoker(PlayerToGiveJoker);
        }
    }

    private static void GiveJoker(int playerToGive) {
        players[playerToGive].TakeAll();
        BurnTurn();
    }
    #endregion

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

  
}