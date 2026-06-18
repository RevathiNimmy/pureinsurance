SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Status_upd'
GO

CREATE PROCEDURE spe_Renewal_Status_upd
    @renewal_status_cnt int,
    @product_id int,
    @renewal_status_type_id int,
    @insurance_holder_cnt int,
    @insurance_file_cnt int,
    @lead_agent_cnt int,
    @created_by_id smallint,
    @date_created datetime,
    @critical_date datetime
AS
BEGIN
UPDATE Renewal_Status
    SET
    product_id=@product_id,
    renewal_status_type_id=@renewal_status_type_id,
    insurance_holder_cnt=@insurance_holder_cnt,
    insurance_file_cnt=@insurance_file_cnt,
    lead_agent_cnt=@lead_agent_cnt,
    created_by_id=@created_by_id,
    date_created=@date_created,
    critical_date=@critical_date
WHERE renewal_status_cnt = @renewal_status_cnt
END

GO

