SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Renewal_Failure'
GO

CREATE PROCEDURE spu_Report_Renewal_Failure
AS

SELECT 
    rf.Failure_reason,
    i.insurance_ref policy_number,
    i.cover_start_date,
    i.expiry_date cover_end_date,
    pr.code product_code,
   
    p.shortname  client_code,
    p.name client_name,
    ISNULL(pa.shortname,'Direct') agent_code,
    src.code branch_code
 
 FROM renewal_automatic_accept_failure rf
   LEFT JOIN insurance_file i ON rf.insurance_file_cnt = i.insurance_file_cnt
   LEFT JOIN product pr ON pr.product_id = i.product_id
   LEFT JOIN party p ON i.insured_cnt = p.party_cnt
   LEFT JOIN party pa ON i.lead_agent_cnt = pa.party_cnt
   LEFT JOIN Source src ON i.source_id = src.source_id

GO




