using Domain.Entities;
using MediatR;

namespace Application.Journeys.Events
{
    public class FavouriteAdded : INotification
    {

        public Favourite Favorite { get; }

        public FavouriteAdded(Favourite favorite)
        {
            Favorite = favorite;
        }
    }
}
