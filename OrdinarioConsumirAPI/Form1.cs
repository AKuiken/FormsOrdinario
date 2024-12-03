using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrdinarioConsumirAPI
{
    public partial class Form1 : Form
    {
        private readonly MovieService _movieService;

        public Form1()
        {
            InitializeComponent();
            _movieService = new MovieService(); 
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Por favor ingresa un título para buscar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await SearchMoviesAsync(title);
        }

        private async Task SearchMoviesAsync(string title)
        {
            try
            {
                var movies = await _movieService.SearchMoviesAsync(title);

                if (movies != null && movies.Count > 0)
                {
                    dgvResults.DataSource = movies;
                    dgvResults.Columns["Poster"].Visible = false; 
                    DisplayPoster(movies[0].Poster); 
                }
                else
                {
                    MessageBox.Show("No se encontraron películas para ese título.", "Sin Resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvResults.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DisplayPoster(string posterUrl)
        {
            if (!string.IsNullOrEmpty(posterUrl) && posterUrl != "N/A")
            {
                picPoster.Load(posterUrl);
            }
            else
            {
                picPoster.Image = null;
            }
        }

        private async void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                var movies = await _movieService.GetAllMoviesAsync();

                if (movies != null && movies.Count > 0)
                {
                    dgvResults.DataSource = movies;
                    dgvResults.Columns["Poster"].Visible = false; 
                    DisplayPoster(movies[0].Poster); 
                }
                else
                {
                    MessageBox.Show("No se encontraron películas.", "Sin Resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvResults.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
