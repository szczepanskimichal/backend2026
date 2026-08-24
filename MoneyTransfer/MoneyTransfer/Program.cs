using MoneyTransfer;
using MoneyTransfer.DomainServices;
using MoneyTransfer.DomainServices.Infrastructure;

try
{
    var repo = new FileAccountRepository();
    var service = new MoneyTransferService(repo);
        service.Transfer(
        fromAccountNumber: "1001",
        toAccountNumber: "1002",
        amount: 250m);

    Console.WriteLine(
        "Overføringen ble gjennomført.");
}
catch (Exception exception)
{
    Console.WriteLine(
        $"Overføringen feilet: {exception.Message}");
}