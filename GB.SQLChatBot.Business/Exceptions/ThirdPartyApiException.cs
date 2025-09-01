using Pars.Core.Exception;
using System.Diagnostics.CodeAnalysis;

namespace GB.SQLChatBot.Business.Exceptions;

[ExcludeFromCodeCoverage]
public class ThirdPartyApiException : BusinessException
{
    public ThirdPartyApiException(string message) : base(message)
    {
    }
}
