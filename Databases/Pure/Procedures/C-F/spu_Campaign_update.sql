SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Campaign_update'
GO


CREATE PROCEDURE spu_Campaign_update
    @campaign_id smallint,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @campaign_date datetime
AS


/* Update the values */
UPDATE  Campaign

SET caption_id = @caption_id,
    code = @code,
    description = @description,
    is_deleted = @is_deleted,
    effective_date = @effective_date,
    campaign_date = @campaign_date
WHERE   campaign_id = @campaign_id
GO


