SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PM_Get_Batch_Tasks'
GO 


CREATE PROC spu_PM_Get_Batch_Tasks 
@Batch_Status     VARCHAR(10), 
@Batch_date_limit DATETIME = NULL 
AS 
  BEGIN 
	  DECLARE @Document_Export_Type_Id INT
	  DECLARE @Payment_Import_Type_Id INT
	  DECLARE @Reference_Import_Type_Id INT
	  DECLARE @Cover_Note_Book_Import_Type_Id INT
	  DECLARE @Instalment_Import_Type_Id INT
	  DECLARE @Bank_Reconciliation_Import_Type_Id INT
	  DECLARE @Cash_Allocation_Import_Type_Id INT
	  DECLARE @MID_IMPORT_Type_Id INT
	  DECLARE @Receipt_Import_Type_Id INT
      DECLARE @Policy_BDX_Type_Id INT 
      DECLARE @Premium_BDX_Type_Id INT 
      DECLARE @Claim_BDX_Type_Id INT
	  DECLARE @Direct_Debit_Batch_Type_Id INT
	  DECLARE @Policy_Batch_Type_Id INT
	  DECLARE @General_Ledger_Export_Type_Id INT

      SELECT @Document_Export_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'DOC'
	  
	  SELECT @Payment_Import_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'SPYI'

	  SELECT @Reference_Import_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'REFI'

	  SELECT @Cover_Note_Book_Import_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'COVBOOKI'

	  SELECT @Instalment_Import_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'INSI'
	  
	  SELECT @Bank_Reconciliation_Import_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'BCP'
	  
	  SELECT @Cash_Allocation_Import_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'CAALLOC'

	  SELECT @MID_IMPORT_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'MIDI'
	  
	  SELECT @Receipt_Import_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'SRPI'
	  
	  SELECT @Policy_BDX_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'BDXPOL' 

      SELECT @Premium_BDX_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'BDXPREM' 

      SELECT @Claim_BDX_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'BDXCLM' 

	  SELECT @Direct_Debit_Batch_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'BAC'
	  
	  SELECT @Policy_Batch_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'POLBAT' 

	  SELECT @General_Ledger_Export_Type_Id = batch_type_id 
      FROM   batch_type 
      WHERE  code = 'GLX'

      SELECT CASE 
               WHEN b.batch_type_id IN( @Payment_Import_Type_Id, @Reference_Import_Type_Id, @Cover_Note_Book_Import_Type_Id,
			    @Instalment_Import_Type_Id, @Bank_Reconciliation_Import_Type_Id, @Cash_Allocation_Import_Type_Id,
				 @MID_IMPORT_Type_Id, @Receipt_Import_Type_Id) THEN 'Sirius Import' 
               WHEN b.batch_type_id IN( @Document_Export_Type_Id ) THEN 'Document Export'
			   WHEN b.batch_type_id IN( @Direct_Debit_Batch_Type_Id ) THEN 'Instalment Export'
			   WHEN b.batch_type_id IN( @Policy_Batch_Type_Id ) THEN 'Policy Batch Export'
			   WHEN b.batch_type_id IN( @General_Ledger_Export_Type_Id) THEN 'General Ledger Export'
			   ELSE interface_code 
             END AS Process, 
             CASE 
               WHEN b.batch_type_id = @Policy_BDX_Type_Id THEN 'BDX policy import' 
               WHEN b.batch_type_id = @Premium_BDX_Type_Id THEN 
               'BDX written premium import' 
               WHEN b.batch_type_id = @Claim_BDX_Type_Id THEN 'BDX Claims import' 
               ELSE b.description 
             END AS Description, 
			 created_date AS StartDateTime, 
             completed_date AS EndDateTime, 
             total_transactions AS TotalRecordsCount, 
             import_file_name AS [FileName], 
			 CASE 
               WHEN bs.code IN ( 'F' ) THEN 0 
               ELSE Isnull(total_transactions, 0) - Isnull(reject_transactions, 0) END AS PassedRecordsCount, 
             CASE 
               WHEN bs.code IN ( 'F' ) THEN 0 
               ELSE reject_transactions END AS FailedRecordsCount, 
             CASE 
               WHEN bs.code IN ( 'F' ) THEN 'F' 
               WHEN bs.code IN ( 'BE', 'BC', 'C' ) THEN 'C' 
               ELSE 'BI' 
             END AS [Status], 
             batch_id, 
             CASE 
               WHEN bs.code IN ( 'F' ) THEN 'Failed' 
               WHEN bs.code IN ( 'BE', 'BC', 'C' ) THEN 'Completed' 
               ELSE 'In Progress' 
             END AS [StatusDescription] 
      FROM   batch b 
             INNER JOIN batchstatus bs 
                     ON b.batchstatus_id = bs.batchstatus_id 
             LEFT OUTER JOIN batch_type bt 
                          ON b.batch_type_id = bt.batch_type_id 
      WHERE  ( ( ( ISNULL(completed_date,created_date) >= @Batch_date_limit ) 
                  --OR ( completed_date IS NULL ) ) 
				  )
                OR ( @Batch_date_limit IS NULL ) ) 
             AND ( ( ( CASE 
                         WHEN bs.code IN ( 'F' ) THEN 'F' 
                         WHEN bs.code IN ( 'BE', 'BC', 'C' ) THEN 'C' 
                         ELSE bs.code 
                       END ) = @Batch_Status ) 
                    OR ( Isnull(@Batch_Status, 'All') = 'All' ) ) 
      ORDER  BY [status] DESC, 
                CASE 
                  WHEN bs.code IN ( 'F' ) THEN B.completed_date 
                  WHEN bs.code IN ( 'BE', 'BC', 'C' ) THEN B.completed_date 
                  ELSE B.created_date 
                END 
  END 
GO 