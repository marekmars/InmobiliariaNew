using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Inmobiliaria.Models;

namespace Inmobiliaria.Controllers
{
    public class MiddlewareExpiracionContratos
    {
        private readonly RequestDelegate _next;

        public MiddlewareExpiracionContratos(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Verificar y actualizar el estado de los contratos
            
            VerificarYActualizarEstadoContratos();


            // Continuar con la ejecución de la solicitud
            await _next(context);
        }

        private void VerificarYActualizarEstadoContratos()
        {
            Console.WriteLine("Ejecutando Middleware");
            ContratosRepository repo= new();
            var contratos = repo.GetAllContratosVencidos();
            foreach (var contrato in contratos)
            {
                repo.DeleteContrato(contrato.Id);
            }
        }

        
    }
}