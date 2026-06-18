Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("QuotePolicy_NET.QuotePolicy")>
Public NotInheritable Class QuotePolicy
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: QuotePolicy
    '
    ' Date: RFC220600
    '
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 19/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "QuotePolicy"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ''' <summary>
    ''' Add the party Details
    ''' </summary>
    ''' <param name="v_sGisDataModelCode"></param>
    ''' <param name="v_sGisBusinessTypeCode"></param>
    ''' <param name="v_sPartyTypeCode"></param>
    ''' <param name="v_sForename"></param>
    ''' <param name="v_sSurname"></param>
    ''' <param name="v_sDateOfBirth"></param>
    ''' <param name="v_sEmailAddress"></param>
    ''' <param name="v_sCurrentRenewalDate"></param>
    ''' <param name="v_sAddress1"></param>
    ''' <param name="v_sAddress2"></param>
    ''' <param name="v_sAddress3"></param>
    ''' <param name="v_sAddress4"></param>
    ''' <param name="v_sPostcode"></param>
    ''' <param name="r_lPartyCnt"></param>
    ''' <param name="v_sTitle"></param>
    ''' <param name="v_sMaritalStatusCode"></param>
    ''' <param name="v_sGenderCode"></param>
    ''' <param name="v_sInitials"></param>
    ''' <param name="v_sTelephoneNumber"></param>
    ''' <param name="v_sTradingName"></param>
    ''' <param name="r_vAdditionalDataArray"></param>
    ''' <param name="v_sAddress5"></param>
    ''' <param name="v_sAddress6"></param>
    ''' <param name="v_sAddress7"></param>
    ''' <param name="v_sAddress8"></param>
    ''' <param name="v_sAddress9"></param>
    ''' <param name="v_sAddress10"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String,
                             ByVal v_sPartyTypeCode As String, ByVal v_sForename As String,
                             ByVal v_sSurname As String, ByVal v_sDateOfBirth As String,
                             ByVal v_sEmailAddress As String, ByVal v_sCurrentRenewalDate As String,
                             ByVal v_sAddress1 As String, ByVal v_sAddress2 As String,
                             ByVal v_sAddress3 As String, ByVal v_sAddress4 As String,
                             ByVal v_sPostcode As String, ByRef r_lPartyCnt As Object,
                             Optional ByVal v_sTitle As String = "",
                             Optional ByVal v_sMaritalStatusCode As String = "",
                             Optional ByVal v_sGenderCode As String = "",
                             Optional ByVal v_sInitials As String = "", Optional ByVal v_sTelephoneNumber As String = "",
                             Optional ByVal v_sTradingName As String = "",
                             Optional ByRef r_vAdditionalDataArray As Object = Nothing,
                             Optional ByVal sAddress5 As String = "", Optional ByVal sAddress6 As String = "",
                             Optional ByVal sAddress7 As String = "", Optional ByVal sAddress8 As String = "",
                             Optional ByVal sAddress9 As String = "", Optional ByVal sAddress10 As String = "") As Object

        Dim nResult As Integer = nResult = gPMConstants.PMEReturnCode.PMTrue
        Dim oBOM As Object
        Dim oBOMAOL As bGISBOMAOL.QuotePolicy
        Try

            ' RJG 21/11/2000
            ' Use the CreateBOM function to create the BOM (if required)
            If v_sGisDataModelCode.ToUpper = "AOL" AndAlso ACClass.ToUpper = "QUOTEPOLICY" Then
                oBOMAOL = New bGISBOMAOL.QuotePolicy

                nResult = oBOMAOL.Initialise(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                nResult = oBOMAOL.AddParty(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode,
                                        v_sPartyTypeCode:=v_sPartyTypeCode, v_sForename:=v_sForename, v_sSurname:=v_sSurname, v_sDateOfBirth:=v_sDateOfBirth,
                                        v_sEmailAddress:=v_sEmailAddress, v_sCurrentRenewalDate:=v_sCurrentRenewalDate, v_sAddress1:=v_sAddress1,
                                        v_sAddress2:=v_sAddress2, v_sAddress3:=v_sAddress3, v_sAddress4:=v_sAddress4, v_sPostcode:=v_sPostcode,
                                        r_lPartyCnt:=r_lPartyCnt, v_sTitle:=v_sTitle, v_sMaritalStatusCode:=v_sMaritalStatusCode,
                                        v_sGenderCode:=v_sGenderCode, v_sInitials:=v_sInitials, v_sTelephoneNumber:=v_sTelephoneNumber,
                                        v_sTradingName:=v_sTradingName, r_vAdditionalDataArray:=r_vAdditionalDataArray,
                                        sAddress5:=sAddress5,
                                        sAddress6:=sAddress6, sAddress7:=sAddress7,
                                        sAddress8:=sAddress8, sAddress9:=sAddress9,
                                        sAddress10:=sAddress10)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If
                oBOMAOL.Dispose()
                oBOMAOL = Nothing
            Else
                nResult = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID),
                                iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If

                If Not (oBOM Is Nothing) Then

                    nResult = oBOM.AddParty(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode),
                                        v_sPartyTypeCode:=gPMFunctions.ToSafeString(v_sPartyTypeCode), v_sForename:=gPMFunctions.ToSafeString(v_sForename), v_sSurname:=gPMFunctions.ToSafeString(v_sSurname), v_sDateOfBirth:=gPMFunctions.ToSafeString(v_sDateOfBirth),
                                        v_sEmailAddress:=gPMFunctions.ToSafeString(v_sEmailAddress), v_sCurrentRenewalDate:=gPMFunctions.ToSafeString(v_sCurrentRenewalDate), v_sAddress1:=gPMFunctions.ToSafeString(v_sAddress1),
                                        v_sAddress2:=gPMFunctions.ToSafeString(v_sAddress2), v_sAddress3:=gPMFunctions.ToSafeString(v_sAddress3), v_sAddress4:=gPMFunctions.ToSafeString(v_sAddress4), v_sPostcode:=gPMFunctions.ToSafeString(v_sPostcode),
                                        r_lPartyCnt:=r_lPartyCnt, v_sTitle:=gPMFunctions.ToSafeString(v_sTitle), v_sMaritalStatusCode:=gPMFunctions.ToSafeString(v_sMaritalStatusCode),
                                        v_sGenderCode:=gPMFunctions.ToSafeString(v_sGenderCode), v_sInitials:=gPMFunctions.ToSafeString(v_sInitials), v_sTelephoneNumber:=gPMFunctions.ToSafeString(v_sTelephoneNumber),
                                        v_sTradingName:=gPMFunctions.ToSafeString(v_sTradingName), r_vAdditionalDataArray:=r_vAdditionalDataArray,
                                        sAddress5:=gPMFunctions.ToSafeString(sAddress5),
                                        sAddress6:=gPMFunctions.ToSafeString(sAddress6), sAddress7:=gPMFunctions.ToSafeString(sAddress7),
                                        sAddress8:=gPMFunctions.ToSafeString(sAddress8), sAddress9:=gPMFunctions.ToSafeString(sAddress9),
                                        sAddress10:=gPMFunctions.ToSafeString(sAddress10))

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nResult
                    End If

                    ' Destroy the BOM.QuotePolicy class

                    oBOM.Dispose()
                    oBOM = Nothing
                End If
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParty", excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindQuote
    '
    ' Description: Find Quotes given one or all of
    ' Reference, Description and Cover Start Date
    '
    ' RJG 04/12/2000 - New method
    ' ***************************************************************** '
    Public Function FindQuote(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_vResultArray As Object, Optional ByVal v_sQuoteRef As Object = Nothing, Optional ByVal v_dCoverStartDate As Object = Nothing, Optional ByVal v_sDescription As Object = Nothing, Optional ByVal v_lLeadAgentCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RJG 04/12/2000
            ' Use the CreateBOM function to create the BOM (if required)
            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.FindQuote(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_vResultArray:=r_vResultArray, v_sQuoteRef:=v_sQuoteRef, v_dCoverStartDate:=v_dCoverStartDate, v_sDescription:=v_sDescription, v_lLeadAgentCnt:=gPMFunctions.ToSafeInteger(v_lLeadAgentCnt))

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetQuotesForParty
    '
    ' Description: List all Quotes of the type requested for
    '              the PartyCnt.
    '
    '              If NO type specified Quotes of ALL types
    '              are returned.
    '
    '              PolicyTypeCode = "MOTOR", "HOME", "TRAVEL" etc
    ' ***************************************************************** '
    Public Function GetQuotesForParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByRef r_vQuoteArray As Object) As Integer
        Return GetQuotesForParty(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lPartyCnt:=v_lPartyCnt, r_vQuoteArray:=r_vQuoteArray, v_sPolicyTypeCode:=Nothing)
    End Function

    Public Function GetQuotesForParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByRef r_vQuoteArray As Object, ByVal v_sPolicyTypeCode As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RJG 21/11/2000
            ' Use the CreateBOM function to create the BOM (if required)
            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.GetQuotesForParty(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), r_vQuoteArray:=r_vQuoteArray, v_sPolicyTypeCode:=gPMFunctions.ToSafeString(v_sPolicyTypeCode))

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotesForParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuotesForParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        'LogMessageToFile _
        'sUsername:="", _
        'iType:=PMLogOnError, _
        'sMsg:="bGIS.initialise starting...", _
        'vApp:=ACApp, _
        'vClass:=ACClass, _
        'vMethod:="bGIS"  ' TEMPDEBUG

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            lReturn = CType(gPMComponentServices.CheckDatabase(v_sUsername:=gPMFunctions.ToSafeString(m_sUsername), v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' RFC170300 - Don't do anything with the Database supplied
            ' as we will always create one later which is specific to the
            ' Data Model Code in use.

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

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
        End If
        Me.disposedValue = True
    End Sub


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
    Public Function GetQuotesPoliciesForParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByVal v_lSearchType As Integer, ByRef r_vQuotePolicyArray As Object) As Integer
        Return GetQuotesPoliciesForParty(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lPartyCnt:=v_lPartyCnt, v_lSearchType:=v_lSearchType, r_vQuotePolicyArray:=r_vQuotePolicyArray, v_sPolicyTypeCode:=Nothing)
    End Function

    Public Function GetQuotesPoliciesForParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByVal v_lSearchType As Integer, ByRef r_vQuotePolicyArray As Object, ByVal v_sPolicyTypeCode As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue, lPartyCnt As Integer
        Dim sClassBOMAppName As String = ""
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'LogMessageToFile m_sUsername, PMLogOnError, "Start", "bGIS", "QuotePolicy", "GetQuotesPoliciesForParty"

            '
            ' Create the BackOfficeMapper QuotePolicy class for the given DataModel
            '
            'sClassBOMAppName = "bGISBOM" & v_sGISDataModelCode & ".QuotePolicy"
            'Set oBOM = CreateObject(sClassBOMAppName)

            'lReturn = oBOM.Initialise( _
            'm_sUsername, _
            'm_sPassword, _
            'm_iUserID, _
            'm_iSourceID, _
            'm_iLanguageID, _
            'm_iCurrencyID, _
            'm_iLogLevel, _
            'ACApp)

            ' RAG 16/11/00
            ' Use the CreateBOM function to create the BOM (if required)
            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                ' Call the GetQuotesPoliciesForParty method of the specific bGISBOMx.QuotePolicy class

                lReturn = oBOM.GetQuotesPoliciesForParty(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lSearchType:=gPMFunctions.ToSafeInteger(v_lSearchType), r_vQuotePolicyArray:=r_vQuotePolicyArray, v_sPolicyTypeCode:=gPMFunctions.ToSafeString(v_sPolicyTypeCode))

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing
            End If

            'LogMessageToFile m_sUsername, PMLogOnError, "End", "bGIS", "QuotePolicy", "GetQuotesPoliciesForParty"

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotesPoliciesForParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuotesPoliciesForParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function GetPolicyVersions(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_vPolicyVersionArray As Object) As Integer
        Return GetPolicyVersions(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_vPolicyVersionArray:=r_vPolicyVersionArray, v_lInsuranceFileCnt:=-1, v_sInsuranceFileRef:="")
    End Function
    Public Function GetPolicyVersions(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_vPolicyVersionArray As Object, ByVal v_lInsuranceFileCnt As Integer) As Integer
        Return GetPolicyVersions(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_vPolicyVersionArray:=r_vPolicyVersionArray, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sInsuranceFileRef:="")
    End Function
    Public Function GetPolicyVersions(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_vPolicyVersionArray As Object, ByVal v_sInsuranceFileRef As String) As Integer
        Return GetPolicyVersions(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_vPolicyVersionArray:=r_vPolicyVersionArray, v_lInsuranceFileCnt:=-1, v_sInsuranceFileRef:=v_sInsuranceFileRef)
    End Function
    Public Function GetPolicyVersions(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_vPolicyVersionArray As Object, ByVal v_lInsuranceFileCnt As Object, ByVal v_sInsuranceFileRef As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue, lPartyCnt As Integer

        Dim sClassBOMAppName As String = ""
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'LogMessageToFile m_sUsername, PMLogOnError, "Start", "bGIS", "QuotePolicy", "GetPolicyVersions"

            '
            ' Create the BackOfficeMapper QuotePolicy class for the given DataModel
            '
            'sClassBOMAppName = "bGISBOM" & v_sGISDataModelCode & ".QuotePolicy"
            'Set oBOM = CreateObject(sClassBOMAppName)

            'lReturn = oBOM.Initialise( _
            'm_sUsername, _
            'm_sPassword, _
            'm_iUserID, _
            'm_iSourceID, _
            'm_iLanguageID, _
            'm_iCurrencyID, _
            'm_iLogLevel, _
            'ACApp)

            ' RAG 16/11/00
            ' Use the CreateBOM function to create the BOM (if required)
            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.GetPolicyVersions(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vPolicyVersionArray:=r_vPolicyVersionArray, v_sInsuranceFileRef:=gPMFunctions.ToSafeString(v_sInsuranceFileRef))

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            'LogMessageToFile m_sUsername, PMLogOnError, "End", "bGIS", "QuotePolicy", "GetPolicyVersions"

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyVersions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: FindParty
    '
    ' Description: Returns a list of Parties given a search Criteria
    '
    ' RJG 21/11/2000 - New method
    ' CTAF 20030623 - Merged in from 1.9 (extra optional address parameter + code)
    '
    ' ***************************************************************** '
    Public Function FindParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sPartyType As String, ByVal v_sShortname As String, ByVal v_sResolvedName As String, ByVal v_sUserID As String, ByVal v_sTelephoneNumber As String, ByVal v_sPostcode As String, ByRef r_vResultArray As Object, Optional ByVal v_lLeadAgentCnt As Integer = 0, Optional ByVal v_vAdditionalDataArray As Object = Nothing, Optional ByVal v_sAddress1 As String = "", Optional ByVal v_sFileCode As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RJG 21/11/2000
            ' Use the CreateBOM function to create the BOM (if required)
            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                '**** START CHANGES - Changed By: AAB  - Changed On: 04-Sep-2002 15:42   ****
                '**** Added this to support calling a BOM.FindParty method with or without the
                '**** v_sAddress1 parameter.  This was done to support Agents On Line.
                If v_sAddress1 <> "" Then
                    'Call a BOM method with v_sAddress1 parameter

                    lReturn = oBOM.FindParty(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sPartyType:=gPMFunctions.ToSafeString(v_sPartyType), v_sShortname:=gPMFunctions.ToSafeString(v_sShortname),
                                             v_sResolvedName:=gPMFunctions.ToSafeString(v_sResolvedName), v_sUserID:=gPMFunctions.ToSafeString(v_sUserID), v_sTelephoneNumber:=gPMFunctions.ToSafeString(v_sTelephoneNumber), v_sPostcode:=gPMFunctions.ToSafeString(v_sPostcode), r_vResultArray:=r_vResultArray, v_lLeadAgentCnt:=gPMFunctions.ToSafeInteger(v_lLeadAgentCnt), v_vAdditionalDataArray:=v_vAdditionalDataArray, v_sAddress1:=gPMFunctions.ToSafeString(v_sAddress1), v_sFileCode:=gPMFunctions.ToSafeString(v_sFileCode))
                Else
                    'Call a BOM method WITHOUT the v_sAddress1 parameter

                    lReturn = oBOM.FindParty(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sPartyType:=gPMFunctions.ToSafeString(v_sPartyType),
                                             v_sShortname:=gPMFunctions.ToSafeString(v_sShortname), v_sResolvedName:=gPMFunctions.ToSafeString(v_sResolvedName), v_sUserID:=gPMFunctions.ToSafeString(v_sUserID), v_sTelephoneNumber:=gPMFunctions.ToSafeString(v_sTelephoneNumber),
                                             v_sPostcode:=gPMFunctions.ToSafeString(v_sPostcode), r_vResultArray:=r_vResultArray, v_lLeadAgentCnt:=gPMFunctions.ToSafeInteger(v_lLeadAgentCnt), v_vAdditionalDataArray:=v_vAdditionalDataArray, v_sFileCode:=gPMFunctions.ToSafeString(v_sFileCode))
                End If
                '****   END CHANGES - Changed By: AAB  - Changed On: 04-Sep-2002 15:42   ****
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Destroy the BOM.QuotePolicy class

            oBOM.Dispose()
            oBOM = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindPolicy
    '
    ' Description: Returns a list of policies given a search Criteria
    '
    ' CL240101
    ' ***************************************************************** '
    Public Function FindPolicy(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_vResultArray As Object) As Integer
        Return FindPolicy(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_vResultArray:=r_vResultArray, v_vAdditionalDataArray:=Nothing)
    End Function

    Public Function FindPolicy(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_vResultArray As Object, ByVal v_vAdditionalDataArray As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.FindPolicy(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_vResultArray:=r_vResultArray, r_vAdditionalDataArray:=v_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProductByAgent
    '
    ' Description: Returns a list of Parties given a search Criteria
    '
    ' RJG 09/01/2001 - New method
    ' ***************************************************************** '
    Public Function GetProductByAgent(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lAgentPartyCnt As Integer, ByRef r_vResultArray As Object) As Integer
        Return GetProductByAgent(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lAgentPartyCnt:=v_lAgentPartyCnt, r_vResultArray:=r_vResultArray, v_vAdditionalDataArray:=Nothing)
    End Function

    Public Function GetProductByAgent(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lAgentPartyCnt As Integer, ByRef r_vResultArray As Object, ByVal v_vAdditionalDataArray As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RJG 09/01/2001
            ' Use the CreateBOM function to create the BOM (if required)
            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.GetProductByAgent(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lAgentPartyCnt:=gPMFunctions.ToSafeInteger(v_lAgentPartyCnt), r_vResultArray:=r_vResultArray, v_vAdditionalDataArray:=v_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductByAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductByAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetParty
    '
    ' Description: Returns the party details for a given Party
    '
    ' RDT 18/02/2005 - Updated.  Added CountryCode parameterfor use in the STS
    ' ***************************************************************** '
    Public Function GetParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByRef r_sSurname As Object, ByRef r_sForename As Object, ByRef r_sPartyType As Object, ByRef r_sAddress1 As Object, ByRef r_sAddress2 As Object, ByRef r_sAddress3 As Object, ByRef r_sAddress4 As Object, ByRef r_sPostcode As Object, ByRef r_sDOB As Object, ByRef r_sEMail As Object, ByRef r_sUserID As Object, ByRef r_sPassword As Object, ByRef r_sShortName As Object, ByRef r_sResolvedName As Object, Optional ByRef r_sMothersMaidenName As Object = Nothing, Optional ByRef r_sTPUserCode As Object = Nothing, Optional ByRef r_sTPIntroducer As Object = Nothing, Optional ByRef r_sAQuestion As Object = Nothing, Optional ByRef r_sTheAnswer As Object = Nothing, Optional ByRef r_dMemorableDate As Object = Nothing, Optional ByRef r_dCurrInsRenewalDate As Object = Nothing, Optional ByRef r_sTitle As Object = Nothing, Optional ByRef r_sMaritalStatusCode As Object = Nothing, Optional ByRef r_sGenderCode As Object = Nothing, Optional ByRef r_sInitials As Object = Nothing, Optional ByRef r_sTelephoneNumber As Object = Nothing, Optional ByRef r_sContactName As Object = Nothing, Optional ByRef r_sTradingName As Object = Nothing, Optional ByRef r_lPartyGroupTypeID As Object = Nothing, Optional ByRef r_bIsRegisteredCharity As Object = Nothing, Optional ByRef r_sNumberOfMembers As Object = Nothing, Optional ByRef r_sCharityNumber As Object = Nothing, Optional ByRef r_sOccupationCode As Object = Nothing, Optional ByRef r_vAllContactsArray As Object = Nothing, Optional ByRef r_sCountryCode As Object = Nothing, Optional ByRef r_lSourceId As Object = 0) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object
        Dim dDateOfBirth As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RJG 28/11/2000
            ' Use the CreateBOM function to create the BOM (if required)
            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=0, vErrDesc:=" CreateBOM " & v_sGisDataModelCode & " " & v_sGisBusinessTypeCode & " returned " & CStr(lReturn))
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.GetParty(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), r_sSurname:=r_sSurname,
                                        r_sForename:=r_sForename, r_sPartyType:=r_sPartyType, r_sAddress1:=r_sAddress1, r_sAddress2:=r_sAddress2,
                                        r_sAddress3:=r_sAddress3, r_sAddress4:=r_sAddress4, r_sPostcode:=r_sPostcode, r_sDOB:=r_sDOB, r_sEMail:=r_sEMail,
                                        r_sUserID:=r_sUserID, r_sPassword:=r_sPassword, r_sShortName:=r_sShortName, r_sResolvedName:=r_sResolvedName,
                                        r_sMothersMaidenName:=r_sMothersMaidenName, r_sTPUserCode:=r_sTPUserCode, r_sTPIntroducer:=r_sTPIntroducer, r_sAQuestion:=r_sAQuestion,
                                        r_sTheAnswer:=r_sTheAnswer, r_dMemorableDate:=r_dMemorableDate, r_dCurrInsRenewalDate:=r_dCurrInsRenewalDate, r_sTitle:=r_sTitle,
                                        r_sMaritalStatusCode:=r_sMaritalStatusCode, r_sGenderCode:=r_sGenderCode, r_sInitials:=r_sInitials, r_sTelephoneNumber:=r_sTelephoneNumber, r_sContactName:=r_sContactName,
                                        r_sTradingName:=r_sTradingName, r_lPartyGroupTypeID:=r_lPartyGroupTypeID, r_bIsRegisteredCharity:=r_bIsRegisteredCharity, r_sNumberOfMembers:=r_sNumberOfMembers, r_sCharityNumber:=r_sCharityNumber,
                                        r_sOccupationCode:=r_sOccupationCode, r_vAllContactsArray:=r_vAllContactsArray, r_sCountryCode:=r_sCountryCode, r_lSourceId:=r_lSourceId)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing
            Else
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=0, vErrDesc:=" CreateBOM " & v_sGisDataModelCode & " " & v_sGisBusinessTypeCode & " returned nothing")

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' FRIEND Methods (Begin)

    ' FRIEND Methods (End)

    ' PRIVATE Methods (Begin)

    '
    '
    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '


    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    '**** Added By: AAB  -  Added On:  19-Sep-2002 10:53 ****
    Public Function GetQuotes(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByRef r_vQuoteArray As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.GetQuotes(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), r_vQuoteArray:=r_vQuoteArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuotes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuotes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetQuoteDetailsSBO
    '
    ' Description: Broking version of GetQuoteDetails
    '
    ' History: 16/07/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetQuoteDetailsSBO(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFileCnt As Integer, ByRef r_vQuoteArray As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the BOM
            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.GetQuoteDetails(v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), v_bUnderwriting:=False, r_vQuoteArray:=r_vQuoteArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteDetailsSBO Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteDetailsSBO", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    '**** Added By: AAB  -  Added On:  19-Sep-2002 10:53 ****
    Public Function GetQuoteDetails(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFileCnt As Integer, ByRef r_vQuoteArray As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.GetQuoteDetails(v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), v_bUnderwriting:=True, r_vQuoteArray:=r_vQuoteArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '**** Added By: AAB  -  Added On:  19-Sep-2002 10:53 ****
    Public Function GetQuoteRisks(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFileCnt As Integer, ByRef r_vQuoteArray As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.GetQuoteRisks(v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), r_vQuoteArray:=r_vQuoteArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteRisks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '**** Added By: AAB  -  Added On:  19-Sep-2002 10:53 ****
    Public Function GetRatingDetails(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_vRatingSections As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.GetRatingDetails(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), v_lRiskCnt:=gPMFunctions.ToSafeInteger(v_lRiskCnt), r_vRatingSections:=r_vRatingSections)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRatingDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRatingDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' UpdateParty method.
    ''' </summary>
    ''' <param name="v_sGisDataModelCode"></param>
    ''' <param name="v_sGisBusinessTypeCode"></param>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="v_sForename"></param>
    ''' <param name="v_sSurname"></param>
    ''' <param name="v_sDateOfBirth"></param>
    ''' <param name="v_sEmailAddress"></param>
    ''' <param name="v_sCurrentRenewalDate"></param>
    ''' <param name="v_sAddress1"></param>
    ''' <param name="v_sAddress2"></param>
    ''' <param name="v_sAddress3"></param>
    ''' <param name="v_sAddress4"></param>
    ''' <param name="v_sPostcode"></param>
    ''' <param name="v_sTitle"></param>
    ''' <param name="v_sMaritalStatusCode"></param>
    ''' <param name="v_sGenderCode"></param>
    ''' <param name="v_sInitials"></param>
    ''' <param name="v_sTelephoneNumber"></param>
    ''' <param name="v_vAdditionalDataArray"></param>
    ''' <param name="sAddress5"></param>
    ''' <param name="sAddress6"></param>
    ''' <param name="sAddress7"></param>
    ''' <param name="sAddress8"></param>
    ''' <param name="sAddress9"></param>
    ''' <param name="sAddress10"></param>
    ''' <returns></returns>
    Public Function UpdateParty(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String,
                               ByVal v_lPartyCnt As Object,
                               Optional ByVal v_sForename As String = "", Optional ByVal v_sSurname As String = "",
                               Optional ByVal v_sDateOfBirth As String = "",
                               Optional ByVal v_sEmailAddress As String = "",
                               Optional ByVal v_sCurrentRenewalDate As String = "",
                               Optional ByVal v_sAddress1 As String = "",
                               Optional ByVal v_sAddress2 As String = "",
                               Optional ByVal v_sAddress3 As String = "",
                               Optional ByVal v_sAddress4 As String = "",
                               Optional ByVal v_sPostcode As String = "", Optional ByVal v_sTitle As String = "",
                               Optional ByVal v_sMaritalStatusCode As String = "",
                               Optional ByVal v_sGenderCode As String = "", Optional ByVal v_sInitials As String = "",
                               Optional ByVal v_sTelephoneNumber As String = "",
                               Optional ByVal v_vAdditionalDataArray As Object = Nothing,
                               Optional ByVal sAddress5 As String = "",
                               Optional ByVal sAddress6 As String = "",
                               Optional ByVal sAddress7 As String = "",
                               Optional ByVal sAddress8 As String = "",
                               Optional ByVal sAddress9 As String = "",
                               Optional ByVal sAddress10 As String = "") As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try

            Dim oBOM As Object


            nResult = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If Not (oBOM Is Nothing) Then

                nResult = oBOM.UpdateParty(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode),
                                           v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_sForename:=gPMFunctions.ToSafeString(v_sForename), v_sSurname:=gPMFunctions.ToSafeString(v_sSurname), v_sDateOfBirth:=gPMFunctions.ToSafeString(v_sDateOfBirth),
                                           v_sEmailAddress:=gPMFunctions.ToSafeString(v_sEmailAddress), v_sCurrentRenewalDate:=gPMFunctions.ToSafeString(v_sCurrentRenewalDate),
                                           v_sAddress1:=gPMFunctions.ToSafeString(v_sAddress1), v_sAddress2:=gPMFunctions.ToSafeString(v_sAddress2), v_sAddress3:=gPMFunctions.ToSafeString(v_sAddress3), v_sAddress4:=gPMFunctions.ToSafeString(v_sAddress4),
                                           v_sPostcode:=gPMFunctions.ToSafeString(v_sPostcode), v_sTitle:=gPMFunctions.ToSafeString(v_sTitle), v_sMaritalStatusCode:=gPMFunctions.ToSafeString(v_sMaritalStatusCode),
                                           v_sGenderCode:=gPMFunctions.ToSafeString(v_sGenderCode), v_sInitials:=gPMFunctions.ToSafeString(v_sInitials), v_sTelephoneNumber:=gPMFunctions.ToSafeString(v_sTelephoneNumber),
                                           v_vAdditionalDataArray:=v_vAdditionalDataArray,
                                            sAddress5:=gPMFunctions.ToSafeString(sAddress5), sAddress6:=gPMFunctions.ToSafeString(sAddress6),
                                           sAddress7:=gPMFunctions.ToSafeString(sAddress7), sAddress8:=gPMFunctions.ToSafeString(sAddress8),
                                           sAddress9:=gPMFunctions.ToSafeString(sAddress9), sAddress10:=gPMFunctions.ToSafeString(sAddress10))

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="UpdateParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateParty",
                               excep:=excep)

            Return nResult

        End Try
    End Function
    '****   END CHANGES - Changed By: AAB  - Changed On: 09-Oct-2002 10:14   ****
    Public Function ProcessAccounts(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String) As Integer
        Return ProcessAccounts(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sTransactionType:=v_sTransactionType, v_bMTAInstallments:=False, v_sCancelRefundAmt:="0", v_bRenewalInstallments:=False, v_iDebitAgainst:=0, v_lPaymentAccountId:=0, r_vTransactionArray:=Nothing)
    End Function
    Public Function ProcessAccounts(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, ByVal v_bMTAInstallments As Boolean, ByVal v_sCancelRefundAmt As String, ByVal v_bRenewalInstallments As Boolean, ByRef r_vTransactionArray As Object) As Integer
        Return ProcessAccounts(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sTransactionType:=v_sTransactionType, v_bMTAInstallments:=v_bMTAInstallments, v_sCancelRefundAmt:=v_sCancelRefundAmt, v_bRenewalInstallments:=v_bRenewalInstallments, v_iDebitAgainst:=0, v_lPaymentAccountId:=0, r_vTransactionArray:=r_vTransactionArray)
    End Function
    Public Function ProcessAccounts(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, ByVal v_bMTAInstallments As Boolean, ByVal v_sCancelRefundAmt As String, ByVal v_bRenewalInstallments As Boolean, ByVal v_iDebitAgainst As Integer, ByVal v_lPaymentAccountId As Integer, ByRef r_vTransactionArray As Object) As Integer

        '******************************************************************************
        '        Function Name:  ProcessAccounts
        '******************************************************************************
        '           Created By:  Ahmed "Jay" Bishtawi
        '           Created On:  23-Jan-2003
        '******************************************************************************
        '       Parameters Are:
        '                        (In) - v_sGisDataModelCode    - String   -
        '                        (In) - v_sGisBusinessTypeCode - String   -
        '                        (In) - v_lInsuranceFileCnt    - Long     -
        '                        (In) - v_sTransactionType     - String   -
        '                        (In) - v_bMTAInstallments     - Boolean  -
        '                        (In) - v_sCancelRefundAmt     - String   -
        '                        (In) - v_bRenewalInstallments - Boolean  -
        '
        ' Return Value Type Is:  Long -
        '******************************************************************************
        ' Function Description:  This function pass information to the BOM to create
        '                        the transaction accounts.
        '******************************************************************************
        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.ProcessAccounts(v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), v_sTransactionType:=gPMFunctions.ToSafeString(v_sTransactionType), v_bMTAInstallments:=gPMFunctions.ToSafeBoolean(v_bMTAInstallments),
                                               v_sCancelRefundAmt:=gPMFunctions.ToSafeString(v_sCancelRefundAmt), v_bRenewalInstallments:=gPMFunctions.ToSafeBoolean(v_bRenewalInstallments), v_iDebitAgainst:=gPMFunctions.ToSafeInteger(v_iDebitAgainst),
                                               v_lPaymentAccountId:=gPMFunctions.ToSafeInteger(v_lPaymentAccountId), r_vTransactionArray:=r_vTransactionArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessAccounts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccounts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSchemeList
    '
    ' Description: Returns a list of Schemes given a search Criteria
    '
    ' TJB 21/01/2003
    ' ***************************************************************** '
    Public Function GetSchemeList(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Use the CreateBOM function to create the BOM (if required)
            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.GetSchemeList(r_vResultArray:=r_vResultArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSchemeList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemeList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    '**** Added By: AAB  -  Added On:  19-Sep-2002 10:53 ****
    Public Function GetRiskByProduct(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lProductID As Integer, ByRef r_vResultArray As Object) As Integer
        Return GetRiskByProduct(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lProductID:=v_lProductID, r_vResultArray:=r_vResultArray, v_vAdditionalDataArray:=Nothing)
    End Function

    Public Function GetRiskByProduct(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lProductID As Integer, ByRef r_vResultArray As Object, ByVal v_vAdditionalDataArray As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBOM As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBOM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBOM Is Nothing) Then

                lReturn = oBOM.GetRiskByProduct(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lProductID:=gPMFunctions.ToSafeInteger(v_lProductID), r_vResultArray:=r_vResultArray, v_vAdditionalDataArray:=v_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.QuotePolicy class

                oBOM.Dispose()
                oBOM = Nothing

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskByProduct Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskByProduct", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
