using Microsoft.AspNetCore.Mvc;
using SchoolOfDev.Entities;
using SchoolOfDev.Services;

namespace SchoolOfDev.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _service;
        public CourseController(ICourseService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Course course) => Ok(await _service.CreateCourse(course));

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetById(id));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Course course, int id)
        {
            await _service.Update(course, id);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);

            return NoContent();
        }
    }
}