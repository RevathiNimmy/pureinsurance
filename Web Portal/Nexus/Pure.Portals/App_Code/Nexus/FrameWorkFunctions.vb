Imports System.Web.HttpContext

Imports System.Xml.XPath
Imports Nexus.Library
Imports Nexus.Library.Config
Imports CMS.Library
Imports NexusProvider.SAMForInsurance
Imports System.Xml.XmlReader
Imports System.Xml
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Text

Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Data
Imports Nexus
Imports SiriusFS.SAM.Client
Imports NexusProvider.Quote
Imports System.Linq
Imports System.Xml.Linq
Imports System.IO
Imports System.Resources

Namespace Nexus

    Public Module FrameWorkFunctions
        ''' <summary>
        ''' Adds a guid to session and also returns this guid as a string
        ''' Used to secure calls to services from AJAX
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function setSecureGuid() As String
            Dim sGuid As String = New Guid().ToString
            HttpContext.Current.Session(CNSecureGuid) = sGuid
            Return sGuid
        End Function
        Sub EnableControls(ByVal oContainer As Control)

            'Disable all controls in the container passed in,
            'also disable all the controls in any child containers

            Dim oControl As Object
            For Each oControl In oContainer.Controls
                Select Case oControl.GetType.Name
                    Case "GridView",
                        "RadioButtonList", "DropDownList",
                        "CheckBox", "CheckBoxList",
                        "LookupList", "HtmlInputText",
                        "HtmlInputHidden", "LookupListV2"
                        If oControl.id <> "chkExGratia" Then
                            oControl.Enabled = True
                        End If
                    Case "TextBox"
                        oControl.Enabled = True
                        If CType(oControl, TextBox).Attributes("readonly") IsNot Nothing Then
                            CType(oControl, TextBox).Attributes.Remove("readonly")
                        End If
                    Case "HyperLink"
                        CType(oControl, HyperLink).Visible = True
                    Case "CompareValidator", "CustomValidator", "RangeValidator", "RegularExpressionValidator", "RequiredFieldValidator"
                        If (oControl.GetType.Name = "RequiredFieldValidator") Then
                            If Not (UCase(DirectCast(oControl, RequiredFieldValidator).ID) = UCase("VLDMAINCONTACT")) Then
                                oControl.Enabled = True
                            End If
                        Else
                            oControl.Enabled = True
                        End If
                    Case "HtmlTableRow", "TableRow", "HtmlTableCell", "TableCell",
                        "Controls_CorporateClient", "Controls_PersonalClient"

                        EnableControls(oControl)

                    Case "Panel"
                        EnableControls(CType(oControl, Panel))

                    Case "UpdatePanel"
                        For Each oCtrl As Object In oControl.Controls
                            EnableControls(oCtrl)
                        Next

                    Case "controls_addresscntrl_ascx"
                        oControl.Enabled = True
                    Case "RiskContainer"
                        EnableControls(CType(oControl, RiskContainer))
                    Case "Button"
                        oControl.Enabled = True
                    Case "controls_calendarlookup_ascx"
                        oControl.Enabled = True

                        Dim opage = TryCast(Current.CurrentHandler, System.Web.UI.Page)
                        Dim m_sLinkedControlClientId As String = oControl.linkedcontrol.ToString

                        opage.ClientScript.RegisterStartupScript(opage.GetType, oControl.id,
                        "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){$('#ctl00_cntMainBody_" & m_sLinkedControlClientId & "').removeAttr('readonly');});</script>")

                    Case "Controls_StandardWordings_ascx"
                        oControl.Enabled = True

                    Case "controls_pfplandetails_ascx", "controls_installmentdetails_ascx", "controls_bankaccountdetails_ascx"
                        EnableControls(oControl)
                    Case "PlaceHolder"
                        EnableControls(CType(oControl, PlaceHolder))


                    Case Else
                        Select Case True
                            Case oControl.GetType.Name.Contains("FindControl")
                                oControl.Enabled = True
                        End Select
                End Select
            Next

        End Sub

    ''' <summary>
        ''' Disable all the form controls in the control container passed
        ''' </summary>
        ''' <param name="oContainer">Control container in which all the controls will be disabled</param>
        ''' <remarks></remarks>
        Sub DisableControls(ByVal oContainer As Control, Optional ByVal bDisableButton As Boolean = False)

            'Disable all controls in the container passed in,
            'also disable all the controls in any child containers

            Dim oControl As Object
            For Each oControl In oContainer.Controls
                Select Case oControl.GetType.Name

                    Case "TextBox", "HtmlInputText",
                    "HtmlInputHidden"

                        oControl.Attributes.Add("readonly", "readonly")

                    Case "GridView", "RadioButtonList", "DropDownList",
                                            "CheckBox", "CheckBoxList",
                                            "LookupList", "LookupListV2"

                        oControl.Enabled = False

                    Case "HyperLink"

                        CType(oControl, HyperLink).Enabled = False

                    Case "CompareValidator", "CustomValidator", "RangeValidator", "RegularExpressionValidator", "RequiredFieldValidator"
                        If (oControl.GetType.Name = "RequiredFieldValidator") Then
                            If Not (UCase(DirectCast(oControl, RequiredFieldValidator).ID) = UCase("VLDMAINCONTACT")) Then
                                oControl.Enabled = False
                            End If
                        Else
                            oControl.Enabled = False
                        End If

                    Case "HtmlTableRow", "TableRow", "HtmlTableCell", "TableCell",
                        "Controls_CorporateClient", "Controls_PersonalClient"

                        DisableControls(oControl)

                    Case "Panel"
                        DisableControls(CType(oControl, Panel))

                    Case "PlaceHolder"
                        DisableControls(CType(oControl, PlaceHolder))


                    Case "UpdatePanel"
                        For Each oCtrl As Object In oControl.Controls
                            DisableControls(oCtrl)
                        Next

                        Dim oUpdPanel As UpdatePanel = CType(oControl, UpdatePanel)
                        If oUpdPanel.HasControls Then
                            For Each oCtrl As Object In oUpdPanel.Controls(0).Controls
                                DisableControls(oCtrl)
                            Next
                        End If

                    Case "HtmlGenericControl"
                        Dim oGenericCtrl As HtmlGenericControl = CType(oControl, HtmlGenericControl)
                        If oGenericCtrl.HasControls Then
                            For Each oCtrl As Object In oGenericCtrl.Controls
                                DisableControls(oCtrl)
                            Next
                        End If

                    Case "Button"
                        If bDisableButton Or oControl.id = "btnAgentCode" Then
                            oControl.Enabled = False
                        End If

                    Case "RiskContainer"
                        DisableControls(oControl, bDisableButton)

                    Case "DropDownList"
                        CType(oContainer, DropDownList).Enabled = False

                    Case Else
                        Select Case True
                            Case oControl.GetType.Name.Contains("controls_addresscntrl_ascx")
                                oControl.Enabled = False

                            Case oControl.GetType.Name.Contains("controls_calendarlookup_ascx")
                                oControl.Enabled = False

                            Case oControl.GetType.Name.Contains("controls_reserveandrecovery_ascx")
                                oControl.Attributes.Add("readonly", "readonly")

                            Case oControl.GetType.Name.Contains("controls_payclaim_ascx")
                                DisableControls(oControl)

                            Case oControl.GetType.Name.Contains("controls_findparty_ascx")
                                oControl.Attributes.Add("readonly", "readonly")

                            Case oControl.GetType.Name.Contains("Controls_CorporateClient"), oControl.GetType.Name.Contains("Controls_PersonalClient")
                                DisableControls(oControl)

                            Case oControl.GetType.Name.Contains("controls_pfplandetails_ascx"), oControl.GetType.Name.Contains("controls_bankaccountdetails_ascx"), oControl.GetType.Name.Contains("controls_installmentdetails_ascx")
                                DisableControls(oControl)
                            Case oControl.GetType.Name.Contains("FindControl")
                                oControl.Enabled = False
                        End Select

                End Select
            Next

            If oContainer.Controls.Count = 0 Then
                Select Case oContainer.GetType.Name.ToUpper
                    Case "TEXTBOX"
                        CType(oContainer, TextBox).Enabled = False
                    Case "RADIOBUTTONLIST"
                        CType(oContainer, RadioButtonList).Enabled = False
                    Case "HYPERLINK"
                        CType(oContainer, HyperLink).Visible = False
                    Case "BUTTON"
                        CType(oContainer, Button).Enabled = False
                    Case "DROPDOWNLIST"
                        CType(oContainer, DropDownList).Enabled = False
                    Case "CHECKBOX"
                        CType(oContainer, CheckBox).Enabled = False
                End Select
            End If

        End Sub

        ''' <summary>
        ''' Clear the values of all the controls in the passed control container
        ''' </summary>
        ''' <param name="oContainer">Control container in which all the controls will be reset</param>
        ''' <remarks></remarks>
        Sub ResetControls(ByVal oContainer As Control)
            Dim oControl As Object
            For Each oControl In oContainer.Controls
                Select Case LCase(oControl.GetType.Name)
                    Case "textbox"
                        oControl.Text = ""
                    Case "radiobuttonlist", "dropdownlist", "checkboxlist"
                        oControl.SelectedIndex = 0
                    Case "checkbox"
                        oControl.Checked = False
                    Case "htmlinputtext", "lookuplist", "lookuplistv2"
                        oControl.Value = ""
                    Case "updatepanel", "control", "panel"
                        ResetControls(oControl)
                    Case "gridview"
                        oControl.DataBind()
                    Case "controls_findparty_ascx"
                        oControl.ReSetValues()
                    Case "controls_addresscntrl_ascx"
                        oControl.Address = Nothing
                    Case "hiddenfield"
                        oControl.Value = ""

                End Select
            Next
        End Sub

        ''' <summary>
        ''' Method to determine if the path passed into the function matches the current page path
        ''' </summary>
        ''' <param name="v_sPagePath">Path to be checked</param>
        ''' <returns>Returns a true or false depending if it matches</returns>
        ''' <remarks></remarks>
        Function IsCurrentPage(ByVal v_sPagePath As String) As Boolean

            Dim bMatch As Boolean = False

            With Current.Request.Url
                If .Segments.Length > 0 Then

                    Dim sCurrentPage As String = .Segments(.Segments.Length - 1)
                    Dim sPagePath() As String = v_sPagePath.Split("/")

                    If sPagePath.Length > 0 Then
                        If LCase(sCurrentPage) = LCase(sPagePath(sPagePath.Length - 1)) Then
                            'A Match, but it doesnt take into account querystrings
                            'at the moment, this may need changing later
                            bMatch = True
                        Else
                            'They dont match !
                            bMatch = False
                        End If
                    Else
                        'No page passed in, so we cant match to the current page, this link is obviously invalid
                        bMatch = False
                    End If
                Else
                    'Can't really match nothing, and something must be wrong if the current page is non existant
                    bMatch = False
                End If
            End With

            Return bMatch

        End Function

        ''' <summary>
        ''' Set the current page on the nexus framework progress bar
        ''' </summary>
        ''' <param name="i_CurrentPageNumber">The current page number</param>
        ''' <remarks></remarks>
        Sub SetPageProgress(ByVal i_CurrentPageNumber As Integer)

            Current.Session(CNCurrentPageNumber) = i_CurrentPageNumber

            Dim i_HighestPageNumber As Integer = Current.Session(CNHighestPageNumber)

            If i_CurrentPageNumber > i_HighestPageNumber Then
                Current.Session(CNHighestPageNumber) = i_CurrentPageNumber
            End If

        End Sub

        ''' <summary>
        ''' Set the current page on the risk progress bar
        ''' </summary>
        ''' <param name="v_sParentTabID">The risk progress bar can consist of multiple progress bar's,
        ''' one for each level of the risk, so the progress bar that the current page is for needs
        ''' to be defined</param>
        ''' <param name="v_iCurrentTabIndex">The Current tab index</param>
        ''' <remarks></remarks>
        Sub SetRiskProgress(ByVal v_sParentTabID As String, ByVal v_iCurrentTabIndex As Integer)

            Dim htRiskProgress As Hashtable

            'Retreive risk progress from session, or create if it does not exist
            If Current.Session.Item(CNRiskProgress) Is Nothing Then
                htRiskProgress = New Hashtable
            Else
                htRiskProgress = CType(Current.Session.Item(CNRiskProgress), Hashtable)
            End If

            If htRiskProgress.ContainsKey(v_sParentTabID) Then
                'if current page is greater than current progress, then update progress to current page
                If htRiskProgress(v_sParentTabID) < v_iCurrentTabIndex Then
                    htRiskProgress(v_sParentTabID) = v_iCurrentTabIndex
                End If
            Else
                'No key, so create with functin parameters
                htRiskProgress.Add(v_sParentTabID, v_iCurrentTabIndex)
            End If

            Current.Session.Item(CNRiskProgress) = htRiskProgress

        End Sub

        ''' <summary>
        ''' Retrieve the path to the first risk page within the product config specified
        ''' </summary>
        ''' <param name="v_sScreenConfigFile">Path to the product config</param>
        ''' <returns>Returns the path to the first risks page</returns>
        ''' <remarks></remarks>
        Function GetFirstRiskScreen(ByVal v_sScreenConfigFile As String, Optional ByRef bMainDetail As String = Nothing) As String

            Dim sFirstRiskScreen As String = String.Empty
            Dim sMainDetail As String = String.Empty
            Dim bvisible As String = String.Empty
            Dim iTabindex As Integer = 1
            Dim Navigator As XPathNavigator
            Dim Doc As XPathDocument = New XPathDocument(Current.Server.MapPath(v_sScreenConfigFile))
            Navigator = Doc.CreateNavigator()
            Dim i, j As XPathNodeIterator
            Dim bStatus As Boolean = False
            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            i = Navigator.Select("/screens/screen/tab[1]")
            While i.MoveNext()
                sMainDetail = i.Current.GetAttribute("maindetails", String.Empty)
                bvisible = i.Current.GetAttribute("visible", String.Empty)
            End While

            bMainDetail = sMainDetail

            If oQuote IsNot Nothing And Current.Session(CNQuoteMode) <> QuoteMode.ReQuote Then
                If Current.Session(CNQuoteMode) = QuoteMode.QuickQuote Then
                    'If Current.Session(CNQuoteMode) = QuoteMode.QuickQuote Or Current.Session(CNQuoteMode) = QuoteMode.FullQuote Then
                    'If Current.Session(CNQuoteMode) = QuoteMode.QuickQuote Then
                    If String.IsNullOrEmpty(sMainDetail) = False AndAlso sMainDetail.ToLower = "false" Then
                        i = Navigator.Select("/screens/screen/tab[2]")
                    Else
                        i = Navigator.Select("/screens/screen/tab[1]")
                    End If
                Else
                    If String.IsNullOrEmpty(sMainDetail) = True Then
                        bMainDetail = "false"
                        i = Navigator.Select("/screens/screen/tab[1]")
                    Else
                        bMainDetail = "false"
                        i = Navigator.Select("/screens/screen/tab[2]")
                    End If

                End If
            ElseIf oQuote Is Nothing Or Current.Session(CNQuoteMode) = QuoteMode.ReQuote Then
                If String.IsNullOrEmpty(sMainDetail) = False AndAlso sMainDetail.ToLower = "false" Then
                    i = Navigator.Select("/screens/screen/tab[2]")
                Else
                    'Check ClaimScreens Configuration 
                    While LCase(bvisible) = "false"
                        iTabindex = iTabindex + 1
                        i = Navigator.Select("/screens/screen/tab[" & iTabindex & "]")
                        While i.MoveNext()
                            bvisible = i.Current.GetAttribute("visible", String.Empty)
                        End While
                    End While
                    i = Navigator.Select("/screens/screen/tab[" & iTabindex & "]")
                End If
            End If

            While i.MoveNext()
                sFirstRiskScreen = i.Current.GetAttribute("url", String.Empty)

            End While
            Return sFirstRiskScreen
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="v_sScreenConfigFile"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetScreenCode(ByVal v_sScreenConfigFile As String) As String
            ''need this function on Primium Display Page in Case of MTA Cancellation
            Dim sScreenCode As String = String.Empty
            Dim Navigator As XPathNavigator
            Dim Doc As XPathDocument = New XPathDocument(Current.Server.MapPath(v_sScreenConfigFile))
            Navigator = Doc.CreateNavigator()
            Dim i As XPathNodeIterator

            i = Navigator.Select("/screens/screen")
            i.MoveNext()
            sScreenCode = i.Current.GetAttribute("screen_code", String.Empty)
            Return sScreenCode
        End Function

        ''' <summary>
        ''' Returns the cover start date of the quote/policy according to the Nexus config
        ''' </summary>
        ''' <returns>Cover start date</returns>
        ''' <remarks></remarks>
        Public Function GetCoverStartDate() As Date

            Dim dCoverStartDate As Date
            Dim sProductPath() As String
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            sProductPath = CStr(Current.Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder) _
                .Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)
            Dim oProductConfig As Config.Product = CType(System.Configuration.ConfigurationManager.GetSection("NexusFrameWork"),
                        Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.GetProductByName(Current.Server.UrlDecode(
                        Current.Request.Url.Segments(sProductPath.Length + 1).TrimEnd("/")))

            Select Case oProductConfig.CoverDate.StartDate
                Case StartDate.Today
                    dCoverStartDate = Now
                Case StartDate.Tommorrow
                    dCoverStartDate = Now.AddDays(1)
                Case Else
                    'Default to today, as this should get overridden by the risk
                    'screens as its not been defined in the product config
                    dCoverStartDate = Now
            End Select

            Return dCoverStartDate

        End Function

        ''' <summary>
        ''' Returns the cover end date of the quote/policy according to the Nexus config
        ''' </summary>
        ''' <param name="dCoverStartDate">The cover state date</param>
        ''' <returns>Cover end date</returns>
        ''' <remarks></remarks>
        Public Function GetCoverEndDate(ByVal dCoverStartDate As Date) As Date

            Dim dCoverEndDate As Date
            Dim sProductPath() As String
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            sProductPath = CStr(Current.Request.ApplicationPath & "/" & oNexusConfig.ProductsFolder) _
                .Split(Regex.Split("/", ""), StringSplitOptions.RemoveEmptyEntries)
            Dim oProductConfig As Config.Product = CType(System.Configuration.ConfigurationManager.GetSection("NexusFrameWork"),
                        Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.GetProductByName(Current.Server.UrlDecode(
                        Current.Request.Url.Segments(sProductPath.Length + 1).TrimEnd("/")))


            Select Case oProductConfig.CoverDate.TimeScale
                Case TimeScale.Day
                    dCoverEndDate = dCoverStartDate.AddDays(oProductConfig.CoverDate.Period)
                Case TimeScale.Week
                    dCoverEndDate = dCoverStartDate.AddDays(oProductConfig.CoverDate.Period * 7)
                Case TimeScale.Month
                    If oProductConfig.CoverDate.TrueMonthlyPolicy Then
                        'End the cover, at the end of the month after period e.g 16/04 - 31/05
                        'DH - not sure if this is right, do we end at the end of current month or next if 1 month policy?
                        dCoverEndDate = dCoverStartDate.AddMonths(oProductConfig.CoverDate.Period)
                        dCoverEndDate = dCoverEndDate.AddDays(CInt(Date.DaysInMonth(dCoverEndDate.Year, dCoverEndDate.Month) - dCoverEndDate.Day))
                    Else
                        'One months time e.g 16/04 - 16-05
                        dCoverEndDate = dCoverStartDate.AddMonths(oProductConfig.CoverDate.Period)
                    End If
                Case TimeScale.Year
                    dCoverEndDate = dCoverStartDate.AddYears(oProductConfig.CoverDate.Period)
                Case Else
                    'Default to the StartDate, as this should be overridden by the risk screen
                    'DH - Not sure if a zero length policy will work?
                    dCoverEndDate = dCoverStartDate
            End Select

            Select Case oProductConfig.CoverDate.MidnightRenewal
                Case "true"
                    'If MidnightRenewal is true this means product need 365 days policy
                    Return dCoverEndDate.AddDays(-1)
                Case "false"
                    'If MidnightRenewal is false this means product need 366 days policy
                    Return dCoverEndDate
            End Select




        End Function

        Function GetReserves(ByVal v_iRiskKey As Integer) As NexusProvider.ClaimRiskLinkCollection

            Dim iInsuranceFileKey As Integer = CType(Current.Session.Item(CNInsuranceFileKey), Integer)

            Dim oUserDetails As NexusProvider.UserDetails = CType(Current.Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code

            Dim owebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimRisks As New NexusProvider.ClaimRiskLinkCollection

            Try
                oClaimRisks = owebservice.GetClaimRiskLinks(iInsuranceFileKey, v_iRiskKey, sBranchCode)

                If oClaimRisks IsNot Nothing Then

                    Dim oReserveDescriptions As New Hashtable
                    For Each oPeril As NexusProvider.ClaimRiskLink In oClaimRisks
                        If oPeril.ReserveItemType IsNot Nothing Then
                            For Each oReserve As NexusProvider.ReserveType In oPeril.ReserveItemType
                                If oReserveDescriptions.Item(oReserve.Code) Is Nothing Then
                                    oReserveDescriptions.Add(oReserve.Code, oReserve.Description)
                                End If
                            Next
                        End If
                    Next
                    Current.Session.Item(CNReserveDescriptions) = oReserveDescriptions

                End If
            Catch ex As NexusProvider.NexusException
                oClaimRisks = Nothing
            End Try

            Return oClaimRisks
        End Function

        Function GetRecovery(ByVal v_iRiskKey As Integer) As NexusProvider.ClaimRiskLinkCollection

            Dim iInsuranceFileKey As Integer = CType(Current.Session.Item(CNInsuranceFileKey), Integer)
            ' RCD - 08/02/2008 - Use Nexus session variables instead of claims
            'Dim sBranchCode As String = CType(Current.Session.Item(CNBranchCodes), BaseBranchType())(0).BranchCode

            Dim oUserDetails As NexusProvider.UserDetails = CType(Current.Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code
            ' END RCD

            Dim owebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimRisks As New NexusProvider.ClaimRiskLinkCollection

            Try
                oClaimRisks = owebservice.GetClaimRiskLinks(iInsuranceFileKey, v_iRiskKey, sBranchCode)

                If oClaimRisks IsNot Nothing Then

                    Dim oRecoveryDescriptions As New Hashtable
                    For Each oPeril As NexusProvider.ClaimRiskLink In oClaimRisks
                        If oPeril.RecoveryItemType IsNot Nothing Then
                            For Each oRecovery As NexusProvider.RecoveryType In oPeril.RecoveryItemType
                                If oRecoveryDescriptions.Item(oRecovery.Code) Is Nothing Then
                                    oRecoveryDescriptions.Add(oRecovery.Code, oRecovery.Description)
                                End If
                            Next
                        End If
                    Next
                    Current.Session.Item(CNRecoveryDescriptions) = oRecoveryDescriptions

                End If
            Catch ex As NexusProvider.NexusException
                oClaimRisks = Nothing
            End Try

            Return oClaimRisks
        End Function

        Public Function GetBranchCode() As String

            Dim oUserDetails As NexusProvider.UserDetails
            oUserDetails = CType(Current.Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code
            Dim iInsuranceFileKey As Integer = CType(Current.Session.Item(CNInsuranceFileKey), Integer)
            Dim sBranchCodeOut As String = ""
            If iInsuranceFileKey <> 0 Then

                Dim bQuoteTimeStamp() As Byte = Nothing
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oreturncode As NexusProvider.Quote
                Try
                    oreturncode = oWebservice.GetHeaderAndSummariesByKey(iInsuranceFileKey, sBranchCodeOut)
                    sBranchCodeOut = oreturncode.BranchCode
                Catch ex As NexusProvider.NexusException
                    oreturncode = Nothing
                End Try
            End If
            Return sBranchCodeOut
        End Function
        Public Function GetDescriptionForCode(ByVal v_oListType As NexusProvider.ListType,
                                             ByVal v_sCodeValue As String,
                                             ByVal v_sListCode As String,
                                             Optional ByVal v_sBranchCode As String = Nothing) As String

            'Dim r_oList() As Object = Nothing
            Dim sDescription As String = Nothing
            Dim owebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim r_oList As NexusProvider.LookupListCollection
            r_oList = owebservice.GetList(v_oListType, v_sListCode, True, False, , , v_sBranchCode)
            'List.GetList(GetUserToken(), STSListType.PMLookup, v_sBranchCode, r_oList, v_sListCode)

            If Not r_oList Is Nothing Then
                For Each tmpRow As Object In r_oList
                    If Trim(tmpRow.Code) = Trim(v_sCodeValue) Then
                        sDescription = tmpRow.Description
                        Exit For
                    End If
                Next
            End If
            Return sDescription

        End Function
        ''' <summary>
        ''' To get Key of State to bind the Address object and to set the State value in the Lookuplist
        ''' </summary>
        ''' <param name="v_oListType"></param>
        ''' <param name="v_sDescription"></param>
        ''' <param name="v_sListCode"></param>
        ''' <param name="v_sBranchCode"></param>
        ''' <returns>State Key</returns>
        ''' <remarks></remarks>
        Public Function GetKeyForDescription(ByVal v_oListType As NexusProvider.ListType,
                                             ByVal v_sDescription As String, ByVal v_sListCode As String,
                                             Optional ByVal v_sBranchCode As String = Nothing) As String
            Dim sKey As String = Nothing
            Dim owebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim r_oList As NexusProvider.LookupListCollection
            r_oList = owebservice.GetList(v_oListType, v_sListCode, False, False, v_sBranchCode)

            If Not r_oList Is Nothing Then
                For Each tmpRow As Object In r_oList
                    If Trim(tmpRow.Description).ToUpper() = Trim(v_sDescription).ToUpper() Then
                        sKey = tmpRow.Key
                        Exit For
                    End If
                Next
            End If
            Return sKey

        End Function
        ''' <summary>
        ''' To get Code and Value of Country to bind the Address object and to set the Country value in the Lookuplist
        ''' </summary>
        ''' <param name="v_oListType"></param>
        ''' <param name="v_sKeyValue"></param>
        ''' <param name="v_sListCode"></param>
        ''' <param name="v_bIsCode"></param>
        ''' <param name="v_sBranchCode"></param>
        ''' <returns>CountryCode or CountryValue</returns>

        Public Function GetCodeForKey(ByVal v_oListType As NexusProvider.ListType,
                                             ByVal v_sKeyValue As String, ByVal v_sListCode As String,
                                             ByVal v_bIsCode As Boolean,
                                             Optional ByVal v_sBranchCode As String = Nothing) As String


            Dim sCode As String = Nothing
            Dim owebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim r_oList As NexusProvider.LookupListCollection

            r_oList = owebservice.GetList(v_oListType, v_sListCode, False, False, v_sBranchCode)

            If Not r_oList Is Nothing Then
                For Each tmpRow As Object In r_oList
                    If v_bIsCode Then
                        If Trim(tmpRow.Key) = Trim(v_sKeyValue) Then
                            sCode = tmpRow.Code
                            Exit For
                        End If
                    Else
                        If Trim(tmpRow.Code) = Trim(v_sKeyValue) Then
                            sCode = tmpRow.Key.ToString()
                            Exit For
                        End If
                    End If
                Next
            End If
            Return sCode

        End Function

        Public Function GetCurrencyForCode(ByVal v_sCurrencyCode As String, Optional ByVal v_sBranchCode As String = Nothing) As String

            Dim sDescription As String = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oCurrencyCollection As NexusProvider.CurrencyCollection
            oCurrencyCollection = oWebservice.GetCurrenciesByBranch(v_sBranchCode)
            If Not oCurrencyCollection Is Nothing Then
                For Each currency As NexusProvider.Currency In oCurrencyCollection
                    If Trim(currency.CurrencyCode) = Trim(v_sCurrencyCode) Then
                        sDescription = currency.Description
                        Exit For
                    End If
                Next
            End If
            Return sDescription
        End Function
        Public Function GetCurrencyForDescription(ByVal v_sCurrencyDescription As String, Optional ByVal v_sBranchCode As String = Nothing) As String

            Dim sCurrencyCode As String = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oCurrencyCollection As NexusProvider.CurrencyCollection
            oCurrencyCollection = oWebservice.GetCurrenciesByBranch(v_sBranchCode)
            If Not oCurrencyCollection Is Nothing Then
                For Each currency As NexusProvider.Currency In oCurrencyCollection
                    If Trim(currency.Description.Trim.ToUpper) = Trim(v_sCurrencyDescription.Trim.ToUpper) Then
                        sCurrencyCode = currency.CurrencyCode
                        Exit For
                    End If
                Next
            End If
            Return sCurrencyCode
        End Function
        ''' <summary>
        ''' To check that the given product is assigned to logged in a agent or not
        ''' </summary>
        ''' <param name="oProductToCheck">Product to check for an agent</param>
        ''' <param name="oAgentProducts">All product assigned to agent</param>
        ''' <returns></returns>
        ''' <remarks></remarks>

        Public Function IsProductAssignedToAgent(ByVal oProductToCheck As Config.Product, ByVal oAgentProducts As NexusProvider.ProductCollection) As Boolean
            If oAgentProducts IsNot Nothing Then
                For Each oAgentProduct As NexusProvider.Product In oAgentProducts
                    If oAgentProduct.ProductCode.Trim() = oProductToCheck.ProductCode.Trim() Then
                        IsProductAssignedToAgent = True
                        Exit For
                    End If
                Next
            End If
        End Function
        ''' <summary>
        ''' To check that the given product is assigned to logged in a agent or not
        ''' </summary>
        ''' <param name="oProductToCheck">Product to check for an agent</param>
        ''' <param name="oUserProductsByBranch">All product assigned to agent</param>
        ''' <returns></returns>
        ''' <remarks></remarks>

        Public Function IsProductAssignedToUserBranch(ByVal oProductToCheck As Config.Product, ByVal oUserProductsByBranch As NexusProvider.UserProductByBranchCollection) As Boolean
            If oUserProductsByBranch IsNot Nothing Then
                For Each oAgentProduct As NexusProvider.UserProductByBranch In oUserProductsByBranch
                    If oAgentProduct.Code.Trim() = oProductToCheck.ProductCode.Trim() Then
                        For i As Integer = 0 To oAgentProduct.ListOfBranches.Count - 1
                            If Current.Session(CNBranchCode) = oAgentProduct.ListOfBranches(i).Code.ToString.Trim() Then
                                IsProductAssignedToUserBranch = True
                                Exit For
                            End If
                        Next
                    End If
                Next

            End If
        End Function
        ''' <summary>
        ''' To check that the logged in branch is assigned to product or not
        ''' </summary>
        ''' <param name="strProductCode">Product to check</param>
        ''' <returns></returns>
        ''' <remarks></remarks>

        Public Function IsloggedInBranchAssignedToProduct(ByVal strProductCode As String) As Boolean
            Dim oUserProducts As NexusProvider.UserProductByBranch = CType(Current.Session(CNAgentDetails), NexusProvider.UserDetails).AvailableUserProductsByBranch.GetProductByCode(strProductCode.Trim)
            If oUserProducts IsNot Nothing Then
                For i As Integer = 0 To oUserProducts.ListOfBranches.Count - 1
                    If Current.Session(CNBranchCode) = oUserProducts.ListOfBranches(i).Code.ToString.Trim() Then
                        IsloggedInBranchAssignedToProduct = True
                        Exit For
                    End If
                Next
            End If
            Dim oResource As ResXResourceReader
            Dim en As IDictionaryEnumerator
            Dim opage = TryCast(Current.CurrentHandler, System.Web.UI.Page)
            If Not IsloggedInBranchAssignedToProduct Then
                'if logged in branch is not assigned to product
                oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "App_LocalResources/Error.aspx.resx"))
                en = oResource.GetEnumerator()

                While (en.MoveNext)
                    If en.Key.ToString.Trim = "error_loggedInBranchAssignedToProduct" Then
                        opage.ClientScript.RegisterClientScriptBlock(opage.GetType(), "loggedInBranchCheck",
            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('" & en.Value.ToString & "'); return false;});</script>")
                    End If
                End While
                'HttpContext.Current.Response.End()
            End If
        End Function

        ''' <summary>
        ''' This Method executes the AuthoriseClaimPayment and take care session timestamp and record locking messages as well
        ''' </summary>
        ''' <param name="v_nClaimPaymentKey"></param>
        ''' <param name="v_sComments"></param>
        ''' <param name="v_bDeclined"></param>
        ''' <param name="v_sBranchCode"></param>
        ''' <param name="v_PaymentCashList"></param>
        ''' <param name="v_nAccountKey"></param>
        ''' <param name="v_dtPaymentDate"></param>
        ''' <param name="v_dtPaymentDateTo"></param>
        ''' <param name="v_sSourceIds"></param>
        ''' <param name="r_bIsUpdated"></param>
        ''' <remarks></remarks>
        Public Sub AuthoriseClaimPaymentCall(ByVal v_nClaimPaymentKey As Integer,
                                                ByVal v_sComments As String,
                                                ByVal v_bDeclined As Boolean,
                                                Optional ByVal v_sBranchCode As String = Nothing,
                                                Optional ByVal v_PaymentCashList As NexusProvider.PaymentCashListItemType = Nothing,
                                                Optional ByVal v_nAccountKey As Integer = 0,
                                                Optional ByVal v_dtPaymentDate As DateTime = Nothing,
                                                Optional ByVal v_dtPaymentDateTo As DateTime = Nothing,
                                                Optional ByVal v_sSourceIds As String = "",
                                                Optional ByRef r_bIsUpdated As Boolean = False,
                                                Optional ByRef r_sFailureReason As String = "",
                                                Optional ByRef r_bExclusiveLock As Boolean = False
            )

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
            Dim v_bTimeStamp As Byte()
            Try
                If Current.Session(CNClaimCallsTimeStamp) IsNot Nothing Then
                    v_bTimeStamp = Current.Session(CNClaimCallsTimeStamp)
                End If
                oWebservice.AuthoriseClaimPayment(v_nClaimPaymentKey, v_sComments, v_bDeclined, v_bTimeStamp, v_sBranchCode, v_PaymentCashList, v_nAccountKey, v_dtPaymentDate, v_dtPaymentDateTo, v_sSourceIds, r_bIsUpdated, r_sFailureReason:=r_sFailureReason, bExclusiveLock:=r_bExclusiveLock)
                Current.Session(CNClaimCallsTimeStamp) = v_bTimeStamp
            Catch ex As NexusProvider.NexusException
                r_sFailureReason = ex.Errors(0).Code
                Throw
                '    'Show the Error message if record locked
                '    HasLockedByAnotherUser(ex)
            End Try

        End Sub


        ''' <summary>
        ''' Method to Authorise the Claim Payment
        ''' </summary>
        ''' <param name="nClaimPaymentKey"></param>
        ''' <param name="sClaimNumber"></param>
        ''' <param name="dPaymentDate"></param>
        ''' <param name="sAhthoriseReason"></param>
        ''' <param name="sProductCode"></param>
        ''' <param name="sFailureReason"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AuthoriseClaimPayment(ByVal nClaimPaymentKey As Integer, ByVal sClaimNumber As String, ByVal dPaymentDate As Date, ByVal sAhthoriseReason As String, ByVal sProductCode As String, ByRef sFailureReason As String, Optional ByVal bExclusiveLock As Boolean = False) As Boolean

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oUnallocatedClaimPaymentsCollection As NexusProvider.UnallocatedClaimPaymentsCollection = Nothing
            Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
            Dim sBranchCode As String
            Dim bTimeStamp() As Byte = Current.Session(CNClaimCallsTimeStamp)

            If Current.Session(CNClaimQuote) IsNot Nothing Then
                sBranchCode = CType(Current.Session(CNClaimQuote), NexusProvider.Quote).BranchCode
            Else
                sFailureReason = "206"
                AuthoriseClaimPayment = False
                Exit Function
            End If

            Try
                oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, sProductCode)
                If oRunClaimWorkFlow.CashPaymentProcess = True Then

                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                    If oPortal.BackGroundCashListProcess = False Then

                        AuthoriseClaimPaymentCall(v_nClaimPaymentKey:=nClaimPaymentKey, v_sComments:=sAhthoriseReason, v_bDeclined:=False, v_sBranchCode:=sBranchCode, r_sFailureReason:=sFailureReason, r_bExclusiveLock:=bExclusiveLock)
                        oUnallocatedClaimPaymentsCollection = oWebservice.GetUnallocatedClaimPayment(0, dPaymentDate, dPaymentDate)

                        If Not oUnallocatedClaimPaymentsCollection Is Nothing AndAlso oUnallocatedClaimPaymentsCollection.Count > 0 Then
                            For i As Integer = 0 To oUnallocatedClaimPaymentsCollection.Count - 1
                                If Current.Session(CNClaimPaymentKey) = oUnallocatedClaimPaymentsCollection(i).BaseClaimPaymentKey Then
                                    Current.Session(CNUnAllocatedClaimPayment) = oUnallocatedClaimPaymentsCollection(i)
                                    Current.Session(CNCurrenyCode) = GetCodeForKey(NexusProvider.ListType.PMLookup, CType(oUnallocatedClaimPaymentsCollection(i).CurrencyKey, String), "Currency", True)
                                    Current.Response.Redirect(AppSettings("WebRoot") & "secure/payment/CashListNew.aspx", True)
                                    Exit For
                                End If
                            Next
                        End If
                    Else
                        CashListProcess(sFailureReason, bTimeStamp, sAhthoriseReason, dPaymentDate)
                    End If
                Else
                    AuthoriseClaimPaymentCall(v_nClaimPaymentKey:=nClaimPaymentKey, v_sComments:=sAhthoriseReason, v_bDeclined:=False, v_sBranchCode:=sBranchCode, r_sFailureReason:=sFailureReason, r_bExclusiveLock:=bExclusiveLock)
                End If

                AuthoriseClaimPayment = True
            Catch ex As NexusProvider.NexusException
                If ex.Errors(0).Code = "331" Then   'Code : 331 :: Description: DebtorUserGroupsAreNotSetup
                    AuthoriseClaimPayment = False
                    sFailureReason = "331"
                ElseIf ex.Errors(0).Code = "1000019" Then
                    AuthoriseClaimPayment = False
                    sFailureReason = "206"
                ElseIf ex.Errors(0).Code = "1000128" Then
                    AuthoriseClaimPayment = False
                    sFailureReason = "1000128"
                ElseIf ex.Errors(0).Code = "336" Then
                    'Throw New Exception(ex.Errors(0).Description)
                    AuthoriseClaimPayment = False
                    sFailureReason = GetGlobalResourceObject("ClaimsResource", "Authorise_Message")
                Else
                    'Throw New Exception(ex.Errors(0).Description)
                    AuthoriseClaimPayment = False
                    sFailureReason = ex.Errors(0).Code
                End If
            Finally
                oWebservice = Nothing
                oUnallocatedClaimPaymentsCollection = Nothing
                oRunClaimWorkFlow = Nothing
            End Try
        End Function

        ''' <summary>
        ''' Method to Decline the Claim Payment
        ''' </summary>
        ''' <param name="nClaimPaymentKey"></param>
        ''' <param name="sDeclineReason"></param>
        ''' <param name="sDeclineFailureReason"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DeclineClaimPayment(ByVal nClaimPaymentKey As Integer, ByVal sDeclineReason As String, Optional ByRef sDeclineFailureReason As String = "", Optional bExclusiveLock As Boolean = False) As Boolean
            Dim sBranchCode As String = CType(Current.Session.Item(CNClaimQuote), NexusProvider.Quote).BranchCode
            Try
                AuthoriseClaimPaymentCall(v_nClaimPaymentKey:=nClaimPaymentKey, v_sComments:=sDeclineReason, v_bDeclined:=True, v_sBranchCode:=sBranchCode, r_sFailureReason:=sDeclineFailureReason, r_bExclusiveLock:=bExclusiveLock)
                DeclineClaimPayment = True
            Catch ex As NexusProvider.NexusException
                If ex.Errors(0).Code = "331" Then   'Code : 331 :: Description: DebtorUserGroupsAreNotSetup
                    DeclineClaimPayment = False
                    sDeclineFailureReason = "331"
                ElseIf ex.Errors(0).Code = "1000019" Then
                    DeclineClaimPayment = False
                    sDeclineFailureReason = "206"
                ElseIf ex.Errors(0).Code = "1000128" Then
                    DeclineClaimPayment = False
                    sDeclineFailureReason = "1000128"
                Else
                    sDeclineFailureReason = ex.Errors(0).Detail
                    DeclineClaimPayment = False
                End If
            Finally
            End Try

        End Function

        ''' <summary>
        ''' This Method executes the UpdateRecommendStatus and take care session timestamp and record locking messages as well
        ''' </summary>
        ''' <param name="v_nClaimKey"></param>
        ''' <param name="v_sBranchCode"></param>
        ''' <remarks></remarks>
        Public Sub UpdateRecommendStatusCall(ByVal v_nClaimKey As Integer,
                                                 Optional ByVal v_sBranchCode As String = Nothing)

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
            Dim v_bTimeStamp As Byte()
            'Try
            If Current.Session(CNClaimCallsTimeStamp) IsNot Nothing Then
                v_bTimeStamp = Current.Session(CNClaimCallsTimeStamp)
            End If
            oWebservice.UpdateRecommendStatus(v_nClaimKey, v_bTimeStamp)
            Current.Session(CNClaimCallsTimeStamp) = v_bTimeStamp
            'Catch ex As NexusProvider.NexusException
            '    'Show the Error message if record locked
            '    HasLockedByAnotherUser(ex)
            'End Try

        End Sub


        ''' <summary>
        ''' Method for Recommending the Claim Payment
        ''' </summary>
        ''' <param name="nClaimKey"></param>
        ''' <param name="sProductCode"></param>
        ''' <param name="sFailureReason"></param>
        ''' <param name="bTimeStamp"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RecommendClaimPayment(ByVal nClaimKey As Integer, ByVal sProductCode As String, ByRef sFailureReason As String, ByRef bTimeStamp() As Byte) As Boolean

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
            Try
                oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, sProductCode)
                If oRunClaimWorkFlow.CashPaymentProcess = True Then
                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
                    If oPortal.BackGroundCashListProcess = False Then
                        Try
                            UpdateRecommendStatusCall(v_nClaimKey:=nClaimKey)
                            Current.Response.Redirect("~/secure/payment/CashList.aspx", True)
                        Catch ex As NexusProvider.NexusException
                            sFailureReason = ex.Errors(0).Code
                        Finally
                            oNexusConfig = Nothing
                            oPortal = Nothing
                        End Try
                    Else
                        CashListProcess(sFailureReason, bTimeStamp)
                        If String.IsNullOrEmpty(sFailureReason) Then
                            Try
                                UpdateRecommendStatusCall(v_nClaimKey:=nClaimKey)
                                Current.Response.Redirect("~/secure/AuthoriseClaimPayments.aspx", True)
                            Catch ex As NexusProvider.NexusException
                                sFailureReason = ex.Errors(0).Code
                            Finally
                                oNexusConfig = Nothing
                                oPortal = Nothing
                            End Try
                        End If
                    End If
                Else
                    Try
                        UpdateRecommendStatusCall(v_nClaimKey:=nClaimKey)
                    Catch ex As NexusProvider.NexusException
                        sFailureReason = ex.Errors(0).Code
                    End Try
                End If
                RecommendClaimPayment = True
            Catch ex As NexusProvider.NexusException
                sFailureReason = ex.Errors(0).Code
                RecommendClaimPayment = False

            Finally
                oWebservice = Nothing
                oRunClaimWorkFlow = Nothing
            End Try

        End Function

        ''' <summary>
        ''' Cash List Process
        ''' </summary>
        ''' <param name="sFailureReason"></param>
        ''' <param name="bTimeStamp"></param>
        ''' <param name="sComments"></param>
        ''' <param name="dPaymentDate"></param>
        ''' <remarks></remarks>
        Private Sub CashListProcess(ByRef sFailureReason As String, ByRef bTimeStamp() As Byte, Optional ByVal sComments As String = "", Optional ByVal dPaymentDate As Date = Nothing)

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPaymentCashListItem As New NexusProvider.PaymentCashListItemType
            Dim oCashListItem As New NexusProvider.PaymentItems
            Dim oOpenClaim As NexusProvider.ClaimOpen
            Dim oQuote As NexusProvider.Quote = Nothing
            Try
                oQuote = CType(Current.Session(CNClaimQuote), NexusProvider.Quote)
                oOpenClaim = CType(Current.Session(CNClaim), NexusProvider.ClaimOpen)
                Dim sBranchCode As String = oQuote.BranchCode
                If Current.Session(CNMode) = Mode.Recommend Then

                    Dim sPerilTypeCode As String = Nothing
                    Dim nPerilKey As Integer
                    Dim nPerilIndex As Integer
                    Dim nClaimPaymentIndex As Integer
                    Dim bFoundPayment As Boolean

                    For lCount As Integer = 0 To oOpenClaim.ClaimPeril.Count - 1
                        For lInnerCount As Integer = 0 To oOpenClaim.ClaimPeril(lCount).ClaimPayment.Count - 1
                            If Current.Session(CNClaimPaymentKey) = oOpenClaim.ClaimPeril(lCount).ClaimPayment(lInnerCount).BaseClaimPaymentKey Then
                                sPerilTypeCode = oOpenClaim.ClaimPeril(lCount).TypeCode
                                nPerilKey = oOpenClaim.ClaimPeril(lCount).ClaimPerilKey
                                nClaimPaymentIndex = lInnerCount
                                bFoundPayment = True
                                Exit For
                            End If
                        Next
                        If bFoundPayment Then
                            nPerilIndex = lCount
                            Current.Session(CNClaimPerilIndex) = nPerilIndex
                            Exit For
                        End If
                    Next

                    Dim nMediaTypeId As Integer
                    Dim sMediaTypeCode As String
                    Dim nBankAccountId As Integer
                    Dim sBankAccountCode As String

                    GetBankAccountDefault(nMediaTypeId, nBankAccountId)

                    If nBankAccountId > 0 Then
                        sMediaTypeCode = GetCodeForKey(NexusProvider.ListType.PMLookup, nMediaTypeId, "MediaType", True)
                        sBankAccountCode = GetCodeForKey(NexusProvider.ListType.PMLookup, nBankAccountId, "BankAccount", True)
                    Else
                        sFailureReason = "Bank"
                        Exit Sub
                    End If

                    With oPaymentCashListItem.CoreCashList
                        .BankAccountCode = sBankAccountCode
                        .CurrencyCode = oOpenClaim.CurrencyISOCode
                        .ListDate = Today.Date
                        .TypeCode = "CP"
                        .StatusCode = "E"
                        .BankAccountKey = nBankAccountId
                    End With

                    If Not String.IsNullOrEmpty(oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).PartyPaidCode) Then
                        oCashListItem.AccountShortCode = oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).PartyPaidCode
                    Else
                        oCashListItem.AccountShortCode = oOpenClaim.ClientShortName
                    End If

                    'Need to pass this amout with tax.So that it can be fully allocated.
                    oCashListItem.Amount = oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).PaymentAmount + oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).TaxAmount
                    If String.IsNullOrEmpty(oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).Payee.MediaTypeCode) Then
                        If String.IsNullOrEmpty(sMediaTypeCode) Then
                            sFailureReason = "Mandatory"
                            Exit Sub
                        Else
                            oCashListItem.MediaTypeCode = sMediaTypeCode
                        End If
                    Else
                        oCashListItem.MediaTypeCode = oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).Payee.MediaTypeCode
                    End If
                    oCashListItem.StatusCode = "ISS"
                    oCashListItem.AllocationStatusCode = "U"

                    With oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).Payee
                        oCashListItem.Address = New NexusProvider.Address
                        oCashListItem.Address.Address1 = .Address.Address1
                        oCashListItem.Address.Address2 = .Address.Address2
                        oCashListItem.Address.Address3 = .Address.Address3
                        oCashListItem.Address.Address4 = .Address.Address4
                        oCashListItem.Address.CountryCode = .Address.CountryCode
                        oCashListItem.Address.PostCode = .Address.PostCode
                        oCashListItem.MediaReference = .MediaReference
                        oCashListItem.Bank = New NexusProvider.Bank
                        oCashListItem.Bank.AccountCode = .BankNumber
                        oCashListItem.Bank.BankCode = .BankCode
                        oCashListItem.Bank.BankName = .BankName
                        oCashListItem.Bank.AccountNumber = .BankNumber
                        oCashListItem.Bank.PayeeName = .Name
                        oCashListItem.FurtherDetails = .Comments
                        oCashListItem.TheirReference = .TheirReference
                        oCashListItem.Bank.BranchCode = .BankCode
                        oCashListItem.Bank.BIC = .BIC
                        oCashListItem.Bank.IBAN = .IBAN
                        oCashListItem.Bank.PartyBankKey = .PartyBankKey

                    End With
                    oCashListItem.OurReference = oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).OurRef

                    oCashListItem.ContactName = oQuote.InsuredName
                    oCashListItem.TransactionDate = Today.Date
                    oCashListItem.TypeCode = "CLP"
                    oCashListItem.SkipPosting = True

                    oPaymentCashListItem.PaymentItems.Add(oCashListItem)

                    oWebservice.CreatePaymentCashListWithItems(oPaymentCashListItem)
                    oWebservice.AddCashClaimLink(CType(Current.Session(CNClaimPaymentKey), Integer), oPaymentCashListItem.PaymentCashList(0).CashListItemKey)

                ElseIf Current.Session(CNMode) = Mode.Authorise Then

                    Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
                    Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
                    Dim oAllocationDetails As New NexusProvider.AllocationDetails
                    Dim oAllocation As NexusProvider.Allocation
                    Dim oTransAllocationDetails As New NexusProvider.AllocationDetails
                    Dim bIsUpdated As Boolean

                    'Fetch CashList Details
                    Dim oCashClaimLink As NexusProvider.CashClaimLink
                    oCashClaimLink = oWebservice.GetCashClaimLink(CType(Current.Session(CNClaimPaymentKey), Integer))

                    If oCashClaimLink IsNot Nothing AndAlso oCashClaimLink.CashListKey > 0 Then

                        'No validation required in this case so we can authorise payment
                        Dim oUnallocatedClaimPaymentsCollection As NexusProvider.UnallocatedClaimPaymentsCollection = Nothing

                        'fetch cashlistitems
                        Dim oPaymentCashListItemsCollection As NexusProvider.PaymentCashListItemTypeCollection

                        oPaymentCashListItemsCollection = oWebservice.GetPaymentTypeCashListItem(oCashClaimLink.CashListItemKey)

                        Dim oPaymentItem As NexusProvider.PaymentItems
                        If oPaymentCashListItemsCollection IsNot Nothing AndAlso oPaymentCashListItemsCollection.Count > 0 Then
                            For Each oCashListItemRet As NexusProvider.PaymentItems In oPaymentCashListItemsCollection(0).PaymentItems
                                oPaymentCashListItem.CoreCashList.BankAccountCode = oPaymentCashListItemsCollection(0).CoreCashList.BankAccountCode
                                oPaymentCashListItem.CoreCashList.CurrencyCode = oPaymentCashListItemsCollection(0).CoreCashList.CurrencyCode
                                oPaymentCashListItem.CoreCashList.ListDate = oPaymentCashListItemsCollection(0).CoreCashList.ListDate
                                oPaymentCashListItem.CoreCashList.Reference = oPaymentCashListItemsCollection(0).CoreCashList.Reference
                                oPaymentCashListItem.CoreCashList.StatusCode = oPaymentCashListItemsCollection(0).CoreCashList.StatusCode
                                oPaymentCashListItem.CoreCashList.TypeCode = oPaymentCashListItemsCollection(0).CoreCashList.TypeCode
                                oPaymentCashListItem.CoreCashList.CashListKey = oCashClaimLink.CashListKey
                                oPaymentCashListItem.CoreCashList.BankAccountKey = oPaymentCashListItemsCollection(0).CoreCashList.BankAccountKey

                                oPaymentItem = New NexusProvider.PaymentItems
                                With oPaymentItem
                                    .Address = oCashListItemRet.Address
                                    .AccountShortCode = oCashListItemRet.AccountShortCode
                                    .AllocationStatusCode = oCashListItemRet.AllocationStatusCode
                                    .Amount = oCashListItemRet.Amount
                                    .Bank = oCashListItemRet.Bank
                                    .BankReference = oCashListItemRet.BankReference
                                    .CashListItemKey = oCashListItemRet.CashListItemKey
                                    .ContactName = oCashListItemRet.ContactName
                                    .CreditCard = oCashListItemRet.CreditCard
                                    .FurtherDetails = oCashListItemRet.FurtherDetails
                                    .IsProduceDocument = oCashListItemRet.IsProduceDocument
                                    .Letter = oCashListItemRet.Letter
                                    .MediaReference = oCashListItemRet.MediaReference
                                    .MediaTypeCode = oCashListItemRet.MediaTypeCode
                                    .OurReference = oCashListItemRet.OurReference
                                    'oPaymentCashListItemsCollection(0).PaymentItems(0).Policies = = oPaymentCashListItemsCollection(0).PaymentCashList(0).p
                                    .SkipPosting = False
                                    .StatusCode = oCashListItemRet.StatusCode
                                    .TaxAmount = oCashListItemRet.TaxAmount
                                    .TaxBandCode = oCashListItemRet.TaxBandCode
                                    .TaxBandKey = oCashListItemRet.TaxBandKey
                                    .TheirReference = oCashListItemRet.TheirReference
                                    .TransactionDate = oCashListItemRet.TransactionDate
                                    .TypeCode = oCashListItemRet.TypeCode
                                End With
                                oPaymentCashListItem.PaymentItems.Add(oPaymentItem)
                            Next
                        End If

                        Dim oUserDetails As NexusProvider.UserDetails = CType(Current.Session(CNAgentDetails), NexusProvider.UserDetails)
                        Dim sSourceIds As String = String.Empty
                        For iCount As Integer = 0 To oUserDetails.ListOfBranches.Count - 1
                            If oUserDetails.ListOfBranches(iCount).BranchKey <> 0 Then
                                sSourceIds = sSourceIds & oUserDetails.ListOfBranches(iCount).BranchKey & ","
                            End If
                        Next
                        If Not String.IsNullOrEmpty(sSourceIds) Then
                            sSourceIds = Left(sSourceIds, Len(sSourceIds) - 1)
                        End If

                        AuthoriseClaimPaymentCall(v_nClaimPaymentKey:=CType(Current.Session(CNClaimPaymentKey), Integer),
                                                          v_sComments:=sComments,
                                                          v_bDeclined:=False,
                                                          v_sBranchCode:=sBranchCode,
                                                          v_PaymentCashList:=oPaymentCashListItem,
                                                          v_nAccountKey:=0,
                                                          v_dtPaymentDate:=dPaymentDate,
                                                          v_dtPaymentDateTo:=dPaymentDate,
                                                          v_sSourceIds:=sSourceIds,
                                                          r_bIsUpdated:=bIsUpdated,
                                                          r_sFailureReason:=sFailureReason)
                        If Not String.IsNullOrEmpty(sFailureReason) Then Exit Sub
                        'From the response of AuthoriseClaimPayment set these session values
                        oUnallocatedClaimPaymentsCollection = oWebservice.GetUnallocatedClaimPayment(0, dPaymentDate, dPaymentDate)
                        For i As Integer = 0 To oUnallocatedClaimPaymentsCollection.Count - 1
                            If Current.Session(CNClaimPaymentKey) = oUnallocatedClaimPaymentsCollection(i).BaseClaimPaymentKey Then
                                Current.Session(CNUnAllocatedClaimPayment) = oUnallocatedClaimPaymentsCollection(i)
                                Current.Session(CNCurrenyCode) = GetCodeForKey(NexusProvider.ListType.PMLookup, oUnallocatedClaimPaymentsCollection(i).CurrencyKey, "Currency", True)
                                Exit For
                            End If
                        Next

                        If bIsUpdated Then
                            Current.Response.Redirect("~/secure/AuthoriseClaimPayments.aspx")
                        End If
                        oUserDetails = Nothing
                        oPaymentCashListItemsCollection = Nothing
                        oUnallocatedClaimPaymentsCollection = Nothing
                    Else
                        Dim sPerilTypeCode As String = Nothing
                        Dim nPerilKey As Integer
                        Dim nPerilIndex As Integer
                        Dim nClaimPaymentIndex As Integer
                        Dim bFoundPayment As Boolean

                        For lCount As Integer = 0 To oOpenClaim.ClaimPeril.Count - 1
                            For lInnerCount As Integer = 0 To oOpenClaim.ClaimPeril(lCount).ClaimPayment.Count - 1
                                If Current.Session(CNClaimPaymentKey) = oOpenClaim.ClaimPeril(lCount).ClaimPayment(lInnerCount).BaseClaimPaymentKey Then
                                    sPerilTypeCode = oOpenClaim.ClaimPeril(lCount).TypeCode
                                    nPerilKey = oOpenClaim.ClaimPeril(lCount).ClaimPerilKey
                                    nClaimPaymentIndex = lInnerCount
                                    bFoundPayment = True
                                    Exit For
                                End If
                            Next
                            If bFoundPayment Then
                                nPerilIndex = lCount
                                Current.Session(CNClaimPerilIndex) = nPerilIndex
                                Exit For
                            End If
                        Next

                        Dim nMediaTypeId As Integer
                        Dim sMediaTypeCode As String
                        Dim nBankAccountId As Integer
                        Dim sBankAccountCode As String

                        GetBankAccountDefault(nMediaTypeId, nBankAccountId)

                        If nBankAccountId > 0 Then
                            sMediaTypeCode = GetCodeForKey(NexusProvider.ListType.PMLookup, CType(nMediaTypeId, String), "MediaType", True)
                            sBankAccountCode = GetCodeForKey(NexusProvider.ListType.PMLookup, CType(nBankAccountId, String), "BankAccount", True)
                        Else
                            sFailureReason = "Bank"
                            Exit Sub
                        End If

                        With oPaymentCashListItem.CoreCashList
                            .BankAccountCode = sBankAccountCode
                            .CurrencyCode = oOpenClaim.CurrencyISOCode
                            .ListDate = Today.Date
                            .TypeCode = "CP"
                            .StatusCode = "E"
                        End With

                        If Not String.IsNullOrEmpty(oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).PartyPaidCode) Then
                            oCashListItem.AccountShortCode = oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).PartyPaidCode
                        Else
                            oCashListItem.AccountShortCode = oOpenClaim.ClientShortName
                        End If

                        oCashListItem.StatusCode = "ISS"
                        oCashListItem.AllocationStatusCode = "U"

                        If String.IsNullOrEmpty(oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).Payee.MediaTypeCode) Then
                            If String.IsNullOrEmpty(sMediaTypeCode) Then
                                sFailureReason = "Mandatory"
                                Exit Sub
                            Else
                                oCashListItem.MediaTypeCode = sMediaTypeCode
                            End If
                        Else
                            oCashListItem.MediaTypeCode = oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).Payee.MediaTypeCode
                        End If

                        'No error occured till now so we can authorise payment
                        Dim oUnallocatedClaimPaymentsCollection As NexusProvider.UnallocatedClaimPaymentsCollection = Nothing
                        AuthoriseClaimPaymentCall(CType(Current.Session(CNClaimPaymentKey), Integer), sComments, False, sBranchCode, r_sFailureReason:=sFailureReason)
                        If Not String.IsNullOrEmpty(sFailureReason) Then Exit Sub

                        oUnallocatedClaimPaymentsCollection = oWebservice.GetUnallocatedClaimPayment(0, dPaymentDate, dPaymentDate)

                        For i As Integer = 0 To oUnallocatedClaimPaymentsCollection.Count - 1
                            If Current.Session(CNClaimPaymentKey) = oUnallocatedClaimPaymentsCollection(i).BaseClaimPaymentKey Then
                                Current.Session(CNUnAllocatedClaimPayment) = oUnallocatedClaimPaymentsCollection(i)
                                Current.Session(CNCurrenyCode) = GetCodeForKey(NexusProvider.ListType.PMLookup, CType(oUnallocatedClaimPaymentsCollection(i).CurrencyKey, String), "Currency", True)
                                Exit For
                            End If
                        Next
                        Dim oUnallocatedClaimPayments As NexusProvider.UnallocatedClaimPayments = CType(Current.Session(CNUnAllocatedClaimPayment), NexusProvider.UnallocatedClaimPayments)
                        Dim iAccountKey As Integer = oUnallocatedClaimPayments.AccountKey
                        Dim dAmount As Double = oUnallocatedClaimPayments.Amount

                        'ClP stored in negative for postive so apply this logic to get posted SPY instead of SRP.
                        oCashListItem.Amount = -1 * dAmount
                        oCashListItem.Amount_tendered = -1 * dAmount

                        With oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).Payee
                            oCashListItem.Address = New NexusProvider.Address
                            oCashListItem.Address.Address1 = .Address.Address1
                            oCashListItem.Address.Address2 = .Address.Address2
                            oCashListItem.Address.Address3 = .Address.Address3
                            oCashListItem.Address.Address4 = .Address.Address4
                            oCashListItem.Address.CountryCode = .Address.CountryCode
                            oCashListItem.Address.PostCode = .Address.PostCode
                            oCashListItem.MediaReference = .MediaReference
                            oCashListItem.Bank = New NexusProvider.Bank
                            oCashListItem.Bank.BankCode = .BankCode
                            oCashListItem.Bank.BankName = .BankName
                            oCashListItem.Bank.AccountNumber = .BankNumber
                            oCashListItem.Bank.PayeeName = .Name
                            oCashListItem.FurtherDetails = .Comments
                            oCashListItem.TheirReference = .TheirReference
                            oCashListItem.Bank.AccountCode = .BankNumber
                            oCashListItem.Bank.BranchCode = .BankCode
                            oCashListItem.Bank.BIC = .BIC
                            oCashListItem.Bank.IBAN = .IBAN
                            oCashListItem.Bank.PartyBankKey = .PartyBankKey

                        End With
                        oCashListItem.OurReference = oOpenClaim.ClaimPeril(nPerilIndex).ClaimPayment(nClaimPaymentIndex).OurRef

                        oCashListItem.ContactName = oQuote.InsuredName 'oOpenClaim.Insurer.ContactName
                        oCashListItem.TransactionDate = Today.Date
                        oCashListItem.TypeCode = "CLP"

                        oPaymentCashListItem.PaymentItems.Add(oCashListItem)

                        oWebservice.CreatePaymentCashListWithItems(oPaymentCashListItem)

                        'Finding of the Transdetails Key               
                        Dim oAccountDetails As New NexusProvider.AccountDetails
                        Dim oAccountDetailsDefaults As New NexusProvider.AccountDetailsDefaults

                        oAccountDetails.DocumentRef = oUnallocatedClaimPayments.DocumentRef
                        oAccountDetails.AccountKey = oUnallocatedClaimPayments.AccountKey
                        Dim oUserDetails As NexusProvider.UserDetails = CType(Current.Session(CNAgentDetails), NexusProvider.UserDetails)
                        Dim sSourceIds As String = String.Empty
                        For iCount As Integer = 0 To oUserDetails.ListOfBranches.Count - 1
                            sSourceIds = sSourceIds & oUserDetails.ListOfBranches(iCount).BranchKey & ","
                        Next
                        If Not String.IsNullOrEmpty(sSourceIds) Then
                            sSourceIds = Left(sSourceIds, Len(sSourceIds) - 1)
                            oAccountDetails.SourceArray = sSourceIds
                        End If
                        oAccountDetailsDefaults = oWebservice.GetAccountDetails(oAccountDetails)
                        'Assignment of the Transdetails Key
                        oAllocationDetails.TransdetailKey = CType(oAccountDetailsDefaults.AccountDetails(0).TransDetailKeys, Integer)
                        oAllocationDetailsCollections.Add(oAllocationDetails)
                        oTrasactionDetails = oWebservice.GetTransactionDetails(iAccountKey, oAllocationDetailsCollections)

                        For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                            oAllocation = New NexusProvider.Allocation
                            oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                            oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp
                            oAllocation.AllocationTransdetailKey = oTempAllocationDetails.TransdetailKey
                            oTransAllocationDetails.Allocation.Add(oAllocation)
                            oAllocation = Nothing
                        Next
                        oTransAllocationDetails.AccountKey = iAccountKey
                        oTransAllocationDetails.CashListItemKey = oPaymentCashListItem.PaymentCashList(0).CashListItemKey
                        oTransAllocationDetails.Amount = -dAmount
                        oTransAllocationDetails.TransdetailKey = oPaymentCashListItem.PaymentCashList(0).TransDetailKey
                        'Allocation done here
                        bIsUpdated = oWebservice.UpdateAllocation(oTransAllocationDetails)
                        If bIsUpdated Then
                            Current.Response.Redirect("~/secure/AuthoriseClaimPayments.aspx")
                        End If
                        oUnallocatedClaimPayments = Nothing
                        oUnallocatedClaimPaymentsCollection = Nothing
                        oAccountDetails = Nothing
                        oAccountDetailsDefaults = Nothing
                        oUserDetails = Nothing
                    End If
                End If

            Catch ex As Exception
                Throw
            Finally
                oWebservice = Nothing
                oPaymentCashListItem = Nothing
                oCashListItem = Nothing
                oOpenClaim = Nothing
                oQuote = Nothing
            End Try
        End Sub


        ''' <summary>
        ''' Method to Get the BankAccount Defaults
        ''' </summary>
        ''' <param name="sType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetBankAccountDefault(ByVal sMediaType As String, ByVal sProductCode As String, ByVal sType As String) As NexusProvider.BankAccountDefaults
            Dim oBankDefaults As New NexusProvider.BankAccountDefaults
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oMediaList As NexusProvider.LookupListCollection
            Try
                Dim v_sOptionList As System.Xml.XmlElement = Nothing
                oMediaList = oWebservice.GetList(NexusProvider.ListType.PMLookup, "BankAccount_Default", True, False, , , , v_sOptionList)
                Dim iSourceId As Integer
                iSourceId = GetCodeForKey(NexusProvider.ListType.PMLookup, CType(Current.Session(CNBranchCode), String), "Source", False)
                'Load the xml element 
                If v_sOptionList IsNot Nothing Then
                    Dim sXML As String = v_sOptionList.OuterXml
                    Dim xmlDoc As New System.Xml.XmlDocument
                    xmlDoc.LoadXml(sXML)
                    Dim oNodeList As XmlNodeList
                    If sType = "Reciept" Then
                        oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/BankAccount_Default[cashlisttype_id=2 and source_id=" & iSourceId & " and is_deleted=0]")
                    Else    'Claim Payment
                        oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/BankAccount_Default[cashlisttype_id=3 and source_id=" & iSourceId & " and is_deleted=0]")
                    End If

                    If oNodeList IsNot Nothing And oNodeList.Count > 0 Then
                        For Each oNode As XmlNode In oNodeList
                            Dim oBankDefault As New NexusProvider.BankAccountDefault
                            oBankDefault.Code = oNode.ChildNodes(9).InnerText
                            oBankDefault.MediaTypeID = CType(oNode.ChildNodes(9).InnerText, Integer)
                            oBankDefaults.Add(oBankDefault)
                        Next
                    End If
                End If

            Catch ex As Exception
            Finally
                oWebservice = Nothing
                oMediaList = Nothing
            End Try

            Return oBankDefaults
        End Function

        ''' <summary>
        ''' To Get Default Bank Account defaults detail
        ''' </summary>
        ''' <param name="nMediaTypeId"></param>
        ''' <param name="nBankAccountId"></param>
        ''' <param name="nCashlistTypeid"></param>
        ''' <remarks></remarks>
        Public Sub GetBankAccountDefault(ByRef nMediaTypeId As Integer, ByRef nBankAccountId As Integer, Optional ByRef nCashlistTypeid As Integer = 0, Optional ByVal sBranchCode As String = Nothing)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oMediaList As NexusProvider.LookupListCollection
            Dim v_sOptionList As System.Xml.XmlElement = Nothing
            Dim nSourceId As Integer
            Try
                oMediaList = oWebservice.GetList(NexusProvider.ListType.PMLookup, "BankAccount_Default", True, False, , , , v_sOptionList)
                If String.IsNullOrEmpty(sBranchCode) Then
                    nSourceId = GetCodeForKey(NexusProvider.ListType.PMLookup, CType(Current.Session(CNBranchCode), String), "Source", False)
                Else
                    nSourceId = GetCodeForKey(NexusProvider.ListType.PMLookup, sBranchCode, "Source", False)
                End If

                'Load the xml element 
                If v_sOptionList IsNot Nothing Then
                    Dim sXML As String = v_sOptionList.OuterXml
                    Dim xmlDoc As New System.Xml.XmlDocument
                    xmlDoc.LoadXml(sXML)
                    Dim oNodeList As XmlNodeList
                    If nCashlistTypeid <> 0 Then
                        oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/BankAccount_Default[cashlisttype_id=" & nCashlistTypeid & " and source_id=" & nSourceId & " and is_deleted=0]")
                    Else
                        oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/BankAccount_Default[cashlisttype_id=3 and source_id=" & nSourceId & " and is_deleted=0]")
                    End If
                    If oNodeList IsNot Nothing AndAlso oNodeList.Count > 0 Then
                        For Each oNode As XmlNode In oNodeList
                            If oNode.ChildNodes(9) IsNot Nothing Then
                                nMediaTypeId = oNode.ChildNodes(9).InnerText
                            End If
                            If oNode.ChildNodes(4) IsNot Nothing Then
                                nBankAccountId = oNode.ChildNodes(4).InnerText
                            End If
                            If nMediaTypeId > 0 AndAlso nBankAccountId > 0 Then
                                Exit For
                            End If
                        Next
                    End If
                    oNodeList = Nothing
                End If
            Catch ex As Exception

            Finally
                oWebservice = Nothing
                oMediaList = Nothing
                v_sOptionList = Nothing
            End Try
        End Sub
        ''' <summary>
        ''' This method checks and redirect the page as per claim builder config
        ''' </summary>
        ''' <remarks></remarks>
        Function CheckClaimBuilder(Optional ByRef sScreenCode As String = "") As String
            Dim sReturnUrl As String = String.Empty
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaim As NexusProvider.ClaimOpen = CType(Current.Session(CNClaim), NexusProvider.ClaimOpen)
            Dim sFolder As String = Nothing
            Dim sClaimConfigFile As String = Nothing
            Dim sFirstPage As String = Nothing
            Dim oOptionType As New NexusProvider.OptionTypeSetting
            oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
            'Checking of the Claim Builder
            If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
            AndAlso oOptionType.OptionValue = "1" Then
                'Claim Builder is ON
                If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)) Is Nothing Then
                    'use the default folder If risk type is not configured
                    sFolder = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "Claims/ClientPages/" & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ScreenLocation & "/Claims/" _
                              & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.DefaultFolder
                ElseIf String.IsNullOrEmpty(oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)).Folder) = True Then
                    'use the default folder,if folder is empty
                    sFolder = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "Claims/ClientPages/" & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ScreenLocation & "/Claims/" _
                              & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.DefaultFolder
                ElseIf String.IsNullOrEmpty(oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)).Folder) = False Then
                    'we have the risk type specified so use that folder
                    sFolder = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "Claims/ClientPages/" & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ScreenLocation & "/Claims/" _
                              & oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaim.RiskType)).Folder
                End If

                sClaimConfigFile = sFolder & "/claimscreens.config"
                sScreenCode = GetScreenCode(sClaimConfigFile)

                If String.IsNullOrEmpty(sFolder) Or System.IO.File.Exists(Current.Server.MapPath(sClaimConfigFile)) = False Then
                    'Claim Builder Configured but Web.Config is Not Configured
                    Current.Session(CNClaimBuilder) = False
                    sReturnUrl = "~/Claims/Perils.aspx"
                Else
                    'Claim Builder Configured 
                    'Redirect to the First Page of the claim builder

                    If System.IO.File.Exists(Current.Server.MapPath(sClaimConfigFile)) = True Then
                        sFirstPage = FrameWorkFunctions.GetFirstRiskScreen(sFolder & "/claimscreens.config")

                        Dim sUrl As String = sFolder & "/" & sFirstPage & ""
                        'Reset the OI collection
                        Current.Session.Remove(CNOI)
                        Current.Session(CNClaimBuilder) = True
                        sReturnUrl = sUrl
                    End If
                End If
            Else
                'Claim Builder is OFF
                'Claim Builder Not Configured
                Current.Session(CNClaimBuilder) = False
                sReturnUrl = "~/Claims/Perils.aspx"
            End If

            Return sReturnUrl
        End Function

        ''' <summary>
        ''' This method update the premium with agent commision for agent type BROKER
        ''' </summary>
        ''' <remarks></remarks>
        Sub UpdatePremiumWithAgentCommision()

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            Dim dTatalPremium As Decimal = CheckAndCalculateRoundOff()
            Dim dAgentCommission As Double
            Dim dTaxOnAgentCommission As Double
            If Current.Session(CNLoginType) = LoginType.Agent Then
                'Check if Agent is Broker then Agent Commission should be deducted from Total AMount
                Dim bFound As Boolean = False
                Dim oAgentCommission As NexusProvider.EditAgentCommission
                'make SAM call to get the Agent Commission and save in cache for further use
                oAgentCommission = oWebService.GetAgentCommission(oQuote.InsuranceFileKey)
                If oAgentCommission IsNot Nothing Then
                    With oAgentCommission
                        For iCt As Integer = 0 To oAgentCommission.AgentCommission.Count - 1
                            Dim oSelectAgentCommission As NexusProvider.AgentCommission = .AgentCommission(iCt)
                            dAgentCommission = dAgentCommission + oSelectAgentCommission.CommissionValue
                            dTaxOnAgentCommission = dTaxOnAgentCommission + oSelectAgentCommission.TaxValue
                            Current.Session(CNAgentComm) = dAgentCommission
                        Next
                    End With
                End If
                If Current.Session(CNAgentType) IsNot Nothing And Current.Session(CNAgentComm) IsNot Nothing Then
                    If Current.Session(CNAgentType).ToString.Trim.ToUpper = "BROKER" Then
                        Dim dAgentComm As Decimal = Current.Session(CNAgentComm)
                        dTatalPremium = dTatalPremium - dAgentComm
                        Current.Session.Add(CNAmountToPay, dTatalPremium)
                        bFound = True
                    End If
                Else
                    'Find The AgentType through SAM Call
                    Dim oTempParty As NexusProvider.PartyCollection
                    Dim oTempSearchCriteria As New NexusProvider.PartySearchCriteria

                    oTempSearchCriteria.AgentType = Nothing
                    oTempSearchCriteria.ShortName = CType(Current.Session(CNQuote), NexusProvider.Quote).AgentCode
                    oTempSearchCriteria.PartyType = If(Current.Session(CNAgentDetails) IsNot Nothing, CType(Current.Session(CNAgentDetails), NexusProvider.UserDetails).PartyType, Nothing)
                    oTempSearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AG)

                    If Not String.IsNullOrEmpty(oTempSearchCriteria.ShortName) Then
                        oTempParty = oWebService.FindParty(oTempSearchCriteria)

                        If oTempParty IsNot Nothing Then
                            If oTempParty.Count > 0 Then
                                Current.Session(CNAgentType) = oTempParty(0).AgentType
                                'Check if Agent is Broker then Agent Commission should be deducted from Total AMount
                                If Current.Session(CNAgentType).ToString.Trim.ToUpper = "BROKER" Then
                                    Dim dAgentComm As Decimal = CDec(If(Current.Session(CNAgentComm), dAgentCommission))
                                    dTatalPremium = dTatalPremium - dAgentComm
                                    Current.Session.Add(CNAmountToPay, dTatalPremium)
                                    bFound = True
                                End If
                            End If
                        End If
                    End If
                End If
                'if bFound is False it means that Agnet is Not Broker so that Full AMount will move further
                If bFound = False Then
                    Current.Session.Add(CNAmountToPay, dTatalPremium)
                End If
            End If
        End Sub
        ''' <summary>
        ''' This method checks the premium and other condition and redirect the relevant url
        ''' </summary>
        ''' <remarks></remarks>
        Function CheckPremiumAndRedirect() As String
            Dim sReturnUrl As String = String.Empty
            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            Dim dTatalPremium As Decimal
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            Dim sIsPrepaymentOptionEnabled As String
            sIsPrepaymentOptionEnabled = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPrepaymentOptionEnabled, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            If sIsPrepaymentOptionEnabled = "" Then
                sIsPrepaymentOptionEnabled = Nothing
            End If

            If oQuote.Risks.Count > 0 Then
                dTatalPremium = oQuote.GrossTotal
            End If
            If dTatalPremium <= 0.0 AndAlso Current.Session(CNMTAType) IsNot Nothing Then
                'In case of MTA
                If dTatalPremium <= 0.0 AndAlso Current.Session(CNMTAType) = MTAType.CANCELLATION Then
                    'if this is Refund Premium or Zero premium and PrePayment = 0 then go to directly TransactionConfirmation page
                    If sIsPrepaymentOptionEnabled Is Nothing Or (sIsPrepaymentOptionEnabled IsNot Nothing AndAlso sIsPrepaymentOptionEnabled = "0") Then
                        Current.Session(CNPaid) = True
                        sReturnUrl = "~/secure/TransactionConfirmation.aspx"
                    ElseIf sIsPrepaymentOptionEnabled IsNot Nothing AndAlso sIsPrepaymentOptionEnabled = "1" Then
                        'this will simply redirect to the PrePayment page in case for Refund Premium.
                        'if this is Refund Premium and PrePayment = 1 then go to PrePayment page and select account
                        sReturnUrl = "~/secure/payment/PrePayment.aspx"
                    End If
                ElseIf Not Current.Session(CNMTAType) Is Nothing And dTatalPremium = 0.0 Then
                    'this will simply redirect to the Transaction Confirmation in case when there is Return Premium
                    'Or Premium equal to Zero in case of MTA Permanent
                    Current.Session(CNPaid) = True
                    sReturnUrl = "~/secure/TransactionConfirmation.aspx"
                ElseIf Not Current.Session(CNMTAType) Is Nothing And dTatalPremium < 0.0 Then
                    'if this is Refund Premium and PrePayment = 0 then go to directly TransactionConfirmation page
                    If sIsPrepaymentOptionEnabled Is Nothing Or (sIsPrepaymentOptionEnabled IsNot Nothing AndAlso sIsPrepaymentOptionEnabled = "0") Then
                        Current.Session(CNPaid) = True
                        sReturnUrl = "~/secure/TransactionConfirmation.aspx"
                    ElseIf sIsPrepaymentOptionEnabled IsNot Nothing AndAlso sIsPrepaymentOptionEnabled = "1" Then
                        'this will simply redirect to the PrePayment page in case for Refund Premium.
                        'if this is Refund Premium and PrePayment = 1 then go to PrePayment page and select account

                        sReturnUrl = "~/secure/payment/PrePayment.aspx"
                    End If
                Else
                    'if premium is in positive
                    Current.Session(CNStatementsAgreed) = True
                    If Current.Session(CNSelectedPaymentIndex) IsNot Nothing Then
                        sReturnUrl = GetPaymentPageUrl()
                    Else
                        sReturnUrl = "~/secure/payment/PaymentSelect.aspx"
                    End If
                End If

            Else
                'in case of NB/Renewal
                Current.Session(CNStatementsAgreed) = True
                If Current.Session(CNSelectedPaymentIndex) IsNot Nothing Then
                    sReturnUrl = GetPaymentPageUrl()
                Else
                    sReturnUrl = "~/secure/payment/PaymentSelect.aspx"
                End If
            End If

            Return sReturnUrl
        End Function
        ''' <summary>
        ''' this function reture the payment URL
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPaymentPageUrl() As String
            Dim sReturnUrl As String = String.Empty
            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            Dim oPaymentOptions As Config.PaymentTypes = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).PaymentTypes
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPaymentType As Config.PaymentType = oPaymentOptions.PaymentType(Current.Session(CNSelectedPaymentIndex))
            Dim sMarkedForCollectionProductOption As String
            sMarkedForCollectionProductOption = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 87).OptionValue

            'code for Marked Collection and redirection
            If ((sMarkedForCollectionProductOption IsNot Nothing AndAlso sMarkedForCollectionProductOption = "1") And Current.Session(CNTotalForQuoteCollection) IsNot Nothing) And oPaymentType.Name.Trim.ToUpper <> "PAYNOW" And oPaymentType.Name.ToUpper <> "BANKGUARANTEE" _
            And oPaymentType.Name.Trim.ToUpper <> "DIRECT DEBIT" And oPaymentType.Name.Trim.ToUpper <> "CREDIT CARD" Then
                sReturnUrl = "~/secure/payment/PrePayment.aspx?quotecollection=true"
            ElseIf Current.Session(CNTotalForQuoteCollection) IsNot Nothing Then
                Current.Session.Remove(CNCashListItem) 'Loads Cash List screen when PayNow option selection
                sReturnUrl = oPaymentType.Url & "?quotecollection=true"
            ElseIf (sMarkedForCollectionProductOption IsNot Nothing AndAlso sMarkedForCollectionProductOption = "1") And oPaymentType.Name.Trim.ToUpper <> "PAYNOW" And oPaymentType.Name.ToUpper <> "BANKGUARANTEE" And oPaymentType.Name.ToUpper <> "CASHDEPOSIT" _
            And oPaymentType.Name.Trim.ToUpper <> "DIRECT DEBIT" And oPaymentType.Name.Trim.ToUpper <> "CREDIT CARD" Then
                sReturnUrl = "~/secure/payment/PrePayment.aspx"
            ElseIf oPaymentType.Type.Trim.ToUpper = "PAYMENTHUB" Then
                Dim oPaymentHubDetails As New NexusProvider.PaymentHubDetails
                oPaymentHubDetails.RequestType = PaymentHub.RequestType.Payment
                oPaymentHubDetails.TransactionAmount = oQuote.GrossTotal
                Current.Session(CNPaymentHubDetails) = oPaymentHubDetails
                sReturnUrl = GetPaymentHubPageURL()
            Else
                Current.Session.Remove(CNCashListItem) 'Loads Cash List screen when PayNow option selection
                If oPaymentType.Type.Trim.ToUpper = "PREMIUMFINANCE" Then
                    Dim PaymentCollectionUrl As String = oPaymentType.PaymentCollectionUrl

                    'set appropriate session values here to indicate payment taken and then redirect to end page
                    If PaymentCollectionUrl <> "" Then
                        If oQuote.InstDepositAmount > 0 AndAlso oQuote.DepositTransactasInstalment = False Then
                            sReturnUrl = PaymentCollectionUrl
                        Else
                            Current.Session(CNPaid) = True
                            sReturnUrl = "~/secure/TransactionConfirmation.aspx"
                        End If
                    Else
                        Current.Session(CNPaid) = True
                        sReturnUrl = "~/secure/TransactionConfirmation.aspx"
                    End If
                Else
                    sReturnUrl = oPaymentType.Url
                End If
            End If

           
            oQuote = Nothing
            oWebService = Nothing
            oPaymentOptions = Nothing
            oPaymentType = Nothing
            Return sReturnUrl
        End Function
        ''' <summary>
        ''' This Method check product risk option setting named Multiple Claims Payments and return boolean value in the form unauthorised or authorised
        ''' </summary>
        ''' <remarks></remarks>
        Public Function AllowMultipleClaimPayment() As Boolean
            Dim bMatch As Boolean = True
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Current.Session(CNClaimQuote)
            Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Current.Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oClaimVersions As NexusProvider.VersionsCollections = Nothing
            Dim oCashListItem As NexusProvider.CashListItemsCollection = Nothing
            Dim iHighest As Integer = 0
            If Current.Session(CNClaimVersion) IsNot Nothing Then
                oClaimVersions = Current.Session(CNClaimVersion)
            Else
                Try
                    oClaimVersions = oWebservice.GetVersionsForClaim(oOpenClaim.ClaimNumber)
                Finally

                End Try
            End If

            For iCount As Integer = 0 To oClaimVersions.Count - 1
                If oClaimVersions(iCount).Version > iHighest Then
                    iHighest = oClaimVersions(iCount).Version
                End If
            Next
            For iCount As Integer = 0 To oClaimVersions.Count - 1
                If oClaimVersions(iCount).Version = iHighest Then

                    Dim sMultipleClaimsPayments As String
                    Dim dMaxUnauthorisedClaimValue As Double
                    Dim iMaxUnauthorisedNoClaimPayments As Integer
                    Dim CashListItem As New NexusProvider.CashListItems
                    Dim dTotalPaymentAmount As Double
                    Dim CountOfClaims As Integer

                    'get the Product Risk option setting named Multiple Claims Payments(MultipleClaimsPayments)
                    sMultipleClaimsPayments = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.MultipleClaimsPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                    'get the Product Risk option setting named max unauthorised claim Value(MaxUnauthorisedClaimValue)
                    dMaxUnauthorisedClaimValue = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.MaxUnauthorisedClaimValue, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
                    'get the Product Risk option setting named max unauthorised number of claim payamnt(MaxUnauthorisedNoClaimPayments)
                    iMaxUnauthorisedNoClaimPayments = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.MaxUnauthorisedNoClaimPayments, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)

                    With CashListItem
                        .ClaimNumber = CType(Current.Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimNumber
                    End With
                    oCashListItem = oWebservice.GetReferredPayments(CashListItem)
                    If sMultipleClaimsPayments = "1" Then
                        Current.Session(CNMaxClaimPaymentValue) = dMaxUnauthorisedClaimValue '- dTotalPaymentAmount
                    End If

                    For Each oCashList As NexusProvider.CashListItems In oCashListItem
                        If sMultipleClaimsPayments = "1" Then
                            If oClaimVersions(iCount).ClaimNumber = oCashList.ClaimNumber Then
                                dTotalPaymentAmount = oCashList.PaymentAmount + dTotalPaymentAmount
                                ' Count the Number of oCashList and add it up in CountOfClaims and check the same against MaxUnauthorisedNoClaimPayments
                                CountOfClaims = CountOfClaims + 1
                                If iMaxUnauthorisedNoClaimPayments <= CountOfClaims Or dMaxUnauthorisedClaimValue <= dTotalPaymentAmount Then
                                    bMatch = False
                                    If dMaxUnauthorisedClaimValue <= dTotalPaymentAmount Then
                                        Current.Session(CNMAXUNAUTHORISEDCLAIMVALUE) = True
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            Next
            Return bMatch
        End Function

        Sub GetClaimDetails(ByVal v_iClaimKey As Integer, Optional ByVal oClaimRisk As NexusProvider.ClaimRisk = Nothing, Optional ByVal v_iFetchAllVersionAmounts As Integer = 0)
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Current.Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oQuote As NexusProvider.Quote = Current.Session(CNClaimQuote)
            Dim sBranchCode As String
            If oQuote IsNot Nothing Then
                sBranchCode = oQuote.BranchCode
            End If
            If oOriginalClaim Is Nothing Then
                oOriginalClaim = New NexusProvider.ClaimOpen
            End If
            'Retreiving the latest details
            'arch issue 268
            oClaimDetails = GetClaimDetailsCall(v_iClaimKey, sBranchCode, v_iFetchAllVersionAmounts)
            'updation of latest session values 
            Current.Session.Item(CNClaimTimeStamp) = oClaimDetails.TimeStamp
            Current.Session.Item(CNBaseClaimKey) = oClaimDetails.BaseClaimKey
            Current.Session.Item(CNClaimKey) = oClaimDetails.ClaimKey
            Current.Session.Item(CNClaimNumber) = oClaimDetails.ClaimNumber

            If oClaimRisk IsNot Nothing Then
                Current.Session.Item(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
                Current.Session.Item(CNDataSet) = oClaimRisk.XMLDataSet
            End If

            With oClaimDetails
                oOriginalClaim.ClaimPayment = .ClaimPayment
                oOriginalClaim.CatastropheCode = .CatastropheCode
                oOriginalClaim.BaseCaseKey = .BaseCaseKey
                oOriginalClaim.BaseClaimKey = .BaseClaimKey
                oOriginalClaim.Claim = .Claim
                oOriginalClaim.ClaimCoInsurer = .ClaimCoInsurer
                oOriginalClaim.ClaimDescription = .ClaimDescription
                oOriginalClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                oOriginalClaim.ClaimKey = .ClaimKey
                oOriginalClaim.ClaimNumber = .ClaimNumber
                oOriginalClaim.ClaimPeril = .ClaimPeril
                oOriginalClaim.ClaimStatus = .ClaimStatus
                oOriginalClaim.ClaimStatusDate = .ClaimStatusDate
                oOriginalClaim.ClaimStatusID = .ClaimStatusID
                oOriginalClaim.ClaimVersion = .ClaimVersion
                oOriginalClaim.ClaimVersionDescription = .ClaimVersionDescription
                oOriginalClaim.IsRecovery = .IsRecovery
                oOriginalClaim.ClientClaimNumber = .ClientClaimNumber
                oOriginalClaim.ClientEmail = .ClientEmail
                oOriginalClaim.ClientFaxNo = .ClientFaxNo
                oOriginalClaim.ClientMobileNo = .ClientMobileNo
                oOriginalClaim.ClientName = .ClientName
                oOriginalClaim.TPA = .TPA 'WPR08-TPA Filtering

                If String.IsNullOrEmpty(.ClientShortName) = True Then
                    Dim oClaimVersions As NexusProvider.VersionsCollections = Current.Session(CNClaimVersion)
                    If oClaimVersions IsNot Nothing AndAlso oClaimVersions.Count > 0 Then
                        oOriginalClaim.ClientShortName = oClaimVersions(0).ClientShortName
                    ElseIf Current.Session(CNParty) IsNot Nothing Then
                        Dim oParty As NexusProvider.BaseParty = Current.Session(CNParty)
                        Select Case True
                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                With CType(oParty, NexusProvider.CorporateParty)
                                    oOriginalClaim.ClientShortName = .ClientSharedData.ShortName.Trim()
                                End With
                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                With CType(oParty, NexusProvider.PersonalParty)
                                    oOriginalClaim.ClientShortName = .ClientSharedData.ShortName.Trim()
                                End With
                        End Select
                    ElseIf oClaimVersions Is Nothing Then
                        oClaimVersions = oWebservice.GetVersionsForClaim(.ClaimNumber)
                        If oClaimVersions IsNot Nothing AndAlso oClaimVersions.Count > 0 Then
                            oOriginalClaim.ClientShortName = oClaimVersions(0).ClientShortName.Trim()
                        End If
                    End If
                Else
                    oOriginalClaim.ClientShortName = .ClientShortName
                End If

                oOriginalClaim.ClientTelNo = .ClientTelNo
                oOriginalClaim.ClientTelNoOff = .ClientTelNoOff
                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                oOriginalClaim.Comments = .Comments
                oOriginalClaim.Contact = .Contact
                oOriginalClaim.CurrencyISOCode = .CurrencyISOCode
                oOriginalClaim.Description = .Description
                oOriginalClaim.ExternalHandler = .ExternalHandler
                oOriginalClaim.HandlerCode = .HandlerCode
                oOriginalClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                oOriginalClaim.InfoOnly = .InfoOnly
                oOriginalClaim.InsuranceFileKey = .InsuranceFileKey
                oOriginalClaim.InsuranceFolderKey = .InsuranceFolderKey
                oOriginalClaim.InsuranceRef = .InsuranceRef
                oOriginalClaim.InsurerClaimNumber = .InsurerClaimNumber
                oOriginalClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                oOriginalClaim.IsDeleted = .IsDeleted
                oOriginalClaim.LastModifiedDate = .LastModifiedDate
                oOriginalClaim.LikelyClaim = .LikelyClaim
                oOriginalClaim.Location = .Location
                oOriginalClaim.LossDate = .LossDate
                oOriginalClaim.LossDateFrom = .LossDateFrom
                oOriginalClaim.LossFromDate = .LossFromDate
                oOriginalClaim.LossToDate = .LossToDate
                oOriginalClaim.LossToDateSpecified = .LossToDateSpecified
                oOriginalClaim.Payments = .Payments
                oOriginalClaim.PolicyNumber = .PolicyNumber
                oOriginalClaim.PolicyType = .PolicyType
                oOriginalClaim.PrimaryCause = .PrimaryCause
                oOriginalClaim.PrimaryCauseCode = .PrimaryCauseCode
                oOriginalClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                oOriginalClaim.ProductDescription = .ProductDescription
                oOriginalClaim.ProgressStatusCode = .ProgressStatusCode
                oOriginalClaim.ProgressStatusDescription = .ProgressStatusDescription
                oOriginalClaim.ReportedDate = .ReportedDate
                oOriginalClaim.RiskKey = .RiskKey
                oOriginalClaim.SecondaryCause = .SecondaryCause
                oOriginalClaim.SecondaryCauseCode = .SecondaryCauseCode
                oOriginalClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                oOriginalClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                oOriginalClaim.TotalShare = .TotalShare
                oOriginalClaim.Town = .Town
                oOriginalClaim.TownCode = .TownCode
                oOriginalClaim.UnderwritingYearCode = .UnderwritingYearCode
                oOriginalClaim.UserDefFldACode = .UserDefFldACode
                oOriginalClaim.UserDefFldBCode = .UserDefFldBCode
                oOriginalClaim.UserDefFldCCode = .UserDefFldCCode
                oOriginalClaim.UserDefFldDCode = .UserDefFldECode
                oOriginalClaim.UserDefFldECode = .UserDefFldECode
                oOriginalClaim.IsPolicyOutstanding = .IsPolicyOutstanding
                oOriginalClaim.CoinsuranceTreatmentCode = .CoinsuranceTreatmentCode

                'Added for Insurer
                oOriginalClaim.Insurer = .Insurer
                Current.Session.Item(CNClaimTimeStamp) = .TimeStamp
                oOriginalClaim.CurrencyISOCode = .CurrencyCode
                Current.Session.Item(CNCurrenyCode) = Trim(.CurrencyCode) 'Changed
                oOriginalClaim.Client = .Client
                'this needs to be removed after SAM issue is resolved
                If oOriginalClaim.Client.PartyKey = 0 AndAlso oQuote IsNot Nothing Then
                    oOriginalClaim.Client.PartyKey = oQuote.PartyKey
                End If
                'Session(CNInsurer_Header) = .ClientName
                Current.Session(CNClaimNumber) = .ClaimNumber
                Current.Session(CNStatus) = .ClaimStatus
            End With
            Current.Session.Item(CNClaim) = oOriginalClaim
        End Sub
        ''' <summary>
        ''' This method retreives the latest claim version details and populate into claim session variables
        ''' </summary>
        ''' <remarks></remarks>
        Sub GetLatestDetails(Optional ByVal bClearSession As Boolean = True, Optional ByVal bExclusiveLock As Boolean = False)
            'Latest Claim Details
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimVersions As NexusProvider.VersionsCollections
            Dim iHighest As Integer = 0
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oUserDetails As NexusProvider.UserDetails = CType(Current.Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim oOpenClaim As New NexusProvider.ClaimOpen
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
            Dim oQuote As NexusProvider.Quote = Current.Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode

            oClaimVersions = oWebservice.GetVersionsForClaim(Current.Session(CNClaimNumber), sBranchCode)

            If bClearSession = True Then
                'Clear the session Variable
                ClearClaims()
            End If

            If oClaimVersions IsNot Nothing Then
                'Find Highest Version
                For iCount As Integer = 0 To oClaimVersions.Count - 1
                    If oClaimVersions(iCount).Version > iHighest Then
                        iHighest = oClaimVersions(iCount).Version
                    End If
                Next
            End If
            For iCount As Integer = 0 To oClaimVersions.Count - 1
                If oClaimVersions(iCount).Version = iHighest Then

                    'arch issue 268
                    oClaimDetails = GetClaimDetailsCall(v_iClaimKey:=oClaimVersions(iCount).ClaimKey, v_sBranchCode:=sBranchCode, bExclusiveLock:=bExclusiveLock)

                    With oClaimDetails
                        oOpenClaim.CatastropheCode = .CatastropheCode
                        oOpenClaim.BaseClaimKey = .BaseClaimKey
                        oOpenClaim.BaseCaseKey = .BaseCaseKey
                        oOpenClaim.Claim = .Claim
                        oOpenClaim.ClaimCoInsurer = .ClaimCoInsurer
                        oOpenClaim.ClaimDescription = .ClaimDescription
                        oOpenClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                        oOpenClaim.ClaimKey = .ClaimKey
                        oOpenClaim.ClaimNumber = .ClaimNumber
                        oOpenClaim.ClaimPeril = .ClaimPeril
                        oOpenClaim.ClaimStatus = .ClaimStatus
                        oOpenClaim.ClaimStatusDate = .ClaimStatusDate
                        oOpenClaim.ClaimStatusID = .ClaimStatusID
                        oOpenClaim.ClaimVersion = .ClaimVersion
                        oOpenClaim.ClaimVersionDescription = .ClaimVersionDescription
                        oOpenClaim.ClientClaimNumber = .ClientClaimNumber
                        oOpenClaim.ClientEmail = .ClientEmail
                        oOpenClaim.ClientFaxNo = .ClientFaxNo
                        oOpenClaim.ClientMobileNo = .ClientMobileNo
                        oOpenClaim.ClientName = .ClientName
                        oOpenClaim.ClientShortName = oClaimVersions(0).ClientShortName 'IIf(.ClientShortName <> String.Empty, .ClientShortName, Trim(lblClientCode.Text))
                        oOpenClaim.ClientTelNo = .ClientTelNo
                        oOpenClaim.ClientTelNoOff = .ClientTelNoOff
                        oOpenClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                        oOpenClaim.Comments = .Comments
                        oOpenClaim.Contact = .Contact
                        oOpenClaim.CurrencyISOCode = .CurrencyCode
                        oOpenClaim.Description = .Description
                        oOpenClaim.ExternalHandler = .ExternalHandler
                        oOpenClaim.HandlerCode = .HandlerCode
                        oOpenClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                        oOpenClaim.InfoOnly = .InfoOnly
                        oOpenClaim.InsuranceFileKey = .InsuranceFileKey
                        oOpenClaim.InsuranceRef = .InsuranceRef
                        oOpenClaim.InsurerClaimNumber = .InsurerClaimNumber
                        oOpenClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                        oOpenClaim.IsDeleted = .IsDeleted
                        oOpenClaim.LastModifiedDate = .LastModifiedDate
                        oOpenClaim.LikelyClaim = .LikelyClaim
                        oOpenClaim.Location = .Location
                        oOpenClaim.LossDate = .LossDate
                        oOpenClaim.LossDateFrom = .LossDateFrom
                        oOpenClaim.LossFromDate = .LossToDate
                        oOpenClaim.LossToDate = .LossToDate
                        oOpenClaim.LossToDateSpecified = .LossToDateSpecified
                        oOpenClaim.Payments = .Payments
                        oOpenClaim.PolicyNumber = .PolicyNumber
                        oOpenClaim.PolicyType = .PolicyType
                        oOpenClaim.PrimaryCause = .PrimaryCause
                        oOpenClaim.PrimaryCauseCode = .PrimaryCauseCode
                        oOpenClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                        oOpenClaim.ProductDescription = .ProductDescription
                        oOpenClaim.ProgressStatusCode = .ProgressStatusCode
                        oOpenClaim.ProgressStatusDescription = .ProgressStatusDescription
                        oOpenClaim.ReportedDate = .ReportedDate
                        oOpenClaim.Reserve = .Reserve
                        oOpenClaim.RiskKey = .RiskKey
                        oOpenClaim.RiskType = CType(Current.Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).RiskTypeCode
                        oOpenClaim.RiskTypeDescription = CType(Current.Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).Description
                        oOpenClaim.SecondaryCause = .SecondaryCause
                        oOpenClaim.SecondaryCauseCode = .SecondaryCauseCode
                        oOpenClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                        oOpenClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                        oOpenClaim.TotalShare = .TotalShare
                        oOpenClaim.Town = .Town
                        oOpenClaim.TownCode = .TownCode
                        oOpenClaim.UnderwritingYearCode = .UnderwritingYearCode
                        oOpenClaim.UserDefFldACode = .UserDefFldACode
                        oOpenClaim.UserDefFldBCode = .UserDefFldBCode
                        oOpenClaim.UserDefFldCCode = .UserDefFldCCode
                        oOpenClaim.UserDefFldDCode = .UserDefFldECode
                        oOpenClaim.UserDefFldECode = .UserDefFldECode
                        'Added for Insurer
                        oOpenClaim.Insurer = .Insurer
                        Current.Session.Item(CNClaimTimeStamp) = .TimeStamp
                        oOpenClaim.CurrencyISOCode = .CurrencyCode
                        Current.Session.Item(CNCurrenyCode) = Trim(.CurrencyCode) 'Changed
                        oOpenClaim.Client = .Client
                        'Session(CNInsurer_Header) = .ClientName
                        Current.Session(CNClaimNumber) = .ClaimNumber
                        Current.Session(CNStatus) = .ClaimStatus

                        Current.Session(CNClaim) = oOpenClaim
                        If Current.Session(CNMode) = Mode.PayClaim Or Current.Session(CNMode) = Mode.ViewClaim Then
                            Dim oOptionType As New NexusProvider.OptionTypeSetting
                            oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
                            If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
AndAlso oOptionType.OptionValue = "1" Then
                                CheckClaimBuilder()
                                'Retreival of the risk related values 
                                'Arch issue 268
                                oClaimRisk = GetClaimRiskCall(.BaseClaimKey, .ClaimKey, sBranchCode)
                                Current.Session(CNDataSet) = oClaimRisk.XMLDataSet
                                Current.Session(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
                            End If
                        End If
                        If Current.Session(CNMode) = Mode.PayClaim Then
                            If Current.Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                                Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
                                For iPeril As Integer = 0 To oOpenClaim.ClaimPeril.Count - 1
                                    If oOpenClaim.ClaimPeril(iPeril).Reserve.Count > 0 Then
                                        PerilsIndex.Add(iPeril)
                                        Exit For
                                    End If
                                Next
                                Current.Session(CNClaimMultiPerilIndex) = PerilsIndex
                            Else
                                For iPeril As Integer = 0 To oOpenClaim.ClaimPeril.Count - 1
                                    If oOpenClaim.ClaimPeril(iPeril).Reserve.Count > 0 Then
                                        Current.Session(CNClaimPerilIndex) = iPeril
                                        Exit For
                                    End If
                                Next
                            End If

                        End If
                    End With
                End If
            Next
            Current.Session(CNClaim) = oOpenClaim
        End Sub

        ''' <summary>
        ''' This Methof executes the PayClaim method with zero amount
        ''' </summary>
        ''' <remarks></remarks>
        Sub PayClaimWithZeroAmount()
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Current.Session(CNClaim), NexusProvider.ClaimOpen)
            Dim iPeril As Integer = 0
            Dim PerilsIndex As New System.Collections.Generic.List(Of Integer)
            'Set the correct perilindex key
            If Current.Session(CNClaimPerilIndex) IsNot Nothing Then
                iPeril = CInt(Current.Session(CNClaimPerilIndex))
            Else
                If Current.Session(CNClaimMultiPerilIndex) IsNot Nothing Then

                    For iCount As Integer = 0 To oClaimOpen.ClaimPeril.Count - 1
                        If oClaimOpen.ClaimPeril(iCount).Reserve.Count > 0 Then
                            PerilsIndex.Add(iCount)
                            Exit For
                        End If
                    Next
                    Current.Session(CNClaimMultiPerilIndex) = PerilsIndex
                Else
                    For iCount As Integer = 0 To oClaimOpen.ClaimPeril.Count - 1
                        If oClaimOpen.ClaimPeril(iCount).Reserve.Count > 0 Then
                            iPeril = iCount
                            Current.Session(CNClaimPerilIndex) = iCount
                            Exit For
                        End If
                    Next
                End If
            End If
            'Update the payments through UpdateClaimReserveOrPayments
            Dim oPayClaimResponse As NexusProvider.PayClaimResponse = Nothing
            Dim oPayment As New NexusProvider.ClaimPayment
            Dim oPaymentCollection As New NexusProvider.ClaimPaymentCollection
            Dim bTimeStamp As Byte() = CType(Current.Session(CNClaimTimeStamp), Byte())
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
            Dim oQuote As NexusProvider.Quote = Current.Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode

            If Current.Session(CNClaimMultiPerilIndex) IsNot Nothing Then
                PerilsIndex = Current.Session(CNClaimMultiPerilIndex)
                For Each PerilItemIndex As Integer In PerilsIndex
                    oPayment = CType(Current.Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Current.Session(CNClaimPerilIndex)).Payment
                    oClaimOpen.ClaimPeril(iPeril).Payment.BaseClaimKey = oClaimOpen.BaseClaimKey
                    oClaimOpen.ClaimPeril(iPeril).Payment.BaseClaimPerilKey = oClaimOpen.ClaimPeril(iPeril).BaseClaimPerilKey

                    oPayment.ClaimVersionDescription = Current.Session(CNChangeReason)

                    'To Skip the posting
                    oPayment.PaymentOnly = True

                    For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem.Count - 1
                        oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem.RemoveAt(0)
                    Next

                    Dim oClaimPaymentItem As NexusProvider.ClaimPaymentItemType
                    For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).Reserve.Count - 1
                        If oClaimOpen.ClaimPeril(iPeril).Reserve(iCount).BaseReserveKey <> 0 Then
                            oClaimPaymentItem = New NexusProvider.ClaimPaymentItemType
                            oClaimPaymentItem.BaseReserveKey = oClaimOpen.ClaimPeril(iPeril).Reserve(iCount).BaseReserveKey
                            oClaimPaymentItem.CurrencyCode = Current.Session(CNCurrenyCode)
                            oClaimPaymentItem.CurrencyRate = 1

                            oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem.Add(oClaimPaymentItem)
                        End If
                    Next

                    If oClaimOpen.ClaimPeril(iPeril).Reserve.Count > 0 Then
                        With oClaimOpen.ClaimPeril(iPeril)
                            oClaimOpen.ClaimPeril(iPeril).Payment.CurrencyCode = Current.Session(CNCurrenyCode)
                            oClaimOpen.ClaimPeril(iPeril).Payment.CurrencyRate = 1
                            oClaimOpen.ClaimPeril(iPeril).Payment.PaymentAmount = 0
                            oClaimOpen.ClaimPeril(iPeril).Payment.RiskType = CType(Current.Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).Payment.RiskType
                            oClaimOpen.ClaimPeril(iPeril).Payment.ReserveType = .Reserve(0).TypeCode
                            oClaimOpen.ClaimPeril(iPeril).Payment.TaxAmount = 0
                            oClaimOpen.ClaimPeril(iPeril).Payment.CurrencyCode = Current.Session(CNCurrenyCode)

                        End With
                    End If
                    oPayment.ClaimPaymentItem = oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem
                    oPaymentCollection.Add(oPayment)
                Next

                'Arch issue 268
                'oPayClaimResponse = oWebservice.PayClaim(oPayment, bTimeStamp, sBranchCode)
                oPayClaimResponse = PayClaimCall(Nothing, bTimeStamp, sBranchCode, oPaymentCollection)
                If oPayClaimResponse Is Nothing Then
                    Exit Sub
                End If
            Else
                oPayment = CType(Current.Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Current.Session(CNClaimPerilIndex)).Payment
                oClaimOpen.ClaimPeril(iPeril).Payment.BaseClaimKey = oClaimOpen.BaseClaimKey
                oClaimOpen.ClaimPeril(iPeril).Payment.BaseClaimPerilKey = oClaimOpen.ClaimPeril(iPeril).BaseClaimPerilKey

                oPayment.ClaimVersionDescription = Current.Session(CNChangeReason)

                'To Skip the posting
                oPayment.PaymentOnly = True

                For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem.Count - 1
                    oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem.RemoveAt(0)
                Next

                Dim oClaimPaymentItem As NexusProvider.ClaimPaymentItemType
                For iCount As Integer = 0 To oClaimOpen.ClaimPeril(iPeril).Reserve.Count - 1
                    If oClaimOpen.ClaimPeril(iPeril).Reserve(iCount).BaseReserveKey <> 0 Then
                        oClaimPaymentItem = New NexusProvider.ClaimPaymentItemType
                        oClaimPaymentItem.BaseReserveKey = oClaimOpen.ClaimPeril(iPeril).Reserve(iCount).BaseReserveKey
                        oClaimPaymentItem.CurrencyCode = Current.Session(CNCurrenyCode)
                        oClaimPaymentItem.CurrencyRate = 1

                        oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem.Add(oClaimPaymentItem)
                    End If
                Next

                If oClaimOpen.ClaimPeril(iPeril).Reserve.Count > 0 Then
                    With oClaimOpen.ClaimPeril(iPeril)
                        oClaimOpen.ClaimPeril(iPeril).Payment.CurrencyCode = Current.Session(CNCurrenyCode)
                        oClaimOpen.ClaimPeril(iPeril).Payment.CurrencyRate = 1
                        oClaimOpen.ClaimPeril(iPeril).Payment.PaymentAmount = 0
                        oClaimOpen.ClaimPeril(iPeril).Payment.RiskType = CType(Current.Session.Item(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).Payment.RiskType
                        oClaimOpen.ClaimPeril(iPeril).Payment.ReserveType = .Reserve(0).TypeCode
                        oClaimOpen.ClaimPeril(iPeril).Payment.TaxAmount = 0
                        oClaimOpen.ClaimPeril(iPeril).Payment.CurrencyCode = Current.Session(CNCurrenyCode)

                    End With
                End If
                oPayment.ClaimPaymentItem = oClaimOpen.ClaimPeril(iPeril).Payment.ClaimPaymentItem
                'Arch issue 268
                'oPayClaimResponse = oWebservice.PayClaim(oPayment, bTimeStamp, sBranchCode)
                oPayClaimResponse = PayClaimCall(oPayment, bTimeStamp, sBranchCode)
                If oPayClaimResponse Is Nothing Then
                    Exit Sub
                End If
            End If
            'Calling of Latest Claim Details after Maintain Claim
            GetClaimDetails(v_iClaimKey:=oPayClaimResponse.ClaimKey, v_iFetchAllVersionAmounts:=1)

            'Updation of latest claim session variables
            'take the latest claim information into Session
            oClaimOpen = Current.Session(CNClaim)

            'check the claim builder hidden product option
            Dim oOptionType As New NexusProvider.OptionTypeSetting
            oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
            If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
AndAlso oOptionType.OptionValue = "1" Then
                CheckClaimBuilder()
                'Calling of the risk realted values after maintain claim
                'Arch issue 268
                oClaimRisk = GetClaimRiskCall(oClaimOpen.BaseClaimKey, oClaimOpen.ClaimKey, sBranchCode)

                Current.Session.Item(CNDataSet) = oClaimRisk.XMLDataSet
                Current.Session(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
            End If

        End Sub


#Region "WPR 73_74 Changes"
        ''' <summary>
        ''' This Method creates a background task for the work manager
        ''' </summary>
        ''' <param name="oQuote"></param>
        ''' <param name="v_sTask"></param>
        ''' <param name="v_sTaskDescription"></param>
        ''' <param name="v_sTaskGroup"></param>
        ''' <remarks></remarks>
        Sub CreateTask(ByVal oQuote As NexusProvider.Quote, ByVal v_sTaskDescription As String, ByVal v_sTask As String, ByVal v_sTaskGroup As String, Optional ByVal v_sAllocationUserGroup As String = "UA")
            'Create the work manager task by passing following details
            Dim oWorkManager As New NexusProvider.WorkManager
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oWorkManager.DueDate = Now
            oWorkManager.Client = oQuote.InsuredName
            oWorkManager.Description = v_sTaskDescription
            oWorkManager.AllocationUser = oQuote.ContactUserName
            oWorkManager.AllocationUserGroup = v_sAllocationUserGroup
            oWorkManager.IsUrgent = True
            oWorkManager.IsUrgentForUpdate = 1
            oWorkManager.IsComplete = False
            oWorkManager.IsTaskReview = True
            oWorkManager.Task = v_sTask
            oWorkManager.TaskGroup = v_sTaskGroup
            If HttpContext.Current.Session(CNMTAType) IsNot Nothing Then 'check if user is doing MTA
                Dim oWmrk As New NexusProvider.KeyData
                oWmrk.KeyName = "MtaType"
                oWmrk.KeyValue = HttpContext.Current.Session(CNMTAType)
                oWorkManager.KeyData.Add(oWmrk)
            Else
                Dim oWmrk As New NexusProvider.KeyData
                oWmrk.KeyName = "MtaType"
                oWmrk.KeyValue = "None"
                oWorkManager.KeyData.Add(oWmrk)
            End If

            If HttpContext.Current.Session(CNQuote) IsNot Nothing Then
                Dim oWmrk As New NexusProvider.KeyData
                oWmrk.KeyName = "InsuranceFileKey"
                oWmrk.KeyValue = CType(HttpContext.Current.Session(CNQuote), NexusProvider.Quote).InsuranceFileKey
                oWorkManager.KeyData.Add(oWmrk)
            End If
            If HttpContext.Current.Session(CNParty) IsNot Nothing Then
                Dim oWmrk As New NexusProvider.KeyData
                oWmrk.KeyName = "PartyKey"
                oWmrk.KeyValue = CType(HttpContext.Current.Session(CNParty), NexusProvider.BaseParty).Key
                oWorkManager.KeyData.Add(oWmrk)
            End If
            If oWorkManager.TaskGroup IsNot Nothing Then
                oWorkManager.LockName = NexusProvider.SAMForInsurance.PureService.TaskLockName.InvalidValue
                oWebService.CreateWmTask(oWorkManager)
            End If
        End Sub


#End Region


        ''' <summary>
        ''' This Method executes the OpenClaim and take care session timestamp and record locking messages as well
        ''' </summary>
        ''' <remarks></remarks>
        Public Function OpenClaimCall(ByVal v_oClaimOpen As NexusProvider.Claim,
                                            Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.ClaimResponse

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing

            Try

                oClaimResponse = oWebservice.OpenClaim(v_oClaimOpen, v_sBranchCode)
                Current.Session(CNClaimCallsTimeStamp) = oClaimResponse.TimeStamp
            Catch ex As NexusProvider.NexusException
                'Show the Error message if record locked
                HasLockedByAnotherUser(ex)
            End Try
            Return oClaimResponse

        End Function
        ''' <summary>
        ''' This Method executes the MaintainClaim and take care session timestamp and record locking messages as well
        ''' </summary>
        ''' <remarks></remarks>
        Public Function MaintainClaimCall(ByVal v_oClaimMaintain As NexusProvider.ClaimOpen,
                                            ByVal v_bTimeStamp As Byte(),
                                            Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.ClaimResponse

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing

            Try
                If Current.Session(CNClaimCallsTimeStamp) IsNot Nothing Then
                    v_bTimeStamp = Current.Session(CNClaimCallsTimeStamp)
                End If
                oClaimResponse = oWebservice.MaintainClaim(v_oClaimMaintain, v_bTimeStamp, v_sBranchCode)
                Current.Session(CNClaimCallsTimeStamp) = oClaimResponse.TimeStamp
            Catch ex As NexusProvider.NexusException
                'Show the Error message if record locked
                HasLockedByAnotherUser(ex)
            End Try
            Return oClaimResponse

        End Function
        ''' <summary>
        ''' This Method executes the PayClaim and take care session timestamp and record locking messages as well
        ''' </summary>
        ''' <remarks></remarks>
        Public Function PayClaimCall(ByVal v_oClaimPayment As NexusProvider.ClaimPayment,
                                    ByVal v_bTimeStamp As Byte(),
                                    Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal oClaimPaymentCollection As NexusProvider.ClaimPaymentCollection = Nothing) As NexusProvider.PayClaimResponse

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPayClaimResponse As NexusProvider.PayClaimResponse = Nothing

            Try
                If Current.Session(CNClaimCallsTimeStamp) IsNot Nothing Then
                    v_bTimeStamp = Current.Session(CNClaimCallsTimeStamp)
                End If
                oPayClaimResponse = oWebservice.PayClaim(v_oClaimPayment, v_bTimeStamp, v_sBranchCode)
                Current.Session(CNClaimCallsTimeStamp) = oPayClaimResponse.TimeStamp
            Catch ex As NexusProvider.NexusException
                'Show the Error message if record locked
                HasLockedByAnotherUser(ex)
            End Try
            Return oPayClaimResponse

        End Function
        ''' <summary>
        ''' This Method executes the ClaimReceipt and take care session timestamp and record locking messages as well
        ''' </summary>
        ''' <remarks></remarks>
        Public Function ClaimReceiptCall(ByRef r_oClaimReceipt As NexusProvider.ClaimReceipt, _
                                         Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal ClaimReceiptCollection As NexusProvider.ClaimReceiptCollection = Nothing) As Boolean

            Const kOptionEnableExclusiveLocking As Integer = NexusProvider.SystemOptions.ExclusiveLock

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim m_sIsExclusiveLockingEnabled As String = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, kOptionEnableExclusiveLocking, v_sBranchCode).OptionValue
            Dim oPayClaimResponse As NexusProvider.PayClaimResponse = Nothing
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())

            Try
                If r_oClaimReceipt IsNot Nothing Then
                    If Current.Session(CNClaimCallsTimeStamp) IsNot Nothing Then
                        r_oClaimReceipt.TimeStamp = Current.Session(CNClaimCallsTimeStamp)
                    End If
                    If oPortal.DoNotCreateClaimVersionOnSalvageReceipt Then
                        r_oClaimReceipt.DoNotCreateClaimVersionOnSalvageReceipt = 1
                    End If
                    oWebservice.ClaimReceipt(r_oClaimReceipt, v_sBranchCode)
                    Current.Session(CNClaimCallsTimeStamp) = r_oClaimReceipt.TimeStamp
                ElseIf ClaimReceiptCollection IsNot Nothing Then
                    For ReceiptItemIndex As Integer = 0 To ClaimReceiptCollection.Count - 1
                        If Current.Session(CNClaimCallsTimeStamp) IsNot Nothing Then
                            ClaimReceiptCollection(ReceiptItemIndex).TimeStamp = Current.Session(CNClaimCallsTimeStamp)
                        End If
                        If oPortal.DoNotCreateClaimVersionOnSalvageReceipt Then
                            ClaimReceiptCollection(ReceiptItemIndex).DoNotCreateClaimVersionOnSalvageReceipt = 1
                        End If
                    Next

                    oWebservice.ClaimReceipt(Nothing, v_sBranchCode, ClaimReceiptCollection)
                    Current.Session(CNClaimCallsTimeStamp) = ClaimReceiptCollection(ClaimReceiptCollection.Count - 1).TimeStamp
                End If
            Catch ex As NexusProvider.NexusException
                'Show the Error message if record locked
                HasLockedByAnotherUser(ex)
                Return False
            Finally
                oWebservice = Nothing
            End Try
        End Function
        ''' <summary>
        ''' This Method executes the UpdateClaimReservesOrPayments and take care session timestamp and record locking messages as well
        ''' </summary>
        ''' <remarks></remarks>
        Public Function UpdateClaimReservesOrPaymentsCall(ByVal v_oClaimOpen As NexusProvider.Claim,
                                                          ByVal v_oClaimPayment As NexusProvider.ClaimPayment,
                                                          ByVal v_bTimeStamp As Byte(),
                                                          ByVal v_iProcessType As Integer,
                                                          Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.ClaimResponse

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing

            Try
                If Current.Session(CNClaimCallsTimeStamp) IsNot Nothing AndAlso v_bTimeStamp Is Nothing Then
                    v_bTimeStamp = Current.Session(CNClaimCallsTimeStamp)
                End If
                oClaimResponse = oWebservice.UpdateClaimReservesOrPayments(v_oClaimOpen, v_oClaimPayment, v_bTimeStamp, v_iProcessType, v_sBranchCode)
                Current.Session(CNClaimCallsTimeStamp) = oClaimResponse.TimeStamp
            Catch ex As NexusProvider.NexusException
                'Show the Error message if record locked
                HasLockedByAnotherUser(ex)
            End Try
            Return oClaimResponse

        End Function
        ''' <summary>
        ''' This Method executes the BindClaim and take care session timestamp and record locking messages as well
        ''' </summary>
        ''' <remarks></remarks>
        Public Function BindClaimCall(ByVal v_oClaimOpen As NexusProvider.Claim,
                                           ByVal v_bTimeStamp As Byte(),
                                           ByVal v_iProcessType As Integer,
                                           ByVal v_oClaimPayment As NexusProvider.ClaimPayment,
                                           Optional ByVal v_sBranchCode As String = Nothing,
                                           Optional ByRef r_bPaymentAuthorized As Boolean = False, Optional ByVal oPaymentCollection As NexusProvider.ClaimPaymentCollection = Nothing) As NexusProvider.ClaimResponse

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
            Dim oQuote As NexusProvider.Quote = Current.Session(CNClaimQuote)
            Dim sOptionValue As String = ""

            If Current.Session(CNMode) = Mode.NewClaim OrElse Current.Session(CNMode) = Mode.EditClaim Then
                sOptionValue = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                NexusProvider.ProductRiskOptions.IsReservesReadOnly, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            ElseIf Current.Session(CNMode) = Mode.PayClaim Then
                sOptionValue = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance,
                NexusProvider.ProductRiskOptions.IsPaymentsReadOnly, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing)
            End If

            Dim bSkipSaveTransaction As Boolean = IIf(sOptionValue = "1", True, False)

            Try
                If Current.Session(CNClaimCallsTimeStamp) IsNot Nothing Then
                    v_bTimeStamp = Current.Session(CNClaimCallsTimeStamp)
                End If
                oClaimResponse = oWebservice.BindClaim(v_oClaimOpen, v_bTimeStamp, v_iProcessType, v_oClaimPayment, v_sBranchCode, ClaimPaymentCollection:=oPaymentCollection, bSkipSaveTransaction:=bSkipSaveTransaction)
                Current.Session(CNClaimStatus) = oClaimResponse.ResultingStatus
                Current.Session(CNClaimCallsTimeStamp) = oClaimResponse.TimeStamp
                r_bPaymentAuthorized = oClaimResponse.PaymentAuthorized
            Catch ex As NexusProvider.NexusException
                'Show the Error message if record locked
                HasLockedByAnotherUser(ex)
            End Try
            Return oClaimResponse

        End Function
        ''' <summary>
        ''' This Method executes the GetClaimDetails and take care session timestamp and record locking messages as well
        ''' </summary>
        ''' <remarks></remarks>
        Public Function GetClaimDetailsCall(ByVal v_iClaimKey As Integer, _
                                         Optional ByVal v_sBranchCode As String = Nothing, _
                                         Optional ByVal v_iFetchAllVersionAmounts As Integer = 0, _
                                         Optional ByVal bExclusiveLock As Boolean = False) As NexusProvider.ClaimDetails

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing

            Try
                If v_sBranchCode Is Nothing AndAlso Current.Session(CNClaimQuote) IsNot Nothing Then
                    v_sBranchCode = CType(Current.Session(CNClaimQuote), NexusProvider.Quote).BranchCode
                End If

                oClaimDetails = oWebservice.GetClaimDetails(v_iClaimKey, v_sBranchCode, v_iFetchAllVersionAmounts, bExclusiveLock:=bExclusiveLock)
                If Current.Session(CNClaimCallsTimeStamp) Is Nothing Then
                    Current.Session(CNClaimCallsTimeStamp) = oClaimDetails.TimeStamp
                End If

            Catch ex As NexusProvider.NexusException
                'Show the Error message if record locked
                HasLockedByAnotherUser(ex)
            Finally
                oWebservice = Nothing
            End Try

            Return oClaimDetails

        End Function
        ''' <summary>
        ''' This Method executes the AddClaimRisk and take care session timestamp and record locking messages as well
        ''' </summary>
        ''' <remarks></remarks>
        Public Function AddClaimRiskCall(ByVal v_iBaseClaimKey As Integer,
                                          ByVal v_bTimeStamp As Byte(),
                                          Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.ClaimRisk

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing

            Try
                If Current.Session(CNClaimCallsTimeStamp) IsNot Nothing Then
                    v_bTimeStamp = Current.Session(CNClaimCallsTimeStamp)
                End If
                oClaimRisk = oWebservice.AddClaimRisk(v_iBaseClaimKey, v_bTimeStamp, v_sBranchCode)
                Current.Session(CNClaimCallsTimeStamp) = oClaimRisk.TimeStamp
            Catch ex As NexusProvider.NexusException
                'Show the Error message if record locked
                HasLockedByAnotherUser(ex)
            End Try
            Return oClaimRisk

        End Function
        ''' <summary>
        ''' This Method executes the GetClaimRisk and take care session timestamp and record locking messages as well
        ''' </summary>
        ''' <remarks></remarks>
        Public Function GetClaimRiskCall(ByVal v_iBaseClaimKey As Integer,
                                              Optional ByVal v_iClaimKey As Integer = 0,
                                              Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.ClaimRisk

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing

            Try
                'Check the Claim Builder Hidden product option
                Dim oOptionType As New NexusProvider.OptionTypeSetting
                oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
                If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
                   AndAlso oOptionType.OptionValue = "1" Then
                    'Calling of the risk realted values after maintain claim
                    oClaimRisk = oWebservice.GetClaimRisk(v_iBaseClaimKey, v_iClaimKey, v_sBranchCode)
                    If Current.Session(CNClaimCallsTimeStamp) Is Nothing Then
                        Current.Session(CNClaimCallsTimeStamp) = oClaimRisk.TimeStamp
                    End If
                End If

            Catch ex As NexusProvider.NexusException
                'Show the Error message if record locked
                HasLockedByAnotherUser(ex)
            End Try
            Return oClaimRisk

        End Function
        ''' <summary>
        ''' This Method executes the UpdateClaimRisk and take care session timestamp and record locking messages as well
        ''' </summary>
        ''' <remarks></remarks>
        Public Function UpdateClaimRiskCall(ByVal v_oClaimRisk As NexusProvider.ClaimRisk,
                                     Optional ByVal v_sBranchCode As String = Nothing) As Boolean

            Const kOptionEnableExclusiveLocking As Integer = NexusProvider.SystemOptions.ExclusiveLock
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim m_sIsExclusiveLockingEnabled As String = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, kOptionEnableExclusiveLocking, v_sBranchCode).OptionValue

            Try
                If Current.Session(CNClaimCallsTimeStamp) IsNot Nothing Then
                    v_oClaimRisk.TimeStamp = Current.Session(CNClaimCallsTimeStamp)
                End If
                v_oClaimRisk.TimeStamp = oWebservice.UpdateClaimRisk(v_oClaimRisk, v_sBranchCode)
                Current.Session(CNClaimCallsTimeStamp) = v_oClaimRisk.TimeStamp
                If m_sIsExclusiveLockingEnabled = "0" AndAlso v_oClaimRisk.TimeStamp Is Nothing Then
                    Return False
                End If
                Return True
            Catch ex As NexusProvider.NexusException
                'Show the Error message if record locked
                HasLockedByAnotherUser(ex)
            Finally
                oWebservice = Nothing
            End Try

        End Function
        ''' <summary>
        ''' This Method check record locked by another user or not  
        ''' </summary>
        ''' <remarks></remarks>
        Sub HasLockedByAnotherUser(ByVal ex As NexusProvider.NexusException, Optional ByVal sVia As String = "CLAIM")
            Dim oResource As ResXResourceReader
            Dim en As IDictionaryEnumerator
            Dim opage = TryCast(Current.CurrentHandler, System.Web.UI.Page)

            If ex.Errors(0).Code = "200" Then   'Code : 200 :: Description: Record Locked By Another User 
                'if record locked by another user, Show the Error message using Resource file
                oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "App_LocalResources/Error.aspx.resx"))
                en = oResource.GetEnumerator()

                Dim sLockedByUser As String = ex.Errors(0).Description.Substring(InStr(ex.Errors(0).Description, "Locked By = "))
                If sVia = "PARTY" Then
                    sLockedByUser = ex.Errors(0).Detail.Substring(InStr(ex.Errors(0).Detail, "Locked By = ") + 11)
                End If
                While (en.MoveNext)
                    If en.Key.ToString.Trim = "error_RecordLockedByAnotherUser" Then
                        opage.ClientScript.RegisterClientScriptBlock(opage.GetType(), "Locking",
            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('" & en.Value.ToString.Replace("[!USER_NAME!]", sLockedByUser) & "'); return false;});</script>")
                    End If
                End While

                If sVia = "CLIAM" Then
                    HttpContext.Current.Session(CNIsClaimLocked) = True
                End If


            ElseIf ex.Errors(0).Code = "206" OrElse ex.Errors(0).Code = "1000019" Then   'Code : 206 :: Description: Record Changed Another User 
                'if Record Changed Another User, Show the Error message using Resource file
                oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "App_LocalResources/Error.aspx.resx"))
                en = oResource.GetEnumerator()
                While (en.MoveNext)
                    If en.Key.ToString.Trim = "error_RecordChanged" Then
                        opage.ClientScript.RegisterClientScriptBlock(opage.GetType(), "Locking",
            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('" & en.Value & "'); return false;});</script>")
                    End If
                End While

                If sVia = "CLIAM" Then
                    Current.Session(CNIsClaimLocked) = True
                End If
            Else
                Throw ex
            End If
        End Sub
        ''Function to check if user can perform as task as specified in config

        Public Function UserCanDoTask(ByVal Task As String) As Boolean
            If CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Tasks.Task(Task) IsNot Nothing Then
                'get the list of roles that can do this task from config
                Dim sRoles As String = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).Tasks.Task(Task).Role
                'call UserIsInRoles to see if the current user can carry out this ta
                Return UserIsInRoles(sRoles)

            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' to search the party type code 
        ''' </summary>
        ''' <param name="sType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function OtherPartySearch(ByVal sType As String) As String
            Dim oWebService As NexusProvider.ProviderBase
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim olist As NexusProvider.LookupListCollection
            olist = oWebService.GetList(NexusProvider.ListType.PMLookup, "party_type", True, False)
            Dim sOTPartyTypeCode As String = ""
            For iCount As Integer = 0 To olist.Count - 1
                If olist(iCount).Code.ToUpper.Trim = sType.ToUpper.Trim OrElse olist(iCount).Description.ToUpper.Trim = sType.ToUpper.Trim Then
                    sOTPartyTypeCode = olist(iCount).Code
                End If
            Next
            oWebService = Nothing
            Return sOTPartyTypeCode
        End Function

        ''' <summary>
        ''' This Method the Update the claim session varible with latest data
        ''' </summary>
        ''' <remarks></remarks>
        Sub UpdateClaim(Optional ByVal bGetClaimDetails As Boolean = True)
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Current.Session(CNClaim), NexusProvider.ClaimOpen)
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = CType(Current.Session(CNClaimQuote), NexusProvider.Quote)
            Dim sBranchCode As String = oQuote.BranchCode

            Try
                If bGetClaimDetails = True Then
                    'Retreiving the claim details
                    oClaimDetails = oWebservice.GetClaimDetails(oOpenClaim.ClaimKey, sBranchCode)
                    'Updating the objects and claim session variables
                    With oClaimDetails
                        oOpenClaim.CatastropheCode = .CatastropheCode
                        oOpenClaim.BaseClaimKey = .BaseClaimKey
                        oOpenClaim.BaseCaseKey = .BaseCaseKey
                        oOpenClaim.Claim = .Claim
                        oOpenClaim.ClaimCoInsurer = .ClaimCoInsurer
                        oOpenClaim.ClaimDescription = .ClaimDescription
                        oOpenClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                        oOpenClaim.ClaimKey = .ClaimKey
                        oOpenClaim.ClaimNumber = .ClaimNumber
                        oOpenClaim.ClaimPeril = .ClaimPeril
                        oOpenClaim.ClaimStatus = .ClaimStatus
                        oOpenClaim.ClaimStatusDate = .ClaimStatusDate
                        oOpenClaim.ClaimStatusID = .ClaimStatusID
                        oOpenClaim.ClaimVersion = .ClaimVersion
                        oOpenClaim.ClaimVersionDescription = .ClaimVersionDescription
                        oOpenClaim.ClientClaimNumber = .ClientClaimNumber
                        oOpenClaim.ClientEmail = .ClientEmail
                        oOpenClaim.ClientFaxNo = .ClientFaxNo
                        oOpenClaim.ClientMobileNo = .ClientMobileNo
                        oOpenClaim.ClientName = .ClientName

                        If String.IsNullOrEmpty(.Client.ShortName) = True Then
                            Dim oClaimVersions As NexusProvider.VersionsCollections = CType(Current.Session(CNClaimVersion), NexusProvider.VersionsCollections)
                            If oClaimVersions IsNot Nothing AndAlso oClaimVersions.Count > 0 Then
                                oOpenClaim.ClientShortName = oClaimVersions(0).ClientShortName
                            End If
                        Else
                            oOpenClaim.ClientShortName = .ClientShortName
                        End If

                        oOpenClaim.ClientTelNo = .ClientTelNo
                        oOpenClaim.ClientTelNoOff = .ClientTelNoOff
                        oOpenClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                        oOpenClaim.Comments = .Comments
                        oOpenClaim.Contact = .Contact
                        oOpenClaim.CurrencyISOCode = .CurrencyCode
                        oOpenClaim.Description = .Description
                        oOpenClaim.ExternalHandler = .ExternalHandler
                        oOpenClaim.HandlerCode = .HandlerCode
                        oOpenClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                        oOpenClaim.InfoOnly = .InfoOnly
                        oOpenClaim.InsuranceFileKey = .InsuranceFileKey
                        oOpenClaim.InsuranceRef = .InsuranceRef
                        oOpenClaim.InsurerClaimNumber = .InsurerClaimNumber
                        oOpenClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                        oOpenClaim.IsDeleted = .IsDeleted
                        oOpenClaim.LastModifiedDate = .LastModifiedDate
                        oOpenClaim.LikelyClaim = .LikelyClaim
                        oOpenClaim.Location = .Location
                        oOpenClaim.LossDate = .LossDate
                        oOpenClaim.LossDateFrom = .LossDateFrom
                        oOpenClaim.LossFromDate = .LossToDate
                        oOpenClaim.LossToDate = .LossToDate
                        oOpenClaim.LossToDateSpecified = .LossToDateSpecified
                        oOpenClaim.Payments = .Payments
                        oOpenClaim.PolicyNumber = .PolicyNumber
                        oOpenClaim.PolicyType = .PolicyType
                        oOpenClaim.PrimaryCause = .PrimaryCause
                        oOpenClaim.PrimaryCauseCode = .PrimaryCauseCode
                        oOpenClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                        oOpenClaim.ProductDescription = .ProductDescription
                        oOpenClaim.ProgressStatusCode = .ProgressStatusCode
                        oOpenClaim.ProgressStatusDescription = .ProgressStatusDescription
                        oOpenClaim.ReportedDate = .ReportedDate
                        oOpenClaim.Reserve = .Reserve
                        oOpenClaim.RiskKey = .RiskKey
                        oOpenClaim.RiskType = CType(Current.Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).RiskTypeCode
                        oOpenClaim.RiskTypeDescription = CType(Current.Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).Description
                        oOpenClaim.SecondaryCause = .SecondaryCause
                        oOpenClaim.SecondaryCauseCode = .SecondaryCauseCode
                        oOpenClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                        oOpenClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                        oOpenClaim.TotalShare = .TotalShare
                        oOpenClaim.Town = .Town
                        oOpenClaim.TownCode = .TownCode
                        oOpenClaim.UnderwritingYearCode = .UnderwritingYearCode
                        oOpenClaim.UserDefFldACode = .UserDefFldACode
                        oOpenClaim.UserDefFldBCode = .UserDefFldBCode
                        oOpenClaim.UserDefFldCCode = .UserDefFldCCode
                        oOpenClaim.UserDefFldDCode = .UserDefFldECode
                        oOpenClaim.UserDefFldECode = .UserDefFldECode
                        'Added for Insurer
                        oOpenClaim.Insurer = .Insurer
                        Current.Session.Item(CNClaimTimeStamp) = .TimeStamp
                        oOpenClaim.CurrencyISOCode = .CurrencyCode
                        Current.Session.Item(CNCurrenyCode) = Trim(.CurrencyCode) 'Changed
                        oOpenClaim.Client = .Client
                        Current.Session(CNClaimNumber) = .ClaimNumber
                        Current.Session(CNStatus) = .ClaimStatus
                    End With
                End If
                If Current.Session(CNMode) = Mode.PayClaim OrElse Current.Session(CNMode) = Mode.ViewClaim Then
                    Dim oOptionType As New NexusProvider.OptionTypeSetting
                    oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 12)
                    If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
    AndAlso oOptionType.OptionValue = "1" Then
                        oClaimRisk = oWebservice.GetClaimRisk(oOpenClaim.BaseClaimKey, oOpenClaim.ClaimKey, sBranchCode)
                        Current.Session.Item(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
                        Current.Session(CNDataSet) = oClaimRisk.XMLDataSet
                    End If
                End If

            Catch ex As Exception

            Finally
                oClaimRisk = Nothing
                oClaimDetails = Nothing
                oOpenClaim = Nothing
                oWebservice = Nothing
                oQuote = Nothing
            End Try
        End Sub


        ''' <summary>
        ''' This Method executes the UpdateParty and take care session timestamp and record locking messages as well
        ''' </summary>
        ''' <remarks></remarks>
        Public Function UpdatePartyCall(ByRef r_oParty As NexusProvider.BaseParty,
                                        Optional ByVal v_sBranchCode As String = Nothing) As Boolean

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            Try
                oWebservice.UpdateParty(r_oParty, v_sBranchCode)
            Catch ex As NexusProvider.NexusException
                'Show the Error message if record locked
                HasLockedByAnotherUser(ex, "PARTY")
                Return False
            End Try
            Return True

        End Function

        ''' <summary>
        ''' Get email template based on template ID and Product code in web.config
        ''' </summary>
        ''' <param name="sTemplateCode"></param>
        ''' <param name="sProductCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetEmailTemplateDetails(ByVal sTemplateCode As String, ByVal sProductCode As String) As NexusProvider.EmailTemplateConfiguration
            Dim dtEmailDetails As New DataTable
            Dim EmailTemplates As New Nexus.Library.Config.EmailTemplates

            EmailTemplates = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).EmailTemplates

            dtEmailDetails.Columns.Add("ID")
            dtEmailDetails.Columns.Add("Path")
            dtEmailDetails.Columns.Add("Sender")
            dtEmailDetails.Columns.Add("Recipient")
            dtEmailDetails.Columns.Add("Subject")
            dtEmailDetails.Columns.Add("ProductCode")
            dtEmailDetails.Columns.Add("EmailTemplateCode")
            dtEmailDetails.Columns.Add("SubjectTemplateCode")
            dtEmailDetails.Columns.Add("TransactionType")

            For i As Integer = 0 To EmailTemplates.Count - 1
                Dim drEmailDetails As DataRow
                If EmailTemplates.EmailTemplate(i).ID.ToString().ToUpper() = sTemplateCode.ToUpper() Then
                    drEmailDetails = dtEmailDetails.NewRow()
                    With EmailTemplates.EmailTemplate(i)
                        drEmailDetails(0) = .ID
                        drEmailDetails(1) = .Path
                        drEmailDetails(2) = .Sender
                        drEmailDetails(3) = .Recipient
                        drEmailDetails(4) = .Subject
                        drEmailDetails(5) = .ProductCode
                        drEmailDetails(6) = .EmailTemplateCode
                        drEmailDetails(7) = .SubjectTemplateCode
                        drEmailDetails(8) = .TransactionType
                    End With
                    dtEmailDetails.Rows.Add(drEmailDetails)
                End If
            Next

            Dim oEmailTemplate As NexusProvider.EmailTemplateConfiguration = New NexusProvider.EmailTemplateConfiguration
            For i As Integer = 0 To dtEmailDetails.Rows.Count - 1
                Dim arrProductCodes() As String = IIf(Not IsDBNull(dtEmailDetails.Rows(i).Item("ProductCode")), dtEmailDetails.Rows(i).Item("ProductCode").ToString().ToUpper().Split(","), New String() {})
                For j As Integer = 0 To arrProductCodes.Length - 1
                    If arrProductCodes(j) = sProductCode.ToUpper Then
                        oEmailTemplate.ID = dtEmailDetails.Rows(i).Item("ID").ToString()
                        oEmailTemplate.Path = dtEmailDetails.Rows(i).Item("Path").ToString()
                        oEmailTemplate.Sender = dtEmailDetails.Rows(i).Item("Sender").ToString()
                        oEmailTemplate.Recipient = dtEmailDetails.Rows(i).Item("Recipient").ToString()
                        oEmailTemplate.Subject = dtEmailDetails.Rows(i).Item("Subject").ToString()
                        oEmailTemplate.ProductCode = dtEmailDetails.Rows(i).Item("ProductCode").ToString()
                        oEmailTemplate.EmailTemplateCode = dtEmailDetails.Rows(i).Item("EmailTemplateCode").ToString()
                        oEmailTemplate.SubjectTemplateCode = dtEmailDetails.Rows(i).Item("SubjectTemplateCode").ToString()
                        oEmailTemplate.TransactionType = dtEmailDetails.Rows(i).Item("TransactionType").ToString()
                    End If
                Next
                ''if product other than in web.config pick datarow with empty productcode value is picked
                If String.IsNullOrEmpty(oEmailTemplate.ID) AndAlso (IsDBNull(dtEmailDetails.Rows(i).Item("ProductCode")) _
                    OrElse String.IsNullOrEmpty(dtEmailDetails.Rows(i).Item("ProductCode"))) Then
                    oEmailTemplate.ID = dtEmailDetails.Rows(i).Item("ID").ToString()
                    oEmailTemplate.Path = dtEmailDetails.Rows(i).Item("Path").ToString()
                    oEmailTemplate.Sender = dtEmailDetails.Rows(i).Item("Sender").ToString()
                    oEmailTemplate.Recipient = dtEmailDetails.Rows(i).Item("Recipient").ToString()
                    oEmailTemplate.Subject = dtEmailDetails.Rows(i).Item("Subject").ToString()
                    oEmailTemplate.ProductCode = String.Empty
                    oEmailTemplate.EmailTemplateCode = dtEmailDetails.Rows(i).Item("EmailTemplateCode").ToString()
                    oEmailTemplate.SubjectTemplateCode = dtEmailDetails.Rows(i).Item("SubjectTemplateCode").ToString()
                    oEmailTemplate.TransactionType = dtEmailDetails.Rows(i).Item("TransactionType").ToString()
                End If
            Next

            Return oEmailTemplate
        End Function
        ''' <summary>
        ''' User need to call this function from the Claim client pages to update the Claim payment in session.
        ''' </summary>
        Sub SetScriptPayment()
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Current.Session.Item(CNClaim), NexusProvider.ClaimOpen)

            For iPeril As Integer = 0 To oClaimOpen.ClaimPeril.Count - 1
                With oClaimOpen.ClaimPeril.Item(iPeril)
                    If .ClaimPayment IsNot Nothing Then
                        For iPayment As Integer = 0 To .ClaimPayment.Count - 1
                            With .ClaimPayment.Item(iPayment)
                                If .IsThisPayment Then
                                    .ThisPaymentINCLTax = .PaymentAmount + .TaxAmount
                                    .ThisPaymentTax = .TaxAmount
                                End If
                            End With
                        Next
                    End If
                End With

            Next
            Current.Session.Item(CNClaim) = oClaimOpen
        End Sub
        ''' <summary>
        '''  User need to call this function from the Claim client pages to update the Claim payment in session.
        ''' </summary>
        ''' <param name="o_sCurrencyCode"></param>
        ''' <param name="bIncludeTax"></param>
        ''' <returns></returns>
        Function GetScriptPerilPaidAmount(Optional ByRef o_sCurrencyCode As String = "", Optional ByVal bIncludeTax As Boolean = False, Optional ByVal nperil As Integer = -1) As Decimal
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Current.Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim iPeril As Integer
            If nperil >= 0 Then
                iPeril = nperil
            Else
                iPeril = CInt(Current.Session(CNClaimPerilIndex))
            End If
            Dim crAmountPaid As Decimal = 0
            With oClaimOpen.ClaimPeril.Item(iPeril)
                If .ClaimPayment IsNot Nothing Then
                    For iPayment As Integer = 0 To .ClaimPayment.Count - 1
                        With .ClaimPayment.Item(iPayment)
                            If .IsThisPayment Then
                                If bIncludeTax Then
                                    crAmountPaid += .ThisPaymentINCLTax
                                Else
                                    crAmountPaid += (.ThisPaymentINCLTax - .ThisPaymentTax)
                                End If
                                o_sCurrencyCode = .CurrencyCode
                            End If
                        End With
                    Next
                End If
            End With

            oClaimOpen = Nothing
            Return crAmountPaid
        End Function
        ''' <summary>
        ''' Get the status for the payment done through script.
        ''' </summary>
        ''' <param name="nPerilID"></param>
        ''' <returns></returns>
        Function IsPaymentDoneViaScriptForPeril(ByVal nPerilID As Integer) As Boolean
            Dim oClaimOpen As NexusProvider.ClaimOpen = CType(Current.Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim crAmountPaid As Decimal = 0
            For iPeril As Integer = 0 To oClaimOpen.ClaimPeril.Count - 1
                With oClaimOpen.ClaimPeril.Item(iPeril)
                    If .ClaimPerilKey = nPerilID Then
                        If .ClaimPayment IsNot Nothing Then
                            For iPayment As Integer = 0 To .ClaimPayment.Count - 1
                                With .ClaimPayment.Item(iPayment)
                                    If .IsThisPayment Then
                                        oClaimOpen = Nothing
                                        Return True
                                    End If
                                End With
                            Next
                        End If
                    End If
                End With
            Next

            oClaimOpen = Nothing
            Return False

        End Function

        ''' <summary>
        ''' RedirectInformationCheckList : sets the next url based on the previous call from overview.aspx
        ''' </summary>
        ''' <param name="sNextpageToRedirect"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function RedirectShowCheckUnpaidPremium(ByVal sNextpageToRedirect As String) As String
            Dim bIsUnpaidCheckFlowEnabledForClaim As Boolean = False
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sProductCode As String = ""

            If Trim(Current.Session(CNProductCode)) IsNot Nothing Then
                sProductCode = Current.Session(CNProductCode)

            End If
            Dim sReturnUrl As String = String.Empty
            Dim oClaim As NexusProvider.ClaimOpen = CType(Current.Session(CNClaim), NexusProvider.ClaimOpen)
            If CType(Current.Session.Item(CNMode), Mode) = Mode.PayClaim Then
                bIsUnpaidCheckFlowEnabledForClaim = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, sProductCode, Nothing).CheckUnpaidStatus

            ElseIf CType(Current.Session.Item(CNMode), Mode) = Mode.NewClaim Then
                bIsUnpaidCheckFlowEnabledForClaim = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.OpenClaim, sProductCode, Nothing).CheckUnpaidStatus
            End If

            If bIsUnpaidCheckFlowEnabledForClaim AndAlso oClaim IsNot Nothing AndAlso oClaim.IsPolicyOutstanding Then
                If sNextpageToRedirect <> "" Then
                    sReturnUrl = "~/Claims/CheckUnPaidStatus.aspx?Mode=" & sNextpageToRedirect
                End If
            End If
            Return sReturnUrl
        End Function
        ''' <summary>
        ''' To Check Whether Insurance_folder is Locked or Not
        ''' If Locked then Return the Name against whom the Record is Locked.
        ''' </summary>
        ''' <param name="nInsuranceFolderCnt"></param>
        ''' <param name="sBranchCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function CheckLock(ByVal nInsuranceFolderCnt As Integer, ByVal sBranchCode As String) As String
            Dim oLockCollection As NexusProvider.LockCollection
            Dim oWebService As NexusProvider.ProviderBase = Nothing
            Dim sUserName As String = String.Empty

            oWebService = New NexusProvider.ProviderManager().Provider
            oLockCollection = oWebService.GetLockDetails(sBranchCode)

            For Each oLockItem As NexusProvider.Locks In oLockCollection
                If HttpContext.Current.Session(CNLoginName).Trim().ToUpper <> oLockItem.LockUserName.Trim().ToUpper _
                AndAlso ((oLockItem.LockName.Trim() = "insurance_folder_cnt" AndAlso oLockItem.LockValue = nInsuranceFolderCnt) _
                OrElse (oLockItem.LockName.Trim() = "pfprem_finance_cnt" AndAlso oLockItem.LockValue = nInsuranceFolderCnt)) Then
                    sUserName = oLockItem.LockUserName.Trim()
                    Exit For
                End If
            Next
            oWebService = Nothing
            Return sUserName
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="nInsuranceFolderCnt"></param>
        ''' <remarks></remarks>
        Sub UnlockPolicy(ByVal nInsuranceFolderCnt As Integer, ByVal sBranchCode As String)
            Dim oLockCollection As NexusProvider.LockCollection
            Dim oWebService As NexusProvider.ProviderBase = Nothing
            Dim sUserName As String = String.Empty
            Dim bMaintainedSuccess As Boolean = False
            Dim bLogout As Boolean = False
            Dim bAllClear As Boolean = False
            Dim oLock As New NexusProvider.Locks
            oWebService = New NexusProvider.ProviderManager().Provider
            oLockCollection = oWebService.GetLockDetails(sBranchCode:=sBranchCode)

            For Each oLockItem As NexusProvider.Locks In oLockCollection
                If HttpContext.Current.Session(CNLoginName).ToLower().Trim() = oLockItem.LockUserName.ToLower().Trim() AndAlso oLockItem.LockName.Trim() = "insurance_folder_cnt" AndAlso oLockItem.LockValue = nInsuranceFolderCnt Then
                    oLock.LockName = oLockItem.LockName
                    oLock.LockValue = oLockItem.LockValue
                    oLockCollection.Add(oLock)
                    bMaintainedSuccess = oWebService.MaintainLock(oLockCollection, bAllClear, bLogout, sBranchCode)
                    Exit For
                End If
            Next
            oWebService = Nothing
        End Sub
        ''' <summary>
        ''' This Method executes the GetAgentSettings and prevents duplicate calls for same agent if its already in session
        ''' </summary>
        ''' <remarks></remarks>
        Public Function GetAgentSettingsCall(ByRef r_oAgentSettings As NexusProvider.AgentSettings, _
                                             ByVal nAgentKey As Integer, _
                                             Optional ByVal v_sBranchCode As String = Nothing) As Boolean

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            Try

                If Current.Session(CNAgentSettings) IsNot Nothing AndAlso _
                    nAgentKey = CType(Current.Session(CNAgentSettings), NexusProvider.AgentSettings).AgentKey Then
                    If nAgentKey = CType(Current.Session(CNAgentSettings), NexusProvider.AgentSettings).AgentKey Then
                        r_oAgentSettings = CType(Current.Session(CNAgentSettings), NexusProvider.AgentSettings)
                        Return True
                    End If
                End If
                r_oAgentSettings = oWebservice.GetAgentSettings(nAgentKey)
                Current.Session(CNAgentSettings) = Nothing
                Current.Session(CNAgentSettings) = r_oAgentSettings
            Catch ex As NexusProvider.NexusException
                HasLockedByAnotherUser(ex, "GetAgentSettings")
                Return False
            End Try
            Return True

        End Function


