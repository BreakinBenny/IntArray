using MoneyTrackClasses;

MoneyTrack moneytrack = new MoneyTrack(0, 0, 0);
List<Transaction> transactions = new List<Transaction>();
double accBalance = 0;

// This is a simple console application for tracking money.
// It allows the user to view their transaction history, add new
// transactions, edit existing transactions, and save and quit the application.
Console.WriteLine("Welcome to the Money Tracker! How can we help you?");

Console.Write("What extension does your transaction history file have? (JSON, CSV or TXT. Empty or other argument to skip): ");
string extension = Console.ReadLine()?.Trim().ToLowerInvariant();
if ((!string.IsNullOrEmpty(extension)) && extension == "csv" || extension == "json" || extension == "txt")
{
	try
	{
		using (StreamReader reader = new StreamReader(".\\transactionHistory." + extension))
		{
            /*
			loadedTransaction.Recipent =
			
			loadedTransaction.Recipent = null; loadedTransaction.In_Or_Out = null; loadedTransaction.Amount = 0;
			loadedTransaction.Category = null; loadedTransaction.Date = "2020-06-04";
			*/
            string line;
			while ((line = reader.ReadLine()) != null)
            {
                Transaction loadedTransaction = new Transaction(null, null, 0, null, null);
                string[] fields = line.Split(',');
				byte index = 0;
				foreach (var field in fields)
				{
                    loadedTransaction.Recipent = fields[0];
					loadedTransaction.In_Or_Out = fields[1];
					loadedTransaction.Amount = Convert.ToDouble(fields[2]);
					loadedTransaction.Category = fields[3];
					loadedTransaction.Date = fields[4];
					index++;
                    //Console.Write(field + ", ");
                    //loadedTransaction.
                }
                transactions.Add(loadedTransaction);
                //Console.ReadLine();
				//line.Split(',');
				//fields[0] = line.Substring(0); fields[1] = line.Substring(1); fields[2] = line.Substring(2);
				//fields[3] = line.Substring(3); fields[4] = line.Substring(4);
				//loadedTransaction.Recipent = fields[0];
				//loadedTransaction.In_Or_Out = fields[1];
				//loadedTransaction.Amount = Convert.ToDouble(fields[2]);
				//loadedTransaction.Category = fields[3];
				//loadedTransaction.Date = fields[4];
				//Console.WriteLine($"Fields: {fields[0]} {fields[1]} {fields[2]} {fields[3]} {fields[4]}");

				//transactions.Recipent = reader.Get
			}
		}
	}
	catch
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"transactionHistory.{extension} does not exist! Skipping...");
		Console.ResetColor();
	}
}

Console.Write("Would you like to set a starting balance before we begin? (enter Y/N and confirm with Enter)\n");
TITLESCREEN:
string userInput = Console.ReadLine()?.Trim().ToLowerInvariant();
if (userInput == "y")
{
	Console.WriteLine("\nThen please input your starting balance; it must be greater than zero.");
	INPUT:
	accBalance = Convert.ToDouble(Console.ReadLine());
	if (accBalance <= 0)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine("Sorry, that is not a valid starting balance. Please try again.");
		Console.ResetColor();
		goto INPUT;
	}
	else
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine("Great, your starting balance is: " + accBalance);
		Console.ResetColor();
	}
}
else if (userInput == "n")
{
	accBalance += 10;
	Console.ForegroundColor = ConsoleColor.Magenta;
	Console.WriteLine("No problem, we will give you starting a balance of: " + accBalance);
	Console.ResetColor();
}
else
{
	Console.WriteLine("Input invalid, try again.");
	goto TITLESCREEN;
}

