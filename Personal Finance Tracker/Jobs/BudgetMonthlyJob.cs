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
                var today = DateTime.UtcNow;

                // Run job only on the 1st day of the month
                if (today.Day == 1)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var budgetService = scope.ServiceProvider.GetRequiredService<IBudgetService>();
                        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                        var users = await userRepository.GetAllUsersAsync();
                        foreach (var user in users)
                        {
                            await budgetService.CreateNewMonthBudgetsAsync(user.User_Id);
                        }
                    }
                }

                // Sleep for a day (job runs every 24 hours)
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
