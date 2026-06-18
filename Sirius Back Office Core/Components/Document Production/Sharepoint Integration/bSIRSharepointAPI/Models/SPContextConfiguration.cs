namespace bSIRSharepointApi.Models
{
   public class SPContextConfiguration
    {
        /// <summary>
        /// Get and Set SharePoint site URL.
        /// </summary>
        public string SharePointSiteURL { get; set; } = string.Empty;

        /// <summary>
        /// Get and Set SharePoint site URL.
        /// </summary>
        public string FromSharePointSiteURL { get; set; } = string.Empty;

        /// <summary>
        /// Get and Set SharePoint site URL.
        /// </summary>
        public string SharePointDocumentLibrary { get; set; } = string.Empty;

        /// <summary>
        /// Get and Set SharePoint site URL.
        /// </summary>
        public string FromSharePointDocumentLibrary { get; set; } = string.Empty;

        /// <summary>
        /// Get and Set SharePoint site URL.
        /// </summary>
        public string SharePointUserName { get; set; } = string.Empty;

        /// <summary>
        /// Get and Set SharePoint site URL.
        /// </summary>
        public string FromSharePointUserName { get; set; } = string.Empty;

        /// <summary>
        /// Get and Set SharePoint site URL.
        /// </summary>
        public string SharePointPassword { get; set; } = string.Empty;

        /// <summary>
        /// Get and Set SharePoint site URL.
        /// </summary>
        public string FromSharePointPassword { get; set; } = string.Empty;

        /// <summary>
        /// Get and Set SharePoint site URL.
        /// </summary>
        public bool IsSharePointOnline { get; set; } = false;
        public string AppClientId { get; set; } = string.Empty;
        public string SharepointTenantId { get; set; }= string.Empty;
    }
}
