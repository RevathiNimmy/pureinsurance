using RulesEngine.Website;
using System;
using RulesEngine.EngineCommon;

public partial class RuleSetAdd : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Private Members

    private void SaveRuleSet()
    {
        if (this.Page.IsValid)
        {
            // Begin - ICM - 23566
            RuleSetAddEditControl1.SaveRuleSet();
            // End - ICM - 23566
        }
    }

    #endregion

    #region Button Events

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveRuleSet();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        base.NavigateTo.RuleSetListPage();
    }

    #endregion
}
