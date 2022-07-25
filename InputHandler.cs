using System;

namespace TestingTest
{
    public class InputHandler
    {
        public DirectionType GetInputDirection(ConsoleKeyInfo key) 
        {
            return key.Key switch
            {
                ConsoleKey.UpArrow => DirectionType.North,
                ConsoleKey.DownArrow => DirectionType.South,
                ConsoleKey.LeftArrow => DirectionType.West,
                ConsoleKey.RightArrow => DirectionType.East,
                ConsoleKey.Backspace => DirectionType.Back,
                _ => DirectionType.Undefined,
            };            
        }
    }
}