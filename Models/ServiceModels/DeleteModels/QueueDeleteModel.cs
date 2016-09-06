namespace Models.ServiceModels.DeleteModels
{
    /// <summary>
    /// Contains the properties for deleteing a Queue
    /// </summary>
    public class QueueDeleteModel : DeleteBase
    {
        #region Properties
        /// <summary>
        /// If true, only deletes the queue if it is empty
        /// </summary>
        public bool IfEmpty { get; set; } = true;
        #endregion

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
