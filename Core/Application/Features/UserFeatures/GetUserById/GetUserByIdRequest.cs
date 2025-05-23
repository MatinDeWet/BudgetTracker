using Domain.Common.Messaging;

namespace Application.Features.UserFeatures.GetUserById;
public sealed record GetUserByIdRequest(int Id) : IQuery<GetUserByIdResponse>;
