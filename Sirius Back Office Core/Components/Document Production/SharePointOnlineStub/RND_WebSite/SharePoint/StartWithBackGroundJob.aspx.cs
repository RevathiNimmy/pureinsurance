using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Client;
using System.IO;

using WPFSP = Microsoft.SharePoint.Client;
using System.Security;
using System.Data.SqlClient;
using System.Data;
using System.Xml;

public partial class SharePoint_StartWithBackGroundJob : System.Web.UI.Page
{
  
    private void AddFile()
    {
        string sDocumentLibrary = "PureDocuments";
        string siteURL=textBox1.Text;

       Dictionary<string, object> SPAttributes= ReadFromBackGroundJob();

       string DestinationFilename = Path.GetFileName(txtlocalfile.Text);

       string SharepointPath = BuildDestinationPath(ref siteURL, sDocumentLibrary, SPAttributes["PartyCnt"].ToString(), SPAttributes["PolicyNumber"].ToString(), SPAttributes["ClaimNumber"].ToString(), DestinationFilename, false);

        //Verify Folder if not created, Create on sharepoint
       createSharePointOnlineFolder(SharepointPath);

       return;
        try
        {

           
            //ClientContext context = new ClientContext(textBox1.Text);
            //context.Credentials = SetCredentials();

            //Web web = context.Web;
            ////web.DocumentGroup = "xmy";

            //FileCreationInformation newFile = new FileCreationInformation();
            //newFile.Content = System.IO.File.ReadAllBytes(txtlocalfile.Text);

            //if (string.IsNullOrEmpty(txtSubFolder.Text))
            //{
            //    newFile.Url = textBox1.Text + "/" + sDocumentLibrary + txtRootFolder.Text + "/" + txtfilename.Text;
            //}
            //else
            //{
            //    newFile.Url = textBox1.Text + "/" + sDocumentLibrary + "/" + txtRootFolder.Text + "/" + txtSubFolder.Text + "/" + txtfilename.Text;
            //}

            //List docs = web.Lists.GetByTitle(sDocumentLibrary);
            //Microsoft.SharePoint.Client.File uploadFile = docs.RootFolder.Files.Add(newFile);

            //Microsoft.SharePoint.Client.ListItem item = uploadFile.ListItemAllFields;
            //item["DocumentGroup"] = "Yogi Singh DocumentGroup";
            //item.Update();

            //context.Load(uploadFile);
            //context.ExecuteQuery();

            //lblMessage.Text = "Document uploaded successfully";

            //using (FileStream fs = new FileStream(@"H:\Temp\Files\temp.txt", FileMode.Open))
            //{
            //    WPFSP.File.SaveBinaryDirect(context, textBox1.Text + "/Rakesh/temp.txt", fs, true);
            //    WPFSP.File.SaveBinaryDirect(context, "http://vm-mitsui-rb:81/sites/PureDocuments/Rakesh/temp.txt", fs, true);
            //    WPFSP.File.SaveBinaryDirect(context, "temp.txt", fs, true);

            //}

        }
        catch (Exception ex)
        {

            lblMessage.Text = ex.Message;
        }
    }


