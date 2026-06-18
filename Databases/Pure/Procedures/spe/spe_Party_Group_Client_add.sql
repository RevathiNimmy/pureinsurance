SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Group_Client_add'
GO

CREATE PROCEDURE spe_Party_Group_Client_add
    @party_cnt int,
    @party_group_type_id int,
    @is_registered_charity tinyint,
    @charity_number varchar(255),
    @number_of_members int,
    @TpsInd tinyint = 0,
    @EmpsInd tinyint= 0,
    @Mailshot tinyint = 0,
    @turnover numeric(19, 4),
    @is_fee_client bit
AS
BEGIN
INSERT INTO Party_Group_Client (
    party_cnt ,
    party_group_type_id ,
    is_registered_charity,
    charity_number,
    number_of_members,
    TpsInd, 
    EmpsInd, 
    Mailshot,
    Turnover,
    is_fee_client)
VALUES (
    @party_cnt,
    @party_group_type_id,
    @is_registered_charity,
    @charity_number,
    @number_of_members,
    @TpsInd,
    @EmpsInd,
    @Mailshot,
    @turnover,
    @is_fee_client)
END

GO

