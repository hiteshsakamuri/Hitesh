using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Accounts
{
    public enum AccountType
    {
        Checking,
        Saving,
        Visa,
        Line_of_credit
    }

    public abstract class Account
    {
        private static int LAST_NUMBER = 100_000;
        protected readonly List<Person> users = new List<Person>();
        public readonly List<Transaction> transactions = new List<Transaction>();

        public string Number { get; }
        public double Balance { get; protected set; }
        public double LowestBalance { get; protected set; }

        public event EventHandler OnLogin;       // Event for login attempts
        public event EventHandler OnTransaction; // Event for transactions

        protected Account(string type, double balance)
        {
            Number = $"{type}{LAST_NUMBER++}";
            Balance = balance;
            LowestBalance = balance;
        }

        public void Deposit(double amount, Person person)
        {
            Balance += amount;
            if (Balance < LowestBalance)
            {
                LowestBalance = Balance;
            }

            var transaction = new Transaction(Number, amount, person, Utils.Now);
            transactions.Add(transaction);

            // Trigger the OnTransaction event
            //OnTransactionOccur(this, new TransactionEventArgs(person.Name, amount, true));
        }

        public void AddUser(Person person)
        {
            users.Add(person);
        }

        public bool IsUser(string name)
        {
            foreach (var user in users)
            {
                if (user.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void OnTransactionOccur(object sender, EventArgs args)
        {
            OnTransaction?.Invoke(sender, args);
        }

        public abstract void PrepareMonthlyStatement();

        //public override string ToString()
        //{
        //    string userNames = string.Join(", ", users);
        //    string transactionDetails = string.Join(Environment.NewLine, transactions);
        //    return $"{Number} {userNames} ${Balance:F2}" +
        //           Environment.NewLine + transactionDetails;
        //}

        public override string ToString()
        {
            string userNames = string.Join(", ", users);
            string transactionDetails = string.Join(Environment.NewLine + "   ", transactions);

            string result = $"{Number} {userNames} ${Balance:F2}";

            if (transactions.Count > 0)
            {
                result += "\n   " + transactionDetails;
            }

            return result;
        }
    }

}
