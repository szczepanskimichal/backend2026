using MoneyTransfer.Model;

namespace MoneyTransfer.DomainServices;

public interface IAccountRepository
{
    Account? Get(string accountNumber);
    void CreateOrUpdate(Account account);
}