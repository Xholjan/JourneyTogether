using Application.Journeys.Models;
using MediatR;

namespace Application.Journeys.Queries
{
    public class GetJourneyByIdQuery : IRequest<JourneyModel?>
    {
        public string? UserId { get; set; } = string.Empty;
        public int Id { get; set; }
        public GetJourneyByIdQuery(int id, string? userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
