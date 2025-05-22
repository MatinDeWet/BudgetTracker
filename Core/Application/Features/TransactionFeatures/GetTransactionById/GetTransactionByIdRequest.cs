using Domain.Common.Messaging;

namespace Application.Features.TransactionFeatures.GetTransactionById;
public sealed record GetTransactionByIdRequest(Guid Id) : IQuery<GetTransactionByIdResponse>;
