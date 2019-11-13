using AutoFixture;
using AutoFixture.Xunit2;
using Moq;
using System.Collections.Generic;
using System.Linq;
using PhotoAlbum.External.ExternalDto;
using PhotoAlbum.External.PhotoAlbumClient.Interface;
using PhotoAlbum.Queries;
using PhotoAlbum.QueryHandler;
using Xunit;

namespace PhotoAlbum.Test
{
    public class GetAllPhotoAlbumHandlerTests
    {
        [Theory, AutoData]
        public async void GetAllPhotoAlbum_ValidData_WhenCalled_ReturnsAll([Frozen]Mock<IPhotoAlbumClient> photoAlbumClientMock)
        {
            //Arrange
            var fixture = new Fixture();

            var photos = fixture.Build<PhotoDto>().CreateMany(3);
            var albums = fixture.Build<AlbumDto>().CreateMany(3);

            photoAlbumClientMock.Setup(o => o.GetAlbum()).ReturnsAsync(albums);
            photoAlbumClientMock.Setup(o => o.GetPhoto()).ReturnsAsync(photos);

            var handler = new GetAllPhotoAlbumHandler(photoAlbumClientMock.Object);

            //Act
            var result = await handler.Handle(new GetAllPhotoAlbum(), default);

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.Count() == 3);
            Assert.Contains(albums, o => o.Id == result.First().Album.Id);
            photoAlbumClientMock.Verify(o => o.GetAlbum(), Times.Once);
            photoAlbumClientMock.Verify(o => o.GetPhoto(), Times.Once);
        }

        [Theory, AutoData]
        public async void GetAllPhotoAlbum_NoData_WhenCalled_ReturnsEmptyList([Frozen]Mock<IPhotoAlbumClient> photoAlbumClientMock)
        {
            //Arrange
            photoAlbumClientMock.Setup(o => o.GetAlbum()).ReturnsAsync(new List<AlbumDto>());
            photoAlbumClientMock.Setup(o => o.GetPhoto()).ReturnsAsync(new List<PhotoDto>());

            var handler = new GetAllPhotoAlbumHandler(photoAlbumClientMock.Object);

            //Act
            var result = await handler.Handle(new GetAllPhotoAlbum(), default);

            //Assert
            Assert.Empty(result);
            Assert.True(!result.Any());
            photoAlbumClientMock.Verify(o => o.GetAlbum(), Times.Once);
            photoAlbumClientMock.Verify(o => o.GetPhoto(), Times.Once);
        }

        [Theory, AutoData]
        public async void GetAllPhotoAlbum_WithAlbumWithoutPhotos_WhenCalled_ReturnsAlbumsWithoutPhotos([Frozen]Mock<IPhotoAlbumClient> photoAlbumClientMock)
        {
            //Arrange
            var fixture = new Fixture();

            var albums = fixture.Build<AlbumDto>().CreateMany(3);

            photoAlbumClientMock.Setup(o => o.GetAlbum()).ReturnsAsync(albums);
            photoAlbumClientMock.Setup(o => o.GetPhoto()).ReturnsAsync(new List<PhotoDto>());

            var handler = new GetAllPhotoAlbumHandler(photoAlbumClientMock.Object);

            //Act
            var result = await handler.Handle(new GetAllPhotoAlbum(), default);

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.Count() == 3);
            Assert.True(!result.First().Photos.Any());

            photoAlbumClientMock.Verify(o => o.GetAlbum(), Times.Once);
            photoAlbumClientMock.Verify(o => o.GetPhoto(), Times.Once);
        }

        [Theory, AutoData]
        public async void GetAllPhotoAlbum_WithoutAlbumWithPhotos_WhenCalled_ReturnsEmpty([Frozen]Mock<IPhotoAlbumClient> photoAlbumClientMock)
        {
            //Arrange
            var fixture = new Fixture();

            var photos = fixture.Build<PhotoDto>().CreateMany(3);

            photoAlbumClientMock.Setup(o => o.GetAlbum()).ReturnsAsync(new List<AlbumDto>());
            photoAlbumClientMock.Setup(o => o.GetPhoto()).ReturnsAsync(photos);

            var handler = new GetAllPhotoAlbumHandler(photoAlbumClientMock.Object);

            //Act
            var result = await handler.Handle(new GetAllPhotoAlbum(), default);

            //Assert
            Assert.Empty(result);

            photoAlbumClientMock.Verify(o => o.GetAlbum(), Times.Once);
            photoAlbumClientMock.Verify(o => o.GetPhoto(), Times.Once);
        }

        [Theory, AutoData]
        public async void GetAllPhotoAlbum_PhotosWithoutAlbumId_WhenCalled_ReturnsAlbumWithoutPhoto([Frozen]Mock<IPhotoAlbumClient> photoAlbumClientMock)
        {
            //Arrange
            var fixture = new Fixture();

            var photos = fixture.Build<PhotoDto>().With(o => o.AlbumId, 2000).Create();
            var albums = fixture.Build<AlbumDto>().With(o => o.Id, 1).Create();

            photoAlbumClientMock.Setup(o => o.GetAlbum()).ReturnsAsync(new List<AlbumDto>() { albums });
            photoAlbumClientMock.Setup(o => o.GetPhoto()).ReturnsAsync(new List<PhotoDto>() { photos });

            var handler = new GetAllPhotoAlbumHandler(photoAlbumClientMock.Object);

            //Act
            var result = await handler.Handle(new GetAllPhotoAlbum(), default);

            //Assert
            Assert.NotEmpty(result);
            Assert.True(result.Count() == 1);
            Assert.DoesNotContain(result.First().Photos, c => c.AlbumId == 2000);

            photoAlbumClientMock.Verify(o => o.GetAlbum(), Times.Once);
            photoAlbumClientMock.Verify(o => o.GetPhoto(), Times.Once);
        }
    }
}