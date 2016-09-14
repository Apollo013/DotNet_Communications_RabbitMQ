namespace Models.ServiceModels.BindingModels
{
    /// <summary>
    /// Contains the properties used to bind one exchange to another exchange
    /// </summary>
    public class ExchangeBindingModel : BindingBase
    {
        #region 'IValidationModel' Implementation
        /// <summary>
        /// Validates the properties for this binding 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public override bool TryValidate(out string error)
        {
            error = "";

            if (string.IsNullOrWhiteSpace(Destination))
            {
                error = "Please supply a name for the destination exchange";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Source))
            {
                error = "Please supply a name for the source exchange";
                return false;
            }

            return true;
        }
        #endregion
    }
}
