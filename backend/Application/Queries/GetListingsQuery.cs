using MediatR;

namespace application.Queries;

public record GetListingsQuery(
    string? CategoryFilter = null,
    string? SearchTerm = null
) : IRequest<IEnumerable<ListingSummaryDto>>;