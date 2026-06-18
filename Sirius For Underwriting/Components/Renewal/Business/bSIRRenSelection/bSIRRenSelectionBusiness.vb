Option Strict Off
Option Explicit On
'Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System.Globalization
'developer guide no. 129
Imports System.Text
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 02/09/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRRenSelection.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' *****************************************************************
    ' Added to replace global variables 29/09/2003
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
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    'RiskData object
    'Private m_oGIS As Object
    Private m_oDataSet As cGISDataSetControl.Application
    'Private m_oPremiumFinance As Object

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Calling Application Name

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer
    Private m_lNumberOPolicies As Integer

    'RWH(15/05/2001) Put single policy into renewal.
    Private m_vRenewalsForPolicy As Object

    Private m_lTransactionType As Integer
    Private m_sUnderwritingOrAgency As String = ""
    Private m_bSystemOptionClientBlacklistingInForce As Boolean
    'Private m_oSIRListRisks As Object

    Private m_lPolicyVersionIncrement As Integer

    Private m_bUnderwritingYearID As Boolean 'Is option switched on/off
    Private bIsValid As Boolean
    Private oPolicyRenewalRules(,) As Object
    Private oRenewalRiskRules(,) As Object
    Private m_sFailureCriterion As String = String.Empty
    Private bSkipGenerateRenewalPolicyNumber As Boolean = False
    Private oBatchRenewalBusiness As bSIRRenewalBusiness

    Private m_dInception_date_tpi As Date
    'Private m_oReinsurance As Object

    ' Primary Keys to work with
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property TransactionType() As Integer
        Get
            If m_lTransactionType = 0 Then
                m_lReturn = GetTransactionType()
            End If

            Return m_lTransactionType
        End Get
    End Property
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
    ''' <summary>
    ''' This Property is used to skip the  Policy number Generation of Renewal whenever Renewal menthod woulld be called from BDX utility.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SkipGenerateRenewalPolicyNumber() As Boolean
        Get
            Return Me.bSkipGenerateRenewalPolicyNumber
        End Get
        Set(ByVal value As Boolean)
            Me.bSkipGenerateRenewalPolicyNumber = value
        End Set
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Dim sValue As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            'sj 16/12/2002 - start
            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now
            'sj 16/12/2002 - end

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDataSet = New cGISDataSetControl.Application()

            ' get black listed client system option
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=kSystemOptionBlackListClientInForce, r_sOptionValue:=sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOption BlackList Client In Force Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bSystemOptionClientBlacklistingInForce = (sValue = "1")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Entry point for any termination code for this
    '''          object.
    ''' </summary>
    ''' <remarks></remarks>
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                oPolicyRenewalRules = Nothing
                oRenewalRiskRules = Nothing
                If m_oDataSet IsNot Nothing Then
                    m_oDataSet.Dispose()
                    m_oDataSet = Nothing
                End If

                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
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

    ' ***************************************************************** '
    ' Name: GetLookUp (Private)
    '
    ' Description: get values from look up table
    '
    ' ***************************************************************** '
    Public Function GetLookUp(ByVal v_sTableName As String, ByVal v_sKeyIDFieldName As String, ByVal v_sDescFieldName As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = ""
            sSQL = sSQL & "SELECT " & v_sKeyIDFieldName & ", " & v_sDescFieldName
            sSQL = sSQL & " FROM " & v_sTableName

            If v_sTableName = "Source" Then
                ' Only load branches accessible to the current user
                sSQL = sSQL & " WHERE source_id not in (select source_id from PMUser_Source where user_id = " & CStr(m_iUserID) & ") "
            Else
                ' Exclude deleted records if we are not loading the branches.
                ' (if we ARE loading the branches, then we do want the deleted ones, i.e. the closed branches)
                'Get only those product which have renewable in product risk maintenance
                If v_sTableName <> "" AndAlso v_sTableName.ToUpper() = "PRODUCT" Then
                    sSQL = sSQL & " WHERE is_deleted = 0 and is_renewable=1"
                Else
                    sSQL = sSQL & " WHERE is_deleted = 0"
                End If
            End If
            sSQL = sSQL & " ORDER BY " & v_sDescFieldName

            m_oDatabase.Parameters.Clear()

            Return m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetLookupValues", bStoredProcedure:=False, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookUp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookUp", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRenewalSelection (Private)
    '
    ' Description:get policies which are not in Renewal Status table or has the status of "Policy Details Changed"
    '                   and expiry date is within range, specified in the Product table and user entered date.
    '                   add these to Renewal_Report table
    'History: 04/09/2000 Created (TN)
    'Changed: 29/08/2002 Added v_vBranchID (MD)
    'Thinh Nguyen 13/02/2004 add optional start date
    ' ***************************************************************** '
    Public Function GetRenewalSelection(ByVal v_vProductID As Object, ByVal v_vBranchID As Object, ByVal v_dtCompareDate As Date, ByRef r_vResultArray As Object) As Integer
        Return GetRenewalSelection(v_vProductID:=v_vProductID, v_vBranchID:=v_vBranchID, v_dtCompareDate:=v_dtCompareDate, r_vResultArray:=r_vResultArray, v_vStartDate:=Nothing)
    End Function

    Public Function GetRenewalSelection(ByVal v_vProductID As Object, ByVal v_vBranchID As Object, ByVal v_dtCompareDate As Date, ByRef r_vResultArray As Object, ByVal v_vStartDate As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'check to see if ProductID is passed in correctly

            Dim dbNumericTemp As Double

            If Not (Convert.IsDBNull(v_vProductID) Or Informations.IsNothing(v_vProductID)) And Not Double.TryParse(CStr(v_vProductID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="v_vProductID must be Null or Numeric", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalSelection", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set global source id (MD)

            m_iSourceID = If(Convert.IsDBNull(v_vBranchID) Or Informations.IsNothing(v_vBranchID), 0, v_vBranchID)

            'clear parameter list and add in required parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_vProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=v_vBranchID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="compare_date", vValue:=v_dtCompareDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'didn't do cdate(v_vStartDate) because vb is too stupid to know that the first part  of the IF expression is true

            'm_lReturn = m_oDatabase.Parameters.Add(sName:="Start_Date", vValue:=CStr(If(Informations.Informations.IsNothing(v_vStartDate) Or Convert.IsDBNull(v_vStartDate) Or Informations.IsNothing(v_vStartDate), DBNull.Value, v_vStartDate)), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If Informations.IsNothing(v_vStartDate) OrElse Convert.IsDBNull(v_vStartDate) Then
                result = m_oDatabase.Parameters.Add(sName:="Start_Date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                result = m_oDatabase.Parameters.Add(sName:="Start_Date", vValue:=v_vStartDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'START PN43571--SS
            m_lReturn = m_oDatabase.Parameters.Add(sName:="userid", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'ENDED PN43571--SS

            'parameters are added sucessfully 'AJM 03/08/01 - Get all records

            Return m_oDatabase.SQLSelect(sSQL:=ACSelRenewalListSQL, sSQLName:=ACSelRenewalListName, bStoredProcedure:=ACSelRenewalListStored, vResultArray:=r_vResultArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewalSelection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalSelection", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DelRenewalReport (Private)
    '
    ' Description: Delete all records in Renewal_Report table
    '
    'History: 06/09/2000 Created (TN)
    '         05/06/2001 - RWH Included user_id.
    ' ***************************************************************** '
    Public Function DelRenewalReport() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If BeginTrans() = gPMConstants.PMEReturnCode.PMTrue Then

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_oDatabase.SQLAction(sSQL:=ACDelRenewalReportSQL, sSQLName:=ACDelRenewalReportName, bStoredProcedure:=ACDelRenewalReportStored) = gPMConstants.PMEReturnCode.PMTrue Then

                    result = CommitTrans()
                Else
                    m_lReturn = RollbackTrans()
                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelRenewalReport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelRenewalReport", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRisk (Private)
    '
    ' Desc: get all associate risks for policy
    '
    ' History: 11/10/2000 Created (TN)
    ' ***************************************************************** '
    Public Function GetRisk(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_oDatabase.SQLSelect(sSQL:=ACSelRiskCntSQL, sSQLName:=ACSelRiskCntName, bStoredProcedure:=ACSelRiskCntStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ApplyIndexLink (Private)
    '
    ' Desc: apply index link value to each sum_insured
    '
    ' ***************************************************************** '
    Public Function ApplyIndexLink(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskID As Integer, ByVal v_dtEffectiveDate As Date) As Integer

        Dim result As Integer = 0
        Const ACFieldPosRSAPolicyBinderID As Integer = 0
        Const ACFieldPosSumInsuredTypeID As Integer = 1
        Const ACFieldPosSequenceID As Integer = 2
        Const ACFieldPosSumInsured As Integer = 3
        Const ACFieldPosIndexLinkingID As Integer = 4

        Dim vSumInsuredArray(,) As Object = Nothing
        Dim vPercentage As Object = Nothing 'index linking value
        Dim dNewSumInsured As Double

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get all associated sum insured
            If GetIndexLink(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskID:=v_lRiskID, r_vResultArray:=vSumInsuredArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'do we have any data
            If Not Informations.IsArray(vSumInsuredArray) Then
                Return result
            End If

            'loop thro and index link each sum_insured

            For lCount As Integer = 0 To vSumInsuredArray.GetUpperBound(1)

                'only apply index linking if sum_insured is not null or zero
                Dim auxVar As Object = vSumInsuredArray(ACFieldPosSumInsured, lCount)

                If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                    If CDbl(vSumInsuredArray(ACFieldPosSumInsured, lCount)) <> 0.0# Then

                        'get index link value to be applied

                        'developer guide no. 98
                        If GetIndexLinkDetail(v_lIndexLinkID:=CInt(vSumInsuredArray(ACFieldPosIndexLinkingID, lCount)), v_dtEffectiveDate:=v_dtEffectiveDate, r_vPercentage:=vPercentage) <> gPMConstants.PMEReturnCode.PMTrue Then

                            Return gPMConstants.PMEReturnCode.PMFalse

                        End If

                        'only apply index linking if index linking value is not null or zero

                        If Not (Convert.IsDBNull(vPercentage) Or Informations.IsNothing(vPercentage)) Then

                            If CDbl(vPercentage) <> 0.0# Then

                                dNewSumInsured = CDbl(vSumInsuredArray(ACFieldPosSumInsured, lCount)) * (1 + (CDbl(vPercentage) / 100))

                                'save new value to RSA_Sum_Insured table

                                If UpdRSASumInsured(v_lRSAPolicyBinderID:=CInt(vSumInsuredArray(ACFieldPosRSAPolicyBinderID, lCount)), v_lSumInsuredTypeID:=CInt(vSumInsuredArray(ACFieldPosSumInsuredTypeID, lCount)), v_lSequenceID:=CInt(vSumInsuredArray(ACFieldPosSequenceID, lCount)), v_dSumInsured:=dNewSumInsured) <> gPMConstants.PMEReturnCode.PMTrue Then

                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                            End If

                        End If

                    End If

                End If

            Next lCount

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ApplyIndexLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyIndexLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetGisIndexLink (Private)
    '
    ' Desc: get all fields and objects on GIS that needs index linking
    '
    ' ***************************************************************** '
    Public Function GetGisIndexLink(ByVal v_vGisScreenID As Object, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_screen_id", vValue:=v_vGisScreenID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_oDatabase.SQLSelect(sSQL:=ACSelGISIndexLinkSQL, sSQLName:=ACSelGISIndexLinkName, bStoredProcedure:=ACSelGISIndexLinkStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGisIndexLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGisIndexLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: IsRIComplete
    '
    ' Description: check to see if reinsurance is complete
    '              both at policy and risk level
    '
    ' History: 18/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function IsRIComplete(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lIsComplete As Integer) As Integer
        Return IsRIComplete(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lIsComplete:=r_lIsComplete, v_lRiskLevelOnly:=0)
    End Function

    Public Function IsRIComplete(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lIsComplete As Integer, ByVal v_lRiskLevelOnly As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim cValue As Decimal

        Const ACFieldPosCount As Integer = 0
        Const ACFieldPosValue As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_lIsComplete = gPMConstants.PMEReturnCode.PMTrue

            If v_lRiskLevelOnly <> gPMConstants.PMEReturnCode.PMTrue Then
                'do we have policy level reinsurance and is it complete
                m_oDatabase.Parameters.Clear()

                result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                result = m_oDatabase.SQLSelect(sSQL:=ACIsPolicyRICompleteSQL, sSQLName:=ACIsPolicyRICompleteName, bStoredProcedure:=ACIsPolicyRICompleteStored, vResultArray:=vResultArray, bKeepNulls:=True)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                If Informations.IsArray(vResultArray) Then
                    'do we have policy level reinsurance

                    If CDbl(vResultArray(ACFieldPosCount, 0)) > 0 Then

                        Dim auxVar As Object = vResultArray(ACFieldPosValue, 0)

                        If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Then
                            r_lIsComplete = gPMConstants.PMEReturnCode.PMFalse
                            Return result
                        End If

                        cValue = CDec(vResultArray(ACFieldPosValue, 0))

                        'does it add up to 100% for each policy (there should only be one policy)

                        If cValue <> (CDbl(vResultArray(ACFieldPosCount, 0)) * 100) Then
                            r_lIsComplete = gPMConstants.PMEReturnCode.PMFalse
                            Return result
                        End If
                    End If
                End If
            End If

            'we either haven't got policy level reinsurance of its complete - now do risk level

            'do we have risk level reinsurance and is it complete
            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACIsRiskRICompleteSQL, sSQLName:=ACIsRiskRICompleteName, bStoredProcedure:=ACIsRiskRICompleteStored, vResultArray:=vResultArray, bKeepNulls:=True)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'Tomo010702
            'Removed this chunk as we do it based on sums insured rather than % reinsured.  And the stored
            'procedure will return either 0 or 1

            '    If Informations.IsArray(vResultArray) Then
            '        If vResultArray(ACFieldPosCount, 0) > 0 Then
            '
            '            If IsNull(vResultArray(ACFieldPosValue, 0)) Then
            '                r_lIsComplete = PMFalse
            '                Exit Function
            '            End If
            '
            '            cValue = vResultArray(ACFieldPosValue, 0)
            '
            '            'does it add up to 100% for each risk
            '            If cValue <> (vResultArray(ACFieldPosCount, 0) * 100) Then
            '                r_lIsComplete = PMFalse
            '                Exit Function
            '            End If
            '
            '        End If
            '    End If

            If Informations.IsArray(vResultArray) Then
                Dim auxVar_2 As Object = vResultArray(0, 0)

                If Not (Convert.IsDBNull(auxVar_2) Or Informations.IsNothing(auxVar_2)) Then

                    If CStr(vResultArray(0, 0)) = "1" Then
                        r_lIsComplete = gPMConstants.PMEReturnCode.PMTrue
                        Return result
                    Else
                        r_lIsComplete = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If
                Else
                    r_lIsComplete = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            Else
                r_lIsComplete = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

        Catch
        End Try

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsRIComplete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsRIComplete", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: IsQuoted
    '
    ' Description: is policy quoted
    '
    ' History: 19/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function IsQuoted(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lIsQuoted As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_lIsQuoted = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACIsQuotedSQL, sSQLName:=ACIsQuotedName, bStoredProcedure:=ACIsQuotedStored, vResultArray:=vResultArray, bKeepNulls:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Dim auxVar As Object = vResultArray(0, 0)

            If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Then
                Return result
            End If

            If CDbl(vResultArray(0, 0)) = 0 Then
                r_lIsQuoted = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsQuoted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsQuoted", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' check to see if current policy has any claim
    ''' return PMTrue if policy has no claims or has claims with allowed causations
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckForClaim(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="nInsuranceFileCNT", vValue:=CStr(v_lInsuranceFileCnt),
                                          iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong) <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACSelCheckForClaimSQL, sSQLName:=ACSelCheckForClaimName,
                                           bStoredProcedure:=ACSelCheckForClaimStored, vResultArray:=vResultArray, bKeepNulls:=True)

            If result = PMEReturnCode.PMTrue Then
                'do we have any claims
                If Informations.IsArray(vResultArray) Then
                    vResultArray = Nothing
                    'If we have claims see if they are inside allowed bounds.
                    m_oDatabase.Parameters.Clear()

                    If m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(v_lInsuranceFileCnt),
                                                  iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong) <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFalse
                    End If

                    result = m_oDatabase.SQLSelect(sSQL:=ACSelCheckForClaimValueSQL, sSQLName:=ACSelCheckForClaimValueName,
                                                   bStoredProcedure:=ACSelCheckForClaimValueStored, vResultArray:=vResultArray)

                    If Informations.IsArray(vResultArray) Then
                        result = PMEReturnCode.PMFalse
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception
            result = PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="CheckForClaim Failed",
                               vApp:=ACApp, vClass:=ACClass, vMethod:="CheckForClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddRenewalStatus (private)
    '
    ' Description: add policy to renewal status table
    '
    'History: 05/09/2000 Created (TN)
    '         24/01/2002 add optional param to return renewal status count (Thinh Nguyen)
    '
    ' ***************************************************************** '
    Public Function AddRenewalStatus(ByVal v_lProductId As Integer, ByVal v_lRenewalStatusTypeID As Integer, ByVal v_lInsuranceHolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_vLeadAgentCnt As String, ByVal v_lRenewalInsuranceFileCnt As Integer, ByRef r_vRenewalStatusCnt As Object) As Integer
        Return AddRenewalStatus(v_lProductId:=v_lProductId, v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID, v_lInsuranceHolderCnt:=v_lInsuranceHolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_vLeadAgentCnt:=v_vLeadAgentCnt, v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lBrokerXferStatusTypeID:=0, r_vRenewalStatusCnt:=r_vRenewalStatusCnt)
    End Function

    Public Function AddRenewalStatus(ByVal v_lProductId As Integer, ByVal v_lRenewalStatusTypeID As Integer, ByVal v_lInsuranceHolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_vLeadAgentCnt As String, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lBrokerXferStatusTypeID As Integer) As Integer
        Return AddRenewalStatus(v_lProductId:=v_lProductId, v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID, v_lInsuranceHolderCnt:=v_lInsuranceHolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_vLeadAgentCnt:=v_vLeadAgentCnt, v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lBrokerXferStatusTypeID:=v_lBrokerXferStatusTypeID, r_vRenewalStatusCnt:=Nothing)
    End Function

    Public Function AddRenewalStatus(ByVal v_lProductId As Integer, ByVal v_lRenewalStatusTypeID As Integer, ByVal v_lInsuranceHolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_vLeadAgentCnt As String, ByVal v_lRenewalInsuranceFileCnt As Integer) As Integer
        Return AddRenewalStatus(v_lProductId:=v_lProductId, v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID, v_lInsuranceHolderCnt:=v_lInsuranceHolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_vLeadAgentCnt:=v_vLeadAgentCnt, v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lBrokerXferStatusTypeID:=0, r_vRenewalStatusCnt:=Nothing)
    End Function

    Public Function AddRenewalStatus(ByVal v_lProductId As Integer, ByVal v_lRenewalStatusTypeID As Integer, ByVal v_lInsuranceHolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_vLeadAgentCnt As String, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lBrokerXferStatusTypeID As Integer, ByRef r_vRenewalStatusCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If BeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If v_vLeadAgentCnt = "" Then

                v_vLeadAgentCnt = Nothing
            End If

            m_oDatabase.Parameters.Clear()

            'Thinh Nguyen 24/01/2001 - change from PMParamInput to PMParamOutput

            ' Developer Guide No. 85
            result = m_oDatabase.Parameters.Add(sName:="renewal_status_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If
            'developer guide no. 98
            result = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_lProductId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="renewal_status_type_id", vValue:=v_lRenewalStatusTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="insurance_holder_cnt", vValue:=v_lInsuranceHolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If
            If Informations.IsNothing(v_vLeadAgentCnt) OrElse v_vLeadAgentCnt = "" Then
                result = m_oDatabase.Parameters.Add(sName:="lead_agent_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                result = m_oDatabase.Parameters.Add(sName:="lead_agent_cnt", vValue:=v_vLeadAgentCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="created_by_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="date_created", vValue:=DateTime.Today, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            'Developer Guide No. 85
            result = m_oDatabase.Parameters.Add(sName:="critical_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="renewal_insurance_file_cnt", vValue:=v_lRenewalInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="is_invite_printed", vValue:=gPMConstants.PMEReturnCode.PMFalse, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            'Developer Guide No. 85
            result = m_oDatabase.Parameters.Add(sName:="BrokerXferStatusTypeID", vValue:=If(v_lBrokerXferStatusTypeID = 0, DBNull.Value, CStr(v_lBrokerXferStatusTypeID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            result = m_oDatabase.SQLAction(sSQL:=ACAddRenewalStatusSQL, sSQLName:=ACAddRenewalStatusName, bStoredProcedure:=ACAddRenewalStatusStored)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
            End If

            result = CommitTrans()

            'Thinh Nguyen 24/01/2002 (start)
            If result = gPMConstants.PMEReturnCode.PMTrue Then

                If Not Informations.IsNothing(r_vRenewalStatusCnt) Then

                    r_vRenewalStatusCnt = m_oDatabase.Parameters.Item("renewal_status_cnt").Value
                End If
            End If
            'Thinh Nguyen 24/01/2002 (end)

            Return result

        Catch excep As System.Exception

            m_lReturn = RollbackTrans()

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRenewalStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRenewalStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddRenewalReport (Private)
    '
    ' Description: add policy to renewal_report table
    '
    'History: 02/09/2000 Created (TN)
    '
    '   RWH(15/11/2000) Removed 3 unrequired fields and added optional failure ones.
    '   RWH(04/06/2001) Updated to include user_id.
    '   JMK 13/Jan/2003 cdate() date parameters
    ' ***************************************************************** '
    Public Function AddRenewalReport(ByVal v_sReportType As String, ByVal v_vClientName As Object, ByVal v_vPolicyNumber As Object, ByVal v_vAgentCode As Object, ByVal v_vCoverStartDate As Object, ByVal v_vCoverEndDate As Object, ByVal v_vProductCode As Object, ByVal v_vFailureCriterion As String) As Integer
        Return AddRenewalReport(v_sReportType:=v_sReportType, v_vClientName:=v_vClientName, v_vPolicyNumber:=v_vPolicyNumber, v_vAgentCode:=v_vAgentCode, v_vCoverStartDate:=v_vCoverStartDate, v_vCoverEndDate:=v_vCoverEndDate, v_vProductCode:=v_vProductCode, v_vFailureCriterion:=v_vFailureCriterion, v_vFailureDetail:="", v_vInsuranceFileCnt:="")
    End Function

    Public Function AddRenewalReport(ByVal v_sReportType As String, ByVal v_vClientName As Object, ByVal v_vPolicyNumber As Object, ByVal v_vAgentCode As Object, ByVal v_vCoverStartDate As Object, ByVal v_vCoverEndDate As Object, ByVal v_vProductCode As Object, ByVal v_vFailureCriterion As String, ByVal v_vFailureDetail As String) As Integer
        Return AddRenewalReport(v_sReportType:=v_sReportType, v_vClientName:=v_vClientName, v_vPolicyNumber:=v_vPolicyNumber, v_vAgentCode:=v_vAgentCode, v_vCoverStartDate:=v_vCoverStartDate, v_vCoverEndDate:=v_vCoverEndDate, v_vProductCode:=v_vProductCode, v_vFailureCriterion:=v_vFailureCriterion, v_vFailureDetail:=v_vFailureDetail, v_vInsuranceFileCnt:="")
    End Function

    Public Function AddRenewalReport(ByVal v_sReportType As String, ByVal v_vClientName As Object, ByVal v_vPolicyNumber As Object, ByVal v_vAgentCode As Object, ByVal v_vCoverStartDate As Object, ByVal v_vCoverEndDate As Object, ByVal v_vProductCode As Object, ByVal v_vFailureCriterion As String, ByVal v_vFailureDetail As String, ByVal v_vInsuranceFileCnt As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If BeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="report_type", vValue:=v_sReportType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="client_name", vValue:=CStr(v_vClientName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_number", vValue:=CStr(v_vPolicyNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_code", vValue:=CStr(v_vAgentCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'JMK - convert dates

            'developer guide no. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cover_start_date", vValue:=CDate(v_vCoverStartDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            'developer guide no. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cover_end_date", vValue:=CDate(v_vCoverEndDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_code", vValue:=CStr(v_vProductCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If Informations.IsNothing(v_vFailureCriterion) Then
                v_vFailureCriterion = ""
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="failure_criterion", vValue:=v_vFailureCriterion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If Informations.IsNothing(v_vFailureDetail) Then
                v_vFailureDetail = ""
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="failure_detail", vValue:=v_vFailureDetail, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If Informations.IsNothing(v_vInsuranceFileCnt) Then
                v_vInsuranceFileCnt = ""
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(gPMFunctions.ToSafeLong(v_vInsuranceFileCnt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If m_oDatabase.SQLAction(sSQL:=ACAddRenewalReportSQL, sSQLName:=ACAddRenewalReportName, bStoredProcedure:=ACAddRenewalReportStored) = gPMConstants.PMEReturnCode.PMTrue Then

                    result = CommitTrans()
                Else
                    m_lReturn = RollbackTrans()
                End If

            End If

            Return result

        Catch excep As System.Exception

            m_lReturn = RollbackTrans()

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRenewalReport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRenewalReport", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DelRenewalStatusPolicies (Private)
    '
    ' Description: Delete Policies from Renewal Status and insurance table which has
    '                    the status of "Policy Detail Changed"
    '                    and are within date range specified in the Product table.
    '
    '                    1. delete renewal version of policies in the insurance file
    '                    2. change original policies status back to Live
    '                    3. delete policies from renewal status table
    '
    'History: 02/09/2000 Created (TN)
    '
    ' Thinh Nguyen 13/02/2004 add optional start date
    ' ***************************************************************** '
    Public Function DelRenewalStatusPolicies(ByVal v_lRenewalStatusTypeID As Integer, ByVal v_dtCompareDate As Date) As Integer
        Return DelRenewalStatusPolicies(v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID, v_dtCompareDate:=v_dtCompareDate, v_vStartDate:=Nothing)
    End Function

    Public Function DelRenewalStatusPolicies(ByVal v_lRenewalStatusTypeID As Integer, ByVal v_dtCompareDate As Date, ByVal v_vStartDate As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '***********************NOTES******************************
            '********These functions must be executed in the correct order*************

            m_lReturn = BeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsNothing(v_vStartDate) Then

                v_vStartDate = DBNull.Value
            End If

            'delete renewal version policies from insurance file
            m_lReturn = DelRenewalPolicies(v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID, v_dtCompareDate:=v_dtCompareDate, v_vStartDate:=v_vStartDate)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                result = m_lReturn
                m_lReturn = RollbackTrans()
                Return result
            End If

            'change status of renewal (original) policies to Null (Live)
            m_lReturn = UpdRenewalPolicies(v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID, v_dtCompareDate:=v_dtCompareDate, v_vStartDate:=v_vStartDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                m_lReturn = RollbackTrans()
                Return result
            End If

            'delete from last print run
            m_lReturn = DeleteLastPrintRun(v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID, v_dtCompareDate:=v_dtCompareDate, v_vStartDate:=v_vStartDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                m_lReturn = RollbackTrans()
                Return result
            End If

            'delete renewal policies from renewal status
            m_lReturn = DelRenewalStatus(v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID, v_dtCompareDate:=v_dtCompareDate, v_vStartDate:=v_vStartDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                m_lReturn = RollbackTrans()
                Return result
            End If

            m_lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            m_lReturn = RollbackTrans()

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelRenewalStatusPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelRenewalStatusPolicies", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ReRatePolicy (Private)
    '
    ' Description: ReRate Policy
    '
    ' History: 04/09/2000 Created (TN)
    ' ***************************************************************** '
    Public Function ReRatePolicy(ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim vRiskIDArray, oTestHarness As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get all associate risks
            If GetRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResultArray:=vRiskIDArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Policy must have risk(s) attached to it
            If Not Informations.IsArray(vRiskIDArray) Then
                'RWH(16/11/2000) Policies don't need risks attached so this is
                'not a failure.
                '        ReRatePolicy = PMFalse
                Return result
            End If

            'set up the Test object and rerate each risk
            'TODO LIST :bSirTestHatness not found in the complete list 
            'oTestHarness = New bSIRTestHarness.Business()
            oTestHarness = Nothing

            If oTestHarness Is Nothing Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRTestHarness object", vApp:=ACApp, vClass:=ACClass, vMethod:="ReRatePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTestHarness.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database)) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'loop thro and rerate each risk

            For lCount As Integer = 0 To vRiskIDArray.GetUpperBound(1)
                'rerate policy for this risk

                If oTestHarness.ReRate(v_sDataModel:="RSA", v_lInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_lRiskID:=vRiskIDArray(0, lCount)) <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to re-rate Policy InsuranceFileCnt: " & v_lInsuranceFileCnt & " RiskID: " & CStr(vRiskIDArray(0, lCount)), vApp:=ACApp, vClass:=ACClass, vMethod:="ReRatePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Exit For

                End If
            Next

            oTestHarness.Dispose()
            oTestHarness = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReRatePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReRatePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : AddTaskToWorkManager
    '
    ' Desc : Add task to work manager
    '        sj 13/12/2002 - Add task code as optional parameter
    ' ***************************************************************** '
    Public Function AddTaskToWorkManager(ByVal v_sClientName As String, ByVal v_sDescription As String, ByVal v_dtDueDate As Date) As Integer
        Return AddTaskToWorkManager(v_sClientName:=v_sClientName, v_sDescription:=v_sDescription, v_dtDueDate:=v_dtDueDate, v_vKeyArray:=Nothing, v_iUserID:=0, v_sTaskCode:="RENAMEND")
    End Function

    Public Function AddTaskToWorkManager(ByVal v_sClientName As String, ByVal v_sDescription As String, ByVal v_dtDueDate As Date, ByVal v_vKeyArray As Object) As Integer
        Return AddTaskToWorkManager(v_sClientName:=v_sClientName, v_sDescription:=v_sDescription, v_dtDueDate:=v_dtDueDate, v_vKeyArray:=v_vKeyArray, v_iUserID:=0, v_sTaskCode:="RENAMEND")
    End Function

    Public Function AddTaskToWorkManager(ByVal v_sClientName As String, ByVal v_sDescription As String, ByVal v_dtDueDate As Date, ByVal v_vKeyArray As Object, ByVal v_iUserID As Integer) As Integer
        Return AddTaskToWorkManager(v_sClientName:=v_sClientName, v_sDescription:=v_sDescription, v_dtDueDate:=v_dtDueDate, v_vKeyArray:=v_vKeyArray, v_iUserID:=v_iUserID, v_sTaskCode:="RENAMEND")
    End Function

    Public Function AddTaskToWorkManager(ByVal v_sClientName As String, ByVal v_sDescription As String, ByVal v_dtDueDate As Date, ByVal v_iUserID As Integer) As Integer
        Return AddTaskToWorkManager(v_sClientName:=v_sClientName, v_sDescription:=v_sDescription, v_dtDueDate:=v_dtDueDate, v_vKeyArray:=Nothing, v_iUserID:=v_iUserID, v_sTaskCode:="RENAMEND")
    End Function

    Public Function AddTaskToWorkManager(ByVal v_sClientName As String, ByVal v_sDescription As String, ByVal v_dtDueDate As Date, ByVal v_sTaskCode As String) As Integer
        Return AddTaskToWorkManager(v_sClientName:=v_sClientName, v_sDescription:=v_sDescription, v_dtDueDate:=v_dtDueDate, v_vKeyArray:=Nothing, v_iUserID:=0, v_sTaskCode:=v_sTaskCode)
    End Function

    Public Function AddTaskToWorkManager(ByVal v_sClientName As String, ByVal v_sDescription As String, ByVal v_dtDueDate As Date, ByVal v_vKeyArray As Object, ByVal v_iUserID As Integer, ByVal v_sTaskCode As String) As Integer

        Dim result As Integer = 0
        Dim oWrkTaskInstance As bPMWrkTaskInstance.TaskControl

        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lPMWrkTaskID, lPMWrkTaskGroupID, lPMUserGroupID As Integer
        Dim sCustomer As String = ""
        Dim lPMWrkTaskInstanceCnt As Integer

        Dim sTaskGroupID, sUserGroupID As String

        'sj 13/12/2002 - start
        'Const PMTaskMemo = "RENAMEND"
        'sj 13/12/2002 - end

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            oWrkTaskInstance = New bPMWrkTaskInstance.TaskControl
            If oWrkTaskInstance.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get renewal task group from system option
            m_lReturn = bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, 1021, sTaskGroupID, 1)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'get renewal user group from system option
            m_lReturn = bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, 1022, sUserGroupID, 1)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If sTaskGroupID = String.Empty Then
                sTaskGroupID = "7"
            End If

            If sUserGroupID = String.Empty Then
                sUserGroupID = "1"
            End If

            lPMWrkTaskGroupID = CInt(sTaskGroupID)
            lPMUserGroupID = CInt(sUserGroupID)

            ' Get the task_id
            sSQL = "SELECT pmwrk_task_id FROM PMWrk_Task WHERE code = {task_code}"

            m_oDatabase.Parameters.Clear()

            'sj 13/12/2002 - start
            If m_oDatabase.Parameters.Add(sName:="task_code", vValue:=v_sTaskCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'sj 13/12/2002 - end

            If m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskID", bStoredProcedure:=False, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the task_id

            lPMWrkTaskID = CInt(vResultArray(0, 0))

            'create task

            If oWrkTaskInstance.CreateNew(v_lPMWrkTaskGroupID:=lPMWrkTaskGroupID, v_lPMWrkTaskID:=lPMWrkTaskID, v_sCustomer:=v_sClientName, v_dtTaskDueDate:=v_dtDueDate, v_lPMUserGroupID:=lPMUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, v_iIsUrgent:=0, r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt, v_iUserID:=v_iUserID, v_vKeyArray:=v_vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : GetPolicyForRenewal
    '
    ' Desc : get correct version of policy for renewal
    '
    ' Hist : 13/02/2001 Created - Tinny
    ' ***************************************************************** '
    Public Function GetPolicyForRenewal(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'parameters are added sucessfully
            result = m_oDatabase.SQLSelect(sSQL:=ACSelPolicyForRenewalSQL, sSQLName:=ACSelPolicyForRenewalName, bStoredProcedure:=ACSelPolicyForRenewalStored, vResultArray:=r_vResultArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'yeap, we have data

            If r_vResultArray.GetUpperBound(1) > 0 Then
                'can't have more than one version of policy
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed - more than one versions of policy is selected", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyForRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyForRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyForRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name :GetRenewalVersion
    '
    ' Desc : get all versions of this policy which are in renewal
    '
    ' Hist : 13/02/2001 Created - Tinny
    ' ***************************************************************** '
    Public Function GetRenewalVersion(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get all versions of this policy which are in renewal
            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'parameters are added sucessfully

            Return m_oDatabase.SQLSelect(sSQL:=ACSelRenewalVersionSQL, sSQLName:=ACSelRenewalVersionName, bStoredProcedure:=ACSelRenewalVersionStored, vResultArray:=r_vResultArray)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewalVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalVersion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name :GetRenewalQuoteRef
    '
    ' Desc : get Renewal Quote Ref for updating the policy after renewal
    '
    ' Hist : 23/12/2013 Created - Sumeet
    ' ***************************************************************** '
    Public Function GetRenewalQuoteRef(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sInsuranceQuoteRef As String) As Integer

        Dim result As Integer = 0
        Dim r_vResultArray(,) As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get all versions of this policy which are in renewal
            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'parameters are added sucessfully

            result = m_oDatabase.SQLSelect(sSQL:=ACSelRenewalQuoteRefSQL, sSQLName:=ACSelRenewalQuoteRefName, bStoredProcedure:=ACSelRenewalQuoteRefStored, vResultArray:=r_vResultArray)

            r_sInsuranceQuoteRef = r_vResultArray(0, 0)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewalVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalVersion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name :DeleteRenewalPolicy
    '
    ' Desc : delete all versions of policy from renewal
    '
    ' Hist : 13/02/2001 Created - Tinny
    ' ***************************************************************** '
    Public Function DeleteRenewalPolicy(ByVal v_lInsuranceFolderCnt As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get all versions of this policy which are in renewal
            result = CType(GetRenewalVersion(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Get Renewal Version Of Policy", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewalPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return result
            End If

            'loop through and delete each renewal policy from renewal

            For lCount As Integer = 0 To vResultArray.GetUpperBound(1)

                result = CType(DeleteRenewal(v_lRenewalInsuranceFileCnt:=CInt(vResultArray(lCount, 0))), gPMConstants.PMEReturnCode)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit For
                End If
            Next

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRenewalPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewalPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : DeleteRenewal
    '
    ' Desc :  1. get original version of policy
    '           2. delete renewal version in Insurance File table
    '           3. change original version of policy back to live
    '           4. delete policy from Renewal Status table
    '
    ' Hist : 12/02/2001 Created - Tinny
    '
    ' ***************************************************************** '
    Public Function DeleteRenewal(ByVal v_lRenewalInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lOriginalInsuranceFileCnt As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get original version of policy
            result = GetOriginalPolicy(v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, r_lInsuranceFileCnt:=lOriginalInsuranceFileCnt)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                If result = gPMConstants.PMEReturnCode.PMNotFound Then
                    'policy is not in renewal, nothing to delete so we return true
                    result = gPMConstants.PMEReturnCode.PMTrue

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Policy Is Not In Renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Else
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Get Original Version Of Policy", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                End If

                Return result
            End If

            'delete renewal version of policy
            result = DeletePolicy(v_lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Tech Spec PGR 8.8 Renewals ---------------------------------
            result = ReorderLaterPolicyVersions(v_lInsuranceFileCnt:=lOriginalInsuranceFileCnt)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            ' End --------------------------------------------------------

            'change original version of policy back to live
            result = UpdatePolicyStatus(v_lInsuranceFileCnt:=lOriginalInsuranceFileCnt)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' delete from last print run (as foreign key link to renewal status stops the whole process)
            result = DeleteRenewalsLastPrintRun(v_lRenewalInsuranceFileCnt)

            'delete from renewal_status table

            Return DeleteRenewalStatus(v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : GetOriginalPolicy
    '
    ' Desc : get original version of policy before being renewal
    '
    ' Hist : 12/02/2001 Created - Tinny
    ' ***************************************************************** '
    Public Function GetOriginalPolicy(ByVal v_lRenewalInsuranceFileCnt As Integer, ByRef r_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="renewal_insurance_file_cnt", vValue:=CStr(v_lRenewalInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'parameters are added sucessfully
            result = m_oDatabase.SQLSelect(sSQL:=ACSelOriginalPolicySQL, sSQLName:=ACSelOriginalPolicyName, bStoredProcedure:=ACSelOriginalPolicyStored, vResultArray:=vResultArray)

            If result = gPMConstants.PMEReturnCode.PMTrue Then
                If Informations.IsArray(vResultArray) Then

                    r_lInsuranceFileCnt = CInt(vResultArray(0, 0))
                Else
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOriginalPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Public Methods (End)

    'Private Methods (Begin)

    ' ***************************************************************** '
    ' Name : DeleteRenewalStatus
    '
    ' Desc : delete policy from renewal status table
    '
    ' Hist : 12/02/2001 Created - Tinny
    ' ***************************************************************** '
    Private Function DeleteRenewalStatus(ByVal v_lRenewalInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            result = BeginTrans()

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="renewal_insurance_file_cnt", vValue:=CStr(v_lRenewalInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            result = m_oDatabase.SQLAction(sSQL:=ACDelRenewalStatusRecordSQL, sSQLName:=ACDelRenewalStatusRecordName, bStoredProcedure:=ACDelRenewalStatusRecordStored)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            m_lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
            End If

            Return result

        Catch excep As System.Exception

            m_lReturn = RollbackTrans()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRenewalStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewalStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : UpdatePolicyStatus
    '
    ' Desc : update policy status to live
    '
    ' Hist : 12/02/2001 Created - Tinny
    ' ***************************************************************** '
    Private Function UpdatePolicyStatus(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            result = BeginTrans()

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            result = m_oDatabase.SQLAction(sSQL:=ACUpdPolicyStatusSQL, sSQLName:=ACUpdPolicyStatusName, bStoredProcedure:=ACUpdPolicyStatusStored)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return result
            End If

            m_lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
            End If

            Return result

        Catch excep As System.Exception

            m_lReturn = RollbackTrans()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : UpdateRiskStatus
    '
    ' Desc : update risk status to quoted
    '
    ' Hist : 26/05/2001 Created - Tom
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (UpdateRiskStatus) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UpdateRiskStatus(ByVal v_lRiskCnt As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'm_oDatabase.Parameters.Clear()
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(v_lRiskCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdRiskStatusSQL, sSQLName:=ACUpdRiskStatusName, bStoredProcedure:=ACUpdRiskStatusStored)
    '
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' Tech Spec PGR 8.8 Renewals ---------------------------------
    Private Function ReorderLaterPolicyVersions(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ReorderLaterPolicyVersions"


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="original_insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "m_oDatabase.Parameters.Add Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACReorderLaterPolicyVersionsSQL, sSQLName:=ACReorderLaterPolicyVersionsName, bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Call to SP " & ACUpdRiskStatusSQL & "Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function
    ' End --------------------------------------------------------

    ' ***************************************************************** '
    ' Name : DeletePolicy
    '
    ' Desc : delete policy and all dependencies
    '
    ' Hist : 12/02/2001 Created - Tinny
    '        21/02/2001 RWH - Delete Insurance_File_System & Insurance_File
    '                           records directly, rather than using
    '                           InsuranceFileServices component which failed.
    '
    ' ***************************************************************** '
    Public Function DeletePolicy(ByVal v_lInsuranceFileCnt As Integer) As Integer

        'Dim oPolicy As Object
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = BeginTrans()

            'delete all dependencies first
            lReturn = CType(DeletePolicyDependant(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'delete Insurance_File_System record.
            lReturn = CType(DeleteInsuranceFileSystem(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Finally delete Insurance_File record itself.
            lReturn = CType(DeleteInsuranceFile(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = CommitTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            lReturn = RollbackTrans()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeletePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : DeletePolicyDependant
    '
    ' Desc : delete all policy dependents eg risk, perils etc
    '
    ' Hist : 15/02/2001 Created - Tinny
    '
    ' Note : for now we only delete insurance_file_risk_link, ins_file_ri_arrangement,
    '           ri_arrangement_line and event_log
    '           DON'T FORGET TO DELETE THE REST LATER
    ' ***************************************************************** '
    Private Function DeletePolicyDependant(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        Return m_oDatabase.SQLAction(sSQL:=ACDelPolicyDependantSQL, sSQLName:=ACDelPolicyDependantName, bStoredProcedure:=ACDelPolicyDependantStored)

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetTransactionType
    '
    ' Description:
    '
    ' History: 03/07/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetTransactionType() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:="REN", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransactionTypeSQL, sSQLName:=ACGetTransactionTypeName, bStoredProcedure:=ACGetTransactionTypeStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then

                m_lTransactionType = CInt(vArray(0, 0))
                vArray = Nothing
                Return result
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransactionType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

    ' ***************************************************************** '
    ' Name: GetIndexLinkDetail (Private)
    '
    ' Desc: get newest value of index linking detail
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function GetIndexLinkDetail(ByVal v_lIndexLinkID As Integer, ByVal v_dtEffectiveDate As Date, ByRef r_vPercentage As Object) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="index_linking_id", vValue:=CStr(v_lIndexLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'developer guide no. 40
        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelIndexLinkDetailSQL, sSQLName:=ACSelIndexLinkDetailName, bStoredProcedure:=ACSelIndexLinkDetailStored, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        'do we have any index linking detail
        If Informations.IsArray(vResultArray) Then
            'get value to be applied to index linking

            r_vPercentage = vResultArray(4, 0)
        Else
            r_vPercentage = 0
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetIndexLink (Private)
    '
    ' Desc: all sum insured that needs index linking
    '
    ' ***************************************************************** '
    Private Function GetIndexLink(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=CStr(v_lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return m_oDatabase.SQLSelect(sSQL:=ACSelIndexLinkSQL, sSQLName:=ACSelIndexLinkName, bStoredProcedure:=ACSelIndexLinkStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

    End Function

    ' ***************************************************************** '
    ' Name: UpdRSASumInsured (Private)
    '
    ' Desc: update RSA Sum Insured with new sum_insured
    '
    ' ***************************************************************** '
    Private Function UpdRSASumInsured(ByVal v_lRSAPolicyBinderID As Integer, ByVal v_lSumInsuredTypeID As Integer, ByVal v_lSequenceID As Integer, ByVal v_dSumInsured As Double) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="rsa_policy_binder_id", vValue:=CStr(v_lRSAPolicyBinderID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="sum_insured_type_id", vValue:=CStr(v_lSumInsuredTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="sequence_id", vValue:=CStr(v_lSequenceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="sum_insured", vValue:=CStr(v_dSumInsured), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return m_oDatabase.SQLAction(sSQL:=ACUpdRSASumInsuredSQL, sSQLName:=ACUpdRSASumInsuredName, bStoredProcedure:=ACUpdRSASumInsuredStored)

    End Function

    ' ***************************************************************** '
    ' Name: DelRenewalPolicies (Public)
    '
    ' Description: Delete renewal policies from insurance file. Policies which are within date range
    '                    specified by Product table and Status on the Renewal_Status table = v_sStatus
    '                    Parent function will control the transaction modes.
    'History: 02/09/2000 Created (TN)
    '
    'Thinh Nguyen 13/02/2004 add optional start date
    ' ***************************************************************** '
    Private Function DelRenewalPolicies(ByVal v_lRenewalStatusTypeID As Integer, ByVal v_dtCompareDate As Date, Optional ByVal v_vStartDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vRenewalPolicies(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="renewal_status_type_id", vValue:=v_lRenewalStatusTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        result = m_oDatabase.Parameters.Add(sName:="compare_Date", vValue:=v_dtCompareDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        'didn't do cdate(v_vStartDate) because vb is too stupid to know that the first part  of the IF expression is true

        'result = m_oDatabase.Parameters.Add(sName:="Start_Date", vValue:=CStr(If(Informations.IsNothing(v_vStartDate) Or Convert.IsDBNull(v_vStartDate) Or Informations.IsNothing(v_vStartDate), DBNull.Value, v_vStartDate)), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If Informations.IsNothing(v_vStartDate) OrElse Convert.IsDBNull(v_vStartDate) Then
            result = m_oDatabase.Parameters.Add(sName:="Start_Date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        Else
            result = m_oDatabase.Parameters.Add(sName:="Start_Date", vValue:=v_vStartDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        End If

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        result = m_oDatabase.SQLSelect(sSQL:=ACSelRenewalPolicySQL, sSQLName:=ACSelRenewalPolicyName, bStoredProcedure:=ACSelRenewalPolicyStored, vResultArray:=vRenewalPolicies, bKeepNulls:=True)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If Not Informations.IsArray(vRenewalPolicies) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        For lCount As Integer = 0 To vRenewalPolicies.GetUpperBound(1)

            result = DeletePolicy(v_lInsuranceFileCnt:=CInt(vRenewalPolicies(0, lCount)))

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit For
            End If
        Next

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteLastPrintRun
    '
    ' Description: Delete from Last Print Run table Policies which are within date range
    '                    specified by Product table and status = v_vStatus
    '                    Parent function will control the transaction modes
    'History: 16/10/2000 Created (TN)
    '
    'Thinh Nguyen add optional start date
    ' ***************************************************************** '
    Private Function DeleteLastPrintRun(ByVal v_lRenewalStatusTypeID As Integer, ByVal v_dtCompareDate As Date, Optional ByVal v_vStartDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        m_oDatabase.Parameters.Clear()
        'developer guide no. 98
        m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_status_type_id", vValue:=v_lRenewalStatusTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="compare_Date", vValue:=v_dtCompareDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        'didn't do cdate(v_vStartDate) because vb is too stupid to know that the first part  of the IF expression is true

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Start_Date", vValue:=If(Informations.IsNothing(v_vStartDate) Or Convert.IsDBNull(v_vStartDate) Or Informations.IsNothing(v_vStartDate), DBNull.Value, v_vStartDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            result = m_oDatabase.SQLAction(sSQL:=ACDelLastPrintRunSQL, sSQLName:=ACDelLastPrintRunName, bStoredProcedure:=ACDelLastPrintRunStored)
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DelRenewalStatus (Public)
    '
    ' Description: Delete from renewal status table. Policies which are within date range
    '                    specified by Product table and status = v_vStatus
    '                    Parent function will control the transaction modes
    'History: 02/09/2000 Created (TN)
    '
    'Thinh Nguyen 13/02/2004 add optional start date
    ' ***************************************************************** '
    Private Function DelRenewalStatus(ByVal v_lRenewalStatusTypeID As Integer, ByVal v_dtCompareDate As Date, Optional ByVal v_vStartDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        m_oDatabase.Parameters.Clear()
        'developer guide no. 98
        m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_status_type_id", vValue:=v_lRenewalStatusTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="compare_Date", vValue:=v_dtCompareDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        'didn't do cdate(v_vStartDate) because vb is too stupid to know that the first part  of the IF expression is true

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Start_Date", vValue:=If(Informations.IsNothing(v_vStartDate) Or Convert.IsDBNull(v_vStartDate) Or Informations.IsNothing(v_vStartDate), DBNull.Value, v_vStartDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            result = m_oDatabase.SQLAction(sSQL:=ACDelRenewalStatusSQL, sSQLName:=ACDelRenewalStatusName, bStoredProcedure:=ACDelRenewalStatusStored)
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdRenewalPolicies (Public)
    '
    ' Description: set insurance_file_status_id = Null (Live). Policies which are within date range
    '                    specified by Product table and Status on the Renewal_Status table = v_sStatus
    '                    Parent function will have to control the transaction modes
    'History: 02/09/2000 Created (TN)
    '
    'Thinh Nguyen 13/02/2004 add optional start date
    ' ***************************************************************** '
    Private Function UpdRenewalPolicies(ByVal v_lRenewalStatusTypeID As Integer, ByVal v_dtCompareDate As Date, Optional ByVal v_vStartDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_status_type_id", vValue:=v_lRenewalStatusTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="compare_Date", vValue:=v_dtCompareDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        'didn't do cdate(v_vStartDate) because vb is too stupid to know that the first part  of the IF expression is true

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Start_Date", vValue:=If(Informations.IsNothing(v_vStartDate) Or Convert.IsDBNull(v_vStartDate) Or Informations.IsNothing(v_vStartDate), DBNull.Value, v_vStartDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            result = m_oDatabase.SQLAction(sSQL:=ACUpdRenewalPolicySQL, sSQLName:=ACUpdRenewalPolicyName, bStoredProcedure:=ACUpdRenewalPolicyStored)
        End If

        Return result

    End Function

    ' private Methods (End)

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


    ' ***************************************************************** '
    '
    ' Name: CreateBusinessObject
    '
    ' Description:
    '
    ' History: 16/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CreateBusinessObject(ByRef r_oObject As Object, ByVal v_sClassName As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Return gPMComponentServices.CreateBusinessObject(r_oObject:=r_oObject, v_sClassName:=v_sClassName, v_sCallingAppName:=m_sCallingAppName, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

    End Function

    ' ***************************************************************** '
    ' Name: GisIndexLink (Private)
    '
    ' Desc: Index link risk details
    '
    ' ***************************************************************** '
    Public Function GisIndexLink(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskID As Integer, ByVal v_vGisScreenID As Object, ByVal v_dtEffectiveDate As Date, ByVal v_sGisDataModelCode As String) As Integer

        Dim result As Integer = 0
        Const ACFieldPosDataModel As Integer = 0
        Const ACFieldPosObjectName As Integer = 1
        Const ACFieldPosPropertyName As Integer = 2
        Const ACFieldPosIndexLinkingID As Integer = 3

        Dim vOIKeyArray As Object
        Dim sParentOIKey, sParentObjectName As String

        Dim vIndexLinkArray(,) As Object
        Dim vCurrentValue As Object 'current value which needs to apply index linking to
        Dim dNewValue As Double 'value after apply index linking on vCurrentValue
        Dim vPercentage As Object 'percentage to apply to vCurrentValue
        Dim lReturn As gPMConstants.PMEReturnCode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get all fields and objects that needs index linking
            If GetGisIndexLink(v_vGisScreenID:=v_vGisScreenID, r_vResultArray:=vIndexLinkArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'do we have any data
            If Not Informations.IsArray(vIndexLinkArray) Then
                Return result
            End If

            'This has already been done.
            '    'load gis object into memory
            '    If m_oGIS.LoadFromDB(v_sGISDataModelCode:=Trim$(vIndexLinkArray(ACFieldPosDataModel, 0)), _
            ''                                    r_vInsuranceFileCnt:=v_lInsuranceFileCnt, _
            ''                                    r_vRiskID:=v_lRiskID, _
            ''                                    r_vPolicyLinkId:=vPolicyLinkID) <> PMTrue Then
            '
            '        GisIndexLink = PMFalse
            '        Exit Function
            '
            '    End If

            'store parent object name to local variable

            sParentObjectName = CStr(vIndexLinkArray(ACFieldPosDataModel, 0)).Trim() & "_Policy_Binder"

            ' Get the Top Level OI Key (Policy_Binder)
            'RWH(22/02/2001) changed from m_oGIS to m_oDataSet.

            If m_oDataSet.GetAllOIKey(v_sObjectName:=sParentObjectName, r_vOIKeyArray:=vOIKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'do we have any data
            If Not Informations.IsArray(vOIKeyArray) Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No OIKey for Policy_Binder", vApp:=ACApp, vClass:=ACClass, vMethod:="GisIndexLink", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

            End If

            'store top level OIKey to local variable (there should be only one record)

            sParentOIKey = CStr(vOIKeyArray(vOIKeyArray.GetUpperBound(0)))

            'loop thro each record and apply index linking

            For lCount As Integer = 0 To vIndexLinkArray.GetUpperBound(1)

                'reset array to use for child object

                'developer guide no. 101
                vOIKeyArray = Nothing

                'get OIKey for child object
                'RWH(22/02/2001) changed from m_oGIS to m_oDataSet.

                If m_oDataSet.GetChildOIKey(v_sParentObjectName:=sParentObjectName, v_sParentOIKey:=sParentOIKey, v_sChildObjectName:=CStr(vIndexLinkArray(ACFieldPosObjectName, lCount)), r_vChildOIKeyArray:=vOIKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return gPMConstants.PMEReturnCode.PMFalse

                End If

                'do we have any data
                If Informations.IsArray(vOIKeyArray) Then
                    'loop thro and process each record

                    For lRecord As Integer = 0 To vOIKeyArray.GetUpperBound(0)

                        'get current value to apply index linking on
                        'RWH(22/02/2001) changed from m_oGIS to m_oDataSet.

                        'developer guide no. 98
                        lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=vIndexLinkArray(ACFieldPosObjectName, lCount), v_sPropertyName:=vIndexLinkArray(ACFieldPosPropertyName, lCount), v_sOIKey:=vOIKeyArray(lRecord), r_vPropertyValue:=vCurrentValue, r_bIsAssumedInfo:=False)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse

                        End If

                        'only apply index linking if current value is not null or zero

                        If Not (Convert.IsDBNull(vCurrentValue) Or Informations.IsNothing(vCurrentValue) Or vCurrentValue = "") Then

                            If ToSafeDouble(vCurrentValue) <> 0.0# Then

                                'get index link value to be applied using vIndexLinkingID

                                'developer guide no. 101
                                If GetIndexLinkDetail(v_lIndexLinkID:=CInt(vIndexLinkArray(ACFieldPosIndexLinkingID, lCount)), v_dtEffectiveDate:=v_dtEffectiveDate, r_vPercentage:=vPercentage) <> gPMConstants.PMEReturnCode.PMTrue Then

                                    Return gPMConstants.PMEReturnCode.PMFalse

                                End If

                                'only apply index linking if index linking value is not null or zero

                                If Not (Convert.IsDBNull(vPercentage) Or Informations.IsNothing(vPercentage)) Then

                                    If ToSafeDouble(vPercentage) <> 0.0# Then
                                        'calculate new value

                                        dNewValue = CDbl(vCurrentValue) * (1 + (CDbl(vPercentage) / 100))

                                        'Thinh Nguyen 19/04/2002 (start) - round to nearest whole number
                                        dNewValue = Math.Floor(dNewValue + 0.5)
                                        'Thinh Nguyen 19/04/2002 (end) - round to nearest whole number
                                        'store new value to GIS object
                                        'RWH(22/02/2001) changed from m_oGIS to m_oDataSet.

                                        'developer guide no. 98
                                        lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=vIndexLinkArray(ACFieldPosObjectName, lCount), v_sPropertyName:=vIndexLinkArray(ACFieldPosPropertyName, lCount), v_sOIKey:=vOIKeyArray(lRecord), v_vPropertyValue:=dNewValue, v_bIsAssumedInfo:=False)
                                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse

                                        End If

                                    End If

                                End If

                            End If
                        End If

                    Next lRecord

                End If

            Next lCount

            'save GIS objects
            '    If m_oGIS.SaveToDB() <> PMTrue Then
            If GIS_SaveToDB(v_sGisDataModelCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GisIndexLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GisIndexLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Checks all Renewal Criteria for a policy.
    ''' </summary>
    ''' <param name="vRenewalList"></param>
    ''' <param name="lCount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function CheckRenewalCriteria(ByRef vRenewalList(,) As Object, ByRef lCount As Integer) As Integer
        Return CheckRenewalCriteria(vRenewalList:=vRenewalList, lCount:=lCount, isOnlyAgentTransfer:=False)
    End Function

    Public Function CheckRenewalCriteria(ByRef vRenewalList(,) As Object, ByRef lCount As Integer,
                                         ByRef isOnlyAgentTransfer As Boolean) As Integer

        Dim nResult As Integer
        Dim lCheckForClaim As gPMConstants.PMEReturnCode 'set to PMTrue if there are no claims
        Dim nRenewalStatusTypeID As Integer = 0 'renewal status to go on the Renewal Status table
        Dim sFailureCriterion As String = String.Empty
        Dim sFailureDetail As String = String.Empty
        Dim nInsuranceFileCnt As Integer = 0
        Dim nInsuranceFolderCnt As Integer = 0
        Dim sBlackListReasonDesc As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oArray(,) As Object
        Dim bis_ValidProduct As Boolean

        Const PMNumberOfRenewalCriteria As Integer = 10

        Const PMRenCritAutoRenewalSet As Integer = 8
        Const PMRenCritPartyRenewalStop As Integer = 2
        Const PMRenCritPolicyRenewalStop As Integer = 3
        Const PMRenCritReferredAtRenewal As Integer = 4
        Const PMRenCritClaims As Integer = 5
        Const PMRenCritAgentRenewalStop As Integer = 6
        Const PMRenCritClosedBranch As Integer = 7
        Const PMRenCritAgentTransferred As Integer = 1
        Const PMRenCritBlackListedClient As Integer = 9
        Const PMRenCritInvalidProduct As Integer = 10

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            nInsuranceFolderCnt = CInt(vRenewalList(PMFieldPosInsuranceFolderCnt, lCount))

            nInsuranceFileCnt = CInt(vRenewalList(PMFieldPosInsuranceFileCnt, lCount))

            m_lReturn = getAllLiveVersions(nInsuranceFolderCnt, nInsuranceFileCnt, oArray)

            If Informations.IsArray(oArray) Then

                For i As Integer = 0 To oArray.GetUpperBound(1)
                    'filter out canceled versions

                    If CStr(oArray(1, i)).Trim() <> "MTACAN" Then

                        lCheckForClaim = CType(CheckForClaim(v_lInsuranceFileCnt:=CInt(oArray(0, i))),
                                               gPMConstants.PMEReturnCode)

                        If _
                            lCheckForClaim = gPMConstants.PMEReturnCode.PMFalse OrElse CStr(oArray(1, i)).Trim() = "POLICY" Or
                            CStr(oArray(1, i)).Trim() = "RENEWAL" Then
                            'Don't need to check anything before NB or Renewed version or if claimCheck failed
                            Exit For
                        End If
                    End If
                Next i
            Else
                'check for claims (WHAT HAPPEN IF ITS FAILED/ERRORED ????????)
                lCheckForClaim = CType(CheckForClaim(v_lInsuranceFileCnt:=nInsuranceFileCnt),
                                       gPMConstants.PMEReturnCode)
            End If

            'Check each renewal failure criterion.
            For iRenewalCriterion As Integer = 1 To PMNumberOfRenewalCriteria
                'For iRenewalCriterion As Integer = 0 To PMNumberOfRenewalCriteria - 1
                sFailureCriterion = ""
                sFailureDetail = ""
                Select Case (iRenewalCriterion)
                    Case PMRenCritAutoRenewalSet

                        If CDbl(vRenewalList(PMFieldPosIsAutoRenewable, lCount)) = 0 Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMAutoRenewalDesc
                            isOnlyAgentTransfer = False
                        End If

                    Case PMRenCritPartyRenewalStop

                        If _
                            (Not _
                             (Convert.IsDBNull(vRenewalList(PMFieldPosClientStopReason, lCount)) OrElse
                              Informations.IsNothing(vRenewalList(PMFieldPosClientStopReason, lCount)))) AndAlso
                            (CStr(vRenewalList(PMFieldPosClientStopReason, lCount)) <> "") Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMPartyRenewalStopDesc

                            sFailureDetail = CStr(vRenewalList(PMFieldPosClientStopReason, lCount))
                            isOnlyAgentTransfer = False
                        End If

                    Case PMRenCritPolicyRenewalStop

                        If _
                            (Not _
                             (Convert.IsDBNull(vRenewalList(PMFieldPosPolicyStopReason, lCount)) OrElse
                              Informations.IsNothing(vRenewalList(PMFieldPosPolicyStopReason, lCount)))) AndAlso
                            (CStr(vRenewalList(PMFieldPosPolicyStopReason, lCount)) <> "") Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMPolicyRenewalStopDesc

                            sFailureDetail = CStr(vRenewalList(PMFieldPosPolicyStopReason, lCount))
                            isOnlyAgentTransfer = False
                        End If

                    Case PMRenCritReferredAtRenewal

                        If CDbl(vRenewalList(PMFieldPosReferredAtRenewal, lCount)) <> 0 Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMReferredAtRenewalDesc
                            isOnlyAgentTransfer = False
                        End If

                    Case PMRenCritClaims
                        If lCheckForClaim <> gPMConstants.PMEReturnCode.PMTrue Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMClaimsMadeDesc
                            isOnlyAgentTransfer = False
                        End If

                    Case PMRenCritAgentRenewalStop

                        If _
                            (Not _
                             (Convert.IsDBNull(vRenewalList(PMFieldPosAgentStopReason, lCount)) OrElse
                              Informations.IsNothing(vRenewalList(PMFieldPosAgentStopReason, lCount)))) AndAlso
                            (CStr(vRenewalList(PMFieldPosAgentStopReason, lCount)) <> "") Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMAgentRenewalStopDesc

                            sFailureDetail = CStr(vRenewalList(PMFieldPosAgentStopReason, lCount))
                            isOnlyAgentTransfer = False
                        End If

                    Case PMRenCritClosedBranch

                        If CStr(vRenewalList(PMFieldPosClosedBranch, lCount)) = "1" Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMClosedBranchDesc
                            isOnlyAgentTransfer = False
                        End If

                    Case PMRenCritAgentTransferred
                        If gPMFunctions.ToSafeInteger(vRenewalList(PMFieldPosAgentInTransfer, lCount), 0) = 1 Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMAgentTransferred
                            isOnlyAgentTransfer = True
                        End If

                    Case PMRenCritBlackListedClient
                        sBlackListReasonDesc = ""

                        lReturn = CType(GetClientBlacklistReason(v_lInsuranceFileCnt:=nInsuranceFileCnt,
                                                                 r_sBlacklistReasonDesc:=sBlackListReasonDesc),
                                        gPMConstants.PMEReturnCode)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = "BlackListed Client Reason Selection Process Failed"
                            isOnlyAgentTransfer = False
                        Else
                            If sBlackListReasonDesc <> "" Then
                                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                                sFailureCriterion = "BlackListed Client:" & sBlackListReasonDesc
                                isOnlyAgentTransfer = False
                            End If
                        End If

                    Case PMRenCritInvalidProduct

                        lReturn = CType(CheckRenewalProduct(nInsuranceFileCnt, bis_ValidProduct),
                                        gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = "Failed to get Renewal Product"
                            isOnlyAgentTransfer = False
                        ElseIf Not bis_ValidProduct Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = "Invalid Renewal Product"
                            isOnlyAgentTransfer = False
                            'Set the Renewal product to blank if its an invalid product
                            lReturn = CType(UpdateRenewalProduct(v_lInsuranceFileCnt:=nInsuranceFileCnt,
                                                                 v_sProductCode:=""),
                                            gPMConstants.PMEReturnCode)
                        End If
                End Select

                If nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    m_sFailureCriterion = sFailureCriterion
                    ' Add to Renewal_Report table for each renewal criterion the policy fails on.

                    m_lReturn =
                        AddRenewalReport(
                            v_sReportType:=
                                            If(nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAutoRated,
                                                "AutoRenewal", "ManualRenewal"),
                            v_vClientName:=vRenewalList(PMFieldPosClientName, lCount),
                            v_vPolicyNumber:=vRenewalList(PMFieldPosInsuranceRef, lCount),
                            v_vAgentCode:=vRenewalList(PMFieldPosAgentName, lCount),
                            v_vCoverStartDate:=vRenewalList(PMFieldPosCoverStartDate, lCount),
                            v_vCoverEndDate:=vRenewalList(PMFieldPosCoverEndDate, lCount),
                            v_vProductCode:=vRenewalList(PMFieldPosProductCode, lCount),
                            v_vFailureCriterion:=sFailureCriterion, v_vFailureDetail:=sFailureDetail,
                            v_vInsuranceFileCnt:=CStr(vRenewalList(PMFieldPosInsuranceFileCnt, lCount)))
                End If

                nRenewalStatusTypeID = 0

            Next iRenewalCriterion

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckRenewalCriteria Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRenewalCriteria", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' Call GIS NBQuote routine and associated setup. This
    '''  basically duplicates the functionality of iGIS.NBQuote.
    ''' </summary>
    ''' <param name="v_sGisDataModelCode"></param>
    ''' <param name="v_lQuoteType"></param>
    ''' <param name="r_sXMLDataSet"></param>
    ''' <param name="r_sXMLDataSetDef"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GIS_NBQuote(ByRef v_sGisDataModelCode As String, ByRef v_lQuoteType As Integer,
                                ByRef r_sXMLDataSet As String, ByRef r_sXMLDataSetDef As String) As Integer

        Dim nResult As Integer
        Dim nReturn As Integer = 0
        Dim sDataModelCode As String = ""
        Dim oGIS As bGIS.Application

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            oGIS = New bGIS.Application

            nReturn = oGIS.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)


            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = oGIS.SetProcessModes(vTask:=m_iTask, vTransactionType:="REN")

            ' RFC300300 - Clear all Quote Output that may already exist
            ' as there is no need to Pass it back across the network.
            nReturn = m_oDataSet.ClearAllQuoteOutput()
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Data as an XML String
            nReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataSet:=r_sXMLDataSet)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    sDataModelCode = m_oDataSet.GISDataModelCode
            m_lReturn = oGIS.SetProcessModes(vTask:=PMConst.PMAdd,
                                               vTransactionType:="REN")
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GIS_NBQuote = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            'Call NBQuote on bGIS.

            m_lReturn = oGIS.NBQuote(v_sGisDataModelCode:=v_sGisDataModelCode, v_lQuoteType:=v_lQuoteType,
                                     v_sGisBusinessTypeCode:="NB", v_dtEffectiveDate:=DateTime.Today,
                                     r_sXMLDataset:=r_sXMLDataSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load the NBQuote Results
            nReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=r_sXMLDataSetDef, v_sXMLDataSet:=r_sXMLDataSet)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'destroy gis object
            If Not (oGIS Is Nothing) Then
                'UPGRADE_TODO: (1067) Member Terminate is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                oGIS.Dispose()
                oGIS = Nothing
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GIS_NBQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GIS_NBQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    '''  Duplicates functionality of iGIS.SaveToDB so we can
    '''              call straight thru' to the business object.
    ''' </summary>
    ''' <param name="v_sGisDataModelCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GIS_SaveToDB(ByVal v_sGisDataModelCode As String) As Integer

        Dim nResult As Integer
        Dim sXMLDataSetDef As String = String.Empty
        Dim sXMLDataSet As String = String.Empty
        Dim nReturn As Integer = 0
        Dim oGIS As bGIS.Application

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            oGIS = New bGIS.Application

            nReturn = oGIS.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Data as an XML String
            nReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = oGIS.SetProcessModes(vTask:=m_iTask, vTransactionType:="REN")

            ' Save it to the DataBase

            nReturn = oGIS.SaveToDB(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataset:=sXMLDataSet)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load the Saved to DB Results
            nReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'destroy gis object
            If Not (oGIS Is Nothing) Then
                'UPGRADE_TODO: (1067) Member Terminate is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                oGIS.Dispose()
                oGIS = Nothing
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GIS_SaveToDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GIS_SaveToDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteInsuranceFileSystem
    '
    ' Description:
    '
    ' History: 21/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function DeleteInsuranceFileSystem(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            result = m_oDatabase.SQLAction(sSQL:=ACDelInsFileSystemSQL, sSQLName:=ACDelInsFileSystemName, bStoredProcedure:=ACDelInsFileSystemStored)
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteInsuranceFile
    '
    ' Description:
    '
    ' History: 21/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function DeleteInsuranceFile(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            result = m_oDatabase.SQLAction(sSQL:=ACDelInsFileSQL, sSQLName:=ACDelInsFileName, bStoredProcedure:=ACDelInsFileStored)
        End If

        Return result

    End Function

    ''' <summary>
    ''' LoadFromDB
    ''' </summary>
    ''' <param name="v_sGisDataModelCode"></param>

    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GIS_LoadFromDB(ByVal v_sGisDataModelCode As String) As Integer
        Return GIS_LoadFromDB(v_sGisDataModelCode:=v_sGisDataModelCode,
                               r_vInsuranceFileCnt:=Nothing,
                               r_vPolicyLinkID:="",
                               r_vRiskID:=Nothing)
    End Function

    Public Function GIS_LoadFromDB(ByVal v_sGisDataModelCode As String,
                           ByRef r_vInsuranceFileCnt As Object) As Integer
        Return GIS_LoadFromDB(v_sGisDataModelCode:=v_sGisDataModelCode,
                               r_vInsuranceFileCnt:=r_vInsuranceFileCnt,
                               r_vPolicyLinkID:="",
                               r_vRiskID:=Nothing)
    End Function

    Public Function GIS_LoadFromDB(ByVal v_sGisDataModelCode As String,
                           ByRef r_vInsuranceFileCnt As Object,
                           ByRef r_vPolicyLinkID As String) As Integer
        Return GIS_LoadFromDB(v_sGisDataModelCode:=v_sGisDataModelCode,
                               r_vInsuranceFileCnt:=r_vInsuranceFileCnt,
                               r_vPolicyLinkID:=r_vPolicyLinkID,
                               r_vRiskID:=Nothing)
    End Function

    Public Function GIS_LoadFromDB(ByVal v_sGisDataModelCode As String,
                                   ByRef r_vInsuranceFileCnt As Object,
                                   ByRef r_vPolicyLinkID As String,
                                   ByRef r_vRiskID As Object) As Integer

        Dim nResult As Integer
        Dim nReturn As Integer = 0
        Dim sXMLDataSet As String = String.Empty
        Dim sXMLDataSetDef As String = String.Empty
        Dim oGIS As bGIS.Application

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            oGIS = New bGIS.Application

            nReturn = oGIS.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Get the Data From the Database in XML
            ' RFC160300 - DataModel code param added to LoadFromDB call.
            m_lReturn = oGIS.SetProcessModes(vTask:=m_iTask, vTransactionType:="REN")
            nReturn = oGIS.LoadFromDB(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet,
                                      v_sGisDataModelCode:=v_sGisDataModelCode,
                                      r_vInsuranceFileCnt:=r_vInsuranceFileCnt, r_vPolicyLinkID:=CInt(r_vPolicyLinkID),
                                      r_vRiskID:=r_vRiskID)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nReturn
            End If

            ' Load Data as XML
            nReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nReturn
            End If

            'destroy gis object
            If Not (oGIS Is Nothing) Then
                'UPGRADE_TODO: (1067) Member Terminate is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                oGIS.Dispose()
                oGIS = Nothing
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LockProductForRenewal
    '
    ' Description: Need to check if requested product or ALL products are
    '               locked. If requesting lock of ALL products then report
    '               which products are locked.
    '
    ' History: 22/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function LockProductForRenewal(ByVal v_lProductId As Integer, ByRef r_sLockedBy As String) As Integer

        Dim result As Integer = 0
        Dim sLockedBy As String = ""
        Dim sSQL As New StringBuilder
        Dim vResultArray(,) As Object = Nothing
        Dim vProductArray(,) As Object
        Dim oPMLock As bpmlock.User

        Const ACPosProductId As Integer = 0
        Const ACPosUser As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Retrieve all Renewal locks.
            sSQL = New StringBuilder("SELECT l.lock_value, u.username, Null" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("FROM PMLock l, PMUser u" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("WHERE UPPER(l.lock_name) = 'RENEWAL'" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("AND l.locked_by_id = u.user_id")

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GetRenewalLocks", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = New StringBuilder("")
            'See if our required product is locked.

            r_sLockedBy = ""

            'Do locks already exist ?
            If Informations.IsArray(vResultArray) Then

                'Loop round existing locks.

                For iCount As Integer = 0 To vResultArray.GetUpperBound(1)

                    'Is this lock for ALL products. If so, assume is only one and
                    'inform user all products locked by this user.

                    If CDbl(vResultArray(ACPosProductId, iCount)) = 0 Then

                        r_sLockedBy = "ALL products locked for Renewal by '" & CStr(vResultArray(ACPosUser, iCount)) & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                        Exit For
                    End If

                    'If this lock is for the product we are attempting to lock OR we are
                    'attempting to lock all products then retrieve this description.

                    If (v_lProductId = 0) Or (v_lProductId = CDbl(vResultArray(ACPosProductId, iCount))) Then
                        sSQL = New StringBuilder("SELECT description" & Strings.ChrW(13) & Strings.ChrW(10))
                        sSQL.Append("FROM Product" & Strings.ChrW(13) & Strings.ChrW(10))

                        sSQL.Append("WHERE product_id = " & CInt(vResultArray(0, iCount)))

                        ' Execute SQL Statement
                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GetProduct", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vProductArray)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If Informations.IsArray(vProductArray) Then

                            vResultArray(2, iCount) = vProductArray(0, 0)

                            r_sLockedBy = r_sLockedBy & "Product " & "'" & CStr(vProductArray(0, 0)) & "'" & " locked for Renewal by '" & CStr(vResultArray(ACPosUser, iCount)) & " '" & Strings.ChrW(13) & Strings.ChrW(10)
                        End If

                        'If we are not attempting to lock all products then
                        'this was the only one we were interested in.
                        If v_lProductId <> 0 Then
                            Exit For
                        End If
                    End If
                Next iCount
            End If

            If r_sLockedBy <> "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Lock the requested product.
            'Get bPMLock
            oPMLock = New bpmlock.User
            m_lReturn = oPMLock.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="LockProductForRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            m_lReturn = oPMLock.LockKey(sKeyName:="renewal", vKeyValue:=v_lProductId, iUserID:=m_iUserID, sCurrentlyLockedBy:=sLockedBy)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    result = gPMConstants.PMEReturnCode.PMFalse
                    If sLockedBy = "ERROR" Then
                        ' Log Error.
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockProductForRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    Else
                        ' Log Error.
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Product locked by """ & sLockedBy & """", vApp:=ACApp, vClass:=ACClass, vMethod:="LockProductForRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to lock the product", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDataModel", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result

            End Select
            oPMLock.Dispose()
            oPMLock = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockProductForRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockDataModel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckOutputTable
    '
    ' Description:
    '
    ' History: 23/02/2001 RWH - Created.
    '
    ' Thinh Nguyen 05/Nov/2004 - we want all the reasons plus the one thats passed
    ' ***************************************************************** '
    Public Function CheckOutputTable(ByVal v_sDataModelCode As String, ByVal v_lPolicyBinderId As Integer, ByRef r_sReasons As String) As Integer

        Dim result As Integer = 0
        Dim sOutputTable, sPolicyBinderIdName, sOutputIdName, sSQL As String
        Dim vResultArray(,) As Object = Nothing

        ' Const ACPosOutputId As Integer = 0
        Const ACPosDeclineReason As Integer = 1
        Const ACPosReferReason As Integer = 2

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_sDataModelCode = v_sDataModelCode.Trim()

            If v_sDataModelCode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Data Model passed in", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckOutputTable", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Build up table & id names using DataModelCode.
            sOutputTable = v_sDataModelCode & "_Output"
            sOutputIdName = sOutputTable & "_id"
            sPolicyBinderIdName = v_sDataModelCode & "_Policy_binder_id"

            sSQL = "SELECT " & sOutputIdName & ", Decline_reason, Refer_reason" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM " & sOutputTable & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE " & sPolicyBinderIdName & " = " & CStr(v_lPolicyBinderId)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetOutputFromRating", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'put in new line if we already have failure reasons
            If r_sReasons <> "" Then
                r_sReasons = r_sReasons & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            'Check to see if failure reasons exist. We are only interested in whether
            'they exist or not so just report on first one found.
            If Informations.IsArray(vResultArray) Then

                For iCount As Integer = 0 To vResultArray.GetUpperBound(1)

                    If CStr(vResultArray(ACPosDeclineReason, iCount)).Trim() <> "" Then

                        r_sReasons = r_sReasons & "DECLINED - " & CStr(vResultArray(ACPosDeclineReason, iCount)).Trim() & Strings.ChrW(13) & Strings.ChrW(10)
                    ElseIf (CStr(vResultArray(ACPosReferReason, iCount)).Trim() <> "") Then

                        r_sReasons = r_sReasons & "REFERRED - " & CStr(vResultArray(ACPosReferReason, iCount)).Trim() & Strings.ChrW(13) & Strings.ChrW(10)
                    End If
                Next iCount
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckOutputTable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckOutputTable", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteOutputTable
    '
    ' Description:
    '
    ' History: 26/05/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteOutputTable(ByVal v_sDataModelCode As String, ByVal v_lPolicyBinderId As Integer) As Integer

        Dim result As Integer = 0
        Dim sOutputTable, sPolicyBinderIdName, sOutputIdName, sSQL As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_sDataModelCode = v_sDataModelCode.Trim()

            If v_sDataModelCode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Data Model passed in", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteOutputTable", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Build up table & id names using DataModelCode.
            sOutputTable = v_sDataModelCode & "_Output"
            sOutputIdName = sOutputTable & "_id"
            sPolicyBinderIdName = v_sDataModelCode & "_Policy_binder_id"

            sSQL = "DELETE " & sOutputTable & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE " & sPolicyBinderIdName & " = " & CStr(v_lPolicyBinderId)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DeleteOutputTable", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteOutputTable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteOutputTable", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPolicyBinderId
    '
    ' Description:
    '
    ' History: 23/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetPolicyBinderId(ByVal v_sDataModelCode As String, ByVal v_lGISPolicyLinkId As Integer, ByRef r_lPolicyBinderId As Integer) As Integer

        Dim result As Integer = 0
        Dim sPolicyBinderTable, sIdName, sSQL As String
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_sDataModelCode = v_sDataModelCode.Trim()

            If v_sDataModelCode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Data Model passed in", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyBinderId", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Build up table & id names using DataModelCode.
            sPolicyBinderTable = v_sDataModelCode & "_Policy_binder"
            sIdName = sPolicyBinderTable & "_id"

            sSQL = "SELECT " & sIdName & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM " & sPolicyBinderTable & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE gis_policy_link_id = " & CStr(v_lGISPolicyLinkId)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPolicyBinderId", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                r_lPolicyBinderId = CInt(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyBinderId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyBinderId", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyRiskStandardWordings
    '
    ' Description:
    '
    ' History: 23/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function CopyRiskStandardWordings(ByVal v_lOldPolicyBinderId As Integer, ByVal v_lNewPolicyBinderId As Integer, ByVal v_sDataModelCode As String, Optional ByVal v_dtEffectiveDate As Date = #12/30/1899#) As Integer

        Dim result As Integer = 0
        Dim sWordingTable, sPolicyBinderIdName As String
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""
        Dim oDocTemplate As bSIRDocTemplate.Business
        Dim lDocumentTemplateID As Integer
        Dim nNewDocumentTemplateId As Integer

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

                    If v_dtEffectiveDate <> #12/30/1899# Then
                        m_lReturn = GetLatestVersionOfDocumentTemplate(v_nDocTemplateId:=lDocumentTemplateID, v_dtEffectiveDate:=v_dtEffectiveDate, r_nLatestVersionDocTemplateId:=nNewDocumentTemplateId)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(m_lReturn.ToString() + ", " + +", Select statement failed.")
                        End If
                    End If

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

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="new_doc_template_id", vValue:=CStr(nNewDocumentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
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

    ' ***************************************************************** '
    '
    ' Name: CopyPolicyStandardWordings
    '
    ' Description:
    '
    ' History: 23/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function CopyPolicyStandardWordings(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, Optional ByVal v_dtEffectiveDate As Date = #12/30/1899#) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""

        Const ACPosWordingId As Integer = 0
        Const ACPosDocId As Integer = 1
        Const kPosDoNotMerge As Integer = 2
        Dim nDocumentTemplateID As Integer
        Dim nNewDocumentTemplateId As Integer
        Dim nDoNotMerge As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT policy_standard_wording_id, document_template_id,do_not_merge" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM policy_standard_wording" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE insurance_file_cnt = " & CStr(v_lOldInsuranceFileCnt)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPolicyStandardWordings", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                For iCount As Integer = 0 To vResultArray.GetUpperBound(1)

                    'get all versions of this policy which are in renewal
                    nDocumentTemplateID = vResultArray(ACPosDocId, iCount)
                    If v_dtEffectiveDate <> #12/30/1899# Then
                        m_lReturn = GetLatestVersionOfDocumentTemplate(v_nDocTemplateId:=nDocumentTemplateID, v_dtEffectiveDate:=v_dtEffectiveDate, r_nLatestVersionDocTemplateId:=nNewDocumentTemplateId)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        nDocumentTemplateID = nNewDocumentTemplateId
                    End If
                    nDoNotMerge = ToSafeInteger(vResultArray(kPosDoNotMerge, iCount))
                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_standard_wording_id", vValue:=CStr(vResultArray(ACPosWordingId, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(nDocumentTemplateID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="do_not_merge", vValue:=CStr(nDoNotMerge), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'parameters are added sucessfully
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertPolicyWordingSQL, sSQLName:=ACInsertPolicyWordingName, bStoredProcedure:=ACInsertPolicyWordingStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next iCount
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicyStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyStandardWordings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRenewalsForPolicy
    '
    ' Description:
    '
    ' History: 15/05/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetRenewalsForPolicy(ByRef v_lInsFolderCnt As Integer, ByRef r_bRenewalsExist As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get all versions of this policy which are in renewal
            m_lReturn = GetRenewalVersion(v_lInsuranceFolderCnt:=v_lInsFolderCnt, r_vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed To Get Renewal Version Of Policy", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewalPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then
                r_bRenewalsExist = True

                m_vRenewalsForPolicy = vResultArray
            Else
                r_bRenewalsExist = False
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewalsForPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalsForPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteRenewalsForPolicy
    '
    ' Description:
    '
    ' History: 15/05/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteRenewalsForPolicy() As Integer

        Dim nResult As Integer = 0
        'Dim oResultArray(,) As Object = Nothing
        'Dim oLatestRenewalArray(,) As Object = Nothing
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If Informations.IsArray(m_vRenewalsForPolicy) Then
                'loop through and delete each renewal policy from renewal
                'm_lReturn = GetPolicyForRenewal(v_lInsuranceFolderCnt:=m_vRenewalsForPolicy(2, 0), _
                '                                                   r_vResultArray:=oResultArray)

                ' m_lReturn = GetLatestPolicyVersion(v_nInsuranceFolderCnt:=m_vRenewalsForPolicy(2, 0), r_oResultArray:=oLatestRenewalArray)
                'If (oResultArray IsNot Nothing AndAlso Informations.IsArray(oResultArray)) Or (oLatestRenewalArray IsNot Nothing AndAlso Informations.IsArray(oLatestRenewalArray)) Then
                For iCount As Integer = 0 To m_vRenewalsForPolicy.GetUpperBound(1)
                    'If m_vRenewalsForPolicy(1, iCount) = oLatestRenewalArray(1, 0) Then
                    m_lReturn = DeleteRenewal(v_lRenewalInsuranceFileCnt:=CInt(m_vRenewalsForPolicy(0, iCount)))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        Exit For
                    End If
                    'End If
                Next
                'Else
                '    'loop through and delete each renewal policy from renewal
                '    For iCount As Integer = 0 To UBound(m_vRenewalsForPolicy, 2)
                '        m_lReturn = DeleteRenewal(v_lRenewalInsuranceFileCnt:=m_vRenewalsForPolicy(0, iCount))

                '        If m_lReturn <> PMEReturnCode.PMTrue Then
                '            nResult = gPMConstants.PMEReturnCode.PMFalse
                '            Exit For
                '        End If
                '    Next

                'End If
            End If
            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRenewalsForPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewalsForPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : GetMidnightRenewal
    '
    ' Desc : do we renew at midnight?
    '
    ' Hist : 23 May 2001
    '
    ' ***************************************************************** '
    Public Function GetMidnightRenewal(ByVal v_lProductId As Integer, ByRef r_bMidnight As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get is_midnight_renewal from the product table
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectIsMidnightRenewalSQL, sSQLName:=ACSelectIsMidnightRenewalName, bStoredProcedure:=ACSelectIsMidnightRenewalStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            r_bMidnight = (CDbl(vResultArray(0, 0)) = 1)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMidnightRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMidnightRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenewalsReportExists
    '
    ' Description: Returns whether any renewals have been entered
    '               into the Renewal_Report table.
    '
    ' History: 08/06/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function RenewalsReportExists(ByVal v_sReportType As String, ByRef r_bExists As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="report_type", vValue:=v_sReportType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACRenewalReportExistsSQL, sSQLName:=ACRenewalReportExistsName, bStoredProcedure:=ACRenewalReportExistsStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_bExists = (Informations.IsArray(vResultArray))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenewalsReportExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalsReportExists", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyCoinsurance
    '
    ' Description:
    '
    ' History: 13/06/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function CopyCoinsurance(ByVal v_lCurrentInsFileCnt As Integer, ByVal v_lNewInsFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=CStr(v_lCurrentInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=CStr(v_lNewInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRenewalCopyCoinsSQL, sSQLName:=ACRenewalCopyCoinsName, bStoredProcedure:=ACRenewalCopyCoinsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyCoinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyCoinsurance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyAgentCommission
    '
    ' Description:
    '
    ' History: 13/06/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function CopyAgentCommission(ByVal v_lCurrentInsFileCnt As Integer, ByVal v_lNewInsFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=CStr(v_lCurrentInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=CStr(v_lNewInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRenewalCopyAgentCommissionSQL, sSQLName:=ACRenewalCopyAgentCommissionName, bStoredProcedure:=ACRenewalCopyAgentCommissionStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyAgentCommission", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyInsurerAgent
    '
    ' Description:
    '
    ' History: 17/08/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function CopyInsuranceFileAgent(ByVal v_lCurrentInsFileCnt As Integer, ByVal v_lNewInsFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=CStr(v_lCurrentInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=CStr(v_lNewInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRenewalCopyInsuranceFileAgentSQL, sSQLName:=ACRenewalCopyInsuranceFileAgentName, bStoredProcedure:=ACRenewalCopyInsuranceFileAgentStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyInsuranceFileAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyInsuranceFileAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteWorkTask
    '
    ' Description: delete task from work manager
    '
    ' History: 13/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteWorkTask(ByVal v_sKeyName As String, ByVal v_sKeyValue As String) As Integer

        Dim result As Integer = 0
        Dim oWrkTaskInstance As bPMWrkTaskInstance.TaskControl

        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'SQL to get back work task instance count
            sSQL = "SELECT wtik.pmwrk_task_instance_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM pmnav_key nk," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "PMWrk_Task_Inst_Key wtik" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE nk.Name = {key_name}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND nk.pmnav_key_id = wtik.pmnav_key_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND wtik.key_value = {key_value}" & Strings.ChrW(13) & Strings.ChrW(10)

            result = m_oDatabase.Parameters.Add(sName:="key_name", vValue:=v_sKeyName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            result = m_oDatabase.Parameters.Add(sName:="key_value", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            result = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskInstanceCount", bStoredProcedure:=False, vResultArray:=vResultArray, bKeepNulls:=True)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return result
            End If

            'yeap we have task(s) - so delete it
            oWrkTaskInstance = New bPMWrkTaskInstance.TaskControl
            result = oWrkTaskInstance.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'loop thro and delete all tasks for this key_value

            For lCount As Integer = 0 To vResultArray.GetUpperBound(1)

                'set status to complete otherwise it won't delete it

                result = oWrkTaskInstance.SetStatusComplete(v_lPMWrkTaskInstanceCnt:=CInt(vResultArray(0, lCount)))

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit For
                End If

                'delete work task

                result = oWrkTaskInstance.Delete(v_lPMWrkTaskInstanceCnt:=CInt(vResultArray(0, lCount)))

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit For
                End If
            Next

            oWrkTaskInstance.Dispose()

            oWrkTaskInstance = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteWorkTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteWorkTask", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ''' <summary>
    ''' Copy data set
    ''' </summary>
    ''' <param name="v_sDataModelCode"></param>
    ''' <param name="r_lNewGISPolicyLinkId"></param>
    ''' <param name="r_sXMLDataSetDef"></param>
    ''' <param name="r_sXMLDataSet"></param>
    ''' <param name="v_vOldGISPolicyLinkId"></param>
    ''' <param name="v_vOldInsuranceFileCnt"></param>
    ''' <param name="v_vOldRiskID"></param>
    ''' <param name="v_vNewInsuranceFileCnt"></param>
    ''' <param name="v_vNewRiskID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyDataSet(ByVal v_sDataModelCode As String, ByRef r_lNewGISPolicyLinkId As Integer,
                                ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataSet As String,
                                ByVal v_vOldGISPolicyLinkId As Object, ByVal v_vOldInsuranceFileCnt As Object,
                                ByVal v_vOldRiskID As Object, ByVal v_vNewInsuranceFileCnt As Object,
                                ByVal v_vNewRiskID As Object) As Integer

        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            Dim oGIS As bGIS.Application
            Dim nReturn As Integer = 0

            oGIS = New bGIS.Application

            nReturn = oGIS.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)


            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = oGIS.SetProcessModes(vTask:=m_iTask, vTransactionType:="REN")
            nResult = oGIS.CopyDataSet(v_sDataModelCode:=v_sDataModelCode,
                                       r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkId,
                                       r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataSet,
                                       v_vOldGISPolicyLinkId:=v_vOldGISPolicyLinkId,
                                       v_vOldInsuranceFileCnt:=v_vOldInsuranceFileCnt, v_vOldXMLDataSet:="",
                                       v_vNewInsuranceFileCnt:=v_vNewInsuranceFileCnt, v_vOldRiskID:=v_vOldRiskID,
                                       v_vNewRiskID:=v_vNewRiskID, v_vCopyQuotes:=False)

            'destroy gis object
            If Not (oGIS Is Nothing) Then
                'UPGRADE_TODO: (1067) Member Terminate is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                oGIS.Dispose()
                oGIS = Nothing
            End If
            Return nResult
        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyDataSet Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDataSet", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LoadFromXML
    '
    ' Description:
    '
    ' History: 24/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function LoadFromXML(ByRef v_sXMLDataSetDef As String, ByRef v_sXMLDataSet As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Return m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=v_sXMLDataSetDef, v_sXMLDataSet:=v_sXMLDataSet)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRenewalFailures
    '
    ' Description:
    '
    ' History: 10/10/2002 CJR - Created.
    '
    ' ***************************************************************** '
    Function GetRenewalFailures(ByRef sPolicyNumber As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT failure_criterion, failure_detail FROM renewal_report " & "WHERE (failure_criterion <> '' OR  failure_detail <> '') AND policy_number ='" & sPolicyNumber & "'" & " AND user_id = " & CStr(m_iUserID)

            m_oDatabase.Parameters.Clear()

            Return m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRenewalFailures", bStoredProcedure:=False, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewalFailures Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalFailures", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name : IsPremiumZero
    '
    ' Desc : is premium zero?
    '
    ' Hist : 01 May 2003 Moh Created
    '
    ' ***************************************************************** '
    Public Function IsPremiumZero(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bIsPremiumZero As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get this_premium from the insurance file table
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectIsPremiumZeroSQL, sSQLName:=ACSelectIsPremiumZeroName, bStoredProcedure:=ACSelectIsPremiumZeroStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            If CStr(vResultArray(0, 0)) <> "" Then

                r_bIsPremiumZero = Not (CInt(vResultArray(0, 0)) > 0)
            Else
                r_bIsPremiumZero = True
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsPremiumZero Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsPremiumZero", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : IsAgentCancelled
    '
    ' Desc : set r_lIsCancelled = pmtrue if party is cancel and pmfalse otherwise
    '
    ' Hist : 15/08/2003 Thinh Nguyen - Created
    '
    ' ***************************************************************** '
    Public Function IsAgentCancelled(ByVal v_lPartyCnt As Integer, ByRef r_lIsCancelled As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PartyCnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelIsAgentCancelledSQL, sSQLName:=ACSelIsAgentCancelledName, bStoredProcedure:=ACSelIsAgentCancelledStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'if we have data then agent as been cancelled
            If Informations.IsArray(vResultArray) Then

                If CStr(vResultArray(0, 0)) <> "" Then
                    r_lIsCancelled = gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsAgentCancelled Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsAgentCancelled", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name : GetRenewalFrequencyDetail
    '
    ' Desc : get details from renewal frequency table
    '
    ' Hist : 29/09/2003 Thinh Nguyen - Created
    '
    ' ***************************************************************** '

    Public Function GetRenewalFrequencyDetail(ByVal v_lFrequencyID As Integer, ByRef r_vResult(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT [Code], [Description], [Number_Of_Months] FROM Renewal_Frequency WHERE renewal_frequency_id = " & v_lFrequencyID

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRenewalFrequencyDetails", bStoredProcedure:=False, vResultArray:=r_vResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(r_vResult) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewalFrequencyDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalFrequencyDetail", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '****************************************************************************
    'Check to see if this policy has an instalment plan attached
    '****************************************************************************
    Public Function IsInstalment(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelIsInstalmentSQL, sSQLName:=ACSelIsInstalmentName, bStoredProcedure:=ACSelIsInstalmentStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check if this policy has an instalment plan attached - Insurance_File_Cnt: " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="IsInstalment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally



        End Try
        Return result
    End Function

    Public Function IsInstalmentAndActivePartyBank(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "IsInstalmentAndActivePartyBank"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelIsInstalmentAndActivePartyBankSQL, sSQLName:=ACSelIsInstalmentAndActivePartyBankName, bStoredProcedure:=True, vResultArray:=r_vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally



        End Try
        Return result
    End Function

    '*****************************************************************************
    ' Create quote plan for v_lRenewalInsuranceFileCnt version of the policy
    '*****************************************************************************
    Public Function CreateInstalmentQuote(ByVal v_lOriginalInsuranceFileCnt As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByRef r_sFailureMessage As String) As Integer
        Return CreateInstalmentQuote(v_lOriginalInsuranceFileCnt:=v_lOriginalInsuranceFileCnt, v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lPartyCnt:=v_lPartyCnt, r_sFailureMessage:=r_sFailureMessage, v_lProductId:=0)
    End Function

    Public Function CreateInstalmentQuote(ByVal v_lOriginalInsuranceFileCnt As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByRef r_sFailureMessage As String, ByRef v_lProductId As Integer) As Integer

        Dim result As Integer = 0
        Dim lPremiumFinanceCnt, lPremiumFinanceVer As Integer
        Dim oProduct As bSIRProduct.Business
        Dim lInstalmentInsFileCnt As Integer
        'Dim vUseNbPaymentTermAtRenSelection As Object
        Dim lReturn As Integer
        Dim oPremiumFinance As bSIRPremiumFinance.Business
        Dim vUsePriorTermSchemeAtRenewal(,) As Object

        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            r_sFailureMessage = ""

            oPremiumFinance = New bSIRPremiumFinance.Business
            lReturn = oPremiumFinance.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If
            'now check to see if renewal version has an instalment plan

            Dim sOptionValue As String = "0"
            Dim sPayment_Method As String = ""
            's
            lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=kSystemOptionAutoInstalment, r_sOptionValue:=sOptionValue)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = "bPMFunc.GetSystemOption Failed"
                Return result
            End If
            lReturn = IsInstalment(v_lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt)

            If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                ' Tech Spec PGR 8.8 Renewals ---------------------------------
                oProduct = New bSIRProduct.Business
                lReturn = oProduct.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to Initialize bSIRProduct.Business"
                    Return result
                End If
                'Get current PaymentTerms

                lReturn = oProduct.GetProductValue(v_lProductId:=v_lProductId, v_sColumnName:="use_prior_term_scheme_at_ren", r_vProductArray:=vUsePriorTermSchemeAtRenewal)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "bSIRProduct.GetProductValue Failed"
                    Return result
                End If


                If Informations.IsArray(vUsePriorTermSchemeAtRenewal) Then

                    If ((CDbl(vUsePriorTermSchemeAtRenewal(0, 0)) = 0) Or (sOptionValue.ToString = "1")) Then
                        lInstalmentInsFileCnt = v_lOriginalInsuranceFileCnt ' Change From Spec
                    Else
                        m_lReturn = GetPriorTermSchemeInsuranceFile(lInsuranceFileCnt:=v_lOriginalInsuranceFileCnt,
                                                       lPriorTermSchemeInsuranceFileCnt:=lInstalmentInsFileCnt,
                                                       v_bUsePriorTermSchemeAtRenewal:=vUsePriorTermSchemeAtRenewal(0, 0))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sFailureMessage = "GetPriorTermSchemeInsuranceFile Failed"
                            Exit Function
                        End If
                        oPremiumFinance.UsePriorTermSchemeAtRenewal = True
                    End If
                Else
                    lInstalmentInsFileCnt = v_lOriginalInsuranceFileCnt ' Change From Spec
                End If

                ' End ----------------------------------------------------------

                'create quote plan for renewal version

                m_lReturn = oPremiumFinance.CopyInstalmentPlanForRenewals(v_lOriginalInsuranceFileCnt:=lInstalmentInsFileCnt, v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lPartyCnt:=v_lPartyCnt, r_lPremiumFinanceCnt:=lPremiumFinanceCnt, r_lPremiumFinanceVer:=lPremiumFinanceVer)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to create quote plan for Insurance File Count " & v_lRenewalInsuranceFileCnt
                    Return result
                End If
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = "Failed to check if original policy has instalment plan for Insurance File Count " & v_lRenewalInsuranceFileCnt
                Return result
            End If
            'destroy finance object
            If Not (oPremiumFinance Is Nothing) Then
                'UPGRADE_TODO: (1067) Member Terminate is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                oPremiumFinance.Dispose()
                oPremiumFinance = Nothing
            End If
            'if we get here then everything is cool
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instalment quote", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateInstalmentQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*****************************************************************************
    ' get broker transfer portfolio details
    '*****************************************************************************
    Public Function GetBrokerTransferPortfolioDetail(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sFailMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                sFailMessage = "Failed to add param for (InsuranceFileCnt)"

                Throw New Exception(sFailMessage)

            End If

            If m_oDatabase.SQLSelect(sSQL:=ACGetBrokerTransferPortfolioDetailSQL, sSQLName:=ACGetBrokerTransferPortfolioDetailName, bStoredProcedure:=ACGetBrokerTransferPortfolioDetailStored, vResultArray:=r_vResultArray, bKeepNulls:=False) <> gPMConstants.PMEReturnCode.PMTrue Then
                'm_oDatabase.SQLSelect(sSQL:=ACGetBrokerTransferPortfolioDetailSQL, sSQLName:=ACGetBrokerTransferPortfolioDetailName, bStoredProcedure:=ACGetBrokerTransferPortfolioDetailStored, vResultArray:=r_vResultArray, bKeepNulls:=False)
                'If IsNothing(r_vResultArray) Then
                sFailMessage = "Failed to get details from database"
                'End If

                Throw New Exception(sFailMessage)

            End If


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            If sFailMessage = "" Then
                sFailMessage = "Failed to broker transfer porfolio details"
            End If

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetBrokerTransferPortfolioDetail()", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    ''' <summary>
    ''' CreateRenewalFees
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateRenewalFees(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim nResult As Integer
        Const kMethodName As String = "CreateRenewalFees"
        Dim bReturn As gPMConstants.PMEReturnCode
        Dim oFee As bSIRPartyFee.UBusiness

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' create fee object
            oFee = New bSIRPartyFee.UBusiness
            bReturn = CType(oFee.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase),
                        gPMConstants.PMEReturnCode)
            If bReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create object bSIRPartyFee.UBusiness",
                                        gPMConstants.PMELogLevel.PMLogError)
            End If

            ' create renewal fees

            bReturn = oFee.CreateRenewalFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
            If bReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CreateRenewalFees Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oFee = Nothing


        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: GetBlacklistedClientDetailsForInsuranceFile
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetBlacklistedClientDetailsForInsuranceFile(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBlacklistedClientDetailsForInsuranceFile"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

        ' Execute selection Query
        If m_oDatabase.SQLSelect(sSQL:=kGetBlacklistedClientDetailsForInsuranceFileSQL, sSQLName:=kGetBlacklistedClientDetailsForInsuranceFileName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, kGetBlacklistedClientDetailsForInsuranceFileSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If

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

            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

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
    ' Name: GetClientBlacklistReason
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetClientBlacklistReason(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sBlacklistReasonDesc As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClientBlacklistReason"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vBlacklistReason(,) As Object
        Dim lBlackListReasonId, sBlackListReasonDesc As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' if the underwriting or agency flag isnt set
            If m_sUnderwritingOrAgency = "" Then
                ' get the underwriting / agency option
                lReturn = getUnderwritingOrAgency()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetClientBlacklistReason Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            ' if its underwriting and the client blacklisting in force option is on
            If m_bSystemOptionClientBlacklistingInForce Then

                ' get the blacklisted client details
                lReturn = CType(GetBlacklistedClientDetailsForInsuranceFile(v_lInsuranceFileCnt, r_vResults:=vBlacklistReason), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetBlacklistedClientDetailsForInsuranceFile Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' NB: if its not an array the insured on the insurance file is not a personal client
                ' so blacklisting doesnt apply
                If Informations.IsArray(vBlacklistReason) Then

                    lBlackListReasonId = CStr(vBlacklistReason(0, 0))

                    sBlackListReasonDesc = CStr(vBlacklistReason(1, 0))
                End If

            End If

            ' return the black list reason desc
            r_sBlacklistReasonDesc = sBlackListReasonDesc


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
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'sj 19/06/2002 - start
        m_lReturn = bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMFunc.getUnderwritingOrAgency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingOrAgency")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' **************************************************************************************************
    ' return RI Model type
    ' 0 = standard
    ' 1 = default
    ' 2 = deferred
    ' 3 = exess of loss
    ' -1 = if error
    ' **************************************************************************************************
    Public Function GetRIModelType(ByVal v_lRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="RiskCnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add RiskCnt parameter"

                Throw New Exception()
            End If

            If m_oDatabase.SQLSelect(sSQL:=ACGetRIModelTypeSQL, sSQLName:=ACGetRIModelTypeName, bStoredProcedure:=ACGetRIModelTypeStored, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to retrieve RI Model type"
                Throw New Exception()
            End If

            If Informations.IsArray(vResultArray) Then

                result = CInt(vResultArray(1, 0))
            Else
                sMessage = "Failed to find RI Model type"
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception

            result = -1

            If sMessage = "" Then
                sMessage = "Failed to get RI Model type"
            End If

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRIModelType()", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindAnniversaryCopy
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function FindAnniversaryCopy(ByVal v_sInsuranceRef As String, ByVal v_dtCoverStartDAte As Date, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "FindAnniversaryCopy"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' insurance ref
            m_lReturn = AddInputParameter(v_sName:="insurance_ref", v_vValue:=v_sInsuranceRef, v_iType:=gPMConstants.PMEDataType.PMString)

            ' cover start date
            m_lReturn = AddInputParameter(v_sName:="cover_start_date", v_vValue:=v_dtCoverStartDAte, v_iType:=gPMConstants.PMEDataType.PMDate)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kFindAnniversaryCopySQL, sSQLName:=kFindAnniversaryCopyName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kFindAnniversaryCopySQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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
    ' Name: ApplyPolicyDiscount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 10-01-2006 : Discount / Loading
    ' ***************************************************************** '
    Public Function ApplyPolicyDiscount(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProductId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ApplyPolicyDiscount"

        Dim lReturn As Integer
        Dim sFailureReason As String = ""
        Dim oSIRListRisks As bSIRListRisks.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oSIRListRisks = New bSIRListRisks.Business
            lReturn = oSIRListRisks.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)
            ' apply discount to policy

            lReturn = oSIRListRisks.ProcessApplyDiscount(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lProductId:=v_lProductId, v_sTransactionType:="REN", v_lTask:=gPMConstants.PMEComponentAction.PMAdd, r_sFailureReason:=sFailureReason)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception(sFailureReason)
            End If

            If Not (oSIRListRisks Is Nothing) Then
                'UPGRADE_TODO: (1067) Member Terminate is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                oSIRListRisks.Dispose()
                oSIRListRisks = Nothing
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
    ' Name: DeleteRenewalsLastPrintRun
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 29-03-2006 : bug fix no PN
    ' ***************************************************************** '
    Private Function DeleteRenewalsLastPrintRun(ByVal v_lRenewalInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteRenewalsLastPrintRun"

        Dim lReturn As gPMConstants.PMEReturnCode


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        lReturn = AddInputParameter(v_sName:="renewal_insurance_file_cnt", v_vValue:=v_lRenewalInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kDeleteRenewalsLastPrintRunSQL, sSQLName:=kDeleteRenewalsLastPrintRunName, bStoredProcedure:=True)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, kDeleteRenewalsLastPrintRunSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name : getAllLiveVersions
    '
    ' Desc : check to see if current policy has any other versions available
    ' ***************************************************************** '
    Public Function getAllLiveVersions(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetAllVersionsSQL, sSQLName:=kGetAllVersionsName, bStoredProcedure:=kGetAllVersionsStored, vResultArray:=vResultArray, bKeepNulls:=True)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getAllLiveVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="getAllLiveVersions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '1.12 Wr25
    Private Function CheckRenewalProduct(ByVal vInsuranceFileCnt As Integer, ByRef v_IsValidProduct As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckRenewalProduct"

        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="ifilecnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        m_oDatabase.Parameters.Add(sName:="is_valid", vValue:=CStr(v_IsValidProduct), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCheckRenewalProductSQL, sSQLName:=ACCheckRenewalProductName, bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        Else
            v_IsValidProduct = gPMFunctions.ToSafeBoolean(m_oDatabase.Parameters.Item("is_valid").Value)
        End If

        Return result
    End Function

    Public Function SelectRenewalProduct(ByVal v_lInsuranceFileCnt As Integer, ByRef v_lProductRenewalId As Integer) As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object = Nothing
        Const kMethodName As String = "SelectRenewalProduct"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_oDatabase.Parameters.Add(sName:="ifilecnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRenewalProductSQL, sSQLName:=ACSelectRenewalProductName, bStoredProcedure:=True, vResultArray:=vResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                If Informations.IsArray(vResult) Then
                    v_lProductRenewalId = gPMFunctions.ToSafeLong(vResult(1, 0))
                End If
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

    Public Function UpdateRenewalProduct(ByVal v_lInsuranceFileCnt As Integer) As Integer
        Return UpdateRenewalProduct(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sProductCode:="", v_lProductId:=0)
    End Function

    Public Function UpdateRenewalProduct(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sProductCode As String) As Integer
        Return UpdateRenewalProduct(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sProductCode:=v_sProductCode, v_lProductId:=0)
    End Function

    Public Function UpdateRenewalProduct(ByVal v_lInsuranceFileCnt As Integer, ByRef v_lProductId As Integer) As Integer
        Return UpdateRenewalProduct(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sProductCode:="", v_lProductId:=v_lProductId)
    End Function

    Public Function UpdateRenewalProduct(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sProductCode As String, ByRef v_lProductId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateRenewalProduct"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_oDatabase.Parameters.Add("ifileCnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            If Not False And v_sProductCode <> "" Then
                m_oDatabase.Parameters.Add("product_code", v_sProductCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            End If
            If Not False And gPMFunctions.ToSafeLong(v_lProductId) > 0 Then
                m_oDatabase.Parameters.Add("productId", CStr(v_lProductId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRenewalProductSQL, sSQLName:=ACUpdateRenewalProductName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
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

    Public Function IsTrueMonthlyPolicyProduct(ByVal v_lProductId As Integer, ByRef v_bIsTMP As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Const kMethodName As String = "IsTrueMonthlyPolicyProduct"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get is_midnight_renewal from the product table
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsTMPProductSQL, sSQLName:=ACIsTMPProductName, bStoredProcedure:=True, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            v_bIsTMP = (CDbl(vResultArray(0, 0)) = 1)


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

    Public Function GetInstalmentFrequency(ByVal v_lInsurance_file_cnt As Integer, ByRef sInstalmentFrequency As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Const kMethodName As String = "GetInstalmentFrequency"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("ifilecnt", CStr(v_lInsurance_file_cnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(ACGetInstalmentFrequencySQL, ACGetInstalmentFrequencyName, True, , vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetInstalmentFrequency", "Failed to get instalment frequency ", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Not Informations.IsArray(vResultArray) Then
                    Return result
                End If
                sInstalmentFrequency = gPMFunctions.ToSafeString(vResultArray(0, 0))

            End With


        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    '*****************************************************************************************
    '
    'Task: WR9 Batch Renewal - Multi Threaded Controller
    '
    'History: 10/06/2008 Pankaj - Created.
    '
    'Description: Following functions are parallel to Interface version of iPMURenSelection
    '
    '****************************************************************************************
    Public Function ProcessRenewalSelection(ByVal v_lInsuranceFileCnt As Integer,
                                        ByVal v_bDoNotCreateTMPAnniversaryVersion As Boolean) As Integer
        Return ProcessRenewalSelection(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                            v_lRecordsCount:=0,
                                            v_sGUID:="",
                                            v_lBatchRenewalJobID:=-1,
                                            r_lNewInsuranceFileCnt:=0,
                                            v_bIgnoreDate:=False,
                                            v_iDoNotCopyRisk:=0,
                                            v_bDoNotCreateTMPAnniversaryVersion:=v_bDoNotCreateTMPAnniversaryVersion)

    End Function

    Public Function ProcessRenewalSelection(ByVal v_lInsuranceFileCnt As Integer,
                                    ByVal v_lRecordsCount As Integer,
                                    ByVal v_sGUID As String,
                                    ByVal v_lBatchRenewalJobID As Integer,
                                    ByVal v_bDoNotCreateTMPAnniversaryVersion As Boolean) As Integer
        Return ProcessRenewalSelection(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                            v_lRecordsCount:=v_lRecordsCount,
                                            v_sGUID:=v_sGUID,
                                            v_lBatchRenewalJobID:=v_lBatchRenewalJobID,
                                            r_lNewInsuranceFileCnt:=0,
                                            v_bIgnoreDate:=False,
                                            v_iDoNotCopyRisk:=0,
                                            v_bDoNotCreateTMPAnniversaryVersion:=v_bDoNotCreateTMPAnniversaryVersion)

    End Function

    Public Function ProcessRenewalSelection(ByVal v_lInsuranceFileCnt As Integer,
                                    ByRef r_lNewInsuranceFileCnt As Integer,
                                    ByVal v_bIgnoreDate As Boolean,
                                    ByVal v_iDoNotCopyRisk As Integer) As Integer
        Return ProcessRenewalSelection(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                            v_lRecordsCount:=0,
                                            v_sGUID:="",
                                            v_lBatchRenewalJobID:=-1,
                                            r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt,
                                            v_bIgnoreDate:=v_bIgnoreDate,
                                            v_iDoNotCopyRisk:=v_iDoNotCopyRisk,
                                            v_bDoNotCreateTMPAnniversaryVersion:=False)

    End Function

    Public Function ProcessRenewalSelection(ByVal v_lInsuranceFileCnt As Integer,
                                    ByRef r_lNewInsuranceFileCnt As Integer,
                                    ByVal v_bIgnoreDate As Boolean,
                                    ByVal v_iDoNotCopyRisk As Integer,
                                    ByRef r_sFailureReason As String) As Integer
        Return ProcessRenewalSelection(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                            v_lRecordsCount:=0,
                                            v_sGUID:="",
                                            v_lBatchRenewalJobID:=-1,
                                            r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt,
                                            v_bIgnoreDate:=v_bIgnoreDate,
                                            v_iDoNotCopyRisk:=v_iDoNotCopyRisk,
                                            v_bDoNotCreateTMPAnniversaryVersion:=False,
                                            r_sFailureReason:=r_sFailureReason)

    End Function

    Public Function ProcessRenewalSelection(ByVal v_lInsuranceFileCnt As Integer,
                                    ByVal v_lRecordsCount As Integer,
                                    ByVal v_sGUID As String,
                                    ByVal v_lBatchRenewalJobID As Integer) As Integer
        Return ProcessRenewalSelection(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                            v_lRecordsCount:=v_lRecordsCount,
                                            v_sGUID:=v_sGUID,
                                            v_lBatchRenewalJobID:=v_lBatchRenewalJobID,
                                            r_lNewInsuranceFileCnt:=0,
                                            v_bIgnoreDate:=False,
                                            v_iDoNotCopyRisk:=0,
                                            v_bDoNotCreateTMPAnniversaryVersion:=False)

    End Function

    Public Function ProcessRenewalSelection(ByVal v_lInsuranceFileCnt As Integer,
                                            ByVal v_lRecordsCount As Integer,
                                            ByVal v_sGUID As String,
                                            ByVal v_lBatchRenewalJobID As Integer,
                                            ByRef r_lNewInsuranceFileCnt As Integer,
                                            ByVal v_bIgnoreDate As Boolean,
                                            ByVal v_iDoNotCopyRisk As Integer,
                                            ByVal v_bDoNotCreateTMPAnniversaryVersion As Boolean) As Integer
        Return ProcessRenewalSelection(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                            v_lRecordsCount:=v_lRecordsCount,
                                            v_sGUID:=v_sGUID,
                                            v_lBatchRenewalJobID:=v_lBatchRenewalJobID,
                                            r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt,
                                            v_bIgnoreDate:=v_bIgnoreDate,
                                            v_iDoNotCopyRisk:=v_iDoNotCopyRisk,
                                            v_bDoNotCreateTMPAnniversaryVersion:=v_bDoNotCreateTMPAnniversaryVersion,
                                            r_sFailureReason:="")

    End Function

    Public Function ProcessRenewalSelection(ByVal v_lInsuranceFileCnt As Integer,
                                            ByVal v_lRecordsCount As Integer,
                                            ByVal v_sGUID As String,
                                            ByVal v_lBatchRenewalJobID As Integer,
                                            ByRef r_lNewInsuranceFileCnt As Integer,
                                            ByVal v_bIgnoreDate As Boolean,
                                            ByVal v_iDoNotCopyRisk As Integer,
                                            ByVal v_bDoNotCreateTMPAnniversaryVersion As Boolean,
                                            ByRef r_sFailureReason As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessRenewalSelection"


        Dim oPMLock As bpmlock.User
        Dim sLockedBy As String = ""
        Dim vRenewalList As Object
        Dim lCount As Integer 'renewal status to go on the Renewal Status table
        Dim sInsuranceRef As String = "" 'new insurance ref
        Dim sFailedText As String = ""
        Dim lRecordsCount, lBatchRenewalJobRunsID As Integer
        Dim iRenewalDocDestination, iReportSortOrder As Integer
        Dim sFailureCriterion As String = ""
        Dim iIsFailed As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get bPMLock
            oPMLock = New bpmlock.User
            lReturn = oPMLock.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)

            'Check for any error
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            'Lock the Key
            lReturn = oPMLock.LockKey(sKeyName:="RENSEL", vKeyValue:=v_lInsuranceFileCnt, iUserID:=m_iUserID, sCurrentlyLockedBy:=sLockedBy)

            'Check for any error
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailedText = "This record has been locked by " & sLockedBy
                Throw New Exception(sFailedText)
            End If

            If v_bDoNotCreateTMPAnniversaryVersion = True Then
                v_bIgnoreDate = True
            End If

            'get policy details that needs renewal
            lReturn = GetRenewalSelectionDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lSourceID:=m_iSourceID, v_dtCompareDate:=CDate(DateTime.Now.ToString("dd MMMM yyyy")), r_vResultArray:=vRenewalList, v_vStartDate:=Nothing, v_bIgnoreDate:=v_bIgnoreDate)

            'Do we have any data ?
            If Not Informations.IsArray(vRenewalList) Then
                r_sFailureReason = "Either no policy version found or renewal version exists over current policy version."
                Return result
            End If
            lCount = 0

            m_lPolicyVersionIncrement = 0

            lReturn = CreateRenewalPolicyWrapper(r_vRenewalList:=vRenewalList, v_lCount:=lCount, r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, r_sFailureCriterion:=sFailureCriterion, v_iDoNotCopyRisk:=v_iDoNotCopyRisk)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            If v_bDoNotCreateTMPAnniversaryVersion = False Then
                lReturn = CreateTMPAnniversaryRenewal(r_vRenewalList:=vRenewalList, v_lCount:=lCount)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If
            iIsFailed = 0

            If sFailureCriterion.Trim().Length > 0 Then
                iIsFailed = 1
            End If

            If v_lBatchRenewalJobID <> -1 Then
                'Add Record to Batch_Renewal_Job_Runs
                lReturn = AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dRunDate:=CDate(DateTime.Now.ToString("dd MMMM yyyy")), v_sFailureReason:=sFailureCriterion, v_sDocumentPrinted:="", v_iIsFailed:=iIsFailed, v_sGUID:=v_sGUID, r_lRecordsCount:=lRecordsCount, r_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception
                End If

                'Here we check the number of records in Batch_Renewal_Job_Runs matched with Total no of policies
                'then print renewal report and send mail to agents
                'v_lRecordsCount is send from SAM as array index rather than
                'the actual number of policies so checking greater than.
                If lRecordsCount > v_lRecordsCount Then
                    lReturn = CType(GetBatchJobPrintingOptions(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, r_iRenewalDocDestination:=iRenewalDocDestination, r_iReportSortOrder:=iReportSortOrder), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception
                    End If

                    If iRenewalDocDestination <> 0 Then
                        lReturn = CType(PrintRenewalReport(iReportSortOrder), gPMConstants.PMEReturnCode)
                    End If
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

            If sFailedText.Length = 0 Then
                sFailedText = Informations.Err().Description
            End If

            If sFailedText.Length = 0 Then
                sFailedText = "Fails to Process Renewal Selection"
            End If

            If v_lBatchRenewalJobID <> -1 Then
                'Add Record to Batch_Renewal_Job_Runs with failure reason
                lReturn = AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=v_lBatchRenewalJobID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_dRunDate:=CDate(DateTime.Now.ToString("dd MMMM yyyy")), v_sFailureReason:=sFailedText, v_sDocumentPrinted:="", v_iIsFailed:=1, v_sGUID:=v_sGUID, r_lRecordsCount:=lRecordsCount, r_lBatchRenewalJobRunsID:=lBatchRenewalJobRunsID)

            End If

            'UnLock the Key
            lReturn = oPMLock.UnLockKey(sKeyName:="RENSEL", vKeyValue:=v_lInsuranceFileCnt, iUserID:=m_iUserID)

            oPMLock.Dispose()
            Return result
        Finally
            'UnLock the Key
            lReturn = oPMLock.UnLockKey(sKeyName:="RENSEL", vKeyValue:=v_lInsuranceFileCnt, iUserID:=m_iUserID)
            oPMLock.Dispose()
            oPMLock = Nothing

        End Try
        Return result
    End Function

    ''' <summary>
    ''' CreateRenewalPolicyWrapper
    ''' </summary>
    ''' <param name="r_vRenewalList"></param>
    ''' <param name="v_lCount"></param>
    ''' <param name="r_lNewInsuranceFileCnt"></param>
    ''' <param name="r_sFailureCriterion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateRenewalPolicyWrapper(ByRef r_vRenewalList As Object, ByVal v_lCount As Integer, ByRef r_lNewInsuranceFileCnt As Integer, ByRef r_sFailureCriterion As String) As Integer
        Return CreateRenewalPolicyWrapper(r_vRenewalList:=r_vRenewalList, v_lCount:=v_lCount, r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, r_sFailureCriterion:=r_sFailureCriterion, v_iDoNotCopyRisk:=0)
    End Function

    ''' <summary>
    ''' CreateRenewalPolicyWrapper
    ''' </summary>
    ''' <param name="r_vRenewalList"></param>
    ''' <param name="v_lCount"></param>
    ''' <param name="r_lNewInsuranceFileCnt"></param>
    ''' <param name="r_sFailureCriterion"></param>
    ''' <param name="v_iDoNotCopyRisk"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateRenewalPolicyWrapper(ByRef r_vRenewalList As Object, ByVal v_lCount As Integer, ByRef r_lNewInsuranceFileCnt As Integer, ByRef r_sFailureCriterion As String, ByVal v_iDoNotCopyRisk As Integer) As Integer

        Dim nResult As Integer
        Const kMethodName As String = "CreateRenewalPolicyWrapper"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' set additional policy version increment
            m_lPolicyVersionIncrement = 1

            ' create the renewal
            lReturn = CType(CreateRenewalPolicy(r_vRenewalList:=r_vRenewalList,
                                                r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, v_lCount:=v_lCount,
                                                r_sFailureCriterion:=r_sFailureCriterion,
                                                v_iDoNotCopyRisk:=v_iDoNotCopyRisk),
                        gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: CreateTMPAnniversaryRenewal
    '
    ' Parameters: n/a
    '
    ' Description: Creates
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function CreateTMPAnniversaryRenewal(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateTMPAnniversaryRenewal"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bIsTrueMonthlyPolicy As Boolean

        Dim lAnniversaryRenewalWeeks As Integer
        Dim sInsuranceRef As String = ""
        Dim dtCoverStartDate As Date
        Dim vAnniversaryCopy As Object
        Dim lAnniversaryCopyCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' determine whether the associated policies product is a "True Monthly Policy"
            bIsTrueMonthlyPolicy = gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosIsTrueMonthlyPolicy, v_lCount)) = 1

            ' if the associated product is "True Monthly Policy"
            If bIsTrueMonthlyPolicy Then

                ' get the anniversary renewal weeks
                ' this is the number of weeks prior to the anniversary date that the
                ' that the anniversary renewal version of the policy should be created

                lAnniversaryRenewalWeeks = CInt(r_vRenewalList(PMFieldPosAnniversaryRenewalWeeks, v_lCount))

                ' if the anniversary renewal version of the policy should have been created
                ' by now

                If gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosRenewalDate, v_lCount)) >= gPMFunctions.ToSafeDate(Informations.DateAdd("ww", -lAnniversaryRenewalWeeks, CDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount)))) Then

                    sInsuranceRef = CStr(r_vRenewalList(PMFieldPosInsuranceRef, v_lCount)).Trim()

                    ' cover start date for the anniversary copy is the anniversary date of the
                    ' original policy

                    dtCoverStartDate = CDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount))

                    'Debug.Print m_oBusiness.TransactionType

                    ' check if it has been created
                    lReturn = CType(FindAnniversaryCopy(v_sInsuranceRef:=sInsuranceRef, v_dtCoverStartDAte:=dtCoverStartDate, r_vResults:=vAnniversaryCopy), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception
                    End If

                    ' if there is an array we have an anniversary copy
                    ' if not go ahead and create the anniversary copy
                    If Not Informations.IsArray(vAnniversaryCopy) Then
                        lAnniversaryCopyCount = 0
                    Else

                        lAnniversaryCopyCount = CInt(vAnniversaryCopy(0, 0))
                    End If

                    If lAnniversaryCopyCount = 0 Then

                        ' create an anniversary renewal version of the policy
                        lReturn = CType(CreateRenewalPolicy(r_vRenewalList:=r_vRenewalList, v_lCount:=v_lCount, v_bTMPAnniversary:=True), gPMConstants.PMEReturnCode)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New Exception
                        End If

                    End If

                End If

            Else
                ' do nothing
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

    ' Tech Spec PGR 8.8 Renewals ---------------------------------
    Public Function GetInitialPolicyDetails(ByVal lInsuranceFileCnt As Integer, ByRef sPaymentMethod As String, ByRef lLatestInstalmentInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInitialPolicyDetails"
        Dim vResultArray(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'clear parameter list and add in required parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInitialPolicyDetailsSQL, sSQLName:=ACGetInitialPolicyDetailsName, bStoredProcedure:=True, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vResultArray) Then
                Throw New Exception
            Else
                sPaymentMethod = gPMFunctions.ToSafeString(vResultArray(0, 0), "")
                lLatestInstalmentInsuranceFileCnt = gPMFunctions.ToSafeInteger(vResultArray(1, 0), 0)
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
    ''' Change current policy status to Renewal Create new policy of type Renewal
    ''' History: 14/02/2001 Created (RWH)
    ''' </summary>
    ''' <param name="r_vRenewalList"></param>
    ''' <param name="v_lCount"></param>
    ''' <param name="v_bTMPAnniversary"></param>
    ''' <param name="r_lNewInsuranceFileCnt"></param>
    ''' <param name="r_sFailureCriterion"></param>
    ''' <param name="v_iDoNotCopyRisk"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateRenewalPolicy(ByRef r_vRenewalList(,) As Object,
                                         ByVal v_lCount As Integer,
                                         Optional ByVal v_bTMPAnniversary As Boolean = False,
                                         Optional ByRef r_lNewInsuranceFileCnt As Integer = 0,
                                         Optional ByRef r_sFailureCriterion As String = "",
                                         Optional ByVal v_iDoNotCopyRisk As Integer = 0) As Integer

        Dim nResult As Integer
        Dim nRenewalStatusTypeID As Integer = 0 'renewal status to go on the Renewal Status table
        Dim sFailureCriterion As String = ""
        Dim nNewInsuranceFileCnt As Integer = 0
        Dim sInsuranceRef As String = ""
        Dim lEligibleForRenewal As gPMConstants.PMEReturnCode
        Dim nProductId As Integer
        Dim nInsuredCnt As Integer
        Dim nSourceID As Integer = 0
        Dim bMidnightRenewal As Boolean
        Dim oKeyArray As Object
        Dim bIsPremiumZero As Boolean
        Dim lIsQuoted As gPMConstants.PMEReturnCode
        Dim oArray As Object = Nothing
        Dim oInsuranceFileTax As Object
        Dim sDescription As String
        Dim sDateInteval As String
        Dim nDateIntervalNumber As Integer = 0
        Dim lIsAgentCancelled As gPMConstants.PMEReturnCode
        Dim oRenewalFrequency As Object = Nothing
        Dim nUnderwritingYearID As Integer = 0
        Dim sFailureMessage As String = ""
        Dim oBrokerTransferPorfolio As Object = Nothing
        Dim nBrokerXferStatusTypeID As Integer = 0

        ' True monthly policy changes
        Dim bIsTrueMonthlyPolicy As Boolean
        Dim nAnniversaryCopy As Integer = 0
        Dim nRenewalDayNumber As Integer = 0
        Dim dtTMPCoverStartDate As Date
        Dim dtTMPExpiryDate As Date
        Dim dtTMPRenewalDate As Date
        Dim dtTMPAnniversaryDate As Date
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bPutOnnextInstalmentRenewal As Boolean
        Dim nOriginalInsuranceFileCnt As Integer = 0
        Dim bContinue As Boolean
        Dim bRenewalStatusAutomatic As Boolean

        'PN 30936
        Dim oReinsurance As Object = Nothing
        Dim bInvoiceEnabled As Boolean
        Dim bInstalmentsEnabled As Boolean
        Dim bPayNowEnabled As Boolean
        Dim sPayment_Method As String = ""
        Dim nRenewalProductId As Integer = 0
        Dim sValue As String = ""
        Dim oIsRI2007 As Object
        Dim sInstFreqency As String = ""
        Dim bisOriginalProductTMP As Boolean
        Dim bSwapProducts As Boolean
        Dim oInsuranceFileBusiness As bSIRInsuranceFile.Business
        Dim oInsuranceFile As bSIRInsuranceFile.Services
        Dim oChangePolicyStatus As bSIRChangePolicyStatus.Business
        Dim oAgentCommission As bSirAgentCommission.Business
        Dim oTax As bSIRRITax.Business
        Dim bBankGuaranteeEnabled As Boolean
        Dim bChanged As Boolean
        Const kPolicyBusinessType As Integer = 2
        Dim oUseNbPaymentTermAtRenSelection As Object = Nothing
        Dim oProduct As bSIRProduct.Business
        Dim nInstalmentInsuranceFileCnt As Integer = 0
        Dim bCashDepositEnabled As Boolean
        Dim bIsOnlyAgentTransfer As Boolean
        Dim bIsReferred As Boolean
        Dim oGracePeriod As Object
        Dim oPolicyNumMaint As bSIRPolicyNumMaint.Business
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue
            r_sFailureCriterion = ""

            oInsuranceFile = New bSIRInsuranceFile.Services
            m_lReturn = oInsuranceFile.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailureCriterion = "Fails to create object bSirInsuranceFile.Services"
                Throw New Exception(sFailureCriterion)
            End If

            oInsuranceFileBusiness = New bSIRInsuranceFile.Business
            m_lReturn = oInsuranceFileBusiness.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailureCriterion = "Fails to create object bSirInsuranceFile.Business"
                Throw New Exception(sFailureCriterion)
            End If

            'assign current InsuranceFiCreateBusinessObject(r_oObject:=oListRisksBleCnt to object
            oInsuranceFile.InsuranceFileCnt = r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)

            'get details of current policy
            If oInsuranceFile.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailureCriterion = "Fails to get insurance details"
                Throw New Exception(sFailureCriterion)
            End If

            'set policy status to Renewal
            oInsuranceFile.InsuranceFileStatus = "REN"
            oInsuranceFile.LapsedReasonID = Nothing
            oInsuranceFile.LapsedDate = Nothing

            lReturn = GetPaymentTerms(v_lInsuranceFileCnt:=oInsuranceFile.InsuranceFileCnt, v_lPMUserID:=m_iUserID,
                                      r_bInvoiceEnabled:=bInvoiceEnabled, r_bInstalmentsEnabled:=bInstalmentsEnabled,
                                      r_bPayNowEnabled:=bPayNowEnabled, r_bBankGuaranteeEnabled:=bBankGuaranteeEnabled,
                                      r_bCashDepositEnabled:=bCashDepositEnabled)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailureCriterion = "bSIRListRisks.Business.GetPaymentTerms Failed"
                Throw New Exception(sFailureCriterion)
            End If

            oProduct = New bSIRProduct.Business
            lReturn = CType(oProduct.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase),
                            gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailureCriterion = "Failed to get instance of bSIRProduct.Business"
                Throw New Exception(sFailureCriterion)
            End If

            lReturn = oProduct.GetProductValue(v_lProductId:=oInsuranceFile.ProductID,
                                               v_sColumnName:="use_nb_payment_term_at_renselection",
                                               r_vProductArray:=oUseNbPaymentTermAtRenSelection)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailureCriterion = "bSIRProduct.Business.GetProductValue Failed"
                Throw New Exception(sFailureCriterion)
            End If

            Dim sOptionValue As String = "0"

            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=kSystemOptionAutoInstalment, r_sOptionValue:=sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                sFailureCriterion = "iPMFunc.GetSystemOption Failed"
            End If

            oBatchRenewalBusiness = New bSIRRenewalBusiness()
            oBatchRenewalBusiness.Initialise(sUsername:="", sPassword:="", iUserID:=0, iSourceID:=1, iLanguageID:=0, iCurrencyID:=0, iLogLevel:=0, sCallingAppName:=ACApp)
            If oBatchRenewalBusiness Is Nothing Then
                Throw New ApplicationException("Failed to create instance of bSIRRenewalBusiness.Business")
            End If
            If Informations.IsArray(oUseNbPaymentTermAtRenSelection) Then
                If Not (Not (CDbl(oUseNbPaymentTermAtRenSelection(0, 0)) = 0)) Then
                    If (sOptionValue.ToString = "1") Then
                        lReturn = oBatchRenewalBusiness.HasInstalmentPlanOnCurrentTerm(nInsuranceFileCnt:=oInsuranceFile.InsuranceFileCnt,
                                                                  sPaymentMethod:=sPayment_Method,
                                                                  nLatestInstalmentInsuranceFileCnt:=
                                                                     nInstalmentInsuranceFileCnt)
                        sPayment_Method = sPayment_Method.ToLower()
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            sFailureCriterion = "HasInstalmentPlanOnCurrentTerm Failed"
                        Else

                            oInsuranceFile.PaymentMethod = sPayment_Method

                        End If
                    Else
                        sPayment_Method = gPMFunctions.ToSafeString(oInsuranceFile.PaymentMethod).ToLower().Trim()
                    End If
                Else
                    If (sOptionValue.ToString = "1") Then
                        lReturn = oBatchRenewalBusiness.HasInstalmentPlanOnCurrentTerm(nInsuranceFileCnt:=oInsuranceFile.InsuranceFileCnt,
                                                                  sPaymentMethod:=sPayment_Method,
                                                                  nLatestInstalmentInsuranceFileCnt:=
                                                                     nInstalmentInsuranceFileCnt)
                    Else
                        lReturn = CType(GetInitialPolicyDetails(lInsuranceFileCnt:=oInsuranceFile.InsuranceFileCnt,
                                                                sPaymentMethod:=sPayment_Method,
                                                                lLatestInstalmentInsuranceFileCnt:=nInstalmentInsuranceFileCnt),
                                        gPMConstants.PMEReturnCode)
                    End If
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sFailureCriterion = "GetInitialPolicyDetails Failed"
                        Throw New Exception(sFailureCriterion)
                    Else
                        sPayment_Method = sPayment_Method.ToLower()
                        If sPayment_Method = "instalment" Then
                            sPayment_Method = "instalments"
                        End If

                    End If
                End If
            Else
                sPayment_Method = gPMFunctions.ToSafeString(oInsuranceFile.PaymentMethod).ToLower().Trim()
            End If
            If (oBatchRenewalBusiness IsNot Nothing) Then
                oBatchRenewalBusiness.Dispose()
                oBatchRenewalBusiness = Nothing
            End If


            'if Payment_Method is not PayNow and Instalments then it must be Invoice
            sPayment_Method = ToSafeString(sPayment_Method).Trim.ToLower

            If sPayment_Method = "" Then
                sPayment_Method = "invoice"
            End If

            'Check if the Payment_Method is Invalid
            If sPayment_Method = "invoice" AndAlso Not bInvoiceEnabled Then
                'CreateRenewalPolicy = PMFalse
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Payment Type - (Invoice) is Invalid"
            ElseIf _
                (sPayment_Method = "instalments" OrElse sPayment_Method = "direct debit" OrElse sPayment_Method = "credit card") AndAlso
                Not bInstalmentsEnabled Then
                'CreateRenewalPolicy = PMFalse
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Payment Type - (Instalments) is Invalid"
            ElseIf sPayment_Method = "paynow" AndAlso Not bPayNowEnabled Then
                'CreateRenewalPolicy = PMFalse
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Payment Type - (PayNow) is Invalid"
            ElseIf sPayment_Method = "bankguarantee" AndAlso Not bBankGuaranteeEnabled Then
                'CreateRenewalPolicy = PMFalse
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Payment Type - (BankGuarantee) is Invalid"
                'Start - Prakash - WPR85_Paralleling
            ElseIf sPayment_Method = "cashdeposit" AndAlso Not bCashDepositEnabled Then
                'CreateRenewalPolicy = PMFalse
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Payment Type - (CashDeposit) is Invalid"
            End If

            If sPayment_Method = "cashdeposit" Then
                'If payment method is cash deposit, always set for manual review.
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Manual Review - Payment option cash depsoit"
            End If

            r_sFailureCriterion = sFailureCriterion

            If sFailureCriterion <> "" Then
                'If there's an Err, Add it to Renewal_report table
                m_lReturn = AddRenewalReport(v_sReportType:=If(bRenewalStatusAutomatic, "AutoRenewal", "ManualRenewal"),
                                             v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                                             v_vPolicyNumber:=r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                             v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                             v_vCoverStartDate:=r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                                             v_vCoverEndDate:=r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                             v_vProductCode:=r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                             v_vFailureCriterion:=sFailureCriterion,
                                             v_vFailureDetail:="",
                                             v_vInsuranceFileCnt:=
                                            CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)))

            End If

            If oInsuranceFile.BusinessType.Trim() = "COIN FOLL" OrElse oInsuranceFile.BusinessType.Trim() = "IN FAC" Then
                sFailureCriterion = "Manual Review Required for Co-Insurance Follow / Inward Facultative"
                m_lReturn = AddRenewalReport(v_sReportType:="ManualRenewal",
                                             v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                                             v_vPolicyNumber:=r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                             v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                             v_vCoverStartDate:=r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                                             v_vCoverEndDate:=r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                             v_vProductCode:=r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                             v_vFailureCriterion:=sFailureCriterion,
                                                 v_vFailureDetail:="",
                                             v_vInsuranceFileCnt:=
                                                CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)))
            End If

            '01/08/2003 Tracy Richards - Add a transaction to umbrella all these
            'calls, to make sure that records are not left in a mid-state.
            m_lReturn = oInsuranceFile.BeginTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'update current policy to status Renewal
            m_lReturn = oInsuranceFile.UpdatePolicy
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'set policy type to Renewal
            oInsuranceFile.InsuranceFileType = "RENEWAL"
            'set policy to Live (i.e. status = Null).
            oInsuranceFile.InsuranceFileStatus = Nothing
            nProductId = oInsuranceFile.ProductID

            m_lReturn = GetMidnightRenewal(v_lProductId:=nProductId, r_bMidnight:=bMidnightRenewal)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get renewal frequency number (number of months)
            m_lReturn = GetRenewalFrequencyDetail(v_lFrequencyID:=oInsuranceFile.RenewalFrequencyID,
                                                  r_vResult:=oRenewalFrequency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' determine which if any policy discount details need
            ' to be applied to the renewal....

            ' if the original policy has a recurring type of discount then
            If oInsuranceFile.DiscountRecurringTypeId = kDiscountRecurringTypeIdPolicy Then
                ' retain the existing policy discount information from the original policy
            Else
                ' otherwise clear down this information
                oInsuranceFile.DiscountPercentage = 0
                oInsuranceFile.DiscountReasonID = 0
                oInsuranceFile.MatchDiscountedPremiumFlag = 0
                oInsuranceFile.DiscountedPremium = 0
                oInsuranceFile.DiscountRecurringTypeId = 0
            End If

            Dim vAltRefMandetory As Object


            If gPMFunctions.ToSafeLong(oInsuranceFile.LeadAgentCnt, 0) <> 0 Then

                m_lReturn = oInsuranceFileBusiness.GetFromTable("party_agent", "alternate_reference_mandatory", "party_cnt",
                                                                oInsuranceFile.LeadAgentCnt, vAltRefMandetory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If gPMFunctions.ToSafeBoolean(vAltRefMandetory, 0) Then
                    If String.IsNullOrEmpty(Convert.ToString(r_vRenewalList(PMFieldAlternateReference, v_lCount))) Then
                        oInsuranceFile.AlternateReference = ""
                        nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                        sFailureCriterion = "The Alternate Reference must be entered for this renewal policy." &
                                    "You must amend the renewal before the renewal can be accepted."
                    Else
                        oInsuranceFile.AlternateReference = r_vRenewalList(PMFieldAlternateReference, v_lCount)
                    End If
                End If
            End If

            sDateInteval = "m"

            nDateIntervalNumber = CInt(oRenewalFrequency(2, 0))
            'check if original product is tmp
            m_lReturn = IsTrueMonthlyPolicyProduct(oInsuranceFile.ProductID, bisOriginalProductTMP)

            'get the instalmment scheme frequency
            If _
                sPayment_Method = "instalments" OrElse sPayment_Method = "direct debit" OrElse
                sPayment_Method <> "paynow" AndAlso sPayment_Method <> "credit card" Then
                m_lReturn = GetInstalmentFrequency(
                    gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)), sInstFreqency)
            End If

            bSwapProducts = False
            If _
                bisOriginalProductTMP AndAlso
                gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosRenewalDate, v_lCount)) >=
                gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount)) Then
                bSwapProducts = True
            ElseIf Not bisOriginalProductTMP Then
                bSwapProducts = True
            End If

            If gPMFunctions.ToSafeString(r_vRenewalList(PMFieldPosRenewalProductId, v_lCount)) <> "" AndAlso bSwapProducts Then
                oInsuranceFile.OriginalProductID = r_vRenewalList(PMFieldPosProductID, v_lCount)
                oInsuranceFile.ProductID = r_vRenewalList(PMFieldPosRenewalProductId, v_lCount)
                oInsuranceFile.RenewalProductID = r_vRenewalList(PMFieldPosRenewalProductId, v_lCount)
            End If

            m_lReturn = IsTrueMonthlyPolicyProduct(oInsuranceFile.ProductID, bIsTrueMonthlyPolicy)
            ' if the associated product is "True Monthly Policy"
            If bIsTrueMonthlyPolicy Then
                If Not v_bTMPAnniversary Then
                    v_bTMPAnniversary =
                        gPMFunctions.ToSafeBoolean(
                            r_vRenewalList(PMFieldPosRenewalDate, v_lCount).Equals(r_vRenewalList(PMFieldPosAnniversaryDate,
                                                                                                  v_lCount)))
                End If
                ' get the appropriate dates for the true monthly policy
                lReturn = CType(GetTrueMonthlyPolicyDates(v_bMidnightRenewal:=bMidnightRenewal,
                                                          v_bTMPAnniversary:=v_bTMPAnniversary, v_lCount:=v_lCount,
                                                          r_vRenewalList:=r_vRenewalList,
                                                          r_dtCoverStartDate:=dtTMPCoverStartDate,
                                                          r_dtExpiryDate:=dtTMPExpiryDate,
                                                          r_dtRenewalDate:=dtTMPRenewalDate,
                                                          r_dtAnniversaryDate:=dtTMPAnniversaryDate,
                                                          r_lRenewalDayNumber:=nRenewalDayNumber,
                                                          r_lAnniversaryCopy:=nAnniversaryCopy),
                                gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oInsuranceFile.RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' save the data back to the insurance file
                oInsuranceFile.CoverStartDate = dtTMPCoverStartDate
                oInsuranceFile.ExpiryDate = dtTMPExpiryDate
                oInsuranceFile.RenewalDate = dtTMPRenewalDate
                oInsuranceFile.InceptionTPI = dtTMPCoverStartDate
                oInsuranceFile.AnniversaryDate = dtTMPAnniversaryDate
                oInsuranceFile.AnniversaryCopy = nAnniversaryCopy
                oInsuranceFile.RenewalDayNumber = nRenewalDayNumber
                oInsuranceFile.PutOnNextInstalmentRenewal = 0

                bPutOnnextInstalmentRenewal =
                    gPMFunctions.ToSafeLong(CDbl(r_vRenewalList(PMFieldPosPutOnNextInstalmentRenewal, v_lCount)) = 1)

                ' if this is the anniversary copy of the policy then
                ' reset the inception date to be that of the new cover start date
                If v_bTMPAnniversary Then
                    oInsuranceFile.InceptionDate = dtTMPCoverStartDate
                End If

            Else

                'set new cover period
                'The problem here is that cover start date is that of this version of
                'the policy not of the policy as a whole.  So let's use the renewal
                'date instead, as that won't have changed
                If bMidnightRenewal Then
                    oInsuranceFile.CoverStartDate = CDate(r_vRenewalList(PMFieldPosCoverEndDate, v_lCount)).AddDays(1)
                    oInsuranceFile.ExpiryDate =
                        Informations.DateAdd(sDateInteval, nDateIntervalNumber, oInsuranceFile.CoverStartDate).AddDays(-1)
                Else
                    oInsuranceFile.CoverStartDate = r_vRenewalList(PMFieldPosCoverEndDate, v_lCount)
                    oInsuranceFile.ExpiryDate = Informations.DateAdd(sDateInteval, nDateIntervalNumber,
                                                                    oInsuranceFile.CoverStartDate)
                End If

                oInsuranceFile.RenewalDate = Informations.DateAdd(sDateInteval, nDateIntervalNumber,
                                                             oInsuranceFile.CoverStartDate)
                oInsuranceFile.InceptionTPI = oInsuranceFile.CoverStartDate
                oInsuranceFile.AnniversaryDate = oInsuranceFile.RenewalDate
                oInsuranceFile.RenewalDayNumber = Convert.ToDateTime(oInsuranceFile.RenewalDate).Day

            End If

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword,
                                                      v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID,
                                                      v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID,
                                                      v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp,
                                                      v_vOptionNumber:=
                                                         gPMConstants.SIRHiddenOptions.SIROPTUnderwritingYear,
                                                      v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=sValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sValue = "1" Then
                m_bUnderwritingYearID = True
            Else
                m_bUnderwritingYearID = False
            End If

            If m_bUnderwritingYearID Then

                m_lReturn = oInsuranceFileBusiness.GetUnderwritingYear(oInsuranceFile.CoverStartDate, ToSafeInteger(nUnderwritingYearID))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                             sMsg:="Failed to get Underwriting Year for " & m_sTransactionType & ".",
                                             vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                ElseIf nUnderwritingYearID = 0 Then
                    'PN14537 - Log the problem, don't prompt the user
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review

                    sFailureCriterion = "No Underwriting Year exists for " &
                                    StringsHelper.Format(oInsuranceFile.CoverStartDate, "General Date")

                    oInsuranceFile.UnderwritingYearID = nUnderwritingYearID
                    lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
                Else

                    oInsuranceFile.UnderwritingYearID = nUnderwritingYearID
                End If
            End If

            oInsuranceFile.EventDescription = "Policy Copied To Renewal"

            If v_bTMPAnniversary Then
                ' if this is an anniversary policy it is possible that
                ' a normal renewal (non anniversary) has already been run
                ' so a further increment of the Policy Version is required
                oInsuranceFile.PolicyVersion = oInsuranceFile.PolicyVersion + 1 + m_lPolicyVersionIncrement
            Else
                Dim iMaxPolicyVersion As Integer = 0
                oInsuranceFile.PolicyVersion += 1

                m_lReturn = GetNextPolicyVersion(oInsuranceFile.InsuranceFileCnt, iMaxPolicyVersion)

                If iMaxPolicyVersion > 0 Then
                    oInsuranceFile.PolicyVersion = iMaxPolicyVersion
                End If
            End If

            'Start(Saurabh Agrawal) Tech Spec VAL p14 Policy Numbering (5.3.1)

            If v_bTMPAnniversary Then
                Dim oAnniversaryCopy As Object = Nothing
                Dim nAnniversaryCopyCount As Integer = 0
                lReturn = FindAnniversaryCopy(v_sInsuranceRef:=oInsuranceFile.InsuranceRef,
                                              v_dtCoverStartDAte:=oInsuranceFile.CoverStartDate,
                                              r_vResults:=oAnniversaryCopy)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If
                ' if there is an array we have an anniversary copy
                ' if not go ahead and create the anniversary copy
                If Informations.IsArray(oAnniversaryCopy) Then
                    nAnniversaryCopyCount = oAnniversaryCopy(0, 0)
                    If nAnniversaryCopyCount <> 0 Then
                        oInsuranceFile.RollbackTrans()
                        m_lReturn = UpdateInsuranceFileCntForAnniversaryVersion(oInsuranceFile.InsuranceFileCnt,
                                                                                oInsuranceFile.InsuranceRef,
                                                                                oInsuranceFile.CoverStartDate)


                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            Return nResult
                        End If
                        Return gPMConstants.PMEReturnCode.PMTrue
                        nResult = m_lReturn
                        Return nResult
                    End If
                End If
            End If


            oPolicyNumMaint = New bSIRPolicyNumMaint.Business()
            m_lReturn = oPolicyNumMaint.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID,
                                                   iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel,
                                                   sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sInsuranceRef = oInsuranceFile.InsuranceRef
            If Not SkipGenerateRenewalPolicyNumber Then
                m_lReturn = oPolicyNumMaint.GenerateRenewalPolicyNumber(v_iPolicy_cnt:=gPMFunctions.ToSafeInteger(CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))),
                                                                        v_lBusinessType:=kPolicyBusinessType, v_iBranch:=gPMFunctions.ToSafeInteger(oInsuranceFile.SourceID),
                                                                        v_lProductId:=gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosProductID, v_lCount)),
                                                                        v_lAgent:=gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount)),
                                                                        r_sGeneratedPolicyNumber:=sInsuranceRef, r_bChanged:=bChanged,
                                                                        v_dtTransactionDate:=oInsuranceFile.CoverStartDate,
                                                                        v_lPartyCnt:=oInsuranceFile.InsuranceHolderCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'create new policy of type Renewal
            If bChanged Then
                oInsuranceFile.InsuranceRef = sInsuranceRef
                r_vRenewalList(PMFieldPosInsuranceRef, v_lCount) = sInsuranceRef
            End If

            'Update Renewal Quote Expiry date
            lReturn = oProduct.GetProductValue(v_lProductId:=oInsuranceFile.ProductID, v_sColumnName:="grace_period", r_vProductArray:=oGracePeriod)
            If Informations.IsArray(oGracePeriod) Then
                Dim iGracePeriod As Integer = CInt(oGracePeriod.GetValue(0, 0))
                If iGracePeriod > 0 Then
                    oInsuranceFile.QuoteExpiryDate = DateTime.Today.AddDays(iGracePeriod)
                End If
            End If
            'Etana batch renewal selection enhancements
            'we're now creating this via a stored procedure for speed instead of business objects
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add("old_insurance_file_cnt", ToSafeInteger(oInsuranceFile.InsuranceFileCnt),
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("new_insurance_file_cnt", nNewInsuranceFileCnt,
                                                   gPMConstants.PMEParameterDirection.PMParamOutput,
                                                   gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("event", oInsuranceFile.EventDescription,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMString)
            m_lReturn = m_oDatabase.Parameters.Add("DiscountPercentage", oInsuranceFile.DiscountPercentage,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMDouble)
            m_lReturn = m_oDatabase.Parameters.Add("DiscountReasonID", oInsuranceFile.DiscountReasonID,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("MatchDiscountedPremiumFlag", oInsuranceFile.MatchDiscountedPremiumFlag,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("DiscountedPremium", oInsuranceFile.DiscountedPremium,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMDouble)
            m_lReturn = m_oDatabase.Parameters.Add("DiscountRecurringTypeId", oInsuranceFile.DiscountRecurringTypeId,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMDouble)
            m_lReturn = m_oDatabase.Parameters.Add("alternate_reference", oInsuranceFile.AlternateReference,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMString)
            m_lReturn = m_oDatabase.Parameters.Add("OriginalProductID", oInsuranceFile.OriginalProductID,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("ProductID", oInsuranceFile.ProductID,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("RenewalProductID", oInsuranceFile.RenewalProductID,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("CoverStartDate", oInsuranceFile.CoverStartDate,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add("ExpiryDate", oInsuranceFile.ExpiryDate,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add("RenewalDate", oInsuranceFile.RenewalDate,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add("InceptionTPI", oInsuranceFile.InceptionTPI,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add("AnniversaryDate", oInsuranceFile.AnniversaryDate,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add("AnniversaryCopy", oInsuranceFile.AnniversaryCopy,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("RenewalDayNumber", oInsuranceFile.RenewalDayNumber,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("PutOnNextInstalmentRenewal", oInsuranceFile.PutOnNextInstalmentRenewal,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("InceptionDate", oInsuranceFile.InceptionDate,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMDate)
            m_lReturn = m_oDatabase.Parameters.Add("EventDescription", oInsuranceFile.EventDescription,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMString)
            m_lReturn = m_oDatabase.Parameters.Add("UnderwritingYearID", oInsuranceFile.UnderwritingYearID,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("PolicyVersion", oInsuranceFile.PolicyVersion,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("UserId", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("Insurance_ref", If(bChanged, oInsuranceFile.InsuranceRef, DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMString)

            If Convert.ToString(oInsuranceFile.PaymentMethod).Trim.Length > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add("PaymentMethod", oInsuranceFile.PaymentMethod,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMString)

            ElseIf Convert.ToString(sPayment_Method).Trim.Length > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add("PaymentMethod", sPayment_Method,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMString)
            End If
            Dim oResultArray(,) As Object
            'now run the new stored procedure
            m_lReturn = m_oDatabase.SQLSelect("spu_sir_copy_policy_for_renewal_selection", "CopyPolicyForRenewal", True, vResultArray:=oResultArray, bKeepNulls:=True)
            ''m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_sir_copy_policy_for_renewal_selection",
            '                                     sSQLName:="CopyPolicyForRenewal",
            '                                     bStoredProcedure:=True,
            '                                     vResultArray:=oResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oInsuranceFile.RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                oInsuranceFile.CommitTrans()
                'If Informations.IsArray(oResultArray) Then
                '    nNewInsuranceFileCnt = CInt(oResultArray(0, 0))

                'End If
            End If
            'Etana batch renewal selection enhancements
            'Get the new insurance file count from the spu return value
            ' nNewInsuranceFileCnt = oInsuranceFile.InsuranceFileCnt _
            nNewInsuranceFileCnt = m_oDatabase.Parameters.Item("new_insurance_file_cnt").Value
            'get client and branch
            nInsuredCnt = oInsuranceFile.InsuredCnt
            nSourceID = oInsuranceFile.SourceID
            sInsuranceRef = oInsuranceFile.InsuranceRef
            r_lNewInsuranceFileCnt = nNewInsuranceFileCnt
            m_dInception_date_tpi = oInsuranceFile.InceptionTPI


            ' SSP-984
            'm_lReturn = CopyPolicyAssociates(nOldInsuranceFileCnt:=CInt(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)), _
            '                                                               nNewInsuranceFileCnt:=nNewInsuranceFileCnt)
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            'oInsuranceFile.RollbackTrans()
            'Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            'RWH(14/06/01) Copy coinsurance.


            'Copy agent commission.
            If Not (bIsTrueMonthlyPolicy And CDbl(r_vRenewalList(PMFieldPosLeadAllowConsolidatedCommission, v_lCount)) = 1 And CDbl(r_vRenewalList(PMFieldPosRenewalCount, v_lCount)) < 11) Then

                m_lReturn = CopyAgentCommission(v_lCurrentInsFileCnt:=CInt(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)), v_lNewInsFileCnt:=nNewInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                    sFailureCriterion = "Failed to copy agent commission"
                End If
            End If



            'RWH(22/08/01) Must set the Task in Tax as this is passed into stored procedures
            'as Mode. If it is wrong the new tax records will not be created.
            oTax = New bSIRRITax.Business
            m_lReturn = oTax.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)

            m_lReturn = oTax.SetProcessModes(vTask:=m_iTask, vTransactionType:="REN")

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword,
                                                      v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID,
                                                      v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID,
                                                      v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp,
                                                      v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007,
                                                      v_vBranch:=m_iSourceID, r_vUnderwriting:=oIsRI2007)

            If oIsRI2007 = "1" Then
                m_lReturn = CreateBusinessObject(r_oObject:=oReinsurance, v_sClassName:="bSIRReinsuranceRI2007.Form")
            Else
                m_lReturn = CreateBusinessObject(r_oObject:=oReinsurance, v_sClassName:="bSIRReinsurance.Form")
            End If

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd,
                                                     vTransactionType:="REN")
            m_lReturn = CreateBusinessObject(r_oObject:=oAgentCommission, v_sClassName:="bSirAgentCommission.Business")

            m_lReturn = CopyRiskData(r_vRenewalList:=r_vRenewalList, v_lCount:=v_lCount,
                                     v_lNewInsuranceFileCnt:=nNewInsuranceFileCnt,
                                     r_sFailureReason:=sFailureCriterion, r_lEligibleForRenewal:=lEligibleForRenewal,
                                     v_bIsTrueMonthlyPolicy:=bIsTrueMonthlyPolicy, r_oReinsurance:=oReinsurance,
                                     v_bTMPAnniversary:=v_bTMPAnniversary, r_bIsReferred:=bIsReferred)

            r_sFailureCriterion = ToSafeString(sFailureCriterion)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Always update policy premium to avoid magical figures during manual review

                'm_lReturn = oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=nNewInsuranceFileCnt)
                'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    'Leave alone. Failure is anyway due to copy risk data
                'End If

                oAgentCommission.InsuranceFileCnt = nNewInsuranceFileCnt
                'The Get deletes the existing records, and recalculates them
                'but does _not_ write them back to the database.  The calculate does...
                If Not (bIsTrueMonthlyPolicy And CDbl(r_vRenewalList(PMFieldPosLeadAllowConsolidatedCommission, v_lCount)) = 1 And CDbl(r_vRenewalList(PMFieldPosRenewalCount, v_lCount)) < 11) Then
                    m_lReturn = oAgentCommission.CalculateAgentCommission(v_lInsuranceFileCnt:=nNewInsuranceFileCnt, v_sTransactionType:="REN", r_vntResult:=oArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                        sFailureCriterion = "Failed to do Agent Commission"
                    End If
                End If

                If sFailureCriterion = "" Then
                    sFailureCriterion = "Failed to copy risk data"
                Else
                    sFailureCriterion = "Failed to copy risk data (" & sFailureCriterion & ")"
                End If

                r_sFailureCriterion = sFailureCriterion

                If nRenewalStatusTypeID = 0 Then
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                End If
            Else

                ' default to continue with process
                bContinue = True

                'PN 67489 -Always update policy premium to avoid magical figures during manual review

                'm_lReturn = oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=nNewInsuranceFileCnt)
                'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    'Leave alone. Failure is anyway due to copy risk data
                'End If

                ' if the renewal policy is subject to a policy discount
                If oInsuranceFile.DiscountRecurringTypeId = kDiscountRecurringTypeIdPolicy Then

                    ' apply any discount that is defined to the NEW renewal quote (not the original policy)
                    m_lReturn = ApplyPolicyDiscount(nNewInsuranceFileCnt, oInsuranceFile.ProductID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' if the apply discount process failed then we dont want to continue
                        ' so drop out of the process
                        bContinue = False

                        ' indicate this item needs to be manually reviewed.
                        If sFailureCriterion = "" Then
                            sFailureCriterion = "Failed to apply policy discount"
                        Else
                            sFailureCriterion = "Failed to apply policy discount (" & sFailureCriterion & ")"
                        End If

                        r_sFailureCriterion = sFailureCriterion

                        If nRenewalStatusTypeID = 0 Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                        End If
                    End If
                End If

                If bContinue Then

                    'Check Auto-renewal eligibility.
                    If CheckRenewalCriteria(r_vRenewalList, v_lCount, bIsOnlyAgentTransfer) = gPMConstants.PMEReturnCode.PMFalse Then
                        sFailureCriterion = m_sFailureCriterion

                        If r_sFailureCriterion <> "" Then
                            r_sFailureCriterion = r_sFailureCriterion & ", " & sFailureCriterion
                        Else
                            r_sFailureCriterion = sFailureCriterion
                        End If

                        m_sFailureCriterion = ""
                        lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue Then

                        m_lReturn = IsQuoted(v_lInsuranceFileCnt:=nNewInsuranceFileCnt, r_lIsQuoted:=lIsQuoted)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            lIsQuoted = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If lIsQuoted = gPMConstants.PMEReturnCode.PMFalse Then
                            lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
                            m_lReturn = AddRenewalReport(v_sReportType:="ManualRenewal",
                                                         v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                                                         v_vPolicyNumber:=r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                                         v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                                         v_vCoverStartDate:=
                                                            r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                                                         v_vCoverEndDate:=r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                                         v_vProductCode:=r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                                         v_vFailureCriterion:=PMIsQuotedDesc, v_vFailureDetail:="",
                                                         v_vInsuranceFileCnt:=
                                                            CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)))
                        End If
                    End If

                    'If not eligible for renewal, either because a risk failed rating OR
                    'policy level auto-renewal criteria were failed, then set RenewalStatusType
                    'and message appropriately.
                    If lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse Then
                        If bIsOnlyAgentTransfer AndAlso Not bIsReferred Then
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAutoRated
                        Else
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                        End If
                        'Do agent commission.
                        'Enhancement 35643 Populate Agent Commission on Renewal Version in 'Manual Review' status
                        m_lReturn = CreateBusinessObject(r_oObject:=oAgentCommission, v_sClassName:="bSirAgentCommission.Business")
                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            oAgentCommission.InsuranceFileCnt = nNewInsuranceFileCnt
                            'The Get deletes the existing records, and recalculates them
                            'but does _not_ write them back to the database.  The calculate does...
                            If Not (bIsTrueMonthlyPolicy And CDbl(r_vRenewalList(PMFieldPosLeadAllowConsolidatedCommission, v_lCount)) = 1 And CDbl(r_vRenewalList(PMFieldPosRenewalCount, v_lCount)) < 11) Then
                                m_lReturn = oAgentCommission.CalculateAgentCommission(v_lInsuranceFileCnt:=nNewInsuranceFileCnt, v_sTransactionType:="REN", r_vntResult:=oArray)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    nResult = gPMConstants.PMEReturnCode.PMFalse
                                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                                    sFailureCriterion = "Failed to do Agent Commission"
                                End If
                            End If
                        End If

                        If Not (oAgentCommission Is Nothing) Then
                            m_lReturn = oAgentCommission.Terminate
                            oAgentCommission = Nothing
                        End If
                        If sFailureCriterion = "" Then
                            sFailureCriterion = "Manual Review - Not every risk is quoted"
                        End If

                        If r_sFailureCriterion <> "" Then
                            r_sFailureCriterion = r_sFailureCriterion & ", " & sFailureCriterion
                        Else
                            r_sFailureCriterion = sFailureCriterion
                        End If
                    End If
                    m_lReturn = IsQuoted(v_lInsuranceFileCnt:=nNewInsuranceFileCnt, r_lIsQuoted:=lIsQuoted)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                    End If
                End If
                nResult = gPMConstants.PMEReturnCode.PMTrue
                If bIsTrueMonthlyPolicy And Not v_bTMPAnniversary Then
                    'Set status to say we are ready to print the renewal notice.
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAwaitUpdate
                ElseIf nRenewalStatusTypeID <> gPMConstants.PMBRenewalStatusTypeManualReview Then
                    'Set status to say we are ready to print the renewal notice.
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAutoRated
                End If

                'Reset RiskId to ensure Policy level reinsurance is done.
                oReinsurance.InsuranceFileCnt = nNewInsuranceFileCnt
                oReinsurance.RiskId = 0

                m_lReturn = oReinsurance.Getdetails

                'Do policy taxes.
                oTax.InsuranceFileCnt = nNewInsuranceFileCnt

                m_lReturn = oTax.GetInsuranceFileTax(oInsuranceFileTax, sDescription)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                    sFailureCriterion = "Failed to do Policy Taxes"

                    If r_sFailureCriterion <> "" Then
                        r_sFailureCriterion = r_sFailureCriterion & ", " & sFailureCriterion
                    Else
                        r_sFailureCriterion = sFailureCriterion
                    End If
                End If
                'Update policy premium.
                oChangePolicyStatus = New bSIRChangePolicyStatus.Business
                m_lReturn = oChangePolicyStatus.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = oChangePolicyStatus.UpdatePolicyPremium(
                                    v_lInsuranceFileCnt:=nNewInsuranceFileCnt)
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        CreateRenewalPolicy = gPMConstants.PMEReturnCode.PMFalse
                        nRenewalStatusTypeID = PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                        sFailureCriterion = "Failed to update Policy Premium"
                    End If
                End If
            End If

            If (Not (oChangePolicyStatus Is Nothing)) Then
                oChangePolicyStatus.Dispose()
                oChangePolicyStatus = Nothing
            End If
            ' create any fees that apply for the renewal policy
            m_lReturn = ApplyPolicyFee(nNewInsuranceFileCnt, sFailureCriterion, oInsuranceFile.ProductID,
                                       r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                sFailureCriterion = "Failed to calculate renewal fees"

                If r_sFailureCriterion <> "" Then
                    r_sFailureCriterion = r_sFailureCriterion & ", " & sFailureCriterion
                Else
                    r_sFailureCriterion = sFailureCriterion
                End If
            End If

            'Update policy premium.
            'If Not (oChangePolicyStatus Is Nothing) Then
            '    m_lReturn = oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=nNewInsuranceFileCnt)
            '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '        nResult = gPMConstants.PMEReturnCode.PMFalse
            '        nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
            '        sFailureCriterion = "Failed to update Policy Premium"
            '    End If
            'End If

            'If Not (oChangePolicyStatus Is Nothing) Then
            '    m_lReturn = oChangePolicyStatus.Terminate
            '    oChangePolicyStatus = Nothing
            'End If

            'Do agent commission.

            oAgentCommission = New bSirAgentCommission.Business
            m_lReturn = oAgentCommission.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_odatabase)


            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                oAgentCommission.InsuranceFileCnt = nNewInsuranceFileCnt
                'The Get deletes the existing records, and recalculates them
                'but does _not_ write them back to the database.  The calculate does...
                If _
                    Not _
                    (bIsTrueMonthlyPolicy AndAlso
                     CDbl(r_vRenewalList(PMFieldPosLeadAllowConsolidatedCommission, v_lCount)) = 1 AndAlso
                     CDbl(r_vRenewalList(PMFieldPosRenewalCount, v_lCount)) < 11) Then
                    If CheckRecalculateCommission() Then

                        'Similar to RenewalSelectionByPolicy via BO
                        m_lReturn = CopyAgentCommission(v_lCurrentInsFileCnt:=r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount),
                                                        v_lNewInsFileCnt:=nNewInsuranceFileCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                            sFailureCriterion = "Failed to copy agent commission"

                            If r_sFailureCriterion <> "" Then
                                r_sFailureCriterion = r_sFailureCriterion & ", " & sFailureCriterion
                            Else
                                r_sFailureCriterion = sFailureCriterion
                            End If
                        End If

                        m_lReturn =
                            oAgentCommission.CalculateAgentCommission(
                                v_lInsuranceFileCnt:=nNewInsuranceFileCnt, v_sTransactionType:="REN",
                                r_vntResult:=oArray)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview _
                            'Awaiting Manual Review
                            sFailureCriterion = "Failed to do Agent Commission"

                            If r_sFailureCriterion <> "" Then
                                r_sFailureCriterion = r_sFailureCriterion & ", " & sFailureCriterion
                            Else
                                r_sFailureCriterion = sFailureCriterion
                            End If
                        End If
                    Else
                        m_lReturn =
                            oAgentCommission.CopyPolicyCommission(
                                r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount), nNewInsuranceFileCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            CreateRenewalPolicy = gPMConstants.PMEReturnCode.PMFalse
                            nRenewalStatusTypeID = PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                            sFailureCriterion = "Failed to copy Commission"

                            If r_sFailureCriterion <> "" Then
                                r_sFailureCriterion = r_sFailureCriterion & ", " & sFailureCriterion
                            Else
                                r_sFailureCriterion = sFailureCriterion
                            End If
                        End If
                    End If
                End If
            End If

            If Not (oAgentCommission Is Nothing) Then
                oAgentCommission.Dispose()
                oAgentCommission = Nothing
            End If

            If Not bIsTrueMonthlyPolicy Then
                ' Is the premium zero ?
                m_lReturn = IsPremiumZero(v_lInsuranceFileCnt:=nNewInsuranceFileCnt,
                                          r_bIsPremiumZero:=bIsPremiumZero)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                    sFailureCriterion = "Failed to check IsPremiumZero"

                    If r_sFailureCriterion <> "" Then
                        r_sFailureCriterion = r_sFailureCriterion & ", " & sFailureCriterion
                    Else
                        r_sFailureCriterion = sFailureCriterion
                    End If
                Else
                    'If the premium is zero then fail and let the user know why in the report
                    If bIsPremiumZero Then
                        nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview _
                        'Awaiting Manual Review
                        sFailureCriterion = "Premium Is Zero"

                        If r_sFailureCriterion <> "" Then
                            r_sFailureCriterion = r_sFailureCriterion & ", " & sFailureCriterion
                        Else
                            r_sFailureCriterion = sFailureCriterion
                        End If
                    End If
                End If
            End If

            If _
                CStr(r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount)) <> "" OrElse
                r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount) Is DBNull.Value Then
                m_lReturn = IsAgentCancelled(
                    v_lPartyCnt:=CInt(r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount)),
                    r_lIsCancelled:=lIsAgentCancelled)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                    sFailureCriterion = "IsAgentCancelled Failed"

                    If r_sFailureCriterion <> "" Then
                        r_sFailureCriterion = r_sFailureCriterion & ", " & sFailureCriterion
                    Else
                        r_sFailureCriterion = sFailureCriterion
                    End If
                ElseIf lIsAgentCancelled = gPMConstants.PMEReturnCode.PMTrue Then
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview 'Awaiting Manual Review
                    sFailureCriterion = "Agent has been cancelled"

                    If r_sFailureCriterion <> "" Then
                        r_sFailureCriterion = r_sFailureCriterion & ", " & sFailureCriterion
                    Else
                        r_sFailureCriterion = sFailureCriterion
                    End If
                End If
            End If



            sFailureMessage = ""
            If bPutOnnextInstalmentRenewal Then
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                nOriginalInsuranceFileCnt =
                    gPMFunctions.ToSafeLong(r_vRenewalList(PMFieldPosLatestInstalmentPlanInsuranceFileCnt, v_lCount), 0)
            Else
                nOriginalInsuranceFileCnt = CInt(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))
                'do we have instalment plan on this policy
                If (sOptionValue.ToString = "1") Then
                    oBatchRenewalBusiness = New bSIRRenewalBusiness()
                    oBatchRenewalBusiness.Initialise(sUsername:="", sPassword:="", iUserID:=0, iSourceID:=1, iLanguageID:=0, iCurrencyID:=0, iLogLevel:=0, sCallingAppName:=ACApp)
                    m_lReturn = oBatchRenewalBusiness.HasInstalmentPlanOnCurrentTerm(nInsuranceFileCnt:=oInsuranceFile.InsuranceFileCnt,
                                                                      sPaymentMethod:=sPayment_Method,
                                                                      nLatestInstalmentInsuranceFileCnt:=
                                                                         nInstalmentInsuranceFileCnt)

                    If oBatchRenewalBusiness IsNot Nothing Then
                        oBatchRenewalBusiness.Dispose()
                        oBatchRenewalBusiness = Nothing
                    End If

                    'If ToSafeInteger(nInstalmentInsuranceFileCnt) <> 0 Then
                    'm_lReturn = gPMConstants.PMEReturnCode.PMTrue
                    nOriginalInsuranceFileCnt = nInstalmentInsuranceFileCnt
                    'End If

                Else
                    If (CDbl(oUseNbPaymentTermAtRenSelection(0, 0)) = 0) Then
                        m_lReturn = IsInstalment(v_lInsuranceFileCnt:=(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)))
                    Else
                        m_lReturn = IsInstalment(v_lInsuranceFileCnt:=nInstalmentInsuranceFileCnt)
                    End If
                End If
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'create quote plan for the renewal version
                m_lReturn = CreateInstalmentQuote(v_lOriginalInsuranceFileCnt:=nOriginalInsuranceFileCnt,
                                                  v_lRenewalInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                  v_lPartyCnt:=nInsuredCnt, r_sFailureMessage:=sFailureMessage,
                                                      v_lProductId:=oInsuranceFile.ProductID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'mark it as manual renewal
                    nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                End If

            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                sFailureMessage = "Check for existing instalment plan failed for Policy ID " & CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))
            End If

            'if we didn't fail then check broker transfer portfolio
            If sFailureMessage = "" Then
                m_lReturn =
                    GetBrokerTransferPortfolioDetail(
                        v_lInsuranceFileCnt:=CInt(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)),
                        r_vResultArray:=oBrokerTransferPorfolio)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sFailureMessage = "Failed to check broker transfer portfolio"
                Else
                    If Informations.IsArray(oBrokerTransferPorfolio) Then
                        If gPMFunctions.ToSafeLong(oBrokerTransferPorfolio(1, 0), 0) = 1 Then
                            nBrokerXferStatusTypeID = nRenewalStatusTypeID
                            nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAwaitBrokerTransfer
                        End If
                    End If
                End If
            End If

            If sFailureMessage <> "" Then
                m_lReturn = AddRenewalReport(v_sReportType:="ManualRenewal",
                                             v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                                             v_vPolicyNumber:=r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                             v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                             v_vCoverStartDate:=r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                                             v_vCoverEndDate:=r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                             v_vProductCode:=r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                             v_vFailureCriterion:="", v_vFailureDetail:=sFailureMessage,
                                             v_vInsuranceFileCnt:=
                                                CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)))
            End If

            m_lReturn = AddRenewalStatus(v_lProductId:=CInt(r_vRenewalList(PMFieldPosProductID, v_lCount)),
                                         v_lRenewalStatusTypeID:=nRenewalStatusTypeID,
                                         v_lInsuranceHolderCnt:=
                                            CInt(r_vRenewalList(PMFieldPosInsuranceHolderCnt, v_lCount)),
                                         v_lInsuranceFileCnt:=CInt(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)),
                                         v_vLeadAgentCnt:=CStr(r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount)),
                                         v_lRenewalInsuranceFileCnt:=nNewInsuranceFileCnt,
                                         v_lBrokerXferStatusTypeID:=nBrokerXferStatusTypeID)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'as far as Silent Renewal is concerned its now in renewal
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If
            '1.12 Wr25
            ' get the Product Id of New Insurancefile cnt
            m_lReturn = SelectRenewalProduct(nNewInsuranceFileCnt, nRenewalProductId)
            If gPMFunctions.ToSafeLong(oInsuranceFile.ProductID) <> gPMFunctions.ToSafeLong(nRenewalProductId) Then
                If gPMFunctions.ToSafeLong(oInsuranceFile.RenewalProductID) <> nRenewalProductId Then
                    m_lReturn = UpdateRenewalProduct(
                        v_lInsuranceFileCnt:=CInt(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)),
                        v_lProductId:=nRenewalProductId)
                    m_lReturn = DeleteRenewal(nNewInsuranceFileCnt)
                    Return nResult
                End If
            End If
            'RWH(16/11/2000) if the policy is not elligible for renewal then we have already
            'added a record to the report.
            If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) AndAlso (lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue) _
                Then
                'add to Renewal_Report table
                'bRenewalStatusAutomatic = ((lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAutoRated) Or ((lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAwaitUpdate) And (Not v_bTMPAnniversary And bIsTrueMonthlyPolicy)))
                bRenewalStatusAutomatic = ((nRenewalStatusTypeID = PMBRenewalStatusTypeAutoRated) OrElse
                                           ((nRenewalStatusTypeID = PMBRenewalStatusTypeAwaitUpdate) AndAlso
                                            (v_bTMPAnniversary = False And bIsTrueMonthlyPolicy = True)) OrElse
                                           (nRenewalStatusTypeID = PMBRenewalStatusTypeAwaitBrokerTransfer))
                If sFailureCriterion <> "" OrElse bRenewalStatusAutomatic Then
                    m_lReturn = AddRenewalReport(
                        v_sReportType:=If(bRenewalStatusAutomatic, "AutoRenewal", "ManualRenewal"),
                        v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                        v_vPolicyNumber:=r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                        v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                        v_vCoverStartDate:=r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                        v_vCoverEndDate:=r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                        v_vProductCode:=r_vRenewalList(PMFieldPosProductCode, v_lCount),
                        v_vFailureCriterion:=sFailureCriterion,
                            v_vFailureDetail:="",
                        v_vInsuranceFileCnt:=CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            If r_sFailureCriterion <> "" Then
                If Not r_sFailureCriterion.StartsWith("Renewal - " & ToSafeString(sInsuranceRef).Trim & " - ") Then
                    sFailureCriterion = "Renewal - " & sInsuranceRef & " - " & r_sFailureCriterion
                Else
                    sFailureCriterion = r_sFailureCriterion
                End If

                ReDim oKeyArray(1, 1)
                oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "insurance_file_cnt"
                oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = nNewInsuranceFileCnt

                m_lReturn = AddTaskToWorkManager(v_sClientName:=CStr(r_vRenewalList(PMFieldPosClientName, v_lCount)),
                                                 v_sDescription:=sFailureCriterion,
                                                 v_dtDueDate:=Informations.DateAdd("ww", 2, DateTime.Today),
                                                 v_vKeyArray:=oKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If r_sFailureCriterion <> "" Then
                If Not r_sFailureCriterion.StartsWith("Renewal - " & ToSafeString(sInsuranceRef).Trim & " - ") Then
                    r_sFailureCriterion = "Renewal - " & sInsuranceRef & " - " & r_sFailureCriterion
                End If
            End If

            If Not (oInsuranceFile Is Nothing) Then
                oInsuranceFile.Dispose()
                oInsuranceFile = Nothing
            End If

            If Not (oInsuranceFileBusiness Is Nothing) Then
                oInsuranceFileBusiness.Dispose()
                oInsuranceFileBusiness = Nothing
            End If

            If Not (oTax Is Nothing) Then
                oTax.Dispose()
                oTax = Nothing
            End If

            If Not (oReinsurance Is Nothing) Then
                oReinsurance.Dispose()
                oReinsurance = Nothing
            End If

            ' Return result

            If (Not (oProduct Is Nothing)) Then
                oProduct.Dispose()
                oProduct = Nothing
            End If

            If Not (oPolicyNumMaint Is Nothing) Then
                oPolicyNumMaint.Dispose()
                oPolicyNumMaint = Nothing
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="CreateRenewalPolicy", r_lFunctionReturn:=nResult, excep:=ex)

        Finally
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' copy all Risks attached to OldInsuranceFileCnt to NewInsuranceFileCnt
    ''' copy all GIS details attached to each risk to NewInsuranceFileCnt
    ''' </summary>
    ''' <param name="r_vRenewalList"></param>
    ''' <param name="v_lCount"></param>
    ''' <param name="v_lNewInsuranceFileCnt"></param>
    ''' <param name="r_lEligibleForRenewal"></param>
    ''' <param name="r_sFailureReason"></param>
    ''' <param name="r_oReinsurance"></param>
    ''' <param name="v_bIsTrueMonthlyPolicy"></param>
    ''' <param name="v_bTMPAnniversary"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CopyRiskData(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Integer, ByVal v_lNewInsuranceFileCnt As Integer, ByRef r_lEligibleForRenewal As Integer, ByRef r_sFailureReason As String, ByRef r_oReinsurance As Object, Optional ByVal v_bIsTrueMonthlyPolicy As Boolean = False, Optional ByVal v_bTMPAnniversary As Boolean = False, Optional ByRef r_bIsReferred As Boolean = False) As Integer

        Dim nResult As Integer
        Dim nNewGisPolicyLinkID As Integer = 0
        Dim oGisPolicyLinkArray(,) As Object
        Dim oRiskArray(,) As Object = Nothing
        Dim nNewRiskCnt As Integer = 0
        Dim sXMLDataSetDef As String = String.Empty
        Dim sXMLDataSet As String = String.Empty
        Dim nOldPolicyBinderId As Integer = 0
        Dim nNewPolicyBinderId As Integer = 0
        Dim sFailureDetail As String
        Dim sFailureCriterion As String
        Dim sDescription As String = ""
        Dim nTransactionType As Integer = 0
        Dim nQuoteType As Integer = 0
        Dim nReinsPremiumOrSumInsured As Integer = 0
        Dim nReinsBand As Integer = 0
        Dim bIsRIValid As Boolean

        Dim sFile As String = ""
        Dim m_lReturn As gPMConstants.PMEReturnCode

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim bExtraRiskDetails As Boolean
        Dim oPerilAllocation As bSirPerilAllocation.Business
        Dim oRiskData As bSIRRiskData.Business
        Dim nOldInsuranceFileCnt As Integer = 0
        Dim bRerate As Boolean
        Dim bRecalcTax As Boolean
        Dim bRecalcRI As Boolean
        Dim bRecalcFees As Boolean
        Dim bValidRiskQuote As Boolean


        Const ACFieldPosRiskID As Integer = 0
        Const ACFieldPosGisScreenID As Integer = 21



        ' set default values
        bExtraRiskDetails = False
        eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
        eProductFamily = g_sProductFamily
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer

        nResult = gPMConstants.PMEReturnCode.PMTrue
        r_lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue

        oRiskData = New bSIRRiskData.Business
        m_lReturn = CType(oRiskData.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_odatabase),
                              gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sFailureReason = "Copy Risk"
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oRiskData.TransactionType = "REN"

        'get all risks associate with OldInsuranceFileCnt
        nOldInsuranceFileCnt = r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)
        If _
            oRiskData.GetRenewalRisk(v_lInsuranceFileCnt:=nOldInsuranceFileCnt, r_vResultArray:=oRiskArray) <>
            gPMConstants.PMEReturnCode.PMTrue Then
            r_sFailureReason = "Getting Risk"
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'do we have any risks
        If Not Informations.IsArray(oRiskArray) Then
            'RWH(16/11/2000) We do not need to report an error if there are no risks.
            r_sFailureReason = "No risks found"
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'loop thro and copy each risk details

        For lCount As Integer = 0 To oRiskArray.GetUpperBound(1)
            bValidRiskQuote = True

            'copy risk to NewInsuranceFileCnt
            'EM 20120819 This code needs refactoring into multiple methods very hard to maintain
            'We check the risk override rules to check if we have to rerate if we do not have to rerate the risk we will follow the new path else follow current
            m_lReturn = CheckRiskRenewalRules(oRiskArray(ACRiskPosFolder, lCount), bRerate, bRecalcTax, bRecalcRI,
                                              bRecalcFees)

            If bRerate OrElse v_bTMPAnniversary OrElse Not v_bIsTrueMonthlyPolicy Then
                m_lReturn = oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                               v_vRiskDetail:=oRiskArray, v_lPosNo:=lCount,
                                               r_lRiskCnt:=nNewRiskCnt,
                                               v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "Copy Risk"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'prepare details to copy GIS Stuff attached to current risk
                'get policy link detail
                'RWH(20/11/2000) Pass folder_cnt instead of file_cnt.

                m_lReturn =
                    oRiskData.GetGISPolicyLink(
                        v_lInsuranceFolderCnt:=r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount),
                        v_lRiskID:=oRiskArray(ACRiskPosCnt, lCount), r_vResultArray:=oGisPolicyLinkArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "GetGISPolicyLink"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'do we have any data
                Dim auxVar_2 As Object = oGisPolicyLinkArray(0, 0)

                If Not (Convert.IsDBNull(auxVar_2) Or Informations.IsNothing(auxVar_2)) Then
                    'Make sure GIS object present.

                    m_lReturn = CType(GIS_LoadFromDB(CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                     r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount),
                                                     CStr(oGisPolicyLinkArray(0, 0)), oRiskArray(0, lCount)),
                                      gPMConstants.PMEReturnCode) 'copy GIS details to NewInsuranceFileCnt
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "LoadFromDB"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'RWH(20/11/2000) REMEMBER we are storing folder_cnt in file_cnt field now !!!!!
                    'So we pass existing folder_cnt in for old and new file_cnt.

                    m_lReturn = CType(CopyDataSet(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                  r_lNewGISPolicyLinkId:=nNewGisPolicyLinkID,
                                                  r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet,
                                                  v_vOldGISPolicyLinkId:=oGisPolicyLinkArray(0, 0),
                                                  v_vOldInsuranceFileCnt:=
                                                     r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount),
                                                  v_vOldRiskID:=oRiskArray(0, lCount),
                                                  v_vNewInsuranceFileCnt:=
                                                     r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount),
                                                  v_vNewRiskID:=nNewRiskCnt),
                                      gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "CopyDataSet"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Initialise the Data Set with the Object/Properties
                    m_lReturn = CType(LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet),
                                      gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "LoadFromXML"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'RWH(28/02/2001)

                    m_lReturn = CType(GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim()),
                                      gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "SaveToDB"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Get Policy Binder Ids

                    m_lReturn = CType(GetPolicyBinderId(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                        v_lGISPolicyLinkId:=nNewGisPolicyLinkID,
                                                        r_lPolicyBinderId:=nNewPolicyBinderId),
                                      gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "GetPolicyBinderId"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = CType(GetPolicyBinderId(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                        v_lGISPolicyLinkId:=CInt(oGisPolicyLinkArray(0, 0)),
                                                        r_lPolicyBinderId:=nOldPolicyBinderId),
                                      gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "GetPolicyBinderId"
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'TN20010711 - start (run the renewal scripts)

                    m_lReturn = CType(DeleteOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                        v_lPolicyBinderId:=nNewPolicyBinderId),
                                      gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureReason = "DeleteOutputTable"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        bValidRiskQuote = False
                    End If
                    'Etana batch renewal selection enhancements
                    'the risk data array now contains whether there is a RENEWAL rule or not
                    'so we check the value and if not, don't execute the block of code for RENEWAL rule
                    If oRiskArray(kRiskRenRule, lCount) = 1 Then

                        nTransactionType = TransactionType
                        EncodeTransactionScreenAndType(nQuoteType, nTransactionType, 0, 5)

                        'run renewal script

                        m_lReturn = CType(GIS_NBQuote(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                      v_lQuoteType:=nQuoteType, r_sXMLDataSet:=sXMLDataSet,
                                                      r_sXMLDataSetDef:=sXMLDataSetDef),
                                          gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            If r_sFailureReason <> "" Then
                                r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                            End If
                            r_sFailureReason = r_sFailureReason & "Failed Renewal Script"
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = CType(GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim()),
                                          gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            If r_sFailureReason <> "" Then
                                r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                            End If
                            r_sFailureReason = r_sFailureReason & "SaveToDB"
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            bValidRiskQuote = False
                        End If

                        'Clear rating output variable before new test.
                        sFailureDetail = ""
                        'Check Output table to see if risk has been referred or declined.

                        m_lReturn =
                            CType(CheckOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                   v_lPolicyBinderId:=nNewPolicyBinderId,
                                                   r_sReasons:=sFailureDetail),
                                  gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            If r_sFailureReason <> "" Then
                                'oPerilAllocation.PopulateRatingSectioilureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                            End If
                            r_sFailureReason = r_sFailureReason & "CheckOutputTable"
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                    'm_lReturn = CType(CopyRiskStandardWordings(v_lOldPolicyBinderId:=nOldPolicyBinderId,
                    '                                           v_lNewPolicyBinderId:=nNewPolicyBinderId,
                    '                                           v_sDataModelCode:=
                    '                                              CStr(oGisPolicyLinkArray(4, 0)).Trim()),
                    '                  gPMConstants.PMEReturnCode)

                    m_lReturn = CType(CopyRiskStandardWordings(v_lOldPolicyBinderId:=nOldPolicyBinderId, v_lNewPolicyBinderId:=nNewPolicyBinderId, v_sDataModelCode:=CStr(oGisPolicyLinkArray.GetValue(4, 0)).Trim(), v_dtEffectiveDate:=CDate(r_vRenewalList(PMFieldPosCoverEndDate, v_lCount)).AddYears(1)), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "CopyRiskStandardWordings"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If (v_bTMPAnniversary And v_bIsTrueMonthlyPolicy) Or (Not v_bIsTrueMonthlyPolicy) Then
                        '************ INDEX LINKING GIS STUFF *******************
                        Dim auxVar As Object = oRiskArray(ACFieldPosGisScreenID, lCount)

                        If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then
                            'index link GIS

                            m_lReturn = CType(GisIndexLink(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                                           v_lRiskID:=CInt(oRiskArray(ACFieldPosRiskID, lCount)),
                                                           v_vGisScreenID:=
                                                              oRiskArray(ACFieldPosGisScreenID, lCount),
                                                           v_dtEffectiveDate:=
                                                              CDate(r_vRenewalList(PMFieldPosCoverStartDate,
                                                                                   v_lCount)).AddYears(1),
                                                           v_sGisDataModelCode:=
                                                              CStr(oGisPolicyLinkArray(4, 0)).Trim()),
                                              gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                If r_sFailureReason <> "" Then
                                    r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                                End If
                                r_sFailureReason = r_sFailureReason & "Index Link"
                                nResult = gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    End If

                    m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot,
                                                                   v_lPMEProductFamily:=eProductFamily,
                                                                   v_lPMERegSettingLevel:=eRegSettingLevel,
                                                                   v_sSettingName:="ExtraRiskData",
                                                                   r_sSettingValue:=sFile),
                                      gPMConstants.PMEReturnCode)

                    'if we have a valid return
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        If Val(sFile) = 1 Then
                            bExtraRiskDetails = True
                        End If
                    End If

                    'check to see if this option is in use
                    If bExtraRiskDetails Then

                        m_lReturn = oRiskData.CopyRSASumInsured(v_lOldPolicyLinkID:=oGisPolicyLinkArray(0, 0),
                                                                v_lNewPolicyLinkID:=nNewGisPolicyLinkID)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            If r_sFailureReason <> "" Then
                                r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                            End If
                            r_sFailureReason = r_sFailureReason & "CopyRSASumInsured"
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    m_lReturn = CType(DeleteOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                        v_lPolicyBinderId:=nNewPolicyBinderId),
                                      gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "DeleteOutputTable"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        bValidRiskQuote = False
                    End If
                    'Etana batch renewal selection enhancements
                    'the risk data array now contains whether there is a UAL rule or not
                    'so we check the value and if not, don't execute the block of code for UAL
                    If oRiskArray(kRiskUALRule, lCount) = 1 Then
                        EncodeTransactionScreenAndType(nQuoteType, nTransactionType, 0, 3)

                        'Check Underwriting Authority Limits.

                        m_lReturn = CType(GIS_NBQuote(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                      v_lQuoteType:=nQuoteType, r_sXMLDataSet:=sXMLDataSet,
                                                      r_sXMLDataSetDef:=sXMLDataSetDef),
                                          gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            If r_sFailureReason <> "" Then
                                r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                            End If
                            r_sFailureReason = r_sFailureReason & "Check Underwriting Authority Limits"
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = CType(GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim()),
                                          gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            If r_sFailureReason <> "" Then
                                r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                            End If
                            r_sFailureReason = r_sFailureReason & "SaveToDB"
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            bValidRiskQuote = False
                        End If

                        'Clear rating output variable before new test.
                        sFailureDetail = ""

                        'Check Output table to see if risk has been referred or declined.

                        m_lReturn =
                            CType(CheckOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                   v_lPolicyBinderId:=nNewPolicyBinderId,
                                                   r_sReasons:=sFailureDetail),
                                  gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            If r_sFailureReason <> "" Then
                                r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                            End If
                            r_sFailureReason = r_sFailureReason & "CheckOutputTable"
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                    m_lReturn = CType(DeleteOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                        v_lPolicyBinderId:=nNewPolicyBinderId),
                                      gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "DeleteOutputTable2"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    EncodeTransactionScreenAndType(nQuoteType, nTransactionType, 0, 1)
                    'Quote risk.

                    m_lReturn = CType(GIS_NBQuote(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                  v_lQuoteType:=nQuoteType, r_sXMLDataSet:=sXMLDataSet,
                                                  r_sXMLDataSetDef:=sXMLDataSetDef),
                                      gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Failed to Quote"
                        bValidRiskQuote = False
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = CType(GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim()),
                                      gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "SaveToDB"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                        bValidRiskQuote = False
                    End If

                    'Clear rating output variable before new test.
                    sFailureDetail = ""

                    'Check Output table to see if risk has been referred or declined.

                    m_lReturn = CType(CheckOutputTable(v_sDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                       v_lPolicyBinderId:=nNewPolicyBinderId,
                                                       r_sReasons:=sFailureDetail),
                                      gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "CheckOutputTable"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Set required params for PerilAllocation
                    oPerilAllocation = New bSirPerilAllocation.Business
                    m_lReturn = CType(oPerilAllocation.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_odatabase),
                                          gPMConstants.PMEReturnCode)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        oPerilAllocation.InsuranceFileCnt = v_lNewInsuranceFileCnt

                        oPerilAllocation.InsuranceFolderCnt = r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount)

                        oPerilAllocation.RiskID = nNewRiskCnt

                        oPerilAllocation.TransactionType = "REN"

                        'Do PerilAllocation/Rating Sections stuff.

                        m_lReturn = oPerilAllocation.PopulateRatingSections

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            If r_sFailureReason <> "" Then
                                r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                            End If
                            r_sFailureReason = r_sFailureReason & "PopulateRatingSections"
                            nResult = gPMConstants.PMEReturnCode.PMFalse
                            bValidRiskQuote = False
                        End If

                        'Update the risk premium

                        m_lReturn = oPerilAllocation.UpdateRisk
                        'Reinsurance should be copied whether rating has succeeded or not.
                    End If

                    If Not (oPerilAllocation Is Nothing) Then

                        oPerilAllocation.Dispose()
                        oPerilAllocation = Nothing
                    End If

                    ' Set RI properties

                    r_oReinsurance.InsuranceFileCnt = v_lNewInsuranceFileCnt

                    r_oReinsurance.RiskId = nNewRiskCnt

                    ' Calculate RI
                    If ((v_bIsTrueMonthlyPolicy AndAlso TransactionType = 10) AndAlso Not v_bTMPAnniversary) AndAlso ToSafeInteger(r_vRenewalList(PMFieldPosTMPAutoRenFAC, v_lCount)) = 1 Then
                        r_oReinsurance.TMPRiskCntUnderRenewal = ToSafeLong(oRiskArray(ACRiskPosCnt, lCount))
                    End If
                    'vidya start
                    m_lReturn = r_oReinsurance.CalculateRI
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Calculating Reinsurance"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Load details to validate

                    m_lReturn = r_oReinsurance.Getdetails
                    If _
                        m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso
                        m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Getting Reinsurance Details"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Update details, this will ensure minor rounding is handled

                    m_lReturn = r_oReinsurance.Update
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Updating Reinsurance"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Validate the reinsurance is all OK

                    m_lReturn = r_oReinsurance.ValidateBands(ToSafeInteger(nReinsPremiumOrSumInsured), ToSafeInteger(nReinsBand))
                    'vidya end
                    ' Must return true AND a zero for lReinsPremiumOrSumInsured
                    bIsRIValid = (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) AndAlso (nReinsPremiumOrSumInsured = 0)

                    If Not bIsRIValid Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Validating Reinsurance"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = ApplyRiskRenewalTaxes(nNewRiskCnt, v_lNewInsuranceFileCnt, r_sFailureReason, True, nOldInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = ApplyRiskFee(nNewRiskCnt, v_lNewInsuranceFileCnt, r_sFailureReason, True, nOldInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'set risk status to QUOTED if reinsurance is complete, Unquoted otherwise
                    If sFailureDetail <> "" Then

                        If (Trim(sFailureDetail).Substring(8)).ToUpper = "DECLINED" Then
                            m_lReturn = oRiskData.UpdateRiskStatus(v_lRiskCnt:=nNewRiskCnt,
                                                                        v_lRiskStatusID:=2)

                        ElseIf (Trim(sFailureDetail).Substring(8)).ToUpper = "REFERRED" Then

                            m_lReturn = oRiskData.UpdateRiskStatus(v_lRiskCnt:=nNewRiskCnt,
                                                                    v_lRiskStatusID:=1)
                        End If
                    Else
                        m_lReturn = oRiskData.UpdateRiskStatus(v_lRiskCnt:=nNewRiskCnt, v_lRiskStatusID:=If(bIsRIValid = True AndAlso bValidRiskQuote = True, 3, 4))
                    End If

                    If r_sFailureReason <> "" Then
                        r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10) & sFailureDetail
                    Else
                        r_sFailureReason = sFailureDetail
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureReason <> "" Then
                            r_sFailureReason = r_sFailureReason & Strings.ChrW(13) & Strings.ChrW(10)
                        End If
                        r_sFailureReason = r_sFailureReason & "Failed to update risk status"
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Not String.IsNullOrEmpty(sFailureDetail) Then
                        r_lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
                        sFailureCriterion = PMFailedReRateDesc

                        m_lReturn = CType(AddRenewalReport(v_sReportType:="ManualRenewal",
                                            v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                                            v_vPolicyNumber:=r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                            v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                            v_vCoverStartDate:=r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                                            v_vCoverEndDate:=r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                            v_vProductCode:=r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                                           v_vFailureCriterion:=sFailureCriterion,
                                                           v_vFailureDetail:=sFailureDetail,
                                            v_vInsuranceFileCnt:=CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))), gPMConstants.PMEReturnCode)
                    End If

                    If Not String.IsNullOrEmpty(sFailureDetail) AndAlso sFailureDetail.Contains("REFERRED") Then
                        r_bIsReferred = True
                    End If
                Else
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    r_sFailureReason = "No Gis detail"

                    ' Log Error Message

                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                    oDict.Add("v_lNewInsuranceFileCnt", v_lNewInsuranceFileCnt)
                    gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:="No Gis detail for InsuranceFileCnt:" &
                                                          CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)) &
                                                          " RiskID:" & CStr(oRiskArray(1, lCount)), vApp:=ACApp,
                                            vClass:=ACClass, vMethod:="CopyRiskData", oDicParms:=oDict)

                End If
            Else 'No only need to point to the risk
                Dim lCurrentRiskCnt As Integer = 0

                lCurrentRiskCnt = oRiskArray(ACRiskPosCnt, lCount)

                m_lReturn = oRiskData.AddRiskLink(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                                  v_lRiskCnt:=lCurrentRiskCnt,
                                                  v_sStatusFlag:="R",
                                    v_lOriginalRiskCnt:=0,
                                                  v_lRenewedRiskCnt:=lCurrentRiskCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureReason = "Failed to add risk link"
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                Else
                    'Apply Risk Taxes or recalc
                    m_lReturn = ApplyRiskRenewalTaxes(lCurrentRiskCnt, v_lNewInsuranceFileCnt, r_sFailureReason,
                                                      bRecalcTax, nOldInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Apply Risk Fees
                    m_lReturn = ApplyRiskFee(lCurrentRiskCnt, v_lNewInsuranceFileCnt, r_sFailureReason, bRecalcFees, nOldInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
            End If
        Next lCount

        If Not (oRiskData Is Nothing) Then

            oRiskData.Dispose()
            oRiskData = Nothing
        End If

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: GetRenewalSelectionDetails (Private)
    '
    ' Description:get Single policy detail which is not in Renewal Status table or has the status of "Policy Details Changed"
    '                   and expiry date is within range, specified in the Product table and user entered date.
    '                   add these to Renewal_Report table
    'History: 11/06/2008 Created Pankaj

    ' ***************************************************************** '
    Public Function GetRenewalSelectionDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSourceID As Integer, ByVal v_dtCompareDate As Date, ByRef r_vResultArray(,) As Object,
                                               ByVal v_vStartDate As Object, ByRef v_bIgnoreDate As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRenewalSelectionDetails"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'clear parameter list and add in required parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=v_lSourceID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="compare_date", vValue:=v_dtCompareDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            'didn't do cdate(v_vStartDate) because vb is too stupid to know that the first part  of the IF expression is true

            'm_lReturn = m_oDatabase.Parameters.Add(sName:="Start_Date", vValue:=CStr(If(Informations.IsNothing(v_vStartDate) Or Convert.IsDBNull(v_vStartDate) Or Informations.IsNothing(v_vStartDate), DBNull.Value, v_vStartDate)), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If Informations.IsNothing(v_vStartDate) OrElse Convert.IsDBNull(v_vStartDate) Then
                result = m_oDatabase.Parameters.Add(sName:="Start_Date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                result = m_oDatabase.Parameters.Add(sName:="Start_Date", vValue:=v_vStartDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="userid", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            If v_bIgnoreDate Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="ignoredate", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                'Check for any error
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception
                End If
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRenewalSelectionDetailsSQL, sSQLName:=ACGetRenewalSelectionDetailsName, bStoredProcedure:=ACGetRenewalSelectionDetailsStored, vResultArray:=r_vResultArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetTrueMonthlyPolicyDates
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetTrueMonthlyPolicyDates(ByVal v_bMidnightRenewal As Boolean, ByVal v_bTMPAnniversary As Boolean, ByVal v_lCount As Integer, ByRef r_vRenewalList(,) As Object, ByRef r_dtCoverStartDate As Date, ByRef r_dtExpiryDate As Date, ByRef r_dtRenewalDate As Date, ByRef r_dtAnniversaryDate As Date, ByRef r_lRenewalDayNumber As Integer, ByRef r_lAnniversaryCopy As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTrueMonthlyPolicyDates"



        Dim dtOriginalRenewalDate, dtOriginalAnniversaryDate As Date
        Dim lOriginalRenewalDayNumber As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            dtOriginalRenewalDate = CDate(r_vRenewalList(PMFieldPosRenewalDate, v_lCount))
            dtOriginalAnniversaryDate = gPMFunctions.ToSafeDate(r_vRenewalList(PMFieldPosAnniversaryDate, v_lCount))

            lOriginalRenewalDayNumber = CInt(r_vRenewalList(PMFieldPosRenewalDayNumber, v_lCount))
            r_lRenewalDayNumber = lOriginalRenewalDayNumber

            If Not v_bTMPAnniversary Then

                ' cover start date
                r_dtCoverStartDate = dtOriginalRenewalDate

                ' New Renewal Date = Renewal Date + 1 Month Aligned to Day Number
                r_dtRenewalDate = GetClosestDate(lOriginalRenewalDayNumber, dtOriginalRenewalDate.Month + 1, dtOriginalRenewalDate.Year)

                If v_bMidnightRenewal Then
                    r_dtExpiryDate = r_dtRenewalDate.AddDays(-1)
                Else
                    r_dtExpiryDate = r_dtRenewalDate
                End If

                ' The new renewals anniversary date = polcies anniversary date
                r_dtAnniversaryDate = dtOriginalAnniversaryDate

                ' Anniversary Copy = 0
                r_lAnniversaryCopy = 0

            Else

                ' Cover Start Date = Anniversary Date
                r_dtCoverStartDate = dtOriginalAnniversaryDate

                ' Expiry Date = Anniversary Date + 1 Month Aligned to Renewal Day Number
                r_dtRenewalDate = GetClosestDate(lOriginalRenewalDayNumber, dtOriginalAnniversaryDate.Month + 1, dtOriginalAnniversaryDate.Year)

                ' If This Is a Midnight Renewal then the Expiry Date (Cover To Date) = Renewal Date - 1 Day
                If v_bMidnightRenewal Then
                    r_dtExpiryDate = r_dtRenewalDate.AddDays(-1)
                Else
                    ' Expiry Date (Cover To Date) = Renewal Date
                    r_dtExpiryDate = r_dtRenewalDate
                End If

                ' Anniversary Date = Anniversary Date + 1 Year
                r_dtAnniversaryDate = dtOriginalAnniversaryDate.AddYears(1)

                ' Anniversary Copy = 1
                r_lAnniversaryCopy = 1

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClosestDate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-10-2005 : Process ID
    ' ***************************************************************** '
    Public Function GetClosestDate(ByVal v_lDay As Integer, ByVal v_lMonth As Integer, ByVal v_lYear As Integer) As Date

        Dim result As Date = DateTime.FromOADate(0)
        Const kMethodName As String = "GetClosestDate"

        Dim lNewMonth As Integer
        Dim dtSerial As Date
        Dim dtMonthStart As Date

        Try

            result = DateTime.FromOADate(gPMConstants.PMEReturnCode.PMTrue)

            If v_lMonth > 12 Then
                v_lMonth = 1
                v_lYear += 1
            End If

            ' serialise the date from the passed data
            dtSerial = Informations.DateSerial(v_lYear, v_lMonth, v_lDay)

            ' get the month from the newly serialised date
            lNewMonth = dtSerial.Month

            ' if the month of the new date doesnt match the
            ' month passed in then
            If lNewMonth <> v_lMonth Then

                dtMonthStart = Informations.DateSerial(v_lYear, lNewMonth, 1)

                dtSerial = dtMonthStart.AddDays(-1)

            End If

            ' return the serialised date
            ' or the last day in the specified month
            result = dtSerial


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=False, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: EncodeTransactionScreenAndType
    '
    ' Description: Encodes Transaction, Screen id and tYpe from encoded value
    '              Originally TTTSSYY
    '              Now        1TTTSSSSYY
    '
    ' History: 19/12/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function EncodeTransactionScreenAndType(ByRef r_lEncoded As Integer, ByRef r_lTransactionType As Integer, ByRef r_lGISScreenId As Object, ByRef r_lQuoteType As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EncodeTransactionScreenAndType"

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass &
                        ".EncodeTransactionScreenAndType")

        Try

            'new format 1TTTSSSSYY
            r_lEncoded = 1000000000 + (r_lTransactionType * 1000000) + (r_lGISScreenId * 100) + r_lQuoteType

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass &
                        ".EncodeTransactionScreenAndType")


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Private Function AddBatchRenewalJobRuns(ByVal v_lBatchRenewalJobID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_dRunDate As Date, ByVal v_sFailureReason As String, ByVal v_sDocumentPrinted As Object, ByVal v_iIsFailed As Integer, ByVal v_sGUID As String, ByRef r_lRecordsCount As Integer, ByRef r_lBatchRenewalJobRunsID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddBatchRenewalJobRuns"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLBeginTrans()

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_id", vValue:=CStr(v_lBatchRenewalJobID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            'developer guide no. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="run_date", vValue:=v_dRunDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="failure_reason", vValue:=v_sFailureReason, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_printed", vValue:=CStr(v_sDocumentPrinted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_failed", vValue:=CStr(v_iIsFailed), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="GUID", vValue:=v_sGUID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Batch_Renewal_Job_Runs_ID", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Record_Count", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddBatchRenewalJobRunsSQL, sSQLName:=ACAddBatchRenewalJobRunsName, bStoredProcedure:=ACAddBatchRenewalJobRunsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.SQLCommitTrans()

            r_lBatchRenewalJobRunsID = m_oDatabase.Parameters.Item("Batch_Renewal_Job_Runs_ID").Value
            r_lRecordsCount = m_oDatabase.Parameters.Item("Record_Count").Value

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If



        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            m_lReturn = m_oDatabase.SQLRollbackTrans()
        Finally
        End Try

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PrintRenewalReport
    '
    ' Description: Print Renewal Reports
    '
    ' ***************************************************************** '
    Public Function PrintRenewalReport(ByVal v_iReportSortOrder As Integer) As Integer

        Dim result As Integer = 0
        Dim oReport As Object = Nothing
        Dim vReportKeys As Object
        Dim bManualRenewalsExist, bAutoRenewalsExist As Boolean
        Dim sCompiledReportPath, sReportTitle As Object
        Dim vDefaultValues As Object
        Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed

        Const kMethodName As String = "PrintRenewalReport"
        Const PMReportAutoRenewal As String = "AutomaticRenewal"
        Const PMReportManualRenewal As String = "ManualRenewal"
        Const ACSpoolReportMode As Integer = 5
        Const kDocumentTypeID As Integer = 5

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bManualRenewalsExist = False
            bAutoRenewalsExist = False

            'If oReport Is Nothing Then
 '    oReport = New bSIRReportPrint.Business
 '    m_lReturn = oReport.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_odatabase)

 '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
 '        Throw New Exception
 '    End If
 'End If

        result = gPMComponentServices.CreateBusinessObject(r_oObject:=oReport, v_sClassName:="bSIRReportPrint.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Dim r_sMessage As String = "Failed to create an instance of bSIRReportPrint.Business"
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bSIRReportPrint.Business", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
            Return result
        End IF

            If Not (oReport Is Nothing) Then

                'RWH(24/05/2001) Check whether report records exist before displaying reports.
                m_lReturn = RenewalsReportExists("ManualRenewal", bManualRenewalsExist)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed

            m_lReturn = oDocManagerWrapper.InitialiseBusiness(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


            If bManualRenewalsExist Then

                oReport.reportName = PMReportManualRenewal

                m_lReturn = oReport.GetParameters(r_vParameters:=vReportKeys, r_vDefaultValues:=vDefaultValues)

                If v_iReportSortOrder = 1 Then

                    vReportKeys(1, 1) = "Client"
                Else

                    vReportKeys(1, 1) = "Policy Number"
                End If

                sReportTitle = "Manual Renewal"

                'oReport.PrintReport = AC_PRINT_ONLY

                'm_lReturn = oReport.SendToPrint(v_sReportTitle:=sReportTitle, r_sCompiledReportPath:=sCompiledReportPath, v_vParameters:=vReportKeys)
                m_lReturn = oReport.ExportToDisk(r_ExportFile:=sCompiledReportPath, v_iFormatType:=0, v_vParameters:=vReportKeys)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception
                End If

                oDocManagerWrapper.Mode = ACSpoolReportMode

                oDocManagerWrapper.DocumentTypeId = kDocumentTypeID

                m_lReturn = oDocManagerWrapper.SpoolDocument(v_sDesc:=sReportTitle, v_sDocName:=sCompiledReportPath)

            End If

            m_lReturn = RenewalsReportExists("AutoRenewal", bAutoRenewalsExist)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If bAutoRenewalsExist Then

                oReport.reportName = PMReportAutoRenewal

                m_lReturn = oReport.GetParameters(r_vParameters:=vReportKeys, r_vDefaultValues:=vDefaultValues)

                If v_iReportSortOrder = 1 Then

                    vReportKeys(1, 1) = "Client"
                Else

                    vReportKeys(1, 1) = "Policy Number"
                End If

                sReportTitle = "Automatic Renewal"

                'oReport.PrintReport = AC_PRINT_ONLY

                '	m_lReturn = oReport.SendToPrint(v_sReportTitle:=sReportTitle, r_sCompiledReportPath:=sCompiledReportPath, v_vParameters:=vReportKeys)
                m_lReturn = oReport.ExportToDisk(r_ExportFile:=sCompiledReportPath, v_iFormatType:=0, v_vParameters:=vReportKeys)

                oDocManagerWrapper.Mode = ACSpoolReportMode

                oDocManagerWrapper.DocumentTypeId = kDocumentTypeID

                m_lReturn = oDocManagerWrapper.SpoolDocument(v_sDesc:=sReportTitle, v_sDocName:=sCompiledReportPath)

            End If

            oReport.Dispose()

            oReport = Nothing

            oDocManagerWrapper.Dispose()

            oDocManagerWrapper = Nothing

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Public Function UpdatePolicyRenewalStatus(ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Try

            Return UpdatePolicyStatus(v_lInsuranceFileCnt)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyRenewalStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyRenewalStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetNextPolicyVersion
    '
    ' Description:
    '
    ' History: 21/01/2003 sj - Created.
    ' RAM20030225   : Changed the embedded SQL into Stored Procedure
    ' ***************************************************************** '
    Public Function GetNextPolicyVersion(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPolicyVersion As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lMaxVersionNo As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                result = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Developer Guide No.85
                result = .Parameters.Add(sName:="max_version_no", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                result = .SQLSelect(sSQL:=ACGetMaxPolicyVersionNoSQL, sSQLName:=ACGetMaxPolicyVersionNoName, bStoredProcedure:=ACGetMaxPolicyVersionNoStored)

                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    lMaxVersionNo = .Parameters.Item("max_version_no").Value
                End If

            End With

            'Increment by 1
            r_lPolicyVersion = lMaxVersionNo + 1

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextPolicyVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextPolicyVersion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetRenewalSelectionPolicyDetails(ByVal lInsurance_file_cnt As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insFileCnt", vValue:=CStr(lInsurance_file_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACGetPolicyDetailsSQL, sSQLName:=ACGetPolicyDetailsName, bStoredProcedure:=True, vResultArray:=vResultArray)

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewalSelectionPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalSelectionPolicyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetBatchJobPrintingOptions(ByVal v_lBatchRenewalJobID As Integer, ByRef r_iRenewalDocDestination As Integer, ByVal r_iReportSortOrder As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBatchJobPrintingOptions"

        Dim vResultArray(,) As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue

        r_iRenewalDocDestination = 0
        r_iReportSortOrder = 0

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Batch_Renewal_Job_Id", vValue:=CStr(v_lBatchRenewalJobID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBatchJobPrintingOptionsSQL, sSQLName:=ACGetBatchJobPrintingOptionsName, bStoredProcedure:=ACGetBatchJobPrintingOptionsStored, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        If Informations.IsArray(vResultArray) Then
            r_iRenewalDocDestination = gPMFunctions.ToSafeLong(vResultArray(0, 0))
            r_iReportSortOrder = gPMFunctions.ToSafeLong(vResultArray(1, 0))
        End If
        Return result
    End Function

    Public Function UpdateRenewalStatus(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sRenewalStatusTypeCode As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateRenewalStatus"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="RENIFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Statuscode", vValue:=v_sRenewalStatusTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRENStatusSQL, sSQLName:=ACUpdateRENStatusName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Public Function BatchRenewalJobSelect(ByVal v_lBatchRenewalJobID As Integer, ByRef r_lBatchRenewalJobTypeId As Integer) As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object = Nothing
        Const kMethodName As String = "BatchRenewalJobSelect"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_oDatabase.Parameters.Add(sName:="batch_renewal_job_id", vValue:=CStr(v_lBatchRenewalJobID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACBatchRenewalJobSelSQL, sSQLName:=ACBatchRenewalJobSelName, bStoredProcedure:=True, vResultArray:=vResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                If Informations.IsArray(vResult) Then
                    r_lBatchRenewalJobTypeId = gPMFunctions.ToSafeLong(vResult(6, 0))
                End If
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Private Function GetPriorTermSchemeInsuranceFile(ByVal lInsuranceFileCnt As Integer,
                                                    ByRef lPriorTermSchemeInsuranceFileCnt As Integer, ByVal v_bUsePriorTermSchemeAtRenewal As Boolean) As Integer

        Const kMethodName As String = "GetPriorTermSchemeInsuranceFile"
        Dim vResultArray(,) As Object = Nothing
        Try

            GetPriorTermSchemeInsuranceFile = gPMConstants.PMEReturnCode.PMTrue

            'clear parameter list and add in required parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                                    vValue:=lInsuranceFileCnt,
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="use_prior_term_scheme_at_ren",
                                               vValue:=v_bUsePriorTermSchemeAtRenewal,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMBoolean)



            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPriorTermSchemeInsuranceFileSQL,
                                                     sSQLName:=ACGetPriorTermSchemeInsuranceFileName,
                                                     bStoredProcedure:=True,
                                                 vResultArray:=vResultArray)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vResultArray) Then
                Throw New Exception
            Else
                lPriorTermSchemeInsuranceFileCnt = ToSafeLong(vResultArray(0, 0), 0)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetPriorTermSchemeInsuranceFile, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Function

    ''' <summary>
    ''' Updates the Last Transaction Details in Insurance File System.
    ''' </summary>
    ''' <param name="nNewInsuranceFileCnt">Insurance File Count Id</param>
    ''' <returns>This will return 1, when it is executed successfully. Otherwise will return any other integer value.</returns>
    ''' <remarks></remarks>
    Public Function UpdateInsuranceFileSystem(ByRef nNewInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim vResultArray1(,) As Object = Nothing
        Dim lTransTypeId As Integer
        Dim sTransTypeDescription As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the required transaction details first.
            sSQL = "SELECT transaction_type_id, description" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM Transaction_Type" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE LTRIM(RTRIM(code)) = 'REN'"

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTransactionTypeDetails", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If Not Information.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            End If

            lTransTypeId = CInt(vResultArray(0, 0))

            sSQL = "Select insurance_file_type_id from Insurance_File Where insurance_file_cnt = " & CStr(nNewInsuranceFileCnt)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL,
                                                    sSQLName:="SelectLastTransDescription",
                                                    bStoredProcedure:=False,
                                                    vResultArray:=vResultArray1)

            If Information.IsArray(vResultArray1) Then
                If vResultArray1(0, 0) = 3 Then
                    sTransTypeDescription = "Renewals"
                End If
            End If
            If sTransTypeDescription = "" Then
                sTransTypeDescription = vResultArray(1, 0)
            End If

            sSQL = "UPDATE Insurance_File_System" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SET modified_by_id = " & m_iUserID & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ", last_modified = {last_modified}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ",last_trans_date = {last_trans_date}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ",last_trans_type_id = " & lTransTypeId & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ",last_trans_description = '" & sTransTypeDescription & "'" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM insurance_file_system ifs" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE ifs.insurance_file_cnt = " & nNewInsuranceFileCnt

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="last_modified", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="last_trans_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateInsuranceFileSystem", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update Insurance_File_System with renewal event", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileSystem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End Try

        Return result
    End Function

    ' ***************************************************************** '
    ' Name :GetInsFolderFromInsRef
    '
    ' Desc :
    '
    ' Hist :
    ' ***************************************************************** '
    Public Function GetInsFolderFromInsRef(ByVal v_sInsuranceRef As String,
                                                        ByRef r_vResultArray(,) As Object) As Long

        Try

            GetInsFolderFromInsRef = gPMConstants.PMEReturnCode.PMTrue

            'get all versions of this policy which are in renewal
            m_oDatabase.Parameters.Clear()

            GetInsFolderFromInsRef = m_oDatabase.Parameters.Add(sName:="insurance_ref",
                                                                                    vValue:=v_sInsuranceRef,
                                                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                                                iDataType:=gPMConstants.PMEDataType.PMString)

            If GetInsFolderFromInsRef <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Function
            End If

            'parameters are added sucessfully
            GetInsFolderFromInsRef = m_oDatabase.SQLSelect(sSQL:=ACGetInsFolderFromInsRefSQL,
                                                    sSQLName:=ACGetInsFolderFromInsRefName,
                                                    bStoredProcedure:=True,
                                                vResultArray:=r_vResultArray)


            Exit Function

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetInsFolderFromInsRef", r_lFunctionReturn:=GetInsFolderFromInsRef, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Function
    ''' <summary>
    ''' Gets the risks renewal override flags
    ''' </summary>
    ''' <param name="v_lRenewalRiskCnt"></param>
    ''' <param name="v_lRenwalInsuranceFileCnt"></param>
    ''' <param name="r_sFailureReason"></param>
    ''' <param name="v_bRecalcTaxes"></param>
    ''' <param name="v_lOriginalInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ApplyRiskRenewalTaxes(ByVal v_lRenewalRiskCnt As Integer,
                                           ByVal v_lRenwalInsuranceFileCnt As Integer,
                                           ByRef r_sFailureReason As String,
                                           Optional ByVal v_bRecalcTaxes As Boolean = True,
                                           Optional ByVal v_lOriginalInsuranceFileCnt As Integer = 0) As Integer

        Const kMethodName As String = "ApplyRiskRenewalTaxes"
        Dim oTax As bSIRRITax.Business
        Dim sDescription As String = String.Empty
        Dim oRiskTax As Object
        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            oTax = New bSIRRITax.Business
            m_lReturn = oTax.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_odatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = String.Concat(r_sFailureReason, " ApplyRiskRenewalTaxes Failed to create bSirRiskTax")
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                nResult = m_lReturn
                'Clean up
                If (Not (oTax Is Nothing)) Then
                    oTax.Dispose()
                    oTax = Nothing
                End If
                Return nResult
                Exit Function
            End If

            oTax.InsuranceFileCnt = v_lRenwalInsuranceFileCnt
            oTax.RiskCnt = v_lRenewalRiskCnt
            oTax.TransactionType = "REN"
            oTax.Task = 2

            If v_bRecalcTaxes Then
                m_lReturn = oTax.GetRiskTax(oRiskTax, sDescription)
            Else
                m_lReturn = oTax.CopyRiskTax(v_lRenewalRiskCnt, v_lOriginalInsuranceFileCnt)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = String.Concat(r_sFailureReason, "GetRiskTax")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return m_lReturn
        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ApplyRiskRenewalTaxes, excep:=ex)
            Return m_lReturn
        Finally
            'Clean up
            If (Not (oTax Is Nothing)) Then
                oTax.Dispose()
                oTax = Nothing
            End If

        End Try
    End Function
    ''' <summary>
    ''' Apply risk fees to policy
    ''' </summary>
    ''' <param name="v_lRenewalRiskCnt"></param>
    ''' <param name="v_lRenwalInsuranceFileCnt"></param>
    ''' <param name="r_sFailureReason"></param>
    ''' <param name="v_bRecalcFees"></param>
    ''' <param name="v_lOriginalInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ApplyRiskFee(ByVal v_lRenewalRiskCnt As Object,
                                  ByVal v_lRenwalInsuranceFileCnt As Object,
                                  ByRef r_sFailureReason As String,
                                  Optional ByVal v_bRecalcFees As Boolean = True,
                                  Optional ByVal v_lOriginalInsuranceFileCnt As Object = 0) As Integer

        Const kMethodName As String = "ApplyRiskFee"

        Dim oFee As Object
        Dim sDescription As String = String.Empty

        Try

            ApplyRiskFee = gPMConstants.PMEReturnCode.PMTrue

            ' create fee object
            m_lReturn = CreateBusinessObject(r_oObject:=oFee,
                                             v_sClassName:="bSIRPartyFee.UBusiness")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = String.Concat(r_sFailureReason, " ApplyRiskFee Failed to create bSIRPartyFee")
                ApplyRiskFee = gPMConstants.PMEReturnCode.PMFalse
                'Clean up
                If (Not (oFee Is Nothing)) Then
                    oFee.Dispose()
                    oFee = Nothing
                End If
                Exit Function
            End If

            If v_bRecalcFees Then
                m_lReturn = oFee.RecalculateRiskFees(v_lInsuranceFileCnt:=v_lRenwalInsuranceFileCnt,
                                                     v_lRiskCnt:=v_lRenewalRiskCnt,
                                                     v_lTransactionTypeId:=TransactionType)
            Else
                m_lReturn = oFee.CopyRiskFees(v_lRenewalRiskCnt, v_lRenwalInsuranceFileCnt, v_lRenewalRiskCnt,
                                              v_lOriginalInsuranceFileCnt)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = String.Concat(r_sFailureReason, "GetRiskTax")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return m_lReturn
        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ApplyRiskFee, excep:=ex)
            Return m_lReturn
        Finally
            'Clean up
            If (Not (oFee Is Nothing)) Then
                oFee.Dispose()
                oFee = Nothing
            End If

        End Try
    End Function

    ''' <summary>
    ''' Gets the risks renewal override flags
    ''' </summary>
    ''' <param name="v_lRenwalInsuranceFileCnt">Insurance file that the risk version belongs to</param>
    ''' <param name="r_sFailureReason">Return message that is if there is a failure</param>
    ''' <param name="v_lOriginalInsuranceFileCnt">An optional parameter for the insruance file from which the fees must be copied</param>
    ''' <returns>The success of the method</returns>
    Private Function ApplyPolicyFee(ByVal v_lRenwalInsuranceFileCnt As Object,
                                    ByRef r_sFailureReason As String,
                                    Optional ByVal v_lProductId As Object = 0,
                                    Optional ByVal v_lOriginalInsuranceFileCnt As Object = 0) As Integer

        Const kMethodName As String = "ApplyPolicyFee"

        Dim oFee As Object
        Dim sDescription As String = String.Empty
        Dim bRecalc As Boolean

        Try

            ApplyPolicyFee = gPMConstants.PMEReturnCode.PMTrue
            bRecalc = True
            If Informations.IsArray(oPolicyRenewalRules) Then
                bRecalc = oPolicyRenewalRules(ACBatchJobStartColumns.RecalculateFees, 0)
            End If

            ' create fee object
            m_lReturn = CreateBusinessObject(r_oObject:=oFee,
                                             v_sClassName:="bSIRPartyFee.UBusiness")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = String.Concat(r_sFailureReason, " ApplyPolicyFee Failed to create bSIRPartyFee")
                ApplyPolicyFee = gPMConstants.PMEReturnCode.PMFalse
                'Clean up
                If (Not (oFee Is Nothing)) Then
                    oFee.Dispose()
                    oFee = Nothing
                End If
                Exit Function
            End If

            If bRecalc Then
                m_lReturn = oFee.RecalculatePolicyFees(v_lInsuranceFileCnt:=v_lRenwalInsuranceFileCnt,
                                                       v_lProductId:=v_lProductId,
                                                       v_lTransactionTypeId:=TransactionType)
            Else
                m_lReturn = oFee.CopyPolicyFees(v_lRenwalInsuranceFileCnt, v_lOriginalInsuranceFileCnt)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureReason = String.Concat(r_sFailureReason, "GetRiskTax")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ApplyPolicyFee, excep:=ex)
        Finally
            'Clean up
            If (Not (oFee Is Nothing)) Then
                oFee.Dispose()
                oFee = Nothing
            End If

        End Try
    End Function

    ''' <summary>
    ''' Gets a value indicating whether the commission can be copied or must be recalculated
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckRecalculateCommission() As Boolean
        CheckRecalculateCommission = True



        If Informations.IsArray(oPolicyRenewalRules) Then
            CheckRecalculateCommission = oPolicyRenewalRules(ACBatchJobStartColumns.RecalculateCommission, 0)
            Return CheckRecalculateCommission
        End If

    End Function

    ''' <summary>
    ''' Gets the risks renewal override flags
    ''' </summary>
    ''' <param name="v_lRiskFolder">The risk folder for the risk that is being renewed</param>
    ''' <param name="r_bRerate">A value indicating whether the risk must be rerated</param>
    ''' <param name="r_bRecalcTax">A value indicating whether the risk taxes must be recalculated</param>
    ''' <param name="r_bRecalculateRI">A value indicating whether the risk reinsurance must be recalculated</param>
    ''' <param name="r_bRecalculateFees">A value indicating whether the risk fees must be recalculated</param>
    ''' <returns>The success of the method</returns>
    Public Function CheckRiskRenewalRules(ByVal v_lRiskFolder As Long, ByRef r_bRerate As Boolean,
                                          ByRef r_bRecalcTax As Boolean, ByRef r_bRecalculateRI As Boolean,
                                          ByRef r_bRecalculateFees As Boolean) As Long


        'Default is always to rerate the risk
        r_bRerate = True
        r_bRecalcTax = False
        r_bRecalculateRI = False
        r_bRecalculateFees = False

        If Informations.IsArray(oRenewalRiskRules) Then
            'Find the risk
            For iCount As Integer = 0 To oRenewalRiskRules.GetUpperBound(1)
                If oRenewalRiskRules(ACRiskProcessingParamatersColumns.RiskFolderCnt, iCount) = v_lRiskFolder Then
                    r_bRerate = oRenewalRiskRules(ACRiskProcessingParamatersColumns.Rerate, iCount)
                    r_bRecalcTax = oRenewalRiskRules(ACRiskProcessingParamatersColumns.RecalculateTaxes, iCount)
                    r_bRecalculateRI = oRenewalRiskRules(ACRiskProcessingParamatersColumns.RecalculateReinsurance,
                                                         iCount)
                    r_bRecalculateFees = oRenewalRiskRules(ACRiskProcessingParamatersColumns.RecalculateFees, iCount)
                    Exit For
                End If
            Next
        End If
        CheckRiskRenewalRules = gPMConstants.PMEReturnCode.PMTrue
    End Function
    ''' <summary>
    ''' ProcessRenewalSelectionBatch
    ''' </summary>
    ''' <param name="v_lBatchId"></param>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="r_bSuccess"></param>
    ''' <param name="r_sMessage"></param>
    ''' <param name="r_lNewInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessRenewalSelectionBatch(ByVal v_lBatchId As Integer, ByVal v_lInsuranceFolderCnt As Integer,
                                                 ByRef r_bSuccess As Boolean, ByRef r_sMessage As String,
                                                 ByRef r_lNewInsuranceFileCnt As Integer) As Integer

        Const kMethodName As String = "ProcessRenewalSelectionBatch"

        Dim oPMLock As bpmlock.User
        Dim sLockedBy As String = String.Empty
        Dim nInsuranceFileCnt As Integer = 0
        Dim nNewInsuranceFileCnt As Integer = 0
        Dim nBatchRenewalJobID As Integer = 0
        Dim oRenewalList As Object
        Dim nCount As Integer = 0
        Dim nRecordsCount As Integer = 0
        Dim nBatchRenewalJobRunsID As Integer = 0
        Dim nReturn As Integer = 0
        Dim sBatchRef As String = String.Empty
        Dim nIsFailed As Integer = 0
        Try
            ' We going to start assuming success any failures should set the paramaters
            ProcessRenewalSelectionBatch = gPMConstants.PMEReturnCode.PMTrue
            r_bSuccess = True
            r_sMessage = ""
            nBatchRenewalJobID = -1
            r_lNewInsuranceFileCnt = -1

            nReturn = GetInsuranceFileFromBatch(v_lBatchId, v_lInsuranceFolderCnt, nInsuranceFileCnt)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failled to retrieve insurance file for the batch"
                GoTo AfterError
            End If

            'Marks the job as in progress and get the paramaters
            nReturn = LoadRenewalInsurancefolderParamaters(v_lBatchId, v_lInsuranceFolderCnt, nInsuranceFileCnt)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failled to load batch processing rules"
                GoTo AfterError
            End If
            nBatchRenewalJobID = oPolicyRenewalRules(ACBatchJobStartColumns.BatchRenewalJobId, 0)
            sBatchRef = oPolicyRenewalRules(ACBatchJobStartColumns.BatchRef, 0)

            'Loads the risk processing details
            nReturn = LoadRenewalRiskParamaters(v_lBatchId, v_lInsuranceFolderCnt)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failled to load risk processing rules"
                GoTo AfterError
            End If

            'Get bPMLock
            oPMLock = New bpmlock.User
            nReturn = oPMLock.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=m_odatabase)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failled to get policy lock object"
                GoTo AfterError
            End If

            'Lock the Key
            nReturn = oPMLock.LockKey(sKeyName:="RENSEL", vKeyValue:=nInsuranceFileCnt, iUserID:=m_iUserID,
                                      sCurrentlyLockedBy:=sLockedBy$)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "This record has been locked by " & sLockedBy
                GoTo AfterError
            End If

            'get policy details that needs renewal
            nReturn = GetRenewalSelectionDetails(v_lInsuranceFileCnt:=nInsuranceFileCnt, v_lSourceID:=m_iSourceID, v_dtCompareDate:=CDate(DateTime.Now.ToString("dd MMMM yyyy")),
                                                 r_vResultArray:=oRenewalList, v_vStartDate:=Nothing, v_bIgnoreDate:=True)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failled to get renewal selection details"
                GoTo AfterError
            End If

            'Do we have any data ?
            If Not Informations.IsArray(oRenewalList) Then
                r_sMessage = "Renewal selection return no data"
                GoTo AfterError
            End If

            nCount = 0
            m_lPolicyVersionIncrement = 0

            nReturn = CreateRenewalPolicyWrapper(oRenewalList, nCount, nNewInsuranceFileCnt, r_sMessage)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If Len(r_sMessage) = 0 Then r_sMessage = "Failled to CreateRenewalPolicyWrapper"
                GoTo AfterError
            End If

            nReturn = CreateTMPAnniversaryRenewal(oRenewalList, nCount)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failled to create anniversary renewal"
                GoTo AfterError
            End If
            If Trim(r_sMessage) = "" Then
                nIsFailed = 0
            Else
                nIsFailed = 1
            End If
            If nBatchRenewalJobID <> -1 Then
                nReturn = AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=nBatchRenewalJobID,
                                                 v_lInsuranceFileCnt:=nInsuranceFileCnt,
                                                 v_dRunDate:=CDate(DateTime.Now.ToString("dd MMMM yyyy")),
                                                 v_sFailureReason:=r_sMessage,
                                                 v_sDocumentPrinted:="",
                                                 v_iIsFailed:=nIsFailed,
                                                 v_sGUID:=sBatchRef,
                                                 r_lRecordsCount:=nRecordsCount,
                                                 r_lBatchRenewalJobRunsID:=nBatchRenewalJobRunsID)

            End If

            r_lNewInsuranceFileCnt = nNewInsuranceFileCnt

AfterError:
        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessRenewalSelectionBatch, excep:=ex)
            r_bSuccess = False

            If Len(r_sMessage) = 0 Then
                r_sMessage = Informations.Err.Description
            End If

            If Len(r_sMessage) = 0 Then
                r_sMessage = "Fails to Process Renewal Selection"
            End If

            nReturn = CompleteRenewalStep(v_lBatchId, v_lInsuranceFolderCnt, r_bSuccess, -1, r_sMessage) _
            'Ignore any errors from this as we can just submit the batch again

            If nBatchRenewalJobID <> -1 Then
                nReturn = AddBatchRenewalJobRuns(v_lBatchRenewalJobID:=nBatchRenewalJobID,
                                                 v_lInsuranceFileCnt:=nInsuranceFileCnt,
                                                 v_dRunDate:=CDate(DateTime.Now.ToString("dd MMMM yyyy")),
                                                 v_sFailureReason:=r_sMessage,
                                                 v_sDocumentPrinted:="",
                                                 v_iIsFailed:=1,
                                                 v_sGUID:=sBatchRef,
                                                 r_lRecordsCount:=nRecordsCount,
                                                 r_lBatchRenewalJobRunsID:=nBatchRenewalJobRunsID)
            End If

            If Not oPMLock Is Nothing Then
                'UnLock the Key
                nReturn = oPMLock.UnLockKey(sKeyName:="RENSEL",
                                            vKeyValue:=nInsuranceFileCnt,
                                            iUserID:=m_iUserID)

                oPMLock.Dispose()
                oPMLock = Nothing
            End If

            Exit Function

        Finally

            nReturn = CompleteRenewalStep(v_lBatchId, v_lInsuranceFolderCnt, True, nNewInsuranceFileCnt, r_sMessage) _
            'Ignore any errors from this as we can just submit the batch again

            'UnLock the Key
            nReturn = oPMLock.UnLockKey(sKeyName:="RENSEL",
                                        vKeyValue:=nInsuranceFileCnt,
                                        iUserID:=m_iUserID)

            oPMLock.Dispose()
            oPMLock = Nothing

        End Try
    End Function

    ''' <summary>
    ''' Loads the renewal policy paramaters
    ''' </summary>
    ''' <param name="v_lBatchId">The currnt batch that is being processed</param>
    ''' <param name="v_lInsurancefolderCnt">Insurance folder that is being renewed</param>
    ''' <param name="v_lInsuranceFileCnt">Insurance file that is being renewed</param>
    ''' <returns>The success of the method</returns>
    Public Function LoadRenewalInsurancefolderParamaters(ByVal v_lBatchId As Long, ByVal v_lInsuranceFolderCnt As Long,
                                                         ByVal v_lInsuranceFileCnt As Long) As Long

        Try

            LoadRenewalInsurancefolderParamaters = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=v_lBatchId,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=v_lInsuranceFolderCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kStartBatchForInsuranceFolderSQL,
                                              sSQLName:=kStartBatchForInsuranceFolderName, bStoredProcedure:=True,
                                              vResultArray:=oPolicyRenewalRules, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LoadRenewalInsurancefolderParamaters = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If Not Informations.IsArray(oPolicyRenewalRules) Then
                LoadRenewalInsurancefolderParamaters = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If oPolicyRenewalRules.GetUpperBound(0) <> ACBatchJobStartColumns.BatchRef Then
                LoadRenewalInsurancefolderParamaters = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

        Catch ex As Exception

            LoadRenewalInsurancefolderParamaters = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRenewalInsurancefolderParamaters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRenewalInsurancefolderParamaters", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)

            Exit Function
        End Try
    End Function


    ''' <summary>
    ''' Completed the renewal for this batch/insurance folder
    ''' </summary>
    ''' <param name="v_lBatchId">The currnt batch that is being processed</param>
    ''' <param name="v_lInsurancefolderCnt">Insurance folder that is being renewed</param>
    ''' <param name="v_bSuccess">A value indicating whether the process was successful</param>
    ''' <param name="v_sMessage">message for the renewal</param>
    ''' <param name="v_lInsuranceFileCnt">Insurance file that is being renewed</param>
    ''' <returns>The success of the method</returns>
    Public Function CompleteRenewalStep(ByVal v_lBatchId As Long, ByVal v_lInsuranceFolderCnt As Long, ByVal v_bSuccess As Boolean, ByVal v_lInsuranceFileCnt As Long, ByVal v_sMessage As String) As Long

        Try

            CompleteRenewalStep = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=v_lBatchId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=v_lInsuranceFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_renewal_job_run_status_id", vValue:=If(v_bSuccess, BatchRunStatus.CompletedSuccess, BatchRunStatus.CompletedFailed), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If v_lInsuranceFileCnt <> -1 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="new_insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
            If Len(v_sMessage) > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="message", vValue:=v_sMessage, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=kCompleteBatchForInsuranceFolderSQL, sSQLName:=kCompleteBatchForInsuranceFolderName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CompleteRenewalStep = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

        Catch ex As Exception

            CompleteRenewalStep = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CompleteRenewalStep Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CompleteRenewalStep", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function
    ''' <summary>
    ''' Loads the renewal risks paramater if there are any specified for this step of the batch
    ''' </summary>
    ''' <param name="v_lBatchId">The risk folder for the risk that is being renewed</param>
    ''' <param name="v_lInsurancefolderCnt">A value indicating whether the risk must be rerated</param>
    ''' <returns>The success of the method</returns>
    Public Function LoadRenewalRiskParamaters(ByVal v_lBatchId As Long, ByVal v_lInsuranceFolderCnt As Long) As Long

        Try

            LoadRenewalRiskParamaters = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=v_lBatchId,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=v_lInsuranceFolderCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetRiskProcessingParamatersSQL,
                                              sSQLName:=kGetRiskProcessingParamatersName, bStoredProcedure:=True,
                                              vResultArray:=oRenewalRiskRules)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LoadRenewalRiskParamaters = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

        Catch ex As Exception

            LoadRenewalRiskParamaters = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRenewalRiskParamaters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRenewalRiskParamaters", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)

            Exit Function
        End Try
    End Function

    ''' <summary>
    ''' Validates that the insurance file cnt stored on the batch job is correct for renewal or retrieve the latest one if not specified
    ''' </summary>
    ''' <param name="lBatchId">The batch id for this job</param>
    ''' <param name="lInsuranceFolderCnt">The insurance folder that is being renewed</param>
    ''' <param name="lInsuranceFileCnt">The insurance file that is must be renewed</param>
    ''' <returns>The success of the method</returns>
    Public Function GetInsuranceFileFromBatch(ByVal lBatchId As Long, ByVal lInsurancefolderCnt As Long,
                                              ByRef lInsuranceFileCnt As Long) As Long

        Try

            Dim oInsuranceFileData(,) As Object

            GetInsuranceFileFromBatch = gPMConstants.PMEReturnCode.PMTrue
            lInsuranceFileCnt = 0

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=lBatchId,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=lInsurancefolderCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetBatchInsuranceFileSQL, sSQLName:=kGetBatchInsuranceFileName,
                                              bStoredProcedure:=True, vResultArray:=oInsuranceFileData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GetInsuranceFileFromBatch = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If Not Informations.IsArray(oInsuranceFileData) Then
                GetInsuranceFileFromBatch = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If Not oInsuranceFileData.GetUpperBound(1) = 0 Then
                GetInsuranceFileFromBatch = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            lInsuranceFileCnt = oInsuranceFileData(0, 0)

            Exit Function

        Catch ex As Exception

            GetInsuranceFileFromBatch = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceFileFromBatch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFileFromBatch", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)

            Exit Function
        End Try
    End Function
    ''' <summary>
    ''' UpdateInsuranceFileCntForAnniversaryVersion
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_sInsuranceRef"></param>
    ''' <param name="v_dtCoverStartDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateInsuranceFileCntForAnniversaryVersion(ByVal v_lInsuranceFileCnt As Integer,
                                                                ByVal v_sInsuranceRef As String,
                                                                ByVal v_dtCoverStartDate As Date) As Integer

        Const kMethodName As String = "UpdateInsuranceFileCntForAnniversaryVersion"


        Try

            UpdateInsuranceFileCntForAnniversaryVersion = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add("insurance_file_cnt", v_lInsuranceFileCnt,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("insurance_ref", v_sInsuranceRef,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMString)
            m_lReturn = m_oDatabase.Parameters.Add("cover_start_date", v_dtCoverStartDate,
                                                   gPMConstants.PMEParameterDirection.PMParamInput,
                                                   gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLAction(
               sSQL:=kUpdateInsuranceFileCntForAnniversaryVersionSQL,
               sSQLName:=kUpdateInsuranceFileCntForAnniversaryVersionName,
               bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=UpdateInsuranceFileCntForAnniversaryVersion, excep:=ex)

            ' If you want to rollback a transaction or something, do it here


        End Try
    End Function

    ''' <summary>
    ''' GetPaymentTerms
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lPMUserID"></param>
    ''' <param name="r_bInvoiceEnabled"></param>
    ''' <param name="r_bInstalmentsEnabled"></param>
    ''' <param name="r_bPayNowEnabled"></param>
    ''' <param name="r_bBankGuaranteeEnabled"></param>
    ''' <param name="r_bCashDepositEnabled"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPaymentTerms(
                                     ByVal v_lInsuranceFileCnt As Integer,
                                     ByVal v_lPMUserID As Integer,
                                     ByRef r_bInvoiceEnabled As Boolean,
                                     ByRef r_bInstalmentsEnabled As Boolean,
                                     ByRef r_bPayNowEnabled As Boolean,
                                     Optional ByRef r_bBankGuaranteeEnabled As Boolean = False,
                                     Optional ByRef r_bCashDepositEnabled As Boolean = False) As Integer
        Const kMethodName As String = "GetPaymentTerms"

        Dim nReturn As Integer = 0
        Dim oResultArray(,) As Object = Nothing




        GetPaymentTerms = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt",
                                               vValue:=v_lInsuranceFileCnt,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserID",
                                               vValue:=v_lPMUserID,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="InvoiceEnabled",
                                               vValue:=r_bInvoiceEnabled,
                                               iDirection:=PMEParameterDirection.PMParamOutput,
                                               iDataType:=gPMConstants.PMEDataType.PMBoolean)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="InstalmentsEnabled",
                                               vValue:=r_bInstalmentsEnabled,
                                               iDirection:=PMEParameterDirection.PMParamOutput,
                                               iDataType:=gPMConstants.PMEDataType.PMBoolean)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="PaynowEnabled",
                                               vValue:=r_bPayNowEnabled,
                                               iDirection:=PMEParameterDirection.PMParamOutput,
                                               iDataType:=gPMConstants.PMEDataType.PMBoolean)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="BankGuaranteeEnabled",
                                               vValue:=r_bBankGuaranteeEnabled,
                                               iDirection:=PMEParameterDirection.PMParamOutput,
                                               iDataType:=gPMConstants.PMEDataType.PMBoolean)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="CashDepositEnabled",
                                               vValue:=r_bCashDepositEnabled,
                                               iDirection:=PMEParameterDirection.PMParamOutput,
                                               iDataType:=gPMConstants.PMEDataType.PMBoolean)

        ' Execute the stored procedure
        m_lReturn = m_oDatabase.SQLAction(
            sSQL:=kGetPaymentTermsSQL,
            sSQLName:=kGetPaymentTermsName,
            bStoredProcedure:=True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GetPaymentTerms = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Get the return parameteres
        r_bInvoiceEnabled = NullToBoolean(m_oDatabase.Parameters.Item("InvoiceEnabled").Value)
        r_bInstalmentsEnabled = NullToBoolean(m_oDatabase.Parameters.Item("InstalmentsEnabled").Value)
        r_bPayNowEnabled = NullToBoolean(m_oDatabase.Parameters.Item("PaynowEnabled").Value)
        r_bBankGuaranteeEnabled = NullToBoolean(m_oDatabase.Parameters.Item("BankGuaranteeEnabled").Value)
        'Start - Prakash - WPR85_Paralleling
        r_bCashDepositEnabled = NullToBoolean(m_oDatabase.Parameters.Item("CashDepositEnabled").Value)
        'End - Prakash - WPR85_Paralleling
        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                               vValue:=v_lInsuranceFileCnt,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.SQLSelect(
            sSQL:=kGetInstalmentSchemeSQL,
            sSQLName:=kGetInstalmentSchemeName,
            bStoredProcedure:=kGetInstalmentSchemeStored,
            vResultArray:=oResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        If NullToLong(oResultArray(0, 0)) <= 0 Then
            r_bInstalmentsEnabled = False
        End If

        m_oDatabase.Parameters.Clear()


    End Function

    Private Function GetLatestVersionOfDocumentTemplate(ByVal v_nDocTemplateId As Integer, ByVal v_dtEffectiveDate As DateTime, ByRef r_nLatestVersionDocTemplateId As Integer) As Integer

        Const kMethodName As String = "GetLatestVersionOfDocumentTemplate"
        Dim vResultArray As Object = Nothing
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = AddInputParameter(v_sName:="doc_template_id", v_vValue:=v_nDocTemplateId, v_iType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = AddInputParameter(v_sName:="effective_date", v_vValue:=v_dtEffectiveDate, v_iType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLSelect(
                                    sSQL:=ACGetLatestVersionOfDocumentTemplateSQL,
                                    sSQLName:=ACGetLatestVersionOfDocumentTemplateName,
                                    bStoredProcedure:=True,
                                    vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                If Informations.IsArray(vResultArray) Then
                    r_nLatestVersionDocTemplateId = ToSafeLong(vResultArray(0, 0))
                End If
            End If
            Return result


        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogError(
                 v_sUsername:=m_sUsername,
                 v_sClass:=ACClass,
                 v_sMethod:=kMethodName,
                 r_lFunctionReturn:=GetLatestVersionOfDocumentTemplate)

            Return result
        End Try
    End Function

    ''' <summary>
    ''' GetLatestPolicyVersion
    ''' </summary>
    ''' <param name="v_nInsuranceFolderCnt"></param>
    ''' <param name="r_oResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLatestPolicyVersion(ByVal v_nInsuranceFolderCnt As Integer,
                                                    ByRef r_oResultArray As Object) As Integer

        Dim nReturn As Integer
        Try
            nReturn = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt",
                                                            vValue:=v_nInsuranceFolderCnt,
                                                            iDirection:=PMEParameterDirection.PMParamInput,
                                                            iDataType:=PMEDataType.PMLong)

            If nReturn <> PMEReturnCode.PMTrue Then
                Return nReturn
            End If

            'parameters are added sucessfully
            nReturn = m_oDatabase.SQLSelect(sSQL:=kSelLatestPolicyVersionSQL,
                                                    sSQLName:=kSelLatestPolicyVersionName,
                                                    bStoredProcedure:=kSelLatestPolicyVersionStored,
                                                    vResultArray:=r_oResultArray)

            If nReturn <> PMEReturnCode.PMTrue Then
                Return nReturn
            End If

            If Not Informations.IsArray(r_oResultArray) Then
                nReturn = PMEReturnCode.PMNotFound
                Return nReturn
            Else

                If UBound(r_oResultArray, 2) > 0 Then
                    'can't have more than one version of policy
                    nReturn = PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed - more than one versions of policy is selected", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLatestPolicyVersion")

                    Return nReturn
                End If
            End If
        Catch ex As Exception
            nReturn = PMEReturnCode.PMError

        End Try
    End Function
    Public Function UpdatePaymentMethod(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            UpdatePaymentMethod = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACpdatePaymentMethodSQL, sSQLName:=ACpdatePaymentMethodName, bStoredProcedure:=ACUpdatePaymentMethodStored, vResultArray:=vResultArray, bKeepNulls:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePaymentMethod Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePaymentMethod", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' To copy Policy Associates from Previous version of Insurance File ' SSP-984
    ''' </summary>
    ''' <param name="nOldInsuranceFileCnt"></param>
    ''' <param name="nNewInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyPolicyAssociates(
            ByVal nOldInsuranceFileCnt As Integer,
            ByVal nNewInsuranceFileCnt As Integer) As Integer

        Dim nResult As Integer = 0

        nResult = gPMConstants.PMEReturnCode.PMTrue

        Try

            m_oDatabase.Parameters.Clear()
            nResult = m_oDatabase.Parameters.Add(sName:="Old_Insurance_File_Cnt",
                                                    vValue:=nOldInsuranceFileCnt,
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                    iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oDatabase.Parameters.Add(sName:="New_Insurance_File_Cnt",
                                                    vValue:=nNewInsuranceFileCnt,
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                    iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' Copy the Policy Associates
            Return m_oDatabase.SQLAction(sSQL:=ACCopyPolicyAssociatesSQL,
                                         sSQLName:=ACCopyPolicyAssociatesName,
                                         bStoredProcedure:=True)

        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicyAssociates Failed",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyAssociates", vErrNo:=Informations.Err().Number,
                                   vErrDesc:=ex.Message, excep:=ex)
            Return nResult

        End Try


    End Function

    Public Function GetDefaultPaymentTerms(ByVal nInsuranceFileCnt As Integer, Optional ByRef sDefaultPaymentMethod As String = "",
                                            Optional ByRef nDefaultInstalmentPlan As Integer = 0, Optional ByRef nDefaultInstalmentPlanVersion As Integer = 0,
                                            Optional ByRef nDefaultSchemeNumber As Integer = 0, Optional ByRef nDefaultSchemeVersion As Integer = 0,
                                            Optional ByRef nInstalmentInsuranceFileCnt As Integer = 0) As Integer
        Const kPaymentMethod As Integer = 0
        Const kInstalmentPlan As Integer = 1
        Const kInstalmentPlanVersion As Integer = 2
        Const kSchemeNumber As Integer = 3
        Const kSchemeVersion As Integer = 4
        Const kInstalmentInsuranceFileCnt As Integer = 5

        Dim vResultArray As Object = Nothing

        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsuranceFileCnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="bIsSelectionMode", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_oDatabase.SQLSelect(sSQL:=kGetDefaultPaymentTermsSQL, sSQLName:=kGetDefaultPaymentTermsName,
                                     bStoredProcedure:=True, vResultArray:=vResultArray,
                                     bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New Exception
            End If

            If Informations.IsArray(vResultArray) Then
                sDefaultPaymentMethod = vResultArray(kPaymentMethod, 0)
                nDefaultInstalmentPlan = vResultArray(kInstalmentPlan, 0)
                nDefaultInstalmentPlanVersion = vResultArray(kInstalmentPlanVersion, 0)
                nDefaultSchemeNumber = vResultArray(kSchemeNumber, 0)
                nDefaultSchemeVersion = vResultArray(kSchemeVersion, 0)
                nInstalmentInsuranceFileCnt = vResultArray(kInstalmentInsuranceFileCnt, 0)
            End If

            Return gPMConstants.PMEReturnCode.PMTrue
        Catch excep As System.Exception

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetDefaultPaymentTerms Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="GetDefaultPaymentTerms", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function

End Class
