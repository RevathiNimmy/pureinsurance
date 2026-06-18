Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 02nd October 2002
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    'developer guide no.7
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
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' Declare an instance of the general interface object.
    Private m_oGeneral As General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Form control
    Private m_oFormfields As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast() As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    'variant array to hold the transactions details

    Private m_vTransactions(,) As Object

    Const klUserError As Integer = 32767


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

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    '' ***************************************************************** '
    'Public Function DisplayLookupDetails() As Long
    '
    '    On Error GoTo Err_DisplayLookupDetails
    '
    '    DisplayLookupDetails = PMTrue
    '
    '    ' Get the lookup values.
    '
    '    m_lReturn& = GetLookupValues()
    '
    '    ' Check for errors.
    '    If (m_lReturn& <> PMTrue) Then
    '        DisplayLookupDetails = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Get all of the lookup details.
    '
    '    ' {* USER DEFINED CODE (Begin) *}
    '
    '    ' ************************************************************
    '    ' Enter your code here to retreive all of the lookup
    '    ' descriptions for a given lookup type.
    '    ' The GetLookupDetails function will allow you to do this.
    '    '
    '    ' Example:-
    '    '
    '    '    m_lReturn& = GetLookupDetails( _
    ''    '        sLookupTable:=PMLookupCodeName, _
    ''    '        ctlLookup:=cmbCodeName)
    '    '
    '    '    ' Check for errors.
    '    '    If (m_lReturn& <> PMTrue) Then
    '    '        DisplayLookupDetails = PMFalse
    '    '        Exit Function
    '    '    End If
    '    '
    '    ' NOTE: Replace this section with your new code.
    '    ' ************************************************************
    '
    '
    '    m_lReturn& = GetLookupDetails(sLookupTable:="contact_type", _
    ''        ctlLookup:=cmbContactType)
    '
    '    ' Check for errors.
    '    If (m_lReturn& <> PMTrue) Then
    '        DisplayLookupDetails = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Check for errors.
    '    If (m_lReturn& <> PMTrue) Then
    '        DisplayLookupDetails = PMFalse
    '
    '        ' Log Error.
    '        LogMessage _
    ''            iType:=PMLogError, _
    ''            sMsg:="Failed to retrieve the contact types from the business object ", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="DisplayLookupDetails"
    '    End If
    '
    '    ' {* USER DEFINED CODE (End) *}
    '
    '
    '    Exit Function
    '
    'Err_DisplayLookupDetails:
    '
    '    ' Error Section
    '
    '    DisplayLookupDetails = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to display the lookup details", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="DisplayLookupDetails", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    ' ***************************************************************************
    '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************************
    'Public Function SetFieldValidation() As Long
    '
    'Dim i As Integer
    '
    '    On Error GoTo Err_SetFieldValidation
    '
    '    Set m_oFormfields = CreateObject("iPMFormControl.FormFields")
    '
    '    m_oFormfields.LanguageID = g_iLanguageID%
    '
    '    SetFieldValidation = PMTrue
    '
    '    For i = 0 To Me.Count - 1
    '        If InStr(Me(i).Tag, "F;") > 0 Then
    '            m_oFormfields.AddNewFormField ctlControl:=Me(i).Control, _
    ''                lFormat:=GetFieldFormat(Me(i).Control), _
    ''                lMandatory:=GetMandatory(Me(i).Control)
    '        End If
    '    Next i
    '
    '    ' {* USER DEFINED CODE (Begin) *}
    '
    '    ' {* USER DEFINED CODE (End) *}
    '
    '    Exit Function
    '
    'Err_SetFieldValidation:
    '
    '    ' Error Section.
    '    SetFieldValidation = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to SetFieldValidation", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="SetFieldValidation", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    'Private Function GetLookupValues() As Long
    '
    '    On Error GoTo Err_GetLookupValues
    '
    '    GetLookupValues = PMTrue
    '
    '    ' Gets all of the lookup values.
    '
    '    ' Check the task.
    '    Select Case (m_iTask)
    '        Case PMAdd
    '            ' Get all of the lookup values.
    '            m_lReturn& = m_oBusiness.GetLookupValues( _
    ''                iLookupType:=PMLookupAll, _
    ''                vTableArray:=m_vLookupValues, _
    ''                iLanguageID:=g_iLanguageID%, _
    ''                vResultArray:=m_vLookupDetails)
    '
    '        Case PMEdit
    '            ' Get all of the lookup values with the correct
    '            ' effective date.
    '            m_lReturn& = m_oBusiness.GetLookupValues( _
    ''                iLookupType:=PMLookupAllEffective, _
    ''                vTableArray:=m_vLookupValues, _
    ''                iLanguageID:=g_iLanguageID%, _
    ''                vResultArray:=m_vLookupDetails)
    '
    '        Case PMView
    '            ' Get lookup values for viewing only.
    '            m_lReturn& = m_oBusiness.GetLookupValues( _
    ''                iLookupType:=PMLookupSingle, _
    ''                vTableArray:=m_vLookupValues, _
    ''                iLanguageID:=g_iLanguageID%, _
    ''                vResultArray:=m_vLookupDetails)
    '    End Select
    '
    '    ' Check for errors.
    '    If (m_lReturn& <> PMTrue) Then
    '        GetLookupValues = PMFalse
    '
    '        ' Log Error.
    '        LogMessagePopup _
    ''            iType:=PMLogError, _
    ''            sMsg:="Failed to get the lookup values from the business object", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetLookupValues"
    '
    '        Exit Function
    '    End If
    '
    '    Exit Function
    '
    'Err_GetLookupValues:
    '
    '    ' Error Section.
    '
    '    GetLookupValues = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to get all of the lookup values", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetLookupValues", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    '
    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'Private Function GetLookupDetails( _
    ''    sLookupTable As String, _
    ''    ctlLookup As Control) As Long
    '
    'Dim lRow As Long
    'Dim lCntr As Long
    'Dim bFoundMatch As Boolean
    '
    '' Lookup value contants.
    'Const ACValueTableName = 0
    'Const ACValueID = 1
    'Const ACValueStartPos = 2
    'Const ACValueNumber = 3
    '
    '' Lookup detail contants.
    'Const ACDetailKey = 0
    'Const ACDetailDesc = 1
    '
    '    On Error GoTo Err_GetLookupDetails
    '
    '    GetLookupDetails = PMTrue
    '
    '    ' Get the lookup values.
    '
    '    bFoundMatch = False
    '
    '    For lRow& = LBound(m_vLookupValues, 2) To UBound(m_vLookupValues, 2)
    '        ' Check for a match of the table name.
    '        If (Trim$(m_vLookupValues(ACValueTableName, lRow&)) = _
    ''        Trim$(sLookupTable$)) Then
    '            ' Found a match
    '            bFoundMatch = True
    '            Exit For
    '        End If
    '    Next lRow&
    '
    '    ' Check if there has been a table match.
    '    If (bFoundMatch = False) Then
    '        GetLookupDetails = PMFalse
    '
    '        ' Log Error.
    '        LogMessage _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to get details for the table, " & sLookupTable$, _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetLookupDetails"
    '
    '        Exit Function
    '    End If
    '
    '    ' Using the lookup values, populate the control with
    '    ' the details from the lookup details array.
    '
    '    For lCntr& = m_vLookupValues(ACValueStartPos, lRow&) To _
    ''    (m_vLookupValues(ACValueStartPos, lRow&) + m_vLookupValues(ACValueNumber, lRow&)) - 1
    '        ' Add the details to the control.
    '        ctlLookup.AddItem m_vLookupDetails(ACDetailDesc, lCntr&)
    '        ctlLookup.ItemData(ctlLookup.NewIndex) = CLng(m_vLookupDetails(ACDetailKey, lCntr&))
    '
    '        If (m_vLookupValues(ACValueID, lRow&) <> "") Then
    '            If (m_vLookupValues(ACValueID, lRow&) = _
    ''            CLng(m_vLookupDetails(ACDetailKey, lCntr&))) Then
    '                ctlLookup.ListIndex = ctlLookup.NewIndex
    '            End If
    '        End If
    '
    '    Next lCntr&
    '
    '    ' Check if the selected index is blank. If so,
    '    ' we set the controls index to zero.
    '    If (m_vLookupValues(ACValueID, lRow&) = "") Then
    '        ctlLookup.ListIndex = 0
    '    End If
    '
    '    Exit Function
    '
    'Err_GetLookupDetails:
    '
    '    ' Error Section.
    '
    '    GetLookupDetails = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to get all of the lookup details", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetLookupDetails", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    '





    ' ***************************************************************** '
    ' Name: UpdateDataStorage
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function UpdateDataStorage() As Integer
        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim lRowCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'update the data storage
            'loop through the array setting the exort status according to the
            'list item checked property
            If Not Information.IsArray(m_vTransactions) Then
                Return result
            End If

            lRowCount = m_vTransactions.GetUpperBound(1)

            For lRow As Integer = 0 To lRowCount

                oListItem = lvTrans.Items.Item(lRow)

                If oListItem.Checked Then
                    m_vTransactions(ACExportStatus, lRow) = "m"
                Else
                    m_vTransactions(ACExportStatus, lRow) = "p"
                End If

            Next lRow

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDataStorage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            'set the checkbox property of the listview
            m_lReturn = CType(SetExtraListViewProperties(lvTrans.Handle.ToInt32(), v_vCheckBoxes:=True), gPMConstants.PMEReturnCode)

            cmdPost.Enabled = False
            txtAgentCode.Enabled = True

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer
        '
        'Dim i As Integer
        'Dim lCaptionID As Long
        'Dim j As Integer
        '
        '    On Error GoTo Err_DisplayCaptions
        '
        Return gPMConstants.PMEReturnCode.PMTrue
        '
        '    ' Display all language specific captions.
        '
        '    'Get the caption from the tag
        '    lCaptionID = GetCaptionID(Me.Tag)
        '    If lCaptionID > 0 Then
        '        'Get the caption from the res file using Id from tag property
        '        Me.Caption = GetResData( _
        ''              iLangID:=g_iLanguageID%, _
        ''              lID:=lCaptionID, _
        ''              iDataType:=PMResString)
        '
        '        ' Check for an error.
        '        If (Me.Caption = "") Then
        '            ' Failed to get data from the resource file.
        '            DisplayCaptions = PMFalse
        '
        '            ' Log Error.
        '            LogMessage _
        ''                iType:=PMLogError, _
        ''                sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
        ''                "Please check the file exists and the correct captions are available", _
        ''                vApp:=ACApp, _
        ''                vClass:=ACClass, _
        ''                vMethod:="DisplayCaptions"
        '
        '            Exit Function
        '        End If
        '    End If
        '
        '    For i = 0 To Me.Count - 1
        '        lCaptionID = GetCaptionID(Me(i).Tag)
        '        If lCaptionID > 0 Then
        '            If TypeOf Me(i) Is SSTab Then
        '                For j = 0 To tabMainTab.Tabs - 1
        '                    tabMainTab.TabCaption(j) = GetResData( _
        ''                        iLangID:=g_iLanguageID%, _
        ''                        lID:=lCaptionID + j, _
        ''                        iDataType:=PMResString)
        '                Next j
        '            Else
        '                Me(i).Caption = GetResData( _
        ''                    iLangID:=g_iLanguageID%, _
        ''                    lID:=lCaptionID, _
        ''                    iDataType:=PMResString)
        '            End If
        '        End If
        '    Next i
        '
        '    ' {* USER DEFINED CODE (Begin) *}
        '
        '    ' {* USER DEFINED CODE (End) *}
        '
        '    Exit Function
        '
        'Err_DisplayCaptions:
        '
        '    ' Error Section.
        '
        '    DisplayCaptions = PMError
        '
        '    ' Log Error.
        '    LogMessage _
        ''        iType:=PMLogOnError, _
        ''        sMsg:="Failed to display the language captions", _
        ''        vApp:=ACApp, _
        ''        vClass:=ACClass, _
        ''        vMethod:="DisplayCaptions", _
        ''        vErrNo:=Err.Number, _
        ''        vErrDesc:=Err.Description
        '
        '    Exit Function

    End Function


    Private Sub cmdFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFind.Click

        Try

            m_lReturn = RefreshDataStorageAndinterface()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(klUserError.ToString() + ", Failed to refresh the ListView")
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the cmdFind_Click event", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private Function RefreshDataStorageAndinterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetDetails(r_vResultArray:=m_vTransactions, v_sAgentCode:=txtAgentCode.Text, v_lTransactionTypeID:=cboPMLookupTransType.ItemId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(m_vTransactions) Then
                lvTrans.Items.Clear()
                cmdPost.Enabled = False
                Return result
            End If

            m_lReturn = CType(PopulateListView(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the RefreshDataStorageAndinterface event", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function PopulateListView() As Integer
        Dim result As Integer = 0
        Dim lRowCount As Integer
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'clear out the current ls view contents
            lvTrans.Items.Clear()

            lRowCount = m_vTransactions.GetUpperBound(1)

            For lRow As Integer = 0 To lRowCount

                oListItem = lvTrans.Items.Add(CStr(m_vTransactions(ACClientCode, lRow)))

                oListItem.Checked = CStr(m_vTransactions(ACExportStatus, lRow)).Trim() = "m"

                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.NullToString(CStr(m_vTransactions(ACAgentCode, lRow))).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.NullToString(CStr(m_vTransactions(ACPolicyNumber, lRow))).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.NullToString(CStr(m_vTransactions(ACCoverStartDate, lRow))).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.NullToString(CStr(m_vTransactions(ACOperator, lRow))).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = gPMFunctions.NullToString(CStr(m_vTransactions(ACGrossAmount, lRow))).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 6).Text = gPMFunctions.NullToString(CStr(m_vTransactions(ACCommission, lRow))).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 7).Text = gPMFunctions.NullToString(CStr(m_vTransactions(ACTax, lRow))).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 8).Text = gPMFunctions.NullToString(CStr(m_vTransactions(ACCurrency, lRow))).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 9).Text = gPMFunctions.NullToString(CStr(m_vTransactions(ACTransactionType, lRow))).Trim()

            Next lRow

            cmdPost.Enabled = True

            Return result

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate the list view", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cmdPost_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPost.Click

        Dim oTransactions As bPMBTransactions.Automated
        Dim lRowCount, lTransCount, lFailCount As Integer
        Dim dblPercentIncrement As Double
        Dim sMessage As String = ""

        Try

            m_lReturn = CType(UpdateDataStorage(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(klUserError.ToString() + ", Unable to update the data storage")
            End If


            m_lReturn = m_oBusiness.Update(m_vTransactions)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(klUserError.ToString() + ", Unable to update the business object")
            End If

            Dim temp_oTransactions As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oTransactions, "bPMBTransactions.Automated", vInstanceManager:="ClientManager")
            oTransactions = temp_oTransactions 'Todo;


            'loop throught the transactions
            lRowCount = m_vTransactions.GetUpperBound(1)

            'Count the number of tranactions to post for the progress bar
            For lRow As Integer = 0 To lRowCount
                If CStr(m_vTransactions(ACExportStatus, lRow)) = "m" Then
                    lTransCount += 1
                End If
            Next

            If lTransCount = 0 Then
                MessageBox.Show("No transactions have been selected for posting", "Select Transaction", MessageBoxButtons.OK)
                Exit Sub
            End If

            fraProgress.Visible = True
            Me.lblProcessingTrans.Visible = True
            prgProcessingTrans.Value = 0

            dblPercentIncrement = 100 / lTransCount

            lTransCount = 0

            For lRow As Integer = 0 To lRowCount

                If CStr(m_vTransactions(ACExportStatus, lRow)) = "m" Then

                    lTransCount += 1
                    prgProcessingTrans.Value = Math.Floor((lTransCount) * dblPercentIncrement)
                    'send to orion

                    m_lReturn = oTransactions.SendToOrion(v_lTransactionFolderCnt:=CInt(m_vTransactions(ACCnt, lRow)), v_bTransferAuthorised:=True)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        lFailCount += 1
                    End If
                End If
            Next

            fraProgress.Visible = False
            prgProcessingTrans.Value = 0

            If lFailCount = 0 Then
                sMessage = "All selected transactions posted correctly."
            Else
                sMessage = "Attempted to post " & lTransCount & " transaction" & (IIf(lTransCount = 1, "", "s")) & " to Orion:" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Transaction post success count: " & CStr(lTransCount - lFailCount) & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Transaction post failed count: " & CStr(lFailCount)
            End If

            'Call frmProg.IncreasePercentage(100)
            prgProcessingTrans.Value = 100

            'display the result
            MessageBox.Show(sMessage, "Posting Completed", MessageBoxButtons.OK)



            oTransactions.Dispose()
            'refresh the listview and data storage

            m_lReturn = RefreshDataStorageAndinterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to refresh the data storage and interface", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPost_Click")
            End If

        Catch excep As System.Exception



            ' Error Section

            prgProcessingTrans.Value = 0
            fraProgress.Visible = True

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the cmdPost_click event", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPost_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()


        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRUnpostedTransactions.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness 'Todo;
            ' Create an instance of the general interface object.
            m_oGeneral = New General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

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

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            '
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            '    ' Set the rules for validating fields.
            '    m_lReturn& = SetFieldValidation()
            '    If (m_lReturn& <> PMTrue) Then
            '        Exit Sub
            '    End If
            '
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
                    eventArgs.Cancel = 1

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

            ' Terminate the business object

            m_oBusiness.Dispose()

            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ''    m_lReturn& = m_oFormfields.Terminate()
            ''
            ''    Set m_oFormfields = Nothing

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

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            If Information.IsArray(m_vTransactions) Then

                'updat the data storage
                m_lReturn = CType(UpdateDataStorage(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(klUserError.ToString() + ", Unable to update the data storage")
                End If

                'update the database

                m_lReturn = m_oBusiness.Update(m_vTransactions)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(klUserError.ToString() + ", Unable to update the business object")
                End If

            End If


            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
End Class