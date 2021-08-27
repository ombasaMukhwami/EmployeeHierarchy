using System;
using System.Collections.Generic;

namespace EmployeeHierarchy.Library
{
    public class Employee
    {
        public Employee()
        {
            Employees = new HashSet<Employee>();
        }
        public string Id { get; set; }
        public int Salary { get; set; }
        public string ManagerId { get; set; }
        public virtual Employee Manager { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public override string ToString()
        {
            return Id;
        }
    }
}
