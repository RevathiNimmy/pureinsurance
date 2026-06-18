Imports System
Imports System.Web.Profile
Imports System.Web.Security


Public Class ProfileGroupRegistrationDetails
    Inherits System.Web.Profile.ProfileGroupBase
    Private _firstName As String = "FirstName"
    Private _surName As String = "SurName"
    Private _title As String = "title"
    Private _partyKey As String = "partyKey"
    Private _email As String = "email"

    <SettingsAllowAnonymous(True)> _
    Public Overridable Property FirstName() As String
        Get
            Return TryCast(MyBase.GetPropertyValue(_firstName), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue(_firstName, value)
        End Set
    End Property
    <SettingsAllowAnonymous(True)> _
    Public Overridable Property Title() As String
        Get
            Return TryCast(MyBase.GetPropertyValue(_title), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue(_title, value)
        End Set
    End Property
    <SettingsAllowAnonymous(True)> _
    Public Overridable Property Surname() As String
        Get
            Return TryCast(MyBase.GetPropertyValue(_surName), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue(_surName, value)
        End Set
    End Property
    <SettingsAllowAnonymous(True)> _
    Public Overridable Property PartyKey() As String
        Get
            Return TryCast(MyBase.GetPropertyValue(_partyKey), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue(_partyKey, value)
        End Set
    End Property
    <SettingsAllowAnonymous(True)> _
    Public Overridable Property Email() As String
        Get
            Return TryCast(MyBase.GetPropertyValue(_email), String)
        End Get
        Set(ByVal value As String)
            MyBase.SetPropertyValue(_email, value)
        End Set
    End Property
    
End Class

Public Class CustomProfile
    Inherits System.Web.Profile.ProfileBase
    ' Private _RegistrationDetails As String = "RegistrationDetails"
    Private _RegistrationDetails As ProfileGroupRegistrationDetails
    Public Shared Function GetUserProfile(ByVal username As String) As CustomProfile
        Return TryCast(Create(username), CustomProfile)
    End Function

    Public Shared Function GetUserProfile() As CustomProfile
        Return TryCast(Create(Membership.GetUser().UserName), CustomProfile)
    End Function
    <SettingsAllowAnonymous(True)> _
    Public Overridable ReadOnly Property RegistrationDetails() As ProfileGroupRegistrationDetails
        Get
            If _RegistrationDetails Is Nothing Then
                _RegistrationDetails = New ProfileGroupRegistrationDetails
                _RegistrationDetails.Init(Me, "RegistrationDetails")
            End If
            Return _RegistrationDetails
        End Get
    End Property

End Class






