using System.Text.Json;
using MoneyTransfer.Model;

namespace MoneyTransfer.DomainServices.Infrastructure;

public class FileAccountRepository : IAccountRepository
{
    public Account? Get(string accountNumber)
    {
        var accountFilePath = CreateAccountFilePath($"accounts/{accountNumber}.json");

        if (!File.Exists(accountFilePath))
        {
            throw new FileNotFoundException(
                $"Fant ikke kontoen {accountNumber}.");
        }

        var accountJson = CreateAccountFilePath(File.ReadAllText(accountFilePath));
        ;
        var account =
            JsonSerializer.Deserialize<Account>(
                accountJson);
        return account;
    }

    private static string CreateAccountFilePath(string accountNumber)
    {
        var accountFilePath = $"accounts/{accountNumber}.json";
        return accountFilePath;
    }

    public void CreateOrUpdate(Account account)
    {
    var options =
        new JsonSerializerOptions
        {
            WriteIndented = true
        };

    var accountFilePath = CreateAccountFilePath(account.AccountNumber);
    
    var json = JsonSerializer.Serialize(account, options);
    File.WriteAllText(
    accountFilePath, json);
    }
}

