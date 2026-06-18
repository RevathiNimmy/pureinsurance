Imports System.Linq
Imports SSP.PureInsuranceRestAPIHandler
Imports SSP.PureInsuranceRestAPIHandler.BaseClasses
Imports SSP.Shared

Public NotInheritable Class DocGenWorker
#Region "Fields"
    Private m_iThreadNumber As Integer
    Private m_oProcessingQueue As Queue
    Private m_sSAMURL As String
    Private m_sSAMUsername As String
    Private m_lTimeOut As Integer
    Private m_sSAMPassword As String
    Private m_sBranchCode As String
    Private m_oDatabase As Object
    Private m_sClientId As String
    Private m_sTenantId As String
    Private m_sTokenUrl As String
#End Region

#Region "Creator"
    Public Sub New(ByVal iThreadNumber As Integer, ByRef oProcessingQueue As Queue, ByVal sSAMURL As String,
                   ByVal sSAMUsername As String, ByVal sSAMPassword As String, ByVal sBranchCode As String, ByVal lTimeOut As Integer,
                  ByVal sClientId As String, ByVal sTenantId As String, ByVal sTokenUrl As String)
        m_iThreadNumber = iThreadNumber
        m_oProcessingQueue = oProcessingQueue
        m_sSAMURL = sSAMURL
        m_sSAMUsername = sSAMUsername
        m_sSAMPassword = sSAMPassword
        m_sBranchCode = sBranchCode
        m_lTimeOut = lTimeOut
        m_sClientId = sClientId
        m_sTenantId = sTenantId
        m_sTokenUrl = sTokenUrl
    End Sub

#End Region

#Region "Public Methods"
    Public Sub Start()
        Dim currentitem As DocGenItem
        'Get Items from queue until none left
        While True

            'Use synclock for thread safety on queue retrieval
            SyncLock m_oProcessingQueue.SyncRoot
                'Check Queue length when in lock. If nothing left exit loop
                If m_oProcessingQueue.Count = 0 Then
                    Exit While
                End If

                currentitem = CType(m_oProcessingQueue.Dequeue, DocGenItem)

            End SyncLock

            ProcessItem(currentitem)
        End While

    End Sub
#End Region
    Private Function GetApiTokendetails() As TokenModel
        Dim apiTokenDetails As TokenModel = New TokenModel()
        apiTokenDetails = GenerateToken.GetJwtTokenForBatchProcess(m_sClientId, m_sTokenUrl)
        Dim address As String = m_sSAMURL
        If address.EndsWith("/") Then
            address = address.Substring(0, address.Length - 1)
        End If
        apiTokenDetails.ApiBaseUrl = address
        apiTokenDetails.TokenUrl = m_sTokenUrl
        Return apiTokenDetails
    End Function
    Private Sub ProcessItem(ByVal oCurrentItem As DocGenItem)
        Dim FaliureMessage As String = ""

        Dim oRequest As New GenerateDocumentCommand
        Dim oResponse As New GenerateDocumentCommandResponse
        Try
            oRequest.BranchCode = m_sBranchCode
            oRequest.DocumentTemplateCode = oCurrentItem.DocumentCode
            oRequest.PartyKey = oCurrentItem.PartyKey
            oRequest.InsuranceFileKey = oCurrentItem.InsuranceFileKey
            oRequest.InsuranceFolderKey = oCurrentItem.InsuranceFolderKey
            oRequest.Mode = 0
            oRequest.OutputAsHTML = True
            oRequest.OutputAsPDF = False
            oRequest.LoginUserName = m_sSAMUsername
            ApiClient._tokenModel = GetApiTokendetails()
            oResponse = ApiClient.DeserializeJson(Of GenerateDocumentCommandResponse)(CStr(ApiClient.Post($"/core/documents/generate", oRequest))) 'RunBatchRenewal(request)

            If Not oResponse.Errors Is Nothing Then
                Dim oError As SAMErrors
                For Each oError In oResponse.Errors
                    FaliureMessage = FaliureMessage & oError.ToString & vbNewLine
                Next
            End If
        Catch ex As Exception
            If Not oResponse.Errors Is Nothing Then
                Dim oError As SAMErrors
                For Each oError In oResponse.Errors
                    FaliureMessage = FaliureMessage & oError.ToString & vbNewLine
                Next
            Else
                FaliureMessage = "Thread " & m_iThreadNumber & " – " & ex.Message
            End If

        End Try
        SetBatchItemStatus(oCurrentItem, FaliureMessage)

    End Sub


    Private Sub SetBatchItemStatus(ByVal oCurrentItem As DocGenItem, ByVal v_sFaliureMessage As String)
        Dim iReturn As Integer

        'Connect to DB
        DBConnect(m_oDatabase)

        AddParameterLite(m_oDatabase, "batch_notification_item_id", oCurrentItem.BatchNotificationId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "failure_text", v_sFaliureMessage, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        iReturn = m_oDatabase.SQLAction("spu_BatchNotification_Batch_Item_Status_upd", "Batch Item Update", True)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_BatchNotification_Batch_Item_Status_upd'")
        End If

        DBDisconnect(m_oDatabase)
    End Sub
    Public Sub DBConnect(ByRef oDatabase As Object)
        Dim iReturn As Integer

        oDatabase = bPMFunc.CreateLateBoundObject("dPMDAO.Database")

        ' Connect to database
        iReturn = oDatabase.OpenDatabase("sirius", 1, 1, ACApp)
        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to connect to Sirius database")
        End If
    End Sub

    Public Sub DBDisconnect(ByRef oDatabase As Object)
        Dim iReturn As Integer

        ' Close database if possible
        If Not oDatabase Is Nothing Then
            iReturn = oDatabase.CloseDatabase()
        End If
    End Sub

End Class
