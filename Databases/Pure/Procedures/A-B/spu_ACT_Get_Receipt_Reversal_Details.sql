SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_Receipt_Reversal_Details'
GO

CREATE PROCEDURE spu_ACT_Get_Receipt_Reversal_Details  
	@cashlistitemid int  
AS  
	select  
		cli.transdetail_id,  
		clirt.code, 
		clirt.is_instalment
	from  
		cashlistitem cli,  
		cashlistitem_receipt_type clirt  
	where  
		cli.cashlistitem_receipt_type_id = clirt.cashlistitem_receipt_type_id  
		and  
		cli.cashlistitem_id = @cashlistitemid  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
