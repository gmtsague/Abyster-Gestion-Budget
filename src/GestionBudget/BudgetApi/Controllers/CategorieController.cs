using Microsoft.AspNetCore.Mvc;
using BudgetApi.Dto;
using Arch.EntityFrameworkCore.UnitOfWork;
using DbEntities.Models;
using Mapster;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.ComponentModel.DataAnnotations;
//using Microsoft.AspNetCore.Components;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BudgetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Basic")]
    public class CategorieController : ControllerBase
    {
        private readonly ILogger<CategorieController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CategorieController(IUnitOfWork unitOfWork, ILogger<CategorieController> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // GET: api/<CategorieController>
        /// <summary>
        /// Affiche la liste des catégories d'opérations financières
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<CategorieDto>> GetAll()
        {
            var Repo = _unitOfWork.GetRepository<Categorie>();
            var resultItems = await Repo.GetPagedListAsync();
            return resultItems.Items.AsQueryable().ProjectToType<CategorieDto>().ToList();
            // return (IEnumerable<Categorie>)resultItems.Items.ToList().ProjectToType<CategorieDto>();
        }

        // GET api/<CategorieController>/5
        /// <summary>
        /// Affiche une catégorie d'opération financière à partir de son Identifiant
        /// </summary>
        /// <param name="id">Identifiant de la catégorie</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult>/*CategorieDto*/ Get(int id, CancellationToken cancellationToken)
        {
            var Repo = _unitOfWork.GetRepository<Categorie>();

            var entity = await Repo.FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity.Adapt<CategorieDto>());
        }

        // POST api/<CategorieController>
        /// <summary>
        /// Créer (Ajoute) une nouvelle catégorie d'opération
        /// </summary>
        /// <param name="dtoValue">Objet représentant la nouvelle catégorie à créer</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody, Required] CategorieDto dtoValue, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Categorie newdata = dtoValue.ToEntity();

            var Repo = _unitOfWork.GetRepository<Categorie>();

            await Repo.InsertAsync(newdata, cancellationToken);
            _unitOfWork.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = dtoValue.Idcategorie }, dtoValue);
        }

        // PUT api/<CategorieController>/5
        /// <summary>
        /// Mettre à jour une catégorie
        /// </summary>
        /// <param name="id">Identifiant de la catégorie à modifier</param>
        /// <param name="dtoValue">Objet representant les nouvelles valeurs de la catgéorie</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategorieDto dtoValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Categorie newdata = dtoValue.ToEntity();

            var Repo = _unitOfWork.GetRepository<Categorie>();
            var find = await Repo.FindAsync(newdata.Idcategorie);
            if (find == null)
            {
                return NotFound();
            }
            Repo.Update(newdata);
            _unitOfWork.SaveChanges();

            return NoContent();
        }

        // DELETE api/<CategorieController>/5
        /// <summary>
        /// Supprimer une catégorie
        /// </summary>
        /// <param name="id">Identifiant de la catégorie à supprimer</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var Repo = _unitOfWork.GetRepository<Categorie>();
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
