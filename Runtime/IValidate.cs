/// <summary>
///     Add this to all scripts that require a validation step
/// </summary>
public interface IValidate
{
    bool IsValid { get; }


    void OnValidate();
}