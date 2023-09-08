using crud_asp.net_core_webapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace crud_asp.net_core_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly DbapiContext context;

        public ProductoController(DbapiContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var productos = context.Productos.Include(p => p.IdCategoriaNavigation).ToList();
                return StatusCode(StatusCodes.Status200OK, productos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet]
        [Route("{idProducto:int}")]
        public ActionResult GetProductById(int idProducto)
        {
            try
            {
                var producto = context.Productos.Where(p => p.IdProducto == idProducto).Include(p => p.IdCategoriaNavigation).FirstOrDefault();

                if (producto == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                return StatusCode(StatusCodes.Status200OK, producto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("add")]
        public ActionResult AddProduct([FromBody] Producto producto)
        {
            try
            {
                context.Productos.Add(producto);
                context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, producto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut]
        [Route("edit")]
        public ActionResult EditProduct([FromBody] Producto producto)
        {
            var dbProduct = context.Productos.Find(producto.IdProducto);

            if (dbProduct == null)
            {
                return NotFound();
            }

            try
            {
                dbProduct.CodigoBarra = producto.CodigoBarra is null ? dbProduct.CodigoBarra : producto.CodigoBarra;
                dbProduct.Descripcion = producto.Descripcion is null ? dbProduct.Descripcion : producto.Descripcion;
                dbProduct.Marca = producto.Marca is null ? dbProduct.Marca : producto.Marca;
                dbProduct.IdCategoria = producto.IdCategoria is null ? dbProduct.IdCategoria : producto.IdCategoria;
                dbProduct.Precio = producto.Precio is null ? dbProduct.Precio : producto.Precio;

                context.Productos.Update(dbProduct);
                context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, dbProduct);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }


        }
    }
}
