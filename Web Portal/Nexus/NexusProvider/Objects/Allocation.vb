<Serializable()> Public Class Allocation
#Region "PrivateFields"
    Private iAllocationTransdetailKey As Integer
    Private dAllocationAmount As Double
    Private bAllocationAmountSpecified As Boolean
    Private bAllocationTimeStamp As Byte()
#End Region
#Region "Properties"

    Public ReadOnly Property AllocationAmountSpecified() As Boolean
        Get
            Return bAllocationAmountSpecified
        End Get
    End Property
    '''<remarks/>
    Public Property AllocationTransdetailKey() As Integer
        Get
            Return Me.iAllocationTransdetailKey
        End Get
        Set(ByVal value As Integer)
            Me.iAllocationTransdetailKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property AllocationAmount() As Double
        Get
            Return Me.dAllocationAmount
        End Get
        Set(ByVal value As Double)
            Me.dAllocationAmount = value
        End Set
    End Property
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")> _
    Public Property AllocationTimeStamp() As Byte()
        Get
            Return Me.bAllocationTimeStamp
        End Get
        Set(ByVal value As Byte())
            Me.bAllocationTimeStamp = value
        End Set
    End Property


    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function

#End Region
End Class
<Serializable()> Public Class AllocationCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oAllocation As Allocation In List
            sbPrint.AppendLine(oAllocation.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a BaseParty object to the collection
    ''' </summary>
    ''' <param name="v_oParty">The BaseParty object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oAllocation As Allocation) As Integer
        Return List.Add(v_oAllocation)
    End Function

    ''' <summary>
    ''' Remove an BaseParty object from the collection
    ''' </summary>
    ''' <param name="v_oParty">The BaseParty object to be removed</param>
    Public Sub Remove(ByVal v_oAllocation As Allocation)
        List.Remove(v_oAllocation)
    End Sub


    ''' <summary>
    ''' Retrieve or replace an BaseParty object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the BaseParty object</param>
    ''' <value>The replacement BaseParty object</value>
    ''' <returns>The BaseParty object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Allocation
        Get
            Return List(i)
        End Get
        Set(ByVal value As Allocation)
            List(i) = value
        End Set
    End Property

End Class