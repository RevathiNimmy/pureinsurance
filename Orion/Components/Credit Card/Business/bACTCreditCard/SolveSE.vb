Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Windows.Forms
Imports System.Xml
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class SolveSE
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name:   SolveSE
    '
    ' Date:         05/01/2005
    '
    ' Description:  Class module for use of Retail Logic's Solve/SE
    '               credit card validation service
    '
    ' Edit History: CLG 05/11/2001 - Created
    '               DD 13/01/2005  - Rewritten for new business object
    '
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 09/12/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Local instance of Winsock form
    Private m_frmWinSock As frmWinSock

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "SolveSE"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    Private m_vSolveXmlTransactions() As Object 'holds transaction XML definitions
    Private m_vErrorMessages() As Object 'holds error message definitions
    Private m_vAuthorisationMessages() As Object 'holds decode of SolveSE responses

    Private m_iSolveSEState As MainModule.eSolveSEState

    Private Const k_sXMLValidation As String = "<RLSOLVE_MSG"

    'This DTD defines the Solve/SE XML Interface
    'It's stored in "$/Third Party/DTD and Schema"
    Private Const k_sSolveSEDTD As String = "RetailLogicSolveSE111A.dtd"

    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)
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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, ByRef vDatabase As dPMDAO.Database) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel
            m_oDatabase = vDatabase

            ' Initialisation Code.

            m_vSolveXmlTransactions = New Object() {New Object() {MainModule.eSolveSEMessage.eTestProbe, "Test Probe", "<?xml version=""1.0"" ?><RLSOLVE_MSG version=""0.1""><REQUESTER><SOURCE id=""RL94"" /><TRANS_NUM> 1 </TRANS_NUM></REQUESTER><REQUEST replyFormat=""standard""><ADMIN><PROBE /></ADMIN></REQUEST></RLSOLVE_MSG>"}, New Object() {MainModule.eSolveSEMessage.eAuthorisationRequest, "Authorisation request", "<?xml version=""1.0"" ?><RLSOLVE_MSG version=""0.1""><REQUESTER><SOURCE id=""88"" /><TRANS_NUM> 99 </TRANS_NUM></REQUESTER><REQUEST replyFormat=""extended""><AUTH_REQ  action=""auth"" on-line=""yes""><CARD><PAN start=""2001-01"" end=""2002-02"" issue=""0""> 0000 </PAN><CV_DATA cv2=""000"" postCode=""xxx"" address=""xxx""/></CARD><TRANSACTION source=""keyed"" customer=""present"" verify=""yes"" type=""purchase""><AMOUNT> 0 </AMOUNT></TRANSACTION></AUTH_REQ></REQUEST></RLSOLVE_MSG>"}, New Object() {MainModule.eSolveSEMessage.eSettlement, "Settlement", "<?xml version=""1.0"" ?><!-- MFI 11 Batch Data Capture --><RLSOLVE_MSG version=""0.1""><REQUESTER><SOURCE id=""RL94"" /><TRANS_NUM> 5 </TRANS_NUM></REQUESTER><REQUEST replyFormat=""standard""><AUTH_REQ action=""settle""><CARD><PAN start=""1999-06"" end=""2001-06"" issue=""0""> 0000 </PAN><CV_DATA cv2=""000"" postCode=""xxx"" address=""xxx""/></CARD><TRANSACTION source=""keyed"" customer=""present"" verify=""yes"" type=""purchase""><AMOUNT> 1200 </AMOUNT><AUTH_CODE method=""on-line""> 0 </AUTH_CODE></TRANSACTION></AUTH_REQ></REQUEST></RLSOLVE_MSG>"}, New Object() {MainModule.eSolveSEMessage.eCancelTransaction, "Cancel Transaction", "<?xml version=""1.0"" ?><RLSOLVE_MSG version=""0.1""><REQUESTER><SOURCE id=""RL94"" /><TRANS_NUM> 4 </TRANS_NUM></REQUESTER><REQUEST><AUTH_REQ action=""cancel"" /></REQUEST></RLSOLVE_MSG>"}, New Object() {MainModule.eSolveSEMessage.eAuthorisationAndSettle, "Authorisation and Settle", "<?xml version=""1.0"" ?><RLSOLVE_MSG version=""0.1""><REQUESTER><SOURCE id=""RL94"" /><TRANS_NUM> 99 </TRANS_NUM></REQUESTER><REQUEST replyFormat=""extended""><AUTH_REQ action=""auth_n_settle"" on-line=""yes""><CARD><PAN start=""1999-06"" end=""2001-06"" issue=""0""> 0000 </PAN><CV_DATA cv2=""000"" postCode=""xxx"" address=""xxx""/></CARD><TRANSACTION source=""keyed"" customer=""present"" verify=""yes"" type=""purchase""><AMOUNT> 0 </AMOUNT></TRANSACTION></AUTH_REQ></REQUEST></RLSOLVE_MSG>"}, New Object() {MainModule.eSolveSEMessage.eCompleteTransaction, "Complete Transaction", "<?xml version=""1.0"" ?><RLSOLVE_MSG version=""0.1""><REQUESTER><SOURCE id=""RL94"" /><TRANS_NUM> 6 </TRANS_NUM></REQUESTER><REQUEST><AUTH_REQ action=""complete""></AUTH_REQ></REQUEST></RLSOLVE_MSG>"}, New Object() {MainModule.eSolveSEMessage.eValidateOnly, "Validate only", "<?xml version=""1.0"" ?><RLSOLVE_MSG version=""0.1""><REQUESTER><SOURCE id=""RL94"" /><TRANS_NUM> 8 </TRANS_NUM></REQUESTER><REQUEST replyFormat=""standard""><VALIDATE><CARD><PAN start=""1999-06"" end=""2002-06"" issue=""0""> 4000000000000002</PAN><CV_DATA cv2=""000"" postCode=""xxx"" address=""xxx""/></CARD><TRANSACTION source=""keyed"" customer=""present"" type=""purchase""></TRANSACTION></VALIDATE></REQUEST></RLSOLVE_MSG>"}, New Object() {MainModule.eSolveSEMessage.eReload, "Reload", "<?xml version=""1.0"" ?><RLSOLVE_MSG version=""0.1""><REQUESTER><REQUEST><ADMIN> <SOURCE id=""RL94"" /><TRANS_NUM/><SHUTDOWN mode=""immediate""/></ADMIN></REQUEST></REQUESTER></RLSOLVE_MSG>"}}


            m_vErrorMessages = New Object() {New Object() {MainModule.eSolveSEExtendedError.eAuthorisationFailed, "Failed"}, New Object() {MainModule.eSolveSEExtendedError.eOkay, "Okay"}, New Object() {MainModule.eSolveSEExtendedError.eNotInitialised, "Not initialised"}, New Object() {MainModule.eSolveSEExtendedError.eAlreadyInitialised, "Already initialised"}, New Object() {MainModule.eSolveSEExtendedError.eWinsockNotConnected, "Winsock not connected"}, New Object() {MainModule.eSolveSEExtendedError.eUnknownTransactionId, "Unknown transaction ID"}, New Object() {MainModule.eSolveSEExtendedError.eNoHostPortDefined, "No host port defined"}, New Object() {MainModule.eSolveSEExtendedError.eNoHostDestinationDefined, "No host destination defined"}, New Object() {MainModule.eSolveSEExtendedError.eParseError, "XML Parse error"}, New Object() {MainModule.eSolveSEExtendedError.eConnectionTimeout, "Connection timeout"}, New Object() {MainModule.eSolveSEExtendedError.eConnectionError, "Winsock connection error"}}


            m_vAuthorisationMessages = New Object() {New Object() {k_eValidated, "Validated"}, New Object() {k_eTerminal, "Terminal"}, New Object() {k_eOnline, "On-line"}, New Object() {k_eManual, "Manual"}, New Object() {k_eDeclined, "Declined"}, New Object() {k_eCancelled, "Cancelled"}, New Object() {k_eUnable_to_cancel, "Unable to cancel"}, New Object() {k_eGet_manual_auth, "Get manual authorisation"}, New Object() {k_eGet_sig_auth, "Get signature authoristation"}, New Object() {k_eTelephone, "Telephone"}, New Object() {k_eGet_manual_auth_resubmit, "Get manual authorisation and re-submit"}, New Object() {k_eHot_card_declined, "Hot card declined"}}

            m_frmWinSock = New frmWinSock()


            'TODO: Need to discuss this. the below line causes a blank form t appear on screen.
            'm_frmWinSock.ShowDialog()
            m_frmWinSock.Visible = False
            m_iSolveSEState = MainModule.eSolveSEState.eFormLoaded

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If disposing Then
            End If
            m_frmWinSock.Close()
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: SendSolveSEMessage
    '
    ' Description:
    '
    ' History: 05/11/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function SendSolveSEMessage(ByVal iSolveSEMessageID As Integer, ByVal iTimeout As Integer, ByVal bIsReceipt As Boolean, ByVal sSourceID As String, ByVal sCCNumber As String, ByVal sCCStartDate As String, ByVal sCCEndDate As String, ByVal sCCIssue As String, ByVal sCCPin As String, ByVal sCCAddress1 As String, ByVal sCCPostcode As String, ByVal sCCCustomerPresent As String, ByVal sCCKeyedOrSwiped As String, ByVal cCCAmount As Decimal, ByRef r_sCCAuthorisationCode As String, ByRef r_sCCTransactionCode As String, ByRef r_sXML As String, ByRef r_sResultXML As String, ByRef r_iExtendedError As MainModule.eSolveSEExtendedError, ByRef r_sExtendedError As String) As Integer

        Dim result As Integer = 0
        Dim sMsg As String = ""
        Dim r_oWinsock As AxMSWinsockLib.AxWinsock
        Dim sSourceXml As String = ""
        Dim cStart As Decimal
        Dim xmlDoc As XmlDocument
        Dim vNodeRoot As XmlNode
        Dim sMessage As String = ""



        result = gPMConstants.PMEReturnCode.PMFalse

        r_oWinsock = m_frmWinSock.Winsock1

        If m_iSolveSEState <> MainModule.eSolveSEState.eFormLoaded Then
            r_iExtendedError = MainModule.eSolveSEExtendedError.eNotInitialised
            Return result
        End If

        If r_oWinsock.CtlState <> MSWinsockLib.StateConstants.sckConnected Then
            r_iExtendedError = MainModule.eSolveSEExtendedError.eWinsockNotConnected

            Return result
        End If

        xmlDoc = New XmlDocument()

        If GetXMLForMessageID(iSolveSEMessageID, sSourceXml) <> gPMConstants.PMEReturnCode.PMTrue Then
            r_iExtendedError = MainModule.eSolveSEExtendedError.eUnknownTransactionId
            Return result
        End If

        'now set the values

        'Set card values
        Try
            xmlDoc.LoadXml(sSourceXml)


        Catch parseError As System.Xml.XmlException
            r_iExtendedError = MainModule.eSolveSEExtendedError.eParseError
            With parseError
                'MsgBox



                'developer guide no solution no. 22
                'sMsg = "document Parse Error:" & Strings.Chr(13) & Strings.Chr(10) & _
                '       "Code: " & CStr(.errorCode) & Strings.Chr(13) & Strings.Chr(10) & _
                '       "Line: " & CStr(CType(parseError, XmlException).LineNumber) & Strings.Chr(13) & Strings.Chr(10) & _
                '       "lPos: " & CStr(CType(parseError, XmlException).LinePosition) & Strings.Chr(13) & Strings.Chr(10) & _
                '       "Reason: " & .Message & Strings.Chr(13) & Strings.Chr(10) & _
                '       "Src: " & .srcText & Strings.Chr(13) & Strings.Chr(10) & _
                '       "fPos: " & CStr(.filepos)

                sMsg = "document Parse Error:" & Strings.Chr(13) & Strings.Chr(10) & _
                       "Line: " & CStr(CType(parseError, XmlException).LineNumber) & Strings.Chr(13) & Strings.Chr(10) & _
                       "lPos: " & CStr(CType(parseError, XmlException).LinePosition) & Strings.Chr(13) & Strings.Chr(10) & _
                       "Reason: " & .Message & Strings.Chr(13) & Strings.Chr(10) & _
                       "Src: " & .Source & Strings.Chr(13) & Strings.Chr(10) & _
                       "fPos: " & CStr(.SourceUri)

                ' Log Error Message
            End With

            r_iExtendedError = MainModule.eSolveSEExtendedError.eParseError

            Return result

        End Try

        'Set Card and Dates
        vNodeRoot = xmlDoc.SelectSingleNode("RLSOLVE_MSG/REQUEST/AUTH_REQ/CARD/PAN")
        If vNodeRoot Is Nothing Then
            vNodeRoot = xmlDoc.SelectSingleNode("RLSOLVE_MSG/REQUEST/VALIDATE/CARD/PAN")
        End If
        If Not (vNodeRoot Is Nothing) Then
            vNodeRoot.ChildNodes.Item(0).InnerText = sCCNumber

            If sCCStartDate <> "" Then
                Dim TempDate As Date
                vNodeRoot.Attributes.GetNamedItem("start").InnerText = IIf(DateTime.TryParse(sCCStartDate, TempDate), TempDate.ToString("yyyy-MM"), sCCStartDate)
            Else
                vNodeRoot.Attributes.RemoveNamedItem("start")
            End If

            Dim TempDate2 As Date
            vNodeRoot.Attributes.GetNamedItem("end").InnerText = IIf(DateTime.TryParse(sCCEndDate, TempDate2), TempDate2.ToString("yyyy-MM"), sCCEndDate)

            If sCCIssue <> "" Then
                vNodeRoot.Attributes.GetNamedItem("issue").InnerText = sCCIssue
            Else
                vNodeRoot.Attributes.RemoveNamedItem("issue")
            End If
        End If

        'Set Extra Security Information
        vNodeRoot = xmlDoc.SelectSingleNode("RLSOLVE_MSG/REQUEST/AUTH_REQ/CARD/CV_DATA")
        If vNodeRoot Is Nothing Then
            vNodeRoot = xmlDoc.SelectSingleNode("RLSOLVE_MSG/REQUEST/VALIDATE/CARD/CV_DATA")
        End If
        If Not (vNodeRoot Is Nothing) Then
            If sCCPin <> "" Then
                vNodeRoot.Attributes.GetNamedItem("cv2").InnerText = sCCPin
            Else
                vNodeRoot.Attributes.RemoveNamedItem("cv2")
            End If

            sCCAddress1 = NumbersOnly(sCCAddress1)
            If sCCAddress1 <> "" Then
                vNodeRoot.Attributes.GetNamedItem("address").InnerText = sCCAddress1
            Else
                vNodeRoot.Attributes.RemoveNamedItem("address")
            End If

            sCCPostcode = NumbersOnly(sCCPostcode)
            If sCCPostcode <> "" Then
                vNodeRoot.Attributes.GetNamedItem("postCode").InnerText = sCCPostcode
            Else
                vNodeRoot.Attributes.RemoveNamedItem("postCode")
            End If
        End If

        'set transaction details
        vNodeRoot = xmlDoc.SelectSingleNode("RLSOLVE_MSG/REQUEST/AUTH_REQ/TRANSACTION")
        If vNodeRoot Is Nothing Then
            vNodeRoot = xmlDoc.SelectSingleNode("RLSOLVE_MSG/REQUEST/VALIDATE/TRANSACTION")
        End If
        If Not (vNodeRoot Is Nothing) Then
            vNodeRoot.Attributes.GetNamedItem("customer").InnerText = sCCCustomerPresent
            vNodeRoot.Attributes.GetNamedItem("source").InnerText = sCCKeyedOrSwiped
            vNodeRoot.SelectSingleNode("AMOUNT")
            If vNodeRoot.ChildNodes.Count > 0 Then
                'Remove decimals
                vNodeRoot.ChildNodes.Item(0).InnerText = gPMFunctions.ToSafeString(cCCAmount * 100)
            End If
            If bIsReceipt Then
                vNodeRoot.Attributes.GetNamedItem("type").InnerText = "purchase"
            Else
                vNodeRoot.Attributes.GetNamedItem("type").InnerText = "refund"
            End If
        End If

        'set authorisation code
        vNodeRoot = xmlDoc.SelectSingleNode("RLSOLVE_MSG/REQUEST/AUTH_REQ/AUTH_CODE")
        If Not (vNodeRoot Is Nothing) Then
            vNodeRoot.ChildNodes.Item(0).InnerText = r_sCCAuthorisationCode
        End If

        'source and transaction ids
        vNodeRoot = xmlDoc.SelectSingleNode("RLSOLVE_MSG/REQUESTER")
        If Not (vNodeRoot Is Nothing) Then
            vNodeRoot.SelectSingleNode("SOURCE")
            vNodeRoot.ChildNodes.Item(0).Attributes.GetNamedItem("id").InnerText = sSourceID
            vNodeRoot.ChildNodes.Item(1).InnerText = r_sCCTransactionCode
        End If
        r_sXML = xmlDoc.InnerXml

        r_oWinsock.SendData(xmlDoc.InnerXml)

        'now wait for a result, error or timeout
        'wait for a connection, an error or timeout
        cStart = DateTime.Now.TimeOfDay.TotalSeconds ' start the timer
        m_frmWinSock.m_sReceivedXml = "" 'clear reasult string

        Do

            Application.DoEvents()
            If m_frmWinSock.m_sReceivedXml <> "" Then 'if got result
                r_sResultXML = m_frmWinSock.m_sReceivedXml
                DecodeResultXML(sXML:=r_sResultXML, r_iExtendedError:=r_iExtendedError, r_sExtendedError:=r_sExtendedError, r_sAuthCode:=r_sCCAuthorisationCode)

                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If cStart + iTimeout < DateTime.Now.TimeOfDay.TotalSeconds Then 'timed out
                r_iExtendedError = MainModule.eSolveSEExtendedError.eConnectionTimeout
                r_sExtendedError = DecodeError(MainModule.eSolveSEExtendedError.eConnectionTimeout)
                r_oWinsock.Close()
                Return result
            End If

            If m_frmWinSock.m_sWinsockError <> "" Then ' a winsock error has occurred
                r_iExtendedError = MainModule.eSolveSEExtendedError.eConnectionError
                r_sExtendedError = m_frmWinSock.m_sWinsockError
                Return result
            End If

        Loop While True

        Return result


    End Function

    ' ***************************************************************** '
    '
    ' Name: GetXmlForMessageId
    '
    ' Description:
    '
    ' History: 05/11/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function GetXMLForMessageID(ByVal iSolveSEMessageID As MainModule.eSolveSEMessage, ByRef r_sXML As String) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue 'okay

        For iEntryCount As Integer = 0 To m_vSolveXmlTransactions.GetUpperBound(0)
            If m_vSolveXmlTransactions(iEntryCount)(0) = iSolveSEMessageID Then
                r_sXML = CStr(m_vSolveXmlTransactions(iEntryCount)(2))
                r_sXML = r_sXML.Replace("&", "&amp;")
                r_sXML = r_sXML.Replace("DTD_PATHNAME", GetDTDPath())
                Return result
            End If
        Next
        'error

        Return gPMConstants.PMEReturnCode.PMFalse


    End Function


    'make this call synchronous
    Private Function Connect(ByVal sHost As String, ByVal sPort As String, ByVal iTimeout As Integer, ByRef r_sCCTransactionCode As String, ByRef r_iExtendedError As MainModule.eSolveSEExtendedError, ByRef r_sExtendedError As String) As Integer

        Dim result As Integer = 0

        Dim r_oWinsock As AxMSWinsockLib.AxWinsock = m_frmWinSock.Winsock1

        result = gPMConstants.PMEReturnCode.PMFalse

        If m_iSolveSEState <> MainModule.eSolveSEState.eFormLoaded Then
            r_iExtendedError = MainModule.eSolveSEExtendedError.eNotInitialised
            Return result
        End If

        Disconnect()

        r_oWinsock.RemoteHost = sHost
        r_oWinsock.RemotePort = CInt(sPort)
        r_oWinsock.Protocol = MSWinsockLib.ProtocolConstants.sckTCPProtocol
        r_oWinsock.Connect()

        'wait for a connection, an error or timeout
        Dim cStart As Single = DateTime.Now.TimeOfDay.TotalSeconds ' used in timer operation 'get the timer value

        Do

            Application.DoEvents()
            If r_oWinsock.CtlState = 7 Then 'if connected
                result = gPMConstants.PMEReturnCode.PMTrue
                r_sCCTransactionCode = GetSessionCode()
                Return result
            End If

            If cStart + iTimeout < DateTime.Now.TimeOfDay.TotalSeconds Then 'timed out
                r_iExtendedError = MainModule.eSolveSEExtendedError.eConnectionTimeout
                r_oWinsock.Close()
                Return result
            End If

            If m_frmWinSock.m_sWinsockError <> "" Then ' a winsock error has occurred
                r_iExtendedError = MainModule.eSolveSEExtendedError.eConnectionError
                r_sExtendedError = m_frmWinSock.m_sWinsockError
                Return result
            End If
        Loop While True

        Return result
    End Function

    Private Sub Disconnect()
        'Disconnect the tcp/ip session
        If m_frmWinSock.Winsock1.CtlState <> MSWinsockLib.StateConstants.sckClosed Then
            m_frmWinSock.Winsock1.Close()
        End If
    End Sub


    Private Sub DecodeResultXML(ByVal sXML As String, ByRef r_iExtendedError As MainModule.eSolveSEExtendedError, ByRef r_sExtendedError As String, ByRef r_sAuthCode As String)
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DecodeResultXML
        ' PURPOSE: Extracts the information and authorisation code from the Result XML
        ' AUTHOR: Danny Davis
        ' DATE: 13 January 2005, 15:06:33
        ' CHANGES:
        ' ---------------------------------------------------------------------------






        Dim vNodeRoot As XmlNode
        Dim xmlDoc As XmlDocument
        Dim sLogMsg, sErrorCode As String
        Dim sMessage As New StringBuilder

        xmlDoc = New XmlDocument()

        'developer guide no. no solution 22
        'xmlDoc.async = False

        sMessage = New StringBuilder("")

        'Correct irregularities
        sXML = sXML.Replace("&", "&amp;")
        sXML = sXML.Replace("DTD_PATHNAME", GetDTDPath())


        Try
            xmlDoc.LoadXml(sXML)



        Catch parseError As System.Xml.XmlException
            With parseError
                'MsgBox



                'developer guide no solution no. 22
                '  sLogMsg = "document Parse Error:" & Strings.Chr(13) & Strings.Chr(10) & _
                '"Code: " & CStr(.errorCode) & Strings.Chr(13) & Strings.Chr(10) & _
                '"Line: " & CStr(CType(parseError, XmlException).LineNumber) & Strings.Chr(13) & Strings.Chr(10) & _
                '"lPos: " & CStr(CType(parseError, XmlException).LinePosition) & Strings.Chr(13) & Strings.Chr(10) & _
                '"Reason: " & .Message & Strings.Chr(13) & Strings.Chr(10) & _
                '"Src: " & .srcText & Strings.Chr(13) & Strings.Chr(10) & _
                '"fPos: " & CStr(.filepos)


                sLogMsg = "document Parse Error:" & Strings.Chr(13) & Strings.Chr(10) & _
              "Line: " & CStr(CType(parseError, XmlException).LineNumber) & Strings.Chr(13) & Strings.Chr(10) & _
              "lPos: " & CStr(CType(parseError, XmlException).LinePosition) & Strings.Chr(13) & Strings.Chr(10) & _
              "Reason: " & .Message & Strings.Chr(13) & Strings.Chr(10) & _
              "Src: " & .Source & Strings.Chr(13) & Strings.Chr(10) & _
              "fPos: " & CStr(.SourceUri)

                ' Log Error Message
            End With

            'developer guide no solution no. 22
            'gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendSolveSEMessage Failed - " & sLogMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="SendSolveSEMessage", vErrNo:=CStr(parseError.errorCode), vErrDesc:=CStr(parseError.errorCode))
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("r_sAuthCode", r_sAuthCode)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendSolveSEMessage Failed - " & sLogMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="SendSolveSEMessage", excep:=parseError, oDicParms:=oDict)

        End Try

        'Decode error code
        vNodeRoot = xmlDoc.SelectSingleNode("RLSOLVE_MSG/RESPONSE")

        If Not (vNodeRoot.GetType().Name = "Nothing") Then
            sErrorCode = vNodeRoot.Attributes.GetNamedItem("label").InnerText

            Select Case sErrorCode
                Case k_eValidated, k_eOnline, k_eCancelled, k_eGet_sig_auth
                    r_iExtendedError = MainModule.eSolveSEExtendedError.eOkay
                Case k_eGet_manual_auth, k_eGet_manual_auth_resubmit
                    r_iExtendedError = MainModule.eSolveSEExtendedError.eConnectionError
                Case Else
                    r_iExtendedError = MainModule.eSolveSEExtendedError.eAuthorisationFailed
            End Select
        End If

        'get the authorisation code
        vNodeRoot = xmlDoc.SelectSingleNode("RLSOLVE_MSG/RESPONSE/AUTH_RESP/AUTH_CODE")

        If Not (vNodeRoot.GetType().Name = "Nothing") Then
            sMessage = New StringBuilder(DecodeSolveSEResponse(vNodeRoot.Attributes.GetNamedItem("response").InnerText).ToUpper())
            r_sAuthCode = vNodeRoot.InnerText
        End If

        vNodeRoot = xmlDoc.SelectSingleNode("RLSOLVE_MSG/RESPONSE/ERRORS")

        If Not (vNodeRoot.GetType().Name = "Nothing") Then
            For errorCount As Integer = 1 To vNodeRoot.ChildNodes.Count
                sMessage.Append(Strings.Chr(13) & Strings.Chr(10) & vNodeRoot.ChildNodes.Item(errorCount - 1).InnerText)
            Next
        End If

        '<MESSAGE type="09" seqNum="0063"> KEEP CRD DECLINE </MESSAGE>
        vNodeRoot = xmlDoc.SelectSingleNode("RLSOLVE_MSG/RESPONSE/APACS30/MESSAGE")
        If Not (vNodeRoot Is Nothing) Then
            sMessage.Append(Strings.Chr(13) & Strings.Chr(10) & vNodeRoot.InnerText)
        End If

        r_sExtendedError = sMessage.ToString().Replace(Constants.vbLf, Strings.Chr(13) & Strings.Chr(10))

        GoTo Finally_Renamed

        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------



