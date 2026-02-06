namespace backend_app.Common.Interfaces
{
    public interface ICurrentUserService
    {
        bool isAdmin { get; }
        string? UserId { get; }
        string? UserName { get; }
    }
}
