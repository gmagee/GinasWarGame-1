using GinasGameofWar.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GinasGameofWar
{
    public partial class Form1 : Form
    {
        // Create new player objects
        private Player Player1;
        private Player Player2;
        private Boolean warFlag;
        private Queue<Card> warQ;
        private Boolean debugFlag;

        public Form1()
        {
            InitializeComponent();
            logo.Image = GinasGameofWar.Properties.Resources.warlogo;
        }   
        
        public void StartNewGame()
        {          
          
            // Create the players
            Player1 = new Player(player1nameTb.Text);
            Player2 = new Player(player2nameTb.Text);

            // Build and Shuffle the deck
            var hand2 = new Queue<Card>();
            var newDeck = CardFunctions.BuildDeck();
            // Deal the cards to the players
            Player1.Hand = CardFunctions.DealCards(newDeck, ref hand2);
            Player2.Hand = hand2;

            // Put visual elements in place
            logo.Visible = false;
            card1.Image =  GinasGameofWar.Properties.Resources.cardback;
            card2.Image = GinasGameofWar.Properties.Resources.cardback;
            warCard1.Image = GinasGameofWar.Properties.Resources.cardback;
            warCard2.Image = GinasGameofWar.Properties.Resources.cardback;
            player1.Image = GinasGameofWar.Properties.Resources.person;
            player2.Image = GinasGameofWar.Properties.Resources.person;
            debugPnl.Visible = false;
            debugBtn.Visible = true;
            p1ccLb.Visible = true;
            p2ccLb.Visible = true;
            card1.Visible = true;
            card2.Visible = true;
            player1.Visible = true;
            player2.Visible = true;
            player1nameLb.Visible = true;
            player2nameLb.Visible = true;
            startPnl.Visible = false;
            goBtn.Visible = true;

            // Label the Players and their hand counts
            player1nameLb.Text = player1nameTb.Text;
            player2nameLb.Text = player2nameTb.Text;
            p1ccLb.Text = "(" + Player1.Hand.Count.ToString() + ")";
            p2ccLb.Text = "(" + Player2.Hand.Count.ToString() + ")";
          
        }

        public void PlayHand()
        {            

            gameannouncerLb.Visible = true;
            int warWinner = 0; // flag for who wins the war when one is played
            string winnerText = " wins the hand.";

            if (warFlag)
            {
                // if there is a war to be dealt, make sure the players have enough cards in their hand to complete.  If they don't they lose.
                if (Player1.Hand.Count <= 1)
                {
                    GameOver(Player2.FirstName);
                    return;
                }
                else if (Player2.Hand.Count <= 1)
                {
                    GameOver(Player1.FirstName);
                    return;
                }

                // set visual elements
                winnerText = " wins the war!";
                warCard1.Visible = true;
                warCard2.Visible = true;
                // this is the card the players each play down during a war.  We are holding it in the warQ
                warQ.Enqueue(Player1.Hand.Dequeue());
                warQ.Enqueue(Player2.Hand.Dequeue());
            }
            else
            {
                warCard1.Visible = false;
                warCard2.Visible = false;
            }
            
            // each player plays a card
            Card p1card = Player1.Hand.Dequeue();
            Card p2card = Player2.Hand.Dequeue();

            // put the images into an object so we can dynamically get them from the resource manager
            object c1 = GinasGameofWar.Properties.Resources.ResourceManager.GetObject(p1card.ImageName);
            object c2 = GinasGameofWar.Properties.Resources.ResourceManager.GetObject(p2card.ImageName);

            // put the correct cards on the screen
            card1.Image = (Image)c1;
            card2.Image = (Image)c2;

            if (p1card.Value > p2card.Value) //player 1 wins
            {
                gameannouncerLb.Text = Player1.FirstName + winnerText;
                // player 1 won so he gets the cards added to his hand
                Player1.Hand.Enqueue(p1card);
                Player1.Hand.Enqueue(p2card);
                if (warFlag)  warWinner = 1;
            }
            else if (p1card.Value < p2card.Value) //player 2 wins
            {                
                gameannouncerLb.Text = Player2.FirstName + winnerText;
                // player 2 won so he gets the cards added to his hand
                Player2.Hand.Enqueue(p1card);
                Player2.Hand.Enqueue(p2card);
                if (warFlag) warWinner = 2;
            }
            else if (p1card.Value == p2card.Value) //war
            {                
                gameannouncerLb.Text = "Time for a war!";
                warFlag = true;
                // a war is happening, hold these cards in the warQ
                warQ.Enqueue(p1card);
                warQ.Enqueue(p2card);
                goBtn.Text = "Play War!";
            } 

            if (warWinner == 0)
            {
                // do nothing
            } else if (warWinner == 1)
            {
               // Player 1 wins the war, she gets all the war cards
                while (warQ.Any())
                {
                    Player1.Hand.Enqueue(warQ.Dequeue());
                }
                ClearWar();
            } else if (warWinner == 2)
            {
                // Player 2 wins the war, she gets all the war cards
                while (warQ.Any())
                {
                    Player2.Hand.Enqueue(warQ.Dequeue());
                }
                ClearWar();
            }

            int p1Count = Player1.Hand.Count;
            int p2Count = Player2.Hand.Count;

            // see if anyone has won the game
            if (Player1.Hand.Count == 0) {
                GameOver(Player2.FirstName);
                return;
            } else if (Player2.Hand.Count == 0){
                GameOver(Player1.FirstName);
                return;
            }

            PrintHands();
            p1ccLb.Text = "(" + p1Count.ToString() + ")";
            p2ccLb.Text = "(" + p2Count.ToString() + ")";


        }

        public void ClearWar()
        {
            goBtn.Text = "GO!";
            warFlag = false;
        }

        public void GameOver(string winner)
        {
            gameannouncerLb.Text = "Game Over.  " + winner + " Wins!!!!!";
            goBtn.Visible = false;
            newgameBtn.Visible = true;
            card1.Image = GinasGameofWar.Properties.Resources.cardback;
            card2.Image = GinasGameofWar.Properties.Resources.cardback;
            warCard1.Visible = false;
            warCard2.Visible = false;
            PrintHands();
            int p1Count = Player1.Hand.Count;
            int p2Count = Player2.Hand.Count;
            p1ccLb.Text = "(" + p1Count.ToString() + ")";
            p2ccLb.Text = "(" + p2Count.ToString() + ")";            
        }

        public void PrintHands()
        {
            StringBuilder warSB = new StringBuilder();
            StringBuilder p1SB = new StringBuilder();
            StringBuilder p2SB = new StringBuilder();
            // make a copy of the Queues so that we can print them without emptying them
            Queue<Card> warQCopy = new Queue<Card>(warQ.ToArray());
            Queue<Card> player1HandCopy = new Queue<Card>(Player1.Hand.ToArray());
            Queue<Card> player2HandCopy = new Queue<Card>(Player2.Hand.ToArray());

            while (warQCopy.Any())
            {
                warSB.Append(warQCopy.Dequeue().DisplayName + " ");
            }
            while (player1HandCopy.Any())
            {
                p1SB.Append(player1HandCopy.Dequeue().DisplayName + " ");
            }
            while (player2HandCopy.Any())
            {
                p2SB.Append(player2HandCopy.Dequeue().DisplayName + " ");
            }

            p1hLb.Text = p1SB.ToString();
            p2hLb.Text = p2SB.ToString();
            wqLb.Text = warSB.ToString();
        }

        private void newgameBtn_Click(object sender, EventArgs e)
        {
            // set visual elements to start a new game
            startPnl.Visible = true;
            logo.Visible = true;
            card1.Visible = false;
            card2.Visible = false;
            player1.Visible = false;
            player2.Visible = false;
            player1nameLb.Visible = false;
            player2nameLb.Visible = false;
            newgameBtn.Visible = false;
            debugBtn.Visible = false;
            debugPnl.Visible = true;
            gameannouncerLb.Visible = false;
            player1nameTb.Text = "";
            player2nameTb.Text = "";
            p1hLb.Text = "";
            p2hLb.Text = "";
            wqLb.Text = "";
            p1ccLb.Text = "";
            p2ccLb.Text = "";
        }

        private void debugBtn_Click(object sender, EventArgs e)
        {
            // Toggle Debug Visibility
            if (debugFlag)
            {
                debugPnl.Visible = true;
                debugBtn.Text = "Show Debug";
                debugFlag = false;
               
            } else
            {
                debugPnl.Visible = false;
                debugBtn.Text = "Hide Debug";
                debugFlag = true;
            }

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            // make sure they have entered a name for each player
            if ((player1nameTb.Text.Length == 0) || (player2nameTb.Text.Length == 0))
            {
                MessageBox.Show("Please enter a name for both players.");
            }
            else
            {
            StartNewGame();
            warFlag = false;
            debugFlag = true;            
            warQ = new Queue<Card>();
            PrintHands();
            }
        }

        private void goBtn_Click(object sender, EventArgs e)
        {
            PlayHand();
        }

    }
}
 