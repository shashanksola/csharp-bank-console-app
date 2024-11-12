using System;
using System.Collections.Generic;

namespace ConsoleBankingApp
{
    class User // to manage user details
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public List<Account> Accounts { get; private set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Accounts = new List<Account>();
        }
    }

    class Account // to manage account and transaction details
    {
        private static int _accountNumberSeed = 100000;
        public int AccountNumber { get; private set; }
        public string AccountHolder { get; private set; }
        public string AccountType { get; private set; }
        public decimal Balance { get; private set; }
        private List<Transaction> Transactions;
        private static decimal InterestRate = 0.01m; // 1% monthly interest

        public Account(string accountHolder, string accountType, decimal initialDeposit)
        {
            AccountNumber = _accountNumberSeed++;
            AccountHolder = accountHolder;
            AccountType = accountType;
            Balance = initialDeposit;
            Transactions = new List<Transaction>();
            Transactions.Add(new Transaction("Initial Deposit", initialDeposit));
        }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                Transactions.Add(new Transaction("Deposit", amount));
                Console.WriteLine("Deposit successful.");
            }
            else
            {
                Console.WriteLine("Invalid deposit amount.");
            }
        }

        public void Withdraw(decimal amount)
        {
            if (amount > 0 && Balance >= amount)
            {
                Balance -= amount;
                Transactions.Add(new Transaction("Withdrawal", amount));
                Console.WriteLine("Withdrawal successful.");
            }
            else
            {
                Console.WriteLine("Insufficient funds or invalid amount.");
            }
        }

        public void PrintStatement()
        {
            Console.WriteLine($"\nAccount Statement for Account No: {AccountNumber}");
            Console.WriteLine("Date\t\t\tType\t\tAmount");
            foreach (var transaction in Transactions)
            {
                Console.WriteLine($"{transaction.Date}\t{transaction.Type}\t\t{transaction.Amount}");
            }
            Console.WriteLine($"Current Balance: {Balance}\n");
        }

        public void AddMonthlyInterest()
        {
            if (AccountType == "Savings")
            {
                decimal interest = Balance * InterestRate;
                Deposit(interest); // treating interest as deposit
                Console.WriteLine($"Monthly interest of {interest} added.");
            }
        }

        public void CheckBalance()
        {
            Console.WriteLine($"Account Balance: {Balance}");
        }
    }

    class Transaction // to manage and store transaction details
    {
        public string Type { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }

        public Transaction(string type, decimal amount)
        {
            Type = type;
            Amount = amount;
            Date = DateTime.Now;
        }
    }

    class Program
    {
        private static List<User> users = new List<User>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nWelcome to Console Banking App");
                Console.WriteLine(" _______       __      _____  ___   __   ___  ");
                Console.WriteLine("|   _  \"\\     /\"\"\\    (\"   \\|\"  \\ |/\"| /  \") ");
                Console.WriteLine("(. |_)  :)   /    \\   |.\\\\   \\    |(: |/   /  ");
                Console.WriteLine("|:     \\/   /' /\\  \\  |: \\.   \\\\  ||    __/   ");
                Console.WriteLine("(|  _  \\\\  //  __'  \\ |.  \\    \\. |(// _  \\   ");
                Console.WriteLine("|: |_)  :)/   /  \\\\  \\|    \\    \\ ||: | \\  \\  ");
                Console.WriteLine("(_______/(___/    \\___)\\___|\\____\\)(__|  \\__) ");
                Console.WriteLine("1. Register\n2. Login\n3. Exit");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterUser();
                        break;
                    case "2":
                        LoginUser();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static void RegisterUser()
        {
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            users.Add(new User(username, password));
            Console.WriteLine("User registered successfully.");
        }

        static void LoginUser()
        {
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            User user = users.Find(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                Console.WriteLine("Login successful.");
                UserMenu(user);
            }
            else
            {
                Console.WriteLine("Invalid credentials.");
            }
        }

        static void UserMenu(User user)
        {
            while (true)
            {
                Console.WriteLine("\nUser Menu");
                Console.WriteLine("1. Open Account\n2. Deposit\n3. Withdraw\n4. Check Balance\n5. Print Statement\n6. Add Monthly Interest\n7. Logout");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        OpenAccount(user);
                        break;
                    case "2":
                        MakeDeposit(user);
                        break;
                    case "3":
                        MakeWithdrawal(user);
                        break;
                    case "4":
                        CheckBalance(user);
                        break;
                    case "5":
                        PrintStatement(user);
                        break;
                    case "6":
                        AddInterest(user);
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static void OpenAccount(User user)
        {
            Console.Write("Enter Account Holder's Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Account Type (Savings/Checking): ");
            string type = Console.ReadLine();
            Console.Write("Enter Initial Deposit: ");
            decimal initialDeposit = decimal.Parse(Console.ReadLine());

            Account account = new Account(name, type, initialDeposit);
            user.Accounts.Add(account);
            Console.WriteLine($"Account opened successfully. Account Number: {account.AccountNumber}");
        }

        static void MakeDeposit(User user)
        {
            Account account = SelectAccount(user);
            if (account != null)
            {
                Console.Write("Enter amount to deposit: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                account.Deposit(amount);
            }
        }

        static void MakeWithdrawal(User user)
        {
            Account account = SelectAccount(user);
            if (account != null)
            {
                Console.Write("Enter amount to withdraw: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                account.Withdraw(amount);
            }
        }

        static void CheckBalance(User user)
        {
            Account account = SelectAccount(user);
            if (account != null)
            {
                account.CheckBalance();
            }
        }

        static void PrintStatement(User user)
        {
            Account account = SelectAccount(user);
            if (account != null)
            {
                account.PrintStatement();
            }
        }

        static void AddInterest(User user)
        {
            Account account = SelectAccount(user);
            if (account != null)
            {
                account.AddMonthlyInterest();
            }
        }

        static Account SelectAccount(User user)
        {
            Console.Write("Enter Account Number: ");
            int accountNumber = int.Parse(Console.ReadLine());
            Account account = user.Accounts.Find(a => a.AccountNumber == accountNumber);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
            }
            return account;
        }
    }
}