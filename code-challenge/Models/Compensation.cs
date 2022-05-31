using System;

namespace challenge.Models
{
    public class Compensation
    {
        public Guid CompensationId { get; set; }
        public Employee Employee { get; set; }
        public decimal Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}