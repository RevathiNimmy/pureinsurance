SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO




EXECUTE DDLDropProcedure 'spu_ACT_Spoke_Get_EventTypeIDFromCode'
GO


CREATE PROCEDURE spu_ACT_Spoke_Get_EventTypeIDFromCode
    @eventtypecode varchar(30)
AS
select event_type_id from event_type where upper(code) = upper(@eventtypecode)
GO
