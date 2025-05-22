using Domain.Common.Messaging;

namespace Application.Features.AccountFeatures.CreateAccount;
public sealed record CreateAccountRequest(string Name) : ICommand<CreateAccountResponse>;
