Imports Microsoft.Web.Services3.Security.Tokens
Imports System.Web.HttpContext
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Xml.Serialization
Imports System.IO
Imports System.Xml
Imports System.Runtime.Serialization.Formatters
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Xml.XmlReader
Imports System 'added by AJU
Imports System.Data.SqlClient  'added by AJU
Imports System.Data  'added by AJU
Imports System.Configuration   'added by AJU


Public Module SAMHelper

    Const HTMLNewLine As String = "<BR />" ' Vivek: added for easy formatting

    Public Function GetUserToken(ByVal sUserName As String, _
                            ByVal sPassword As String) As UsernameToken
        'create usertoken from username and a hash of the password
        Return New UsernameToken(sUserName, SiriusFS.SAM.Client.Security.ComputeSiriusLegacyHash(sPassword), PasswordOption.SendHashed)
    End Function


    Public Function GetMessageFromSamError(ByVal oSamErrors As SAMError()) As String
        'extracts messages from an array of SAM errors
        Dim sMessage As String = String.Empty
        Dim oError As SAMError
        For Each oError In oSamErrors
            'sMessage += oError.ToString
            sMessage += GetErrorDetails(oError).Replace(vbNewLine, HTMLNewLine) + HTMLNewLine ' Vivek: added to get proper details
        Next
        Return sMessage
    End Function

 Public Function WriteToLog(ByVal Ses As SessionState.HttpSessionState, ByVal PageName As String, ByVal ServiceName As String, ByVal WebMethodName As String, ByVal CallTime As Date, ByVal ResponseTime As Date) As Date
        'Dim flWriter As FileStream
        'Dim stWriter As StreamWriter
        'flWriter = File.Open("C:/Log.txt", FileMode.Append, FileAccess.Write)
        'stWriter = New StreamWriter(flWriter)
        'stWriter.WriteLine(Ses.SessionID.ToString() + "," + PageName + "," + ServiceName + "," + WebMethodName + "," + CallTime.ToString + "," + ResponseTime.ToString + "," + (ResponseTime - CallTime).TotalMilliseconds.ToString)
        'stWriter.Flush()
        'stWriter.Close()
        'flWriter.Close()
'<All the below line need to be commeted because it injects the log to the seperate DB called THLOG DB>
'Till use the specified line Ajuz code to write to DB ends here
' SO that it produce only flate file in C:\log.txt

'code to write to DB by AJU
        'Dim con As SqlConnection = Nothing
        'Dim cmd As SqlCommand = Nothing
        'Dim cmdd As SqlCommand = Nothing
        'Dim sqlConn As SqlConnection
        'Dim strConnection As String


        '     strConnection = System.Configuration.ConfigurationManager.AppSettings.Item("ConnectionString").ToString()
        '      sqlConn = New SqlConnection(strConnection)
        '        sqlConn.Open()
        '        Dim sql As SqlDataReader
        '        Dim id As String = Ses.SessionID.ToString()
        '        Dim page As String = PageName
        '        Dim service As String = ServiceName
        '        Dim method As String =  WebMethodName
        '        Dim start As String = CallTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt")
        '        Dim ends As String = ResponseTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt")
        '        Dim duration As String =  ResponseTime.Subtract(CallTime).TotalMilliseconds.ToString()
        '        Dim param As New SqlParameter("@sessionid", SqlDbType.VarChar, 50)
        '        param.Value = id
        '        Dim param1 As New SqlParameter("@pagename", SqlDbType.VarChar, 50)
        '        param1.Value = page
        '        Dim param2 As New SqlParameter("@webservice", SqlDbType.VarChar, 50)
        '        param2.Value = service
        '        Dim param3 As New SqlParameter("@webmethod", SqlDbType.VarChar, 50)
        '        param3.Value = method
        '        Dim param4 As New SqlParameter("@starttime",  SqlDbType.VarChar, 50)
        '        param4.Value = start
        '        Dim param5 As New SqlParameter("@endtime",  SqlDbType.VarChar, 50)
        '        param5.Value = ends
        '        Dim param6 As New SqlParameter("@duration",  SqlDbType.VarChar, 50)
        '        param6.Value = duration
        '        cmd = New SqlCommand("DB_Logs", sqlConn)
        '        cmd.CommandType = CommandType.StoredProcedure
        '        cmd.Parameters.Add (param)
        '       cmd.Parameters.Add (param1)
        '       cmd.Parameters.Add (param2)
        '       cmd.Parameters.Add (param3)
        '        cmd.Parameters.Add (param4)
        '        cmd.Parameters.Add (param5)
        '        cmd.Parameters.Add (param6)
        '        cmd.ExecuteNonQuery()
        '        sqlConn.Close()

