using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public static class Logger
    {
        private static List<string> loginEvents = new List<string>();
        private static List<string> transactionEvents = new List<string>();

        public static void LoginHandler(object sender, EventArgs args)
        {
            if (args is LoginEventArgs loginArgs)
            {
                string log = $"{loginArgs.PersonName} {(loginArgs.Success ? "logged in successfully" : "failed to log in")} at {Utils.ToString(Utils.Now)}";
                loginEvents.Add(log);
            }
        }

        public static void TransactionHandler(object sender, EventArgs args)
        {
            if (args is TransactionEventArgs transactionArgs)
            {
                string operation = transactionArgs.Amount > 0 ? "Deposit" : "Withdraw";
                string log = $"{transactionArgs.PersonName} {operation} {Math.Abs(transactionArgs.Amount):C} {(transactionArgs.Success ? "successful" : "failed")} at {Utils.ToString(Utils.Now)}";
                transactionEvents.Add(log);
            }
        }

        public static void SaveLoginEvents(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine($"Login events as of {Utils.ToString(Utils.Now)}");
                for (int i = 0; i < loginEvents.Count; i++)
                {
                    writer.WriteLine($"{i + 1}. {loginEvents[i]}");
                }
            }
        }

        public static void SaveTransactionEvents(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine($"Transaction events as of {Utils.ToString(Utils.Now)}");
                for (int i = 0; i < transactionEvents.Count; i++)
                {
                    writer.WriteLine($"{i + 1}. {transactionEvents[i]}");
                }
            }
        }
    }
}
