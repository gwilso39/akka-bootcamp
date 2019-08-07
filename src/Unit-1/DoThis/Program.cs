using Akka.Actor;
using System;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            // initialize MyActorSystem
            MyActorSystem = ActorSystem.Create("MyActorSystem");
            Console.WriteLine("\n MyActorSystem has been created\n");
                        
            //PrintInstructions();

            // time to make your first actors!
            //var consoleWriterActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()));
            //Console.WriteLine("\n consoleWriterActor has been instantiated.  Definition previously created\n");
            //var consoleReaderActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriterActor)));
            //Console.WriteLine("\n consoleReaderActor has been instantiated.  Definition previously created\n");
            
            Props consoleWriterProps = Props.Create<ConsoleWriterActor>();
            IActorRef consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

            Props validationActorProps = Props.Create(() => new ValidationActor(consoleWriterActor));
            IActorRef validationActor = MyActorSystem.ActorOf(validationActorProps, "validationActor");

            Props consoleReaderProps = Props.Create<ConsoleReaderActor>(validationActor);
            IActorRef consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");

            // tell console reader to begin
            Console.WriteLine("\n telling the readerActor to begin...........\n");
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();

        }

        //private static void PrintInstructions()
        //{
        //    Console.WriteLine("Write whatever you want into the console!");
        //    Console.Write("Some lines will appear as");
        //    Console.ForegroundColor = ConsoleColor.DarkMagenta;
        //    Console.Write(" magenta ");
        //    Console.ResetColor();
        //    Console.Write(" and others will appear as");
        //    Console.ForegroundColor = ConsoleColor.Blue;
        //    Console.Write(" NOT green! ");
        //    Console.ResetColor();
        //    Console.WriteLine();
        //    Console.WriteLine();
        //    Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        //}
    }
    #endregion
}
