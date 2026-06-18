SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'Spu_Report_Prepaid_Premium_SFU'
GO


 /**********************************************************************************************************************************
** Created by Gaurav Arora
** Created On 03 May 2007 At 4:30 PM
** Created For S4I Reports - Prepaid_Premium.rpt**
**********************************************************************************************************************************
**
**
**
**********************************************************************************************************************************
***********************************************************************************************************************************/

CREATE  PROCEDURE Spu_Report_Prepaid_Premium_SFU  
       @branch_id  	INT,  
       @startDTCombo  	VARCHAR(50) = '',  
       @endDTCombo	VARCHAR(50) = '' 
  
AS  
  
DECLARE @ibranchid 		INT,
	@dtPeriodStartDate	DATETIME,
	@dtPeriodEndDate	DATETIME




SELECT 	@dtPeriodStartDate = CONVERT (Datetime, @startDTCombo),    
	@dtPeriodEndDate = CONVERT (Datetime, @endDTCombo)    

  
IF  @branch_id IS NULL  
   SELECT @iBranchID = 0  
ELSE  
   SELECT @iBranchID = @branch_id  
  
        CREATE TABLE #PolicyDetails  
        (  
               Policy_Number VARCHAR(50),  
               EffectiveDate DATETIME,  
               InsuredName VARCHAR(387),  
               Document_Ref VARCHAR(50),  
               ReceivedDate DATETIME,  
               Prepaid_Amount NUMERIC(18,4),  
               Policy_Version VARCHAR(50)  
        )  
  
        SET NOCOUNT ON  
  
        INSERT INTO #PolicyDetails  
        SELECT   INF.Insurance_ref Policy_Number,  
                 INF.cover_start_date Effective_Date,  
                 P.resolved_name Insured_Name,  
                 --D.document_ref Document_ref,  
                 (SELECT TOP 1 Document_Ref  
                  FROM    Document D  
                         INNER JOIN TransDetail TD  
                              ON   TD.Document_Id = D.Document_id  
                         INNER JOIN CashListItem CLI1  
                              ON   CLI1.TransDetail_Id = TD.TransDetail_Id  
                 WHERE   CLI1.CashListItem_Id = CLI.CashListItem_id) Document_ref,  
                 --TD.Accounting_date Received_Date,  
                 CLI.transaction_date Received_Date,  
                 CLI.amount Prepaid_Amount,  
                 INF.Policy_Version  
        FROM  Document D  
  
        INNER JOIN Insurance_File INF  
              ON D.Insurance_File_Cnt = INF.Insurance_File_Cnt  
        INNER JOIN (Select TD1.*  
                    FROM TransDetail TD1
                    INNER JOIN Document D1  
                          ON D1.Document_id = TD1.Document_id  
                    INNER JOIN AllocationDetail AD1  
                          ON AD1.TransDetail_id = TD1.TransDetail_id  
                    WHERE AD1.CashListItem_id IS NOT NULL) TD  
              ON TD.Document_Id =D.Document_Id  
        INNER JOIN AllocationDetail AD  
              ON TD.TransDetail_id= AD.Transdetail_id  
        INNER JOIN Account AC  
              ON AC.account_id = TD.account_id  
              AND (AC.Account_key=inf.insured_cnt  
              OR  AC.Account_key = (SELECT   TOP 1 INF1.lead_agent_cnt  
                                    FROM    Insurance_File INF1  
                                    WHERE   INF1.Insurance_File_Cnt = INF.Insurance_File_Cnt))
  
        INNER JOIN PARTY P  
              ON P.Party_cnt = AC.Account_Key  
        INNER JOIN Allocation A  
              ON AD.Allocation_id = A.Allocation_id  
        INNER JOIN CashListItem CLI  
              ON AD.CashListItem_Id = CLI.CashListItem_Id  
  
        WHERE   CLI.Transaction_Date <= INF.cover_start_date
		AND (Select Sum(alloc_base_amount) from AllocationDetail AD
			WHERE AD.Transdetail_id in (Select Transdetail_id 
					from Transdetail td 
					inner join document d 
						on d.document_id = td.document_id 
					inner join insurance_file INF_IN
						on INF_IN.insurance_file_cnt = d.insurance_file_cnt
					WHERE 	INF_IN.Insurance_File_Cnt = INF.insurance_file_cnt
					AND TD.account_id = AC.Account_Id))  > = (SELECT SUM(amount)
     							FROM TransDetail TD3  
	     						WHERE  TD3.Document_id = D.Document_Id  
     							AND TD3.account_id = AC.Account_id)  
        AND

        (  
                (@ibranchid <> 0 AND INF.source_id = @ibranchid)  
                  OR  
                (@ibranchid =0)  
        )  
  
  	AND 	
	(
		(@startDTCombo = '' 
		AND @endDTCombo = ''
		)
  		  OR
		(@startDTCombo <> '' 
		AND @endDTCombo <> '' 
		AND INF.cover_start_date BETWEEN @dtPeriodStartDate AND @dtPeriodEndDate
		)
  		  OR
		(@startDTCombo <> '' 
		AND @endDTCombo = ''
		AND INF.cover_start_date  >= @dtPeriodStartDate)
  		  OR
		(@startDTCombo = '' 
		AND @endDTCombo <> ''
		AND INF.cover_start_date  <= @dtPeriodEndDate)
	
	)	
--SELECT the records from the Temp Table and sent it back to the Report  
        SELECT DISTINCT  
             Policy_Number,  
             EffectiveDate,  
             InsuredName,  
             Document_Ref,  
             ReceivedDate,  
             Prepaid_Amount,  
             Policy_Version  
        FROM #PolicyDetails  
        ORDER BY Policy_Number  
  
        DROP TABLE #PolicyDetails  
  
        SET NOCOUNT ON  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

