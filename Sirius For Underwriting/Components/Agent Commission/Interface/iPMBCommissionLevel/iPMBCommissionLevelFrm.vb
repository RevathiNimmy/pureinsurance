Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic.Compatibility
Imports System.Runtime.Remoting.Messaging


Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer


    ' {* USER DEFINED CODE (Begin) *}

    Private m_lItemsFound As Integer
    Private m_vCommissionLevel(,) As Object
    Private m_vSearchData(,) As Object
    Private m_vCommissionLvl(,) As Object
    Private m_vSaveSearchData(,) As Object
    'Variables to store data taken from the List View
    Private m_iAction As gPMConstants.PMEComponentAction
    Private m_lAgentPartyCnt As Integer
    Private lPartyCnt As Integer
    Private m_sAgentPartyShortname As String = ""
    Private m_lCommissionLevelId As Integer
    Private m_sCommissionLevelDescription As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lIsDeleted As Integer = 0
    Private m_lAgent_Commission_Level_Id As Integer = 0
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form



    Private m_iSelectedIndex As Integer = -1
    Dim PrevWidth As Integer = 464
    Dim PrevHeight As Integer = 269
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBCommissionLevel.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_oLookup As bPMLookup.Business

    ' Stores party agent type information.
    ' Private m_sPartyAgentType As String = ""

    ' Stores information if commission transfer is true
    ' Private m_bCommissionTransfer As Boolean

    ' Stores information for underwriter or agent.
    ' Private m_sUnderwritingOrAgency As String = ""

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    Public Property Task() As Integer
        Get

            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property
    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property
    Public Property SearchData() As Object
        Get

            Return VB6.CopyArray(m_vSearchData)

        End Get
        Set(ByVal Value As Object)

            m_vSearchData = Value

            m_vSaveSearchData = VB6.CopyArray(m_vSearchData)

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBCommissionLevel.General()


            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.
        PrevHeight = Me.Height
        PrevWidth = Me.Width
        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            Me.cboCommissionLevel.FirstItem = "(none)"

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception
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
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the form control object.
            m_oFormFields.Dispose()
            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Closing", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    'Private Sub frmInterface_ResizeBegin(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeBegin
    '    PrevWidth = Me.Width
    '    PrevHeight = Me.Height
    'End Sub

    'Private isInitializingComponent As Boolean
    'Private Sub frmInterface_Resize(sender As Object, e As EventArgs) Handles Me.Resize
    '    If isInitializingComponent Then
    '        Exit Sub
    '    End If
    '    Try
    '        '    m_lReturn = ResizeInterface()
    '    Catch
    '        Exit Sub
    '    End Try
    'End Sub

    ' ***************************************************************** '
    ' Name: DataRefresh
    '
    ' Description: Populate Associate Refreshs
    '              storage.
    '
    ' ***************************************************************** '
    Private Function DataRefresh() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer
        Dim iDelete As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the Refresh Refreshs.
            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}


            Select Case m_iAction
                Case gPMConstants.PMEComponentAction.PMAdd

                    'm_lCommissionLevelId = cboCommissionLevel.ItemId
                    'm_sCommissionLevelDescription = cboCommissionLevel.ItemCaption
                    'm_dtEffectiveDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtEffectiveDate))
                    'm_lIsDeleted = chkIsDeleted.CheckState

                    If Not Information.IsArray(m_vSearchData) Then
                        ReDim m_vSearchData(4, 0)
                        lSelectedItem = 0
                    Else
                        lSelectedItem = m_vSearchData.GetUpperBound(1) + 1
                        ReDim Preserve m_vSearchData(4, lSelectedItem)
                    End If

                    m_vSearchData(ACICommissionLevelID, lSelectedItem) = cboCommissionLevel.ItemId
                    m_vSearchData(ACICommissionLevelDescription, lSelectedItem) = CStr(cboCommissionLevel.ItemCaption)
                    m_vSearchData(ACIEffectiveDate, lSelectedItem) = gPMFunctions.ToSafeDate(m_oFormFields.UnformatControl(ctlControl:=txtEffectiveDate))
                    m_vSearchData(ACIIsDeleted, lSelectedItem) = chkIsDeleted.CheckState
                    m_vSearchData(ACIAgent_Commission_Level_Id, lSelectedItem) = 0


                Case gPMConstants.PMEComponentAction.PMEdit
                    lSelectedItem = Convert.ToString(lvwCommissionLevel.Items.Item(lvwCommissionLevel.FocusedItem.Index).Index)


                Case gPMConstants.PMEComponentAction.PMDelete
                    lSelectedItem = Convert.ToString(lvwCommissionLevel.Items.Item(lvwCommissionLevel.FocusedItem.Index).Index)

                    If lvwCommissionLevel.FocusedItem.ForeColor.Equals(Color.Gray) Then
                        iDelete = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_vSearchData(ACIIsDeleted, lSelectedItem) = iDelete
            End Select



            m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)


            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Refresh Refreshs from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataRefresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearDetail
    '
    ' Description: Clear Associate Details
    '              storage.
    '
    ' ***************************************************************** '
    Private Function ClearDetail() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lAgentPartyCnt = 0
            m_sAgentPartyShortname = ""
            m_lCommissionLevelId = 0
            m_sCommissionLevelDescription = ""
            m_dtEffectiveDate = DateTime.Today

            cboCommissionLevel.ItemId = m_lCommissionLevelId

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=m_dtEffectiveDate)

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToDetail
    '
    ' Description: Populate Associate Details
    '              storage.
    '
    ' ***************************************************************** '
    Private Function DataToDetail() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the Detail details.


            lSelectedItem = Convert.ToString(lvwCommissionLevel.Items.Item(lvwCommissionLevel.FocusedItem.Index).Index)

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            m_lCommissionLevelId = CInt(m_vSearchData(ACICommissionLevelID, lSelectedItem))
            m_sCommissionLevelDescription = CStr(m_vSearchData(ACICommissionLevelDescription, lSelectedItem))
            m_dtEffectiveDate = gPMFunctions.ToSafeDate(m_vSearchData(ACIEffectiveDate, lSelectedItem))
            m_lIsDeleted = gPMFunctions.ToSafeInteger(m_vSearchData(ACIIsDeleted, lSelectedItem))
            m_lAgent_Commission_Level_Id = gPMFunctions.ToSafeInteger(m_vSearchData(ACIAgent_Commission_Level_Id, lSelectedItem))
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    ' <need to be reviewed>
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        '  Dim oPartyBusiness As bSIRFindParty.Business
        Dim sFormattedCurrency As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the search details.
            lvwCommissionLevel.Items.Clear()

            m_lItemsFound = 0

            ' Check that search details are valid before
            ' continuing.

            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            ' Assign the details to the interface.
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                Dim dtAccountBalance As New DataTable
                Dim dtClaimIncurred As New DataTable
                'Don't show Deleted item
                If CStr(m_vSearchData(ACICommissionLevelID, lRow)) <> "" Then

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lItemsFound += 1

                    ' Assign the details to the first column.
                    ' Column 1 Commission Level

                    oListItem = lvwCommissionLevel.Items.Add(gPMFunctions.ToSafeString(m_vSearchData(ACICommissionLevelID, lRow)))
                    ' Assign details to the other columns

                    ' Column 2 Commission Level Description

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.ToSafeString(m_vSearchData(ACICommissionLevelDescription, lRow))

                    ' Column 3 Effective Date

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.ToSafeString(gPMFunctions.ToSafeDate(m_vSearchData(ACIEffectiveDate, lRow)))
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.ToSafeBoolean(m_vSearchData(ACIIsDeleted, lRow))
                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.ToSafeInteger(m_vSearchData(ACIAgent_Commission_Level_Id, lRow))

                    If m_vSearchData(ACIIsDeleted, lRow) = gPMConstants.PMEReturnCode.PMTrue Then

                        'Developer Guide No.(changed code as per work functionality)
                        oListItem.ForeColor = Color.Gray
                    Else
                        oListItem.ForeColor = Color.Black
                    End If




                    ' {* USER DEFINED CODE (End) *}

                    ' Set the tag property with the index of
                    ' the search data storage.

                    'oListItem.Tag = CStr(lRow)
                    oListItem.Tag = CStr(m_vSearchData(ACICommissionLevelID, lRow)).Trim()

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        If m_iSelectedIndex <= 0 Then lvwCommissionLevel.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        lvwCommissionLevel.Refresh()
                    End If

                End If
            Next lRow

            ' Enable the interface now that the search
            ' has completed.

            m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            If m_iSelectedIndex > 0 AndAlso lvwCommissionLevel.Items.Count - 1 >= m_iSelectedIndex Then
                lvwCommissionLevel.Items(m_iSelectedIndex).EnsureVisible()
                lvwCommissionLevel.Items(m_iSelectedIndex).Selected = True
                lvwCommissionLevel.Items(m_iSelectedIndex).Focused = True
            End If

            cmdDelete.Enabled = False
            cmdAdd.Enabled = Not (Task = gPMConstants.PMEComponentAction.PMView)
            cmdEdit.Enabled = False
            cmdOK.Enabled = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim lRow2 As Integer
        Dim bFirst As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            lRow2 = 0
            bFirst = True
            'eck 091003 PN7334
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            'Go thru Associate List to new Associate details
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                'If CStr(m_vSearchData(ACIPartyAgentShortname, lRow)) = "" Then
                ' Else
                If bFirst Then
                    ReDim m_vCommissionLevel(5, lRow2)
                    bFirst = False
                Else
                    lRow2 += 1
                    ReDim Preserve m_vCommissionLevel(5, lRow2)
                End If

                m_vCommissionLevel(ACICommissionLevelID, lRow2) = m_vSearchData(ACICommissionLevelID, lRow)
                m_vCommissionLevel(ACICommissionLevelDescription, lRow2) = m_vSearchData(ACICommissionLevelDescription, lRow)
                m_vCommissionLevel(ACIEffectiveDate, lRow2) = m_vSearchData(ACIEffectiveDate, lRow)
                m_vCommissionLevel(ACIIsDeleted, lRow2) = m_vSearchData(ACIIsDeleted, lRow)
                m_vCommissionLevel(ACIAgent_Commission_Level_Id, lRow2) = m_vSearchData(ACIAgent_Commission_Level_Id, lRow)


            Next lRow

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

            'DC Not Required
            '    m_lReturn& = m_oBusiness.GetNext()
            '
            '    ' {* USER DEFINED CODE (End) *}
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        BusinessToData = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to retreive the details from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="BusinessToData"
            '    End If
            '
            '    Exit Function

        Catch
        End Try




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            'GK 19/08/10 PN 74466
            'If the control name is cboSource then add none as the first option...
            If ctlLookup.Name = "cboCommissionLevel" Then
                ctlLookup.Items.Add(New VB6.ListBoxItem("(None)", 0))
            End If


            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            Dim newIndex As Integer = -1
            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)


                newIndex = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CInt(m_vLookupDetails(ACDetailKey, lCntr))))

                ' Check if this is the selected index.
                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then
                        ctlLookup.SelectedIndex = newIndex
                    End If
                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            ' PW120702 set to -1 to blank current entry
            '  however leave cboSource as it was as we don't want to
            '  change the current functionality
            'If (CStr(m_vLookupValues(ACValueID, lRow)) = "") Or (CDbl(m_vLookupValues(ACValueID, lRow)) = -1) Then
            If (CStr(m_vLookupValues(ACValueID, lRow)) = "" Or CStr(m_vLookupValues(ACValueID, lRow)) = "0") OrElse (CDbl(m_vLookupValues(ACValueID, lRow)) = -1) Then
                If ctlLookup.Name = "cboCommissionLevel" Then

                    'NIIT - type cast the control to combobox
                    'ctlLookup.ListIndex = 0
                    ctlLookup.SelectedIndex = 0
                Else

                    'NIIT - type cast the control to combobox
                    'ctlLookup.ListIndex = -1
                    ctlLookup.SelectedIndex = -1
                End If
            End If



            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

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

            '  m_lReturn = GetLookupDetails(sLookupTable:=SharedFiles.gSIRLibrary.SIRLookupCommissionLevel, ctlLookup:=cboCommissionLevel)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMNonMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            ''Reference must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCommissionLevel, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)
            'Display the ListView Tab
            tabDetailTab.Top = VB6.TwipsToPixelsY(120)
            tabDetailTab.Left = VB6.TwipsToPixelsX(120)
            tabMainTab.Top = VB6.TwipsToPixelsY(120)
            tabDetailTab.Left = VB6.TwipsToPixelsX(120)
            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            ' Set the column widths for the search list.
            lvwCommissionLevel.Columns.Item(0).Width = 0
            lvwCommissionLevel.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(2500))
            lvwCommissionLevel.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(2500))
            lvwCommissionLevel.Columns.Item(3).Width = 0 'CInt(VB6.TwipsToPixelsX(1500))
            lvwCommissionLevel.Columns.Item(4).Width = 0


            cmdEdit.Enabled = False

            cmdDelete.Enabled = False

            cmdAdd.Enabled = Not (Task = gPMConstants.PMEComponentAction.PMView)

            lvwCommissionLevel.FullRowSelect = True

            ' Get information about underwriter or agent.
            'm_lReturn = CType(iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=m_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    ' Error has occured
            '    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get underwriter or agent information.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")

            '    Return result
            'End If

            'If m_lPartyCnt <> 0 Then
            '    m_lReturn = CType(GetPartyAgentType(v_lPartyCnt:=m_lPartyCnt, r_sPartyAgentType:=m_sPartyAgentType), gPMConstants.PMEReturnCode)

            '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get party agent type information.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")

            '        Return result
            '    End If
            'End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdAdd_Click(sender As Object, e As EventArgs) Handles cmdAdd.Click
        m_iAction = gPMConstants.PMEComponentAction.PMAdd
        cmdOK.Enabled = False
        cmdCancel.Enabled = False
        '  tabMainTab.Top = VB6.TwipsToPixelsY(120000000)
        tabDetailTab.Top = VB6.TwipsToPixelsY(120)

        tabDetailTab.Visible = True
        tabMainTab.Visible = False
        m_lReturn = CType(ClearDetail(), gPMConstants.PMEReturnCode)
        m_lReturn = CType(SetUpCommissionLevel(), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        Dim Msg As String = ""
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



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                m_vSearchData = VB6.CopyArray(m_vSaveSearchData)

                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub


    'Private Sub lvwCommissionLevel_MouseDown(sender As Object, eventArgs As MouseEventArgs)
    '    Dim Button As Integer = CInt(eventArgs.Button)
    '    Dim Shift As Integer = Control.ModifierKeys \ &H10000
    '    'Modified by milan.rawat on 6/10/2010 10:04:48 PM refer developer guide no. 70(Latest Guide)
    '    'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
    '    'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

    '    Dim x As Single = eventArgs.X
    '    Dim y As Single = eventArgs.Y
    '    If Task <> gPMConstants.PMEComponentAction.PMView Then
    '        If lvwCommissionLevel.GetItemAt(x, y) Is Nothing Then
    '            cmdDelete.Enabled = False
    '            cmdAdd.Enabled = True
    '            cmdEdit.Enabled = False
    '        Else
    '            cmdDelete.Enabled = True
    '            cmdAdd.Enabled = True
    '            cmdEdit.Enabled = True
    '        End If
    '    End If
    'End Sub

    'Private Sub lvwCommissionLevel_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    If Not (lvwCommissionLevel.SelectedItems Is Nothing) Then
    '        If lvwCommissionLevel.SelectedItems.Count > 0 Then
    '            m_iSelectedIndex = lvwCommissionLevel.SelectedItems(0).Index
    '        End If
    '    End If
    'End Sub

    Private Sub lvwCommissionLevel_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles lvwCommissionLevel.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwCommissionLevel.Columns(e.Column)

        Static lastColumn As Integer
        Static lastOrder As SortOrder
        Dim iOrder As SortOrder

        Try

            If ListViewHelper.GetSortOrderProperty(lvwCommissionLevel) = SortOrder.Ascending Then
                iOrder = SortOrder.Descending
            Else
                iOrder = SortOrder.Ascending
            End If

            ListViewHelper.SetSortedProperty(lvwCommissionLevel, True)

            Select Case (Convert.ToString(ColumnHeader.Tag))
                Case "DateColumn"
                    ' Sort by date
                    ListViewFunc.ListViewSortByDate(lvwCommissionLevel, ColumnHeader.Index + 1 - 1, iOrder)
                Case "Currency"
                    ' Sort by currency
                    ListViewFunc.ListViewSortByValue(lvwCommissionLevel, ColumnHeader.Index + 1 - 1, iOrder)
                Case Else
                    ' Default sort
                    ListViewHelper.SetSortedProperty(lvwCommissionLevel, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwCommissionLevel, iOrder)

                    ListViewHelper.SetSortKeyProperty(lvwCommissionLevel, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwCommissionLevel, True)
                    iOrder = ListViewHelper.GetSortKeyProperty(lvwCommissionLevel)
            End Select

            lastColumn = ColumnHeader.Index + 1
            lastOrder = iOrder

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwCommissionLevel_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Exit Sub

        End Try
    End Sub

    Private Sub lvwCommissionLevel_KeyDown(sender As Object, eventargs As KeyEventArgs)
        Dim KeyCode As Integer = eventargs.KeyCode
        Dim Shift As Integer = eventargs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabDetailTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabDetailTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabDetailTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetSelectedIndex(tabDetailTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetTabCount(tabDetailTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabDetailTab) < (SSTabHelper.GetTabCount(tabDetailTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetSelectedIndex(tabDetailTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                        End If
                End Select
            End With
            'developer guide no.293

            If eventargs.Alt And eventargs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try
    End Sub

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
            'cmdEdit.Enabled = Not bDisable

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdEdit_Click(sender As Object, e As EventArgs) Handles cmdEdit.Click
        m_iAction = gPMConstants.PMEComponentAction.PMEdit
        cmdOK.Enabled = False
        cmdCancel.Enabled = False
        tabMainTab.Top = VB6.TwipsToPixelsY(120)
        tabDetailTab.Top = VB6.TwipsToPixelsY(120)
        tabDetailTab.Visible = True
        tabMainTab.Visible = False
        m_lReturn = CType(DataToDetail(), gPMConstants.PMEReturnCode)
        m_lReturn = CType(SetUpCommissionLevel(), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cmdDetailOK_Click(sender As Object, e As EventArgs) Handles cmdDetailOK.Click

        m_lReturn = CType(ValidateCommissionLevel(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        tabMainTab.Visible = True
        tabDetailTab.Visible = False
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True
        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cmdDetailCancel_Click(sender As Object, e As EventArgs) Handles cmdDetailCancel.Click
        tabDetailTab.Top = VB6.TwipsToPixelsY(120)
        tabMainTab.Top = VB6.TwipsToPixelsY(120)
        tabMainTab.Visible = True
        tabDetailTab.Visible = False
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            cmdOK.Enabled = True
        End If

    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        m_iAction = gPMConstants.PMEComponentAction.PMDelete

        Dim lSelectedItem As Integer = Convert.ToString(lvwCommissionLevel.Items.Item(lvwCommissionLevel.FocusedItem.Index).Tag)
        '   m_vSearchData(ACIAssociatePartyShortname, lSelectedItem) = ""
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True
        m_lReturn = CType(DataRefresh(), gPMConstants.PMEReturnCode)
    End Sub

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabDetailTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function ValidateCommissionLevel() As Integer
        Dim lCommissionLevelId As Integer
        Dim dEffectiveDate As Date
        Dim bContinue As Boolean = True
        Dim sMessage As String
        Try

            m_lReturn = m_oFormFields.CheckMandatoryControls()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lCommissionLevelId = cboCommissionLevel.ItemId
            dEffectiveDate = ToSafeDate(txtEffectiveDate.Text)
            If lCommissionLevelId = 0 Then
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNullCommissionLevel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                MessageBox.Show(sMessage, "Validation Message", MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_vSearchData IsNot Nothing Then
                For i As Integer = 0 To m_vSearchData.GetUpperBound(1)
                    If m_vSearchData(0, i) = lCommissionLevelId AndAlso m_vSearchData(3, i) = 0 AndAlso ToSafeDate(m_vSearchData(2, i)) > ToSafeDate(dEffectiveDate) Then
                        bContinue = False
                        Exit For
                    End If
                Next
            End If

            If Not bContinue Then

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIncorrectEffectiveDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                MessageBox.Show(sMessage, "Validation Message", MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return gPMConstants.PMEReturnCode.PMTrue
        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetCommissionLevelValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetCommissionLevelValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMError

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


            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).


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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function SetUpCommissionLevel() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '' Assign the details to the interface.

            cboCommissionLevel.ItemId = m_lCommissionLevelId
            txtEffectiveDate.Text = gPMFunctions.ToSafeString((gPMFunctions.ToSafeDate(m_dtEffectiveDate)))
            chkIsDeleted.Checked = ToSafeBoolean(m_lIsDeleted)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set up commission level", vApp:=ACApp, vClass:=ACClass, vMethod:="SetUpCommissionLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdHelp_Click(sender As Object, e As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, MainModule.ScreenHelpID)
    End Sub

    Private Sub lvwCommissionLevel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvwCommissionLevel.SelectedIndexChanged
        If Not (lvwCommissionLevel.SelectedItems Is Nothing) Then
            If lvwCommissionLevel.SelectedItems.Count > 0 Then
                m_iSelectedIndex = lvwCommissionLevel.SelectedItems(0).Index
            End If
        End If
    End Sub

    Private Sub lvwCommissionLevel_MouseDown(sender As Object, eventArgs As MouseEventArgs) Handles lvwCommissionLevel.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Modified by milan.rawat on 6/10/2010 10:04:48 PM refer developer guide no. 70(Latest Guide)
        'Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        'Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwCommissionLevel.GetItemAt(x, y) Is Nothing Then
                cmdDelete.Enabled = False
                cmdAdd.Enabled = True
                cmdEdit.Enabled = False
            Else
                cmdDelete.Enabled = True
                cmdAdd.Enabled = True
                cmdEdit.Enabled = True
            End If
        End If
    End Sub

    Private Sub lvwCommissionLevel_Click(sender As Object, e As EventArgs) Handles lvwCommissionLevel.Click
        If Not (lvwCommissionLevel.FocusedItem Is Nothing) Then


            'If Me.lvwRiskType.SelectedItem.Ghosted Then
            If Me.lvwCommissionLevel.FocusedItem.ForeColor.Equals(Color.Gray) Then

                cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUndeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                cmdEdit.Enabled = False
            Else


                cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
        End If
    End Sub
End Class