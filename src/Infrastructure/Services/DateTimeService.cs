using ProductsArchive.Application.Common.Interfaces;

namespace ProductsArchive.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
