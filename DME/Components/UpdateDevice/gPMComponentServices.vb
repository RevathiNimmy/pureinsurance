Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Module gPMComponentServices

    ' ***************************************************************** '
    ' Class Name: ServerBusinessCS
    '
    ' Date: 23rd June 1998
    '
    ' Description: Server Component Services for Components that
    '              require a link to Server.
    ' Edit History:
    ' RFC 23/06/1998 - Original
    ' RFC 26/08/1998 - GetDSN, CheckDatabase and NewDatabase added.
    ' RFC 20/11/1998 - Added Calc&Uncalc CombinedKey methods.
    ' RFC 20/11/1998 - All LogMessagePopup changed to LogMessage.
    ' RFC 20/11/1998 - CheckPMProductInstalled added.
    'RFC060799 - Added GeminiII Product Family, DSN etc etc
    ' RDC 13062002 changed to BAS module
    '              all referencing projects will need reference to bPMPropertyManager
    ' SJP 06072002 Changed CalcCombinedKey to return Party Cnt (allowing for
    '               multiple branches >8.  Removed UnCalcCombinedKey.
    '               Copied both functions as were used previously and renamed
    '               calcCombinedGenericKey, unCalcCombinedGenericKey
    ' DD 23/09/2002: Changed GetDSN to merge DSNs to reduce database
    ' connections and a chance of deadlocks occurring.
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "PMServerBusinessCS"

    ' RDC 16102002 SysAdmin status
    Private Const ACGetSysAdminStatusStored As Boolean = True
    Private Const ACGetSysAdminStatusName As String = "GetSysAdminStatus"
    Private Const ACGetSysAdminStatusSQL As String = "spu_get_sys_admin_status"

    ' PUBLIC Data Members (Begin)
    ' RDC 1306002 added from orginal CompServ MainModule
    Public Const ACProductFamilyPrefix As String = "PF"
    Public Const ACCombinedKeyPower As Integer = 28

    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' RFC 260898
    ' ***************************************************************** '
    ' Name: CheckDatabase
    '
    ' Description: Checks to see if the supplied PMDAO.Database instance
    '              is correct for the PM Product Family.
    '              If it isn't it creates a new one.
    ' ***************************************************************** '
    ' REMOVE GLOBAL VARIABLES
#If PD_EARLYBOUND = 1 Then

	Public Function CheckDatabase( _
	        ByVal v_sUsername As String, _
	        ByVal v_iSourceID As Integer, _
	        ByVal v_iLanguageID As Integer, _
	        ByVal v_lPMProductFamily As Long, _
	        ByRef r_bNewInstanceCreated As Boolean, _
	        ByRef r_oCheckedDatabase As DPMDAO.Database, _
	        Optional ByVal v_vDatabase As Variant) As Long
#Else
    Public Function CheckDatabase(ByVal v_sUsername As String, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_lPMProductFamily As Integer, ByRef r_bNewInstanceCreated As Boolean, ByRef r_oCheckedDatabase As Object, Optional ByVal v_vDatabase As Object = Nothing) As Integer
        Dim result As Integer = 0
