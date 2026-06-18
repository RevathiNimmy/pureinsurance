
Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
'Developer Guide No. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

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
    '                   and gSIRLibrary.bas
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

    'JMK 23/10/2001 - Underwriting hidden option
    Private m_sUnderwritingType As String = ""

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

    ' JMK 23/10/2001 "A" for Underwriting Agency and "U" for Reinsurance
    Public ReadOnly Property UnderwritingType() As String
        Get

            If m_sUnderwritingType = "" Then
                m_lReturn = getUnderwritingType()
            End If

            Return m_sUnderwritingType

        End Get
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
    ' JMK 23/10/2001    Created
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

            m_lCurrentRecord = 0
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
    ' Name: GetLookUp
    '
    ' Description:get id and caption from table (use to display in combo boxes)
    '
    ' ***************************************************************** '
    Public Function GetLookUp(ByVal v_sTableName As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL, sKeyField As String

        Try

            sKeyField = v_sTableName & "_id"

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT " & sKeyField & ", code, cap.caption FROM " & v_sTableName & " tn, pmcaption cap " &
                   "WHERE tn.is_deleted = 0 AND tn.effective_date <= {effective_date} AND " &
                   "tn.caption_id = cap.caption_id AND cap.language_id =" & CStr(m_iLanguageID)


            m_oDatabase.Parameters.Clear()
            'Developer Guide No. 40
            If m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetLookUpValues", bStoredProcedure:=False, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookUp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookUp", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : GetAvailableCausation
    '
    ' Desc : get available causations which are not already allowed for this product type
    '
    ' Hist : 31/10/200 Created
    '
    ' ***************************************************************** '
    Public Function GetAvailableCausation(ByVal v_lProductId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Developer Guide No. 40
            If m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_oDatabase.SQLSelect(sSQL:=ACSelAvailableCausationSQL, sSQLName:=ACSelAvailableCausationName, bStoredProcedure:=ACSelAvailableCausationStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAvailableCausation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailableCausation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name : GetSuspendedTransaction
    '
    ' Desc : Get number of Suspended transactions of agent or sub agent
    '
    ' Hist : 29/06/2006 Created
    '
    ' ***************************************************************** '
    Public Function GetSuspendedTransaction(ByVal v_lProductId As Integer, ByRef r_vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="Product_ID", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_oDatabase.SQLSelect(sSQL:=ACSelProductSuspendSQL, sSQLName:=ACSelProductSuspendName, bStoredProcedure:=ACSelProductSuspendStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSuspendedTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSuspendedTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name : GetAllowedCausation
    '
    ' Desc : get allowed causations for product id
    '
    ' Hist : 31/10/200 Created
    '
    ' ***************************************************************** '
    Public Function GetAllowedCausation(ByVal v_lProductId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="Product_ID", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return m_oDatabase.SQLSelect(sSQL:=ACSelProductAllowedCausationSQL, sSQLName:=ACSelProductAllowedCausationName, bStoredProcedure:=ACSelProductAllowedCausationStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllowedCausation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllowedCausation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetComboDetails
    '
    ' Description: Get Description and ID number from Numbering_Scheme
    '              table to be used in the Combo Box
    '
    ' ***************************************************************** '
    Public Function GetComboDetails(ByVal v_lNumberingSchemeTypeID As Integer, ByRef r_vResultArray(,) As Object) As Integer


        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'SQL to select numbering_schemes for a specified numbering_scheme_type
            sSQL = "SELECT numbering_scheme_id, description"
            sSQL = sSQL & " From numbering_scheme"
            sSQL = sSQL & " WHERE numbering_scheme_Type_id = " & CStr(v_lNumberingSchemeTypeID)
            sSQL = sSQL & " AND is_deleted = 0"
            sSQL = sSQL & " ORDER BY 2"

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Numbering_Scheme_Sel_For_ComboBox", bStoredProcedure:=False, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetComboDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProductDetails
    '
    ' Description: get product details for specified product id
    '
    ' ***************************************************************** '
    Public Function GetProductDetails(ByVal v_lProductId As Integer, ByRef r_vResultArray(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Product_ID", vValue:=v_lProductId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelProductSQL, sSQLName:=ACSelProductName, bStoredProcedure:=ACSelProductStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllowedRiskTypeGroup
    '
    ' Description: get all risk type groups for a productID
    '
    ' ***************************************************************** '
    Public Function GetAllowedRiskTypeGroup(ByVal v_lProductId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Product_ID", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelProductRiskTypeGroupSQL, sSQLName:=ACSelProductRiskTypeGroupName, bStoredProcedure:=ACSelProductRiskTypeGroupStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllowedRiskTypeGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllowedRiskTypeGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRiskTypeGroup
    '
    ' Description: get all risk type groups
    '
    ' ***************************************************************** '
    Public Function GetAllRiskTypeGroup(ByRef r_vResultArray(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSaaRiskTypeGroupSQL, sSQLName:=ACSaaRiskTypeGroupName, bStoredProcedure:=ACSaaRiskTypeGroupStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllRiskTypeGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllRiskTypeGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllProducts
    '
    ' Description: SQL Query to Select Product File details
    '
    ' ***************************************************************** '
    Public Function GetAllProducts(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Execute SQL Statement - use array for speed

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSaaProductSQL, sSQLName:=ACSaaProductName, bStoredProcedure:=ACSaaProductStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllProducts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllProducts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' UpdateProduct
    ''' </summary>
    ''' <param name="v_iTask"></param>
    ''' <param name="r_lProductID"></param>
    ''' <param name="r_vParamArray"></param>
    ''' <param name="r_vAllowedRiskTypeGroup"></param>
    ''' <param name="r_vAllowedCausation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateProduct(ByVal v_iTask As Object, ByRef r_lProductID As Object, ByRef r_vParamArray As Object, ByRef r_vAllowedRiskTypeGroup As Object, ByRef r_vAllowedCausation As Object, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim nResult As Integer
        Dim nCaptionID As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetCaptionID(r_vParamArray(ACIDescription), nCaptionID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            'Add Product ID
            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Product_ID", vValue:=r_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Product_ID", vValue:=r_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Caption_ID", vValue:=nCaptionID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Code", vValue:=r_vParamArray(ACICode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Description", vValue:=r_vParamArray(ACIDescription), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Effective_Date", vValue:=r_vParamArray(ACIProductEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_Deleted", vValue:=r_vParamArray(ACIIsDeleted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Scheme_Agency_Ref", vValue:=r_vParamArray(ACISchemeAgencyRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Block_no", vValue:=r_vParamArray(ACIBlockNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_Tax_Suppressed", vValue:=r_vParamArray(ACIIsTaxSuppressed), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_Short_period_Rated", vValue:=r_vParamArray(ACIIsShortPeriodRated), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_Midnight_Renewal", vValue:=r_vParamArray(ACIIsMidnightRenewal), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_auto_renewable", vValue:=r_vParamArray(ACIIsAutoRenewable), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_period", vValue:=r_vParamArray(ACIRenewalPeriod), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="quote_auto_numbering_id", vValue:=r_vParamArray(ACIQuoteAutoNumberingID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_auto_numbering_id", vValue:=r_vParamArray(ACIPolicyAutoNumberingID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="prov_claim_auto_numbering_id", vValue:=r_vParamArray(ACIProvClaimAutoNumberingID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="full_claim_auto_numbering_id", vValue:=r_vParamArray(ACIFullClaimAutoNumberingID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_accumulation", vValue:=r_vParamArray(ACIAccumulation), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ri_Pointer", vValue:=r_vParamArray(ACIRIPointer), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="report_pointer", vValue:=r_vParamArray(ACIReportPointer), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_year_to_check", vValue:=r_vParamArray(ACIClaimYearToCheck), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="max_single_claim_value", vValue:=r_vParamArray(ACIMaxSingleClaimValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="max_number_of_claim", vValue:=r_vParamArray(ACIMaxNumberOfClaim), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="max_total_claim_value", vValue:=r_vParamArray(ACIMaxTotalClaimValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nb_prorata", vValue:=r_vParamArray(ACINBProrata), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="mta_prorata", vValue:=r_vParamArray(ACIMTAProrata), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="round_prem_to_nearest_unit", vValue:=r_vParamArray(ACIRoundPremium), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="rounding_section_id", vValue:=r_vParamArray(ACIRoundingSection), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_policy_number_at_quote", vValue:=r_vParamArray(ACIPolicyNumberAtQuote), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_standard_wording_edit", vValue:=r_vParamArray(ACIAllowStandardWordingEdit), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="follow_up_time_frame", vValue:=r_vParamArray(ACIFollowUpTimeFrame), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="grace_period", vValue:=r_vParamArray(ACIGracePeriod), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="prevent_cancelled_agents", vValue:=r_vParamArray(ACIPreventCancelledAgents), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_positive_cancellation", vValue:=r_vParamArray(ACIAllowPositiveValues), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="media_type_mandatory", vValue:=r_vParamArray(ACIMediaTypeMandatory), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_style_id", vValue:=r_vParamArray(ACIPolicyStyleID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_style_mandatory", vValue:=r_vParamArray(ACIPolicyStyleMandatory), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_currency_change", vValue:=r_vParamArray(ACICurrencyChange), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="allow_loss_currency_change", vValue:=r_vParamArray(ACILossCurrencyChange), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ChangePolicyNumberAtRenewal", vValue:=r_vParamArray(ACIChangePolicyNumberAtRenewal), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="HideSummaryAtRenewalAcceptance", vValue:=r_vParamArray(ACIHideSummaryAtRenewalAcceptance), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'True monthly policy
            m_lReturn = m_oDatabase.Parameters.Add(sName:="UnifiedRenewalDay", vValue:=r_vParamArray(ACIUnifiedRenewalDay), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="LeadAllowConsolidateComm", vValue:=r_vParamArray(ACILeadAllowConsolidateComm), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="LeadMonthInCycle", vValue:=gPMFunctions.ToSafeInteger(r_vParamArray(ACILeadMonthInCycle)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="LeadSuspenseAcct", vValue:=r_vParamArray(ACILeadSuspenseAcct), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="SubAllowConsolidateComm", vValue:=r_vParamArray(ACISubAllowConsolidateComm), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="SubMonthInCycle", vValue:=gPMFunctions.ToSafeInteger(r_vParamArray(ACISubMonthInCycle)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="SubSuspenseAcct", vValue:=r_vParamArray(ACISubSuspenseAcct), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("is_true_monthly_policy", r_vParamArray(ACIProductTrueMonthlyPolicy), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'm_lReturn = CType(AddInputParameter("anniversary_renewal_weeks", CBool(r_vParamArray(ACIProductAnniversaryRenewalWeeks)), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("anniversary_renewal_weeks", r_vParamArray(ACIProductAnniversaryRenewalWeeks), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("suppress_reserves", r_vParamArray(ACIProductSuppressClaimTransactionsReserves), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("suppress_payments", r_vParamArray(ACIProductSuppressClaimTransactionsPayments), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = CType(AddInputParameter("suppress_recoveries", r_vParamArray(ACIProductSuppressClaimTransactionsRecoveries), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("can_make_live_invoice", r_vParamArray(ACICanMakeLiveInvoice), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = CType(AddInputParameter("can_make_live_instalments", r_vParamArray(ACICanMakeLiveInstalments), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("can_make_live_paynow", r_vParamArray(ACICanMakeLivePaynow), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = CType(AddInputParameter("produce_schedule", r_vParamArray(ACIProduceSchedule), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("produce_certificate", r_vParamArray(ACIProduceCertificate), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("produce_debit_Note", r_vParamArray(ACIProduceDebitNote), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("enable_mtc_rating_rule", gPMFunctions.ToSafeLong(r_vParamArray(ACMTCRatingRulesEnabled)), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("Cover_Note_Default_Period", r_vParamArray(ACICPUpdCNDefaultPeriod), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("can_make_live_bankguarantee", gPMFunctions.ToSafeLong(r_vParamArray(ACICanMakeBankGuarantee)), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("Cover_Note_reused_upto", r_vParamArray(ACICPUpdCNMaxNo), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("Cover_Note_doc_Template_id", r_vParamArray(ACICPUpdCNDocTemplateId), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("Cover_Note_numbering_id", r_vParamArray(ACICPUpdCNNumberingId), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("allow_backdated_mtas", gPMFunctions.ToSafeLong(r_vParamArray(ACICPUpdAllowBackdatedMTAs)), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = AddInputParameter("allow_backdated_can", gPMFunctions.ToSafeLong(r_vParamArray(ACICPUpdAllowBackdatedCan)), gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = AddInputParameter("TMPautrenfac", ToSafeLong(r_vParamArray(ACICPUpdTMPAutoRenFac)), gPMConstants.PMEDataType.PMInteger)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_lReturn = RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = AddInputParameter("Mandatory_Risk_Type_Id", ToSafeLong(r_vParamArray(ACICPUpdMandatoryRiskTypeId)), gPMConstants.PMEDataType.PMInteger)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("out_of_sequence_MTA_UserGroup", gPMFunctions.ToSafeLong(r_vParamArray(ACICPUpdOutOfsequenceMTAUserGroup)), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("out_of_sequence_MTA_TaskGroup", gPMFunctions.ToSafeLong(r_vParamArray(ACICPUpdOutOfsequenceMTATaskGroup)), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("is_roundoff_to_zero", gPMFunctions.ToSafeInteger(r_vParamArray(ACIRoundOffToZero)), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("use_nb_payment_term_at_renselection", gPMFunctions.ToSafeLong(r_vParamArray(ACICPUpdUseNBRenPaymentTermsAtSelection)), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("check_mediatype_status_at_claim_payment", gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdCheckMediaTypeStatusAtClaimPayment)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("check_mediatype_status_at_policy_refund", gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdCheckMediaTypeStatusAtPolicyRefund)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("change_policy_number_at_renewal_auto", gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdChangePolicyNumberAtRenewalAutomatically)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("can_make_live_cashdeposit", gPMFunctions.ToSafeInteger(r_vParamArray(ACICanMakeCashDeposit)), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = AddInputParameter("allow_written_status",
                           ToSafeBoolean(r_vParamArray(ACIPUpdWrittenPolicyStatus)),
                           gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                UpdateProduct = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            m_lReturn = AddInputParameter("written_task_manager_days",
                       ToSafeInteger(r_vParamArray(ACIPUpdTaskManagerDays)),
                       gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                UpdateProduct = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            m_lReturn = AddInputParameter("written_rem_user_group",
                       ToSafeInteger(r_vParamArray(ACIPUpdReminderUserGroup)),
                       gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                UpdateProduct = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            m_lReturn = AddInputParameter("written_rem_task_group",
                       ToSafeInteger(r_vParamArray(ACIPUpdReminderTaskGroup)),
                       gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = RollbackTrans()
                UpdateProduct = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = CType(AddInputParameter("ri_manual_premium_adjustment", gPMFunctions.ToSafeInteger(r_vParamArray(ACICPUpdRIManaulPremiumAdjustment)), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("use_prior_term_scheme_at_ren", gPMFunctions.ToSafeInteger(r_vParamArray(ACICPUpdUsePriorTermSchemeAtRenewal)), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("bind_renewal_without_invitation", gPMFunctions.ToSafeInteger(r_vParamArray(ACICPUpdBindRenewalWithoutInvitation)), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'WPR11(a)(b)(c)-start

            m_lReturn = CType(AddInputParameter("bIsReservesReadonly", gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdIsReservesReadonly)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(AddInputParameter("bIsRecoveriesReadonly", gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdIsRecoveriesReadonly)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("bIsPaymentsReadonly", gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdIsPaymentsReadonly)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'WPR11(a)(b)(c)-END

            'WPR05 Start
            m_lReturn = CType(AddInputParameter("Quote_all_risk_NB", gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdDisplayRerateForQuoteAndNB)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("Quote_all_risk_MTC", gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdDisplayRerateForCancellationsAndReinstatments)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("Quote_all_risk_MTA", gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdDisplayRerateForMTA)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = CType(AddInputParameter("nQuote_all_risk_RENEWAL", gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdDisplayRerateForRenewal)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'WPR05 End
            m_lReturn = CType(AddInputParameter("Auto_Renew_BDMPolicy", gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdAutoRenewBackDatedMonthlyPolicy)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_enable_PrePayment", vValue:=r_vParamArray(ACICPUpdEnablePrePayment), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("Default_Cover_to_Date_to_Last_Day", gPMFunctions.ToSafeInteger(r_vParamArray(kICPUpdDefaultCoverToDateToLastDay)), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("nDo_Not_Delete_RenQuote_On_Mta", gPMFunctions.ToSafeInteger(r_vParamArray(ACICPUpdDoNotDeleteRenewalQuoteOnMTA)), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("nUnified_Renewal_Date_Is_Read_only",
                                              gPMFunctions.ToSafeInteger(
                                                  r_vParamArray(kICPUpdUnifiedRenewlDateIsReadOnly)),
                                              gPMConstants.PMEDataType.PMInteger),
                            gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = CType(AddInputParameter("nDelete_ren_quote_ReRun_on_MTA", gPMFunctions.ToSafeInteger(r_vParamArray(ACICPUpdDoNotDeleteRenQuote)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_retain_policy_number_on_copy", vValue:=gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdRetainPolicyNumberonCopy)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("Anniversary_Date_Editable", gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdEditAnnivDate)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(AddInputParameter("disable_cover_start_date_on_REN", gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdDisableCoverStartDateonREN)), gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="use_policy_inception_date", vValue:=r_vParamArray(ACICPUpdUsePolicyInceptionDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Authorisation_Threshold", vValue:=gPMFunctions.ToSafeDouble(r_vParamArray(ACICPUpdAuthorisationThreshold)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDecimal)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
			
            m_lReturn = m_oDatabase.Parameters.Add(sName:="void_policy_version", vValue:=gPMFunctions.ToSafeBoolean(r_vParamArray(ACICPUpdVoidTransaction)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_quote_versioning", vValue:=gPMFunctions.ToSafeInteger(r_vParamArray(ACICPUpdIsQuoteVersioning)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="delete_quote_after", vValue:=gPMFunctions.ToSafeInteger(r_vParamArray(ACICPUpdDeleteQuoteAfter)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(AddAdditionalParams(r_vParamArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddProductSQL, sSQLName:=ACAddProductName, bStoredProcedure:=ACAddProductStored)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    'get new product_id back to display on the parent listview
                    r_lProductID = m_oDatabase.Parameters.Item("Product_ID").Value
                End If
            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdProductSQL, sSQLName:=ACUpdProductName, bStoredProcedure:=ACUpdProductStored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Delete all allowed risktypegroup for product id
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Product_ID", vValue:=r_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

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

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelProductRiskTypeGroupSQL, sSQLName:=ACDelProductRiskTypeGroupName, bStoredProcedure:=ACDelProductRiskTypeGroupStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(r_vAllowedRiskTypeGroup) Then

                For lCount As Integer = r_vAllowedRiskTypeGroup.GetLowerBound(1) To r_vAllowedRiskTypeGroup.GetUpperBound(1)
                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=r_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_group_id", vValue:=r_vAllowedRiskTypeGroup(0, lCount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

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

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddProductRiskTypeGroupSQL, sSQLName:=ACAddProductRiskTypeGroupName, bStoredProcedure:=ACAddProductRiskTypeGroupStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next

            End If

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="Product_ID", vValue:=r_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.SQLAction(sSQL:=ACDelProductAllowedCausationSQL, sSQLName:=ACDelProductAllowedCausationName, bStoredProcedure:=ACDelProductAllowedCausationStored) <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'loop thro and add all allowed causations for this product type if any
            If Informations.IsArray(r_vAllowedCausation) Then

                For lCount As Integer = 0 To r_vAllowedCausation.GetUpperBound(1)

                    m_oDatabase.Parameters.Clear()

                    If m_oDatabase.Parameters.Add(sName:="Product_ID", vValue:=r_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_oDatabase.Parameters.Add(sName:="primary_cause_id", vValue:=r_vAllowedCausation(0, lCount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_oDatabase.SQLAction(sSQL:=ACAddProductAllowedCausationSQL, sSQLName:=ACAddProductAllowedCausationName, bStoredProcedure:=ACAddProductAllowedCausationStored) <> gPMConstants.PMEReturnCode.PMTrue Then

                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                Next lCount

            End If

            Return CommitTrans()

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateProduct  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateProduct ", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DelProduct
    '
    ' Description: set is_deleted to v_iIsDeleted
    '
    ' ***************************************************************** '
    Public Function DelProduct(ByVal v_lProductId As Integer, ByVal v_iIsDeleted As Integer, Optional ByVal v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = "UPDATE Product SET is_deleted = " & v_iIsDeleted
            sSQL = sSQL & ", UserId = " & m_iUserID
            sSQL = sSQL & ", UniqueId = '" & CStr(v_sUniqueId) & "'"
            sSQL = sSQL & ", ScreenHierarchy = '" & CStr(v_sScreenHierarchy) & "'"
            sSQL = sSQL & " WHERE product_id = " & CStr(v_lProductId)


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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelProduct  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelProduct ", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function BeginTrans() As Integer

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oArchDatabase As dPMDAO.Database = Nothing




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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Public Function GetAllowCurrencyChange(ByVal v_lProductId As Integer, ByRef r_lAllowCurrencyChange As Integer, ByRef r_lAllowLossCurrencyChange As Integer) As Integer

        Dim result As Integer = 0
        Dim vValue As String = ""
        Dim vResult(,) As Object = Nothing

        ' column position
        Const ALLOW_CURRENCY_CHANGE As Integer = 36
        Const ALLOW_LOSS_CURRENCY_CHANGE As Integer = 37

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(GetProductDetails(v_lProductId:=v_lProductId, r_vResultArray:=vResult), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Product Details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllowCurrencyChange", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            If Not Informations.IsArray(vResult) Then
                Return result
            End If


            vValue = CStr(vResult(ALLOW_CURRENCY_CHANGE, 0))

            If vValue <> "1" Or Convert.IsDBNull(vValue) Or Informations.IsNothing(vValue) Then
                vValue = "0"
            End If
            r_lAllowCurrencyChange = CInt(vValue)

            'DD 26/7/2004

            vValue = CStr(vResult(ALLOW_LOSS_CURRENCY_CHANGE, 0))

            If vValue <> "1" Or Convert.IsDBNull(vValue) Or Informations.IsNothing(vValue) Then
                vValue = "0"
            End If
            r_lAllowLossCurrencyChange = CInt(vValue)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllowCurrencyChange Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllowCurrencyChange", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: IsAllowedStandardWordingEdit
    '
    ' Description:
    '
    ' History: 29/04/2005 RKS - Created.
    '
    ' ***************************************************************** '
    Public Function IsAllowedStandardWordingEdit(ByVal v_lProductId As Integer, ByRef r_lAllowedStandardWordingEdit As Integer) As Integer
        Dim result As Integer = 0
        Dim vValue As String = ""
        Dim vResult(,) As Object = Nothing

        ' column position
        Const ALLOW_STANDARD_WORDING_EDIT As Integer = 39

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_lReturn = CType(GetProductDetails(v_lProductId:=v_lProductId, r_vResultArray:=vResult), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Product Details", vApp:=ACApp, vClass:=ACClass, vMethod:="IsAllowedStandardWordingEdit", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            If Not Informations.IsArray(vResult) Then
                Return result
            End If


            vValue = CStr(vResult(ALLOW_STANDARD_WORDING_EDIT, 0))

            If vValue <> "1" Or Convert.IsDBNull(vValue) Or Informations.IsNothing(vValue) Then
                vValue = "0"
            End If
            r_lAllowedStandardWordingEdit = CInt(vValue)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsAllowedStandardWordingEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsAllowedStandardWordingEdit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' AddInputParameter
    ''' </summary>
    ''' <param name="v_sName"></param>
    ''' <param name="v_vValue"></param>
    ''' <param name="v_iType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddInputParameter(ByVal v_sName As Object, ByVal v_vValue As Object, ByVal v_iType As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object
            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                        ", values :" & v_vValue & ", type:" & v_iType, gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetNoOfPoliciesOnProduct
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function GetNoOfPoliciesOnProduct(ByVal v_lProductId As Object, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetNoOfPoliciesOnProduct"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="product_id", v_vValue:=v_lProductId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetNoOfPoliciesOnProductSQL, sSQLName:=kGetNoOfPoliciesOnProductName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetNoOfPoliciesOnProductSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: CreateTask
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function CreateTask(ByVal v_lTaskGroupId As Integer, ByVal v_lTaskId As Integer, ByVal v_sCustomer As String, ByVal v_lUserGroupId As Integer, ByVal v_sDescription As String, ByVal v_iIsVisible As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateTask"

        Const kTaskStatusNew As Integer = 0
        Const kTaskIsNotUrgent As Integer = 0

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oTaskInstance As Object 'bPMWrkTaskInstance.FormClass
        Dim lTaskInstanceCnt As Object = Nothing

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' create instance of business object

            'oTaskInstance = New bPMWrkTaskInstance.FormClass

            oTaskInstance = Nothing
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=oTaskInstance, v_sClassName:="bPMWrkTaskInstance.FormClass", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bPMWrkTaskInstance.FormClass"
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bSIRRiskScreen.Business", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'lReturn = oTaskInstance.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError(kMethodName, "gPMComponentServices.CreateBusinessObject bPMWrkTaskInstance.Form Failed", gPMConstants.PMELogLevel.PMLogError)
            'End If

            ' create new task

            lReturn = oTaskInstance.CreateNew(v_lPMWrkTaskGroupID:=ToSafeInteger(v_lTaskGroupId), v_lPMWrkTaskID:=ToSafeInteger(v_lTaskId), v_sCustomer:=ToSafeString(v_sCustomer), v_dtTaskDueDate:=DateTime.Now, v_lPMUserGroupID:=ToSafeInteger(v_lUserGroupId), v_sDescription:=ToSafeString(v_sDescription), v_iTaskStatus:=ToSafeInteger(kTaskStatusNew), v_iIsUrgent:=ToSafeInteger(kTaskIsNotUrgent), v_dtDateCreated:=DateTime.Now, v_iCreatedByID:=ToSafeInteger(m_iUserID), r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_iIsVisible:=ToSafeInteger(v_iIsVisible))

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bPMWrkTaskInstance.TaskControl.CreateNew Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            oTaskInstance = Nothing

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CreateTransSuppressionNotificationTask
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function CreateTransSuppressionNotificationTask(ByVal v_sDescription As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateTransSuppressionNotificationTask"

        Const kTaskGroupId As Integer = 0
        Const kTaskId As Integer = 1
        Const kUserGroupId As Integer = 2

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lTaskGroupId, lTaskId, lUserGroupId As Integer
        Dim vTaskDetails(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' get required task details

            lReturn = CType(GetSysAdminMemoTaskDetails(vTaskDetails), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSysAdminMemoTaskDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get the required details from the array

            lTaskGroupId = CInt(vTaskDetails(kTaskGroupId, 0))

            lTaskId = CInt(vTaskDetails(kTaskId, 0))

            lUserGroupId = CInt(vTaskDetails(kUserGroupId, 0))

            ' NB: WE CREATE TWO TASK HERE
            ' ONE IS SO THE SYSADMINISTRATOR CAN SEE SOMEONE HAS CHANGED


            ' create the memo task against the system administors group (this could be deleted)
            lReturn = CType(CreateTask(v_lTaskGroupId:=lTaskGroupId, v_lTaskId:=lTaskId, v_sCustomer:="Product Risk Maintenance", v_lUserGroupId:=lUserGroupId, v_sDescription:=v_sDescription, v_iIsVisible:=1), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CreateTask Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' create the "hidden" memo task against the system administors group (this cannot be deleted)
            lReturn = CType(CreateTask(v_lTaskGroupId:=lTaskGroupId, v_lTaskId:=lTaskId, v_sCustomer:="Product Risk Maintenance", v_lUserGroupId:=lUserGroupId, v_sDescription:="HIDDEN LOGGING " & v_sDescription, v_iIsVisible:=0), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CreateTask Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetSysAdminMemoTaskDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetSysAdminMemoTaskDetails(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSysAdminMemoTaskDetails"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Execute selection Query
        If m_oDatabase.SQLSelect(sSQL:=kGetSysAdminMemoTaskDetailsSQL, sSQLName:=kGetSysAdminMemoTaskDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, kGetSysAdminMemoTaskDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If

        Return result
    End Function
    ' ******************************************************************
    ' Developed By : Puneet Kukreti
    ' Description : Navigator Enhancement
    ' Dated : 16-08-2006
    ' ***************************************************************** '
    Public Function CheckIfProduced(ByVal v_lProductId As Integer, ByRef r_vProducedArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_oDatabase.SQLSelect(sSQL:=ACSelGetDocProduceSQL, sSQLName:=ACSelGetDocProduceName, bStoredProcedure:=True, vResultArray:=r_vProducedArray, bKeepNulls:=False)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckIfProduced Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfProduced", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function ValidateAccountCode(ByVal sAccountCode As String, ByRef bFound As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="Code", vValue:=sAccountCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACChkAccountCodeSQL, sSQLName:=ACChkAccountCodeName, bStoredProcedure:=False, vResultArray:=vResult)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vResult) Then

            If CStr(vResult(0, 0)).Trim().ToUpper() = sAccountCode.Trim().ToUpper() Then bFound = True
        Else
            bFound = False
        End If
        Return result
    End Function

    Public Function UpdateProductPaymentMethod(ByVal v_lProductId As Integer, ByVal v_sPaymentMethod As String) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="paymentmethod", vValue:=v_sPaymentMethod, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdPaymentMethodSQL, sSQLName:=ACUpdPaymentMethodName, bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Return result
    End Function

    Public Function GetProductid(ByVal ifilecnt As Integer, ByRef vProduct_id As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()
        If m_oDatabase.Parameters.Add(sName:="ifilecnt", vValue:=CStr(ifilecnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductIDSQL, sSQLName:=ACGetProductIDName, bStoredProcedure:=False, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        ElseIf Informations.IsArray(vArray) Then

            Dim auxVar As Object = vArray(0, 0)


            vProduct_id = If(Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar), 0, CInt(vArray(0, 0)))
            Return result
        End If
        Return result
    End Function

    'Sankar - (WPRvb64 Media Type Status) - Paralleling
    'Added parameter r_bCheckMediaTypeStatusAtClaimPayment
    Public Function GetProductLevelOptionsForClaim(ByVal v_lClaimID As Integer, Optional ByRef r_bIs_Multiple_claims_payments As Boolean = False, Optional ByRef r_cMax_unauthorised_claim_value As Decimal = 0, Optional ByRef r_iMax_unauthorised_no_claim_payments As Integer = 0, Optional ByRef r_bRun_authorisation_scripts_claim_payments As Boolean = False, Optional ByRef r_cClaim_Value_For_Large_Loss_Advice As Decimal = 0, Optional ByRef r_bInclusion_of_CoInsurers_On_Claims As Boolean = False, Optional ByRef r_bAllow_Negative_Reserve As Boolean = False, Optional ByRef r_dExt_Clm_Handler_Acknowledged_Task_Allowed_Time As Double = 0, Optional ByRef r_dExt_Clm_Handler_Supply_Pre_Report_Task_Allowed_Time As Double = 0, Optional ByRef r_bValid_Policy_Version_At_Loss_Date As Boolean = False, Optional ByRef r_bIs_Gross_Claim_Payment_Amount As Boolean = False, Optional ByRef r_iClaim_Task_Group As Integer = 0, Optional ByRef r_iClaim_User_Group As Integer = 0, Optional ByRef r_bIs_Duplicate_Claim_Check_Enabled As Boolean = False, Optional ByRef r_bIs_Advanced_Tax_Script_Enabled As Boolean = False, Optional ByRef r_bIs_Payment_Ref_Check_Enabled As Boolean = False, Optional ByRef r_bIs_Recommend_Claim_Payment As Boolean = False, Optional ByRef r_bPaymentCannotExceedReserve As Boolean = False, Optional ByRef r_bCheckMediaTypeStatusAtClaimPayment As Boolean = False, Optional ByRef r_bSuppressReserve As Boolean = False, Optional ByRef r_nAuthorisation_Threshold As Decimal = 0.0) As Integer  'PN-69520 by Sushil Kumar

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Const kMethodName As String = "GetProductLevelOptionsForClaim"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "m_oDatabase.Parameters.Add(Claim_id)" & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductDetailsForClaimSQL, sSQLName:=ACGetProductDetailsForClaimName, bStoredProcedure:=True, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetProductDetailsForClaimSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If Informations.IsArray(vResultArray) Then
                'REMOVED CBool in the whole function beacuse of type caste error
                'start

                r_bIs_Multiple_claims_payments = gPMFunctions.ToSafeBoolean(vResultArray(ACIsMultipleClaimsPayments, 0))

                r_cMax_unauthorised_claim_value = gPMFunctions.ToSafeCurrency(vResultArray(ACMaxUnauthorisedClaimValue, 0))

                r_iMax_unauthorised_no_claim_payments = gPMFunctions.ToSafeInteger(vResultArray(ACMaxUnauthorisedNoClaimPayments, 0))

                r_bRun_authorisation_scripts_claim_payments = gPMFunctions.ToSafeBoolean(vResultArray(ACRunAuthorisationScriptsClaimPayments, 0))

                r_cClaim_Value_For_Large_Loss_Advice = gPMFunctions.ToSafeCurrency(vResultArray(ACClaimValueForLargeLossAdvice, 0))

                r_bInclusion_of_CoInsurers_On_Claims = gPMFunctions.ToSafeBoolean(vResultArray(ACInclusionOfCoInsurersOnClaims, 0))

                r_bAllow_Negative_Reserve = gPMFunctions.ToSafeBoolean(vResultArray(ACAllowNegativeReserve, 0))

                r_dExt_Clm_Handler_Acknowledged_Task_Allowed_Time = gPMFunctions.ToSafeDouble(vResultArray(ACExtClmHandlerAcknowledgedTaskAllowedTime, 0))

                r_dExt_Clm_Handler_Supply_Pre_Report_Task_Allowed_Time = gPMFunctions.ToSafeDouble(vResultArray(ACExtClmHandlerSupplyPreReportTaskAllowedTime, 0))

                r_bValid_Policy_Version_At_Loss_Date = gPMFunctions.ToSafeBoolean(vResultArray(ACValidPolicyversionatlossdate, 0))

                r_bIs_Gross_Claim_Payment_Amount = gPMFunctions.ToSafeBoolean(vResultArray(ACIsGrossClaimPaymentAmount, 0))

                r_iClaim_Task_Group = gPMFunctions.ToSafeInteger(vResultArray(ACClaimTaskGroup, 0))

                r_iClaim_User_Group = gPMFunctions.ToSafeInteger(vResultArray(ACClaimUserGroup, 0))

                r_bIs_Duplicate_Claim_Check_Enabled = gPMFunctions.ToSafeBoolean(vResultArray(ACIsDuplicateClaimcheckEnabled, 0))

                r_bIs_Advanced_Tax_Script_Enabled = gPMFunctions.ToSafeBoolean(vResultArray(ACIsadvancedTaxScriptEnabled, 0))

                r_bIs_Payment_Ref_Check_Enabled = gPMFunctions.ToSafeBoolean(vResultArray(ACIsPaymentRefCheckEnabled, 0))

                r_bIs_Recommend_Claim_Payment = gPMFunctions.ToSafeBoolean(vResultArray(ACIsRecommendClaimPayment, 0))

                r_bPaymentCannotExceedReserve = gPMFunctions.ToSafeBoolean(vResultArray(ACIsPaymentCannotExceedReserve, 0))
                'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling

                r_bCheckMediaTypeStatusAtClaimPayment = gPMFunctions.ToSafeBoolean(vResultArray(ACCheckMediaTypeStatusAtClaimPayment, 0))
                'End - Sankar - (WPRvb64 Media Type Status) - Paralleling
                'Start -Sushil Kumar(PN-69520)
                r_bSuppressReserve = gPMFunctions.ToSafeBoolean(vResultArray(ACCSuppressReserve, 0))
                'end
                r_nAuthorisation_Threshold = gPMFunctions.ToSafeDecimal(vResultArray(ACCAuthorisationThreshold, 0))
                'Start -Sushil Kumar(PN-69520)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************************** '
    ' Name: GetClaimWorkflow
    '
    ' Parameters: n/a
    '
    ' Description: This function will return Claim Workflow for a product
    '               and, if supplied, for a claim process type (open/maintain/pay)
    ' ***************************************************************************** '
    Public Function GetClaimWorkflow(ByRef r_vResults(,) As Object, ByVal v_lProductId As Integer) As Integer
        Return GetClaimWorkflow(r_vResults:=r_vResults, v_lProductId:=v_lProductId, v_lWorkflowID:=0)
    End Function

    Public Function GetClaimWorkflow(ByRef r_vResults(,) As Object, ByVal v_lProductId As Integer, ByVal v_lWorkflowID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimWorkflow"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="product_id", v_vValue:=v_lProductId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            'Validate workflowId otherwise return all rows applicable to the product
            If Not False Then
                If gPMFunctions.ToSafeLong(v_lWorkflowID) > 0 Then
                    m_lReturn = CType(AddInputParameter(v_sName:="claim_process_type_id", v_vValue:=gPMFunctions.ToSafeInteger(v_lWorkflowID), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
                End If
            End If

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetClaimWorkFlowSQL, sSQLName:=kGetClaimWorkFlowName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClaimWorkFlowName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************************** '
    ' Name: UpdateClaimWorkflow
    '
    ' Parameters: n/a
    '
    ' Description: This function will return Claim Workflow for a product
    '               and, if supplied, for a claim process type (open/maintain/pay)
    ' ***************************************************************************** '
    'Developer Guide No. 101
    Public Function UpdateClaimWorkflow(ByVal v_iTask As Object, ByVal v_lProductId As Object, ByVal vWorkflowArray As Object, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "UpdateClaimWorkflow"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            'If m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            If m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_lProductId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Claim_Process_Type_Id", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Process_Type_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Check_Unpaid_Status", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.ECheck_Unpaid_Status), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Reinsurance_Recovery", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EReinsurance_Recovery), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Salvage_Recovery", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.ESalvage_Recovery), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Third_Party_Recovery", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EThird_Party_Recovery), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="External_Claim_Handling", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EExternal_Claim_Handling), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Description_for_Change_in_Reserve", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Reserve), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Claim_Notification_Doc_Message", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Notification_Doc_Message), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Generate_Claim_Notification_Doc", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Notification_Doc), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Claim_Payment_Process", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Payment_Process), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Check_Deferred_Reinsurance", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.ECheck_Deferred_Reinsurance), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Fast_Track_Claims", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EFast_Track_Claims), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Reinsurance_Payment", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EReinsurance_Payment), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Description_for_Change_in_Payment", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EDescription_for_Change_in_Payment), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Cash_Payment_process", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.ECash_Payment_process), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Claim_Payment_Doc_Message", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EClaim_Payment_Doc_Message), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Generate_Claim_Payment_doc", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EGenerate_Claim_Payment_doc), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oDatabase.Parameters.Add(sName:="Make_Further_Payments", vValue:=vWorkflowArray(gPMConstants.EClaimWorkflowId.EMake_Further_Payments), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
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

            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                If m_oDatabase.SQLAction(sSQL:=kAddClaimWorkFlowSQL, sSQLName:=kAddClaimWorkFlowName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                    gPMFunctions.RaiseError(kMethodName, kAddClaimWorkFlowName & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                If m_oDatabase.SQLAction(sSQL:=kUpdClaimWorkFlowSQL, sSQLName:=kUpdClaimWorkFlowName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                    gPMFunctions.RaiseError(kMethodName, kUpdClaimWorkFlowName & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************************** '
    ' Name: GetClaimWorkflowForClaim
    '
    ' Parameters: n/a
    '
    ' Description: This function will return Claim Workflow for a product
    '               and, if supplied, for a claim process type (open/maintain/pay)
    ' ***************************************************************************** '
    Public Function GetClaimWorkflowForClaim(ByRef r_vResults(,) As Object, ByVal v_lClaimID As Integer, ByVal v_lWorkflowID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimWorkflowForClaim"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            m_lReturn = CType(AddInputParameter(v_sName:="claim_process_type_id", v_vValue:=v_lWorkflowID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetClaimWorkflowForClaimSQL, sSQLName:=kGetClaimWorkflowForClaimName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClaimWorkflowForClaimName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function PickListLoad(ByVal sPickListType As String, ByVal vFKArray(,) As Object, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            If vFKArray.GetUpperBound(1) > 0 And (sPickListType.Trim().ToUpper() = "CAUSATION" Or sPickListType.Trim().ToUpper() = "MTA" Or sPickListType.Trim().ToUpper() = "CLAIM" Or sPickListType.Trim().ToUpper() = "SOURCE") Then
                ReDim Preserve vFKArray(vFKArray.GetUpperBound(0), vFKArray.GetUpperBound(1) - 1)
            End If
            With m_oDatabase
                .Parameters.Clear()

                'Load the parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)

                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=CType(CInt(vFKArray(2, iParam)), gPMConstants.PMEDataType))
                    Exit For
                Next iParam

                'Call the SP
                Select Case sPickListType.Trim().ToUpper()
                    Case "CAUSATION"
                        'Developer Guide No. 39
                        m_lReturn = .SQLSelect("spu_SIR_SelectAll_Causation", sPickListType & " PickList Load", True, , vResultArray)

                    Case "MTA"
                        m_lReturn = .SQLSelect("spu_SIR_SelectAll_MTAEventDescription", sPickListType & " PickList Load", True, , vResultArray)

                    Case "CLAIM"
                        m_lReturn = .SQLSelect("spu_SIR_SelectAll_ClaimEventDescription", sPickListType & " PickList Load", True, , vResultArray)
                    Case "SOURCE"
                        m_lReturn = .SQLSelect("spu_SIR_SelectAll_Product_Source", sPickListType & " PickList Load", True, , vResultArray)
                End Select

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return m_lReturn
                End If
            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListLoad Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListLoad", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListParams
    '
    ' Description: Returns a string of question marks for the SP definition
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
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

    Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys As Object) As Integer

        Dim result As Integer = 0

        Try

            BeginTrans()


            With m_oDatabase

                'clear the old data
                .Parameters.Clear()

                'Load the FK parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)

                    If CStr(vFKArray(0, iParam)).Trim() = "UserId" Then
                        Exit For
                    End If
                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=CStr(vFKArray(1, iParam)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Next iParam

                .Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                .Parameters.Add(sName:="UniqueId", vValue:=CStr(vFKArray(1, 3)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                Select Case sPickListType.Trim().ToUpper()
                    Case "MTA"
                        .Parameters.Add(sName:="ScreenHierarchy", vValue:=CStr(vFKArray(1, 4)) & $"/MTA Events", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        m_lReturn = .SQLAction("spu_SIR_Delete_MTA", sPickListType & " PickList Delete", True)

                    Case "CLAIM"
                        .Parameters.Add(sName:="ScreenHierarchy", vValue:=CStr(vFKArray(1, 4)) & $"/Claim Events", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                        m_lReturn = .SQLAction("spu_SIR_Delete_Claim", sPickListType & " PickList Delete", True)

                End Select

                'See if there is anything to save
                If Informations.IsArray(vKeys) Then

                    For iKey As Integer = vKeys.GetLowerBound(0) To vKeys.GetUpperBound(0)
                        .Parameters.Clear()
                        .Parameters.Add(sName:=CStr(vFKArray(0, 0)), vValue:=CStr(vFKArray(1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        'Call the SP
                        Select Case sPickListType.Trim().ToUpper()
                            Case "MTA"

                                .Parameters.Add("mta_event_description_id", CStr(vKeys(iKey)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                .Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                                .Parameters.Add(sName:="UniqueId", vValue:=CStr(vFKArray(1, 3)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                                .Parameters.Add(sName:="ScreenHierarchy", vValue:=CStr(vFKArray(1, 4)) & $"/MTA Events", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                                m_lReturn = .SQLAction("spu_SIR_Save_MTA", sPickListType & " PickList Load", True)

                            Case "CLAIM"

                                .Parameters.Add("claim_event_description_id", CStr(vKeys(iKey)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                .Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                                .Parameters.Add(sName:="UniqueId", vValue:=CStr(vFKArray(1, 3)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                                .Parameters.Add(sName:="ScreenHierarchy", vValue:=CStr(vFKArray(1, 4)) & $"/Claim Events", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                                m_lReturn = .SQLAction("spu_SIR_Save_Claim", sPickListType & " PickList Load", True)

                        End Select

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Write Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            RollbackTrans()
                            Return m_lReturn
                        End If
                    Next iKey

                    If sPickListType.Trim().ToUpper() = "SOURCE" Then

                        Dim dtSaveProductSource As DataTable
                        Dim r_dtDataDetails As DataTable = New DataTable
                        Dim sColumnvalue As String = CStr(vFKArray(1, 0))
                        dtSaveProductSource = GetDataTableFromArray(vKeys, sColumnvalue)
                        m_oDatabase.Parameters.Clear()
                        Using cmd As New SqlCommand(ACSaveProductBranchSQL)

                            cmd.Parameters.AddWithValue("@Sources", dtSaveProductSource)
                            cmd.Parameters.AddWithValue("@UserId", m_iUserID)
                            cmd.Parameters.AddWithValue("@UniqueId", CStr(vFKArray(1, 3)))
                            cmd.Parameters.AddWithValue("@ScreenHierarchy", $"{CStr(vFKArray(1, 4))}/Branches")
                            cmd.CommandType = CommandType.StoredProcedure

                            m_lReturn = m_oDatabase.ExecuteDataTable(cmd, r_dtDataDetails)
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Write Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                RollbackTrans()
                                Return m_lReturn
                            End If
                        End Using
                    ElseIf sPickListType.Trim().ToUpper() = "CAUSATION" Then
                        Dim dtSaveCausation As DataTable
                        Dim r_dtDataDetails As DataTable = New DataTable
                        Dim sColumnvalue As String = CStr(vFKArray(1, 0))
                        dtSaveCausation = GetDataTableFromArray(vKeys, sColumnvalue)
                        m_oDatabase.Parameters.Clear()
                        Using cmd As New SqlCommand(ACSaveProductCausationSQL)

                            cmd.Parameters.AddWithValue("@Sources", dtSaveCausation)
                            cmd.Parameters.AddWithValue("@UserId", m_iUserID)
                            cmd.Parameters.AddWithValue("@UniqueId", CStr(vFKArray(1, 3)))
                            cmd.Parameters.AddWithValue("@ScreenHierarchy", CStr(vFKArray(1, 4)))
                            cmd.CommandType = CommandType.StoredProcedure

                            m_lReturn = m_oDatabase.ExecuteDataTable(cmd, r_dtDataDetails)
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Write Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                RollbackTrans()
                                Return m_lReturn
                            End If
                        End Using
                    End If
                End If
            End With

            CommitTrans()

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PickListSave Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PickListSave", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            RollbackTrans()
            Return result

        End Try
    End Function

    Public Function GetDataTableFromArray(ByVal array() As Object, ByVal sColumnvalue As String) As DataTable

        Dim dataTable As New DataTable("Sources")

        dataTable.Columns.Add("product_id", Type.GetType("System.Int32"))
        dataTable.Columns.Add("Linked_id", Type.GetType("System.Int32"))

        For i As Integer = array.GetLowerBound(0) To array.GetUpperBound(0)
            Dim datarow As DataRow
            datarow = dataTable.NewRow()
            datarow.Item(0) = sColumnvalue
            datarow.Item(1) = array(i)
            dataTable.Rows.Add(datarow)

        Next
        Return dataTable

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_vParamArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddAdditionalParams(ByRef r_vParamArray() As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Developer Guide No. 89
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_renewable", vValue:=gPMFunctions.ToSafeInteger(r_vParamArray(ACIIsRenewable)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_renewal_selection_enabled", vValue:=gPMFunctions.ToSafeInteger(r_vParamArray(ACIIsRenewalSelectionEnabled)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="true_monthly_policy_renewal_communication", vValue:=gPMFunctions.ToSafeInteger(r_vParamArray(ACITrueMonthlyPolicyRenewalCommunication)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_selection_man_review_template_id", vValue:=r_vParamArray(ACIRenewalSelectionManReviewTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_selection_man_review_attachment_template_id", vValue:=r_vParamArray(ACIRenewalSelectionManReviewAttachmentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_selection_invite_template_id", vValue:=r_vParamArray(ACIRenewalSelectionInviteTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_selection_invite_attachment_template_id", vValue:=r_vParamArray(ACIRenewalSelectionInviteAttachmentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_selection_update_template_id", vValue:=r_vParamArray(ACIRenewalSelectionUpdateTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_selection_update_attachment_template_id", vValue:=r_vParamArray(ACIRenewalSelectionUpdateAttachmentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_renewal_invite_enabled", vValue:=r_vParamArray(ACIIsRenewalInviteEnabled), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_invite_man_review_template_id", vValue:=r_vParamArray(ACIRenewalInviteManReviewTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_invite_man_review_attachment_template_id", vValue:=r_vParamArray(ACIRenewalInviteManReviewAttachmentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_invite_invite_template_id", vValue:=r_vParamArray(ACIRenewalInviteInviteTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_invite_invite_attachment_template_id", vValue:=r_vParamArray(ACIRenewalInviteInviteAttachmentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_invite_update_template_id", vValue:=r_vParamArray(ACIRenewalInviteUpdateTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_invite_update_attachment_template_id", vValue:=r_vParamArray(ACIRenewalInviteUpdateAttachmentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_renewal_update_enabled", vValue:=r_vParamArray(ACIIsRenewalUpdateEnabled), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_update_man_review_template_id", vValue:=r_vParamArray(ACIRenewalUpdateManReviewTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_update_man_review_attachment_template_id", vValue:=r_vParamArray(ACIRenewalUpdateManReviewAttachmentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_update_invite_template_id", vValue:=r_vParamArray(ACIRenewalUpdateInviteTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_update_invite_attachment_template_id", vValue:=r_vParamArray(ACIRenewalUpdateInviteAttachmentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_update_update_template_id", vValue:=r_vParamArray(ACIRenewalUpdateUpdateTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="renewal_update_update_attachment_template_id", vValue:=r_vParamArray(ACIRenewalUpdateUpdateAttachmentTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_agent_renewal_selection_enabled", vValue:=gPMFunctions.ToSafeInteger(r_vParamArray(ACIIsAgentRenewalSelectionEnabled)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_agent_renewal_invite_enabled", vValue:=gPMFunctions.ToSafeInteger(r_vParamArray(ACIIsAgentRenewalInviteEnabled)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_agent_renewal_update_enabled", vValue:=gPMFunctions.ToSafeInteger(r_vParamArray(ACIIsAgentRenewalUpdateEnabled)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_renewal_man_review_template_id", vValue:=r_vParamArray(ACIAgentRenewalManReviewTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_renewal_man_review_report_id", vValue:=r_vParamArray(ACIAgentRenewalManReviewReportId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_renewal_invite_template_id", vValue:=r_vParamArray(ACIAgentRenewalInviteTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_renewal_invite_report_id", vValue:=r_vParamArray(ACIAgentRenewalInviteReportId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_renewal_update_template_id", vValue:=r_vParamArray(ACIAgentRenewalUpdateTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="agent_renewal_update_report_id", vValue:=r_vParamArray(ACIAgentRenewalUpdateReportId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="multiple_claims_payments", vValue:=r_vParamArray(ACICPUpdMultipleClaimPayments), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="max_unauthorised_claim_value", vValue:=r_vParamArray(ACICPUpdMaxUnauthorisedClaimValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="max_unAuthorised_no_claim_payments", vValue:=r_vParamArray(ACICPUpdMaxNoofUnauthorisedClaimPayments), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="run_authorisation_scripts_claim_payments", vValue:=r_vParamArray(ACICPUpdRunAuthorisationScriptsforClaimPayments), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="BankAccount_Id", vValue:=r_vParamArray(ACICPUpdBankAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Value_For_Large_Loss_Advice", vValue:=r_vParamArray(ACICPUpdClaimValueForLargeLossAdvice), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Inclusion_of_CoInsurers_On_Claims", vValue:=r_vParamArray(ACICPUpdInclusionofCoInsurersOnClaims), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Allow_Negative_Reserve", vValue:=r_vParamArray(ACICPUpdAllowNegativeReserve), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Ext_Clm_Handler_Acknowledged_Task_Allowed_Time", vValue:=r_vParamArray(ACICPUpdExtClmHandlerAcknowledgedTaskAllowedTime), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Ext_Clm_Handler_Supply_Pre_Report_Task_Allowed_Time", vValue:=r_vParamArray(ACICPUpdExtClmHandlerSupplyPreReportTaskAllowedTime), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Valid_Policy_Version_At_Loss_Date", vValue:=r_vParamArray(ACICPUpdValidPolicyVersionAtLossDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_Gross_Claim_Payment_Amount", vValue:=r_vParamArray(ACICPUpdIsGrossClaimPaymentAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_Task_Group", vValue:=r_vParamArray(ACICPUpdClaimTaskGroup), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_User_Group", vValue:=r_vParamArray(ACICPUpdClaimUserGroup), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claims_UDT_A", vValue:=r_vParamArray(ACICPUpdClaimsUDTA), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claims_UDT_B", vValue:=r_vParamArray(ACICPUpdClaimsUDTB), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claims_UDT_C", vValue:=r_vParamArray(ACICPUpdClaimsUDTC), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claims_UDT_D", vValue:=r_vParamArray(ACICPUpdClaimsUDTD), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claims_UDT_E", vValue:=r_vParamArray(ACICPUpdClaimsUDTE), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_Duplicate_Claim_Check_Enabled", vValue:=r_vParamArray(ACICPUpdIsDuplicateClaimCheckEnabled), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_Advanced_Tax_Script_Enabled", vValue:=r_vParamArray(ACICPUpdIsAdvancedTaxScriptEnabled), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Is_Payment_Ref_Check_Enabled", vValue:=r_vParamArray(ACICPUpdIsPaymentRefCheckEnabled), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_Recommend_Claim_Payments", vValue:=r_vParamArray(ACICPUpdIsRecommendClaimPayments), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="MTA_Date_allowed", vValue:=r_vParamArray(ACICPUpdMTAdateallowed), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="MtA_Allocation", vValue:=r_vParamArray(ACICPUpdMTAAllocation), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="default_renewal_months", vValue:=r_vParamArray(ACICPUpdDefaultRenMonths), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="payment_cannot_exceed_reserve", vValue:=r_vParamArray(ACICPPaymentCannotExceedReserve), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="recovery_instalments_enabled", vValue:=r_vParamArray(ACICPUpdRecoveryInstalmentsEnabled), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
 If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
 m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
 Return gPMConstants.PMEReturnCode.PMFalse
 End If


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAdditionalParams Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateProduct - AddAdditionalParams ", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Start (Gaurav Arora) Tech Spec HG PS001 - Reinsurance Modifications)

    Public Function GetProductDetailsForPolicy(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vProductArray(,) As Object) As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Const kMethodName As String = "GetProductValue"
        Dim vResults(,) As Object = Nothing

        Try
            Catch_Renamed = True



            result = gPMConstants.PMEReturnCode.PMTrue
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductValuesFromInsuranceFileIDSQL, sSQLName:=ACGetProductValuesFromInsuranceFileIDName, bStoredProcedure:=ACGetProductValuesFromInsuranceFileIDStored, vResultArray:=vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to get the product value from the given insurance file ID", gPMConstants.PMELogLevel.PMLogError)
            Else
                If Informations.IsArray(vResults) Then


                    r_vProductArray = vResults
                Else
                    gPMFunctions.RaiseError(kMethodName, CStr(CDbl("No Product Details found for Insurance File Cnt = ") + v_lInsuranceFileCnt), gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep

            End If

            GoTo Finally_Renamed

            If Catch_Renamed Then


                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

            End If
Finally_Renamed:
        End Try
    End Function

    'Start (Venkatesh Raman) Tech Spec WR19 - Cover Note Functionality.doc section(4.4.1.1.1)

    Public Function GetProductValue(ByVal v_lProductId As Integer, ByVal v_sColumnName As String, ByRef r_vProductArray(,) As Object) As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Const kMethodName As String = "GetProductValue"

        Try
            Catch_Renamed = True



            result = gPMConstants.PMEReturnCode.PMTrue
            bPMAddParameter.AddParameterLite(m_oDatabase, "product_id", v_lProductId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            'bPMAddParameter.AddParameterLite(m_oDatabase, "column_name", CInt(v_sColumnName), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "column_name", (v_sColumnName), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductValueSQL, sSQLName:=ACGetProductValueName, bStoredProcedure:=ACGetProductValueStored, vResultArray:=r_vProductArray)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetProductValueSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            If Catch_Renamed Then


                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

            End If
Finally_Renamed:
        End Try
    End Function

    'End (Venkatesh Raman) Tech Spec WR19 - Cover Note Functionality.doc sectionsection(4.4.1.1.1)
End Class
