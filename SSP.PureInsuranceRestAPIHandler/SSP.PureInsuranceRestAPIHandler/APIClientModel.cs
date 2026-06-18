namespace SSP.PureInsuranceRestAPIHandler
{
    public class APIClientModel
    {
        public string TokenCacheKey = "access_token";
        public string TokenUrl = "https://your-auth-server.com/connect/token";
        public string ApiUrl = "https://your-api-url.com/api/data";

        public string ClientId = "your-client-id";
        public string ClientSecret = "your-client-secret";
        public string Scope = "api.read";
        //public string GetDataFromApi()
        //{
        //    string token = Task.Run(() => GetTokenAsync()).Result;
        //    string result = Task.Run(() => CallApiAsync(token)).Result;
        //    return result;
        //}
        //protected void btnGetData_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string data = ApiClient.Get("products"); // GET https://your-api-url.com/api/products
        //        lblOutput.Text = data;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblOutput.Text = "Error: " + ex.Message;
        //    }
        //}

        //protected void btnCreate_Click(object sender, EventArgs e)
        //{
        //    var newItem = new { Name = "Sample Item", Price = 100 };
        //    string result = ApiClient.Post("products", newItem); // POST
        //    lblOutput.Text = result;
        //}

    }
}
