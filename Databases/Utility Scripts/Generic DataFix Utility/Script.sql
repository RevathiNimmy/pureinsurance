IF Isnull(Objectproperty(Object_id('dbo.DataFixUtility_log'), 'IsTable'), 0) = 0
  BEGIN
      CREATE TABLE [dbo].[DataFixUtility_log]
        (
           [DataFixUtility_log_id]      [INT] IDENTITY(1, 1) NOT NULL,
           [PMNumber]                   [VARCHAR](50) NULL,
           [Created_by]                 [VARCHAR](50) NULL,
           [insurance_file_cnt]         [INT] NULL,
           [old_document_id]            [INT] NULL,
           [new_document_id]            [INT] NULL,
           [created_date]               [DATETIME] NULL,
           [comment]                    [VARCHAR](100) NULL,
           [Claim_id]                   [INT] NULL,
           [AllocationID]               [INT] NULL,
           [BordereauReference]         [NVARCHAR](100) NULL,
           [DepositNumber]              [NVARCHAR](100) NULL,
           [DomainAaccount]             [NVARCHAR](100) NULL,
           [ApplicationLoggedInAccount] [NVARCHAR](100) NULL
        )
      ON [PRIMARY]
  END

GO

EXEC Ddldropprocedure
  'spu_DataFixUtility_log_add'

GO

CREATE PROCEDURE Spu_datafixutility_log_add @PMNumber                   VARCHAR(50) = '',
                                            @Created_by                 VARCHAR(50),
                                            @insurance_file_cnt         INT=0,
                                            @old_document_ref           VARCHAR(20) = '',
                                            @new_document_id            INT = 0,
                                            @is_reversal                BIT=0,
                                            @Claim_id                   INT=0,
                                            @BordereauReference         NVARCHAR(100) = '',
                                            @DepositNumber              NVARCHAR(100) = '',
                                            @DomainAaccount             NVARCHAR(100) = '',
                                            @ApplicationLoggedInAccount NVARCHAR(100) = '',
                                            @AllocationID               INT = 0,
                                            @AssociatedDocRef           NVARCHAR(20) = ''
AS
  BEGIN
      DECLARE @old_document_id INT,
              @comment         VARCHAR(100)

      SELECT @old_document_id = document_id
      FROM   document
      WHERE  document_ref = @old_document_ref

      IF @is_reversal = 1
        SELECT @comment = 'Reversal of document_ref - '
                          + @old_document_ref
      ELSE
        SELECT @comment = 'Reposting of document_ref -'
                          + @old_document_ref

      IF @AllocationID > 0
        SELECT @comment = @old_document_ref
                          + ' has been reversed and now allocated to '
                          + @AssociatedDocRef

      IF Isnull(@new_document_id, 0) = 0
        BEGIN
            IF @insurance_file_cnt = 0
              BEGIN
                  SELECT TOP 1 @new_document_id = D.document_id,
                               @insurance_file_cnt = D.insurance_file_cnt
                  FROM   Stats_Folder SF
                         INNER JOIN Document D
                                 ON SF.document_ref = D.document_ref
                  WHERE  loss_id = @Claim_id
                  ORDER  BY stats_folder_cnt DESC
              END
            ELSE
              SELECT @new_document_id = Max(document_id)
              FROM   document
              WHERE  insurance_file_cnt = @insurance_file_cnt
        END

      IF @claim_id = 0
        SET @claim_id = NULL

      INSERT INTO DataFixUtility_log
                  (PMNumber,
                   Created_by,
                   insurance_file_cnt,
                   old_document_id,
                   new_document_id,
                   created_date,
                   comment,
                   Claim_id,
                   BordereauReference,
                   DepositNumber,
                   DomainAaccount,
                   ApplicationLoggedInAccount,
                   AllocationID)
      VALUES      ( @PMNumber,
                    @Created_by,
                    @insurance_file_cnt,
                    @old_document_id,
                    @new_document_id,
                    Getdate(),
                    @comment,
                    @Claim_id,
                    @BordereauReference,
                    @DepositNumber,
                    @DomainAaccount,
                    @ApplicationLoggedInAccount,
                    @AllocationID )
  END

GO