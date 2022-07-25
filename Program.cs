using System;

namespace TestingTest
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            GameLoop gameLoop = new GameLoop();
            gameLoop.StartGame();
        }
    }
}