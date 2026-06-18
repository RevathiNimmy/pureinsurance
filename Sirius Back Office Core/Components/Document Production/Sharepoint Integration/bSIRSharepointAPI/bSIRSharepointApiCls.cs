using bSIRSharepointApi.Models;
using bSIRSharepointApi.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace bSIRSharepointApi
{
    public class bSIRSharepointApiCls
    {
        static HttpClient _httpClient = null;
        SPClient sPClient = null;
        public SPContextConfiguration model;
        const int kTWO_HUNDRED_FIFTY_MB = 250 * 1024 * 1024; // 250 MB
        const int kCHUNK_SIZE_BYTES = kTWO_HUNDRED_FIFTY_MB; // max 250 mb
        /// <summary>
        /// set the model objects of sharepoint
        /// </summary>
        /// <param name="_model"></param>
        public bSIRSharepointApiCls(SPContextConfiguration _model)
        {
            model = _model;
        }

        public void Initialise()
        {
            // var _uploadPath = $"{sSharepointSite}/_api/web/GetFolderByServerRelativeUrl('{{0}}')/Files/Add(overwrite=true, url='{{1}}')";
            AuthorizationManager authorizationManager = new AuthorizationManager(model);
            _httpClient = authorizationManager.GetHttpClient();
            sPClient = new SPClient(_httpClient);
        }

        /// <summary>
        /// check if document library exists in sharepoint site or not
        /// </summary>
        /// <param name="sDocumentLibrary"></param>
        /// <returns></returns>
        public bool IsDocumentLibraryNotExists(string sDocumentLibrary)
        {
            try
            {
                var url = string.Format($"{model.SharePointSiteURL}/_api/web/lists/getbytitle('{{0}}')?items$select=ID,Title,Folder/ItemCount&$expand=Folder/ItemCount&$filter=FSObjType eq 1",
                                          sDocumentLibrary ?? model.SharePointDocumentLibrary);

                var response = sPClient.ExcuteJson(HttpMethod.Get, url);
                if (response == null) return true;
                return (response.StatusCode == HttpStatusCode.NotFound ? true : false);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Create Document Library
        /// </summary>
        /// <param name="sDocumentLibrary"></param>
        /// <param name="bLinkContentType"></param>
        /// <returns></returns>
        public bool CreateDocumentLibrary(string sDocumentLibrary, bool bLinkContentType = false)
        {
            try
            {
                bool bResult = false;
                if (string.IsNullOrEmpty(sDocumentLibrary)) sDocumentLibrary = model.SharePointDocumentLibrary;

                var url = $"{model.SharePointSiteURL}/_api/Web/lists";
                var body = "{'__metadata':{ type: 'SP.List' }, " +
                       "AllowContentTypes: true," +
                     "BaseTemplate: 101," +
                      "ContentTypesEnabled: true," +
                      "OnQuickLaunch: true," +
                     "Title: '" + sDocumentLibrary + "',Description:'" + sDocumentLibrary + "'" +
                     "}";

                var response = sPClient.ExcuteJson(HttpMethod.Post, url, body);
                if (response == null) return false;
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    bResult = true;
                    model.SharePointDocumentLibrary = sDocumentLibrary;
                    if (bLinkContentType)
                    {
                        bResult = CreateListContentTypes();
                    }
                    return bResult;
                }
                return bResult;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Create Pure Document Feilds
        /// </summary>
        /// <returns></returns>
        public bool CreatePureDocumentFeilds()
        {
            var url = "";
            var sFieldName = "";
            var sfieldSchema = "";

            try
            {
                ClsSPAttributes clsSPAttributes = new ClsSPAttributes();
                foreach (KeyValuePair<string, object> property in clsSPAttributes.SPAttributes())
                {
                    try
                    {
                        sFieldName = property.Key;
                        if (!sFieldName.Equals("Title"))
                        {
                            switch (property.Value.ToString())
                            {
                                case "Text":
                                    sfieldSchema = "<Field Type='Text' DisplayName='" + sFieldName + "' Name='" + sFieldName + "' />";
                                    break;
                                case "Boolean":
                                    sfieldSchema = "<Field Type='Boolean' DisplayName='" + sFieldName + "' Name='" + sFieldName + "' />";
                                    break;
                                case "DateTime":
                                    sfieldSchema = "<Field Type='DateTime' DisplayName='" + sFieldName + "' Name='" + sFieldName + "' />";
                                    break;
                                case "Currency":
                                    sfieldSchema = "<Field Type='Currency' DisplayName='" + sFieldName + "' Name='" + sFieldName + "' />";
                                    break;
                                case "Number":
                                    sfieldSchema = "<Field Type='Number' DisplayName='" + sFieldName + "' Name='" + sFieldName + "' />";
                                    break;

                                default:
                                    sfieldSchema = "<Field Type='Text' DisplayName='" + sFieldName + "' Name='" + sFieldName + "' />";
                                    break;
                            }

                            url = string.Format($"{model.SharePointSiteURL}/_api/web/lists/getbytitle('{{0}}')/fields?$filter=InternalName eq '{{1}}'", model.SharePointDocumentLibrary, sFieldName);

                            var response = sPClient.ExcuteJson(HttpMethod.Get, url);

                            JObject responseObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                            if (responseObj["d"]["results"].Count() == 0)
                            {

                                string body = "{\"parameters\":{\"__metadata\":{\"type\":\"SP.XmlSchemaFieldCreationInformation\"},\"SchemaXml\":\"" + sfieldSchema + "\",\"Options\":4}}";

                                url = string.Format($"{model.SharePointSiteURL}/_api/web/lists/getbytitle('{{0}}')/fields/createfieldasxml", model.SharePointDocumentLibrary);

                                response = sPClient.ExcuteJson(HttpMethod.Post, url, body);
                                if (response.StatusCode != HttpStatusCode.OK) return false;
                            }

                        }
                    }
                    catch
                    {
                        ///Do nothing
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Create List Content Types
        /// </summary>
        /// <param name="sPureGroupName"></param>
        /// <param name="sCustomContentType"></param>
        /// <returns></returns>
        public bool CreateListContentTypes(string sPureGroupName = "SSP Templates", string sCustomContentType = "PureDocument")
        {
            try
            {
                var url = string.Format($"{model.SharePointSiteURL}/_api/web/lists/getbytitle('{{0}}')/contenttypes?$select=id,name&$filter=Name eq '{{1}}'", model.SharePointDocumentLibrary, sCustomContentType);
                var response = sPClient.ExcuteJson(HttpMethod.Get, url);

                bool bCreate = false;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JObject responseObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                    if (responseObj["d"]["results"].Count() == 0)
                    {
                        bCreate = true;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    bCreate = true;
                }
                if (bCreate)
                {
                    var body = "{'__metadata': { type: 'SP.ContentType'}," +
                               "Description: '" + sCustomContentType + "'," +
                               "Name: '" + sCustomContentType + "'," +
                               "Group: '" + sPureGroupName + "'}";

                    url = string.Format($"{model.SharePointSiteURL}/_api/web/lists/getbytitle('{{0}}')/contenttypes", model.SharePointDocumentLibrary);
                    response = sPClient.ExcuteJson(HttpMethod.Post, url, body);
                    if (response == null) bCreate = false;
                    bCreate = (response.StatusCode == HttpStatusCode.Created ? true : false);

                    if (bCreate)
                    {
                        bCreate = CreatePureDocumentFeilds();
                    }
                }
                return bCreate;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Create SharePoint Folders
        /// </summary>
        /// <param name="folderNames"></param>
        /// <returns></returns>
        public bool CreateSharePointFolders(string[] folderNames)
        {
            bool bReturn = false;
            try
            {
                var url = "";
                var addingFolder = model.SharePointDocumentLibrary;
                HttpResponseMessage response = null;
                foreach (string folderName in folderNames)
                {
                    if (folderName != "")
                    {
                        addingFolder += "/";
                        addingFolder += folderName;
                        url = $"{model.SharePointSiteURL}/_api/Web/Folders/add('" + addingFolder + "')";
                        response = sPClient.ExcuteJson(HttpMethod.Post, url);

                        dynamic parsed = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);

                        if (response.StatusCode != HttpStatusCode.OK) bReturn = false;
                    }
                }

                //check folders path exists
                if (IsFolderExists(addingFolder)) bReturn = true;
            }
            catch (Exception ex) { throw ex; }

            return bReturn;
        }

        /// <summary>
        /// Is Folder Exists in sharepoint site
        /// </summary>
        /// <param name="sFolderNames"></param>
        /// <returns></returns>
        public bool IsFolderExists(string sFolderNames)
        {
            try
            {
                var url = $"{model.SharePointSiteURL}/_api/web/GetFolderByServerRelativeUrl('" + sFolderNames + "')/exists";

                var response = sPClient.ExcuteJson(HttpMethod.Get, url);
                if ((bool)(((JValue)JObject.Parse(response.Content.ReadAsStringAsync().Result)["d"]["Exists"]).Value)) return true;

                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Get Document Library by Party Shortname
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public string GetDocumentLibrarybyPartyShortname(string folderName)
        {
            try
            {
                List<string> ListOfDocumentLibraries = new List<string>();
                ListOfDocumentLibraries = GetAllDocumentLibraries();
                if (ListOfDocumentLibraries != null)
                {
                    foreach (string sDocumentLibrary in ListOfDocumentLibraries)
                    {
                        var sNewFolderPath = sDocumentLibrary + "/" + folderName;
                        var url = $"{model.SharePointSiteURL}/_api/web/GetFolderByServerRelativeUrl('" + sNewFolderPath + "')/exists";
                        var response = sPClient.ExcuteJson(HttpMethod.Get, url);
                        if ((bool)(((JValue)JObject.Parse(response.Content.ReadAsStringAsync().Result)["d"]["Exists"]).Value)) return sDocumentLibrary;
                    }
                }
                return "";
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Create Party DocumentLibrary
        /// </summary>
        /// <returns></returns>
        public string CreatePartyDocumentLibrary()
        {
            try
            {
                var url = "";
                var sNewDocumentLibrary = "";
                if (IsDocumentLibraryNotExists(model.SharePointDocumentLibrary))
                {
                    var j = CreateDocumentLibrary(model.SharePointDocumentLibrary, true);
                }
                else
                {
                    url = string.Format($"{model.SharePointSiteURL}/_api/web/GetFolderByServerRelativeUrl('{{0}}')?items$select=ID,Title,Folder/ItemCount&$expand=Folder/ItemCount&$filter=FSObjType eq 1", model.SharePointDocumentLibrary);
                    var response = sPClient.ExcuteJson(HttpMethod.Get, url);

                    JObject responseObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    if ((long)((Newtonsoft.Json.Linq.JValue)responseObj["d"]["ItemCount"]).Value >= 5000)
                    {
                        sNewDocumentLibrary = GetNextDocumentLibrary();
                        if (IsDocumentLibraryNotExists(sNewDocumentLibrary))
                        {
                            var i = CreateDocumentLibrary(sNewDocumentLibrary, true);
                        }
                    }
                }
                return sNewDocumentLibrary.Length == 0 ? model.SharePointDocumentLibrary : sNewDocumentLibrary;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Rename QuoteFolder To PolicyFolder
        /// </summary>
        /// <param name="sQuoteNumber"></param>
        /// <param name="sPolicyNumber"></param>
        /// <param name="sQuoteFolderSharepointSite"></param>
        /// <param name="sPolicyFolderSharepointSite"></param>
        /// <param name="bUpdateFileFields"></param>
        public void RenameQuoteFolderToPolicyFolder(string sQuoteNumber, string sPolicyNumber, string sQuoteFolderSharepointSite, string sPolicyFolderSharepointSite, bool bUpdateFileFields = false)
        {
            try
            {
                string sFolderPath = sQuoteFolderSharepointSite.Replace(model.SharePointSiteURL + "/", "");

                var url = string.Format($"{model.SharePointSiteURL}/_api/web/GetFolderByServerRelativeUrl('{{0}}')/listitemallfields/id", sFolderPath);

                var response = sPClient.ExcuteJson(HttpMethod.Get, url);
                if (response != null && response.StatusCode != HttpStatusCode.NotFound)
                {
                    string sFolderID = "";
                    if (response.StatusCode == System.Net.HttpStatusCode.OK && (JObject.Parse(response.Content.ReadAsStringAsync().Result)["d"]) != null)
                    {
                        sFolderID = ((Newtonsoft.Json.Linq.JValue)JObject.Parse(response.Content.ReadAsStringAsync().Result)["d"]["Id"]).Value.ToString();
                    }

                    url = $"{model.SharePointSiteURL}/_api/web/Lists/GetByTitle('" + model.SharePointDocumentLibrary + "')/Items(" + sFolderID + ")";

                    string fields = "";
                    fields += "\"Title\":\"" + sPolicyNumber + "\",";
                    fields += "\"FileLeafRef\":\"" + sPolicyNumber + "\",";

                    var body = "{\"__metadata\":{\"type\":\"SP.Data." + model.SharePointDocumentLibrary + "Item\"}," + fields + "}";

                    body = body.Replace(",}", "}");
                    body = body.Replace(" ", "_x0020_");

                    _httpClient.DefaultRequestHeaders.Remove("X-HTTP-Method");
                    _httpClient.DefaultRequestHeaders.Remove("If-Match");
                    _httpClient.DefaultRequestHeaders.Add("X-HTTP-Method", "MERGE");
                    _httpClient.DefaultRequestHeaders.Add("If-Match", "*");

                    response = sPClient.ExcuteJson(HttpMethod.Post, url, body);
                }
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Rename Folder
        /// </summary>
        /// <param name="sQuoteFolderSharepointSite"></param>
        /// <param name="sPolicyFolderSharepointSite"></param>
        /// <returns></returns>
        public int RenameFolder(string sQuoteFolderSharepointSite, string sPolicyFolderSharepointSite)
        {
            int nReturn = 1;
            try
            {
                var url = string.Format($"{model.SharePointSiteURL}/_api/web/GetFolderByServerSharepointSite('{{0}}')/moveTo(newurl='{{1}}')",
                    sQuoteFolderSharepointSite, sPolicyFolderSharepointSite);
                var response = sPClient.ExcuteJson(HttpMethod.Post, url);

                if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotFound) { return nReturn; }
                else
                {
                    nReturn = 0;
                    return nReturn;
                }
            }
            catch (Exception ex) { throw ex; }

        }

        /// <summary>
        /// Get File List
        /// </summary>
        /// <param name="destinationUrl"></param>
        /// <returns></returns>
        public DataTable GetFileList(string destinationUrl)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Title", typeof(String));
                dt.Columns.Add("PureUser", typeof(String));
                dt.Columns.Add("DocumentGroup", typeof(String));
                dt.Columns.Add("DocumentSubGroup", typeof(String));
                dt.Columns.Add("FileLeafRef", typeof(String));
                dt.Columns.Add("DocIcon", typeof(String));
                dt.Columns.Add("ModifiedDate", typeof(DateTime));
                dt.Columns.Add("CreatedDate", typeof(DateTime));
                dt.Columns.Add("ServerUrl", typeof(String));
                dt.Columns.Add("InternalOnly", typeof(Boolean));
                dt.Columns.Add("FileRef", typeof(String));
                string sFolderPath = string.Empty;
                if (destinationUrl.Contains("%20") || destinationUrl.Contains(" "))
                {
                    var libpath = destinationUrl.Replace(model.SharePointSiteURL, "");
                    string[] lstDocFolder;
                    lstDocFolder = libpath.Split('/');
                    sFolderPath = destinationUrl.Replace(model.SharePointSiteURL + "/" + lstDocFolder[1] + "/", "");
                }
                else
                {
                    sFolderPath = destinationUrl.Replace(model.SharePointSiteURL.TrimEnd('/') + "/" + model.SharePointDocumentLibrary + "/", "");
                }
                List<string> ListOfDocumentLibraries = new List<string>();
                ListOfDocumentLibraries = GetAllDocumentLibraries();
                var getFileListePath = "";
                foreach (string sDocumentLibrary in ListOfDocumentLibraries)
                {
                    var sNewFolderPath = sDocumentLibrary + "/" + sFolderPath;
                    getFileListePath = $"{model.SharePointSiteURL}/_api/web/GetFolderByServerRelativeUrl('" + sNewFolderPath + "')/files?" +
                        "$select=ListItemAllFields/Title,ListItemAllFields/PureUser,ListItemAllFields/DocumentGroup,ListItemAllFields/DocumentSubGroup,ListItemAllFields/FileLeafRef," +
                        "ListItemAllFields/DocIcon,ListItemAllFields/Created,ListItemAllFields/Modified,ListItemAllFields/InternalOnly,ListItemAllFields/ServerRedirectedEmbedUrl,ListItemAllFields/FileRef&$expand=ListItemAllFields";
                    var response = sPClient.ExcuteJson(HttpMethod.Get, getFileListePath);                      
                    GetResponseDataTable(response, ref dt);
                }
                return dt;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Update List Items
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="nDocId"></param>
        /// <param name="sFileRef"></param>
        public void UpdateListItems(Dictionary<string, object> properties, int nDocId, string sFileRef)
        {
            try
            {
                string id = GetContentTypeId();
                string val = "";
                string fields = "";
                fields += "\"FileRef\":\"" + sFileRef + "\",";
                if (id.Length > 0) fields += "\"ContentTypeId\":\"" + id + "\",";
                foreach (KeyValuePair<string, object> property in properties)
                {
                    if (property.Value != null)
                        val = property.Value.ToString().Replace("&", "&amp;");

                    fields += "\"" + property.Key + "\":\"" + val + "\",";

                }

                var body = "{\"__metadata\":{\"type\":\"SP.Data." + model.SharePointDocumentLibrary + "Item\"}," + fields + "}";

                body = body.Replace(",}", "}");

                var url = string.Format($"{model.SharePointSiteURL}/_api/web/lists/getbytitle('{{0}}')/items('{{1}}')", model.SharePointDocumentLibrary, nDocId);
                var response = sPClient.ExcuteJson(HttpMethod.Post, url, body, true);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Get Content Type Id
        /// </summary>
        /// <returns></returns>
        public string GetContentTypeId()
        {
            try
            {
                var url = string.Format($"{model.SharePointSiteURL}/_api/web/lists/getbytitle('{{0}}')/contenttypes?$select=Id&$filter=Name eq 'PureDocument'", model.SharePointDocumentLibrary);

                var response = sPClient.ExcuteJson(HttpMethod.Get, url);

                return GetResponseData(response, false);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Get File Id
        /// </summary>
        /// <param name="sSharepointSite"></param>
        /// <param name="sSharepointLibrary"></param>
        /// <param name="sDestinationUrl"></param>
        /// <returns></returns>
        public int GetFileId(string sSharepointSite, string sSharepointLibrary, string sDestinationUrl)
        {
            try
            {
                var sFilename = System.IO.Path.GetFileName(sDestinationUrl);
                var url = string.Format($"{sSharepointSite}/_api/web/lists/getbytitle('{{0}}')/items?$select=ID&$filter=FileLeafRef eq '{{1}}'", sSharepointLibrary, sFilename);

                var response = sPClient.ExcuteJson(HttpMethod.Get, url);
                int ID = 0;
                if (int.TryParse(GetResponseData(response, true), out ID))
                    return ID;

                return ID;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Get List By Title
        /// </summary>
        /// <param name="kPureGroupName"></param>
        /// <param name="kCustomContentType"></param>
        /// <returns></returns>
        public bool GetListByTitle(string kPureGroupName = "SSP Templates", string kCustomContentType = "PureDocument")
        {
            try
            {
                var url = string.Format($"{model.SharePointSiteURL}/_api/web/lists/getbytitle('{{0}}')", model.SharePointDocumentLibrary);
                var response = sPClient.ExcuteJson(HttpMethod.Get, url);

                if (response == null || response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    if (IsDocumentLibraryNotExists(model.SharePointDocumentLibrary))
                    {
                        //create document library
                        return CreateDocumentLibrary(model.SharePointDocumentLibrary, true);
                    }
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Found || response.StatusCode == System.Net.HttpStatusCode.OK) return true;

                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Get Sites Group By Name
        /// </summary>
        /// <param name="sSharepointSite"></param>
        /// <param name="sSharepointLibrary"></param>
        /// <param name="sPureGroupName"></param>
        /// <returns></returns>
        public bool GetSitesGroupByName(string sSharepointSite, string sSharepointLibrary, string sPureGroupName = "SSP Templates")
        {
            try
            {
                var url = string.Format($"{sSharepointSite}/_api/web/sitegroups/GetByName('{{0}}')", sPureGroupName);
                var response = sPClient.ExcuteJson(HttpMethod.Get, url);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //create ste group
                    return CreateSitesGroup(sSharepointSite, sPureGroupName);
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Found)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Create Sites Group
        /// </summary>
        /// <param name="sSharepointSite"></param>
        /// <param name="sSharepointLibrary"></param>
        /// <param name="sPureGroupName"></param>
        /// <returns></returns>
        public bool CreateSitesGroup(string sSharepointSite, string sPureGroupName = "SSP Templates")
        {

            try
            {
                var body = "{'__metadata': { type: 'SP.Group' }, " +
                                    "Title: '" + sPureGroupName + "',Description:'" + sPureGroupName + "'" +
                                    "}";

                var url = $"{sSharepointSite}/_api/Web/sitegroups";
                var response = sPClient.ExcuteJson(HttpMethod.Post, url, body);

                if (response.StatusCode == HttpStatusCode.Created) return true;
                return false;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Get List Content Types
        /// </summary>
        /// <param name="sSharepointSite"></param>
        /// <param name="sSharepointLibrary"></param>
        /// <param name="sPureGroupName"></param>
        /// <param name="sCustomContentType"></param>
        /// <returns></returns>
        public bool GetListContentTypes(string sSharepointSite, string sSharepointLibrary, string sPureGroupName = "SSP Templates", string sCustomContentType = "PureDocument")
        {
            try
            {
                var url = string.Format($"{sSharepointSite}/_api/web/lists/getbytitle('{{0}}')/contenttypes?$select=id,name&$filter=Name eq '{{1}}'", sSharepointLibrary, sCustomContentType);

                var response = sPClient.ExcuteJson(HttpMethod.Get, url);
                bool bCreate = false;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JObject responseObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                    if (responseObj["d"]["results"].Count() == 0) bCreate = true;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound) bCreate = true;

                if (bCreate) return CreateListContentTypes(sPureGroupName, sCustomContentType);

                return bCreate;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// UploadFile365
        /// </summary>
        /// <param name="SharepointUrl"></param>
        /// <param name="sUrl"></param>
        /// <param name="sSourceFile"></param>
        /// <param name="sDocumentLibrary"></param>
        public void UploadFile365(string SharepointUrl, string sUrl, string sSourceFile, string sDocumentLibrary)
        {
            try
            {
                var uri = new Uri(sUrl);
                string sDestinationFolder = sUrl;

                if (SharepointUrl.LastIndexOf("/") == SharepointUrl.Length - 1)
                {
                    sDestinationFolder = sDestinationFolder.Replace(SharepointUrl + sDocumentLibrary + "/", "");
                }
                else
                {
                    sDestinationFolder = sDestinationFolder.Replace(SharepointUrl + "/" + sDocumentLibrary + "/", "");
                }

                string[] oFolderCollection = sDestinationFolder.Split('/');

                var addingFolder = model.SharePointDocumentLibrary;

                foreach (string folderName in oFolderCollection)
                {
                    if (folderName != "")
                    {
                        addingFolder += "/";
                        addingFolder += folderName;
                    }
                }

                var fileinfo = new System.IO.FileInfo(sSourceFile);

                if (fileinfo.Length > kTWO_HUNDRED_FIFTY_MB)
                {
                    UploadLargeFile(sSourceFile, addingFolder);
                }
                else
                {
                    UploadSmallFile(sSourceFile, addingFolder);
                }
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Upload Small File
        /// </summary>
        /// <param name="sSourceFile"></param>
        /// <param name="addingFolder"></param>
        void UploadSmallFile(string sSourceFile, string addingFolder)
        {
            var fileinfo = new System.IO.FileInfo(sSourceFile);

            if (fileinfo.Length > kTWO_HUNDRED_FIFTY_MB)
            {
                UploadLargeFile(sSourceFile, addingFolder);
                return;
            }

            string fileName = System.IO.Path.GetFileName(addingFolder);
            addingFolder = addingFolder.Replace(fileName, "");

            if (addingFolder.EndsWith("/")) addingFolder = addingFolder.Substring(0, addingFolder.Length - 1);
            var url = string.Format($"{model.SharePointSiteURL}/_api/web/GetFolderByServerRelativeUrl('{{0}}')/Files/Add(overwrite=true, url='{{1}}')", addingFolder, fileName);
            var response = sPClient.ExcuteJson(HttpMethod.Post, url, byteArrayContent: new ByteArrayContent(System.IO.File.ReadAllBytes(fileinfo.FullName)));
        }

        /// <summary>
        /// UploadLargeFile
        /// </summary>
        /// <param name="sSourceFile"></param>
        /// <param name="addingFolder"></param>
        void UploadLargeFile(string sSourceFile, string addingFolder)
        {
            var fileinfo = new System.IO.FileInfo(sSourceFile);

            string fileName = System.IO.Path.GetFileName(addingFolder);
            addingFolder = addingFolder.Replace(fileName, "");

            if (addingFolder.EndsWith("/")) addingFolder = addingFolder.Substring(0, addingFolder.Length - 1);

            // open the file
            using (var fileStream = System.IO.File.OpenRead(fileinfo.FullName))
            {
                fileStream.Position = 0;

                // generate a unique guid for the upload session
                string uploadId = Guid.NewGuid().ToString();

                long offset = 0;
                int bytesRead = 0;

                var bytes = new byte[kCHUNK_SIZE_BYTES];

                do
                {
                    if (fileStream.Position == 0)
                    {
                        // start chunked upload
                        var _startUploadPath = $"{model.SharePointSiteURL}/_api/web/GetFolderByServerRelativeUrl('{{0}}')/Files/GetByUrlOrAddStub('{{1}}')/StartUpload(uploadId=guid'{{2}}')";
                        _startUploadPath = string.Format(_startUploadPath, addingFolder, fileName, uploadId);
                        var response = sPClient.ExcuteJson(HttpMethod.Post, _startUploadPath);

                    }
                    else if (fileStream.Position < fileStream.Length)
                    {
                        // continue chunked upload
                        var _continueUploadPath = $"{model.SharePointSiteURL}/_api/web/GetFolderByServerRelativeUrl('{{0}}/{{1}}')/ContinueUpload(uploadId=guid'{{2}}',fileOffset={{3}})";
                        _continueUploadPath = string.Format(_continueUploadPath, addingFolder, fileName, uploadId, offset);

                        var response = sPClient.ExcuteJson(HttpMethod.Post, _continueUploadPath, byteArrayContent: new ByteArrayContent(bytes, 0, bytesRead));

                        // read the response to find the offset for the next chunk
                        dynamic parsed = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);

                        offset = (int)parsed.d.ContinueUpload;

                    }
                    else if (fileStream.Position == fileStream.Length)
                    {
                        // finish chunked upload
                        var _finishUploadPath = $"{model.SharePointSiteURL}/_api/web/GetFolderByServerRelativeUrl('{{0}}/{{1}}')/FinishUpload(uploadId=guid'{{2}}',fileOffset={{3}})";
                        _finishUploadPath = string.Format(_finishUploadPath, addingFolder, fileName, uploadId, offset);
                        var response = sPClient.ExcuteJson(HttpMethod.Post, _finishUploadPath, byteArrayContent: new ByteArrayContent(bytes, 0, bytesRead));
                    }

                    bytesRead = fileStream.Read(bytes, 0, bytes.Length);
                }
                while (bytesRead > 0);
            }
        }

        /// <summary>
        /// GetFileLists
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <param name="CopyDestinationPath"></param>
        /// <param name="CopyFiles"></param>
        public void GetFileLists(string FolderPath, string CopyDestinationPath, bool CopyFiles = false)
        {
            try
            {
                var url = $"{model.SharePointSiteURL}/_api/web/GetFolderByServerRelativeUrl('" + FolderPath + "')/files?$select=name";
                var response = sPClient.ExcuteJson(HttpMethod.Get, url);

                if (CopyFiles)
                {
                    List<string> lstFileNames = GetResponseData(response, "Name");
                    if (lstFileNames != null)
                    {
                        foreach (string filename in lstFileNames)
                        {
                            CopyFile(FolderPath + "/" + filename, CopyDestinationPath + "/" + filename);
                        }
                    }
                }
            }

            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Copy File
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <param name="CopyDestinationPath"></param>
        public void CopyFile(string FolderPath, string CopyDestinationPath)
        {
            try
            {
                var body = "{\"srcPath\": {\"__metadata\": {\"type\": \"SP.ResourcePath\"}," +
                       "\"DecodedUrl\": \"" + FolderPath + "\"}," +
                       "\"destPath\": {\"__metadata\":  {\"type\": \"SP.ResourcePath\"}," +
                       "\"DecodedUrl\": \"" + CopyDestinationPath + "\"}}";

                var url = $"{model.SharePointSiteURL}/_api/SP.MoveCopyUtil.CopyFileByPath(overwrite=@a1)?@a1=true";

                var response = sPClient.ExcuteJson(HttpMethod.Post, url, body);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Get Response Data
        /// </summary>
        /// <param name="response"></param>
        /// <param name="bGetSelectQueryData"></param>
        /// <returns></returns>
        private string GetResponseData(HttpResponseMessage response, bool bGetSelectQueryData = false)
        {
            string id = "0";
            if (response.StatusCode == System.Net.HttpStatusCode.OK && (JObject.Parse(response.Content.ReadAsStringAsync().Result)["d"]["results"].Count() != 0))
            {

                JObject responseObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                // get JSON result objects into a list
                IList<JToken> entries = responseObj["d"]["results"].Children().Children().ToList();

                foreach (JToken entry in entries)
                {
                    if (((Newtonsoft.Json.Linq.JProperty)entry).Name == "Id")
                    {
                        if (bGetSelectQueryData)
                        {
                            id = (string)((Newtonsoft.Json.Linq.JProperty)entry).Value;
                            break;
                        }
                        IList<JToken> entriesChild = entry.Children().Children().ToList();
                        foreach (JToken entryChild in entriesChild)
                        {
                            if (((Newtonsoft.Json.Linq.JProperty)entryChild).Name == "StringValue")
                            {
                                id = (string)((Newtonsoft.Json.Linq.JProperty)entryChild).Value;

                                break;
                            }
                        }
                    }
                }

            }
            return id;
        }

        /// <summary>
        /// Get Response Data Table
        /// </summary>
        /// <param name="response"></param>
        /// <param name="dt"></param>
        private void GetResponseDataTable(HttpResponseMessage response, ref DataTable dt)
        {
            List<string> Names = null;
            if (response.StatusCode == System.Net.HttpStatusCode.OK && (JObject.Parse(response.Content.ReadAsStringAsync().Result)["d"]["results"].Count() != 0))
            {

                JObject responseObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                // get JSON result objects into a list
                IList<JToken> entries = responseObj["d"]["results"].Children().Children().ToList();
                Names = new List<string>();
                DataRow dr;
                foreach (JToken entry in entries)
                {
                    if (((Newtonsoft.Json.Linq.JProperty)entry).Name == "ListItemAllFields")
                    {
                        dr = dt.NewRow();
                        IList<JToken> entriesChild = entry.Children().Children().ToList();
                        foreach (JToken entryChild in entriesChild)
                        {
                            switch (((Newtonsoft.Json.Linq.JProperty)entryChild).Name)
                            {
                                case "Title":
                                    dr["Title"] = (string)((Newtonsoft.Json.Linq.JProperty)entryChild).Value;
                                    break;
                                case "PureUser":
                                    dr["PureUser"] = (string)((Newtonsoft.Json.Linq.JProperty)entryChild).Value;
                                    break;
                                case "DocumentGroup":
                                    dr["DocumentGroup"] = (string)((Newtonsoft.Json.Linq.JProperty)entryChild).Value;
                                    break;
                                case "DocumentSubGroup":
                                    dr["DocumentSubGroup"] = (string)((Newtonsoft.Json.Linq.JProperty)entryChild).Value;
                                    break;
                                case "FileLeafRef":
                                    dr["FileLeafRef"] = (string)((Newtonsoft.Json.Linq.JProperty)entryChild).Value;
                                    break;
                                case "DocIcon":
                                    dr["DocIcon"] = (string)((Newtonsoft.Json.Linq.JProperty)entryChild).Value;
                                    break;
                                case "Created":
                                    dr["CreatedDate"] = (DateTime)((Newtonsoft.Json.Linq.JProperty)entryChild).Value;
                                    break;
                                case "Modified":
                                    dr["ModifiedDate"] = (DateTime)((Newtonsoft.Json.Linq.JProperty)entryChild).Value;
                                    break;
                                case "InternalOnly":
                                    if (((Newtonsoft.Json.Linq.JProperty)entryChild).Value.ToString() != "")
                                    {
                                        dr["InternalOnly"] = (bool)((Newtonsoft.Json.Linq.JProperty)entryChild).Value;
                                    }
                                    else
                                    {
                                        dr["InternalOnly"] = false;
                                    }
                                    break;
                                case "ServerRedirectedEmbedUrl":
                                    dr["ServerUrl"] = (string)((Newtonsoft.Json.Linq.JProperty)entryChild).Value;
                                    break;
                                case "FileRef":
                                    dr["FileRef"] = (string)((Newtonsoft.Json.Linq.JProperty)entryChild).Value;
                                    break;

                            }

                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
        }

        /// <summary>
        /// Get Next Document Library
        /// </summary>
        /// <returns></returns>
        private string GetNextDocumentLibrary()
        {
            List<string> lstDocumentLibrary = GetAllDocumentLibraries();
            List<int> lstDocumentLibraryNumbers = new List<int>();
            string sDocumentLibraryNumber = "";
            foreach (string s in lstDocumentLibrary)
            {
                sDocumentLibraryNumber = s.ToLower().Replace(model.SharePointDocumentLibrary.ToLower(), "");
                if (sDocumentLibraryNumber.Length == 0)
                { lstDocumentLibraryNumbers.Add(0); }
                else
                { lstDocumentLibraryNumbers.Add(int.Parse(s.ToLower().Replace(model.SharePointDocumentLibrary.ToLower(), ""))); }
            }
            lstDocumentLibraryNumbers.Sort();
            lstDocumentLibraryNumbers.Reverse();
            string sNewDocumentLibrary = model.SharePointDocumentLibrary + Convert.ToInt32(lstDocumentLibraryNumbers[0]).ToString();
            int nLargestNumber = Convert.ToInt32(lstDocumentLibraryNumbers[0]) + 1;
            var url = string.Format($"{model.SharePointSiteURL}/_api/web/lists/getbytitle('{{0}}')?items$select=ID,Title,Folder/ItemCount&$expand=Folder/ItemCount&$filter=FSObjType eq 1",
                                           model.SharePointDocumentLibrary + Convert.ToInt32(lstDocumentLibraryNumbers[0]).ToString());
            var response = sPClient.ExcuteJson(HttpMethod.Get, url);

            JObject responseObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            if (response.StatusCode == HttpStatusCode.NotFound || (long)((Newtonsoft.Json.Linq.JValue)responseObj["d"]["ItemCount"]).Value >= 5000)
            {
                sNewDocumentLibrary = model.SharePointDocumentLibrary + nLargestNumber.ToString();
            }
            return sNewDocumentLibrary;
        }

        /// <summary>
        /// Get All Document Libraries
        /// </summary>
        /// <returns></returns>
        private List<string> GetAllDocumentLibraries()
        {
            List<string> ListOfDocumentLibraries = null;
            var url = $"{model.SharePointSiteURL}/_api/web/lists?$select=id,Title&$filter=BaseTemplate eq 101 &$orderby=Title desc";
            var response = sPClient.ExcuteJson(HttpMethod.Get, url);

            ListOfDocumentLibraries = new List<string>();
            ListOfDocumentLibraries = GetResponseData(response, "");
            if(ListOfDocumentLibraries != null && ListOfDocumentLibraries.Count>0)
            {
                ListOfDocumentLibraries= ListOfDocumentLibraries.Where(v => v.StartsWith(model.SharePointDocumentLibrary, StringComparison.OrdinalIgnoreCase) && (int.TryParse(v.ToLower().Replace(model.SharePointDocumentLibrary.ToLower(),""), out int result)) ).ToList(); 
            }
            return ListOfDocumentLibraries;
        }

        /// <summary>
        /// Get Response Data
        /// </summary>
        /// <param name="response"></param>
        /// <param name="sEntryName"></param>
        /// <returns></returns>
        private List<string> GetResponseData(HttpResponseMessage response, [Optional] string sEntryName)
        {
            List<string> Names = null;
            if (response.StatusCode == System.Net.HttpStatusCode.OK && (JObject.Parse(response.Content.ReadAsStringAsync().Result)["d"]["results"].Count() != 0))
            {
                JObject responseObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                // get JSON result objects into a list
                IList<JToken> entries = responseObj["d"]["results"].Children().Children().ToList();
                Names = new List<string>();
                foreach (JToken entry in entries)
                {
                    if (((Newtonsoft.Json.Linq.JProperty)entry).Name == (string.IsNullOrEmpty(sEntryName) ? "Title" : sEntryName))
                    {
                        Names.Add((string)((Newtonsoft.Json.Linq.JProperty)entry).Value);
                    }

                }
            }
            return Names;
        }
    }
}
