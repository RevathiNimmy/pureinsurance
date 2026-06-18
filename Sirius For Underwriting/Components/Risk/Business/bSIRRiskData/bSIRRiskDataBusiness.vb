Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRRiskData.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 19/12/2003
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

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            m_sTransactionType = Value

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

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
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
    ' Name: GetRisk (Public)
    '
    ' Desc: get all associate risks for policy
    '
    ' ***************************************************************** '
    Public Function GetRisk(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return m_oDatabase.SQLSelect(sSQL:=ACSaaRiskSQL, sSQLName:=ACSaaRiskName, bStoredProcedure:=ACSaaRiskStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyRisk (Private)
    '
    ' Desc: copy risk details from OldInsuranceFileCnt to NewInsuranceFileCnt
    '
    ' Changes: RWH(20/11/2000) Updated for new Risk database structure
    '           which uses Insurance_file_risk_link table rather than
    '           storing insurance_file_cnt on the Risk table.
    ' ***************************************************************** '

    Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_vRiskDetail(,) As Object, ByVal v_lPosNo As Integer, ByRef r_lRiskCnt As Integer, ByVal v_lResetStatus As Integer, ByVal v_lCreateLinkType As Integer) As Integer
        Return CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vRiskDetail:=v_vRiskDetail, v_lPosNo:=v_lPosNo, r_lRiskCnt:=r_lRiskCnt, v_lResetStatus:=v_lResetStatus, v_lCreateLinkType:=v_lCreateLinkType, v_bAutoCancellation:=False, v_sRiskMergeStatus:="", v_iRiskSelected:=0)
    End Function
    Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_vRiskDetail(,) As Object, ByVal v_lPosNo As Integer, ByRef r_lRiskCnt As Integer, ByVal v_lResetStatus As Integer) As Integer
        Return CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vRiskDetail:=v_vRiskDetail, v_lPosNo:=v_lPosNo, r_lRiskCnt:=r_lRiskCnt, v_lResetStatus:=v_lResetStatus, v_lCreateLinkType:=0, v_bAutoCancellation:=False, v_sRiskMergeStatus:="", v_iRiskSelected:=0)
    End Function

    Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_vRiskDetail(,) As Object, ByVal v_lPosNo As Integer, ByRef r_lRiskCnt As Integer, ByVal v_lResetStatus As Integer, ByVal v_bAutoCancellation As Boolean, ByRef v_sRiskMergeStatus As String) As Integer
        Return CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vRiskDetail:=v_vRiskDetail, v_lPosNo:=v_lPosNo, r_lRiskCnt:=r_lRiskCnt, v_lResetStatus:=v_lResetStatus, v_lCreateLinkType:=0, v_bAutoCancellation:=v_bAutoCancellation, v_sRiskMergeStatus:=v_sRiskMergeStatus, v_iRiskSelected:=0)
    End Function

    Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_vRiskDetail(,) As Object, ByVal v_lPosNo As Integer, ByRef r_lRiskCnt As Integer) As Integer
        Return CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vRiskDetail:=v_vRiskDetail, v_lPosNo:=v_lPosNo, r_lRiskCnt:=r_lRiskCnt, v_lResetStatus:=0, v_lCreateLinkType:=0, v_bAutoCancellation:=False, v_sRiskMergeStatus:="", v_iRiskSelected:=0)
    End Function

    Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_vRiskDetail(,) As Object, ByVal v_lPosNo As Integer, ByRef r_lRiskCnt As Integer, ByVal v_lResetStatus As Integer, ByVal v_lCreateLinkType As Integer, ByVal v_bAutoCancellation As Boolean, ByRef v_sRiskMergeStatus As String, ByVal v_iRiskSelected As Integer) As Integer

        Dim result As Integer = 0
        Dim lRiskCnt As Integer

        Dim lOldRiskCnt, lNewRiskCnt As Integer

        Dim iIsAutoReinsured As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            lOldRiskCnt = CInt(v_vRiskDetail(0, v_lPosNo))

            'Tomo030801
            'This bit's here because we need to reset the auto reinsured flag to that
            'from the risk type.
            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_vRiskDetail(4, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAutoReinsuredSQL, sSQLName:=ACGetAutoReinsuredName, bStoredProcedure:=ACGetAutoReinsuredStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then

                iIsAutoReinsured = CInt(vArray(0, 0))
            Else
                iIsAutoReinsured = 0
            End If


            v_vRiskDetail(20, v_lPosNo) = iIsAutoReinsured


            vArray = Nothing

            'Tomo030801 - End

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TN20010719 - start
            If v_lResetStatus = gPMConstants.PMEReturnCode.PMTrue Then
                'reset status to UnQuoted
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_status_id", vValue:=CStr(4), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'TN20010719 - end
            Else

                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_status_id", vValue:=CStr(v_vRiskDetail(1, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_cnt", vValue:=CStr(v_vRiskDetail(2, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="accumulation_id", vValue:=CStr(v_vRiskDetail(3, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_vRiskDetail(4, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(v_vRiskDetail(5, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="sequence_number", vValue:=CStr(v_vRiskDetail(6, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="sum_insured_requested", vValue:=CStr(v_vRiskDetail(7, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="inception_date", vValue:=CStr(v_vRiskDetail(8, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="expiry_date", vValue:=CStr(v_vRiskDetail(9, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_not_index_linked", vValue:=CStr(v_vRiskDetail(10, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_accumulated", vValue:=CStr(v_vRiskDetail(11, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_reason_id", vValue:=CStr(v_vRiskDetail(12, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_date", vValue:=CStr(v_vRiskDetail(13, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_description", vValue:=CStr(v_vRiskDetail(14, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="var_data_ref", vValue:=CStr(v_vRiskDetail(15, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="total_sum_insured", vValue:=CStr(v_vRiskDetail(16, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="total_annual_premium", vValue:=CStr(v_vRiskDetail(17, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="total_this_premium", vValue:=CStr(v_vRiskDetail(18, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_ri_at_risk_level", vValue:=CStr(v_vRiskDetail(19, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_auto_reinsured", vValue:=CStr(v_vRiskDetail(20, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_screen_id", vValue:=CStr(v_vRiskDetail(21, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="eml_percentage", vValue:=CStr(v_vRiskDetail(22, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW311002

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_number", vValue:=CStr(v_vRiskDetail(23, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW021202

            m_lReturn = m_oDatabase.Parameters.Add(sName:="variation_number", vValue:=CStr(v_vRiskDetail(24, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW021202
            If v_iRiskSelected = 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="is_risk_selected", vValue:=CStr(v_vRiskDetail(25, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="is_risk_selected",
                                                    vValue:=1,
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                    iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' PW311002

            m_lReturn = m_oDatabase.Parameters.Add(sName:="coverage", vValue:=CStr(v_vRiskDetail(26, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW311002

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insured_item", vValue:=CStr(v_vRiskDetail(27, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW311002

            m_lReturn = m_oDatabase.Parameters.Add(sName:="extensions", vValue:=CStr(v_vRiskDetail(28, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="premium_this_year", vValue:=CStr(v_vRiskDetail(31, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Wpr53
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_mandatory_risk", vValue:=ToSafeInteger(v_vRiskDetail(36, v_lPosNo), 0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRiskSQL, sSQLName:=ACAddRiskName, bStoredProcedure:=ACAddRiskStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lRiskCnt = m_oDatabase.Parameters.Item("risk_cnt").Value

            If Not v_bAutoCancellation Then

                'RWH(20/11/2000) Add link record into Insurance_file_risk_link.
                If lRiskCnt <> 0 Then

                    ' Determine whether or not to create a link and
                    ' if so what kind of link

                    Select Case v_lCreateLinkType
                        Case 0 ' standard - original and renewed risk cnt not populated
                            m_lReturn = CType(AddRiskLink(v_lNewInsuranceFileCnt, lRiskCnt, "C", 0, 0), gPMConstants.PMEReturnCode)

                        Case 1 ' populate original_risk_cnt
                            m_lReturn = CType(AddRiskLink(v_lNewInsuranceFileCnt, lRiskCnt, "C", lOldRiskCnt, 0), gPMConstants.PMEReturnCode)

                        Case 2 ' populate renewed_risk_cnt
                            m_lReturn = CType(AddRiskLink(v_lNewInsuranceFileCnt, lRiskCnt, "C", 0, lOldRiskCnt), gPMConstants.PMEReturnCode)

                        Case Else
                            ' do nothing
                    End Select

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    r_lRiskCnt = lRiskCnt

                    m_lReturn = CType(CopyRatingSection(v_lOldRiskCnt:=lOldRiskCnt, v_lNewRiskCnt:=lRiskCnt), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            Else

                ' delete the original insurance file risk link
                m_lReturn = CType(DeleteInsuranceFileRiskLink(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lRiskCnt:=lOldRiskCnt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' add new
                m_lReturn = CType(AddRiskLink(v_lNewInsuranceFileCnt, lRiskCnt, If(v_sRiskMergeStatus = "DP", "D", "C"), If(v_sRiskMergeStatus = "A", 0, lOldRiskCnt)), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_lRiskCnt = lRiskCnt
                lNewRiskCnt = lRiskCnt

                m_lReturn = CType(CopyRiskExtras(v_lOldRiskCnt:=lOldRiskCnt, v_lNewRiskCnt:=lNewRiskCnt), gPMConstants.PMEReturnCode)

                'sj 20/12/2002 - end
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetGISPolicyLink (Private)
    '
    ' Desc: get details from gis policy link table using InsuranceFileCnt and RiskID
    '
    ' ***************************************************************** '
    Public Function GetGISPolicyLink(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add("gis_policy_link_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RWH(20/11/2000) We are using file_cnt field to hold folder_cnt (are you scared yet!!!)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add("quote_ref", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=CStr(v_lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return m_oDatabase.SQLSelect(sSQL:=ACSelGisPolicyLinkSQL, sSQLName:=ACSelGisPolicyLinkName, bStoredProcedure:=ACSelGisPolicyLinkStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGISPolicyLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISPolicyLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyBinderID (Private)
    '
    ' Desc: get policy binder id attach to policy link id
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetPolicyBinderID) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetPolicyBinderID(ByVal v_lPolicyLinkID As Object, ByRef r_lPolicyBinderID As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim vResultArray As Object
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'm_oDatabase.Parameters.Clear()
    '

    'm_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lPolicyLinkID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'If m_oDatabase.SQLSelect(sSQL:=ACSelPolicyBinderSQL, sSQLName:=ACSelPolicyBinderName, bStoredProcedure:=ACSelPolicyBinderStored, vResultArray:=vResultArray, bKeepNulls:=True) <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'If Informations.IsArray(vResultArray) Then
    'there should only be one record

    'r_lPolicyBinderID = CInt(vResultArray(0, 0))
    'Else
    'result = gPMConstants.PMEReturnCode.PMNotFound
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyBinderID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyBinderID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetRSASumInsured (Private)
    '
    ' Desc: get all sum insured attached to policy link id
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetRSASumInsured) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetRSASumInsured(ByVal v_lPolicyLinkID As Integer, ByRef r_vResultArray As Object) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'm_oDatabase.Parameters.Clear()
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lPolicyLinkID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    '
    'Return m_oDatabase.SQLSelect(sSQL:=ACSelRSASumInsuredSQL, sSQLName:=ACSelRSASumInsuredName, bStoredProcedure:=ACSelRSASumInsuredStored, vResultArray:=r_vResultArray, bKeepNulls:=True)
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRSASumInsured Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRSASumInsured", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CopyRSASumInsured (Private)
    '
    ' Desc: copy all sum insured from old policy binder to new policy binder
    '
    ' ***************************************************************** '
    Public Function CopyRSASumInsured(ByVal v_lOldPolicyLinkID As Integer, ByVal v_lNewPolicyLinkID As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            '    'get policy binder id for v_lNewPolicyLinkID
            '    m_lReturn& = GetPolicyBinderID(v_lPolicyLinkID:=v_lNewPolicyLinkID, r_lPolicyBinderID:=lNewPolicyBinderID)
            '
            '    If m_lReturn <> PMTrue Then
            '        If m_lReturn = PMNotFound Then
            '            ' Log Error Message
            '            LogMessage m_sUsername, _
            ''                iType:=PMLogOnError, _
            ''                sMsg:="No Policy Binder found for PolicyLinkID :=" & v_lNewPolicyLinkID, _
            ''                vApp:=ACApp, _
            ''                vClass:=ACClass, _
            ''                vMethod:="CopyRSASumInsured", _
            ''                vErrNo:=Err.Number, _
            ''                vErrDesc:=Err.Description
            '        End If
            '
            '        CopyRSASumInsured = PMFalse
            '        Exit Function
            '    End If
            '
            '    'get all sum insured attached to Policy Binder using v_lOldPolicyLinkID
            '    If GetRSASumInsured(v_lPolicyLinkID:=v_lOldPolicyLinkID, r_vResultArray:=vSumInsuredArray) <> PMTrue Then
            '        CopyRSASumInsured = PMFalse
            '        Exit Function
            '    End If
            '
            '    'do we have any data
            '    If Not IsArray(vSumInsuredArray) Then
            '        Exit Function
            '    End If
            '
            '    'loop thro each sum insured and copy them to new policy binder id
            '    For lCount& = 0 To UBound(vSumInsuredArray, 2)
            '
            '        m_oDatabase.Parameters.Clear
            '
            '        m_lReturn& = m_oDatabase.Parameters.Add(sName:="rsa_policy_binder_id", _
            ''                                                vValue:=lNewPolicyBinderID, _
            ''                                                iDirection:=PMParamInput, _
            ''                                                iDataType:=PMLong)
            '        If m_lReturn& <> PMTrue Then
            '            CopyRSASumInsured = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = m_oDatabase.Parameters.Add(sName:="sum_insured_type_id", _
            ''                                                vValue:=vSumInsuredArray(1, lCount&), _
            ''                                                iDirection:=PMParamInput, _
            ''                                                iDataType:=PMLong)
            '        If m_lReturn& <> PMTrue Then
            '            CopyRSASumInsured = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = m_oDatabase.Parameters.Add(sName:="sequence_id", _
            ''                                                vValue:=vSumInsuredArray(2, lCount&), _
            ''                                                iDirection:=PMParamInput, _
            ''                                                iDataType:=PMLong)
            '        If m_lReturn& <> PMTrue Then
            '            CopyRSASumInsured = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = m_oDatabase.Parameters.Add(sName:="description", _
            ''                                                vValue:=vSumInsuredArray(3, lCount&), _
            ''                                                iDirection:=PMParamInput, _
            ''                                                iDataType:=PMString)
            '        If m_lReturn& <> PMTrue Then
            '            CopyRSASumInsured = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = m_oDatabase.Parameters.Add(sName:="reference", _
            ''                                                vValue:=vSumInsuredArray(4, lCount&), _
            ''                                                iDirection:=PMParamInput, _
            ''                                                iDataType:=PMString)
            '        If m_lReturn& <> PMTrue Then
            '            CopyRSASumInsured = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = m_oDatabase.Parameters.Add(sName:="sum_insured", _
            ''                                                vValue:=vSumInsuredArray(5, lCount&), _
            ''                                                iDirection:=PMParamInput, _
            ''                                                iDataType:=PMDouble)
            '        If m_lReturn& <> PMTrue Then
            '            CopyRSASumInsured = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = m_oDatabase.Parameters.Add(sName:="date_added", _
            ''                                                vValue:=vSumInsuredArray(6, lCount&), _
            ''                                                iDirection:=PMParamInput, _
            ''                                                iDataType:=PMDate)
            '        If m_lReturn& <> PMTrue Then
            '            CopyRSASumInsured = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = m_oDatabase.Parameters.Add(sName:="date_deleted", _
            ''                                                vValue:=vSumInsuredArray(7, lCount&), _
            ''                                                iDirection:=PMParamInput, _
            ''                                                iDataType:=PMDate)
            '        If m_lReturn& <> PMTrue Then
            '            CopyRSASumInsured = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = m_oDatabase.Parameters.Add(sName:="is_valuation_required", _
            ''                                                vValue:=vSumInsuredArray(8, lCount&), _
            ''                                                iDirection:=PMParamInput, _
            ''                                                iDataType:=PMInteger)
            '        If m_lReturn& <> PMTrue Then
            '            CopyRSASumInsured = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = m_oDatabase.Parameters.Add(sName:="valuation_date", _
            ''                                                vValue:=vSumInsuredArray(9, lCount&), _
            ''                                                iDirection:=PMParamInput, _
            ''                                                iDataType:=PMDate)
            '        If m_lReturn& <> PMTrue Then
            '            CopyRSASumInsured = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = m_oDatabase.Parameters.Add(sName:="rate", _
            ''                                                vValue:=vSumInsuredArray(10, lCount&), _
            ''                                                iDirection:=PMParamInput, _
            ''                                                iDataType:=PMDecimal)
            '        If m_lReturn& <> PMTrue Then
            '            CopyRSASumInsured = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = m_oDatabase.Parameters.Add(sName:="premium", _
            ''                                                vValue:=vSumInsuredArray(11, lCount&), _
            ''                                                iDirection:=PMParamInput, _
            ''                                                iDataType:=PMDouble)
            '        If m_lReturn& <> PMTrue Then
            '            CopyRSASumInsured = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = m_oDatabase.SQLAction(sSQL:=ACAddSumInsuredSQL, _
            ''                                sSQLName:=ACAddSumInsuredName, _
            ''                                bStoredProcedure:=ACAddSumInsuredStored)
            '
            '        If m_lReturn& <> PMTrue Then
            '            CopyRSASumInsured = PMFalse
            '            Exit Function
            '        End If
            '
            '    Next lCount&
            '
            '    CopyRSASumInsured = PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="old_policy_link_id", vValue:=CStr(v_lOldPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_policy_link_id", vValue:=CStr(v_lNewPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopySumsInsuredSQL, sSQLName:=ACCopySumsInsuredName, bStoredProcedure:=ACCopySumsInsuredStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRSASumInsured Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRSASumInsured", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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
    '
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
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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
    '
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
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' private Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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


    ' ***************************************************************** '
    '
    ' Name: AddRiskLink
    '
    ' Description: Adds record to Insurance_file_risk_link.
    '
    ' History: 20/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function AddRiskLink(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_sStatusFlag As String) As Integer
        Return AddRiskLink(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lRiskCnt:=v_lRiskCnt, v_sStatusFlag:=v_sStatusFlag, v_lOriginalRiskCnt:=0, v_lRenewedRiskCnt:=0)
    End Function
    Public Function AddRiskLink(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_sStatusFlag As String, ByVal v_lOriginalRiskCnt As Integer) As Integer
        Return AddRiskLink(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lRiskCnt:=v_lRiskCnt, v_sStatusFlag:=v_sStatusFlag, v_lOriginalRiskCnt:=v_lOriginalRiskCnt, v_lRenewedRiskCnt:=0)
    End Function
    Public Function AddRiskLink(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_sStatusFlag As String, ByVal v_lOriginalRiskCnt As Integer, ByVal v_lRenewedRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="status_flag", vValue:=v_sStatusFlag, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_lOriginalRiskCnt = 0 Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="original_risk_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="original_risk_cnt", vValue:=CStr(v_lOriginalRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_lRenewedRiskCnt = 0 Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add("renewed_risk_cnt", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="renewed_risk_cnt", vValue:=CStr(v_lRenewedRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_sTransactionType = "REN" Or m_sTransactionType = "PT" Or m_sTransactionType = "DRI" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="is_risk_edited", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRiskLinkSQL, sSQLName:=ACAddRiskLinkName, bStoredProcedure:=ACAddRiskLinkStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRiskLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRiskLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: CopyRatingSection
    '
    ' Description:
    '
    ' History: 23/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function CopyRatingSection(ByVal v_lOldRiskCnt As Integer, ByVal v_lNewRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="OldRiskCnt", vValue:=CStr(v_lOldRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="NewRiskCnt", vValue:=CStr(v_lNewRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyRatingSectionSQL, sSQLName:=ACCopyRatingSectionName, bStoredProcedure:=ACCopyRatingSectionStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRatingSection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRatingSection", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyPerils
    '
    ' Description:
    '
    ' History: 23/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function CopyPerils(ByVal v_lOldRiskCnt As Integer, ByVal v_lNewRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="OldRiskCnt", vValue:=CStr(v_lOldRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="NewRiskCnt", vValue:=CStr(v_lNewRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyPerilsSQL, sSQLName:=ACCopyPerilsName, bStoredProcedure:=ACCopyPerilsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPerils Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPerils", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateRiskStatus
    '
    ' Description: update risk status id
    '
    ' History: 19/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function UpdateRiskStatus(ByVal v_lRiskCnt As Object, ByVal v_lRiskStatusID As Object) As Integer
        Return UpdateRiskStatus(v_lRiskCnt:=v_lRiskCnt, v_lRiskStatusID:=v_lRiskStatusID, v_sRiskStatusCode:=Nothing)
    End Function
    Public Function UpdateRiskStatus(ByVal v_lRiskCnt As Object, ByVal v_lRiskStatusID As Object, ByVal v_sRiskStatusCode As Object) As Integer

        Dim sMessage As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue
            sMessage = ""

            If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to begin SQL Transaction"
                Throw New Exception(sMessage)
            End If


            'get risk status id
            If v_sRiskStatusCode <> "" Then
                If GetRiskStatus(r_lRiskStatusID:=v_lRiskStatusID, r_sRiskStatusCode:=v_sRiskStatusCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = "Failed to get Risk Status ID (Risk Status Code: " & v_sRiskStatusCode & ")"
                    Throw New Exception(sMessage)
                End If
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_status_id", vValue:=v_lRiskStatusID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add risk_status_id param"
                Throw New Exception(sMessage)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add risk_cnt param"
                Throw New Exception(sMessage)
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdRiskStatusSQL, sSQLName:=ACUpdRiskStatusName, bStoredProcedure:=ACUpdRiskStatusStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed SQLAction risk_status_id: " & v_lRiskStatusID & " risk_cnt: " & CStr(v_lRiskCnt)
                Throw New Exception(sMessage)
            End If

            If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to commit to database risk_status_id: " & v_lRiskStatusID & " risk_cnt: " & CStr(v_lRiskCnt)
                Throw New Exception(sMessage)
            End If


        Catch ex As Exception

            m_lReturn = m_oDatabase.SQLRollbackTrans()

            lReturnValue = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed UpdateRiskStatus()", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally


        End Try
        Return lReturnValue
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRiskStatus
    '
    ' Description: get risk status code and or id
    '
    ' History: 01/03/2004 Thinh Nguyen - Created.
    '
    ' ***************************************************************** '
    Public Function GetRiskStatus(ByRef r_lRiskStatusID As Integer, ByRef r_sRiskStatusCode As String) As Integer

        Dim sMessage As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim vResultArray(,) As Object = Nothing

        Try
            sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue

            If r_lRiskStatusID = 0 And r_sRiskStatusCode = "" Then
                sMessage = "Must supply either RiskStatusID or RiskStatusCode"
                Return lReturnValue
            End If

            m_oDatabase.Parameters.Clear()


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add("RiskStatusID", If(r_lRiskStatusID = 0, DBNull.Value, CStr(r_lRiskStatusID)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add RiskStatusID param"
                Return lReturnValue
            End If


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add("RiskStatusCode", If(r_sRiskStatusCode = "", DBNull.Value, r_sRiskStatusCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add RiskStatusCode param"
                Return lReturnValue
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskStatusSQL, sSQLName:=ACGetRiskStatusName, bStoredProcedure:=ACGetRiskStatusStored, vResultArray:=vResultArray, bKeepNulls:=True)
            If Informations.IsArray(vResultArray) Then
                r_lRiskStatusID = gPMFunctions.NullToLong(vResultArray(0, 0))
                r_sRiskStatusCode = gPMFunctions.NullToString(vResultArray(1, 0))
            End If


        Catch ex As Exception
            lReturnValue = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed GetRiskStatus()", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally


        End Try
        Return lReturnValue
    End Function

    ' Name: GetRiskAllStatuses (Public)
    '
    ' Desc: get all associate risks for policy
    '
    ' ***************************************************************** '
    Public Function GetRiskAllStatuses(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return m_oDatabase.SQLSelect(sSQL:=ACGetRiskAllStatusesSQL, sSQLName:=ACGetRiskAllStatusesName, bStoredProcedure:=ACGetRiskAllStatusesStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskAllStatuses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskAllStatuses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteInsuranceFileRiskLink
    '
    ' Description:
    '
    ' History: 25/08/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteInsuranceFileRiskLink(ByVal v_lInsuranceFileCnt As Integer) As Integer
        Return DeleteInsuranceFileRiskLink(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=0)
    End Function
    Public Function DeleteInsuranceFileRiskLink(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_lRiskCnt > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteInsuranceFileRiskLinkDetailsSQL, sSQLName:=ACDeleteInsuranceFileRiskLinkDetailsName, bStoredProcedure:=ACDeleteInsuranceFileRiskLinkDetailsStored)
            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteInsuranceFileRiskLinkDetails2SQL, sSQLName:=ACDeleteInsuranceFileRiskLinkDetails2Name, bStoredProcedure:=ACDeleteInsuranceFileRiskLinkDetails2Stored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteInsuranceFileRiskLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteInsuranceFileRiskLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name:         AddRiskRenewalLink
    '
    ' Description:  Adds the renewal_original_risk_cnt to the
    '               insurance_file_risk_link record, for renewals.
    '               Note: the risk_cnt is stored in renewal_original_risk_cnt
    '               as it is not certain whether the
    '               insurance_file_risk_link.original_risk_cnt
    '               can be used to store the risk_cnt without breaking
    '               anything else.
    '
    ' History:      AMB 26/06/2003: 1.9 IAG PS068 Date Effective
    '               Rating - created
    ' ***************************************************************** '
    Public Function AddRiskRenewalLink(ByVal v_lRiskCnt As Integer, ByVal v_lRenewalOriginalRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_original_risk_cnt", vValue:=CStr(v_lRenewalOriginalRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRiskRenewalLinkSQL, sSQLName:=ACAddRiskRenewalLinkName, bStoredProcedure:=ACAddRiskRenewalLinkStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRiskRenewalLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRiskRenewalLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: CopyRiskExtras
    '
    ' Description:
    '
    ' History: 20/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Function CopyRiskExtras(ByVal v_lOldRiskCnt As Integer, ByVal v_lNewRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="old_risk_cnt", vValue:=CStr(v_lOldRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_risk_cnt", vValue:=CStr(v_lNewRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyRiskExtrasSQL, sSQLName:=ACCopyRiskExtrasName, bStoredProcedure:=ACCopyRiskExtrasStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskExtras Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskExtras", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetUncopiedRisks
    '
    ' Description: Gets The Risks with status = 'U' from Insurance_file_Risk_link
    '
    ' History: 10/07/2006 Roopaly
    '
    ' ***************************************************************** '


    Public Function GetUncopiedRisks(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_oDatabase.SQLSelect(sSQL:=ACGetAllUnCopiedRiskSQL, sSQLName:=ACGetAllUnCopiedRiskName, bStoredProcedure:=ACGetAllUnCopiedRiskStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUnCopiedRisks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnCopiedRisks", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: CopyRiskFolder
    '
    ' Description: JPG created for PN# 71788 (21/05/2010)
    '
    ' History: 21/05/2010 Jai Prakash - Created.
    '
    ' ***************************************************************** '
    Public Function CopyRiskFolder(
        ByVal v_lRisk_folder_cnt As Long,
        ByVal v_lInsuranceFileCnt As Long,
        ByRef r_lNew_risk_folder_cnt As Long) As Long


        Const kMethodName As String = "CopyRiskFolder"
        Dim iResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try


            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_cnt", vValue:=CStr(v_lRisk_folder_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_risk_folder_cnt", vValue:=CStr(r_lNew_risk_folder_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyRiskFolderSQL, sSQLName:=ACCopyRiskFolderName, bStoredProcedure:=ACCopyRiskFolderStored)

            r_lNew_risk_folder_cnt = m_oDatabase.Parameters.Item("new_risk_folder_cnt").Value


        Catch excep As Exception
            iResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

        Return iResult

    End Function
    ''' <summary>
    ''' Get all associate risks for policy for renewal selection
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="r_vResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRenewalRisk(ByVal v_lInsuranceFileCnt As Integer,
                                    ByRef r_vResultArray(,) As Object) As Integer


        Try

            GetRenewalRisk = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                                                            vValue:=v_lInsuranceFileCnt,
                                                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                                            iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GetRenewalRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            GetRenewalRisk = m_oDatabase.SQLSelect(sSQL:=kSaaRenRiskSQL,
                                                                sSQLName:=kSaaRenRiskName,
                                                                bStoredProcedure:=kSaaRenRiskStored,
                                                                vResultArray:=r_vResultArray,
                                                           bKeepNulls:=True)

            Exit Function

        Catch ex As Exception

            GetRenewalRisk = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewalRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            Exit Function

        End Try
    End Function



    ' **************************************************************************************************
    ' return RI Model type
    ' 0 = standard
    ' 1 = default
    ' 2 = deferred
    ' 3 = exess of loss
    ' -1 = if error
    ' **************************************************************************************************
    Public Function GetRIModelTypeByRisk(ByVal v_lRiskCnt As Integer) As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim result As Integer = 0
        Dim lReturn As Integer = gPMConstants.PMEReturnCode.PMTrue


        Dim sMessage As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we were given an ri model id add it, else select all
            If v_lRiskCnt > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "RiskCnt", v_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            Else
                m_oDatabase.Parameters.Clear()
            End If
            ' Call sql
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRIModelTypeSQL, sSQLName:=ACGetRIModelTypeName, bStoredProcedure:=ACGetRIModelTypeStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get ri model type")
            End If
            If Information.IsArray(vResultArray) Then
                result = CInt(vResultArray(1, 0))
            Else
                sMessage = "Failed to find RI Model type"
                Throw New Exception()
            End If


            Return result

        Catch excep As System.Exception

            result = -1

            If sMessage = "" Then
                sMessage = "Failed to get RI Model type"
            End If

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRIModelType()", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try

    End Function


End Class
