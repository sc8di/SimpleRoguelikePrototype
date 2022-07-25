using System;
using System.Collections.Generic;

namespace TestingTest
{
    public class CommandRecorder
    {
        private const int RollBackStepsMax = 10;

        private readonly Stack<List<CommandBase>> _commands = new Stack<List<CommandBase>>(RollBackStepsMax);

        public Action<int> RollBackStepsChanged;

        public void PrepareNextRecordList()
        {
            if (_commands.Count == RollBackStepsMax)
            {
                Stack<List<CommandBase>> tempCommands = new Stack<List<CommandBase>>();
                for (int i = 0; i < RollBackStepsMax; i++)
                {
                    tempCommands.Push(_commands.Pop());
                }
                tempCommands.Pop();
                for (int i = 0; i < RollBackStepsMax - 1; i++)
                {
                    _commands.Push(tempCommands.Pop());
                }
            }

            _commands.Push(new List<CommandBase>());
            RollBackStepsChanged?.Invoke(_commands.Count);
        }

        public void Record(CommandBase command)
        {
            _commands.Peek().Add(command);
            command.Execute();
        }

        public void Rewind()
        {
            if (_commands.Count == 0)
            {
                return;
            }

            var commands = _commands.Pop();
            RollBackStepsChanged?.Invoke(_commands.Count);

            foreach (var command in commands)
            {
                command.Undo();
            }
        }
    }
}