Console.WriteLine($"\nYour current account balance is {accBalance}. How may we help you?");
START:
Console.WriteLine("1: Show expense(s), income(s), or both\n2: Add a new expense or income\n3: Edit or remove an existing expense/income\n4: Save and Quit");
char charInput = Console.ReadKey().KeyChar;
switch (charInput)
{
	case '1':
		Console.Write("\nYou chose to view your transaction history.");
		// Code to view transaction history goes here
		if (transactions.Count < 1)
		{
			Console.Write(".. but you have no transactions at present.\n\n");
			goto START;
		}
		Console.Write(" Which would you like to see?\n1: Expenses\n2: Incomes\n3: Expenses and Incomes\n");
		IEnumerable<Transaction> SortedTransactions;
	EXP_AND_INCOM:
		charInput = Console.ReadKey().KeyChar;
		Console.WriteLine();
		switch (charInput)
		{
			case '1':
				// Code to display expenses goes here
				SortedTransactions = transactions.Where(t => string.Equals(t.In_Or_Out, "Expense", StringComparison.OrdinalIgnoreCase));
				break;
			case '2':
				// Code to display incomes goes here
				SortedTransactions = transactions.Where(t => string.Equals(t.In_Or_Out, "Income", StringComparison.OrdinalIgnoreCase));
				break;
			case '3':
				// Code to display both expenses and incomes goes here
				SortedTransactions = transactions;
				Console.WriteLine("Both expenses and incomes.");
				break;
			default:
				Console.WriteLine("Sorry, we don't understand that command. Please try again.");
				goto EXP_AND_INCOM;
		}

		if (!SortedTransactions.Any())
		{
			Console.WriteLine("... But none match your selection.\n");
			Console.ReadLine();
			Console.Clear();
			break;
		}

		Console.Write("What would you like to sort by?\n1: Recipent\n2: Amount\n3: Category\n4: Date\n");
		char sortChoice = Console.ReadKey().KeyChar;
		Console.WriteLine("\nAnd in what order?\n1: Ascending\n2: Descending\n");
		char orderChoice = Console.ReadKey().KeyChar;
		bool descending = orderChoice == '2';

		IOrderedEnumerable<Transaction> orderedTransactions;
		switch (sortChoice)
		{
			case '1': // sort by recipent
				orderedTransactions = descending ? SortedTransactions.OrderByDescending(t => t.Recipent): SortedTransactions.OrderBy(t => t.Recipent);
				break;
			case '2': // amount
				orderedTransactions = descending ? SortedTransactions.OrderByDescending(t => t.Amount) : SortedTransactions.OrderBy(t => t.Amount);
				break;
			case '3': // category
				orderedTransactions = descending ? SortedTransactions.OrderByDescending(t => t.Category) : SortedTransactions.OrderBy(t => t.Category);
				break;
			case '4': // date
				orderedTransactions = descending ? SortedTransactions.OrderByDescending(t => t.Date) : SortedTransactions.OrderBy(t => t.Date);
				break;
			default: // then let's just sort by date ascending
				orderedTransactions = SortedTransactions.OrderBy(t => t.Date);
				break;
		}

		SortedTransactions = orderedTransactions.ToList();
		if (!SortedTransactions.Any())
			Console.WriteLine("... But none match your selection.\n");
		else
		{
			foreach (var transaction in SortedTransactions)
			{
				Console.WriteLine($"\nRecipent: {transaction.Recipent}\nAmount: {transaction.Amount}\nCategory: {transaction.Category}\nDate: {transaction.Date}\n");
			}
		}
		
		Console.ReadLine();
		Console.Clear();
		break;
	case '2':
/*		if (accBalance == 0)
		{   //No more transactions when broke!
			Console.WriteLine("You have no money left to make transactions with, cancelling...");
			goto START;
		}
*/		Console.Clear();
		Console.WriteLine("You chose to add a new transaction.");
		// Code for adding a new transaction (like expense or income) goes here
		var newTransaction = new Transaction(null, null, 0, null, null);
		accBalance = newTransaction.AddTransaction(transactions, newTransaction, accBalance);

		break;
	case '3':
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Write("\nYou chose to edit an item (edit or remove).");
		// Code for editing an item goes here
		if (transactions.Count < 1)
		{
			Console.Write(".. but there are no transactions at present.\n\n");
			Console.ResetColor();
			break;
		}
		Console.ResetColor();

		Console.WriteLine(" Enter the number or ID of transaction you wish to edit.");
		string indexInput = Console.ReadLine()?.Trim();
		if (sbyte.TryParse(indexInput, out sbyte indexTransaction))
		{   // Let's edit transaction # int (or byte)
			indexTransaction--;
			if (indexTransaction > transactions.Count)
			{
				indexTransaction = Convert.ToSByte(transactions.Count);
				indexTransaction--;
			}
			else if (indexTransaction < 0)
			{
				indexInput = "1";
				indexTransaction = 0;
			}
			Console.Clear();
			try
			{
				Console.WriteLine($"Editing transaction #{indexInput}: {transactions[indexTransaction].Amount} as {transactions[indexTransaction].In_Or_Out} for {transactions[indexTransaction].Category} to {transactions[indexTransaction].Recipent}");
				Console.WriteLine("What would you like to change in the transaction? (Press 'e' to cancel', or 'd' to remove it.)\n1: Recipent\n2: Amount\n3: Category\n");
				char editInput = Console.ReadKey().KeyChar;
				switch (editInput)
				{
					case '1':
						Console.WriteLine($"\nThe recipent. Who was this transaction with? (Currently {transactions[indexTransaction].Recipent})");
						transactions[indexTransaction].Recipent = Console.ReadLine()?.Trim();
						break;
					case '2':
						Console.WriteLine($"\nThe amount. How much did the recipent receive? (Must be positive and no more than your current balance.)\nCurrently {transactions[indexTransaction].Amount}");
						transactions[indexTransaction].Amount = Convert.ToDouble(Console.ReadLine()?.Trim());
						break;
					case '3':
						Console.WriteLine($"\nThe category (currently '{transactions[indexTransaction].Category}'). What was this transaction for? (Bills, loans, damages...): ");
						transactions[indexTransaction].Category = Console.ReadLine()?.Trim();
						break;
					case 'd':
						Console.WriteLine($"\nPlease confirm that you wish to remove transaction #{indexTransaction + 1} (Y/N)");
						editInput = Console.ReadKey().KeyChar;
						switch (editInput)
						{
							case 'y':
								transactions.RemoveAt(indexTransaction);
								Console.WriteLine("\nTransaction removed.");
								break;
							default:
							case 'n':
								Console.WriteLine("\nCancelling...");
								break;
						}
						break;
					case 'e':
					default:
						Console.WriteLine("\nCancelling...");
						break;
				}
			}
			catch (ArgumentOutOfRangeException)
			{
				Console.WriteLine("Out of range, cancelling...");
			}
		}
		break;
	case '4':
		if (transactions.Count < 1)
		{
			Console.WriteLine("\nYou have no transactions to save, but we will still quit the application for you. Goodbye!");
			return;
		}

		Console.WriteLine("\nYou chose to save and quit.");
		
		// Code to save and quit the application goes here
		Console.Write("What will you name the file containing the transaction history, and save as? (.csv, .json or .txt): ");

		string saveInput = Console.ReadLine()?.Trim();
		if (!string.IsNullOrWhiteSpace(saveInput))
			SaveToFile.ExportTransactionHistory(transactions, saveInput);
		else
			Console.WriteLine("Can not save to unnamed file, cancelling...");
		// Saving as file, then StreamWriter writes the transactions to the file. If .json, then we will save as JSON, if .txt, then we will save as plain text.
		//StreamWriter savedTransaction = new StreamWriter($".\\{saveInput}");
		//savedTransaction.WriteLine($"==Transaction History for Money Tracker - Saved on {DateTime.Now:g}==\n");
		//transactions.ForEach(t => savedTransaction.WriteLine($"===Transaction {transactions}"));

		return;
	default:
		Console.WriteLine("\nSorry, we don't understand that command. Please try again.");
		goto START;
}
Console.WriteLine($"You now have {accBalance} left, how else may we help you?");
goto START;