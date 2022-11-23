#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace accounting
{
    class Program
    {
        //Transaction action = new Transaction();
        static string filename = "data_accounts.csv";

        static string EnterExpenseCategory()
        {
            Transaction.showExpenseCategories();
            Console.Write("Enter the Expenses category number: ");
            string? category = Console.ReadLine();
            int categoryID = Convert.ToInt32(category);
            return Transaction.ReturnExpenseCategory(categoryID);
        }

        static Transaction EnterData()
        {
            double income = 0;
            double expense = 0;
            string? company, expenseCategory, notes;
            DateTime today = DateTime.Now;
            DateTime date = today;
            Console.WriteLine("Adding new transaction");
            bool validDate = false;
            
            while (!validDate)
            {
                Console.Write("Enter date (Default is today {0}): ", today.ToString("d/M/yyyy"));
                string? userDate = Console.ReadLine();
                if (userDate == "")
                {
                    date = today;
                    validDate = true;
                }
                else
                    if (DateTime.TryParse(userDate, out date))
                    {
                        validDate = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Date, Please try again.");
                        date = today;
                    }
            }
            Console.Write("Enter income (Default is 0): ");
            string? userIncome = Console.ReadLine();
            if (userIncome == "")
                income = 0;
            else
                income = Convert.ToDouble(userIncome);
            
            if (income == 0)
            {
                Console.Write("Enter expense (Default is 0): ");
                string? userExpense = Console.ReadLine();
                if (userExpense == "")
                    expense = 0;
                else
                    expense = Convert.ToDouble(userExpense);
            }
            
            Console.Write("Enter Company / Individual with whom transaction takes place: ");
            company = Console.ReadLine();

            if (expense > 0)
                expenseCategory = EnterExpenseCategory();
            else
                expenseCategory = "";
            
            Console.Write("Write any notes about this transaction: ");
            notes = Console.ReadLine();

            Transaction action = new Transaction(date, income, expense, company, expenseCategory, notes);
            return action;
        }

        // https://stackoverflow.com/questions/7565415/edit-text-in-c-sharp-console-application
        static string? EditLine(string Default)
        {
            int pos = Console.CursorLeft;
            Console.Write(Default);
            ConsoleKeyInfo info;
            List<char> chars = new List<char> ();
            if (string.IsNullOrEmpty(Default) == false) {
                chars.AddRange(Default.ToCharArray());
            }

            while (true)
            {
                info = Console.ReadKey(true);
                if (info.Key == ConsoleKey.Backspace && Console.CursorLeft > pos)
                {
                    chars.RemoveAt(chars.Count - 1);
                    Console.CursorLeft -= 1;
                    Console.Write(' ');
                    Console.CursorLeft -= 1;

                }
                else if (info.Key == ConsoleKey.Enter) { Console.Write(Environment.NewLine); break; }
                //Here you need create own checking of symbols Letters, digits and punctuation
                else if (char.IsLetterOrDigit(info.KeyChar) || char.IsPunctuation(info.KeyChar) || char.IsWhiteSpace(info.KeyChar))
                {
                    Console.Write(info.KeyChar);
                    chars.Add(info.KeyChar);
                }
            }
            return new string(chars.ToArray ());
        }


        static void MainMenu(List<Transaction> actions)
        {
            // print the main menu system
            Console.WriteLine("\n\nWelcome to the accounting software");
            Console.WriteLine();
            Console.WriteLine("1. Add Entry");
            Console.WriteLine("2. Edit Entry");
            Console.WriteLine("3. View Records");
            Console.WriteLine("4. Save Records");
            Console.WriteLine("5. Calculate Profit and Loss");
            Console.WriteLine("6. Save and Exit");
            Console.WriteLine("7. Abandon all changes and Quit");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? userInput = Console.ReadLine();

            switch(userInput)
            {
                case "1":
                    AddEntry(actions);
                    break;
                case "2":
                    EditEntry(actions);
                    break;
                case "3":
                    ViewRecords(actions);
                    break;
                case "4":
                    SaveRecords(actions);
                    break;
                case "5":
                    CalculateProfitLoss(actions);
                    break;
                case "6":
                    Exit(actions);
                    break;
                case "7":
                    Quit();
                    break;
                default:
                    MainMenu(actions);
                    break;
            }
        }

        static void EditEntry(List<Transaction> actions)
        {
            double expense= 0;
            double income= 0;
            string? company = "";
            string? notes = "";
            string? expenseCategory = "";

            ViewRecords(actions);
            Console.Write("Which record do you wish to edit? ");
            string? userInput = Console.ReadLine();
            if (userInput != "")
            {
                int record = Convert.ToInt32(userInput);
                

                // Date
                Console.Write("Edit Date: ");
                DateTime oldDate = DateTime.Parse(actions[record].Date);
                string? newText = EditLine(actions[record].Date);
                DateTime date;
                if (DateTime.TryParse(newText, out date))
                {
                }
                else
                {
                    Console.WriteLine("Invalid Date, Reverting to original.");
                    date = oldDate;
                }
                Console.Write("Edit income: ");
                double oldIncome = actions[record].Income;
                string? newIncome =  EditLine(actions[record].Income.ToString());
                if (newIncome == "")
                    income = 0;
                else
                    income = Convert.ToDouble(newIncome);
                
                if (income == 0)
                {
                    
                    double oldExpense = actions[record].Expense;
                    Console.Write("Edit expense: ");
                    string? newExpense = EditLine(actions[record].Expense.ToString());
                    if (newExpense == "")
                        expense = 1;
                    else
                        expense = Convert.ToDouble(newExpense);
                }
                
                Console.Write("Edit Company / Individual with whom transaction takes place: ");
                company = EditLine(actions[record].Company);

                if (expense > 0)
                {
                    Console.WriteLine(actions[record].ExpenseCategory);
                    Console.Write("Do you wish to change the expense category? (y/n) ");
                    string? userChoice = Console.ReadLine();
                    if (userChoice.ToLower() =="y")
                        expenseCategory = EnterExpenseCategory();
                    else
                        expenseCategory = actions[record].ExpenseCategory;
                }
                else
                    expenseCategory = "";
                
                Console.Write("Edit notes about this transaction: ");
                notes = EditLine(actions[record].Notes);
                
                // Delete old entry
                actions.RemoveAt(record);
                Transaction newRecord = new Transaction(date, income, expense, company, expenseCategory, notes);
                actions.Add(newRecord);
            }
        }


        static List<Transaction> SortList(List<Transaction> actions)
        {
            List<Transaction> SortedList = actions.OrderBy(o=>o.date).ToList();
            return SortedList;
        }

        static void ViewRecords(List<Transaction> actions)
        {
            int counter = 0;    // Makes it easier to choose an item :-)
            // Show the data in date order
            Console.WriteLine("\n\n----------------------------------\n");
            var transactions = from a in actions orderby a.date select a;
            foreach (var item in transactions)
            {
                Console.WriteLine(counter.ToString() + "  " +  item);
                counter++;
            }
            Console.WriteLine("\n----------------------------------\n\n");
        }

        static void AddEntry(List<Transaction> actions)
        {
            bool moreData = true;
            while (moreData)
            {
                Transaction action = EnterData();
                //Console.WriteLine(action);
                actions.Add(action);
                Console.Write("Add more data (y/n)? ");
                string? userRequest = Console.ReadLine();
                if (userRequest == "y" || userRequest == "Y")
                    moreData = true;
                else
                    moreData = false;
            }
        }

        static void SaveRecords(List<Transaction> actions)
        {
            // We will sort the list of transactions by date before saving it.
            actions = SortList(actions);

            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                Encoding = Encoding.UTF8
            };

            using (var writer = new StreamWriter(filename))
            using (var csvWriter = new CsvWriter(writer, csvConfig))
            {
                csvWriter.WriteRecords(actions);
            }
        }


        static void CalculateProfitLoss(List<Transaction> actions)
        {
            double totalIncome = 0;
            double totalExpense = 0;
            foreach (var element in actions)
            {
                totalIncome = totalIncome + element.Income;
                totalExpense = totalExpense + element.Expense;
            }
            Console.WriteLine("\n\nTotal Income:  {0}", totalIncome);
            Console.WriteLine("Total Expense: {0}", totalExpense);
            Console.WriteLine("-------------------\n");
            if ((totalIncome - totalExpense) > 0)
                Console.WriteLine("Profit for the year: {0}",totalIncome - totalExpense);
            else if ((totalIncome - totalExpense) < 0)
                Console.WriteLine("Loss for the year: {0}",Math.Abs(totalIncome - totalExpense));
            else
                Console.WriteLine("Break Even");
            Console.Write("\nPress any key to continue");
            Console.ReadKey();
        }

        static void Exit(List<Transaction> actions)
        {
            SaveRecords(actions);
            System.Environment.Exit(0);
        }
        static void Quit()
        {
            System.Environment.Exit(1);
        }
        

        static void Main(string[] args)
        {
            // Initialise everything
            var actions = new List<Transaction>();
            // Load data from the disk
            
            if (File.Exists(filename))
            {
                Console.WriteLine("File loaded");
              /*
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ",",
                    Encoding = Encoding.UTF8
                };
                */
                var record = new Transaction();
                using (var reader = new StreamReader(filename))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    actions = csv.GetRecords<Transaction>().ToList();
                }

            }
            while (true)
                MainMenu(actions);

        }
    }
}
