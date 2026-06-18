<Serializable()> Public Class BaseClaim
#Region "PrivateFields"
    Private sClientTelNoField As String

    Private sClientFaxNoField As String

    Private sClientMobileNoField As String

    Private sClientEmailField As String

    Private sClientTelNoOffField As String

    Private oClientField As ClaimParty

    Private oInsurerField As ClaimPartyInsurer

    Private sDescriptionField As String

    Private sProgressStatusCodeField As String

    Private sPrimaryCauseCodeField As String

    Private dtLossFromDateField As Date

    Private dtReportedDateField As Date

    Private sHandlerCodeField As String

    Private bInfoOnlyField As Boolean

    Private bLikelyClaimField As Boolean

    Private sSecondaryCauseCodeField As String

    Private sCatastropheCodeField As String

    Private dtLossToDateField As Date

    Private bLossToDateFieldSpecified As Boolean

    Private sLocationField As String

    Private sTownCodeField As String

    Private sUserDefFldACodeField As String

    Private sUserDefFldBCodeField As String

    Private sUserDefFldCCodeField As String

    Private sUserDefFldDCodeField As String

    Private sUserDefFldECodeField As String

    Private sCommentsField As String

    Private sClaimVersionDescriptionField As String

    Private iClaimVersionField As Integer

    Private sClaimStatusField As String

    Private dtClaimStatusDateField As Date

    Private dtLastModifiedDateField As Date
