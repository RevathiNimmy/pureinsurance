SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_Client_Online_Access'
GO

CREATE PROCEDURE spu_SAM_Get_Client_Online_Access
    @source_id int,
    @after_date datetime

AS
BEGIN

        SELECT p.party_cnt,
               c.number,
               pnd.online_status,
               p.resolved_name
          FROM party_net_data pnd
    INNER JOIN party p ON pnd.party_cnt = p.party_cnt
     LEFT JOIN contact c on pnd.contact_cnt = c.contact_cnt
         WHERE p.source_id = @source_id
           AND pnd.online_status_updated > @after_date
END
GO

