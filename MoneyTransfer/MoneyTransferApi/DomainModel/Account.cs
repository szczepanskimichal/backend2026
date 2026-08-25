namespace MoneyTransfer.Model;


    public class Account
    {
        public string AccountNumber { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public decimal Balance { get; set; }
    }