#Region "WPR05"

        Public Sub QuoteAllRisk()
            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            Dim oRiskCollection As New NexusProvider.RiskCollection
            oRiskCollection = oQuote.Risks
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oNexusFrameWork As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim sProductFolder As String = "~/" & oNexusFrameWork.ProductsFolder & "/" & oProduct.Name & "/"

            Dim oRiskType As Config.RiskType

            For jCount As Integer = 0 To oQuote.Risks.Count - 1
                If oQuote.Risks(jCount).RiskCode Is Nothing Then
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(jCount).RiskTypeCode.Trim)
                Else
                    oRiskType = oProduct.RiskTypes.RiskType(oQuote.Risks(jCount).RiskCode.Trim)
                End If

                If oQuote.Risks(jCount).ScreenCode Is Nothing Then
                    oQuote.Risks(jCount).ScreenCode = GetScreenCode(sProductFolder & oRiskType.Path & "\" & oProduct.FullQuoteConfig)
                End If
                oWebService.UpdateRisk(oQuote, jCount, Nothing)
                Current.Session(CNQuote) = oQuote
            Next

        End Sub
#End Region


        Sub SetCurrentMTATypeSession()
            If Current.Session(CNQuote) IsNot Nothing Then
                Dim oQuote As NexusProvider.Quote = CType(Current.Session(CNQuote), NexusProvider.Quote)
                If oQuote.InsuranceFileTypeCode.Trim() = "MTAQCAN" Then
                    Current.Session(CNMTAType) = MTAType.CANCELLATION
                ElseIf oQuote.InsuranceFileTypeCode.Trim() = "MTAQREINS" Then
                    Current.Session(CNMTAType) = MTAType.REINSTATEMENT
                ElseIf oQuote.InsuranceFileTypeCode.Trim() = "MTAQTETEMP" Then
                    Current.Session(CNMTAType) = MTAType.TEMPORARY
                Else
                    Current.Session(CNMTAType) = MTAType.PERMANENT
                End If
            End If
        End Sub

        ReadOnly Property GetCurrentMTAType() As MTAType
            Get
                Return CType(Current.Session(CNMTAType), MTAType)
            End Get
        End Property
        Public Sub BuyButton(ByVal sender As Object, ByVal e As System.EventArgs)
            'if MTA is backdated then we need to call AddBackdatedMtaQuote and show backdated versions on a modal page
            'else it should work as per previous functionality
            If CType(Current.Session(CNIsBackDatedMTA), Boolean) = True Then
                'Dim sURL As String
                Dim oBackDatedVersions As NexusProvider.PolicyCollection
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oNexusFrameWork As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
                Dim oProduct As Config.Product = oNexusFrameWork.Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
                Dim oMtaQuote As New NexusProvider.MTA()

                'Create a request for SAM Method "AddBackdatedMtaQuote"
                oMtaQuote.InsuranceFileKey = CType(Current.Session(CNInsuranceFileKey), Integer)
                oMtaQuote.MTAType = CNMTATypeDesc 'as discessed MTA Type is fixed for both the cases either PERMANENT or CANCELLATION
                oMtaQuote.TypeOfMta = "PERMANENT" 'CNMTATypeDesc
                oMtaQuote.MtaEffectiveDate = oQuote.CoverStartDate
                oMtaQuote.MtaExpiryDate = oQuote.ExpiryDate
                Dim iGracePeriod As Integer = 0
                With oQuote
                    oMtaQuote.AccountHandlerCnt = .AccountHandlerCnt
                    oMtaQuote.AnalysisCode = .AnalysisCode
                    oMtaQuote.BranchCode = .BranchCode
                    oMtaQuote.BusinessTypeCode = .BusinessTypeCode
                    oMtaQuote.IssueDate = .InceptionDate
                    oMtaQuote.InsuranceFileKey = .InsuranceFileKey
                    oMtaQuote.InsuredName = .InsuredName
                    oMtaQuote.LTUExpiryDate = .LTUExpiryDate
                    oMtaQuote.PolicyStatusCode = .PolicyStatusCode
                    oMtaQuote.PolicyStyleCode = .PolicyStyleCode
                    oMtaQuote.ProposalDate = .ProposalDate
                    iGracePeriod = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code, oProduct.ProductCode, oQuote.Risks(Current.Session(CNCurrentRiskKey)).RiskCode)
                    oMtaQuote.QuoteExpiryDate = .QuoteExpiryDate
                    oMtaQuote.QuoteTimeStamp = .TimeStamp
                    oMtaQuote.ReferredAtRenewal = .ReferredAtRenewal
                    oMtaQuote.ReferredOnMTA = .ReferredAtMTA
                    oMtaQuote.Regarding = .Regarding
                    oMtaQuote.RenewalMethodCode = .RenewalMethodCode
                    oMtaQuote.StopReasonCode = .StopReasonCode
                    oMtaQuote.FrequencyCode = .FrequencyCode
                    oMtaQuote.PolicyKey = .InsuranceFileRef
                    If Current.Session(CNMTAType) = MTAType.CANCELLATION Then
                        oMtaQuote.TranactionType = "MTC"
                        oMtaQuote.IsInteractive = False
                        Current.Session(CNIsInteractiveBackdatedMTA) = False
                    Else
                        oMtaQuote.TranactionType = "MTA"
                        oMtaQuote.IsInteractive = True
                        Current.Session(CNIsInteractiveBackdatedMTA) = True
                    End If
                    Current.Session(CNBaseInsuranceFileKey) = .InsuranceFileKey
                    oMtaQuote.InsuranceFolderKey = oQuote.InsuranceFolderKey
                    oMtaQuote.PartyKey = oQuote.PartyKey
                End With

                Dim sFailureReason As String = ""
                Try
                    ' if some records exists in session CNBackDatedVersions then no need to make SAM call again
                    If Current.Session(CNBackDatedVersions) Is Nothing Then
                        'Call SAM Method for Adding back dated versions. This will return backdated versions or failure reason 
                        'WPR 33 Web Method, Uncomment on SAM Update
                        oBackDatedVersions = oWebService.AddBackdatedMtaQuote(oMtaQuote, sFailureReason)
                        'oBackDatedVersions = Nothing
                        'Add backdated version in session.So that they can be used further
                        Current.Session(CNBackDatedVersions) = oBackDatedVersions
                        'Add failure message in view state so that it can be used further
                    Else
                        Current.Session.Remove(CNBackDatedVersions)
                    End If
                Finally
                    oWebService = Nothing
                    oMtaQuote = Nothing
                End Try

                'if failure reason returned then show failure reason as alert 
                If Not String.IsNullOrEmpty(sFailureReason) Then
                    Dim oPage As Page
                    oPage = CType(Current.Handler, Page)
                    If oPage IsNot Nothing Then
                        ScriptManager.RegisterStartupScript(oPage, GetType(String), "AddBackdatedMTAQuoteError", "<script language=""JavaScript"" type=""text/javascript"">function ShowError(){alert('" & sFailureReason & "');}</script>", True)
                    End If
                Else ' Redirect to backdated versions page
                    Current.Response.Redirect("~/secure/BackDatedMTA.aspx")
                End If
            Else
                'Do as per previous logic
                RedirectOnBuyNowClick()

            End If
            'Else
            '    'Do as per previous logic
            '    RedirectOnBuyNowClick()

        End Sub

        ''' <summary>
        ''' For Normal MTA(non backdated) this method should be called.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RedirectOnBuyNowClick()
            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProductConfig As Config.Product = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(oQuote.ProductCode)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            If CheckRefer() = True Then
                Current.Session(CNQuoteMode) = QuoteMode.FullQuote
                Current.Response.Redirect(AppSettings("WebRoot") & "referred.aspx")
            ElseIf CheckDecline() = True Then
                Current.Session(CNQuoteMode) = QuoteMode.FullQuote
                Current.Response.Redirect(AppSettings("WebRoot") & "declined.aspx")
            Else
                If Current.Session(CNMTAType) <> MTAType.CANCELLATION Then
                    Dim bRiskDeleted As Boolean = False
                    Dim iTotalRiskCount As Integer = oQuote.Risks.Count
                    For iTempVar As Integer = 0 To iTotalRiskCount - 1
                        If bRiskDeleted = True Then
                            bRiskDeleted = False
                            iTempVar -= 1
                        End If
                        If iTempVar < iTotalRiskCount AndAlso oQuote.Risks(iTempVar).IsRisk = False Then
                            oWebService.DeleteRisk(oQuote, iTempVar, oQuote.BranchCode)
                            oQuote.Risks.Remove(iTempVar)
                            bRiskDeleted = True
                            iTotalRiskCount = oQuote.Risks.Count
                        End If
                    Next
                    'Update Session(CNQuote) with only selected risk
                    Current.Session(CNQuote) = oQuote
                End If

                Dim dTatalPremium As Decimal
                If oQuote.Risks.Count > 0 Then
                    dTatalPremium = oQuote.GrossTotal
                End If

                If CType(Current.Session(CNIsAnonymous), Boolean) = True Then
                    Current.Session(CNRedirectedFor) = "BuyQuote"
                    'redirecting the user to Find Client Page if Quote is anonymous
                    If CType(Current.Session.Item(CNLoginType), LoginType) = LoginType.Agent Then
                        Current.Response.Redirect("~/secure/agent/FindClient.aspx")
                    End If
                End If

                If (dTatalPremium < 0.0) And (Current.Session(CNMTAType) <> MTAType.CANCELLATION) Then
                    Current.Session(CNMode) = Mode.Edit
                    Current.Session(CNQuoteInSync) = False
                    Current.Session.Remove(CNOI)
                    'this will check in case of MTA Return Premium exists
                    ' which will check statements is set to true in web.config and then will redirect to staements page
                    If CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(CMS.Library.Portal.GetPortalID()).ShowStatements = True Then
                        Current.Response.Redirect("~/secure/Statements.aspx")
                        'else will redirect to transaction confirmation page directly
                    Else
                        Current.Session(CNPaid) = True
                        Current.Response.Redirect("~/secure/TransactionConfirmation.aspx")
                        'End If
                    End If
                End If
                'This will allow Zero Premium to be transacted in Case of NB/MTA/Renewals
                If (dTatalPremium = 0.0) Then
                    Current.Session(CNMode) = Mode.Edit
                    Current.Session(CNQuoteInSync) = False
                    Current.Session.Remove(CNOI)
                    Current.Session(CNPaid) = True
                    Current.Response.Redirect("~/secure/TransactionConfirmation.aspx")
                End If

                If Current.Request.CurrentExecutionFilePath.ToUpper.Contains("/SECURE/CREDITCARD.ASPX") Then
                    Current.Session(CNPaid) = True
                    Current.Response.Redirect("~/secure/TransactionConfirmation.aspx")
                Else
                    Current.Response.Redirect("~/secure/Statements.aspx")
                End If

            End If
        End Sub

        Public Function GetPaymentHubPageURL() As String
            Dim sReturnUrl As String = String.Empty
            Dim oBasePayment As New BasePayment
            sReturnUrl = oBasePayment.GetPayPageURL()
            Return sReturnUrl
        End Function

        Public Sub PaymentHubProcessPurchase(ByRef oPaymentItem As NexusProvider.PaymentHubDetails)
            Dim oBasePayment As New BasePayment
            oBasePayment.ProcessPurchase(oPaymentItem)
        End Sub

        Public Sub PaymentHubProcessRefund(ByRef oPaymentItem As NexusProvider.PaymentHubDetails)
            Dim oBasePayment As New BasePayment
            oBasePayment.ProcessRefund(oPaymentItem)
        End Sub

        Public Function GetPaymentHubConfig() As NexusProvider.PaymentHubConfig
            Dim oPaymentHubConfig As NexusProvider.PaymentHubConfig = CType(HttpContext.Current.Application(CNPaymentHubDetails), NexusProvider.PaymentHubConfig)
            If oPaymentHubConfig Is Nothing Then
                Dim oWebService As NexusProvider.ProviderBase

                oWebService = New NexusProvider.ProviderManager().Provider
                oPaymentHubConfig = CType(oWebService.GetPaymentHubSystemOptions(HttpContext.Current.Session(CNBranchCode)), NexusProvider.PaymentHubConfig)
                HttpContext.Current.Application(CNPaymentHubDetails) = oPaymentHubConfig

            End If
            Return oPaymentHubConfig
        End Function

    End Module
End Namespace
