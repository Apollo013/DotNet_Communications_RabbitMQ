using Models.PayrollModels.Base;
using System;

namespace Models.PayrollModels
{
    public class PaySlip : EntityBase<int>
    {
        #region Properties
        /// <summary>
        /// Week-end date for which the employee worked
        /// </summary>
        public DateTime WeekEndDate { get; set; }
        /// <summary>
        /// Amount due before deductions are taken into consideration
        /// </summary>
        public double GrossAmount { get; set; }
        /// <summary>
        /// Deductions to be subtracted from the GrossAmount
        /// </summary>
        public double Deductions { get; set; }
        /// <summary>
        /// The net amount due (gross - deductions) due to the employee
        /// </summary>
        public double NetAmount { get { return (GrossAmount - Deductions); } }
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

            if (GrossAmount < 0)
            {
                errorMessage = $"Gross pay cannot be less than 0";
                return false;
            }

            return true;
        }
        #endregion
    }
}
