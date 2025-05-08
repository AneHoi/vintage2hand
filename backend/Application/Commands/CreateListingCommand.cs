using domain;
using MediatR;

namespace application.Commands;

public record CreateListingCommand(
    Guid SellerId,
    string Title,
    string Description,
    string Address,
    double Price,
    List<CreateListingImageDto> Images
): IRequest<Guid>; // Returns the ID of the created listing

public record CreateListingImageDto(
    string ImageUrl,
    bool IsPrimary = false
);
