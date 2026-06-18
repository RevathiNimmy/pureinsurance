Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("InterfaceNoLogin_NET.InterfaceNoLogin")>
Public NotInheritable Class InterfaceNoLogin
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


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Initialise"

    Private Const acModule As String = "InterfaceNoLogin"

    Public Const GEMMaxListItems As Integer = 2500
    Private m_vVehicleData() As Object
    Private _m_oCommon As Common = Nothing
    Private Property m_oCommon() As Common
        Get
            If _m_oCommon Is Nothing Then
                _m_oCommon = New Common()
            End If
            Return _m_oCommon
        End Get
        Set(ByVal Value As Common)
            _m_oCommon = Value
        End Set
    End Property
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lMaxListItems As Integer

    'sj 02/02/2001 - start
    Public WriteOnly Property NumberOfVehicles() As Integer
        Set(ByVal Value As Integer)
            m_oCommon.NumberOfVehicles = Value
        End Set
    End Property
    Public WriteOnly Property VehicleListId() As String
        Set(ByVal Value As String)
            m_oCommon.VehicleListId = Value
        End Set
    End Property
    'sj 02/02/2001 - end
    Public WriteOnly Property MaxListItems() As Integer
        Set(ByVal Value As Integer)
            m_lMaxListItems = Value
        End Set
    End Property
    Public WriteOnly Property ClassOfBusiness() As String
        Set(ByVal Value As String)
            m_oCommon.ClassOfBusiness = Value
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

            ' removed all the object manager stuff - CL230699

            m_lMaxListItems = GEMMaxListItems

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_oCommon.CloseFiles()
                m_oCommon = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' CTAF 20040122 - Start
    ' We need to have the ByRef value as a variant as this method will
    ' be called from a script
    Public Function GetDescriptionFromABICodeScripting(ByVal v_sPropertyId As String, ByVal v_sABICode As String, ByRef r_sDescription As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sDescTemp As String = ""

        result = GetDescriptionFromABICode(v_sPropertyId:=v_sPropertyId, v_sABICode:=v_sABICode, r_sDescription:=sDescTemp)

        r_sDescription = sDescTemp

        Return result
    End Function
    ' CTAF 20040122 - End

    Public Function GetDescriptionFromABICode(ByVal v_sPropertyId As String, ByVal v_sABICode As String, ByRef r_sDescription As String) As gPMConstants.PMEReturnCode


        m_oCommon.MaxListItems = m_lMaxListItems
        Return CType(m_oCommon.GetDescriptionFromABICodeC(v_sPropertyId:=v_sPropertyId, v_sABICode:=v_sABICode, r_sDescription:=r_sDescription), gPMConstants.PMEReturnCode)

    End Function
    ''' <summary>
    ''' used
    ''' </summary>
    ''' <param name="v_sPropertyId"></param>
    ''' <param name="v_sDescription"></param>
    ''' <param name="r_sABICode"></param>
    ''' <returns></returns>
    Public Function GetABICodeFromDescription(ByVal v_sPropertyId As String, ByVal v_sDescription As String, ByRef r_sABICode As String) As gPMConstants.PMEReturnCode

        m_oCommon.MaxListItems = m_lMaxListItems
        Return CType(m_oCommon.GetABICodeFromDescriptionC(v_sPropertyId:=v_sPropertyId, v_sDescription:=v_sDescription, r_sABICode:=r_sABICode), gPMConstants.PMEReturnCode)

    End Function

    ''' <summary>
    ''' used
    ''' </summary>
    ''' <param name="v_sPropertyId"></param>
    ''' <param name="r_vListData"></param>
    ''' <param name="r_vListDataCode"></param>
    ''' <param name="v_vSearchString"></param>
    ''' <param name="v_bMultiSearch"></param>
    ''' <returns></returns>
    Public Function GetListAndCodes(ByVal v_sPropertyId As String, ByRef r_vListData As Object, ByRef r_vListDataCode As Object, Optional ByVal v_vSearchString As Object = Nothing, Optional ByVal v_bMultiSearch As Boolean = False) As Integer

        Dim result As Integer = 0
        m_oCommon.MaxListItems = m_lMaxListItems

        'DB 16/2/2000 Start

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lReturn As gPMConstants.PMEReturnCode

        'If it's a occupation/employer's business search, we now need
        'to loop up to 3 times until we get less than 15 matches (hopefully).
        'First loop with a 3 character search, then 4, then 5 (stop at 5).

        If v_bMultiSearch Then
            For iNoChars As Integer = 3 To 5

                'If the number of characters is greater than the length of the search string,
                'don't continue in the loop - it isn't going to get any better.

                If iNoChars > v_vSearchString.Length Then
                    Exit For
                End If

                lReturn = CType(m_oCommon.GetListC(v_sPropertyId, r_vListData, v_vSearchString, r_vListDataCode, v_bMultiSearch:=True, v_iFirstNoChars:=iNoChars), gPMConstants.PMEReturnCode)

                If Informations.IsArray(r_vListData) Then
                    If (r_vListData.GetUpperBound(0) + 1) <= 15 Then
                        result = lReturn
                        Exit For
                    End If
                Else
                    Return lReturn
                End If

            Next
        Else
            result = m_oCommon.GetListC(v_sPropertyId, r_vListData, v_vSearchString, r_vListDataCode, v_bMultiSearch:=False)
        End If

        'DB 16/2/2000 End

        Return result
    End Function

    ''' <summary>
    ''' used
    ''' </summary>
    ''' <param name="v_sGisDataModelCode"></param>
    ''' <param name="v_sSellerCode"></param>
    ''' <returns></returns>
    Public Function CheckListVersions(ByVal v_sGisDataModelCode As String, Optional ByVal v_sSellerCode As String = "") As Integer

        m_oCommon.NoLogin = True

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

        m_oCommon.MaxListItems = m_lMaxListItems
        Return m_oCommon.GetDescriptionC(sPropertyId:=sPropertyId, sABICodeTarget:=sABICodeTarget, sDescription:=tosafestring(sDescription))

    End Function

    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

