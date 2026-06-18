Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 10/05/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '
    'Developer Guide No. 69
    Public frmDetail As frmDetail

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

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

    Private m_sStepStatus As String = ""

    ' {* USER DEFINED CODE (Begin) *}

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPolicyNumMaint.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    Private m_vResultArray(,) As Object

    Private m_vArray As Object
    Private m_iActionArray() As Integer
    Private Const m_iACTION_ADD As Integer = 1
    Private Const m_iACTION_UPDATE As Integer = 2

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    Private m_sDelSchemesMsg As String = ""
    Private m_SUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""



    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property StepStatus() As String
        Get
            Return m_sStepStatus
        End Get
    End Property

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

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

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
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' ***** Mandatory *****************************************

            '    m_lReturn& = m_oFormFields.AddNewFormField( _
            ''        ctlControl:=txtCode, _
            ''        lFormat:=PMFormatString, _
            ''        lFieldType:=PMString, _
            ''        lMandatory:=PMMandatory)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If


            ' ***** Non-Mandatory *************************************

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
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'We already have all the details we need but we need to set it up in
            'the business object else the deletion won't work.

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.SearchAll(r_vResultArray:=m_vResultArray)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            'Dimension action array which be used to flag the necessary database action
            'required on each record when we exit.
            If Information.IsArray(m_vResultArray) Then
                ReDim m_iActionArray(m_vResultArray.GetUpperBound(1))
            End If

            Return result

        Catch excep As System.Exception




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
        Dim oListItem As ListViewItem

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

            'Populate ListView from results array retrieved from database.
            lvwSchemes.Items.Clear()
            If Information.IsArray(m_vResultArray) Then
                For iRecord As Integer = m_vResultArray.GetLowerBound(1) To m_vResultArray.GetUpperBound(1)

                    'Developer Guide No. 49
                    oListItem = lvwSchemes.Items.Add(CStr(m_vResultArray(0, iRecord)), "Textfile")
                    UpdateListItem(oListItem, iRecord)

                    If CDbl(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_DELETED, iRecord)) <> 0 Then 'Deleted show in Grayed

                        lvwSchemes.Items(iRecord).ForeColor = Color.Gray

                    End If
                   

                Next iRecord

                oListItem = Nothing
            End If

            Return result

        Catch excep As System.Exception




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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            'RWH - already done
            '    m_lReturn& = InterfaceToData()
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_SUniqueId = GetUniqueID()
            m_lReturn = m_oBusiness.Writeall(v_vArray:=m_vResultArray, v_iActionArray:=m_iActionArray, v_sUniqueId:=m_SUniqueId)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
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
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try


            ' Get the lookup values.

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

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




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


            ' Assign the details to the data storage.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




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
    'UPGRADE_NOTE: (7001) The following declaration (InterfaceToData) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function InterfaceToData() As Integer
    '
    'Dim result As Integer = 0
    'Dim iSchemeCount, iArrayCount As Integer
    'Dim oListItem As ListViewItem
    '
    'Try 
    '******************************************************************
    '** THE DATA IS KEPT UPDATED BEHIND THE SCENES THROUGHOUT, AS ONLY
    '** A PRECIS OF THE DATA IS SHOWN IN THE LIST.
    '******************************************************************
    '
    ' Update the data storage.
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    ' ************************************************************
    ' Enter your code here to assign all of the details from the
    ' interface to the data storage.
    ''
    ' Example:-
    ''
    '    m_DName$ = trim$(txtName.Text)
    '    m_DDate = CDate(txtDate.Text)
    '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
    '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
    ''
    ' NOTE: Replace this section with your new code.
    ' ************************************************************
    '
    ' {* USER DEFINED CODE (End) *}
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
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            '    CenterForm Me

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSchemes.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            cmdSchemeEdit.Enabled = False
            cmdSchemeDelete.Enabled = False '68780

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

            '    Set m_ctlTabFirstLast(ACControlStart, 0) = OLE1
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = OLE1

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




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
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            'Developer Guide No. 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            'Developer Guide No. 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            cmdSchemeEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            cmdSchemeAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            fraSchemes.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFraSchemes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PRIVATE Methods (End)
    ' PRIVATE Events (Begin)

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

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

                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, MainModule.MainScreenHelpID), gPMConstants.PMEReturnCode)


    End Sub

    Private Sub cmdSchemeAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSchemeAdd.Click

        Dim oListItem As ListViewItem

        'Developer Guide No. code added
        frmDetail = New frmDetail()

        Try

            frmDetail.ExistingNumberingSchemes = VB6.CopyArray(m_vResultArray)


            frmDetail.Task = gPMConstants.PMEComponentAction.PMAdd


            ClearDetailScreen()

            frmDetail.ShowDialog()


            If frmDetail.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                'Increase size of results array to accommodate new Scheme.
                If Information.IsArray(m_vResultArray) Then
                    'Add new record to array to contain new scheme.
                    ReDim Preserve m_vResultArray(m_vResultArray.GetUpperBound(0), m_vResultArray.GetUpperBound(1) + 1)
                Else
                    ReDim m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_NUMBER_OF_DATA_FIELDS - 1, 0)
                End If

                If UpdateArrayRecord(m_vResultArray.GetUpperBound(1)) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                'If data array has updated successfully then update action array.
                If Information.IsArray(m_iActionArray) Then
                    ReDim Preserve m_iActionArray(m_vResultArray.GetUpperBound(1))
                Else
                    ReDim m_iActionArray(0)
                End If
                m_iActionArray(m_iActionArray.GetUpperBound(0)) = m_iACTION_ADD

                'Display new record in ListView.

                oListItem = lvwSchemes.Items.Add(CStr(m_vResultArray(0, m_vResultArray.GetUpperBound(1))), "Textfile")
                UpdateListItem(oListItem, m_vResultArray.GetUpperBound(1))

            End If

            frmDetail = Nothing


        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Client Add command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSchemeAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdSchemeDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSchemeDelete.Click
        Dim bPartyCodeSchemeAlreadyExist As Boolean

        Try

            'Test whether function is to set or cancel flag to delete scheme.
            If cmdSchemeDelete.Text = "&Delete" Then
                'Developer Guide No.  edited code as per vb6

                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_DELETED, lvwSchemes.FocusedItem.Index) = 1
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_DELETED_TEMP, lvwSchemes.FocusedItem.Index) = 1
                'Flag record to be updated on the database
                m_iActionArray(lvwSchemes.FocusedItem.Index) = m_iACTION_UPDATE

                lvwSchemes.FocusedItem.ForeColor = Color.Gray
                cmdSchemeDelete.Text = "Un&delete"
                cmdSchemeEdit.Enabled = False
            Else
                'Note: DO NOT clear update flag in case record has been edited
                'with required updates and then been deleted/undeleted by mistake.
                'party_code
                If CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_PARTY_TYPE_ID, lvwSchemes.FocusedItem.Index + 1 - 1)) <> "0" Then
                    For i As Integer = 0 To m_vResultArray.GetUpperBound(1)
                        'Developer Guide No. edited code as per vb6
                        If gPMFunctions.ToSafeString(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_PARTY_TYPE_ID, i)) = CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_PARTY_TYPE_ID, lvwSchemes.FocusedItem.Index)) And CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_DELETED, i)) = "0" Then
                            bPartyCodeSchemeAlreadyExist = True
                            MessageBox.Show("This Party Type Scheme already exists.", g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Exit For
                        End If
                    Next
                End If
                'PN5082-End
                If Not bPartyCodeSchemeAlreadyExist Then
                    m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_DELETED, lvwSchemes.FocusedItem.Index + 1 - 1) = 0
                    m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_DELETED_TEMP, lvwSchemes.FocusedItem.Index + 1 - 1) = 0
                    m_iActionArray(lvwSchemes.FocusedItem.Index + 1 - 1) = m_iACTION_UPDATE

                    lvwSchemes.FocusedItem.ForeColor = Color.Black
                    cmdSchemeDelete.Text = "&Delete"
                    cmdSchemeEdit.Enabled = True
                End If
            End If

            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                m_lReturn = CType(CheckNumberingSchemeReadOnly(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Scheme Delete command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSchemeDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdSchemeEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSchemeEdit.Click

        Dim oListItem As ListViewItem
        Dim iScheme As Integer

        Try

            '    PassExistingSchemeCodesToDetailScreen True

            frmDetail = New frmDetail
            frmDetail.ExistingNumberingSchemes = VB6.CopyArray(m_vResultArray)

            PopulateDetailScreen()

            frmDetail.Task = gPMConstants.PMEComponentAction.PMEdit
            frmDetail.ShowDialog()

            If frmDetail.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                oListItem = lvwSchemes.FocusedItem
                iScheme = lvwSchemes.FocusedItem.Index + 1 - 1
                'Check
                If CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME_ID, iScheme)) <> oListItem.Text Then
                    For iScheme = 0 To m_vResultArray.GetUpperBound(1) - 1
                        If CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME_ID, iScheme)) = oListItem.Text Then
                            Exit For
                        End If
                    Next iScheme
                End If

                If UpdateArrayRecord(iScheme) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                'If data array has updated successfully then update action array.
                If CDbl(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME_ID, iScheme)) = 0 Then
                    m_iActionArray(iScheme) = m_iACTION_ADD
                Else
                    m_iActionArray(iScheme) = m_iACTION_UPDATE
                End If

                'Refresh Scheme in ListView.
                UpdateListItem(oListItem, iScheme)

                If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                    m_lReturn = CType(CheckNumberingSchemeReadOnly(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                End If

            End If

            frmDetail = Nothing

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Scheme Edit command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSchemeEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Warn user if schemes have been selected for deletion.
            If CheckForSchemesToDelete() = gPMConstants.PMEReturnCode.PMTrue Then
                '        If MsgBox("Are you sure you wish to delete the greyed Schemes", vbYesNo + vbExclamation, "") = vbNo Then
                If MessageBox.Show(m_sDelSchemesMsg, g_sMsgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                ' Everything OK, so we can hide the interface.
                Me.Hide()
                'Me.Close()
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'Developer Guide No. 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No. 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBPolicyNumMaint.General()

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

        Catch
            Exit Sub
        End Try


    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            iPMFunc.ShowFormInTaskBar_Detach()

            If GetMessages() <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

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
            ' {* USER DEFINED CODE (End) *}

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

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                cmdSchemeAdd.Enabled = False
                cmdSchemeDelete.Enabled = False
                cmdSchemeEdit.Enabled = False
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private Const vbFormCode As Integer = 0

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
                    '        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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



    Private Sub lvwSchemes_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSchemes.DoubleClick

        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then

            If Not lvwSchemes.FocusedItem.ForeColor.Equals(Color.Gray) Then
                cmdSchemeEdit_Click(cmdSchemeEdit, New EventArgs())
            End If
        End If
    End Sub

    Private Sub lvwSchemes_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSchemes.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            'Check to see if item is ghosted, that is selected for deletion,
            'before enabling Edit button and setting caption on delete button.
            cmdSchemeDelete.Enabled = True '68780


            If lvwSchemes.FocusedItem.ForeColor.Equals(Color.Gray) Then
                cmdSchemeDelete.Text = "Un&delete"
                cmdSchemeEdit.Enabled = False
            Else
                cmdSchemeDelete.Text = "&Delete"
                cmdSchemeEdit.Enabled = True
            End If

            m_lReturn = CType(CheckNumberingSchemeReadOnly(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        End If

    End Sub

    Private Sub lvwSchemes_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSchemes.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        'Developer Guide No 70

        If lvwSchemes.GetItemAt(eventArgs.X, eventArgs.Y) Is Nothing Then
            ' Nothing selected
            cmdSchemeEdit.Enabled = False
            cmdSchemeDelete.Enabled = False '68780
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                'Check to see if item is ghosted, that is selected for deletion,
                'before enabling Edit button and setting caption on delete button.
                cmdSchemeDelete.Enabled = True '68780

                If lvwSchemes.FocusedItem.ForeColor.Equals(Color.Gray) Then
                    cmdSchemeDelete.Text = "Un&delete"
                    cmdSchemeEdit.Enabled = False
                Else
                    cmdSchemeDelete.Text = "&Delete"
                    cmdSchemeEdit.Enabled = True
                End If

                m_lReturn = CType(CheckNumberingSchemeReadOnly(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

            End If
        End If

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
        '
        '
        'tabMainTabPreviousTab = tabMainTab.SelectedIndex
        'End Try

    End Sub

    ' PRIVATE Events (End)



    Public Sub PopulateDetailScreen()

        Dim iSelectedScheme As Integer

        Try

            iSelectedScheme = lvwSchemes.FocusedItem.Index + 1

            With frmDetail

                .SchemeId = CInt(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME_ID, iSelectedScheme - 1))
                .Scheme = CInt(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME, iSelectedScheme - 1))
                .BusinessType = CInt(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_TYPE, iSelectedScheme - 1))
                .Code = CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_CODE, iSelectedScheme - 1))
                .Description = CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_DESCRIPTION, iSelectedScheme - 1))
                .FixedCode = CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_FIXED_CODE, iSelectedScheme - 1))
                .Generated = CBool(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_GENERATED, iSelectedScheme - 1))
                .HighestNumber = CInt(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_HIGHEST_NUMBER, iSelectedScheme - 1))
                .Mask = CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_MASK_CODE, iSelectedScheme - 1))
                .NextNumber = CInt(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_NEXT_NUMBER, iSelectedScheme - 1))
                .NextNumberToAllocate = CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_TYPE, iSelectedScheme - 1))
                .Reuse = CBool(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_REUSE, iSelectedScheme - 1))
                .Step_Renamed = CInt(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_STEP, iSelectedScheme - 1))
                .PartyTypeID = CInt(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_PARTY_TYPE_ID, iSelectedScheme - 1))
                .IsReadOnly = CInt(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_READ_ONLY, iSelectedScheme - 1))

                'Start - Prakash - (WPR87 Paralleling)
                'IsResetNumberDaily was using enuNSF_NUMBER_OF_DATA_FIELDS which is not correct. Changing to
                If CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_RESET_DAILY, iSelectedScheme - 1)) = "" Or CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_RESET_DAILY, iSelectedScheme - 1)) = "0" Then
                    .IsResetNumberDaily = False
                ElseIf (CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_RESET_DAILY, iSelectedScheme - 1)) = "1") Then
                    .IsResetNumberDaily = True
                End If

                If CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_RESET_NUMBER, iSelectedScheme - 1)) = "" Or CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_RESET_NUMBER, iSelectedScheme - 1)) = "0" Then
                    .ResetNumber = False
                ElseIf (CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_RESET_NUMBER, iSelectedScheme - 1)) = "1") Then
                    .ResetNumber = True
                End If
                'End - Prakash - (WPR87 Paralleling)

            End With

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to pass Numbering Scheme data to Details screen", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateDetailScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Function UpdateArrayRecord(ByVal iRecord As Integer) As Integer
        'RWH(20/06/2000) Function to update a specified record in the Results Array with
        'the values entered on the Detail screen.

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With frmDetail

                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME_ID, iRecord) = .SchemeId
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_DESCRIPTION, iRecord) = .Description.Trim()
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_EFFECTIVE_DATE, iRecord) = .EffectiveDate
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_TYPE, iRecord) = .BusinessType
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_TYPE_DESCRIPTION, iRecord) = .BusinessTypeDescription
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME, iRecord) = .Scheme
                If .Generated Then
                    m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_GENERATED, iRecord) = 1
                Else
                    m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_GENERATED, iRecord) = 0
                End If
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_MASK_CODE, iRecord) = .Mask.Trim()
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_FIXED_CODE, iRecord) = .FixedCode.Trim()
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_NEXT_NUMBER, iRecord) = .NextNumber
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_HIGHEST_NUMBER, iRecord) = .HighestNumber
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_STEP, iRecord) = .Step_Renamed
                If .Reuse Then
                    m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_REUSE, iRecord) = 1
                Else
                    m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_REUSE, iRecord) = 0
                End If
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_CODE, iRecord) = .Code

                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_READ_ONLY, iRecord) = .IsReadOnly
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_PARTY_TYPE_ID, iRecord) = .PartyTypeID
                '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
                m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_RESET_DAILY, iRecord) = .IsResetNumberDaily
                '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
                'Start - Renuka - (WPR87 Paralleling)
                If .ResetNumber Then
                    m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_RESET_NUMBER, iRecord) = 1
                Else
                    m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_RESET_NUMBER, iRecord) = 0
                End If
                'End - Renuka - (WPR87 Paralleling)
            End With
            'Temp defaults
            m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_CAPTION_ID, iRecord) = 1
            m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_EFFECTIVE_DATE, iRecord) = DateTime.Now
            m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_DELETED, iRecord) = 0

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update array with Scheme details", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateArrayRecord", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub UpdateListItem(ByRef oListItem As ListViewItem, ByRef iRecord As Integer)

        Try

            With oListItem
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_DESCRIPTION, iRecord))
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_MASK_CODE, iRecord))
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_FIXED_CODE, iRecord))
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME, iRecord))
                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_TYPE_DESCRIPTION, iRecord))

                .Tag = CStr(iRecord)
            End With

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update List Item with Scheme details", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateListItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Function CheckForSchemesToDelete() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            For iScheme As Integer = 0 To (m_vResultArray.GetUpperBound(1))
                'If m_vResultArray(enuNSF_IS_DELETED, iScheme) = 1 Then
                If CDbl(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_DELETED_TEMP, iScheme)) = 1 Then
                    result = gPMConstants.PMEReturnCode.PMTrue
                    Exit For
                End If
            Next iScheme

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed while checking for scheme to delete", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForSchemesToDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub ClearDetailScreen()

        Try

            With frmDetail
                .Generated = True
                .cboBusinessType.ListIndex = 0
                .txtScheme.Text = ""
                .txtMask.Text = ""
                .txtFixedCode.Text = ""
                .txtNextNo.Text = ""
                .txtHighestNo.Text = ""
                .txtStep.Text = ""
                .chkReuse.CheckState = CheckState.Unchecked
                .optGenVal(0).Checked = 1
                .optGenVal(1).Checked = 0
                .chkIsResetDaily.CheckState = CheckState.Unchecked

            End With

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed clearing detail screen", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearDetailScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private Function GetMessages() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Developer Guide No. 243
            g_sMsgBoxTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMsgBoxTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            m_sDelSchemesMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDelSchemes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            g_sCancelMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Messages from resource file", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMessages", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CheckNumberingSchemeReadOnly() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckNumberingSchemeReadOnly"
        Dim iFindRecord As Integer



        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            If Information.IsArray(m_vResultArray) And gPMFunctions.ToSafeString(lvwSchemes.FocusedItem) <> "" Then

                For iRecord As Integer = m_vResultArray.GetLowerBound(1) To m_vResultArray.GetUpperBound(1)
                    If CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME_ID, iRecord)) = lvwSchemes.FocusedItem.Text Then
                        iFindRecord = iRecord
                        Exit For
                    End If
                Next iRecord

                If CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_DELETED, iFindRecord)) = "0" And CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_READ_ONLY, iFindRecord)) = "1" And CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_TYPE_DESCRIPTION, iFindRecord)).Trim().ToUpper() = "PARTY CODE" And CStr(m_vResultArray(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME_ID, iFindRecord)) <> "" And iFindRecord > 0 Then

                    cmdSchemeDelete.Enabled = False
                Else
                    cmdSchemeDelete.Enabled = True
                End If
            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally





        End Try
        Return result
    End Function

    Private Sub tabMainTab_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMainTab.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub

    Private Sub tabMainTab_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tabMainTab.KeyPress
        'Developer Guide No 293
        'If e.Alt And EventArgs.KeyCode = Keys.D1 Then
        '    tabMainTab.SelectedIndex = 0
        'End If
    End Sub
End Class
