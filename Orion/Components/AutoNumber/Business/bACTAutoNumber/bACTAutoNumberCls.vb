Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ************************************************
    ' Added to replace global variables 09/12/2003
    Private m_sUsername As String = ""
    'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
    Private m_bUniqueDocumentReferenceEnabled As Boolean
    'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
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
    Private m_oPMLookup As BPMLOOKUP.Business

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lError As Integer

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Component Sub Type
    Private m_sSubType As New StringsHelper.FixedLengthString(20)


    ' PRIVATE Data Members (End)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            'SD 23/07/2002 variable name correction

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 070300 - m_bCloseDatabase is set above, no need to do it here
            ' Close Database in Terminate() method
            'm_bCloseDatabase = True

            m_oPMLookup = New BPMLOOKUP.Business()

            m_lReturn = m_oPMLookup.Initialise(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(m_oPMLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion, gPMConstants.PMEReturnCode)

            'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            Dim r_vValue As String = ""

            ' Get the product option "Enable Unique Document Reference
            m_lReturn = CType(bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, 0, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, SIRHiddenOptions.SIROPTEnableUniqueDocumentReference, gPMConstants.SIRBCHHeadOffice, r_vValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Initilize", "Failed to read product option Enable Unique Document Reference", gPMConstants.PMELogLevel.PMLogError)

                m_bUniqueDocumentReferenceEnabled = False
            End If

            m_bUniqueDocumentReferenceEnabled = r_vValue = "1"
            'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
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
    Public Function GetNumberRange(ByVal v_sGroupCode As String, ByVal v_sRangeCode As String, ByRef r_lNumberRangeID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


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
            'developer guide no. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="EffectiveDate", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
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

            ' Get the returned value

            If Convert.IsDBNull(m_oDatabase.Parameters.Item("NumberRangeID").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("NumberRangeID").Value) Then
                r_lNumberRangeID = 1
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
    'eck170500 Add Company ID

    Public Function GenerateNumber(ByVal v_lNumberRangeID As Integer, ByVal v_iUserID As Integer, ByVal v_iCompanyID As Integer, ByRef r_lNumber As Integer) As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            ' Set the return value to true
            result = gPMConstants.PMEReturnCode.PMTrue
            'Check for a pool number
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the v_lNumberRangeID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="actnumber_range_id", vValue:=CStr(v_lNumberRangeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'eck180500
            ' Add the v_CompanyID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_iCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectPoolNumberSQL, sSQLName:=ACSelectPoolNumberName, bStoredProcedure:=ACSelectPoolNumberStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then

                r_lNumber = CInt(vArray(0, 0))
                If r_lNumber = 0 Then GoTo GenerateNewNumber
                'eck170500 Add company parameter
                m_lReturn = CType(AllocatePoolNumber(v_lNumberRangeID:=v_lNumberRangeID, v_iUserID:=v_iUserID, v_iCompanyID:=v_iCompanyID, r_lNumber:=r_lNumber), gPMConstants.PMEReturnCode)
            Else
GenerateNewNumber:
                'eck170500 Add company parameter
                m_lReturn = CType(AllocateNewNumber(v_lNumberRangeID:=v_lNumberRangeID, v_iUserID:=v_iUserID, v_iCompanyID:=v_iCompanyID, r_lNumber:=r_lNumber), gPMConstants.PMEReturnCode)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch ex As Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateNumber", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: PoolNumber (Standard Method)
    '
    ' Description: Get the db to generate a unique counter
    '
    ' ***************************************************************** '
    'eck170500 Add Company Id
    Public Function PoolNumber(ByVal v_lNumberRangeID As Integer, ByVal v_iUserID As Integer, ByVal v_iCompanyID As Integer, ByRef r_lNumber As Integer) As Integer



        Dim result As Integer = 0
        Try

            ' Set the return value to true
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="actnumber_pool_id", vValue:=CStr(r_lNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the v_lNumberRangeID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="actnumber_range_id", vValue:=CStr(v_lNumberRangeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the UserID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'eck170500 Add company parameter
            'eck220601 remove ridiculous coding
            '        m_lReturn = AllocateNewNumber( _
            ''            v_lNumberRangeID:=v_lNumberRangeID, _
            ''            v_iUserID:=v_iUserID%, _
            ''            v_iCompanyID:=v_iCompanyID, _
            ''            r_lNumber:=r_lNumber&)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_iCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPoolNumberSQL, sSQLName:=ACAddPoolNumberName, bStoredProcedure:=ACAddPoolNumberStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PoolNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PoolNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'eck170500 Add Company ID
    Private Function AllocatePoolNumber(ByVal v_lNumberRangeID As Integer, ByVal v_iUserID As Integer, ByVal v_iCompanyID As Integer, ByRef r_lNumber As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the PoolNumberID parameter (INPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="actnumber_pool_id", vValue:=CStr(r_lNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        ' Add the NumberRangeID parameter (INPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="actnumber_range_id", vValue:=CStr(v_lNumberRangeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        'eck170500
        ' Add the CompanyID parameter (INPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_iCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Delete the Pool record
        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeletePoolNumberSQL, sSQLName:=ACDeletePoolNumberName, bStoredProcedure:=ACDeletePoolNumberStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Update the Main Number details

        m_oDatabase.Parameters.Clear()

        ' Add the Numberparameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="actnumber_id", vValue:=CStr(r_lNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="actnumber_range_id", vValue:=CStr(v_lNumberRangeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        'eck170500
        ' Add the CompanyID parameter (INPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_iCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateNumberSQL, sSQLName:=ACUpdateNumberName, bStoredProcedure:=ACUpdateNumberStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return result



        ' Error Section.

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllocatepoolNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AllocatePoolNumber", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result
    End Function
    'eck170500 Add Company ID
    Private Function AllocateNewNumber(ByVal v_lNumberRangeID As Integer, ByVal v_iUserID As Integer, ByVal v_iCompanyID As Integer, ByRef r_lNumber As Integer) As Integer
        ' Clear the Database Parameters Collection

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' CTAF 201200 - Made the OUTPUT parameter the first parameter.

        ' Add the Counter parameter (OUTPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="actnumber_id", vValue:=CStr(r_lNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the v_lNumberRangeID parameter (INPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="actnumber_range_id", vValue:=CStr(v_lNumberRangeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the UserID parameter (INPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'eck170500
        ' Add the CompanyID parameter (INPUT)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_iCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAllocateNumberSQL, sSQLName:=ACAllocateNumberName, bStoredProcedure:=ACAllocateNumberStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' return the calculated counter value
        r_lNumber = m_oDatabase.Parameters.Item("actnumber_id").Value

        Return result
    End Function


    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

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




    ' ***************************************************************** '
    ' Name: EncodeLong (Private)
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
            sPaddedValue = String.Format(lRemainder, "000000000")

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
    'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
    Public Function GenerateDocumentReferenceNumber(ByVal v_sRangeCode As String, ByVal v_iUserID As Integer, ByVal v_iCompanyID As Integer, ByRef r_sDocumentRef As String) As Integer
        Return GenerateDocumentReferenceNumber(v_sRangeCode:=v_sRangeCode, v_iUserID:=v_iUserID, v_iCompanyID:=v_iCompanyID, r_sDocumentRef:=r_sDocumentRef, v_lNumberRangeID:=0)
    End Function

    Public Function GenerateDocumentReferenceNumber(ByVal v_sRangeCode As String, ByVal v_iUserID As Integer, ByVal v_iCompanyID As Integer, ByRef r_sDocumentRef As String, ByVal v_lNumberRangeID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GenerateDocumentReferenceNumber"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'If number range id is not provided, get it from Range code.
            If v_lNumberRangeID = 0 Then
                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                ' Add the output parameter- NumberRangeID
                m_lReturn = m_oDatabase.Parameters.Add(sName:="NumberRangeID", vValue:=CStr(v_lNumberRangeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to add parameter(NumberRangeID) for stored procedure" & ACGetNumberRangeFromCodeSQL, gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Add the parameter - range code
                m_lReturn = m_oDatabase.Parameters.Add(sName:="RangeCode", vValue:=v_sRangeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to add parameter(RangeCode) for stored procedure" & ACGetNumberRangeFromCodeSQL, gPMConstants.PMELogLevel.PMLogError)
                End If

                ' execute the Stored Procedure
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetNumberRangeFromCodeSQL, sSQLName:=ACGetNumberRangeFromCodeName, bStoredProcedure:=ACGetNumberRangeFromCodeStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to execute the stored procedure: " & ACGetNumberRangeFromCodeSQL, gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Get the returned value

                If Convert.IsDBNull(m_oDatabase.Parameters.Item("NumberRangeID").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("NumberRangeID").Value) Then
                    v_lNumberRangeID = 1
                Else
                    v_lNumberRangeID = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("NumberRangeID").Value)
                End If

            End If

            Dim lNumber As Integer
            If m_bUniqueDocumentReferenceEnabled Then

                ' Unique Document Reference product option is enabled.
                ' Call Generate unique document reference to get the next number
                m_lReturn = CType(GenerateUniqueDocumentReferenceNumber(v_lNumberRangeID:=v_lNumberRangeID, v_iUserID:=v_iUserID, v_iCompanyID:=v_iCompanyID, r_sDocumentRef:=r_sDocumentRef), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to excecute GenerateUniqueDocumentReference", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                ' Unique Document Reference product option is enabled.
                ' Call Generate number to get the next number
                m_lReturn = CType(GenerateNumber(v_lNumberRangeID:=v_lNumberRangeID, v_iUserID:=v_iUserID, v_iCompanyID:=v_iCompanyID, r_lNumber:=lNumber), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to excecute GenerateNumber", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Format the number
                r_sDocumentRef = StringsHelper.Format(lNumber, "0000000000")

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    Private Function GenerateUniqueDocumentReferenceNumber(ByVal v_lNumberRangeID As Integer, ByVal v_iUserID As Integer, ByVal v_iCompanyID As Integer, ByRef r_sDocumentRef As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GenerateUniqueDocumentReferenceNumber"




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' The sp has 4 parameters:
        ' number_range_id - Mapped to v_lNumberRangeID
        ' user_id - Mapped to v_iUserID
        ' company_id - Mapped to v_iCompanyID
        ' document_reference - Output parameter. Mapped to r_sDocumentRef

        ' Add the output parameter- document_reference
        m_lReturn = m_oDatabase.Parameters.Add(sName:="document_reference", vValue:=r_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to add parameter(document_reference) for stored procedure" & ACAllocateUniqueNumberSQL, gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Add the parameter - number_range_id if available
        m_lReturn = m_oDatabase.Parameters.Add(sName:="number_range_id", vValue:=CStr(v_lNumberRangeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to add parameter(document_type_code) for stored procedure" & ACAllocateUniqueNumberSQL, gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Add the parameter - user_id
        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to add parameter(user_id) for stored procedure" & ACAllocateUniqueNumberSQL, gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Add the parameter - company_id
        m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_iCompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to add parameter(company_id) for stored procedure" & ACAllocateUniqueNumberSQL, gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAllocateUniqueNumberSQL, sSQLName:=ACAllocateUniqueNumberName, bStoredProcedure:=ACAllocateUniqueNumberStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to execute the stored procedure: " & ACAllocateUniqueNumberSQL, gPMConstants.PMELogLevel.PMLogError)
        End If

        ' If returned value is null return false.

        If Convert.IsDBNull(m_oDatabase.Parameters.Item("document_reference").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("document_reference").Value) Then
            gPMFunctions.RaiseError(kMethodName, "Failed to get document reference from the stored procedure: " & ACAllocateUniqueNumberSQL, gPMConstants.PMELogLevel.PMLogError)
        Else
            r_sDocumentRef = m_oDatabase.Parameters.Item("document_reference").Value
        End If


        Return result

    End Function

    'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
End Class