#End Region
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bInfoOnlyField = False
        bLikelyClaimField = False
        bLossToDateFieldSpecified = False

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Client Tel Number  : " & sClientTelNoField & "<br />")
        sbPrint.AppendLine("Client Fax Number  : " & sClientFaxNoField & "<br />")
        sbPrint.AppendLine("Client Mobile Number : " & sClientMobileNoField & "<br />")
        sbPrint.AppendLine("Client Email Field : " & sClientEmailField & "<br />")
        sbPrint.AppendLine("Client Tel No Office Field : " & sClientTelNoOffField & "<br />")
        sbPrint.AppendLine("Description Field : " & sDescriptionField & "<br />")
        sbPrint.AppendLine("Progress Status Code Field : " & sProgressStatusCodeField & "<br />")
        sbPrint.AppendLine("Primary Cause Code Field : " & sPrimaryCauseCodeField & "<br />")
        sbPrint.AppendLine("Loss From Date Field : " & dtLossFromDateField & "<br />")
        sbPrint.AppendLine("Reported Date Field : " & dtReportedDateField & "<br />")
        sbPrint.AppendLine("Handler Code Field : " & sHandlerCodeField & "<br />")
        sbPrint.AppendLine("Info Only Field: " & IIf(bInfoOnlyField, "true", "false") & "<br />")
        sbPrint.AppendLine("Likely Claim Field: " & IIf(bLikelyClaimField, "true", "false") & "<br />")
        sbPrint.AppendLine("Secondary Cause Code Field : " & sSecondaryCauseCodeField & "<br />")
        sbPrint.AppendLine("Catastrophe Code Field : " & sCatastropheCodeField & "<br />")
        sbPrint.AppendLine("Loss To Date Field : " & dtLossToDateField & "<br />")
        sbPrint.AppendLine("Loss To Date Field Specified: " & IIf(bLossToDateFieldSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Location Field : " & sLocationField & "<br />")
        sbPrint.AppendLine("Town Code Field : " & sTownCodeField & "<br />")
        sbPrint.AppendLine("UserDefFldACodeField : " & sUserDefFldACodeField & "<br />")
        sbPrint.AppendLine("UserDefFldBCodeField : " & sUserDefFldBCodeField & "<br />")
        sbPrint.AppendLine("UserDefFldCCodeField : " & sUserDefFldCCodeField & "<br />")
        sbPrint.AppendLine("UserDefFldDCodeField : " & sUserDefFldDCodeField & "<br />")
        sbPrint.AppendLine("UserDefFldECodeField : " & sUserDefFldECodeField & "<br />")
        sbPrint.AppendLine("Comments Field : " & sCommentsField & "<br />")
        sbPrint.AppendLine("Claim Version Description Field : " & sClaimVersionDescriptionField & "<br />")
        sbPrint.AppendLine("Claim Version Field : " & iClaimVersionField & "<br />")
        sbPrint.AppendLine("Claim Status Field : " & sClaimStatusField & "<br />")
        sbPrint.AppendLine("Claim Status Date Field : " & dtClaimStatusDateField & "<br />")
        sbPrint.AppendLine("Last Modified Date Field : " & dtLastModifiedDateField & "<br />")

        Return sbPrint.ToString

    End Function
#Region "Properties"

    '''<remarks/>
    Public Property ClientTelNo() As String
        Get
            Return Me.sClientTelNoField
        End Get
        Set(ByVal value As String)
            Me.sClientTelNoField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientFaxNo() As String
        Get
            Return Me.sClientFaxNoField
        End Get
        Set(ByVal value As String)
            Me.sClientFaxNoField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientMobileNo() As String
        Get
            Return Me.sClientMobileNoField
        End Get
        Set(ByVal value As String)
            Me.sClientMobileNoField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientEmail() As String
        Get
            Return Me.sClientEmailField
        End Get
        Set(ByVal value As String)
            Me.sClientEmailField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientTelNoOff() As String
        Get
            Return Me.sClientTelNoOffField
        End Get
        Set(ByVal value As String)
            Me.sClientTelNoOffField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Client() As ClaimParty
        Get
            Return Me.oClientField
        End Get
        Set(ByVal value As ClaimParty)
            Me.oClientField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Insurer() As ClaimPartyInsurer
        Get
            Return Me.oInsurerField
        End Get
        Set(ByVal value As ClaimPartyInsurer)
            Me.oInsurerField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Description() As String
        Get
            Return Me.sDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ProgressStatusCode() As String
        Get
            Return Me.sProgressStatusCodeField
        End Get
        Set(ByVal value As String)
            Me.sProgressStatusCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property PrimaryCauseCode() As String
        Get
            Return Me.sPrimaryCauseCodeField
        End Get
        Set(ByVal value As String)
            Me.sPrimaryCauseCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property LossFromDate() As Date
        Get
            Return Me.dtLossFromDateField
        End Get
        Set(ByVal value As Date)
            Me.dtLossFromDateField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ReportedDate() As Date
        Get
            Return Me.dtReportedDateField
        End Get
        Set(ByVal value As Date)
            Me.dtReportedDateField = value
        End Set
    End Property

    '''<remarks/>
    Public Property HandlerCode() As String
        Get
            Return Me.sHandlerCodeField
        End Get
        Set(ByVal value As String)
            Me.sHandlerCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property InfoOnly() As Boolean
        Get
            Return Me.bInfoOnlyField
        End Get
        Set(ByVal value As Boolean)
            Me.bInfoOnlyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property LikelyClaim() As Boolean
        Get
            Return Me.bLikelyClaimField
        End Get
        Set(ByVal value As Boolean)
            Me.bLikelyClaimField = value
        End Set
    End Property

    '''<remarks/>
    Public Property SecondaryCauseCode() As String
        Get
            Return Me.sSecondaryCauseCodeField
        End Get
        Set(ByVal value As String)
            Me.sSecondaryCauseCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CatastropheCode() As String
        Get
            Return Me.sCatastropheCodeField
        End Get
        Set(ByVal value As String)
            Me.sCatastropheCodeField = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
    Public Property LossToDate() As Date
        Get
            Return Me.dtLossToDateField
        End Get
        Set(ByVal value As Date)
            Me.dtLossToDateField = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property LossToDateSpecified() As Boolean
        Get
            Return Me.bLossToDateFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bLossToDateFieldSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property Location() As String
        Get
            Return Me.sLocationField
        End Get
        Set(ByVal value As String)
            Me.sLocationField = value
        End Set
    End Property

    '''<remarks/>
    Public Property TownCode() As String
        Get
            Return Me.sTownCodeField
        End Get
        Set(ByVal value As String)
            Me.sTownCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property UserDefFldACode() As String
        Get
            Return Me.sUserDefFldACodeField
        End Get
        Set(ByVal value As String)
            Me.sUserDefFldACodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property UserDefFldBCode() As String
        Get
            Return Me.sUserDefFldBCodeField
        End Get
        Set(ByVal value As String)
            Me.sUserDefFldBCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property UserDefFldCCode() As String
        Get
            Return Me.sUserDefFldCCodeField
        End Get
        Set(ByVal value As String)
            Me.sUserDefFldCCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property UserDefFldDCode() As String
        Get
            Return Me.sUserDefFldDCodeField
        End Get
        Set(ByVal value As String)
            Me.sUserDefFldDCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property UserDefFldECode() As String
        Get
            Return Me.sUserDefFldECodeField
        End Get
        Set(ByVal value As String)
            Me.sUserDefFldECodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Comments() As String
        Get
            Return Me.sCommentsField
        End Get
        Set(ByVal value As String)
            Me.sCommentsField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimVersionDescription() As String
        Get
            Return Me.sClaimVersionDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sClaimVersionDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimVersion() As Integer
        Get
            Return Me.iClaimVersionField
        End Get
        Set(ByVal value As Integer)
            Me.iClaimVersionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimStatus() As String
        Get
            Return Me.sClaimStatusField
        End Get
        Set(ByVal value As String)
            Me.sClaimStatusField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimStatusDate() As Date
        Get
            Return Me.dtClaimStatusDateField
        End Get
        Set(ByVal value As Date)
            Me.dtClaimStatusDateField = value
        End Set
    End Property

    '''<remarks/>
    Public Property LastModifiedDate() As Date
        Get
            Return Me.dtLastModifiedDateField
        End Get
        Set(ByVal value As Date)
            Me.dtLastModifiedDateField = value
        End Set
    End Property
#End Region

End Class
<Serializable()> Public Class ClaimMaintain : Inherits BaseClaim
#Region "PrivateFields"
    Private oClaimPerilMaintain As ClaimPerilMaintain
    Private iBaseClaimKeyField As Integer
    Private bIgnoreClaimMaintainField As Boolean
    Private bExternalHandlerField As Boolean
    Private bCloseClaimOnZeroReserveRecoveryBalanceField As Boolean
#End Region
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bIgnoreClaimMaintainField = False
        bExternalHandlerField = False
        bCloseClaimOnZeroReserveRecoveryBalanceField = False

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Override Function Print() As String
    Public Overrides Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Base Claim Key Field  : " & iBaseClaimKeyField & "<br />")
        sbPrint.AppendLine("IgnoreClaimMaintainField: " & IIf(bIgnoreClaimMaintainField, "true", "false") & "<br />")
        sbPrint.AppendLine("ExternalHandlerField: " & IIf(bExternalHandlerField, "true", "false") & "<br />")
        Return sbPrint.ToString

    End Function
#Region "Properties"
    '''<remarks/>
    Public Property BaseClaimKey() As Integer
        Get
            Return Me.iBaseClaimKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimKeyField = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("ClaimPeril")> _
    Public Property ClaimPeril() As ClaimPerilMaintain
        Get
            Return Me.oClaimPerilMaintain
        End Get
        Set(ByVal value As ClaimPerilMaintain)
            Me.oClaimPerilMaintain = value
        End Set
    End Property

    '''<remarks/>
    Public Property IgnoreClaimMaintain() As Boolean
        Get
            Return Me.bIgnoreClaimMaintainField
        End Get
        Set(ByVal value As Boolean)
            Me.bIgnoreClaimMaintainField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ExternalHandler() As Boolean
        Get
            Return Me.bExternalHandlerField
        End Get
        Set(ByVal value As Boolean)
            Me.bExternalHandlerField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CloseClaimOnZeroReserveRecoveryBalance() As Boolean
        Get
            Return Me.bCloseClaimOnZeroReserveRecoveryBalanceField
        End Get
        Set(ByVal value As Boolean)
            Me.bCloseClaimOnZeroReserveRecoveryBalanceField = value
        End Set
    End Property
#End Region
End Class
<Serializable()> Public Class ClaimOpen : Inherits BaseClaim
#Region "PrivateFields"
    Private oClaimPeril As ClaimPeril

    Private iInsuranceFileKey As Integer

    Private iRiskKey As Integer

    Private sCurrencyCode As String

    Private sUnderwritingYearCode As String

    Private bIgnoreClaimMaintain As Boolean

    Private sDuplicateClaimOverrideUserName As String

    Private sDuplicateClaimOverrideUserPassword As String

#End Region
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bIgnoreClaimMaintain = False
        oClaimPeril = New ClaimPeril


    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("ClaimPeril ---------------><br />")

        If oClaimPeril IsNot Nothing Then
            sbPrint.AppendLine(oClaimPeril.Print())
        End If
        sbPrint.AppendLine("Insurance File Key  : " & iInsuranceFileKey & "<br />")
        sbPrint.AppendLine("Risk Key  : " & iRiskKey & "<br />")
        sbPrint.AppendLine("Currency Code : " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("Underwriting Year Code : " & sUnderwritingYearCode & "<br />")
        sbPrint.AppendLine("Duplicate Claim Override UserName : " & sDuplicateClaimOverrideUserName & "<br />")
        sbPrint.AppendLine("Duplicate Claim Override UserPassword : " & sDuplicateClaimOverrideUserPassword & "<br />")
        sbPrint.AppendLine("IgnoreClaimMaintain: " & IIf(bIgnoreClaimMaintain, "true", "false") & "<br />")

        Return sbPrint.ToString

    End Function
#Region "Properties"
    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("ClaimPeril")> _
    Public Property oClaimPeril() As ClaimPeril()
        Get
            Return Me.oClaimPeril
        End Get
        Set(ByVal value As ClaimPeril())
            Me.oClaimPeril = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property RiskKey() As Integer
        Get
            Return Me.iRiskKey
        End Get
        Set(ByVal value As Integer)
            Me.iRiskKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property UnderwritingYearCode() As String
        Get
            Return Me.sUnderwritingYearCode
        End Get
        Set(ByVal value As String)
            Me.sUnderwritingYearCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property IgnoreClaimMaintain() As Boolean
        Get
            Return Me.bIgnoreClaimMaintain
        End Get
        Set(ByVal value As Boolean)
            Me.bIgnoreClaimMaintain = value
        End Set
    End Property

    '''<remarks/>
    Public Property DuplicateClaimOverrideUserName() As String
        Get
            Return Me.sDuplicateClaimOverrideUserName
        End Get
        Set(ByVal value As String)
            Me.sDuplicateClaimOverrideUserName = value
        End Set
    End Property

    '''<remarks/>
    Public Property DuplicateClaimOverrideUserPassword() As String
        Get
            Return Me.sDuplicateClaimOverrideUserPassword
        End Get
        Set(ByVal value As String)
            Me.sDuplicateClaimOverrideUserPassword = value
        End Set
    End Property
#End Region
End Class



