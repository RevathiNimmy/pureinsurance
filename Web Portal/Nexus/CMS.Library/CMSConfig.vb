Imports System.Web.Configuration.WebConfigurationManager

Public Class CMSConfig

    Private Shared Function GetBooleanSetting(ByVal SettingName As String) As Boolean
        Try
            Return (AppSettings(SettingName).ToLower = "true")
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function AllowFrontEndRestrictions() As Boolean
        Return GetBooleanSetting("AllowFrontendRestrictions")
    End Function

    Public Shared Function AllowMenuItemsToBeHidden() As Boolean
        Return GetBooleanSetting("AllowMenuItemsToBeHidden")
    End Function

    Public Shared Function AllowLiveAndExpiryDates() As Boolean
        Return GetBooleanSetting("AllowLiveAndExpiryDates")
    End Function

    Public Shared Function AllowNodesToBeMoved() As Boolean
        Return GetBooleanSetting("AllowNodesToBeMoved")
    End Function

    Public Shared Function AllowNodesOrderToBeChanged() As Boolean
        Return GetBooleanSetting("AllowNodesOrderToBeChanged")
    End Function

    Public Shared Function InheritMetaTagsFromParent() As Boolean
        Return GetBooleanSetting("InheritMetaTagsFromParent")
    End Function

    Public Shared Function AutomaticApproval() As Boolean
        Return GetBooleanSetting("AutomaticApproval")
    End Function

    Public Shared Function AllowFastLinks() As Boolean
        Return GetBooleanSetting("AllowFastLinks")
    End Function

    Public Shared Function AllowContentPages() As Boolean
        Return GetBooleanSetting("AllowContentPages")
    End Function

    Public Shared Function AllowContentCustomProperties() As Boolean
        Return GetBooleanSetting("AllowContentCustomProperties")
    End Function

End Class