using FluentValidation;

namespace Application.Journeys.Commands
{
    public class CreateJourneyCommandValidator : AbstractValidator<CreateJourneyCommand>
    {
        public CreateJourneyCommandValidator()
        {
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

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
        }
    }
}
