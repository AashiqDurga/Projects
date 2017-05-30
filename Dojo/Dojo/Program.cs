using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
            var destroyCommand = new DestroyCommand(Target.Sandile);
            var destroyCommandHandler = new DestroyCommandHandler();
            var commandDispatcher = new CommandDispatcher();
            
            collectCommandHandler.Execute(collectCommand);
            commandDispatcher.DispatchCommand(moveCommand);
            destroyCommandHandler.Execute(destroyCommand);
        }
    }

    public interface ICommandHandler<T>
    {
        void Execute(T command);
    }

    public class MoveCommandHandler : ICommandHandler<MoveCommand>
    {
        public void Execute(MoveCommand moveCommand)
        {
            Console.WriteLine(moveCommand.Direction);
        }
    }

    public class CollectCommandHandler : ICommandHandler<CollectCommand>
    {
        public void Execute(CollectCommand collectCommand)
        {
            Console.WriteLine($"Collecting {collectCommand.Material}");
        }
    }

    public class DestroyCommandHandler : ICommandHandler<DestroyCommand>
    {
        public void Execute(DestroyCommand destroyCommand)
        {
            Console.WriteLine(destroyCommand.Target);
        }
    }

    public class DestroyCommand
    {
        public DestroyCommand(Target target)
        {
            Target = target;
        }

        public Target Target { get; set; }
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

    public interface ICommandDispatcher
    {
        void DispatchCommand(ICommand command);
    }

    public class CommandDispatcher : ICommandDispatcher
    {
        public void DispatchCommand(ICommand command)
        {
            //ToDo: Find all types that implement ICommandHandler
            var type = typeof(ICommandHandler<>);
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .Where(p => p.GetInterfaces().Any(ip=> ip.IsGenericType &&
                    ip.GetGenericTypeDefinition() == type && ip.GetGenericArguments().Contains(command.GetType())) );


            //types.FirstOrDefault(x => x.GenericTypeArguments)
            if (command is MoveCommand)
            {
                var commandHandler = new MoveCommandHandler();
                commandHandler.Execute((MoveCommand)command);
            }
        }
    }
}