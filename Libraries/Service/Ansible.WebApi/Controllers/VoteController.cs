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
    public class VoteController : ControllerBase
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

        // GET api/Vote
        public async Task<IActionResult> Get()
        {
            var votes = await _voteService.Query().SelectAsync();
            if (votes == null)
                return NotFound();

            return Ok(votes);
        }

        // GET api/Vote/37
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vote = await _voteService.FindAsync(id);
            if (vote == null)
                return NotFound();

            return Ok(vote);
        }

        #endregion

        #region Command Operations (CQRS)

        // PUT api/Vote/37
        public async Task<IActionResult> Put(int id, [FromBody] Vote vote)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != vote.VoteId)
                return BadRequest();

            _voteService.Update(vote);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _voteService.ExistsAsync(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // PATCH, MERGE api/Vote/37
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IActionResult> Patch(int id, [FromBody] Delta<Vote> product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _voteService.FindAsync(id);
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
                if (!await _voteService.ExistsAsync(id))
                    return NotFound();
                throw;
            }
            return Ok(entity);
        }

        // DELETE api/Vote(37)
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _voteService.DeleteAsync(id);
            if (!result)
                return NotFound();

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #endregion
    }
}
