#nullable enable
using System;

class Transaction
{
    public DateTime date;   // Make public so we can order by date
    double income;
    double expense;
    public bool isCredit = false;
    string? company = "";
    string? expenseCategory = "";
    string? notes = "";  // Any additional notes user wants to store about this transaction


    public string? Date
    {
        get
        {
            return date.ToShortDateString();
        }
        set
        {
            if (DateTime.TryParse(value, out date))
            {
            }
            else
            {
                Console.WriteLine("Invalid Date, Please try again.");
                date = DateTime.Now;
            }
        }

    }
    public double Income
    {
        get {return income;}
        set {income = value;}
    }
    public double Expense
    {
        get {return expense;}
        set {expense = value;}
    }

    public string? Company
    {
        get {return company;}
        set {company = value;}
    }

    public string? ExpenseCategory
    {
        get {return expenseCategory;}
        set {expenseCategory = value;}
    }

    public string? Notes
    {
        get {return notes;}
        set {notes = value;}
    }


    /*
    Expenses categories:
    01 Cost of goods bought for resale or goods used
    02 Construction industry – payments to subcontractors
    03 Wages, salaries and other staff costs
    04 Car, van and travel expenses
    05 Rent, rates, power and insurance costs
    06 Repairs and maintenance of property and equipment
    07 Phone, fax, stationery and other office costs
    08 Advertising and business entertainment costs
    09 Interest on bank and other loans
    10 Bank, credit card and other financial charges
    11 Irrecoverable debts written off
    12 Accountancy, legal and other professional fees
    13 Depreciation and loss or profit on sale of assets
    14 Other business expense
    */
    
    public Transaction(DateTime aDate, double aIncome, double aExpense, string? aCompany, string? aExpenseCategory, string? aNotes)
    {
        date = aDate;
        income = aIncome;
        expense = aExpense;

        if (income > 0)
            isCredit = true;
        
        company = aCompany;
        expenseCategory = aExpenseCategory;
        notes = aNotes;
    }
    public Transaction()
    {

    }

    public override string ToString()
    {
        /*
        if (isCredit)
            return date.ToString("d/M/yyyy") + " £" + income + "\t" + company + "\t" + notes ;
        else
            return date.ToString("d/M/yyyy") + " £" + expense + "\t" + company + "\t" + expenseCategory + "\t" + notes ;
        */
        return date.ToString("dd/MM/yyyy") + "  £" + income + "\t" + " £" + expense + "\t"+ company + "\t" + expenseCategory + "\t" + notes ;
    }
    public static void showExpenseCategories()
    {
        Console.WriteLine();
        Console.WriteLine("1  Cost of goods bought for resale or goods used");
        Console.WriteLine("2  Construction industry – payments to subcontractors");
        Console.WriteLine("3  Wages, salaries and other staff costs");
        Console.WriteLine("4  Car, van and travel expenses");
        Console.WriteLine("5  Rent, rates, power and insurance costs");
        Console.WriteLine("6  Repairs and maintenance of property and equipment");
        Console.WriteLine("7  Phone, fax, stationery and other office costs");
        Console.WriteLine("8  Advertising and business entertainment costs");
        Console.WriteLine("9  Interest on bank and other loans");
        Console.WriteLine("10 Bank, credit card and other financial charges");
        Console.WriteLine("11 Irrecoverable debts written off");
        Console.WriteLine("12 Accountancy, legal and other professional fees");
        Console.WriteLine("13 Depreciation and loss or profit on sale of assets");
        Console.WriteLine("14 Other business expense");
        Console.WriteLine();
    }

    public static string ReturnExpenseCategory(int categoryId)
    {
        switch (categoryId)
        {
            case 1:
                return "01 Cost of goods bought for resale or goods used";
            case 2:
                return "02 Construction industry – payments to subcontractors";
            case 3:
                return "03 Wages, salaries and other staff costs";
            case 4:
                return "04 Car, van and travel expenses";
            case 5:
                return "05 Rent, rates, power and insurance costs";
            case 6:
                return "06 Repairs and maintenance of property and equipment";
            case 7:
                return "07 Phone, fax, stationery and other office costs";
            case 8:
                return "08 Advertising and business entertainment costs";
            case 9:
                return "09 Interest on bank and other loans";
            case 10:
                return "10 Bank, credit card and other financial charges";
            case 11:
                return "11 Irrecoverable debts written off";
            case 12:
                return "12 Accountancy, legal and other professional fees";
            case 13:
                return "13 Depreciation and loss or profit on sale of assets";
            default:
                return "14 Other business expense";
        }
    }



}