#End If

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sCurrentDSN, sRequiredDSN As String
        Dim bNewInstanceRequired As Boolean
        Dim sPosInFunction As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        sPosInFunction = "1" 'Temp Debug Line

        r_bNewInstanceCreated = False
        bNewInstanceRequired = False

        sPosInFunction = "2" 'Temp Debug Line

        ' Have we a Database Parameter

        If Information.IsNothing(v_vDatabase) Then

            sPosInFunction = "3" 'Temp Debug Line

            ' No then we need to create a New Instance
            bNewInstanceRequired = True
        End If

        sPosInFunction = "4" 'Temp Debug Line

        If Not bNewInstanceRequired Then

            sPosInFunction = "5" 'Temp Debug Line

            ' Have we a valid Object Reference?
            If Not Information.IsReference(v_vDatabase) Then

                sPosInFunction = "6" 'Temp Debug Line

                ' No then we need to create a New Instance
                bNewInstanceRequired = True
            End If
        End If

        sPosInFunction = "7" 'Temp Debug Line

        If Not bNewInstanceRequired Then

            sPosInFunction = "8" 'Temp Debug Line

            ' Get the Current Database DSN
            Try

                sCurrentDSN = v_vDatabase.CurrentDSN

            Catch
            End Try



            sPosInFunction = "9" 'Temp Debug Line

            'ED 04/12/2002 - Only compare if Current DSN not
            '              - prefixed with XFER
            If Not sCurrentDSN.StartsWith("XFER") Then

                sPosInFunction = "10" 'Temp Debug Line

                ' Get the required DSN
                sRequiredDSN = GetDSN(v_lPMProductFamily:=v_lPMProductFamily)

                sPosInFunction = "11" 'Temp Debug Line

                ' Are they the same
                If sCurrentDSN.Trim() <> sRequiredDSN.Trim() Then

                    sPosInFunction = "12" 'Temp Debug Line

                    ' No then we need to create a New Instance
                    bNewInstanceRequired = True
                End If
            End If

        End If

        sPosInFunction = "13" 'Temp Debug Line

        ' Do we need to create a new Instance
        If bNewInstanceRequired Then

            sPosInFunction = "14" 'Temp Debug Line

            ' Yes.
            ' Use NewDatabase method to create one.
            ' REMOVE GLOBAL VARIABLES
            r_bNewInstanceCreated = True

            lReturn = CType(NewDatabase(v_sUsername:=v_sUsername, v_iSourceID:=v_iSourceID, v_iLanguageID:=v_iLanguageID, v_lPMProductFamily:=v_lPMProductFamily, r_oDatabase:=r_oCheckedDatabase), gPMConstants.PMEReturnCode)

            sPosInFunction = "15" 'Temp Debug Line

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_oCheckedDatabase = Nothing
                Return result
            End If

            sPosInFunction = "16" 'Temp Debug Line
        Else

            sPosInFunction = "17" 'Temp Debug Line

            ' No new instance required,
            ' so indicate that we did not create one.
            r_bNewInstanceCreated = False
            ' Return the checked Instance
            r_oCheckedDatabase = v_vDatabase
        End If

        sPosInFunction = "18" 'Temp Debug Line

        Return result

Err_CheckDatabase:

        ' Error Section.
        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check Database for DSN - " & sRequiredDSN, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDatabase", excep:=New Exception(Information.Err().Description & " Position = " & sPosInFunction))

        Return result

    End Function

    ' RFC 260898
    ' ***************************************************************** '
    ' Name: NewDatabase
    '
    ' Description: Creates a new instance of PMDAO.Database connected
    '              to the correct database for the suplied
    '              PM Product Family.
    ' ***************************************************************** '
    ' REMOVE GLOBAL VARIABLES
#If PD_EARLYBOUND = 1 Then

	Public Function NewDatabase( _
	        ByVal v_sUsername As String, _
	        ByVal v_iSourceID As Integer, _
	        ByVal v_iLanguageID As Integer, _
	        ByVal v_lPMProductFamily As Long, _
	        ByRef r_oDatabase As DPMDAO.Database) As Long
#Else
    Public Function NewDatabase(ByVal v_sUsername As String, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_lPMProductFamily As Integer, ByRef r_oDatabase As dPMDAO.Database) As Integer
        Dim result As Integer = 0
#End If

        Dim sDSN As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the DSN for this Product Family
            sDSN = GetDSN(v_lPMProductFamily:=v_lPMProductFamily)

            ' Create a New instance of PMDAO
#If PD_EARLYBOUND = 1 Then

			Set r_oDatabase = New DPMDAO.Database
#Else
            r_oDatabase = New dPMDAO.Database()
#End If

            ' Open the database, using the DSN if we know what it should be

            ' RDC 27062002 new parameters required for OpenDatabase method

            If sDSN.Trim() = "" Then
                Return r_oDatabase.OpenDatabase(sSiriusUsername:=v_sUsername, iSourceID:=CShort(v_iSourceID), iLanguageID:=CShort(v_iLanguageID), sCallingAppName:=ACApp)
            Else
                Return r_oDatabase.OpenDatabase(sSiriusUsername:=v_sUsername, iSourceID:=CShort(v_iSourceID), iLanguageID:=CShort(v_iLanguageID), sCallingAppName:=ACApp, vDSN:=sDSN)
                '        NewDatabase = r_oDatabase.OpenDatabase(vDSN:=sDSN)
            End If

        Catch
        End Try



        ' Error Section.
        result = gPMConstants.PMEReturnCode.PMError

        r_oDatabase = Nothing

        ' Log Error.
        LogMessageToFile(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create a new instance of PMDAO for DSN - " & sDSN, vApp:=ACApp, vClass:=ACClass, vMethod:="NewDatabase", excep:=New Exception(Information.Err().Description))

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateBusinessObject
    '
    ' Description: Creates an instance of the class name passed.
    '
    ' ***************************************************************** '
#If PD_EARLYBOUND = 1 Then

	Public Function CreateBusinessObject( _
	        ByRef r_oObject As Object, _
	        ByVal v_sClassName As String, _
	        ByVal v_sCallingAppName As String, _
	        ByVal v_sUsername As String, _
	        ByVal v_sPassword As String, _
	        ByVal v_iUserID As Integer, _
	        ByVal v_iSourceID As Integer, _
	        ByVal v_iLanguageID As Integer, _
	        ByVal v_iCurrencyID As Integer, _
	        ByVal v_iLogLevel As Integer, _
	        Optional ByVal v_oDatabase As DPMDAO.Database = Nothing) As Long
#Else
    Public Function CreateBusinessObject(ByRef r_oObject As Object, ByVal v_sClassName As String, ByVal v_sCallingAppName As String, ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, Optional ByVal v_oDatabase As Object = Nothing) As Integer
        Dim result As Integer = 0
#End If

        Dim lReturnCode As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object.
            ' NOTE: Because of passing in the class name,
            ' the object can ONLY be created late bound.
            r_oObject = Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType(v_sClassName + "," + v_sClassName.Substring(0, v_sClassName.LastIndexOf(".")))).FullName, v_sClassName).Unwrap()

            ' Do we have a valid Database ref to pass
            If v_oDatabase Is Nothing Then
                ' No, so Initialise without

                lReturnCode = CType(r_oObject, SSP.S4I.Interfaces.IBusiness).Initialise(sUserName:=v_sUsername, sPassword:=v_sPassword, iUserID:=v_iUserID, iSourceID:=v_iSourceID, iLanguageID:=v_iLanguageID, iCurrencyID:=v_iCurrencyID, iLogLevel:=v_iLogLevel, sCallingAppName:=v_sCallingAppName)

            Else
                ' Yes, so Initialise with

                lReturnCode = r_oObject.Initialise(sUserName:=v_sUsername, sPassword:=v_sPassword, iUserID:=v_iUserID, iSourceID:=v_iSourceID, iLanguageID:=v_iLanguageID, iCurrencyID:=v_iCurrencyID, iLogLevel:=v_iLogLevel, sCallingAppName:=v_sCallingAppName, vDatabase:=v_oDatabase)
            End If

            ' Check for errors.
            If lReturnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Set the object to nothing

            ' Log Error.
            LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object instance (" & v_sClassName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateUserProperty
    '
    ' Description: Updates a Property in Property Manager
    '
    ' ***************************************************************** '
    Public Function UpdateUserProperty(ByVal v_sUsername As String, ByVal v_sPropertyName As String, ByVal v_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(UpdateProperty(v_sGroupName:=v_sUsername, v_sPropertyName:=v_sPropertyName, v_vPropertyValue:=v_vPropertyValue), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserProperty", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserProperty
    '
    ' Description: Gets a Property from Property Manager
    '
    ' ***************************************************************** '
    Public Function GetUserProperty(ByVal v_sUsername As String, ByVal v_sPropertyName As String, ByRef r_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(GetProperty(v_sGroupName:=v_sUsername, v_sPropertyName:=v_sPropertyName, r_vPropertyValue:=r_vPropertyValue), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserProperty", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteUserProperty
    '
    ' Description: Deletes a specific property for a specific user.
    '
    ' ***************************************************************** '
    Public Function DeleteUserProperty(ByVal v_sUsername As String, ByVal v_sPropertyName As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(DeleteProperty(v_sGroupName:=v_sUsername, v_sPropertyName:=v_sPropertyName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteUserProperty", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAllUserProperties
    '
    ' Description: Deletes all Properties for a given user.
    '
    ' ***************************************************************** '
    Public Function DeleteAllUserProperties(ByVal v_sUsername As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(DeleteGroup(v_sGroupName:=v_sUsername), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete All Properties for User - " & v_sUsername, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllUserProperties", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateProductProperty
    '
    ' Description: Updates a Property in Property Manager
    '
    ' ***************************************************************** '
    Public Function UpdateProductProperty(ByVal v_lPMEProductFamily As Integer, ByVal v_sPropertyName As String, ByVal v_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sGroupName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sGroupName = (ACProductFamilyPrefix & CStr(v_lPMEProductFamily)).Trim()

            lReturn = CType(UpdateProperty(v_sGroupName:=sGroupName, v_sPropertyName:=v_sPropertyName, v_vPropertyValue:=v_vPropertyValue), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateProductProperty", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProductProperty
    '
    ' Description: Gets a Property from Property Manager
    '
    ' ***************************************************************** '
    Public Function GetProductProperty(ByVal v_lPMEProductFamily As Integer, ByVal v_sPropertyName As String, ByRef r_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sGroupName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sGroupName = (ACProductFamilyPrefix & CStr(v_lPMEProductFamily)).Trim()

            lReturn = CType(GetProperty(v_sGroupName:=sGroupName, v_sPropertyName:=v_sPropertyName, r_vPropertyValue:=r_vPropertyValue), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductProperty", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteProductProperty
    '
    ' Description: Deletes a Property from Property Manager
    '
    ' ***************************************************************** '
    Public Function DeleteProductProperty(ByVal v_lPMEProductFamily As Integer, ByVal v_sPropertyName As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sGroupName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sGroupName = (ACProductFamilyPrefix & CStr(v_lPMEProductFamily)).Trim()

            lReturn = CType(DeleteProperty(v_sGroupName:=sGroupName, v_sPropertyName:=v_sPropertyName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteProductProperty", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAllProductProperties
    '
    ' Description: Deletes all Properties for a given Product
    '
    ' ***************************************************************** '
    Public Function DeleteAllProductProperties(ByVal v_lPMEProductFamily As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sGroupName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sGroupName = (ACProductFamilyPrefix & CStr(v_lPMEProductFamily)).Trim()

            lReturn = CType(DeleteGroup(v_sGroupName:=sGroupName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete All Properites for Group - " & sGroupName, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllProductProperties", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CalcCombinedKey - 'SJP 06062002
    '
    ' Description: Returns Party Cnt in the r_lCombinedKey
    '   This allows for multiple branches
    ' ***************************************************************** '
    Public Function calccombinedkey(ByVal v_lSourceID As Integer, ByVal v_lKeyID As Integer, ByRef r_lCombinedKeyID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lSourceID < 1 Then
                LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="SourceID must be greater than zero.", vApp:=ACApp, vClass:=ACClass, vMethod:="CalcCombinedKey")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SJP 03/06/2002
            '   This will equal what is passed in.
            r_lCombinedKeyID = r_lCombinedKeyID

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to calculate a combined key for SourceID - " & v_lSourceID & _
                             " KeyID - " & CStr(v_lKeyID), vApp:=ACApp, vClass:=ACClass, vMethod:="CalcCombinedKey", excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CheckPMProductInstalled  - 'RFC201198
    '
    ' Description: Checks to see if the PMProduct supplied has been
    '              installed.
    '
    ' Note: The code to do this should really be in the bPMProduct
    '       component, though as it doesn't exist yet it will have to
    '       go here.
    ' ***************************************************************** '
    ' REMOVE GLOBAL VARIABLES
    'Public Function CheckPMProductInstalled( _
    ''    ByVal v_lPMProductFamily As Long, _
    ''    ByRef r_bInstalled As Boolean) As Long
    Public Function CheckPMProductInstalled(ByVal v_sUsername As String, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_lPMProductFamily As Integer, ByRef r_bInstalled As Boolean) As Integer

        Dim result As Integer = 0
#If PD_EARLYBOUND = 1 Then

		Dim oDatabase As DPMDAO.Database
#Else
        Dim oDatabase As Object
#End If
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL, sPMProductCode As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an Instance of PMDAO Pointing to the Sirius Architecture.

            lReturn = CType(NewDatabase(v_sUsername:=v_sUsername, v_iSourceID:=v_iSourceID, v_iLanguageID:=v_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Product Code for this Product Family
            sPMProductCode = PMProductCode(v_lPMProductFamily)

            sSQL = ""
            sSQL = sSQL & "SELECT pmproduct_id FROM PMProduct "
            sSQL = sSQL & "WHERE code = {code} "
            sSQL = sSQL & "AND is_deleted = 0"

            r_bInstalled = False

            With oDatabase

                .Parameters.Clear()


                lReturn = .Parameters.Add(sName:="code", vValue:=sPMProductCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement

                lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckPMProductInstalled", bStoredProcedure:=False)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If we have a record then the Product is installed

                r_bInstalled = Not (.Records.Count < 1)

            End With


            lReturn = oDatabase.CloseDatabase

            oDatabase = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check Product Installation for Product Code - " & sPMProductCode, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPMProductInstalled", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSysAdminAccessStatus
    '
    ' Description: check if user is member of a SysAdmin user group.
    '
    ' History: RDC 17102002 created
    ' ***************************************************************** '
    ' REMOVE GLOBAL VARIABLES
    'Public Function GetSysAdminAccessStatus( _
    ''            ByRef lStatus As Long, _
    ''            ByRef oDatabase As Object) As Long
    Public Function GetSysAdminAccessStatus(ByVal v_sUsername As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByRef lStatus As Integer, ByRef oDatabase As Object) As Integer

        Dim result As Integer = 0
        Dim bDBOpened As Boolean
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If oDatabase Is Nothing Then

                lReturn = CType(NewDatabase(v_sUsername:=v_sUsername, v_iSourceID:=v_iSourceID, v_iLanguageID:=v_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                bDBOpened = True
            End If


            oDatabase.Parameters.Clear()


            lReturn = oDatabase.Parameters.Add(sName:="user_id", vValue:=v_iUserID, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If bDBOpened Then

                    oDatabase.CloseDatabase()
                    oDatabase = Nothing
                End If

                Return result
            End If


            lReturn = oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If bDBOpened Then

                    oDatabase.CloseDatabase()
                    oDatabase = Nothing
                End If

                Return result
            End If


            lReturn = oDatabase.SQLSelect(sSQL:=ACGetSysAdminStatusSQL, sSQLName:=ACGetSysAdminStatusName, bStoredProcedure:=ACGetSysAdminStatusStored)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If bDBOpened Then

                    oDatabase.CloseDatabase()
                    oDatabase = Nothing
                End If

                Return result
            End If


            lStatus = oDatabase.Records.Fields("sys_admin_count").Value

            If bDBOpened Then

                oDatabase.CloseDatabase()
                oDatabase = Nothing
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSysAdminAccessStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSysAdminAccessStatus", excep:=excep)

            Return result
        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: UpdateProperty
    '
    ' Description: Updates a Property in Property Manager
    '
    ' ***************************************************************** '
    Private Function UpdateProperty(ByVal v_sGroupName As String, ByVal v_sPropertyName As String, ByVal v_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
#If PD_EARLYBOUND = 1 Then

		Dim oPM As bPMPropertyManager.BusinessXML
#Else
        Dim oPM As bPMPropertyManager.BusinessXML
#End If

        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

#If PD_EARLYBOUND = 1 Then

			Set oPM = New bPMPropertyManager.BusinessXML
#Else
        oPM = New bPMPropertyManager.BusinessXML()
#End If

        lReturn = CType(CType(oPM, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = CType(oPM.UpdateProperty(v_sGroupName:=v_sGroupName, v_sPropertyName:=v_sPropertyName, v_vPropertyValue:=v_vPropertyValue), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oPM.Dispose()
        oPM = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperty
    '
    ' Description: Gets a Property from Property Manager
    '
    ' ***************************************************************** '
    Private Function GetProperty(ByVal v_sGroupName As String, ByVal v_sPropertyName As String, ByRef r_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
#If PD_EARLYBOUND = 1 Then

		Dim oPM As bPMPropertyManager.BusinessXML
#Else
        Dim oPM As bPMPropertyManager.BusinessXML
#End If

        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

#If PD_EARLYBOUND = 1 Then

			Set oPM = New bPMPropertyManager.BusinessXML
#Else
        oPM = New bPMPropertyManager.BusinessXML()
#End If

        lReturn = CType(CType(oPM, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = CType(oPM.GetProperty(v_sGroupName:=v_sGroupName, v_sPropertyName:=v_sPropertyName, r_vPropertyValue:=r_vPropertyValue), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oPM.Dispose()
        oPM = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteProperty
    '
    ' Description: Deletes a Property from Property Manager
    '
    ' ***************************************************************** '
    Private Function DeleteProperty(ByVal v_sGroupName As String, ByVal v_sPropertyName As String) As Integer

        Dim result As Integer = 0
#If PD_EARLYBOUND = 1 Then

		Dim oPM As bPMPropertyManager.BusinessXML
#Else
        Dim oPM As bPMPropertyManager.BusinessXML
#End If

        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

#If PD_EARLYBOUND = 1 Then

			Set oPM = New bPMPropertyManager.BusinessXML
#Else
        oPM = New bPMPropertyManager.BusinessXML()
#End If

        lReturn = CType(CType(oPM, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = CType(oPM.DeleteProperty(v_sGroupName:=v_sGroupName, v_sPropertyName:=v_sPropertyName), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oPM.Dispose()
        oPM = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteGroup
    '
    ' Description: Deletes a Group from Property Manager
    '
    ' ***************************************************************** '
    Private Function DeleteGroup(ByVal v_sGroupName As String) As Integer

        Dim result As Integer = 0
#If PD_EARLYBOUND = 1 Then

		Dim oPM As bPMPropertyManager.BusinessXML
#Else
        Dim oPM As bPMPropertyManager.BusinessXML
#End If

        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

#If PD_EARLYBOUND = 1 Then

			Set oPM = New bPMPropertyManager.BusinessXML
#Else
        oPM = New bPMPropertyManager.BusinessXML()
#End If

        lReturn = CType(CType(oPM, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = CType(oPM.DeleteGroup(v_sGroupName:=v_sGroupName), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oPM.Dispose()
        oPM = Nothing

        Return result

    End Function

    ' RFC 260898
    ' ***************************************************************** '
    ' Name: GetDSN
    '
    ' Description: Returns the relevant DSN name for the
    '              supplied PM Product Family.
    ' Changes:
    ' DD 23/09/2002: Merged DSNs to reduce database connections and
    ' a chance of deadlocks occurring.
    ' RAM20050104   : Added code to support SWIFT DSN
    ' ***************************************************************** '
    Private Function GetDSN(ByVal v_lPMProductFamily As Integer) As String

        Dim result As String = String.Empty


        result = ""

        ' Work out the correct DSN to open

        Select Case v_lPMProductFamily
            Case gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, gPMConstants.PMEProductFamily.pmePFSiriusUnderwriting, gPMConstants.PMEProductFamily.pmePFSiriusBroking, gPMConstants.PMEProductFamily.pmePFSiriusSolutions, gPMConstants.PMEProductFamily.pmePFOrion, gPMConstants.PMEProductFamily.pmePFDocumaster, gPMConstants.PMEProductFamily.pmePFGeminiII, gPMConstants.PMEProductFamily.pmePFClaims
                Return PMSiriusSolutionsDSN
                '      Case pmePFSiriusArchitecture
                '        GetDSN = PMSiriusArchitectureDSN
                '      Case pmePFSiriusUnderwriting
                '        GetDSN = PMSiriusUnderwritingDSN
                '      Case pmePFSiriusBroking
                '        GetDSN = PMSiriusBrokingDSN
                '      Case pmePFSiriusSolutions
                '        GetDSN = PMSiriusSolutionsDSN
            Case gPMConstants.PMEProductFamily.pmePFGemini
                Return gPMConstants.PMGeminiDSN
                '      Case pmePFOrion
                '        GetDSN = PMOrionDSN
            Case gPMConstants.PMEProductFamily.pmePFVoyager
                Return gPMConstants.PMVoyagerDSN
            Case gPMConstants.PMEProductFamily.pmePFMercury
                Return PMMercuryDSN
                '      Case pmePFDocumaster
                '        GetDSN = PMDocumasterDSN
            Case gPMConstants.PMEProductFamily.pmePFNirvana
                Return PMNirvanaDSN
                'RFC060799 - Added GeminiII Product Family, DSN etc etc
                '      Case pmePFGeminiII
                '        GetDSN = PMGeminiIIDSN
                '      Case pmePFClaims
                '        GetDSN = PMClaimsDSN
                'RDC 13092002
            Case gPMConstants.PMEProductFamily.pmePFDocumasterScan
                Return PMDocumasterScanDSN
                'JSB 09/09/03
            Case gPMConstants.PMEProductFamily.pmePFMediquote
                Return PMMediquoteDSN
            Case gPMConstants.PMEProductFamily.pmePFSwift
                Return PMSwiftDSN
            Case Else
                Return ""
        End Select




        ' Error Section.

        Return ""

    End Function

    ' PRIVATE Methods (End)
End Module

