namespace Application.Interfaces
{
    namespace Application.Interfaces
    {
        public interface IShareRepository
        {
            Task ShareJourneyAsync(int journeyId, int sharedByUserId, List<int> userIds, CancellationToken cancellationToken);

            Task<string> CreatePublicLinkAsync(int journeyId, int userId, CancellationToken cancellationToken);
        }
    }
}
