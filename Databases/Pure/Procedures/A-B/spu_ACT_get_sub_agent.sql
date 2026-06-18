SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_get_sub_agent'
GO

CREATE PROCEDURE spu_ACT_get_sub_agent
    @transdetail_id INT
AS

DECLARE @party_agent_type_id INT
DECLARE @insurance_file_cnt INT
DECLARE @document_id INT
DECLARE @agent_transdetail_id INT
DECLARE @agent_account_id INT

--Get the insurance_file_cnt
SELECT @insurance_file_cnt = d.insurance_file_cnt 
FROM transdetail t,
     document d
WHERE t.transdetail_id = @transdetail_id
AND t.document_id = d.document_id

--Get the party agent type for sub agent
SELECT @party_agent_type_id = party_agent_type_id 
FROM party_agent_type 
WHERE code = '4'

--Get the sub agent details if they exist
SELECT t.transdetail_id, t.account_id, t.amount
FROM transdetail t,
     party_agent pa,
     document d,
     account a
WHERE t.account_id = a.account_id
AND t.document_id = d.document_id
AND d.insurance_file_cnt = @insurance_file_cnt
AND a.account_key = pa.party_cnt 
AND pa.party_agent_type_id = @party_agent_type_id   

