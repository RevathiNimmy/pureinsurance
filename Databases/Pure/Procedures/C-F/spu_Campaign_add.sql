SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Campaign_add'
GO


CREATE PROCEDURE spu_Campaign_add
    @campaign_id smallint,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @campaign_date datetime
AS


/* Insert the values */
INSERT INTO Campaign
( campaign_id, caption_id, code, description, is_deleted, effective_date, campaign_date )
VALUES
( @campaign_id, @caption_id, @code, @description, @is_deleted, @effective_date, @campaign_date )
GO


