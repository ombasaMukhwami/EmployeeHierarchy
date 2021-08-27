using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeHierarchy.Library
{
    public class EmployeeDto
    {
        public string Id { get; set; }
        public string Salary { get; set; }
        public string ManagerId { get; set; }
        public List<Employee> Employees { get; set; }
        public bool ImportSuccessfull { get; set; }

        public EmployeeDto()
        {
            Employees = new List<Employee>();
        }
        public EmployeeDto(string employeeCsv)
        {
            string[] employeeArray = employeeCsv.Split('\n');
            var employeeList = (from emp in employeeArray
                                let data = emp.Split(',')
                                select new EmployeeDto
                                {
                                    Id = data[0],
                                    ManagerId = data[1],
                                    Salary = data[2]
                                }).ToList();
            if(employeeList.Any())
            {
                Employees = new List<Employee>();
                foreach (var emp in employeeList)
                    Validate(emp);
            }
        }
        private void Validate(EmployeeDto employee)
        {
            Employee validateEmployee = new Employee();
            ImportSuccessfull = int.TryParse(employee.Salary, out int validatedSalary);
            if (ImportSuccessfull)
            {
                validateEmployee.Id = employee.Id;
                validateEmployee.ManagerId = employee.ManagerId;
                validateEmployee.Salary = validatedSalary;
                Employees.Add(validateEmployee);
            }
        }

        public long SalaryBudget(string employeeId)
        {
            var employee = Employees.Where(e => e.Id.Equals(employeeId, System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            var currentEmployees = Employees.Where(e => e.ManagerId.Equals(employee.Id, StringComparison.OrdinalIgnoreCase)).ToList();
            
            currentEmployees.AddRange(SalaryBudgetEmployeeReporting(currentEmployees));
            currentEmployees.Add(employee);
            var distinctList = currentEmployees.Distinct().Select(e => new Employee { Id = e.Id, ManagerId = e.ManagerId, Salary = e.Salary }).ToList();
            int sum = distinctList.Sum(c => c.Salary);
            return sum;
        }
        private List<Employee> SalaryBudgetEmployeeReporting(List<Employee> lstOfEmployee)
        {
            var listOfSubordinateemployee = new List<Employee>();            
            foreach(var emp in lstOfEmployee)
            {
                var nextEmployee = Employees.Where(e => e.ManagerId.Equals(emp.Id, StringComparison.OrdinalIgnoreCase)).ToList();
                if (nextEmployee.Any())
                {
                    listOfSubordinateemployee.AddRange(SalaryBudgetEmployeeReporting(nextEmployee));
                    //continue;
                }
                listOfSubordinateemployee.Add(emp);
            }
            return listOfSubordinateemployee;
        }
        public int NumberOfCeos()
        {
            return Employees
               .Where(c => string.IsNullOrWhiteSpace(c.ManagerId))
               .Count();
        }
        public bool EmployeeReportingToMoreThanOneManager()
        {
            return Employees.GroupBy(e => e.Id)
                .Where(g => g.Count() > 1)
                .Select(c => new { Id = c.Key, Count = c.Count() }).Any();
        }
    }
}
