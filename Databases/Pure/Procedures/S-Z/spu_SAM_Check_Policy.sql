EXECUTE DDLDropProcedure 'spu_SAM_Check_Policy'
GO
CREATE PROCEDURE [dbo].[spu_SAM_Check_Policy] 
@insurance_file_cnt int

AS
BEGIN
  SELECT
    ifi.insurance_folder_cnt,
    ifi.insurance_file_type_id,
    ifi.source_id,
    ifi.lead_agent_cnt,
    p.quote_auto_numbering_id,
    b.code,
    p.product_id

  FROM insurance_file ifi

  INNER JOIN product p
    ON ifi.product_id = p.product_id

  INNER JOIN Source b
    ON ifi.Source_id = b.Source_id

  WHERE insurance_file_cnt = @insurance_file_cnt

  END
  GO