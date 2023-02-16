using Microsoft.AspNetCore.Mvc;
using BudgetApi.Dto;
using Arch.EntityFrameworkCore.UnitOfWork;
using DbEntities.Models;
using Mapster;
using System.Reflection.Metadata;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Permissions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BudgetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        /// <summary>
        /// Affiche la liste de tous les utilisateurs
        /// </summary>
        /// <returns></returns>
        [HttpGet]        
        public async Task <IEnumerable<UserDto>> GetAll()
        {
            var Repo = _unitOfWork.GetRepository<User>();
            var resultItems = await Repo.GetPagedListAsync();
            return resultItems.Items.AsQueryable().ProjectToType<UserDto>().ToList();
        }

        // GET api/<UsersController>/5
        /// <summary>
        /// Affiche le détail des informations d'un utilisateur
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Affiche la liste des opérations financières d'un utilisateur
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}/[action]")]
        public Task<IEnumerable<OperationDto>> Operations(int userId, CancellationToken cancellationToken)
        {
             var repo = _unitOfWork.GetRepository<Operation>();
            var donnees = repo.GetPagedList(predicate: s => s.Userid == userId);
            return (Task<IEnumerable<OperationDto>>)(IEnumerable<OperationDto>) donnees.Items.ToList();
        }

        // POST api/<UsersController>
        /// <summary>
        /// Permet à un utilisateur de se connecter au système
        /// </summary>
        /// <param name="dtoValue">Objet contenant les informations d'identification de l'utilisateur</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        [AllowAnonymous]
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
        /// <summary>
        /// Réalise la modification des informations (Nom, Prenom, Email) d'un utilisateur 
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur</param>
        /// <param name="dtoValue"></param>
        /// <returns></returns>
        [HttpPut("Update/{id}")]
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

        /// <summary>
        /// Permet à un utilisateur de se connecter au système
        /// </summary>
        /// <param name="creds">Objet contenant les informations d'identification de l'utilisateur</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public IActionResult Login([FromBody] object creds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState) ;
            }

            return NoContent();
        }

        /// <summary>
        /// Réalise la déconnexion d'un utilisateur du système
        /// </summary>
        /// <param name="creds">Objet contenant les informations d'identification de l'utilisateur</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public IActionResult Signout([FromBody] object creds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        // POST api/<UsersController>/5
        /// <summary>
        /// Permet à l'adminisrateur d'activer ou de désactiver un compte utilisateur
        /// </summary>
        /// <param name="userId">Identifiant de l'utilisateur à Activer/Désactiver</param>
        /// <param name="newStatut"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{userId}/[action]")]
        public async Task<IActionResult> Activate(int userId, [FromQuery] bool newStatut)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Repo = _unitOfWork.GetRepository<User>();
            var find = await Repo.FindAsync(userId);
            if (find == null)
            {
                return NotFound();
            }
            find.Isactive = newStatut;
            Repo.Update(find);
            _unitOfWork.SaveChanges();

            return NoContent();
        }

        // DELETE api/<UsersController>/5
        /// <summary>
        /// Permet de supprimer un utilisateur
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur à supprimer</param>
        /// <returns></returns>
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
