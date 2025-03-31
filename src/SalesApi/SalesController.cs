using Application.Commands.Products;
using Application.Commands.Sales;
using Application.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SalesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetSales()
        {
            var response = await _mediator.Send(new GetSalesQuery());

            return ResponseHelper.SuccessResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
        {
            var response = await _mediator.Send(command);

            return ResponseHelper.SuccessResponse(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(Guid id)
        {
            var command = new DeleteSaleCommand { Id = id };
            var result = await _mediator.Send(command);
            return ResponseHelper.SuccessResponse(result);
        }

    }
}