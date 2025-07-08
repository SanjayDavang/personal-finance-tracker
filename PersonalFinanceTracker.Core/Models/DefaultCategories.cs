using Personal_Finance_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.Models
{
    public class DefaultCategories
    {
        public static List<Category> GetDefaultCategories(int userId)
        {
            return new List<Category>
        {
            new Category { Name = "housing", Type = "Expense", User_Id = userId },
            new Category { Name = "transportation", Type = "Expense", User_Id = userId },
            new Category { Name = "food", Type = "Expense", User_Id = userId },
            new Category { Name = "personal", Type = "Expense", User_Id = userId },
            new Category { Name = "health", Type = "Expense", User_Id = userId },
            new Category { Name = "education", Type = "Expense", User_Id = userId },
            new Category { Name = "debt", Type = "Expense", User_Id = userId },
            new Category { Name = "savings", Type = "Expense", User_Id = userId },
            new Category { Name = "gifts", Type = "Expense", User_Id = userId },

            new Category { Name = "salary", Type = "Income", User_Id = userId },
            new Category { Name = "business", Type = "Income", User_Id = userId },
            new Category { Name = "Investments", Type = "Income", User_Id = userId },
            new Category { Name = "extra Income", Type = "Income", User_Id = userId }
        };
        }
    }
}
