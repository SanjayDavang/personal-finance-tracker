using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.DTOs
{
    public class UpdateCategoryDto
    {
        public int Category_Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
