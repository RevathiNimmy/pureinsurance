Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Public Class frmDetail
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmDetail
    '
    ' Date: 19/06/2000
    '
    ' Description: Policy Number Maintenance Detail interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmDetail"

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

    Private m_sStepStatus As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lSchemeId As Integer
    Private m_lBusinessType As Integer
    Private m_sBusinessTypeDescription As String = ""
    Private m_bGenerated As Boolean
    Private m_sMask As String = ""
    Private m_sFixedCode As String = ""
    Private m_lNextNumber As Integer
    Private m_lHighestNumber As Integer
    Private m_lStep As Integer
    Private m_sNextNumberToAllocate As String = ""
    Private m_sCode As String = ""
    Private m_sDescription As String = ""
    Private m_bReuse As Boolean
    Private m_iScheme As Integer
    Private m_dtEffectiveDate As Date
    Private m_iNoOfXsInMask As Integer
    Private m_iNoOf9sInMask As Integer
    Private m_oBusiness As Object

    Private m_vExistingNumberingSchemes(,) As Object
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    Private Const PMLookupBusinessType As String = "numbering_scheme_type"

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    'Message variables
    Private m_sNextNoGtThanHighestMsg As String = ""
    Private m_sDuplicateNumScheme1Msg As String = ""
    Private m_sDuplicateNumScheme2Msg As String = ""
    Private m_sSelGenValMsg As String = ""
    Private m_sNumSchemeLimitsMsg As String = ""
    Private m_sValidCharsForValMsg As String = ""
    Private m_sMaskCharsConsecutiveMsg As String = ""
    Private m_sFixedCodeMatchesPlaceHoldersMsg As String = ""
    Private m_sDigitsinHighestNumMsg As String = ""
    ' Start - (Sankar) - (Tech Spec - VAL P14 - Policy Numbering.doc)
    Private m_sRenewalCodeAtEnd As String = ""
    Private m_sInvalidRenewalCode As String = ""
    ' End - (Sankar) - (Tech Spec - VAL P14 - Policy Numbering.doc)
    'Start - Renuka - (WPR87 Paralleling)
    Private m_sInvalidAccountingPeriod As String = ""
    Private m_sBusinessFail As String = ""
    'End - Renuka - (WPR87 Paralleling)
    'Maintain Party Code
    Private m_iIsReadOnly As Integer
    Private m_lPartyTypeID As Integer
    Private m_bClearMask As Boolean
    '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.1)
    Dim m_bIsResetNumberDaily As Boolean
    '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.1)
    'Start - Renuka - (WPR87 Paralleling)
    Private m_bResetNumber As Boolean
    Dim m_bResetMask As Boolean

    Public Property ResetNumber() As Boolean
        Get
            Return m_bResetNumber
        End Get
        Set(ByVal Value As Boolean)
            m_bResetNumber = Value
        End Set
    End Property
    'End - Renuka - (WPR87 Paralleling)
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


    Public Property BusinessType() As Integer
        Get
            Return m_lBusinessType
        End Get
        Set(ByVal Value As Integer)
            m_lBusinessType = Value
        End Set
    End Property


    Public Property FixedCode() As String
        Get
            Return m_sFixedCode
        End Get
        Set(ByVal Value As String)
            m_sFixedCode = Value
        End Set
    End Property


    Public Property Generated() As Boolean
        Get
            Return m_bGenerated
        End Get
        Set(ByVal Value As Boolean)
            m_bGenerated = Value
        End Set
    End Property


    Public Property Mask() As String
        Get
            Return m_sMask
        End Get
        Set(ByVal Value As String)
            m_sMask = Value
        End Set
    End Property


    Public Property NextNumber() As Integer
        Get
            Return m_lNextNumber
        End Get
        Set(ByVal Value As Integer)
            m_lNextNumber = Value
        End Set
    End Property


    Public Property HighestNumber() As Integer
        Get
            Return m_lHighestNumber
        End Get
        Set(ByVal Value As Integer)
            m_lHighestNumber = Value
        End Set
    End Property


    Public Property Step_Renamed() As Integer
        Get
            Return m_lStep
        End Get
        Set(ByVal Value As Integer)
            m_lStep = Value
        End Set
    End Property


    Public Property NextNumberToAllocate() As String
        Get
            Return m_sNextNumberToAllocate
        End Get
        Set(ByVal Value As String)
            m_sNextNumberToAllocate = Value
        End Set
    End Property


    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property


    Public Property Reuse() As Boolean
        Get
            Return m_bReuse
        End Get
        Set(ByVal Value As Boolean)
            m_bReuse = Value
        End Set
    End Property


    Public Property ExistingNumberingSchemes() As Object
        Get
            Return VB6.CopyArray(m_vExistingNumberingSchemes)
        End Get
        Set(ByVal Value As Object)
            m_vExistingNumberingSchemes = Value
        End Set
    End Property

    Public Property SchemeId() As Integer
        Get
            Return m_lSchemeId
        End Get
        Set(ByVal Value As Integer)
            m_lSchemeId = Value
        End Set
    End Property


    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property


    Public Property Scheme() As Integer
        Get
            Return m_iScheme
        End Get
        Set(ByVal Value As Integer)
            m_iScheme = Value
        End Set
    End Property


    Public Property BusinessTypeDescription() As String
        Get
            Return m_sBusinessTypeDescription
        End Get
        Set(ByVal Value As String)
            m_sBusinessTypeDescription = Value
        End Set
    End Property


    Public Property Code() As String
        Get
            Return m_sCode
        End Get
        Set(ByVal Value As String)
            m_sCode = Value
        End Set
    End Property

    'Maintain Party Code

    Public Property IsReadOnly() As Integer
        Get
            Return m_iIsReadOnly
        End Get
        Set(ByVal Value As Integer)
            m_iIsReadOnly = Value
        End Set
    End Property


    Public Property PartyTypeID() As Integer
        Get
            Return m_lPartyTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lPartyTypeID = Value
        End Set
    End Property
    '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.1)

    Public Property IsResetNumberDaily() As Boolean
        Get
            Return m_bIsResetNumberDaily
        End Get
        Set(ByVal Value As Boolean)
            m_bIsResetNumberDaily = Value
        End Set
    End Property
    '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.1)

    ' PRIVATE Property Procedures (End)


    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer

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

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.

                Dim ctlLookup_NewIndex As Integer = -1
                ctlLookup_NewIndex = ReflectionHelper.Invoke(ReflectionHelper.GetMember(ctlLookup, "Items"), "Add", New Object() {New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CInt(m_vLookupDetails(ACDetailKey, lCntr)))})
                'SP150998 - compare long value not string
                ' Check if this is the selected index.
                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


                        ReflectionHelper.SetMember(ctlLookup, "SelectedIndex", ctlLookup_NewIndex)
                    End If
                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

                'ctlLookup.ListIndex = 0
                ReflectionHelper.SetMember(ctlLookup, "SelectedIndex", 0)
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

            m_lReturn = GetLookupDetails(sLookupTable:=PMLookupBusinessType, ctlLookup:=cboBusinessType)

            ' Check for errors.
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

            ' ***** Mandatory *****************************************

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboBusinessType, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtScheme, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtMask, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringUpper, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSchemeCode, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPartyType, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            ' ***** Non-Mandatory *************************************

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
            m_lReturn = DisplayCaptions()

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

            m_lReturn = SetFirstLastControls()

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

            m_ctlTabFirstLast(ACControlStart, 0) = optGenVal(0)
            m_ctlTabFirstLast(ACControlEnd, 0) = txtAllocateNext

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
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    lblSlot.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACLblNumber, _
            ''        iDataType:=PMResString)
            '
            '    lblDescription.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACLblDescription, _
            ''        iDataType:=PMResString)

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

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function DataToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If m_sMask.ToUpper().IndexOf("U"c) >= 0 Then
                chkResetNumber.Enabled = True
            Else
                chkResetNumber.CheckState = CheckState.Unchecked
                chkResetNumber.Enabled = False
            End If
            If m_sCode = "" Then
                Exit Function
            End If
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtMask, vControlValue:=m_sMask)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSchemeCode, vControlValue:=m_sCode)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_sDescription)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtScheme, vControlValue:=m_iScheme)

            'Populate fields not under FormFields control.
            For i As Integer = 0 To cboBusinessType.ListCount - 1
                cboBusinessType.ListIndex = i
                'Renuka - (WPR87 Paralleling)
                m_bResetMask = True
                If cboBusinessType.ItemId = m_lBusinessType Then
                    'Renuka - (WPR87 Paralleling)
                    m_bResetMask = False
                    Exit For
                End If
            Next i

            'Maintain Party Code
            For i As Integer = 0 To cboPartyType.ListCount - 1
                cboPartyType.ListIndex = i
                If cboPartyType.ItemId = m_lPartyTypeID Then
                    Exit For
                End If
            Next i

            If m_iIsReadOnly Then
                chkIsReadOnly.CheckState = CheckState.Checked
            End If


            If m_bReuse Then
                chkReuse.CheckState = CheckState.Checked
            End If
            '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.3)
            If m_bIsResetNumberDaily Then
                chkIsResetDaily.CheckState = CheckState.Checked
            Else
                chkIsResetDaily.CheckState = CheckState.Unchecked
            End If
            '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.3)
            txtFixedCode.Text = m_sFixedCode
            txtNextNo.Text = CStr(m_lNextNumber)
            txtHighestNo.Text = CStr(m_lHighestNumber)
            txtStep.Text = CStr(m_lStep)
            'Do option button last as it enables/disables other fields.
            If m_bGenerated Then
                optGenVal(0).Checked = 1
            Else
                optGenVal(1).Checked = 1
            End If
            'Start - Renuka - (WPR87 Paralleling)
            If m_bResetNumber Then
                chkResetNumber.CheckState = CheckState.Checked
            End If


            'End - Renuka - (WPR87 Paralleling)

            DisplayNextNumberToAllocate()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_lBusinessType = cboBusinessType.ItemId
            m_sBusinessTypeDescription = cboBusinessType.ItemCaption
            m_sFixedCode = txtFixedCode.Text.Trim()
            If optGenVal(0).Checked Then
                m_bGenerated = 1
            Else
                m_bGenerated = 0
            End If
            m_bReuse = chkReuse.CheckState
            m_lNextNumber = CInt(Conversion.Val(txtNextNo.Text))

            m_lHighestNumber = CInt(Conversion.Val(txtHighestNo.Text))
            m_lStep = CInt(Conversion.Val(txtStep.Text))


            m_iScheme = CInt(m_oFormFields.UnformatControl(ctlControl:=txtScheme))

            m_sMask = CStr(m_oFormFields.UnformatControl(ctlControl:=txtMask))

            m_sCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtSchemeCode))

            m_sDescription = CStr(m_oFormFields.UnformatControl(ctlControl:=txtDescription))

            'Maintain Party Code
            If cboBusinessType.ItemCode.Trim() = "PARTY" Then
                m_iIsReadOnly = chkIsReadOnly.CheckState
                m_lPartyTypeID = cboPartyType.ItemId
            Else
                m_iIsReadOnly = 0
                m_lPartyTypeID = 0
            End If
            '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
            m_bIsResetNumberDaily = chkIsResetDaily.CheckState
            '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_iTask = gPMConstants.PMEComponentAction.PMEdit) Then
                m_lReturn = ValidateNumberingScheme()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Start - Renuka - (WPR87 Paralleling)
            m_bResetNumber = chkResetNumber.CheckState
            'End - Renuka - (WPR87 Paralleling)
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
    '
    ' Name: ValidateNumberingScheme
    '
    ' Description:
    '
    ' History: 20/06/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function ValidateNumberingScheme() As Integer

        Dim result As Integer = 0
        Dim sMsg As String = ""
        Dim sInvalidMaskCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vExistingNumberingSchemes) Then
                Return result
            End If

            'Maintain Party Code
            If m_sBusinessTypeDescription.Trim().ToUpper() = "PARTY CODE" Then
                If Task = gPMConstants.PMEComponentAction.PMAdd Then
                    For iScheme As Integer = m_vExistingNumberingSchemes.GetLowerBound(1) To m_vExistingNumberingSchemes.GetUpperBound(1)
                        If CDbl(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_PARTY_TYPE_ID, iScheme)) = m_lPartyTypeID And CStr(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_DELETED, iScheme)) <> "1" Then
                            MessageBox.Show("This Party Type is already associated", g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            result = gPMConstants.PMEReturnCode.PMFalse
                            cboBusinessType.Focus()
                            Return result
                        End If
                    Next
                End If
                If Task = gPMConstants.PMEComponentAction.PMEdit Then
                    For iScheme As Integer = m_vExistingNumberingSchemes.GetLowerBound(1) To m_vExistingNumberingSchemes.GetUpperBound(1)
                        If CDbl(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_PARTY_TYPE_ID, iScheme)) = m_lPartyTypeID And CDbl(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME_ID, iScheme)) <> m_lSchemeId And CStr(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_IS_DELETED, iScheme)) <> "1" Then
                            MessageBox.Show(m_sDuplicateNumScheme1Msg, g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            result = gPMConstants.PMEReturnCode.PMFalse
                            cboBusinessType.Focus()
                            Return result
                        End If
                    Next
                End If
            End If

            'First, the obvious..
            If m_lNextNumber > m_lHighestNumber Then
                'We will allow them to be equal for the moment.
                '        MsgBox "'Next Number' cannot be greater than 'Highest Number'.", vbExclamation, g_sMsgBoxTitle$
                MessageBox.Show(m_sNextNoGtThanHighestMsg, g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
                txtNextNo.Focus()
                Return result
            End If

            'Scheme must be a unique combination of Mask and Fixed code.
            For iScheme As Integer = m_vExistingNumberingSchemes.GetLowerBound(1) To m_vExistingNumberingSchemes.GetUpperBound(1)
                'Make sure we are not checking record against itself.
                If (Task = gPMConstants.PMEComponentAction.PMAdd) Or (m_lSchemeId <> CDbl(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME_ID, iScheme))) Then
                    '********************************************************************
                    '*** Check Business Type, Scheme combination does not already exist.
                    '********************************************************************
                    If CDbl(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_TYPE, iScheme)) = m_lBusinessType Then
                        If CDbl(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_SCHEME, iScheme)) = m_iScheme Then
                            '                    MsgBox "Numbering Scheme already exists." & vbCrLf & vbCrLf & _
                            ''                        "Please change either the 'Business Type' or 'Numbering Scheme'.", vbExclamation, g_sMsgBoxTitle$
                            MessageBox.Show(m_sDuplicateNumScheme1Msg, g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            result = gPMConstants.PMEReturnCode.PMFalse
                            cboBusinessType.Focus()
                            Return result
                        End If

                        '********************************************************************
                        '*** Check Business Type, Mask,Fixed Code, Number Range combination
                        '*** does not already exist.
                        '********************************************************************
                        'We have already established the BusinessType match above.
                        'Compare Fixed code first as it is the shorter of the two.
                        If CStr(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_FIXED_CODE, iScheme)) = m_sFixedCode Then
                            'If Fixed code already exists, check to see if mask does also.
                            If CStr(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_MASK_CODE, iScheme)) = m_sMask Then
                                'Does numbering range overlap as well ?
                                If WithinRange(m_lNextNumber, CInt(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_NEXT_NUMBER, iScheme)), CInt(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_HIGHEST_NUMBER, iScheme))) Or WithinRange(m_lHighestNumber, CInt(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_NEXT_NUMBER, iScheme)), CInt(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_HIGHEST_NUMBER, iScheme))) Or WithinRange(CInt(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_NEXT_NUMBER, iScheme)), m_lNextNumber, m_lHighestNumber) Or WithinRange(CInt(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_HIGHEST_NUMBER, iScheme)), m_lNextNumber, m_lHighestNumber) Then
                                    '                            MsgBox "Numbering Scheme already exists." & vbCrLf & vbCrLf & _
                                    ''                                "Please change either" & vbCrLf & vbCrLf & _
                                    ''                                "'Business Type'," & vbCrLf & _
                                    ''                                "'Mask ValidateMaskCode'," & vbCrLf &
                                    '                                "'Fixed Code' or " & vbCrLf & _
                                    ''                                "the numbering range." _
                                    ''                                & vbCrLf & "(Exising numbering range is '" & m_vExistingNumberingSchemes(enuNSF_NEXT_NUMBER, iScheme) & "' To '" _
                                    ''                                & m_vExistingNumberingSchemes(enuNSF_HIGHEST_NUMBER, iScheme) & "'.)", vbExclamation, g_sMsgBoxTitle$
                                    If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" And (txtMask.Text.IndexOf("AA") + 1) Then
                                        ' Do Nothing and consider Mask code as valid one
                                    Else
                                        MessageBox.Show(m_sDuplicateNumScheme2Msg & Strings.Chr(13) & Strings.Chr(10) & "(Existing numbering range is '" & CStr(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_NEXT_NUMBER, iScheme)) & "' To '" & CStr(m_vExistingNumberingSchemes(PolicyNumConst.enuNumberingSchemeFields.enuNSF_HIGHEST_NUMBER, iScheme)) & "'.)", g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                                        result = gPMConstants.PMEReturnCode.PMFalse
                                        txtMask.Focus()
                                        Return result
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next iScheme

            'MIPS01 Valid Mask Code for client numbering
            sInvalidMaskCode = "PYA"
            For iLoop1 As Integer = 1 To sInvalidMaskCode.Length
                If txtMask.Text.IndexOf(Mid(sInvalidMaskCode, iLoop1, 1)) >= 0 And cboBusinessType.ItemCode.Trim().ToUpper() = "CLIENT" Then
                    MessageBox.Show("'" & Mid(sInvalidMaskCode, iLoop1, 1) & "' is not a valid 'Mask Code' for 'Client Code'.", g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    txtMask.Focus()
                    Return result
                End If
            Next iLoop1

            'PLICO39 Valid Mask Code for client numbering
            sInvalidMaskCode = "TGILPA"
            For iLoop1 As Integer = 1 To sInvalidMaskCode.Length
                If txtMask.Text.IndexOf(Mid(sInvalidMaskCode, iLoop1, 1)) >= 0 And cboBusinessType.ItemCode.Trim().ToUpper() = "CASE" Then
                    MessageBox.Show("'" & Mid(sInvalidMaskCode, iLoop1, 1) & "' is not a valid 'Mask Code' for 'Case Numbering'.", g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    txtMask.Focus()
                    Return result
                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateNumberingScheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateNumberingScheme", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function DisplayNextNumberToAllocate() As Integer

        Dim result As Integer = 0
        Dim sPreFixedCodeSection, sTmp As String
        Dim sNumberFormat As String = String.Empty
        Dim sNonNumericRemainder As String = String.Empty
        Dim sMask, sFixedCode, sNextNo As String
        Dim lNoOfXs, lNoOf9s, lNextNo As Integer
        Dim sFormat As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Ensure this is for Generated number.
            If optGenVal(1).Checked Then
                Return result
            End If

            sMask = txtMask.Text.Trim()
            sFixedCode = txtFixedCode.Text.Trim()
            sNextNo = txtNextNo.Text.Trim()
            'Check necessary fields have been filled

            If sMask = "" Then
                Return result
            End If

            lNoOfXs = 0
            lNoOf9s = 0

            For lTemp As Integer = 1 To sMask.Length
                Select Case sMask.Substring(lTemp - 1, 1)
                    Case "X"
                        lNoOfXs += 1
                    Case "9"
                        lNoOf9s += 1
                End Select
            Next lTemp

            Dim dbNumericTemp As Double
            If Double.TryParse(sNextNo, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                lNextNo = CInt(sNextNo)
            Else
                lNextNo = (10 ^ lNoOf9s) - 1
            End If

            If lNoOf9s > 0 Then
                sFormat = New String("0", lNoOf9s)
                sNextNo = StringsHelper.Format(lNextNo, sFormat)
            End If

            If lNoOfXs > 0 Then
                sFormat = New String("X", lNoOfXs)
                If sFixedCode.Length > 0 Then
                    If sFixedCode.Length > lNoOfXs Then
                        sFixedCode = sFixedCode.Substring(0, lNoOfXs)
                    Else
                        sFixedCode = sFixedCode & sFormat.Substring(lNoOfXs)
                    End If
                End If
            End If

            lNoOfXs = 0
            lNoOf9s = 0

            For lTemp As Integer = 1 To sMask.Length
                Select Case sMask.Substring(lTemp - 1, 1)
                    Case "X"
                        lNoOfXs += 1
                        'Developer Guide No. 131
                        If (Not (String.IsNullOrEmpty(sFixedCode)) And sFixedCode.Length >= lNoOfXs) Then
                            Mid(sMask, lTemp, 1) = sFixedCode.Substring(lNoOfXs - 1, 1)
                        End If
                    Case "9"
                        lNoOf9s += 1
                        'Developer Guide No. 131
                        If (Not (String.IsNullOrEmpty(sNextNo)) And sNextNo.Length >= lNoOf9s) Then
                            Mid(sMask, lTemp, 1) = sNextNo.Substring(lNoOf9s - 1, 1)
                        End If
                End Select
            Next lTemp

            txtAllocateNext.Text = sMask

            m_iNoOfXsInMask = lNoOfXs
            m_iNoOf9sInMask = lNoOf9s


            '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
            If (sMask <> "") And (sFixedCode <> "") And (sNextNo <> "") Then
                m_iNoOfXsInMask = 0
                m_iNoOf9sInMask = 0
                If txtMask.Text.IndexOf("X"c) >= 0 Then
                    sPreFixedCodeSection = txtMask.Text.Substring(0, txtMask.Text.IndexOf("X"c))

                    'Strip out Fixed Code placeholders from Mask and keep count
                    'of no. of X's for later use.
                    sTmp = txtMask.Text.Substring(txtMask.Text.IndexOf("X"c))
                    Do
                        m_iNoOfXsInMask += 1
                        sTmp = sTmp.Substring(sTmp.IndexOf("X"c) + 1)
                    Loop Until (sTmp.IndexOf("X"c) + 1) = 0

                    'If there is another no-numeric section between Fixed Code
                    '& Numeric sections, strip it out.
                    '                If InStr(sTmp, "9") <> 1 Then
                    If sTmp.IndexOf("9"c) >= 0 Then
                        sNonNumericRemainder = sTmp.Substring(0, sTmp.IndexOf("9"c))
                        sTmp = sTmp.Substring(sTmp.IndexOf("9"c))
                    End If

                    'Establish length of remaining numeric section.
                    For i As Integer = 1 To sTmp.Length
                        m_iNoOf9sInMask += 1
                        sNumberFormat = sNumberFormat & "0"
                    Next i

                    'Construct Next Number to be allocated.
                    txtAllocateNext.Text = sPreFixedCodeSection & txtFixedCode.Text.Trim() & sNonNumericRemainder & StringsHelper.Format(txtNextNo.Text, sNumberFormat)
                Else
                    '
                End If
            End If
            '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)

            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to construct Next Number to be Allocated", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayNextNumberToAllocate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    Private Function WithinRange(ByVal lNumber As Integer, ByVal lLower As Integer, ByVal lUpper As Integer) As Boolean

        Return (lNumber >= lLower) And (lNumber <= lUpper)

    End Function

    Private Function CheckMandatoryFieldsExtra() As Integer

        Dim result As Integer = 0
        Const sMsg As String = "This is a mandatory field. You must enter data in this field"
        Const sMSG_TITLE As String = "Mandatory Control - "

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If cboBusinessType.ListIndex < 1 Then
                MessageBox.Show(sMsg, sMSG_TITLE & "Business Type", MessageBoxButtons.OK, MessageBoxIcon.Error)
                result = gPMConstants.PMEReturnCode.PMFalse
                cboBusinessType.Focus()
                Return result

            ElseIf (optGenVal(0).Checked = 0) And (optGenVal(1).Checked = 0) Then
                MessageBox.Show(sMsg, sMSG_TITLE & "Generated/Validated", MessageBoxButtons.OK, MessageBoxIcon.Error)
                result = gPMConstants.PMEReturnCode.PMFalse
                optGenVal(0).Focus()
                Return result

            ElseIf chkReuse.CheckState = CheckState.Indeterminate Then
                MessageBox.Show(sMsg, sMSG_TITLE & "Reuse abandoned numbers", MessageBoxButtons.OK, MessageBoxIcon.Error)
                result = gPMConstants.PMEReturnCode.PMFalse
                chkReuse.Focus()
                Return result

            ElseIf txtFixedCode.Enabled And txtFixedCode.Text = "" And cboBusinessType.ItemCode.Trim() <> "CLIENT" And cboBusinessType.ItemCode.Trim() <> "PARTY" Then
                MessageBox.Show(sMsg, sMSG_TITLE & "Fixed Code", MessageBoxButtons.OK, MessageBoxIcon.Error)
                result = gPMConstants.PMEReturnCode.PMFalse
                txtFixedCode.Focus()
                Return result

            ElseIf optGenVal(0).Checked Then
                If txtNextNo.Text = "" Then
                    MessageBox.Show(sMsg, sMSG_TITLE & "Next Number", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    txtNextNo.Focus()
                    Return result

                ElseIf txtHighestNo.Text = "" Then
                    MessageBox.Show(sMsg, sMSG_TITLE & "Highest Number", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    txtHighestNo.Focus()
                    Return result

                ElseIf txtStep.Text = "" Then
                    MessageBox.Show(sMsg, sMSG_TITLE & "Step", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    txtStep.Focus()
                    Return result

                End If

            End If

            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check EXTRA mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatoryFieldsExtra", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function ValidateMask(ByVal sMask As String, ByVal bGenerated As Boolean) As Integer
        Dim result As Integer = 0

        Dim oACTPeriod As Object
        Dim sChar As String = ""
        Dim iAscChar As Keys
        Dim arrChars() As String = Nothing
        Dim iPos, iNoOfYsInMask, iCodeLength, icodeStart As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iCharCount As Integer = 1 To sMask.Length
                sChar = sMask.Substring(iCharCount - 1, 1)
                iAscChar = Strings.Asc(sChar(0))
                ReDim Preserve arrChars(iCharCount)
                arrChars(iCharCount) = sChar
                If bGenerated Then
                    Select Case iAscChar
                        Case Keys.D9, Keys.A, Keys.B, Keys.P, Keys.X, Keys.Help, Keys.Insert, Keys.Back, Keys.Tab '47= /, 45 = -.
                            'That's ok, so do nothing.
                        Case Else
                            'error
                    End Select
                Else
                    Select Case iAscChar
                        Case Keys.D9, Keys.X, Keys.Help, Keys.Insert, Keys.Back, Keys.Tab '47 = /, 45 = -.
                            'That's ok, so do nothing.
                        Case Else
                            '                    MsgBox "Only 'X' and '9' are valid characters for a 'Validated' Policy Number.", vbExclamation, g_sMsgBoxTitle$
                            MessageBox.Show(m_sValidCharsForValMsg, g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            Return gPMConstants.PMEReturnCode.PMFalse
                    End Select

                End If
            Next iCharCount
            Dim sOptionValue As String = String.Empty
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOptionValue, v_iSourceID:=g_oObjectManager.SourceID)
            If String.IsNullOrEmpty(sOptionValue) Then
                sOptionValue = "0"
            End If

            'Check characters of same type, other than separaters, are consecutive.
            For iCharCount As Integer = 1 To (arrChars.GetUpperBound(0) - 1)
                'If this is a separater, skip to next char.
                If ((arrChars(iCharCount) <> "/") And (arrChars(iCharCount) <> "-") And (arrChars(iCharCount) <> "_") And (sOptionValue <> "2")) Or (sOptionValue = "2" And (arrChars(iCharCount) <> "-") And (arrChars(iCharCount) <> "_")) Then

                    If (arrChars(iCharCount) = "N") And cboBusinessType.ItemCode.Trim() = "PARTY" Then
                        'do nothing
                    Else
                        'Find position of first instance of char.
                        iPos = (sMask.IndexOf(arrChars(iCharCount)) + 1)
                        Do While Strings.InStr(iPos + 1, sMask, arrChars(iCharCount)) <> 0
                            'Check subsequent instances of char follow each other.
                            If Strings.InStr(iPos + 1, sMask, arrChars(iCharCount)) > iPos + 1 Then
                                '                    MsgBox "Mask Code characters of same type must be consecutive.", vbExclamation, g_sMsgBoxTitle$
                                MessageBox.Show(m_sMaskCharsConsecutiveMsg, g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            iPos += 1
                        Loop
                    End If

                End If
            Next iCharCount
            'Start - (Sankar) - (Tech Spec - VAL P14 - Policy Numbering.doc)
            If sMask.IndexOf("I"c) >= 0 And cboBusinessType.ItemCode = "POLICY" Then
                iPos = (sMask.IndexOf("I"c) + 1)
                icodeStart = iPos
                Do While (Strings.InStr(iPos, sMask, "I") <> 0)
                    iCodeLength += 1
                    iPos += 1
                Loop
                If Not (iPos - 1 = sMask.Length) Then
                    MessageBox.Show(m_sRenewalCodeAtEnd & " - " & Mid(sMask, sMask.IndexOf("I"c) + 1, iCodeLength), g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If iCodeLength <> 2 Then
                    MessageBox.Show(m_sInvalidRenewalCode & " - " & Mid(sMask, sMask.IndexOf("I"c) + 1, iCodeLength), g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'End - (Sankar) - (Tech Spec - VAL P14 - Policy Numbering.doc)

            'If this Number is to be generated, check that the no. of X's identified in the
            'Mask (in DisplayNextNumberToAllocate) match the no. of chars in the Fixed code.
            If (optGenVal(0).Checked) And (m_iNoOfXsInMask <> txtFixedCode.Text.Trim().Length) Then
                '       MsgBox "Number of X's in Mask Code must equal number of characters in Fixed Code", vbExclamation, g_sMsgBoxTitle$
                MessageBox.Show(m_sFixedCodeMatchesPlaceHoldersMsg, g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If this Number is to be generated, check that the no. of 9's identified in the
            'Mask (in DisplayNextNumberToAllocate) is not greater than the no. of digits in the highest number.

            If (CInt(m_iNoOf9sInMask > 0)) Then
                If m_iNoOf9sInMask < txtHighestNo.Text.Trim().Length Then
                    If gPMFunctions.ToSafeInteger(txtHighestNo) = 0 And cboBusinessType.ItemCode.Trim().ToUpper() = "CLIENT" Then
                        'OK
                    Else
                        '   MsgBox "Number of digits in Highest Number cannot be greater than number of placeholders (9's) in Mask Code", vbExclamation, g_sMsgBoxTitle$
                        MessageBox.Show(m_sDigitsinHighestNumMsg, g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
            Dim iNoOfDupsInMask As Integer
            Dim counter As Integer
            If (cboBusinessType.ItemCode.Trim().ToUpper() = "CASE" Or cboBusinessType.ItemCode.Trim().ToUpper() = "FULL CLAIM" Or cboBusinessType.ItemCode.Trim().ToUpper() = "POLICY" Or cboBusinessType.ItemCode.Trim().ToUpper() = "PROV CLAIM" Or cboBusinessType.ItemCode.Trim().ToUpper() = "QUOTE" Or cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" Or cboBusinessType.ItemCode.Trim().ToUpper() = "MEDIAREF") And txtMask.Text.ToUpper().IndexOf("Y"c) >= 0 Then

                iNoOfDupsInMask = NumberOfDuplicatesInMask("Y")
                '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)

                iNoOfYsInMask = 0
                iPos = (txtMask.Text.ToUpper().IndexOf("Y"c) + 1)
                iPos -= 1
                counter = iPos
                For iCharCount As Integer = counter To Strings.Len(txtMask.Text)
                    Do While Strings.InStr(iPos + 1, txtMask.Text.ToUpper(), "Y") <> 0
                        iNoOfYsInMask += 1
                        iPos += 1
                    Loop
                Next
                If iNoOfYsInMask <> 2 And iNoOfYsInMask <> 4 Then
                    MessageBox.Show("The valid mask code for Year can be 'YY' or 'YYYY'", g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)

            If cboBusinessType.ItemCode.Trim().ToUpper() = "MEDIAREF" Then
                iNoOfDupsInMask = NumberOfDuplicatesInMask("D")
                If iNoOfDupsInMask <> 0 And iNoOfDupsInMask <> 2 Then
                    MessageBox.Show("The valid mask code for Day can only be 'DD'", g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                iNoOfDupsInMask = NumberOfDuplicatesInMask("M")
                If iNoOfDupsInMask <> 0 And iNoOfDupsInMask <> 2 Then
                    MessageBox.Show("The valid mask code for Month can only be 'MM'", g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
            'Start - Renuka - (WPR87 Paralleling)
            Dim vResultArray(,) As Object = Nothing
            Dim sYearPattern As String = ""
            If (cboBusinessType.ItemCode.Trim().ToUpper() = "FULL CLAIM" Or cboBusinessType.ItemCode.Trim().ToUpper() = "POLICY" Or cboBusinessType.ItemCode.Trim().ToUpper() = "PROV CLAIM" Or cboBusinessType.ItemCode.Trim().ToUpper() = "QUOTE") And txtMask.Text.ToUpper().IndexOf("U"c) >= 0 Then

                ' Create an object of bACTPeriod and make a call to method GetPeriodYears. If Not Item Found
                ' then  Raise a message and not allow user to proceed
                sYearPattern = "[1-9]###"


                ' Get an instance of the business object via the public object manager.
                Dim temp_oACTPeriod As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_oACTPeriod, "bACTPeriod.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oACTPeriod = temp_oACTPeriod

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Display error stating the problem.
                    MessageBox.Show(m_sBusinessFail, g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=m_sBusinessFail & ": bACTPeriod.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateMask")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = oACTPeriod.GetPeriodYears(vResultArray:=vResultArray)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Call to method bACTPeriod.Form.GetPeriodYears failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateMask")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vResultArray) Then

                    For iRecord As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                        If Not CStr(vResultArray(0, iRecord)).Trim() Like sYearPattern Then
                            ' Accounting Period is not in correct format. Display error
                            MessageBox.Show(m_sInvalidAccountingPeriod, g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Next iRecord
                Else
                    ' Accounting Period is not in correct format. Display error
                    MessageBox.Show(m_sInvalidAccountingPeriod, g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Terminate the business object

		oACTPeriod.Dispose()
                ' Destroy the instance of the business object from memory.
                oACTPeriod = Nothing

            End If
            'End - Renuka - (WPR87 Paralleling)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateMask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateMask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
    Private Function NumberOfDuplicatesInMask(ByVal dupCharacter As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "NumberOfDuplicatesInMask"



        result = gPMConstants.PMEReturnCode.PMTrue

        result = 0

        Dim iPos As Integer = (txtMask.Text.IndexOf(dupCharacter, StringComparison.CurrentCultureIgnoreCase) + 1)

        If iPos = 0 Then
            Return result
        End If

        For iCharCount As Integer = iPos To Strings.Len(txtMask.Text)
            If Mid(txtMask.Text, iCharCount, 1) = dupCharacter Then
                result += 1
            Else
                Return result
            End If
        Next

        GoTo Finally_Renamed



        iPMFunc.LogError(v_sUsername:="sirius", v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:

        Return result
    End Function
    '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.4.1.2)
    Public Sub ProcessMaskKeyPress(ByRef iKeyAscii As Integer, ByRef bGenerated As Boolean)
        'Routine to ensure only correct characters are entered in mask.

        ' JMK 15/10/2001 - allow "L" and "R" - claims only

        Dim sBusType As String = ""
        Try
            ' JMK get the business type
            sBusType = cboBusinessType.ItemCaption
            Dim sOptionValue As String = String.Empty
            If iKeyAscii = 47 Then
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOptionValue, v_iSourceID:=g_oObjectManager.SourceID)
                If String.IsNullOrEmpty(sOptionValue) Then
                    sOptionValue = "0"
                End If
            End If

            If bGenerated Then
                Select Case iKeyAscii
                    Case 57, 65, 66, 80, 88, 47, 45, 8, 9 '9, A, B, P, X, /, -, backspace, tab
                        'That's ok, so do nothing. Now allow for PARTY - 9, A, B, X
                        If iKeyAscii = 47 And sOptionValue = "2" Then
                            Exit Select
                        End If
                        If (cboBusinessType.ItemCode.Trim().ToUpper() = "CLIENT" Or cboBusinessType.ItemCode.Trim().ToUpper() = "CASE") And (iKeyAscii = 80 Or iKeyAscii = 65) Then
                            iKeyAscii = 0
                        End If

                        If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" Then
                            If iKeyAscii = 57 Or iKeyAscii = 65 Or iKeyAscii = 66 Or iKeyAscii = 88 Or iKeyAscii = 8 Or iKeyAscii = 47 Or iKeyAscii = 45 Then
                            Else
                                iKeyAscii = 0
                            End If
                        End If
                        '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.8)
                        If cboBusinessType.ItemCode.Trim().ToUpper() = "MEDIAREF" Then
                            If iKeyAscii = 57 Or iKeyAscii = 65 Or iKeyAscii = 66 Or iKeyAscii = 88 Or iKeyAscii = 8 Or iKeyAscii = 47 Or iKeyAscii = 45 Then
                            Else
                                iKeyAscii = 0
                            End If
                        End If
                        '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.8)
                    Case 95
                        If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" Then
                            If txtMask.Text.IndexOf("_"c) >= 0 Then 'Only one underscore allowed
                                iKeyAscii = 0
                            End If
                        Else
                            iKeyAscii = 0
                        End If
                    Case 76 ', 82
                        If (sBusType.ToUpper().IndexOf("CLAIM") + 1) Or cboBusinessType.ItemCode.Trim().ToUpper() = "CLIENT" Or cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" Then 'L, R. (claims only)
                            If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" Then
                                If cboPartyType.ItemCode.Trim().ToUpper() = "PC" Or cboPartyType.ItemCode.Trim().ToUpper() = "AH" Or cboPartyType.ItemCode.Trim().ToUpper() = "CO" Or cboPartyType.ItemCode.Trim().ToUpper() = "HC" Then 'Personal Client, Accounts Handler/Executive
                                    iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                                Else
                                    iKeyAscii = 0
                                End If
                            End If
                            'OK
                        Else
                            iKeyAscii = 0
                        End If
                    Case 82
                        If sBusType.ToUpper().IndexOf("CLAIM") + 1 Then
                            'OK
                        Else
                            iKeyAscii = 0
                        End If

                    Case 97, 98, 112, 120 'a, b, p, x.
                        'Change to upper case.
                        If (cboBusinessType.ItemCode.Trim().ToUpper() = "CLIENT" Or cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" Or cboBusinessType.ItemCode.Trim().ToUpper() = "CASE") And (iKeyAscii = 112 Or iKeyAscii = 97) Then
                            If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" And iKeyAscii = 112 Then
                                iKeyAscii = 0
                            Else
                                iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                            End If
                            '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.8)
                            If cboBusinessType.ItemCode.Trim().ToUpper() = "MEDIAREF" And (iKeyAscii = 97 Or iKeyAscii = 112) Then
                                iKeyAscii = 0
                            Else
                                iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                            End If
                            '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.8)
                        Else
                            iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                        End If

                    Case 108 ', 114
                        If (sBusType.ToUpper().IndexOf("CLAIM") + 1) Or cboBusinessType.ItemCode.Trim().ToUpper() = "CLIENT" Or cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" Then 'l, r. (claims only)

                            If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" Then
                                If cboPartyType.ItemCode.Trim().ToUpper() = "PC" Or cboPartyType.ItemCode.Trim().ToUpper() = "AH" Or cboPartyType.ItemCode.Trim().ToUpper() = "CO" Or cboPartyType.ItemCode.Trim().ToUpper() = "HC" Then 'Personal Client, Accounts Handler/Executive
                                    iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                                Else
                                    iKeyAscii = 0
                                End If
                            Else
                                iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                            End If

                        Else
                            iKeyAscii = 0
                        End If
                    Case 70, 102 'F,f - PARTY
                        If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" Then
                            If cboPartyType.ItemCode.Trim().ToUpper() = "PC" Or cboPartyType.ItemCode.Trim().ToUpper() = "AH" Or cboPartyType.ItemCode.Trim().ToUpper() = "CO" Or cboPartyType.ItemCode.Trim().ToUpper() = "HC" Then 'Personal Client, Accounts Handler/Executive
                                iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                            Else
                                iKeyAscii = 0
                            End If
                        Else
                            iKeyAscii = 0
                        End If
                    Case 78, 110 'N,n - PARTY
                        If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" Then
                            If cboPartyType.ItemCode.Trim().ToUpper() = "PC" Or cboPartyType.ItemCode.Trim().ToUpper() = "AH" Or cboPartyType.ItemCode.Trim().ToUpper() = "CO" Or cboPartyType.ItemCode.Trim().ToUpper() = "HC" Then 'Personal Client, Accounts Handler/Executive
                                iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                            Else
                                iKeyAscii = 0
                            End If
                        Else
                            iKeyAscii = 0
                        End If
                    Case 114
                        If sBusType.ToUpper().IndexOf("CLAIM") + 1 Then
                            'OK
                        Else
                            iKeyAscii = 0
                        End If
                    Case 103, 105, 116, 73 ' 73 for I                                      'g,i,t(client only)
                        If cboBusinessType.ItemCode.Trim().ToUpper() = "CLIENT" Or cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" Then
                            If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" And iKeyAscii = 103 Then 'g not allowed in PARTY
                                iKeyAscii = 0
                            End If

                            If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" And iKeyAscii = 105 Then 'i
                                If cboPartyType.ItemCode.Trim().ToUpper() = "PC" Or cboPartyType.ItemCode.Trim().ToUpper() = "AH" Or cboPartyType.ItemCode.Trim().ToUpper() = "CO" Or cboPartyType.ItemCode.Trim().ToUpper() = "HC" Then 'Personal Client, Accounts Handler/Executive
                                    iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                                Else
                                    iKeyAscii = 0
                                End If
                            End If

                            If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" And iKeyAscii = 116 Then 't
                                If cboPartyType.ItemCode.Trim().ToUpper() = "PC" Or cboPartyType.ItemCode.Trim().ToUpper() = "AH" Or cboPartyType.ItemCode.Trim().ToUpper() = "CO" Or cboPartyType.ItemCode.Trim().ToUpper() = "HC" Then 'Personal Client, Accounts Handler/Executive
                                    iKeyAscii = 0
                                Else
                                    iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                                End If
                            End If


                            ''Start(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (5.1.1)
                        ElseIf cboBusinessType.ItemCode.Trim().ToUpper() = "POLICY" And (iKeyAscii = 105 Or iKeyAscii = 73) Then
                            ' Do Nothing
                        Else
                            iKeyAscii = 0
                        End If
                        ''End(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (5.1.1)


                    Case 73, 84 'I,T (Client Only) Stop G  for client code PN 46618
                        If cboBusinessType.ItemCode.Trim().ToUpper() = "CLIENT" Or cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" Then
                            If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" And iKeyAscii = 71 Then 'G not allowed in PARTY
                                iKeyAscii = 0
                            End If

                            If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" And iKeyAscii = 73 Then 'I
                                If cboPartyType.ItemCode.Trim().ToUpper() = "PC" Or cboPartyType.ItemCode.Trim().ToUpper() = "AH" Or cboPartyType.ItemCode.Trim().ToUpper() = "CO" Or cboPartyType.ItemCode.Trim().ToUpper() = "HC" Then 'Personal Client, Accounts Handler/Executive
                                    iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                                Else
                                    iKeyAscii = 0
                                End If
                            End If

                            If cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" And iKeyAscii = 84 Then 'T
                                If cboPartyType.ItemCode.Trim().ToUpper() = "PC" Or cboPartyType.ItemCode.Trim().ToUpper() = "AH" Or cboPartyType.ItemCode.Trim().ToUpper() = "CO" Or cboPartyType.ItemCode.Trim().ToUpper() = "HC" Then 'Personal Client, Accounts Handler/Executive
                                    iKeyAscii = 0
                                Else
                                    iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                                End If
                            End If

                        Else
                            iKeyAscii = 0
                        End If
                    Case 121 'y (case only)
                        If cboBusinessType.ItemCode.Trim().ToUpper() = "CASE" Or cboBusinessType.ItemCode.Trim().ToUpper() = "FULL CLAIM" Or cboBusinessType.ItemCode.Trim().ToUpper() = "POLICY" Or cboBusinessType.ItemCode.Trim().ToUpper() = "PROV CLAIM" Or cboBusinessType.ItemCode.Trim().ToUpper() = "PARTY" Or cboBusinessType.ItemCode.Trim().ToUpper() = "QUOTE" Then

                            iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                        Else
                            iKeyAscii = 0
                        End If
                    Case 89 'Y (case only
                        '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.8)
                        'Note:"MEDIAREF" checking is added
                        If cboBusinessType.ItemCode.Trim().ToUpper() <> "CASE" And cboBusinessType.ItemCode.Trim().ToUpper() <> "FULL CLAIM" And cboBusinessType.ItemCode.Trim().ToUpper() <> "POLICY" And cboBusinessType.ItemCode.Trim().ToUpper() <> "PROV CLAIM" And cboBusinessType.ItemCode.Trim().ToUpper() <> "PARTY" And cboBusinessType.ItemCode.Trim().ToUpper() <> "QUOTE" And cboBusinessType.ItemCode.Trim().ToUpper() <> "MEDIAREF" Then
                            '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)- (5.2.2.8)
                            iKeyAscii = 0
                        End If
                        '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.8)
                    Case 68 ' D
                        If cboBusinessType.ItemCode.Trim().ToUpper() <> "MEDIAREF" Then
                            iKeyAscii = 0
                        End If
                    Case 100 ' d
                        If cboBusinessType.ItemCode.Trim().ToUpper() <> "MEDIAREF" Then
                            iKeyAscii = 0
                        Else
                            iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                        End If
                    Case 77 ' M
                        If cboBusinessType.ItemCode.Trim().ToUpper() <> "MEDIAREF" Then
                            iKeyAscii = 0
                        End If
                    Case 109 ' m
                        If cboBusinessType.ItemCode.Trim().ToUpper() <> "MEDIAREF" Then
                            iKeyAscii = 0
                        Else
                            iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                        End If
                        '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.8)
                        'Start - Renuka - (WPR87 Paralleling)
                    Case 85 'U (case only
                        If cboBusinessType.ItemCode.Trim().ToUpper() <> "FULL CLAIM" And cboBusinessType.ItemCode.Trim().ToUpper() <> "POLICY" And cboBusinessType.ItemCode.Trim().ToUpper() <> "PROV CLAIM" And cboBusinessType.ItemCode.Trim().ToUpper() <> "QUOTE" Then
                            iKeyAscii = 0
                        End If
                        'End - Renuka - (WPR87 Paralleling)
                    Case 69 'E (case, full claim, provisional claim, quote and policy only)
                        'Enhancement PM041940 - WPR63_Add the State to the Numbering Scheme (Jai 21/04/2015)


                        Select Case cboBusinessType.ItemCode.Trim().ToUpper()
                            Case "FULL CLAIM", "POLICY", "PROV CLAIM", "QUOTE", "CASE"
                                If txtMask.Text.IndexOf("EE") >= 0 Then
                                    iKeyAscii = 0
                                End If
                            Case Else
                                iKeyAscii = 0
                        End Select
                    Case Else
                        iKeyAscii = 0
                End Select
            Else
                Select Case iKeyAscii
                    Case 57, 88, 47, 45, 8, 9 '9, X, /, -, backspace, tab.
                        If iKeyAscii = 47 And sOptionValue = "2" Then
                            iKeyAscii = 0
                        End If
                        'That's ok, so do nothing.
                    Case 120 ' x.
                        'Change to upper case.
                        iKeyAscii = Strings.Asc(Strings.Chr(iKeyAscii).ToString().ToUpper()(0))
                    Case Else
                        iKeyAscii = 0
                End Select

            End If

            'Start - Renuka - (WPR87 Paralleling)
            'Restrict the maximum number of 'U' entered to 4
            Dim iLength As Integer
            For iCnt As Integer = 1 To Strings.Len(txtMask.Text)
                If Mid(txtMask.Text, iCnt, 1).ToUpper() = "U" Then
                    iLength += 1
                End If
            Next
            If iLength >= 4 And iKeyAscii = 85 Then
                iKeyAscii = 0
            End If
            'End - Renuka - (WPR87 Paralleling)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process KeyPress for Mask Code", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessMaskKeyPress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub TextGotFocus(ByRef txtBox As TextBox)

        txtBox.SelectionStart = 0
        txtBox.SelectionLength = Strings.Len(txtBox.Text)

    End Sub

    Private Function GetMessages() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Developer Guide No. 243
            m_sNextNoGtThanHighestMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNextGtThanHighest, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            m_sDuplicateNumScheme1Msg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDuplicateScheme1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            m_sDuplicateNumScheme2Msg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDuplicateScheme2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            m_sSelGenValMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelGenVal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            m_sNumSchemeLimitsMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNumSchemeLimit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            m_sValidCharsForValMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACValidCharsVal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            m_sMaskCharsConsecutiveMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMaskCharsConsecutive, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            m_sFixedCodeMatchesPlaceHoldersMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMask_FixedCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            m_sDigitsinHighestNumMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHighestNumberDigits, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'Start -(Sankar) - (Tech Spec - VAL P14 - Policy Numbering.doc)

            'Developer Guide No. 243
            m_sRenewalCodeAtEnd = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRenewalCodeAtEnd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            m_sInvalidRenewalCode = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInValidRenewalCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'End -(Sankar) - (Tech Spec - VAL P14 - Policy Numbering.doc)
            'Start - Renuka - (WPR87 Paralleling)

            'Developer Guide No. 243
            m_sInvalidAccountingPeriod = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInvalidAccountingPeriodFormat, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No. 243
            m_sBusinessFail = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'End - Renuka - (WPR87 Paralleling)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Messages from resource file", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMessages", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboBusinessType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBusinessType.Click
        'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
        'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()

        Try

            If cboBusinessType.ItemCode.Trim() = "PARTY" Then
                cboPartyType.Enabled = True
                chkIsReadOnly.Enabled = True
                lblPartyType.Enabled = True
                m_oFormFields.Item("cboPartyType-0").IsMandatory = True

            Else
                cboPartyType.ItemId = 0
                chkIsReadOnly.CheckState = CheckState.Unchecked
                cboPartyType.Enabled = False
                chkIsReadOnly.Enabled = False
                lblPartyType.Enabled = False
                m_oFormFields.Item("cboPartyType-0").IsMandatory = False
            End If

            'Set the FontBold of lblFixedCode to True for BusinessType other than "CLIENT" and "PARTY"
            'to indicate "Fixed code" as mandatory field.
            If cboBusinessType.ItemCode.Trim() <> "CLIENT" And cboBusinessType.ItemCode.Trim() <> "PARTY" And cboBusinessType.ItemCode.Trim() <> "" Then
                lblFixedCode.Font = VB6.FontChangeBold(lblFixedCode.Font, True)
            Else
                lblFixedCode.Font = VB6.FontChangeBold(lblFixedCode.Font, False)
            End If
            '(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.2)
            If cboBusinessType.ItemCode.Trim() = "MEDIAREF" Then
                chkIsResetDaily.Enabled = True
                chkIsReadOnly.Enabled = False
                chkIsReadOnly.CheckState = CheckState.Unchecked
                optGenVal(1).Enabled = False
                chkReuse.Enabled = False
            Else
                chkIsResetDaily.Enabled = False
                'chkIsReadOnly.Enabled = True
                optGenVal(1).Enabled = True
                chkReuse.Enabled = True
                chkIsResetDaily.CheckState = CheckState.Unchecked
            End If
            '(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(5.2.2.2)
            'Start - Renuka - (WPR87 Paralleling)
            If cboBusinessType.ItemCode.Trim().ToUpper() <> "QUOTE" And cboBusinessType.ItemCode.Trim().ToUpper() <> "POLICY" And cboBusinessType.ItemCode.Trim().ToUpper() <> "PROV CLAIM" And cboBusinessType.ItemCode.Trim().ToUpper() <> "FULL CLAIM" Then
                If txtMask.Text.Trim().ToUpper().IndexOf("U"c) + 1 Then
                    If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                        txtMask.Text = ""
                    End If
                    If m_bResetMask Then
                        'Do nothing
                    ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                        txtMask.Text = ""
                    End If
                End If
            End If
            'End - Renuka - (WPR87 Paralleling)

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Business Type Combo Click", vApp:=ACApp, vClass:=ACClass, vMethod:="cboBusinessType_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub





        End Try

    End Sub

    Private Sub cboPartyType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPartyType.Click
        If cboBusinessType.ItemCode.Trim() = "PARTY" And m_bClearMask Then
            txtMask.Text = ""
            txtFixedCode.Text = ""
            txtNextNo.Text = ""
            txtHighestNo.Text = ""
            txtStep.Text = ""
            txtAllocateNext.Text = ""
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            '    If MsgBox("Cancelling will lose all of your current details." & vbCrLf & _
            ''            "Do you really wish to cancel ?", _
            ''            vbYesNo + vbDefaultButton2 + vbQuestion, g_sMsgBoxTitle$) = vbYes Then
            If MessageBox.Show(g_sCancelMsg, g_sMsgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

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
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, MainModule.DetailScreenHelpID)


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

            'Check mandatory fields for controls FormFields does not deal with properly.
            m_lReturn = CheckMandatoryFieldsExtra()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If ValidateMask(txtMask.Text, optGenVal(0).Checked) <> gPMConstants.PMEReturnCode.PMTrue Then
                txtMask.Focus()
                Exit Sub
            End If

            m_lReturn = InterfaceToData()

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

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmDetail_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
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

            Exit Sub
        End Try


    End Sub


    Private Sub frmDetail_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try


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

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}



            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            'Developer Guide No. added code
            cboPartyType.Visible = True
            lblPartyType.Visible = True
            'Developer Guide No. 
            Me.cboBusinessType.FirstItem = "(Select Business Type)"
            Me.cboPartyType.FirstItem = "(Select Party Type)"
            'After executing the above two lines the controls chkIsReadOnly,cboPartyType,chkIsResetDaily,lblPartyType was getting disabled.So these are made enabled again.
            chkIsReadOnly.Enabled = True
            cboPartyType.Enabled = True
            chkIsResetDaily.Enabled = True
            lblPartyType.Enabled = True

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = DataToInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_bClearMask = True

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private Const vbFormControlMenu As Integer = 0
    Private Sub frmDetail_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'PN: 48012
            'Check if the close button of form was clicked
            'then it will behave like cancel button click.


            If UnloadMode <> vbFormControlMenu Then
                If MessageBox.Show(MainModule.g_sCancelMsg, MainModule.g_sMsgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
                    ' Set the interface status.
                    m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                Else
                    ' Reset the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                    Cancel = True
                    eventArgs.Cancel = True
                    Exit Sub
                End If
            End If

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



    Private isInitializingComponent As Boolean
    Private Sub optGenVal_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optGenVal_1.CheckedChanged, _optGenVal_0.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            Dim Index As Integer = Array.IndexOf(optGenVal, eventSender)

            'Reuse is only allowed when the number is generated.
            If Index = 0 Then
                chkReuse.Enabled = True
                txtFixedCode.Enabled = True
                lblFixedCode.Enabled = True
                txtNextNo.Enabled = True
                lblNextNo.Enabled = True
                txtHighestNo.Enabled = True
                lblHighestNo.Enabled = True
                txtStep.Enabled = True
                lblStep.Enabled = True
            Else
                chkReuse.Enabled = False
                chkReuse.CheckState = CheckState.Unchecked
                txtFixedCode.Enabled = False
                lblFixedCode.Enabled = False
                txtFixedCode.Text = ""
                txtNextNo.Enabled = False
                txtNextNo.Text = ""
                lblNextNo.Enabled = False
                txtHighestNo.Enabled = False
                txtHighestNo.Text = ""
                lblHighestNo.Enabled = False
                txtStep.Enabled = False
                txtStep.Text = ""
                lblStep.Enabled = False
                txtAllocateNext.Text = ""
            End If

        End If
    End Sub

    Private Sub txtAllocateNext_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAllocateNext.Enter
        TextGotFocus(txtAllocateNext)
        '    m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAllocateNext)
    End Sub

    Private Sub txtAllocateNext_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAllocateNext.Leave
        '    m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAllocateNext)
    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDescription)
    End Sub

    Private Sub txtFixedCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFixedCode.Enter
        TextGotFocus(txtFixedCode)
        '    m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtFixedCode)
    End Sub

    Private Sub txtFixedCode_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtFixedCode.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'Change entry to upper case.

        KeyAscii = Strings.Asc(Strings.Chr(KeyAscii).ToString().ToUpper()(0))
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtFixedCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFixedCode.Leave
        '    m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtFixedCode)

        DisplayNextNumberToAllocate()

    End Sub

    Private Sub txtHighestNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtHighestNo.Enter
        TextGotFocus(txtHighestNo)
        '    m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtHighestNo)
    End Sub

    Private Sub txtHighestNo_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtHighestNo.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'Only allow numerics.
        If KeyAscii <> CInt(Keys.Back) Then
            If (KeyAscii < Keys.D0) Or (KeyAscii > Keys.D9) Then
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtHighestNo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtHighestNo.Leave
        '    m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtHighestNo)
    End Sub

    Private Sub txtMask_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMask.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMask)
    End Sub
    Private Sub txtMask_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtMask.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii <> 9 Then 'tab
            If (optGenVal(0).Checked = 0) And (optGenVal(1).Checked = 0) Then
                '            MsgBox "Please select Generated/Validated before entering Mask Code", vbExclamation, g_sMsgBoxTitle$
                MessageBox.Show(m_sSelGenValMsg, g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                KeyAscii = 0
                optGenVal(0).Focus()
            Else
                If Strings.Asc(Strings.Chr(KeyAscii).ToString().ToUpper()(0)) = Strings.Asc("9"c) Then
                    checkNumericMaskLimit(KeyAscii)
                End If

                ProcessMaskKeyPress(KeyAscii, optGenVal(0).Checked)
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtMask_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMask.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMask)

        DisplayNextNumberToAllocate()
        'Start - Renuka - (WPR87 Paralleling)
        'Restrict the maximum number of 'U' entered to 4
        Dim iLength As Integer
        For iCnt As Integer = 1 To Strings.Len(txtMask.Text)
            If Mid(txtMask.Text, iCnt, 1).ToUpper() = "U" Then
                iLength += 1
            End If
        Next
        If iLength = 0 And chkResetNumber.Enabled Then
            chkResetNumber.CheckState = CheckState.Unchecked
            chkResetNumber.Enabled = False
        End If
        If iLength > 0 And Not chkResetNumber.Enabled Then
            chkResetNumber.Enabled = True
        End If
        'End - Renuka - (WPR87 Paralleling)

    End Sub

    Private Sub checkNumericMaskLimit(ByRef iKeyAscii As Integer)

        Dim lNoOf9s As Integer = 0
        Dim sMask As String = txtMask.Text.Trim()

        For lTemp As Integer = 1 To sMask.Length
            Select Case sMask.Substring(lTemp - 1, 1)
                Case "9"
                    lNoOf9s += 1
            End Select
        Next lTemp

        ' The no of 9s should not be more than nine. We check here for 8
        ' as the check is performed before the number is rendered in textbox
        If lNoOf9s > 8 Then
            iKeyAscii = 0
            Exit Sub
        End If
    End Sub

    Private Sub txtNextNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtNextNo.Enter
        TextGotFocus(txtNextNo)
        '    m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtNextNo)
    End Sub

    Private Sub txtNextNo_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtNextNo.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'Only allow numerics.
        If KeyAscii <> CInt(Keys.Back) Then
            If (KeyAscii < Keys.D0) Or (KeyAscii > Keys.D9) Then
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtNextNo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtNextNo.Leave
        '    m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtNextNo)
        DisplayNextNumberToAllocate()
    End Sub

    Private Sub txtScheme_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtScheme.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtScheme)
    End Sub

    Private Sub txtScheme_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtScheme.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'Only allow numerics.
        If KeyAscii <> CInt(Keys.Back) Then
            If (KeyAscii < Keys.D0) Or (KeyAscii > Keys.D9) Then
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtScheme_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtScheme.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtScheme)
        If Conversion.Val(txtScheme.Text) > 255 Then
            '        MsgBox "Numbering Scheme cannot be greater than 255", vbExclamation, g_sMsgBoxTitle$
            MessageBox.Show(m_sNumSchemeLimitsMsg, g_sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtScheme.Text = ""
            txtScheme.Focus()
        End If

    End Sub

    Private Sub txtSchemeCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeCode.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSchemeCode)
    End Sub

    Private Sub txtSchemeCode_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtSchemeCode.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'No special character allowed
        If (KeyAscii >= 32 And KeyAscii <= 47) Or (KeyAscii >= 58 And KeyAscii <= 64) Or (KeyAscii >= 91 And KeyAscii <= 96) Or (KeyAscii >= 123 And KeyAscii <= 126) Then
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtSchemeCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSchemeCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSchemeCode)
    End Sub

    Private Sub txtStep_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStep.Enter
        TextGotFocus(txtStep)
        '    m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtStep)
    End Sub

    Private Sub txtStep_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtStep.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'Only allow numerics.
        If KeyAscii <> CInt(Keys.Back) Then
            If (KeyAscii < Keys.D0) Or (KeyAscii > Keys.D9) Then
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtStep_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStep.Leave
        '    m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtStep)
    End Sub

    Private Sub tabMainTab_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMainTab.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub


End Class
