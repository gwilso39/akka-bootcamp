using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor that validates user input and signals result to other.
    /// </summary>
    public class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                // signal that the useer needs to supply and input
                _consoleWriterActor.Tell(new Messages.NullInputError("No input received dork!"));
            }
            else
            {
                var valid = IsValid(msg);
                if (valid)
                {
                    // sedn success to console writer
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you! Message was valid."));
                }
                else
                {
                    // signal that the input was bad
                    _consoleWriterActor.Tell(new Messages.ValidationError("Invalid: input had an odd number of characters."));
                }
            }

            // tell sender to continue doing it's thing
            // (whatever that may be, this actor doesn't care)
            Sender.Tell(new Messages.ContinueProcessing());
        }

        /// <summary>
        /// Determines if the message received is valid.
        /// Checks if the number of chars in message received is even.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        
        private static bool IsValid(string msg)
        {
            var valid = msg.Length % 2 == 0;
            return valid;
        }
    }
}
