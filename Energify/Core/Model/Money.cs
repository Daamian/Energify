namespace Energify.Core.Model;

public readonly struct Money(decimal amount, string currency = "USD") : IEquatable<Money>
{
    public decimal Amount { get; } = amount;
    public string Currency { get; } = currency ?? "USD";

    public override string ToString() => $"{Amount:F2} {Currency}";

    public static Money operator +(Money m1, Money m2)
    {
        if (m1.Currency != m2.Currency)
            throw new InvalidOperationException("Cannot add money with different currencies.");

        return new Money(m1.Amount + m2.Amount, m1.Currency);
    }

    public static Money operator -(Money m1, Money m2)
    {
        if (m1.Currency != m2.Currency)
            throw new InvalidOperationException("Cannot subtract money with different currencies.");

        return new Money(m1.Amount - m2.Amount, m1.Currency);
    }

    public static bool operator >(Money m1, Money m2)
    {
        if (m1.Currency != m2.Currency)
            throw new InvalidOperationException("Cannot compare money with different currencies.");

        return m1.Amount > m2.Amount;
    }

    public static bool operator <(Money m1, Money m2)
    {
        if (m1.Currency != m2.Currency)
            throw new InvalidOperationException("Cannot compare money with different currencies.");

        return m1.Amount < m2.Amount;
    }

    public static bool operator >=(Money m1, Money m2) => !(m1 < m2);
    public static bool operator <=(Money m1, Money m2) => !(m1 > m2);
    public static bool operator ==(Money m1, Money m2) => m1.Equals(m2);
    public static bool operator !=(Money m1, Money m2) => !m1.Equals(m2);

    public bool Equals(Money other)
    {
        return Amount == other.Amount && Currency == other.Currency;
    }

    public override bool Equals(object? obj)
    {
        return obj is Money other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }
}