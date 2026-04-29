using ApiAzureExamenAGC.Data;
using ApiAzureExamenAGC.Models;
using ApiAzureExamenAGC.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace ApiAzureExamenAGC.Repositories
{
    public class RepositoryCubos
    {

        private readonly ContextCubo context;
        private readonly ServicesBlobStorage services;


        public RepositoryCubos(ContextCubo context, ServicesBlobStorage storageBlobService)
        {
            this.context = context;
            this.services = storageBlobService;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {

            return await this.context.cubos.ToListAsync();

        }

        public async Task<List<Cubo>> getCubosByMarca(string marca)
        {

            return await this.context.cubos.Where(x => x.Marca == marca).ToListAsync();
        }



        public async Task<int> maxIdUsuarioAsync()
        {


            return await this.context.usuarios.MaxAsync(x => x.Id) + 1;
        }

   public async Task InsertarUsuarioAsync(Usuario usuario, IFormFile imagen)
{
    usuario.Id = await this.maxIdUsuarioAsync();

    if (imagen != null)
    {
        // Sube la imagen al blob y guarda solo el nombre
        string nombreBlob = await this.services.SubirImagenAsync("usuarios", imagen);
        usuario.Imagen = nombreBlob; // guarda "leonard.jpg" en BD
    }

    await this.context.usuarios.AddAsync(usuario);
    await this.context.SaveChangesAsync();
}

        public async Task<Usuario> LogInUsuarioAsync(string email, string pass)
        {
            return await this.context.usuarios.Where(x => x.Email == email && x.Password == pass).FirstOrDefaultAsync();
        }

        public async Task<List<Compra>> GetCompraUsuarioAsync(int id)
        {
            return await this.context.compras
                .Where(x => x.Id == id)
                .ToListAsync();
        }
        public async Task InsertarCompraAsync(int idLibro, int idLector)
        {
            var maxIdCompra = await this.context.compras.MaxAsync(x => x.Id) + 1;

            int idCompra = maxIdCompra;
            DateTime fecha = DateTime.Now;

            Compra compra = new Compra();
            compra.Id = idCompra;
            compra.IdCubo = idLibro;
            compra.IdUsuario = idLector;
            compra.FechaPedido = fecha;

            await this.context.compras.AddAsync(compra);
            await this.context.SaveChangesAsync();
        }


        public async Task<List<Cubo>> GetCubosBlobAsync()
        {
            List<Cubo> cubos = await this.GetCubosAsync();

            string containerUrl = this.services.GetContainerUrlAsync("cubos");

            foreach (var c in cubos)
            {

                if (!c.Imagen.StartsWith("http"))
                {
                    string imagePath = c.Imagen;
                

                    c.Imagen = containerUrl + "/" + imagePath;
                }

            }
            return cubos;

        }
        public async Task<UsuarioModel> PerfilBlobAsync(int id)
        {
            Usuario usuario = await this.context.usuarios
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            UsuarioModel model = new UsuarioModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Imagen = usuario.Imagen
            };

            if (!string.IsNullOrEmpty(usuario.Imagen) && !usuario.Imagen.StartsWith("http"))
            {
                // Genera URL privada con SAS que expira en 1 hora
                model.Imagen = this.services.GenerarUrlTemporalUsuario("usuarios", usuario.Imagen);
            }

            return model;
        }
    }
}


