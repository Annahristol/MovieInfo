using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MovieInfo
{
    class MovieApi
    {
        public Movie GetMovieInfo(string query)
        {
            string result = null;
            query = WebUtility.UrlEncode(query);
            try
            {
                WebRequest request = WebRequest.Create($"http://www.omdbapi.com/?apikey=80c2e706&t={query}&plot=full");
                WebResponse response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    result = reader.ReadToEnd();
            }
            catch
            {
                MessageBox.Show("Connection error");
                return null;
            }
            if (result == null) return null;
            Movie movie = null;
            try
            {
                movie = JsonConvert.DeserializeObject<Movie>(result);
            }
            catch
            {
                MessageBox.Show("Can't load result");
                return null;
            }
            return movie;
        }
    }
    class Movie
    {
        public string Response;
        public string Error;
        public string Title;
        public string Year;
        public string Rated;
        public string Runtime;
        public string Genre;
        public string Plot;
        public string Type;
        public string Poster;
        public string imdbID;
    }
}
