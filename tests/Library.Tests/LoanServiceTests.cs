using Library.Core;
using Xunit;

namespace Library.Tests;

public class LoanServiceTests
{
    private sealed class FakeRepository : ILoanRepository
    {
        public readonly List<Loan> Loans = new();
        public int Licenses { get; init; } = 2;

        public int? GetLicenseCount(int bookId) => bookId == 1 ? Licenses : null;

        public int CountActiveLoans(int bookId, DateTime now) =>
            Loans.Count(l => l.BookId == bookId && l.ExpiresAt > now);

        public void Add(Loan loan) => Loans.Add(loan);
    }

    private static readonly DateTime Now = new(2026, 10, 28, 10, 0, 0);

    [Fact]
    public void IssueAccess_WhenLicenseLimitReached_ReturnsConflict()
    {
        var repository = new FakeRepository { Licenses = 2 };
        var service = new LoanService(repository);
        service.IssueAccess(1, readerId: 10, Now);
        service.IssueAccess(1, readerId: 11, Now);

        LoanResult result = service.IssueAccess(1, readerId: 12, Now);

        Assert.Equal(LoanResult.LicenseLimitReached, result);
        Assert.Equal(2, repository.Loans.Count);
    }

    [Fact]
    public void IssueAccess_SetsExpiryFourteenDaysAfterIssue()
    {
        var repository = new FakeRepository();
        var service = new LoanService(repository);

        service.IssueAccess(1, readerId: 10, Now);

        Assert.Equal(Now.AddDays(14), repository.Loans[0].ExpiresAt);
    }

    [Fact]
    public void IssueAccess_WhenBookMissing_ReturnsNotFound()
    {
        var service = new LoanService(new FakeRepository());

        Assert.Equal(LoanResult.BookNotFound, service.IssueAccess(99, readerId: 10, Now));
    }
}
