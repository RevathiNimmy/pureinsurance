Option Strict Off
Option Explicit On
Imports System.ComponentModel
Imports System.Globalization
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports SharedFiles
Imports SSP.Pure.UsersSync
Imports SSP.Pure.UsersSync.Services

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Edit History:
    '
    ' RAW 07/02/2003 : ISS2070 : load options 1024, 1025 into m_cLookupTables even if no
    '                            tax bands actually exist in Tax band table
    ' RAW 06/06/2003 : CR23 : load combo box for Premium Override Event level (1030)
    ' CJB 30/06/2005 : PN22008 : Changed IsExcluded to check for system option no. 5003 rather than
    '                  65 as it has been changed for Enhanced Cheque Production
    ' CJB 22/07/2005 : PN22554 : Changed IsExcluded to NOT check for system option no. 66 as this
    '                  has nothing to do with Enhanced Cheque Production!
    ' CJB 16/08/2005 : PN23212 : Changed cboOption_Click to disable a checkbox if a relevant combo's
    '                  value is not chosen etc
    ' ***************************************************************** '



    Private Const ACClass As String = "frmInterface"

    Private Const ACOptionNumber As Integer = 0
    Private Const ACSystemOptionConfigurationGroupId As Integer = 1
    Private Const ACControlType As Integer = 2
    Private Const ACControlTop As Integer = 3
    Private Const ACControlHeight As Integer = 4
    Private Const ACControlLeft As Integer = 5
    Private Const ACControlWidth As Integer = 6
    Private Const ACControlCaption As Integer = 7
    Private Const ACLookupTable As Integer = 0
    Private Const ACCommand As Integer = 8
    Private Const ACMorO As Integer = 9
    Private Const ACControlTabIndex As Integer = 10
    Private Const ACCommandParameters As Integer = 11
    Private Const ACParentName As Integer = 12
    Private Const ACControlName As Integer = 13

    Private Const ACDOptionNumber As Integer = 0
    Private Const ACDValue As Integer = 1

    Private Const ACIconFolderClosed As Integer = 1
    Private Const ACIconFolderOpen As Integer = 3

    Private Const ACD1OptionNumber As Integer = 0
    Private Const ACD1OriginalValue As Integer = 1
    Private Const ACD1NewValue As Integer = 2
    Private Const ACD1Updated As Integer = 3
    Private Const ACD1Size As Integer = 3
    Private Const ACD1MultiTxtHeight As Integer = 70
    Private Const ACDocOpenDelay As Integer = 5036
    Private Const ACNumberMultiThread As Integer = 5147
    Private Const ACUserPassword As Integer = 5183
    Private Const ACEnableSSL As Integer = 5184
    Private Const kPaymentHubPassword As Integer = 5186
    Private Const kMarkDefaultCard As Integer = 5199
    Private Const kSystemPasscode As Integer = 5192
    Private Const kSystemGUID As Integer = 5191
    Private Const kAccountID As Integer = 5193
    Private Const kAccountPasscode As Integer = 5205
    Private Const kRefundPasscode As Integer = 5195
    Private Const kAuthenticationIntegration As Integer = 5249
    Private Const kRealm As Integer = 5250
    Private Const kClientId As Integer = 5251
    Private Const kClientSceret As Integer = 5252
    Private Const kUserName As Integer = 5253
    Private Const kPassword As Integer = 5254
    Private Const kAdminGroup As Integer = 5255
    Private Const kTokenUrl As Integer = 5256

    Private Const ACCommandGetDocument As String = "GetDocument"
    Private Const ACCommandClearDocument As String = "ClearDocument"
    Private Const ACCommandShowTemplateCode As String = "ShowTemplateCode"
    Private Const ACCommandLocateQAS As String = "Locate QAS"
    Private Const ACCommandClaimHelpFile As String = "ClaimHelpFile"
    Private Const ACCommandValidateNumeric As String = "ValidateNumeric"
    Private Const ACCommandValidateRequired As String = "ValidateRequired"
    Private Const ACCommandFormatCurrency As String = "FormatCurrency"
    Private Const ACCommandRegex As String = "Regex"
    Private Const ACCommandFailureMsg As String = "FailureMsg"
    Private Const ACCommandValidateNumericRange As String = "ValidateNumericRange"
    ' AMB 05/03/2003: PS220/123 - added for Claims Roadmaps development - browse for generic file
    Private Const ACCommandBrowseForFile As String = "BrowseForFile"
    Private Const ACCommandEnableDisableParent As String = "EnableDisableParent"
    Private Const ACCommandEnableDisableChild As String = "EnableDisableChild"
    Private Const ACCommandCCMTemplateSync As String = "CCMTemplateSync"

    'Start (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (not mentioned in the spec)
    Private Const ACOptionValue As Integer = 5064
    'End (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (not mentioned in the spec)

    ' AMB 05/03/2003: PS220/123 - if you need to add other file types,
    '                             add a constant and filter string here
    Private Const ACBrowseFileHelp As String = "HELP"
    Private Const ACBrowseFileHTML As String = "HTML"
    Private Const ACBrowseFileText As String = "TEXT"
    Private Const ACBrowseFileAll As String = "ALL"
    Private Const ACBrowseFileFilterHelp As String = "Help (*.hlp)|*.hlp|All files (*.*)|*.*"
    Private Const ACBrowseFileFilterHTML As String = "HTML Files (*.htm, *.html)|*.htm; *.html|All files (*.*)|*.*"
    Private Const ACBrowseFileFilterText As String = "Text Files (*.txt)|*.txt|All files (*.*)|*.*"
    Private Const ACBrowseFileFilterAll As String = "All files (*.*)|*.*"
    Private Const kAddressLookupInstallation As Integer = 13

    Private m_cLookupTables As CollectionWrapper
    Private m_cSystemOptionData As CollectionWrapper
    Private m_oBusiness As bSIROptions.Business

    Private m_oGisSchemeBusiness As bGISSchemeBusiness.Business
    Private m_oSchemeGroup As Object
    Private m_oPMLookup As Object

    Private m_vUserTables(,) As Object
    Private m_vDocCodes(,) As Object
    Private m_vUserGroups(,) As Object
    Private m_bCloseDatabase As Boolean
    Private m_vOptionsConfiguration(,) As Object
    Private m_lReturn As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_sUnderwritingOrAgency As String = ""
    Private m_bShowGeminiRenewals As Boolean
    Private m_bManageSalvageSet As Boolean
    Private m_bEnhancedChequeProduction As Boolean
    ' RDC 22/09/2005
    Private m_bArchiveSoftwareEnabled As Boolean
    Private m_bUnderwritingYearLabelling As Boolean
    Private m_bArchiveAsPDF As Boolean
    'S4BDAT003 DATASURE
    Private m_bEnhancedAccountingBasis As Boolean
    Private m_bIsActivateBroker As Boolean
    Public m_sRegex As String = ""
    Public m_sPassexpDuration As String = ""
    '' AC 28/08/2003 CQ1123
    '' might need to adjust this value if someone else has used the code
    '' while this CQ has been developed.
    Private Const DefaultDataModelLookup As String = "1051"
    Private m_bMultipleVersions As Boolean
    Private m_bCreateAllPartyHistory As Boolean

    Private m_bRI2007 As Boolean

    Private bUKPAFInstalled As Boolean = False
    Private m_sDynamicControlName As String = String.Empty
    Private m_bIsCCMDocProduction As Boolean = False
    Private m_bIsSharePoint As Boolean = False
    Private m_bSharepointOnline As Boolean
    Private m_bEnablePaymentHub As Boolean = False
    Private m_bEnableAuthenticationConfig As Boolean = False
    Public Delegate Sub RefreshAllCCMTemplates(nIndex As Integer)
    Dim frmCCMTemplateSyncNotification As New CCMTemplateSyncNotification
    Dim frmSPOValidateNotification As New CCMTemplateSyncNotification
    Dim frmAuthValidateNotification As New AuthenticationServerNotification
    Public m_oSharepoinOnlineBusiness As System.Object
    Private m_sSharePointOnlineInitialURL As String = String.Empty
    Private m_sSharePointOnlineInitialLibrary As String = String.Empty
    Private m_sSharePointOnlineInitialUserId As String = String.Empty
    Private m_sSharePointOnlineInitialPassword As String = String.Empty
    Private m_sSharePointOnlineAppClientId As String = String.Empty

    Private m_iBranchBaseCurrency As Integer = 0
    Private m_bAutoReconOnCancellation As Boolean = False

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property
    Public ReadOnly Property ErrorNumber() As Integer
        Get
            Return m_lErrorNumber
        End Get
    End Property
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: SetupTabs
    '
    ' Description:
    '
    ' History: 03/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function SetupTabs() As Integer

        Dim result As Integer = 0
        Dim oTopLevelNode, oParentNode, oNode, oFirstNode As TreeNode

        Dim vNodeArray(,) As Object = Nothing
        Dim vChildNodeArray(,) As Object = Nothing
        Dim sName As String = ""
        Dim lId, lUBound1, lUBound2 As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            tvwTabs.ImageList = ImageList1


            m_lReturn = m_oBusiness.GetFirstLevelNodes(r_vResultArray:=vNodeArray)

            With tvwTabs
                'Add top level mode
                oTopLevelNode = .Nodes.Add("", "System Options", ACIconFolderClosed - 1, ACIconFolderOpen - 1)

                oTopLevelNode.Expand()


                lUBound1 = vNodeArray.GetUpperBound(1)
                For i As Integer = 0 To lUBound1
                    'Add the first level nodes

                    sName = CStr(vNodeArray(1, i))

                    If sName = "Renewals" And Not m_bShowGeminiRenewals Then
                        'Do not show in this case
                    Else


                        lId = CInt(Conversion.Val(CStr(vNodeArray(0, i))))
                        'Developer Guide no.111
                        'oParentNode = .Nodes.Find(oTopLevelNode.Name, True)(0).Nodes.Add("", sName, ACIconFolderClosed, ACIconFolderOpen)
                        oParentNode = .Nodes.Find(oTopLevelNode.Name, True)(0).Nodes.Add("", sName, ACIconFolderClosed - 1, ACIconFolderOpen - 1)
                        '''''
                        oParentNode.Name = oParentNode.Text
                        ''''''
                        oParentNode.Expand()

                        If i = 0 Then
                            'Save the first one
                            oFirstNode = oParentNode
                        End If

                        'Get the second level nodes

                        m_lReturn = m_oBusiness.GetSecondLevelNodes(v_lId:=lId, r_vResultArray:=vChildNodeArray)
                        If Information.IsArray(vChildNodeArray) Then
                            'Add second level nodes to the treeview

                            lUBound2 = vChildNodeArray.GetUpperBound(1)
                            For j As Integer = 0 To lUBound2

                                sName = CStr(vChildNodeArray(1, j))

                                lId = CInt(Conversion.Val(CStr(vChildNodeArray(0, j))))
                                'Developer Guide no.111
                                'oNode = .Nodes.Find(oParentNode.Name, True)(0).Nodes.Add("", sName, ACIconFolderClosed, ACIconFolderOpen)
                                oNode = .Nodes.Find(oParentNode.Name, True)(0).Nodes.Add("", sName, ACIconFolderClosed - 1, ACIconFolderOpen - 1)
                                ''
                                oNode.Name = oNode.Text
                                '''''
                                oNode.Tag = CStr(lId)
                                oNode.Expand()
                            Next j
                        Else
                            oParentNode.Tag = CStr(lId)
                        End If
                    End If
                Next i

            End With


            'Modified by Archana Tokas on 5/12/2010 11:33:44 AM refer developer guide no. 35
            'oFirstNode.Selected = True

            oFirstNode.Checked = True
            tvwTabs.SelectedNode = oFirstNode
            tvwTabs.ExpandAll()
            ' do some initial setup of the form
            'Developer Guide no. 111
            'tabOptions.TabPages(1).Text = oFirstNode.Text
            tabOptions.TabPages(0).Text = oFirstNode.Text
            picContainer.BorderStyle = BorderStyle.None
            imgSplitter.Left = tvwTabs.Left + tvwTabs.Width

            tvwTabs_AfterSelect(tvwTabs, New TreeViewEventArgs(oFirstNode))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetupTabs Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetupTabs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadSystemOptionData
    '
    ' Description:
    '
    ' History: 04/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function LoadSystemOptionData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing
            Dim vArray(ACD1Size) As Object
            Dim sSQL As String = ""
            Dim lUBound As Integer


            m_lReturn = m_oBusiness.LoadSystemOptionData(r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadSystemOptionData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadSystemOptionData")
                Return result
            End If

            If Not Information.IsArray(vResultArray) Then
                Return result
            End If

            m_cSystemOptionData = New CollectionWrapper()


            lUBound = vResultArray.GetUpperBound(1)
            For i As Integer = 0 To lUBound


                If CDbl(vResultArray(ACDOptionNumber, i)) = 4001 Then
                    ' Option 4001 specifies if Gemini Renewals are required

                    If Conversion.Val(CStr(vResultArray(ACDValue, i))) = 1 Then
                        m_bShowGeminiRenewals = True
                    End If
                End If

                If ToSafeInteger(vResultArray(ACDOptionNumber, i)) = ACUserPassword OrElse ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kPassword OrElse ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kPaymentHubPassword OrElse ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kSystemPasscode OrElse ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kSystemGUID OrElse ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kAccountID OrElse ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kAccountPasscode OrElse ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kRefundPasscode Then
                    Dim sDecryptedPwd As String = GetOVal(vResultArray(ACDValue, i))
                    vResultArray(ACDValue, i) = sDecryptedPwd
                End If

                If ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kSystemPasscode Or
                    ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kPaymentHubPassword Or
                    ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kSystemGUID Or
                    ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kAccountID Or
                    ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kAccountPasscode Or
                    ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kRefundPasscode Then

                    Dim sPaymentHubWrapperParameters As String = ToSafeString(vResultArray(ACDValue, i))
                    If Not String.IsNullOrEmpty(sPaymentHubWrapperParameters) Then
                        Dim sDecryptedPaymentHub As String = bPMFunc.GetOVal(sPaymentHubWrapperParameters)
                        vResultArray(ACDValue, i) = sDecryptedPaymentHub
                    End If
                End If

                If ToSafeInteger(vResultArray(ACDOptionNumber, i)) = kMarkDefaultCard Then
                    If String.IsNullOrEmpty(ToSafeString(vResultArray(ACDValue, i))) Then
                        vResultArray(ACDValue, i) = "1"
                    End If
                End If

                vArray = Array.CreateInstance(GetType(Object), New Integer() {4}, New Integer() {0})

                vArray(ACD1OptionNumber) = vResultArray(ACDOptionNumber, i)

                vArray(ACD1OriginalValue) = vResultArray(ACDValue, i)

                vArray(ACD1Updated) = False

                m_lReturn = m_cSystemOptionData.Add(v_vItem:=vArray, v_vKey:=CStr(vResultArray(ACDOptionNumber, i)).Trim())

            Next i

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadSystemOptionData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadSystemOptionData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnloadControls
    '
    ' Description:
    '
    ' History: 03/12/2002 sj - Created.
    '
    ' AMB 02-Oct-03: 1.8.6 - made controls invisible before unloading
    '                        to stop the nasty screen redraw
    ' ***************************************************************** '
    Private Function UnloadControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lOptionIndex As Integer
            Dim sAdditionalData As String = ""
            'Developer Guide no,check in all for loops nothing conditions
            'Start
            For i As Integer = 1 To lblOption.Length - 1
                If Not lblOption(i) Is Nothing Then
                    lblOption(i).Visible = False
                    lblOption(i).Name = m_sDynamicControlName
                    ContainerHelper.UnloadControl(Me, "lblOption", i)
                End If
            Next i

            For i As Integer = 1 To txtOption.Length - 1
                If Not txtOption(i) Is Nothing Then
                    txtOption(i).Visible = False

                    m_lReturn = GetTagData(v_cControl:=txtOption(i), r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)

                    m_lReturn = CacheSystemOptionData(v_sKey:=CStr(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex)), v_sControlType:="TextBox", r_cControl:=txtOption(i), v_sCommand:=CStr(m_vOptionsConfiguration(ACCommand, lOptionIndex)), v_sAdditionalData:=sAdditionalData)

                    txtOption(i).Name = m_sDynamicControlName
                    ContainerHelper.UnloadControl(Me, "txtOption", i)
                End If
            Next i

            For i As Integer = 1 To chkOption.Length - 1
                If Not chkOption(i) Is Nothing Then
                    chkOption(i).Visible = False

                    ' lOptionIndex = chkOption(i).Tag

                    m_lReturn = GetTagData(v_cControl:=chkOption(i), r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)

                    m_lReturn = CacheSystemOptionData(v_sKey:=CStr(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex)), v_sControlType:="CheckBox", r_cControl:=chkOption(i))

                    chkOption(i).Name = m_sDynamicControlName
                    ContainerHelper.UnloadControl(Me, "chkOption", i)
                End If
            Next i

            For i As Integer = 1 To optOption.Length - 1
                If Not optOption(i) Is Nothing Then
                    optOption(i).Visible = False
                    ' lOptionIndex = chkOption(i).Tag

                    m_lReturn = GetTagData(v_cControl:=optOption(i), r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)

                    m_lReturn = CacheSystemOptionData(v_sKey:=CStr(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex)), v_sControlType:="OptionButton", r_cControl:=optOption(i))

                    optOption(i).Name = m_sDynamicControlName
                    ContainerHelper.UnloadControl(Me, "optOption", i)
                End If
            Next i

            For i As Integer = 1 To cboOption.Length - 1
                If Not cboOption(i) Is Nothing Then
                    cboOption(i).Visible = False

                    'lOptionIndex = cboOption(i).Tag
                    m_lReturn = GetTagData(v_cControl:=cboOption(i), r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)

                    m_lReturn = CacheSystemOptionData(v_sKey:=CStr(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex)), v_sControlType:="ComboBox", r_cControl:=cboOption(i))

                    cboOption(i).Name = m_sDynamicControlName
                    ContainerHelper.UnloadControl(Me, "cboOption", i)
                End If
            Next i

            For i As Integer = 1 To cmdOption.Length - 1
                If Not cmdOption(i) Is Nothing Then
                    cmdOption(i).Visible = False
                    cmdOption(i).Name = m_sDynamicControlName
                    ContainerHelper.UnloadControl(Me, "cmdOption", i)
                End If
                'end
            Next i

            For i As Integer = 1 To UctCompiledRuleOption.Length - 1
                If Not UctCompiledRuleOption(i) Is Nothing Then
                    m_lReturn = GetTagData(v_cControl:=UctCompiledRuleOption(i), r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)
                    If UctCompiledRuleOption(i).Visible = True Then
                        m_lReturn = CacheSystemOptionData(v_sKey:=CStr(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex)), v_sControlType:="uctCompiledRule", r_cControl:=UctCompiledRuleOption(i))
                    End If

                    UctCompiledRuleOption(i).Visible = False
                    UctCompiledRuleOption(i).Name = m_sDynamicControlName
                    ContainerHelper.UnloadControl(Me, "UctCompiledRuleOption", i)
                End If
            Next i

            For i As Integer = 1 To grpBoxOption.Length - 1
                If Not grpBoxOption(i) Is Nothing Then
                    grpBoxOption(i).Visible = False
                    grpBoxOption(i).Name = m_sDynamicControlName
                    ContainerHelper.UnloadControl(Me, "grpBoxOption", i)
                End If
            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnloadControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadControls
    '
    ' Description:
    '
    ' History: 03/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function LoadControls(ByRef v_lId As Integer) As Integer

        Dim result As Integer = 0
        Dim lLabelIndex, lTextBoxIndex, lCheckBoxIndex, lComboBoxIndex, lOptionButtonIndex, lSSCommandIndex, lUctCompiledRuleIndex, nGroupBoxIndex As Integer
        Dim bFound As Boolean
        Dim sLookupTable, sControlType As String
        Dim cFirstControl As Control
        Dim iFirstEnabledTag, lUBound As Integer
        Dim gParentGrpBox As GroupBox = New GroupBox
        Dim bArchiveToSharePoint As Boolean = False
        Dim sRSToleranceValue As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lLabelIndex = 1
            lTextBoxIndex = 1
            lCheckBoxIndex = 1
            lComboBoxIndex = 1
            lSSCommandIndex = 1
            lOptionButtonIndex = 1
            lUctCompiledRuleIndex = 1
            nGroupBoxIndex = 1

            If Not Information.IsArray(m_vOptionsConfiguration) Then
                Return result
            End If

            lUBound = m_vOptionsConfiguration.GetUpperBound(1)
            For i As Integer = 0 To lUBound

                If CDbl(m_vOptionsConfiguration(ACSystemOptionConfigurationGroupId, i)) = v_lId Then
                    bFound = True
                End If

                If CDbl(m_vOptionsConfiguration(ACSystemOptionConfigurationGroupId, i)) = v_lId And Not IsExcluded(i) Then

                    sControlType = CStr(m_vOptionsConfiguration(ACControlType, i)).Trim()

                    Select Case sControlType
                        Case "Label"
                            ContainerHelper.LoadControl(Me, "lblOption", lLabelIndex)
                            m_sDynamicControlName = lblOption(lLabelIndex).Name

                            With lblOption(lLabelIndex)
                                .Top = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlTop, i)))
                                .Left = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlLeft, i)))
                                .Height = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlHeight, i)))
                                .Width = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlWidth, i)))

                                .Tag = CStr(i)
                                .Text = Trim(CStr(m_vOptionsConfiguration(ACControlCaption, i)))
                                .Name = ToSafeString(m_vOptionsConfiguration(ACControlName, i))
                                If Not String.IsNullOrEmpty(ToSafeString(m_vOptionsConfiguration(ACParentName, i))) Then
                                    .Parent = gParentGrpBox
                                End If
                                .SendToBack()
                                If CStr(m_vOptionsConfiguration(ACMorO, i)) = "M" Then
                                    .Font = FontChangeBold(.Font, True)
                                    .Width *= 1.1
                                End If
                            End With

                            If lblOption(lLabelIndex).Text = "Media Type Validation" OrElse lblOption(lLabelIndex).Text = "Credit Control" OrElse lblOption(lLabelIndex).Text = "Chase Cycle" _
                                        OrElse lblOption(lLabelIndex).Text = "Payment Gateway" OrElse lblOption(lLabelIndex).Text = "Address Lookup Installation" _
                                        OrElse (m_bEnablePaymentHub AndAlso (lblOption(lLabelIndex).Text = "System User Name:" _
                                        OrElse lblOption(lLabelIndex).Text = "Password:" _
                                        OrElse lblOption(lLabelIndex).Text = "Customer:" _
                                        OrElse lblOption(lLabelIndex).Text = "Merchant ID:" _
                                        OrElse lblOption(lLabelIndex).Text = "Client Name:" _
                                        OrElse lblOption(lLabelIndex).Text = "Broker SCID:" _
                                        OrElse lblOption(lLabelIndex).Text = "Payment HUB Service URL:")) _
                                        OrElse (m_bEnableAuthenticationConfig AndAlso (lblOption(lLabelIndex).Text = "Realm:" _
                                        OrElse lblOption(lLabelIndex).Text = "Client ID:" _
                                        OrElse lblOption(lLabelIndex).Text = "Client Secret:" _
                                        OrElse lblOption(lLabelIndex).Text = "User Name:" _
                                        OrElse lblOption(lLabelIndex).Text = "Password:" _
                                        OrElse lblOption(lLabelIndex).Text = "Admin User Group:" _
                                        OrElse lblOption(lLabelIndex).Text = "Token Generation Url:")) Then
                                lblOption(lLabelIndex).Font = FontChangeBold(lblOption(lLabelIndex).Font, True)
                            End If

                            If lblOption(lLabelIndex).Text = "Total Incurred as Incurred less Receipts:" Then
                                If m_bRI2007 Then
                                    lblOption(lLabelIndex).Enabled = True
                                Else
                                    lblOption(lLabelIndex).Enabled = False
                                End If
                            End If
                            lLabelIndex += 1
                        Case "ComboBox"
                            ContainerHelper.LoadControl(Me, "cboOption", lComboBoxIndex)
                            m_sDynamicControlName = cboOption(lComboBoxIndex).Name

                            With cboOption(lComboBoxIndex)
                                .Top = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlTop, i)))
                                .Left = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlLeft, i)))
                                ' .Height = m_vOptionsConfiguration(ACControlHeight, i)
                                .Width = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlWidth, i)))
                                .Tag = CStr(i)
                                .TabIndex = CInt(m_vOptionsConfiguration(ACControlTabIndex, i))
                                .Name = ToSafeString(m_vOptionsConfiguration(ACControlName, i))
                                If Not String.IsNullOrEmpty(ToSafeString(m_vOptionsConfiguration(ACParentName, i))) Then
                                    .Parent = gParentGrpBox
                                End If
                                .BringToFront()
                            End With

                            sLookupTable = CStr(m_vOptionsConfiguration(ACLookupTable, i)).Trim()
                            If sLookupTable <> "" Then
                                m_lReturn = GetLookupTable(v_sKey:=sLookupTable, r_cComboBox:=cboOption(lComboBoxIndex))
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupTable Failed for " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControls")
                                    Return result
                                End If

                                m_lReturn = GetSystemOptionData(v_sKey:=CStr(m_vOptionsConfiguration(ACOptionNumber, i)), v_sControlType:=sControlType, r_cControl:=cboOption(lComboBoxIndex))
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionData Failed for option " &
                                                       CStr(m_vOptionsConfiguration(ACOptionNumber, i)), vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControls")
                                    Return result
                                End If
                            End If

                            If CDbl(m_vOptionsConfiguration(ACControlTabIndex, i)) = iFirstEnabledTag Then
                                If cboOption(lComboBoxIndex).Enabled Then
                                    cFirstControl = cboOption(lComboBoxIndex)
                                Else
                                    iFirstEnabledTag += 1
                                End If
                            End If

                            If CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = GeneralConst.kSystemOptionRuleTypeCreditControl OrElse
                                CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = GeneralConst.kSystemOptionRuleTypePaymentGateway OrElse
                                CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = GeneralConst.kSystemOptionRuleTypeChaseCycle OrElse
                                CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = GeneralConst.kSystemOptionRuleTypeAddressLookup Then
                                If cboOption(lComboBoxIndex).SelectedIndex = -1 Then
                                    cboOption(lComboBoxIndex).SelectedIndex = 0
                                End If
                            End If

                            If CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = GeneralConst.kSystemOptionRuleTypeAddressLookup Then
                                If bUKPAFInstalled Then
                                    cboOption(lComboBoxIndex).Enabled = True
                                Else
                                    cboOption(lComboBoxIndex).Enabled = False
                                End If
                            End If

                            'If m_sDynamicControlName = "cboOption10" Then
                            '    If cboOption(lComboBoxIndex).SelectedIndex = 3 Then
                            '        bArchiveToSharePoint = True
                            '    End If
                            'End If

                            cboOption(lComboBoxIndex).Visible = True
                            lComboBoxIndex += 1

                        Case "CheckBox"

                            ContainerHelper.LoadControl(Me, "chkOption", lCheckBoxIndex)
                            m_sDynamicControlName = chkOption(lCheckBoxIndex).Name

                            With chkOption(lCheckBoxIndex)
                                .Top = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlTop, i)))
                                .Left = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlLeft, i)))
                                .Height = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlHeight, i)))
                                If .Height < 17 Then
                                    .Height = 17
                                End If
                                .Width = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlWidth, i)))
                                .Tag = CStr(i)
                                .TabIndex = CInt(m_vOptionsConfiguration(ACControlTabIndex, i))
                                .Text = Trim(CStr(m_vOptionsConfiguration(ACControlCaption, i)))
                                .Name = ToSafeString(m_vOptionsConfiguration(ACControlName, i))
                                If Not String.IsNullOrEmpty(ToSafeString(m_vOptionsConfiguration(ACParentName, i))) Then
                                    .Parent = gParentGrpBox
                                End If
                                If .Text = "Recalculate pro-rata reinsurance rates during MTA:" Then
                                    .Height = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlHeight, i))) + 10
                                End If

                                Select Case m_vOptionsConfiguration(ACMorO, i)
                                    Case "M"
                                        .Font = VB6.FontChangeBold(.Font, True)
                                    Case "D"
                                        .Enabled = False
                                End Select
                                If .Name = "chkOption5207" Then
                                    .CheckAlign = ContentAlignment.MiddleRight
                                End If

                                .BringToFront()
                            End With

                            m_lReturn = GetSystemOptionData(v_sKey:=CStr(m_vOptionsConfiguration(ACOptionNumber, i)), v_sControlType:=sControlType, r_cControl:=chkOption(lCheckBoxIndex))

                            If m_vOptionsConfiguration(ACOptionNumber, i) = ACUserPassword OrElse
                                m_vOptionsConfiguration(ACOptionNumber, i) = kSystemPasscode OrElse
                                 m_vOptionsConfiguration(ACOptionNumber, i) = kSystemGUID OrElse
                                  m_vOptionsConfiguration(ACOptionNumber, i) = kAccountID OrElse
                                   m_vOptionsConfiguration(ACOptionNumber, i) = kAccountPasscode OrElse
                                   m_vOptionsConfiguration(ACOptionNumber, i) = kPaymentHubPassword OrElse
                                m_vOptionsConfiguration(ACOptionNumber, i) = kRefundPasscode Then

                                txtOption(lTextBoxIndex).PasswordChar = "*"
                            End If
                            If m_vOptionsConfiguration(ACOptionNumber, i) = kPassword Then
                                txtOption(lTextBoxIndex).PasswordChar = "*"
                            End If
                            chkOption(lCheckBoxIndex).Visible = True

                            If CStr(m_vOptionsConfiguration(ACOptionNumber, i)) = "10" Then
                                m_bArchiveSoftwareEnabled = (chkOption(lCheckBoxIndex).CheckState = CheckState.Checked)
                            End If

                            ' system option (5012) underwriting year mandatory on cash and journals
                            ' is dependant on product option underwriting year labelling being
                            ' switched on...

                            ' if this is option (5012)
                            If CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = 5012 Then
                                ' if the product option isnt switched on
                                If Not m_bUnderwritingYearLabelling Then
                                    ' hide the system option
                                    chkOption(lCheckBoxIndex).Visible = False
                                End If
                            End If

                            'Start (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (not mentioned in the spec)
                            If CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = ACOptionValue Then
                                chkOption(lCheckBoxIndex).Visible = False
                            End If
                            'End (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (not mentioned in the spec)

                            'S4B Claim Enhancements - ensure disabled options are ticked
                            If m_sUnderwritingOrAgency = "A" Then
                                Select Case gPMFunctions.ToSafeLong(CStr(m_vOptionsConfiguration(ACOptionNumber, i)), 0)
                                    Case 2022, 2023, 2025, 2031
                                        chkOption(lCheckBoxIndex).CheckState = CheckState.Checked
                                End Select
                            End If

                            ' WPR 63
                            If CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = 5089 Then
                                'If chkOption(lCheckBoxIndex).CheckState = CheckState.Checked Then
                                'Since the Quote Versioning is not implementing till now So disabling this option.
                                'DataScript Sets this option OFF if anyclient have ON.
                                chkOption(lCheckBoxIndex).Enabled = False

                                'End If
                            End If

                            'WPR55
                            If CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = 5260 Then

                                If m_bRI2007 = True Then
                                    chkOption(lCheckBoxIndex).Visible = True
                                Else
                                    chkOption(lCheckBoxIndex).Visible = True
                                End If
                            End If

                            If CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = 5029 Then
                                chkOption(lCheckBoxIndex).Checked = False
                                chkOption(lCheckBoxIndex).Visible = False
                            End If

                            If CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = 5263 Then
                                ' Read RI 2007 Product Option to set default state
                                If m_bRI2007 Then
                                    chkOption(lCheckBoxIndex).Enabled = True
                                Else
                                    chkOption(lCheckBoxIndex).CheckState = CheckState.Unchecked
                                    chkOption(lCheckBoxIndex).Enabled = False
                                End If
                            End If

                            If CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = 5200 Then
                                If chkOption(lCheckBoxIndex).CheckState = CheckState.Checked Then
                                    m_bEnablePaymentHub = True
                                Else
                                    m_bEnablePaymentHub = False
                                End If
                            End If
                            If CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = 5249 Then
                                If chkOption(lCheckBoxIndex).CheckState = CheckState.Checked Then
                                    m_bEnableAuthenticationConfig = True
                                Else
                                    m_bEnableAuthenticationConfig = False
                                End If
                            End If
                            m_bAutoReconOnCancellation = False
                            If CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = 5246 Then
                                If chkOption(lCheckBoxIndex).CheckState = CheckState.Checked Then
                                    m_bAutoReconOnCancellation = True
                                End If

                            End If

                            'Add the addhandler for click event
                            AddHandler chkOption(lCheckBoxIndex).CheckStateChanged, AddressOf chkOption_CheckStateChanged
                            AddHandler chkOption(lCheckBoxIndex).Click, AddressOf chkOption_Click
                            lCheckBoxIndex += 1

                        Case "TextBox"
                            ContainerHelper.LoadControl(Me, "txtOption", lTextBoxIndex)
                            m_sDynamicControlName = txtOption(lTextBoxIndex).Name

                            With txtOption(lTextBoxIndex)
                                .Top = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlTop, i)))
                                .Left = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlLeft, i)))
                                '.Height = m_vOptionsConfiguration(ACControlHeight, i)
                                .Width = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlWidth, i)))
                                .Tag = CStr(i)
                                .TabIndex = CInt(m_vOptionsConfiguration(ACControlTabIndex, i))
                                If CStr(m_vOptionsConfiguration(ACMorO, i)) = "D" Then
                                    .Enabled = False
                                End If
                                .Name = ToSafeString(m_vOptionsConfiguration(ACControlName, i))
                                If Not String.IsNullOrEmpty(ToSafeString(m_vOptionsConfiguration(ACParentName, i))) Then
                                    .Parent = gParentGrpBox
                                End If
                                .BringToFront()
                            End With



                            m_lReturn = GetSystemOptionData(v_sKey:=CStr(m_vOptionsConfiguration(ACOptionNumber, i)), v_sControlType:=sControlType, r_cControl:=txtOption(lTextBoxIndex), v_sCommand:=CStr(m_vOptionsConfiguration(ACCommand, i)))

                            If CDbl(m_vOptionsConfiguration(ACControlTabIndex, i)) = iFirstEnabledTag Then
                                If txtOption(lTextBoxIndex).Enabled Then
                                    'Modified by Archana Tokas on 5/12/2010 11:35:59 AM to be checked later todolist
                                    'cFirstControl = txtOption(lTextBoxIndex)
                                    cFirstControl = txtOption(lTextBoxIndex)
                                Else
                                    iFirstEnabledTag += 1
                                End If
                            End If

                            If txtOption(lTextBoxIndex).Name.ToUpper.Trim = "TXTRSTOLERANCE" Then
                                sRSToleranceValue = txtOption(lTextBoxIndex).Text
                            End If

                            If txtOption(lTextBoxIndex).Name = "txtOption5185" _
                       OrElse txtOption(lTextBoxIndex).Name = "txtOption5186" _
                       OrElse txtOption(lTextBoxIndex).Name = "txtOption5187" _
                       OrElse txtOption(lTextBoxIndex).Name = "txtOption5188" _
                       OrElse txtOption(lTextBoxIndex).Name = "txtOption5189" _
                       OrElse txtOption(lTextBoxIndex).Name = "txtOption5190" Then
                                If m_bEnablePaymentHub Then
                                    txtOption(lTextBoxIndex).Enabled = True
                                Else
                                    txtOption(lTextBoxIndex).Enabled = False
                                End If
                            End If
                            If txtOption(lTextBoxIndex).Name = "txtOption5250" _
                       OrElse txtOption(lTextBoxIndex).Name = "txtOption5251" _
                       OrElse txtOption(lTextBoxIndex).Name = "txtOption5252" _
                       OrElse txtOption(lTextBoxIndex).Name = "txtOption5253" _
                       OrElse txtOption(lTextBoxIndex).Name = "txtOption5254" _
                       OrElse txtOption(lTextBoxIndex).Name = "txtOption5255" _
                       OrElse txtOption(lTextBoxIndex).Name = "txtOption5256" Then
                                If m_bEnableAuthenticationConfig Then
                                    txtOption(lTextBoxIndex).Enabled = True
                                Else
                                    txtOption(lTextBoxIndex).Enabled = False
                                End If
                            End If
                            If txtOption(lTextBoxIndex).Name = "txtOption5254" Then
                                txtOption(lTextBoxIndex).PasswordChar = "*"
                            End If
                            If txtOption(lTextBoxIndex).Name = "txtOption5186" OrElse
                            txtOption(lTextBoxIndex).Name = "txtOption5191" OrElse
                            txtOption(lTextBoxIndex).Name = "txtOption5192" OrElse
                            txtOption(lTextBoxIndex).Name = "txtOption5193" OrElse
                            txtOption(lTextBoxIndex).Name = "txtOption5205" OrElse
                            txtOption(lTextBoxIndex).Name = "txtOption5195" OrElse
                            txtOption(lTextBoxIndex).Name = "txtUserPassword" Then
                                txtOption(lTextBoxIndex).PasswordChar = "*"

                            End If

                            If txtOption(lTextBoxIndex).Name = "txtOption5247" OrElse
                                txtOption(lTextBoxIndex).Name = "txtOption5248" Then
                                If m_bAutoReconOnCancellation Then
                                    txtOption(lTextBoxIndex).Enabled = True
                                Else
                                    txtOption(lTextBoxIndex).Enabled = False
                                End If
                            End If

                            AddHandler txtOption(lTextBoxIndex).KeyPress, AddressOf txtOption_KeyPress
                            AddHandler txtOption(lTextBoxIndex).Leave, AddressOf txtOption_Leave
                            txtOption(lTextBoxIndex).Visible = True
                            lTextBoxIndex += 1

                        Case "SSCommand", "Command"
                            ' AMB 02-Oct-03: 1.8.6 - replace use of SSCommandButton with normal VB command button
                            ContainerHelper.LoadControl(Me, "cmdOption", lSSCommandIndex)
                            m_sDynamicControlName = cmdOption(lSSCommandIndex).Name

                            With cmdOption(lSSCommandIndex)
                                .Top = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlTop, i)))
                                .Left = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlLeft, i)))
                                '.Height = m_vOptionsConfiguration(ACControlHeight, i)
                                .Width = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlWidth, i)))
                                .Tag = CStr(i)
                                .Text = Trim(CStr(m_vOptionsConfiguration(ACControlCaption, i)))
                                .TabIndex = CInt(m_vOptionsConfiguration(ACControlTabIndex, i))
                                .Name = ToSafeString(m_vOptionsConfiguration(ACControlName, i))
                                If Not String.IsNullOrEmpty(ToSafeString(m_vOptionsConfiguration(ACParentName, i))) Then
                                    .Parent = gParentGrpBox
                                End If
                                .BringToFront()
                            End With

                            If CDbl(m_vOptionsConfiguration(ACControlTabIndex, i)) = iFirstEnabledTag Then
                                If cmdOption(lSSCommandIndex).Enabled Then
                                    'Modified by Archana Tokas on 5/12/2010 11:35:59 AM to be checked later todolist
                                    'cFirstControl = cmdOption(lSSCommandIndex)
                                    cFirstControl = cmdOption(lSSCommandIndex)
                                Else
                                    iFirstEnabledTag += 1
                                End If
                            End If
                            'Add the addhandler for click event
                            AddHandler cmdOption(lSSCommandIndex).Click, AddressOf cmdOption_Click
                            cmdOption(lSSCommandIndex).Visible = True
                            lSSCommandIndex += 1

                        Case "OptionButton"
                            ContainerHelper.LoadControl(Me, "optOption", lOptionButtonIndex)
                            m_sDynamicControlName = optOption(lOptionButtonIndex).Name

                            With optOption(lOptionButtonIndex)
                                .Top = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlTop, i)))
                                .Left = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlLeft, i)))
                                .Height = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlHeight, i)))
                                .Width = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlWidth, i)))
                                .Tag = CStr(i)

                                .TabIndex = CInt(m_vOptionsConfiguration(ACControlTabIndex, i))
                                .Text = Trim(CStr(m_vOptionsConfiguration(ACControlCaption, i)))
                                .Name = ToSafeString(m_vOptionsConfiguration(ACControlName, i))
                                .BringToFront()
                                If CStr(m_vOptionsConfiguration(ACMorO, i)) = "M" Then
                                    .Font = VB6.FontChangeBold(.Font, True)
                                End If

                            End With

                            m_lReturn = GetSystemOptionData(v_sKey:=CStr(m_vOptionsConfiguration(ACOptionNumber, i)), v_sControlType:=sControlType, r_cControl:=optOption(lOptionButtonIndex))

                            If CDbl(m_vOptionsConfiguration(ACControlTabIndex, i)) = iFirstEnabledTag Then
                                If optOption(lOptionButtonIndex).Enabled Then
                                    'Modified by Archana Tokas on 5/12/2010 11:35:59 AM to be checked later todolist
                                    'cFirstControl = optOption(lOptionButtonIndex)
                                    cFirstControl = optOption(lOptionButtonIndex)
                                Else
                                    iFirstEnabledTag += 1
                                End If
                            End If

                            optOption(lOptionButtonIndex).Visible = True
                            lOptionButtonIndex += 1
                        Case "uctCompiledRule"
                            ContainerHelper.LoadControl(Me, "UctCompiledRuleOption", lUctCompiledRuleIndex)
                            m_sDynamicControlName = UctCompiledRuleOption(lUctCompiledRuleIndex).Name

                            With UctCompiledRuleOption(lUctCompiledRuleIndex)
                                .Top = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlTop, i)))
                                .Left = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlLeft, i)))
                                .Height = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlHeight, i)))
                                .Width = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlWidth, i)))
                                .Tag = CStr(i)
                                .Text = Trim(CStr(m_vOptionsConfiguration(ACControlCaption, i)))
                                .Name = ToSafeString(m_vOptionsConfiguration(ACControlName, i))
                                .SendToBack()
                                .Load()
                                .bEnterOnlyAssemblyName = IIf(CStr(m_vOptionsConfiguration(ACOptionNumber, i)) = GeneralConst.kSystemOptionCompiledRuleMediaType, True, False)
                            End With

                            m_lReturn = GetSystemOptionData(v_sKey:=CStr(m_vOptionsConfiguration(ACOptionNumber, i)), v_sControlType:=sControlType, r_cControl:=UctCompiledRuleOption(lUctCompiledRuleIndex))
                            If CDbl(m_vOptionsConfiguration(ACControlTabIndex, i)) = iFirstEnabledTag Then
                                If UctCompiledRuleOption(lUctCompiledRuleIndex).Enabled Then
                                    cFirstControl = UctCompiledRuleOption(lUctCompiledRuleIndex)
                                Else
                                    iFirstEnabledTag += 1
                                End If
                            End If

                            UctCompiledRuleOption(lUctCompiledRuleIndex).Visible = True
                            lUctCompiledRuleIndex += 1

                        Case "GroupBox"
                            ContainerHelper.LoadControl(Me, "grpBoxOption", nGroupBoxIndex)
                            m_sDynamicControlName = grpBoxOption(nGroupBoxIndex).Name

                            With grpBoxOption(nGroupBoxIndex)
                                .Top = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlTop, i)))
                                .Left = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlLeft, i)))
                                .Height = VB6.TwipsToPixelsY(CDbl(m_vOptionsConfiguration(ACControlHeight, i)))
                                .Width = VB6.TwipsToPixelsX(CDbl(m_vOptionsConfiguration(ACControlWidth, i)))
                                .Tag = CStr(i)
                                .Text = Trim(CStr(m_vOptionsConfiguration(ACControlCaption, i)))
                                .Name = ToSafeString(m_vOptionsConfiguration(ACControlName, i))
                                .SendToBack()
                            End With

                            grpBoxOption(nGroupBoxIndex).Visible = True
                            nGroupBoxIndex += 1

                    End Select
                Else

                    If bFound And CDbl(m_vOptionsConfiguration(ACSystemOptionConfigurationGroupId, i)) <> v_lId Then
                        If lCheckBoxIndex > 1 Then
                            For lLoopy As Integer = 1 To lCheckBoxIndex - 1
                                chkOption_CheckStateChanged(chkOption(lLoopy), New EventArgs())
                            Next lLoopy
                        End If
                        result = gPMConstants.PMEReturnCode.PMTrue
                    End If
                End If
            Next i

            Dim nSelectedItemId As Integer
            Dim nSelectedIndex As Integer
            lUBound = m_vOptionsConfiguration.GetUpperBound(1)
            For i As Integer = 0 To lUBound
                If Not String.IsNullOrEmpty(m_vOptionsConfiguration(ACOptionNumber, i)) Then
                    If CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = GeneralConst.kSystemOptionRuleTypePaymentGateway Then
                        If lComboBoxIndex > 1 Then
                            For lLoop As Integer = 1 To lComboBoxIndex - 1
                                If cboOption(lLoop).Name = "cboOption5159" Then
                                    nSelectedItemId = DirectCast(cboOption(lLoop).SelectedItem, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemData
                                    SetLableRuleTypeVal(nSelectedItemId, GeneralConst.kSystemOptionRuleTypePaymentGateway)
                                    ShowHideRuleFileTxt(nSelectedItemId, GeneralConst.kSystemOptionRuleTypePaymentGateway)
                                    ShowHideRuleFileBtn(nSelectedItemId, GeneralConst.kSystemOptionRuleTypePaymentGateway)
                                    ShowHideRuleFileUct(nSelectedItemId, GeneralConst.kSystemOptionRuleTypePaymentGateway)
                                End If
                            Next
                        End If
                    ElseIf CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = GeneralConst.kSystemOptionRuleTypeCreditControl Then
                        If lComboBoxIndex > 1 Then
                            For lLoop As Integer = 1 To lComboBoxIndex - 1
                                If cboOption(lLoop).Name = "cboOption5160" Then
                                    nSelectedItemId = DirectCast(cboOption(lLoop).SelectedItem, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemData
                                    SetLableRuleTypeVal(nSelectedItemId, GeneralConst.kSystemOptionRuleTypeCreditControl)
                                    ShowHideRuleFileTxt(nSelectedItemId, GeneralConst.kSystemOptionRuleTypeCreditControl)
                                    ShowHideRuleFileBtn(nSelectedItemId, GeneralConst.kSystemOptionRuleTypeCreditControl)
                                    ShowHideRuleFileUct(nSelectedItemId, GeneralConst.kSystemOptionRuleTypeCreditControl)
                                End If
                            Next
                        End If
                    ElseIf CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = GeneralConst.kSystemOptionRuleTypeChaseCycle Then
                        If lComboBoxIndex > 1 Then
                            For lLoop As Integer = 1 To lComboBoxIndex - 1
                                If cboOption(lLoop).Name = "cboOption5161" Then
                                    nSelectedItemId = DirectCast(cboOption(lLoop).SelectedItem, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemData
                                    SetLableRuleTypeVal(nSelectedItemId, GeneralConst.kSystemOptionRuleTypeChaseCycle)
                                    ShowHideRuleFileTxt(nSelectedItemId, GeneralConst.kSystemOptionRuleTypeChaseCycle)
                                    ShowHideRuleFileBtn(nSelectedItemId, GeneralConst.kSystemOptionRuleTypeChaseCycle)
                                    ShowHideRuleFileUct(nSelectedItemId, GeneralConst.kSystemOptionRuleTypeChaseCycle)
                                End If
                            Next
                        End If
                    ElseIf CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = GeneralConst.kSystemOptionRuleTypeAddressLookup Then
                        If lComboBoxIndex > 1 Then
                            For lLoop As Integer = 1 To lComboBoxIndex - 1
                                If cboOption(lLoop).Name = "cboOption5162" Then
                                    nSelectedItemId = DirectCast(cboOption(lLoop).SelectedItem, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemData
                                    SetLableRuleTypeVal(nSelectedItemId, GeneralConst.kSystemOptionRuleTypeAddressLookup)
                                    ShowHideRuleFileTxt(nSelectedItemId, GeneralConst.kSystemOptionRuleTypeAddressLookup)
                                    ShowHideRuleFileBtn(nSelectedItemId, GeneralConst.kSystemOptionRuleTypeAddressLookup)
                                    ShowHideRuleFileUct(nSelectedItemId, GeneralConst.kSystemOptionRuleTypeAddressLookup)
                                End If
                            Next
                        End If
                    ElseIf CDbl(m_vOptionsConfiguration(ACOptionNumber, i)) = GeneralConst.kSystemOptionMediaTypeIsCompliedRuleEnabled Then
                        If lCheckBoxIndex > 1 Then
                            For lLoop As Integer = 1 To lCheckBoxIndex - 1
                                If chkOption(lLoop).Name = "chkOption5149" Then
                                    ShowHideOnMediaTypeValidation(lLoop)
                                End If
                            Next
                        End If
                    End If
                End If
            Next

            ' psuedo-click each checkbox so that the enable/disable kicks off
            If lCheckBoxIndex > 1 Then
                For lLoopy As Integer = 1 To lCheckBoxIndex - 1
                    chkOption_CheckStateChanged(chkOption(lLoopy), New EventArgs())
                Next lLoopy
            End If

            For Each grp As GroupBox In grpBoxOption
                If Not grp Is Nothing Then
                    If Not String.IsNullOrEmpty(grp.Name) Then
                        If grp.Name = "grpOptionDocProduction" Then
                            If lLabelIndex > 1 Then
                                For lLoop As Integer = 1 To lLabelIndex - 1
                                    If TypeOf lblOption(lLoop).Parent Is GroupBox Then
                                        grp.Controls.Add(lblOption(lLoop))
                                    End If
                                Next
                            End If
                            If lComboBoxIndex > 1 Then
                                For lLoop As Integer = 1 To lComboBoxIndex - 1
                                    If TypeOf cboOption(lLoop).Parent Is GroupBox Then
                                        grp.Controls.Add(cboOption(lLoop))
                                    End If
                                Next
                            End If
                            If lTextBoxIndex > 1 Then
                                For lLoop As Integer = 1 To lTextBoxIndex - 1
                                    If TypeOf txtOption(lLoop).Parent Is GroupBox Then
                                        grp.Controls.Add(txtOption(lLoop))
                                    End If
                                Next
                            End If
                            If lCheckBoxIndex > 1 Then
                                For lLoop As Integer = 1 To lCheckBoxIndex - 1
                                    If TypeOf chkOption(lLoop).Parent Is GroupBox Then
                                        grp.Controls.Add(chkOption(lLoop))
                                    End If
                                Next
                            End If
                            If lSSCommandIndex > 1 Then
                                For lLoop As Integer = 1 To lSSCommandIndex - 1
                                    If TypeOf cmdOption(lLoop).Parent Is GroupBox Then
                                        grp.Controls.Add(cmdOption(lLoop))
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If
            Next

            ''Enable/Disable controls based on Doc production ddl selection
            If lComboBoxIndex > 1 Then
                For lLoop As Integer = 1 To lComboBoxIndex - 1
                    If Not cboOption(lLoop) Is Nothing Then
                        If cboOption(lLoop).Name = "cmbOption5163" Then
                            nSelectedIndex = cboOption(lLoop).SelectedIndex
                            EnableDisableControlsForCCM(nSelectedIndex)
                        End If
                        If cboOption(lLoop).Name = "cmbOption5171" Then
                            If cboOption(lLoop).SelectedIndex = -1 Then
                                cboOption(lLoop).SelectedIndex = 0
                            End If
                        End If
                        If cboOption(lLoop).Name = "cboOption5243" Then
                            If Not String.IsNullOrEmpty(sRSToleranceValue) Then
                                cboOption(lLoop).Enabled = True
                            Else
                                cboOption(lLoop).Enabled = False
                            End If
                        End If
                    End If
                Next
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    Private Function IsExcluded(ByVal v_iIndex As Integer) As Boolean


        Dim result As Boolean = False
        'Developer Guide no. 248
        'Select Case m_vOptionsConfiguration(ACOptionNumber, v_iIndex)
        Select Case Conversion.Val(m_vOptionsConfiguration(ACOptionNumber, v_iIndex))
            'Manage salvage enabled
            Case 2016
                result = (Not m_bManageSalvageSet)
                'Enhanced Cheque Production
                'DC150606 PN28922 5006 option is for "Show Agent Commission On Remittance Advice"
                '                 which doesnt relate to cheque production
                '                   kept in for underwriting tho incase option for them
            Case 158, 5003 ', 5006 'PN22008  PN22554 PN24841
                result = (Not m_bEnhancedChequeProduction)
            Case 5006
                If m_sUnderwritingOrAgency = "U" Then
                    result = (Not m_bEnhancedChequeProduction)
                End If
            Case 4012 'S4BDAT003
                result = (Not m_bEnhancedAccountingBasis)
            Case 5029
                result = (Not m_bIsActivateBroker)
            Case Else
                result = False

        End Select

        Return result
    End Function
    ' ***************************************************************** '
    '
    ' Name: BuildLookupTables
    '
    ' Description:
    '
    ' History: 04/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function BuildLookupTables() As Integer

        Dim result As Integer = 0
        Dim vArray As Array
        Dim vResultArray(,) As Object = Nothing
        Dim lUBound, lLBound As Integer

        Const kAddFailedEmailTaskManagerGroup As String = "5068"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_cLookupTables = New CollectionWrapper()
            'Option 1
            vArray = Array.CreateInstance(GetType(Object), New Integer() {4}, New Integer() {0})
            vArray(0) = "UK formats with IPT"
            vArray(1) = "Overseas format"
            vArray(2) = "Republic Of Ireland"
            vArray(3) = "UK formats without IPT"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1")

            'Option 2
            vArray = Array.CreateInstance(GetType(Object), New Integer() {7}, New Integer() {0})
            vArray(0) = "Client sequence"
            vArray(1) = "Renewal date sequence"
            vArray(2) = "Insurer sequence"
            vArray(3) = "Brokerage code sequence"
            vArray(4) = "Consultant sequence"
            vArray(5) = "Account Handler sequence"
            vArray(6) = "Agent sequence"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2")

            'Option 5
            vArray = Array.CreateInstance(GetType(Object), New Integer() {4}, New Integer() {0})
            vArray(0) = "At end of transaction, no daily report"
            vArray(1) = "At end of transaction, daily report"
            vArray(2) = "Additional reports for client and insurer"
            vArray(3) = "Daily report only"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5")

            'Option 8
            vArray = Array.CreateInstance(GetType(Object), New Integer() {4}, New Integer() {0})
            vArray(0) = "Statement only"
            vArray(1) = "Letter only"
            vArray(2) = "Both Statement and Letter"
            vArray(3) = "Neither Statement nor Letter"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="8")

            'Option 9
            vArray = Array.CreateInstance(GetType(Object), New Integer() {3}, New Integer() {0})
            vArray(0) = "No tracking of discounts"
            vArray(1) = "Track discounts with sub-agent accounting"
            vArray(2) = "track discounts without sub-agent accounting"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="9")

            'Option 10
            vArray = Array.CreateInstance(GetType(Object), New Integer() {3}, New Integer() {0})
            vArray(0) = "Spooler Only"
            vArray(1) = "Documaster"
            vArray(2) = "Microsoft Sharepoint 2019 (or later)"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="10")

            'Option 13
            vArray = Array.CreateInstance(GetType(Object), New Integer() {5}, New Integer() {0})
            vArray(0) = ACQASNotInstalled ' "No Address Lookup installed"
            vArray(1) = ACQASRapid ' "UK QAS Rapid installed"
            vArray(2) = ACQASPro ' "UK QAS Pro installed"
            vArray(3) = ACQASNames ' "UK QAS Names installed"
            vArray(4) = ACPAFWrapper ' "UK QAS PAF Wrapper installed"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="13")

            'Option 16
            vArray = Array.CreateInstance(GetType(Object), New Integer() {4}, New Integer() {0})
            vArray(0) = "As Debited"
            vArray(1) = "When Client Pays"
            vArray(2) = "When Insurer Settled"
            vArray(3) = "When Policy Effective"
            'eck100203
            'FSA Phase 3.2 Remove
            '    vArray(4) = "When Client Pays incl Direct"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="16")
            'Datasure New fee movement

            If m_sUnderwritingOrAgency = "U" Then
                'Option 5005
                vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})
                vArray(0) = "When Client Pays"
                vArray(1) = "When Insurer Settled"

                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5005")
            End If

            'Hidden Option 72 - enhanced cheque production
            If m_bEnhancedChequeProduction Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {4}, New Integer() {0})
                vArray(0) = ACChequeProductionNotInstalled
                vArray(1) = ACEnhChequeProductionCM
                vArray(2) = ACEnhChequeProductionInHouse
                vArray(3) = ACEnhChequeProductionExport
            Else
                vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})
                vArray(0) = ACChequeProductionNotInstalled
                vArray(1) = ACChequeProductionInstalled
            End If

            'System Option 60 - cheque production
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="60")

            'S4BDAT003 DataSure
            If m_bEnhancedAccountingBasis Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})
                vArray(0) = "Invoice Basis"
                vArray(1) = "Cash Basis"
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="4012")
            End If

            If m_sUnderwritingOrAgency = "U" Then

                vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})
                vArray(0) = "By Transaction Date"
                vArray(1) = "By Cover Start Date"
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1040")

                vArray = Array.CreateInstance(GetType(Object), New Integer() {4}, New Integer() {0})
                vArray(0) = "By Transaction Date"
                vArray(1) = "By Cover Start Date"
                vArray(2) = "By Risk Inception Date"
                vArray(3) = "By Inception Date TPI"
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5019")

                '1004
                vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})
                vArray(0) = "Period Basis"
                vArray(1) = "Effective Month Basis"
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1004")

                '1005
                vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})
                vArray(0) = "Period Basis"
                vArray(1) = "Effective Month Basis"
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1005")

                '1006
                vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})
                vArray(0) = "Period Basis"
                vArray(1) = "Effective Month Basis"
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1006")

                'Me.txtOption1014.Text = "0"
                '1015
                vArray = Array.CreateInstance(GetType(Object), New Integer() {3}, New Integer() {0})
                vArray(0) = "Not Required"
                vArray(1) = "Required"
                vArray(2) = "Required With Statistics"
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1015")
            End If

            If m_sUnderwritingOrAgency = "U" Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {4}, New Integer() {0})
                'vArray(0) = "Direct to Suspense"
                vArray(1) = "Third Party"
                vArray(2) = "User's choice"
                vArray(3) = "Claim Party"
            Else
                '2002
                vArray = Array.CreateInstance(GetType(Object), New Integer() {3}, New Integer() {0})
                vArray(0) = "Suspense"
                vArray(1) = "Third Party"
                vArray(2) = "Nominal"
            End If

            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2002")

            'Option 1019 & 1021

            m_lReturn = m_oBusiness.GetArchitectureComboDetails(v_sTableName:="pmwrk_task_group", r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetArchitectureComboDetails Failed for pmwrk_task_group", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookupTables")
                Return result
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})

                lUBound = vResultArray.GetUpperBound(1)
                For lCount As Integer = 0 To lUBound
                    vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lCount + 1}, New Integer() {0, 0})

                    vArray(0, lCount) = vResultArray(2, lCount)

                    vArray(1, lCount) = vResultArray(0, lCount)
                Next
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1019")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1021")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:=kAddFailedEmailTaskManagerGroup)
            End If


            m_lReturn = m_oBusiness.GetArchitectureComboDetails(v_sTableName:="pmuser_group", r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetArchitectureComboDetails Failed for pmuser_group", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookupTables")
                Return result
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})

                lUBound = vResultArray.GetUpperBound(1)

                For lCount As Integer = 0 To lUBound
                    vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lCount + 1}, New Integer() {0, 0})

                    vArray(0, lCount) = vResultArray(2, lCount)

                    vArray(1, lCount) = vResultArray(0, lCount)
                Next lCount

                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1020")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1022")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5048")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5049")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5050")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5051")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5052")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5053")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5054")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5055")
                ' new user group for ADDACS failure group
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5041")

                ' new user group for ARUDDS failure group
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5042")

                'sw 31/01/2003 option 79 Claim Debt PM Group
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="79")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5202")
            End If

            'Options 2003 - 2007, 2009 - 2013
            vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})
            vArray(0, 0) = "None"
            vArray(1, 0) = 0
            If Information.IsArray(m_vUserTables) Then
                lLBound = m_vUserTables.GetLowerBound(1)
                lUBound = m_vUserTables.GetUpperBound(1)
                vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lUBound + 2}, New Integer() {0, 0})
                For lCount As Integer = lLBound To lUBound
                    vArray(0, lCount + 1) = m_vUserTables(2, lCount)
                    vArray(1, lCount + 1) = m_vUserTables(0, lCount)
                Next lCount
            End If
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2003")
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2004")
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2005")
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2006")
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2007")
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2009")
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2010")
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2011")
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2012")
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2013")

            'Option 2016
            Dim sCode As String = ""

            If Information.IsArray(m_vUserGroups) Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})
                vArray(0, 0) = ""
                vArray(1, 0) = 0
                lLBound = m_vUserGroups.GetLowerBound(1)
                lUBound = m_vUserGroups.GetUpperBound(1)
                vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lUBound + 2}, New Integer() {0, 0})
                For lCount As Integer = lLBound To lUBound
                    If CStr(m_vUserGroups(1, lCount)).Trim().Length <10 Then
                        sCode= CStr(m_vUserGroups(1, lCount)).Trim() & New String(" "c, 10 - CStr(m_vUserGroups(1, lCount)).Trim().Length)
                    Else
                        sCode = CStr(m_vUserGroups(1, lCount)).Trim()
                    End If
                    vArray(0, lCount + 1) = sCode & ": " & CStr(m_vUserGroups(2, lCount))
                    vArray(1, lCount + 1) = m_vUserGroups(0, lCount)
                Next lCount
            End If
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2016")

            '************************************
            ' MEvans : 14-05-2003 : CQ 709
            ' Option 2018
            If Information.IsArray(m_vUserGroups) Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})
                vArray(0, 0) = ""
                vArray(1, 0) = 0
                lLBound = m_vUserGroups.GetLowerBound(1)
                lUBound = m_vUserGroups.GetUpperBound(1)
                vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lUBound + 2}, New Integer() {0, 0})
                For lCount As Integer = lLBound To lUBound
                    If CStr(m_vUserGroups(1, lCount)).Trim().Length < 10 Then
                        sCode = CStr(m_vUserGroups(1, lCount)).Trim() & New String(" "c, 10 - CStr(m_vUserGroups(1, lCount)).Trim().Length)
                    Else
                        sCode = CStr(m_vUserGroups(1, lCount)).Trim()
                    End If
                    vArray(0, lCount + 1) = sCode & ": " & CStr(m_vUserGroups(2, lCount))
                    vArray(1, lCount + 1) = m_vUserGroups(0, lCount)
                Next lCount
            End If
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2018")

            '************************************


            'Option 1028

            m_lReturn = m_oBusiness.GetArchitectureComboDetails(v_sTableName:="party_type", r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "m_oBusiness." & "GetArchitectureComboDetails Failed for party_type", ACApp, ACClass, "BuildLookupTables")
                Return result
            End If
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {3, 1}, New Integer() {0, 0})

                lUBound = vResultArray.GetUpperBound(1)
                For lCount As Integer = 0 To lUBound
                    vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {3, lCount + 1}, New Integer() {0, 0})

                    vArray(0, lCount) = vResultArray(2, lCount)

                    vArray(1, lCount) = vResultArray(0, lCount)

                    vArray(2, lCount) = vResultArray(1, lCount)
                Next
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1028")
            End If

            '''''''''''''''
            '' AC 27/08/2003 CQ1123 Addition of new combo to handle the different
            '' data models.
            '' START
            'Option 1051

            m_lReturn = m_oBusiness.GetArchitectureComboDetails(v_sTableName:="gis_data_model", r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "m_oBusiness." & "GetArchitectureComboDetails Failed for party_type", ACApp, ACClass, "BuildLookupTables")
                Return result
            End If
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {3, 1}, New Integer() {0, 0})
                '' Set up default Data Model of GIIM then add other codes onto the end
                vArray(0, 0) = "GIIM (default)"
                vArray(1, 0) = 0
                vArray(2, 0) = "GIIM Data Model"

                lUBound = vResultArray.GetUpperBound(1)
                For lCount As Integer = 0 To lUBound
                    vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {3, lCount + 2}, New Integer() {0, 0})

                    vArray(0, lCount + 1) = vResultArray(2, lCount)

                    vArray(1, lCount + 1) = vResultArray(0, lCount)

                    vArray(2, lCount + 1) = vResultArray(1, lCount)
                Next
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:=DefaultDataModelLookup)
            End If
            '' CQ1123 END
            '''''''''''''''

            'Option 1003

            m_lReturn = m_oBusiness.GetArchitectureComboDetails(v_sTableName:="country", r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetArchitectureComboDetails Failed for pmwrk_task_group", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookupTables")
                Return result
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})

                lUBound = vResultArray.GetUpperBound(1)
                For lCount As Integer = 0 To lUBound
                    vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lCount + 1}, New Integer() {0, 0})

                    vArray(0, lCount) = vResultArray(2, lCount)

                    vArray(1, lCount) = vResultArray(0, lCount)
                Next
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1003")
            End If

            'Option 4001
            vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})
            vArray(0) = "Manual"
            vArray(1) = "Automatic"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="4001")

            '    'Option 4002
            '    ReDim vArray(2)
            '    vArray(0) = "Suspend processing of record"
            '    vArray(1) = "Cheapest alternative quote provided less" & vbCrLf & " than holding insurer's premium"
            '    vArray(2) = "Cheapest alternative quote regardless of it cheaper than the holding insurer's premium "
            '    m_cLookupTables.Add v_vItem:=vArray, v_vKey:="4002"

            'Option 4006
            m_lReturn = LoadRenewalsLists(v_sGisBusinessTypeCode:="GIIM", v_sKey:="4006")

            'Option 4007
            m_lReturn = LoadRenewalsLists(v_sGisBusinessTypeCode:="GIIH", v_sKey:="4007")

            'Option 4008
            m_lReturn = LoadRenewalsLists(v_sGisBusinessTypeCode:="GIIT", v_sKey:="4008")

            'Options 1024,1025

            m_lReturn = m_oBusiness.GetArchitectureComboDetails(v_sTableName:="Tax_Band", r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetArchitectureComboDetails Failed for tax_band", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookupTables")
                Return result
            End If

            vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})
            vArray(0, 0) = "Not applicable"
            vArray(1, 0) = 0

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                lUBound = vResultArray.GetUpperBound(1)
                For lCount As Integer = 0 To lUBound
                    vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lCount + 2}, New Integer() {0, 0})

                    vArray(0, lCount + 1) = vResultArray(2, lCount)

                    vArray(1, lCount + 1) = vResultArray(0, lCount)
                Next

            End If

            ' RAW 07/02/2003 : ISS2070 : moved from within preceding if test and added UW condition
            If m_sUnderwritingOrAgency = "U" Then
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1024")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1025")
            End If
            ' RAW 07/02/2003 : ISS2070 : end

            'Options 77, 78, Added SW 31/01/2003

            m_lReturn = m_oBusiness.GetArchitectureComboDetails(v_sTableName:="BankAccount", r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetArchitectureComboDetails Failed for BankAccount", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookupTables")
                Return result
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})

                lUBound = vResultArray.GetUpperBound(1)
                For lCount As Integer = 0 To lUBound
                    vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lCount + 1}, New Integer() {0, 0})

                    vArray(0, lCount) = vResultArray(2, lCount)

                    vArray(1, lCount) = vResultArray(0, lCount)
                Next
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="77")
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="78")
                ' RVH 20/02/2003 : ISS2356 : Start
                ' Converted claim bank account system option from a textbox to a combo
                ' might as well use the data loaded here to save doing another business
                ' call elsewhere in this routine...
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="2017")
                ' RVH 20/02/2003 : ISS2356 : End
            End If

            'end SW 31/01/2003

            'Options 1013

            m_lReturn = m_oBusiness.GetPeriodDetails(r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetPeriodDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookupTables")
                Return result
            End If

            vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})
            vArray(0, 0) = "None"
            vArray(1, 0) = 0

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                lUBound = vResultArray.GetUpperBound(1)
                For lCount As Integer = 0 To lUBound
                    vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lCount + 2}, New Integer() {0, 0})

                    vArray(0, lCount + 1) = vResultArray(1, lCount)

                    vArray(1, lCount + 1) = vResultArray(0, lCount)
                Next
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1013")
            End If


            ' RAW 06/06/2003 : CR23 : added
            'Option 1030 - Premium Override Event level
            If m_sUnderwritingOrAgency = "U" Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {3}, New Integer() {0})
                vArray(0) = "None"
                vArray(1) = "Summary"
                vArray(2) = "Full"
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="1030")
            End If

            ' SET 07/04/2004 ISS11535 - Raise debit system option
            'Option 201
            vArray = Array.CreateInstance(GetType(Object), New Integer() {3}, New Integer() {0})
            vArray(0) = "Not Required"
            vArray(1) = "Invisibly"
            vArray(2) = "Interactively"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="201")

            ' SET 07/04/2004 ISS11535 - edit policy system option
            'Option 202
            vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})
            vArray(0) = "Not Required"
            vArray(1) = "Required"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="202")

            ' SET 15/06/2004 ISS12435 - Renewal Auto Debit system option
            vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})
            vArray(0) = "At the Quoted stage"
            vArray(1) = "At the Invited stage"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="203")

            'Plico - 21
            If m_sUnderwritingOrAgency = "U" Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {3}, New Integer() {0})
                vArray(0) = "Reported Date"
                vArray(1) = "System Date"
                vArray(2) = "Loss Date"
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5030")
            End If

            If m_sUnderwritingOrAgency = "U" Then
                'Options 5031

                m_lReturn = m_oBusiness.GetNumberingSchemes(r_vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetNumberingSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookupTables")
                    Return result
                End If

                vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})
                vArray(0, 0) = "None"
                vArray(1, 0) = 0

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    lUBound = vResultArray.GetUpperBound(1)
                    For lCount As Integer = 0 To lUBound
                        vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lCount + 2}, New Integer() {0, 0})

                        vArray(0, lCount + 1) = vResultArray(1, lCount)

                        vArray(1, lCount + 1) = vResultArray(0, lCount)
                    Next
                End If
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5031")

                'Options 5035

                m_lReturn = m_oBusiness.GetGISScreens(r_vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetGISScreens Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookupTables")
                    Return result
                End If

                vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})
                vArray(0, 0) = "None"
                vArray(1, 0) = 0

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    lUBound = vResultArray.GetUpperBound(1)
                    For lCount As Integer = 0 To lUBound
                        vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lCount + 2}, New Integer() {0, 0})

                        vArray(0, lCount + 1) = vResultArray(1, lCount)

                        vArray(1, lCount + 1) = vResultArray(0, lCount)
                    Next

                End If
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5035")
            End If

            'Start (Prakash C Varghese)-(Tech Spec - PGR003 - V2 - Credit Card Encryption.doc)
            'Option 5069: Credit Card Processing Method
            vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})
            vArray(0) = "Internal"
            vArray(1) = "External"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5069")
            'End (Prakash C Varghese)-(Tech Spec - PGR003 - V2 - Credit Card Encryption.doc)

            vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})
            vArray(0) = "yyyyMMdd hhmmss tt"
            vArray(1) = "MMddyyyy hhmmss tt"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5146")


            m_lReturn = m_oBusiness.GetArchitectureComboDetails(v_sTableName:="risk_type_rule_set_type", sWhereClause:="CODE<>'PRE'", r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetArchitectureComboDetails Failed for tax_band", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookupTables")
                Return result
            End If

            vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                lUBound = vResultArray.GetUpperBound(1)
                For lCount As Integer = 0 To lUBound
                    vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lCount + 1}, New Integer() {0, 0})
                    vArray(0, lCount) = vResultArray(2, lCount)
                    vArray(1, lCount) = vResultArray(0, lCount)
                Next

                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:=CStr(GeneralConst.kSystemOptionRuleTypePaymentGateway))
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:=CStr(GeneralConst.kSystemOptionRuleTypeCreditControl))
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:=CStr(GeneralConst.kSystemOptionRuleTypeChaseCycle))
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:=CStr(GeneralConst.kSystemOptionRuleTypeAddressLookup))
            End If

            'Option 5163
            vArray = Array.CreateInstance(GetType(Object), New Integer() {2}, New Integer() {0})
            vArray(0) = "Pure In-House document production"
            vArray(1) = "KCM document production"
            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:=GeneralConst.kSystemOptionDocumentProductionSystem)

            'Option 5171
            m_lReturn = m_oBusiness.GetArchitectureComboDetails(v_sTableName:="CCMStatus", r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetArchitectureComboDetails Failed for pmwrk_task_group", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookupTables")
                Return result
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})

                lUBound = vResultArray.GetUpperBound(1)
                For lCount As Integer = 0 To lUBound
                    vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lCount + 1}, New Integer() {0, 0})
                    vArray(0, lCount) = vResultArray(2, lCount)
                    vArray(1, lCount) = vResultArray(0, lCount)
                Next
                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:=GeneralConst.kSystemOptionCCMStatus)
            End If

            m_lReturn = m_oBusiness.GetTaxGroupForClaims(r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetClaimsReserveTaxGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookupTables")
                Return result
            End If

            vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})
            vArray(0, 0) = "(None)"
            vArray(1, 0) = 0

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                lUBound = vResultArray.GetUpperBound(1)
                For lCount As Integer = 0 To lUBound

                    vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lCount + 2}, New Integer() {0, 0})
                    vArray(0, lCount + 1) = vResultArray(1, lCount)
                    vArray(1, lCount + 1) = vResultArray(0, lCount)
                Next
            End If

            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5240")

            m_lReturn = m_oBusiness.GetALLCurrency(r_vResultArray:=vResultArray, r_BranchBaseCurrencyID:=m_iBranchBaseCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGetbSIRBusiness.GetALLCurrency Failed for spu_currency_selall", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookupTables")

                Return result

            End If





            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                vArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})



                lUBound = vResultArray.GetUpperBound(1)

                For lCount As Integer = 0 To lUBound

                    vArray = ArraysHelper.RedimPreserve(Of Object(,))(vArray, New Integer() {2, lCount + 1}, New Integer() {0, 0})

                    vArray(0, lCount) = vResultArray(2, lCount)

                    vArray(1, lCount) = vResultArray(0, lCount)

                Next

                m_cLookupTables.Add(v_vItem:=vArray, v_vKey:="5243")

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildLookupTables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildLookupTables", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupTable
    '
    ' Description:
    '
    ' History: 04/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function GetLookupTable(ByVal v_sKey As String, ByRef r_cComboBox As ComboBox) As Integer

        Dim result As Integer = 0
        Dim vArray As Object = Nothing
        Dim bExists As Boolean
        Dim lUBound As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_cLookupTables.Item(v_vKey:=v_sKey, r_vItem:=vArray, r_vExists:=bExists)
            If Not bExists Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to load lookup table for " & v_sKey, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupTable")
                Return result
            End If

            Dim r_cComboBox_NewIndex As Integer = -1
            If ArrayBound(vArray) = 1 Then
                lUBound = vArray.GetUpperBound(0)
                For i As Integer = 0 To lUBound
                    'Modified by Archana Tokas on 5/12/2010 11:38:06 AM declared out side if condition
                    'Dim r_cComboBox_NewIndex As Integer = -1
                    r_cComboBox_NewIndex = r_cComboBox.Items.Add(CStr(vArray(i)))
                Next i
            Else
                lUBound = vArray.GetUpperBound(1)
                For i As Integer = 0 To lUBound
                    r_cComboBox_NewIndex = r_cComboBox.Items.Add(CStr(vArray(0, i)))
                    VB6.SetItemData(r_cComboBox, r_cComboBox_NewIndex, CInt(vArray(1, i)))
                Next i
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupTable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupTable", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: ArrayBound
    '
    ' Description:
    '
    ' History: 04/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function ArrayBound(ByVal vArray As Array) As Integer

        Dim result As Integer = 0
        Try

            result = 1

            If vArray.GetUpperBound(1) > -1 Then
                result = 2
            End If

            Return result

        Catch




            Return 1
        End Try

    End Function
    ' ***************************************************************** '
    ' Name: GetSystemOptionData
    '
    ' Description:
    '
    ' History: 04/12/2002 sj - Created.
    '
    ' ***************************************************************** '

    Private Function GetSystemOptionData(ByVal v_sKey As String, ByVal v_sControlType As String, ByRef r_cControl As Control, Optional ByVal v_sCommand As String = "") As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim bExists As Boolean
            Dim vValue As Object = Nothing
            Dim vArray As Object = Nothing
            Dim vActualValue As String = ""

            m_lReturn = m_cSystemOptionData.Item(v_vKey:=v_sKey, r_vItem:=vValue, r_vExists:=bExists)
            If Not bExists Then
                Return result
            End If

            If CBool(vValue(ACD1Updated)) Then

                vActualValue = CStr(vValue(ACD1NewValue))
            Else

                vActualValue = CStr(vValue(ACD1OriginalValue))
            End If

            If v_sKey = "5243" Then
                If (String.IsNullOrEmpty(vActualValue) OrElse gPMFunctions.ToSafeInteger(vActualValue) = 0) AndAlso m_iBranchBaseCurrency <> 0 Then
                    vActualValue = gPMFunctions.ToSafeString(m_iBranchBaseCurrency)
                End If
            End If

            Select Case v_sControlType
                Case "ComboBox"
                    m_lReturn = m_cLookupTables.Item(v_vKey:=v_sKey, r_vItem:=vArray, r_vExists:=bExists)

                    If ArrayBound(vArray) = 1 Then
                        ' if the stored list index is greater than the
                        ' possible list indexes then dont set the listindex

                        If Conversion.Val(vActualValue) <= CType(r_cControl, ComboBox).Items.Count - 1 Then
                            'This is the list index
                            CType(r_cControl, ComboBox).SelectedIndex = Conversion.Val(vActualValue)
                        End If
                    Else

                        For i As Integer = 0 To CInt(CType(r_cControl, ComboBox).Items.Count - 1)
                            If CType(r_cControl, ComboBox).Items(i).itemdata = Conversion.Val(vActualValue) Then
                                CType(r_cControl, ComboBox).SelectedIndex = i
                                Exit For
                            End If
                        Next i
                    End If
                Case "CheckBox"

                    If Conversion.Val(vActualValue) <> 0 Then
                        CType(r_cControl, CheckBox).CheckState = CheckState.Checked
                    End If
                Case "OptionButton"

                    If Conversion.Val(vActualValue) = 1 Then

                        ReflectionHelper.SetMember(r_cControl, "Value", True)

                    Else

                        ReflectionHelper.SetMember(r_cControl, "Value", False)
                    End If

                Case "TextBox"

                    Select Case v_sCommand
                        Case ACCommandShowTemplateCode
                            If vActualValue.Trim() = "" Then
                                r_cControl.Text = ACNoTemplate
                            Else
                                m_lReturn = ShowTemplateCode(v_lTemplateID:=CInt(Conversion.Val(vActualValue)), r_txtText:=r_cControl)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowTemplate Code Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOptionData")
                                    Return result
                                End If
                            End If
                        Case ACCommandFormatCurrency
                            If vActualValue.Trim() <> "" Then
                                r_cControl.Text = StringsHelper.Format(vActualValue, "##############0")

                            End If
                        Case ACCommandFailureMsg
                            r_cControl.Text = vActualValue
                            CType(r_cControl, TextBox).Multiline = True
                            CType(r_cControl, TextBox).Height = ACD1MultiTxtHeight

                        Case ACCommandValidateNumeric

                            Select Case v_sKey
                                Case "5103"
                                    CType(r_cControl, TextBox).MaxLength = "2"
                                Case "5105"
                                    CType(r_cControl, TextBox).MaxLength = "1"
                                Case "5107"
                                    CType(r_cControl, TextBox).MaxLength = "1"
                                Case "5109"
                                    CType(r_cControl, TextBox).MaxLength = "1"
                                Case "5111"
                                    CType(r_cControl, TextBox).MaxLength = "2"
                                Case "5099"
                                    CType(r_cControl, TextBox).MaxLength = "3"
                                Case "5242"
                                    CType(r_cControl, TextBox).MaxLength = "6"
                            End Select
                            r_cControl.Text = vActualValue
                        Case Else
                            r_cControl.Text = vActualValue
                    End Select

                    If v_sKey = "5085" Then
                        m_sSharePointOnlineInitialURL = vActualValue
                    End If

                    If v_sKey = "5086" Then
                        m_sSharePointOnlineInitialLibrary = vActualValue
                    End If

                    If v_sKey = "5178" Then
                        m_sSharePointOnlineInitialUserId = vActualValue
                    End If
                    If v_sKey = "5258" Then
                        m_sSharePointOnlineAppClientId = vActualValue
                    End If
                    If v_sKey = "5259" Then
                        m_sSharePointTenantId = vActualValue
                    End If
                    If v_sKey = "5179" Then
                        Dim sDecryptedPassword As String = String.Empty
                        sDecryptedPassword = bPMFunc.DecryptPassword(vActualValue, PMEncryptionEntropy)
                        r_cControl.Text = sDecryptedPassword
                        m_sSharePointOnlineInitialPassword = sDecryptedPassword
                    End If

                    If v_sKey = "5190" Then
                        CType(r_cControl, TextBox).Multiline = True
                        CType(r_cControl, TextBox).Height = 50
                    End If

                Case "uctCompiledRule"
                    r_cControl.Text = vActualValue

            End Select

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOptionData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CacheSystemOptionData
    '
    ' Description:
    '
    ' History: 04/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function CacheSystemOptionData(ByVal v_sKey As String, ByVal v_sControlType As String, ByRef r_cControl As Control, Optional ByVal v_sCommand As String = "", Optional ByVal v_sAdditionalData As String = "") As Integer

        Dim result As Integer = 0
        Dim bExists As Boolean
        Dim vValue As Object = Nothing
        Dim vArray As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_cSystemOptionData.Item(v_vKey:=v_sKey, r_vItem:=vValue, r_vExists:=bExists)
            If Not bExists Then
                ReDim vValue(ACD1Size)

                vValue(ACD1OptionNumber) = v_sKey
            End If


            Select Case v_sControlType
                Case "ComboBox"
                    m_lReturn = m_cLookupTables.Item(v_vKey:=v_sKey, r_vItem:=vArray, r_vExists:=bExists)
                    If ArrayBound(vArray) = 1 Then
                        'This is the list index


                        'Developer Guide no. 198,ctype in combobox
                        'vValue(r_cControl.ListIndex, ACD1NewValue)
                        vValue(ACD1NewValue) = CType(r_cControl, ComboBox).SelectedIndex


                        vValue(ACD1Updated) = True
                    Else


                        'If r_cControl.ListIndex > -1 Then
                        If CType(r_cControl, ComboBox).SelectedIndex > -1 Then



                            'Developer Guide no. 198,ctype in combobox
                            'vValue(r_cControl.ItemData(r_cControl.ListIndex), ACD1NewValue)
                            vValue(ACD1NewValue) = CType(r_cControl, ComboBox).Items(CType(r_cControl, ComboBox).SelectedIndex).ItemData

                            vValue(ACD1Updated) = True
                        End If
                    End If

                Case "CheckBox"


                    'Developer Guide no. 198,ctype in CheckBox
                    'vValue(r_cControl.Value, ACD1NewValue)
                    vValue(ACD1NewValue) = CType(r_cControl, CheckBox).CheckState

                    vValue(ACD1Updated) = True

                Case "OptionButton"

                    'NIIT - Replaced with the Migrated code 1144 
                    'If r_cControl.Value Then
                    If ReflectionHelper.GetMember(r_cControl, "Value") Then


                        vValue(ACD1NewValue) = 1
                    Else

                        vValue(ACD1NewValue) = 0
                    End If

                    vValue(ACD1Updated) = True

                Case "TextBox"
                    If v_sCommand = ACCommandShowTemplateCode Then

                        vValue(ACD1NewValue) = Conversion.Val(v_sAdditionalData)
                    Else


                        vValue(ACD1NewValue) = r_cControl.Text
                    End If

                    vValue(ACD1Updated) = True

                Case "uctCompiledRule"
                    vValue(ACD1NewValue) = r_cControl.Text

                    vValue(ACD1Updated) = True

            End Select

            If bExists Then
                m_lReturn = m_cSystemOptionData.Remove(v_vKey:=v_sKey)
            End If

            m_lReturn = m_cSystemOptionData.Add(v_vItem:=vValue, v_vKey:=v_sKey)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CacheSystemOptionData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CacheSystemOptionData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SaveSystemOptionData
    '
    ' Description:
    '
    ' History: 05/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function SaveSystemOptionData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vValue As Object = Nothing
            Dim sSQL As String = ""
            Dim lCount As Integer
            Dim bInitialiseDocumaster, bDocumasterOptionIsSet As Boolean
            Dim vUpdateArray(,) As Object = Nothing
            Dim lCnt, iUpdateFlag As Integer

            Const ACNoUpdate As Integer = 0
            Const ACUpdate As Integer = 1
            Const ACInsert As Integer = 2

            Const ACOptionNumber As Integer = 0
            Const ACValue As Integer = 1
            Const ACUpdateFlag As Integer = 2
            Const ACUpdateArraySize As Integer = 2

            lCnt = 0

            m_lReturn = m_cSystemOptionData.Count(r_lCount:=lCount)

            For i As Integer = 1 To lCount
                iUpdateFlag = ACNoUpdate

                m_lReturn = m_cSystemOptionData.Item(v_vKey:=i, r_vItem:=vValue)

                If CBool(vValue(ACD1Updated)) Then

                    If CStr(vValue(ACD1NewValue)).Trim() <> CStr(vValue(ACD1OriginalValue)).Trim() Then

                        If CDbl(vValue(ACD1OptionNumber)) = 10 Then

                            If CDbl(vValue(ACD1NewValue)) = 1 Then
                                bInitialiseDocumaster = True
                                bDocumasterOptionIsSet = True
                            End If
                        End If
                        iUpdateFlag = ACUpdate

                    End If


                    If Object.Equals(vValue(ACD1OriginalValue), Nothing) And Not Object.Equals(vValue(ACD1NewValue), Nothing) Then
                        iUpdateFlag = ACInsert

                    End If

                End If


                If vValue(ACD1OptionNumber) = ACUserPassword OrElse vValue(ACD1OptionNumber) = kPassword OrElse vValue(ACD1OptionNumber) = kPaymentHubPassword OrElse vValue(ACD1OptionNumber) = kSystemPasscode OrElse vValue(ACD1OptionNumber) = kSystemGUID OrElse vValue(ACD1OptionNumber) = kAccountID OrElse vValue(ACD1OptionNumber) = kAccountPasscode OrElse vValue(ACD1OptionNumber) = kRefundPasscode Then

                    Dim sEnCryptedStr As String = ""
                    Dim sPwd As String = ""
                    Dim sdecryptpwd As String = ""

                    If ToSafeString(vValue(ACD1NewValue)).Trim() <> ToSafeString(vValue(ACD1OriginalValue)).Trim() Then
                        sPwd = ToSafeString(vValue(ACD1NewValue)).Trim()
                        If sPwd <> String.Empty Then
                            sEnCryptedStr = GetEVal(sPwd)
                            vValue(ACD1NewValue) = sEnCryptedStr
                        Else
                            vValue(ACD1NewValue) = sPwd
                        End If

                    Else
                        sPwd = ToSafeString(vValue(ACD1OriginalValue)).Trim()
                        If sPwd <> String.Empty Then
                            sEnCryptedStr = GetEVal(sPwd)
                            vValue(ACD1OriginalValue) = sEnCryptedStr
                        Else
                            vValue(ACD1OriginalValue) = sPwd
                        End If

                    End If
                End If
                'PN 21921 : Applied a check for checking OptionNumber is supplied or not

                If (iUpdateFlag = ACUpdate Or iUpdateFlag = ACInsert) And CStr(vValue(ACD1OptionNumber)).Trim().Length > 0 Then
                    If Not Information.IsArray(vUpdateArray) Then
                        ReDim vUpdateArray(ACUpdateArraySize, lCnt)
                    Else
                        ReDim Preserve vUpdateArray(ACUpdateArraySize, lCnt)
                    End If


                    vUpdateArray(ACOptionNumber, lCnt) = CStr(vValue(ACD1OptionNumber)).Trim()


                    vUpdateArray(ACValue, lCnt) = CStr(vValue(ACD1NewValue)).Trim()
                    If iUpdateFlag = ACUpdate Then

                        vUpdateArray(ACUpdateFlag, lCnt) = True
                    Else

                        vUpdateArray(ACUpdateFlag, lCnt) = False
                    End If

                    lCnt += 1
                End If
            Next i

            If Information.IsArray(vUpdateArray) Then

                'Save the data to the database

                m_lReturn = m_oBusiness.UpdateSystemOptionData(v_vUpdateArray:=vUpdateArray, v_bInitialiseDocumaster:=bInitialiseDocumaster)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lReturn = gPMConstants.PMEReturnCode.PMDocumasterError Then
                        ' If documaster error, we tell user he can't enable
                        ' Documaster option because Documaster is not installed
                        ' on the system
                        If bDocumasterOptionIsSet Then
                            MessageBox.Show("Documaster doesn't seem to be installed on this environment.", "DocuMaster", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    Else
                        ' Standard error logging
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UpdateSystemOptionData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveSystemOptionData")
                        Return result
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveSystemOptionData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveSystemOptionData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: ShowTemplateCode
    '
    ' Description:
    '
    ' History: 14/06/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ShowTemplateCode(ByVal v_lTemplateID As Integer, ByRef r_txtText As TextBox) As Integer

        Dim result As Integer = 0
        Dim iLoop1, lUBound, lLBound As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lTemplateID = 0 Then
                Return result
            End If

            ' CTAF 280600
            If Not Information.IsArray(m_vDocCodes) Then
                Return result
            End If

            ' Get the location in the doc code array
            lUBound = m_vDocCodes.GetUpperBound(1)
            lLBound = m_vDocCodes.GetLowerBound(1)
            For iLoop1 = lLBound To lUBound

                If CInt(m_vDocCodes(0, iLoop1)) = v_lTemplateID Then
                    ' Set the text
                    r_txtText.Text = CStr(m_vDocCodes(1, iLoop1))
                    Exit For
                End If
            Next iLoop1

            m_lReturn = SetTagData(r_cControl:=r_txtText, v_sAdditionalData:=CStr(v_lTemplateID))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowTemplateCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowTemplateCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetCorrespondingControl
    '
    ' Description:
    '
    ' History: 09/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function GetCorrespondingControl(ByRef r_sControlType As String, ByVal v_lOptionNumber As Integer, ByRef r_iIndex As Integer, ByRef r_bFound As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lOptionIndex As Integer
            Dim sAdditionalData As String = ""
            Dim lOptionNumber As Integer

            r_bFound = False
            Select Case r_sControlType
                Case "TextBox"
                    For i As Integer = 1 To txtOption.Length
                        m_lReturn = GetTagData(v_cControl:=txtOption(i), r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMError
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTagData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCorrespondingControl")
                            Return result
                        End If
                        lOptionNumber = CInt(Conversion.Val(CStr(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex))))
                        If lOptionNumber = v_lOptionNumber Then
                            r_iIndex = i
                            r_bFound = True
                            Return result
                        End If
                    Next i
            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCorrespondingControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCorrespondingControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetDocument
    '
    ' Description:
    '
    ' History: 01/06/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetDocument(ByRef txtDocument As TextBox) As Integer

        Dim result As Integer = 0
        Dim lDocumentTemplateID As Integer
        Dim sDocumentCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the document template
            m_lReturn = GetDocumentTemplate(r_lDocumentTemplateID:=lDocumentTemplateID, r_sDocumentCode:=sDocumentCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMCancel) Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocument")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                Return result
            End If

            txtDocument.Text = sDocumentCode

            m_lReturn = SetTagData(r_cControl:=txtDocument, v_sAdditionalData:=CStr(lDocumentTemplateID))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetTagData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocument")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetTagData
    '
    ' Description:
    '
    ' History: 09/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function GetTagData(ByVal v_cControl As Control, ByRef r_lOptionIndex As Integer, ByRef v_sAdditionalData As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sTag As String = ""
            Dim vArray As Object
            Dim lUBound As Integer

            sTag = Convert.ToString(ControlHelper.GetTag(v_cControl))
            If sTag.Trim() = "" Then
                r_lOptionIndex = 0
                v_sAdditionalData = ""
            End If


            vArray = sTag.Split("|"c)

            lUBound = vArray.GetUpperBound(0)
            If lUBound = 1 Then

                r_lOptionIndex = CInt(Conversion.Val(CStr(vArray(0))))

                v_sAdditionalData = CStr(vArray(1))
            ElseIf lUBound = 0 Then

                r_lOptionIndex = CInt(Conversion.Val(CStr(vArray(0))))
                v_sAdditionalData = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTagData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTagData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: SetTagData
    '
    ' Description:
    '
    ' History: 09/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function SetTagData(ByRef r_cControl As Control, ByVal v_sAdditionalData As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lOptionIndex As Integer
            Dim sAdditionalData As String = ""

            m_lReturn = GetTagData(v_cControl:=r_cControl, r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTagData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetTagData")
                Return result
            End If

            ControlHelper.SetTag(r_cControl, CStr(lOptionIndex) & "|" & v_sAdditionalData)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetTagData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetTagData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetDocumentTemplate
    '
    ' Description: Creates an instance of find document template and gets the properties
    '
    ' History: 01/06/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetDocumentTemplate(ByRef r_lDocumentTemplateID As Integer, ByRef r_sDocumentCode As String) As Integer
        Dim result As Integer = 0


        Dim oObject As iPMBFindDocTemplate.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of iPMBFindDocTemplate.Interface
            Dim temp_oObject As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oObject = temp_oObject
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Start it up

            m_lReturn = oObject.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Exit out if it was cancelled

            If oObject.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            ' Get it's properties
            With oObject

                r_lDocumentTemplateID = .DocumentTemplateId

                r_sDocumentCode = .DocumentCode
            End With

            ' Terminate it

            oObject.Dispose()
            ' Clear up
            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentTemplate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************************** '
    '
    ' Name: LocateQAS
    '
    ' Description: Displays a common dialog box to locate the qaddress.ini file
    '
    ' History: 17/02/2000 CTAF - Created.
    '
    ' ***************************************************************************** '
    Private Function LocateQAS(ByVal v_sQASOption As String) As Integer

        Dim result As Integer = 0
        Dim sFilename As String = ""

        On Error GoTo Err_LocateQAS

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the current path
        m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=ACQASProRegistry, r_sSettingValue:=sFilename)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If sFilename = "" Then
            ' try the rapid path
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=ACQASRapidRegistry, r_sSettingValue:=sFilename)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        dlgQAS = New OpenFileDialog
        With dlgQAS
            ' Set the flags


            .CheckFileExists = True
            .CheckPathExists = True


            .ShowReadOnly = False
            ' Set the file
            .FileName = sFilename
            ' Error if cancel is pressed

            'Modified by Archana Tokas on 5/12/2010 11:46:26 AM to be checked later todolist
            '.CancelError = True
        End With

        ' Note: This is not in the above with incase the user clicks cancel
        '       and a memory leak occurs

        ' Show the dialog
        dlgQASOpen.ShowDialog()

        ' Get the filename
        sFilename = dlgQASOpen.FileName

        ' CTAF 211100 - Changed to distinguish between pro and rapid
        If sFilename.Length > 0 Then


            Select Case v_sQASOption
                Case ACQASRapid

                    ' Store the filename in the registry
                    m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=ACQASRapidRegistry, v_sSettingValue:=sFilename)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case ACQASPro

                    ' Store the filename in the registry
                    m_lReturn = gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=ACQASProRegistry, v_sSettingValue:=sFilename)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case Else

                    ' Dont care

            End Select

        End If

