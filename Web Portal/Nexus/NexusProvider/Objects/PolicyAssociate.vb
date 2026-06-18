<Serializable()> Public Class PolicyAssociate

    Private nRowKey As Integer = 0
    Private nInsuranceFileAssociatesKey As Integer = 0
    Private nInsuranceFileKey As Integer = 0
    Private nInsuranceFolderCnt As Integer = 0
    Private nPartyKey As Integer = 0
    Private nAssociationTypeKey As Integer = 0
    Private dDateAttached As DateTime
    Private dDateRemoved As DateTime
    Private bIsDeleted As Boolean = False
    Private sActionType As PolicyAssociateActionType
    Private sAssociationDetail As String = String.Empty
    Private bTimeStamp() As Byte = Nothing
    Private sPartyType As String = String.Empty
    Private sPartyCode As String = String.Empty
    Private sPartyName As String = String.Empty
    Private sCompleteAddress As String = String.Empty
    Private sPostCode As String = String.Empty
    Private oAddress As Address
    Private bDateAttachedSpecified As Boolean = False
    Private bIsDeletedSpecified As Boolean = False
    Private bDateRemovedSpecified As Boolean = False
    Private bAddUnConfirmed As Boolean = False
    Private bDelUnConfirmed As Boolean = False

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("RowKey : " & nRowKey & "<br />")
        sbPrint.AppendLine("InsuranceFileAssociatesKey : " & nInsuranceFileAssociatesKey & "<br />")
        sbPrint.AppendLine("InsuranceFileKey : " & nInsuranceFileKey & "<br />")
        sbPrint.AppendLine("InsuranceFolderCnt : " & nInsuranceFolderCnt & "<br />")
        sbPrint.AppendLine("PartyKey : " & nPartyKey & "<br />")
        sbPrint.AppendLine("AssociationTypeKey : " & nAssociationTypeKey & "<br />")
        sbPrint.AppendLine("DateAttached : " & dDateAttached & "<br />")
        sbPrint.AppendLine("DateRemoved : " & dDateRemoved & "<br />")
        sbPrint.AppendLine("IsDeleted : " & bIsDeleted & "<br />")
        sbPrint.AppendLine("ActionType : " & sActionType & "<br />")
        sbPrint.AppendLine("AssociationDetail : " & sAssociationDetail & "<br />")
        sbPrint.AppendLine("PartyType : " & sPartyType & "<br />")
        sbPrint.AppendLine("PartyCode : " & sPartyCode & "<br />")
        sbPrint.AppendLine("PartyName : " & sPartyName & "<br />")

        Return sbPrint.ToString

    End Function

    Public Property RowKey() As Integer
        Get
            Return nRowKey
        End Get
        Set(ByVal value As Integer)
            nRowKey = value
        End Set
    End Property

    Public Property InsuranceFileAssociatesKey() As Integer
        Get
            Return nInsuranceFileAssociatesKey
        End Get
        Set(ByVal value As Integer)
            nInsuranceFileAssociatesKey = value
        End Set
    End Property
    Public Property InsuranceFileKey() As Integer
        Get
            Return nInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            nInsuranceFileKey = value
        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return nInsuranceFolderCnt
        End Get
        Set(ByVal value As Integer)
            nInsuranceFolderCnt = value
        End Set
    End Property

    Public Property AssociationTypeKey() As Integer
        Get
            Return nAssociationTypeKey
        End Get
        Set(ByVal value As Integer)
            nAssociationTypeKey = value
        End Set
    End Property

    Public Property PartyKey() As Integer
        Get
            Return nPartyKey
        End Get
        Set(ByVal value As Integer)
            nPartyKey = value
        End Set
    End Property

    Public Property DateAttached() As DateTime
        Get
            Return dDateAttached
        End Get
        Set(ByVal value As DateTime)
            dDateAttached = value
        End Set
    End Property

    Public Property DateRemoved() As DateTime
        Get
            Return dDateRemoved
        End Get
        Set(ByVal value As DateTime)
            dDateRemoved = value
        End Set
    End Property

    Public Property IsDeleted() As Boolean
        Get
            Return bIsDeleted
        End Get
        Set(ByVal value As Boolean)
            bIsDeleted = value
        End Set
    End Property

    Public Property AssociationDetail() As String
        Get
            Return sAssociationDetail
        End Get
        Set(ByVal value As String)
            sAssociationDetail = value
        End Set
    End Property

    Public Property ActionType() As PolicyAssociateActionType
        Get
            Return Me.sActionType
        End Get
        Set(ByVal value As PolicyAssociateActionType)
            Me.sActionType = value
        End Set
    End Property

    Public Property TimeStamp() As Byte()
        Get
            Return Me.bTimeStamp
        End Get
        Set(ByVal value As Byte())
            Me.bTimeStamp = value
        End Set
    End Property


    Public Property PartyType() As String
        Get
            Return sPartyType
        End Get
        Set(ByVal value As String)
            sPartyType = value
        End Set
    End Property

    Public Property PartyCode() As String
        Get
            Return sPartyCode
        End Get
        Set(ByVal value As String)
            sPartyCode = value
        End Set
    End Property

    Public Property PartyName() As String
        Get
            Return sPartyName
        End Get
        Set(ByVal value As String)
            sPartyName = value
        End Set
    End Property

    Public Property DateAttachedSpecified() As Boolean
        Get
            Return bDateAttachedSpecified
        End Get
        Set(ByVal value As Boolean)
            bDateAttachedSpecified = value
        End Set
    End Property

    Public Property IsDeletedSpecified() As Boolean
        Get
            Return bIsDeletedSpecified
        End Get
        Set(ByVal value As Boolean)
            bIsDeletedSpecified = value
        End Set
    End Property

    Public Property DateRemovedSpecified() As Boolean
        Get
            Return bDateRemovedSpecified
        End Get
        Set(ByVal value As Boolean)
            bDateRemovedSpecified = value
        End Set
    End Property

    Public Property AddUnConfirmed() As Boolean
        Get
            Return bAddUnConfirmed
        End Get
        Set(ByVal value As Boolean)
            bAddUnConfirmed = value
        End Set
    End Property

    Public Property DelUnConfirmed() As Boolean
        Get
            Return bDelUnConfirmed
        End Get
        Set(ByVal value As Boolean)
            bDelUnConfirmed = value
        End Set
    End Property


    Public ReadOnly Property CompleteAddress() As String
        Get
            Dim sfulladdress As String = String.Empty
            sfulladdress = IIf(Not String.IsNullOrEmpty(Address.Address1), Address.Address1 + ", ", "") + _
                           IIf(Not String.IsNullOrEmpty(Address.Address2), Address.Address2 + ", ", "") + _
                           IIf(Not String.IsNullOrEmpty(Address.Address3), Address.Address3 + ", ", "") + _
                           IIf(Not String.IsNullOrEmpty(Address.Address4), Address.Address4 + ", ", "")
            If sfulladdress <> String.Empty Then
                If (Trim(sfulladdress).Substring(sfulladdress.Length - 2) = ",") Then
                    sCompleteAddress = Trim(sfulladdress).TrimEnd(", ")
                Else
                    sCompleteAddress = Trim(sfulladdress)
                End If
            End If

            Return sCompleteAddress
        End Get

    End Property

    Public Property PostCode() As String
        Get
            Return sPostCode
        End Get
        Set(ByVal value As String)
            sPostCode = value
        End Set
    End Property

    Public Property Address() As Address
        Get
            Return oAddress
        End Get
        Set(ByVal value As Address)
            oAddress = value
        End Set
    End Property

