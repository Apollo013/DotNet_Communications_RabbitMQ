namespace Models.ServiceModels.DeleteModels
{
    /// <summary>
    /// Contains the properties for deleteing an Exchange
    /// </summary>
    public class ExchangeDeleteModel : DeleteBase
    {
        #region 'IValidationModel' Implementation
        /// <summary>
        /// Validates this object
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public override bool TryValidate(out string error)
        {
            error = "";

            if (string.IsNullOrWhiteSpace(Name))
            {
                error = "Please supply a name for the Queue";
                return false;
            }

            return true;
        }
        #endregion
    }
}
