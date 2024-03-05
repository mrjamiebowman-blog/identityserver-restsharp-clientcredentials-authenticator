// console
Console.WriteLine("Press [T] to Test.");
ConsoleKeyInfo keyInfo;

do
{
    // read key
    keyInfo = Console.ReadKey();

    if (keyInfo.Key == ConsoleKey.T) {
        Console.WriteLine("Running Auth Test...");
        break;
    }
} while (true);
