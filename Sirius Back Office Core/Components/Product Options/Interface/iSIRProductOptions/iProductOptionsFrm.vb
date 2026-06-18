Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 11th June 2002
    '
    ' Description: Main interface.
    '
    ' Edit History:
    '   11062002 SJP - Created
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    '   These are used for import export
    Private Const sCOption As String = "Option"
    Private Const sCValue As String = "Value"
    Private Const sCTotalNumber As String = "TotalNumber="

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iSIRProductOptions.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    '   This will determine if should exit
    Private m_bExit As Boolean

    '   List whether or not this is a Sirius Installer
    Private m_bSiriusInstaller As Boolean

    'List array
    Private m_vProductOptions(,) As Object
    Private m_vBranches(,) As Object

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control






    ' Stores the details from the business object.

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

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
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

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

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)




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

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 0)

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

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: doCommonDialog()
    '
    ' Description: This will copy Branch values from the array
    '               to the Combo Box
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Function doCommonDialog(ByVal v_sFilter As String, ByVal v_sDialogTitle As String, ByVal v_bOpen As Boolean) As String

        Dim result As String = String.Empty
        Try



            'Modified
            'With cdgFile
            If v_sDialogTitle = "Please select INI file to save to" Then
                With cdgFileSave


                    .Filter = v_sFilter
                    .FilterIndex = 1
                    .InitialDirectory = My.Application.Info.DirectoryPath
                    .Title = v_sDialogTitle
                    .FileName = ""
                    If v_bOpen Then
                        .ShowDialog()
                    Else
                        .ShowDialog()
                    End If

                    result = .FileName

                End With
            Else
                With cdgFileOpen


                    .Filter = v_sFilter
                    .FilterIndex = 1
                    .InitialDirectory = My.Application.Info.DirectoryPath
                    .Title = v_sDialogTitle
                    .FileName = ""
                    If v_bOpen Then
                        .ShowDialog()
                    Else
                        .ShowDialog()
                    End If

                    result = .FileName
                End With
            End If

            Return result

        Catch

            ' User pressed cancel button

            Return result
        End Try

    End Function

    ' ******************************************************'
    '
    ' Name: initialiseComboBox()
    '
    ' Description: This will copy Branch values from the array
    '               to the Combo Box
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Function initialiseComboBox(ByVal v_vBranches(,) As Object) As Integer

        Dim result As Integer = 0
        Dim iBranchEnd As Integer

        Try

            '   Clears Combo Box
            cboBranch.Items.Clear()

            '   Either populate with Head Office or ALL branches
            If m_bSiriusInstaller Then
                iBranchEnd = 0
            Else
                iBranchEnd = v_vBranches.GetUpperBound(1)
            End If

            If iBranchEnd = 0 Then
                For iLoop As Integer = 0 To v_vBranches.GetUpperBound(1)

                    If ToSafeString(v_vBranches(1, iLoop)).Trim().ToUpper() = "HEADOFF" Then
                        Dim cboBranch_NewIndex As Integer = -1

                        cboBranch_NewIndex = cboBranch.Items.Add(CStr(v_vBranches(2, iLoop)).Trim())

                        VB6.SetItemData(cboBranch, cboBranch_NewIndex, CInt(v_vBranches(0, iLoop)))
                        Exit For
                    End If
                Next iLoop
                cboBranch.SelectedIndex = 0
            Else
                For iLoop As Integer = 0 To iBranchEnd

                    'developer guide no. 29
                    Dim cboBranch_NewIndex As Integer = -1
                    cboBranch_NewIndex = cboBranch.Items.Add(CStr(v_vBranches(2, iLoop)).Trim())

                    VB6.SetItemData(cboBranch, cboBranch_NewIndex, CInt(v_vBranches(0, iLoop)))

                    If CStr(v_vBranches(1, iLoop)).Trim() = "HeadOff" Then
                        cboBranch.SelectedIndex = iLoop
                    End If
                Next iLoop
            End If

            '   Set it always to Head Office
            '    cboBranch.ListIndex = 0

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="initialiseComboBox failed", vApp:=ACApp, vClass:=ACClass, vMethod:="initialiseComboBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: getOptions()
    '
    ' Description: This will get all Options from the business
    '               class
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Function getOptions(ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            '   This will get the Master Options list or the one
            '   in the database.
            If m_bSiriusInstaller Then

                lResult = m_oBusiness.getMasterOptions(r_vResultArray:=r_vArray)
            Else

                lResult = m_oBusiness.getProductOptionsWithDesc(r_vResultArray:=r_vArray)
            End If


            Return lResult

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getOptions failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getOptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: dataToInterface
    '
    ' Description: This is used for the General class only
    '               It will do nothing
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Public Function dataToProperties() As Integer

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' ******************************************************'
    '
    ' Name: dataToInterface
    '
    ' Description: This will refresh the list view based on
    '               the values in the array
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Public Function dataToInterface() As Integer
        Dim result As Integer = 0
        Dim lstItem As ListViewItem
        Dim sKey, sText, sOptionValue As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the list
            lvwSettings.Items.Clear()

            ' If we don't have anything then exit
            If (Not Information.IsArray(m_vProductOptions)) Or Not m_bSiriusInstaller Then
                Return result
            End If

            '   This adds all Options with the relevant Branch Id
            For iLoop As Integer = 0 To m_vProductOptions.GetUpperBound(1)
                If CInt(m_vProductOptions(1, iLoop)) = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex) Then
                    ' Generate a key
                    sKey = "X" & CStr(m_vProductOptions(0, iLoop)).Trim()
                    ' Get the text to use
                    sText = CStr(m_vProductOptions(4, iLoop)).Trim()

                    ' Add the item to the list view
                    lstItem = lvwSettings.Items.Add(sKey, sText, "")

                    sOptionValue = CStr(m_vProductOptions(2, iLoop)).Trim()
                    '   Puts disabled in list view
                    If m_bSiriusInstaller And sOptionValue = "" Then
                        sOptionValue = g_sDisabled
                    End If

                    ListViewHelper.GetListViewSubItem(lstItem, 1).Text = sOptionValue
                    ListViewHelper.GetListViewSubItem(lstItem, 2).Text = CStr(m_vProductOptions(5, iLoop))
                End If
            Next iLoop

            ' Auto size the list
            'Developer Guide no.178
            m_lReturn = CType(ListViewFunc.ListViewAutoSize(lvwList:=lvwSettings, bSizeHeaders:=True), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' No great shakes
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="dataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="dataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: addBranchValues()
    '
    ' Description: This will add any values for the Branch
    '               that do not already exist.  Will default
    '               in head office values.
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Function addBranchValues() As Integer

        Dim result As Integer = 0
        Dim iColumns As Integer
        Dim iRows As Integer
        Dim iNewRows As Integer
        Dim iHeadOfficeNoOptions, iBranchOptions, iBranch As Integer
        Dim bFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iBranch = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)

            '   If Branch is head office then we can exit safely
            If iBranch = g_iBCHHeadOffice Then
                Return result
            End If

            '   Check no of head office options on system
            iHeadOfficeNoOptions = 0
            iRows = m_vProductOptions.GetUpperBound(1)
            For iLoop As Integer = 0 To iRows
                If CInt(m_vProductOptions(1, iLoop)) = g_iBCHHeadOffice Then
                    iHeadOfficeNoOptions += 1
                End If
            Next iLoop

            '   Check no of branch options on system
            iBranchOptions = 0
            For iLoop As Integer = 0 To iRows
                If CInt(m_vProductOptions(1, iLoop)) = iBranch Then
                    iBranchOptions += 1
                End If
            Next iLoop

            '   Branch options = Head Office Options then exit
            If iBranchOptions = iHeadOfficeNoOptions Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            '   This will redimension the array correctly
            iColumns = m_vProductOptions.GetUpperBound(0)
            iNewRows = iRows + (iHeadOfficeNoOptions - iBranchOptions)
            ReDim Preserve m_vProductOptions(iColumns, iNewRows)
            iNewRows = iRows + 1

            '   Add all head office values not included with the Branch
            For iLoop As Integer = 0 To iRows

                If CInt(m_vProductOptions(1, iLoop)) = g_iBCHHeadOffice Then
                    bFound = False
                    For iLoop2 As Integer = 0 To iRows
                        If CInt(m_vProductOptions(1, iLoop2)) = iBranch Then
                            If CInt(m_vProductOptions(0, iLoop2)) = CInt(m_vProductOptions(0, iLoop)) Then
                                bFound = True
                                iLoop2 = iRows
                            End If
                        End If
                    Next iLoop2

                    '   Copy the columns over
                    If Not bFound Then
                        m_vProductOptions(0, iNewRows) = m_vProductOptions(0, iLoop)
                        m_vProductOptions(1, iNewRows) = iBranch
                        m_vProductOptions(2, iNewRows) = m_vProductOptions(2, iLoop)
                        m_vProductOptions(3, iNewRows) = m_vProductOptions(3, iLoop)
                        m_vProductOptions(4, iNewRows) = m_vProductOptions(4, iLoop)
                        m_vProductOptions(5, iNewRows) = m_vProductOptions(5, iLoop)
                        iNewRows += 1
                    End If

                End If
            Next iLoop


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=" AddBranchValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddBranchValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: getBusiness()
    '
    ' Description: This will perform get all Data
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Public Function getBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.getBranches(r_vResultArray:=m_vBranches)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = m_lReturn
                Return m_lReturn
            End If

            '   Get the Product Options
            m_lReturn = CType(getOptions(m_vProductOptions), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    m_lErrorNumber = m_lReturn
                    Return m_lReturn
                End If
            End If

            '   Initialise the Combo Box, also Refresh the List View
            m_lReturn = CType(initialiseComboBox(m_vBranches), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = m_lReturn
                Return m_lReturn
            End If

            '   Copy the Array to the Business Data
            'm_lReturn& = dataToInterface
            'If (m_lReturn& <> PMTrue) Then
            '    m_lErrorNumber& = PMFalse
            '    Exit Sub
            'End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    ' ******************************************************'
    '
    ' Name:  SetItems
    '
    ' Description: This will set all selected listview items
    '               with the value
    '
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Function SetItems(ByRef v_vValue As String) As Integer

        Dim result As Integer = 0
        Dim iOptionNumber As gPMConstants.SIRHiddenOptions
        Dim sOptNumber As String = ""
        'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        Dim sMessage, sTitle As String
        Dim lAnswer As DialogResult
        'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '   Need to update the array with the selected values
            For Each lItem As ListViewItem In lvwSettings.Items
                If lItem.Selected Then

                    sOptNumber = lItem.Name
                    iOptionNumber = CType(CInt(sOptNumber.Substring(sOptNumber.Length - (sOptNumber.Length - 1))), gPMConstants.SIRHiddenOptions)

                    ' PW190303 - If this is the Force Numeric Client Code option
                    '            and it is being enable, need to ensure the unique
                    '            number record has been created
                    ' PS186
                    If iOptionNumber = gPMConstants.SIRHiddenOptions.SIROPTForceNumericClientCode And Conversion.Val(v_vValue) = 1 Then
                        m_lReturn = CreateUniqueNumberForClientCode()
                    End If

                    For iLoop As Integer = 0 To m_vProductOptions.GetUpperBound(1)
                        If CInt(m_vProductOptions(1, iLoop)) = (VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)) Then
                            If CInt(m_vProductOptions(0, iLoop)) = iOptionNumber Then
                                'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                                'If option SIROPTEnableUniqueDocumentReference is enabled in the existing sytem, don't disable it.
                                If iOptionNumber = gPMConstants.SIRHiddenOptions.SIROPTEnableUniqueDocumentReference Then

                                    If v_vValue = "" And CStr(m_vProductOptions(2, iLoop)) <> "" Then
                                        ' Get description from the resource file.

                                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUniqueDocumentDisableFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                                        ' Display message.
                                        MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)

                                        v_vValue = "1"
                                    ElseIf v_vValue <> "" And CStr(m_vProductOptions(2, iLoop)) = "" Then

                                        ' Get description from the resource file.

                                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUniqueDocumentEnableWarning, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                                        ' Display message.
                                        lAnswer = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                        If lAnswer = System.Windows.Forms.DialogResult.No Then
                                            v_vValue = ""
                                        End If
                                    End If
                                End If
                                'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                                m_vProductOptions(2, iLoop) = v_vValue
                                iLoop = m_vProductOptions.GetUpperBound(1)
                            End If
                        End If
                    Next iLoop
                End If
            Next lItem

            '   This will refresh list view
            m_lReturn = dataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPassword failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPassword", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: cboBranch_Click()
    '
    ' Description: This occurs whenever we click on the combo
    '               box button
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Sub cboBranch_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranch.SelectedIndexChanged

        Try

            '   This will determine if all values are included for branch
            m_lReturn = addBranchValues()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            '   This will refresh the list view
            m_lReturn = dataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Combo Box failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cboBranchClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ******************************************************'
    '
    ' Name: cmdExit_Click()
    '
    ' Description: This occurs if we click on Exit button
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click

        Try

            m_bExit = True
            'Developer Guide no. 231
            Me.Close()

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Exit Command failed failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExit_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ******************************************************'
    '
    ' Name: cmdDisable_Click
    '
    ' Description: This occurs if we click the Disable button
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Sub cmdDisable_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDisable.Click

        Try

            m_lReturn = CType(SetItems(""), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDisableClick failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDisableClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ******************************************************'
    '
    ' Name: cmdHelp_click
    '
    ' Description: This does the product help
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        Dim sHelp As String = ""

        Try


            sHelp = sHelp & "SetValue" & Strings.Chr(13) & Strings.Chr(10)
            sHelp = sHelp & "To set a value for product option with the contents of the value input"
            sHelp = sHelp & " to the left please select a option(s) and then click on the SetValue "
            sHelp = sHelp & "button.  However no changes are permanent until "
            sHelp = sHelp & "the OK button is pressed" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            If m_bSiriusInstaller Then
                sHelp = sHelp & "Disable" & Strings.Chr(13) & Strings.Chr(10)
                sHelp = sHelp & "To disable a product option select the "
                sHelp = sHelp & "option(s) and then click on the Disable button."
                sHelp = sHelp & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
            End If

            sHelp = sHelp & "Import Product Options" & Strings.Chr(13) & Strings.Chr(10)
            sHelp = sHelp & "This will import all product options into the database "
            sHelp = sHelp & "and update them automatically "
            sHelp = sHelp & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            If m_bSiriusInstaller Then
                sHelp = sHelp & "Export Product Options" & Strings.Chr(13) & Strings.Chr(10)
                sHelp = sHelp & "This will export the Head Office product option settings "
                sHelp = sHelp & "to a specified INI file.  "
                sHelp = sHelp & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
            End If

            sHelp = sHelp & "OK" & Strings.Chr(13) & Strings.Chr(10)
            sHelp = sHelp & "This will permanently update the product option values"
            sHelp = sHelp & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

            MessageBox.Show(sHelp, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdHelpClick failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHelpClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ******************************************************'
    '
    ' Name: cmdImport_click
    '
    ' Description: This will import from a text file
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Sub cmdImport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdImport.Click

        Dim sFileName, sLine As String
        Dim fso As Object
        Dim fil1 As FileInfo
        Dim ts As FileStream
        Dim lAnswer As DialogResult
        Dim bFound As Boolean
        Dim vImport(,) As Object
        Dim iLoop2 As Integer
        Dim iNumberOfOptions As Double
        Dim iPos As Integer
        Dim iUpperBound As Integer
        '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        Dim bEnableUniqueDocument As Boolean

        Try

            sLine = "This will update the Options for Head Office."
            sLine = sLine & " However it might remove existing Options if these"
            sLine = sLine & " are not contained in the INI file. " & Strings.Chr(13) & Strings.Chr(10)
            sLine = sLine & "Do you wish to proceed?"

            lAnswer = MessageBox.Show(sLine, "Confirmation", MessageBoxButtons.YesNo)
            If lAnswer = System.Windows.Forms.DialogResult.No Then Exit Sub

            '   Get the fileName
            sFileName = doCommonDialog("Ini File Settings (*.ini)|*.ini", "Please select INI file to read from", True)

            If sFileName = "" Then
                Exit Sub
            End If

            '   This will perform the Reading and Copy into an array
            fso = New Object()

            fil1 = New FileInfo(sFileName)
            'developer guide no. 80
            ts = fil1.Open(FileMode.Open, FileAccess.Read)
            Dim tsWriter As StreamReader = New StreamReader(ts)
            sLine = tsWriter.ReadLine()

            iNumberOfOptions = Conversion.Val(Mid(sLine, sCTotalNumber.Length + 1, sLine.Length - sCTotalNumber.Length))
            iNumberOfOptions -= 1
            ReDim vImport(5, CInt(iNumberOfOptions))

            For iLoop As Double = 0 To iNumberOfOptions


                vImport(1, CInt(iLoop)) = g_iBCHHeadOffice

                '   Puts Option Number into Array
                sLine = tsWriter.ReadLine()
                iPos = 1
                iLoop2 = 1
                Do Until iLoop2 >= sLine.Length
                    If Mid(sLine, iLoop2, 1) = "=" Then
                        iPos = iLoop2
                        iLoop2 = sLine.Length
                    End If
                    iLoop2 += 1
                Loop
                If iPos <= sLine.Length Then

                    vImport(0, CInt(iLoop)) = Mid(sLine, iPos + 1, sLine.Length - iPos)
                End If

                '   Puts Value into Array
                sLine = tsWriter.ReadLine()
                iPos = 1
                iLoop2 = 1
                Do Until iLoop2 >= sLine.Length
                    If Mid(sLine, iLoop2, 1) = "=" Then
                        iPos = iLoop2
                        iLoop2 = sLine.Length
                    End If
                    iLoop2 += 1
                Loop
                If iPos <= sLine.Length Then

                    vImport(2, CInt(iLoop)) = Mid(sLine, iPos + 1, sLine.Length - iPos)
                End If

            Next iLoop

            'developer guide no. 235
            tsWriter.Close()

            If Information.IsArray(m_vProductOptions) Then
                'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                If m_vProductOptions.GetUpperBound(1) >= gPMConstants.SIRHiddenOptions.SIROPTEnableUniqueDocumentReference Then
                    bEnableUniqueDocument = (CStr(m_vProductOptions(2, gPMConstants.SIRHiddenOptions.SIROPTEnableUniqueDocumentReference)) <> "")
                End If
                'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                For iLoop As Double = 0 To m_vProductOptions.GetUpperBound(1)
                    If CInt(m_vProductOptions(1, CInt(iLoop))) = g_iBCHHeadOffice Then
                        bFound = False
                        For iLoop2 = 0 To vImport.GetUpperBound(1)

                            If CInt(vImport(0, iLoop2)) = CInt(m_vProductOptions(0, CInt(iLoop))) Then
                                'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                                'If option SIROPTEnableUniqueDocumentReference is enabled in the existing sytem, enable it in the import file.

                                If iLoop = gPMConstants.SIRHiddenOptions.SIROPTEnableUniqueDocumentReference And bEnableUniqueDocument And CStr(vImport(2, iLoop2)) = "" Then

                                    vImport(2, iLoop2) = "1"
                                End If
                                'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                                bFound = True
                                iLoop2 = vImport.GetUpperBound(1)
                            End If
                        Next iLoop2

                        '   Copy the columns over
                        If Not bFound Then
                            iUpperBound = vImport.GetUpperBound(1) + 1
                            ReDim Preserve vImport(5, iUpperBound)


                            vImport(0, iUpperBound) = m_vProductOptions(0, CInt(iLoop))

                            vImport(1, iUpperBound) = g_iBCHHeadOffice

                            vImport(2, iUpperBound) = ""
                            'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                            'If option SIROPTEnableUniqueDocumentReference is enabled in the existing sytem, enable it in the import file.
                            If iLoop = gPMConstants.SIRHiddenOptions.SIROPTEnableUniqueDocumentReference And bEnableUniqueDocument Then

                                vImport(2, iUpperBound) = "1"
                            Else

                                vImport(2, iUpperBound) = ""
                            End If
                            'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                        End If
                    End If
                Next iLoop
            End If


            ' PW190303 - If the Force Numeric Client Code option
            '            is being enable, need to ensure the unique
            '            number record has been created
            ' PS186
            For iLoop As Double = 0 To vImport.GetUpperBound(1)

                If Conversion.Val(CStr(vImport(0, CInt(iLoop)))) = gPMConstants.SIRHiddenOptions.SIROPTForceNumericClientCode And Conversion.Val(CStr(vImport(2, CInt(iLoop)))) = 1 Then
                    m_lReturn = CreateUniqueNumberForClientCode()
                    Exit For
                End If
            Next

            '   This will update the options in the database
            m_lReturn = CType(updateOptions(vImport, True), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                '   Refresh the grid
                getBusiness()
                'Modified
                'MessageBox.Show("Update successful", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                MessageBox.Show("Update successful", "iSIRProductOptions", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                'Modified
                'MessageBox.Show("Update failed", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                MessageBox.Show("Update failed", "iSIRProductOptions", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CmdImport_click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdImport_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ******************************************************'
    '
    ' Name: cmdExport_click
    '
    ' Description: This will export tables to a text file
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Sub cmdExport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExport.Click

        Dim sFileName As String = ""
        Dim sOutput As String = ""
        Dim iOutputCount As Integer
        'Modified,there is no need for this
        'Dim fso As Object

        Try

            '   Get the filename
            sFileName = doCommonDialog("Ini File Settings (*.ini)|*.ini", "Please select INI file to save to", False)

            If sFileName = "" Then
                Exit Sub
            End If

            iOutputCount = 0
            '   This will output to a ini file
            For iLoop As Integer = 0 To m_vProductOptions.GetUpperBound(1)

                If CStr(m_vProductOptions(2, iLoop)) <> "" Then
                    iOutputCount += 1
                    sOutput = sOutput & sCOption & Conversion.Str(iOutputCount).Trim() & "="
                    sOutput = sOutput & Conversion.Str(m_vProductOptions(0, iLoop)).Trim() & Strings.Chr(13) & Strings.Chr(10)
                    sOutput = sOutput & sCValue & Conversion.Str(iOutputCount).Trim() & "="
                    'Modified
                    'sOutput = sOutput + CDbl(m_vProductOptions(2, iLoop)) + Strings.Chr(13) & Strings.Chr(10)
                    sOutput = sOutput + Conversion.Str(m_vProductOptions(2, iLoop)).Trim() + Strings.Chr(13) & Strings.Chr(10)
                End If
            Next iLoop

            '   This will write the total number at the start
            sOutput = sCTotalNumber & Conversion.Str(iOutputCount).Trim() & Strings.Chr(13) & Strings.Chr(10) & sOutput

            '   Write the file to the selected file
            'Modified,there is no need for this
            'fso = New Object()

            'Check that file exists
            'Modified,it creating problem and StreamWriter will create the file so no need for this
            'If Not File.Exists(sFileName) Then

            '	Dim tempAuxVar As FileStream = New FileStream(sFileName, FileMode.CreateNew)
            'End If

            'Open the file
            'Modified,no need for this
            'fil = New FileInfo(sFileName)
            'developer guide no. 80
            Dim tsFileNameWriter As StreamWriter = New StreamWriter(sFileName)
            tsFileNameWriter.WriteLine(sOutput)

            'developer guide no. 235
            tsFileNameWriter.Close()
            MessageBox.Show("Update successful", "iSIRProductOptions", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdExport_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExport_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ******************************************************'
    '
    ' Name: cmdSet_Click
    '
    ' Description: This occurs if we click the set button
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Sub cmdSet_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSet.Click

        Try

            m_lReturn = CType(SetItems(txtValue.Text.Trim()), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdSet_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSet_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ******************************************************'
    '
    ' Name: updateOptions
    '
    ' Description: This will update the options in the database
    '
    ' History: 12/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Function updateOptions(ByRef r_vProductOptions(,) As Object, ByVal v_bSiriusInstall As Boolean) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            If Not v_bSiriusInstall Then

                lReturn = m_oBusiness.updateProductOptions(r_vResultArray:=r_vProductOptions)
            Else

                lReturn = m_oBusiness.updateMasterOptions(r_vResultArray:=r_vProductOptions)
            End If


            Return lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="updateOptions failed", vApp:=ACApp, vClass:=ACClass, vMethod:="updateOptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: cmdOK_Click
    '
    ' Description: This occurs if we click the OK button
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Try

            m_lReturn = CType(updateOptions(m_vProductOptions, m_bSiriusInstaller), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Update successful", "Product Options", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Update failed", "Product Options", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="err_cmdOK_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="err_cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ******************************************************'
    '
    ' Name: Form_Initialize
    '
    ' Description: This occurs if we initialise the form
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'
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
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRProductOptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            '   Check password is ok and whether Sirius Installer.
            m_lReturn = GetPassword()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AK 260203 - disable this line, as it does not need to raise vb-error

                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    MessageBox.Show(g_sUserAuthorityDenial, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ' PW240303 - if there is an error set error number, so will
                    ' go no further
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                End If
                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iSIRProductOptions.General()

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
            lvwSettings.GridLines = True
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ******************************************************'
    '
    ' Name: Form_Load
    '
    ' Description: This occurs when form is loaded
    '
    ' History: 11/06/2002 SJP - Created.
    '
    '*******************************************************'

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.
        Try

            iPMFunc.ShowFormInTaskBar_Detach()

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            If Not m_bSiriusInstaller Then
                cmdExport.Visible = False
                cmdDisable.Visible = False

                cmdSet.Visible = False
                txtValue.Visible = False
                lblHousekeeping.Visible = False
            End If

            'COMMENT OUT
            '   Get all data
            'm_lReturn& = getBusiness()

            ' Check for errors.
            'If (m_lReturn& <> PMTrue) Then
            '    m_lErrorNumber& = PMFalse
            '    Exit Sub
            'End If

            ' Set full row select and grid lines (it breaks up the white space)
            'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSettings.Handle.ToInt32(), v_vShowRowSelect:=True, v_vShowGridLines:=True), gPMConstants.PMEReturnCode)

            ' {* USER DEFINED CODE (End) *}
            lvwSettings.FullRowSelect = True
            lvwSettings.GridLines = True

            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
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



            If UnloadMode <> 0 Or m_bExit Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'Developer Guide no. 7
                    eventArgs.Cancel = True
                    Cancel = 1

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

            ' Terminate the form control object.
            m_oFormFields.Dispose()

            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            m_vBranches = Nothing
            m_vProductOptions = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: SortListView
    '
    ' Description:
    '
    ' History: 13/03/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function SortListView(ByVal v_iIndex As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Tell it that it's not sorted
            ListViewHelper.SetSortedProperty(lvwSettings, False)

            ' Set the column to sort on
            ListViewHelper.SetSortKeyProperty(lvwSettings, v_iIndex)

            ' Swap the ascending/descending around
            If ListViewHelper.GetSortOrderProperty(lvwSettings) = SortOrder.Ascending Then
                ListViewHelper.SetSortOrderProperty(lvwSettings, SortOrder.Descending)
            Else
                ListViewHelper.SetSortOrderProperty(lvwSettings, SortOrder.Ascending)
            End If

            ' Tell it that it's now sorted
            ListViewHelper.SetSortedProperty(lvwSettings, True)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SortListView Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SortListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub lvwSettings_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSettings.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSettings.Columns(eventArgs.Column)

        ' Sort the data
        m_lReturn = CType(SortListView(v_iIndex:=ColumnHeader.Index + 1 - 1), gPMConstants.PMEReturnCode)

    End Sub

    '*******************************************************'********************
    ' Name: GetPassword()
    '
    ' Description: This will check whether the user
    '               is a Sirius Installer or Client User
    '
    '
    ' History: 11/06/2002 SJP - Created.
    '****************************************************************************
    Private Function GetPassword() As Integer

        Dim result As Integer = 0
        Dim sPassword As String = ""
        Dim bAdminUser As Boolean
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sICCSCodeEntered As String
        Dim sICCSCode As String = String.Empty
        Dim oPasswordForm As frmDetail
        Dim lPwdAction As gPMConstants.PMEReturnCode

        Try

            m_bSiriusInstaller = False

            'Get the password
            'sPassword = InputBox(g_sPasswordMessage, ACApp)
            oPasswordForm = New frmDetail()
            iPMFunc.CenterForm(oPasswordForm)

            oPasswordForm.ShowDialog()
            sPassword = oPasswordForm.txtPassword.Text
            lPwdAction = oPasswordForm.Status

            oPasswordForm.Close()
            oPasswordForm = Nothing

            If lPwdAction = gPMConstants.PMEReturnCode.PMCancel Then
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            End If

            '   This will check the password entered
            If sPassword.Length > g_sPasswordSearchString.Length Then
                If Mid(sPassword, 1, 6) = g_sPasswordSearchString Then

                    '   This will get the ICCS Number from the database

                    lResult = m_oBusiness.getICCSCode(r_vICCSCode:=sICCSCode)

                    '   Could not find ICCS Number which we should have
                    If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMError
                    End If

                    sICCSCodeEntered = Mid(sPassword, g_sPasswordSearchString.Length + 1, sPassword.Length - g_sPasswordSearchString.Length).Trim()

                    '   This will compare the value entered with the other value
                    If sICCSCode = sICCSCodeEntered Then
                        m_bSiriusInstaller = True
                    End If

                End If
            End If

            '   User is NOT Sirius Configurator therefore check whether admin user
            If Not m_bSiriusInstaller Then

                '   Check whether an admin user.

                lResult = m_oBusiness.isUserAdmin(v_vUserId:=g_oObjectManager.UserID, r_bAdminUser:=bAdminUser)

                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lResult
                ElseIf Not bAdminUser Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPassword failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPassword", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function






    '*****************************************************************************
    ' Name: CreateUniqueNumberForClientCode (private)
    '
    ' Description: This function calls the business function to create a record
    '              on the unique number table (if it doesn't already exist), for
    '              the Force Numeric Client Code option
    '
    ' History: PW190303 - Created (PS186).
    '*****************************************************************************
    Private Function CreateUniqueNumberForClientCode() As Integer

        Dim result As Integer = 0
        Dim lClientStartNumber As Integer

        Try

            ' The Force Numeric Client Code product option has been created
            ' for IAG who have requested this
            ' functionality. They want the Numeric Client Code to start at
            ' 100,000,000. If other customers require this functionality in the
            ' future, it is likely they will want the client code to start at
            ' a different number. Therefore, rather than creating a data script
            ' that runs on install to create the Unique Number record starting at
            ' 100,000,000, it will be done here when the option is selected. This
            ' means that if/when another customer requests the functionality, we
            ' can put a dialogue box in here, to allow the start number to be
            ' entered. (Can't check the IsNRMA product option, 'cos if this is
            ' being done on first import of .INI file, will not be set yet.)
            lClientStartNumber = 100000000

            ' Create the unique number record

            m_lReturn = m_oBusiness.CreateUniqueNumberForClientCode(lClientStartNumber:=lClientStartNumber)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Error creating the Unique Number record for the " & "Force Numeric Client Code option.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateUniqueNumberForClientCode failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateUniqueNumberForClientCode", excep:=excep)

            Return result
        End Try
    End Function

    Private Sub txtValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtValue.Leave
        If txtValue.Text.Trim() <> "1" Then
            txtValue.Text = ""
        End If
    End Sub

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.P Then
            tabMain.SelectedIndex = 0
        End If
    End Sub
End Class
