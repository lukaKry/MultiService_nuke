using lukaKry.Calc.API.DataAccess;
using lukaKry.Calc.API.Models;
using lukaKry.Calc.API.Services;
using lukaKry.Calc.Library.Interfaces;
using lukaKry.Calc.Library.Logic;
using lukaKry.Calc.Library.Logic.Calculations;
using lukaKry.Calc.Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lukaKry.Calc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationsController : ControllerBase
    {
        private readonly IEquationsRepository _archiver;
        private readonly CalculatorService _calculatorService;

        public CalculationsController(IEquationsRepository archiver, CalculatorService calculatorService)
        {
            _archiver = archiver;
            _calculatorService = calculatorService;
        }

        [HttpGet]
        [Route("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EquationRecord>> GetAllEquations()
        {
            return Ok(_archiver.GetAllEquations());
        }

        [HttpGet]
        [Route("getlast")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<EquationRecord> GetLast()
        {
            var lastEequation = _archiver.GetLastEquation();
            return Ok(lastEequation);
        }

        [HttpPost]
        [Route("solve")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EquationDTO))]
        public IActionResult Calculate(EquationDTO equation)
        {
            try
            {
                equation = _calculatorService.Evaluate(equation);
                _archiver.AddEquation(equation);
                return Ok(equation);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch(DivideByZeroException e)
            {
                return BadRequest(e.Message);
            }
        }

    }

    
}
