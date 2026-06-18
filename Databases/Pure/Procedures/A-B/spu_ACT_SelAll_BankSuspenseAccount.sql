SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_BankSuspenseAccount'
GO


CREATE PROCEDURE spu_ACT_SelAll_BankSuspenseAccount
AS
/*select the Bank Suspense Accounts*/

DECLARE @node_id INT

SELECT  
     @node_id = STT.parent_node_id
FROM StructureTree  STT
JOIN Element ELE 
     ON ELE.element_id = STT.element_id  
     AND ELE.element_name = 'BANKSUSP'
   
EXEC spu_ACT_Select_StructChildren1 @node_id = @node_id

GO


