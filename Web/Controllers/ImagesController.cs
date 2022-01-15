using Contemplation.Services;
using Microsoft.AspNetCore.Mvc;

namespace Contemplation.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImagesController : ControllerBase
    {
        public ImagesController(IImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        private IImagesService _imagesService;

        [HttpGet]
        public string[] GetAll()
        {
            return _imagesService.GetAll();
        }

        [HttpGet("{name}")]
        public IActionResult GetFull(string name, [FromQuery] int? w, [FromQuery] int? h)
        {
            var image = _imagesService.GetFull(name, w, h);
            return File(image, "image/jpeg", name);
        }
    }
}
