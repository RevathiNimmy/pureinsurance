Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System

'Developer Guide No.: 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 03/01/2001
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a RI Band Version.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' ************************************************
    ' Added to replace global variables 11/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lPMAuthorityLevel As Integer

    ' Primary Keys to work with
    Private m_lRIBandId As Integer
    Private m_sRIBandCode As String
    Private m_sDescription As String



    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            Value = m_lPMAuthorityLevel
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public Property RIBandId() As Integer
        Get
            Return m_lRIBandId
        End Get
        Set(ByVal Value As Integer)
            m_lRIBandId = Value
        End Set
    End Property
    Public Property RIBandCode() As String
        Get
            Return m_sRIBandCode
        End Get
        Set(ByVal Value As String)
            m_sRIBandCode = Value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


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

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function GetRIBandVersion(ByRef r_vRIBandVersion(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRIBandVersion"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_band_id", m_lRIBandId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Call procedure
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRIBandSQL, sSQLName:=ACSelectRIBandName, bStoredProcedure:=ACSelectRIBandStored, vResultArray:=r_vRIBandVersion)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect()", "Command = " & ACSelectRIBandSQL)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here
            '		Return result

            ' This is for debugging only
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function


    Public Function Update(ByVal v_vRIBands(,) As Object, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Update"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bTransaction As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin a transaction for this process
            lReturn = m_oDatabase.SQLBeginTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans()", "Begin Transaction Failed")
            End If

            bTransaction = True

            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_band_id", m_lRIBandId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "userid", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", v_sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", v_sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRIBandSQL, sSQLName:=ACDeleteRIBandName, bStoredProcedure:=ACDeleteRIBandStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to execute: " & ACDeleteRIBandSQL)
            End If


            ' Delete current ri bands
            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRIBandSQL, sSQLName:=ACDeleteRIBandName, bStoredProcedure:=ACDeleteRIBandStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect()", "Command = " & ACDeleteRIBandSQL)
            End If

            ' Only add new records if we have a valid array
            If Information.IsArray(v_vRIBands) Then
                ' Process all the ri band values
                For lCount As Integer = v_vRIBands.GetLowerBound(1) To v_vRIBands.GetUpperBound(1)
                    ' Check for valid code

                    ' If Strings.Len(CStr(v_vRIBands(ACRCode, lCount))) Then
                    If v_vRIBands(ACRRIBandId, lCount) <> 0 Then
                        ' Add parameters
                        bPMAddParameter.AddParameterLite(m_oDatabase, "ri_band_id", m_lRIBandId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "code", m_sRIBandCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                        ' bPMAddParameter.AddParameterLite(m_oDatabase, "caption_id", v_vRIBands(ACRCaptionId, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "description", m_sDescription, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "effective_date", v_vRIBands(ACREffectiveDate, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                        'bPMAddParameter.AddParameterLite(m_oDatabase, "is_deleted", v_vRIBands(ACRIsDeleted, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "Date_for_Treaty_XOL_Calculation_id", v_vRIBands(ACRDateForTreatyID, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "XOL_Treaty_To_Recover_From_id", v_vRIBands(ACRXOLID, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "Proportional_RI_Cal_Method", v_vRIBands(ACRPRICALID, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "use_anniversary_date_for_TMP", v_vRIBands(ACRUseAnniversaryDate, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


                        bPMAddParameter.AddParameterLite(m_oDatabase, "userid", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", v_sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", v_sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                        lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertRIBandVersionSQL, sSQLName:=ACInsertRIBandVersionName, bStoredProcedure:=ACInsertRIBandVersionStored)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("m_oDatabase.SQLSelect()", "Command = " & ACInsertRIBandVersionSQL)
                        End If
                    End If
                Next lCount
            End If

            ' If we have a transaction commit it
            If bTransaction Then
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans()", "Commit Transaction Failed")
                End If

                bTransaction = False
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            If bTransaction Then
                m_oDatabase.SQLRollbackTrans()
                bTransaction = False
            End If

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here
            '		Return result

            ' This is for debugging only
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function



    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



End Class
