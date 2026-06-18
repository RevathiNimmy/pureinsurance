SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_do_IsInsurer'
GO

CREATE PROCEDURE spu_ACT_do_IsInsurer
    @transdetail_id int,
    @IsInsurer tinyint OUTPUT
AS


SELECT @IsInsurer = 0

SELECT @IsInsurer = 1
FROM   transdetail t
JOIN   account a ON a.account_id = t.account_id 
JOIN   party p ON p.party_cnt = a.account_key
JOIN   party_type pt ON pt.party_type_id = p.party_type_id
WHERE  pt.code = 'IN'
AND    t.transdetail_id = @transdetail_id


GO

