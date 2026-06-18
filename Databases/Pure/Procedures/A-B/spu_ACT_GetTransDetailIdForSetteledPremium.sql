SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_ACT_GetTransDetailIdForSetteledPremium'
GO
CREATE PROCEDURE spu_ACT_GetTransDetailIdForSetteledPremium  
@transdetail_id int  
AS  
SELECT TD.transdetail_id  
FROM TransDetail TD 
INNER JOIN Transdetail_Type TT ON TD.transdetail_type_id = TT.transdetail_type_id    
INNER JOIN (SELECT DISTINCT TDetail1.document_id FROM TransDetail TDetail1   
  INNER JOIN Transdetail_Type TType1  ON TDetail1.transdetail_type_id = TType1.transdetail_type_id  
  WHERE TDetail1.outstanding_account_amount = 0 AND TType1.code = 'GROSS') TD1 ON TD.document_id = TD1.document_id    
INNER JOIN (SELECT DISTINCT TDetail2.document_id FROM TransDetail TDetail2  
  INNER JOIN Transdetail_Type TType2  ON TDetail2.transdetail_type_id = TType2.transdetail_type_id 
  WHERE TDetail2.outstanding_account_amount = 0 AND TType2.code = 'TAX') TD2 ON TD.document_id = TD2.document_id 
WHERE  TD.outstanding_account_amount <> 0 AND TT.CODE in('COMM','COMMTAX') AND TD.transdetail_id=@transdetail_id  
GO