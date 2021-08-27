using NUnit.Framework;
using EmployeeHierarchy.Library;
using System.Linq;
namespace EmployeeHierarchy.Test
{
    public class Tests
    {
        string employeeCsv = "Employee4,Employee2,500\n" +
                             "Employee3,Employee1,800\n" +
                             "Employee1,,1000\n" +
                             "Employee6,Employee2,500\n" +
                             "Employee5,Employee1,500\n" +
                             "Employee2,Employee1,800";

        private Employee Employee;
        private EmployeeDto EmployeeDto;

        [SetUp]
        public void Setup()
        {
            EmployeeDto = new EmployeeDto(employeeCsv);
        }

        [Test]
        public void ImportSuccessfullTest()
        {
            Assert.IsTrue(EmployeeDto.ImportSuccessfull);
        }
        [Test]
        public void EmployeesShouldNotReportToMoreThanOneManager()
        {
            Assert.IsTrue(!EmployeeDto.EmployeeReportingToMoreThanOneManager());
        }
        [Test]
        public void OnlyOneCEO()
        {           
            Assert.AreEqual(1, EmployeeDto.NumberOfCeos());
        }
        [Test]
        public void SalaryBudgetTest()
        {          
            var salaryBudget = EmployeeDto.SalaryBudget("Employee2");
            Assert.AreEqual(1800, salaryBudget);
        }
    }
}