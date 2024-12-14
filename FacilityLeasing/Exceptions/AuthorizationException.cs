using FacilityLeasing.Exceptions.Abstract;
using FacilityLeasing.Enums;
using System.Net;

namespace FacilityLeasing.Exceptions;

public sealed class AuthorizationException : RequestException
{
    public AuthorizationException() : base(
                                        "Authentication failed due to invalid API key.", 
                                        ErrorType.Unauthorized, 
                                        HttpStatusCode.Unauthorized)
    {
    }
}