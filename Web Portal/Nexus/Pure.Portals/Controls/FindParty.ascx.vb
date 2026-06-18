Imports SiriusFS.SAM.Client
Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.XmlReader
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.HttpContext
Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library
Imports System.Globalization.CultureInfo
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Controls_FindParty : Inherits System.Web.UI.UserControl
        Private sLinkedTextBox As String
        Private sModalURL As String
        Private sTextToShow As String
        Private sType As String
        Private iPartyKey As Integer
        Private sPartyCode As String
        Private isEnabledTextSearch As Boolean
        Private isEnabledWriteOnly As Boolean
        Private isDisableControl As Boolean
        Private sID As String
        Protected oMaster As ContentPlaceHolder
        Protected oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

#Region "Public Property"
        Public Property PartyKey() As Integer
            Get
                iPartyKey = CInt(hPartyKey.Value)
                Return iPartyKey
            End Get
            Set(ByVal value As Integer)
                iPartyKey = value
                hPartyKey.Value = value
            End Set
        End Property

        Public Property PartyCode() As String
            Get
                sPartyCode = txtPartyName.Text
                Return sPartyCode
            End Get
            Set(ByVal value As String)
                sPartyCode = value
                txtPartyName.Text = value
            End Set
        End Property

        Public Property ModalURL() As String
            Get
                Return sModalURL
            End Get
            Set(ByVal value As String)
                sModalURL = value
            End Set
        End Property
        Public Property Type() As String
            Get
                Return sType
            End Get
            Set(ByVal value As String)
                sType = value
            End Set
        End Property
        Public Property TextToShow() As String
            Get
                Return sTextToShow
            End Get
            Set(ByVal value As String)
                sTextToShow = value
            End Set
        End Property
        ''' <summary>
        ''' Property to enable the text box but search will not be enabled
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EnabledWriteOnly() As Boolean
            Get
                Return isEnabledWriteOnly
            End Get
            Set(ByVal value As Boolean)
                isEnabledWriteOnly = value
            End Set
        End Property
        ''' <summary>
        ''' Property to enable the text box
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EnabledTextSearch() As Boolean
            Get
                Return isEnabledTextSearch
            End Get
            Set(ByVal value As Boolean)
                isEnabledTextSearch = value
            End Set
        End Property

        ''' <summary>
        ''' Property to disable the control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DisableControl() As Boolean
            Get
                Return isDisableControl
            End Get
            Set(ByVal value As Boolean)
                isDisableControl = value
            End Set
        End Property
        ''' <summary>
        ''' Property to pass the client id of the registered control 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PassedClientID() As String
            Get
                Return sID
            End Get
            Set(ByVal value As String)
                sID = value
            End Set
        End Property
