namespace Cattle.Domain.ValueObjects;

public sealed record EarTag
{
    private const int EarTagLength = 5;
    public EarTag(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != EarTagLength)
        {
            throw new ArgumentException($"Ear tag must be {EarTagLength} characters long.", nameof(value));
        }
        Value = value;
    }
    public string Value { get; }
}
