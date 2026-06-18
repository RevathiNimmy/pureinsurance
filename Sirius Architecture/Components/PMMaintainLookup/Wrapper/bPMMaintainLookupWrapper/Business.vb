Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no 129. 
Imports SharedFiles


<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 16/11/1998
    '
    ' Description: Business functions for PMMaintainLookupWrapper.
    '
    ' Edit History:
    ' DAK291099 - Change GetTables to search new table PMUser_Maintain_lookup
    '             for allowed tables.
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    Private Const ACClass As String = "Business"

    ' Return value
    Private m_lReturn As Integer

    ' Cache the architecture tables
    Private m_vArchitectureArray As Object

    ' Product family
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Get an instance of object manager
            ' RDC 24112000 Now this us just plain silly. Forces a login prompt on the server!
            'Set g_oObjectManager = New bObjectManager.ObjectManager

            ' Initialise it
            '    m_lReturn& = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            '    If (m_lReturn& <> PMTrue) Then
            '        Initialise = PMFalse
            '        Exit Function
            '    End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description: Standard terminate function
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
                g_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetTables
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetTables(ByVal v_lPMProductID As Integer, ByRef r_vArray As Object) As Integer

        'Dim bClose As Boolean
        Dim result As Integer = 0
        Dim oDatabase As dPMDAO.Database
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim cServices As sPMServerCS.PMServerBusinessCS
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a connection to the architecture database
            oDatabase = New dPMDAO.Database()
            m_lReturn = gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'DAK291199
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oDatabase.Parameters.Clear()

            m_lReturn = oDatabase.Parameters.Add(sName:="pmproduct_id", vValue:=CStr(v_lPMProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'DAK291199
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the SQL
            m_lReturn = oDatabase.SQLSelect(sSQL:=ACGetTablesSQL, sSQLName:=ACGetTablesName, bStoredProcedure:=ACGetTablesStored, lNumberRecords:=-1, vResultArray:=vArray)
            'DAK291199
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Close the instance of the database reference
            m_lReturn = oDatabase.CloseDatabase()
            'DAK291199
            oDatabase = Nothing

            If Not Information.IsArray(vArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the return array


            r_vArray = vArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTables", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTable
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function GetTable(ByVal v_lPMProductID As Integer, ByVal v_sTableName As String, ByRef r_iPrivilegeLevel As Integer, ByRef r_sLinkedCaption As String, ByRef r_sLinkedObjectName As String, ByRef r_sLinkedClassName As String, Optional ByRef r_sInterface_component As String = "") As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oDatabase As dPMDAO.Database
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim cServices As sPMServerCS.PMServerBusinessCS
        Dim oRecord As dPMDAO.Records
        Dim lLinkedCaptionID As Integer
        Dim sLinkedCaption As String = ""
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oComponentServices As PMServerBusinessCS
        Dim oCaption As bPMCaption.Business
        ' developer guide no. 17
        Dim vCaptionID As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a connection to the architecture database
            oDatabase = New dPMDAO.Database()

            ' New instance of component services
            '    Set cServices = New sPMServerCS.PMServerBusinessCS

            ' Get a new reference to the database
            '    m_lReturn& = cServices.NewDatabase(v_lPMProductFamily:=pmePFSiriusArchitecture, _
            'r_oDatabase:=oDatabase)
            m_lReturn = gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase)

            ' Remove instance of component services
            '    Set cServices = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'DAK291199
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oDatabase.Parameters.Clear()

            m_lReturn = oDatabase.Parameters.Add(sName:="pmproduct_id", vValue:=CStr(v_lPMProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'DAK291199
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oDatabase.Parameters.Add(sName:="table_name", vValue:=v_sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            'DAK291199
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the SQL
            'DAK291199
            m_lReturn = oDatabase.SQLSelect(sSQL:=ACGetTableSQL, sSQLName:=ACGetTableName, bStoredProcedure:=ACGetTableStored, bKeepNulls:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'DAK291199
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oDatabase.Records.Count() = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Close the instance of the database reference
                m_lReturn = oDatabase.CloseDatabase()
                'DAK291199
                oDatabase = Nothing
                Return result
            End If

            ' developer guide no. 162
            oRecord = oDatabase.Records.Item(0)

            r_iPrivilegeLevel = oRecord.Fields()("edit_privilege_level")
            vCaptionID = oRecord.Fields()("linked_caption_id")

            ' RDC 20052003 test for NULL properly!

            If Convert.IsDBNull(vCaptionID) Or IsNothing(vCaptionID) Then
                'If vCaptionID = Null Then
                r_sLinkedCaption = ""
                r_sLinkedObjectName = ""
                r_sLinkedClassName = ""
            Else
                '        Set oComponentServices = New PMServerBusinessCS
                '        m_lReturn = oComponentServices.CreateBusinessObject( _
                'r_oObject:=oCaption, _
                'v_sClassName:="bPMCaption.Business", _
                'v_sCallingAppName:=ACApp, _
                'v_sUserName:=g_sUsername, _
                'v_sPassword:=g_sPassword, _
                'v_iUserID:=g_iUserID, _
                'v_iSourceID:=g_iSourceID, _
                'v_iLanguageId:=g_iLanguageID, _
                'v_iCurrencyID:=g_iCurrencyID, _
                'v_iLogLevel:=g_iLogLevel)

                oCaption = New bPMCaption.Business
                m_lReturn = oCaption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

                '        Set oComponentServices = Nothing

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Close the instance of the database reference
                    m_lReturn = oDatabase.CloseDatabase()
                    Return result
                End If

                lLinkedCaptionID = vCaptionID

                m_lReturn = oCaption.GetCaptionDesc(v_lCaptionID:=lLinkedCaptionID, r_sCaption:=sLinkedCaption)
                oCaption = Nothing
                r_sLinkedCaption = sLinkedCaption
                r_sLinkedObjectName = oRecord.Fields()("linked_object_name")
                r_sLinkedClassName = oRecord.Fields()("linked_class_name")
            End If
            r_sInterface_component = gPMFunctions.ToSafeString(oRecord.Fields()("Interface_Component"))
            ' Close the instance of the database reference
            m_lReturn = oDatabase.CloseDatabase()
            'DAK291199
            oDatabase = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTable", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProductCode
    '
    ' Description: Uses bPMLookup to get the code from an id
    '
    '
    ' ***************************************************************** '
    Public Function GetProductCode(ByVal v_lID As Integer, ByRef r_sCode As String) As Integer

        Dim result As Integer = 0
        Dim oLookup As bPMLookup.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RDC 24112000 user createobject instead START
            '    m_lReturn& = g_oObjectManager.GetInstance(oObject:=oLookup, _
            'sClassName:="bPMLookup.Business", _
            'vInstanceManager:=PMGetViaClientManager)
            '    If (m_lReturn& <> PMTrue) Then
            '        GetProductCode = PMFalse
            '        Exit Function
            '    End If

            oLookup = New bPMLookup.Business()

            If oLookup Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' developer guide no. 9
            m_lReturn = oLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oLookup = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' RDC 24112000 END

            ' Set to look on the architecture table
            oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

            ' Use bPMLookup to get the code from the id
            m_lReturn = oLookup.GetCodeFromID(v_sTableName:=ACProductTable, v_lID:=v_lID, r_sCode:=r_sCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Terminate the instance
            oLookup.Dispose()

            oLookup = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserAuthority
    '
    ' Description: Gets the User Groups that the current user is a
    '              Supervisor of.
    '
    ' ***************************************************************** '
    Public Function GetUserAuthority(ByRef r_bIsAdministrator As Boolean, ByRef r_vSupervisedGroups As Object) As Integer

        Dim result As Integer = 0
        Dim oUserGroup As bPMUserGroup.Utilities
        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS
        Dim oDatabase As dPMDAO.Database

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a connection to the architecture database
            oDatabase = New dPMDAO.Database()
            m_lReturn = gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=oDatabase)


            oUserGroup = New bPMUserGroup.Utilities
            m_lReturn = oUserGroup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=oDatabase)

            '    Set oCS = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing
                Return result
            End If

            ' Is the User an Administrator

            m_lReturn = oUserGroup.IsUserAdministrator(v_iUserID:=m_iUserID, r_bIsAdministrator:=r_bIsAdministrator)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                oUserGroup.Dispose()
                oUserGroup = Nothing
                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing
                Return result
            End If

            ' Get the Groups they Supervise
            m_lReturn = oUserGroup.GetGroupsSupervisedByUser(v_iUserID:=m_iUserID, r_vSupervisedGroups:=r_vSupervisedGroups)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                oUserGroup.Dispose()
                oUserGroup = Nothing
                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing
                Return result
            End If


            oUserGroup.Dispose()
            oUserGroup = Nothing
            m_lReturn = oDatabase.CloseDatabase()
            oDatabase = Nothing

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSysAdminStatus
    '
    ' Description: check if user is member of a SysAdmin user group.
    '
    ' History: RDC 17102002 created
    ' ***************************************************************** '
    Public Function GetSysAdminStatus(ByRef lStatus As Integer) As Integer

        Dim result As Integer = 0
        Dim oDatabase As dPMDAO.Database

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = gPMComponentServices.GetSysAdminAccessStatus(m_sUsername, m_iUserID, m_iSourceID, m_iLanguageID, lStatus, oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSysAdminStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSysAdminStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
