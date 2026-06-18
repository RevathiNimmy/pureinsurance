SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_consultant'
GO


CREATE PROCEDURE spu_wp_consultant
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

/* Stored proc to take a partycnt and from that ascertain the consultant for the client.
Returns consultant details */
	SELECT	p2.name consultant_surname,
			p2.resolved_name consultant_name,
			pc.party_title_code consultant_title,
			pc.forename consultant_forename,
			pc.initials consultant_initials,
			(select d.description from department d where d.department_id=pc.department_id) consultant_department
	FROM    	party p,
			party p2,
			party_consultant pc
	WHERE	p.party_cnt=@PartyCnt
			AND p2.party_cnt = p.consultant_cnt
			AND pc.party_cnt = p.consultant_cnt
GO


