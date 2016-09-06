using Models.PayrollModels.Base;
using System;

namespace Models.PayrollModels
{
    public class WorkCard : EntityBase<int>
    {
        #region Properties
        /// <summary>
        /// The total hours worked by the employee for the week
        /// </summary>
        public double HoursWorked { get; set; }
        /// <summary>
        /// Week-end date for which the employee worked
        /// </summary>
        public DateTime WeekEndDate { get; set; }
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

            if (Id <= 0)
            {
                errorMessage = "Employee Id must be greater than 0";
                return false;
            }

            if (HoursWorked < 0)
            {
                errorMessage = $"Hours Worked cannot be less than 0";
                return false;
            }

            return true;
        }
        #endregion
    }
}
