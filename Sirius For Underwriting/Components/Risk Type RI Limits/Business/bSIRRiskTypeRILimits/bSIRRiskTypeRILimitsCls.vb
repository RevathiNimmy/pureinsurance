Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 09/06/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a Risk Type RI Limit.
    '
    ' Edit History:
    ' SJP14062002 - getUnderwritingType uses new product options scheme
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

    ' Primary Keys to work with
    Private m_lRiskTypeId As Integer
    Private m_nRiskTypeRILimitVersionId As Integer
    'JMK 22/10/2001 - Underwriting hidden option
    Private m_sUnderwritingType As String = ""

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

    Public Property RiskTypeId() As Integer
        Get
            Return m_lRiskTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    Public Property RiskTypeRILimitVersionId() As Integer
        Get
            Return m_nRiskTypeRILimitVersionId
        End Get
        Set(value As Integer)
            m_nRiskTypeRILimitVersionId = value
        End Set
    End Property

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetUnderwritingType
    '
    ' Description:  Finds out Underwriting type - U or A
    '               For labelling: A - Insurer. U - Reinsurer
    '
    ' JMK 22/10/2001    Created
    ' SJP14062002 - getUnderwritingType uses new product options scheme
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (getUnderwritingType) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function getUnderwritingType() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    'Return bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingType)
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUnderwritingTypeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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

    Public Function GetRiskTypeRILimits(ByRef r_vRiskTypeRILimits(,) As Object) As Integer

        Dim result As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(m_lRiskTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id", vValue:=m_nRiskTypeRILimitVersionId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRiskTypeRILimitsSQL, sSQLName:=ACSelectRiskTypeRILimitsName, bStoredProcedure:=ACSelectRiskTypeRILimitsStored, vResultArray:=r_vRiskTypeRILimits)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeRILimits")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskTypeRILimits Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeRILimits", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetAllowedProperties(ByRef r_vAllowedProperties(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(m_lRiskTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectGISPropertiesSQL, sSQLName:=ACSelectGISPropertiesName, bStoredProcedure:=ACSelectGISPropertiesStored, vResultArray:=r_vAllowedProperties)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllowedProperties")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllowedProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllowedProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name: Update (Public)
    '
    '
    ' ***************************************************************** '
    Public Function Update(ByVal v_vRiskTypeRILimits(,) As Object, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer
        Dim ScreenHierarchy As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(m_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id", vValue:=m_nRiskTypeRILimitVersionId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=ScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Delete the risk type properties
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRiskTypeRILimitsSQL, sSQLName:=ACDeleteRiskTypeRILimitsName, bStoredProcedure:=ACDeleteRiskTypeRILimitsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(m_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id", vValue:=m_nRiskTypeRILimitVersionId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Delete the risk type values
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRiskTypeRIValuesSQL, sSQLName:=ACDeleteRiskTypeRIValuesName, bStoredProcedure:=ACDeleteRiskTypeRIValuesStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(v_vRiskTypeRILimits) Then
                m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            'Re-add the properties
            For lTemp As Integer = v_vRiskTypeRILimits.GetLowerBound(1) To v_vRiskTypeRILimits.GetUpperBound(1)


                If CInt(v_vRiskTypeRILimits(ACRGISPropertyId, lTemp)) <> 0 Then

                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(m_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_properties_seq_id", vValue:=CStr(lTemp + 1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_property_id", vValue:=CStr(v_vRiskTypeRILimits(ACRGISPropertyId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id", vValue:=m_nRiskTypeRILimitVersionId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=ScreenHierarchy & $"/Object({CStr(v_vRiskTypeRILimits(ACRDescription, lTemp)).Trim()})", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertRiskTypeRILimitsSQL, sSQLName:=ACInsertRiskTypeRILimitsName, bStoredProcedure:=ACInsertRiskTypeRILimitsStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

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


    ''' <summary>
    ''' returns all ri limits for a risk type
    ''' </summary>
    ''' <param name="m_oRILimitVersion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRiskTypeRILimitsVersion(ByRef m_oRILimitVersion As Object) As Integer
        Dim result As gPMConstants.PMEReturnCode
        Dim nReturn As Integer
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=m_lRiskTypeId,
                                                 iDirection:=PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRiskTypeRILimitsVersionSQL,
                                            sSQLName:=ACSelectRiskTypeRILimitsVersionName,
                                            bStoredProcedure:=ACSelectRiskTypeRILimitsVersionStored,
                                            vResultArray:=m_oRILimitVersion)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername,
                                   iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="m_oDatabase.SQLSelect failed",
                                   vApp:=ACApp,
                                   vClass:=ACClass,
                                   vMethod:="GetRiskTypeRILimits")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskTypeRILimits Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeRILimits", vErrNo:=Err.Number, vErrDesc:=ex.Message, excep:=ex)
            Return result
        End Try
    End Function

    ''' <summary>
    ''' This method copies Limits
    ''' </summary>
    ''' <param name="nRiskTypeRILimitVersionId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyRiskTypeRILimitsVersion(ByVal nRiskTypeRILimitVersionId As Integer) As Integer
        Dim result As Integer
        Dim nReturn As Integer
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=m_lRiskTypeId,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id",
                                                 vValue:=nRiskTypeRILimitVersionId,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            nReturn = m_oDatabase.SQLAction(sSQL:=ACCopyRiskTypeRILimitsSQL,
                                            sSQLName:=ACCopyRiskTypeRILimitsVersionName,
                                            bStoredProcedure:=ACSelectRiskTypeRILimitsVersionStored)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername,
                                   iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="m_oDatabase.SQLSelect failed",
                                   vApp:=ACApp,
                                   vClass:=ACClass,
                                   vMethod:="CopyRiskTypeRILimitsVersion")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskTypeRILimits Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeRILimits", vErrNo:=Err.Number, vErrDesc:=ex.Message, excep:=ex)

            Return result

        End Try
    End Function


    ''' <summary>
    ''' updates limits 
    ''' </summary>
    ''' <param name="m_oRILimitVersion"></param>
    ''' <param name="nRILimitVersionId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateLimitVersions(ByVal m_oRILimitVersion As Object, ByRef nRILimitVersionId As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_SScreenHierarchy As String = "") _
        As Integer


        Dim nTemp As Integer
        Dim nRecordsAffected As Integer
        Dim result As Integer
        Dim nReturn As Integer
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            nReturn = 1
            'Read the RI model usages
            For nTemp = LBound(m_oRILimitVersion, 2) To UBound(m_oRILimitVersion, 2)

                Select Case (m_oRILimitVersion(ACRItemStatus, nTemp))
                    Case ACItemStatus_Unchanged
                        ' -------------------------------
                        ' We don't do anything
                        ' -------------------------------

                    Case ACItemStatus_Changed
                        ' -------------------------------
                        ' We update the existing record
                        ' -------------------------------
                        m_oDatabase.Parameters.Clear()
                        ' Add parameters
                        nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id",
                                                             vValue:=m_oRILimitVersion(ACRRILimitVersionId, nTemp),
                                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                             iDataType:=gPMConstants.PMEDataType.PMInteger)
                        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=m_lRiskTypeId,
                                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                             iDataType:=gPMConstants.PMEDataType.PMInteger)
                        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        nReturn = m_oDatabase.Parameters.Add(sName:="ri_limit_desc",
                                                             vValue:=m_oRILimitVersion(ACRLimitDescription, nTemp),
                                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                             iDataType:=gPMConstants.PMEDataType.PMString)
                        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        nReturn = m_oDatabase.Parameters.Add(sName:="limit_effective_date",
                                                             vValue:=CDate(m_oRILimitVersion(ACREffectiveDate, nTemp)),
                                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                             iDataType:=gPMConstants.PMEDataType.PMDate)
                        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        nReturn = m_oDatabase.Parameters.Add(sName:="limit_expiry_date",
                                                             vValue:=CDate(m_oRILimitVersion(ACRExpiryDate, nTemp)),
                                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                             iDataType:=gPMConstants.PMEDataType.PMDate)
                        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_SScreenHierarchy & $"/Limit({CStr(m_oRILimitVersion(ACRLimitDescription, nTemp)).Trim()})", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' SQL
                        nReturn = m_oDatabase.SQLAction(
                            sSQL:=ACUpdateRiskTypeRILimitsVersionSQL,
                            sSQLName:=ACUpdateRiskTypeRILimitsVersionName,
                            bStoredProcedure:=ACUpdateRiskTypeRILimitsVersionStored)

                    Case ACItemStatus_Added
                        ' -------------------------------
                        ' We add a new record
                        ' -------------------------------
                        m_oDatabase.Parameters.Clear()
                        ' Add parameters
                        nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id", vValue:=0,
                                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput,
                                                             iDataType:=gPMConstants.PMEDataType.PMInteger)

                        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=m_lRiskTypeId,
                                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                             iDataType:=gPMConstants.PMEDataType.PMInteger)

                        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        nReturn = m_oDatabase.Parameters.Add(sName:="ri_limit_desc",
                                                             vValue:=m_oRILimitVersion(ACRLimitDescription, nTemp),
                                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                             iDataType:=gPMConstants.PMEDataType.PMString)

                        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        nReturn = m_oDatabase.Parameters.Add(sName:="limit_effective_date",
                                                             vValue:=CDate(m_oRILimitVersion(ACREffectiveDate, nTemp)),
                                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                             iDataType:=gPMConstants.PMEDataType.PMDate)

                        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        nReturn = m_oDatabase.Parameters.Add(sName:="limit_expiry_date",
                                                             vValue:=CDate(m_oRILimitVersion(ACRExpiryDate, nTemp)),
                                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                             iDataType:=gPMConstants.PMEDataType.PMDate)
                        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_SScreenHierarchy & $"/Limit({CStr(m_oRILimitVersion(ACRLimitDescription, nTemp)).Trim()})", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        ' SQL
                        nReturn = m_oDatabase.SQLAction(
                            sSQL:=ACInsertRiskTypeRILimitsVersionSQL,
                            sSQLName:=ACInsertRiskTypeRILimitsVersionName,
                            bStoredProcedure:=ACInsertRiskTypeRILimitsVersionStored,
                            lRecordsAffected:=nRecordsAffected)

                        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If Convert.IsDBNull(m_oDatabase.Parameters.Item("risk_type_ri_limit_version_id").Value) OrElse IsNothing(m_oDatabase.Parameters.Item("risk_type_ri_limit_version_id").Value) Then
                            m_oDatabase.Parameters.Item("risk_type_ri_limit_version_id").Value = 0
                        End If
                        ' Get the Cnt of the record inserted
                        nRILimitVersionId = m_oDatabase.Parameters.Item("risk_type_ri_limit_version_id").Value

                        If (nRILimitVersionId < 1) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                    Case ACItemStatus_Deleted
                        ' -------------------------------
                        ' We delete this record
                        ' -------------------------------
                        m_oDatabase.Parameters.Clear()

                        nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=m_lRiskTypeId,
                                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                             iDataType:=gPMConstants.PMEDataType.PMInteger)
                        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id",
                                                             vValue:=m_oRILimitVersion(ACRRILimitVersionId, nTemp),
                                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                             iDataType:=gPMConstants.PMEDataType.PMInteger)
                        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_SScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        nReturn = m_oDatabase.SQLAction(
                            sSQL:=ACDeleteRiskTypeRILimitVersionSQL,
                            sSQLName:=ACDeleteRiskTypeRILimitVersionName,
                            bStoredProcedure:=ACDeleteRiskTypeRILimitVersionStored)

                    Case ACItemStatus_Copy
                        '  m_oDatabase.Parameters.Clear
                        '
                        '  nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", _
                        '                                             vValue:=m_lRiskTypeId, _
                        '                                            idirection:=PMParamInput, _
                        '                                           iDataType:=PMLong)
                        '
                        '
                        '  nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id", _
                        '                                                    vValue:=m_oRILimitVersion(ACRRILimitVersionId, nTemp), _
                        '                                                   idirection:=PMParamInput, _
                        '                                                  iDataType:=PMLong)
                        '
                        '
                        '  nReturn& = m_oDatabase.SQLAction(sSQL:=ACCopyRiskTypeRILimitsSQL, _
                        '                                             sSQLName:=ACCopyRiskTypeRILimitsVersionName, _
                        ''                                            bStoredProcedure:=ACSelectRiskTypeRILimitsVersionStored)
                        '

                End Select

                If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next nTemp

            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLimitVersions  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLimitVersions ", vErrNo:=Err.Number, vErrDesc:=ex.Message, excep:=ex)
            Return result
        End Try
    End Function

    ''' <summary>
    ''' delete risk type limit version
    ''' </summary>
    ''' <param name="r_nRiskTypeRILimitVersionId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteRiskTypeRILimitVersion(ByVal r_nRiskTypeRILimitVersionId As Integer) As Integer

        Dim nResult As Integer
        Dim nReturn As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=m_lRiskTypeId,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id",
                                                   vValue:=r_nRiskTypeRILimitVersionId,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = m_oDatabase.SQLAction(
                sSQL:=ACDeleteRiskTypeRILimitVersionSQL,
                sSQLName:=ACDeleteRiskTypeRILimitVersionName,
                bStoredProcedure:=ACDeleteRiskTypeRILimitVersionStored)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return nResult
        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRILimitVersion  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRILimitVersion ", vErrNo:=Err.Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try
    End Function

End Class

