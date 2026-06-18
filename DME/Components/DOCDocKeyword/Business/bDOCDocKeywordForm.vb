Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Globalization
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 16/01/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Keyword.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
#If PD_EARLYBOUND = 1 Then

	Private m_oDatabase As dPMDAO.Database
#Else
    Private m_oDatabase As dPMDAO.Database
#End If

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' ***************************************************************** '
    ' Standard Product Family Constant (Read Only)
    ' ***************************************************************** '

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            '
            Return gPMConstants.PMEProductFamily.pmePFDocumaster

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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            ' Set Username and Password
            g_sUsername = sUsername
            g_sPassword.Value = sPassword

            ' Set UserID
            g_iUserID = iUserID

            ' Set Calling Application
            g_sCallingAppName = sCallingAppName

            ' Set Language ID
            g_iLanguageID = iLanguageID

            ' Set Source ID
            g_iSourceID = iSourceID

            ' Set Currency ID
            g_iCurrencyID = iCurrencyID

            ' Set Log Level
            g_iLogLevel = iLogLevel

            ' Have we a valid Database Object Reference?

            If (Not Information.IsNothing(vDatabase)) And (Information.IsReference(vDatabase)) Then
                ' Yes, so use it.
                m_oDatabase = vDatabase

                ' Do NOT Close Database in Terminate() method
                m_bCloseDatabase = False
            Else
                ' NO, Create new instance of the database object
#If PD_EARLYBOUND = 1 Then

				Set m_oDatabase = New dPMDAO.Database
#Else
                m_oDatabase = New dPMDAO.Database()
#End If

                ' Open the Database
                m_lReturn = NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close Database in Terminate() method
                m_bCloseDatabase = True
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single DocKeyword directly into the database.
    '        Note: The DocKeyword will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vDocKeywordID As Integer = 0, Optional ByRef vKeywordID As Object = Nothing, Optional ByRef vDocNum As Object = Nothing, Optional ByRef vUsername As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oDocKeyword As bDOCDocKeyword.DocKeyword

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new DocKeyword
            oDocKeyword = New bDOCDocKeyword.DocKeyword()

            ' Populate DocKeyword Attributes




            m_lReturn = SetProperties(oDocKeyword, gPMConstants.PMEComponentAction.PMAdd, vDocKeywordID:=vDocKeywordID, vKeywordID:=CInt(vKeywordID), vDocNum:=CInt(vDocNum), vUsername:=CStr(vUsername), vCreateDate:=CDate(vCreateDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the DocKeyword to the Database
            m_lReturn = AddItem(oDocKeyword)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the DocKeyword Added

            If Not Information.IsNothing(vDocKeywordID) Then
                vDocKeywordID = oDocKeyword.DocKeywordID
            End If

            ' {* USER DEFINED CODE (End) *}

            oDocKeyword = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)
    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oDocKeyword As bDOCDocKeyword.DocKeyword) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oDocKeyword:=oDocKeyword)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add DocKeywordID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Doc_Keyword_id", vValue:=oDocKeyword.DocKeywordID, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamOutput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oDocKeyword.DocKeywordID = m_oDatabase.Parameters.Item("Doc_Keyword_id").Value

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied DocKeyword property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oDocKeyword As bDOCDocKeyword.DocKeyword, ByRef iStatus As Integer, Optional ByRef vDocKeywordID As Integer = 0, Optional ByRef vKeywordID As Integer = 0, Optional ByRef vDocNum As Integer = 0, Optional ByRef vUsername As String = "", Optional ByRef vCreateDate As Date = #12/30/1899#) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CheckMandatory(vDocKeywordID:=vDocKeywordID, vKeywordID:=vKeywordID, vDocNum:=vDocNum, vUsername:=vUsername, vCreateDate:=vCreateDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Mandatory field missing.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Default Any Missing Parameters
            m_lReturn = DefaultParameters(bDefaultAll:=False, vDocKeywordID:=vDocKeywordID, vKeywordID:=vKeywordID, vDocNum:=vDocNum, vUsername:=vUsername, vCreateDate:=vCreateDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Mandatory field missing.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vDocKeywordID:=vDocKeywordID, vKeywordID:=vKeywordID, vDocNum:=vDocNum, vUsername:=vUsername, vCreateDate:=vCreateDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Mandatory field missing.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oDocKeyword


            If Not Information.IsNothing(vDocKeywordID) Then
                If .DocKeywordID <> vDocKeywordID Then
                    .DocKeywordID = vDocKeywordID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vKeywordID) Then
                If .KeywordID <> vKeywordID Then
                    .KeywordID = vKeywordID
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vDocNum) Then
                If .DocNum <> vDocNum Then
                    .DocNum = vDocNum
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vUsername) Then
                If .UserName.Trim() <> vUsername.Trim() Then
                    .UserName = vUsername
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vCreateDate) Then
                If .CreateDate <> vCreateDate Then
                    .CreateDate = vCreateDate
                    bDataChanged = True
                End If
            End If


            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                .DatabaseStatus = iStatus
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oDocKeyword As bDOCDocKeyword.DocKeyword) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="keyword_id", vValue:=oDocKeyword.KeywordID, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="doc_num", vValue:=oDocKeyword.DocNum, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .Parameters.Add(sName:="user_name", vValue:=oDocKeyword.UserName, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="create_date", vValue:=oDocKeyword.CreateDate, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a DocKeyword.
    '
    ' ***************************************************************** '
    'as developer's guide no 1010
    'Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vDocKeywordID As Byte = 0, Optional ByRef vKeywordID As Object = Nothing, Optional ByRef vDocNum As Object = Nothing, Optional ByRef vUsername As Object = Nothing, Optional ByRef vCreateDate As Date = #12/30/1899#) As Integer
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vDocKeywordID As Object = Nothing, Optional ByRef vKeywordID As Object = Nothing, Optional ByRef vDocNum As Object = Nothing, Optional ByRef vUsername As Object = Nothing, Optional ByRef vCreateDate As Date = #12/30/1899#) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Information.IsNothing(vDocKeywordID)) Or (vDocKeywordID.Equals(0)) Or (bDefaultAll) Then
            vDocKeywordID = 0
        End If



        If (Information.IsNothing(vCreateDate)) Or (vCreateDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vCreateDate = DateTime.Now
        End If


        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function


    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a DocKeyword.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vDocKeywordID As Object = Nothing, Optional ByRef vKeywordID As Object = Nothing, Optional ByRef vDocNum As Object = Nothing, Optional ByRef vUsername As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}

        '    If (IsMissing(vDocKeywordID) = True) _
        ''    Or (IsEmpty(vDocKeywordID) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If



        If (Information.IsNothing(vKeywordID)) Or (Object.Equals(vKeywordID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vDocNum)) Or (Object.Equals(vDocNum, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vUsername)) Or (Object.Equals(vUsername, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        '    If (IsMissing(vCreateDate) = True) _
        ''    Or (IsEmpty(vCreateDate) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the DocKeyword for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vDocKeywordID As Object = Nothing, Optional ByRef vKeywordID As Object = Nothing, Optional ByRef vDocNum As Object = Nothing, Optional ByRef vUsername As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vDocKeywordID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vKeywordID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vDocNum), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsDate(vCreateDate) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
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
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
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
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
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
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
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
        ' Error.
        '
        ' Log Error Message
        'iPMFunc.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class