using Domain.Common.Messaging;

namespace Application.Features.AccountFeatures.GetAccountById;
public sealed record GetAccountByIdRequest(Guid Id) : IQuery<GetAccountByIdResponse>;
