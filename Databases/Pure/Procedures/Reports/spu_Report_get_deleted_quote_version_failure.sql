SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_get_deleted_quote_version_failure'
GO


CREATE PROCEDURE spu_Report_get_deleted_quote_version_failure  
  
    @startDTCombo   VARCHAR(50) = '',  
    @endDTCombo VARCHAR(50) = ''  
  
AS  
 DECLARE @dtPeriodStartDate DATE,
   @dtPeriodEndDate   DATE,  
   @Status            INT=0  
  
     BEGIN  
     select @dtPeriodStartDate = CONVERT (Datetime, @startDTCombo),  
            @dtPeriodEndDate = CONVERT (Datetime, @endDTCombo)  
                  
                  
                  SELECT  (RTRIM(LTRIM(ifdl.insurance_ref)) + ' ' +  ISNULL(RTRIM(LTRIM(ifdl.quote_status_description)),' ')  + ' V' + ISNULL(Convert(varchar,ifdl.quote_version),'') ) AS   'quote_ref',  
                                      p1.shortname									'agent_code',  
                                      p1.resolved_name								        'agent_name',  
                                      p.description									'product_desc',  
                                      p2.shortname									'policy_holder_name',  
                                     convert(varchar,ifdl.deletion_date,101)                                            'deletion_date',  
                                      ifdl.failure_description						                'failure_description'  
                  FROM Insurance_File_Delete_Log ifdl									  
                                      INNER JOIN Party P2 ON p2.party_cnt=ifdl.insured_cnt  
                                      INNER JOIN Product p on p.product_id =ifdl.product_id                                        
                                      LEFT JOIN PARTY p1 ON p1.party_cnt=ifdl.lead_agent_cnt                                      
				      WHERE Convert(DATE,ifdl.deletion_date) >= @dtPeriodStartDate  
                                      AND Convert(DATE,ifdl.deletion_date) <= @dtPeriodEndDate  
                                      AND ifdl.status=@Status                    
      END  



GO
