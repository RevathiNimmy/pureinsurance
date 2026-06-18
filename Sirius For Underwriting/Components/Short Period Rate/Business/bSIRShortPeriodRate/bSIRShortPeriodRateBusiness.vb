Option Strict Off
Option Explicit On
'developer guide no 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 11/07/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRShortPeriodRate.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 22/12/2003
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

    Private m_lProductId As Integer
    Private m_dRefund As Double 'refund value


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


    Public WriteOnly Property ProductId() As Integer
        Set(ByVal Value As Integer)

            m_lProductId = Value

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

                m_iTask = ToSafeInteger(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = ToSafeInteger(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = ToSafeInteger(vProcessMode)
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
    ' Name: Refund (Public)
    '
    ' Description: Calculate the value to be refunded
    '
    ' ***************************************************************** '
    Public Function GetRefund(ByVal v_vProductID As Integer, ByVal v_vType As String, ByVal v_vStartDate As Object, ByVal v_vEndDate As Object, ByVal v_vTransactDate As Date, ByVal v_vPremium As Object, ByRef r_vRefundValue As Double) As Integer


        Dim result As Integer = 0
        Dim lValueDefault As Integer 'EndDate - StartDate in the interval of either month/week/day
        Dim vResultArray(,) As Object = Nothing 'array to store results

        Try

            'default value to return
            result = gPMConstants.PMEReturnCode.PMFalse

            'only do calculation when all parameters are passed
            If False Or False Or False Or False Or False Or False Or False Then

                Return result
            End If

            'default refund value to 0
            r_vRefundValue = 0.0#

            'calculate the number of months between StartDate and EndDate


            'lValueDefault = DateAndTime.DateDiff("m", CDate(v_vStartDate), CDate(v_vEndDate), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)
            lValueDefault = Informations.DateDiff("m", CDate(v_vStartDate), CDate(v_vEndDate), DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)

            'search for Monthly Short Period Rate

            If (FindSPR(v_vProductID, v_vType, "M", lValueDefault, v_vTransactDate, vResultArray) = gPMConstants.PMEReturnCode.PMTrue) And (Not Informations.IsArray(vResultArray)) Then

                'calculate the number of weeks between StartDate and EndDate


                ' lValueDefault = DateAndTime.DateDiff("ww", CDate(v_vStartDate), CDate(v_vEndDate), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)
                lValueDefault = Informations.DateDiff("ww", CDate(v_vStartDate), CDate(v_vEndDate), DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)

                'search for Weekly Short Period Rate

                If (FindSPR(v_vProductID, v_vType, "W", lValueDefault, v_vTransactDate, vResultArray) = gPMConstants.PMEReturnCode.PMTrue) And (Not Informations.IsArray(vResultArray)) Then

                    'calculate the number of days between StartDate and EndDate


                    'lValueDefault = DateAndTime.DateDiff("d", CDate(v_vStartDate), CDate(v_vEndDate), FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)
                    lValueDefault = Informations.DateDiff("d", CDate(v_vStartDate), CDate(v_vEndDate), DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)

                    'search for Daily Short Period Rate

                    If (FindSPR(v_vProductID, v_vType, "D", lValueDefault, v_vTransactDate, vResultArray) = gPMConstants.PMEReturnCode.PMTrue) And (Not Informations.IsArray(vResultArray)) Then

                        'ooops no short period rate found....return default value

                        Return result
                    End If
                End If
            End If

            'yeap we have data and didn't failed in FindSPR
            If Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMTrue


                r_vRefundValue = (CDbl(v_vPremium) * CDbl(vResultArray(0, 0))) / 100
            End If


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Refund Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRefund ", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Refund (Public)
    '
    ' Description: Calculate the value to be refunded
    '
    ' ***************************************************************** '
    Public Function GetShortPeriodRate(ByVal v_lProductID As Integer, ByVal v_sType As String, ByVal v_dtStartDate As Date, ByVal v_dtEndDate As Date, ByVal v_dtTransactDate As Date, ByRef r_dRefundRate As Double) As Integer
        Dim Catch_Renamed As Boolean = False


        Dim result As Integer = 0
        Dim lPeriod As Integer 'EndDate - StartDate in the interval of either month/week/day
        Dim vResultArray(,) As Object = Nothing 'array to store results


        Try
            Catch_Renamed = True

            'default value to return
            result = gPMConstants.PMEReturnCode.PMFalse

            'default refund value to 0
            r_dRefundRate = 0

            'calculate the number of months between StartDate and EndDate
            'lPeriod = DateAndTime.DateDiff("m", v_dtStartDate, v_dtEndDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)
            lPeriod = Informations.DateDiffMonths(CDate(v_dtStartDate), CDate(v_dtEndDate), DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)

            'search for Monthly Short Period Rate

            m_lReturn = CType(FindSPR(v_lProductID, v_sType, "M", lPeriod, v_dtTransactDate, vResultArray), gPMConstants.PMEReturnCode)
            If Not ((m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And Informations.IsArray(vResultArray)) Then

                'calculate the number of weeks between StartDate and EndDate
                ' lPeriod = DateAndTime.DateDiff("ww", v_dtStartDate, v_dtEndDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)
                lPeriod = Informations.DateDiffWeeks(CDate(v_dtStartDate), CDate(v_dtEndDate), DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)

                'search for Weekly Short Period Rate

                m_lReturn = CType(FindSPR(v_lProductID, v_sType, "W", lPeriod, v_dtTransactDate, vResultArray), gPMConstants.PMEReturnCode)
                If Not ((m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And Informations.IsArray(vResultArray)) Then

                    'calculate the number of days between StartDate and EndDate
                    'lPeriod = DateAndTime.DateDiff("d", v_dtStartDate, v_dtEndDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)
                    lPeriod = Informations.DateDiff("d", CDate(v_dtStartDate), CDate(v_dtEndDate), DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)

                    'search for Daily Short Period Rate

                    m_lReturn = CType(FindSPR(v_lProductID, v_sType, "D", lPeriod, v_dtTransactDate, vResultArray), gPMConstants.PMEReturnCode)
                    If Not ((m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And Informations.IsArray(vResultArray)) Then
                        'ooops no short period rate found....return not found
                        Return gPMConstants.PMEReturnCode.PMNotFound
                    End If
                End If
            End If

            'yeap we have data and didn't failed in FindSPR
            result = gPMConstants.PMEReturnCode.PMTrue

            r_dRefundRate = CDbl(vResultArray(0, 0)) / 100

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

            'Developer Guide No 32


            If Catch_Renamed Then

                Select Case Informations.Err().Number
                    Case gPMConstants.Constants.vbObjectError
                        ' Log internal failure.
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:=excep.Source, excep:=excep)

                        Return gPMConstants.PMEReturnCode.PMFalse

                    Case Else
                        ' Log internal error.
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to calculate short period rate", vApp:=ACApp, vClass:=ACClass, vMethod:="GetShortPeriodRate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


                        Return gPMConstants.PMEReturnCode.PMError
                End Select



                Return result
            End If
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SearchAll
    '
    ' Description: SQL Query to Select ShortPeriodRate File details
    '
    ' ***************************************************************** '
    Public Function SearchAll(ByRef r_vResultArray(,) As Object, ByVal v_vProductID As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered

            sSQL = ""
            sSQL = sSQL & "SELECT SPR.* FROM Short_Period_Rates SPR" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " WHERE SPR.Product_ID = " & CStr(ToSafeInteger(v_vProductID)) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " ORDER BY SPR.Product_ID"

            ' Execute SQL Statement - use array for speed

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAllDetails", bStoredProcedure:=False, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO records were found return PMFalse
            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetProductCode
    '
    ' Description: SQL Query to Select Product Code for display on form
    ' JMK 27/03/2001 Display the ProductCode
    ' ***************************************************************** '
    Public Function GetProductCode(ByRef r_vResultArray(,) As Object, ByVal v_vProductID As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered

            sSQL = ""
            sSQL = sSQL & "SELECT code FROM Product" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & " WHERE product_id = " & CStr(ToSafeInteger(v_vProductID)) & Strings.ChrW(13) & Strings.ChrW(10)

            ' Execute SQL Statement - use array for speed

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetProductCode", bStoredProcedure:=False, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductCode")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO records were found return PMFalse
            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateShortPeriodRate (Public)
    '
    ' Description: Update Short Period Rates
    '
    ' ***************************************************************** '

    Public Function UpdateShortPeriodRate(ByVal v_vProductID As Object, ByRef v_vShortPeriodRates(,) As Object) As Integer


        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Add new Short Period Rates for the Product Code passed

            If (ToSafeInteger(v_vProductID) <> 0) And (Not False) And (Informations.IsArray(v_vShortPeriodRates)) Then

                m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'delete old data
                m_oDatabase.Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="Product_ID", vValue:=CStr(ToSafeInteger(v_vProductID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelDetailsSQL, sSQLName:=ACDelDetailsName, bStoredProcedure:=ACDelDetailsStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                For i As Integer = v_vShortPeriodRates.GetLowerBound(1) To v_vShortPeriodRates.GetUpperBound(1)

                    With m_oDatabase
                        .Parameters.Clear()

                        'Add Product Code

                        m_lReturn = .Parameters.Add(sName:="Product_ID", vValue:=CStr(ToSafeInteger(v_vProductID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        'Add Type

                        m_lReturn = .Parameters.Add(sName:="Type", vValue:=CStr(v_vShortPeriodRates(1, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                        'Add Period

                        m_lReturn = .Parameters.Add(sName:="Period", vValue:=CStr(v_vShortPeriodRates(2, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                        'Add Value

                        m_lReturn = .Parameters.Add(sName:="Value", vValue:=CStr(ToSafeInteger(v_vShortPeriodRates(3, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        'Add Effective Date

                        m_lReturn = .Parameters.Add(sName:="Effective_Date", vValue:=CStr(v_vShortPeriodRates(4, i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                        'Add Percentage

                        m_lReturn = .Parameters.Add(sName:="Percentage", vValue:=CStr(CDbl(v_vShortPeriodRates(5, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                        'Add Is Deleted

                        m_lReturn = .Parameters.Add(sName:="Is_Deleted", vValue:=CStr(ToSafeInteger(v_vShortPeriodRates(6, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    End With

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddDetailsSQL, sSQLName:=ACAddDetailsName, bStoredProcedure:=ACAddDetailsStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next i

                m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateShortPeriodRate  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateShortPeriodRate ", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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




    ' ***************************************************************** '
    ' Name: FindSPR (Private)
    '
    ' Description: Find Short Period Rate
    '
    ' ***************************************************************** '
    Private Function FindSPR(ByVal v_vProductID As Integer, ByVal v_vType As String, ByVal v_vPeriod As String, ByVal v_vValue As Integer, ByVal v_vTransactDate As Date, ByRef r_vResultArray(,) As Object) As Integer


        Dim result As Integer = 0


        'default to successful
        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="Product_ID", vValue:=CStr(v_vProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="Type", vValue:=v_vType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="Period", vValue:=v_vPeriod, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="Value", vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'developer guide no. 40
            m_lReturn = .Parameters.Add(sName:="Effective_Date", vValue:=v_vTransactDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)


        End With

        'check to see if an error has occurred while adding in parameters
        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACFindDetailsSQL, sSQLName:=ACAddDetailsName, bStoredProcedure:=ACFindDetailsStored, vResultArray:=r_vResultArray)

        'check for error on calling stored procedure
        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'everything is cool
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

End Class