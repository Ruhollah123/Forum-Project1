using Forum_Project1.Data;
using Forum_Project1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum1_API.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Post>> AddPost(Post post)
        {
            if (post == null)
            {
                return BadRequest("Post-data saknas");
            }

            if (post.CreatedAt == default)
            {
                post.CreatedAt = DateTime.Now;
            }

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return Ok(post);

        }
    }
}
