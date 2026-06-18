Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports System.Globalization
'Developer Guide No. 129
Imports SharedFiles
Imports System.Runtime.InteropServices


Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 23-07-1997
    '
    ' Description: Main interface.
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormControlMenu As Integer = 1
    Private Const vbFormCode As Integer = 0
    Private m_lPartyCnt As Integer
    Private m_sPartyCode As String = ""

    Private m_lAgentCnt As Integer
    Private m_sAgentCode As String = ""
    Private m_lIsDirectBusiness As Integer
    Private m_sQuoteType As String = ""

    Private m_sInsuranceFileRef As String = ""
    Private m_lInsuranceFileCnt As Integer

    Private m_dtStartDate As Date
    Private m_dtEndDate As Date

    Private m_sRiskIndex As Integer

    Private m_lProductId As Integer

    Private m_vSearchData(,) As Object

    Private m_sRunningMethodName As String = ""

    ' Declare an instance of the Business object.

    Private m_oBusiness As bSIRFindInsurance.Form 'Object of bSirFindInsurance.Form
    Private m_vProducts As Object
    Private sortColumn As Integer = -1

    Private Const Column1 As Integer = 1 'Insurance_ref
    Private Const Column2 As Integer = 2 'Currency
    Private Const Column3 As Integer = 3 'Insurance_file_cnt
    Private Const Column4 As Integer = 4 'Client
    Private Const Column5 As Integer = 5 'Agent
    Private Const Column6 As Integer = 6 'Product
    Private Const Column7 As Integer = 7 'Branch
    Private Const Column8 As Integer = 8 'Amount
    Private Const Column9 As Integer = 9 'insurance_file_type_id

    Private Const Column10 As Integer = 10 'Agent Type Code
    Private Const Column11 As Integer = 11 'Product Id
    Private Const Column12 As Integer = 12 'Client Id
    Private Const Column13 As Integer = 13 'Round Off Amount



    'Constants for Defining Width of Columns in List View

    Private Const ColWidth As Integer = 1700

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' *** BEGIN Inserted By ResGen ***

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date


    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUQuoteCollectionProcess.General

    ' Stores the return value for function calls.
    Private m_lReturn As gPMConstants.PMEReturnCode


    Private m_oChangePolicyStatus As bSIRChangePolicyStatus.Business

    Private m_vProductID As String = ""
    Private m_vQuoteTypeID As Byte

    Private m_cNetAmount As Decimal
    Private m_sSelectedCurrency As String = ""

    Private m_vSelectedProducts As Object

    'WPR12
    Private m_sTaskQuote As String = ""
    Private m_sTaskParty As String = ""
    Private m_sTaskAgent As String = ""
    Private m_sTaskProduct As String = ""
    Private m_sTaskRiskIndex As String = ""
    Private m_dtTaskFromDate As Date
    Private m_dtTaskToDate As Date
    Private m_lTaskDirectBusiness As Integer
    Private m_iTaskVia As Integer
    Private m_sTaskProductText As String = ""
    Private m_bAgentTypeBroker As Boolean
    Private hScrollValue As Integer = 0

    'Win32 API declarations to preserve list view horizontal scroll position after sort
    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0



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

    'WPR12

    Public Property TaskQuote() As String
        Get
            Return m_sTaskQuote
        End Get
        Set(ByVal Value As String)
            m_sTaskQuote = Value
        End Set
    End Property


    Public Property TaskParty() As String
        Get
            Return m_sTaskParty
        End Get
        Set(ByVal Value As String)
            m_sTaskParty = Value
        End Set
    End Property


    Public Property TaskAgent() As String
        Get
            Return m_sTaskAgent
        End Get
        Set(ByVal Value As String)
            m_sTaskAgent = Value
        End Set
    End Property


    Public Property TaskProduct() As String
        Get
            Return m_sTaskProduct
        End Get
        Set(ByVal Value As String)
            m_sTaskProduct = Value
        End Set
    End Property


    Public Property TaskRiskIndex() As String
        Get
            Return m_sTaskRiskIndex
        End Get
        Set(ByVal Value As String)
            m_sTaskRiskIndex = Value
        End Set
    End Property


    Public Property TaskFromDate() As Date
        Get
            Return m_dtTaskFromDate
        End Get
        Set(ByVal Value As Date)
            m_dtTaskFromDate = Value
        End Set
    End Property


    Public Property TaskToDate() As Date
        Get
            Return m_dtTaskToDate
        End Get
        Set(ByVal Value As Date)
            m_dtTaskToDate = Value
        End Set
    End Property


    Public Property TaskDirectBusiness() As Integer
        Get
            Return m_lTaskDirectBusiness
        End Get
        Set(ByVal Value As Integer)
            m_lTaskDirectBusiness = Value
        End Set
    End Property


    Public Property TaskVia() As Integer
        Get
            Return m_iTaskVia
        End Get
        Set(ByVal Value As Integer)
            m_iTaskVia = Value
        End Set
    End Property


    Public Property TaskProductText() As String
        Get
            Return m_sTaskProductText
        End Get
        Set(ByVal Value As String)
            m_sTaskProductText = Value
        End Set
    End Property

    <DllImport("user32.dll")> _
    Private Shared Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

    End Function
    <DllImport("user32.dll")> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

    End Function
    <DllImport("user32.dll")> _
    Private Shared Function LockWindowUpdate(ByVal Handle As IntPtr) As Boolean

    End Function
    'Store the horizontal scroll value.
    Private Sub StoreHScrollValue()
        hScrollValue = GetScrollPos(lvwsearchdetails.Handle, SBS_HORZ)
    End Sub
    'Recover the old scroll position
    Private Sub RecoverHorizontalScroll()
        LockWindowUpdate(lvwsearchdetails.Handle)
        'Calculate the value the scroll needs to scroll back.
        Dim dx As Integer = hScrollValue - GetScrollPos(lvwsearchdetails.Handle, SBS_HORZ)
        'Send the scroll message.
        Dim b As Boolean = SendMessage(lvwsearchdetails.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)

    End Sub




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
        'Const ACPercentageDecimalPlaces As Integer = 2

        Try

            ' Get the Mandatory details from the business object.

            '    Set m_oFormFields = CreateObject("iPMFormControl.FormFields")
            '
            '    m_oFormFields.LanguageID = g_iLanguageID%
            '
            '    m_lReturn = m_oFormFields.AddNewFormField( _
            ''                 ctlControl:=cboPurgefrequencyID, _
            ''                 lFieldType:=PMLookup, _
            ''                 lFormat:=PMLookup, _
            ''                 lMandatory:=lPurgefrequencyIDMandy)
            '
            '    If m_lReturn <> PMTrue Then
            '      SetFieldValidation = PMFalse
            '      Exit Function
            '    End If
            '


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description: Explicit intialise for the form
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Dim sMessage, sTitle As String

        'TN20010628 - variable to see if we need to display postcode

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value. ? why
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Initailise business objects to serve this form

            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRFindInsurance.Form", vInstanceManager:="ClientManager")
            'm_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSirFindInsurance.Form", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Get description from the resource file.

                'Developer Guide No 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                MessageBox.Show("Unable to gain access to the business object." & Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Business Object", MessageBoxButtons.OK, MessageBoxIcon.Error)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUQuoteCollectionProcess.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the cancelled property to true. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            'Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Return result

            End If
            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise the interface form", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            'Developer Guide No 32


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



            Return gPMConstants.PMEReturnCode.PMTrue

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

            'Party Bank Details

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
    ' Name:         CreateWorkManagerMemo
    '
    ' Description:  Creates a memo for an insurance team memeber
    '               to change the Agent status to Stopped or Active.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CreateWorkManagerMemo) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CreateWorkManagerMemo(ByVal v_iAccountStatusID As Integer, ByVal v_sAccountCode As String, ByVal v_sAccountName As String) As Integer
    '
    'Dim vKeys As Variant
    'Dim sTaskDescription As String
    'Dim oWrkTaskInstance As Object
    'Dim oPMLookUp As Object
    'Dim lTaskGroupID As Long
    'Dim lTaskID As Long
    'Dim sVerb As String
    ''
    'Const TASK_CODE = "MEMO"
    'Const TASK_GROUP_CODE = "COMMON"
    ''
    '    On Error GoTo Err_CreateWorkManagerMemo
    ''
    '    CreateWorkManagerMemo = PMTrue
    ''
    '    ReDim vKeys(0 To 1, 0 To 7)
    ''
    '    ' Object to create work manager tasks
    '    m_lReturn& = g_oObjectManager.GetInstance( _
    ''        oObject:=oWrkTaskInstance, _
    ''        sClassName:="iPMWrkTaskInstance.NavigatorV3", _
    ''        vInstanceManager:=PMGetLocalInterface)
    '    If (m_lReturn& <> PMTrue) Then
    '        CreateWorkManagerMemo = PMFalse
    '        Exit Function
    '    End If
    ''
    '    ' Create an instance of bPMLookup
    '    m_lReturn& = g_oObjectManager.GetInstance( _
    ''        oObject:=oPMLookUp, _
    ''        sClassName:="bPMLookup.Business", _
    ''        vInstanceManager:=PMGetViaClientManager)
    '    If (m_lReturn& <> PMTrue) Then
    '        CreateWorkManagerMemo = PMFalse
    '        Exit Function
    '    End If
    ''
    '    ' Set the product family
    '    oPMLookUp.PMLookupProductFamily = pmePFSiriusArchitecture
    ''
    '    ' Use the lookup to get the ID for the group COMMON
    '    m_lReturn& = oPMLookUp.GetEffectiveIDFromCode( _
    ''                    v_sTableName:="pmwrk_task_group", _
    ''                    v_sCode:=TASK_GROUP_CODE, _
    ''                    v_dtEffectiveDate:=Date, _
    ''                    r_lID:=lTaskGroupID)
    '    If (m_lReturn& <> PMTrue) Then
    '        CreateWorkManagerMemo = PMFalse
    '        Exit Function
    '    End If
    ''
    '    ' Use the lookup to get the ID of the MEMO task
    '    m_lReturn& = oPMLookUp.GetEffectiveIDFromCode( _
    ''                    v_sTableName:="pmwrk_task", _
    ''                    v_sCode:=TASK_CODE, _
    ''                    v_dtEffectiveDate:=Date, _
    ''                    r_lID:=lTaskID&)
    '    If (m_lReturn& <> PMTrue) Then
    '        CreateWorkManagerMemo = PMFalse
    '        Exit Function
    '    End If
    ''
    '    ' Remove instance of lookup
    '    m_lReturn& = oPMLookUp.Terminate()
    '    Set oPMLookUp = Nothing
    ''
    '    ' Set the authority level
    '    oWrkTaskInstance.NavigatorV3_PMAuthorityLevel = m_lPMAuthorityLevel&
    ''
    '    ' Set to ADD mode
    '    m_lReturn& = oWrkTaskInstance.NavigatorV3_SetProcessModes( _
    ''                    vTask:=PMAdd)
    '    If (m_lReturn& <> PMTrue) Then
    '        CreateWorkManagerMemo = PMFalse
    '        Exit Function
    '    End If
    ''
    '    If (v_iAccountStatusID% = ACTAccountStatusActive) Then
    '        sVerb$ = "ACTIVATED"
    '    Else
    '        sVerb$ = "STOPPED"
    '    End If
    ''
    '    ' Active or Stopped ?
    '    sTaskDescription$ = "The account """ & Trim$(v_sAccountCode$) & """ has been " & _
    ''                        sVerb$ & " in Orion. " & _
    ''                        "The corresponding agent must be " & _
    ''                        sVerb$ & " in Sirius Back Office."
    ''
    '    vKeys(PMKeyName, 0) = PMKeyNameTaskGroupID
    '    vKeys(PMKeyValue, 0) = lTaskGroupID&
    '    vKeys(PMKeyName, 1) = PMKeyNameTaskGroupCode
    '    vKeys(PMKeyValue, 1) = TASK_GROUP_CODE
    '    vKeys(PMKeyName, 2) = PMKeyNameTaskID
    '    vKeys(PMKeyValue, 2) = lTaskID&
    '    vKeys(PMKeyName, 3) = PMKeyNameTaskCode
    '    vKeys(PMKeyValue, 3) = TASK_CODE
    '    vKeys(PMKeyName, 4) = PMKeyNameTaskDescription
    '    vKeys(PMKeyValue, 4) = sTaskDescription$
    '    vKeys(PMKeyName, 5) = PMKeyNameTaskCustomer
    '    vKeys(PMKeyValue, 5) = m_sAccountName$
    '    vKeys(PMKeyName, 6) = PMKeyNameTaskDueDate
    '    vKeys(PMKeyValue, 6) = Date
    '    vKeys(PMKeyName, 7) = PMKeyNameTaskIsUrgent
    '    vKeys(PMKeyValue, 7) = PMTrue
    ''
    '    ' Pass the keys in
    '    m_lReturn& = oWrkTaskInstance.NavigatorV3_SetKeys(vKeyArray:=vKeys)
    '    If (m_lReturn& <> PMTrue) Then
    '        CreateWorkManagerMemo = PMFalse
    '        Exit Function
    '    End If
    ''
    '    ' Display the form
    '    m_lReturn& = oWrkTaskInstance.NavigatorV3_Start()
    '    If (m_lReturn& <> PMTrue) Then
    '        CreateWorkManagerMemo = PMFalse
    '        Exit Function
    '    End If
    ''
    '    ' Remove instance
    '    m_lReturn& = oWrkTaskInstance.Terminate()
    '    Set oWrkTaskInstance = Nothing
    ''
    '    Exit Function
    ''
    'Err_CreateWorkManagerMemo:
    ''
    '    CreateWorkManagerMemo = PMError
    ''
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="CreateWorkManagerMemo Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="CreateWorkManagerMemo", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    ''
    '    Exit Function
    ''
    'End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim vParamArray As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ReDim vParamArray(0)

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = CType(InterfaceToData(), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
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


            ' Get the lookup values.

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
            '    ' RAW 17/12/2002 : PS187 : constant renamed
            '    m_lReturn& = GetLookupDetails( _
            ''        iLookupTable:=m_kiTableAccountType, _
            ''        ctlLookup:=cboAccounttypeID)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '

            Return gPMConstants.PMEReturnCode.PMTrue

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



            Return gPMConstants.PMEReturnCode.PMTrue

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            lvwsearchdetails.Columns.Clear()
            lvwsearchdetails.Columns.Insert(Column1 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column2 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column3 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column4 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column5 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column6 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column7 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column8 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column9 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column10 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column11 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column12 - 1, "", 94)
            lvwsearchdetails.Columns.Insert(Column13 - 1, "", 94)

            lvwsearchdetails.Columns.Item(Column1 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth + 1000))
            lvwsearchdetails.Columns.Item(Column2 - 1).Width = CInt(0) ' Currency
            lvwsearchdetails.Columns.Item(Column3 - 1).Width = CInt(0) 'Insurance_file_cnt
            lvwsearchdetails.Columns.Item(Column4 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth + 500))
            lvwsearchdetails.Columns.Item(Column5 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth))
            lvwsearchdetails.Columns.Item(Column6 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth))
            lvwsearchdetails.Columns.Item(Column7 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth - 150))
            lvwsearchdetails.Columns.Item(Column8 - 1).Width = CInt(VB6.TwipsToPixelsX(ColWidth - 150))
            lvwsearchdetails.Columns.Item(Column9 - 1).Width = CInt(0)
            lvwsearchdetails.Columns.Item(Column10 - 1).Width = CInt(0)
            lvwsearchdetails.Columns.Item(Column11 - 1).Width = CInt(0)
            lvwsearchdetails.Columns.Item(Column12 - 1).Width = CInt(0)
            lvwsearchdetails.Columns.Item(Column13 - 1).Width = CInt(0)

            lvwsearchdetails.Columns.Item(Column8 - 1).TextAlign = HorizontalAlignment.Right

            lvwsearchdetails.AllowColumnReorder = False


            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Update the interface details with the
            ' property members.
            m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'RWH(18/04/2001)
            'Made full row select on list views
            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwsearchdetails.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not (Convert.IsDBNull(dtpStartDate.Value) Or IsNothing(dtpStartDate.Value)) Then
                'Developer Guide No 264
                m_dtStartDate = CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=dtpStartDate.Value))
            End If

            If Not (Convert.IsDBNull(dtpEndDate.Value) Or IsNothing(dtpEndDate.Value)) Then
                'Developer Guide No 264
                m_dtEndDate = CDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=dtpEndDate.Value))
            End If

            m_lIsDirectBusiness = chkDirectBusiness.CheckState

            If m_iTaskVia = 1 Then
                cmdFindNow_Click(cmdFindNow, New EventArgs())
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
    ' Name: SetMandatoryDetails
    '
    ' Description: Sets the mandatory details on the interface.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (SetMandatoryDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SetMandatoryDetails() As Integer
    '
    'Dim result As Integer = 0
    'Dim vAccountName As Object
    '
    'Try 
    '
    '
    ' Get the mandatory details from the business object.
    '    m_lReturn& = m_oBusiness.GetMandatory( _
    ''        vAccountName:=vAccountName)
    ''
    '    ' Check for errors.
    '    If (m_lReturn& <> PMTrue) Then
    '        SetMandatoryDetails = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Clear the collection
    '    m_lReturn = m_oGeneral.ResetMandatoryControls
    ''
    '    If (m_lReturn& <> PMTrue) Then
    '        SetMandatoryDetails = PMFalse
    '        Exit Function
    '    End If
    ''
    '    ' Set the mandatory details on the interface.
    ''
    '    If (vAccountName = PMMandatory) Then
    '        m_lReturn = m_oGeneral.SetMandatoryControl(txtAccountName)
    '    End If
    ''
    '    If m_lReturn <> PMTrue Then
    '      SetMandatoryDetails = PMFalse
    '      Exit Function
    '    End If
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the mandatory details", vApp:=ACApp, vClass:=ACClass, vMethod:="SetMandatoryDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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

            ' Display all language specific captions


            'Developer Guide No 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'Developer Guide No 243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No 243
            cmdFindNow.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindNowButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No 243
            cmdNewSearch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewSearchButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'Insurance Reference


            'Developer Guide No 243
            lvwsearchdetails.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lvwsearchdetails.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            lvwsearchdetails.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No 243
            lvwsearchdetails.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            lvwsearchdetails.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            lvwsearchdetails.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            lvwsearchdetails.Columns.Item(6).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle7, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            lvwsearchdetails.Columns.Item(7).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle8, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            cmdQuoteRef.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindQuoteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            cmdClient.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindClientButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No 243
            cmdAgent.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindAgentButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Developer Guide No 243
            cmdProduct.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFindProductButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            lblRiskIndex.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskIndexLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            chkDirectBusiness.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDirectBusinessCheck, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            cmdSelectAlll.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelectAllButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide No 243
            cmdMakePayment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMakePaymentButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' *** END Inserted By ResGen ***

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
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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
    '    Select Case (m_iTask)
    '        Case PMAdd
    '            ' Get all of the lookup values.
    '            m_lReturn& = m_oBusiness.GetLookupValues( _
    ''                iLookupType:=PMLookupAll, _
    ''                vTableArray:=m_vLookupValues, _
    ''                iLanguageID:=g_iLanguageID%, _
    ''                vResultArray:=m_vLookupDetails)
    ''
    '        Case PMEdit
    '            ' Get all of the lookup values with the correct
    '            ' effective date.
    '            m_lReturn& = m_oBusiness.GetLookupValues( _
    ''                iLookupType:=PMLookupAllEffective, _
    ''                vTableArray:=m_vLookupValues, _
    ''                iLanguageID:=g_iLanguageID%, _
    ''                vResultArray:=m_vLookupDetails)
    ''
    '        Case PMView
    '            ' Get lookup values for viewing only.
    '            m_lReturn& = m_oBusiness.GetLookupValues( _
    ''                iLookupType:=PMLookupSingle, _
    ''                vTableArray:=m_vLookupValues, _
    ''                iLanguageID:=g_iLanguageID%, _
    ''                vResultArray:=m_vLookupDetails)
    '    End Select
    ''
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
    'Catch 
    'End Try
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Return result
    '
    'End Function

    Private Sub chkDirectBusiness_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDirectBusiness.CheckStateChanged

        If chkDirectBusiness.CheckState = CheckState.Checked Then
            m_lIsDirectBusiness = 1
            txtAgent.Text = ""
            txtAgent.Enabled = False
            cmdAgent.Enabled = False
            m_lAgentCnt = 0
        Else
            m_lIsDirectBusiness = 0
            cmdAgent.Enabled = True
            txtAgent.Enabled = True
        End If

    End Sub

    Private Sub cmdAddTask_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddTask.Click


        Try



            m_lReturn = CreateWorkManagerTask()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If



        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Task Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddTask_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally


        End Try
    End Sub

    Private Sub cmdAgent_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgent.Click

        Dim oFindParty As iPMBFindParty.Interface_Renamed

        Dim sAgentCode As String = ""
        Dim lAgentCnt As Integer

        Const kMethodName As String = "cboAgent_Click"

        ' Create Find Party object
        Dim temp_oFindParty As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oFindParty = temp_oFindParty

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "iPMBFindParty.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        oFindParty.SpecialParty = "AG"
        ' Set component properties and start interface

        oFindParty.ShortName = txtAgent.Text

        oFindParty.CallingAppName = ACApp

        oFindParty.IgnoreDPAQuestions = True

        oFindParty.NotEditable = 1

        oFindParty.EnableNewParty = False
        'PN65466

        oFindParty.SuppressSubAgents = True


        m_lReturn = oFindParty.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "oFindParty.Start Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
            'Retrieve party details

            sAgentCode = oFindParty.ShortName

            lAgentCnt = oFindParty.PartyCnt

            ' Destroy Find Party object

            oFindParty.Dispose()
            oFindParty = Nothing

            ' Display Agent on form
            txtAgent.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=sAgentCode.Trim())

            txtAgent.Tag = txtAgent.Text

            txtClient.Text = ""

            m_sAgentCode = sAgentCode
            m_lAgentCnt = lAgentCnt


        End If



    End Sub

    Private Sub cmdClient_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClient.Click

        Dim sPartyCode As String = ""
        Dim lPartyCnt As Integer

        Const kMethodName As String = "cmdClient_Click"

        Dim oFindParty As iPMBFindParty.Interface_Renamed

        ' Create Find Party object
        Dim temp_oFindParty As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oFindParty = temp_oFindParty

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "iPMBFindParty.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Set component properties and start interface

        oFindParty.CallingAppName = ACApp

        oFindParty.IgnoreDPAQuestions = True

        oFindParty.NotEditable = 1

        oFindParty.EnableNewParty = False

        oFindParty.ShortName = txtClient.Text

        m_lReturn = oFindParty.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "oFindParty.Start Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
            'Retrieve party details

            sPartyCode = oFindParty.ShortName

            lPartyCnt = oFindParty.PartyCnt

            ' Destroy Find Party object

            oFindParty.Dispose()
            oFindParty = Nothing

            ' Display Client on form
            txtClient.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=sPartyCode.Trim())

            txtAgent.Text = ""

            m_sPartyCode = sPartyCode
            m_lPartyCnt = lPartyCnt


        End If

    End Sub


    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click
        Dim bSIRInsuranceFile As Object

        Dim oInsuranceFile As bSIRInsuranceFile.Business
        Dim lPartyCnt, lAgentCnt, lInsuranceFileCnt As Integer

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)



            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwsearchdetails.Items.Clear()

            ' Clear the search status bar.
            _stbstatus_Panel1.Text = ""
            m_sSelectedCurrency = ""
            lblNetAmount.Text = "0.00"
            m_cNetAmount = 0

            Dim temp_oInsuranceFile As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oInsuranceFile, "bSIRInsuranceFile.Business", vInstanceManager:="ClientManager")
            oInsuranceFile = temp_oInsuranceFile

            If txtClient.Text <> "" Then

                m_lReturn = oInsuranceFile.GetFromTable(v_vTableName:="Party", v_vFieldName:="Party_cnt", v_vKeyField:="ShortName", v_vKeyID:="'" & txtClient.Text & "'", r_vResult:=lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("cmdFindNow_Click", "SearchResults Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                If lPartyCnt = 0 Then
                    MessageBox.Show("Client Code You Entered is Not Correct", "Find Quote", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Else
                    m_lPartyCnt = lPartyCnt
                End If
            End If

            If txtQuoteRef.Text <> "" Then

                m_lReturn = oInsuranceFile.GetFromTable(v_vTableName:="Insurance_File", v_vFieldName:="Insurance_File_cnt", v_vKeyField:="Insurance_Ref", v_vKeyID:="'" & txtQuoteRef.Text & "'", r_vResult:=lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("cmdFindNow_Click", "SearchResults Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                If lInsuranceFileCnt = 0 Then
                    MessageBox.Show("Quote Reference You Entered is Not Correct", "Find Quote", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Else
                    m_lInsuranceFileCnt = lInsuranceFileCnt
                End If
            End If

            If txtAgent.Text <> "" Then

                m_lReturn = oInsuranceFile.GetFromTable(v_vTableName:="Party", v_vFieldName:="Party_cnt", v_vKeyField:="ShortName", v_vKeyID:="'" & txtAgent.Text & "'", r_vResult:=lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("cmdFindNow_Click", "SearchResults Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                If lPartyCnt = 0 Then
                    MessageBox.Show("Agent Code You Entered is Not Correct", "Find Quote", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Else

                    m_lReturn = oInsuranceFile.GetFromTable(v_vTableName:="Party_Agent", v_vFieldName:="Party_cnt", v_vKeyField:="Party_cnt", v_vKeyID:=lPartyCnt, r_vResult:=lAgentCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("cmdFindNow_Click", "SearchResults Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    If lAgentCnt = 0 Then
                        MessageBox.Show("Agent Code You Entered is Not Correct", "Find Quote", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    Else
                        m_lAgentCnt = lAgentCnt
                    End If
                End If
            End If


            m_lReturn = CType(SearchResults(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdFindNow_Click", "SearchResults Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdFindNow_Click", "DataToInterface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdMakePayment_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    Private Sub cmdMakePayment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMakePayment.Click

        Dim cNetAmountPayable As Decimal
        Dim bMultiplePoliciesSelected As Boolean

        Dim sErrorMessage As String = ""

        Const kMethodName As String = "cmdMakePayment_Click"

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_sRunningMethodName = kMethodName

            m_lReturn = CType(ProcessValidation(r_cNetAmountPayable:=cNetAmountPayable, r_bMultiplePoliciesSelected:=bMultiplePoliciesSelected), gPMConstants.PMEReturnCode)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(ProcessCollection(v_cNetAmountPayable:=cNetAmountPayable, v_bMultiplePoliciesSelected:=bMultiplePoliciesSelected), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sErrorMessage = "Payment could not be processed"
                End If

                'lblNetAmount.Caption = "0.00"
                'm_cNetAmount = 0
            End If



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdMakePayment_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            If sErrorMessage.Trim().Length > 0 Then
                MessageBox.Show(sErrorMessage, ACClass, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            m_sRunningMethodName = ""

            ''cmdFindNow_Click
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try

            ' Clear the interface details.
            m_lReturn = CType(ClearInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

            m_lInsuranceFileCnt = 0
            m_lPartyCnt = 0
            m_lAgentCnt = 0
            m_vProductID = CStr(0)
            m_vQuoteTypeID = 0
            m_cNetAmount = 0
            m_sSelectedCurrency = ""

            m_vSelectedProducts = ""

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private Sub cmdProduct_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdProduct.Click


        Dim sInsuranceFileRef As String = ""

        Dim lLbound, lUbound As Integer



        Dim oFindProduct As New frmSelectProducts

        If Information.IsArray(m_vSelectedProducts) Then

            'TODO:MILAN:: SetAlreadySelected is not a member of uctPickList
            'm_lReturn = oFindProduct.PickListProducts.SetAlreadySelected(m_vSelectedProducts)

        End If

        oFindProduct.ShowDialog()




        m_vSelectedProducts = oFindProduct.PickListProducts.GetItemDetails

        If oFindProduct.Status = gPMConstants.PMEReturnCode.PMOK Then
            txtProduct.Text = ""
            txtProduct.Tag = ""

            If Information.IsArray(m_vSelectedProducts) Then


                lLbound = m_vSelectedProducts.GetLowerBound(0)

                lUbound = m_vSelectedProducts.GetUpperBound(0)

                For i As Integer = lLbound To lUbound

                    'Developer Guide No 98
                    txtProduct.Text = txtProduct.Text & "," + m_vSelectedProducts(i, 1)

                    'Developer Guide No 98
                    txtProduct.Tag = Convert.ToString(txtProduct.Tag) & "," + m_vSelectedProducts(i, 0)
                Next

                'PN65470
                If lUbound > 0 Then
                    txtProduct.Text = ",Multiple"
                End If
            End If
            txtProduct.Text = Mid(txtProduct.Text, 2)
            txtProduct.Tag = Mid(Convert.ToString(txtProduct.Tag), 2)
        End If

        oFindProduct.Close()
        oFindProduct = Nothing

    End Sub

    Private Sub cmdQuoteRef_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdQuoteRef.Click

        Const kMethodName As String = "cmdQuote_Click"


        Dim oFindPolicy As iPMBFindInsurance.Interface_Renamed

        Dim sInsuranceFileRef As String = ""
        Dim lInsuranceFileCnt As Integer

        ' Create Find Insurance object
        Dim temp_oFindPolicy As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oFindPolicy, sClassName:="iPMBFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oFindPolicy = temp_oFindPolicy

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "iSIRFindInsurance.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Set component properties and start interface

        oFindPolicy.CallingAppName = ACApp

        oFindPolicy.InsFileType = "ALLQUOTE"

        oFindPolicy.InsReference = txtQuoteRef.Text

        m_lReturn = oFindPolicy.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "iSIRFindInsurance.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        If oFindPolicy.Status = gPMConstants.PMEReturnCode.PMOK Then
            ' Retrieve InsuranceRef and set as PolicyRef

            sInsuranceFileRef = oFindPolicy.InsReference

            lInsuranceFileCnt = oFindPolicy.InsFileCnt
        End If

        ' Destroy Find Insurance object

        oFindPolicy.Dispose()
        oFindPolicy = Nothing

        ' Display Quote Reference on form
        txtQuoteRef.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=sInsuranceFileRef.Trim())

        txtQuoteRef.Tag = txtQuoteRef.Text

        m_sInsuranceFileRef = sInsuranceFileRef
        m_lInsuranceFileCnt = lInsuranceFileCnt


    End Sub

    Private Sub cmdSelectAlll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectAlll.Click

        Dim cNetAmount As Decimal


        If lvwsearchdetails.Items.Count = 0 Then Exit Sub

        Dim bChecked As Boolean = False

        m_sSelectedCurrency = ""


        If cmdSelectAlll.Text = "&Select All" Then
            bChecked = True
            cmdSelectAlll.Text = "&Deselect All"
            m_cNetAmount = 0
        Else
            cmdSelectAlll.Text = "&Select All"
        End If
        RemoveHandler lvwsearchdetails.ItemChecked, AddressOf lvwsearchdetails_ItemChecked
        For i As Integer = 1 To lvwsearchdetails.Items.Count

            lvwsearchdetails.Items.Item(i - 1).Checked = bChecked
            cNetAmount = gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwsearchdetails.Items.Item(i - 1), Column7).Text.Substring(0, Math.Min(ListViewHelper.GetListViewSubItem(lvwsearchdetails.Items.Item(i - 1), Column7).Text.Length, ListViewHelper.GetListViewSubItem(lvwsearchdetails.Items.Item(i - 1), Column7).Text.IndexOf("("c))))
            If bChecked Then
                m_cNetAmount += cNetAmount
            Else
                m_cNetAmount -= cNetAmount
            End If

        Next i
        AddHandler lvwsearchdetails.ItemChecked, AddressOf lvwsearchdetails_ItemChecked
        lblNetAmount.Text = StringsHelper.Format(m_cNetAmount, "#,##0.00")

    End Sub

    Private Sub dtpEndDate_ValueChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles dtpEndDate.ValueChanged
        'Developer Guide No 264
        m_dtEndDate = gPMFunctions.ToSafeDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=dtpEndDate.Value))
    End Sub

    Private Sub dtpStartDate_ValueChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles dtpStartDate.ValueChanged
        'Developer Guide No. 264
        m_dtStartDate = gPMFunctions.ToSafeDate(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=dtpStartDate.Value))
    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            'DD 20/01/2003: Put form in taskbar
            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

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
        Const kMethodName As String = "Form_Load"

        Dim lReturn, lSubValue As Integer


        Try




            iPMFunc.ShowFormInTaskBar_Detach()



        Catch ex As Exception


            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means other than pressing the command buttons.



            If UnloadMode = vbFormControlMenu Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            ElseIf (UnloadMode <> vbFormCode) Then

                ' Process the next set of actions depending upon the interface task etc.
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

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

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


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK


            Me.Hide()

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

    Private Function ClearInterface(Optional ByVal sCallingMethod As String = "") As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the user still wishes to clear
            ' the interface.

            ' Display the message.
            If sCallingMethod <> "SearchResults" Then
                iMsgResult = MessageBox.Show("A new search will clear all of your existing search details." & Strings.Chr(13) & Strings.Chr(10) & " Do you wish to continue? ", "Quote Cancellation Process", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.No Then
                    ' Don't continue with the clear.
                    Return result
                End If
            End If
            ' Clear the interface details.

            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwsearchdetails.Items.Clear()

            ' Clear the search status bar.
            _stbstatus_Panel1.Text = ""

            ' All fields should be cleared.
            txtQuoteRef.Text = ""
            txtClient.Text = ""
            txtAgent.Text = ""
            txtProduct.Text = ""
            txtProduct.Tag = ""
            txtRiskIndex.Text = ""
            dtpStartDate.Value = DateTime.Today
            dtpStartDate.Checked = False
            dtpEndDate.Value = DateTime.Today
            dtpEndDate.Checked = False
            'Cahnegs done as per VB functionality
            'dtpStartDate.Value = CDate("")
            'dtpEndDate.Value = CDate("")
            dtpStartDate.Value = Date.Today
            dtpEndDate.Value = Date.Today
            lblNetAmount.Text = "0.00"
            m_sSelectedCurrency = ""

            chkDirectBusiness.CheckState = CheckState.Unchecked


            'Developer Guide No 243
            cmdSelectAlll.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSelectAllButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    Private Function SearchResults() As Integer

        Dim result As Integer = 0
        Dim iCnt As Integer
        Dim lLbound, lUbound As Integer

        Dim bFound As Boolean

        Dim vRiskIndexSearchData As Object
        Dim vRiskIndexSearchString As String = ""

        Dim vSearchDataTemp As Object

        Const kMethodName As String = "SearchResults"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            DisplayStatusSearching()

            If txtRiskIndex.Text.Trim().Length > 0 Then

                vRiskIndexSearchString = gPMFunctions.ToSafeString(txtRiskIndex.Text).Trim()
                ClearInterface("SearchResults")
                txtRiskIndex.Text = vRiskIndexSearchString
                m_lPartyCnt = 0
                m_lAgentCnt = 0
                m_lInsuranceFileCnt = 0
                m_lIsDirectBusiness = 0


                m_lReturn = m_oBusiness.FindLikeIndexForCollection(sIndex:=vRiskIndexSearchString, lNumberOfRecords:=gPMConstants.PMAllRecords, vResultArray:=vRiskIndexSearchData)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    gPMFunctions.RaiseError(kMethodName, "g_oBusiness.FindLikeIndexForCollection Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            'Developer Guide No.290
            If dtpStartDate.Checked = False Then
                m_dtStartDate = #12:00:00 AM#
            End If

            If dtpEndDate.Checked = False Then
                m_dtEndDate = #12:00:00 AM#
            End If


            m_lReturn = m_oBusiness.GetQuotesMarkedForCollection(v_lPartyCnt:=m_lPartyCnt, v_lAgentCnt:=m_lAgentCnt, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_sProductIds:=Convert.ToString(txtProduct.Tag), v_lIsDirectBusiness:=m_lIsDirectBusiness, v_dtStartDate:=m_dtStartDate, v_dtEndDate:=m_dtEndDate, r_vResultArray:=m_vSearchData)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kMethodName, "m_oBusiness.GetQuotesMarkedForCollection Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vRiskIndexSearchData) And Information.IsArray(m_vSearchData) Then


                vSearchDataTemp = VB6.CopyArray(m_vSearchData)


                m_lReturn = CType(gPMFunctions.FlipArray(r_vArray:=vSearchDataTemp), gPMConstants.PMEReturnCode)


                lLbound = vSearchDataTemp.GetLowerBound(0)

                lUbound = vSearchDataTemp.GetUpperBound(0)


                ReDim Preserve vSearchDataTemp(lUbound, vSearchDataTemp.GetUpperBound(1) + 1)

                bFound = False

                For iLoop1 As Integer = vRiskIndexSearchData.GetLowerBound(1) To vRiskIndexSearchData.GetUpperBound(1)
                    For iLoop2 As Integer = lLbound To lUbound


                        If gPMFunctions.ToSafeLong(CStr(vRiskIndexSearchData(2, iLoop1))) = gPMFunctions.ToSafeLong(CStr(vSearchDataTemp(iLoop2, 0))) Then

                            vSearchDataTemp(iLoop2, 18) = 1
                            bFound = True
                        End If
                    Next
                Next
                'Developer Guide No. 146
                'Array.Clear(m_vSearchData, 0, m_vSearchData.Length)
                m_vSearchData = Nothing
                If bFound Then

                    m_lReturn = CType(gPMFunctions.FlipArray(r_vArray:=vSearchDataTemp), gPMConstants.PMEReturnCode)

                    iCnt = 0

                    For iLoop1 As Integer = lLbound To lUbound

                        If CDbl(vSearchDataTemp(18, iLoop1)) = 1 Then
                            If iCnt > 0 Then

                                ReDim Preserve m_vSearchData(vSearchDataTemp.GetUpperBound(0) - 1, m_vSearchData.GetUpperBound(1) + 1)
                            Else
                                ReDim m_vSearchData(MAXCOL, 0)
                            End If


                            m_vSearchData(ACIInsuranceFileCnt, iCnt) = vSearchDataTemp(ACIInsuranceFileCnt, iLoop1)

                            m_vSearchData(ACIInsuranceRef, iCnt) = vSearchDataTemp(ACIInsuranceRef, iLoop1)

                            m_vSearchData(ACIClientID, iCnt) = vSearchDataTemp(ACIClientID, iLoop1)

                            m_vSearchData(ACIClientName, iCnt) = vSearchDataTemp(ACIClientName, iLoop1)

                            m_vSearchData(ACIClientResolvedName, iCnt) = vSearchDataTemp(ACIClientResolvedName, iLoop1)

                            m_vSearchData(ACIAgentID, iCnt) = vSearchDataTemp(ACIAgentID, iLoop1)

                            m_vSearchData(ACIAgentRef, iCnt) = vSearchDataTemp(ACIAgentRef, iLoop1)

                            m_vSearchData(ACIAgentResolvedName, iCnt) = vSearchDataTemp(ACIAgentResolvedName, iLoop1)

                            m_vSearchData(ACIProductID, iCnt) = vSearchDataTemp(ACIProductID, iLoop1)

                            m_vSearchData(ACIProductCode, iCnt) = vSearchDataTemp(ACIProductCode, iLoop1)

                            m_vSearchData(ACISourceID, iCnt) = vSearchDataTemp(ACISourceID, iLoop1)

                            m_vSearchData(ACISource, iCnt) = vSearchDataTemp(ACISource, iLoop1)

                            m_vSearchData(ACICurrencyID, iCnt) = vSearchDataTemp(ACICurrencyID, iLoop1)

                            m_vSearchData(ACICurrencyCode, iCnt) = vSearchDataTemp(ACICurrencyCode, iLoop1)

                            m_vSearchData(ACIPremium, iCnt) = vSearchDataTemp(ACIPremium, iLoop1)

                            m_vSearchData(ACIInsuranceFileType, iCnt) = vSearchDataTemp(ACIInsuranceFileType, iLoop1)

                            m_vSearchData(ACIAgentType, iCnt) = vSearchDataTemp(ACIAgentType, iLoop1)

                            m_vSearchData(ACIAgentCommission, iCnt) = vSearchDataTemp(ACIAgentCommission, iLoop1)

                            m_vSearchData(ACIRoundAmount, iCnt) = vSearchDataTemp(ACIRoundAmount, iLoop1)


                            iCnt += 1

                        End If
                    Next
                End If
                'Developer Guide No. 146
                ' Array.Clear(vSearchDataTemp, 0, vSearchDataTemp.Length)
                vSearchDataTemp = Nothing
                'Developer Guide No. 146
                'Array.Clear(vRiskIndexSearchData, 0, vRiskIndexSearchData.Length)
                vRiskIndexSearchData = Nothing
            ElseIf vRiskIndexSearchString.Trim().Length > 0 Then
                'Developer Guide No. 146
                'Array.Clear(m_vSearchData, 0, m_vSearchData.Length)
                m_vSearchData = Nothing
            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function


    Public Function DataToInterface() As Integer

        'This will populate values to the lvwsearchdetails from m_vSearchData.
        'It will filter the results selected Products and Selected QuoteType

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwsearchdetails.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                DisplayStatusFound()
                If (m_sRunningMethodName.IndexOf("MakePayment") + 1) = 0 Then
                    MessageBox.Show("No matching data found! ", "Find Quote", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Return result
            End If
            RemoveHandler lvwsearchdetails.ItemChecked, AddressOf lvwsearchdetails_ItemChecked
            ' Assign the details to the interface.
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)


                oListItem = lvwsearchdetails.Items.Add(CStr(m_vSearchData(ACIInsuranceRef, lRow)).Trim())

                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACICurrencyCode, lRow))).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACIInsuranceFileCnt, lRow))).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACIClientResolvedName, lRow))).Trim()
                'Developer Guide No 149
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = gPMFunctions.ToSafeString(Convert.ToString(m_vSearchData(ACIAgentResolvedName, lRow))).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACIProductCode, lRow))).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 6).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACISource, lRow))).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, 7).Text = CStr(gPMFunctions.ToSafeCurrency(CStr(m_vSearchData(ACIPremium, lRow)))) & "(" & CStr(m_vSearchData(ACICurrencyCode, lRow)).Trim() & ")"
                ListViewHelper.GetListViewSubItem(oListItem, 8).Text = CStr(gPMFunctions.ToSafeLong(CStr(m_vSearchData(ACIInsuranceFileType, lRow))))


                If Convert.IsDBNull(m_vSearchData(ACIAgentType, lRow)) Or IsNothing(m_vSearchData(ACIAgentType, lRow)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, 9).Text = ""
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, 9).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACIAgentType, lRow)).Trim())
                End If

                ListViewHelper.GetListViewSubItem(oListItem, 10).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACIProductID, lRow)).Trim())
                ListViewHelper.GetListViewSubItem(oListItem, 11).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACIClientID, lRow)).Trim())
                ListViewHelper.GetListViewSubItem(oListItem, 12).Text = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACIRoundAmount, lRow)).Trim())


                ' Set the tag property with the index of the search data storage.
                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows, to allow the user to see the results instantly.

                If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    lvwsearchdetails.Items.Item(0).Selected = True
                    lvwsearchdetails.Refresh()
                End If
            Next lRow
            AddHandler lvwsearchdetails.ItemChecked, AddressOf lvwsearchdetails_ItemChecked
            ' lvwsearchdetails.ListItems(1).Selected = True
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                'Developer Guide No 243
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbstatus_Panel1.Text = " " & sMessage
            Application.DoEvents()

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Function ProcessValidation(ByRef r_cNetAmountPayable As Decimal, ByRef r_bMultiplePoliciesSelected As Boolean) As Integer


        Dim result As Integer = 0
        Dim iSelectedRows As Integer
        Dim sBranch, sAgent As String

        Dim iCurrencyType As Integer


        Dim sSelectedBranch, sSelectedCurrency, sSelectedAgent, sSelectedClient, sSelectedAgentType As String

        Dim sErrorMessage As String = ""

        Dim cNetAmountPayable As Decimal

        Dim bAgentValidated, bClientValidated As Boolean

        Dim lSelectedAgentCnt, lSelectedClientCnt As Integer

        Dim bSelected As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMFalse



            bSelected = False
            'Check if multiple quotes are selected
            For Each lvwSelectedItem As ListViewItem In lvwsearchdetails.Items
                If lvwSelectedItem.Checked Then
                    sSelectedBranch = ListViewHelper.GetListViewSubItem(lvwSelectedItem, Column6).Text
                    sSelectedCurrency = ListViewHelper.GetListViewSubItem(lvwSelectedItem, Column1).Text
                    sSelectedAgent = ListViewHelper.GetListViewSubItem(lvwSelectedItem, Column4).Text
                    lSelectedAgentCnt = gPMFunctions.ToSafeLong(CStr(m_vSearchData(ACIAgentID, lvwSelectedItem.Index + 1 - 1)))
                    sSelectedClient = ListViewHelper.GetListViewSubItem(lvwSelectedItem, Column3).Text
                    lSelectedClientCnt = gPMFunctions.ToSafeLong(CStr(m_vSearchData(ACIClientID, lvwSelectedItem.Index + 1 - 1)))
                    m_lInsuranceFileCnt = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvwSelectedItem, Column2).Text)

                    If Convert.IsDBNull(m_vSearchData(ACIAgentType, lvwSelectedItem.Index + 1 - 1)) Or IsNothing(m_vSearchData(ACIAgentType, lvwSelectedItem.Index + 1 - 1)) Then
                        sSelectedAgentType = ""
                    Else
                        sSelectedAgentType = gPMFunctions.ToSafeString(CStr(m_vSearchData(ACIAgentType, lvwSelectedItem.Index + 1 - 1)).Trim())
                    End If
                    bSelected = True
                    Exit For
                End If
            Next lvwSelectedItem

            If Not bSelected Then
                sErrorMessage = "Select atleast one row to Proceed! "
                Return result
            End If

            'Check if multiple quotes are selected
            For Each lvwSelectedItem As ListViewItem In lvwsearchdetails.Items

                If lvwSelectedItem.Checked Then
                    If ListViewHelper.GetListViewSubItem(lvwSelectedItem, Column6).Text <> sSelectedBranch Then
                        sErrorMessage = "The selected row (" & lvwSelectedItem.Index + 1 & ") Branch " & _
                                        Strings.Chr(13) & Strings.Chr(10) & " is not the same as other selected rows. Cannot continue!"
                    End If
                    If ListViewHelper.GetListViewSubItem(lvwSelectedItem, Column1).Text <> sSelectedCurrency Then
                        sErrorMessage = "The selected row (" & lvwSelectedItem.Index + 1 & ") Currency " & _
                                        Strings.Chr(13) & Strings.Chr(10) & " is not the same as other selected rows. Cannot continue!"
                    End If

                    '    If lvwSelectedItem.SubItems(Column4) <> sSelectedAgent Then
                    '        sErrorMessage = "The selected row (" & lvwSelectedItem.Index & ") Agent " & _
                    ''                        vbCrLf + " is not the same as other selected rows. Cannot continue!"
                    '
                    '    ElseIf lvwSelectedItem.SubItems(Column3) <> sSelectedClient Then
                    '        sErrorMessage = "The selected row (" & lvwSelectedItem.Index & ") Client " & _
                    ''                        vbCrLf + " is not the same as other selected rows. Cannot continue!"
                    '
                    '    End If

                    If sSelectedAgentType = "Broker" Or sSelectedAgentType = "Intermed" Then
                        If ListViewHelper.GetListViewSubItem(lvwSelectedItem, Column9).Text <> "Broker" And ListViewHelper.GetListViewSubItem(lvwSelectedItem, Column9).Text <> "Intermed" Then
                            MessageBox.Show("All The Selected Policies Should be Either Direct Business/Commission Agent or Broker/Intermediatry", "Quote Collection Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return result
                        End If
                        If ListViewHelper.GetListViewSubItem(lvwSelectedItem, Column4).Text <> sSelectedAgent Then
                            MessageBox.Show("All The Selected Policies Should Belong to Same Agent if Broker/Intermediatry", "Quote Collection Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return result
                        End If
                    End If

                    If sSelectedAgentType = "Comm Acc" Or sSelectedAgentType = "" Then
                        If ListViewHelper.GetListViewSubItem(lvwSelectedItem, Column9).Text <> "Comm Acc" And ListViewHelper.GetListViewSubItem(lvwSelectedItem, Column9).Text <> "" Then
                            MessageBox.Show("All The Selected Policies Should be Either Direct Business/Commission Agent or Broker/Intermediatry", "Quote Collection Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return result
                        End If
                        If ListViewHelper.GetListViewSubItem(lvwSelectedItem, Column3).Text <> sSelectedClient Then
                            MessageBox.Show("All The Selected Policies Should Belong to Same Client if Direct Business/Commission Agent", "Quote Collection Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return result
                        End If
                    End If

                    cNetAmountPayable += gPMFunctions.ToSafeCurrency(CStr(CDbl(m_vSearchData(ACIPremium, lvwSelectedItem.Index + 1 - 1)) - CDbl(m_vSearchData(ACIAgentCommission, lvwSelectedItem.Index + 1 - 1))))
                    iSelectedRows += 1
                End If

                If sErrorMessage.Trim().Length > 0 Then Return result

            Next lvwSelectedItem



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Process Validation  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessValidation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            Return result

        Finally

            If sErrorMessage.Trim().Length = 0 Then

                r_cNetAmountPayable = cNetAmountPayable
                r_bMultiplePoliciesSelected = iSelectedRows > 1

                m_lAgentCnt = lSelectedAgentCnt
                m_lPartyCnt = lSelectedClientCnt

                If r_bMultiplePoliciesSelected Then
                    If sSelectedAgent.Trim().Length > 0 Then
                        m_lPartyCnt = 0
                    ElseIf sSelectedClient.Trim().Length > 0 Then
                        m_lAgentCnt = 0
                    End If
                End If
                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                m_lInsuranceFileCnt = 0
                MessageBox.Show(sErrorMessage, "Quote Collection Process", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            If Not r_bMultiplePoliciesSelected Then
                m_lReturn = CType(CheckAgentTypeToCashListItem(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ProcessValidation", "Error in calling CheckAgentTypeToCashListItem", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If


        End Try
        Return result
    End Function


    Private Function ProcessCollection(ByVal v_cNetAmountPayable As Decimal, ByVal v_bMultiplePoliciesSelected As Boolean) As Integer
        Dim result As Integer = 0
        Dim iPMUPaynowOptions As Object
        Const kMethodName As String = "ProcessCollection"
        Try


            Dim m_lAccountId As Integer

            Dim oPayNow As iPMUPayNowOptions.Interface_Renamed
            Dim cGrossTotal As Decimal

            Dim m_lPaymentAccountID As Integer
            Dim m_iDebitAgainst As Integer
            Dim m_vCreditTransactions As Object
            Dim m_lCashListID, m_lCashListItemID, m_lTransactionID, lInsuranceFile As Integer
            Dim sTransactionType As String = ""
            Dim cPremium As Decimal
            Dim lInsurance_file_Type_ID As Integer
            Dim m_vLetters As Object
            Dim m_bLetterPrint As Boolean
            Dim lProductId, lClientId As Integer
            Dim cRoundAmount As Decimal

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            Dim temp_oPayNow As Object
            'Developer Guide No 218
            m_lReturn = g_oObjectManager.GetInstance(temp_oPayNow, sClassName:="iPMUPayNowOptions.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oPayNow = temp_oPayNow
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Cannot Create iPMUPaynowOptions", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_bAgentTypeBroker Then

                oPayNow.ClientCnt = 0
            Else

                oPayNow.ClientCnt = m_lPartyCnt
            End If

            oPayNow.AgentCnt = m_lAgentCnt

            oPayNow.AmountDue = v_cNetAmountPayable

            oPayNow.PaymentOption = "paynow"
            If Not v_bMultiplePoliciesSelected Then

                oPayNow.InsuranceFileCnt = m_lInsuranceFileCnt
            End If

            oPayNow.MultiplePoliciesSelected = v_bMultiplePoliciesSelected

            oPayNow.CallingAppName = ACApp

            oPayNow.LetterPrint = True

            ' Start the component

            m_lReturn = oPayNow.Start

            If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Cannot Run iPMUPaynowOptions.Start", gPMConstants.PMELogLevel.PMLogError)
            End If



            If oPayNow.OKClick Then
                'Get Values from iPMUPaynowOptions for GetKeys()

                m_lPaymentAccountID = oPayNow.PaymentAccountID

                m_iDebitAgainst = oPayNow.DebitAgainst



                m_vCreditTransactions = oPayNow.CreditTransactions


                m_lCashListID = oPayNow.CashListID

                m_lCashListItemID = oPayNow.CashListItemID

                m_lTransactionID = oPayNow.CashTransDetailID


                m_vLetters = oPayNow.Letters

                m_bLetterPrint = oPayNow.LetterPrint
                'In case other form is shown debitagainst ='' credittransactions=""
            Else
                result = gPMConstants.PMEReturnCode.PMCancel
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If


            oPayNow.Dispose()

            oPayNow = Nothing

            For lRow As Integer = 1 To lvwsearchdetails.Items.Count
                If lvwsearchdetails.Items.Item(lRow - 1).Checked Then
                    lInsuranceFile = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvwsearchdetails.Items.Item(lRow - 1), Column2).Text)
                    lInsurance_file_Type_ID = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvwsearchdetails.Items.Item(lRow - 1), Column8).Text)
                    lProductId = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvwsearchdetails.Items.Item(lRow - 1), Column10).Text)
                    lClientId = gPMFunctions.ToSafeLong(ListViewHelper.GetListViewSubItem(lvwsearchdetails.Items.Item(lRow - 1), Column11).Text)

                    'sTransactionType = Figure out the type of the transaction i.e. NB/MTA/MTC/MTR/REN
                    If lInsurance_file_Type_ID = 1 Then
                        sTransactionType = "NB"
                    ElseIf lInsurance_file_Type_ID = 2 Then
                        sTransactionType = "REN"
                    ElseIf lInsurance_file_Type_ID = 10 Then
                        sTransactionType = "MTR"
                    Else
                        sTransactionType = "MTA"
                    End If

                    'strip the amount
                    cPremium = gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwsearchdetails.Items.Item(lRow - 1), Column7).Text.Substring(0, Math.Min(ListViewHelper.GetListViewSubItem(lvwsearchdetails.Items.Item(lRow - 1), Column7).Text.Length, ListViewHelper.GetListViewSubItem(lvwsearchdetails.Items.Item(lRow - 1), Column7).Text.IndexOf("("c))))
                    cRoundAmount = gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lvwsearchdetails.Items.Item(lRow - 1), Column12).Text)


                    m_lReturn = CType(ProcessMakeLive(lInsuranceFile, sTransactionType), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ProcessMakeLive Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    m_lReturn = CType(GetStats(lInsuranceFile, m_vCreditTransactions, sTransactionType, m_lPaymentAccountID, m_iDebitAgainst, m_lCashListID, m_lCashListItemID, m_lTransactionID, cPremium, cRoundAmount), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetStats Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    'Check Whether Product Risk Option is on for Producing Schedule, Certificate and Debit Note if Yes then call the GenerateDocument method for each of them. By Passing Document Type as 3 (DebitNote), 4 (Schedule), 5 (Certificate). GenerateDocument is defined below.
                    m_lReturn = GenerateDocuments(lProductId, lInsuranceFile, lClientId, sTransactionType)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GenerateDocuments Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            Next
            If m_bLetterPrint Then

                m_lReturn = CType(ProcessLetters(m_vLetters), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetStats Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Process Validation  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result


        Finally

            If txtQuoteRef.Text.Trim().Length = 0 Then
                m_lInsuranceFileCnt = 0
            End If
            If txtClient.Text.Trim().Length = 0 Then
                m_lPartyCnt = 0
            End If
            If txtAgent.Text.Trim().Length = 0 Then
                m_lAgentCnt = 0
            End If
            If txtProduct.Text.Trim().Length = 0 Then

                m_vSelectedProducts = ""
                txtProduct.Tag = ""
            End If

            If Convert.IsDBNull(dtpStartDate.Value) Or IsNothing(dtpStartDate.Value) Then
                'm_dtStartDate = Null
                m_dtStartDate = #12:00:00 AM#
            End If

            If Convert.IsDBNull(dtpEndDate.Value) Or IsNothing(dtpEndDate.Value) Then
                'm_dtEndDate = Null
                m_dtStartDate = #12:00:00 AM#
            End If

            cmdFindNow_Click(cmdFindNow, New EventArgs())

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)






        End Try
        Return result
    End Function


    'ProcessMakeLive (New)
    Public Function ProcessMakeLive(ByVal lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String) As Integer
        Dim result As Integer = 0
        Dim bSIRListRisks As Object

        Const kMethodName As String = "ProcessMakeLive"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vAnswer As DialogResult
        Dim vSelectionArray As Object
        Dim lValidationStatus As Integer
        Dim m_sFailureReason As String = ""
        Dim vReturnArray As Object '(RC) IH-UDPP
        Dim bCanOverride As Boolean '(RC) IH-UDPP
        Dim bAlreadyLive As Boolean
        'PN 52000
        Dim obSIRInsuranceFile As Object
        Dim iLoop As Integer

        Dim m_oListRiskBusiness As bSIRListRisks.Business
        Dim lInsuranceFolderCnt As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_m_oListRiskBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oListRiskBusiness, "bSIRListRisks.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oListRiskBusiness = temp_m_oListRiskBusiness

            If v_sTransactionType = "MTA" Then

                m_lReturn = m_oListRiskBusiness.CopyRisksMTA(lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Unable to copy Risks for MTA", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            lReturn = CType(UpdatePolicyPremium(v_lInsuranceFileCnt:=lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePolicyPremium Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            ' process change policy status
            lReturn = CType(ProcessChangePolicyStatus(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_sTransactionType:=v_sTransactionType), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ProcessChangePolicyStatus", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' update policy details
            lReturn = CType(UpdatePolicyDetails(v_lInsuranceFileCnt:=lInsuranceFileCnt, v_oListRiskBusiness:=m_oListRiskBusiness), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' reset any applied discount fields
            lReturn = CType(ProcessPolicyMakeLive(v_lInsuranceFileCnt:=CStr(lInsuranceFileCnt), m_oListRiskBusiness:=m_oListRiskBusiness), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ProcessPolicyMakeLive Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            'END PN 52000
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Process Make Live Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            Return result
        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    'UpdatePolicyPremium (New)
    Private Function UpdatePolicyPremium(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePolicyPremium"

        Dim lReturn As gPMConstants.PMEReturnCode


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' create instance of object
            Dim temp_m_oChangePolicyStatus As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oChangePolicyStatus, "bSIRChangePolicyStatus.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oChangePolicyStatus = temp_m_oChangePolicyStatus
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get instance of bSIRChangePolicyStatus.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' update policy premium

            lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePolicyPremium Failed", gPMConstants.PMELogLevel.PMLogError)
            End If




        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Policy Premium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            Return result

        Finally

            ' destroy object instance





        End Try
        Return result
    End Function

    'ProcessChangePolicyStatus (New)
    Private Function ProcessChangePolicyStatus(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessChangePolicyStatus"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oChangePolicyStatus As Object
        Dim vRisks As Object
        Dim sMessage As String = ""
        Dim lLevel, lRisk, lLbound, lUbound As Integer
        Dim bSelectedRisks As Boolean
        'Developer Guide No.17
        Dim m_vRisks As Object
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'if we're really making the policy live...


            lReturn = m_oChangePolicyStatus.GetRisksByStatus(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vRisks:=m_vRisks)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetRisksByStatus Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



            ' If going live, delete any unselected risks' link
            '            records

            lReturn = m_oChangePolicyStatus.DeleteRisks(v_vrisks:=m_vRisks)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "DeleteRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Re-jig the risk and variation numbers of the remaining
            '            risks on this policy

            lReturn = m_oChangePolicyStatus.RenumberRisks(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RenumberRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_vRisks = ""



            m_oChangePolicyStatus.Mode = 0

            m_oChangePolicyStatus.TransactionType = v_sTransactionType
            'Start (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)
            'Added the optional parameters

            lReturn = m_oChangePolicyStatus.ChangePolicyStatus(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
            'End (Girija chokkalingam) - (Tech Spec - SFI QBENZCR003 - Back Dated MTAs ) - (not mentioned in the spec)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ChangePolicyStatus Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePolicyPremium Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Policy Premium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            Return result

        Finally




        End Try
        Return result
    End Function

    'UpdatePolicyDetails(New)
    Private Function UpdatePolicyDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_oListRiskBusiness As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePolicyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPutOnNextInstalmentRenewal, lMarkedForCollection As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Debug.Print m_oBusiness.PMProductFamily

            ' get details to update
            lPutOnNextInstalmentRenewal = 0

            lMarkedForCollection = 1

            ' update policy

            lReturn = v_oListRiskBusiness.UpdatePolicyDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lPutOnNextInstalmentRenewal:=lPutOnNextInstalmentRenewal, v_sPaymentMethod:="PayNow")
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePolicyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Policy Details Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            Return result


        Finally




        End Try
        Return result
    End Function

    'ProcessPolicyMakeLive (New)
    Private Function ProcessPolicyMakeLive(ByVal v_lInsuranceFileCnt As String, ByVal m_oListRiskBusiness As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPolicyMakeLive"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lPolicyDiscountStatus As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' process all policy discount make live steps

            lReturn = m_oListRiskBusiness.ProcessPolicyMakeLive(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ProcessPolicyMakeLive Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Process Policy Make Live Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            Return result

        Finally




        End Try
        Return result
    End Function

    'GetStats (New)
    Private Function GetStats(ByVal v_lInsuranceFileCnt As Integer, ByVal v_vCreditTransactions(,) As Object, ByVal v_sTransactionType As String, ByVal v_lPaymentAccountID As Integer, ByVal v_iDebitAgainst As Integer, ByVal v_lCashListID As Integer, ByVal v_lCashListItemID As Integer, ByVal v_lTransactionID As Integer, ByVal cGrossTotalForSelectedPolicy As Decimal, ByVal cRoundAmount As Decimal) As Integer

        Dim result As Integer = 0
        Dim oObject As iPMUStats.Interface_Renamed
        Dim vCreditTransactions(,) As Object

        Dim itemp As Integer

        Dim vKeys(1, 10) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oObject = New iPMUStats.Interface_Renamed()

            'Developer Guide No 9
            m_lReturn = oObject.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("error init", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vTransactionType:=v_sTransactionType)


            vKeys(0, 0) = "insurance_file_cnt"

            vKeys(1, 0) = v_lInsuranceFileCnt

            If Information.IsArray(v_vCreditTransactions) Then
                For iLV As Integer = 0 To v_vCreditTransactions.GetUpperBound(1)

                    If CDbl(v_vCreditTransactions(2, iLV)) > 0 Then
                        If Information.IsArray(vCreditTransactions) Then


                            ReDim Preserve vCreditTransactions(2, vCreditTransactions.GetUpperBound(1) + 1)
                        Else

                            ReDim vCreditTransactions(2, 0)
                        End If


                        vCreditTransactions(0, itemp) = v_vCreditTransactions(0, iLV)


                        vCreditTransactions(1, itemp) = v_vCreditTransactions(1, iLV)

                        If cGrossTotalForSelectedPolicy >= v_vCreditTransactions(2, iLV) Then


                            vCreditTransactions(2, itemp) = v_vCreditTransactions(2, iLV)


                            cGrossTotalForSelectedPolicy -= gPMFunctions.ToSafeCurrency(CStr(v_vCreditTransactions(2, iLV)))

                            v_vCreditTransactions(2, iLV) = 0
                        Else

                            vCreditTransactions(2, itemp) = cGrossTotalForSelectedPolicy


                            v_vCreditTransactions(2, iLV) = CDbl(v_vCreditTransactions(2, iLV)) - cGrossTotalForSelectedPolicy
                            cGrossTotalForSelectedPolicy = 0

                        End If
                        If cGrossTotalForSelectedPolicy <= 0 Then Exit For
                        itemp += 1
                    End If

                Next
            End If
            'If iAllPaynow Then

            vKeys(0, 1) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeys(1, 1) = v_lInsuranceFileCnt

            vKeys(0, 2) = PMNavKeyConst.PMKeyNamePaymentAccountID

            vKeys(1, 2) = v_lPaymentAccountID

            vKeys(0, 3) = PMNavKeyConst.PMKeyNameDebitAgainst

            vKeys(1, 3) = v_iDebitAgainst

            vKeys(0, 4) = PMNavKeyConst.PMKeyNameCreditTransactions


            vKeys(1, 4) = vCreditTransactions

            vKeys(0, 5) = PMNavKeyConst.PMKeyNameCashListID

            vKeys(1, 5) = v_lCashListID

            vKeys(0, 6) = PMNavKeyConst.PMKeyNameCashListItemID

            vKeys(1, 6) = v_lCashListItemID

            vKeys(0, 7) = PMNavKeyConst.PMKeyNameTransactionID

            vKeys(1, 7) = v_lTransactionID

            vKeys(0, 8) = "payment_method"

            vKeys(1, 8) = "PayNow"

            vKeys(0, 9) = "TransactionAmount"

            vKeys(1, 9) = cGrossTotalForSelectedPolicy

            vKeys(0, 10) = PMNavKeyConst.PMKeyNameRoundOffAmount

            vKeys(1, 10) = cRoundAmount



            'End If

            m_lReturn = oObject.SetKeys(vKeyArray:=vKeys)
            oObject.CallingAppName = "iPMUQuoteCollectionProcess"
            m_lReturn = oObject.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("error start", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    m_lStatus = oObject.Status

            oObject.Dispose()
            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStats Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStats", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'GenerateDocument (New)
    Private Function GenerateDocument(ByRef v_iDocType As Integer, ByRef v_lInsuranceFileCnt As Integer, ByRef v_lInsuranceFolderCnt As Integer, ByRef v_lPartyCnt As Integer, ByRef v_sTransType As String, ByRef v_lProductId As Integer) As Integer

        Dim result As Integer = 0

        Dim oGetDocument As iPMUGetDocument.Interface_Renamed
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeyArray(1, 4)



            'Generate document.
            oGetDocument = New iPMUGetDocument.Interface_Renamed()

            If oGetDocument Is Nothing Then

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create iPMUGetDocument object", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Guide No 9
            oGetDocument.Initialise()


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameInsFileCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lInsuranceFileCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameDocumentID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_iDocType


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameInsFolderCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_lInsuranceFolderCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_lPartyCnt


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameProductID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = v_lProductId



            m_lReturn = oGetDocument.SetKeys(vKeyArray:=vKeyArray)

            oGetDocument.FunctionalArea = 1
            oGetDocument.SlientMode = True
            oGetDocument.DocumentDescription = "Policy Made Live Through Quote Collection"
            oGetDocument.TransactionType = v_sTransType

            m_lReturn = oGetDocument.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oGetDocument.Dispose()
            oGetDocument = Nothing



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (ValidateIndex) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ValidateIndex() As Integer
    '
    'Dim result As Integer = 0
    'Dim lReturn As Integer
    'Dim sIndex As String = ""
    'Dim vGISSearchDataArray As Object
    '
    'Try 
    '
    'Const csGISDataModelTypeClaim As String = ""
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'sIndex = txtRiskIndex.Text.Trim()
    '
    'DC310701 changed ACMaxSearchDetails to PMAllRecords

    'lReturn = g_oBackofficelink.FindLikeIndex(sIndex:=sIndex, lNumberOfRecords:=gPMConstants.PMAllRecords, vResultArray:=vGISSearchDataArray, sDataModelType:=csGISDataModelTypeClaim)
    '
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to GetAllGISSearchResults", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateIndex")
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    '
    'Else
    '
    ' We have the Insurance File Cnt
    '        lReturn& = g_oBusiness.GetMultiPolicyClaims(vGISSearchDataArray, m_vSearchData, _
    ''                                                        v_vsiriusproduct:="U", _
    ''                                                        v_vClientName:=txtPolicyHolder.Text, _
    ''                                                        v_vPolicyNumber:=txtQuoteRef.Text, _
    ''                                                        v_vRegNumber:=txtRegNumber.Text, _
    ''                                                        v_vLossFromDate:=m_vLossFromDate, _
    ''                                                        v_vLossToDate:=m_vLossToDate, _
    ''                                                        v_vClaimStatus:=m_bClaimStatus)
    ''
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetUWPolicyByGISSearchIndex failed to get Policy Details", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateIndex")
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' If NO Indexes were found return Not Found
    'If Not Information.IsArray(m_vSearchData) Then
    'result = gPMConstants.PMEReturnCode.PMNotFound
    'End If
    '
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate index", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '


    '
    'Return result
    'End Try
    'End Function

    Private Sub DisplayStatusFound()

        Static sMessage As String = ""
        Dim lItemsFound As Integer

        Try

            ' Store the total of item found.
            If Not Information.IsArray(m_vSearchData) Then
                lItemsFound = 0
            Else
                lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
            End If

            ' Get message text if not already present.
            If sMessage = "" Then

                'Developer Guide No 243
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbstatus_Panel1.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


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

            ' Update the interface details.

            If m_iTaskVia = 1 Then
                'txtQuoteRef.Text = Trim(m_sInsuranceFileRef$)
                'txtAgent.Text = Trim(m_sAgentCode$)
                'txtClient.Text = Trim(m_sPartyCode$)
                txtQuoteRef.Text = m_sTaskQuote
                txtClient.Text = m_sTaskParty
                txtAgent.Text = m_sTaskAgent
                m_vProductID = m_sTaskProduct
                txtProduct.Tag = m_sTaskProduct
                txtProduct.Text = m_sTaskProductText
                txtRiskIndex.Text = m_sTaskRiskIndex
                m_lIsDirectBusiness = m_lTaskDirectBusiness
                chkDirectBusiness.CheckState = m_lTaskDirectBusiness
                If Information.IsDate(m_dtTaskFromDate) And m_dtTaskFromDate <> #12:00:00 AM# Then
                    dtpStartDate.ShowCheckBox = True
                    dtpStartDate.Value = DateTime.FromOADate(True)
                    m_dtStartDate = m_dtTaskFromDate
                    dtpStartDate.Value = m_dtTaskFromDate
                End If
                If Information.IsDate(m_dtTaskToDate) And m_dtTaskToDate <> #12:00:00 AM# Then
                    dtpEndDate.ShowCheckBox = True
                    dtpEndDate.Value = DateTime.FromOADate(True)
                    m_dtEndDate = m_dtTaskToDate
                    dtpEndDate.Value = m_dtTaskToDate
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (GetComboDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetComboDetails(ByRef r_cboControl As ComboBox, ByVal v_sTableName As String) As Integer
    '
    'Dim result As Integer = 0
    'Dim vResultArray(,) As Object
    '
    'result = gPMConstants.PMEReturnCode.PMTrue 'PMFalse
    '
    'Try 
    '
    'make sure combobox is empty
    'r_cboControl.Items.Clear()
    '
    'add in non applicable value with ID of 0
    'Dim r_cboControl_NewIndex As Integer = -1
    'r_cboControl_NewIndex = r_cboControl.Items.Add("All")
    'VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, 0)
    '
    'Select Case v_sTableName.ToUpper()
    'Case "PRODUCT"

    'm_lReturn = m_oBusiness.GetLookUp(v_sTableName:="Product", v_sKeyIDFieldName:="product_id", v_sDescFieldName:="description", r_vResultArray:=vResultArray)
    '
    'Case "SOURCE"

    'm_lReturn = m_oBusiness.GetLookUp(v_sTableName:="Source", v_sKeyIDFieldName:="source_id", v_sDescFieldName:="description", r_vResultArray:=vResultArray)
    '
    'End Select
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'If Information.IsArray(vResultArray) Then

    'For 'iCount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

    'r_cboControl_NewIndex = r_cboControl.Items.Add(CStr(vResultArray(1, iCount)))

    'VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, CInt(vResultArray(0, iCount)))
    'Next 
    'End If
    '
    'default to all products
    'r_cboControl.SelectedIndex = 0
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetComboDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Sub lvwsearchdetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwsearchdetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwsearchdetails.Columns(eventArgs.Column)


        StoreHScrollValue()


        ' Column click event for the List details
        Try
            RemoveHandler lvwsearchdetails.ItemChecked, AddressOf lvwsearchdetails_ItemChecked
            Dim iDirection As SortOrder

            If ListViewHelper.GetSortOrderProperty(lvwsearchdetails) = 1 Then
                iDirection = SortOrder.Descending
            Else
                iDirection = SortOrder.Ascending
            End If

            With lvwsearchdetails
                If eventArgs.Column <> sortColumn Then
                    sortColumn = eventArgs.Column
                    .Sorting = SortOrder.Ascending
                Else
                    If .Sorting = SortOrder.Ascending Then
                        .Sorting = SortOrder.Descending
                    Else
                        .Sorting = SortOrder.Ascending
                    End If

                End If
                Select Case ColumnHeader.Text
                    Case "Amount Due"

                        ListViewSortByStringVal(v_oListView:=lvwsearchdetails, v_iSourceColumn:=ColumnHeader.Index, v_iDirection:=iDirection)
                    Case Else
                        .Sort()
                        .ListViewItemSorter = New ListViewItemComparer(eventArgs.Column, .Sorting)
                End Select


            End With
            AddHandler lvwsearchdetails.ItemChecked, AddressOf lvwsearchdetails_ItemChecked
            RecoverHorizontalScroll()
        Catch excep As System.Exception





            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwsearchdetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtAgent_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgent.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If txtAgent.Text <> Convert.ToString(txtAgent.Tag) Then m_lAgentCnt = 0
    End Sub

    Private Sub txtClient_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClient.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If txtClient.Text <> Convert.ToString(txtClient.Tag) Then m_lPartyCnt = 0
    End Sub

    Private Sub txtQuoteRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtQuoteRef.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If txtQuoteRef.Text <> Convert.ToString(txtQuoteRef.Tag) Then m_lInsuranceFileCnt = 0
    End Sub

    Public Function IsInArray(ByRef FindValue As String, ByRef arrSearch() As Object) As Boolean
        Try
            If Not Information.IsArray(arrSearch) Then Exit Function

            Return (Strings.Chr(0) & String.Join(Strings.Chr(0), arrSearch) & _
                                Strings.Chr(0)).IndexOf(Strings.Chr(0) & FindValue & Strings.Chr(0)) >= 0

        Catch

            'Justin (just in case)
        End Try
    End Function

    Private Function CreateWorkManagerTask() As Integer
        Dim result As Integer = 0
        Dim bPMLookup, iPMWrkTaskInstance As Object
        Const kMethodName As String = "SelectParty"

        Dim oWrkTaskInstance As iPMWrkTaskInstance.NavigatorV3

        Dim oPMLookUp As bPMLookup.Business
        Dim lTaskID, lTaskGroupID As Integer
        Dim vKeys As Object
        Dim sTaskDesc As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeys(1, 17)

            ' Change the cursor mode
            iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Object to create work manager tasks
            Dim temp_oWrkTaskInstance As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oWrkTaskInstance, "iPMWrkTaskInstance.NavigatorV3", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oWrkTaskInstance = temp_oWrkTaskInstance
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of iPMWrkTaskInstance.NavigatorV3")
                ' Change the cursor mode
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Set to ADD mode
            m_lReturn = CType(oWrkTaskInstance.NavigatorV3_SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oWrkTaskInstance.NavigatorV3_SetProcessModes Failed")
                ' Change the cursor mode
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Set the authority level
            ''oWrkTaskInstance.NavigatorV3_PMAuthorityLevel = m_lPMAuthorityLevel&


            ' Create an instance of bPMLookup
            Dim temp_oPMLookUp As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLookUp, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLookUp = temp_oPMLookUp
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of bPMLookup.Business")
                ' Change the cursor mode
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Set the product family

            oPMLookUp.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusUnderwriting

            ' Use the lookup to get the ID of the PMTMAINT task

            m_lReturn = oPMLookUp.GetEffectiveIDFromCode(v_sTableName:="pmwrk_task", v_sCode:="QUCOLLECTP", v_dtEffectiveDate:=DateTime.Now, r_lID:=lTaskID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to GetEffectiveIDFromCode for " & Environment.NewLine & _
                                        "TableName: pmwrk_task" & Environment.NewLine & _
                                        "Code: PMTMAINT" & Environment.NewLine & _
                                        "EffectiveDate: " & DateTimeHelper.ToString(DateTime.Today))
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Use the lookup to get the ID of the SLACS task group

            m_lReturn = oPMLookUp.GetEffectiveIDFromCode(v_sTableName:="pmwrk_task_group", v_sCode:="SLACS", v_dtEffectiveDate:=DateTime.Now, r_lID:=lTaskGroupID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to GetEffectiveIDFromCode for " & Environment.NewLine & _
                                        "TableName: pmwrk_task_group" & Environment.NewLine & _
                                        "Code: SLACS" & Environment.NewLine & _
                                        "EffectiveDate: " & DateTimeHelper.ToString(DateTime.Today))
                ' Change the cursor mode
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Remove instance of lookup

            oPMLookUp.Dispose()
            oPMLookUp = Nothing

            sTaskDesc = "Quote Collection Process"
            ' Set up the key array

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameTaskGroupCode

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = "SLACS"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameTaskID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = lTaskID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNameTaskCode

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = "QUCOLLECTP"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameTaskDescription

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = sTaskDesc

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameTaskCustomer

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = "Customer"

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.PMKeyNameTaskDueDate

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = DateTime.Today.AddDays(1).AddSeconds(-1)

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.PMKeyNameTaskIsUrgent

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = gPMConstants.PMEReturnCode.PMTrue

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMNavKeyConst.PMKeyNameTaskGroupID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = lTaskGroupID
            'WPR12

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMNavKeyConst.PMKeyNameTaskQuote

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = txtQuoteRef.Text.Trim()


            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMNavKeyConst.PMKeyNameTaskClient

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = txtClient.Text.Trim()


            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMNavKeyConst.PMKeyNameTaskAgent

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = txtAgent.Text.Trim()


            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = PMNavKeyConst.PMKeyNameTaskProduct

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = Convert.ToString(txtProduct.Tag).Trim()


            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 12) = PMNavKeyConst.PMKeyNameTaskRiskIndex

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 12) = txtRiskIndex.Text.Trim()


            If Not (Convert.IsDBNull(dtpStartDate.Value) Or IsNothing(dtpStartDate.Value)) Then

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 13) = PMNavKeyConst.PMKeyNameTaskFromDate


                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 13) = dtpStartDate.Value
            Else

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 13) = PMNavKeyConst.PMKeyNameTaskFromDate

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 13) = #12:00:00 AM#
            End If


            If Not (Convert.IsDBNull(dtpEndDate.Value) Or IsNothing(dtpEndDate.Value)) Then

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 14) = PMNavKeyConst.PMKeyNameTaskToDate


                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 14) = dtpEndDate.Value
            Else

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 14) = PMNavKeyConst.PMKeyNameTaskToDate

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 14) = #12:00:00 AM#
            End If


            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 15) = PMNavKeyConst.PMKeyNameTaskDirectBusiness

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 15) = chkDirectBusiness.CheckState


            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 16) = PMNavKeyConst.PMKeyNameTaskVia

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 16) = 1


            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 17) = PMNavKeyConst.PMKeyNameTaskProductText

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 17) = txtProduct.Text.Trim()

            'End

            ' Pass the keys in
            m_lReturn = CType(oWrkTaskInstance.NavigatorV3_SetKeys(vKeyArray:=vKeys), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to set keys.")
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Change the cursor mode
            iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Display the form

            m_lReturn = oWrkTaskInstance.NavigatorV3_Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to start WrkTaskInstance")
            End If


            If oWrkTaskInstance.NavigatorV3_Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
            End If



        Catch ex As Exception

            ' Log Error
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwsearchdetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            ' Terminate the object

            oWrkTaskInstance.Dispose()


        End Try
        Return result
    End Function

    Private Function CheckAgentTypeToCashListItem() As Integer
        Dim result As Integer = 0
        Dim bSIRPayNowOptions As Object

        Const kMethodName As String = "CheckAgentTypeToCashListItem"

        Dim lCheckAgentCnt As Integer

        Dim obPayNowOption As bSIRPayNowOptions.Business
        Dim vResultArray(,) As Object
        Dim sAgentType As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            lCheckAgentCnt = 0
            m_bAgentTypeBroker = False
            For Each lvwSelectedItem As ListViewItem In lvwsearchdetails.Items
                If lvwSelectedItem.Checked Then
                    If lCheckAgentCnt = 0 Then
                        lCheckAgentCnt = gPMFunctions.ToSafeLong(CStr(m_vSearchData(ACIAgentID, lvwSelectedItem.Index + 1 - 1)))
                    Else
                        If lCheckAgentCnt <> gPMFunctions.ToSafeLong(CStr(m_vSearchData(ACIAgentID, lvwSelectedItem.Index + 1 - 1))) Then
                            lCheckAgentCnt = 0
                            Exit For
                        End If
                    End If
                End If
            Next lvwSelectedItem
            If lCheckAgentCnt > 0 Then
                'Check if its broker then reset the client to 0 - no need to pass client at all
                Dim temp_obPayNowOption As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_obPayNowOption, "bSIRPayNowOptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                obPayNowOption = temp_obPayNowOption
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Cannot Create bSIRPayNowOptions", gPMConstants.PMELogLevel.PMLogError)
                End If


                m_lReturn = obPayNowOption.GetAgentDetailsFromAgentID(lCheckAgentCnt, vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Error in calling GetAgentDetailsFromAgentID-bSIRPayNowOptions", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Information.IsArray(vResultArray) Then

                    sAgentType = CStr(vResultArray(0, 0)).Trim()
                    If sAgentType.ToUpper() = "BROKER" Then
                        m_bAgentTypeBroker = True
                    End If
                End If

            End If



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            obPayNowOption = Nothing

            iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)




        End Try
        Return result
    End Function
    Public Function ProcessLetters(ByRef m_vLetters(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sDocumentTemplateID As String = ""
        Dim lDocumentTemplateID, lDocumentTypeID As Integer
        Dim oDocTemplate As iPMBDocTemplate.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Get Default Letter template for type

            m_lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=61, r_sOptionValue:=sDocumentTemplateID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + +", Failed to read system option for Letter Template.")
            End If

            lDocumentTemplateID = gPMFunctions.ToSafeLong(sDocumentTemplateID, 0)

            ' If template has not been specified in Maintain System Options do not proceed!  PN24096
            If lDocumentTemplateID = 0 Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + +", The letter cannot be produced since the relevant document template for this process has not been configured in Maintain System Options.")
            End If

            m_lReturn = CType(GetTemplateType(lDocumentTemplateID:=lDocumentTemplateID, r_lDocumentTypeID:=lDocumentTypeID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + +", Failed to Get Template Type.")
            End If

            oDocTemplate = New iPMBDocTemplate.Interface_Renamed()

            For iIndex As Integer = 0 To m_vLetters.GetUpperBound(1)

                With oDocTemplate


                    .PartyCnt = CInt(m_vLetters(0, iIndex))

                    .DocumentRef = CStr(m_vLetters(1, iIndex)).Trim()

                    .DocumentTemplateId = lDocumentTemplateID
                    .DocumentTypeId = lDocumentTypeID
                    .Mode = 4
                    .ArchiveMode = False
                    .SpoolDesc = "Receipt Generated By Quote Collection Process"
                    'Developer Guide No 9
                    m_lReturn = oDocTemplate.Initialise()
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        oDocTemplate.AutoGenerateDocumentTask = False
                        m_lReturn = .Start()
                    End If

                End With

            Next iIndex

            oDocTemplate.Dispose()
            oDocTemplate = Nothing

            Return result

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process letters:" & Strings.Chr(13) & Strings.Chr(10) & excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessLetters", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetTemplateType
    '
    ' Description: Get the Document Template Type from the Template
    '
    ' History: 24/03/2005 DD (taken from original frmDocument)
    '
    ' ***************************************************************** '
    Private Function GetTemplateType(ByVal lDocumentTemplateID As Integer, ByRef r_lDocumentTypeID As Integer) As Integer
        Dim result As Integer = 0


        Dim oDocTemplate As bSIRDocTemplate.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If oDocTemplate Is Nothing Then
                Dim temp_oDocTemplate As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oDocTemplate, "bSIRDocTemplate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oDocTemplate = temp_oDocTemplate

                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    Return gPMConstants.PMEReturnCode.PMCancel
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If


            m_lReturn = oDocTemplate.GetDetails(vDocumentTemplateId:=lDocumentTemplateID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = oDocTemplate.GetNext(vDocumentTypeId:=r_lDocumentTypeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            oDocTemplate.Dispose()

            oDocTemplate = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTemplateType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTemplateType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GenerateDocuments(ByRef lProduct_id As Integer, ByRef lInsuranceFile As Integer, ByRef lParty_cnt As Integer, ByRef sTransactionType As String) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Dim oRenewal As bSIRRenewal.Business
        Dim vResultArray(,) As Object
        Const kMethodName As String = "GenerateDocuments"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oRenewal As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oRenewal, "bSIRRenewal.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oRenewal = temp_oRenewal

            '         Check Whether Product Risk Option is on for Producing Schedule, Certificate and Debit Note if Yes then call the GenerateDocument method for each of them. By Passing Document Type as 3 (DebitNote), 4 (Schedule), 5 (Certificate). GenerateDocument is defined below.


            m_lReturn = oRenewal.GetProdPrintOptions(lproduct_id:=lProduct_id, vPrintOptions:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed To retreive Product Risk Maintainence option for Roundoff", gPMConstants.PMELogLevel.PMLogError)
            Else
                If Information.IsArray(vResultArray) Then

                    If CDbl(vResultArray(1, 0)) = 1 Then
                        'Certificate
                        m_lReturn = CType(GenerateDocument(v_iDocType:=5, v_lInsuranceFileCnt:=lInsuranceFile, v_lInsuranceFolderCnt:=0, v_lPartyCnt:=lParty_cnt, v_sTransType:=sTransactionType, v_lProductId:=lProduct_id), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GenerateDocument Failed for Certificate", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If


                    If CDbl(vResultArray(2, 0)) = 1 Then
                        'Debit Note
                        m_lReturn = CType(GenerateDocument(v_iDocType:=3, v_lInsuranceFileCnt:=lInsuranceFile, v_lInsuranceFolderCnt:=0, v_lPartyCnt:=lParty_cnt, v_sTransType:=sTransactionType, v_lProductId:=lProduct_id), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GenerateDocument Failed for Debit Note", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If


                    If CDbl(vResultArray(0, 0)) = 1 Then
                        'Schedule
                        m_lReturn = CType(GenerateDocument(v_iDocType:=4, v_lInsuranceFileCnt:=lInsuranceFile, v_lInsuranceFolderCnt:=0, v_lPartyCnt:=lParty_cnt, v_sTransType:=sTransactionType, v_lProductId:=lProduct_id), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GenerateDocument Failed for Debit Note", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                End If
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateDocuments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocuments", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub lvwsearchdetails_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwsearchdetails.ItemChecked
        Dim Item As ListViewItem = lvwsearchdetails.Items(e.Item.Index)
        Dim bSomethingSelected As Boolean

        'strip the amount
        Dim cNetAmount As Decimal = gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(Item, Column7).Text.Substring(0, Math.Min(ListViewHelper.GetListViewSubItem(Item, Column7).Text.Length, ListViewHelper.GetListViewSubItem(Item, Column7).Text.IndexOf("("c))))

        'if nothing is selected then blank m_sSelectedCurrency
        For iCount As Integer = 1 To lvwsearchdetails.Items.Count
            If lvwsearchdetails.Items.Item(iCount - 1).Checked Then
                bSomethingSelected = True
                Exit For
            End If
        Next iCount

        If Not bSomethingSelected Then
            m_sSelectedCurrency = ""
        End If

        If Item.Checked Then
            If gPMFunctions.ToSafeString(m_sSelectedCurrency) <> "" Then
                If ListViewHelper.GetListViewSubItem(Item, Column1).Text <> m_sSelectedCurrency Then
                    Item.Checked = False
                    MessageBox.Show("Cannot Proceed as the selected currencies are different! ", "Quote Collection Process", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    m_cNetAmount += cNetAmount
                    m_sSelectedCurrency = ListViewHelper.GetListViewSubItem(Item, Column1).Text
                End If
            Else
                m_cNetAmount += cNetAmount
                m_sSelectedCurrency = ListViewHelper.GetListViewSubItem(Item, Column1).Text
            End If
        Else
            m_cNetAmount -= cNetAmount
        End If

        lblNetAmount.Text = StringsHelper.Format(m_cNetAmount, "#,##0.00")
    End Sub
    ' ***************************************************************** '
    ' Name: ListViewSortByStringValue
    '
    ' Description: Sorts the list view based on the column passed, and
    '              the order given.
    '
    ' Note : This is the copy of the original function of iPMListViewFunc.Bas
    '        with some changes particular to the issue no 32220
    ' ***************************************************************** '
    Public Function ListViewSortByStringVal(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder) As Integer

        Dim result As Integer = 0
        Const ACLVTag As String = "SORT_VALUE_HIDDEN"

        Dim cValue As Decimal
        Dim sValue As String = ""
        Dim iIndex As Integer
        Dim bNegative As Boolean
        Dim iLen As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the column
            'PSL 02/10/2003 Should be zero width as well
            v_oListView.Columns.Add(ACLVTag, "Internal Sort", CInt(VB6.TwipsToPixelsX(0)))

            ' Get the index of this new column, -1 because it's a sub item
            iIndex = v_oListView.Columns.Count - 1

            ' Not sorted yet
            ListViewHelper.SetSortedProperty(v_oListView, False)

            ' Add the items
            For lLoop1 As Integer = 1 To v_oListView.Items.Count

                If v_iSourceColumn = 0 Then
                    sValue = StringsHelper.Format(v_oListView.Items.Item(lLoop1 - 1).Text, "#,##0.00")
                Else

                    'PSL 05/08/2003 Issue 5830
                    'Changed various bits, so negative numbers, and various currency formats work
                    Dim dbNumericTemp5 As Double
                    If ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1 Then
                        Dim dbNumericTemp3 As Double
                        Dim dbNumericTemp As Double
                        If Double.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Substring(0, ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                            sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Substring(0, ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1)
                        ElseIf Double.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Substring(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Length - (ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Length - (ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1))), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                            sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Substring(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Length - (ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Length - (ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1)))
                            If ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.StartsWith("-") Then
                                sValue = CStr(CDbl(sValue) * -1)
                            End If
                        Else
                            iLen = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Trim().Length

                            For iCount As Integer = iLen To 1 Step -1
                                sValue = Mid(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, iCount, 1)
                                Dim dbNumericTemp2 As Double
                                If Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                                    sValue = Mid(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, 1, iCount)
                                    Exit For
                                End If
                            Next
                        End If
                    ElseIf Not Double.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                        iLen = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Trim().Length

                        For iCount As Integer = iLen To 1 Step -1
                            sValue = Mid(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, iCount, 1)
                            Dim dbNumericTemp4 As Double
                            If Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                                sValue = Mid(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, 1, iCount)
                                Exit For
                            End If
                        Next

                    Else
                        sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text
                    End If
                    sValue.TrimEnd()

                    If sValue.StartsWith("-") Then
                        sValue = Mid(sValue, 2, sValue.Length - 1)
                        bNegative = True
                    Else
                        bNegative = False
                    End If
                    If sValue.Substring(0, 1) < "0" Or sValue.Substring(0, 1) > "9" Then
                        sValue = sValue.Substring(sValue.Length - (sValue.Length - 1))
                    End If
                    If bNegative Then
                        cValue = 1000000000 - CDec(sValue)
                    Else
                        cValue = CDec(sValue) + 1000000000
                    End If
                    sValue = StringsHelper.Format(cValue, "0000000000.00")

                End If
                ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), iIndex).Text = sValue

            Next lLoop1

            ' Sort now
            ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)

            ' Set the sort key
            ListViewHelper.SetSortKeyProperty(v_oListView, iIndex)

            ListViewHelper.SetSortedProperty(v_oListView, True)

            ' Remove the column now
            v_oListView.Columns.RemoveAt(iIndex)

            ' Reset the sort key
            'eck 010800
            '    v_oListView.SortKey = v_iSourceColumn%
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByStringVal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByStringVal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
