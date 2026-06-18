using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using RulesEngine.EngineSupport.Csv.Exceptions;
using RulesEngine.Website;
using RulesEngine.EngineCommon;
using System.Collections.ObjectModel;
using System;
using System.Web;
using RulesEngine.BaseSystem.Factories;
using System.IO;
using RulesEngine.BaseSystem.DataAccess;
using System.Text;
using RulesEngine.CodeGenerator;
using RulesEngine.Executor;
using RulesEngine.Website.Code;
using RulesEngine.Website.RuleLineEditor;
using System.Collections.Generic;
using System.Linq;


public partial class WebPages_RuleSets_RuleSets : BasePage
{
    private SessionData sessionData = SessionData.Get();

    private IRuleSet RuleSet
    {
        get
        {
            IRuleSet ruleSet;
            if (sessionData.WorkingRuleSet == null)
            {
                int profileKey = base.RequestParams.ProfileKey;
                int ruleSetKey = base.RequestParams.RuleSetKey;

                ruleSet = LoadRuleSet(profileKey, ruleSetKey);
                sessionData.WorkingRuleSet = ruleSet;

                
                
                
            }
            else
            {
                ruleSet = sessionData.WorkingRuleSet;
            }

            return ruleSet;
        }
    }

    private Collection<string> RuleLineGroups
    {
        get
        {
            return RuleSet.RuleLines.GroupList;
        }
    }

    private string RuleLineGuid
    {
        get
        {
            return RuleLineTreeControl.RuleLineGuid;
        }
        set
        {
            RuleLineTreeControl.RuleLineGuid = value;
        }
    }

    private IRuleLine GetRuleLineByGuid(string lineGuid)
    {
        for (int i = 0; i < this.RuleSet.RuleLines.Count; i++)
        {
            IRuleLine line = this.RuleSet.RuleLines[i];
            if (line.Guid == lineGuid)
            {
                return line;
            }
        }

        return null;
    }

    private IRuleLine NewRuleLineObject()
    {
        IRuleLine line = RuleSetFactory.CreateLine();
        return line;
    }

    private IRuleSet LoadRuleSet(int profileKey, int ruleSetKey)
    {
        IRuleSet ruleSet;
        IRuleSetProfile profile = ProfileFactory.Create(DateTime.Now);

        profile.ProfileKey = profileKey;

        ((BasePage)base.Page).SystemStorageHelper.Load(profile, true,true);
        ruleSet = RuleSetFactory.Create(profile, DateTime.Now);

        ruleSet.RuleSetKey = ruleSetKey;

        ((BasePage)base.Page).SystemStorageHelper.Load(ruleSet);

        return ruleSet;
    }

