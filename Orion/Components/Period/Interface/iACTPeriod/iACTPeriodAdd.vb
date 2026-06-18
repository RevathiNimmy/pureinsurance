Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'Developer Guide no 129
Imports SharedFiles
Partial Public Class frmAdd
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: {TodaysDate}
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmAdd"

    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_sComboMonthText As String = ""
    Private m_sComboQuarterText As String = ""
    Private m_sComboFourWeeksText As String = ""

    Private m_sPeriodPrefixDefault As String = ""
    ' Used to return latest period date from business select
    Private m_vResultArray(,) As Object
    ' Used to store array of generated periods
    Private m_vPeriodData(,) As Object
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Instance of form control
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    Private m_dPrevYearEndDate As Date
    Private m_dPeriodEndDate As Date
    Private m_dYearEndDate As Date
    Private m_dYearEndDateLessWeek As Date
    Private m_iPeriodLength As Integer
    Private m_sYearName As New FixedLengthString(20)
    Private m_sPeriodPrefix As New FixedLengthString(15)
    Private m_sPeriodName As New FixedLengthString(15)
    'DD 02/08/2002: For Multi-Branch
    Public SubBranchID As Integer
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

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)


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

    ' {* USER DEFINED CODE (Begin) *}
    Public ReadOnly Property YearName() As String
        Get

            ' Return the year name
            Return m_sYearName.Value

        End Get
    End Property
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef oBusiness As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the instance of the business object
            ' into the memberfor use by lookup
            m_oBusiness = oBusiness

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Load (Standard Method)
    '
    ' Description: Load Interface defaults and get details replaces
    '              form load event
    '
    ' ***************************************************************** '
    Public Function Load_Renamed() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ShowForm (Standard Method)
    '
    ' Description: Show the form
    '
    ' ***************************************************************** '
    Public Function ShowForm(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show the the form, allow user input etc.
            VB6.ShowForm(Me, lDisplayState)

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************************
    '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************************
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            m_oFormFields = New iPMFormControl.FormFields()

            m_oFormFields.LanguageID = g_iLanguageID

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the controls...

            ' Year end date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPrevYearEndDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Year name
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtYearName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Period prefix
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPeriodPrefix, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

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
            ' Default Period Prefix
            txtPeriodPrefix.Text = m_sPeriodPrefixDefault
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
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details and sets the appropriate
    '              style.
    '
    ' ***************************************************************** '
    Private Function GetInterfaceDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the interface details from the
            ' business object.
            m_lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)

            ' If we have a record populate the interface
            If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the business to the interface.
                m_lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Display all of the lookup details.
            m_lReturn = CType(DisplayLookupDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetPeriodLastDate(vResultArray:=m_vResultArray)

            ' If no records found return NotFound
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
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
    Private Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_dPrevYearEndDate = CDate(m_vResultArray(0, 0))

            ' Assign the details to the interface.
            'Developer Guide no 40
          txtPrevYearEndDate.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateTimeLong, vFieldValue:=m_dPrevYearEndDate)

            ' We have an end date so disable text box to prevent change
            txtPrevYearEndDate.Enabled = False

            ' Set first control to be next field
            m_ctlTabFirstLast(ACControlStart, 0) = cmbPeriodLength

            ' Set default year name to last year + 1
            txtYearName.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=CStr(DateAndTime.DatePart("yyyy", m_dPrevYearEndDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1) + 1))

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
    Private Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim lRecordCount, lPeriodCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details from the interface to the data storage.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get number of records currently in business colection

            lRecordCount = m_oBusiness.RecordCount

            ' Loop round generated period array adding each one to business
            For lRow As Integer = 0 To m_vPeriodData.GetUpperBound(1)

                ' This period business row is current record count + array pos + 1
                lPeriodCount = lRecordCount + lRow + 1

                ' Inform the business object with the next period

                m_lReturn = m_oBusiness.EditAdd(lRow:=lPeriodCount, vPeriodID:=0, vCompanyID:=g_iSourceID, vSubBranchID:=SubBranchID, vYearName:=m_vPeriodData(ACSubPeriodYearName, lRow), vPeriodName:=m_vPeriodData(ACSubPeriodName, lRow), vPeriodEndDate:=m_vPeriodData(ACSubPeriodEndDate, lRow), vPeriodEndComplete:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit For
                End If
            Next

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
    Private Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Dim sMonthText As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'BB not required by this form
            '    ' Get the lookup values.
            '    m_lReturn& = GetLookupValues()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retrieve all of the lookup
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
            Dim cmbPeriodLength_NewIndex As Integer = -1
            cmbPeriodLength_NewIndex = cmbPeriodLength.Items.Add(m_sComboMonthText)
            VB6.SetItemData(cmbPeriodLength, cmbPeriodLength_NewIndex, ACMonth)
            cmbPeriodLength_NewIndex = cmbPeriodLength.Items.Add(m_sComboFourWeeksText)
            VB6.SetItemData(cmbPeriodLength, cmbPeriodLength_NewIndex, ACFourWeeks)
            cmbPeriodLength_NewIndex = cmbPeriodLength.Items.Add(m_sComboQuarterText)
            VB6.SetItemData(cmbPeriodLength, cmbPeriodLength_NewIndex, ACQuarter)

            cmbPeriodLength.SelectedIndex = 0

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
    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Public Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Check if form has been cancelled, if so,
            ' prompt if you wish to lose details.
            If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                ' Get string messages


                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Set return to false, meaning
                    ' don't cancel.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' Form hasn't been cancelled, so we just go
                ' ahead and add the details.

                ' Update the business from the interface.
                m_lReturn = CType(InterfaceToBusiness(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the details using the business object.

                m_lReturn = m_oBusiness.Update()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to update business.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                    Return result
                End If
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetLastDateForMonth
    '
    ' Description: Returns the date of the last day in a month
    '
    ' History: 09/02/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetLastDateForMonth(ByVal v_dDate As Date, ByRef r_dNewDate As Date) As Integer

        Dim result As Integer = 0
        Dim dDate, dNewDate As Date
        Dim sMonth1, sMonth2 As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            dNewDate = v_dDate

            ' Loop until the month has advanced
            Do
                dDate = dNewDate
                sMonth1 = CStr(DateAndTime.DatePart("m", dDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1))
                dNewDate = dDate.AddDays(1)
                sMonth2 = CStr(DateAndTime.DatePart("m", dNewDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1))
            Loop While (CInt(sMonth1) = CInt(sMonth2))

            ' Assign the date
            r_dNewDate = dDate

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLastDateForMonth Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLastDateForMonth", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim lPeriod As Integer
        Dim dtNewDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' Following code used to generate a year's worth of periods
            ' based on parameters entered

            m_dPrevYearEndDate = CDate(txtPrevYearEndDate.Text)
            ' Calculate year end
            m_dYearEndDate = m_dPrevYearEndDate.AddYears(1)
            ' Calculate year end less a week
            m_dYearEndDateLessWeek = DateAndTime.DateAdd("ww", -1, m_dYearEndDate)
            m_iPeriodLength = VB6.GetItemData(cmbPeriodLength, cmbPeriodLength.SelectedIndex)
            m_sYearName.Value = txtYearName.Text.Trim()
            m_sPeriodPrefix.Value = txtPeriodPrefix.Text.Trim()

            ' Initialise the period array
            ReDim m_vPeriodData(3, 0)

            ' Set initial end date to prev year end date
            m_dPeriodEndDate = m_dPrevYearEndDate

            ' Loop enough times to to generate possible periods
            For lRow As Integer = 0 To 100

                ' Set this period number
                lPeriod = lRow + 1

                ' Construct default period name
                m_sPeriodName.Value = m_sPeriodPrefix.Value.Trim() & CStr(lPeriod)

                ' Calculate end date for next period depending on period length
                Select Case m_iPeriodLength
                    Case ACMonth
                        dtNewDate = m_dPrevYearEndDate.AddMonths(lPeriod)
                        ' CTAF 090200 - Get last date for month now, instead of just adding a month.
                        ' This was causing problems if the year ended on February as all the other month
                        ' ends would be 28/29th
                        m_lReturn = CType(GetLastDateForMonth(v_dDate:=dtNewDate, r_dNewDate:=m_dPeriodEndDate), gPMConstants.PMEReturnCode)
                    Case ACFourWeeks
                        m_dPeriodEndDate = DateAndTime.DateAdd("ww", lPeriod * 4, m_dPrevYearEndDate)
                    Case ACQuarter
                        m_dPeriodEndDate = DateAndTime.DateAdd("q", lPeriod, m_dPrevYearEndDate)
                    Case Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit For
                End Select

                ' Populate array with names and end date
                m_vPeriodData(ACSubPeriodYearName, lRow) = m_sYearName.Value.Trim()
                m_vPeriodData(ACSubPeriodName, lRow) = m_sPeriodName.Value.Trim()

                ' Store unique key for this row.
                m_vPeriodData(m_vPeriodData.GetUpperBound(0), lRow) = lPeriod

                Select Case m_iPeriodLength
                    Case ACFourWeeks
                        ' If within the year to the nearest week use calculated date
                        ' to avoid odd few days at the end of the year
                        If m_dPeriodEndDate < m_dYearEndDateLessWeek Then
                            m_vPeriodData(ACSubPeriodEndDate, lRow) = m_dPeriodEndDate
                        Else
                            ' otherwise set last period date to year end and exit loop
                            m_vPeriodData(ACSubPeriodEndDate, lRow) = m_dYearEndDate
                            Exit For
                        End If
                    Case Else
                        ' If within the year use calculated date
                        If m_dPeriodEndDate < m_dYearEndDate Then
                            m_vPeriodData(ACSubPeriodEndDate, lRow) = m_dPeriodEndDate
                        Else
                            ' otherwise set last period date to year end and exit loop
                            m_vPeriodData(ACSubPeriodEndDate, lRow) = m_dYearEndDate
                            Exit For
                        End If
                End Select


                ' Increment the data array ready for next entry
                ReDim Preserve m_vPeriodData(m_vPeriodData.GetUpperBound(0), m_vPeriodData.GetUpperBound(1) + 1)

            Next

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            m_ctlTabFirstLast(ACControlStart, 0) = txtPrevYearEndDate
            m_ctlTabFirstLast(ACControlEnd, 0) = txtPeriodPrefix
            ' {* USER DEFINED CODE (End) *}

            Return result

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
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


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


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            m_sComboMonthText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACComboMonth, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            m_sComboFourWeeksText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACComboFourWeeks, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            m_sComboQuarterText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACComboQuarter, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            m_sPeriodPrefixDefault = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPeriodPrefixDefault, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '

    'Private Function GetLookupValues() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Gets all of the lookup values.
    '
    ' Check the task.
    'Select Case (m_iTask)
    'Case gPMConstants.PMEComponentAction.PMAdd
    ' Get all of the lookup values.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMEdit
    ' Get all of the lookup values with the correct
    ' effective date.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    '
    'Case gPMConstants.PMEComponentAction.PMView
    ' Get lookup values for viewing only.

    'm_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
    'End Select
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
    '
    'Return result
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '

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
    ' PRIVATE Methods (End)


    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer Guide no184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID1), gPMConstants.PMEReturnCode)

    End Sub


    Private Sub frmAdd_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

    End Sub

    ' PRIVATE Events (Begin)
    Private Sub frmAdd_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'Developer Guide no 19
            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Remove the local instance of the business object
            m_oBusiness = Nothing

            '
		m_oFormFields.Dispose()

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

    Private Sub frmAdd_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
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

            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                '        ' Set the default button.
                '        If (.Tab < cmdNext.Count) Then
                '            cmdNext(.Tab).Default = True
                '        Else
                '            cmdOK.Default = True
                '        End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If
            End With

        Catch



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Check form control
            m_lReturn = m_oFormFields.CheckMandatoryControls()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

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

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

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

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub txtPeriodPrefix_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPeriodPrefix.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPeriodPrefix)

    End Sub

    Private Sub txtPrevYearEndDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPrevYearEndDate.Enter

        ' commented - cf - 04/08/98
        ' Check date.
        'CheckDateGotFocus txtPrevYearEndDate

        ' commented - cf - 04/08/98
        ' Hightlight any text.
        'SelectText txtPrevYearEndDate

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPrevYearEndDate)

    End Sub

    Private Sub txtPrevYearEndDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPrevYearEndDate.Leave


        ' commented - cf - 04/08/98
        ' Check date.
        'CheckDateLostFocus txtPrevYearEndDate

        ' commented - cf - 04/08/98
        ' Is this a valid date? as this can happen concurrently with CheckDate
        ' Set default year name to last year + 1
        '    If (IsDate(txtPrevYearEndDate.Text)) _
        ''    And (Trim$(txtYearName.Text) = "") Then
        '        txtYearName.Text = FormatField( _
        ''            iFormatType:=PMFormatString, _
        ''            vFieldValue:=DatePart("yyyy", CDate(txtPrevYearEndDate.Text)) + 1)
        '    End If

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPrevYearEndDate)

    End Sub

    Private Sub txtYearName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtYearName.Enter

        '    SelectText txtYearName

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtYearName)

    End Sub
    Private Sub txtPeriodPrefix_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPeriodPrefix.Enter

        '    SelectText txtPeriodPrefix

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPeriodPrefix)

    End Sub

    ' PRIVATE Events (End)

    Private Sub txtYearName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtYearName.Leave
        Dim lReturn As Integer
        'JK040299
        'Check to see if last four characters of the year name is a valid year

        If txtYearName.Text > "****" Then
            Dim dbNumericTemp As Double
            If txtYearName.Text.Length > 3 AndAlso Double.TryParse(txtYearName.Text.Substring(txtYearName.Text.Length - 4), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                If StringsHelper.ToDoubleSafe(txtYearName.Text.Substring(txtYearName.Text.Length - 4)) > 1900 And StringsHelper.ToDoubleSafe(txtYearName.Text.Substring(txtYearName.Text.Length - 4)) < 3000 Then
                    m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtYearName)
                Else
                    lReturn = MessageBox.Show("Please Enter Last Four Characters As Valid Year", "Period", MessageBoxButtons.OK, MessageBoxIcon.Error) = System.Windows.Forms.DialogResult.OK
                    txtYearName.Focus()
                End If
            Else
                lReturn = MessageBox.Show("Please Enter Last Four Characters As Valid Year", "Period", MessageBoxButtons.OK, MessageBoxIcon.Error) = System.Windows.Forms.DialogResult.OK
                txtYearName.Focus()
            End If
        End If

    End Sub
End Class
