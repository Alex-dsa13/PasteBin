namespace ContentService.Auth
{
    public class IdentityProvider : IIdentityProvider
    {
        public IIdentity Current { get; set; }
    }
}
