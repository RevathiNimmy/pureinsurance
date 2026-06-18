<Serializable()> Public Class MidFileDetails
    Dim sFileSequenceNumber As String
    Dim oMIDPolicy As MidPolicyCollection

    Dim bFailuresOnly As Boolean

    Sub New()
        oMIDPolicy = New MidPolicyCollection

    End Sub
    Public Property FileSequenceNumber() As String
        Get
            Return sFileSequenceNumber
        End Get
        Set(ByVal value As String)
            sFileSequenceNumber = value
        End Set
    End Property
    Public Property Policies() As MidPolicyCollection
        Get
            Return oMIDPolicy
        End Get
        Set(ByVal value As MidPolicyCollection)
            oMIDPolicy = value
        End Set
    End Property
   
    Public Property FailuresOnly() As Boolean
        Get
            Return bFailuresOnly
        End Get
        Set(ByVal value As Boolean)
            bFailuresOnly = value
        End Set
    End Property
End Class
<Serializable()> Public Class MidPolicy
    Dim iMidPolicyKey As Integer
    Dim sMidPolicyStatusCode As String
    Dim iInsuranceFileKey As Integer
    Dim sInsuranceFileRef As String
    Dim iPPPC As Integer
    Dim iExpectedPPPC As Integer
    Dim sUpdateType As String
    Dim iBatchKey As Integer
    Dim sBatchRef As String
    Dim sRejectReference As String
    Dim sRejectErrorCodes As String
    Dim sStatusCode As String
    Dim oMIDVehicle As MIDVehicleCollection
    Sub New()
        oMIDVehicle = New MIDVehicleCollection
    End Sub
    Public Property Vehicles() As MIDVehicleCollection
        Get
            Return oMIDVehicle
        End Get
        Set(ByVal value As MIDVehicleCollection)
            oMIDVehicle = value
        End Set
    End Property
    Public Property StatusCode() As String
        Get
            Return sStatusCode
        End Get
        Set(ByVal value As String)
            sStatusCode = value
        End Set
    End Property
    Public Property RejectErrorCodes() As String
        Get
            Return sRejectErrorCodes
        End Get
        Set(ByVal value As String)
            sRejectErrorCodes = value
        End Set
    End Property
    Public Property RejectReference() As String
        Get
            Return sRejectReference
        End Get
        Set(ByVal value As String)
            sRejectReference = value
        End Set
    End Property
    Public Property BatchRef() As String
        Get
            Return sBatchRef
        End Get
        Set(ByVal value As String)
            sBatchRef = value
        End Set
    End Property
    Public Property BatchKey() As Integer
        Get
            Return iBatchKey
        End Get
        Set(ByVal value As Integer)
            iBatchKey = value
        End Set
    End Property
    Public Property UpdateType() As String
        Get
            Return sUpdateType
        End Get
        Set(ByVal value As String)
            sUpdateType = value
        End Set
    End Property
    Public Property ExpectedPPPC() As Integer
        Get
            Return iExpectedPPPC
        End Get
        Set(ByVal value As Integer)
            iExpectedPPPC = value
        End Set
    End Property
    Public Property PPPC() As Integer
        Get
            Return iPPPC
        End Get
        Set(ByVal value As Integer)
            iPPPC = value
        End Set
    End Property
    Public Property InsuranceFileRef() As String
        Get
            Return sInsuranceFileRef
        End Get
        Set(ByVal value As String)
            sInsuranceFileRef = value
        End Set
    End Property
    Public Property MidPolicyKey() As Integer
        Get
            Return iMidPolicyKey
        End Get
        Set(ByVal value As Integer)
            iMidPolicyKey = value
        End Set
    End Property
    Public Property MidPolicyStatusCode() As String
        Get
            Return sMidPolicyStatusCode
        End Get
        Set(ByVal value As String)
            sMidPolicyStatusCode = value
        End Set
    End Property
    Public Property InsuranceFileKey() As Integer
        Get
            Return iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            iInsuranceFileKey = value
        End Set
    End Property
