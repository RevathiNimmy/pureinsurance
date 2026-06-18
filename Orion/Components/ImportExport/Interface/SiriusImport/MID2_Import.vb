Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Xml
Imports System.Xml.Xsl
Imports System.Xml.XPath
Imports System.Xml.Serialization
Imports System.Text


Friend NotInheritable Class MID2_Import : Inherits ImportBase

#Region "Private Variables"

    Private m_sBatchRefNum As String = "0"
    Private m_nNoOfTotalRecords As Integer
    Private m_nNoOfRejections As Integer
    Private m_nSourceId As Integer

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Specifies the batch code for this interface
    ''' </summary>
    Public Overrides ReadOnly Property BatchCode() As String
        Get
            Return "MID2"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the active interface name
    ''' </summary>
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "MID2_IMPORT"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the number of records in batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfTotalRecords() As Integer
        Get
            Return m_nNoOfTotalRecords
        End Get
        Set(ByVal value As Integer)
            m_nNoOfTotalRecords = value
        End Set
    End Property

    ''' <summary>
    ''' Specifies the no of rejected records in the batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfRejections() As Integer
        Get
            Return m_nNoOfRejections
        End Get
        Set(ByVal value As Integer)
            m_nNoOfRejections = value
        End Set
    End Property

#End Region

#Region "Object Creater"

    Public Sub New(ByVal oXML As XmlDocument)
        MyBase.New(oXML)
    End Sub