    protected void Page_Load(object sender, EventArgs e)
    {        
        // If any of the update buttons have been pressed then add data controls earlier than usual
        // to make sure we get the posted data 
        if (this.DataEntryControl.ChangeButtonPressed())
        {
            CreateDataEntryControls();
        }

        this.DataEntryControl.CancelUpdateDelegate = this.DataEntryCancelUpdateHandler;
        this.DataEntryControl.CreatedDelegate = this.DataEntryCreateHandler;
        this.DataEntryControl.DeleteDelegate = this.DataEntryDeleteHandler;
        this.DataEntryControl.UpdateDelegate = this.DataEntryUpdateHandler;
        this.DataEntryControl.GroupRemoved = this.DataEntryGroupRemoved;

        // Populate rule set side box data
        this.lblRuleSetPublishBoxName.Text = this.RuleSet.Name;
        
        this.lblPnlRuleSetInfoName.Text = this.RuleSet.Name;
        this.lblPnlRuleSetInfoEffectiveDate.Text = this.RuleSet.EffectiveDate.ToString(ConfigurationManager.AppSettings["LongDateTimeFormat"]);
        this.lblRuleSetPublishBoxDateEffective.Text = this.RuleSet.EffectiveDate.ToString(ConfigurationManager.AppSettings["ShortDateTimeFormat"]);
        this.lblPnlRuleSetInfoProfile.Text = this.RuleSet.Profile.DisplayName;
        this.lblPnlRuleSetInfoLineCount.Text = this.RuleSet.RuleLines.Count.ToString();

        // Load the username from the LastUpdatedBy id
        DataTable userData = this.SystemStorageHelper.LoadUsersDataTable(this.RuleSet.AuditInfo.LastUpdatedBy);
        if (userData.Rows.Count > 0) 
	        this.lblPnlRuleSetInfoLastUpdatedBy.Text = userData.Rows[0]["RSU_NAME"] as string;
        else
	        this.lblPnlRuleSetInfoLastUpdatedBy.Text = string.Empty;

        this.lblPnlRuleSetInfoLastUpdated.Text = this.RuleSet.AuditInfo.LastUpdatedDate.ToString(ConfigurationManager.AppSettings["LongDateTimeFormat"]);

        this.lblPnlRuleSetInfoStatus.Text = this.RuleSet.Status.ToString();

        if (this.RuleSet.Status == RuleEngineStatus.Live)
        {
            btnSave.Enabled = false;
        }

        if (this.RuleSet.OverrideOutput > 0)
        {
            btnTest.Visible = false;
            btnPublish.Visible = false;
        }
        btnTest.OnClientClick = @"javascript:showPopup('" + pnlTest.ClientID + @"', 'popup testPublishPopup'); return false;";
        btnPublish.OnClientClick = @"javascript:showPopup('" + pnlPublish.ClientID + @"', 'popup testPublishPopup'); return false;";

        this.btnExport.OnClientClick = "javascript:hidePopup('" + pnlExport.ClientID + "');";
        
        if (!IsPostBack)
        {
            this.PublishButton.Enabled = false;

            
            List<KeyValuePair<Int32, Boolean>> liveKeys = new List<KeyValuePair<Int32, Boolean>>();
            this.PublishLocation.Items.Clear();
            Collection<ServerClassification> classes = new Collection<ServerClassification>();
            base.SystemStorageHelper.Load(classes, RuleSet.Profile.System.SystemKey, RuleSet.CustomerKey);
            for (int i = 0; i < classes.Count; i++)
            {
                System.Web.UI.WebControls.ListItem itm = new System.Web.UI.WebControls.ListItem(classes[i].Name, classes[i].ClassificationKey.ToString());
                
                this.PublishLocation.Items.Add(itm);
                this.PublishButton.Enabled = true;

                liveKeys.Add(new KeyValuePair<Int32, Boolean>(classes[i].ClassificationKey, classes[i].Live));
            }
            hfListOfLiveAndIntermediateList = liveKeys;

            if (classes.Count > 0)
            {
                pnlPublishLive.Visible = (classes[0].Live);
                pnlPublishIntermediate.Visible = !(classes[0].Live);
            }
        }

        SetupCopyRules();

        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "RuleLineFunctions.js", "<script type=\"text/javascript\" src=\"" + VirtualPathUtility.ToAbsolute("~/JavaScript/RuleLineFunctions.js") + "\"></script>");
    }

    public List<KeyValuePair<Int32, Boolean>> hfListOfLiveAndIntermediateList
    {
        get {
            String[] values = hfListOfLiveAndIntermediate.Value.Split('|');

            List<KeyValuePair<Int32, Boolean>> returnList = new List<KeyValuePair<int, bool>>();
            foreach (String item in values)
            {
                if (item.Equals(String.Empty)) continue;

                String[] keyValue = item.Split(',');
                returnList.Add(new KeyValuePair<Int32, Boolean>(Convert.ToInt32(keyValue[0]), Convert.ToBoolean(keyValue[1])));
            }

            return returnList;
        }
        set {
            StringBuilder builder = new StringBuilder();

            foreach (KeyValuePair<Int32, Boolean> item in value)
	        {
                builder.Append(item.Key);
                builder.Append(",");
                builder.Append(item.Value);
		        builder.Append("|");
	        }

            hfListOfLiveAndIntermediate.Value = builder.ToString();
        }
    }

    private void DataEntryGroupRemoved(object sender, EventArgs e)
    {
        RuleLineTreeControl.PopulateTree();
        this.RuleLineGuid = string.Empty;
    }

    private void DisableMainMenu()
    {
        Control foundControl;
        FindFirstControl(this.Master.Controls, "pnlDreMainMenu", out foundControl);

        Panel menuPanel = foundControl as Panel;

        if (menuPanel != null)
            menuPanel.Enabled = false;
    }

    private void SetupCopyRules()
    {
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "RuleLineFunctions.js", "<script type=\"text/javascript\" src=\"" + VirtualPathUtility.ToAbsolute("~/JavaScript/RuleLineFunctions.js") + "\"></script>");
        txtCopySourceStart.Attributes.Add("onkeypress", "return AllowNumbersOnly(event)");
        txtCopySourceEnd.Attributes.Add("onkeypress", "return AllowNumbersOnly(event)");
        txtCopyTargetLineNumber.Attributes.Add("onkeypress", "return AllowNumbersOnly(event)");
        ddlCopy.Attributes.Add("onchange", "javascript:HideShowCopyDivs(this.value);");

        ddlSourceGroup.DataSource = this.RuleSet.RuleLines.GroupList;
        ddlSourceGroup.DataBind();
    }

    public string GetCopyValidationFunction()
    {
        StringBuilder lineNumbers = new StringBuilder();

        for (int ndx = 0; ndx < this.RuleSet.RuleLines.Count; ndx++)
        {
            if (!this.RuleSet.RuleLines[ndx].Deleted)
            {
                lineNumbers.Append(lineNumbers.Length > 0 ? "," : string.Empty);
                lineNumbers.Append(this.RuleSet.RuleLines[ndx].LineNumber.ToString());
            }

        }
        return string.Format("javascript:return ValidateCopyInformation('{0}','{1}','{2}','{3}','{4}','{5}');", ddlCopy.ClientID, txtCopySourceStart.ClientID, txtCopySourceEnd.ClientID, txtCopyTargetLineNumber.ClientID, ddlSourceGroup.ClientID, lineNumbers);
    }

    private void CreateDataEntryControls()
    {        
        this.DataEntryControl.AddDataEntryControls(this.GetRuleLineByGuid(this.RuleLineGuid), this.RuleLineGroups);

        this.treePropertyExplorer.ClientIdsToMakeTargets = this.DataEntryControl.TargetControls;
        this.treePropertyExplorer.RuleSetProfile = this.RuleSet.Profile;
        this.treePropertyExplorer.OveriddenOutputObject = this.RuleSet.OverrideOutputObjectname;
        try
        {
            this.treePropertyExplorer.Populate();
        }
        catch(NullReferenceException) // Input object was null
        {
            string js = "showPopup('" + this.pnlProfileCorrupted.ClientID + "','popup corruptedProfilePopup');";
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowCorruptedInputPopup", "<script type=\"text/javascript\">/*\n<![CDATA[*/\n" + js + "\n/*]]>*/</script>");
            this.profileCorruptedEditProfileLink.HRef = "~/WebPages/Profiles/ProfileEdit.aspx?" + RequestParameters.ProfileKeyParam + "=" + base.RequestParams.ProfileKey;
        }
    }

    /// <summary>
    /// Overload to allow us to create data entry controls after load to avoid posted back data
    /// overriding the values in the controls
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoadComplete(EventArgs e)
    {
        base.OnLoadComplete(e);
        CreateDataEntryControls();
        btnCopy.OnClientClick = GetCopyValidationFunction();
        if (RuleLineTreeControl.RuleLineGuid != string.Empty)
        {
            this.DataEntryControl.SetDataEntryButtons(DataEntryMode.Update);
        }
        else
        {
            this.DataEntryControl.SetDataEntryButtons(DataEntryMode.Create);
        }

        DisableMainMenu();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool stillHaveLock = StillHaveLock(LockTypeEnum.RuleSet, this.RuleSet.RuleSetKey);
        if (stillHaveLock == false)
        {
            LostLock();
            return;
        }

        Save();
    }

    private void Save()
    {
        if (this.RuleSet.Status == RuleEngineStatus.Live)
        {
            throw new Exception("Cannot edit live RuleSet");
        }

        ((BasePage)base.Page).SystemStorageHelper.Save(this.RuleSet);
    }

    protected void DataEntryCancelUpdateHandler(object sender, EventArgs e)
    {
        this.RuleLineGuid = "";
        CreateDataEntryControls();
    }

    protected void DataEntryCreateHandler(object sender, EventArgs e)
    {
        if (this.RuleSet.Status == RuleEngineStatus.Live)
        {
            throw new Exception("Cannot edit live RuleSet");
        }

        IRuleLine line = NewRuleLineObject();

        this.DataEntryControl.SetEnteredValuesBackToLine(line);
        if (line.Task != string.Empty)
        {
            this.RuleSet.RuleLines.Add(line);
        }

        // Check task config to see if the creation of this rule line type necessitates the creation
        // of a corresponding task afterwards
        string taskName = line.Task;
        if (taskName != string.Empty)
        {
            DynamicRuleEngineTaskConfigTask task = this.TaskConfig.GetTaskByInternalName(taskName);

            if (task != null && task.AutoCreateEndTaskSpecified)
            {
                IRuleLine endTaskLine = NewRuleLineObject();
                endTaskLine.Task = task.AutoCreateEndTask.ToString();
                endTaskLine.LineGroup = line.LineGroup;
                endTaskLine.LineNumber = line.LineNumber + 1;
                this.RuleSet.RuleLines.Add(endTaskLine);

                line.LinkedLine = endTaskLine;
                endTaskLine.LinkedLine = line;
            }
        }

        
        this.RuleSet.RuleLines.ReOrderList();
        this.DataEntryControl.setLinenumber(line.LineNumber + 1);
        RuleLineTreeControl.PopulateTree();
        this.RuleLineGuid = string.Empty;
        
    }

    protected void DataEntryUpdateHandler(object sender, EventArgs e)
    {
        if (this.RuleSet.Status == RuleEngineStatus.Live)
        {
            throw new Exception("Cannot edit live RuleSet");
        }

        IRuleLine line = this.GetRuleLineByGuid(this.RuleLineGuid);
        this.DataEntryControl.SetEnteredValuesBackToLine(line);
        this.RuleSet.RuleLines.ReOrderList();
        RuleLineTreeControl.PopulateTree();
        this.RuleLineGuid = string.Empty;
    }

    protected void DataEntryDeleteHandler(object sender, EventArgs e)
    {
        if (this.RuleSet.Status == RuleEngineStatus.Live)
        {
            throw new Exception("Cannot edit live RuleSet");
        }

        if (this.RuleLineGuid != "")
        {
            IRuleLine line = this.GetRuleLineByGuid(this.RuleLineGuid);
            line.Deleted = true;

            IRuleLine linkedLine = line.LinkedLine;
            if (linkedLine != null)
                linkedLine.Deleted = true;

        }

        RuleLineTreeControl.PopulateTree();
        this.RuleLineGuid = string.Empty;
    }
    
    /// <summary>
    /// Adds a start up script to the generated page to trigger an auto post back of the grid
    /// views to refresh them. This is done because they are already bound by the time we can get
    /// post back data to set back to changed/entered rule lines.
    /// </summary>
    //private void ForceAjaxRefreshOfGrid()
    //{
    //    Control ctl;
    //    int i = 0;
    //    BasePage.FindFirstControl(this.Controls, "upd" + i, out ctl);

    //    StringBuilder sb = new StringBuilder();
    //    while (ctl != null)
    //    {
    //        sb.AppendLine("try{" + ClientScript.GetPostBackClientHyperlink(ctl, "", false) + ";}catch(err){}");
    //        BasePage.FindFirstControl(this.Controls, "upd" + i, out ctl);
    //        i++;
    //    }
    //    this.ClientScript.RegisterStartupScript(this.GetType(), "postUpdateGridRefresh", sb.ToString(), true);

    //}

    protected void btnExport_Click(object sender, EventArgs e)
    {
        bool stillHaveLock = StillHaveLock(LockTypeEnum.RuleSet, this.RuleSet.RuleSetKey);
        if (stillHaveLock == false)
        {
            LostLock();
            return;
        }

        base.NavigateTo.RuleSetDownload(this.rsExportFirstlineHeaders.Checked);
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        if (IsValid == false)
            return;

        bool stillHaveLock = StillHaveLock(LockTypeEnum.RuleSet, this.RuleSet.RuleSetKey);
        if (stillHaveLock == false)
        {
            LostLock();
            return;
        }

        try
        {
            using (StreamReader reader = new StreamReader(new MemoryStream(this.rsImportFile.FileBytes)))
            {
                RuleSetImportExport.ImportRuleSetLines(reader.ReadToEnd(), this.RuleSet, this.rsImportOverwriteOrAppend.SelectedValue == "Append", this.rsImportFirstlineHeaders.Checked);
            }

            this.RuleSet.RuleLines.ReOrderList();
            RuleLineTreeControl.PopulateTree();
        }
        catch (MalformedCsvException ex)
        {
            StringBuilder message = new StringBuilder();
            message.Append(Path.GetFileName(this.rsImportFile.FileName));
            message.Append(" is corrupt near line ");
            message.Append(ex.CurrentRecordIndex);
            message.Append(", field ");
            message.Append(ex.CurrentFieldIndex);
    
            ShowPopup("ImportDialog", pnlImport.ClientID, "exportImportRuleSetPopup");
            importCustomValidator.ToolTip = message.ToString();
            importCustomValidator.IsValid = false;
            //int a = 0;
        }
        
    }

    

    protected void btnPublish_Click(object sender, EventArgs e)
    {
        bool stillHaveLock = StillHaveLock(LockTypeEnum.RuleSet, this.RuleSet.RuleSetKey);
        if (stillHaveLock == false)
        {
            LostLock();
            return;
        }

        Save();
        bool success = PublishHelper.Publish(this, this.RuleSet, Convert.ToInt32(PublishLocation.SelectedValue));

        if (success)
        {
            UpdateRuleSetAfterPublishing();
        }
    }

    private void UpdateRuleSetAfterPublishing()
    {
        Int32 index = Convert.ToInt32(PublishLocation.SelectedValue);
        List<KeyValuePair<Int32, Boolean>> list = hfListOfLiveAndIntermediateList;
        KeyValuePair<Int32, Boolean> value = list.FirstOrDefault(p => p.Key == index);
        Boolean livePublish = value.Value;

        RuleSet.PublishDate = DateTime.Now;
        RuleSet.PublishUserKey = sessionData.CurrentUserSession.User.UserKey;

        if (livePublish)
        {
            RuleSet.Status = RuleEngineStatus.Live;
            RuleSet.Profile.Status = RuleEngineStatus.Live;

            SystemStorageHelper.Save(RuleSet.Profile);
        }
        
        SystemStorageHelper.Save(RuleSet);
    }

    protected void btnTest_Click(object sender, EventArgs e)
    {
        bool stillHaveLock = StillHaveLock(LockTypeEnum.RuleSet, this.RuleSet.RuleSetKey);
        if (stillHaveLock == false)
        {
            LostLock();
            return;
        }

        //string xml = Encoding.ASCII.GetString(testFileImport.FileBytes);
        RateLibrary rl = new RateLibrary();
        LibraryGenerator lib = new LibraryGenerator(rl, this.RuleSet, new SystemStorageHelper(sessionData.CurrentUserSession).GridHelper, DateTime.Now);
        if (lib.Generate())
        {
            //RuleSetExecutor ex = new RuleSetExecutor("");
            //RuleSetReturn ret = ex.ExecuteRuleSet(xml, DateTime.Now, this.RuleSet.Profile.Token, this.RuleSet.RuleSetKey); //, lib.Output.BinaryData
            //this.sessionData.RuleSetTestResult = ret.Output[0].Output;
            string path = VirtualPathUtility.ToAbsolute("~/Handlers/RuleSets/RuleSetTest.ashx");
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('" + path + "');", true);
        }
        else
        {
            sessionData.CompilerErrors = lib.CompilerErrors;
            sessionData.GeneratedCode = rl.GeneratedCode;
            base.NavigateTo.CompilationErrors();
        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        bool stillHaveLock = StillHaveLock(LockTypeEnum.RuleSet, this.RuleSet.RuleSetKey);
        if (stillHaveLock == false)
        {
            LostLock();
            return;
        }

        int sourceStartLineNumber;
        int sourceEndLineNumber;
        int targetLineNumber;

        if (ddlCopy.SelectedIndex == 0) // Lines
        {
            sourceStartLineNumber = Convert.ToInt32(txtCopySourceStart.Text);
            sourceEndLineNumber = Convert.ToInt32(txtCopySourceEnd.Text);
        }
        else
        {
            sourceStartLineNumber = this.RuleSet.RuleLines.FirstLineInGroup(ddlSourceGroup.Text);
            sourceEndLineNumber = this.RuleSet.RuleLines.LastLineInGroup(ddlSourceGroup.Text);
        }

        Int32.TryParse(txtCopyTargetLineNumber.Text, out targetLineNumber);


        string copyMode = hdnCopyMode.Value;

        switch (copyMode)
        {
            case "Copy":
                this.RuleSet.RuleLines.Copy(sourceStartLineNumber, sourceEndLineNumber, targetLineNumber, false);
                break;
            case "Move":
                this.RuleSet.RuleLines.Copy(sourceStartLineNumber, sourceEndLineNumber, targetLineNumber, true);
                break;
        }
        
        RuleLineTreeControl.PopulateTree();
    }

    private void LostLock()
    {
        ShowPopup("LockHasBeenLostPopup", this.pnlLockHasBeenLost.ClientID, "lockedInformation");
    }

    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        GoBack();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        bool stillHaveLock = StillHaveLock(LockTypeEnum.RuleSet, this.RuleSet.RuleSetKey);
        if (stillHaveLock == false)
        {
            LostLock();
            return;
        }

        int sourceStartLineNumber;
        int sourceEndLineNumber;

        if (ddlDelete.SelectedIndex == 0) // Lines
        {
            sourceStartLineNumber = Convert.ToInt32(txtDeleteSourceStart.Text);
            sourceEndLineNumber = Convert.ToInt32(txtDeleteSourceEnd.Text);
        }
        else
        {
            sourceStartLineNumber = this.RuleSet.RuleLines.FirstLineInGroup(ddlDeleteSourceGroup.Text);
            sourceEndLineNumber = this.RuleSet.RuleLines.LastLineInGroup(ddlDeleteSourceGroup.Text);
        }

        this.RuleSet.RuleLines.Delete(sourceStartLineNumber, sourceEndLineNumber);
         
        RuleLineTreeControl.PopulateTree();
    }

    protected void PublishLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<KeyValuePair<Int32, Boolean>> list = hfListOfLiveAndIntermediateList;
        DropDownList ddd = (DropDownList)sender;
        Int32 index = Convert.ToInt32(ddd.SelectedValue);

        KeyValuePair<Int32, Boolean> value = list.FirstOrDefault(p => p.Key == index);

        pnlPublishLive.Visible = (value.Value);
        pnlPublishIntermediate.Visible = !(value.Value);

        ShowPopup("LockHasBeenLostPopup", pnlPublish.ClientID, "testPublishPopup");
    }

    private void ShowPopup(string javascriptName, string clientId, string cssClass)
    {
        String js = "showPopup('" + clientId + "','popup " + cssClass + "')";
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), javascriptName, "<script type=\"text/javascript\">/*\n<![CDATA[*/\n" + js + "\n/*]]>*/</script>");
    }

    protected void btnConfirmBack_Click(object sender, EventArgs e)
    {
        GoBack();
    }

    protected void btnConfirmCancel_Click(object sender, EventArgs e)
    {
        bool stillHaveLock = StillHaveLock(LockTypeEnum.RuleSet, this.RuleSet.RuleSetKey);
        if (stillHaveLock == false)
        {
            LostLock();
            return;
        }

        int profileKey = this.RuleSet.Profile.ProfileKey;
        int ruleSetKey = this.RuleSet.RuleSetKey;
        sessionData.WorkingRuleSet = null;
        base.NavigateTo.RuleSetEditRulePage(profileKey, ruleSetKey);
    }

    private void GoBack()
    {
        ReleaseLock(LockTypeEnum.RuleSet, this.RuleSet.RuleSetKey);

        base.NavigateTo.RuleSetListPage();
    }

    protected void importCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (this.rsImportFile.HasFile == false)
        {
            ShowPopup("ImportDialog", pnlImport.ClientID, "exportImportRuleSetPopup");

            importCustomValidator.Visible = true;
            importCustomValidator.ToolTip = "No file";
            
            args.IsValid = false;
            
            return;
        }

        string extension = Path.GetExtension(this.rsImportFile.FileName);
        if (extension.Equals(".csv") == false)
        {
            ShowPopup("ImportDialog", pnlImport.ClientID, "exportImportRuleSetPopup");

            importCustomValidator.Visible = true;
            importCustomValidator.ToolTip = "Not a csv";
            
            args.IsValid = false;
            
            return;
        }

        // Validate
        if (this.rsImportFile.FileBytes.Length <= 0)
        {
            ShowPopup("ImportDialog", pnlImport.ClientID, "exportImportRuleSetPopup");

            importCustomValidator.Visible = true;
            importCustomValidator.ToolTip = "Select file";

            args.IsValid = false;

            return;
        }

        args.IsValid = true;
    }
}
