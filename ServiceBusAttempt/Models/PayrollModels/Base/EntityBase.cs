namespace Models.PayrollModels.Base
{
    public abstract class EntityBase<Tkey>
    {
        #region Properties
        public Tkey Id { get; set; }
        #endregion

        #region Overrides
        public override bool Equals(object entity)
        {
            return entity != null
               && entity is EntityBase<Tkey>
               && this == (EntityBase<Tkey>)entity;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        #endregion

        #region Operators
        /// <summary>
        /// 'Equals' Operator
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="entity2"></param>
        /// <returns></returns>
        public static bool operator ==(EntityBase<Tkey> entity1, EntityBase<Tkey> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
            {
                return true;
            }

            if ((object)entity1 == null || (object)entity2 == null)
            {
                return false;
            }

            if (entity1.Id.ToString() == entity2.Id.ToString())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 'Not Equals' Operator 
        /// </summary>
        /// <param name="entity1"></param>
        /// <param name="entity2"></param>
        /// <returns></returns>
        public static bool operator !=(EntityBase<Tkey> entity1, EntityBase<Tkey> entity2)
        {
            return (!(entity1 == entity2));
        }
        #endregion

        #region Abstract
        protected abstract bool TryValidate(out string errorMessage);
        #endregion
    }
}
