using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GinasGameofWar.Code
{
    public static class CardFunctions
    {
        public static Queue<Card> Deck { get; set; }

        public static Queue<Card> BuildDeck()
        {
            Queue<Card> cards = new Queue<Card>();
                for (int i = 2; i <= 14; i++)
                {
                    foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                    {
                        cards.Enqueue(new Card()
                        {
                            Suit = suit,
                            Value = i,
                            DisplayName = GetDisplayName(i, suit),
                            ImageName = GetImageName(i, suit)
                        });
                    }
                }
                return Shuffle(cards);
        }
        
        private static string GetDisplayName(int value, Suit suit)
        {
            string nametoShow = "";

            switch (value)
            {
                case 11:
                    nametoShow = "J";
                    break;
                case 12:
                    nametoShow = "Q";
                    break;
                case 13:
                    nametoShow = "K";
                    break;
                case 14:
                    nametoShow = "A";
                    break;
                default:
                    nametoShow = value.ToString();
                    break;
            }
            return nametoShow + Enum.GetName(typeof(Suit), suit)[0];
        }

        private static string GetImageName(int value, Suit suit)
        {
            StringBuilder imgname = new StringBuilder("");

            switch (value)
            {
                case 11:
                    imgname.Append("jack");
                    break;
                case 12:
                    imgname.Append("queen");
                    break;
                case 13:
                    imgname.Append("king");
                    break;
                case 14:
                    imgname.Append("ace");
                    break;
                default:
                    imgname.Append("_" + value.ToString());
                    break;
            }

            imgname.Append("_of_");
            imgname.Append(Enum.GetName(typeof(Suit), suit).ToLower());

            return imgname.ToString();
        }

        public static Queue<T> Shuffle<T>(this Queue<T> queue)
        {
            Random random = new Random();
            return new Queue<T>(queue.OrderBy(x => random.Next()));
        }

        public static Queue<Card> DealCards(Queue<Card> newDeck, ref Queue<Card> temphand2)
        {
            Queue<Card> hand1 = new Queue<Card>();
            Queue<Card> hand2 = new Queue<Card>();

            while (newDeck.Any())
            {
                hand1.Enqueue(newDeck.Dequeue());
                hand2.Enqueue(newDeck.Dequeue());
            }

            // Code to make player 1 hand smaller for debugging purposes.
            //for (int i = 0; i <= 4; i++)
            //{
            //    hand1.Enqueue(newDeck.Dequeue());
            //}

            //while (newDeck.Any())
            //{
            //    hand2.Enqueue(newDeck.Dequeue());
            //}

            temphand2 = hand2;
            return hand1;
        }
       
    }
}

