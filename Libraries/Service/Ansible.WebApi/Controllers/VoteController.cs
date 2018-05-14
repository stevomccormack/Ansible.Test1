using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ansible.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Ansible.Data.Model;

namespace Ansible.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class VoteController : ControllerBase
    {
        private readonly AnsibleDbContext _context;

        public VoteController(AnsibleDbContext context)
        {
            _context = context;
        }

        // GET api/vote
        /// <summary>
        /// Get all votes
        /// </summary>
        /// <returns>All votes with HTTP200(Success)</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Vote>), 200)]
        public IActionResult Get()
        {
            var votes = _context.Votes.ToList();
            return Ok(votes);
        }

        // GET api/vote/5
        /// <summary>
        /// Get vote by id
        /// </summary>
        /// <param name="id">vote id</param>
        /// <returns>The specified <code>Vote</code> with HTTP200(Success), otherwise returns HTTP404 (NotFound)</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Vote), 200)]
        [ProducesResponseType(404)]
        public IActionResult Get(int id)
        {
            var vote = _context.Votes.FirstOrDefault(x => x.VoteId == id);
            if (vote == null)
            {
                return NotFound();
            }

            return new ObjectResult(vote);
        }

        // POST api/vote
        /// <summary>
        /// Creates a new vote
        /// </summary>
        /// <param name="vote">vote to add</param>
        /// <returns>HTTP200 for successful creation (includes location for get), otherwise returns HTTP400 (Bad Request)</returns>
        [HttpPost]
        public IActionResult Post([FromBody] Vote vote)
        {
            if (vote == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Votes.Add(vote);
            return CreatedAtAction("GetById", new { id = vote.VoteId }, vote);
        }
    }
}
