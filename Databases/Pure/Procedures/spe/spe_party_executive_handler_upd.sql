

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_executive_handler_upd'
GO

CREATE PROCEDURE spe_party_executive_handler_upd
    @party_cnt int,
    @forename varchar(60),
    @initials varchar(20),
    @department_id int,
    @party_title_code varchar(70),
    @commission_cnt int=NULL
AS
BEGIN
UPDATE party_handler
    SET
    forename=@forename,
    initials=@initials,
    department_id=@department_id,
    party_title_code=@party_title_code,
    commission_cnt=@commission_cnt
WHERE party_cnt = @party_cnt
END
GO
