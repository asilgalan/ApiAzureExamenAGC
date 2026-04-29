using ApiAzureExamenAGC.Helpers;
using ApiAzureExamenAGC.Models;
using ApiAzureExamenAGC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAzureExamenAGC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {


        private readonly RepositoryCubos repo;
    private readonly HelperUsuarioToken helper;



    public CubosController(RepositoryCubos repo, HelperUsuarioToken helper)
    {
        this.repo = repo;
        this.helper = helper;
    }

    [HttpGet]
    public async Task<ActionResult<List<Cubo>>> GetCubos()
    {
        return await this.repo.GetCubosAsync();
    }

    [HttpGet("CubosBlob")]
    public async Task<ActionResult<List<Cubo>>> GetCubosBlob()
    {
        return await this.repo.GetCubosBlobAsync();
    }

    [HttpGet("{marca}")]
    public async Task<ActionResult<List<Cubo>>> GetCubosMarca(string marca)
    {
        return await this.repo.getCubosByMarca(marca);
    }

        [HttpPost]
        [Route("[action]")]
        public async Task InsertarUsuario([FromForm] string nombre,
                                         [FromForm] string email,
                                         [FromForm] string password,
                                         IFormFile imagen)
        {
            Usuario usuario = new Usuario
            {
                Nombre = nombre,
                Email = email,
                Password = password
            };

            await this.repo.InsertarUsuarioAsync(usuario, imagen);
        }

       [Authorize]
    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult<UsuarioModel>> Perfil()
    {
        UsuarioModel model = this.helper.GetUsuario();
        return model;
    }

    [Authorize]
    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult<UsuarioModel>> PerfilUsuarioBlob()
    {

        UsuarioModel model = this.helper.GetUsuario();
        var usuatioblob = await this.repo.PerfilBlobAsync(model.Id);
        return usuatioblob;
    }

    [Authorize]
    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult<List<Compra>>> GetCompraUsuario()
    {

        UsuarioModel model = this.helper.GetUsuario();
        var compras = await this.repo.GetCompraUsuarioAsync(model.Id);
        return compras;
    }

    [Authorize]
    [HttpPost]
    [Route("[action]")]
    public async Task InsertarPedido(int idLibro)
    {
        UsuarioModel model = this.helper.GetUsuario();
        await this.repo.InsertarCompraAsync(model.Id, idLibro);
    }
}
}
