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

            var commandDispatcher = new CommandDispatcher();
            var queryDispatcher = new QueryDispatcher();

            var results = queryDispatcher.DispatchQuery<MoveQuery,List<Direction>>(new MoveQuery());

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }

            commandDispatcher.DispatchCommand(new CollectCommand(Material.Air));
            commandDispatcher.DispatchCommand(new MoveCommand(Direction.Down));
            commandDispatcher.DispatchCommand(new DestroyCommand(Target.Sandile));
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

    public class DestroyCommand : ICommand
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
        void DispatchCommand<T>(T command) where T : ICommand;
    }

    public class CommandDispatcher : ICommandDispatcher
    {
        public void DispatchCommand<T>(T command) where T : ICommand
        {
            var handlerTypes = GetHandlers(command.GetType());

            foreach (var handlerType in handlerTypes)
            {
                var handler = (ICommandHandler<T>)Activator.CreateInstance(handlerType);

                handler.Execute(command);
            }
        }

        private IEnumerable<Type> GetHandlers(Type commandType)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .Where(p => p.GetInterfaces().Any(ip => ip.IsGenericType &&
                    ip.GetGenericTypeDefinition() == typeof(ICommandHandler<>) && ip.GetGenericArguments().Contains(commandType)));
        }
    }

    public class MoveQuery: IQuery<List<Direction>>
    {
        public Direction Direction { get; set; } 
    }

    public class MoveQueryHandler : IQueryHandler<MoveQuery, List<Direction>>
    {
        public List<Direction> Execute(MoveQuery query)
        {
            return new List<Direction>() { Direction.Up, Direction.Down };
        }
    }

    public interface IQuery<TResult>
    {
    }

    public interface IQueryHandler<in TQuery, out TResult> where TQuery : IQuery<TResult>
    {
        TResult Execute(TQuery query);
    }

    public class QueryDispatcher
    {
        public TResult DispatchQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var handlerType = GetHandlers(query.GetType()).First();
            var handler = (IQueryHandler<TQuery, TResult>)Activator.CreateInstance(handlerType);
            return handler.Execute(query);
        }

        private IEnumerable<Type> GetHandlers(Type queryType)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .Where(p => p.GetInterfaces().Any(ip => ip.IsGenericType &&
                    ip.GetGenericTypeDefinition() == typeof(IQueryHandler<,>) && ip.GetGenericArguments().Contains(queryType)));
        }
    }
}