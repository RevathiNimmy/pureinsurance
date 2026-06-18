Public NotInheritable Class TEMPLATEValidation 


    Sub Val()


    End Sub

    Private _engine As cGISDataSetControl.Node
    Private _dataSetControl As cGISDataSetControl.Application
    Private _gisExtras As bGISPMUExtras.Business
    Private _transactionType As String = String.Empty
    Private _additionalData As Object
    Private _isBackdatedMTA As Boolean

    'sets the default values as per the old vbscript method (included below for reference)
    '"Sub SetDefaultValue(sTransactionType, vArray, v_bIsBackdatedMTA)" & Strings.Chr(13) & Strings.Chr(10)
    '"    TransactionType = sTransactionType" & Strings.Chr(13) & Strings.Chr(10)
    '"    bIsBackdatedMTA = v_bIsBackdatedMTA" & Strings.Chr(13) & Strings.Chr(10)
    '"    vAdditionalData = vArray" & Strings.Chr(13) & Strings.Chr(10)
    '"End Sub" & Strings.Chr(13) & Strings.Chr(10)
    Public Sub SetDefaultValue(ByVal sTransactionType As String, ByVal vAdditionalData As Object, ByVal bIsBackDatedMTA As Boolean)
        _transactionType = sTransactionType
        _additionalData = vAdditionalData
        _isBackdatedMTA = bIsBackDatedMTA
    End Sub

    'sets the global objects as per the old vbscript method (included below for reference)

    'oScriptControl.AddObject("Engine", oDataSet.Risk, False)
    'oScriptControl.AddObject("DataSet", oDataSet, False)
    'oScriptControl.AddObject("Extras", oExtras, False)
    Public Sub Initialise(ByRef Engine As cGISDataSetControl.Node, ByRef DataSet As cGISDataSetControl.Application, ByVal Extras As Object)
        _engine = Engine
        _dataSetControl = DataSet
        _gisExtras = Extras
    End Sub

    'this is called from bGISQEMPMU for VBScript so bGISQEMCompiled also calls it - hence needed in template
    Public Sub SetAll(ByRef Engine As cGISDataSetControl.Node, ByRef Dataset As cGISDataSetControl.Application, ByVal Extras As Object)
        _engine = Engine
        _dataSetControl = Dataset
        _gisExtras = Extras
    End Sub

    'implement the quote method
    Public Sub Quote()

    End Sub

    'implement the start method
    'PBQuoteTypeEncode.PBCQemQuoteTypeDefault, PBQuoteTypeEncode.PBCQemQuoteTypeQuote, PBQuoteTypeEncode.PBCQemQuoteTypeValidate, 
    'PBQuoteTypeEncode.PBCQemQuoteTypeRenewal, PBQuoteTypeEncode.PBCQemQuoteTypeUal, PBQuoteTypeEncode.PBCQemQuoteTypeRenewalLapse
    Public Sub Start()

    End Sub

    'implement the Main method
    'PBQuoteTypeEncode.PBCQemQuoteTypePreScreen
    Public Sub Main()

    End Sub
    'implement the CopyRisk method
    'PBQuoteTypeEncode.PBCQemQuoteTypeCopyRisk
    Public Sub CopyRisk()

    End Sub

End Class
