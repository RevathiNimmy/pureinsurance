SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Group_Client_sel'
GO

CREATE PROCEDURE spe_Party_Group_Client_sel
    @party_cnt int
AS
SELECT
    party_cnt,
    party_group_type_id,
    is_registered_charity,
    charity_number,
    number_of_members,
    TpsInd,
    EmpsInd,
    Mailshot,
    turnover,
    is_fee_client
 FROM Party_Group_Client
WHERE party_cnt = @party_cnt

GO

