namespace Models.ServiceModels.BindingModels
{
    /// <summary>
    /// Contains the properties used to bind a queue to an exchange
    /// </summary>
    public class QueueBindingModel : BindingBase
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

            if (string.IsNullOrWhiteSpace(Destination))
            {
                error = "Please supply a name for the Queue";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Source))
            {
                error = "Please supply a name for the Exchange";
                return false;
            }

            return true;
        }
        #endregion
    }
}
