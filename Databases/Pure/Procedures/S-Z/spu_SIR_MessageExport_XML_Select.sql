SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_MessageExport_XML_Select'
GO

CREATE PROCEDURE spu_SIR_MessageExport_XML_Select
    @Batch_id int,
    @event_type_code varchar(50),
    @new_batch  SMALLINT = 0 
AS 
  
    If (Select export_date From batch Where batch_id = @batch_id) Is Null  
    Begin  
        Update  batch  
        Set     export_date = GetDate()  
        Where   batch_id = @batch_id  
    End  
    Else  
    Begin  
        Update  batch  
        Set     reexport_date = GetDate()  
        Where   batch_id = @batch_id  
    End  
  
IF @new_batch = 1  
BEGIN  
    Update Event_log Set batch_id =  @Batch_id  
    Where event_type_id =(Select event_type_id from Event_type Where Code = @event_type_code)  
    AND batch_id IS NULL  
END  
  
SELECT  1                As Tag,  
    Null             As Parent,  
    'http://www.siriusfs.com/SFI/Export/Payment_Export/20071220' As [EXPORT_HEADER!1!xmlns],  
    'http://www.w3.org/2001/XMLSchema-instance'      As [EXPORT_HEADER!1!xmlns:xsi],  
    'http://www.siriusfs.com/SFI/Export/Payment_Export/20071220 Message_Export.xsd'     As [EXPORT_HEADER!1!xsi:schemaLocation],  
    GetDate()        As [EXPORT_HEADER!1!date_exported],  
    'Message_Export' As [EXPORT_HEADER!1!interface_name],  
    @batch_id             As [EXPORT_HEADER!1!batch_id],  
    b.batch_ref             As [EXPORT_HEADER!1!batch_reference], 
    NULL             As [MESSAGE!2!event_log_id],  
    NULL             As [MESSAGE!2!event_log_type_code],  
    NULL             As [MESSAGE!2!event_log_type_Group_code],  
    NULL             As [MESSAGE!2!destination],  
    NULL             As [MESSAGE!2!message_text]  
FROM    batch b 
    WHERE   (ISNULL(b.batch_id,0) <> 0 and b.batch_id = @batch_id)  
      
UNION ALL  
  
SELECT 2          As Tag,  
    1             As Parent,  
    'http://www.siriusfs.com/SFI/Export/Payment_Export/20071220' As [EXPORT_HEADER!1!xmlns],  
    'http://www.w3.org/2001/XMLSchema-instance'      As [EXPORT_HEADER!1!xmlns:xsi],  
    'http://www.siriusfs.com/SFI/Export/Payment_Export/20071220 Message_Export.xsd'     As [EXPORT_HEADER!1!xsi:schemaLocation],  
    GetDate()        As [EXPORT_HEADER!1!date_exported],  
    'Message_Export' As [EXPORT_HEADER!1!interface_name], 
    @batch_id             As [EXPORT_HEADER!1!batch_id],  
    b.batch_ref             As [EXPORT_HEADER!1!batch_reference],  
    Event_cnt As [MESSAGE!2!event_log_id],  
    ET.Code   As [MESSAGE!2!event_log_type_code],  
    ETG.Code As [MESSAGE!2!event_log_type_Group_code],  
    EL.Short_Description   As [MESSAGE!2!destination],  
    EL.Description As [MESSAGE!2!message_text]  
  
    From Event_Log EL  
    Join Event_type ET  
    ON EL.Event_Type_id = ET.Event_type_ID    
    JOIN Event_type_Group ETG  
    ON ET.Event_type_Group_id = ETG.Event_type_Group_id  
    JOIN Batch b 
    ON EL.Batch_id = b.batch_id
      
    Where EL.Batch_id =@Batch_ID  
  
FOR XML EXPLICIT  
  
SET QUOTED_IDENTIFIER OFF  
GO
SET ANSI_NULLS ON
GO
