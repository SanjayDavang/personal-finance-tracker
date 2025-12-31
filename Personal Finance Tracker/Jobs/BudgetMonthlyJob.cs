using PersonalFinanceTracker.Core.Interfaces;

namespace Personal_Finance_Tracker.Jobs
{
    public class BudgetMonthlyJob : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BudgetMonthlyJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var today = DateTime.UtcNow.Date;

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                        var monthlyBudgetRunRepository = scope.ServiceProvider.GetRequiredService<IMonthlyBudgetRunRepository>();
                        var budgetService = scope.ServiceProvider.GetRequiredService<IBudgetService>();

                        var users = await userRepository.GetAllUsersAsync();

                        foreach (var user in users)
                        {
                            var lastRun = await monthlyBudgetRunRepository.GetLastRunDateAsync(user.User_Id);

                            // Run if last run is null OR last run month is different from current month
                            if (lastRun == null || lastRun.Value.Month != today.Month || lastRun.Value.Year != today.Year)
                            {
                                // Update budgets for the new month
                                await budgetService.UpdateBudgetsForNewMonthAsync(user.User_Id, today);

                                // Save last run date
                                await monthlyBudgetRunRepository.SetLastRunDateAsync(user.User_Id, today);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"BudgetMonthlyJob failed: {ex}");
                }
                

                // Sleep for a day (job runs every 24 hours)
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