End Class

<Serializable()> Public Class PolicyAssociateCollection : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(PolicyAssociate)
    End Sub
    ''' <summary>
    ''' Debug interface to the object
    ''' </summary>
    ''' <returns>An HTML string containining data held within the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oAddress As Address In List
            sbPrint.AppendLine(oAddress.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add an Associate object to the collection
    ''' </summary>
    ''' <param name="v_oAssociate">The Associate object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oAssociate As PolicyAssociate) As Integer
        v_oAssociate.RowKey = List.Add(v_oAssociate)
        Return v_oAssociate.RowKey
    End Function

    ''' <summary>
    ''' Remove an Associate object from the collection
    ''' </summary>
    ''' <param name="v_oAssociate">The Associate object to be removed</param>
    Public Sub Remove(ByVal v_oAssociate As PolicyAssociate)
        List.Remove(v_oAssociate)
    End Sub

    ''' <summary>
    ''' Remove an Associate object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Associate object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Associate object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Associate object</param>
    ''' <value>The replacement Associate object</value>
    ''' <returns>The Associate object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As PolicyAssociate
        Get
            Return List(i)
        End Get
        Set(ByVal value As PolicyAssociate)
            List(i) = value
        End Set
    End Property

    Public Sub Update(ByVal v_oAssociate As PolicyAssociate)
        List.Item(v_oAssociate.RowKey) = v_oAssociate
    End Sub

    Public Sub Update(ByVal v_oAssociate As PolicyAssociate, ByVal index As Integer)
        List.Item(index) = v_oAssociate
    End Sub

   
End Class

Public Enum PolicyAssociateActionType

    '''<remarks/>

    AddRow

    EditRow

    DeleteRow

End Enum