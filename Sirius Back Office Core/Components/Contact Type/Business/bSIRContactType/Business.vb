Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    Private Const ACClass As String = "Business"

    Private m_iDatabaseStatus As Integer
    ' Reference to dPMDAO
    Private m_oDatabase As dPMDAO.Database
    Private m_oArcDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean
    Private m_bCloseArcDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lReturn As Integer

    ' ************************************************
    ' Added to replace global variables 18/12/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    Public Property DatabaseStatus() As Integer
        Get
            Return m_iDatabaseStatus
        End Get
        Set(ByVal Value As Integer)
            m_iDatabaseStatus = Value
        End Set
    End Property
    Public ReadOnly Property PMProductFamily() As gPMConstants.PMEProductFamily
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise





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


            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            'DC 11/09/00 changed from Now to Date
            m_dtEffectiveDate = DateTime.Today

            ' Get an instance of component services


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get connection to broking database.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=m_bCloseArcDatabase, r_oCheckedDatabase:=m_oArcDatabase, v_vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get connection to architecture database.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
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
                If m_bCloseArcDatabase Then
                    m_oArcDatabase.CloseDatabase()

                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetDetails
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetDetails(ByRef r_vDetailArray As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT ct.contact_type_id, " & _
                   "ct.caption_id, ct.code, ct.description, " & _
                   "ct.is_deleted, ct.effective_date, " & _
                   "ISNULL(ct.is_contact_type, 0), " & _
                   "ISNULL(ct.is_correspondence_type, 0) " & _
                   "FROM contact_type ct " & _
                   "ORDER BY ct.code"

            ' Execute the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDetails", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' No values, so return not found
            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Return the array


            r_vDetailArray = vResultArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateDetails
    '
    ' Description: Updates the database with the passed array
    '
    ' History: 24/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateDetails(ByVal v_vArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop the array
            For iLoop1 As Integer = 0 To v_vArray.GetUpperBound(1)


                If CDbl(v_vArray(ACArrayIsContactType, iLoop1)) = 1 Then

                    ' Do we need to do an update, or an add ?

                    If CDbl(v_vArray(ACArrayCaptionID, iLoop1)) = 0 Then
                        ' Add
                        m_lReturn = AddRecord(r_vArray:=v_vArray, v_iIndex:=iLoop1)
                    Else
                        ' Update
                        m_lReturn = UpdateRecord(r_vArray:=v_vArray, v_iIndex:=iLoop1)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddRecord
    '
    ' Description:
    '
    ' History: 25/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function AddRecord(ByRef r_vArray(,) As Object, ByVal v_iIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim vContactTypeID As Byte
        Dim vCaptionID As Object = Nothing
        Dim vCode As Byte
        Dim vDescription As Byte
        Dim vIsDeleted As Byte
        Dim vIsContactType As Byte
        Dim vIsCorrespondenceType As Byte



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the values out of the array

        vContactTypeID = CByte(r_vArray(ACArrayContactTypeID, v_iIndex))

        vCode = CByte(r_vArray(ACArrayCode, v_iIndex))

        vDescription = CByte(r_vArray(ACArrayDescription, v_iIndex))
        vIsDeleted = 0

        r_vArray(ACArrayIsDeleted, v_iIndex) = 0
        vIsContactType = 1

        r_vArray(ACArrayIsContactType, v_iIndex) = 1
        vIsCorrespondenceType = 0

        r_vArray(ACArrayIsCorrespondenceType, v_iIndex) = 0

        ' Get the caption
        m_lReturn = GetCaptionID(v_vDescription:=vDescription, r_vCaptionID:=vCaptionID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Remove the parameters
        m_oDatabase.Parameters.Clear()

        ' Add new ones
        m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_type_id", vValue:=CStr(vContactTypeID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(vCaptionID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(vCode), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(vDescription), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", vValue:=CStr(vIsDeleted), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Today.ToString("MM-dd-yyyy"), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_contact_type", vValue:=CStr(vIsContactType), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_correspondence_type", vValue:=CStr(vIsCorrespondenceType), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute the SQL
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add new record : " & ACAddSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="AddRecord", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateRecord
    '
    ' Description:
    '
    ' History: 25/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateRecord(ByRef r_vArray(,) As Object, ByVal v_iIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim vContactTypeID As Byte
        Dim vCaptionID As Object = Nothing
        Dim vCode As Byte
        Dim vDescription As Byte
        Dim vIsDeleted As Byte
        Dim vIsContactType As Byte
        Dim vIsCorrespondenceType As Byte



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the values out of the array

        vContactTypeID = CByte(r_vArray(ACArrayContactTypeID, v_iIndex))

        vCode = CByte(r_vArray(ACArrayCode, v_iIndex))

        vDescription = CByte(r_vArray(ACArrayDescription, v_iIndex))

        vIsDeleted = CByte(r_vArray(ACArrayIsDeleted, v_iIndex))
        vIsContactType = 1

        r_vArray(ACArrayIsContactType, v_iIndex) = 1
        vIsCorrespondenceType = 0

        r_vArray(ACArrayIsCorrespondenceType, v_iIndex) = 0

        ' Get the caption
        m_lReturn = GetCaptionID(v_vDescription:=vDescription, r_vCaptionID:=vCaptionID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Clear the parameters
        m_oDatabase.Parameters.Clear()

        ' Add new ones
        m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_type_id", vValue:=CStr(vContactTypeID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(vCaptionID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(vCode), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(vDescription), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", vValue:=CStr(vIsDeleted), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DC 11/09/00 changed from Now to Date
        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(DateTime.Today), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_contact_type", vValue:=CStr(vIsContactType), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_correspondence_type", vValue:=CStr(vIsCorrespondenceType), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute the SQL
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update record : " & ACAddSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="AddRecord", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCaptionID
    '
    ' Description:
    '
    ' History: 25/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetCaptionID(ByVal v_vDescription As Object, ByRef r_vCaptionID As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oArcDatabase.Parameters.Clear()

            ' Add the new ones
            m_lReturn = m_oArcDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oArcDatabase.Parameters.Add(sName:="caption", vValue:=CStr(v_vDescription), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oArcDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(r_vCaptionID), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the SQL
            m_lReturn = m_oArcDatabase.SQLAction(sSQL:=ACGetCaptionSQL, sSQLName:=ACGetCaptionName, bStoredProcedure:=ACGetCaptionStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on sql = " & ACGetCaptionSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptionID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Get the caption_id

            r_vCaptionID = m_oArcDatabase.Parameters.Item("caption_id").Value

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptionID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptionID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

