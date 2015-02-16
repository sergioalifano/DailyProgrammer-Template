using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework; //test classes need to have the using statement

///     REDDIT DAILY PROGRAMMER SOLUTION TEMPLATE 
///                             http://www.reddit.com/r/dailyprogrammer
///     Your Name: 
///     Challenge Name:
///     Challenge #: 
///     Challenge URL: 
///     Brief Description of Challenge:
///
/// 
///
///     What was difficult about this challenge?
/// 
///
///     
///
///     What was easier than expected about this challenge?
///     
///
///
///
///     BE SURE TO CREATE AT LEAST TWO TESTS FOR YOUR CODE IN THE TEST CLASS
///     One test for a valid entry, one test for an invalid entry.

namespace DailyProgrammer_Template
{
    class Program
    {
        static List<Player> listOfPlayers;

        static void Main(string[] args)
        {
            Deck myDeck = new Deck();
            myDeck.Shuffle();

            int numberOfPlayers;
            bool invalidInput = true;
            

            while (invalidInput)
            {
                //ask for the number of players (and check if valid input) 
                Console.Write("Insert number of players: ");
                if (int.TryParse(Console.ReadLine(), out numberOfPlayers))
                {
                    listOfPlayers = new List<Player>();

                    //ask for names of the players and add them to the list of players
                    for (int i = 1; i < numberOfPlayers + 1; i++)
                    {
                        Console.WriteLine("Insert player name #{0}: ", i);
                        //add a new player to the list
                        listOfPlayers.Add(new Player(Console.ReadLine()));

                        //Set the current hand of the player with a random numberof cards
                        listOfPlayers[i - 1].DrawHand(myDeck.Deal());
                    }
                    invalidInput = false;
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }

            //show the cards for each player
            foreach (Player player in listOfPlayers)
            {
                Console.Write(player.Name+": ");
                foreach (Card card in player.MyCards)
                {
                    Console.Write(card.Rank+" ");
                }
                Console.WriteLine();
            }
            //show the winner
            Console.WriteLine("The winner is: "+TheWinnerIs(listOfPlayers).Name );

            Console.ReadKey();
        }//end of main





        /// <summary>
        /// Simple function to illustrate how to use tests
        /// </summary>
        /// <param name="inputInteger"></param>
        /// <returns></returns>
        public static int MyTestFunction(int inputInteger)
        {
            return inputInteger;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="listOfPlayers"></param>
        /// <returns></returns>
        public static Player TheWinnerIs(List<Player> listOfPlayers)
        {
            List<Player> notBusted = new List<Player>();

            //list of player with rank<=21
            notBusted = listOfPlayers.Where(x => x.GetRankSum() <= 21).ToList();

            int max = notBusted.Max(x => x.GetRankSum());
            return notBusted.Where(x => x.GetRankSum() == max).ToList().First();
        }

       
    }

    public enum Suit
    {
        Heart,
        Diamond,
        Club,
        Spade
    }

    public enum Rank
    {
        Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack = 10, Queen = 10, King = 10, Ace = 11
    }

    class Card
    {
        //Rank aceValue;
        public Suit Suit { get; set; }
        public Rank Rank
        {
            get;

            set;
        }

        public Card(int rank, int suit)
        {
            this.Suit = (Suit)suit;
            this.Rank = (Rank)rank;
        }

        public int GetRankValue()
        {
            return (int)Rank;
        }
        
    }

    class Deck
    {
        public List<Card> deckOfCards;

        public Deck()
        {
            //create a new deck
            deckOfCards = new List<Card>();

            //add cards to deck
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    deckOfCards.Add(new Card((int)rank, (int)suit));
                }
            }
        }

        public void Shuffle()
        {
            List<Card> shuffled = new List<Card>();
            int randomCard, cardsInDeck = deckOfCards.Count;

            Random gnr = new Random();
            for (int i = 0; i < cardsInDeck; i++)
            {
                randomCard = gnr.Next(0, deckOfCards.Count);

                //add the random card to the shuffled deck
                shuffled.Add(deckOfCards[randomCard]);

                //remove from the original deck the card
                deckOfCards.RemoveAt(randomCard);
            }

            deckOfCards = shuffled;
        }

        /// <summary>
        /// Take a random number of cards(between 1 and 5) from the top of the deck
        /// </summary>
        /// <returns>a list of cards</returns>
        public List<Card> Deal()
        {
            List<Card> myCards = new List<Card>();
            Random gnr = new Random();

            //generate a random number between 1 and 5
            int randomNumberOfCards = gnr.Next(1, 6);
         
           //save cards into another list
            myCards = deckOfCards.Take(randomNumberOfCards).ToList();

           //remove cards from the deck
            deckOfCards.RemoveRange(0, randomNumberOfCards);
           
           return myCards;
        }

    }

    class Player
    {
        public List<Card> MyCards { get; set; }

        public string Name { get; set; }

        public Player(string name)
        {
            this.Name = name;            
        }

        /// <summary>
        /// Set the card to the player
        /// </summary>
        /// <param name="myCards"></param>
        public void DrawHand(List<Card> myCards)
        {
            this.MyCards = myCards;
        }

        public int GetRankSum()
        {

            return MyCards.Sum(x => x.GetRankValue());
        }

        
    }


#region " TEST CLASS "

    //We need to use a Data Annotation [ ] to declare that this class is a Test class
    [TestFixture]
    class Test
    {
        //Test classes are declared with a return type of void.  Test classes also need a data annotation to mark them as a Test function
        [Test]
        public void MyValidTest()
        {
            //inside of the test, we can declare any variables that we'll need to test.  Typically, we will reference a function in your main program to test.
            int result = Program.MyTestFunction(15);  // this function should return 15 if it is working correctly
            //now we test for the result.
            Assert.IsTrue(result == 15, "This is the message that displays if it does not pass");
            // The format is:
            // Assert.IsTrue(some boolean condition, "failure message");
        }

        [Test]
        public void MyInvalidTest()
        {
            int result = Program.MyTestFunction(15);
            Assert.IsFalse(result == 14);
        }
    }
#endregion
}
