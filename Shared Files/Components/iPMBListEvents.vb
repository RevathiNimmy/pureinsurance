Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
<System.Runtime.InteropServices.ProgId("iPMBListEvents_NET.iPMBListEvents")> _
 Public Module iPMBListEvents
	
	Private Const ACClass As String = "iPMBListEvents"
    ' Main public constant for all functions
    ' to identify which application this is.

	Private m_lReturn As gPMConstants.PMEReturnCode
    'Added by Deepak Sharma on 4/21/2010 10:13:47 AM refer developer guide no. 

    <ThreadStatic()> _
    Public g_oObjectManager As Object = CreateLateBoundObject("bObjectManager.ObjectManager")



    ' ***************************************************************** '
    '
    ' Name: ShowEvents
    '
    ' Description:
    '
    ' History: 02/10/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function ShowEvents(ByVal v_lPartyCnt As Integer, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lClaimCnt As Integer = 0, Optional ByVal v_sInsuranceRef As String = "", Optional ByVal v_sClaimRef As String = "", Optional ByVal v_sTransactionType As String = "", Optional ByVal v_lAccountKey As Integer = 0, Optional ByVal v_bSearchOnPartyCnt As Boolean = True, Optional ByVal v_sSource_App As String = "", Optional ByVal v_lBaseClaimId As Integer = 0, Optional ByVal v_lCaseID As Integer = 0, Optional ByVal v_sCaseNumber As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Dim oListEvents As iPMBListEvents.Interface
            Dim oListEvents As Object
            'Set oListEvents = CreateNewObject("iPMBListEvents.Interface")

            Dim temp_oListEvents As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oListEvents, sClassName:="iPMBListEvents.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oListEvents = temp_oListEvents

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'CMG/PB 20022003 Bug Fix 1838, Show all events for the policy,not just this client
            'This will then show all Lead Client and Corresponance Flag changes
            ' Alix - Added an optional parameter to check if we search on the client or not
            If v_bSearchOnPartyCnt Then

                oListEvents.PartyCnt = v_lPartyCnt
            End If



            oListEvents.InsuranceFolderCnt = v_lInsuranceFolderCnt

            oListEvents.InsuranceFileCnt = v_lInsuranceFileCnt

            oListEvents.ClaimCnt = v_lClaimCnt

            oListEvents.InsuranceRef = v_sInsuranceRef

            oListEvents.ClaimRef = v_sClaimRef

            oListEvents.TransactionType = v_sTransactionType

            oListEvents.BaseClaimId = v_lBaseClaimId

            oListEvents.CaseID = v_lCaseID

            oListEvents.CaseNumber = v_sCaseNumber

            'SMJB CQ1620 Pass party count through
            If v_sSource_App.ToUpper() = "IACTFINDTRANSACTION" Then

                oListEvents.AccountKey = v_lPartyCnt
            Else

                oListEvents.AccountKey = v_lAccountKey
            End If

            'S4B Claims Enhancements R&D 2005

            oListEvents.EnableDefaultedFields = v_sSource_App.ToUpper() = "ICLMRISKDETAILS"

            'Need Not do this.. You are resetting the transactiontype property.
            'Anyway Creating an object does initialise this ----Amit

            '    m_lReturn = oListEvents.Initialise
            '    If m_lReturn <> PMTrue Then
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="oListEvents.Initialise Failed", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="ShowEvents"
            '        ShowEvents = PMFalse
            '        Set oListEvents = Nothing
            '        Exit Function
            '    End If


            m_lReturn = oListEvents.Start
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oListEvents.Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowEvents")
                result = gPMConstants.PMEReturnCode.PMFalse
                oListEvents = Nothing
                Return result
            End If


            oListEvents.Dispose()

            oListEvents = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowEvents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateNewObject
    '
    ' Description:
    '
    ' History: 02/10/2002 SJ - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CreateNewObject) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CreateNewObject(ByVal v_sClassName As String) As Object
    '
    'Dim result As Object = Nothing
    'Try 
    '
    '
    'Return Activator.CreateInstance(System.Reflection.Assembly.GetAssembly(Type.GetType(v_sClassName + "," + v_sClassName.Substring(0, v_sClassName.LastIndexOf(".")))).FullName, v_sClassName).Unwrap()
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object " & v_sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateNewObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '


    '
    'Return result


End Module