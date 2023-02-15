using Microsoft.AspNetCore.Mvc;
using BudgetApi.Dto;
using Arch.EntityFrameworkCore.UnitOfWork;
using DbEntities.Models;
using Mapster;
using System.Reflection.Metadata;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BudgetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork, ILogger<UserController> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task <IEnumerable<UserDto>> GetAll()
        {
            var Repo = _unitOfWork.GetRepository<User>();
            var resultItems = await Repo.GetPagedListAsync();
            return resultItems.Items.AsQueryable().ProjectToType<UserDto>().ToList();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> /*UserDto*/ Get(int id, CancellationToken cancellationToken)
        {
            var Repo = _unitOfWork.GetRepository<Operation>();

            var entity = await Repo.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity.Adapt<UserDto>());
        }

        // GET api/<UsersController>/5/operations
        [HttpGet]
        [Route("{userId}/[action]")]
        public Task<IEnumerable<OperationDto>> Operations(int userId, CancellationToken cancellationToken)
        {
             var repo = _unitOfWork.GetRepository<Operation>();
            var donnees = repo.GetPagedList(predicate: s => s.Userid == userId);
            return (Task<IEnumerable<OperationDto>>)(IEnumerable<OperationDto>) donnees.Items.ToList();
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDto dtoValue, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User newdata = dtoValue.ToEntity();

            var Repo = _unitOfWork.GetRepository<User>();

            await Repo.InsertAsync(newdata, cancellationToken);
            _unitOfWork.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = newdata.Userid }, newdata);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDto dtoValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User newdata = dtoValue.ToEntity();

            var Repo = _unitOfWork.GetRepository<User>();
            var find = await Repo.FindAsync(newdata.Userid);
            if (find == null)
            {
                return NotFound();
            }
            Repo.Update(newdata);
            _unitOfWork.SaveChanges();

            return NoContent();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Repo = _unitOfWork.GetRepository<User>();
            var find = await Repo.FindAsync(id);

            if (find == null)
            {
                return NotFound();
            }
            Repo.Delete(find);
            _unitOfWork.SaveChanges();

            return NoContent();
        }
    }
}
