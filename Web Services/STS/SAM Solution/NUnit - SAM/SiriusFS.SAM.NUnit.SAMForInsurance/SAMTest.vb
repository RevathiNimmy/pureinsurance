Option Strict On

''' <summary>
''' Methods for administering SAM response tests in all other test classes.
''' </summary>
Friend NotInheritable Class SAMTest

#Region "Constructors"

    Private Sub New()
        ' This class cannot be instantiated.
    End Sub

#End Region

#Region "Public Shared Methods"

    ''' <summary>
    ''' Assert that the SAM method call succeeded.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    Public Shared Sub AssertCallSucceeded(ByVal oResponse As ProxyWS.SAMMethodResponseData)

        Assert.IsTrue( _
            Not HasHandlingInstanceID(oResponse) AndAlso _
            Not HasErrors(oResponse), _
            "SAM method was expected to succeed. " & GetDiagnosticMessage(oResponse))

    End Sub

    ''' <summary>
    ''' Assert that the SAM method call succeeded and .
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="ResultSet"> The result set of data returned from the SAM call.</param>
    Public Shared Sub AssertCallSucceededWithResults(ByVal oResponse As ProxyWS.SAMMethodResponseData, ByVal ResultSet As System.Array)

        Assert.IsTrue( _
            Not HasHandlingInstanceID(oResponse) AndAlso _
            Not HasErrors(oResponse) AndAlso _
            ResultSet IsNot Nothing AndAlso _
            (ResultSet.Length > 0), _
            "SAM method was expected succeed with results. " & GetDiagnosticMessage(oResponse))

    End Sub

    ''' <summary>
    ''' Assert that the SAM method call failed with an exception.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    Public Shared Sub AssertCallFailedWithException(ByVal oResponse As ProxyWS.SAMMethodResponseData)

        Assert.IsTrue( _
            HasHandlingInstanceID(oResponse) AndAlso _
            Not HasErrors(oResponse), _
            "SAM method was expected to fail with an exception. " & GetDiagnosticMessage(oResponse))

    End Sub

    ''' <summary>
    ''' Assert that the SAM method call failed with a collection of errors.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    Public Shared Sub AssertCallFailedWithErrors(ByVal oResponse As ProxyWS.SAMMethodResponseData)

        Assert.IsTrue( _
            HasHandlingInstanceID(oResponse) AndAlso _
            HasErrors(oResponse), _
            "SAM method was expected to fail with errors. " & GetDiagnosticMessage(oResponse))

    End Sub

    ''' <summary>
    ''' Assert that the SAM method call failed with a collection of errors.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="nErrors">The number of errors expected.</param>
    Public Shared Sub AssertCallFailedWithErrors(ByVal oResponse As ProxyWS.SAMMethodResponseData, _
        ByVal nErrors As Integer)

        Assert.IsTrue( _
            HasHandlingInstanceID(oResponse) AndAlso _
            HasErrors(oResponse), _
            "SAM method call was expected to fail with errors. " & GetDiagnosticMessage(oResponse))

        Assert.AreEqual(nErrors, oResponse.Errors.Length, "Errors collection has unexpected number of elements.")

    End Sub

    ''' <summary>
    ''' Assert that the SAM method call failed with a collection of Warnings.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    Public Shared Sub AssertCallSucceedededWithWarnings(ByVal oResponse As ProxyWS.BaseClaimResponseType)

        Assert.IsTrue( _
            Not HasHandlingInstanceID(oResponse) AndAlso _
            Not HasErrors(oResponse) AndAlso _
            HasWarnings(oResponse), _
            "SAM method was expected to fail with Warnings. " & GetDiagnosticMessage(oResponse))

    End Sub

    ''' <summary>
    ''' Assert that the SAM method call failed with a collection of Warnings.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="nWarnings">The number of Warnings expected.</param>
    Public Shared Sub AssertCallSucceedededWithWarnings(ByVal oResponse As ProxyWS.BaseClaimResponseType, _
        ByVal nWarnings As Integer)

        Assert.IsTrue( _
            Not HasHandlingInstanceID(oResponse) AndAlso _
            Not HasErrors(oResponse) AndAlso _
            HasWarnings(oResponse), _
            "SAM method call was expected to fail with Warnings. " & GetDiagnosticMessage(oResponse))

        Assert.AreEqual(nWarnings, oResponse.Warnings.Length, "Warnings collection has unexpected number of elements.")

    End Sub

    ''' <summary>
    ''' Assert that the errors collection contains a SAMErrorBusinessRule with specified property values.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="iError">The index into the errors collection.</param>
    Public Shared Function AssertErrorBusinessRule(ByVal oResponse As ProxyWS.SAMMethodResponseData, _
        ByVal iError As Integer) As ProxyWS.SAMErrorBusinessRule

        Assert.IsNotNull(oResponse.Errors, "Errors collection not populated.")
        Assert.Less(iError, oResponse.Errors.Length, "Errors(" & iError & ") not in the collection.")
        Assert.IsInstanceOfType(GetType(ProxyWS.SAMErrorBusinessRule), oResponse.Errors(iError), "Errors(" & iError & ") has unexpected type.")

        Dim oError As ProxyWS.SAMErrorBusinessRule = DirectCast(oResponse.Errors(iError), ProxyWS.SAMErrorBusinessRule)

        Return oError

    End Function

    ''' <summary>
    ''' Assert that the errors collection contains a SAMErrorBusinessRule with specified property values.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="iError">The index into the errors collection.</param>
    ''' <param name="nCode">Error property value expected.</param>
    Public Shared Function AssertErrorBusinessRule(ByVal oResponse As ProxyWS.SAMMethodResponseData, _
        ByVal iError As Integer, _
        ByVal nCode As Integer) As ProxyWS.SAMErrorBusinessRule

        Assert.IsNotNull(oResponse.Errors, "Errors collection not populated.")
        Assert.Less(iError, oResponse.Errors.Length, "Errors(" & iError & ") not in the collection.")
        Assert.IsInstanceOfType(GetType(ProxyWS.SAMErrorBusinessRule), oResponse.Errors(iError), "Errors(" & iError & ") has unexpected type.")

        Dim oError As ProxyWS.SAMErrorBusinessRule = DirectCast(oResponse.Errors(iError), ProxyWS.SAMErrorBusinessRule)

        Assert.AreEqual(nCode, oError.Code, "Errors(" & iError & ") has unexpected Code value.")

        Return oError

    End Function

    ''' <summary>
    ''' Assert that the errors collection contains a SAMErrorBusinessRule with specified property values.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="iError">The index into the errors collection.</param>
    ''' <param name="nCode">Error property value expected.</param>
    ''' <param name="sDescription">Error property value expected.</param>
    Public Shared Function AssertErrorBusinessRule(ByVal oResponse As ProxyWS.SAMMethodResponseData, _
        ByVal iError As Integer, _
        ByVal nCode As Integer, _
        ByVal sDescription As String) As ProxyWS.SAMErrorBusinessRule

        Assert.IsNotNull(oResponse.Errors, "Errors collection not populated.")
        Assert.Less(iError, oResponse.Errors.Length, "Errors(" & iError & ") not in the collection.")
        Assert.IsInstanceOfType(GetType(ProxyWS.SAMErrorBusinessRule), oResponse.Errors(iError), "Errors(" & iError & ") has unexpected type.")

        Dim oError As ProxyWS.SAMErrorBusinessRule = DirectCast(oResponse.Errors(iError), ProxyWS.SAMErrorBusinessRule)

        Assert.AreEqual(nCode, oError.Code, "Errors(" & iError & ") has unexpected Code value.")
        Assert.AreEqual(sDescription, oError.Description, "Errors(" & iError & ") has unexpected Description value.")

        Return oError

    End Function

    ''' <summary>
    ''' Assert that the errors collection contains a SAMErrorBusinessRule with specified property values.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="iError">The index into the errors collection.</param>
    ''' <param name="nCode">Error property value expected.</param>
    ''' <param name="sDescription">Error property value expected.</param>
    ''' <param name="sDetail">Error property value expected.</param>
    Public Shared Function AssertErrorBusinessRule(ByVal oResponse As ProxyWS.SAMMethodResponseData, _
        ByVal iError As Integer, _
        ByVal nCode As Integer, _
        ByVal sDescription As String, _
        ByVal sDetail As String) As ProxyWS.SAMErrorBusinessRule

        Assert.IsNotNull(oResponse.Errors, "Errors collection not populated.")
        Assert.Less(iError, oResponse.Errors.Length, "Errors(" & iError & ") not in the collection.")
        Assert.IsInstanceOfType(GetType(ProxyWS.SAMErrorBusinessRule), oResponse.Errors(iError), "Errors(" & iError & ") has unexpected type.")

        Dim oError As ProxyWS.SAMErrorBusinessRule = DirectCast(oResponse.Errors(iError), ProxyWS.SAMErrorBusinessRule)

        Assert.AreEqual(nCode, oError.Code, "Errors(" & iError & ") has unexpected Code value.")
        Assert.AreEqual(sDescription, oError.Description, "Errors(" & iError & ") has unexpected Description value.")
        Assert.AreEqual(sDetail, oError.Detail, "Errors(" & iError & ") has unexpected Detail value.")

        Return oError

    End Function

    ''' <summary>
    ''' Assert that the errors collection contains a SAMErrorInvalidData with specified property values.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="iError">The index into the errors collection.</param>
    Public Shared Function AssertErrorInvalidData(ByVal oResponse As ProxyWS.SAMMethodResponseData, _
        ByVal iError As Integer) As ProxyWS.SAMErrorInvalidData

        Assert.IsNotNull(oResponse.Errors, "Errors collection not populated.")
        Assert.Less(iError, oResponse.Errors.Length, "Errors(" & iError & ") not in the collection.")
        Assert.IsInstanceOfType(GetType(ProxyWS.SAMErrorInvalidData), oResponse.Errors(iError), "Errors(" & iError & ") has unexpected type.")

        Dim oError As ProxyWS.SAMErrorInvalidData = DirectCast(oResponse.Errors(iError), ProxyWS.SAMErrorInvalidData)

        Return oError

    End Function

    ''' <summary>
    ''' Assert that the errors collection contains a SAMErrorInvalidData with specified property values.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="iError">The index into the errors collection.</param>
    ''' <param name="nCode">Error property value expected.</param>
    Public Shared Function AssertErrorInvalidData(ByVal oResponse As ProxyWS.SAMMethodResponseData, _
        ByVal iError As Integer, _
        ByVal nCode As Integer) As ProxyWS.SAMErrorInvalidData

        Assert.IsNotNull(oResponse.Errors, "Errors collection not populated.")
        Assert.Less(iError, oResponse.Errors.Length, "Errors(" & iError & ") not in the collection.")
        Assert.IsInstanceOfType(GetType(ProxyWS.SAMErrorInvalidData), oResponse.Errors(iError), "Errors(" & iError & ") has unexpected type.")

        Dim oError As ProxyWS.SAMErrorInvalidData = DirectCast(oResponse.Errors(iError), ProxyWS.SAMErrorInvalidData)

        Assert.AreEqual(nCode, oError.Code, "Errors(" & iError & ") has unexpected Code value.")

        Return oError

    End Function

    ''' <summary>
    ''' Assert that the errors collection contains a SAMErrorInvalidData with specified property values.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="iError">The index into the errors collection.</param>
    ''' <param name="nCode">Error property value expected.</param>
    ''' <param name="sDescription">Error property value expected.</param>
    Public Shared Function AssertErrorInvalidData(ByVal oResponse As ProxyWS.SAMMethodResponseData, _
        ByVal iError As Integer, _
        ByVal nCode As Integer, _
        ByVal sDescription As String) As ProxyWS.SAMErrorInvalidData

        Assert.IsNotNull(oResponse.Errors, "Errors collection not populated.")
        Assert.Less(iError, oResponse.Errors.Length, "Errors(" & iError & ") not in the collection.")
        Assert.IsInstanceOfType(GetType(ProxyWS.SAMErrorInvalidData), oResponse.Errors(iError), "Errors(" & iError & ") has unexpected type.")

        Dim oError As ProxyWS.SAMErrorInvalidData = DirectCast(oResponse.Errors(iError), ProxyWS.SAMErrorInvalidData)

        Assert.AreEqual(nCode, oError.Code, "Errors(" & iError & ") has unexpected Code value.")
        Assert.AreEqual(sDescription, oError.Description, "Errors(" & iError & ") has unexpected Description value.")

        Return oError

    End Function

    ''' <summary>
    ''' Assert that the errors collection contains a SAMErrorInvalidData with specified property values.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="iError">The index into the errors collection.</param>
    ''' <param name="nCode">Error property value expected.</param>
    ''' <param name="sDescription">Error property value expected.</param>
    ''' <param name="sFieldName">Error property value expected.</param>
    Public Shared Function AssertErrorInvalidData(ByVal oResponse As ProxyWS.SAMMethodResponseData, _
        ByVal iError As Integer, _
        ByVal nCode As Integer, _
        ByVal sDescription As String, _
        ByVal sFieldName As String) As ProxyWS.SAMErrorInvalidData

        Assert.IsNotNull(oResponse.Errors, "Errors collection not populated.")
        Assert.Less(iError, oResponse.Errors.Length, "Errors(" & iError & ") not in the collection.")
        Assert.IsInstanceOfType(GetType(ProxyWS.SAMErrorInvalidData), oResponse.Errors(iError), "Errors(" & iError & ") has unexpected type.")

        Dim oError As ProxyWS.SAMErrorInvalidData = DirectCast(oResponse.Errors(iError), ProxyWS.SAMErrorInvalidData)

        Assert.AreEqual(nCode, oError.Code, "Errors(" & iError & ") has unexpected Code value.")
        Assert.AreEqual(sDescription, oError.Description, "Errors(" & iError & ") has unexpected Description value.")
        Assert.AreEqual(sFieldName, oError.FieldName, "Errors(" & iError & ") has unexpected FieldName value.")

        Return oError

    End Function

    ''' <summary>
    ''' Assert that the errors collection contains a SAMErrorInvalidData with specified property values.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="iError">The index into the errors collection.</param>
    ''' <param name="nCode">Error property value expected.</param>
    ''' <param name="sDescription">Error property value expected.</param>
    ''' <param name="sFieldName">Error property value expected.</param>
    ''' <param name="sSuppliedValue">Error property value expected.</param>
    Public Shared Function AssertErrorInvalidData(ByVal oResponse As ProxyWS.SAMMethodResponseData, _
        ByVal iError As Integer, _
        ByVal nCode As Integer, _
        ByVal sDescription As String, _
        ByVal sFieldName As String, _
        ByVal sSuppliedValue As String) As ProxyWS.SAMErrorInvalidData

        Assert.IsNotNull(oResponse.Errors, "Errors collection not populated.")
        Assert.Less(iError, oResponse.Errors.Length, "Errors(" & iError & ") not in the collection.")
        Assert.IsInstanceOfType(GetType(ProxyWS.SAMErrorInvalidData), oResponse.Errors(iError), "Errors(" & iError & ") has unexpected type.")

        Dim oError As ProxyWS.SAMErrorInvalidData = DirectCast(oResponse.Errors(iError), ProxyWS.SAMErrorInvalidData)

        Assert.AreEqual(nCode, oError.Code, "Errors(" & iError & ") has unexpected Code value.")
        Assert.AreEqual(sDescription, oError.Description, "Errors(" & iError & ") has unexpected Description value.")
        Assert.AreEqual(sFieldName, oError.FieldName, "Errors(" & iError & ") has unexpected FieldName value.")
        Assert.AreEqual(sSuppliedValue, oError.SuppliedValue, "Errors(" & iError & ") has unexpected SuppliedValue value.")

        Return oError

    End Function

    ''' <summary>
    ''' Assert that the Warnings collection contains a BaseClaimResponseTypeWarnings with specified property values.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="iWarning">The index into the Warnings collection.</param>
    Public Shared Function AssertWarning(ByVal oResponse As ProxyWS.BaseClaimResponseType, _
        ByVal iWarning As Integer) As ProxyWS.BaseClaimResponseTypeWarnings

        Assert.IsNotNull(oResponse.Warnings, "Warnings collection not populated.")
        Assert.Less(iWarning, oResponse.Warnings.Length, "Warnings(" & iWarning & ") not in the collection.")

        Dim oWarning As ProxyWS.BaseClaimResponseTypeWarnings = oResponse.Warnings(iWarning)

        Return oWarning

    End Function

    ''' <summary>
    ''' Assert that the Warnings collection contains a BaseClaimResponseTypeWarnings with specified property values.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="iWarning">The index into the Warnings collection.</param>
    ''' <param name="nCode">Warning property value expected.</param>
    Public Shared Function AssertWarning(ByVal oResponse As ProxyWS.BaseClaimResponseType, _
        ByVal iWarning As Integer, _
        ByVal nCode As Integer) As ProxyWS.BaseClaimResponseTypeWarnings

        Assert.IsNotNull(oResponse.Warnings, "Warnings collection not populated.")
        Assert.Less(iWarning, oResponse.Warnings.Length, "Warnings(" & iWarning & ") not in the collection.")

        Dim oWarning As ProxyWS.BaseClaimResponseTypeWarnings = oResponse.Warnings(iWarning)

        Assert.AreEqual(nCode, oWarning.Code, "Warnings(" & iWarning & ") has unexpected Code value.")

        Return oWarning

    End Function

    ''' <summary>
    ''' Assert that the Warnings collection contains a BaseClaimResponseTypeWarnings with specified property values.
    ''' </summary>
    ''' <param name="oResponse">The response proxy object to test.</param>
    ''' <param name="iWarning">The index into the errors collection.</param>
    ''' <param name="nCode">Warning property value expected.</param>
    ''' <param name="sDescription">Warning property value expected.</param>
    Public Shared Function AssertWarning(ByVal oResponse As ProxyWS.BaseClaimResponseType, _
        ByVal iWarning As Integer, _
        ByVal nCode As Integer, _
        ByVal sDescription As String) As ProxyWS.BaseClaimResponseTypeWarnings

        Assert.IsNotNull(oResponse.Warnings, "Warnings collection not populated.")
        Assert.Less(iWarning, oResponse.Warnings.Length, "Warnings(" & iWarning & ") not in the collection.")

        Dim oWarning As ProxyWS.BaseClaimResponseTypeWarnings = oResponse.Warnings(iWarning)

        Assert.AreEqual(nCode, oWarning.Code, "Warnings(" & iWarning & ") has unexpected Code value.")
        Assert.AreEqual(sDescription, oWarning.Description, "Warnings(" & iWarning & ") has unexpected Description value.")

        Return oWarning

    End Function

#End Region

#Region "Private Shared Methods"

    Private Shared Function HasHandlingInstanceID(ByVal oResponse As ProxyWS.SAMMethodResponseData) As Boolean
        Return oResponse.HandlingInstanceID <> Guid.Empty
    End Function

    'Private Shared Function HasExceptionType(ByVal oResponse As ProxyWS.SAMMethodResponseData) As Boolean
    '    Return oResponse.ExceptionType IsNot Nothing AndAlso oResponse.ExceptionType.Length > 0
    'End Function

    Private Shared Function HasErrors(ByVal oResponse As ProxyWS.SAMMethodResponseData) As Boolean
        Return oResponse.Errors IsNot Nothing AndAlso oResponse.Errors.Length > 0
    End Function

    Private Shared Function HasWarnings(ByVal oResponse As ProxyWS.BaseClaimResponseType) As Boolean
        Return oResponse.Warnings IsNot Nothing AndAlso oResponse.Warnings.Length > 0
    End Function

    Private Shared Function GetDiagnosticMessage(ByVal oResponse As ProxyWS.SAMMethodResponseData) As String

        Dim sMessage As String = "Unexpected response:" & vbCrLf & _
            "HandlingInstanceID = {" & oResponse.HandlingInstanceID.ToString() & "}"
        '& vbCrLf &        "ExceptionType = """ & oResponse.ExceptionType & """"

        If oResponse.Errors IsNot Nothing Then
            For iError As Integer = 0 To oResponse.Errors.Length - 1
                sMessage &= vbCrLf & "Error(" & iError & ") = " & GetDiagnosticMessage(oResponse.Errors(iError))
            Next
        Else
            sMessage &= vbCrLf & "Errors.Count = 0"
        End If

        Dim oResponse2 As ProxyWS.BaseClaimResponseType = TryCast(oResponse, ProxyWS.BaseClaimResponseType)
        If oResponse2 IsNot Nothing Then
            If oResponse2.Warnings IsNot Nothing Then
                For iWarning As Integer = 0 To oResponse2.Warnings.Length - 1
                    sMessage &= vbCrLf & "Warning(" & iWarning & ") = " & GetDiagnosticMessage(oResponse2.Warnings(iWarning))
                Next
            Else
                sMessage &= vbCrLf & "Warnings.Count = 0"
            End If
        End If

        Return sMessage

    End Function

    Private Shared Function GetDiagnosticMessage(ByVal oError As ProxyWS.SAMError) As String

        Dim oErrorBusinessRule As ProxyWS.SAMErrorBusinessRule = TryCast(oError, ProxyWS.SAMErrorBusinessRule)
        Dim oErrorInvalidData As ProxyWS.SAMErrorInvalidData = TryCast(oError, ProxyWS.SAMErrorInvalidData)

        If oErrorBusinessRule IsNot Nothing Then
            Return GetDiagnosticMessage(oErrorBusinessRule)
        ElseIf oErrorInvalidData IsNot Nothing Then
            Return GetDiagnosticMessage(oErrorInvalidData)
        Else
            Return String.Empty
        End If

    End Function

    Private Shared Function GetDiagnosticMessage(ByVal oError As ProxyWS.SAMErrorBusinessRule) As String

        Return "<" & _
            "Code = " & oError.Code & ", " & _
            "Description = """ & oError.Description & """, " & _
            "Detail = """ & oError.Detail & """" & _
            ">"

    End Function

    Private Shared Function GetDiagnosticMessage(ByVal oError As ProxyWS.SAMErrorInvalidData) As String

        Return "<" & _
            "Code = " & oError.Code & ", " & _
            "Description = """ & oError.Description & """, " & _
            "FieldName = """ & oError.FieldName & """, " & _
            "SuppliedValue = """ & oError.SuppliedValue & """" & _
            ">"

    End Function

    Private Shared Function GetDiagnosticMessage(ByVal oWarning As ProxyWS.BaseClaimResponseTypeWarnings) As String

        Return "<" & _
            "Code = " & oWarning.Code & ", " & _
            "Description = """ & oWarning.Description & """" & _
            ">"

    End Function

#End Region

End Class
