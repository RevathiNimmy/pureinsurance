SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Peril_del'
GO

CREATE PROCEDURE spe_Peril_del
    @insurance_file_cnt int,
    @risk_id int,
    @rating_section_id int,
    @peril_id int
AS

DELETE FROM Peril

WHERE   risk_cnt = @risk_id
AND rating_section_id = @rating_section_id
AND peril_id = @peril_id

GO

