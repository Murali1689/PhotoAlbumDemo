using System.Collections.Generic;
using PhotoAlbum.External.ExternalDto;

namespace PhotoAlbum.Dto
{
    public class PhotoAlbumDto
    {
        public AlbumDto Album { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }
}