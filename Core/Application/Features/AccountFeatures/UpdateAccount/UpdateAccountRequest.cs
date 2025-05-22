using Domain.Common.Messaging;

namespace Application.Features.AccountFeatures.UpdateAccount;
public sealed record UpdateAccountRequest(Guid Id, string Name) : ICommand;
