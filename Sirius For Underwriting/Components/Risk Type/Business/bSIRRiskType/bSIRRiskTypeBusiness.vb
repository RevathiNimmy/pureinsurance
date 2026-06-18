Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Text
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 20/07/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRProduct.
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

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Calling Application Name

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

    'JMK 22/10/2001 - Underwriting hidden option
    Private m_sUnderwritingType As String = ""

    '---------------------------------------------------------------------------------------
    ' Procedure : GetRIModelUsageDeferredRI
    ' DateTime  : 15 Sep 03 14:51
    ' Author    : AMB
    ' Purpose   : Gets the deferred RI models for a risk type
    '---------------------------------------------------------------------------------------
    Public Function GetRIModelUsageDeferredRI(ByVal v_lRiskTypeID As Integer, ByRef r_vRIModelUsageDeferredRI(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_type_id", v_lRiskTypeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_deferred", gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelRiskTypeRIModelUsageSQL, sSQLName:=ACSelRiskTypeRIModelUsageName, bStoredProcedure:=ACSelRiskTypeRIModelUsageStored, vResultArray:=r_vRIModelUsageDeferredRI)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRIModelUsageDeferredRI")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRIModelUsageDeferredRI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRIModelUsageDeferredRI", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
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

    ' JMK 22/10/2001 "A" for Underwriting Agency and "U" for Reinsurance
    Public ReadOnly Property UnderwritingType() As String
        Get

            If m_sUnderwritingType = "" Then
                m_lReturn = getUnderwritingType()
            End If

            Return m_sUnderwritingType

        End Get
    End Property


    ' ***************************************************************** '
    ' Name: GetUnderwritingType
    '
    ' Description:  Finds out Underwriting type - U or A
    '               For labelling: A - Insurer. U - Reinsurer
    '
    ' JMK 22/10/2001    Created
    ' SJP14062002 - getUnderwritingType uses new product options scheme
    ' ***************************************************************** '
    Private Function getUnderwritingType() As Integer

        Dim result As Integer = 0



        Return bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingType)

    End Function

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



    ''' <summary>
    ''' get Risk Type details for a Risk Type ID
    ''' </summary>
    ''' <param name="v_lRiskTypeID"></param>
    ''' <param name="r_vResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRiskTypeDetails(ByVal v_lRiskTypeID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id",
                                                   vValue:=CStr(v_lRiskTypeID),
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMLong)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelRiskTypeSQL,
                                              sSQLName:=ACSelRiskTypeName,
                                              bStoredProcedure:=ACSelRiskTypeStored,
                                              lNumberRecords:=gPMConstants.PMAllRecords,
                                              vResultArray:=r_vResultArray)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                nResult = PMEReturnCode.PMFalse
            End If
            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername,
                               iType:=PMELogLevel.PMLogOnError,
                               sMsg:="GetRiskTypeDetails Failed",
                               vApp:=ACApp, vClass:=ACClass,
                               vMethod:="GetRiskTypeDetails",
                               vErrNo:=Information.Err().Number,
                               vErrDesc:=excep.Message)
            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllRiskTypeGroup
    '
    ' Description: get all risk type groups
    '
    ' ***************************************************************** '
    Public Function GetAllRiskTypeGroup(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()



            Return m_oDatabase.SQLSelect(sSQL:=ACSaaRiskTypeGroupSQL, sSQLName:=ACSaaRiskTypeGroupName, bStoredProcedure:=ACSaaRiskTypeGroupStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllRiskTypeGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllRiskTypeGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRiskTypeGroup
    '
    ' Description: get risk type groups for a risk type id
    '
    ' ***************************************************************** '
    Public Function GetRiskTypeGroup(ByVal v_lRiskTypeID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelRiskTypeGroupSQL, sSQLName:=ACSelRiskTypeGroupName, bStoredProcedure:=ACSelRiskTypeGroupStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskTypeGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllowedGISScreen
    '
    ' Description: get all GIS_Screen for a risk type id
    '
    ' ***************************************************************** '
    Public Function GetAllowedGISScreen(ByVal v_lRiskTypeID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelRiskTypeGISScreenSQL, sSQLName:=ACSelRiskTypeGISScreenName, bStoredProcedure:=ACSelRiskTypeGISScreenStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllowedGISScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllowedGISScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllGISScreen
    '
    ' Description: get all GIS Screen groups
    '
    ' ***************************************************************** '
    Public Function GetAllGISScreen(ByRef r_vResultArray(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSaaGISScreenSQL, sSQLName:=ACSaaGISScreenName, bStoredProcedure:=ACSaaGISScreenStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllGISScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllGISScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetClauses
    '
    ' Description: get all clauses for a risk type id
    '
    ' ***************************************************************** '
    Public Function GetClauses(ByVal v_lRiskTypeID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelClausesSQL, sSQLName:=ACSelClausesName, bStoredProcedure:=ACSelClausesStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClauses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClauses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' GetAllRiskType
    ''' </summary>
    ''' <param name="r_vResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllRiskType(ByRef r_vResultArray(,) As Object) As Integer

        Dim nResult As Integer = 0
        Try

            nResult = PMEReturnCode.PMTrue

            ' Execute SQL Statement - use array for speed

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSaaRiskTypeSQL, sSQLName:=ACSaaRiskTypeName, bStoredProcedure:=ACSaaRiskTypeStored, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetAllRiskType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllRiskType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateRiskType (Public)
    '
    ' Description: Update Risk Type details for a risk_type id
    '
    ' ***************************************************************** '
    Public Function UpdateRiskType(ByVal v_iTask As Integer, ByRef r_lRiskTypeID As Integer, ByVal v_sCode As String, ByVal v_vDescription As String, ByVal v_dtEffectiveDate As Date, ByVal v_vShareWithCoInsurer As Object, ByVal v_vShareWithReInsurer As Object, ByVal v_vSuppressPublicText As Object, ByVal v_vSuppressPrivateText As Object, ByVal v_vSuppressTaxes As Object, ByVal v_vReportPointer As Object, ByVal v_vSectionMask As Object, ByVal v_vStampDutyRate1 As Object, ByVal v_vStampDutyRate2 As Object, ByVal v_vPrimarySort As Object, ByVal v_vSecondarySort As Object, ByVal v_vHeaderClause As Object, ByVal v_vTrailerClause As Object, ByVal v_vIsRiAtRiskLevel As Object, ByVal v_vIsAutoReinsured As Object, ByVal v_vHeaderClauseId As Object, ByVal v_vTrailerClauseId As Object, ByVal v_vAccumulationLevel As Object, ByVal v_vGISScreenId As Object, ByVal v_vClauses As Object, ByRef r_vLinkedRiskTypeGroup As Object, Optional ByVal v_vIsDeferredRIPermitted As gPMConstants.PMEReturnCode = 0, Optional ByVal v_vClaimsIsPostTaxes As Object = Nothing, Optional ByVal v_vDisplayReinsurance As Object = Nothing, Optional ByVal v_vAllowRatingSectionAdd As Object = Nothing, Optional ByVal v_vAllowRatingSectionEdit As Object = Nothing, Optional ByVal v_vAllowRatingSectionDelete As Object = Nothing, Optional ByVal v_vAllowEditRatingSectionRateType As Object = Nothing, Optional ByVal v_vAllowEditRatingSectionRate As Object = Nothing, Optional ByVal v_vAllowEditRatingSectionSumInsured As Object = Nothing, Optional ByVal v_vAllowEditRatingSectionThisPremium As Object = Nothing, Optional ByVal v_vDisplayClaimReinsurance As Object = Nothing, Optional ByVal v_lClaimsTypeBasis As Long = 0, Optional ByVal v_lClaimsCoverBasis As Long = 0, Optional ByVal oAttachClaimOutsideOfPolicyPeriod As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        ' AMB 21/05/2003: 1.8.6 ICB Deferred RI RFC - r_vIsDeferredRIPermitted added

        Dim result As Integer = 0

        'below variables use to default not passed in fields
        Dim oRiskFolderTypeID As Object
        Dim lCaptionID As Integer
        Dim iIsDeleted As gPMConstants.PMEReturnCode
        Dim oVarDataStructureID As Object
        Dim oInterfaceObjectName As Object
        Dim oInterfaceClassName As Object
        Dim oOverridePerilRIBand As Object
        Dim oOverridePerilXLBand As Object
        Dim oNBPremiumProRataTypeID As Object
        Dim oMTAPremiumProRataTypeID As Object
        Dim oRNPremiumProRataTypeID As Object
        ' AMB 21/05/2003: 1.8.6 ICB Deferred RI RFC
        Dim vIsDeferredRIPermitted As gPMConstants.PMEReturnCode
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'initialised not passed fields
            iIsDeleted = gPMConstants.PMEReturnCode.PMFalse

            oRiskFolderTypeID = DBNull.Value

            oVarDataStructureID = DBNull.Value

            oInterfaceObjectName = DBNull.Value

            oInterfaceClassName = DBNull.Value

            oOverridePerilRIBand = DBNull.Value

            oOverridePerilXLBand = DBNull.Value

            oNBPremiumProRataTypeID = DBNull.Value

            oMTAPremiumProRataTypeID = DBNull.Value
            oRNPremiumProRataTypeID = DBNull.Value
            ' AMB 21/05/2003: 1.8.6 ICB Deferred RI RFC
            If Information.IsNothing(v_vIsDeferredRIPermitted) Then
                vIsDeferredRIPermitted = gPMConstants.PMEReturnCode.PMFalse
            Else
                vIsDeferredRIPermitted = v_vIsDeferredRIPermitted
            End If

            'get caption id
            m_lReturn = CType(GetCaptionID(v_vDescription, lCaptionID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'update Risk Type

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'update Risk Type details (there should only be one record)
            With m_oDatabase
                .Parameters.Clear()

                'Add risk type ID
                If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                    m_lReturn = .Parameters.Add(sName:="risk_type_id", vValue:=CStr(r_lRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else
                    m_lReturn = .Parameters.Add(sName:="risk_type_id", vValue:=CStr(r_lRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                'Add risk folder type ID


                m_lReturn = .Parameters.Add(sName:="risk_folder_type_ID", vValue:=oRiskFolderTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add Caption ID

                m_lReturn = .Parameters.Add(sName:="Caption_ID", vValue:=lCaptionID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                'Add Code
                m_lReturn = .Parameters.Add(sName:="Code", vValue:=v_sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                'Add Description
                m_lReturn = .Parameters.Add(sName:="Description", vValue:=v_vDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                'Add Effective Date

                m_lReturn = .Parameters.Add(sName:="Effective_Date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                'Add Is_Deleted
                m_lReturn = .Parameters.Add(sName:="Is_Deleted", vValue:=iIsDeleted, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                'Add var data structure id

                m_lReturn = .Parameters.Add(sName:="var_data_structure_id", vValue:=oVarDataStructureID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add interface object name

                m_lReturn = .Parameters.Add(sName:="interface_object_name", vValue:=oInterfaceObjectName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                'Add interface class name


                m_lReturn = .Parameters.Add(sName:="interface_class_name", vValue:=oInterfaceClassName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                'Add override peril ri band


                m_lReturn = .Parameters.Add(sName:="override_peril_ri_band", vValue:=oOverridePerilRIBand, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                'Add override peril xl band


                m_lReturn = .Parameters.Add(sName:="override_peril_xl_band", vValue:=oOverridePerilXLBand, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                'Add nb premium pro rata type id


                m_lReturn = .Parameters.Add(sName:="nb_premium_pro_rata_type_id", vValue:=oNBPremiumProRataTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add mta premium pro rata type id


                m_lReturn = .Parameters.Add(sName:="mta_premium_pro_rata_type_id", vValue:=oMTAPremiumProRataTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add rn premium pro rata type id


                m_lReturn = .Parameters.Add(sName:="rn_premium_pro_rata_type_id", vValue:=oRNPremiumProRataTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add is share with co insurers


                m_lReturn = .Parameters.Add(sName:="is_share_with_co_insurers", vValue:=v_vShareWithCoInsurer, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                'Add is share with re insurers


                m_lReturn = .Parameters.Add(sName:="is_share_with_re_insurers", vValue:=v_vShareWithReInsurer, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                'Add is suppress public text


                m_lReturn = .Parameters.Add(sName:="is_suppress_public_text", vValue:=v_vSuppressPublicText, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                'Add is suppress private text


                m_lReturn = .Parameters.Add(sName:="is_suppress_private_text", vValue:=v_vSuppressPrivateText, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                'Add is suppress taxes


                m_lReturn = .Parameters.Add(sName:="is_suppress_taxes", vValue:=v_vSuppressTaxes, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                'Add report pointer


                m_lReturn = .Parameters.Add(sName:="report_pointer", vValue:=v_vReportPointer, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add section mask


                m_lReturn = .Parameters.Add(sName:="section_mask", vValue:=v_vSectionMask, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add stamp duty rate1


                m_lReturn = .Parameters.Add(sName:="stamp_duty_rate1", vValue:=v_vStampDutyRate1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                'Add stamp duty rate2


                m_lReturn = .Parameters.Add(sName:="stamp_duty_rate2", vValue:=v_vStampDutyRate2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                'Add primary sort


                m_lReturn = .Parameters.Add(sName:="primary_sort", vValue:=v_vPrimarySort, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                'Add secondary sort


                m_lReturn = .Parameters.Add(sName:="secondary_sort", vValue:=v_vSecondarySort, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                'Add header clause


                m_lReturn = .Parameters.Add(sName:="header_clause", vValue:=v_vHeaderClause, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                'Add trailer clause


                m_lReturn = .Parameters.Add(sName:="trailer_clause", vValue:=v_vTrailerClause, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                'Add is ri at risk level


                m_lReturn = .Parameters.Add(sName:="is_ri_at_risk_level", vValue:=v_vIsRiAtRiskLevel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add is auto reinsured


                m_lReturn = .Parameters.Add(sName:="is_auto_reinsured", vValue:=v_vIsAutoReinsured, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' AMB 21/05/2003: 1.8.6 ICB Deferred RI RFC - add 'is deferred RI permitted'

                m_lReturn = .Parameters.Add(sName:="is_deferred_RI_permitted", vValue:=vIsDeferredRIPermitted, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add header clause id


                m_lReturn = .Parameters.Add(sName:="header_clause_id", vValue:=v_vHeaderClauseId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add trailer clause id


                m_lReturn = .Parameters.Add(sName:="trailer_clause_id", vValue:=v_vTrailerClauseId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add accumulation level


                m_lReturn = .Parameters.Add(sName:="accumulation_level", vValue:=v_vAccumulationLevel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add GIS screen id


                m_lReturn = .Parameters.Add(sName:="gis_screen_id", vValue:=v_vGISScreenId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Add Claims post taxes flag
                m_lReturn = .Parameters.Add(sName:="claims_is_post_taxes", vValue:=gPMFunctions.ToSafeInteger(v_vClaimsIsPostTaxes), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                'Add Flag to Display Reinsurance Screen
                m_lReturn = .Parameters.Add(sName:="display_reinsurance_screen", vValue:=gPMFunctions.ToSafeInteger(v_vDisplayReinsurance), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .Parameters.Add(sName:="allow_add_ratingsection", vValue:=Math.Abs(CInt(gPMFunctions.ToSafeBoolean(v_vAllowRatingSectionAdd))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .Parameters.Add(sName:="allow_edit_ratingsection", vValue:=Math.Abs(CInt(gPMFunctions.ToSafeBoolean(v_vAllowRatingSectionEdit))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .Parameters.Add(sName:="allow_delete_ratingsection", vValue:=Math.Abs(CInt(gPMFunctions.ToSafeBoolean(v_vAllowRatingSectionDelete))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .Parameters.Add(sName:="allow_edit_ratingsection_ratetype", vValue:=Math.Abs(CInt(gPMFunctions.ToSafeBoolean(v_vAllowEditRatingSectionRateType))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .Parameters.Add(sName:="allow_edit_ratingsection_rate", vValue:=Math.Abs(CInt(gPMFunctions.ToSafeBoolean(v_vAllowEditRatingSectionRate))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .Parameters.Add(sName:="allow_edit_ratingsection_suminsured", vValue:=Math.Abs(CInt(gPMFunctions.ToSafeBoolean(v_vAllowEditRatingSectionSumInsured))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .Parameters.Add(sName:="allow_edit_ratingsection_thispremium", vValue:=Math.Abs(CInt(gPMFunctions.ToSafeBoolean(v_vAllowEditRatingSectionThisPremium))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .Parameters.Add(sName:="Display_Claims_Reinsurance_screen", vValue:=gPMFunctions.ToSafeInteger(v_vDisplayClaimReinsurance), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .Parameters.Add(sName:="Claims_type_basis_ID", vValue:=v_lClaimsTypeBasis, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .Parameters.Add(sName:="Claims_Cover_basis_ID", vValue:=v_lClaimsCoverBasis, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .Parameters.Add(sName:="bAttach_Claim_Outside_Of_Policy_Period", vValue:=CStr(Math.Abs(CInt(gPMFunctions.ToSafeBoolean(oAttachClaimOutsideOfPolicyPeriod)))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRiskTypeSQL, sSQLName:=ACAddRiskTypeName, bStoredProcedure:=ACAddRiskTypeStored)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    'get new risk_type_id back to display on the parent listview
                    r_lRiskTypeID = m_oDatabase.Parameters.Item("risk_type_id").Value
                End If
            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdRiskTypeSQL, sSQLName:=ACUpdRiskTypeName, bStoredProcedure:=ACUpdRiskTypeStored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(LinkedRiskTypeGroupUpd(v_lRiskTypeID:=r_lRiskTypeID, r_vLinkedRiskTypeGroup:=r_vLinkedRiskTypeGroup, sUniqueId:=sUniqueId, sScreenHierarchy:=sScreenHierarchy), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskType  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskType ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DelRiskType
    '
    ' Description: set is_deleted to v_iIsDeleted
    '
    ' ***************************************************************** '
    Public Function DelRiskType(ByVal v_lRiskTypeID As Integer, ByVal v_iIsDeleted As Integer, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = "UPDATE Risk_Type SET is_deleted = " & v_iIsDeleted
            sSQL = sSQL & ", UserId = " & m_iUserID
            sSQL = sSQL & ", UniqueId = '" & CStr(sUniqueId) & "'"
            sSQL = sSQL & ", ScreenHierarchy = '" & CStr(sScreenHierarchy) & "'"
            sSQL = sSQL & " WHERE risk_type_id = " & CStr(v_lRiskTypeID)

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateIsDeleted", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelRiskType  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelRiskType ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DelRiskType
    '
    ' Description: set is_deleted to v_iIsDeleted
    '
    ' ***************************************************************** '
    Public Function UpdateRiskTypeRuleSet(ByVal v_iTask As Integer,
                                          ByRef r_lRiskTypeRuleSetID As Integer,
                                          ByVal v_sCode As String,
                                          ByVal v_vDescription As String,
                                          ByVal v_dtEffectiveDate As Date,
                                          ByVal v_vRiskTypeID As Object,
                                          ByVal v_vFileName As Object,
                                          ByVal v_vLive As Object,
                                          ByVal v_vType As Object,
                                          Optional ByVal v_lRiskTypeRuleSetTypeID As Integer = 0,
                                          Optional ByVal v_sDREExecutorURL As String = "",
                                          Optional ByVal v_sDREDefaultToken As String = "",
                                          Optional ByVal v_bDREDefault As Boolean = False,
                                          Optional ByVal v_bDREQuote As Boolean = False,
                                          Optional ByVal v_bDREValidate As Boolean = False,
                                          Optional ByVal v_bPostDREVB As Boolean = False,
                                          Optional ByVal v_bPrePre As Boolean = False,
                                          Optional ByVal v_lPREVersion As String = "",
                                          Optional ByVal v_lPRERulesetEffectiveDate As String = "",
                                          Optional ByVal v_bUseChildRuleSetEffDate As Boolean = False, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim lCaptionID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get caption id
            m_lReturn = GetCaptionID(v_vDescription, lCaptionID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = BeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'update Risk Type Rule Set details (there should only be one record)
            m_oDatabase.Parameters.Clear()

            'Add risk type ID
            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_rule_set_id", vValue:=CStr(r_lRiskTypeRuleSetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_rule_set_id", vValue:=CStr(r_lRiskTypeRuleSetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=lCaptionID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=v_vDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", vValue:=gPMConstants.PMEReturnCode.PMFalse, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'start
            If v_vRiskTypeID Is Nothing Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_vRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
            'end

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="file_name", vValue:=v_vFileName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="live", vValue:=v_vLive, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="type", vValue:=v_vType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_lReturn = RollbackTrans()
                UpdateRiskTypeRuleSet = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_rule_set_type_id", vValue:=v_lRiskTypeRuleSetTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_lReturn = RollbackTrans()
                UpdateRiskTypeRuleSet = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="dre_executor_url", vValue:=v_sDREExecutorURL, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_lReturn = RollbackTrans()
                UpdateRiskTypeRuleSet = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="dre_default_token", vValue:=v_sDREDefaultToken, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_lReturn = RollbackTrans()
                UpdateRiskTypeRuleSet = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="dre_default", vValue:=v_bDREDefault, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_lReturn = RollbackTrans()
                UpdateRiskTypeRuleSet = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="dre_quote", vValue:=v_bDREQuote, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_lReturn = RollbackTrans()
                UpdateRiskTypeRuleSet = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="dre_validate", vValue:=v_bDREValidate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_lReturn = RollbackTrans()
                UpdateRiskTypeRuleSet = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="post_dre_vb", vValue:=v_bPostDREVB, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_lReturn = RollbackTrans()
                UpdateRiskTypeRuleSet = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pre_pre_rule", vValue:=v_bPrePre, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_lReturn = RollbackTrans()
                UpdateRiskTypeRuleSet = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If (v_lRiskTypeRuleSetTypeID = 2) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="pre_version", vValue:=v_lPREVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    m_lReturn = RollbackTrans()
                    UpdateRiskTypeRuleSet = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="pre_ruleset_effective_date", vValue:=v_lPRERulesetEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    m_lReturn = RollbackTrans()
                    UpdateRiskTypeRuleSet = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="pre_child_ruleset_effectivedate", vValue:=v_bUseChildRuleSetEffDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    m_lReturn = RollbackTrans()
                    UpdateRiskTypeRuleSet = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRiskTypeRuleSetSQL, sSQLName:=ACAddRiskTypeRuleSetName, bStoredProcedure:=ACAddRiskTypeRuleSetStored)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    'get new risk_type_rule_set_id back to display on parent's listview
                    r_lRiskTypeRuleSetID = m_oDatabase.Parameters.Item("risk_type_rule_set_id").Value
                End If
            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdRiskTypeRuleSetSQL, sSQLName:=ACUpdRiskTypeRuleSetName, bStoredProcedure:=ACUpdRiskTypeRuleSetStored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return CommitTrans()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskTypeRuleSet  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskTypeRuleSet ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DelRiskTypeRuleSet
    '
    ' Description: set is_deleted to v_iIsDeleted
    '
    ' ***************************************************************** '
    Public Function DelRiskTypeRuleSet(ByVal v_lRiskTypeID As Integer, ByVal v_lRiskTypeRuleSetID As Integer, ByVal v_iIsDeleted As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = "UPDATE Risk_Type_Rule_Set SET is_deleted = " & v_iIsDeleted
            sSQL = sSQL & ", UserId = " & m_iUserID
            sSQL = sSQL & ", UniqueId = '" & CStr(v_sUniqueId) & "'"
            sSQL = sSQL & ", ScreenHierarchy = '" & CStr(v_sScreenHierarchy) & "'"
            sSQL = sSQL & " WHERE risk_type_id = " & CStr(v_lRiskTypeID)
            sSQL = sSQL & " AND risk_type_rule_set_id = " & CStr(v_lRiskTypeRuleSetID)

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateIsDeleted", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelRiskTypeRuleSet  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelRiskTypeRuleSet ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRiskTypeRuleSet
    '
    ' Description: Get risk type rule set details
    '
    ' ***************************************************************** '
    Public Function GetRiskTypeRuleSet(ByVal v_lRiskTypeRuleSetID As Integer, ByVal v_vRiskTypeID As Object, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_rule_set_id", vValue:=CStr(v_lRiskTypeRuleSetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Added check for nothing
            'start
            If v_vRiskTypeID Is Nothing Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_vRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
            'end

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return m_oDatabase.SQLSelect(sSQL:=ACSelRiskTypeRuleSetSQL, sSQLName:=ACSelRiskTypeRuleSetName, bStoredProcedure:=ACSelRiskTypeRuleSetStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskTypeRuleSet  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeRuleSet ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllRiskTypeRuleSet
    '
    ' Description: Get all rules for risk type id
    '
    ' ***************************************************************** '
    Public Function GetAllRiskTypeRuleSet(ByVal v_lRiskTypeID As Integer, ByVal v_sType As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="type", vValue:=v_sType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return m_oDatabase.SQLSelect(sSQL:=ACSaaRiskTypeRuleSetSQL, sSQLName:=ACSaaRiskTypeRuleSetName, bStoredProcedure:=ACSaaRiskTypeRuleSetStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllRiskTypeRuleSet  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllRiskTypeRuleSet ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Public Methods (End)


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

    ' ***************************************************************** '
    ' Name: GetCaptionID(Private)
    '
    ' Description: get/create captionID associate with a caption
    '
    ' ***************************************************************** '
    Private Function GetCaptionID(ByVal v_sCaption As String, ByRef r_lCaptionID As Integer) As Integer

        Dim result As Integer = 0
        Dim oArchDatabase As dPMDAO.Database




        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oArchDatabase), gPMConstants.PMEReturnCode)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'begin transaction
        m_lReturn = oArchDatabase.SQLBeginTrans()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oArchDatabase.Parameters.Clear()

        m_lReturn = oArchDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = oArchDatabase.SQLRollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = oArchDatabase.Parameters.Add(sName:="caption", vValue:=v_sCaption, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = oArchDatabase.SQLRollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = oArchDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = oArchDatabase.SQLRollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = oArchDatabase.SQLAction(sSQL:=ACAddCaptionIDSQL, sSQLName:=ACAddCaptionIDName, bStoredProcedure:=ACAddCaptionIDStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = oArchDatabase.SQLRollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'commit transaction
        m_lReturn = oArchDatabase.SQLCommitTrans()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        'Get returned caption_id to pass into AddScheme proc.
        r_lCaptionID = oArchDatabase.Parameters.Item("caption_id").Value

        oArchDatabase.CloseDatabase()
        oArchDatabase = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AllowedGISScreenUpd(Private)
    '
    ' Description: delete all allowed GISScreen which are linked to supplied risk type id
    '                    add allowed risk type gis screen for supplied risk type id
    '                     parent function will control transaction mode.
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AllowedGISScreenUpd) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AllowedGISScreenUpd(ByVal v_lRiskTypeID As Integer, ByRef r_vAllowedGISScreen(,) As Object) As Integer
    '
    'Dim result As Integer = 0
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'delete all Allowed GIS Screen for risk type id
    'm_oDatabase.Parameters.Clear()
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelRiskTypeGISScreenSQL, sSQLName:=ACDelRiskTypeGISScreenName, bStoredProcedure:=ACDelRiskTypeGISScreenStored)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'update allowed GIS Screen for risk type id
    'If Information.IsArray(r_vAllowedGISScreen) Then
    '
    'add all GIS Screen for risk type id

    'For 'lCount As Integer = r_vAllowedGISScreen.GetLowerBound(1) To r_vAllowedGISScreen.GetUpperBound(1)
    'm_oDatabase.Parameters.Clear()
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = m_oDatabase.Parameters.Add(sName:="gis_screen_id", vValue:=CStr(CInt(r_vAllowedGISScreen(0, lCount))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRiskTypeGISScreenSQL, sSQLName:=ACAddRiskTypeGISScreenName, bStoredProcedure:=ACAddRiskTypeGISScreenStored)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Next lCount
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllowedGISScreenUpd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AllowedGISScreenUpd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: ClausesUpd(Private)
    '
    ' Description: delete all allowed GISScreen which are linked to supplied risk type id
    '                    add allowed risk type gis screen for supplied risk type id
    '                     parent function will control transaction mode.
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ClausesUpd) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ClausesUpd(ByVal v_lRiskTypeID As Integer, ByRef r_vClauses( ,  ) As Object) As Integer
    '
    'Dim result As Integer = 0
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'delete all Clauses for risk type id
    'm_oDatabase.Parameters.Clear()
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelClausesSQL, sSQLName:=ACDelClausesName, bStoredProcedure:=ACDelClausesStored)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'update Clauses for risk type id
    'If Information.IsArray(r_vClauses) Then
    '
    'add all GIS Screen for risk type id
    'For 'lCount As Integer = r_vClauses.GetLowerBound(1) To r_vClauses.GetUpperBound(1)

    'If CDbl(r_vClauses(3, lCount)) = 1 Then
    'm_oDatabase.Parameters.Clear()
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(CInt(r_vClauses(0, lCount))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddClausesSQL, sSQLName:=ACAddClausesName, bStoredProcedure:=ACAddClausesStored)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    'Next lCount
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClausesUpd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClausesUpd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: LinkedRiskTypeGroupUpd(Private)
    '
    ' Description: delete all risk type group which are linked to supplied risk type id
    '                    add linked risk type group for supplied risk type id
    '                     parent function will control transaction mode.
    ' ***************************************************************** '
    Private Function LinkedRiskTypeGroupUpd(ByVal v_lRiskTypeID As Integer, ByRef r_vLinkedRiskTypeGroup(,) As Object, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        'delete all linked risk type group for risk type id
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
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

        m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelRiskTypeUsageSQL, sSQLName:=ACDelRiskTypeUsageName, bStoredProcedure:=ACDelRiskTypeUsageStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'update risk type group for risk type id
        If Information.IsArray(r_vLinkedRiskTypeGroup) Then

            'add all linked risk type group for risk type id

            For lCount As Integer = r_vLinkedRiskTypeGroup.GetLowerBound(1) To r_vLinkedRiskTypeGroup.GetUpperBound(1)
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRiskTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_group_id", vValue:=CStr(CInt(r_vLinkedRiskTypeGroup(0, lCount))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRiskTypeUsageSQL, sSQLName:=ACAddRiskTypeUsageName, bStoredProcedure:=ACAddRiskTypeUsageStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lCount
        End If

        Return result

    End Function

    ' private Methods (End)


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



    'QBENZ022
    Public Function PickListLoad(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            With m_oDatabase
                .Parameters.Clear()

                'Load the parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)



                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=CType(CInt(vFKArray(2, iParam)), gPMConstants.PMEDataType))
                Next iParam

                'Call the SP

                m_lReturn = .SQLSelect("spu_sir_Rating_SectionsTypes_Sel", sPickListType & " PickList Load", True, gPMConstants.PMAllRecords, vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return m_lReturn
                End If
            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys() As Object) As Integer

        Dim result As Integer = 0

        Try

            BeginTrans()

            If vFKArray.GetUpperBound(1) > 2 And sPickListType.Trim().ToUpper() = "" Then
                ReDim Preserve vFKArray(vFKArray.GetUpperBound(0), vFKArray.GetUpperBound(1))
            End If

            With m_oDatabase

                'clear the old data
                .Parameters.Clear()

                'Load the FK parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                    Dim key As String = CStr(vFKArray(0, iParam))

                    Select Case key

                        Case "UserId"
                            m_lReturn = m_oDatabase.Parameters.Add(
                                                            sName:="UserId",
                                                            vValue:=m_iUserID,
                                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                            iDataType:=gPMConstants.PMEDataType.PMLong)

                        Case "UniqueId"
                            m_lReturn = m_oDatabase.Parameters.Add(
                                                            sName:="UniqueId",
                                                            vValue:=CStr(vFKArray(1, iParam)),
                                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                            iDataType:=gPMConstants.PMEDataType.PMString)

                        Case "ScreenHierarchy"
                            m_lReturn = m_oDatabase.Parameters.Add(
                                                            sName:="ScreenHierarchy",
                                                            vValue:=CStr(vFKArray(1, iParam)) & "/Rating Section",
                                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                            iDataType:=gPMConstants.PMEDataType.PMString)

                        Case "risk_type_id"
                            m_lReturn = m_oDatabase.Parameters.Add(
                                                            sName:=CStr(vFKArray(0, iParam)),
                                                            vValue:=CStr(vFKArray(1, iParam)),
                                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                            iDataType:=gPMConstants.PMEDataType.PMLong)
                        Case "rating_section_type_id"
                            m_lReturn = m_oDatabase.Parameters.Add(
                                                            sName:=CStr(vFKArray(0, iParam)),
                                                            vValue:=CStr(vFKArray(1, iParam)),
                                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                            iDataType:=gPMConstants.PMEDataType.PMLong)

                    End Select
                Next iParam

                m_lReturn = .SQLAction("spu_Risk_Type_Rating_section_type_Del", sPickListType & " PickList Delete", True)

                'See if there is anything to save
                If Information.IsArray(vKeys) Then

                    For iKey As Integer = vKeys.GetLowerBound(0) To vKeys.GetUpperBound(0)
                        .Parameters.Clear()

                        'Load the FK parameters
                        'For iParam = LBound(vFKArray, 2) To UBound(vFKArray, 2)


                        .Parameters.Add(sName:=CStr(vFKArray(0, 0)), vValue:=CStr(vFKArray(1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        'Next iParam


                        .Parameters.Add("rating_section_type_id", CStr(vKeys(iKey)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        .Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        .Parameters.Add(sName:="UniqueId", vValue:=CStr(vFKArray(1, 3)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                        .Parameters.Add(sName:="ScreenHierarchy", vValue:=CStr(vFKArray(1, 4)) & $"/Rating Section", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        'Call the SP

                        m_lReturn = .SQLAction("spu_Risk_Type_Rating_section_type_Add", sPickListType & " PickList Load", True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                            RollbackTrans()
                            Return m_lReturn
                        End If
                    Next iKey
                End If
            End With

            CommitTrans()

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            RollbackTrans()
            Return result
        End Try
    End Function

    Private Function PickListParams(ByRef vParams(,) As Object) As String

        Dim result As String = String.Empty


        Dim sComma As String = ""
        Dim sParam As New StringBuilder
        sComma = ""
        sParam = New StringBuilder("")
        For iParam As Integer = vParams.GetLowerBound(1) To vParams.GetUpperBound(1)
            sParam.Append(sComma & "?")
            sComma = ","
        Next iParam
        Return sParam.ToString()
    End Function
    Public Function GetRuleTypes(ByRef r_oResultArray(,) As Object) As Long

        Dim result As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Execute SQL Statement - use array for speed
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(
                sSQL:=ACSaaRuleTypeSQL,
                sSQLName:=ACSaaRuleTypeName,
                bStoredProcedure:=ACSaaRuleTypeStored,
                vResultArray:=r_oResultArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return m_lReturn
            End If

            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRuleTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRuleTypes", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
        End Try

    End Function
End Class
