SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Party_Finance_Provider_upd'
GO
CREATE PROCEDURE spe_Party_Finance_Provider_upd
    @party_cnt int,
    @finance_provider_number int,
    @agency_number varchar(255),
    @mailbox_number varchar(30),
    @pfedidefinition_id int,
    @dob tinyint,
    @companyreg tinyint
AS
BEGIN

UPDATE Party_Finance_Provider
    SET
    finance_provider_number = @finance_provider_number,
    agency_number = @agency_number,
    mailbox_number = @mailbox_number,
    pfedidefinition_id = @pfedidefinition_id,
    dob = @dob,
    companyreg = @companyreg

WHERE party_cnt = @party_cnt

END
GO

