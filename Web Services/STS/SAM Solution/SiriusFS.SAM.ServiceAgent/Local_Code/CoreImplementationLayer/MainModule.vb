Option Strict Off
Option Explicit On

Imports System.IO
Imports Microsoft.Win32
Imports Microsoft.ApplicationBlocks.Data
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.STSErrorPublisher
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports System.Reflection
Imports dPMDAOBridge
Imports System.Runtime.Serialization
Imports System.Security.Cryptography

' Class to contain common SAM functions. These should be declared as 'shared'
' so that they can be used without instantiating an instance of the class
Public Class SAMFunc

    Private Const ACClass As String = "MainModule"

    Public Const ConnectionStringFrame As String = "Server={server};Database={database};Integrated Security=False; User ID={loginid}; Password={loginpassword}"
    Public Const ConnectionStringFrameWindowsAuthentication As String = "Server={server};Database={database};Integrated Security=True;"
    Public Const ACApp As String = "SiriusTransactionService"

    Public Const ACSTSUserName As String = "STS"

    Private _SiriusUser As New SIRIUSUSER

    Private Shared m_oCache As System.Web.Caching.Cache
    ' ***************************************************************** '
    ' Name: BuildKeyString (Private)
    '
    ' Description: Builds up the Key String for the Reg Setting
    ' ***************************************************************** '
    Private Shared Function BuildKeyString(ByVal v_ePMEProductFamily As PMEProductFamily, ByVal v_ePMERegSettingLevel As PMERegSettingLevel, Optional ByVal v_sSubKey As String = "") As String

        Dim sKeyString As String = String.Empty

        Try

            ' Build up the key String

            ' Start with Root
            sKeyString = ACRegRoot

            ' Add PM Product
            Select Case v_ePMEProductFamily
                Case PMEProductFamily.pmePFSiriusArchitecture
                    sKeyString = sKeyString & ACRegSiriusArchitecture
                Case PMEProductFamily.pmePFSiriusUnderwriting
                    sKeyString = sKeyString & ACRegSiriusUnderwriting
                Case PMEProductFamily.pmePFOrion
                    sKeyString = sKeyString & ACRegOrion
                Case PMEProductFamily.pmePFGemini
                    sKeyString = sKeyString & ACRegGemini
                Case PMEProductFamily.pmePFVoyager
                    sKeyString = sKeyString & ACRegVoyager
                Case PMEProductFamily.pmePFMercury
                    sKeyString = sKeyString & ACRegMercury
                Case PMEProductFamily.pmePFDocumaster
                    sKeyString = sKeyString & ACRegDocumaster
                Case PMEProductFamily.pmePFSiriusBroking
                    sKeyString = sKeyString & ACRegSiriusBroking
                    'RFC251198 - Added SiriusSolutions & Nirvana Registry Constants
                Case PMEProductFamily.pmePFSiriusSolutions
                    sKeyString = sKeyString & ACRegSiriusSolutions
                Case PMEProductFamily.pmePFNirvana
                    sKeyString = sKeyString & ACRegNirvana
                    'RFC060799 - Added GeminiII Product Family, DSN etc etc
                Case PMEProductFamily.pmePFGeminiII
                    sKeyString = sKeyString & ACRegGeminiII
                    ' RDC 07082000 - new product family: Claims
                Case PMEProductFamily.pmePFClaims
                    sKeyString = sKeyString & ACRegClaims
            End Select

            ' Add Level
            Select Case v_ePMERegSettingLevel
                Case PMERegSettingLevel.pmeRSLClient
                    sKeyString = sKeyString & ACRegClient
                Case PMERegSettingLevel.pmeRSLServer
                    sKeyString = sKeyString & ACRegServer
                Case PMERegSettingLevel.pmeRSLCommon
                    sKeyString = sKeyString & ACRegCommon
                Case PMERegSettingLevel.pmeRSLSetup
                    sKeyString = sKeyString & ACRegSetup
                Case Else
                    sKeyString = sKeyString & ACRegCommon
            End Select

            ' Has a Sub key been supplied
            If (v_sSubKey <> "") Then

                ' Yes we have a sub key

                ' Add a separator if the Start of the sub key does not have one
                If (Mid(v_sSubKey, 1, 1) <> "\") Then
                    sKeyString = sKeyString & "\"
                End If

                ' Add the Sub Key
                sKeyString = sKeyString & v_sSubKey

                ' Remove a Trailing separator if there is one
                If (Mid(sKeyString, Len(sKeyString), 1) = "\") Then
                    sKeyString = Mid(sKeyString, 1, Len(sKeyString) - 1)
                End If

            End If

            ' Return the string
            BuildKeyString = sKeyString



        Catch ex As Exception

            BuildKeyString = ""

            Exit Function

        End Try
    End Function

    ''' <summary>
    ''' ConvertSTSError - This section contains overloaded methods to convert the 
    ''' Base Implementation STS error type into a specific public type.  
    ''' </summary>
    ''' <param name="impSTSError">The base implementation object variable</param>
    ''' <param name="msgSTSError">The public interface type object variable of a specific type for the conversion</param>
    ''' <remarks>
    ''' Each method is identical except for the datatype in use.
    ''' 
    ''' Important - If you change the behaviour of one of the methods you MUST 
    ''' replicate that change through the remaining methods.
    ''' </remarks>

    Public Overloads Shared Sub ConvertSTSError(ByVal impSTSError As STSErrorType, ByRef msgSTSError As SFI.MessagingTypes.STSErrorType)

        Dim iCnt As Integer
        Dim iLBnd As Integer
        Dim iUBnd As Integer

        If (impSTSError Is Nothing) = False Then

            msgSTSError = New SFI.MessagingTypes.STSErrorType

            msgSTSError.WebMethod = impSTSError.WebMethod
            msgSTSError.WebService = impSTSError.WebService
            msgSTSError.Label = impSTSError.Label

            If (impSTSError.InternalException Is Nothing) = False Then
                msgSTSError.InternalException = New SFI.MessagingTypes.STSErrorInternalExceptionType
                msgSTSError.InternalException.Description = impSTSError.InternalException.Description
            End If

            If (impSTSError.InvalidData Is Nothing) = False Then
                ReDim msgSTSError.InvalidData(impSTSError.InvalidData.GetUpperBound(0))

                iLBnd = impSTSError.InvalidData.GetLowerBound(0)
                iUBnd = impSTSError.InvalidData.GetUpperBound(0)

                For iCnt = iLBnd To iUBnd
                    msgSTSError.InvalidData(iCnt) = New SFI.MessagingTypes.STSErrorInvalidDataType
                    msgSTSError.InvalidData(iCnt).Code = impSTSError.InvalidData(iCnt).Code
                    msgSTSError.InvalidData(iCnt).Description = impSTSError.InvalidData(iCnt).Description
                    msgSTSError.InvalidData(iCnt).FieldName = impSTSError.InvalidData(iCnt).FieldName
                    msgSTSError.InvalidData(iCnt).SuppliedValue = impSTSError.InvalidData(iCnt).SuppliedValue
                Next
            End If

            If (impSTSError.SiriusBackOffice Is Nothing) = False Then
                msgSTSError.SiriusBackOffice = New SFI.MessagingTypes.STSErrorSiriusBackOfficeType

                msgSTSError.SiriusBackOffice.Description = impSTSError.SiriusBackOffice.Description
                msgSTSError.SiriusBackOffice.ReturnValue = impSTSError.SiriusBackOffice.ReturnValue
            End If

            If (impSTSError.STSBusinessRule Is Nothing) = False Then
                msgSTSError.STSBusinessRule = New SFI.MessagingTypes.STSErrorSTSBusinessRuleType

                msgSTSError.STSBusinessRule.Code = impSTSError.STSBusinessRule.Code
                msgSTSError.STSBusinessRule.Description = impSTSError.STSBusinessRule.Description
                msgSTSError.STSBusinessRule.Detail = impSTSError.STSBusinessRule.Detail
            End If

        End If

    End Sub

#Region "ConvertWCFSTSError- Messaging Services"

    ''' <summary>
    ''' This method will used to collect the error informaion for WCF messaging layer
    ''' </summary>
    ''' <param name="oImpSTSError"></param>
    ''' <param name="oMsgSTSError"></param>
    ''' <remarks></remarks>
    Public Shared Sub ConvertWCFSTSError(ByVal oImpSTSError As STSErrorType, ByRef oMsgSTSError As SFI.Messaging.WCF.STSErrorType)

        Dim nCnt As Integer = 0
        Dim nLBnd As Integer = 0
        Dim nUBnd As Integer = 0

        Try
            If oImpSTSError IsNot Nothing Then

                oMsgSTSError = New SFI.Messaging.WCF.STSErrorType

                oMsgSTSError.WebMethod = oImpSTSError.WebMethod
                oMsgSTSError.WebService = oImpSTSError.WebService
                oMsgSTSError.Label = oImpSTSError.Label

                If oImpSTSError.InternalException IsNot Nothing Then
                    oMsgSTSError.InternalException = New SFI.Messaging.WCF.STSErrorInternalExceptionType
                    oMsgSTSError.InternalException.Description = oImpSTSError.InternalException.Description
                End If

                If oImpSTSError.InvalidData IsNot Nothing Then
                    oMsgSTSError.InvalidData() = New List(Of SFI.Messaging.WCF.STSErrorInvalidDataType)
                    oMsgSTSError.InvalidData.Capacity = (oImpSTSError.InvalidData.GetUpperBound(0))

                    nLBnd = oImpSTSError.InvalidData.GetLowerBound(0)
                    nUBnd = oImpSTSError.InvalidData.GetUpperBound(0)
                    Dim oTempMsgSTSError As SFI.Messaging.WCF.STSErrorInvalidDataType
                    For nCnt = nLBnd To nUBnd
                        oTempMsgSTSError = New SFI.Messaging.WCF.STSErrorInvalidDataType
                        oTempMsgSTSError.Code = oImpSTSError.InvalidData(nCnt).Code
                        oTempMsgSTSError.Description = oImpSTSError.InvalidData(nCnt).Description
                        oTempMsgSTSError.FieldName = oImpSTSError.InvalidData(nCnt).FieldName
                        oTempMsgSTSError.SuppliedValue = oImpSTSError.InvalidData(nCnt).SuppliedValue
                        oMsgSTSError.InvalidData.Add(oTempMsgSTSError)
                    Next
                End If

                If oImpSTSError.SiriusBackOffice IsNot Nothing Then
                    oMsgSTSError.SiriusBackOffice = New SFI.Messaging.WCF.STSErrorSiriusBackOfficeType

                    oMsgSTSError.SiriusBackOffice.Description = oImpSTSError.SiriusBackOffice.Description
                    oMsgSTSError.SiriusBackOffice.ReturnValue = oImpSTSError.SiriusBackOffice.ReturnValue
                End If

                If oImpSTSError.STSBusinessRule IsNot Nothing Then
                    oMsgSTSError.STSBusinessRule = New SFI.Messaging.WCF.STSErrorSTSBusinessRuleType

                    oMsgSTSError.STSBusinessRule.Code = oImpSTSError.STSBusinessRule.Code
                    oMsgSTSError.STSBusinessRule.Description = oImpSTSError.STSBusinessRule.Description
                    oMsgSTSError.STSBusinessRule.Detail = oImpSTSError.STSBusinessRule.Detail
                End If

            End If
        Catch exError As Exception
            Dim STSError As New STSErrorPublisher()
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), "ConvertWCFSTSError", exError.Message.ToString(), True)
        End Try

    End Sub