#End Region
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim SetOtherPartyScript As String = "function set" & Me.ClientID & "OtherParty(sName,sKey,sAgentCode,sType){" &
              " document.getElementById('" & txtPartyName.ClientID & "').value=sName; " &
              " document.getElementById('" & hPartyKey.ClientID & "').value=sKey; " &
              " document.getElementById('" & hPartyType.ClientID & "').value=sType; " &
              "self.window.tb_remove();" &
                                         " }   "

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "SetOtherPartyScript_" & Me.ClientID, SetOtherPartyScript, True)

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'if textbox search is not enabled set textbox as readonly
            If Not EnabledTextSearch And Not EnabledWriteOnly Then
                txtPartyName.Attributes.Add("readonly", "readonly")
                txtPartyName.Attributes.Remove("AutoPostBack")
            End If

            'disable the button and set textbox to read only if control is disabled.
            If DisableControl Then
                txtPartyName.ReadOnly = True
                btnFindParty.Enabled = False
            Else
                txtPartyName.ReadOnly = False
                btnFindParty.Enabled = True
            End If

            'Dim sUrl As String
            If Not IsPostBack Then
                If PartyKey > 0 Then
                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oParty As NexusProvider.BaseParty
                    oParty = oWebservice.GetParty(PartyKey)
                    If oParty IsNot Nothing Then
                        Select Case True
                            Case TypeOf oParty Is NexusProvider.OtherParty
                                With CType(oParty, NexusProvider.OtherParty)
                                    txtPartyName.Text = .Name.Trim
                                    If EnabledTextSearch Then
                                        txtPartyName.Text = .Name.Trim
                                    End If
                                End With
                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                With CType(oParty, NexusProvider.PersonalParty)
                                    txtPartyName.Text = .Title & " " & .Forename & " " & .Lastname
                                End With
                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                With CType(oParty, NexusProvider.CorporateParty)
                                    txtPartyName.Text = .CompanyName
                                End With

                        End Select
                    End If
                End If

                Dim sUrl As String
                If HttpContext.Current.Session.IsCookieless Then
                    sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/" & sModalURL & "?Type=" + sType + "&ClientID=" + Me.ClientID + "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=800"
                Else
                    sUrl = AppSettings("WebRoot") & sModalURL & "?Type=" + sType + "&ClientID=" + Me.ClientID + "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=800"
                End If
                If Not String.IsNullOrEmpty(TextToShow) Then
                    lblFindParty.Text = TextToShow
                End If
                btnFindParty.OnClientClick = "tb_show(null , '" & sUrl & "' , null);return false;"

                oNexusConfig = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
            Else

                If txtPartyName.Text = "" And EnabledTextSearch Then
                    FindOtherParty()

                ElseIf Not Page.IsPostBack AndAlso txtPartyName.Text.ToString.Trim = "" AndAlso EnabledWriteOnly Then
                    ReSetValues()
                End If
            End If
            If Not String.IsNullOrEmpty(TextToShow) Then
                lblFindParty.Text = TextToShow
            End If
            If Not String.IsNullOrEmpty(TextToShow) Then
                lblFindParty.Text = TextToShow
            End If
            'Find Button should be Display as a Disable on View Mode
            Select Case CType(Session(CNMode), Mode)
                Case Mode.ViewClaim, Mode.View
                    btnFindParty.Enabled = False
            End Select

        End Sub
        Public Sub ReSetValues()
            txtPartyName.Text = String.Empty
            PartyKey = 0
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Dim sUrl As String
            If HttpContext.Current.Session.IsCookieless Then
                sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/" & sModalURL & "?Type=" + sType + "&ClientID=" + Me.ClientID + "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=800"
            Else
                sUrl = AppSettings("WebRoot") & sModalURL & "?Type=" + sType + "&ClientID=" + Me.ClientID + "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=800"
            End If

            btnFindParty.OnClientClick = "tb_show(null , '" & sUrl & "' , null);return false;"
        End Sub
        ''' <summary>
        ''' If text box is not read only then check if code added in textbox is correct.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub txtPartyName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPartyName.TextChanged
            If IsPostBack And EnabledTextSearch Then
                If (hPartyCode.Value = "0") Then
                    hPartyCode.Value = txtPartyName.Text
                End If
                FindOtherParty()
            End If
        End Sub

        ''' <summary>
        ''' To check if the value added in text box is correct.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub FindOtherParty()

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPartySearchCriteria As NexusProvider.PartySearchCriteria
            Dim oPartyCollection As NexusProvider.PartyCollection
            If txtPartyName.Text <> "" Then
                oPartySearchCriteria = New NexusProvider.PartySearchCriteria()
                oPartySearchCriteria.PartyType = NexusProvider.PartyTypeType.OTOTHERPARTY
                oPartySearchCriteria.OtherPartyTypeCode = OtherPartySearch(Type)
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.OTOTHERPARTY)
                oPartySearchCriteria.ShortName = hPartyCode.Value.Trim
                oPartyCollection = oWebService.FindParty(oPartySearchCriteria)
            End If

            Dim str As String
            str = PassedClientID

            If txtPartyName.Text = "" Then 'in case existing party is deleted

                hPartyKey.Value = 0

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "NoResult", _
             "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){set" & str & "OtherParty('',0,'','');});</script>")

            ElseIf oPartyCollection.Count = 1 Then 'in case correct code is entered, proceed without opening the model page

                hPartyKey.Value = oPartyCollection.Item(0).Key
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "PromptAgentwarning", _
            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){set" & str & "OtherParty('" & oPartyCollection.Item(0).Name & "'," & oPartyCollection.Item(0).Key & ",'" & oPartyCollection.Item(0).ShortName & "','" & oPartyCollection.Item(0).Type & "');});</script>")

            ElseIf oPartyCollection.Count = 0 Then 'in case junk value is entered

                hPartyKey.Value = 0

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "NoResult", _
             "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){set" & str & "OtherParty('" & txtPartyName.Text.Trim & "',0,'','');});</script>")

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "PromptAgentwarning", _
      "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('Record not found. Please refine your search.');return false;});</script>") 'Can’t use “TPA” hardcoded as this control is used by other party type also


            ElseIf oPartyCollection.Count > 1 Then 'in case % is entered

                hPartyKey.Value = 0

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "NoResult", _
                            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){set" & str & "OtherParty('',0,'','');});</script>")

                Dim sUrl As String
                If HttpContext.Current.Session.IsCookieless Then
                    sUrl = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/" & sModalURL & "?Type=" + sType + "&ClientID=" + Me.ClientID + "&PartyCode=" + txtPartyName.Text.Trim + "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=800"
                Else
                    sUrl = AppSettings("WebRoot") & "/" & sModalURL & "?Type=" + sType + "&ClientID=" + Me.ClientID + "&PartyCode=" + txtPartyName.Text.Trim + "&modal=true&KeepThis=true&TB_iframe=true&height=550&width=800"
                End If

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
                "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")

            End If

        End Sub


    End Class
End Namespace
