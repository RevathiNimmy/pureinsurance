Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.Text
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Renewals_NET.Renewals")>
Public NotInheritable Class Renewals
    Implements IDisposable
    '
    ' History :
    '
    ' 11/11/2004 CJB PN16651 Accept renewal date as a param for use with index linking
    '                        rather than current date (in RenSelectionAfter). Also change
    '                        IndexLink to accept the date rather than just use current date
    '                        and same for GetIndexLinkDetails.
    ' 02/12/2004 CJB PN17260 Added code in RenQuotationBrokerLeadBefore to clear all previous
    '                        quote output (QEM only clears it with same scheme name)
    ' 09/12/2004 CJB PN17352 Ensure that docs are archived when flagged to do so
    '
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


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Renewal"

    ' PRIVATE Data Members (Begin)
    Private Const ACOptionInvite As Integer = 37
    Private Const ACOptionReminder As Integer = 36
    Private Const ACOptionSchedule As Integer = 34
    Private Const ACOptionLapse As Integer = 39

    'sj 11/09/2001 - start
    Private Const ACOptionPreferredQuotes As Integer = 4009

    'sj 11/09/2001 - end
    Private Const ACNormalMode As Integer = 0
    Private Const ACMergeMode As Integer = 1
    Private Const ACPrintMode As Integer = 2
    Private Const ACPrintSilentMode As Integer = 3

    'AK 300701 - Constants for Claims Data
    Private Const ACClaimID As Integer = 0
    Private Const ACClaimDate As Integer = 1
    Private Const ACPrimaryCause As Integer = 2
    'For Motor
    Private Const ACAtFault As Integer = 3
    Private Const ACBodilyInjury As Integer = 4

    'AK 090801
    Private Const ACDriver As Integer = 5

    'For Household
    Private Const ACClaimLikely As Integer = 3
    Private Const ACClaimDescription As Integer = 4

    'AK 060801 - constants for GIS-Claim Link
    Private Const ACGISClaimID As Integer = 1
    Private Const ACGISDriverID As Integer = 2
    Private Const ACGISIncidentID As Integer = 3

    Private Const ACGetRenewalPremiumStored As Boolean = True
    Private Const ACGetRenewalPremiumName As String = "GetRenewalPremium"
    Private Const ACGetRenewalPremiumSQL As String = "{call sp_SirRen_Renewal_Premium_Sel(?,?,?)}"

    Private Const ACRenewalManagerSelectSQL As String = "{call spu_I4M_Ren_Manager_sel (?,?,?,?,?,?,?,?,?,?)}"
    Private Const ACRenewalManagerSelectStored As Boolean = True
    Private Const ACRenewalManagerSelectName As String = "RenewalManagerSelect"

    'TF291101
    Private Const ACUpdateRenewalPremiumStored As Boolean = True
    Private Const ACUpdateRenewalPremiumName As String = "UpdateRenewalPremium"
    Private Const ACUpdateRenewalPremiumSQL As String = "{call spu_I4M_Renewal_Premium_Upd(?)}"

    'SJ 29/03/2004 - start
    Private Const ACGetIndexLinkDetailsStored As Boolean = True
    Private Const ACGetIndexLinkDetailsName As String = "GetIndexLinkDetails"
    Private Const ACGetIndexLinkDetailsSQL As String = "{call spu_gis_property_index_link_sel(?,?)}"
    'SJ 29/03/2004 - end

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    ' Data Set Definition
    Private m_oDataSet As cGISDataSetControl.Application

    Private m_lReturn As Integer

    Private m_sBusinessTypeCode As String = ""

    'AK 170402
    Private m_sDataModelCode As String = ""
    Private m_sPolicyObject As String = ""
    Private m_sOutPutObject As String = ""

    'AK 260402
    Private m_lSchemeGroupId As Integer

    'TF190303 - Merged AMJ 231002
    Private m_lInsuranceFileCnt As Integer
    'TF190303 - AMJ 040203
    Private m_oIPT As Object
    Private m_oInsuranceFile As bSIRInsuranceFile.Services
    'Premium Values
    Private m_cPremium As Decimal
    Private m_cOverridePremium As Decimal
    Private m_cIPT As Decimal
    Private m_cVAT As Decimal
    Private m_cIPTRate As Decimal


    'Public Property - Start
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property


    Public Property GISBusinessTypeCode() As String
        Get
            Return m_sBusinessTypeCode
        End Get
        Set(ByVal Value As String)
            m_sBusinessTypeCode = Value
        End Set
    End Property

    Public ReadOnly Property GroupID() As Integer
        Get

            'TF190303 - Merged AMJ 231002
            If m_lSchemeGroupId = 0 And m_lInsuranceFileCnt > 0 Then
                GetGisSchemeGroupID(m_lInsuranceFileCnt)
            End If

            Return m_lSchemeGroupId

        End Get
    End Property

    Public WriteOnly Property GISDataModel() As String
        Set(ByVal Value As String)

            m_sDataModelCode = Value

        End Set
    End Property

    'TF190303 - Merged AMJ 231002
    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property


    'Public Property - End

    'Public Function - Start
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer






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


            ' Initialisation Code.

            'AK 170402
            m_sPolicyObject = m_sDataModelCode & "_POLICY"
            m_sOutPutObject = m_sDataModelCode & "_OUTPUT"

            ' Set Username and Password

            'Check database

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TF190303 - Merged AMJ040203
            m_oInsuranceFile = New bSIRInsuranceFile.Services
            m_lReturn = m_oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSIRInsuranceFile.Services.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="Initialise")
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

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
                If m_oInsuranceFile IsNot Nothing Then
                    m_oInsuranceFile.Dispose()
                    m_oInsuranceFile = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: PreRenSelection
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function PreRenSelection(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sGisDataModelCode As String, Optional ByRef r_vSuspensionLevel As Byte = 0) As Integer

        Dim result As Integer = 0
        Dim oSiriusLinkClaim As Object
        Dim vKeyArray As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' AK 011101 - In order to automatically update NCD years for policies in Renewal cycle


            m_lReturn = CreateSiriusLinkClaims(oSiriusLinkClaim)


            m_lReturn = oSiriusLinkClaim.GetClaimsForPolicyYear(v_sDataModel:=ToSafeString(v_sGisDataModelCode), v_lInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), r_vKey:=vKeyArray)


            oSiriusLinkClaim.Dispose()
            oSiriusLinkClaim = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=" PreRenSelection Failed to get Claims Data", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:=" PreRenSelection", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Else
                'AK 021101

                If Informations.IsArray(vKeyArray) Then
                    'set the suspension flag
                    r_vSuspensionLevel = 1
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PreRenSelection Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="PreRenSelection", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenSelectionBefore
    '
    ' Description:
    '
    ' History: 18/05/2001 CTAF - Created
    '
    ' ***************************************************************** '
    Public Function RenSelectionBefore(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_sGisDataModelCode As String, ByVal v_lOldInsuranceFileCnt As Integer, Optional ByRef r_vSuspensionLevel As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AK 170402
            m_sDataModelCode = v_sGisDataModelCode

            Dim vOIKeyArray As Object
            'CJR 16/9/02 check policy exists before attempting to update NCD

            m_lReturn = r_oDataset.GetAllOIKey("Policy", vOIKeyArray)

            If Informations.IsArray(vOIKeyArray) Then
                'AK 230402 - update NCD information for the Renewal Risk
                m_lReturn = UpdateRiskNCD(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_oDataset:=r_oDataset)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' SET 19/12/2002 - not needed cos bGIS does this as part of copy risk
            '    sSQL = "INSERT INTO insurance_file_risk_link (" & vbCrLf & _
            ''           "insurance_file_cnt," & vbCrLf & _
            ''           "risk_cnt," & vbCrLf & _
            ''           "status_flag, " & vbCrLf & _
            ''           "original_risk_cnt)" & vbCrLf & _
            ''           "SELECT " & v_lInsuranceFileCnt & "," & vbCrLf & _
            ''           "risk_cnt," & vbCrLf & _
            ''           "'C'," & vbCrLf & _
            ''           "original_risk_cnt" & vbCrLf & _
            ''           "FROM insurance_file_risk_link" & vbCrLf & _
            ''           "WHERE insurance_file_cnt = " & v_lOldInsuranceFileCnt & vbCrLf
            '
            '    m_oDatabase.Parameters.Clear
            '
            '    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, _
            ''                                      sSQLName:="CopyInsuranceFileRiskLink", _
            ''                                      bStoredProcedure:=False)
            '
            '    ' Check for errors
            '    If m_lReturn <> PMTrue Then
            '        RenSelectionBefore = m_lReturn
            '        Exit Function
            '    End If
            ' SET 19/12/2002 -

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenSelectionBefore Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenSelectionBefore", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenSelectionAfter
    '
    ' Description:
    '
    ' History: 18/05/2001 CTAF - Created.
    '          23/10/2001 Thinh.Nguyen - set suspension level flag to 1
    '                     to give user a chance to update risk details
    '                     before the rest of the renewal process is run
    ' ***************************************************************** '
    Public Function RenSelectionAfter(ByVal v_lOldGISPolicyLinkID As Integer, ByVal v_lNewGISPolicyLinkID As Integer, ByVal v_dtRenewalDate As Date, ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_lGISSchemeID As Integer) As Integer 'PN16651

        Dim result As Integer = 0
        Dim sOutputTable As String = ""
        Dim vOIKeyArray As Object
        Dim sOutput, sSchemeName As String
        'AK - 170402
        Dim sObjPolicy As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'SJ 30/03/2004 - start (CJB171104 Moved from botton so gets exectuted even when no quote o/p PN16777)
            m_lReturn = IndexLink(v_dtRenewalDate:=v_dtRenewalDate, r_oDataset:=r_oDataset) 'PN16651
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenSelectionAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass)
                Return result
            End If
            'SJ 30/03/2004 - end

            'Delete existing output
            '    sObjPolicy = m_sDataModelCode & "_POLICY"
            '    sOutputTable = m_sDataModelCode & "_OUTPUT"  '"CNC_OUTPUT"

            m_lReturn = r_oDataset.GetAllOIKey(v_sObjectName:=m_sOutPutObject, r_vOIKeyArray:=vOIKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process r_oDataset.GetAllOIKey(" & m_sOutPutObject & ").", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenSelectionAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Not Informations.IsArray(vOIKeyArray) Then
                Return result
            End If


            For lLoop As Integer = vOIKeyArray.GetLowerBound(0) To vOIKeyArray.GetUpperBound(0)

                ' Get the index

                sOutput = CStr(vOIKeyArray(lLoop))

                ' Get the scheme
                m_lReturn = r_oDataset.GetPropertyValue(v_sObjectName:=m_sOutPutObject, v_sPropertyName:="scheme_desc", v_sOIKey:=sOutput, r_vPropertyValue:=sSchemeName, r_bIsAssumedInfo:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process r_oDataset.GetPropertyValue(scheme_desc).", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenSelectionAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                With m_oDatabase
                    .Parameters.Clear()

                    m_lReturn = .Parameters.Add(sName:="POLICYLINKID", vValue:=CStr(r_oDataset.PolicyLinkID()), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    m_lReturn = .Parameters.Add(sName:="DATAMODELCODE", vValue:=m_sDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    m_lReturn = .Parameters.Add(sName:="SCHEMENAME", vValue:=sSchemeName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    m_lReturn = .SQLAction(sSQL:="{call spu_GIS_clear_quote_output (?,?,?)}", sSQLName:=" ", bStoredProcedure:=True)
                End With
            Next lLoop

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenSelectionAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenSelectionAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenMtaAtRenewalQuote
    '
    ' Description:
    '
    ' History: 11/06/2001 sj - Created.
    '
    ' ***************************************************************** '
    Public Function RenMtaAtRenewalQuote(ByVal v_lRenewalEdiAuditId As Integer, ByVal v_cOldAnnualPremium As Decimal, ByVal v_cNewAnnualPremium As Decimal, ByRef r_cNewRenewalPremiumIncIpt As Decimal, ByRef r_cOldRenewalPremiumIncIpt As Decimal) As Integer

        Dim result As Integer = 0
        Dim oRenEdiAudit As Object = Nothing
        Dim dPremium, dPremiumIncIpt, dIPT As Object
        Dim lRnlRecNo As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CreateBusinessObject(sClassName:="bRenEdiAudit.Business", r_oObject:=oRenEdiAudit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bRenEdiAudit.Business", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenMtaAtRenewalQuote")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Laod the motor details for this audit id

            m_lReturn = oRenEdiAudit.GetRenewalEdiMotorSel(v_lRenewalEdiAuditId:=ToSafeInteger(v_lRenewalEdiAuditId))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oRenEdiAudit.GetRenewalEdiMotorSel Failed ", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenMtaAtRenewalQuote")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Return motor details

            m_lReturn = oRenEdiAudit.GetRenewalEdiMotorDetails(r_dPremium:=dPremium, r_dPremiumIncIpt:=dPremiumIncIpt, r_dIPT:=dIPT, r_lRnlRecNo:=lRnlRecNo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oRenEdiAudit.GetRenewalEdiMotorDetails Failed ", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenMtaAtRenewalQuote")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            oRenEdiAudit.Dispose()

            oRenEdiAudit = Nothing

            'Calculate new renewal premium using XYZ rule
            r_cNewRenewalPremiumIncIpt = dPremiumIncIpt * (v_cNewAnnualPremium / v_cOldAnnualPremium)

            r_cOldRenewalPremiumIncIpt = dPremiumIncIpt

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMtaAtRenewalQuote Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenMtaAtRenewalQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenMtaAtRenewalTransact
    '
    ' Description:
    '
    ' History: 11/06/2001 sj - Created.
    '
    ' ***************************************************************** '
    Public Function RenMtaAtRenewalTransact(ByVal v_lRenewalEdiAuditId As Integer, ByVal v_cOldAnnualPremium As Decimal, ByVal v_cNewAnnualPremium As Decimal, ByRef r_lRnlRecNo As Integer, ByRef r_cNewPremiumIncIpt As Decimal, ByRef r_cNewPremium As Decimal, ByRef r_cNewIptAmount As Decimal) As Integer

        Dim result As Integer = 0
        Dim oRenEdiAudit As Object
        Dim dPremium, dPremiumIncIpt, dIPT As Object
        Dim cIptRate As Double
        Dim lIptRate, lPremium As Integer
        Dim lRnlRecNo As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CreateBusinessObject(sClassName:="bRenEdiAudit.Business", r_oObject:=oRenEdiAudit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise oRenEdiAudit", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenMtaAtRenewalTransact")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Load the motor details for this audit id

            m_lReturn = oRenEdiAudit.GetRenewalEdiMotorSel(v_lRenewalEdiAuditId:=ToSafeInteger(v_lRenewalEdiAuditId))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oRenEdiAudit.GetRenewalEdiMotorSel Failed ", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenMtaAtRenewalTransact")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Return motor details

            m_lReturn = oRenEdiAudit.GetRenewalEdiMotorDetails(r_dPremium:=dPremium, r_dPremiumIncIpt:=dPremiumIncIpt, r_dIPT:=dIPT, r_lRnlRecNo:=lRnlRecNo)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oRenEdiAudit.GetRenewalEdiMotorDetails Failed ", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenMtaAtRenewalTransact")

                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                r_lRnlRecNo = lRnlRecNo

            End If

            'Recalculate the ipt rate to attemp to ensure that the net
            'premium is correct
            cIptRate = (dPremiumIncIpt * 100 / dPremium) - 100
            lIptRate = CInt(cIptRate * 100)
            cIptRate = lIptRate / 100

            dPremium = dPremiumIncIpt / ((100 + cIptRate) / 100)
            lPremium = CInt(dPremium * 100)
            dPremium = lPremium / 100

            dIPT = dPremiumIncIpt - dPremium

            'Calculate new renewal premium using XYZ rule
            r_cNewPremiumIncIpt = dPremiumIncIpt * (v_cNewAnnualPremium / v_cOldAnnualPremium)
            r_cNewPremium = dPremium * (v_cNewAnnualPremium / v_cOldAnnualPremium)
            r_cNewIptAmount = dIPT * (v_cNewAnnualPremium / v_cOldAnnualPremium)

            'Update the premiums on the renewal edi motor table

            m_lReturn = oRenEdiAudit.DirectUpdateRenEdiMotor(v_lRenewalEdiAuditId:=ToSafeInteger(v_lRenewalEdiAuditId), v_vPremium:=ToSafeDouble(r_cNewPremium), v_vPremiumIncIpt:=ToSafeDouble(r_cNewPremiumIncIpt), v_vIPTAmount:=ToSafeDouble(r_cNewIptAmount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oRenEdiAudit.GetRenewalEdiMotorDetails Failed ", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenMtaAtRenewalTransact")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Set the status to unprocessed on the renewal edi audit table

            m_lReturn = oRenEdiAudit.UpdateRenewalEdiStatus(v_lRenewalEdiAuditId:=ToSafeInteger(v_lRenewalEdiAuditId), v_vRenewalEdiStatus:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oRenEdiAudit.UpdateRenewalEdiStatus Failed ", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenMtaAtRenewalTransact")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            oRenEdiAudit.Dispose()

            oRenEdiAudit = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMtaAtRenewalTransact Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenMtaAtRenewalTransact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessDocument
    '
    ' Description:
    '
    ' History: 06/06/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ProcessDocument) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ProcessDocument(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_vProcessType As String) As Integer
    '
    'Dim result As Integer = 0
    'Dim oDoc As iPMBDocProductionWrapper.NavigatorV3
    'Dim vKeyArray As Object
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'oDoc = New iPMBDocProductionWrapper.NavigatorV3()
    '
    ' Initialise it
    'm_lReturn = CType(oDoc, SSP.S4I.Interfaces.ILocalInterface).Initialise()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ''ReDim vKeyArray(1, 3)
    '
    '
    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "party_cnt"
    '
    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lPartyCnt
    '
    '
    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "insurance_file_cnt"
    '
    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_lInsuranceFileCnt
    '
    '
    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "scheme_id"
    '
    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_lSchemeID
    '
    '
    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "process_type"
    '
    'vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_vProcessType
    '
    ' Pass in the keys
    'm_lReturn = oDoc.NavigatorV3_SetKeys(vKeyArray:=directcast(vKeyArray,object(,)))
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Start it
    'm_lReturn = oDoc.NavigatorV3_Start()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Terminate the object
    'm_lReturn = oDoc.Terminate()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'oDoc = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDocument Failed", vApp:=tosafestring(ACApp), vClass:=ACClass, vMethod:="ProcessDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    '

    '
    'Return result
    'End Try
    'End Function
    ' ***************************************************************** '
    ' Name: ProcessDocument2
    '
    ' Description:
    '
    ' History: 12/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessDocument2(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_vProcessType As Object, Optional ByVal v_lAgentCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        'Developer guide no. 108

        Dim oDocMgrWrapper As Object
        Dim oDocLink As bPMBDocLink.Business
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const LCACArrayDocumentTemplateId As Integer = 0
            Const LCACArrayDocumentTypeId As Integer = 1
            Const LCACArraySpoolDocument As Integer = 2
            Const LCACArrayArchiveDocument As Integer = 3 'PN17352

            ' Modes for printing
            'Const ACPrintMode As Integer = 2
            'Const ACPrintSilentMode As Integer = 3
            'Const ACSpoolDocMode As Integer = 4
            'Const ACSpoolReportMode As Integer = 5

            Dim vDocumentArray(,) As Object


            oDocLink = New bPMBDocLink.Business
            m_lReturn = oDocLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bPMBDocLink.Business.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessDocument2")
                result = gPMConstants.PMEReturnCode.PMFalse
                If Not (oDocLink Is Nothing) Then

                    oDocLink.Dispose()
                    oDocLink = Nothing
                End If
                Return result
            End If
            'Developer guide no. 108

            oDocMgrWrapper = gPMFunctions.CreateLateBoundObject("bSIRDocManagerWrapper.Interface_Renamed")
            m_lReturn = oDocMgrWrapper.InitialiseBusiness(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeString(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSIRDocManagerWrapper.Interface.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessDocument2")
                result = gPMConstants.PMEReturnCode.PMFalse
                If Not (oDocMgrWrapper Is Nothing) Then
                    oDocMgrWrapper.Dispose()
                    oDocMgrWrapper = Nothing
                End If
                If Not (oDocLink Is Nothing) Then

                    oDocLink.Dispose()
                    oDocLink = Nothing
                End If
                Return result
            End If


            m_lReturn = oDocLink.GetDocTemplate(v_lSchemeID:=v_lSchemeID, v_lAgentCnt:=v_lAgentCnt, v_sProcessTypeCode:=v_vProcessType, v_vDocumentArray:=vDocumentArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = True
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process oDocLink.GetDocTemplate().", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessDocument2")
                If Not (oDocMgrWrapper Is Nothing) Then
                    oDocMgrWrapper.Dispose()
                    oDocMgrWrapper = Nothing
                End If
                If Not (oDocLink Is Nothing) Then

                    oDocLink.Dispose()
                    oDocLink = Nothing
                End If
                Return result
            End If

            If Informations.IsArray(vDocumentArray) Then
                ' documents found
                With oDocMgrWrapper

                    For iNum As Integer = 0 To vDocumentArray.GetUpperBound(1)
                        .PartyCnt = v_lPartyCnt
                        .InsuranceFolderCnt = v_lInsuranceFolderCnt
                        .InsuranceFileCnt = v_lInsuranceFileCnt

                        .DocumentTemplateId = CInt(vDocumentArray(LCACArrayDocumentTemplateId, iNum))

                        .DocumentTypeId = CInt(vDocumentArray(LCACArrayDocumentTypeId, iNum))

                        .ArchiveDoc = CBool(vDocumentArray(LCACArrayArchiveDocument, iNum)) 'PN17352

                        If CDbl(vDocumentArray(LCACArraySpoolDocument, iNum)) = 1 Then
                            .Mode = ACSpoolDocMode
                        Else
                            .Mode = ACPrintSilentMode
                        End If

                        m_lReturn = .Start()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = True
                            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to print document", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessDocument2")
                            If Not (oDocMgrWrapper Is Nothing) Then
                                oDocMgrWrapper.Dispose()
                                oDocMgrWrapper = Nothing
                            End If
                            If Not (oDocLink Is Nothing) Then

                                oDocLink.Dispose()
                                oDocLink = Nothing
                            End If
                            Return result
                        End If
                    Next
                End With
            End If

            If Not (oDocMgrWrapper Is Nothing) Then
                oDocMgrWrapper.Dispose()
                oDocMgrWrapper = Nothing
            End If
            If Not (oDocLink Is Nothing) Then

                oDocLink.Dispose()
                oDocLink = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDocument2 Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ProcessDocument2", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            If Not (oDocMgrWrapper Is Nothing) Then
                oDocMgrWrapper.Dispose()
                oDocMgrWrapper = Nothing
            End If
            If Not (oDocLink Is Nothing) Then

                oDocLink.Dispose()
                oDocLink = Nothing
            End If

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenInvitationBefore
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    '          27/06/2001 AK - Added the functionality to print Cover-letter
    '
    ' ***************************************************************** '
    Public Function RenInvitationBefore(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRenInsFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String
        Dim lSchemeID As Integer
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CTAF 060602
            ' Get the scheme_id
            sSQL = "SELECT gis_scheme_id FROM gis_policy_link WHERE insurance_file_cnt = {insurance_file_cnt}"

            m_oDatabase.Parameters.Clear()

            ' Add the insurance_file parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lRenInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="getscheme", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                lSchemeID = CInt(vResultArray(0, 0))
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 060602
            'SJ 12/05/2004 - start
            '    m_lReturn& = ProcessDocument(v_lInsuranceFileCnt:=v_lRenInsFileCnt, _
            ''                                 v_lPartyCnt:=tosafeinteger(v_lPartyCnt), _
            ''                                 v_lSchemeID:=lSchemeID, _
            ''                                 v_vProcessType:="INVITE")
            m_lReturn = ProcessDocument2(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lRenInsFileCnt, v_lPartyCnt:=v_lPartyCnt, v_lSchemeID:=lSchemeID, v_vProcessType:="INVITE")
            'SJ 12/05/2004 - end
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitationBefore Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenInvitationBefore", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenInvitationAfter
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenInvitationAfter() As Integer

        Dim result As Integer = 0
        Try


            'this function is called by bGIS.Renewals.RenInvitationBrokerLed()
            ' do any process that should occur after the call to QEM

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitationAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenInvitationAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenInvitePreferredQuotesBefore
    '
    ' Description:
    '
    ' History: 11/09/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenInvitePreferredQuotesBefore(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRenInsFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sValue As String = ""

        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitePreferredQuotesBefore Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenInvitePreferredQuotesBefore", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenInvitePreferredQuotesAfter
    '
    ' Description:
    '
    ' History: 18/10/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function RenInvitePreferredQuotesAfter(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRenInsFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try


            'not currently called by bGIS.Renewals.RenInvitePreferredQuotes()
            'do any processing after the call to QEM
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitePreferredQuotesAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenInvitePreferredQuotesAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LapseConfirmed
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '          29/05/2001 SSL - Updated to confirm the lapse
    ' ***************************************************************** '
    Public Function LapseConfirmed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oSiriusLink As bSIRIUSLink.Renewals

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of sirius link
            oSiriusLink = New bSIRIUSLink.Renewals
            m_lReturn = oSiriusLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call sirius link

            m_lReturn = oSiriusLink.LapseConfirmed(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lSchemeID:=v_lSchemeID, v_lPartyCnt:=v_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Terminate Sirius Link

            oSiriusLink.Dispose()

            oSiriusLink = Nothing

            ' Debug message
            'Debug.Print Timer & ": Exiting " & ACApp & "." & ACClass & ".LapseConfirmed"

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LapseConfirmed Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="LapseConfirmed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenewalConfirmed
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenewalConfirmed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenewalConfirmed Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenewalConfirmed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenewalConfirmedBrokerLed
    '
    ' Description: Confirm the Renewal
    '
    ' History: TF291101 - Created from RenewalConfirmed.
    ' DD, 19/12/2001: Added new parameters. Method now determines
    '                 if Policy Number needs changing.
    '
    ' ***************************************************************** '
    Public Function RenewalConfirmedBrokerLed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, Optional ByVal v_lPolicyBinderID As Object = Nothing, Optional ByVal v_lPolicyID As Object = Nothing, Optional ByVal v_lOldInsurerNo As Object = Nothing, Optional ByVal v_lNewInsurerNo As Object = Nothing, Optional ByVal v_lSchemeNo As Object = Nothing, Optional ByVal v_sCoverCode As String = "") As Integer

        Dim result As Integer = 0
        Dim sPolNo, sQMMSuffix As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Determine if the Policy Number needs changing (change of Insurer)


            If Not v_lOldInsurerNo.Equals(v_lNewInsurerNo) Then
                'Get the chosen cover
                Select Case v_sCoverCode
                    Case "01" 'Comprehensive
                        sQMMSuffix = "293"
                    Case "02" 'TPFT
                        sQMMSuffix = "292"
                    Case "03" 'TP
                        sQMMSuffix = "291"
                End Select

                ' Generate the Policy Number
                m_lReturn = PolicyNumberGen.GenPolicyNum(v_lInsurerNo:=CInt(v_lNewInsurerNo), v_lSchemeNo:=CInt(v_lSchemeNo), v_dtEffectiveDate:=DateTime.Now, v_sQMMSuffix:=sQMMSuffix, r_sPolicyNum:=sPolNo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenPolicyNum returned with m_lReturn=" & m_lReturn, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenewalConfirmedBrokerLed", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return m_lReturn
                End If

                ' and save it in the database


                m_lReturn = SavePolNoInDB(v_oDatabase:=m_oDatabase, v_sGisBusinessTypeCode:=GISBusinessTypeCode, v_lPolicyBinderID:=CInt(v_lPolicyBinderID), v_lPolicyID:=CInt(v_lPolicyID), v_sPolNo:=sPolNo, v_lInsuranceFileCnt:=CStr(v_lInsuranceFileCnt))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn

                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Save the Policy Number in the Database", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenewalConfirmedBrokerLed", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            With m_oDatabase
                ' Update the chosen Scheme in the Renewal Control table
                ' this will ensure that the user sees the new Insurer on screen

                m_lReturn = .SQLAction("UPDATE Renewal_Control SET renewal_gis_scheme_id=" & v_lSchemeID &
                            " WHERE Insurance_folder_cnt=" & CStr(v_lInsuranceFolderCnt), "Update Renewal Control", False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Renewal Control Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenewalConfirmedBrokerLed", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Update premium details in SBO

                ' Clear parameters
                .Parameters.Clear()

                'Insurance Folder Cnt
                m_lReturn = .Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' run the stored procedure now
                m_lReturn = .SQLAction(sSQL:=ACUpdateRenewalPremiumSQL, sSQLName:=ACUpdateRenewalPremiumName, bStoredProcedure:=ACUpdateRenewalPremiumStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction for UpdateRenewalPremium Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenewalConfirmedBrokerLed", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenewalConfirmedBrokerLed Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenewalConfirmedBrokerLed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RenewalTransactAfter
    '
    ' Description:
    '
    ' TF031201  - Created from NBTransactAfter.
    ' ***************************************************************** '
    Public Function RenewalTransactAfter(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_oDataset As cGISDataSetControl.Application, ByRef r_cPremium As Decimal, ByRef r_cIPT As Decimal, ByVal v_vSchemeArray As Object, ByRef r_vAdditionalDataArray As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenewalTransactAfter Failed at line " & Informations.Erl() & " with error " & CStr(Informations.Err().Number), vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenewalTransactAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            m_oDataSet = Nothing
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompLapseBefore
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '          27/06/2001 AK - Added the functionality to print Cover-letter
    '
    ' ***************************************************************** '
    Public Function RenCompLapseBefore(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRenInsFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompLapseBefore  Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenCompLapseBefore", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompLapseAfter
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompLapseAfter(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByRef r_oDataset As cGISDataSetControl.Application) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompLapseAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenCompLapseAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompletionHoldingInsurerBefore
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '          27/06/2001 AK - Added the functionality to print Cover-letter
    '
    ' ***************************************************************** '
    Public Function RenCompHoldingInsurerBefore(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRenInsFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompHoldingInsurerBefore  Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenCompHoldingInsurerBefore", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompHoldingInsurerAfter
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompHoldingInsurerAfter(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByRef r_oDataset As cGISDataSetControl.Application) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompHoldingInsurerAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenCompHoldingInsurerAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompAlternateInsurerBefore
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompAlternateInsurerBefore() As Integer

        Dim result As Integer = 0
        Try


            'do any processing that should occur before the renewal completion with an alternate insurer

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompAlternateInsurerBefore Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenCompAlternateInsurerBefore", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompAlternateInsurerAfter
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompAlternateInsurerAfter(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByRef r_oDataset As cGISDataSetControl.Application) As Integer

        Dim result As Integer = 0
        Try


            'do any  processing that should occur after the renewal completion with an alternate insurer

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompAlternateInsurerAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenCompAlternateInsurerAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPolicyRenewalVersions
    '
    ' Description:
    '
    ' History: 21/05/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetPolicyRenewalVersions(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Dim oSiriusLink As bSIRIUSLink.Renewals

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            oSiriusLink = New bSIRIUSLink.Renewals
            m_lReturn = oSiriusLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call bSiriusLink.Renewals

            m_lReturn = oSiriusLink.GetPolicyRenewalVersions(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_vResultArray:=r_vResultArray)

            result = m_lReturn

            ' Clear up sirius link

            oSiriusLink.Dispose()

            oSiriusLink = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyRenewalVersions Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetPolicyRenewalVersions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenReminder
    '
    ' Description:
    '
    ' History: 26/06/2001 AK  - Created.
    '          11/05/2004 CJB - Although not used, update parms passed in
    '                     (in line with call from bGIS) to stop fails.
    '
    ' ***************************************************************** '
    Public Function RenReminder(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRenInsFileCnt As Integer, ByVal v_lGISSchemeID As Integer, ByVal v_bBatchRun As Boolean) As Integer

        Dim result As Integer = 0
        Dim sValue As String = ""


        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReminder Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenReminder", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompletionBefore
    '
    ' Description:
    '
    ' History: 18/10/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompletionBefore(ByRef r_oDataset As cGISDataSetControl.Application) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompletionBefore Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenCompletionBefore", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompletionAfter
    '
    ' Description:
    '
    ' History: 22/08/2001 AK - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompletionAfter(ByRef r_oDataset As cGISDataSetControl.Application) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompletionAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenCompletionAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenQuotationBrokerLeadBefore
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenQuotationBrokerLeadBefore(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByRef r_oDataset As cGISDataSetControl.Application) As Integer

        Dim result As Integer = 0
        Dim vArray As Object
        Dim sOIKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AK 260402 - extract Scheme group
            m_lReturn = GetGisSchemeGroupID(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

            ' Clear alll previous quote output   PN17260

            m_lReturn = r_oDataset.GetAllOIKey(r_oDataset.GISDataModelCode & "_OUTPUT", vArray)

            If Informations.IsArray(vArray) Then


                For iCnt As Integer = vArray.GetLowerBound(0) To vArray.GetUpperBound(0)

                    sOIKey = CStr(vArray(iCnt))
                    m_lReturn = r_oDataset.DelObjectInstance(r_oDataset.GISDataModelCode & "_OUTPUT", sOIKey)
                Next iCnt

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenQuotationBrokerLeadBefore Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadBefore", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenQuotationBrokerLeadAfter
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '          130901     AK - Put some life into it
    '          06/04/2004 CJB - Suspend if the current scheme did not quote
    '                           (we can't allow batch processing to continue
    '                           with a different quote!). Also if the current
    '                           scheme didn't quote (it declined or referred)
    '                           then suspend the renewal with a relevant
    '                           reason...keeping code in to check premium
    '                           tolerance and suspend if not in limits
    '                           (but this is only done for comparative quotes).
    '          26/10/2004 CJB - PN16186 Suspend (instead of just exiting function)
    '                           if all schemes did not either quote, decline or
    '                           refer. Also suspend if no quotes, declines or
    '                           refers etc from the holding scheme.
    ' ***************************************************************** '
    Public Function RenQuotationBrokerLeadAfter(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByRef r_oDataset As cGISDataSetControl.Application, ByRef r_cPremium As Decimal, ByRef r_cIPT As Decimal, ByVal v_lSchemeID As Integer) As Integer


        Dim result As Integer = 0
        Dim sOutputTable As String = ""
        Dim vOIKeyArray As Object
        Dim sOutput As String = ""
        Dim vValue As Boolean
        Dim lSchemeID As Integer
        Dim sSchemeName As String = ""
        Dim bUseExistingScheme As Boolean
        Dim sKeyToKeep As String = ""
        Dim vQuoteArray(,) As Object
        Dim cExistingPremium As Decimal
        Dim nMaxChangeNum, nMinChangeNum As Single
        Dim lPartyCnt As Integer
        Dim bSuspendRenewal As Boolean
        Dim lEventCnt As Integer
        Dim oSiriusLink As bSIRIUSLink.Renewals
        Dim sCurrentSchemeInstance As String
        Dim sKeep As New StringBuilder
        Dim lNumQuotes As Integer
        Dim cIptRate As Decimal
        Dim sOIKey As String = ""
        Dim oQuoteDisplay, vQuoteFees As Object
        Dim vLapsedReasonCode As String = ""
        Dim bBrokerlinkPolicy As Boolean

        ''PN12541
        'Dim sStatus As String
        'Dim sReferReason As String
        'Dim sDeclineReason As String

        'PN12541End

        ' CJB 06/04/2004
        Dim sStatus, sDeclineReferReason As String
        Dim sSuspendReason As New StringBuilder

        Const ACKeyIndex As Integer = 0
        Const ACQuotedPremium As Integer = 1
        Const ACIsFavourable As Integer = 2

        ' CJB 06/04/2004 Quote Status Constants
        Const ACStatusQuote As String = "QUOTE"
        Const ACStatusDecline As String = "DECLINE"
        Const ACStatusRefer As String = "REFER"

        ' CJB 06/04/2004
        Const ACNoReason As String = "No reason defined"
        ' Const ACQuoteErrored As String = "Quote Errored"
        Const ACNoRecognisedStatusSet As String = "No Recognised Status was set"
        Const ACSuspendAsZeroPremium As String = "Zero Premium"

        ' CJB 26/10/2004 PN16186
        Const ACNoQuotesDeclinesRefersFromAllSchemes As String = "No schemes have quoted, declined or referred"
        Const ACNoQuotesDeclinesRefersFromCurrentScheme As String = "Current scheme has not quoted, declined or referred"
        Dim bFoundExistingScheme As Boolean

        'JES Valentine's Day 2003
        'initialise holding scheme premium
        Dim cCurrentSchemePremium As Decimal = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bSuspendRenewal = False

            m_lReturn = CheckBrokerlinkPolicy(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_bBrokerlinkPolicy:=bBrokerlinkPolicy)

            'sj 18/09/2002 - start
            'Get the ipt rate for this risk
            m_lReturn = GetIPT(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_cIptRate:=cIptRate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetIpt Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter")
                Return result
            End If
            'sj 18/09/2002 - end

            ' Get existing premium and scheme tolerances

            sOutputTable = m_sOutPutObject

            m_lReturn = r_oDataset.GetAllOIKey(v_sObjectName:=sOutputTable, r_vOIKeyArray:=vOIKeyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process r_oDataset.GetAllOIKey(" & sOutputTable & ").", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Informations.IsArray(vOIKeyArray) Then

                ' Check premium tolerances
                bUseExistingScheme = False

                ReDim vQuoteArray(2, vOIKeyArray.GetUpperBound(0))


                For lLoop As Integer = vOIKeyArray.GetLowerBound(0) To vOIKeyArray.GetUpperBound(0)

                    ' Get the index

                    sOutput = CStr(vOIKeyArray(lLoop))

                    ' Get the scheme
                    m_lReturn = r_oDataset.GetPropertyValue(v_sObjectName:=sOutputTable, v_sPropertyName:="scheme_id", v_sOIKey:=sOutput, r_vPropertyValue:=CStr(lSchemeID), r_bIsAssumedInfo:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process r_oDataset.GetPropertyValue(scheme_id).", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    m_lReturn = r_oDataset.GetPropertyValue(v_sObjectName:=sOutputTable, v_sPropertyName:="scheme_desc", v_sOIKey:=sOutput, r_vPropertyValue:=sSchemeName, r_bIsAssumedInfo:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process r_oDataset.GetPropertyValue(scheme_desc).", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    'AK 170402 - check if it is the current (holding) scheme
                    If lSchemeID = v_lSchemeID Then

                        ' CJB 26/10/2004 PN16186 Set flag indicating that the current scheme has quote o/p
                        bFoundExistingScheme = True

                        ' CJB 06/04/2004 Check if scheme actually quoted instead of just checking the
                        ' premium (note that you can refer with a premium!)
                        m_lReturn = r_oDataset.GetPropertyValue(v_sObjectName:=sOutputTable, v_sPropertyName:="status", v_sOIKey:=sOutput, r_vPropertyValue:=sStatus, r_bIsAssumedInfo:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process r_oDataset.GetPropertyValue(status).", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return result
                        End If

                        ' CJB 06/04/2004 If declined or referred then get reason
                        If sStatus.Trim().ToUpper() = ACStatusDecline Or sStatus.Trim().ToUpper() = ACStatusRefer Then
                            m_lReturn = r_oDataset.GetPropertyValue(v_sObjectName:=sOutputTable, v_sPropertyName:="reason", v_sOIKey:=sOutput, r_vPropertyValue:=sDeclineReferReason, r_bIsAssumedInfo:=False)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process r_oDataset.GetPropertyValue(reason).", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                Return result
                            End If

                        End If

                        ' CJB 06/04/2004 If no quote then set reason why to store in suspend reason later
                        ' Don't bother checking for ERROR as the processing wont get this far in that case

                        Select Case sStatus.Trim().ToUpper()
                            Case ACStatusQuote

                            Case ACStatusDecline
                                bSuspendRenewal = True
                                sSuspendReason = New StringBuilder("Quote Declined - ")
                                If sDeclineReferReason.Trim() <> "" Then
                                    sSuspendReason.Append(sDeclineReferReason.Trim())
                                Else
                                    sSuspendReason.Append(ACNoReason)
                                End If

                            Case ACStatusRefer
                                bSuspendRenewal = True
                                sSuspendReason = New StringBuilder("Quote Referred - ")
                                If sDeclineReferReason.Trim() <> "" Then
                                    sSuspendReason.Append(sDeclineReferReason.Trim())
                                Else
                                    sSuspendReason.Append(ACNoReason)
                                End If

                            Case Else
                                bSuspendRenewal = True
                                sSuspendReason = New StringBuilder(ACNoRecognisedStatusSet)
                                If sStatus.Trim() <> "" Then
                                    sSuspendReason.Append(" - " & sStatus.Trim())
                                End If
                        End Select

                        'Get Premium details, to post to Insurance file
                        m_lReturn = r_oDataset.GetPropertyValue(v_sObjectName:=sOutputTable, v_sPropertyName:="premium_minus_ipt", v_sOIKey:=sOutput, r_vPropertyValue:=CStr(r_cPremium), r_bIsAssumedInfo:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process r_oDataset.GetPropertyValue(premium_minus_ipt).", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return result
                        End If
                        m_lReturn = r_oDataset.GetPropertyValue(v_sObjectName:=sOutputTable, v_sPropertyName:="ipt_value", v_sOIKey:=sOutput, r_vPropertyValue:=CStr(r_cIPT), r_bIsAssumedInfo:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process r_oDataset.GetPropertyValue(ipt_value).", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return result
                        End If

                        'PN12541
                        '            m_lReturn = r_oDataSet.GetPropertyValue( _
                        ''                                    v_sObjectName:=sOutputTable, _
                        ''                                    v_sPropertyName:="status", _
                        ''                                    v_sOIKey:=sOutput, _
                        ''                                    r_vPropertyValue:=sStatus, _
                        ''                                    r_bIsAssumedInfo:=False)
                        '
                        '            If (m_lReturn& <> PMTrue) Then
                        '                RenQuotationBrokerLeadAfter = PMFalse
                        '                LogMessage m_sUsername, _
                        ''                    iType:=PMLogError, _
                        ''                    sMsg:="Failed to process r_oDataset.GetPropertyValue(status).", _
                        ''                    vApp:=tosafestring(ACApp), _
                        ''                    vClass:=ACClass, _
                        ''                    vMethod:="RenQuotationBrokerLeadAfter", _
                        ''                    vErrNo:=Err.Number, _
                        ''                    vErrDesc:=Err.Description
                        '                Exit Function
                        '            End If
                        '            m_lReturn = r_oDataSet.GetPropertyValue( _
                        ''                                    v_sObjectName:=sOutputTable, _
                        ''                                    v_sPropertyName:="refer_reason", _
                        ''                                    v_sOIKey:=sOutput, _
                        ''                                    r_vPropertyValue:=sReferReason, _
                        ''                                    r_bIsAssumedInfo:=False)
                        '
                        '            If (m_lReturn& <> PMTrue) Then
                        '                RenQuotationBrokerLeadAfter = PMFalse
                        '                LogMessage m_sUsername, _
                        ''                    iType:=PMLogError, _
                        ''                    sMsg:="Failed to process r_oDataset.GetPropertyValue(refer_reason).", _
                        ''                    vApp:=tosafestring(ACApp), _
                        ''                    vClass:=ACClass, _
                        ''                    vMethod:="RenQuotationBrokerLeadAfter", _
                        ''                    vErrNo:=Err.Number, _
                        ''                    vErrDesc:=Err.Description
                        '                Exit Function
                        '            End If
                        '            m_lReturn = r_oDataSet.GetPropertyValue( _
                        ''                                    v_sObjectName:=sOutputTable, _
                        ''                                    v_sPropertyName:="decline_reason", _
                        ''                                    v_sOIKey:=sOutput, _
                        ''                                    r_vPropertyValue:=sDeclineReason, _
                        ''                                    r_bIsAssumedInfo:=False)
                        '
                        '            If (m_lReturn& <> PMTrue) Then
                        '                RenQuotationBrokerLeadAfter = PMFalse
                        '                LogMessage m_sUsername, _
                        ''                    iType:=PMLogError, _
                        ''                    sMsg:="Failed to process r_oDataset.GetPropertyValue(decline_reason).", _
                        ''                    vApp:=tosafestring(ACApp), _
                        ''                    vClass:=ACClass, _
                        ''                    vMethod:="RenQuotationBrokerLeadAfter", _
                        ''                    vErrNo:=Err.Number, _
                        ''                    vErrDesc:=Err.Description
                        '                Exit Function
                        '            End If

                        'PN12541End


                        'Jes 09 May 2002 --- Get holding scheme premium
                        cCurrentSchemePremium = r_cPremium
                        sCurrentSchemeInstance = sOutput

                        If Not (bBrokerlinkPolicy) Then
                            r_cIPT = (r_cPremium * (100 + cIptRate) / 100) - r_cPremium
                        End If
                        r_cPremium += r_cIPT

                        'get holding scheme settings
                        With m_oDatabase
                            .Parameters.Clear()

                            m_lReturn = .Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                            m_lReturn = .Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(lSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                            m_lReturn = .SQLSelect(sSQL:=ACPremiumTolerancesSQL, sSQLName:=ACPremiumTolerancesName, bStoredProcedure:=ACPremiumTolerancesStored)
                        End With

                        If m_oDatabase.Records.Count() <> 1 Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process " & ACPremiumTolerancesSQL, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return result
                        End If

                        With m_oDatabase.Records.Item(1).Fields
                            cExistingPremium = gPMFunctions.NullToLong(m_oDatabase.Records.Item(1).Fields()("existing_premium"))
                            nMaxChangeNum = gPMFunctions.NullToLong(m_oDatabase.Records.Item(1).Fields("max_tolerance"))
                            nMinChangeNum = gPMFunctions.NullToLong(m_oDatabase.Records.Item(1).Fields("min_tolerance"))
                            lPartyCnt = gPMFunctions.NullToLong(m_oDatabase.Records.Item(1).Fields("party_cnt"))
                        End With

                        'end of holding scheme if
                    End If

                    ' Get the quoted premium for this output item
                    m_lReturn = r_oDataset.GetPropertyValue(v_sObjectName:=sOutputTable, v_sPropertyName:="premium_minus_ipt", v_sOIKey:=sOutput, r_vPropertyValue:=CStr(vValue), r_bIsAssumedInfo:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process r_oDataset.GetPropertyValue(premium_minus_ipt).", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    ' Populate temporary array

                    vQuoteArray(ACKeyIndex, lLoop) = sOutput

                    vQuoteArray(ACQuotedPremium, lLoop) = vValue
                    'default to wanted

                    vQuoteArray(ACIsFavourable, lLoop) = True

                    'end of output loop
                Next lLoop

                'Jes 09 May 2002 --- Selection quotes to stay

                'initialise number of quotes to 0
                lNumQuotes = 0

                'sort array in premium order

                m_lReturn = gPMFunctions.ShellSort2DArray(r_vArray:=vQuoteArray, v_iSortColumn:=ACQuotedPremium)

                'process selections
                'TF060902 - Let's use the correct array
                'For lLoop = LBound(vOIKeyArray) To UBound(vOIKeyArray)

                For lLoop As Integer = vQuoteArray.GetLowerBound(1) To vQuoteArray.GetUpperBound(1)

                    ' Get the index
                    'sOutput = vOIKeyArray(lLoop)

                    sOutput = CStr(vQuoteArray(ACKeyIndex, lLoop))

                    ' Get the scheme
                    m_lReturn = r_oDataset.GetPropertyValue(v_sObjectName:=sOutputTable, v_sPropertyName:="scheme_id", v_sOIKey:=sOutput, r_vPropertyValue:=CStr(lSchemeID), r_bIsAssumedInfo:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process r_oDataset.GetPropertyValue(scheme_id).", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    'handle existing scheme
                    If lSchemeID = v_lSchemeID Then

                        'JES 140203 keep it if it's quoted
                        If cCurrentSchemePremium > 0 Then
                            'make sure it's kept ( Probably not necessary )

                            vQuoteArray(ACIsFavourable, lLoop) = True
                        Else

                            vQuoteArray(ACIsFavourable, lLoop) = False

                            ' CJB 06/04/2004 Since there is a zero premium for the renewal quote and we
                            ' are now going to suspend the renewal process if the current quote doesn't
                            ' quote (or quotes zero) then set the suspend reason here
                            sSuspendReason = New StringBuilder(ACSuspendAsZeroPremium)
                            bSuspendRenewal = True
                        End If

                    Else

                        'If lNumQuotes > 3 Then reject
                        If lNumQuotes > 3 Then
                            'Reject other quotes

                            vQuoteArray(ACIsFavourable, lLoop) = False
                        Else

                            'if no premium for holding insurer then display top 3
                            If cCurrentSchemePremium = 0 Then
                                'DJM 13/02/2004 PN8921 : Only keep other quotes if they have a premium.

                                If CDbl(vQuoteArray(ACQuotedPremium, lLoop)) <> 0 Then
                                    'Increment no of quotes
                                    lNumQuotes += 1
                                    'make sure it's kept ( Probably not necessary )

                                    vQuoteArray(ACIsFavourable, lLoop) = True
                                Else

                                    vQuoteArray(ACIsFavourable, lLoop) = False
                                End If
                            Else
                                'check cheapness contraint

                                If CDbl(vQuoteArray(ACQuotedPremium, lLoop)) <= (cCurrentSchemePremium * (1 - (nMinChangeNum / 100))) Then
                                    'Increment no of quotes
                                    lNumQuotes += 1
                                    'make sure it's kept ( Probably not necessary )

                                    vQuoteArray(ACIsFavourable, lLoop) = True
                                Else
                                    'reject it

                                    vQuoteArray(ACIsFavourable, lLoop) = False
                                End If
                            End If
                        End If

                    End If

                    'TF060902 - Build the 'keep' listy now as sequence is correct

                    If CBool(vQuoteArray(ACIsFavourable, lLoop)) Then
                        If sKeep.ToString() > "" Then
                            sKeep.Append(", ")
                        End If

                        'Jes 05/02/2003 removing OI from start of string

                        sOIKey = CStr(vQuoteArray(ACKeyIndex, lLoop))
                        sOIKey = sOIKey.Replace("OI", "")

                        'add OI to keep string
                        sKeep.Append(sOIKey)
                    End If
                    'end of selection loop
                Next lLoop

                ' CJB 26/10/2004 PN16186 If the current scheme had no output then suspend
                If Not bFoundExistingScheme Then
                    bSuspendRenewal = True
                    sSuspendReason = New StringBuilder(ACNoQuotesDeclinesRefersFromCurrentScheme)
                End If

            Else
                ' CJB 26/10/2004 PN16186 Suspend if there are no quotes, declines or refers from ANY scheme!
                bSuspendRenewal = True
                sSuspendReason = New StringBuilder(ACNoQuotesDeclinesRefersFromAllSchemes)
            End If

            ' CJB 06/04/2004 We now suspend under the following scenarios:
            '   Current scheme had quote output but NOT with a "QUOTE" status
            '   Current scheme quoted but had a zero premium
            '   No quote output found i.e. no schemes quoted, referred or declined PN16186
            '   Current scheme did not quote (or decline or refer etc) PN16186
            ' Code before was just seeing if the premium was zero AND no other schemes quoted
            '  (If cCurrentSchemePremium = 0 And lNumQuotes = 0'... and was setting the reason
            '   as suspended as premium was ouside of tolerances regardless. Note that tolerances
            '   are only checked for schemes other than the current one and we only suspend if the
            '   current scheme does not quote with a premium so we'll no longer have an outside
            '   tolerances reason...
            ' We now need to ignore the other quotes and suspend if we're not happy with the current
            ' quote as we can't let a batch renewal process continue with a different scheme!

            If bSuspendRenewal Then

                oSiriusLink = New bSIRIUSLink.Renewals
                m_lReturn = oSiriusLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process CreateBusinessObject(bSiriusLink.Renewals).", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' CJB 07/04/2004 We will also set the new short suspension reason on the event log
                ' (this is shown in the Renewal manager's listview in the status column.

                m_lReturn = oSiriusLink.SuspendEvent(v_lPartyCnt:=lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSuspendReason:=sSuspendReason.ToString(), r_lEventCnt:=lEventCnt, v_sShortSuspendReason:=sSuspendReason.ToString().Substring(0, 70).Trim())

                '        'PN12541 Vary suspend message based on reason
                '        If UCase(sStatus) = "REFER" Then
                '         m_lReturn = oSiriusLink.SuspendEvent( _
                ''                        v_lPartyCnt:=tosafeinteger(lPartyCnt), _
                ''                        v_lInsuranceFolderCnt:=tosafeinteger(v_lInsuranceFolderCnt), _
                ''                        v_sSuspendReason:="REFER: " & sReferReason, _
                ''                        r_lEventCnt:=tosafeinteger(lEventCnt))
                '
                '        Else
                '            If UCase(sStatus) = "DECLINE" Then
                '                m_lReturn = oSiriusLink.SuspendEvent( _
                ''                        v_lPartyCnt:=tosafeinteger(lPartyCnt), _
                ''                        v_lInsuranceFolderCnt:=tosafeinteger(v_lInsuranceFolderCnt), _
                ''                        v_sSuspendReason:="DECLINE: " & sDeclineReason, _
                ''                        r_lEventCnt:=tosafeinteger(lEventCnt))
                '
                '            Else
                '            m_lReturn = oSiriusLink.SuspendEvent( _
                ''                        v_lPartyCnt:=tosafeinteger(lPartyCnt), _
                ''                        v_lInsuranceFolderCnt:=tosafeinteger(v_lInsuranceFolderCnt), _
                ''                        v_sSuspendReason:="Premium Outside Tolerances", _
                ''                        r_lEventCnt:=tosafeinteger(lEventCnt))
                '            End If
                '        End If
                '        'PN12541End

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process oSiriusLink.SuspendEvent.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = SetSuspensionLevel(v_lSuspensionLevel:=lEventCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process oSiriusLink.SetSuspensionLevel.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If


                oSiriusLink.Dispose()
                oSiriusLink = Nothing

                Return result
            End If

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="POLICYLINKID", vValue:=CStr(r_oDataset.PolicyLinkID()), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .Parameters.Add(sName:="DATAMODELCODE", vValue:=m_sDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                'jes 5/02/03 PN ref 1986 START
                'm_lReturn = .Parameters.Add( _
                'sName:="SCHEMENAME", _
                'vValue:=sSchemeName, _
                'idirection:=PMParamInput, _
                'iDataType:=PMString)


                'Developer Guide no. 85

                m_lReturn = .Parameters.Add(sName:="SCHEMENAME", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                'jes 5/02/03 PN ref 1986 END

                m_lReturn = .Parameters.Add(sName:="sKeep", vValue:=sKeep.ToString(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .SQLAction(sSQL:="{call spu_GIS_clear_quote_output (?,?,?,?)}", sSQLName:=" ", bStoredProcedure:=True)
            End With

            ' SET 30/03/2004 ISS11186
            m_lReturn = CreateBusinessObject(sClassName:="bPMBQuoteDisplay.Business", r_oObject:=oQuoteDisplay)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process CreateBusinessObject(bCNQuoteDisplay.Business).", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter")
                oQuoteDisplay = Nothing
                Return result
            End If

            ' get fees and extras

            m_lReturn = oQuoteDisplay.GetPolicyFees(v_lInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_sTransactionCode:=gPMConstants.PMTransactionTypeNB, r_vQuoteFees:=vQuoteFees)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process oQuoteDisplay.GetRenewalFees.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter")

                oQuoteDisplay.Dispose()
                oQuoteDisplay = Nothing
                Return result
            End If


            m_lReturn = oQuoteDisplay.UpdateAddOns(v_lInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_vAddonArray:=vQuoteFees)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process oQuoteDisplay.UpdateAddOns.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter")

                oQuoteDisplay.Dispose()
                oQuoteDisplay = Nothing
                Return result
            End If


            oQuoteDisplay.Dispose()
            oQuoteDisplay = Nothing

            'SJ 12/05/2004 - start
            If sCurrentSchemeInstance <> "" Then
                'Do we have as lapse reason?
                'If so print the document
                m_lReturn = r_oDataset.GetPropertyValue(v_sObjectName:=sOutputTable, v_sPropertyName:="lapsed_reason_code", v_sOIKey:=sOutput, r_vPropertyValue:=vLapsedReasonCode, r_bIsAssumedInfo:=False)

                If vLapsedReasonCode.Trim() <> "" Then
                    m_lReturn = ProcessDocument2(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lPartyCnt:=lPartyCnt, v_lSchemeID:=v_lSchemeID, v_vProcessType:="RENQUOTEL")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If
            'SJ 12/05/2004 - end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            If Not (oQuoteDisplay Is Nothing) Then

                oQuoteDisplay.Dispose()
                oQuoteDisplay = Nothing
            End If

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenQuotationBrokerLeadAfter Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="RenQuotationBrokerLeadAfter", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SelectRenewalControl
    '
    ' Description: Reads the contents of the renewal control table
    '
    ' History:  14/03/2001 CTAF - Created.
    '           25/10/2001 DD - Fully implemented the parameters.
    '           29/10/2001 DD - added SuspensionLevel parameter.
    '
    ' ***************************************************************** '
    Public Function SelectRenewalControl(ByRef r_vResultArray As String, Optional ByVal v_sRenewalStatus As String = "", Optional ByVal v_dtDueDateStart As Date = #12/29/1899#, Optional ByVal v_dtDueDateLimit As Date = #12/29/1899#, Optional ByVal v_sClientCode As String = "", Optional ByVal v_sPolicyNo As String = "", Optional ByVal v_lBusinessTypeId As Integer = 0, Optional ByVal v_lSchemeID As Integer = 0, Optional ByVal v_lInsurerId As Integer = 0, Optional ByVal v_lSuspensionLevel As Integer = 0, Optional ByVal v_lOfferAlt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                ' Clear the parameters
                .Parameters.Clear()

                ' DD 25102001
                ' If the parameters are the defaults then send Nulls through


                'Developer Guide No. 85

                m_lReturn = .Parameters.Add(sName:="RenewalStatus", vValue:=If(v_sRenewalStatus = "", DBNull.Value, v_sRenewalStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="DueDateStart", vValue:=If(v_dtDueDateStart = #12/29/1899#, DBNull.Value, v_dtDueDateStart.ToString), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="DueDateLimit", vValue:=If(v_dtDueDateLimit = #12/29/1899#, DBNull.Value, v_dtDueDateLimit.ToString), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="ClientCode", vValue:=If(v_sClientCode = "", DBNull.Value, v_sClientCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="PolicyNo", vValue:=If(v_sPolicyNo = "", DBNull.Value, v_sPolicyNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="BusinessTypeID", vValue:=If(v_lBusinessTypeId = 0, DBNull.Value, CStr(v_lBusinessTypeId)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="SchemeId", vValue:=If(v_lSchemeID = 0, DBNull.Value, CStr(v_lSchemeID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="InsurerId", vValue:=If(v_lInsurerId = 0, DBNull.Value, CStr(v_lInsurerId)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="SuspensionLevel", vValue:=If(v_lSuspensionLevel = 0, DBNull.Value, CStr(v_lSuspensionLevel)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = .Parameters.Add(sName:="OfferAlt", vValue:=If(v_lOfferAlt = 0, DBNull.Value, CStr(v_lOfferAlt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call the procedure
                m_lReturn = .SQLSelect(sSQL:=ACRenewalManagerSelectSQL, sSQLName:=ACRenewalManagerSelectName, bStoredProcedure:=ACRenewalManagerSelectStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            ' Check we have some results
            If Not Informations.IsArray(r_vResultArray) Then
                r_vResultArray = Nothing
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectRenewalControl Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="SelectRenewalControl", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Public Function - End

    'Private Function - Start
    ' ***************************************************************** '
    '
    ' Name: CreateBusinessObject
    '
    ' Description: create an instance of the specified class and return it
    '
    ' History: 16/10/2001 TN - Created.
    '
    ' ***************************************************************** '
    Private Function CreateBusinessObject(ByVal sClassName As String, ByRef r_oObject As Object) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create a new component services

        ' New instance of bSIRIUSLink
        m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=r_oObject, v_sClassName:=sClassName, v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of " & sClassName, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        End If


        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: SetSuspensionLevel
    '
    ' Description: set suspension level to specified value
    '
    ' History: 23/10/2001 Thinh.Nguyen - Created.
    '
    ' ***************************************************************** '
    Private Function SetSuspensionLevel(ByVal v_lSuspensionLevel As Integer, ByVal v_lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        sSQL = "UPDATE Renewal_Control SET suspension_level = {suspension_level}" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "WHERE insurance_folder_cnt = {insurance_folder_cnt}" & Strings.ChrW(13) & Strings.ChrW(10)

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="suspension_level", vValue:=CStr(v_lSuspensionLevel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.SQLBeginTrans()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="SetSuspensionLevel", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = m_oDatabase.SQLRollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLCommitTrans()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = m_oDatabase.SQLRollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetGisSchemeGroupID
    '
    ' Description: return gis scheme group id for a insurance file
    '
    ' AK - 260402 - created
    ' ***************************************************************** '
    Private Function GetGisSchemeGroupID(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        m_lSchemeGroupId = 0

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSchemeGroupSQL, sSQLName:=ACGetSchemeGroupName, bStoredProcedure:=ACGetSchemeGroupStored, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        m_lSchemeGroupId = CInt(vResultArray(0, 0))

        Return result

    End Function

    'Private Function - End

    ' ***************************************************************** '
    '
    ' Name: CreateSiriusLinkClaims
    '
    ' Description:
    '
    ' History: 30/07/01 - AK - Created.
    '
    ' ***************************************************************** '
    Private Function CreateSiriusLinkClaims(ByRef r_oSiriusLinkClaims As Object) As Integer


        Dim result As Integer = 0


        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".CreateSiriusLinkClaims")



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create a new component services

        ' New instance of bSIRIUSLink
        r_oSiriusLinkClaims = New bSIRIUSLink.Claims
        Dim oDatabase As Object = m_oDatabase
        m_lReturn = r_oSiriusLinkClaims.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeString(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSIRIUSLink.Renewals", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreateSiriusLinkClaims", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If
        m_oDatabase = oDatabase


        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".CreateSiriusLinkClaims")

        Return result

    End Function

    'AK 230402 -added this function to update NCD on Renewal-Risk
    Private Function UpdateRiskNCD(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_oDataset As cGISDataSetControl.Application) As Integer

        Dim result As Integer = 0
        Const LCNCDDefault As Single = 0

        Dim lNCD As Integer

        Dim sSQL As String = ""

        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Construct the SQL
        'AK 121001 check for existence of null value
        sSQL = "SELECT renewal_NCD_Year FROM Insurance_Folder " &
               "WHERE insurance_folder_cnt = {insurance_folder_cnt}"

        ' Clear parameters
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the NCD years off the InsuranceFolder
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetNCD", bStoredProcedure:=False, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            ' Default to 0
            lNCD = CInt(LCNCDDefault)
        Else

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(vResultArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                ' Retrieve the value

                lNCD = CInt(vResultArray(0, 0))
            Else
                'AK 021101 - If no NCD has been set on the insurance folder, then
                '            NCD can be incremented by 1, assuming that no claims exist for this policy,
                '            as user has not marked it for update
                ' Default to the exisitng value (and add one now)
                lNCD = CInt(Val(r_oDataset.Risk.Item("Policy").Item("NCBYears").Value) + 1)
            End If
        End If

        ' Update the Risk
        ' If this is wrong then use NCD_YEARS_EARNED
        r_oDataset.Risk.Item("Policy").Item("NCBYears").Value = CStr(lNCD)

        'AK 021101 - Will need to reset the NCD on Insurance Folder back to NULL, so that the
        '            same logic of extracting Updated NCD can be applied

        ' Construct the SQL

        ' Clear parameters
        m_oDatabase.Parameters.Clear()

        sSQL = "UPDATE Insurance_Folder Set renewal_NCD_Year = NULL " &
               "WHERE insurance_folder_cnt = " & CStr(v_lInsuranceFolderCnt)



        ' execute the SQL now
        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="ResetNCD", bStoredProcedure:=False)

        'AK 021101 - End

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetIPT
    '
    ' Description:
    '
    ' History: 18/09/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetIPT(ByVal v_lInsuranceFileCnt As Integer, ByRef r_cIptRate As Decimal) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vResultArray(,) As Object

        ' Clear parameters
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetIptSQL, sSQLName:=ACGetIptName, bStoredProcedure:=ACGetIptStored, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            r_cIptRate = 0
        Else

            r_cIptRate = CDec(vResultArray(0, 0))
        End If


        Return result

    End Function

    Private Function CheckBrokerlinkPolicy(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bBrokerlinkPolicy As Boolean) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim oGis As Object = Nothing
        Dim sQemCode, sDmCode As Object

        ' oGis = New bGis.Application
        ' Create bGIS.Application object
        m_lReturn = CreateBusinessObject(r_oObject:=oGis, sClassName:="bGIS.Application")
        If m_lReturn <> PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError, sMsg:="NBQuoteAfter Failed to Initialise bGIS.Application object.", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="NBQuoteAfter", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If
        Dim oDatabase As Object = m_oDatabase
        m_lReturn = oGis.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeString(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bGis.Application", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CheckBrokerlinkPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        End If
        m_oDatabase = oDatabase


        m_lReturn = oGis.GetQemDmCode(ToSafeInteger(v_lInsuranceFileCnt), r_sQemCode:=sQemCode, r_sDmCode:=sDmCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to run bGis.Application.GetQemDmCode", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CheckBrokerlinkPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        End If

        If Not (oGis Is Nothing) Then

            oGis.Dispose()
            oGis = Nothing
        End If

        If sQemCode.Trim().ToUpper() = "BROKERLINK" Then
            r_bBrokerlinkPolicy = True
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: IndexLink
    '
    ' Description:
    '
    ' History: 29/03/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function IndexLink(ByVal v_dtRenewalDate As Date, ByRef r_oDataset As cGISDataSetControl.Application) As Integer 'PN16651

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Const ACObjectName As Integer = 0
        Const ACPropertyName As Integer = 1
        Const ACPercentage As Integer = 3

        Dim vResultArray(,) As Object
        Dim sLastObjectAndPropertyName, sCurrentObjectAndPropertyName As String

        m_lReturn = GetIndexLinkDetails(v_dtRenewalDate:=v_dtRenewalDate, r_vResultArray:=vResultArray) 'PN16651
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetIndexLinkDetails Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="IndexLink")
            Return result
        End If

        If Not Informations.IsArray(vResultArray) Then
            'Nothing to do
            Return result
        End If


        For iCnt As Integer = 0 To vResultArray.GetUpperBound(1)

            ' CJB 280904 PN15174 The array of indexation results contains all current & previously dated rates for
            ' each object and property (that has rates for it) in the datamodel...they are now returned in object,
            ' property and date desc order so for each unique name just get the 1st result and use the % from it
            ' Code was previously applying all rates so just the last got applied (i.e. the oldest)!

            sCurrentObjectAndPropertyName = CStr(vResultArray(ACObjectName, iCnt)).Trim() & "/" & CStr(vResultArray(ACPropertyName, iCnt)).Trim()
            If sLastObjectAndPropertyName <> sCurrentObjectAndPropertyName Then




                m_lReturn = IndexLinkProperty(r_oDataset:=r_oDataset, v_sObjectName:=CStr(vResultArray(ACObjectName, iCnt)), v_sPropertyName:=CStr(vResultArray(ACPropertyName, iCnt)), v_cPercentage:=Val(CStr(vResultArray(ACPercentage, iCnt))))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(sUsername:=ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IndexLinkProperty Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="IndexLink")
                    Return result
                End If

                ' Save the object/property just processed as we just have applied the correct % indexation and don't
                ' want to process any further (older) ones for this property...

                sLastObjectAndPropertyName = CStr(vResultArray(ACObjectName, iCnt)).Trim() & "/" & CStr(vResultArray(ACPropertyName, iCnt)).Trim()
            End If

        Next iCnt

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: GetIndexLinkDetails
    '
    ' Description:
    '
    ' History: 29/03/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetIndexLinkDetails(ByVal v_dtRenewalDate As Date, ByRef r_vResultArray As Object) As Integer  'PN16651

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()

            'effective_date
            m_lReturn = .Parameters.Add(sName:="effective_date", vValue:=v_dtRenewalDate.ToString, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) 'PN16651
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'gis_data_model_code
            m_lReturn = .Parameters.Add(sName:="gis_data_model_code", vValue:=m_sDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLSelect(sSQL:=ACGetIndexLinkDetailsSQL, sSQLName:=ACGetIndexLinkDetailsName, bStoredProcedure:=ACGetIndexLinkDetailsStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction for GetIndexLinkDetails Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GetIndexLinkDetails")
                Return result
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: IndexLinkProperty
    '
    ' Description:
    '
    ' History: 29/03/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function IndexLinkProperty(ByRef r_oDataset As cGISDataSetControl.Application, ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByVal v_cPercentage As Decimal) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vOIKeyArray As Object
        Dim cSumInsured As Decimal
        Dim bIsAssumedInfo As Boolean
        Dim vPropertyValue As String = ""

        If v_cPercentage = 0 Then
            'No increase so exit no
            Return result
        End If


        m_lReturn = r_oDataset.GetAllOIKey(v_sObjectName:=v_sObjectName, r_vOIKeyArray:=vOIKeyArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="r_oDataSet.GetAllOIKey Failed for " & v_sObjectName, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="IndexLinkProperty")
            Return result
        End If

        If Not Informations.IsArray(vOIKeyArray) Then
            'Nothing to do
            Return result
        End If

        'Loop around all instances of the object updating the property value

        For iCnt As Integer = 0 To vOIKeyArray.GetUpperBound(0)

            m_lReturn = r_oDataset.GetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, v_sOIKey:=CStr(vOIKeyArray(iCnt)), r_vPropertyValue:=vPropertyValue, r_bIsAssumedInfo:=bIsAssumedInfo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="r_oDataSet.GetPropertyValue Failed for " & v_sPropertyName, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="IndexLinkProperty")
                Return result
            End If

            If Val(vPropertyValue) > 0 Then
                'If the sum insured is > 0 then calculate the new value and update the
                'property
                cSumInsured = CDec(vPropertyValue) * (1 + (v_cPercentage / 100))


                m_lReturn = r_oDataset.SetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, v_sOIKey:=CStr(vOIKeyArray(iCnt)), v_vPropertyValue:=CStr(cSumInsured), v_bIsAssumedInfo:=bIsAssumedInfo)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="r_oDataSet.SetPropertyValue Failed for " & v_sPropertyName, vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="IndexLinkProperty")
                    Return result
                End If
            End If

        Next iCnt

        Return result

    End Function
End Class
