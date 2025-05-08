using application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace presentation.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly ISender _mediator;

    public ReviewsController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
    [HttpPost("seller")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitSellerReview([FromBody] SubmitSellerReviewCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var reviewId = await _mediator.Send(command);
        // Return a 201 Created response
        // To show an even simpler example, not adding a GetReviewById action, so just returning the ID.
        return StatusCode(StatusCodes.Status201Created, new { id = reviewId });
    }
}