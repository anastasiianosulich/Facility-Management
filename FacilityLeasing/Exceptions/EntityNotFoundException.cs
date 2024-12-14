using FacilityLeasing.Exceptions.Abstract;
using FacilityLeasing.Enums;
using System.Net;

namespace FacilityLeasing.Exceptions;

public sealed class EntityNotFoundException : RequestException
{
    public EntityNotFoundException(string message) : base(message, ErrorType.EntityNotFound, HttpStatusCode.NotFound)
    {
    }
}