using System.Text.Json;
using MoneyTransfer.DomainServices;
using MoneyTransfer.Model;

namespace MoneyTransfer;

public class MoneyTransferService
{
    private readonly IAccountRepository _accountRepository;
    public MoneyTransferService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    public void Transfer(
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

        


       
        var fromAccount = _accountRepository.Get(fromAccountNumber);
        var toAccount = _accountRepository.Get(toAccountNumber);
        
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
        _accountRepository.CreateOrUpdate(fromAccount);
        _accountRepository.CreateOrUpdate(toAccount);
    }
}