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
