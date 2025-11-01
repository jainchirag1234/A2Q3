using System.Collections.Generic;
using System.Web.Mvc;

namespace A2Q3.Models
{
    public class IncrementViewModel
    {
        public int Dept_id { get; set; }
        public decimal IncrementPercent { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; }
    }
}
