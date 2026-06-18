SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_DOC_DocumentExport_XML_Select'
GO


CREATE PROCEDURE [dbo].[spu_DOC_DocumentExport_XML_Select]
        @start_date DATETIME = NULL,
        @end_date   DATETIME = NULL,
        @batch_id   INT,
        @newbatch   INT
AS
        DECLARE @parameters                 VARCHAR (300)
        DECLARE @batch_ref                  VARCHAR(20)
        DECLARE @batch_type_id              INT
        DECLARE @BatchStatus_Batch_Exported INT
        SET @parameters = 'start_date=' + CONVERT(VARCHAR, @start_date) + ' end_date=' + CONVERT(VARCHAR, @end_date)
        -- get required id constants
        SELECT @batch_type_id = batch_type_id
        FROM   batch_type
        WHERE  code       ='DOC'
        SELECT @batch_ref = batch_ref
        FROM   batch
        WHERE  batch_id                    = @batch_id
        SELECT @BatchStatus_Batch_Exported = batchstatus_id
        FROM   BatchStatus
        WHERE  code   = 'BE'
        IF @newbatch = 1
        BEGIN
                UPDATE doc_document 
                SET    batch_id     = @batch_id
                WHERE  
					  (
							 --@batch_id IS NULL
							 batch_id IS NULL
					  AND
							 (
								@start_date IS NULL
							 OR CAST(CONVERT(VARCHAR(10),create_date,111) AS DATETIME)>=CAST(CONVERT(VARCHAR(10),@start_date,111) AS DATETIME)
							 )
					  AND
							 (
								@end_date IS NULL
							 OR CAST(CONVERT(VARCHAR(10),create_date,111) AS DATETIME)<=CAST(CONVERT(VARCHAR(10),@end_date,111) AS DATETIME)
							 )
					  )                       
        END
        UPDATE batch
        SET    batchstatus_id =@BatchStatus_Batch_Exported
        WHERE  batch_id       = @batch_id
        SELECT 1                                                                                 AS Tag                                   ,
               NULL                                                                              AS Parent                                ,
               'http://www.siriusfs.com/SFI/Export/document_Export/20091101'                     AS [EXPORT_HEADER!1!xmlns]               ,
               'http://www.w3.org/2001/XMLSchema-instance'                                       AS [EXPORT_HEADER!1!xmlns:xsi]           ,
               'http://www.siriusfs.com/SFI/Export/document_Export/20091101
Document_Export.xsd'																			 AS [EXPORT_HEADER!1!xsi:schemaLocation]  ,  
               GETDATE()                                                                         AS [EXPORT_HEADER!1!date_exported]       ,
               'DOCUMENT_EXPORT'                                                                 AS [EXPORT_HEADER!1!interface_name]      ,
               @parameters                                                                       AS [EXPORT_HEADER!1!parameters_used]     ,
               @batch_id                                                                         AS [EXPORT_HEADER!1!batch_id]            ,
               b.batch_ref                                                                       AS [EXPORT_HEADER!1!batch_reference]     ,
			   b.total_transactions																 AS [EXPORT_HEADER!1!total_transactions]  ,  
               b.total_amount																	 AS [EXPORT_HEADER!1!total_amount]        ,  
               NULL                                                                              AS [document!2!document_key]             ,
               NULL                                                                              AS [document!2!document_description]     ,
               NULL                                                                              AS [document!2!document_type_code]       ,
               NULL                                                                              AS [document!2!document_type_description],
               NULL                                                                              AS [document!2!date_created]             ,
               NULL                                                                              AS [document!2!document_template_code]   ,
               NULL                                                                              AS [document!2!file_path]                ,
               NULL                                                                              AS [document!2!insurance_ref]            ,
               NULL                                                                              AS [document!2!insurance_folder_key]     ,
			   NULL                                                                              AS [document!2!party_code]               ,  
               NULL                                                                              AS [document!2!party_key]
        FROM   batch b
        WHERE  b.batch_id = @batch_id
        
        UNION ALL
        
        SELECT   2                                                                                  AS Tag                                    ,
                 1                                                                                  AS Parent                                 ,
                 'http://www.siriusfs.com/SFI/Export/document_Export/20091101'                      AS [EXPORT_HEADER!1!xmlns]                ,
                 'http://www.w3.org/2001/XMLSchema-instance'                                        AS [EXPORT_HEADER!1!xmlns:xsi]            ,
				 'http://www.siriusfs.com/SFI/Export/document_Export/20091101
Document_Export.xsd'																			    AS [EXPORT_HEADER!1!xsi:schemaLocation]   ,  
                 GETDATE()                                                                          AS [EXPORT_HEADER!1!date_exported]        ,
                 'DOCUMENT_EXPORT'                                                                  AS [EXPORT_HEADER!1!interface_name]       ,
                 @parameters                                                                        AS [EXPORT_HEADER!1!parameters_used]      ,
                 @batch_id                                                                          AS [EXPORT_HEADER!1!batch_id]             ,
				 b.batch_ref																		AS [EXPORT_HEADER!1!batch_reference]      ,  
                 b.total_transactions																AS [EXPORT_HEADER!1!total_transactions]   ,  
                 b.total_amount 																	AS [EXPORT_HEADER!1!total_amount]         ,
                 dd.doc_num                                                                         AS [document!2!document_key]              ,
                 LTRIM(RTRIM(dd.doc_name))                                                          AS [document!2!document_description]      ,
                 case dd.doc_type   when 'S' then 'XML' else dd.doc_type    end                                                                        AS [document!2!document_type_code]        ,
                 LTRIM(RTRIM(dp.page_type))                                                         AS [document!2!document_type_description] ,
                 dd.create_date                                                                     AS [document!2!date_created]              ,
                 dt.code                                                                            AS [document!2!document_template_code]    ,
                 LTRIM(RTRIM(ddv.server_unc)) + LTRIM(RTRIM(ddv.share_name)) + LTRIM(RTRIM(dv.directory)) + LTRIM(RTRIM(dp.page_name)) + '.' + LTRIM(RTRIM(dp.page_type)) AS [document!2!file_path],
                 LTRIM(RTRIM(df1.folder_name))                                                      AS [document!2!insurance_ref]             ,
                 df1.ex_code                                                                        AS [document!2!insurance_folder_key]      ,
                 df2.folder_name                                                                    AS [document!2!party_code]                ,
                 df2.ex_code                                                                        AS [document!2!party_key]
        FROM     Batch b
				 LEFT OUTER JOIN doc_document dd
				 ON b.batch_id=dd.batch_id
                 LEFT OUTER JOIN doc_folder AS df1
                 ON       dd.folder_num = df1.folder_num
                 LEFT OUTER JOIN doc_folder AS df2
                 ON       df1.parent_num = df2.folder_num
                 LEFT OUTER JOIN document_template dt
                 ON       dd.document_template_id = dt.document_template_id
                 LEFT OUTER JOIN doc_page dp
                 ON       dp.doc_num = dd.doc_num
                 LEFT OUTER JOIN doc_volume dv
                 ON       dv.volume_id = dp.volume_id
                 LEFT OUTER JOIN doc_device ddv
                 ON       ddv.device_id = dv.device_id
        WHERE    
				 dd.batch_id          = @batch_id

FOR XML EXPLICIT

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

 