#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sPassword"></param>
    ''' <param name="r_sDecryptedPassword"></param>
    ''' <remarks></remarks>
    Friend Shared Sub Decrypt(ByVal v_sPassword As String, ByRef r_sDecryptedPassword As String)

        Dim sAString As String = String.Empty
        Dim sBString As String = String.Empty
        Dim iCntr As Integer
        Dim iCntr2 As Integer
        Dim sChar1 As String = String.Empty
        Dim sChar2 As String = String.Empty
        Dim iSn As Integer
        Dim sCodeString As String = String.Empty
        Dim iClen As Integer

        ' Decrypts the supplied string returning the Decrypted
        ' result. Decrypted string will always be 2 characters
        ' shorter than original
        '

        sCodeString = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"
        iClen = Len(sCodeString)

        sAString = v_sPassword
        iCntr = Len(sAString) - 2 'take 2 off as ignoring first and last characters in password

        If (iCntr < 1) Then

            r_sDecryptedPassword = ""

            Exit Sub
        End If

        '1..Find out value for iSn
        'ASCII value of last character + ASCII value of first character, mod by 56, add 1 to result
        iSn = ((Asc(Right(sAString, 1)) + Asc(Left(sAString, 1))) Mod iClen) + 1

        '2..Find value of sChar1$
        'Is simply the last character
        sChar1 = Right(sAString, 1)

        '3..Find value of sChar2$
        'Is simply the first character
        sChar2 = Left(sAString, 1)

        '4..Now we have all the variable values used, plug the value into the loop for every char in
        '   the password. Note that we ignore the first and last characters in the password now
        sAString = Mid(sAString, 2, Len(sAString) - 2)

        Dim iPos As Integer
        Dim iTemp As Integer
        Dim sTemp As String = String.Empty
        For iCntr2 = 1 To iCntr

            iPos = InStr(1, sCodeString, Mid(sAString, iCntr2, 1), CompareMethod.Binary)
            iTemp = iPos - 1
            iTemp = iTemp + (iClen * 2) 'this iClen * 2 is dodgy ! - could be * 1 or other???
            iTemp = iTemp - iSn - iCntr2

            If iTemp > 122 Then iTemp = iTemp - 56

            sTemp = Chr(iTemp)

            sBString = sBString & sTemp
        Next

        ' Return the result.
        r_sDecryptedPassword = Trim(sBString)

        Exit Sub

    End Sub

      Public Shared Sub DeserializeImplementationDataSet( _
          ByVal sXMLString As String, _
          ByVal oXMLSerializer As Serialization.XmlSerializer, _
        ByRef r_oResultDataSet As Object)

        ' Deserialize the XML from the implementation resultdataset into 
        ' the correct messaginging format
        Dim oXMLContext As New XmlParserContext(Nothing, Nothing, Nothing, XmlSpace.None)
        Using oXMLReader As New XmlTextReader(sXMLString, XmlNodeType.Document, oXMLContext)
            ' Deserialize the XML back into a Class
            r_oResultDataSet = oXMLSerializer.Deserialize(oXMLReader)
            oXMLReader.Close()
        End Using
    End Sub

    'This is an overloaded method and will take precedence in future
    Public Shared Sub DeserializeImplementationDataSet( _
    ByVal sXMLString As String, _
    ByVal oXMLSerializer As Serialization.XmlSerializer, _
    ByRef r_oResultDataSet As Object, _
    ByVal sDefaultNameSpace As String)

        ' Deserialize the XML from the implementation resultdataset into 
        ' the correct messaginging format

        Dim oXMLContext As XmlParserContext
        If String.IsNullOrEmpty(sDefaultNameSpace) = False Then
            Dim xmlDoc As New XmlDocument
            Dim xmlNamespace As New XmlNamespaceManager(xmlDoc.NameTable)
            xmlNamespace.AddNamespace(String.Empty, sDefaultNameSpace)
            xmlDoc = Nothing
            oXMLContext = New XmlParserContext(Nothing, xmlNamespace, Nothing, XmlSpace.None)
        Else
            oXMLContext = New XmlParserContext(Nothing, Nothing, Nothing, XmlSpace.None)
        End If

        Using oXMLReader As New XmlTextReader(sXMLString, XmlNodeType.Document, oXMLContext)

            ' Deserialize the XML back into a Class
            r_oResultDataSet = oXMLSerializer.Deserialize(oXMLReader)
        End Using
    End Sub

    ''' <summary>
    ''' Used to convert Get List of Type Specified by processing XML information provided as input by internally calling DeserializeImplementationDataSetWCF
    ''' </summary>
    ''' <param name="elmResultDataSet">Input information as XmlElement</param>
    ''' <param name="sFromTypeName">Specify the name of type to convert</param>
    ''' <param name="sConvertToTypeName">Specify name of list type expected in return</param>
    ''' <param name="sFromTypeIsArrayOf">Specify the name of type the sFromTypeName is array of</param>
    ''' <remarks></remarks>
    Public Shared Function GetDeserializedValues(Of T)(ByVal elmResultDataSet As System.Xml.XmlElement, ByVal sFromTypeName As String, ByVal sConvertToTypeName As String, Optional ByVal sFromTypeIsArrayOf As String = "Row") As T

        Dim r_lstResultDataSet As T = Nothing

        If elmResultDataSet IsNot Nothing AndAlso elmResultDataSet.InnerXml <> String.Empty Then
            Dim oDCSerializer As New DataContractSerializer(GetType(T))
            Dim lstFilterNodes As New List(Of xmlNodeCasting)

            lstFilterNodes.Add(New xmlNodeCasting(sFromTypeName, sConvertToTypeName, True))
            lstFilterNodes.Add(New xmlNodeCasting(sFromTypeIsArrayOf, sConvertToTypeName))

            SAMFunc.DeserializeImplementationDataSetWCF(sXMLString:=elmResultDataSet.OuterXml, oDCSerializer:=oDCSerializer, o_oResultDataSet:=r_lstResultDataSet, lstFilterNodes:=lstFilterNodes)

            lstFilterNodes = Nothing
            oDCSerializer = Nothing

        End If

        Return r_lstResultDataSet

    End Function

    ''' <summary>
    ''' Used to convert xml to the type of specified in DatContractSerializer passed as an argument `
    ''' </summary>
    ''' <param name="sXMLString">XML as String</param>
    ''' <param name="oDCSerializer">Expects DataContractSerializer</param>
    ''' <param name="o_oResultDataSet">Returns Object of type specified in DatContractSerializer argument</param>
    ''' <param name="lstFilterNodes">Expects List of xmlNodeCasting specifying details of Conversion</param>
    ''' <remarks></remarks>
    Private Shared Sub DeserializeImplementationDataSetWCF( _
            ByVal sXMLString As String, _
            ByVal oDCSerializer As DataContractSerializer, _
            ByRef o_oResultDataSet As Object,
            Optional ByVal lstFilterNodes As List(Of xmlNodeCasting) = Nothing)

        'Make Sure This constant (kNameSpace) has a string value with empty space as prefix to avoid wrong concat
        Const kNameSpace As String = " xmlns='http://www.siriusfs.com/SFI/SAM/BaseTypes/20080429'"

        Dim sXMLStringBuilder As StringBuilder

        'CAST THE ELEMENTS BASED ON FILTERS
        sXMLStringBuilder = New StringBuilder(sXMLString)
        If lstFilterNodes IsNot Nothing Then
            For Each e As xmlNodeCasting In lstFilterNodes
                If e.IsList = True Then
                    sXMLStringBuilder.Replace("<" & e.FromTypeName & ">", "<ArrayOf" & e.ConvertToTypeName & kNameSpace & ">")
                    sXMLStringBuilder.Replace("</" & e.FromTypeName & ">", "</ArrayOf" & e.ConvertToTypeName & ">")
                Else
                    sXMLStringBuilder.Replace("<" & e.FromTypeName & ">", "<" & e.ConvertToTypeName & ">")
                    sXMLStringBuilder.Replace("</" & e.FromTypeName & ">", "</" & e.ConvertToTypeName & ">")
                End If
            Next
        End If

        sXMLString = sXMLStringBuilder.ToString

        'Sort the XML as DataContractSerializer expects sorted XML to evaluate values correctly.
        xmlSort.Sort(sXMLString)

        Using msXMLData As MemoryStream = New MemoryStream
            Using swXMLData As New StreamWriter(msXMLData)
                swXMLData.Write(sXMLString)
                swXMLData.Flush()

                msXMLData.Position = 0

                Using xdrReadXML As XmlDictionaryReader = XmlDictionaryReader.CreateTextReader(msXMLData, New XmlDictionaryReaderQuotas())
                    ' Deserialize the XML back into a Class
                    o_oResultDataSet = oDCSerializer.ReadObject(xdrReadXML, True)
                    xdrReadXML.Close()
                End Using
                msXMLData.Close()
            End Using
        End Using
        'CLear All Objects
        sXMLStringBuilder = Nothing

    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sPassword"></param>
    ''' <param name="r_sEncryptedPassword"></param>
    ''' <remarks></remarks>
    Friend Shared Sub Encrypt(ByVal sPassword As String, ByRef r_sEncryptedPassword As String)

        Dim sAString As String = String.Empty
        Dim sBString As String = String.Empty
        Dim iCntr As Integer
        Dim iCntr2 As Integer
        Dim sChar1 As String = String.Empty
        Dim sChar2 As String = String.Empty
        Dim iSn As Integer
        Dim sCodeString As String = String.Empty
        Dim iClen As Integer

        ' Encrypts the supplied string returning the Encrypted
        ' result. Encrypted string will always be 2 characters
        ' longer than original (leave space!)
        '
        ' Encrypted string contains only ASCII characters in
        ' range 32-126

        sCodeString = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"
        iClen = sCodeString.Length

        sAString = sPassword
        iCntr = sAString.Length

        If (iCntr < 1) Then
            r_sEncryptedPassword = ""
            Exit Sub
        End If

        sChar1 = Mid(sCodeString, (((Asc(Left(sAString, 1)) + iCntr) Mod iClen) + 1), 1)
        sChar2 = Mid(sCodeString, ((Asc(Right(sAString, 1)) Mod iClen) + 1), 1)
        iSn = ((Asc(sChar1) + Asc(sChar2)) Mod iClen) + 1
        sBString$ = sChar2

        For iCntr2 = 1 To iCntr
            sBString = sBString + Mid(sCodeString, ((Asc(Mid(sAString, iCntr2, 1)) + _
                iSn + iCntr2) Mod iClen) + 1, 1)
        Next iCntr2

        sBString = sBString + sChar1

        ' Return the result.
        r_sEncryptedPassword = Trim(sBString)
    End Sub

    ' Get the ABI address code as stored on the Sirius address_usage_type table
    ' Should this be done after the SAM contact type has been passed to bGIS?
    Friend Shared Function GetABIAddressCode(ByVal eAddressType As AddressTypeType) As String

        Dim sOut As String = String.Empty
        Select Case eAddressType
            Case BaseImplementationTypes.AddressTypeType.Home
                sOut = "3131 001"
            Case BaseImplementationTypes.AddressTypeType.Business
                sOut = "3131 002"
            Case BaseImplementationTypes.AddressTypeType.Other
                sOut = "3131 0X9"
            Case BaseImplementationTypes.AddressTypeType.SubAgent
                sOut = "3131 0XR"
            Case BaseImplementationTypes.AddressTypeType.Branch
                sOut = "3131 XBA"
            Case BaseImplementationTypes.AddressTypeType.Billing
                sOut = "3131 XBI"
            Case BaseImplementationTypes.AddressTypeType.Correspondence
                sOut = "3131 XCO"
            Case BaseImplementationTypes.AddressTypeType.Previous
                sOut = "3131 XPR"
            Case BaseImplementationTypes.AddressTypeType.Registered
                sOut = "3131 XRE"
            Case BaseImplementationTypes.AddressTypeType.Site
                sOut = "3131 XSA"
            Case BaseImplementationTypes.AddressTypeType.Email     ' Vivek: 20080704 - added the missing item to fix bug in existing system (Amend Client)
                sOut = "3131 ECK"
            Case Else
                sOut = "UNKNOWN Address Type Enum"
        End Select

        Return sOut

    End Function

    ' Get the ABI address code as stored on the Sirius address_usage_type table
    ' Should this be done after the SAM contact type has been passed to bGIS?
    Friend Shared Function GetAddressTypeCode(ByVal sAddressCode As String) As AddressTypeType

        Select Case sAddressCode
            Case "3131 001"
                GetAddressTypeCode = BaseImplementationTypes.AddressTypeType.Home
            Case "3131 002"
                GetAddressTypeCode = BaseImplementationTypes.AddressTypeType.Business
            Case "3131 0X9"
                GetAddressTypeCode = BaseImplementationTypes.AddressTypeType.Other
            Case "3131 0XR"
                GetAddressTypeCode = BaseImplementationTypes.AddressTypeType.SubAgent
            Case "3131 XBA"
                GetAddressTypeCode = BaseImplementationTypes.AddressTypeType.Branch
            Case "3131 XBI"
                GetAddressTypeCode = BaseImplementationTypes.AddressTypeType.Billing
            Case "3131 XCO"
                GetAddressTypeCode = BaseImplementationTypes.AddressTypeType.Correspondence
            Case "3131 XPR"
                GetAddressTypeCode = BaseImplementationTypes.AddressTypeType.Previous
            Case "3131 XRE"
                GetAddressTypeCode = BaseImplementationTypes.AddressTypeType.Registered
            Case "3131 XSA"
                GetAddressTypeCode = BaseImplementationTypes.AddressTypeType.Site
            Case "3131 ECK"     ' Vivek: 20080704 - added the missing item to fix bug in existing system (Amend Client)
                GetAddressTypeCode = BaseImplementationTypes.AddressTypeType.Email
        End Select

        Return GetAddressTypeCode

    End Function

    ' Get the contact type code as stored on the Sirius Contacts table
    ' Should this be done after the SAM contact type has been passed to bGIS?
    Friend Shared Function GetPMContactTypeCode(ByVal eContactType As ContactTypeType, Optional ByVal sOtherContactType As String = "") As String

        Dim sOut As String = String.Empty
        Select Case eContactType
            Case BaseImplementationTypes.ContactTypeType.EMAIL
                sOut = "E-MAIL"
            Case BaseImplementationTypes.ContactTypeType.MAINEMAILCONTACT
                sOut = "MEMAIL"
            Case BaseImplementationTypes.ContactTypeType.HOMEPHONE
                sOut = "TELEPHONE"
            Case BaseImplementationTypes.ContactTypeType.OTHER
                sOut = Trim(sOtherContactType)
            Case Else
                sOut = [Enum].GetName(GetType(BaseImplementationTypes.ContactTypeType), eContactType)
        End Select

        Return sOut

    End Function

    Friend Overloads Shared Sub InitialiseSBOObject(ByVal oObject As Object, ByVal siriusUser As SIRIUSUSER, Optional ByVal sBranchCode As String = "", Optional ByVal sObjectName As String = "")
        'Dim iRet As Int32

        'Const ACMethodName As String = "InitialiseSBOObject"

        'RK modifies as part of SAM SFI Interop conversion and as required by SSP
        Dim con As SiriusConnection = Nothing
        InitialiseSBOObject(con, oObject, siriusUser, sBranchCode, sObjectName)

    End Sub

    Friend Overloads Shared Sub InitialiseSBOObject(ByVal Con As SiriusConnection, ByVal oObject As Object, ByVal SiriusUser As SIRIUSUSER, Optional ByVal sBranchCode As String = "", Optional ByVal sObjectName As String = "")
        Dim iRet As Int32
        Const ACMethodName As String = "InitialiseSBOObject"

        'Rk modifies as part of SAM SFI Interop conversions in this subroutine & starts it here
        Dim oDatabase As Object = Nothing
        If Not Con Is Nothing Then
            oDatabase = Con.PMDAODatabase
        End If

        Try

            'iRet = CInt(initialiseMethod.Invoke(oObject, New Object() {SiriusUser.Username, SiriusUser.Password, SiriusUser.UserID, SiriusUser.SourceID, SiriusUser.LanguageID, SiriusUser.CurrencyID, SiriusUserDefaults.LogLevel, SiriusUserDefaults.AppName, False, oDatabase}))
            iRet = oObject.Initialise(sUsername:=SiriusUser.Username, sPassword:=SiriusUser.Password, iUserID:=SiriusUser.UserID, iSourceID:=SiriusUser.SourceID, iLanguageID:=SiriusUser.LanguageID, iCurrencyID:=SiriusUser.CurrencyID, iLogLevel:=SiriusUserDefaults.LogLevel, sCallingAppName:=SiriusUserDefaults.AppName, vDatabase:=oDatabase)

            If iRet <> PMEReturnCode.PMTrue Then
                Dim STSError As New STSErrorPublisher(iRet, sObjectName & ".Initialise failed")
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseGISReturn", True)
            End If

        Catch ex As Exception
            Dim STSError As New STSErrorPublisher(sObjectName & ".Initialise failed", ex)
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "Initialise SBO Object Exception", True)
        End Try

        'Rk modifies as part of SAM SFI Interop conversions in this subroutine & ends it here
    End Sub

    Friend Overloads Shared Sub InitialiseGIS(ByVal Con As SiriusConnection, ByVal oGIS As bGIS.Application, ByVal SiriusUser As SIRIUSUSER, Optional ByVal sBranchCode As String = "")
        Dim iRet As Int32

        Const ACMethodName As String = "InitialiseGIS"


        'Following line is being added by rk as part of SAM Interop conversions and as per internal and SSP discussion in place of LOC next to it (now commented somewhere below saying oDatabase = con.PMDAODatabase)
        Dim oDatabase As Object = Nothing
        If Not Con Is Nothing Then
            oDatabase = Con.PMDAODatabase
        End If
        'oDatabase = Con.SqlConnection.Database
        'oDatabase = Con.PMDAODatabase

        ' Initialise the Component
        Try
            iRet = CInt(oGIS.Initialise(SiriusUser.Username, SiriusUser.Password, SiriusUser.UserID, SiriusUser.SourceID, SiriusUser.LanguageID, SiriusUser.CurrencyID, SiriusUserDefaults.LogLevel, SiriusUserDefaults.AppName, False, oDatabase))
        Catch ex As Exception
            Dim STSError As New STSErrorPublisher("oGIS.Initialise failed", ex)
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseGISException", True)
        End Try

        If iRet <> PMEReturnCode.PMTrue Then
            Dim STSError As New STSErrorPublisher(iRet, "bGIS.Application.Initialise failed")
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseGISReturn", True)
        End If

    End Sub

    Friend Overloads Shared Sub InitialiseGISQP(ByVal oGIS As bGIS.QuotePolicy, ByVal SiriusUser As SIRIUSUSER, Optional ByVal sBranchCode As String = "")

        Const ACMethodName As String = "InitialiseGISQP"

        Dim iRet As Int32

        ' Initialise the Component
        Try
            iRet = oGIS.Initialise(SiriusUser.Username, SiriusUser.Password, CInt(SiriusUser.UserID), CInt(SiriusUser.SourceID), CInt(SiriusUser.LanguageID), CInt(SiriusUser.CurrencyID), 1, SiriusUserDefaults.AppName)
        Catch ex As Exception
            Dim STSError As New STSErrorPublisher("bGIS.QuotePolicy.Initialise failed", ex)
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseGISQPException", True)
        End Try

        If iRet <> PMEReturnCode.PMTrue Then
            Dim STSError As New STSErrorPublisher(iRet, "bGIS.QuotePolicy.Initialise failed")
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseGISQPReturn", True)
        End If

    End Sub

    Friend Overloads Shared Sub InitialiseGISQP(ByVal Con As SiriusConnection, ByVal oGIS As bGIS.QuotePolicy, ByVal SiriusUser As SIRIUSUSER, Optional ByVal sBranchCode As String = "")

        Const ACMethodName As String = "InitialiseGISQP"

        Dim iRet As Int32

        Dim oDatabase As Object = Nothing
        If Not Con Is Nothing Then
            oDatabase = Con.PMDAODatabase
        End If

        ' Initialise the Component
        Try
            iRet = oGIS.Initialise(SiriusUser.Username, SiriusUser.Password, CInt(SiriusUser.UserID), CInt(SiriusUser.SourceID), CInt(SiriusUser.LanguageID), CInt(SiriusUser.CurrencyID), 1, SiriusUserDefaults.AppName, oDatabase)
        Catch ex As Exception
            Dim STSError As New STSErrorPublisher("bGIS.QuotePolicy.Initialise failed", ex)
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseGISQPException", True)
        End Try

        If iRet <> PMEReturnCode.PMTrue Then
            Dim STSError As New STSErrorPublisher(iRet, "bGIS.QuotePolicy.Initialise failed")
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseGISQPReturn", True)
        End If

    End Sub

    Friend Overloads Shared Sub InitialiseGISSecurity(ByVal oGIS As bGIS.Security, ByVal SiriusUser As SIRIUSUSER, Optional ByVal sBranchCode As String = "")

        Const ACMethodName As String = "InitialiseGISSecurity"

        Dim iRet As Int32

        ' Initialise the Component
        Try
            iRet = oGIS.Initialise(SiriusUser.Username, SiriusUser.Password, CInt(SiriusUser.UserID), CInt(SiriusUser.SourceID), CInt(SiriusUser.LanguageID), CInt(SiriusUser.CurrencyID), 1, SiriusUserDefaults.AppName)
        Catch ex As Exception
            Dim STSError As New STSErrorPublisher("bGIS.Security.Initialise failed", ex)
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseGISSecurityException", True)
        End Try

        If iRet <> PMEReturnCode.PMTrue Then
            Dim STSError As New STSErrorPublisher(iRet, "bGIS.Security.Initialise failed")
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseGISSecurityException", True)
        End If

    End Sub

    Friend Overloads Shared Sub InitialiseGISSTS(ByVal oGIS As bGIS.STS, ByVal SiriusUser As SIRIUSUSER, Optional ByVal sBranchCode As String = "")

        Const ACMethodName As String = "InitialiseGISSTS"

        Dim iRet As Int32

        ' Initialise the Component
        Try
            iRet = oGIS.Initialise(SiriusUser.Username, SiriusUser.Password, CInt(SiriusUser.UserID), CInt(SiriusUser.SourceID), CInt(SiriusUser.LanguageID), CInt(SiriusUser.CurrencyID), 1, SiriusUserDefaults.AppName)
        Catch ex As Exception
            Dim STSError As New STSErrorPublisher("bGIS.STS.Initialise failed", ex)
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseGISSTSException", True)
        End Try

        If iRet <> PMEReturnCode.PMTrue Then
            Dim STSError As New STSErrorPublisher(iRet, "bGIS.STS.Initialise failed")
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseGISSTSReturn", True)
        End If

    End Sub

    Friend Overloads Shared Sub InitialiseGISSTS(ByVal Con As SiriusConnection, ByVal oGIS As bGIS.STS, ByVal SiriusUser As SIRIUSUSER, Optional ByVal sBranchCode As String = "")

        Const ACMethodName As String = "InitialiseGISSTS"

        Dim iRet As Int32


        'Following line is being added by rk as part of SAM Interop conversions and as per internal and SSP discussion in place of LOC next to it (now commented somewhere below saying oDatabase = con.PMDAODatabase)
        'Con = SiriusConnection.FromAny(connectionString:=ConnectionString)
        'oDatabase = Con.SqlConnection.Database
        Dim oDatabase As Object = Nothing
        If Not Con Is Nothing Then
            Try
                oDatabase = Con.PMDAODatabase
            Catch ex As Exception
                oDatabase = Nothing
            End Try

        End If

        ' Initialise the Component
        Try
            'rk modified as part of SAM SFI Interop conversion
            If (oDatabase Is Nothing) Then
                iRet = oGIS.Initialise(SiriusUser.Username, SiriusUser.Password, CInt(SiriusUser.UserID), CInt(SiriusUser.SourceID), CInt(SiriusUser.LanguageID), CInt(SiriusUser.CurrencyID), 1, SiriusUserDefaults.AppName)
            Else
                iRet = oGIS.Initialise(SiriusUser.Username, SiriusUser.Password, CInt(SiriusUser.UserID), CInt(SiriusUser.SourceID), CInt(SiriusUser.LanguageID), CInt(SiriusUser.CurrencyID), 1, SiriusUserDefaults.AppName, oDatabase)
            End If
            'iRet = oGIS.Initialise(SiriusUser.Username, SiriusUser.Password, CInt(SiriusUser.UserID), CInt(SiriusUser.SourceID), CInt(SiriusUser.LanguageID), CInt(SiriusUser.CurrencyID), 1, SiriusUserDefaults.AppName, oDatabase)
            'vikas
            'iRet = oGIS.Initialise(SiriusUser.Username, SiriusUser.Password, CInt(SiriusUser.UserID), CInt(SiriusUser.SourceID), CInt(SiriusUser.LanguageID), CInt(SiriusUser.CurrencyID), 1, SiriusUserDefaults.AppName)
        Catch ex As Exception
            Dim STSError As New STSErrorPublisher("bGIS.STS.Initialise failed", ex)
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseGISSTSException", True)
        End Try

        If iRet <> PMEReturnCode.PMTrue Then
            Dim STSError As New STSErrorPublisher(iRet, "bGIS.STS.Initialise failed")
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseGISSTSReturn", True)
        End If

    End Sub

    Public Shared Function NothingToString(ByVal sStringObject As String) As String

        Dim sOut As String = String.Empty

        If Not IsNothing(sStringObject) Then
            sOut = sStringObject
        End If

        Return sOut

    End Function

    ' Writes to the trace listeners collection
    Friend Shared Sub STSLogMessage(ByVal v_oTraceSetting As TraceSwitch, _
                                  ByVal v_oLogLevel As TraceLevel, _
                                  ByVal v_sMessage As String)

        Dim sNewMessage As String = "[" & Date.Now & "] " & v_sMessage

        Trace.WriteLineIf(v_oTraceSetting.Level >= v_oLogLevel, sNewMessage)

        ' We'll just flush it anyway
        If (Trace.AutoFlush = False) Then
            Trace.Flush()
        End If

    End Sub

    ' Indent or unindent?
    Friend Shared Sub STSLogMessageIndent(ByVal v_bIndent As Boolean)

        If (v_bIndent = True) Then
            Trace.Indent()
        Else
            Trace.Unindent()
        End If

    End Sub

    Private Shared Function GetAddressXPathElements(ByVal con As SiriusConnection, ByVal DataModelCode As String, ByVal _oCache As System.Web.Caching.Cache) As String

        Dim sXPathElements As String = String.Empty
        Dim CacheKey As String = String.Empty

        Const NoAddressElementsFound As String = "NoAddressElementsFound"

        _oCache = HttpRuntime.Cache()

        ' Generate a Cache Key using the DataModelCode as we may have two DataModels
        CacheKey = "Address_XPathElements_" & DataModelCode

        ' Try to get the Full List from the Cache
        sXPathElements = CType(_oCache(CacheKey), String)

        If sXPathElements Is Nothing Then

            Dim dt As DataTable = Nothing

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Dataset_Address_Properties")
                cmd.AddInParameter("@GisDataModelCode", SqlDbType.Char, 10).Value = DataModelCode
                dt = con.ExecuteDataTable(cmd)
            End Using

            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    For Each oRow As DataRow In dt.Rows
                        If sXPathElements = "" Then
                            sXPathElements = sXPathElements & oRow.Item("XPathElements").ToString
                        Else
                            sXPathElements = sXPathElements & " | " & oRow.Item("XPathElements").ToString
                        End If
                    Next
                End If
            End If

            If sXPathElements Is Nothing Then
                sXPathElements = NoAddressElementsFound
            End If

            _oCache.Insert(CacheKey, sXPathElements)

        End If

        If sXPathElements = NoAddressElementsFound Then
            Return String.Empty
        Else
            Return sXPathElements
        End If

    End Function

    Private Shared Function GetAddressXPathAttributes(ByVal con As SiriusConnection, ByVal DataModelCode As String, ByVal _oCache As System.Web.Caching.Cache) As String

        Dim sXPathAttributes As String = String.Empty
        Dim CacheKey As String = String.Empty

        Const NoAddressAttributesFound As String = "NoAddressAttributesFound"

        _oCache = HttpRuntime.Cache()

        ' Generate a Cache Key using the DataModelCode as we may have two DataModels
        CacheKey = "Address_XPathAttributes_" & DataModelCode

        ' Try to get the Full List from the Cache
        sXPathAttributes = CType(_oCache(CacheKey), String)

        If sXPathAttributes Is Nothing Then

            Dim dt As DataTable = Nothing

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Dataset_Address_Properties")
                cmd.AddInParameter("@GisDataModelCode", SqlDbType.Char, 10).Value = DataModelCode
                dt = con.ExecuteDataTable(cmd)
            End Using

            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    For Each oRow As DataRow In dt.Rows
                        If sXPathAttributes = "" Then
                            sXPathAttributes = sXPathAttributes & oRow.Item("XPathAttributes").ToString
                        Else
                            sXPathAttributes = sXPathAttributes & " | " & oRow.Item("XPathAttributes").ToString
                        End If
                    Next
                End If
            End If

            If sXPathAttributes Is Nothing Then
                sXPathAttributes = NoAddressAttributesFound
            End If

            _oCache.Insert(CacheKey, sXPathAttributes)

        End If

        If sXPathAttributes = NoAddressAttributesFound Then
            Return String.Empty
        Else
            Return sXPathAttributes
        End If

    End Function

    ''' <summary>
    ''' This transforms the dataset from its Generic SAM format (i.e. without the Primary Key values or OI, US attributes
    ''' into the standard GIS/PB forat. 
    ''' The DTD is also added on.
    ''' The aim is that the output dataset can be loaded straight into the DatasetControl.
    ''' </summary>
    ''' <param name="XMLDataset"></param>
    ''' <remarks>Returns the transformed XML datatset.</remarks>
    ''' 

    Public Overloads Shared Function TransformDatasetSAMtoPB(ByVal XMLDataset As String, Optional ByVal SiriusUser As SIRIUSUSER = Nothing, Optional ByRef r_bRemoveAllSW As Boolean = False) As String

        If SiriusUser Is Nothing Then
            SiriusUser = New SIRIUSUSER
        End If

        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            SiriusUser.Username, SiriusUser.SourceID, _
                                            SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Dim oResponse As String = String.Empty

            oResponse = TransformDatasetSAMtoPB(con, XMLDataset, r_bRemoveAllSW)

            Return oResponse

        End Using

    End Function

    Private Shared Sub ProcessDatasetAddressesSAMToPB(ByVal con As SiriusConnection, ByRef xmldoc As XmlDocument, ByRef sNamespace As String, ByVal DataModelCode As String)

        Const ACMethodName As String = "ProcessDatasetAddressesSAMToPB"

        Dim sName As String = String.Empty
        Dim iAddressCnt As Integer
        Dim sUpdateStatus As String = String.Empty
        Dim _oCache As System.Web.Caching.Cache = HttpRuntime.Cache()
        Dim sXPath As String = GetAddressXPathElements(con, DataModelCode, _oCache)
        Dim underlyingObjectXMLNode As XmlNode

        If sXPath <> "" Then


            Dim navigator As XPathNavigator = xmldoc.CreateNavigator()
            Dim nodes As XPathNodeIterator = navigator.Select(sXPath)

            sNamespace = navigator.LookupNamespace(navigator.Prefix)

            While nodes.MoveNext
                sName = nodes.Current.Name

                sUpdateStatus = nodes.Current.GetAttribute("US", sNamespace)

                underlyingObjectXMLNode = DirectCast(nodes.Current.UnderlyingObject, XmlNode)

                navigator.MoveToId(underlyingObjectXMLNode.ParentNode.Attributes("OI").Value)
                iAddressCnt = XmlSafeConvert.ToInt32((navigator.GetAttribute(sName, sNamespace)), 0)
                navigator.MoveToChild(sName, sNamespace)

                ' If the update status is insert then
                If sUpdateStatus = "1" Then

                    Dim oCoreSAMBusiness As New CoreSAMBusiness
                    Dim oAddAddressRequest As New BaseImplementationTypes.BaseAddAddressRequestType
                    Dim oAddAddressRespone As BaseAddAddressResponseType

                    oAddAddressRequest.AddressTypeCode = BaseImplementationTypes.AddressTypeType.Other
                    oAddAddressRequest.AddressLine1 = navigator.GetAttribute("ADDRESS_LINE1", sNamespace)
                    oAddAddressRequest.AddressLine2 = navigator.GetAttribute("ADDRESS_LINE2", sNamespace)
                    oAddAddressRequest.AddressLine3 = navigator.GetAttribute("ADDRESS_LINE3", sNamespace)
                    oAddAddressRequest.AddressLine4 = navigator.GetAttribute("ADDRESS_LINE4", sNamespace)
                    oAddAddressRequest.PostCode = navigator.GetAttribute("POSTCODE", sNamespace)
                    oAddAddressRequest.CountryCode = navigator.GetAttribute("COUNTRYCODE", sNamespace)


                    ' Default the Country code as it is mandatory
                    If oAddAddressRequest.CountryCode = "" Then
                        oAddAddressRequest.CountryCode = "GBR"
                    End If
                    oAddAddressRequest.AddressLine5 = navigator.GetAttribute("ADDRESS_LINE5", sNamespace)
                    oAddAddressRequest.AddressLine6 = navigator.GetAttribute("ADDRESS_LINE6", sNamespace)
                    oAddAddressRequest.AddressLine7 = navigator.GetAttribute("ADDRESS_LINE7", sNamespace)
                    oAddAddressRequest.AddressLine8 = navigator.GetAttribute("ADDRESS_LINE8", sNamespace)
                    oAddAddressRequest.AddressLine9 = navigator.GetAttribute("ADDRESS_LINE9", sNamespace)
                    oAddAddressRequest.AddressLine10 = navigator.GetAttribute("ADDRESS_LINE10", sNamespace)




                    oAddAddressRespone = oCoreSAMBusiness.AddAddress(con, oAddAddressRequest)

                    If (oAddAddressRespone.STSError Is Nothing) = False Then
                        Dim STSErrorEX As New STSErrorPublisher
                        STSErrorEX.SetContext(oAddAddressRespone.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetSAMtoPB", True)
                        STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetSAMtoPB", True)
                    End If

                    iAddressCnt = oAddAddressRespone.AddressKey

                    navigator.MoveToParent()
                    If navigator.MoveToAttribute(sName, sNamespace) = False Then
                        Dim Attributes As XmlWriter = navigator.CreateAttributes
                        Attributes.WriteAttributeString(sName, sNamespace, iAddressCnt.ToString)
                        Attributes.Close()
                    Else
                        navigator.SetValue(iAddressCnt.ToString)
                    End If

                    ' If Update status is update then
                ElseIf sUpdateStatus = "2" Then

                    Dim oCoreSAMBusiness As New CoreSAMBusiness
                    Dim oAddAddressRequest As New BaseImplementationTypes.BaseAddAddressRequestType
                    Dim oAddAddressRespone As BaseAddAddressResponseType

                    oAddAddressRequest.AddressTypeCode = BaseImplementationTypes.AddressTypeType.Other
                    oAddAddressRequest.AddressLine1 = navigator.GetAttribute("ADDRESS_LINE1", sNamespace)
                    oAddAddressRequest.AddressLine2 = navigator.GetAttribute("ADDRESS_LINE2", sNamespace)
                    oAddAddressRequest.AddressLine3 = navigator.GetAttribute("ADDRESS_LINE3", sNamespace)
                    oAddAddressRequest.AddressLine4 = navigator.GetAttribute("ADDRESS_LINE4", sNamespace)
                    oAddAddressRequest.PostCode = navigator.GetAttribute("POSTCODE", sNamespace)
                    oAddAddressRequest.CountryCode = navigator.GetAttribute("COUNTRYCODE", sNamespace)
                    oAddAddressRequest.AddressLine5 = navigator.GetAttribute("ADDRESS_LINE5", sNamespace)
                    oAddAddressRequest.AddressLine6 = navigator.GetAttribute("ADDRESS_LINE6", sNamespace)
                    oAddAddressRequest.AddressLine7 = navigator.GetAttribute("ADDRESS_LINE7", sNamespace)
                    oAddAddressRequest.AddressLine8 = navigator.GetAttribute("ADDRESS_LINE8", sNamespace)
                    oAddAddressRequest.AddressLine9 = navigator.GetAttribute("ADDRESS_LINE9", sNamespace)
                    oAddAddressRequest.AddressLine10 = navigator.GetAttribute("ADDRESS_LINE10", sNamespace)



                    oAddAddressRespone = oCoreSAMBusiness.AddAddress(con, oAddAddressRequest)

                    If (oAddAddressRespone.STSError Is Nothing) = False Then
                        Dim STSErrorEX As New STSErrorPublisher
                        STSErrorEX.SetContext(oAddAddressRespone.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetSAMtoPB", True)
                        STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetSAMtoPB", True)
                    End If

                    iAddressCnt = oAddAddressRespone.AddressKey

                    navigator.MoveToParent()
                    If navigator.MoveToAttribute(sName, sNamespace) = False Then
                        Dim Attributes As XmlWriter = navigator.CreateAttributes
                        Attributes.WriteAttributeString(sName, sNamespace, iAddressCnt.ToString)
                        Attributes.Close()
                    Else
                        navigator.SetValue(iAddressCnt.ToString)
                    End If

                    ' If Update status is delete then
                ElseIf sUpdateStatus = "3" Then

                    'Remove the existing AddressCnt
                    navigator.MoveToId(nodes.Current.Value)
                    If navigator.MoveToAttribute(sName, sNamespace) = True Then
                        navigator.SetValue("")
                    End If

                End If

            End While

            navigator.MoveToRoot()

            'underlyingObjectXMLNode = DirectCast(navigator.UnderlyingObject, XmlNode)

            'XMLDataset = underlyingObjectXMLNode.OuterXml.ToString

        End If

    End Sub
    Private Shared Sub ProcessXMLDocumentType(ByRef XMLDataset As String, ByVal DataModelCode As String, ByVal xmldoc As XmlDocument, Optional ByVal IsMarketplacePolicy As Boolean = False)

        Const ACMethodName As String = "ProcessXMLDocumentType"

        Dim _oCache As System.Web.Caching.Cache = HttpRuntime.Cache()
        Dim CacheKey As String = String.Empty
        Dim iReturn As Integer

        ' Generate a Cache Key using the DataModelCode as we may have two DataModels
        ' TO DO: Convert this to string builder
        CacheKey = "DTD_" & DataModelCode

        ' Try to get the Full List from the Cache
        Dim doctype As XmlDocumentType
        doctype = CType(_oCache(CacheKey), XmlDocumentType)

        If doctype Is Nothing Then

            Dim xmldoctype As New XmlDocument

            Dim sDatasetsPath As String = "C:\Program Files\PM\GIS\Server\System\Datasets\"
            iReturn = GetPMRegSetting(PMERegSettingRoot.pmeRSRLocalMachine, PMEProductFamily.pmePFSiriusSolutions, PMERegSettingLevel.pmeRSLServer, "DataSetsPath", sDatasetsPath, "GIS\" & DataModelCode$)
            If iReturn <> PMEReturnCode.PMTrue Then
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.DatasetPathRegistrySettingNotFound, "GetPMRegSetting failed", "The registry setting for the datasets path does not exist for datamodel " & DataModelCode$)
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetSAMtoPB", True)
            End If

            sDatasetsPath = Cast.ToString(IIf(sDatasetsPath.EndsWith("\"), sDatasetsPath, sDatasetsPath & "\"), String.Empty)

            Dim sXMLDoc As String = My.Computer.FileSystem.ReadAllText(sDatasetsPath$ & DataModelCode$ & "DS.XML")

            xmldoctype.LoadXml(sXMLDoc)
            doctype = xmldoctype.DocumentType

            _oCache.Insert(CacheKey, doctype)

        End If

        Dim newdoctype As XmlDocumentType = xmldoc.CreateDocumentType(doctype.Name, Nothing, Nothing, doctype.InternalSubset.ToString)
        'If IsMarketplacePolicy = False Then
        If newdoctype IsNot Nothing AndAlso newdoctype.OuterXml IsNot Nothing AndAlso Not String.IsNullOrEmpty(newdoctype.OuterXml.ToString) Then
            XMLDataset = newdoctype.OuterXml.ToString + xmldoc.FirstChild.OuterXml.ToString
            xmldoc.LoadXml(XMLDataset)
        End If
        'Else
        '    xmldoc.InsertAfter(newdoctype, xmldoc.FirstChild)
        '    XMLDataset = xmldoc.OuterXml.ToString
        'End If
    End Sub

    '    Private Shared Sub ProcessDatasetSumsInsuredSAMtoPB(ByRef XMLDataset As String, ByVal sNamespace As String, ByVal DataModelCode As String, ByVal PolicyLinkID As Integer)
    Private Shared Sub ProcessDatasetSumsInsuredSAMtoPB(ByRef xmlDoc As XmlDocument, ByVal sNamespace As String, ByVal DataModelCode As String, ByVal PolicyLinkID As Integer)

        Const ACMethodName As String = "ProcessDatasetSumsInsuredSAMtoPB"

        Dim _oCache As System.Web.Caching.Cache = HttpRuntime.Cache()

        Dim sName As String = ""
        Dim sXPath As String = ""
        Dim sUpdateStatus As String = ""
        Dim iReturn As Integer = 0
        Dim CacheKey As String = ""

        Dim dr As DataSet = Nothing
        Dim iSumInsuredType As Integer
        Dim sTagName As String = String.Empty
        Dim oSIProps As DataRowCollection

        Dim arrSumsInsured(8, 0) As String
        Dim iCnt As Integer = 0

        _oCache = HttpRuntime.Cache()

        ' Generate a Cache Key using the DataModelCode as we may have two DataModels
        CacheKey = "SIProps_" & DataModelCode

        ' Try to get the Full List from the Cache
        oSIProps = CType(_oCache(CacheKey), DataRowCollection)

        If oSIProps Is Nothing Then

            Try

                ' BSJ April 09 - SQL Mixed Mode Compliance
                Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                    Using cmd As SiriusCommand = SiriusCommand.FromText("select upper(go.object_name), upper(gp.property_name), gp.specials_type_reference from gis_property gp inner join gis_object go on go.gis_object_id = gp.gis_object_id  where gp.specials_type = 4 order by gp.specials_type_reference")

                        dr = con.ExecuteDataSet(cmd, "dr")
                    End Using
                End Using

                'dr = SqlHelper.ExecuteDataset(ConnectionString, _
                '        CommandType.Text, _
                '        "select upper(go.object_name), upper(gp.property_name), gp.specials_type_reference from gis_property gp inner join gis_object go on go.gis_object_id = gp.gis_object_id  where gp.specials_type = 4 order by gp.specials_type_reference")
            Catch ex As Exception
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.FailedToConnectToTheSiriusDatabase, "ExecuteDataset failed", "Failed to get a list of data items containing Sums Insured details")
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetPBtoSAM", True)
            End Try

            If dr.Tables.Count = 0 Then
                oSIProps = Nothing
            Else
                oSIProps = dr.Tables(0).Rows
            End If

            Dim sDatasetSchemaFilename As String = String.Empty
            Dim sDatasetSchemaPath As String = String.Empty

            iReturn = GetPMRegSetting(PMERegSettingRoot.pmeRSRLocalMachine, PMEProductFamily.pmePFSiriusSolutions, PMERegSettingLevel.pmeRSLServer, "DataSetsPath", sDatasetSchemaPath, "GIS\" & DataModelCode)
            If iReturn <> PMEReturnCode.PMTrue Then
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.DatasetPathRegistrySettingNotFound, "GetPMRegSetting failed", "The registry setting for the datasets path does not exist for datamodel " & DataModelCode)
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetPBtoSAM", True)
            End If

            sDatasetSchemaPath = Cast.ToString(IIf((Right(sDatasetSchemaPath, 1) <> "\"), sDatasetSchemaPath & "\", sDatasetSchemaPath), String.Empty)
            sDatasetSchemaFilename = sDatasetSchemaPath & DataModelCode & "DS.XML"

            Dim dependency As New CacheDependency(sDatasetSchemaFilename)
            _oCache.Insert(CacheKey, oSIProps, dependency)

        End If

        If oSIProps Is Nothing = False Then

            Dim oRow As DataRow
            Dim sRate As String = String.Empty
            Dim sPremium As String = String.Empty


            Dim navigator As XPathNavigator = xmlDoc.CreateNavigator()

            navigator.MoveToRoot()

            For Each oRow In oSIProps

                sXPath = "//" & oRow.Item(0).ToString()
                iSumInsuredType = CInt(oRow.Item(2))
                sTagName = oRow.Item(1).ToString()

                Dim nodes As XPathNodeIterator = navigator.Select(sXPath)

                If nodes.MoveNext = True Then

                    sName = nodes.Current.Name
                    sUpdateStatus = nodes.Current.GetAttribute("US", sNamespace)
                    sRate = navigator.GetAttribute("RATE", sNamespace)
                    sPremium = navigator.GetAttribute("PREMIUM", sNamespace)

                    nodes.Current.MoveToAttribute("OI", "")
                    If sUpdateStatus <> "0" Then
                        If navigator.MoveToId(nodes.Current.Value) = True Then
                            If navigator.MoveToChild(sTagName, sNamespace) = True Then

                                If navigator.MoveToChild("SUM_INSURED", sNamespace) = True Then
                                    iCnt = -1
                                    Do 'Check condition at end of loop
                                        iCnt = iCnt + 1
                                        ReDim Preserve arrSumsInsured(8, iCnt)
                                        arrSumsInsured(0, iCnt) = navigator.GetAttribute("DESCRIPTION", sNamespace)
                                        arrSumsInsured(1, iCnt) = navigator.GetAttribute("REFERENCE", sNamespace)
                                        arrSumsInsured(2, iCnt) = navigator.GetAttribute("SUM_INSURED", sNamespace)
                                        arrSumsInsured(3, iCnt) = navigator.GetAttribute("DATE_ADDED", sNamespace)
                                        arrSumsInsured(4, iCnt) = navigator.GetAttribute("DATE_DELETED", sNamespace)
                                        arrSumsInsured(5, iCnt) = navigator.GetAttribute("IS_VALUATION_REQUIRED", sNamespace)
                                        arrSumsInsured(6, iCnt) = navigator.GetAttribute("VALUATION_DATE", sNamespace)
                                        arrSumsInsured(7, iCnt) = sRate
                                        arrSumsInsured(8, iCnt) = sPremium
                                    Loop While navigator.MoveToNext("SUM_INSURED", sNamespace) = True

                                    UpdateSumInsured(DataModelCode, PolicyLinkID, iSumInsuredType, arrSumsInsured)

                                End If
                            End If
                        End If
                    End If
                End If
            Next

            navigator.MoveToRoot()

            'Dim underlyingObjectXMLNode As XmlNode = DirectCast(navigator.UnderlyingObject, XmlNode)
            'XMLDataset = underlyingObjectXMLNode.OuterXml.ToString

        End If
    End Sub

    Private Shared Sub ProcessDatasetStandardWordingsSAMtoPB(ByVal con As SiriusConnection, ByRef xmlDoc As XmlDocument, ByVal sNamespace As String, ByVal DataModelCode As String, ByVal PolicyBinderID As Integer, Optional ByRef r_bRemoveAllSW As Boolean = False)

        Const ACMethodName As String = "ProcessDatasetSumsInsuredSAMtoPB"

        Const StandardWordingPropertiesDoNotExist As String = "PROPERTIESDONOTEXIST"
        Const StandardWordingPropertiesExist As String = "PROPERTIESDOEXIST"

        Dim _oCache As System.Web.Caching.Cache = HttpRuntime.Cache()

        Dim sName As String = ""
        Dim sUpdateStatus As String = ""
        Dim iReturn As Integer = 0
        Dim CacheKey As String = ""
        Dim CacheExistsKey As String = String.Empty

        'Dim dr As DataSet =Nothing
        'Dim iSumInsuredType As Integer
        Dim oSWProps As DataRowCollection = Nothing

        Dim arrSumsInsured(8, 0) As String
        Dim iCnt As Integer = 0

        _oCache = HttpRuntime.Cache()

        ' Generate a Cache Key using the DataModelCode as we may have two DataModels
        CacheExistsKey = "ExistsSWProps_" & DataModelCode.Trim.ToUpper
        CacheKey = "SWProps_" & DataModelCode.Trim.ToUpper

        Dim cacheExistsValue As String = CType(_oCache(CacheExistsKey), String)
        Dim dt As DataTable = Nothing
        If cacheExistsValue <> StandardWordingPropertiesExist AndAlso _
            cacheExistsValue <> StandardWordingPropertiesDoNotExist Then

            'If oSWProps Is Nothing AndAlso String.IsNullOrEmpty(cacheExistsValue) Then

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Standard_Wording_Properties")
                cmd.AddInParameter("@gis_datamodel_code", SqlDbType.Char, 30).Value = DataModelCode
                dt = con.ExecuteDataTable(cmd)
            End Using

            If dt IsNot Nothing Then
                If dt.Rows.Count = 0 Then
                    oSWProps = Nothing
                Else
                    oSWProps = dt.Rows
                End If
            End If

            Dim sDatasetSchemaFilename As String = String.Empty
            Dim sDatasetSchemaPath As String = String.Empty

            iReturn = GetPMRegSetting(PMERegSettingRoot.pmeRSRLocalMachine, PMEProductFamily.pmePFSiriusSolutions, PMERegSettingLevel.pmeRSLServer, "DataSetsPath", sDatasetSchemaPath, "GIS\" & DataModelCode)
            If iReturn <> PMEReturnCode.PMTrue Then
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.DatasetPathRegistrySettingNotFound, "GetPMRegSetting failed", "The registry setting for the datasets path does not exist for datamodel " & DataModelCode)
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetPBtoSAM", True)
            End If

            sDatasetSchemaPath = Cast.ToString(IIf((Right(sDatasetSchemaPath, 1) <> "\"), sDatasetSchemaPath & "\", sDatasetSchemaPath), String.Empty)
            sDatasetSchemaFilename = sDatasetSchemaPath & DataModelCode & "DS.XML"

            If oSWProps Is Nothing Then

                _oCache.Insert(CacheExistsKey, StandardWordingPropertiesDoNotExist)
                cacheExistsValue = StandardWordingPropertiesDoNotExist

            Else
                Dim dependency As New CacheDependency(sDatasetSchemaFilename)
                _oCache.Insert(CacheKey, oSWProps, dependency)
                _oCache.Insert(CacheExistsKey, StandardWordingPropertiesExist)
                cacheExistsValue = StandardWordingPropertiesExist
            End If

        Else

            ' Try to get the Full List from the Cache
            oSWProps = CType(_oCache(CacheKey), DataRowCollection)
            If oSWProps Is Nothing Then
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Standard_Wording_Properties")
                    cmd.AddInParameter("@gis_datamodel_code", SqlDbType.Char, 30).Value = DataModelCode
                    dt = con.ExecuteDataTable(cmd)
                End Using

                If dt IsNot Nothing Then
                    If dt.Rows.Count = 0 Then
                        oSWProps = Nothing
                    Else
                        oSWProps = dt.Rows
                    End If
                End If
            End If
        End If
        Dim oRow As DataRow



        Dim navigator As XPathNavigator = xmlDoc.CreateNavigator()

        navigator.MoveToRoot()
        If oSWProps IsNot Nothing Then
            For Each oRow In oSWProps

                Dim sXPath As String = "//" & oRow.Item(0).ToString()
                Dim ObjectName As String = oRow.Item(0).ToString()
                Dim PropertyName As String = oRow.Item(1).ToString()
                PropertyName = "SW." + PropertyName
                Dim ChildObject As Integer = Cast.ToInt32(oRow.Item(2), 0)
                Dim TableName As String = oRow.Item(3).ToString()
                Dim sOI As String = ""
                Dim oxmlnodes As XmlNodeList = xmlDoc.SelectNodes(sXPath)
                For Each oxmlnode As XmlNode In oxmlnodes
                    If oxmlnode.Attributes("OI") IsNot Nothing Then

                        sOI = oxmlnode.Attributes("OI").Value
                        Dim nodes As XPathNodeIterator = navigator.Select(sXPath & "[@OI='" & sOI & "']")
                        If nodes.MoveNext = True Then

                            sName = nodes.Current.Name
                            sUpdateStatus = nodes.Current.GetAttribute("US", sNamespace)

                            nodes.Current.MoveToAttribute("OI", "")
                            Dim OIKey As String = nodes.Current.Value
                            If sUpdateStatus <> "0" Then

                                'Delete existing Standard Wording entries for this property
                                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Delete_Standard_Wording")
                                    cmd.AddInParameter("@gis_datamodel_code", SqlDbType.VarChar, 30).Value = DataModelCode
                                    cmd.AddInParameter("@gis_policy_binder_id", SqlDbType.Int).Value = PolicyBinderID
                                    cmd.AddInParameter("@gis_object_name", SqlDbType.VarChar, 255).Value = ObjectName
                                    cmd.AddInParameter("@gis_property_name", SqlDbType.VarChar, 255).Value = PropertyName.Substring(3)
                                    If ChildObject = 1 Then
                                        cmd.AddInParameter("@parent_key_name", SqlDbType.VarChar, 255).Value = TableName & "_ID" 'DataModelCode & "_" & ObjectName & "_ID"
                                        cmd.AddInParameter("@parent_key_value", SqlDbType.VarChar, 255).Value = OIKey.Substring(2)
                                    End If
                                    iReturn = con.ExecuteNonQuery(cmd)
                                End Using
                                If sUpdateStatus <> "3" Then
                                    If navigator.MoveToId(nodes.Current.Value) = True Then
                                        If navigator.MoveToChild(PropertyName, sNamespace) = True Then
                                            '                                    If navigator.MoveToChild("CODE", sNamespace) = True Then
                                            iCnt = 0
                                            Do 'Check condition at end of loop
                                                iCnt = iCnt + 1
                                                If Not (String.IsNullOrEmpty(navigator.GetAttribute("CODE", sNamespace))) Then
                                                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Insert_Standard_Wording")
                                                        cmd.AddInParameter("@gis_datamodel_code", SqlDbType.VarChar, 30).Value = DataModelCode
                                                        cmd.AddInParameter("@gis_policy_binder_id", SqlDbType.Int).Value = PolicyBinderID
                                                        cmd.AddInParameter("@gis_object_name", SqlDbType.VarChar, 255).Value = ObjectName
                                                        cmd.AddInParameter("@gis_property_name", SqlDbType.VarChar, 255).Value = PropertyName.Substring(3)
                                                        cmd.AddInParameter("@document_template_code", SqlDbType.VarChar, 255).Value = navigator.GetAttribute("CODE", sNamespace)
                                                        If Not String.IsNullOrEmpty(navigator.GetAttribute("ID", sNamespace)) Then
                                                            cmd.AddInParameter("@documenttemplateid", SqlDbType.Int).Value = Convert.ToInt32(navigator.GetAttribute("ID", sNamespace))
                                                        Else
                                                            cmd.AddInParameter("@documenttemplateid", SqlDbType.Int).Value = 0
                                                        End If
                                                        cmd.AddInParameter("@Sequence", SqlDbType.Int).Value = iCnt%
                                                        If ChildObject = 1 Then
                                                            cmd.AddInParameter("@parent_key_name", SqlDbType.VarChar, 255).Value = TableName & "_ID" 'DataModelCode & "_" & ObjectName & "_ID"
                                                            cmd.AddInParameter("@parent_key_value", SqlDbType.VarChar, 255).Value = OIKey.Substring(2)
                                                        End If
                                                        iReturn = con.ExecuteNonQuery(cmd)
                                                    End Using
                                                Else
                                                    r_bRemoveAllSW = True
                                                End If
                                            Loop While navigator.MoveToNext(PropertyName, sNamespace) = True
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next oxmlnode
            Next oRow
        End If

        navigator.MoveToRoot()

        'Dim underlyingObjectXMLNode As XmlNode = DirectCast(navigator.UnderlyingObject, XmlNode)
        'XMLDataset = underlyingObjectXMLNode.OuterXml.ToString

    End Sub

    Friend Overloads Shared Function TransformDatasetSAMtoPB(ByVal con As SiriusConnection, ByVal XMLDataset As String, Optional ByRef r_bRemoveAllSW As Boolean = False, Optional ByVal IsMarketplacePolicy As Boolean = False) As String

        Const ACMethodName As String = "TransformDatasetSAMtoPB"

        Dim sName As String = String.Empty
        Dim iValue As Integer = 0
        Dim iAddressCnt As Integer = 0
        Dim sNamespace As String = String.Empty
        Dim sUpdateStatus As String = String.Empty
        'Dim iReturn As Integer
        Dim DataModelCode As String = String.Empty
        Dim PolicyLinkID As Integer
        Dim PolicyBinderID As Integer

        Dim xmldoc As New XmlDocument
        Dim _oCache As System.Web.Caching.Cache = HttpRuntime.Cache()
        'Dim CacheKey As String = String.Empty

        If XMLDataset <> "" Then

            'Output the trace xml 
            'DebugTraceXML("SAMtoPBIn.xml", XMLDataset)

            xmldoc = New XmlDocument
            xmldoc.LoadXml(XMLDataset)
            Try
                DataModelCode = xmldoc.SelectSingleNode("DATA_SET").Attributes("DataModelCode").Value
            Catch ex As Exception
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.XmldatasetBadlyFormed, ACMethodName & " Failed", "Failed to retrieve the DataModelCode attribute from the DATA_SET Element in the XMLDataset.")
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName & " Failed", True)
            End Try

            Try
                PolicyLinkID = XmlSafeConvert.ToInt32(xmldoc.SelectSingleNode("DATA_SET").Item("RISK_OBJECTS").Item(DataModelCode.ToUpper & "_POLICY_BINDER").GetAttribute("GIS_POLICY_LINK_ID", ""), 0)
            Catch ex As Exception
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.XmldatasetBadlyFormed, ACMethodName & " Failed", "Failed to retrieve the GIS_POLICY_LINK_ID attribute from the " & DataModelCode.ToUpper & "_POLICY_BINDER Element in the XMLDataset.")
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName & " Failed", True)
            End Try

            Try
                PolicyBinderID = XmlSafeConvert.ToInt32(xmldoc.SelectSingleNode("DATA_SET").Item("RISK_OBJECTS").Item(DataModelCode.ToUpper & "_POLICY_BINDER").GetAttribute(DataModelCode.ToUpper & "_POLICY_BINDER_ID", ""), 0)
            Catch ex As Exception
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.XmldatasetBadlyFormed, ACMethodName & " Failed", "Failed to retrieve the POLICY_BINDER_ID attribute from the " & DataModelCode.ToUpper & "_POLICY_BINDER Element in the XMLDataset.")
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName & " Failed", True)
            End Try

            If DataModelCode = String.Empty Then
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.XmldatasetBadlyFormed, ACMethodName & " Failed", "Failed to retrieve the DataModelCode attribute from the DATA_SET Element in the XMLDataset.")
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName & " Failed", True)
            ElseIf PolicyBinderID = -1 Then
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.XmldatasetBadlyFormed, ACMethodName & " Failed", "Failed to retrieve the GIS_POLICY_LINK_ID attribute from the " & DataModelCode.ToUpper & "_POLICY_BINDER Element in the XMLDataset.")
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName & " Failed", True)
            End If

            If xmldoc.DocumentType Is Nothing Then
                'xmldoc = New XmlDocument
                'xmldoc.LoadXml(XMLDataset$)
                ProcessXMLDocumentType(XMLDataset, DataModelCode, xmldoc, IsMarketplacePolicy)
            End If

            ProcessDatasetAddressesSAMToPB(con, xmldoc, sNamespace, DataModelCode)

            ProcessDatasetSumsInsuredSAMtoPB(xmldoc, sNamespace, DataModelCode, PolicyLinkID)

            ProcessDatasetStandardWordingsSAMtoPB(con, xmldoc, sNamespace, DataModelCode, PolicyBinderID, r_bRemoveAllSW)

            'DebugTraceXML("SAMtoPBOut.xml", navigator.OuterXml.ToString)

        End If
        XMLDataset = xmldoc.OuterXml

        ' Return the PB Dataset
        Return XMLDataset

    End Function

    Public Overloads Shared Function TransformDatasetPBtoSAM(ByVal XMLDataset As String, Optional ByVal SiriusUser As SIRIUSUSER = Nothing, Optional ByVal bSkipSaveToDB As Boolean = False, Optional ByVal sCalledVia As String = "", Optional ByVal iRiskTypeID As Integer = 0, Optional ByVal iSourceID As Integer = 0, Optional ByVal bRemoveAllSW As Boolean = False) As String

        If SiriusUser Is Nothing Then
            SiriusUser = New SIRIUSUSER
        End If
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                            SiriusUser.Username, SiriusUser.SourceID, _
                                            SiriusUser.LanguageID, _
                                            SiriusUserDefaults.AppName)
            Dim oResponse As String = String.Empty

            oResponse = TransformDatasetPBtoSAM(con, XMLDataset, bSkipSaveToDB, sCalledVia, iRiskTypeID, iSourceID, bRemoveAllSW)

            Return oResponse

        End Using

    End Function

    Friend Overloads Shared Sub InitialisePRE(ByVal Con As SiriusConnection, ByVal oGISQEMPRE As bGISQEMDRE.DRE, ByVal SiriusUser As SIRIUSUSER, Optional ByVal sBranchCode As String = "")

        Dim iRet As Int32
        Const ACMethodName As String = "InitialisePRE"

        Dim oDatabase As Object = Nothing
        If Not Con Is Nothing Then
            oDatabase = Con.PMDAODatabase
        End If

        Try
            iRet = CInt(oGISQEMPRE.Initialise(SiriusUser.Username, SiriusUser.Password, SiriusUser.UserID, SiriusUser.SourceID, SiriusUser.LanguageID, SiriusUser.CurrencyID, SiriusUserDefaults.LogLevel, SiriusUserDefaults.AppName, False, oDatabase))
        Catch ex As Exception
            Dim STSError As New STSErrorPublisher("oGISQEMPRE.Initialise failed", ex)
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialisePREException", True)
        End Try

        If iRet <> PMEReturnCode.PMTrue Then
            Dim STSError As New STSErrorPublisher(iRet, "bGISQEMDRE.DRE.Initialise failed")
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialisePREReturn", True)
        End If

    End Sub

    Friend Overloads Shared Function TransformDatasetPBtoSAM(ByVal con As SiriusConnection, ByVal XMLDataset As String, Optional ByVal bSkipSaveToDB As Boolean = False, Optional ByVal sCalledVia As String = "", Optional ByVal iRiskTypeID As Integer = 0, Optional ByVal iSourceID As Integer = 0, Optional ByVal bRemoveAllSW As Boolean = False) As String

        Const ACMethodName As String = "TransformDatasetPBtoSAM"
        ' Output UnTranformed Risk Dataset
        'DebugTraceXML("PBtoSAMIn.xml", XMLDataset)

        Dim sName As String = String.Empty
        Dim iValue As Integer = 0

        Dim sAddressLine1 As String = String.Empty
        Dim sAddressLine2 As String = String.Empty
        Dim sAddressLine3 As String = String.Empty
        Dim sAddressLine4 As String = String.Empty
        Dim sAddressLine5 As String = String.Empty
        Dim sAddressLine6 As String = String.Empty
        Dim sAddressLine7 As String = String.Empty
        Dim sAddressLine8 As String = String.Empty
        Dim sAddressLine9 As String = String.Empty
        Dim sAddressLine10 As String = String.Empty
        Dim sPostCode As String = String.Empty
        Dim sCountryCode As String = String.Empty
        Dim underlyingObjectXMLNode As XmlNode
        Dim xmldoc As New XmlDocument
        Dim _oCache As System.Web.Caching.Cache = Nothing
        Dim CacheKey As String = String.Empty

        If XMLDataset$ <> "" Then


            xmldoc.LoadXml(XMLDataset$)

            Dim DataModelCode As String = xmldoc.SelectSingleNode("DATA_SET").Attributes("DataModelCode").Value
            Dim PolicyLinkID As Integer = XmlSafeConvert.ToInt32(xmldoc.SelectSingleNode("DATA_SET").Item("RISK_OBJECTS").Item(DataModelCode.ToUpper & "_POLICY_BINDER").GetAttribute("GIS_POLICY_LINK_ID", ""), 0)
            Dim PolicyBinderID As Integer = XmlSafeConvert.ToInt32(xmldoc.SelectSingleNode("DATA_SET").Item("RISK_OBJECTS").Item(DataModelCode.ToUpper & "_POLICY_BINDER").GetAttribute(DataModelCode.ToUpper & "_POLICY_BINDER_ID", ""), 0)

            If DataModelCode = String.Empty Then
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.XmldatasetBadlyFormed, "TransformDatasetPBtoSAM Failed", "Failed to retrieve the DataModelCode attribute from the DATA_SET Element in the XMLDataset.")
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetSAMtoPB", True)
            ElseIf PolicyLinkID = -1 Then
                Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.XmldatasetBadlyFormed, "TransformDatasetPBtoSAM Failed", "Failed to retrieve the GIS_POLICY_LINK_ID attribute from the " & DataModelCode.ToUpper & "_POLICY_BINDER Element in the XMLDataset.")
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetSAMtoPB", True)
            End If

            Dim sXPath As String = GetAddressXPathAttributes(con, DataModelCode, _oCache)

            If sXPath <> "" Then

                'Dim document As XmlDocument = New XmlDocument
                'document.Load(New StringReader(XMLDataset))
                Dim navigator As XPathNavigator = xmldoc.CreateNavigator()
                Dim nodes As XPathNodeIterator = navigator.Select(sXPath)

                Dim oCoreSAMBusiness As New CoreSAMBusiness
                Dim oGetAddressRequest As New BaseImplementationTypes.BaseGetAddressRequestType
                Dim oGetAddressRespone As BaseGetAddressResponseType

                While nodes.MoveNext
                    sName = nodes.Current.Name
                    iValue = nodes.Current.ValueAsInt

                    nodes.Current.MoveToParent()
                    nodes.Current.MoveToAttribute("OI", "")
                    navigator.MoveToId(nodes.Current.Value)


                    oGetAddressRequest.AddressKey = iValue
                    oGetAddressRequest.BranchCode = "HeadOff"

                    oGetAddressRespone = oCoreSAMBusiness.GetAddress(oGetAddressRequest)

                    If (oGetAddressRespone.STSError Is Nothing) = False Then
                        Dim STSErrorEX As New STSErrorPublisher
                        STSErrorEX.SetContext(oGetAddressRespone.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetSAMtoPB", True)
                        STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetSAMtoPB", True)
                    End If

                    sAddressLine1 = oGetAddressRespone.Address.AddressLine1
                    sAddressLine2 = oGetAddressRespone.Address.AddressLine2
                    sAddressLine3 = oGetAddressRespone.Address.AddressLine3
                    sAddressLine4 = oGetAddressRespone.Address.AddressLine4
                    sPostCode = oGetAddressRespone.Address.PostCode
                    sCountryCode = oGetAddressRespone.Address.CountryCode
                    sAddressLine5 = oGetAddressRespone.Address.AddressLine5
                    sAddressLine6 = oGetAddressRespone.Address.AddressLine6
                    sAddressLine7 = oGetAddressRespone.Address.AddressLine7
                    sAddressLine8 = oGetAddressRespone.Address.AddressLine8
                    sAddressLine9 = oGetAddressRespone.Address.AddressLine9
                    sAddressLine10 = oGetAddressRespone.Address.AddressLine10

                    If bSkipSaveToDB = False Then
                        navigator.AppendChildElement(navigator.Prefix, sName, navigator.LookupNamespace(navigator.Prefix), "")
                        navigator.MoveToChild(sName, navigator.LookupNamespace(navigator.Prefix))

                        Dim Attributes As XmlWriter = navigator.CreateAttributes
                        Attributes.WriteAttributeString("US", "0")
                        Attributes.WriteAttributeString("ADDRESS_LINE1", sAddressLine1)
                        Attributes.WriteAttributeString("ADDRESS_LINE2", sAddressLine2)
                        Attributes.WriteAttributeString("ADDRESS_LINE3", sAddressLine3)
                        Attributes.WriteAttributeString("ADDRESS_LINE4", sAddressLine4)
                        Attributes.WriteAttributeString("POSTCODE", sPostCode)
                        Attributes.WriteAttributeString("COUNTRYCODE", sCountryCode)
                        Attributes.WriteAttributeString("ADDRESS_LINE5", sAddressLine5)
                        Attributes.WriteAttributeString("ADDRESS_LINE6", sAddressLine6)
                        Attributes.WriteAttributeString("ADDRESS_LINE7", sAddressLine7)
                        Attributes.WriteAttributeString("ADDRESS_LINE8", sAddressLine8)
                        Attributes.WriteAttributeString("ADDRESS_LINE9", sAddressLine9)
                        Attributes.WriteAttributeString("ADDRESS_LINE10", sAddressLine10)


                        Attributes.Close()
                    End If
                End While

                navigator.MoveToRoot()

                'underlyingObjectXMLNode = DirectCast(navigator.UnderlyingObject, XmlNode)

                'XMLDataset = underlyingObjectXMLNode.OuterXml.ToString

            End If

            Dim dr As DataSet = Nothing
            Dim iSumsInsuredType As Integer
            Dim sChildNode As String = String.Empty
            Dim sTagName As String = String.Empty
            Dim oSIProps As DataRowCollection

            _oCache = HttpRuntime.Cache()

            ' Generate a Cache Key using the DataModelCode as we may have two DataModels 
            CacheKey = "SIProps_" & DataModelCode

            ' Try to get the Full List from the Cache
            oSIProps = CType(_oCache(CacheKey), DataRowCollection)

            If oSIProps Is Nothing Then

                Try

                    ' BSJ April 09 - SQL Mixed Mode Compliance
                    Using cmd As SiriusCommand = SiriusCommand.FromText("select upper(go.object_name), upper(gp.property_name), gp.specials_type_reference from gis_property gp inner join gis_object go on go.gis_object_id = gp.gis_object_id  where gp.specials_type = 4 order by gp.specials_type_reference")

                        dr = con.ExecuteDataSet(cmd, "dr")
                    End Using

                    'dr = SqlHelper.ExecuteDataset(ConnectionString, _
                    '        CommandType.Text, _
                    '        "select upper(go.object_name), upper(gp.property_name), gp.specials_type_reference from gis_property gp inner join gis_object go on go.gis_object_id = gp.gis_object_id  where gp.specials_type = 4 order by gp.specials_type_reference")
                Catch ex As Exception
                    Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.FailedToConnectToTheSiriusDatabase, "ExecuteDataset failed", "Failed to get a list of data items containing Sums Insured details")
                    STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetPBtoSAM", True)
                End Try

                If dr.Tables.Count = 0 Then
                    oSIProps = Nothing
                Else
                    oSIProps = dr.Tables(0).Rows
                End If

                Dim sDatasetSchemaFilename As String = String.Empty
                Dim sDatasetSchemaPath As String = String.Empty
                Dim iReturn As Integer

                iReturn = GetPMRegSetting(PMERegSettingRoot.pmeRSRLocalMachine, PMEProductFamily.pmePFSiriusSolutions, PMERegSettingLevel.pmeRSLServer, "DataSetsPath", sDatasetSchemaPath, "GIS\" & DataModelCode)
                If iReturn <> PMEReturnCode.PMTrue Then
                    Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.DatasetPathRegistrySettingNotFound, "GetPMRegSetting failed", "The registry setting for the datasets path does not exist for datamodel " & DataModelCode)
                    STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "TransformDatasetPBtoSAM", True)
                End If

                sDatasetSchemaPath = Cast.ToString(IIf((Right(sDatasetSchemaPath, 1) <> "\"), sDatasetSchemaPath & "\", sDatasetSchemaPath), String.Empty)
                sDatasetSchemaFilename = sDatasetSchemaPath & DataModelCode & "DS.XML"

                Dim dependency As New CacheDependency(sDatasetSchemaFilename)
                _oCache.Insert(CacheKey, oSIProps, dependency)

            End If

            If oSIProps Is Nothing = False Then

                Dim oRow As DataRow

                'Dim document As New XmlDocument
                'document.LoadXml(XMLDataset$)

                Dim navigator As XPathNavigator = xmldoc.CreateNavigator()

                navigator.MoveToRoot()

                For Each oRow In oSIProps

                    sXPath = "//" & oRow.Item(0).ToString()
                    iSumsInsuredType = CInt(oRow.Item(2))
                    sTagName = oRow.Item(1).ToString()

                    Dim nodes As XPathNodeIterator = navigator.Select(sXPath)

                    If nodes.MoveNext = True Then

                        sName = nodes.Current.Name

                        nodes.Current.MoveToAttribute("OI", "")
                        navigator.MoveToId(nodes.Current.Value)

                        'exec spu_STS_Get_Sum_Insured 'HOMEOWNERS', 1341, 1, 'HIGHVAL'
                        Dim dsSumsIns As DataSet = Nothing

                        ' BSJ April 09 - SQL Mixed Mode Compliance
                        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Sum_Insured")
                            cmd.AddInParameter("@gis_datamodel_code", SqlDbType.VarChar, 30).Value = DataModelCode
                            cmd.AddInParameter("@gis_policy_link_id", SqlDbType.Int).Value = PolicyLinkID
                            cmd.AddInParameter("@sum_insured_type_id", SqlDbType.Int).Value = iSumsInsuredType
                            cmd.AddInParameter("@tagname", SqlDbType.VarChar, 255).Value = sTagName

                            dsSumsIns = con.ExecuteDataSet(cmd, "dsSumsIns")
                        End Using

                        'Dim arParams(3) As SqlParameter
                        'arParams(0) = New SqlParameter("@gis_datamodel_code", DataModelCode)
                        'arParams(1) = New SqlParameter("@gis_policy_link_id", PolicyLinkID)
                        'arParams(2) = New SqlParameter("@sum_insured_type_id", iSumsInsuredType)
                        'arParams(3) = New SqlParameter("@tagname", sTagName)
                        '
                        'dsSumsIns = SqlHelper.ExecuteDataset(ConnectionString, _
                        '    CommandType.StoredProcedure, _
                        '    "spu_SAM_Get_Sum_Insured", _
                        '    arParams)

                        'Append the XML returned from the SP as a child node
                        If dsSumsIns.Tables(0).Rows.Count > 0 Then
                            sChildNode = Cast.ToString(dsSumsIns.Tables(0).Rows(0).Item(0), String.Empty)
                            navigator.AppendChild(sChildNode)
                        End If

                    End If

                Next

                navigator.MoveToRoot()

                'underlyingObjectXMLNode = DirectCast(navigator.UnderlyingObject, XmlNode)

                'XMLDataset = underlyingObjectXMLNode.OuterXml.ToString

            End If

            Dim oSWProps As DataRowCollection = Nothing
            Dim ds As DataSet = Nothing
            Dim dt As DataTable = Nothing
            Dim sCacheKeySWO As String = ""

            ' Generate a Cache Key using the DataModelCode as we may have two DataModels 
            sCacheKeySWO = "SWO_" & DataModelCode.Trim.ToUpper

            ' Try to get the Full List from the Cache
            ds = CType(_oCache(sCacheKeySWO), DataSet)

            If ds Is Nothing Then
                'Get property name and other revelant fields details
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Standard_Wording_Objects")
                    cmd.AddInParameter("@gis_datamodel_code", SqlDbType.VarChar, 30).Value = DataModelCode
                    ds = con.ExecuteDataSet(cmd, "ROW")
                End Using
                If ds IsNot Nothing Then
                    _oCache.Insert(sCacheKeySWO, ds)
                End If
            End If
            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then

                        Dim DocumentCode As String = String.Empty
                        Dim ChildObject As Integer = 0
                        'Dim document As New XmlDocument
                        Dim docXML As New XmlDocument
                        Dim oNode As XmlNode
                        Dim sDocumentDescription As String = String.Empty
                        Dim sIO As String = ""
                        Dim DocumentTemplateID As String = String.Empty
                        'document.LoadXml(XMLDataset$)

                        Dim navigator As XPathNavigator = xmldoc.CreateNavigator()

                        navigator.MoveToRoot()
                        If ds.Tables(0).Rows.Count > 0 Then docXML.LoadXml(XMLDataset$)

                        For Each oRow As DataRow In ds.Tables(0).Rows
                            sXPath = "//" & oRow.Item("object_name").ToString.ToUpper.Trim
                            If sXPath.Length > 0 Then
                                'docXML.LoadXml(XMLDataset$)
                                Dim oxmlNodes As XmlNodeList = docXML.SelectNodes(sXPath)
                                For Each oxmlnode As XmlNode In oxmlNodes
                                    If oxmlnode IsNot Nothing Then
                                        If oxmlnode.Attributes("OI") IsNot Nothing Then
                                            sIO = oxmlnode.Attributes("OI").Value
                                            Dim nodes As XPathNodeIterator = navigator.Select(sXPath & "[@OI='" & sIO & "']")
                                            If sCalledVia = "AddRisk" Then
                                                xmldoc.SelectSingleNode(sXPath).Attributes("US").Value = "2"
                                            End If
                                            If nodes.MoveNext = True Then

                                                sName = oRow.Item("property_name").ToString.ToUpper.Trim
                                                sName = "SW." + sName
                                                oNode = xmldoc.SelectSingleNode(sXPath & "[@OI='" & sIO & "']/" & sName & "[@TYPE='SW-5']")

                                                nodes.Current.MoveToAttribute("OI", "")
                                                navigator.MoveToId(nodes.Current.Value)
                                                Dim OIKey As String = nodes.Current.Value
                                                Dim iReturn As Integer = 0
                                                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Standard_Wording_Values")
                                                    cmd.AddInParameter("@gis_datamodel_code", SqlDbType.VarChar, 30).Value = DataModelCode
                                                    cmd.AddInParameter("@gis_policy_binder_id", SqlDbType.Int).Value = PolicyBinderID
                                                    cmd.AddInParameter("@gis_object_name", SqlDbType.VarChar, 70).Value = oRow.Item("object_name").ToString()
                                                    cmd.AddInParameter("@OIKey", SqlDbType.VarChar, 20).Value = OIKey.Substring(2)
                                                    cmd.AddInParameter("@property_name", SqlDbType.VarChar, 70).Value = sName.Substring(3)
                                                    cmd.AddInParameter("@table_name", SqlDbType.VarChar, 70).Value = oRow.Item("table_name").ToString()
                                                    dt = con.ExecuteDataTable(cmd)
                                                End Using

                                                Dim dtAddRisk As DataTable
                                                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Clauses_Set_To_default_In_Risk")
                                                    cmd.AddInParameter("@risk_type_id", SqlDbType.Int).Value = iRiskTypeID
                                                    cmd.AddInParameter("@Source_id", SqlDbType.Int).Value = iSourceID
                                                    cmd.AddInParameter("@property_name", SqlDbType.VarChar, 70).Value = sName.Substring(3)
                                                    cmd.AddInParameter("@gis_object_name", SqlDbType.VarChar, 70).Value = oRow.Item("object_name").ToString()
                                                    dtAddRisk = con.ExecuteDataTable(cmd)
                                                End Using

                                                If dt IsNot Nothing Then
                                                    If dt.Rows.Count > 0 Then
                                                        If sCalledVia <> "SKIP" Then
                                                            For Each orRow As DataRow In dt.Rows
                                                                DocumentCode = RTrim(orRow.Item("DocumentCode").ToString)
                                                                sDocumentDescription = RTrim(orRow.Item("DESCRIPTION").ToString)
                                                                sDocumentDescription = sDocumentDescription.Replace("&", "&amp;")
                                                                DocumentTemplateID = RTrim(orRow.Item("document_template_id").ToString)
                                                                sChildNode = "<" & sName & " CODE=""" & DocumentCode & """" & " DESCRIPTION=""" & sDocumentDescription & """" & " ID=""" & DocumentTemplateID & """" & " TYPE=""" & "SW-5" & """" & " US=""" & "0" & """/>"
                                                                navigator.AppendChild(sChildNode)
                                                                DocumentCode = ""
                                                                sDocumentDescription = ""
                                                            Next
                                                        ElseIf sCalledVia = "SKIP" AndAlso dtAddRisk.Rows.Count > 0 AndAlso bRemoveAllSW = False AndAlso (oNode Is Nothing) Then
                                                            For Each oRDARiskRow As DataRow In dtAddRisk.Rows
                                                                DocumentCode = RTrim(oRDARiskRow.Item("Code").ToString)
                                                                sDocumentDescription = RTrim(oRDARiskRow.Item("DESCRIPTION").ToString)
                                                                sDocumentDescription = sDocumentDescription.Replace("&", "&amp;")
                                                                DocumentTemplateID = RTrim(oRDARiskRow.Item("document_template_id").ToString)
                                                                sChildNode = "<" & sName & " CODE=""" & DocumentCode & """" & " DESCRIPTION=""" & sDocumentDescription & """" & " ID=""" & DocumentTemplateID & """" & " TYPE=""" & "SW-5" & """" & " US=""" & "1" & """/>"
                                                                navigator.AppendChild(sChildNode)
                                                                DocumentCode = ""
                                                                sDocumentDescription = ""
                                                            Next
                                                        ElseIf sCalledVia = "SKIP" AndAlso dtAddRisk.Rows.Count = 0 AndAlso oNode Is Nothing Then
                                                            sChildNode = "<" & sName & " CODE=""" & DocumentCode & """" & " DESCRIPTION=""" & sDocumentDescription & """" & " ID=""" & DocumentTemplateID & """" & " TYPE=""" & "SW-5" & """" & " US=""" & "0" & """/>"
                                                            navigator.AppendChild(sChildNode)
                                                        End If
                                                    Else

                                                        If dtAddRisk IsNot Nothing Then
                                                            If (sCalledVia = "AddRisk" Or sCalledVia = "SaveRisk" Or sCalledVia = "SKIP") AndAlso dtAddRisk.Rows.Count > 0 AndAlso bRemoveAllSW = False Then
                                                                If oNode Is Nothing Then
                                                                    If (sCalledVia = "SKIP" Or sCalledVia = "AddRisk") Then
                                                                        For Each oAddRiskRow As DataRow In dtAddRisk.Rows
                                                                            DocumentCode = RTrim(oAddRiskRow.Item("Code").ToString)
                                                                            sDocumentDescription = RTrim(oAddRiskRow.Item("DESCRIPTION").ToString)
                                                                            sDocumentDescription = sDocumentDescription.Replace("&", "&amp;")
                                                                            DocumentTemplateID = RTrim(oAddRiskRow.Item("document_template_id").ToString)
                                                                            sChildNode = "<" & sName & " CODE=""" & DocumentCode & """" & " DESCRIPTION=""" & sDocumentDescription & """" & " ID=""" & DocumentTemplateID & """" & " TYPE=""" & "SW-5" & """" & " US=""" & "1" & """/>"
                                                                            navigator.AppendChild(sChildNode)
                                                                            DocumentCode = ""
                                                                            sDocumentDescription = ""
                                                                        Next
                                                                    End If
                                                                Else
                                                                    sChildNode = "<" & sName & " CODE=""" & DocumentCode & """" & " DESCRIPTION=""" & sDocumentDescription & """" & " ID=""" & DocumentTemplateID & """" & " TYPE=""" & "SW-5" & """" & " US=""" & "0" & """/>"
                                                                    navigator.AppendChild(sChildNode)
                                                                End If

                                                            Else 'put a check wheter an empty cell is already present or not
                                                                If oNode IsNot Nothing AndAlso oNode.Attributes("CODE").Value <> "" AndAlso (sCalledVia <> "SKIP") Then
                                                                    sChildNode = "<" & sName & " CODE=""" & DocumentCode & """" & " DESCRIPTION=""" & sDocumentDescription & """" & " ID=""" & DocumentTemplateID & """" & " TYPE=""" & "SW-5" & """" & " US=""" & "0" & """/>"
                                                                    navigator.AppendChild(sChildNode)
                                                                ElseIf oNode Is Nothing Then
                                                                    sChildNode = "<" & sName & " CODE=""" & DocumentCode & """" & " DESCRIPTION=""" & sDocumentDescription & """" & " ID=""" & DocumentTemplateID & """" & " TYPE=""" & "SW-5" & """" & " US=""" & "0" & """/>"
                                                                    navigator.AppendChild(sChildNode)
                                                                End If
                                                            End If
                                                            Else
                                                                sChildNode = "<" & sName & " CODE=""" & DocumentCode & """" & " DESCRIPTION=""" & sDocumentDescription & """" & " ID=""" & DocumentTemplateID & """" & " TYPE=""" & "SW-5" & """" & " US=""" & "0" & """/>"
                                                                navigator.AppendChild(sChildNode)
                                                            End If
                                                    End If
                                                Else
                                                    sChildNode = "<" & sName & " CODE=""" & DocumentCode & """" & " DESCRIPTION=""" & sDocumentDescription & """" & " ID=""" & DocumentTemplateID & """" & " TYPE=""" & "SW-5" & """" & " US=""" & "0" & """/>"
                                                    navigator.AppendChild(sChildNode)

                                                End If
                                                dt = Nothing
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        Next

                        navigator.MoveToRoot()

                        'underlyingObjectXMLNode = DirectCast(navigator.UnderlyingObject, XmlNode)

                        'XMLDataset = underlyingObjectXMLNode.OuterXml.ToString

                    End If
                End If
            End If

            If dt IsNot Nothing Then
                If dt.Rows.Count = 0 Then
                    oSWProps = Nothing
                Else
                    oSWProps = dt.Rows
                End If
            End If

            If oSWProps Is Nothing = False Then

                Dim DocumentCode As String = ""
                Dim ChildObject As Integer = 0
                ' Dim document As New XmlDocument

                'document.LoadXml(XMLDataset$)

                Dim navigator As XPathNavigator = xmldoc.CreateNavigator()

                navigator.MoveToRoot()

                For Each oRow As DataRow In oSWProps
                    '//GENERAL[@QBENZ_GENERAL_ID="700133539"]
                    If CInt(oRow.Item("child")) = 1 Then
                        Dim IdColumnName As String = DataModelCode & "_" & oRow.Item("object_name").ToString() & "_id"
                        sXPath = "//" & oRow.Item("object_name").ToString.ToUpper & "[@" & IdColumnName.ToUpper & "=" & oRow.Item(IdColumnName).ToString & "]"
                    Else
                        sXPath = "//" & oRow.Item("object_name").ToString()
                    End If

                    DocumentCode = RTrim(oRow.Item("DocumentCode").ToString)

                    Dim nodes As XPathNodeIterator = navigator.Select(sXPath)

                    If nodes.MoveNext = True Then

                        sName = oRow.Item("property_name").ToString.ToUpper.Trim

                        nodes.Current.MoveToAttribute("OI", "")
                        navigator.MoveToId(nodes.Current.Value)

                        sChildNode = "<" & sName & " CODE=""" & DocumentCode & """/>"
                        navigator.AppendChild(sChildNode)
                    End If

                Next

                navigator.MoveToRoot()

                'underlyingObjectXMLNode = DirectCast(navigator.UnderlyingObject, XmlNode)

                'XMLDataset = underlyingObjectXMLNode.OuterXml.ToString

            End If

        End If
        XMLDataset = xmldoc.OuterXml
        Return XMLDataset

    End Function

    Public Shared Sub UpdateSumInsured(ByVal DataModelCode As String, ByVal PolicyLinkID As Int32, ByVal iSumInsuredType As Integer, ByVal SumsInsured As Object)

        Const ACMethodName As String = "UpdateSumInsured"

        Dim iRet As Int32

        Dim bRiskScreen As bSIRRiskScreen.Business = Nothing
        Try
            bRiskScreen = New bSIRRiskScreen.Business
        Catch ex As Exception
            ExceptionManager.Publish(ex)
        Finally

        End Try

        ' Initialise the COM object
        Try
            iRet = CInt(bRiskScreen.Initialise("sirius", "sirius", SiriusUserDefaults.UserID, SiriusUserDefaults.SourceID, SiriusUserDefaults.LanguageID, SiriusUserDefaults.CurrencyID, 1, SiriusUserDefaults.AppName))
        Catch ex As Exception
            If bRiskScreen IsNot Nothing Then
                bRiskScreen.Dispose()
                bRiskScreen = Nothing
            End If
            ExceptionManager.Publish(ex)
            Throw
        End Try

        If (iRet <> 1) Then
            If bRiskScreen IsNot Nothing Then
                bRiskScreen.Dispose()
                bRiskScreen = Nothing
            End If
            Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.DatasetPathRegistrySettingNotFound, "bSIRRiskScreen.Initialise failed", "Failed to Initialise bRiskScreen. Return Value = " + iRet.ToString)
            STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "UpdateSumInsured", True)
        End If

        Try
            iRet = bRiskScreen.UpdateSumInsured(lPolicyLinkId:=PolicyLinkID, _
                                                sDataModel:=DataModelCode, _
                                                lSumInsuredType:=iSumInsuredType, _
                                                vSumInsuredArray:=SumsInsured)
        Catch ex As Exception
            If bRiskScreen IsNot Nothing Then
                bRiskScreen.Dispose()
                bRiskScreen = Nothing
            End If
            Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.DatasetPathRegistrySettingNotFound, "bSIRRiskScreen.SumInsured failed", "Failed to Update the Sum Insured. Return Value = " + iRet.ToString)
            STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "UpdateSumInsured", True)
        End Try

        If bRiskScreen IsNot Nothing Then
            bRiskScreen.Dispose()
            bRiskScreen = Nothing
        End If

    End Sub

    Friend Shared Function CheckSubBranch(ByVal BranchCode As String, ByVal SubBranchCode As String) As Boolean
        Dim ds As DataSet = Nothing

        ' BSJ April 09 - SQL Mixed Mode Compliance
        Try

            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_subbranch_check")
                    cmd.AddInParameter("@branch_code", SqlDbType.VarChar, 10).Value = BranchCode
                    cmd.AddInParameter("@subbranch_code", SqlDbType.VarChar, 10).Value = SubBranchCode

                    ds = con.ExecuteDataSet(cmd, "ds")
                End Using
            End Using

        Catch ex As Exception
            CheckSubBranch = False
            Throw
        End Try

        Try
            If ds.Tables(0).Rows.Count = 1 Then
                CheckSubBranch = True
            Else
                CheckSubBranch = False
            End If
        Catch ex As Exception
            CheckSubBranch = False
        End Try
    End Function

    Friend Shared Sub CheckForRefersAndDeclines(ByVal v_sXMLDataset As String, _
                                              ByVal v_sDataModelCode As String, _
                                              ByRef r_blReferred As Boolean, _
                                              ByRef r_blDeclined As Boolean)

        Dim oDomDocRisk As New System.Xml.XmlDocument ' MSXML2.DOMDocument
        Dim oDomNodeRisk As System.Xml.XmlNode ' MSXML2.IXMLDOMNode

        Dim oSTSError As STSErrorPublisher

        oSTSError = New STSErrorPublisher

        r_blReferred = False
        r_blDeclined = False

        ' Load the XMLDataset containing the Risk data
        Try
            oDomDocRisk.LoadXml(v_sXMLDataset)
        Catch ex As Exception
            ' Exception handling
            oSTSError = New STSErrorPublisher(STSErrorCodes.XmldatasetBadlyFormed, "XML dataset badly formed", "XML dataset badly formed")
        End Try

        Dim sRoot As String = String.Empty
        Dim sXpath As String = String.Empty

        ' Construct the XPath of the data items we need to find and substitute
        sRoot$ = [String].Format(InternalSAMConstants.ACMergeXPath, v_sDataModelCode.ToUpper)

        ' Construct the Xpath of the Refer Reason
        sXpath = sRoot$ & [String].Format(InternalSAMConstants.ACMergeOutputReferMessage, v_sDataModelCode.ToUpper)

        ' Find the correct node using the XPath of the dataitem
        oDomNodeRisk = oDomDocRisk.SelectSingleNode(sXpath)

        If (oDomNodeRisk Is Nothing) = False Then
            ' Set the value of the node
            If oDomNodeRisk.Value <> "" Then
                r_blReferred = True
            End If
        End If

        ' Construct the XPath of the data items we need to find and substitute
        sRoot$ = [String].Format(InternalSAMConstants.ACMergeXPath, v_sDataModelCode.ToUpper)

        ' Construct the Xpath of the Refer Reason
        sXpath = sRoot$ & [String].Format(InternalSAMConstants.ACMergeOutputDeclineMessage, v_sDataModelCode.ToUpper)

        ' Find the correct node using the XPath of the dataitem
        oDomNodeRisk = oDomDocRisk.SelectSingleNode(sXpath)

        If (oDomNodeRisk Is Nothing) = False Then
            ' Set the value of the node
            If oDomNodeRisk.Value <> "" Then
                r_blDeclined = True
            End If
        End If

        ' Construct the XPath of the data items we need to find and substitute
        sRoot$ = [String].Format(InternalSAMConstants.ACMergeXPath, v_sDataModelCode.ToUpper)

        ' Construct the Xpath of the Refer Reason
        sXpath = sRoot$ & [String].Format(InternalSAMConstants.ACMergeOutputStatus, v_sDataModelCode.ToUpper)

        ' Find the correct node using the XPath of the dataitem
        oDomNodeRisk = oDomDocRisk.SelectSingleNode(sXpath)

        If (oDomNodeRisk Is Nothing) = False Then
            ' Set the value of the node
            If oDomNodeRisk.Value = "DECLINE" Then
                r_blDeclined = True
            ElseIf oDomNodeRisk.Value = "REFER" Then
                r_blReferred = True
            End If
        End If

    End Sub
#Region " GetPMRegSetting"
    ' ***************************************************************** '
    ' Name: GetPMRegSetting
    '
    ' Description: Gets the value for the specified setting in the
    '              Registry at the specified location.
    '
    ' RegSettingRoot  = Local Machine Or Current User
    ' Software\PM     = Fixed location
    ' ProductFamily   = SiriusArchitecture Or Gemini Or Mercury etc
    ' RegSettinglevel = Client OR Server Or Common
    '
    ' e.g. HKEY_CURRENT_USER\software\PM\SiriusArchitecture\Client
    '
    ' ***************************************************************** '

    Friend Shared Function GetPMRegSetting(ByVal v_lPMERegSettingRoot As Integer, ByVal v_lPMEProductFamily As Integer, ByVal v_lPMERegSettingLevel As Integer, ByVal v_sSettingName As String, ByRef r_sSettingValue As String, Optional ByVal v_sSubKey As String = "") As Integer

        Dim bKeyExists As Boolean
        Dim sKeyString As String = String.Empty
        Dim lRoot As Integer
        Dim vSettingValue As Object = Nothing
        Dim rk As RegistryKey
        Dim _oCache As System.Web.Caching.Cache = Nothing
        Dim sCacheKey As String = String.Empty


        Try

            GetPMRegSetting = PMEReturnCode.PMTrue

            ' Current User OR Local Machine
            If (v_lPMERegSettingRoot = PMERegSettingRoot.pmeRSRCurrentUser) Then
                rk = Registry.CurrentUser
            Else
                rk = Registry.LocalMachine
            End If

            Dim localPMEProductFamily As PMEProductFamily = CType(v_lPMEProductFamily, PMEProductFamily)
            Dim localPMERegistrySettingLevel As PMERegSettingLevel = CType(v_lPMERegSettingLevel, PMERegSettingLevel)

            ' Build up the key String
            sKeyString = BuildKeyString(v_ePMEProductFamily:=localPMEProductFamily, v_ePMERegSettingLevel:=localPMERegistrySettingLevel, v_sSubKey:=v_sSubKey)

            ' Do we have a key string
            If (Trim(sKeyString) = "") Then
                GetPMRegSetting = PMEReturnCode.PMFalse
                Exit Function
            End If
            _oCache = HttpContext.Current.Cache()

            ' Generate a Cache Key using the DataModelCode as we may have two DataModels 
            sCacheKey = "RegSetting_" & sKeyString & "_setting_" & v_sSettingName

            If _oCache.Item(sCacheKey) Is Nothing = False Then
                ' Try to get the Full List from the Cache
                r_sSettingValue = CType(_oCache(sCacheKey), String)
            Else
                ' Get the Value
                Dim myRK As RegistryKey = rk.OpenSubKey(sKeyString)
                vSettingValue = myRK.GetValue(v_sSettingName)

                ' If the Setting Value Is Null, Empty or Nothing

                If (IsNothing(vSettingValue) = True) Then
                    ' Return an Empty String
                    r_sSettingValue = ""
                Else
                    ' Otherwise, Return the Setting Value
                    r_sSettingValue = Trim(CStr(vSettingValue))
                End If

                _oCache.Insert(sCacheKey, r_sSettingValue)

            End If
            Exit Function

        Catch ex As Exception

            GetPMRegSetting = PMEReturnCode.PMError

            Exit Function

        End Try
    End Function
#End Region

    ''' <summary>
    ''' Returns the Connection string 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property ConnectionString() As String
        Get

            Dim nReturn As Integer
            Dim sString As String = String.Empty

            'Use Cache to store the connection String.
            m_oCache = HttpRuntime.Cache()

            Dim sCacheKey As String = "ConnectionString"

            ' Try to get the ConnectString from the Cache
            sString = CType(m_oCache(sCacheKey), String)
            If sString = "" Then

                Dim sDatabase As String = String.Empty
                Dim sServer As String = String.Empty

                ' Get the database registry setting
                nReturn = GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine, _
                                            v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture, _
                                            v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLServer, _
                                            v_sSettingName:="Database", _
                                            r_sSettingValue:=sDatabase, _
                                            v_sSubKey:="Databases\Pure")
                If nReturn <> PMEReturnCode.PMTrue Then
                    Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.BackOfficeUnavailable, "GetPMRegSetting failed", "The registry setting for the DataBase/Server does not exist.")
                    STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), "ConnectionString", True)
                End If

                ' Get the server setting
                nReturn = GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine, _
                                            v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture, _
                                            v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLServer, _
                                            v_sSettingName:="Server", _
                                            r_sSettingValue:=sServer, _
                                            v_sSubKey:="Databases\Pure")

                If nReturn <> PMEReturnCode.PMTrue Then
                    Dim STSErrorEX As New STSErrorPublisher(STSErrorCodes.BackOfficeUnavailable, "GetPMRegSetting failed", "The registry setting for the DataBase/Server does not exist.")
                    STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), "ConnectionString", True)
                End If

                Dim sTrusted As String = String.Empty
                nReturn = GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine, _
                                        v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture, _
                                        v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLServer, _
                                        v_sSettingName:="Trusted", _
                                        r_sSettingValue:=sTrusted, _
                                        v_sSubKey:="Databases\Pure")

                Dim sLoginIDSecure As String = String.Empty
                Dim sPasswordSecure As String = String.Empty
                If sTrusted <> "1" Then
                    'Get the Login ID
                    nReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, _
                                v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, _
                                v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon,
                                v_sSettingName:=PMSQLLoginId, _
                                r_sSettingValue:=sLoginIDSecure)

                    'Get the Password
                    nReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, _
                                            v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, _
                                            v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon,
                                            v_sSettingName:=PMSQLLoginPassword, _
                                            r_sSettingValue:=sPasswordSecure)

                    Dim aKeys As Byte()
                    aKeys = Encoding.ASCII.GetBytes(PMEncryptionEntropy)

                    'Decrypt the LoginId and Password
                    sLoginIDSecure = Decrypt(sLoginIDSecure, aKeys)
                    sPasswordSecure = Decrypt(sPasswordSecure, aKeys)
                    sString = SAMFunc.ConnectionStringFrame
                Else
                    sString = SAMFunc.ConnectionStringFrameWindowsAuthentication
                End If

                ' Replace the placeholders with the correct values
                sString = Replace(sString, "{server}", sServer)
                sString = Replace(sString, "{database}", sDatabase)
                If sTrusted <> "1" Then
                    sString = Replace(sString, "{loginid}", sLoginIDSecure)
                    sString = Replace(sString, "{loginpassword}", sPasswordSecure)
                End If
                'Add to Cache
                m_oCache.Insert(sCacheKey, sString)
            End If
            Return sString
        End Get
    End Property
    ''' <summary>
    ''' Decrypt 
    ''' </summary>
    ''' <param name="sCipher"></param>
    ''' <param name="aKeys"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function Decrypt(sCipher As String, aKeys As Byte()) As String
        Const Scope As DataProtectionScope = DataProtectionScope.LocalMachine
        If sCipher = "" Then
            Return ""
        End If
        If sCipher Is Nothing Then
            Throw New ArgumentNullException("cipher")
        End If

        'parse base64 string
        Dim aData As Byte() = Convert.FromBase64String(sCipher)

        'decrypt data
        Dim aDecrypted As Byte() = ProtectedData.Unprotect(aData, aKeys, Scope)
        Return Encoding.Unicode.GetString(aDecrypted)
    End Function
End Class

' Class to contain the various performance counters used each method, and also code
' to initialise them in the overloaded public constructor method
Friend Class SiriusPerfCounters

    ' Used for Performance Counter
    Declare Function QueryPerformanceCounter Lib "Kernel32" (ByRef X As Long) As Short

    Private Const CounterCategory As String = "Sirius STS"

    Private Called As PerformanceCounter
    Private Succeed As PerformanceCounter
    Private Failed As PerformanceCounter
    Private Timer As PerformanceCounter
    Private Base As PerformanceCounter

    Private t1 As Long
    Private t2 As Long

    Public Sub StartMethod()
        ' Get the timer
        QueryPerformanceCounter(t1)
        ' Increase the called counter
        Called.Increment()
    End Sub

    Public Sub EndMethod()
        ' Get the timer
        QueryPerformanceCounter(t2)
        ' Increment the timer
        Timer.IncrementBy(t2 - t1)
        Base.Increment()
        ' Increment the success
        Succeed.Increment()
    End Sub

    Public Sub FailMethod()
        ' Increment the failed counter
        Failed.Increment()
    End Sub

    Public Sub New(ByVal Method As String)

        Dim sCalled As String = Method & " called"
        Dim sSucceed As String = Method & " succeeded"
        Dim sFailed As String = Method & " failed"
        Dim sTimer As String = Method & " time taken"
        Dim sBase As String = Method & " base time taken"

        Called = New PerformanceCounter(CounterCategory, sCalled, False)
        Succeed = New PerformanceCounter(CounterCategory, sSucceed, False)
        Failed = New PerformanceCounter(CounterCategory, sFailed, False)
        Timer = New PerformanceCounter(CounterCategory, sTimer, False)
        Base = New PerformanceCounter(CounterCategory, sBase, False)

        ' Start the recording
        Call StartMethod()

    End Sub

End Class
''' <summary>
''' Utility Class- Helps to sort xml in alphabetical order of nodes and attributes
''' </summary>
''' <remarks></remarks>

Public Class xmlSort

    Public Shared Sub Sort(ByRef r_sXMLString As String)
        'we need to create a proper xml Document so that it can be looped through 
        Dim oXMLContext As XmlParserContext = New XmlParserContext(Nothing, Nothing, Nothing, XmlSpace.None)
        Using xrReadXML As XmlTextReader = New XmlTextReader(r_sXMLString, XmlNodeType.Document, oXMLContext)
            Dim r_docXML As New XmlDocument
            r_docXML.Load(xrReadXML)
            xmlSort.Sort(r_docXML)
            r_sXMLString = r_docXML.OuterXml
        End Using
        oXMLContext = Nothing
    End Sub

    Public Shared Sub Sort(ByRef r_docXML As XmlDocument)
        Call Sort(r_docXML.DocumentElement)
    End Sub

    Private Shared Sub Sort(ByRef r_ndCurrent As XmlNode)

        SortXmlAttributes(r_ndCurrent.Attributes)
        SortXmlElements(r_ndCurrent)
        For Each r_ndchild As XmlNode In r_ndCurrent.ChildNodes
            Sort(r_ndchild)
        Next
    End Sub

    Private Shared Sub SortXmlAttributes(ByRef r_attCurrentNodeCollection As XmlAttributeCollection)

        If (r_attCurrentNodeCollection IsNot Nothing) Then
            Dim bIschanged As Boolean = True
            While (bIschanged)
                bIschanged = False

                For ix As Integer = 1 To r_attCurrentNodeCollection.Count - 1
                    If String.Compare(r_attCurrentNodeCollection(ix).Name, r_attCurrentNodeCollection(ix - 1).Name, StringComparison.Ordinal) < 0 Then
                        r_attCurrentNodeCollection.InsertBefore(r_attCurrentNodeCollection(ix), r_attCurrentNodeCollection(ix - 1))
                        bIschanged = True
                    End If
                Next

            End While
        End If

    End Sub

    Private Shared Sub SortXmlElements(ByRef r_ndCurrent As XmlNode)

        Dim bIschanged As Boolean = True
        While (bIschanged)
            bIschanged = False
            For ix As Integer = 1 To r_ndCurrent.ChildNodes.Count - 1
                If String.Compare(r_ndCurrent.ChildNodes(ix).Name, r_ndCurrent.ChildNodes(ix - 1).Name, StringComparison.Ordinal) < 0 Then
                    r_ndCurrent.InsertBefore(r_ndCurrent.ChildNodes(ix), r_ndCurrent.ChildNodes(ix - 1))
                    bIschanged = True
                End If
            Next
        End While
    End Sub

End Class

''' <summary>
''' XML Node Casting is used to store information of a node and 
''' its replacement with a flag  to specifiy if converting to list of specified node type.
''' </summary>
''' <remarks></remarks>
Public Class xmlNodeCasting
    Private m_sFromTypeName As String
    Private m_sConvertToTypeName As String
    Private m_bIsList As Boolean

    Sub New(ByVal sFromTypeName As String, ByVal sConvertToTypeName As String, Optional ByVal bIsList As Boolean = False)
        m_sFromTypeName = sFromTypeName
        m_sConvertToTypeName = sConvertToTypeName
        m_bIsList = bIsList
    End Sub

    Public ReadOnly Property FromTypeName() As String
        Get
            Return m_sFromTypeName
        End Get
    End Property

    Public ReadOnly Property ConvertToTypeName() As String
        Get
            Return m_sConvertToTypeName
        End Get
    End Property

    Public ReadOnly Property IsList() As Boolean
        Get
            Return m_bIsList
        End Get
    End Property

End Class

