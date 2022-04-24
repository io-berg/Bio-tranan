using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTheaterCore.Models;

namespace Infrastructure
{
    public class MovieTheaterContextSeeding
    {
        public static async Task SeedAsync(MovieTheaterContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!await context.Movies.AnyAsync())
            {
                await context.Movies.AddRangeAsync(
                    new Movie
                    {
                        Title = "Disaster Movie",
                        Description = "Over the course of one evening, an unsuspecting group of twenty-somethings find themselves bombarded by a series of natural disasters and catastrophic events.",
                        Genre = "Comedy",
                        Runtime = 97,
                        ReleaseYear = 2008,
                        Directors = new List<Director>
                        {
                            new Director
                            {
                                Name = "Jason Friedberg"
                            },
                            new Director
                            {
                                Name = "Aaron Seltzer"
                            }
                        },
                        Actors = new List<Actor>
                        {
                            new Actor
                            {
                                Name = "Carmen Electra"
                            },
                            new Actor
                            {
                                Name = "Vanessa Lachey"
                            },
                            new Actor
                            {
                                Name = "Nicole Parker"
                            },
                        },
                        SpokenLanguage = "English",
                        TextLanguage = "Swedish",
                        AgeRating = 11,
                        AllowedViewings = 3
                    },
                    new Movie
                    {
                        Title = "The Matrix",
                        Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                        Genre = "Action",
                        Runtime = 136,
                        ReleaseYear = 1999,
                        Directors = new List<Director>
                        {
                            new Director
                            {
                                Name = "Lana Wachowski"
                            },
                            new Director
                            {
                                Name = "Lilly Wachowski"
                            }
                        },
                        Actors = new List<Actor>
                        {
                            new Actor
                            {
                                Name = "Keanu Reeves"
                            },
                            new Actor
                            {
                                Name = "Laurence Fishburne"
                            },
                            new Actor
                            {
                                Name = "Carrie-Anne Moss"
                            },
                        },
                        SpokenLanguage = "English",
                        TextLanguage = "English",
                        AgeRating = 15,
                        AllowedViewings = 3
                    },
                    new Movie
                    {
                        Title = "The Dark Knight",
                        Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                        Genre = "Action",
                        Runtime = 152,
                        ReleaseYear = 2008,
                        Directors = new List<Director>
                        {
                            new Director
                            {
                                Name = "Christopher Nolan"
                            }
                        },
                        Actors = new List<Actor>
                        {
                            new Actor
                            {
                                Name = "Christian Bale"
                            },
                            new Actor
                            {
                                Name = "Heath Ledger"
                            },
                            new Actor
                            {
                                Name = "Aaron Eckhart"
                            },
                        },
                        SpokenLanguage = "English",
                        TextLanguage = "English",
                        AgeRating = 18,
                        AllowedViewings = 3
                    },
                    new Movie
                    {
                        Title = "Lord of the Rings: The Fellowship of the Ring",
                        Description = "A meek Hobbit from the Shire and eight companions set out on a journey to destroy the powerful One Ring and save Middle-earth from the Dark Lord Sauron.",
                        Genre = "Adventure",
                        Runtime = 178,
                        ReleaseYear = 2001,
                        Directors = new List<Director>
                        {
                            new Director
                            {
                                Name = "Peter Jackson"
                            }
                        },
                        Actors = new List<Actor>
                        {
                            new Actor
                            {
                                Name = "Elijah Wood"
                            },
                            new Actor
                            {
                                Name = "Ian McKellen"
                            },
                            new Actor
                            {
                                Name = "Viggo Mortensen"
                            },
                        },
                        SpokenLanguage = "English",
                        TextLanguage = "Swedish",
                        AgeRating = 12,
                        AllowedViewings = 3
                    });

                await context.SaveChangesAsync();
            }

            if (!await context.Salons.AnyAsync())
            {
                await context.Salons.AddRangeAsync(
                    new Salon
                    {
                        Name = "Salon 1",
                        Seats = 45
                    },
                    new Salon
                    {
                        Name = "Salon 2",
                        Seats = 40
                    }
                );

                await context.SaveChangesAsync();
            }

            if (!await context.Viewings.AnyAsync())
            {
                await context.Viewings.AddRangeAsync(
                    new MovieViewing
                    {
                        Movie = context.Movies.FirstOrDefault(m => m.Title == "Disaster Movie"),
                        Salon = context.Salons.FirstOrDefault(s => s.Name == "Salon 1"),
                        ViewingStart = new System.DateTime(2022, 4, 24, 10, 0, 0),
                        TicketPrice = 99M
                    },
                    new MovieViewing
                    {
                        Movie = context.Movies.FirstOrDefault(m => m.Title == "The Matrix"),
                        Salon = context.Salons.FirstOrDefault(s => s.Name == "Salon 1"),
                        ViewingStart = new System.DateTime(2022, 4, 24, 11, 0, 0),
                        TicketPrice = 99M
                    },
                    new MovieViewing
                    {
                        Movie = context.Movies.FirstOrDefault(m => m.Title == "Lord of the Rings: The Fellowship of the Ring"),
                        Salon = context.Salons.FirstOrDefault(s => s.Name == "Salon 1"),
                        ViewingStart = new System.DateTime(2022, 4, 24, 12, 0, 0),
                        TicketPrice = 99M
                    },
                    new MovieViewing
                    {
                        Movie = context.Movies.FirstOrDefault(m => m.Title == "The Dark Knight"),
                        Salon = context.Salons.FirstOrDefault(s => s.Name == "Salon 1"),
                        ViewingStart = new System.DateTime(2022, 1, 1, 12, 0, 0),
                        TicketPrice = 99M
                    },
                    new MovieViewing
                    {
                        Movie = context.Movies.FirstOrDefault(m => m.Title == "The Dark Knight"),
                        Salon = context.Salons.FirstOrDefault(s => s.Name == "Salon 2"),
                        ViewingStart = new System.DateTime(2022, 4, 17, 12, 0, 0),
                        TicketPrice = 99M
                    }
                );

                await context.SaveChangesAsync();
            }

            if (!await context.Reservations.AnyAsync())
            {
                await context.Reservations.AddRangeAsync(
                    new Reservation
                    {
                        MovieViewing = context.Viewings.FirstOrDefault(v => v.Movie.Title == "The Dark Knight"),
                        ReservationCode = "12345",
                        Seats = 5,
                    },
                    new Reservation
                    {
                        MovieViewing = context.Viewings.FirstOrDefault(v => v.Movie.Title == "The Dark Knight" && v.Salon.Name == "Salon 2"),
                        ReservationCode = "123",
                        Seats = 5,
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}