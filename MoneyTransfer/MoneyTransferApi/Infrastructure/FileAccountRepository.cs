using System.Text.Json;
using Microsoft.Extensions.Hosting;
using MoneyTransfer.Model;

namespace MoneyTransfer.DomainServices.Infrastructure;

public class FileAccountRepository : IAccountRepository
{
    private readonly string _accountsDirectoryPath;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public FileAccountRepository(IHostEnvironment environment)
    {
        _accountsDirectoryPath = Path.Combine(
            environment.ContentRootPath,
            "Account");
    }

    public Account? Get(string accountNumber)
    {
        var accountFilePath = CreateAccountFilePath(accountNumber);

        if (!File.Exists(accountFilePath))
        {
            throw new FileNotFoundException(
                $"Fant ikke kontoen {accountNumber}.");
        }

        var accountJson = File.ReadAllText(accountFilePath);
        return JsonSerializer.Deserialize<Account>(accountJson);
    }

    private string CreateAccountFilePath(string accountNumber)
        => Path.Combine(_accountsDirectoryPath, $"{accountNumber}.json");

    public void CreateOrUpdate(Account account)
    {
        var accountFilePath = CreateAccountFilePath(account.AccountNumber);
        Directory.CreateDirectory(Path.GetDirectoryName(accountFilePath)!);

        var json = JsonSerializer.Serialize(account, JsonOptions);
        File.WriteAllText(accountFilePath, json);
    }
}
