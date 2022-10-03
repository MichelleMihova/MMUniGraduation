namespace MMUniGraduation.Models
{
    public interface IApplicationUser
    {
        bool Equals(object obj);
        int GetHashCode();
    }
}