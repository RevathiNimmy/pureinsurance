Imports Sirius.Architecture.Data

Namespace Controller
    ''' <summary>
    ''' Details about the job
    ''' </summary>
    Friend NotInheritable Class JobDetail 

        ''' <summary>
        ''' Gets or sets the job id as stored in the dbo.Batch_Renewal_Job table
        ''' </summary>
        Public Property JobId() As Integer

        ''' <summary>
        ''' Gets or sets the job code as stored in the dbo.Batch_Renewal_Job table
        ''' </summary>
        Public Property Code() As String

        ''' <summary>
        ''' Gets or sets the type of renewal job id as stored in the dbo.Batch_Renewal_Job table
        ''' </summary>
        Public Property JobTypeId As Integer

        ''' <summary>
        ''' Gets or sets the type of renewal code as stored in the dbo.Batch_Renewal_Job table
        ''' </summary>
        Public Property JobTypeCode As String

        ''' <summary>
        ''' Gets or sets the batch job id for this batch as stored in dbo.Batch
        ''' </summary>
        Public Property BatchId As Integer

        ''' <summary>
        ''' Gets or sets the batch job id for this batch as stored in dbo.Batch
        ''' </summary>
        Public Property JobDescription As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Batch_Job_Type_Description As String
        ''' <summary>
        ''' Gets or sets the type of Run Renewal Extended Rule as stored in the dbo.Batch_Renewal_Job table
        ''' </summary>
        ''' <returns></returns>
        Public Property Run_Renewal_Extended_Rule As Byte

    End Class

End Namespace
