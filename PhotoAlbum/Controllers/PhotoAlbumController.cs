using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Dto;
using PhotoAlbum.Queries;

namespace PhotoAlbum.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PhotoAlbumController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PhotoAlbumController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(PhotoAlbumDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPhotoAlbum()
        {
            var result = await _mediator.Send(new GetAllPhotoAlbum());

            return Ok(result);
        }

        [HttpGet("GetPhotoAlbumByUserId")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(PhotoAlbumDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPhotoAlbumByUserId(int userId)
        {
            var result = await _mediator.Send(new GetPhotoAlbumForUser(userId));

            return Ok(result);
        }
    }
}