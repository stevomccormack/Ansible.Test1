using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ansible.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Ansible.Data.Model;
using Ansible.Services.Interfaces;
using Microsoft.AspNet.OData;
using Microsoft.EntityFrameworkCore;
using TrackableEntities.Common.Core;
using Urf.Core.Abstractions;

namespace Ansible.WebApi.Controllers
{
    [Produces("application/json")]
    public class VoteController : ODataController
    {
        #region Declarations

        private readonly IVoteService _voteService;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public VoteController(
            IVoteService voteService,
            IUnitOfWork unitOfWork)
        {
            _voteService = voteService;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Methods

        #region Query Operations (CQRS)

        // GET odata/Vote?$skip=2&$top=10
        [EnableQuery]
        public IQueryable<Vote> Get() => _voteService.Queryable();

        // GET odata/Vote(37)
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vote = await _voteService.FindAsync(key);
            if (vote == null)
                return NotFound();

            return Ok(vote);
        }

        #endregion

        #region Command Operations (CQRS)

        // PUT odata/Vote(37)
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Vote vote)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (key != vote.VoteId)
                return BadRequest();

            _voteService.Update(vote);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _voteService.ExistsAsync(key))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // PUT odata/Vote
        public async Task<IActionResult> Post([FromBody] Vote vote)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _voteService.Insert(vote);
            await _unitOfWork.SaveChangesAsync();

            return Created(vote);
        }

        // PATCH, MERGE odata/Vote(37)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<Vote> product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _voteService.FindAsync(key);
            if (entity == null)
                return NotFound();

            product.Patch(entity);
            _voteService.Update(entity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _voteService.ExistsAsync(key))
                    return NotFound();
                throw;
            }
            return Updated(entity);
        }

        // DELETE odata/Vote(37)
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _voteService.DeleteAsync(key);
            if (!result)
                return NotFound();

            await _unitOfWork.SaveChangesAsync();

            return StatusCode((int)HttpStatusCode.NoContent);
        }

        #endregion

        #endregion
    }
}
