Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Module MainModule
    ' Main public constant for all functions
    ' to identify which application this is.

    Public Const ACApp As String = "iVOYLookup"
    ' Constant for the functions to identify
    ' which class this is.

    Public Const ACDefaultFirstItem As Integer = 0

    Private Const ACClass As String = "MainModule"

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager
    'developer guide no. 107
    <ThreadStatic()> _
    Private m_oBusiness As Object
    'developer guide no. 107
    <ThreadStatic()> _
    Private m_oTypeTableBusiness As Object
    'developer guide no. 107
    <ThreadStatic()> _
    Private m_oBankBusiness As Object
    'developer guide no. 107
    <ThreadStatic()> _
    Private m_oExplorerBusiness As Object

    '' Legal values for TableName
    'Public Enum actTableName
    '  actDocumentType
    '  actPostingStatus
    '  actLedgerType
    '  actPurgeFrequency
    '  actAccountType
    'End Enum

    Public Function Terminate() As Integer
        g_oObjectManager = Nothing
        m_oBusiness = Nothing
        m_oTypeTableBusiness = Nothing
        m_oBankBusiness = Nothing
        m_oExplorerBusiness = Nothing
    End Function

    ' Initialise the Object Manager Etc
    Public Function Initialise() As Integer




        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Have we been here before ?
            If Not (g_oObjectManager Is Nothing) Then
                Return result
            End If

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function
    'Public Function GetLookupValues(ByVal v_sTypeTable As String, ByVal v_dtEffectiveDate As Date, ByRef ctlLookup As Control, Optional ByVal v_vInsurerNo As Object = Nothing, Optional ByVal v_vObjectID As Object = Nothing) As Integer
    Public Function GetLookupValues(ByVal v_sTypeTable As String, ByVal v_dtEffectiveDate As Date, ByRef ctlLookup As ComboBox, Optional ByVal v_vInsurerNo As Object = Nothing, Optional ByVal v_vObjectID As Object = Nothing) As Integer
        ' To hold the lookup results
        Dim result As Integer = 0
        Dim vLookupValues(,) As Object = Nothing
        Dim vLookupDetails As Object = Nothing


        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetTypeTableBusiness(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set the business to look at the particular table in question

            m_oTypeTableBusiness.LookupName = v_sTypeTable

            ' Get all of the lookup values with effective date passed


            m_lReturn = m_oTypeTableBusiness.GetLookupValues(dtEffectiveDate:=v_dtEffectiveDate, vTableArray:=vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=vLookupDetails, v_vInsurerNo:=v_vInsurerNo, v_vObjectID:=v_vObjectID)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_dtEffectiveDate", v_dtEffectiveDate)
                oDict.Add("v_vObjectID", v_vObjectID)
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", oDicParms:=oDict)

                Return result
            End If

            ' Now load the values into the combo/list box passed in




            For lCntr As Integer = CInt(vLookupValues(ACValueStartPos, 0)) To CInt((CDbl(vLookupValues(ACValueStartPos, 0)) + CDbl(vLookupValues(ACValueNumber, 0))) - 1)


                'developer guide no.29
                ' ctlLookup.AddItem(vLookupDetails(ACDetailDesc, lCntr))



                'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(vLookupDetails(ACDetailKey, lCntr))
                Dim NewIndex As Integer = ctlLookup.Items.Add(New VB6.ListBoxItem(vLookupDetails(ACDetailDesc, lCntr)))
            Next lCntr

            m_oBusiness = Nothing
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetTypeTableEntry(ByVal v_lTypeTableID As Integer, ByRef r_bIsDeleted As Boolean, ByRef r_dtEffectiveDate As Date, ByRef r_sDescription As String, ByRef r_sCode As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetTypeTableBusiness(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = m_oTypeTableBusiness.GetDetails(vTypeTableID:=v_lTypeTableID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = m_oTypeTableBusiness.GetNext(vIsDeleted:=r_bIsDeleted, vEffectiveDate:=r_dtEffectiveDate, vDescription:=r_sDescription, vCode:=r_sCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get type table entry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTypeTableEntry", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    Function GetTypeTableBusiness() As Integer

        Dim result As Integer = 0
        Try
            ' Ensure that we have an object manager

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If m_oTypeTableBusiness Is Nothing Then
                ' Get a TypeTable Business Object
                Dim temp_m_oTypeTableBusiness As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oTypeTableBusiness, "bGEMLookup.Form", vInstanceManager:="ClientManager")
                m_oTypeTableBusiness = temp_m_oTypeTableBusiness

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get an instance of the TypeTable business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTypeTableBusiness")
                    Return result
                End If
            End If
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the type table business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTypeTableBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Function to return the position of an Item in ItemData
    Public Function IndexOfItem(ByRef r_cboCombo As ComboBox, ByVal v_lItemId As Integer) As Integer
        With r_cboCombo

            'developer guide no.275
            'For nIndex As Integer = 0 To .ListCount - 1
            For nIndex As Integer = 0 To .Items.Count - 1

                'to be checked at runtime
                'If .ItemData(nIndex) = v_lItemId Then
                '    Return nIndex
                'End If
                If .Items.Item(nIndex) = v_lItemId Then
                    Return nIndex
                End If
            Next nIndex
        End With
        Return -1
    End Function
End Module
