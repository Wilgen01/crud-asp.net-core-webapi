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
                var producto = context.Productos.Find(idProducto);
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
    }
}
