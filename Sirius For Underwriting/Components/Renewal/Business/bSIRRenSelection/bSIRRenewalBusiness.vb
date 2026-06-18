Imports SSP.Shared
Public NotInheritable Class bSIRRenewalBusiness
    Implements IDisposable

    Private Const ACClass As String = "BatchRenewalBusiness"
    Private m_oDatabase As dPMDAO.Database
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private m_bCloseDatabase As Boolean
    Private m_lReturn As Integer
    Private m_sErrorString As String = ""
    Private disposedValue As Boolean

    ''' <summary>
    ''' Initialise
    ''' </summary>
    ''' <param name="sUsername"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="iUserID"></param>
    ''' <param name="iSourceID"></param>
    ''' <param name="iLanguageID"></param>
    ''' <param name="iCurrencyID"></param>
    ''' <param name="iLogLevel"></param>
    ''' <param name="sCallingAppName"></param>
    ''' <param name="bStandAlone"></param>
    ''' <param name="vDatabase"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=Nothing, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch excep As System.Exception
            m_lReturn = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BatchRenewalBusiness Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return m_lReturn
        End Try
        Return m_lReturn
    End Function

    ''' <summary>
    ''' Dispose
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    ''' <summary>
    ''' Dispose
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
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
    ''' <summary>
    ''' HasInstalmentPlanOnCurrentTerm
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="sPaymentMethod"></param>
    ''' <param name="nLatestInstalmentInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function HasInstalmentPlanOnCurrentTerm(ByVal nInsuranceFileCnt As Integer, ByRef sPaymentMethod As String, ByRef nLatestInstalmentInsuranceFileCnt As Integer) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Const kMethodName As String = "HasInstalmentPlanOnCurrentTerm"
        Dim aoResultArray(,) As Object = Nothing
        Try

            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(nInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            nResult = m_oDatabase.SQLSelect(sSQL:=kHasInstalmentPlanOnCurrentTermSQL, sSQLName:=kHasInstalmentPlanOnCurrentTermName, bStoredProcedure:=True, vResultArray:=aoResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(aoResultArray) Then
                Throw New Exception
            Else
                sPaymentMethod = gPMFunctions.ToSafeString(aoResultArray(0, 0), "")
                nLatestInstalmentInsuranceFileCnt = gPMFunctions.ToSafeInteger(aoResultArray(1, 0), 0)
            End If

        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

        End Try
        Return nResult
    End Function

End Class


