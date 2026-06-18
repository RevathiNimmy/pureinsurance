Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
'developer guide no. 129 (guide)
Imports System.Text
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: FieldManager Business
    '
    ' Date: 30/07/1997
    '
    ' Description: Creatable FieldManager business class used by the Field Manager
    '
    ' Edit History:
    ' SJP14062002 moved to uniform Product Options scheme and gSIRLibrary.bas
    ' RKS 04/05/2005 Added optional parameter vStandardWordingProperty to GetStandardWording
    '                354-Standard Wording Control Enhancements
    ' CJB23112005 PN25916 Changed GetStoredProcName to order by each loop which corresponds to each group level.
    '             Rqd to enable us to get correct sql back (worked up to 20/1/05 change in processing).
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' ************************************************
    ' Added to replace global variables 12/01/2004
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    ' Parameter Collection (Private)
    Private m_oParameters As dPMDAO.Parameters
    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lError As Integer
    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' Collection to hold selected records
    Private m_oFields As bSIRFieldManager.Fields
    ' Collection of records returned from the database
    Private m_oResults As bSIRFieldManager.Results
    ' Collection of databases
    Private m_oDbases As bSIRFieldManager.PMDAOInstances
    'List Manager...
    'Private m_oListManager As iGISListManager.InterfaceNoLogin
    Private m_oListManager As Object
    'DN 07/01/02
    Private m_sUnderwritingOrAgency As String = ""
    Private m_bCalledFromSwift As Boolean ' RAM20050201 - Added for Swift Support
    Private m_oStoredProcNames As Dictionary(Of String, String)
    Private m_oKeysCollection As New Dictionary(Of String, Object(,))
    '*************************************
    ' ME : 29-11-2002 : 202
    ' Holds an array: stored procs name, value, datatype
    ' This array passes additional parameters to
    ' the stored procedures referenced by fields
    ' in wp_fields
    Private m_vFieldParams(,) As Object
    '*************************************
    '*************************************
    ' ME : 29-11-2002 : 202
    Public WriteOnly Property FieldParameters() As Object(,)
        Set(ByVal Value As Object(,))
            m_vFieldParams = Value
        End Set
    End Property
    '*************************************
    ' ***************************************************************** '
    ' Standard Product Family Constant (Read Only)
    ' ***************************************************************** '
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property
    'DN 07/01/02
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get
            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If
            Return m_sUnderwritingOrAgency
        End Get
    End Property

    Public WriteOnly Property CalledFromSwift() As Boolean
        Set(ByVal Value As Boolean)
            m_bCalledFromSwift = Value
        End Set
    End Property


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
            m_oStoredProcNames = New Dictionary(Of String, String)
            'DN 07/01/02
            m_sUnderwritingOrAgency = UnderwritingOrAgency

            Return result

        Catch excep As System.Exception



            ' Error Section.
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
            Me.disposedValue = True
            If disposing Then

                m_oFields = Nothing
                m_oResults = Nothing
                m_oDbases = Nothing

                If m_oListManager IsNot Nothing Then
                    m_oListManager.Dispose()
                    m_oListManager = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub
    ''' <summary>
    ''' Get the Insurance File Count Based on Claim Cnt
    ''' </summary>
    ''' <param name="v_iClaimCnt">Claim Cnt</param>
    ''' <returns></returns>
    Public Function GetInsuranceFileCntFromClaim(ByVal v_iClaimCnt As Integer) As Integer

        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""
        Dim iInsuranceFileCnt As Integer = 0
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_iClaimCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        sSQL = "select policy_id from claim where claim_id = {claim_id}"

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Claim ID", bStoredProcedure:=False, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return 0
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return 0
        End If

        iInsuranceFileCnt = CInt(vResultArray(0, 0))
        Return iInsuranceFileCnt

    End Function


    ' ***************************************************************** '
    ' Name: GetFieldList
    '
    ' Description: Retrieves a list of tables and fields in the database
    '               which contain policy data
    ' Edit History  :
    ' RAM20050104   : Added the v_iProductFamily optional Parameter to support SWIFT
    ' ***************************************************************** '
    Public Function GetFieldList(ByRef vFieldArray As Object, Optional ByVal v_iProductFamily As Integer = 0) As Integer

        Dim result As Integer = 0
        Const ACGetFieldListStored As Boolean = True
        Const ACGetFieldListName As String = "GetWpFields"
        'developer guide no.39
        Const ACGetFieldListSQL As String = "spu_wp_get_fields"

        Dim vResultArray(,) As Object = Nothing
        Dim lRecords As Integer

        Try

            vFieldArray = Nothing

            lRecords = gPMConstants.PMAllRecords

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If v_iProductFamily > 0 Then
                    m_lReturn = .Parameters.Add(sName:="product_family", vValue:=CStr(v_iProductFamily), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                Else

                    'Modified by Sudhanshu Behera on 4/26/2010 4:56:29 PM refer developer guide no. 85(guide)
                    m_lReturn = .Parameters.Add("product_family", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                End If

                ' Execute SQL Statement
                m_lError = .SQLSelect(sSQL:=ACGetFieldListSQL, sSQLName:=ACGetFieldListName, bStoredProcedure:=ACGetFieldListStored, lNumberRecords:=lRecords, vResultArray:=vResultArray)

            End With

            ' Return the data


            vFieldArray = vResultArray
            vResultArray = Nothing

            ' Set the return value

            Return m_lError

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Field List Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFieldList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetClauseList
    '
    ' Description: Retrieves all clauses that can validly be included
    '               in the parent document. The validation of the
    '               appropriate clauses will be carried out in the
    '               stored procedure itself.
    '
    ' History: 08/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetClauseList(ByRef lDocId As Integer, ByRef vClauseArray As Object) As Integer

        Dim result As Integer = 0
        Dim sSQLString As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lRecords As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vClauseArray = Nothing

            lRecords = gPMConstants.PMAllRecords
            'developer guide no.39
            sSQLString = "spu_valid_clauses_sel"

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="document_template_id", vValue:=CStr(lDocId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=sSQLString, sSQLName:="", bStoredProcedure:=True, lNumberRecords:=0, vResultArray:=vResultArray)

            End With



            vClauseArray = vResultArray

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClauseList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClauseList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadFields
    '
    ' Description: Loads a list of tables and fields in the database
    '               which contain policy data
    '
    ' ***************************************************************** '
    Public Function LoadFields() As Integer

        Dim result As Integer = 0
        Dim sSQLString As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lRecords As Integer
        Dim oField As Field
        Dim sFieldName, sFieldSQL, sColumnName As String
        Dim iColumnType, iProductFamily, iSpecialType As Integer
        Dim vDataModel As String = ""
        Dim vPropertyId, sMainGroup As String 'RWH(03/10/2000) RSAIB Process 28. Risk Loop.
        Dim sSubGroup As String
        'RWH(16/01/2001) RSAIB Process 28. Extra levels of Risk Looping.
        Dim sLoop1, sLoop2, sLoop3, sLoop4 As String
        Dim sTableName As String
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oFields.Count > 0 Then
                'We already have the data no need to reload
                Return result
            End If
            vResultArray = Nothing

            m_oFields.Clear()

            lRecords = 2000

            'sSQLString$ = "exec spu_get_data_fields "
            sSQLString = "exec spu_wp_get_fields " & m_iSourceID

            ' RAM20050201 - Added code for support Swift
            If m_bCalledFromSwift Then
                sSQLString = sSQLString & "," & CStr(gPMConstants.PMEProductFamily.pmePFSwift)
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQLString, sSQLName:="", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If Informations.IsArray(vResultArray) Then


                For lRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)


                    sFieldName = vResultArray(MainModule.ACFields_FieldName, lRow)
                    sFieldSQL = vResultArray(MainModule.ACFields_SQL, lRow)
                    sColumnName = vResultArray(MainModule.ACFields_ColumnName, lRow)
                    sMainGroup = vResultArray(MainModule.ACFields_MainGroup, lRow) 'RWH(03/10/2000) RSAIB Process 28. Risk Loop.
                    sSubGroup = vResultArray(MainModule.ACFields_SubGroup, lRow)


                    iColumnType = CInt(vResultArray(MainModule.ACFields_ColumnType, lRow))

                    ' CTAF 130700
                    If vResultArray(MainModule.ACFields_ProductFamily, lRow) <> "" Then
                        iProductFamily = CInt(vResultArray(MainModule.ACFields_ProductFamily, lRow))
                    End If

                    vDataModel = vResultArray(MainModule.ACFields_DataModel, lRow)
                    vPropertyId = vResultArray(MainModule.ACFields_PropertyID, lRow)

                    'RWH(16/01/2001) RSAIB Process 28. Extra levels of Risk Looping.
                    sLoop1 = vResultArray(MainModule.ACFields_Loop1, lRow)
                    sLoop2 = vResultArray(MainModule.ACFields_Loop2, lRow)
                    sLoop3 = vResultArray(MainModule.ACFields_Loop3, lRow)
                    sLoop4 = vResultArray(MainModule.ACFields_Loop4, lRow)
                    sTableName = vResultArray(MainModule.ACFields_TableName, lRow)
                    'RWH(03/10/2000) RSAIB Process 28. Risk Loop.
                    oField = m_oFields.Add(sKey:=sFieldName, sSQLString:=sFieldSQL, sColName:=sColumnName, sColType:=iColumnType, sMainGroup:=sMainGroup, sSubGroup:=sSubGroup, iProductFamily:=iProductFamily, vDataModel:=vDataModel, vPropertyId:=vPropertyId, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4, iSpecialType:=iSpecialType, r_sTableName:=sTableName)


                Next lRow

            End If

            vResultArray = Nothing
            oField = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Load Field Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFields", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Gets the value of a field from the database
    ''' </summary>
    ''' <param name="lPartyCnt"></param>
    ''' <param name="lInsuranceFileCnt"></param>
    ''' <param name="lClaimCnt"></param>
    ''' <param name="sDocumentRef"></param>
    ''' <param name="vArray"></param>
    ''' <param name="vRiskId"></param>
    ''' <param name="sGroup"></param>
    ''' <param name="sSubGroup"></param>
    ''' <param name="sLoop1"></param>
    ''' <param name="sLoop2"></param>
    ''' <param name="sLoop3"></param>
    ''' <param name="r_sTableName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFieldValues(ByRef lPartyCnt As Integer, ByRef lInsuranceFileCnt As Integer,
                                   ByRef lClaimCnt As Integer,
                                   ByRef sDocumentRef As String,
                                   ByRef vArray(,) As Object,
                                   Optional ByRef vRiskId As Object = Nothing,
                                   Optional ByRef sGroup As String = "",
                                   Optional ByRef sSubGroup As String = "",
                                   Optional ByRef sLoop1 As String = "",
                                    Optional ByRef sLoop2 As String = "",
                                    Optional ByRef sLoop3 As String = "",
                                    Optional ByRef r_sTableName As String = "") As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue

        Dim oField As New bSIRFieldManager.Field
        Dim sFieldName As String = ""
        Dim sValue As String = " "
        Dim sSQL As String = ""
        Dim oResult As New bSIRFieldManager.Result
        Dim sColumnName As String = ""
        Dim oRecord As dPMDAO.Records
        Dim sTemp As StringBuilder
        Dim sKey As StringBuilder
        Dim iType As Integer
        Dim iInstance1, iInstance2, iInstance3 As Integer
        Dim iProductFamily As Integer
        Dim sProductFamily As String = ""
        Dim vDataModel As Object = Nothing
        Dim vPropertyId As Object = Nothing
        Dim oDatabase As New dPMDAO.Database
        Dim oDbase As bSIRFieldManager.PMDAOInstance
        Dim sDescription As Object = ""

        Dim lRiskCnt As Integer

        Try
            sTemp = New StringBuilder("")
            sKey = New StringBuilder("")
            If (Not Object.Equals(vRiskId, Nothing)) And (Not Informations.IsNothing(vRiskId)) Then

                lRiskCnt = CInt(vRiskId)
            Else
                If lClaimCnt > 0 Then
                    m_lReturn = CType(GetRiskID(v_lClaimCnt:=lClaimCnt, r_lRiskCnt:=lRiskCnt), gPMConstants.PMEReturnCode)
                End If
            End If

            ' Loop through the Array
            For lRow As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                ' Get the field name

                sFieldName = vArray(MainModule.BookmarkName, lRow)

                If ((Right(sFieldName, 3) = "[1]") Or (Right(sFieldName, 3) = "[0]")) Then
                    sFieldName = sFieldName.Substring(0, sFieldName.Length - 3)
                End If

                'ED 26092002 - First 3 intances are always present
                iInstance1 = CInt(vArray(MainModule.BookmarkInstance1, lRow))
                iInstance2 = CInt(vArray(MainModule.BookmarkInstance2, lRow))
                iInstance3 = CInt(vArray(MainModule.BookmarkInstance3, lRow))

                oField = m_oFields(sFieldName.ToUpper())
                'Added the standard wording check as no spg is generated for the same.
                If Not (oField Is Nothing) AndAlso oField.SpecialType <> 5 Then

                    ' Get the SQL query and column name
                    sSQL = oField.SQLString
                    iType = oField.ColumnType
                    sColumnName = oField.ColumnName
                    iProductFamily = oField.ProductFamily
                    sProductFamily = CStr(iProductFamily).Trim()
                    sGroup = oField.MainGroup
                    sSubGroup = oField.SubGroup
                    sLoop1 = oField.Loop1
                    sLoop2 = oField.Loop2
                    sLoop3 = oField.Loop3
                    r_sTableName = oField.TableName
                    vDataModel = oField.DataModel

                    vPropertyId = oField.PropertyId

                    sKey.Append(sSQL)

                    ' AJM 15/03/01 - expand the Informations used to make the key
                    sKey.Append(lPartyCnt.ToString & ACInsuranceFileCnt.ToString)

                    'Thinh Nguyen 25/06/2003 (start) - pass risk_id in all the time
                    sKey.Append(lRiskCnt.ToString)
                    'Thinh Nguyen 25/06/2003 (end) - pass risk_id in all the time

                    sKey.Append(CStr(lClaimCnt) & sDocumentRef & StringsHelper.Format(iInstance1, "000") & StringsHelper.Format(iInstance2, "000") & StringsHelper.Format(iInstance3, "000"))
                    'ED 26092002 - Dynamically add any further instances -
                    '              BookmarkInstance3 represents the default
                    '              uppor bound of the array.
                    If vArray.GetUpperBound(0) > MainModule.BookmarkInstance3 Then 'PN46453-Reomoved the code bbecause it was blocking to display fourth level field value in the document production.
                        For iIndex As Integer = (MainModule.BookmarkInstance3 + 1) To vArray.GetUpperBound(0)


                            sKey.Append(StringsHelper.Format(vArray(iIndex, lRow), "000"))

                        Next iIndex
                    End If
                    'ED 26092002 - END

                    ' Check if there is already a result object for this query
                    oResult = m_oResults.Item(sKey.ToString)

                    If oResult Is Nothing Then
                        sTemp.Length = 0
                        sTemp.Append("exec " & sSQL & " " & CStr(lPartyCnt) & ", " & CStr(lInsuranceFileCnt) & ", ")

                        'Thinh Nguyen 25/06/2003 (start) - pass risk_id in all the time
                        sTemp.Append(CStr(lRiskCnt) & ",")
                        'Thinh Nguyen 25/06/2003 (end) - pass risk_id in all the time

                        sTemp.Append(CStr(lClaimCnt) & ", " & "'" & sDocumentRef & "', " & CStr(iInstance1) & ", " & CStr(iInstance2) & ", " & CStr(iInstance3))

                        'ED 26092002 - Dynamically add any further instances -
                        '              BookmarkInstance3 represents the default
                        '              uppor bound of the array.
                        If vArray.GetUpperBound(0) > MainModule.BookmarkInstance3 Then 'PN46453-Reomoved the code bbecause it was blocking to display fourth level field value in the document production.
                            For iIndex As Integer = (MainModule.BookmarkInstance3 + 1) To vArray.GetUpperBound(0)


                                sTemp.Append(", " & CStr(vArray(iIndex, lRow)))

                            Next iIndex
                        End If
                        'ED 26092002 - END

                        'Get the right PMDAO...

                        ' Check if there is already a PMDAO for this product family
                        oDbase = m_oDbases.Item(iProductFamily)

                        If oDbase Is Nothing Then
                            If m_oDatabase Is Nothing Then
                                m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=iProductFamily, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)
                                ' Add a database to the collection
                                m_lReturn = CType(m_oDbases.Add(iProductFamily, oDatabase), gPMConstants.PMEReturnCode)
                                oDbase = m_oDbases.Item(iProductFamily)
                            Else
                                oDatabase = m_oDatabase
                            End If
                        End If

                        If oDatabase Is Nothing Then
                            oDatabase = oDbase.Database
                        End If

                        ' Execute SQL Statement
                        m_lReturn = oDatabase.SQLSelect(sSQL:=sTemp.ToString, sSQLName:="", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            sValue = ""
                            Return result
                        End If

                        ' Get the first returned record
                        If oDatabase.Records.Count() > 0 Then
                            oRecord = oDatabase.Records.Item(0)
                            'sTemp.Length = 0
                            '' AJM 15/03/01 - expand the Informations used to make the key
                            'sTemp.Append(sSQL & CStr(lPartyCnt) & CStr(lInsuranceFileCnt))

                            ''Thinh Nguyen 25/06/2003 (start) - pass risk_id in all the time
                            'sTemp.Append(CStr(lRiskCnt))
                            ''Thinh Nguyen 25/06/2003 (end) - pass risk_id in all the time

                            'sTemp.Append(CStr(lClaimCnt) & sDocumentRef & StringsHelper.Format(iInstance1, "000") & StringsHelper.Format(iInstance2, "000") & StringsHelper.Format(iInstance3, "000"))

                            ''ED 26092002 - Dynamically add any further instances -
                            ''              BookmarkInstance3 represents the default
                            ''              uppor bound of the array.
                            'If vArray.GetUpperBound(0) > MainModule.BookmarkInstance3 Then 'PN46453-Reomoved the code bbecause it was blocking to display fourth level field value in the document production.
                            '    For iIndex As Integer = (MainModule.BookmarkInstance3 + 1) To vArray.GetUpperBound(0)


                            '        sTemp.Append(StringsHelper.Format(vArray(iIndex, lRow), "000"))

                            '    Next iIndex
                            'End If
                            'ED 26092002

                            ' Add a result to the collection
                            oResult = m_oResults.Add(sKey.ToString, oRecord)
                        End If

                    End If

                    If Not (oResult Is Nothing) Then

                        ' Get the field from the result object
                        If oResult.Record.Fields.Table.Columns.Contains(sColumnName) Then
                            If Convert.IsDBNull(oResult.Record.Item(0).Fields()(sColumnName)) Or Informations.IsNothing(oResult.Record.Item(0).Fields()(sColumnName)) Then
                                sValue = ""
                            Else
                                'Tomo161002
                                'sValue = oResult.Record.Fields.Item(sColumnName).Value
                                sValue = Convert.ToString(oResult.Record.Item(0).Fields()(sColumnName)).Trim()
                                If Not String.IsNullOrEmpty(sValue) Then
                                    'sValue = sValue.Replace("&", "&amp;")
                                    sValue = sValue.Trim.Replace("&", "&amp;")
                                End If
                            End If
                        End If

                    Else
                        sValue = ""
                    End If
                Else
                    sValue = ""
                End If

                'Remove any NULLs
                sValue = sValue.Replace(Strings.ChrW(0).ToString(), "")

                If Not sValue.Contains("{\rtf1") Then
                    'Replace "<" and ">" with HTML codes
                    If sValue.IndexOf("<"c) >= 0 Then
                        sValue = sValue.Replace("<", "&lt;")
                    End If
                    If sValue.IndexOf(">"c) >= 0 Then
                        sValue = sValue.Replace(">", "&gt;")
                    End If
                    'Now convert cr and lf characters into "<br>" without fear of being turned into the HTML codes
                    'ReplaceCrLfWithBr(sValue)
                End If
                'Overkill - just in case

                If Convert.IsDBNull(vDataModel) Or Informations.IsNothing(vDataModel) Then
                    vDataModel = ""
                End If

                'If we have a datamodel and a value then use it to get a description
                If vDataModel <> "" And sValue <> "" And vPropertyId <> "" Then
                    'It's a list manager thing and we need to call it
                    'to get the description from the value

                    If m_oListManager Is Nothing Then
                        m_oListManager = New bGISListManager.Form()


                        m_lReturn = m_oListManager.Initialise

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise List Manager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFieldValues", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            Return result
                        End If

                    End If

                    m_lReturn = m_oListManager.CheckListVersions(v_sGISDataModelCode:=vDataModel)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Check List Versions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFieldValues", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return result
                    End If

                    m_lReturn = m_oListManager.GetDescriptionFromABICode(v_sPropertyId:=ToSafeInteger(vPropertyId), v_sABICode:=ToSafeString(sValue), r_sDescription:=sDescription)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            sDescription = ""
                        Else
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Description From ABI Code Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFieldValues", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            Return result
                        End If
                    End If

                    sValue = sDescription
                End If

                'ED 26092002 - Use defined constants
                ' Stuff the value into the array

                vArray(MainModule.BookmarkValue, lRow) = sValue
                vArray(MainModule.BookmarkType, lRow) = CStr(iType)

            Next lRow

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Field Values Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFieldValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function ClearResults() As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oResults.Clear()

            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ClearParameters (Private)
    '
    ' Description: Clears the Database Parameters Collection if there
    '              are any.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ClearParameters) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub ClearParameters()
    '
    'Try 
    '
    ' Clear the Databases Parameters Collection
    'If m_oDatabase.Parameters Is Nothing Then
    ' Do Nothing
    'Else
    'm_oDatabase.Parameters.Clear()
    'End If
    '
    ' Create New Parameter Collection
    'm_oParameters = Nothing
    'm_oParameters = New dPMDAO.Parameters()
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Clear Parameters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearParameters", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    ' ***************************************************************** '
    '
    ' Name: ReplaceCrLfWithBr
    '
    ' Description: Replace line breaks with <br>
    '
    ' History: 16/10/02 Tomo (Lifted from replaceVbcrWithVbcrlf in gPMLibrary - by CLG)
    '
    ' ***************************************************************** '
    Private Sub ReplaceCrLfWithBr(ByRef sString As String)



        Dim iLoop As Integer
        Dim sStr() As Object
        Dim sReturnVal As String
        Dim sLineBreak As String

        'If the routine's called ReplacevbCr, why look for vbLf?

        'For safety, let's do the combination, then each separately...
        '    iLoop = InStr(sString, vbCrLf)

        sReturnVal = sString
        If Not sReturnVal.StartsWith("{\rtf1") Then
            sStr = sString.Split({vbLf}, StringSplitOptions.None)

            sLineBreak = "<w:br w:type=""text-wrapping""/>"

            If sStr.GetUpperBound(0) > 0 Then
                sReturnVal = ""

                For iLoop = 0 To sStr.GetUpperBound(0)
                    If iLoop = 0 Then
                        sReturnVal = sStr(iLoop) & "</w:t>"
                    ElseIf iLoop = sStr.GetUpperBound(0) Then
                        sReturnVal = sReturnVal & sLineBreak & "<w:t>" & sStr(iLoop)
                    Else
                        sReturnVal = sReturnVal & sLineBreak & "<w:t>" & sStr(iLoop) & "</w:t>"
                    End If
                Next
            End If

            'sStr = Split(sString, vbCrLf)

            'If UBound(sStr) > 0 Then
            '    sReturnVal = ""

            '    For iLoop = 0 To UBound(sStr)
            '        If iLoop = 0 Then
            '            sReturnVal = sStr(iLoop) & "</w:t></w:r>"
            '        ElseIf iLoop = UBound(sStr) Then
            '            sReturnVal = sReturnVal & sLineBreak & "<w:r><w:t>" & sStr(iLoop)
            '        Else
            '            sReturnVal = sReturnVal & sLineBreak & "<w:r><w:t>" & sStr(iLoop) & "</w:t></w:r>"
            '        End If
            '    Next
            'End If

            'sStr = Split(sString, vbCrLf)

            'If UBound(sStr) > 0 Then
            '    sReturnVal = ""

            '    For iLoop = 0 To UBound(sStr)
            '        If iLoop = 0 Then
            '            sReturnVal = sStr(iLoop) & "</w:t></w:r>"
            '        ElseIf iLoop = UBound(sStr) Then
            '            sReturnVal = sReturnVal & sLineBreak & "<w:r><w:t>" & sStr(iLoop)
            '        Else
            '            sReturnVal = sReturnVal & sLineBreak & "<w:r><w:t>" & sStr(iLoop) & "</w:t></w:r>"
            '        End If
            '    Next
            'End If
        End If

        sString = sReturnVal
        'If the routine's called ReplacevbCr, why look for vbLf?

        'For safety, let's do the combination, then each separately...
        'iLoop = (sString.IndexOf(Strings.ChrW(13) & Strings.ChrW(10)) + 1)

        'Do While iLoop > 0
        '    sString = sString.Substring(0, iLoop - 1) & "<br>" & Mid(sString, iLoop + 2)
        '    iLoop = Strings.InStr(iLoop + 2, sString, Strings.ChrW(13) & Strings.ChrW(10))
        'Loop

        'iLoop = (sString.IndexOf(Constants.vbLf) + 1)

        'Do While iLoop > 0
        '    sString = sString.Substring(0, iLoop - 1) & "<br>" & Mid(sString, iLoop + 1)
        '    iLoop = Strings.InStr(iLoop + 2, sString, Constants.vbLf)
        'Loop

        'iLoop = (sString.IndexOf(Strings.ChrW(13)) + 1)

        'Do While iLoop > 0
        '    sString = sString.Substring(0, iLoop - 1) & "<br>" & Mid(sString, iLoop + 1)
        '    iLoop = Strings.InStr(iLoop + 2, sString, Strings.ChrW(13))
        'Loop



    End Sub

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        Try

            ' Class Initialise
            m_oFields = New Fields()
            m_oResults = New Results()
            m_oDbases = New PMDAOInstances()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetAllTemplates
    '
    ' Description: Retrieves details for all document templates on the
    '               system. This was created for use by the Document
    '               Conversion Utility.
    '
    ' History: 25/08/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetAllTemplates(ByRef r_vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT document_template_id," & Strings.ChrW(13) & Strings.ChrW(10) & "document_type_id," & Strings.ChrW(13) & Strings.ChrW(10) & "code," & Strings.ChrW(13) & Strings.ChrW(10) & "description," & Strings.ChrW(13) & Strings.ChrW(10) & "entity_type_id = NULL," & Strings.ChrW(13) & Strings.ChrW(10) & "slot_number = NULL," & Strings.ChrW(13) & Strings.ChrW(10) & "file_number = NULL," & Strings.ChrW(13) & Strings.ChrW(10) & "pmuser.username" & Strings.ChrW(13) & Strings.ChrW(10) & "FROM document_template " & Strings.ChrW(13) & Strings.ChrW(10) & "LEFT JOIN pmuser on document_template.created_by_id = pmuser.user_id" & Strings.ChrW(13) & Strings.ChrW(10) & "UNION ALL" & Strings.ChrW(13) & Strings.ChrW(10) & "SELECT document_template_id = NULL," & Strings.ChrW(13) & Strings.ChrW(10) & "document_type_id = NULL," & Strings.ChrW(13) & Strings.ChrW(10) & "code = NULL," & Strings.ChrW(13) & Strings.ChrW(10) & "description = NULL," & Strings.ChrW(13) & Strings.ChrW(10) & "entity_type_id," & Strings.ChrW(13) & Strings.ChrW(10) & "slot_number," & Strings.ChrW(13) & Strings.ChrW(10) & "file_number," & Strings.ChrW(13) & Strings.ChrW(10) & "user_name = NULL" & Strings.ChrW(13) & Strings.ChrW(10) & "FROM text_file "


            With m_oDatabase

                .Parameters.Clear()

                m_lError = .SQLSelect(sSQL:=sSQL, sSQLName:="GetAllTemplates", bStoredProcedure:=False, lNumberRecords:=-1, vResultArray:=r_vResultArray)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllTemplates")

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllTemplates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllTemplates", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetTemplateIdFromCode
    '
    ' Description:
    '
    ' History: 13/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetTemplateFromCode(ByVal sCode As String, ByRef lDocId As Integer, ByRef lDocType As Integer, ByVal v_dtEffectiveDate As Date) As Integer

        'AJM 13/03/01 - this field is needed to conform with changes made to the stored
        ' used for getting the document templates
        Dim result As Integer = 0
        Dim sDocDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sCode.Trim() = "" And (lDocType <> 7 Or lDocId = 0) Then
                Return result
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(lDocId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInputOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_type_id", vValue:=CStr(lDocType), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'AJM 8/3/01 - need to add description parameter so that SP works
            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=sDocDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetDocumentTemplateSQL, sSQLName:=ACGetDocumentTemplateName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("document_template_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("document_template_id").Value)) Then
                lDocId = m_oDatabase.Parameters.Item("document_template_id").Value
            End If

            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("document_type_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("document_type_id").Value)) Then
                lDocType = m_oDatabase.Parameters.Item("document_type_id").Value
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTemplateFromCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTemplateFromCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetTemplateCodeAndDescFromID
    '
    ' Description:
    '
    ' History: 13/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetTemplateCodeAndDescFromID(ByRef sCode As String, ByRef sDesc As String, ByVal lDocId As Integer) As Integer

        'AJM 13/03/01 - this field is needed to conform with changes made to the stored
        ' used for getting the document templates
        Dim result As Integer = 0
        Dim sDocDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lDocId = 0 Then
                Return result
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="docid", vValue:=lDocId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_desc", vValue:=sDesc, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetDocumentCodeAndDescTemplateSQL, sSQLName:=ACGetDocumentTemplateName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'If Not ((Convert.IsDBNull(m_oDatabase.Parameters.Item("document_template_code").Value) And Convert.IsDBNull(m_oDatabase.Parameters.Item("document_template_desc").Value)) Or (IsNothing(m_oDatabase.Parameters.Item("document_template_code").Value And IsNothing(m_oDatabase.Parameters.Item("document_template_desc").Value)) Then
            sCode = m_oDatabase.Parameters.Item("document_template_code").Value
            sDesc = m_oDatabase.Parameters.Item("document_template_desc").Value
            'End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTemplateCodeAndDescFromID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTemplateCodeAndDescFromID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRisksForPolicy
    '
    ' Description:
    '
    ' History: 13/09/2000 RWH - Created.
    '          24/01/2001 RWH - Added RiskStatus param.
    '
    ' ***************************************************************** '
    Public Function GetRisksForPolicy(ByVal lFileInsuranceCnt As Integer, ByVal sRiskStatus As String, ByRef vRiskArray As Object) As Integer
        Dim result As Integer = 0
        Dim sSQLString As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lRecords As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lRecords = gPMConstants.PMAllRecords

            sSQLString = "exec spu_risks_for_policy_saa " & lFileInsuranceCnt & ",'" & sRiskStatus & "'"

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQLString, sSQLName:="", bStoredProcedure:=False, lNumberRecords:=lRecords, vResultArray:=vResultArray)

            If Informations.IsArray(vResultArray) Then


                vRiskArray = vResultArray
            End If

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRisksForPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisksForPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRiskForPolicy
    '
    ' Description:
    '
    ' History: 25/06/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetRiskForPolicy(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRiskID As Integer, Optional ByRef r_sDescription As String = "") As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GetRiskForPolicy")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the sql
            ' TOT says this is one-to-one but I don't trust his lying ass
            ' hence the MAX(risk_count)
            sSQL = "SELECT MAX(ifrl.risk_cnt),r.description " &
                   "FROM insurance_file_risk_link ifrl,risk r " &
                   "WHERE insurance_file_cnt = " & CStr(v_lInsuranceFileCnt) & " and r.risk_cnt=ifrl.risk_cnt group by r.description"

            ' Call it
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetRiskForPolicy", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Grab the return value
            If Informations.IsArray(vResultArray) Then
                'Thinh Nguyen 10/07/2003 (start) - it will return a Null if nothing is found

                If CStr(vResultArray(0, 0)) <> "" Then

                    r_lRiskID = CInt(vResultArray(0, 0))
                    r_sDescription = ToSafeString(vResultArray(1, 0))
                End If
                'Thinh Nguyen 10/07/2003 (end) - it will return a Null if nothing is found
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GetRiskForPolicy")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".GetRiskForPolicy")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskForPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskForPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetParentKey
    '
    ' Description: Retrieves the parent key for a given risk table name
    '               and RiskId (if present).
    '
    ' History: 16/01/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetParentKey(ByRef r_lParentKey As Integer, ByVal v_sChildTable As String, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimCnt As Integer, ByVal v_sDocumentRef As String, Optional ByVal v_vRiskId As Object = Nothing) As Integer

        Dim result As Integer = 0
        'Dim sSPname As String
        Dim vResultArray(,) As Object = Nothing
        Dim sStoredProcName As String = ""
        Dim sSQL As New StringBuilder
        Dim sKey As StringBuilder
        Const sGET_KEYS_END As String = "_get_parent_key"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetStoredProcName(v_sLoopName:=v_sChildTable, r_sStoredProcName:=sStoredProcName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'sSPname = sStoredProcName & sGET_KEYS_END

            sSQL = New StringBuilder("exec " & sStoredProcName & sGET_KEYS_END & " " & CStr(v_lPartyCnt) & ", " & CStr(v_lInsuranceFileCnt) & ", ")


            sKey = New StringBuilder(sStoredProcName & sGET_KEYS_END)
            sKey.Append(v_lPartyCnt.ToString & v_lInsuranceFileCnt.ToString & v_vRiskId.ToString)
            sKey.Append(v_lClaimCnt.ToString & v_sDocumentRef)



            If (Not Object.Equals(v_vRiskId, Nothing)) And (Not Informations.IsNothing(v_vRiskId)) Then

                sSQL.Append(CStr(v_vRiskId) & ", ")
            Else
                'AJM 13/03/01 - ensure NULL is added as a string
                sSQL.Append("Null" & ", ")
            End If

            sSQL.Append(CStr(v_lClaimCnt) & ", " & "'" & v_sDocumentRef & "', " & CStr(0) & "," & CStr(0) & "," & CStr(0))


            If m_oKeysCollection.ContainsKey(sKey.ToString) Then
                vResultArray = m_oKeysCollection(sKey.ToString)
            Else


                With m_oDatabase

                    .Parameters.Clear()

                    m_lError = .SQLSelect(sSQL:=sSQL.ToString, sSQLName:="GetParentKey", bStoredProcedure:=False, vResultArray:=vResultArray)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParentKey")

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' If NO records were found return PMFalse
                    If Not Informations.IsArray(vResultArray) Then
                        Return gPMConstants.PMEReturnCode.PMNotFound
                    End If
                    m_oKeysCollection.Add(sKey.ToString, vResultArray)
                End With
            End If

            r_lParentKey = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParentKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParentKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetLoopKeys
    '
    ' Description:
    '
    ' History: 16/01/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetLoopKeys(ByRef r_vKeyArray(,) As Object, ByVal v_sTableName As String, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimCnt As Integer, ByVal v_sDocumentRef As String, ByVal v_vInstanceArray() As Object, Optional ByVal v_vRiskId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSPname As String = ""
        Dim sSQL As New StringBuilder
        Dim iInstanceTotal As Integer
        Dim sStoredProcName As String = ""
        Dim sKey As StringBuilder
        Const sGET_KEYS_END As String = "_get_keys"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetStoredProcName(v_sLoopName:=v_sTableName, r_sStoredProcName:=sStoredProcName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSPname = sStoredProcName & sGET_KEYS_END
            sKey = New StringBuilder(sSPname)
            sKey.Append(v_lPartyCnt.ToString & v_lInsuranceFileCnt.ToString & v_vRiskId.ToString)
            sKey.Append(v_lClaimCnt.ToString & v_sDocumentRef)

            sSQL = New StringBuilder("exec " & sSPname & " " & CStr(v_lPartyCnt) & ", " & CStr(v_lInsuranceFileCnt) & ", ")



            If (Not Object.Equals(v_vRiskId, Nothing)) And (Not Informations.IsNothing(v_vRiskId)) Then

                sSQL.Append(CStr(v_vRiskId) & ", ")
            Else
                'AJM 13/03/01 - ensure NULL is added as a string
                sSQL.Append("Null" & ", ")
            End If

            sSQL.Append(CStr(v_lClaimCnt) & ", " & "'" & v_sDocumentRef & "'")

            'ED 26092002 - Replaced use of constants with dynamic code to cater
            '              for multiple instances.
            '              Subsequent loop uses iInstanceTotal instead
            '              of UBound(v_vInstanceArray).
            If v_vInstanceArray.GetUpperBound(0) > 2 And (sSQL.ToString().IndexOf("spg_wp_") + 1) < 1 Then
                iInstanceTotal = v_vInstanceArray.GetUpperBound(0) + 1
            Else
                iInstanceTotal = BookmarkInstance1 - 1
            End If

            For iInstanceCount As Integer = 0 To (iInstanceTotal - 1)
                If iInstanceCount > v_vInstanceArray.GetUpperBound(0) Then
                    sSQL.Append("," & 0)
                    sKey.Append(StringsHelper.Format("0", "000"))
                Else

                    sSQL.Append("," & CStr(v_vInstanceArray(iInstanceCount)))
                    sKey.Append(StringsHelper.Format(v_vInstanceArray(iInstanceCount), "000"))
                End If
            Next iInstanceCount

            With m_oDatabase


                If m_oKeysCollection.ContainsKey(sKey.ToString) Then
                    r_vKeyArray = m_oKeysCollection(sKey.ToString)
                Else
                    .Parameters.Clear()

                    m_lError = .SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GetLoopKeys", bStoredProcedure:=False, vResultArray:=r_vKeyArray)

                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLoopKeys")

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' If NO records were found return PMFalse
                    If Not Informations.IsArray(r_vKeyArray) Then
                        Return gPMConstants.PMEReturnCode.PMNotFound
                    End If
                    m_oKeysCollection.Add(sKey.ToString, r_vKeyArray)
                End If
                '***************
                ' MEvans : 13/08/2003 : 223 Document Production
                ' for adhoc documents we can produce documents for specific
                ' items such as claim peril and claim debts..

                If ProcessAdhocDocOverrides(v_sTableName:=v_sTableName, r_vKeyArray:=r_vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLoopKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLoopKeys")

                End If
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLoopKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLoopKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetStandardWordings
    '
    ' Description:
    '
    ' History: 23/04/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Overloads Function GetStandardWordings(ByRef r_vStandardWordingsArray(,) As Object, Optional ByVal vInsFileCnt As Object = Nothing, Optional ByVal vRiskCnt As Object = Nothing, Optional ByVal vStandardWordingProperty As String = "", Optional ByVal vChildID As Object = Nothing, Optional sTableName As String = "") As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Informations.IsNothing(vRiskCnt) Then

                If Not Informations.IsNothing(vStandardWordingProperty) Then

                    If Not Informations.IsNothing(vStandardWordingProperty) Then

                        m_lReturn = CType(GetRiskStandardWordings(CInt(vRiskCnt), r_vStandardWordingsArray, vStandardWordingProperty, vChildID, sTableName), gPMConstants.PMEReturnCode)
                    Else

                        m_lReturn = CType(GetRiskStandardWordings(CInt(vRiskCnt), r_vStandardWordingsArray, vStandardWordingProperty, sTableName), gPMConstants.PMEReturnCode)
                    End If
                Else

                    m_lReturn = CType(GetRiskStandardWordings(CInt(vRiskCnt), r_vStandardWordingsArray), gPMConstants.PMEReturnCode)
                End If
            Else

                m_lReturn = CType(GetPolicyStandardWordings(CInt(vInsFileCnt), r_vStandardWordingsArray), gPMConstants.PMEReturnCode)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStandardWordings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Overloads Function GetStandardWordings(ByRef r_vStandardWordingsArray(,) As Object, ByVal sStandardWordingTag As String, Optional ByVal vInsFileCnt As Object = Nothing, Optional ByVal vRiskCnt As Object = Nothing, Optional ByVal vStandardWordingProperty As String = "", Optional ByVal vChildID As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Informations.IsNothing(vRiskCnt) Then

                If Not Informations.IsNothing(vStandardWordingProperty) Then

                    If Not Informations.IsNothing(vStandardWordingProperty) Then

                        m_lReturn = CType(GetRiskStandardWordings(CInt(vRiskCnt), r_vStandardWordingsArray, vStandardWordingProperty, vChildID), gPMConstants.PMEReturnCode)
                    Else

                        m_lReturn = CType(GetRiskStandardWordings(CInt(vRiskCnt), r_vStandardWordingsArray, vStandardWordingProperty), gPMConstants.PMEReturnCode)
                    End If
                Else

                    m_lReturn = CType(GetRiskStandardWordings(CInt(vRiskCnt), r_vStandardWordingsArray), gPMConstants.PMEReturnCode)
                End If
            Else

                m_lReturn = CType(GetPolicyStandardWordings(CInt(vInsFileCnt), r_vStandardWordingsArray, sStandardWordingTag), gPMConstants.PMEReturnCode)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStandardWordings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDataModel
    '
    ' Description:
    '
    ' History: 23/04/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetDataModel(ByRef lRiskCnt As Integer, ByRef r_sDataModelCode As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "exec spu_get_GIS_data_model " & lRiskCnt

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            If Informations.IsArray(vResultArray) Then

                r_sDataModelCode = CStr(vResultArray(0, 0)).Trim()
            End If

            vResultArray = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataModel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataModel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRiskStandardWordings
    '
    ' Description:
    '
    ' History: 23/04/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetRiskStandardWordings(ByRef lRiskCnt As Integer, ByRef r_vStandardWordings(,) As Object, Optional ByVal vStandardWordingProperty As String = "", Optional ByVal vChildID As Object = Nothing, Optional sTableName As String = "") As Integer

        Dim result As Integer = 0
        Dim sDataModelCode As String = ""
        Dim sStdWrdTableName As String = ""
        Dim sPolicyBinderTableName As String = ""
        Dim sPolicyBinderId As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""

        Const sCOMMON_STD_WRD_TABLE_NAME As String = "_standard_wording"
        Const sCOMMON_POL_BIND_TABLE_NAME As String = "_policy_binder"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetDataModel(lRiskCnt:=lRiskCnt, r_sDataModelCode:=sDataModelCode), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Data Model", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskStandardWordings", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            sStdWrdTableName = sDataModelCode & sCOMMON_STD_WRD_TABLE_NAME
            sPolicyBinderTableName = sDataModelCode & sCOMMON_POL_BIND_TABLE_NAME
            sPolicyBinderId = sPolicyBinderTableName & "_id"


            If Not Informations.IsNothing(vStandardWordingProperty) Then


                If Not Informations.IsNothing(vChildID) Then
                    sSQL = ""
                    sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "    gpx.column_name" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "FROM gis_property gp" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "JOIN gis_property gpx" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "    ON gpx.gis_object_id = gp.gis_object_id" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & " JOIN gis_object obj ON OBj.gis_object_id  = gpx.gis_object_id " & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "WHERE gp.property_name = '" & vStandardWordingProperty & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "AND gpx.gis_property_id = " & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "    (" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "        SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "            MAX(gis_property_id)" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "        FROM gis_property" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "        WHERE gis_object_id = gpx.gis_object_id" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "        AND is_primary_key = 1" & Strings.ChrW(13) & Strings.ChrW(10)
                    sSQL = sSQL & "    )" & Strings.ChrW(13) & Strings.ChrW(10)
                    If sTableName <> "" Then
                        sSQL = sSQL & " AND obj.table_name = '" + sTableName + "'"
                    End If
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, vResultArray:=vResultArray)
                End If

                sSQL = ""
                sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    sw.sequence_id," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    sw.document_template_id," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    dt.document_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    dt.code," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    dt.description" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "FROM " & sStdWrdTableName & " sw" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "JOIN " & sPolicyBinderTableName & " pb" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    ON pb." & sPolicyBinderId & " = sw." & sPolicyBinderId & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "JOIN gis_policy_link gpl" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    ON gpl.gis_policy_link_id = pb.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "JOIN gis_property gp" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    ON gp.property_name = '" & vStandardWordingProperty & "'" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    AND gp.gis_property_id = sw.gis_property_id" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    AND gp.gis_object_id = sw.gis_object_id" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "JOIN document_template dt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    ON dt.document_template_id = sw.document_template_id" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE gpl.risk_id = " & CStr(lRiskCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                If Informations.IsArray(vResultArray) Then


                    sSQL = sSQL & "AND sw." & CStr(vResultArray(0, 0)) & " = " & CStr(vChildID) & Strings.ChrW(13) & Strings.ChrW(10)
                End If
                sSQL = sSQL & "ORDER BY sw.sequence_id" & Strings.ChrW(13) & Strings.ChrW(10)

            Else

                sSQL = ""
                sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    sw.sequence_id," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    sw.document_template_id," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    dt.document_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    dt.code," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    dt.description" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "FROM " & sStdWrdTableName & " sw" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "JOIN " & sPolicyBinderTableName & " pb" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    ON pb." & sPolicyBinderId & " = sw." & sPolicyBinderId & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "JOIN gis_policy_link gpl" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    ON gpl.gis_policy_link_id = pb.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "JOIN document_template dt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    ON dt.document_template_id = sw.document_template_id" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE gpl.risk_id = " & CStr(lRiskCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "ORDER BY" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    sw.gis_property_id," & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "    sw.sequence_id" & Strings.ChrW(13) & Strings.ChrW(10)

            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, vResultArray:=vResultArray)

            If Informations.IsArray(vResultArray) Then

                r_vStandardWordings = vResultArray
            End If

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskStandardWordings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPolicyStandardWordings
    '
    ' Description:
    '
    ' History: 23/04/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Overloads Function GetPolicyStandardWordings(ByRef lInsFileCnt As Integer, ByRef r_vStandardWordings(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = ""
            sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    psw.policy_standard_wording_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    psw.document_template_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    dt.document_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    dt.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    dt.description" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM policy_standard_wording psw" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN document_template dt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    ON dt.document_template_id = psw.document_template_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE insurance_file_cnt = " & CStr(lInsFileCnt) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND psw.do_not_merge = 0 " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ORDER BY policy_standard_wording_id" & Strings.ChrW(13) & Strings.ChrW(10)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, vResultArray:=vResultArray)

            If Informations.IsArray(vResultArray) Then

                r_vStandardWordings = vResultArray
            End If

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyStandardWordings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Overloads Function GetPolicyStandardWordings(ByRef lInsFileCnt As Integer, ByRef r_vStandardWordings(,) As Object, ByVal sStandardWordingTag As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = ""
            sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    psw.policy_standard_wording_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    psw.document_template_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    dt.document_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    dt.code," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    dt.description" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM policy_standard_wording psw" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "JOIN document_template dt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "    ON dt.document_template_id = psw.document_template_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE insurance_file_cnt = " & CStr(lInsFileCnt) & Strings.ChrW(13) & Strings.ChrW(10)
            If sStandardWordingTag = "DESC" Then
                sSQL = sSQL & "ORDER BY dt.description asc" & Strings.ChrW(13) & Strings.ChrW(10)
            ElseIf sStandardWordingTag = "CODE" Then
                sSQL = sSQL & "ORDER BY isnumeric(dt.code)asc,dt.code" & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "ORDER BY policy_standard_wording_id" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, vResultArray:=vResultArray)

            If Informations.IsArray(vResultArray) Then

                r_vStandardWordings = vResultArray
            End If

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyStandardWordings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyStandardWordings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRiskID
    '
    ' Description: get risk id for this claim
    '
    ' History: 15/08/2001 TN - Created.
    '
    ' ***************************************************************** '
    Public Function GetRiskID(ByVal v_lClaimCnt As Integer, ByRef r_lRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '**************** risk_type_id in claim is the risk_id*****************
            sSQL = "SELECT Risk_type_id FROM Claim WHERE Claim_id = {claim_id}"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Risk ID", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lRiskCnt = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' Created By:  'RWH(09/11/2000)
    ' 06/06/2002 SP - moved to uniform Product Options scheme
    ' ***************************************************************** '
    Public Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Try


            Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetLoopCount
    '
    ' Description: Returns the number of loop lines in a loop.
    '
    ' History: 22/05/2002 DJM - Created.
    '
    ' ***************************************************************** '
    Public Function GetLoopCount(ByRef r_lLoopCount As Integer, ByVal v_sLoopName As String, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimCnt As Integer, ByVal v_sDocumentRef As String, Optional ByVal v_lRiskID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim sSPname As String = ""
        Dim vResultArray(,) As Object = Nothing

        '*************************************
        ' ME : 02-12-2002 : 202
        Dim bAddPurchaseOrderId As Boolean
        Dim lPurchaseOrderID As Integer
        '*************************************
        Const sGET_KEYS_START As String = "spu_wp_"
        Const sGET_KEYS_END As String = "count"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSPname = sGET_KEYS_START & v_sLoopName & sGET_KEYS_END

            '*************************************
            ' ME : 02-12-2002 : 202
            ' initialise
            bAddPurchaseOrderId = False

            ' Get Main Group for Field to determine whether
            ' or not to add purchase order id param to stored procedure call
            If v_sLoopName.ToUpper() = "POIPURCHASEORDERITEMS" Or v_sLoopName.ToUpper() = "LSILOSSSCHEDULEITEMS" Then

                bAddPurchaseOrderId = True

                ' Get purchase order id from field params array
                If Informations.IsArray(m_vFieldParams) Then
                    If GetFieldProcValue(gPMConstants.ACParamNamePurchaseOrderId, lPurchaseOrderID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        lPurchaseOrderID = 0
                    End If
                Else
                    ' Default Values
                    lPurchaseOrderID = 0
                End If
            End If
            '*************************************

            'Thinh Nguyen 25/06/2003 (start) - pass in risk id
            sSQL = "exec " & sSPname & " " & CStr(v_lPartyCnt) & ", " & CStr(v_lInsuranceFileCnt) & ", " & CStr(v_lRiskID) & ", "
            'Thinh Nguyen 25/06/2003 (end) - pass in risk id

            sSQL = sSQL & CStr(v_lClaimCnt) & ", " & "'" & v_sDocumentRef & "', " & CStr(0) & "," & CStr(0) & "," & CStr(0)

            With m_oDatabase

                .Parameters.Clear()

                m_lError = .SQLSelect(sSQL:=sSQL, sSQLName:=sSPname, bStoredProcedure:=False, vResultArray:=vResultArray)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLoopCount")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If NO records were found return PMFalse
                If Not Informations.IsArray(vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With


            r_lLoopCount = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLoopCount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLoopCount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetKeysExists
    '
    ' Description: Checks for stored procedure.
    '
    ' History: DJM 29/08/2002 : Created.
    '
    ' ***************************************************************** '
    Public Function GetKeysExists(ByVal v_sLoopName As String) As Integer

        Dim result As Integer = 0
        Dim sSPname As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim sStoredProcName As String = ""

        'Const sGET_KEYS_START As String = "spg_wp_"     '' Unused Local Var
        Const sGET_KEYS_END As String = "_get_keys"
        'Const sGET_KEYS_START_RISKTAX As String = "spu_wp_"     '' Unused Local Var

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetStoredProcName(v_sLoopName:=v_sLoopName, r_sStoredProcName:=sStoredProcName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSPname = sStoredProcName & sGET_KEYS_END

            '        'If risk tax then treat manually created script like auto generated
            '        If v_sLoopName = "RiskTax" Then
            '            sSPname = sGET_KEYS_START_RISKTAX & v_sLoopName & sGET_KEYS_END
            '        Else
            '            sSPname = sGET_KEYS_START & v_sLoopName & sGET_KEYS_END
            '        End If
            'Thinh Nguyen 25/06/2003 (end) - get prefix from wp_fields

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="sp_name", vValue:=sSPname, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                'developer guide no.39
                m_lError = .SQLSelect(sSQL:="spu_get_stored_proc", sSQLName:="spu_get_stored_proc", bStoredProcedure:=True, vResultArray:=vResultArray)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeysExists")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If NO records were found return PMFalse
                If Not Informations.IsArray(vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeysExists", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetPrimaryKeyExists
    '
    ' Description: Checks for stored procedure.
    '
    ' History: 22/05/2002 DJM - Created.
    '
    ' ***************************************************************** '
    Public Function GetParentKeyExists(ByVal v_sLoopName As String) As Integer

        Dim result As Integer = 0
        Dim sSPname As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim sStoredProcName As String = ""

        Const sGET_KEYS_END As String = "_get_parent_key"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetStoredProcName(v_sLoopName:=v_sLoopName, r_sStoredProcName:=sStoredProcName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSPname = sStoredProcName & sGET_KEYS_END

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="sp_name", vValue:=sSPname, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lError = .SQLSelect(sSQL:="spu_get_stored_proc", sSQLName:="spu_get_stored_proc", bStoredProcedure:=True, vResultArray:=vResultArray)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParentKeyExists")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If NO records were found return PMFalse
                If Not Informations.IsArray(vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParentKeyExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParentKeyExists", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:GetFieldProcValue
    '
    ' Parameters:   v_ParameterName - specifies which parameter to return
    '               r_vValue        - the value for the specified param
    '
    ' Description: Returns the specified sproc parameter value from the
    '                   array.
    ' History:
    '           Created : MEvans : 29-11-2002 : 202
    ' ***************************************************************** '
    Private Function GetFieldProcValue(ByVal v_sParameterName As String, ByRef r_vValue As Integer) As Integer

        Dim result As Integer = 0
        Dim iParamType As Integer
        Dim nParams As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        nParams = m_vFieldParams.GetUpperBound(1)

        ' For each parameter in the array
        For iParam As Integer = 0 To nParams

            ' Find the one we are looking for
            If CStr(m_vFieldParams(gPMConstants.ACParamName, iParam)) = v_sParameterName Then

                ' Get parameter type
                iParamType = CInt(m_vFieldParams(gPMConstants.ACParamType, iParam))

                ' Cast the correct value to the required type

                Select Case iParamType
                    Case gPMConstants.PMEDataType.PMLong
                        r_vValue = CInt(m_vFieldParams(gPMConstants.ACParamValue, iParam))
                    Case Else
                        r_vValue = 0

                End Select

                ' quit for
                iParam = nParams
            End If
        Next

        Return result

    End Function


    '**********************************************************************
    ' Name  : GetStoredProcName
    '
    ' Desc  : get the first part of stored procedure in wp_fields up to _wp_
    '
    'Author : Thinh Nguyen 25/06/2003
    '**********************************************************************
    Private Function GetStoredProcName(ByVal v_sLoopName As String, ByRef r_sStoredProcName As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        v_sLoopName = v_sLoopName.ToLower

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oStoredProcNames.ContainsKey(v_sLoopName) Then
            m_oStoredProcNames.TryGetValue(v_sLoopName, r_sStoredProcName)
            Return result
        End If


        ' Changed the above SQL to the following SQL (Requested by Thinh Nguyen)
        sSQL = "SELECT TOP 1 sql FROM wp_fields WHERE (loop1 = {loop1} AND loop2 Is Null) OR (loop2 = {loop2} AND loop3 Is Null) OR (loop3 = {loop3} AND loop4 Is Null) OR loop4 = {loop4}"
        sSQL = sSQL & "ORDER BY loop1, loop2, loop3, loop4" 'PN25916

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="loop1", vValue:=v_sLoopName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="loop2", vValue:=v_sLoopName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="loop3", vValue:=v_sLoopName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="loop4", vValue:=v_sLoopName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get stored procedure prefix", bStoredProcedure:=False, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        r_sStoredProcName = CStr(vResultArray(0, 0))
        m_oStoredProcNames.Add(v_sLoopName, r_sStoredProcName)
        Return result

    End Function

    '**********************************************************************
    ' Name  : GetPolicyCurrency
    '
    ' Desc  : get currency details to use in document production from policy or document_ref
    '
    'Author : Thinh Nguyen 25/02/2004
    '**********************************************************************
    Public Function GetDocumentCurrency(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sDocumentRef As String, ByRef r_vResultArray(,) As Object, Optional ByVal v_lPartyCnt As Integer = 0) As Integer

        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            lReturnValue = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lReturnValue = gPMConstants.PMEReturnCode.PMFalse
                Return lReturnValue
            End If

            If v_sDocumentRef.Trim().Length = 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=CStr(gPMConstants.VariantType_Null), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lReturnValue = gPMConstants.PMEReturnCode.PMFalse
                Return lReturnValue
            End If

            If v_lPartyCnt > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="PartyCnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                'Pass Null to handle unexpected

                'Modified by Sudhanshu Behera on 4/26/2010 4:57:11 PM refer developer guide no. 85(guide)
                m_lReturn = m_oDatabase.Parameters.Add("PartyCnt", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lReturnValue = gPMConstants.PMEReturnCode.PMFalse
                Return lReturnValue
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_GetDocCurrency", sSQLName:="Get Document Currency Detail", bStoredProcedure:=True, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lReturnValue = gPMConstants.PMEReturnCode.PMFalse
                Return lReturnValue
            End If


        Catch ex As Exception

            lReturnValue = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get currency detail for document", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            Return lReturnValue
        Finally


        End Try
        Return lReturnValue
    End Function


    ' ***************************************************************** '
    ' Name          : GetCurrencyDetails
    ' Description   : Function to Fetch the Currency details, for the supplied
    '                   currency id
    ' Edit History  :
    ' RAM20050104   : Created
    ' ***************************************************************** '
    Public Function GetCurrencyDetails(ByVal v_iCurrencyID As Integer, ByRef r_vCurrencyDetails As Object) As Integer

        Dim result As Integer = 0
        Dim sSQLString As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Modified by Upendra Singh on 6/4/2010 8:35:50 PM refer developer guide no. 40
            sSQLString = "spu_currency_sel"

            With m_oDatabase

                .Parameters.Clear()

                ' Note : For some reasons, if the currency id is zero, send the
                '           business component's currency id details
                If v_iCurrencyID < 1 Then
                    v_iCurrencyID = m_iCurrencyID
                End If

                'm_lReturn = .Parameters.Add("currency_id", CStr(v_iCurrencyID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                m_lReturn = .Parameters.Add("currency_id", v_iCurrencyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=sSQLString, sSQLName:="GetCurrencyDetails", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            End With

            If Informations.IsArray(vResultArray) Then


                r_vCurrencyDetails = vResultArray
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrencyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrencyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name          : GetSubDocumentsList
    '
    ' Description   : Retrieves all Sub-Documents (document_type.code = "SUBDOC")
    '
    ' Edit History  :
    ' RAM20050104   : Created
    ' ***************************************************************** '
    Public Function GetSubDocumentsList(ByRef r_vSubDocumentsArray As Object) As Integer

        Dim result As Integer = 0
        Const ACGetSubDocumentsListStored As Boolean = True
        Const ACGetSubDocumentsListName As String = "GetSubDocumentsList"
        Const ACGetSubDocumentsListSQL As String = "spu_get_sub_document_template_list"

        Dim vResultArray(,) As Object = Nothing
        Dim lRecords As Integer

        Try

            lRecords = gPMConstants.PMAllRecords

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


                ' Execute SQL Statement
                m_lError = .SQLSelect(sSQL:=ACGetSubDocumentsListSQL, sSQLName:=ACGetSubDocumentsListName, bStoredProcedure:=ACGetSubDocumentsListStored, lNumberRecords:=lRecords, vResultArray:=vResultArray)

            End With

            ' Return the data


            r_vSubDocumentsArray = vResultArray

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubDocumentsList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubDocumentsList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name          : GetSubDocumentTemplateIdFromCode
    '
    ' Description   : Get the Sub Document details from the supplied code
    '
    ' Edit History  :
    ' RAM20050117   : Created
    ' ***************************************************************** '
    Public Function GetSubDocumentTemplateIdFromCode(ByVal v_sSubDocumentTemplateCode As String, ByRef r_lSubDocumentTemplateID As Integer, ByRef r_lSubDocumentTemplateTypeID As Integer, ByRef r_sSubDocumentTemplateDescription As String) As Integer

        Dim result As Integer = 0
        Dim sDocDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_sSubDocumentTemplateCode.Trim() = "" Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SubDocumentTemplateCode is Missing", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubDocumentTemplateIdFromCode", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sSubDocumentTemplateCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(r_lSubDocumentTemplateID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_type_id", vValue:=CStr(r_lSubDocumentTemplateTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'AJM 8/3/01 - need to add description parameter so that SP works
            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=r_sSubDocumentTemplateDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetSubDocumentTemplateSQL, sSQLName:=ACGetSubDocumentTemplateName, bStoredProcedure:=ACGetSubDocumentTemplateStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Return the values

            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("document_template_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("document_template_id").Value)) Then
                r_lSubDocumentTemplateID = m_oDatabase.Parameters.Item("document_template_id").Value
            End If

            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("document_type_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("document_type_id").Value)) Then
                r_lSubDocumentTemplateTypeID = m_oDatabase.Parameters.Item("document_type_id").Value
            End If

            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("description").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("description").Value)) Then
                r_sSubDocumentTemplateDescription = m_oDatabase.Parameters.Item("description").Value
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubDocumentTemplateIdFromCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubDocumentTemplateIdFromCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ProcessAdhocDocOverrides
    '
    ' Parameters: n/a
    '
    ' Description:  MEvans : 13/08/2003 : 223 Document Production
    '           Adhoc documents and workflow documents can be
    '           produced for specific
    '           items such as claim peril and claim debts..
    '           i.e. if we have a specifed claim peril id we want to look at then
    '           use the specified key instead of all the keys and repopulate
    '           the r_vKeyArray accordingly
    '
    ' History:
    '           Created : MEvans : 11-02-2004 : CQ2643
    ' ***************************************************************** '
    Private Function ProcessAdhocDocOverrides(ByVal v_sTableName As String, ByRef r_vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Dim sSearchKey As String = ""
        Dim lLbound, lUbound As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        If Informations.IsArray(m_vFieldParams) Then

            lLbound = m_vFieldParams.GetLowerBound(1)
            lUbound = m_vFieldParams.GetUpperBound(1)

            For lItem As Integer = lLbound To lUbound

                If (v_sTableName.IndexOf(m_vFieldParams(0, lItem), StringComparison.CurrentCultureIgnoreCase) + 1) <> 0 Then
                    ReDim r_vKeyArray(0, 0)

                    r_vKeyArray(0, 0) = m_vFieldParams(1, lItem)
                End If

            Next

        End If

        Return result
    End Function
    ''' <summary>
    ''' Get Risk Clauses
    ''' </summary>
    ''' <param name="r_vClauses"></param>
    ''' <param name="vRiskCnt"></param>
    ''' <param name="vId"></param>
    ''' <param name="vClauseProperty"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '
    ' ***************************************************************** '
    Public Function GetRiskClauses(ByRef r_vClauses As Object, ByVal vRiskCnt As Object, ByVal vId As Object, ByVal vClauseProperty As String) As Integer

        Dim result As Integer = 0
        Dim oField As bSIRFieldManager.Field
        Dim colName, sStdWrdTableName, sPolicyBinderTableName, sPolicyBinderId As String
        Dim sDataModelCode As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""
        Dim sTableName As String = ""
        Const sCOMMON_STD_WRD_TABLE_NAME As String = "_standard_wording"
        Const sCOMMON_POL_BIND_TABLE_NAME As String = "_policy_binder"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oField = m_oFields(vClauseProperty.ToUpper())
            If Not Informations.IsNothing(oField) Then
                colName = oField.ColumnName
                sTableName = oField.TableName
            Else
                colName = ""
            End If

            If CInt(vRiskCnt) > 0 And colName.Trim() > "" Then

                m_lReturn = CType(GetDataModel(lRiskCnt:=CInt(vRiskCnt), r_sDataModelCode:=sDataModelCode), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(vRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="col_name", vValue:=colName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="table_prefix", vValue:=sDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not String.IsNullOrEmpty(sTableName) Then
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="table_name", vValue:=sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskClauseInfoSQL, sSQLName:=ACGetRiskClauseInfoName, bStoredProcedure:=ACGetRiskClauseInfoStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(vResultArray) Then
                    sStdWrdTableName = sDataModelCode & sCOMMON_STD_WRD_TABLE_NAME
                    sPolicyBinderTableName = sDataModelCode & sCOMMON_POL_BIND_TABLE_NAME
                    sPolicyBinderId = sPolicyBinderTableName & "_id"

                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(vRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="StdWrdTableName", vValue:=sStdWrdTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="PolicyBinderTableName", vValue:=sPolicyBinderTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="PolicyBinderId", vValue:=sPolicyBinderId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="colName", vValue:=colName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="parentCol", vValue:=CStr(vResultArray(3, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="vID", vValue:=Convert.ToString(vId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskClausesSQL, sSQLName:=ACGetRiskClausesName, bStoredProcedure:=ACGetRiskClausesStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    r_vClauses = vResultArray
                End If
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskClauses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskClauses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oDT"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllClauses(ByRef oDT As DataTable) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sSQL As String = ""

        Try

            m_oDatabase.Parameters.Clear()

            '  m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskClauseInfoSQL, sSQLName:=ACGetRiskClauseInfoName, bStoredProcedure:=ACGetRiskClauseInfoStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            m_oDatabase.ExecuteDataTable(sSQL:=kGetAllRiskClauseInfoSQL, bStoredProcedure:=True, sSQLName:=kGetAllRiskClauseInfoName, oRecordset:=oDT)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskClauses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskClauses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
        Return nResult
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetPolicyEffectiveDate
    '
    ' Description:
    '
    ' History: 26/04/2007 VB - Created.
    '
    ' ***************************************************************** '
    Public Function GetPolicyEffectiveDate(ByVal v_lInsuranceFileCnt As Integer, ByRef r_dtEffectiveDate As Date) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyEffectiveDate"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=r_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            ' Execute SQL Statement

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetPolicyEffectiveDateSQL, sSQLName:=ACGetPolicyEffectiveDateName, bStoredProcedure:=ACGetPolicyEffectiveDateStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New Exception
            End If


            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("effective_date").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("effective_date").Value)) Then
                r_dtEffectiveDate = gPMFunctions.ToSafeDate(m_oDatabase.Parameters.Item("effective_date").Value)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyEffectiveDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
    Public Function GetActualRiskForLoopPolicy(ByVal v_lInsuranceFileCnt As Integer, ByVal s_RiskTypeCode As String, ByRef r_lRiskID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GetActualRiskForLoopPolicy")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "select r.risk_cnt from risk r " &
                   "inner join insurance_file_risk_link ifrl on r.risk_cnt=ifrl.risk_cnt " &
                   "inner join insurance_file ins on ins.insurance_file_cnt = ifrl.insurance_file_cnt " &
                   "inner join risk_type rt on r.risk_type_id = rt.risk_type_id " &
                   "WHERE ins.insurance_file_cnt = " & CStr(v_lInsuranceFileCnt) & " AND rt.code like '%" & s_RiskTypeCode & "%'"

            ' Call it
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetActualRiskForLoopPolicy", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Grab the return value
            If Informations.IsArray(vResultArray) Then
                'Thinh Nguyen 10/07/2003 (start) - it will return a Null if nothing is found

                If CStr(vResultArray(0, 0)) <> "" Then

                    r_lRiskID = CInt(vResultArray(0, 0))
                End If
                'Thinh Nguyen 10/07/2003 (end) - it will return a Null if nothing is found
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GetRiskForPolicy")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".GetActualRiskForLoopPolicy")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetActualRiskForLoopPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetActualRiskForLoopPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetPreviousLivePolicyDetails(ByVal v_lInsuranceFileCnt As Long,
                                                 ByVal v_lRiskCnt As Long,
                                       ByRef r_vResultArray(,) As Object) As Long


        Const kMethodName As String = "GetPreviousLivePolicyDetails"
        Dim nResult As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="nInsuranceFileCnt",
                                                   vValue:=v_lInsuranceFileCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            nResult = m_oDatabase.Parameters.Add(sName:="nRiskCnt",
                                                   vValue:=v_lRiskCnt,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            ' Execute SQL Statement

            nResult = m_oDatabase.SQLSelect(
                                sSQL:=ACGetPreviousPolicyDetailSQL,
                                sSQLName:=ACGetPreviousPolicyDetailName,
                                bStoredProcedure:=ACGetPreviousPolicyDetailStored,
                                vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Previous Policy Detail", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            nResult = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPreviousLivePolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err.Description, excep:=ex)


        Finally
        End Try


        Return m_lReturn

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dtFields"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWPFieldsDetails(ByVal dtFields As DataTable, ByRef r_dtResultSet As DataTable, ByVal InsurancefileCnt As Integer, Optional ByVal ClaimID As Integer = 0) As Integer

        Const kMethodName As String = "GetWPFieldsDetails"
        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            Dim cmd As SqlCommand

            m_oDatabase.Parameters.Clear()

            cmd = New SqlCommand("spu_get_WPField_Details")
            cmd.Parameters.AddWithValue("@CCMWPFields", dtFields)
            cmd.Parameters.Add("Insurance_file_cnt", SqlDbType.Int).Value = InsurancefileCnt
            cmd.Parameters.Add("Claim_id", SqlDbType.Int).Value = ClaimID
            cmd.CommandType = CommandType.StoredProcedure
            ' Execute SQL Statement
            nResult = m_oDatabase.ExecuteDataTable(cmd, r_dtResultSet)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Previous Policy Detail", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            nResult = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetWPFieldsDetails Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName, excep:=ex)
        Finally
        End Try

        Return nResult

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dtFields"></param>
    ''' <param name="r_dtResultSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCCMFieldDetailsWithSpecialsType(ByVal dtFields As DataTable, ByRef r_dtResultSet As DataTable, InsuranceFileCnt As Integer, Optional ByVal ClaimID As Integer = 0) As Integer

        Const kMethodName As String = "GetWPFieldsDetails"
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try

            Dim cmd As SqlCommand

            m_oDatabase.Parameters.Clear()

            cmd = New SqlCommand("spu_get_WPField_DetailsWithSpecialsType")
            cmd.Parameters.AddWithValue("@CCMWPFields", dtFields)
            cmd.Parameters.Add("insurance_file_cnt", SqlDbType.Int).Value = InsuranceFileCnt
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("Claim_id", SqlDbType.Int).Value = ClaimID
            ' Execute SQL Statement
            nResult = m_oDatabase.ExecuteDataTable(cmd, r_dtResultSet)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to retrieve WPfield Details.", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            nResult = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetWPFieldsDetails Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName, excep:=ex)
        Finally
        End Try

        Return nResult
    End Function
    Public Function GetFieldValuesFromDB(ByVal vInstanceArray As Object(), ByVal nPartyCnt As Integer, ByVal nInsuranceFileCnt As Integer, ByVal nRiskCnt As Integer,
                                         ByVal nClaimCnt As Integer, ByVal sDocumentRef As String, ByVal nInstance1 As Integer, ByRef nInstance2 As Integer, ByRef nInstance3 As Integer,
                                         ByVal nInstance4 As Integer, ByVal sStoredProcName As String, ByRef oResultSet As DataTable) As Integer


        'For Each Value in KeyArray 
        If Informations.IsArray(vInstanceArray) Then
            nInstance1 = ToSafeInteger(vInstanceArray(0))

            If UBound(vInstanceArray) >= 1 Then
                nInstance2 = ToSafeInteger(vInstanceArray(1))
            End If
            If UBound(vInstanceArray) >= 2 Then
                nInstance3 = ToSafeInteger(vInstanceArray(2))
            End If
            If UBound(vInstanceArray) >= 3 Then
                nInstance4 = ToSafeInteger(vInstanceArray(3))
            End If
        End If

        oResultSet = New DataTable

        m_oDatabase.Parameters.Clear()
        m_oDatabase.Parameters.Add(sName:="PartyCnt", vValue:=nPartyCnt, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=nInsuranceFileCnt, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add(sName:="RiskId", vValue:=nRiskCnt, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add(sName:="ClaimCnt", vValue:=nClaimCnt, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=sDocumentRef, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        m_oDatabase.Parameters.Add(sName:="Instance1", vValue:=nInstance1, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add(sName:="Instance2", vValue:=nInstance2, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add(sName:="Instance3", vValue:=nInstance3, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If nInstance4 > 0 Then
            m_oDatabase.Parameters.Add(sName:="Instance4", vValue:=nInstance4, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        End If
        m_oDatabase.ExecuteDataTable(sSQL:=sStoredProcName, sSQLName:=sStoredProcName, bStoredProcedure:=True, oRecordset:=oResultSet)
        Return 1
    End Function

    ''' <summary>
    ''' Get All Risks For Policy
    ''' </summary>
    ''' <param name="nFileInsuranceCnt"></param>
    ''' <param name="dtRisks"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetALLRisksForPolicy(ByVal nFileInsuranceCnt As Integer, ByRef dtRisks As DataTable) As Integer
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sSQLString As String = "spu_Get_Risk_Cnt"

        Try
            m_oDatabase.Parameters.Clear()
            m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=nFileInsuranceCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            ' Execute SQL Statement
            nReturn = m_oDatabase.ExecuteDataTable(sSQL:=sSQLString, sSQLName:="sSQLString", bStoredProcedure:=True, oRecordset:=dtRisks)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException(sSQLString & " Failed to execute.")
            End If

        Catch excep As System.Exception
            nReturn = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetALLRisksForPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisksForPolicy", excep:=excep)
        End Try
        Return nReturn
    End Function

End Class

