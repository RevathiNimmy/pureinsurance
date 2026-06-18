Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Wrapper
    '
    ' Date: 16/09/1998
    '
    ' Description: Main public class of the Wrapper.
    '
    ' Edit History:
    ' ***************************************************************** '
    'developer guide no. 107(Guide)	
    ' Public instance of the object manager.
    Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public source and language ID's from the
    ' Object Manager.
    Public g_iLanguageID As Integer
    Public g_iSourceID As Integer
    ' RDC 31052001
    'end'
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Initialise"

    ' {* USER DEFINED CODE (Begin) *}
    Private _m_oCommon As Common = Nothing
    Private Property m_oCommon() As Common
        Get
            If _m_oCommon Is Nothing Then
                _m_oCommon = New Common()
            End If
            Return _m_oCommon
        End Get
        Set(ByVal Value As Common)
            _m_oCommon = value
        End Set
    End Property

    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer


    Private m_oBusiness As bGISListManager.Form
    Private m_oZipper As bPMZipper.Business
    Private m_lMaxListItems As Integer

    Public WriteOnly Property MaxListItems() As Integer
        Set(ByVal Value As Integer)
            m_lMaxListItems = Value
        End Set
    End Property
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sTitle, sMessage As String

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                ' Log Error.
                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bGISListManager.Form", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Display error stating the problem.
                sTitle = "Unable to Create List Update Business"
                sMessage = "iGISListManager"
                'MsgBox sMessage$, vbCritical, sTitle$
                bPMFunc.LogMessage("", gPMConstants.PMELogLevel.PMLogError, sMessage, ACApp, ACClass, "Initialise")
                Return result
            End If

            ' File compresser
            m_oZipper = New bPMZipper.Business()

            If m_oZipper Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lMaxListItems = GEMMaxListItems

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            Me.disposedValue = True
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                m_oCommon = Nothing
                If Not (m_oZipper Is Nothing) Then
                    m_oZipper = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetListIdsAndNames
    '
    ' Description:
    '
    ' History: 19/01/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetListIdsAndNames(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            Return m_oCommon.GetListIdsAndNamesC(r_vResultArray:=r_vResultArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListIdsAndNames Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListIdsAndNames", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetDescriptionFromABICode(ByVal v_sPropertyId As String, ByVal v_sABICode As String, ByRef r_sDescription As String) As gPMConstants.PMEReturnCode


        m_oCommon.MaxListItems = m_lMaxListItems
        Return CType(m_oCommon.GetDescriptionFromABICodeC(v_sPropertyId:=v_sPropertyId, v_sABICode:=v_sABICode, r_sDescription:=r_sDescription), gPMConstants.PMEReturnCode)

    End Function
    Public Function GetABICodeFromDescription(ByVal v_sPropertyId As String, ByVal v_sDescription As String, ByRef r_sABICode As String) As gPMConstants.PMEReturnCode


        m_oCommon.MaxListItems = m_lMaxListItems
        Return CType(m_oCommon.GetABICodeFromDescriptionC(v_sPropertyId:=v_sPropertyId, v_sDescription:=v_sDescription, r_sABICode:=r_sABICode), gPMConstants.PMEReturnCode)

    End Function

    ' CL090699 BEGIN-->
    Public Function GetListAndCodes(ByVal v_sPropertyId As String, ByRef r_vListData() As Object, ByRef r_vListDataCode() As Object, Optional ByVal v_vSearchString As String = "") As Integer

        m_oCommon.MaxListItems = m_lMaxListItems
        Return m_oCommon.GetListC(v_sPropertyId, r_vListData, v_vSearchString, r_vListDataCode)

    End Function
    ' <-- END CL090699

    ' CL090699 BEGIN-->
    ' This wrapper avoids binary imcompatibility
    'Developer Guide No 101
    Public Function GetList(ByVal v_sPropertyId As String, ByRef r_vListData As Object, Optional ByVal v_vSearchString As Object = Nothing) As Integer

        m_oCommon.MaxListItems = m_lMaxListItems
        Return m_oCommon.GetListC(v_sPropertyId, r_vListData, v_vSearchString)

    End Function
    ' <-- END CL090699

    Public Function PopulateListControl(ByVal v_sPropertyId As String, ByRef r_oControl As Object, Optional ByVal v_vSearchString As Object = Nothing) As Integer

        m_oCommon.MaxListItems = m_lMaxListItems

        Return m_oCommon.PopulateListControlC(v_sPropertyId:=v_sPropertyId, r_oControl:=r_oControl, v_vSearchString:=CStr(v_vSearchString))

    End Function

    Public Function CheckListVersions(ByVal v_sGisDataModelCode As String, Optional ByVal v_sSellerCode As String = "") As Integer

        m_oCommon.NoLogin = False


        m_oCommon.Business = m_oBusiness

        'Modified by Archana Tokas on 4/27/2010 10:31:27 AM changes as per requirement
        'm_oCommon.set_Zipper(m_oZipper)
        m_oCommon.Zipper = m_oZipper

        Return m_oCommon.CheckListVersionsC(v_sGisDataModelCode:=v_sGisDataModelCode, v_sSellerCode:=v_sSellerCode)

    End Function
    ' ***************************************************************** '
    ' Name: GetDescription (Standard Method)
    '
    ' Description: Returns a desc for a given property idand ABI code.
    '
    ' CL090699
    '
    ' ***************************************************************** '
    Public Function GetDescription(ByVal sPropertyId As String, ByVal sABICodeTarget As String, ByRef sDescription As String) As Integer

        Return m_oCommon.GetDescriptionC(sPropertyId:=sPropertyId, sABICodeTarget:=sABICodeTarget, sDescription:=sDescription)

    End Function



    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the Wrapper entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
		Dispose(False)
    End Sub

End Class
