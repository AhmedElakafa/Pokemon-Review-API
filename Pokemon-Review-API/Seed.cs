using Pokemon_Review_API.Data;
using Pokemon_Review_API.Models;

namespace Pokemon_Review_API
{
    public class Seed
    {
        private readonly ApplictionDBCotext dataContext; // Ensure the correct context name is used

        public Seed(ApplictionDBCotext context)
        {
            this.dataContext = context;
        }

        public void SeedDataContext()
        {
            if (!dataContext.PokemonOwners.Any())
            {
                var pokemonOwners = new List<PokemonOwner>()
                {
                    new PokemonOwner()
                    {
                        Pokemon = new Pokemon()
                        {
                            Name = "Pikachu",
                            BirthDate = new DateTime(1903, 1, 1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Electric" }}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Title = "Pikachu", Text = "Pikachu is the best pokemon, because it is electric", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review { Title = "Pikachu", Text = "Pikachu is the best at killing rocks", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title = "Pikachu", Text = "Pikachu, Pikachu, Pikachu", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            FirstName = "Jack",
                            LastName = "London",
                            Gym = "Brock's Gym",
                            Country = new Country()
                            {
                                Name = "Kanto"
                            }
                        }
                    },
                    new PokemonOwner()
                    {
                        Pokemon = new Pokemon()
                        {
                            Name = "Squirtle",
                            BirthDate = new DateTime(1903, 1, 1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Water" }}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Title = "Squirtle", Text = "Squirtle is the best pokemon, because it is water", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review { Title = "Squirtle", Text = "Squirtle is the best at killing rocks", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title = "Squirtle", Text = "Squirtle, Squirtle, Squirtle", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            FirstName = "Harry",
                            LastName = "Potter",
                            Gym = "Misty's Gym",
                            Country = new Country()
                            {
                                Name = "Saffron City"
                            }
                        }
                    },
                    new PokemonOwner()
                    {
                        Pokemon = new Pokemon()
                        {
                            Name = "Venusaur", // Fixed typo from "Venasuar" to "Venusaur"
                            BirthDate = new DateTime(1903, 1, 1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Leaf" }}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Title = "Venusaur", Text = "Venusaur is the best pokemon, because it is leaf", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review { Title = "Venusaur", Text = "Venusaur is the best at killing rocks", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title = "Venusaur", Text = "Venusaur, Venusaur, Venusaur", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            FirstName = "Ash",
                            LastName = "Ketchum",
                            Gym = "Ash's Gym",
                            Country = new Country()
                            {
                                Name = "Pallet Town" // Fixed typo from "Millet Town" to "Pallet Town"
                            }
                        }
                    }
                };

                dataContext.PokemonOwners.AddRange(pokemonOwners);
                try
                {
                    dataContext.SaveChanges();
                    Console.WriteLine("Data seeded successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while seeding the data: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Data already exists, seeding skipped.");
            }
        }
    }
}
