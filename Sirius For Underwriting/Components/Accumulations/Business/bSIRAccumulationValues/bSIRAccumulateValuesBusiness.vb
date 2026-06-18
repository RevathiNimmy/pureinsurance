Option Strict Off
Option Explicit On
'developer guide no. 129
Imports System.Text
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 20/09/00
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              the accumulation values table
    '
    ' Edit History:
    '
    ' PN 16750      AG  18/11/2004  Put a condition in AddValues function to
    '                               check TotalSumInsured is not equal to zero.
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

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lInsuranceFileCnt As Integer
    Private m_bAnyFailed As Boolean

    'TN20010411 Start
    Private m_oRIBusiness As bSIRReinsurance.Form
    'TN20010411 End

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = CInt(Value)
        End Set
    End Property



    Public Property AnyFailed() As Boolean
        Get
            Return m_bAnyFailed
        End Get
        Set(ByVal Value As Boolean)

            m_bAnyFailed = CBool(Value)
        End Set
    End Property

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


        Dim result As Integer = 0
        Try
            'Todo
            m_oDatabase = New dPMDAO.Database

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




            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password


            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            'TN20010411 Start
            m_oRIBusiness = New bSIRReinsurance.Form
            m_lReturn = m_oRIBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'TN20010411 End

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
                If m_oRIBusiness IsNot Nothing Then
                    m_oRIBusiness.Dispose()
                    m_oRIBusiness = Nothing
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
    ' Name: RepopulateAccumValues
    '
    ' Description: Function which controls the repopulation of the
    '              accumulation_values table
    '
    ' ***************************************************************** '
    Public Function RepopulateAccumValues() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vInsurance(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'delete accumulation values
            m_lReturn = DeleteValues()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                m_lReturn = CommitTrans()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Get all insurance files with risks that are accumulated
            sSQL = "SELECT DISTINCT i.insurance_file_cnt, i.insurance_ref" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM insurance_file i," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "risk r," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "insurance_file_risk_link ifr" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE ifr.insurance_file_cnt = i.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND ifr.risk_cnt = r.risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND r.is_accumulated = 1" & Strings.ChrW(13) & Strings.ChrW(10)

            If InsuranceFileCnt <> 0 Then
                sSQL = sSQL & "AND i.insurance_file_cnt = " & CStr(InsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetInsurance", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vInsurance)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vInsurance) Then
                'DO NOT have a message box in a business object
                '        MsgBox ("There are no risks to accumulate")
                Return gPMConstants.PMEReturnCode.PMTrue
            End If


            For i As Integer = vInsurance.GetLowerBound(1) To vInsurance.GetUpperBound(1)
                m_lReturn = BeginTrans()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'add accumulation values for 1 insurance file


                m_lReturn = AddValues(CStr(vInsurance(0, i)), CStr(vInsurance(1, i)))

                'If this was an unforeseen error we will return false from addvalues
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                Else
                    m_lReturn = CommitTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RepopulateAccumValues failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RepopulateAccumValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteValues
    '
    ' Description: Function which deletes all or selected records from the
    '              accumulation_values table
    '
    ' ***************************************************************** '
    Public Function DeleteValues() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Delete selected or all records from accumulation_values table
            sSQL = "DELETE FROM accumulation_values" & Strings.ChrW(13) & Strings.ChrW(10)

            If InsuranceFileCnt <> 0 Then
                sSQL = sSQL & "WHERE risk_cnt IN " & Strings.ChrW(13) & Strings.ChrW(10) &
                       "(SELECT risk_cnt FROM insurance_file_risk_link" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "WHERE insurance_file_cnt = " & CStr(InsuranceFileCnt) & ")"
            End If

            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DeleteAccumulationValues", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteValues failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddValues
    '
    ' Description: Function which adds all or selected records from the
    '              accumulation_values table
    '
    ' ***************************************************************** '
    Public Function AddValues(ByRef sInsuranceFileCnt As String, ByRef sInsuranceRef As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim vAccumDetails(,) As Object = Nothing
        Dim vAccumKeys() As Integer
        Dim bBuiltOK As Boolean

        'TN20010411 Start
        Dim cRetainedSum, cTreatySum, cFacSum As Decimal
        'TN20010411 End

        Dim cSumInsured, cTotalSumInsured As Decimal

        Dim cWorkRetainedSum, cWorkTreatySum, cWorkFacSum As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetTreatyFac(v_lInsuranceFileCnt:=CInt(sInsuranceFileCnt), r_cRetainedSum:=cRetainedSum, r_cTreatySum:=cTreatySum, r_cFacSum:=cFacSum)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="failed to get treaty and fac sum insured", vApp:=ACApp, vClass:=ACClass, vMethod:="AddValues", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            'get details needed to add records to accumulation_values table
            sSQL = New StringBuilder("SELECT p.peril_type_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("SUM(p.sum_insured)," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("r.accumulation_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("r.risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(",SUM(p.coinsured_sum_insured)" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append(",SUM(p.retained_sum_insured)" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("FROM peril p," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("risk r," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("insurance_file_risk_link ifr," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("rating_section rs" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("WHERE ifr.risk_cnt = r.risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("AND rs.risk_cnt = p.risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("AND rs.rating_section_id = p.rating_section_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("AND rs.original_flag = 0" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("AND ifr.insurance_file_cnt = " & sInsuranceFileCnt & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("AND ifr.status_flag <> 'D'" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("AND p.risk_cnt = r.risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("AND r.is_accumulated = 1" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("AND r.accumulation_id IS NOT NULL" & Strings.ChrW(13) & Strings.ChrW(10))

            sSQL.Append("GROUP BY p.peril_type_id, r.accumulation_id, r.risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10))

            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GetAccumulationDetails", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vAccumDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vAccumDetails) Then
                ' Log Error Message
                AnyFailed = True
                'still return true as we want to continue after logging message
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            'We'll need this later...
            cTotalSumInsured = 0

            For i As Integer = vAccumDetails.GetLowerBound(1) To vAccumDetails.GetUpperBound(1)

                cTotalSumInsured += CDbl(vAccumDetails(1, i))
            Next i


            For i As Integer = vAccumDetails.GetLowerBound(1) To vAccumDetails.GetUpperBound(1)
                'build accumulation keys

                m_lReturn = BuildAccumulationKeys(vAccumKeys:=vAccumKeys, vAccumulationID:=CInt(vAccumDetails(2, i)), sInsuranceRef:=sInsuranceRef, bBuiltOK:=bBuiltOK)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'if a non fatal error occured in BuildAccumulationKeys we would still return true but the
                'bBuilt parameter would be false, so we must test it before we try to add
                If bBuiltOK Then

                    'Recalculate these - coinsurance figure should be correct...

                    cSumInsured = CDec(vAccumDetails(1, i))
                    If cTotalSumInsured <> 0 Then 'PN 16750
                        cWorkRetainedSum = (cRetainedSum / cTotalSumInsured) * cSumInsured
                        cWorkTreatySum = (cTreatySum / cTotalSumInsured) * cSumInsured
                        cWorkFacSum = (cFacSum / cTotalSumInsured) * cSumInsured
                    End If

                    'we managed to build the keys
                    'Add selected or all records to accumulation_values table
                    sSQL = New StringBuilder("")
                    sSQL.Append("INSERT INTO accumulation_values ")
                    sSQL.Append("(accumulation_code_1, accumulation_code_2, ")
                    sSQL.Append("accumulation_code_3, accumulation_code_4, ")
                    sSQL.Append("accumulation_code_5, accumulation_code_6, ")
                    sSQL.Append("accumulation_code_7, accumulation_code_8, ")
                    sSQL.Append("accumulation_code_9, ")
                    sSQL.Append("risk_cnt, peril_type_id, sum_insured ")

                    sSQL.Append(",coinsured_sum_insured")
                    sSQL.Append(",retained_sum_insured")
                    sSQL.Append(",treaty_sum_insured")
                    sSQL.Append(",fac_sum_insured) ")

                    sSQL.Append("VALUES (" & vAccumKeys(0) & ", " & CStr(vAccumKeys(1)) & ", ")
                    sSQL.Append(CStr(vAccumKeys(2)) & ", " & CStr(vAccumKeys(3)) & ", ")
                    sSQL.Append(CStr(vAccumKeys(4)) & ", " & CStr(vAccumKeys(5)) & ", ")
                    sSQL.Append(CStr(vAccumKeys(6)) & ", " & CStr(vAccumKeys(7)) & ", ")
                    sSQL.Append(CStr(vAccumKeys(8)) & ", ")


                    sSQL.Append(CStr(vAccumDetails(3, i)) & ", " & CStr(vAccumDetails(0, i)) & " , ")

                    sSQL.Append(CStr(vAccumDetails(1, i))) '& ")"





                    sSQL.Append("," & (If(CStr(vAccumDetails(4, i)) = "" Or vAccumDetails(4, i) Is DBNull.Value, CStr(0), CStr(vAccumDetails(4, i)))))
                    sSQL.Append("," & cWorkRetainedSum)
                    sSQL.Append("," & cWorkTreatySum)
                    sSQL.Append("," & cWorkFacSum & ")")

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="AddAccumulationValues", bStoredProcedure:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Next i

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddValues failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BuildAccumulationKeys
    '
    ' Description: Function which builds the accumulation keys for an
    '              accumulation_value record
    '
    ' ***************************************************************** '
    Public Function BuildAccumulationKeys(ByRef vAccumKeys() As Integer, ByRef vAccumulationID As Integer, ByRef sInsuranceRef As String, ByRef bBuiltOK As Boolean) As Integer

        Dim result As Integer = 0
        Dim vAccumKey As Integer
        Dim sSQL As New StringBuilder
        Dim vAccumParent(,) As Object = Nothing
        Dim bAtTop As Boolean


        Try

            bAtTop = False
            bBuiltOK = False

            result = gPMConstants.PMEReturnCode.PMTrue
            vAccumKey = vAccumulationID

            ReDim vAccumKeys(8)
            For iCode As Integer = 0 To 8
                vAccumKeys(iCode) = 0
            Next iCode

            Do While Not bAtTop
                For iCode As Integer = 7 To 0 Step -1
                    vAccumKeys(iCode + 1) = vAccumKeys(iCode)
                Next iCode
                vAccumKeys(0) = vAccumKey

                'get  parent code
                sSQL = New StringBuilder("")
                sSQL = New StringBuilder("SELECT  a.parent_id ")
                sSQL.Append("FROM accumulation a ")
                sSQL.Append("WHERE a.accumulation_id = " & vAccumKey)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="BuildAccumulationKeys", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vAccumParent)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(vAccumParent) Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find necessary details to add accumulation values for insurance file " & sInsuranceRef, vApp:=ACApp, vClass:=ACClass, vMethod:="AddValues", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    bBuiltOK = False
                    AnyFailed = True
                    'still return true as we want to continue after logging message
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If


                Dim auxVar As Object = vAccumParent(0, 0)


                If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Or CStr(vAccumParent(0, 0)) = "" Then
                    bAtTop = True
                Else

                    vAccumKey = CInt(vAccumParent(0, 0))
                End If
            Loop

            bBuiltOK = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddValues failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

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

    '*******************************************************************************
    ' Name : GetTreatyFac
    '
    ' Desc : sum up treaty and fac for this policy
    '
    ' Hist : 11 April 2001 Created - Tinny
    '*******************************************************************************
    Private Function GetTreatyFac(ByVal v_lInsuranceFileCnt As Integer, ByRef r_cRetainedSum As Decimal, ByRef r_cTreatySum As Decimal, ByRef r_cFacSum As Decimal) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim cRiskRetainedSum As Decimal 'sum of all retained for this risk
        Dim cRiskTreatySum As Decimal 'sum of all treaty for this risk
        Dim cRiskFacSum As Decimal 'sum of all fac for this risk



        result = gPMConstants.PMEReturnCode.PMTrue

        'get all risks attached to this policy
        m_lReturn = GetRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return result
        End If

        'loop thro and sum up treaty and fac for all risks

        For lCount As Integer = 0 To vResultArray.GetUpperBound(1)
            cRiskRetainedSum = 0
            cRiskTreatySum = 0
            cRiskFacSum = 0


            m_lReturn = GetRiskTreatyFac(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=CInt(vResultArray(0, lCount)), r_cRiskRetainedSum:=cRiskRetainedSum, r_cRiskTreatySum:=cRiskTreatySum, r_cRiskFacSum:=cRiskFacSum)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'sum up all treaty and fac for policy
            r_cRetainedSum += cRiskRetainedSum
            r_cTreatySum += cRiskTreatySum
            r_cFacSum += cRiskFacSum
        Next

        Return result

    End Function

    '*******************************************************************************
    ' Name : GetRiskTreatyFac
    '
    ' Desc : get and sum up treaty and fac details for this risk
    '
    ' Hist : 11 April 2001 Created - Tinny
    '*******************************************************************************
    Private Function GetRiskTreatyFac(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_cRiskRetainedSum As Decimal, ByRef r_cRiskTreatySum As Decimal, ByRef r_cRiskFacSum As Decimal) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        r_cRiskRetainedSum = 0
        r_cRiskTreatySum = 0
        r_cRiskFacSum = 0


        m_oRIBusiness.InsuranceFileCnt = v_lInsuranceFileCnt

        m_oRIBusiness.RiskID = v_lRiskCnt

        'get details and put in collection

        m_lReturn = m_oRIBusiness.GetDetails()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Get totals from RI Business object

        m_lReturn = m_oRIBusiness.GetRiskTotals(r_cRiskRetainedSI:=r_cRiskRetainedSum, r_cRiskTreatySI:=r_cRiskTreatySum, r_cRiskFacSI:=r_cRiskFacSum)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        Return result

    End Function

    '*******************************************************************************
    ' Name : GetRisk
    '
    ' Desc : get all risks attached to policy
    '
    ' Hist : 11 April 2001 Created - Tinny
    '*******************************************************************************
    Private Function GetRisk(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelRiskSQL, sSQLName:=ACSelRiskName, bStoredProcedure:=ACSelRiskStored, vResultArray:=r_vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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

End Class
