Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports SSP.Shared
Imports System.Text
<System.Runtime.InteropServices.ProgId("LicenceAdmin_NET.LicenceAdmin")>
Public NotInheritable Class LicenceAdmin
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: LicenceAdmin
    '
    ' Date: 18 July 1996
    '
    ' Description: Main class containing all of the business methods.
    '
    ' Edit History:
    '
    'RFC231098 - Amended to look for non empty strings as well as non Null.
    'RFC080399 - Update Licence Limit added.
    'DAK261099 - Set all user in progress tasks to incomplete when resetting
    '            PMUser
    'DAK231299 - Allow for product licencing
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



    Private m_oDatabase As dPMDAO.Database

    'DAK060100
    Private m_oCategory As bPMTaskCategory.Business

    ' Constant for the methods to identify
    ' which class this is.
    Private Const ACClass As String = "LicenceAdmin"

    ' Declare an instance of the licence manager object.
    '************************************************************
    ' NOTE:
    '       This has been changed to use CreateObject due to a
    '       funny problem with the SIRIUS_NT_SRV.
    '       LicenceManager cannot be created using the early binding
    '       method and the CreateObject method has to be used.
    '************************************************************


    Const colUserName As Integer = 0
    Const colLogonTime As Integer = 2
    Const colLoggedOnAtClient As Integer = 1

    Const colLicenceLimit As Integer = 0

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    '**********************************************************************
    '
    ' Name: GetLicenceLimit
    '
    ' This function gets the licence limit from the database
    '
    '**********************************************************************

    Public Function GetLicenceLimit(ByRef r_lLicenceLimit As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String
        Dim sSystemName As String = String.Empty
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lLicenceLimit = 0

            lReturn = gPMFunctions.GetSystemName(sSystemName:=sSystemName)

            'Remove the "_sidX" if there is one.
            If sSystemName.IndexOf("_sid") + 1 Then
                'Check that after _sid is just numeric. Extra safety check.
                Dim dbNumericTemp As Double
                If Double.TryParse(Mid(sSystemName, (sSystemName.IndexOf("_sid") + 1) + 4), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    sSystemName = sSystemName.Substring(0, sSystemName.IndexOf("_sid"))
                End If
            End If

            'SQL statement to get the data
            sSQL = ""
            sSQL = sSQL & "SELECT licence_limit FROM PMSystem "
            sSQL = sSQL & "WHERE system_name = {system_name}"

            'DAK060100
            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="system_name", vValue:=CStr(sSystemName), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectLogLimit", bStoredProcedure:=False)

            If m_oDatabase.Records.Count() = 0 Then
                Return result
            End If
            'developer guide no.111
            r_lLicenceLimit = m_oDatabase.Records.Item(0).Fields()("licence_limit")

            Return result

        Catch excep As System.Exception





            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display details from database", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLicenceLimit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    '**********************************************************************
    '
    'Name:UpdatePMUser
    '
    ' Function to the loggedonat row of the table
    '
    '**********************************************************************
    Public Function UpdatePMUser(ByRef sUsername As String, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lRecords As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(UpdatePMUserTasks(sUsername), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
            End If

            ' Build SQL string

            sSQL = ""
            sSQL = sSQL & "UPDATE PMUser "
            sSQL = sSQL & "SET logged_on_at_client = Null, "
            sSQL = sSQL & "ModifiedBy = " & m_iUserID & ", "
            sSQL = sSQL & "UniqueId = '" & sUniqueId & "', "
            sSQL = sSQL & "ScreenHierarchy = '" & sScreenHierarchy & "' "
            sSQL = sSQL & "WHERE username = '" & sUsername & "'"

            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="username", vValue:=sUsername.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateUser", bStoredProcedure:=False, lRecordsAffected:=lRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception





            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update database", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePMUser", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '**********************************************************************
    '
    'Name:UpdatePMUserTasks
    '
    ' Function to the loggedonat row of the table
    '
    '**********************************************************************
    Public Function UpdatePMUserTasks(ByRef sUsername As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lRecords As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset visible tasks
            sSQL = ""
            sSQL = sSQL & "DECLARE @user_id INT "
            sSQL = sSQL & "SELECT @user_id = (SELECT user_id FROM PMUser "
            sSQL = sSQL & "WHERE username = {username}) "
            sSQL = sSQL & "UPDATE PMWrk_Task_Instance "
            sSQL = sSQL & "SET task_status = 2 "
            sSQL = sSQL & "WHERE user_id = @user_id "
            sSQL = sSQL & "AND task_status = 1 "
            sSQL = sSQL & "AND is_visible = 1"

            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="username", vValue:=sUsername.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateUserTasks", bStoredProcedure:=False, lRecordsAffected:=lRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Delete invisible tasks
            sSQL = ""
            sSQL = sSQL & "DELETE " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM pmwrk_task_inst_key" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE pmwrk_task_instance_cnt IN" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        SELECT " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            wti.pmwrk_task_instance_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        FROM pmwrk_task_instance wti" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN pmuser u" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON u.user_id = wti.user_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        WHERE wti.is_visible = 0" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        AND u.username = {username}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    )" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "DELETE " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM pmwrk_task_inst_log" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE pmwrk_task_instance_cnt IN" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        SELECT " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            wti.pmwrk_task_instance_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        FROM pmwrk_task_instance wti" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN pmuser u" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON u.user_id = wti.user_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        WHERE wti.is_visible = 0" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        AND u.username = {username}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    )" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "DELETE " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM pmwrk_task_instance" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE pmwrk_task_instance_cnt IN" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        SELECT " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            wti.pmwrk_task_instance_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        FROM pmwrk_task_instance wti" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        JOIN pmuser u" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "            ON u.user_id = wti.user_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        WHERE wti.is_visible = 0" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "        AND u.username = {username}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    )" & Strings.ChrW(13) & Strings.ChrW(10)

            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="username", vValue:=sUsername.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DeleteUserTasks", bStoredProcedure:=False, lRecordsAffected:=lRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update database", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePMUserTasks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '**********************************************************************
    '
    'Name: Selectdata
    '
    ' Funtion to get Details from the database about users logged on.
    '
    '**********************************************************************

    Public Function selectdata(ByRef r_vUserdataArray(,) As Object) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue



        ' Build SQL string

        'RFC231098 - Amended to look for non empty strings as well as non Null.
        Dim sSQL As String = ""
        sSQL = sSQL & "SELECT username, logged_on_at_client, lastlogin FROM PMUser "
        sSQL = sSQL & "WHERE logged_on_at_client <> '' OR logged_on_at_client <> NULL "
        sSQL = sSQL & "ORDER BY lastlogin"

        Dim lReturn As gPMConstants.PMEReturnCode = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectLogUser", bStoredProcedure:=False, vResultArray:=r_vUserdataArray)



        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If






        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: SelectProductData
    '
    ' Description:
    '
    ' History: 06/01/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function SelectProductData(ByVal v_sCategoryCode As String, ByRef r_vProductDataArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="category_code", vValue:=CStr(v_sCategoryCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetProductUserSQL, sSQLName:=kGetProductUserName, bStoredProcedure:=True, vResultArray:=r_vProductDataArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectProductData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectProductData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'RFC080399 - Update Licence Limit added.
    ' ***************************************************************** '
    ' Name: UpdateLicenceLimit
    '
    ' Description: Updates the PMSystem entry for this system with
    '              the new LicenceLimit/Key if they match.
    '
    ' ***************************************************************** '
    Public Function UpdateLicenceLimit(ByVal v_iNewLicenceLimit As Integer, ByVal v_sNewLicenceKey As String) As Integer

        Dim result As Integer = 0
        Dim oPMSystem As BPMSYSTEM.Business
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oPMSystem = New BPMSYSTEM.Business()

            lReturn = oPMSystem.Initialise(sUserName:="", sPassword:="", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=1, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = oPMSystem.UpdateLicenceLimit(v_sProductCode:=gPMConstants.PMProduct, v_iNewLicenceLimit:=v_iNewLicenceLimit, v_sNewLicenceKey:=v_sNewLicenceKey)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
            End If

            oPMSystem.Dispose()
            oPMSystem = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLicenceLimitFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLicenceLimit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            '    m_sUsername$ = sUserName$
            '    m_sPassword$ = sPassword$
            '    m_iUserID% = iUserID%
            '    m_sCallingAppName$ = sCallingAppName$
            '    m_iLanguageID% = iLanguageID%
            '    m_iSourceID% = iSourceID%
            '    m_iCurrencyID% = iCurrencyID%
            '    m_iLogLevel% = iLogLevel%

            ' Default LanguageID, SourceID & CallingAppName
            m_iLanguageID = 1
            m_iSourceID = 1
            m_iCurrencyID = 1
            m_sCallingAppName = "LICENCEMANAGER"
            m_iUserID = 1


            ' Load the instance of the licence
            ' manager object into memory.

            '************************************************************
            ' NOTE:
            '       This has been changed to use CreateObject due to a
            '       funny problem with the SIRIUS_NT_SRV.
            '       LicenceManager cannot be created using the early binding
            '       method and the CreateObject method has to be used.
            '************************************************************


            'open a new database

            m_oDatabase = New dPMDAO.Database()

            ' RDC 27062002 use Comp Serv to open database
            lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oCategory = New bPMTaskCategory.Business()


            lReturn = m_oCategory.Initialise(sUsername:="", sPassword:="", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=1, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_oCategory = Nothing
                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the licence manager object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If Informations.IsReference(m_oCategory) Then
                    m_oCategory.Dispose()
                    m_oCategory = Nothing
                End If
                If Informations.IsReference(m_oDatabase) Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: UpdateProductLimit
    '
    ' Description:
    '
    ' History: 23/12/1999 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateProductLimit(ByVal v_sTaskCategory As String, ByVal v_sCategoryDescription As String, ByVal v_iNewLicenceLimit As Integer, ByVal v_iIsBlockAboveLicenceLimit As Integer, ByVal v_iIsWarnAboveLicenceLimit As Integer, ByVal v_sNewLicenceKey As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sTaskCategory As String = ""
        Dim iLicenceLimit, iIsBlockAboveLicenceLimit, iIsWarnAboveLicenceLimit As Integer
        Dim sLicenceKey As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oCategory.CurrentRecord = 0

            Do
                sTaskCategory = ""
                lReturn = m_oCategory.GetNext(vCategoryCode:=sTaskCategory, vLicenceLimit:=iLicenceLimit, vLicenceKey:=sLicenceKey, vIsBlockAboveLicenceLimit:=iIsBlockAboveLicenceLimit, vIsWarnAboveLicenceLimit:=iIsWarnAboveLicenceLimit)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Do
                End If

            Loop Until (sTaskCategory = v_sTaskCategory)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue And lReturn <> gPMConstants.PMEReturnCode.PMEOF Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sTaskCategory <> v_sTaskCategory Then

                lReturn = m_oCategory.EditAdd(lRow:=m_oCategory.CurrentRecord + 1, vCategoryCode:=v_sTaskCategory, vCategoryDescription:=v_sCategoryDescription, vIsDeleted:=gPMConstants.PMEReturnCode.PMFalse, vEffectiveDate:=DateTime.Now, vLicenceLimit:=v_iNewLicenceLimit, vLicenceKey:=v_sNewLicenceKey, vIsBlockAboveLicenceLimit:=v_iIsBlockAboveLicenceLimit, vIsWarnAboveLicenceLimit:=v_iIsWarnAboveLicenceLimit, vWarnsSinceLicenceUpgrade:=0)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

            Else

                If iLicenceLimit <> v_iNewLicenceLimit Or sLicenceKey <> v_sNewLicenceKey Or iIsBlockAboveLicenceLimit <> v_iIsBlockAboveLicenceLimit Or iIsWarnAboveLicenceLimit <> v_iIsWarnAboveLicenceLimit Then

                    lReturn = m_oCategory.EditUpdate(lRow:=m_oCategory.CurrentRecord, vCategoryDescription:=v_sCategoryDescription, vLicenceLimit:=v_iNewLicenceLimit, vLicenceKey:=v_sNewLicenceKey, vIsBlockAboveLicenceLimit:=v_iIsBlockAboveLicenceLimit, vIsWarnAboveLicenceLimit:=v_iIsWarnAboveLicenceLimit, vWarnsSinceLicenceUpgrade:=0)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lReturn
                    End If

                End If

            End If

            lReturn = m_oCategory.Update()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateProductLimit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateProductLimit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ReadLicenceFile
    '
    ' Description:
    '
    ' History: 04/01/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function ReadLicenceFile(ByRef sFileName As String) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lResult = CType(ProcessSystem(sFileName), gPMConstants.PMEReturnCode)
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lResult = CType(ProcessProducts(sFileName), gPMConstants.PMEReturnCode)
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReadLicenceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReadLicenceFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetProducts
    '
    ' Description:
    '
    ' History: 06/01/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetProducts(ByRef r_vProductArray() As Object) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sProduct As String = ""
        Dim iSub As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lResult = m_oCategory.GetDetails()
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iSub = 0
            Do While lResult = gPMConstants.PMEReturnCode.PMTrue

                lResult = m_oCategory.GetNext(vCategoryCode:=sProduct)
                If lResult = gPMConstants.PMEReturnCode.PMTrue Then
                    If iSub = 0 Then
                        ReDim r_vProductArray(iSub)
                    Else
                        ReDim Preserve r_vProductArray(iSub)
                    End If
                    r_vProductArray(iSub) = sProduct
                End If

                iSub += 1

            Loop

            If lResult <> gPMConstants.PMEReturnCode.PMEOF Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProducts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProducts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetProductLimit
    '
    ' Description:
    '
    ' History: 06/01/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetProductDetails(ByVal v_sCategoryCode As String, Optional ByRef r_vCategoryDescription As Object = Nothing, Optional ByRef r_vIsDeleted As Object = Nothing, Optional ByRef r_vEffectiveDate As Object = Nothing, Optional ByRef r_vLicenceLimit As Object = Nothing, Optional ByRef r_vLicenceKey As Object = Nothing, Optional ByRef r_vIsBlockAboveLicenceLimit As Object = Nothing, Optional ByRef r_vIsWarnAboveLicenceLimit As Object = Nothing, Optional ByRef r_vWarnsSinceLicenceUpgrade As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim sCategoryCode As String = ""
        Dim lResult As gPMConstants.PMEReturnCode


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oCategory.CurrentRecord = 0

            Do

                lResult = m_oCategory.GetNext(vCategoryCode:=sCategoryCode, vCategoryDescription:=r_vCategoryDescription, vIsDeleted:=r_vIsDeleted, vEffectiveDate:=r_vEffectiveDate, vLicenceLimit:=r_vLicenceLimit, vLicenceKey:=r_vLicenceKey, vIsBlockAboveLicenceLimit:=r_vIsBlockAboveLicenceLimit, vIsWarnAboveLicenceLimit:=r_vIsWarnAboveLicenceLimit, vWarnsSinceLicenceUpgrade:=r_vWarnsSinceLicenceUpgrade)
                If sCategoryCode = v_sCategoryCode Then
                    Exit Do
                End If

            Loop While lResult = gPMConstants.PMEReturnCode.PMTrue

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    '
    ' Name: ProcessSystem
    '
    ' Description:
    '
    ' History: 04/01/2000 DAK - Created.
    ' DAK190400 - System name is noe ignored
    '
    ' ***************************************************************** '
    Private Function ProcessSystem(ByRef sFileName As String) As Integer
        Dim result As Integer = 0
        Dim lResult, lNoOfChars As Integer
        Dim sSystemName As String = String.Empty
        Dim sDefault As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the system name
        lResult = gPMFunctions.GetSystemName(sSystemName)
        If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Dim sbLicenceLimit As New StringBuilder(128) ' Allocate 128 characters for the result
        Dim keyName As String = ACSystemPrefix & gPMConstants.PMProduct

        Dim fileName As String = sFileName

        ' Call the function directly, passing the keyName as a managed string
        lNoOfChars = GetPrivateProfileString(keyName, ACLicenceLimit, sDefault, sbLicenceLimit, sbLicenceLimit.Capacity, fileName)

        ' Get the result as a string
        sbLicenceLimit.Length = lNoOfChars ' Set the length to the number of characters returned
        Dim sLicenceLimit As String = sbLicenceLimit.ToString()

        ' Get system licence limit
        If sLicenceLimit = "" Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="System Licence Limit Not Found", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSystem")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sDefault = ""
        Dim sbLicenceKey As New StringBuilder(128) ' Allocate 128 characters for the result

        ' Call the function directly, passing the keyName as a managed string
        lNoOfChars = GetPrivateProfileString(keyName, ACLicenceKey, sDefault, sbLicenceKey, sbLicenceKey.Capacity, fileName)

        ' Get the result as a string
        sbLicenceKey.Length = lNoOfChars ' Set the length to the number of characters returned
        Dim sLicenceKey As String = sbLicenceKey.ToString()

        If sLicenceKey = "" Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="System Licence Key Not Found", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSystem")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Update the system licence limit
        lResult = UpdateLicenceLimit(CInt(sLicenceLimit), sLicenceKey)
        If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessProducts
    '
    ' Description:
    '
    ' History: 04/01/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessProducts(ByRef sFileName As String) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim lNoOfChars As Integer
        Dim sDefault As String = ""
        Dim iSub As Integer
        Dim sProductCode As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Update the product licences
        iSub = 0

        Do
            iSub += 1

            Dim sbProductCode As New StringBuilder(128) ' Allocate 128 characters for the result
            ' Call the function directly, passing the keyName as a managed string
            lNoOfChars = GetPrivateProfileString(ACInstalledProducts, iSub.ToString, sDefault, sbProductCode, sbProductCode.Capacity, sFileName)

            ' Get the result as a string
            sbProductCode.Length = lNoOfChars ' Set the length to the number of characters returned
            sProductCode = sbProductCode.ToString()

            If sProductCode <> "" Then

                lResult = CType(ProcessProduct(sFileName, sProductCode), gPMConstants.PMEReturnCode)
                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

        Loop While sProductCode <> ""

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessProduct
    '
    ' Description:
    '
    ' History: 04/01/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessProduct(ByRef sFileName As String, ByRef sProductCode As String) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim lNoOfChars As Integer
        Dim sDefault, sProductDescription, sLicenceLimit As String
        Dim iLicenceLimit As Integer
        Dim sLicenceKey, sExceedLimitAction As String
        Dim iIsBlockAboveLicenceLimit As gPMConstants.PMEReturnCode
        Dim iIsWarnAboveLicenceLimit As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        'Set default key value
        sLicenceKey = ""

        ' Get product description
        Dim sbProductDescription As New StringBuilder(128) ' Allocate 128 characters for the result
        sDefault = ""
        ' Call the function directly, passing the keyName as a managed string
        lNoOfChars = GetPrivateProfileString(ACProductPrefix & sProductCode, ACProductDescription, sDefault, sbProductDescription, sbProductDescription.Capacity, sFileName)

        ' Get the result as a string
        sbProductDescription.Length = lNoOfChars ' Set the length to the number of characters returned
        sProductDescription = sbProductDescription.ToString()

        If sProductDescription = "" Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sProductCode & " Product Description Not Found", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessProduct")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get product licence limit
        Dim sbLicenceLimit As New StringBuilder(128) ' Allocate 128 characters for the result
        sDefault = ""
        ' Call the function directly, passing the keyName as a managed string
        lNoOfChars = GetPrivateProfileString(ACProductPrefix & sProductCode, ACLicenceLimit, sDefault, sbLicenceLimit, sbLicenceLimit.Capacity, sFileName)

        ' Get the result as a string
        sbLicenceLimit.Length = lNoOfChars ' Set the length to the number of characters returned
        sLicenceLimit = sbLicenceLimit.ToString()

        If sLicenceLimit = "" Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sProductCode & " Product Licence Limit Not Found", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessProduct")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        iLicenceLimit = CInt(sLicenceLimit)

        Dim sbLicenceKey As New StringBuilder(128) ' Allocate 128 characters for the result
        sDefault = ""
        ' Call the function directly, passing the keyName as a managed string
        lNoOfChars = GetPrivateProfileString(ACProductPrefix & sProductCode, ACLicenceKey, sDefault, sbLicenceKey, sbLicenceKey.Capacity, sFileName)

        ' Get the result as a string
        sbLicenceKey.Length = lNoOfChars ' Set the length to the number of characters returned
        sLicenceKey = sbLicenceKey.ToString()


        ' Get exceed limit action
        Dim sbExceedLimitAction As New StringBuilder(128) ' Allocate 128 characters for the result
        sDefault = ""
        ' Call the function directly, passing the keyName as a managed string
        lNoOfChars = GetPrivateProfileString(ACProductPrefix & sProductCode, ACExceedLimitAction, sDefault, sbExceedLimitAction, sbExceedLimitAction.Capacity, sFileName)

        ' Get the result as a string
        sbExceedLimitAction.Length = lNoOfChars ' Set the length to the number of characters returned
        sExceedLimitAction = sbExceedLimitAction.ToString()

        If sExceedLimitAction = "" Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sProductCode & " Exceed Limit Action Not Found", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessProduct")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Select Case sExceedLimitAction
            Case ACExceedLimitBlock

                iIsBlockAboveLicenceLimit = gPMConstants.PMEReturnCode.PMTrue
                iIsWarnAboveLicenceLimit = gPMConstants.PMEReturnCode.PMFalse

            Case ACExceedLimitWarn

                iIsBlockAboveLicenceLimit = gPMConstants.PMEReturnCode.PMFalse
                iIsWarnAboveLicenceLimit = gPMConstants.PMEReturnCode.PMTrue

            Case ACExceedLimitNone

                iIsBlockAboveLicenceLimit = gPMConstants.PMEReturnCode.PMFalse
                iIsWarnAboveLicenceLimit = gPMConstants.PMEReturnCode.PMFalse

            Case Else

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sProductCode & _
                                   " Invalid Exceed Limit Action = " & _
                                       sExceedLimitAction, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessProduct")

                Return gPMConstants.PMEReturnCode.PMFalse

        End Select

        ' Update the product licence limit
        lResult = CType(UpdateProductLimit(v_sTaskCategory:=sProductCode, v_sCategoryDescription:=sProductDescription, v_iNewLicenceLimit:=iLicenceLimit, v_iIsBlockAboveLicenceLimit:=iIsBlockAboveLicenceLimit, v_iIsWarnAboveLicenceLimit:=iIsWarnAboveLicenceLimit, v_sNewLicenceKey:=sLicenceKey), gPMConstants.PMEReturnCode)
        If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' PRIVATE Methods (End)




    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

