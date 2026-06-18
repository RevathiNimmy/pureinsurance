Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Xml
Imports SSP.Shared


<System.Runtime.InteropServices.ProgId("STS_NET.STS")>
Public NotInheritable Class STS
    Implements IDisposable
    Private Const ACClass As String = "STS"

    Private Const ACGISSchemeEDILinkSTSStored As Boolean = True
    Private Const ACGISSchemeEDILinkSTSName As String = "GISSchemeEDILinkSTS"
    Private Const ACGISSchemeEDILinkSTSSQL As String = "spu_GIS_Scheme_EDI_Link_STS_sel"
    Private Const ACGetpasswordhistorySQL As String = "spu_sir_passwordhistory_sel"
    Private Const ACGetpasswordhistoryName As String = "Getpasswordhistory"
    Private Const ACGetpasswordhistoryStored As Boolean = True
    Private Const ACIsValidPasswordForReUseStored As Boolean = True
    Private Const ACIsValidPasswordForReUseName As String = "GetSystemOption"
    Private Const ACIsValidPasswordForReUseSQL As String = "Spu_SIR_IsValidPasswordForReUse"
    Private Const CNCalledFromSTS As String = "CalledFromSTS"
    Private Const ACLeadAgent As String = "PARTY_LEAD_AGENT_CNT"

    Private Const SAMContactTypeEmail As Byte = 0
    Private Const SAMContactTypeHomePhone As Byte = 1
    Private Const SAMContactTypeMobile As Byte = 2
    Private Const SAMContactTypeFax As Byte = 3
    Private Const SAMContactTypeWeb As Byte = 4
    Private Const SAMContactTypeMain As Byte = 5

    Private Const SAMAddressTypeHome As Byte = 0
    Private Const SAMAddressTypeBusiness As Byte = 1
    Private Const SAMAddressTypeOther As Byte = 2
    Private Const SAMAddressTypeSubAgent As Byte = 3
    Private Const SAMAddressTypeBranch As Byte = 4
    Private Const SAMAddressTypeBilling As Byte = 5
    Private Const SAMAddressTypeCorrespondence As Byte = 6
    Private Const SAMAddressTypePrevious As Byte = 7
    Private Const SAMAddressTypeRegistered As Byte = 8
    Private Const SAMAddressTypeSite As Byte = 9

    Private Const ACInsurance_file_cnt As Integer = 0
    Private Const ACPolicy_version As Integer = 1
    Private Const ACLast_trans_date As Integer = 2
    Private Const ACCode As Integer = 3
    Private Const ACCover_start_date As Integer = 4
    Private Const ACExpiry_date As Integer = 5
    Private Const ACRenewal_date As Integer = 6
    Private Const ACInsurance_ref As Integer = 7
    Private Const ACLast_trans_description As Integer = 8
    Private Const ACDate_created As Integer = 9
    Private Const ACResolved_name As Integer = 10
    Private Const ACInsured_name As Integer = 11
    Private Const ACProduct_id As Integer = 12
    Private Const ACInsurance_folder_cnt As Integer = 13
    Private Const ACDescription As Integer = 15
    Private Const ACInsuredCnt As Integer = 16
    Private Const ACQuoteExpiryDate As Integer = 17
    Private Const ACSubBranchCode As Integer = 18
    Private Const ACLeadAllowConsolidatedCommission As Integer = 19
    Private Const ACSubAllowConsolidatedCommission As Integer = 20
    Const ACProcessed As String = "Processed"

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private m_bCloseDatabase As Boolean
    Private m_lReturn As Integer

    Private m_lSTSErrorReturnValue As gPMConstants.PMEReturnCode
    Private m_sSTSErrorDescription As String = ""

    Private m_oListManager As bGISListManager.InterfaceNoLogin
    Private m_oDatabase As dPMDAO.Database
    Private Const ACEmailDocType As Integer = 8
    Private m_oResolvedDocumentList As List(Of String)
    Private m_oSplitDocMergedCodes As List(Of String)

    Public ReadOnly Property ResolvedDocumentList() As List(Of String)
        Get
            Return m_oResolvedDocumentList
        End Get
    End Property

    Public ReadOnly Property SplitDocMergedCodes() As List(Of String)
        Get
            Return m_oSplitDocMergedCodes
        End Get
    End Property



    Public Function CheckTSAndLock(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByVal v_vTimestamp As Object, ByRef r_sCurrentlyLockedBy As String, ByRef r_bTimestampMatches As Boolean) As Integer
        Dim result As Integer = 0
        Dim oLock As bPMLock.User
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            r_sCurrentlyLockedBy = ""


            oLock = New bPMLock.User
            lReturn = oLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Check that the timestamp is still valid for this record
            ' and lock it for update. Use the userid that bGIS has been initialised with for the lock
            result = oLock.CheckTSAndLock(v_sKeyName:=v_sKeyName, v_lKeyValue:=v_lKeyValue, v_iUserID:=m_iUserID, v_vTimestamp:=v_vTimestamp, r_sCurrentlyLockedBy:=r_sCurrentlyLockedBy, r_bTimestampMatches:=r_bTimestampMatches)

            oLock.Dispose()
            oLock = Nothing

            Return result
        Catch excep As System.Exception
            ' Error.
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckTSAndLock Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckTSAndLock", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function DeleteRisk(ByVal v_lRiskCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_sTransactionType As String) As Integer
        Return DeleteRisk(v_lRiskCnt:=v_lRiskCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sTransactionType:=v_sTransactionType, bViaSAM:=False)
    End Function
    Public Function DeleteRisk(ByVal v_lRiskCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_sTransactionType As String, ByVal bViaSAM As Boolean) As Integer
        Dim result As Integer = 0
        Dim oRiskScreen As bSIRRiskScreen.Business
        Dim sRiskStatusCode As String = String.Empty
        Dim nReturn As Integer
        Dim sSQL As String = String.Empty
        Dim vResultDataset(,) As Object
        Dim oGISApplication As Application
        Dim oBusiness As bSirPerilAllocation.Business
        Dim nOriginalRiskCnt As Integer
        Dim sStatusFlag As String

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            sSQL = "spu_GetRiskStatusByRisk"
            m_oDatabase.Parameters.Clear()

            ' lPolicyBinderID
            nReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add parameter 'Risk_Cnt' to m_oDatabase, value = " & v_lRiskCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk")
                Return result
            End If

            ' ProductCode
            nReturn = m_oDatabase.Parameters.Add(sName:="Risk_Status_Code", vValue:=sRiskStatusCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add parameter 'Risk_Status_Code' to m_oDatabase", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk")
                Return result
            End If

            ' Call the SQL
            nReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectRiskSummariesByKey", bStoredProcedure:=True, vResultArray:=vResultDataset)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQLAction failed to Delete Risk for Risk Cnt - " & v_lRiskCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk")
                Return result
            End If

            If m_oDatabase.Parameters.Item("Risk_Status_Code") IsNot Nothing Then
                sRiskStatusCode = m_oDatabase.Parameters.Item("Risk_Status_Code").Value.ToString.TrimEnd
            End If
            vResultDataset = Nothing
            sSQL = ""
            sSQL = "spu_GetRiskStatusFlag"

            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add parameter 'Risk_Cnt' to m_oDatabase, value = " & v_lRiskCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk")
                Return result
            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="OriginalRiskCnt", vValue:=CStr(nOriginalRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add parameter 'OriginalRiskCnt' to m_oDatabase, value = " & nOriginalRiskCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk")
                Return result
            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="StatusFlag", vValue:=sStatusFlag, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add parameter 'StatusFlag' to m_oDatabase, value = " & sStatusFlag, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk")
                Return result
            End If

            ' Call the SQL
            nReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRiskStatusFlag", bStoredProcedure:=True, vResultArray:=vResultDataset)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQLAction failed to Delete Risk for Risk Cnt - " & v_lRiskCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk")
                Return result
            End If

            If m_oDatabase.Parameters.Item("StatusFlag") IsNot Nothing Then
                sStatusFlag = m_oDatabase.Parameters.Item("StatusFlag").Value.ToString.Trim
            End If
            If m_oDatabase.Parameters.Item("OriginalRiskCnt") IsNot Nothing Then
                nOriginalRiskCnt = ToSafeLong(m_oDatabase.Parameters.Item("OriginalRiskCnt").Value)
            End If

            If sRiskStatusCode = "" Or (sStatusFlag <> "U" And nOriginalRiskCnt = 0) Then

                oRiskScreen = New bSIRRiskScreen.Business
                nReturn = oRiskScreen.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nReturn
                End If

                oRiskScreen.RiskId = v_lRiskCnt

                nReturn = oRiskScreen.DeleteRiskCancelledOnAdd()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRRiskScreen.GetDetails failed to delete the following risk :- " & v_lRiskCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk")
                    Return result
                End If
                oRiskScreen.Dispose()
                ' Remove and we out...
                oRiskScreen = Nothing
            Else

                nReturn = DeleteRiskLink(lInsuranceFileCnt:=v_lInsuranceFileCnt, lRiskID:=v_lRiskCnt)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRiskLink failed to delete the following risk :- " & v_lRiskCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk")
                    Return result
                End If

                If v_lRiskCnt <> 0 Then
                    ' Create bSIRPerilAllocation object

                    oBusiness = New bSirPerilAllocation.Business
                    nReturn = oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = nReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRisk Failed to Initialise bSIRPerilAllocation.Business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    oBusiness.InsuranceFolderCnt = v_lInsuranceFolderCnt
                    oBusiness.InsuranceFileCnt = v_lInsuranceFileCnt
                    oBusiness.RiskID = v_lRiskCnt
                    oBusiness.TransactionType = v_sTransactionType

                    nReturn = oBusiness.PopulateRatingSections()
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRiskLink failed to call PopulateRatingSections for the following risk :- " & v_lRiskCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk")
                        Return result
                    End If

                    oGISApplication = New Application()

                    nReturn = oGISApplication.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = nReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRisk Failed to Initialise bGIS.Application object.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    nReturn = oGISApplication.UpdateRiskAfter(v_lInsuranceFileCnt, v_lInsuranceFolderCnt, v_lRiskCnt, v_sTransactionType, v_bViaSAM:=bViaSAM)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRiskLink failed to call UpdateRiskAfter for the following risk :- " & v_lRiskCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk")
                        Return result
                    End If
                End If
            End If

            oBusiness = Nothing
            oGISApplication = Nothing
            Return result
        Catch excep As System.Exception
            ' Error.
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ''' <summary>
    ''' Checks the timestamp for the supplied key  and locks the entity if it matches.
    ''' </summary>
    ''' <param name="v_sKeyName"></param>
    ''' <param name="v_lKeyValue"></param>
    ''' <param name="r_vTimestamp"></param>
    ''' <param name="r_bCurrentlyLocked"></param>
    ''' <param name="r_iLockedByUserID"></param>
    ''' <param name="r_sLockedByUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLastUnlockTimestamp(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByRef r_vTimestamp As Object, ByRef r_bCurrentlyLocked As Object, ByRef r_iLockedByUserID As Object, ByRef r_sLockedByUser As Object) As Integer
        Dim nResult As Integer = 0
        Dim oLock As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            oLock = gPMFunctions.CreateLateBoundObject("bPMLock.User")
            Dim oDatabase As Object = m_oDatabase
            oLock.Initialise(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=gPMFunctions.ToSafeString(ACApp), vDatabase:=oDatabase)
            m_oDatabase = oDatabase

            ' Get the timestamp for this record
            nResult = oLock.GetLastUnlockTimestamp(v_sKeyName:=gPMFunctions.ToSafeString(v_sKeyName), v_lKeyValue:=gPMFunctions.ToSafeInteger(v_lKeyValue), r_vTimestamp:=r_vTimestamp,
                                                   r_bCurrentlyLocked:=r_bCurrentlyLocked, r_iLockedByUserId:=r_iLockedByUserID, r_sLockedByUser:=r_sLockedByUser)
            oLock.Dispose()

            oLock = Nothing

            Return nResult
        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLastUnlockTimestamp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLastUnlockTimestamp", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    Public Function GetList(ByVal v_sPropertyId As String, ByRef r_vListData As Object) As Integer
        Return GetList(v_sPropertyId:=v_sPropertyId, r_vListData:=r_vListData, v_vSearchString:=Nothing)
    End Function
    Public Function GetList(ByVal v_sPropertyId As String, ByRef r_vListData As Object, ByVal v_vSearchString As Object) As Integer
        Dim result As Integer = 0
        Dim vListData, vListDataCodes As Object
        Dim lRow As Integer

        Try
            ' Pass the call onto iGISListManager
            result = m_oListManager.GetListAndCodes(v_sPropertyId:=v_sPropertyId, r_vListData:=vListData, r_vListDataCode:=vListDataCodes, v_vSearchString:=v_vSearchString)
            ' How many list items do we have
            lRow = vListData.GetUpperBound(0)
            ' resize the output array
            ReDim r_vListData(1, lRow)

            ' Merge the two arrays into one
            For lRow = vListData.GetLowerBound(0) To vListData.GetUpperBound(0)
                r_vListData(0, lRow) = vListData(lRow)
                r_vListData(1, lRow) = vListDataCodes(lRow)
            Next lRow
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnlockAndGetTS (Public)
    '
    ' Description: Checks the timestamp for the supplied key
    '              and locks the entity if it matches.
    ' ***************************************************************** '
    Public Function UnlockAndGetTS(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByRef r_vTimestamp As Object) As Integer
        Dim result As Integer = 0
        Dim oLock As bPMLock.User
        Dim lReturn As gPMConstants.PMEReturnCode


        Try
            result = gPMConstants.PMEReturnCode.PMTrue


            oLock = New bPMLock.User
            lReturn = oLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Unlock the record and return the timestamp for this record.
            ' Use the userid that bGIS has been initialised with for the lock
            result = oLock.UnlockAndGetTS(v_sKeyName:=v_sKeyName, v_lKeyValue:=v_lKeyValue, v_iUserID:=m_iUserID, r_vTimestamp:=r_vTimestamp)

            oLock.Dispose()
            oLock = Nothing
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockAndGetTS Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockAndGetTS", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public Property STSErrorReturnValue() As Integer
        Get
            Return m_lSTSErrorReturnValue
        End Get
        Set(ByVal Value As Integer)
            m_lSTSErrorReturnValue = Value
        End Set
    End Property

    Public Property STSErrorDescription() As String
        Get
            Return m_sSTSErrorDescription
        End Get
        Set(ByVal Value As String)
            m_sSTSErrorDescription = Value
        End Set
    End Property

    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

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

            m_lReturn = gPMComponentServices.CheckDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                m_oListManager = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function InitialiseListManager(ByVal v_sDataModel As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the iGISListManager object
            m_oListManager = New bGISListManager.InterfaceNoLogin()

            ' Initialise the object
            lReturn = m_oListManager.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load the lookups and make sure we have the right versions
            lReturn = CType(m_oListManager.CheckListVersions(v_sDataModel), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oListManager.MaxListItems = 600000 'current motor list is 400000
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseListManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseListManager", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetDescriptionFromABICode(ByVal v_sPropertyId As String, ByVal v_sABICode As String, ByRef r_sDescription As String) As Integer
        Dim result As Integer = 0
        Try
            ' Pass the call onto iGISListManager
            Return m_oListManager.GetDescriptionFromABICode(v_sPropertyId:=v_sPropertyId, v_sABICode:=v_sABICode, r_sDescription:=r_sDescription)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDescriptionFromABICode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDescriptionFromABICode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function TerminateListManager() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Terminate the list manager object
            m_oListManager.Dispose()
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TerminateListManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TerminateListManager", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function AddMTAQuote(ByVal v_sMTAType As String, ByVal v_lNewMessageVersion As Integer, ByVal v_sGisDataModelCode As String, ByVal v_dtEffectiveDate As Date, ByRef r_lInsuranceFileCnt As Object, ByRef r_lInsuranceFolderCnt As Object, ByVal v_cThisPremium As Decimal, ByVal v_cNetPremium As Decimal, ByVal v_cTaxAmount As Decimal, ByVal v_cTaxRate As Decimal, Optional ByRef r_lRiskFolderCnt As Object = 0, Optional ByVal v_dtCoverEndDate As Date = #12/30/1899#, Optional ByVal v_vInsuranceRef As Object = Nothing, Optional ByVal v_vAlternateReference As Object = Nothing, Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oBom As Object

            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:="Application", r_oBOM:=oBom, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddMTAQuote Failed to Create BOM", vApp:=ACApp, vClass:=ACClass, vMethod:="AddMTAQuote")
                Return result
            End If

            If Not (oBom Is Nothing) Then
                m_lReturn = oBom.AddMTAQuote(v_sMTAType:=gPMFunctions.ToSafeString(v_sMTAType), v_lNewMessageVersion:=gPMFunctions.ToSafeInteger(v_lNewMessageVersion), v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), r_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_lInsuranceFolderCnt:=r_lInsuranceFolderCnt, v_cThisPremium:=gPMFunctions.ToSafeDecimal(v_cThisPremium), v_cNetPremium:=gPMFunctions.ToSafeDecimal(v_cNetPremium), v_cTaxAmount:=gPMFunctions.ToSafeDecimal(v_cTaxAmount), v_cTaxRate:=gPMFunctions.ToSafeDecimal(v_cTaxRate), v_vInsuranceRef:=v_vInsuranceRef, r_lRiskFolderCnt:=r_lRiskFolderCnt, v_dtCoverEndDate:=gPMFunctions.ToSafeDate(v_dtCoverEndDate), v_vAlternateReference:=v_vAlternateReference,
                                             r_vAdditionalDataArray:=r_vAdditionalDataArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BOM AddMTAQuote Method Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddQuote")
                    Return result
                End If

                oBom.Dispose()
                oBom = Nothing
            End If
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddMTAQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddMTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    ''' <summary>
    ''' Used to add the party.
    ''' </summary>
    ''' <param name="sPartyTypeCode"></param>
    ''' <param name="sBranchCode"></param>
    ''' <param name="vAddresses"></param>
    ''' <param name="sTPUserCode"></param>
    ''' <param name="sTPIntroducer"></param>
    ''' <param name="vContacts"></param>
    ''' <param name="sSurname"></param>
    ''' <param name="sForename"></param>
    ''' <param name="sDateOfBirth"></param>
    ''' <param name="sTitle"></param>
    ''' <param name="sMaritalStatusCode"></param>
    ''' <param name="sGenderCode"></param>
    ''' <param name="sInitials"></param>
    ''' <param name="sOccupationCode"></param>
    ''' <param name="sEmployerBusinessCode"></param>
    ''' <param name="sEmploymentStatusCode"></param>
    ''' <param name="sAlternativeID"></param>
    ''' <param name="sCompanyName"></param>
    ''' <param name="sTradingName"></param>
    ''' <param name="sBusinessCode"></param>
    ''' <param name="lAgentCnt"></param>
    ''' <param name="r_lPartyCnt"></param>
    ''' <param name="r_sPartyCode"></param>
    ''' <param name="sMainContact"></param>
    ''' <param name="r_sTaxNumber"></param>
    ''' <param name="r_bDomiciledForTax"></param>
    ''' <param name="r_bTaxExempt"></param>
    ''' <param name="r_lPercentage"></param>
    ''' <param name="r_oConvictions"></param>
    ''' <param name="r_oAccidents"></param>
    ''' <param name="r_oOtherPartyInfo"></param>
    ''' <param name="vAddressContacts"></param>
    ''' <param name="r_oSuppBusiness"></param>
    ''' <param name="v_sFileCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddParty(ByVal sPartyTypeCode As String, ByVal sBranchCode As String, ByVal vAddresses(,) As Object,
                             ByVal sTPUserCode As String, ByVal sTPIntroducer As String, ByVal vContacts(,) As Object,
                             ByVal sSurname As String, ByVal sForename As String, ByVal sDateOfBirth As String,
                             ByVal sTitle As Object, ByVal sMaritalStatusCode As Object,
                             ByVal sGenderCode As Object, ByVal sInitials As Object,
                             ByVal sOccupationCode As String, ByVal sEmployerBusinessCode As String,
                             ByVal sEmploymentStatusCode As String, ByVal sAlternativeID As String,
                             ByVal sCompanyName As String, ByVal sTradingName As String,
                             ByVal sBusinessCode As String, ByVal lAgentCnt As Integer,
                             ByRef r_lPartyCnt As Integer, ByRef r_sPartyCode As String,
                             Optional ByVal sMainContact As String = "", Optional ByVal r_sTaxNumber As String = "",
                             Optional ByVal r_bDomiciledForTax As Integer = 0,
                             Optional ByVal r_bTaxExempt As Integer = 0,
                             Optional ByVal r_lPercentage As Integer = 0,
                             Optional ByVal r_oConvictions As Object = Nothing, Optional ByVal r_oAccidents As Object = Nothing,
                             Optional ByVal r_oOtherPartyInfo As Object = Nothing,
                             Optional ByVal vAddressContacts(,) As Object = Nothing,
                             Optional ByVal r_oSuppBusiness As Object = Nothing, Optional ByVal v_sFileCode As String = "") As Integer

        Dim nResult As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim nLBnd As Integer
        Dim nUBnd As Integer
        Dim iCount As Integer
        Dim sEmailAddress As String
        Dim sTelephoneNumber As String
        Dim sAddress1 As String
        Dim sAddress2 As String
        Dim sAddress3 As String
        Dim sAddress4 As String
        Dim sCountryCode As String
        Dim sPostcode As String
        Dim sContactDesc As String
        Dim sAddress5 As String
        Dim sAddress6 As String
        Dim sAddress7 As String
        Dim sAddress8 As String
        Dim sAddress9 As String
        Dim sAddress10 As String
        Dim aoAdditionalDataArray(,) As Object
        Dim aoUpdateAddressArray(,) As Object
        Dim aoUpdateContactArray() As Object
        Dim oGisQuotePolicy As QuotePolicy
        Dim oParty As bSIRParty.Business

        Dim oContact As bSIRContact.Business
        Dim oAddress As bSIRAddress.Business
        Dim lSourceID As Integer
        Dim aoUpdateAddContact(,) As Object
        Const kCSCAContactTypeCode As Byte = 0
        Const kCSCAContactTypeID As Byte = 1
        Const kCSCAContactDetail As Byte = 2
        Const kCSCAAreaCode As Byte = 3
        Const kCSCAExtension As Byte = 6
        Const kCSCADescription As Byte = 5

        ' SAM Address array position constants
        Const kCSAAAddressTypeCode As Byte = 0
        Const kCSAAAddressTypeID As Byte = 1
        Const kCSAAAddress1 As Byte = 2
        Const kCSAAAddress2 As Byte = 3
        Const kCSAAAddress3 As Byte = 4
        Const kCSAAAddress4 As Byte = 5
        Const kCSAACountryCode As Byte = 6
        Const kCSAAPostCode As Byte = 7
        Const kCSAACountryID As Byte = 8
        Const kACSAAAddress5 As Byte = 9
        Const kACSAAAddress6 As Byte = 10
        Const kACSAAAddress7 As Byte = 11
        Const kACSAAAddress8 As Byte = 12
        Const kACSAAAddress9 As Byte = 13
        Const kACSAAAddress10 As Byte = 14



        Const kCProcessed As String = "Processed"

        Const kCAddPartyErrNum As Integer = Constants.vbObjectError + 1
        Const kCAddPartyErrSrc As String = "bGIS.STS.AddParty"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Move company name if CC
            If sPartyTypeCode = "CC" Then
                sSurname = sCompanyName
                sForename = sCompanyName
            End If

            'Initialise GISQuotePolicy object
            oGisQuotePolicy = New QuotePolicy()
            m_lReturn = oGisQuotePolicy.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Failed to initialise QuotePolicy object. Return code " & m_lReturn)
            End If

            ' Create an instance of the bSIRContact object

            oContact = New bSIRContact.Business
            m_lReturn = oContact.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                            iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Failed to create bSIRContact.Business object. Return code " & m_lReturn)
            End If

            ' Create an instance of the bSIRAddress object

            oAddress = New bSIRAddress.Business
            m_lReturn = oAddress.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Failed to create bSIRAddress.Business object. Return code " & m_lReturn)
            End If

            ' Create an instance of the bSIRParty object

            'oParty = New bSIRParty.Business
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oParty, v_sClassName:="bSIRParty.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParty Failed - Failed to create business object to bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesByBranch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return m_lReturn
            End If
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Failed to create bSIRParty.Business object. Return code " & m_lReturn)
            End If

            ' Extract the first email address / telephone number (if there are any)
            ' from the contacts array
            If Informations.IsArray(vContacts) Then
                For iLoop As Integer = vContacts.GetLowerBound(1) To vContacts.GetUpperBound(1)
                    Select Case vContacts(kCSCAContactTypeCode, iLoop)
                        Case SAMContactTypeEmail
                            If sEmailAddress = "" Then
                                sEmailAddress = CStr(vContacts(kCSCAContactDetail, iLoop)).Trim()
                                vContacts(kCSCAContactTypeCode, iLoop) = kCProcessed
                            End If
                        Case Else
                    End Select
                Next
            End If

            ' Extract the first correspondence address (if any exist) from the
            ' address array
            If Informations.IsArray(vAddresses) Then
                For iLoop As Integer = vAddresses.GetLowerBound(1) To vAddresses.GetUpperBound(1)

                    If CDbl(vAddresses(kCSAAAddressTypeCode, iLoop)) = SAMAddressTypeCorrespondence Then
                        sAddress1 = CStr(vAddresses(kCSAAAddress1, iLoop))
                        sAddress2 = CStr(vAddresses(kCSAAAddress2, iLoop))
                        sAddress3 = CStr(vAddresses(kCSAAAddress3, iLoop))
                        sAddress4 = CStr(vAddresses(kCSAAAddress4, iLoop))
                        sCountryCode = CStr(vAddresses(kCSAACountryCode, iLoop))
                        sPostcode = CStr(vAddresses(kCSAAPostCode, iLoop))
                        vAddresses(kCSAAAddressTypeCode, iLoop) = kCProcessed
                        sAddress5 = CStr(vAddresses(kACSAAAddress5, iLoop))
                        sAddress6 = CStr(vAddresses(kACSAAAddress6, iLoop))
                        sAddress7 = CStr(vAddresses(kACSAAAddress7, iLoop))
                        sAddress8 = CStr(vAddresses(kACSAAAddress8, iLoop))
                        sAddress9 = CStr(vAddresses(kACSAAAddress9, iLoop))
                        sAddress10 = CStr(vAddresses(kACSAAAddress10, iLoop))
                        Exit For
                    End If
                Next
            End If

            lSourceID = -1
            If sBranchCode <> "" Then
                m_lReturn = GetPMLookupIdFromCode("Source", sBranchCode, lSourceID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Failed to retrieve Source ID for Branch code - " & sBranchCode & ".  The following errorcode was returned - " & CStr(m_lReturn))
                End If
            End If

            ' Build the Additional Data array
            ReDim aoAdditionalDataArray(1, 8)

            aoAdditionalDataArray(0, 0) = "TP_USER_CODE"
            aoAdditionalDataArray(1, 0) = sTPUserCode
            aoAdditionalDataArray(0, 1) = "TP_INTRODUCER"
            aoAdditionalDataArray(1, 1) = sTPIntroducer
            aoAdditionalDataArray(0, 2) = "AddressCountry"
            aoAdditionalDataArray(1, 2) = sCountryCode
            aoAdditionalDataArray(0, 3) = "OCCUPATION_CODE"
            aoAdditionalDataArray(1, 3) = sOccupationCode
            aoAdditionalDataArray(0, 4) = "EMPLOYER_BUSINESS_CODE"
            aoAdditionalDataArray(1, 4) = sEmployerBusinessCode
            aoAdditionalDataArray(0, 5) = "EMPLOYMENT_STATUS_CODE"
            aoAdditionalDataArray(1, 5) = sEmploymentStatusCode
            aoAdditionalDataArray(0, 6) = "ALTERNATIVE_ID"
            aoAdditionalDataArray(1, 6) = sAlternativeID
            aoAdditionalDataArray(0, 7) = "BUSINESS_CODE"
            aoAdditionalDataArray(1, 7) = sBusinessCode
            aoAdditionalDataArray(0, 8) = CNCalledFromSTS
            aoAdditionalDataArray(1, 8) = 1

            ' If the agent cnt is present then use it
            If lAgentCnt > 0 Then
                nUBnd = aoAdditionalDataArray.GetUpperBound(1) + 1
                ReDim Preserve aoAdditionalDataArray(1, nUBnd)

                aoAdditionalDataArray(0, nUBnd) = ACLeadAgent
                aoAdditionalDataArray(1, nUBnd) = lAgentCnt
            End If

            If lSourceID > 0 Then
                nUBnd = aoAdditionalDataArray.GetUpperBound(1) + 1
                ReDim Preserve aoAdditionalDataArray(1, nUBnd)

                aoAdditionalDataArray(0, nUBnd) = "SOURCE_ID"
                aoAdditionalDataArray(1, nUBnd) = lSourceID
            End If


            If Not (String.IsNullOrEmpty(sMainContact) Or Convert.IsDBNull(sMainContact) Or Informations.IsNothing(sMainContact)) _
                Then
                If sMainContact <> "" Then
                    nUBnd = aoAdditionalDataArray.GetUpperBound(1) + 1
                    ReDim Preserve aoAdditionalDataArray(1, nUBnd)

                    aoAdditionalDataArray(0, nUBnd) = "CONTACT_NAME"
                    aoAdditionalDataArray(1, nUBnd) = sMainContact
                End If
            End If

            If v_sFileCode <> "" Then
                nUBnd = aoAdditionalDataArray.GetUpperBound(1) + 1
                ReDim Preserve aoAdditionalDataArray(1, nUBnd)

                aoAdditionalDataArray(0, nUBnd) = "FILECODE"
                aoAdditionalDataArray(1, nUBnd) = v_sFileCode
            End If

            ' Use the existing AddParty on QuotePolicy to Add the Party and first
            ' address/contact

            m_lReturn = oGisQuotePolicy.AddParty(v_sGisDataModelCode:="AOL", v_sGisBusinessTypeCode:="NB", v_sPartyTypeCode:=sPartyTypeCode,
                                                 v_sForename:=sForename, v_sSurname:=sSurname, v_sDateOfBirth:=sDateOfBirth,
                                                 v_sEmailAddress:=sEmailAddress, v_sCurrentRenewalDate:=DateTime.Now, v_sAddress1:=sAddress1,
                                                 v_sAddress2:=sAddress2, v_sAddress3:=sAddress3, v_sAddress4:=sAddress4, v_sPostcode:=sPostcode,
                                                 r_lPartyCnt:=r_lPartyCnt, v_sTitle:=sTitle, v_sMaritalStatusCode:=sMaritalStatusCode,
                                                 v_sGenderCode:=sGenderCode, v_sInitials:=sInitials, v_sTelephoneNumber:=sTelephoneNumber,
                                                 v_sTradingName:=sTradingName, r_vAdditionalDataArray:=aoAdditionalDataArray,
                                                 sAddress5:=sAddress5,
                                                 sAddress6:=sAddress6, sAddress7:=sAddress7,
                                                 sAddress8:=sAddress8, sAddress9:=sAddress9,
                                                 sAddress10:=sAddress10)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Call to bGIS.QuotePolicy.AddParty failed. Return code " & m_lReturn)
            End If

            ' Find Party Shortcode
            If Informations.IsArray(aoAdditionalDataArray) Then
                nLBnd = aoAdditionalDataArray.GetLowerBound(1)
                nUBnd = aoAdditionalDataArray.GetUpperBound(1)
                For iLoop As Integer = nLBnd To nUBnd

                    If CStr(aoAdditionalDataArray(0, iLoop)) = "PARTY_CODE" Then

                        r_sPartyCode = CStr(aoAdditionalDataArray(1, iLoop))
                        Exit For
                    End If
                Next iLoop
            End If

            ' Add extra addresses
            iCount = 0
            Dim iCountContacts As Integer
            If Informations.IsArray(vAddresses) Then
                For iLoop As Integer = vAddresses.GetLowerBound(1) To vAddresses.GetUpperBound(1)

                    If CStr(vAddresses(kCSAAAddressTypeCode, iLoop)) <> kCProcessed Then

                        m_lReturn = oAddress.DirectAdd(vAddress1:=vAddresses(kCSAAAddress1, iLoop), vAddress2:=vAddresses(kCSAAAddress2, iLoop),
                                                       vAddress3:=vAddresses(kCSAAAddress3, iLoop), vAddress4:=vAddresses(kCSAAAddress4, iLoop),
                                                       vPostalCode:=vAddresses(kCSAAPostCode, iLoop), vCountryID:=vAddresses(kCSAACountryID, iLoop),
                                                       sAddress5:=vAddresses(kACSAAAddress5, iLoop),
                                                       sAddress6:=vAddresses(kACSAAAddress6, iLoop),
                                                       sAddress7:=vAddresses(kACSAAAddress7, iLoop),
                                                       sAddress8:=vAddresses(kACSAAAddress8, iLoop),
                                                       sAddress9:=vAddresses(kACSAAAddress9, iLoop),
                                                       sAddress10:=vAddresses(kACSAAAddress10, iLoop))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Call to bSIRAddress.Business.DirectAdd failed. Return code " & m_lReturn)
                        End If
                        ReDim Preserve aoUpdateAddressArray(1, iCount)
                        aoUpdateAddressArray(0, iCount) = oAddress.AddressCnt
                        aoUpdateAddressArray(1, iCount) = vAddresses(kCSAAAddressTypeID, iLoop)
                        iCount += 1

                        If Informations.IsArray(vAddressContacts) Then
                            For icontacts As Integer = vAddressContacts.GetLowerBound(0) To _
                                vAddressContacts.GetUpperBound(0)

                                If CDbl(CDbl(vAddressContacts(4, icontacts)) - 1) = iLoop Then
                                    sContactDesc = CStr(vAddressContacts(kCSCADescription, iLoop)) ' Vivek: 20080704 - added the missing item
                                    m_lReturn = oContact.DirectAdd(vContactTypeID:=vAddressContacts(kCSCAContactTypeID, iLoop), vDescription:=sContactDesc, vAreaCode:=vAddressContacts(kCSCAAreaCode, iLoop), vNumber:=vAddressContacts(kCSCAContactDetail, iLoop))
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Call to bSIRContact.Business.DirectAdd failed. Return code " & m_lReturn)
                                    End If
                                    aoUpdateAddContact(iCountContacts, 0) = oContact.ContactCnt
                                    aoUpdateAddContact(iCountContacts, 1) = oAddress.AddressCnt
                                    iCountContacts += 1
                                End If
                            Next
                        End If
                    End If
                Next
            End If
            'Add Address Contacts
            If iCountContacts > 0 Then

                m_lReturn = oParty.AddAddressContacts(vAddContacts:=aoUpdateAddContact)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Call to bSIRParty.Business.AddAddressContacts failed. Return code " & m_lReturn)
                End If
            End If
            ' Update the address usage
            If iCount > 0 Then

                m_lReturn = oParty.UpdateAddresses(vPartyCnt:=r_lPartyCnt, vAddAddresses:=aoUpdateAddressArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Call to bSIRParty.Business.UpdateAddresses failed. Return code " & m_lReturn)
                End If
            End If

            ' Process extra Contacts
            iCount = 0
            If Informations.IsArray(vContacts) Then
                For iLoop As Integer = vContacts.GetLowerBound(1) To vContacts.GetUpperBound(1)

                    If CStr(vContacts(kCSCAContactTypeCode, iLoop)) <> kCProcessed Then
                        sContactDesc = CStr(vContacts(kCSCADescription, iLoop)) ' Vivek: 20080704 - added the missing item
                        m_lReturn = oContact.DirectAdd(vContactTypeID:=vContacts(kCSCAContactTypeID, iLoop), vDescription:=sContactDesc, vAreaCode:=vContacts(kCSCAAreaCode, iLoop), vNumber:=vContacts(kCSCAContactDetail, iLoop), vExtension:=vContacts(kCSCAExtension, iLoop))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Call to bSIRContact.Business.DirectAdd failed. Return code " & m_lReturn)
                        End If
                        ReDim Preserve aoUpdateContactArray(iCount)
                        aoUpdateContactArray(iCount) = oContact.ContactCnt
                        iCount += 1
                    End If
                Next
            End If

            ' Update the address usage
            If iCount > 0 Then

                m_lReturn = oParty.UpdateContacts(vPartyCnt:=r_lPartyCnt, vAddContacts:=aoUpdateContactArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Call to bSIRParty.Business.UpdateContacts failed. Return code " & m_lReturn)
                End If
            End If

            'Add Party Table for Tax Details
            m_lReturn = oParty.AddTaxDetails(r_lPartyCnt:=r_lPartyCnt, r_sTaxNumber:=r_sTaxNumber,
                                             r_bDomiciledForTax:=r_bDomiciledForTax, r_bTaxExempt:=r_bTaxExempt,
                                             r_lPercentage:=r_lPercentage)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Call to bSIRParty.Business.AddTaxDetails failed. Return code " & m_lReturn)
            End If

            '------------------------Other Party Changes---------------------------
            If sPartyTypeCode.StartsWith("OT") Then
                If Informations.IsArray(r_oAccidents) Then
                    m_lReturn = oParty.AddAccidents(r_oAccidents:=r_oAccidents, r_lPartyCnt:=r_lPartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Call to bSIRParty.Business.AddAccidents failed. Return code " & m_lReturn)
                    End If
                End If


                'Add Other party Convictions Details
                If Informations.IsArray(r_oConvictions) Then
                    m_lReturn = oParty.AddConvictions(r_oConvictions:=r_oConvictions, r_lPartyCnt:=r_lPartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Call to bSIRParty.Business.AddConvictions failed. Return code " & m_lReturn)
                    End If
                End If

                'Add Other party Info
                If Informations.IsArray(r_oOtherPartyInfo) Then
                    m_lReturn = oParty.AddOtherPartyInfo(r_oOtherPartyInfo:=r_oOtherPartyInfo, r_lPartyCnt:=r_lPartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Call to bSIRParty.Business.AddOtherPartyInfo failed. Return code " & m_lReturn)
                    End If
                End If

                'Update Party Table for Tax Details

                m_lReturn = oParty.AddTaxDetails(r_lPartyCnt:=r_lPartyCnt, r_sTaxNumber:=r_sTaxNumber,
                                                 r_bDomiciledForTax:=r_bDomiciledForTax, r_bTaxExempt:=r_bTaxExempt,
                                                 r_lPercentage:=r_lPercentage)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Call to bSIRParty.Business.AddTaxDetails failed. Return code " & m_lReturn)
                End If
                If Informations.IsArray(r_oSuppBusiness) Then

                    m_lReturn = oParty.AddPartySupplier(r_oPartySupplier:=r_oSuppBusiness, r_lPartyCnt:=r_lPartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(kCAddPartyErrNum.ToString() + ", " + kCAddPartyErrSrc + ", " + "Call to bSIRParty.Business.AddPartySupplier failed. Return code " & m_lReturn)
                    End If
                End If
            End If
        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParty", excep:=excep)
        Finally
            ' Destroy objects
            If (oGisQuotePolicy IsNot Nothing) Then
                oGisQuotePolicy.Dispose()
                oGisQuotePolicy = Nothing
            End If
            If (oAddress IsNot Nothing) Then
                oAddress.Dispose()
                oAddress = Nothing
            End If
            If (oParty IsNot Nothing) Then
                oParty.Dispose()
                oParty.Dispose()
            End If
            If (oContact IsNot Nothing) Then
                oContact.Dispose()
                oContact = Nothing
            End If
        End Try
        Return nResult
    End Function

    Public Function UpdateOtherPartyInfo(ByVal oOtherInfo() As Object) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oParty As bSIRParty.Business
        Const ACUpdatePartyErrNum As Integer = Constants.vbObjectError + 1
        Const ACUpdatePartyErrSrc As String = "bGIS.STS.UpdateParty"
        Dim m_lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the bSIRParty object

            oParty = New bSIRParty.Business

            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " + "Failed to create bSIRParty.Business object. Return code " & m_lReturn)
            End If

            Dim lPartyCnt As Integer
            Dim sLicenseTypeCode, sLicenseNumber As String
            Dim DateOfBirth As Date
            Dim sGender, sDriverStatusCode, sReferenceNo, sExternalId, sRegNumber As String
            Dim DatePassedTest As Date
            Dim sContactName, sContacttelNo, sInsurerName, sInsurerAdd1, sInsurerAdd2, sInsurerAdd3, sInsurerAdd4, sInsurerPostCode, sInsurerTelNo, sInsurerFaxNo, sInsurerContactName, sInsurerEmail, sInsurerNotes, sCompanyNotes As String
            Dim lActiveIndicator, lAfterHoursIndicator, lPriorityIndicator As Integer

            lPartyCnt = CInt(oOtherInfo(0))
            sLicenseTypeCode = CStr(oOtherInfo(1))
            sLicenseNumber = CStr(oOtherInfo(2))
            DateOfBirth = CDate(oOtherInfo(3))
            sGender = CStr(oOtherInfo(4))
            sDriverStatusCode = CStr(oOtherInfo(5))
            sReferenceNo = CStr(oOtherInfo(6))
            sExternalId = CStr(oOtherInfo(7))
            sRegNumber = CStr(oOtherInfo(8))
            DatePassedTest = CDate(oOtherInfo(9))
            sContactName = CStr(oOtherInfo(10))
            sContacttelNo = CStr(oOtherInfo(11))
            sInsurerName = CStr(oOtherInfo(12))
            sInsurerAdd1 = CStr(oOtherInfo(13))
            sInsurerAdd2 = CStr(oOtherInfo(14))
            sInsurerAdd3 = CStr(oOtherInfo(15))
            sInsurerAdd4 = CStr(oOtherInfo(16))
            sInsurerPostCode = CStr(oOtherInfo(17))
            sInsurerTelNo = CStr(oOtherInfo(18))
            sInsurerFaxNo = CStr(oOtherInfo(19))
            sInsurerContactName = CStr(oOtherInfo(20))
            sInsurerEmail = CStr(oOtherInfo(21))
            sInsurerNotes = CStr(oOtherInfo(22))
            sCompanyNotes = CStr(oOtherInfo(23))
            lActiveIndicator = CInt(oOtherInfo(24))
            lAfterHoursIndicator = CInt(oOtherInfo(25))
            lPriorityIndicator = CInt(oOtherInfo(26))

            m_lReturn = oParty.UpdateOtherpartyInfo(lPartyCnt:=lPartyCnt, sLicenseTypeCode:=sLicenseTypeCode, sLicenseNumber:=sLicenseNumber, DateOfBirth:=DateOfBirth, sGender:=sGender, sPartyStatus:=sDriverStatusCode, sReferenceNo:=sReferenceNo, sExternalId:=sExternalId, sRegNumber:=sRegNumber, DatePassedTest:=DatePassedTest, sContactName:=sContactName, sContacttelNo:=sContacttelNo, sInsurerName:=sInsurerName, sInsurerAdd1:=sInsurerAdd1, sInsurerAdd2:=sInsurerAdd2, sInsurerAdd3:=sInsurerAdd3, sInsurerAdd4:=sInsurerAdd4, sInsurerPostCode:=sInsurerPostCode, sInsurerTelNo:=sInsurerTelNo, sInsurerFaxNo:=sInsurerFaxNo, sInsurerContactName:=sInsurerContactName, sInsurerEmail:=sInsurerEmail, sInsurerNotes:=sInsurerNotes, sCompanyNotes:=sCompanyNotes, lActiveIndicator:=lActiveIndicator, lAfterHoursIndicator:=lAfterHoursIndicator, lPriorityIndicator:=lPriorityIndicator)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " + "Call to bSIRAddress.Business.UpdateOtherPartyInfo failed. Return code " & m_lReturn)
            End If
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateOtherPartyInfo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        Finally
            ' Destroy objects
            If oParty IsNot Nothing Then
                oParty.Dispose()
                oParty = Nothing
            End If
        End Try
        Return result
    End Function

    Public Function UpdatePartyAccident(ByVal oAccidents(,) As Object) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oParty As bSIRParty.Business
        Const ACUpdatePartyErrNum As Integer = ObjectError + 1
        Const ACUpdatePartyErrSrc As String = "bGIS.STS.UpdateParty"
        Dim m_lReturn As gPMConstants.PMEReturnCode

        Dim lPartyCnt As Integer
        Dim dDate As Date
        Dim sDescription As String = ""
        Dim lIsAtFault As Integer
        Dim lAccidentId As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            oParty = New bSIRParty.Business
            ' Create an instance of the bSIRParty object
            'm_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oParty, v_sClassName:="bSIRParty.Business", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database)), gPMConstants.PMEReturnCode)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Throw New System.Exception(ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " + "Failed to create bSIRParty.Business object. Return code " & m_lReturn)
            'End If
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " + "Failed to create bSIRParty.Business object. Return code " & m_lReturn)
            End If
            If Informations.IsArray(oAccidents) Then
                For cnt As Integer = 0 To oAccidents.GetUpperBound(0)
                    ' If Previous Accident Id is not 0 then update an existing accident details

                    If CDbl(oAccidents(cnt, 4)) <> 0 Then
                        lPartyCnt = CInt(oAccidents(cnt, 0))
                        dDate = CDate(oAccidents(cnt, 1))
                        sDescription = CStr(oAccidents(cnt, 2))
                        lIsAtFault = CInt(oAccidents(cnt, 3))
                        lAccidentId = CInt(oAccidents(cnt, 4))

                        m_lReturn = oParty.UpdatePartyAccident(lParty_cnt:=lPartyCnt, lPrevious_accidents_id:=lAccidentId, dDate:=dDate, sDescription:=sDescription, lIs_at_fault:=lIsAtFault)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " + "Call to bSIRParty.Business.UpdatePartyAccident failed. Return code " & m_lReturn)
                        End If
                        ' If Previous Accident Id stands 0 then Add a new Accident
                    ElseIf CDbl(oAccidents(cnt, 4)) = 0 Then
                        ' Make a call to AddAccidents to add a new accident

                        m_lReturn = oParty.AddAccidents(oAccidents, oAccidents(cnt, 0))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " + "Call to bSIRParty.Business.AddPartyAccident failed. Return code " & m_lReturn)
                        End If
                    End If
                Next
            End If
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartyAccident Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        Finally
            ' Destroy objects
            If Not (oParty Is Nothing) Then
                oParty.Dispose()
                oParty = Nothing
            End If
        End Try
        Return result
    End Function

    Public Function UpdatePartyConviction(ByVal oConvictions(,) As Object) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oParty As bSIRParty.Business
        Const ACUpdatePartyErrNum As Integer = Constants.vbObjectError + 1
        Const ACUpdatePartyErrSrc As String = "bGIS.STS.UpdateParty"
        Dim m_lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Create an instance of the bSIRParty object

            oParty = New bSIRParty.Business
            'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oParty, v_sClassName:="bSIRParty.Business", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    ' Log Error Message
            '    bPMFunc.LogMessage(sUsername:=gpmfunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateOtherPartyInfo Failed - Failed to create business object to bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesByBranch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            '    Return m_lReturn
            'End If
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " + "Failed to create bSIRParty.Business object. Return code " & m_lReturn)
            End If

            Dim lPartyCnt As Integer
            Dim sTypeCode, sStatusCode, sDescription As String
            Dim dFineAmount As Decimal
            Dim ConvDate As Date
            Dim sSentenceTypeCode, sSentenceDescription As String
            Dim dSentenceDuration As Decimal
            Dim sSentenceDurationQualifier As String = ""
            Dim SentenceEffectiveDate As Date
            Dim dAlcoholLevel As Decimal
            Dim sAlcoholMeasurementMethod As String = ""
            Dim DrivingLicencePenaltyPoints As Decimal
            Dim lConvictionId As Integer

            If Informations.IsArray(oConvictions) Then
                For cnt As Integer = 0 To oConvictions.GetUpperBound(0)
                    'if oconviction(cnt)

                    lConvictionId = CInt(oConvictions(cnt, 14))
                    If lConvictionId <> 0 Then
                        sTypeCode = CStr(oConvictions(cnt, 1))
                        sStatusCode = CStr(oConvictions(cnt, 2))
                        sDescription = CStr(oConvictions(cnt, 3))
                        dFineAmount = CDec(oConvictions(cnt, 4))
                        ConvDate = CDate(oConvictions(cnt, 5))
                        sSentenceTypeCode = CStr(oConvictions(cnt, 6))
                        sSentenceDescription = CStr(oConvictions(cnt, 7))
                        dSentenceDuration = CDec(oConvictions(cnt, 8))
                        sSentenceDurationQualifier = CStr(oConvictions(cnt, 9))
                        SentenceEffectiveDate = CDate(oConvictions(cnt, 10))
                        dAlcoholLevel = CDec(oConvictions(cnt, 11))
                        sAlcoholMeasurementMethod = CStr(oConvictions(cnt, 12))
                        DrivingLicencePenaltyPoints = CDec(oConvictions(cnt, 13))

                        m_lReturn = oParty.UpdatePartyConviction(lParty_cnt:=lPartyCnt, lParty_conviction_id:=lConvictionId, sCode:=sTypeCode, sconviction_date:=ConvDate, sDescription:=sDescription, dfine_amt:=dFineAmount, sSentence_code:=sSentenceTypeCode, sSentence_description:=sSentenceDescription, dSentence_duration:=dSentenceDuration, sSentence_duration_qualifier:=sSentenceDurationQualifier, sSentence_effective_date:=SentenceEffectiveDate, sStatus_code:=sStatusCode, dAlcohol_level:=dAlcoholLevel, sAlcohol_measurement_method:=sAlcoholMeasurementMethod, dDriving_licence_penalty_pts:=DrivingLicencePenaltyPoints)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " + "Call to bSIRParty.Business.UpdatePartyConviction failed. Return code " & m_lReturn)
                        End If
                    ElseIf lConvictionId = 0 Then

                        m_lReturn = oParty.AddConvictions(oConvictions, oConvictions(cnt, 0))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " + "Call to bSIRParty.Business.AddPartyConviction failed. Return code " & m_lReturn)
                        End If
                    End If
                Next
            End If
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartyConviction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        Finally
            ' Destroy objects
            If Not (oParty Is Nothing) Then

                oParty.Dispose()
                oParty = Nothing
            End If
        End Try
        Return result
    End Function
    ''' <summary>
    ''' it used to Update the Party .
    ''' </summary>
    ''' <param name="r_lPartyCnt"></param>
    ''' <param name="sPartyTypeCode"></param>
    ''' <param name="sBranchCode"></param>
    ''' <param name="vAddresses"></param>
    ''' <param name="sTPUserCode"></param>
    ''' <param name="sTPIntroducer"></param>
    ''' <param name="vContacts"></param>
    ''' <param name="sSurname"></param>
    ''' <param name="sForename"></param>
    ''' <param name="sDateOfBirth"></param>
    ''' <param name="sTitle"></param>
    ''' <param name="sMaritalStatusCode"></param>
    ''' <param name="sGenderCode"></param>
    ''' <param name="sInitials"></param>
    ''' <param name="sOccupationCode"></param>
    ''' <param name="sEmployerBusinessCode"></param>
    ''' <param name="sEmploymentStatusCode"></param>
    ''' <param name="sAlternativeID"></param>
    ''' <param name="sCompanyName"></param>
    ''' <param name="sTradingName"></param>
    ''' <param name="sBusinessCode"></param>
    ''' <param name="lAgentCnt"></param>
    ''' <param name="vAddressContacts"></param>
    ''' <param name="r_sTaxNumber"></param>
    ''' <param name="r_bDomiciledForTax"></param>
    ''' <param name="r_bTaxExempt"></param>
    ''' <param name="r_lPercentage"></param>
    ''' <param name="vSupplierBusiness"></param>
    ''' <param name="v_sFileCode"></param>
    ''' <param name="sMainContact"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateParty(ByVal r_lPartyCnt As Integer, ByVal sPartyTypeCode As String,
                                ByVal sBranchCode As String, ByVal vAddresses(,) As Object, ByVal sTPUserCode As String,
                                ByVal sTPIntroducer As String, ByVal vContacts(,) As Object, ByVal sSurname As String,
                                ByVal sForename As String, ByVal sDateOfBirth As String, ByVal sTitle As Object,
                                ByVal sMaritalStatusCode As Object, ByVal sGenderCode As Object,
                                ByVal sInitials As Object, ByVal sOccupationCode As String,
                                ByVal sEmployerBusinessCode As String, ByVal sEmploymentStatusCode As String,
                                ByVal sAlternativeID As String, ByVal sCompanyName As String,
                                ByVal sTradingName As String, ByVal sBusinessCode As String, ByVal lAgentCnt As Integer,
                                Optional ByRef vAddressContacts(,) As Object = Nothing,
                                Optional ByVal r_sTaxNumber As String = "",
                                Optional ByVal r_bDomiciledForTax As Integer = 0,
                                Optional ByVal r_bTaxExempt As Integer = 0, Optional ByVal r_lPercentage As Integer = 0,
                                Optional ByVal vSupplierBusiness As Object = Nothing,
                                Optional ByVal v_sFileCode As String = "", Optional ByVal sMainContact As String = "") As Integer

        Dim nResult As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim nLBnd
        Dim nUBnd As Integer
        Dim iCount As Integer
        Dim sEmailAddress As String = String.Empty
        Dim sTelephoneNumber As String = String.Empty
        Dim sAddress1 As String = String.Empty
        Dim sAddress2 As String = String.Empty
        Dim sAddress3 As String = String.Empty
        Dim sAddress4 As String = String.Empty
        Dim sCountryCode As String = String.Empty
        Dim sPostcode As String = String.Empty
        Dim sContactDesc As String = String.Empty
        Dim sAddress5 As String = String.Empty
        Dim sAddress6 As String = String.Empty
        Dim sAddress7 As String = String.Empty
        Dim sAddress8 As String = String.Empty
        Dim sAddress9 As String = String.Empty
        Dim sAddress10 As String = String.Empty
        Dim aoAdditionalDataArray(,) As Object, vUpdateAddressArray(,) As Object, vDeleteAddressArray(,) As Object
        Dim aoUpdateContactArray() As Object, vDeleteContactArray() As Object
        Dim oGisQuotePolicy As QuotePolicy
        Dim oParty As bSIRParty.Business
        Dim oContact As bSIRContact.Business
        Dim oAddress As bSIRAddress.Business
        Dim iCount2 As Integer
        Dim aoUpdateAddContact(,) As Object

        ' SAM Contact array position constants
        Const ACSCAContactTypeCode As Byte = 0
        Const ACSCAContactTypeID As Byte = 1
        Const ACSCAContactDetail As Byte = 2
        Const ACSCAAreaCode As Byte = 3
        Const ACSCAExtension As Byte = 6
        Const ACSCADescription As Byte = 5 ' Vivek: 20080704 - added the missing item

        ' SAM Address array position constants
        Const ACSAAAddressTypeCode As Byte = 0
        Const ACSAAAddressTypeID As Byte = 1
        Const ACSAAAddress1 As Byte = 2
        Const ACSAAAddress2 As Byte = 3
        Const ACSAAAddress3 As Byte = 4
        Const ACSAAAddress4 As Byte = 5
        Const ACSAACountryCode As Byte = 6
        Const ACSAAPostCode As Byte = 7
        Const ACSAACountryID As Byte = 8
        Const kACSAAAddress5 As Byte = 9
        Const kACSAAAddress6 As Byte = 10
        Const kACSAAAddress7 As Byte = 11
        Const kACSAAAddress8 As Byte = 12
        Const kACSAAAddress9 As Byte = 13
        Const kACSAAAddress10 As Byte = 14

        'Position of address array vDeleteAddresses
        Const ADBAPostCode As Byte = 0
        Const ADBAAddressTypeID As Byte = 1
        Const ADBAAddress1 As Byte = 2
        Const ADBAAddress2 As Byte = 3
        Const ADBAAddress3 As Byte = 4
        Const ADBAAddress4 As Byte = 5
        Const ADBAAddress5 As Byte = 8
        Const ADBAAddress6 As Byte = 9
        Const ADBAAddress7 As Byte = 10
        Const ADBAAddress8 As Byte = 11
        Const ADBAAddress9 As Byte = 12
        Const ADBAAddress10 As Byte = 13
        Const ADBAAddressCountryID As Byte = 14


        Const ACUpdatePartyErrNum As Integer = Constants.vbObjectError + 1
        Const ACUpdatePartyErrSrc As String = "bGIS.STS.UpdateParty"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Move company name if CC
            If sPartyTypeCode = "CC" Then
                sSurname = sCompanyName
                sForename = sCompanyName
            End If

            ' Initialise GISQuotePolicy object
            oGisQuotePolicy = New QuotePolicy()
            m_lReturn = oGisQuotePolicy.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw _
                    New System.Exception(
                        ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                        "Failed to initialise QuotePolicy object. Return code " & m_lReturn)
            End If

            ' Create an instance of the bSIRContact object

            oContact = New bSIRContact.Business
            m_lReturn = oContact.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw _
                    New System.Exception(
                        ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                        "Failed to create bSIRContact.Business object. Return code " & m_lReturn)
            End If

            ' Create an instance of the bSIRAddress object

            oAddress = New bSIRAddress.Business
            m_lReturn = oAddress.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw _
                    New System.Exception(
                        ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                        "Failed to create bSIRAddress.Business object. Return code " & m_lReturn)
            End If

            ' Create an instance of the bSIRParty object

            oParty = New bSIRParty.Business
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw _
                    New System.Exception(
                        ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                        "Failed to create bSIRParty.Business object. Return code " & m_lReturn)
            End If

            ' Extract the first correspondence address (if any exist) from the
            ' address array
            If Informations.IsArray(vAddresses) Then
                For iLoop As Integer = vAddresses.GetLowerBound(1) To vAddresses.GetUpperBound(1)
                    If CDbl(vAddresses(ACSAAAddressTypeCode, iLoop)) = SAMAddressTypeCorrespondence Then
                        sAddress1 = CStr(vAddresses(ACSAAAddress1, iLoop))
                        sAddress2 = CStr(vAddresses(ACSAAAddress2, iLoop))
                        sAddress3 = CStr(vAddresses(ACSAAAddress3, iLoop))
                        sAddress4 = CStr(vAddresses(ACSAAAddress4, iLoop))
                        sCountryCode = CStr(vAddresses(ACSAACountryCode, iLoop))
                        sPostcode = CStr(vAddresses(ACSAAPostCode, iLoop))
                        sAddress5 = CStr(vAddresses(kACSAAAddress5, iLoop))
                        sAddress6 = CStr(vAddresses(kACSAAAddress6, iLoop))
                        sAddress7 = CStr(vAddresses(kACSAAAddress7, iLoop))
                        sAddress8 = CStr(vAddresses(kACSAAAddress8, iLoop))
                        sAddress9 = CStr(vAddresses(kACSAAAddress9, iLoop))
                        sAddress10 = CStr(vAddresses(kACSAAAddress10, iLoop))
                        Exit For
                    End If
                Next
            End If

            ' Build the Additional Data array
            ReDim aoAdditionalDataArray(1, 10)

            aoAdditionalDataArray(0, 0) = "TP_USER_CODE"
            aoAdditionalDataArray(1, 0) = sTPUserCode
            aoAdditionalDataArray(0, 1) = "TP_INTRODUCER"
            aoAdditionalDataArray(1, 1) = sTPIntroducer
            aoAdditionalDataArray(0, 2) = "AddressCountry"
            aoAdditionalDataArray(1, 2) = sCountryCode
            aoAdditionalDataArray(0, 3) = "OCCUPATION_CODE"
            aoAdditionalDataArray(1, 3) = sOccupationCode
            aoAdditionalDataArray(0, 4) = "EMPLOYER_BUSINESS_CODE"
            aoAdditionalDataArray(1, 4) = sEmployerBusinessCode
            aoAdditionalDataArray(0, 5) = "EMPLOYMENT_STATUS_CODE"
            aoAdditionalDataArray(1, 5) = sEmploymentStatusCode
            aoAdditionalDataArray(0, 6) = "ALTERNATIVE_ID"
            aoAdditionalDataArray(1, 6) = sAlternativeID
            aoAdditionalDataArray(0, 7) = "BUSINESS_CODE"
            aoAdditionalDataArray(1, 7) = sBusinessCode
            aoAdditionalDataArray(0, 8) = CNCalledFromSTS
            aoAdditionalDataArray(1, 8) = 1
            aoAdditionalDataArray(0, 9) = "party_type_code"
            aoAdditionalDataArray(1, 9) = sPartyTypeCode
            aoAdditionalDataArray(0, 10) = "TRADING_NAME"
            aoAdditionalDataArray(1, 10) = sTradingName

            ' If the agent cnt is present then use it
            If lAgentCnt > 0 Then
                nUBnd = aoAdditionalDataArray.GetUpperBound(1) + 1
                ReDim Preserve aoAdditionalDataArray(1, nUBnd)

                aoAdditionalDataArray(0, nUBnd) = ACLeadAgent
                aoAdditionalDataArray(1, nUBnd) = lAgentCnt
            End If

            If v_sFileCode <> "" Then
                nUBnd = aoAdditionalDataArray.GetUpperBound(1) + 1
                ReDim Preserve aoAdditionalDataArray(1, nUBnd)

                aoAdditionalDataArray(0, nUBnd) = "FILECODE"
                aoAdditionalDataArray(1, nUBnd) = v_sFileCode
            End If

            'get address from DB
            Dim vDeleteAddresses(,) As Object

            m_lReturn = oParty.GetAddressDetails(vPartyCnt:=r_lPartyCnt, vAddresses:=vDeleteAddresses)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw _
                    New System.Exception(
                        ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                        "Call to bSIRParty.Business.GetAddressDetails failed. Return code " & m_lReturn)
            End If

            'Check if address is updated
            Dim isUpdated As Boolean = True
            If Information.IsArray(vAddresses) Then
                If Information.IsArray(vDeleteAddresses) Then
                    nLBnd = vAddresses.GetLowerBound(1)
                    nUBnd = vAddresses.GetUpperBound(1)

                    'means address is added or deleted
                    If nUBnd <> vDeleteAddresses.GetUpperBound(1) Then
                        isUpdated = True
                    Else
                        For ILoop As Integer = nLBnd To nUBnd
                            If vDeleteAddresses(ADBAAddress1, ILoop) = vAddresses(ACSAAAddress1, ILoop) And
                                       vDeleteAddresses(ADBAAddress2, ILoop) = vAddresses(ACSAAAddress2, ILoop) And
                                       vDeleteAddresses(ADBAAddress3, ILoop) = vAddresses(ACSAAAddress3, ILoop) And
                                       vDeleteAddresses(ADBAAddress4, ILoop) = vAddresses(ACSAAAddress4, ILoop) And
                                       vDeleteAddresses(ADBAPostCode, ILoop) = vAddresses(ACSAAPostCode, ILoop) And
                                       vDeleteAddresses(ADBAAddress5, ILoop) = vAddresses(kACSAAAddress5, ILoop) And
                                       vDeleteAddresses(ADBAAddress6, ILoop) = vAddresses(kACSAAAddress6, ILoop) And
                                       vDeleteAddresses(ADBAAddress7, ILoop) = vAddresses(kACSAAAddress7, ILoop) And
                                       vDeleteAddresses(ADBAAddress8, ILoop) = vAddresses(kACSAAAddress8, ILoop) And
                                       vDeleteAddresses(ADBAAddress9, ILoop) = vAddresses(kACSAAAddress9, ILoop) And
                                       vDeleteAddresses(ADBAAddress10, ILoop) = vAddresses(kACSAAAddress10, ILoop) And
                                       vDeleteAddresses(ADBAAddressCountryID, ILoop) = vAddresses(ACSAACountryID, ILoop) And
                                       vDeleteAddresses(ADBAAddressTypeID, ILoop) = vAddresses(ACSAAAddressTypeID, ILoop) Then

                                'address is not updates
                                isUpdated = False
                            Else
                                isUpdated = True
                                Exit For
                            End If
                        Next
                    End If

                    iCount = 0
                    If isUpdated Then
                        ' Add extra addresses
                        For ILoop As Integer = nLBnd To nUBnd
                            If CStr(vAddresses(ACSAAAddressTypeCode, ILoop)) <> ACProcessed Then

                                m_lReturn = oAddress.DirectAdd(vAddress1:=vAddresses(ACSAAAddress1, ILoop), vAddress2:=vAddresses(ACSAAAddress2, ILoop),
                                                                                    vAddress3:=vAddresses(ACSAAAddress3, ILoop), vAddress4:=vAddresses(ACSAAAddress4, ILoop),
                                                                                    vPostalCode:=vAddresses(ACSAAPostCode, ILoop), vCountryID:=vAddresses(ACSAACountryID, ILoop),
                                                                                    sAddress5:=vAddresses(kACSAAAddress5, ILoop),
                                                                                    sAddress6:=vAddresses(kACSAAAddress6, ILoop),
                                                                                    sAddress7:=vAddresses(kACSAAAddress7, ILoop),
                                                                                    sAddress8:=vAddresses(kACSAAAddress8, ILoop),
                                                                                    sAddress9:=vAddresses(kACSAAAddress9, ILoop),
                                                                                    sAddress10:=vAddresses(kACSAAAddress10, ILoop))
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Throw _
                                        New System.Exception(
                                            ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                                            "Call to bSIRAddress.Business.DirectAdd failed. Return code " & m_lReturn)
                                End If
                                ReDim Preserve vUpdateAddressArray(1, iCount)

                                vUpdateAddressArray(0, iCount) = oAddress.AddressCnt
                                vUpdateAddressArray(1, iCount) = vAddresses(ACSAAAddressTypeID, ILoop)
                                iCount += 1
                            End If
                        Next


                    End If

                End If
            End If



            Dim iCountContacts As Integer
            If Informations.IsArray(vAddresses) Then
                nLBnd = vAddresses.GetLowerBound(1)
                nUBnd = vAddresses.GetUpperBound(1)
                For iLoop As Integer = nLBnd To nUBnd

                    If CStr(vAddresses(ACSAAAddressTypeCode, iLoop)) <> ACProcessed Then



                        If Informations.IsArray(vAddressContacts) Then
                            ReDim aoUpdateAddContact(vAddressContacts.GetUpperBound(1) + 1, 1)
                            For icontacts As Integer = vAddressContacts.GetLowerBound(1) To vAddressContacts.GetUpperBound(1)

                                If CDbl(CDbl(vAddressContacts(4, icontacts)) - 1) = iLoop Then
                                    sContactDesc = CStr(vAddressContacts(ACSCADescription, iLoop))

                                    m_lReturn = oContact.DirectAdd(
                                        vContactTypeID:=vAddressContacts(ACSCAContactTypeID, iLoop),
                                        vDescription:=sContactDesc,
                                        vAreaCode:=vAddressContacts(ACSCAAreaCode, iLoop),
                                        vNumber:=vAddressContacts(ACSCAContactDetail, iLoop))
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Throw _
                                            New System.Exception(
                                                ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                                                "Call to bSIRContact.Business.DirectAdd failed. Return code " &
                                                m_lReturn)
                                    End If
                                    aoUpdateAddContact(iCountContacts, 0) = oContact.ContactCnt
                                    aoUpdateAddContact(iCountContacts, 1) = oAddress.AddressCnt
                                    iCountContacts += 1
                                End If
                            Next
                        End If
                    End If
                Next
            End If

            If iCountContacts > 0 Then

                m_lReturn = oParty.UpdateAddressContacts(vAddContacts:=aoUpdateAddContact)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw _
                        New System.Exception(
                            ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                            "Call to bSIRParty.Business.UpdateAddressContacts failed. Return code " & m_lReturn)
                End If
            End If

            If isUpdated Then
                If Informations.IsArray(vDeleteAddresses) Then
                    'ReDim vDeleteAddressArray(1, 0)
                    iCount2 = 0

                    For iLoop As Integer = vDeleteAddresses.GetLowerBound(1) To vDeleteAddresses.GetUpperBound(1)
                        ReDim Preserve vDeleteAddressArray(1, iCount2)

                        vDeleteAddressArray(0, iCount2) = vDeleteAddresses(6, iLoop) 'address_cnt
                        vDeleteAddressArray(1, iCount2) = vDeleteAddresses(1, iLoop) 'address_usage_type_id
                        iCount2 += 1
                    Next iLoop
                End If

                ' Update the address usage
                If iCount > 0 Then
                    m_lReturn = oParty.UpdateAddresses(vPartyCnt:=r_lPartyCnt, vAddAddresses:=vUpdateAddressArray,
                                                       vDeleteAddresses:=vDeleteAddressArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw _
                            New System.Exception(
                                ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                                "Call to bSIRParty.Business.UpdateAddresses failed. Return code " & m_lReturn)
                    End If
                End If
            End If

            iCount = 0
            If Informations.IsArray(vContacts) Then
                For iLoop As Integer = vContacts.GetLowerBound(1) To vContacts.GetUpperBound(1)

                    If CStr(vContacts(ACSCAContactTypeCode, iLoop)) <> ACProcessed Then
                        sContactDesc = CStr(vContacts(ACSCADescription, iLoop)) _
                        ' Vivek: 20080704 - added the missing item

                        m_lReturn = oContact.DirectAdd(vContactTypeID:=vContacts(ACSCAContactTypeID, iLoop),
                                                       vDescription:=sContactDesc,
                                                       vAreaCode:=vContacts(ACSCAAreaCode, iLoop),
                                                       vNumber:=vContacts(ACSCAContactDetail, iLoop),
                                                       vExtension:=vContacts(ACSCAExtension, iLoop))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw _
                                New System.Exception(
                                    ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                                    "Call to bSIRContact.Business.DirectAdd failed. Return code " & m_lReturn)
                        End If
                        ReDim Preserve aoUpdateContactArray(iCount)

                        aoUpdateContactArray(iCount) = oContact.ContactCnt
                        iCount += 1
                    End If
                Next
            End If

            Dim vDeleteContacts(,) As Object
            Dim iDeleteCount As Integer
            iDeleteCount = 0

            m_lReturn = oParty.GetContactDetails(vPartyCnt:=r_lPartyCnt, vContacts:=vDeleteContacts)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw _
                    New System.Exception(
                        ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                        "Call to bSIRParty.Business.GetContactDetails failed. Return code " & m_lReturn)
            End If
            If Informations.IsArray(vDeleteContacts) Then
                ReDim vDeleteContactArray(vDeleteContacts.GetUpperBound(1))
                For iLoop As Integer = vDeleteContacts.GetLowerBound(1) To vDeleteContacts.GetUpperBound(1)
                    vDeleteContactArray(iLoop) = vDeleteContacts(0, iLoop) 'contact_cnt
                    iDeleteCount += 1
                Next iLoop
            End If

            ' Update the address usage
            ' NOTE: Vivek - updating the mapping for Party and Contacts
            If iCount > 0 Or iDeleteCount > 0 Then
                If Informations.IsArray(vDeleteContacts) Then
                    If iCount > 0 Then
                        m_lReturn = oParty.UpdateContacts(vPartyCnt:=r_lPartyCnt, vAddContacts:=aoUpdateContactArray, vDeleteContacts:=vDeleteContactArray, bAddContacts:=True)
                    Else
                        m_lReturn = oParty.UpdateContacts(vPartyCnt:=r_lPartyCnt, vAddContacts:=aoUpdateContactArray, vDeleteContacts:=vDeleteContactArray, bAddContacts:=False)
                    End If
                Else
                    m_lReturn = oParty.UpdateContacts(vPartyCnt:=r_lPartyCnt, vAddContacts:=aoUpdateContactArray)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw _
                        New System.Exception(
                            ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                            "Call to bSIRParty.Business.UpdateContacts failed. Return code " & m_lReturn)
                End If
            End If

            'Code added To Manage MainContact PN Number 42880
            Dim lMainContactCnt As Integer
            Dim sMainContactDesc As String = ""
            If sMainContact <> "" Then
                m_lReturn = oParty.GetMainContact(vPartyCnt:=(r_lPartyCnt), lMainContactCnt:=lMainContactCnt,
                                                  sMainContactDesc:=sMainContactDesc)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw _
                        New System.Exception(
                            ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                            "Call to bSIRParty.Business.GetMainContact failed. Return code " & m_lReturn)
                End If

                m_lReturn = oParty.UpdateMainContact(vPartyCnt:=r_lPartyCnt, lMainContactCnt:=lMainContactCnt,
                                                     sMainContactDesc:=sMainContact)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw _
                        New System.Exception(
                            ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                            "Call to bSIRParty.Business.UpdateMainContact failed. Return code " & m_lReturn)
                End If
            End If

            m_lReturn = oGisQuotePolicy.UpdateParty(v_sGisDataModelCode:="AOL", v_sGisBusinessTypeCode:="NB", v_lPartyCnt:=r_lPartyCnt,
                                                    v_sForename:=sForename, v_sSurname:=sSurname, v_sDateOfBirth:=sDateOfBirth,
                                                    v_sEmailAddress:=sEmailAddress, v_sCurrentRenewalDate:=DateTime.Now,
                                                    v_sAddress1:=sAddress1, v_sAddress2:=sAddress2, v_sAddress3:=sAddress3, v_sAddress4:=sAddress4,
                                                    v_sPostcode:=sPostcode, v_sTitle:=sTitle, v_sMaritalStatusCode:=sMaritalStatusCode,
                                                    v_sGenderCode:=sGenderCode, v_sInitials:=sInitials, v_sTelephoneNumber:=sTelephoneNumber,
                                                    v_vAdditionalDataArray:=aoAdditionalDataArray,
                                                     sAddress5:=sAddress5,
                                                    sAddress6:=sAddress6,
                                                    sAddress7:=sAddress7,
                                                    sAddress8:=sAddress8,
                                                    sAddress9:=sAddress9,
                                                    sAddress10:=sAddress10)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(ACUpdatePartyErrNum.ToString() + ", " _
                                           + ACUpdatePartyErrSrc + ", " +
                                           "Call to bGIS.QuotePolicy.UpdateParty " +
                                           "failed. Return code " & m_lReturn)
            End If

            'Update Party Table for Tax Details
            m_lReturn = oParty.AddTaxDetails(r_lPartyCnt:=r_lPartyCnt, r_sTaxNumber:=r_sTaxNumber,
                                             r_bDomiciledForTax:=r_bDomiciledForTax, r_bTaxExempt:=r_bTaxExempt,
                                             r_lPercentage:=r_lPercentage)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw _
                    New System.Exception(
                        ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                        "Call to bSIRParty.Business.AddTaxDetails failed. Return code " & m_lReturn)
            End If

            If Informations.IsArray(vSupplierBusiness) Then
                m_lReturn = oParty.UpdateSupplierBusiness(vSupplierBusiness, r_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw _
                        New System.Exception(
                            ACUpdatePartyErrNum.ToString() + ", " + ACUpdatePartyErrSrc + ", " +
                            "Call to bSIRParty.Business.SupplierBusiness failed. Return code " & m_lReturn)
                End If
            End If
        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateParty", excep:=excep)
        Finally
            ' Destroy objects
            If (oGisQuotePolicy IsNot Nothing) Then
                oGisQuotePolicy.Dispose()
                oGisQuotePolicy = Nothing
            End If
            If (oAddress IsNot Nothing) Then
                oAddress.Dispose()
                oAddress = Nothing
            End If
            If (oParty IsNot Nothing) Then
                oParty.Dispose()
                oParty = Nothing
            End If
            If (oContact IsNot Nothing) Then
                oContact.Dispose()
                oContact = Nothing
            End If
        End Try

        Return nResult
    End Function

    Public Function UpdateLastEdiMessageCountReceived(ByVal v_sGisDataModelCode As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lLastEdiMessageCountReceived As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Dim oBom As Object

            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:="Application", r_oBOM:=oBom, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLastEdiMessageCountReceived Failed to Create BOM", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLastEdiMessageCountReceived")
                Return result
            End If

            If Not (oBom Is Nothing) Then
                m_lReturn = oBom.UpdateLastEdiMessageCountReceived(v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), v_lLastEdiMessageCountReceived:=gPMFunctions.ToSafeInteger(v_lLastEdiMessageCountReceived))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BOM UpdateLastEdiMessageCountReceived Method Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLastEdiMessageCountReceived")
                    Return result
                End If

                ' Destroy the BOM.QuotePolicy class

                oBom.Dispose()
                oBom = Nothing
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLastEdiMessageCountReceived Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLastEdiMessageCountReceived", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetBackofficeData(ByVal v_sBrokerAbiId As Object, ByVal v_sExternalSchemeNo As String, ByRef r_lAgentCnt As Integer) As Integer
        Return GetBackofficeData(v_sBrokerAbiId:=v_sBrokerAbiId, v_sExternalSchemeNo:=v_sExternalSchemeNo, r_lAgentCnt:=r_lAgentCnt, r_lGisSchemeId:=0, r_lGisInsurerId:=0, r_lRiskCodeId:=0, r_lRiskGroupId:=0, r_lInsurerCnt:=0, r_sSchemeDesc:="", r_sInsurerDesc:="")
    End Function
    Public Function GetBackofficeData(ByVal v_sBrokerAbiId As Object, ByVal v_sExternalSchemeNo As String, ByRef r_lAgentCnt As Integer, ByRef r_lGisSchemeId As Integer, ByRef r_lGisInsurerId As Integer, ByRef r_lRiskCodeId As Integer, ByRef r_lRiskGroupId As Integer, ByRef r_lInsurerCnt As Integer, ByRef r_sSchemeDesc As String, ByRef r_sInsurerDesc As String) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetAgentCntFromBrokerID(v_sBrokerAbiId:=v_sBrokerAbiId, r_lAgentCnt:=r_lAgentCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                STSErrorReturnValue = gPMConstants.PMEReturnCode.PMFalse
                STSErrorDescription = "GetAgentCntFromBrokerID failed"

                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgentCntFromBrokerID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBackofficeData")
                Return result
            End If

            m_lReturn = GetDataFromExternalSchemeNo(v_sExternalSchemeNo:=v_sExternalSchemeNo, r_lGisSchemeId:=r_lGisSchemeId, r_lGisInsurerId:=r_lGisInsurerId, r_lRiskCodeId:=r_lRiskCodeId, r_lRiskGroupId:=r_lRiskGroupId, r_lInsurerCnt:=r_lInsurerCnt, r_sSchemeDesc:=r_sSchemeDesc, r_sInsurerDesc:=r_sInsurerDesc)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataFromExternalSchemeNo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBackofficeData")
                Return result
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBackofficeData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBackofficeData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetUserDetails(ByVal v_sUsername As String, ByRef r_sUserDataXML As String) As Integer
        Dim result As Integer = 0
        Dim oXML As New XmlDocument

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            ' Add the username parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="username", vValue:=v_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the stored procedure in the database to return the XML data
            m_lReturn = m_oDatabase.SQLSelectForXML(sSQL:=ACSQLEDIGetUserDetailsSQL, bStoredProcedure:=ACSQLEDIGetUserDetailsStored, oXMLDOM:=oXML)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the XML. This can then be deserialized into a class in the STS
            r_sUserDataXML = oXML.InnerXml

            ' Clear up the XML object
            oXML = Nothing

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    Public Function GetAgentCntFromBrokerID(ByVal v_sBrokerAbiId As Object, ByRef r_lAgentCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim oInsuranceFile As bSIRInsuranceFile.Services

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of bSIRInsuranceFile.Services

            oInsuranceFile = New bSIRInsuranceFile.Services
            'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oInsuranceFile, v_sClassName:="bSIRInsuranceFile.Services", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    result = m_lReturn
            '    ' Log Error Message
            '    bPMFunc.LogMessage(sUsername:=gpmfunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgentCntFromBrokerID Failed - Failed to create business object to bSIRInsuranceFile.Services.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesByBranch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            'End If
            m_lReturn = oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the property to load all the details
            oInsuranceFile.BrokerAbiId = v_sBrokerAbiId

            ' Get the party_cnt
            r_lAgentCnt = CInt(oInsuranceFile.LeadAgentCnt)
            ' Clear up
            oInsuranceFile.Dispose()
            oInsuranceFile = Nothing
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgentCntFromBrokerID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgentCntFromBrokerID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetDataFromExternalSchemeNo(ByVal v_sExternalSchemeNo As String) As Integer
        Return GetDataFromExternalSchemeNo(v_sExternalSchemeNo:=v_sExternalSchemeNo, r_lGisSchemeId:=0, r_lGisInsurerId:=0, r_lRiskCodeId:=0, r_lRiskGroupId:=0, r_lInsurerCnt:=0, r_sSchemeDesc:="", r_sInsurerDesc:="")
    End Function
    Public Function GetDataFromExternalSchemeNo(ByVal v_sExternalSchemeNo As String, ByRef r_lGisSchemeId As Integer, ByRef r_lGisInsurerId As Integer, ByRef r_lRiskCodeId As Integer, ByRef r_lRiskGroupId As Integer, ByRef r_lInsurerCnt As Integer, ByRef r_sSchemeDesc As String, ByRef r_sInsurerDesc As String) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Dim vResultArray(,) As Object

            r_lGisSchemeId = 0
            r_lGisInsurerId = 0
            r_lRiskCodeId = 0
            r_lRiskGroupId = 0
            r_lInsurerCnt = 0

            m_oDatabase.Parameters.Clear()

            ' Add the external scheme no parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="sExternalSchemeNo", vValue:=v_sExternalSchemeNo.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the policy start date parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="dtPolicyStartDate", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the source_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGISSchemeEDILinkSTSSQL, sSQLName:=ACGISSchemeEDILinkSTSName, bStoredProcedure:=ACGISSchemeEDILinkSTSStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then
                r_lGisSchemeId = CInt(vResultArray(0, 0))
                r_lGisInsurerId = CInt(vResultArray(1, 0))
                r_lRiskGroupId = CInt(vResultArray(2, 0))
                r_lRiskCodeId = CInt(vResultArray(3, 0))
                r_lInsurerCnt = CInt(vResultArray(4, 0))
                r_sSchemeDesc = CStr(vResultArray(5, 0))
                r_sInsurerDesc = CStr(vResultArray(6, 0))
            Else
                'not found raise an error
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            If r_lGisSchemeId = 0 Or r_lGisInsurerId = 0 Or r_lRiskCodeId = 0 Or r_lRiskGroupId = 0 Or r_lInsurerCnt = 0 Then
                'Should always return a value
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataFromExternalSchemeNo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataFromExternalSchemeNo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function RollbackEDI(ByVal v_lGISPolicyLinkID As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameter collection
            m_oDatabase.Parameters.Clear()

            ' ... and add the policy_link
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lGISPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' along with the insurance_file_cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the stored procedure to delete all the data
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRollbackEDISQL, sSQLName:=ACRollbackEDIName, bStoredProcedure:=ACRollbackEDIStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call " & ACRollbackEDISQL & ". policy_link_id=" & CStr(v_lGISPolicyLinkID) & " insurance_file_cnt=" & CStr(v_lInsuranceFileCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackEDI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackEDI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackEDI", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    Public Function GetPolicyDetailsFromAlternateReference(ByVal v_vAlternateReference As Object) As Integer
        Return GetPolicyDetailsFromAlternateReference(v_vAlternateReference:=v_vAlternateReference, r_lPartyCnt:=0, r_lInsuranceFileCnt:=0, r_lInsuranceFolderCnt:=0, r_lLastEdiMessageCountReceived:=0, r_lRiskFolderCnt:=0)
    End Function
    Public Function GetPolicyDetailsFromAlternateReference(ByVal v_vAlternateReference As Object, ByRef r_lInsuranceFolderCnt As Integer) As Integer
        Return GetPolicyDetailsFromAlternateReference(v_vAlternateReference:=v_vAlternateReference, r_lPartyCnt:=0, r_lInsuranceFileCnt:=0, r_lInsuranceFolderCnt:=r_lInsuranceFolderCnt, r_lLastEdiMessageCountReceived:=0, r_lRiskFolderCnt:=0)
    End Function
    Public Function GetPolicyDetailsFromAlternateReference(ByVal v_vAlternateReference As Object, ByRef r_lPartyCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_lLastEdiMessageCountReceived As Integer, ByRef r_lRiskFolderCnt As Integer) As Integer
        Dim result As Integer = 0
        Try

            Const ACPartyCnt As Integer = 0
            Const ACInsuranceFolderCnt As Integer = 1
            Const ACInsuranceFileCnt As Integer = 6
            Const ACLastEdiMessageCountReceived As Integer = 10
            Const ACRiskFolderCnt As Integer = 11

            Const ACGetPolicyVersionForMtaAltReferenceSQL As String = "spu_GetPolicyVersionForMtaAltReference"
            Const ACGetPolicyVersionForMtaAltReferenceName As String = "GetPolicyVersionForMtaAltReference"
            Const ACGetPolicyVersionForMtaAltReferenceStored As Boolean = True

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the new one
            m_lReturn = m_oDatabase.Parameters.Add(sName:="alternate_reference", vValue:=CStr(v_vAlternateReference), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter alternate_reference = " & CStr(v_vAlternateReference), vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetailsFromAlternateReference")
                Return result
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyVersionForMtaAltReferenceSQL, sSQLName:=ACGetPolicyVersionForMtaAltReferenceName, bStoredProcedure:=ACGetPolicyVersionForMtaAltReferenceStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call SQL : " & ACGetPolicyVersionForMtaAltReferenceSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetailsFromAlternateReference")
                Return result
            End If

            ' Check our results
            If Informations.IsArray(vResultArray) Then
                r_lPartyCnt = CInt(vResultArray(ACPartyCnt, 0))
                r_lInsuranceFolderCnt = CInt(vResultArray(ACInsuranceFolderCnt, 0))
                r_lInsuranceFileCnt = CInt(vResultArray(ACInsuranceFileCnt, 0))
                r_lLastEdiMessageCountReceived = CInt(vResultArray(ACLastEdiMessageCountReceived, 0))
                r_lRiskFolderCnt = CInt(vResultArray(ACRiskFolderCnt, 0))
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
                ' Didn't return anything, lets error
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACGetPolicyVersionForMtaAltReferenceName & " didn't return any data.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetailsFromAlternateReference")
                Return result
            End If

            ' Clear up
            vResultArray = Nothing
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyDetailsFromAlternateReference Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetailsFromAlternateReference", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetRenPolicyDetailsFromAltRef(ByVal v_sAlternateReference As String) As Integer
        Return GetRenPolicyDetailsFromAltRef(v_sAlternateReference:=v_sAlternateReference, r_lPartyCnt:=0, r_lInsuranceFileCnt:=0, r_lInsuranceFolderCnt:=0, r_lLastEdiMessageCountReceived:=0, r_lRiskFolderCnt:=0, r_sThisPremium:="", r_sNetPremium:="0", r_sTaxAmount:="0", r_lRenewalGisSchemeId:=0, r_sGISDataModelCode:="")
    End Function
    Public Function GetRenPolicyDetailsFromAltRef(ByVal v_sAlternateReference As String, ByRef r_lPartyCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_lLastEdiMessageCountReceived As Integer, ByRef r_lRiskFolderCnt As Integer, ByRef r_sThisPremium As String, ByRef r_sNetPremium As String, ByRef r_sTaxAmount As String, ByRef r_lRenewalGisSchemeId As Integer, ByRef r_sGISDataModelCode As String) As Integer
        Dim result As Integer = 0
        Try
            Const ACPartyCnt As Integer = 0
            Const ACInsuranceFolderCnt As Integer = 1
            Const ACGisSchemeId As Integer = 4
            Const ACGisDataModelCode As Integer = 7
            Const ACInsuranceFileCnt As Integer = 6
            Const ACLastEdiMessageCountReceived As Integer = 10
            Const ACRiskFolderCnt As Integer = 11
            Const ACThisPremium As Integer = 12
            Const ACNetPremium As Integer = 13
            Const ACTaxAmount As Integer = 14

            Const ACGetRenPolicyDetailsFromAltRefSQL As String = "spu_RenewalPolicyDetailsFromAltReference"
            Const ACGetRenPolicyDetailsFromAltRefName As String = "GetRenPolicyDetailsFromAltRef"
            Const ACGetRenPolicyDetailsFromAltRefStored As Boolean = True

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the new one
            m_lReturn = m_oDatabase.Parameters.Add(sName:="alternate_reference", vValue:=v_sAlternateReference, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter alternate_reference = " & v_sAlternateReference, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenPolicyDetailsFromAltRef")
                Return result
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRenPolicyDetailsFromAltRefSQL, sSQLName:=ACGetRenPolicyDetailsFromAltRefName, bStoredProcedure:=ACGetRenPolicyDetailsFromAltRefStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call SQL : " & ACGetRenPolicyDetailsFromAltRefSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetGetRenPolicyDetailsFromAltRef")
                Return result
            End If

            ' Check our results
            If Informations.IsArray(vResultArray) Then
                r_lPartyCnt = CInt(vResultArray(ACPartyCnt, 0))
                r_lInsuranceFolderCnt = CInt(vResultArray(ACInsuranceFolderCnt, 0))
                r_lInsuranceFileCnt = CInt(vResultArray(ACInsuranceFileCnt, 0))
                r_lLastEdiMessageCountReceived = CInt(vResultArray(ACLastEdiMessageCountReceived, 0))
                r_lRiskFolderCnt = CInt(vResultArray(ACRiskFolderCnt, 0))
                r_sThisPremium = StringsHelper.Format(vResultArray(ACThisPremium, 0), "######0.00")
                r_sNetPremium = StringsHelper.Format(vResultArray(ACNetPremium, 0), "######0.00")
                r_sTaxAmount = StringsHelper.Format(vResultArray(ACTaxAmount, 0), "######0.00")
                r_lRenewalGisSchemeId = CInt(vResultArray(ACGisSchemeId, 0))
                r_sGISDataModelCode = CStr(vResultArray(ACGisDataModelCode, 0))
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
                ' Didn't return anything, lets error
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACGetRenPolicyDetailsFromAltRefName & " didn't return any data.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenPolicyDetailsFromAltRef")
                Return result
            End If
            ' Clear up
            vResultArray = Nothing
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenPolicyDetailsFromAltRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenPolicyDetailsFromAltRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function ProcessClaim(ByVal v_bIsNewClaim As Boolean, ByVal v_lInsuranceFileCnt As Integer, ByVal v_dtLossDate As Date, ByVal v_sClaimHandler As String, ByVal v_sProgressStatus As String, ByVal v_sDescription As String, ByVal v_sPrimaryCause As String, ByVal v_dtDateReported As Date, ByVal v_lClaimStatusId As Integer, ByVal v_sClaimNumber As String, ByVal v_sPerilType As String, ByVal v_sReserveType As String, ByVal v_cReserveAmount As Decimal, ByVal v_dtPaymentDate As Date, ByVal v_cPaymentAmount As Decimal, ByVal v_sSalvageRecoveryType As String, ByVal v_cSalvageAmount As Decimal, ByVal v_sTPRecoveryType As String, ByVal v_cTPAmount As Decimal, ByVal v_sInsurerClaimNo As String) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Dim oSBOLink As bSIRIUSLink.Claims

            m_lSTSErrorReturnValue = gPMConstants.PMEReturnCode.PMTrue
            m_sSTSErrorDescription = ""

            oSBOLink = New bSIRIUSLink.Claims
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise SBO link object", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessClaim")
                oSBOLink = Nothing
                Return result
            End If

            m_lReturn = oSBOLink.ProcessSTSClaim(v_bIsNewClaim:=v_bIsNewClaim, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dtLossDate:=v_dtLossDate, v_sClaimHandler:=v_sClaimHandler, v_sProgressStatus:=v_sProgressStatus, v_sDescription:=v_sDescription, v_sPrimaryCause:=v_sPrimaryCause, v_dtDateReported:=v_dtDateReported, v_lClaimStatusId:=v_lClaimStatusId, v_sClaimNumber:=v_sClaimNumber, v_sPerilType:=v_sPerilType, v_sReserveType:=v_sReserveType, v_cReserveAmount:=v_cReserveAmount, v_dtPaymentDate:=v_dtPaymentDate, v_cPaymentAmount:=v_cPaymentAmount, v_sSalvageRecoveryType:=v_sSalvageRecoveryType, v_cSalvageAmount:=v_cSalvageAmount, v_sTPRecoveryType:=v_sTPRecoveryType, v_cTPAmount:=v_cTPAmount, v_sInsurerClaimNo:=v_sInsurerClaimNo)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSTSClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessClaim")

                'ensure STS error is available to calling process
                m_lSTSErrorReturnValue = oSBOLink.STSErrorReturnValue
                m_sSTSErrorDescription = oSBOLink.STSErrorDescription

                oSBOLink.Dispose()
                oSBOLink = Nothing
                Return result
            End If
            oSBOLink.Dispose()
            oSBOLink = Nothing
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function FindPolicies(ByVal v_dtDateOfLoss As Date, ByVal v_sPolicyNumber As String, ByVal v_sClientSurname As String, ByVal v_sClientPostcode As String, ByVal v_sAgentName As String, ByVal v_sBranchCode As String, ByRef r_vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Const ACFindPoliciesSQL As String = "spu_STS_FindPolicies"
            Const ACFindPoliciesName As String = "FindPolicies"
            Const ACFindPoliciesStored As Boolean = True
            Dim vValue As Object

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' date of loss
            '13184 STS FindPolicies method does not work due to missing SP
            If v_dtDateOfLoss = GISSharedConstants.GISLowDate Or v_dtDateOfLoss = #12:00:00 AM# Then
                vValue = DBNull.Value
            Else
                vValue = v_dtDateOfLoss
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="date_of_loss", vValue:=CStr(vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter date_of_loss = " & v_dtDateOfLoss, vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicies")
                Return result
            End If

            ' policy number
            If v_sPolicyNumber.Trim() = "" Then
                vValue = DBNull.Value
            Else
                vValue = v_sPolicyNumber.Trim()
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_number", vValue:=CStr(vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter policy_number = " & v_sPolicyNumber, vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicies")
                Return result
            End If

            ' client surname
            If v_sClientSurname.Trim() = "" Then
                vValue = DBNull.Value
            Else
                vValue = v_sClientSurname.Trim()
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="client_surname", vValue:=CStr(vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter client_surname = " & v_sClientSurname, vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicies")
                Return result
            End If

            ' client postcode
            If v_sClientPostcode.Trim() = "" Then
                vValue = DBNull.Value
            Else
                vValue = v_sClientPostcode.Trim()
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="client_postcode", vValue:=CStr(vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter client_postcode = " & v_sClientPostcode, vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicies")
                Return result
            End If

            ' agent name
            If v_sAgentName.Trim() = "" Then
                vValue = DBNull.Value
            Else
                vValue = v_sAgentName.Trim()
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_name", vValue:=CStr(vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter agent_name = " & v_sAgentName, vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicies")
                Return result
            End If

            ' branch code
            If v_sBranchCode.Trim() = "" Then
                vValue = DBNull.Value
            Else
                vValue = v_sBranchCode.Trim()
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="branch_code", vValue:=CStr(vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter branch_code = " & v_sBranchCode, vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicies")
                Return result
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACFindPoliciesSQL, sSQLName:=ACFindPoliciesName, bStoredProcedure:=ACFindPoliciesStored, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call SQL : " & ACFindPoliciesSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicies")
                Return result
            End If
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicies", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ' ***************************************************************** '
    ' Name: GetPMLookupIdFromCode
    '
    ' Description:
    '
    ' History: 09/11/2004 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function GetPMLookupIdFromCode(ByVal v_sLookupTable As String, ByVal v_sLookupCode As String, ByRef r_sLookupId As Integer) As Integer


        Dim result As Integer = 0
        Try

            Dim sGetPMLookupIdFromCodeSQL As String = ""
            Const ACGetPMLookupIdFromCodeName As String = "GetPMLookupIdFromCode"
            Const ACGetPMLookupIdFromCodeStored As Boolean = False

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            'build the SQL
            sGetPMLookupIdFromCodeSQL = "select " & v_sLookupTable & "_id from " & v_sLookupTable & " where code='" & v_sLookupCode & "'"

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sGetPMLookupIdFromCodeSQL, sSQLName:=ACGetPMLookupIdFromCodeName, bStoredProcedure:=ACGetPMLookupIdFromCodeStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call SQL : " & sGetPMLookupIdFromCodeSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetGetPMLookupIdFromCode")
                Return result
            End If

            ' Check our results
            If Informations.IsArray(vResultArray) Then

                r_sLookupId = CInt(vResultArray(0, 0))
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
                ' Didn't return anything, lets error
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACGetPMLookupIdFromCodeName & " didn't return any data.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMLookupIdFromCode")
                Return result
            End If

            ' Clear up
            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPMLookupIdFromCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMLookupIdFromCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function UpdateCoverDetails(ByVal v_lInsuranceFileCnt As Integer) As Integer
        Return UpdateCoverDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dtCoverStartDate:=Nothing, v_dtExpiryDate:=Nothing, v_sInsuranceRef:=Nothing, v_sVehicleMakeModel:=Nothing, v_lPartyCnt:=Nothing, v_sRiskCode:=Nothing, v_lLeadInsurerABICode:=Nothing, v_lCurrencyID:=-1, v_sInsuredName:="")
    End Function
    Public Function UpdateCoverDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dtCoverStartDate As Object, ByVal v_dtExpiryDate As Object, ByVal v_sInsuranceRef As Object, ByVal v_sVehicleMakeModel As Object, ByVal v_lPartyCnt As Object, ByVal v_sRiskCode As Object, ByVal v_lLeadInsurerABICode As Object, ByVal v_lCurrencyID As Integer, ByVal v_sInsuredName As String) As Integer

        Dim result As Integer = 0
        Dim oSiriusLink As bSIRIUSLink.SIRIUSLink

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            oSiriusLink = New bSIRIUSLink.SIRIUSLink()

            m_lReturn = oSiriusLink.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_oDatabase)

            'RJG 27/06/2000 - Quit the function if the quote object failed to initialise
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return result
            End If





            m_lReturn = oSiriusLink.UpdateCoverDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dtCoverStartDate:=gPMFunctions.ToSafeDate(v_dtCoverStartDate), v_dtExpiryDate:=gPMFunctions.ToSafeDate(v_dtExpiryDate), v_sInsuranceRef:=v_sInsuranceRef, v_sVehicleMakeModel:=v_sVehicleMakeModel, v_lPartyCnt:=v_lPartyCnt, v_sRiskCode:=v_sRiskCode, v_lLeadInsurerABICode:=v_lLeadInsurerABICode, v_lCurrencyID:=v_lCurrencyID, v_sInsuredName:=v_sInsuredName)

            'Test for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateCoverDetails failed attempting to update Insurance File - " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCoverDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            oSiriusLink.Dispose()

            oSiriusLink = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Clean up objects
            If oSiriusLink Is Nothing Then
            Else
                oSiriusLink = Nothing
            End If

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateCoverDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCoverDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

            Return result
        End Try
    End Function

    Public Function GenerateDocument(ByVal v_sDocumentTemplateCode As String,
                                     ByVal v_lMode As Integer, ByVal v_lPartyCnt As Integer,
                                     Optional ByVal v_lInsuranceFolderCnt As Integer = 0,
                                     Optional ByVal v_lInsuranceFileCnt As Integer = 0,
                                     Optional ByVal v_sParameterXML As String = "",
                                     Optional ByVal v_bOutputAsHTML As Boolean = True,
                                     Optional ByRef r_sMergedFilePath As String = "",
                                     Optional ByRef r_sSpooledFilePath As String = "",
                                     Optional ByVal v_bOutputAsPDF As Boolean = False,
                                     Optional ByVal v_lClaimKey As Integer = 0,
                                     Optional ByVal v_dtEffectiveDate As Date = #12/30/1899#,
                                     Optional ByVal v_sDocumentRef As String = "",
                                     Optional ByVal v_bOutputAsTXT As Boolean = False,
                                     Optional ByVal sArchiveDocFileName As String = "",
                                     Optional ByVal v_bSkipArchiveonEdit As Boolean = False,
                                     Optional ByVal sDestinationFilename As String = "",
                                     Optional ByVal bIsSuppressArchive As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oDocumentWrapper As bSIRDocManagerWrapper.Interface_Renamed
        Dim bDocumasterInstalled As Boolean
        Dim sOptionValue As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            oDocumentWrapper = New bSIRDocManagerWrapper.Interface_Renamed
            'Invoke Initialise method
            lReturn = CType(oDocumentWrapper.InitialiseBusiness(sUsername:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=gPMFunctions.ToSafeString(m_sCallingAppName), vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            'Test for errors
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateDocument Failed - Failed to create bSIRDocManagerWrapper object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Set the object property values
            oDocumentWrapper.DocumentTemplateCode = v_sDocumentTemplateCode ' The document we need to generate
            oDocumentWrapper.Mode = v_lMode ' The document generation mode
            oDocumentWrapper.PartyCnt = v_lPartyCnt ' This is a mandatory property to be set
            oDocumentWrapper.InsuranceFolderCnt = v_lInsuranceFolderCnt
            oDocumentWrapper.InsuranceFileCnt = v_lInsuranceFileCnt
            oDocumentWrapper.ParameterXML = v_sParameterXML ' This is needed if you want to send any Parameters into the document
            If v_lMode = ACEmailDocType Then
                oDocumentWrapper.CalledFromSAM = False
                oDocumentWrapper.RetainTempFiles = True
            Else
                oDocumentWrapper.CalledFromSAM = True
            End If
            oDocumentWrapper.ArchieveDocFileName = sArchiveDocFileName
            oDocumentWrapper.OutputAsHTML = v_bOutputAsHTML
            oDocumentWrapper.OutputAsPDF = v_bOutputAsPDF
            oDocumentWrapper.OutputAsTXT = v_bOutputAsTXT
            oDocumentWrapper.DestinationFilename = sDestinationFilename
            If v_lClaimKey <> 0 Then
                oDocumentWrapper.ClaimCnt = v_lClaimKey
            End If

            ' RDT - PN64173 - Add Document Ref for producing Invoices etc.
            If v_sDocumentRef <> "" Then
                oDocumentWrapper.DocumentRef = v_sDocumentRef
            End If

            oDocumentWrapper.SkipArchiveOnEdit = v_bSkipArchiveonEdit

            oDocumentWrapper.EffectiveDate = If(v_dtEffectiveDate = #12/30/1899#, CDate(DateTime.Now.ToString("d")), v_dtEffectiveDate)
            bDocumasterInstalled = False
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=10, r_sOptionValue:=sOptionValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DocuMaster is not enabled", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=0, vErrDesc:="DocuMaster is not enabled")
            ElseIf sOptionValue = "1" Then
                bDocumasterInstalled = True
            End If

            If bDocumasterInstalled Then
                oDocumentWrapper.ArchiveDoc = True
            End If
            '' Set property if document is already auto archive to sharepoint (Issue SSP-1828)
            oDocumentWrapper.IsSuppressArchive = bIsSuppressArchive
            oDocumentWrapper.IsNonBatchProcess = True

            'Invoke object Start method
            lReturn = CType(oDocumentWrapper.Start(), gPMConstants.PMEReturnCode)

            'Test for errors
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GenerateDocument Failed - Failed to execute bSIRDocManagerWrapper.Interface Start ", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Set return values
            r_sMergedFilePath = oDocumentWrapper.MergedFilePath
            r_sSpooledFilePath = oDocumentWrapper.SpooledFilePath

            'Capture split document list from the wrapper before disposing
            If oDocumentWrapper.ResolvedDocumentList IsNot Nothing Then
                m_oResolvedDocumentList = New List(Of String)(oDocumentWrapper.ResolvedDocumentList)
            Else
                m_oResolvedDocumentList = Nothing
            End If
            If oDocumentWrapper.SplitDocMergedCodes IsNot Nothing Then
                m_oSplitDocMergedCodes = New List(Of String)(oDocumentWrapper.SplitDocMergedCodes)
            Else
                m_oSplitDocMergedCodes = Nothing
            End If

            'Clean up objects
            oDocumentWrapper.Dispose()
            oDocumentWrapper = Nothing


            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            'Clean up objects
            If oDocumentWrapper Is Nothing Then
            Else
                oDocumentWrapper = Nothing
            End If

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    '******************************************************************************
    '        Function Name:  GetCurrenciesByBranch
    '******************************************************************************
    '           Created By:  Richard Taylor
    '           Created On:  10/03/2005
    '******************************************************************************
    '       Parameters Are:
    '                        (In)     - v_iSourceID                   - Integer  -
    '                        (In/Out) - r_vCurrencies                 - Variant  -
    '
    ' Return Value Type Is:  Long -
    '******************************************************************************
    ' Function Description:  This method calls bSIRInsuranceFile.Business to retieve
    '                        a list of currencies for a given Branch
    '******************************************************************************
    Public Function GetCurrenciesByBranch(ByVal v_iSourceID As Integer, ByRef r_vCurrencies As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oInsFile As bSIRInsuranceFile.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_iSourceID > 0 Then

                'Check to see if the system is Underwriting

                oInsFile = New bSIRInsuranceFile.Business
                'lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oInsFile, v_sClassName:="bSIRInsuranceFile.Business", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))
                'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    result = lReturn
                '    ' Log Error Message
                '    bPMFunc.LogMessage(sUsername:=gpmfunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrenciesByBranch Failed - Failed to create business object to bSIRInsuranceFile.Business.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesByBranch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                'End If

                lReturn = oInsFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = lReturn

                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrenciesByBranch Failed - Failed to create reference to bSIRInsuranceFile.Business.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesByBranch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                End If


                lReturn = oInsFile.RetrieveCurrenciesForBranch(iSourceID:=v_iSourceID, vReturnArray:=r_vCurrencies)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = lReturn

                    ' Log Error Message
                    bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrenciesByBranch Failed - Failed to retrieve list of currencies for Branch :- " & v_iSourceID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesByBranch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                End If


                oInsFile.Dispose()

                oInsFile = Nothing

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Clean up objects
            If oInsFile Is Nothing Then
            Else
                oInsFile = Nothing
            End If

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrenciesByBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesByBranch", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

            Return result
        End Try
    End Function

    '******************************************************************************
    '        Function Name:  GetSourceListForUser
    '******************************************************************************
    '           Created By:  Richard Taylor
    '           Created On:  10/03/2005
    '******************************************************************************
    '       Parameters Are:
    '                        (In)     - v_iUserID                   - Integer  -
    '                        (In/Out) - r_vSources                  - Variant  -
    '
    ' Return Value Type Is:  Long -
    '******************************************************************************
    ' Function Description:  This method calls bPMUser.Business to retieve
    '                        a list of Sources for a given User
    '******************************************************************************
    Public Function GetSourceListForUser(ByVal v_iUserID As Integer, ByRef r_vSources As Object) As Integer


        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oUser As Bpmuser.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_iUserID > 0 Then


                oUser = New Bpmuser.Business
                lReturn = oUser.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceListForUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Retrieve the branches for the given user.

                lReturn = oUser.GetUserSources(r_vSourceArray:=r_vSources, v_vUserID:=v_iUserID)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bPMUser.Business.GetUserSources Method Failed for User - " & v_iUserID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceListForUser", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If


                oUser.Dispose()

                oUser = Nothing

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Clean up objects
            If oUser Is Nothing Then
            Else
                oUser = Nothing
            End If

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSourceListForUser Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceListForUser", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

            Return result
        End Try
    End Function

    ' ************************************************************************
    '
    ' Function: GetQuoteAndSummariesByKey
    '
    ' Description: Retrieves Quote header data and List of associated risks.
    '              Called from the STS
    '
    ' ************************************************************************
    Public Function GetQuoteAndSummariesByKey(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPartyCnt As Integer, ByRef r_sGenderCode As String, ByRef r_dtDateOfBirth As String, ByRef r_blIsAnonymous As Integer, ByRef r_dtEffectiveDate As String, ByRef r_dtExpirationDate As String, ByRef r_sDescription As String, ByRef r_sProductCode As String, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sInsuranceFileRef As String, ByRef r_vResultDataset(,) As Object, ByRef r_vAdditionalDataArray As Object) As Integer
        Return GetQuoteAndSummariesByKey(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lPartyCnt:=r_lPartyCnt, r_sGenderCode:=r_sGenderCode, r_dtDateOfBirth:=r_dtDateOfBirth, r_blIsAnonymous:=r_blIsAnonymous, r_dtEffectiveDate:=r_dtEffectiveDate, r_dtExpirationDate:=r_dtExpirationDate, r_sDescription:=r_sDescription, r_sProductCode:=r_sProductCode, r_lInsuranceFolderCnt:=r_lInsuranceFolderCnt, r_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_sInsuranceFileRef:=r_sInsuranceFileRef, r_vResultDataset:=r_vResultDataset, r_vAdditionalDataArray:=r_vAdditionalDataArray, r_dtQuoteExpiryDate:="", r_sSubBranchCode:="", v_sScreenCode:="", r_blConsLeadAgntComm:=False, r_blConsSubAgntComm:=False)
    End Function
    Public Function GetQuoteAndSummariesByKey(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPartyCnt As Integer, ByRef r_sGenderCode As String, ByRef r_dtDateOfBirth As String, ByRef r_blIsAnonymous As Integer, ByRef r_dtEffectiveDate As String, ByRef r_dtExpirationDate As String, ByRef r_sDescription As String, ByRef r_sProductCode As String, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sInsuranceFileRef As String, ByRef r_vResultDataset(,) As Object, ByRef r_vAdditionalDataArray As Object, ByRef r_dtQuoteExpiryDate As String, ByRef r_sSubBranchCode As String, ByVal v_sScreenCode As String, ByRef r_blConsLeadAgntComm As Boolean, ByRef r_blConsSubAgntComm As Boolean) As Integer


        Dim result As Integer = 0
        Try
            Dim sSQL As String = ""
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim vQuoteArray(,) As Object
            Dim oQuotePolicy As New QuotePolicy
            Dim lCnt As Integer

            Dim sSurname, sForename, sPartyType, sAddress1, sAddress2, sAddress3, sAddress4, sPostcode, sEMail, sUserID, sPassword, sShortName, sResolvedName, sDOB As String

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(oQuotePolicy.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise QuotePolicy", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteAndSummariesByKey")
                Return result
            End If

            lReturn = CType(oQuotePolicy.GetQuoteDetails(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vQuoteArray:=vQuoteArray), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oQuotePolicy.Dispose()

                oQuotePolicy = Nothing

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QuotePolicy.GetQuoteDetails failed to get quote details for Insurance File - " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteAndSummariesByKey")
                Return result
            End If

            If Not Informations.IsArray(vQuoteArray) Then

                result = gPMConstants.PMEReturnCode.PMNotFound

                oQuotePolicy.Dispose()
                oQuotePolicy = Nothing

                Return result

            End If


            lCnt = vQuoteArray.GetUpperBound(1)


            r_lPartyCnt = CInt(vQuoteArray(ACInsuredCnt, lCnt))

            r_dtEffectiveDate = CStr(vQuoteArray(ACCover_start_date, lCnt))

            r_dtExpirationDate = CStr(vQuoteArray(ACExpiry_date, lCnt))

            r_sDescription = CStr(vQuoteArray(ACDescription, lCnt))

            r_lInsuranceFolderCnt = CInt(vQuoteArray(ACInsurance_folder_cnt, lCnt))

            r_lInsuranceFileCnt = CInt(vQuoteArray(ACInsurance_file_cnt, lCnt))

            r_sInsuranceFileRef = CStr(vQuoteArray(ACInsurance_ref, lCnt))

            r_dtQuoteExpiryDate = CStr(vQuoteArray(ACQuoteExpiryDate, lCnt))

            r_sSubBranchCode = CStr(vQuoteArray(ACSubBranchCode, lCnt))

            r_blConsLeadAgntComm = CBool(vQuoteArray(ACLeadAllowConsolidatedCommission, lCnt))

            r_blConsSubAgntComm = CBool(vQuoteArray(ACSubAllowConsolidatedCommission, lCnt))


            Dim oPMLookup As New BPMLOOKUP.Business

            lReturn = oPMLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn

                oPMLookup = Nothing

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise bPMLookup", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteAndSummariesByKey")

                Return result
            End If

            'if we're going direct then point lookup to SBO/S4U
            oPMLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            ' convert supplied ID to Code

            lReturn = oPMLookup.GetCodeFromID(v_sTableName:="Product", v_lID:=CInt(vQuoteArray(ACProduct_id, lCnt)), r_sCode:=r_sProductCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn

                oPMLookup.Dispose()
                oPMLookup = Nothing

                ' Log Error Message

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product code for product Id - " & CStr(vQuoteArray(ACProduct_id, lCnt)), vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteAndSummariesByKey")

                Return result
            End If

            r_sProductCode = r_sProductCode.TrimEnd()

            oPMLookup.Dispose()

            oPMLookup = Nothing

            lReturn = CType(oQuotePolicy.GetParty(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lPartyCnt:=r_lPartyCnt, r_sSurname:=sSurname, r_sForename:=sForename, r_sPartyType:=sPartyType, r_sAddress1:=sAddress1, r_sAddress2:=sAddress2, r_sAddress3:=sAddress3, r_sAddress4:=sAddress4, r_sPostcode:=sPostcode, r_sDOB:=sDOB, r_sEMail:=sEMail, r_sUserID:=sUserID, r_sPassword:=sPassword, r_sShortName:=sShortName, r_sResolvedName:=sResolvedName, r_sGenderCode:=r_sGenderCode), gPMConstants.PMEReturnCode)

            r_dtDateOfBirth = sDOB

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = lReturn

                oQuotePolicy.Dispose()

                oQuotePolicy = Nothing

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get party details Party Cnt - " & r_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteAndSummariesByKey")

                Return result
            End If

            oQuotePolicy.Dispose()

            oQuotePolicy = Nothing

            sSQL = "spu_STS_RiskSummariesByKey_sel"

            m_oDatabase.Parameters.Clear()

            ' lPolicyBinderID
            lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add parameter 'insurance_file_cnt' to m_oDatabase, value = " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteAndSummariesByKey")
                Return result
            End If

            ' ProductCode
            lReturn = m_oDatabase.Parameters.Add(sName:="ProductCode", vValue:=r_sProductCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add parameter 'ProductCode' to m_oDatabase, value = " & r_sProductCode, vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteAndSummariesByKey")
                Return result
            End If

            ' Call the SQL
            lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectRiskSummariesByKey", bStoredProcedure:=True, vResultArray:=r_vResultDataset)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQLAction failed to Get Quote And Summaries By Key for Insurance File - " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteAndSummariesByKey")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteAndSummariesByKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteAndSummariesByKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ************************************************************************
    '
    ' Function: GetQuoteAndSummariesByRef
    '
    ' Description: Retrieves Quote header data and List of associated risks.
    '              Called from the STS
    '
    ' ************************************************************************
    Public Function GetQuoteAndSummariesByRef(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sInsuranceFileRef As String, ByRef r_lPartyCnt As Integer, ByRef r_sGenderCode As String, ByRef r_dtDateOfBirth As String, ByRef r_blIsAnonymous As Integer, ByRef r_dtEffectiveDate As String, ByRef r_dtExpirationDate As String, ByRef r_sDescription As String, ByRef r_sProductCode As String, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sInsuranceFileRef As String, ByRef r_vResultDataset As Object, ByRef r_vAdditionalDataArray As Object) As Integer
        Return GetQuoteAndSummariesByRef(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sInsuranceFileRef:=v_sInsuranceFileRef, r_lPartyCnt:=r_lPartyCnt, r_sGenderCode:=r_sGenderCode, r_dtDateOfBirth:=r_dtDateOfBirth, r_blIsAnonymous:=r_blIsAnonymous, r_dtEffectiveDate:=r_dtEffectiveDate, r_dtExpirationDate:=r_dtExpirationDate, r_sDescription:=r_sDescription, r_sProductCode:=r_sProductCode, r_lInsuranceFolderCnt:=r_lInsuranceFolderCnt, r_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_sInsuranceFileRef:=r_sInsuranceFileRef, r_vResultDataset:=r_vResultDataset, r_vAdditionalDataArray:=r_vAdditionalDataArray, r_dtQuoteExpiryDate:="", r_sSubBranchCode:="", v_sScreenCode:="", r_blConsLeadAgntComm:=False, r_blConsSubAgntComm:=False)
    End Function
    Public Function GetQuoteAndSummariesByRef(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sInsuranceFileRef As String, ByRef r_lPartyCnt As Integer, ByRef r_sGenderCode As String, ByRef r_dtDateOfBirth As String, ByRef r_blIsAnonymous As Integer, ByRef r_dtEffectiveDate As String, ByRef r_dtExpirationDate As String, ByRef r_sDescription As String, ByRef r_sProductCode As String, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sInsuranceFileRef As String, ByRef r_vResultDataset As Object, ByRef r_vAdditionalDataArray As Object, ByRef r_dtQuoteExpiryDate As String, ByRef r_sSubBranchCode As String, ByVal v_sScreenCode As String, ByRef r_blConsLeadAgntComm As Boolean, ByRef r_blConsSubAgntComm As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = ""
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim vResultArray(,) As Object
            Dim oQuotePolicy As New QuotePolicy
            Dim oFindInsurance As bSIRFindInsurance.Form

            result = gPMConstants.PMEReturnCode.PMTrue

            oFindInsurance = New bSIRFindInsurance.Form()
            ' lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oFindInsurance, v_sClassName:="bSIRFindInsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))

            lReturn = oFindInsurance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageId:=m_iLanguageID, iCurrencyId:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            'RJG 04/12/2000 - Quit the function if the FindInsurance object failed to initialise
            If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_sGisDataModelCode", v_sGisDataModelCode)
                oDict.Add("v_sGisBusinessTypeCode", v_sGisBusinessTypeCode)
                oDict.Add("v_sInsuranceFileRef", v_sInsuranceFileRef)
                oDict.Add("r_lPartyCnt", r_lPartyCnt)
                oDict.Add("r_sGenderCode", r_sGenderCode)
                oDict.Add("r_dtEffectiveDate", r_dtEffectiveDate)
                oDict.Add("r_dtExpirationDate", r_dtExpirationDate)
                oDict.Add("r_sProductCode", r_sProductCode)
                oDict.Add("r_lInsuranceFolderCnt", r_lInsuranceFolderCnt)
                oDict.Add("r_lInsuranceFileCnt", r_lInsuranceFileCnt)
                oDict.Add("r_sInsuranceFileRef", r_sInsuranceFileRef)
                oDict.Add("r_dtQuoteExpiryDate", r_dtQuoteExpiryDate)
                oDict.Add("r_sSubBranchCode", r_sSubBranchCode)
                oDict.Add("v_sScreenCode", v_sScreenCode)
                gPMFunctions.LogMessageToFile(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "Failed to Initialise FindInsurance object.", ACApp, ACClass, "GetQuoteAndSummariesByRef", oDicParms:=oDict)
                Return result
            End If

            lReturn = oFindInsurance.FindQuote(r_vResultArray:=vResultArray, v_sQuoteRef:=v_sInsuranceFileRef)

            If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_sGisDataModelCode", v_sGisDataModelCode)
                oDict.Add("v_sGisBusinessTypeCode", v_sGisBusinessTypeCode)
                oDict.Add("v_sInsuranceFileRef", v_sInsuranceFileRef)
                oDict.Add("r_lPartyCnt", r_lPartyCnt)
                oDict.Add("r_sGenderCode", r_sGenderCode)
                oDict.Add("r_dtEffectiveDate", r_dtEffectiveDate)
                oDict.Add("r_dtExpirationDate", r_dtExpirationDate)
                oDict.Add("r_sProductCode", r_sProductCode)
                oDict.Add("r_lInsuranceFolderCnt", r_lInsuranceFolderCnt)
                oDict.Add("r_lInsuranceFileCnt", r_lInsuranceFileCnt)
                oDict.Add("r_sInsuranceFileRef", r_sInsuranceFileRef)
                oDict.Add("r_dtQuoteExpiryDate", r_dtQuoteExpiryDate)
                oDict.Add("r_sSubBranchCode", r_sSubBranchCode)
                oDict.Add("v_sScreenCode", v_sScreenCode)
                gPMFunctions.LogMessageToFile(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "m_oFindInsurance.FindQuote Failed.", ACApp, ACClass, "GetQuoteAndSummariesByRef", oDicParms:=oDict)
                oFindInsurance.Dispose()
                oFindInsurance = Nothing
                Return result
            End If

            oFindInsurance.Dispose()
            oFindInsurance = Nothing

            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oQuotePolicy.Dispose()

                oQuotePolicy = Nothing

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QuotePolicy.GetQuoteDetails failed to get quote details for Insurance Ref - " & v_sInsuranceFileRef, vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteAndSummariesByRef")
                Return result
            End If


            If Not Informations.IsArray(vResultArray) Then
                ' ERROR Failed to find any
            ElseIf vResultArray.GetUpperBound(1) > 0 Then
                ' ERROR Found too many
            End If


            r_lInsuranceFileCnt = CInt(vResultArray(0, 0))

            oQuotePolicy.Dispose()

            oQuotePolicy = Nothing

            ' Now we have the insurance file cnt call the existing method to retrieve
            ' the Quote and Summaries information

            lReturn = CType(GetQuoteAndSummariesByKey(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_lPartyCnt:=r_lPartyCnt, r_sGenderCode:=r_sGenderCode, r_dtDateOfBirth:=r_dtDateOfBirth, r_blIsAnonymous:=r_blIsAnonymous, r_dtEffectiveDate:=r_dtEffectiveDate, r_dtExpirationDate:=r_dtExpirationDate, r_sDescription:=r_sDescription, r_sProductCode:=r_sProductCode, r_lInsuranceFolderCnt:=r_lInsuranceFolderCnt, r_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_sInsuranceFileRef:=r_sInsuranceFileRef, r_vResultDataset:=r_vResultDataset, r_vAdditionalDataArray:=r_vAdditionalDataArray, r_dtQuoteExpiryDate:=r_dtQuoteExpiryDate, r_sSubBranchCode:=r_sSubBranchCode, v_sScreenCode:=v_sScreenCode, r_blConsLeadAgntComm:=r_blConsLeadAgntComm, r_blConsSubAgntComm:=r_blConsSubAgntComm), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteAndSummariesByRef failed to get details for Insurance File Cnt - " & r_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteAndSummariesByRef")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteAndSummariesByRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteAndSummariesByRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*******************************************************************************
    ' Name: UpdateQuote (Public)
    '
    ' Description: Updates the quote header
    '*******************************************************************************
    Public Function UpdateQuote(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dtCoverStart As Date, ByVal v_dtCoverEnd As Date, ByVal v_sDescription As String, ByVal v_vInsuredParties As Object, Optional ByVal v_lCurrencyID As Integer = -1, Optional ByVal v_lAnalysisCodeId As Integer = 0, Optional ByVal v_blConsLeadAgntComm As Boolean = False, Optional ByVal v_blConsSubAgntComm As Boolean = False) As Integer

        'TODO handle InsuredParties

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateQuote"

        Dim kErrorCode As Integer = gPMConstants.PMEReturnCode.PMBackOfficeError
        Dim oInsuranceFileServices As bSIRInsuranceFile.Services

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the InsuranceFile.Services object

            oInsuranceFileServices = New bSIRInsuranceFile.Services
            'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oInsuranceFileServices, v_sClassName:="bSIRInsuranceFile.Services", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    result = m_lReturn
            '    ' Log Error Message
            '    bPMFunc.LogMessage(sUsername:=gpmfunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateQuote Failed - Failed to create business object to bSIRInsuranceFile.Services.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesByBranch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            'End If
            m_lReturn = oInsuranceFileServices.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to create bSIRInsuranceFile.Services business object.")
            End If

            'Set the insurance file to work upon.

            oInsuranceFileServices.InsuranceFileCnt = v_lInsuranceFileCnt

            m_lReturn = oInsuranceFileServices.GetDetails

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to get Policy details. InsuranceFile.Services.GetDetails failed.")
            End If

            ' Set the properties that are changing on Insurance File

            oInsuranceFileServices.AnalysisCodeId = v_lAnalysisCodeId

            oInsuranceFileServices.CoverStartDate = v_dtCoverStart

            oInsuranceFileServices.ExpiryDate = v_dtCoverEnd

            oInsuranceFileServices.RenewalDate = v_dtCoverEnd

            oInsuranceFileServices.InsuranceFolderDescription = v_sDescription

            If v_lCurrencyID <> -1 Then

                oInsuranceFileServices.CurrencyID = v_lCurrencyID
            End If

            If Not False Then

                oInsuranceFileServices.LeadAgentAllowCommission = If(v_blConsLeadAgntComm, 1, 0)
            End If

            If Not False Then

                oInsuranceFileServices.SubAgentAllowCommission = If(v_blConsSubAgntComm, 1, 0)
            End If

            ' Update the InsuranceFile quote header

            m_lReturn = oInsuranceFileServices.UpdatePolicy
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to update Policy header. InsuranceFile.Services.UpdatePolicy failed.")
            End If


        Catch ex As Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

            If Not (oInsuranceFileServices Is Nothing) Then

                oInsuranceFileServices.Dispose()
                oInsuranceFileServices = Nothing
            End If


        End Try
        Return result
    End Function

    '*******************************************************************************
    ' Function: GetHeaderAndSummariesByKey
    '
    ' Description: Retrieves Policy header data and List of associated risks.
    '              Called from the STS
    '
    ' History: PW280904 - created
    '*******************************************************************************
    Public Function GetHeaderAndSummariesByKey(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPartyCnt As Integer, ByRef r_sEffectiveDate As String, ByRef r_sExpirationDate As String, ByRef r_sInceptionDate As String, ByRef r_sDescription As String, ByRef r_sProductCode As String, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sInsuranceFileRef As String, ByRef r_lInsuranceFileVer As Integer, ByRef r_vResultDataset(,) As Object, ByRef r_vAdditionalDataArray As Object, ByRef r_vInsuredParties(,) As Object, ByRef r_sQuoteExpiryDate As String, ByRef r_sSubBranchCode As String, ByRef r_sInsuranceFileTypeCode As String, ByRef r_sInsuranceFileStatusCode As String, ByRef r_sPaymentMethodCode As String, Optional ByRef r_sCurrencyCode As String = "", Optional ByRef r_blConsLeadAgntComm As Boolean = False, Optional ByRef r_blConsSubAgntComm As Boolean = False, Optional ByRef r_blContactuserKey As Integer = 0, Optional ByRef r_bPutOnNextMTAInstalmentRenewal As Boolean = False, Optional ByRef r_bAnniversaryCopy As Boolean = False, Optional ByRef r_lRenewalDayNumber As Integer = 0, Optional ByRef r_bReferredAtRenewal As Boolean = False) As Integer



        Dim result As Integer = 0
        Const kMethodName As String = "GetHeaderAndSummariesByKey"

        Dim kErrorCode As Integer = gPMConstants.PMEReturnCode.PMBackOfficeError

        Dim vInsuredParties(,) As Object
        Dim oInsuranceFileServices As bSIRInsuranceFile.Services
        Dim oInsuranceFileBusiness As bSIRInsuranceFile.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the InsuranceFile.Services object

            oInsuranceFileServices = New bSIRInsuranceFile.Services
            'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oInsuranceFileServices, v_sClassName:="bSIRInsuranceFile.Services", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    result = m_lReturn
            '    ' Log Error Message
            '    bPMFunc.LogMessage(sUsername:=gpmfunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHeaderAndSummariesByKey Failed - Failed to create business object to bSIRInsuranceFile.Services.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesByBranch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            'End If

            m_lReturn = oInsuranceFileServices.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to create bSIRInsuranceFile.Services business object.")
            End If

            '' Create the InsuranceFile.Business object

            oInsuranceFileBusiness = New bSIRInsuranceFile.Business

            'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oInsuranceFileBusiness, v_sClassName:="bSIRInsuranceFile.Business", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    result = m_lReturn
            '    ' Log Error Message
            '    bPMFunc.LogMessage(sUsername:=gpmfunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHeaderAndSummariesByKey Failed - Failed to create business object to bSIRInsuranceFile.Business.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesByBranch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            'End If

            m_lReturn = oInsuranceFileBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to create bSIRInsuranceFile.Business object.")
            End If

            ' Get the policy header for the passed insurance file cnt

            oInsuranceFileServices.InsuranceFileCnt = v_lInsuranceFileCnt

            m_lReturn = oInsuranceFileServices.GetDetails

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to get Policy details. InsuranceFile.Services.GetDetails failed.")
            End If

            ' Return the details

            r_lPartyCnt = oInsuranceFileServices.InsuranceHolderCnt

            r_sEffectiveDate = oInsuranceFileServices.CoverStartDate

            r_sExpirationDate = oInsuranceFileServices.ExpiryDate

            r_sInceptionDate = oInsuranceFileServices.InceptionDate

            r_sDescription = gPMFunctions.NullToString(oInsuranceFileServices.InsuranceFolderDescription)

            r_sProductCode = oInsuranceFileServices.Product

            r_lInsuranceFolderCnt = oInsuranceFileServices.InsuranceFolderCnt

            r_lInsuranceFileCnt = oInsuranceFileServices.InsuranceFileCnt

            r_sInsuranceFileRef = oInsuranceFileServices.InsuranceRef

            r_lInsuranceFileVer = oInsuranceFileServices.PolicyVersion

            r_sSubBranchCode = gPMFunctions.NullToString(oInsuranceFileServices.SubBranch)

            r_sInsuranceFileTypeCode = gPMFunctions.NullToString(oInsuranceFileServices.InsuranceFileType)

            r_sInsuranceFileStatusCode = gPMFunctions.NullToString(oInsuranceFileServices.InsuranceFileStatus)

            r_sPaymentMethodCode = gPMFunctions.NullToString(oInsuranceFileServices.PaymentMethod)

            r_sCurrencyCode = gPMFunctions.NullToString(oInsuranceFileServices.CurrencyCode)

            r_blConsLeadAgntComm = (oInsuranceFileServices.LeadAgentAllowCommission = 1)

            r_blConsSubAgntComm = (oInsuranceFileServices.SubAgentAllowCommission = 1)


            If Convert.IsDBNull(oInsuranceFileServices.QuoteExpiryDate) Or Informations.IsNothing(oInsuranceFileServices.QuoteExpiryDate) Then
                r_sQuoteExpiryDate = ""
            Else

                r_sQuoteExpiryDate = oInsuranceFileServices.QuoteExpiryDate
            End If
            'Begin WPR36
            r_bPutOnNextMTAInstalmentRenewal = gPMFunctions.ToSafeBoolean(oInsuranceFileServices.PutOnNextInstalmentRenewal)
            r_bAnniversaryCopy = gPMFunctions.ToSafeBoolean(oInsuranceFileServices.AnniversaryCopy)
            r_lRenewalDayNumber = gPMFunctions.ToSafeInteger(oInsuranceFileServices.RenewalDayNumber)
            'End WPR36
            'WPR73-74
            r_blContactuserKey = oInsuranceFileServices.ContactuserId
            r_bReferredAtRenewal = ToSafeBoolean(oInsuranceFileServices.IsReferredAtRenewal)

            ' Get the Insured Parties

            ' Get the Insured Parties
            m_lReturn = oInsuranceFileBusiness.GetPolicyClient(v_lInsuranceFolderCnt:=r_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lPartyCnt:=r_lPartyCnt, r_vResultArray:=vInsuredParties)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to get Insured Parties. InsuranceFile.Business.GetPolicyClient failed.")
            End If

            ' Convert Insured Parties array into the structure STS wants
            If Informations.IsArray(vInsuredParties) Then

                ReDim r_vInsuredParties(5, vInsuredParties.GetUpperBound(1))

                For i As Integer = 0 To vInsuredParties.GetUpperBound(1)


                    r_vInsuredParties(0, i) = vInsuredParties(0, i)


                    r_vInsuredParties(1, i) = vInsuredParties(1, i)


                    r_vInsuredParties(2, i) = vInsuredParties(2, i)

                    r_vInsuredParties(3, i) = vInsuredParties(3, i)
                    r_vInsuredParties(4, i) = vInsuredParties(4, i)
                    r_vInsuredParties(5, i) = vInsuredParties(5, i)

                Next
            End If

            ' Get the risk details...

            ' Clear Database parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add InsuranceFileCnt parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "Failed to Add parameter 'insurance_file_cnt' to m_oDatabase, value = " & v_lInsuranceFileCnt)
            End If

            ' Add ProductCode parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ProductCode", vValue:=r_sProductCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "Failed to Add parameter 'ProductCode' to m_oDatabase, value = " & r_sProductCode)
            End If

            ' Call the SQL

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_STS_RiskSummariesByKey_sel", sSQLName:="SelectRiskSummariesByKey", bStoredProcedure:=True, vResultArray:=r_vResultDataset)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "SQLAction failed to Get Quote And Summaries By Key for Insurance File - " & v_lInsuranceFileCnt)
            End If

            '
            ' Error handler
            '
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHeaderAndSummariesByKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHeaderAndSummariesByKey", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            '
            ' Common exit point
            '
        Finally

            ' Kill off the InsuranceFile.Services object
            If Not (oInsuranceFileServices Is Nothing) Then

                oInsuranceFileServices.Dispose()
                oInsuranceFileServices = Nothing
            End If

            ' Kill off the InsuranceFile.Business object
            If Not (oInsuranceFileBusiness Is Nothing) Then

                oInsuranceFileBusiness.Dispose()
                oInsuranceFileBusiness = Nothing
            End If


        End Try
        Return result
    End Function



    ' ************************************************************************
    '
    ' Function: GetHeaderAndSummariesByRef
    '
    ' Description: Retrieves Quote header data and List of associated risks.
    '              Called from the STS
    '
    ' ************************************************************************

    Public Function GetHeaderAndSummariesByRef(ByVal v_sInsuranceFileRef As String, ByRef r_lPartyCnt As Integer, ByRef r_sEffectiveDate As String, ByRef r_sExpirationDate As String, ByRef r_sInceptionDate As String, ByRef r_sDescription As String, ByRef r_sProductCode As String, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sInsuranceFileRef As String, ByRef r_lInsuranceFileVer As Integer, ByRef r_vResultDataset As Object, ByRef r_vAdditionalDataArray As Object, ByRef r_vInsuredParties As Object, ByRef r_sQuoteExpiryDate As String, ByRef r_sSubBranchCode As String, ByRef r_sInsuranceFileTypeCode As String, ByRef r_sInsuranceFileStatusCode As String, ByRef r_sPaymentMethodCode As String, Optional ByRef r_sCurrencyCode As String = "", Optional ByRef r_blConsLeadAgntComm As Boolean = False, Optional ByRef r_blConsSubAgntComm As Boolean = False, Optional ByRef r_blContactuserKey As Integer = 0, Optional ByRef r_bPutOnNextMTAInstalmentRenewal As Boolean = False, Optional ByRef r_bAnniversaryCopy As Boolean = False, Optional ByRef r_lRenewalDayNumber As Integer = 0, Optional ByRef r_bReferredAtRenewal As Boolean = False) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = ""
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim vResultArray(,) As Object
            Dim oQuotePolicy As New QuotePolicy
            Dim oFindInsurance As bSIRFindInsurance.Form

            result = gPMConstants.PMEReturnCode.PMTrue

            oFindInsurance = New bSIRFindInsurance.Form()
            ' lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oFindInsurance, v_sClassName:="bSIRFindInsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))

            lReturn = oFindInsurance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageId:=m_iLanguageID, iCurrencyId:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            'RJG 04/12/2000 - Quit the function if the FindInsurance object failed to initialise
            If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_sInsuranceFileRef", v_sInsuranceFileRef)
                oDict.Add("r_lPartyCnt", r_lPartyCnt)
                oDict.Add("r_sEffectiveDate", r_sEffectiveDate)
                oDict.Add("r_sExpirationDate", r_sExpirationDate)
                oDict.Add("r_sInceptionDate", r_sInceptionDate)
                oDict.Add("r_sProductCode", r_sProductCode)
                oDict.Add("r_lInsuranceFolderCnt", r_lInsuranceFolderCnt)
                oDict.Add("r_lInsuranceFileCnt", r_lInsuranceFileCnt)
                oDict.Add("r_sInsuranceFileRef", r_sInsuranceFileRef)
                oDict.Add("r_sQuoteExpiryDate", r_sQuoteExpiryDate)
                oDict.Add("r_sSubBranchCode", r_sSubBranchCode)
                oDict.Add("r_sInsuranceFileTypeCode", r_sInsuranceFileTypeCode)
                oDict.Add("r_sInsuranceFileStatusCode", r_sInsuranceFileStatusCode)
                oDict.Add("r_sPaymentMethodCode", r_sPaymentMethodCode)
                oDict.Add("r_sCurrencyCode", r_sCurrencyCode)
                gPMFunctions.LogMessageToFile(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "Failed to Initialise FindInsurance object.", ACApp, ACClass, "GetHeaderAndSummariesByRef", oDicParms:=oDict)
                Return result
            End If

            lReturn = oFindInsurance.FindQuote(r_vResultArray:=vResultArray, v_sQuoteRef:=v_sInsuranceFileRef)

            If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_sInsuranceFileRef", v_sInsuranceFileRef)
                oDict.Add("r_lPartyCnt", r_lPartyCnt)
                oDict.Add("r_sEffectiveDate", r_sEffectiveDate)
                oDict.Add("r_sExpirationDate", r_sExpirationDate)
                oDict.Add("r_sInceptionDate", r_sInceptionDate)
                oDict.Add("r_sProductCode", r_sProductCode)
                oDict.Add("r_lInsuranceFolderCnt", r_lInsuranceFolderCnt)
                oDict.Add("r_lInsuranceFileCnt", r_lInsuranceFileCnt)
                oDict.Add("r_sInsuranceFileRef", r_sInsuranceFileRef)
                oDict.Add("r_sQuoteExpiryDate", r_sQuoteExpiryDate)
                oDict.Add("r_sSubBranchCode", r_sSubBranchCode)
                oDict.Add("r_sInsuranceFileTypeCode", r_sInsuranceFileTypeCode)
                oDict.Add("r_sInsuranceFileStatusCode", r_sInsuranceFileStatusCode)
                oDict.Add("r_sPaymentMethodCode", r_sPaymentMethodCode)
                oDict.Add("r_sCurrencyCode", r_sCurrencyCode)
                gPMFunctions.LogMessageToFile(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "m_oFindInsurance.FindQuote Failed.", ACApp, ACClass, "GetHeaderAndSummariesByRef", oDicParms:=oDict)
                oFindInsurance.Dispose()
                oFindInsurance = Nothing
                Return result
            End If

            oFindInsurance.Dispose()
            oFindInsurance = Nothing

            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oQuotePolicy.Dispose()

                oQuotePolicy = Nothing

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRFindInsurance.FindQuote failed to get quote details for Insurance Ref - " & r_sInsuranceFileRef, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHeaderAndSummariesByRef")
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                lReturn = gPMConstants.PMEReturnCode.PMNotFound
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRFindInsurance.FindQuote failed to get quote details for Insurance Ref - " & r_sInsuranceFileRef, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHeaderAndSummariesByRef")
                Return result
                'ElseIf UBound(vResultArray, 2) > 0 Then
                ' ERROR Found too many
                'lReturn = PMBusinessRuleError
                'LogMessageFile _
                'iType:=PMLogOnError, _
                'sMsg:="bSIRFindInsurance.FindQuote returned multiple records when only one was expected. Insurance Ref - " & r_sInsuranceFileRef, _
                'vApp:=ACApp, _
                'vClass:=ACClass, _
                'vMethod:="GetHeaderAndSummariesByRef"
                ' Exit Function
            End If

            'r_lInsuranceFileCnt = CLng(vResultArray(0, 0))


            r_lInsuranceFileCnt = CInt(vResultArray(0, vResultArray.GetUpperBound(1)))
            oQuotePolicy.Dispose()

            oQuotePolicy = Nothing

            ' Now we have the insurance file cnt call the existing method to retrieve
            ' the Quote and Summaries information


            lReturn = CType(GetHeaderAndSummariesByKey(v_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_lPartyCnt:=r_lPartyCnt, r_sEffectiveDate:=r_sEffectiveDate, r_sExpirationDate:=r_sExpirationDate, r_sInceptionDate:=r_sInceptionDate, r_sDescription:=r_sDescription, r_sProductCode:=r_sProductCode, r_lInsuranceFolderCnt:=r_lInsuranceFolderCnt, r_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_sInsuranceFileRef:=r_sInsuranceFileRef, r_lInsuranceFileVer:=r_lInsuranceFileVer, r_vResultDataset:=r_vResultDataset, r_vAdditionalDataArray:=r_vAdditionalDataArray, r_vInsuredParties:=r_vInsuredParties, r_sQuoteExpiryDate:=r_sQuoteExpiryDate, r_sSubBranchCode:=r_sSubBranchCode, r_sInsuranceFileTypeCode:=r_sInsuranceFileTypeCode, r_sInsuranceFileStatusCode:=r_sInsuranceFileStatusCode, r_sPaymentMethodCode:=r_sPaymentMethodCode, r_sCurrencyCode:=r_sCurrencyCode, r_blConsLeadAgntComm:=r_blConsLeadAgntComm, r_blConsSubAgntComm:=r_blConsSubAgntComm, r_blContactuserKey:=r_blContactuserKey, r_bPutOnNextMTAInstalmentRenewal:=r_bPutOnNextMTAInstalmentRenewal, r_bAnniversaryCopy:=r_bAnniversaryCopy, r_lRenewalDayNumber:=r_lRenewalDayNumber, r_bReferredAtRenewal:=r_bReferredAtRenewal), gPMConstants.PMEReturnCode)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteAndSummariesByRef failed to get details for Insurance File Cnt - " & r_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHeaderAndSummariesByRef")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHeaderAndSummariesByRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHeaderAndSummariesByRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function Login(ByVal v_sUsername As String, ByVal v_sPassword As String, ByRef r_lAgentCnt As Integer, ByRef r_dtPasswordChange As Date, ByRef r_dtLastlogin As Date, ByRef r_sAgentName As String, ByRef r_sFullUserName As String, ByRef r_sEmailAddress As String, ByRef r_vSourceList(,) As Object, ByRef r_lAgentType As Integer) As Integer
        Return Login(v_sUsername:=v_sUsername, v_sPassword:=v_sPassword, r_lAgentCnt:=r_lAgentCnt, r_dtPasswordChange:=r_dtPasswordChange, r_dtLastlogin:=r_dtLastlogin, r_sAgentName:=r_sAgentName, r_sFullUserName:=r_sFullUserName, r_sEmailAddress:=r_sEmailAddress, r_vSourceList:=r_vSourceList, r_lAgentType:=r_lAgentType, r_blAllowConsolidatedCommission:=False)
    End Function
    Public Function Login(ByVal v_sUsername As String, ByVal v_sPassword As String, ByRef r_lAgentCnt As Integer, ByRef r_dtPasswordChange As Date, ByRef r_dtLastlogin As Date, ByRef r_sAgentName As String, ByRef r_sFullUserName As String, ByRef r_sEmailAddress As String, ByRef r_vSourceList(,) As Object, ByRef r_lAgentType As Integer, ByRef r_blAllowConsolidatedCommission As Boolean) As Integer
        Dim result As Integer = 0
        Dim oSIRPartyAgent As bSIRPartyAG.Business

        Dim sLoggedOnAt As String = ""
        Dim oPMUserBusiness As Bpmuser.Business
        Dim oPMLockForm As bpmlock.Form
        Dim sPasswordEncrypted As String = ""
        Dim lUserID As Integer
        Dim bTransactionInProgress As Boolean
        Dim vSourceListArray(,) As Object

        Const kMethodName As String = "Login"
        ' Use PMBackOfficeError for now as this is what stateless classes use
        ' may be changed later (should we add vbObjectError?) (ref: RFC)

        Dim kErrorCode As Integer = gPMConstants.PMEReturnCode.PMBackOfficeError

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the bPMUser.business object
            oPMUserBusiness = New Bpmuser.Business

            ' Create an instance of the bPMLock.Form object
            oPMLockForm = New bpmlock.Form

            'Encrypt the password
            m_lReturn = bPMFunc.Encrypt(sPassword:=v_sPassword, sEncryptedPassword:=sPasswordEncrypted)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to encrypt the password.")
            End If

            ' Start a database transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", SQLBeginTrans failed.")
            Else
                bTransactionInProgress = True
            End If

            ' Generate a unique 'logged on at' string for the user
            sLoggedOnAt = "AGENTPACKAGE-" & DateTime.Now.ToString("yyyyMMddHHMMssmm")

            'Check Logon for the user and password passed in.

            m_lReturn = oPMUserBusiness.CheckLogon(sCheckUsername:=v_sUsername, sCheckPassword:=sPasswordEncrypted, dtEffectiveFrom:=DateTime.Now, iLanguageID:=m_iLanguageID, lPartyCnt:=r_lAgentCnt, vUserId:=lUserID, sLoggedOnAtClient:=sLoggedOnAt)

            ' Process return value
            result = m_lReturn
            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' CheckLogon OK, so do nothing
                Case gPMConstants.PMEReturnCode.PMIncorrectUsername
                    Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "Failed to login. Incorrect username - " & v_sUsername)
                Case gPMConstants.PMEReturnCode.PMIncorrectPassword
                    Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "Failed to login. Incorrect password for username - " & v_sUsername)
                Case gPMConstants.PMEReturnCode.PMLoggedOnElsewhere
                    ' User is logged on elsewhere, so log them out

                    m_lReturn = oPMUserBusiness.Logoff(v_sUsername:=v_sUsername)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "bPMUser.Business.Logoff Method Failed for User - " & v_sUsername)
                    End If

                    ' Clear all locks for the user

                    m_lReturn = oPMLockForm.UnLockAllForUser(lUserID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "bPMLock.Form.UnlockAllForUser Method Failed for User - " & v_sUsername)
                    End If

                    'Check Logon for the user and password passed in, again

                    m_lReturn = oPMUserBusiness.CheckLogon(sCheckUsername:=v_sUsername, sCheckPassword:=sPasswordEncrypted, dtEffectiveFrom:=DateTime.Now, iLanguageID:=m_iLanguageID, lPartyCnt:=r_lAgentCnt, vUserId:=lUserID, sLoggedOnAtClient:=sLoggedOnAt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "Failed to login. Checklogon failed at second attempt. Username - " & v_sUsername)
                    End If

                Case Else
                    ' Unknown error
                    Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "bPMUser.Business.CheckLogon Method Failed for username - " & v_sUsername)
            End Select

            'Get the details of the user

            m_lReturn = oPMUserBusiness.GetDetails(vUserId:=lUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "bPMUser.Business.GetDetails Method Failed for User - " & lUserID)
            End If

            'Get required details from the User record
            ' PN27238 - Get email address from User (instead of Agent)

            m_lReturn = oPMUserBusiness.GetNext(vUserId:=lUserID, vPasswordChangeDate:=r_dtPasswordChange, vLastLogin:=r_dtLastlogin, vFullName:=r_sFullUserName, vEmailAddress:=r_sEmailAddress)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "bPMUser.Business.GetNext Method Failed for Userid - " & lUserID)
            End If

            Dim lAllowConComm As Integer
            Dim blAllowConComm As Boolean
            If r_lAgentCnt <> 0 Then


                ' Create an instance of the bSIRPartyAG.Business object
                oSIRPartyAgent = New bSIRPartyAG.Business
                'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oSIRPartyAgent, v_sClassName:="bSIRPartyAG.Business", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))

                'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to get instance of bSIRPartyAG.Business.")
                'End If

                ' Get the required Agent details from the Party record

                oSIRPartyAgent.PartyCnt = r_lAgentCnt

                m_lReturn = oSIRPartyAgent.GetDetails(vPartyCnt:=r_lAgentCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "bSIRPartyAG.Business.GetDetails Method Failed for Agent. PartyCnt - " & r_lAgentCnt)
                End If

                m_lReturn = oSIRPartyAgent.GetNext(vPartyCnt:=r_lAgentCnt, vName:=r_sAgentName, vPartyAgentTypeID:=r_lAgentType) ', r_blAllowConsolidatedCommission:=r_blAllowConsolidatedCommission)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "bSIRPartyAG.Business.GetNext Method Failed for Agent. PartyCnt - " & r_lAgentCnt)
                End If

            End If

            ' Get the array of branches for the user

            m_lReturn = oPMUserBusiness.GetUserSources(v_vUserID:=lUserID, r_vSourceArray:=vSourceListArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "bPMUser.Business.GetUserSources Method Failed for User - " & v_sUsername)
            End If

            ' Restructure the source array to contain what the STS required
            If Informations.IsArray(vSourceListArray) Then

                ReDim r_vSourceList(2, vSourceListArray.GetUpperBound(1) - 1)

                For i As Integer = 1 To vSourceListArray.GetUpperBound(1)


                    r_vSourceList(0, i - 1) = vSourceListArray(1, i)


                    r_vSourceList(1, i - 1) = vSourceListArray(2, i)


                    r_vSourceList(2, i - 1) = vSourceListArray(3, i)
                Next
            End If

            ' Log the user on

            m_lReturn = oPMUserBusiness.Logon(v_sUsername:=v_sUsername, v_sLoggedOnAtClient:=sLoggedOnAt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "bPMUser.Business.Logon Method Failed for User - " & v_sUsername)
            End If

            '
            ' Error Routine
            '
        Catch ex As Exception

            ' Rollback the database transaction
            If bTransactionInProgress Then
                m_oDatabase.SQLRollbackTrans()
                bTransactionInProgress = False
            End If

            ' Error.
            If result = gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
            End If

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Login Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Login", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            '
            ' Common exit point
            '
        Finally

            ' Commit the database transaction
            If bTransactionInProgress Then
                result = m_oDatabase.SQLCommitTrans()
                bTransactionInProgress = False
            End If

            ' Kill off the bPMUser.Business object
            If Not (oPMUserBusiness Is Nothing) Then

                oPMUserBusiness.Dispose()
                oPMUserBusiness = Nothing
            End If

            ' Kill off the bSIRPartyAG.Business object
            If Not (oSIRPartyAgent Is Nothing) Then

                oSIRPartyAgent.Dispose()
                oSIRPartyAgent = Nothing
            End If

            ' Kill off the bPMLock.Form object
            If Not (oPMLockForm Is Nothing) Then

                oPMLockForm.Dispose()
                oPMLockForm = Nothing
            End If


        End Try
        Return result
    End Function


    '*******************************************************************************
    ' Name: Logoff (Public)
    '
    ' Description: Calls the Logoff routines for the user.
    '
    ' History: PW081004 - created
    '*******************************************************************************
    Public Function Logoff(ByVal v_sUsername As String) As Integer

        Dim result As Integer = 0
        Dim oPMUserBusiness As Bpmuser.Business
        Dim bTransactionInProgress As Boolean

        Const kMethodName As String = "Logoff"
        ' Use PMBackOfficeError for now as this is what stateless classes use
        ' may be changed later (should we add vbObjectError?) (ref: RFC)

        Dim kErrorCode As Integer = gPMConstants.PMEReturnCode.PMBackOfficeError

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the bPMUser.business object

            oPMUserBusiness = New Bpmuser.Business
            m_lReturn = oPMUserBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to get instance of bPMUser.Business.")
            End If

            ' Start a database transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", SQLBeginTrans failed.")
            Else
                bTransactionInProgress = True
            End If

            ' Log the user off

            m_lReturn = oPMUserBusiness.Logoff(v_sUsername:=v_sUsername)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "bPMUser.Business.Logoff Method Failed for User - " & v_sUsername)
            End If

            '
            ' Error Routine
            '
        Catch ex As Exception

            ' Rollback the database transaction
            If bTransactionInProgress Then
                m_oDatabase.SQLRollbackTrans()
                bTransactionInProgress = False
            End If

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Logoff Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Logoff", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            '
            ' Common exit point
            '
        Finally

            ' Commit the database transaction
            If bTransactionInProgress Then
                result = m_oDatabase.SQLCommitTrans()
                bTransactionInProgress = False
            End If

            ' Kill off the bPMUser.Business object
            If Not (oPMUserBusiness Is Nothing) Then

                oPMUserBusiness.Dispose()
                oPMUserBusiness = Nothing
            End If


        End Try
        Return result
    End Function



    '*******************************************************************************
    ' Name: ChangePassword (Public)
    '
    ' Description: This method validates the current password for the user
    '              then updates it with the new one.
    '
    ' History: PW081004 - created
    '*******************************************************************************
    Public Function ChangePassword(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_sNewPassword As String) As Integer

        Dim result As Integer = 0
        Dim oPMUserBusiness As Bpmuser.Business
        Dim bTransactionInProgress As Boolean
        Dim sPasswordEncrypted As String = ""
        Dim lUserID, lPartyCnt As Integer
        Dim iLanguageID As Integer
        Dim bStrongPassword As Boolean = True
        Dim bIsValid As Boolean = False

        Const kMethodName As String = "ChangePassword"
        ' Use PMBackOfficeError for now as this is what stateless classes use
        ' may be changed later (should we add vbObjectError?) (ref: RFC)

        Dim kErrorCode As Integer = gPMConstants.PMEReturnCode.PMBackOfficeError

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the bPMUser.business object

            oPMUserBusiness = New Bpmuser.Business
            m_lReturn = oPMUserBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to get instance of bPMUser.Business.")
            End If

            ' Start a database transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", SQLBeginTrans failed.")
            Else
                bTransactionInProgress = True
            End If

            'Check Logon for the user and password passed in.

            m_lReturn = oPMUserBusiness.CheckLogon(sCheckUsername:=v_sUsername, sCheckPassword:=v_sPassword, dtEffectiveFrom:=DateTime.Now, iLanguageID:=iLanguageID, lPartyCnt:=lPartyCnt, vUserId:=lUserID)

            ' Process return value
            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' CheckLogon OK, so do nothing
                Case gPMConstants.PMEReturnCode.PMIncorrectUsername
                    Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "Checklogon failed. Incorrect username - " & v_sUsername)
                Case gPMConstants.PMEReturnCode.PMIncorrectPassword
                    Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "Checklogon failed. Incorrect current password for username - " & v_sUsername)
                Case gPMConstants.PMEReturnCode.PMUserTemporaryPassword, gPMConstants.PMEReturnCode.PMUserPasswordExpired, gPMConstants.PMEReturnCode.PMUserWeakPassword
                    'Do nothing for change password as user may be forced to Change password due to the given reasons
                Case Else
                    ' Unknown error
                    Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "bPMUser.Business.CheckLogon Method Failed for username - " & v_sUsername)
            End Select

            'Get the details of the user

            m_lReturn = oPMUserBusiness.GetDetails(vUserId:=lUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", " + "bPMUser.Business.GetDetails Method Failed for User - " & lUserID)
            End If

            'Encrypt the new password
            m_lReturn = bPMFunc.Encrypt(sPassword:=v_sNewPassword, sEncryptedPassword:=gPMFunctions.ToSafeString(sPasswordEncrypted))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to encrypt the new password.")
            End If

            m_lReturn = gPMFunctions.IsStrongPassword(v_sUsername:=v_sUsername, v_iUserID:=lUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, sPasswordString:=v_sNewPassword, bIsstrongPassword:=bStrongPassword, v_iSourceID:=m_iSourceID)

            If bStrongPassword = False Then
                Return PMEReturnCode.PMUserWeakPassword
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = IsReusedPassword(iUser_Id:=lUserID, sNewPassword:=v_sNewPassword.Trim(), IsValid:=bIsValid)

            If Not bIsValid Then
                Return gPMConstants.PMEReturnCode.PMReusedPassword
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMReusedPassword
            End If

            ' Update the password
            m_lReturn = oPMUserBusiness.EditUpdate(lRow:=1, vUserId:=lUserID, vUsername:=v_sUsername, vPassword:=sPasswordEncrypted, vPasswordChangeDate:=DateTime.Now, vIsTempPassword:=False)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", bPMUser.Business.EditUpdate Method Failed.")
            End If

            m_lReturn = oPMUserBusiness.Update
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", bPMUser.Business.Update Method Failed.")
            End If

            m_lReturn = oPMUserBusiness.UpdatePasswordHistory(iUser_Id:=lUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oPMUserBusiness = Nothing
                m_oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '
            ' Error Routine
            '
        Catch ex As Exception

            ' Rollback the database transaction
            If bTransactionInProgress Then
                m_oDatabase.SQLRollbackTrans()
                bTransactionInProgress = False
            End If

            ' Error.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
            Else
                result = gPMConstants.PMEReturnCode.PMError
            End If

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangePassword Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangePassword", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            '
            ' Common exit point
            '
        Finally

            ' Commit the database transaction
            If bTransactionInProgress Then
                result = m_oDatabase.SQLCommitTrans()
                bTransactionInProgress = False
            End If

            ' Kill off the bPMUser.Business object
            If Not (oPMUserBusiness Is Nothing) Then

                oPMUserBusiness.Dispose()
                oPMUserBusiness = Nothing
            End If


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CreateEvent
    '
    ' Description: Creates an event in the backoffice event log against the specified entity.
    '              If a document is passed in then the document is also archived in DME. The EventNotes,
    '              if passed in, need to be passed in as an array of 255 length strings.
    '
    ' History : Created
    ' ***************************************************************** '
    Public Function CreateEvent(ByVal v_lPartyCnt As Integer, ByVal v_sDescription As String, Optional ByVal v_sFilename As String = "", Optional ByVal v_lInsuranceFolderCnt As Integer = -1, Optional ByVal v_lInsuranceFileCnt As Integer = -1, Optional ByVal v_lClaimCnt As Integer = -1, Optional ByVal v_vEventNotes As Object = Nothing, Optional ByVal v_lEventLogSubjectId As Integer = -1, Optional ByVal v_sDocumentDescription As String = "") As Integer
        Dim result As Integer = 0
        Const PMBEventDocument As Integer = 10
        Const PMBEventClientNotes As Integer = 20
        Dim sOptionValue As String = ""
        Dim vDocNumber, lDocNumber As Integer
        Dim sDescription As String = ""
        Dim oEvent As bSIREvent.Business
        Dim oFreeFormatText As Object 'MKW 281003 PN7287 1.8.5 to 1.8.6 Catchup
        Dim lEventType, lEventCnt, lLBnd, lUBnd As Integer
        Dim vInsuranceFolderCnt As Integer
        Dim vInsuranceFileCnt As Integer
        Dim vClaimCnt As Integer
        Dim vEventLogSubjectId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lInsuranceFolderCnt < 1 Then

                vInsuranceFolderCnt = Nothing
            Else
                vInsuranceFolderCnt = v_lInsuranceFolderCnt
            End If

            If v_lInsuranceFileCnt < 1 Then

                vInsuranceFileCnt = Nothing
            Else
                vInsuranceFileCnt = v_lInsuranceFileCnt
            End If

            If v_lClaimCnt < 1 Then

                vClaimCnt = Nothing
            Else
                vClaimCnt = v_lClaimCnt
            End If

            If v_lEventLogSubjectId < 1 Then

                vEventLogSubjectId = Nothing
            Else
                vEventLogSubjectId = v_lEventLogSubjectId
            End If

            'Add it to FileMaster, returning the document cnt

            'First check if FileMaster is installed

            If v_sFilename <> "" Then

                m_lReturn = bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, 10, sOptionValue)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sOptionValue.Trim() <> "1") Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogError, "DocuMaster is not enabled", ACApp, ACClass, "CreateEvent", 0, "DocuMaster is not enabled")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'DN 08/12/01 - Archive the document with the input description
                m_lReturn = UpdateFileMaster(v_lPartyCnt, v_lInsuranceFolderCnt, v_lClaimCnt, v_sFilename, v_sDocumentDescription, lDocNumber)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                vDocNumber = lDocNumber
                lEventType = PMBEventDocument

            Else

                vDocNumber = Nothing
                lEventType = PMBEventClientNotes
            End If

            oEvent = New bSIREvent.Business

            m_lReturn = oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse


                bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogError, "Failed to initialise the bSirEvent.Business object", ACApp, ACClass, "CreateEvent", 0, "Failed to initialise the bSirEvent.Business object")

                Return result
            End If

            m_lReturn = oEvent.DirectAdd(vEventCnt:=lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt,
                                         vClaimCnt:=vClaimCnt, vDocumentCnt:=vDocNumber, vEventType:=lEventType, vEventLogSubjectId:=vEventLogSubjectId, vUserId:=m_iUserID,
                                         vDescription:=v_sDescription, vEventDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogError, "Failed to add the event", ACApp, ACClass, "CreateEvent", Informations.Err().Number, Informations.Err().Description)

                Return result
            End If
            oEvent.Dispose()
            oEvent = Nothing

            ' If we are adding a document event then add the text to the Notes Field
            If Informations.IsArray(v_vEventNotes) Then

                ' Create an instance of the bPMUser.business object
                m_lReturn = gPMComponentServices.CreateBusinessObject(oFreeFormatText, "bSirFreeFormText.Business", ACApp, m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_oDatabase)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogError, "Failed to initialise the DOC API object", ACApp, ACClass, "CreateEvent", Informations.Err().Number, Informations.Err().Description)

                    Return result
                End If

                With oFreeFormatText

                    'Set the Business Keys in the business object

                    oFreeFormatText.KeyFieldValue = gPMFunctions.ToSafeInteger(lEventCnt)

                    oFreeFormatText.EntityName = "Event"

                    oFreeFormatText.Texttype = "Public"

                    'Set the names of the stored procedures

                    'ReflectionHelper.Invoke(oFreeFormatText, "SQLSet", New Object() {})

                    m_lReturn = oFreeFormatText.sqlset()

                    m_lReturn = oFreeFormatText.EditAdd(1, gPMFunctions.ToSafeInteger(lEventCnt), "[ONLINE USER " & DateTime.Now & "]")

                    lLBnd = v_vEventNotes.GetLowerBound(0)
                    lUBnd = v_vEventNotes.GetUpperBound(0)
                    For iCnt As Integer = lLBnd To lUBnd
                        m_lReturn = oFreeFormatText.EditAdd(iCnt + 2, gPMFunctions.ToSafeInteger(lEventCnt), v_vEventNotes(iCnt))
                    Next iCnt

                    m_lReturn = oFreeFormatText.Update()

                End With

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogError, "CreateEvent Failed", ACApp, ACClass, "CreateEvent", Informations.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: UpdateFileMaster
    '
    ' Description:
    '
    ' Edit History  :
    ' 03/09/1999 Tomo - Created.
    ' RAM20040609   : Bug fix for PN Issue 10321
    '                 1. Removed unwanted Dir Commands
    '                 2. Used bPMDocFunctions CopyFile instead of Standard FileCopy Function
    '                 3. Modified code to use CreateFolderTree instead of MkDir Function
    ' CJB20040818   : Bug fix for PN Issue 14209
    '                 Check new flag to see if to bypass the delete of the .doc file (flag
    '                 will be set to true in SpoolDocument as .doc file is used in there
    '                 on return from calling this (via ArchiveDocument function)
    ' ***************************************************************** '
    Private Function UpdateFileMaster(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lClaimCnt As Integer, ByVal v_sFilename As String, ByVal v_sDescription As String, ByRef r_lDocNumber As Integer) As Integer

        '090699SOB Using Constants to cordinate document Types
        'File Types these should cornatate with Document Types
        'Longer term using a char in the database will be very restricting
        Dim result As Integer = 0
        Const kDocFileTypeUnknown As String = "U" 'Unknown
        Const kDocFileTypeTIF As String = "I" 'TIF
        Const kDocFileTypeTXT As String = "T" 'TXT
        Const kDocFileTypeRTF As String = "W" 'RTF
        Const kDocFileTypeWRD As String = "D" 'Ms Word
        Const kDocFileTypeEXL As String = "X" 'Excel
        Const kDocFileTypePWP As String = "P" 'PowerPoint
        Const kDocFileTypeACC As String = "A" 'Access
        Const kDocFileTypeHTM As String = "H" 'HTML
        Const kDocFileTypeGIF As String = "G" 'GIF
        Const kDocFileTypeJPG As String = "J" 'JPEG
        Const kDocFileTypeEML As String = "M" 'EML Email Doc
        Const kDocFileTypePDF As String = "F" 'Adobe Documents
        Const kDocFileTypeHLP As String = "E" 'Help Files
        Const kDocFileTypeZIP As String = "Z" 'Zip Files

        Dim sDocType, sPageType, sDocName As String ' DN 12/02/01
        Dim sServer As String = "" ' DN 12/02/01
        Dim sErrorMessage As String = "" ' RAM20040209
        Dim oSIRDOCAPI As bSIRDOCAPI.Form
        Dim oSirParty As bSIRParty.Business
        Dim sPartyShortname As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        oSirParty = New bSIRParty.Business
        m_lReturn = oSirParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the bSIRParty object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If


        m_lReturn = oSirParty.GetDetails(vPartyCnt:=v_lPartyCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Method GetDetails of object bSIRParty.Business failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If


        m_lReturn = oSirParty.GetNext(vShortname:=sPartyShortname)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Method GetNext of object bSIRParty.Business failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        sPartyShortname = sPartyShortname.Trim()


        oSirParty.Dispose()
        oSirParty = Nothing

        If oSIRDOCAPI Is Nothing Then

            ' Create an instance of the bPMUser.business object

            oSIRDOCAPI = New bSIRDOCAPI.Form
            m_lReturn = oSIRDOCAPI.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the DOC API object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If

        'It's not used, but we need to define it anyway...
        Dim sKeywords(0) As String

        ' Find the file extension
        sPageType = v_sFilename.Substring(v_sFilename.Length - 6)
        sPageType = sPageType.Substring(sPageType.Length - (sPageType.Length - (sPageType.IndexOf("."c) + 1)))

        'SOB050599 Added Doc, Html doc types and changed the else from producing
        'an error to setting an unknown so that you can import any document type
        'get doc type
        Select Case sPageType.ToUpper()
            Case "TIF", "TIFF"
                sDocType = kDocFileTypeTIF
            Case "RTF"
                sDocType = kDocFileTypeRTF
            Case "TXT", "TEXT", "ASCI"
                sDocType = kDocFileTypeTXT
            Case "DOC", "DOT", "ASC", "ANS", "MCW", "WPS" 'SOB 01/06/99 WORD FILES
                sDocType = kDocFileTypeWRD
            Case "XLS", "XLT", "XLS", "CSV", "WK1", "WK2", "WK3", "WK4", "WQ1", "PRN", "DIF", "SLK", "XLA", "TAB" 'SOB 01/06/99 EXCEL Files
                sDocType = kDocFileTypeEXL
            Case "PPT", "POT", "PPS", "PPA" 'SOB 01/06/99 Power Point Files
                sDocType = kDocFileTypePWP
            Case "MDB", "ADP", "MDW", "MDA", "MDE", "ADE", "DBF", "DB" 'SOB 01/06/99 Ms Access Files
                sDocType = kDocFileTypeACC
            Case "HTM", "HTML", "SHTM", "SHTML", "STM", "ASP", "HTT", "CSS", "CFML", "XML" 'SOB 01/06/99 IE, Netscape Files
                sDocType = kDocFileTypeHTM
            Case "GIF", "GIFF"
                sDocType = kDocFileTypeGIF 'SOB 01/06/99 GIF Files
            Case "JPEG", "JPG"
                sDocType = kDocFileTypeJPG
            Case "EML", "OFT", "MSG", "EML" 'SOB 01/06/99 E-Mail Doc
                sDocType = kDocFileTypeEML
            Case "PDF"
                sDocType = kDocFileTypePDF 'SOB 01/06/99 Adobe Accrobat Files
            Case "HLP"
                sDocType = kDocFileTypeHLP 'SOB 01/06/99 Help Files
            Case "ZIP", "GZ"
                sDocType = kDocFileTypeZIP 'SOB 01/06/99 ZIP Files
            Case Else
                sDocType = kDocFileTypeUnknown
        End Select


        'DN 08/12/01 - Trim description as DME can only handle desc of 50
        sDocName = v_sDescription.Substring(0, Math.Min(v_sDescription.Length, 50))

        'DJM 18/09/2003 : Pass in everything and let Documaster sort it out.
        'FSA 3.2 Pass Complaint details

        m_lReturn = oSIRDOCAPI.AddDocument(lPartyId:=v_lPartyCnt, sPartyName:=sPartyShortname, lInsuranceFolderId:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), sInsuranceFileRef:="", lClaimId:=v_lClaimCnt, sClaimRef:="", lFSAComplaintFolderCnt:=0, sFSAComplaintReference:="", sDocType:=sDocType, sPageType:=sPageType, sDocName:=v_sDescription, sFilename:=v_sFilename, sAnnotation:="", sKeywords:=sKeywords, lDocNumber:=r_lDocNumber)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Archive Document  - " & sDocName, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileMaster", vErrNo:=0, vErrDesc:="Failed to Archive Document  - " & sDocName)

        End If


        oSIRDOCAPI.Dispose()
        oSIRDOCAPI = Nothing

        Return result

    End Function

    Public Function UpdateQuoteWithAdditionalData(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dtCoverStart As Date, ByVal v_dtCoverEnd As Date, ByVal v_sDescription As String, ByVal v_vInsuredParties As Object) As Integer
        Return UpdateQuoteWithAdditionalData(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dtCoverStart:=v_dtCoverStart, v_dtCoverEnd:=v_dtCoverEnd, v_sDescription:=v_sDescription, v_vInsuredParties:=v_vInsuredParties, v_lCurrencyID:=-1, v_lAnalysisCodeId:=0, v_blConsLeadAgntComm:=False, v_blConsSubAgntComm:=False, v_vAdditionalDataArray:=Nothing)
    End Function
    Public Function UpdateQuoteWithAdditionalData(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dtCoverStart As Date, ByVal v_dtCoverEnd As Date, ByVal v_sDescription As String, ByVal v_vInsuredParties As Object, ByVal v_lCurrencyID As Integer, ByVal v_lAnalysisCodeId As Integer, ByVal v_blConsLeadAgntComm As Boolean, ByVal v_blConsSubAgntComm As Boolean, ByVal v_vAdditionalDataArray As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "UpdateQuoteWithAdditionalData"

        Dim kErrorCode As Integer = gPMConstants.PMEReturnCode.PMBackOfficeError
        Dim oInsuranceFileServices As bSIRInsuranceFile.Services

        Dim sAlternativeReference As String = ""
        Dim dtRenewalDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get parameters
            GetAdditionalKeyValues(v_vAdditionalDataArray, dtRenewalDate, sAlternativeReference)

            ' Create the InsuranceFile.Services object

            oInsuranceFileServices = New bSIRInsuranceFile.Services
            'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oInsuranceFileServices, v_sClassName:="bSIRInsuranceFile.Services", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    result = m_lReturn
            '    ' Log Error Message
            '    bPMFunc.LogMessage(sUsername:=gpmfunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateQuoteWithAdditionalData Failed - Failed to create business object to bSIRInsuranceFile.Services.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesByBranch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            'End If
            m_lReturn = oInsuranceFileServices.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to create bSIRInsuranceFile.Services business object.")
            End If

            'Set the insurance file to work upon.

            oInsuranceFileServices.InsuranceFileCnt = v_lInsuranceFileCnt

            m_lReturn = oInsuranceFileServices.GetDetails

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to get Policy details. InsuranceFile.Services.GetDetails failed.")
            End If

            ' Set the properties that are changing on Insurance File

            oInsuranceFileServices.AnalysisCodeId = v_lAnalysisCodeId

            oInsuranceFileServices.CoverStartDate = v_dtCoverStart

            'Inception date Should be same as Policy start Date For New Business PN Number:43518

            If oInsuranceFileServices.InsuranceFileTypeID = 1 Then

                oInsuranceFileServices.CCInceptionDate = v_dtCoverStart
            End If


            oInsuranceFileServices.ExpiryDate = v_dtCoverEnd
            oInsuranceFileServices.RenewalDate = dtRenewalDate
            oInsuranceFileServices.InsuranceFolderDescription = v_sDescription

            If sAlternativeReference <> "" Then

                oInsuranceFileServices.AlternateReference = sAlternativeReference
            End If

            If v_lCurrencyID <> -1 Then

                oInsuranceFileServices.CurrencyID = v_lCurrencyID
            End If

            If Not False Then

                oInsuranceFileServices.LeadAgentAllowCommission = If(v_blConsLeadAgntComm, 1, 0)
            End If

            If Not False Then

                oInsuranceFileServices.SubAgentAllowCommission = If(v_blConsSubAgntComm, 1, 0)
            End If

            ' Update the InsuranceFile quote header

            m_lReturn = oInsuranceFileServices.UpdatePolicy
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(kErrorCode.ToString() + ", " + kMethodName + ", Failed to update Policy header. InsuranceFile.Services.UpdatePolicy failed.")
            End If


        Catch ex As Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateQuoteWithAdditionalData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateQuoteWithAdditionalData", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

            If Not (oInsuranceFileServices Is Nothing) Then

                oInsuranceFileServices.Dispose()
                oInsuranceFileServices = Nothing
            End If


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetAdditionalKeyValues
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-02-2008 : AlternativeRef Changes
    ' ***************************************************************** '
    Private Sub GetAdditionalKeyValues(ByVal v_sParameters As String, ByRef r_dtRenewalDate As Date, ByRef r_sAlternativeRef As String)
        Const kMethodName As String = "GetAdditionalKeyValues"
        Dim lReturn, lSubValue As Integer
        Dim keys() As String
        Dim lUBoundKeys, lLBoundKeys As Integer
        Dim key() As String


        Dim sKeyName, sKeyValue As String

        keys = v_sParameters.Split(New String() {"||||"}, StringSplitOptions.None)

        lLBoundKeys = keys.GetLowerBound(0)
        lUBoundKeys = keys.GetUpperBound(0)

        For lKey As Integer = lLBoundKeys To lUBoundKeys
            key = keys(lKey).Split(New String() {"****"}, StringSplitOptions.None)
            sKeyName = key(0)
            sKeyValue = key(1)

            Select Case sKeyName.ToUpper()
                Case "RENEWALDATE"
                    r_dtRenewalDate = CDate(sKeyValue)
                Case "ALTERNATIVEREF"
                    r_sAlternativeRef = sKeyValue
            End Select
        Next

    End Sub


    Public Function DeleteRiskLink(ByRef lInsuranceFileCnt As Integer, ByRef lRiskID As Integer) As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateInsuranceFileRiskLinkDetailsStatusSQL, sSQLName:=ACUpdateInsuranceFileRiskLinkDetailsStatusName, bStoredProcedure:=ACUpdateInsuranceFileRiskLinkDetailsStatusStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now we need to know if we deleted the record.  The easiest way to tell is to try and select it...

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectInsuranceFileRiskLinkDetailsSQL, sSQLName:=ACSelectInsuranceFileRiskLinkDetailsName, bStoredProcedure:=ACSelectInsuranceFileRiskLinkDetailsStored, vResultArray:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                lRiskID = 0
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRiskLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRiskLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ''' <summary>
    ''' ProcessCopyRisk
    ''' </summary>
    ''' <param name="r_lNewRiskKey"></param>
    ''' <param name="v_iCopyType"></param>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="v_lRiskID"></param>
    ''' <param name="v_lRiskNo"></param>
    ''' <returns></returns>
    Public Function ProcessCopyRisk(ByRef r_lNewRiskKey As Integer, ByVal v_iCopyType As Integer,
                                    ByVal v_lInsuranceFileCnt As Integer,
                                    ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskID As Integer,
                                    ByVal v_lRiskNo As Integer) As Integer
        Return ProcessCopyRisk(r_lNewRiskKey:=r_lNewRiskKey, v_iCopyType:=v_iCopyType,
                                     v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                     v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskID:=v_lRiskID,
                                     v_lRiskNo:=v_lRiskNo, bViaSAM:=False)
    End Function
    Public Function ProcessCopyRisk(ByRef r_lNewRiskKey As Integer, ByVal v_iCopyType As Integer,
                                     ByVal v_lInsuranceFileCnt As Integer,
                                     ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskID As Integer,
                                     ByVal v_lRiskNo As Integer, ByVal bViaSAM As Boolean) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sFailureReason As String = ""
        Dim nRiskNumber As Object
        Dim m_oBusiness As Object = Nothing
        Dim oGISApplication As Application
        Dim oBusiness As Object = Nothing
        Dim nRiskStatusId As Integer
        Dim bIsRIValid As Boolean = True

        Try

            nRiskStatusId = 0
            ' copy the risk
            nResult = CType(CopyRiskData(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                         v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskCnt:=v_lRiskID,
                                         r_lNewRiskCnt:=r_lNewRiskKey, r_sFailureReason:=sFailureReason,
                                         v_bViaSAM:=bViaSAM, r_iRiskStatusID:=nRiskStatusId, r_RiskCopyType:=v_iCopyType),
                            PMEReturnCode)

            If nResult <> PMEReturnCode.PMTrue Then
                nResult = PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                  sMsg:="Unable to copy the risk. CopyRiskData failed on " & sFailureReason,
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCopyRisk")
                Return nResult
                'RaiseError kMethodName, "Unable to copy the risk. CopyRiskData failed on " + sFailureReason, PMLogError
            End If

            ' Create the bSIRListRisks.Business object
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oBusiness,
                                                                v_sClassName:="bSIRListRisks.Business",
                                                                v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername,
                                                                v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                                v_iSourceID:=m_iSourceID,
                                                                v_iLanguageID:=m_iLanguageID,
                                                                v_iCurrencyID:=m_iCurrencyID,
                                                                v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            'Comparative Risk
            If v_iCopyType = 0 Then

                'make sure this risk is not selected
                If m_oBusiness.SetRiskSelectedValue(gPMFunctions.ToSafeInteger(r_lNewRiskKey), 0) <> PMEReturnCode.PMTrue Then
                    nResult = PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                      sMsg:="Failed to reset selected flag for copied risk",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCopyRisk")
                    Return nResult
                End If

                'update the variation number on the new risk
                nResult = m_oBusiness.UpdateRiskVarNo(v_lRiskNumber:=gPMFunctions.ToSafeInteger(v_lRiskNo), v_lRiskCnt:=gPMFunctions.ToSafeInteger(r_lNewRiskKey),
                                                      v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt))
                If nResult <> PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                      sMsg:="Unable to update the risk variation number.",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCopyRisk")
                    Return nResult
                End If
                'Duplicate Risk
            Else
                If m_oBusiness.SetRiskSelectedValue(gPMFunctions.ToSafeInteger(r_lNewRiskKey), 1) <> PMEReturnCode.PMTrue Then
                    nResult = PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                      sMsg:="Failed to reset selected flag for copied risk",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCopyRisk")
                    Return nResult
                End If
                ' Find out the next risk number for this policy
                nResult = m_oBusiness.GetNextRiskNo(v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt),
                                                    r_lRiskNumber:=nRiskNumber)
                If nResult <> PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                      sMsg:="GetNextRiskNo Failed", vApp:=ACApp, vClass:=ACClass,
                                                      vMethod:="ProcessCopyRisk")
                    Return nResult
                End If

                ' Save the risk number to the risk record
                nResult = m_oBusiness.UpdateRiskNo(v_lRiskCnt:=gPMFunctions.ToSafeInteger(r_lNewRiskKey), v_lRiskNumber:=gPMFunctions.ToSafeInteger(nRiskNumber))
                If nResult <> PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                      sMsg:="UpdateRiskNo Failed", vApp:=ACApp, vClass:=ACClass,
                                                      vMethod:="ProcessCopyRisk")
                    Return nResult
                End If
            End If
            ' Assign new risk folder
            nResult = m_oBusiness.UpdateRiskFolder(v_lRiskCnt:=gPMFunctions.ToSafeInteger(r_lNewRiskKey))
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCopyRisk")
                Return nResult
            End If

            ' Assign new risk folder
            nResult = m_oBusiness.UpdateRiskFolder(v_lRiskCnt:=gPMFunctions.ToSafeInteger(r_lNewRiskKey))
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCopyRisk")
                Return nResult
            End If

            nResult = m_oBusiness.UpdateRiskSelection(v_lRiskCnt:=gPMFunctions.ToSafeInteger(r_lNewRiskKey), v_vIsRiskSelected:=0)
            If nResult <> PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                  sMsg:="UpdateRiskSelection Failed", vApp:=ACApp, vClass:=ACClass,
                                                  vMethod:="ProcessCopyRisk")
                Return nResult
            End If

            If r_lNewRiskKey <> 0 Then
                ' Create bSIRPerilAllocation object
                nResult = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oBusiness,
                                                                          v_sClassName:="bSirPerilAllocation.Business",
                                                                          v_sCallingAppName:=ACApp,
                                                                          v_sUsername:=m_sUsername,
                                                                          v_sPassword:=m_sPassword,
                                                                          v_iUserID:=m_iUserID,
                                                                          v_iSourceID:=m_iSourceID,
                                                                          v_iLanguageID:=m_iLanguageID,
                                                                          v_iCurrencyID:=m_iCurrencyID,
                                                                          v_iLogLevel:=m_iLogLevel,
                                                                          v_oDatabase:=m_oDatabase),
                                PMEReturnCode)
                If nResult <> PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                      sMsg:="ProcessCopyRisk Failed to Initialise bSirPerilAllocation.Business object.",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCopyRisk",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                oBusiness.InsuranceFolderCnt = v_lInsuranceFolderCnt
                oBusiness.InsuranceFileCnt = v_lInsuranceFileCnt
                oBusiness.RiskID = r_lNewRiskKey
                oBusiness.TransactionType = "NB"

                nResult = oBusiness.PopulateRatingSections()

                If nResult <> PMEReturnCode.PMTrue Then
                    nResult = PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                      sMsg:="ProcessCopyRisk failed to call PopulateRatingSections for the following risk :- " &
                                                      r_lNewRiskKey, vApp:=ACApp, vClass:=ACClass,
                                                      vMethod:="ProcessCopyRisk")
                    Return nResult
                End If

                oGISApplication = New Application()

                m_lReturn = oGISApplication.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


                If nResult <> PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                      sMsg:="ProcessCopyRisk Failed to Initialise bGIS.Application object.",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCopyRisk",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                nResult = oGISApplication.UpdateRiskAfter(v_lInsuranceFileCnt, v_lInsuranceFolderCnt, r_lNewRiskKey,
                                                          "NB", bViaSAM, nRiskStatusId)

                If nResult <> PMEReturnCode.PMTrue Then
                    nResult = PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                      sMsg:="ProcessCopyRisk failed to call UpdateRiskAfter for the following risk :- " &
                                                      r_lNewRiskKey, vApp:=ACApp, vClass:=ACClass,
                                                      vMethod:="ProcessCopyRisk")
                    Return nResult
                End If

                oBusiness.Dispose()
                oBusiness = Nothing

                ' Create the bSIRRiskData.Business object
                nResult = gPMComponentServices.CreateBusinessObject(r_oObject:=oBusiness,
                                                                    v_sClassName:="bSIRRiskData.Business",
                                                                    v_sCallingAppName:=ACApp,
                                                                    v_sUsername:=m_sUsername,
                                                                    v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                                                                    v_iSourceID:=m_iSourceID,
                                                                    v_iLanguageID:=m_iLanguageID,
                                                                    v_iCurrencyID:=m_iCurrencyID,
                                                                    v_iLogLevel:=m_iLogLevel,
                                                                    v_oDatabase:=m_oDatabase)

                If nResult <> PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                      sMsg:="UpdateRiskAfter Failed to Initialise bSirRiskData.Business object.",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                nResult = oBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit,
                                                    vNavigate:=0, vProcessMode:=110, vTransactionType:="",
                                                    vEffectiveDate:=DateTime.Now)
                If nResult <> PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                      sMsg:="UpdateRiskAfter Failed to SetProcessModes for bSirRiskData.Business object.",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                'updated the risk status same as the original risk have
                nResult = oBusiness.UpdateRiskStatus(v_lRiskCnt:=gPMFunctions.ToSafeInteger(r_lNewRiskKey), v_lRiskStatusID:=If(bIsRIValid, nRiskStatusId, 4))
                If nResult <> PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=PMELogLevel.PMLogOnError,
                                                      sMsg:="ProcessCopyRisk failed to call UpdateRiskStatus method.",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCopyRisk",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

            End If

        Catch ex As Exception

            nResult = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="ProcessCopyRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCopyRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

            m_oBusiness = Nothing
            oBusiness.Dispose()
            oBusiness = Nothing
            oGISApplication = Nothing

        End Try

        Return nResult

    End Function


    ' ***************************************************************** '
    ' Name: CopyRiskData
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Jaideep : Date : Process ID
    ' ***************************************************************** '
    Public Function CopyRiskData(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_lNewRiskCnt As Integer, ByRef r_sFailureReason As String) As Integer
        Return CopyRiskData(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskCnt:=v_lRiskCnt, r_lNewRiskCnt:=r_lNewRiskCnt, r_sFailureReason:=r_sFailureReason, v_bViaSAM:=False, r_iRiskStatusID:=0)
    End Function
    Public Function CopyRiskData(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_lNewRiskCnt As Integer, ByRef r_sFailureReason As String, ByVal v_bViaSAM As Boolean) As Integer
        Return CopyRiskData(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskCnt:=v_lRiskCnt, r_lNewRiskCnt:=r_lNewRiskCnt, r_sFailureReason:=r_sFailureReason, v_bViaSAM:=v_bViaSAM, r_iRiskStatusID:=0)
    End Function
    Public Function CopyRiskData(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_lNewRiskCnt As Integer, ByRef r_sFailureReason As String, ByRef r_iRiskStatusID As Integer) As Integer
        Return CopyRiskData(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskCnt:=v_lRiskCnt, r_lNewRiskCnt:=r_lNewRiskCnt, r_sFailureReason:=r_sFailureReason, v_bViaSAM:=False, r_iRiskStatusID:=r_iRiskStatusID)
    End Function
    Public Function CopyRiskData(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_lNewRiskCnt As Integer, ByRef r_sFailureReason As String, ByVal v_bViaSAM As Boolean, ByRef r_iRiskStatusID As Integer, Optional ByVal r_RiskCopyType As Integer = 0) As Integer
        Dim result As Integer = 0
        'Const kMethodName As String = "CopyRiskData"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bFound As Boolean
        Dim lNewGisPolicyLinkID As Object
        Dim vGisPolicyLinkArray As Object = Nothing
        Dim vRiskArray(,) As Object = Nothing
        Dim lCount As Integer
        Dim sXMLDataSetDef, sXMLDataSet As Object

        Dim m_oRenSelection As Object
        Dim m_oRiskData As bSIRRiskData.Business
        Const ACRiskPosCnt As Integer = 0
        Dim lNewPolicyBinderId As Integer = 0
        Dim lOldPolicyBinderId As Integer = 0
        Const kRisFolderkPosCnt As Integer = 2
        Dim nNew_risk_folder_cnt As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the bSirRiskData.business object

            m_oRiskData = New bSIRRiskData.Business
            m_lReturn = m_oRiskData.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            ' Get all risks associate with the InsuranceFileCnt

            lReturn = m_oRiskData.GetRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResultArray:=vRiskArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = "Getting Risk"
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                Return result
                'RaiseError kMethodName, "GetRisk Failed", PMLogError
            End If

            ' Check if we have any risks
            If Not Informations.IsArray(vRiskArray) Then
                r_sFailureReason = "No risks found"
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRisk Returned no data", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                Return result
                'RaiseError kMethodName, "GetRisk Returned no data", PMLogError
            End If

            ' Find the risk that matches the passed risk count, i.e. the one we want
            ' to copy
            bFound = False
            For lCount = 0 To vRiskArray.GetUpperBound(1)

                If CDbl(vRiskArray(0, lCount)) = v_lRiskCnt Then
                    If Not Informations.IsNothing(vRiskArray(1, lCount)) Then
                        r_iRiskStatusID = vRiskArray(1, lCount)
                    Else
                        r_iRiskStatusID = 4
                    End If
                    bFound = True
                    Exit For
                End If
            Next

            ' Check if we have found the risk to copy
            If Not bFound Then
                r_sFailureReason = "Cannot find risk to copy"
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Risk to Copy Not Found", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                Return result
                'RaiseError kMethodName, "Risk to Copy Not Found", PMLogError
             End If

        '1 for Duplicate risk
        If r_RiskCopyType = 1 Then


            lReturn = m_oRiskData.CopyRiskFolder(v_lRisk_folder_cnt:=vRiskArray(kRisFolderkPosCnt, lCount), v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lNew_risk_folder_cnt:=nNew_risk_folder_cnt)

            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                vRiskArray(kRisFolderkPosCnt, lCount) = nNew_risk_folder_cnt
            Else
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRisk Folder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                Return result
            End If
        End If


            ' Copy risk with same insurance file cnt
            If (r_iRiskStatusID = 2 Or r_iRiskStatusID = 1) And v_bViaSAM Then
                lReturn = m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lInsuranceFileCnt, v_vRiskDetail:=vRiskArray, v_lPosNo:=lCount, r_lRiskCnt:=r_lNewRiskCnt, v_lResetStatus:=gPMConstants.PMEReturnCode.PMFalse)
            Else
                lReturn = m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lInsuranceFileCnt,
                                           v_vRiskDetail:=vRiskArray,
                                           v_lPosNo:=lCount,
                                           r_lRiskCnt:=r_lNewRiskCnt,
                                           v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue)
            End If
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = "Copy Risk"
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                Return result
                'RaiseError kMethodName, "CopyRisk Failed", PMLogError
            End If

            ' Prepare details to copy GIS Stuff attached to current risk

            ' Get policy link detail
            lReturn = m_oRiskData.GetGISPolicyLink(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskID:=vRiskArray(ACRiskPosCnt, lCount), r_vResultArray:=vGisPolicyLinkArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = "GetGISPolicyLink"
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGISPolicyLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                Return result
                'RaiseError kMethodName, "GetGISPolicyLink Failed", PMLogError
            End If

            ' Do we have any data?
            Dim auxVar As Object = vGisPolicyLinkArray(0, 0)

            If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then
                ' Create the bSirRenSelection.business object
                m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oRenSelection, v_sClassName:="bSIRRenSelection.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

                ' m_oRenSelection = New bSIRRenSelection.Business
                Dim oDatabase As Object = m_oDatabase
                m_lReturn = m_oRenSelection.Initialise(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=oDatabase)

                lReturn = m_oRenSelection.GIS_LoadFromDB(CStr(vGisPolicyLinkArray(4, 0)).Trim(), gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), vGisPolicyLinkArray(0, 0), vRiskArray(0, lCount)) 'copy GIS details to NewInsuranceFileCnt
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "LoadFromDB"
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                    Return result
                    'RaiseError kMethodName, "LoadFromDB Failed", PMLogError
                End If
                m_oDatabase = oDatabase

                ' REMEMBER we are storing folder_cnt in file_cnt field now !!!!!
                ' So we pass existing folder_cnt in for old and new file_cnt.
                lReturn = m_oRenSelection.CopyDataSet(v_sDataModelCode:=CStr(vGisPolicyLinkArray(4, 0)).Trim(), r_lNewGISPolicyLinkId:=lNewGisPolicyLinkID, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet, v_vOldGISPolicyLinkId:=vGisPolicyLinkArray(0, 0), v_vOldInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_vOldRiskID:=vRiskArray(0, lCount), v_vNewInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_vNewRiskID:=gPMFunctions.ToSafeInteger(r_lNewRiskCnt))
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "CopyDataSet"
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyDataSet Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                    Return result
                    'RaiseError kMethodName, "CopyDataSet Failed", PMLogError
                End If

                ' Initialise the Data Set with the Object/Properties
                m_lReturn = m_oRenSelection.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "LoadFromXML"
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                    Return result
                    'RaiseError kMethodName, "LoadFromXML Failed", PMLogError
                End If

                'RWH(28/02/2001)


                lReturn = m_oRenSelection.GIS_SaveToDB(v_sGisDataModelCode:=CStr(vGisPolicyLinkArray(4, 0)).Trim())
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "SaveToDB"
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveToDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                    Return result
                    'RaiseError kMethodName, "SaveToDB Failed", PMLogError
                End If

                lReturn = CopyRiskStandardWordings(v_lOldPolicyBinderId:=vGisPolicyLinkArray(0, 0), v_lNewPolicyBinderId:=lNewGisPolicyLinkID, v_sDataModelCode:=CStr(vGisPolicyLinkArray(4, 0)).Trim())

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "CopyRiskStandardWordings"
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                    Return result
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result
    End Function
    ' ***************************************************************** '
    '
    ' Name: CopyRiskStandardWordings
    '
    '
    ' ***************************************************************** '
    Public Function CopyRiskStandardWordings(ByVal v_lOldPolicyBinderId As Integer, ByVal v_lNewPolicyBinderId As Integer, ByVal v_sDataModelCode As String) As Integer

        Dim result As Integer = 0
        Dim sWordingTable, sPolicyBinderIdName As String
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""
        Dim oDocTemplate As Object    ' bSIRDocTemplate.Business
        Dim lDocumentTemplateID As Integer

        Const ACPosSequenceID As Integer = 0
        Const ACPosDocTemplateID As Integer = 1
        Const ACPosPropertyID As Integer = 2
        Const ACPosObjectID As Integer = 3
        Const ACPosChild As Integer = 4
        Const ACPosCopyOfOriginal As Integer = 5

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_sDataModelCode = v_sDataModelCode.Trim()
            sWordingTable = v_sDataModelCode & "_standard_wording"
            sPolicyBinderIdName = v_sDataModelCode & "_Policy_binder_id"

            sSQL = "SELECT SW.sequence_id, SW.document_template_id, SW.gis_property_id, SW.gis_object_id, "
            sSQL = sSQL & "SW.child, DT.copy_of_original" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM " & sWordingTable & " SW" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "INNER JOIN Document_Template DT ON DT.document_template_id=SW.document_template_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE SW." & sPolicyBinderIdName & " = " & CStr(v_lOldPolicyBinderId)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRiskStandardWordings", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + +", Select statement failed.")

            End If

            sSQL = ""

            If Informations.IsArray(vResultArray) Then

                For iCount As Integer = 0 To vResultArray.GetUpperBound(1)

                    lDocumentTemplateID = gPMFunctions.ToSafeLong(vResultArray(ACPosDocTemplateID, iCount), 0)

                    m_oDatabase.Parameters.Clear()
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="data_model", vValue:=v_sDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="new_policy_binder", vValue:=CStr(v_lNewPolicyBinderId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="old_policy_binder", vValue:=CStr(v_lOldPolicyBinderId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_prop_id", vValue:=CStr(gPMFunctions.ToSafeLong(vResultArray(ACPosPropertyID, iCount))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_obj_id", vValue:=CStr(gPMFunctions.ToSafeLong(vResultArray(ACPosObjectID, iCount))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="doc_template_id", vValue:=CStr(lDocumentTemplateID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="seq_id", vValue:=CStr(gPMFunctions.ToSafeLong(vResultArray(ACPosSequenceID, iCount))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="isChild", vValue:=CStr(gPMFunctions.ToSafeInteger(vResultArray(ACPosChild, iCount), CInt("0"))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'WPR 33-75 added
                    If gPMFunctions.ToSafeInteger(vResultArray(ACPosChild, iCount), 0) = 1 Then

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="ChildId", vValue:=CStr(ToSafeLong(vResultArray(ACPosDocTemplateID, iCount), 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' Execute SP
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyRiskStandardWordingsSQL, sSQLName:=ACCopyRiskStandardWordingsName, bStoredProcedure:=ACCopyRiskStandardWordingsStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", " + +", Insert statement failed.")
                    End If

                Next iCount

                If Not (oDocTemplate Is Nothing) Then

                    oDocTemplate.Dispose()
                    oDocTemplate = Nothing
                End If
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskStandardWordings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' This function is used to check password is used earlier
    ''' </summary>
    ''' <param name="iUser_Id"></param>
    ''' <param name="sNewPassword"></param>
    ''' <param name="IsValid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsReusedPassword(ByVal iUser_Id As Integer, ByVal sNewPassword As String, ByRef IsValid As Boolean) As Integer

        Dim lReturn As Integer = 0
        Dim lRecordsAffected As Integer = 0
        Dim dtResult As New DataTable
        Dim oDatabase As dPMDAO.Database
        Dim sPasswordEncrypted As String = ""
        Try
            lReturn = gPMConstants.PMEReturnCode.PMTrue
            lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, 1, 1, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
            oDatabase.Parameters.Clear()

            lReturn = oDatabase.Parameters.Add(sName:="user_id", vValue:=CInt(iUser_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            ' Execute SQL Statement

            lReturn = oDatabase.ExecuteDataTable(sSQL:=ACGetpasswordhistorySQL, sSQLName:=ACGetpasswordhistoryName, bStoredProcedure:=ACGetpasswordhistoryStored, oRecordset:=dtResult)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oDatabase.CloseDatabase()
                oDatabase = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If dtResult.Rows.Count > 0 Then
                For i As Integer = 0 To dtResult.Rows.Count - 1
                    sPasswordEncrypted = dtResult.Rows(i)("historic_password").ToString()

                    ' check the password’s match
                    If bPMFunc.CheckPassword(sNewPassword, sPasswordEncrypted) Then
                        IsValid = False
                        Return gPMConstants.PMEReturnCode.PMReusedPassword
                    End If
                Next
                IsValid = True
            Else
                IsValid = True
            End If

            Return lReturn

        Catch ex As Exception

            ' Error.
            lReturn = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsReusedPassword Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsReusedPassword", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return lReturn
        End Try
    End Function


End Class
