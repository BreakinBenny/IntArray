using System.Text.Json;

namespace MoneyTrackClasses
{
	internal class MoneyTrack
	{
		public MoneyTrack()
		{
		}

		public MoneyTrack(double amount, double loans, double debts)
		{
			Amount = amount;
			Loans = loans;
			Debts = debts;
		}

		public double Amount { get; set; }
		public double Loans { get; set; }
		public double Debts { get; set; }

		public string GetAmount()
		{
			return Amount.ToString();
		}
		public string GetLoans()
		{
			return Loans.ToString();
		}
		public string GetDebts()
		{
			return Debts.ToString();
		}
		public string GetDueDate()
		{
			DateTime dueDate = DateTime.Now.AddDays(30);
			return dueDate.ToString("MMMM dd, yyyy");
		}

		public string ViewTransactionHistory(double amount, double loans, double debts, string dueDate)
		{
			Console.WriteLine("Your current account balance is: ");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write($"{Amount}, ");
			Console.ResetColor();
			Console.Write(" and you have ");
			Console.ForegroundColor = ConsoleColor.DarkMagenta;
			Console.Write($"{Loans} ");
			Console.ResetColor();
			Console.Write("in loans and ");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write($"{Debts}");
			Console.ResetColor();
			Console.Write(" in debts to pay.");
			return $"You currently have {amount}, with {loans} in loans and a debt of {debts} that is to be paid no later than {dueDate}.";
		}
	}

	internal static class SaveToFile
	{
		// Export the transaction history to a text file, JSON or TXT (more like a CSV).
		public static void ExportTransactionHistory(List<Transaction> transactions, string filePath)
		{
			if (filePath.Contains(".json", StringComparison.OrdinalIgnoreCase))
			{
				var options = new JsonSerializerOptions { WriteIndented = true };
				string json = JsonSerializer.Serialize(transactions, options);
				File.WriteAllText(filePath, json);
			}
			else if (filePath.Contains(".txt", StringComparison.OrdinalIgnoreCase) || filePath.Contains(".csv", StringComparison.OrdinalIgnoreCase))
			{
				var sw = new StreamWriter($".\\..\\..\\..\\{filePath}", false);
				// TRANSACTION HISTORY FOR MONEY TRACKER GUIDE: RECIPENT, IN_OR_OUT, AMOUNT, CATEGORY, DATE
				foreach (var t in transactions)
				{
					string Escape(string s) => s.Contains(',') ? $"\"{s}\"" : s; // Escape commas in fields by wrapping them in quotes
					sw.WriteLine($"{Escape(t.Recipent)},{Escape(t.In_Or_Out)},{t.Amount},{Escape(t.Category)},{Escape(t.Date)}");
				}
				sw.Close();
			}
			Console.WriteLine($"Transaction history successfully exported to {filePath}");
		}
	}

	internal class Transaction
	{
		public Transaction()
		{
		}

		public Transaction(string recipent, string in_or_out, double amount, string category, string date)
		{
			Recipent = recipent;
			In_Or_Out = in_or_out;
			Amount = amount;
			Category = category;
			Date = date;
		}

		public string Recipent { get; set; }
		public string In_Or_Out { get; set; }
		public double Amount { get; set; }
		public string Category { get; set; }
		public string Date { get; set; }
		public string GetRecipent()
		{
			return Recipent;
		}
		public string GetInOrOut()
		{
			return In_Or_Out;
		}
		public string GetAmount()
		{
			return Amount.ToString();
		}
		public string GetCategory()
		{
			return Category;
		}
		public string GetDate()
		{
			return Date.ToString();
		}

		public double AddTransaction(List<Transaction> transactions, Transaction newTransction, double accBalance)
		{
			Console.Write("Who is this transaction with? ");
			RECIPENT:   
			string input = Console.ReadLine();
			if (!string.IsNullOrWhiteSpace(input))
				Recipent = input.Trim();
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Please specify the transaction's recipent.");
				Console.ResetColor();
				goto RECIPENT;
			}

