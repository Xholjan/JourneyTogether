using Api.Extensions;
using Application.Exceptions;
using Application.Interfaces;
using Application.Journeys.Commands;
using Application.Journeys.Events;
using Application.Journeys.Models;
using Application.Journeys.Queries;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Moq;
using System.Security.Claims;

namespace Tests
{
    public class Unit
    {
        public class ApiCallsTests
        {
            private readonly Mock<IJourneyRepository> _journeyRepoMock;
            private readonly Mock<IShareRepository> _shareRepoMock;
            private readonly Mock<IUserRepository> _userRepoMock;
            private readonly Mock<IFavouriteRepository> _favouriteRepoMock;
            private readonly Mock<IMonthlyDistanceRepository> _monthlyDistanceRepoMock;
            private readonly Mock<IMediator> _mediatorMock;
            private readonly AddFavoriteCommandHandler _addFavoriteHandler;
            private readonly CreateJourneyCommandHandler _createJourneyHandler;
            private readonly DeleteJourneyCommandHandler _deleteJourneyHandler;
            private readonly RemoveFavouriteCommandHandler _removeFavouriteHandler;
            private readonly UpdateJourneyCommandHandler _updateHandler;
            private readonly ShareJourneyCommandHandler _shareHandler;
            private readonly GetAdminJourneysQueryHandler _getAdminJourneysHandler;
            private readonly GetJourneyByIdQueryHandler _getJourneyByIdHandler;
            private readonly GetJourneyByPublicIdQueryHandler _getJourneyByPublicIdHandler;
            private readonly GetJourneysPagedQueryHandler _getJourneysPagedHandler;
            private readonly JourneyCreatedHandler _journeyCreatedHandler;


            public ApiCallsTests()
            {
                _journeyRepoMock = new Mock<IJourneyRepository>();
                _userRepoMock = new Mock<IUserRepository>();
                _shareRepoMock = new Mock<IShareRepository>();
                _favouriteRepoMock = new Mock<IFavouriteRepository>();
                _monthlyDistanceRepoMock = new Mock<IMonthlyDistanceRepository>();
                _mediatorMock = new Mock<IMediator>();
                _journeyCreatedHandler = new JourneyCreatedHandler(
                    _journeyRepoMock.Object,
                    _mediatorMock.Object,
                    _monthlyDistanceRepoMock.Object);
                _addFavoriteHandler = new AddFavoriteCommandHandler(
                    _favouriteRepoMock.Object,
                    _userRepoMock.Object);
                _createJourneyHandler = new CreateJourneyCommandHandler(
                    _journeyRepoMock.Object,
                    _userRepoMock.Object);
                _deleteJourneyHandler = new DeleteJourneyCommandHandler(
                    _journeyRepoMock.Object,
                    _userRepoMock.Object,
                    _shareRepoMock.Object);
                _removeFavouriteHandler = new RemoveFavouriteCommandHandler(
                    _favouriteRepoMock.Object,
                    _userRepoMock.Object);
                _updateHandler = new UpdateJourneyCommandHandler(
                    _journeyRepoMock.Object,
                    _userRepoMock.Object);
                _shareHandler = new ShareJourneyCommandHandler(
                    _shareRepoMock.Object,
                    _userRepoMock.Object);
                _getAdminJourneysHandler = new GetAdminJourneysQueryHandler(
                    _journeyRepoMock.Object,
                    _userRepoMock.Object);
                _getJourneyByIdHandler = new GetJourneyByIdQueryHandler(
                    _journeyRepoMock.Object,
                    _userRepoMock.Object);
                _getJourneyByPublicIdHandler = new GetJourneyByPublicIdQueryHandler(
                    _shareRepoMock.Object);
                _getJourneysPagedHandler = new GetJourneysPagedQueryHandler(
                    _journeyRepoMock.Object,
                    _userRepoMock.Object);
            }

