using application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace presentation.Controllers;

[ApiController]
[Route("api/listings")]
public class ListingsController : ControllerBase
{
    private readonly ISender _mediator;

    public ListingsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateListing([FromBody] CreateListingCommand command)
    {
        // For file uploads we might have to use IFormFile, letting the handler extract the stream...
        // For simplicity, we assume the command has all necessary data.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var listingId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetListingById), new { id = listingId }, new { id = listingId });
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ListingSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> BrowseListings([FromQuery] string? categoryFilter, [FromQuery] string? searchTerm)
    {
        var query = new GetListingsQuery(categoryFilter, searchTerm);
        var listings = await _mediator.Send(query);
        return Ok(listings);
    }
}
