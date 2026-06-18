EXECUTE DDLDropProcedure 'spu_ACT_Select_BankAccount_Delay'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE spu_ACT_Select_BankAccount_Delay
    @bankaccount_id int,
    @mediatype_id int
AS
SELECT
	bad.bankaccount_delay_id,
    bad.mediatype_id,
    bad.delay,
	mt.description
FROM 	BankAccount_Delay bad
JOIN	MediaType mt ON mt.mediatype_id = bad.mediatype_id
WHERE 	bad.bankaccount_id = @bankaccount_id
AND		(bad.mediatype_id = @mediatype_id OR @mediatype_id IS NULL)
ORDER BY mt.description

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO