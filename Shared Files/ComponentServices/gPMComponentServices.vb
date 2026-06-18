Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
<System.Runtime.InteropServices.ProgId("gPMComponentServices_NET.gPMComponentServices")> _
Public Module gPMComponentServices
    <ThreadStatic()> Private oCache As New Hashtable
    Private Const ACClass As String = "PMServerBusinessCS"
    Private Const ACKeyDelimiter As String = " "
    Private Const ACGetSysAdminStatusStored As Boolean = True
    Private Const ACGetSysAdminStatusName As String = "GetSysAdminStatus"
    Private Const ACGetSysAdminStatusSQL As String = "spu_get_sys_admin_status"

    Public Const ACProductFamilyPrefix As String = "PF"
    Public Const ACCombinedKeyPower As Integer = 28

    Public Function CheckDatabase(ByVal v_sUsername As String, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_lPMProductFamily As Integer, ByRef r_bNewInstanceCreated As Boolean, ByRef r_oCheckedDatabase As Object, Optional ByVal v_vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bNewInstanceRequired As Boolean
        Dim sPosInFunction As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue
        r_bNewInstanceCreated = False
        bNewInstanceRequired = False

        ' Have we a Database Parameter
        If Information.IsNothing(v_vDatabase) Then
            bNewInstanceRequired = True
        End If

        If Not bNewInstanceRequired Then
            ' Have we a valid Object Reference?
            If Not Information.IsReference(v_vDatabase) Then
                bNewInstanceRequired = True
            End If
        End If

        ' Do we need to create a new Instance
        If bNewInstanceRequired Then
            r_bNewInstanceCreated = True
            lReturn = CType(NewDatabase(v_sUsername:=v_sUsername, v_iSourceID:=v_iSourceID, v_iLanguageID:=v_iLanguageID, v_lPMProductFamily:=v_lPMProductFamily, r_oDatabase:=r_oCheckedDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_oCheckedDatabase = Nothing
                Return result
            End If
        Else

            r_bNewInstanceCreated = False
            ' Return the checked Instance
            r_oCheckedDatabase = v_vDatabase
        End If
        Return result
Err_CheckDatabase:
        ' Error Section.
        result = gPMConstants.PMEReturnCode.PMError
        ' Log Error.
        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
        oDict.Add("v_iSourceID", v_iSourceID)
        oDict.Add("v_iLanguageID", v_iLanguageID)
        gPMFunctions.LogMessageToFile(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check Database for DSN - ", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDatabase", excep:=New Exception(Information.Err().Description & " Position = " & sPosInFunction), oDicParms:=oDict)
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
    Public Function NewDatabase(ByVal v_sUsername As String, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_lPMProductFamily As Integer, ByRef r_oDatabase As dPMDAO.Database) As Integer
        Dim result As Integer = 0
        Dim sDSN As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Create a New instance of PMDAO
            r_oDatabase = New dPMDAO.Database

            Return r_oDatabase.OpenDatabase(sSiriusUsername:=v_sUsername, iSourceID:=v_iSourceID, iLanguageID:=v_iLanguageID, sCallingAppName:=ACApp)
        Catch
        End Try
        result = gPMConstants.PMEReturnCode.PMError
        r_oDatabase = Nothing
        ' Log Error.
        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
        oDict.Add("v_iSourceID", v_iSourceID)
        oDict.Add("v_iLanguageID", v_iLanguageID)
        gPMFunctions.LogMessageToFile(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create a new instance of PMDAO for DSN - " & sDSN, vApp:=ACApp, vClass:=ACClass, vMethod:="NewDatabase", excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
        Return result
    End Function
    Public Function NewDatabase(ByVal v_sUsername As String, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_lPMProductFamily As Integer, ByRef r_oDatabase As Object) As Integer
        Dim oDatabase As dPMDAO.Database
        Dim lReturnCode As Integer

        oDatabase = CType(r_oDatabase, dPMDAO.Database)
        lReturnCode = NewDatabase(v_sUsername, v_iSourceID, v_iLanguageID, v_lPMProductFamily, oDatabase)
        r_oDatabase = CType(oDatabase, Object)
        Return lReturnCode
    End Function
    ' ***************************************************************** '
    ' Name: CreateBusinessObject
    '
    ' Description: Creates an instance of the class name passed.
    '
    ' ***************************************************************** '
    Public Function CreateBusinessObject(ByRef r_oObject As Object, ByVal v_sClassName As String, ByVal v_sCallingAppName As String, ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, Optional ByVal v_oDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturnCode As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object.
            ' NOTE: Because of passing in the class name,
            ' the object can ONLY be created late bound.
            r_oObject = CreateLateBoundObject(v_sClassName)

            ' Do we have a valid Database ref to pass
            If v_oDatabase Is Nothing Then
                ' No, so Initialise without

                'Modified by Deepak Sharma on 5/4/2010 4:27:02 PM refer developer guide no. 46(Guide)
                'lReturnCode = CType(r_oObject, SSP.S4I.Interfaces.IBusiness).Initialise(sUserName:=v_sUsername, sPassword:=v_sPassword, iUserID:=v_iUserID, iSourceID:=v_iSourceID, iLanguageID:=v_iLanguageID, iCurrencyID:=v_iCurrencyID, iLogLevel:=v_iLogLevel, sCallingAppName:=v_sCallingAppName)
                lReturnCode = r_oObject.Initialise(sUserName:=v_sUsername, sPassword:=v_sPassword, iUserID:=v_iUserID, iSourceID:=v_iSourceID, iLanguageID:=v_iLanguageID, iCurrencyID:=v_iCurrencyID, iLogLevel:=v_iLogLevel, sCallingAppName:=v_sCallingAppName)

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

            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_iUserID", v_iUserID)
            oDict.Add("v_iSourceID", v_iSourceID)
            oDict.Add("v_iLanguageID", v_iLanguageID)
            oDict.Add("v_iCurrencyID", v_iCurrencyID)
            gPMFunctions.LogMessageToFile(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object instance (" & v_sClassName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObject", excep:=excep, oDicParms:=oDict)

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
            gPMFunctions.LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserProperty", excep:=excep)

            Return result

        End Try
    End Function
    Public Function NewUpdateUserProperty(ByVal v_sUsername As String, ByVal v_sPropertyName As String, ByVal v_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(NewUpdateProperty(v_sGroupName:=v_sUsername, v_sPropertyName:=v_sPropertyName, v_vPropertyValue:=v_vPropertyValue), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUserProperty", excep:=excep)

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
            gPMFunctions.LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserProperty", excep:=excep)

            Return result

        End Try
    End Function
    Public Function NewGetUserProperty(ByVal v_sUsername As String, ByVal v_sPropertyName As String, ByRef r_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(NewGetProperty(v_sGroupName:=v_sUsername, v_sPropertyName:=v_sPropertyName, r_vPropertyValue:=r_vPropertyValue), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserProperty", excep:=excep)

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
            gPMFunctions.LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteUserProperty", excep:=excep)

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
            gPMFunctions.LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete All Properties for User - " & v_sUsername, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllUserProperties", excep:=excep)

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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateProductProperty", excep:=excep)

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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductProperty", excep:=excep)

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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteProductProperty", excep:=excep)

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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete All Properites for Group - " & sGroupName, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllProductProperties", excep:=excep)

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
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lSourceID", v_lSourceID)
                oDict.Add("v_lKeyID", v_lKeyID)
                oDict.Add("r_lCombinedKeyID", r_lCombinedKeyID)
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="SourceID must be greater than zero.", vApp:=ACApp, vClass:=ACClass, vMethod:="CalcCombinedKey", oDicParms:=oDict)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SJP 03/06/2002
            '   This will equal what is passed in.
            r_lCombinedKeyID = r_lCombinedKeyID

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lSourceID", v_lSourceID)
            oDict.Add("v_lKeyID", v_lKeyID)
            oDict.Add("r_lCombinedKeyID", r_lCombinedKeyID)
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to calculate a combined key for SourceID - " & v_lSourceID & _
                                          " KeyID - " & CStr(v_lKeyID), vApp:=ACApp, vClass:=ACClass, vMethod:="CalcCombinedKey", excep:=excep, oDicParms:=oDict)

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
        Dim oDatabase As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL As String = ""
        Dim sPMProductCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an Instance of PMDAO Pointing to the Sirius Architecture.
            lReturn = CType(NewDatabase(v_sUsername:=v_sUsername, v_iSourceID:=v_iSourceID, v_iLanguageID:=v_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Product Code for this Product Family
            sPMProductCode = gPMConstants.PMProductCode(v_lPMProductFamily)

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
                r_bInstalled = Not (.Records.Count() < 1)

            End With

            lReturn = oDatabase.CloseDatabase()

            oDatabase = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_iSourceID", v_iSourceID)
            oDict.Add("v_iLanguageID", v_iLanguageID)
            gPMFunctions.LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check Product Installation for Product Code - " & sPMProductCode, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPMProductInstalled", excep:=excep, oDicParms:=oDict)

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


            ' developer guide no. 
            'lStatus = oDatabase.Records.Fields("sys_admin_count").Value
            lStatus = oDatabase.Records.Fields("sys_admin_count")

            If bDBOpened Then

                oDatabase.CloseDatabase()
                oDatabase = Nothing
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_iUserID", v_iUserID)
            oDict.Add("v_iSourceID", v_iSourceID)
            oDict.Add("v_iLanguageID", v_iLanguageID)
            gPMFunctions.LogMessageToFile(sUserName:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSysAdminAccessStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSysAdminAccessStatus", excep:=excep, oDicParms:=oDict)

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
        Dim oPM As Object
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        oPM = CreateLateBoundObject("bPMPropertyManager.BusinessXML")


        'lReturn = CType(oPM, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        lReturn = oPM.Initialise()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = oPM.UpdateProperty(v_sGroupName:=v_sGroupName, v_sPropertyName:=v_sPropertyName, v_vPropertyValue:=v_vPropertyValue)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oPM.Dispose()

        oPM = Nothing

        Return result

    End Function
    Private Function NewUpdateProperty(ByVal v_sGroupName As String, ByVal v_sPropertyName As String, ByVal v_vPropertyValue As Object) As Integer
        Dim result As Integer = 0
        Dim sKey As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' RFC280802 - Remove Spaces from names
        v_sGroupName = v_sGroupName.Replace(" ", "").Trim().ToUpper()
        v_sPropertyName = v_sPropertyName.Replace(" ", "").Trim().ToUpper()

        'oCache = New VariantCacheLib.Cache()
        sKey = v_sGroupName & ACKeyDelimiter & v_sPropertyName
        'oCache.Remove(sKey)
        If oCache Is Nothing Then
            oCache = New Hashtable()
        End If
        If Not oCache.ContainsKey(sKey) Then
            oCache.Add(sKey, v_vPropertyValue)
        End If


        'Store property in Cache List if it is not already present


        ''vCacheKeys = oCache.Item(ACCacheKeys)
        'If Not Information.IsArray(vCacheKeys) Then
        '    ReDim vCacheKeys(0)


        'Else

        '    lArraySize = vCacheKeys.GetUpperBound(0)
        '    For lCount As Integer = 0 To lArraySize

        '        If CStr(vCacheKeys(lCount)) = sKey Then
        '            bFound = True
        '            Exit For
        '        End If
        '    Next lCount
        '    If Not bFound Then
        '        lArraySize += 1
        '        ReDim Preserve vCacheKeys(lArraySize)


        '    End If

        ''Store Cache List
        'oCache.Add(ACCacheKeys, vCacheKeys)

        'oCache = Nothing

        Return result

    End Function

    Public Function NewDeleteUserProperty(ByVal v_sUsername As String, ByVal v_sPropertyName As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(NewDeleteProperty(v_sGroupName:=v_sUsername, v_sPropertyName:=v_sPropertyName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Property Named - " & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteUserProperty", excep:=excep)

            Return result

        End Try
    End Function

    Public Function NewDeleteAllUserProperties(ByVal v_sUsername As String) As Integer

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
            gPMFunctions.LogMessageToFile(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete All Properties for User - " & v_sUsername, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllUserProperties", excep:=excep)

            Return result

        End Try
    End Function
    Public Function NewDeleteProperty(ByVal v_sGroupName As String, ByVal v_sPropertyName As String) As Integer
        Dim result As Integer = 0

        'Dim oCache As Hashtable
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RFC280802 - Remove Spaces from names
            v_sGroupName = v_sGroupName.Replace(" ", "").Trim().ToUpper()
            v_sPropertyName = v_sPropertyName.Replace(" ", "").Trim().ToUpper()

            'Remove the property from the cache

            If oCache Is Nothing Then
                oCache = New Hashtable()
            End If
            sKey = v_sGroupName & ACKeyDelimiter & v_sPropertyName
            If oCache.ContainsKey(sKey) Then
                oCache.Remove(sKey)
            End If


            'Update the Cache List


            'vCacheKeys = oCache.Item(ACCacheKeys)
            'If Information.IsArray(vCacheKeys) Then
            '    'Is the Property to be deleted in the cache

            '    lOldArraySize = vCacheKeys.GetUpperBound(0)
            '    For lCount As Integer = 0 To lOldArraySize

            '        If CStr(vCacheKeys(lCount)) = sKey Then
            '            bFound = True
            '            Exit For
            '        End If
            '    Next lCount

            '    'Remove the property from the Cache List if it exists
            '    If bFound Then

            '        lOldArraySize = vCacheKeys.GetUpperBound(0)

            '        lNewArraySize = lOldArraySize - 1
            '        If lNewArraySize > -1 Then
            '            ReDim vNewCacheKeys(lNewArraySize)
            '        End If

            '        lRow = 0
            '        For lCount As Integer = 0 To lOldArraySize
            '            'Copy all other properties to the new Cache List

            '            If CStr(vCacheKeys(lCount)) <> sKey Then



            '                lRow += 1
            '            End If
            '        Next lCount

            '        'Store the updated Cache List
            '        oCache.Remove(ACCacheKeys)
            '        If Information.IsArray(vNewCacheKeys) Then
            '            oCache.Add(ACCacheKeys, vNewCacheKeys)
            '        End If
            '    End If


            'oCache = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            'gPMFunctions.LogMessageToFile(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Property Value : " & v_sGroupName & "." & v_sPropertyName, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteProperty", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperty
    '
    ' Description: Gets a Property from Property Manager
    '
    ' ***************************************************************** '
    Private Function GetProperty(ByVal v_sGroupName As String, ByVal v_sPropertyName As String, ByRef r_vPropertyValue As Object) As Integer

        Dim result As Integer = 0
        Dim oPM As Object
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        oPM = CreateLateBoundObject("bPMPropertyManager.BusinessXML")

        'lReturn = CType(oPM, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        lReturn = oPM.Initialise()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = oPM.GetProperty(v_sGroupName:=v_sGroupName, v_sPropertyName:=v_sPropertyName, r_vPropertyValue:=r_vPropertyValue)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oPM.Dispose()

        oPM = Nothing

        Return result

    End Function
    Private Function NewGetProperty(ByVal v_sGroupName As String, ByVal v_sPropertyName As String, ByRef r_vPropertyValue As Object) As Integer
        Dim result As Integer = 0
        Dim sKey As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' RFC280802 - Remove Spaces from names
        v_sGroupName = v_sGroupName.Replace(" ", "").Trim().ToUpper()
        v_sPropertyName = v_sPropertyName.Replace(" ", "").Trim().ToUpper()

        'Retrieve the property from the Cache
        'oCache = New VariantCacheLib.Cache()
        sKey = v_sGroupName & ACKeyDelimiter & v_sPropertyName

        If Not Object.Equals(oCache.Item(sKey), Nothing) Then
            If Information.IsReference(oCache.Item(sKey)) Then
                r_vPropertyValue = oCache.Item(sKey)
            Else


                r_vPropertyValue = oCache.Item(sKey)
            End If
        End If
        ' oCache = Nothing

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
        Dim oPM As Object
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        oPM = CreateLateBoundObject("bPMPropertyManager.BusinessXML")

        'lReturn = CType(oPM, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        lReturn = oPM.Initialise()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = oPM.DeleteProperty(v_sGroupName:=v_sGroupName, v_sPropertyName:=v_sPropertyName)
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
        Dim oPM As Object
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        oPM = CreateLateBoundObject("bPMPropertyManager.BusinessXML")

        'Modified by Archana Tokas on 5/19/2010 12:29:33 PM refer developer guide no. 9
        lReturn = oPM.Initialise()
        'lReturn = CType(oPM, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = oPM.DeleteGroup(v_sGroupName:=v_sGroupName)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oPM.Dispose()
        oPM = Nothing

        Return result

    End Function

End Module

