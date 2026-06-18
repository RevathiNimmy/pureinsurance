SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Status_add'
GO

CREATE PROCEDURE spe_Renewal_Status_add
    @renewal_status_cnt int OUTPUT ,
    @product_id int ,
    @renewal_status_type_id int ,
    @insurance_holder_cnt int ,
    @insurance_file_cnt int ,
    @lead_agent_cnt int ,
    @created_by_id smallint ,
    @date_created datetime ,
    @critical_date datetime ,
    @renewal_insurance_file_cnt int,
    @is_invite_printed tinyint,
    @BrokerXferStatusTypeID int

AS

BEGIN
INSERT INTO Renewal_Status (
    product_id,
    renewal_status_type_id,
    insurance_holder_cnt,
    insurance_file_cnt,
    lead_agent_cnt,
    created_by_id,
    date_created,
    critical_date,
    renewal_insurance_file_cnt,
    is_invite_printed,
    broker_xfer_status_type_id)
VALUES (
    @product_id,
    @renewal_status_type_id,
    @insurance_holder_cnt,
    @insurance_file_cnt,
    @lead_agent_cnt,
    @created_by_id,
    @date_created,
    @critical_date,
    @renewal_insurance_file_cnt,
    @is_invite_printed,
    @BrokerXferStatusTypeID)
END

BEGIN
SELECT @renewal_status_cnt = @@IDENTITY
END

GO

