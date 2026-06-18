IF Isnull(Objectproperty(Object_id('dbo.PM058633_DataFix_Part1_log'), 'IsTable'), 0) = 0
  BEGIN
      CREATE TABLE [dbo].PM058633_DataFix_Part1_log
        (
           [DataFixUtility_log_id]      [INT] IDENTITY(1, 1) NOT NULL,
           [Created_by_ID]              [INT] NOT NULL,
           [insurance_file_cnt]         [INT] NOT NULL,
           [created_date]               [DATETIME] NOT NULL
        )
      ON [PRIMARY]
  END

GO

--******************************************************************************************************************
EXEC Ddldropprocedure
  'spu_PM058633_DataFix_Part1_log_add'

GO

CREATE PROCEDURE spu_PM058633_DataFix_Part1_log_add 
			@Created_by_ID              INT,
			@insurance_file_cnt         INT

AS
  BEGIN
      INSERT INTO PM058633_DataFix_Part1_log
                  (Created_by_ID,
                   insurance_file_cnt,
                   created_date)
      VALUES       (@Created_by_ID,
                    @insurance_file_cnt,
                    Getdate())
  END

GO

--******************************************************************************************************************