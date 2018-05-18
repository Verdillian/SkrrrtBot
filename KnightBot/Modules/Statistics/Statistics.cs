using System;
using System.Collections.Generic;
using System.Text;

namespace SkrrrtBot.Modules.Statistics
{
    public class Statistics
    {
        private static int incomingMessages, outgoingMessages, commandRequests, errorsDetected, profanityDetected;

        public static int GetIncomingMessages() { return incomingMessages; }
        public static int GetOutgoingMessages() { return outgoingMessages; }
        public static int GetCommandRequests() { return commandRequests; }
        public static int GetErrorsDetected() { return errorsDetected; }
        public static int GetProfanityDetected() { return profanityDetected; }

        public static void AddIncomingMessages() { incomingMessages++; }
        public static void AddOutgoingMessages() { outgoingMessages++; }
        public static void AddCommandRequests() { commandRequests++; }
        public static void AddErrorsDetected() { errorsDetected++; }
        public static void AddProfanityDetected() { profanityDetected++; }

        public static void ResetIncomingMessages() { incomingMessages = 0; }
        public static void ResetOutgoingMessages() { outgoingMessages = 0; }
        public static void ResetCommandRequests() { commandRequests = 0; }
        public static void ResetErrorsDetected() { errorsDetected = 0; }
        public static void ResetProfanityDetected() { profanityDetected = 0; }
    }
}
