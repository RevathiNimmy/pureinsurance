SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GetStatsCurrencyXRate'
GO
 
CREATE PROCEDURE spu_GetStatsCurrencyXRate  
   @DocumentRef VARCHAR(25) 
AS  
       SELECT currency_rate FROM stats_detail sd JOIN stats_folder sf 
            ON sf.stats_folder_cnt= sd.stats_folder_cnt 
			WHERE loss_id =(SELECT TOP 1 loss_id FROM Stats_Folder WHERE document_ref= @DocumentRef )
			AND sf.document_ref NOT LIKE 'CLC%' AND sf.document_ref NOT LIKE 'CLD%'  
            AND sd.stats_detail_type='GRS'
                            
                            