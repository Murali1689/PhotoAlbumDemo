using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PhotoAlbum.Dto;
using PhotoAlbum.External.ExternalDto;
using PhotoAlbum.External.PhotoAlbumClient.Interface;
using PhotoAlbum.Queries;

namespace PhotoAlbum.QueryHandler
{
    public class GetPhotoAlbumForUserHandler : IRequestHandler<GetPhotoAlbumForUser, IEnumerable<PhotoAlbumDto>>
    {
        private readonly IPhotoAlbumClient _photoAlbumClient;

        public GetPhotoAlbumForUserHandler(IPhotoAlbumClient photoAlbumClient)
        {
            _photoAlbumClient = photoAlbumClient;
        }

        public async Task<IEnumerable<PhotoAlbumDto>> Handle(GetPhotoAlbumForUser request, CancellationToken cancellationToken)
        {
            var photos = await _photoAlbumClient.GetPhoto();
            IEnumerable<AlbumDto> albums = await _photoAlbumClient.GetAlbum();

            if (request.UserId != null)
            {
                albums = albums.Where(o => o.UserId == request.UserId);
            }

            var toSend = albums.Select(o => new PhotoAlbumDto()
            {
                Album = o,
                Photos = GetPhotoByAlbumId(o.Id, photos)
            });

            return toSend;
        }

        private List<PhotoDto> GetPhotoByAlbumId(int albumId, IEnumerable<PhotoDto> photos)
        {
            var photosGroupedByAlbums = photos.GroupBy(o => o.AlbumId).ToDictionary(p => p.Key, p => p.ToList());

            var filteredPhotos = photosGroupedByAlbums.GetValueOrDefault(albumId) ?? new List<PhotoDto>();

            return filteredPhotos;
        }
    }
}