using FluentValidation;

namespace Application.Journeys.Commands
{
    public class UpdateJourneyCommandValidator : AbstractValidator<UpdateJourneyCommand>
    {
        public UpdateJourneyCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Journey ID must be greater than zero");

            RuleFor(x => x.StartLocation)
                .NotEmpty().WithMessage("Start location is required")
                .MaximumLength(100);

            RuleFor(x => x.ArrivalLocation)
                .NotEmpty().WithMessage("Arrival location is required")
                .MaximumLength(100);

            RuleFor(x => x.StartTime)
                .LessThan(x => x.ArrivalTime)
                .WithMessage("Start time must be before arrival time");

            RuleFor(x => x.DistanceKm)
                .GreaterThan(0).WithMessage("Distance must be greater than zero");
        }
    }
}
