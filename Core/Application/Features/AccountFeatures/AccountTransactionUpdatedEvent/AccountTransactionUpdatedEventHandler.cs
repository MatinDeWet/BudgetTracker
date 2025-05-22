using Application.Common.Messaging;
using Application.Repositories.Command;
using Application.Repositories.Query;
using Domain.Entities;
using Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AccountFeatures.AccountTransactionUpdatedEvent;
internal sealed class AccountTransactionUpdatedEventHandler(IAccountQueryRepository queryRepo, IAccountCommandRepository commandRepo) : IDomainEventHandler<TransactionUpdatedEvent>
{
    public async Task Handle(TransactionUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        Account? account = await queryRepo.InsecureAccounts
            .Where(a => a.Id == domainEvent.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            return;
        }

        account.UpdateBalance(-domainEvent.OriginalAmount);
        account.UpdateBalance(domainEvent.NewAmount);

        commandRepo.InsecureUpdate(account);

        await commandRepo.SaveAsync(cancellationToken);
    }
}
