using System.Net;
using FacilityLeasing.Dtos;
using FacilityLeasing.Enums;
using FacilityLeasing.Exceptions.Abstract;

namespace FacilityLeasing.Extensions;

public static class ExceptionExtensions
{
    public static (ErrorDetailsDto, HttpStatusCode) GetErrorDetailsAndStatusCode(this Exception exception)
    {
        return exception switch
        {
            RequestException e => (new ErrorDetailsDto(e.Message, e.ErrorType), e.StatusCode),
            _ => (new ErrorDetailsDto(exception.Message, ErrorType.InternalServerError), HttpStatusCode.InternalServerError)
        };
    }
}