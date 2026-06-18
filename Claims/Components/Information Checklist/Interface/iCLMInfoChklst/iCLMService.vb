Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

'Developer Guide No.: 129
Imports SharedFiles


Friend Partial Class frmService
	Inherits System.Windows.Forms.Form
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmService"
	
	' Declare an instance of the FormControl object
	Private m_oFormFields As iPMFormControl.FormFields
	'Private variable to store the property values
	Private m_lMode As Integer
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lErrorNumber As Integer
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	'CJB 151003 PN7501 - Cater for biger claim id's than an integer can hold!
	Private m_lClaim As Integer
	
	Private m_iClmPrtyId As Integer
	
	Private m_sService As String = ""
	Private m_sReference As String = ""
    'developer guide no.(As per VB Code)
    'start
    Private m_vDateReq As Object
    Private m_vTimeReq As Object
    Private m_vDateCrit As Object
    Private m_vTimeCrit As Object
    Private m_vDateRecv As Object
    Private m_vTimeRecv As Object
    'end
	Private m_sContact As String = ""
	Private m_sDesc As String = ""
	Private m_lPrtyClmId As Integer
	Private m_lClaimID As Integer
	
	Const PMKeyNameOperateMode As String = "claim_mode"
	Const PMKeyNameClaimCnt As String = "claim_cnt"
	Const PMKeyNamePartyID As String = "party_claim_id"
	Const PMKeyNamePartyTypeID As String = "claim_party_type_id"
	Const PMKeyNamePartyName As String = "claim_party_name"
	Const PMKeyNamePartyAddress As String = "claim_party_address"
	Const PMKeyNamePartyPhoneNumber As String = "claim_party_phone_number"
	
	'AJM (25/07/2001) - pass in transaction type
	Private m_iTask As gPMConstants.PMEComponentAction


	'AJM (25/07/2001) - pass in transaction type
	Public Property Task() As Integer
		Get
			
			Return m_iTask
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iTask = Value
			
		End Set
	End Property
	
	Public Property Desc() As String
		Get
			
			Return m_sDesc
			
		End Get
		Set(ByVal Value As String)
			
			m_sDesc = Value
			
		End Set
	End Property
	
	Public Property Service() As String
		Get
			Return m_sService
		End Get
		Set(ByVal Value As String)
			m_sService = Value
		End Set
	End Property
	Public Property Contact() As String
		Get
			
			Return m_sContact
			
		End Get
		Set(ByVal Value As String)
			
			m_sContact = Value
			
		End Set
	End Property
	
	
	Public Property Reference() As String
		Get
			
			Return m_sReference
			
		End Get
		Set(ByVal Value As String)
			
			m_sReference = Value
			
		End Set
	End Property
    'developer guide no.101
    Public Property DateReq() As Object
        Get

            Return m_vDateReq

        End Get
        Set(ByVal Value As Object)


            m_vDateReq = Value

        End Set
    End Property

    Public Property PrtyClmId() As Integer
        Get

            Return m_lPrtyClmId

        End Get
        Set(ByVal Value As Integer)

            m_lPrtyClmId = Value

        End Set
    End Property
    Public Property ClaimId() As Integer
        Get

            Return m_lClaimID

        End Get
        Set(ByVal Value As Integer)

            m_lClaimID = Value

        End Set
    End Property
    'developer guide no.101
    Public Property TimeReq() As Object
        Get
            Return m_vTimeReq
        End Get
        Set(ByVal Value As Object)


            m_vTimeReq = Value

        End Set
    End Property

    'developer guide no.101
    Public Property DateRecv() As Object
        Get

            Return m_vDateRecv

        End Get
        Set(ByVal Value As Object)


            m_vDateRecv = Value

        End Set
    End Property

    'developer guide no.101
    Public Property TimeRecv() As Object
        Get

            Return m_vTimeRecv

        End Get
        Set(ByVal Value As Object)


            m_vTimeRecv = Value

        End Set
    End Property

    'developer guide no.101
    Public Property TimeCrit() As Object
        Get

            Return m_vTimeCrit

        End Get
        Set(ByVal Value As Object)


            m_vTimeCrit = Value

        End Set
    End Property

    'developer guide no.101
    Public Property DateCrit() As Object
        Get

            Return m_vDateCrit

        End Get
        Set(ByVal Value As Object)


            m_vDateCrit = Value

        End Set
    End Property


    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '

    'Private Function SetFirstLastControls() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Initialise the control array with the number of
    ' tabs which contain data entry fields on (Remember
    ' that arrays start from zero, therefore you must
    ' subtract one from the number of tabs).
    'Dim m_ctlTabFirstLast(1, 0) As Object
    '
    'm_ctlTabFirstLast(ACControlStart, 0) = txtRequirement
    'm_ctlTabFirstLast(ACControlEnd, 0) = txtDescription
    '
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function




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
            'AJM (25/07/2001) - Only use 'Add Requirement' caption when adding
            If Task = gPMConstants.PMEComponentAction.PMAdd Then

                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddServiceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else

                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditServiceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-


            lblRequirement.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACServicelbl, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '       lblReference.Caption = iPMFunc.GetResData( _
            ''            iLangID:=g_iLanguageID%, _
            ''            lID:=ACReference, _
            ''            iDataType:=PMResString)


            lblDateReq.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDateRequested, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDateRecv.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDateReceived, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDateCrit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDateCritical, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblContact.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACContactlbl, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDesc.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdParty.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartyButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


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

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        '**************Start of Code Changes Bug id 28

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel


        Dim sTitle As String = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


        Dim sMessage As String = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        Dim iMsgResult As DialogResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        ' Check message result.
        If iMsgResult = System.Windows.Forms.DialogResult.No Then
            ' Set return to PMFalse, meaning
            ' don't cancel.
            Exit Sub
        End If
        '**************end  of Code Changes Bug id 28

        Me.Hide()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim bForceLostFocus As Boolean = iPMFunc.ForceLostFocus(cmdOK)

        'DoEvents
        If Not bForceLostFocus Then
            '       txtExchangeRate.SetFocus
            Exit Sub
        End If


        Dim bCheckMandatory As Boolean = CheckMandatory()

        If Not bCheckMandatory Then
            Exit Sub
        End If

        'DC100703 -ISS5314 -Date/Time Received
        If g_dtDateRecv <> "" And g_dtTimeRecv = "" Then
            g_dtTimeRecv = StringsHelper.Format(DateTimeHelper.Time, ACTimeConversion)
        End If

        If Information.IsDate(g_dtDateReqt) And Information.IsDate(g_dtDateRecv) And IsTime(g_dtTimeReqt) And IsTime(g_dtTimeRecv) Then
            If (DateAndTime.DateDiff("d", CDate(g_dtDateReqt), CDate(g_dtDateRecv), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) = 0) And (DateAndTime.DateDiff("s", CDate(g_dtTimeReqt), CDate(g_dtTimeRecv), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0) Then
                MessageBox.Show("Requested Date must not be later than Received Date", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        If Information.IsDate(g_dtDateReqt) And Information.IsDate(g_dtDateCrit) And IsTime(g_dtTimeReqt) And IsTime(g_dtTimeCrit) Then
            If (DateAndTime.DateDiff("d", CDate(g_dtDateReqt), CDate(g_dtDateCrit), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) = 0) And (DateAndTime.DateDiff("s", CDate(g_dtTimeReqt), CDate(g_dtTimeCrit), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0) Then
                MessageBox.Show("Requested Date must not be later than Critical Date", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        'Traping changes made for updating Event
        If Me.Service <> txtRequirement.Text Then
            m_ofrmInterface.EventInfo = True
        End If
        If Me.Reference <> txtReference.Text Then
            m_ofrmInterface.EventInfo = True
        End If
        If Me.DateReq <> Format(g_dtDateReqt, "short date") Then
            m_ofrmInterface.EventInfo = True
        End If
        If Me.TimeReq <> g_dtTimeReqt Then
            m_ofrmInterface.EventInfo = True
        End If
        If Me.DateCrit <> Format(g_dtDateCrit, "short date") Then
            m_ofrmInterface.EventInfo = True
        End If
        If Me.TimeCrit <> g_dtTimeCrit Then
            m_ofrmInterface.EventInfo = True
        End If
        If Me.DateRecv <> Format(g_dtDateRecv, "short date") Then
            m_ofrmInterface.EventInfo = True
        End If
        If Me.TimeRecv <> g_dtTimeRecv Then
            m_ofrmInterface.EventInfo = True
        End If
        If Me.Contact <> txtContact.Text Then
            m_ofrmInterface.EventInfo = True
        End If
        If Me.Desc <> txtDescription.Text Then
            m_ofrmInterface.EventInfo = True
        End If
        'Before unloading the form
        'set the control values to the form properties
        Me.Service = txtRequirement.Text
        '            frmService.PrtyClmId = 0  'as nothing is being passed for
        '                                            'requirement screen
        Me.Reference = txtReference.Text
        '            frmService.DateReq = Format(g_dtDateReqt, "short date")
        '            frmService.TimeReq = txtTimeRequested
        '            frmService.DateCrit = Format(g_dtDateCrit, "short date")
        '            frmService.TimeCrit = txtTimeCritical
        '            frmService.DateRecv = Format(g_dtDateRecv, "short date")
        '            frmService.TimeRecv = txtTimeReceived


        Me.DateReq = Format(g_dtDateReqt, "short date")
        Me.TimeReq = g_dtTimeReqt
        Me.DateCrit = Format(g_dtDateCrit, "short date")
        Me.TimeCrit = g_dtTimeCrit
        Me.DateRecv = Format(g_dtDateRecv, "short date")
        Me.TimeRecv = g_dtTimeRecv

        Me.Contact = txtContact.Text
        Me.Desc = txtDescription.Text

        Me.Hide()


    End Sub

    Private Sub cmdParty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdParty.Click
        Dim FindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object

        'CJB 151003 PN7501 - Cater for biger claim id's than an integer can hold!
        m_lClaim = ClaimId

        m_iClmPrtyId = 0

        'ISS1368 and ISS1598
        'DN 18/12/02 - Change to use the Sirius Find Party for both Underwriting and Broking
        Dim temp_FindParty As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_FindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        FindParty = temp_FindParty

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        ' Assign the key array with the parameters
        ReDim vKeyArray(1, 0)

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "special_party"

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = "OT"


        m_lReturn = FindParty.SetKeys(vKeyArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            FindParty = Nothing
            Exit Sub
        End If

        vKeyArray = Nothing


        FindParty.NotEditable = 1


        FindParty.IgnoreDriversAndWitnesses = True


        FindParty.Start()

        'Add the PartyId & Party Details to the Party Listview

        If FindParty.Status <> gPMConstants.PMEReturnCode.PMOK Then

            FindParty.Dispose()
            FindParty = Nothing
            Exit Sub
        End If


        PrtyClmId = FindParty.PartyCnt

        txtReference.Text = FindParty.LongName


        FindParty.Dispose()
        FindParty = Nothing


    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Select the first otem in the List View
            ' lvwInformationChecklist.ListItems.Item(1).Selected = True

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub frmService_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

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

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Function ValidateDates() As Integer
        'Code to Validate Dates for Requested,Received and Critical
    End Function

    Public Function GetServiceDetails() As Integer

    End Function
    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory() As Boolean

        Dim result As Boolean = False
        Try


            If txtRequirement.Text.Trim() = "" Then ' Requirement text box
                '        Call DisplayMessage(ACMandatoryFieldMsg, Mid(lblRequirement.Name, 4))
                DisplayMessage(ACMandatoryFieldMsg, lblRequirement.Text.Substring(0, 7))
                Return False
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If

            If txtReference.Text.Trim() = "" Then ' Requirement text box
                DisplayMessage(ACMandatoryFieldMsg, Mid(cmdParty.Name, 4))
                Return False
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If

            If txtDateRequested.Text.Trim() = "" Then ' Requirement text box
                DisplayMessage(ACMandatoryFieldMsg, Mid(lblDateReq.Name, 4))
                Return False
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                result = True
            End If


            'TN20010425 Start
            '    If Trim(txtContact.Text) = "" Then        ' Requirement text box
            '        Call DisplayMessage(ACMandatoryFieldMsg, Mid(lblContact.Name, 4))
            '        CheckMandatory = False
            '        Exit Function
            '    Else
            '    '   If all the Mandatory fields are having values SET the CheckMandatory = True
            '        CheckMandatory = True
            '    End If
            '
            '    If Trim(txtDescription.Text) = "" Then        ' Requirement text box
            '        Call DisplayMessage(ACMandatoryFieldMsg, Mid(lblDesc.Name, 4))
            '        CheckMandatory = False
            '        Exit Function
            '    Else
            '    '   If all the Mandatory fields are having values SET the CheckMandatory = True
            '        CheckMandatory = True
            '    End If
            'TN20010425 End

            If txtTimeRequested.Text.Trim() = "" Then ' Requirement text box
                DisplayMessage(ACMandatoryFieldMsg, Mid(txtTimeRequested.Name, 4))
                Return False
            Else
                '   If all the Mandatory fields are having values SET the CheckMandatory = True
                Return True
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for Mandatory Fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name:         DisplayMessage
    '
    ' Description:  This function is used to display he Error Messages for this Form.
    '               We are passing two parameters MessageCount which is the
    '               Constant defined in the Resource file
    '                The Title is the Error Message Text for the same.
    '
    ' ***************************************************************** '

    Private Sub DisplayMessage(ByRef MessageConstant As Integer, ByRef sTitle As String)

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.


            sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display the status message.

            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' Date :15/07/2000
    '
    ' Edit History : Pandu
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            'SK-might need later if combo is required
            '    'Load Details of Recovery Type in the combo
            '    Call LoadDataInCombo(cboRecoveryType, g_vLookupArray, vTableArray(2, 0), vTableArray(3, 0))

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                txtContact.Enabled = False
                txtDateCritical.Enabled = False
                txtDateReceived.Enabled = False
                txtDateRequested.Enabled = False
                txtDescription.Enabled = False
                txtReference.Enabled = False
                txtRequirement.Enabled = False
                txtTimeCritical.Enabled = False
                txtTimeReceived.Enabled = False
                txtTimeRequested.Enabled = False
                cmdParty.Enabled = False
            End If

            If g_bAddExpSer Then
                'Setting Default Values to Date Field
                g_dtDateReqt = StringsHelper.Format(DateTime.Today, ACDateConversion)
                g_dtDateCrit = StringsHelper.Format(DateTime.Today, ACDateConversion)
                'DC200201 do not set ot todays date
                'g_dtDateRecv = Format(Date, ACDateConversion)
                'AJM (25/07/2001) - do not default a date but if one has been entered, show it!
                If Me.DateRecv = "" Then
                    g_dtDateRecv = ""
                Else
                    g_dtDateRecv = StringsHelper.Format(Me.DateRecv, ACDateConversion)
                End If

                '    g_dtTimeReqt = "00:00"
                '    g_dtTimeRecv = "00:00"
                '    g_dtTimeCrit = "00:00"
                g_dtTimeReqt = StringsHelper.Format(DateTimeHelper.Time, ACTimeConversion)
                'DC200201 do not set time recieved
                'g_dtTimeRecv = Format(Time, ACTimeConversion)
                'AJM (25/07/2001) - Do not default a time but if one has been enetered, show it!
                If Me.TimeRecv = "" Then
                    g_dtTimeRecv = ""
                Else
                    g_dtTimeRecv = StringsHelper.Format(Me.TimeRecv, ACTimeConversion)
                End If

                g_dtTimeCrit = StringsHelper.Format(DateTimeHelper.Time, ACTimeConversion)

                '    txtTimeRequested.Text = "00:00"
                '    txtTimeReceived.Text = "00:00"
                '    txtTimeCritical.Text = "00:00"
                txtTimeRequested.Text = StringsHelper.Format(DateTimeHelper.Time, ACTimeConversion)
                'DC200201 do not set time
                'txtTimeReceived.Text = Format(Time, ACTimeConversion)
                txtTimeReceived.Text = ""
                txtTimeCritical.Text = StringsHelper.Format(DateTimeHelper.Time, ACTimeConversion)

                FormatDate(g_dtDateReqt, txtDateRequested)
                FormatDate(g_dtDateCrit, txtDateCritical)
                FormatDate(g_dtDateRecv, txtDateReceived)

            Else
                g_dtDateReqt = StringsHelper.Format(Me.DateReq, ACDateConversion)
                g_dtDateCrit = StringsHelper.Format(Me.DateCrit, ACDateConversion)
                'DC200201 do not use a date
                'g_dtDateRecv = Format(frmService.DateRecv, ACDateConversion)
                'AJM (25/07/2001) - do not default a date but if one has been entered, show it!
                If Me.DateRecv = "" Then
                    g_dtDateRecv = ""
                Else
                    g_dtDateRecv = StringsHelper.Format(Me.DateRecv, ACDateConversion)
                End If

                g_dtTimeReqt = Me.TimeReq
                'DC200201 do not use a time
                'g_dtTimeRecv = frmService.TimeRecv
                'AJM (25/07/2001) - Do not default a time but if one has been enetered, show it!
                If Me.TimeRecv = "" Then
                    g_dtTimeRecv = ""
                Else
                    g_dtTimeRecv = StringsHelper.Format(Me.TimeRecv, ACTimeConversion)
                End If

                g_dtTimeCrit = Me.TimeCrit

                txtTimeRequested.Text = g_dtTimeReqt
                txtTimeCritical.Text = g_dtTimeCrit
                txtTimeReceived.Text = g_dtTimeRecv

                FormatDate(g_dtDateReqt, txtDateRequested)
                FormatDate(g_dtDateCrit, txtDateCritical)
                FormatDate(g_dtDateRecv, txtDateReceived)

            End If

            ' Update the interface details with the
            ' property members.
            m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            txtDescription.Text = m_sDesc
            txtReference.Text = m_sReference
            txtContact.Text = m_sContact
            txtRequirement.Text = m_sService

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub txtDateCritical_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateCritical.Enter
        iPMFunc.SelectText(txtDateCritical)

        txtDateCritical.Text = Format(g_dtDateCrit, ACDateConversion)
        g_dtDateCrit = ""
    End Sub

    Private Sub txtDateCritical_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateCritical.Leave
        'Any date entered here should be validated to be equal to or greater than the date requested.,
        'should not be empty, Valid date should be entered.
        'Not MANDATORY
        'Any date entered here should be validated to be equal to or greater than the date requested.,
        'should not be empty, Valid date should be entered.
        'Not MANDATORY
        If txtDateCritical.Text.Trim() <> "" Then
            'Changes by Sameer for the Message to come from the Resource File
            If Information.IsDate(txtDateCritical.Text) Then
                g_dtDateCrit = StringsHelper.Format(txtDateCritical.Text, ACDateConversion)
                FormatDate(txtDateCritical.Text, txtDateCritical)
            Else
                DisplayMessage(ACInvalidDateMsg, Mid(txtDateCritical.Name, 4))
                txtDateCritical.Text = ""
                txtDateCritical.Focus()
                Exit Sub
            End If


            If g_dtDateReqt.Trim() <> "" And g_dtDateCrit.Trim() <> "" Then

                If DateAndTime.DateDiff("d", CDate(StringsHelper.Format(g_dtDateReqt, ACDateConversion)), CDate(StringsHelper.Format(g_dtDateCrit, ACDateConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then

                    DisplayMessage(ACCannotBeGreaterThanLossDateMsg, Mid(txtDateCritical.Name, 4))
                    txtDateCritical.Text = ""
                    g_dtDateCrit = ""
                    txtDateCritical.Focus()
                    'Exit Sub
                End If
            End If

        Else
            g_dtDateCrit = ""
            g_dtTimeCrit = ""


        End If

    End Sub


    Private Sub txtDateReceived_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateReceived.Enter
        iPMFunc.SelectText(txtDateReceived)

        txtDateReceived.Text = Format(g_dtDateRecv, ACDateConversion)
        g_dtDateRecv = ""
    End Sub

    Private Sub txtDateReceived_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateReceived.Leave
        'Should be  equal to or later than the date requested
        If txtDateReceived.Text.Trim() <> "" Then

            'Should be  equal to or later than the date requested
            'Changes by Sameer for the Message to come from the Resource File
            If Information.IsDate(txtDateReceived.Text) Then
                g_dtDateRecv = StringsHelper.Format(txtDateReceived.Text, ACDateConversion)
                FormatDate(txtDateReceived.Text, txtDateReceived)
            Else
                DisplayMessage(ACInvalidDateMsg, Mid(txtDateReceived.Name, 4))
                txtDateReceived.Text = ""
                txtDateReceived.Focus()
                Exit Sub
            End If

            If txtDateRequested.Text.Trim() <> "" Then 'SK
                If g_dtDateReqt.Trim() <> "" And g_dtDateRecv.Trim() <> "" Then
                    If DateAndTime.DateDiff("d", CDate(StringsHelper.Format(g_dtDateReqt, ACDateConversion)), CDate(StringsHelper.Format(g_dtDateRecv, ACDateConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then

                        DisplayMessage(ACCannotBeGreaterThanLossDateMsg, Mid(txtDateReceived.Name, 4))
                        'MsgBox "Invalid Date Cannot be earlier than Loss Date"
                        txtDateReceived.Text = ""
                        g_dtDateRecv = ""
                        txtDateReceived.Focus()
                        Exit Sub
                    End If
                End If
            End If 'SK

            'check if the date reqt is less than or equal to current date
            If DateAndTime.DateDiff("d", CDate(g_dtDateRecv), CDate(StringsHelper.Format(DateTime.Today, ACDateConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then

                DisplayMessage(ACCannotBeGreaterThanTodaysDateMsg2, Mid(txtDateReceived.Name, 4))

                txtDateReceived.Text = ""
                txtDateReceived.Focus()

                Exit Sub

            End If

            '/************REVALIDATION FOR REPORTED TO DATE******IN CASE USER FILLS THIS AS LAST VALUE

        Else
            g_dtDateRecv = ""
            g_dtTimeRecv = ""

        End If

    End Sub

    Private Sub txtDateRequested_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateRequested.Enter
        iPMFunc.SelectText(txtDateRequested)

        txtDateRequested.Text = Format(g_dtDateReqt, ACDateConversion)
        g_dtDateReqt = ""
    End Sub

    Private Sub txtDateRequested_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateRequested.Leave
        'Should be earlier than or equal to the current date,
        'should not be empty, Valid date should be entered
        'Should be earlier than or equal to the current date,
        'should not be empty, Valid date should be entered
        If txtDateRequested.Text.Trim() <> "" Then


            If Information.IsDate(txtDateRequested.Text) Then
                g_dtDateReqt = StringsHelper.Format(txtDateRequested.Text, ACDateConversion)
                FormatDate(txtDateRequested.Text, txtDateRequested)
            Else
                DisplayMessage(ACInvalidDateMsg, Mid(txtDateRequested.Name, 4))
                txtDateRequested.Text = ""
                txtDateRequested.Focus()
                Exit Sub
            End If

            'check if the date reqt is less than or equal to current date
            If DateAndTime.DateDiff("d", CDate(g_dtDateReqt), CDate(StringsHelper.Format(DateTime.Today, ACDateConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then

                DisplayMessage(ACCannotBeGreaterThanTodaysDateMsg, Mid(txtDateRequested.Name, 4))

                txtDateRequested.Text = ""
                txtDateRequested.Focus()

                Exit Sub

            End If

            '/**********IF USER IS ENTERING ALL FIELDS AND LEAVING THIS FIELD EMPTY AND FILL IT AS THE LAST VALUE *********/

            If g_dtDateReqt.Trim() <> "" And g_dtDateRecv.Trim() <> "" Then
                If DateAndTime.DateDiff("d", CDate(StringsHelper.Format(g_dtDateReqt, ACDateConversion)), CDate(StringsHelper.Format(g_dtDateRecv, ACDateConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then

                    DisplayMessage(ACCannotBeGreaterThanLossDateMsg, Mid(txtDateReceived.Name, 4))
                    'MsgBox "Invalid Date Cannot be earlier than Loss Date"
                    txtDateReceived.Text = ""
                    g_dtDateRecv = ""
                    txtDateReceived.Focus()
                    'Exit Sub
                End If
            End If

            If g_dtDateReqt.Trim() <> "" And g_dtDateCrit.Trim() <> "" Then

                If DateAndTime.DateDiff("d", CDate(StringsHelper.Format(g_dtDateReqt, ACDateConversion)), CDate(StringsHelper.Format(g_dtDateCrit, ACDateConversion)), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) < 0 Then

                    DisplayMessage(ACCannotBeGreaterThanLossDateMsg, Mid(txtDateCritical.Name, 4))
                    txtDateCritical.Text = ""
                    g_dtDateCrit = ""
                    txtDateCritical.Focus()
                    'Exit Sub
                End If

            End If

        End If

    End Sub
    ' ***************************************************************** '
    ' Name: FormatDate
    '
    ' Description:  To write the details from the Business to Local variables
    '               INPUT : Input Date as strings
    '                       Control array index
    ' ***************************************************************** '
    Private Function FormatDate(ByRef sInDate As String, ByRef txtIndex As TextBox) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACLongdate As String = "long date"

            If Not Information.IsDate(sInDate) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'MsgBox "Invalid Date"
                txtIndex.Text = ""
                Return result
            End If

            txtIndex.Text = StringsHelper.Format(sInDate, ACLongdate)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatDate Method Failure", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub txtTimeCritical_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTimeCritical.Enter
        iPMFunc.SelectText(txtTimeCritical)

        If txtTimeCritical.Text.Trim() = "" Then
            txtTimeCritical.Text = StringsHelper.Format(DateTimeHelper.Time, ACTimeConversion)
        End If

    End Sub

    Private Sub txtTimeCritical_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTimeCritical.Leave
        If txtTimeCritical.Text.Trim() <> "" Then
            If IsTime(txtTimeCritical.Text) Then

                g_dtTimeCrit = StringsHelper.Format(txtTimeCritical.Text, ACTimeConversion)
            Else
                DisplayMessage(ACInvaildTimeMsg, "Reported_To_Time")
                txtTimeCritical.Text = ""
                txtTimeCritical.Focus()
            End If
            If txtDateCritical.Text.Trim() = "" Then
                txtTimeCritical.Text = ""
                g_dtTimeCrit = ""
            End If
        End If
    End Sub

    Private Sub txtTimeReceived_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTimeReceived.Enter
        iPMFunc.SelectText(txtTimeReceived)

        If txtTimeReceived.Text.Trim() = "" Then
            txtTimeReceived.Text = StringsHelper.Format(DateTimeHelper.Time, ACTimeConversion)
        End If

    End Sub

    Private Sub txtTimeReceived_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTimeReceived.Leave
        If txtTimeReceived.Text.Trim() <> "" Then

            If IsTime(txtTimeReceived.Text) Then

                g_dtTimeRecv = StringsHelper.Format(txtTimeReceived.Text, ACTimeConversion)
            Else
                DisplayMessage(ACInvaildTimeMsg, "Reported_Time")
                txtTimeReceived.Text = ""
                txtTimeReceived.Focus()
            End If

            If txtDateReceived.Text.Trim() = "" Then
                txtTimeReceived.Text = ""
                g_dtTimeRecv = ""
            End If
        End If
    End Sub

    Private Sub txtTimeRequested_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTimeRequested.Enter
        iPMFunc.SelectText(txtTimeRequested)
        If txtTimeRequested.Text.Trim() = "" Then
            txtTimeRequested.Text = StringsHelper.Format(DateTimeHelper.Time, ACTimeConversion)
        End If
    End Sub

    Private Sub txtTimeRequested_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTimeRequested.Leave
        If txtTimeRequested.Text.Trim() <> "" Then

            If IsTime(txtTimeRequested.Text) Then

                g_dtTimeReqt = StringsHelper.Format(txtTimeRequested.Text, ACTimeConversion)

            Else

                DisplayMessage(ACInvaildTimeMsg, "Loss_Time")
                txtTimeRequested.Text = ""
                txtTimeRequested.Focus()

            End If

        End If
    End Sub
    ' ***************************************************************** '
    ' Name: IsTime
    '
    ' Description:  To Check if the entered time value is in the correct
    '               Time Format
    '               INPUT : Time as string
    '               OUTPUT: True if correct format else False
    '
    ' ***************************************************************** '
    Private Function IsTime(ByRef sTime As String) As Boolean

        Dim result As Boolean = False
        Dim dtCheckTime As Date

        Try

            result = True

            sTime = sTime.Trim()

            If sTime <> "" Then

                If sTime.Length <= 5 Then

                    dtCheckTime = CDate(sTime)

                Else

                    result = False

                End If

            End If

            Return result

        Catch



            ' Error Section.


            Return False
        End Try

    End Function
End Class