    private Dictionary<string, object> ReadFromBackGroundJob()
    {
        Dictionary<string, object> SPAttributes = new Dictionary<string, object>();

        using (SqlConnection con = new SqlConnection("Data Source=(local);Initial Catalog=Pure41;User ID=sirius;Password=$1R1U5"))
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select top 1 background_job_id,party_code,job_xml from background_job where job_status='W'", con);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    rdr.Read();
                    //Set the Tags
                    SPAttributes.Add("BackGroundJobId", rdr["background_job_id"].ToString());
                    SPAttributes.Add("PartyCnt", rdr["party_code"].ToString());


                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(rdr["job_xml"].ToString());

                    rdr.Close();

                    XmlNode root = xdoc.DocumentElement;
                    XmlNodeList parameters = root.SelectSingleNode("JOB").SelectSingleNode("PARAMETERS").SelectNodes("PARAMETER");

                    for (int i = 0; i < parameters.Count - 1; i++)
                    {
                        if (parameters[i].Attributes["name"].Value == "DocumentTemplateGroupID")
                        {
                            SPAttributes.Add("DocumentTemplateGroupID", parameters[i].Attributes["value"].Value);
                        }
                        if (parameters[i].Attributes["name"].Value == "DocumentTemplateGroupID")
                        {
                            SPAttributes.Add("DocumentTemplateSubGroupID", parameters[i].Attributes["value"].Value);
                        }
                    }


                    cmd.CommandText = "spu_SIR_Get_Sharepoint_Tags";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Clear();

                    cmd.Parameters.Add("document_template_id", "");
                    cmd.Parameters.Add("template_group_id", SPAttributes["DocumentTemplateGroupID"].ToString());
                    cmd.Parameters.Add("template_sub_group_id", SPAttributes["DocumentTemplateSubGroupID"].ToString());
                    cmd.Parameters.Add("party_cnt", SPAttributes["PartyCnt"].ToString());
                    cmd.Parameters.Add("insurance_file_cnt", "0");
                    cmd.Parameters.Add("claim_id", "0");
                    cmd.Parameters.Add("background_job_id", SPAttributes["BackGroundJobId"].ToString());

                    SqlDataAdapter adptr = new SqlDataAdapter(cmd);

                    DataSet dDataSet = new DataSet();
                    adptr.Fill(dDataSet);

                    rdr.Dispose();
                    cmd.Dispose();
                    adptr.Dispose();

                    if (dDataSet != null && dDataSet.Tables.Count > 0)
                    {
                        SPAttributes.Add("PartyShortName", dDataSet.Tables[0].Rows[0]["party_shortname"].ToString());
                        SPAttributes.Add("PartyFullName", dDataSet.Tables[0].Rows[0]["party_resolved_name"].ToString());
                        SPAttributes.Add("PolicyNumber", dDataSet.Tables[0].Rows[0]["policy_number"].ToString());
                        SPAttributes.Add("ClaimNumber", dDataSet.Tables[0].Rows[0]["claim_number"].ToString());
                        SPAttributes.Add("ProductCode", dDataSet.Tables[0].Rows[0]["product_code"].ToString());
                        SPAttributes.Add("ClaimPrimaryCause", dDataSet.Tables[0].Rows[0]["claim_primary_cause_description"].ToString());
                        SPAttributes.Add("ClaimIncurredAmount", dDataSet.Tables[0].Rows[0]["claim_incurred_amount"].ToString());
                        SPAttributes.Add("AgentShortName", dDataSet.Tables[0].Rows[0]["agent_shortname"].ToString());
                        SPAttributes.Add("AgentFullName", dDataSet.Tables[0].Rows[0]["agent_resolved_name"].ToString());
                        SPAttributes.Add("InternalOnly", true);
                        SPAttributes.Add("party_cnt", SPAttributes["PartyCnt"].ToString());
                        SPAttributes.Add("insurance_file_cnt", 0);
                        SPAttributes.Add("claim_id", 0);
                        SPAttributes.Add("PureUser", dDataSet.Tables[0].Rows[0]["USER_NAME"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Failed to Load BackGround Job";
            }
            finally {
                
                con.Close();
                con.Dispose();
            }
            return SPAttributes;
        }
    }



    private string BuildDestinationPath(ref string SharepointSite,
         string SharepointLibrary, string PartyShortname,
         string PolicyNumber, string ClaimNumber, string Filename, bool IsGeneratedMail)
    {

       string DestinationFolder = SharepointSite.Trim();
        
        if (SharepointLibrary.Length > 0) {
           DestinationFolder += "/" + (SharepointLibrary.Trim().Replace(" ", "%20")) + "/";
        }

        if ((!Filename.ToUpper().Contains("/REPORTS/")))
        {

            if (((Filename.ToUpper().EndsWith(".EML") || Filename.ToUpper().EndsWith(".MSG"))
                        && IsGeneratedMail))
            {

                DestinationFolder += (PartyShortname.Trim() + "/");
                DestinationFolder += ("Generated Emails" + "/");
            }
            else if ((ClaimNumber.Length > 0))
            {
                DestinationFolder += (PartyShortname.Trim() + "/");
                DestinationFolder += ("Claim" + "/");
                DestinationFolder += (ClaimNumber.Trim() + "/");
            }
            else if ((PolicyNumber.Length > 0))
            {
                DestinationFolder += (PartyShortname.Trim() + "/");
                DestinationFolder += ("Policy" + "/");
                DestinationFolder += (PolicyNumber.Trim() + "/");
            }
            else
            {
                DestinationFolder += (PartyShortname.Trim() + "/");
                DestinationFolder += ("General" + "/");
            }
        }
        
        
        DestinationFolder = DestinationFolder.Replace("//", "/");
        DestinationFolder = DestinationFolder.Replace("http:/", "http://");
        DestinationFolder = DestinationFolder.Replace("https:/", "https://");
        return DestinationFolder;
    }


    private void createSharePointOnlineFolder(string SharepointPath)
    {
        try
        {
            ClientContext clientContext = new ClientContext(textBox1.Text);
            clientContext.Credentials = SetCredentials();
            Web web = clientContext.Web;
            var query = clientContext.LoadQuery(web.Lists.Where(p => p.Title == "PureDocuments"));
            clientContext.ExecuteQuery();
            List list = query.FirstOrDefault();
            var folder = list.RootFolder;

            clientContext.Load(folder);
            clientContext.ExecuteQuery();

            string[] FolderCollection = SharepointPath.Replace(textBox1.Text, "").Split('/');


            //folder = folder.Folders.Add(txtRootFolder.Text);

            //string[] namesArray = txtSubFolder.Text.Split('/');
            //foreach (string name in namesArray)
            //{
            //    folder = folder.Folders.Add(name);
            //}
            ////folder.Update();
            ////list.Update();
            ////clientContext.Load(folder);
            ////clientContext.Load(list);
            //clientContext.ExecuteQuery();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }

    protected void btnGetList_Click(object sender, EventArgs e)
    {
        GetList();
    }
    protected void btnCraeteList_Click(object sender, EventArgs e)
    {
        // Site Collecton URL 
        ClientContext createContext = new ClientContext(textBox1.Text);

        // Retrive Sites 
        Web mySite = createContext.Web;

        createContext.Credentials = SetCredentials();

        // Rettrive Lists Collections.... 
        ListCollection lists = mySite.Lists;
        //List Create Infomation which hold list Title, Description and Template Type
        ListCreationInformation listCreationInfo =
      new ListCreationInformation();


        listCreationInfo.TemplateType = (int)ListTemplateType.DocumentLibrary;


        // Assign value to list 
        listCreationInfo.Title = txtListName.Text;
        listCreationInfo.Description = txtListDescription.Text;



        // Create List      
        WPFSP.List list = mySite.Lists.Add(listCreationInfo);


        // Enable Content Types on list
        list.ContentTypesEnabled = true;


        // Update List Configuration
        list.Update();

        // Send it to SharePoint

        //ContentType ctx; //= createContext.Site.RootWeb.AvailableContentTypes.GetById("0x0101000B1552A3A8236B4D84236102A3243D86");
        IList<ContentTypeId> result = new List<ContentTypeId>();

        //Web mySite = createContext.Web;
        WPFSP.ContentTypeCollection ct = mySite.ContentTypes;
        createContext.Load(ct);
        createContext.ExecuteQuery();
        for (int i = 0; i < ct.Count; i++)
        {

            if (ct[i].Name.Contains("PureDocuments"))
            {
                list.ContentTypes.AddExistingContentType(ct[i]);
                break;
            }
        }


        createContext.Load(list);
        createContext.ExecuteQuery();



        list.RootFolder.UniqueContentTypeOrder = getContentType(txtListName.Text);
        list.RootFolder.Update();
        list.Update();


        createContext.Load(list);
        createContext.ExecuteQuery();



        GetList();
    }
    protected void btnDeleteList_Click(object sender, EventArgs e)
    {
        ClientContext deleteContext = new ClientContext(textBox1.Text);
        deleteContext.Credentials = SetCredentials();
        deleteContext.Web.Lists.GetByTitle(lbList.SelectedValue).DeleteObject();
        deleteContext.ExecuteQuery();
        GetList();
    }

    private void GetList()
    {
        WPFSP.ClientContext context = new WPFSP.ClientContext(textBox1.Text);
        context.Credentials = SetCredentials();
        Web site = context.Web;

        context.Load(site, osite => osite.Title);
        context.ExecuteQuery();
        Title = site.Title;

        ListCollection lists = site.Lists;
        IEnumerable<WPFSP.List> listCollection =
               context.LoadQuery(lists.Include(l => l.Title, l => l.Id));
        context.ExecuteQuery();
        lbList.DataSource = listCollection;
        lbList.DataTextField = "Title";
        lbList.DataBind();





    }


    protected void btnAddFile_Click(object sender, EventArgs e)
    {
        AddFile();
    }


    public void DeleteFolder(string siteUrl, string listName, string relativePath, string folderName)
    {




        ClientContext clientContext = new ClientContext(textBox1.Text);
        clientContext.Credentials = SetCredentials();
        Uri uri = new Uri("http://vm-mitsui-rb:81/sites/PureDocuments/Rakesh/Temp/temp.txt");
        List spList = clientContext.Web.Lists.GetByTitle("Rakesh");
        Microsoft.SharePoint.Client.CamlQuery query = new Microsoft.SharePoint.Client.CamlQuery();
        query.ViewXml = "<View>"
           + "<Query>"
           + "<Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='File'>" + "Temp" + "</Value></Eq></Where>"
           + "</Query>"
           + "</View>";
        // execute the query                
        WPFSP.ListItemCollection listItems = spList.GetItems(query);
        clientContext.Load(listItems);
        clientContext.ExecuteQuery();
        foreach (WPFSP.ListItem listitem in listItems)
        {


            listitem.DeleteObject();
            clientContext.ExecuteQuery();
        }

    }
    protected void btnDeleteFolder_Click(object sender, EventArgs e)
    {
        DeleteFolder(textBox1.Text, "Rakesh", "http://vm-mitsui-rb:81/sites/PureDocuments/Rakesh/Temp", "Temp");
    }

    public IList<ContentTypeId> getContentType(string sListName)
    {
        // Site Collecton URL 
        ClientContext createContext = new ClientContext(textBox1.Text);
        createContext.Credentials = SetCredentials();
        // Retrive Sites 
        Web mySite = createContext.Web;

        List list = mySite.Lists.GetByTitle(sListName);

        IList<ContentTypeId> result = new List<ContentTypeId>();

        WPFSP.ContentTypeCollection ct = list.ContentTypes;
        createContext.Load(ct);

        createContext.ExecuteQuery();
        for (int i = 0; i < ct.Count; i++)
        {

            if (ct[i].Name == "PureDocuments")
            {
                WPFSP.ContentTypeId contentTypeId = ct[i].Id;
                result.Add(contentTypeId);
                break;
            }
        }

        for (int i = 0; i < ct.Count; i++)
        {
            if (ct[i].Name == "Document")
            {
                result.Add(ct[i].Id);
                break;
            }
        }

        return result;
    }

    public ContentType GetPureDocumentContentType()
    {
        // Site Collecton URL 
        ClientContext createContext = new ClientContext(textBox1.Text);
        ContentType contentType = null;
        // Retrive Sites 
        createContext.Credentials = SetCredentials();
        Web mySite = createContext.Web;
        WPFSP.ContentTypeCollection ct = mySite.ContentTypes;
        createContext.Load(ct);
        createContext.ExecuteQuery();
        for (int i = 0; i < ct.Count; i++)
        {
            if (ct[i].Name == "PureDocument")
            {
                contentType = ct[i];
                break;
            }
        }
        return contentType;
    }

    protected void btnCreateFolder_Click(object sender, EventArgs e)
    {
        ClientContext clientContext = new ClientContext(textBox1.Text);
        clientContext.Credentials = SetCredentials();
        Web web = clientContext.Web;
        var query = clientContext.LoadQuery(web.Lists.Where(p => p.Title == "Rakesh"));
        clientContext.ExecuteQuery();
        List list = query.FirstOrDefault();
        var folder = list.RootFolder;

        clientContext.Load(folder);
        clientContext.ExecuteQuery();

        string[] namesArray = new string[] { "Temp", "Folder1" };
        foreach (string name in namesArray)
        {
            folder = folder.Folders.Add(name);
        }
        //folder.Update();
        //list.Update();
        //clientContext.Load(folder);
        //clientContext.Load(list);
        clientContext.ExecuteQuery();
    }

    private SharePointOnlineCredentials SetCredentials()
    {
        SecureString passWord = new SecureString();
        foreach (char c in "@sirius123".ToCharArray()) passWord.AppendChar(c);

        return new SharePointOnlineCredentials("yogender.singh@ssp-worldwide.com", passWord);
    }
}