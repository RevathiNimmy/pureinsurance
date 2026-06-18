using System;
using System.Collections.ObjectModel;
using System.Web;
using RulesEngine.EngineSupport.DataAccess;
using RulesEngine.Website;
using RulesEngine.EngineCommon;
using System.Data;
using RulesEngine.BaseSystem.Factories;
using RulesEngine.Website.Code;

public partial class RuleSetEdit : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "RuleLineFunctions.js", "<script type=\"text/javascript\" src=\"" + VirtualPathUtility.ToAbsolute("~/JavaScript/RuleLineFunctions.js") + "\"></script>");
    }

    #region Private Members

    private void SaveRuleSet()
    {
        if (this.Page.IsValid)
        {
            IRuleSet ruleSet = RuleSetAddEditControl1.GetRuleSet();
            
            bool success = base.SystemStorageHelper.Save(ruleSet, RuleSetAddEditControl1.PreviousStatus);
            if (success)
            {
                try
                {
                    if (ruleSet.Status == RuleEngineStatus.NotActive)
                    {
                        Collection<ServerClassification> classes = new Collection<ServerClassification>();
                        this.SystemStorageHelper.Load(classes, ruleSet.Profile.System.SystemKey, ruleSet.CustomerKey);

                        foreach (var classe in classes)
                        {
                            bool published = PublishHelper.Publish(this, ruleSet, classe.ClassificationKey);
                        }
                    }    
                }
                finally
                {
                    GoBack();
                }
                
            }

        }
    }

    private void LostLock()
    {
        string js = "showPopup('" + this.pnlLockHasBeenLost.ClientID + "','popup lockedInformation');";
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "LockHasBeenLostPopup", "<script type=\"text/javascript\">/*\n<![CDATA[*/\n" + js + "\n/*]]>*/</script>");
    }

    private void NotInUse()
    {
        string js = "showPopup('" + this.pnlNotIsUse.ClientID + "','popup lockedInformation');";
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "NotInUse", "<script type=\"text/javascript\">/*\n<![CDATA[*/\n" + js + "\n/*]]>*/</script>");
    }

    private void GoBack()
    {
        ReleaseLock(LockTypeEnum.RuleSet, this.RuleSetAddEditControl1.GetRuleSet().RuleSetKey);

        base.NavigateTo.RuleSetListPage();
    }

    #endregion

    #region Button Events

    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool stillHaveLock = StillHaveLock(LockTypeEnum.RuleSet, this.RuleSetAddEditControl1.GetRuleSet().RuleSetKey);
        if (stillHaveLock == false)
        {
            LostLock();
            return;
        }

        IRuleSet ruleSet = RuleSetAddEditControl1.GetRuleSet();
        if (ruleSet.Status == RuleEngineStatus.NotActive)
        {
            NotInUse();
            return;
        }

        SaveRuleSet();    
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        GoBack();   
    }

    #endregion

    protected void btnGoBack_Click(object sender, EventArgs e)
    {
        GoBack(); 
    }

    protected void btnContinueWithNotInUse_Click(object sender, EventArgs e)
    {
        SaveRuleSet();   
    }

}
