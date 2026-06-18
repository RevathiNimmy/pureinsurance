Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Drawing.Printing
Imports SSP.Shared



<Serializable()>
<System.Runtime.InteropServices.ProgId("ClientManager_NET.ClientManager")>
Public NotInheritable Class ClientManager
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: ClientManager
    '
    ' Date: 05 July 1996
    '
    ' Description: Main Class.
    '
    ' Edit History:
    ' RFC 17061998 - Server Printer details got in Logon method
    ' RFC 17061998   and returned in the GetPropertyDetails method.
    ' RFC 17061998 - UpdateUser method amended to include ServerPrinter.
    ' RFC 23061998 - PMPropertyManager created/Terminated by Logon/Logoff
    ' RFC 23061998 - All user locks released at Logon/Logoff.
    ' RFC 19081998 - Return the name of the Server on which Client Manager
    '                is running. This can then be displayed on
    '                Logon Status Manager
    ' DAK200100 - Get the process id
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "ClientManager"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' Broking Logon Flag
    Private m_bLoggedOnToPMB As Boolean

    Private m_oPMUnixCache As Object

    Private m_lReturn As gPMConstants.PMEReturnCode
    'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.3.1.1)
    Private m_sUserConfigXMLDataset As String = ""
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.3.1.1)

    ' RFC 23061998
    ' Property Manager Reference
    ' RDC 17062002 new XML version of Property Manager

    Private m_oPMPropMan As bPMPropertyManager.BusinessXML

    ' PMDAO Instances Collection
    Private m_oPMDAOInstances As BCLIENTMANAGER.PMDAOInstances

    ' RDC 06092002 checking user is group supervisor
    Private Const ACGetSystemOptionStored As Boolean = True
    Private Const ACGetSystemOptionName As String = "GetSystemOption"


    Private Const ACGetSystemOptionSQL As String = "spu_get_system_option"

    Private Const ACGetpasswordhistoryStored As Boolean = True
    Private Const ACGetpasswordhistoryName As String = "Getpasswordhistory"
    Private Const ACGetpasswordhistorySQL As String = "spu_sir_passwordhistory_sel"
    Private m_sPasswordChangeDate As String = ""
    Private m_bIsTempPassword As Boolean = False
    'end of constants


    ' PRIVATE Data Members (End)
    ' PUBLIC Property Procedures (Begin)
    Public Property LanguageID() As Integer
        Get

            ' Return the language ID.
            Return g_iLanguageID

        End Get
        Set(ByVal Value As Integer)

            ' Set the language ID.
            g_iLanguageID = LanguageID

        End Set
    End Property

    ' RDC 13082002

    Public Property SourceID() As Integer
        Get
            ' Return the Source ID.
            Return g_iSourceID
        End Get
        Set(ByVal Value As Integer)
            g_iSourceID = Value
        End Set
    End Property

    Public Property UserName() As String
        Get

            ' Return the UserName.
            Return g_sUsername

        End Get
        Set(ByVal value As String)
            g_sUsername = value
        End Set
    End Property

    Public Property Password() As String
        Get

            ' Return the Password
            Return g_sPassword

        End Get
        Set(ByVal value As String)
            g_sPassword = value
        End Set
    End Property

    Public Property LogLevel() As Integer
        Get

            Return g_iLogLevel

        End Get
        Set(ByVal value As Integer)
            g_iLogLevel = value
        End Set
    End Property

    Public Property CurrencyID() As Integer
        Get

            Return g_iCurrencyID

        End Get
        Set(ByVal value As Integer)
            g_iCurrencyID = value
        End Set
    End Property

    ' RDC 29112002

    Public Property CountryID() As Integer
        Get

            Return g_iCountryID

        End Get
        Set(ByVal Value As Integer)
            g_iCountryID = Value
        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get

            Return g_lPartyCnt

        End Get
        Set(ByVal Value As Integer)
            g_lPartyCnt = Value
        End Set
    End Property

    Public Property UserID() As Integer
        Get

            Return g_iUserID

        End Get
        Set(ByVal Value As Integer)
            g_iUserID = Value
        End Set
    End Property

    ' RFC 19081998
    ' Return the name of the Server on which Client Manager is running.
    ' This can then be displayed on Logon Status Manager
    Public ReadOnly Property ServerName() As String
        Get
            Dim sServerName As String = ""

            Dim lReturn As Integer = gPMFunctions.GetSystemName(sServerName)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sServerName = ""
            End If
            Return sServerName

        End Get
    End Property
    ' PUBLIC Property Procedures (End)

    Public Property UserConfigXMLDataSet() As String
        Get
            Return m_sUserConfigXMLDataset
        End Get
        Set(ByVal Value As String)
            m_sUserConfigXMLDataset = Value
        End Set
    End Property
    ''' <summary>
    ''' To get and set Password change date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PasswordChangeDate() As String
        Get
            Return m_sPasswordChangeDate
        End Get
        Set(ByVal Value As String)
            m_sPasswordChangeDate = Value
        End Set
    End Property

    ''' <summary>
    ''' To get and set Temporary password
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TempPassword() As Boolean
        Get
            Return m_bIsTempPassword
        End Get
        Set(ByVal Value As Boolean)
            m_bIsTempPassword = Value
        End Set
    End Property
    'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.3.1.1)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef iSourceID As Integer, ByRef iCountryID As Integer, ByRef iLanguageId As Integer, ByRef iLogLevel As Integer, ByRef iCurrencyID As Integer, ByRef sCallingAppName As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Save the Initialisation Values
            g_iSourceID = iSourceID
            g_iCountryID = iCountryID
            g_iLanguageID = iLanguageId
            g_iLogLevel = iLogLevel
            g_iCurrencyID = iCurrencyID
            g_sCallingAppName = sCallingAppName
            ' Create new PMDAO Instances Collection if needed
            If m_oPMDAOInstances Is Nothing Then
                m_oPMDAOInstances = New BCLIENTMANAGER.PMDAOInstances()
            End If

            ' Initialise PMBroking Flags
            m_bLoggedOnToPMB = False

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPropertyValues
    '
    ' Description: Returns the System Proprty Values for this
    '              Client Manager.
    '
    '              (This method is implemented for performance reasons,
    '              as it is quicker to call one method than it is to
    '              access eight property gets.
    '
    ' ***************************************************************** '
    Public Function GetPropertyValues(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iCountryID As Integer, ByRef iLanguageId As Integer, ByRef iLogLevel As Integer, ByRef iCurrencyID As Integer, ByRef lPartyCnt As Integer, ByRef sCallingAppName As String, Optional ByRef sServerPrinter As String = "", Optional ByRef iIsPrinterChangeable As Integer = 0, Optional ByRef sUserConfigXMLDataSet As String = "", Optional ByRef sTempPassword As String = "", Optional ByRef oPasswordChangeDate As Object = Nothing) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Return Username and Password
            sUsername = g_sUsername
            sPassword = g_sPassword

            ' Return the User ID
            iUserID = g_iUserID

            ' Return the Client Manager System Values
            iSourceID = g_iSourceID
            iCountryID = g_iCountryID
            iLanguageId = g_iLanguageID
            iLogLevel = g_iLogLevel
            iCurrencyID = g_iCurrencyID
            lPartyCnt = g_lPartyCnt
            sCallingAppName = g_sCallingAppName
            sServerPrinter = g_sServerPrinter
            iIsPrinterChangeable = g_iIsPrinterChangeable
            sUserConfigXMLDataSet = m_sUserConfigXMLDataset
            sTempPassword = m_bIsTempPassword
            oPasswordChangeDate = m_sPasswordChangeDate

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Return the SystemValues", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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
                Logoff()
                m_oPMDAOInstances = Nothing
                m_oPMUnixCache = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: AddMessage
    '
    ' Description: Wrapper method to the LogMessage function.
    '
    ' ***************************************************************** '
    Public Sub AddMessage(ByVal iType As Integer, ByVal sMsg As String, Optional ByVal vApp As Object = Nothing, Optional ByVal vClass As Object = Nothing, Optional ByVal vMethod As Object = Nothing, Optional ByVal vErrNo As Object = Nothing, Optional ByVal vErrDesc As Object = Nothing)

        Try

            bPMFunc.LogMessage(iType:=iType, sMsg:=sMsg, vApp:=vApp, vClass:=vClass, vMethod:=vMethod, vErrNo:=vErrNo, vErrDesc:=vErrDesc)

        Catch excep As Exception



            ' Error Section.

            ' Failed to log message, so we must
            ' call the function to popup the
            ' message instead.





            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=iType, sMsg:=sMsg, vApp:=vApp, vClass:=vClass, vMethod:=vMethod, vErrNo:=vErrNo, vErrDesc:=vErrDesc, excep:=excep)

            Exit Sub
        End Try


    End Sub

    ' ***************************************************************** '
    ' Name: GetInstance
    '
    ' Description: Creates an instance of the class name passed.
    '
    ' ***************************************************************** '
    Public Function GetInstance(ByRef oObject As Object, ByRef sClassName As String, ByRef sCallingAppName As String) As Integer

        Dim result As Integer = 0
        Dim lErrorValue As Integer
        Dim ePMProductFamily As gPMConstants.PMEProductFamily
        Dim sDSN As String = ""
        Dim oDatabase As dPMDAO.Database


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create an instance of the object.
        ' NOTE: Because of passing in the class name,
        ' the object can ONLY be created late bound.

        If sClassName.ToUpper.Contains("BPMUSERGROUP.LOOKUP") And oObject Is Nothing Then
            oObject = New bPMUserGroup.Lookup
        ElseIf oObject Is Nothing Then
            oObject = bPMFunc.CreateLateBoundObject(sClassName)
        End If

        ' Get the product family so we can determine which
        ' database to pass in
        ' Note: Not ALL business object will support this property yet
        ' so we must use Resume Next to trap this situation
        ePMProductFamily = -1
        Try


            'ePMProductFamily = ReflectionHelper.GetMember(oObject, "PMProductFamily")
            ePMProductFamily = oObject.PMProductFamily

        Catch
        End Try



        ' Did the Object support the Property
        If ePMProductFamily = -1 Then
            ' No, so just get a default PMDAO
            oDatabase = m_oPMDAOInstances.GetPMDAOInstance()
        Else
            oDatabase = m_oPMDAOInstances.GetPMDAOInstance(ePMProductFamily)
        End If

        ' Check whether the existing instance of dPMDAO has an open transaction
        ' we need to log this for tracking purposes. It will be either because we are
        ' instantiating an object within a transaction or another process has failed
        ' leaving the transaction open

        If oDatabase.TransactionNestLevel > 0 Then
            ' Log Error.
            bPMFunc.LogMessage(g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OBJECT INSTANTIATED WITH TRANSACTION OPEN" & Strings.ChrW(13) & Strings.ChrW(10) & "Calling App Name: " & sCallingAppName & Strings.ChrW(13) & Strings.ChrW(10) & "Object requested: " & sClassName & Strings.ChrW(13) & Strings.ChrW(10) & "dPMDAO Transaction Level: " & CStr(oDatabase.TransactionNestLevel) & Strings.ChrW(13) & Strings.ChrW(10) & "Product Family: " & CStr(ePMProductFamily), vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstance", vErrNo:=0, vErrDesc:="Warning Message")

        End If

        ' Initialise the object.

        ' Standard Initialise

        lErrorValue = StandardInitialise(oObject, oDatabase, sCallingAppName)

        ' Check for errors.
        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        oDatabase = Nothing

        Return result