#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Update MID policy record with the import data
    ''' </summary>
    ''' <param name="sInsuranceRef"></param>
    ''' <param name="iBatchRef"></param>
    ''' <param name="iPPCC"></param>
    ''' <param name="iExpectedPPCC"></param>
    ''' <param name="sErrorRef"></param>
    ''' <param name="sErrors"></param>
    ''' <remarks></remarks>
    Private Sub UpdateMID2Policy(ByVal sInsuranceRef As String, ByVal sBatchRef As String, ByVal iPPCC As Integer, _
                                 ByVal iExpectedPPCC As Integer, ByVal sErrorRef As String, ByVal sErrors As String)
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue

        ' Add parameters
        m_oDatabase.Parameters.Clear()
        AddParameterLite(m_oDatabase, "InsuranceRef", sInsuranceRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
        AddParameterLite(m_oDatabase, "Batch_Ref", Convert.ToString(sBatchRef), PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
        AddParameterLite(m_oDatabase, "PPCC", iPPCC, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)
        AddParameterLite(m_oDatabase, "Expected_PPCC", iExpectedPPCC, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)
        AddParameterLite(m_oDatabase, "Error_Ref", sErrorRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
        AddParameterLite(m_oDatabase, "Errors", sErrors, PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
        AddParameterLite(m_oDatabase, "source_id", m_nSourceId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, False)

        ' Execute command
        nReturn = m_oDatabase.SQLAction("spu_MID2_Policy_Import_Failiure", "MID2 Import", True)
        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute ‘spu_MID2_Policy_Import_Failiure?")
        End If
    End Sub

    ''' <summary>
    ''' Update MID vehicle record with the import data
    ''' </summary>
    ''' <param name="nBatchRef"></param>
    ''' <param name="sInsuranceRef"></param>
    ''' <param name="sRegistrationNumber"></param>
    ''' <param name="sErrorRef"></param>
    ''' <param name="sErrors"></param>
    ''' <remarks></remarks>
    Private Sub UpdateMID2Vehicle(ByVal sBatchRef As String, ByVal sInsuranceRef As String, _
                                    ByVal sRegistrationNumber As String, _
                                    ByVal sErrorRef As String, ByVal sErrors As String)
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue

        ' Add parameters
        m_oDatabase.Parameters.Clear()
        AddParameterLite(m_oDatabase, "Batch_Ref", Convert.ToString(sBatchRef), PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
        AddParameterLite(m_oDatabase, "InsuranceRef", sInsuranceRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
        AddParameterLite(m_oDatabase, "Registration_Number", sRegistrationNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
        AddParameterLite(m_oDatabase, "Error_Ref", sErrorRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
        AddParameterLite(m_oDatabase, "Errors", sErrors, PMEParameterDirection.PMParamInput, PMEDataType.PMString, False)
        AddParameterLite(m_oDatabase, "source_id", m_nSourceId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, False)

        ' Execute command
        nReturn = m_oDatabase.SQLAction("spu_MID2_Vehicle_Import_Failiure", "MID2 Import", True)
        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute ‘spu_MID2_Vehicle_Import_Failiure?")
        End If
    End Sub

    ''' <summary>
    ''' Fetch the branch id for the insurance reference
    ''' </summary>
    ''' <param name="sInsuranceRef"></param>
    ''' <param name="vResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetBranchIDForPolicy(ByVal sInsuranceRef As String, ByRef r_aoResult(,) As Object)
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        ' Add parameters
        AddParameterLite(m_oDatabase, "insurance_ref", sInsuranceRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)

        ' Execute command
        nResult = m_oDatabase.SQLSelect("spu_MID_GetBranchIdOfPolicy", "Confirm Batch", True, vResultArray:=r_aoResult)
        If nResult <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute 'spu_MID_GetBranchIdOfPolicy'")
        End If
        Return nResult
    End Function

#End Region

#Region "Protected Methods"

    ''' <summary>
    ''' Processes the import record
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub ProcessElement()
        ' Get parameters
        NoOfTotalRecords += 1
        Dim sInsuranceRef As String = GetAttribute("insurance_ref").Trim()
        Dim sRegistrationNumber As String = GetAttribute("registration_number").Trim()
        Dim nExpectedPPCC As Integer
        Dim nPPCC As Integer
        Dim sErrorRef As String = GetAttribute("error_reference").Trim()
        Dim sErrors As String = GetAttribute("errors").Trim()

        If m_oXML.GetHeaderAttribute("batch_reference") Is Nothing OrElse Trim(m_oXML.GetHeaderAttribute("batch_reference")) = "" Then
            m_sBatchRefNum = "0"
        Else
            m_sBatchRefNum = m_oXML.GetHeaderAttribute("batch_reference")
        End If
        If GetAttribute("pppc") Is Nothing OrElse Trim(GetAttribute("pppc")) = "" Then
            nPPCC = 0
        Else
            nPPCC = CInt(GetAttribute("pppc"))
        End If
        If GetAttribute("expected_pppc") Is Nothing OrElse Trim(GetAttribute("expected_pppc")) = "" Then
            nExpectedPPCC = 0
        Else
            nExpectedPPCC = CInt(GetAttribute("expected_pppc"))
        End If

        Dim aoResultArray(,) As Object = Nothing
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue

        nReturn = GetBranchIDForPolicy(sInsuranceRef, aoResultArray)
        If nReturn <> PMEReturnCode.PMTrue OrElse aoResultArray Is Nothing OrElse aoResultArray Is String.Empty Then
            OutputLine(String.Format("No branch could be associated with this policy"))
            Exit Sub
        End If

        m_nSourceId = Convert.ToInt32(aoResultArray(0, aoResultArray.GetLowerBound(1)).ToString())

        If sRegistrationNumber = "" Then
            UpdateMID2Policy(sInsuranceRef, m_sBatchRefNum, nPPCC, nExpectedPPCC, sErrorRef, sErrors)
        Else
            UpdateMID2Vehicle(m_sBatchRefNum, sInsuranceRef, sRegistrationNumber, sErrorRef, sErrors)
        End If
    End Sub

    ''' <summary>
    ''' Finalise the import by updating the records not part of the error impo
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub FinaliseImport()
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue
        If m_sBatchRefNum = "0" Then
            If m_oXML.GetHeaderAttribute("batch_reference") Is Nothing OrElse Trim(m_oXML.GetHeaderAttribute("batch_reference")) = "" Then
                m_sBatchRefNum = "0"
            Else
                m_sBatchRefNum = m_oXML.GetHeaderAttribute("batch_reference")
            End If
        End If
        Dim nSupplierId, nSiteNumber, nIsLoaded, nIsReceived As Integer
        If m_oXML.GetHeaderAttribute("supplier_id") Is Nothing OrElse Trim(m_oXML.GetHeaderAttribute("supplier_id")) = "" Then
            nSupplierId = 0
        Else
            nSupplierId = m_oXML.GetHeaderAttribute("supplier_id")
        End If
        If m_oXML.GetHeaderAttribute("site_number") Is Nothing OrElse Trim(m_oXML.GetHeaderAttribute("site_number")) = "" Then
            nSiteNumber = 0
        Else
            nSiteNumber = m_oXML.GetHeaderAttribute("site_number")
        End If
        If m_oXML.GetHeaderAttribute("is_loaded") Is Nothing OrElse Trim(m_oXML.GetHeaderAttribute("is_loaded")) = "" Then
            nIsLoaded = 0
        Else
            nIsLoaded = m_oXML.GetHeaderAttribute("is_loaded")
        End If
        If m_oXML.GetHeaderAttribute("is_received") Is Nothing OrElse Trim(m_oXML.GetHeaderAttribute("is_received")) = "" Then
            nIsReceived = 0
        Else
            nIsReceived = m_oXML.GetHeaderAttribute("is_received")
        End If
        Try
            m_oDatabase.Parameters.Clear()
            AddParameterLite(m_oDatabase, "Batch_Ref", m_sBatchRefNum, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            AddParameterLite(m_oDatabase, "source_id", m_nSourceId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)
            AddParameterLite(m_oDatabase, "supplier_id", nSupplierId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)
            AddParameterLite(m_oDatabase, "site_number", nSiteNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)
            AddParameterLite(m_oDatabase, "is_loaded", nIsLoaded, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)
            AddParameterLite(m_oDatabase, "is_received", nIsReceived, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, False)

            nReturn = m_oDatabase.SQLAction("spu_MID2_Import_Finalise", "Import Finalise", True)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execure 'spu_MID2_Import_Finalise'")
            End If

        Catch ex As Exception
            Throw New ApplicationException("Unable to process import finalise.", ex)
        End Try

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    ''' <summary>
    ''' Update batch Status
    ''' </summary>
    Protected Overrides Sub UpdateBatchStatus()
        UpdateImportBatchStatus(kBatchStatusComplete, NoOfTotalRecords, NoOfRejections)
    End Sub

#End Region

End Class
