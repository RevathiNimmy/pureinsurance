
<System.Serializable()> _
Friend Class ProductDetailsType

    Public Sub New()

    End Sub

    Public Sub New(ByVal productCode As String)

        If productCode <> "" Then

            Load(Nothing, productCode)

        End If

    End Sub

    Public Sub New(ByVal insuranceFileKey As Integer)

        If insuranceFileKey <> 0 Then

            Load(Nothing, insuranceFileKey)

        End If

    End Sub

    Private _isMidnightRenewal As Boolean
    Public Property IsMidNightRenewal() As Boolean
        Get
            Return _isMidnightRenewal
        End Get
        Set(ByVal value As Boolean)
            _isMidnightRenewal = value
        End Set
    End Property

    Private _productId As Integer
    Public WriteOnly Property ProductId() As Integer
        Set(ByVal value As Integer)
            _productId = value
        End Set
    End Property

    Private _code As String
    Public WriteOnly Property Code() As String
        Set(ByVal value As String)
            _code = value
        End Set
    End Property

    Private _description As String
    Public WriteOnly Property Description() As String
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    Public Sub Load( _
    ByVal con As SiriusConnection, _
    ByVal productCode As String)

        If con Is Nothing Then
            con = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
        End If

        Dim productDataTable As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Product_Select_By_Code")

            cmd.AddInParameter("code", SqlDbType.VarChar, 20).Value = productCode

            productDataTable = con.ExecuteDataTable(cmd)

        End Using

        If productDataTable IsNot Nothing AndAlso productDataTable.Rows.Count > 0 Then

            Me.Code = productCode
            Me.Description = Cast.ToString(productDataTable.Rows(0).Item("description"))
            Me.IsMidNightRenewal = Cast.ToBoolean(productDataTable.Rows(0).Item("is_midnight_renewal"), False)
            Me.ProductId = Cast.ToInt32(productDataTable.Rows(0).Item("product_id"), 0)

        End If

    End Sub

    Public Sub Load( _
        ByVal con As SiriusConnection, _
        ByVal insuranceFileKey As Integer)

        If con Is Nothing Then
            con = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
        End If

        Dim productDataTable As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Product_Select_By_InsuranceFile")

            cmd.AddInParameter("insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey

            productDataTable = con.ExecuteDataTable(cmd)

        End Using

        If productDataTable IsNot Nothing AndAlso productDataTable.Rows.Count > 0 Then

            Me.Code = Cast.ToString(productDataTable.Rows(0).Item("code"))
            Me.Description = Cast.ToString(productDataTable.Rows(0).Item("description"))
            Me.IsMidNightRenewal = Cast.ToBoolean(productDataTable.Rows(0).Item("is_midnight_renewal"), False)
            Me.ProductId = Cast.ToInt32(productDataTable.Rows(0).Item("product_id"), 0)

        End If

    End Sub

End Class

<System.Serializable()> _
Public Class InsuranceFileDetailsType

    Public Sub New()

    End Sub

    Public Sub New(ByVal insuranceFileKey As Integer)

        If insuranceFileKey <> 0 Then
            Load(Nothing, insuranceFileKey)
        Else
            Throw New Exception("Could not initialise InsuranceFileDetailsType as  zero(0) was used as the insurance file cnt")
        End If

    End Sub

    Private _alternativeReference As String
    Public Property AlternativeReference() As String
        Get
            Return _alternativeReference
        End Get
        Set(ByVal value As String)
            _alternativeReference = value
        End Set
    End Property

    Private _insuranceFolderKey As Integer

    Public Property InsuranceFolderKey() As Integer
        Get
            Return _insuranceFolderKey
        End Get
        Set(ByVal value As Integer)
            _insuranceFolderKey = value
        End Set
    End Property

    Private _insuranceRef As String

    Public Property InsuranceRef() As String
        Get
            Return _insuranceRef
        End Get
        Set(ByVal value As String)
            _insuranceRef = value
        End Set
    End Property

    Public Sub Load( _
    ByVal con As SiriusConnection, _
    ByVal insuranceFileKey As Integer)

        If con Is Nothing Then
            con = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
        End If

        Dim insuranceFileDetailsTable As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Insurance_File_Details_Select_By_Key")

            cmd.AddInParameter("insurance_file_cnt", SqlDbType.Int).Value = insuranceFileKey

            insuranceFileDetailsTable = con.ExecuteDataTable(cmd)

        End Using

        If insuranceFileDetailsTable IsNot Nothing AndAlso insuranceFileDetailsTable.Rows.Count > 0 Then

            Me.AlternativeReference = Cast.ToString(insuranceFileDetailsTable.Rows(0).Item("alternate_reference"))

            Me.InsuranceFolderKey = Cast.ToInt32(insuranceFileDetailsTable.Rows(0).Item("insurance_folder_cnt"), 0)

            Me.InsuranceRef = Cast.ToString(insuranceFileDetailsTable.Rows(0).Item("insurance_ref"), String.Empty)

        End If

    End Sub

End Class

