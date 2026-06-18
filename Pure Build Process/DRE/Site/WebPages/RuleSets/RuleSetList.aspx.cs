using System.Web;
using RulesEngine.Website;
using System.Web.UI.WebControls;
using System;
using RulesEngine.EngineCommon;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RulesEngine.BaseSystem.Factories;
using System.Data;
using System.Configuration;
using RulesEngine.Website.WebUserControls;

public partial class RuleSets : BasePage
{
    private const string DDL_SYSTEM = "ddlSys";
    private const string DDL_CUSTOMER = "ddlCust";
    private const string DDL_PROFILE = "ddlProf";
    
    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            int systemKey = (base.CurrentUser.SuperUser ? 0 : base.CurrentUser.System.SystemKey);
            int customerKey = (base.CurrentUser.Administrator ? 0 : base.CurrentUser.Customer.CustomerKey);
            int profileKey = 0;
            LoadRuleSets(systemKey, customerKey, profileKey);
        }

        ceEffectiveDate.Format = ConfigurationManager.AppSettings["ShortDateFormat"];

        Page.ClientScript.RegisterStartupScript(cvEffectiveDate.GetType(), "asdasd", @"
                <script type='text/javascript' language='javascript'>
                    ValidatorHookupControlID('" + txtEffectiveDate.ClientID + @"', document.all['" + cvEffectiveDate.ClientID + @"']);
                    ValidatorHookupControlID('" + ddlEffectiveTimeHours.ClientID + @"', document.all['" + cvEffectiveDate.ClientID + @"']);
                    ValidatorHookupControlID('" + ddlEffectiveTimeMinutes.ClientID + @"', document.all['" + cvEffectiveDate.ClientID + @"']);
                </script>
            ");

        Page.ClientScript.RegisterStartupScript(cvEffectiveDate.GetType(), "qwqeqwe", @"
                <script type='text/javascript' language='javascript'>
                    ValidatorHookupControlID('" + cbxCreateNewRuleSet.ClientID + @"', document.all['" + cvNewRuleSetName.ClientID + @"']);
                    ValidatorHookupControlID('" + txtNewRuleSetName.ClientID + @"', document.all['" + cvNewRuleSetName.ClientID + @"']);
                </script>
            ");

        Page.ClientScript.RegisterStartupScript(cvEffectiveDate.GetType(), "ertertert", @"
                <script type='text/javascript' language='javascript'>
                    ValidatorHookupControlID('" + cbxCreateNewRuleSet.ClientID + @"', document.all['" + cvNewRuleSetName.ClientID + @"']);
                    ValidatorHookupControlID('" + txtNewRuleSetName.ClientID + @"', document.all['" + cvNewRuleSetName.ClientID + @"']);
                    ValidatorHookupControlID('" + txtEffectiveDate.ClientID + @"', document.all['" + cvEffectiveDate.ClientID + @"']);
                    ValidatorHookupControlID('" + ddlEffectiveTimeHours.ClientID + @"', document.all['" + cvEffectiveDate.ClientID + @"']);
                    ValidatorHookupControlID('" + ddlEffectiveTimeMinutes.ClientID + @"', document.all['" + cvEffectiveDate.ClientID + @"']);
                </script>
            ");

        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "RuleLineFunctions.js", "<script type=\"text/javascript\" src=\"" + VirtualPathUtility.ToAbsolute("~/JavaScript/RuleLineFunctions.js") + "\"></script>");
    }

    #endregion

    #region Control Events

    protected void btnAddNewRuleSet_Click(object sender, EventArgs e)
    {
        DropDownList ddlSystem = ((DropDownList)gvRuleSets.HeaderRow.FindControl(DDL_SYSTEM));
        DropDownList ddlCustomer = ((DropDownList)gvRuleSets.HeaderRow.FindControl(DDL_CUSTOMER));
        DropDownList ddlProfile = ((DropDownList)gvRuleSets.HeaderRow.FindControl(DDL_PROFILE));

        int systemKey = Convert.ToInt32(ddlSystem.SelectedValue);
        int customerKey = Convert.ToInt32(ddlCustomer.SelectedValue);
        int profileKey = 0;
        if(!String.IsNullOrEmpty(ddlProfile.SelectedValue))
        {
            profileKey = Convert.ToInt32(ddlProfile.SelectedValue);
        }
        
        base.NavigateTo.RuleSetAddPage(systemKey, customerKey, profileKey);
    }
    
    protected void gvRuleSet_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandSource is GridView)
            return;

        e = base.getGridViewCommandEventArgs(sender, e);

        switch (e.CommandName)
        {
            case "Sort":
                base.SortExpression(e.CommandArgument.ToString());
                LoadRuleSets();
                break;

            case "EditRuleSet":
            case "EditRules":
                int ruleSetKey = Convert.ToInt32(e.CommandArgument.ToString().Split(',')[0]);

                DataTable locks = RequestLock(LockTypeEnum.RuleSet, ruleSetKey);
                
                if (locks.Rows.Count == 0)
                {
                    base.HandleGridNavigation(e);
                }
                else
                {
                    RuleSetIsLocked(locks);
                }
                break;

            case "CopyRuleset":
                CopyRuleSet(Convert.ToInt32(e.CommandArgument));
                break;

            default:
                base.HandleGridNavigation(e);
                break;
         }
    }

    private void RuleSetIsLocked(DataTable locks)
    {
        string js = "showPopup('" + this.pnlLockedInformation.ClientID + "','popup lockedInformation');";
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowLockedInformationPopup", "<script type=\"text/javascript\">/*\n<![CDATA[*/\n" + js + "\n/*]]>*/</script>");

        hfdEntityID.Value = (string) locks.Rows[0]["ELK_ENTITY_KEY"];

        lblPnlLockedInformationUsername.Text = (string)(locks.Rows[0]["RSU_LOGON_NAME"]);
        lblPnlLockedInformationName.Text = (string)(locks.Rows[0]["RSU_NAME"]);
        
        lblPnlLockedInformationEmailHyperLink.Text = (string)(locks.Rows[0]["RSU_EMAIL"]);
        lblPnlLockedInformationEmailHyperLink.NavigateUrl = "mailto:" + lblPnlLockedInformationEmailHyperLink.Text;

        lblPnlLockedInformationLockTimeOut.Text = DateTime.Parse((string)locks.Rows[0]["ELK_LOCK_TIMEOUT_DATE"]).ToString(ConfigurationManager.AppSettings["LongDateTimeFormat"]);
        lblPnlLockedInformationLockCreated.Text = DateTime.Parse((string)locks.Rows[0]["ELK_CREATED_DATE"]).ToString(ConfigurationManager.AppSettings["LongDateTimeFormat"]);

        btnForceUnlock.Enabled = AccessManager.AllowAccess(ResourceTypes.UnlockRuleSet, PermissionTypes.FullAccess);
    }

    protected void gvRuleSet_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    ASP.PopupMenuControl popupMenuControl = (ASP.PopupMenuControl)e.Row.FindControl("PopupMenu");            
        //    HoverMenuExtender hoveMenu = (HoverMenuExtender)popupMenuControl.FindControl("hoverMenu");
        //    e.Row.ID = e.Row.RowIndex.ToString();
        //    hoveMenu.TargetControlID = e.Row.ID;   
        //}
    }

    protected void gvRuleSet_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRuleSets.PageIndex = e.NewPageIndex;
        LoadRuleSets();
    }

    protected void ddlSystem_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadRuleSets();
    }
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadRuleSets();
    }
    protected void ddlProfile_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadRuleSets();
    }

    #endregion

    #region Private Members

    private void LoadSystems(int systemKey, int customerKey)
    {
        DropDownList ddlSystem = ((DropDownList)gvRuleSets.HeaderRow.FindControl(DDL_SYSTEM));
        Collection<IRuleSystem> systems = new Collection<IRuleSystem>();
        base.SystemStorageHelper.Load(systems);

        if (systems.Count > 0)
        {
            if (base.CurrentUser.SuperUser)
            {
                IRuleSystem dummy = RulesEngine.BaseSystem.Factories.SystemFactory.Create("All systems...");
                dummy.SystemKey = 0;
                systems.Insert(0, dummy);
            }
            else
            {
                ddlSystem.Visible = false;
            }

            ddlSystem.DataSource = systems;
            ddlSystem.DataTextField = "Name";
            ddlSystem.DataValueField = "SystemKey";

            ddlSystem.DataBind();

            List<IRuleSystem> systemList = new List<IRuleSystem>(systems);
            ddlSystem.SelectedIndex = systemList.FindIndex(delegate(IRuleSystem system) { return system.SystemKey == systemKey; });

            LoadCustomers(systemKey, customerKey);
        }
    }

    private void LoadCustomers(int systemKey, int customerKey)
    {
        DropDownList ddlSystem = ((DropDownList)gvRuleSets.HeaderRow.FindControl(DDL_SYSTEM));
        DropDownList ddlCustomer = ((DropDownList)gvRuleSets.HeaderRow.FindControl(DDL_CUSTOMER));
        if(ddlSystem.SelectedIndex >= 0)
        {
            Collection<IRuleCustomer> customers = new Collection<IRuleCustomer>();
            base.SystemStorageHelper.Load(customers, systemKey);
            if (customers.Count > 0)
            {
                if (base.CurrentUser.Administrator)
                {
                    IRuleSystem dummySystem = RulesEngine.BaseSystem.Factories.SystemFactory.Create("All systems...");
                    IRuleCustomer dummyCustomer = RulesEngine.BaseSystem.Factories.CustomerFactory.Create(dummySystem);
                    dummyCustomer.CustomerKey = 0;
                    dummyCustomer.Name = "All customers...";
                    customers.Insert(0, dummyCustomer);
                }
                else
                {
                    ddlCustomer.Visible = false;
                }
            }

            ddlCustomer.DataSource = customers;
            ddlCustomer.DataTextField = "Name";
            ddlCustomer.DataValueField = "CustomerKey";

            ddlCustomer.DataBind();

            List<IRuleCustomer> customerList = new List<IRuleCustomer>(customers);
            ddlCustomer.SelectedIndex = customerList.FindIndex(delegate(IRuleCustomer customer) { return customer.CustomerKey == customerKey; });
        }
    }
    
    private void LoadProfiles(int systemKey, int customerKey, int profileKey)
    {
        DropDownList ddlProfile = ((DropDownList)gvRuleSets.HeaderRow.FindControl(DDL_PROFILE));

        Collection<IRuleSetProfile> profiles = new Collection<IRuleSetProfile>();
        
        base.SystemStorageHelper.Load(profiles, systemKey, customerKey, false);
        if (profiles.Count > 0)
        {
            IRuleSetProfile profile = RulesEngine.BaseSystem.Factories.ProfileFactory.Create(DateTime.Now);
            profile.Name = "All Profiles...";
            profile.ProfileKey = 0;
            profiles.Insert(0, profile);
        }

        ddlProfile.DataSource = profiles;
        ddlProfile.DataTextField = "Name";
        ddlProfile.DataValueField = "ProfileKey";

        ddlProfile.DataBind();

        List<IRuleSetProfile> profileList = new List<IRuleSetProfile>(profiles);
        ddlProfile.SelectedIndex = profileList.FindIndex(delegate(IRuleSetProfile profile) { return profile.ProfileKey == profileKey; });
        
        LoadSystems(systemKey, customerKey);
    }

    private void LoadRuleSets()
    {
        DropDownList ddlSystem = ((DropDownList)gvRuleSets.HeaderRow.FindControl(DDL_SYSTEM));
        DropDownList ddlCustomer = ((DropDownList)gvRuleSets.HeaderRow.FindControl(DDL_CUSTOMER));
        DropDownList ddlProfile = ((DropDownList)gvRuleSets.HeaderRow.FindControl(DDL_PROFILE));

        int systemKey = Convert.ToInt32(ddlSystem.SelectedValue);
        int customerKey = (ddlCustomer.SelectedIndex >= 0 ? Convert.ToInt32(ddlCustomer.SelectedValue) : 0);
        int profileKey = (ddlProfile.SelectedIndex > 0 ? Convert.ToInt32(ddlProfile.SelectedValue) : 0);
        LoadRuleSets(systemKey, customerKey, profileKey);
    }

    private void LoadRuleSets(int systemKey, int customerKey, int profileKey)
    {
        bool empty = false;
        string profileToken = string.Empty;

        if (profileKey > 0)
        {
            DataTable profileDataRow = SystemStorageHelper.LoadProfileDataTable(profileKey);

            if (profileDataRow.Rows.Count > 0)
                profileToken = profileDataRow.Rows[0]["RPR_TOKEN"] as string;

            if (profileToken == null)
                profileToken = string.Empty;
        }

        DataTable ruleSets = SystemStorageHelper.LoadRuleSetsDataTable(systemKey, customerKey, profileToken);

        if (ruleSets.Rows.Count == 0)
        {
            ZeroRowHack(ruleSets);
            empty = true;
        }

        gvRuleSets.DataSource = base.SortedDataView(ruleSets);
        gvRuleSets.DataBind();

        if (empty)
        {
            ClearGridViewCellsAndDisplayDefaultText(gvRuleSets);
        }


        LoadProfiles(systemKey, customerKey, profileKey);
    }
    public override void ClearGridViewCellsAndDisplayDefaultText(GridView gv)
    {
        foreach (TableCell cell in gv.Rows[0].Cells)
            cell.Text = string.Empty;

        gv.Rows[0].Cells[1].Text = gv.EmptyDataText;
    }
    private static void ZeroRowHack(DataTable tableOfRuleSets)
    {
        tableOfRuleSets.Columns.Add("RUS_NAME");
        tableOfRuleSets.Columns.Add("RULE_SYSTEM_KEY");
        tableOfRuleSets.Columns.Add("RSY_NAME");
        tableOfRuleSets.Columns.Add("CUSTOMER_RULE_SYSTEM_KEY");
        tableOfRuleSets.Columns.Add("CUSTOMER_RSY_NAME");
        tableOfRuleSets.Columns.Add("RULE_CUSTOMER_KEY");
        tableOfRuleSets.Columns.Add("RCU_NAME");
        tableOfRuleSets.Columns.Add("RULE_PROFILE_KEY");
        tableOfRuleSets.Columns.Add("RPR_NAME");
        tableOfRuleSets.Columns.Add("RUS_EFFECTIVE_DATE");
        tableOfRuleSets.Columns.Add("RUS_ACTIVE");
        tableOfRuleSets.Columns.Add("RULE_SET_KEY");
        tableOfRuleSets.Columns.Add("RUS_OVERRIDE_OUTPUT");
        tableOfRuleSets.Columns.Add("LOCKED_BY_USER");

        DataRow newRow = tableOfRuleSets.NewRow();
        newRow["RUS_ACTIVE"] = "1";
        newRow["RUS_EFFECTIVE_DATE"] = DateTime.Now;

        tableOfRuleSets.Rows.Add(newRow);
    }

    #endregion

    protected void btnForceUnlock_Click(object sender, EventArgs e)
    {
        RemoveLock(LockTypeEnum.RuleSet, Convert.ToInt32(hfdEntityID.Value));

        string js = "hidePopup('" + pnlLockedInformation.ClientID + "');";
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "HideLockedInformationPopup", "<script type=\"text/javascript\">/*\n<![CDATA[*/\n" + js + "\n/*]]>*/</script>");
    }

    protected void btnCopyClick(object sender, EventArgs e)
    {
        if (IsValid == false)
            return;

        SaveCopy();
    }

    private void SaveCopy()
    {
        int ruleSetKey = Convert.ToInt32(hdnRuleSetKey.Value);
        bool createNewRuleSet = cbxCreateNewRuleSet.Checked;
        string newRuleSetName = txtNewRuleSetName.Text;
        bool copyGrids = cbxCopyGrids.Checked;
        bool copyRuleSetKeys = cbxCopyRuleSetKeys.Checked;
        DateTime effectiveDate = GetEffectiveDate().Value;

        SystemStorageHelper.CopyRuleSet(ruleSetKey, createNewRuleSet, newRuleSetName, copyGrids, copyRuleSetKeys, effectiveDate);
    }

    private void CopyRuleSet(int ruleSetKey)
    {
        DataTable ruleSetData = SystemStorageHelper.LoadRuleSetDataTable(ruleSetKey);

        if (ruleSetData.Rows.Count == 0)
        {
            return;
        }

        ShowCopyRuleSetPopup();

        hdnRuleSetName.Value = (string)ruleSetData.Rows[0]["RUS_NAME"];
        hdnRuleSetKey.Value = ruleSetKey.ToString();
        txtNewRuleSetName.Text = string.Empty;

        cbxCopyGrids.Checked = false;
        cbxCopyRuleSetKeys.Checked = true;
        SetEffectiveDate(DateTime.Now);

        cvNewRuleSetName.Validate();
        cvEffectiveDate.Validate();
    }

    private void ShowCopyRuleSetPopup()
    {
        string js = "showPopup('" + this.pnlCopyRuleSet.ClientID + "','popup copyRuleSetPopup');";
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowCopyRuleSetPopup", "<script type=\"text/javascript\">/*\n<![CDATA[*/\n" + js + "\n/*]]>*/</script>");
    }

    protected void cvNewRuleSetName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = ValidateNewRuleSetName();

        if (args.IsValid == false)
            ShowCopyRuleSetPopup();
    }

    private bool ValidateNewRuleSetName()
    {
        if (cbxCreateNewRuleSet.Checked == false)
        {
            return true;
        }
        else if (txtNewRuleSetName.Text.Trim().Equals(string.Empty))
        {
            return false;
        }
        else if (txtNewRuleSetName.Text.Trim().Equals(tbwmNewRuleSetName.WatermarkText))
        {
            return false;
        }

        DataTable rulesets = SystemStorageHelper.LoadRuleSetDataTable(txtNewRuleSetName.Text.Trim(), null);
        return (rulesets.Rows.Count == 0);
    }

    private void SetEffectiveDate(DateTime? setToDateTime)
    {
        if (setToDateTime.HasValue == false)
            setToDateTime = DateTime.Now;

        txtEffectiveDate.Text = setToDateTime.Value.ToString(ConfigurationManager.AppSettings["LongDateFormat"]);

        const int minuteIncrement = 15;

        for (int hour = 0; hour < 24; hour++)
        {
            string hourString = hour.ToString().PadLeft(2, '0');
            ddlEffectiveTimeHours.Items.Add(new ListItem(hourString, hourString));
        }

        for (int minute = 0; minute < 60; minute += minuteIncrement)
        {
            string minuteString = minute.ToString().PadLeft(2, '0');
            ddlEffectiveTimeMinutes.Items.Add(new ListItem(minuteString, minuteString));
        }

        int currentHour = setToDateTime.Value.Hour;
        int currentMinute = setToDateTime.Value.Minute;

        currentMinute = (((currentMinute / 15) + 1) * 15) % 60; // Round up to nearest 15 minutes

        if (currentMinute == 0)
            currentHour = (currentHour + 1) % 24;

        ddlEffectiveTimeHours.SelectedIndex = currentHour;
        int index = (currentMinute / 15);

        ddlEffectiveTimeMinutes.SelectedIndex = index;
    }

    private DateTime? GetEffectiveDate()
    {
        return BasePage.GetEffectiveDate(txtEffectiveDate, ddlEffectiveTimeHours, ddlEffectiveTimeMinutes);
    }

    protected void cvEffectiveDate_ServerValidate(object source, ServerValidateEventArgs args)
    {

        args.IsValid = true;

        DateTime? effectiveDate = GetEffectiveDate();

        if (effectiveDate.HasValue == false)
        {
            args.IsValid = false;
        }
        else if (effectiveDate.Value < DateTime.Now)
        {
            args.IsValid = false;
        }
      
        if (args.IsValid == false)
        {
            ShowCopyRuleSetPopup();
        }
            
    }

    protected void cvValidate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string name = cbxCreateNewRuleSet.Checked ? 
            txtNewRuleSetName.Text.Trim() : 
            hdnRuleSetName.Value;

        DateTime? effectiveDate = GetEffectiveDate();

        DataTable ruleSets = SystemStorageHelper.LoadRuleSetDataTable(name, effectiveDate);

        args.IsValid = (ruleSets.Rows.Count == 0);

        if (args.IsValid == false)
        {
            ShowCopyRuleSetPopup();
        }
    }
}
