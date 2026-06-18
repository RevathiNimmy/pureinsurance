<Serializable()> Public Class AddDocumentToDocumasterType
    Private iClaimKey As Integer
    Private iInsuranceFolderKey As Integer
    Private iPartyKey As Integer
    Private iFolderNum As Integer
    Private bVisibleFromWeb As Boolean
    Private sDescription As String
    Private sFileName As String
    Private bDocument As Byte
    Private iDocumentTemplateGroupId As Integer
    Private iDocumentTemplateSubGroupId As Integer
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("ClaimKey : " & iClaimKey & "<br />")
        sbPrint.AppendLine("InsuranceFolderKey : " & iInsuranceFolderKey & "<br />")
        sbPrint.AppendLine("PartyKey : " & iPartyKey & "<br />")
        sbPrint.AppendLine("FolderNum : " & iFolderNum.ToString & "<br />")
        sbPrint.AppendLine("VisibleFromWeb : " & bVisibleFromWeb.ToString & "<br />")
        sbPrint.AppendLine("Description : " & sDescription & "<br />")
        sbPrint.AppendLine("FileName : " & sFileName & "<br />")

        Return sbPrint.ToString()

    End Function
    ''' <summary>
    ''' ClaimKey
    ''' </summary>
    ''' <value>Set the ClaimKey</value>
    ''' <returns>Get the ClaimKey</returns>
    Public Property v_iClaimKey() As Integer
        Get
            Return iClaimKey
        End Get
        Set(ByVal value As Integer)
            iClaimKey = value
        End Set
    End Property
    ''' <summary>
    ''' InsuranceFolderKey
    ''' </summary>
    ''' <value>Set the InsuranceFolderKey</value>
    ''' <returns>Get the InsuranceFolderKey</returns>
    Public Property v_iInsuranceFolderKey() As Integer
        Get
            Return iInsuranceFolderKey
        End Get
        Set(ByVal value As Integer)
            iInsuranceFolderKey = value
        End Set
    End Property
    ''' <summary>
    ''' PartyKey
    ''' </summary>
    ''' <value>Set the PartyKey</value>
    ''' <returns>Get the PartyKey</returns>
    Public Property v_iPartyKey() As Integer
        Get
            Return iPartyKey
        End Get
        Set(ByVal value As Integer)
            iPartyKey = value
        End Set
    End Property
    ''' <summary>
    ''' FolderNum
    ''' </summary>
    ''' <value>Set the FolderNum</value>
    ''' <returns>Get the FolderNum</returns>
    Public Property v_iFolderNum() As Integer
        Get
            Return iFolderNum
        End Get
        Set(ByVal value As Integer)
            iFolderNum = value
        End Set
    End Property
    ''' <summary>
    ''' VisibleFromWeb
    ''' </summary>
    ''' <value>Set the VisibleFromWeb</value>
    ''' <returns>Get the VisibleFromWeb</returns>
    Public Property v_bVisibleFromWeb() As Boolean
        Get
            Return bVisibleFromWeb
        End Get
        Set(ByVal value As Boolean)
            bVisibleFromWeb = value
        End Set
    End Property
    ''' <summary>
    ''' Description
    ''' </summary>
    ''' <value>Set the Description</value>
    ''' <returns>Get the Description</returns>
    Public Property v_sDescription() As String
        Get
            Return sDescription
        End Get
        Set(ByVal value As String)
            sDescription = value
        End Set
    End Property
    ''' <summary>
    ''' FileName
    ''' </summary>
    ''' <value>Set the FileName</value>
    ''' <returns>Get the FileName</returns>
    Public Property v_sFileName() As String
        Get
            Return sFileName
        End Get
        Set(ByVal value As String)
            sFileName = value
        End Set
    End Property
    ''' <summary>
    ''' Document
    ''' </summary>
    ''' <value>Set the Document</value>
    ''' <returns>Get the Document</returns>
    Public Property v_bDocument() As Byte
        Get
            Return bDocument
        End Get
        Set(ByVal value As Byte)
            bDocument = value
        End Set
    End Property
    ''' <summary>
    ''' DocumentTemplateGroupId - Document Category (Document_Template_Group.document_template_group_id)
    ''' </summary>
    ''' <value>Set the DocumentTemplateGroupId</value>
    ''' <returns>Get the DocumentTemplateGroupId</returns>
    Public Property v_iDocumentTemplateGroupId() As Integer
        Get
            Return iDocumentTemplateGroupId
        End Get
        Set(ByVal value As Integer)
            iDocumentTemplateGroupId = value
        End Set
    End Property
    ''' <summary>
    ''' DocumentTemplateSubGroupId - Document Sub-Category (Document_Template_Sub_Group.document_template_sub_group_id)
    ''' </summary>
    ''' <value>Set the DocumentTemplateSubGroupId</value>
    ''' <returns>Get the DocumentTemplateSubGroupId</returns>
    Public Property v_iDocumentTemplateSubGroupId() As Integer
        Get
            Return iDocumentTemplateSubGroupId
        End Get
        Set(ByVal value As Integer)
            iDocumentTemplateSubGroupId = value
        End Set
    End Property
End Class

