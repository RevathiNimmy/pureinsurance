SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_party_agent_type_update'
GO

CREATE PROCEDURE spu_party_agent_type_update
    @party_agent_type_id smallint,
    @caption_id smallint,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @is_visible tinyint
AS
/* Update the values */
UPDATE  Party_Agent_Type
SET caption_id = @caption_id,
    code = @code,
    description = @description,
    is_deleted = @is_deleted,
    effective_date = @effective_date,
    is_visible = @is_visible
WHERE   party_agent_type_id = @party_agent_type_id
GO


