using Microsoft.AspNetCore.Mvc;
using BudgetApi.Dto;
using Arch.EntityFrameworkCore.UnitOfWork;
using DbEntities.Models;
using Mapster;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BudgetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        private readonly ILogger<OperationController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public OperationController(IUnitOfWork unitOfWork, ILogger<OperationController> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // GET: api/<TransactionController>
        [HttpGet]
        public async Task <IEnumerable<OperationDto>> GetAll()
        {
            var Repo = _unitOfWork.GetRepository<Operation>();
            var resultItems = await Repo.GetPagedListAsync();
            return resultItems.Items.AsQueryable().ProjectToType<OperationDto>().ToList();

            //return (IEnumerable<Operation>)resultItems.Items.ToList();
        }

        // GET api/<TransactionController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> /*OperationDto*/ Get(int id, CancellationToken cancellationToken)
        {
            var Repo = _unitOfWork.GetRepository<Operation>();

            var entity = await Repo.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        // POST api/<TransactionController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OperationDto dtoValue, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Operation newdata = dtoValue.ToEntity();

            var Repo = _unitOfWork.GetRepository<Operation>();

            await Repo.InsertAsync(newdata, cancellationToken);
            _unitOfWork.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = newdata.Idoperation }, newdata);
        }

        // PUT api/<TransactionController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OperationDto dtoValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Repo = _unitOfWork.GetRepository<Operation>();
            var find = await Repo.FindAsync(dtoValue.Idoperation);
            if (find == null)
            {
                return NotFound();
            }

            Operation newdata = dtoValue.ToEntity();
            Repo.Update(newdata);
            _unitOfWork.SaveChanges();

            return NoContent();
        }

        // DELETE api/<TransactionController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var Repo = _unitOfWork.GetRepository<Operation>();
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
