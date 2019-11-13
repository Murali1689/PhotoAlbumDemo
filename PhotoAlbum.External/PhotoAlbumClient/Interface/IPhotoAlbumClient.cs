using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PhotoAlbum.External.ExternalDto;

namespace PhotoAlbum.External.PhotoAlbumClient.Interface
{
    public interface IPhotoAlbumClient
    {
        Task<IEnumerable<PhotoDto>> GetPhoto();

        Task<IEnumerable<AlbumDto>> GetAlbum();
    }
}