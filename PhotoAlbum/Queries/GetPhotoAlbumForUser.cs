using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PhotoAlbum.Dto;

namespace PhotoAlbum.Queries
{
    public class GetPhotoAlbumForUser : IRequest<IEnumerable<PhotoAlbumDto>>
    {
        public GetPhotoAlbumForUser(int? userId)
        {
            UserId = userId;
        }

        public int? UserId { get; }
    }
}