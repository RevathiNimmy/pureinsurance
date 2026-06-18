SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Short_Period_Rates_Sel'
GO


CREATE PROCEDURE spu_Short_Period_Rates_Sel
    @Product_id int,
    @Type char(1),
    @Period char(1),
    @Value int,
    @Effective_Date datetime
AS


SELECT
    Percentage
FROM Short_Period_Rates
WHERE
   Product_ID = @Product_ID
   AND Type = @Type
   AND Period = @Period
   AND Value <= @Value
   AND Effective_Date <= @Effective_Date
   AND Is_Deleted = 0
Order By Value Desc, Effective_Date Desc
GO


