using Domain.Entities;
using MediatR;

namespace Application.Journeys.Commands
{
    public record UpdateJourneyCommand(int Id, string StartLocation, DateTime StartTime, string ArrivalLocation, DateTime ArrivalTime, TransportType TransportType, decimal DistanceKm, int UserId) : IRequest<Unit>;
}
