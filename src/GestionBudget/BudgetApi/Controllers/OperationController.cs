using Microsoft.AspNetCore.Mvc;
using BudgetApi.Dto;
using Arch.EntityFrameworkCore.UnitOfWork;
using DbEntities.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BudgetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OperationController : ControllerBase
    {
        private readonly ILogger<OperationController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public OperationController(IUnitOfWork unitOfWork, ILogger<OperationController> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Affiche la liste des opérations des utilisateurs en fonction de leur type
        /// </summary>
        /// <param name="typeOperation">Type d'opération à afficher (0=ALL, 1=REVENU, 2=DEPENSE)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task <IEnumerable<OperationDto>> GetAll(int typeOperation=0)
        {
            bool? financeFilter = null;
            var Repo = _unitOfWork.GetRepository<Operation>();

            switch (typeOperation){
                case ((int)FinanceOperation.REVENU): financeFilter = true; break;
                case ((int)FinanceOperation.DEPENSE): financeFilter = false; break;
            }
            var resultItems = await Repo.GetPagedListAsync(predicate: p => ((financeFilter.HasValue)? p.Isrevenu == financeFilter.Value : true) );
            return resultItems.Items.AsQueryable().ProjectToType<OperationDto>().ToList();

            //return (IEnumerable<Operation>)resultItems.Items.ToList();
        }

        // GET api/<TransactionController>/5
        /// <summary>
        /// Affiche les informations concernant une opération
        /// </summary>
        /// <param name="id">Identifiat de l'opération</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Ajoute une nouvelle opération financiere (Déclaration de revenu ou enregistrement d'une dépense)
        /// </summary>
        /// <param name="dtoValue">Objet representant la nouvelle opération financière</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Met à jour les informations concernant une opération financière
        /// </summary>
        /// <param name="id">Identifiant de l'opération financière</param>
        /// <param name="dtoValue">Objet representant l'opération financière</param>
        /// <returns></returns>
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
        /// <summary>
        /// Supprime une opération financière
        /// </summary>
        /// <param name="id">Identifiant de l'opération à supprimer</param>
        /// <returns></returns>
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
