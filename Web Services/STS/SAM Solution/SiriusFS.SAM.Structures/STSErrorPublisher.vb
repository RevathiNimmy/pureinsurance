Imports System.Xml.Serialization
Imports System.IO
Imports System.Xml
Imports System.Runtime.Serialization.Formatters
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Xml.XmlReader
Imports System.Text
Imports System.Web.Services.Protocols
' Custom Imports
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports SiriusFS.SAM.Structure.BaseImplementationTypes

Public Class STSErrorPublisher

    Private _Error As New STSErrorType
    Private _TheException As Exception

    Public Const MandatoryInputMissing As String = "Mandatory {0} is missing"
    Public Const MandatoryInputInvalid As String = "{0} is invalid"
    Public Const DateInputInvalid As String = "{0} is not a valid date"
    Public Const InValidField As String = "Invalid Field"

    Public Enum STSErrorCodes As Long
        GeneralFailure = 0
        BackofficeComponentFailed = 11
        MandatoryInputMissing = 100
        InvalidDateFormat = 101
        InvalidLookupListValue = 102
        InvalidFormat = 103
        RecordLockedByAnotherUser = 200
        RecordNotLockedByCurrentUser = 201
        BackOfficeUnavailable = 202
        UserNotAuthorisedToActOnData = 203
        SecurityCheckFailed = 204
        TokenExpired = 205
        RecordChanged = 206
        BranchCodeInvalid = 210
        BranchMismatch = 211
        PolicyMismatch = 212
        PartyDOBIsInFuture = 213
        PartyDOBIsTooOld = 214
        PolicyRiskLinkRecordNotFound = 219
        QuoteHeaderRecordNotFound = 220
        CoverStartDateIsInThePast = 221
        CoverEndDateIsBeforeCoverStartDate = 222
        PartyRecordNotFound = 223
        PolicyRecordNotFound = 224
        FailedToLoadRiskDB = 225
        FailedToRetrievePremiumDetails = 226
        ListTypeNotFound = 227
        AddressRecordNotFound = 228
        RiskRecordNotFound = 229
        DefaultXMLFileNotAvailable = 230
        DefaultXMLFilePathNotFound = 232
        DefaultXMLFileFailedToLoad = 233
        DefaultXMLFilePathTooLong = 234
        ConfigurationFileNotAvailable = 240
        ConfigurationFilePathNotFound = 242
        ConfigurationFileFailedToLoad = 243
        ConfigurationFilePathTooLong = 244
        XMLDocumentBadlyFormed = 245
        FailedToCreateBackofficeComponent = 250
        FailedToInitialiseBackofficeComponent = 251
        BackofficeFailed = 252
        FailedToConnectToTheSiriusDatabase = 253
        SQLServerReturnedAnError = 254
        FailedToRetrieveDatamodelCodeFromXml = 260
        XmldatasetBadlyFormed = 261
        DatasetPathRegistrySettingNotFound = 262
        FailedToAddRiskRecord = 263
        FailedToMergeRiskDataset = 264
        UserAuthorityLevelsCheckFailed = 265
        ValidationRulesFailed = 266
        FailedToQuoteTheRisk = 267
        FailedToSaveRiskToDatabase = 268
        SchemeVersionNumberMissing = 269
        SchemaForVersionMissing = 270
        FileNotFound = 271
        RecordNotFound = 272
        StatusOfRiskPreventsDeletion = 273
        AgentRecordNotFound = 274
        BackofficeComponentReturnedRecordInUse = 810
        BackofficeComponentReturnedNotFound = 811
        BrokerOrSchemeInvalid = 812
        ValidationRulesReferred = 275
        ValidationRulesDeclined = 276
        UALRulesReferred = 277
        UALRulesDeclined = 278
        RatingRulesReferred = 279
        RatingRulesDeclined = 280
        LoginFailureIncorrectUsername = 290
        LoginFailureIncorrectPassword = 291
        LoginFailureLoggedInElsewhere = 292
        LoginFailureNotLinkedToAgent = 293
        UserDoesNotExist = 294
        NoEmailAddressDefinedForUser = 295
        UserUpdateFailed = 296
        StartDateIsInThePast = 297
        EndDateIsBeforeStartDate = 298
        EnableAgentProductLinkFieldInvalid = 299
        UnableToCloseTheCase = 300
        LoginFailureWeakPassword = 301
        LoginFailureReusedPassword = 302
        NotValidMarketPlaceProduct = 303
        GenerateDocumentFailed = 304
        BlankOtherPartyCode = 305
    End Enum

    Public ReadOnly Property HasErrors() As Boolean
        Get
            With _Error
                If .InternalException Is Nothing _
                And .InvalidData Is Nothing _
                And .SiriusBackOffice Is Nothing _
                And .STSBusinessRule Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            End With
        End Get
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal Description As String, ByVal TheException As Exception)

        ' Create a new instance of Back Office Error
        _Error.InternalException = New STSErrorInternalExceptionType
        ' Set the Description
        _Error.InternalException.Description = Description

        _TheException = TheException

    End Sub

    Public Sub New(ByVal BackOfficeReturnValue As Int32, ByVal Description As String)

        ' Create a new instance of Back Office Error
        _Error.SiriusBackOffice = New STSErrorSiriusBackOfficeType
        ' Set the Back Office Return Value
        _Error.SiriusBackOffice.ReturnValue = BackOfficeReturnValue
        _Error.SiriusBackOffice.Description = Description

    End Sub

    Public Sub New(ByVal STSErrorCode As STSErrorCodes, ByVal Description As String, ByVal Detail As String)

        ' Create a new instance of the STS Error
        _Error.STSBusinessRule = New STSErrorSTSBusinessRuleType

        ' Setup the specific error details
        With _Error.STSBusinessRule
            .Code = STSErrorCode
            .Description = Description
            .Detail = Detail
        End With

    End Sub

    Public Sub New(ByVal InvalidFieldName As String, ByVal Code As String, ByVal Description As String, ByVal SuppliedValue As String)

        ' Has the Error Data been created
        'If _Error.InvalidData Is Nothing Then
        '_Error.InvalidData = New STSErrorInvalidData
        'End If

        Dim Invalid As New STSErrorInvalidDataType

        ' Set the supplied values
        With Invalid
            .FieldName = InvalidFieldName
            .SuppliedValue = SuppliedValue
            .Code = Code
            .Description = Description
        End With

        ' Create an entry for one Missing Data item
        ReDim _Error.InvalidData(0)

        ' Store it
        _Error.InvalidData(0) = Invalid

    End Sub

    Public Sub AddInvalidField(ByVal InvalidFieldName As String, ByVal Code As String, ByVal Description As String, ByVal SuppliedValue As String)

        ' Has the Error Data been created
        'If _Error.InvalidData Is Nothing Then
        '_Error.InvalidData = New STSErrorInvalidData
        'End If

        Dim Invalid As New STSErrorInvalidDataType

        ' Set the supplied values
        With Invalid
            .FieldName = InvalidFieldName
            .SuppliedValue = SuppliedValue
            .Code = Code
            .Description = Description
        End With

        ' Create an entry for one Missing Data item
        If _Error.InvalidData Is Nothing Then
            ReDim _Error.InvalidData(0)
        ElseIf _Error.InvalidData.Length = 0 Then
            ReDim _Error.InvalidData(0)
        Else
            ReDim Preserve _Error.InvalidData(_Error.InvalidData.Length)
        End If

        ' Store it
        _Error.InvalidData(_Error.InvalidData.Length - 1) = Invalid

    End Sub

    Public Sub Raise(ByVal WebService As String, ByVal WebMethod As String, ByVal Label As String, ByVal LogLocally As Boolean)

        ' For some unknown reason the details property of the SOAP Fault has to be wrapped in a detail element or it wont be recieved on the client

        ' Format the Message
        Dim sMessage As New String("STSError, see SoapException.detail for more information.")

        ' Populate the method Specific Stuff
        With _Error
            .WebService = WebService
            .WebMethod = WebMethod
            .Label = Label
        End With

        ' Load the detail into a DOM
        Dim ErrorDoc As New XmlDocument
        Dim oNode As XmlNode
        ErrorDoc.LoadXml(Me.ToString)

        Dim ErrorDoc2 As New XmlDocument
        ErrorDoc2.LoadXml("<detail/>")

        oNode = ErrorDoc2.ImportNode(ErrorDoc.DocumentElement, True)

        ErrorDoc2.DocumentElement.AppendChild(oNode)

        ' Always Log InternalExceptions
        If _TheException Is Nothing = False Then
            LogLocally = True
        End If

        ' Do we want to log it locally
        If LogLocally = True Then
            ' Yes, so create an Exception and Publish It
            If _TheException Is Nothing Then
                Dim TheError As New Exception(ErrorDoc2.OuterXml)
                ExceptionManager.Publish(TheError)
            Else
                Dim TheError As New Exception(ErrorDoc2.OuterXml, _TheException)
                ExceptionManager.Publish(TheError)
            End If

        End If

        ' Throw the SOAP FAULT
        Throw New SoapException(Message:=sMessage.ToString, _
                     code:=SoapException.ClientFaultCode, _
                    actor:=WebService, _
                    Detail:=ErrorDoc2)

    End Sub

    Public Sub RaiseAgain(ByVal WebMethod As String)

        ' Throw the error again
        Call Raise(WebMethod, _Error.Label, False)

    End Sub

    Public Sub Raise(ByVal WebMethod As String, ByVal Label As String, ByVal LogLocally As Boolean)

        ' For some unknown reason the details property of the SOAP Fault has to be wrapped in a detail element or it wont be recieved on the client

        ' Format the Message
        Dim sMessage As New String("STSError, see SoapException.detail for more information.")

        ' Populate the method Specific Stuff
        With _Error
            .WebService = HttpContext.Current.Request.Url.ToString()
            .WebMethod = WebMethod
            .Label = Label
        End With

        ' Load the detail into a DOM
        Dim ErrorDoc As New XmlDocument
        Dim oNode As XmlNode
        ErrorDoc.LoadXml(Me.ToString)

        Dim ErrorDoc2 As New XmlDocument
        ErrorDoc2.LoadXml("<detail/>")

        oNode = ErrorDoc2.ImportNode(ErrorDoc.DocumentElement, True)

        ErrorDoc2.DocumentElement.AppendChild(oNode)

        ' Always Log InternalExceptions
        If _TheException Is Nothing = False Then
            LogLocally = True
        End If

        ' Do we want to log it locally
        If LogLocally = True Then
            ' Yes, so create an Exception and Publish It
            If _TheException Is Nothing Then
                Dim TheError As New Exception(ErrorDoc2.OuterXml)
                ExceptionManager.Publish(TheError)
            Else
                Dim TheError As New Exception(ErrorDoc2.OuterXml, _TheException)
                ExceptionManager.Publish(TheError)
            End If

        End If

        ' Throw the SOAP FAULT
        Throw New SoapException(Message:=sMessage.ToString, _
                        code:=SoapException.ClientFaultCode, _
                    actor:=HttpContext.Current.Request.Url.ToString(), _
                    Detail:=ErrorDoc2)

    End Sub

    Public Sub SetContext(ByVal Label As String)

        ' Populate the method Specific Stuff
        With _Error
            .Label = Label
        End With

    End Sub

    Public Sub SetContext(ByVal WebService As String, ByVal WebMethod As String, ByVal LogLocally As Boolean)

        ' Populate the method Specific Stuff
        With _Error
            .WebService = WebService
            .WebMethod = WebMethod
        End With

        ' Do we want to log it locally
        If LogLocally = True Then
            Try
                ' Yes, so create an Exception and Publish It
                Dim TheError As New Exception(Me.ToString)
                ExceptionManager.Publish(TheError)
            Catch
            End Try
        End If

    End Sub

    Public Sub SetContext(ByVal WebService As String, ByVal WebMethod As String, ByVal Label As String, ByVal LogLocally As Boolean)

        ' Populate the method Specific Stuff
        With _Error
            .WebService = WebService
            .WebMethod = WebMethod
            .Label = Label
        End With

        ' Do we want to log it locally
        If LogLocally = True Then
            Try
                ' Yes, so create an Exception and Publish It
                Dim TheError As New Exception(Me.ToString)
                ExceptionManager.Publish(TheError)
            Catch
            End Try
        End If

    End Sub

    Public Sub SetContext(ByRef PutErrorHere As STSErrorType, ByVal WebService As String, ByVal WebMethod As String, ByVal Label As String, ByVal LogLocally As Boolean)

        ' Populate the method Specific Stuff
        With _Error
            .WebService = WebService
            .WebMethod = WebMethod
            .Label = Label
        End With

        ' Do we want to log it locally
        If LogLocally = True Then
            ' Yes, so create an Exception and Publish It
            Try
                Dim TheError As New Exception(Me.ToString)
                ExceptionManager.Publish(TheError)
            Catch
            End Try
        End If

        ' Set the Error
        PutErrorHere = _Error

    End Sub

    Public Sub SetContext(ByRef PutErrorHere As STSErrorType, ByVal WebService As String, ByVal WebMethod As String, ByVal LogLocally As Boolean)

        ' Populate the method Specific Stuff
        With _Error
            .WebService = WebService
            .WebMethod = WebMethod
        End With

        ' Do we want to log it locally
        If LogLocally = True Then
            Try
                ' Yes, so create an Exception and Publish It
                Dim TheError As New Exception(Me.ToString)
                ExceptionManager.Publish(TheError)
            Catch
            End Try
        End If

        ' Set the Error
        PutErrorHere = _Error

    End Sub

    Public Overrides Function ToString() As String

        ' Create an instance of the XmlSerializer class;
        ' specify the type of object to serialize.
        Dim serializer As New XmlSerializer(GetType(STSErrorType))

        Dim form As New BinaryFormatter

        Dim writer As New StringWriter

        Try
            serializer.Serialize(writer, _Error)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

        Dim message As String = writer.ToString()

        writer.Close()

        Return message

    End Function

    Public Sub FromString(ByVal XMLString As String)

        Dim ErrorDom As New XmlDocument
        Dim sXML As String

        ErrorDom.LoadXml(XMLString)

        ' Is the STSError wrapped in the SOAP detail element
        If (ErrorDom.DocumentElement.Name = "STSError") Then
            ' No, the document element is the STSError
            sXML = ErrorDom.DocumentElement.InnerXml
        Else
            ' Yes, so ignore the detail element and go straight to the STSError
            sXML = ErrorDom.DocumentElement.FirstChild.InnerXml
        End If

        Dim STSError As STSErrorType

        STSError = DeserializeXML(v_sXML:=sXML, _
                                            v_vType:=GetType(STSErrorType))

        _Error = STSError

    End Sub

#Region " DeserializeXML"

    Private Function DeserializeXML(ByVal v_sXML As String, _
                                    ByVal v_vType As System.Type) As Object

        Dim oDS As Object

        Dim oXMLContext As New XmlParserContext(Nothing, Nothing, Nothing, System.Xml.XmlSpace.None)
        Dim oXMLSerialzier As New Xml.Serialization.XmlSerializer(v_vType)

        ' Deserialize the XML back into a Class
        Try
            Using oXMLReader As New XmlTextReader(v_sXML, XmlNodeType.Document, oXMLContext)
                oDS = oXMLSerialzier.Deserialize(oXMLReader)
            End Using
        Catch ex As Exception
            Throw New Exception("Failed to deserialize XML.", ex)
        End Try

        Return oDS

    End Function

#End Region

End Class
