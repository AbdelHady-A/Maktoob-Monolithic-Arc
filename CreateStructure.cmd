dotnet new sln -n Maktoob

REM dotnet "new" "angular" "--auth" "Individual" "-lang" "C#" "-n" "Maktoob.Presentation" "-o" "src\Presentation"
REM dotnet "new" "classlib" "-lang" "C#" "-n" "Maktoob.CrossCuttingConcerns" "-o" "src\CrossCuttingConcerns"
REM dotnet "new" "classlib" "-lang" "C#" "-n" "Maktoob.Domain" "-o" "src\Domain"
REM dotnet "new" "classlib" "-lang" "C#" "-n" "Maktoob.Infrastructure" "-o" "src\Infrastructure"
REM dotnet "new" "classlib" "-lang" "C#" "-n" "Maktoob.Persistance" "-o" "src\Persistance"
REM dotnet "new" "classlib" "-lang" "C#" "-n" "Maktoob.Application" "-o" "src\Application"

dotnet "sln" "Maktoob.sln" "add" "src\Presentation\Maktoob.Presentation.csproj"
dotnet "sln" "Maktoob.sln" "add" "src\CrossCuttingConcerns\Maktoob.CrossCuttingConcerns.csproj"
dotnet "sln" "Maktoob.sln" "add" "src\Domain\Maktoob.Domain.csproj"
dotnet "sln" "Maktoob.sln" "add" "src\Infrastructure\Maktoob.Infrastructure.csproj"
dotnet "sln" "Maktoob.sln" "add" "src\Persistance\Maktoob.Persistance.csproj"
dotnet "sln" "Maktoob.sln" "add" "src\Application\Maktoob.Application.csproj"
