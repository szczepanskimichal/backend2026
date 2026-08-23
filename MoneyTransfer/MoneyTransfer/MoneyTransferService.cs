using System.Text.Json;
using MoneyTransfer.Model;

namespace MoneyTransfer;

public class MoneyTransferService
{
    public static void Transfer(
        string fromAccountNumber,
        string toAccountNumber,
        decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException(
                "Beløpet må være større enn null.");
        }

        if (fromAccountNumber == toAccountNumber)
        {
            throw new ArgumentException(
                "Fra-konto og til-konto kan ikke være den samme.");
        }

        var fromAccountFilePath =
            $"accounts/{fromAccountNumber}.json";

        var toAccountFilePath =
            $"accounts/{toAccountNumber}.json";


        if (!File.Exists(fromAccountFilePath))
        {
            throw new FileNotFoundException(
                $"Fant ikke fra-kontoen {fromAccountNumber}.");
        }

        if (!File.Exists(toAccountFilePath))
        {
            throw new FileNotFoundException(
                $"Fant ikke til-kontoen {toAccountNumber}.");
        }


        var fromAccountJson =
            File.ReadAllText(fromAccountFilePath);

        var toAccountJson =
            File.ReadAllText(toAccountFilePath);


        var fromAccount =
            JsonSerializer.Deserialize<Account>(
                fromAccountJson);

        var toAccount =
            JsonSerializer.Deserialize<Account>(
                toAccountJson);


        if (fromAccount == null ||
            toAccount == null)
        {
            throw new InvalidOperationException(
                "Kunne ikke lese konto.");
        }


        if (fromAccount.Balance < amount)
        {
            throw new InvalidOperationException(
                "Det er ikke nok penger på kontoen.");
        }


        fromAccount.Balance -= amount;
        toAccount.Balance += amount;


        var options =
            new JsonSerializerOptions
            {
                WriteIndented = true
            };


        File.WriteAllText(
            fromAccountFilePath,
            JsonSerializer.Serialize(
                fromAccount,
                options));


        File.WriteAllText(
            toAccountFilePath,
            JsonSerializer.Serialize(
                toAccount,
                options));
    }
}