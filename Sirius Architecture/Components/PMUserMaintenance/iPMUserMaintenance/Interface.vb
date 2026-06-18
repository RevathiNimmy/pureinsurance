Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
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
    ' Date: {TodaysDate}
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' DAK231199 - Check PMProductLookup table to see if we can continue
    ' DAK011299 - Changes to privileges
    ' CJB090205 - PN18636 Change Initialise to call new function LockUserMaintenance
    '             Also call new function UnLockUserMaintenance from Terminate method.
    ' ***************************************************************** '

    Public m_ofrmUserMaintenance As frmUserMaintenance

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    ' SourceArray
    Private m_vSourceArray As Object

    ' RDC 01102002
    Private m_sSysOption As String = ""

    Private m_bUserMaintenanceLocked As Boolean
    Private Const ACUserMaintenanceLockName As String = "User Maintenance"
    Private Const ACUserMaintenanceLockId As Integer = 1

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    'PN23693
    Private m_sAD_OU_Path As String = ""
    Private m_sAD_OU_Domain As String = ""
    Private m_bLDAPQueryProvided As Boolean

    ' User ID
    Private m_iUserID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Source ID
    Private m_iSourceID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    Private m_iHomeCountryID As Integer
    Private m_iLicenceLimit As Integer
    Private m_iPoolSize As Integer
    Private m_iModifiedUserId As Integer
    ' PRIVATE Data Members (End)

    ' ***************************************************************** '
    ' Name: GetAD_OU_Path
    ' PN23693
    ' Description: Gets the AD_OU_Path for unified login on Sirius21 systems
    '
    ' ***************************************************************** '
    Private Function GetAD_OU_Path(ByRef sAD_OU_Path As String, ByRef sAD_OU_Domain As String) As Integer
        Dim result As Integer = 0
        Dim oPMSystem As bPMSystem.Business
        Dim lErrorValue As Integer
        Dim iSystemID, iProductID As Integer
        Dim sSystemName, sLicenceKey As String
        Dim vTimestamp As Object



        sAD_OU_Path = ""
        sAD_OU_Domain = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the Business object
        Dim temp_oPMSystem As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oPMSystem, "bPMSystem.Business", vInstanceManager:="ClientManager")
        oPMSystem = temp_oPMSystem
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            oPMSystem.Dispose()
            oPMSystem = Nothing

            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bPMSystem.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAD_OU_Path")
            Return result
        End If



        ' Call the initialise method.

        lErrorValue = oPMSystem.Initialise(sUserName:="", sPassword:="", iUserID:=m_iUserID, iLanguageID:=m_iLanguageID, iSourceID:=m_iSourceID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

        ' Check for errors.
        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get system details from the system object.

        lErrorValue = oPMSystem.GetValidSystem(sProductCode:=gPMConstants.PMProduct, iSystemID:=iSystemID, iProductID:=iProductID, sSystemName:=sSystemName, iDefaultSourceID:=m_iSourceID, iHomeCountryID:=m_iHomeCountryID, iCurrencyID:=m_iCurrencyID, iLanguageID:=m_iLanguageID, iLicenceLimit:=m_iLicenceLimit, sLicenceKey:=sLicenceKey, iLogLevel:=m_iLogLevel, iPoolSize:=m_iPoolSize, vTimestamp:=vTimestamp, sAD_OU_Path:=sAD_OU_Path, sAD_OU_Domain:=sAD_OU_Domain)

        ' Check for errors.
        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then

            ' System Record is not OK, we must return the error value.
            result = gPMConstants.PMEReturnCode.PMInvalidLicenceKey

            ' Set AD_OU_Path to empty
            m_sAD_OU_Path = ""
            m_sAD_OU_Domain = ""

            ' Release PMSystem

            oPMSystem.Dispose()
            oPMSystem = Nothing

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Valid System Record for " & gPMConstants.PMProduct & "/" & gPMConstants.PMCustomer, vApp:=ACApp, vClass:=ACClass, vMethod:="GetAD_OU_Path")

            Return result

        End If



        ' Call the terminate method.

        oPMSystem.Dispose()
        ' Release Instance
        oPMSystem = Nothing

        Return result

    End Function


    ' PUBLIC Property Procedures (Begin)

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            ' Standard Property.

            ' Return the task.
            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            ' Standard Property.

            ' Return the navigate flag.
            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            ' Standard Property.

            ' Return the process mode.
            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            ' Standard Property.

            ' Return the type of business.
            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            ' Standard Property.

            ' Return the effective date.
            Return m_dtEffectiveDate

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Set the object manager to nothing.
                g_oObjectManager = Nothing
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
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bPMUser.Business", vInstanceManager:="ClientManager")
            g_oBusiness = temp_g_oBusiness
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse
                'Tracy Richards - 07/08/03 - Changed the wording of the message
                MessageBox.Show("The was a problem creating bPMUser.Business", "iPMUserMaintenance.Interface", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oAuthority As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oAuthority, "bACTUserAuthorities.Business", vInstanceManager:="ClientManager")
            g_oAuthority = temp_g_oAuthority
            m_iModifiedUserId = g_oAuthority.UserID
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse
                'Tracy Richards - 07/08/03 - Changed the wording of the message
                MessageBox.Show("The was a problem creating bACTUserAuthorities.Business", "iPMUserMaintenance.Interface", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            'DAK231199
            ' RDC 16102002
            'Check if we can continue
            m_lReturn = CType(GetSysAdminStatus(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Check that there is not already someone using this function  PN18636
            m_lReturn = CType(LockUserMaintenance(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'DAK190400

            m_lReturn = g_oBusiness.GetAllSources(m_vSourceArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' RDC 01102002 get option that

            m_lReturn = g_oBusiness.GetSystemOption(1, 1, m_sSysOption)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

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
            If disposing Then
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()
                    g_oBusiness = Nothing
                End If
                If g_oAuthority IsNot Nothing Then
                    g_oAuthority.Dispose()
                    g_oAuthority = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetPrivileges
    '
    ' Description:
    '
    ' History: 23/11/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetPrivileges() As Integer
        Dim result As Integer = 0
        Dim oBusiness As bPMUser.Business
        Dim iPrivilegeLevel As gPMConstants.PMELookupEditPrivlegeLevel
        Dim bIsAdministrator As Boolean
        Dim vSupervisedGroups As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the Business object
            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMUser.Business", vInstanceManager:="ClientManager")
            oBusiness = temp_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oBusiness.Dispose()
                oBusiness = Nothing

                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivileges")

                Return result
            End If

            m_lReturn = oBusiness.GetPrivilegeLevel(r_iPrivilegeLevel:=iPrivilegeLevel)
            'DAK011299
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or (iPrivilegeLevel <> gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges And iPrivilegeLevel <> gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserCaptions And iPrivilegeLevel <> gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserView And iPrivilegeLevel <> gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserNone) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                oBusiness.Dispose()
                oBusiness = Nothing

                MessageBox.Show("You do not have permission to access " & _
                                "PM User Maintenance." & _
                                Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & _
                                "Please contact your System Administrator.", Application.ProductName)

                Return result
            End If

            If iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges Then

                oBusiness.Dispose()
                oBusiness = Nothing
                Return result
            End If


            m_lReturn = oBusiness.GetUserAuthority(r_bIsAdministrator:=bIsAdministrator, r_vSupervisedGroups:=vSupervisedGroups)

            oBusiness.Dispose()
            oBusiness = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Not bIsAdministrator Then
                result = gPMConstants.PMEReturnCode.PMFalse

                MessageBox.Show("You do not have permission to access " & _
                                "PM User Maintenance." & _
                                Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & _
                                "Please contact your System Administrator.", Application.ProductName)

                Return result
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPrivileges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivileges", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSysAdminStatus
    '
    ' Description: check if user is member of a SysAdmin user group.
    '
    ' History: RDC 16102002 created
    ' ***************************************************************** '
    Private Function GetSysAdminStatus() As Integer

        Dim result As Integer = 0
        Dim lStatus As Integer



        result = gPMConstants.PMEReturnCode.PMFalse


        m_lReturn = g_oBusiness.GetSysAdminStatus(lStatus)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or lStatus = 0 Then

            MessageBox.Show("You do not have permission to access " & _
                            "User Maintenance." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                            "Please contact your System Administrator.", Application.ProductName)

            Return result
        End If


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

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

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    'Case PMKeyNameInsReference
                    '    m_sClientKey$ = CStr(vKeyArray(PMKeyValue, lRow&))

                End Select

                ' {* USER DEFINED CODE (End) *}
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
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.
            ' {* USER DEFINED CODE (End) *}

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

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vSummaryArray(1, 0)

            ' Assign the key array with the parameter members.

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "Dummy Key"

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = "Dummy Value"

            ' {* USER DEFINED CODE (End) *}

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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

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

        'PN23693
        m_lReturn = CType(GetAD_OU_Path(m_sAD_OU_Path, m_sAD_OU_Domain), gPMConstants.PMEReturnCode)
        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Set a flag to show we are requiring LDAP domain users
        If m_sAD_OU_Path.Trim() <> "" And m_sAD_OU_Domain.Trim() <> "" Then
            m_bLDAPQueryProvided = gPMConstants.PMEReturnCode.PMTrue
        Else
            m_bLDAPQueryProvided = gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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
    ' PN23684 add AD_OU_Path and AD_OU_Domain
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the parameters to the interface properties.


        ' Guide No 50
        m_ofrmUserMaintenance = New frmUserMaintenance()

        With m_ofrmUserMaintenance
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate



            .SourceArray = m_vSourceArray
            .SysOption = m_sSysOption
            .Task = m_iTask
            .AD_OU_Path = m_sAD_OU_Path
            .AD_OU_Domain = m_sAD_OU_Domain
            .ModifiedUserId = m_iModifiedUserId
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Load the instance of the interface into memory.

        Dim tempLoadForm As frmUserMaintenance = m_ofrmUserMaintenance

        ' Check if we have had an error so far.

        'If frmUserMaintenance.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
        If m_ofrmUserMaintenance.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.

            result = m_ofrmUserMaintenance.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.

        With m_ofrmUserMaintenance
            m_lStatus = .Status

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.

        If Not (g_oObjectManager Is Nothing) Then
            ' Unlock the 'User Maintenance' function to allow others in   PN18636
            m_lReturn = CType(UnlockUserMaintenance(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Unlock User Maintenance", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockUserMaintenance")
            End If
        End If


        m_ofrmUserMaintenance.Close()

        m_ofrmUserMaintenance = Nothing

        Return result

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

        VB6.ShowForm(m_ofrmUserMaintenance, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.

            If m_ofrmUserMaintenance.ErrorNumber <> 0 Then

                result = m_ofrmUserMaintenance.ErrorNumber
            End If
        End If

        Return result

    End Function
    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
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


    ' ***************************************************************** '
    ' Name: LockUserMaintenance
    '
    ' Description: Lock the User Maintenance function (only one user is
    '              allowed at a time).
    '
    ' History: CJB 090205 Created as part of PN18636
    ' ***************************************************************** '
    Private Function LockUserMaintenance() As Integer
        Dim result As Integer = 0
        Dim bPMLock As Object


        Dim oLock As bPMLock.User
        Dim sLockedBy As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get instance of bPMLock.User
        Dim temp_oLock As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oLock = temp_oLock

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Failed to get instance of bPMLock.User", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Lock this function (just pass '1' as the id)

        m_lReturn = oLock.LockKey(sKeyName:=ACUserMaintenanceLockName, vKeyValue:=ACUserMaintenanceLockId, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)

        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

            If sLockedBy = "ERROR" Then
                gPMFunctions.RaiseError("oLock.LockKey", "Error trying to lock 'User Maintenance' record", gPMConstants.PMELogLevel.PMLogError)
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("User Maintenance is currently locked by " & sLockedBy & "." & _
                                Strings.Chr(13) & Strings.Chr(10) & "Please try later.", "User Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return result
            End If

        End If

        ' Flag that we have successfully locked the function (so that we know to unlock later)
        m_bUserMaintenanceLocked = True

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UnlockUserMaintenance
    '
    ' Description: Unlock the User Maintenance function.
    '
    ' History: CJB 090205 Created as part of PN18636
    ' ***************************************************************** '
    Private Function UnlockUserMaintenance() As Integer
        Dim result As Integer = 0
        Dim bPMLock As Object


        Dim oLock As bPMLock.User



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get instance of bPMLock.User
        Dim temp_oLock As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oLock = temp_oLock

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Failed to get instance of bPMLock.User", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Unlock this function (if we successfully locked it in the first place!)
        If m_bUserMaintenanceLocked Then

            m_lReturn = oLock.UnLockKey(sKeyName:=ACUserMaintenanceLockName, vKeyValue:=ACUserMaintenanceLockId, iUserID:=g_oObjectManager.UserID)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                gPMFunctions.RaiseError("oLock.UnLockKey", "Error trying to unlock 'User Maintenance'", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_bUserMaintenanceLocked = False
        End If


        Return result
    End Function
End Class