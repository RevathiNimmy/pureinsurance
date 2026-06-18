Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Net
Imports SSP.Shared
Imports Standard.Licensing
Imports Standard.Licensing.Validation

Public Class LicenseManager

    Private _ICCS As Integer, _ELEMENT_NAME_ICCS As System.Xml.Linq.XName
    Public Const FileExtension_License As String = ".lic"

    Private Const MESSAGE_LICENSE_MISSING1 As String = "Unable to find license file {0}."
    Private Const MESSAGE_LICENSE_INVALID_PRODUCT_IDENTITY1 As String = "License file {0} is not associated with this Pure product."
    Private Const MESSAGE_LICENSE_INVALID_PRODUCT_INSTANCE1 As String = "License file {0} is not associated with this instance of Pure product."
    Private Const MESSAGE_LICENSE_INVALID As String = "License validation failure."
    Private Const MESSAGE_LICENSE_RESOLVE As String = "Please contact  IT department or Support"

    Private Const FileExtension_PrivateKey As String = ".private"
    Private Const ELEMENT_NAME_ROOT As String = "private"
    Private Const ELEMENT_NAME_PUBLICKEY As String = "public-key"
    Private Const ELEMENT_NAME_ID As String = "id"
    Private Const ELEMENT_NAME_PRODUCTID As String = "product-id"

    Private Const ProductFeature_Name_Product As String = "Product"
    Private Const ProductFeature_Name_Version As String = "Version"
    Private Const ProductFeature_Name_PublishDate As String = "Publish Date"

    Private Const Attribute_Name_ProductIdentity As String = "Product Identity"
    Private Const Attribute_Name_AssemblyIdentity As String = "Assembly Identity"
    Private Const Attribute_Name_ExpirationDays As String = "Expiration Days"
    Private Const Attribute_Name_EmailThresholdDays As String = "Email Threshold Days"
    Private Const Attribute_Name_EmailThresholdMonths As String = "Email Threshold Months"
    Private Const Attribute_Name_PublicKey As String = "Public Key"
    ' 
    '  Licensor: These properties are set for creating a new license.
    '  Licensee: These properties are set when a license has been validated.
    ' 
    Private StandardOrTrial As LicenseType = LicenseType.Standard

    Public ExpirationDays As Integer

    Public Quantity As Integer = 1
    Public LoginReminderToStart As Integer
    Private Product As String = String.Empty

    Private Version As String = String.Empty

    Private PublishDate As Date?

    Private Name As String = String.Empty

    Private Email As String = String.Empty

    Private Company As String = String.Empty

    Public loadErrors As IEnumerable(Of IValidationFailure) = Nothing

    Public KeyPublic As String = String.Empty

    Public ID As Guid

    Public ICCS As String = String.Empty
    Private Shared Function CreateProductIdentity(ICCS As String, keyPublic As String) As String
        Return ICCS & " " & keyPublic
    End Function
    ' 
    '  We handle two use cases:
    ' 
    '  1. Licensor -- private passphrase and key to create keypair and license file(s) for OTHER executables.
    '  2. Licensee -- public key to load and validate license file for THIS executable.
    ' 
    Public Sub New()
    End Sub
    Public Sub NewID()
        ID = Guid.NewGuid()
    End Sub

    Public Function IsThisLicenseValid(ByVal iccs As String, ByRef errorMessage As String) As Boolean
        Product = String.Empty
        Version = String.Empty
        PublishDate = Nothing

        ExpirationDays = 0
        Quantity = 1

        Name = String.Empty
        Email = String.Empty
        Company = String.Empty

        Dim licenseKeyPath As String = ""
        gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:=gPMConstants.kRegKeyLicenseKeyPath, r_sSettingValue:=licenseKeyPath)
        If String.IsNullOrEmpty(licenseKeyPath) Then
            errorMessage = "No License File Found"
            Return False
        End If
        Dim publicKey As String
        Dim pathLicense As String = licenseKeyPath

        Try
            If Not File.Exists(pathLicense) Then
                Return String.Format(MESSAGE_LICENSE_MISSING1, pathLicense) & Environment.NewLine & Environment.NewLine & MESSAGE_LICENSE_RESOLVE
            End If

            Dim xmlLicense = File.ReadAllText(pathLicense, Encoding.UTF8)
            Dim license As Standard.Licensing.License = Standard.Licensing.License.Load(xmlLicense)
            Const Attribute_Name_Domain As String = "Network Domain"

            Dim domain As String = license.AdditionalAttributes.Get(Attribute_Name_Domain)
            If Not String.IsNullOrEmpty(domain) Then
                Dim systemDomainName As String = NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName
                If systemDomainName.ToLower() <> domain.ToLower() Then
                    Return False
                End If
            End If
            Dim validationFailures As List(Of IValidationFailure) = New List(Of IValidationFailure)()

            Dim identityProductLicense As String = license.AdditionalAttributes.[Get](Attribute_Name_ProductIdentity)
            publicKey = license.AdditionalAttributes.[Get](Attribute_Name_PublicKey)

            Dim identityProductCaller As String = SecureHash.ComputeSHA256Hash(CreateProductIdentity(iccs, publicKey))
            If Not Equals(identityProductCaller, identityProductLicense) Then
                validationFailures.Add(New GeneralValidationFailure() With {
.Message = String.Format(MESSAGE_LICENSE_INVALID_PRODUCT_IDENTITY1, pathLicense),
.HowToResolve = MESSAGE_LICENSE_RESOLVE
})
                errorMessage = String.Format(MESSAGE_LICENSE_INVALID_PRODUCT_IDENTITY1, pathLicense)
                Return False
            End If
            Dim dateTime = Convert.ToDateTime(license.Expiration)
            Dim dateNow = Date.Now

            Dim expirationDays = Convert.ToString((license.Expiration - dateNow).TotalDays)

            loadErrors = license.Validate().ExpirationDate().[When](Function(lic) Not String.IsNullOrEmpty(expirationDays)).[And]().Signature(publicKey).AssertValidLicense()

            If Not loadErrors.Any() Then
                Const Attribute_Name_LoginReminderToStart As String = "Login Reminders To Start"

                Product = license.ProductFeatures.[Get](ProductFeature_Name_Product)
                Version = license.ProductFeatures.[Get](ProductFeature_Name_Version)
                LoginReminderToStart = license.AdditionalAttributes.Get(Attribute_Name_LoginReminderToStart)
                Dim s As String = license.ProductFeatures.[Get](ProductFeature_Name_PublishDate)
                If Not String.IsNullOrEmpty(s) Then
                    PublishDate = Date.Parse(s, CultureInfo.InvariantCulture)
                End If

                StandardOrTrial = license.Type
                If license.Expiration.[Date] <> Date.MaxValue.Date Then
                    Me.ExpirationDays = Convert.ToInt32(license.Expiration.Subtract(CDate(Date.UtcNow)).TotalDays)
                End If

                Quantity = license.Quantity

                Name = license.Customer.Name
                Email = license.Customer.Email
                Company = license.Customer.Company
            End If
            validationFailures.AddRange(loadErrors)

            If validationFailures.Count > 0 Then
                Dim errorMessages As List(Of String) = New List(Of String)()
                For Each failure In validationFailures
                    errorMessages.Add($"{failure.[GetType]().Name}: {failure.Message}{Environment.NewLine}{failure.HowToResolve}")
                Next
                errorMessage = String.Join(Environment.NewLine, errorMessages)
                Return False
            End If
        Catch ex As FileNotFoundException
            errorMessage = ex.Message
            Return False
        Catch ex As Exception
            errorMessage = ex.Message
            Return False
        End Try

        Return True
    End Function

    Private Shared Function GetLicensePath() As String
        Dim pathExecutable As String = GetAssemblyFilePath()
        Return Path.ChangeExtension(pathExecutable, FileExtension_License)
    End Function

    Public Shared Function GetAssemblyFilePath() As String
        ' AppContext.BaseDirectory is just the folder path (e.g., "C:\Path\To\").
        Dim asm As Assembly = Assembly.GetEntryAssembly()
        Return If(asm?.Location, String.Empty)
    End Function
End Class