End Class
<Serializable()> Public Class MidPolicyCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()
        For Each oMidPolicy As MidPolicy In List
            sbPrint.AppendLine("<br />")
        Next
        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oMidPolicy As MidPolicy) As Integer
        Return List.Add(v_oMidPolicy)
    End Function
    Public Sub Remove(ByVal v_oMidPolicy As MidPolicy)
        List.Remove(v_oMidPolicy)
    End Sub
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    Default Public Property Item(ByVal i As Integer) As MidPolicy
        Get
            Return List(i)
        End Get
        Set(ByVal value As MidPolicy)
            List(i) = value
        End Set
    End Property

    Public Function NumberOfRows() As Integer
        Return List.Count()
    End Function

End Class
<Serializable()> Public Class MIDVehicle
    Dim iMIDVehicleKey As Integer
    Dim iMIDPolicyKey As Integer
    Dim sUpdateType As String
    Dim sRegistration As String
    Dim bIsForeignReg As Boolean
    Dim bIsTradeReg As Boolean
    Dim sMake As String
    Dim sModel As String
    Dim dOnDate As DateTime
    Dim dOffDate As DateTime
    Dim sRejectReference As String
    Dim sRejectErrorCodes As String
    Dim sStatusCode As String
    Sub New()

    End Sub
    Public Property StatusCode() As String
        Get
            Return sStatusCode
        End Get
        Set(ByVal value As String)
            sStatusCode = value
        End Set
    End Property
    Public Property RejectErrorCodes() As String
        Get
            Return sRejectErrorCodes
        End Get
        Set(ByVal value As String)
            sRejectErrorCodes = value
        End Set
    End Property
    Public Property RejectReference() As String
        Get
            Return sRejectReference
        End Get
        Set(ByVal value As String)
            sRejectReference = value
        End Set
    End Property
    Public Property OffDate() As DateTime
        Get
            Return dOffDate
        End Get
        Set(ByVal value As DateTime)
            dOffDate = value
        End Set
    End Property
    Public Property OnDate() As DateTime
        Get
            Return dOnDate
        End Get
        Set(ByVal value As DateTime)
            dOnDate = value
        End Set
    End Property
    Public Property Model() As String
        Get
            Return sModel
        End Get
        Set(ByVal value As String)
            sModel = value
        End Set
    End Property
    Public Property Make() As String
        Get
            Return sMake
        End Get
        Set(ByVal value As String)
            sMake = value
        End Set
    End Property
    Public Property IsTradeReg() As Boolean
        Get
            Return bIsTradeReg
        End Get
        Set(ByVal value As Boolean)
            bIsTradeReg = value
        End Set
    End Property
    Public Property IsForeignReg() As Boolean
        Get
            Return bIsForeignReg
        End Get
        Set(ByVal value As Boolean)
            bIsForeignReg = value
        End Set
    End Property
    Public Property Registration() As String
        Get
            Return sRegistration
        End Get
        Set(ByVal value As String)
            sRegistration = value
        End Set
    End Property
    Public Property UpdateType() As String
        Get
            Return sUpdateType
        End Get
        Set(ByVal value As String)
            sUpdateType = value
        End Set
    End Property
    Public Property MIDPolicyKey() As Integer
        Get
            Return iMIDPolicyKey
        End Get
        Set(ByVal value As Integer)
            iMIDPolicyKey = value
        End Set
    End Property
    Public Property MIDVehicleKey() As Integer
        Get
            Return iMIDVehicleKey
        End Get
        Set(ByVal value As Integer)
            iMIDVehicleKey = value
        End Set
    End Property
End Class
<Serializable()> Public Class MIDVehicleCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()
        For Each oMidPolicy As MidPolicy In List
            sbPrint.AppendLine("<br />")
        Next
        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oMIDVehicle As MIDVehicle) As Integer
        Return List.Add(v_oMIDVehicle)
    End Function
    Public Sub Remove(ByVal v_oMIDVehicle As MIDVehicle)
        List.Remove(v_oMIDVehicle)
    End Sub
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    Default Public Property Item(ByVal i As Integer) As MIDVehicle
        Get
            Return List(i)
        End Get
        Set(ByVal value As MIDVehicle)
            List(i) = value
        End Set
    End Property

    Public Function NumberOfRows() As Integer
        Return List.Count()
    End Function

End Class