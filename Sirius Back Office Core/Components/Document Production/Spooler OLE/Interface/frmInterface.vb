Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
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

    Private Const vbFormCode As Integer = 0
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

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sStepStatus As String = ""

    ' {* USER DEFINED CODE (Begin) *}

    Private m_lDocNumber As Integer
    Private m_sFileName As String = ""
    Private m_lMode As Integer
    Private m_lPartyCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    'DC180401 as requested by Dave Newson (Documaster)
    Private m_lInsuranceFolderCnt As Integer
    'DC180401
    Private m_sDescription As String = ""
    'DC110203 -ISS1460 -cater for archiving claims documents
    Private m_lClaimCnt As Integer

    'MKW060503 PN1839 START
    Private m_sPartyName As String = ""
    Private m_sInsuranceFileRef As String = ""
    Private m_sClaimRef As String = ""
    'MKW060503 PN1839 END

    'Holds what class of OLE we're using
    Private m_sClass As String = ""
    'Allowed values are:
    'Word.Document.8 for Word 97
    'Word.Document.9 for Word 2000

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBSpoolerOLE.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRDocTemplate.Business

    'UPGRADE_ISSUE: (2068) bSIRDOCAPI.Form object was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2068.aspx
    Private m_oSIRDOCAPI As bSIRDOCAPI.Form
    'Private m_oSIRDOCAPI As bSIRDOCAPI.Form

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

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


    ' PUBLIC Property Procedures (Begin)

    Public Property DocNumber() As Integer
        Get
            Return m_lDocNumber
        End Get
        Set(ByVal Value As Integer)
            m_lDocNumber = Value
        End Set
    End Property

    Public Property FileName() As String
        Get
            Return m_sFileName
        End Get
        Set(ByVal Value As String)
            m_sFileName = Value
        End Set
    End Property

    Public Property Mode() As Integer
        Get
            Return m_lMode
        End Get
        Set(ByVal Value As Integer)
            m_lMode = Value
        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    'DC180401 as requested by Dave Newson (Documaster)
    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property
    'DC180401
    'DC110203 -ISS1460 -start -cater for archiving claims documents
    Public Property ClaimCnt() As Integer
        Get
            Return m_lClaimCnt
        End Get
        Set(ByVal Value As Integer)
            m_lClaimCnt = Value
        End Set
    End Property
    'DC110203 -end

    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
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

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
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
    Public Function GetBusiness() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            '    m_lReturn& = m_oBusiness.GetDetails(vLockMode:=PMNoLock, _
            ''                                        vDocumentTemplateId:=m_lDocumentTemplateId)
            '
            '    ' {* USER DEFINED CODE (End) *}
            '
            '    ' Check for errors
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to get details.
            '        GetBusiness = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to get details from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetBusiness"
            '
            '        Exit Function
            '    End If

            'MKW060503 PN1839 START
            'UPGRADE_TODO: (1067) Member GetInformation is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_lReturn = m_oBusiness.GetInformation(lPartyCnt:=m_lPartyCnt, lInsuranceFolderCnt:=m_lInsuranceFolderCnt, lClaimCnt:=m_lClaimCnt, sPartyName:=m_sPartyName, sInsuranceFileRef:=m_sInsuranceFileRef, sClaimRef:=m_sClaimRef, lInsuranceFileCnt:=m_lInsuranceFileCnt)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If
            'MKW060503 PN1839 END

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
    Public Function BusinessToInterface() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

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
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

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
    Public Function DisplayLookupDetails() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try


            ' Get the lookup values.

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

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetOLE
    '
    ' Description: sets the ole container
    '
    ' ***************************************************************** '
    Public Function SetOLE() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sClient As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'UPGRADE_ISSUE: (2064) Ole method OLE1.CreateLink was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            'Sumeet
            'OLE1.CreateLink(m_sFileName)

            'UPGRADE_ISSUE: (2064) Ole property OLE1.Class was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            'Sumeetm_sClass = OLE1.Class

            'UPGRADE_ISSUE: (2064) Ole property OLE1.AppIsRunning was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            'SumeetIf OLE1.AppIsRunning Then
            'UPGRADE_ISSUE: (2064) Ole property OLE1.AppIsRunning was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            'SumeetOLE1.AppIsRunning = False
            'SumeetClose()
            'SumeetEnd If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the ole container", vApp:=ACApp, vClass:=ACClass, vMethod:="SetOLE", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function BusinessToData() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            '    m_lReturn& = m_oBusiness.GetNext( _
            'vDocumentTemplateId:=m_lDocumentTemplateId, vCode:=m_sDocumentTemplateCode, _
            'vDescription:=m_sDocumentTemplateDescription, vSourceId:=m_lSourceId, vDocumentTypeId:=m_lDocumentTypeId, _
            'vIsDeleted:=m_iIsDeleted, vSlotNumber:=m_vSlotNumber, vRiskCodeId:=m_vRiskCodeId, _
            'vRiskGroupId:=m_vRiskGroupId, vIsEditableAfterMerging:=m_iIsEditableAfterMerging)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        BusinessToData = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to retrieve the details from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="BusinessToData"
            '    End If

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
    Private Function InterfaceToData() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
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
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

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
    Private Function SetInterfaceDefaults() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            'CenterForm Me

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
    Private Function SetFirstLastControls() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
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

            m_ctlTabFirstLast(ACControlStart, 0) = OLE1
            m_ctlTabFirstLast(ACControlEnd, 0) = OLE1

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
    'UPGRADE_NOTE: (7001) The following declaration (DisplayCaptions) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DisplayCaptions() As gPMConstants.PMEReturnCode
    '
    'Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
    'Try 
    '
    '
    ' Display all language specific captions.
    '
    '    Me.Caption = GetResData( _
    ''        iLangID:=g_iLanguageID%, _
    ''        lID:=ACInterfaceTitle, _
    ''        iDataType:=PMResString)
    ''
    '    ' Check for an error.
    '    If (Me.Caption = "") Then
    '        ' Failed to get data from the resource file.
    '        DisplayCaptions = PMFalse
    ''
    '        ' Log Error.
    '        LogMessage _
    ''            iType:=PMLogError, _
    ''            sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
    ''            "Please check the file exists and the correct captions are available", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="DisplayCaptions"
    ''
    '        Exit Function
    '    End If
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
    '    lblDesc.Caption = GetResData( _
    ''        iLangID:=g_iLanguageID%, _
    ''        lID:=ACDesc, _
    ''        iDataType:=PMResString)
    ''
    ' NOTE: Replace this section with your new code.
    ' ************************************************************
    '
    ' {* USER DEFINED CODE (End) *}
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessForm
    '
    ' Description:
    '
    ' History: 08/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function ProcessForm() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = SetOLE()

            Select Case m_lMode
                Case 1
                    m_lReturn = ArchiveDocument()
                Case 2
                    m_lReturn = EditDocument()
                Case 3
                    m_lReturn = MailDocument()
                Case 4
                    m_lReturn = PrintDocument()
            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'UPGRADE_TODO: (1065) Error handling statement (Resume) could not be converted. More Information: http://www.vbtonet.com/ewis/ewi1065.aspx
            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ArchiveDocument
    '
    ' Description:
    '
    ' History: 08/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function ArchiveDocument() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add it to FileMaster

            m_lReturn = UpdateFileMaster()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ArchiveDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDocument
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function EditDocument() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'UPGRADE_ISSUE: (2064) Ole property OLE1.AppIsRunning was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            'SumeetIf Not OLE1.AppIsRunning Then
            'This one causes a Dr Watson error...
            'OLE1.DoVerb vbOLEInPlaceActivate
            'UPGRADE_ISSUE: (2070) Constant vbOLEShow was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2070.aspx
            'UPGRADE_ISSUE: (2064) Ole method OLE1.DoVerb was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            'SumeetOLE1.DoVerb(vbOLEShow)

            Timer1.Enabled = True

            'SumeetEnd If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: MailDocument
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function MailDocument() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'UPGRADE_ISSUE: (2064) Ole property OLE1.AppIsRunning was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            'SumeetIf Not OLE1.AppIsRunning Then
            'This one causes a Dr Watson error...
            'OLE1.DoVerb vbOLEInPlaceActivate
            'UPGRADE_ISSUE: (2070) Constant vbOLEShow was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2070.aspx
            'UPGRADE_ISSUE: (2064) Ole method OLE1.DoVerb was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            'SumeetOLE1.DoVerb(vbOLEShow)

            'Mail it
            Select Case m_sClass
                Case "Word.Document.8", "Word.Document.9"
                    'UPGRADE_ISSUE: (2064) Ole property OLE1.object was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
                    'UPGRADE_TODO: (1067) Member Application is not defined in type Object(...). More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                    'SumeetOLE1.object.Application.Run("Normal.PMDocumentManager.PMBEmailDocument")
            End Select

            m_lReturn = ShutItDown()

            'SumeetEnd If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MailDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MailDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PrintDocument
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function PrintDocument() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sOptionValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'UPGRADE_ISSUE: (2064) Ole property OLE1.AppIsRunning was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            'SumeetIf Not OLE1.AppIsRunning Then
            'This one causes a Dr Watson error...
            'OLE1.DoVerb vbOLEInPlaceActivate
            'UPGRADE_ISSUE: (2070) Constant vbOLEShow was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2070.aspx
            'UPGRADE_ISSUE: (2064) Ole method OLE1.DoVerb was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            'SumeetOLE1.DoVerb(vbOLEShow)

            'Print it
            Select Case m_sClass
                Case "Word.Document.8", "Word.Document.9"
                    'UPGRADE_ISSUE: (2064) Ole property OLE1.object was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
                    'UPGRADE_TODO: (1067) Member Application is not defined in type Object(...). More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                    'SumeetOLE1.object.Application.Run("Normal.PMDocumentManager.PMBPrintDocument")
            End Select

            m_lReturn = ShutItDown()

            'SumeetEnd If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function ShutItDown() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim iCount As Integer = Nothing
        Dim sTemp As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'UPGRADE_ISSUE: (2064) Ole property OLE1.AppIsRunning was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            'SumeetIf Not OLE1.AppIsRunning Then
            Return result
            'SumeetEnd If

            'Program specific code - extend when we add new types of document
            Select Case m_sClass
                Case "Word.Document.8", "Word.Document.9"
                    'UPGRADE_ISSUE: (2064) Ole property OLE1.object was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
                    'UPGRADE_TODO: (1067) Member Application is not defined in type Object(...). More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                    'SumeetiCount = OLE1.object.Application.Windows.Count
                    For iTemp As Integer = 1 To iCount
                        'UPGRADE_ISSUE: (2064) Ole property OLE1.object was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
                        'UPGRADE_TODO: (1067) Member Application is not defined in type Object(...). More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                        'SumeetsTemp = OLE1.object.Application.Windows(iTemp).Document.Name
                        'UPGRADE_ISSUE: (2064) Ole property OLE1.SourceDoc was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
                        'SumeetIf sTemp = OLE1.SourceDoc.Substring(OLE1.SourceDoc.Length - sTemp.Length) Then
                        'UPGRADE_ISSUE: (2064) Ole property OLE1.object was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
                        'UPGRADE_TODO: (1067) Member Application is not defined in type Object(...). More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                        'SumeetOLE1.object.Application.Windows(iTemp).Document.Save()
                        'UPGRADE_ISSUE: (2064) Ole property OLE1.object was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
                        'UPGRADE_TODO: (1067) Member Application is not defined in type Object(...). More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                        'SumeetOLE1.object.Application.Windows(iTemp).Document.Close()
                        iCount -= 1
                        Exit For
                        'SumeetEnd If

                    Next iTemp

                    If iCount = 0 Then
                        'UPGRADE_ISSUE: (2064) Ole property OLE1.object was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
                        'UPGRADE_TODO: (1067) Member Application is not defined in type Object(...). More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                        'SumeetOLE1.object.Application.Quit()
                    End If
                Case Else
                    MessageBox.Show("Not yet implemented", Application.ProductName)
            End Select

            'UPGRADE_ISSUE: (2064) Ole property OLE1.AppIsRunning was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            'SumeetOLE1.AppIsRunning = False
            Close()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to shut it down", vApp:=ACApp, vClass:=ACClass, vMethod:="ShutItDown", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateFileMaster
    '
    ' Description:
    '
    ' History: 03/09/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateFileMaster() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sClient, sDocType, sPageType, sDocName, sTemp As String 'DN 24/04/01
        Dim sServer As String = "" 'DN 24/04/01
        Dim lCount As Integer 'DN 24/04/01
        Dim sDocument As String = "" 'DN 24/04/01

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oSIRDOCAPI Is Nothing Then
                Dim temp_m_oSIRDOCAPI As Object
                m_lReturn = CType(g_oObjectManager.GetInstance(temp_m_oSIRDOCAPI, "bSIRDOCAPI.Form", vInstanceManager:=PMGetViaClientManager), gPMConstants.PMEReturnCode)
                m_oSIRDOCAPI = temp_m_oSIRDOCAPI

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the DOC API object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If

            'DN 24/04/01 - DME can now handle .Docs plus was causing permission issues
            '    If Not OLE1.AppIsRunning Then
            '        'This one causes a Dr Watson error...
            '        'OLE1.DoVerb vbOLEInPlaceActivate
            '        OLE1.DoVerb vbOLEShow
            '
            '        'Print it
            '        Select Case m_sClass
            '        Case "Word.Document.8", "Word.Document.9"
            '            OLE1.object.Application.Run "Normal.PMDocumentManager.SaveAsRTF"
            '        End Select
            '
            '        m_lReturn = ShutItDown
            '
            '    End If

            'It's not used, but we need to define it anyway...
            Dim sKeywords(0) As String

            sClient = m_sFileName

            'DN 24/04/01 - Get Server Document registry setting
            m_lReturn = GetServer(sServer)

            lCount = (sClient.ToLower().IndexOf("doc 0.doc") + 1)
            sDocument = sClient.Substring(lCount - 1, Math.Min(sClient.Length, sClient.Length + 1 - lCount))

            sServer = sServer & "\" & sDocument

            'Copy over to server
            File.Copy(sClient, sServer, True)

            'DN 05/07/01 - Loop until file exists
            Do
                sTemp = FileSystem.Dir(sServer, FileAttribute.Normal)
            Loop While sTemp = ""

            'PR 18/NOV/2002
            'ISS1298
            'Changed to Word document (DOC) rather than Rich Text Format (RTF).
            'For RTF, set sDocType="L" and sPageType="RTF".
            'For DOC, set sDocType="D" and sPageType="DOC".

            sDocType = "D"
            sPageType = "DOC"

            sDocName = m_sDescription
            'DN 27/04/01 - Pass Ins Folder Cnt instead of Ins File Cnt
            'DC110203 -ISS1460 -added m_lClaimCnt for archiving of claims documents
            'Thinh Nguyen 05/09/2003 - pass in policy ref instead of empty quote
            'UPGRADE_ISSUE: (2072) Control AddDocument could not be resolved because it was within the generic namespace Form. More Information: http://www.vbtonet.com/ewis/ewi2072.aspx
            m_lReturn = m_oSIRDOCAPI.AddDocument(lPartyId:=PartyCnt, sPartyName:="", lInsuranceFolderId:=m_lInsuranceFolderCnt, sInsuranceFileRef:=m_sInsuranceFileRef, lClaimId:=m_lClaimCnt, sClaimRef:=m_sClaimRef, lFSAComplaintFolderCnt:=0, sFSAComplaintReference:="", sDocType:=sDocType, sPageType:=sPageType, sDocName:=sDocName, sFilename:=sServer, sAnnotation:="", sKeywords:=sKeywords, lDocNumber:=m_lDocNumber)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DN 06/07/01 - Stop spooler hanging after archiving is finished
            Timer1.Enabled = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFileMaster Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateForm
    '
    ' Description: Validates the things FormControl can't.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ValidateForm) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ValidateForm() As gPMConstants.PMEReturnCode
    '
    'Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
    'Try 
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
    'LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: GetServer
    '
    ' Description:
    '
    ' History: 24/01/2000 Tom - Created.
    '
    ' ***************************************************************** '
    Private Function GetServer(ByRef sServer As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sServer.Trim() > "" Then
                Return result
            End If

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DocServer", r_sSettingValue:=sServer)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Server from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetServer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)
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
            Dim temp_m_oBusiness As Object
            m_lReturn = CType(g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRDocTemplate.Business", vInstanceManager:=PMGetViaClientManager), gPMConstants.PMEReturnCode)
            m_oBusiness = temp_m_oBusiness
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to get an instance of the business object.
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Display error stating the problem.
            '
            '        ' Get description from the resource file.
            '        sTitle$ = GetResData( _
            ''            iLangID:=g_iLanguageID%, _
            ''            lID:=ACBusinessFailTitle, _
            ''            iDataType:=PMResString)
            '
            '        sMessage$ = GetResData( _
            ''            iLangID:=g_iLanguageID%, _
            ''            lID:=ACBusinessFail, _
            ''            iDataType:=PMResString)
            '
            '        ' Display message.
            '        MsgBox sMessage$, vbCritical, sTitle$
            '
            '        Exit Sub
            '    End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBSpoolerOLE.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = CShort(g_iLanguageID)

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

    'UPGRADE_WARNING: (2080) Form_Load event was upgraded to Form_Load event and has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2080.aspx
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

            '    ' Set the process modes for the busines object.
            '    m_lReturn& = m_oBusiness.SetProcessModes( _
            ''        vTask:=CVar(m_iTask%), _
            ''        vNavigate:=CVar(m_lNavigate&), _
            ''        vProcessMode:=CVar(m_lProcessMode&), _
            ''        vTransactionType:=CVar(m_sTransactionType$), _
            ''        vEffectiveDate:=CVar(m_dtEffectiveDate))
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to process the interface.
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Log Error Message
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to set the process modes for the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Form_Load"
            '
            '        Exit Sub
            '    End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            Me.Height = 0
            Me.Width = 0

            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
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
            m_lReturn = m_oGeneral.GetInterfaceDetails()

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
            'UPGRADE_ISSUE: (2070) Constant vbFormCode was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2070.aspx
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
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
            'UPGRADE_TODO: (1067) Member Terminate is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            If Not (m_oSIRDOCAPI Is Nothing) Then
                ' Terminate the API object
                'UPGRADE_ISSUE: (2072) Control Terminate could not be resolved because it was within the generic namespace Form. More Information: http://www.vbtonet.com/ewis/ewi2072.aspx
                m_oSIRDOCAPI.Dispose()

                ' Destroy the instance of the API object
                ' from memory.
                m_oSIRDOCAPI = Nothing
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

    Private Sub Timer1_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Timer1.Tick

        Dim sTemp As String = ""

        Try

            'Word's closed down
            'UPGRADE_ISSUE: (2064) Ole property OLE1.AppIsRunning was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            'SumeetIf Not OLE1.AppIsRunning Then
            m_lReturn = ShutItDown()

            m_lReturn = SetOLE()

            Timer1.Enabled = False

            Me.Close()
            'SumeetElse

            'Check that the document has been closed
            'bThere = False

            'Program specific code - extend when we add new types of document
            'Select Case m_sClass
            '    Case "Word.Document.8", "Word.Document.9"
            '        'UPGRADE_ISSUE: (2064) Ole property OLE1.object was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            '        'UPGRADE_TODO: (1067) Member Application is not defined in type Object(...). More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            '        'SumeetiCount = OLE1.object.Application.Windows.Count
            '        For iTemp As Integer = 1 To iCount
            '            'UPGRADE_ISSUE: (2064) Ole property OLE1.object was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            '            'UPGRADE_TODO: (1067) Member Application is not defined in type Object(...). More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            '            'SumeetsTemp = OLE1.object.Application.Windows(iTemp).Document.Name
            '            'UPGRADE_ISSUE: (2064) Ole property OLE1.SourceDoc was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            '            'UPGRADE_ISSUE: (2064) Ole property OLE1.object was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2064.aspx
            '            'UPGRADE_TODO: (1067) Member Application is not defined in type Object(...). More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            '            'SumeetIf OLE1.object.Application.Documents(iTemp).Name = OLE1.SourceDoc.Substring(OLE1.SourceDoc.Length - sTemp.Length) Then
            '            bThere = True
            '            Exit For
            '            'SumeetEnd If

            '        Next iTemp

            '    Case Else
            '        MessageBox.Show("Not yet implemented", Application.ProductName)
            'End Select

            'If Not bThere Then
            '    m_lReturn = ShutItDown()

            '    m_lReturn = SetOLE()

            '    Timer1.Enabled = False

            '    Me.Close()
            'End If

            'SumeetEnd If

        Catch




            Exit Sub
        End Try


    End Sub

    ' PRIVATE Events (End)
End Class
