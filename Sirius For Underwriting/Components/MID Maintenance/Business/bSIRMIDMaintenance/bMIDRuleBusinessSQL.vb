Option Strict On
Option Explicit On

Module BusinessSQL

    Public Const kGetALLPMWrkTaskGroupTasksName As String = "Returns all effective PMWrkTasks for all effective PMWrkTaskGroups"
    Public Const kGetALLPMWrkTaskGroupTasksSQL As String = "spu_ACT_PMwrk_Task_Group_Tasks_Select"

    Public Const kGetALLPMWrkTaskGroupPMUserGroupsName As String = "Returns all effective PMUSerGroups for all effective PMWrkTaskGroups"
    Public Const kGetALLPMWrkTaskGroupPMUserGroupsSQL As String = "spu_ACT_PMwrk_Task_Group_PMUserGroup_Select"

    Public Const kGetMIDRuleDetailsName As String = "Returns list of rules associated with a perticular branch"
    Public Const kGetMIDRuleDetailsSQL As String = "spu_MID_Rule_Details_Get"

    Public Const kAddMIDRuleName As String = "Adds a new rule for a branch"
    Public Const kAddMIDRuleSQL As String = "spu_MID_Rule_Details_Add"

    Public Const kUpdateMIDRuleName As String = "updates an existing rule"
    Public Const kUpdateMIDRuleSQL As String = "spu_MID_Rule_Details_Update"

    Public Const kDeleteMIDRuleName As String = "deletes a rule"
    Public Const kDeleteMIDRuleSQL As String = "spu_MID_Rule_Details_Delete"

    Public Const kUnDeleteMIDRuleName As String = "Undeletes a rule"
    Public Const kUnDeleteMIDRuleSQL As String = "spu_MID_Rule_Details_UnDelete"

End Module
