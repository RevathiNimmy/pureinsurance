SET QUOTED_IDENTIfIER OFF SET ANSI_NullS On
GO


Execute DDLDropProcedure 'spu_Get_PMNav_Batch_Transaction_Details'
GO
CREATE PROCEDURE spu_Get_PMNav_Batch_Transaction_Details 
		(@Batch_set_id as INT) 
AS
	SELECT 
		t.transdetail_id,
		t.document_id
	FROM 
		transdetail t 
	INNER JOIN 
		PMNav_Batch_Key_Value p On p.key_value = t.transdetail_id
	WHERE 
		p.pmnav_batch_set_id =@Batch_set_id and  t.spare = 'WRITEOFF'  
GO  