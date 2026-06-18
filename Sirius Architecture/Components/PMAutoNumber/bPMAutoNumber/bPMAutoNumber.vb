Option Strict Off
Option Explicit On
Imports System.Text
'Developer Guide No 129. 
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business

    Implements IDisposable
    ' ************************************************
    ' Added to replace global variables 18/09/2003
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


    ' ***************************************************************** '
    ' Class Name: bPMAutoNumber
    '
    ' Date: 27th September 1997
    '
    ' Description: Autonumbering!
    '
    ' Edit History: 27/09/97    Original created                    JRW
    '               10/11/97    Updated for different product refs  JRW
    '               14/10/98    Moved Generate functions to Private  CF
    '                           Added GetNumberRange function
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Parameter Collection (Private)
    Private m_oParameters As dPMDAO.Parameters

    ' PMLookup object
    Private m_oPMLookup As bPMLookup.Business

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lError As gPMConstants.PMEReturnCode

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Component Sub Type
    Private m_sSubType As New StringsHelper.FixedLengthString(20)


    ' PRIVATE Data Members (End)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

    ' PUBLIC Property Procedures (Begin)
    ' PUBLIC Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer

        Dim result As Integer = 0
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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Have we a valid Database Object Reference?

            If (Not Informations.IsNothing(vDatabase)) And (Informations.IsReference(vDatabase)) Then
                ' Yes, so use it.
                m_oDatabase = vDatabase

                ' Do NOT Close Database in Terminate() method
                m_bCloseDatabase = False
            Else
                ' NO, Create new instance of the database object
                m_oDatabase = New dPMDAO.Database()

                ' Open the Database.
                ' NOTE that we do not pass ANY parameters as standard.
                ' Username and Password WILL ONLY  be passed when we DO NOT
                ' wish to use the Sirius Defaults.

                'BB
                ' RDC 27062002 use Comp Serv to open database
                m_lError = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close Database in Terminate() method
                m_bCloseDatabase = True
            End If

            m_oPMLookup = New bPMLookup.Business()

            m_lReturn = m_oPMLookup.Initialise(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(m_oPMLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion, gPMConstants.PMEReturnCode)

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
    ' Name: GetNumberRange (Standard Method)
    '
    ' Description: Gets the NumberRangeID
    '
    ' ***************************************************************** '
    Public Function GetNumberRange(ByVal v_sGroupCode As String, ByVal v_sRangeCode As String, ByVal v_sPMProductCode As String, ByRef r_lNumberRangeID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the product code parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ProductCode", vValue:=v_sPMProductCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the group code parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="GroupCode", vValue:=v_sGroupCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the range code parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RangeCode", vValue:=v_sRangeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the effective date parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="EffectiveDate", vValue:=(DateTime.Now).ToString(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the number range parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="NumberRangeID", vValue:=CStr(r_lNumberRangeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the Stored Procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetNumberRangeIDSQL, sSQLName:=ACGetNumberRangeIDName, bStoredProcedure:=ACGetNumberRangeIDStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the returned values

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("NumberRangeID").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("NumberRangeID").Value) Then
                'DAK200700 - This should error, rather than set default value of 1
                Throw New Exception()
            Else
                r_lNumberRangeID = m_oDatabase.Parameters.Item("NumberRangeID").Value
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get number range.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNumberRange", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GenerateNumber (Standard Method)
    '
    ' Description: Get the db to generate a unique counter
    '
    ' ***************************************************************** '
    Public Function GenerateNumber(ByVal v_lNumberRangeID As Integer, ByVal v_iUserId As Integer, ByRef r_lNumber As Integer) As Integer

        Dim result As Integer = 0
        Try

            ' Set the return value to true
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the v_lNumberRangeID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmnumber_range_id", vValue:=CStr(v_lNumberRangeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the UserID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Counter parameter (OUTPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmallocatednumber", vValue:=CStr(r_lNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DAK200700
            ' Add the Prefix parameter (OUTPUT)

            'Developer Guide No 85. 
            m_lReturn = m_oDatabase.Parameters.Add(sName:="range_prefix", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Suffix parameter (OUTPUT)

            'Developer Guide No 85.
            m_lReturn = m_oDatabase.Parameters.Add(sName:="range_suffix", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the New Range Code parameter (OUTPUT)

            'Developer Guide No 85.
            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_range_code", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAllocateNumberSQL, sSQLName:=ACAllocateNumberName, bStoredProcedure:=ACAllocateNumberStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' return the calculated counter value
            r_lNumber = m_oDatabase.Parameters.Item("pmallocatednumber").Value

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EncodeLong (Public)
    '
    ' Description: Takes a long and represents it as a 8 letter code
    ' (without vowels)
    '
    ' Author: CL030899
    '
    ' ***************************************************************** '

    Public Function EncodeLong(ByVal lNumber As Integer, ByRef r_sCode As String) As Integer

        Dim result As Integer = 0
        Dim sLetterSpace As String = ""
        Dim lTemp, lPower As Integer
        Dim iDigit As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sLetterSpace = "BCDFGHJKLMNPQRSTVWXYZ"

            lTemp = lNumber

            r_sCode = ""

            ' reduce lNumber to base 21, digit by digit

            For lCounter As Integer = 7 To 0 Step -1

                lPower = 21 ^ lCounter

                iDigit = (lTemp \ lPower) + 1

                r_sCode = r_sCode & sLetterSpace.Substring(iDigit - 1, 1)

                lTemp = lTemp Mod lPower ' continue with remainder

            Next lCounter

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncodeLong Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncodeLong", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EncodeLongV2 (Public)
    '
    ' Description: Takes a long and represents it as an 11 character code
    '              in the format xxnnnnnnnnn, where xx starts at AA, AB, AC
    '              and nnnnnnnnn is a numeric.
    '              Note that AC is the limit at this stage as 2,147,483,647
    '              is the max that can be stored in a long. Also, the
    '              numeric part will always start at 000000001 and will
    '              never be zero.
    '
    ' Author: CB210100
    '
    ' ***************************************************************** '
    Public Function EncodeLongV2(ByVal v_lNumber As Integer, ByRef r_sCode As String) As Integer

        Dim result As Integer = 0
        Dim sPrefix As String = ""
        Dim sPaddedValue As String
        Dim iDivisor As Integer
        Dim lRemainder As Integer
        Const OneBillion As Integer = 1000000000

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Find letter prefix of the given number using integer division
            iDivisor = v_lNumber \ OneBillion

            Select Case iDivisor
                Case 0
                    sPrefix = "AA"
                Case 1
                    sPrefix = "AB"
                Case 2
                    sPrefix = "AC"
            End Select

            'Find remainder of the given number
            lRemainder = v_lNumber Mod OneBillion

            'If remainder is zero then set to 1 to prevent AA000000000 etc
            If lRemainder = 0 Then lRemainder = 1

            'Zero pad remainder to be 9 characters long
            sPaddedValue = StringsHelper.Format(lRemainder, "000000000")

            'Return letter prefix and padded remainder
            r_sCode = sPrefix & sPaddedValue

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncodeLongV2 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncodeLongV2", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateNewNumber
    '
    ' Description: Generates a number for a given Product, Group and range
    '
    '
    ' History: 20/07/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GenerateNewNumber(ByVal v_sPMProductCode As String, ByVal v_sGroupCode As String, ByVal v_sRangeCode As String, ByVal v_iUserId As Integer, ByRef r_lNumber As Integer, Optional ByRef r_sPrefix As String = "", Optional ByRef r_sSuffix As String = "", Optional ByRef r_sRangeCodeOut As String = "") As Integer
        Dim result As Integer = 0
        Dim lNumberRangeID As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetNumberRange(v_sGroupCode:=v_sGroupCode, v_sRangeCode:=v_sRangeCode, v_sPMProductCode:=v_sPMProductCode, r_lNumberRangeID:=lNumberRangeID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetNewNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserId:=v_iUserId, r_lNumber:=r_lNumber, r_sPrefix:=r_sPrefix, r_sSuffix:=r_sSuffix, r_sRangeCodeOut:=r_sRangeCodeOut), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateNewNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateNewNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GenerateNumberFromSource (Standard Method)
    '
    ' Description: Get the db to generate a unique counter from Source
    '
    ' CF141098 - Changed to Private as function is no longer used
    '
    ' ***************************************************************** '

    'UPGRADE_NOTE: (7001) The following declaration (GenerateNumberFromSource) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GenerateNumberFromSource(ByVal v_lSourceID As Integer, ByVal v_iUserId As Integer, ByVal v_iTypeOfNumber As Integer, ByRef r_lNumber As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim sSQL As String = ""
    'Dim iParamCount As Integer
    'Dim iCount2 As Integer
    'Dim iCount1 As Integer
    'Dim vStatusTypes As Object ' array of relevant renewal types
    'Dim iArrayCount As Integer
    'Dim vResultArray As Object
    'Dim iNumberRangeID As Integer
    'Dim lNumber As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'iParamCount = 0
    '
    'build the sql select statement according to the parameters passed
    '
    'sSQL = ""
    'sSQL = sSQL & "SELECT DISTINCT Source."
    '
    '
    'Select Case v_iTypeOfNumber
    'Case gPMConstants.PMEAutoNumberType.PMAutoNumInsFile
    'sSQL = sSQL & "ins_file_num_range_id" & Strings.ChrW(13) & Strings.ChrW(10)
    'iParamCount += 1
    '
    'Case gPMConstants.PMEAutoNumberType.PMAutoNumInsFolder
    'sSQL = sSQL & "ins_folder_num_range_id" & Strings.ChrW(13) & Strings.ChrW(10)
    'iParamCount += 1
    '
    'Case gPMConstants.PMEAutoNumberType.PMAutoNumRiskFolder
    'sSQL = sSQL & "risk_folder_num_range_id" & Strings.ChrW(13) & Strings.ChrW(10)
    'iParamCount += 1
    '
    'Case gPMConstants.PMEAutoNumberType.PMAutoNumParty
    'sSQL = sSQL & "party_num_range_id" & Strings.ChrW(13) & Strings.ChrW(10)
    'iParamCount += 1
    '
    'Case gPMConstants.PMEAutoNumberType.PMAutoNumContact
    'sSQL = sSQL & "contact_num_range_id" & Strings.ChrW(13) & Strings.ChrW(10)
    'iParamCount += 1
    '
    'Case gPMConstants.PMEAutoNumberType.PMAutoNumAddress
    'sSQL = sSQL & "address_num_range_id" & Strings.ChrW(13) & Strings.ChrW(10)
    'iParamCount += 1
    '
    'Case Else
    ' param count will still be 0
    '
    'End Select
    '
    'sSQL = sSQL & "FROM Source" & Strings.ChrW(13) & Strings.ChrW(10)
    '
    'If v_lSourceID > -1 Then
    'sSQL = sSQL & "WHERE Source.Source_id = " & CStr(v_lSourceID)
    'End If
    '
    'append the parameters to the where clause
    '
    'If iParamCount = 0 Then
    'no parameters passed so query cannot be executed
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Execute SQL Statement
    'm_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACNumberRangeFromSourceName, bStoredProcedure:=ACNumberRangeFromSourceStored, vResultArray:=vResultArray)
    '
    ' If NO records were found return PMFalse
    'If Not Informations.IsArray(vResultArray) Then
    'result = gPMConstants.PMEReturnCode.PMNotFound
    'Else

    'iNumberRangeID = ToSafeInteger(vResultArray(0, 0))
    '
    ' next, call the generate number method....
    '
    'm_lError = CType(GenerateNumber(v_lNumberRangeID:=iNumberRangeID, v_iUserId:=m_iUserID, r_lNumber:=lNumber), gPMConstants.PMEReturnCode)
    'If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMError
    'End If
    '
    'r_lNumber = lNumber
    '
    'End If
    '
    'Return result
    '
    'Catch 
    '
    '
    '
    'If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateNumberFromSource")
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    '
    'End If
    '
    'Return result
    'End Try
    'End Function


    ' ***************************************************************** '
    ' Name: GenerateReference (Standard Method)
    '
    ' Description: Get the db to generate a string from referencetypecode
    '
    ' CF141098 - Changed to Private as function is no longer used
    '
    ' ***************************************************************** '

    Private Function GenerateReference(ByVal v_dtCoverStartDate As Date, ByVal v_dtCoverExpiryDate As Date, ByVal v_dtEffectiveDate As Date, ByVal v_lSourceID As Integer, ByVal v_lBranchID As Integer, ByVal v_lProductID As Integer, ByVal v_lTransactionTypeID As Integer, ByRef r_sGeneratedReference As String, Optional ByVal v_sReferenceTypeCode As String = "", Optional ByVal v_lReferenceTypeID As Integer = 0) As Integer

        Dim result As Integer = 0


        ' fields that will be selected by SQL
        Dim field_code As String

        Dim start_position, number_of_characters As Integer
        Dim is_counter As Boolean
        Dim delimit_character, pad_character As String

        Dim manipulate_string As String = ""
        Dim sOutput_string As New StringBuilder
        Dim manipulated_string As New StringBuilder
        Dim sSQL As New StringBuilder
        Dim iParamCount As Integer

        Dim sBranchCode As String
        Dim sProductCode As String
        Dim sProductAnalysisCode As String = String.Empty
        Dim sTransactionTypeCode As String
        Dim sSourceCode As String

        ' counter stuff
        Dim iNumberRangeID As Integer
        Dim lNumber As Integer

        ' admin...
        Dim vResultArray(,) As Object = Nothing
        Dim vSubResultArray(,) As Object = Nothing
        Dim iNumRangeID As Integer

        Dim lProductAnalysisID As Integer
        Dim sTransactionTypeBasis As String = String.Empty
        Dim sTempSQL As String

        Dim UniqueRef As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        ' check if a reference type has been supplied
        If v_sReferenceTypeCode = "" And v_lReferenceTypeID < 1 Then

            ' log the error
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GenerateReference failed: No ReferenceType supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateReference")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' if ReferenceTypeID is not supplied, get the ID using the CODE
        If v_lReferenceTypeID < 1 Then

            m_lReturn = m_oPMLookup.GetEffectiveIDFromCode(v_sTableName:="PMReference_Type", v_sCode:=v_sReferenceTypeCode, v_dtEffectiveDate:=v_dtEffectiveDate, r_lID:=v_lReferenceTypeID)

        End If

        ' use PMLookup to get the codes for Branch,Source.....etc


        m_lReturn = CType(GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, iLanguageID:=m_iLanguageID, vResultArray:=vSubResultArray, lSourceID:=v_lSourceID, lBranchID:=v_lBranchID, lProductID:=v_lProductID, lTransactionTypeID:=v_lTransactionTypeID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sSourceCode = CStr(vSubResultArray(2, 0)).Trim().ToUpper()

        sBranchCode = CStr(vSubResultArray(2, 1)).Trim().ToUpper()

        sProductCode = CStr(vSubResultArray(2, 2)).Trim().ToUpper()

        sTransactionTypeCode = CStr(vSubResultArray(2, 3)).Trim().ToUpper()

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the v_lNumberRangeID parameter (INPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="ReferenceTypeID", vValue:=CStr(v_lReferenceTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGenerateReferenceSQL, sSQLName:=ACGenerateReferenceName, bStoredProcedure:=ACGenerateReferenceStored, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then

            Return gPMConstants.PMEReturnCode.PMNotFound

        End If

        sOutput_string = New StringBuilder("")

        If Informations.IsArray(vResultArray) Then
            ' loop through the data...

            For iCount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)


                start_position = ToSafeInteger(vResultArray(1, iCount))

                number_of_characters = ToSafeInteger(vResultArray(2, iCount))

                is_counter = CBool(vResultArray(3, iCount))

                delimit_character = CStr(vResultArray(4, iCount)).Trim()

                pad_character = CStr(vResultArray(5, iCount)).Trim()

                field_code = CStr(vResultArray(6, iCount)).Trim()

                sSQL = New StringBuilder("")
                sSQL.Append("select pmnumber_range_id" & Strings.ChrW(13) & Strings.ChrW(10))
                iNumberRangeID = -1


                Select Case field_code
                    Case "CSD"
                        If is_counter Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GenerateReference failed: Cannot have a CSD Counter", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateReference")

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        manipulate_string = (v_dtCoverStartDate).ToString
                        iNumberRangeID = 0

                    Case "CED"
                        If is_counter Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GenerateReference failed: Cannot have a CED Counter", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateReference")

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        manipulate_string = v_dtCoverExpiryDate.ToString()
                        iNumberRangeID = 0

                    Case "BC"
                        If sBranchCode.Length > 0 Then
                            sSQL.Append("FROM Branch" & Strings.ChrW(13) & Strings.ChrW(10))
                            sSQL.Append("WHERE Code = '" & sBranchCode & "'")
                            iParamCount += 1
                        End If
                        manipulate_string = sBranchCode.ToUpper()

                    Case "PC"
                        If sProductCode.Length > 0 Then
                            sSQL.Append("FROM Product" & Strings.ChrW(13) & Strings.ChrW(10))
                            sSQL.Append("WHERE Code = '" & sProductCode & "'")
                            iParamCount += 1
                        End If
                        manipulate_string = sProductCode.ToUpper()

                    Case "PAC"
                        If v_lProductID > -1 Then
                            ' get the product_analysis_code...
                            sTempSQL = "SELECT Product_Analysis_ID FROM " &
                                       "Product WHERE Product_ID = " & CStr(v_lProductID)

                            ' Execute SQL Statement
                            m_lError = m_oDatabase.SQLSelect(sSQL:=sTempSQL, sSQLName:=ACGetNumberRangeName, bStoredProcedure:=ACGetNumberRangeStored, vResultArray:=vSubResultArray)

                            If Informations.IsArray(vSubResultArray) Then

                                lProductAnalysisID = ToSafeInteger(vSubResultArray(0, 0))
                            End If

                            sTempSQL = "SELECT code FROM Product_Analysis " &
                                       "WHERE Product_Analysis_ID = " & CStr(lProductAnalysisID)

                            ' Execute SQL Statement
                            m_lError = m_oDatabase.SQLSelect(sSQL:=sTempSQL, sSQLName:=ACGetNumberRangeName, bStoredProcedure:=ACGetNumberRangeStored, vResultArray:=vSubResultArray)

                            If Informations.IsArray(vSubResultArray) Then

                                sProductAnalysisCode = CStr(vSubResultArray(0, 0)).Trim()
                            End If

                            sTempSQL = ""

                            sSQL.Append("FROM Product_Analysis" & Strings.ChrW(13) & Strings.ChrW(10))
                            sSQL.Append("WHERE Code = '" & sProductAnalysisCode & "'")
                            iParamCount += 1
                        End If
                        manipulate_string = sProductAnalysisCode.ToUpper()

                    Case "TTC"
                        If sTransactionTypeCode.Length > 0 Then
                            sSQL.Append("FROM Transaction_Type" & Strings.ChrW(13) & Strings.ChrW(10))
                            sSQL.Append("WHERE Code = '" & sTransactionTypeCode & "'")
                            iParamCount += 1
                        End If
                        manipulate_string = sTransactionTypeCode.ToUpper()

                    Case "TB"
                        If is_counter Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GenerateReference failed: Cannot have a TB Counter", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateReference")

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If sTransactionTypeCode.Length > 0 Then
                            ' get the transaction_type_basis...
                            sTempSQL = "SELECT Transaction_Type_Basis FROM " &
                                       "Transaction_Type WHERE Transaction_Type_ID = " & CStr(v_lTransactionTypeID)

                            ' Execute SQL Statement
                            m_lError = m_oDatabase.SQLSelect(sSQL:=sTempSQL, sSQLName:=ACGetNumberRangeName, bStoredProcedure:=ACGetNumberRangeStored, vResultArray:=vSubResultArray)

                            If Informations.IsArray(vSubResultArray) Then

                                sTransactionTypeBasis = CStr(vSubResultArray(0, 0)).Trim()
                            End If

                        End If
                        manipulate_string = sTransactionTypeBasis.ToUpper()

                    Case "SC"
                        iNumberRangeID = 0
                        manipulate_string = sSourceCode.ToUpper()

                    Case Else
                        ' some kind of crazy problem!

                End Select

                If is_counter Then
                    ' counter is true, so we must retrieve the range for this groupcode
                    ' ...which means some more SQL.

                    ' dont forget error checking... on everything!

                    ' Clear the Database Parameters Collection
                    m_oDatabase.Parameters.Clear()

                    ' Add the v_lNumberRangeID parameter (INPUT)
                    If iParamCount = 0 Then
                        'no parameters passed so query cannot be executed
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Execute SQL Statement
                    m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:=ACGetNumberRangeName, bStoredProcedure:=ACGetNumberRangeStored, vResultArray:=vSubResultArray)

                    ' If NO records were found return PMFalse
                    If Not Informations.IsArray(vSubResultArray) And iNumberRangeID = -1 Then

                        result = gPMConstants.PMEReturnCode.PMNotFound
                    Else


                        iNumberRangeID = ToSafeInteger(vSubResultArray(0, 0))
                    End If

                    m_lError = CType(GenerateNumber(v_lNumberRangeID:=iNumRangeID, v_iUserId:=m_iUserID, r_lNumber:=lNumber), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    manipulate_string = CStr(lNumber).Trim()

                End If

                ' now we should have which string to manipulate, in the manipulate_string string ;)

                If number_of_characters > 0 Then
                    manipulated_string = New StringBuilder(Informations.Mid(manipulate_string, start_position, number_of_characters))
                    If manipulated_string.ToString().Trim().Length < number_of_characters Then
                        ' pad out with pad char if one exists...
                        If pad_character.Trim().Length > 0 Then
                            manipulated_string = New StringBuilder(New String(pad_character, number_of_characters - manipulated_string.ToString().Trim().Length) & manipulated_string.ToString().Trim())
                        End If
                    End If
                End If

                ' add the delimit char if it exists...
                If delimit_character.Trim().Length > 0 Then

                    manipulated_string.Append(delimit_character)
                End If

                ' add to generated reference...
                sOutput_string.Append(manipulated_string.ToString())

            Next iCount

        End If

        r_sGeneratedReference = sOutput_string.ToString()

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Private)
    '
    ' Description: Gets the Lookup values
    '
    ' CF141098 - Changed to Private as function is no longer used
    '
    ' ***************************************************************** '
    Private Function GetLookupValues(ByRef iLookupType As Integer, ByRef iLanguageID As Integer, ByRef vResultArray As Object, Optional ByRef lSourceID As Integer = -1, Optional ByRef lBranchID As Integer = -1, Optional ByRef lProductID As Integer = -1, Optional ByRef lTransactionTypeID As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim dtEffectiveDate As Date

        'Const ACValueID = 1

        ' {* USER DEFINED CODE (Begin) *}

        Dim vTabArray(3, 3) As Object
        Dim vTableArray As String = ""

        ' {* USER DEFINED CODE (End) *}



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Reset Result Array
        vResultArray = Nothing

        Dim iKeyIndex As Integer

        ' {* USER DEFINED CODE (Begin) *}

        iKeyIndex = 0

        ' Setup Lookup Table Names
        If lSourceID > 0 Then

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, iKeyIndex) = "Source"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, iKeyIndex) = lSourceID
            iKeyIndex += 1
        Else

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, iKeyIndex) = "Source"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, iKeyIndex) = 1
            iKeyIndex += 1
        End If

        If lBranchID > 0 Then

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, iKeyIndex) = "Branch"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, iKeyIndex) = lBranchID
            iKeyIndex += 1
        Else

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, iKeyIndex) = "Branch"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, iKeyIndex) = 1
            iKeyIndex += 1
        End If

        If lProductID > 0 Then

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, iKeyIndex) = "Product"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, iKeyIndex) = lProductID
            iKeyIndex += 1
        Else

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, iKeyIndex) = "Product"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, iKeyIndex) = 1
            iKeyIndex += 1
        End If

        If lTransactionTypeID > 0 Then

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, iKeyIndex) = "Transaction_Type"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, iKeyIndex) = lTransactionTypeID
        Else

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, iKeyIndex) = "Transaction_Type"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, iKeyIndex) = 1
        End If

        If iKeyIndex <> 3 Then

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 3) = ""

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 3) = ""
        End If

        dtEffectiveDate = DateTime.Now
        ' {* USER DEFINED CODE (End) *}

        ' Get the Lookup items
        m_lReturn = m_oPMLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

        ' Return the Table Array
        vTableArray = vResultArray

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GenerateInsFileReference (Private)
    '
    ' Description: Generates an Insurance File Reference (funnily enough)
    '
    ' CF141098 - Changed to Private as function is no longer used
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GenerateInsFileReference) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GenerateInsFileReference(ByVal v_iInsFileReferenceType As gPMConstants.PMEAutoNumInsFileRefType, ByVal v_dtCoverStartDate As Date, ByVal v_dtCoverExpiryDate As Date, ByVal v_dtEffectiveDate As Date, ByVal v_lSourceID As Integer, ByVal v_lBranchID As Integer, ByVal v_lProductID As Integer, ByVal v_lTransactionTypeID As Integer, ByRef r_sGeneratedReference As String) As Integer
    '
    '
    'Dim result As Integer = 0
    'Dim sSQL As String = ""
    'Dim vSubResultArray As Object
    'Dim lReferenceTypeID As Integer
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' check to see if the parameter v_iInsFileReferenceType is valid...
    '
    'If v_iInsFileReferenceType > 0 And v_iInsFileReferenceType < 5 Then
    '
    'sSQL = ""
    '
    'Select Case v_iInsFileReferenceType
    'Case gPMConstants.PMEAutoNumInsFileRefType.pmeRefTypeNBQuotation
    'sSQL = "SELECT nb_quote_ref_type_id "
    '
    'Case gPMConstants.PMEAutoNumInsFileRefType.pmeRefTypeNBMakeLive
    'sSQL = "SELECT nb_live_ref_type_id "
    '
    'Case gPMConstants.PMEAutoNumInsFileRefType.pmeRefTypeRenewalNotice
    'sSQL = "SELECT rn_notice_ref_type_id "
    '
    'Case gPMConstants.PMEAutoNumInsFileRefType.pmeRefTypeRenewalUpdate
    'sSQL = "SELECT rn_update_ref_type_id "
    '
    'Case Else
    ' kill! kill! kill!
    'Return gPMConstants.PMEReturnCode.PMFalse
    '
    'End Select
    '
    'sSQL = sSQL & "FROM Product WHERE Product_id = " & CStr(v_lProductID)
    '
    ' Execute SQL Statement
    'm_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetNumberRangeName, bStoredProcedure:=ACGetNumberRangeStored, vResultArray:=vSubResultArray)
    '
    'If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'If Informations.IsArray(vSubResultArray) Then

    'lReferenceTypeID = ToSafeInteger(vSubResultArray(0, 0))
    'End If
    '
    'm_lReturn = CType(GenerateReference(v_dtCoverStartDate:=v_dtCoverStartDate, v_dtCoverExpiryDate:=v_dtCoverExpiryDate, v_dtEffectiveDate:=v_dtEffectiveDate, v_lSourceID:=v_lSourceID, v_lBranchID:=v_lBranchID, v_lProductID:=v_lProductID, v_lTransactionTypeID:=v_lTransactionTypeID, r_sGeneratedReference:=r_sGeneratedReference, v_lReferenceTypeID:=lReferenceTypeID), gPMConstants.PMEReturnCode)
    '
    'Else
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateInsFileReference Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateInsFileReference", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'Return result
    '
    'End Function

    ' ***************************************************************** '
    ' Name: ClearParameters (Private)
    '
    ' Description: Clears the Database Parameters Collection if there
    '              are any.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ClearParameters) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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
    ' Name: GetNewNumber
    '
    ' Description: Generates a new number
    '
    ' History: 20/07/2000 DAK - Created.
    '
    'DAK110800 - Only variants can be missing or Null
    ' ***************************************************************** '
    Private Function GetNewNumber(ByVal v_lNumberRangeID As Integer, ByVal v_iUserId As Integer, ByRef r_lNumber As Integer, Optional ByRef r_sPrefix As String = "", Optional ByRef r_sSuffix As String = "", Optional ByRef r_sRangeCodeOut As String = "") As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the v_lNumberRangeID parameter (INPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="pmnumber_range_id", vValue:=CStr(v_lNumberRangeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the UserID parameter (INPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Counter parameter (OUTPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="pmallocatednumber", vValue:=CStr(r_lNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Prefix parameter (OUTPUT)
        If Not False Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="range_prefix", vValue:=r_sPrefix, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
        Else

            'Developer Guide No 85.
            m_lReturn = m_oDatabase.Parameters.Add(sName:="range_prefix", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Suffix parameter (OUTPUT)
        If Not False Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="range_suffix", vValue:=r_sSuffix, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
        Else

            'Developer Guide No 85.
            m_lReturn = m_oDatabase.Parameters.Add(sName:="range_suffix", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the New Range Code parameter (OUTPUT)
        If Not False Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_range_code", vValue:=r_sRangeCodeOut, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
        Else

            'Developer Guide No 85.
            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_range_code", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAllocateNumberSQL, sSQLName:=ACAllocateNumberName, bStoredProcedure:=ACAllocateNumberStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' return the calculated counter value

        If Convert.IsDBNull(m_oDatabase.Parameters.Item("pmallocatednumber").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("pmallocatednumber").Value) Then
            Throw New Exception()
        End If

        r_lNumber = m_oDatabase.Parameters.Item("pmallocatednumber").Value

        'DAK110800
        '    If IsMissing(r_sPrefix) = False Then

        If Convert.IsDBNull(m_oDatabase.Parameters.Item("range_prefix").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("range_prefix").Value) Then
            r_sPrefix = ""
        Else
            r_sPrefix = m_oDatabase.Parameters.Item("range_prefix").Value
        End If

        'DAK110800
        '    If IsMissing(r_sSuffix) = False Then

        If Convert.IsDBNull(m_oDatabase.Parameters.Item("range_suffix").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("range_suffix").Value) Then
            r_sSuffix = ""
        Else
            r_sSuffix = m_oDatabase.Parameters.Item("range_suffix").Value
        End If

        'DAK110800
        '    If IsMissing(r_sRangeCodeOut) = False Then

        If Convert.IsDBNull(m_oDatabase.Parameters.Item("new_range_code").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("new_range_code").Value) Then
            r_sRangeCodeOut = ""
        Else
            r_sRangeCodeOut = m_oDatabase.Parameters.Item("new_range_code").Value
        End If

        Return result

    End Function

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
        ' Error Section.
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