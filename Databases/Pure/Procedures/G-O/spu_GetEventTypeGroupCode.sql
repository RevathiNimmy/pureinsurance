
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_GetEventTypeGroupCode'
GO

Create Procedure spu_GetEventTypeGroupCode  
    @Code varchar(10)  
AS  
    Select ETG.Code  
    From Event_Type ET  
    JOIN event_type_Group ETG   
    ON ET.Event_Type_Group_Id = ETG.Event_Type_Group_Id  
    Where ET.code=@Code  


  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
