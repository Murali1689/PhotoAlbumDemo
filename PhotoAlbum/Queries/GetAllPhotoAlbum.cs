using System.Collections.Generic;
using MediatR;
using PhotoAlbum.Dto;

namespace PhotoAlbum.Queries
{
    public class GetAllPhotoAlbum : IRequest<IEnumerable<PhotoAlbumDto>>
    {
        public GetAllPhotoAlbum()
        {
        }
    }
}