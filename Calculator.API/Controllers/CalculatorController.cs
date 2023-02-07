using Calculator.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Calculator.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]/")]
    public class CalculatorController : Controller
    {
        private readonly ICalculator _repo;

        public CalculatorController(ICalculator repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public int Calculate(string Expression)
        {
            return _repo.Calculate(Expression);
        }
    }
}
