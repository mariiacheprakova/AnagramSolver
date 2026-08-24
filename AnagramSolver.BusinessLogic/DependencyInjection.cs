using AnagramSolver.BusinessLogic.Filters;
using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AnagramSolver.BusinessLogic;

public static class DependencyInjection
{
    public static IServiceCollection AddAnagramSolverServices(
        this IServiceCollection services,
        AnagramSettings settings)
    {
        services.AddSingleton(settings);
        services.AddSingleton<FormatingUserInput>();
        services.AddMemoryCache();
        services.AddSingleton<UserInputValidation>();
        services.AddSingleton<IWordRepository, FileWordRepository>();
        services.AddSingleton<IWordFilter,SupportedWordTypeFilter>();
        services.AddSingleton<IAnagramSolver, AnagramSolverService>();

        return services;
    }
}