			bool bGiveMe = Recipent.Equals("Me", StringComparison.OrdinalIgnoreCase) || Recipent.Equals("Self", StringComparison.OrdinalIgnoreCase);
			if (!bGiveMe)
				Console.Write($"How much will {Recipent} receive? (Must be positive and no more than {accBalance}, your current balance.): ");
			else
			{
				Recipent = "You";
				Console.Write($"How much will you receive? (Must be positive.): ");
			}
		GIVEMONEY:
			input = Console.ReadLine()?.Trim();
			if(string.IsNullOrEmpty(input))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Please specify the transaction's amount.");
				Console.ResetColor();
				goto GIVEMONEY;
			}

			double inputAmount;
			if (input.Equals("all", StringComparison.OrdinalIgnoreCase))    //Can be a value or "all" to give the recipent the full balance.
				inputAmount = accBalance;
			else if (!double.TryParse(input, out inputAmount))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Invalid input, please enter a valid number or 'all'.");
				Console.ResetColor();
				goto GIVEMONEY;
			}

			if (inputAmount <= 0 || (!bGiveMe && inputAmount > accBalance))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("The transaction must be at least above zero and if an expense, no more than your current balance, please try again.");
				Console.ResetColor();
				goto GIVEMONEY;
			}
			
			Amount = inputAmount;
			if (!bGiveMe)
			{
				In_Or_Out = "Expense";
				Console.WriteLine($"{Recipent} will receive {Amount}.\n");
			}
			else
			{
				In_Or_Out = "Income";
				Console.WriteLine($"You will receive {Amount}.\n");
			}

			Console.Write("What transaction is this for? (Bills, loans, damages...): ");
		CATEGORY:
			input = Console.ReadLine();
			if (!string.IsNullOrWhiteSpace(input))
			{
				Category = input.Trim();
				if (!bGiveMe)
					Console.WriteLine($"\n{Recipent} will receive {Amount} for the purpose of: {input}");
				else
					Console.WriteLine($"\nYou will receive {Amount} for the purpose of: {input}");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Please specify the transaction's category.");
				Console.ResetColor();
				goto CATEGORY;
			}

			string date = DateTime.Now.ToString("g");
			Console.WriteLine($"Transaction takes place at {date}, is this correct? (Y/N)");
		DATEANDCONFIRM:
			input = Console.ReadLine()?.Trim();
			switch (input?.ToLowerInvariant())
			{
				case "y":
					Console.Clear();
					Console.WriteLine("Great, the transaction will be added to your history.");
					break;
				case "n":
					Console.WriteLine("What is incorrect?\n1: Recipent\n2: Amount\n3: Category");
				CORRECTIONS:
					char charInput = Console.ReadKey().KeyChar;
					switch (charInput)
					{
						case '1':
							Console.WriteLine("\nThe recipent. Who is this transaction with? ");
							goto RECIPENT;
						case '2':
							Console.WriteLine("\nThe amount. How much will the recipent receive? (Must be positive and no more than your current balance.)");
							goto GIVEMONEY;
						case '3':
							Console.WriteLine("\nThe category. What transaction is this for? (Bills, loans, damages...): ");
							goto CATEGORY;
						default:
							Console.WriteLine("Input invalid, try again.");
							goto CORRECTIONS;
					}
				default:
					Console.WriteLine("Input invalid, try again.");
					goto DATEANDCONFIRM;
			}

			//Transaction newTransaction.(Recipent, In_Or_Out, Amount, Category, Date);
			var newTransaction = new Transaction(Recipent, In_Or_Out, Amount, Category, date);
			if (!bGiveMe)
			{
				Console.WriteLine($"\n({date}): {Recipent} will receive {Amount} for the purposes of: {Category}");
				accBalance -= newTransaction.Amount;
			}
			else
			{
				Console.WriteLine($"\n({date}): You will receive {Amount} for the purposes of: {Category}");
				accBalance += newTransaction.Amount;
			}
			
			transactions.Add(newTransaction);
			return accBalance;
		}
		
		public string TransactionDetails()
		{
			return $"{Recipent} ({In_Or_Out}): {Amount} for {Category} ({Date})";
		}

		public string ViewTransaction()
		{
			Console.WriteLine($"You have a transaction with {Recipent} for the amount of {Amount} in the category of {Category} on the date of {Date}.");
			return $"You have a transaction with {Recipent} for the amount of {Amount} in the category of {Category} on the date of {Date}.";
		}
	}
}
