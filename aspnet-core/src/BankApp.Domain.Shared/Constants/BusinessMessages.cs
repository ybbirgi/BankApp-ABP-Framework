namespace BankApp.Constants;

public class BusinessMessages
{
    public class CustomerMessages
    {
        public const string IdentityNumberIsInUse = "Identity Number Is In Use!";
        public const string IdentityNumberMustBe11Digits = "Identity Number Must Be 11 Digits!";
        public const string CustomerNotFound = "Customer With Given Id Doesn't Exists!";
        public const string CustomerHasDebt = "Customer Has Debt, Pay For Debts First!";
        public const string InvalidRiskLimit = "Remaining Risk Limit Must Be Greater Then 0!";
    }

    public class AccountMessages
    {
        public const string IbanIsAlreadyInUse = "Iban Is Already In Use";
        public const string AccountNotFound = "Account With Given Id Doesn't Exists!";
        public const string IbanIsNotValid = "Iban Is Not Valid!";
    }

    public class CardMessages
    {
        public const string CardNotFound = "Card With Given Id Doesn't Exists!";
        public const string RiskLimitExceeded = "Risk Limit Exceeded!";
        public const string AlreadyHaveDebitCard = "This Account Already Has a Debit Card, Please Deactivate it First!";
        public const string PayDebtFirst = "Please Pay Debt First!";
        public const string CardNumberIsInUse = "Card Number is Already in Use!";
        public const string CardNumberIsNotValid = "Card Number is not Valid!";
    }

    public class TransactionHistoryMessages
    {
        public const string TransactionNotFound = "Transaction With Given Id Does not Exists!";
        public const string NotEnoughBalance = "There is not enough balance!";
        public const string InvalidTransaction = "Can't Deposit To Credit Card If You Don't Have Debt!";
        public const string InvalidDepositTransaction = "Deposited more money then your debt!";
    }
}