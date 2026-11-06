namespace Library.Core;

/// <summary>Выдача читателю доступа к электронной книге.</summary>
public record Loan(int BookId, int ReaderId, DateTime IssuedAt, DateTime ExpiresAt);

public enum LoanResult { Issued, BookNotFound, LicenseLimitReached }

/// <summary>Хранилище лицензий и выдач.</summary>
public interface ILoanRepository
{
    /// <summary>Число купленных лицензий на книгу или null, если книги нет.</summary>
    int? GetLicenseCount(int bookId);

    int CountActiveLoans(int bookId, DateTime now);

    void Add(Loan loan);
}

public class LoanService
{
    public static readonly TimeSpan LoanPeriod = TimeSpan.FromDays(14);

    private readonly ILoanRepository _repository;

    public LoanService(ILoanRepository repository) => _repository = repository;

    /// <summary>Выдает доступ на 14 дней, если есть свободная лицензия.</summary>
    public LoanResult IssueAccess(int bookId, int readerId, DateTime issuedAt)
    {
        int? licenses = _repository.GetLicenseCount(bookId);
        if (licenses is null)
            return LoanResult.BookNotFound;

        if (_repository.CountActiveLoans(bookId, issuedAt) >= licenses)
            return LoanResult.LicenseLimitReached;

        // LIB-57: срок считается от момента выдачи, а не от даты заказа
        _repository.Add(new Loan(bookId, readerId, issuedAt, issuedAt + LoanPeriod));
        return LoanResult.Issued;
    }
}
