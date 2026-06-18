Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("QuotePolicy_NET.QuotePolicy")> _
Public NotInheritable Class QuotePolicy
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: QuotePolicy
    '
    ' Date: RFC210600
    '
    ' Description: Contains the methods for dealing with Quotes and Policies
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "QuotePolicy"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' GIS Data Set
    Private m_oDataSet As cGISDataSetControl.Application

    ' ***************************************************************** '
    ' Name: AddParty
    '
    ' Description: Add a Party

    ' ***************************************************************** '
    Public Function AddParty(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_sPartyTypeCode As String, ByVal v_sForename As String, ByVal v_sSurname As String, ByVal v_sDateOfBirth As String, ByVal v_sEmailAddress As String, ByVal v_sCurrentRenewalDate As String, ByVal v_sAddress1 As String, ByVal v_sAddress2 As String, ByVal v_sAddress3 As String, ByVal v_sAddress4 As String, ByVal v_sPostcode As String, ByRef r_lPartyCnt As Integer, Optional ByVal v_sTitle As Object = Nothing, Optional ByVal v_sMaritalStatusCode As Object = Nothing, Optional ByVal v_sGenderCode As Object = Nothing, Optional ByVal v_sInitials As Object = Nothing, Optional ByVal v_sTelephoneNumber As Object = Nothing, Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim lReturn As gPMConstants.PMEReturnCode
            Dim sActionXML As String = ""

            Dim sActionReturnXML As String = ""
            Dim lReturnValue As gPMConstants.PMEReturnCode

            Dim lPartyCnt As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Action Properties





            lReturn = CType(FormatActionXMLQuotePolicy(v_sDataModelCode:=v_sGisDataModelCode, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_lAction:=iGISSharedConstants.GISDSActionAddParty, r_sActionXML:=sActionXML, v_sPartyTypeCode:=v_sPartyTypeCode, v_sForename:=v_sForename, v_sSurname:=v_sSurname, v_sDateOfBirth:=v_sDateOfBirth, v_sEmailAddress:=v_sEmailAddress, v_sCurrentRenewalDate:=v_sCurrentRenewalDate, v_sAddress1:=v_sAddress1, v_sAddress2:=v_sAddress2, v_sAddress3:=v_sAddress3, v_sAddress4:=v_sAddress4, v_sPostcode:=v_sPostcode, v_sTitle:=CStr(v_sTitle), v_sMaritalStatusCode:=CStr(v_sMaritalStatusCode), v_sGenderCode:=CStr(v_sGenderCode), v_sInitials:=CStr(v_sInitials), v_sTelephoneNumber:=CStr(v_sTelephoneNumber), r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            '**** START CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:14   ****
            '**** I added the AdditionalDataArray variable.
            lReturn = CType(UnFormatActionReturnXMLQuotePolicy(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_lPartyCnt:=lPartyCnt, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
            '**** END CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:14   ****

            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_lPartyCnt = lPartyCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindQuote
    '
    ' Description: List all Quotes that match the given search criteria
    ' ***************************************************************** '
    Public Function FindQuote(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByRef r_vResultArray(,) As Object, Optional ByVal v_sQuoteRef As Object = Nothing, Optional ByVal v_dCoverStartDate As Object = Nothing, Optional ByVal v_sDescription As Object = Nothing, Optional ByVal v_lLeadAgentCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim lReturn As gPMConstants.PMEReturnCode
            Dim sActionXML As String = ""

            Dim sActionReturnXML As String = ""
            Dim lReturnValue As gPMConstants.PMEReturnCode

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Action Properties




            lReturn = CType(FormatActionXMLQuotePolicy(v_sDataModelCode:=v_sGisDataModelCode, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_lAction:=iGISSharedConstants.GISDSActionFindQuote, r_sActionXML:=sActionXML, v_sInsuranceFileRef:=CStr(v_sQuoteRef), v_sInsFolderDescription:=CStr(v_sDescription), v_dCoverStartDate:=CStr(v_dCoverStartDate), v_lLeadAgentCnt:=CInt(v_lLeadAgentCnt)), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXMLQuotePolicy(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vFindQuoteArray:=r_vResultArray), gPMConstants.PMEReturnCode)

            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindQuote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetParty
    '
    ' Description: Get a Parties details given a PartyCnt

    ' ***************************************************************** '
    Public Function GetParty(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_lPartyCnt As Object, ByRef r_sSurname As Object, ByRef r_sForename As Object, ByRef r_sPartyType As Object, ByRef r_sAddress1 As Object, ByRef r_sAddress2 As Object, ByRef r_sAddress3 As Object, ByRef r_sAddress4 As Object, ByRef r_sPostcode As Object, ByRef r_sDOB As Object, ByRef r_sEMail As Object, ByRef r_sUserID As Object, ByRef r_sPassword As Object, ByRef r_sShortName As Object, ByRef r_sResolvedName As Object, Optional ByRef r_sMothersMaidenName As String = "", Optional ByRef r_sTPUserCode As String = "", Optional ByRef r_sTPIntroducer As String = "", Optional ByRef r_sAQuestion As String = "", Optional ByRef r_sTheAnswer As String = "", Optional ByRef r_dMemorableDate As String = "", Optional ByRef r_dCurrInsRenewalDate As String = "", Optional ByRef r_sTitle As String = "", Optional ByRef r_sMaritalStatusCode As String = "", Optional ByRef r_sGenderCode As String = "", Optional ByRef r_sInitials As String = "", Optional ByRef r_sTelephoneNumber As String = "") As Integer

        Dim result As Integer = 0
        Try

            Dim lReturn As gPMConstants.PMEReturnCode
            Dim sActionXML As String = ""

            Dim sActionReturnXML As String = ""
            Dim lReturnValue As gPMConstants.PMEReturnCode


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Action Properties

            lReturn = CType(FormatActionXMLQuotePolicy(v_sDataModelCode:=v_sGisDataModelCode, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_lAction:=iGISSharedConstants.GISDSActionGetParty, r_sActionXML:=sActionXML, v_lPartyCnt:=CInt(v_lPartyCnt)), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXMLQuotePolicy(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_lPartyCnt:=v_lPartyCnt, r_sSurname:=r_sSurname, r_sForename:=r_sForename, r_sPartyTypeCode:=r_sPartyType, r_sAddress1:=r_sAddress1, r_sAddress2:=r_sAddress2, r_sAddress3:=r_sAddress3, r_sAddress4:=r_sAddress4, r_sPostcode:=r_sPostcode, r_sDateOfBirth:=r_sDOB, r_sEMail:=r_sEMail, r_sUserID:=r_sUserID, r_sPassword:=r_sPassword, r_sShortName:=r_sShortName, r_sResolvedName:=r_sResolvedName, r_sMothersMaidenName:=r_sMothersMaidenName, r_sTPUserCode:=r_sTPUserCode, r_sTPIntroducer:=r_sTPIntroducer, r_sAQuestion:=r_sAQuestion, r_sTheAnswer:=r_sTheAnswer, r_sMemorableDate:=r_dMemorableDate, r_sCurrInsRenewalDate:=r_dCurrInsRenewalDate, r_sTitle:=r_sTitle, r_sMaritalStatusCode:=r_sMaritalStatusCode, r_sGenderCode:=r_sGenderCode, r_sInitials:=r_sInitials, r_sTelephoneNumber:=r_sTelephoneNumber), gPMConstants.PMEReturnCode)

            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindParty
    '
    ' Description: List all Parties that match the given search criteria

    '**** CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:20   ****
    '**** Added the optional v_sAddress1 variable to be used in Agents On Line
    ' ***************************************************************** '
    Public Function FindParty(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_sPartyType As String, ByVal v_sShortname As String, ByVal v_sResolvedName As String, ByVal v_sUserID As String, ByVal v_sTelephoneNumber As String, ByVal v_sPostcode As String, ByRef r_vResultArray(,) As Object, Optional ByVal v_lLeadAgentCnt As Byte = 0, Optional ByVal v_vAdditionalDataArray(,) As Object = Nothing, Optional ByVal v_sAddress1 As String = "") As Integer

        Dim result As Integer = 0
        Try

            Dim lReturn As gPMConstants.PMEReturnCode
            Dim sActionXML As String = ""

            Dim sActionReturnXML As String = ""
            Dim lReturnValue As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            '**** START CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:22   ****
            '**** Added the v_sAddress1 Variable
            ' Set the Action Properties
            lReturn = CType(FormatActionXMLQuotePolicy(v_sDataModelCode:=v_sGisDataModelCode, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_lAction:=iGISSharedConstants.GISDSActionFindParty, r_sActionXML:=sActionXML, v_sPartyTypeCode:=v_sPartyType, r_sShortName:=v_sShortname, v_sResolvedName:=v_sResolvedName, r_sUserID:=v_sUserID, v_sTelephoneNumber:=v_sTelephoneNumber, v_sPostcode:=v_sPostcode, v_lLeadAgentCnt:=v_lLeadAgentCnt, r_vAdditionalDataArray:=v_vAdditionalDataArray, v_sAddress1:=v_sAddress1), gPMConstants.PMEReturnCode)
            '****   END CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:22   ****

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXMLQuotePolicy(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vPartyArray:=r_vResultArray), gPMConstants.PMEReturnCode)

            '**** Added By: AAB  -  Added On:  08-Oct-2002 09:10 ****
            '**** I changed lReturnValue to lReturn when we check for PMTrue
            '**** If there was an error lReturnValue does not get set to FALSE
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindPolicy
    '
    ' Description: List all policies that match the given search criteria

    ' ***************************************************************** '
    Public Function FindPolicy(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByRef r_vResultArray(,) As Object, Optional ByRef r_vAdditionalDataArray(,) As Object = Nothing) As Integer
        'Public Function FindPolicy() As Variant

        Dim result As Integer = 0
        Try


            Dim lReturn As gPMConstants.PMEReturnCode
            Dim sActionXML As String = ""

            Dim sActionReturnXML As String = ""
            Dim lReturnValue As gPMConstants.PMEReturnCode

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Set the Action Properties
            lReturn = CType(FormatActionXMLQuotePolicy(v_sDataModelCode:=v_sGisDataModelCode, v_sBusinessTypeCode:=v_sGISBusinessTypeCode, v_lAction:=iGISSharedConstants.GISDSActionFindPolicy, r_sActionXML:=sActionXML, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXMLQuotePolicy(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vFindPolicyArray:=r_vResultArray), gPMConstants.PMEReturnCode)

            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetProductByParty
    '
    ' Description: List all products available to a given agent
    ' ***************************************************************** '
    Public Function GetProductByAgent(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_lAgentPartyCnt As Integer, ByRef r_vResultArray(,) As Object, Optional ByVal v_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim lReturn As gPMConstants.PMEReturnCode

            result = gPMConstants.PMEReturnCode.PMTrue

            '**** START CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:25   ****
            '**** I commented the code, and replaced with GISCall below instead.
            'Dim sActionXML As String
            '
            'Dim sActionReturnXML As String
            'Dim lReturnValue As Long

            '    ' Set the Action Properties
            '    lReturn = FormatActionXMLQuotePolicy( _
            ''        v_sDataModelCode:=v_sGisDataModelCode, _
            ''        v_sBusinessTypeCode:=v_sGISBusinessTypeCode, _
            ''        v_lAction:=GISDSActionGetProductByAgent, _
            ''        r_sActionXML:=sActionXML, _
            ''        v_lPartyCnt:=v_lAgentPartyCnt, _
            ''        r_vAdditionalDataArray:=v_vAdditionalDataArray)
            '
            '    If (lReturn <> PMTrue) Then
            '        GetProductByAgent = PMFalse
            '        Exit Function
            '    End If
            '
            '    ' Send and Process The Command
            '    lReturn = ProcessActionViaHTTP( _
            ''        v_oDataSet:=m_oDataSet, _
            ''        v_sActionXML:=sActionXML, _
            ''        r_sActionReturnXML:=sActionReturnXML)
            '    If (lReturn <> PMTrue) Then
            '        GetProductByAgent = PMFalse
            '        Exit Function
            '    End If
            '
            '    ' Check the ActionReturn Value
            '    ' Unformat the Action Return XML
            '    lReturn = UnFormatActionReturnXMLQuotePolicy( _
            ''         v_sActionReturnXML:=sActionReturnXML, _
            ''        r_lReturnValue:=lReturnValue, _
            ''        r_vProductArray:=r_vResultArray)


            lReturn = CType(GISCall("QUOTEPOLICY", "GetProductByAgent", v_sGisDataModelCode, v_sGISBusinessTypeCode, v_lAgentPartyCnt, r_vResultArray, v_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            '****   END CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:25   ****

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductByAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductByAgent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDataSet = New cGISDataSetControl.Application()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                m_oDataSet = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    ' Name: GetQuotesForParty
    '
    ' Description: List all Quotes of the type requested for
    '              this PartyCnt.
    '
    '              If NO type specified Quotes of ALL types
    '              are returned.
    '
    '              PolicyTypeCode = "MOTOR", "HOME", "TRAVEL" etc
    ' ***************************************************************** '
    Public Function GetQuotesForParty(ByVal v_sDataModelCode As String, ByVal v_lPartyCnt As Integer, ByRef r_vQuoteArray(,) As Object, Optional ByVal v_sPolicyTypeCode As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML As String = ""

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Action Properties
            lReturn = CType(FormatActionXMLQuotePolicy(v_sDataModelCode:=v_sDataModelCode, v_sBusinessTypeCode:="", v_lAction:=iGISSharedConstants.GISDSActionGetQuotesForParty, r_sActionXML:=sActionXML, v_lPartyCnt:=v_lPartyCnt), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXMLQuotePolicy(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vQuoteArray:=r_vQuoteArray), gPMConstants.PMEReturnCode)

            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotesForParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuotesForParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '**** START CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:30   ****
    '**** Added this new GetQuotes call to Support Agents On Line
    Public Function GetQuotes(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByRef r_vQuoteArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lReturn As gPMConstants.PMEReturnCode


            lReturn = CType(GISCall("QUOTEPOLICY", "GetQuotes", v_sGisDataModelCode, v_sGISBusinessTypeCode, v_lPartyCnt, r_vQuoteArray), gPMConstants.PMEReturnCode)

            ' Check the Return Value
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuotes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '****   END CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:30   ****

    '**** START CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:31   ****
    '**** Added this new GetQuotesDetails call to Support Agents On Line
    Public Function GetQuoteDetails(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_lInsuranceFileCnt As Integer, ByRef r_vQuoteArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lReturn As gPMConstants.PMEReturnCode


            lReturn = CType(GISCall("QUOTEPOLICY", "GetQuoteDetails", v_sGisDataModelCode, v_sGISBusinessTypeCode, v_lInsuranceFileCnt, r_vQuoteArray), gPMConstants.PMEReturnCode)

            ' Check the Return Value
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '****   END CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:31   ****

    '**** START CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:33   ****
    '**** Added this new GetQuoteRisks call to Support Agents On Line
    Public Function GetQuoteRisks(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_lInsuranceFileCnt As Integer, ByRef r_vQuoteArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lReturn As gPMConstants.PMEReturnCode


            lReturn = CType(GISCall("QUOTEPOLICY", "GetQuoteRisks", v_sGisDataModelCode, v_sGISBusinessTypeCode, v_lInsuranceFileCnt, r_vQuoteArray), gPMConstants.PMEReturnCode)

            ' Check the Return Value
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '****   END CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:33   ****

    ' ***************************************************************** '
    ' Name: GetPoliciesForParty
    '
    ' Description: List all Policies of the type requested for
    '              this PartyCnt.
    '
    '              If NO type specified Policies of ALL types
    '              are returned.
    '
    '              PolicyTypeCode = "MOTOR", "HOME", "TRAVEL" etc
    ' ***************************************************************** '
    Public Function GetPoliciesForParty(ByVal v_sDataModelCode As String, ByVal v_lPartyCnt As Integer, ByRef r_vPolicyArray(,) As Object, Optional ByVal v_sPolicyTypeCode As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return GetQuotesPoliciesForParty(v_sDataModelCode:=v_sDataModelCode, v_lPartyCnt:=v_lPartyCnt, v_lSearchType:=iGISSharedConstants.GISIFSTPolicy, r_vQuotePolicyArray:=r_vPolicyArray, v_sPolicyTypeCode:=v_sPolicyTypeCode)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPoliciesForParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPoliciesForParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetQuotesAndPoliciesForParty
    '
    ' Description: List all Quotes AND Policies of the type requested for
    '              this PartyCnt.
    '
    '              If NO type specified Quotes AND Policies of ALL types
    '              are returned.
    '
    '              PolicyTypeCode = "MOTOR", "HOME", "TRAVEL" etc
    ' ***************************************************************** '
    Public Function GetQuotesAndPoliciesForParty(ByVal v_sDataModelCode As String, ByVal v_lPartyCnt As Integer, ByRef r_vQuotePolicyArray(,) As Object, Optional ByVal v_sPolicyTypeCode As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return GetQuotesPoliciesForParty(v_sDataModelCode:=v_sDataModelCode, v_lPartyCnt:=v_lPartyCnt, v_lSearchType:=iGISSharedConstants.GISIFSTQuotePolicy, r_vQuotePolicyArray:=r_vQuotePolicyArray, v_sPolicyTypeCode:=v_sPolicyTypeCode)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotesAndPoliciesForParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuotesAndPoliciesForParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPolicyVersions
    '
    ' Description: List the versions of a Policy.
    '
    ' RFC120700 - Optionally Get the PolicyVersions via the InsuranceFileCnt
    '             OR Insurance File Reference (Policy Num)
    ' ***************************************************************** '
    Public Function GetPolicyVersions(ByVal v_sDataModelCode As String, ByRef r_vPolicyVersionArray(,) As Object, Optional ByVal v_lInsuranceFileCnt As Integer = -1, Optional ByVal v_sInsuranceFileRef As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue, lPartyCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Action Properties
            lReturn = CType(FormatActionXMLQuotePolicy(v_sDataModelCode:=v_sDataModelCode, v_sBusinessTypeCode:="", v_lAction:=iGISSharedConstants.GISDSActionGetPolicyVersions, r_sActionXML:=sActionXML, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sInsuranceFileRef:=v_sInsuranceFileRef), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXMLQuotePolicy(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vPolicyVersionArray:=r_vPolicyVersionArray), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyVersions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetQuotesPoliciesForParty
    '
    ' Description: List all Quotes AND/OR Policies of the type requested for
    '              this PartyCnt.
    '
    '              If NO type specified Quotes/Policies of ALL types
    '              are returned.
    '
    '              PolicyTypeCode = "MOTOR", "HOME", "TRAVEL" etc
    ' ***************************************************************** '
    Private Function GetQuotesPoliciesForParty(ByVal v_sDataModelCode As String, ByVal v_lPartyCnt As Integer, ByVal v_lSearchType As Integer, ByRef r_vQuotePolicyArray(,) As Object, Optional ByVal v_sPolicyTypeCode As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue, lPartyCnt As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set the Action Properties

        lReturn = CType(FormatActionXMLQuotePolicy(v_sDataModelCode:=v_sDataModelCode, v_sBusinessTypeCode:="", v_lAction:=iGISSharedConstants.GISDSActionGetQuotesPoliciesForParty, r_sActionXML:=sActionXML, v_lPartyCnt:=v_lPartyCnt, v_lInsFileSearchType:=v_lSearchType, v_sPolicyTypeCode:=CStr(v_sPolicyTypeCode)), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Send and Process The Command
        lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check the ActionReturn Value
        ' Unformat the Action Return XML
        lReturn = CType(UnFormatActionReturnXMLQuotePolicy(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_vQuotePolicyArray:=r_vQuotePolicyArray), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    '**** START CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:34   ****
    '**** Added this new GetRiskByProduct call to Support Agents On Line
    Public Function GetRiskByProduct(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_lProductID As Integer, ByRef r_vResultArray As Object, Optional ByVal v_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim lReturn As gPMConstants.PMEReturnCode

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(GISCall("QuotePolicy", "GetRiskByProduct", v_sGisDataModelCode, v_sGISBusinessTypeCode, v_lProductID, r_vResultArray, v_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskByProduct Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskByProduct", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '****   END CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:34   ****

    '**** START CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:36   ****
    '**** Added this new GetRatingDetails call to Support Agents On Line
    Public Function GetRatingDetails(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_vRatingSections As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lReturn As gPMConstants.PMEReturnCode


            lReturn = CType(GISCall("QUOTEPOLICY", "GetRatingDetails", v_sGisDataModelCode, v_sGISBusinessTypeCode, v_lInsuranceFolderCnt, v_lInsuranceFileCnt, v_lRiskCnt, r_vRatingSections), gPMConstants.PMEReturnCode)

            ' Check the Return Value
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRatingDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRatingDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    '****   END CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:36   ****

    '**** Added By: AAB  -  Added On:  08-Oct-2002 15:22 ****
    Public Function UpdateParty(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_lPartyCnt As Object, Optional ByVal v_sForename As Object = Nothing, Optional ByVal v_sSurname As Object = Nothing, Optional ByVal v_sDateOfBirth As Object = Nothing, Optional ByVal v_sEmailAddress As Object = Nothing, Optional ByVal v_sCurrentRenewalDate As Object = Nothing, Optional ByVal v_sAddress1 As Object = Nothing, Optional ByVal v_sAddress2 As Object = Nothing, Optional ByVal v_sAddress3 As Object = Nothing, Optional ByVal v_sAddress4 As Object = Nothing, Optional ByVal v_sPostcode As Object = Nothing, Optional ByVal v_sTitle As Object = Nothing, Optional ByVal v_sMaritalStatusCode As Object = Nothing, Optional ByVal v_sGenderCode As Object = Nothing, Optional ByVal v_sInitials As Object = Nothing, Optional ByVal v_sTelephoneNumber As Object = Nothing, Optional ByVal v_vAdditionalDataArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim lReturn As gPMConstants.PMEReturnCode

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(GISCall("QUOTEPOLICY", "UpdateParty", v_sGisDataModelCode, v_sGISBusinessTypeCode, v_lPartyCnt, v_sForename, v_sSurname, v_sDateOfBirth, v_sEmailAddress, v_sCurrentRenewalDate, v_sAddress1, v_sAddress2, v_sAddress3, v_sAddress4, v_sPostcode, v_sTitle, v_sMaritalStatusCode, v_sGenderCode, v_sInitials, v_sTelephoneNumber, v_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            ' Check the Return Value
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If


            Return result

        Catch excep As System.Exception





            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '****   END CHANGES - Changed By: AAB  - Changed On:  08-Oct-2002 15:22   ****
End Class

