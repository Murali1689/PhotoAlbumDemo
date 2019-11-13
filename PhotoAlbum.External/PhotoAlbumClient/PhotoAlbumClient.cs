using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PhotoAlbum.External.ExternalDto;
using PhotoAlbum.External.PhotoAlbumClient.Interface;

namespace PhotoAlbum.External.PhotoAlbumClient
{
    public class PhotoAlbumClient : IPhotoAlbumClient
    {
        private readonly HttpClient _httpClient;

        //private readonly IConfigurationManager _configurationManager;

        public PhotoAlbumClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(configuration.GetSection("PhotoAlbumBaseUrl").Value);
        }

        public async Task<IEnumerable<PhotoDto>> GetPhoto()
        {
            var response = await _httpClient.GetStringAsync("photos");

            var albums = JsonConvert.DeserializeObject<IReadOnlyCollection<PhotoDto>>(response);
            return albums;
        }

        public async Task<IEnumerable<AlbumDto>> GetAlbum()
        {
            var response = await _httpClient.GetStringAsync("albums");

            var albums = JsonConvert.DeserializeObject<IReadOnlyCollection<AlbumDto>>(response);
            return albums;
        }
    }
}