Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
'Developer Guide No. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 30/10/2002
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRListRisks.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 19/12/2003
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
    Private Const ACClass As String = "Business"

    ' Constants added to update insurance file to rollback the discounts
    Private Const ACRollBackDicountedPremium As Double = 0
    Private Const ACRollBackDicountedPercentage As Double = 0

    'Developer Guide No. 85
    Private Const ACRollBackDicountedReasonId As Object = Nothing
    Private Const ACRollBackMatchDicountedPremium As Integer = 0
    Private Const CheckStateChecked As Integer = 1

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)


    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Calling Application Name

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer

    Private m_sUnderwritingType As String = ""
    ' Private m_oSIRRITax As Object
    ' Private m_oSIRPartyFee As Object
    Private m_oSIRReinsurance As Object
    'Private m_oSirRiskData As Object
    Private m_oSirPerilAllocation As bSirPerilAllocation.Business
    'Private m_oBusiness As Object
    Private m_bIsRi2007Enabled As Boolean
    'Private m_oBusinessFindInsurance As Object

    ' Primary Keys to work with
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property

    Public ReadOnly Property UnderwritingType() As String
        Get

            If m_sUnderwritingType = "" Then
                m_lReturn = getUnderwritingType()
            End If

            Return m_sUnderwritingType

        End Get
    End Property

    Public Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get

        Set(ByVal Value As String)

            m_sTransactionType = Value

        End Set
    End Property

    ''' <summary>
    ''' CopyRisksMTA
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="bFromSAM"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyRisksMTA(ByVal v_lInsuranceFileCnt As Long, Optional ByVal v_lCreateLinkType As Long = 1, Optional ByVal Is_SAM_Copy_Quote As Boolean = False, Optional ByVal bFromSAM As Boolean = False, Optional bCopyRiskMTA As Boolean = False) _
        As Integer
        Dim nResult As Integer = 0
        nResult = CopyRisksMTAEx(v_lInsuranceFileCnt, bFromSAM, v_lCreateLinkType, Is_SAM_Copy_Quote, v_bCopyRiskOnMTA:=bCopyRiskMTA)
        Return nResult
    End Function
    '*****************************************************************
    '
    ' Get the next risk number - PW301002
    '
    ' Returns the next risk number for a policy by checking the existing
    ' risk numbers
    '*****************************************************************
    '
    Public Function GetNextRiskNo(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRiskNumber As Integer) As Integer

        Dim result As Integer = 0
        Try

            ' Add parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Number", vValue:=CStr(r_lRiskNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetNextRiskNoSQL, sSQLName:=ACGetNextRiskNoName, bStoredProcedure:=ACGetNextRiskNoStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the return parameteres
            r_lRiskNumber = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("risk_number").Value)
            r_lRiskNumber += 1

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextRiskNo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextRiskNo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateFlaggedQuote
    '
    ' Description: Updates the Flagged Quote table to indicate if this
    '              quote is a "hot" quote
    '
    ' Created: PW301002
    '
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function UpdateFlaggedQuote(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sFollowUpNote As String, ByVal v_sReferredTo As String, ByVal v_bIsHotQuote As Object) As Integer

        Dim result As Integer = 0
        Dim lFollowUpTimeFrame As Integer
        Dim dFollowUpDueDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' If this is a hot quote, determine the follow up due date
            '
            'CheckState.Checked is removed as we have to remove refrences of System.Windows.Forms
            If v_bIsHotQuote = CheckStateChecked Then
                '
                ' Get the follow up time frame
                '
                m_oDatabase.Parameters.Clear()
                ' Add parameters
                m_lReturn = m_oDatabase.Parameters.Add(sName:="follow_up_time_frame", vValue:=CStr(lFollowUpTimeFrame), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Execute the stored procedure
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetFollowUpTimeFrameSQL, sSQLName:=ACGetFollowUpTimeFrameName, bStoredProcedure:=ACGetFollowUpTimeFrameStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get the return parameteres
                lFollowUpTimeFrame = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("follow_up_time_frame").Value)
                '
                ' Calculate the follow up due date
                '
                dFollowUpDueDate = DateTime.Today.AddDays(lFollowUpTimeFrame)

            End If
            '
            ' Update the flagged quote table
            '
            m_oDatabase.Parameters.Clear()

            ' Add the insurance file count parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If this is a hot quote, add the other parameters
            'CheckState.Checked is removed as we have to remove refrences of System.Windows.Forms
            If v_bIsHotQuote = CheckStateChecked Then
                'Developer Guide No. 40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="follow_up_due_date", vValue:=dFollowUpDueDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = m_oDatabase.Parameters.Add(sName:="follow_up_note", vValue:=v_sFollowUpNote, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = m_oDatabase.Parameters.Add(sName:="referred_to", vValue:=v_sReferredTo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' This is a hot quote, so call the sp to update/add the
                ' flagged_quote record
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateFlaggedQuoteSQL, sSQLName:=ACUpdateFlaggedQuoteName, bStoredProcedure:=ACUpdateFlaggedQuoteStored)
            Else
                ' This is not a hot quote, so call the sp to delete
                ' any existing flagged_quote record
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteFlaggedQuoteSQL, sSQLName:=ACDeleteFlaggedQuoteName, bStoredProcedure:=ACDeleteFlaggedQuoteStored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFlaggedQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFlaggedQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateRiskSelectionStatus
    '
    ' Description: Accepts an array of risk cnts/selection status'
    ' and updates the corresponding risk records with the status'.
    '
    ' Created: PW301002
    '
    ' ***************************************************************** '
    Public Function UpdateRiskSelectionStatus(ByVal v_vSelectionArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Start a transaction
            result = BeginTrans()
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Loop through the array

            For i As Integer = v_vSelectionArray.GetLowerBound(1) To v_vSelectionArray.GetUpperBound(1)

                ' Add parameters
                m_oDatabase.Parameters.Clear()


                'Developer Guide No. 162
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(v_vSelectionArray(0, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    RollbackTrans()
                    Return result
                End If



                'Developer Guide No. 85,162
                m_lReturn = m_oDatabase.Parameters.Add(sName:="is_selected", vValue:=If(CBool(v_vSelectionArray(1, i)), CStr(1), DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    RollbackTrans()
                    Return result
                End If

                ' Execute the stored procedure
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskSelectionStatusSQL, sSQLName:=ACUpdateRiskSelectionStatusName, bStoredProcedure:=ACUpdateRiskSelectionStatusStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    RollbackTrans()
                    Return result
                End If

            Next

            ' Complete the transaction

            Return CommitTrans()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskSelectionStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskSelectionStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    '*****************************************************************
    '
    ' Update the variation number for the risk - PW301002
    '
    ' Takes the current risk identified by the passed risk count,
    ' and gives it the next variation number
    '*****************************************************************
    '
    Public Function UpdateRiskVarNo(ByVal v_lRiskNumber As Integer, ByVal v_lRiskCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lVariationNo As Integer

        Try

            '
            ' Get the next variation number using stored procedure
            '
            ' Add parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Variation_Number", vValue:=CStr(lVariationNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Number", vValue:=CStr(v_lRiskNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetNextRiskVarSQL, sSQLName:=ACGetNextRiskVarName, bStoredProcedure:=ACGetNextRiskVarStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the return parameteres
            lVariationNo = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("variation_number").Value)
            lVariationNo += 1
            '
            ' Update the risk with the variation number via the stored procedure
            '
            ' Add parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Variation_Number", vValue:=CStr(lVariationNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskVariationSQL, sSQLName:=ACUpdateRiskVariationName, bStoredProcedure:=ACUpdateRiskVariationStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskVarNo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskVarNo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*****************************************************************
    '
    ' Update Risk Number - PW301002
    '
    ' Updates the risk number for the risk cnt
    '*****************************************************************
    '
    Public Function UpdateRiskNo(ByVal v_lRiskCnt As Integer, ByRef v_lRiskNumber As Integer) As Integer

        Dim result As Integer = 0
        Try

            ' Add parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Number", vValue:=CStr(v_lRiskNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskNoSQL, sSQLName:=ACUpdateRiskNoName, bStoredProcedure:=ACUpdateRiskNoStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Updateriskno Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Updateriskno", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function UpdateRiskFolder(ByVal v_lRiskCnt As Long) As Long

        Dim vRiskDetails(,) As Object = Nothing
        Dim vRiskFolderArray(,) As Object = Nothing


        Try

            UpdateRiskFolder = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt",
                                                   vValue:=v_lRiskCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetRiskDetailsSQL,
                                              sSQLName:=kGetRiskDetailsName,
                                              bStoredProcedure:=kGetRiskDetailsStored,
                                              vResultArray:=vRiskDetails,
                                              bKeepNulls:=True)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("UpdateRiskFolder", "Failed to fetch Risk details", gPMConstants.PMEReturnCode.PMError)
            End If


            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_cnt",
                                                   vValue:=vRiskDetails(ACRRiskFolderCnt, 0),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetRiskFolderDetailsSQL,
                                              sSQLName:=kGetRiskFolderDetailsName,
                                              bStoredProcedure:=kGetRiskFolderDetailsStored,
                                              vResultArray:=vRiskFolderArray,
                                              bKeepNulls:=True)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("UpdateRiskFolder", "Failed to fetch Risk folder details", gPMConstants.PMEReturnCode.PMError)
            End If


            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_cnt",
                                                    vValue:=0,
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput,
                                                    iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_id",
                                                   vValue:=0,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id",
                                                   vValue:=vRiskFolderArray(ACRFSourceId, 0),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_type_id",
                                                   vValue:=vRiskFolderArray(ACRFRiskFolderTypeId, 0),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code",
                                                   vValue:=vRiskFolderArray(ACRFCode, 0),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="description",
                                                   vValue:=vRiskFolderArray(ACRFDescription, 0),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt",
                                                   vValue:=vRiskFolderArray(ACRFInsuranceFolderCnt, 0),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=kInsertRiskFolderDetailsSQL,
                                                sSQLName:=kInsertRiskFolderDetailsName,
                                                bStoredProcedure:=kInsertRiskFolderDetailsStored)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("UpdateRiskFolder", "Failed to update Risk folder", gPMConstants.PMEReturnCode.PMError)
            End If

            vRiskFolderArray(ACRFRiskFolderCnt, 0) = m_oDatabase.Parameters.Item("risk_folder_cnt").Value


            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt",
                                                   vValue:=v_lRiskCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_cnt",
                                                   vValue:=vRiskFolderArray(ACRFRiskFolderCnt, 0),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetUpdateRiskFolderDetailsSQL,
                                              sSQLName:=kGetUpdateRiskFolderDetailsName,
                                              bStoredProcedure:=kGetUpdateRiskFolderDetailsStored,
                                              vResultArray:=vRiskFolderArray,
                                              bKeepNulls:=True)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("UpdateRiskFolder", "Failed to update Risk folder details", gPMConstants.PMEReturnCode.PMError)
            End If

        Catch excep As System.Exception

            Return gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="UpdateRiskFolder", r_lFunctionReturn:=UpdateRiskFolder, excep:=excep)

        End Try

    End Function



    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetUnderwritingType
    '
    ' Description:  Finds out Underwriting type - U or A
    '               For labelling: A - Insurer. U - Reinsurer
    '
    ' JMK 23/10/2001    Created
    ' ***************************************************************** '
    Private Function getUnderwritingType() As Integer

        Dim result As Integer = 0



        Return bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingType)

    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Dim sValue As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' get instance of reinsurance component
            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=1, r_vUnderwriting:=sValue), gPMConstants.PMEReturnCode)

            If sValue = "1" Then
                m_bIsRi2007Enabled = True
            Else
                m_bIsRi2007Enabled = False
            End If


            ' get instance of reinsurance component
            If m_bIsRi2007Enabled Then
                m_lReturn = gPMComponentServices.CreateBusinessObject(
                                    r_oObject:=m_oSIRReinsurance,
                                    v_sClassName:="bSIRReinsuranceRI2007.Form",
                                    v_sCallingAppName:=ACApp,
                                    v_sUsername:=m_sUsername,
                                    v_sPassword:=m_sPassword,
                                    v_iUserID:=m_iUserID,
                                    v_iSourceID:=m_iSourceID,
                                    v_iLanguageID:=m_iLanguageID,
                                    v_iCurrencyID:=m_iCurrencyID,
                                    v_iLogLevel:=m_iLogLevel,
                                    v_oDatabase:=m_oDatabase)
            Else
                m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oSIRReinsurance, v_sClassName:="bSIRReinsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Get the object for perilAllocation
            m_oSirPerilAllocation = New bSirPerilAllocation.Business
            m_lReturn = CType(m_oSirPerilAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            m_lReturn = m_oSIRReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vTransactionType:="NB")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If Not m_oSIRReinsurance Is Nothing Then
                    m_oSIRReinsurance.Dispose()
                    m_oSIRReinsurance = Nothing
                End If

                m_oSirPerilAllocation.Dispose()
                m_oSirPerilAllocation = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Public Methods (End)


    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ''' <summary>
    ''' Get InsuranceFileDetails
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="r_vResults"></param>
    ''' <param name="v_lOriginalInsuranceFileCnt"></param>
    ''' <param name="bIsSelectLivePlan"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetInsuranceFileDetails(ByVal v_lInsuranceFileCnt As Object, ByRef r_vResults(,) As Object, Optional ByRef v_lOriginalInsuranceFileCnt As Object = Nothing, Optional ByVal bIsSelectLivePlan As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInsuranceFileDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' RAM20050826 - Added the v_lOriginalInsuranceFileCnt Parameter.

            If Not Informations.IsNothing(v_lOriginalInsuranceFileCnt) Then

                AddInputParameter(v_sName:="original_insurance_file_cnt", v_vValue:=v_lOriginalInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
            End If
            If bIsSelectLivePlan Then
                AddInputParameter(v_sName:="bIsSelectLivePlan", v_vValue:=1, v_iType:=gPMConstants.PMEDataType.PMInteger)
            End If
            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetInsuranceFileDetailsSQL, sSQLName:=kGetInsuranceFileDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetInsuranceFileDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object
            If v_vValue Is DBNull.Value Then
                lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)
            Else
                lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)
            End If


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: UpdateRiskSelection
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function UpdateRiskSelection(ByVal v_lRiskCnt As Integer, ByVal v_vIsRiskSelected As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateRiskSelection"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddInputParameter(v_sName:="risk_cnt", v_vValue:=v_lRiskCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            AddInputParameter(v_sName:="is_risk_selected", v_vValue:=v_vIsRiskSelected, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateRiskSelectionSQL, sSQLName:=kUpdateRiskSelectionName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kUpdateRiskSelectionSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ''' <summary>
    ''' UpdatePolicyDetails
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lPutOnNextInstalmentRenewal"></param>
    ''' <param name="v_sPaymentMethod"></param>
    ''' <param name="v_lMarkedForCollection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdatePolicyDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPutOnNextInstalmentRenewal As Integer, Optional ByVal v_sPaymentMethod As String = "", Optional ByVal v_lMarkedForCollection As Integer = 0, Optional ByVal v_nCollectionFrequency As Integer = 0, Optional ByVal v_nDOPaymentTerms As Integer = 0) As Integer

        Const kMethodName As String = "UpdatePolicyDetails"
        Dim nResult As Integer

        Try

            nResult = PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="put_on_next_instalment_renewal", v_vValue:=v_lPutOnNextInstalmentRenewal, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="payment_method", v_vValue:=v_sPaymentMethod, v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="marked_for_collection", v_vValue:=v_lMarkedForCollection, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="marked_date", v_vValue:=DateTime.Today, v_iType:=gPMConstants.PMEDataType.PMDate)

            If v_nDOPaymentTerms > 0 Then
                AddInputParameter(v_sName:="payment_term", v_vValue:=v_nDOPaymentTerms, v_iType:=gPMConstants.PMEDataType.PMLong)
            End If

            If v_nCollectionFrequency > 0 Then
                AddInputParameter(v_sName:="collection_frequency", v_vValue:=v_nCollectionFrequency, v_iType:=gPMConstants.PMEDataType.PMLong)
            End If

            ' Execute Action Query
            nResult = m_oDatabase.SQLAction(sSQL:=kUpdatePolicyDetailsSQL, sSQLName:=kUpdatePolicyDetailsName, bStoredProcedure:=True)

            If nResult <> PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kUpdatePolicyDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return nResult
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return PMEReturnCode.PMError
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RecalculateRiskFees
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : discount / loading
    ' ***************************************************************** '
    Public Function RecalculateRiskFees(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTransactionTypeId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RecalculateRiskFees"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="transaction_type_id", v_vValue:=v_lTransactionTypeId, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kRecalculateRiskFeesSQL, sSQLName:=kRecalculateRiskFeesName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kRecalculateRiskFeesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ''' <summary>
    ''' RecalculateRiskTaxes
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lTask"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RecalculateRiskTaxes(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTask As Integer,
                                         ByVal v_sTransactionType As String) As Integer

        Dim nResult As Integer
        Const kMethodName As String = "RecalculateRiskTaxes"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oSIRRITax As New bSIRRITax.Business
        Dim oRisks(,) As Object = Nothing
        Dim nLBound As Integer = 0
        Dim nUBound As Integer = 0
        Dim nRiskCnt As Integer
        Try


            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' return the risks to be recalculated
            lReturn = CType(GetPolicyDiscountRisks(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResults:=oRisks),
                            gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            oSIRRITax = New bSIRRITax.Business
            m_lReturn = oSIRRITax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRRITax.InsuranceFileCnt = v_lInsuranceFileCnt
            If Informations.IsArray(oRisks) Then


                nLBound = oRisks.GetLowerBound(1)

                nUBound = oRisks.GetUpperBound(1)

                For iRisk As Integer = nLBound To nUBound


                    nRiskCnt = CInt(oRisks(0, iRisk))

                    ' recalculate the policy risk taxes
                    'Developer Guide No.20
                    oSIRRITax.m_oDatabase = m_oDatabase
                    lReturn = oSIRRITax.RecalculatePolicyRiskTaxes(v_lRiskCnt:=nRiskCnt, v_lTask:=0,
                                                                     v_sTransactionType:=v_sTransactionType)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "RecalculatePolicyRiskTaxes Failed",
                                                gPMConstants.PMELogLevel.PMLogError)
                    End If

                Next

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            oSIRRITax.Dispose()
            oSIRRITax = Nothing


        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyDiscountRisks
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function GetPolicyDiscountRisks(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyDiscountRisks"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetPolicyDiscountRisksSQL, sSQLName:=kGetPolicyDiscountRisksName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetPolicyDiscountRisksSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessApplyDiscount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function ProcessApplyDiscount(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_sTransactionType As String, ByVal v_lTask As Integer, ByRef r_sFailureReason As String, Optional ByVal crAppliedDiscountPremium As Decimal = 0, Optional ByVal dAppliedDiscountPercentage As Double = 0, Optional ByVal lAppliedMatchDiscountPremium As Integer = 0, Optional ByVal lAppliedDiscountReasonId As Integer = 0, Optional ByVal lAppliedDiscountRecurringTypeId As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessApplyDiscount"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim crOriginalPremium As Decimal
        Dim dDiscountPercentage As Double
        Dim crDiscountedPremium As Decimal
        Dim lMatchDiscountedPremium, lTransactionTypeId As Integer
        Dim lDiscountReasonId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the required policy discount details
            lReturn = CType(GetPolicyDiscountDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sTransactionType:=v_sTransactionType, r_crPremium:=crOriginalPremium, r_crDiscountedPremium:=crDiscountedPremium, r_dDiscountPercentage:=dDiscountPercentage, r_lMatchDiscountedPremium:=lMatchDiscountedPremium, r_lTransactionTypeId:=lTransactionTypeId, r_lProductId:=v_lProductId), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Apply the previous discount
            If dDiscountPercentage = 0 And crDiscountedPremium = 0 Then
                ' Update the Insurance File if the discount is to be re - applied
                crDiscountedPremium = crAppliedDiscountPremium
                dDiscountPercentage = dAppliedDiscountPercentage
                lMatchDiscountedPremium = lAppliedMatchDiscountPremium
                lDiscountReasonId = lAppliedDiscountReasonId

                m_lReturn = CType(UpdatePolicyDiscounts(v_lInsuranceFileCnt, crDiscountedPremium, dDiscountPercentage, lDiscountReasonId, lMatchDiscountedPremium, lAppliedDiscountRecurringTypeId), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdatePolicyDiscounts Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' begin transaction
            lReturn = BeginTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "BeginTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' apply discount percentage to rating sections
            lReturn = CType(ApplyPolicyDiscount(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dDiscountPercentage:=dDiscountPercentage), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ApplyPolicyDiscount Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' recreate perils based on modified rating sections
            lReturn = CType(AddPerils(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddPerils Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' recalculate reinsurance
            lReturn = CType(RecalculateReinsurance(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sFailureReason:=r_sFailureReason), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RecalculateReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' recalculate all risk / peril level fees and taxes
            lReturn = CType(RecalculateFeesAndTaxes(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lTransactionTypeId:=lTransactionTypeId, v_sTransactionType:=v_sTransactionType, v_lProductId:=v_lProductId, v_lTask:=v_lTask), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RecalculateFeesAndTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' process any required adjustments of value based fees / taxes
            lReturn = CType(ProcessAdjustments(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dDiscountPercentage:=dDiscountPercentage, v_crDiscountedPremium:=crDiscountedPremium, v_lMatchDiscountedPremium:=lMatchDiscountedPremium), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ProcessAdjustments Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' update the discounted risks "is_discounted" indicator
            lReturn = CType(UpdateRiskDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lIsDiscounted:=kRiskIsDiscounted), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdateDiscountedRisksIndicator Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' commit transaction
            lReturn = CommitTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CommitTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

            ' rollback transaction
            lReturn = RollbackTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RollbackTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: RecalculatePolicyTaxes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function RecalculatePolicyTaxes(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTask As Integer, ByVal v_sTransactionType As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RecalculatePolicyTaxes"
        Dim oSIRRITax As New bSIRRITax.Business
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue












            oSIRRITax = New bSIRRITax.Business
            m_lReturn = oSIRRITax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lReturn = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            oSIRRITax.SetProcessModes(vTask:=m_iTask)
            lReturn = oSIRRITax.RecalculatePolicyTaxes(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lTask:=0, v_sTransactionType:=v_sTransactionType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RecalculatePolicyTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            If Not oSIRRITax Is Nothing Then
                oSIRRITax.Dispose()
                oSIRRITax = Nothing
            End If
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: RecalculatePolicyFees
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function RecalculatePolicyFees(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_lTransactionTypeId As Integer, Optional ByVal v_bUseExistingFeeDetails As Boolean = True) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RecalculatePolicyFees"
        Dim oSIRPartyFee As New bSIRPartyFee.UBusiness

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            oSIRPartyFee = New bSIRPartyFee.UBusiness
            lReturn = oSIRPartyFee.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lReturn = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            lReturn = oSIRPartyFee.RecalculatePolicyFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lProductId:=v_lProductId, v_lTransactionTypeId:=v_lTransactionTypeId, v_bUseExistingFeeDetail:=v_bUseExistingFeeDetails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bSIRPartyFee.UBusiness.RecalculatePolicyFees Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            If Not oSIRPartyFee Is Nothing Then
                oSIRPartyFee.Dispose()
                oSIRPartyFee = Nothing
            End If



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ApplyPolicyDiscount
    '
    ' Parameters: n/a
    '
    ' Description: Updates the Rating Section "This Premium" field
    '                   with the relevant discount percentage
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function ApplyPolicyDiscount(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dDiscountPercentage As Double) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ApplyPolicyDiscount"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
        AddInputParameter(v_sName:="discount_percentage", v_vValue:=v_dDiscountPercentage, v_iType:=gPMConstants.PMEDataType.PMDouble)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kApplyPolicyDiscountSQL, sSQLName:=kApplyPolicyDiscountName, bStoredProcedure:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kApplyPolicyDiscountSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AdjustPolicyDiscount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function AdjustPolicyDiscount(ByVal v_lInsuranceFileCnt As Integer, ByVal v_crDiscountAdjustment As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AdjustPolicyDiscount"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
        AddInputParameter(v_sName:="discount_adjustment", v_vValue:=v_crDiscountAdjustment, v_iType:=gPMConstants.PMEDataType.PMCurrency)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kAdjustPolicyDiscountSQL, sSQLName:=kAdjustPolicyDiscountName, bStoredProcedure:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kAdjustPolicyDiscountSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AdjustValuesFees
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function AdjustValuesFees(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dDiscountPercentage As Double) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AdjustValuesFees"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

        AddInputParameter(v_sName:="discount_percentage", v_vValue:=v_dDiscountPercentage, v_iType:=gPMConstants.PMEDataType.PMDouble)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kAdjustValuesFeesSQL, sSQLName:=kAdjustValuesFeesName, bStoredProcedure:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kAdjustValuesFeesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AdjustValuesTaxes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AdjustValuesTaxes) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AdjustValuesTaxes(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dDiscountPercentage As Double) As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "AdjustValuesTaxes"
    '
    'Dim lReturn As gPMConstants.PMEReturnCode
    '
    'On Error GoTo Catch_Renamed
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Clear Down Database Parameters
    'm_oDatabase.Parameters.Clear()
    '
    ' Add Required Stored Procedure Parameters
    'AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
    'AddInputParameter(v_sName:="discount_percentage", v_vValue:=v_dDiscountPercentage, v_iType:=gPMConstants.PMEDataType.PMDouble)
    '
    ' Execute Action Query
    'lReturn = m_oDatabase.SQLAction(sSQL:=kAdjustValuesTaxesSQL, sSQLName:=kAdjustValuesTaxesName, bStoredProcedure:=True)
    '
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, kAdjustValuesTaxesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    '
    'Return result
    'Resume 
    'Return result
    'End Function

    ' ***************************************************************** '
    ' Name: GetPolicyDiscountTotalPremium
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function GetPolicyDiscountTotalPremium(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyDiscountTotalPremium"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetPolicyDiscountTotalPremiumSQL, sSQLName:=kGetPolicyDiscountTotalPremiumName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetPolicyDiscountTotalPremiumSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyDiscountDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function GetPolicyDiscountDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, ByRef r_crPremium As Decimal, ByRef r_crDiscountedPremium As Decimal, ByRef r_dDiscountPercentage As Double, ByRef r_lMatchDiscountedPremium As Integer, ByRef r_lTransactionTypeId As Integer, ByRef r_lProductId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyDiscountDetails"
        Const kThisPolicy As Integer = 0

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vPremiumDetails As Object = Nothing
        Dim vPolicyDiscountDetails As Object = Nothing
        Dim vInsuranceFileDetails As Object = Nothing




        result = gPMConstants.PMEReturnCode.PMTrue

        ' get the insurance file details discount details
        lReturn = CType(GetInsuranceFileDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResults:=vInsuranceFileDetails), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetInsuranceFileDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetInsuranceFileDetails Failed to return any data", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' get the total premium details
        lReturn = CType(GetPolicyDiscountTotalPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResults:=vPremiumDetails), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountTotalPremium  Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountTotalPremium Failed to return any data", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' get the policy discount required details
        lReturn = CType(GetPolicyDiscountRequiredInfo(v_sTransactionType:=v_sTransactionType, r_vResults:=vPolicyDiscountDetails), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountRequiredInfo Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountRequiredInfo Failed to return any data", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' return details

        ' insurance file details

        r_crDiscountedPremium = CDec(gPMFunctions.ToSafeCurrency(vInsuranceFileDetails(kInsFileDiscountedPremium, kThisPolicy), 0))

        r_dDiscountPercentage = gPMFunctions.ToSafeDouble(vInsuranceFileDetails(kInsFileDiscountPercentage, kThisPolicy), 0)

        r_lMatchDiscountedPremium = gPMFunctions.ToSafeLong(gPMFunctions.ToSafeDouble(vInsuranceFileDetails(kInsFileMatchDiscountedPremium, kThisPolicy), 0), 0)

        ' If r_lProductId is 0, get it from insurance file details
        If r_lProductId = 0 Then
            r_lProductId = gPMFunctions.NullToLong(vInsuranceFileDetails(kInsFileMatchProductId, kThisPolicy)) ' Adjust index as needed based on stored procedure result
        End If

        ' insurance file premium details

        r_crPremium = CDec(gPMFunctions.ToSafeCurrency(vPremiumDetails(kInsFileTotalPremium, kThisPolicy), 0))

        ' policy discount required details

        r_lTransactionTypeId = CInt(gPMFunctions.NullToLong(vPolicyDiscountDetails(kPolicyDiscountTransactionTypeId, kThisPolicy)))

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePolicyPremium
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function UpdatePolicyPremium(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePolicyPremium"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kUpdatePolicyPremiumSQL, sSQLName:=kUpdatePolicyPremiumName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kUpdatePolicyPremiumSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateRiskPremium
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function UpdateRiskPremium(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateRiskPremium"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateRiskPremiumSQL, sSQLName:=kUpdateRiskPremiumName, bStoredProcedure:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kUpdateRiskPremiumSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ProcessRollbackDiscount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function ProcessRollbackDiscount(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_sTransactionType As String, ByVal v_lTask As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessRollbackDiscount"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim crOriginalPremium As Decimal
        Dim dDiscountPercentage As Double
        Dim crDiscountedPremium As Decimal
        Dim lMatchDiscountedPremium, lTransactionTypeId As Integer
        Dim sFailureReason As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the required policy discount details
            lReturn = CType(GetPolicyDiscountDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sTransactionType:=v_sTransactionType, r_crPremium:=crOriginalPremium, r_crDiscountedPremium:=crDiscountedPremium, r_dDiscountPercentage:=dDiscountPercentage, r_lMatchDiscountedPremium:=lMatchDiscountedPremium, r_lTransactionTypeId:=lTransactionTypeId, r_lProductId:=v_lProductId), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' begin transaction
            lReturn = BeginTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "BeginTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' update all rating sections to use original values
            lReturn = CType(RollbackPolicyDiscount(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dDiscountPercentage:=dDiscountPercentage), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ApplyPolicyDiscount Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' recreate perils based on modified rating sections
            lReturn = CType(AddPerils(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' recalculate reinsurance
            lReturn = CType(RecalculateReinsurance(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sFailureReason:=sFailureReason), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RecalculateReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' recalculate all risk / peril / policy level fees and taxes
            lReturn = CType(RecalculateFeesAndTaxes(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lTransactionTypeId:=lTransactionTypeId, v_sTransactionType:=v_sTransactionType, v_lProductId:=v_lProductId, v_lTask:=v_lTask), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RecalculateFeesAndTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' update the risks premium from the newly discounted rating section values
            lReturn = CType(UpdateRiskPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdateRiskPremium Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' updated the policy premium from the newly discounted risk premiums
            lReturn = CType(UpdatePolicyPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePolicyPremium Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            ' rollback the policy premium from the newly discounted risk premiums

            'Developer Guide No. 98
            lReturn = CType(UpdatePolicyDiscounts(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, crDiscountPremium:=ACRollBackDicountedPremium, dDiscountPercentage:=ACRollBackDicountedPercentage, lDiscountReasonId:=ACRollBackDicountedReasonId, lMatchDiscountPremium:=ACRollBackMatchDicountedPremium), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePolicyDiscounts Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' update the discounted risks "is_discounted" indicator
            lReturn = CType(UpdateRiskDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lIsDiscounted:=kRiskIsNotDiscounted), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdateDiscountedRisksIndicator Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' commit transaction
            lReturn = CommitTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CommitTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePolicyDiscounts
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav : 02-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function UpdatePolicyDiscounts(ByVal v_lInsuranceFileCnt As Integer, ByVal crDiscountPremium As Decimal, ByVal dDiscountPercentage As Double, ByVal lDiscountReasonId As Integer, ByVal lMatchDiscountPremium As Integer, Optional ByVal lDiscountRecurringTypeId As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePolicyPremium"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="discounted_premium", v_vValue:=crDiscountPremium, v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="discount_percentage", v_vValue:=dDiscountPercentage, v_iType:=gPMConstants.PMEDataType.PMDouble), gPMConstants.PMEReturnCode)
            If lDiscountReasonId = 0 Then
                m_lReturn = CType(AddInputParameter(v_sName:="discount_reason_id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="discount_reason_id", v_vValue:=lDiscountReasonId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            m_lReturn = CType(AddInputParameter(v_sName:="match_discount_premium", v_vValue:=lDiscountReasonId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If lDiscountRecurringTypeId = 0 Then
                m_lReturn = CType(AddInputParameter(v_sName:="discount_recurring_type_id", v_vValue:=DBNull.Value, v_iType:=gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(AddInputParameter(v_sName:="discount_recurring_type_id", v_vValue:=lDiscountRecurringTypeId, v_iType:=gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            End If
            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kUpdatePolicyDiscountSQL, sSQLName:=kUpdatePolicyDiscountName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kUpdatePolicyDiscountSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: IsDiscountApplied
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav : 02-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function IsDiscountApplied(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "IsDicountApplied"

        Dim lReturn As gPMConstants.PMEReturnCode


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            '    m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=PMLong)
            '    m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_discount_applied", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            ' Execute Select Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kIsDiscountAppliedSQL, sSQLName:=kIsDiscountAppliedName, bStoredProcedure:=True)


            r_vResults = m_oDatabase.Parameters.Item("is_discount_applied").Value

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kUpdatePolicyPremiumSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyDiscountRequiredInfo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function GetPolicyDiscountRequiredInfo(ByVal v_sTransactionType As String, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyDiscountRequiredInfo"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="code", v_vValue:=v_sTransactionType, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLSelect(sSQL:=kGetPolicyDiscountRequiredInfoSQL, sSQLName:=kGetPolicyDiscountRequiredInfoName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, kGetPolicyDiscountRequiredInfoSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateRiskDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function UpdateRiskDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lIsDiscounted As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateRiskDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            lReturn = CType(AddInputParameter(v_sName:="is_discounted", v_vValue:=v_lIsDiscounted, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateRiskDetailsSQL, sSQLName:=kUpdateRiskDetailsName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kUpdateRiskDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: RecalculateFeesAndTaxes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function RecalculateFeesAndTaxes(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTransactionTypeId As Integer, ByVal v_sTransactionType As String, ByVal v_lProductId As Integer, ByVal v_lTask As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RecalculateFeesAndTaxes"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' update the risks premium from the newly discounted rating section values
        lReturn = CType(UpdateRiskPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "UpdateRiskPremium Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' updated the policy premium from the newly discounted risk premiums
        lReturn = CType(UpdatePolicyPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "UpdatePolicyPremium Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' update the tax premiums if any of the items have been manually
        ' changed as this will mean items are not recalculated
        ' but the existing tax items are used but based on the new premiums
        lReturn = CType(UpdateTaxPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "UpdateTaxPremium Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' recalculate risk / peril fee based on new rating section premiums
        lReturn = CType(RecalculateRiskFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lTransactionTypeId:=v_lTransactionTypeId), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "RecalculateRiskFees Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' recalculate risk / peril taxes based on new rating section premiums
        lReturn = CType(RecalculateRiskTaxes(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lTask:=v_lTask, v_sTransactionType:=v_sTransactionType), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "RecalculateRiskTaxes Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        '    ' recalculate policy level fees based on new rating section premiums
        '    lReturn = RecalculatePolicyFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, _
        ''                                    v_lProductId:=v_lProductId, _
        ''                                    v_lTransactionTypeId:=v_lTransactionTypeId)
        '    If lReturn <> PMTrue Then
        '        RaiseError kMethodName, "RecalculatePolicyFees Failed", PMLogError
        '    End If
        '
        '    ' recalculate policy level taxes based on new rating section premiums
        '    lReturn = RecalculatePolicyTaxes(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, _
        ''                                     v_lTask:=v_lTask, _
        ''                                     v_sTransactionType:=v_sTransactionType)
        '    If lReturn <> PMTrue Then
        '        RaiseError kMethodName, "RecalculatePolicyTaxes Failed", PMLogError
        '    End If

        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ProcessAdjustments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function ProcessAdjustments(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dDiscountPercentage As Double, ByVal v_crDiscountedPremium As Decimal, ByVal v_lMatchDiscountedPremium As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessAdjustments"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vPremiumDetails As Object = Nothing
        Dim crCurrentPremium, crRequiredDiscountAdjustment As Decimal




        result = gPMConstants.PMEReturnCode.PMTrue

        ' apply the specified discount percentage to any value based fees
        lReturn = CType(AdjustValuesFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dDiscountPercentage:=v_dDiscountPercentage), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "AdjustValuesFees Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        '****************************************************************************************
        ' IMPORTANT NB: Value based taxes cannot be altered; so if there are any value based taxes
        ' this will result in a larger than normal adjustment of the final rating section
        '****************************************************************************************

        ' update the risks premium from the newly discounted rating section values
        lReturn = CType(UpdateRiskPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "UpdateRiskPremium Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' updated the policy premium from the newly discounted risk premiums
        lReturn = CType(UpdatePolicyPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "UpdatePolicyPremium Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' get the update total premium
        lReturn = CType(GetPolicyDiscountTotalPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResults:=vPremiumDetails), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountTotalPremium Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' with the new premium details
        If Informations.IsArray(vPremiumDetails) Then
            ' get the new total premium

            crCurrentPremium = CDec(gPMFunctions.ToSafeCurrency(vPremiumDetails(kInsFileTotalPremium, kThisPolicy), 0))
        End If

        ' if the discount is required to match a specified discounted premium amount
        If v_lMatchDiscountedPremium Then
            ' if the current total premium does not match the specified discounted premium
            If crCurrentPremium <> v_crDiscountedPremium Then
                ' determine the amount we need to update the current premium
                ' by to match the discounted premium
                crRequiredDiscountAdjustment = v_crDiscountedPremium - crCurrentPremium
            End If
        End If

        If crRequiredDiscountAdjustment <> 0 Then

            ' adjust the policy discount
            lReturn = CType(AdjustPolicyDiscount(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_crDiscountAdjustment:=crRequiredDiscountAdjustment), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AdjustPolicyDiscount Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' update the risks premium from the newly discounted rating section values
            lReturn = CType(UpdateRiskPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdateRiskPremium Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' updated the policy premium from the newly discounted risk premiums
            lReturn = CType(UpdatePolicyPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePolicyPremium Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: RollbackPolicyDiscount
    '
    ' Parameters: n/a
    '
    ' Description: Updates the Rating Section "This Premium" field
    '                   with the relevant discount percentage
    '
    ' History:
    '           Created : MEvans : 01-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function RollbackPolicyDiscount(ByVal v_lInsuranceFileCnt As Integer, ByVal v_dDiscountPercentage As Double) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RollbackPolicyDiscount"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kRollbackPolicyDiscountSQL, sSQLName:=kRollbackPolicyDiscountName, bStoredProcedure:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kRollbackPolicyDiscountSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function

    'Ashwani - (RFC_Enable_PrePayment_functionality)
    Public Function GetPrePaymentOptionValue(ByVal v_lproductid As Integer, ByRef r_Prepayment(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPrePaymentOptionValue"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = CType(AddInputParameter(v_sName:="product_id", v_vValue:=v_lproductid, v_iType:=gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetPrepaymentOPtionValSQL, sSQLName:=kGetPrepaymentOPtionVal, bStoredProcedure:=True, vResultArray:=r_Prepayment, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PrePaymentOptionValue Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ProcessPolicyMakeLive
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function ProcessPolicyMakeLive(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_bIsValid As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPolicyMakeLive"
        'Dim bIsValid As Boolean = False
        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = ValidateCertificateYear(r_bIsValid, v_lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to getCertificate Year for " & m_sTransactionType & ".", gPMConstants.PMELogLevel.PMLogError)
            End If
            'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError(kMethodName, "ClearRatingsDiscountRelatedDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            'End If
            If r_bIsValid = False Then
                '  gPMFunctions.RaiseError(kMethodName, "You Cannot Make This Transaction Live- Please check the Certificate Year Configuration of Sub Agent", gPMConstants.PMELogLevel.PMLogError)
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="You Cannot Make This Transaction Live- Please check the Certificate Year Configuration of Sub Agent", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPolicyMakeLive", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            ' update rating sections to remove any original premium values
            lReturn = CType(ClearRatingsDiscountRelatedDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ClearRatingsDiscountRelatedDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' update risks to for make live process
            lReturn = CType(ProcessPolicyMakeLiveRisks(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ProcessPolicyMakeLiveRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ClearRatingsDiscountRelatedDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function ClearRatingsDiscountRelatedDetails(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ClearRatingsDiscountRelatedDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kClearRatingsDiscountRelatedDetailsSQL, sSQLName:=kClearRatingsDiscountRelatedDetailsName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kClearRatingsDiscountRelatedDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessPolicyMakeLiveRisks
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 13-12-2005 : discount / loading
    ' ***************************************************************** '
    Public Function ProcessPolicyMakeLiveRisks(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPolicyMakeLiveRisks"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kProcessPolicyMakeLiveRisksSQL, sSQLName:=kProcessPolicyMakeLiveRisksName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kProcessPolicyMakeLiveRisksSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddPerils
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 15-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function AddPerils(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddPerils"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kAddPerilsSQL, sSQLName:=kAddPerilsName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kAddPerilsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessPolicyPreMakeLive
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 16-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function ProcessPolicyPreMakeLive(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer, ByVal v_sTransactionType As String, ByVal v_lTask As Integer, ByRef r_sInvalidRiskMessage As String, ByVal v_lPolicyDiscountStatus As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPolicyPreMakeLive"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lInvalidRiskCount As Integer
        Dim vInvalidRisks(,) As Object = Nothing
        Dim llBound, lUBound As Integer
        Dim bTransStarted As Boolean
        Dim sInvalidRiskMessage As New StringBuilder
        Dim lRiskCnt As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get any invalid risks
            lReturn = CType(GetInvalidPolicyDiscountRisks(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResults:=vInvalidRisks), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInvalidPolicyDiscountRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get the number of invalid risks
            If Informations.IsArray(vInvalidRisks) Then


                llBound = vInvalidRisks.GetLowerBound(1)

                lUBound = vInvalidRisks.GetUpperBound(1)

                If lUBound = 0 Then
                    lInvalidRiskCount = 1
                Else
                    lInvalidRiskCount = lUBound
                End If

                sInvalidRiskMessage = New StringBuilder("The following risks have an invalid return premium, and have been been reset to 'UNQUOTED'." & Strings.ChrW(13) & Strings.ChrW(10))

                ' generate invalid risk message
                For lRisk As Integer = llBound To lUBound





                    sInvalidRiskMessage.Append(
                                                   " Risk: " & CStr(vInvalidRisks(4, 0)) & " - " & CStr(vInvalidRisks(3, 0)) &
                                                   " has a total billed premium of " & CStr(vInvalidRisks(1, 0)) &
                                                   " and a total return premium of " & CStr(vInvalidRisks(2, 0)) & Strings.ChrW(13) & Strings.ChrW(10))

                Next

            End If

            ' ensure that the invalid risk message is returned
            r_sInvalidRiskMessage = sInvalidRiskMessage.ToString()

            ' if there are invalid risks
            If lInvalidRiskCount > 0 Then

                ' start transaction
                lReturn = BeginTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Begin Transaction Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                bTransStarted = True

                ' only allow policy discount to be rolled back if is applicable
                If v_lPolicyDiscountStatus = kPolicyDiscountStatusAllowRollback Then

                    ' rollback any applied discount
                    lReturn = CType(ProcessRollbackDiscount(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lProductId:=v_lProductId, v_sTransactionType:=v_sTransactionType, v_lTask:=v_lTask), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ProcessRollbackDiscount Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                End If

                ' update invalid risks to "UNQUOTED"
                For lRisk As Integer = llBound To lUBound


                    lRiskCnt = CInt(vInvalidRisks(0, 0))

                    ' update the invalid risks status - set them to "UNQUOTED"
                    lReturn = CType(UpdateRiskStatus(lRiskCnt, kRiskStatusCodeUNQUOTED), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "UpdateInvalidRisks Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Next

                ' commit transaction
                lReturn = CommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Commit Transaction Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            If bTransStarted Then
                lReturn = RollbackTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Rollback Transaction Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetInvalidPolicyDiscountRisks
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 16-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function GetInvalidPolicyDiscountRisks(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInvalidPolicyDiscountRisks"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetInvalidPolicyDiscountRisksSQL, sSQLName:=kGetInvalidPolicyDiscountRisksName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetInvalidPolicyDiscountRisksSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: UpdateRiskStatus
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 16-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function UpdateRiskStatus(ByVal v_lRiskCnt As Integer, ByVal v_sRiskStatusCode As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateRiskStatus"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = CType(AddInputParameter(v_sName:="risk_cnt", v_vValue:=v_lRiskCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            lReturn = CType(AddInputParameter(v_sName:="risk_status_code", v_vValue:=v_sRiskStatusCode, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateRiskStatusSQL, sSQLName:=kUpdateRiskStatusName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kUpdateRiskStatusSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: RecalculateReinsurance
    '
    ' Parameters: n/a
    '
    ' Description: recalculates the reinsurance for the relevant risks
    '
    ' History:
    '           Created : MEvans : 05-01-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function RecalculateReinsurance(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sFailureReason As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RecalculateReinsurance"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim llBound, lUBound, lRiskCnt As Integer
        Dim bIsRIValid As Boolean
        Dim vRisks(,) As Object = Nothing

        Dim lReinsPremiumOrSumInsured As Object
        Dim lReinsBand As Object

        


        result = gPMConstants.PMEReturnCode.PMTrue

        ' set reinsurance properties
        'Developer Guide No.20
        If Not m_bIsRi2007Enabled Then m_oSIRReinsurance.m_oDatabase = m_oDatabase
        m_oSIRReinsurance.InsuranceFileCnt = v_lInsuranceFileCnt

        ' return the risks to be recalculated
        lReturn = CType(GetPolicyDiscountRisks(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResults:=vRisks), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountRisks Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' if there are risks where the reinsurance needs to be recalculated
        If Informations.IsArray(vRisks) Then

            ' get the array boundaries

            llBound = vRisks.GetLowerBound(1)

            lUBound = vRisks.GetUpperBound(1)

            ' for each risk
            For lRisk As Integer = llBound To lUBound

                ' get risk cnt

                lRiskCnt = CInt(vRisks(0, lRisk))


                m_oSIRReinsurance.RiskId = lRiskCnt

                ' calculate reinsurance

                lReturn = m_oSIRReinsurance.CalculateRI
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "Calculating Reinsurance Failed"
                    gPMFunctions.RaiseError(kMethodName, "bSIRReinsurance.Form.CalculateRI Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' load details to validate

                lReturn = m_oSIRReinsurance.Getdetails
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "bSIRReinsurance.Form.GetDetails Failed"
                    gPMFunctions.RaiseError(kMethodName, "bSIRReinsurance.Form.GetDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' update details, this will ensure minor rounding is handled

                lReturn = m_oSIRReinsurance.Update
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "bSIRReinsurance.Form.Update Failed"
                    gPMFunctions.RaiseError(kMethodName, "bSIRReinsurance.Form.Update Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' validate the reinsurance is all ok

                'lReturn = m_oSIRReinsurance.ValidateBands(r_lValid:=lReinsPremiumOrSumInsured, r_lBand:=lReinsBand)
                '' Must return true AND a zero for lReinsPremiumOrSumInsured
                'bIsRIValid = (lReturn = gPMConstants.PMEReturnCode.PMTrue) And (lReinsPremiumOrSumInsured = 0)
                'If Not bIsRIValid Then
                '    r_sFailureReason = "bSIRReinsurance.Form.ValidateBands Failed"
                '    gPMFunctions.RaiseError(kMethodName, "bSIRReinsurance.Form.ValidateBands Failed", gPMConstants.PMELogLevel.PMLogError)
                'End If

            Next

        End If


        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateRiskTaxPremium
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-01-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function UpdateRiskTaxPremium(ByVal v_lRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateRiskTaxPremium"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        lReturn = CType(AddInputParameter(v_sName:="risk_cnt", v_vValue:=v_lRiskCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateRiskTaxPremiumSQL, sSQLName:=kUpdateRiskTaxPremiumName, bStoredProcedure:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kUpdateRiskTaxPremiumSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePolicyTaxPremium
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 05-01-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function UpdatePolicyTaxPremium(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePolicyTaxPremium"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kUpdatePolicyTaxPremiumSQL, sSQLName:=kUpdatePolicyTaxPremiumName, bStoredProcedure:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kUpdatePolicyTaxPremiumSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateTaxPremium
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 06-01-2005 : Discount / Loading
    ' ***************************************************************** '
    Public Function UpdateTaxPremium(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateTaxPremium"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vRisks(,) As Object = Nothing
        Dim llBound, lUBound, lRiskCnt As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' return the risks to be recalculated
            lReturn = CType(GetPolicyDiscountRisks(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResults:=vRisks), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPolicyDiscountRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' if there are risks where the reinsurance needs to be recalculated
            If Informations.IsArray(vRisks) Then

                ' get the array boundaries

                llBound = vRisks.GetLowerBound(1)

                lUBound = vRisks.GetUpperBound(1)

                ' for each risk
                For lRisk As Integer = llBound To lUBound

                    ' get risk cnt

                    lRiskCnt = CInt(vRisks(0, lRisk))

                    ' update the any existing risk tax entries to use the updated premium
                    ' but only if any of the entries have been manually changed
                    ' otherwise all entries will be calculated correctly when calling "RecalculatePolicyRiskTax"
                    lReturn = CType(UpdateRiskTaxPremium(v_lRiskCnt:=lRiskCnt), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "UpdateRiskTaxPremium Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next

            End If

            ' update policy tax entries to use the updated premium
            ' but only if any of the entries have been manually changed
            ' otherwise all entries will be calculated correctly when calling "RecalculatePolicyTax"
            lReturn = CType(UpdatePolicyTaxPremium(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePolicyTaxPremium Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name:  GetLookupsByEffectiveDate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function GetLookupsByEffectiveDate(ByVal v_sTableName As String, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookupsByEffectiveDate"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="table", v_vValue:=v_sTableName, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetLookupsByEffectiveDateSQL, sSQLName:=kGetLookupsByEffectiveDateName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetLookupsByEffectiveDateSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' *****************************************************************
    ' Get risk and status for this policy version
    ' *****************************************************************
    Public Function GetRiskStatus(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            If m_oDatabase.SQLSelect(sSQL:=ACGetRiskForPolicyIDSQL, sSQLName:=ACGetRiskForPolicyIDName, bStoredProcedure:=ACGetRiskForPolicyIDStored, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetRiskStatus", r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' *****************************************************************
    ' set risk status provided in array
    ' *****************************************************************
    Public Function SetRiskStatusArray(ByVal v_vRiskStatus As Object) As Integer

        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'exit if we don't have anything to do
            If Not Informations.IsArray(v_vRiskStatus) Then
                Return result
            End If

            m_oDatabase.SQLBeginTrans()

            For lCount As Integer = 0 To v_vRiskStatus.GetUpperBound(1)
                m_oDatabase.Parameters.Clear()


                If m_oDatabase.Parameters.Add("RiskID", CStr(CInt(v_vRiskStatus(0, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception
                End If


                If m_oDatabase.Parameters.Add("RiskStatusID", CStr(CInt(v_vRiskStatus(1, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception
                End If

                If m_oDatabase.SQLAction(ACUpdRiskStatusSQL, ACUpdRiskStatusName, ACUpdRiskStatusStored) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Throw New Exception
                End If

            Next lCount

            m_oDatabase.SQLCommitTrans()


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="SetRiskStatusArray", r_lFunctionReturn:=result, excep:=ex)

            m_oDatabase.SQLRollbackTrans()
        Finally



        End Try
        Return result
    End Function

    ' *****************************************************************
    ' reset risk status to unquoted for this policy version
    ' *****************************************************************
    Public Function ResetRiskStatusForPolicyID(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("insurance_file_cnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            If m_oDatabase.SQLAction(ACUpdResetRiskStatusForPolicyIDSQL, ACUpdResetRiskStatusForPolicyIDName, ACUpdResetRiskStatusForPolicyIDStored) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="ResetRiskStatusForPolicyID", r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' *****************************************************************
    ' Set risk's selected value
    ' *****************************************************************
    Public Function SetRiskSelectedValue(ByVal v_lRiskCnt As Integer, ByVal v_iIsSelect As Integer) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("RiskCnt", CStr(v_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            If m_oDatabase.Parameters.Add("IsSelect", CStr(v_iIsSelect), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            If m_oDatabase.SQLAction(ACUpdRiskSelectedSQL, ACUpdRiskSelectedName, ACUpdRiskSelectedStored) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="SetRiskSelectedValue", r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ''' <summary>
    ''' Copy risk during MTA
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="bFromSAM"></param>
    ''' <param name="v_lOnlyRiskCnt"></param>
    ''' <param name="r_lLastNewRiskCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyRisksMTAEx(ByVal v_lInsuranceFileCnt As Integer,
                            Optional ByVal bFromSAM As Boolean = False, Optional ByVal v_lCreateLinkType As Long = 1, Optional ByVal Is_SAM_Copy_Quote As Boolean = False, Optional ByVal v_lOnlyRiskCnt As Integer = 0, Optional ByRef r_lLastNewRiskCnt As Integer = 0, Optional v_bCopyRiskOnMTA As Boolean = False) As Integer
        Dim nResult As Integer
        Dim r_vResultArray As Object = Nothing
        Dim r_vResultArray2(,) As Object
        Dim nCount1 As Integer = 0
        Dim nCount2 As Integer = 0
        Dim nCnt1 As Integer = 0
        Dim nCnt2 As Integer = 0
        Dim nRiskCnt As Integer = 0
        Dim nOriginalRiskCnt As Integer = 0
        Dim m_dProrataRate As Double
        Dim nInsFolderCnt As Integer = 0
        Dim sValue As String = ""
        Dim bMustCopy As Boolean
        Dim oSirPerilAllocation As bSirPerilAllocation.Business
        Dim oSirRiskData As New bSIRRiskData.Business
        Dim oSirRisktax As New bSIRRITax.Business
        Dim r_lNew_risk_folder_cnt As Long
        Dim vArray(,) As Object = Nothing


        Try



            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Check if QBE RI2007 is enabled
            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=1, r_vUnderwriting:=sValue), gPMConstants.PMEReturnCode)

            If sValue = "1" Then
                m_bIsRi2007Enabled = True
            Else
                m_bIsRi2007Enabled = False
            End If

            'Get the object for perilAllocation

            oSirPerilAllocation = New bSirPerilAllocation.Business
            m_lReturn = oSirPerilAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                CopyRisksMTAEx = PMEReturnCode.PMFalse
                Return PMEReturnCode.PMFalse
            End If

            'Get the Object for bSirRiskData
            oSirRiskData = New bSIRRiskData.Business
            m_lReturn = oSirRiskData.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                CopyRisksMTAEx = PMEReturnCode.PMFalse
                Return PMEReturnCode.PMFalse
            End If
            m_lReturn = oSirRiskData.GetUncopiedRisks(v_lInsuranceFileCnt, r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(r_vResultArray) Then
                r_vResultArray2 = Nothing
                'Get Insurance Folder
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                           vValue:=v_lInsuranceFileCnt,
                                           iDirection:=PMEParameterDirection.PMParamInput,
                                           iDataType:=PMEDataType.PMInteger)


                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsFolderCntSQL,
                                                  sSQLName:=ACGetInsFolderCntName,
                                                  bStoredProcedure:=ACGetInsFolderCntStored,
                                                  lNumberRecords:=gPMConstants.PMAllRecords,
                                                  vResultArray:=r_vResultArray2)

                If (m_lReturn <> PMEReturnCode.PMTrue) Then
                    CopyRisksMTAEx = PMEReturnCode.PMFalse
                    Exit Function
                End If

                nInsFolderCnt = r_vResultArray2(0, 0)


                If m_lReturn <> PMEReturnCode.PMTrue Then
                    CopyRisksMTAEx = PMEReturnCode.PMFalse
                    Exit Function
                End If

                nCount1 = r_vResultArray.GetLowerBound(1)

                nCount2 = r_vResultArray.GetUpperBound(1)

                For i As Integer = nCount1 To nCount2

                    nRiskCnt = CInt(r_vResultArray(0, i))

                    nOriginalRiskCnt = CInt(r_vResultArray(0, i))

                    bMustCopy = True
                    If (v_lOnlyRiskCnt > 0) Then
                        If v_lOnlyRiskCnt <> 0 And v_lOnlyRiskCnt <> nOriginalRiskCnt Then
                            bMustCopy = False
                        End If
                    End If


                    If Is_SAM_Copy_Quote = True OrElse v_bCopyRiskOnMTA Then
                        m_lReturn = oSirRiskData.DeleteInsuranceFileRiskLink(v_lInsuranceFileCnt, nRiskCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'Copy Risk Folder
                        m_lReturn = oSirRiskData.CopyRiskFolder(r_vResultArray(2, i), v_lInsuranceFileCnt, r_lNew_risk_folder_cnt)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            r_vResultArray(2, i) = r_lNew_risk_folder_cnt

                        If v_bCopyRiskOnMTA Then
                            'copy Risk and rating section and added Risk Link , with quoted status
                            m_lReturn = oSirRiskData.CopyRisk(v_lInsuranceFileCnt, r_vResultArray, i, nRiskCnt, 0, v_lCreateLinkType)
                        Else
                            'copy Risk and rating section and added Risk Link , with unquoted status
                            m_lReturn = oSirRiskData.CopyRisk(v_lInsuranceFileCnt, r_vResultArray, i, nRiskCnt, 1, v_lCreateLinkType)
                        End If
                   
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                 Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        oSirRisktax = New bSIRRITax.Business
                        m_lReturn = oSirRisktax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            CopyRisksMTAEx = PMEReturnCode.PMFalse
                            Return PMEReturnCode.PMFalse
                        End If
                        m_lReturn = oSirRisktax.UpdateRiskInTaxCalculation(nOriginalRiskCnt, nRiskCnt, v_lInsuranceFileCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'All unedited Risks go through without any pro-rata
                        m_dProrataRate = 1

                        'Del Rating Sections
                        m_oDatabase.Parameters.Clear()

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=CStr(nRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelRatingSectionSQL, sSQLName:=ACDelRatingSectionName, bStoredProcedure:=ACDelRatingSectionStored)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Get Original Rating Sections
                        m_oDatabase.Parameters.Clear()

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=CStr(nOriginalRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        'Fetch the records
                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRatingSectionSQL, sSQLName:=ACSelectRatingSectionName, bStoredProcedure:=ACSelectRatingSectionStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray2)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Add MTA Rating Section and associated perils
                        If Informations.IsArray(r_vResultArray2) Then


                            nCnt1 = r_vResultArray2.GetLowerBound(1)

                            nCnt2 = r_vResultArray2.GetUpperBound(1)

                            For j As Integer = nCnt1 To nCnt2
                                m_lReturn = CType(CopyRatingSectionsAndPerils(r_vResultArray2, -1, 1, v_lInsuranceFileCnt, nRiskCnt, j, m_dProrataRate), gPMConstants.PMEReturnCode)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            Next

                            For j As Integer = nCnt1 To nCnt2
                                m_lReturn = CType(CopyRatingSectionsAndPerils(r_vResultArray2, 1, 0, v_lInsuranceFileCnt, nRiskCnt, j, m_dProrataRate), gPMConstants.PMEReturnCode)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            Next

                        End If

                        m_oDatabase.Parameters.Clear()

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=CStr(nOriginalRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProrataRateForUneditedRiskSQL, sSQLName:=ACGetProrataRateForUneditedRiskName, bStoredProcedure:=ACGetProrataRateForUneditedRiskStored, vResultArray:=vArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If Informations.IsArray(vArray) Then

                            m_dProrataRate = CDbl(vArray(0, 0))
                        End If

                        'Update Risk
                        m_oDatabase.Parameters.Clear()

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=CStr(nRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                        m_lReturn = m_oDatabase.Parameters.Add(sName:="Pro_rata_rate", vValue:=If(CStr(m_dProrataRate) Is DBNull.Value.ToString(), CStr(0), CStr(m_dProrataRate)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskSQL, sSQLName:=ACUpdateRiskName, bStoredProcedure:=ACUpdateRiskStored)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        r_vResultArray2 = Nothing
                        'Get Insurance Folder
                        m_oDatabase.Parameters.Clear()

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsFolderCntSQL, sSQLName:=ACGetInsFolderCntName, bStoredProcedure:=ACGetInsFolderCntStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray2)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        nInsFolderCnt = CInt(r_vResultArray2(0, 0))

                        'Copy GIS Screen Details
                        m_lReturn = CType(CopyGISRiskScreenDetails(nOriginalRiskCnt, nRiskCnt, nInsFolderCnt), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'If Not bFromSAM Then
                        'Copy Reinsurance Details
                        m_lReturn = CType(CopyRIDetailsMTA(v_lInsuranceFileCnt, nRiskCnt, nOriginalRiskCnt), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'End If
                    ElseIf bMustCopy Then
                        m_lReturn = oSirRiskData.DeleteInsuranceFileRiskLink(v_lInsuranceFileCnt, nRiskCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'copy Risk and rating section and added Risk Link , with unquoted status
                        m_lReturn = oSirRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lInsuranceFileCnt, v_vRiskDetail:=r_vResultArray, v_lPosNo:=i, r_lRiskCnt:=nRiskCnt, v_lResetStatus:=0, v_lCreateLinkType:=If(m_sTransactionType = "REN", 0, 1))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Copy GIS Screen Details
                        m_lReturn = CopyGISRiskScreenDetails(nOriginalRiskCnt, nRiskCnt, nInsFolderCnt)
                        If (m_lReturn <> PMEReturnCode.PMTrue) Then
                            CopyRisksMTAEx = PMEReturnCode.PMFalse
                            Exit Function
                        End If

                        oSirPerilAllocation.TransactionType = m_sTransactionType '"MTA" EM
                        oSirPerilAllocation.InsuranceFolderCnt = nInsFolderCnt
                        oSirPerilAllocation.InsuranceFileCnt = v_lInsuranceFileCnt
                        oSirPerilAllocation.RiskID = nRiskCnt

                        m_lReturn = oSirPerilAllocation.PopulateRatingSections
                        If (m_lReturn <> PMEReturnCode.PMTrue) Then
                            CopyRisksMTAEx = PMEReturnCode.PMFalse
                            Exit Function
                        End If

                        'Update risk with values from sections an perils
                        m_lReturn = oSirPerilAllocation.UpdateRisk
                        If (m_lReturn <> PMEReturnCode.PMTrue) Then
                            CopyRisksMTAEx = PMEReturnCode.PMFalse
                            Exit Function
                        End If

                        'Do risk level reinsurance
                        m_lReturn = ProcessRiskReinsurance(
                                    v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                    v_lRiskId:=nRiskCnt,
                                    v_sTransactionType:=m_sTransactionType)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            CopyRisksMTAEx = PMEReturnCode.PMFalse
                            Exit Function
                        End If
                        r_lLastNewRiskCnt = nRiskCnt

                    End If

                Next
            End If


        Catch ex As Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRisksMTAEx Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRisksMTAEx", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally
            If Not oSirRiskData Is Nothing Then
                oSirRiskData.Dispose()
                oSirRiskData = Nothing
            End If
            If Not oSirRisktax Is Nothing Then
                oSirRisktax.Dispose()
                oSirRisktax = Nothing
            End If

        End Try
        Return nResult
    End Function

    Public Function CopyRIDetailsMTA(ByVal v_lInsuranceFileCnt As Integer, ByVal r_lRiskCnt As Integer, ByVal r_lOriginalRiskCnt As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=CStr(r_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyRiskRIDetailsSQL, sSQLName:=ACCopyRiskRIDetailsName, bStoredProcedure:=ACCopyRiskRIDetailsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get The Premium and update Ri_Arrangement
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Original_flag", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=CStr(r_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Original_Risk_Cnt", vValue:=CStr(r_lOriginalRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRIDetailsSQL, sSQLName:=ACUpdateRIDetailsName, bStoredProcedure:=ACUpdateRIDetailsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch ex As Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRIDetailsMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRIDetailsMTA", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Public Function CopyGISRiskScreenDetails(ByVal r_lOriginalRiskCnt As Integer, ByVal r_lNewRiskCnt As Integer, ByVal r_lInsFolderCnt As Integer) As Integer


        Dim result As Integer = 0
        Dim r_vResultArray2(,) As Object = Nothing
        Dim m_sGisModelCode, m_sGisModelCodeSQL As String
        Dim m_lNewGisId As Integer
        Dim m_sQuoteRef As String = String.Empty
        Dim m_sQuotePwd As String
        Dim MyValue As Integer
        Dim oBusinessFindInsurance As bSIRFindInsurance.Form
        Dim oGISBusiness As Object
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            oGISBusiness = Nothing
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=oGISBusiness, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bGIS.Application"
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bGIS.Application", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'm_lReturn = oGISBusiness.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            'If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            '    CopyGISRiskScreenDetails = m_lReturn
            '    Return result
            'End If

            oBusinessFindInsurance = New bSIRFindInsurance.Form
            m_lReturn = oBusinessFindInsurance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageId:=m_iLanguageID, iCurrencyId:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CopyGISRiskScreenDetails = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            'get GIS model Code
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_cnt", vValue:=CStr(r_lOriginalRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectGISDataModelSQL, sSQLName:=ACSelectGISDataModelName, bStoredProcedure:=ACSelectGISDataModelStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray2)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(r_vResultArray2) Then

                'Copy GIS Details

                m_sGisModelCode = CStr(r_vResultArray2(0, 0))


                If m_sGisModelCode.Trim() <> "" Then
                    m_sGisModelCodeSQL = "spg_" & m_sGisModelCode.Trim() & "_copy_dataset"

                    'Create Dataset
                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Old_gis_policy_link_id", vValue:=CStr(r_vResultArray2(1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="old_insurance_file_cnt", vValue:=CStr(r_lInsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="old_risk_id", vValue:=CStr(r_lOriginalRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="new_insurance_file_cnt", vValue:=CStr(r_lInsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="new_risk_id", vValue:=CStr(r_lNewRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="copy_quotes", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="new_quote_ref", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="new_quote_ref_password", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="new_gis_policy_link_id", vValue:=CStr(m_lNewGisId), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=m_sGisModelCodeSQL, sSQLName:="GisModelCodeName", bStoredProcedure:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Get New GIS Policy Link ID
                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(r_lNewRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectGISPolicyLinkIDSQL, sSQLName:=ACSelectGISPolicyLinkIDName, bStoredProcedure:=ACSelectGISPolicyLinkIDStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    Else
                        m_lNewGisId = m_oDatabase.Records.Fields("gis_policy_link_id")
                    End If

                    'Generate GIS Quote Ref
                    m_lReturn = oGISBusiness.GenerateQuoteRef(ToSafeInteger(m_lNewGisId), ToSafeString(m_sQuoteRef), ToSafeString(m_sGisModelCode))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Generate Gis Quote Password

                    m_sQuotePwd = ""
                    Dim rnd As New Random

                    ' Generate 8 Chars
                    For lSub As Integer = 1 To 8

                        ' Generate random value between 65 and 91.
                        ' i.e. ASCII A to Z - but exclude any vowels
                        Do
                            MyValue = Math.Floor(rnd.Next(65, 91))
                        Loop While MyValue = 65 Or MyValue = 69 Or MyValue = 73 Or MyValue = 79 Or MyValue = 85


                        ' Convert to a Char and add to Password
                        m_sQuotePwd = m_sQuotePwd & Strings.ChrW(MyValue).ToString()

                    Next lSub

                    'Update Quote Ref

                    m_lReturn = oGISBusiness.UpdateQuoteRef(ToSafeInteger(m_lNewGisId), ToSafeString(m_sQuoteRef), ToSafeString(m_sQuotePwd), ToSafeString(m_sGisModelCode))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = oBusinessFindInsurance.CopyRiskStandardWordings(r_vResultArray2(1, 0), m_lNewGisId, m_sGisModelCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyGISRiskScreenDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyGISRiskScreenDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally
            oGISBusiness = Nothing
            oBusinessFindInsurance = Nothing

        End Try
        Return result
    End Function


    Public Function CopyRatingSectionsAndPerils(ByVal r_vResultArray(,) As Object, ByVal i_ThisPremiumSign As Integer, ByVal i_OriginalFlag As Integer, ByVal m_lInsuranceFileCnt As Integer, ByVal m_lRiskCnt As Integer, ByRef iIndex As Integer, Optional ByRef dProrata As Double = 0) As Integer

        Dim result As Integer = 0
        Dim m_lPolicyRatingSectionTypeId As Integer
        Dim m_cThisPremium As Decimal
        Dim m_lInsuranceFileNoOfDp As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lPolicyRatingSectionTypeId = -1
            If False Then

                m_cThisPremium = i_ThisPremiumSign * CDbl(r_vResultArray(5, iIndex))
            Else

                m_cThisPremium = i_ThisPremiumSign * (CDbl(r_vResultArray(5, iIndex)) * dProrata)
            End If
            m_lInsuranceFileNoOfDp = 2

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="rating_section_type_id", vValue:=CStr(r_vResultArray(10, iIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_section_type_id", vValue:=CStr(m_lPolicyRatingSectionTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=CStr(m_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="sum_insured", vValue:=CStr(r_vResultArray(4, iIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="annual_rate", vValue:=CStr(r_vResultArray(3, iIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="annual_premium", vValue:=CStr(r_vResultArray(6, iIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="this_premium", vValue:=CStr(m_cThisPremium), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="rate_type_id", vValue:=CStr(r_vResultArray(12, iIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_no_of_dp", vValue:=CStr(m_lInsuranceFileNoOfDp), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="original_flag", vValue:=CStr(i_OriginalFlag), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)



            m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=(If(CStr(r_vResultArray(14, iIndex)) = "", DBNull.Value, r_vResultArray(14, iIndex))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)



            m_lReturn = m_oDatabase.Parameters.Add(sName:="country_id", vValue:=(If(CStr(r_vResultArray(15, iIndex)) = "", DBNull.Value, r_vResultArray(15, iIndex))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)



            m_lReturn = m_oDatabase.Parameters.Add(sName:="state_id", vValue:=(If(CStr(r_vResultArray(16, iIndex)) = "", DBNull.Value, r_vResultArray(16, iIndex))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_amended", vValue:=(r_vResultArray(17, iIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="calculated_premium", vValue:=(gPMFunctions.ToSafeCurrency(r_vResultArray(18, iIndex), 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="override_reason", vValue:=(r_vResultArray(19, iIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            ' Begin transaction
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add Section & Perils
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSectionAndPerilsSQL, sSQLName:=ACAddSectionAndPerilsName, bStoredProcedure:=ACAddSectionAndPerilsStored)

            ' Commit or Rollback trans
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRatingSectionsAndPerils Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRatingSectionsAndPerils", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPaymentTerms
    '
    ' Parameters: InsuranceFileCnt, UserID
    '
    ' Description:
    '
    ' History:
    '           Created : Rajesh Choudhary : 02 Aug 2006 - Float Balance and Pre-Payment (RC)
    ' ***************************************************************** '
    'Start - Prakash - WPR85_Paralleling
    'Added optional paramenter r_bCashDepositEnabled
    Public Function GetPaymentTerms(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPMUserID As Integer, ByRef r_bInvoiceEnabled As Boolean, ByRef r_bInstalmentsEnabled As Boolean, ByRef r_bPayNowEnabled As Boolean, Optional ByRef r_bBankGuaranteeEnabled As Boolean = False, Optional ByRef r_bCashDepositEnabled As Boolean = False) As Integer
        'End - Prakash - WPR85_Paralleling
        Dim result As Integer = 0
        Const kMethodName As String = "GetPaymentTerms"

        Dim vResultArray(,) As Object = Nothing
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserID", vValue:=CStr(v_lPMUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InvoiceEnabled", vValue:=r_bInvoiceEnabled, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InstalmentsEnabled", vValue:=r_bInstalmentsEnabled, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PaynowEnabled", vValue:=r_bPayNowEnabled, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="BankGuaranteeEnabled", vValue:=r_bBankGuaranteeEnabled, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            'Start - Prakash - WPR85_Paralleling
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CashDepositEnabled", vValue:=r_bCashDepositEnabled, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            'End - Prakash - WPR85_Paralleling
            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetPaymentTermsSQL, sSQLName:=ACGetPaymentTermsName, bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the return parameteres
            r_bInvoiceEnabled = gPMFunctions.NullToBoolean(m_oDatabase.Parameters.Item("InvoiceEnabled").Value)
            r_bInstalmentsEnabled = gPMFunctions.NullToBoolean(m_oDatabase.Parameters.Item("InstalmentsEnabled").Value)
            r_bPayNowEnabled = gPMFunctions.NullToBoolean(m_oDatabase.Parameters.Item("PaynowEnabled").Value)
            r_bBankGuaranteeEnabled = gPMFunctions.NullToBoolean(m_oDatabase.Parameters.Item("BankGuaranteeEnabled").Value)
            'Start - Prakash - WPR85_Paralleling
            r_bCashDepositEnabled = gPMFunctions.NullToBoolean(m_oDatabase.Parameters.Item("CashDepositEnabled").Value)
            'End - Prakash - WPR85_Paralleling
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInstalmentSchemeSQL, sSQLName:=ACGetInstalmentSchemeName, bStoredProcedure:=ACGetInstalmentSchemeStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToLong(vResultArray(0, 0)) <= 0 Then
                r_bInstalmentsEnabled = False
            End If

            m_oDatabase.Parameters.Clear()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetPaymentType(ByVal v_lProduct_Id As Integer, ByRef r_PaymentType(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            Dim sSQL As String = ""

            ' Add parameters

            sSQL = "SELECT Default_Payment_method " &
                   "From Product " &
                   "Where product_id = {v_lProduct_Id} And is_deleted = 0"


            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="v_lProduct_Id", vValue:=CStr(v_lProduct_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute the stored procedure

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPaymentType", bStoredProcedure:=False, vResultArray:=r_PaymentType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the return parameteres

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetPaymentMethod(ByVal m_lOriginalInsFileCnt As Integer, ByRef v_Result(,) As Object) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(m_lOriginalInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPaymentMethodSQL, sSQLName:=ACGetPaymentMethodName, bStoredProcedure:=ACGetPaymentMethodStored, vResultArray:=v_Result, lNumberRecords:=gPMConstants.PMAllRecords)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Return result
    End Function

    'Add task
    Public Function CreateWorkTask(ByVal v_sTaskCode As String, ByVal v_sDescription As String, ByRef r_vKeyArray(,) As Object, Optional ByVal v_lUserGroupID As Integer = 0, Optional ByVal v_lUserID As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim oRoadmap As bSIRRoadmap.Business
        Dim lPartyCnt As Integer
        Dim iTaskDaysDue As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of bSIRRoadmap

            oRoadmap = New bSIRRoadmap.Business
            m_lReturn = oRoadmap.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Default to no party
            lPartyCnt = 0

            'Default TaskDaysDue to 7
            iTaskDaysDue = 7

            ' Get the party count & TaskDaysDue
            If Informations.IsArray(r_vKeyArray) Then
                For iLoop1 As Integer = 0 To r_vKeyArray.GetUpperBound(1)

                    If CStr(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)) = "party_cnt" Then

                        lPartyCnt = CInt(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    ElseIf (CStr(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)) = "TaskDaysDue") Then

                        iTaskDaysDue = CInt(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))

                        'Default TaskDueDate Key to 7 for future steps

                        r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1) = "7"
                    End If
                Next iLoop1


            End If

            ' Remove any empty keys from the task
            m_lReturn = CType(RemoveBlankKeys(r_vKeyArray:=r_vKeyArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call Roadmap to create the task - Default to 7 days?

            m_lReturn = oRoadmap.CreateWorkManagerTask(v_lPartyCnt:=lPartyCnt, v_sDescription:=v_sDescription, v_sTask:=v_sTaskCode, v_lNumDays:=iTaskDaysDue, v_vKeyArray:=r_vKeyArray, v_lUserGroupID:=v_lUserGroupID, v_lUserID:=v_lUserID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear up

            oRoadmap.Dispose()

            oRoadmap = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWorkTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWorkTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Deepak

    Public Function RemoveBlankKeys(ByRef r_vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vNewArray(,) As Object
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do we have something to do?
            If Not Informations.IsArray(r_vKeyArray) Then
                Return result
            End If

            vNewArray = Nothing

            ' Loop the array
            For iLoop1 As Integer = 0 To r_vKeyArray.GetUpperBound(1)

                If CStr(r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) <> "" Then

                    ' Prep the array
                    If Informations.IsArray(vNewArray) Then

                        iIndex = vNewArray.GetUpperBound(1) + 1
                        ReDim Preserve vNewArray(1, iIndex)
                    Else
                        iIndex = 0
                        ReDim vNewArray(1, iIndex)
                    End If

                    ' Store it


                    vNewArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iIndex) = r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)


                    vNewArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)

                End If
            Next iLoop1


            r_vKeyArray = vNewArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveBlankKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveBlankKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function
    'Get the Agent Type for A Insurance File
    Public Function GetAgentType(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            Dim lReturn As gPMConstants.PMEReturnCode

            Const kMethodName As String = "GetAgentType"

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("InsuranceFileCnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAgentCodeForInsuranceFileCntSQL, sSQLName:=ACSelectAgentCodeForInsuranceFileCntName, bStoredProcedure:=False, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSelectAgentCodeForInsuranceFileCntSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetAgentType", r_lFunctionReturn:=result, excep:=ex)
        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetOpenPostingPeriods
    '
    ' Parameters: v_dtEffectiveDate, r_vOpenPostingPeriods
    '
    ' Description:
    '
    ' History:
    '           Created : Rajesh Choudhary : 15 Dec 2006 - (RC) IH-UDPP
    ' ***************************************************************** '
    Public Function GetOpenPostingPeriods(ByVal v_dtEffectiveDate As Date, ByRef r_vOpenPostingPeriods(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetOpenPostingPeriods"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()
            'Developer Guide No. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetOpenPostingPeriodsSQL, sSQLName:=ACGetOpenPostingPeriodsName, bStoredProcedure:=True, vResultArray:=r_vOpenPostingPeriods)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(r_vOpenPostingPeriods) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: UpdatePolicyPostingPeriod
    '
    ' Parameters: v_lInsuranceFileCnt, v_lPostingPeriodID
    '
    ' Description:
    '
    ' History:
    '           Created : Rajesh Choudhary : 15 Dec 2006 - (RC) IH-UDPP
    ' ***************************************************************** '
    Public Function UpdatePolicyPostingPeriod(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPostingPeriodID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePolicyPostingPeriod"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="posting_period_id", vValue:=CStr(v_lPostingPeriodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdatePolicyPostingPeriodSQL, sSQLName:=ACUpdatePolicyPostingPeriodName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetUserCanOverridePostingPeriod
    '
    ' Parameters: v_lUserID
    '
    ' Description:
    '
    ' History:
    '           Created : Rajesh Choudhary : 15 Dec 2006 - (RC) IH-UDPP
    ' ***************************************************************** '
    Public Function GetUserCanOverridePostingPeriod(ByVal v_lUserID As Integer, ByRef r_bCanOverride As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetUserCanOverridePostingPeriod"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_lUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_override", vValue:=CStr(r_bCanOverride), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserCanOverridePostingPeriodSQL, sSQLName:=ACGetUserCanOverridePostingPeriodName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the return parameteres
            r_bCanOverride = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("can_override").Value)

            m_oDatabase.Parameters.Clear()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetPolicyVersionDetails(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyVersionDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetPolicyVersionDetailsSQL, sSQLName:=kGetPolicyVersionDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetPolicyVersionDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function UpdateIFRLInkRisk(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskID As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "UpdateIFRLInkRisk"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            ' Add parameters

            If v_lInsuranceFileCnt = 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If v_lRiskID = 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=v_lRiskID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            lReturn = m_oDatabase.SQLAction(sSQL:=UpdateIFRLinkRiskSQL, sSQLName:=UpdateIFRLinkRiskName, bStoredProcedure:=UpdateIFRLinkRiskStored)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to run: " & UpdateIFRLinkRiskSQL)
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    Public Function CheckInstallmentSchemesforMTA(ByRef r_bSchemesExists As Boolean) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "CheckInstallmentSchemesforMTA"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResults(,) As Object = Nothing
        Dim dCurrentDate As Date
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            dCurrentDate = DateTime.Now
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            'Developer Guide No. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Date", vValue:=dCurrentDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ProductFamily", vValue:="MTA", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="has_client_fees", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PFRFValidDates_Sel_All", sSQLName:="spu_PFRFValidDates_Sel_All", bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "spu_PFRFValidDates_Sel_All" & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If Informations.IsArray(vResults) Then
                r_bSchemesExists = True
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetMTAPaymentTerms
    '
    ' Parameters: InsuranceFileCnt,InsuranceFolderCnt
    '
    ' Description:
    '
    ' History:
    '           Created : Samrendu Bhushan : 29 Jan 2008
    ' ***************************************************************** '
    Public Function GetMTAPaymentTerms(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByRef r_bInstalmentsEnabled As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetMTAPaymentTerms"
        Dim dCurrentDate As Date
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            dCurrentDate = DateTime.Now

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            'Developer Guide No. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Date", vValue:=dCurrentDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFolderCnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InstalmentsEnabled", vValue:=CStr(r_bInstalmentsEnabled), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetMTAPaymentTermsSQL, sSQLName:=ACGetMTAPaymentTermsName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the return parameteres
            r_bInstalmentsEnabled = gPMFunctions.NullToBoolean(m_oDatabase.Parameters.Item("InstalmentsEnabled").Value)

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetCurRiskIdtForOriginalRiskId
    '
    ' Parameters: OriginalRiskId
    '
    ' Description: Get current risk_cnt for the original_risk_cnt
    '
    ' History: 22 Jul 2008 Gautam Poddar - Created.
    ' ***************************************************************** '
    Public Function GetCurRiskIdtForOriginalRiskId(ByVal v_lOriginalRiskId As Integer, ByRef r_vCurRiskId(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCurRiskIdtForOriginalRiskId"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            m_lReturn = m_oDatabase.Parameters.Add(sName:="original_risk_cnt", vValue:=CStr(v_lOriginalRiskId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCurRiskNoSQL, sSQLName:=ACGetCurRiskNoName, bStoredProcedure:=True, vResultArray:=r_vCurRiskId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, ACGetCurRiskNoSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO not call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetAutoRenewalFlag(ByVal v_lInsfileCnt As Integer, ByRef r_bAutoRenFlag As Boolean) As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing
        Const kMethodName As String = "GetAutoRenewalFlag"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsFileCnt", vValue:=CStr(v_lInsfileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACAutoRenFlagSQL, sSQLName:=ACAutoRenFlagName, bStoredProcedure:=ACAutoRenFlagStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetAutoRenewalFlag", "Failed to fetch Auto renewal flag", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(vArray) Then
                Return result
            End If

            r_bAutoRenFlag = gPMFunctions.ToSafeBoolean(vArray(0, 0))


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetAgentDetails(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "GetAgentDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetAgentDetailsSQL, sSQLName:=kGetAgentDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetPolicyVersionDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetMTAQuotePolicyVersions
    '
    ' Parameters: InsuranceFileCnt,InsuranceFolderCnt
    '
    ' Description:
    ' ***************************************************************** '
    Public Function GetMTAQuotePolicyVersions(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetMTAQuotePolicyVersions"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            AddInputParameter(v_sName:="insurance_folder_cnt", v_vValue:=v_lInsuranceFolderCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetMTAQuotePolicyVersionsSQL, sSQLName:=kGetMTAQuotePolicyVersionsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetMTAQuotePolicyVersionsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ''Start Saurabh Agrawal Out of Sequence MTA Bug fixing

    Public Function GetPMWrkTaskID(ByVal v_sTaskCode As String, ByRef r_vTaskId(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPMWrkTaskID"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="taskcode", vValue:=v_sTaskCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get Id for the task code", gPMConstants.PMELogLevel.PMLogError)

            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPMwrkTaskIdSQL, sSQLName:=ACGetPMwrkTaskId, bStoredProcedure:=ACGetPMwrkTaskIdStored, vResultArray:=r_vTaskId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetPMwrkTaskIdSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    ''End Saurabh Agrawal Out of Sequence MTA Bug fixing

    ' ***************************************************************** '
    ' Name: GetPremiumDetailsForAllPolicyVersions
    '
    ' Parameters: InsuranceFileCnt
    '
    ' Description: Get premium details for all live policy versions upto
    '              passed insurance file count
    '
    ' History:
    '           Created : Gautam Poddar : Date : 27 Apr 2009
    ' ***************************************************************** '
    Public Function GetPremiumDetailsForAllPolicyVersions(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPremiumDetailsForAllPolicyVersions"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ninsurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetPremiumDetailsForAllPolicyVersionsSQL, sSQLName:=kGetPremiumDetailsForAllPolicyVersionsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetPremiumDetailsForAllPolicyVersionsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function IsSubsequentRiskVersionsEdited(ByVal v_lRiskID As Integer, ByVal v_dtMTAEffectiveDate As Date) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "IsSubsequentRiskVersionsEdited"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResultArray(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = m_oDatabase.Parameters.Add(sName:="riskid", vValue:=CStr(v_lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'Developer Guide No. 40
            lReturn = m_oDatabase.Parameters.Add(sName:="mta_effective_date", vValue:=v_dtMTAEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)


            ' Execute the stored procedure
            lReturn = m_oDatabase.SQLSelect(sSQL:="spu_Is_Subsequent_Risk_Versions_Edited", sSQLName:="Is Subsequent Risk Versions Edited", bStoredProcedure:=True, vResultArray:=vResultArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If Informations.IsArray(vResultArray) Then

                If CDbl(vResultArray(0, 0)) = 0 Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Public Function ProcessPolicyReceiptMediaTypeStatus(ByVal v_lInsuranceFileId As Integer, ByRef r_bProceed As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPolicyReceiptMediaTypeStatus"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="IsValid", vValue:=CStr(r_bProceed), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter name: IsValid", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(v_lInsuranceFileId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter name: Insurance_File_Cnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Execute SQL Statement
            lReturn = m_oDatabase.SQLAction(sSQL:=kCheckPolicyReceiptMediaTypeStatusSQL, sSQLName:=kCheckPolicyReceiptMediaTypeStatusName, bStoredProcedure:=kCheckPolicyReceiptMediaTypeStatusStored)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to execute the stored procedure: " & kCheckPolicyReceiptMediaTypeStatusSQL, gPMConstants.PMELogLevel.PMLogError)
            End If

            r_bProceed = gPMFunctions.ToSafeBoolean(m_oDatabase.Parameters.Item("IsValid").Value)


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    'End - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Public Function ValidateCertificateYear(ByRef bIsValid As Boolean, ByVal lNewInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim vValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'm_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTSubAgentCertificateYears, 1, vValue)
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTSubAgentCertificateYears, v_vBranch:=1, r_vUnderwriting:=vValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'bPMFunc.LogError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product option " & gPMConstants.SIRHiddenOptions.SIROPTHoldCoverExpiryDate, vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product option " & gPMConstants.SIRHiddenOptions.SIROPTHoldCoverExpiryDate, vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            If vValue = "1" Then
                m_lReturn = GetAndValidateSubAgentDetailsViaInsFile(bIsValid, lNewInsuranceFileCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                If bIsValid = False Then
                    gPMFunctions.RaiseError("ValidateCertificateYear", "You Cannot Make This Transaction Live- Please check the Certificate Year Configuration of Sub Agent", gPMConstants.PMELogLevel.PMLogError)
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                bIsValid = True
            End If
            Return result
        Catch ex As Exception
            bPMFunc.LogError(m_sUsername, v_sClass:=ACClass, v_sMethod:="ValidateCertificateYear", r_lFunctionReturn:=result, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    Public Function GetAndValidateSubAgentDetailsViaInsFile(ByRef r_bIsValid As Boolean, ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            If v_lInsuranceFileCnt <> 0 Then

                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSubAgentDetailsSQL, sSQLName:=ACGetSubAgentDetailsName, bStoredProcedure:=ACGetSubAgentDetailsStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If vResultArray IsNot Nothing AndAlso Informations.IsArray(vResultArray) Then

                    Dim iUBound As Integer = 0
                    Dim iLBound As Integer = 0
                    Dim sPartyCode As String = String.Empty

                    iLBound = vResultArray.GetLowerBound(1)
                    iUBound = vResultArray.GetUpperBound(1)

                    For iCNT As Integer = iLBound To iUBound
                        sPartyCode = CStr(vResultArray(1, iCNT))
                        m_lReturn = GetAndValidateSubAgentDetails(r_bIsValid:=r_bIsValid, v_sPartyCode:=sPartyCode, v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
                        If r_bIsValid = False Then
                            Exit For
                        End If
                    Next
                Else
                    r_bIsValid = True
                    Return result
                End If
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAndValidateSubAgentDetailsViaInsFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubAgents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetAndValidateSubAgentDetails(ByRef r_bIsValid As Boolean, Optional ByVal v_sPartyCode As String = "", Optional ByVal v_dtCoverStartDate As Date = Nothing, Optional ByVal v_dtCoverEndDate As Date = Nothing, Optional ByVal v_lInsuranceFileCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_code", vValue:=CStr(v_sPartyCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CoverStartDate", vValue:=CStr(v_dtCoverStartDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CoverEndDate", vValue:=CStr(v_dtCoverEndDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetandValidateSubAgentDetailSQL, sSQLName:=ACGetandValidateSubAgentDetailName, bStoredProcedure:=ACGetandValidateSubAgentDetailStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If vResultArray IsNot Nothing AndAlso Informations.IsArray(vResultArray) Then
                If CStr(vResultArray(0, 0)).Trim() = "VALID" Then
                    r_bIsValid = True
                Else
                    r_bIsValid = False
                End If
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAndValidateSubAgentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubAgents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function UpdateIFRLInkRisk(ByVal v_lInsuranceFileCnt As Long,
                            ByVal v_lRiskId As Long) As Long

        Dim lReturn As Long
        Const kMethodName As String = "UpdateIFRLInkRisk"

        Try

            UpdateIFRLInkRisk = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            ' Add parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                   vValue:=v_lInsuranceFileCnt,
                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt",
                                   vValue:=v_lRiskId,
                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                   iDataType:=gPMConstants.PMEDataType.PMLong)



            lReturn = m_oDatabase.SQLAction(
                    sSQL:=UpdateIFRLinkRiskSQL,
                    sSQLName:=UpdateIFRLinkRiskName,
                    bStoredProcedure:=UpdateIFRLinkRiskStored)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("m_oDatabase.SQLAction", "Unable to run: " & UpdateIFRLinkRiskSQL)
            End If


        Catch excep As System.Exception

            lReturn = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=UpdateIFRLInkRisk, excep:=excep)
            Return lReturn
        End Try
        ' DO Not Call any functions before here or the error will be lost
    End Function
    Public Function GetNoOfPoliciesOnAgent(
                    ByVal v_lLeadAgentCnt As Long,
                    ByRef r_lNoOfPolicies As Long) As Long

        Dim lReturn As Long
        Dim r_vResults(,) As Object = Nothing
        Const kMethodName As String = "GetNoOfPoliciesOnAgent"
        Try
            GetNoOfPoliciesOnAgent = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("lead_agent_cnt", v_lLeadAgentCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                GetNoOfPoliciesOnAgent = gPMConstants.PMEReturnCode.PMFalse

            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetNoOfPoliciesForSingleInsAgentSQL,
                                           sSQLName:=kGetNoOfPoliciesForSingleInsAgentName,
                                            bStoredProcedure:=True,
                                            vResultArray:=r_vResults,
                                            lNumberRecords:=gPMConstants.PMAllRecords)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, GetNoOfPoliciesOnAgent & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(r_vResults) Then
                r_lNoOfPolicies = ToSafeLong(r_vResults(0, 0))
            End If
        Catch excep As System.Exception
            lReturn = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetNoOfPoliciesOnAgent", r_lFunctionReturn:=GetNoOfPoliciesOnAgent, excep:=excep)
            Return lReturn
        End Try
    End Function

    ' WPR53
    Public Function GetRiskTypeDetails(ByVal v_lRiskTypeId As Integer, ByRef r_vArray As Integer) As Long

        Const kMethodName As String = "GetRiskTypeDetails"

        Dim lReturn As Integer
        Dim r_vResults(,) As Object = Nothing
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If Not Informations.IsNothing(v_lRiskTypeId) Then
                AddInputParameter(v_sName:="risk_type_id", v_vValue:=CInt(v_lRiskTypeId), v_iType:=gPMConstants.PMEDataType.PMInteger)
            End If

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetRiskTypeDetailsSQL, sSQLName:=kGetRiskTypeDetailsName, bStoredProcedure:=kGetRiskTypeDetailsStored, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, kGetRiskTypeDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetRiskTypeDetails, excep:=ex)
            Return result
        End Try
    End Function

    Public Function AddRisk(ByVal v_lRiskTypeId As Integer,
                            ByVal v_lInsuranceFileCnt As Integer,
                            ByVal v_lInsuranceFolderCnt As Integer,
                            ByVal v_lProductID As Integer,
                            ByRef r_lRiskFolderCnt As Integer,
                            ByRef r_lRiskCnt As Integer) As Long

        Const kMethodName As String = "AddRisk"
        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oSIRRiskScreen As New bSIRRiskScreen.Business
        Dim obGIS As Object
        Dim vRiskDetails(,) As Object = Nothing
        Dim vRiskTypeDetails(,) As Object = Nothing
        Dim sGISDataModel As String = String.Empty
        Dim lGISDataModelId As Integer
        Dim lRiskId As Long
        Dim v_lRiskScreenId As Long
        Dim v_sRiskDescription As String
        Dim lRiskNumber As Long

        Dim sQuoteRef As Object = String.Empty
        Dim sQuoteRefPassword As Object = String.Empty
        Dim sTopOIKey As Object = String.Empty
        Dim sXMLDataSetDef As Object = String.Empty
        Dim sXMLDataSet As Object = String.Empty
        Dim lPolicyLinkID As Object = 0
        Dim dtResult As New DataTable
        Dim sqlCmd As New SqlCommand
        Dim sqlda As New SqlDataAdapter
        result = gPMConstants.PMEReturnCode.PMTrue
        AddRisk = gPMConstants.PMEReturnCode.PMTrue

        Try


            m_oDatabase.Parameters.Clear()

            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "ProductId", CInt(v_lProductID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CInt(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            lReturn = m_oDatabase.ExecuteDataTable(sSQL:=kGetMandatoryRiskTypeDetailsSQL, sSQLName:=kGetMandatoryRiskTypeDetailsName, bStoredProcedure:=True, oRecordset:=dtResult)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, kGetMandatoryRiskTypeDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If dtResult.Rows.Count > 0 Then
                v_lRiskTypeId = CInt(dtResult.Rows(0)(0).ToString)
                v_sRiskDescription = dtResult.Rows(0)(1).ToString
                v_lRiskScreenId = CInt(dtResult.Rows(0)(2).ToString)

                ' ***************************************************************** '
                '             BEGIN: bSIRRiskScreen.Business Object Code
                ' ***************************************************************** '
                ' Create Business Object

                oSIRRiskScreen = New bSIRRiskScreen.Business
                lReturn = oSIRRiskScreen.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to get instance of bSIRRiskScreen.Business", gPMConstants.PMELogLevel.PMLogError)
                End If

                obGIS = Nothing
                result = gPMComponentServices.CreateBusinessObject(r_oObject:=obGIS, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim r_sMessage As String = "Failed to create an instance of bGIS.Application"
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bGIS.Application", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                'lReturn = obGIS.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(m_sCallingAppName), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

                'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    RaiseError(kMethodName, "Failed to get instance of bSIRRiskScreen.Business", gPMConstants.PMELogLevel.PMLogError)
                'End If


                ' Set the process modes for the busines object.
                lReturn = oSIRRiskScreen.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:="AOL", vEffectiveDate:=Date.Now)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to SetProcessModes", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Set the Business object keys
                oSIRRiskScreen.InsuranceFolderCnt = v_lInsuranceFolderCnt
                oSIRRiskScreen.InsuranceFileCnt = v_lInsuranceFileCnt
                oSIRRiskScreen.RiskId = r_lRiskCnt
                oSIRRiskScreen.RiskTypeId = v_lRiskTypeId
                oSIRRiskScreen.ProductId = v_lProductID
                oSIRRiskScreen.RiskFolderCnt = r_lRiskFolderCnt

                If (v_lRiskScreenId > 0) Then
                    ' Set the risk screen id
                    oSIRRiskScreen.ScreenId = v_lRiskScreenId
                    ' Get DataModel code
                    lReturn = oSIRRiskScreen.GetGISDataModel(r_lGISDataModelId:=lGISDataModelId,
                                                              r_sGISDataModel:=sGISDataModel)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to GetGISDataModel", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                lReturn = oSIRRiskScreen.GetRisk(vRiskArray:=vRiskDetails,
                                                  vRiskTypeArray:=vRiskTypeDetails)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to GetRisk", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Informations.IsArray(vRiskDetails) = True Then
                    r_lRiskCnt = vRiskDetails(0, 0)
                    r_lRiskFolderCnt = vRiskDetails(2, 0)
                Else
                    lRiskId = r_lRiskCnt
                End If

                With m_oDatabase
                    .Parameters.Clear()
                    lReturn = .Parameters.Add(
                        sName:="risk_cnt",
                        vValue:=r_lRiskCnt,
                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                        iDataType:=gPMConstants.PMEDataType.PMLong)
                    ' For Unquoted
                    lReturn = .Parameters.Add(
                        sName:="risk_status_id",
                        vValue:=4,
                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                        iDataType:=gPMConstants.PMEDataType.PMInteger)
                    lReturn = .Parameters.Add(
                        sName:="description",
                        vValue:=v_sRiskDescription,
                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                        iDataType:=gPMConstants.PMEDataType.PMString)
                    lReturn = .Parameters.Add(
                        sName:="variation_number",
                        vValue:=0,
                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                        iDataType:=gPMConstants.PMEDataType.PMInteger)
                    lReturn = .SQLAction(
                        sSQL:=kUpdateMandatoryRiskDetailsSQL,
                        sSQLName:=kUpdateMandatoryRiskDetailsName,
                        bStoredProcedure:=kUpdateMandatoryRiskDetailsStored)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to Update Risk Details", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End With

                ' Update Risk Number : Find out the next risk number for this policy
                lReturn = GetNextRiskNo(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                    r_lRiskNumber:=lRiskNumber)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Unable to retrieve Next Risk Number", gPMConstants.PMELogLevel.PMLogError)
                End If
                ' Save the risk number to the risk record
                lReturn = UpdateRiskNo(v_lRiskCnt:=r_lRiskCnt,
                                                     v_lRiskNumber:=lRiskNumber)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Unable to update the Next Risk Number on the Risk record", gPMConstants.PMELogLevel.PMLogError)
                End If

                lReturn = obGIS.NewRiskDataset(v_sGisDataModelCode:=ToSafeString(sGISDataModel),
                                     r_lPolicyLinkID:=lPolicyLinkID,
                                     r_sTopOIKey:=sTopOIKey,
                                     r_sXMLDataSetDef:=sXMLDataSetDef,
                                     r_sXMLDataset:=sXMLDataSet,
                                     v_lInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt),
                                     r_sQuoteRef:=sQuoteRef,
                                     r_sQuoteRefPassword:=sQuoteRefPassword,
                                     v_lRiskID:=ToSafeInteger(r_lRiskCnt))

                lReturn = obGIS.SaveToDB(v_sGisDataModelCode:=ToSafeString(sGISDataModel), r_sXMLDataset:=sXMLDataSet)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "obGIS.SaveToDB" & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                Dim vSelectedRisk(1, 0) As Object
                vSelectedRisk(0, 0) = r_lRiskCnt
                vSelectedRisk(1, 0) = True

                lReturn = UpdatePolicyLink(v_lPolicyLinkID:=lPolicyLinkID, v_lInsuranceFileCnt:=v_lInsuranceFolderCnt)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "UpdatePolicyLink" & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                lReturn = UpdateMandatoryRisk(v_lRiskId:=r_lRiskCnt)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "UpdateMandatoryRisk" & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                lReturn = UpdateRiskSelectionStatus(v_vSelectionArray:=vSelectedRisk)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "UpdateRiskSelectionStatus" & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                ' ***************************************************************** '
                '                END: bGIS.Application Object Code
                ' ***************************************************************** '
            End If

            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=AddRisk, excep:=ex)
            Return result
        Finally
            If Not obGIS Is Nothing Then
                obGIS.Dispose()
                obGIS = Nothing
            End If
            If Not oSIRRiskScreen Is Nothing Then
                oSIRRiskScreen.Dispose()
                oSIRRiskScreen = Nothing
            End If

        End Try

    End Function

    Private Function UpdatePolicyLink(
        ByVal v_lPolicyLinkID As Long,
        ByVal v_lInsuranceFileCnt As Long) As Long
        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePolicyLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the paramaters
        m_oDatabase.Parameters.Clear()

        ' Add the new parameters
        m_lReturn = m_oDatabase.Parameters.Add(
                        sName:="insurance_file_cnt",
                        vValue:=v_lInsuranceFileCnt,
                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                        iDataType:=gPMConstants.PMEDataType.PMLong)
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            UpdatePolicyLink = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If
        m_lReturn = m_oDatabase.Parameters.Add(
                        sName:="gis_policy_link_id",
                        vValue:=v_lPolicyLinkID,
                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                        iDataType:=gPMConstants.PMEDataType.PMLong)
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            UpdatePolicyLink = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If
        ' Call the SQL
        m_lReturn = m_oDatabase.SQLAction(
                        sSQL:=kUpdateGISPolicyLinkSQL,
                        sSQLName:=kUpdateGISPolicyLinkName,
                        bStoredProcedure:=kUpdateGISPolicyLinkStored)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, kUpdateGISPolicyLinkSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        Return result

    End Function

    Public Function UpdateMandatoryRisk(
        ByVal v_lRiskId As Long) As Long

        Const kMethodName As String = "UpdateMandatoryRisk"
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Clear the paramaters
            m_oDatabase.Parameters.Clear()

            ' Add the new parameters
            m_lReturn = m_oDatabase.Parameters.Add(
                            sName:="riskId",
                            vValue:=v_lRiskId,
                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                            iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            ' Call the SQL
            m_lReturn = m_oDatabase.SQLAction(
                            sSQL:=kUpdateMandatoryRiskSQL,
                            sSQLName:=kUpdateMandatoryRiskName,
                            bStoredProcedure:=kUpdateMandatoryRiskStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, kUpdateMandatoryRiskSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=UpdateMandatoryRisk, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            Return result
        End Try
    End Function
    ''' <summary>
    ''' UnquoteMandatoryRisk
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lRiskId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UnquoteMandatoryRisk(ByVal v_lInsuranceFileCnt As Long,
                                         ByVal v_lRiskId As Long) As Long

        Const kMethodName As String = "UnquoteMandatoryRisk"
        Dim sRiskDescription As String
        Dim nVariationNumber As Integer
        Dim nRiskCnt As Integer = 0
        Dim nResult As Integer
        Dim nReturn As Integer = 0
        Dim dtResult As New DataTable
        Dim sRiskLinkStatus As String = String.Empty
        Dim nNewRiskCnt As Integer = 0
        Try

            UnquoteMandatoryRisk = gPMConstants.PMEReturnCode.PMTrue
            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CInt(v_lRiskId),
                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                             gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CInt(v_lInsuranceFileCnt),
                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                             gPMConstants.PMEDataType.PMInteger)


            nReturn = m_oDatabase.ExecuteDataTable(sSQL:=kGetMandatoryRiskSQL, sSQLName:=kGetMandatoryRiskName,
                                                   bStoredProcedure:=True, oRecordset:=dtResult)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, kGetMandatoryRiskSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If dtResult.Rows.Count > 0 Then
                nRiskCnt = CInt(dtResult.Rows(0)(0).ToString)
                sRiskDescription = CStr(dtResult.Rows(0)(1).ToString)
                nVariationNumber = CInt(dtResult.Rows(0)(2).ToString())
                sRiskLinkStatus = dtResult.Rows(0)(3).ToString() 'r_vResults(3, 0)
                If sRiskLinkStatus = "U" OrElse sRiskLinkStatus = "R" Then
                    m_lReturn = CopyRisksMTAEx(v_lInsuranceFileCnt, False, v_lOnlyRiskCnt:=nRiskCnt, r_lLastNewRiskCnt:=nNewRiskCnt)
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        UnquoteMandatoryRisk = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                    nRiskCnt = nNewRiskCnt
                End If
                ' Clear the paramaters
                m_oDatabase.Parameters.Clear()
                ' Add the new parameters
                m_lReturn = m_oDatabase.Parameters.Add(
                    sName:="risk_cnt",
                    vValue:=nRiskCnt,
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMInteger)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    UnquoteMandatoryRisk = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                ' Add the new parameters
                m_lReturn = m_oDatabase.Parameters.Add(
                    sName:="risk_status_id",
                    vValue:=4,
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMInteger)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    UnquoteMandatoryRisk = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                ' Add the new parameters
                m_lReturn = m_oDatabase.Parameters.Add(
                    sName:="description",
                    vValue:=sRiskDescription,
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMString)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    UnquoteMandatoryRisk = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                ' Add the new parameters
                m_lReturn = m_oDatabase.Parameters.Add(
                    sName:="variation_number",
                    vValue:=nVariationNumber,
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMInteger)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    UnquoteMandatoryRisk = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                ' Call the SQL
                m_lReturn = m_oDatabase.SQLAction(
                    sSQL:=kUpdateMandatoryRiskDetailsSQL,
                    sSQLName:=kUpdateMandatoryRiskDetailsName,
                    bStoredProcedure:=kUpdateMandatoryRiskDetailsStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, kUpdateMandatoryRiskDetailsSQL & " Failed",
                               gPMConstants.PMELogLevel.PMLogError)
                End If

            End If
            Return nResult

        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=UnquoteMandatoryRisk, excep:=ex)
            Return nResult
        End Try
    End Function

    Public Function CheckClaimOnRisk(ByVal v_lRiskId As Long,
                                 ByRef v_bRiskHasClaim As Boolean) As Long

        Const kMethodName As String = "kCheckClaimOnRisk"
        Dim r_vResults(,) As Object = Nothing
        Dim result As Integer = 0

        Try

            CheckClaimOnRisk = gPMConstants.PMEReturnCode.PMTrue
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the paramaters
            m_oDatabase.Parameters.Clear()

            ' Add the new parameters
            m_lReturn = m_oDatabase.Parameters.Add(
                            sName:="risk_cnt",
                            vValue:=v_lRiskId,
                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                            iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CheckClaimOnRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Execute selection Query
            m_lReturn = m_oDatabase.SQLSelect(
                                    sSQL:=kCheckClaimOnRiskSQL,
                                    sSQLName:=kCheckClaimOnRiskName,
                                    bStoredProcedure:=kCheckClaimOnRiskStored,
                                    vResultArray:=r_vResults,
                                    lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "CheckClaimOnRisk Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(r_vResults) Then
                If ToSafeInteger(r_vResults(0, 0)) > 0 Then
                    v_bRiskHasClaim = True
                End If
            End If

            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CheckClaimOnRisk, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            Return result
        End Try

    End Function
    Public Function GetTransNBAccountId(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            Dim lReturn As gPMConstants.PMEReturnCode

            Const kMethodName As String = "GetTransNBAccountId"

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("InsuranceFolderCnt", CStr(v_lInsuranceFolderCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransNBAccountIdForInsuranceFileCntSQL, sSQLName:=ACGetTransNBAccountIdForInsuranceFileCntName, bStoredProcedure:=False, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetTransNBAccountIdForInsuranceFileCntSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetTransNBAccountId", r_lFunctionReturn:=result, excep:=ex)
        Finally



        End Try
        Return result
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lRiskId"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <param name="lFac_risk_cnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessRiskReinsurance(
          ByVal v_lInsuranceFileCnt As Long,
          ByVal v_lRiskId As Long,
          Optional v_sTransactionType As String = Nothing,
          Optional lFac_risk_cnt As Long = 0) As Long


        ProcessRiskReinsurance = PMEReturnCode.PMTrue

        Dim bApplyReinsurance As Object = False

        m_lReturn = m_oSIRReinsurance.SetProcessModes(
                     vTask:=PMEComponentAction.PMAdd,
                     vTransactionType:=ToSafeString(v_sTransactionType))

        With m_oSIRReinsurance
            .InsuranceFileCnt = v_lInsuranceFileCnt
            .RiskId = v_lRiskId
            If m_bIsRi2007Enabled Then
                .CopyFACRiskCnt = lFac_risk_cnt
            End If
        End With

        m_lReturn = m_oSIRReinsurance.ApplyReinsurance(bApplyReinsurance)
        If m_lReturn <> PMEReturnCode.PMTrue Then
            '  bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="ProcessRiskReinsurance", r_lFunctionReturn:=result)
            gPMFunctions.RaiseError("ProcessRiskReinsurance", "ProcessRiskReinsurance Failed", gPMConstants.PMELogLevel.PMLogError)
            Return m_lReturn = PMEReturnCode.PMFalse
        End If

        If (bApplyReinsurance = False) Then

            If (v_lRiskId <> 0) Then
                'Set the status to quoted
                m_lReturn = m_oSIRReinsurance.ChangeRiskStatus

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("ProcessRiskReinsurance", "m_oReinsurance.ChangeRiskStatus Failed", gPMConstants.PMELogLevel.PMLogError)
                    Return m_lReturn = PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            Exit Function
        End If

        'Generate reinsurance details for risk
        m_lReturn = m_oSIRReinsurance.CalculateRI
        If m_lReturn <> PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("ProcessRiskReinsurance", "m_oReinsurance.CalculateRI Failed", gPMConstants.PMELogLevel.PMLogError)
            Return m_lReturn = PMEReturnCode.PMFalse
            Exit Function
        End If

        'Update the risk status to quoted
        m_lReturn = m_oSIRReinsurance.ChangeRiskStatus
        If m_lReturn <> PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("ProcessRiskReinsurance", "m_oReinsurance.ChangeRiskStatus Failed", gPMConstants.PMELogLevel.PMLogError)
            Return m_lReturn = PMEReturnCode.PMFalse
            Exit Function
        End If



        Return m_lReturn
    End Function

    ''' <summary>
    ''' Unquote Risks Forward
    ''' </summary>
    ''' <param name="nRiskCnt"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function UnquoteRisksForward(
                    ByVal nRiskCnt As Integer) As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the paramaters
            m_oDatabase.Parameters.Clear()

            ' Add the new parameters
            m_lReturn = m_oDatabase.Parameters.Add(
                            sName:="nRiskcnt",
                            vValue:=nRiskCnt,
                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                            iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UnquoteRisksForward Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnquoteRisksForward")
                Return nResult
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLAction(
                            sSQL:=kUnquoteRisksForwardSQL,
                            sSQLName:=kUnquoteRisksForwardName,
                            bStoredProcedure:=kUnquoteRisksForwardStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UnquoteRisksForward Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnquoteRisksForward")
                Return nResult
            End If
            Return nResult
        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnquoteRisksForward Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnquoteRisksForward", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)
            Return nResult
        End Try
    End Function

    Public Function GetUserGroupId(ByVal sUserGroup As String, ByRef o_nUserGroupId As Integer) As Integer

        Dim nResult As Integer = 0
        Dim oaResults(,) As Object = Nothing
        nResult = gPMConstants.PMEReturnCode.PMTrue
        Try
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="sCode", vValue:=sUserGroup, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetUserGroupIdSQL, sSQLName:=kGetUserGroupIdName, bStoredProcedure:=True, vResultArray:=oaResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (oaResults IsNot Nothing) Then
                o_nUserGroupId = oaResults(0, 0)
            End If

        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserGroupId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroupId", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return nResult
        End Try
        Return nResult
    End Function
    ''' <summary>
    ''' GetPolicyRisksForNoChange
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="v_nRiskId"></param>
    ''' <param name="r_oResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPolicyRisksForNoChange(ByVal nInsuranceFileCnt As Integer, ByVal v_nRiskId As Integer,
                                               ByRef r_oResults(,) As Object) As Integer

        Const kMethodName As String = " GetPolicyRisksForNoChange"
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Try
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            Call AddInputParameter(v_sName:="Insurance_File_Cnt",
                                   v_vValue:=nInsuranceFileCnt,
                                   v_iType:=PMEDataType.PMInteger)

            Call AddInputParameter(v_sName:="risk_Cnt",
                                   v_vValue:=v_nRiskId,
                                   v_iType:=PMEDataType.PMInteger)


            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect(sSQL:=kGetPolicyRisksForNoChangeSQL,
                                            sSQLName:=kGetPolicyRisksForNoChangeName,
                                            bStoredProcedure:=kGetPolicyRisksForNoChangeStored,
                                            vResultArray:=r_oResults,
                                            lNumberRecords:=gPMConstants.PMAllRecords)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + kGetPolicyRisksForNoChangeSQL + " Failed")
            End If
            Return nReturn
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        End Try
    End Function
    ''' <summary>
    ''' GetUserAuthorityDisplayReinsurance
    ''' </summary>
    ''' <param name="v_nUserID"></param>
    ''' <param name="r_bDisplayReinsurance"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserAuthorityDisplayReinsurance(ByVal v_nUserID As Integer, ByRef r_bDisplayReinsurance As Boolean) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "GetUserAuthorityDisplayReinsurance"
        Dim oResultArray(,) As Object = Nothing

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_nUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            nResult = m_oDatabase.SQLSelect(sSQL:=kGetUserAuthorityDisplayReinsuranceSQL, sSQLName:=kGetUserAuthorityDisplayReinsuranceName, bStoredProcedure:=kGetUserAuthorityDisplayReinsuranceStored, vResultArray:=oResultArray)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oResultArray IsNot Nothing AndAlso Informations.IsArray(oResultArray) Then
                r_bDisplayReinsurance = ToSafeBoolean(oResultArray(65, 0))

            End If
            Return nResult

        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

        End Try
        Return nResult
    End Function
    Public Function GetAttachedInstalmentPlans(ByVal v_nInsurance_FileKey As Integer, ByRef r_oActivePlan As Object) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="Insurance_file_cnt", vValue:=v_nInsurance_FileKey, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyLivePlansSQL, sSQLName:=ACGetPolicyLivePlansName, bStoredProcedure:=ACGetPolicyLivePlansStored, vResultArray:=r_oActivePlan)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAttachedInstalmentPlans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAttachedInstalmentPlans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return m_lReturn
        Finally

        End Try


    End Function

    Public Function GetTotalPremiumAmountForALLPolicyVersions(ByVal sInsuranceRef As String, ByVal nInsuranceCnt As Integer, ByRef dTotalPremium As Decimal, Optional ByRef dTotalTaxNotAppliedToClient As Decimal = 0.00) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim kMethodName As String = "GetTotalPremiumAmountForALLPolicyVersions"
        Dim aoResultArray As Object(,) = Nothing
        Try
            m_oDatabase.Parameters.Clear()
            m_oDatabase.Parameters.Add(sName:="insurance_ref", vValue:=sInsuranceRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_oDatabase.Parameters.Add(sName:="nFileCnt", vValue:=nInsuranceCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            nResult = m_oDatabase.SQLSelect(sSQL:=ACGetTotalPremiumForAllVersionsSQL, sSQLName:=ACGetTotalPremiumForAllVersionsName, bStoredProcedure:=ACGetTotalPremiumForAllVersionsStored, vResultArray:=aoResultArray)

            If nResult = PMEReturnCode.PMTrue AndAlso aoResultArray IsNot Nothing AndAlso aoResultArray.Length > 0 Then
                dTotalPremium = Convert.ToDecimal(aoResultArray(0, 0))
                dTotalTaxNotAppliedToClient = Convert.ToDecimal(aoResultArray(2,0))
            End If

            Return nResult

        Catch ex As System.Exception
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTotalPremiumAmountForALLPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=ex)
            Return m_lReturn
        Finally

        End Try


    End Function


    ''' <summary>
    ''' Gets the information necessary from all risks of this policy for auto quote risks functionality
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="r_oResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPolicyRisksForAutoQuote(ByVal nInsuranceFileCnt As Integer,
                                               ByRef r_oResults(,) As Object) As Integer

        Const kMethodName As String = "GetPolicyRisksForAutoQuote"
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Try
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            Call AddInputParameter(v_sName:="Insurance_File_Cnt",
                                   v_vValue:=nInsuranceFileCnt,
                                   v_iType:=PMEDataType.PMInteger)

            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect(sSQL:=kGetPolicyRisksForAutoQuoteSQL,
                                            sSQLName:=kGetPolicyRisksForAutoQuoteName,
                                            bStoredProcedure:=kGetPolicyRisksForAutoQuoteStored,
                                            vResultArray:=r_oResults,
                                            lNumberRecords:=gPMConstants.PMAllRecords)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kMethodName + " : " + kGetPolicyRisksForAutoQuoteSQL + " Failed")
            End If
            Return nReturn
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        End Try
    End Function

    ''' <summary>
    ''' DeletePFPremiumFinance
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function DeletePFPremiumFinance(ByVal nInsuranceFileCnt As Integer) As Integer
        Const kMethodName As String = "DeletePFPremiumFinance"
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMFalse

        Try
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            nReturn = AddInputParameter(v_sName:="nInsurance_file_cnt", v_vValue:=nInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            nReturn = m_oDatabase.SQLAction(sSQL:=kDeletePFPremiumFinanceSQL, sSQLName:=kDeletePFPremiumFinanceName, bStoredProcedure:=kDeletePFPremiumFinanceStored)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, kDeletePFPremiumFinanceSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch excep As Exception
            ' DO Not Call any functions before here or the error will be lost
            nReturn = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeletePFPremiumFinance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nReturn
        End Try
        Return nReturn
    End Function

    'Start - (Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enhancement)
    ' ***************************************************************** '
    ' Name: GetPolicyRisksForAutoQuote
    '
    ' Parameters: InsuranceFileCnt
    '
    ' Description: Gets the information necessary from all risks of this
    '               policy for auto quote risks functionality
    ' ***************************************************************** '
    Public Function GetPolicyRisksForAutoQuote(
                                            ByVal v_lInsuranceFileCnt As Long,
                                            ByRef r_vResults As Object) As Long
        Const kMethodName As String = "GetPolicyRisksForAutoQuote"
        Dim lReturn As Long

        Try
            GetPolicyRisksForAutoQuote = gPMConstants.PMEReturnCode.PMTrue
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()
            ' Add Required Stored Procedure Parameters
            Call AddInputParameter(v_sName:="Insurance_File_Cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(
                                    sSQL:=ACGetPolicyRisksForAutoQuoteSQL,
                                    sSQLName:=ACGetPolicyRisksForAutoQuoteName,
                                    bStoredProcedure:=True,
                                    vResultArray:=r_vResults,
                                    lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, ACGetPolicyRisksForAutoQuoteSQL, gPMConstants.PMEReturnCode.PMError)
            End If
        Catch
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:=kMethodName,
                r_lFunctionReturn:=GetPolicyRisksForAutoQuote)
        End Try
    End Function
    'End - (Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enhancement)

    Public Function ProcessAgentCommission(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim bCommissionRequired As Boolean
            Dim vAgentCommission As Object
            Dim oSirAgentCommission As BSirAgentCommission.Business
            'Get the Object for BSirAgentCommission
            oSirAgentCommission = New BSirAgentCommission.Business
            m_lReturn = oSirAgentCommission.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = oSirAgentCommission.SetProcessModes(vTask:=PMEComponentAction.PMEdit, vNavigate:=0, vProcessMode:=110, vTransactionType:=ToSafeString(m_sTransactionType), vEffectiveDate:=DateTime.Now)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            oSirAgentCommission.InsuranceFileCnt = v_lInsuranceFileCnt

            'Do we require agent commission

            m_lReturn = oSirAgentCommission.CheckDisplayCommission(r_bDisplayScreen:=bCommissionRequired)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not bCommissionRequired Then
                'No processing required
                Return result
            End If

            'Calculate agent commission

            m_lReturn = oSirAgentCommission.CalculateAgentCommission(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sTransactionType:=m_sTransactionType, r_vntResult:=vAgentCommission)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Calculate lead commission

            m_lReturn = oSirAgentCommission.UpdateLeadCommission(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            Return result

        End Try
    End Function

End Class
