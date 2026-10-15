using Forum_Project1.Models;
using Forum_Project1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Forum1_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicsController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Topic>>> GetTopics()
        {
            var topics = await _topicService.GetAllTopicsAsync();
            return Ok(topics);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Topic>> GetTopic(int id)
        {
            var topic = await _topicService.GetTopicByIdAsync(id);

            if (topic == null)
                return NotFound();

            return Ok(topic);
        }

        [HttpPost]
        public async Task<ActionResult<Topic>> SaveTopic([FromBody] Topic topic)
        {
            if (topic == null)
                return BadRequest();

            if (topic.CreatedAt == default)
                topic.CreatedAt = DateTime.Now;

            var createdTopic = await _topicService.CreateTopicAsync(topic);

            return CreatedAtAction(nameof(GetTopic), new { id = createdTopic.Id }, createdTopic);
        }
    }
}
