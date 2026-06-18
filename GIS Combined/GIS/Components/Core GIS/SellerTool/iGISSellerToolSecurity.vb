Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Security_NET.Security")> _
Public NotInheritable Class Security
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Security
    '
    ' Date: CL0406000
    '
    ' Description:
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Security"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' GIS Data Set
    Private m_oDataSet As cGISDataSetControl.Application

    ' List Manager
    Private m_oListManager As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property








    'RFC190400 - Add Lookup Methods to GIS


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
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
    Public Function Initialise() As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDataSet = New cGISDataSetControl.Application()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oListManager IsNot Nothing Then
                    m_oListManager.Dispose()
                    m_oListManager = Nothing
                End If
                m_oDataSet = Nothing
            End If
        End If
		Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    ' Name: RegisterUser
    '
    ' Description: RegisterUser method
    '
    ' Author: RAG070600
    '
    ' RFC050900 - Added Title, MaritalStatusCode & Address/postcode  parameters, needed for its4me.
    ' RFC050900 - Added BusinessTypeCode Parameter as this will be needed by Xelector multi channels
    ' RFC050900 - Added AdditonalDataArray. This is a array of name/value
    '             pairs, that can be used in the future to pass to OR return
    '             extra data from the Back Office Mapper, without need to change
    '             the method interface.
    ' ***************************************************************** '
    Public Function RegisterUser(ByVal v_sDataModelCode As Object, ByVal v_sForename As Object, ByVal v_sSurname As Object, ByVal v_sMothersMaidenName As Object, ByVal v_sDateOfBirth As Object, ByVal v_sEmailAddress As Object, ByVal v_sMemorableDate As Object, ByVal v_sAQuestion As Object, ByVal v_sTheAnswer As Object, ByVal v_sCurrentRenewalDate As Object, ByRef r_sUserID As String, ByRef r_sPassword As String, ByRef r_sPartyCnt As Integer, Optional ByVal v_sBusinessTypeCode As String = "", Optional ByVal v_sTitle As String = "", Optional ByVal v_sMaritalStatusCode As String = "", Optional ByVal v_sAddress1 As String = "", Optional ByVal v_sAddress2 As String = "", Optional ByVal v_sAddress3 As String = "", Optional ByVal v_sAddress4 As String = "", Optional ByVal v_sPostcode As String = "", Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sActionXML, sDataModelCode As String

        Dim sActionReturnXML As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim lTemp As Integer
        Dim sUserID, sPassword As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'sDataModelCode = m_oDataSet.GISDataModelCode

            ' Set the Action Properties










            lReturn = CType(FormatActionXMLSecurity(v_sDataModelCode:=CStr(v_sDataModelCode), v_sBusinessTypeCode:=v_sBusinessTypeCode, v_lAction:=iGISSharedConstants.GISDSActionSecurityRegisterUser, v_sSellerGUID:="", r_sActionXML:=sActionXML, v_sForename:=CStr(v_sForename), v_sSurname:=CStr(v_sSurname), v_sMothersMaidenName:=CStr(v_sMothersMaidenName), v_sDateOfBirth:=CStr(v_sDateOfBirth), v_sEmailAddress:=CStr(v_sEmailAddress), v_sMemorableDate:=CStr(v_sMemorableDate), v_sAQuestion:=CStr(v_sAQuestion), v_sTheAnswer:=CStr(v_sTheAnswer), v_sCurrentRenewalDate:=CStr(v_sCurrentRenewalDate), v_sTitle:=v_sTitle, v_sMaritalStatusCode:=v_sMaritalStatusCode, v_sAddress1:=v_sAddress1, v_sAddress2:=v_sAddress2, v_sAddress3:=v_sAddress3, v_sAddress4:=v_sAddress4, v_sPostcode:=v_sPostcode, v_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Send and Process The Command
            lReturn = CType(ProcessActionViaHTTP(v_oDataSet:=m_oDataSet, v_sActionXML:=sActionXML, r_sActionReturnXML:=sActionReturnXML), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check the ActionReturn Value
            ' Unformat the Action Return XML
            lReturn = CType(UnFormatActionReturnXMLSecurity(v_sActionReturnXML:=sActionReturnXML, r_lReturnValue:=lReturnValue, r_sUserID:=sUserID, r_sPassword:=sPassword, r_lPartyCnt:=lTemp), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_sUserID = sUserID
            r_sPassword = sPassword
            r_sPartyCnt = lTemp

            ' Check the Return Value
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturnValue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RegisterUser Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RegisterUser", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' RFC200700 - Return the PMUserID if the login is a TPA
    ' RFC310700 - Added return of Forename, surname, DOB & Email
    ' RFC050900 - Added BusinessTypeCode Parameter as this will be needed by Xelector multi channels
    ' RFC050900 - Added AdditonalDataArray. This is a array of name/value
    '             pairs, that can be used in the future to pass to OR return
    '             extra data from the Back Office Mapper, without need to change
    '             the method interface.
    Public Function LoginUser(ByVal v_sDataModelCode As Object, ByVal v_sUserID As Object, ByVal v_sPassword As Object, ByRef r_lPartyCnt As Object, Optional ByRef r_lPMUserID As Object = Nothing, Optional ByRef r_sForename As Object = Nothing, Optional ByRef r_sSurname As Object = Nothing, Optional ByRef r_dtDateOfBirth As Object = Nothing, Optional ByRef r_sEmailAddress As Object = Nothing, Optional ByVal v_sBusinessTypeCode As Object = "", Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer


        'Dim lReturn As Long
        'Dim sActionXML As String
        'Dim sDataModelCode As String
        '
        'Dim sActionReturnXML As String
        'Dim lReturnValue As Long
        'Dim lPartyCnt As Long
        'Dim lPMUserID As Long
        'Dim sForename As String
        'Dim sSurname As String
        'Dim dtDateOfBirth As Date
        'Dim sEmailAddress As String

        Dim result As Integer = 0
        Try


            'LoginUser = PMTrue

            'sDataModelCode = m_oDataSet.GISDataModelCode

            ' Set the Action Properties
            '    lReturn = FormatActionXMLSecurity( _
            ''        v_sDataModelCode:=v_sDataModelCode, _
            ''        v_sBusinessTypeCode:=v_sBusinessTypeCode, _
            ''        v_lAction:=GISDSActionSecurityLoginUser, _
            ''        v_sSellerGUID:="", _
            ''        r_sActionXML:=sActionXML, _
            ''        v_sUserID:=v_sUserID, _
            ''        v_sPassword:=v_sPassword, _
            ''        v_vAdditionalDataArray:=r_vAdditionalDataArray)
            '
            '    If (lReturn <> PMTrue) Then
            '        LoginUser = PMFalse
            '        Exit Function
            '    End If
            '
            '    ' Send and Process The Command
            '    lReturn = ProcessActionViaHTTP( _
            ''        v_oDataSet:=m_oDataSet, _
            ''        v_sActionXML:=sActionXML, _
            ''        r_sActionReturnXML:=sActionReturnXML)
            '    If (lReturn <> PMTrue) Then
            '        LoginUser = PMFalse
            '        Exit Function
            '    End If
            '
            '    ' Check the ActionReturn Value
            '    ' Unformat the Action Return XML
            '    lReturn = UnFormatActionReturnXMLSecurity( _
            ''        v_sActionReturnXML:=sActionReturnXML, _
            ''        r_lReturnValue:=lReturnValue, _
            ''        r_lPartyCnt:=lPartyCnt, _
            ''        r_lPMUserID:=lPMUserID, _
            ''        r_sForename:=sForename, _
            ''        r_sSurname:=sSurname, _
            ''        r_dtDateOfBirth:=dtDateOfBirth, _
            ''        r_sEmailAddress:=sEmailAddress)
            '
            '    If (lReturn <> PMTrue) Then
            '        LoginUser = PMFalse
            '        Exit Function
            '    End If
            '
            '    r_lPartyCnt = lPartyCnt
            '    r_lPMUserID = lPMUserID
            '    r_sForename = sForename
            '    r_sSurname = sSurname
            '    r_dtDateOfBirth = dtDateOfBirth
            '    r_sEmailAddress = sEmailAddress
            '
            '    ' Check the Return Value
            '    If (lReturnValue <> PMTrue) Then
            '        LoginUser = lReturnValue
            '        Exit Function
            '    End If

            Return GISCall("Security", "LoginUser", v_sDataModelCode, v_sBusinessTypeCode, v_sUserID, v_sPassword, r_lPartyCnt, r_lPMUserID, r_sSurname, r_sForename, r_dtDateOfBirth, r_sEmailAddress, r_vAdditionalDataArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginUser Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoginUser", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '**** START CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:42   ****
    '**** Added this new LoginAgent call to support Agents On Line
    Public Function LoginAgent(ByVal v_sDataModelCode As Object, ByVal v_sBusinessTypeCode As Object, ByVal v_sUsername As Object, ByVal v_sPassword As Object, ByRef r_lAgentCnt As Object, ByRef r_lPMUserID As Object, Optional ByRef r_bUnrestrictedSearch As Object = Nothing, Optional ByRef r_dtPasswordChangeDate As Object = Nothing, Optional ByRef r_dtLastlogin As Object = Nothing, Optional ByRef r_sForename As Object = Nothing, Optional ByRef r_sSurname As Object = Nothing, Optional ByRef r_sEmailAddress As Object = Nothing, Optional ByRef r_iLanguageId As Object = Nothing, Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try


            Return GISCall("Security", "LoginAgent", v_sDataModelCode, v_sBusinessTypeCode, v_sUsername, v_sPassword, r_lAgentCnt, r_lPMUserID, r_bUnrestrictedSearch, r_dtPasswordChangeDate, r_dtLastlogin, r_sForename, r_sSurname, r_sEmailAddress, r_iLanguageId, r_vAdditionalDataArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoginAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoginAgent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '****   END CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:42   ****

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    '**** START CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:42   ****
    '**** Added this new LogoffAgent call to support Agents On Line
    Public Function LogoffAgent(ByVal v_sDataModelCode As Object, ByVal v_sBusinessTypeCode As Object, ByVal v_sUsername As Object, Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try


            Return GISCall("Security", "LogoffAgent", v_sDataModelCode, v_sBusinessTypeCode, v_sUsername, r_vAdditionalDataArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LogoffAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LogoffAgent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '****   END CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:42   ****

    '**** START CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:43   ****
    '**** Added this new UpdateAgentLogonDetails call to support Agents On Line
    Public Function UpdateAgentLogonDetails(ByVal v_sDataModelCode As Object, ByVal v_sBusinessTypeCode As Object, ByVal v_sUsername As Object, ByVal v_sPassword As Object, ByVal v_sNewPassword As Object, Optional ByRef r_vAdditionalDataArray As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try


            Return GISCall("Security", "UpdateAgentLogonDetails", v_sDataModelCode, v_sBusinessTypeCode, v_sUsername, v_sPassword, v_sNewPassword, r_vAdditionalDataArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAgentLogonDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAgentLogonDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '****   END CHANGES - Changed By: AAB  - Changed On: 16-Sep-2002 16:43   ****
End Class
