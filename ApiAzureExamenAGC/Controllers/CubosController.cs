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

    [HttpGet("LibrosBlob")]
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
    public async Task InsertarUsuario(Usuario usuario)
    {
        await this.repo.insertarUsuarioAsync(usuario);
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
