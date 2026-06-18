Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
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
    Private m_lPartyCnt As Integer
    Private m_lPartyConvictionID As Integer

    Private m_sCode As String = ""
    'developer guide no. 33
    Private m_vConvictionDate As Object
    Private m_sDescription As String = ""
    Private m_cFineAmt As Decimal
    Private m_sSentenceCode As String = ""
    Private m_sSentenceDescription As String = ""
    Private m_lSentenceDuration As Integer
    Private m_sSentenceDurationQualifier As String = ""
    'Start (Sriram P)PN 53931
    Private m_dtSentenceEffectiveDate As Date
    'End (Sriram P)PN 53931
    Private m_sStatusCode As String = ""
    Private m_lAlcoholLevel As Integer
    Private m_sAlcoholMeasurementMethod As String = ""
    Private m_lDrivingLicencePenaltyPoints As Integer

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPartyConviction.General

    ' Declare an instance of the Gemini List Manager
    'Private m_oGEMListManager As iGEMListManager.Interface

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)
    'Developer Guide No 7
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Property Procedures (Begin)
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public Property PartyConvictionID() As Integer
        Get
            Return m_lPartyConvictionID
        End Get
        Set(ByVal Value As Integer)
            m_lPartyConvictionID = Value
        End Set
    End Property

    Public ReadOnly Property Code() As String
        Get
            Return m_sCode
        End Get
    End Property
    Public ReadOnly Property ConvictionDate() As String
        Get
            Return m_vConvictionDate
        End Get
    End Property
    Public ReadOnly Property Description() As String
        Get
            Return m_sDescription
        End Get
    End Property
    Public ReadOnly Property FineAmount() As Decimal
        Get
            Return m_cFineAmt
        End Get
    End Property
    Public ReadOnly Property SentenceCode() As String
        Get
            Return m_sSentenceCode
        End Get
    End Property
    Public ReadOnly Property SentenceDescription() As String
        Get
            Return m_sSentenceDescription
        End Get
    End Property
    Public ReadOnly Property SentenceDuration() As Decimal
        Get
            Return m_lSentenceDuration
        End Get
    End Property
    Public ReadOnly Property SentenceDurationQualifier() As String
        Get
            Return m_sSentenceDurationQualifier
        End Get
    End Property
    Public ReadOnly Property SentenceEffectiveDate() As Date
        Get
            Return m_dtSentenceEffectiveDate
        End Get
    End Property
    Public ReadOnly Property StatusCode() As String
        Get
            Return m_sStatusCode
        End Get
    End Property
    Public ReadOnly Property AlcoholLevel() As Integer
        Get
            Return m_lAlcoholLevel
        End Get
    End Property
    Public ReadOnly Property AlcoholMeasurementMethod() As String
        Get
            Return m_sAlcoholMeasurementMethod
        End Get
    End Property
    Public ReadOnly Property DrivingLicencePenaltyPoints() As Integer
        Get
            Return m_lDrivingLicencePenaltyPoints
        End Get
    End Property

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

    Public WriteOnly Property UniqueId() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sUniqueId = Value
        End Set
    End Property

    Public WriteOnly Property ScreenHierarchy() As String
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
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

            result = gPMConstants.PMEReturnCode.PMTrue

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
            ''        ctlControl:=cboConvictionType, _
            ''        lFormat:=PMFormatString, _
            ''        lFieldType:=PMString, _
            ''        lMandatory:=PMMandatory)
            '    If (m_lReturn& <> PMTrue) Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'EK 21/10/99
            '    m_lReturn& = m_oFormFields.AddNewFormField( _
            ''        ctlControl:=cboConvStatus, _
            ''        lFormat:=PMFormatString, _
            ''        lFieldType:=PMString, _
            ''        lMandatory:=PMMandatory)
            '    If (m_lReturn& <> PMTrue) Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If

            ' ***** Non-Mandatory *************************************

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFine, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    m_lReturn& = m_oFormFields.AddNewFormField( _
            'ctlControl:=cboSentence, _
            'lFormat:=PMFormatString, _
            'lFieldType:=PMString, _
            'lMandatory:=PMNonMandatory)
            '    If (m_lReturn& <> PMTrue) Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSentDuration, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lFieldType:=gPMConstants.PMEDataType.PMLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSentDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lFieldType:=gPMConstants.PMEDataType.PMDate, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAlcLevel, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lFieldType:=gPMConstants.PMEDataType.PMLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPenaltyPts, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lFieldType:=gPMConstants.PMEDataType.PMLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    m_lReturn& = m_oFormFields.AddNewFormField( _
            'ctlControl:=cboTimeUnit, _
            'lFormat:=PMFormatString, _
            'lFieldType:=PMString, _
            'lMandatory:=PMNonMandatory)
            '    If (m_lReturn& <> PMTrue) Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSentDesc, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

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

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetDetails(vLockMode:=gPMConstants.PMELockMode.PMNoLock, vPartyCnt:=m_lPartyCnt, vPartyConvictionID:=m_lPartyConvictionID)

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
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            '    cboConvictionType.Text = m_sCode$
            ddConvictionType.Text = m_sCode
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControLValue:=m_vConvictionDate)
            txtDescription.Text = m_sDescription
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFine, vControLValue:=m_cFineAmt)
            '    cboSentence.Text = m_sSentenceCode$
            ddSentence.Text = m_sSentenceCode
            txtSentDesc.Text = m_sSentenceDescription
            'EK 21/10/99
            If m_lSentenceDuration > 0 Then
                txtSentDuration.Text = CStr(m_lSentenceDuration)
            Else
                txtSentDuration.Text = ""
            End If
            '    cboTimeUnit.Text = m_sSentenceDurationQualifier$
            ddTimeUnit.Text = m_sSentenceDurationQualifier
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSentDate, vControLValue:=m_dtSentenceEffectiveDate)
            '    cboConvStatus.Text = m_sStatusCode$
            ddConvStatus.Text = m_sStatusCode
            '    cboAlcMeasurement.Text = m_sAlcoholMeasurementMethod$
            ddAlcMeasurement.Text = m_sAlcoholMeasurementMethod
            'EK 21/10/99
            If m_lAlcoholLevel > 0 Then
                txtAlcLevel.Text = CStr(m_lAlcoholLevel)
            Else
                txtAlcLevel.Text = ""
            End If
            If m_lDrivingLicencePenaltyPoints > 0 Then
                txtPenaltyPts.Text = CStr(m_lDrivingLicencePenaltyPoints)
            Else
                txtPenaltyPts.Text = ""
            End If

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
            m_sScreenHierarchy = m_sScreenHierarchy & $"\Conviction({txtDescription.Text.Trim()})"
            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vPartyCnt:=m_lPartyCnt, vPartyConvictionID:=m_lPartyConvictionID, vCode:=m_sCode, vConvictionDate:=m_vConvictionDate, vDescription:=m_sDescription, vFineAmt:=m_cFineAmt, vSentenceCode:=m_sSentenceCode, vSentenceDescription:=m_sSentenceDescription, vSentenceDuration:=m_lSentenceDuration, vSentenceDurationQualifier:=m_sSentenceDurationQualifier, vSentenceEffectiveDate:=m_dtSentenceEffectiveDate, vStatusCode:=m_sStatusCode, vAlcoholLevel:=m_lAlcoholLevel, vAlcoholMeasurementMethod:=m_sAlcoholMeasurementMethod, vDrivingLicencePenaltyPoints:=m_lDrivingLicencePenaltyPoints, vUniqueId:=m_sUniqueId, vScreenHierarchy:=m_sScreenHierarchy)

                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vPartyCnt:=m_lPartyCnt, vPartyConvictionID:=m_lPartyConvictionID, vCode:=m_sCode, vConvictionDate:=m_vConvictionDate, vDescription:=m_sDescription, vFineAmt:=m_cFineAmt, vSentenceCode:=m_sSentenceCode, vSentenceDescription:=m_sSentenceDescription, vSentenceDuration:=m_lSentenceDuration, vSentenceDurationQualifier:=m_sSentenceDurationQualifier, vSentenceEffectiveDate:=m_dtSentenceEffectiveDate, vStatusCode:=m_sStatusCode, vAlcoholLevel:=m_lAlcoholLevel, vAlcoholMeasurementMethod:=m_sAlcoholMeasurementMethod, vDrivingLicencePenaltyPoints:=m_lDrivingLicencePenaltyPoints, vUniqueId:=m_sUniqueId, vScreenHierarchy:=m_sScreenHierarchy)

                Case gPMConstants.PMEComponentAction.PMDelete
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditDelete(lRow:=lBusinessDataID, vUniqueId:=m_sUniqueId, vScreenHierarchy:=m_sScreenHierarchy)

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    'EK 21/10/99
    ' ***************************************************************** '
    ' Name: ValidateOK
    '
    ' Description: Validates Screen
    ' ***************************************************************** '
    Private Function ValidateOK() As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        '        If (cboConvictionType.Text = "") Then
        If ddConvictionType.Text = "" Then
            MessageBox.Show("No Conviction Type Entered", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        '        If (cboConvStatus.Text = "") Then
        If ddConvStatus.Text = "" Then
            MessageBox.Show("No Conviction Status Entered", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' RAG 10/11/2002 - Validate to disallow future dates
        ' ISS: 1245
        If Information.IsDate(txtDate.Text) Then
            If CDate(txtDate.Text) > DateTime.Now Then
                MessageBox.Show("Future Dated Conviction Not Allowed", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        Return result
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


            m_lReturn = m_oBusiness.GetNext(vPartyCnt:=m_lPartyCnt, vPartyConvictionID:=m_lPartyConvictionID, vCode:=m_sCode, vConvictionDate:=m_vConvictionDate, vDescription:=m_sDescription, vFineAmt:=m_cFineAmt, vSentenceCode:=m_sSentenceCode, vSentenceDescription:=m_sSentenceDescription, vSentenceDuration:=m_lSentenceDuration, vSentenceDurationQualifier:=m_sSentenceDurationQualifier, vSentenceEffectiveDate:=m_dtSentenceEffectiveDate, vStatusCode:=m_sStatusCode, vAlcoholLevel:=m_lAlcoholLevel, vAlcoholMeasurementMethod:=m_sAlcoholMeasurementMethod, vDrivingLicencePenaltyPoints:=m_lDrivingLicencePenaltyPoints)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            Return result

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
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            '    m_sCode$ = cboConvictionType.Text
            m_sCode = ddConvictionType.Text
            'm_vConvictionDate = m_oFormFields.UnformatControl(txtDate)
            'developer guide no. changed the code in order to get the date in the format as was in VB Code.
            m_vConvictionDate = CDate(txtDate.Text).ToString("dd MMMM yyyy")
            m_sDescription = txtDescription.Text

            If txtFine.Text.Trim() <> "" Then
                m_cFineAmt = CDec(txtFine.Text)
            Else
                m_cFineAmt = 0
            End If

            '    m_sSentenceCode$ = cboSentence.Text
            m_sSentenceCode = ddSentence.Text
            m_sSentenceDescription = txtSentDesc.Text

            If txtSentDuration.Text.Trim() <> "" Then
                m_lSentenceDuration = CInt(txtSentDuration.Text)
            Else
                m_lSentenceDuration = 0
            End If

            '    m_sSentenceDurationQualifier$ = cboTimeUnit.Text
            m_sSentenceDurationQualifier = ddTimeUnit.Text
            'Start (Sriram P)PN 53931
            m_dtSentenceEffectiveDate = gPMFunctions.ToSafeDate(txtSentDate.Text.Trim())
            'end (Sriram P)PN 53931
            '    m_sStatusCode$ = cboConvStatus.Text
            m_sStatusCode = ddConvStatus.Text
            '    m_sAlcoholMeasurementMethod$ = cboAlcMeasurement.Text
            m_sAlcoholMeasurementMethod = ddAlcMeasurement.Text

            If txtAlcLevel.Text.Trim() <> "" Then
                m_lAlcoholLevel = CInt(txtAlcLevel.Text)
            Else
                m_lAlcoholLevel = 0
            End If

            If txtPenaltyPts.Text.Trim() <> "" Then
                m_lDrivingLicencePenaltyPoints = CInt(txtPenaltyPts.Text)
            Else
                m_lDrivingLicencePenaltyPoints = 0
            End If

            Return result

        Catch excep As System.Exception



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

            ' Populate the combo boxes using the gemini list control, and
            ' the polaris codes.
            ' then default the combo boxes to empty

            '    m_lReturn& = m_oGEMListManager.PopulateListControl( _
            'v_sPropertyID:="1114113", _
            'r_oControl:=cboConvictionType)
            '    cboConvictionType.ListIndex = -1

            '    m_lReturn& = m_oGEMListManager.PopulateListControl( _
            'v_sPropertyID:="1114119", _
            'r_oControl:=cboSentence)
            '    cboSentence.ListIndex = -1

            '    m_lReturn& = m_oGEMListManager.PopulateListControl( _
            'v_sPropertyID:="1114122", _
            'r_oControl:=cboTimeUnit)
            '    cboTimeUnit.ListIndex = -1

            '    m_lReturn& = m_oGEMListManager.PopulateListControl( _
            'v_sPropertyID:="1114124", _
            'r_oControl:=cboConvStatus)
            '    cboConvStatus.ListIndex = -1

            '    m_lReturn& = m_oGEMListManager.PopulateListControl( _
            'v_sPropertyID:="1114126", _
            'r_oControl:=cboAlcMeasurement)
            '    cboAlcMeasurement.ListIndex = -1

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

            '    Set m_ctlTabFirstLast(ACControlStart, 0) = cboConvictionType
            m_ctlTabFirstLast(ACControlStart, 0) = ddConvictionType
            m_ctlTabFirstLast(ACControlEnd, 0) = txtPenaltyPts

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


            'developer guide no. 243
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


            'developer guide no. 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no. 243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no. 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no. 243
            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no. 243
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
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


            'developer guide no. 243
            lblConvictionType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no. 243
            lblDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no. 243
            lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no. 243
            lblFine.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblFine, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'developer guide no. 243
            lblSentence.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblSentence, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no. 243
            lblSentenceDesc.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblSentenceDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no. 243
            lblSentDuration.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblSentenceDuration, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no. 243
            lblTimeUnit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblTimeUnit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'developer guide no. 243
            lblEffectiveDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblSentenceDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

            'developer guide no. 243
            lblConvictionStatus.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblConvictionStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'developer guide no. 243
            lblAlcMethod.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblAlcMethod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'developer guide no. 243
            lblAlcoholLevel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblAlcLevel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


            'developer guide no. 243
            lblPenaltyPoints.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLblPenaltyPts, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




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
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try


            ' Gets all of the lookup values.

            ' Check the task.
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

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




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
    'SP150998 - compare long value not string
    ' Check if this is the selected index.
    'If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
    'If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    'End If
    '
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

    'Private Sub cboAlcMeasurement_DropDown()
    '
    '    m_lReturn& = FillCombo(cboControl:=cboAlcMeasurement, _
    ''                           bRefill:=True, _
    ''                           sPropertyID:="1114126")
    '
    'End Sub
    '
    'Private Sub cboConvictionType_DropDown()
    '
    '    m_lReturn& = FillCombo(cboControl:=cboConvictionType, _
    ''                           bRefill:=True, _
    ''                           sPropertyID:="1114113")
    '
    'End Sub
    '
    'Private Sub cboConvictionType_GotFocus()
    '   ' m_lReturn& = m_oFormFields.GotFocus(ctlControl:=cboConvictionType)
    'End Sub
    '
    'Private Sub cboConvictionType_LostFocus()
    '   ' m_lReturn& = m_oFormFields.LostFocus(ctlControl:=cboConvictionType)
    'End Sub
    '
    'Private Sub cboConvStatus_DropDown()
    '    m_lReturn& = FillCombo(cboControl:=cboConvStatus, _
    ''                           bRefill:=True, _
    ''                           sPropertyID:="1114124")
    'End Sub
    '
    'Private Sub cboSentence_DropDown()
    '    m_lReturn& = FillCombo(cboControl:=cboSentence, _
    ''                           bRefill:=True, _
    ''                           sPropertyID:="1114119")
    'End Sub
    '
    'Private Sub cboSentence_GotFocus()
    '    m_lReturn& = m_oFormFields.GotFocus(ctlControl:=cboSentence)
    'End Sub
    '
    'Private Sub cboSentence_LostFocus()
    '    m_lReturn& = m_oFormFields.LostFocus(ctlControl:=cboSentence)
    'End Sub
    '
    'Private Sub cboTimeUnit_DropDown()
    '    m_lReturn& = FillCombo(cboControl:=cboTimeUnit, _
    ''                           bRefill:=True, _
    ''                           sPropertyID:="1114122")
    'End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        'developer guide no. 20
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        End If

    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyConviction.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBPartyConviction.General()

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

            ' Initialise Gemini List Manager
            '    Set m_oGEMListManager = New iGEMListManager.Interface
            '
            '    m_lReturn& = m_oGEMListManager.Initialise()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '        Exit Sub
            '    End If
            '
            '    m_lReturn& = m_oGEMListManager.CheckListVersions()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '        Exit Sub
            '    End If

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

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    'Developer Guide No 7
                    eventArgs.Cancel = True

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

            '    ' Terminate the Gemini list manager
            '    m_lReturn& = m_oGEMListManager.Terminate()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        m_lErrorNumber& = PMFalse
            '    End If
            '
            '    ' Destroy the instance of the list manager
            '    Set m_oGEMListManager = Nothing

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
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.C Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




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

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            m_lReturn = ValidateOK()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

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

    'UPGRADE_NOTE: (7001) The following declaration (cmdNext_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdNext_Click(ByRef Index As Integer)
    '
    ''UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Try 
    '
    ''     Change to the next tab.
    ''    If (tabMainTab.Tab < tabMainTab.Tabs - 1) Then
    ''        tabMainTab.Tab = Index + 1
    ''    End If
    ''
    ''     Set focus to the first control on the tab.
    ''    If (tabMainTab.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
    ''        m_ctlTabFirstLast(ACControlStart, Index + 1).SetFocus
    ''    End If
    '
    'Catch 
    '
    '
    '
    '
    'Exit Sub
    'End Try
    '
    '
    'End Sub
    ' PRIVATE Events (End)

    Private Sub txtAlcLevel_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAlcLevel.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAlcLevel)
    End Sub

    Private Sub txtAlcLevel_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAlcLevel.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAlcLevel)
    End Sub

    Private Sub txtDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDate)
    End Sub

    Private Sub txtDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDate)
    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtFine_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFine.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtFine)
    End Sub

    Private Sub txtFine_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFine.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtFine)
    End Sub

    Private Sub txtPenaltyPts_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPenaltyPts.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPenaltyPts)
    End Sub

    Private Sub txtPenaltyPts_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPenaltyPts.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPenaltyPts)
    End Sub

    Private Sub txtSentDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSentDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSentDate)
    End Sub

    Private Sub txtSentDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSentDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSentDate)
    End Sub

    Private Sub txtSentDesc_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSentDesc.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSentDesc)
    End Sub

    Private Sub txtSentDesc_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSentDesc.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSentDesc)
    End Sub

    Private Sub txtSentDuration_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSentDuration.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSentDuration)
    End Sub

    Private Sub txtSentDuration_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSentDuration.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSentDuration)
    End Sub


    ' ************************************************************************** '
    '
    ' Fills combo box with polaris list via ComponentManager
    '
    ' History: original by SJ
    '          modified heavily for S4B by CF - 7/5/99
    '
    ' ************************************************************************** '
    'Public Function FillCombo(cboControl As ComboBox, _
    ''                          bRefill As Boolean, _
    ''                          sPropertyID As String) As Long
    '
    'Dim vListArray As Variant
    '
    'Dim lPropertyID As Long
    'Dim iPropertyType As Integer
    'Dim sTableName As String
    'Dim sFieldName As String
    '
    'Dim sMatchString As String
    'Dim lNumItems As Long
    'Dim lItem As Long
    '
    'Dim lReturn As Long
    'Dim sText As String
    '
    '    On Error GoTo Err_FillCombo
    '
    '    FillCombo& = PMTrue
    '    sMatchString = ""
    '
    '    With cboControl
    '
    '        ' Save text
    '        sText = .Text
    '
    '        If (Len(sText) = 0) Then
    '
    '            ' Mouse pointer to busy while it re-loads the list
    '            SetMousePointer PMMouseBusy
    '
    '            m_lReturn& = m_oGEMListManager.PopulateListControl( _
    ''                            v_sPropertyID:=sPropertyID, _
    ''                            r_oControl:=cboControl)
    '
    '            ' Reset the mouse pointer back to normal
    '            SetMousePointer PMMouseNormal
    '
    '            If (m_lReturn& <> PMTrue) Then
    '                FillCombo = PMFalse
    '                Exit Function
    '            End If
    '
    '            Exit Function
    '
    '        End If
    '
    '        ' If it's not a refill, it only needs filling once
    '        If (bRefill = False) And (.ListCount > 0) Then
    '            ' Return successful
    '            Exit Function
    '        End If
    '
    '        ' If it's a refill, then only return matching items
    '        If (bRefill = True) Then
    '            sMatchString = .Text
    '        End If
    '
    '        ' Get the List from the list manager
    '        If sMatchString <> "" Then
    '
    '            lReturn& = m_oGEMListManager.GetList( _
    ''                v_sPropertyID:=sPropertyID, _
    ''                r_vListData:=vListArray, _
    ''                v_vSearchString:=sMatchString)
    '        Else
    '
    '            lReturn& = m_oGEMListManager.GetList( _
    ''                v_sPropertyID:=CStr(lPropertyID), _
    ''                r_vListData:=vListArray)
    '
    '        End If
    '
    '        If (lReturn& <> PMTrue) Then
    '            FillCombo& = PMFalse
    '
    '            ' Log Error.
    '            LogMessage _
    ''                iType:=PMLogOnError, _
    ''                sMsg:="Failed to get list from List Manager", _
    ''                vApp:=ACApp, _
    ''                vClass:=ACClass, _
    ''                vMethod:="FillCombo", _
    ''                vErrNo:=Err.Number, _
    ''                vErrDesc:=Err.Description
    '
    '            Exit Function
    '        End If
    '
    '        ' Put the list into the Array
    '        lNumItems = UBound(vListArray)
    '        If IsArray(vListArray) = True Then
    '            .Clear
    '            .AddItem " "
    '
    '            For lItem& = 0 To lNumItems&
    '            .AddItem Trim$(vListArray(lItem&))
    '
    '            Next
    '        End If
    '
    '        'sj 15/02/99 - end
    '
    '        ' Restore text
    '        If .Style = vbComboDropdown Then
    '            .Text = sText
    '        End If
    '
    '    End With
    '
    '    Exit Function
    '
    'Err_FillCombo:
    '
    '    FillCombo& = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="FillCombo Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="FillCombo", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    'End Function
End Class
