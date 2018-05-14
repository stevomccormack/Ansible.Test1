using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Ansible.Data.Model;

namespace Ansible.WebApi.Controllers
{
    
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class VoteController : ControllerBase
    {
        List<Vote> _votes = new List<Vote>
        {
            new Vote { GivenName = "Steve", Surname= "McCormack", Gender = "Male", Age = 42  },
            new Vote { GivenName = "Krishna", Surname= "Ghandi", Gender = "Male", Age = 41  },
            new Vote { GivenName = "Daniel", Surname= "Whitaker", Gender = "Male", Age = 45  },
            new Vote { GivenName = "Elle", Surname= "McPherson", Gender = "Female", Age = 50  }
        };

        public VoteController()
        {
            //TODO: repo
        }


        [HttpGet(Name = "GetVotes")]
        [ProducesResponseType(typeof(IEnumerable<Vote>), 200)]
        public IActionResult Get()
        {
            return Ok(_votes);
        }
        
        [HttpGet("{id}", Name = "GetVote")]
        [ProducesResponseType(typeof(Vote), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetById(int id)
        {
            var vote = _votes.FirstOrDefault(x => x.VoteId == id);
            if (vote == null)
            {
                return NotFound();
            }
            return new ObjectResult(vote);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Vote vote)
        {
            if (vote == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _votes.Add( vote );
            return CreatedAtAction("GetById", new { id = vote.VoteId }, vote);
        }
    }
}
