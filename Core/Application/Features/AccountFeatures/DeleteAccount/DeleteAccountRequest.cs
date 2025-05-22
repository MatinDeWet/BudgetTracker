using Domain.Common.Messaging;

namespace Application.Features.AccountFeatures.DeleteAccount;
public sealed record DeleteAccountRequest(Guid Id) : ICommand;
