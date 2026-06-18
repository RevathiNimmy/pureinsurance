Option Strict Off
Option Explicit On
Imports System.Text
Imports SSP.Shared
Imports System.Linq
Imports System.Collections
'developer guide no 129. 
'Start
'End
<System.Runtime.InteropServices.ProgId("bBOLink_NET.bBOLink")> _
Public NotInheritable Class bBOLink
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name:   bBOLink
    ' Description:  Creatable bBOLink class which contains all the
    '               methods, business rules required for the
    '               Back Office Link.
    ' Date:         20/06/2000
    ' Author:       SK
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 11/12/2003
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
    Private Const ACClass As String = "bBOLInk"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    'Private oComponentServices As PMServerBusinessCS

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lError As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Insurance File ID
    Private m_lInsuranceFileCnt As Integer
    ' Insurance File ID
    Private m_lPartyCnt As Integer

    ' Insurance Folder ID
    Private m_lInsuranceFolderCnt As Integer

    'Link to Gemini
    Private m_oVehicle As Object
    'Private m_oVehicle As bSirToGemVehicle.Business

    ' Sirius Product Name
    Private m_sSPName As String = ""

    Private m_bGeminiLink As Boolean
    Private m_bGeminiIILink As Boolean
    Private m_bSwiftLink As Boolean
    'SJ 23/02/2004 - start
    Private m_bUnderwritingBranchEnabled As Boolean
    Private m_bIsUnderwritingBranch As Boolean
    'SJ 23/02/2004 - end

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property


    Public Property InsuranceFileCnt() As Integer
        Get

            Return m_lInsuranceFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = Value

        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property
    ' ***************************************************************** '
    ' Name:         Sirius_Product (Public Property - Read Only)
    ' Description:  contains the informaton regarding the Sirius
    '               Product callin gthe BOLink object
    ' Date:         18/07/2000
    ' Author:       SK
    ' ***************************************************************** '
    Public ReadOnly Property Sirius_Product() As String
        Get

            Return m_sSPName

        End Get
    End Property

    ' ***************************************************************** '
    ' Name:         Terminate (Standard Method)
    ' Description:  Entry point for any termination code for this
    '               object.
    ' Date:         20/06/2000
    ' Author:       SK
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name:         SetProcessModes (Standard Method)
    ' Description:  Set the optional process modes.
    ' Date:         20/06/2000
    ' Author:       SK
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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name:         GetSiriusProd  (Private)
    ' Description:  Gets the client details for the given policy from S for B
    ' SP:           spu_get_sirius_prod
    ' Date:         20/06/2000
    ' Author:       SK
    ' ***************************************************************** '
    Private Function GetSiriusProd(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0




        Dim sContactType As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        'Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()


        'Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSiriusProdSQL, sSQLName:=ACGetSiriusProdName, bStoredProcedure:=ACGetSiriusProdStored, vResultArray:=r_vResultArray)


        ' If NO record was found, return Not Found
        If Not Informations.IsArray(r_vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If


        Return result

    End Function

    '*****************************************************************
    ' Name: GetUWPolicyList
    '
    ' Desc : get all policies which are either Live Policy,Policy under Renewal,
    '            Permanent MTA or Temporary MTA
    ' Hist  : 04/12/2000 Created - Tinny
    '       : 04/12/2001    JMK - cDate the date parameters
    '
    ' Note : Do not use GetPolicyList() to get underwriting policies
    '
    '*****************************************************************
    Public Function GetUWPolicyList(ByRef r_vResultArray As Object,
                                    Optional ByVal v_vPolicyNo As Object = "",
                                    Optional ByVal v_vPartyShortName As Object = "",
                                    Optional ByVal v_vPostCode As Object = "",
                                    Optional ByVal v_vPolicyStartDate As Object = "",
                                    Optional ByVal v_vPolicyEndDate As Object = "",
                                    Optional ByVal v_vClaimDate As Object = "",
                                    Optional ByVal v_bLimitResults As Boolean = False,
                                    Optional ByVal v_lCoverNoteSheetNumber As Integer = 0,
                                    Optional ByVal v_vAgentGroupCnt As Object = 0,
                                    Optional ByVal v_lNumberofRecords As Integer = -1,
                                    Optional ByVal nAgentKey As Integer = 0,
                                    Optional ByVal bRetrieveAssociated As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim lAgentGroupCnt As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If Not False Then
                lAgentGroupCnt = v_vAgentGroupCnt
            Else
                lAgentGroupCnt = 0
            End If

            m_oDatabase.Parameters.Clear()
            If m_oDatabase.Parameters.Add(sName:="PolicyNo", vValue:=If(v_vPolicyNo = "", DBNull.Value, v_vPolicyNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Parameters.Add(sName:="PartyShortName", vValue:=If(v_vPartyShortName = "", DBNull.Value, v_vPartyShortName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Parameters.Add(sName:="PostCode", vValue:=If(v_vPostCode = "", DBNull.Value, v_vPostCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim vPStart As Object
            If v_vPolicyStartDate = "" Then
                vPStart = DBNull.Value
            Else
                vPStart = CDate(v_vPolicyStartDate)
            End If

            If m_oDatabase.Parameters.Add(sName:="PolicyStartDate", vValue:=vPStart, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Dim vPEnd As Object
            If v_vPolicyEndDate = "" Then
                vPEnd = DBNull.Value
            Else
                vPEnd = CDate(v_vPolicyEndDate)
            End If

            If m_oDatabase.Parameters.Add(sName:="PolicyEndDate", vValue:=vPEnd, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Parameters.Add(sName:="GISValue", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Parameters.Add(sName:="ClaimDate", vValue:=If(gPMFunctions.ToSafeString(v_vClaimDate) = "", DBNull.Value, v_vClaimDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Parameters.Add(sName:="SourceID", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_lCoverNoteSheetNumber <> 0 Then
                m_oDatabase.Parameters.Add(sName:="CoverNoteSheetNumber", vValue:=CStr(v_lCoverNoteSheetNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_group_cnt", vValue:=CStr(lAgentGroupCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed (Agent Group Key)", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUWPolicyList")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Parameters.Add(sName:="AgentKey", vValue:=CStr(nAgentKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If m_oDatabase.Parameters.Add(sName:="RetrieveAssociates", vValue:=If(bRetrieveAssociated = True, 1, 0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If v_bLimitResults Then
                If v_lNumberofRecords = -1 Then
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLatestVersionPolicySQL, sSQLName:=ACGetLatestVersionPolicyName, bStoredProcedure:=ACGetLatestVersionPolicyStored, vResultArray:=r_vResultArray, bKeepNulls:=True)
                Else
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLatestVersionPolicySQL, sSQLName:=ACGetLatestVersionPolicyName, bStoredProcedure:=ACGetLatestVersionPolicyStored, lNumberRecords:=v_lNumberofRecords, vResultArray:=r_vResultArray, bKeepNulls:=True)
                End If
            Else
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLatestVersionPolicySQL, sSQLName:=ACGetLatestVersionPolicyName, bStoredProcedure:=ACGetLatestVersionPolicyStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray, bKeepNulls:=True)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUWPolicyList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUWPolicyList ", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function FindLikeIndex(ByRef sIndex As String, ByRef lNumberOfRecords As Integer,
                                  ByRef vResultArray(,) As Object,
                                  Optional ByRef sDataModelType As String = "RISK",
                                  Optional ByVal v_vAgentGroupCnt As Object = 0) As Integer
        Return FindLikeIndex(sIndex:=sIndex,
                      lNumberOfRecords:=lNumberOfRecords,
                      vResultArray:=vResultArray,
                      sInsuranceRef:="",
                      sDataModelType:=sDataModelType,
                      v_vAgentGroupCnt:=v_vAgentGroupCnt)
    End Function

    Public Function FindLikeIndex(ByRef sIndex As String, ByRef lNumberOfRecords As Integer,
                                  ByRef vResultArray(,) As Object,
                                  ByRef sInsuranceRef As String,
                                  Optional ByRef sDataModelType As String = "RISK",
                                  Optional ByVal v_vAgentGroupCnt As Object = 0) As Integer
        Dim result As Integer = 0
        Dim vDataModelCodeArray(,) As Object = Nothing

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            If sIndex = "*" Then
                sIndex = "%"
            End If

            If sIndex = "" Then
                sIndex = "%"
            End If

            m_lReturn = CType(GetAllDataModelCodes(vDataModelCodeArray, sDataModelType), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get DataModel Codes", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeIndex")

                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                If Not Informations.IsArray(vDataModelCodeArray) Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                m_lReturn = CType(GetAllGISSearchResults(sIndex, lNumberOfRecords, vDataModelCodeArray, vResultArray, sInsuranceRef:=sInsuranceRef, v_vAgentGroupCnt:=v_vAgentGroupCnt), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="FindLikeIndex Failed. Failed to GetAllGISSearchResults", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeIndex")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindLikeIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindLikeIndex", excep:=excep)
            Return result
        End Try
    End Function



    ' Author        : Ram Chandrabose
    ' Date          : 05-01-2000
    ' Description   : Funtion to get all Data Model Code
    Private Function GetAllDataModelCodes(ByRef vArray(,) As Object, ByRef sDataModelType As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' RVH 02/02/2004 CQ4070 - Need to limit data models being returned by type - i.e. claims etc
        If sDataModelType = "" Then
            sSQL = "SELECT Code From GIS_Data_Model"
        ElseIf sDataModelType.ToUpper() = "CASE" Then
            sSQL = "SELECT Code From GIS_Data_Model WHERE GIS_Data_Model_Type_Id in (SELECT GIS_Data_Model_Type_Id FROM GIS_Data_Model_Type WHERE Code='CASE' or Code='CLAIM')"
        Else
            sSQL = "SELECT Code From GIS_Data_Model WHERE GIS_Data_Model_Type_Id=(SELECT GIS_Data_Model_Type_Id FROM GIS_Data_Model_Type WHERE Code='" & sDataModelType.ToUpper() & "')"
        End If

        With m_oDatabase
            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetAllDataModelCodes", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        End With

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed to get all DataModel Codes", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllDataModelCodes")

            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        Return result

    End Function

    Public Function GetAllGISSearchResults(ByRef sSearchStr As String, ByRef lNoOfRecords As Integer, ByRef vDataModelsArray(,) As Object, ByRef vResultArray As Object, ByRef sInsuranceRef As String, Optional ByVal v_vAgentGroupCnt As Object = Nothing) As Integer '(Prakash C Varghese) - (Tech Spec - WRQBENCF04-Association of multiple agents)-(5.1.1)
        Dim result As Integer = 0
        Dim NoofFields As Integer
        Dim vTempData(,) As Object = Nothing
        Dim vResultData(,) As Object = Nothing
        Dim sDataModelCode As String = ""
        Dim iMaxRow, iFromRow As Integer
        Dim lAgentGroupCnt As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            If Not False Then
                lAgentGroupCnt = v_vAgentGroupCnt
            Else
                lAgentGroupCnt = 0
            End If

            For iCounter As Integer = vDataModelsArray.GetLowerBound(1) To vDataModelsArray.GetUpperBound(1) Step 1
                sDataModelCode = CStr(vDataModelsArray(0, iCounter)).Trim()

                With m_oDatabase
                    .Parameters.Clear()
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_code", vValue:=sDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="search_object_name", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="search_value", vValue:=sSearchStr, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_group_cnt", vValue:=CStr(lAgentGroupCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If sInsuranceRef <> "" Then
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_ref", vValue:=ToSafeString(sInsuranceRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    End If


                    If m_sTransactionType = "C_CP" Or m_sTransactionType = "C_CR" Then
                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGisSearchPropertyGisSQL, sSQLName:=ACGisSearchPropertyGisName, bStoredProcedure:=ACGisSearchPropertyGisStored, vResultArray:=vTempData)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_insurance_ref_reqd", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsLikeIndexGISSearchSQL, sSQLName:=ACInsLikeIndexGISSearchName, bStoredProcedure:=ACInsLikeIndexGISSearchStored, lNumberRecords:=lNoOfRecords, vResultArray:=vTempData)

                    Else
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_insurance_ref_reqd", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsLikeIndexGISSearchSQL, sSQLName:=ACInsLikeIndexGISSearchName, bStoredProcedure:=ACInsLikeIndexGISSearchStored, lNumberRecords:=lNoOfRecords, vResultArray:=vTempData)
                    End If

                    If Informations.IsArray(vTempData) Then
                        NoofFields = vTempData.GetUpperBound(0)

                        If Not Informations.IsArray(vResultData) Then
                            vResultData = vTempData
                        Else
                            ' We alreay have some data and we have to merge it with new data
                            iFromRow = vResultData.GetUpperBound(1)

                            iMaxRow = vResultData.GetUpperBound(1) + vTempData.GetUpperBound(1) + 1
                            ReDim Preserve vResultData(NoofFields, iMaxRow)

                            For iCounter1 As Integer = vTempData.GetLowerBound(1) To vTempData.GetUpperBound(1)
                                iFromRow += 1
                                For iCounter2 As Integer = 0 To NoofFields
                                    vResultData(iCounter2, iFromRow) = vTempData(iCounter2, iCounter1)
                                Next iCounter2
                            Next iCounter1
                        End If
                    End If

                End With
            Next iCounter

            vResultArray = vResultData
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllGISSearchResults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllGISSearchResults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    ''' <summary>
    ''' 'This funciton is used to  collect the details related to the RiskIndex 
    ''' </summary>
    ''' <param name="vInputData"></param>
    ''' <param name="vOutputData"></param>
    ''' <param name="v_vPolicyNo"></param>
    ''' <param name="v_vPartyShortName"></param>
    ''' <param name="v_vPostCode"></param>
    ''' <param name="v_vPolicyStartDate"></param>
    ''' <param name="v_vPolicyEndDate"></param>
    ''' <param name="v_vClaimDate"></param>
    ''' <param name="v_lCoverNoteSheetNumber"></param>
    ''' <param name="v_lNumbersOfRecords"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function GetUWPolicyByGISSearchIndex(ByRef vInputData As Object, ByRef vOutputData(,) As Object,
                                                Optional ByVal v_vPolicyNo As Object = "",
                                                Optional ByVal v_vPartyShortName As Object = "",
                                                Optional ByVal v_vPostCode As Object = "",
                                                Optional ByVal v_vPolicyStartDate As Object = "",
                                                Optional ByVal v_vPolicyEndDate As Object = "",
                                                Optional ByVal v_vClaimDate As Object = "",
                                                Optional ByVal v_lCoverNoteSheetNumber As Integer = 0,
                                                Optional ByVal v_lNumbersOfRecords As Integer = -1,
                                                Optional ByVal bRetrieveAssociates As Boolean = False) As Integer
        Dim nResult As Integer = 0
        Dim oPStart As Object
        Dim oPEnd As Object
        Dim nNoofFields, iMaxRow, iFromRow As Integer
        Dim sSQL As String = ""
        Dim oTempData As Object = Nothing
        Dim oResultData As Object = Nothing
        Dim oInsuranceFileCnt As Object
        Dim oPropertyValue As Object
        Dim vInsuranceFileCnt As Object
        Dim vPropertyValue As Object
        Dim vResultpolicyData(,) As Object = Nothing
        Dim vResultRiskData(,) As Object = Nothing

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Get all related data
            If Informations.IsArray(vInputData) Then
                ReDim vResultRiskData(0, vInputData.GetUpperBound(1))
                For iCounter As Integer = vInputData.GetLowerBound(0) To vInputData.GetUpperBound(1)
                    vResultRiskData(0, iCounter) = vInputData(4, iCounter)
                Next

                ReDim vResultpolicyData(0, vInputData.GetUpperBound(1))
                For iCounter As Integer = vInputData.GetLowerBound(0) To vInputData.GetUpperBound(1)
                    vResultpolicyData(0, iCounter) = vInputData(5, iCounter)
                Next

                Dim policyNumberData As Array = (From policyNumbers In vResultpolicyData Select policyNumbers).Distinct().ToArray()

                For iCounter As Integer = 0 To policyNumberData.Length - 1
                    ' Initialise the search Criteria Variables
                    vInsuranceFileCnt = ""
                    v_vPolicyNo = CStr(policyNumberData(iCounter)).Trim()

                    For i As Integer = 0 To vResultpolicyData.GetUpperBound(1) - 1
                        If vResultpolicyData(0, i) = v_vPolicyNo Then
                            vPropertyValue = CStr(vResultRiskData(0, i))
                            Exit For
                        End If
                    Next


                    m_oDatabase.Parameters.Clear()

                    If m_oDatabase.Parameters.Add(sName:="PolicyNo", vValue:=If(v_vPolicyNo = "", DBNull.Value, v_vPolicyNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If m_oDatabase.Parameters.Add(sName:="PartyShortName", vValue:=If(v_vPartyShortName = "", DBNull.Value, v_vPartyShortName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If m_oDatabase.Parameters.Add(sName:="PostCode", vValue:=If(v_vPostCode = "", DBNull.Value, v_vPostCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If v_vPolicyStartDate = "" Then
                        oPStart = DBNull.Value
                    Else
                        oPStart = CDate(v_vPolicyStartDate)
                    End If
                    If m_oDatabase.Parameters.Add(sName:="PolicyStartDate", vValue:=oPStart, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If v_vPolicyEndDate = "" Then
                        oPEnd = DBNull.Value
                    Else
                        oPEnd = CDate(v_vPolicyEndDate)
                    End If
                    If m_oDatabase.Parameters.Add(sName:="PolicyEndDate", vValue:=oPEnd, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=oInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If m_oDatabase.Parameters.Add(sName:="GISValue", vValue:=oPropertyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If m_oDatabase.Parameters.Add(sName:="ClaimDate", vValue:=If(Not Informations.IsDate(v_vClaimDate), DBNull.Value, v_vClaimDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If m_oDatabase.Parameters.Add(sName:="SourceID", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If v_lCoverNoteSheetNumber <> 0 Then
                        m_oDatabase.Parameters.Add(sName:="CoverNoteSheetNumber", vValue:=CStr(v_lCoverNoteSheetNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    End If
                    If m_oDatabase.Parameters.Add(sName:="RetrieveAssociates", vValue:=If(bRetrieveAssociates = True, 1, 0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_oDatabase.SQLSelect(sSQL:=ACGetLatestVersionPolicySQL, sSQLName:=ACGetLatestVersionPolicyName, bStoredProcedure:=ACGetLatestVersionPolicyStored, lNumberRecords:=v_lNumbersOfRecords, vResultArray:=oTempData, bKeepNulls:=True) <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUWPolicyByGISSearchIndex")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Informations.IsArray(oTempData) Then
                        nNoofFields = oTempData.GetUpperBound(0)

                        If Not Informations.IsArray(oResultData) Then
                            oResultData = oTempData
                        Else
                            iFromRow = oResultData.GetUpperBound(1)
                            iMaxRow = oResultData.GetUpperBound(1) + oTempData.GetUpperBound(1) + 1
                            ReDim Preserve oResultData(nNoofFields, iMaxRow)
                            For iCounter1 As Integer = oTempData.GetLowerBound(1) To oTempData.GetUpperBound(1)
                                iFromRow += 1
                                For iCounter2 As Integer = 0 To nNoofFields
                                    oResultData(iCounter2, iFromRow) = oTempData(iCounter2, iCounter1)
                                Next iCounter2
                            Next iCounter1
                        End If
                    End If
                Next iCounter
            End If

            vOutputData = oResultData

            Return nResult
        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUWPolicyByGISSearchIndex Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUWPolicyByGISSearchIndex", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetPolicyList
    ' Description:  SQL Query to Select Insurance File (Policy) details
    ' Returns    :  2-dimensional ResultArray (r_vResultArray)
    '               For BROKING(in the following order)
    '                   Policy ID, Policy Holder, Policy Number,
    '                   From Date, To Date, Postal Code
    '               For UNDERWRITING(in the following order)
    '                   Policy ID, Policy Holder, Policy Number,
    '                   Risk Index, Product Code,
    '                   From Date, To Date, Postal Code
    ' Date:         20/06/2000
    ' Author:       SK
    ' ***************************************************************** '
    'DC261001 -added exclude lapsed parameter &
    '           tidied up this mess has hard to read
    'MKW190503- restrict search to sources available to user
    'AR20051026 - S4B Claim Enhancements R&D 2005 - Filter by Client Name, return Address1 and Account Exec
    'developer Guide 71
    'Public Function GetPolicyList(ByRef r_vResultArray As Object, Optional ByVal v_vpol_no As String = "", Optional ByVal v_vpol_code As String = "", Optional ByVal v_vrisk_code As Object = Nothing, Optional ByVal v_vclm_dt As Object = Nothing, Optional ByVal v_vprty_shrt_nm As String = "", Optional ByVal v_vpost_code As String = "", Optional ByVal v_vfrm_dt As String = "", Optional ByVal v_vto_dt As String = "", Optional ByVal v_vexclude_lapsed As Byte = 0, Optional ByVal v_vValidSourceArray() As Object = Nothing, Optional ByVal v_bIncludeDeleted As Boolean = True, Optional ByVal v_vClientName As Object = Nothing) As Integer
    Public Function GetPolicyList(ByRef r_vResultArray(,) As Object, Optional ByVal v_vpol_no As Object = Nothing, Optional ByVal v_vpol_code As Object = Nothing, Optional ByVal v_vrisk_code As Object = Nothing, Optional ByVal v_vclm_dt As Object = Nothing, Optional ByVal v_vprty_shrt_nm As Object = Nothing, Optional ByVal v_vpost_code As Object = Nothing, Optional ByVal v_vfrm_dt As Object = Nothing, Optional ByVal v_vto_dt As Object = Nothing, Optional ByVal v_vexclude_lapsed As Object = Nothing, Optional ByVal v_vValidSourceArray As Object = Nothing, Optional ByVal v_bIncludeDeleted As Boolean = True, Optional ByVal v_vClientName As Object = Nothing) As Integer
        ' MKW 190503 PN2032 Added v_vValidSourceArray as parameter.
        ' S4B Claim Enhancements R&D 2005 - Add ClientName parameter.

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim iParamCount As Integer
        'developer guide no 21. 
        Dim sVbsFlag As String = ""
        'DC250501 for searching via shortname
        Dim sShortname As String = ""
        Dim sClientName As String = ""

        'MKW 190503 PN2032 START
        Dim sTemp As New StringBuilder
        Dim nLower, nUpper As Integer
        'MKW 190503 PN2032 END

        'DD 08/10/2003 - Added for filtering in a multi-company broking setup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC250501 -start -double up apostrophes for searching
            sShortname = v_vprty_shrt_nm
            m_lReturn = CType(Apostrophes(sShortname), gPMConstants.PMEReturnCode)
            'DC250501 -end

            'S4B Claim Enhancement R&D 2005

            If Not Informations.IsNothing(v_vClientName) Then
                sClientName = gPMFunctions.ToSafeString(v_vClientName).Trim()
                m_lReturn = CType(Apostrophes(sClientName), gPMConstants.PMEReturnCode)
            End If

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered
            sSQL = ""
            'SJ 23/02/2004 - start
            'sSql = sSql & "SELECT Insurance_File.insurance_file_cnt, Party.name, Insurance_File.insurance_ref," & vbCrLf
            'SJ 23/02/2004 - end


            'SJ 23/02/2004 - start
            sSQL = sSQL & "SELECT Insurance_File.insurance_file_cnt, Party.name, Insurance_File.insurance_ref," & Strings.ChrW(13) & Strings.ChrW(10)
            'SJ 23/02/2004 - end
            sSQL = sSQL & " 'Risk Code' Risk_Code , Product.code Prod_Code, " & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " Insurance_File.cover_start_date, Insurance_File.expiry_date," & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " Address.postal_code" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " From Address, Party_Address_Usage, Insurance_File, Insurance_File_Type, product, party"
            sSQL = sSQL & " WHERE (Insurance_File_Type.Insurance_File_Type_ID <> 1"
            sSQL = sSQL & " AND Insurance_File_Type.Insurance_File_Type_ID <> 3)"
            sSQL = sSQL & " AND Insurance_File.insurance_file_type_id = Insurance_File_Type.insurance_file_type_id"
            sSQL = sSQL & " AND Insurance_File.product_id = Product.product_id"
            sSQL = sSQL & " AND Party_Address_Usage.party_cnt = Insurance_File.insured_cnt"
            sSQL = sSQL & " AND (Address.address_cnt = Party_Address_Usage.address_cnt"
            sSQL = sSQL & " AND Insurance_File.insured_cnt = party.party_cnt"
            sSQL = sSQL & " AND Party_Address_Usage.address_usage_type_id = 4)"

            iParamCount = 0

            'if the field value IS supplied

            If Not Informations.IsNothing(v_vpol_no) Then
                If v_vpol_no <> "" Then

                    'therefore, Address Cnt is present
                    'increase the parameter count by 1
                    iParamCount += 1

                    sSQL = sSQL & " AND"

                    'SJ 23/02/2004 - start
                    If m_bUnderwritingBranchEnabled Then
                        If m_bIsUnderwritingBranch Then
                            sSQL = sSQL & " (Insurance_File.alternate_reference Like '" & v_vpol_no.Trim() & "' "
                            sSQL = sSQL & " OR (Insurance_File.insurance_ref like '" & v_vpol_no.Trim() & "' AND Insurance_File.alternate_reference IS NULL))"
                        Else
                            sSQL = sSQL & " Insurance_File.insurance_ref Like '" & v_vpol_no.Trim() & "' "
                        End If
                    Else
                        sSQL = sSQL & " Insurance_File.insurance_ref Like '" & v_vpol_no.Trim() & "' "
                    End If
                    'SJ 23/02/2004 - end
                End If
            End If
            'SJ 23/02/2004 - start
            If m_bUnderwritingBranchEnabled Then
                sSQL = sSQL & " AND"
                If m_bIsUnderwritingBranch Then
                    sSQL = sSQL & " insurance_file.source_id = source.source_id "
                    sSQL = sSQL & " AND (source.underwriting_branch_ind = 1 OR insurance_file.policy_type_id = 3 or insurance_file.source_id = " & CStr(m_iSourceID) & ") "
                Else
                    sSQL = sSQL & " insurance_file.source_id = source.source_id "
                    sSQL = sSQL & " AND (source.underwriting_branch_ind = 0 or source.underwriting_branch_ind is NULL) "
                End If
            End If
            'SJ 23/02/2004 - end

            'if the field value IS supplied

            If Not Informations.IsNothing(v_vfrm_dt) Then
                If v_vfrm_dt <> "" Then

                    'therefore, Address Cnt is present
                    'increase the parameter count by 1
                    iParamCount += 1

                    sSQL = sSQL & " AND"

                    Dim TempDate As Date
                    v_vfrm_dt = If(DateTime.TryParse(v_vfrm_dt, TempDate), TempDate.ToString("dd/MM/yyyy"), v_vfrm_dt)
                    sSQL = sSQL & " CONVERT(DATETIME, Insurance_file.cover_start_date, 103) >= CONVERT(DATETIME, '" & v_vfrm_dt.Trim() & "',103)" & Strings.ChrW(13) & Strings.ChrW(10)

                End If
            End If

            'if the field value IS supplied

            If Not Informations.IsNothing(v_vto_dt) Then
                If v_vto_dt <> "" Then

                    'therefore, Address Cnt is present
                    'increase the parameter count by 1
                    iParamCount += 1

                    sSQL = sSQL & " AND"

                    Dim TempDate2 As Date
                    v_vto_dt = If(DateTime.TryParse(v_vto_dt, TempDate2), TempDate2.ToString("dd/MM/yyyy"), v_vto_dt)
                    sSQL = sSQL & " CONVERT(DATETIME, Insurance_file.expiry_date, 103) <= CONVERT(DATETIME, '" & v_vto_dt.Trim() & "',103)" & Strings.ChrW(13) & Strings.ChrW(10)

                End If
            End If

            'if the field value IS supplied

            If Not Informations.IsNothing(v_vpost_code) Then
                If v_vpost_code <> "" Then

                    'therefore, Address Cnt is present
                    'increase the parameter count by 1
                    iParamCount += 1

                    sSQL = sSQL & " AND"

                    sSQL = sSQL & " Address.postal_code Like '" & v_vpost_code.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)

                End If
            End If


            If Not Informations.IsNothing(v_vpol_code) Then
                If v_vpol_code <> "" Then

                    'increase the parameter count by 1
                    iParamCount += 1

                    sSQL = sSQL & " AND"

                    sSQL = sSQL & " Insurance_File_Type.code Like '" & v_vpol_code.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)

                End If
            End If

            'DC261001 -added check for new parameter for excluding lapsed policies

            If Not Informations.IsNothing(v_vexclude_lapsed) Then
                If v_vexclude_lapsed = 1 Then

                    iParamCount += 1

                    sSQL = sSQL & " AND"
                    sSQL = sSQL & " (Insurance_File.Insurance_File_Status_Id IS NULL OR"
                    sSQL = sSQL & " (Insurance_File.Insurance_File_Status_id IS NOT NULL AND"
                    sSQL = sSQL & " Insurance_File.Insurance_File_Status_Id NOT IN"
                    sSQL = sSQL & " (SELECT Insurance_File_Status_Id "
                    sSQL = sSQL & " FROM Insurance_File_Status"
                    sSQL = sSQL & " WHERE code = 'LAP')))"
                End If
            End If

            'if the field value IS supplied

            If Not Informations.IsNothing(v_vprty_shrt_nm) Then
                If v_vprty_shrt_nm <> "" Then

                    'therefore, Address Cnt is present
                    'increase the parameter count by 1
                    iParamCount += 1

                    sSQL = sSQL & " AND"

                    sSQL = sSQL & " Party.shortname Like '" & sShortname.Trim() & "'" & Strings.ChrW(13) & Strings.ChrW(10)

                End If
            End If

            sSQL = sSQL & " AND Insurance_File_Type.Insurance_File_Type_ID <> 1" & Strings.ChrW(13) & Strings.ChrW(10)

            'S4B Claim Enhancements R&D 2005

            If Not Informations.IsNothing(v_vClientName) Then
                If sClientName <> "" Then
                    iParamCount += 1
                    sSQL = sSQL & " AND party.resolved_name LIKE '" & sClientName & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                End If
            End If

            'MKW060204 PN10311. Exclude Deleted Policies.
            If Not v_bIncludeDeleted Then

                'therefore, Address Cnt is present
                'increase the parameter count by 1
                iParamCount += 1

                sSQL = sSQL & " AND"

                sSQL = sSQL & " (Insurance_File.Policy_ignore <> 1 or Insurance_File.Policy_ignore is null)" & Strings.ChrW(13) & Strings.ChrW(10)

            End If

            ' MKW 190503 PN2032 START.  Put valid sources in sql
            If Informations.IsArray(v_vValidSourceArray) Then
                nLower = v_vValidSourceArray.GetLowerBound(1)
                nUpper = v_vValidSourceArray.GetUpperBound(1)

                sTemp = New StringBuilder("")

                For iLoop As Integer = nLower To nUpper
                    If iLoop = nLower Then
                        sTemp = New StringBuilder("(party.source_id IN (")
                    End If


                    sTemp.Append(CStr(Val(CStr(v_vValidSourceArray(1, iLoop)))))

                    If iLoop = nUpper Then
                        sTemp.Append("))")
                    Else
                        sTemp.Append(",")
                    End If
                Next

                iParamCount += 1

                If iParamCount > 1 Then
                    sSQL = sSQL & " AND "
                Else
                    sSQL = sSQL & " WHERE "
                End If

                sSQL = sSQL & sTemp.ToString()

            End If
            ' MKW 190503 PN2032 END.

            'add the order by clause

            sSQL = sSQL & " ORDER BY insurance_file.insurance_file_cnt DESC"

            If iParamCount = 0 Then
                'no parameters passed so query cannot be executed
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement - use array for speed
            With m_oDatabase

                .Parameters.Clear()

                'DC310701 get all records not just first 500
                m_lError = .SQLSelect(sSQL:=sSQL, sSQLName:=ACInsFileSearchName, bStoredProcedure:=ACInsFileSearchStored, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords, bKeepNulls:=True)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyList")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If NO records were found return PMFalse
                If Not Informations.IsArray(r_vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Search Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name:         GetPolicyDetails (Public)
    ' Description:  Gets the Policy Details for the given policy
    '               from S for B
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '               values in the following order
    '               0-Policy Start Date
    '               1-Policy End Date
    '               2-Currency Id
    ' SP:           spu_get_policy_details
    ' Date:         28/07/2000
    ' Author:       SK
    ' ***************************************************************** '
    Public Function GetPolicyDetails(ByRef r_vResultArray(,) As Object, ByVal v_lpol_id As Integer, Optional ByVal v_vclm_dt As String = "") As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Insurance Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pol_id", vValue:=CStr(v_lpol_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Dim dtclm_dt As Date
            If v_vclm_dt <> "" Then

                'convert Claim Date from variant format to date for SP
                dtclm_dt = CDate(v_vclm_dt)

                'Add the Claim Date parameter (INPUT)
                'developer guide no.40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="clm_dt", vValue:=dtclm_dt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetails")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '    If Sirius_Product = BROKING Then    'uncomment when implementing for underwriting too

                'Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyDetailsSQL, sSQLName:=ACGetPolicyDetailsName, bStoredProcedure:=ACGetPolicyDetailsStored, vResultArray:=r_vResultArray)
                '    End If              'uncomment when implementing for underwriting too
            Else
                'if blank value is passed take default date
                v_vclm_dt = "01/01/2000"
                'Add the Claim Date parameter (INPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="clm_dt", vValue:=v_vclm_dt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetails")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                '    If Sirius_Product = BROKING Then        'uncomment when implementing for underwriting too

                'Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyDetailsSQL, sSQLName:=ACGetPolicyDetailsName, bStoredProcedure:=ACGetPolicyDetailsStored, vResultArray:=r_vResultArray)

                '    End If          'uncomment when implementing for underwriting too
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetails")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO Insurers were found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetClientDetails (Public)
    ' Description:  Gets the client details for the given policy
    '               from S for B
    ' Returns    :  r_vResultArray: (1-dimensional Array) containing
    '               values in the following order
    '               [0-Client name=sPrtyNm, 1-Client short name=sPrtyShrtNm,
    '               2-Address1=sAdd1, 3-Address2=sAdd2, 4-Address3=sAdd3,
    '               5-Address4=sAdd4, 6-Post Code=sPostCode, 7-Tele Home=sFullTeleHome,
    '               8-Tele Off=sFullTeleOff, 9-Fax=sFullFax, 10-Mobile=sFullMobile,
    '               11-Email=sFullEmail,11-Party Cnt=lPrtyCnt]
    ' Date:         20/06/2000
    ' Author:       SK
    '
    ' *****************************************************************
    ' Change history
    ' JMK 01/06/2001:       include CountryId (UW only)
    ' ***************************************************************** '
    Public Function GetClientDetails(ByRef r_vResultArray() As Object, ByVal v_lpol_id As Integer, Optional ByVal v_vclm_dt As String = "") As Integer

        Dim result As Integer = 0
        Try


            'list of all the columns in the SP query resultset
            'Const COL_ADD_CNT = 0
            Const COL_PRTY_NM As Integer = 0
            Const COL_PRTY_SHRTNM As Integer = 1
            Const COL_ADD1 As Integer = 2
            Const COL_ADD2 As Integer = 3
            Const COL_ADD3 As Integer = 4
            Const COL_ADD4 As Integer = 5
            Const COL_POSTCODE As Integer = 6
            Const COL_AREACODE As Integer = 7
            Const COL_TELENO As Integer = 8
            Const COL_EXTN As Integer = 9
            'Const COL_CONTACT_ID As Integer = 10
            Const COL_CONTACT_TYPE As Integer = 11
            Const COL_PRTY_CNT As Integer = 12
            ' JMK 01/06/2001
            Const COL_COUNTRY_ID As Integer = 13
            'RWH(13/06/01)
            Const COL_ADD_USAGE As Integer = 14

            'AR20050303 - PN15644
            'Const COL_ADDRESSID_BR As Integer = 13
            Const COL_ADDRESSID_UW As Integer = 16

            Dim sContactType As String = ""

            Dim iTeleCnt As Integer
            'Dim bTele As Boolean

            Dim lPrtyCnt As Integer
            Dim sPrtyNm As String = String.Empty
            Dim sPrtyShrtNm As String = String.Empty
            Dim sAdd1 As String = String.Empty
            Dim sAdd2 As String = String.Empty
            Dim sAdd3 As String = String.Empty
            Dim sAdd4 As String = String.Empty
            Dim sPostCode As String = String.Empty
            Dim sFullTeleHome As String = String.Empty
            Dim sFullTeleOff As String = String.Empty
            Dim sFullFax As String = String.Empty
            Dim sFullMobile As String = String.Empty
            Dim sFullEmail As String = String.Empty
            ' JMK 01/06/2001
            Dim lCountryId, lAddressId As Integer 'AR20050303 - PN15644
            'Dim sFullAdd As String
            'Dim sFullTele As String

            'RWH(05/10/01)
            Dim bUWAddressFound As Boolean

            'constants from Contact_Type_Id
            'Const TELEPHONE As Integer = 1
            'Const FAX As Integer = 2
            'Const EMAIL As Integer = 3
            'Const MOBILE As Integer = 4
            'Const WORKPHONE As Integer = 10
            'Const HOMEPHONE As Integer = 9

            iTeleCnt = 0

            'array declared to get query resultset using
            'the database's SQLSelect function
            Dim vSqlArray(,) As Object = Nothing

            ' JMK 01/06/2001
            'AR20050303 - PN15644
            Dim varray1d(14) As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Insurance Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pol_id", vValue:=CStr(v_lpol_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Dim dtclm_dt As Date
            If v_vclm_dt <> "" Then

                'convert Claim Date from variant format to date for SP
                dtclm_dt = CDate(v_vclm_dt)

                'Add the Claim Date parameter (INPUT)
                'developer guide no.40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="clm_dt", vValue:=dtclm_dt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientDetails")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'TN20001207 - START


                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClientDetailsUSQL, sSQLName:=ACGetClientDetailsUName, bStoredProcedure:=ACGetClientDetailsUStored, vResultArray:=vSqlArray)


            Else

                '**********************************TN20001207***************************
                'WHY ARE WE DEFAULTING TO '01/01/200???????

                'if blank value is passed take default date
                v_vclm_dt = "01/01/2000"
                'Add the Claim Date parameter (INPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="clm_dt", vValue:=v_vclm_dt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientDetails")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'TN20001207 - START


                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClientDetailsUSQL, sSQLName:=ACGetClientDetailsUName, bStoredProcedure:=ACGetClientDetailsUStored, vResultArray:=vSqlArray)

            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientDetails")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'TN20001207 - START
            If Not Informations.IsArray(vSqlArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If
            'TN20001207 - END

            'Assign the party & Address Details from the 1st row to the variables

            sPrtyShrtNm = CStr(vSqlArray(COL_PRTY_SHRTNM, 0)).Trim()

            sPrtyNm = CStr(vSqlArray(COL_PRTY_NM, 0)).Trim()

            sAdd1 = CStr(vSqlArray(COL_ADD1, 0)).Trim()

            sAdd2 = CStr(vSqlArray(COL_ADD2, 0)).Trim()

            sAdd3 = CStr(vSqlArray(COL_ADD3, 0)).Trim()

            sAdd4 = CStr(vSqlArray(COL_ADD4, 0)).Trim()

            sPostCode = CStr(vSqlArray(COL_POSTCODE, 0)).Trim()

            lPrtyCnt = CInt(CStr(vSqlArray(COL_PRTY_CNT, 0)).Trim())
            ' JMK 01/06/2001

            'RWH(08/10/01) Convert data type in case blank.

            lCountryId = CInt(Val(CStr(vSqlArray(COL_COUNTRY_ID, 0))))
            'AR20050303 - PN15644

            lAddressId = gPMFunctions.ToSafeLong(CStr(vSqlArray(COL_ADDRESSID_UW, 0)), 0)
            'DC281101 moved following into UNDERWRITING check
            'End If

            'RWH(05/10/01) We need to ensure that the correspondence address is displayed.

            If CStr(vSqlArray(COL_ADD_USAGE, 0)).Trim().ToUpper() = "CORRESPONDENCE ADDRESS" Then
                bUWAddressFound = True
            End If



            For iRow As Integer = 0 To vSqlArray.GetUpperBound(1)

                'RWH(05/10/01) Make sure we display the correspondence address for the client.
                If Not bUWAddressFound Then

                    If CStr(vSqlArray(COL_ADD_USAGE, iRow)).Trim().ToUpper() = "CORRESPONDENCE ADDRESS" Then

                        sAdd1 = CStr(vSqlArray(COL_ADD1, iRow)).Trim()

                        sAdd2 = CStr(vSqlArray(COL_ADD2, iRow)).Trim()

                        sAdd3 = CStr(vSqlArray(COL_ADD3, iRow)).Trim()

                        sAdd4 = CStr(vSqlArray(COL_ADD4, iRow)).Trim()

                        sPostCode = CStr(vSqlArray(COL_POSTCODE, iRow)).Trim()
                        'RWH(08/10/01) Convert data type in case blank.

                        lCountryId = CInt(Val(CStr(vSqlArray(COL_COUNTRY_ID, iRow))))
                        'AR20050303 - PN15644

                        lAddressId = gPMFunctions.ToSafeLong(CStr(vSqlArray(COL_ADDRESSID_UW, iRow)), 0)

                        bUWAddressFound = True
                    End If
                End If


                sContactType = CStr(vSqlArray(COL_CONTACT_TYPE, iRow)).Trim()



                Select Case sContactType.ToUpper().Trim()
                    'Case CStr(TELEPHONE)  'if ContactType is TELEPHONE
                    '     sFullTeleHome = CStr(vSqlArray.GetValue(COL_AREACODE, iRow)).Trim() & " " & CStr(vSqlArray.GetValue(COL_TELENO, iRow)).Trim() & " " & CStr(vSqlArray.GetValue(COL_EXTN, iRow)).Trim()
                    Case "HOMEPHONE"
                        sFullTeleHome = CStr(vSqlArray.GetValue(COL_AREACODE, iRow)).Trim() & " " & CStr(vSqlArray.GetValue(COL_TELENO, iRow)).Trim() & " " & CStr(vSqlArray.GetValue(COL_EXTN, iRow)).Trim()
                    Case "WORKPHONE"
                        sFullTeleOff = CStr(vSqlArray.GetValue(COL_AREACODE, iRow)).Trim() & " " & CStr(vSqlArray.GetValue(COL_TELENO, iRow)).Trim() & " " & CStr(vSqlArray.GetValue(COL_EXTN, iRow)).Trim()

                    Case "FAX"



                        'If CStr(vSqlArray(COL_ADD_USAGE, iRow)).Trim().ToUpper() = "CORRESPONDENCE ADDRESS" Then

                        sFullFax = CStr(vSqlArray(COL_AREACODE, iRow)).Trim() & " " & CStr(vSqlArray(COL_TELENO, iRow)).Trim() & " " & CStr(vSqlArray(COL_EXTN, iRow)).Trim()
                        'End If

                    Case "E-MAIL"



                        If CStr(vSqlArray(COL_ADD_USAGE, iRow)).Trim() = "" Then

                            sFullEmail = CStr(vSqlArray(COL_TELENO, iRow)).Trim()
                        End If

                    Case "MOBILE"

                        If CStr(vSqlArray.GetValue(COL_ADD_USAGE, iRow)).Trim() = "" Then

                            sFullMobile = CStr(vSqlArray(COL_AREACODE, iRow)).Trim() & " " & CStr(vSqlArray(COL_TELENO, iRow)).Trim() & "  " & CStr(vSqlArray(COL_EXTN, iRow)).Trim()
                        End If

                End Select

            Next iRow


            varray1d(0) = sPrtyNm

            varray1d(1) = sPrtyShrtNm

            varray1d(2) = sAdd1

            varray1d(3) = sAdd2

            varray1d(4) = sAdd3

            varray1d(5) = sAdd4

            varray1d(6) = sPostCode

            varray1d(7) = sFullTeleHome.Trim

            varray1d(8) = sFullTeleOff.Trim

            varray1d(9) = sFullFax.Trim

            varray1d(10) = sFullMobile.Trim

            varray1d(11) = sFullEmail.Trim

            varray1d(12) = lPrtyCnt

            ' JMK 01/06/2001

            varray1d(13) = lCountryId

            varray1d(14) = lAddressId

            r_vResultArray = varray1d


            ' If NO Clients were found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetInsurerDetails (Public)
    ' Description:  Gets the Insurer details for the given policy
    '               from S for B
    ' Returns    :  r_vResultArray: (1-dimensional Array) containing
    '               values in the following order
    '               [0-Insurer name=sPrtyNm, 1-Insurer short name=sPrtyShrtNm,
    '               2-Address1=sAdd1, 3-Address2=sAdd2, 4-Address3=sAdd3,
    '               5-Address4=sAdd4, 6-Post Code=sPostCode, 7-Tele Home=sFullTeleHome,
    '               8-Fax=sFullFax, 9-Email=sFullEmail]
    ' Date:         20/06/2000
    ' Author:       SK
    ' Note : v_vclm_dt is no longer used in proc spu_get_insurer_details
    ' *****************************************************************
    ' Change history
    ' JMK 01/06/2001:       include CountryId (UW only)
    ' ***************************************************************** '
    Public Function GetInsurerDetails(ByRef r_vResultArray() As Object, ByVal v_lpol_id As Integer, Optional ByVal v_vclm_dt As Object = Nothing, Optional ByVal lTransactionMode As Integer = gPMConstants.PMEComponentAction.PMView) As Integer

        Dim result As Integer = 0
        Try


            'list of all the columns in the SP query resultset
            'Const COL_ADD_CNT = 0
            Const COL_PRTY_NM As Integer = 0
            Const COL_PRTY_SHRTNM As Integer = 1
            Const COL_ADD1 As Integer = 2
            Const COL_ADD2 As Integer = 3
            Const COL_ADD3 As Integer = 4
            Const COL_ADD4 As Integer = 5
            Const COL_POSTCODE As Integer = 6
            Const COL_AREACODE As Integer = 7
            Const COL_TELENO As Integer = 8
            Const COL_EXTN As Integer = 9
            Const COL_CONTACT_ID As Integer = 10
            'Const COL_CONTACT_TYPE As Integer = 11
            ' JMK 01/06/2001
            Const COL_COUNTRY_ID As Integer = 12
            'RWH(13/06/01)
            Const COL_ADD_USAGE As Integer = 13

            'AR20050303 - PN15644
            'Const COL_ADDRESSID_BR As Integer = 13
            Const COL_ADDRESSID_UW As Integer = 15
            'PN48079
            Const COL_INSURERCONTACT As Long = 16

            Dim sContactType As String = ""

            'Dim iTeleCnt As Integer


            Dim sPrtyNm As String = ""
            Dim sPrtyShrtNm As String = ""
            Dim sAdd1 As String = ""
            Dim sAdd2 As String = ""
            Dim sAdd3 As String = ""
            Dim sAdd4 As String = ""
            Dim sPostCode As String = ""
            Dim sFullTeleHome As String = ""
            'Dim sFullTeleOff As String
            Dim sFullFax As String = ""
            Dim sFullEmail As String = ""
            ' JMK 01/06/2001
            Dim lCountryId, lAddressId As Integer 'AR20050303 - PN15644
            'Dim sFullAdd As String
            'Dim sFullTele As String

            'RWH(05/10/01)
            Dim bUWAddressFound As Boolean
            'PN48079
            Dim sInsurerContact As String

            'constants from Contact_Type_Id
            Const TELEPHONE As Integer = 1
            Const FAX As Integer = 2
            Const EMAIL As Integer = 3
            'Const MOBILE As Integer = 4
            'RWH(13/06/01) Need to use Business number for agent.
            Const BUSTEL As Integer = 9

            'iTeleCnt = 0

            'array declared to get query resultset using
            'the database's SQLSelect function
            Dim vSqlArray(,) As Object = Nothing
            'JMK 04/06/2001 Add CountryID
            Dim varray1d(12) As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pol_id", vValue:=CStr(v_lpol_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="TransactionMode", vValue:=CStr(lTransactionMode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsurerDetailsSQL, sSQLName:=ACGetInsurerDetailsName, bStoredProcedure:=ACGetInsurerDetailsStored, vResultArray:=vSqlArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerDetails")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'TN20001206 - START
            If Not Informations.IsArray(vSqlArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If
            'TN20001206 - END

            'Assign the party & Address Details from the 1st row to the variables

            sPrtyShrtNm = CStr(vSqlArray(COL_PRTY_SHRTNM, 0)).Trim()

            sPrtyNm = CStr(vSqlArray(COL_PRTY_NM, 0)).Trim()

            sAdd1 = CStr(vSqlArray(COL_ADD1, 0)).Trim()

            sAdd2 = CStr(vSqlArray(COL_ADD2, 0)).Trim()

            sAdd3 = CStr(vSqlArray(COL_ADD3, 0)).Trim()

            sAdd4 = CStr(vSqlArray(COL_ADD4, 0)).Trim()

            sPostCode = CStr(vSqlArray(COL_POSTCODE, 0)).Trim()
            'JMK 04/06/2001



            lCountryId = CInt(Val(CStr(vSqlArray(COL_COUNTRY_ID, 0))))

            lAddressId = gPMFunctions.ToSafeLong(CStr(vSqlArray(COL_ADDRESSID_UW, 0)), 0)

            'RWH(05/10/01) We need to ensure that the correspondence address is displayed.


            If CStr(vSqlArray(COL_ADD_USAGE, 0)).Trim().ToUpper() = "CORRESPONDENCE ADDRESS" Then
                bUWAddressFound = True
            End If
            'PN48079
            sInsurerContact = Trim(vSqlArray(COL_INSURERCONTACT, 0))
            For iRow As Integer = 0 To vSqlArray.GetUpperBound(1)

                'RWH(05/10/01) Make sure we display the correspondence address for the client.
                If Not bUWAddressFound Then

                    If CStr(vSqlArray(COL_ADD_USAGE, iRow)).Trim().ToUpper() = "CORRESPONDENCE ADDRESS" Then

                        sAdd1 = CStr(vSqlArray(COL_ADD1, iRow)).Trim()

                        sAdd2 = CStr(vSqlArray(COL_ADD2, iRow)).Trim()

                        sAdd3 = CStr(vSqlArray(COL_ADD3, iRow)).Trim()

                        sAdd4 = CStr(vSqlArray(COL_ADD4, iRow)).Trim()

                        sPostCode = CStr(vSqlArray(COL_POSTCODE, iRow)).Trim()
                        'RWH(08/10/01) Convert data type in case blank.

                        lCountryId = CInt(Val(CStr(vSqlArray(COL_COUNTRY_ID, iRow))))
                        'AR20050303 - PN15644

                        lAddressId = gPMFunctions.ToSafeLong(CStr(vSqlArray(COL_ADDRESSID_UW, iRow)), 0)

                        bUWAddressFound = True
                    End If
                End If


                sContactType = CStr(vSqlArray(COL_CONTACT_ID, iRow)).Trim()


                Select Case sContactType
                    'RWH(05/10/01) Use home phone number attached to Correspondence Address for UW agent.
                    Case CStr(TELEPHONE)


                        If CStr(vSqlArray(COL_ADD_USAGE, iRow)).Trim().ToUpper() = "CORRESPONDENCE ADDRESS" Then

                            sFullTeleHome = CStr(vSqlArray(COL_AREACODE, iRow)).Trim() & " " & CStr(vSqlArray(COL_TELENO, iRow)).Trim() & " " & CStr(vSqlArray(COL_EXTN, iRow)).Trim()
                        End If

                        'RWH(13/06/01) Use business phone number for agent.
                    Case CStr(BUSTEL)
                        '    Case TELEPHONE    'if ContactType is TELEPHONE

                        'If Sirius_Product <> UNDERWRITING Then
                        '    If bTele = False Then        'if it has not been repeated more than twice
                        '        sFullTeleHome = Trim(vSqlArray(COL_AREACODE, iRow)) & " " & Trim(vSqlArray(COL_TELENO, iRow)) & " " & Trim(vSqlArray(COL_EXTN, iRow))
                        '        bTele = True
                        '    End If
                        'End If

                    Case CStr(FAX)
                        'RWH(05/10/01) Use fax no. attached to corres address for UW agent.


                        If CStr(vSqlArray(COL_ADD_USAGE, iRow)).Trim().ToUpper() = "CORRESPONDENCE ADDRESS" Then

                            sFullFax = CStr(vSqlArray(COL_AREACODE, iRow)).Trim() & " " & CStr(vSqlArray(COL_TELENO, iRow)).Trim() & " " & CStr(vSqlArray(COL_EXTN, iRow)).Trim()
                        End If
                    Case CStr(EMAIL)


                        If CStr(vSqlArray(COL_ADD_USAGE, iRow)).Trim().ToUpper() = "CORRESPONDENCE ADDRESS" Then

                            sFullEmail = CStr(vSqlArray(COL_TELENO, iRow)).Trim()
                        End If
                End Select

            Next iRow


            varray1d(0) = sPrtyNm

            varray1d(1) = sPrtyShrtNm

            varray1d(2) = sAdd1

            varray1d(3) = sAdd2

            varray1d(4) = sAdd3

            varray1d(5) = sAdd4

            varray1d(6) = sPostCode

            varray1d(7) = sFullTeleHome

            varray1d(8) = sFullFax

            varray1d(9) = sFullEmail
            'JMK 04/06/2001


            varray1d(10) = lCountryId

            'AR20050303 - PN15644

            varray1d(11) = lAddressId
            'PN48079
            varray1d(12) = sInsurerContact
            r_vResultArray = varray1d


            ' If NO Insurers were found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsurerDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name:         GetRiskDetails (Public)
    ' Description:  If BROKING Gets the Risk Code Id, Risk Code
    '               from Risk_code & Event_Insurance_File table in S for B
    '               If UNDERWRITING Gets the Risk Id, Risk Description
    '               from Risk & Event_Insurance_File table in S for B
    '               for the given pol_id, clm_dt (not being useed in SP)
    ' Returns:      r_vResultArray: (1-dimensional Array) containing
    '               values in the following order
    '               0-Risk Code ID      =iRiskCodeID,
    '               1-Risk Description  =sRiskDesc
    ' SP:           BROKING-spu_get_risk_details (pol_id, clm_dt)
    ' SP:           UNDERWRITING-spu_get_risk_details_U (pol_id, clm_dt)
    ' Date:         20/06/2000
    ' Author:       SK
    ' ***************************************************************** '
    Public Function GetRiskDetails(ByRef r_vResultArray(,) As Object, ByVal v_lpol_id As Integer, Optional ByVal v_vclm_dt As String = "", Optional ByVal v_vclm_no As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Insurance Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pol_id", vValue:=CStr(v_lpol_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Dim dtclm_dt As Date
            If v_vclm_dt <> "" Then

                'convert Claim Date from variant format to date for SP
                dtclm_dt = CDate(v_vclm_dt)

                'Add the Claim Date parameter (INPUT)
                'developer guide no.40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="clm_dt", vValue:=dtclm_dt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDetails")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Execute SQL Statement
                If TransactionType = "C_CO" Then
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskDetailsOpenClaimUSQL, sSQLName:=ACGetRiskDetailsOpenClaimUName, bStoredProcedure:=ACGetRiskDetailsOpenClaimUStored, vResultArray:=r_vResultArray)
                Else
                    '"v_vclm_no        "

                    If Not Informations.IsNothing(v_vclm_no) Then
                        'Add the Claim Date parameter (INPUT)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="clm_no", vValue:=CStr(v_vclm_no), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDetails")

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskDetailsUSQL, sSQLName:=ACGetRiskDetailsUName, bStoredProcedure:=ACGetRiskDetailsUStored, vResultArray:=r_vResultArray)
                End If


            Else
                'if blank value is passed take default date
                v_vclm_dt = "01/01/2000"
                'Add the Claim Date parameter (INPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="clm_dt", vValue:=v_vclm_dt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDetails")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                'Execute SQL Statement
                If TransactionType = "C_CO" Then
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskDetailsOpenClaimUSQL, sSQLName:=ACGetRiskDetailsOpenClaimUName, bStoredProcedure:=ACGetRiskDetailsOpenClaimUStored, vResultArray:=r_vResultArray)
                Else
                    '"v_vclm_no        "

                    If Not Informations.IsNothing(v_vclm_no) Then
                        'Add the Claim Date parameter (INPUT)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="clm_no", vValue:=CStr(v_vclm_no), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDetails")

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskDetailsUSQL, sSQLName:=ACGetRiskDetailsUName, bStoredProcedure:=ACGetRiskDetailsUStored, vResultArray:=r_vResultArray)
                End If


                '    End If          'uncomment when implementing for underwriting too
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDetails")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO Insurers were found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name:         GetRiskDesc (Public)
    ' Description:  Gets the Risk Description for the given Risk Id
    '               from Risk_code table in S for B
    ' Returns:      r_vResultArray: (1-dimensional Array) containing
    '               values in the following order
    '               0-Risk Code=sRiskCode,
    '               1-Risk Description=sRiskDesc
    ' SP:           spu_get_risk_desc
    ' Date:         20/06/2000
    ' Author:       SK
    ' ***************************************************************** '
    Public Function GetRiskDesc(ByRef r_vResultArray(,) As Object, ByVal v_irisk_code_id As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Risk Code Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_code_id", vValue:=CStr(v_irisk_code_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDesc")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    If Sirius_Product = BROKING Then    'uncomment when implementing for underwriting too

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskDescSQL, sSQLName:=ACGetRiskDescName, bStoredProcedure:=ACGetRiskDescStored, vResultArray:=r_vResultArray)
            '    End If              'uncomment when implementing for underwriting too


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDesc")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO record was found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskDesc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDesc", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    '*************Added Method For Making It Comatible For Client Server Model*********
    '*************Author -Pandu- 12-10-2000********************************************************

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' Edit History:Pandu
    '
    ' Date        :12-10-2000
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Dim r_vResultArray(,) As Object = Nothing

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
            m_iSourceID = iSourceId
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


            m_lReturn = CType(GetSiriusProd(r_vResultArray), gPMConstants.PMEReturnCode)


            '    Set the Sirius_Product property

            m_sSPName = CStr(r_vResultArray(0, 0))

            'SJ 23/02/2004 - start
            'Are we running the folgate branch acting as insurer solution
            m_lReturn = CType(bUnderwritingBranchFunc.GetUnderwritingBranchDetails(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase, v_sCallingAppName:=m_sCallingAppName, r_bUnderwritingBranchEnabled:=m_bUnderwritingBranchEnabled, r_bIsUnderwritingBranch:=m_bIsUnderwritingBranch), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUnderwritingBranchDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If
            'SJ 23/02/2004 - end


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '*************End of Method For Making It Comatible For Client Server Model*********
    '*************Author -Pandu- 12-10-2000********************************************************

    '*******************************************************************************
    ' Name : GetPolicyForClaimDate
    ' Desc : get closest version of policy for claim date
    ' Hist : 23 May 2001 - Created  Tinny
    '        15/02/2004 - Alix - Process Sheet 52/53
    '*******************************************************************************
    Public Function GetPolicyForClaimDate(ByVal v_dtClaimDate As Date, ByRef r_lInsuranceFileCnt As Integer, ByRef r_sPolicyNumber As String, ByRef r_dtStartDate As Date, Optional ByRef r_lReturnCode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim lCurrentPosition As Integer

        Dim dtFirstStartDate, dtLastExpiryDate As Date
        Dim bFoundVoid As Boolean

        Const ACInsuranceFileCnt As Integer = 0
        Const ACStartDate As Integer = 1
        Const ACEndDate As Integer = 2
        Const ACPolicyRef As Integer = 3
        Const ACLapsedReason As Integer = 4

        Const ReturnCode_Error As Integer = 0
        Const ReturnCode_Ok As Integer = 1
        Const ReturnCode_TooEarly As Integer = 2
        Const ReturnCode_TooLate As Integer = 3
        Const ReturnCode_Voided As Integer = 4

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Pass insurance file cnt in
            m_oDatabase.Parameters.Clear()
            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(r_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_lReturnCode = ReturnCode_Error
                Return result
            End If

            ' Get all policy versions
            result = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyForClaimDateSQL, sSQLName:=ACGetPolicyForClaimDateName, bStoredProcedure:=ACGetPolicyForClaimDateStored, vResultArray:=vResultArray, bKeepNulls:=False)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_lReturnCode = ReturnCode_Error
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                r_lReturnCode = ReturnCode_Error
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Find a version of the policy which encompasses the claim date
            lCurrentPosition = -1

            For lCount As Integer = 0 To vResultArray.GetUpperBound(1)


                If (CDate(vResultArray(ACStartDate, lCount)) <= v_dtClaimDate) And (v_dtClaimDate <= CDate(vResultArray(ACEndDate, lCount))) Then

                    If CStr(vResultArray(ACLapsedReason, lCount)).Trim() <> "VOIDED" Then
                        ' We found a valid one, note its position and exit!
                        lCurrentPosition = lCount
                        Exit For
                    Else
                        ' We found a void one, we carry on searching but note this to report to user
                        bFoundVoid = True
                        If lCount = 0 Then
                            Exit For
                        End If
                    End If
                End If
                ' Save the earliest start date and the latest expiry date

                If CDate(vResultArray(ACStartDate, lCount)) < dtFirstStartDate Or lCount = 0 Then

                    dtFirstStartDate = CDate(vResultArray(ACStartDate, lCount))
                End If

                If CDate(vResultArray(ACEndDate, lCount)) > dtLastExpiryDate Or lCount = 0 Then

                    dtLastExpiryDate = CDate(vResultArray(ACEndDate, lCount))
                End If
            Next

            If lCurrentPosition <> -1 Then

                ' Found one, we returns its details

                r_sPolicyNumber = CStr(vResultArray(ACPolicyRef, lCurrentPosition))

                r_dtStartDate = CDate(vResultArray(ACStartDate, lCurrentPosition))

                r_lInsuranceFileCnt = CInt(vResultArray(ACInsuranceFileCnt, lCurrentPosition))

                r_lReturnCode = ReturnCode_Ok

            Else

                ' Else, we check why we haven't found one, to report to user
                If bFoundVoid Then
                    ' We found one but it was VOID
                    r_lReturnCode = ReturnCode_Voided
                ElseIf v_dtClaimDate > dtLastExpiryDate Then
                    ' The claim date is before the earliest start date
                    r_lReturnCode = ReturnCode_TooLate
                ElseIf v_dtClaimDate < dtFirstStartDate Then
                    ' The claim date if after the latest expiry date
                    r_lReturnCode = ReturnCode_TooEarly
                Else
                    ' We can't find any policy, probably because the claim date is in between
                    ' two valid policy versions
                    r_lReturnCode = ReturnCode_Error
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyForClaimDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyForClaimDate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC250501 routine to duble up apostrophe
    '******************************************************************************
    ' Apostrophes
    '
    ' Take a string and replace ' with ''
    '
    '******************************************************************************
    Public Function Apostrophes(ByRef sString As String) As Integer

        Dim result As Integer = 0
        Dim i As Integer
        Dim sTemp As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sString.Length = 0 Then
                Return result
            End If

            sTemp = New StringBuilder("")

            Do While True
                i = (sString.IndexOf("'"c) + 1)

                If i = 0 Then
                    sTemp.Append(sString)
                    Exit Do
                End If

                sTemp.Append(sString.Substring(0, i - 1) & "''")
                sString = sString.Substring(i)
            Loop

            sString = sTemp.ToString()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Run Time Error", vApp:=ACApp, vClass:="ExtraFunc", vMethod:="Apostrophes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckInRenewal
    '
    ' Description: Verifies if the Insurance Folder is in Renewal
    '
    ' ***************************************************************** '
    Public Function CheckInRenewal(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRenewalStatus As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lRenewalStatus = -1

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_check_in_renewal", sSQLName:="Check in Renewal", bStoredProcedure:=True, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Return result
            End If


            r_lRenewalStatus = CInt(vArray(0, 0))

            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckInRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class