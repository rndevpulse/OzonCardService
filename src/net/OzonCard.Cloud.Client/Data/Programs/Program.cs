namespace OzonCard.Cloud.Client.Data.Programs;

public record LoyaltyProgram(
    Guid Id,
    string Name,
    string Description,
    DateTime? ServiceTo,
    ProgramType ProgramType,
    bool IsActive,
    Guid? WalletId,
    bool NotifyAboutBalanceChanges
);

public enum ProgramType
{
    Deposit = 0, //0 - deposit or corporate nutrition,
    Bonus = 1, //1 - bonus program,
    Products = 2, //2 - products program,
    Discount = 3, //3 - discount program,
    Certificate = 4, //4 - certificate program.
}