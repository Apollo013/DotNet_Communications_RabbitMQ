using Models.PayrollModels.Base;
namespace Models.PayrollModels
{
    public class Employee : EntityBase<int>
    {
        #region Fields
        private static int _currentEmployeeNo = 0;
        private static double _minimumHourlyRate = 9.15;
        #endregion

        #region Properties
        /// <summary>
        /// Employee's full name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Rate paid per hour to employee
        /// </summary>
        public double HourlyRate { get; set; } = _minimumHourlyRate;
        /// <summary>
        /// Rate of tax to be calculated on the gross amount earned by the employee
        /// </summary>
        public double TaxRate { get; set; } = 21.00;
        #endregion

        #region Constructors
        public Employee()
        {
            // Increment Employee Id with each new instance
            Id = ++_currentEmployeeNo;
        }
        #endregion

        #region Implemented Abstract Methods
        /// <summary>
        /// Validates the entity
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected override bool TryValidate(out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(Name))
            {
                errorMessage = "Please supply a name for the employee";
                return false;
            }

            if (Id <= 0)
            {
                errorMessage = "Employee Id must be greater than 0";
                return false;
            }

            if (HourlyRate <= _minimumHourlyRate)
            {
                errorMessage = $"Hourly Rate cannot be less than {_minimumHourlyRate}";
                return false;
            }

            if (TaxRate < 0 || TaxRate > 100)
            {
                errorMessage = $"Tax Rate must be between 0% and 100 %";
                return false;
            }

            return true;
        }
        #endregion
    }
}
