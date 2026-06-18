SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Net_Data_sel'
GO

CREATE PROCEDURE spe_Party_Net_Data_sel
    @party_cnt int
AS

SELECT
    party_cnt,
    password,
    mothers_maiden_name,
    tp_introducer_code,
    tp_user_code,
    memorable_date,
    a_question,
    the_answer,
    userid,
    current_ins_renewal_date
 FROM Party_Net_Data

WHERE party_cnt = @party_cnt

GO

