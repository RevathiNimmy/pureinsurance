SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_BusinessDays_sel'
GO

CREATE PROCEDURE spu_SIR_BusinessDays_sel
    (@Date DATETIME)
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 09/10/2002 SJP */
/********************************************************************************************************/
SELECT non_business_days_id,
       code,
       description
FROM   Non_Business_Days
WHERE  (effective_date=@Date AND is_repeating=0)
OR     (DAY(effective_date)=DAY(@Date) AND
       MONTH(effective_date) = MONTH(@Date) AND
       is_repeating=1)

GO



