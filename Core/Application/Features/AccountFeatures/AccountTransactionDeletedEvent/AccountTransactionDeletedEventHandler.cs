using Application.Common.Messaging;
using Application.Repositories.Command;
using Application.Repositories.Query;
using Domain.Entities;
using Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AccountFeatures.AccountTransactionDeletedEvent;
internal sealed class AccountTransactionDeletedEventHandler(IAccountQueryRepository queryRepo, IAccountCommandRepository commandRepo) : IDomainEventHandler<TransactionDeletedEvent>
{
    public async Task Handle(TransactionDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        Account? account = await queryRepo.InsecureAccounts
            .Where(a => a.Id == domainEvent.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            return;
        }

        account.UpdateBalance(-domainEvent.Amount);

        commandRepo.InsecureUpdate(account);

        await commandRepo.SaveAsync(cancellationToken);
    }
}
