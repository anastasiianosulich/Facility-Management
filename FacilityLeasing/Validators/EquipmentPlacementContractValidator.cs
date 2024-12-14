using FacilityLeasing.Dtos;
using FluentValidation;

namespace FacilityLeasing.Validators;

public class EquipmentPlacementContractValidator : AbstractValidator<CreateEquipmentPlacementContractDto>
{
    public EquipmentPlacementContractValidator()
    {
        RuleFor(e => e.ProcessEquipmentTypeCode)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(e => e.ProductionFacilityCode)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(e => e.EquipmentUnits)
            .GreaterThan(0);
    }
}
