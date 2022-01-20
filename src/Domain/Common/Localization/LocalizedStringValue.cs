namespace ProductsArchive.Domain.Common.Localization;

public class LocalizedStringValue
{
    public Guid? LocalizedStringId { get; set; }
    public virtual LocalizedString? LocalizedString { get; set; }

    public string? TwoLetterISOLanguageName { get; set; }
    public string? Value { get; set; }
}
