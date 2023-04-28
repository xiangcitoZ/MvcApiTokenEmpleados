﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcApiTokenEmpleados.Services;
using System.Security.Claims;

namespace MvcApiTokenEmpleados.Controllers
{
    public class ManagedController : Controller
    {
        private ServiceApiEmpleados service;

        public ManagedController(ServiceApiEmpleados service)
        {
            this.service = service;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login
            (string username, string password)
        {
            string token = 
                await this.service.GetTokenAsync(username, password);
            if(token == null) 
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
            }
            else
            {
                ViewData["MENSAJE"] = "Bienvenid@ a mi App!!!";
                HttpContext.Session.SetString("TOKEN", token);
                ClaimsIdentity identity =
                    new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);

                identity.AddClaim(new Claim(ClaimTypes.Name, username));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, password));
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , userPrincipal, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });
                return RedirectToAction("Index", "Home");

            }
            return View();
        }

    }



    
}
