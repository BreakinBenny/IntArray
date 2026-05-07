Console.WriteLine("Input numbers and type exit to show a list");
int[] valueArray = new int[0];
int index = 0;

while (true)
{
	Console.WriteLine("Input a number: ");
	string data = Console.ReadLine();
	if (data.ToLower().Trim() == "exit")
		break;

	bool isInt = int.TryParse(data, out int result);
	//Console.WriteLine(isInt);
	//Console.WriteLine(result);
	if (isInt)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine("{0} is a NUMBER", data);
		Array.Resize(ref valueArray, index + 1);
		valueArray[index] = result;
		index++;
	}
	else
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"'{data}' is not a NUMBER, try again.");
	}

	Console.ResetColor();

	//Array.Resize(ref valueArray, index + 1);
	//valueArray[index] = Convert.ToInt32(data);
	//valueArray[index] = int.Parse(data);
	//index++;
}
Console.WriteLine("Numbers in the unsorted list:");

foreach (int value in valueArray)
{
	Console.WriteLine(value);
}

Array.Sort(valueArray);
Console.WriteLine("Numbers in the sorted list:");
foreach (int value in valueArray)
{
	Console.WriteLine(value);
}



Console.ReadLine();