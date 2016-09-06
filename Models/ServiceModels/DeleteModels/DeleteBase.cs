using Models.Common.Interfaces;

namespace Models.ServiceModels.DeleteModels
{
    public abstract class DeleteBase : IValidationModel
    {
        #region Properties
        /// <summary>
        /// The name of the address to delete
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Determines whether to remove the address only if unused ( true == will not be removed if it is in use)
        /// </summary>
        public bool IfUnused { get; set; } = true;
        #endregion

        #region 'IValidationModel' Implementation
        public abstract bool TryValidate(out string error);
        #endregion
    }
}
