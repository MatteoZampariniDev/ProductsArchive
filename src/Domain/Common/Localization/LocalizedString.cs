using System.Globalization;

namespace ProductsArchive.Domain.Common.Localization;

public class LocalizedString
{
    public Guid Id { get; set; }

    public virtual IList<LocalizedStringValue> LocalizedValues { get; private set; } = new List<LocalizedStringValue>();

    public string? this[CultureInfo culture]
    {
        get => GetValueFor(culture);
        set => SetValueFor(culture.TwoLetterISOLanguageName, value);
    }

    public string? this[string twoLetterISOLanguageName]
    {
        get => GetValueFor(twoLetterISOLanguageName);
        set => SetValueFor(twoLetterISOLanguageName, value);
    }

    public string? GetValueFor(CultureInfo culture) => GetValueFor(culture.TwoLetterISOLanguageName);
    public string? GetValueFor(string twoLetterISOLanguageName) => LocalizedValues.Where(x => x.TwoLetterISOLanguageName == twoLetterISOLanguageName).Select(x => x.Value).SingleOrDefault();

    public bool HasValueFor(string twoLetterISOLanguageName) => LocalizedValues.Any(x => x.TwoLetterISOLanguageName == twoLetterISOLanguageName);

    public void SetValueFor(string? twoLetterISOLanguageName, string? value)
    {
        if (string.IsNullOrWhiteSpace(twoLetterISOLanguageName))
        {
            twoLetterISOLanguageName = "Default";
        }

        var localizedValue = LocalizedValues.SingleOrDefault(x => x.TwoLetterISOLanguageName == twoLetterISOLanguageName);

        if (localizedValue == null)
        {
            localizedValue = new LocalizedStringValue { TwoLetterISOLanguageName = twoLetterISOLanguageName };
            LocalizedValues.Add(localizedValue);
        }

        localizedValue.Value = value ?? string.Empty;
    }

    public static LocalizedString Create() => new();
    public static LocalizedString Create(CultureInfo culture, string? value) => Create(culture.TwoLetterISOLanguageName, value);
    public static LocalizedString Create(List<(string twoLetterISOLanguageName, string? value)> values)
    {
        var p = Create();

        foreach (var (twoLetterISOLanguageName, value) in values)
        {
            p[twoLetterISOLanguageName] = value;
        }

        return p;
    }
    public static LocalizedString Create(string twoLetterISOLanguageName, string? value)
    {
        var p = Create();
        p[twoLetterISOLanguageName] = value;

        return p;
    }
}