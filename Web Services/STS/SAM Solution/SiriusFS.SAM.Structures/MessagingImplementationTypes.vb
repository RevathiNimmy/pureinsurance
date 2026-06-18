Option Strict Off

' Changes:
' 170505 CJB PN20978 Changes in Broking to allow document producton to be used in Swift (SJP) via the STS

#Region " Imports "

Imports system
Imports System.Text
Imports System.Xml.Serialization
Imports SiriusFS.SAM.Structure.BaseImplementationTypes

#End Region

Namespace MessagingImplementationTypes

#Region " Messaging Declarations"

    Public Class NewBusinessRequestType
        Inherits BaseNBQuoteRequestType
    End Class

    Public Class NewBusinessResponseType
        Inherits BaseNBQuoteResponseType
    End Class

    Public Class NBTransactRequestType
        Inherits BaseTransactRequestType
    End Class

    Public Class NBTransactResponseType
        Inherits BaseTransactResponseType
    End Class

    Public Class PolicyProcessRequestType
        Inherits BaseNBQuoteRequestType
    End Class

    Public Class PolicyProcessResponseType
        Inherits BaseNBQuoteResponseType
    End Class

    Public Class PolicyProcessV2ResponseType
        Inherits BasePolicyProcessV2ResponseType
    End Class

    Partial Public Class PolicyProcessV2RequestType
        Inherits BaseNBQuoteV2RequestType
    End Class

    Partial Public Class GenerateDocumentsV2RequestType
        Inherits BaseGenerateDocumentsV2RequestType
    End Class

    Partial Public Class GenerateDocumentsV2ResponseType
        Inherits BaseGenerateDocumentV2ResponseType
    End Class

#End Region
End Namespace