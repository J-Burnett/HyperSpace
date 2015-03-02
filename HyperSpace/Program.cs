using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperSpace
{
    class Program
    {
        static void Main(string[] args)
        {
            //Prompts user to play game
            Console.WriteLine("Press a key to play...");
            Console.ReadKey();

            //Creates new game
            Hyperspace game = new Hyperspace();
            game.PlayGame();
        }
    }

    /// <summary>
    /// New class for obstacles 
    /// </summary>
    class Unit
    {
        public int X { get; set; }//X coordiante
        public int Y { get; set; }//Y coordinate
        public ConsoleColor Color { get; set; }//Color of obstacle
        public string Symbol { get; set; }//Shape of obstacle
        public bool IsSpaceRift { get; set; }//determines if object is a space rift

        //Flying objects
        static List<string> ObstacleList = new List<string>() { "!", "%", "()", "&", "#", "?" };

        //Random number
        Random rng = new Random();
        //Constructor
        public Unit(int x, int y)
        {
            //Initializing and sets values
            this.X = x;
            this.Y = y;
            this.Symbol = ObstacleList[rng.Next(ObstacleList.Count())];
            this.Color = ConsoleColor.Cyan;
        }

        //Constructor with color, symbol, space rift overloads
        public Unit(int x, int y, ConsoleColor color, string symbol, bool IsSpaceRift)
        {
            //Initializes and sets values
            this.X = x;
            this.Y = y;
            this.Color = color;
            this.Symbol = symbol;
            this.IsSpaceRift = IsSpaceRift;
        }

        //Draw method for Units
        public void Draw()
        {
            //Draws the obstacle based on x,y coordinates, sets the color, and selects the symbol
            Console.SetCursorPosition(X,Y);
            Console.ForegroundColor = Color;
            Console.WriteLine(Symbol);
        }
    }

    /// <summary>
    /// Hyperspace Class for gameplay
    /// </summary>
    class Hyperspace
    {
        public int Score {get;set;}//Player's score
        public int Speed {get;set;}//Ship's spped
        public List<Unit> ObstacleList {get;set;}//List of obstacles to avoid
        public Unit SpaceShip {get;set;}//Spaceship
        public bool Smashed {get;set;}//Game play boolean
        //Random number generator
        private Random rng = new Random();

        /// <summary>
        /// Constructor
        /// </summary>
        public Hyperspace()
        {
            //Sets console size and buffer
            Console.WindowWidth = 60;
            Console.BufferWidth = 60;
            Console.BufferHeight = 30;
            Console.WindowHeight = 30;
            
            //Initializes and sets values to properties
            this.Score = 0;
            this.Speed = 0;
            this.ObstacleList = new List<Unit>();
            this.Smashed = false;
            this.SpaceShip = new Unit((Console.WindowWidth/2)-1 , Console.WindowHeight -1, ConsoleColor.Red, "@", false);
        }

        /// <summary>
        /// Play Game Method
        /// </summary>
        public void PlayGame()
        {
            //Keeps game open
            while(!Smashed)
            {
                //Determines chance for rift to be created
                int newRift = rng.Next(10);
                //if a rift can be created,
                if(newRift > 8)
                {
                    //Create rift
                    Unit newSpaceRift = new Unit(rng.Next(Console.WindowWidth - 2), 5, ConsoleColor.Green, "%", true);
                    //Add to the Obstacle List
                    this.ObstacleList.Add(newSpaceRift);
                }
                    //Otherwise,
                else
                {
                    //Create a regular obstacle
                    Unit newObstacle = new Unit(rng.Next(0, Console.WindowWidth - 2), 5);
                    //Add to the Obstacle List
                    this.ObstacleList.Add(newObstacle);
                }
                //Call move ship, move obstacles, and draw game methods
                MoveShip();
                MoveObstacles();
                DrawGame();

                //If less than 170
                if(this.Speed < 170)
                {
                    //Increases speed
                    this.Speed++;
                }
                //Slows how quickly screen is printed
                System.Threading.Thread.Sleep(170 - this.Speed);
            }
        }
        /// <summary>
        /// Controls the Ship's movements
        /// </summary>
        public void MoveShip()
        {
            //Checks for key strokes
            if(Console.KeyAvailable)
            {
                //Stores the key pressed
                ConsoleKey keyPressed = Console.ReadKey(true).Key;
                //Increases efficient game play
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
                //If the Left Arrow key is pressed,
                if (keyPressed == ConsoleKey.LeftArrow && this.SpaceShip.X > 0)
                {
                    //Decrement Ship's x coordinate position
                    this.SpaceShip.X--;
                }
                //If the Right Arrow Key is pressed,
                else if(keyPressed == ConsoleKey.RightArrow && this.SpaceShip.X < Console.WindowWidth - 2)
                {
                    //Increment Ship's x coordinate position
                    this.SpaceShip.X++;
                }
                //Otherwise,
                else
                {
                    //Print error message
                    Console.WriteLine("Invalid Move");
                }
            }
        }
        /// <summary>
        /// Moves the obstacles in the list
        /// </summary>
        public void MoveObstacles()
        {
            //Creates new list of obstacles
            List<Unit> newObstacleList = new List<Unit>();
            //Loops through obstacles in ObstacleList
            foreach(Unit obstacle in ObstacleList)
            {
                //Increments obstacle Y coordinate
                obstacle.Y++;
                //Checks if its a space rift and has the same coordinates as the Ship
                if(obstacle.IsSpaceRift && obstacle.X == SpaceShip.X && obstacle.Y == SpaceShip.Y)
                {
                    //Decrease speed
                    Speed -= 50;
                    //Allow game to continue
                    Smashed = false;
                }
                //Checks if a non-rift obstacle has the same coordinates as the Ship
                else if(obstacle.X == SpaceShip.X && obstacle.Y == SpaceShip.Y)
                {
                    //Ends Game
                   Smashed = true;
                }
                //If still on the screen
                if(obstacle.Y < Console.WindowHeight)
                {
                    //Adds obstacle to the list
                    newObstacleList.Add(obstacle);
                }
                else
                {
                    Score++;//Increments score
                }
            }

            ObstacleList = newObstacleList;
        }

        /// <summary>
        /// Draw's the game
        /// </summary>
        public void DrawGame()
        {
            //Clears the console
            Console.Clear();
            //Calls Draw method on the Ship
            this.SpaceShip.Draw();
            //Loops through each obstacle
            foreach (Unit obstacle in ObstacleList)
            {
                //Calls the Draw method on the obstacle
                obstacle.Draw();
            }
            //Prints Score and Speed info to the screen
            PrintAtPosition(20, 20, "Score: " + this.Score, ConsoleColor.Green);
            PrintAtPosition(20, 3, "Speed: " + this.Speed, ConsoleColor.Green);
        }

        /// <summary>
        /// Prints specific text to the console
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="text">Text to print</param>
        /// <param name="color">Color to print the text in</param>
        public void PrintAtPosition(int x, int y, string text, ConsoleColor color)
        {
            //Sets cursor info
            Console.SetCursorPosition(x, y);
            //Sets color
            Console.ForegroundColor = color;
            //prints the text to the console
            Console.WriteLine(text);

        }
    }
}
