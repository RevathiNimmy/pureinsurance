SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_EventId_FromEventCode'
GO

CREATE PROCEDURE spu_ACT_Get_EventId_FromEventCode
    @eventcode varchar(10)

AS
	SELECT ET.event_type_id
        FROM Event_Type ET	
        WHERE ET.code=@eventcode

GO
