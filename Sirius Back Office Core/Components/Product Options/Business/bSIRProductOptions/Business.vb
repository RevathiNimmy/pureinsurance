Option Strict Off
Option Explicit On
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: bSIRProductOptions
    '
    ' Date: 6th June 2002
    '
    ' Description: Business objects to retrieve product option
    '
    ' Edit History:
    '   11062002 SJP - Created
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 22/01/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    Private Const ACClass As String = "Business"
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Public Shared iCache As ICacheManager
    Private m_sCachePath As String
    Private m_sCacheFilename As String

    Private m_oDatabase As dPMDAO.Database
    Private m_lReturn As Integer


    Const sProductOption As String = "productoptions"
    '****************************************************************************
    ' Name: CreateUniqueNumberForClientCode
    '
    ' Description: Calls the stored procedure that checks if a Unique Number
    '              record exists for Client Code, and if not creates one with
    '              the passed start number as the seed.
    '
    ' History: PW190303 - created (PS186)
    '***************************************************************************
    Public Function CreateUniqueNumberForClientCode(ByVal lClientStartNumber As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' This gets the database connection (as per the workings of this component)
            If getDatabaseConnection() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Clear the parameter collection
            m_oDatabase.Parameters.Clear()

            ' Add the start number parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Start_Number", vValue:=CStr(lClientStartNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure

            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_unique_number_create", sSQLName:="Create Unique Number", bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CreateUniqueNumberForClientCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateUniqueNumberForClientCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function








    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 06/06/2002 SJP - Created.
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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 06/06/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 06/06/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' BSJ 09/08/04

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            Try
                iCache = CacheFactory.GetCacheManager("PureCache")
            Catch ex As Exception

            End Try

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=m_sCachePath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Right(m_sCachePath, 1) <> "\" Then
                m_sCachePath += "\"
            End If

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeySystemOptionCacheFileName, r_sSettingValue:=m_sCacheFilename)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: getAllHiddenOptions
    '
    ' Description: This is the public interface to populate
    '               an array of all values
    '
    ' History: 06/06/2002 SJP - Created.
    '
    '*******************************************************'
    Public Function getAllHiddenOptions(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            '   This will return a list of all options
            result = getAllProductOptions(r_vResultArray, "", False)

            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getAllHiddenOptionsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="getAllHiddenOptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: getMasterOptions
    '
    ' Description: This will return the Master Categories List
    '               populated with option values from database
    '               if present
    '
    ' History: 07/06/2002 SJP - Created.
    '
    '*******************************************************'
    Public Function getMasterOptions(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vArray(,) As Object = Nothing

        Try

            '   This will find all values on the array

            lReturn = CType(getAllProductOptions(vArray, CStr(gPMConstants.SIRBCHHeadOffice), False), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            '   This will copy all Master Options (apart from U/W option)
            gSIRLibrary.populateArray()
            r_vResultArray = (gSIRLibrary.SIROPTMasterOptions).Clone()

            '   Need to find if existing database values for values
            '   and replace default value with this if possible


            'vArray = Nothing

            Return findAndCopyArrayValue(vArray, r_vResultArray, 2)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getMasterOptions failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getMasterOptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: updateMasterOptions
    '
    ' Description: This will update the Master Options and
    '               delete any options that are no longer
    '               required
    '
    ' History: 07/06/2002 SJP - Created.
    '
    '*******************************************************'
    Public Function updateMasterOptions(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim bDeleteHeadOfficeOnly As Boolean

        Try

            If getDatabaseConnection() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.SQLBeginTrans()

            '   This will loop through all options and delete all product options
            '   from the database where the product option is not required
            For i As Integer = 0 To r_vResultArray.GetUpperBound(1)
                bDeleteHeadOfficeOnly = False

                If (CStr(r_vResultArray(2, i))) <> "" Then
                    bDeleteHeadOfficeOnly = True
                End If


                lResult = deleteOptions(CStr(r_vResultArray(0, i)), False, bDeleteHeadOfficeOnly)

                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_oDatabase.SQLRollbackTrans()
                    Return lResult
                End If
            Next i

            '   This will update the master Category
            lResult = CType(insertOptions(r_vResultArray, True), gPMConstants.PMEReturnCode)

            '   Commit transaction if successful
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lResult
                m_oDatabase.SQLRollbackTrans()
            Else
                result = gPMConstants.PMEReturnCode.PMTrue
                m_oDatabase.SQLCommitTrans()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getMasterOptions failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getMasterOptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: getProductOptionsWithDesc
    '
    ' Description: This will return the Product Options with
    '               Description for all values (excluding Option 1)
    '
    ' History: 07/06/2002 SJP - Created.
    '
    '*******************************************************'
    Public Function getProductOptionsWithDesc(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim vDescArray(,) As Object

        Try

            '   This will retrieve all rows except Option Number 1
            lResult = CType(getAllProductOptions(r_vResultArray, "", True), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            '   Populate masterCategory
            gSIRLibrary.populateArray()
            vDescArray = (gSIRLibrary.SIROPTMasterOptions).Clone()

            '   It will then fill in the descriptions for each Options
            lResult = CType(findAndCopyArrayValue(vDescArray, r_vResultArray, 4), gPMConstants.PMEReturnCode)
            lResult = CType(findAndCopyArrayValue(vDescArray, r_vResultArray, 5), gPMConstants.PMEReturnCode)


            Return lResult

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionsWithDesc failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getProductOptionsWithDesc", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: updateProductOptions
    '
    ' Description: This will update all Product Options in
    '               Array.  It will do this by performing a
    '               delete and then an insert
    '
    ' History: 07/06/2002 SJP - Created.
    '
    '*******************************************************'
    Public Function updateProductOptions(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            If getDatabaseConnection() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.SQLBeginTrans()

            '   This will loop through all options and delete all product options
            '   from the database where the product option is not required
            lResult = deleteOptions("", True, False)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            '   This will update the master Category
            lResult = CType(insertOptions(r_vResultArray, False), gPMConstants.PMEReturnCode)

            '   Commit transaction if successful
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_oDatabase.SQLRollbackTrans()
            Else
                result = gPMConstants.PMEReturnCode.PMTrue
                m_oDatabase.SQLCommitTrans()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateProductOptions failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateProductOptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: getBranches
    '
    ' Description: This will return a list of Branch code and description
    '
    ' History: 07/06/2002 SJP - Created.
    '
    '*******************************************************'
    Public Function getBranches(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            If getDatabaseConnection() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            '   Retrieve values into array
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACFindBranchesSQL, sSQLName:=ACFindBranchesName, bStoredProcedure:=ACFindBranchesProc, vResultArray:=r_vResultArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            lResult = m_oDatabase.CloseDatabase()
            m_oDatabase = Nothing

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            '   Checks whether values were found
            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBranches Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranches", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: IsUserAdmin
    '
    ' Description: This will determine if user is a admin user
    '
    ' History: 07/06/2002 SJP - Created.
    '          21/06/2002 SP - modified to constant values in BAS file
    '*******************************************************'
    Public Function isUserAdmin(ByRef v_vUserId As Object, ByRef r_bAdminUser As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            r_bAdminUser = False

            '   This gets the database connection
            If getDatabaseConnection() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            With m_oDatabase

                .Parameters.Clear()

                lReturn = .Parameters.Add(sName:=ACUser_id, vValue:=CStr(v_vUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                lReturn = .Parameters.Add(sName:=ACEffective_date, vValue:=DateTime.Today, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                ' Execute SQL Statement
                lReturn = .SQLSelect(sSQL:=ACGetUserIsAdminSQL, sSQLName:=ACGetUserIsAdminName, bStoredProcedure:=ACGetUserIsAdminProc, lNumberRecords:=0, vResultArray:=vResultArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    lReturn = m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                    Return result
                End If

            End With

            lReturn = m_oDatabase.CloseDatabase()
            m_oDatabase = Nothing

            '   This will check the count returned
            '   1 = user Admin  , 0 = User not Admin
            If Informations.IsArray(vResultArray) Then

                If CDbl(vResultArray(0, 0)) > 0 Then
                    r_bAdminUser = True
                End If
            End If

            vResultArray = Nothing

            Return lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsUserAdmin failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUserAdmin", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: getICCSNumber
    '
    ' Description: This will get the ICCS Number from the database
    '
    ' History: 07/06/2002 SJP - Created.
    '          21/06/2002 SP - modified to constant values in BAS file
    '*******************************************************'
    Public Function getICCSCode(ByRef r_vICCSCode As String) As Integer

        Dim result As Integer = 0
        Dim sICSSCode As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            '   This gets the database connection
            If getDatabaseConnection() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '   This sorts out the database query
            With m_oDatabase
                .Parameters.Clear()
                lReturn = .Parameters.Add(sName:=ACICCS, vValue:=sICSSCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
                lReturn = .SQLAction(sSQL:=ACGetICCSSQL, sSQLName:=ACGetICCSName, bStoredProcedure:=ACGetICCSProc)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the iccs code
                sICSSCode = .Parameters.Item(ACICCS).Value

            End With

            lReturn = m_oDatabase.CloseDatabase()
            m_oDatabase = Nothing

            r_vICCSCode = sICSSCode.Trim()

            Return lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getICCSCode failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getICCSCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ******************************************************'
    '
    ' Name: deleteOptions
    '
    ' Description: This will delete all options with key > 1
    '               or a specific option number.
    '               It will always be called within a transaction
    '
    ' History: 07/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Function deleteOptions(ByVal v_vOptionNumber As String, ByVal v_bDeleteAll As Boolean, ByVal v_bDeleteHeadOfficeOnly As Boolean) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim iBranchId As String = ""
        Dim iOptionNumber, iDeleteOption As Integer
        Dim lReturn As gPMConstants.PMEReturnCode



        '   Sanity check - make sure we can't delete Option No 1
        If ToSafeDouble(v_vOptionNumber) = 1 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        iOptionNumber = 0
        iBranchId = CStr(0)
        iDeleteOption = 0

        '   This will sort out what the where clause of the
        '   delete statement will be.
        If v_bDeleteAll Then
            iDeleteOption = 1
        ElseIf v_bDeleteHeadOfficeOnly Then
            iOptionNumber = CInt(v_vOptionNumber)
            iBranchId = CStr(gPMConstants.SIRBCHHeadOffice)
            iDeleteOption = 2
        Else
            iOptionNumber = CInt(v_vOptionNumber)
            iDeleteOption = 3
        End If

        '   This will call the stored procedure.
        With m_oDatabase
            .Parameters.Clear()

            lReturn = .Parameters.Add(sName:=ACOption_number, vValue:=CStr(iOptionNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            lReturn = .Parameters.Add(sName:=ACBranch_id, vValue:=iBranchId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            lReturn = .Parameters.Add(sName:=ACDelete_option, vValue:=CStr(iDeleteOption), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            result = m_oDatabase.SQLAction(sSQL:=ACDeleteOptionsSQL, sSQLName:=ACDeleteOptionsName, bStoredProcedure:=ACDeleteOptionsProc)

        End With

        Return result

    End Function

    ' ******************************************************'
    '
    ' Name: insertOptions
    '
    ' Description: This will populate all options where
    '               value of array is not "" or null
    '
    ' History: 06/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Function insertOptions(ByRef r_vResultArray(,) As Object, ByVal v_bMasterOptionsIns As Boolean) As Integer

        Dim result As Integer = 0
        Dim iOptionNumber, iBranchId As Integer

        Dim lReturn As gPMConstants.PMEReturnCode



        '   This will insert all values of the array into the hidden options table
        For i As Integer = 0 To r_vResultArray.GetUpperBound(1)

            '   This will insert if NOT equal to option number 1
            '   OR there is a value
            '   Or there is a no value and this is not a Master Options Insert
            If r_vResultArray(0, i) = gPMConstants.SIRHiddenOptions.SIROPTUnderwriting Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not (CStr(r_vResultArray(2, i)) = "" And v_bMasterOptionsIns) Then

                With m_oDatabase

                    .Parameters.Clear()


                    iOptionNumber = CInt(r_vResultArray(0, i))
                    lReturn = .Parameters.Add(sName:=ACOption_number, vValue:=CStr(iOptionNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


                    iBranchId = CInt(r_vResultArray(1, i))
                    lReturn = .Parameters.Add(sName:=ACBranch_id, vValue:=CStr(iBranchId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)



                    lReturn = .Parameters.Add(sName:=ACValue, vValue:=CStr(r_vResultArray(2, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


                    lReturn = .Parameters.Add(sName:=ACUW_type, vValue:=CStr(r_vResultArray(3, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    lReturn = .SQLAction(sSQL:=ACInsertProductOptionSQL, sSQLName:=ACInsertProductOptionName, bStoredProcedure:=ACInsertProductOptionProc)

                End With
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If
        Next i


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ******************************************************'
    '
    ' Name: getDatabaseConnection
    '
    ' Description: This will return a database connection
    ' History: 06/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Function getDatabaseConnection() As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        'Open Sirius Database Connection and close the component services
        If gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

        End If


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ******************************************************'
    '
    ' Name: getAllProductOptions
    '
    ' Description: This populates an array from the database
    '
    ' History: 06/06/2002 SJP - Created.
    ' Edit History  :
    ' RAM20040304   : Code changes relates to Caching of Product Options
    '*******************************************************'
    Private Function getAllProductOptions(ByRef r_vResultArray(,) As Object,
                                          ByVal v_vBranch As String,
                                          ByVal v_bRemoveOption1 As Boolean) As Integer


        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim iOptionNumber, iBranchId As Integer

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20040304 : Code changes related to Caching - START
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim sKey As String = ""
        Dim vValue As Object = Nothing
        Dim vKeyArray As Object = Nothing
        Dim sContent(1) As String
        sContent(0) = ""
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20040304 : Code changes related to Caching - END
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''



        result = gPMConstants.PMEReturnCode.PMTrue

        iOptionNumber = 0
        iBranchId = 0

        If v_bRemoveOption1 Then
            iOptionNumber = 1
        Else
            If v_vBranch <> "" Then
                iBranchId = CInt(v_vBranch)
            End If
        End If


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20040304 : Code changes related to Caching - START
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Create key for the input parameters
        ' eg. KEY_PRODUCT_OPTION_00001_00001 :  means : Branch ID 1 Option Number 1
        sKey = "KEY_PRODUCT_OPTION_" & StringsHelper.Format(iBranchId, "00000") & "_" & StringsHelper.Format(iOptionNumber, "00000")

        ' Create the Cache Object
        ' Get from the Cache by the Key, if available
        If Not iCache Is Nothing AndAlso iCache.Contains(sKey) Then
            vValue = iCache.GetData(sKey)
            r_vResultArray = vValue
            'vValue = oCache.Item(sKey)
        Else

            ' Not in the CACHE, therefore we need to hit the database to get the value
            If Object.Equals(vValue, Nothing) Then

                If getDatabaseConnection() <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not m_oDatabase.Parameters Is Nothing Then
                    With m_oDatabase

                        .Parameters.Clear()

                        lResult = .Parameters.Add(sName:=ACFind_Option, vValue:=CStr(iOptionNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                        lResult = .Parameters.Add(sName:=ACBranch_id, vValue:=CStr(iBranchId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                        '   Retrieve values into array
                        lReturn = .SQLSelect(sSQL:=ACFindProductOptionsSQL, sSQLName:=ACFindProductOptionsName, bStoredProcedure:=ACFindProductOptionsProc, vResultArray:=r_vResultArray)

                        lResult = .CloseDatabase()

                    End With

                    m_oDatabase = Nothing

                    '   Checks whether no database problems
                    If (lResult <> gPMConstants.PMEReturnCode.PMTrue) Or (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    '   Checks whether values were found
                    If Not Informations.IsArray(r_vResultArray) Then
                        Return gPMConstants.PMEReturnCode.PMNotFound
                    End If

                    result = lReturn

                    ' Add them to the Cache for future use
                    'oCache.Add(sKey, sValue)
                    If Not FileExists(m_sCachePath + m_sCacheFilename) Then
                        Dim fileIO As FileStream
                        fileIO = File.Create(m_sCachePath + m_sCacheFilename)
                        fileIO.Close()
                        File.WriteAllLines(m_sCachePath + m_sCacheFilename, sContent)
                    End If

                    ' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
                    ' Sirius Cache Controller
                    If Not iCache Is Nothing Then
                        iCache.Add(sKey, r_vResultArray, CacheItemPriority.NotRemovable, Nothing, New FileDependency(m_sCachePath + m_sCacheFilename))
                        'vKeyArray = oCache.Item("SIRIUS_CACHE_KEYS")
                    End If

                End If

            End If
        End If

        Return result

    End Function

    ' ******************************************************'
    '
    ' Name: findAndCopyArrayValue
    '
    ' Description: This will find a value in the source array
    '               and copy the column to the destination array
    '
    ' History: 07/06/2002 SJP - Created.
    '
    '*******************************************************'
    Private Function findAndCopyArrayValue(ByVal v_vSourceArray(,) As Object, ByRef r_vDestArray(,) As Object, ByVal v_iCopyColumn As Integer) As Integer

        Dim result As Integer = 0



        '   This will look through the array for matching option numbers.
        '   If it finds these then it will copy the correct column
        For i As Integer = 0 To r_vDestArray.GetUpperBound(1)
            For j As Integer = 0 To v_vSourceArray.GetUpperBound(1)
                '   If it is found in the database


                If CInt(r_vDestArray(0, i)) = CDbl(v_vSourceArray(0, j)) Then


                    r_vDestArray(v_iCopyColumn, i) = v_vSourceArray(v_iCopyColumn, j)
                    j = v_vSourceArray.GetUpperBound(1)
                End If
            Next j
        Next i


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
    Private Shared _DefaultInstance As Business = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Business
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Business
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class

