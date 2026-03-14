using Domain.Entities;
using MediatR;

namespace Application.Journeys.Commands
{
    public record CreateJourneyCommand(string StartLocation, DateTime StartTime, string ArrivalLocation, DateTime ArrivalTime, TransportType TransportType, decimal DistanceKm, int UserId) : IRequest<int>;
}
