namespace Models.Common.Interfaces
{
    public interface IValidationModel
    {
        bool TryValidate(out string error);
    }
}
