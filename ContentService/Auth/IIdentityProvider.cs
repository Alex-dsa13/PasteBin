namespace ContentService.Auth
{
    public interface IIdentityProvider
    {
        IIdentity Current { get; set; }
    }
}