Catch_Renamed:
        Select Case Information.Err().Number
            Case Else
                ' Log Error.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DecodeResultXML", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                GoTo Finally_Renamed
        End Select

Finally_Renamed:
        Exit Sub


    End Sub

    ' ***************************************************************** '
    '
    ' Name: DecodeExtendedError
    '
    ' Description: Returns the string error message for the error id
    '
    ' History: 07/11/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function DecodeError(ByVal v_iExtenderErrorId As Integer) As String




        For iIterator As Integer = 0 To m_vErrorMessages.GetUpperBound(0)
            If v_iExtenderErrorId = CDbl(m_vErrorMessages(iIterator)(0)) Then
                Return CStr(m_vErrorMessages(iIterator)(1))
            End If
        Next


        Return "Unknown error"

    End Function


    ' ***************************************************************** '
    '
    ' Name: SolveSE
    '
    ' Description:
    ' Must be one sychronous call to ensure that winsock control is not shared
    '
    ' History: 07/11/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function SolveSE(ByVal iSolveSEMessageID As MainModule.eSolveSEMessage, ByVal sHost As String, ByVal sPort As String, ByVal iTimeout As Integer, ByVal bIsReceipt As Boolean, ByVal sSourceID As String, ByVal sCCNumber As String, ByVal sCCStartDate As String, ByVal sCCEndDate As String, ByVal sCCIssue As String, ByVal sCCPin As String, ByVal sCCAddress1 As String, ByVal sCCPostcode As String, ByVal sCCCustomerPresent As String, ByVal sCCKeyedOrSwiped As String, ByVal cCCAmount As Decimal, ByRef r_sCCAuthorisationCode As String, ByRef r_sCCTransactionCode As String, Optional ByRef r_iExtendedError As MainModule.eSolveSEExtendedError = 0, Optional ByRef r_sExtendedError As String = "", Optional ByRef r_sXML As String = "", Optional ByRef r_sResultXML As String = "") As Integer

        Dim result As Integer = 0
        Dim bIsConnected As Boolean
        Try

            Dim sLastTransactionId As String = ""

            bIsConnected = False

            'if follow on messages "cancel", "complete transaction"
            'use previous ID
            If iSolveSEMessageID = MainModule.eSolveSEMessage.eCancelTransaction Or iSolveSEMessageID = MainModule.eSolveSEMessage.eCompleteTransaction Then
                sLastTransactionId = r_sCCTransactionCode
            End If

            m_lReturn = CType(Connect(sHost, sPort, iTimeout, r_sCCTransactionCode, r_iExtendedError, r_sExtendedError), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bIsConnected = True

                'if follow on messages "cancel", "complete transaction"
                'use previous ID
                If iSolveSEMessageID = MainModule.eSolveSEMessage.eCancelTransaction Or iSolveSEMessageID = MainModule.eSolveSEMessage.eCompleteTransaction Then
                    r_sCCTransactionCode = sLastTransactionId
                End If

                m_lReturn = CType(SendSolveSEMessage(iSolveSEMessageID:=iSolveSEMessageID, iTimeout:=iTimeout, bIsReceipt:=bIsReceipt, sSourceID:=sSourceID, sCCNumber:=sCCNumber, sCCStartDate:=sCCStartDate, sCCEndDate:=sCCEndDate, sCCIssue:=sCCIssue, sCCPin:=sCCPin, sCCAddress1:=sCCAddress1, sCCPostcode:=sCCPostcode, sCCCustomerPresent:=sCCCustomerPresent, sCCKeyedOrSwiped:=sCCKeyedOrSwiped, cCCAmount:=cCCAmount, r_sCCAuthorisationCode:=r_sCCAuthorisationCode, r_sCCTransactionCode:=r_sCCTransactionCode, r_sXML:=r_sXML, r_sResultXML:=r_sResultXML, r_iExtendedError:=r_iExtendedError, r_sExtendedError:=r_sExtendedError), gPMConstants.PMEReturnCode)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    'Analyse the extended error
                    If r_iExtendedError = MainModule.eSolveSEExtendedError.eAuthorisationFailed Then
                        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                    ElseIf r_iExtendedError <> MainModule.eSolveSEExtendedError.eOkay Then
                        'Return PMFail for communication errors
                        m_lReturn = gPMConstants.PMEReturnCode.PMFail
                    End If
                End If
            Else
                If r_iExtendedError = MainModule.eSolveSEExtendedError.eAuthorisationFailed Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                Else
                    'Return PMFail for communication errors
                    m_lReturn = gPMConstants.PMEReturnCode.PMFail
                End If
            End If

            Disconnect()

            'Send the value back
            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            If bIsConnected Then 'must disconnect
                Disconnect()
            End If

            ' Log Error Message
            bPMFunc.LogMessage(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SolveSE Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SolveSE", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DecodeSolveSEResponse
    '
    ' Description:
    '
    ' History: 07/11/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Private Function DecodeSolveSEResponse(ByVal sSolveSEResponseID As String) As String




        For iIterator As Integer = 0 To m_vAuthorisationMessages.GetUpperBound(0)
            If sSolveSEResponseID = CStr(m_vAuthorisationMessages(iIterator)(0)) Then
                Return CStr(m_vAuthorisationMessages(iIterator)(1))
            End If
        Next


    End Function

    Private Function GetDTDPath() As String
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetDTDPath
        ' PURPOSE: Return the DTD used by the SolveSE Interface
        ' AUTHOR: Danny Davis
        ' DATE: 13 January 2005, 15:11:26
        ' RETURNS: Local path on the server machine
        ' ---------------------------------------------------------------------------

        Dim sPMPath As String = ""

        gPMFunctions.GetPMRegSetting(gPMConstants.HKEY_LOCAL_MACHINE, 0, 0, "PMDIR", sPMPath)
        sPMPath = sPMPath & "\" & k_sSolveSEDTD
    End Function

    Private Function GetSessionCode() As String
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetSessionCode
        ' PURPOSE: Returns a random session ID
        '          (unsigned int as a string, 0-65535, 5 chars)
        ' AUTHOR: Danny Davis
        ' DATE: 13 January 2005, 15:11:26
        ' RETURNS: Session ID string
        ' ---------------------------------------------------------------------------

        Return CStr(CInt(65536 * VBMath.Rnd()))
    End Function

    Private Function NumbersOnly(ByRef sAlphaString As String) As String
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: NumbersOnly
        ' PURPOSE: Strips all non-numbers from a string.
        '          Required for Addresses and Postcodes
        ' AUTHOR: Danny Davis
        ' DATE: 13 January 2005, 15:11:26
        ' RETURNS: Stripped string
        ' ---------------------------------------------------------------------------


        Dim sResult As New StringBuilder
        Dim iLen As Integer = sAlphaString.Length

        For i As Integer = 1 To iLen
            If Mid(sAlphaString, i, 1) >= "0" And Mid(sAlphaString, i, 1) <= "9" Then
                sResult.Append(Mid(sAlphaString, i, 1))
            End If
        Next i

        Return sResult.ToString()
    End Function
End Class

