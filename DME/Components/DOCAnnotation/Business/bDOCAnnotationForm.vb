Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 16/01/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Annotation.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 02/12/2003
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
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

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

#If PD_EARLYBOUND = 1 Then

				Set m_oDatabase = New dPMDAO.Database
#Else
                m_oDatabase = New dPMDAO.Database()
#End If

                ' Open the Database
                m_lReturn = gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=m_oDatabase)


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
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single Annotation directly into the database.
    '        Note: The Annotation will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vAnnotationID As Integer = 0, Optional ByRef vDocNum As Object = Nothing, Optional ByRef vAnnText As Object = Nothing, Optional ByRef vUsername As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oAnnotation As bDOCAnnotation.Annotation

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Annotation
            oAnnotation = New bDOCAnnotation.Annotation()

            ' Populate Annotation Attributes




            m_lReturn = SetProperties(oAnnotation, gPMConstants.PMEComponentAction.PMAdd, vAnnotationID:=vAnnotationID, vDocNum:=CInt(vDocNum), vAnnText:=CStr(vAnnText), vUsername:=CStr(vUsername), vCreateDate:=CDate(vCreateDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Annotation to the Database
            m_lReturn = AddItem(oAnnotation)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the Annotation Added

            If Not Informations.IsNothing(vAnnotationID) Then
                vAnnotationID = oAnnotation.AnnotationID
            End If

            ' {* USER DEFINED CODE (End) *}

            oAnnotation = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function AddItem(ByRef oAnnotation As bDOCAnnotation.Annotation) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oAnnotation:=oAnnotation)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add AnnotationID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Annotation_id", vValue:=oAnnotation.AnnotationID, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamOutput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oAnnotation.AnnotationID = m_oDatabase.Parameters.Item("Annotation_id").Value

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied Annotation property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oAnnotation As bDOCAnnotation.Annotation, ByRef iStatus As Integer, Optional ByRef vAnnotationID As Integer = 0, Optional ByRef vDocNum As Integer = 0, Optional ByRef vAnnText As String = "", Optional ByRef vUsername As String = "", Optional ByRef vCreateDate As Date = #12/30/1899#) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CheckMandatory(vAnnotationID:=vAnnotationID, vDocNum:=vDocNum, vAnnText:=vAnnText, vUsername:=vUsername, vCreateDate:=vCreateDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Mandatory field missing.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            ' Default Any Missing Parameters
            m_lReturn = DefaultParameters(bDefaultAll:=False, vAnnotationID:=vAnnotationID, vDocNum:=vDocNum, vAnnText:=vAnnText, vUsername:=vUsername, vCreateDate:=vCreateDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to set a default parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vAnnotationID:=vAnnotationID, vDocNum:=vDocNum, vAnnText:=vAnnText, vUsername:=vUsername, vCreateDate:=vCreateDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to validate a parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oAnnotation


            If Not Informations.IsNothing(vAnnotationID) Then
                If .AnnotationID <> vAnnotationID Then
                    .AnnotationID = vAnnotationID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDocNum) Then
                If .DocNum <> vDocNum Then
                    .DocNum = vDocNum
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vAnnText) Then
                If .AnnText.Trim() <> vAnnText.Trim() Then
                    .AnnText = vAnnText
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vUsername) Then
                If .UserName.Trim() <> vUsername.Trim() Then
                    .UserName = vUsername
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCreateDate) Then
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
    Private Function AddInputParam(ByRef oAnnotation As bDOCAnnotation.Annotation) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="doc_num", vValue:=oAnnotation.DocNum, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMLong))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="ann_text", vValue:=oAnnotation.AnnText, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="user_name", vValue:=oAnnotation.UserName, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMString))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="create_date", vValue:=oAnnotation.CreateDate, iDirection:=CShort(gPMConstants.PMEParameterDirection.PMParamInput), iDataType:=CShort(gPMConstants.PMEDataType.PMDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Annotation.
    '
    ' ***************************************************************** '
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vAnnotationID As Byte = 0, Optional ByRef vDocNum As Integer = 0, Optional ByRef vAnnText As String = "", Optional ByRef vUsername As String = "", Optional ByRef vCreateDate As Date = #12/30/1899#) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vAnnotationID)) Or (vAnnotationID.Equals(0)) Or (bDefaultAll) Then
            vAnnotationID = 0
        End If



        If (Informations.IsNothing(vDocNum)) Or (vDocNum.Equals(0)) Or (bDefaultAll) Then
            vDocNum = 0
        End If



        If (Informations.IsNothing(vAnnText)) Or (String.IsNullOrEmpty(vAnnText)) Or (bDefaultAll) Then
            vAnnText = ""
        End If



        If (Informations.IsNothing(vUsername)) Or (String.IsNullOrEmpty(vUsername)) Or (bDefaultAll) Then
            vUsername = ""
        End If



        If (Informations.IsNothing(vCreateDate)) Or (vCreateDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vCreateDate = DateTime.Now
        End If


        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Annotation.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vAnnotationID As Object = Nothing, Optional ByRef vDocNum As Object = Nothing, Optional ByRef vAnnText As Object = Nothing, Optional ByRef vUsername As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}

        '    If (IsMissing(vAnnotationID) = True) _
        ''    Or (IsEmpty(vAnnotationID) = True) Then
        '        CheckMandatory = PMMandatoryMissing
        '        Exit Function
        '    End If



        If (Informations.IsNothing(vDocNum)) Or (Object.Equals(vDocNum, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vAnnText)) Or (Object.Equals(vAnnText, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vUsername)) Or (Object.Equals(vUsername, Nothing)) Then
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
    ' Description: Checks the Annotation for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vAnnotationID As Object = Nothing, Optional ByRef vDocNum As Object = Nothing, Optional ByRef vAnnText As Object = Nothing, Optional ByRef vUsername As Object = Nothing, Optional ByRef vCreateDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vAnnotationID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vDocNum), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsDate(vCreateDate) Then
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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
        'bPMFunc.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class