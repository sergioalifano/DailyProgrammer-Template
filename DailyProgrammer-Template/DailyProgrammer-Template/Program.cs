using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework; //test classes need to have the using statement



namespace DailyProgrammer_Template
{
    class Program
    {
        static List<Player> listOfPlayers;
		
		public static List<string> listStringDate = new List<string>();
        public static List<DateTime> listDateTime = new List<DateTime>();

        static void Main(string[] args)
        {
            //FIRST CHALLANGE
            //initialize the game
            InitGame();

            //show the cards of the player
            ShowHand();

            //show the winner
            DisplayWinner();

			//SECOND CHALLANGE
			//read from file
            readFile();
            
            //convert each string in DateTime type
            foreach (string s in listStringDate)
            {
                listDateTime.Add(Convert.ToDateTime(s));
            }

            sortDate();

            writeOnFile();
            Console.ReadKey();
        } //end main

       

        ///     REDDIT DAILY PROGRAMMER SOLUTION TEMPLATE 
        ///                             http://www.reddit.com/r/dailyprogrammer
        ///     Your Name: Sergio
        ///     Challenge Name: [Intermediate]Date Sorting
        ///     Challenge #: 112
        ///     Challenge URL: http://www.reddit.com/r/dailyprogrammer/comments/137f87/11142012_challenge_112_intermediatedate_sorting/
        ///     Brief Description of Challenge:
        ///     The program take a list of string date and sort the date in ascendig way. I modified a little bit the input and the output
        ///     in order to read from and write to a file
        ///
        /// 
        ///
        ///     What was difficult about this challenge?
        ///     Nothing in particular
        ///
        ///     
        ///
        ///     What was easier than expected about this challenge?
        ///     I don't think it was intermediate. Wrong choice. I'll choose a better one next time! 
        ///
        ///
        ///
        ///     BE SURE TO CREATE AT LEAST TWO TESTS FOR YOUR CODE IN THE TEST CLASS
        ///     One test for a valid entry, one test for an invalid entry.
        ///     

        /// <summary>
        /// read from the file and populate list of date
        /// </summary>
        public static void readFile()
        {
            StreamReader reader = new StreamReader("date_list.txt");

            // Read all the file
            while (!reader.EndOfStream)
            {
                //add each line to the list 
                listStringDate.Add(reader.ReadLine());
            }
        }

        //write on a new file the ordered dates
        public static void writeOnFile()
        {
            using (StreamWriter writer = new StreamWriter("date sorted ascending.txt"))
            {
                foreach (DateTime date in listDateTime)
                {
                    writer.WriteLine(date);
                }
            }
        }

        /// <summary>
        /// sort list of date in ascending way
        /// </summary>
        public static void sortDate()
        {
            listDateTime.Sort((a, b) => a.CompareTo(b));
        }







        ///     REDDIT DAILY PROGRAMMER SOLUTION TEMPLATE 
        ///                             http://www.reddit.com/r/dailyprogrammer
        ///     Your Name: Sergio
        ///     Challenge Name: BlackJack Checker
        ///     Challenge #: 170
        ///     Challenge URL: http://www.reddit.com/r/dailyprogrammer/comments/29zut0/772014_challenge_170_easy_blackjack_checker/
        ///     Brief Description of Challenge:
        ///     Calculate the value of your card and pick the winner. There is a small change of the normal rules of Black Jack: the 
        ///     player that has exactly 5 cards with a value less or equal to 21 automatically wins.
        ///     I made another modification not about the rules but how to choose the cards: cards are picked random and players does't know
        ///     how many cards will be given
        ///
        ///
        ///     What was difficult about this challenge?
        ///     Find the more efficient way to find the winner using lambda expressions
        ///     Keep the code organized
        ///
        ///     
        ///
        ///     What was easier than expected about this challenge?
        ///     I actually reused classes we already created for poker. I'm trying to get gradually more skills using classes for 
        ///     poker game because I want to create a "real" poker game using graphic and AI
        ///
        ///
        ///
        ///     BE SURE TO CREATE AT LEAST TWO TESTS FOR YOUR CODE IN THE TEST CLASS
        ///     One test for a valid entry, one test for an invalid entry.





        /// <summary>
        /// Initialize the game and ask for name players
        /// </summary>
        public static void InitGame()
        {
            int numberOfPlayers;
            bool invalidInput = true;

            //create a new deck
            Deck myDeck = new Deck();
            //shuffle the deck
            myDeck.Shuffle();

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
        }


        /// <summary>
        /// Show the cards of each player
        /// </summary>
        static void ShowHand()
        {
            //show the cards for each player
            foreach (Player player in listOfPlayers)
            {
                Console.Write("\n"+player.Name + ": ");
                foreach (Card card in player.MyCards)
                {
                    Console.Write(card.Rank + " of "+ card.Suit+" ");
                }
                Console.WriteLine();
            }
        }

        static void DisplayWinner()
        {
            //Call the function that calculate the winner(s)
            List<Player> winner = TheWinnerIs(listOfPlayers);
            //if there is at least a winner
            if (winner.Count == 1)
            {
                Console.WriteLine("\nThe winner is: " + winner.First().Name);
            }
                //if it's a tie
            else if (winner.Count > 1)
            {
                Console.WriteLine("\nIt's a tie! Winners are: ");
                foreach (Player player in winner)
                {
                    Console.WriteLine(player.Name);
                }
            }
            else
            {
                Console.WriteLine("\nNo winner. All busted!!");
            }
        }        