Err_GetInstance:

        ' Error Section.
        result = gPMConstants.PMEReturnCode.PMError

        ' Set the object to nothing
        oObject = Nothing

        ' Log Error.
        bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object instance (" & sClassName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstance", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Logon
    '
    ' Description: Logs the user specified on.
    '
    ' ***************************************************************** '

    Public Function Logon(ByVal sUsername As String, ByVal sPassword As String, ByRef sLoggedOnAtClient As String, ByRef bPMBLinkRequired As Boolean) As Integer

        Dim result As Integer = 0
        Dim oUser As Bpmuser.Business
        Dim lErrorValue As gPMConstants.PMEReturnCode
        Dim lCheckLogonResult As Integer
        Dim iLanguageId As Integer
        Dim lPartyCnt As Integer
        Dim vUserID As Object = Nothing
        Dim iIsPMBLinkRequired As Integer
        Dim oDatabase As dPMDAO.Database
        Dim sServerPrinter As String = ""
        Dim iIsPrinterChangeable As Integer
        'DAK200100
        Dim lAppId As Integer
        Dim sNow As String = ""
        ' CTAF 20030718
        Dim sObjectName As String = ""

        Dim sUserConfigXMLDataSet As String = ""
        Dim sPasswordChangeDate As String = ""
        Dim sIsTempPassword As String = ""

        Try

            ' Get a Sirius Architecture PMDAo
            oDatabase = m_oPMDAOInstances.GetPMDAOInstance(gPMConstants.PMEProductFamily.pmePFSiriusArchitecture)

            ' CTAF 20030718 - Set the object we' about to create for error reporting
            sObjectName = "bPMUser.Business"

            ' Get an instance of the user business object.
            oUser = New Bpmuser.Business()

            lErrorValue = oUser.Initialise("", "", 1, g_iSourceID, g_iLanguageID, g_iCurrencyID, g_iLogLevel, ACApp, vDatabase:=oDatabase)

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                oUser = Nothing
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check if the username and password are valid,
            ' and return the result back.
            ' AMB 24-Nov-03: 1.8.6 Unified login - username and password are now explicitly ByRef
            ' and so are passed back from CheckLogon, cos the username (at least) can change
            sUsername = sUsername.Trim()
            sPassword = sPassword.Trim()

            lCheckLogonResult = oUser.CheckLogon(sUsername, sPassword, DateTime.Now, iLanguageId, lPartyCnt, vUserID, sLoggedOnAtClient, iIsPMBLinkRequired, sServerPrinter, iIsPrinterChangeable, sUserConfigXMLDataSet, sIsTempPassword, sPasswordChangeDate, False)


            ' If logged checked out ok
            If lCheckLogonResult = gPMConstants.PMEReturnCode.PMTrue OrElse lCheckLogonResult = gPMConstants.PMEReturnCode.PMUserTemporaryPassword OrElse lCheckLogonResult = gPMConstants.PMEReturnCode.PMUserPasswordExpired OrElse lCheckLogonResult = gPMConstants.PMEReturnCode.PMUserWeakPassword OrElse lCheckLogonResult = gPMConstants.PMEReturnCode.PMNewBuildUpgrade Then

                ' Store details
                g_sUsername = sUsername
                g_sPassword = sPassword

                ' User Language Overrides system default
                g_iLanguageID = iLanguageId
                LanguageID = g_iLanguageID
                ' Party Count for this User
                g_lPartyCnt = lPartyCnt
                ' User ID

                g_iUserID = CInt(vUserID)
                ' Server Printer properties
                g_sServerPrinter = sServerPrinter
                g_iIsPrinterChangeable = iIsPrinterChangeable

                m_sUserConfigXMLDataset = sUserConfigXMLDataSet
                m_bIsTempPassword = sIsTempPassword
                m_sPasswordChangeDate = sPasswordChangeDate
                'DAK200100
                ' Display the PID and User Name and logon day & time
                ' As the application title
                sNow = DateTime.Now.ToString("ddHHMM")
                lAppId = GetCurrentProcessId()

            End If

            ' Return the logon result.
            result = lCheckLogonResult

            ' Did the Logon Check OK?
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                oUser.Dispose()
                oUser = Nothing
                oDatabase = Nothing
                Return result
            End If

            ' RFC270398
            ' All OK, so Logon User
            lErrorValue = oUser.Logon(sUsername, sLoggedOnAtClient)

            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Initialise the user object.
            oUser.Dispose()

            ' Destroy the instance of the user object
            ' from memory.
            oUser = Nothing
            oDatabase = Nothing

            ' RDC 13082001 remove any 'hung' licences from last time this user logged on.
            lErrorValue = CType(ClearExistingLicences(sUsername), gPMConstants.PMEReturnCode)

            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ' do nothing
            End If

            ' CTAF 20030718 - Set the object we' about to create for error reporting
            sObjectName = "bPMPropertyManager.BusinessXML"

            ' RFC23061998
            ' Create and Initialise Property Manager
            m_oPMPropMan = New bPMPropertyManager.BusinessXML()


            lErrorValue = m_oPMPropMan.Initialise()

            'PN17766 - Delete all properties for this User

            lErrorValue = gPMComponentServices.DeleteAllUserProperties(sUsername)

            ' Release All User Locks

            lErrorValue = ReleaseAllUserLocks()

            ' RFC 06/04/1998
            ' Is a Link to Broking Required.
            If iIsPMBLinkRequired = gPMConstants.PMEReturnCode.PMTrue Then

                ' Yes it is required
                bPMBLinkRequired = True

            Else

                ' No it isn't required
                bPMBLinkRequired = False

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' CTAF 20030718 - Hopefully more helpful error message

            Select Case Informations.Err().Number
                Case 429

                    If sObjectName = "" Then
                        sObjectName = "(Unknown)"
                    End If

                    ' Log Error.
                    bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of " & sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="Logon", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to logon", vApp:=ACApp, vClass:=ACClass, vMethod:="Logon", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End Select

            Return result

        End Try
    End Function

    Public Function UpdateUser(Optional ByRef vUserID As Object = Nothing, Optional ByRef vLanguageID As Integer = 0,
                               Optional ByRef vUsername As Object = Nothing,
                               Optional ByRef vPassword As Object = Nothing,
                               Optional ByRef vPasswordChangeDate As Object = Nothing,
                               Optional ByRef vDateCreated As Object = Nothing,
                               Optional ByRef vLastLogin As Object = Nothing,
                               Optional ByRef vPartyCnt As Object = Nothing,
                               Optional ByRef vIsDeleted As Object = Nothing,
                               Optional ByRef vEffectiveDate As Object = Nothing,
                               Optional ByRef vServerPrinter As String = "",
                               Optional ByVal bSystemUpgradeTempPwd As Boolean = False,
                               Optional ByVal sOldPassword As String = Nothing,
                               Optional ByVal sPasswordChanged As String = "") As Integer


        Dim result As Integer = 0
        Dim lErrorValue As Integer
        Dim oPMUser As Bpmuser.Business
        Dim iUserID As Integer
        Dim lRowcount As Integer

        Dim sUsername As String = String.Empty
        Dim sPassword As String = String.Empty
        Dim iLanguageId As Integer
        Dim lPartyCnt As Integer
        Dim oDatabase As dPMDAO.Database
        Dim bIsTempPassword As Boolean
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Sirius Architecture PMDAo
            oDatabase = m_oPMDAOInstances.GetPMDAOInstance(gPMConstants.PMEProductFamily.pmePFSiriusArchitecture)

            ' Create an instance of the User object.
            oPMUser = New Bpmuser.Business()

            lErrorValue = oPMUser.Initialise("", "", g_iUserID, g_iSourceID, g_iLanguageID, g_iCurrencyID, g_iLogLevel, ACApp, vDatabase:=oDatabase)

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Informations.IsNothing(vUserID) And Informations.IsNothing(vUsername) Then
                'pmerror a userid or username must be supplied

                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="Failed to Update User.  Username or UserId must be supplied.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                oPMUser = Nothing
                oDatabase = Nothing
                Return result
            End If


            If Not Informations.IsNothing(vUsername) Then
                'if vUsername has been passed assign it to a string
                sUsername = CStr(vUsername)
            End If

            If Not Informations.IsNothing(vPassword) Then
                'if vpassword has been passed assign it to a string
                sPassword = CStr(vPassword)
                bIsTempPassword = False
            End If


            If Informations.IsNothing(vUserID) Then
                'get the user_id from the username by calling checklogon on bpmuser
                lErrorValue = oPMUser.CheckLogon(sUsername, g_sPassword, DateTime.Now, iLanguageId, lPartyCnt, iUserID)

                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                iUserID = CInt(vUserID)
            End If

            lErrorValue = oPMUser.GetDetails(iUserID)

            'set the collection index pointing to current row
            lErrorValue = oPMUser.GetNext(iUserID)

            lRowcount = 1
            lErrorValue = oPMUser.EditUpdate(lRow:=lRowcount, vUserId:=iUserID, vLanguageID:=vLanguageID,
                                             vUsername:=vUsername, vPassword:=vPassword,
                                             vPasswordChangeDate:=vPasswordChangeDate, vDateCreated:=vDateCreated,
                                             vLastLogin:=vLastLogin, vPartyCnt:=vPartyCnt, vIsDeleted:=vIsDeleted,
                                             vEffectiveDate:=vEffectiveDate, vServerPrinter:=vServerPrinter,
                                             vIsTempPassword:=bIsTempPassword,
                                             bSystemUpgradeTempPwd:=bSystemUpgradeTempPwd, sOldPassword:=sOldPassword,
                                             sPasswordChanged:=sPasswordChanged)


            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                oPMUser = Nothing
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lErrorValue = oPMUser.Update()

            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                oPMUser = Nothing
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            lErrorValue = oPMUser.UpdatePasswordHistory(iUser_Id:=iUserID)
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                oPMUser = Nothing
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Initialise the user object.
            oPMUser.Dispose()
            ' Destroy the instance of the User object.
            oPMUser = Nothing
            oDatabase = Nothing

            ' RFC23061998
            ' If the ServerPrinter was Updated, update our Local Copy

            If Not Informations.IsNothing(vServerPrinter) Then
                g_sServerPrinter = vServerPrinter
            End If
            ' If the Langauge was Updated, update our Local Copy

            If Not Informations.IsNothing(vLanguageID) Then
                g_iLanguageID = vLanguageID
            End If


            Return result

        Catch excep As System.Exception





            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update User", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUser", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Function GetDefaultPrinterName() As String
        Dim printerSettings As New PrinterSettings()
        Return printerSettings.PrinterName
    End Function

    ' ***************************************************************** '
    ' Name: GetAvailablePrinters
    '
    ' Description: Gets and returns the list of Printers, and the
    '              default printer.
    '
    ' ***************************************************************** '
    Public Function GetAvailablePrinters(ByRef r_vPrinterArray As Object, ByRef r_sDefaultPrinter As String) As Integer

        Dim result As Integer = 0
        ' Dim lNoOfPrinters, lSub As Integer
        Dim sPrinter As String()

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Initialise
        r_sDefaultPrinter = ""
        Dim defaultPrinter As String = GetDefaultPrinterName()

        Try
            ' Return the Default Printer Name

            r_sDefaultPrinter = GetDefaultPrinterName()
            ' Dim deviceName As String = If(PrinterHelper.Printer.DeviceName IsNot Nothing, PrinterHelper.Printer.DeviceName.Trim(), Nothing)

            sPrinter = PrinterSettings.InstalledPrinters.Cast(Of String)().ToArray()
            r_vPrinterArray = sPrinter

        Catch excep As System.Exception
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to logon to PM Broking", vApp:=ACApp, vClass:=ACClass, vMethod:="PMBLogon", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result = gPMConstants.PMEReturnCode.PMFalse
        End Try


        ' If there are no printers, exit
        If r_sDefaultPrinter = "" Then

            r_vPrinterArray = ""
            Return result
        End If

        ' Get the number of Printers
        '        lNoOfPrinters = r_vPrinterArray

        ' If there are none, exit
        'If lNoOfPrinters < 1 Then

        '    r_vPrinterArray = ""
        '    Return result
        'End If

        ' Size the Printer array accordingly
        'ReDim r_vPrinterArray(lNoOfPrinters - 1)

        '' Add the name of each printer to the Array
        'lSub = 0
        'For Each oPrinter As PrinterHelper In PrinterHelper.Printers

        '    'it's going for one extra loop which gives runtime error
        '    If lSub <= (lNoOfPrinters - 1) Then
        '        r_vPrinterArray(lSub) = PrinterHelper.Printer.DeviceName
        '    End If
        '    lSub += 1
        'Next oPrinter

        'Return result

        'Err_GetAvailablePrinters:

        '        ' Error Section.
        '        result = gPMConstants.PMEReturnCode.PMError

        '        ' Log Error.
        '        bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the List of Printers", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailablePrinters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: PMBLogon
    '
    ' Description: Logs on to PM Broking
    '
    ' ***************************************************************** '
    Public Function PMBLogon(ByRef r_lReturnValue As Integer, ByRef r_iNoOfCompanies As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturnCode As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set logged on to pmb flag
            m_bLoggedOnToPMB = False

            ' Get Unix Cache
            lReturnCode = CType(GetUnixCache(), gPMConstants.PMEReturnCode)

            If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Connect




            'lReturnCode = ReflectionHelper.Invoke(m_oPMUnixCache, "Connect", New Object() {g_sUsername, g_sPassword, r_lReturnValue})

            'If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then

            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            ' Logon





            ' lReturnCode = ReflectionHelper.Invoke(m_oPMUnixCache, "Logon", New Object() {g_sUsername, g_sPassword, r_iNoOfCompanies, r_lReturnValue})

            If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Logged on to PMB flag
            m_bLoggedOnToPMB = True

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to logon to PM Broking", vApp:=ACApp, vClass:=ACClass, vMethod:="PMBLogon", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PMB Get Valid Companies
    '
    ' Description: Gets the list of Valid PM Broking Companies for
    '              this user.
    ' ***************************************************************** '
    Public Function PMBGetValidCompanies(ByRef r_vValidCompanies As Object, ByRef r_vValidDescriptions As Object) As Integer

        Dim result As Integer = 0
        Dim lReturnCode As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get Unix Cache
            lReturnCode = CType(GetUnixCache(), gPMConstants.PMEReturnCode)

            If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If





            'lReturnCode = ReflectionHelper.Invoke(m_oPMUnixCache, "ValidCompanies", New Object() {g_sUsername, r_vValidCompanies, r_vValidDescriptions})

            'If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the valid companies", vApp:=ACApp, vClass:=ACClass, vMethod:="PMBGetValidCompanies", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PMBSetCompany
    '
    ' Description: Sets the PMB Company for this user
    '
    ' ***************************************************************** '
    Public Function PMBSetCompany(ByVal v_sCompany As String) As Integer

        Dim result As Integer = 0
        Dim lReturnCode As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get Unix Cache
            lReturnCode = CType(GetUnixCache(), gPMConstants.PMEReturnCode)

            If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the Company for this User



            'lReturnCode = ReflectionHelper.Invoke(m_oPMUnixCache, "SetCompany", New Object() {g_sUsername, v_sCompany})

            'If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to logon to PM Company", vApp:=ACApp, vClass:=ACClass, vMethod:="PMBSetCompany", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckClientDateFormat
    '
    ' Description: Checks to see if the Client Date Format and System
    '              Date is the same as the Server Date Format and
    '              System Date.
    ' ***************************************************************** '
    Public Function CheckClientDateFormat(ByVal v_sDateString As String, ByVal v_lClientDay As Integer, ByVal v_lClientMonth As Integer, ByVal v_lClientYear As Integer, ByVal v_dtClientSystemDate As Date) As Integer

        Dim result As Integer = 0
        Dim dtServerSystemDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check that the Client Date Format is the Server Date Format.
            If ((CDate(v_sDateString).Day) = v_lClientDay) And (CDate(v_sDateString).Month = v_lClientMonth) And (CDate(v_sDateString).Year = v_lClientYear) Then
                ' Formats match
            Else
                ' Error
                Return gPMConstants.PMEReturnCode.PMIncorrectDateFormat
            End If

            ' Get the Server Date
            dtServerSystemDate = DateTime.Now

            ' Check that the Client System Date Matches the Server System Date
            If (dtServerSystemDate.Day = (v_dtClientSystemDate.Day)) And (dtServerSystemDate.Month = v_dtClientSystemDate.Month) And (dtServerSystemDate.Year = v_dtClientSystemDate.Year) Then
                ' System Dates Match
            Else
                Return gPMConstants.PMEReturnCode.PMIncorrectSystemDate
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckClientDateFormatFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckClientDateFormat", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSources
    '
    ' Description: Get the available sources for a given username
    ' Change history
    ' RDC 07082002 created
    ' ***************************************************************** '
    Public Function GetSources(ByVal iUserID As Integer, ByRef vSources As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oDatabase As dPMDAO.Database = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' developer guide no.182
            lReturn = CType(gPMComponentServices.NewDatabase(UserName, SourceID, LanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            oDatabase.Parameters.Clear()

            lReturn = oDatabase.Parameters.Add("UserID", CStr(iUserID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oDatabase.CloseDatabase()
                oDatabase = Nothing

                Return result
            End If


            lReturn = oDatabase.SQLSelect("spu_pm_get_user_sources", "GetUserSources", True, , vSources)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oDatabase.CloseDatabase()
                oDatabase = Nothing

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSources failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSources", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: StandardInitialise
    '
    ' Description: Calls the Standard Sirius Initialise Method on a
    '              component.
    ' ***************************************************************** '
    Private Function StandardInitialise(ByRef oObject As Object, ByRef oDatabase As dPMDAO.Database, ByRef sCallingAppName As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do we have a valid Database ref to pass

            If oDatabase Is Nothing Then
                ' No, so Initialise without
                'Return CType(oObject, SSP.S4I.Interfaces.IBusiness).Initialise(g_sUsername, g_sPassword, g_iUserID, g_iSourceID, g_iLanguageID, g_iCurrencyID, g_iLogLevel, sCallingAppName)

                Return oObject.Initialise(ToSafeString(g_sUsername), ToSafeString(g_sPassword), ToSafeInteger(g_iUserID), ToSafeInteger(g_iSourceID), ToSafeInteger(g_iLanguageID), ToSafeInteger(g_iCurrencyID), ToSafeInteger(g_iLogLevel), ToSafeString(sCallingAppName))

            Else
                ' Yes, so Initialise with
                Return oObject.Initialise(ToSafeString(g_sUsername), ToSafeString(g_sPassword), ToSafeInteger(g_iUserID), ToSafeInteger(g_iSourceID), ToSafeInteger(g_iLanguageID), ToSafeInteger(g_iCurrencyID), ToSafeInteger(g_iLogLevel), ToSafeString(sCallingAppName), vDatabase:=CType(oDatabase, dPMDAO.Database))
            End If
        Catch
        End Try



        ' Error Section.
        result = gPMConstants.PMEReturnCode.PMError

        ' Set the object to nothing
        oObject = Nothing

        ' Log Error.
        bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise Object", vApp:=ACApp, vClass:=ACClass, vMethod:="StandardInitialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Logoff
    '
    ' Description: Logoff the user of this Client Manager
    '
    ' ***************************************************************** '
    Private Function Logoff() As Integer

        Dim result As Integer = 0

        Dim oUser As Bpmuser.Business
        Dim lErrorValue As gPMConstants.PMEReturnCode
        Dim oDatabase As dPMDAO.Database

        ' CTAF 20030718
        Dim sClassName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do we have a Username to logoff.
            If g_sUsername.Trim() = "" Then
                ' No, so just exit
                Return result
            End If

            ' Get a Sirius Architecture PMDAo
            oDatabase = m_oPMDAOInstances.GetPMDAOInstance(gPMConstants.PMEProductFamily.pmePFSiriusArchitecture)

            ' CTAF 20030718 - Which object are we about to create?
            sClassName = "bPMUser.Business"

            ' Get an instance of the user business object.
            'MessageBox.Show("bclientmanagercls logoff")
            oUser = New Bpmuser.Business()

            lErrorValue = oUser.Initialise("", "", 1, g_iSourceID, g_iLanguageID, g_iCurrencyID, g_iLogLevel, ACApp, vDatabase:=oDatabase)

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                oUser = Nothing
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Start - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.1.3)
            ' Logoff User
            lErrorValue = oUser.Logoff(g_sUsername, m_sUserConfigXMLDataset)

            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End - (Sankar) - (Tech Spec - Trac3039 - Saving User Preferences on Screen Lists) - (5.1.1.3)

            ' Terminate the user object.
            oUser.Dispose()
            ' Destroy the instance of the user object
            ' from memory.
            oUser = Nothing
            oDatabase = Nothing

            'PN17766 - Delete all properties for this User
            lErrorValue = CType(gPMComponentServices.DeleteAllUserProperties(g_sUsername), gPMConstants.PMEReturnCode)

            ' RFC23061998
            ' Terminate Property Manager
            If Not m_oPMPropMan Is Nothing Then
                m_oPMPropMan.Dispose()
                m_oPMPropMan = Nothing
            End If
            ' Release All User Locks
            lErrorValue = CType(ReleaseAllUserLocks(), gPMConstants.PMEReturnCode)

            ' If logged on to PMB
            ' RDC 30062004 Unixcache no longer required
            '    If (LoggedOnToPMB = True) Then
            '        ' Logoff from PMBroking
            '        Logoff = PMBLogoff()
            '    End If

            ' RDC 13082001 remove any 'hung' licences from last time this user logged on.
            lErrorValue = CType(ClearExistingLicences(g_sUsername), gPMConstants.PMEReturnCode)

            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ' do nothing
            End If

            Return result
        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' CTAF 20030718 - Hopefully more helpful error message

            Select Case Informations.Err().Number
                Case 429
                    ' Log Error.
                    bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of " & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="Logoff", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Logoff", vApp:=ACApp, vClass:=ACClass, vMethod:="Logoff", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End Select

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSystemOption
    '
    ' Description: get a value from the hidden options table
    '
    ' ***************************************************************** '
    Public Function GetSystemOption(ByVal lOptionID As Integer, ByVal iSourceID As Integer, ByRef vOptionValue As Object) As Integer

        Dim result As Integer = 0
        Dim oDatabase As dPMDAO.Database

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oDatabase = New dPMDAO.Database()


            m_lReturn = CType(gPMComponentServices.NewDatabase(UserName, SourceID, LanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            oDatabase.Parameters.Clear()

            m_lReturn = oDatabase.Parameters.Add("option_number", CStr(lOptionID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = oDatabase.Parameters.Add("branch_id", CStr(iSourceID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            m_lReturn = oDatabase.Parameters.Add("option_value", CStr(vOptionValue), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = oDatabase.SQLSelect(ACGetSystemOptionSQL, ACGetSystemOptionName, ACGetSystemOptionStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            If Not Informations.IsDBNull(oDatabase.Parameters.Item("option_value").Value) Then
                vOptionValue = oDatabase.Parameters.Item("option_value").Value
            End If

            oDatabase.CloseDatabase()

            oDatabase = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: ReleaseAllUserLocks (RFC23061998)
    '
    ' Description: Release all Locks for a User.
    '              Called at Logon and Logoff
    '
    ' Note: This may not work on some installations as PMLock
    '       may not be installed.
    '       Therefore do not report errors if it does not work.
    ' ***************************************************************** '
    Private Function ReleaseAllUserLocks() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oPMLock As bpmlock.Form
        Dim oDatabase As dPMDAO.Database = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Release Any Locks for this User

            ' Create and Initialise bPMLock
            oPMLock = New bpmlock.Form()
            lReturn = CType(StandardInitialise(oPMLock, oDatabase, ACApp), gPMConstants.PMEReturnCode)

            ' If PMTrue then we have an Instance
            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Call the UnLockAllForUser method
                lReturn = oPMLock.UnLockAllForUser(g_iUserID)
                oPMLock.Dispose()
                oPMLock = Nothing
            End If


            Return result
        Catch


            ' Error Section.
            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Policy Master Broking Methods
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: PMBLogoff
    '
    ' Description: Logs off PM Broking
    '
    ' ***************************************************************** '

    'Private Function PMBLogoff() As Integer
    '
    'Dim result As Integer = 0
    'Dim lReturnCode As gPMConstants.PMEReturnCode
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get Unix Cache
    'lReturnCode = CType(GetUnixCache(), gPMConstants.PMEReturnCode)
    '
    'If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Logoff
    '
    '
    'lReturnCode = ReflectionHelper.Invoke(m_oPMUnixCache, "Logoff", New Object(){g_sUsername})
    ' Terminate
    '
    'lReturnCode = ReflectionHelper.GetMember(m_oPMUnixCache, "Terminate")
    '
    ' Release any reference
    'm_oPMUnixCache = Nothing
    '
    ' Set Logged on to PMB flag
    'm_bLoggedOnToPMB = False
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
    'gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Failed to logoff PM Broking", ACApp, ACClass, "PMBLogoff", CStr(Informations.Err().Number), excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetUnixCache
    '
    ' Description: Gets an Instance of the Unix Cache
    '
    ' ***************************************************************** '
    Private Function GetUnixCache() As Integer

        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oPMUnixCache Is Nothing Then

            result = GetInstance(m_oPMUnixCache, "bPMUnixCache.ClientManager", ACApp)

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ClearExistingLicences
    '
    ' Description: Clears outstanding licences for given user, in case
    '              user crashed and system held licences
    '
    ' Created: RDC 13082001
    ' ***************************************************************** '
    Private Function ClearExistingLicences(ByVal sUsername As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oLicenceAdmin As bPMLicenceAdmin.LicenceAdmin



        result = gPMConstants.PMEReturnCode.PMTrue
        ' create bPMLicenceAdmin
        oLicenceAdmin = New bPMLicenceAdmin.LicenceAdmin()

        ' creates own reference to SA database

        lReturn = oLicenceAdmin.Initialise()

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oLicenceAdmin = Nothing
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' clear the licences, if any
        lReturn = oLicenceAdmin.UpdatePMUserTasks(sUsername)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oLicenceAdmin = Nothing
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        oLicenceAdmin.Dispose()

        oLicenceAdmin = Nothing

        Return result

    End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        Try

            ' Create New PMDAO Instances Collection
            m_oPMDAOInstances = New BCLIENTMANAGER.PMDAOInstances()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error Message
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the business entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
    Public Function IsReusedPassword(ByVal iUser_Id As Integer, ByVal sNewPassword As String, ByRef bIsValid As Boolean) As Integer

        Dim lReturn As Integer = 0
        Dim dtResult As New DataTable
        Dim oDatabase As dPMDAO.Database = Nothing
        Dim sPasswordEncrypted As String = ""
        Try
            lReturn = gPMConstants.PMEReturnCode.PMTrue
            lReturn = CType(gPMComponentServices.NewDatabase(UserName, SourceID, LanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
            oDatabase.Parameters.Clear()

            lReturn = oDatabase.Parameters.Add(sName:="user_id", vValue:=CInt(iUser_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            ' Execute SQL Statement

            lReturn = oDatabase.ExecuteDataTable(sSQL:=ACGetpasswordhistorySQL, sSQLName:=ACGetpasswordhistoryName, bStoredProcedure:=ACGetpasswordhistoryStored, oRecordset:=dtResult)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oDatabase.CloseDatabase()
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If dtResult.Rows.Count > 0 Then
                For i As Integer = 0 To dtResult.Rows.Count - 1
                    sPasswordEncrypted = dtResult.Rows(i)("historic_password").ToString()

                    ' check the password’s match
                    If bPMFunc.CheckPassword(sNewPassword, sPasswordEncrypted) Then
                        bIsValid = 0
                        Return lReturn
                    End If
                Next
                bIsValid = 1
            Else
                bIsValid = 1
            End If

            Return lReturn

        Catch ex As Exception

            ' Error.
            lReturn = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsReusedPassword Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsReusedPassword", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return lReturn
        End Try
    End Function

End Class