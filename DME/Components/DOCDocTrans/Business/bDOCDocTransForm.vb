Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
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
    '              a Document.
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
    ' Description: Adds a single Document directly into the database.
    '        Note: The Document will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vDocNum As Object = Nothing, Optional ByRef vFolderNum As Object = Nothing, Optional ByRef vDocName As Object = Nothing, Optional ByRef vExCode As Object = Nothing, Optional ByRef vDocType As Object = Nothing, Optional ByRef vAccessLevel As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vZipped As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing, Optional ByRef vLink As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oDocument As bDOCDocTrans.Document

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Document
            oDocument = New bDOCDocTrans.Document()

            ' Populate Document Attributes










            m_lReturn = SetProperties(oDocument, gPMConstants.PMEComponentAction.PMAdd, vDocNum:=CInt(vDocNum), vFolderNum:=CInt(vFolderNum), vDocName:=CStr(vDocName), vExCode:=CStr(vExCode), vDocType:=CStr(vDocType), vAccessLevel:=CInt(vAccessLevel), vPassword:=CStr(vPassword), vZipped:=CStr(vZipped), vCreateDate:=CDate(vCreateDate), vLink:=CInt(vLink))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Document to the Database
            m_lReturn = AddItem(oDocument)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the Document Added
            '    If (IsMissing(vDocNum) = False) Then
            '            vDocNum = oDocument.DocNum
            '    End If

            ' {* USER DEFINED CODE (End) *}

            oDocument = Nothing

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
    Private Function AddItem(ByRef oDocument As bDOCDocTrans.Document) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oDocument:=oDocument)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add DocNum as an OUTPUT param for an insert
        '    m_lreturn& = m_oDatabase.Parameters.Add( _
        ''          sName:="doc_num", _
        ''          vValue:=oDocument.DocNum, _
        ''          iDirection:=PMParamOutput, _
        ''          iDataType:=PMLong)
        '
        '    If (m_lreturn& <> PMTrue) Then
        '        AddItem = PMFalse
        '        Exit Function
        '    End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    ' Get the ID of the record inserted
        '     oDocument.DocNum = m_oDatabase.Parameters.Item("doc_num").Value
        '
        '    If (m_lreturn& <> PMTrue) Then
        '        AddItem = PMFalse
        '        Exit Function
        '    End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Document property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oDocument As bDOCDocTrans.Document, ByRef iStatus As Integer, Optional ByRef vDocNum As Integer = 0, Optional ByRef vFolderNum As Integer = 0, Optional ByRef vDocName As String = "", Optional ByRef vExCode As String = "", Optional ByRef vDocType As String = "", Optional ByRef vAccessLevel As Integer = 0, Optional ByRef vPassword As String = "", Optional ByRef vZipped As String = "", Optional ByRef vCreateDate As Date = #12/30/1899#, Optional ByRef vLink As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CheckMandatory(vDocNum:=vDocNum, vFolderNum:=vFolderNum, vDocName:=vDocName, vExCode:=vExCode, vDocType:=vDocType, vAccessLevel:=vAccessLevel, vPassword:=vPassword, vZipped:=vZipped, vCreateDate:=vCreateDate, vLink:=vLink)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Mandatory field missing.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' Default Any Missing Parameters
            m_lReturn = DefaultParameters(bDefaultAll:=False, vDocNum:=vDocNum, vFolderNum:=vFolderNum, vDocName:=vDocName, vExCode:=vExCode, vDocType:=vDocType, vAccessLevel:=vAccessLevel, vPassword:=vPassword, vZipped:=vZipped, vCreateDate:=vCreateDate, vLink:=vLink)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to set a default parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vDocNum:=vDocNum, vFolderNum:=vFolderNum, vDocName:=vDocName, vExCode:=vExCode, vDocType:=vDocType, vAccessLevel:=vAccessLevel, vPassword:=vPassword, vZipped:=vZipped, vCreateDate:=vCreateDate, vLink:=vLink)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to validate a parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oDocument


            If Not Information.IsNothing(vDocNum) Then
                If .DocNum <> vDocNum Then
                    .DocNum = vDocNum
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vFolderNum) Then
                If .FolderNum <> vFolderNum Then
                    .FolderNum = vFolderNum
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vDocName) Then
                If .DocName.Trim() <> vDocName.Trim() Then
                    .DocName = vDocName
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vExCode) Then
                If .ExCode.Trim() <> vExCode.Trim() Then
                    .ExCode = vExCode
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vDocType) Then
                If .DocType.Trim() <> vDocType.Trim() Then
                    .DocType = vDocType
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vAccessLevel) Then
                If .AccessLevel <> vAccessLevel Then
                    .AccessLevel = vAccessLevel
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vPassword) Then
                If .Password.Trim() <> vPassword.Trim() Then
                    .Password = vPassword
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vZipped) Then
                If .Zipped.Trim() <> vZipped.Trim() Then
                    .Zipped = vZipped
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vCreateDate) Then
                If .CreateDate <> vCreateDate Then
                    .CreateDate = vCreateDate
                    bDataChanged = True
                End If
            End If


            If Not Information.IsNothing(vLink) Then
                If .Link <> vLink Then
                    .Link = vLink
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
    Private Function AddInputParam(ByRef oDocument As bDOCDocTrans.Document) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase


            m_lReturn = .Parameters.Add(sName:="doc_num", vValue:=oDocument.DocNum, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="folder_num", vValue:=oDocument.FolderNum, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="doc_name", vValue:=oDocument.DocName, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="ex_code", vValue:=oDocument.ExCode, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="doc_type", vValue:=oDocument.DocType, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="access_level", vValue:=oDocument.AccessLevel, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMInteger))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="password", vValue:=oDocument.Password, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="zipped", vValue:=oDocument.Zipped, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="create_date", vValue:=oDocument.CreateDate, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="link", vValue:=oDocument.Link, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Document.
    '
    ' ***************************************************************** '
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vDocNum As Byte = 0, Optional ByRef vFolderNum As Byte = 0, Optional ByRef vDocName As String = "", Optional ByRef vExCode As String = "", Optional ByRef vDocType As String = "", Optional ByRef vAccessLevel As Byte = 0, Optional ByRef vPassword As String = "", Optional ByRef vZipped As String = "", Optional ByRef vCreateDate As Date = #12/30/1899#, Optional ByRef vLink As Byte = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Information.IsNothing(vDocNum)) Or (vDocNum.Equals(0)) Or (bDefaultAll) Then
            vDocNum = 0
        End If



        If (Information.IsNothing(vFolderNum)) Or (vFolderNum.Equals(0)) Or (bDefaultAll) Then
            vFolderNum = 0
        End If



        If (Information.IsNothing(vDocName)) Or (String.IsNullOrEmpty(vDocName)) Or (bDefaultAll) Then
            vDocName = ""
        End If



        If (Information.IsNothing(vExCode)) Or (String.IsNullOrEmpty(vExCode)) Or (bDefaultAll) Then
            vExCode = ""
        End If



        If (Information.IsNothing(vDocType)) Or (String.IsNullOrEmpty(vDocType)) Or (bDefaultAll) Then
            vDocType = ""
        End If



        If (Information.IsNothing(vAccessLevel)) Or (vAccessLevel.Equals(0)) Or (bDefaultAll) Then
            vAccessLevel = 9
        End If



        If (Information.IsNothing(vPassword)) Or (String.IsNullOrEmpty(vPassword)) Or (bDefaultAll) Then
            vPassword = ""
        End If



        If (Information.IsNothing(vZipped)) Or (String.IsNullOrEmpty(vZipped)) Or (bDefaultAll) Then
            vZipped = "N"
        End If



        If (Information.IsNothing(vCreateDate)) Or (vCreateDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vCreateDate = DateTime.Now
        End If



        If (Information.IsNothing(vLink)) Or (vLink.Equals(0)) Or (bDefaultAll) Then
            vLink = 0
        End If


        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function


    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Document.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vDocNum As Object = Nothing, Optional ByRef vFolderNum As Object = Nothing, Optional ByRef vDocName As Object = Nothing, Optional ByRef vExCode As Object = Nothing, Optional ByRef vDocType As Object = Nothing, Optional ByRef vAccessLevel As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vZipped As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing, Optional ByRef vLink As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Information.IsNothing(vDocNum)) Or (Object.Equals(vDocNum, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vFolderNum)) Or (Object.Equals(vFolderNum, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Information.IsNothing(vDocName)) Or (Object.Equals(vDocName, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        '    If (IsMissing(vExCode) = True) _
        ''    Or (IsEmpty(vExCode) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If



        If (Information.IsNothing(vDocType)) Or (Object.Equals(vDocType, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        '    If (IsMissing(vAccessLevel) = True) _
        ''    Or (IsEmpty(vAccessLevel) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If

        '    If (IsMissing(vPassword) = True) _
        ''    Or (IsEmpty(vPassword) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If

        '    If (IsMissing(vZipped) = True) _
        ''    Or (IsEmpty(vZipped) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If

        '    If (IsMissing(vCreateDate) = True) _
        ''    Or (IsEmpty(vCreateDate) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If

        '    If (IsMissing(vLink) = True) _
        ''    Or (IsEmpty(vLink) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Document for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vDocNum As Object = Nothing, Optional ByRef vFolderNum As Object = Nothing, Optional ByRef vDocName As Object = Nothing, Optional ByRef vExCode As Object = Nothing, Optional ByRef vDocType As Object = Nothing, Optional ByRef vAccessLevel As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vZipped As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing, Optional ByRef vLink As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vDocNum), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vFolderNum), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vAccessLevel), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsDate(vCreateDate) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vLink), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
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
        'LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class