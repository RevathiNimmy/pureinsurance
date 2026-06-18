

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_executive_handler_add'
GO

CREATE PROCEDURE spe_party_executive_handler_add
    @party_cnt int,
    @forename varchar(60),
    @initials varchar(20),
    @department_id int,
    @party_title_code varchar(70),
    @commission_cnt int=NULL
AS
BEGIN
INSERT INTO party_handler (
    party_cnt ,
    forename ,
    initials ,
    department_id ,
    party_title_code ,
    commission_cnt)
VALUES (
    @party_cnt,
    @forename,
    @initials,
    @department_id,
    @party_title_code,
    @commission_cnt)
END

GO
