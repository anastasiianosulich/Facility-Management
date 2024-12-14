using FacilityLeasing.Enums;

namespace FacilityLeasing.Dtos;

public record ErrorDetailsDto(string Message, ErrorType ErrorType);
