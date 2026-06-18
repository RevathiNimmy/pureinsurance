Imports SharedFiles
Public NotInheritable Class BusinessSharepointOnlineValidate
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    Private Const ACClass As String = "BusinessSharepointOnlineValidate"
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BusinessSharepointOnlineValidate Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ''' ValidateSharePointURL
    ''' </summary>
    ''' <param name="sSharePointOnlineURL"></param>
    ''' <param name="sSharePointLibrary"></param>
    ''' <param name="sUserId"></param>
    ''' <param name="sPassword"></param>
    ''' <remarks></remarks>
    Public Sub ValidateSharePointURL(ByVal sSharePointOnlineURL As String,
                                     ByVal sSharePointLibrary As String,
                                     ByVal sUserId As String, ByVal sPassword As String, ByRef sResponse As String,
                                     ByVal sAppClientId As String, ByVal sSharepointTenantId As String)
        Try

            Dim m_oSharePoint As Object = CreateLateBoundObject("bSIRSharePointOnline.BusinessSharepointOnline")
            m_oSharePoint.Initialise(
                    sUsername:="",
                    sPassword:="",
                    iUserID:=1,
                    iSourceID:=1,
                    iLanguageID:=1,
                    iCurrencyID:=26,
                    iLogLevel:=PMELogLevel.PMLogError,
                    sCallingAppName:="bSIRSharePointValidate",
                    vDatabase:=Nothing)

            m_oSharePoint.ValidateSharepointOnlineURL(sSharepointSite:=sSharePointOnlineURL,
                                                    sSharepointLibrary:=sSharePointLibrary,
                                                    sUserName:=sUserId, sPassword:=sPassword, sResponse:=sResponse,
                                                    sAppClientId:=sAppClientId, sSharepointTenantId:=sSharepointTenantId)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

End Class

