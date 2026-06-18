''' <summary>
''' All errors returned by SAM are repackaged into an exception to allow the Nexus
''' Framework to deal with them either by a custom error page or within code.
''' </summary>
''' <remarks></remarks>
Public Class NexusException : Inherits System.ApplicationException

    Private oErrors As ErrorCollection


    Public Sub New(ByVal v_oErrors As Object)

        For Each oError As Object In v_oErrors

            oErrors = New ErrorCollection()

            Dim oNexusError As NexusError

            With oError

                Select Case .GetType.Name

                    Case "SAMErrorBusinessRule"

                        Select Case .Code

                            Case 1000001 'Duplicate Claim Exists
                                oNexusError = New NexusError(ErrorCodes.DuplicateClaimExists)
                            Case 272 'FindControl 'No Results Found'
                                oNexusError = New NexusError(ErrorCodes.NoResultsFound)
                            Case Else
                                oNexusError = New NexusError(ErrorCodes.BusinessRule)
                        End Select

                        oNexusError.Code = .Code
                        oNexusError.Description = .Description
                        oNexusError.Detail = .Detail

                    Case "SAMErrorInvalidData"

                        oNexusError = New NexusError(ErrorCodes.InvalidData)
                        oNexusError.Code = .Code
                        oNexusError.Description = .Description
                        If (.SuppliedValue = "") Then
                            oNexusError.Detail = .FieldName
                        Else
                            oNexusError.Detail = .FieldName & " : " & .SuppliedValue
                        End If

                    Case "SAMErrorFatal"

                        oNexusError = New NexusError(ErrorCodes.FatalError)
                        oNexusError.Description = .Type

                    Case Else

                        oNexusError = New NexusError(ErrorCodes.Unknown)
                        oNexusError.Detail = .ToString

                End Select

            End With

            oErrors.Add(oNexusError)

        Next

    End Sub

    ''' <summary>
    ''' Process the error object and repackage some of the SAM error codes to
    ''' reduce the work need further up the stack
    ''' </summary>
    ''' <param name="v_oErrors">Error object returned by a SAM method call</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal v_oErrors() As Object)

        For Each oError As Object In v_oErrors

            oErrors = New ErrorCollection()

            Dim oNexusError As NexusError

            With oError

                Select Case .GetType.Name

                    Case "SAMErrorBusinessRule"

                        Select Case .Code

                            Case 1000001 'Duplicate Claim Exists
                                oNexusError = New NexusError(ErrorCodes.DuplicateClaimExists)
                            Case 272 'FindControl 'No Results Found'
                                oNexusError = New NexusError(ErrorCodes.NoResultsFound)
                            Case Else
                                oNexusError = New NexusError(ErrorCodes.BusinessRule)
                        End Select

                        oNexusError.Code = .Code
                        oNexusError.Description = .Description
                        oNexusError.Detail = .Detail

                    Case "SAMErrorInvalidData"

                        oNexusError = New NexusError(ErrorCodes.InvalidData)
                        oNexusError.Code = .Code
                        oNexusError.Description = .Description
                        oNexusError.Detail = .FieldName & " : " & .SuppliedValue

                    Case "SAMErrorFatal"

                        oNexusError = New NexusError(ErrorCodes.FatalError)
                        oNexusError.Description = .Type

                    Case Else

                        oNexusError = New NexusError(ErrorCodes.Unknown)
                        oNexusError.Detail = .ToString

                End Select

            End With

            oErrors.Add(oNexusError)

        Next

    End Sub

    ''' <summary>
    ''' Readonly access to the ErrorCollection object
    ''' </summary>
    ''' <returns>ErrorCollection object</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Errors() As ErrorCollection
        Get
            Return oErrors
        End Get
    End Property

    ''' <summary>
    ''' Override the exception message
    ''' </summary>
    ''' <returns>SAM Exception</returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property Message() As String
        Get
            Return "SAM Exception"
        End Get
    End Property

End Class

''' <summary>
''' ErrorCollection object, inherited from CollectionBase
''' </summary>
''' <remarks></remarks>
Public Class ErrorCollection : Inherits Collections.CollectionBase

    ''' <summary>
    ''' Add a NexusError object to the collection
    ''' </summary>
    ''' <param name="v_oNexusError">NexusError object</param>
    ''' <returns>The result, as inherited from CollectionBase</returns>
    ''' <remarks></remarks>
    Public Function Add(ByVal v_oNexusError As NexusError) As Integer
        Return List.Add(v_oNexusError)
    End Function

    ''' <summary>
    ''' Remove a NexusError object from the collection
    ''' </summary>
    ''' <param name="v_oNexusError">The NexusError object to be removed</param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal v_oNexusError As NexusError)
        List.Remove(v_oNexusError)
    End Sub

    ''' <summary>
    ''' Retrieve a NexusError object from the collection by the index
    ''' </summary>
    ''' <param name="i">Index of the NexusError to be returned</param>
    ''' <value>A NexusError to replace the existing NexusError at the index specified</value>
    ''' <returns>The NexusError object at the index specified</returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As NexusError
        Get
            Return List(i)
        End Get
        Set(ByVal value As NexusError)
            List(i) = value
        End Set
    End Property

End Class

''' <summary>
''' NexusError object to contain an error returned by implemented provider
''' </summary>
''' <remarks></remarks>
Public Class NexusError

    Private oNexusCode As ErrorCodes
    Private sCode As String
    Private sDescription As String
    Private sDetail As String

    ''' <summary>
    ''' Constructor, the Code is the only required attribute
    ''' </summary>
    ''' <param name="v_oNexusCode">Identify code of the error</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal v_oNexusCode As ErrorCodes)
        oNexusCode = v_oNexusCode
    End Sub

    ''' <summary>
    ''' Error Code
    ''' </summary>
    ''' <returns>Error Code</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NexusCode() As ErrorCodes
        Get
            Return oNexusCode
        End Get
    End Property

    ''' <summary>
    ''' The code returned by the service called within the provider
    ''' </summary>
    ''' <value>String representation of the code (usually numeric)</value>
    ''' <returns>Strng representation of the code (usually numeric)</returns>
    ''' <remarks></remarks>
    Public Property Code() As String
        Get
            Return sCode
        End Get
        Set(ByVal value As String)
            sCode = value
        End Set
    End Property

    ''' <summary>
    ''' Description summary of the error
    ''' </summary>
    ''' <value>Description summary of the error</value>
    ''' <returns>Description summary of the error</returns>
    ''' <remarks></remarks>
    Public Property Description() As String
        Get
            Return sDescription
        End Get
        Set(ByVal value As String)
            sDescription = value
        End Set
    End Property

    ''' <summary>
    ''' Further details of the error
    ''' </summary>
    ''' <value>Further details of the error</value>
    ''' <returns>Further details of the error</returns>
    ''' <remarks></remarks>
    Public Property Detail() As String
        Get
            Return sDetail
        End Get
        Set(ByVal value As String)
            sDetail = value
        End Set
    End Property

End Class

Public Enum ErrorCodes

    Unknown = 0

    BusinessRule = 101
    InvalidData = 102
    FatalError = 103

    DuplicateClaimExists = 201
    NoResultsFound = 202

End Enum

