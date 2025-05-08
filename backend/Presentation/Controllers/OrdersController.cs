using application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace presentation.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly ISender _mediator;

    public OrdersController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // Can also add  other responses for business rule violations (e.g., item not available)
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var orderId = await _mediator.Send(command);
        // Return a 201 Created response
        return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, new { id = orderId });
    }