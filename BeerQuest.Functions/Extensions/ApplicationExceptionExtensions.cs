using System;
using BeerQuest.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BeerQuest.Functions.Extensions
{
    public static class ApplicationExceptionExtensions
    {
        /// <summary>
        /// Returns an action result based on the input application exception
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static IActionResult ToActionResult(this ApplicationException ex)
        {
            return ex switch
            {
                BadRequestException _ => new BadRequestObjectResult(ex.Message), //400

                ServiceNotAvailableException _ => new StatusCodeResult(503),

                _ => (IActionResult)new BadRequestResult(),
            };
        }
    }
}
