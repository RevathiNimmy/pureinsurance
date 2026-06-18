public class KeyCloakConfiguration
{
    public string grant_type { get; set; }
    public string Realm { get; set; }
    public string client_id { get; set; }
    public string username { get; set; }
    public string client_secret { get; set; }
    public string TokenEndpoint { get; set; }
    public string Password { get; set; }
    public string AdminGroupName { get; set; }
    public int UserKey { get; set; }
    public string LoggedInUser { get; set; }
}