using Microsoft.AspNetCore.Mvc;
using MvcApiTokenEmpleados.Filters;
using MvcApiTokenEmpleados.Models;
using MvcApiTokenEmpleados.Services;

namespace MvcApiTokenEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private ServiceApiEmpleados service;

        public EmpleadosController(ServiceApiEmpleados service)
        {
            this.service = service;
        }


        [AuthorizeEmpleados]
        public async Task<IActionResult> Index()
        {   
            string token = 
                HttpContext.Session.GetString("TOKEN");
            if(token == null) 
            {
                ViewData["MENSAJE"] = "Debe realizar Login para visualizar datos";
                return View();
            }
            else
            {
                List<Empleado> empleados =
                    await this.service.GetEmpleadosAsync(token);
                return View(empleados);
            }
          
        }


        public async Task<IActionResult> Details(int id)
        {
            Empleado empleado = await this.service.FindEmpeladoAsync(id);
            return View(empleado);
            
        }

    }
}
