Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Partial Friend Class frmRIBrokerParticipants
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 17/02/1997
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmRIBrokerParticipants"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_sLongName As String = ""
    Private m_sPostalCode As String = ""
    Private m_sFileCode As String = ""
    Private m_iNotEditable As Integer
    Private m_bDeleteMode As Boolean
    Private m_vSourceArray As Object
    'Array for holding data for include Closed branch as well
    Private m_vSourceArrayIncludeClosedBranch As Object
    'Var for Checking that whether Include Closed Branch Checkbox is Checked or Not
    Private m_bIsIncludeClosedBranchChecked As Boolean

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUFindRIParty.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    '' Variables to store the lookup values/details.
    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Declare an instance of the Lock object.
    Private m_oPMLock As Object

    Private m_sAgencyOrunderwriting As String = ""

    ' PartyType
    Private m_sPartyType As String = ""

    Private m_sUnderwritingType As String = ""
    Private m_bIncludeClosedBranches As Boolean

    Private m_bViewAuthority As Boolean '2005 Client manager Security
    Private m_bEditAuthority As Boolean '2005 Client manager Security
    Private m_bDeleteAuthority As Boolean '2005 Client Manager Security

    'QBENZ005
    Private m_oXa As XArrayHelper
    Private iRow As Integer
    Private m_iAction As Integer
    Private m_lRi_Arrangement_line_id As Integer
    Private m_vSearchData(,) As Object
    Private bIsParticipant100percent As Boolean
    Private vBrokerArray(,) As Object
    Private m_bIsFAX As Boolean
    Private m_lOriginalPartyCnt As Integer
    Private vParticipantArray(,) As Object
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_bChanged As Boolean
    Private m_sDeletedBrokerArray(,) As String
    Private m_bAddParticipantsFromTreaty As Boolean
    Private m_vTreatyPartiesBrokerParticipantForDisplay As Object

    Public Property IncludeClosedBranches() As Boolean
        Get
            Return m_bIncludeClosedBranches
        End Get
        Set(ByVal Value As Boolean)
            m_bIncludeClosedBranches = Value
        End Set
    End Property

    Public Property IsIncludeClosedBranchChecked() As Boolean
        Get
            Return m_bIsIncludeClosedBranchChecked
        End Get
        Set(ByVal Value As Boolean)
            m_bIsIncludeClosedBranchChecked = Value
        End Set
    End Property

    Public Property FileCode() As String
        Get
            Return m_sFileCode
        End Get
        Set(ByVal Value As String)
            m_sFileCode = Value
        End Set
    End Property

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.
            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)
    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.
            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.
            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.
            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.
            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public Property BrokerArray() As Object
        Get
            Return VB6.CopyArray(vParticipantArray)
        End Get
        Set(ByVal Value As Object)
            vParticipantArray = Value
        End Set
    End Property

    Public WriteOnly Property Action() As Integer
        Set(ByVal Value As Integer)
            m_iAction = Value
        End Set
    End Property

    Public WriteOnly Property RiArrangementLineID() As Integer
        Set(ByVal Value As Integer)
            m_lRi_Arrangement_line_id = Value
        End Set
    End Property

    Public WriteOnly Property IsFAX() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsFAX = Value
        End Set
    End Property

    Public Property ShortName() As String
        Get
            Return m_sShortName
        End Get
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property


    Public Property LongName() As String
        Get
            Return m_sLongName
        End Get
        Set(ByVal Value As String)
            m_sLongName = Value
        End Set
    End Property

    Public Property NotEditable() As Integer
        Get
            Return m_iNotEditable
        End Get
        Set(ByVal Value As Integer)
            m_iNotEditable = Value
        End Set
    End Property

    Public Property DeleteMode() As Boolean
        Get
            Return m_bDeleteMode
        End Get
        Set(ByVal Value As Boolean)
            m_bDeleteMode = Value
        End Set
    End Property

    Public WriteOnly Property SourceArray() As Object
        Set(ByVal Value As Object)
            ' Set the valid sources for the user
            m_vSourceArray = Value
        End Set
    End Property
    'JT PN-15885
    Public WriteOnly Property SourceArrayIncludeClosedBranch() As Object
        Set(ByVal Value As Object)
            ' Set the valid sources for the user
            m_vSourceArrayIncludeClosedBranch = Value
        End Set
    End Property

    '2005 Client Manager Security
    Public WriteOnly Property ViewAuthority() As Boolean
        Set(ByVal Value As Boolean)
            m_bViewAuthority = Value
        End Set
    End Property
    Public WriteOnly Property EditAuthority() As Boolean
        Set(ByVal Value As Boolean)
            m_bEditAuthority = Value
        End Set
    End Property
    Public WriteOnly Property DeleteAuthority() As Boolean
        Set(ByVal Value As Boolean)
            m_bDeleteAuthority = Value
        End Set
    End Property

    Public WriteOnly Property Task() As Integer
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property


    Public Property IsBrokerChanged() As Boolean
        Get
            Return m_bChanged
        End Get
        Set(ByVal Value As Boolean)
            m_bChanged = Value
        End Set
    End Property

    Public Property AddParticipantsFromTreaty() As Boolean
        Get
            Return m_bAddParticipantsFromTreaty
        End Get
        Set(ByVal Value As Boolean)
            m_bAddParticipantsFromTreaty = Value
        End Set
    End Property

    Public Property TreatyPartiesBrokerParticipantsForDisplay() As Object
        Get
            Return m_vTreatyPartiesBrokerParticipantForDisplay
        End Get
        Set(ByVal Value As Object)
            m_vTreatyPartiesBrokerParticipantForDisplay = Value
        End Set
    End Property

    ' PRIVATE Property Procedures (End)
    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    ' ***************************************************************** '

    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim iRow As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            g_oBusiness.PartyCnt = m_lOriginalPartyCnt

            If chkIncludeClosedBranches.CheckState = CheckState.Checked Then

                m_lReturn = g_oBusiness.FindNow(vName:=txtLongName.Text.Trim(), vShortName:=txtShortName.Text.Trim(), vFileCode:=txtFileCode.Text.Trim(), vValidBranches:=m_vSourceArrayIncludeClosedBranch, m_vSearchData:=m_vSearchData, m_lPartyCnt:=m_lOriginalPartyCnt, bIsFAx:=m_bIsFAX, bIsParticipant:=True)
            Else

                m_lReturn = g_oBusiness.FindNow(vName:=txtLongName.Text.Trim(), vShortName:=txtShortName.Text.Trim(), vFileCode:=txtFileCode.Text.Trim(), vValidBranches:=m_vSourceArray, m_vSearchData:=m_vSearchData, m_lPartyCnt:=m_lOriginalPartyCnt, bIsFAx:=m_bIsFAX, bIsParticipant:=True)
            End If

            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.
                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.
                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

            'get the Participation arrangement from Broker_participants table
            If m_iAction > 1 Then
                If m_lRi_Arrangement_line_id > 0 And Not m_bChanged Then 'Edit

                    Select Case m_sTransactionType
                        Case "NB", "MTA", "REN", "MTC", "", "MTR"

                            m_lReturn = g_oBusiness.GetBrokerParticipants(m_lRi_Arrangement_line_id:=m_lRi_Arrangement_line_id, m_iProcessId:=1, vArray:=vBrokerArray)
                        Case "C_CO", "C_CP", "C_CR", "C_CV"

                            m_lReturn = g_oBusiness.GetBrokerParticipants(m_lRi_Arrangement_line_id:=m_lRi_Arrangement_line_id, m_iProcessId:=2, vArray:=vBrokerArray)
                    End Select

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Not Information.IsArray(vBrokerArray) And m_lRi_Arrangement_line_id > 0 Then
                        MessageBox.Show("No Broker Participants attached with the selected RI Broker", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return result
                    ElseIf Information.IsArray(vBrokerArray) And m_oXa.GetUpperBound(0) < 0 Then
                        For lCount As Integer = 0 To vBrokerArray.GetUpperBound(1)
                            m_oXa.Rows.InsertAt(m_oXa.NewRow, m_oXa.Rows.Count - 1)
                            iRow = m_oXa.Rows.Count - 2
                            m_oXa(iRow, ACIBrokerShortName) = vBrokerArray(ACIBrokerShortName, iRow)
                            m_oXa(iRow, ACIBrokerLongName) = vBrokerArray(ACIBrokerLongName, iRow)
                            m_oXa(iRow, ACIBrokerParticipant_percent) = CDbl(vBrokerArray(ACIBrokerParticipant_percent, iRow)) / 100
                            m_oXa(iRow, ACIBrokerAssociationPartyCnt) = m_lOriginalPartyCnt
                            m_oXa(iRow, ACIBrokerPartyCnt) = vBrokerArray(3, iRow)
                        Next
                        grdParticipants.ReBind()
                        grdParticipants.Refresh()
                        RefreshTotalPercent()
                    End If
                End If


                If m_lRi_Arrangement_line_id = 0 Or m_bChanged Then

                    If Information.IsArray(vParticipantArray) And Not (vParticipantArray Is Nothing) Then
                        For iRow = 0 To vParticipantArray.GetUpperBound(0)

                            If Not Object.Equals(vParticipantArray(iRow, ACIBrokerPartyCnt), Nothing) Then
                                m_oXa.Rows.InsertAt(m_oXa.NewRow, m_oXa.Rows.Count - 1)
                                m_oXa(iRow, ACIBrokerShortName) = vParticipantArray(iRow, ACIBrokerShortName)
                                m_oXa(iRow, ACIBrokerLongName) = vParticipantArray(iRow, ACIBrokerLongName)
                                m_oXa(iRow, ACIBrokerParticipant_percent) = vParticipantArray(iRow, ACIBrokerParticipant_percent)
                                m_oXa(iRow, ACIBrokerAssociationPartyCnt) = vParticipantArray(iRow, ACIBrokerAssociationPartyCnt)
                                m_oXa(iRow, ACIBrokerPartyCnt) = vParticipantArray(iRow, ACIBrokerPartyCnt)
                            End If
                        Next
                        grdParticipants.ReBind()
                        grdParticipants.Refresh()
                        RefreshTotalPercent()
                    Else
                        If m_iTask <> gPMConstants.PMEComponentAction.PMEdit Then
                            MessageBox.Show("No Broker Participants attached with the selected RI Broker", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Return result
                        End If
                    End If
                End If

                CmdDelete.Enabled = m_oXa.Rows.Count > 1 '  m_oXa.GetUpperBound(0) > -1
            End If
            cmdFindNow.Enabled = False
            With grdParticipants
                .Rows(.Rows.Count - 1).DefaultCellStyle.BackColor = SystemColors.ButtonFace
                .Rows(.Rows.Count - 1).ReadOnly = True
                .Rows(.Rows.Count - 1).DefaultCellStyle.Font = New Font(.Font, FontStyle.Bold)
                .Rows(.Rows.Count - 1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End With
            Return result

        Catch excep As System.Exception

            ''Debugger.Break()


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the search details.
            lvwSearchDetails.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                ' Reset the number of items found message.
                DisplayStatusFound()
                Return result
            End If


            ' Assign the details to the interface.
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACIRI2007ShortName, lRow)).Trim())

                ' Assign details to other the columns
                ' Column 2 Long Name
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vSearchData(ACIRI2007LongName, lRow)).Trim()
                ' Column 3 Address Line 1
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACIRI2007Address1, lRow)).Trim()
                ' Column 4 Address Line 2
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vSearchData(ACIRI2007Address2, lRow)).Trim()
                ' Column 5 Post Code
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vSearchData(ACIRI2007PostalCode, lRow)).Trim()
                'Column 6 Column Type (Reinsurer)
                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = "Reinsurer"
                'Column 7 Source
                ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vSearchData(ACIRI2007SourceName, lRow)).Trim()

                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.
                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwSearchDetails.Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwSearchDetails.Refresh()
                End If
                'End If
            Next lRow

            ' Select the first item.
            If lvwSearchDetails.Items.Count > 0 Then
                lvwSearchDetails.Items.Item(0).Selected = True

                ' Alix - 07/11/2002
                m_lPartyCnt = CInt(m_vSearchData(ACIRI2007PartyCnt, Convert.ToString(lvwSearchDetails.SelectedItems(0).Tag)))

                ' Enable the interface now that the search has completed.
                m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' PWF 11/09/2002 - Moved from GetBusiness
            ' Display the number of items found message.
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception
            ''Debugger.Break()


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: LockParty
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (LockParty) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function LockParty() As Integer
    '
    'Dim result As Integer = 0
    'Dim sLockedBy As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_lReturn = m_oPMLock.LockKey(sKeyName:="party_cnt", vKeyValue:=m_lPartyCnt, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)
    '
    '
    'Select Case m_lReturn
    'Case gPMConstants.PMEReturnCode.PMTrue
    'OK
    '
    'Case gPMConstants.PMEReturnCode.PMFalse
    'Locked or error
    'If sLockedBy = "ERROR" Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return result
    'Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    'MessageBox.Show("Party currently locked by " & sLockedBy &  _
    '                Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Party Lock")
    'Return result
    'End If
    '
    '
    'Case Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the party", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    '
    'End Select
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: UnlockParty
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (UnlockParty) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UnlockParty() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_lReturn = m_oPMLock.UnLockKey(sKeyName:="party_cnt", vKeyValue:=m_lPartyCnt, iUserID:=g_oObjectManager.UserID)
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to process the interface.
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to unlock the party", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return result
    '
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function



    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    'History:   08/02/2007  Roopaly
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer


        Dim result As Integer = 0
        Dim iRow As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If cmbType.Items.Count = 0 Then
                cmbType.Items.Add("<ALL>")
                cmbType.Items.Add("Reinsurer")
                cmbType.SelectedIndex = 1
            End If

            'TDbG Default Setting
            m_oXa.RedimXArray(New Integer() {-1, 4}, New Integer() {0, 0})
            Dim iCountRow As Integer = 0
            If IsArray(m_vTreatyPartiesBrokerParticipantForDisplay) And m_bAddParticipantsFromTreaty = True Then
                For iRow = 0 To UBound(m_vTreatyPartiesBrokerParticipantForDisplay)
                    If ToSafeLong(m_vTreatyPartiesBrokerParticipantForDisplay(iRow, ACBPPartyCnt)) = m_lPartyCnt Then 'Party Cnt
                        ' Needs to check before filling based on treaty party id
                        m_oXa.AppendRows()
                        m_oXa(iCountRow, ACIBrokerShortName) = m_vTreatyPartiesBrokerParticipantForDisplay(iRow, ACBPShortCode) ' short name
                        m_oXa(iCountRow, ACIBrokerLongName) = m_vTreatyPartiesBrokerParticipantForDisplay(iRow, ACBPName) ' long name
                        m_oXa(iCountRow, ACIBrokerParticipant_percent) = m_vTreatyPartiesBrokerParticipantForDisplay(iRow, ACBPParticipantPercent)
                        m_oXa(iCountRow, ACIBrokerAssociationPartyCnt) = m_vTreatyPartiesBrokerParticipantForDisplay(iRow, ACBPPartyCnt) ' party cnt
                        m_oXa(iCountRow, ACIBrokerPartyCnt) = m_vTreatyPartiesBrokerParticipantForDisplay(iRow, ACBPPassociatedPartyCnt) 'assocation party cnt
                        iCountRow = iCountRow + 1
                    End If
                Next
            End If

            m_oXa.Rows.Add(m_oXa.NewRow)

            With grdParticipants
                Dim bindingSource As BindingSource = New BindingSource(m_oXa, "")
                .DataSource = bindingSource
                .ReBind()
                .Refresh()

                .Columns(0).HeaderText = "Reinsurer Code :"
                .Columns(0).ReadOnly = True
                .Columns(0).Width = 100
                .Columns(1).HeaderText = "Name"
                .Columns(1).ReadOnly = True
                .Columns(1).Width = 300
                .Columns(2).HeaderText = "Participation%"
                .Columns(2).ReadOnly = False
                .Columns(2).DefaultCellStyle.BackColor = Color.White
                .Columns(2).Width = 100
                .Columns(3).Visible = False
                .Columns(4).Visible = False
                .Rows(.Rows.Count - 1).DefaultCellStyle.BackColor = SystemColors.ButtonFace
                .Rows(.Rows.Count - 1).ReadOnly = True
                .Rows(.Rows.Count - 1).DefaultCellStyle.Font = New Font(.Font, FontStyle.Bold)
                .Rows(.Rows.Count - 1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                m_oXa.Rows(m_oXa.Rows.Count - 1)(1) = "Total:"


                For Each dc As DataGridViewColumn In .Columns
                    dc.SortMode = DataGridViewColumnSortMode.NotSortable
                Next
            End With



            chkIncludeClosedBranches.CheckState = CheckState.Unchecked
            chkIncludeClosedBranches.Visible = m_bIncludeClosedBranches

            lvwSearchDetails.Columns.Insert(0, "", "Reinsurer Code", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)
            lvwSearchDetails.Columns.Insert(1, "", "Name", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)
            lvwSearchDetails.Columns.Insert(2, "", "Address Line 1", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)
            lvwSearchDetails.Columns.Insert(3, "", "Address Line 2", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)
            lvwSearchDetails.Columns.Insert(4, "", "Postcode", CInt(VB6.TwipsToPixelsX(1300)), HorizontalAlignment.Left, -1)
            lvwSearchDetails.Columns.Insert(5, "", "Type", CInt(VB6.TwipsToPixelsX(1300)), HorizontalAlignment.Left, -1)
            lvwSearchDetails.Columns.Insert(6, "", "Source", CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)


            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchDetails.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            RefreshTotalPercent()
            Return result

        Catch excep As System.Exception

            ''Debugger.Break()


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' ***************************************************************** '
    Private Function ClearInterface(ByRef bConfirm As Boolean) As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the interface details.

            If bConfirm Then

                ' Check if the user still wishes to clear
                ' the interface.


                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display the message.
                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Don't continue with the clear.
                    Return result
                End If

            End If


            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwSearchDetails.Items.Clear()

            ' Clear the search status bar.
            _stbStatus_Panel1.Text = ""

            ' {* USER DEFINED CODE (Begin) *}

            'RKS 141004 PN13238 & PN14838
            chkIncludeClosedBranches.CheckState = CheckState.Unchecked
            txtShortName.Text = ""
            txtLongName.Text = ""
            txtFileCode.Text = ""

            ' Set focus to the search details.
            txtShortName.Focus()

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

            ' {* USER DEFINED CODE (End) *}

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception

            ''Debugger.Break()


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '' ********************************************************************************* '
    '' Name: Private Function                                                            '
    ''                                                                                   '
    '' Description: Checks that the transaction is for one of the branches being paid    '
    ''                                                                                   '
    '' ********************************************************************************* '
    'Private Function ValidSource(ByVal vSource As Variant) As Boolean
    'Dim i As Integer
    '    ValidSource = False
    '    If UBound(m_vSearchData, 2) = 0 And Trim$(m_vSearchData(2, 0)) = Trim$(txtShortName.Text) Then
    '        ValidSource = True
    '        Exit Function
    '    End If
    '    If IsArray(m_vSourceArray) = False Then
    '        ValidSource = True
    '        Exit Function
    '    End If
    '    For i = 1 To UBound(m_vSourceArray, 2)
    '        If CLng(m_vSourceArray(1, i)) = CLng(vSource) Then
    '            ValidSource = True
    '        End If
    '    Next i
    'End Function


    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOK.Enabled = Not bDisable

            Return result

        Catch excep As System.Exception

            ''Debugger.Break()


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & sMessage

        Catch excep As System.Exception
            ''Debugger.Break()



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()

        Static sMessage As String = ""
        Dim lItemsFound As Integer

        Try

            ' Store the total of item found.
            lItemsFound = lvwSearchDetails.Items.Count
            '
            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception
            ''Debugger.Break()



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check all fields for data.
            ' At least one field must be populated

            If txtShortName.Text.Trim() <> "" Then
                If txtShortName.Text.Trim().Length >= ACMinSearchLength Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtLongName.Text.Trim() <> "" Then
                If txtLongName.Text.Trim().Length >= ACMinSearchLength Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            If txtFileCode.Text.Trim() <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cmbType.Text.ToUpper() <> "<ALL>" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If
            Return result

        Catch excep As System.Exception
            ''Debugger.Break()



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub CmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdDelete.Click

        Dim lSelectedPartyCnt As Integer
        'if the last row is selected
        If grdParticipants.CurrentRow.Index = grdParticipants.RowCount - 1 Then
            Exit Sub
        Else
            lSelectedPartyCnt = grdParticipants.CurrentRow().Cells(4).Value
        End If

        For lCount As Integer = m_oXa.Rows.Count - 2 To 0 Step -1
            'Developer Guide no. 188 (Latest Guide)
            If m_oXa(lCount, ACIBrokerPartyCnt) = lSelectedPartyCnt Then
                m_oXa.Rows(lCount).Delete()
                m_oXa.AcceptChanges()
                Exit For
            End If
        Next

        grdParticipants.ReBind()
        grdParticipants.Refresh()
        RefreshTotalPercent()

        CmdDelete.Enabled = m_oXa.Rows.Count > 1

        'PN 63157 - need to prepare array to keep all broker participants
        Dim iArrayIndex As Integer = m_sDeletedBrokerArray.GetUpperBound(1)
        ReDim Preserve m_sDeletedBrokerArray(1, iArrayIndex + 1)
        m_sDeletedBrokerArray(0, iArrayIndex) = CStr(m_lRi_Arrangement_line_id)
        m_sDeletedBrokerArray(1, iArrayIndex) = CStr(lSelectedPartyCnt)
        With grdParticipants
            .Rows(.Rows.Count - 1).DefaultCellStyle.BackColor = SystemColors.ButtonFace
            .Rows(.Rows.Count - 1).ReadOnly = True
            .Rows(.Rows.Count - 1).DefaultCellStyle.Font = New Font(.Font, FontStyle.Bold)
            .Rows(.Rows.Count - 1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
        End With
    End Sub

    ' PRIVATE Methods (End)

    Private Sub cmdSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelect.Click
        Dim iRow As Integer
        Dim bExists As Boolean

        If lvwSearchDetails.Items.Count < 1 Then Exit Sub

        If lvwSearchDetails.Items.Count > 0 Then
            If lvwSearchDetails.FocusedItem Is Nothing Then
                lvwSearchDetails.Items(0).Selected = True
                lvwSearchDetails.Items(0).Focused = True
            End If
        End If
        Dim rowlv As Integer = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
        If m_oXa.Rows.Count > 1 Then
            For lCount As Integer = 0 To m_oXa.Rows.Count - 2
                'Developer Guide No.188
                If m_oXa(lCount, 0).Trim() = CStr(m_vSearchData(ACIRI2007ShortName, rowlv)).Trim() Then
                    bExists = True
                    Exit For
                End If
            Next
        End If
        If Not bExists Then

            m_oXa.Rows.InsertAt(m_oXa.NewRow, m_oXa.Rows.Count - 1)
            iRow = m_oXa.Rows.Count - 2

            'TODO
            m_oXa(iRow, ACIBrokerShortName) = CStr(m_vSearchData(ACIRI2007ShortName, rowlv)).Trim()
            m_oXa(iRow, ACIBrokerLongName) = CStr(m_vSearchData(ACIRI2007LongName, rowlv)).Trim()
            m_oXa(iRow, ACIBrokerParticipant_percent) = 0
            m_oXa(iRow, ACIBrokerAssociationPartyCnt) = m_lOriginalPartyCnt
            m_oXa(iRow, ACIBrokerPartyCnt) = m_vSearchData(ACIRI2007PartyCnt, rowlv)
        Else
            MessageBox.Show("Reinsurer is already on the list.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        grdParticipants.ReBind()
        grdParticipants.Refresh()
        RefreshTotalPercent()

        CmdDelete.Enabled = m_oXa.Rows.Count > 1
        With grdParticipants
            .Rows(.Rows.Count - 1).DefaultCellStyle.BackColor = SystemColors.ButtonFace
            .Rows(.Rows.Count - 1).ReadOnly = True
            .Rows(.Rows.Count - 1).DefaultCellStyle.Font = New Font(.Font, FontStyle.Bold)
            .Rows(.Rows.Count - 1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
        End With
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sValue As String = ""

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUFindRIParty.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Get bPMLock
            Dim temp_m_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMLock = temp_m_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                'Initialise = PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
            ReDim Preserve m_sDeletedBrokerArray(1, 0) 'PN 63157
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            m_oXa = New XArrayHelper()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception
            ''Debugger.Break()


            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmRIBrokerParticipants_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            m_sAgencyOrunderwriting = g_oBusiness.UnderwritingOrAgency


            m_sUnderwritingType = g_oBusiness.UnderwritingType

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' {* USER DEFINED CODE (Begin) *}

            If CheckMandatory() <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Inadequate data so cannot
                ' continue with the search.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' {* USER DEFINED CODE (End) *}

            '    If m_iAction = 2 Then 'edit
            m_lOriginalPartyCnt = m_lPartyCnt
            '    End If

            m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.
                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.
                Case Else
                    ' Failed to get details.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set properties", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

            End Select

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception
            ''Debugger.Break()



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private Sub frmRIBrokerParticipants_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'Developer Guide No. 7
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'Developer Guide No. 7
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            'Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Destroy the instance of the lock object
            ' from memory.
            If Not (m_oPMLock Is Nothing) Then

                m_oPMLock = Nothing
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception

            ''Debugger.Break()



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmRIBrokerParticipants_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'm_lReturn& = ResizeInterface()
        '
        'Catch 
        '
        '
        '
        '
        'Exit Sub
        'End Try


    End Sub

    Dim OldValue As Object

    Private Sub grdParticipants_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles grdParticipants.CellBeginEdit
        OldValue = grdParticipants.CurrentRow.Cells(e.ColumnIndex).Value
    End Sub
    Private Sub grdParticipants_CellEndEdit(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles grdParticipants.CellEndEdit
        Dim ColIndex As Integer = eventArgs.ColumnIndex

        Dim Cancel As Integer = 0

        Dim NewValue As Object = ToSafeDouble(Convert.ToString(grdParticipants.Rows(eventArgs.RowIndex).Cells(eventArgs.ColumnIndex).Value).Replace("%", "").Trim)
        If OldValue Is DBNull.Value Or NewValue Is DBNull.Value Then
            Exit Sub
        End If
        If (OldValue = NewValue) Or OldValue Is DBNull.Value Or NewValue Is DBNull.Value Then
            Exit Sub
        End If

        If ColIndex = 2 Then
            'If CStr(grdParticipants.CurrentRow().Cells(2).Value).IndexOf("%"c) >= 0 Then
            '    grdParticipants.CurrentRow().Cells(2).Value = (Conversion.Val(CStr(grdParticipants.CurrentRow().Cells(2).Value).Replace("%", "")) / 100)
            'Else
            '    grdParticipants.CurrentRow().Cells(2).Value = (gPMFunctions.ToSafeDouble(grdParticipants.CurrentRow().Cells(2).Value) / 100).ToString("P4")
            'End If
            grdParticipants.CurrentRow().Cells(2).Value = ToSafeDouble(Convert.ToString(grdParticipants.CurrentRow().Cells(2).Value).Replace("%", "").Trim) / 100

        End If
        If Cancel <> 0 Then
            grdParticipants.CancelEdit()
        End If




        If ColIndex = 2 Then
            RefreshTotalPercent()
        End If

        If gPMFunctions.ToSafeDouble(grdParticipants.Rows(grdParticipants.Rows.Count - 1).Cells(2).Value.ToString.Trim) > 100 Then
            MessageBox.Show("Participation percentage cannot be greater than 100.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
            m_bChanged = True
        End If
    End Sub
    Private Sub lvwSearchDetails_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        Dim iRow As Integer

        If lvwSearchDetails.Items.Count > 0 Then
            If (eventArgs.KeyCode = Keys.Up) Or eventArgs.KeyCode = Keys.Down Then

                iRow = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
                txtShortName.Text = CStr(m_vSearchData(ACIRI2007ShortName, iRow)).Trim()
                txtLongName.Text = CStr(m_vSearchData(ACIRI2007LongName, iRow)).Trim()
                txtFileCode.Text = CStr(m_vSearchData(ACIRI2007FileCode, iRow)).Trim()
            End If
        End If
    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click


        ' Click event of the OK button.

        Try

            'Check Participation %

            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                RefreshTotalPercent()

                If Not bIsParticipant100percent Then
                    MessageBox.Show("Total Participant percentage should be equal to 100%.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                ReDim vParticipantArray(grdParticipants.RowsCount - 2, 4)
                For iRow As Integer = 0 To grdParticipants.RowsCount - 2

                    'TODO
                    'grdParticipants.Bookmark = iRow

                    vParticipantArray(iRow, ACIBrokerShortName) = grdParticipants.Rows(iRow).Cells(ACIBrokerShortName).Value

                    vParticipantArray(iRow, ACIBrokerLongName) = grdParticipants.Rows(iRow).Cells(ACIBrokerLongName).Value

                    vParticipantArray(iRow, ACIBrokerParticipant_percent) = grdParticipants.Rows(iRow).Cells(ACIBrokerParticipant_percent).Value

                    vParticipantArray(iRow, ACIBrokerAssociationPartyCnt) = grdParticipants.Rows(iRow).Cells(ACIBrokerAssociationPartyCnt).Value

                    vParticipantArray(iRow, ACIBrokerPartyCnt) = grdParticipants.Rows(iRow).Cells(ACIBrokerPartyCnt).Value
                Next
                'PN 63157
                For iRow As Integer = 0 To m_sDeletedBrokerArray.GetUpperBound(1) - 1
                    Select Case m_sTransactionType
                        Case "NB", "MTA", "REN", "MTC", "MTR"

                            m_lReturn = g_oBusiness.DeleteBrokerParticipants(m_lRi_Arrangement_line_id:=m_sDeletedBrokerArray(0, iRow), m_iProcessId:=1, m_lPartyCnt:=m_sDeletedBrokerArray(1, iRow))
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("g_oBusiness.DeleteBrokerParticipants", "Unable to Delete Broker Participants")
                            End If
                        Case "C_CO", "C_CP", "C_CR", "C_CV"

                            m_lReturn = g_oBusiness.DeleteBrokerParticipants(m_lRi_Arrangement_line_id:=m_sDeletedBrokerArray(0, iRow), m_iProcessId:=2, m_lPartyCnt:=m_sDeletedBrokerArray(1, iRow))
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("g_oBusiness.DeleteBrokerParticipants", "Unable to Delete Broker Participants")
                            End If
                    End Select
                Next
            End If

            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            'Developer Guide No. 231
            Me.Hide()

        Catch excep As System.Exception

            ''Debugger.Break()


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            'Developer Guide No. 231
            Me.Hide()
            ' Process the next set of actions.
            'm_lReturn& = m_oGeneral.ProcessCommand()

            ' Check the return value.
            '    If (m_lReturn& = PMTrue) Then
            '
            '        m_lPartyCnt = 0     'PN20059
            '
            '        ' Everything OK, so we can hide the interface.
            '        Me.Hide
            '    End If

        Catch excep As System.Exception
            ''Debugger.Break()



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click

        ' Click event of the Cancel button.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get the interface details from the business object.
            m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
            End If

            ' Assign the details from the search data storage
            ' to the interface.
            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the details.
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            If lvwSearchDetails.Items.Count > 0 Then
                VB6.SetDefault(cmdFindNow, False)
                VB6.SetDefault(cmdOK, False)
            End If

            ' Set the focus.
            lvwSearchDetails.Focus()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception
            ''Debugger.Break()



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try

            'm_lPartyCnt = 0
            ' Clear the interface details.
            m_lReturn = CType(ClearInterface(bConfirm:=True), gPMConstants.PMEReturnCode)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

        Catch excep As System.Exception

            ''Debugger.Break()


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Enter

        ' GotFocus Event for the search details

        Try

            ' Unset any default buttons so can
            VB6.SetDefault(cmdFindNow, False)
            VB6.SetDefault(cmdOK, False)

        Catch excep As System.Exception

            ''Debugger.Break()


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Leave

        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception

            ''Debugger.Break()


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Click

        Dim sShortName As String = ""
        Dim iCount, iRow, n As Integer

        If lvwSearchDetails.Items.Count > 0 Then
            sShortName = lvwSearchDetails.FocusedItem.Text

            iRow = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
            m_lPartyCnt = CInt(m_vSearchData(ACIRI2007PartyCnt, iRow))
            txtShortName.Text = CStr(m_vSearchData(ACIRI2007ShortName, iRow)).Trim()
            txtLongName.Text = CStr(m_vSearchData(ACIRI2007LongName, iRow)).Trim()
            txtFileCode.Text = CStr(m_vSearchData(ACIRI2007FileCode, iRow)).Trim()
            VB6.SetDefault(cmdOK, True)
        End If

    End Sub

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick

        ' Double click event for the search details.

        Try
            If (lvwSearchDetails.Items.Count > 0) And cmdSelect.Enabled Then
                cmdSelect_Click(cmdSelect, New EventArgs())
            End If

        Catch excep As System.Exception

            ''Debugger.Break()


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetails_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSearchDetails.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If KeyCode <> 13 Then
            VB6.SetDefault(cmdOK, False)
        End If
    End Sub

    Private Sub lvwSearchDetails_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lvwSearchDetails.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        Dim sShortName As String = ""
        Dim iCount, iRow As Integer

        If KeyAscii = 13 Then
            If lvwSearchDetails.Items.Count > 0 Then
                sShortName = lvwSearchDetails.FocusedItem.Text

                iRow = Convert.ToString(lvwSearchDetails.FocusedItem.Tag)
                txtShortName.Text = CStr(m_vSearchData(ACIRI2007ShortName, iRow)).Trim()
                txtLongName.Text = CStr(m_vSearchData(ACIRI2007LongName, iRow)).Trim()
                VB6.SetDefault(cmdOK, True)

            End If
        End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)

        ' Column click event for the search details

        Try

            With lvwSearchDetails
                ' If current sort column header is
                ' pressed.
                If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetails) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwSearchDetails, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetails, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwSearchDetails, True)
                End If
            End With

        Catch excep As System.Exception

            ''Debugger.Break()


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub txtFileCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFileCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
        End If
    End Sub

    Private Sub txtFileCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFileCode.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtFileCode)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub


    Private Sub txtShortName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortName.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtShortName)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtShortName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShortName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
        End If
    End Sub

    Private Sub txtLongName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLongName.Enter

        ' Highlight any text.
        iPMFunc.SelectText(txtLongName)

        ' Change the default button.
        VB6.SetDefault(cmdFindNow, True)

    End Sub

    Private Sub txtLongName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLongName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            cmdFindNow.Enabled = (CheckMandatory() = gPMConstants.PMEReturnCode.PMTrue)
        End If
    End Sub
    'QBENZ005
    Private Sub RefreshTotalPercent()

        Try
            Dim lSelectedRow As Integer
            'TODO
            Dim dTotalPercent As Double
            dTotalPercent = 0
            For lCount As Integer = 0 To grdParticipants.RowsCount - 2

                'TODO
                dTotalPercent += CDbl(StringsHelper.Format(gPMFunctions.ToSafeDouble(grdParticipants.Rows(lCount).Cells(2).Value.ToString.Replace("%", "")), "00.######"))
            Next

            grdParticipants.Rows(grdParticipants.Rows.Count - 1).Cells(2).Value = (dTotalPercent / 100).ToString("P4")

            If CInt(dTotalPercent.ToString("P4").Replace("%", "").Trim) = 100 Then
                bIsParticipant100percent = True
            Else
                bIsParticipant100percent = False
            End If

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Refresh Total participation Percentage ", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshTotalPercent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' ***************************************************************** '
    Public Function PropertiesToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' {* USER DEFINED CODE (Begin) *}

            txtShortName.Text = m_sShortName.Trim()
            txtLongName.Text = m_sLongName.Trim()
            txtFileCode.Text = m_sFileCode.Trim()


            If m_iTask = gPMConstants.PMEComponentAction.PMView Or m_iAction = 3 Then
                cmdFindNow.Enabled = False
                cmdNewSearch.Enabled = False
                cmdSelect.Enabled = False
                CmdDelete.Enabled = False
                For iCol As Integer = 0 To grdParticipants.Columns.Count - 1
                    grdParticipants.Columns(iCol).ReadOnly = True
                Next
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmRIBrokerParticipants_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub

    Private Sub grdParticipants_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles grdParticipants.CellFormatting
        Dim dTempVal As Double = 0D
        Select Case e.ColumnIndex
            Case 2
                If (Double.TryParse(e.Value.ToString.Replace("%", ""), dTempVal)) Then
                    'If e.RowIndex = grdParticipants.Rows.Count - 1 Then
                    '    Exit Sub
                    'End If
                End If
                e.Value = dTempVal.ToString("P4")
        End Select
    End Sub
End Class