            [Fact]
            public async Task Handle_ValidRequest_UpdatesJourneyFields()
            {
                var userId = 2;
                var user = new User { Id = userId };
                var journey = new Journey { Id = 50, UserId = userId };

                var command = new UpdateJourneyCommand
                {
                    Id = journey.Id,
                    UserId = "auth0|123",
                    StartLocation = "London",
                    StartTime = new DateTime(2024, 1, 1, 9, 0, 0),
                    ArrivalLocation = "Manchester",
                    ArrivalTime = new DateTime(2024, 1, 1, 11, 0, 0),
                    TransportType = (Application.Journeys.Models.TransportType)Application.Journeys.Models.TransportType.Train,
                    DistanceKm = 320.5m
                };

                _userRepoMock
                    .Setup(r => r.GetByAuth0Id(command.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

                _journeyRepoMock
                    .Setup(r => r.GetJourneyByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(journey);

                await _updateHandler.Handle(command, CancellationToken.None);

                Assert.Equal("London", journey.StartLocation);
                Assert.Equal("Manchester", journey.ArrivalLocation);
                Assert.Equal(320.5m, journey.DistanceKm);
                Assert.Equal((Domain.Entities.TransportType)command.TransportType, journey.TransportType);

                _journeyRepoMock.Verify(
                    r => r.UpdateJourneyAsync(journey, false, It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async Task Handle_ValidRequest_SharesJourneyWithUsers()
            {
                var userId = 2;
                var user = new User { Id = userId };
                var journeyId = 50;
                var targetUserIds = new List<int> { 3, 4, 5 };

                var command = new ShareJourneyCommand(
                    journeyId: journeyId,
                    userId: "auth0|123",
                    userIds: targetUserIds
                );

                _userRepoMock
                    .Setup(r => r.GetByAuth0Id(command.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

                _shareRepoMock
                    .Setup(r => r.ShareJourneyAsync(command.JourneyId, user.Id, command.UserIds, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                await _shareHandler.Handle(command, CancellationToken.None);

                _shareRepoMock.Verify(
                    r => r.ShareJourneyAsync(journeyId, userId, targetUserIds, It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async Task Handle_ValidRequest_RemovesFavouriteForUser()
            {
                var userId = 2;
                var user = new User { Id = userId };
                var journeyId = 50;

                var command = new RemoveFavouriteCommand(
                    journeyId: journeyId,
                    userId: "auth0|123"
                );

                _userRepoMock
                    .Setup(r => r.GetByAuth0Id(command.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

                _favouriteRepoMock
                    .Setup(r => r.RemoveFavouriteAsync(command.JourneyId, user.Id, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                await _removeFavouriteHandler.Handle(command, CancellationToken.None);

                _favouriteRepoMock.Verify(
                    r => r.RemoveFavouriteAsync(journeyId, userId, It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async Task Handle_OwnerDeletesJourney_DeletesJourneyDirectly()
            {
                var userId = 2;
                var user = new User { Id = userId };
                var journey = new Journey { Id = 50, UserId = userId };

                var command = new DeleteJourneyCommand(journey.Id, userId: "auth0|123");

                _userRepoMock
                    .Setup(r => r.GetByAuth0Id(command.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

                _journeyRepoMock
                    .Setup(r => r.GetJourneyByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(journey);

                _journeyRepoMock
                    .Setup(r => r.DeleteJourneyAsync(journey, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                await _deleteJourneyHandler.Handle(command, CancellationToken.None);

                _journeyRepoMock.Verify(
                    r => r.DeleteJourneyAsync(journey, It.IsAny<CancellationToken>()),
                    Times.Once);

                _shareRepoMock.Verify(
                    r => r.DeleteSharedJourneyAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()),
                    Times.Never);
            }

            [Fact]
            public async Task Handle_SharedUserDeletesJourney_DeletesSharedEntryOnly()
            {
                var userId = 2;
                var ownerId = 99;
                var user = new User { Id = userId };
                var journey = new Journey { Id = 50, UserId = ownerId };
                var share = new Share { Id = 7 };

                var command = new DeleteJourneyCommand(journey.Id, userId: "auth0|123");

                _userRepoMock
                    .Setup(r => r.GetByAuth0Id(command.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

                _journeyRepoMock
                    .Setup(r => r.GetJourneyByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(journey);

                _shareRepoMock
                    .Setup(r => r.GetSharedJourneyAsync(command.Id, user.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(share);

                _shareRepoMock
                    .Setup(r => r.DeleteSharedJourneyAsync(share.Id, user.Id, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                await _deleteJourneyHandler.Handle(command, CancellationToken.None);

                _shareRepoMock.Verify(
                    r => r.DeleteSharedJourneyAsync(share.Id, user.Id, It.IsAny<CancellationToken>()),
                    Times.Once);

                _journeyRepoMock.Verify(
                    r => r.DeleteJourneyAsync(It.IsAny<Journey>(), It.IsAny<CancellationToken>()),
                    Times.Never);
            }

            [Fact]
            public async Task Handle_JourneyNotFound_ThrowsKeyNotFoundException()
            {
                var user = new User { Id = 2 };
                var command = new DeleteJourneyCommand(50, userId: "auth0|123");

                _userRepoMock
                    .Setup(r => r.GetByAuth0Id(command.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

                _journeyRepoMock
                    .Setup(r => r.GetJourneyByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Journey?)null);

                await Assert.ThrowsAsync<KeyNotFoundException>(
                    () => _deleteJourneyHandler.Handle(command, CancellationToken.None));

                _journeyRepoMock.Verify(
                    r => r.DeleteJourneyAsync(It.IsAny<Journey>(), It.IsAny<CancellationToken>()),
                    Times.Never);
            }

            [Fact]
            public async Task Handle_UnauthorizedUserWithNoShare_ThrowsCustomException()
            {
                var userId = 2;
                var ownerId = 99;
                var user = new User { Id = userId };
                var journey = new Journey { Id = 50, UserId = ownerId };

                var command = new DeleteJourneyCommand(journey.Id, userId: "auth0|123");

                _userRepoMock
                    .Setup(r => r.GetByAuth0Id(command.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

                _journeyRepoMock
                    .Setup(r => r.GetJourneyByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(journey);

                _shareRepoMock
                    .Setup(r => r.GetSharedJourneyAsync(command.Id, user.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Share?)null);

                await Assert.ThrowsAsync<CustomException>(
                    () => _deleteJourneyHandler.Handle(command, CancellationToken.None));

                _shareRepoMock.Verify(
                    r => r.DeleteSharedJourneyAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()),
                    Times.Never);
            }

            [Fact]
            public async Task Handle_ValidRequest_CreatesJourneyWithCorrectFields()
            {
                var userId = 2;
                var user = new User { Id = userId };

                var command = new CreateJourneyCommand
                {
                    UserId = "auth0|123",
                    StartLocation = "London",
                    StartTime = new DateTime(2024, 1, 1, 9, 0, 0),
                    ArrivalLocation = "Manchester",
                    ArrivalTime = new DateTime(2024, 1, 1, 11, 0, 0),
                    TransportType = (Application.Journeys.Models.TransportType)Application.Journeys.Models.TransportType.Train,
                    DistanceKm = 320.5m
                };

                _userRepoMock
                    .Setup(r => r.GetByAuth0Id(command.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

                _journeyRepoMock
                    .Setup(r => r.AddJourneyAsync(It.IsAny<Journey>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                await _createJourneyHandler.Handle(command, CancellationToken.None);

                _journeyRepoMock.Verify(r => r.AddJourneyAsync(
                    It.Is<Journey>(j =>
                        j.StartLocation == command.StartLocation &&
                        j.ArrivalLocation == command.ArrivalLocation &&
                        j.StartTime == command.StartTime &&
                        j.ArrivalTime == command.ArrivalTime &&
                        j.DistanceKm == command.DistanceKm &&
                        j.UserId == user.Id),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async Task Handle_ValidRequest_AddsFavouriteForUser()
            {
                var userId = 2;
                var user = new User { Id = userId };
                var journeyId = 50;

                var command = new AddFavoriteCommand(journeyId: journeyId, userId: "auth0|123");

                _userRepoMock
                    .Setup(r => r.GetByAuth0Id(command.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

                _favouriteRepoMock
                    .Setup(r => r.AddFavouriteAsync(command.JourneyId, user.Id, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                await _addFavoriteHandler.Handle(command, CancellationToken.None);

                _favouriteRepoMock.Verify(
                    r => r.AddFavouriteAsync(journeyId, userId, It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async Task Handle_ValidRequest_ReturnsPagedJourneys()
            {
                var user = new User { Id = 2 };
                var journeyModels = new List<JourneyModel>
                {
                    new JourneyModel { Id = 1 },
                    new JourneyModel { Id = 2 }
                };

                var query = new GetAdminJourneysQuery
                {
                    UserId = "auth0|123",
                    Page = 1,
                    PageSize = 10
                };

                _userRepoMock
                    .Setup(r => r.GetByAuth0Id(query.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

                _journeyRepoMock
                    .Setup(r => r.GetAdminJourneysAsync(query, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(journeyModels);

                var result = await _getAdminJourneysHandler.Handle(query, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(query.PageSize, result.PageSize);
                Assert.Equal(journeyModels.Count, result.Items.Count());

                _journeyRepoMock.Verify(
                    r => r.GetAdminJourneysAsync(query, It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async Task Handle_ValidRequest_ReturnsJourneyModel()
            {
                var userId = 2;
                var user = new User { Id = userId };
                var journeyModel = new JourneyModel { Id = 50 };

                var query = new GetJourneyByIdQuery(id: 50, userId: "auth0|123");

                _userRepoMock
                    .Setup(r => r.GetByAuth0Id(query.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

                _journeyRepoMock
                    .Setup(r => r.GetJourneyWithFavouritesByIdAsync(query.Id, user.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(journeyModel);

                var result = await _getJourneyByIdHandler.Handle(query, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(journeyModel.Id, result.Id);

                _journeyRepoMock.Verify(
                    r => r.GetJourneyWithFavouritesByIdAsync(query.Id, userId, It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async Task Handle_ShareNotFound_ReturnsNull()
            {
                var query = new GetJourneyByPublicIdQuery(id: Guid.NewGuid());

                _shareRepoMock
                    .Setup(r => r.GetSharedJourneyByGuidAsync(query.Id, It.IsAny<CancellationToken>()));

                var result = await _getJourneyByPublicIdHandler.Handle(query, CancellationToken.None);

                Assert.Null(result);
            }

            [Fact]
            public async Task Handle_ValidRequest_ReturnsMappedPagedJourneys()
            {
                var userId = 2;
                var user = new User { Id = userId };
                var journeys = new List<Journey>
                {
                    new Journey { Id = 1, StartLocation = "London", ArrivalLocation = "Manchester", StartTime = new DateTime(2024, 1, 1, 9, 0, 0), ArrivalTime = new DateTime(2024, 1, 1, 11, 0, 0), TransportType = Domain.Entities.TransportType.Train, DistanceKm = 320.5m },
                    new Journey { Id = 2, StartLocation = "Bristol", ArrivalLocation = "Leeds", StartTime = new DateTime(2024, 2, 1, 9, 0, 0), ArrivalTime = new DateTime(2024, 2, 1, 12, 0, 0), TransportType = Domain.Entities.TransportType.Car, DistanceKm = 210.0m }
                };

                var query = new GetJourneysPagedQuery(userId: "auth0|123", page: 1, pageSize: 10);

                _userRepoMock
                    .Setup(r => r.GetByAuth0Id(query.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

                _journeyRepoMock
                    .Setup(r => r.GetJourneysAsync(user.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(journeys);

                var result = await _getJourneysPagedHandler.Handle(query, CancellationToken.None);

                Assert.NotNull(result);
                Assert.Equal(query.PageSize, result.PageSize);
                Assert.Equal(journeys.Count, result.Items.Count());

                var items = result.Items.ToList();
                Assert.Equal(journeys[0].Id, items[0].Id);
                Assert.Equal(journeys[0].StartLocation, items[0].StartLocation);
                Assert.Equal(journeys[0].ArrivalLocation, items[0].ArrivalLocation);
                Assert.Equal(journeys[0].DistanceKm, items[0].DistanceKm);
                Assert.Equal((Application.Journeys.Models.TransportType)journeys[0].TransportType, items[0].TransportType);

                _journeyRepoMock.Verify(
                    r => r.GetJourneysAsync(userId, It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async Task Handle_TotalDistanceBelowGoal_DoesNotPublishDailyGoalAchieved()
            {
                var journey = new Journey { Id = 1, UserId = 2, StartTime = DateTime.UtcNow, DistanceKm = 19.9m, IsDailyGoalAchieved = false };
                var existingJourneys = new List<Journey> { journey };
                var notification = new JourneyCreated(journey);

                _journeyRepoMock
                    .Setup(r => r.GetJourneysAsync(journey.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(existingJourneys);

                _monthlyDistanceRepoMock
                    .Setup(r => r.AddOrUpdateAsync(journey.UserId, journey.StartTime, journey.DistanceKm, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                await _journeyCreatedHandler.Handle(notification, CancellationToken.None);

                _mediatorMock.Verify(
                    m => m.Publish(It.IsAny<DailyGoalAchieved>(), It.IsAny<CancellationToken>()),
                    Times.Never);

                _journeyRepoMock.Verify(
                    r => r.UpdateJourneyAsync(It.IsAny<Journey>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
                    Times.Never);
            }

            [Fact]
            public async Task Handle_TotalDistanceExactlyAtGoal_PublishesDailyGoalAchieved()
            {
                var journey = new Journey { Id = 1, UserId = 2, StartTime = DateTime.UtcNow, DistanceKm = 20.0m, IsDailyGoalAchieved = false };
                var existingJourneys = new List<Journey> { journey };
                var notification = new JourneyCreated(journey);

                _journeyRepoMock
                    .Setup(r => r.GetJourneysAsync(journey.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(existingJourneys);

                _monthlyDistanceRepoMock
                    .Setup(r => r.AddOrUpdateAsync(journey.UserId, journey.StartTime, journey.DistanceKm, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                _journeyRepoMock
                    .Setup(r => r.UpdateJourneyAsync(journey, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                await _journeyCreatedHandler.Handle(notification, CancellationToken.None);

                Assert.True(journey.IsDailyGoalAchieved);

                _journeyRepoMock.Verify(
                    r => r.UpdateJourneyAsync(journey, true, It.IsAny<CancellationToken>()),
                    Times.Once);

                _mediatorMock.Verify(
                    m => m.Publish(It.IsAny<DailyGoalAchieved>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async Task Handle_TotalDistanceAboveGoal_PublishesDailyGoalAchieved()
            {
                var journey = new Journey { Id = 1, UserId = 2, StartTime = DateTime.UtcNow, DistanceKm = 20.1m, IsDailyGoalAchieved = false };
                var existingJourneys = new List<Journey> { journey };
                var notification = new JourneyCreated(journey);

                _journeyRepoMock
                    .Setup(r => r.GetJourneysAsync(journey.UserId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(existingJourneys);

                _monthlyDistanceRepoMock
                    .Setup(r => r.AddOrUpdateAsync(journey.UserId, journey.StartTime, journey.DistanceKm, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                _journeyRepoMock
                    .Setup(r => r.UpdateJourneyAsync(journey, true, It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                await _journeyCreatedHandler.Handle(notification, CancellationToken.None);

                Assert.True(journey.IsDailyGoalAchieved);

                _journeyRepoMock.Verify(
                    r => r.UpdateJourneyAsync(journey, true, It.IsAny<CancellationToken>()),
                    Times.Once);

                _mediatorMock.Verify(
                    m => m.Publish(It.IsAny<DailyGoalAchieved>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public void UserAuth0Id_Returns_NameIdentifier_WhenPresent()
            {
                var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "auth0|user-123"), new Claim("sub", "sub-value") };
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
                var result = user.UserAuth0Id();

                Assert.Equal("auth0|user-123", result);
            }
        }
    }
}