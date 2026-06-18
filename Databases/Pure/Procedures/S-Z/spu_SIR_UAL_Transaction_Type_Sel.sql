
EXECUTE DDLDropProcedure 'spu_SIR_UAL_Transaction_Type_Sel'
GO

CREATE PROCEDURE spu_SIR_UAL_Transaction_Type_Sel
AS

SELECT transaction_type_id, 
       code,
       description
FROM   transaction_type
WHERE  code IN ('C_CR', 'C_CO','C_CP','NB','MTA','REN','BACKCAN','BACKMTA','MTC')
GO
 