Exit_LocateQAS:

        Return result

Err_LocateQAS:


        If Information.Err().Number = DialogResult.Cancel Then
            Resume Exit_LocateQAS
        End If

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LocateQAS Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LocateQAS", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: GetClaimHelpFile
    '
    ' Description:
    '
    ' History: 11/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function GetClaimHelpFile(ByRef r_cClaimHelpFile As Control) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sFilename As String = ""

            Dim sOrigFilename As String = String.Empty

            'Borrow the dlgHelp common dialog control - we don't need to add a new one

            'Modified by Archana Tokas on 5/12/2010 11:48:51 AM refer developer guide no. 20
            'With dlgHelp
            'Preserve the original values

            '	lOrifFlags = .Flags

            '	sOrigFilter = .Filter
            '	sOrigFilename = .FileName

            '	bOrigCancelError = .CancelError

            ' Set the flags


            '	.CheckFileExists = True
            '	.CheckPathExists = True


            '	.ShowReadOnly = False
            ' Set the file

            '	.Filter = "Help (*.hlp)|*.hlp|All files (*.*)|*.*"

            '	.FileName = r_cClaimHelpFile.Text.Trim()
            ' No Error if cancel is pressed

            '	.CancelError = False
            'End With

            ' Note: This is not in the above with incase the user clicks cancel
            '       and a memory leak occurs

            ' Show the dialog
            '	dlgHelpOpen.ShowDialog()
            'Modified by Archana Tokas on 5/12/2010 11:48:51 AM refer developer guide no. 20
            System.Diagnostics.Process.Start(r_cClaimHelpFile.Text)

            ' Get the filename and put it in control
            r_cClaimHelpFile.Text = dlgHelpOpen.FileName


            'Modified by Archana Tokas on 5/12/2010 11:48:51 AM refer developer guide no. 20
            '	With dlgHelp
            'Reset the original values

            '		.Flags = lOrifFlags

            '		.Filter = sOrigFilter
            '		.FileName = sOrigFilename

            '		.CancelError = bOrigCancelError
            '	End With
            System.Diagnostics.Process.Start(sOrigFilename)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimHelpFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimHelpFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetGenericFile
    '
    ' Description:
    '
    ' History: AMB 05/03/2003: PS220/123 - created
    '
    ' ***************************************************************** '
    Private Function GetGenericFile(ByVal v_sBrowseType As String, ByRef r_cGenericFile As Control) As Integer

        Dim result As Integer = 0
        Dim sFilename As String = ""
        Dim sOrigFilename As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Modified by Archana Tokas on 5/12/2010 11:54:39 AM refer developer guide no. 20
            ' set up the generic common dialog

            '	With cdgGeneric
            'Preserve the original values

            '		lOrifFlags = .Flags

            '		sOrigFilter = .Filter
            '		sOrigFilename = .FileName

            '		bOrigCancelError = .CancelError

            ' Set the flags


            '		.CheckFileExists = True
            '		.CheckPathExists = True


            '		.ShowReadOnly = False

            ' Set the file filter
            '		Select Case v_sBrowseType
            '			Case ACBrowseFileHelp

            '				.Filter = ACBrowseFileFilterHelp
            '			Case ACBrowseFileHTML

            '				.Filter = ACBrowseFileFilterHTML
            '			Case ACBrowseFileAll

            '				.Filter = ACBrowseFileFilterAll
            '			Case Else

            '				.Filter = ACBrowseFileFilterAll
            '		End Select


            '		.FileName = r_cGenericFile.Text.Trim()
            ' No Error if cancel is pressed

            '		.CancelError = False
            '	End With
            System.Diagnostics.Process.Start(r_cGenericFile.Text)
            ' Note: This is not in the above with incase the user clicks cancel
            '       and a memory leak occurs

            ' Show the dialog
            cdgGenericOpen.ShowDialog()

            ' Get the filename and put it in control
            r_cGenericFile.Text = cdgGenericOpen.FileName


            'Modified by Archana Tokas on 5/12/2010 11:54:39 AM refer developer guide no. 20
            '	With cdgGeneric
            'Reset the original values

            '		.Flags = lOrifFlags

            '		.Filter = sOrigFilter
            '		.FileName = sOrigFilename

            '		.CancelError = bOrigCancelError
            '	End With
            System.Diagnostics.Process.Start(sOrigFilename)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGenericFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGenericFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: LoadRenewalsLists
    '
    ' Description:
    '
    ' History: 22/06/2001 sj - Created.
    '
    ' ***************************************************************** '
    Private Function LoadRenewalsLists(ByVal v_sGisBusinessTypeCode As String, ByVal v_sKey As String) As Integer

        Dim result As Integer = 0
        Dim lGisBusinessTypeId As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim vParameterValues As Object = Nothing
        'Dim lCnt As Long
        'Dim lArraySize As Long
        'Dim lListItem As Long
        Dim lUBound As Integer

        Const ACId As Integer = 0
        Const ACCode As Integer = 1
        Const ACDescription As Integer = 3
        Const ACName As Integer = 0
        Const ACValue As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vParameterValues(1, 0)

            vParameterValues(ACName, 0) = "gis_business_type_id"

            'Get the gis business type id

            m_lReturn = m_oPMLookup.GetEffectiveIDFromCode(v_sTableName:="gis_business_type", v_sCode:=v_sGisBusinessTypeCode, v_dtEffectiveDate:=DateTime.Now, r_lId:=lGisBusinessTypeId)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then


                vParameterValues(ACValue, 0) = lGisBusinessTypeId


                m_lReturn = m_oSchemeGroup.GetList(v_lListType:=2, r_vSchemeGroupsArray:=vResultArray, v_vParameterValues:=vParameterValues)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Dim vArray(1, 0) As Object

            vArray(0, 0) = "None"

            vArray(1, 0) = 0

            If Information.IsArray(vResultArray) Then

                lUBound = vResultArray.GetUpperBound(1)
                For lCount As Integer = 0 To lUBound
                    ReDim Preserve vArray(1, lCount + 1)


                    vArray(0, lCount + 1) = vResultArray(ACDescription, lCount)


                    vArray(1, lCount + 1) = vResultArray(ACId, lCount)
                Next lCount
            End If

            m_cLookupTables.Add(v_vItem:=vArray, v_vKey:=v_sKey)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRenewalsLists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRenewalsLists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboOption_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cboOption_0.SelectedIndexChanged
        Dim Index As Integer = Array.IndexOf(cboOption, eventSender)

        Dim lOptionIndex As Integer
        Dim sAdditionalData As String = ""
        Static bInit As Boolean
        Dim iControlCount As Integer
        Dim bFoundItem As Boolean
        Dim arrControlList As ArrayList = New ArrayList

        If Me.HasChildren Then
            ControlList(Me, arrControlList)
        End If

        m_lReturn = GetTagData(v_cControl:=cboOption(Index), r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)

        Dim lListIndex As Integer = cboOption(Index).SelectedIndex

        Dim sCommand As String = CStr(m_vOptionsConfiguration(ACCommand, lOptionIndex))

        Select Case sCommand
            Case ACCommandLocateQAS
                If bInit Then
                    If VB6.GetItemString(cboOption(Index), lListIndex) <> ACQASNotInstalled And VB6.GetItemString(cboOption(Index), lListIndex) <> ACPAFWrapper Then
                        m_lReturn = LocateQAS(v_sQASOption:=VB6.GetItemString(cboOption(Index), lListIndex))
                    End If
                Else
                    bInit = True
                End If
        End Select

        ' Check if we are processing system option 16 (Earn Commission combo on General tab) PN23212
        ' If so and it's value is set to "When Client Pays' then enable the
        ' Earn Commission on Part Payment for TP Finance' checkbox
        ' else disable it and set it to unchecked
        If CDbl(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex)) = 16 Then

            ' First try and find the correct checkbox
            For iControlCount = 0 To chkOption.Length - 1
                ' Caption has changed. PN23449. In the long run, would this
                ' type of functionality be better being configurable from the System
                ' Options the configuration utility rather than hard
                ' coded here??. However, for now, with the time limitations we
                ' currently face...
                'Developer Guide no, add if condition to check the blank element
                If Not chkOption(iControlCount) Is Nothing Then
                    If chkOption(iControlCount).Text.Trim().ToLower() = "proportional commission movement on client part payments:" Then
                        bFoundItem = True
                        Exit For
                    End If
                End If
            Next

            If bFoundItem Then
                If cboOption(Index).Text.Trim().ToLower() = "when client pays" Then
                    chkOption(iControlCount).Enabled = True
                Else
                    chkOption(iControlCount).Enabled = False
                    chkOption(iControlCount).CheckState = CheckState.Unchecked
                End If
            End If
        End If

        If CDbl(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex)) = 10 Then
            m_bArchiveSoftwareEnabled = (cboOption(Index).SelectedIndex > 0)
            m_bIsSharePoint = (cboOption(Index).SelectedIndex = 2)
            'Modified,add a for loop and a dim condition,comment the for loop
            For arrItem As Integer = 0 To arrControlList.Count - 1
                Dim oControl As Control = arrControlList(arrItem)
                'For Each oControl As Control In ContainerHelper.Controls(Me)
                If TypeOf oControl Is CheckBox Then
                    With CType(oControl, CheckBox)
                        If .Text = "Auto-archive:" Or .Text = "Archive as PDF:" Or .Text = "Archive Document with Timestamp:" Then
                            .Visible = m_bArchiveSoftwareEnabled
                            .Enabled = m_bArchiveSoftwareEnabled
                            If .Text = "Archive as PDF:" Then
                                m_bArchiveAsPDF = (.CheckState = CheckState.Checked)
                            End If
                        End If

                        If Not String.IsNullOrEmpty(.Name) Then
                            If .Name = "chkOption5177" Then
                                If lListIndex = 2 Then
                                    .Enabled = True
                                    m_bSharepointOnline = (.CheckState = CheckState.Checked)
                                Else
                                    .Enabled = False
                                    .CheckState = CheckState.Unchecked
                                    m_bSharepointOnline = CheckState.Unchecked
                                End If
                            End If
                        End If
                    End With
                ElseIf TypeOf oControl Is ComboBox Then
                    With CType(oControl, ComboBox)
                        If .Tag = 53 Then
                            .Visible = m_bArchiveSoftwareEnabled
                            .Enabled = m_bArchiveSoftwareEnabled
                        End If
                    End With
                ElseIf TypeOf oControl Is Label Then
                    With CType(oControl, Label)
                        If .Text = "Timestamp Format:" Then
                            .Visible = m_bArchiveSoftwareEnabled
                            .Enabled = m_bArchiveSoftwareEnabled
                        End If

                        If Not String.IsNullOrEmpty(.Name) Then
                            If .Name = "lblSharepointUserName" OrElse .Name = "lblSharepointPassword" OrElse .Name = "lblAppClientId" OrElse .Name = "lblTenantId" Then
                                .Visible = m_bSharepointOnline
                            End If
                        End If
                    End With
                ElseIf TypeOf oControl Is TextBox Then
                    With CType(oControl, TextBox)
                        If Not String.IsNullOrEmpty(.Name) Then
                            If .Name = "txtOption5178" OrElse .Name = "txtOption5179" OrElse .Name = "txtOption5258" OrElse .Name = "txtOption5259" Then
                                .Visible = m_bSharepointOnline
                                If .Name = "txtOption5179" Then
                                    .PasswordChar = "*"
                                End If
                            End If

                            If .Name = "txtOption5191" OrElse .Name = "txtOption5192" OrElse .Name = "txtOption5193" OrElse .Name = "txtOption5205" OrElse .Name = "txtOption5186" OrElse .Name = "txtOption5195" Then
                                .PasswordChar = "*"
                            End If
                        End If
                    End With
                End If
                'Next oControl
            Next arrItem

            'Modified,add a for loop and a dim condition,comment the for loop
            For arrItem As Integer = 0 To arrControlList.Count - 1
                Dim oControl As Control = arrControlList(arrItem)
                'For Each oControl As Control In ContainerHelper.Controls(Me)
                If TypeOf oControl Is CheckBox Then
                    With oControl
                        If .Text.Trim() = "Digital Signature for PDF Documents enabled:" Then
                            If m_bArchiveSoftwareEnabled Then
                                .Visible = m_bArchiveSoftwareEnabled = m_bArchiveAsPDF
                            Else
                                .Visible = False
                            End If
                        End If
                    End With
                End If
                'Next oControl
            Next arrItem
        End If

        Dim nSelectedItemId As Integer = 0
        Dim nSelectedIndex As Integer = 0
        Dim nOptionNumber As Integer = CDbl(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex))
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypePaymentGateway OrElse nOptionNumber = GeneralConst.kSystemOptionRuleTypeCreditControl OrElse nOptionNumber = GeneralConst.kSystemOptionRuleTypeChaseCycle _
                            OrElse nOptionNumber = GeneralConst.kSystemOptionRuleTypeAddressLookup Then
            nSelectedItemId = DirectCast(cboOption(Index).SelectedItem, Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem).ItemData
            SetLableRuleTypeVal(nSelectedItemId, nOptionNumber)
            ShowHideRuleFileTxt(nSelectedItemId, nOptionNumber)
            ShowHideRuleFileBtn(nSelectedItemId, nOptionNumber)
            ShowHideRuleFileUct(nSelectedItemId, nOptionNumber)
        End If

        ''Document Production CCM
        If nOptionNumber = GeneralConst.kSystemOptionDocumentProductionSystem Then
            nSelectedIndex = cboOption(Index).SelectedIndex
            EnableDisableControlsForCCM(nSelectedIndex)
        End If

    End Sub

    '---------------------------------------------------------------------------------------
    ' Procedure : chkOption_Click
    ' DateTime  : 08/Oct/03 09:41
    ' Author    : AMB
    ' Purpose   : Process the enable/disable command (if any) of a check box
    ' Notes     : This relies on the 'EnableDisableParent' and 'EnableDisableChild'
    '             tag being set on the control that you want to trigger the enable/disable
    '             (parent) and the controls you want enabled/disabled (child).
    '             NOTE: You can only have one 'parent' control on a form, but this can
    '             enable/disable any number of controls on the form.
    '---------------------------------------------------------------------------------------
    '
    'Modified add an extra funtion
    Public Function ControlList(ByVal root As Control, ByRef resultArray As ArrayList) As ArrayList

        If root.HasChildren Then
            For Each cControl As Control In root.Controls
                resultArray.Add(cControl)
                If cControl.HasChildren Then
                    If Not cControl.Name.Contains("uct") Then
                        ControlList(cControl, resultArray)
                    End If
                End If
            Next cControl
        End If
        Return resultArray
    End Function

    Private Sub chkOption_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim Index As Integer = Array.IndexOf(chkOption, eventSender)

        Dim lId As Integer
        Dim sAdditionalData As String = String.Empty
        Dim sCommand As String = String.Empty
        Dim lWhatToDo As Integer
        Dim lEnableDisable As Integer
        ' 'Modified add an
        Dim arrControlList As ArrayList = New ArrayList
        Const klDoNothing As Integer = 0
        Const klDoEnable As Integer = 1
        Const klDoDisable As Integer = 2
        ' RDC 22/05/2005

        'Start (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.2)
        'End (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.2)
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.1)
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.1)

        Try

            'Modified add an if condition
            If Me.HasChildren Then
                ControlList(Me, arrControlList)
            End If

            m_lReturn = GetTagData(v_cControl:=chkOption(Index), r_lOptionIndex:=lId, v_sAdditionalData:=sAdditionalData)

            If lId = 0 Then
                Exit Sub
            End If

            sCommand = CStr(m_vOptionsConfiguration(ACCommand, lId))

            ' if we have the right tag, let's do the enable/disabling
            If sCommand = ACCommandEnableDisableParent Then

                lWhatToDo = klDoNothing

                Select Case chkOption(Index).CheckState
                    Case CheckState.Checked
                        ' need to enable each 'child' item
                        lWhatToDo = klDoEnable
                    Case CheckState.Unchecked
                        ' need to disable each 'child' item
                        lWhatToDo = klDoDisable
                    Case Else
                        ' do nothing
                        lWhatToDo = klDoNothing
                End Select

                If lWhatToDo <> klDoNothing Then

                    ' loop thru each control on the form
                    'Modified,add a for loop and a dim condition,comment the for loop
                    For arrItem As Integer = 0 To arrControlList.Count - 1
                        Dim conCurrControl As Control = arrControlList(arrItem)
                        'For Each conCurrControl As Control In ContainerHelper.Controls(Me)

                        ' get the tag etc for the current control
                        m_lReturn = GetTagData(v_cControl:=conCurrControl, r_lOptionIndex:=lId, v_sAdditionalData:=sAdditionalData)

                        If lId Then
                            sCommand = CStr(m_vOptionsConfiguration(ACCommand, lId))

                            ' check if the control is an 'EnableDisableChild'
                            If sCommand = ACCommandEnableDisableChild Then

                                lEnableDisable = lWhatToDo = klDoEnable
                                ' set enable/disable property of control
                                If TypeOf conCurrControl Is Button Then
                                    conCurrControl.Enabled = lEnableDisable
                                ElseIf TypeOf conCurrControl Is TextBox Then
                                    conCurrControl.Enabled = lEnableDisable
                                    CType(conCurrControl, TextBox).BackColor = IIf(lEnableDisable, ColorTranslator.ToOle(SystemColors.Window), ColorTranslator.ToOle(SystemColors.Control))
                                ElseIf TypeOf conCurrControl Is Label Then
                                    conCurrControl.Enabled = lEnableDisable
                                ElseIf TypeOf conCurrControl Is CheckBox Then
                                    conCurrControl.Enabled = lEnableDisable
                                ElseIf TypeOf conCurrControl Is RadioButton Then
                                    conCurrControl.Enabled = lEnableDisable
                                ElseIf TypeOf conCurrControl Is ComboBox Then
                                    conCurrControl.Enabled = lEnableDisable
                                End If

                            End If

                        End If
                        'Next conCurrControl
                    Next arrItem


                End If

            End If

            'Start (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.2)

            'Modified,add a for loop and a dim condition,comment the for loop
            For arrItem As Integer = 0 To arrControlList.Count - 1
                Dim oControl As Control = arrControlList(arrItem)
                'For Each oControl As Control In ContainerHelper.Controls(Me)
                If TypeOf oControl Is CheckBox Then
                    With CType(oControl, CheckBox)
                        If .Text.Trim() = ACEnhancedPersonalClientResolvedName Then
                            If .CheckState = CheckState.Checked Then
                                '-----------------Update Existing Clients-------------
                                'Modified,add a for loop and a dim condition,comment the for loop
                                For arrItem1 As Integer = 0 To arrControlList.Count - 1
                                    Dim oControlUpdate As Control = arrControlList(arrItem1)
                                    'For Each oControlUpdate As Control In ContainerHelper.Controls(Me)
                                    If TypeOf oControlUpdate Is CheckBox Then
                                        With oControlUpdate
                                            If .Text.Trim() = "Update Existing Clients" Then
                                                .Visible = True
                                                '.Value = 0
                                            End If
                                        End With
                                    End If
                                    'Next oControlUpdate
                                Next arrItem1

                            Else
                                'Modified,add a for loop and a dim condition,comment the for loop
                                For arrItem1 As Integer = 0 To arrControlList.Count - 1
                                    Dim oControlUpdate As Control = arrControlList(arrItem1)
                                    'For Each oControlUpdate As Control In ContainerHelper.Controls(Me)
                                    If TypeOf oControlUpdate Is CheckBox Then
                                        With oControlUpdate
                                            If .Text.Trim() = ACUpdateExistingClients Then
                                                CType(oControlUpdate, CheckBox).Checked = False
                                                .Visible = False
                                                '.Value = 0
                                            End If
                                        End With
                                    End If
                                    ' Next oControlUpdate
                                Next arrItem1
                                '-----------------Update Existing Clients-------------------
                            End If
                        End If

                        'Start - (Issue: 62332)' - Enable/Disable 'Agent Suspense Account' textbox
                        If .Text.Trim() = ACAgentCommissionSuspendedPosting Then
                            'Modified,add a for loop and a dim condition,comment the for loop
                            For arrItem1 As Integer = 0 To arrControlList.Count - 1
                                Dim oControlUpdate As Control = arrControlList(arrItem1)
                                'For Each oControlUpdate As Control In ContainerHelper.Controls(Me)
                                If TypeOf oControlUpdate Is TextBox Then
                                    m_lReturn = GetTagData(v_cControl:=oControlUpdate, r_lOptionIndex:=lId, v_sAdditionalData:=sAdditionalData)

                                    If CStr(m_vOptionsConfiguration(ACOptionNumber, lId)) = "5039" Then
                                        If .CheckState = CheckState.Checked Then
                                            txtOption(ContainerHelper.GetControlIndex(oControlUpdate)).Enabled = True
                                            txtOption(ContainerHelper.GetControlIndex(oControlUpdate)).Focus()
                                        Else
                                            txtOption(ContainerHelper.GetControlIndex(oControlUpdate)).Enabled = False
                                            txtOption(ContainerHelper.GetControlIndex(oControlUpdate)).Text = ""
                                        End If
                                    End If

                                End If
                                'Next oControlUpdate
                            Next arrItem1
                        End If
                        'End - (Issue: 62332) - Enable/Disable 'Agent Suspense Account' textbox

                        If .Text.Trim() = kExclusiveLocking Then
                            For arrItem1 As Integer = 0 To arrControlList.Count - 1
                                Dim oControlUpdate As Control = arrControlList(arrItem1)


                                'TextBox
                                m_lReturn = GetTagData(v_cControl:=oControlUpdate, r_lOptionIndex:=lId, v_sAdditionalData:=sAdditionalData)
                                If CStr(m_vOptionsConfiguration(ACOptionNumber, lId)) = "5175" Then
                                    If .CheckState = CheckState.Checked Then
                                        txtOption(ContainerHelper.GetControlIndex(oControlUpdate)).Visible = True
                                        txtOption(ContainerHelper.GetControlIndex(oControlUpdate)).Enabled = True
                                        txtOption(ContainerHelper.GetControlIndex(oControlUpdate)).Focus()
                                    Else
                                        txtOption(ContainerHelper.GetControlIndex(oControlUpdate)).Text = 0
                                        txtOption(ContainerHelper.GetControlIndex(oControlUpdate)).Enabled = False
                                        txtOption(ContainerHelper.GetControlIndex(oControlUpdate)).Visible = False
                                    End If
                                End If

                                'Lable
                                If CStr(m_vOptionsConfiguration(ACOptionNumber, lId)) = "5117" OrElse
                                CStr(m_vOptionsConfiguration(ACOptionNumber, lId)) = "5118" Then

                                    If .CheckState = CheckState.Checked Then
                                        lblOption(ContainerHelper.GetControlIndex(oControlUpdate)).Visible = True
                                    Else
                                        lblOption(ContainerHelper.GetControlIndex(oControlUpdate)).Visible = False
                                    End If

                                End If




                            Next
                        End If
                        If .Name = "chkOption5246" Then
                            If .CheckState = CheckState.Checked Then
                                m_bAutoReconOnCancellation = True

                            Else
                                m_bAutoReconOnCancellation = False

                            End If
                        End If

                    End With
                End If
                'Next oControl
            Next arrItem
            'End (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.2)
            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.1)
            'Modified,add a for loop and a dim condition,comment the for loop
            For arrItem As Integer = 0 To arrControlList.Count - 1
                Dim oControl As Control = arrControlList(arrItem)
                'For Each oControl As Control In ContainerHelper.Controls(Me)
                If TypeOf oControl Is CheckBox Then
                    With CType(oControl, CheckBox)
                        If .Text.Trim() = ACDisableAllWildcardSearches Then
                            If .CheckState = CheckState.Checked Then
                                For arrItem1 As Integer = 0 To arrControlList.Count - 1
                                    Dim oControlWildcard As Control = arrControlList(arrItem1)
                                    'For Each oControlWildcard As Control In ContainerHelper.Controls(Me)
                                    If TypeOf oControlWildcard Is CheckBox Then
                                        With oControlWildcard
                                            If .Text.Trim() = ACEnableWildcardSearchesEndingWith Then
                                                'If .Tag = 5066 Then
                                                .Enabled = False
                                                '.Value = 0
                                            End If
                                        End With
                                    End If
                                    'Next oControlWildcard
                                Next arrItem1
                            Else
                                For arrItem1 As Integer = 0 To arrControlList.Count - 1
                                    Dim oControlWildcard As Control = arrControlList(arrItem1)
                                    'For Each oControlWildcard As Control In ContainerHelper.Controls(Me)
                                    If TypeOf oControlWildcard Is CheckBox Then
                                        With oControlWildcard
                                            If .Text.Trim() = ACEnableWildcardSearchesEndingWith Then
                                                'If .Tag = 5066 Then
                                                .Enabled = True
                                                '.Value = 0
                                            End If
                                        End With
                                    End If
                                    'Next oControlWildcard
                                Next arrItem1
                            End If
                        End If

                        If .Text.Trim() = "Archive as PDF:" Then
                            m_bArchiveAsPDF = .Checked
                        End If

                        If Not String.IsNullOrEmpty(.Name) Then
                            If .Name = "chkOption5177" Then
                                m_bSharepointOnline = .Checked
                                .Enabled = m_bIsSharePoint
                            End If
                        End If

                        If .Text.Trim() = kClaimsReservesGross Then
                            For arrItem1 As Integer = 0 To arrControlList.Count - 1
                                Dim oControlUpdate As Control = arrControlList(arrItem1)

                                If TypeOf oControlUpdate Is Label Then
                                    With oControlUpdate
                                        If .Text.Trim() = "Tax Group" AndAlso CType(oControl, CheckBox).Checked Then
                                            .Enabled = True
                                            .Font = New Font(.Font, System.Drawing.FontStyle.Bold)
                                        ElseIf .Text.Trim() = "Tax Group" AndAlso Not CType(oControl, CheckBox).Checked Then
                                            .Enabled = False
                                            .Font = New Font(.Font, System.Drawing.FontStyle.Regular)
                                        End If
                                    End With
                                End If

                                If TypeOf oControlUpdate Is ComboBox Then
                                    With oControlUpdate
                                        If .Name.Trim() = "cboOption5240" AndAlso CType(oControl, CheckBox).Checked Then
                                            .Enabled = True
                                        ElseIf .Name.Trim() = "cboOption5240" AndAlso Not CType(oControl, CheckBox).Checked Then
                                            .Enabled = False
                                            CType(oControlUpdate, ComboBox).SelectedIndex = 0
                                        End If
                                    End With
                                End If
                            Next arrItem1
                        End If

                    End With
                End If
                ' Next oControl
            Next arrItem

            'Modified,add a for loop and a dim condition,comment the for loop
            For arrItem As Integer = 0 To arrControlList.Count - 1
                Dim oControl As Control = arrControlList(arrItem)
                ' For Each oControl As Control In ContainerHelper.Controls(Me)
                If TypeOf oControl Is CheckBox Then
                    With CType(oControl, CheckBox)
                        If .Text.Trim() = ACEnableWildcardSearchesEndingWith Then
                            'If .Tag = 5066 Then
                            If .CheckState = CheckState.Checked Then
                                For arrItem1 As Integer = 0 To arrControlList.Count - 1
                                    Dim oControlWildcard As Control = arrControlList(arrItem1)
                                    'For Each oControlWildcard As Control In ContainerHelper.Controls(Me)
                                    If TypeOf oControlWildcard Is CheckBox Then
                                        With oControlWildcard
                                            If .Text.Trim() = ACDisableAllWildcardSearches Then
                                                .Enabled = False
                                                '.Value = 0
                                            End If
                                        End With
                                    End If
                                    'Next oControlWildcard
                                Next arrItem1
                            Else
                                For arrItem1 As Integer = 0 To arrControlList.Count - 1
                                    Dim oControlWildcard As Control = arrControlList(arrItem1)
                                    'For Each oControlWildcard As Control In ContainerHelper.Controls(Me)
                                    If TypeOf oControlWildcard Is CheckBox Then
                                        With oControlWildcard
                                            If .Text.Trim() = ACDisableAllWildcardSearches Then
                                                .Enabled = True
                                                '.Value = 0
                                            End If
                                        End With
                                    End If
                                    'Next oControlWildcard
                                Next arrItem1
                            End If
                        End If

                        'For Each oControl As Control In ContainerHelper.Controls(Me)                       

                        If .Text.Trim() = "Digital Signature for PDF Documents enabled:" Then
                            If m_bArchiveAsPDF Then
                                .Visible = m_bArchiveAsPDF
                            Else
                                .Visible = False
                            End If
                        End If

                        If .Text.Trim() = "Payment HUB Integration" Then
                            If .CheckState = CheckState.Checked Then
                                m_bEnablePaymentHub = True
                            Else
                                m_bEnablePaymentHub = False
                            End If
                            EnableDisableControlsForPaymentHub(.Checked)
                        End If
                        If .Text.Trim() = "Authentication Integration" Then
                            If .CheckState = CheckState.Checked Then
                                m_bEnableAuthenticationConfig = True
                            Else
                                m_bEnableAuthenticationConfig = False
                            End If
                            EnableDisableControlsForAuthenticationIntegration(.Checked)
                        End If

                    End With

                ElseIf TypeOf oControl Is Label Then
                    With CType(oControl, Label)
                        If .Text = "Timestamp Format:" Then
                            .Visible = m_bArchiveSoftwareEnabled
                            .Enabled = m_bArchiveSoftwareEnabled
                        End If

                        If Not String.IsNullOrEmpty(.Name) Then
                            If .Name = "lblSharepointUserName" OrElse .Name = "lblSharepointPassword" OrElse .Name = "lblAppClientId" OrElse .Name = "lblTenantId" Then
                                .Visible = m_bSharepointOnline
                            End If
                            If .Name = "lblSharepointUserName" OrElse .Name = "lblSharepointPassword" OrElse .Name = "lblAppClientId" OrElse .Name = "lblTenantId" OrElse .Name = "lbl5086" OrElse .Name = "Label1" Then
                                If m_bSharepointOnline Then
                                    .Font = New Font(.Font, System.Drawing.FontStyle.Bold)
                                Else
                                    .Font = New Font(.Font, System.Drawing.FontStyle.Regular)
                                End If
                            End If
                            If .Name = "lblOption5247" OrElse
                          .Name = "lblOption5248" Then
                                If m_bAutoReconOnCancellation Then
                                    .Enabled = True
                                Else
                                    .Enabled = False
                                End If

                            End If
                        End If

                    End With

                ElseIf TypeOf oControl Is TextBox Then
                    With CType(oControl, TextBox)
                        If Not String.IsNullOrEmpty(.Name) Then
                            If .Name = "txtOption5178" OrElse .Name = "txtOption5179" OrElse .Name = "txtOption5258" OrElse .Name = "txtOption5259" Then
                                .Visible = m_bSharepointOnline
                                If .Name = "txtOption5179" Then
                                    .PasswordChar = "*"
                                End If
                                If Not m_bSharepointOnline Then
                                    .Text = String.Empty
                                End If
                            End If

                            If .Name = "txtOption5186" OrElse
                           .Name = "txtOption5191" OrElse
                           .Name = "txtOption5192" OrElse
                           .Name = "txtOption5193" OrElse
                           .Name = "txtOption5205" OrElse
                           .Name = "txtOption5195" Then
                                .PasswordChar = "*"
                            End If

                            If .Name = "txtOption5247" OrElse
                           .Name = "txtOption5248" Then
                                If m_bAutoReconOnCancellation Then
                                    .Enabled = True
                                Else
                                    .Enabled = False
                                End If

                            End If
                        End If
                    End With

                End If

                'Next oControl
            Next arrItem

            ''Media type Validation
            If Not String.IsNullOrEmpty(m_vOptionsConfiguration(ACOptionNumber, lId)) Then
                If CDbl(m_vOptionsConfiguration(ACOptionNumber, lId)) = GeneralConst.kSystemOptionMediaTypeIsCompliedRuleEnabled Then
                    ShowHideOnMediaTypeValidation(Index)
                End If
            End If

            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.1)
        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="chkOption_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="chkOption_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    ' ***************************************************************** '
    '
    ' Name: ShowHelp
    '
    ' Description: Display the help for this screen
    '
    ' History: 11/06/2004 CTAF - Created.
    '
    ' ***************************************************************** '
    'Modified by Archana Tokas on 5/12/2010 11:57:08 AM refer developer guide no. 20
    'Private Function ShowHelp(ByVal v_lContextID As Integer) As Integer
    Private Function ShowHelp() As Integer

        Dim result As Integer = 0
        Dim sHelpFile As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Find out from the registry where the Help File is
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile)
            'Modified by Archana Tokas on 5/12/2010 11:48:51 AM refer developer guide no. 20

            'dlgHelp.HelpFile = sHelpFile

            'dlgHelp.HelpContext = v_lContextID

            'dlgHelp.HelpCommand = cdlHelpContext

            'dlgHelp.ShowHelp()
            'Modified by Archana Tokas on 5/12/2010 11:48:51 AM refer developer guide no. 20
            System.Diagnostics.Process.Start(sHelpFile)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowHelp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowHelp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click


        'Modified by Archana Tokas on 5/12/2010 11:56:50 AM refer developer guide no. 20
        'm_lReturn = ShowHelp(cmdHelp.HelpContextID)
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, HelpContextID)

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim nReturn As Integer
        m_lReturn = ControlValidation()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return
        End If

        If m_bCreateAllPartyHistory Then
            nReturn = CreatePartyHistory()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreatePartyHistory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
            End If
        End If

        If m_bSharepointOnline Then
            m_lReturn = EncryptPwdSharepoint()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If

            m_lReturn = ValidateSharepointOnlineUrl()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If
        End If
        If m_bEnableAuthenticationConfig Then
            m_lReturn = ValidateAutenticationUrl()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If
        End If

        m_lReturn = UnloadControls()
        m_lReturn = SaveSystemOptionData()

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        Me.Hide()

    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim lReturn As Integer
        Dim sValue As String = ""

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            'SetMousePointer PMMouseBusy

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIROptions.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise")
                Exit Sub
            End If

            'Get reference to bGisSchemeBusiness
            Dim temp_m_oGisSchemeBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oGisSchemeBusiness, "bGISSchemeBusiness.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oGisSchemeBusiness = temp_m_oGisSchemeBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bGisSchemeBusiness", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise")

                Exit Sub
            End If

            'Get a reference to the scheme group class

            m_oSchemeGroup = m_oGisSchemeBusiness.SchemeGroup

            ' Get an instance of the pm lookup business object via
            ' the public object manager.
            Dim temp_m_oPMLookup As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLookup, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMLookup = temp_m_oPMLookup

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bPMLookup.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise")

                Exit Sub
            End If


            m_oPMLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            'Are we broking or underwriting
            m_lReturn = iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=m_sUnderwritingOrAgency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getUnderwritingOrAgency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise")
                Exit Sub
            End If

            'Get document template codes

            m_lReturn = m_oBusiness.GetDocCodes(r_vDocCodes:=m_vDocCodes)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the document_template codes.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise")

                    Exit Sub
                End If
            End If

            'Get user tables

            m_lReturn = m_oBusiness.GetUserTables(r_vUserTables:=m_vUserTables)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oBusiness.GetUserTables failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise")
                    Exit Sub
                End If
            End If


            If m_oBusiness.ManageSalvageSet Then
                m_bManageSalvageSet = True
            End If

            'Get user groups

            m_lReturn = m_oBusiness.GetUserGroups(r_vUserGroups:=m_vUserGroups)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oBusiness.GetUserGroups failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise")
                    Exit Sub
                End If
            End If

            ' if this is underwriting
            If m_sUnderwritingOrAgency = "U" Then

                ' get product option for underwriting year labelling
                lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear, g_oObjectManager.SourceID, sValue)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="getProductOptionValue Falied to return option for UnderwritingYear Labelling", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise")
                    Exit Sub
                End If

                If sValue = "1" Then
                    m_bUnderwritingYearLabelling = True
                End If

            End If

            ' WPR 63
            m_bMultipleVersions = False
            m_lReturn = m_oBusiness.CheckMultipleQuoteVersionRecords(m_bMultipleVersions)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oBusiness.CheckMultipleQuoteVersionRecords falied ", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise")
            End If

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub


    Private isInitializingComponent As Boolean
    Private m_sSharePointTenantId As String

    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If


        ' We are going for a best visual fit here, errors will occur as the form
        ' become too small to hold all controls. This is expected and safe!!

        Try

            ' Check minimum left/right spacing for splitter
            Dim lPosition As Integer = CInt(VB6.PixelsToTwipsX(imgSplitter.Left))
            'If lPosition < 600 Then ' Is within 600 twips of left border?
            If lPosition < 40 Then ' Is within 600 twips of left border?
                ' lPosition = 600
                lPosition = 40
            End If
            'If lPosition > (VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - 600) Then ' Is within 600 twips of right border?
            If lPosition > (VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - 40) Then ' Is within 600 twips of right border?
                'lPosition = (CInt(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - 600))
                lPosition = (CInt(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - 40))
            End If

            ' Get CommandButton positions
            'Dim lCmdTop As Integer = CInt(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdOK.Height) - (VB6.PixelsToTwipsY(sbrMain.Height) - 90))
            Dim lCmdTop As Integer = CInt(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdOK.Height) - (VB6.PixelsToTwipsY(sbrMain.Height) - 90))
            'Dim lCmdLeft As Integer = CInt(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - (VB6.PixelsToTwipsX(cmdOK.Width) + 120))
            Dim lCmdLeft As Integer = CInt(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - (VB6.PixelsToTwipsX(cmdOK.Width) + 8))

            ' Ensure proper size for splitter
            imgSplitter.SetBounds(VB6.TwipsToPixelsX(lPosition), 0, VB6.TwipsToPixelsX(75), Me.ClientRectangle.Height)

            ' Position other controls relative to splitter
            'tvwTabs.SetBounds(VB6.TwipsToPixelsX(120), VB6.TwipsToPixelsY(120), VB6.TwipsToPixelsX(lPosition - 120), VB6.TwipsToPixelsY(lCmdTop - 240))
            'tvwTabs.SetBounds(VB6.TwipsToPixelsX(120), VB6.TwipsToPixelsY(120), VB6.TwipsToPixelsX(lPosition - 120), VB6.TwipsToPixelsY(lCmdTop - 240))
            tvwTabs.Width = VB6.TwipsToPixelsX(lPosition - 120)
            'tabOptions.SetBounds(VB6.TwipsToPixelsX(lPosition) + imgSplitter.Width, VB6.TwipsToPixelsY(120), Me.ClientRectangle.Width - VB6.TwipsToPixelsX(lPosition + VB6.PixelsToTwipsX(imgSplitter.Width) + 120), VB6.TwipsToPixelsY(lCmdTop - 240))
            tabOptions.SetBounds(VB6.TwipsToPixelsX(lPosition) + imgSplitter.Width, VB6.TwipsToPixelsY(120), Me.ClientRectangle.Width - VB6.TwipsToPixelsX(lPosition + VB6.PixelsToTwipsX(imgSplitter.Width) + 120), VB6.TwipsToPixelsY(lCmdTop - 240))


            '    'Modified by Archana Tokas on 5/12/2010 11:57:44 AM changes to be checked later todolist
            '    'picContainer.SetBounds(VB6.TwipsToPixelsX(tabOptions.ClientLeft), VB6.TwipsToPixelsY(tabOptions.ClientTop), tabOptions.ClientSize.Width, tabOptions.ClientSize.Height)
            picContainer.SetBounds(tabOptions.Left, tabOptions.Top, tabOptions.ClientSize.Width, tabOptions.ClientSize.Height)

            '    ' move the buttons
            '    cmdHelp.SetBounds(VB6.TwipsToPixelsX(lCmdLeft), VB6.TwipsToPixelsY(lCmdTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            '    cmdCancel.SetBounds(VB6.TwipsToPixelsX(lCmdLeft - (VB6.PixelsToTwipsX(cmdOK.Width) + 105)), VB6.TwipsToPixelsY(lCmdTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            '    cmdOK.SetBounds(VB6.TwipsToPixelsX(lCmdLeft - (VB6.PixelsToTwipsX(cmdOK.Width) + 105) * 2), VB6.TwipsToPixelsY(lCmdTop), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

            '    ' send that statusbar to the back

            '    VB6.ZOrder(sbrMain, VB6.ZOrderConstants.SendToBack)

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        Try

            ' Terminate the business object
            If Not (m_oBusiness Is Nothing) Then

                m_oBusiness.Dispose()
                m_oBusiness = Nothing
            End If

            m_oSchemeGroup = Nothing

            If Not (m_oGisSchemeBusiness Is Nothing) Then

                m_oGisSchemeBusiness.Dispose()
                m_oGisSchemeBusiness = Nothing
            End If

            If Not (m_oPMLookup Is Nothing) Then

                m_oPMLookup.Dispose()
                m_oPMLookup = Nothing
            End If

            m_cLookupTables = Nothing
            m_cSystemOptionData = Nothing

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unload the interface form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Unload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            Dim vValue As String = ""
            Dim sOptionValue As String = String.Empty
            iPMFunc.ShowFormInTaskBar_Detach()

            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                Exit Sub
            End If

            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, 1, vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue for SIROPTEnableRI2007 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                Exit Sub
            End If

            m_bRI2007 = (gPMFunctions.ToSafeInteger(vValue) = 1)

            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnhancedChequeProduction, 1, vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue for SIROPTEnhancedChequeProduction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                Exit Sub
            End If

            m_bEnhancedChequeProduction = (gPMFunctions.ToSafeInteger(vValue) = 1)

            'S4BDAT003
            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnhancedAccountingBasis, 1, vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue for SIROPTEnhancedAccountingBasis Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                Exit Sub
            End If

            m_bEnhancedAccountingBasis = (gPMFunctions.ToSafeInteger(vValue) = 1)

            m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, 1, vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue for SIROPTEnableRI2007 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                Exit Sub
            End If

            m_bIsActivateBroker = (gPMFunctions.ToSafeInteger(vValue) = 1)

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kAddressLookupInstallation, r_sOptionValue:=sOptionValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getSystemOption for Address Lookup Installation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                Exit Sub
            End If

            If sOptionValue = "4" Then
                bUKPAFInstalled = True
            End If

            m_lReturn = BuildLookupTables()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildLookupTables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                Exit Sub
            End If

            m_lReturn = LoadSystemOptionData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadSystemOptionData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                Exit Sub
            End If


            m_lReturn = m_oBusiness.GetSystemOptionConfiguration(r_vResultArray:=m_vOptionsConfiguration)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetSystemOptionConfiguration Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                Exit Sub
            End If

            m_lReturn = SetupTabs()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetupTabs Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                Exit Sub
            End If

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub



    Private Sub cmdOption_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim Index As Integer = Array.IndexOf(cmdOption, eventSender)

        Dim lId As Integer
        Dim iIndex As Integer
        Dim bFound As Boolean
        Dim sAdditionalData As String = ""
        ' AMB 05/03/2003: PS220/123 - added for GetGenericFile
        Dim sBrowseType As String = ""
        Dim sCommandTemp() As String

        m_lReturn = GetTagData(v_cControl:=cmdOption(Index), r_lOptionIndex:=lId, v_sAdditionalData:=sAdditionalData)

        'lId = Val(cmdOption.Tag)

        If lId = 0 Then
            Exit Sub
        End If

        ' AMB 05/03/2003: PS220/123 - work out the file browse type, if any
        Dim sCommand As String = CStr(m_vOptionsConfiguration(ACCommand, lId))

        If sCommand.IndexOf(ACCommandBrowseForFile) + 1 Then
            ' get the browse type (ie. the file filter) from the second part of the string
            sCommandTemp = sCommand.Split("|"c)
            sBrowseType = sCommandTemp(1).ToUpper()
            ' set the command to just 'browse for file' for the select..case below
            sCommand = sCommandTemp(0)
        End If

        Select Case sCommand
            Case ACCommandGetDocument
                m_lReturn = GetCorrespondingControl(r_sControlType:="TextBox", v_lOptionNumber:=CInt(Conversion.Val(CStr(m_vOptionsConfiguration(ACOptionNumber, lId)))), r_iIndex:=iIndex, r_bFound:=bFound)
                m_lReturn = GetDocument(txtDocument:=txtOption(iIndex))
            Case ACCommandClearDocument
                m_lReturn = GetCorrespondingControl(r_sControlType:="TextBox", v_lOptionNumber:=CInt(Conversion.Val(CStr(m_vOptionsConfiguration(ACOptionNumber, lId)))), r_iIndex:=iIndex, r_bFound:=bFound)
                If bFound Then
                    txtOption(iIndex).Text = ACNoTemplate
                    'sj 25/04/2003 - Start
                    m_lReturn = SetTagData(r_cControl:=txtOption(iIndex), v_sAdditionalData:=CStr(0))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetTagData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOption_Click")
                        Exit Sub
                    End If
                    'sj 25/04/2003 - End
                End If
                ' AMB 05/03/2003: PS220/123 - this is retained for backward compatibility
                '   however the ACCommandBrowseForFile should be used in future (below)
            Case ACCommandClaimHelpFile
                m_lReturn = GetCorrespondingControl(r_sControlType:="TextBox", v_lOptionNumber:=CInt(Conversion.Val(CStr(m_vOptionsConfiguration(ACOptionNumber, lId)))), r_iIndex:=iIndex, r_bFound:=bFound)
                If bFound Then
                    m_lReturn = GetClaimHelpFile(r_cClaimHelpFile:=txtOption(iIndex))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimHelpFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOption_Click")
                        Exit Sub
                    End If
                End If
                ' AMB 05/03/2003: PS220/123 - added for Claims Roadmaps development - browse for generic file
            Case ACCommandBrowseForFile
                m_lReturn = GetCorrespondingControl(r_sControlType:="TextBox", v_lOptionNumber:=CInt(Conversion.Val(CStr(m_vOptionsConfiguration(ACOptionNumber, lId)))), r_iIndex:=iIndex, r_bFound:=bFound)
                If bFound Then
                    m_lReturn = GetGenericFile(v_sBrowseType:=sBrowseType, r_cGenericFile:=txtOption(iIndex))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGenericFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOption_Click")
                        Exit Sub
                    End If
                End If
            Case ACCommandCCMTemplateSync
                frmCCMTemplateSyncNotification.Visible = True
                Dim dRefreshAllCCMTemplates As New RefreshAllCCMTemplates(AddressOf RefreshCCMTemplates)

                ''using callback approach
                Dim oAsyncResult As IAsyncResult = dRefreshAllCCMTemplates.BeginInvoke(Index, New AsyncCallback(AddressOf HideLoadingMsg), Nothing)
                dRefreshAllCCMTemplates.EndInvoke(oAsyncResult)
        End Select

        If tvwTabs.SelectedNode.Text.ToLower() = "compiled rules" Then
            RuleEditor(cmdOption(Index).Name)
        End If
    End Sub

    ''' <summary>
    ''' Time taking task
    ''' This method is used to sync CCM templates
    ''' </summary>
    ''' <param name="nIndex"></param>
    ''' <remarks></remarks>
    Public Sub RefreshCCMTemplates(ByVal nIndex As Integer)
        cmdOption(nIndex).Enabled = False
        m_lReturn = m_oBusiness.RefreshCCMTemplates(True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshCCMTemplates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOption_Click")
            cmdOption(nIndex).Enabled = True
            Exit Sub
        End If
        cmdOption(nIndex).Enabled = True
    End Sub

    ''' <summary>
    ''' Hide Please wait msg
    ''' </summary>
    ''' <param name="oAsyncResult"></param>
    ''' <remarks></remarks>
    Public Sub HideLoadingMsg(ByVal oAsyncResult As IAsyncResult)
        frmCCMTemplateSyncNotification.Hide()
    End Sub

    Private Sub tvwTabs_AfterSelect(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwTabs.AfterSelect
        Dim Node As TreeNode = eventArgs.Node

        Static lOldID As Integer ' holds the id of the last node clicked
        ' RDC 22/09/2005

        Dim lId As Integer = CInt(Conversion.Val(Convert.ToString(Node.Tag)))

        If lId = 0 Then
            Exit Sub
        End If

        If lId <> lOldID Then ' only bother to redraw the screen if it's a new tab
            m_lReturn = ControlValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If

            'DD 28/11/2003: Added to clean up screen redraws.
            LockWindowUpdate(Me.Handle.ToInt32())
            'Developer Guide no.111
            'tabOptions.TabPages(1).Text = Node.Text
            tabOptions.TabPages(0).Text = Node.Text
            m_lReturn = ControlValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If
            m_lReturn = UnloadControls()
            m_lReturn = LoadControls(v_lId:=lId)

            ' RDC 22/05/2005

            For Each oControl As Control In ContainerHelper.Controls(Me)
                If TypeOf oControl Is CheckBox Then
                    With oControl
                        If .Text = "Auto-archive:" Or .Text = "Archive as PDF:" Then
                            .Visible = m_bArchiveSoftwareEnabled
                            .Enabled = m_bArchiveSoftwareEnabled
                        End If
                    End With
                End If
            Next oControl

            LockWindowUpdate(0)
        End If

        lOldID = lId
    End Sub

    Private Sub txtOption_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim Index As Integer = Array.IndexOf(txtOption, eventSender)

        Dim sAdditionalData As String = ""
        Dim lOptionIndex As Integer

        m_lReturn = GetTagData(v_cControl:=txtOption(Index), r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)

        Dim sCommand As String = CStr(m_vOptionsConfiguration(ACCommand, lOptionIndex)).Trim()
        Select Case sCommand
            Case ACCommandFormatCurrency
                If txtOption(Index).Text.Trim() <> "" Then
                    ' Peter Finney 01/07/2003
                    ' Removed Val() call as it does not handle commas ("123,456" = "123"!!)
                    ' Added check for numeric instead.
                    Dim dbNumericTemp As Double
                    If Double.TryParse(txtOption(Index).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        txtOption(Index).Text = StringsHelper.Format(txtOption(Index).Text, "#0")
                    Else
                        txtOption(Index).Text = ""
                    End If
                End If
        End Select

    End Sub

    Private Sub txtOption_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs)
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim Index As Integer = Array.IndexOf(txtOption, eventSender)

        Dim sAdditionalData As String = ""
        Dim lOptionIndex As Integer

        m_lReturn = GetTagData(v_cControl:=txtOption(Index), r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)

        Dim sCommand As String = CStr(m_vOptionsConfiguration(ACCommand, lOptionIndex)).Trim()

        Select Case sCommand
            Case ACCommandFormatCurrency, ACCommandValidateNumeric, ACCommandValidateNumericRange

                If eventSender.Tag.ToString() = "174" AndAlso (KeyAscii < Strings.Asc("0"c)) Or (KeyAscii > Strings.Asc("9"c)) Then
                    If (KeyAscii = Strings.Asc("."c) AndAlso Not eventSender.Text.ToString.Contains(".")) Then
                        'Nothing 
                    Else

                        If KeyAscii <> CInt(Keys.Back) Then
                            KeyAscii = 0
                        End If
                    End If
                Else

                    If (KeyAscii < Strings.Asc("0"c)) Or (KeyAscii > Strings.Asc("9"c)) Then
                        If KeyAscii <> CInt(Keys.Back) Then
                            KeyAscii = 0
                        End If
                    End If
                End If

                If sCommand = ACCommandValidateNumeric And KeyAscii <> CInt(Keys.Back) And eventSender.Tag.ToString() = "176" Then

                    'Present Value in the TextBox + The Key Pressed Value >  32767
                    If Convert.ToInt32(gPMFunctions.ToSafeInteger(eventSender.Text + Convert.ToChar(KeyAscii).ToString())) > 32767 Then
                        KeyAscii = 0
                    End If

                End If


            Case Else
                ' do nothing
        End Select

        'PSL 14/08/2003 5262 Stop them putting to many digits for a long
        If sCommand = ACCommandFormatCurrency Then
            If (KeyAscii >= Strings.Asc("0"c)) And (KeyAscii <= Strings.Asc("9"c)) Then
                If Strings.Len(txtOption(Index).Text) > 9 Then
                    KeyAscii = 0
                End If
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)

        Dim sValidateNumeric As String = CStr(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex)).Trim()
        If sValidateNumeric = ACDocOpenDelay OrElse sValidateNumeric = ACNumberMultiThread Then
            If Char.IsNumber(eventArgs.KeyChar) Or eventArgs.KeyChar = vbBack Then
            Else
                eventArgs.Handled = True
            End If
        End If
        If sValidateNumeric = ACUserPassword Then
            txtOption(Index).PasswordChar = "*"
        End If
    End Sub

    Private Sub txtOption_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim Index As Integer = Array.IndexOf(txtOption, eventSender)

        Dim sAdditionalData As String = ""
        Dim lOptionIndex As Integer

        m_lReturn = GetTagData(v_cControl:=txtOption(Index), r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)

        Dim sCommand As String = CStr(m_vOptionsConfiguration(ACCommand, lOptionIndex)).Trim()
        Select Case sCommand
            Case ACCommandFormatCurrency
                ' RAW 07/02/2003 : ISS2070 : added if test to prevent error when = ""
                If txtOption(Index).Text <> "" Then
                    txtOption(Index).Text = StringsHelper.Format(CInt(txtOption(Index).Text), "#,###,###,###,###,##0")
                End If
        End Select

        For Each cboOptionVal As ComboBox In cboOption
            If Not cboOptionVal Is Nothing Then
                If Not String.IsNullOrEmpty(cboOptionVal.Name) Then
                    If cboOptionVal.Name = "cboOption5243" Then
                        If txtOption(Index).Name.Trim.ToUpper = "TXTRSTOLERANCE" AndAlso Not String.IsNullOrEmpty(txtOption(Index).Text) Then
                            cboOptionVal.Enabled = True
                            txtOption(Index).Text = StringsHelper.Format(gPMFunctions.ToSafeDecimal(txtOption(Index).Text), "0.00")
                        Else
                            cboOptionVal.Enabled = False
                        End If
                    End If
                End If
            End If
        Next
        Dim sValidateNumeric As String = CStr(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex)).Trim()

        If sValidateNumeric = ACUserPassword Then
            txtOption(Index).PasswordChar = "*"
        End If


    End Sub

    ' ***************************************************************** '
    ' Name: txtOption_Validate
    '
    ' Parameters: n/a
    '
    ' Description: perform any validation specified for any text
    '                   controls additional commands
    ' History:
    '           Created : MEvans : 03-02-2003 : 213
    ' ***************************************************************** '
    Private Sub txtOption_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs)
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim Index As Integer = Array.IndexOf(txtOption, eventSender)

        Const sFunctionName As String = "txtOption_Validate"

        Dim sCommand As String
        Dim sCommandParameters As String = ""
        Dim lOptionIndex As Integer
        Dim vRanges As Object
        Dim sMin, sMax As String
        Dim sAdditionalData As String = String.Empty

        Try

            'Get lOption index
            m_lReturn = GetTagData(v_cControl:=txtOption(Index), r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)
            ' get command for selected control
            sCommand = CStr(m_vOptionsConfiguration(ACCommand, lOptionIndex)).Trim()
            sCommandParameters = CStr(m_vOptionsConfiguration(ACCommandParameters, lOptionIndex)).Trim()


            Select Case sCommand.ToUpper()
                ' use specified numeric range
                Case ACCommandValidateNumericRange.ToUpper()

                    ' get max min

                    vRanges = sCommandParameters.Split(":"c)

                    sMin = CStr(vRanges(0))

                    sMax = CStr(vRanges(1))

                    ' determine if we need to cancel
                    If CDec(txtOption(Index).Text) < CDec(sMin) Or CDec(txtOption(Index).Text) > CDec(sMax) Then
                        MessageBox.Show("The value must be between " & sMin & " and " & sMax, "Numeric Range Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        txtOption(Index).Focus()
                        Cancel = True
                    End If

                    'PSL 14/08/2003 5262 Stop them putting in too large a number for a long
                Case ACCommandFormatCurrency.ToUpper()
                    If Strings.Len(txtOption(Index).Text) = 10 Then
                        If txtOption(Index).Text > "2147483647" Then
                            MessageBox.Show("The value must be less than 2,147,483,648 ", "Numeric Range Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            txtOption(Index).Focus()
                            Cancel = True
                        End If
                    End If
                Case Else
                    ' do nothing
            End Select

        Catch
        End Try



        '******************************
        ' Log Error.
        gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))
        '*******************************

        eventArgs.Cancel = Cancel
    End Sub

    Private Sub Splitter1_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles imgSplitter.SplitterMoved
        'Dim Button As Integer = CInt(e.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = e.X
        Dim y As Single = e.Y

        ' Simple splitter movement
        ' Set position and force a resize (to redraw other controls in correct position)
        imgSplitter.Left += x
        frmInterface_Resize(Me, New EventArgs())

    End Sub

    Public Function IsRegexPatternValid(ByVal pattern As String) As Integer
        Dim result As Integer = 0
        Dim regex As Regex
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            If Trim(pattern) <> "" Then
                regex = New Regex(pattern)
            End If
            Return result

        Catch excep As System.Exception
            ' Error.
            result = gPMConstants.PMEReturnCode.PMError
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ControlValidation
    '
    ' Description:
    '
    '     '
    ' AMB 02-Oct-03: 1.8.6 - made controls ControlValidation
    ' ***************************************************************** '
    Private Function ControlValidation() As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lOptionIndex As Integer
            Dim sAdditionalData As String = ""

            For i As Integer = 1 To txtOption.Length - 1
                If Not txtOption(i) Is Nothing Then
                    m_lReturn = GetTagData(v_cControl:=txtOption(i), r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)
                    Select Case CStr(m_vOptionsConfiguration(ACCommand, lOptionIndex))
                        Case ACCommandRegex
                            If txtOption(i).Text <> "" Then
                                m_lReturn = IsRegexPatternValid(txtOption(i).Text)
                            End If
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                MessageBox.Show("Invalid regular expression - please review and update or select a new regular expression.", "Invalid Regx", MessageBoxButtons.OK)
                                txtOption(i).Focus()
                                Return m_lReturn
                            End If
                            m_sRegex = txtOption(i).Text
                        Case ACCommandFailureMsg
                            If txtOption(i).Text = "" And m_sRegex <> "" Then
                                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                MessageBox.Show("'Password Failure Message Text' can not be blank if a REGEX is set.", "Password Failure Message Text", MessageBoxButtons.OK)
                                txtOption(i).Focus()
                                Return m_lReturn
                            End If
                        Case ACCommandValidateRequired
                            Select Case CStr(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex))
                                Case "5178", "5179", "5085", "5086", "5258", "5259"
                                    If txtOption(i).Text = "" AndAlso m_bSharepointOnline Then
                                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                        MessageBox.Show("This is a Mandatory Field and cannot be blank.", "Mandatory Field", MessageBoxButtons.OK)
                                        txtOption(i).Focus()
                                        Return m_lReturn
                                    End If
                                Case "5185", "5186", "5187", "5188", "5189", "5241"
                                    If txtOption(i).Text = "" AndAlso m_bEnablePaymentHub Then
                                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                        MessageBox.Show("This is a Mandatory Field and cannot be blank.", "Mandatory Field", MessageBoxButtons.OK)
                                        txtOption(i).Focus()
                                        Return m_lReturn
                                    End If
                                Case "5250", "5251", "5252", "5253", "5254", "5255", "5256"
                                    If txtOption(i).Text = "" AndAlso m_bEnableAuthenticationConfig Then
                                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                        MessageBox.Show("This is a Mandatory Field and cannot be blank.", "Mandatory Field", MessageBoxButtons.OK)
                                        txtOption(i).Focus()
                                        Return m_lReturn
                                    End If
                            End Select
                        Case ACCommandValidateNumeric
                            If txtOption(i).Text = "" Then
                                Select Case CStr(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex))
                                    Case "5103", "5105", "5107", "5109", "5111", "5178", "5179", "5258", "5259"
                                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                        MessageBox.Show("This is a Mandatory Field and cannot be blank.", "Mandatory Field", MessageBoxButtons.OK)
                                        txtOption(i).Focus()
                                        Return m_lReturn
                                End Select
                            ElseIf System.Text.RegularExpressions.Regex.IsMatch(txtOption(i).Text, "^[A-Za-z]+$") Then
                                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                MessageBox.Show("This is a Numeric Field.", "Numeric Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                txtOption(i).Focus()
                                Return m_lReturn
                            Else
                                Select Case CStr(m_vOptionsConfiguration(ACOptionNumber, lOptionIndex))
                                    Case "5111"
                                        If CInt(txtOption(i).Text) > 10 Then
                                            txtOption(i).Text = ""
                                            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                            MessageBox.Show("Password Expiry warning (days) Can't be greater than 10.", "Password Expiry validation", MessageBoxButtons.OK)
                                            txtOption(i).Focus()
                                            Return m_lReturn
                                        End If
                                        If CInt(txtOption(i).Text) >= m_sPassexpDuration And m_sPassexpDuration <> 0 Then
                                            txtOption(i).Text = ""
                                            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                            MessageBox.Show("Password Expiry Duration(Days) should be greater than Password Expiry warning (days) field.", "Password Expiry validation", MessageBoxButtons.OK)
                                            txtOption(i).Focus()
                                            Return m_lReturn
                                        End If
                                    Case "5103"
                                        m_sPassexpDuration = txtOption(i).Text
                                        If CInt(txtOption(i).Text) = 0 AndAlso Not String.IsNullOrEmpty(txtOption(i + 4).Text) AndAlso (Not System.Text.RegularExpressions.Regex.IsMatch(txtOption(i + 4).Text, "^[A-Za-z]+$") AndAlso CInt(txtOption(i + 4).Text) <> 0) Then
                                            MessageBox.Show("If Password Expiry Duration(Days) is 0 then Password expiry warning (days) should also be set to 0.", "Password Expiry validation", MessageBoxButtons.OK)
                                            txtOption(i + 4).Text = ""
                                            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                            txtOption(i + 4).Focus()
                                            Return m_lReturn
                                        End If
                                End Select
                            End If
                    End Select
                End If
            Next i

            Dim CompiledRuleClassName As String = String.Empty
            Dim sObjectName As String = ""
            Dim sAssemblyName As String = ""
            Dim oRules As Object = Nothing
            Dim sRulePath As String = ""
            Dim sSubKey As String = "GIS"

            For i As Integer = 1 To UctCompiledRuleOption.Length - 1
                If Not UctCompiledRuleOption(i) Is Nothing Then
                    If UctCompiledRuleOption(i).Visible = True AndAlso UctCompiledRuleOption(i).Enabled = True Then
                        m_lReturn = GetTagData(v_cControl:=UctCompiledRuleOption(i), r_lOptionIndex:=lOptionIndex, v_sAdditionalData:=sAdditionalData)
                        CompiledRuleClassName = UctCompiledRuleOption(i).Text

                        If Trim(CompiledRuleClassName) = "" Then
                            MessageBox.Show("Please enter the Rating assembly and class name.", "Compiled Rules", MessageBoxButtons.OK)
                            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                            UctCompiledRuleOption(i).Focus()
                            Return m_lReturn
                        End If

                        sObjectName = Trim(CompiledRuleClassName)

                        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

                        If sRulePath <> "" Then
                            If Not sRulePath.EndsWith("\") Then
                                sRulePath = sRulePath & "\" & sObjectName
                            End If
                        End If

                        If sRulePath.Length > 255 Then
                            MessageBox.Show("The total length of rule folder path and Assembly.Class name should not exceed 255 characters.", "Compiled Rules", MessageBoxButtons.OK)
                            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                            UctCompiledRuleOption(i).Focus()
                            Return m_lReturn
                        End If

                        If Not UctCompiledRuleOption(i).bEnterOnlyAssemblyName Then
                            oRules = CreateLateBoundObject_CompiledRules(sObjectName)

                            If Not IsNothing(oRules) Then
                                MessageBox.Show(sObjectName + " validated successfully.", "Compiled Rules", MessageBoxButtons.OK)
                            End If

                            If Not String.IsNullOrEmpty(sObjectName) Then
                                sAssemblyName = sObjectName.Split(".")(0).Trim()
                            End If

                            If IsNothing(oRules) Then
                                MessageBox.Show("Unable to find compiled rule class " + sObjectName + ". Please ensure the " + sAssemblyName + " assembly is in the Rules folder, and the class name is correct. " +
                               "The format should be assemblyname.classname.", "Compiled Rules", MessageBoxButtons.OK)
                                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                UctCompiledRuleOption(i).Focus()
                                Return m_lReturn
                            End If
                        End If
                    End If
                End If
            Next

            ''if drpdown value is CCM doc production then fields in grpbox are mandatory
            If m_bIsCCMDocProduction Then
                For Each txtOptionVal As TextBox In txtOption
                    If Not txtOptionVal Is Nothing Then
                        If Not String.IsNullOrEmpty(txtOptionVal.Name) Then
                            If txtOptionVal.Name = "txtOption5164" _
                                OrElse txtOptionVal.Name = "txtOption5165" _
                                OrElse txtOptionVal.Name = "txtOption5166" _
                                OrElse txtOptionVal.Name = "txtOption5167" _
                                OrElse txtOptionVal.Name = "txtOption5168" _
                                OrElse txtOptionVal.Name = "txtOption5169" Then
                                If String.IsNullOrEmpty(txtOptionVal.Text) Then
                                    MessageBox.Show("This is a mandatory field. You must select data in this field.", "Mandatory Field - Document Production System", MessageBoxButtons.OK)
                                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                    txtOptionVal.Focus()
                                    Return m_lReturn
                                End If
                            End If
                        End If
                    End If
                Next

                ''get CCM web service URL
                Dim sCCMWebServiceURL As String = String.Empty
                For Each txtOptionVal As TextBox In txtOption
                    If Not txtOptionVal Is Nothing Then
                        If Not String.IsNullOrEmpty(txtOptionVal.Name) Then
                            If txtOptionVal.Name = "txtOption5164" Then
                                If Not String.IsNullOrEmpty(txtOptionVal.Text) Then
                                    sCCMWebServiceURL = txtOptionVal.Text
                                End If
                            End If
                        End If
                    End If
                Next

                For Each chkOptionVal As CheckBox In chkOption
                    If Not chkOptionVal Is Nothing Then
                        If Not String.IsNullOrEmpty(chkOptionVal.Name) Then
                            If chkOptionVal.Name = "chkOption5170" Then
                                ''check if SSL is checked or not
                                If chkOptionVal.Checked Then
                                    If Not String.IsNullOrEmpty(sCCMWebServiceURL) Then
                                        ''check web service URL starts with https or not
                                        If Not sCCMWebServiceURL.StartsWith("https") Then
                                            MessageBox.Show("The KCM Web Service URL must contain https if SSL is selected.", "Warning - Document Production System", MessageBoxButtons.OK)
                                            m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                            chkOptionVal.Focus()
                                            Return m_lReturn
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            For Each chkOptionVal As CheckBox In chkOption
                If Not chkOptionVal Is Nothing Then
                    If Not String.IsNullOrEmpty(chkOptionVal.Name) Then
                        If chkOptionVal.Name = "chkClaimsReserveGross" Then
                            If chkOptionVal.Checked Then
                                For Each cboOptionVal As ComboBox In cboOption
                                    If cboOptionVal IsNot Nothing AndAlso Not String.IsNullOrEmpty(cboOptionVal.Name) Then
                                        If cboOptionVal.Name = "cboOption5240" Then
                                            If cboOptionVal.SelectedIndex = 0 Then
                                                MessageBox.Show("This is a mandatory field. You must select data in this field.", "Mandatory Field - Claims", MessageBoxButtons.OK)
                                                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                                cboOptionVal.Focus()
                                                Return m_lReturn
                                            End If
                                        End If
                                    End If
                                Next
                            End If

                        End If
                    End If
                End If
            Next
            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ControlValidation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ControlValidation",
                               vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ''' <summary>
    ''' Function is used to encrypt the sharepoint online password
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function EncryptPwdSharepoint() As Integer
        Dim nResult As Integer = 0
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            Dim nOptionIndex As Integer
            Dim sAdditionalData As String = ""

            For i As Integer = 1 To txtOption.Length - 1
                m_lReturn = GetTagData(v_cControl:=txtOption(i), r_lOptionIndex:=nOptionIndex, v_sAdditionalData:=sAdditionalData)
                Select Case CStr(m_vOptionsConfiguration(ACOptionNumber, nOptionIndex))
                    Case "5179"
                        If txtOption(i).Text.Length > 0 AndAlso m_bSharepointOnline Then
                            Dim sEncryptedPassword As String = String.Empty
                            sEncryptedPassword = bPMFunc.EncryptPassword(txtOption(i).Text, PMEncryptionEntropy)
                            txtOption(i).Text = sEncryptedPassword
                        End If
                        Exit For
                End Select
            Next i

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncryptPwdSharepoint Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncryptPwdSharepoint",
                               vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function
    Private Function EncryptPwdAuthentication() As Integer
        Dim nResult As Integer = 0
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            Dim nOptionIndex As Integer
            Dim sAdditionalData As String = ""

            For i As Integer = 1 To txtOption.Length - 1
                m_lReturn = GetTagData(v_cControl:=txtOption(i), r_lOptionIndex:=nOptionIndex, v_sAdditionalData:=sAdditionalData)
                Select Case CStr(m_vOptionsConfiguration(ACOptionNumber, nOptionIndex))
                    Case "5250"
                        If txtOption(i).Text.Length > 0 AndAlso m_bEnableAuthenticationConfig Then
                            Dim sEncryptedPassword As String = String.Empty
                            sEncryptedPassword = bPMFunc.EncryptPassword(txtOption(i).Text, PMEncryptionEntropy)
                            txtOption(i).Text = sEncryptedPassword
                        End If
                        Exit For
                End Select
            Next i

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncryptPwdAuthentication Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncryptPwdAuthentication",
                               vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' Function is used to Validate sharepoint online URL
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateSharepointOnlineUrl() As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try

            Dim nOptionIndex As Integer
            Dim sAdditionalData As String = ""
            Dim sSharepointSite As String = String.Empty
            Dim sSharepointLibrary As String = String.Empty
            Dim sUserName As String = String.Empty
            Dim sPassword As String = String.Empty
            Dim sAppClientId As String = String.Empty
            Dim sSharepointTenantId As String = String.Empty
            Dim sResponse As String = String.Empty
            Dim nPasswordBoxIndex As Integer
            Dim sSharePointValidationTitle As String = "Sharepoint Server Validation"
            Dim SSharepointInvalidDetailMessage As String = "This User ID and Password does not have access to connect to the SharePoint Online."

            For i As Integer = 1 To txtOption.Length - 1
                nResult = GetTagData(v_cControl:=txtOption(i), r_lOptionIndex:=nOptionIndex, v_sAdditionalData:=sAdditionalData)
                Select Case CStr(m_vOptionsConfiguration(ACOptionNumber, nOptionIndex))

                    Case "5085"
                        sSharepointSite = txtOption(i).Text
                    Case "5086"
                        sSharepointLibrary = txtOption(i).Text
                    Case "5178"
                        sUserName = txtOption(i).Text
                    Case "5258"
                        sAppClientId = txtOption(i).Text
                    Case "5259"
                        sSharepointTenantId = txtOption(i).Text
                    Case "5179"
                        sPassword = bPMFunc.DecryptPassword(txtOption(i).Text, PMEncryptionEntropy)
                        nPasswordBoxIndex = i
                End Select
            Next i

            If ((m_sSharePointOnlineInitialURL.ToUpper().Trim() = sSharepointSite.ToUpper.Trim()) And
                (m_sSharePointOnlineInitialLibrary.ToUpper().Trim() = sSharepointLibrary.ToUpper.Trim()) And
                (m_sSharePointOnlineInitialUserId.ToUpper().Trim() = sUserName.ToUpper.Trim()) And
                (m_sSharePointOnlineInitialPassword.ToUpper().Trim() = sPassword.ToUpper.Trim()) And
                 (m_sSharePointOnlineAppClientId.ToUpper().Trim() = sAppClientId.ToUpper.Trim())) Then
                Return nResult
            End If

            frmSPOValidateNotification.Text = "Sharepoint Online"
            frmSPOValidateNotification.lblNotificationCCM.Text = "Please Wait. Configuring Sharepoint Online..."
            frmSPOValidateNotification.Visible = True
            frmSPOValidateNotification.Update()

            nResult = m_oBusiness.ValidateSharepointOnlineUrl(sSharepointSite:=sSharepointSite,
                                                sSharepointLibrary:=sSharepointLibrary,
                                                sUserName:=sUserName, sPassword:=sPassword, sResponse:=sResponse, sAppClientId:=sAppClientId, sSharepointTenantId:=sSharepointTenantId)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="ValidateSharepointOnlineUrl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateSharepointOnlineUrl")
                Return nResult
            End If

            frmSPOValidateNotification.Hide()

            If sResponse.Trim.Length > 0 Then
                txtOption(nPasswordBoxIndex).Text = bPMFunc.DecryptPassword(txtOption(nPasswordBoxIndex).Text, PMEncryptionEntropy)
                If (sResponse.Contains("sign-in")) Then
                    MessageBox.Show(SSharepointInvalidDetailMessage, sSharePointValidationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                ElseIf (sResponse.Contains("Standard configuration")) Then
                    MessageBox.Show(sResponse, sSharePointValidationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show(sResponse, sSharePointValidationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateSharepointOnlineUrl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncryptPwdSharepoint",
                                excep:=excep)
            Return nResult
        End Try

    End Function
    ''' <summary>
    ''' Function is used to Validate sharepoint online URL
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateAutenticationUrl() As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try

            Dim nOptionIndex As Integer
            Dim sAdditionalData As String = ""
            Dim sRealm As String = ""
            Dim sTokenUrl As String = String.Empty
            Dim sClientId As String = String.Empty
            Dim sClientSecret As String = String.Empty
            Dim sUserName As String = String.Empty
            Dim sPassword As String = String.Empty
            Dim sValidationTitle As String = "Autentication Server Validation"
            Dim sInvalidDetailMessage As String = "Unable to connect to Authentication Server.Please re-check the details."

            For i As Integer = 1 To txtOption.Length - 1
                nResult = GetTagData(v_cControl:=txtOption(i), r_lOptionIndex:=nOptionIndex, v_sAdditionalData:=sAdditionalData)
                Select Case CStr(m_vOptionsConfiguration(ACOptionNumber, nOptionIndex))
                    Case "5250"
                        sRealm = txtOption(i).Text
                    Case "5251"
                        sClientId = txtOption(i).Text
                    Case "5252"
                        sClientSecret = txtOption(i).Text
                    Case "5253"
                        sUserName = txtOption(i).Text
                    Case "5256"
                        sTokenUrl = txtOption(i).Text
                    Case "5254"
                        sPassword = txtOption(i).Text
                End Select
            Next i

            frmAuthValidateNotification.Text = "Authentication Server Connectivity Check"
            frmAuthValidateNotification.lblNotificationCCM.Text = "Please Wait. Trying to connect to Authentication Server..."
            frmAuthValidateNotification.Visible = True
            frmAuthValidateNotification.Update()

            Dim keyCloakConfigSettings As New KeyCloakConfiguration()
            keyCloakConfigSettings.Realm = sRealm
            keyCloakConfigSettings.client_id = sClientId
            keyCloakConfigSettings.client_secret = sClientSecret
            keyCloakConfigSettings.grant_type = "password"
            keyCloakConfigSettings.username = sUserName
            keyCloakConfigSettings.Password = sPassword
            keyCloakConfigSettings.TokenEndpoint = sTokenUrl
            Dim usersSync As New AuthenticationService(keyCloakConfigSettings)

            Try
                frmAuthValidateNotification.Hide()
                Dim task As Task(Of Models.KeycloakToken) = usersSync.GetAdminTokenAsync()
                If task IsNot Nothing AndAlso task.Result IsNot Nothing AndAlso String.IsNullOrEmpty(task.Result.AccessToken) = False Then
                    MessageBox.Show("Authentication Server Connected Successfully.", sValidationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("unable to connect to Authentication Server", sValidationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    For i As Integer = 1 To chkOption.Length - 1
                        nResult = GetTagData(v_cControl:=chkOption(i), r_lOptionIndex:=nOptionIndex, v_sAdditionalData:=sAdditionalData)
                        Select Case CStr(m_vOptionsConfiguration(ACOptionNumber, nOptionIndex))
                            Case "5249"
                                chkOption(i).Checked = False
                                Exit For
                        End Select
                    Next
                End If
            Catch
                nResult = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("unable to connect to Authentication Server", sValidationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="ValidateAuthenticationUrl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateAuthenticationUrl")
                Return nResult
            End Try

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateAuthenticationUrl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateAuthenticationUrl",
                                excep:=excep)
            Return nResult
        End Try

    End Function
    ''' <summary>
    ''' Set Rule Type label based on ddl value
    ''' </summary>
    ''' <param name="nSelectedItemId"></param>
    ''' <param name="nOptionNumber"></param>
    ''' <remarks></remarks>
    Private Sub SetLableRuleTypeVal(ByVal nSelectedItemId As Integer, ByVal nOptionNumber As Integer)
        ''Payment Gateway
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypePaymentGateway Then
            For Each lblOptionVal As Label In lblOption
                If Not lblOptionVal Is Nothing Then
                    If Not String.IsNullOrEmpty(lblOptionVal.Name) Then
                        If lblOptionVal.Name = "_lblOption01_3" Then
                            If nSelectedItemId = 1 Then
                                lblOptionVal.Text = "Rule File"
                            Else
                                lblOptionVal.Text = "Compiled Rule Assembly"
                            End If
                        End If
                    End If
                End If
            Next
        End If

        ''Credit Control
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypeCreditControl Then
            For Each lblOptionVal As Label In lblOption
                If Not lblOptionVal Is Nothing Then
                    If Not String.IsNullOrEmpty(lblOptionVal.Name) Then
                        If lblOptionVal.Name = "_lblOption01_1" Then
                            If nSelectedItemId = 1 Then
                                lblOptionVal.Text = "Rule File"
                            Else
                                lblOptionVal.Text = "Compiled Rule Assembly"
                            End If
                        End If
                    End If
                End If
            Next
        End If

        ''Chase Cycle
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypeChaseCycle Then
            For Each lblOptionVal As Label In lblOption
                If Not lblOptionVal Is Nothing Then
                    If Not String.IsNullOrEmpty(lblOptionVal.Name) Then
                        If lblOptionVal.Name = "_lblOption01_2" Then
                            If nSelectedItemId = 1 Then
                                lblOptionVal.Text = "Rule File"
                            Else
                                lblOptionVal.Text = "Compiled Rule Assembly"
                            End If
                        End If
                    End If
                End If
            Next
        End If

        ''Address Validation
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypeAddressLookup Then
            For Each lblOptionVal As Label In lblOption
                If Not lblOptionVal Is Nothing Then
                    If Not String.IsNullOrEmpty(lblOptionVal.Name) Then
                        If lblOptionVal.Name = "_lblOption01_4" Then
                            If bUKPAFInstalled Then
                                If nSelectedItemId = 1 Then
                                    lblOptionVal.Text = "PAF Script"
                                Else
                                    lblOptionVal.Text = "Compiled Rule Assembly"
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Show rule file textbox if .Rul script selected
    ''' </summary>
    ''' <param name="nSelectedItemId"></param>
    ''' <param name="nOptionNumber"></param>
    ''' <remarks></remarks>
    Private Sub ShowHideRuleFileTxt(ByVal nSelectedItemId As Integer, ByVal nOptionNumber As Integer)
        ''Payment Gateway
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypePaymentGateway Then
            For Each txtOptionVal As TextBox In txtOption
                If Not txtOptionVal Is Nothing Then
                    If Not String.IsNullOrEmpty(txtOptionVal.Name) Then
                        If txtOptionVal.Name = "txtOption5150" Then
                            If nSelectedItemId = 1 Then
                                txtOptionVal.Visible = True
                                txtOptionVal.Enabled = False
                                txtOptionVal.Text = "Payment_gateway.rul"
                            Else
                                txtOptionVal.Visible = False
                                txtOptionVal.Text = ""
                            End If
                        End If
                    End If
                End If
            Next
        End If

        ''Credit Control
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypeCreditControl Then
            For Each txtOptionVal As TextBox In txtOption
                If Not txtOptionVal Is Nothing Then
                    If Not String.IsNullOrEmpty(txtOptionVal.Name) Then
                        If txtOptionVal.Name = "txtOption5155" Then
                            If nSelectedItemId = 1 Then
                                txtOptionVal.Visible = True
                                txtOptionVal.Enabled = False
                                txtOptionVal.Text = "AUTO_CANCELLATION.rul"
                            Else
                                txtOptionVal.Visible = False
                                txtOptionVal.Text = ""
                            End If
                        End If
                    End If
                End If
            Next
        End If

        ''Chase Cycle
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypeChaseCycle Then
            For Each txtOptionVal As TextBox In txtOption
                If Not txtOptionVal Is Nothing Then
                    If Not String.IsNullOrEmpty(txtOptionVal.Name) Then
                        If txtOptionVal.Name = "txtOption5156" Then
                            If nSelectedItemId = 1 Then
                                txtOptionVal.Visible = True
                                txtOptionVal.Enabled = False
                                txtOptionVal.Text = "CHASE_CYCLE_AUTO_CANCELLATION.rul"
                            Else
                                txtOptionVal.Visible = False
                                txtOptionVal.Text = ""
                            End If
                        End If
                    End If
                End If
            Next
        End If

        ''Address Validation
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypeAddressLookup Then
            For Each txtOptionVal As TextBox In txtOption
                If Not txtOptionVal Is Nothing Then
                    If Not String.IsNullOrEmpty(txtOptionVal.Name) Then
                        If txtOptionVal.Name = "txtOption5157" Then
                            If bUKPAFInstalled Then
                                If nSelectedItemId = 1 Then
                                    txtOptionVal.Visible = True
                                    txtOptionVal.Enabled = False
                                    txtOptionVal.Text = "PAFWrapper.rul"
                                Else
                                    txtOptionVal.Visible = False
                                    txtOptionVal.Text = ""
                                End If
                            Else
                                txtOptionVal.Enabled = False
                                txtOptionVal.Text = ""
                            End If

                        End If
                    End If
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Show ellipsis btn if .Rul script selected
    ''' </summary>
    ''' <param name="nSelectedItemId"></param>
    ''' <param name="nOptionNumber"></param>
    ''' <remarks></remarks>
    Private Sub ShowHideRuleFileBtn(ByVal nSelectedItemId As Integer, ByVal nOptionNumber As Integer)
        ''Payment Gateway
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypePaymentGateway Then
            For Each cmdOptionVal As Button In cmdOption
                If Not cmdOptionVal Is Nothing Then
                    If Not cmdOptionVal Is Nothing Then
                        If Not String.IsNullOrEmpty(cmdOptionVal.Name) Then
                            If cmdOptionVal.Name = "cmdRuleFilePayG" Then
                                If nSelectedItemId = 1 Then
                                    cmdOptionVal.Visible = True
                                Else
                                    cmdOptionVal.Visible = False
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If

        ''Credit Control
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypeCreditControl Then
            For Each cmdOptionVal As Button In cmdOption
                If Not cmdOptionVal Is Nothing Then
                    If Not cmdOptionVal Is Nothing Then
                        If Not String.IsNullOrEmpty(cmdOptionVal.Name) Then
                            If cmdOptionVal.Name = "cmdRuleFileCC" Then
                                If nSelectedItemId = 1 Then
                                    cmdOptionVal.Visible = True
                                Else
                                    cmdOptionVal.Visible = False
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If

        ''Chase Cycle
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypeChaseCycle Then
            For Each cmdOptionVal As Button In cmdOption
                If Not cmdOptionVal Is Nothing Then
                    If Not cmdOptionVal Is Nothing Then
                        If Not String.IsNullOrEmpty(cmdOptionVal.Name) Then
                            If cmdOptionVal.Name = "cmdRuleFileChase" Then
                                If nSelectedItemId = 1 Then
                                    cmdOptionVal.Visible = True
                                Else
                                    cmdOptionVal.Visible = False
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If

        ''Address Validation
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypeAddressLookup Then
            For Each cmdOptionVal As Button In cmdOption
                If Not cmdOptionVal Is Nothing Then
                    If Not cmdOptionVal Is Nothing Then
                        If Not String.IsNullOrEmpty(cmdOptionVal.Name) Then
                            If cmdOptionVal.Name = "cmdRuleFileAddLkp" Then
                                If bUKPAFInstalled Then
                                    If nSelectedItemId = 1 Then
                                        cmdOptionVal.Visible = True
                                    Else
                                        cmdOptionVal.Visible = False
                                    End If
                                Else
                                    cmdOptionVal.Visible = False
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Show Compiled rule assembly auto complete txtbox if .Rul script selected
    ''' </summary>
    ''' <param name="nSelectedItemId"></param>
    ''' <param name="nOptionNumber"></param>
    ''' <remarks></remarks>
    Private Sub ShowHideRuleFileUct(ByVal nSelectedItemId As Integer, ByVal nOptionNumber As Integer)
        ''Payment Gateway
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypePaymentGateway Then
            For Each uctCompiledRuleOptionVal As uctCompiledRule.uctCompiledRule In UctCompiledRuleOption
                If Not uctCompiledRuleOptionVal Is Nothing Then
                    If Not String.IsNullOrEmpty(uctCompiledRuleOptionVal.Name) Then
                        If uctCompiledRuleOptionVal.Name = "UctCompiledRuleOption03" Then
                            If nSelectedItemId = 3 Then
                                uctCompiledRuleOptionVal.Visible = True
                            Else
                                uctCompiledRuleOptionVal.Visible = False
                                uctCompiledRuleOptionVal.Text = ""
                            End If
                        End If
                    End If
                End If
            Next
        End If

        ''Credit Control
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypeCreditControl Then
            For Each uctCompiledRuleOptionVal As uctCompiledRule.uctCompiledRule In UctCompiledRuleOption
                If Not uctCompiledRuleOptionVal Is Nothing Then
                    If Not String.IsNullOrEmpty(uctCompiledRuleOptionVal.Name) Then
                        If uctCompiledRuleOptionVal.Name = "UctCompiledRuleOption01" Then
                            If nSelectedItemId = 3 Then
                                uctCompiledRuleOptionVal.Visible = True
                            Else
                                uctCompiledRuleOptionVal.Visible = False
                                uctCompiledRuleOptionVal.Text = ""
                            End If
                        End If
                    End If
                End If
            Next
        End If

        ''Chase Cycle
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypeChaseCycle Then
            For Each uctCompiledRuleOptionVal As uctCompiledRule.uctCompiledRule In UctCompiledRuleOption
                If Not uctCompiledRuleOptionVal Is Nothing Then
                    If Not String.IsNullOrEmpty(uctCompiledRuleOptionVal.Name) Then
                        If uctCompiledRuleOptionVal.Name = "UctCompiledRuleOption02" Then
                            If nSelectedItemId = 3 Then
                                uctCompiledRuleOptionVal.Visible = True
                            Else
                                uctCompiledRuleOptionVal.Visible = False
                                uctCompiledRuleOptionVal.Text = ""
                            End If
                        End If
                    End If
                End If
            Next
        End If

        ''Address Validation
        If nOptionNumber = GeneralConst.kSystemOptionRuleTypeAddressLookup Then
            For Each uctCompiledRuleOptionVal As uctCompiledRule.uctCompiledRule In UctCompiledRuleOption
                If Not uctCompiledRuleOptionVal Is Nothing Then
                    If Not String.IsNullOrEmpty(uctCompiledRuleOptionVal.Name) Then
                        If uctCompiledRuleOptionVal.Name = "UctCompiledRuleOption04" Then
                            If bUKPAFInstalled Then
                                If nSelectedItemId = 3 Then
                                    uctCompiledRuleOptionVal.Visible = True
                                Else
                                    uctCompiledRuleOptionVal.Visible = False
                                    uctCompiledRuleOptionVal.Text = ""
                                End If
                            Else
                                uctCompiledRuleOptionVal.Visible = False
                                uctCompiledRuleOptionVal.Text = ""
                            End If

                        End If
                    End If
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Show Compiled rule assembly auto complete txtbox if enabled compiled rule checked
    ''' </summary>
    ''' <param name="nIndex"></param>
    ''' <remarks></remarks>
    Private Sub ShowHideOnMediaTypeValidation(ByVal nIndex As Integer)
        For Each UctOptionVal As uctCompiledRule.uctCompiledRule In UctCompiledRuleOption
            If Not UctOptionVal Is Nothing Then
                If UctOptionVal.Name = "UctCompiledRuleOption05" Then
                    If chkOption(nIndex).CheckState = CheckState.Checked Then
                        UctOptionVal.Visible = True
                        UctOptionVal.Enabled = True
                    Else
                        UctOptionVal.Text = ""
                        UctOptionVal.Enabled = False
                    End If
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Open rule file on btn click
    ''' </summary>
    ''' <param name="cmdOptionName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RuleEditor(ByVal cmdOptionName As String) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".RuleEditor")
        Dim oRuleEditor As iPMURuleEditor.Interface_Renamed
        Dim sRuleFileName As String = String.Empty
        Dim sRulePath As String = String.Empty

        Dim temp_oRuleEditor As Object = Nothing
        nResult = g_oObjectManager.GetInstance(temp_oRuleEditor, sClassName:="iPMURuleEditor.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oRuleEditor = temp_oRuleEditor

        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        For Each txtOptionVal As TextBox In txtOption
            If Not txtOptionVal Is Nothing Then
                If txtOptionVal.Visible = True Then
                    Select Case cmdOptionName
                        Case "cmdRuleFileCC"
                            If txtOptionVal.Name = "txtOption5155" Then
                                sRuleFileName = txtOptionVal.Text.Trim()
                            End If
                        Case "cmdRuleFileChase"
                            If txtOptionVal.Name = "txtOption5156" Then
                                sRuleFileName = txtOptionVal.Text.Trim()
                            End If
                        Case "cmdRuleFilePayG"
                            If txtOptionVal.Name = "txtOption5150" Then
                                sRuleFileName = txtOptionVal.Text.Trim()
                            End If
                        Case "cmdRuleFileAddLkp"
                            If txtOptionVal.Name = "txtOption5157" Then
                                sRuleFileName = txtOptionVal.Text.Trim()
                            End If
                    End Select
                End If
            End If
        Next


        nResult = CType(GetRulePath(r_sRulePath:=sRulePath), gPMConstants.PMEReturnCode)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not (oRuleEditor Is Nothing) Then
            'set the default editor values
            oRuleEditor.RulePath = sRulePath
            oRuleEditor.FixedFile = True
            oRuleEditor.RuleFileName = sRuleFileName
            oRuleEditor.Start()
            If oRuleEditor.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                nResult = gPMConstants.PMEReturnCode.PMTrue
            End If
            oRuleEditor.Dispose()
            oRuleEditor = Nothing
        End If

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".RuleEditor")

        Return nResult
    End Function

    ''' <summary>
    ''' Get path for rule files
    ''' </summary>
    ''' <param name="r_sRulePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetRulePath(ByRef r_sRulePath As String) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sSubKey As String = ""

        sSubKey = "GIS"

        nResult = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=r_sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

        If r_sRulePath = "" Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
        Else
            If Not r_sRulePath.EndsWith("\") Then
                r_sRulePath = r_sRulePath & "\"
            End If
        End If

        Return nResult

    End Function

    Private Sub EnableDisableControlsForCCM(ByVal nSelectedIndex As Integer)
        For Each txtOptionVal As TextBox In txtOption
            If Not txtOptionVal Is Nothing Then
                If Not String.IsNullOrEmpty(txtOptionVal.Name) Then
                    If txtOptionVal.Name = "txtOption5164" _
                        OrElse txtOptionVal.Name = "txtOption5165" _
                        OrElse txtOptionVal.Name = "txtOption5166" _
                        OrElse txtOptionVal.Name = "txtOption5167" _
                        OrElse txtOptionVal.Name = "txtOption5168" _
                        OrElse txtOptionVal.Name = "txtOption5169" _
                        OrElse txtOptionVal.Name = "txtOption5173" Then
                        If nSelectedIndex = 1 Then
                            'show controls in grpbox if CCM doc production selected
                            txtOptionVal.Enabled = True
                            m_bIsCCMDocProduction = True
                        Else
                            'else hide controls in grpbox
                            txtOptionVal.Enabled = False
                            txtOptionVal.Text = String.Empty
                            m_bIsCCMDocProduction = False
                        End If
                    End If
                End If
            End If
        Next

        For Each chkOptionVal As CheckBox In chkOption
            If Not chkOptionVal Is Nothing Then
                If Not String.IsNullOrEmpty(chkOptionVal.Name) Then
                    If chkOptionVal.Name = "chkOption5170" Then
                        If nSelectedIndex = 1 Then
                            'show controls in grpbox if CCM doc production selected
                            chkOptionVal.Enabled = True
                        Else
                            'else hide controls in grpbox
                            chkOptionVal.Enabled = False
                        End If
                    End If
                    If chkOptionVal.Name = "chkOption5207" Then
                        If nSelectedIndex = 1 Then
                            chkOptionVal.Enabled = True
                        Else
                            chkOptionVal.Checked = False
                            chkOptionVal.Enabled = False
                        End If
                    End If
                End If
            End If
        Next

        For Each lblOptionVal As Label In lblOption
            If Not lblOptionVal Is Nothing Then
                If Not String.IsNullOrEmpty(lblOptionVal.Name) Then
                    If lblOptionVal.Name = "lblDocProduction" Then
                        lblOptionVal.Font = VB6.FontChangeBold(lblOptionVal.Font, True)
                    ElseIf lblOptionVal.Name = "lblCCMWebURL" _
                        OrElse lblOptionVal.Name = "lblPartner" _
                        OrElse lblOptionVal.Name = "lblCustomer" _
                        OrElse lblOptionVal.Name = "lblConrtactTypeName" _
                        OrElse lblOptionVal.Name = "lblRepositoryProj" _
                        OrElse lblOptionVal.Name = "lblContractTypeVersion" _
                        OrElse lblOptionVal.Name = "lblCCMStatus" Then
                        If nSelectedIndex = 1 Then
                            'make label bold i.e. mandatory
                            lblOptionVal.Font = VB6.FontChangeBold(lblOptionVal.Font, True)
                        Else
                            lblOptionVal.Font = VB6.FontChangeBold(lblOptionVal.Font, False)
                        End If
                    End If
                End If
            End If
        Next

        For Each cboOptionVal As ComboBox In cboOption
            If Not cboOptionVal Is Nothing Then
                If Not String.IsNullOrEmpty(cboOptionVal.Name) Then
                    If cboOptionVal.Name = "cmbOption5171" Then
                        If nSelectedIndex = 1 Then
                            cboOptionVal.Enabled = True
                        Else
                            cboOptionVal.Enabled = False
                        End If

                    End If
                End If
            End If
        Next

        For Each cmdOptionVal As Button In cmdOption
            If Not cmdOptionVal Is Nothing Then
                If Not String.IsNullOrEmpty(cmdOptionVal.Name) Then
                    If cmdOptionVal.Name = "btnCCMTemplateSync" Then
                        If nSelectedIndex = 1 Then
                            'enable controls in grpbox if CCM doc production selected
                            cmdOptionVal.Enabled = True
                        Else
                            'else disable controls in grpbox
                            cmdOptionVal.Enabled = False
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Private Function TruncateHash(ByVal key As String, ByVal length As Integer) As Byte()

        Dim sha1 As New SHA1CryptoServiceProvider

        ' Hash the key. 
        Dim keyBytes() As Byte =
            System.Text.Encoding.Unicode.GetBytes(key)
        Dim hash() As Byte = sha1.ComputeHash(keyBytes)

        ' Truncate or pad the hash. 
        ReDim Preserve hash(length - 1)
        Return hash
    End Function

    Public Function GetEVal(ByVal plaintext As String) As String
        Dim sKey As String = "!@$1R1U5"

        Dim TripleDes As New TripleDESCryptoServiceProvider

        TripleDes.Key = TruncateHash(sKey, TripleDes.KeySize \ 8)
        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)

        Dim sRetVal As String = ""

        ' Convert the plaintext string to a byte array. 
        Dim plaintextBytes() As Byte =
            System.Text.Encoding.Unicode.GetBytes(plaintext)

        ' Create the stream. 
        Dim ms As New System.IO.MemoryStream
        ' Create the encoder to write to the stream. 
        Dim encStream As New CryptoStream(ms,
                                    TripleDes.CreateEncryptor(),
                                    System.Security.Cryptography.CryptoStreamMode.Write)

        ' Use the crypto stream to write the byte array to the stream.
        encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
        encStream.FlushFinalBlock()

        ' Convert the encrypted stream to a printable string. 
        sRetVal = Convert.ToBase64String(ms.ToArray)

        TripleDes = Nothing
        Return sRetVal
    End Function

    Public Function GetOVal(ByVal encryptedtext As String) As String
        Dim sRetVal As String = ""

        Dim TripleDes As New TripleDESCryptoServiceProvider
        Dim sKey As String = "!@$1R1U5"
        TripleDes.Key = TruncateHash(sKey, TripleDes.KeySize \ 8)
        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)


        Try

            ' Convert the encrypted text string to a byte array. 
            Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

            ' Create the stream. 
            Dim ms As New System.IO.MemoryStream
            ' Create the decoder to write to the stream. 
            Dim decStream As New CryptoStream(ms,
                TripleDes.CreateDecryptor(),
                System.Security.Cryptography.CryptoStreamMode.Write)

            ' Use the crypto stream to write the byte array to the stream.
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
            decStream.FlushFinalBlock()

            ' Convert the plaintext stream to a string. 
            sRetVal = System.Text.Encoding.Unicode.GetString(ms.ToArray)

            TripleDes = Nothing
        Catch ex As Exception
            Return ""
        End Try

        Return sRetVal
    End Function

    Private Sub EnableDisableControlsForPaymentHub(ByVal bCheckState As Boolean)
        For Each txtOptionVal As TextBox In txtOption
            If Not txtOptionVal Is Nothing Then
                If Not String.IsNullOrEmpty(txtOptionVal.Name) Then
                    If txtOptionVal.Name = "txtOption5185" _
                        OrElse txtOptionVal.Name = "txtOption5186" _
                        OrElse txtOptionVal.Name = "txtOption5187" _
                        OrElse txtOptionVal.Name = "txtOption5188" _
                        OrElse txtOptionVal.Name = "txtOption5189" _
                        OrElse txtOptionVal.Name = "txtOption5190" _
                        OrElse txtOptionVal.Name = "txtOption5241" Then
                        If bCheckState = True Then
                            'show controls in grpbox if CCM doc production selected
                            txtOptionVal.Enabled = True
                            m_bEnablePaymentHub = True
                        Else
                            'else hide controls in grpbox
                            txtOptionVal.Enabled = False
                            m_bEnablePaymentHub = False
                        End If
                    End If
                    If txtOptionVal.Name = "txtOption5190" Then
                        txtOptionVal.Multiline = True
                        txtOptionVal.Height = 50
                    End If
                End If
            End If
        Next

        For Each lblOptionVal As Label In lblOption
            If Not lblOptionVal Is Nothing Then
                If Not String.IsNullOrEmpty(lblOptionVal.Name) Then
                    If lblOptionVal.Name = "lblOption5185" _
                            OrElse lblOptionVal.Name = "lblOption5186" _
                            OrElse lblOptionVal.Name = "lblOption5187" _
                            OrElse lblOptionVal.Name = "lblOption5188" _
                            OrElse lblOptionVal.Name = "lblOption5189" _
                            OrElse lblOptionVal.Name = "lblOption5241" Then
                        If bCheckState = True Then
                            'make label bold i.e. mandatory
                            lblOptionVal.Font = VB6.FontChangeBold(lblOptionVal.Font, True)
                        Else
                            lblOptionVal.Font = VB6.FontChangeBold(lblOptionVal.Font, False)
                        End If
                    End If
                End If
            End If
        Next

    End Sub
    Private Sub EnableDisableControlsForAuthenticationIntegration(ByVal bCheckState As Boolean)
        For Each txtOptionVal As TextBox In txtOption
            If Not txtOptionVal Is Nothing Then
                If Not String.IsNullOrEmpty(txtOptionVal.Name) Then
                    If txtOptionVal.Name = "txtOption5250" _
                        OrElse txtOptionVal.Name = "txtOption5251" _
                        OrElse txtOptionVal.Name = "txtOption5252" _
                        OrElse txtOptionVal.Name = "txtOption5253" _
                        OrElse txtOptionVal.Name = "txtOption5254" _
                        OrElse txtOptionVal.Name = "txtOption5255" _
                        OrElse txtOptionVal.Name = "txtOption5256" Then
                        If bCheckState = True Then
                            'show controls in grpbox if CCM doc production selected
                            txtOptionVal.Enabled = True
                            m_bEnableAuthenticationConfig = True
                        Else
                            'else hide controls in grpbox
                            txtOptionVal.Enabled = False
                            m_bEnableAuthenticationConfig = False
                        End If
                    End If

                End If
            End If
        Next

        For Each lblOptionVal As Label In lblOption
            If Not lblOptionVal Is Nothing Then
                If Not String.IsNullOrEmpty(lblOptionVal.Name) Then
                    If lblOptionVal.Name = "lblOption5250" _
                            OrElse lblOptionVal.Name = "lblOption5251" _
                            OrElse lblOptionVal.Name = "lblOption5252" _
                            OrElse lblOptionVal.Name = "lblOption5253" _
                            OrElse lblOptionVal.Name = "lblOption5254" _
                            OrElse lblOptionVal.Name = "lblOption5255" _
                            OrElse lblOptionVal.Name = "lblOption5256" Then
                        If bCheckState = True Then
                            'make label bold i.e. mandatory
                            lblOptionVal.Font = VB6.FontChangeBold(lblOptionVal.Font, True)
                        Else
                            lblOptionVal.Font = VB6.FontChangeBold(lblOptionVal.Font, False)
                        End If
                    End If
                End If
            End If
        Next

    End Sub
    Private Sub chkOption_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim Index As Integer = Array.IndexOf(chkOption, eventSender)
        Dim lId As Integer
        Dim sAdditionalData, sCommand As String
        Dim arrControlList As ArrayList = New ArrayList
        Dim bChecked As Boolean = False
        Try

            If Me.HasChildren Then
                ControlList(Me, arrControlList)
            End If

            m_lReturn = GetTagData(v_cControl:=chkOption(Index), r_lOptionIndex:=lId, v_sAdditionalData:=sAdditionalData)

            If lId = 0 Then
                Exit Sub
            End If

            sCommand = CStr(m_vOptionsConfiguration(ACCommand, lId))

            'Modified add an if condition
            If Me.HasChildren Then
                ControlList(Me, arrControlList)
            End If

            m_lReturn = GetTagData(v_cControl:=chkOption(Index), r_lOptionIndex:=lId, v_sAdditionalData:=sAdditionalData)

            If lId = 0 Then
                Exit Sub
            End If

            sCommand = CStr(m_vOptionsConfiguration(ACCommand, lId))

            For arrItem As Integer = 0 To arrControlList.Count - 1
                Dim oControl As Control = arrControlList(arrItem)
                If TypeOf oControl Is CheckBox Then
                    With CType(oControl, CheckBox)
                        If .Text.Trim() = kPartyHistoryLoggingEnabled Then
                            If .CheckState = CheckState.Checked Then
                                bChecked = True
                            End If
                        End If
                    End With
                End If
            Next
            If bChecked = True Then
                If MessageBox.Show("Would you like to update all party records for the party history table?", "Party History Logging Enabled", MessageBoxButtons.OKCancel) = Windows.Forms.DialogResult.OK Then
                    m_bCreateAllPartyHistory = True
                End If
            End If
        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="chkOption_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="chkOption_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        End Try
    End Sub

    Private Function CreatePartyHistory() As Integer
        Dim nReturn As Integer
        Dim kMethodName As String = "CreatePartyHistory"
        Try
            Dim oSirPartyBusiness As Object = Nothing
            nReturn = g_oObjectManager.GetInstance(oSirPartyBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRPartyBusiness", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                Exit Function
            End If

            ' Create Schema File
            nReturn = oSirPartyBusiness.CreateAndSavePartyHistorySchema()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Party History Schema.", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                Exit Function
            End If

            Dim oPartCnt As Object = Nothing
            nReturn = oSirPartyBusiness.GetAllParties(oPartCnt)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all party cnt.", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                Exit Function
            End If

            For i As Integer = 0 To oPartCnt.Length - 1
                nReturn = oSirPartyBusiness.AddPartyHistory(CInt(oPartCnt(0, i)), String.Empty)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create Party History", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)

                End If
            Next

        Catch excep As System.Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        End Try
        Return nReturn
    End Function

End Class

Public NotInheritable Class SiriusUserDefaults

#Region "Constructors"
    Private Sub New()
        ' This class cannot be instantiated.
    End Sub
#End Region

    Public Const Username As String = "sirius"
    Public Const Password As String = "XctqMUbg"
    Public Const LanguageID As Int16 = 1
    Public Const CurrencyID As Int16 = 26
    Public Const SourceID As Int16 = 1
    Public Const UserID As Int32 = 1
    Public Const LogLevel As Int16 = 9
    Public Const AppName As String = "SSPPureWindowsService"
End Class
