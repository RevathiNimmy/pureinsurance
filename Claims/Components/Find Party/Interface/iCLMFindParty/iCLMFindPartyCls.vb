Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 10/08/2000
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' object for the party screens
    Private oparties As Object
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"
    Dim frmInterface As frmInterface

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As String = ""
    Private m_lStatus As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_iNotEditable As Integer
    Private m_vSourceArray As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Member to hold instance of form interface
    'Private m_ofrmInterface As iCLMFindParty.frmInterface

    ' Do we want to show frmParties only?
    Private m_bPartyTypeOnly As Boolean

    ' Party type to return
    Private m_sPartyType As String = ""

    Private m_bDeleteMode As Boolean

    'sj 3/11/99 - start
    Private m_lInvariantKey As Integer

    'DC120701
    'Variable for Underwriting/Broking
    Private m_sSiriusUnderWritingBroking As String = ""


    Public Property InvariantKey() As Integer
        Get
            Return m_lInvariantKey
        End Get
        Set(ByVal Value As Integer)
            m_lInvariantKey = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            m_sCallingAppName = Value

        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = CStr(Value)

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property PartyClaimID() As Integer
        Get

            Return m_lPartyClaimID

        End Get
        Set(ByVal Value As Integer)

            m_lPartyClaimID = Value

        End Set
    End Property

    Public Property Name() As String
        Get

            Return m_sName

        End Get
        Set(ByVal Value As String)

            m_sName = Value

        End Set
    End Property



    Public Property NotEditable() As Integer
        Get

            Return m_iNotEditable

        End Get
        Set(ByVal Value As Integer)

            m_iNotEditable = Value

        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer




        Dim result As Integer = 0
        Dim sMessage, sTitle, sHelpFile As String
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bCLMFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            'sj 3/11/99 - start
            g_bGenericConnectionStatus = g_oObjectManager.GenericConnectionStatus
            'sj 3/11/99 - end

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = PMProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Find out from the registry where the Help File is
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'App.HelpFile = sHelpFile
            End If

            ' Default to showing the normal interface
            m_bPartyTypeOnly = False

            'DC120701 get Underwriting or Broking
            Dim temp_g_oBOBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBOBusiness, "bBackOfficeLink.bBOLink", vInstanceManager:="ClientManager")
            g_oBOBusiness = temp_g_oBOBusiness


            m_sSiriusUnderWritingBroking = g_oBOBusiness.Sirius_Product
            'DC120701

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()

                End If
                g_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMKeyNameOperateMode

                        m_iTask = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMKeyNamePartyClaimID

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then
                            'DC290601 was CInt now CLng

                            m_lPartyClaimID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameClaimPartyTypeID

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then
                            'UNCOMMENT FOR INTEGRATION******************************************************
                            'DC290601 was CInt now CLng

                            m_lClaimPartyTypeID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                            'UNCOMMENT FOR INTEGRATION******************************************************
                        Else
                            m_lClaimPartyTypeID = 0
                        End If
                    Case PMKeyNameClaimCnt
                        'UNCOMMENT FOR INTEGRATION******************************************************
                        'DC290601 was CInt now CLng

                        m_lClaimID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'fUNCOMMENT FOR INTEGRATION******************************************************
                    Case PMKeyNamePartyName

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_sPartyName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameAddress

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_sPartyAddress = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                        'DC120701 more than one line of address for Broking
                    Case PMKeyNameAddress1

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_sPartyAddress1 = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameAddress2

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_sPartyAddress2 = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameAddress3

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_sPartyAddress3 = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameAddress4

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_sPartyAddress4 = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNamePostcode

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_sPartyPostcode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                        'DC120701 -end

                    Case PMKeyNamePhone_Number

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_sPartyPhoneNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameLicense_type

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_vLicensetype = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameLicensetypedescription

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_vLicenseDescription = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameLicense_Number

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_vLicenseNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameDate_of_Birth

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_vDOB = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameSex

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_vSex = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameparty_status

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_vPartyStatus = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNamedriver_status_description

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_vDriverStatusDescription = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNamePhone_Number

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_vPhoneNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameFax_Number

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_vFaxNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameReference_Number

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_vReferenceNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                    Case PMKeyNameReg_Number

                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) <> "" Then

                            m_vRegNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        End If
                End Select

            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object, Optional ByRef iMode As Integer = 0) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC120701 increase array for Broking
            ReDim vKeyArray(1, 16)


            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMKeyNameOperateMode

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = iMode ' m_iTask


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMKeyNamePartyClaimID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lPartyClaimID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMKeyNameClaimPartyTypeID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lClaimPartyTypeID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMKeyNameClaimCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lClaimID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMKeyNamePartyName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_sPartyName


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMKeyNameAddress

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_sPartyAddress

            'DC120701 more than one line of address for Broking

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMKeyNameLicense_type

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = m_vLicensetype


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 7) = PMKeyNameLicensetypedescription

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 7) = m_vLicenseDescription


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 8) = PMKeyNameLicense_Number

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 8) = m_vLicenseNumber


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 9) = PMKeyNameDate_of_Birth

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 9) = m_vDOB


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 10) = PMKeyNameSex

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 10) = m_vSex


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 11) = PMKeyNameparty_status

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 11) = m_vPartyStatus


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 12) = PMKeyNamedriver_status_description

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 12) = m_vDriverStatusDescription


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 13) = PMKeyNamePhone_Number

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 13) = m_vPhoneNumber


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 14) = PMKeyNameFax_Number

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 14) = m_vFaxNumber


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 15) = PMKeyNameReference_Number

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 15) = m_vReferenceNumber


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 16) = PMKeyNameReg_Number

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 16) = m_vRegNumber

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            'SP011298 - Changes to support roadmap (if nowt to do return vSummarryArray="")
            ReDim vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0)

            ' Assign the key array with the parameter members.
            ' TF180199 - Set header to appropriate party type

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = m_sClaimPartyType
            'vSummaryArray(PMNavSummValue, 0) = m_sLongName$


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            ' Set the process modes for the business object.
            If Not (g_oBusiness Is Nothing) Then

                m_lReturn = g_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessParties
    '
    ' Description: Shows the parties form and gets the party type
    ' ***************************************************************** '
    Private Function ProcessParties() As Integer

        'Dim oParties As frmParties

        Dim result As Integer = 0



        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CF261099
            If m_bPartyTypeOnly Then
                m_lReturn = CType(ProcessParties(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                Return result
            End If

            ' Check if the PartyClaimID is greater than zero. If so,
            ' there is no need to proceed with the interface. We
            ' can therefore return straight back out.
            'If (m_lPartyClaimID& > 0) Then
            ' ID is greater than zero.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'Else

            ' ID is not greater than zero.

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            'End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Set m_ofrmInterface = New iCLMFindParty.frmInterface


        ' Assign the parameters to the interface properties.
        'With m_ofrmInterface
        With frmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .oFindParty = Me
            .NotEditable = m_iNotEditable
            .DeleteMode = m_bDeleteMode
            'eck090500


            '.set_SourceArray(m_vSourceArray)
            .SourceArray = m_vSourceArray
            .SiriusUnderwritingBroking = m_sSiriusUnderWritingBroking
        End With

        ' Load the instance of the interface into memory.
        'Load m_ofrmInterface
        Dim tempLoadForm As frmInterface = frmInterface

        ' Check if we have had an error so far.
        'If (m_ofrmInterface.ErrorNumber = PMFalse) Then
        If frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            '        LoadInterface = m_ofrmInterface.ErrorNumber
            result = frmInterface.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Public Function UnLoadInterface() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the property members from the interface parameters.
            'With m_ofrmInterface
            With frmInterface
                m_lStatus = .Status

                m_lPartyClaimID = .PartyClaimID

            End With

            ' Unload and destroy the instance of the interface
            ' from memory.
            '    Unload m_ofrmInterface
            '    Set m_ofrmInterface = Nothing

            frmInterface.Close()
            frmInterface = Nothing
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unload the interface from memory", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLoadInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        'm_ofrmInterface.Show lDisplayState&
        VB6.ShowForm(frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            'If (m_ofrmInterface.ErrorNumber <> 0) Then
            If frmInterface.ErrorNumber <> 0 Then
                'ShowInterface = m_ofrmInterface.ErrorNumber
                result = frmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

