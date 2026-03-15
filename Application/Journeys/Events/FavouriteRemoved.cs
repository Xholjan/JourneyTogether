using Domain.Entities;
using MediatR;

namespace Application.Journeys.Events
{
    public class FavouriteRemoved : INotification
    {
        public Favourite Favorite { get; }

        public FavouriteRemoved(Favourite favorite)
        {
            Favorite = favorite;
        }
    }
}
