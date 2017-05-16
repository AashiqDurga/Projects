using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CQRSKata
{
    class Program
    {
        static void Main(string[] args)
        {
            var moveCommand = new MoveCommand(Direction.Down);
            var moveCommandHandler = new MoveCommandHandler();
            var collectCommand = new CollectCommand(Material.Air);
            var collectCommandHandler = new CollectCommandHandler();
            var destroyCommand = new DestroyCommandHandler(Target.Sandile);

            collectCommandHandler.Execute(collectCommand);
            moveCommandHandler.Execute(moveCommand);
            destroyCommand.Execute();
        }
    }

    public interface ICommandHandler
    {
        //void Execute();
    }

    public class MoveCommandHandler : ICommandHandler
    {
        public void Execute(MoveCommand moveCommand)
        {
            Console.WriteLine(moveCommand.Direction);
        }
    }
    public class CollectCommandHandler : ICommandHandler
    {
        public void Execute(CollectCommand collectCommand)
        {
            Console.WriteLine($"Collecting {collectCommand.Material}");
        }
    }
    
    public class DestroyCommandHandler:ICommandHandler
    {
        private Target _target;

        public DestroyCommandHandler(Target target)
        {
            _target = target;
        }

        public void Execute()
        {
            Console.WriteLine("Shoot bad people");
        }
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum Target
    {
        Rock,
        Alien,
        Sandile
    }

    public enum Material
    {
        Sand,
        Air,
        Water
    }
}
