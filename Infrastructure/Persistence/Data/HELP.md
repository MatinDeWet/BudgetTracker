## Add Migration
| Action | Command |
| --- | --- |
| Add-Migration | EntityFrameworkCore\Add-Migration XXX -Context BudgetContext -Project Persistence -StartupProject Api -Verbose -o Data/Migrations |

## Remove Migration
| Action | Command |
| --- | --- |
| Remove-Migration | EntityFrameworkCore\Remove-Migration -Context BudgetContext -Project Persistence -StartupProject Api -Verbose |

## Update Database
| Action | Command |
| --- | --- |
| Update-Database | EntityFrameworkCore\Update-Database -Context BudgetContext -Project Persistence -StartupProject Api -Verbose |