' Ajuz code to write to DB ends here
        Return Date.Now
    End Function
    ''' <summary>
    ''' This method is used to get the XML representation of the response object. (or any other object)
    ''' </summary>
    ''' <param name="obj">the object to be serialized</param>
    ''' <returns>xml string</returns>
    ''' <remarks>Added by Vivek</remarks>
    Public Function XMLString(ByVal obj As Object) As String

        ' Create an instance of the XmlSerializer class;
        ' specify the type of object to serialize.
        Dim serializer As New XmlSerializer(obj.GetType())

        Dim form As New BinaryFormatter

        Dim writer As New StringWriter

        Try
            serializer.Serialize(writer, obj)
        Catch ex As Exception
            writer.WriteLine("There was an error while serializing object. Error Details: " + ex.Message)
        End Try

        Dim message As String = writer.ToString()

        writer.Close()

        Return message

    End Function


    ''' <summary>
    ''' Currently this method is used to add a delete link as the first column in GridView.
    ''' We can modify it to set more properties of the grid view
    ''' </summary>
    ''' <param name="gv">GridView in which delete column to be added</param>
    ''' <remarks>Added by Vivek</remarks>
    Public Sub setGridView(ByRef gv As GridView)

        Dim col As WebControls.CommandField
        col = New CommandField()
        col.ButtonType = ButtonType.Link
        col.DeleteText = "Delete"
        col.HeaderText = "Delete"
        col.ShowDeleteButton = True
        ' gv.Columns.Add(col)
        gv.Columns.Insert(0, col)
    End Sub

    ''' <summary>
    ''' This method is used to show text field in each cell of the GridView
    ''' </summary>
    ''' <param name="gv"></param>
    ''' <remarks>Added by Vivek</remarks>
    Public Sub setTextFieldsInGridView(ByRef gv As GridView)

        For Each row As GridViewRow In gv.Rows
            For Each cell As DataControlFieldCell In row.Cells

                Dim txt As New TextBox()
                txt.Text = cell.Text
                cell.Controls.Add(txt)
            Next
        Next
    End Sub

    ''' <summary>
    ''' This method is used to add a new element at the end of an array
    ''' </summary>
    ''' <typeparam name="DataType">DataType of the array elements</typeparam>
    ''' <param name="vArray">array object</param>
    ''' <param name="oNewEle">new element to be appended</param>
    ''' <remarks>Added by Vivek</remarks>
    Public Sub AddToArray(Of DataType)(ByRef vArray() As DataType, ByVal oNewEle As DataType)

        ReDim Preserve vArray(0 To UBound(vArray) + 1)
        vArray(UBound(vArray)) = oNewEle

    End Sub

    ''' <summary>
    ''' This method is used to remove a given element from the array
    ''' </summary>
    ''' <typeparam name="DataType">DataType of the array elements</typeparam>
    ''' <param name="array">array object</param>
    ''' <param name="ind">index of the element to be removed</param>
    ''' <remarks>Added by Vivek</remarks>
    Public Sub RemoveFromArray(Of DataType)(ByRef array() As DataType, ByVal ind As Integer)
        Dim tmpArray() As DataType
        ReDim tmpArray(0 To UBound(array))

        array.CopyTo(tmpArray, 0)

        ReDim array(0 To UBound(array) - 1)

        Dim iOld As Integer = 0
        Dim iNew As Integer = 0
        For Each ele As DataType In tmpArray
            If iOld <> ind Then
                array(iNew) = tmpArray(iOld)
                iNew = iNew + 1
            End If
            iOld = iOld + 1
        Next

    End Sub

    ''' <summary>
    ''' This method returns the details of given SAMError object based on its type
    ''' </summary>
    ''' <param name="oError">Error object</param>
    ''' <returns>error details</returns>
    ''' <remarks>Added by Vivek</remarks>
    Public Function GetErrorDetails(ByVal oError As SAMError) As String
        Dim strDetails As String = Nothing

        Select Case oError.GetType.FullName
            Case GetType(SAMErrorInvalidData).FullName
                Dim oErr As SAMErrorInvalidData = DirectCast(oError, SAMErrorInvalidData)
                strDetails = String.Format("{0}: {1}={2} " + vbNewLine + "Err Code: {3}, Reason: {4}", oErr.Description, oErr.FieldName, oErr.SuppliedValue, oErr.Code, oErr.Reason)

            Case GetType(SAMErrorBusinessRule).FullName
                Dim oErr As SAMErrorBusinessRule = DirectCast(oError, SAMErrorBusinessRule)
                strDetails = String.Format("{0} " + vbNewLine + "Err Code: {1}, Details: {2}", oErr.Description, oErr.Code, oErr.Detail)

            Case GetType(SAMErrorFatal).FullName
                Dim oErr As SAMErrorFatal = DirectCast(oError, SAMErrorFatal)
                strDetails = String.Format("{0}", oErr.Type)

            Case Else
                strDetails = oError.ToString
        End Select

        Return strDetails
    End Function

End Module

Public Class SamResponseException
    Inherits System.ApplicationException

    Sub New(ByVal message As String)
        'constructor to allow simple SAM exception to be thrown
        MyBase.New(message)
    End Sub

    Sub New(ByVal oSamErrors As SAMError())
        'take SAM error array, parse the individual messages and place them all in the exception message property
        MyBase.New(GetMessageFromSamError(oSamErrors))
    End Sub
End Class

Public Class RecoveryDetails
    Private initialreserveField As Decimal
    Private revisionamountField As Decimal
    Private thisRevisionField As Decimal
    Private totalreserveField As Decimal
    Private descriptionfield As String
    Private isSalvageField As Integer
    Private typeCodeField As String
    Private partyTypeCodeField As String
    Private partyCodeField As String
    Private partyTypeKeyField As Integer
    Private partyKeyField As Integer
    Private receiptedAmountField As Decimal
    Private receiptedTaxAmountField As Decimal
    Private baseRecoveryKeyField As Integer

    Public Property InitialReserve() As Decimal
        Get
            Return Me.initialreserveField
        End Get
        Set(ByVal value As Decimal)
            Me.initialreserveField = value

        End Set
    End Property
    Public Property RevisionAmount() As Decimal
        Get
            Return Me.revisionamountField
        End Get
        Set(ByVal value As Decimal)
            Me.revisionamountField = value

        End Set
    End Property
    Public Property ThisRevision() As Decimal
        Get
            Return Me.thisRevisionField
        End Get
        Set(ByVal value As Decimal)
            Me.thisRevisionField = value

        End Set
    End Property
    Public Property TotalReserve() As Decimal
        Get
            Return Me.totalreserveField
        End Get
        Set(ByVal value As Decimal)
            Me.totalreserveField = value

        End Set
    End Property

    Public Property Description() As String
        Get
            Return Me.descriptionfield
        End Get
        Set(ByVal value As String)
            Me.descriptionfield = value
        End Set
    End Property
    Public Property IsSalvage() As Integer
        Get
            Return Me.isSalvageField
        End Get
        Set(ByVal value As Integer)
            Me.isSalvageField = value
        End Set
    End Property
    Public Property TypeCode() As String
        Get
            Return Me.typeCodeField
        End Get
        Set(ByVal value As String)
            Me.typeCodeField = value
        End Set
    End Property
    Public Property PartyTypeCode() As String
        Get
            Return Me.partyTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.partyTypeCodeField = value
        End Set
    End Property
    Public Property PartyCode() As String
        Get
            Return Me.partyCodeField
        End Get
        Set(ByVal value As String)
            Me.partyCodeField = value
        End Set
    End Property
    Public Property PartyTypeKey() As Integer
        Get
            Return Me.partyTypeKeyField
        End Get
        Set(ByVal value As Integer)
            Me.partyTypeKeyField = value
        End Set
    End Property
    Public Property PartyKey() As Integer
        Get
            Return Me.partyKeyField
        End Get
        Set(ByVal value As Integer)
            Me.partyKeyField = value
        End Set
    End Property
    Public Property ReceiptedAmount() As Decimal
        Get
            Return Me.receiptedAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.receiptedAmountField = value

        End Set
    End Property
    Public Property ReceiptedAmountTax() As Decimal
        Get
            Return Me.receiptedTaxAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.receiptedTaxAmountField = value

        End Set
    End Property
    Public Property BaseRecoveryKey() As Integer
        Get
            Return Me.baseRecoveryKeyField
        End Get
        Set(ByVal value As Integer)
            Me.baseRecoveryKeyField = value
        End Set
    End Property
End Class
Public Class ReserveDetails
    Private initialreserveField As Decimal
    Private revisionamountField As Decimal
    Private thisRevisionField As Decimal
    Private currentreserveField As Decimal
    Private incurredField As Decimal
    Private sumIncurredField As Decimal
    Private averageField As Decimal
    Private descriptionfield As String
    Private typeCodeField As String
    Private baseResereveKeyField As Integer

    Public Property InitialReserve() As Decimal
        Get
            Return Me.initialreserveField
        End Get
        Set(ByVal value As Decimal)
            Me.initialreserveField = value

        End Set
    End Property
    Public Property RevisionAmount() As Decimal
        Get
            Return Me.revisionamountField
        End Get
        Set(ByVal value As Decimal)
            Me.revisionamountField = value

        End Set
    End Property
    Public Property ThisRevision() As Decimal
        Get
            Return Me.thisRevisionField
        End Get
        Set(ByVal value As Decimal)
            Me.thisRevisionField = value

        End Set
    End Property
    Public Property CurrentReserve() As Decimal
        Get
            Return Me.currentreserveField
        End Get
        Set(ByVal value As Decimal)
            Me.currentreserveField = value

        End Set
    End Property
    Public Property Incurred() As Decimal
        Get
            Return Me.incurredField
        End Get
        Set(ByVal value As Decimal)
            Me.incurredField = value
        End Set
    End Property
    Public Property SumIncurred() As Decimal
        Get
            Return Me.sumIncurredField
        End Get
        Set(ByVal value As Decimal)
            Me.sumIncurredField = value
        End Set
    End Property
    Public Property Average() As Decimal
        Get
            Return Me.averageField
        End Get
        Set(ByVal value As Decimal)
            Me.averageField = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return Me.descriptionfield
        End Get
        Set(ByVal value As String)
            Me.descriptionfield = value
        End Set
    End Property
    Public Property TypeCode() As String
        Get
            Return Me.typeCodeField
        End Get
        Set(ByVal value As String)
            Me.typeCodeField = value
        End Set
    End Property
    Public Property BaseResereveKey() As Integer
        Get
            Return Me.baseResereveKeyField
        End Get
        Set(ByVal value As Integer)
            Me.baseResereveKeyField = value
        End Set
    End Property
End Class

Public Class Peril

    Private perilTypeCodeField As String
    Private perilDescriptionField As String
    Private sumInsuredfield As Decimal
    Private reservesField() As ReserveDetails
    Private recoveriesField() As RecoveryDetails
    Private baseClaimPerilKeyfield As Integer

    Public Property PerilTypeCode() As String
        Get
            Return Me.perilTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.perilTypeCodeField = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return Me.perilDescriptionField
        End Get
        Set(ByVal value As String)
            Me.perilDescriptionField = value
        End Set
    End Property

    Public Property SumInsured() As Decimal
        Get
            Return Me.sumInsuredfield
        End Get
        Set(ByVal value As Decimal)
            Me.sumInsuredfield = value
        End Set
    End Property

    Public Property Reserves() As ReserveDetails()
        Get
            Return Me.reservesField
        End Get
        Set(ByVal value As ReserveDetails())
            Me.reservesField = value
        End Set
    End Property
    Public Property Recoveries() As RecoveryDetails()
        Get
            Return Me.recoveriesField
        End Get
        Set(ByVal value As RecoveryDetails())
            Me.recoveriesField = value
        End Set
    End Property
    Public Property BaseClaimPerilKey() As Integer
        Get
            Return Me.baseClaimPerilKeyfield
        End Get
        Set(ByVal value As Integer)
            Me.baseClaimPerilKeyfield = value
        End Set
    End Property
End Class
Public Class ClaimReceipt
    Private recoveryDescriptionField As String
    Private totalRecoveryField As Decimal
    Private receivedToDateField As Decimal
    Private receivedTodateTaxField As Decimal
    Private currentReserveField As Decimal
    Private thisReceiptInclTaxField As Decimal
    Private thisReceiptTaxField As Decimal
    Private netRecoveryField As Decimal
    Private baseRecoveryKeyField As Decimal
    Private recoveryPartyField As String
    Private isSalvageField As Integer




    Public Property RecoveryDescription() As String
        Get
            Return Me.recoveryDescriptionField
        End Get
        Set(ByVal value As String)
            Me.recoveryDescriptionField = value
        End Set
    End Property

    Public Property BaseRecoveryKey() As Integer
        Get
            Return Me.BaseRecoveryKeyField
        End Get
        Set(ByVal value As Integer)
            Me.BaseRecoveryKeyField = value

        End Set
    End Property
    Public Property TotalRecovery() As Decimal
        Get
            Return Me.totalRecoveryField
        End Get
        Set(ByVal value As Decimal)
            Me.totalRecoveryField = value

        End Set
    End Property

    Public Property ReceivedToDate() As Decimal
        Get
            Return Me.receivedToDateField
        End Get
        Set(ByVal value As Decimal)
            Me.receivedToDateField = value

        End Set
    End Property
    Public Property ReceivedToDateTax() As Decimal
        Get
            Return Me.receivedTodateTaxField
        End Get
        Set(ByVal value As Decimal)
            Me.receivedTodateTaxField = value
        End Set
    End Property

    Public Property CurrentReserve() As Decimal
        Get
            Return Me.currentReserveField
        End Get
        Set(ByVal value As Decimal)
            Me.currentReserveField = value

        End Set
    End Property
    Public Property ThisReceiptInclTax() As Decimal
        Get
            Return Me.thisReceiptInclTaxField

        End Get
        Set(ByVal value As Decimal)
            Me.thisReceiptInclTaxField = value
        End Set
    End Property
    Public Property ThisReceiptTax() As Decimal
        Get
            Return Me.thisReceiptTaxField

        End Get
        Set(ByVal value As Decimal)
            Me.thisReceiptTaxField = value
        End Set
    End Property
    Public Property NetReceipt() As Decimal
        Get
            Return Me.netRecoveryField
        End Get
        Set(ByVal value As Decimal)
            Me.netRecoveryField = value
        End Set
    End Property
    Public Property RecoveryParty() As String
        Get
            Return Me.recoveryPartyField
        End Get
        Set(ByVal value As String)
            Me.recoveryPartyField = value
        End Set
    End Property
    Public Property IsSalvage() As Integer
        Get
            Return Me.isSalvageField
        End Get
        Set(ByVal value As Integer)
            Me.isSalvageField = value
        End Set
    End Property



End Class

Public Class DocTemplate
    Private codeField As String
    Private descriptionField As String
    Public Property Code() As String
        Get
            Return Me.codeField
        End Get
        Set(ByVal value As String)
            Me.codeField = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return Me.descriptionField
        End Get
        Set(ByVal value As String)
            Me.descriptionField = value
        End Set
    End Property

End Class
