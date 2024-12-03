using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrdinarioConsumirAPI
{
    public class MovieResponse
    {
        public List<Movie> Search { get; set; }
    }

    public class Movie
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }
    public class MovieService
    {
        private readonly string _apiKey = "a3aac1ed";


        public async Task<List<Movie>> SearchMoviesAsync(string title)
        {
            string url = $"https://www.omdbapi.com/?apikey={_apiKey}&s={title}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                var movies = JsonConvert.DeserializeObject<MovieResponse>(response);

                return movies?.Search ?? new List<Movie>();
            }
        }
        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            var movies = new List<Movie>();
            try
            {
                for (int page = 1; page <= 10; page++) 
                {
                    string url = $"https://www.omdbapi.com/?apikey={_apiKey}&s=movie&page={page}";

                    using (HttpClient client = new HttpClient())
                    {
                        var response = await client.GetStringAsync(url);
                        var result = JsonConvert.DeserializeObject<MovieResponse>(response);

                        if (result != null && result.Search != null)
                        {
                            movies.AddRange(result.Search);


                            if (movies.Count >= 100)
                                break;
                        }
                        else
                        {
                            break; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las películas: {ex.Message}");
            }

            return movies.Take(100).ToList();
        }

    }  
}

