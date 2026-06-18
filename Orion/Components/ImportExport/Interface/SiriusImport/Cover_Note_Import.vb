Option Explicit On
Option Strict On

Imports System.Collections.ObjectModel
Imports System.Xml


Friend NotInheritable Class Cover_Note_Import : Inherits ImportBase
#Region " Private Fields"
    Private m_nNoOfTotalRecords As Integer
    Private m_nNoOfRejections As Integer
#End Region
#Region "Public Properties"
    ''' <summary>
    ''' Specifies the batch code for this interface
    ''' </summary>
    Public Overrides ReadOnly Property BatchCode() As String
        Get
            Return "COVBOOKI"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the active interface name
    ''' </summary>
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Cover_Note_Import"
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
    Private Sub CreateBook()
        Dim iReturn As Integer
        '    Dim m_oChildElement As Xml.XmlNodeList = Nothing
        Dim m_lChildCount As Integer
        Dim lCnt As Integer

        Dim r_lCoverNoteBookID As Long = Nothing
        Dim sBookNumber As String = m_oElement.GetAttribute("book_number")
        Dim lStart_Number As Integer = Util.ToSafeInt(m_oElement.GetAttribute("start_number"), 0)
        Dim lEnd_Number As Integer = Util.ToSafeInt(m_oElement.GetAttribute("end_number"), 0)
        Dim sAgentCode As String = m_oElement.GetAttribute("agent_short_code")
        Dim sBranchCode As String = m_oElement.GetAttribute("branch_code")
        Dim sProductCode As String = Nothing
        Dim r_lReturnStatus As Integer = Nothing

        Try
            If sAgentCode.Trim.Length = 0 Then
                Throw New Exception(String.Format("Unable to find Agent Code for '{0}'", sBookNumber))
            End If

            If sBranchCode.Trim.Length = 0 Then
                Throw New Exception(String.Format("Unable to find Branch Code for '{0}'", sBookNumber))
            End If

            ' Add parameters
            AddParameterLite(m_oDatabase, "new_book_id", r_lCoverNoteBookID, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "book_number", sBookNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "start_number", lStart_Number, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "end_number", lEnd_Number, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "agent_code", sAgentCode.Trim.ToString, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "source_code", sBranchCode.Trim.ToString, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "user_id", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)

            ' Execute sql
            iReturn = m_oDatabase.SQLAction("spu_SIR_Cover_Note_Book_Add", "Add Cover Note Book", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_SIR_Cover_Note_Book_Add'")
            End If

            ' Get book id
            r_lCoverNoteBookID = Util.ToSafeInt(m_oDatabase.Parameters.Item("new_book_id").Value, 0)
            If r_lCoverNoteBookID <= 0 Then
                If r_lCoverNoteBookID = -1 Then
                    Throw New Exception(String.Format("Book number '{0}' already exists.", sBookNumber))
                ElseIf r_lCoverNoteBookID = -2 Then
                    Throw New Exception(String.Format("Unable to find Branch Code for '{0}'", sBookNumber))
                ElseIf r_lCoverNoteBookID = -3 Then
                    Throw New Exception(String.Format("Unable to find Agent Code for '{0}'", sBookNumber))
                Else
                    Throw New Exception(String.Format("Unable to add Book Number '{0}'", sBookNumber))
                End If
            End If

            'm_oChildElement = m_oElement.ChildNodes
            m_lChildCount = m_oElement.ChildNodes.Count

            ' Process each reference
            For lCnt = 0 To m_lChildCount - 1
                sProductCode = m_oElement.ChildNodes(lCnt).Attributes(0).Value
                If sProductCode.Trim.Length = 0 Then
                    Throw New Exception(String.Format("Unable to find Product Code for '{0}'", sBookNumber))
                End If

                'reset to zero
                r_lReturnStatus = 0
                AddParameterLite(m_oDatabase, "product_code", sProductCode.Trim.ToString, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
                AddParameterLite(m_oDatabase, "cover_note_book_id", r_lCoverNoteBookID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
                AddParameterLite(m_oDatabase, "return_status", r_lReturnStatus, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

                ' Execute sql
                iReturn = m_oDatabase.SQLAction("spu_SIR_Save_CoverNoteProducts", "Add Cover Note Book Products", True)
                If iReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("Unable to execute 'spu_SIR_Save_CoverNoteProducts'")
                End If

                ' Get book id
                r_lReturnStatus = Util.ToSafeInt(m_oDatabase.Parameters.Item("return_status").Value, 0)
                If r_lReturnStatus <= 0 Then
                    If r_lReturnStatus = -1 Then
                        Throw New Exception(String.Format("Unable to find Product Code: '{0}'", sProductCode.Trim.ToString))
                    Else
                        Throw New Exception(String.Format("Unable to Add Book Number '{0}'", sBookNumber))
                    End If
                End If

            Next lCnt

        Catch ex As Exception
            Throw New Exception(String.Format("Unable to add new book for booknumber '{0}'", sBookNumber), ex)
        Finally
            ' Check for successful creation of book
            If r_lCoverNoteBookID = 0 Then
                Throw New Exception(String.Format("Unable to add new book for booknumber '{0}'", sBookNumber))
            End If
        End Try
    End Sub

    Protected Overrides Sub ProcessElement()
        Try
            NoOfTotalRecords += 1
            ' Create New Book
            CreateBook()
        Catch ex As Exception
            Throw New Exception("Unable to add new book.", ex)
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
