using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MovieInfo
{
    internal class ViewModel : INotifyPropertyChanged
    {
        private ImageSource _poster;
        public ImageSource Poster
        {
            get { return _poster; }
            set
            {
                _poster = value;
                OnPropertyChanged("Poster");
            }
        }
        private string _searchBox;
        public string SearchBox
        {
            get { return _searchBox; }
            set
            {
                _searchBox = value;
                OnPropertyChanged("SearchBox");
            }
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }
        private string _year { get; set; }
        public string Year
        {
            get { return _year; }
            set
            {
                _year = value;
                OnPropertyChanged("Year");
            }
        }
        private string _rating { get; set; }
        public string Rating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                OnPropertyChanged("Rating");
            }
        }
        private string _runtime { get; set; }
        public string Runtime
        {
            get { return _runtime; }
            set
            {
                _runtime = value;
                OnPropertyChanged("Runtime");
            }
        }
        private string _genre { get; set; }
        public string Genre
        {
            get { return _genre; }
            set
            {
                _genre = value;
                OnPropertyChanged("Genre");
            }
        }
        private string _plot;
        public string Plot
        {
            get { return _plot; }
            set
            {
                _plot = value;
                OnPropertyChanged("Plot");
            }
        }
        private bool _isFave;
        public bool IsFave
        {
            get { return _isFave; }
            set
            {
                _isFave = value;
                OnPropertyChanged("IsFave");
            }
        }
        public Command Search { get; set; } = new Command();
        public Command Fave { get; set; } = new Command();
        public ViewModel()
        {
            Search.Method = GoSearch;
            Fave.Method = GoSave;
        }

        private void GoSave(object obj)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SQL))
            {
                SqlCommand request = new SqlCommand($"insert into Faves(imdbID) values (\'{movie.imdbID}\')", connection);
                try
                {
                    connection.Open();
                    request.ExecuteNonQuery();
                    connection.Close();
                    IsFave = false;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        private Movie movie;
        private void GoSearch(object obj)
        {
            IsFave = false;
            Poster = new BitmapImage();
            Title = string.Empty;
            Year = string.Empty;
            Rating = string.Empty;
            Runtime = string.Empty;
            Genre = string.Empty;
            Plot = string.Empty;
            MovieApi api = new MovieApi();
            movie = api.GetMovieInfo(SearchBox);
            if (movie == null) return;
            if (string.Equals(movie.Response, "False"))
            {
                MessageBox.Show(movie.Error);
                return;
            }
            Title = string.Equals(movie.Type, "movie") ? movie.Title : $"({movie.Type}) {movie.Title}";
            Year = movie.Year;
            Rating = movie.Rated;
            Runtime = movie.Runtime;
            Genre = movie.Genre.Replace(", ", ",\n");
            Plot = movie.Plot;
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "poster.png");
                using (WebClient c = new WebClient())
                    c.DownloadFile(movie.Poster, path);
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                img.UriSource = new Uri(path, UriKind.Absolute);
                img.EndInit();
                Poster = img;
            }
            catch
            {
                MessageBox.Show("Can't load poster");
            }
            IsFave = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public Action<object> Method { get; set; }
        public void Execute(object parameter)
        {
            Method(parameter);
        }
    }
}