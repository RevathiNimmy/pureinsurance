Imports System.Collections.ObjectModel
Imports System.Xml


Friend NotInheritable Class Reference_Import : Inherits ImportBase
#Region "Private Fields"
    Private m_nNoOfTotalRecords As Integer
    Private m_nNoOfRejections As Integer

#End Region
#Region "Public Properties"
    ''' <summary>
    ''' Specifies the batch code for this interface
    ''' </summary>
    Public Overrides ReadOnly Property BatchCode() As String
        Get
            Return "REFI"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the active interface name
    ''' </summary>
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Reference_Import"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the number of records in batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfTotalRecords() As Integer
        Get
            Return m_nNoOfTotalRecords
        End Get
        Set(ByVal value As Integer)
            m_nNoOfTotalRecords = value
        End Set
    End Property

    ''' <summary>
    ''' Specifies the no of rejected records in the batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfRejections() As Integer
        Get
            Return m_nNoOfRejections
        End Get
        Set(ByVal value As Integer)
            m_nNoOfRejections = value
        End Set
    End Property

#End Region

#Region "Methods"
    Protected Overrides Sub ProcessElement()
        NoOfTotalRecords += 1
        ' Get parameters
        Dim iTransDetailID As Integer = GetAttribute("transdetail_id")
        Dim sMediaRef As String = GetAttribute("media_ref")

        ' Update media reference
        UpdateMediaRef(iTransDetailID, sMediaRef)
    End Sub

    Private Sub UpdateMediaRef(ByVal iTransDetailID As Integer, ByVal sMediaRef As String)
        Dim iItemsAffected As Integer = 0
        Dim iReturn As PMEReturnCode

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "transdetail_id", iTransDetailID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "media_ref", sMediaRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "items_affected", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

            ' Execute command
            iReturn = m_oDatabase.SQLAction("spu_ACT_Import_Reference_Import", "Reference Import", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_Import_Reference_Import'")
            End If

            ' Check how many items we affected
            iItemsAffected = m_oDatabase.Parameters.Item("items_affected").Value
        Catch ex As Exception
            Throw New Exception("Unable to process reference.", ex)
        Finally
            ' Final check that we update at least 1 cashlistitem
            If iItemsAffected = 0 Then
                Throw New Exception(String.Format("Unable to process media reference '{0}'. CashListItem for transaction {1} not found.", sMediaRef, iTransDetailID))
            End If
        End Try
    End Sub
    ''' <summary>
    ''' Update batch Status
    ''' </summary>
    Protected Overrides Sub UpdateBatchStatus()
        UpdateImportBatchStatus(kBatchStatusComplete, NoOfTotalRecords, NoOfRejections)
    End Sub
#End Region

#Region "Creator"
    Public Sub New(ByVal oXML As XmlDocument)
        MyBase.New(oXML)
    End Sub
#End Region
End Class
