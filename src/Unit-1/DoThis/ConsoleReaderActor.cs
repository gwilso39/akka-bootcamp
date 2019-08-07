using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Terminate"/>.
    /// </summary>
    class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        public const string StartCommand = "start";
        private readonly IActorRef _validationActor;

        public ConsoleReaderActor(IActorRef validationActor)
        {
            _validationActor = validationActor;
        }

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                DoPrintInstructions();
            }
            //else if (message is Messages.InputError)
            //{
            //    _consoleWriterActor.Tell(message as Messages.InputError);
            //}

            GetAndValidateInput();

            //if (message is Messages.InputError)
            //{
            //    var msg = message as Messages.InputError;
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine(msg.Reason);
            //}
            //else
            //{
            //    Unhandled(message);
            //}

            //var read = Console.ReadLine();
            ////Console.WriteLine($"this is what I read: {message}");
            //if (!string.IsNullOrEmpty(read) && String.Equals(read, ExitCommand, StringComparison.OrdinalIgnoreCase))
            //{
            //    // shut down the system (acquire handle to system via
            //    // this actors context)
            //    Context.System.Terminate();
            //    return;
            //}


            //// send input to the console writer to process and print
            //_consoleWriterActor.Tell(read);

            //// continue reading messages from the console
            //Self.Tell("continue");
        }

        #region Internal Methods

        private void DoPrintInstructions()
        {
            Console.WriteLine("Write whatever you want to the console!");
            Console.WriteLine("Some entries will pass validation, and some will NOT...\n\n");
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }

        /// <summary>
        /// reads input from console, validates it, then signals appropriate response
        /// (continu processing, error, success, etc.).
        /// </summary>
        private void GetAndValidateInput()
        {
            var message = Console.ReadLine();
            if (!string.IsNullOrEmpty(message) && String.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                //if user typed ExitCommand, shut down the entire actor system (allows the process to exit)
                Context.System.Terminate();
                return;

                //Self.Tell(new Messages.NullInputError("No input received."));
            }
            //else if (String.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            //{
            //    //shut down the entire actor system (allows process to exit)
            //    Context.System.Terminate();
            //}
            //else
            //{
            //    var valid = IsValid(message);
            //    if (valid)
            //    {
            //        _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you! Message was valid."));

            //        // continue reading messages from console
            //        Self.Tell(new Messages.ContinueProcessing());
            //    }
            //    else
            //    {
            //        Self.Tell(new Messages.ValidationError("Invalid: input had odd number of characters."));
            //    }
            //}
            // otherwise, just hand message off to validation actor (by telling its actor ref)
            _validationActor.Tell(message);
        }

        /// <summary>
        /// Validates <see cref="message"/>.
        /// Currently says messages are valid if contain even number of characters.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
 
        //private static bool IsValid(string message)
        //{
        //    var valid = message.Length % 2 == 0;
        //    return valid;
        //}
        #endregion
    }
}