        /// <summary>
        /// Calculate the winner
        /// </summary>
        /// <param name="listOfPlayers">List of players</param>
        /// <returns>A list with one or more winners</returns>
        public static List<Player> TheWinnerIs(List<Player> listOfPlayers)
        {
            List<Player> notBusted = new List<Player>();
            List<Player> fiveCardsTrick = new List<Player>();

            //filter the player with sum <= 21
            notBusted = listOfPlayers.Where(x => x.GetRankSum() <= 21).ToList();

            //select players with exactly 5 cards
            fiveCardsTrick = notBusted.Where(x => x.MyCards.Count == 5).ToList();

            if (fiveCardsTrick.Count >= 1)
            {
                return fiveCardsTrick;
            }

            //if there is at leat a winner
            if (notBusted.Count > 0)
            {
                //find the value of the max rank
                int max = notBusted.Max(x => x.GetRankSum());

                //return all the players with the highest rank
                return notBusted.Where(x => x.GetRankSum() == max).ToList();
            }
            else
                return notBusted;
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
        public Suit Suit { get; set; }
        public Rank Rank { get; set;}

        public Card(int rank, int suit)
        {
            this.Suit = (Suit)suit;
            this.Rank = (Rank)rank;
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
        /// <param name="myCards">List of cards</param>
        public void DrawHand(List<Card> myCards)
        {
            this.MyCards = myCards;
        }

        /// <summary>
        /// Calculate the sum of the card ranks
        /// </summary>
        /// <returns>The total rank</returns>
        public int GetRankSum()
        {
            return MyCards.Sum(x => (int)x.Rank);
        }        
        
    }


#region " TEST CLASS "

    //We need to use a Data Annotation [ ] to declare that this class is a Test class
    [TestFixture]
    class Test
    {
        Player sergio;
        Player jim;
        List<Player> listOfPlayers;

        List<string> listStringDate;

        [SetUp]
        public void testSetup()
        {
            sergio = new Player("Sergio");
            jim = new Player("Jim");
            listOfPlayers = new List<Player>();


            //SECOND CHALLANGE
            listStringDate = new List<string>();
        }

        //Test classes are declared with a return type of void.  Test classes also need a data annotation to mark them as a Test function
        [Test]
        public void MyValidTest()
        {
            sergio.MyCards = new List<Card>();
            sergio.MyCards.Add(new Card((int)Rank.Ten, (int)Suit.Club));
            sergio.MyCards.Add(new Card((int)Rank.Ten, (int)Suit.Club));
            sergio.MyCards.Add(new Card((int)Rank.Jack, (int)Suit.Club));
            sergio.MyCards.Add(new Card((int)Rank.Ace, (int)Suit.Club));            
            listOfPlayers.Add(sergio);

            jim.MyCards = new List<Card>();
            jim.MyCards.Add(new Card((int)Rank.Two, (int)Suit.Club));
            jim.MyCards.Add(new Card((int)Rank.Five, (int)Suit.Club));
            jim.MyCards.Add(new Card((int)Rank.Jack, (int)Suit.Club));
            jim.MyCards.Add(new Card((int)Rank.Two, (int)Suit.Club));           
            listOfPlayers.Add(jim);

            string winner = Program.TheWinnerIs(listOfPlayers).First().Name;
            Assert.IsTrue(winner == "Jim", "Failed");


            //SECOND CHALLENGE
            listStringDate.Add("2012 12 02 23:02:12 ");
            listStringDate.Add("2012 12 02 23:02:13 ");
            listStringDate.Add("2011 12 02 23:02:12 ");
            listStringDate.Add("2012 12 03 23:02:12 ");
            listStringDate.Sort((a, b) => a.CompareTo(b));

            string date = listStringDate[0];
            Assert.IsTrue(date == "2011 12 02 23:02:12 ");
            date = listStringDate[3];
            Assert.IsTrue(date == "2012 12 03 23:02:12 ");


        }

        [Test]
        public void MyInvalidTest()
        {
            sergio.MyCards = new List<Card>();
            sergio.MyCards.Add(new Card((int)Rank.Two, (int)Suit.Club));
            sergio.MyCards.Add(new Card((int)Rank.Three, (int)Suit.Club));
            sergio.MyCards.Add(new Card((int)Rank.Five, (int)Suit.Club));
            sergio.MyCards.Add(new Card((int)Rank.Six, (int)Suit.Club));
            sergio.MyCards.Add(new Card((int)Rank.Two, (int)Suit.Heart));            
            listOfPlayers.Add(sergio);

            jim.MyCards = new List<Card>();
            jim.MyCards.Add(new Card((int)Rank.Five, (int)Suit.Club));
            jim.MyCards.Add(new Card((int)Rank.Jack, (int)Suit.Club));
            jim.MyCards.Add(new Card((int)Rank.Six, (int)Suit.Club));             
            listOfPlayers.Add(jim);

            string winner = Program.TheWinnerIs(listOfPlayers).First().Name;

            //Sergio wins with 5cards trick rule
            Assert.IsFalse(winner=="Jim");



            //SECOND CHALLENGE
            listStringDate.Add("2012 12 02 23:02:12 ");
            listStringDate.Add("2012 12 02 23:02:13 ");
            listStringDate.Add("2011 12 02 23:02:12 ");
            listStringDate.Add("2012 12 03 23:02:12 ");
            listStringDate.Sort((a, b) => a.CompareTo(b));

            string date = listStringDate[0];
            Assert.IsFalse(date == "2012 12 02 23:02:13");
            date = listStringDate[3];
            Assert.IsFalse(date == "2012 12 02 23:02:12 ");
        }
    }
#endregion
}
