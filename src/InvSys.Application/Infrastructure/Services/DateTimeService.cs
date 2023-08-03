using InvSys.Application.Common.Interfaces;

namespace InvSys.Application.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}