Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 05/05/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a PMU Lookup Header.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 19/09/2003
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

    ' Database Class (Private)
    Private m_oArcDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Close Database Flag (Private)
    Private m_bCloseArcDatabase As Boolean

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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get an instance to the architecture database
            m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oArcDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bCloseArcDatabase = True


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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_bCloseArcDatabase Then

                    m_oArcDatabase.CloseDatabase()

                    m_oArcDatabase = Nothing

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


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails
    '
    ' Description: Get details from DB.
    '
    ' ***************************************************************** '
    Public Function GetDetails(ByRef lLookupHeaderId As Integer, ByRef vLookupDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get from DB
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_user_def_header_id", vValue:=CStr(lLookupHeaderId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLookupDetailsSQL, sSQLName:=ACGetLookupDetailsName, bStoredProcedure:=ACGetLookupDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vLookupDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    '
    ' ***************************************************************** '

    Public Function Update(ByRef vLookupDetails(,) As Object, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim lCaption As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vLookupDetails) Then
                Return result
            End If

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'First loop around and add in the new guys
            For lTemp As Integer = vLookupDetails.GetLowerBound(1) To vLookupDetails.GetUpperBound(1)


                If CDbl(vLookupDetails(ACHLookupDetailId, lTemp)) = -1 Then
                    m_oDatabase.Parameters.Clear()


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_user_def_detail_id", vValue:=CStr(vLookupDetails(ACHLookupDetailId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_user_def_header_id", vValue:=CStr(vLookupDetails(ACHLookupHeaderId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Always get the new caption - the description may have changed...


                    'developer guide no. 98
                    Dim screenHierarchy As String = ""

                    If v_sScreenHierarchy <> "" Then
                        screenHierarchy = v_sScreenHierarchy & "/" & $"LookUp Detail({(CStr(vLookupDetails(ACHCode, lTemp))).Trim()})"
                    End If

                    m_lReturn = CType(GetCaptionID(v_sCaption:=vLookupDetails(ACHDescription, lTemp), r_lCaptionID:=lCaption), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    vLookupDetails(ACHCaptionId, lTemp) = lCaption


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(vLookupDetails(ACHCaptionId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(vLookupDetails(ACHCode, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(vLookupDetails(ACHDescription, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", vValue:=CStr(vLookupDetails(ACHIsDeleted, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=CStr(vLookupDetails(ACHEffectiveDate, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    If CStr(vLookupDetails(ACHParentId, lTemp)) = "" Then

                        'developer guide no. 85

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="parent", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    Else

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="parent", vValue:=CStr(vLookupDetails(ACHParentId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    If CStr(vLookupDetails(ACHLookupHeaderIndsId, lTemp)) = "" Then

                        'developer guide no. 85

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_user_def_header_inds_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    Else

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_user_def_header_inds_id", vValue:=CStr(vLookupDetails(ACHLookupHeaderIndsId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=screenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertLookupDetailsSQL, sSQLName:=ACInsertLookupDetailsName, bStoredProcedure:=ACInsertLookupDetailsStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    vLookupDetails(ACHLookupDetailId, lTemp) = m_oDatabase.Parameters.Item("GIS_user_def_detail_id").Value

                End If
            Next lTemp

            'Now loop around and update everything
            For lTemp As Integer = vLookupDetails.GetLowerBound(1) To vLookupDetails.GetUpperBound(1)

                m_oDatabase.Parameters.Clear()
                Dim screenHierarchy As String = ""

                If v_sScreenHierarchy <> "" Then
                    screenHierarchy = v_sScreenHierarchy & "/" & $"LookUp Detail({(CStr(vLookupDetails(ACHCode, lTemp))).Trim()})"
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_user_def_detail_id", vValue:=CStr(vLookupDetails(ACHLookupDetailId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_user_def_header_id", vValue:=CStr(vLookupDetails(ACHLookupHeaderId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Tomo030401
                'Forgot to do it in the update as well as the add...

                'Always get the new caption - the description may have changed...


                'developer guide no. 98

                m_lReturn = CType(GetCaptionID(v_sCaption:=vLookupDetails(ACHDescription, lTemp), r_lCaptionID:=lCaption), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                vLookupDetails(ACHCaptionId, lTemp) = lCaption

                'Tomo030401
                'End


                m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(vLookupDetails(ACHCaptionId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(vLookupDetails(ACHCode, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(vLookupDetails(ACHDescription, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", vValue:=CStr(vLookupDetails(ACHIsDeleted, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'RWH(Added CDate to prevent dates reversing.)

                'developer guide no. 40

                m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=CDate(vLookupDetails(ACHEffectiveDate, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If CStr(vLookupDetails(ACHParentId, lTemp)) = "" Then

                    'developer guide no. 85

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="parent", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="parent", vValue:=CStr(vLookupDetails(ACHParentId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If


                If (CStr(vLookupDetails(ACHLookupHeaderIndsId, lTemp)) = "") Or (CStr(vLookupDetails(ACHLookupHeaderIndsId, lTemp)) = "0") Then

                    'developer guide no. 85

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_user_def_header_inds_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_user_def_header_inds_id", vValue:=CStr(vLookupDetails(ACHLookupHeaderIndsId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=screenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateLookupDetailsSQL, sSQLName:=ACUpdateLookupDetailsName, bStoredProcedure:=ACUpdateLookupDetailsStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lTemp

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetHeaderInds
    '
    ' Description:
    '
    ' History: 24/05/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetHeaderInds(ByRef lLookupHeaderId As Integer, ByRef vHeaderInds(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get from DB
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_user_def_header_id", vValue:=CStr(lLookupHeaderId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetHeaderIndSQL, sSQLName:=ACGetHeaderIndName, bStoredProcedure:=ACGetHeaderIndStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vHeaderInds)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHeaderInds Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHeaderInds", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataTakeOn (Public)
    '
    '
    ' ***************************************************************** '

    Public Function DataTakeOn(ByRef vLookupDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        'This is very close to update, but we have to check that the item already exists first.

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vLookupDetails) Then
                Return result
            End If

            'First loop around and add in the new guys
            For lTemp As Integer = vLookupDetails.GetLowerBound(1) To vLookupDetails.GetUpperBound(1)


                If CDbl(vLookupDetails(ACHLookupDetailId, lTemp)) = -1 Then
                    m_oDatabase.Parameters.Clear()


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_user_def_header_id", vValue:=CStr(vLookupDetails(ACHLookupHeaderId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(vLookupDetails(ACHCode, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckLookupDetailsSQL, sSQLName:=ACCheckLookupDetailsName, bStoredProcedure:=True, vResultArray:=vArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Information.IsArray(vArray) Then


                        vLookupDetails(ACHLookupDetailId, lTemp) = vArray(0, 0)
                    Else

                        vLookupDetails(ACHLookupDetailId, lTemp) = -1
                    End If

                End If
            Next lTemp

            m_lReturn = CType(Update(vLookupDetails:=vLookupDetails), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataTakeOn  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataTakeOn ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetCaptionID
    '
    ' Description: Calls the spu_pm_caption_id_return stored procedure
    '              to either get or create a caption_id
    '
    ' ***************************************************************** '
    Private Function GetCaptionID(ByVal v_sCaption As String, ByRef r_lCaptionID As Integer) As Integer

        Dim result As Integer = 0


        ' m_iLanguageID

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the parameters
        m_oArcDatabase.Parameters.Clear()

        ' Add the parameters
        m_lReturn = m_oArcDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oArcDatabase.Parameters.Add(sName:="caption", vValue:=v_sCaption, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oArcDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(r_lCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Perform the stored procedure
        m_lReturn = m_oArcDatabase.SQLAction(sSQL:=ACSQLCaptionReturn, sSQLName:=ACSQLCaptionReturnName, bStoredProcedure:=ACSQLCaptionReturnStored)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the returned caption_id
        r_lCaptionID = m_oArcDatabase.Parameters.Item("caption_id").Value

        Return result

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
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
    ' Name: GetByCode
    '
    ' Description:
    '
    ' History: 18/05/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetByCode(ByRef lGISUserDefHeaderId As Integer, ByRef lGISUserDefDetailId As Integer, ByRef sCode As String) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sCode = sCode.Trim().ToUpper()

            sSQL = "SELECT GIS_user_def_detail_id" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "FROM GIS_user_def_detail" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "WHERE upper(code) = '" & sCode & "'"
            sSQL = sSQL & "AND GIS_user_def_header_id = " & CStr(lGISUserDefHeaderId)

            'Don't check against itself for update.
            If lGISUserDefDetailId > 0 Then
                sSQL = sSQL & Strings.Chr(13) & Strings.Chr(10) & "AND GIS_user_def_detail_id <> " & CStr(lGISUserDefDetailId)
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetUserDefDetailByCode", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetByCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetByCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'Start(Sriram P)CacheBug
    Public Function UpdateCache(ByVal v_lLookupHeaderId As Integer) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        Const kMethodName As String = "UpdateCache"

        Dim vSelectCaptionsResults As Object

        ' RAM20040310 : Code changes related to Caching
        ''developer guide no. 12

        Dim oCache As Hashtable
        Dim sKey As String = ""
        Dim vLookupsXML, sLookupsXML As String ' 64 K Limit ????
        Dim sFilter As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oADORecordset As DataSet
        'developer guide no. 30
        'Dim oADOStream As ADODB.Stream
        Dim vFieldNameArray As Object
        Dim sSQL As String = ""
        Dim vKeyArray As Object


        Try

            ' eg. KEY_GIS_LOOKUP_00026_00018 :  means : Language ID 26,  Table ID 18
            sKey = "KEY_GIS_LOOKUP_" & StringsHelper.Format(m_iLanguageID, "00000") & "_" & StringsHelper.Format(v_lLookupHeaderId, "00000")

            ' Create the Cache Object
            'developer guide no. 12

            oCache = New Hashtable

            ' Get from the Cache by the Key, if available

            vLookupsXML = CStr(oCache.Item(sKey))

            ' Make-up the SQL Script to fetch the Lookups
            sSQL = ACSelectGISUserDefDetailToCacheSQL
            ' Replace the Table Name with the supplied one
            sSQL = sSQL.Replace("{table}", CStr(v_lLookupHeaderId))
            ' Replace the Language ID
            sSQL = sSQL.Replace("{Language_ID}", CStr(m_iLanguageID))

            ' Create a new ADO Recordset
            oADORecordset = New DataSet()

            ' Execute the SQL
            lReturn = m_oDatabase.BatchSQLSelect(sSQL, oADORecordset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get the values from database")
            End If

            ' Save the Recordset into the Stream as XML

            ' Create a New Stream Object
            'developer guide no. 30
            'oADOStream = New ADODB.Stream()



            'oADORecordset.save(oADOStream, ADODB.PersistFormatEnum.adPersistXML)

            ' Get the Lookups into a String
            'developer guide no. 30
            'vLookupsXML = oADOStream.ReadText()
            If (oADORecordset.Tables.Count > 0) Then
                If (oADORecordset.Tables(0).Rows.Count > 0) Then
                    vLookupsXML = oADORecordset.GetXml()
                End If
            End If

            'Clean up memory
            'developer guide no. 30
            'oADOStream.Close()
            'oADOStream = Nothing
            oADORecordset = Nothing

            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Add them to the Cache
                oCache.Add(sKey, vLookupsXML)


                ' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
                ' Sirius Cache Controller


                vKeyArray = oCache.Item("SIRIUS_CACHE_KEYS")

                If Object.Equals(vKeyArray, Nothing) Then
                    ReDim vKeyArray(0)
                Else

                    ReDim Preserve vKeyArray(vKeyArray.GetUpperBound(0) + 1)
                End If


                vKeyArray(vKeyArray.GetUpperBound(0)) = sKey
                ' Remove the existing keys first
                oCache.Remove("SIRIUS_CACHE_KEYS")
                ' Add the updated one
                oCache.Add("SIRIUS_CACHE_KEYS", vKeyArray)

            Else
                ' Log Error Message
                gPMFunctions.RaiseError(kMethodName, "Failed to get the values from database")

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function
    'End(Sriram P)CacheBug
End Class
