using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ansible.Data.Model;

namespace Ansible.WebApi.Controllers
{
    [Route( "api/[controller]" )]
    [Produces( "application/json" )]
    public class VoteController : ControllerBase
    {
        //TOD: Mock DB or cache for persistency
        List<Vote> _votes = new List<Vote>
        {
            new Vote { VoteId = 1, GivenName = "Steve", Surname = "McCormack", Gender = "Male", Age = 42 },
            new Vote { VoteId = 2, GivenName = "Krishna", Surname = "Ghandi", Gender = "Male", Age = 41 },
            new Vote { VoteId = 3, GivenName = "Daniel", Surname = "Whitaker", Gender = "Male", Age = 45 },
            new Vote { VoteId = 4, GivenName = "Elle", Surname = "McPherson", Gender = "Female", Age = 50 }
        };

        public VoteController()
        {
            //TODO: repo
        }

        [HttpGet]
        [ProducesResponseType( typeof( IEnumerable<Vote> ), 200 )]
        public IActionResult Get()
        {
            return Ok( _votes );
        }

        [HttpGet( "{id}" )]
        [ProducesResponseType( typeof( Vote ), 200 )]
        [ProducesResponseType( 404 )]
        public IActionResult Get( int id )
        {
            var vote = _votes.FirstOrDefault( x => x.VoteId == id );
            if( vote == null )
            {
                return NotFound();
            }

            return new ObjectResult( vote );
        }

        [HttpPost]
        public IActionResult Post( [FromBody] Vote vote )
        {
            if( vote == null )
            {
                return BadRequest();
            }

            if( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            _votes.Add( vote );
            return CreatedAtAction( "GetById", new { id = vote.VoteId }, vote );
        }
    }
}
