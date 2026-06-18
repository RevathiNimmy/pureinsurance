Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Linq
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Partial Friend Class frmInterface

    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 28/08/1998
    '
    ' Description: Main interface.
    '
    ' Edit History: SD 05/03/2003 Migrating bug fixes for 1.8.6 ISSUE 2709
    '               KN (CMG) 04/02/03 Issue 1941 - Multiple Sub branches
    '               causing Period End to crash
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    'developer guide no.7
    Public Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date


    ' {* USER DEFINED CODE (Begin) *}
    Private m_oACTLedger As Object ' bACTLedger
    Private m_oACTPeriod As Object ' bACTPeriod

    Private m_lLastPeriodID As Integer

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTPeriodEnd.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object ' bACTPeriodEnd

    ' Instance of the report object
    Private m_oReportPrint As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private Const ACT_ADVANCE_BUTTON As String = "Advance"
    Private Const ACT_RETREAT_BUTTON As String = "Retreat"
    Private Const ACT_MAX_LEDGERS As Integer = 3
    'eck310701
    Private Const ACT_MIN_LEDGERS As Integer = 0
    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Where we are in the sequence of closing ledgers
    Private m_iCurrentSequence As Integer

    'DD 27/08/2002: Added for Multi-Tree product option
    Private m_bMultiTree As Boolean
    ' {* USER DEFINED CODE (Begin) *}

    ' Store the report data
    Private m_vReportArray(,) As Object
    Private m_iBatchProcessId As Integer
    Private m_sbatchStatus As String
    Private m_sBatchProcessName As String
    Private m_sbatchContentDetails As String
    Dim m_dtParameters As DataTable = Nothing
    Private m_ibatchSchedulerId As Integer
    Private m_bAttachToScheduler As Boolean
    Private m_ledgerPeriod As String
    Private m_oBatchParameters(,) As Object

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


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


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
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

    Public Property BatchProcessId() As Integer
        Get
            Return m_iBatchProcessId
        End Get
        Set(ByVal Value As Integer)
            m_iBatchProcessId = Value
        End Set
    End Property
    Public Property BatchProcessName() As String
        Get
            Return m_sBatchProcessName
        End Get
        Set(ByVal Value As String)
            m_sBatchProcessName = Value
        End Set
    End Property
    Public Property ProcessParameters() As DataTable
        Get
            Return m_dtParameters
        End Get
        Set(ByVal Value As DataTable)
            m_dtParameters = Value
        End Set
    End Property
    Public Property BatchFileContentDetails() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_sbatchContentDetails
        End Get
        Set(value As String)
            m_sbatchContentDetails = value
        End Set
    End Property
    Public Property BatchSchedulerId() As Integer
        Get
            Return m_ibatchSchedulerId
        End Get
        Set(ByVal Value As Integer)
            m_ibatchSchedulerId = Value
        End Set
    End Property
    Public Property AttachToScheduler() As Boolean
        Get
            Return m_bAttachToScheduler
        End Get
        Set(ByVal Value As Boolean)
            m_bAttachToScheduler = Value
        End Set
    End Property

    Public Property BatchParameters() As Object(,)
        Get
            Return m_oBatchParameters
        End Get
        Set(ByVal Value As Object(,))
            m_oBatchParameters = Value
        End Set
    End Property

    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim lSubBranchID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            ' DD 02/08/2002 : Multi-Branch
            ' Only retrieve data when the Sub-Branch is chosen
            If cboSubBranch.SelectedIndex >= 0 Or Not m_bMultiTree Then
                'DD 27/08/2002: Added product option
                If m_bMultiTree Then
                    lSubBranchID = VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex)
                Else
                    'Hard-coded for speed
                    lSubBranchID = 1
                End If


                m_lReturn = m_oACTLedger.GetDetails(vSubBranchID:=lSubBranchID)

                ' {* USER DEFINED CODE (End) *}

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = CType(BusinessToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    txtDesc.Text = FormatField( _
            ''        iFormatType:=PMFormatString, _
            ''        vFieldValue:=m_sDDesc$)
            '
            '    optChoice.Value = CBool(FormatField( _
            ''        iFormatType:=PMFormatBoolean, _
            ''        vFieldValue:=m_iDChoice%))
            '
            '    txtDate.Text = FormatField( _
            ''        iFormatType:=PMFormatDateLong, _
            ''        vFieldValue:=m_dtDDate)
            '
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lBusinessDataID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}
                    'm_lReturn& = m_oBusiness.EditAdd(lRow:=lBusinessDataID&, )
                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}
                    'm_lReturn& = m_oBusiness.EditUpdate(lRow:=lBusinessDataID&, )
                    ' {* USER DEFINED CODE (End) *}
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retreive all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            'm_lReturn& = m_oBusiness.GetNext()
            If AttachToScheduler Then
                If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                    cmdSchedule.Text = "View &Schedule"
                    cmdCancel.Location = New System.Drawing.Point(440, 288)
                    Me.HelpButton = False

                End If
                If m_iTask = gPMConstants.PMEComponentAction.PMEdit Or m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                    cmdCancel.Location = New System.Drawing.Point(440, 288)
                    Me.HelpButton = False
                End If

            End If
            '    Select Case m_iTask
            '        Case gPMConstants.PMEComponentAction.PMEdit
            '            m_lReturn = LoadBatchParameters()
            '            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '                gPMFunctions.RaiseError("LoadBatchParameters", "Unable to load parameters for batch schedular")
            '            End If
            '        Case gPMConstants.PMEComponentAction.PMView
            '            cmdSchedule.Text = "View Schedule"

            '            m_lReturn = LoadBatchParameters()
            '            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '                gPMFunctions.RaiseError("LoadBatchParameters", "Unable to load parameters for batch schedular")
            '            End If

            '        Case gPMConstants.PMEComponentAction.PMAdd
            '            m_lReturn = CType(StoreLedgerTitles(), gPMConstants.PMEReturnCode)

            '    End Select
            'Else
            m_lReturn = CType(StoreLedgerTitles(), gPMConstants.PMEReturnCode)
            'End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try


            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim vValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            'm_lReturn& = DisplayCaptions()

            m_lReturn = gPMConstants.PMEReturnCode.PMTrue
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    'cmdNavigate.Visible = True
                    'cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    'cmdNavigate.Visible = True
                    'cmdNavigate.Enabled = False

                Case Else
                    'cmdNavigate.Visible = False
            End Select

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            'DD 27/08/2002: Get product option setting
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, g_iSourceID, vValue)
            m_bMultiTree = (gPMFunctions.NullToString(vValue) = "1")

            If m_bMultiTree Then
                ' DD 02/08/2002: Added population of Sub-branch Combo

                m_lReturn = m_oACTPeriod.GetSubBranches(vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Display message.
                    MessageBox.Show("Failed to get Sub-Branches", "Sub-Branches load", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vResultArray) Then

                    For lRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                        Dim cboSubBranch_NewIndex As Integer = -1

                        cboSubBranch_NewIndex = cboSubBranch.Items.Add(CStr(vResultArray(3, lRow)))

                        VB6.SetItemData(cboSubBranch, cboSubBranch_NewIndex, CInt(vResultArray(0, lRow)))
                    Next lRow

                    'If there is only one sub-branch then choose it and disable the control

                    If vResultArray.GetUpperBound(1) = 0 Then
                        cboSubBranch.SelectedIndex = 0
                        cboSubBranch.Enabled = False
                        'SD 05/03/2003 Migrating bug fixes for 1.8.6 ISSUE 2709
                        'KN (CMG) 04/02/03
                    Else

                        cboSubBranch.SelectedIndex = vResultArray.GetLowerBound(1)
                        cboSubBranch.Enabled = True
                    End If
                End If
            Else
                'Hide Sub branch neatly
                lblSubBranch.Visible = False
                cboSubBranch.Visible = False

                lblCurrentPeriodIn.Top -= (lblCurrentPeriodIn.Top - lblSubBranch.Top) / 2
                pnlCurrentYear.Top = lblCurrentPeriodIn.Top
            End If
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            If AttachToScheduler Then
                cmdSchedule.Visible = True
                _cmdAdvance_1.Visible = False
                _cmdAdvance_2.Visible = False
                _cmdAdvance_3.Visible = False
                cmdPeriodEnd.Visible = False
                cmdOK.Visible = False
                cmdApply.Visible = False
                cmdHelp.Visible = False
                lblNote.Visible = False
            Else
                cmdSchedule.Visible = False
                _cmdAdvance_1.Visible = True
                _cmdAdvance_2.Visible = True
                _cmdAdvance_3.Visible = True
                cmdPeriodEnd.Visible = True
                cmdOK.Visible = True
                cmdApply.Visible = True
                cmdHelp.Visible = True
                lblNote.Visible = True
            End If


            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            'ReDim m_ctlTabFirstLast(1, )

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayCaptions) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DisplayCaptions() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Display all language specific captions.
    '
    '    Me.Caption = iPMFunc.GetResData( _
    ''        iLangID:=g_iLanguageID%, _
    ''        lID:=ACInterfaceTitle, _
    ''        iDataType:=PMResString)
    '
    ' Check for an error.
    'If Me.Text = "" Then
    ' Failed to get data from the resource file.
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &  _
    '                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")
    '
    'Return result
    'End If
    '

    'cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '

    'cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '
    'cmdNavigate.Caption = iPMFunc.GetResData( _
    ''iLangID:=g_iLanguageID%, _
    ''lID:=ACNavigateButton, _
    ''iDataType:=PMResString)
    '

    'SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    ' ************************************************************
    ' Enter your code here to display all language specific
    ' captions.
    ' The GetResData function will allow you to do this.
    ''
    ' Example:-
    ''
    '    lblDesc.Caption = iPMFunc.GetResData( _
    ''        iLangID:=g_iLanguageID%, _
    ''        lID:=ACDesc, _
    ''        iDataType:=PMResString)
    ''
    ' NOTE: Replace this section with your new code.
    ' ************************************************************
    '
    ' {* USER DEFINED CODE (End) *}
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.
                    '            m_lReturn& = m_oBusiness.GetLookupValues( _
                    ''                iLookupType:=PMLookupAll, _
                    ''                vTableArray:=m_vLookupValues, _
                    ''                iLanguageID:=g_iLanguageID%, _
                    ''                vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.
                    '            m_lReturn& = m_oBusiness.GetLookupValues( _
                    ''                iLookupType:=PMLookupAllEffective, _
                    ''                vTableArray:=m_vLookupValues, _
                    ''                iLanguageID:=g_iLanguageID%, _
                    ''                vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.
                    '            m_lReturn& = m_oBusiness.GetLookupValues( _
                    ''                iLookupType:=PMLookupSingle, _
                    ''                vTableArray:=m_vLookupValues, _
                    ''                iLanguageID:=g_iLanguageID%, _
                    ''                vResultArray:=m_vLookupDetails)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow As Integer
    'Dim bFoundMatch As Boolean
    '
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'bFoundMatch = False
    '
    'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
    ' Check for a match of the table name.
    'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
    ' Found a match
    'bFoundMatch = True
    'Exit For
    'End If
    'Next lRow
    '
    ' Check if there has been a table match.
    'If Not bFoundMatch Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
    '
    'Return result
    'End If
    '
    ' Using the lookup values, populate the control with
    ' the details from the lookup details array.
    '
    'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
    ' Add the details to the control.

    'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))


    'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
    '
    ' Check if this is the selected index.
    'If m_vLookupValues(ACValueID, lRow).Equals(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    'Next lCntr
    '
    ' Check if the selected index is blank. If so,
    ' we set the controls index to zero.
    'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

    'ctlLookup.ListIndex = 0
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: FindEarliestDate
    '
    ' Description:
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (FindEarliestDate) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function FindEarliestDate(ByRef v_vPeriodEndDates() As Object, ByRef r_iEarliest As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'r_iEarliest = 1
    '



    'If v_vPeriodEndDates(2) < v_vPeriodEndDates(1) Then
    'r_iEarliest = 1
    'End If
    '
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindEarliestDateFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindEarliestDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    ' ***************************************************************** '
    ' Name: CheckPeriodsMatch
    '
    ' Description: Checks that the periods match
    '
    ' ***************************************************************** '
    Private Function CheckPeriodsMatch(ByRef v_vLedgerPeriods() As Object) As Integer

        Dim result As Integer = 0
        Dim bMatch As Boolean
        Dim vPeriodEndDates As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bMatch = True

            ' Check to see that they're all the same...


            'developer guide no.162
            'start
            If Not v_vLedgerPeriods(0).Equals(v_vLedgerPeriods(1)) Then
                bMatch = False
            End If


            If Not v_vLedgerPeriods(0).Equals(v_vLedgerPeriods(2)) Then
                bMatch = False
            End If


            If Not v_vLedgerPeriods(1).Equals(v_vLedgerPeriods(2)) Then
                bMatch = False
            End If
            'end
            ' If they are, then exit
            If bMatch Then
                Return result
            End If

            ' Different, so we need to find out what the dates are

            m_lReturn = m_oBusiness.GetPeriodEndDates(v_vPeriodIDs:=v_vLedgerPeriods, r_vPeriodEndDates:=vPeriodEndDates)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' loop thru ledgers
            For iLoop1 As Integer = 1 To ACT_MAX_LEDGERS




                'developer guide no.162
                If CDbl(v_vLedgerPeriods(iLoop1 - 1)) = CInt(vPeriodEndDates(vPeriodEndDates.GetLowerBound(0), 0)) Then
                    cmdAdvance(iLoop1).Text = ACT_ADVANCE_BUTTON
                Else
                    cmdAdvance(iLoop1).Text = ACT_RETREAT_BUTTON
                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckPeriodsMatchFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPeriodsMatch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCurrentSequence
    '
    ' Description: Runs through the command buttons and finds which the
    '              most recently pressed button was.
    '
    ' ***************************************************************** '
    Private Function GetCurrentSequence() As Integer

        Dim result As Integer = 0
        Dim iTempPos As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set to an impossible value to start with
            iTempPos = 4

            ' Now loop through each RETREAT button and check its sequence
            ' number.
            For iLoop1 As Integer = 1 To ACT_MAX_LEDGERS
                'eck200700
                '        If (cmdAdvance(iLoop1%).Caption = ACT_RETREAT_BUTTON) Then
                If cmdAdvance(iLoop1).Text = ACT_ADVANCE_BUTTON Then
                    If CInt(Convert.ToString(cmdAdvance(iLoop1).Tag)) < iTempPos Then
                        iTempPos = CInt(Convert.ToString(cmdAdvance(iLoop1).Tag))
                    End If
                End If
            Next iLoop1

            ' Set the proper position
            m_iCurrentSequence = iTempPos - 1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrentSequenceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentSequence", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *******************************************************************
    '
    ' Name: StoreLedgerTitles
    '
    ' Description: Gets the names of the three ledgers from the database
    '              and stores them on the form
    '
    ' *******************************************************************
    Private Function StoreLedgerTitles() As Integer

        Dim result As Integer = 0

        Dim iLedgerID, iCompanyID As Integer
        Dim sLedgerName As New FixedLengthString(30)
        Dim sLedgerShortName As New FixedLengthString(2)
        Dim lMappingID, lLedgerTypeID As Integer
        Dim iIsDeletable As Integer
        Dim lCurrentPeriodID As Integer

        Dim sYearName As String = ""
        Dim iPeriodCompanyID As Integer
        Dim sPeriodName As String = ""
        Dim dPeriodEndDate As Date
        Dim iPeriodEndComplete, iSequence As Integer

        Dim iDisabledCnt As Integer

        Dim iPreviousPeriodEndComplete As Integer

        Dim vLedgerPeriods(ACT_MAX_LEDGERS - 1) As Object

        'KB PN 2662 1.6.9 -> 1.8.6 catchup

        Dim bAllowYearEnd As Boolean

        Dim lNextPeriodId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset the count of disabled buttons
            iDisabledCnt = 0

            ' Start the sequence at 3
            m_iCurrentSequence = ACT_MAX_LEDGERS

            ' There's three ledgers in this system
            For iLoop1 As Integer = 1 To ACT_MAX_LEDGERS

                ' Get the ledger title (and other info)

                m_lReturn = m_oACTLedger.GetNext(vLedgerID:=iLedgerID, vCompanyID:=iCompanyID, vLedgerName:=sLedgerName.Value, vLedgerShortName:=sLedgerShortName.Value, vMappingID:=lMappingID, vLedgertypeID:=lLedgerTypeID, vIsDeletable:=iIsDeletable, vCurrentPeriodID:=lCurrentPeriodID, vSequence:=iSequence)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'eck200700 display in advance sequence not stored sequence
                ' Store the sequence on the button
                '        frmInterface.cmdAdvance(iLoop1%).Tag = iSequence
                Me.cmdAdvance(iSequence).Tag = CStr(iSequence)
                'eck230800
                Me.lblLedger(iSequence).Tag = CStr(iLedgerID)
                '
                ' Set the caption
                '        frmInterface.lblLedger(iLoop1%).Caption = "&" & Trim$(sLedgerName) & " Ledger :"
                Me.lblLedger(iSequence).Text = "&" & sLedgerName.Value.Trim() & " Ledger " &
                                               ":"


                m_lReturn = m_oACTPeriod.GetDetails(vPeriodID:=lCurrentPeriodID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Now set the current period

                m_lReturn = m_oACTPeriod.GetNext(vPeriodID:=lCurrentPeriodID, vCompanyID:=iPeriodCompanyID, vYearName:=sYearName, vPeriodName:=sPeriodName, vPeriodEndDate:=dPeriodEndDate, vPeriodEndComplete:=iPeriodEndComplete)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lCurrentPeriodID <> 1 Then


                    m_lReturn = m_oBusiness.GetPreviousPeriodEndComplete(v_lCurrentPeriodID:=lCurrentPeriodID, r_lPreviousPeriodID:=m_lLastPeriodID, r_iPreviousPeriodEndComplete:=iPreviousPeriodEndComplete)

                    ' Enable/Disable the advance button
                    'eck210700 replace loop counter with sequence
                    If iPeriodEndComplete = 0 Then
                        If iPreviousPeriodEndComplete = 1 Then
                            Me.cmdAdvance(iSequence).Text = ACT_ADVANCE_BUTTON
                        Else
                            Me.cmdAdvance(iSequence).Text = ACT_RETREAT_BUTTON
                        End If
                    Else
                        Me.cmdAdvance(iSequence).Text = ACT_RETREAT_BUTTON
                    End If

                Else

                    Me.cmdAdvance(iSequence).Text = ACT_ADVANCE_BUTTON

                End If

                ' Set the caption

                'developer guide no.26
                Me.lblPeriod(iSequence).Text = sYearName.Trim() & " " &
                                                            sPeriodName.Trim()
                ' Store the period_id in the tag of the panel
                Me.pnlPeriod(iSequence).Tag = CStr(lCurrentPeriodID)

                ' Store it in the array

                vLedgerPeriods(iSequence - 1) = lCurrentPeriodID

                ' Set the "Current Period In" from the first ledger
                If iLoop1 = 1 Then

                    'developer guide no.26
                    Me.lblCurrentYear.Text = sYearName.Trim()

                    'Check if periods available to advance

                    m_lReturn = m_oACTPeriod.GetNextPeriodID(lPeriodID:=lCurrentPeriodID, lNextPeriodId:=lNextPeriodId)
                End If

                If Me.cmdAdvance(iSequence).Text = ACT_ADVANCE_BUTTON And lNextPeriodId = 0 Then
                    Me.cmdAdvance(iSequence).Enabled = False
                End If
            Next iLoop1

            ' Check that the periods are the same, if not, alter the captions
            m_lReturn = CType(CheckPeriodsMatch(v_vLedgerPeriods:=vLedgerPeriods), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check for to see if we have retreat buttons
            For iLoop1 As Integer = 1 To ACT_MAX_LEDGERS
                If cmdAdvance(iLoop1).Text = ACT_RETREAT_BUTTON Then
                    iDisabledCnt += 1
                End If
            Next iLoop1
            ' Do we want the apply button enabled or disabled?
            Me.cmdApply.Enabled = (iDisabledCnt = ACT_MAX_LEDGERS)
            'eck310701 Permit Year End Period End has been done and no ledgers Advanced yet
            If iDisabledCnt = ACT_MIN_LEDGERS Then
                ' Disable the period end button anyway
                'KB PN 2662 1.6.9 -> 1.8.6 catchup
                'DJM 13/01/2003 : Only enable year end button if the financial year has finished.
                bAllowYearEnd = False

                m_lReturn = m_oBusiness.AllowYearEnd(CInt(Convert.ToString(pnlPeriod(ACT_MAX_LEDGERS).Tag)), bAllowYearEnd)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Me.cmdPeriodEnd.Text = "Year End"
                If bAllowYearEnd Then
                    Me.cmdPeriodEnd.Enabled = True
                    Me.lblNote.Text = "To close financial year, transfer balances and produce year end reports, run year end"
                Else
                    Me.cmdPeriodEnd.Enabled = False
                    Me.lblNote.Text = ""
                End If
            Else
                Me.cmdPeriodEnd.Enabled = False
                Me.cmdPeriodEnd.Text = "Period End"
                Me.lblNote.Text = "Once all Ledger periods have been advanced, " &
                                  "Period End should be run."
            End If
            ' Calculate the current position in the sequence
            m_lReturn = CType(GetCurrentSequence(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the names of the ledgers", vApp:=ACApp, vClass:=ACClass, vMethod:="StoreLedgerTitles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetReports
    '
    ' Description: Calls the business to get all the data from the
    '              reports table.
    '
    ' History: 26/08/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetReports() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetReports(r_vReportArray:=m_vReportArray)
            'eck19062003 PN4872
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                ' Only log an error message if there are no reports
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve reports from database (PeriodEndReports). You will need to run any required reports manually", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReports", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                Return result
            End If

            ' Get the path of the reports into the print object

            m_lReturn = m_oReportPrint.GetReportPath()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve report paths.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReports", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReports Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReports", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ********************************************************************* '
    ' Name: GetPeriodName
    '
    ' Description: Gets the yearname and period name for the passed period
    ' eck230102 : Return PeriodEnd Date
    ' ********************************************************************* '
    Private Function GetPeriodName(ByVal v_lPeriodID As Integer, ByRef r_sPeriodName As String, ByRef r_sYearName As String, ByRef r_dtPeriodEndDate As Date) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Read the record

            m_lReturn = m_oACTPeriod.GetDetails(vPeriodID:=v_lPeriodID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the values
            'eck230102 return period end date

            m_lReturn = m_oACTPeriod.GetNext(vYearName:=r_sYearName, vPeriodName:=r_sPeriodName, vPeriodEndDate:=r_dtPeriodEndDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPeriodNameFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessAdvanceRetreat
    '
    ' Description: Processes either an Advance or Retreat click
    '
    ' ***************************************************************** '
    Private Function ProcessAdvanceRetreat(ByVal v_iCurrControl As Integer) As Integer
        'eck230800

        Dim result As Integer = 0
        Dim lPeriodID, lNextPeriodId, lPreviousPeriodID As Integer
        'eck230102
        Dim dtPeriodEndDate As Date

        Dim iDisableCnt As Integer

        Dim sYearName, sPeriodName As String

        Dim lRow As Integer
        Dim vLedgerID As Integer
        Dim vCompanyID, vSubBranchID, vLedgerName, vLedgerShortName, vMappingID, vLedgertypeID, vIsDeletable As Object
        Dim vCurrentPeriodID As Integer
        Dim vSequence As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'EK 11/11/99 Initialise row counter
            lRow = 0
            ' Get the current period ID from the control's tag property
            lPeriodID = CInt(Convert.ToString(pnlPeriod(v_iCurrControl).Tag))


            Select Case cmdAdvance(v_iCurrControl).Text
                Case ACT_ADVANCE_BUTTON

                    m_lLastPeriodID = lPeriodID

                    ' Get the next period number

                    m_lReturn = m_oACTPeriod.GetNextPeriodID(lPeriodID:=lPeriodID, lNextPeriodId:=lNextPeriodId)

                    ' Change the caption to retreat
                    cmdAdvance(v_iCurrControl).Text = ACT_RETREAT_BUTTON
                    pnlPeriod(v_iCurrControl).Tag = CStr(lNextPeriodId)

                    lPeriodID = lNextPeriodId

                Case ACT_RETREAT_BUTTON

                    ' Get the previous period number

                    m_lReturn = m_oACTPeriod.GetPreviousPeriodID(lPeriodID:=lPeriodID, lPreviousPeriodID:=lPreviousPeriodID)

                    ' Change the caption to advance
                    cmdAdvance(v_iCurrControl).Text = ACT_ADVANCE_BUTTON
                    pnlPeriod(v_iCurrControl).Tag = CStr(lPreviousPeriodID)

                    lPeriodID = lPreviousPeriodID

                    ' Get the previous period number

                    m_lReturn = m_oACTPeriod.GetPreviousPeriodID(lPeriodID:=lPeriodID, lPreviousPeriodID:=lPreviousPeriodID)

                    ' previous of retreated period
                    m_lLastPeriodID = lPreviousPeriodID

                Case Else

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unknown button pressed.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdvance_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

            End Select
            'EK 11/11/99 Get all ledgers with the same sequence as the lead ledger
            ' Update the ledger
            'eck230800
            vLedgerID = CInt(Convert.ToString(lblLedger(v_iCurrControl).Tag))

            m_lReturn = m_oACTLedger.GetClosures(vLedgerID:=vLedgerID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'EK 11/11/99

            While m_lReturn = gPMConstants.PMEReturnCode.PMTrue


                m_lReturn = m_oACTLedger.GetNext(vLedgerID:=vLedgerID, vCompanyID:=vCompanyID, vSubBranchID:=vSubBranchID, vLedgerName:=vLedgerName, vLedgerShortName:=vLedgerShortName, vMappingID:=vMappingID, vLedgertypeID:=vLedgertypeID, vIsDeletable:=vIsDeletable, vCurrentPeriodID:=vCurrentPeriodID, vSequence:=vSequence)
                Select Case m_lReturn
                    Case gPMConstants.PMEReturnCode.PMEOF
                    Case gPMConstants.PMEReturnCode.PMTrue
                        lRow += 1
                        vCurrentPeriodID = lPeriodID

                        m_lReturn = m_oACTLedger.EditUpdate(lRow:=lRow, vLedgerID:=vLedgerID, vCompanyID:=vCompanyID, vSubBranchID:=vSubBranchID, vLedgerName:=vLedgerName, vLedgerShortName:=vLedgerShortName, vMappingID:=vMappingID, vLedgertypeID:=vLedgertypeID, vIsDeletable:=vIsDeletable, vCurrentPeriodID:=vCurrentPeriodID, vSequence:=vSequence)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        m_lReturn = m_oACTLedger.Update()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Case Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                End Select

            End While
            ' Get the year and period names
            'eck230102 Return Period End Date
            m_lReturn = CType(GetPeriodName(v_lPeriodID:=lPeriodID, r_sPeriodName:=sPeriodName, r_sYearName:=sYearName, r_dtPeriodEndDate:=dtPeriodEndDate), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the tag on the panel
            ' frmInterface.pnlPeriod(v_iCurrControl%).Tag = lPeriodID

            ' Set the caption on the panel

            'developer guide no.26
            Me.lblPeriod(v_iCurrControl).Text = sYearName.Trim() & " " &
                                                            sPeriodName.Trim()
            ' Disable period end, this will be re-enabled later on if needed
            cmdPeriodEnd.Enabled = False

            ' Check to see if all buttons are ACT_RETREAT_BUTTON
            iDisableCnt = 0
            For iLoop1 As Integer = 1 To ACT_MAX_LEDGERS
                If cmdAdvance(iLoop1).Text = ACT_RETREAT_BUTTON Then
                    iDisableCnt += 1
                End If
            Next iLoop1

            ' If all buttons are retreat, then enable the Apply button
            If iDisableCnt = ACT_MAX_LEDGERS Then
                cmdApply.Enabled = True
                cmdApply.Focus()
            Else
                cmdApply.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAdvanceRetreatFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAdvanceRetreat", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNextLedgerSequence
    '
    ' Description: Gets the name of the next ledger that is expected
    '              to be advanced.
    '
    ' ***************************************************************** '
    'eck200700 pass current ledger index
    Private Function GetNextLedgerSequence(ByRef r_sLedgerName As String, ByRef r_iIndex As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Find the current button and grab the caption
            'eck200700
            Select Case cmdAdvance(r_iIndex).Text
                Case ACT_ADVANCE_BUTTON
                    r_sLedgerName = Me.lblLedger(r_iIndex - 1).Text
                Case ACT_RETREAT_BUTTON
                    r_sLedgerName = Me.lblLedger(r_iIndex + 1).Text
            End Select

            ' Right now, its "&Name Ledger:" so just get "Name"
            r_sLedgerName = r_sLedgerName.Substring(1, Math.Min(r_sLedgerName.Length, r_sLedgerName.Length - 1 - (" Ledger:").Length))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextLedgerSequenceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextLedgerSequence", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private isInitializingComponent As Boolean
    Private Sub cboSubBranch_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSubBranch.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        ' Gets the interface details to be displayed.
        m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get the interface details.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If
    End Sub

    Private Sub cmdAdvance_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdAdvance_2.Click, _cmdAdvance_1.Click, _cmdAdvance_3.Click
        Dim Index As Integer = Array.IndexOf(cmdAdvance, eventSender)

        Dim sLedgerName, sMsg, sThisName As String
        'KB PN 2662 1.6.9 -> 1.8.6 catchup
        Dim bAllowYearEnd As Boolean
        'sw 10/04/2003 added these declarations
        Const kAuthAccountsTransOptionNo As Integer = 81
        Dim sAuthAccountsTransOptionVal As String = ""

        Dim oUnpostedTrans As bSIRUnpostedTransactions.Business
        Dim vUnpostedTrans As Object
        'end sw 10/04/2003

        Try

            ' set the cursor to busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'start sw 10/04/2003 #####################################
            'Defer Transaction Posting Tech Spec
            'If the Authorise Accounts Transactions system option has been switched on
            'check for o/s transactions in the transaction_export_folder before
            'advancing the period end

            m_lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=kAuthAccountsTransOptionNo, r_sOptionValue:=sAuthAccountsTransOptionVal, v_iSourceID:=g_iSourceID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Set the cursor to normal
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Authorised Accounts Transactions system option.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdvance_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            If CInt(sAuthAccountsTransOptionVal) = gPMConstants.PMEReturnCode.PMTrue Then
                'now check for any o/s transactions
                'create the business object

                ' Get an instance of the business object via
                ' the public object manager.
                Dim temp_oUnpostedTrans As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oUnpostedTrans, "bSIRUnpostedTransactions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oUnpostedTrans = temp_oUnpostedTrans

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSIRUnpostedTransactions.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdvance_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                'call the GetDetails method to return any o/s transactions

                m_lReturn = oUnpostedTrans.GetDetails(r_vResultArray:=vUnpostedTrans)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed get the unposted transactions details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdvance_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                If Information.IsArray(vUnpostedTrans) Then
                    'there are unposted transactions outstanding so period end cannot be completed
                    MessageBox.Show("There are still unposted transactions waiting to be approved." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                    "These must be actioned before the Accounting Period can be advanced.", "Unposted Transactions Present", MessageBoxButtons.OK, MessageBoxIcon.Error)


                    oUnpostedTrans.Dispose()
                    oUnpostedTrans = Nothing
                    Me.Cursor = Cursors.Default
                    Exit Sub
                End If


                oUnpostedTrans.Dispose()
                oUnpostedTrans = Nothing


            End If

            'end sw 10/04/2003  ############################################



            ' Check to see if we're allowed to press this yet
            '     If (CLng(cmdAdvance(Index).Tag) >= m_iCurrentSequence) Then
            If (CInt(Convert.ToString(cmdAdvance(Index).Tag)) = m_iCurrentSequence + 1 And cmdAdvance(Index).Text = ACT_ADVANCE_BUTTON) Or (CInt(Convert.ToString(cmdAdvance(Index).Tag)) = m_iCurrentSequence And cmdAdvance(Index).Text = ACT_RETREAT_BUTTON) Then

                ' Process the button press
                m_lReturn = CType(ProcessAdvanceRetreat(v_iCurrControl:=Index), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Set the cursor to normal
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process advance or retreat.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdvance_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                ' Find out where we are in the ledger advancing sequence
                m_lReturn = CType(GetCurrentSequence(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Set the cursor to normal
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the current ledger sequence.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdvance_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

            Else

                ' Get the name of the expected ledger
                'eck200700 Pass index
                m_lReturn = CType(GetNextLedgerSequence(r_sLedgerName:=sLedgerName, r_iIndex:=Index), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the current ledger sequence.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdvance_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    ' dont exit
                End If

                ' Work out the name of the current ledger
                sThisName = lblLedger(Index).Text
                sThisName = sThisName.Substring(1, Math.Min(sThisName.Length, sThisName.Length - 1 - (" Ledger :").Length))

                ' Construct the message
                'eck200700 Depends whether we are advancing or retreating
                If cmdAdvance(Index).Text = ACT_ADVANCE_BUTTON Then
                    sMsg = "The " & sLedgerName & " Ledger must be advanced before the " &
                           sThisName & " Ledger."
                Else
                    sMsg = "The " & sLedgerName & " Ledger must retreat before the " & sThisName & " Ledger."
                End If
                ' And show it
                MessageBox.Show(sMsg, "Advance Ledger", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If
            'eck310701 Control Period end command button
            If m_iCurrentSequence = ACT_MIN_LEDGERS Then
                ' Disable the period end button anyway

                'KB PN 2662 1.6.9 -> 1.8.6 catchup
                'DJM 13/01/2003 : Only enable year end button if the financial year has finished.
                bAllowYearEnd = False

                m_lReturn = m_oBusiness.AllowYearEnd(CInt(Convert.ToString(pnlPeriod(ACT_MAX_LEDGERS).Tag)), bAllowYearEnd)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'On error disable year end button
                    bAllowYearEnd = False
                End If

                Me.cmdPeriodEnd.Text = "Year End"
                If bAllowYearEnd Then
                    Me.cmdPeriodEnd.Enabled = True
                    Me.lblNote.Text = "To close financial year, transfer balances and produce year end reports, run year end"
                Else
                    Me.cmdPeriodEnd.Enabled = False
                    Me.lblNote.Text = ""
                End If
                'KB PN 2662 1.6.9 -> 1.8.6 catchup end
            Else
                Me.cmdPeriodEnd.Enabled = False
                Me.cmdPeriodEnd.Text = "Period End"
                Me.lblNote.Text = "Once all ledger periods have been advanced, period end should be run."
            End If
            ' Set the cursor to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to advance or retreat period", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdvance_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        ' enable the period end button
        cmdPeriodEnd.Enabled = True
        cmdApply.Enabled = False

    End Sub

    ' ***************************************************************** '
    '
    ' Name: DisableControls
    '
    ' Description: Kinda like DisableForm in General, but it's in here instead.
    '
    ' History: 31/08/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function DisableControls(ByVal v_bEnabled As Boolean) As Integer
        'KB PN 2662 1.6.9 -> 1.8.6 catchup (Plus a bit)
        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' The following code could be changed to use For...Each.
            ' That's left as an exercise for the reader.

            'cmdAdvance(1).Enabled = v_bEnabled
            'cmdAdvance(2).Enabled = v_bEnabled
            'cmdAdvance(3).Enabled = v_bEnabled

            'KB Its hardly difficult and we want to set the captions anyway so ..
            For iLoop1 As Integer = 1 To ACT_MAX_LEDGERS
                cmdAdvance(iLoop1).Text = ACT_ADVANCE_BUTTON
                cmdAdvance(iLoop1).Enabled = v_bEnabled
            Next iLoop1


            cmdPeriodEnd.Enabled = v_bEnabled

            cmdCancel.Enabled = v_bEnabled
            cmdOK.Enabled = v_bEnabled
            cmdApply.Enabled = v_bEnabled
            cmdHelp.Enabled = v_bEnabled

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisableControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: ProcessReport
    '
    ' Description: Prints, saves and exports a report
    '
    ' History: 26/08/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessReport(ByVal v_sFileName As String, ByVal v_sDescription As String) As Integer

        Dim result As Integer = 0
        Dim vParamArray As Object
        'KB PN 2662 1.6.9 -> 1.8.6 catchup
        Dim vDefaultArray As Object

        Dim sText, sCompiledReportPath As String


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the status
            sText = "Processing report:" & Environment.NewLine & v_sDescription
            lblStatus.Text = sText


            m_oReportPrint.ReportName = v_sFileName


            ' Print and save to file

            m_oReportPrint.PrintReport = PMNavKeyConst.AC_PRINT_AND_VIEW

            ' Get the parameters

            m_lReturn = m_oReportPrint.GetParameters(vParamArray, vDefaultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Reset the status caption
                lblStatus.Text = ""
                ' Re-enable all the command buttons
                Return result
            End If

            ' Call SendToPrint

            m_lReturn = m_oReportPrint.SendToPrint(v_sReportTitle:=v_sFileName, r_sCompiledReportPath:=sCompiledReportPath, v_vParameters:=vParamArray)

            ' TODO - Store in DME now
            'm_lReturn& = ExportToDME(v_sFileName:=sCompiledReportPath$)
            'If (m_lReturn& <> PMTrue) Then
            '    ProcessReport = PMFalse
            '    ' Reset the status caption
            '    lblStatus.Caption = ""
            '    Exit Function
            'End If

            ' Reset the status caption
            lblStatus.Text = ""

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessReport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessReport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ReportsPeriodEnd
    '
    ' Description:
    '
    ' History: 26/08/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ReportsPeriodEnd() As Integer

        Dim result As Integer = 0
        Dim sFileName, sDescription As String
        'KB PN 2662 1.6.9 -> 1.8.6 catchup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop the array and process the period end reports
            For iLoop1 As Integer = m_vReportArray.GetLowerBound(1) To m_vReportArray.GetUpperBound(1)
                If CInt(m_vReportArray(ACReportPeriodEnd, iLoop1)) = 1 Then
                    ' Disable the form
                    m_lReturn = CType(DisableControls(v_bEnabled:=False), gPMConstants.PMEReturnCode)

                    ' Process the report
                    sFileName = CStr(m_vReportArray(ACReportFilename, iLoop1))
                    sDescription = CStr(m_vReportArray(ACReportDescription, iLoop1))
                    m_lReturn = CType(ProcessReport(v_sFileName:=sFileName, v_sDescription:=sDescription), gPMConstants.PMEReturnCode)

                    ' Re-enable the form
                    m_lReturn = CType(DisableControls(v_bEnabled:=True), gPMConstants.PMEReturnCode)


                End If
            Next iLoop1

            'KB PN 2662 1.6.9 -> 1.8.6 catchup (plus)
            'But not the period end button cos we've just run the period end
            If cmdPeriodEnd.Text = "Period End" Then
                cmdPeriodEnd.Enabled = gPMConstants.PMEReturnCode.PMFalse
            Else
                For iLoop2 As Integer = 1 To ACT_MAX_LEDGERS
                    cmdAdvance(iLoop2).Enabled = gPMConstants.PMEReturnCode.PMFalse
                Next iLoop2
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReportsPeriodEnd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportsPeriodEnd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ReportsYearEnd
    '
    ' Description:
    '
    ' History: 26/08/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ReportsYearEnd() As Integer

        Dim result As Integer = 0
        Dim sFileName, sDescription As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop the array and process the period end reports
            For iLoop1 As Integer = m_vReportArray.GetLowerBound(1) To m_vReportArray.GetUpperBound(1)
                If CInt(m_vReportArray(ACReportYearEnd, iLoop1)) = 1 Then
                    ' Disable the form
                    m_lReturn = CType(DisableControls(v_bEnabled:=False), gPMConstants.PMEReturnCode)

                    ' Process the report
                    sFileName = CStr(m_vReportArray(ACReportFilename, iLoop1))
                    sDescription = CStr(m_vReportArray(ACReportDescription, iLoop1))
                    m_lReturn = CType(ProcessReport(v_sFileName:=sFileName, v_sDescription:=sDescription), gPMConstants.PMEReturnCode)

                    ' Re-enable the form
                    m_lReturn = CType(DisableControls(v_bEnabled:=True), gPMConstants.PMEReturnCode)
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReportsYearEnd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReportsYearEnd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' *******************************************************************
    '
    ' Name: ProcessPeriodEnd
    '
    ' Description: Processes the period end when the PeriodEnd button
    '              is pressed.
    'eck 310701 Return Previous Period Id

    ' *******************************************************************
    Private Function ProcessPeriodEnd(ByVal v_lPeriodID As Integer, ByRef v_lPreviousPeriodId As Integer) As Integer

        Dim result As Integer = 0
        Dim sYearName, sPeriodName As String
        'eck230102
        Dim dtPeriodEndDate As Date
        Dim sMsg As String = ""
        Dim iContinue As DialogResult
        'eck230102

        Dim lPeriodID As Integer

        Dim bAllowYearEnd As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'eck230102 return period end date
            m_lReturn = CType(GetPeriodName(v_lPeriodID:=v_lPreviousPeriodId, r_sPeriodName:=sPeriodName, r_sYearName:=sYearName, r_dtPeriodEndDate:=dtPeriodEndDate), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'eck230102 Extra Bit here to warn if current period is being closed prematurely
            If dtPeriodEndDate >= DateTime.Now Then
                sMsg = "You are about to close the current period before the period end date." &
                       Strings.Chr(13) & Strings.Chr(10) & "Period does not end until " & DateTimeHelper.ToString(dtPeriodEndDate) &
                       Strings.Chr(13) & Strings.Chr(10) & "Are you sure? "
                iContinue = MessageBox.Show(sMsg, "Warning", MessageBoxButtons.YesNo)

                ' Check message result.
                If iContinue = System.Windows.Forms.DialogResult.No Then
                    Return result
                End If

            End If

            ' Call the business object to do the work
            'eck310701 return Previous Period Id

            m_lReturn = m_oBusiness.ProcessPeriodEnd(v_lCurrentPeriodID:=v_lPeriodID, v_lPreviousPeriodId:=v_lPreviousPeriodId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process period end.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPeriodEnd", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Do this after we've considered year end
            ' Set the captions on the advance buttons to "Advance"
            '  For iLoop1% = 1 To ACT_MAX_LEDGERS
            '     cmdAdvance(iLoop1%).Caption = ACT_ADVANCE_BUTTON
            'Next iLoop1%

            '' Get the current period ID from the control's tag property
            ' 'Just use the first panel, as they're all the same
            'lPeriodID& = pnlPeriod(1).Tag

            ' Get the year and period names
            'eck230102 return period end date

            'm_lReturn& = GetPeriodName( _
            ''        v_lPeriodID:=lPeriodID&, _
            ''        r_sPeriodName:=sPeriodName$, _
            ''        r_sYearName:=sYearName$, _
            ''        r_dtPeriodEndDate:=dtPeriodEndDate)
            'If (m_lReturn& <> PMTrue) Then
            '    ProcessPeriodEnd = PMFalse
            '    Exit Function
            'End If

            ' Set the year name
            'frmInterface.pnlCurrentYear.Caption = Trim$(sYearName$)

            ' Reset the ledger sequence
            'eck200700
            '    m_iCurrentSequence% = 3
            m_iCurrentSequence = 0

            'eck310701 Set up to allow Year End
            ' Disable the period end button
            'DJM 13/01/2003 : Only enable year end button if the financial year has finished.
            bAllowYearEnd = False
            'm_lReturn = m_oBusiness.AllowYearEnd(CLng(pnlPeriod(ACT_MAX_LEDGERS).Tag), bAllowYearEnd)
            'the pnlperiod has already been advanced use lPeriodID&

            m_lReturn = m_oBusiness.AllowYearEnd(v_lPreviousPeriodId, bAllowYearEnd)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If bAllowYearEnd Then
                Me.cmdPeriodEnd.Text = "Year End"
                Me.cmdPeriodEnd.Enabled = True
                Me.lblNote.Text = "To close financial year, transfer balances and produce year end reports run year end"
                'if we want to run a year end then dont allow ledgers to be advanced
                For iLoop1 As Integer = 1 To ACT_MAX_LEDGERS
                    cmdAdvance(iLoop1).Enabled = gPMConstants.PMEReturnCode.PMFalse
                Next iLoop1
            Else
                Me.cmdPeriodEnd.Enabled = False
                Me.lblNote.Text = ""
            End If

            '' Get the current period ID from the control's tag property
            ''Just use the first panel, as they're all the same
            lPeriodID = CInt(Convert.ToString(pnlPeriod(1).Tag))

            'Disable the apply button
            cmdApply.Enabled = False

            m_lReturn = CType(GetPeriodName(v_lPeriodID:=lPeriodID, r_sPeriodName:=sPeriodName, r_sYearName:=sYearName, r_dtPeriodEndDate:=dtPeriodEndDate), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the year name

            'developer guide no.26
            Me.lblCurrentYear.Text = sYearName.Trim()

            ' Get the reports
            m_lReturn = CType(GetReports(), gPMConstants.PMEReturnCode)
            'WE are saying the period end hasn't run cos there are no reports!
            'let report check tell us this
            'If (m_lReturn& <> PMTrue) Then
            '    If (m_lReturn& <> PMNotFound) Then
            '        ProcessPeriodEnd = pmfalse
            '    End If

            ' Disable the period end button
            'eck310701
            '       cmdPeriodEnd.Enabled = False
            ' Disable the apply button
            '   cmdApply.Enabled = False
            '   Exit Function

            'End If

            ' Process any period end reports now
            ' but only if there are some

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(ReportsPeriodEnd(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'eck310701 This should now be done in ProcessYearEnd
            ' If it's a year end, then
            '    If (bYearEnd = True) Then
            '        ' process the year end reports too.
            '        m_lReturn& = ReportsYearEnd()
            '        If (m_lReturn& <> PMTrue) Then
            '            ProcessPeriodEnd = PMFalse
            '            Exit Function
            '        End If
            '
            '    End If
            '   cmdPeriodEnd.Enabled = False

            ' Set the captions on the advance buttons to "Advance"
            For iLoop1 As Integer = 1 To ACT_MAX_LEDGERS
                cmdAdvance(iLoop1).Text = ACT_ADVANCE_BUTTON
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process period end.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPeriodEnd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'developer guide no.51
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID), gPMConstants.PMEReturnCode)

    End Sub
    ' *******************************************************************
    '
    ' Name: ProcessYearEnd
    '
    ' Description: Processes the Year end when the YearEnd button
    '              is pressed.
    '
    ' History: Created ECK 31/07/01
    '
    ' *******************************************************************
    Private Function ProcessYearEnd(ByVal v_lPeriodID As Integer) As Integer

        Dim result As Integer = 0
        Dim sYearName, sPeriodName As String
        'eck230102
        Dim dtPeriodEndDate As Date
        Dim sMsg As String = ""
        Dim iContinue As DialogResult
        'eck230102


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'eck230102 return period end date
            m_lReturn = CType(GetPeriodName(v_lPeriodID:=v_lPeriodID, r_sPeriodName:=sPeriodName, r_sYearName:=sYearName, r_dtPeriodEndDate:=dtPeriodEndDate), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'eck230102 Extra Bit here to warn if current period is being closed prematurely
            sMsg = "You are about to run a year end as at " & DateTimeHelper.ToString(dtPeriodEndDate) & Strings.Chr(13) & Strings.Chr(10) & "This will Process a Retained Profit Journal" & Strings.Chr(13) & Strings.Chr(10) & "Are you sure?"

            iContinue = MessageBox.Show(sMsg, "Warning", MessageBoxButtons.YesNo)
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030127 : Bug fix for PN Issue 10023 - START
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Check message result, if the user clicked no, exit the function
            If iContinue = System.Windows.Forms.DialogResult.No Then
                Return result
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030127 : Bug fix for PN Issue 10023 - END
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ' Call the business object to do the work

            m_lReturn = m_oBusiness.ProcessRetainedProfitJournalV2(v_lPeriodID:=v_lPeriodID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process period end.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessYearEnd", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cmdPeriodEnd.Enabled = False

            ' Get the reports
            ' We may well want year end reports in addition to period end ones
            ' Again do we really want to fail if no reports set up?
            m_lReturn = CType(GetReports(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    MessageBox.Show("Year end - running any required reports.", "Year end", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'ProcessYearEnd = PMFalse
                Else
                    ' process the year end reports too.
                    m_lReturn = CType(ReportsYearEnd(), gPMConstants.PMEReturnCode)

                End If

            End If



            ' Disable the period end button
            cmdPeriodEnd.Enabled = False

            ' Disable the apply button
            cmdApply.Enabled = False

            'Remove the message
            Me.lblNote.Text = ""

            'enable advance

            For iLoop1 As Integer = 1 To ACT_MAX_LEDGERS

                cmdAdvance(iLoop1).Text = ACT_ADVANCE_BUTTON
                cmdAdvance(iLoop1).Enabled = True

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process period end.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessYearEnd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub cmdPeriodEnd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPeriodEnd.Click

        Dim lNextPeriodId As Integer


        Dim lPeriodID As Integer = gPMFunctions.ToSafeLong(Convert.ToString(Me.pnlPeriod(1).Tag))


        Select Case cmdPeriodEnd.Text
            Case "Period End"

                ' Set the mouse pointer to busy.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


                'eck310701 Return Previous Period Id
                m_lReturn = CType(ProcessPeriodEnd(v_lPeriodID:=lPeriodID, v_lPreviousPeriodId:=m_lLastPeriodID), gPMConstants.PMEReturnCode)

                ' Set the mouse pointer to busy.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Case "Year End"

                ' Set the mouse pointer to busy.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                m_lReturn = CType(ProcessYearEnd(v_lPeriodID:=m_lLastPeriodID), gPMConstants.PMEReturnCode)

                ' Set the mouse pointer to busy.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Select

        'Check if period available to advance

        m_lReturn = m_oACTPeriod.GetNextPeriodID(lPeriodID:=lPeriodID, lNextPeriodId:=lNextPeriodId)

        If lNextPeriodId = 0 Then
            'Disable all advance buttons
            For iLoop As Integer = 1 To ACT_MAX_LEDGERS
                cmdAdvance(iLoop).Enabled = False
            Next iLoop
        End If

    End Sub

    ' PRIVATE Methods (End)


    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTPeriodEnd.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTPeriodEnd", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            ' Get an instance of bACTLedger
            Dim temp_m_oACTLedger As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oACTLedger, "bACTLedger.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oACTLedger = temp_m_oACTLedger
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTLedger", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            ' Get an instance of bACTPeriod
            Dim temp_m_oACTPeriod As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oACTPeriod, "bACTPeriod.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oACTPeriod = temp_m_oACTPeriod
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTPeriod", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the report print object
            Dim temp_m_oReportPrint As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oReportPrint, "bSIRReportPrint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oReportPrint = temp_m_oReportPrint
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If



            ' Create an instance of the general interface object.
            m_oGeneral = New iACTPeriodEnd.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the cancelled property to true. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            '    Cancelled = True

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

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

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'developer guide no.7
                    eventArgs.Cancel = True
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            ' Check for errors.


            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object
            'm_lReturn& = m_oBusiness.Terminate()


            m_oACTLedger.Dispose()
            m_oACTLedger = Nothing


            m_oACTPeriod.Dispose()
            m_oACTPeriod = Nothing


            m_oReportPrint.Dispose()
            m_oReportPrint = Nothing
            ' Destroy the instance of the business object
            ' from memory.
            '    Set m_oBusiness = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With
            'Developer Guie no 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        '    With tabMainTab
        '        ' Set the default button.
        '        If (.Tab < cmdNext.Count) Then
        '            cmdNext(.Tab).Default = True
        '        Else
        '            cmdOK.Default = True
        '        End If
        ''
        '        ' Now I know this is crap, this goes against
        '        ' all my principles, but for some reason when
        '        ' using the mouse to select a tab the setfocus
        '        ' code below doesn't work. The cursor sticks,
        '        ' and you can't tab off. Therefore I've used
        '        ' this to get around the problem.
        '        DoEvents
        ''
        '        ' Set focus to the first control on the tab.
        '        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
        '            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
        '        End If
        '    End With
        '
        'Catch 
        '
        '
        '
        ' Error Section.
        '
        '
        'tabMainTabPreviousTab = tabMainTab.SelectedIndex
        'End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

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
            If AttachToScheduler <> True Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
            End If
            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdNavigate_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdNavigate_Click()
    '
    ' Click event of the Navigate button.
    '
    'Try 
    '
    ' Set the interface status.
    'm_lStatus = gPMConstants.PMEReturnCode.PMNavigate
    '
    ' Process the next set of actions depending
    ' upon the interface task etc.
    'm_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
    '
    ' Check the return value.
    'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
    ' Everything OK, so we can hide the interface.
    'Me.Hide()
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    'Private Sub cmdNext_Click(Index As Integer)
    '
    '    On Error GoTo Err_cmdNextClick
    '
    ''    ' Change to the next tab.
    ''    If (tabMainTab.Tab < tabMainTab.Tabs - 1) Then
    ''        tabMainTab.Tab = Index + 1
    ''    End If
    ''
    ''    ' Set focus to the first control on the tab.
    ''    If (tabMainTab.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
    ''        m_ctlTabFirstLast(ACControlStart, Index + 1).SetFocus
    ''    End If
    '
    '    Exit Sub
    '
    'Err_cmdNextClick:
    '
    '    ' Error Section
    '
    '    Exit Sub
    '
    'End Sub
    '
    ' PRIVATE Events (End)

    'SD 05/03/2003 Migrating bug fixes for 1.8.6 ISSUE 2709
    Private Sub cboSubBranch_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSubBranch.SelectionChangeCommitted
        'KN (CMG) 04/02/03
        ' Gets the interface details to be displayed.
        m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get the interface details.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub
        End If
    End Sub
    Private Function LoadBatchParameters() As Integer

        Dim ctlFormControl As Object
        Dim controlValue As String = ""
        LoadBatchParameters = gPMConstants.PMEReturnCode.PMTrue
        Try
            If Information.IsArray(m_oBatchParameters) Then
                For lCount As Integer = m_oBatchParameters.GetLowerBound(1) To m_oBatchParameters.GetUpperBound(1)
                    controlValue = (m_oBatchParameters(1, lCount))
                    ctlFormControl = Me.Controls.Find(m_oBatchParameters(0, lCount), True).FirstOrDefault()
                    If Not ctlFormControl Is Nothing Then
                        If (TypeOf ctlFormControl Is TextBox) Then
                            ctlFormControl.Text = controlValue
                        ElseIf (TypeOf ctlFormControl Is Panel) Then
                            ctlFormControl.controls(0).Text = controlValue
                        ElseIf (TypeOf ctlFormControl Is Label) Then
                            ctlFormControl.Text = controlValue
                        ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                            ctlFormControl.Checked = IIf(controlValue = "1", CheckState.Checked, CheckState.Unchecked)

                        End If
                    Else
                        pnlCurrentYear.Controls(0).Text = controlValue
                    End If
                Next
            End If
        Catch
            LoadBatchParameters = gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function
    Private Sub CreateProcessParameter()

        m_dtParameters = New DataTable("Scheduler")


        m_dtParameters.Columns.AddRange(New DataColumn(4) {
                                                    New DataColumn("Id", System.Type.GetType("System.String")),
                                                   New DataColumn("ParameterName", System.Type.GetType("System.String")),
                                                   New DataColumn("DefaultValue", System.Type.GetType("System.String")),
                                                   New DataColumn("DataType", System.Type.GetType("System.String")),
                                                   New DataColumn("CurrentValue", System.Type.GetType("System.String"))})


    End Sub

    Private Sub cmdSchedule_Click(sender As Object, e As EventArgs) Handles cmdSchedule.Click
        Dim temp_ofrequency As Object = Nothing

        Dim iCnt As Integer = 0
        Dim m_sledgerName As String = String.Empty
        Dim m_sledgerText As String = String.Empty
        Dim m_sledgerPeriodName As String = String.Empty
        Dim m_sledgerPeriodValue As String = String.Empty

        If Task <> gPMConstants.PMEComponentAction.PMView Then
            CreateProcessParameter()
            m_dtParameters.LoadDataRow(New String(4) {String.Empty, pnlCurrentYear.Name, pnlCurrentYear.Controls(0).Text, "Integer", pnlCurrentYear.Controls(0).Text.ToString()}, True)
            For iSequence As Integer = 1 To 3

                m_sledgerName = Me.lblLedger(iSequence).Name
                m_sledgerText = Me.lblLedger(iSequence).Text
                m_sledgerPeriodName = Me.pnlPeriod(iSequence).Name
                m_sledgerPeriodValue = pnlPeriod(iSequence).Controls(0).Text
                m_dtParameters.LoadDataRow(New String(4) {String.Empty, m_sledgerName, m_sledgerText, "String", m_sledgerText.ToString()}, True)
                m_dtParameters.LoadDataRow(New String(4) {String.Empty, m_sledgerPeriodName, m_sledgerPeriodValue, "String", m_sledgerPeriodValue.ToString()}, True)
            Next
        End If

        m_sbatchContentDetails = Application.StartupPath() & "\" & "periodendcli.exe includeyearend=false"

        Dim iSIRFrequencyScheduler As iSIRFrequencyScheduler.Interface_Renamed = New iSIRFrequencyScheduler.Interface_Renamed
        m_lReturn = g_oObjectManager.GetInstance(temp_ofrequency, sClassName:="iSIRFrequencyScheduler.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        iSIRFrequencyScheduler = temp_ofrequency
        iSIRFrequencyScheduler.BatchProcessId = m_iBatchProcessId
        iSIRFrequencyScheduler.Process = "Period End"
        iSIRFrequencyScheduler.ProcessDescription = "Period End" + "_" + Now.ToString("yyyyMMddhhmm")
        iSIRFrequencyScheduler.BatchFileName = "PeriodEnd" & "_" & pnlCurrentYear.Controls(0).Text & "_" & Now.ToString("yyyyMMddhhmm")
        iSIRFrequencyScheduler.BatchFileContentDetails = m_sbatchContentDetails   'm_jobCode & ".log"
        iSIRFrequencyScheduler.BatchProcessName = m_sBatchProcessName
        iSIRFrequencyScheduler.ProcessParameters = m_dtParameters
        iSIRFrequencyScheduler.BatchSchedulerId = m_ibatchSchedulerId
        iSIRFrequencyScheduler.Task = m_iTask
        iSIRFrequencyScheduler.UserName = g_sUsername
        'iSIRFrequencyScheduler.Start()
        'Microsoft.VisualBasic.Compatibility.VB6.ShowForm(iPMUBatchRenewalJobs, 1)
        temp_ofrequency.Start()
        ' frmFrequency.ShowDialog()
        Me.Hide()
    End Sub


End Class
