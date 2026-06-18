SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Party_Finance_Provider_add'
GO
CREATE PROCEDURE spe_Party_Finance_Provider_add
    @party_cnt int,
    @finance_provider_number int,
    @agency_number varchar(255),
    @mailbox_number varchar(30),
    @pfedidefinition_id int,
    @dob tinyint,
    @companyreg tinyint
AS
BEGIN
INSERT INTO Party_Finance_Provider (
    party_cnt,
    finance_provider_number,
    agency_number,
    mailbox_number,
    pfedidefinition_id,
    dob,
    companyreg)
VALUES (
    @party_cnt,
    @finance_provider_number,
    @agency_number,
    @mailbox_number,
    @pfedidefinition_id,
    @dob,
    @companyreg)
END
GO

