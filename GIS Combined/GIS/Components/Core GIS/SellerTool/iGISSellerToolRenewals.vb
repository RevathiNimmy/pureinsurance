Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Renewals_NET.Renewals")> _
Public NotInheritable Class Renewals
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Renewals
    '
    ' Date: 19/10/2001
    '
    ' Description: Implements the Renewals calls to bGIS
    '
    ' Edit History:
    '   DD19102001 - Created
    '   DD09112001 - Recoded for new Generic calls to bGIS
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Renewals"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' GIS Data Set
    Private m_oDataSet As cGISDataSetControl.Application

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

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

    ' PUBLIC Property Procedures (End)
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
                If m_oDataSet IsNot Nothing Then
                    m_oDataSet.Dispose()
                    m_oDataSet = Nothing
                End If
            End If
        End If
		Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    '
    ' Name: RenGetPassword
    '
    ' Description:  Returns the decrypted Policy Password
    '
    ' History: 27/11/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function RenGetPassword(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_lInsuranceFileCnt As Object, ByRef r_sUnencryptedPassword As Object) As Integer


        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenGetPassword", v_sDataModelCode, v_sBusinessTypeCode, v_lInsuranceFileCnt, r_sUnencryptedPassword)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenGetPassword Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenGetPassword", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: UpdateRenewalControl
    '
    ' Description: Sets any field in the renewal control
    '
    ' History: 1/11/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '          12/11/2001 Thinh Nguyen - change option param to variant
    '          16/11/2001 DD - Added OfferAlt parameter
    '
    ' ***************************************************************** '
    Public Function UpdateRenewalControl(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, Optional ByVal v_lProductID As Object = Nothing, Optional ByVal v_lRenewalInsuranceFileCnt As Object = Nothing, Optional ByVal v_sRenewalStatusCode As Object = Nothing, Optional ByVal v_lSuspensionLevel As Object = Nothing, Optional ByVal v_lRenewalEdiAuditId As Object = Nothing, Optional ByVal v_lRenewalSchemeID As Object = Nothing, Optional ByVal v_lSchemeID As Object = Nothing, Optional ByVal v_dtRenewalDate As Object = Nothing, Optional ByVal v_lDataModelID As Object = Nothing, Optional ByVal v_lOldInsuranceFileCnt As Object = Nothing, Optional ByVal v_lOfferAlt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "UpdateRenewalControl", v_lInsuranceFolderCnt, v_lProductID, v_lRenewalInsuranceFileCnt, v_sRenewalStatusCode, v_lSuspensionLevel, v_lRenewalEdiAuditId, v_lRenewalSchemeID, v_lSchemeID, v_dtRenewalDate, v_lDataModelID, v_lOldInsuranceFileCnt, v_lOfferAlt)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRenewalControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ListRenewals
    '
    ' Description: Returns a list of Renewals using the criteria
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '          DD16112001 - Added OfferAlt parameter
    '
    ' ***************************************************************** '
    Public Function ListRenewals(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByRef r_vResultArray As Object, Optional ByVal v_sRenewalStatus As String = "", Optional ByVal v_dtDueDateStart As Date = #12/29/1899#, Optional ByVal v_dtDueDateLimit As Date = #12/29/1899#, Optional ByVal v_sClientCode As String = "", Optional ByVal v_sPolicyNo As String = "", Optional ByVal v_lBusinessTypeID As Integer = 0, Optional ByVal v_lSchemeID As Integer = 0, Optional ByVal v_lInsurerId As Integer = 0, Optional ByVal v_lSuspensionLevel As Integer = 0, Optional ByVal v_lOfferAlt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "ListRenewals", r_vResultArray, v_sDataModelCode, v_sRenewalStatus, v_dtDueDateStart, v_dtDueDateLimit, v_sClientCode, v_sPolicyNo, v_lBusinessTypeID, v_lSchemeID, v_lInsurerId, v_lSuspensionLevel, v_lOfferAlt)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListRenewals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListRenewals", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetPolicyRenewalVersion
    '
    ' Description:  Gets the latest version of the Policy record to be
    '               used for the Renewal process.
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function GetPolicyRenewalVersion(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "GetPolicyRenewalVersion", v_sDataModelCode, v_sBusinessTypeCode, v_lInsuranceFolderCnt, r_vResultArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyRenewalVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyRenewalVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: ConfirmLapse
    '
    ' Description: Confirms the Lapse of a Policy
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function ConfirmLapse(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "ConfirmLapse", v_lInsuranceFolderCnt, v_lInsuranceFileCnt, v_lSchemeID, v_lPartyCnt, v_sDataModelCode, v_sBusinessTypeCode)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConfirmLapse Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfirmLapse", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: ConfirmRenewal
    '
    ' Description: Confirms the Renewal of a Policy
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function ConfirmRenewal(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer, Optional ByVal v_bIsWhatIfQ As Boolean = False, Optional ByVal v_bAutoConfirm As Boolean = False) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "ConfirmRenewal", v_lInsuranceFolderCnt, v_lInsuranceFileCnt, v_lSchemeID, v_lPartyCnt, v_sDataModelCode, v_sBusinessTypeCode, v_bIsWhatIfQ, v_bAutoConfirm)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConfirmRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfirmRenewal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenQuotationBrokerLead
    '
    ' Description: Retrieves a renewal quotation for the Policy
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function RenQuotationBrokerLead(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lDataModelID As Integer, ByVal v_lSchemeID As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalSchemeID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenQuotationBrokerLead", v_lInsuranceFolderCnt, v_lPartyCnt, v_dtRenewalDate, v_lRiskCodeID, v_lDataModelID, v_sDataModelCode, v_lSchemeID, v_lProductID, r_lRenewalInsuranceFileCnt, v_lRenewalSchemeID, v_sBusinessTypeCode)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenQuotationBrokerLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationBrokerLead", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenMultipleQuotationBrokerLead
    '
    ' Description:  Produces the quotations for an array of
    '               selected Insurance folders.
    '
    ' History: 19/11/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function RenMultipleQuotationBrokerLead(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByRef v_vSelectedArray As Object, ByRef r_vFailedArray As Object, ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Try

            ' Execute the call at the GIS end

            Return GISCall("Renewals", "RenMultipleQuotationBrokerLead", v_sDataModelCode, v_sBusinessTypeCode, v_vSelectedArray, r_vFailedArray, r_vResultArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMultipleQuotationBrokerLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMultipleQuotationBrokerLead", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: RenCompAlternateInsurer
    '
    ' Description: Renews a Policy with an alternate insurer
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function RenCompAlternateInsurer(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lSchemeID As Integer, ByVal v_lRenewalSchemeID As Integer, ByVal v_lProductID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lDataModelID As Integer, ByVal v_lGisBusinessTypeId As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByRef r_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenCompAlternateInsurer", v_lInsuranceFolderCnt, v_lRenewalEdiAuditId, v_lSchemeID, v_lRenewalSchemeID, r_lRenewalInsuranceFileCnt, v_lProductID, v_dtRenewalDate, v_lPartyCnt, v_lRiskCodeID, v_lDataModelID, v_sDataModelCode, v_lGisBusinessTypeId, r_lNewInsuranceFileCnt)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompAlternateInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompAlternateInsurer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenCompHoldingInsurer
    '
    ' Description: Completes the Renewal by sending the EDI
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function RenCompHoldingInsurer(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lSchemeID As Integer, ByVal v_lRenewalSchemeID As Integer, ByVal v_lProductID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lDataModelID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenCompHoldingInsurer", v_lInsuranceFolderCnt, v_lRenewalEdiAuditId, v_lSchemeID, v_lRenewalSchemeID, r_lRenewalInsuranceFileCnt, v_lProductID, v_dtRenewalDate, v_lPartyCnt, v_lRiskCodeID, v_lDataModelID, v_sDataModelCode, v_sBusinessTypeCode)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompHoldingInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompHoldingInsurer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenCompLapse
    '
    ' Description: Confirms a Lapse by sending the EDI
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function RenCompLapse(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lSchemeID As Integer, ByVal v_lRenewalSchemeID As Integer, ByVal v_lProductID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lDataModelID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, Optional ByVal v_sRenewalStatusCode As String = PMRenewalStatusTypePolicyLapseConfirmed, Optional ByVal v_lOldInsuranceFileCnt As Integer = 0) As Integer


        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenCompLapse", v_lInsuranceFolderCnt, v_lRenewalEdiAuditId, v_lSchemeID, v_lRenewalSchemeID, r_lRenewalInsuranceFileCnt, v_lProductID, v_dtRenewalDate, v_lPartyCnt, v_lRiskCodeID, v_lDataModelID, v_sDataModelCode, v_sBusinessTypeCode, v_sRenewalStatusCode, v_lOldInsuranceFileCnt)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompLapse Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompLapse", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: ConfirmRenewalBrokerLed
    '
    ' Description:  Transacts the Renewal in advance of completion.
    '
    ' History: DD30112001 - Created from ConfirmRenewal.
    ' ***************************************************************** '
    Public Function ConfirmRenewalBrokerLed(ByVal v_sGisDataModelCode As String, ByVal v_sGISBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer, Optional ByVal v_bIsWhatIfQ As Boolean = False, Optional ByVal v_bAutoConfirm As Boolean = False, Optional ByVal v_lPolicyBinderID As Object = Nothing, Optional ByVal v_lPolicyID As Object = Nothing, Optional ByVal v_lOldInsurerNo As Object = Nothing, Optional ByVal v_lNewInsurerNo As Object = Nothing, Optional ByVal v_lSchemeNo As Object = Nothing, Optional ByVal v_sCoverCode As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "ConfirmRenewalBrokerLed", v_lInsuranceFolderCnt, v_lInsuranceFileCnt, v_lSchemeID, v_lPartyCnt, v_sGisDataModelCode, v_sGISBusinessTypeCode, v_bIsWhatIfQ, v_bAutoConfirm, v_lPolicyBinderID, v_lPolicyID, v_lOldInsurerNo, v_lNewInsurerNo, v_lSchemeNo, v_sCoverCode)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConfirmRenewalBrokerLed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfirmRenewalBrokerLed", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenConfDocsHoldingInsurer
    '
    ' Description: Produces the confirmation documents
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function RenConfDocsHoldingInsurer(ByVal v_sBusinessTypeCode As String, ByVal v_sDataModelCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lSchemeID As Integer, ByVal v_lRenewalSchemeID As Integer, ByVal v_lProductID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lDataModelID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenConfDocsHoldingInsurer", v_lInsuranceFolderCnt, v_lPartyCnt, v_dtRenewalDate, v_lRiskCodeID, v_lDataModelID, v_sDataModelCode, v_lSchemeID, v_lProductID, r_lRenewalInsuranceFileCnt, v_lRenewalSchemeID, v_lRenewalEdiAuditId, v_sBusinessTypeCode)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenConfDocsHoldingInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenConfDocsHoldingInsurer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenMultipleInvitationBrokerLed
    '
    ' Description:  Produces the invitation documents for an array of
    '               selected Insurance folders.
    '
    ' History: 07/11/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function RenMultipleInvitationBrokerLed(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByRef v_vSelectedArray As Object, ByRef r_vFailedArray As Object) As Integer

        Dim result As Integer = 0
        Try

            ' Execute the call at the GIS end

            Return GISCall("Renewals", "RenMultipleInvitationBrokerLed", v_sDataModelCode, v_sBusinessTypeCode, v_vSelectedArray, r_vFailedArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMultipleInvitationBrokerLed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMultipleInvitationBrokerLed", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenInvitePreferredQuotes
    '
    ' Description:
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function RenInvitePreferredQuotes(ByVal v_sBusinessTypeCode As String, ByVal v_sDataModelCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lSchemeID As Integer, ByVal v_lRenewalSchemeID As Integer, ByVal v_lProductID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lDataModelID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenInvitePreferredQuotes", v_lInsuranceFolderCnt, v_lPartyCnt, v_dtRenewalDate, v_lRiskCodeID, v_lDataModelID, v_sDataModelCode, v_lSchemeID, v_lProductID, r_lRenewalInsuranceFileCnt, v_lRenewalSchemeID, v_sBusinessTypeCode)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitePreferredQuotes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitePreferredQuotes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenReminder
    '
    ' Description: Produce the reminder documents for a Policy
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function RenReminder(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenReminder", v_lInsuranceFolderCnt, v_lPartyCnt, v_lRenewalInsuranceFileCnt, v_sDataModelCode)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReminder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenReminder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenReprintConfirm
    '
    ' Description:
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '          DD 05/03/2002: Changed r_lRenewalInsuranceFileCnt to ByVal
    '
    ' ***************************************************************** '
    Public Function RenReprintConfirm(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_lDataModelID As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal r_lRenewalInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenReprintConfirm", v_lDataModelID, v_sDataModelCode, r_lRenewalInsuranceFileCnt, v_lRenewalEdiAuditId, v_sBusinessTypeCode)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReprintConfirm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenReprintConfirm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenReprintInvitationBrokerLead
    '
    ' Description:
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function RenReprintInvitationBrokerLead(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByVal v_vSelectedArray As Object, ByRef r_vFailedArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenReprintInvitationBrokerLead", v_sBusinessTypeCode, v_sDataModelCode, v_vSelectedArray, r_vFailedArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReprintInvitationBrokerLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenReprintInvitationBrokerLead", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenResendEDI
    '
    ' Description: Resend the EDI message for the Renewal
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function RenResendEDI(ByVal v_sBusinessTypeCode As String, ByVal v_sDataModelCode As String, ByVal v_lDataModelID As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenResendEDI", v_lDataModelID, v_sDataModelCode, r_lRenewalInsuranceFileCnt, v_lRenewalEdiAuditId, v_sBusinessTypeCode)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenResendEDI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenResendEDI", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenSelection
    '
    ' Description:
    '
    ' History: 19/10/2001 DD - Created.
    '          DD09112001 - Recoded for new Generic calls to bGIS
    '
    ' ***************************************************************** '
    Public Function RenSelection(ByVal v_sBusinessTypeCode As String, ByVal v_sDataModelCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByRef r_sDataModelCode As String) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenSelection", v_lInsuranceFolderCnt, v_lPartyCnt, v_dtRenewalDate, v_lRiskCodeID, r_sDataModelCode, v_sBusinessTypeCode)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenSelection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenSelection", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateLapseReason
    '
    ' Description:  Saves the lapse reason and comment in the Insurance File
    '
    ' History: 16/11/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateLapseReason(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lLapseReasonID As Integer, ByVal v_sLapseComment As String) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "UpdateLapseReason", v_lInsuranceFolderCnt, v_lLapseReasonID, v_sLapseComment)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLapseReason Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLapseReason", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenGetInsurerQuoteOptions
    '
    ' Description:  Takes a comma list of schemes and returns an array with
    '               the Web grid layout design. If more than one scheme is
    '               passed then the grid determines whether columns are
    '               shown or hidden.
    '
    ' History: 29/11/2001 DD - Created.
    '           DD, 14/01/2002: Added Cover Code parameter
    '
    ' ***************************************************************** '
    Public Function RenGetInsurerQuoteOptions(ByVal v_vSchemes As Object, ByVal v_vCoverCode As Object, ByRef r_vGridLayout As Object) As Integer


        Dim result As Integer = 0
        Return GISCall("Renewals", "RenGetInsurerQuoteOptions", v_vSchemes, v_vCoverCode, r_vGridLayout)



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenGetInsurerQuoteOptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenGetInsurerQuoteOptions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: RenCustLogin
    '
    ' Description:  Validates the Renewal Customer Login
    '               Returns the Insurance Folder/File Record ids
    '               and the menu location or blank (go directly to Quote).
    '
    ' History: 30/11/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function RenCustLogin(ByVal v_sGisDataModelCode As String, ByVal v_sPolicyNo As String, ByVal v_sPassword As String, ByRef r_lInsuranceFolderCnt As Object, ByRef r_lInsuranceFileCnt As Object, ByRef r_sMenuURL As Object) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenCustLogin", v_sGisDataModelCode, v_sPolicyNo, v_sPassword, r_lInsuranceFolderCnt, r_lInsuranceFileCnt, r_sMenuURL)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCustLogin Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCustLogin", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: SelectRenewalTaskLog
    '
    ' Description:  Returns the contents of the Renewal Task Log
    '
    ' History:  06/12/2001 DD - Created.
    '           DD, 07/12/2001: Added optional filters.
    '
    ' ***************************************************************** '
    Public Function SelectRenewalTaskLog(ByRef r_vResultArray As Object, Optional ByVal v_vStartDate As Object = Nothing, Optional ByVal v_vEndDate As Object = Nothing, Optional ByVal v_vStatus As Object = Nothing, Optional ByVal v_vPolicyNo As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "SelectRenewalTaskLog", r_vResultArray, v_vStartDate, v_vEndDate, v_vStatus, v_vPolicyNo)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectRenewalTaskLog Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectRenewalTaskLog", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenIsActiveRenewal
    '
    ' Description:  Returns the Insurance Folder and Status if the File
    '               or Reference is currently an active Renewal.
    '
    ' History: 30/01/2002 DD - Created.
    '
    ' ***************************************************************** '
    Public Function RenIsActiveRenewal(ByVal v_lInsuranceFileCnt As Object, ByVal v_sInsuranceRef As Object, ByRef r_lInsuranceFolderCnt As Object, ByRef r_sStatus As Object, ByRef r_sNewPolicyNo As Object, ByRef r_dRenewalDate As Object, ByRef r_sInsurer As Object, ByRef r_sScheme As Object) As Integer

        Dim result As Integer = 0
        Try


            Return GISCall("Renewals", "RenIsActiveRenewal", v_lInsuranceFileCnt, v_sInsuranceRef, r_lInsuranceFolderCnt, r_sStatus, r_sNewPolicyNo, r_dRenewalDate, r_sInsurer, r_sScheme)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenIsActiveRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenIsActiveRenewal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
