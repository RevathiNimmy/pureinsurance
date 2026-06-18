SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_all_chasers'
GO

CREATE PROCEDURE spu_get_all_chasers
AS
    SELECT description from PMWrk_Task_Instance_Temp

GO

