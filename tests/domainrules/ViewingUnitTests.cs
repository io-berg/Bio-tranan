using MovieTheaterCore.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Infrastructure.Data;
using Infrastructure;
using MovieTheaterCore.Models;
using System;
using MovieTheaterCore.Interfaces;

namespace domainrules;

public class ViewingUnitTests : IDisposable
{
    private MovieTheaterContext _context;
    private MovieViewingService _viewingService;
    private MovieService _movieService;
    private readonly IRepository<MovieViewing> _viewingRepo;
    private readonly IRepository<Movie> _movieRepo;
    private readonly IRepository<Salon> _salonRepo;
    private readonly IRepository<Actor> _actorRepo;
    private readonly IRepository<Director> _directorRepo;

    public ViewingUnitTests()
    {
        var options = new DbContextOptionsBuilder<MovieTheaterContext>()
            .UseInMemoryDatabase(databaseName: "MovieTheater")
            .Options;

        _context = new MovieTheaterContext(options);
        _actorRepo = new Repository<Actor>(_context);
        _directorRepo = new Repository<Director>(_context);
        _movieRepo = new Repository<Movie>(_context);
        _salonRepo = new Repository<Salon>(_context);
        _viewingRepo = new Repository<MovieViewing>(_context);
        _movieService = new MovieService(_movieRepo, _actorRepo, _directorRepo);
        _viewingService = new MovieViewingService(_viewingRepo, _salonRepo, _movieService);

        _salonRepo.InsertAsync(new Salon() { Name = "Salon 1", Seats = 50 }).Wait();
        _movieService.AddMovieAsync(new Movie() { Title = "Movie 1", Genre = "Genre 1", Description = "Description 1", SpokenLanguage = "Spoken Language 1", TextLanguage = "Text Language 1", Runtime = 100, ReleaseYear = 2000, AgeRating = 18, AllowedViewings = 2 }).Wait();
        _movieService.AddMovieAsync(new Movie() { Title = "Movie 2", Genre = "Genre 2", Description = "Description 2", SpokenLanguage = "Spoken Language 2", TextLanguage = "Text Language 2", Runtime = 60, ReleaseYear = 3000, AgeRating = 18, AllowedViewings = 100 }).Wait();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
    }

    [Fact]
    public void TestThatAMovieCantBeUsedInViewingsIfMaxViewingsIsReached()
    {
        // Arrange
        var viewing = new MovieViewingCreationModel
        {
            MovieId = 1,
            SalonId = 1,
            ViewingStart = new DateTime(2024, 1, 1, 2, 2, 2),
            TicketPrice = 10
        };
        var viewing2 = new MovieViewingCreationModel
        {
            MovieId = 1,
            SalonId = 1,
            ViewingStart = new DateTime(2025, 1, 1, 2, 2, 2),
            TicketPrice = 10
        };
        _viewingService.AddMovieViewingAsync(viewing).Wait();
        _viewingService.AddMovieViewingAsync(viewing2).Wait();

        // Act
        var viewing3 = new MovieViewingCreationModel
        {
            MovieId = 1,
            SalonId = 1,
            ViewingStart = new DateTime(2026, 1, 1, 2, 2, 2),
            TicketPrice = 10
        };

        //Assert
        Assert.ThrowsAny<Exception>(() => _viewingService.AddMovieViewingAsync(viewing3).Wait());
    }

    [Theory]
    [InlineData("2024-04-04 10:30:00", false)]
    [InlineData("2024-04-04 08:50:00", true)]
    [InlineData("2024-04-04 15:30:00", true)]
    [InlineData("2024-04-04 09:30:00", false)]
    [InlineData("2024-04-04 10:59:00", false)]
    [InlineData("2024-04-04 11:11:00", true)]
    public void TestThatViewingsCantHappenAtTheSameTime(string viewingStart, bool expected)
    {
        // Arrange
        bool result = true;
        var staticViewing = new MovieViewingCreationModel
        {
            MovieId = 2,
            SalonId = 1,
            ViewingStart = DateTime.Parse("2024-04-04 10:00:00"),
            TicketPrice = 10
        };
        _viewingService.AddMovieViewingAsync(staticViewing).Wait();
        var viewing = new MovieViewingCreationModel
        {
            MovieId = 2,
            SalonId = 1,
            ViewingStart = DateTime.Parse(viewingStart),
            TicketPrice = 5
        };

        // Act
        try
        {
            _viewingService.AddMovieViewingAsync(viewing).Wait();
        }
        catch (Exception)
        {
            result = false;
        }

        // Assert
        Assert.Equal(expected, result);
    }
}