SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Group_Client_upd'
GO

CREATE PROCEDURE spe_Party_Group_Client_upd
    @party_cnt int,
    @party_group_type_id int,
    @is_registered_charity tinyint,
    @charity_number varchar(255),
    @number_of_members int,
    @TpsInd  tinyint = 0,
    @EmpsInd  tinyint = 0,
    @Mailshot tinyint =0,
    @turnover numeric(19, 4),
    @is_fee_client bit
AS
BEGIN
UPDATE Party_Group_Client
    SET
    party_group_type_id=@party_group_type_id,
    is_registered_charity=@is_registered_charity,
    charity_number=@charity_number,
    number_of_members=@number_of_members,
    TpsInd=@TpsInd,
    EmpsInd=@EmpsInd,
    Mailshot=@Mailshot,
    turnover=@turnover,
    is_fee_client=@is_fee_client
WHERE party_cnt = @party_cnt
END

GO

